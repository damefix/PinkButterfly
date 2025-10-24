// ============================================================================
// DecisionFusionModel.cs
// PinkButterfly CoreBrain - Componente 5 del DFM
// 
// Responsabilidades:
// - Evaluar todas las HeatZones con riesgo calculado
// - Aplicar la fórmula de scoring con pesos configurados
// - Calcular Confidence final para cada zona con explainability completa
// - Aplicar penalización por BiasOverrideConfidenceFactor si va contra el bias
// - Seleccionar la mejor zona (mayor Confidence)
// - Generar Action (BUY/SELL/WAIT) basado en thresholds
//
// Fórmula de Scoring (con explainability):
//   CoreScoreContribution = zone.Score * Weight_CoreScore
//   ProximityContribution = ProximityFactor * Weight_Proximity
//   ConfluenceContribution = min(1.0, ConfluenceCount/MaxConfluenceReference) * Weight_Confluence
//   TypeContribution = TypeMultiplier * Weight_Type
//   BiasContribution = BiasAlignment * Weight_Bias
//   MomentumContribution = MomentumFactor * Weight_Momentum
//   VolumeContribution = VolumeFactor * Weight_Volume
//   
//   RawConfidence = Sum(todas las contribuciones)
//   
//   Si Direction != GlobalBias:
//     RawConfidence *= BiasOverrideConfidenceFactor
//   
//   FinalConfidence = Clamp(RawConfidence, 0.0, 1.0)
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// DecisionFusionModel: Quinto componente del pipeline DFM
    /// Aplica la fórmula de scoring y genera la decisión final
    /// </summary>
    public class DecisionFusionModel : IDecisionComponent
    {
        private EngineConfig _config;
        private ILogger _logger;

        public string ComponentName => "DecisionFusionModel";

        public void Initialize(EngineConfig config, ILogger logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug("[DecisionFusionModel] Inicializado");
        }

        public void Process(DecisionSnapshot snapshot, IBarDataProvider barData, CoreEngine coreEngine, int currentBar, double accountSize)
        {
            if (snapshot == null)
                throw new ArgumentNullException(nameof(snapshot));

            _logger.Debug("[DecisionFusionModel] Evaluando HeatZones y generando decisión...");

            if (snapshot.HeatZones == null || snapshot.HeatZones.Count == 0)
            {
                _logger.Debug("[DecisionFusionModel] No hay HeatZones para evaluar");
                snapshot.Metadata["BestZone"] = null;
                snapshot.Metadata["BestConfidence"] = 0.0;
                return;
            }

            // Filtrar zonas con riesgo calculado
            var validZones = snapshot.HeatZones
                .Where(z => z.Metadata.ContainsKey("RiskCalculated") && (bool)z.Metadata["RiskCalculated"])
                .ToList();

            if (validZones.Count == 0)
            {
                _logger.Debug("[DecisionFusionModel] No hay HeatZones con riesgo calculado");
                snapshot.Metadata["BestZone"] = null;
                snapshot.Metadata["BestConfidence"] = 0.0;
                return;
            }

            _logger.Debug(string.Format("[DecisionFusionModel] Evaluando {0} HeatZones válidas", validZones.Count));

            // Evaluar cada zona y calcular Confidence
            HeatZone bestZone = null;
            double bestConfidence = 0.0;
            DecisionScoreBreakdown bestBreakdown = null;

            foreach (var zone in validZones)
            {
                var breakdown = CalculateConfidence(zone, snapshot, coreEngine, currentBar);
                
                // Almacenar breakdown en Metadata
                zone.Metadata["ConfidenceBreakdown"] = breakdown;
                zone.Metadata["FinalConfidence"] = breakdown.FinalConfidence;

                _logger.Debug(string.Format(
                    "[DecisionFusionModel] HeatZone {0}: Confidence={1:F3} (Core={2:F3}, Prox={3:F3}, Conf={4:F3}, Type={5:F3}, Bias={6:F3})",
                    zone.Id, breakdown.FinalConfidence, breakdown.CoreScoreContribution, breakdown.ProximityContribution,
                    breakdown.ConfluenceContribution, breakdown.TypeContribution, breakdown.BiasContribution
                ));

                // Actualizar mejor zona
                if (breakdown.FinalConfidence > bestConfidence)
                {
                    bestConfidence = breakdown.FinalConfidence;
                    bestZone = zone;
                    bestBreakdown = breakdown;
                }
            }

            // Almacenar mejor zona en snapshot
            snapshot.Metadata["BestZone"] = bestZone;
            snapshot.Metadata["BestConfidence"] = bestConfidence;
            snapshot.Metadata["BestBreakdown"] = bestBreakdown;

            if (bestZone != null)
            {
                _logger.Debug(string.Format(
                    "[DecisionFusionModel] Mejor zona: {0} ({1}), Confidence: {2:F3}",
                    bestZone.Id, bestZone.Direction, bestConfidence
                ));
            }
            else
            {
                _logger.Debug("[DecisionFusionModel] No se encontró una zona válida");
            }
        }

        /// <summary>
        /// Calcula la Confidence de una HeatZone aplicando la fórmula de scoring completa
        /// Retorna un DecisionScoreBreakdown con todas las contribuciones (explainability)
        /// </summary>
        private DecisionScoreBreakdown CalculateConfidence(HeatZone zone, DecisionSnapshot snapshot, CoreEngine coreEngine, int currentBar)
        {
            var breakdown = new DecisionScoreBreakdown();

            // 1. CoreScoreContribution: Score base de la zona
            breakdown.CoreScoreContribution = zone.Score * _config.Weight_CoreScore;

            // 2. ProximityContribution: Proximidad al precio actual
            double proximityFactor = zone.Metadata.ContainsKey("ProximityFactor")
                ? (double)zone.Metadata["ProximityFactor"]
                : 0.0;
            breakdown.ProximityContribution = proximityFactor * _config.Weight_Proximity;

            // 3. ConfluenceContribution: Confluencia de estructuras (normalizada con saturación)
            double confluenceFactor = Math.Min(1.0, (double)zone.ConfluenceCount / _config.MaxConfluenceReference);
            breakdown.ConfluenceContribution = confluenceFactor * _config.Weight_Confluence;

            // 4. TypeContribution: Tipo de estructura dominante
            double typeMultiplier = _config.TypeMultipliers.ContainsKey(zone.DominantType)
                ? _config.TypeMultipliers[zone.DominantType]
                : 1.0;
            breakdown.TypeContribution = (typeMultiplier / 1.5) * _config.Weight_Type; // Normalizar por max (1.5 = OB)

            // 5. BiasContribution: Alineamiento con el bias global
            double biasAlignment = CalculateBiasAlignment(zone.Direction, snapshot.GlobalBias, snapshot.GlobalBiasStrength);
            breakdown.BiasContribution = biasAlignment * _config.Weight_Bias;

            // 6. MomentumContribution: Momentum reciente (basado en BOS/CHoCH)
            double momentumFactor = CalculateMomentumFactor(zone, coreEngine, currentBar);
            breakdown.MomentumContribution = momentumFactor * _config.Weight_Momentum;

            // 7. VolumeContribution: Volumen de las estructuras en la zona
            double volumeFactor = CalculateVolumeFactor(zone);
            breakdown.VolumeContribution = volumeFactor * _config.Weight_Volume;

            // 8. Sumar todas las contribuciones
            double rawConfidence = breakdown.CoreScoreContribution
                                 + breakdown.ProximityContribution
                                 + breakdown.ConfluenceContribution
                                 + breakdown.TypeContribution
                                 + breakdown.BiasContribution
                                 + breakdown.MomentumContribution
                                 + breakdown.VolumeContribution;

            // 9. Aplicar penalización si va contra el bias global
            if (zone.Direction != snapshot.GlobalBias && snapshot.GlobalBias != "Neutral")
            {
                rawConfidence *= _config.BiasOverrideConfidenceFactor;
                _logger.Debug(string.Format(
                    "[DecisionFusionModel] HeatZone {0} va contra el bias global ({1} vs {2}), aplicando penalización: {3:F3} -> {4:F3}",
                    zone.Id, zone.Direction, snapshot.GlobalBias, rawConfidence / _config.BiasOverrideConfidenceFactor, rawConfidence
                ));
            }

            // 10. Clamp a [0.0, 1.0]
            breakdown.FinalConfidence = Math.Max(0.0, Math.Min(1.0, rawConfidence));

            return breakdown;
        }

        /// <summary>
        /// Calcula el alineamiento con el bias global
        /// Retorna: 1.0 (alineado), 0.5 (neutral), 0.0 (contra el bias)
        /// </summary>
        private double CalculateBiasAlignment(string zoneDirection, string globalBias, double globalBiasStrength)
        {
            if (globalBias == "Neutral")
                return 0.5;

            if (zoneDirection == globalBias)
                return globalBiasStrength; // 0.5 - 1.0 según la fuerza del bias

            return 0.0; // Contra el bias
        }

        /// <summary>
        /// Calcula el factor de momentum basado en BOS/CHoCH recientes en la zona
        /// Retorna: 0.0 - 1.0
        /// </summary>
        private double CalculateMomentumFactor(HeatZone zone, CoreEngine coreEngine, int currentBar)
        {
            // Buscar BOS/CHoCH en el rango de la zona usando QueryOverlappingStructures
            var bosStructures = coreEngine.QueryOverlappingStructures(zone.TFDominante, zone.Low, zone.High)
                .Where(s => (s.GetType().Name == "BOS" || s.GetType().Name == "CHoCH") &&
                            (currentBar - s.CreatedAtBarIndex) <= 20) // Recientes (20 barras)
                .ToList();

            if (bosStructures.Count == 0)
                return 0.3; // Momentum neutral

            // Contar breaks alineados con la dirección de la zona
            int alignedBreaks = bosStructures.Count(s => GetStructureDirection(s) == zone.Direction);
            double momentumFactor = Math.Min(1.0, (double)alignedBreaks / 3.0); // Saturación en 3 breaks

            return momentumFactor;
        }

        /// <summary>
        /// Calcula el factor de volumen basado en las estructuras de la zona
        /// Retorna: 0.0 - 1.0
        /// </summary>
        private double CalculateVolumeFactor(HeatZone zone)
        {
            // Por ahora, retornar un valor neutral
            // TODO: Implementar análisis de volumen cuando esté disponible en las estructuras
            return 0.5;
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

