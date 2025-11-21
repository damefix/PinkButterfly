// ============================================================================
// StructureFusion.cs
// PinkButterfly CoreBrain - Componente 2 del DFM
// 
// Responsabilidades:
// - Obtener todas las estructuras activas del CoreEngine
// - Fusionar estructuras que se solapan dentro de HeatZone_OverlapToleranceATR
// - Crear HeatZone con mínimo HeatZone_MinConfluence estructuras
// - Calcular DominantType, TFDominante, DominantStructureId (estructura con mayor score)
// - Calcular Direction de la zona (suma ponderada de scores bullish vs bearish)
// - Calcular Score agregado de la zona (media ponderada por TFWeight)
//
// Algoritmo:
//   1. Filtrar estructuras con Score >= HeatZone_MinScore
//   2. Ordenar por Score descendente
//   3. Para cada estructura no asignada:
//      - Buscar estructuras cercanas usando QueryStructuresByPriceRange (O(log n + k))
//      - Si hay >= MinConfluence estructuras cercanas: crear HeatZone
//   4. Calcular propiedades de cada HeatZone (Score ponderado, Direction ponderada)
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// StructureFusion: Segundo componente del pipeline DFM
    /// Fusiona estructuras solapadas en HeatZones con confluencia
    /// </summary>
    public class StructureFusion : IDecisionComponent
    {
        private EngineConfig _config;
        private ILogger _logger;

        public string ComponentName => "StructureFusion";

        public void Initialize(EngineConfig config, ILogger logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug("[StructureFusion] Inicializado");
        }

        public void Process(DecisionSnapshot snapshot, IBarDataProvider barData, CoreEngine coreEngine, int currentBar, int timeframeMinutes, double accountSize)
        {
            if (snapshot == null)
                throw new ArgumentNullException(nameof(snapshot));
            if (barData == null)
                throw new ArgumentNullException(nameof(barData));
            if (coreEngine == null)
                throw new ArgumentNullException(nameof(coreEngine));

            if (_config.EnablePerfDiagnostics)
                _logger.Debug("[StructureFusion] Fusionando estructuras en HeatZones (Fusión Jerárquica)...");

            // 1. Separar estructuras por categoría: Anchors (TF alto) y Triggers (TF bajo)
            var anchors = new List<StructureBase>();  // TFs altos: 60m, 240m, 1440m (Contexto)
            var triggers = new List<StructureBase>(); // TFs bajos: 5m, 15m (Entrada)
            
            foreach (int tf in _config.TimeframesToUse)
            {
                var allStructures = coreEngine.GetAllStructures(tf).Where(s => s.IsActive).ToList();
                var rejectedStructures = allStructures.Where(s => s.Score < _config.HeatZone_MinScore).ToList();
                var structures = allStructures.Where(s => s.Score >= _config.HeatZone_MinScore).ToList();
                
                int rejected = rejectedStructures.Count;
                
                if (_config.EnablePerfDiagnostics && allStructures.Count > 0)
                {
                    var avgScore = allStructures.Average(s => s.Score);
                    var maxScore = allStructures.Max(s => s.Score);
                    _logger.Info($"[FUSION][TF_FILTER] TF={tf} Total={allStructures.Count} Passed={structures.Count} Rejected={rejected} (Score<{_config.HeatZone_MinScore:F2}) AvgScore={avgScore:F2} MaxScore={maxScore:F2}");
                    
                    // Log detallado de primeras 3 estructuras rechazadas de TF altos (60m+)
                    if (tf >= 60 && rejected > 0)
                    {
                        // Convertir currentBar (TF decisión) a índice del TF de la estructura
                        DateTime currentTime = barData.GetBarTime(timeframeMinutes, currentBar);
                        int currentBarInStructureTF = barData.GetBarIndexFromTime(tf, currentTime);
                        
                        foreach (var s in rejectedStructures.Take(3))
                        {
                            int ageBars = currentBarInStructureTF - s.CreatedAtBarIndex;
                            int ageMinutes = ageBars * tf;
                            int ageDays = ageMinutes / 1440;
                            _logger.Info($"  └─ REJECTED: Type={s.Type} Score={s.Score:F3} Age={ageBars}bars_TF{tf} ({ageDays}días) TouchBody={s.TouchCount_Body} Active={s.IsActive}");
                        }
                    }
                }
                
                // Clasificar: TFs >= 60m son Anchors, TFs < 60m son Triggers
                if (tf >= 60)
                    anchors.AddRange(structures);
                else
                    triggers.AddRange(structures);
            }

            if (triggers.Count == 0)
            {
                _logger.Debug("[StructureFusion] No hay estructuras Trigger (TF bajo) con score suficiente");
                snapshot.HeatZones = new List<HeatZone>();
                return;
            }

            if (_config.EnablePerfDiagnostics)
                _logger.Debug(string.Format("[StructureFusion] Anchors (TF alto): {0}, Triggers (TF bajo): {1}", 
                    anchors.Count, triggers.Count));

            // 2. Obtener ATR del timeframe más bajo para tolerancia de overlap (alineado por tiempo)
            int lowestTF = _config.TimeframesToUse.Min();
            DateTime analysisTime = barData.GetBarTime(timeframeMinutes, currentBar);
            int idxLowest = barData.GetBarIndexFromTime(lowestTF, analysisTime);
            double atr;
            if (idxLowest < 0)
            {
                _logger.Info($"[CTX_NO_DATA] StructureFusion: TF={lowestTF} sin índice alineado para {analysisTime:yyyy-MM-dd HH:mm}. ATR=1.0");
                atr = 1.0;
            }
            else
            {
                atr = barData.GetATR(lowestTF, 14, idxLowest);
            }
            double overlapTolerance = _config.HeatZone_OverlapToleranceATR * atr;

            _logger.Debug(string.Format("[StructureFusion] ATR({0}): {1:F2}, OverlapTolerance: {2:F2}",
                lowestTF, atr, overlapTolerance));

            // 3. Fusión Jerárquica: Para cada Trigger, buscar Anchors que lo contengan o solapen
            var heatZones = new List<HeatZone>();
            var assignedTriggers = new HashSet<string>();

            // NO ordenar - preservar secuencia temporal de inserción (Fase 1)
            // triggers ya vienen ordenados temporalmente desde AddRange(structures)

            int sfBull = 0, sfBear = 0, sfNeutral = 0, sfWithAnchors = 0;
            foreach (var trigger in triggers)
            {
                // Si ya está asignado, saltar
                if (assignedTriggers.Contains(trigger.Id))
                    continue;

                // Buscar Anchors que contengan o solapen con este Trigger
                var supportingAnchors = anchors
                    .Where(anchor => IsOverlapping(trigger, anchor, overlapTolerance))
                    .ToList();

                // Buscar otros Triggers cercanos del mismo TF (confluencia intra-TF)
                var nearbyTriggers = triggers
                    .Where(t => t.Id != trigger.Id && 
                                !assignedTriggers.Contains(t.Id) &&
                                t.TF == trigger.TF &&
                                IsOverlapping(trigger, t, overlapTolerance))
                    .ToList();

                // Combinar: Trigger principal + Triggers cercanos + Anchors
                var allStructures = new List<StructureBase> { trigger };
                allStructures.AddRange(nearbyTriggers);
                allStructures.AddRange(supportingAnchors);

                // Verificar confluencia mínima
                if (allStructures.Count < _config.HeatZone_MinConfluence)
                {
                if (_config.EnablePerfDiagnostics)
                    _logger.Debug(string.Format("[StructureFusion] Trigger {0} no tiene confluencia suficiente ({1} < {2})",
                            trigger.Id, allStructures.Count, _config.HeatZone_MinConfluence));
                    continue;
                }

                // Crear HeatZone usando SOLO las dimensiones del Trigger (no envolvente total)
                var heatZone = CreateHierarchicalHeatZone(snapshot, trigger, nearbyTriggers, supportingAnchors, barData, analysisTime, currentBar, atr);
                if (heatZone == null)
                    continue; // Zona descartada por tamaño excesivo
                
                heatZones.Add(heatZone);

                if (supportingAnchors.Count > 0) sfWithAnchors++;
                if (heatZone.Direction == "Bullish") sfBull++;
                else if (heatZone.Direction == "Bearish") sfBear++;
                else sfNeutral++;

                // Marcar Triggers como asignados (NO los Anchors, pueden reutilizarse)
                assignedTriggers.Add(trigger.Id);
                foreach (var t in nearbyTriggers)
                    assignedTriggers.Add(t.Id);

                if (_config.EnablePerfDiagnostics)
                    _logger.Debug(string.Format("[StructureFusion] HeatZone jerárquica creada: {0} (Trigger TF:{1}, {2} Anchors, Score: {3:F2}, Height: {4:F2})",
                        heatZone.Id, trigger.TF, supportingAnchors.Count, heatZone.Score, heatZone.High - heatZone.Low));
                // Diagnóstico por zona: lista de TFs trigger/anchor
                string tfTrigList = string.Join("/", new List<StructureBase> { trigger }.Concat(nearbyTriggers).Select(s=>s.TF).Distinct());
                string tfAncList = string.Join("/", supportingAnchors.Select(a=>a.TF).Distinct());
                _logger.Info(string.Format("[DIAGNOSTICO][StructureFusion] ZONA HZ={0} TFTrig={1} TFAng={2}", heatZone.Id, tfTrigList, tfAncList));
            }

            snapshot.HeatZones = heatZones;

            if (_config.EnablePerfDiagnostics)
                _logger.Debug(string.Format("[StructureFusion] Fusión jerárquica completada: {0} HeatZones creadas", heatZones.Count));
            _logger.Info(string.Format("[DIAGNOSTICO][StructureFusion] TotHZ={0} WithAnchors={1} DirBull={2} DirBear={3} DirNeutral={4}",
                heatZones.Count, sfWithAnchors, sfBull, sfBear, sfNeutral));

            // Resumen agregado del pipeline de StructureFusion cada N barras del TF de decisión
            if (timeframeMinutes == _config.DecisionTimeframeMinutes)
            {
                int interval = Math.Max(1, _config.DiagnosticsInterval);
                if ((currentBar % interval) == 0)
                {
                    _logger.Info(string.Format("[PIPE][SF] TF={0} Bar={1} Triggers={2} Anchors={3} HeatZones={4}",
                        timeframeMinutes, currentBar, triggers.Count, anchors.Count, heatZones.Count));
                }
            }
        }
        
        /// <summary>
        /// Verifica si dos estructuras se solapan (con tolerancia)
        /// </summary>
        private bool IsOverlapping(StructureBase s1, StructureBase s2, double tolerance)
        {
            // Expandir rangos con tolerancia
            double s1Low = s1.Low - tolerance;
            double s1High = s1.High + tolerance;
            double s2Low = s2.Low - tolerance;
            double s2High = s2.High + tolerance;
            
            // Verificar solapamiento
            return !(s1High < s2Low || s2High < s1Low);
        }
        
        /// <summary>
        /// Crea una HeatZone jerárquica: dimensiones del Trigger, score bonificado por Anchors
        /// </summary>
        private HeatZone CreateHierarchicalHeatZone(
            DecisionSnapshot snapshot,
            StructureBase triggerMain,
            List<StructureBase> triggersSame,
            List<StructureBase> anchors,
            IBarDataProvider barData,
            DateTime analysisTime,
            int currentBar,
            double atr)
        {
            var heatZone = new HeatZone
            {
                Id = "HZ_" + Guid.NewGuid().ToString().Substring(0, 8),
                SourceStructureIds = new List<string> { triggerMain.Id },
                ConfluenceCount = 1 + triggersSame.Count + anchors.Count
            };
            
            // Añadir IDs de todas las estructuras
            heatZone.SourceStructureIds.AddRange(triggersSame.Select(s => s.Id));
            heatZone.SourceStructureIds.AddRange(anchors.Select(s => s.Id));

            // 1. DIMENSIONES: Usar envolvente de los Triggers (TF bajo) SOLAMENTE
            var allTriggers = new List<StructureBase> { triggerMain };
            allTriggers.AddRange(triggersSame);
            
            heatZone.High = allTriggers.Max(s => s.High);
            heatZone.Low = allTriggers.Min(s => s.Low);

            // Validación de tamaño máximo de zona (V6.0c: evitar mega-zonas por fusión transitiva)
            double zoneSize = Math.Abs(heatZone.High - heatZone.Low);
            if (atr <= 0) atr = 1.0;
            double zoneSizeATR = zoneSize / atr;
            if (zoneSizeATR > _config.MaxZoneSizeATR)
            {
                // Cambio a Debug para evitar spam en logs (puede haber millones de zonas descartadas)
                _logger.Debug($"[StructureFusion] Zona {heatZone.Id} descartada por tamaño: {zoneSizeATR:F2} ATR (>{_config.MaxZoneSizeATR}). Rango={heatZone.Low:F2}-{heatZone.High:F2}");
                return null;
            }

            // 2. SCORE: Score del Trigger principal + Bono por Anchors
            double baseScore = triggerMain.Score;
            
            // Bono por Triggers cercanos del mismo TF (confluencia intra-TF)
            double intraBonus = triggersSame.Count * 0.10; // 10% por cada Trigger adicional
            
            // Bono por Anchors (contexto de TF alto)
            double anchorBonus = 0.0;
            foreach (var anchor in anchors)
            {
                // Bono proporcional al score del Anchor y su TF
                double tfWeight = _config.TFWeights.ContainsKey(anchor.TF) ? _config.TFWeights[anchor.TF] : 1.0;
                anchorBonus += anchor.Score * tfWeight * 0.20; // 20% del score ponderado del Anchor
            }
            
            // Score final = Base + Bonos (limitado a 1.0)
            heatZone.Score = Math.Min(1.0, baseScore + intraBonus + anchorBonus);

            // 3. DIRECTION: Confluencia ponderada Triggers + Anchors, con desempate por GlobalBias
            double bullDir = 0.0;
            double bearDir = 0.0;

            // Triggers: ponderar por score (TF bajo ya implícito)
            foreach (var t in allTriggers)
            {
                double weighted = t.Score;
                string dir = GetStructureDirection(t);
                if (dir == "Bullish") bullDir += weighted; else if (dir == "Bearish") bearDir += weighted;
            }

            // Anchors: ponderar por TFWeights y multiplicador de dirección
            foreach (var a in anchors)
            {
                double tfWeight = _config.TFWeights.ContainsKey(a.TF) ? _config.TFWeights[a.TF] : 1.0;
                double weighted = a.Score * tfWeight * _config.AnchorDirectionWeight;
                string dir = GetStructureDirection(a);
                if (dir == "Bullish") bullDir += weighted; else if (dir == "Bearish") bearDir += weighted;
            }

            string dirFinal;
            double margin = _config.DirectionTieMargin;
            bool bullWins = bullDir > bearDir * (1.0 + margin);
            bool bearWins = bearDir > bullDir * (1.0 + margin);
            if (bullWins) dirFinal = "Bullish";
            else if (bearWins) dirFinal = "Bearish";
            else dirFinal = snapshot.GlobalBiasStrength >= 0.7 ? snapshot.GlobalBias : "Neutral";

            heatZone.Direction = dirFinal;

            // Motivo de decisión direccional
            string reason;
            if (!bullWins && !bearWins)
                reason = "tie-bias";
            else if (anchors.Count > 0)
                reason = "anchors+triggers";
            else
                reason = "triggers-only";

            _logger.Info(string.Format("[DIAGNOSTICO][StructureFusion] HZ={0} Triggers={1} Anchors={2} BullDir={3:F3} BearDir={4:F3} → Dir={5} Reason={6} Bias={7}/{8:F2}",
                heatZone.Id, allTriggers.Count, anchors.Count, bullDir, bearDir, dirFinal, reason, snapshot.GlobalBias, snapshot.GlobalBiasStrength));

            // 4. Estructura dominante: Seleccionar mejor Trigger por Score × TFWeight (V5.7d)
            var dominantTrigger = allTriggers
                .Select(t => new {
                    Structure = t,
                    Weight = t.Score * (_config.TFWeights.ContainsKey(t.TF) ? _config.TFWeights[t.TF] : 1.0),
                    Age = Math.Max(0, (barData.GetBarIndexFromTime(t.TF, analysisTime) >= 0
                        ? barData.GetBarIndexFromTime(t.TF, analysisTime)
                        : int.MaxValue) - t.CreatedAtBarIndex)
                })
                .OrderByDescending(x => x.Weight)                       // Primero: mejor Score × TFWeight
                .ThenByDescending(x => x.Structure.TF)                  // Desempate: TF más alto
                .ThenBy(x => x.Age)                                     // Desempate: más fresco
                .ThenBy(x => x.Structure.CreatedAtBarIndex)             // Desempate: más antiguo
                .ThenBy(x => x.Structure.StartTime)                     // Desempate: por tiempo de inicio
                .ThenBy(x => x.Structure.Low)                           // Desempate: precio bajo
                .ThenBy(x => x.Structure.High)                          // Desempate: precio alto
                .First();

            heatZone.DominantStructureId = dominantTrigger.Structure.Id;
            heatZone.DominantType = dominantTrigger.Structure.GetType().Name;
            heatZone.TFDominante = dominantTrigger.Structure.TF;
            
            // Logging de trazabilidad
            _logger.Info(string.Format("[StructureFusion] HZ={0} DominantTrigger: Type={1} TF={2} Score={3:F2} Weight={4:F2} Age={5}",
                heatZone.Id, dominantTrigger.Structure.GetType().Name, dominantTrigger.Structure.TF, 
                dominantTrigger.Structure.Score, dominantTrigger.Weight, dominantTrigger.Age));

            // 5. Metadata adicional
            heatZone.Metadata = new Dictionary<string, object>
            {
                { "CreatedAtBar", currentBar },
                { "TriggerTF", triggerMain.TF },
                { "AnchorCount", anchors.Count },
                { "IntraTriggerCount", triggersSame.Count },
                { "BaseScore", baseScore },
                { "AnchorBonus", anchorBonus },
                { "IntraBonus", intraBonus },
                { "StructureTypes", string.Join(",", allTriggers.Select(s => s.GetType().Name).Distinct()) }
            };

            return heatZone;
        }

        /// <summary>
        /// Crea una HeatZone a partir de un conjunto de estructuras solapadas
        /// </summary>
        private HeatZone CreateHeatZone(List<StructureBase> structures, IBarDataProvider barData, int currentBar)
        {
            var heatZone = new HeatZone
            {
                Id = "HZ_" + Guid.NewGuid().ToString().Substring(0, 8),
                SourceStructureIds = structures.Select(s => s.Id).ToList(),
                ConfluenceCount = structures.Count
            };

            // 1. Calcular envolvente (High/Low)
            heatZone.High = structures.Max(s => s.High);
            heatZone.Low = structures.Min(s => s.Low);

            // 2. Calcular Score agregado (media ponderada por TFWeight)
            double sumWeightedScores = 0.0;
            double sumWeights = 0.0;

            foreach (var structure in structures)
            {
                double tfWeight = _config.TFWeights.ContainsKey(structure.TF)
                    ? _config.TFWeights[structure.TF]
                    : 1.0;

                sumWeightedScores += structure.Score * tfWeight;
                sumWeights += tfWeight;
            }

            heatZone.Score = sumWeights > 0 ? sumWeightedScores / sumWeights : 0.0;

            // 3. Calcular Direction (suma ponderada de scores bullish vs bearish)
            double bullishScore = 0.0;
            double bearishScore = 0.0;

            foreach (var structure in structures)
            {
                double tfWeight = _config.TFWeights.ContainsKey(structure.TF)
                    ? _config.TFWeights[structure.TF]
                    : 1.0;

                double weightedScore = structure.Score * tfWeight;

                string direction = GetStructureDirection(structure);
                if (direction == "Bullish")
                    bullishScore += weightedScore;
                else if (direction == "Bearish")
                    bearishScore += weightedScore;
            }

            // Direction basada en suma ponderada de scores
            if (bullishScore > bearishScore * 1.2) // 20% de margen para evitar ruido
                heatZone.Direction = "Bullish";
            else if (bearishScore > bullishScore * 1.2)
                heatZone.Direction = "Bearish";
            else
                heatZone.Direction = "Neutral";

            // 4. Identificar estructura dominante (mayor score) + desempates deterministas
            var dominantStructure = structures
                .OrderByDescending(s => s.Score)
                .ThenByDescending(s => s.TF)
                .ThenBy(s => s.CreatedAtBarIndex)
                .ThenBy(s => s.StartTime)
                .ThenBy(s => s.Low)
                .ThenBy(s => s.High)
                .First();
            heatZone.DominantStructureId = dominantStructure.Id;
            heatZone.DominantType = dominantStructure.GetType().Name;
            heatZone.TFDominante = dominantStructure.TF;

            // 5. Metadata adicional
            heatZone.Metadata = new Dictionary<string, object>
            {
                { "BullishScore", bullishScore },
                { "BearishScore", bearishScore },
                { "CreatedAtBar", currentBar },
                { "StructureTypes", string.Join(",", structures.Select(s => s.GetType().Name).Distinct()) }
            };

            return heatZone;
        }

        /// <summary>
        /// Obtiene la dirección de una estructura de forma segura (cast a tipos específicos)
        /// </summary>
        private string GetStructureDirection(StructureBase structure)
        {
            if (structure is FVGInfo fvg)
                return fvg.Direction;
            if (structure is StructureBreakInfo sb)
                return sb.Direction;
            if (structure is OrderBlockInfo ob)
                return ob.Direction;
            if (structure is LiquidityGrabInfo lg)
                return lg.DirectionalBias; // LG usa DirectionalBias, no Direction
            
            return "Neutral";
        }
    }
}

