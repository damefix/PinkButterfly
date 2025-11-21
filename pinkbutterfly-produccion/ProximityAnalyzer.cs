// ==========================================================================
// ProximityAnalyzer.cs
// PinkButterfly CoreBrain - Componente 3 del DFM
//
// V5.6: Proximidad sesgo-consciente
// - T_eff = ProximityThresholdATR * (1 + BiasProximityMultiplier) si la zona
//   está alineada con el GlobalBias y GlobalBiasStrength > 0.
// - Gating: no descartar zonas ALINEADAS aunque ProximityFactor == 0 (permitir
//   que el DFM aplique BiasContribution). Contra-bias se filtran si
//   ProximityFactor == 0.
// ==========================================================================

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

        public void Process(DecisionSnapshot snapshot, IBarDataProvider barData, CoreEngine coreEngine, int currentBar, int timeframeMinutes, double accountSize)
        {
            if (snapshot == null)
                throw new ArgumentNullException(nameof(snapshot));
            if (barData == null)
                throw new ArgumentNullException(nameof(barData));
            if (coreEngine == null)
                throw new ArgumentNullException(nameof(coreEngine));

            if (_config.EnablePerfDiagnostics && _config.EnableDebug)
                _logger.Debug("[ProximityAnalyzer] Analizando proximidad de HeatZones...");

            if (snapshot.HeatZones == null || snapshot.HeatZones.Count == 0)
            {
                if (_config.EnablePerfDiagnostics && _config.EnableDebug)
                    _logger.Debug("[ProximityAnalyzer] No hay HeatZones para analizar");
                return;
            }

            double currentPrice = snapshot.Summary.CurrentPrice;

            // Guard: si CurrentPrice inválido, no procesar proximidad
            if (currentPrice <= 0.0)
            {
                _logger.Warning($"[ProximityAnalyzer] CurrentPrice inválido ({currentPrice:F2}). Saltando {snapshot.HeatZones.Count} zonas.");
                return;
            }

            if (_config.EnablePerfDiagnostics && _config.EnableDebug)
                _logger.Debug(string.Format("[ProximityAnalyzer] Precio actual: {0:F2}, HeatZones: {1}",
                    currentPrice, snapshot.HeatZones.Count));

            // Procesar cada HeatZone
            var processedZones = new List<HeatZone>();
            int keptAligned = 0, filteredAligned = 0, keptCounter = 0, filteredCounter = 0;
            double sumProxAligned = 0.0, sumProxCounter = 0.0, sumDistATRAligned = 0.0, sumDistATRCounter = 0.0;

            foreach (var zone in snapshot.HeatZones)
            {
                // V5.6: determinar alineación con bias
                bool isAligned = (zone.Direction == snapshot.GlobalBias) && (snapshot.GlobalBiasStrength > 0.0);

                // Calcular distancia y proximidad (sesgo-consciente)
                CalculateProximityV56(zone, currentPrice, barData, currentBar, isAligned, snapshot.GlobalBiasStrength);

                // Gate temprano por distancia (usar umbral de registro unificado)
                if (zone.Metadata.ContainsKey("DistanceATR"))
                {
                    double distAtr = (double)zone.Metadata["DistanceATR"];
                    bool isHV = snapshot.MarketRegime == "HighVol";
                    double maxGate = isHV ? _config.MaxDistanceToRegister_ATR_HighVol : _config.MaxDistanceToRegister_ATR_Normal;
                    if (distAtr > maxGate)
                    {
                        if (_config.EnablePerfDiagnostics && _config.EnableDebug)
                            _logger.Debug($"[ProximityAnalyzer] SKIP Zone={zone.Id} DistATR={distAtr:F2} > Gate={maxGate:F2} (Regime={snapshot.MarketRegime})");
                        continue;
                    }
                }

                // Filtrar zonas demasiado lejas
                double proximityFactor = zone.Metadata.ContainsKey("ProximityFactor")
                    ? (double)zone.Metadata["ProximityFactor"]
                    : 0.0;

                // V5.6.1: NO mantener zonas con ProximityFactor == 0, incluso si están alineadas
            if (proximityFactor > 0.0)
                {
                    processedZones.Add(zone);
                    if (_config.EnablePerfDiagnostics && _config.EnableDebug)
                        _logger.Debug(string.Format("[ProximityAnalyzer] HeatZone {0}: Proximity={1:F2}, Distance={2:F2} ATR, Aligned={3}",
                            zone.Id, proximityFactor, zone.Metadata["DistanceATR"], isAligned));
                    if (isAligned)
                    {
                        keptAligned++;
                        sumProxAligned += proximityFactor;
                        sumDistATRAligned += (double)zone.Metadata["DistanceATR"];
                    }
                    else
                    {
                        keptCounter++;
                        sumProxCounter += proximityFactor;
                        sumDistATRCounter += (double)zone.Metadata["DistanceATR"];
                    }
                }
                else
                {
                    if (_config.EnablePerfDiagnostics && _config.EnableDebug)
                        _logger.Debug(string.Format("[ProximityAnalyzer] HeatZone {0} filtrada (demasiado lejos). Aligned={1}", zone.Id, isAligned));
                    if (isAligned) filteredAligned++; else filteredCounter++;
                }
            }

            // Ordenar por proximidad (más cercanas primero) + desempates deterministas
            processedZones = processedZones
                .OrderByDescending(z => (double)z.Metadata["ProximityFactor"])
                .ThenByDescending(z => z.TFDominante)
                .ThenByDescending(z => z.Score)                    // Score de la zona
                .ThenByDescending(z => z.ConfluenceCount)          // Más confluencias
                .ThenBy(z => z.Metadata.ContainsKey("DistanceATR") ? (double)z.Metadata["DistanceATR"] : 999.0)
                .ThenBy(z => z.Low)
                .ThenBy(z => z.High)
                .ThenBy(z => z.DominantType, StringComparer.Ordinal)
                .ToList();

            // V5.6.4: si existen zonas alineadas con Proximity>0, preferirlas y purgar contra-bias este ciclo
            bool hasAligned = processedZones.Any(z => z.Metadata.ContainsKey("AlignedWithBias")
                                                      && (bool)z.Metadata["AlignedWithBias"]
                                                      && (double)z.Metadata["ProximityFactor"] > 0.0);
            // Guard: no aplicar PreferAligned si el bias global es Neutral (evita vaciar el embudo)
            bool isNeutralBias = snapshot.GlobalBias == "Neutral";
            // Diagnóstico previo a preferencia
            int preAligned = processedZones.Count(z => z.Metadata.ContainsKey("AlignedWithBias") && (bool)z.Metadata["AlignedWithBias"]);
            int preCounter = processedZones.Count - preAligned;
            double preAvgProxAligned = processedZones.Where(z => z.Metadata.ContainsKey("AlignedWithBias") && (bool)z.Metadata["AlignedWithBias"]) 
                                                     .Select(z => (double)z.Metadata["ProximityFactor"]).DefaultIfEmpty(0).Average();
            double preAvgDistAligned = processedZones.Where(z => z.Metadata.ContainsKey("AlignedWithBias") && (bool)z.Metadata["AlignedWithBias"]) 
                                                     .Select(z => (double)z.Metadata["DistanceATR"]).DefaultIfEmpty(0).Average();
            _logger.Info(string.Format("[DIAGNOSTICO][Proximity] Pre: Aligned={0}/{1} Counter={2}/{3} AvgProxAligned={4:F3} AvgDistATRAligned={5:F2}",
                preAligned, processedZones.Count, preCounter, processedZones.Count, preAvgProxAligned, preAvgDistAligned));

            if (!isNeutralBias && hasAligned)
            {
                int before = processedZones.Count;
                processedZones = processedZones
                    .Where(z => z.Metadata.ContainsKey("AlignedWithBias") && (bool)z.Metadata["AlignedWithBias"]) 
                    .ToList();
                int after = processedZones.Count;
                _logger.Info(string.Format("[DIAGNOSTICO][Proximity] PreferAligned: filtradas {0} contra-bias, quedan {1}", before - after, after));
            }

            // V5.6.7-a: se elimina endurecimiento adicional; PreferAligned ya controla el funnel

            snapshot.HeatZones = processedZones;

            if (_config.EnablePerfDiagnostics && _config.EnableDebug)
                _logger.Debug(string.Format("[ProximityAnalyzer] Análisis completado: {0}/{1} HeatZones relevantes",
                    processedZones.Count, snapshot.HeatZones.Count));

            // Resumen agregado del pipeline de Proximity cada N barras del TF de decisión
            if (timeframeMinutes == _config.DecisionTimeframeMinutes)
            {
                int interval = Math.Max(1, _config.DiagnosticsInterval);
                if ((currentBar % interval) == 0)
                {
                    _logger.Info(string.Format("[PIPE][PROX2] TF={0} Bar={1} ZonesIn={2} ZonesKept={3} KeptAligned={4} KeptCounter={5}",
                        timeframeMinutes, currentBar, snapshot.HeatZones.Count, processedZones.Count, keptAligned, keptCounter));
                }
            }

            // Resumen diagnóstico
            int totalAligned = keptAligned + filteredAligned;
            int totalCounter = keptCounter + filteredCounter;
            double avgProxAligned = keptAligned > 0 ? (sumProxAligned / keptAligned) : 0.0;
            double avgProxCounter = keptCounter > 0 ? (sumProxCounter / keptCounter) : 0.0;
            double avgDistATRAligned = keptAligned > 0 ? (sumDistATRAligned / keptAligned) : 0.0;
            double avgDistATRCounter = keptCounter > 0 ? (sumDistATRCounter / keptCounter) : 0.0;

            _logger.Info("[DIAGNOSTICO][Proximity]" +
                string.Format(" KeptAligned={0}/{1}, KeptCounter={2}/{3}, AvgProxAligned={4:F3}, AvgProxCounter={5:F3}, AvgDistATRAligned={6:F2}, AvgDistATRCounter={7:F2}",
                    keptAligned, totalAligned, keptCounter, totalCounter, avgProxAligned, avgProxCounter, avgDistATRAligned, avgDistATRCounter));

            // Resumen de drivers (INFO): medias de BaseProx, ZoneHeightATR, SizePenalty y FinalProx por alineación
            int aCount = 0, cCount = 0;
            double aSumBase = 0.0, aSumZoneATR = 0.0, aSumSizePen = 0.0, aSumFinal = 0.0;
            double cSumBase = 0.0, cSumZoneATR = 0.0, cSumSizePen = 0.0, cSumFinal = 0.0;

            foreach (var z in processedZones)
            {
                bool aligned = z.Metadata.ContainsKey("AlignedWithBias") && (bool)z.Metadata["AlignedWithBias"];
                double baseProx = z.Metadata.ContainsKey("BaseProx") ? (double)z.Metadata["BaseProx"] : 0.0;
                double zoneATRh = z.Metadata.ContainsKey("ZoneHeightATR") ? (double)z.Metadata["ZoneHeightATR"] : 0.0;
                double sizePen = z.Metadata.ContainsKey("SizePenalty") ? (double)z.Metadata["SizePenalty"] : 0.0;
                double finalProx = z.Metadata.ContainsKey("ProximityFactor") ? (double)z.Metadata["ProximityFactor"] : 0.0;

                if (aligned)
                {
                    aCount++; aSumBase += baseProx; aSumZoneATR += zoneATRh; aSumSizePen += sizePen; aSumFinal += finalProx;
                }
                else
                {
                    cCount++; cSumBase += baseProx; cSumZoneATR += zoneATRh; cSumSizePen += sizePen; cSumFinal += finalProx;
                }
            }

            double aAvgBase = aCount > 0 ? aSumBase / aCount : 0.0;
            double aAvgZone = aCount > 0 ? aSumZoneATR / aCount : 0.0;
            double aAvgSize = aCount > 0 ? aSumSizePen / aCount : 0.0;
            double aAvgFinal = aCount > 0 ? aSumFinal / aCount : 0.0;
            double cAvgBase = cCount > 0 ? cSumBase / cCount : 0.0;
            double cAvgZone = cCount > 0 ? cSumZoneATR / cCount : 0.0;
            double cAvgSize = cCount > 0 ? cSumSizePen / cCount : 0.0;
            double cAvgFinal = cCount > 0 ? cSumFinal / cCount : 0.0;

            _logger.Info(string.Format("[DIAGNOSTICO][Proximity] Drivers: Aligned n={0} BaseProx≈ {1:F3} ZoneATR≈ {2:F2} SizePenalty≈ {3:F3} FinalProx≈ {4:F3} | Counter n={5} BaseProx≈ {6:F3} ZoneATR≈ {7:F2} SizePenalty≈ {8:F3} FinalProx≈ {9:F3}",
                aCount, aAvgBase, aAvgZone, aAvgSize, aAvgFinal,
                cCount, cAvgBase, cAvgZone, cAvgSize, cAvgFinal));
        }

        /// <summary>
        /// V5.6: Calcula la distancia y el factor de proximidad aplicando umbral sesgo-consciente
        /// </summary>
        private void CalculateProximityV56(HeatZone zone, double currentPrice, IBarDataProvider barData, int currentBar,
                                           bool isAlignedWithBias, double globalBiasStrength)
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

            // 2. Obtener ATR del TF Dominante de la zona, alineado por tiempo del TF de decisión
            int decisionTF = _config.DecisionTimeframeMinutes;
            DateTime analysisTime = barData.GetBarTime(decisionTF, currentBar);
            int idxDom = barData.GetBarIndexFromTime(zone.TFDominante, analysisTime);
            if (idxDom < 0)
            {
                // Sin datos alineados: marcar zona sin proximidad en este ciclo
                zone.Metadata["Distance"] = Math.Abs(currentPrice - entryPrice);
                zone.Metadata["DistanceATR"] = double.MaxValue;
                zone.Metadata["ProximityFactor"] = 0.0;
                zone.Metadata["BaseProx"] = 0.0;
                zone.Metadata["ZoneHeightATR"] = 0.0;
                zone.Metadata["SizePenalty"] = 0.0;
                zone.Metadata["ProximityScore"] = 0.0;
                zone.Metadata["DistanceTicks"] = 0.0;
                zone.Metadata["IsInside"] = false;
                zone.Metadata["CurrentPrice"] = currentPrice;
                zone.Metadata["AlignedWithBias"] = isAlignedWithBias;
                zone.Metadata["ProximityThresholdEff_ATR"] = _config.ProximityThresholdATR;
                _logger.Info($"[CTX_NO_DATA] Proximity: TF={zone.TFDominante} sin índice para time={analysisTime:yyyy-MM-dd HH:mm}. Zona {zone.Id} => Proximity=0");
                return;
            }
            double atr = barData.GetATR(zone.TFDominante, 14, idxDom);

            // Evitar división por cero
            if (atr <= 0)
            {
                _logger.Warning(string.Format("[ProximityAnalyzer] ATR({0}) es 0 para HeatZone {1}, usando ATR=1.0",
                    zone.TFDominante, zone.Id));
                atr = 1.0;
            }

            // 3. Normalizar distancia por ATR
            double distanceATR = distance / atr;

            // 4. Umbral efectivo V5.6 (sesgo-consciente)
            double thresholdEff = _config.ProximityThresholdATR;
            if (isAlignedWithBias && globalBiasStrength > 0.0)
            {
                thresholdEff *= (1.0 + _config.BiasProximityMultiplier);
            }

            // 5. Calcular factor de proximidad base (lineal) con threshold efectivo
            double baseProximityFactor = Math.Max(0.0, 1.0 - (distanceATR / thresholdEff));
            
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
            zone.Metadata["BaseProx"] = baseProximityFactor;
            zone.Metadata["ZoneHeightATR"] = zoneHeightATR;
            zone.Metadata["SizePenalty"] = sizePenalty;
            zone.Metadata["ProximityScore"] = proximityFactor; // Alias para compatibilidad
            zone.Metadata["DistanceTicks"] = distanceTicks;
            zone.Metadata["IsInside"] = distance == 0.0;
            zone.Metadata["CurrentPrice"] = currentPrice; // Para debugging
            zone.Metadata["AlignedWithBias"] = isAlignedWithBias;
            zone.Metadata["ProximityThresholdEff_ATR"] = thresholdEff;
            
            // Logging de depuración
            if (currentPrice == 0.0)
            {
                _logger.Warning(string.Format(
                    "[ProximityAnalyzer] ⚠️ BUG DETECTADO: CurrentPrice = 0.00 para HeatZone {0} (TF={1})",
                    zone.Id, zone.TFDominante));
            }
            
            // Logging detallado para depuración
            if (_config.EnablePerfDiagnostics && _config.EnableDebug)
                _logger.Debug(string.Format(
                    "[ProximityAnalyzer] HeatZone {0}: Entry={1:F2}, Price={2:F2}, Dist={3:F2} ({4:F2} ATR), ThrEff={5:F2} ATR, BaseProx={6:F4}, ZoneATR={7:F2}, SizePenalty={8:F4}, FinalProx={9:F4}, Aligned={10}",
                    zone.Id, entryPrice, currentPrice, distance, distanceATR, thresholdEff, baseProximityFactor,
                    zoneHeightATR, sizePenalty, proximityFactor, isAlignedWithBias));
        }
    }
}

