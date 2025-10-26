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

        public void Process(DecisionSnapshot snapshot, IBarDataProvider barData, CoreEngine coreEngine, int currentBar, double accountSize)
        {
            if (snapshot == null)
                throw new ArgumentNullException(nameof(snapshot));
            if (barData == null)
                throw new ArgumentNullException(nameof(barData));
            if (coreEngine == null)
                throw new ArgumentNullException(nameof(coreEngine));

            _logger.Debug("[StructureFusion] Fusionando estructuras en HeatZones (Fusión Jerárquica)...");

            // 1. Separar estructuras por categoría: Anchors (TF alto) y Triggers (TF bajo)
            var anchors = new List<StructureBase>();  // TFs altos: 240m, 1440m (Contexto)
            var triggers = new List<StructureBase>(); // TFs bajos: 5m, 15m, 60m (Entrada)
            
            foreach (int tf in _config.TimeframesToUse)
            {
                var structures = coreEngine.GetAllStructures(tf)
                    .Where(s => s.IsActive && s.Score >= _config.HeatZone_MinScore)
                    .ToList();
                
                // Clasificar: TFs >= 240m son Anchors, TFs < 240m son Triggers
                if (tf >= 240)
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

            _logger.Debug(string.Format("[StructureFusion] Anchors (TF alto): {0}, Triggers (TF bajo): {1}", 
                anchors.Count, triggers.Count));

            // 2. Obtener ATR del timeframe más bajo para tolerancia de overlap
            int lowestTF = _config.TimeframesToUse.Min();
            double atr = barData.GetATR(lowestTF, currentBar, 14);
            double overlapTolerance = _config.HeatZone_OverlapToleranceATR * atr;

            _logger.Debug(string.Format("[StructureFusion] ATR({0}): {1:F2}, OverlapTolerance: {2:F2}",
                lowestTF, atr, overlapTolerance));

            // 3. Fusión Jerárquica: Para cada Trigger, buscar Anchors que lo contengan o solapen
            var heatZones = new List<HeatZone>();
            var assignedTriggers = new HashSet<string>();

            // Ordenar Triggers por score descendente (mejores primero)
            triggers = triggers.OrderByDescending(s => s.Score).ToList();

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
                    _logger.Debug(string.Format("[StructureFusion] Trigger {0} no tiene confluencia suficiente ({1} < {2})",
                        trigger.Id, allStructures.Count, _config.HeatZone_MinConfluence));
                    continue;
                }

                // Crear HeatZone usando SOLO las dimensiones del Trigger (no envolvente total)
                var heatZone = CreateHierarchicalHeatZone(trigger, nearbyTriggers, supportingAnchors, barData, currentBar);
                heatZones.Add(heatZone);

                // Marcar Triggers como asignados (NO los Anchors, pueden reutilizarse)
                assignedTriggers.Add(trigger.Id);
                foreach (var t in nearbyTriggers)
                    assignedTriggers.Add(t.Id);

                _logger.Debug(string.Format("[StructureFusion] HeatZone jerárquica creada: {0} (Trigger TF:{1}, {2} Anchors, Score: {3:F2}, Height: {4:F2})",
                    heatZone.Id, trigger.TF, supportingAnchors.Count, heatZone.Score, heatZone.High - heatZone.Low));
            }

            snapshot.HeatZones = heatZones;

            _logger.Debug(string.Format("[StructureFusion] Fusión jerárquica completada: {0} HeatZones creadas", heatZones.Count));
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
            StructureBase triggerMain, 
            List<StructureBase> triggersSame, 
            List<StructureBase> anchors, 
            IBarDataProvider barData, 
            int currentBar)
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

            // 3. DIRECTION: Basada en el Trigger principal
            heatZone.Direction = GetStructureDirection(triggerMain);

            // 4. Estructura dominante: El Trigger principal
            heatZone.DominantStructureId = triggerMain.Id;
            heatZone.DominantType = triggerMain.GetType().Name;
            heatZone.TFDominante = triggerMain.TF;

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

            // 4. Identificar estructura dominante (mayor score)
            var dominantStructure = structures.OrderByDescending(s => s.Score).First();
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

