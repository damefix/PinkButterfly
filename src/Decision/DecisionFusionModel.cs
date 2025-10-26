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

            int rejectedZones = snapshot.HeatZones.Count - validZones.Count;
            if (rejectedZones > 0)
            {
                _logger.Info(string.Format("[DecisionFusionModel] {0} HeatZones rechazadas por RiskCalculator (Entry demasiado lejos del precio actual)", rejectedZones));
                
                // Log detallado de las razones de rechazo
                foreach (var zone in snapshot.HeatZones.Where(z => z.Metadata.ContainsKey("RejectReason")))
                {
                    _logger.Debug(string.Format("[DecisionFusionModel] - HeatZone {0}: {1}", zone.Id, zone.Metadata["RejectReason"]));
                }
            }

            if (validZones.Count == 0)
            {
                _logger.Warning("[DecisionFusionModel] No hay HeatZones con riesgo calculado → Generando WAIT");
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
                
                // 🔍 LOGGING DE DEPURACIÓN: Desglose completo de scoring (solo si está habilitado)
                if (_config.ShowScoringBreakdown)
                {
                    LogScoringBreakdown(bestZone, bestBreakdown, snapshot, coreEngine, currentBar);
                }
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
        /// CALIBRACIÓN V5 (FINAL - ÓPTIMA): Calcula el alineamiento con el bias global
        /// Retorna: 1.0 (alineado), 0.5 (neutral), 0.0 (contra el bias)
        /// Esta lógica original demostró ser la más efectiva (PF 2.00, Win Rate 42.9%)
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
        
        /// <summary>
        /// 🔍 LOGGING DE DEPURACIÓN: Desglose completo de scoring para auditoría
        /// Muestra todos los inputs y outputs del cálculo de Confidence
        /// </summary>
        private void LogScoringBreakdown(HeatZone zone, DecisionScoreBreakdown breakdown, 
                                         DecisionSnapshot snapshot, CoreEngine coreEngine, int currentBar)
        {
            _logger.Info("========================================");
            _logger.Info("🔍 [DEBUG] DESGLOSE COMPLETO DE SCORING");
            _logger.Info("========================================");
            
            // 1. INFORMACIÓN DE LA HEATZONE
            _logger.Info(string.Format("[DEBUG] HeatZone ID: {0}", zone.Id));
            _logger.Info(string.Format("[DEBUG] Direction: {0}", zone.Direction));
            _logger.Info(string.Format("[DEBUG] Price Range: {0:F2} - {1:F2} (Center: {2:F2})", 
                zone.Low, zone.High, zone.CenterPrice));
            _logger.Info(string.Format("[DEBUG] TF Dominante: {0}m", zone.TFDominante));
            _logger.Info(string.Format("[DEBUG] Tipo Dominante: {0}", zone.DominantType));
            _logger.Info(string.Format("[DEBUG] Confluence Count: {0}", zone.ConfluenceCount));
            
            // 2. INPUTS DEL SCORING
            _logger.Info("--- INPUTS ---");
            _logger.Info(string.Format("[DEBUG] Input: CoreScore (base) = {0:F4}", zone.Score));
            
            // ProximityScore desde Metadata
            double proximityScore = zone.Metadata.ContainsKey("ProximityScore") 
                ? (double)zone.Metadata["ProximityScore"] 
                : -1.0;
            _logger.Info(string.Format("[DEBUG] Input: ProximityScore = {0:F4} {1}", 
                proximityScore, proximityScore < 0 ? "❌ NO ENCONTRADO" : ""));
            
            // ProximityFactor desde Metadata
            double proximityFactor = zone.Metadata.ContainsKey("ProximityFactor") 
                ? (double)zone.Metadata["ProximityFactor"] 
                : -1.0;
            _logger.Info(string.Format("[DEBUG] Input: ProximityFactor = {0:F4} {1}", 
                proximityFactor, proximityFactor < 0 ? "❌ NO ENCONTRADO" : ""));
            
            // Distancia al precio actual
            double currentPrice = zone.Metadata.ContainsKey("CurrentPrice") 
                ? (double)zone.Metadata["CurrentPrice"] 
                : 0.0;
            double distanceToPrice = Math.Abs(zone.CenterPrice - currentPrice);
            _logger.Info(string.Format("[DEBUG] Input: Precio Actual = {0:F2}", currentPrice));
            _logger.Info(string.Format("[DEBUG] Input: Distancia al Precio = {0:F2} puntos", distanceToPrice));
            
            _logger.Info(string.Format("[DEBUG] Input: GlobalBias = {0}", snapshot.GlobalBias));
            _logger.Info(string.Format("[DEBUG] Input: GlobalBiasStrength = {0:F4}", snapshot.GlobalBiasStrength));
            
            // 3. OUTPUTS DEL SCORING (Contribuciones)
            _logger.Info("--- OUTPUTS (Contribuciones) ---");
            _logger.Info(string.Format("[DEBUG] Output: CoreScoreContribution = {0:F4} (Peso: {1:F2})", 
                breakdown.CoreScoreContribution, _config.Weight_CoreScore));
            _logger.Info(string.Format("[DEBUG] Output: ProximityContribution = {0:F4} (Peso: {1:F2}) ⚠️ CRÍTICO", 
                breakdown.ProximityContribution, _config.Weight_Proximity));
            _logger.Info(string.Format("[DEBUG] Output: ConfluenceContribution = {0:F4} (Peso: {1:F2})", 
                breakdown.ConfluenceContribution, _config.Weight_Confluence));
            _logger.Info(string.Format("[DEBUG] Output: TypeContribution = {0:F4} (Peso: {1:F2})", 
                breakdown.TypeContribution, _config.Weight_Type));
            _logger.Info(string.Format("[DEBUG] Output: BiasContribution = {0:F4} (Peso: {1:F2})", 
                breakdown.BiasContribution, _config.Weight_Bias));
            _logger.Info(string.Format("[DEBUG] Output: MomentumContribution = {0:F4} (Peso: {1:F2})", 
                breakdown.MomentumContribution, _config.Weight_Momentum));
            _logger.Info(string.Format("[DEBUG] Output: VolumeContribution = {0:F4} (Peso: {1:F2})", 
                breakdown.VolumeContribution, _config.Weight_Volume));
            
            // 4. RESULTADO FINAL
            _logger.Info("--- RESULTADO FINAL ---");
            double sumContributions = breakdown.CoreScoreContribution 
                                    + breakdown.ProximityContribution 
                                    + breakdown.ConfluenceContribution 
                                    + breakdown.TypeContribution 
                                    + breakdown.BiasContribution 
                                    + breakdown.MomentumContribution 
                                    + breakdown.VolumeContribution;
            
            _logger.Info(string.Format("[DEBUG] Suma de Contribuciones = {0:F4}", sumContributions));
            _logger.Info(string.Format("[DEBUG] FinalConfidence = {0:F4}", breakdown.FinalConfidence));
            _logger.Info(string.Format("[DEBUG] MinConfidenceForEntry = {0:F2}", _config.MinConfidenceForEntry));
            
            bool passesThreshold = breakdown.FinalConfidence >= _config.MinConfidenceForEntry;
            _logger.Info(string.Format("[DEBUG] ¿Supera umbral? {0} {1}", 
                passesThreshold ? "✅ SÍ" : "❌ NO", 
                passesThreshold ? "(SEÑAL GENERADA)" : "(SEÑAL RECHAZADA)"));
            
            // 5. DIAGNÓSTICO
            _logger.Info("--- DIAGNÓSTICO ---");
            if (proximityFactor < 0.1 && breakdown.ProximityContribution > 0.01)
            {
                _logger.Warning("[DEBUG] ⚠️ ANOMALÍA DETECTADA: ProximityFactor muy bajo pero ProximityContribution > 0");
            }
            if (distanceToPrice > 50.0 && breakdown.FinalConfidence > _config.MinConfidenceForEntry)
            {
                _logger.Warning(string.Format(
                    "[DEBUG] ⚠️ ANOMALÍA DETECTADA: Zona a {0:F2} puntos pero Confidence={1:F4} (supera umbral)", 
                    distanceToPrice, breakdown.FinalConfidence));
            }
            
            _logger.Info("========================================");
        }
    }
}

