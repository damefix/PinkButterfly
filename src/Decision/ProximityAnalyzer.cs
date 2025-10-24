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
            // 1. Calcular distancia (lógica jerárquica: dentro/fuera)
            double distance;

            if (currentPrice >= zone.Low && currentPrice <= zone.High)
            {
                // Precio está DENTRO de la zona
                distance = 0.0;
            }
            else
            {
                // Precio está FUERA de la zona: distancia al borde más cercano
                double distanceToHigh = Math.Abs(currentPrice - zone.High);
                double distanceToLow = Math.Abs(currentPrice - zone.Low);
                distance = Math.Min(distanceToHigh, distanceToLow);
            }

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

            // 4. Calcular factor de proximidad (lineal)
            // proximityFactor = max(0, 1 - (distanceATR / ProximityThresholdATR))
            double proximityFactor = Math.Max(0.0, 1.0 - (distanceATR / _config.ProximityThresholdATR));

            // 5. Calcular distancia en ticks (para Entry/Slippage)
            double tickSize = barData.GetTickSize();
            double distanceTicks = tickSize > 0 ? distance / tickSize : 0.0;

            // 6. Añadir a Metadata
            zone.Metadata["Distance"] = distance;
            zone.Metadata["DistanceATR"] = distanceATR;
            zone.Metadata["ProximityFactor"] = proximityFactor;
            zone.Metadata["DistanceTicks"] = distanceTicks;
            zone.Metadata["IsInside"] = distance == 0.0;
        }
    }
}

