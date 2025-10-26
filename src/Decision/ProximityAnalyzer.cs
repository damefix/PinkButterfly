// ============================================================================
// ProximityAnalyzer.cs
// PinkButterfly CoreBrain - Componente 3 del DFM
// 
// Responsabilidades:
// - Para cada HeatZone, calcular la distancia al precio actual
// - Calcular el factor de proximidad normalizado (0.0 = lejos, 1.0 = muy cerca)
// - Ordenar las HeatZones por proximidad (las más cercanas primero)
// - Filtrar zonas que están demasiado lejos (> ProximityThresholdATR)
// - Añadir distanceTicks a Metadata para uso posterior
//
// Fórmula de Proximidad:
//   1. Si CurrentPrice está DENTRO de [Low, High]: distance = 0
//   2. Si CurrentPrice está FUERA: distance = min(|CurrentPrice - High|, |CurrentPrice - Low|)
//   3. distanceATR = distance / ATR(TF_Dominante)
//   4. proximityFactor = max(0, 1 - (distanceATR / ProximityThresholdATR))
//
// Rango: proximityFactor = 1.0 (dentro de la zona) a 0.0 (muy lejos)
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// ProximityAnalyzer: Tercer componente del pipeline DFM
    /// Calcula la distancia y relevancia de cada HeatZone respecto al precio actual
    /// </summary>
    public class ProximityAnalyzer : IDecisionComponent
    {
        private EngineConfig _config;
        private ILogger _logger;

        public string ComponentName => "ProximityAnalyzer";

        public void Initialize(EngineConfig config, ILogger logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug("[ProximityAnalyzer] Inicializado");
        }

        public void Process(DecisionSnapshot snapshot, IBarDataProvider barData, CoreEngine coreEngine, int currentBar, double accountSize)
        {
            if (snapshot == null)
                throw new ArgumentNullException(nameof(snapshot));
            if (barData == null)
                throw new ArgumentNullException(nameof(barData));
            if (coreEngine == null)
                throw new ArgumentNullException(nameof(coreEngine));

            _logger.Debug("[ProximityAnalyzer] Analizando proximidad de HeatZones...");

            if (snapshot.HeatZones == null || snapshot.HeatZones.Count == 0)
            {
                _logger.Debug("[ProximityAnalyzer] No hay HeatZones para analizar");
                return;
            }

            double currentPrice = snapshot.Summary.CurrentPrice;

            _logger.Debug(string.Format("[ProximityAnalyzer] Precio actual: {0:F2}, HeatZones: {1}",
                currentPrice, snapshot.HeatZones.Count));

            // Procesar cada HeatZone
            var processedZones = new List<HeatZone>();

            foreach (var zone in snapshot.HeatZones)
            {
                // Calcular distancia y proximidad
                CalculateProximity(zone, currentPrice, barData, currentBar);

                // Filtrar zonas demasiado lejas
                double proximityFactor = zone.Metadata.ContainsKey("ProximityFactor")
                    ? (double)zone.Metadata["ProximityFactor"]
                    : 0.0;

                if (proximityFactor > 0.0)
                {
                    processedZones.Add(zone);
                    _logger.Debug(string.Format("[ProximityAnalyzer] HeatZone {0}: Proximity={1:F2}, Distance={2:F2} ATR",
                        zone.Id, proximityFactor, zone.Metadata["DistanceATR"]));
                }
                else
                {
                    _logger.Debug(string.Format("[ProximityAnalyzer] HeatZone {0} filtrada (demasiado lejos)", zone.Id));
                }
            }

            // Ordenar por proximidad (más cercanas primero)
            processedZones = processedZones
                .OrderByDescending(z => (double)z.Metadata["ProximityFactor"])
                .ToList();

            snapshot.HeatZones = processedZones;

            _logger.Debug(string.Format("[ProximityAnalyzer] Análisis completado: {0}/{1} HeatZones relevantes",
                processedZones.Count, snapshot.HeatZones.Count));
        }

        /// <summary>
        /// Calcula la distancia y el factor de proximidad para una HeatZone
        /// Añade los resultados a zone.Metadata
        /// </summary>
        private void CalculateProximity(HeatZone zone, double currentPrice, IBarDataProvider barData, int currentBar)
        {
            // 1. Calcular distancia al ENTRY ESTRUCTURAL (no al borde de la zona)
            // Esto refleja la distancia real a donde se colocará la orden
            double entryPrice;
            
            if (zone.Direction == "Bullish")
            {
                // BUY: Entry en el borde inferior de la zona
                entryPrice = zone.Low;
            }
            else if (zone.Direction == "Bearish")
            {
                // SELL: Entry en el borde superior de la zona
                entryPrice = zone.High;
            }
            else
            {
                // Neutral: usar centro de la zona
                entryPrice = zone.CenterPrice;
            }
            
            // Distancia al Entry estructural
            double distance = Math.Abs(currentPrice - entryPrice);

            // 2. Obtener ATR del TF Dominante de la zona
            double atr = barData.GetATR(zone.TFDominante, currentBar, 14);

            // Evitar división por cero
            if (atr <= 0)
            {
                _logger.Warning(string.Format("[ProximityAnalyzer] ATR({0}) es 0 para HeatZone {1}, usando ATR=1.0",
                    zone.TFDominante, zone.Id));
                atr = 1.0;
            }

            // 3. Normalizar distancia por ATR
            double distanceATR = distance / atr;

            // 4. Calcular factor de proximidad base (lineal)
            // proximityFactor = max(0, 1 - (distanceATR / ProximityThresholdATR))
            double baseProximityFactor = Math.Max(0.0, 1.0 - (distanceATR / _config.ProximityThresholdATR));
            
            // 5. Penalización por tamaño de zona (zonas grandes son menos precisas)
            double zoneHeight = zone.High - zone.Low;
            double zoneHeightATR = zoneHeight / atr;
            
            // CALIBRACIÓN V5 (ÓPTIMA): Penalización suavizada para zonas de tamaño medio
            // - Zonas < 5 ATR: sin penalización (1.0)
            // - Zonas 5-15 ATR: penalización MUY leve (1.0 -> 0.80) - SUAVIZADO
            // - Zonas 15-30 ATR: penalización moderada (0.80 -> 0.30)
            // - Zonas > 30 ATR: penalización máxima (0.30 mínimo)
            double sizePenalty;
            if (zoneHeightATR <= 5.0)
            {
                sizePenalty = 1.0; // Sin penalización para zonas pequeñas
            }
            else if (zoneHeightATR <= 15.0)
            {
                // CALIBRACIÓN V5: Penalización MUY leve: 1.0 -> 0.80 (antes era 1.0 -> 0.5)
                // Formula: 1.0 - ((zoneHeightATR - 5.0) / 50.0)
                // En 5 ATR: 1.0, en 10 ATR: 0.90, en 15 ATR: 0.80
                sizePenalty = 1.0 - ((zoneHeightATR - 5.0) / 50.0);
            }
            else if (zoneHeightATR <= 30.0)
            {
                // Penalización moderada: 0.80 -> 0.30
                sizePenalty = 0.80 - ((zoneHeightATR - 15.0) / 30.0);
            }
            else
            {
                // Penalización máxima para zonas gigantes
                sizePenalty = 0.30;
            }
            
            // 6. Factor de proximidad final (con penalización)
            double proximityFactor = baseProximityFactor * sizePenalty;

            // 7. Calcular distancia en ticks (para Entry/Slippage)
            double tickSize = barData.GetTickSize();
            double distanceTicks = tickSize > 0 ? distance / tickSize : 0.0;

            // 8. Añadir a Metadata
            zone.Metadata["Distance"] = distance;
            zone.Metadata["DistanceATR"] = distanceATR;
            zone.Metadata["ProximityFactor"] = proximityFactor;
            zone.Metadata["ProximityScore"] = proximityFactor; // Alias para compatibilidad
            zone.Metadata["DistanceTicks"] = distanceTicks;
            zone.Metadata["IsInside"] = distance == 0.0;
            zone.Metadata["CurrentPrice"] = currentPrice; // Para debugging
            
            // Logging de depuración
            if (currentPrice == 0.0)
            {
                _logger.Warning(string.Format(
                    "[ProximityAnalyzer] ⚠️ BUG DETECTADO: CurrentPrice = 0.00 para HeatZone {0} (TF={1})",
                    zone.Id, zone.TFDominante));
            }
            
            // Logging detallado para depuración
            _logger.Debug(string.Format(
                "[ProximityAnalyzer] HeatZone {0}: EntryPrice={1:F2}, CurrentPrice={2:F2}, Distance={3:F2}, " +
                "DistanceATR={4:F2}, BaseProximity={5:F4}, ZoneHeightATR={6:F2}, SizePenalty={7:F4}, FinalProximity={8:F4}",
                zone.Id, entryPrice, currentPrice, distance, distanceATR, baseProximityFactor, 
                zoneHeightATR, sizePenalty, proximityFactor));
        }
    }
}

