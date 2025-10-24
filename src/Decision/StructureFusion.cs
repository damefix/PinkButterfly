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

            _logger.Debug("[StructureFusion] Fusionando estructuras en HeatZones...");

            // 1. Obtener todas las estructuras activas con score suficiente (de todos los TFs)
            var allStructures = new List<StructureBase>();
            foreach (int tf in _config.TimeframesToUse)
            {
                allStructures.AddRange(coreEngine.GetAllStructures(tf));
            }
            
            allStructures = allStructures
                .Where(s => s.IsActive && s.Score >= _config.HeatZone_MinScore)
                .OrderByDescending(s => s.Score)
                .ToList();

            if (allStructures.Count == 0)
            {
                _logger.Debug("[StructureFusion] No hay estructuras activas con score suficiente");
                snapshot.HeatZones = new List<HeatZone>();
                return;
            }

            _logger.Debug(string.Format("[StructureFusion] Procesando {0} estructuras activas", allStructures.Count));

            // 2. Obtener ATR del timeframe principal para calcular tolerancia de overlap
            int primaryTF = _config.TimeframesToUse.OrderByDescending(tf => tf).FirstOrDefault();
            if (primaryTF == 0)
                primaryTF = 60; // Fallback a 1H

            double atr = barData.GetATR(primaryTF, currentBar, 14);
            double overlapTolerance = _config.HeatZone_OverlapToleranceATR * atr;

            _logger.Debug(string.Format("[StructureFusion] ATR({0}): {1:F2}, OverlapTolerance: {2:F2}",
                primaryTF, atr, overlapTolerance));

            // 3. Fusionar estructuras en HeatZones
            var heatZones = new List<HeatZone>();
            var assignedStructures = new HashSet<string>(); // IDs de estructuras ya asignadas

            foreach (var structure in allStructures)
            {
                // Si ya está asignada, saltar
                if (assignedStructures.Contains(structure.Id))
                    continue;

                // Calcular rango de búsqueda: [Low - tolerance, High + tolerance]
                double searchLow = structure.Low - overlapTolerance;
                double searchHigh = structure.High + overlapTolerance;

                // Buscar estructuras cercanas usando QueryOverlappingStructures (eficiente O(log n + k))
                var nearbyStructures = coreEngine.QueryOverlappingStructures(structure.TF, searchLow, searchHigh)
                    .Where(s => s.IsActive && s.Score >= _config.HeatZone_MinScore && !assignedStructures.Contains(s.Id))
                    .ToList();

                // Verificar si hay confluencia suficiente (incluyendo la estructura actual)
                if (nearbyStructures.Count < _config.HeatZone_MinConfluence)
                {
                    _logger.Debug(string.Format("[StructureFusion] Estructura {0} no tiene confluencia suficiente ({1} < {2})",
                        structure.Id, nearbyStructures.Count, _config.HeatZone_MinConfluence));
                    continue;
                }

                // Crear HeatZone
                var heatZone = CreateHeatZone(nearbyStructures, barData, currentBar);
                heatZones.Add(heatZone);

                // Marcar estructuras como asignadas
                foreach (var s in nearbyStructures)
                    assignedStructures.Add(s.Id);

                _logger.Debug(string.Format("[StructureFusion] HeatZone creada: {0} ({1} estructuras, Score: {2:F2}, Direction: {3})",
                    heatZone.Id, heatZone.ConfluenceCount, heatZone.Score, heatZone.Direction));
            }

            snapshot.HeatZones = heatZones;

            _logger.Debug(string.Format("[StructureFusion] Fusión completada: {0} HeatZones creadas", heatZones.Count));
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

