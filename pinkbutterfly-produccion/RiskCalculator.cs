// ============================================================================
// RiskCalculator.cs
// PinkButterfly CoreBrain - Componente 4 del DFM
// 
// Responsabilidades:
// - Para cada HeatZone relevante, calcular niveles de SL y TP ESTRUCTURALES
// - SL estructural: Basado en el Swing Low/High protector más cercano
// - TP jerárquico: Basado en Liquidity Grabs/Voids, FVGs/OBs opuestos, o Swings
// - Calcular PositionSizeContracts basado en RiskPercentPerTrade y distancia al SL
// - Validar que el R:R sea >= MinRiskRewardRatio
//
// LÓGICA ESTRUCTURAL INTELIGENTE:
//   SL: Swing Low/High protector ± (ATR * SLBufferFactor), mínimo 3.0 × ATR
//   TP: Prioridad: LG/LV opuesto > FVG/OB opuesto (Score>0.7) > Swing opuesto > R:R mínimo
//   Entry: Borde de la HeatZone (Low para BUY, High para SELL)
//
// PositionSize:
//   riskPerContract = |Entry - SL| * PointValue
//   accountRisk = AccountSize * (RiskPercentPerTrade / 100)
//   positionSize = accountRisk / riskPerContract
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// RiskCalculator: Cuarto componente del pipeline DFM
    /// Calcula niveles de SL, TP y position size para cada HeatZone usando INTELIGENCIA ESTRUCTURAL
    /// </summary>
    public class RiskCalculator : IDecisionComponent
    {
        private EngineConfig _config;
        private ILogger _logger;
        private int _riskRejectionCounter = 0;
        // Tiempo de análisis alineado al TF de decisión para este ciclo
        private DateTime _currentAnalysisTime = DateTime.MinValue;

        public string ComponentName => "RiskCalculator";

        public void Initialize(EngineConfig config, ILogger logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug("[RiskCalculator] Inicializado con lógica estructural inteligente");
        }

        public void Process(DecisionSnapshot snapshot, IBarDataProvider barData, CoreEngine coreEngine, int currentBar, int timeframeMinutes, double accountSize)
        {
            if (snapshot == null)
                throw new ArgumentNullException(nameof(snapshot));
            if (barData == null)
                throw new ArgumentNullException(nameof(barData));
            // Permitir funcionamiento en modo fallback cuando no hay CoreEngine (p.ej., tests unitarios)
            bool fallbackMode = coreEngine == null;
            if (accountSize <= 0)
            {
                _logger.Warning("[RiskCalculator] AccountSize <= 0, no se puede calcular position size");
                return;
            }

            _logger.Debug("[RiskCalculator] Calculando riesgo estructural para HeatZones...");

            // Fijar tiempo de análisis global para este ciclo (barrera temporal) usando currentBar
            int decisionTF_forRisk = _config.DecisionTimeframeMinutes;
            _currentAnalysisTime = barData.GetBarTime(decisionTF_forRisk, currentBar);

            if (snapshot.HeatZones == null || snapshot.HeatZones.Count == 0)
            {
                _logger.Debug("[RiskCalculator] No hay HeatZones para calcular riesgo");
                return;
            }

            // ✅ TRAZA CRÍTICA: Contar zonas recibidas desde ProximityAnalyzer
            int zonesReceived = snapshot.HeatZones.Count;
            _logger.Info(string.Format("[TRACE][RiskCalculator] Zonas recibidas desde ProximityAnalyzer: {0}", zonesReceived));

            // Procesar cada HeatZone
            int rejSLTooFar = 0, rejTPTooNear = 0, rejRRLow = 0, rejEntryTooFar = 0, accepted = 0;
            // Métricas de selección SLPick por bandas y TF para aceptadas
            int band_lt8 = 0, band_8_10 = 0, band_10_12_5 = 0, band_12_5_15 = 0, band_gt15 = 0;
            int tf_5 = 0, tf_15 = 0, tf_60 = 0, tf_240 = 0, tf_1440 = 0;
            // Promedio RR por bandas gruesas
            double rr_sum_0_10 = 0.0; int rr_n_0_10 = 0;
            double rr_sum_10_15 = 0.0; int rr_n_10_15 = 0;
            // Histograma de SLDistanceATR por alineación (bins: 0-10, 10-15, 15-20, 20-25, 25+)
            int[] binsAligned = new int[5];
            int[] binsCounter = new int[5];
            foreach (var zone in snapshot.HeatZones)
            {
                int beforeRejects = (rejSLTooFar + rejTPTooNear + rejRRLow + rejEntryTooFar);
                if (fallbackMode)
                {
                    CalculateFallbackRiskLevels(zone, barData, currentBar, timeframeMinutes, accountSize);
                }
                else
                {
                    CalculateStructuralRiskLevels(zone, snapshot, barData, coreEngine, currentBar, accountSize);
                }
                bool riskCalculated = zone.Metadata.ContainsKey("RiskCalculated") && (bool)zone.Metadata["RiskCalculated"];
                if (riskCalculated)
                {
                    accepted++;
                    // Acumular métricas de bandas/TF y RR
                    double distAtr = zone.Metadata.ContainsKey("SLPickDistATR") ? (double)zone.Metadata["SLPickDistATR"] : (zone.Metadata.ContainsKey("SLDistanceATR") ? (double)zone.Metadata["SLDistanceATR"] : 0.0);
                    int swingTf = zone.Metadata.ContainsKey("SL_SwingTF") ? (int)zone.Metadata["SL_SwingTF"] : -1;
                    double rr = zone.Metadata.ContainsKey("ActualRR") ? (double)zone.Metadata["ActualRR"] : 0.0;
                    if (distAtr < 8.0) band_lt8++; else if (distAtr < 10.0) band_8_10++;
                    else if (distAtr < 12.5) band_10_12_5++; else if (distAtr <= 15.0) band_12_5_15++; else band_gt15++;
                    if (swingTf == 5) tf_5++; else if (swingTf == 15) tf_15++; else if (swingTf == 60) tf_60++; else if (swingTf == 240) tf_240++; else if (swingTf == 1440) tf_1440++;
                    if (distAtr < 10.0) { rr_sum_0_10 += rr; rr_n_0_10++; }
                    else if (distAtr <= 15.0) { rr_sum_10_15 += rr; rr_n_10_15++; }
                }
                else
                {
                    var reason = zone.Metadata.ContainsKey("RejectReason") ? (string)zone.Metadata["RejectReason"] : string.Empty;
                    if (!string.IsNullOrEmpty(reason))
                    {
                        if (reason.StartsWith("SL absurdo"))
                        {
                            rejSLTooFar++;
                            // Usar bin y alineación guardados en CalculateStructuralRiskLevels
                            int binIdx = zone.Metadata.ContainsKey("SLRejectedBin") ? (int)zone.Metadata["SLRejectedBin"] : -1;
                            bool aligned = zone.Metadata.ContainsKey("RejectedAligned") && (bool)zone.Metadata["RejectedAligned"];
                            if (binIdx >= 0)
                            {
                                if (aligned) binsAligned[binIdx]++; else binsCounter[binIdx]++;
                            }
                        }
                        else if (reason.StartsWith("TP insuficiente")) rejTPTooNear++;
                        else if (reason.StartsWith("R:R absurdo")) rejRRLow++;
                        else if (reason.StartsWith("Entry demasiado lejos")) rejEntryTooFar++;
                    }
                }
            }

            _logger.Debug(string.Format("[RiskCalculator] Riesgo calculado para {0} HeatZones", snapshot.HeatZones.Count));

            // ✅ TRAZA CRÍTICA: Resumen del procesamiento
            int totalProcessed = accepted + rejSLTooFar + rejTPTooNear + rejRRLow + rejEntryTooFar;
            int lostZones = zonesReceived - totalProcessed;
            _logger.Info(string.Format("[TRACE][RiskCalculator] Resumen: Recibidas={0} | Procesadas={1} | Aceptadas={2} | Rechazadas={3} | PERDIDAS={4}",
                zonesReceived, totalProcessed, accepted, (totalProcessed - accepted), lostZones));
            
            // Resumen diagnóstico
            _logger.Info(string.Format("[DIAGNOSTICO][Risk] Accepted={0} RejSL={1} RejTP={2} RejRR={3} RejEntry={4}",
                accepted, rejSLTooFar, rejTPTooNear, rejRRLow, rejEntryTooFar));
            // Bridge para analizadores que esperan prefijo [DIAG]
            _logger.Info(string.Format("[DIAG][Risk] Accepted={0} RejSL={1} RejTP={2} RejRR={3} RejEntry={4}",
                accepted, rejSLTooFar, rejTPTooNear, rejRRLow, rejEntryTooFar));

            // Resumen histograma SL por alineación
            _logger.Info(string.Format("[DIAGNOSTICO][Risk] HistSL Aligned=0-10:{0},10-15:{1},15-20:{2},20-25:{3},25+:{4} | Counter=0-10:{5},10-15:{6},15-20:{7},20-25:{8},25+:{9}",
                binsAligned[0], binsAligned[1], binsAligned[2], binsAligned[3], binsAligned[4],
                binsCounter[0], binsCounter[1], binsCounter[2], binsCounter[3], binsCounter[4]));

            // Resumen diagnóstico: SLPick por bandas y TF
            _logger.Info(string.Format("[DIAGNOSTICO][Risk] SLPickBands: lt8:{0},8-10:{1},10-12.5:{2},12.5-15:{3},gt15:{4} | TF 5:{5},15:{6},60:{7},240:{8},1440:{9}",
                band_lt8, band_8_10, band_10_12_5, band_12_5_15, band_gt15, tf_5, tf_15, tf_60, tf_240, tf_1440));
            _logger.Info(string.Format("[DIAG][Risk] SLPickBands: lt8:{0},8-10:{1},10-12.5:{2},12.5-15:{3},gt15:{4} | TF 5:{5},15:{6},60:{7},240:{8},1440:{9}",
                band_lt8, band_8_10, band_10_12_5, band_12_5_15, band_gt15, tf_5, tf_15, tf_60, tf_240, tf_1440));
            // Resumen RR planned por bandas 0-10 y 10-15
            _logger.Info(string.Format("[DIAGNOSTICO][Risk] RRPlanBands: 0-10={0:F2}(n={1}),10-15={2:F2}(n={3})",
                rr_n_0_10 > 0 ? rr_sum_0_10 / rr_n_0_10 : 0.0, rr_n_0_10,
                rr_n_10_15 > 0 ? rr_sum_10_15 / rr_n_10_15 : 0.0, rr_n_10_15));
            _logger.Info(string.Format("[DIAG][Risk] RRPlanBands: 0-10={0:F2}(n={1}),10-15={2:F2}(n={3})",
                rr_n_0_10 > 0 ? rr_sum_0_10 / rr_n_0_10 : 0.0, rr_n_0_10,
                rr_n_10_15 > 0 ? rr_sum_10_15 / rr_n_10_15 : 0.0, rr_n_10_15));
        }

        /// <summary>
        /// Calcula Entry, SL, TP y PositionSize para una HeatZone usando ESTRUCTURA INTELIGENTE
        /// Añade los resultados a zone.Metadata
        /// </summary>
        private void CalculateStructuralRiskLevels(HeatZone zone, DecisionSnapshot snapshot, IBarDataProvider barData, CoreEngine coreEngine, int currentBar, double accountSize)
        {
            // Alinear al tiempo de análisis: usar currentBar del pipeline, no el último índice
            int decisionTF = _config.DecisionTimeframeMinutes;
            DateTime analysisTime = barData.GetBarTime(decisionTF, currentBar);
            int idxDom = barData.GetBarIndexFromTime(zone.TFDominante, analysisTime);
            if (idxDom < 0)
            {
                zone.Metadata["RiskCalculated"] = false;
                zone.Metadata["RejectReason"] = "NoDataAtAnalysisTime";
                _logger.Info($"[DIAGNOSTICO][Risk] REJECT: NoDataAtAnalysisTime Zone={zone.Id} TF={zone.TFDominante} Time={analysisTime:yyyy-MM-dd HH:mm}");
                return;
            }



            // Obtener ATR del TF Dominante (firma correcta)
            double atr = barData.GetATR(zone.TFDominante, 14, idxDom);
            if (atr <= 0)
            {
                _logger.Warning(string.Format("[RiskCalculator] ATR({0}) es 0 para HeatZone {1}, usando ATR=1.0",
                    zone.TFDominante, zone.Id));
                atr = 1.0;
            }

            // GATE REMOVIDO: Movido a TradeManager (línea 142) para permitir procesamiento de todas las zonas
            // Calculamos distancia para logging y análisis, pero NO rechazamos aquí
            // TradeManager decidirá si ejecutar basándose en proximidad actual del precio
            double currentPriceGate = barData.GetClose(zone.TFDominante, idxDom);
            double entryStructuralGate = zone.Direction == "Bullish" ? zone.Low : zone.High;
            double distanceToEntryGate = Math.Abs(currentPriceGate - entryStructuralGate);
            double distanceATRGate = distanceToEntryGate / atr;
            
            // Guardar distancia en metadata para uso posterior (TradeManager, análisis)
            zone.Metadata["DistanceToEntry_Points"] = distanceToEntryGate;
            zone.Metadata["DistanceToEntry_ATR"] = distanceATRGate;

            // VALIDACIÓN CRÍTICA: Verificar proximidad del Entry SOLO en tiempo real (no en backtest histórico)
            // En backtest, necesitamos ver TODAS las señales históricas para auditar el sistema
            if (!barData.IsHistorical)
            {
                double currentPrice = barData.GetClose(zone.TFDominante, idxDom);
                double entryStructural = zone.Direction == "Bullish" ? zone.Low : zone.High;
                double distanceToEntry = Math.Abs(currentPrice - entryStructural);
                double maxDistanceAllowed = _config.MaxEntryProximityFactor * atr; // Configurable (default 15.0 × ATR)

                if (distanceToEntry > maxDistanceAllowed)
                {
                    _logger.Warning(string.Format(
                        "[RiskCalculator] ⚠ HeatZone {0} RECHAZADA (Realtime): Entry estructural @ {1:F2} está demasiado lejos del precio actual {2:F2} (Distancia={3:F2}, Max={4:F2})",
                        zone.Id, entryStructural, currentPrice, distanceToEntry, maxDistanceAllowed));
                    
                    zone.Metadata["RiskCalculated"] = false;
                    zone.Metadata["RejectReason"] = "Entry demasiado lejos del precio actual (oportunidad perdida)";
                    return;
                }
            }

            double entry, stopLoss, takeProfit;
            double tickSize = barData.GetTickSize(); // Declarar una sola vez (V5.7d)
            // Régimen para selección de bandas y gates posteriores
            string regime = snapshot.MarketRegime ?? "Normal";
            bool isHighVol = (regime == "HighVol");

            // Calcular Entry, SL, TP según la dirección de la zona
            if (zone.Direction == "Bullish")
            {
                // BUY Limit Order: Entry anclado a estructura dominante + snap conservador (V5.7d)
                var dominantStructure = coreEngine.GetStructureById(zone.DominantStructureId);
                
                double entryRaw;
                if (dominantStructure != null)
                {
                    // Entry desde estructura dominante
                    entryRaw = dominantStructure.Low;

                    // Metadata de fuente de Entry
                    zone.Metadata["EntrySourceId"] = dominantStructure.Id;
                    zone.Metadata["EntrySourceTF"] = dominantStructure.TF;
                    zone.Metadata["EntrySourceType"] = dominantStructure.GetType().Name;
                }
                else
                {
                    // Fallback a envolvente si no existe (robustez)
                    entryRaw = zone.Low;
                    _logger.Warning(string.Format("[RiskCalculator] HZ={0} DominantStructure not found, using zone envelope", zone.Id));
                    zone.Metadata["EntrySourceType"] = "Envelope";
                }
                
                // Snap conservador a tick (BUY: redondear arriba)
                entry = Math.Ceiling(entryRaw / tickSize) * tickSize;
                
                // Logging de trazabilidad
                double snapDelta = Math.Abs(entry - entryRaw);
                int snapDeltaTicks = (int)Math.Round(snapDelta / tickSize);
                
                if (dominantStructure != null)
                {
                    _logger.Info(string.Format("[RiskCalculator] HZ={0} Entry: Raw={1:F2} Snapped={2:F2} SnapDelta={3} ticks | Source={4} TF={5} Score={6:F2}",
                        zone.Id, entryRaw, entry, snapDeltaTicks, dominantStructure.GetType().Name, dominantStructure.TF, dominantStructure.Score));
                }
                
                // Penalización suave por EDAD del ENTRY (threshold dinámico por TF) - NO rechaza
                if (coreEngine != null && !string.IsNullOrEmpty(zone.DominantStructureId))
                {
                    var entryStruct = coreEngine.GetStructureById(zone.DominantStructureId);
                    if (entryStruct != null)
                    {
                        DateTime entryCreatedTime = barData.GetBarTime(entryStruct.TF, entryStruct.CreatedAtBarIndex);
                        int entryCreatedIdxInDecision = barData.GetBarIndexFromTime(decisionTF, entryCreatedTime);
                        if (entryCreatedIdxInDecision >= 0 && currentBar >= entryCreatedIdxInDecision)
                        {
                            int entryAgeBars = currentBar - entryCreatedIdxInDecision;
                            double entryAgeHours = (entryAgeBars * decisionTF) / 60.0;

                            double maxEntryAge =
                                (entryStruct.TF <= 15) ? 12.0 :
                                (entryStruct.TF <= 60) ? 24.0 : 72.0;

                            double agePenalty = entryAgeHours > maxEntryAge ? 0.85 : 1.0;
                            zone.Metadata["EntryAgeHours"] = entryAgeHours;
                            zone.Metadata["EntryAgePenalty"] = agePenalty;
                            _logger.Debug($"[RISK][ENTRY_AGE] Zone={zone.Id} Struct={entryStruct.Id} TF={entryStruct.TF} AgeH={entryAgeHours:F1} MaxH={maxEntryAge:F1} Penalty={agePenalty:F2}");
                        }
                    }
                }

                // SL ESTRUCTURAL: Buscar Swing Low protector
                stopLoss = CalculateStructuralSL_Buy(zone, coreEngine, barData, currentBar, atr, isHighVol);
                if (double.IsNaN(stopLoss))
                {
                    zone.Metadata["RiskCalculated"] = false;
                    zone.Metadata["RejectReason"] = "NO_SL_STRUCTURAL";
                    _logger.Info($"[RISK][NO_SL] Zone={zone.Id} Dir=BUY → REJECT (no SL estructural válido)");
                    return;
                }

                // ✅ ELIMINADO filtro arbitrario "RejectLowTFTightSL" (SL < 10 ATR)
                // 10 ATR es swing trading, no intradía. El scoring inteligente ya decide el mejor SL
                
                // TP JERÁRQUICO: Buscar objetivo estructural
                takeProfit = CalculateStructuralTP_Buy(zone, snapshot, coreEngine, barData, currentBar, entry, stopLoss, isHighVol);
            }
            else if (zone.Direction == "Bearish")
            {
                // SELL Limit Order: Entry anclado a estructura dominante + snap conservador (V5.7d)
                var dominantStructure = coreEngine.GetStructureById(zone.DominantStructureId);
                
                double entryRaw;
                if (dominantStructure != null)
                {
                    // Entry desde estructura dominante (SELL: usar High = borde superior de la zona)
                    entryRaw = dominantStructure.High;

                    // Metadata de fuente de Entry
                    zone.Metadata["EntrySourceId"] = dominantStructure.Id;
                    zone.Metadata["EntrySourceTF"] = dominantStructure.TF;
                    zone.Metadata["EntrySourceType"] = dominantStructure.GetType().Name;
                }
                else
                {
                    // Fallback a envolvente si no existe (robustez)
                    entryRaw = zone.High;
                    _logger.Warning(string.Format("[RiskCalculator] HZ={0} DominantStructure not found, using zone envelope", zone.Id));
                    zone.Metadata["EntrySourceType"] = "Envelope";
                }
                
                // Snap conservador a tick (SELL: redondear abajo)
                entry = Math.Floor(entryRaw / tickSize) * tickSize;
                
                // Logging de trazabilidad
                double snapDelta = Math.Abs(entry - entryRaw);
                int snapDeltaTicks = (int)Math.Round(snapDelta / tickSize);
                
                if (dominantStructure != null)
                {
                    _logger.Info(string.Format("[RiskCalculator] HZ={0} Entry: Raw={1:F2} Snapped={2:F2} SnapDelta={3} ticks | Source={4} TF={5} Score={6:F2}",
                        zone.Id, entryRaw, entry, snapDeltaTicks, dominantStructure.GetType().Name, dominantStructure.TF, dominantStructure.Score));
                }
                
                // Penalización suave por EDAD del ENTRY (threshold dinámico por TF) - NO rechaza
                if (coreEngine != null && !string.IsNullOrEmpty(zone.DominantStructureId))
                {
                    var entryStruct = coreEngine.GetStructureById(zone.DominantStructureId);
                    if (entryStruct != null)
                    {
                        DateTime entryCreatedTime = barData.GetBarTime(entryStruct.TF, entryStruct.CreatedAtBarIndex);
                        int entryCreatedIdxInDecision = barData.GetBarIndexFromTime(decisionTF, entryCreatedTime);
                        if (entryCreatedIdxInDecision >= 0 && currentBar >= entryCreatedIdxInDecision)
                        {
                            int entryAgeBars = currentBar - entryCreatedIdxInDecision;
                            double entryAgeHours = (entryAgeBars * decisionTF) / 60.0;

                            double maxEntryAge =
                                (entryStruct.TF <= 15) ? 12.0 :
                                (entryStruct.TF <= 60) ? 24.0 : 72.0;

                            double agePenalty = entryAgeHours > maxEntryAge ? 0.85 : 1.0;
                            zone.Metadata["EntryAgeHours"] = entryAgeHours;
                            zone.Metadata["EntryAgePenalty"] = agePenalty;
                            _logger.Debug($"[RISK][ENTRY_AGE] Zone={zone.Id} Struct={entryStruct.Id} TF={entryStruct.TF} AgeH={entryAgeHours:F1} MaxH={maxEntryAge:F1} Penalty={agePenalty:F2}");
                        }
                    }
                }

                // SL ESTRUCTURAL: Buscar Swing High protector
                stopLoss = CalculateStructuralSL_Sell(zone, coreEngine, barData, currentBar, atr, isHighVol);
                if (double.IsNaN(stopLoss))
                {
                    zone.Metadata["RiskCalculated"] = false;
                    zone.Metadata["RejectReason"] = "NO_SL_STRUCTURAL";
                    _logger.Info($"[RISK][NO_SL] Zone={zone.Id} Dir=SELL → REJECT (no SL estructural válido)");
                    return;
                }

                // ✅ ELIMINADO filtro arbitrario "RejectLowTFTightSL" (SL < 10 ATR)
                // 10 ATR es swing trading, no intradía. El scoring inteligente ya decide el mejor SL
                
                // TP JERÁRQUICO: Buscar objetivo estructural
                takeProfit = CalculateStructuralTP_Sell(zone, snapshot, coreEngine, barData, currentBar, entry, stopLoss, isHighVol);
            }
            else
            {
                // Zona Neutral: No se puede calcular riesgo
                _logger.Debug(string.Format("[RiskCalculator] HeatZone {0} es Neutral, no se calcula riesgo", zone.Id));
                zone.Metadata["RiskCalculated"] = false;
                return;
            }

            // ✅ Guardar Entry/SL/TP en metadata para análisis posterior (rejected signals, MFE/MAE)
            zone.Metadata["Entry"] = entry;
            zone.Metadata["SL"] = stopLoss;
            zone.Metadata["TP"] = takeProfit;

            // Calcular PositionSize
            double pointValue = barData.GetPointValue();
            
            if (pointValue <= 0 || tickSize <= 0)
            {
                _logger.Warning("[RiskCalculator] PointValue o TickSize inválidos, usando defaults");
                pointValue = 50.0; // Default para ES
                tickSize = 0.25;
            }

            // Riesgo por contrato = |Entry - SL| * PointValue
            double riskPerContract = Math.Abs(entry - stopLoss) * pointValue;

            // Riesgo de cuenta = AccountSize * (RiskPercentPerTrade / 100)
            double accountRisk = accountSize * (_config.RiskPercentPerTrade / 100.0);

            // PositionSize = accountRisk / riskPerContract
            double positionSize = riskPerContract > 0 ? accountRisk / riskPerContract : 0.0;

            // Redondear a contratos enteros (mínimo 1)
            int positionSizeContracts = Math.Max(1, (int)Math.Floor(positionSize));

            // Calcular R:R real
            double rewardDistance = Math.Abs(takeProfit - entry);
            double riskDistance = Math.Abs(entry - stopLoss);
            double actualRR = riskDistance > 0 ? rewardDistance / riskDistance : 0.0;
            
            // V6.0c-FIX: Usar ATR del TF del swing seleccionado, no del TF dominante de la zona
            // Si SL/TP son de TFs altos (240/1440), el ATR debe ser del TF alto
            double atrForSL = atr;  // default: TF dominante
            double atrForTP = atr;
            
            // Declarar variables una sola vez
            int slTF = zone.TFDominante;  // default
            int tpTF = zone.TFDominante;  // default
            
            // Obtener TF del SL seleccionado
            if (zone.Metadata.ContainsKey("SL_SwingTF"))
            {
                slTF = (int)zone.Metadata["SL_SwingTF"];
                if (slTF > 0 && slTF != zone.TFDominante)
                {
                    int idxSL = barData.GetBarIndexFromTime(slTF, analysisTime);
                    if (idxSL >= 0)
                    {
                        atrForSL = barData.GetATR(slTF, 14, idxSL);
                        if (atrForSL <= 0) atrForSL = atr;
                        _logger.Debug($"[RiskCalculator] Zone={zone.Id} SL TF={slTF} (dom={zone.TFDominante}): ATR_dom={atr:F2} → ATR_SL={atrForSL:F2}");
                    }
                }
            }
            
            // Obtener TF del TP seleccionado
            if (zone.Metadata.ContainsKey("TP_TargetTF"))
            {
                tpTF = (int)zone.Metadata["TP_TargetTF"];
                if (tpTF > 0 && tpTF != zone.TFDominante)
                {
                    int idxTP = barData.GetBarIndexFromTime(tpTF, analysisTime);
                    if (idxTP >= 0)
                    {
                        atrForTP = barData.GetATR(tpTF, 14, idxTP);
                        if (atrForTP <= 0) atrForTP = atr;
                        _logger.Debug($"[RiskCalculator] Zone={zone.Id} TP TF={tpTF} (dom={zone.TFDominante}): ATR_dom={atr:F2} → ATR_TP={atrForTP:F2}");
                    }
                }
            }
            
            double slDistanceATR = riskDistance / atrForSL;
            double tpDistanceATR = rewardDistance / atrForTP;
            
            // Trazas de auditoría ATR multi-TF (V6.0c-bis)
            if (slTF != zone.TFDominante || tpTF != zone.TFDominante)
            {
                _logger.Info($"[RISK][ATR_MULTI] Zone={zone.Id} DomTF={zone.TFDominante} ATRdom={atr:F2} | SL: TF={slTF} ATR={atrForSL:F2} Dist={slDistanceATR:F2} | TP: TF={tpTF} ATR={atrForTP:F2} Dist={tpDistanceATR:F2}");
            }
            
            // ========================================================================
            // V6.0i.9: GATE POR DISTANCIA AL ENTRY (HighVol) - Versión Final
            // ========================================================================
            // Obtener precio actual del TF dominante
            double currentPriceForGate = barData.GetClose(zone.TFDominante, idxDom);
            
            // Calcular distancia al Entry en puntos
            double entryDistancePoints = Math.Abs(currentPriceForGate - entry);
            
            // Determinar régimen para aplicar gate (ya calculado arriba como 'regime' e 'isHighVol')
            
            // ATR dominante (línea 182): double atr = barData.GetATR(zone.TFDominante, 14, idxDom);
            double atrDom = atr;
            
            if (idxDom < 14 || atrDom <= 0 || atrDom == 1.0)
            {
                _logger.Info(string.Format(
                    "[DIAGNOSTICO_ATR] Zone={0} TFDom={1} idxDom={2} ATR={3:F2} decisionTF={4} currentBar={5} analysisTime={6:O}",
                    zone.Id, zone.TFDominante, idxDom, atr, decisionTF, currentBar, analysisTime));
            }
            
            // Gate condicional: solo si ATR dominante es válido y NO es fallback 1.0
            if (idxDom >= 14 && atrDom > 0 && atrDom != 1.0)
            {
                // Calcular DistATR con ATR dominante (semántica correcta)
                double entryDistanceATR = entryDistancePoints / atrDom;
                
                // Guardar en metadata para [SIGNAL_METRICS] y análisis
                zone.Metadata["ATRdom"] = atrDom;
                zone.Metadata["DistanceToEntry_Points"] = entryDistancePoints;
                zone.Metadata["DistanceToEntry_ATR"] = entryDistanceATR;
                
                // Gate solo en HighVol: rechazar señales nacidas muy lejas
                if (isHighVol)
                {
                    double maxEntryDistATR = _config.MaxDistanceToEntry_ATR_HighVol; // 1.0
                    
                    if (entryDistanceATR > maxEntryDistATR)
                    {
                        // Clasificar por bucket para calibración
                        string bucket = "";
                        if (entryDistanceATR <= 0.6) bucket = "≤0.6";
                        else if (entryDistanceATR <= 1.0) bucket = "0.6-1.0";
                        else if (entryDistanceATR <= 2.0) bucket = "1.0-2.0";
                        else bucket = ">2.0";
                        
                        zone.Metadata["RiskCalculated"] = false;
                        zone.Metadata["RejectReason"] = $"ENTRY_TOO_FAR_HV: {entryDistanceATR:F2} ATR > {maxEntryDistATR:F2} (bucket={bucket})";
                        
                        _logger.Info($"[RISK][ENTRY_TOO_FAR] Zone={zone.Id} Regime={regime} Entry={entry:F2} Current={currentPriceForGate:F2} DistATR={entryDistanceATR:F2} > {maxEntryDistATR:F2} Bucket={bucket} REJECTED");
                        return;
                    }
                    else
                    {
                        // Log de aceptación para análisis de distribución
                        _logger.Debug($"[RISK][ENTRY_OK] Zone={zone.Id} Regime={regime} Entry={entry:F2} Current={currentPriceForGate:F2} DistATR={entryDistanceATR:F2} <= {maxEntryDistATR:F2} ACCEPTED");
                    }
                }
            }
            else
            {
                // ATR no válido: NO aplicar gate, guardar metadata con -1 para diagnóstico
                zone.Metadata["ATRdom"] = -1.0;
                zone.Metadata["DistanceToEntry_Points"] = entryDistancePoints;
                zone.Metadata["DistanceToEntry_ATR"] = -1.0;
                
                // Opcional: ATR del TF decisión para logging/diagnóstico (NO usado en decisión)
                double atrFallbackForLog = barData.GetATR(decisionTF, 14, currentBar);
                zone.Metadata["ATRdom_fallback"] = atrFallbackForLog;
                zone.Metadata["DistATR_fallback"] = (atrFallbackForLog > 0) ? entryDistancePoints / atrFallbackForLog : -1.0;
                
                _logger.Debug($"[RISK][ENTRY_NO_GATE] Zone={zone.Id} Regime={regime} idxDom={idxDom} atrDom={atrDom:F2} - ATR no válido, gate desactivado");
            }
            
            // ========================================================================
            // V6.0i.3b: HARD CAP SL + Gate AND solo en TP
            // ========================================================================
            
            double slDistancePoints = riskDistance;
            double tpDistancePoints = rewardDistance;

            // Régimen ya declarado arriba (línea 407), reutilizar variables

            // Límites por régimen (reutiliza propiedades existentes de EngineConfig)
            double maxSLPoints = isHighVol ? _config.MaxSLDistancePoints_HighVol : _config.MaxSLDistancePoints_Normal;
            double maxSLATR    = _config.MaxSLDistanceATR;
            double maxTPPoints = isHighVol ? _config.MaxTPDistancePoints_HighVol : _config.MaxTPDistancePoints_Normal;
            double maxTPATR    = isHighVol ? _config.MaxTPDistanceATR_HighVol    : _config.MaxTPDistanceATR;

            // Tolerancia (5%) solo para ATR, NO para puntos en SL
            double tol = 1.0 + _config.ValidationTolerancePercent;
            double tolSLATR    = maxSLATR * tol;
            double tolTPPoints = maxTPPoints * tol;
            double tolTPATR    = maxTPATR * tol;

            // ========================================================================
            // ✅ HARD CAP por puntos ELIMINADO - El scoring multi-factor ya selecciona el mejor SL
            // El sistema es inteligente y prioriza: calidad estructural, recencia, distancia óptima, TF
            // Si el SL es "grande", es porque es el swing protector real del mercado
            // ========================================================================

            // ========================================================================
            // 1) Validación SL-ATR con tolerancia (Gate OR)
            // ========================================================================
            if (slDistanceATR > tolSLATR)
            {
                zone.Metadata["RiskCalculated"] = false;
                zone.Metadata["RejectReason"] = $"SL_ATR>{maxSLATR}ATR";
                _logger.Info($"[RISK][SL_CHECK_FAIL] Zone={zone.Id} Regime={regime} SL_ATR={slDistanceATR:F2}>{tolSLATR:F2}ATR (with tol) REJECTED");
                return;
            }
            else if (slDistanceATR > maxSLATR && slDistanceATR <= tolSLATR)
            {
                _logger.Info($"[RISK][NEAR_LIMIT] Zone={zone.Id} Regime={regime} SL={slDistancePoints:F2}pts/{slDistanceATR:F2}ATR Limits={maxSLPoints:F2}pts/{maxSLATR:F2}ATR ACCEPTED");
            }

            // ========================================================================
            // 3) Gate AND + tolerancia para TP (solo en HighVol)
            // ========================================================================
            bool tpExPts = tpDistancePoints > tolTPPoints;
            bool tpExATR = tpDistanceATR > tolTPATR;

            // Gate: AND en HighVol, OR en Normal
            bool rejectTP = isHighVol ? (tpExATR && tpExPts) : (tpExATR || tpExPts);

            if (rejectTP)
            {
                zone.Metadata["RiskCalculated"] = false;
                zone.Metadata["RejectReason"] = isHighVol
                    ? $"TP_CHECK_FAIL HighVol: {tpDistancePoints:F2}>{maxTPPoints:F2}pts AND {tpDistanceATR:F2}>{maxTPATR:F2}ATR"
                    : $"TP_CHECK_FAIL Normal: {tpDistancePoints:F2}>{maxTPPoints:F2}pts OR {tpDistanceATR:F2}>{maxTPATR:F2}ATR";
                _logger.Info($"[RISK][TP_CHECK_FAIL] Zone={zone.Id} Regime={regime} Gate={(isHighVol ? "AND" : "OR")} TP={tpDistancePoints:F2}pts/{tpDistanceATR:F2}ATR Limits={maxTPPoints:F2}pts/{maxTPATR:F2}ATR REJECTED");
                return;
            }
            else if (tpExPts || tpExATR)
            {
                _logger.Info($"[RISK][NEAR_LIMIT] Zone={zone.Id} Regime={regime} TP={tpDistancePoints:F2}pts/{tpDistanceATR:F2}ATR Limits={maxTPPoints:F2}pts/{maxTPATR:F2}ATR ACCEPTED");
            }

            // Trazas de auditoría cuando pasan validaciones
            _logger.Debug($"[RISK][SL_CHECK_PASS] Zone={zone.Id} Regime={regime} SL: {slDistancePoints:F2}pts/{slDistanceATR:F2}ATR (Limit={maxSLPoints}pts/{maxSLATR}ATR) TF={slTF}");
            _logger.Debug($"[RISK][TP_CHECK_PASS] Zone={zone.Id} Regime={regime} TP: {tpDistancePoints:F2}pts/{tpDistanceATR:F2}ATR (Limit={maxTPPoints}pts/{maxTPATR}ATR) TF={tpTF}");
            
            // Guardar drivers SIEMPRE (antes de posibles returns por rechazo)
            zone.Metadata["SLDistanceATR"] = slDistanceATR;
            zone.Metadata["TPDistanceATR"] = tpDistanceATR;

            // ========================================================================
            // VALIDACIONES ADICIONALES (R:R mínimo)
            // ========================================================================
            // ✅ ELIMINADO filtro arbitrario MinTPDistanceATR - El scoring inteligente ya decide el mejor TP
            // Los límites de puntos (25/35) ya evitan TPs absurdos
            
            // 1. Validar R:R mínimo
            if (actualRR < _config.MinRiskRewardRatio)
            {
                _logger.Warning(string.Format(
                    "[RiskCalculator] ⚠ HeatZone {0} RECHAZADA: R:R insuficiente ({1:F2} < mínimo {2:F2})",
                    zone.Id, actualRR, _config.MinRiskRewardRatio));
                
                zone.Metadata["RiskCalculated"] = false;
                zone.Metadata["RejectReason"] = string.Format("R:R absurdo: {0:F2} (mínimo: {1:F2})", actualRR, _config.MinRiskRewardRatio);
                return;
            }

            // 3b. Guard adicional: rechazar FALLBACK (P4) con proximidad baja
            // Criterio: TP no estructural (P4_Fallback) y ProximityFactor < 0.30
            bool isFallbackTP = zone.Metadata.ContainsKey("TP_Structural") && !(bool)zone.Metadata["TP_Structural"];
            double proximityFactor = zone.Metadata.ContainsKey("ProximityFactor") ? (double)zone.Metadata["ProximityFactor"] : 0.0;
            if (isFallbackTP && proximityFactor < 0.30)
            {
                _logger.Info(string.Format("[DIAGNOSTICO][Risk] RejFallbackLowProx: Zone={0} Prox={1:F3} (<0.30)", zone.Id, proximityFactor));
                zone.Metadata["RiskCalculated"] = false;
                zone.Metadata["RejectReason"] = string.Format("Fallback con proximidad baja: {0:F2} (<0.30)", proximityFactor);
                return;
            }
            // 3c. Guard adicional V6.0j: Fallback (P4) solo si RR >= 1.80 y TPDistATR <= 2.5
            if (isFallbackTP && (actualRR < 1.80 || tpDistanceATR > 2.5))
            {
                _logger.Info(string.Format("[DIAGNOSTICO][Risk] RejFallbackRR/Dist: Zone={0} RR={1:F2} (<1.80) OR TPDistATR={2:F2} (>2.5)", zone.Id, actualRR, tpDistanceATR));
                zone.Metadata["RiskCalculated"] = false;
                zone.Metadata["RejectReason"] = string.Format("Fallback no apto: RR={0:F2} TPDistATR={1:F2}", actualRR, tpDistanceATR);
                return;
            }

            // Añadir a Metadata
            zone.Metadata["Entry"] = entry;
            zone.Metadata["StopLoss"] = stopLoss;
            zone.Metadata["TakeProfit"] = takeProfit;
            zone.Metadata["PositionSizeContracts"] = positionSizeContracts;
            zone.Metadata["RiskPerContract"] = riskPerContract;
            zone.Metadata["AccountRisk"] = accountRisk;
            zone.Metadata["ActualRR"] = actualRR;
            zone.Metadata["SLDistanceATR"] = slDistanceATR;
            zone.Metadata["TPDistanceATR"] = tpDistanceATR;
            zone.Metadata["RiskCalculated"] = true;

            _logger.Debug(string.Format(
                "[RiskCalculator] HeatZone {0} ({1}): Entry={2:F2}, SL={3:F2}, TP={4:F2}, Size={5}, R:R={6:F2}",
                zone.Id, zone.Direction, entry, stopLoss, takeProfit, positionSizeContracts, actualRR
            ));

            // Log diagnóstico de aceptación para análisis WR vs SLDistATR (A+)
            double proxAccepted = zone.Metadata.ContainsKey("ProximityFactor") ? (double)zone.Metadata["ProximityFactor"] : 0.0;
            // Extraer confianza final si está disponible en Metadata
            double finalConf = 0.0;
            try
            {
                if (zone.Metadata.ContainsKey("ConfidenceBreakdown"))
                {
                    var breakdown = zone.Metadata["ConfidenceBreakdown"] as DecisionScoreBreakdown;
                    if (breakdown != null)
                        finalConf = breakdown.FinalConfidence;
                }
                else if (zone.Metadata.ContainsKey("FinalConfidence"))
                {
                    finalConf = (double)zone.Metadata["FinalConfidence"];
                }
            }
            catch { /* fallback 0.0 */ }

            _logger.Info(string.Format(
                "[DIAGNOSTICO][Risk] SLAccepted: Zone={0} Dir={1} Entry={2:F2} SL={3:F2} TP={4:F2} SLDistATR={5:F2} Prox={6:F3} Core={7:F3} RR={8:F2} Conf={9:F2}",
                zone.Id, zone.Direction, entry, stopLoss, takeProfit, slDistanceATR, proxAccepted, zone.Score, actualRR, finalConf));

            // Log de detalle de calidad de zona aceptada
            bool alignedWithBias = zone.Metadata.ContainsKey("AlignedWithBias") && (bool)zone.Metadata["AlignedWithBias"];
            int slTf = zone.Metadata.ContainsKey("SL_SwingTF") ? (int)zone.Metadata["SL_SwingTF"] : -1;
            bool slStruct = zone.Metadata.ContainsKey("SL_Structural") && (bool)zone.Metadata["SL_Structural"];
            int tpTf = zone.Metadata.ContainsKey("TP_TargetTF") ? (int)zone.Metadata["TP_TargetTF"] : -1;
            bool tpStruct = zone.Metadata.ContainsKey("TP_Structural") && (bool)zone.Metadata["TP_Structural"];
            double proxF = zone.Metadata.ContainsKey("ProximityFactor") ? (double)zone.Metadata["ProximityFactor"] : 0.0;
            double confContribution = 0.0;
            double confluenceScoreRaw = zone.Metadata.ContainsKey("ConfluenceScore") ? (double)zone.Metadata["ConfluenceScore"] : 0.0;
            try
            {
                if (zone.Metadata.ContainsKey("ConfidenceBreakdown"))
                {
                    var bd = zone.Metadata["ConfidenceBreakdown"] as DecisionScoreBreakdown;
                    if (bd != null) confContribution = bd.ConfluenceContribution;
                }
            }
            catch { }
            _logger.Info(string.Format(
                "[DIAGNOSTICO][Risk] SLAccepted DETAIL: Zone={0} Dir={1} Aligned={2} Core={3:F2} Prox={4:F2} ConfC={5:F2} ConfScore={6:F2} SL_TF={7} SL_Struct={8} TP_TF={9} TP_Struct={10} RR={11:F2} Confidence={12:F2}",
                zone.Id, zone.Direction, alignedWithBias, zone.Score, proxF, confContribution, confluenceScoreRaw, slTf, slStruct, tpTf, tpStruct, actualRR, finalConf));
            
            // ✅ PHANTOM TRACKING: Log para análisis MFE/MAE de TODAS las zonas procesadas
            // Esto permite analizar retrospectivamente qué habría pasado con zonas no ejecutadas por TradeManager
            double distATR = zone.Metadata.ContainsKey("DistanceToEntry_ATR") ? (double)zone.Metadata["DistanceToEntry_ATR"] : -1.0;
            _logger.Info(string.Format(
                "[PHANTOM_OPPORTUNITY] Zone={0} Dir={1} Entry={2:F2} SL={3:F2} TP={4:F2} RR={5:F2} DistATR={6:F2} TF={7} Bar={8} Time={9:yyyy-MM-dd HH:mm:ss}",
                zone.Id, zone.Direction, entry, stopLoss, takeProfit, actualRR, distATR, zone.TFDominante, currentBar, analysisTime));
        }

        /// <summary>
        /// Modo fallback cuando no hay CoreEngine disponible: calcula Entry/SL/TP con reglas conservadoras
        /// - Entry: envolvente de la zona (Low para BUY, High para SELL) con snap a tick
        /// - SL: buffer configurable sobre ATR (SL_BufferATR)
        /// - TP: distancia mínima según MinRiskRewardRatio
        /// Aplica las mismas validaciones que el cálculo estructural
        /// </summary>
        private void CalculateFallbackRiskLevels(HeatZone zone, IBarDataProvider barData, int currentBar, int timeframeMinutes, double accountSize)
        {
            double atr = barData.GetATR(timeframeMinutes, currentBar, 14);
            if (atr <= 0)
            {
                _logger.Warning($"[RiskCalculator] ATR({timeframeMinutes}) es 0 para HeatZone {zone.Id}, usando ATR=1.0");
                atr = 1.0;
            }

            double tickSize = barData.GetTickSize();
            double entry;
            double stopLoss;
            double takeProfit;

            if (zone.Direction == "Bullish")
            {
                double entryRaw = zone.Low;
                entry = Math.Ceiling(entryRaw / tickSize) * tickSize;
                double riskDist = Math.Max(_config.MinSLDistanceATR, _config.SL_BufferATR) * atr;
                stopLoss = entry - riskDist;
                double rewardDist = Math.Max(_config.MinRiskRewardRatio, 1.0) * riskDist;
                takeProfit = entry + rewardDist;
            }
            else if (zone.Direction == "Bearish")
            {
                double entryRaw = zone.High;
                entry = Math.Floor(entryRaw / tickSize) * tickSize;
                double riskDist = Math.Max(_config.MinSLDistanceATR, _config.SL_BufferATR) * atr;
                stopLoss = entry + riskDist;
                double rewardDist = Math.Max(_config.MinRiskRewardRatio, 1.0) * riskDist;
                takeProfit = entry - rewardDist;
            }
            else
            {
                zone.Metadata["RiskCalculated"] = false;
                zone.Metadata["RejectReason"] = "Zona neutral";
                return;
            }

            // Reutilizar pipeline de validaciones y cálculos finales
            double pointValue = barData.GetPointValue();
            if (pointValue <= 0 || tickSize <= 0)
            {
                pointValue = 50.0;
                tickSize = 0.25;
            }

            double riskDistance = Math.Abs(entry - stopLoss);
            double rewardDistance = Math.Abs(takeProfit - entry);
            double slDistanceATR = riskDistance / atr;
            double tpDistanceATR = rewardDistance / atr;
            zone.Metadata["SLDistanceATR"] = slDistanceATR;
            zone.Metadata["TPDistanceATR"] = tpDistanceATR;

            if (slDistanceATR > _config.MaxSLDistanceATR)
            {
                zone.Metadata["RiskCalculated"] = false;
                zone.Metadata["RejectReason"] = $"SL absurdo: {slDistanceATR:F2} ATR (límite: {_config.MaxSLDistanceATR:F2})";
                return;
            }

            // ✅ ELIMINADO filtro MinTPDistanceATR también en fallback mode
            // El scoring inteligente y límites de puntos ya controlan TPs

            double accountRisk = accountSize * (_config.RiskPercentPerTrade / 100.0);
            double riskPerContract = riskDistance * pointValue;
            double positionSize = riskPerContract > 0 ? accountRisk / riskPerContract : 0.0;
            int positionSizeContracts = Math.Max(1, (int)Math.Floor(positionSize));

            double actualRR = riskDistance > 0 ? (rewardDistance / riskDistance) : 0.0;
            if (actualRR < _config.MinRiskRewardRatio)
            {
                zone.Metadata["RiskCalculated"] = false;
                zone.Metadata["RejectReason"] = $"R:R absurdo: {actualRR:F2} (mínimo: {_config.MinRiskRewardRatio:F2})";
                return;
            }

            zone.Metadata["Entry"] = entry;
            zone.Metadata["SL"] = stopLoss;
            zone.Metadata["TP"] = takeProfit;
            // Alias esperados por tests y otras rutas
            zone.Metadata["StopLoss"] = stopLoss;
            zone.Metadata["TakeProfit"] = takeProfit;
            zone.Metadata["PositionSizeContracts"] = positionSizeContracts;
            zone.Metadata["ActualRR"] = actualRR;
            // Metadatos de coherencia con cálculo estructural
            zone.Metadata["SL_Structural"] = false;
            zone.Metadata["TP_Structural"] = false;
            zone.Metadata["SL_SwingTF"] = timeframeMinutes;
            zone.Metadata["SL_TF"] = timeframeMinutes; // alias esperado por algunos tests
            zone.Metadata["TP_TF"] = timeframeMinutes;
            zone.Metadata["TP_Struct"] = false; // alias esperado por algunos tests
            zone.Metadata["SLPickDistATR"] = slDistanceATR;
            zone.Metadata["RiskCalculated"] = true;
        }

        // ========================================================================
        // CÁLCULO DE SL ESTRUCTURAL (SWING-BASED)
        // ========================================================================

        /// <summary>
        /// Calcula SL estructural para BUY: Swing Low protector - (ATR * buffer)
        /// Usa exclusivamente SL estructural (sin MinSL artificial)
        /// </summary>
        private double CalculateStructuralSL_Buy(HeatZone zone, CoreEngine coreEngine, IBarDataProvider barData, int currentBar, double atr, bool isHighVol)
        {
            double entry = zone.Low;

            // Buscar Swing Low protector (debajo de la zona)
            var swingLowPick = FindProtectiveSwingLowBanded(zone, coreEngine, barData, atr, entry, true, isHighVol);
            var swingLow = swingLowPick != null ? swingLowPick.Item1 : null;
            int swingTf = swingLowPick != null ? swingLowPick.Item2 : -1;

            if (swingLow != null)
            {
                // SL = Swing Low - (ATR * buffer)
                double structuralSL = swingLow.Low - (_config.SL_BufferATR * atr);
                
                // Usar directamente el SL estructural (sin mínimos artificiales)
                double finalSL = structuralSL;
                // Diagnóstico selección de swing (BUY)
                double slDistAtrPick = Math.Abs(entry - finalSL) / atr;
                zone.Metadata["SL_SwingTF"] = swingTf;
                zone.Metadata["SL_Structural"] = true;
                zone.Metadata["SL_SwingPrice"] = swingLow.Low;
                _logger.Info(string.Format("[DIAGNOSTICO][Risk] SLPick BUY: Zone={0} SwingTF={1} SwingPrice={2:F2} SLDistATR={3:F2}",
                    zone.Id, swingTf, swingLow.Low, slDistAtrPick));
                
                _logger.Info(string.Format("[RiskCalculator] ✓ SL estructural (BUY): SwingLow={0:F2}, FinalSL={1:F2}",
                    swingLow.Low, finalSL));
                
                return finalSL;
            }
            else
            {
                // No se encontró Swing protector → no hay SL válido
                _logger.Warning(string.Format("[RiskCalculator] ⚠ No se encontró Swing Low protector para HeatZone {0} (TF={1}) → NO_SL_STRUCTURAL",
                    zone.Id, zone.TFDominante));
                zone.Metadata["SL_Structural"] = false;
                zone.Metadata["SL_SwingTF"] = -1;
                return double.NaN;
            }
        }

        /// <summary>
        /// Calcula SL estructural para SELL: Swing High protector + (ATR * buffer)
        /// Usa exclusivamente SL estructural (sin MinSL artificial)
        /// </summary>
        private double CalculateStructuralSL_Sell(HeatZone zone, CoreEngine coreEngine, IBarDataProvider barData, int currentBar, double atr, bool isHighVol)
        {
            double entry = zone.High;

            // Buscar Swing High protector (encima de la zona)
            var swingHighPick = FindProtectiveSwingHighBanded(zone, coreEngine, barData, atr, entry, true, isHighVol);
            var swingHigh = swingHighPick != null ? swingHighPick.Item1 : null;
            int swingTf = swingHighPick != null ? swingHighPick.Item2 : -1;

            if (swingHigh != null)
            {
                // SL = Swing High + (ATR * buffer)
                double structuralSL = swingHigh.High + (_config.SL_BufferATR * atr);
                
                // Usar directamente el SL estructural (sin mínimos artificiales)
                double finalSL = structuralSL;
                // Diagnóstico selección de swing (SELL)
                double slDistAtrPick = Math.Abs(finalSL - entry) / atr;
                zone.Metadata["SL_SwingTF"] = swingTf;
                zone.Metadata["SL_Structural"] = true;
                zone.Metadata["SL_SwingPrice"] = swingHigh.High;
                _logger.Info(string.Format("[DIAGNOSTICO][Risk] SLPick SELL: Zone={0} SwingTF={1} SwingPrice={2:F2} SLDistATR={3:F2}",
                    zone.Id, swingTf, swingHigh.High, slDistAtrPick));
                
                _logger.Info(string.Format("[RiskCalculator] ✓ SL estructural (SELL): SwingHigh={0:F2}, FinalSL={1:F2}",
                    swingHigh.High, finalSL));
                
                return finalSL;
            }
            else
            {
                // No se encontró Swing protector → no hay SL válido
                _logger.Warning(string.Format("[RiskCalculator] ⚠ No se encontró Swing High protector para HeatZone {0} (TF={1}) → NO_SL_STRUCTURAL",
                    zone.Id, zone.TFDominante));
                zone.Metadata["SL_Structural"] = false;
                zone.Metadata["SL_SwingTF"] = -1;
                return double.NaN;
            }
        }

        // ========================================================================
        // CÁLCULO DE TP JERÁRQUICO (LIQUIDITY + STRUCTURE)
        // ========================================================================

        /// <summary>
        /// Calcula TP jerárquico para BUY:
        /// 1. Liquidity Grab/Void opuesto (arriba)
        /// 2. FVG/OB opuesto con Score > 0.7
        /// 3. Swing High opuesto
        /// 4. Fallback: Entry + (Entry - SL) * MinRiskRewardRatio
        /// </summary>
        private double CalculateStructuralTP_Buy(HeatZone zone, DecisionSnapshot snapshot, CoreEngine coreEngine, IBarDataProvider barData, int currentBar, double entry, double stopLoss, bool isHighVol)
        {
            double riskDistance = entry - stopLoss;
            double fallbackTP = entry + (riskDistance * _config.MinRiskRewardRatio);

            _logger.Info(string.Format("[RiskCalculator] === CALCULANDO TP JERÁRQUICO (BUY) para HeatZone {0} ===", zone.Id));
            _logger.Info(string.Format("[RiskCalculator] Entry={0:F2}, SL={1:F2}, RiskDistance={2:F2}, FallbackTP={3:F2}",
                entry, stopLoss, riskDistance, fallbackTP));

            // LOG DETALLADO: Recopilar TODOS los candidatos TP para análisis post-mortem
            var allCandidates = new List<Tuple<string, string, double, int, double, double, int>>(); // (priority, type, score, tf, price, distATR, age)
            int decisionTF_local = _config.DecisionTimeframeMinutes;
            DateTime analysisTimeTP = barData.GetBarTime(decisionTF_local, currentBar);
            int idxAtrDom = barData.GetBarIndexFromTime(zone.TFDominante, analysisTimeTP);
            double atr = idxAtrDom >= 0 ? barData.GetATR(zone.TFDominante, 14, idxAtrDom) : 1.0;

            // ✅ V6.0j: Recolectar TODOS los candidatos TP (P0, P1, P2, P3, P4) para competencia con scoring inteligente
            var allTPCandidates = new List<(string Type, double Price, double DistATR, double RR, double StructuralScore, double AgeHours, int TF)>();

            // P0: Opposing Zone (ya NO retorna prematuramente, se añade a la lista)
            double? opposingTP = GetOpposingZoneTP_Buy(zone, snapshot, barData, coreEngine, analysisTimeTP, entry, riskDistance, atr);
            if (opposingTP.HasValue)
            {
                double ageP0 = 0; // P0 no tiene edad estructural definida
                allTPCandidates.Add(("P0_Zone", opposingTP.Value, Math.Abs(opposingTP.Value - entry) / atr, 
                    (opposingTP.Value - entry) / riskDistance, 0.5, ageP0, zone.TFDominante));
                _logger.Debug($"[TP_CANDIDATES] P0_Zone añadido: {opposingTP.Value:F2}, DistATR={(Math.Abs(opposingTP.Value - entry) / atr):F2}");
            }

            // P1: Recopilar Liquidity targets
            foreach (var tf in _config.TimeframesToUse.OrderByDescending(t => t))
            {
                var liquidities = coreEngine.GetAllStructures(tf)
                    .Where(s => (s.Type == "LiquidityGrabInfo" || s.Type == "LiquidityVoidInfo") && s.IsActive && s.Low > zone.High)
                    .ToList();
                foreach (var liq in liquidities)
                {
                    double tpPrice = liq.Low;
                    if (tpPrice <= entry) continue;
                    double distATR = Math.Abs(tpPrice - entry) / atr;
                    double potentialRR = (tpPrice - entry) / riskDistance;
                    int idxStruct = barData.GetBarIndexFromTime(liq.TF, analysisTimeTP);
                    int ageBars = idxStruct >= 0 ? (idxStruct - liq.CreatedAtBarIndex) : int.MaxValue;
                    double ageHours = (ageBars * liq.TF) / 60.0;
                    allTPCandidates.Add(("P1_Liquidity", tpPrice, distATR, potentialRR, liq.Score, ageHours, liq.TF));
                    allCandidates.Add(Tuple.Create("P1", liq.Type, liq.Score, liq.TF, tpPrice, distATR, ageBars));
                }
            }
            // P2: Recopilar FVG/OB/POI opuestos
            double minScoreP2 = (zone.TFDominante <= 15) ? 0.60 : 0.70;
            foreach (var tf in _config.TimeframesToUse.OrderByDescending(t => t))
            {
                var structures = coreEngine.GetAllStructures(tf)
                    .Where(s => (s.Type == "FairValueGapInfo" || s.Type == "OrderBlockInfo" || s.Type == "PointOfInterestInfo") 
                             && s.IsActive && s.Score >= minScoreP2 && s.Low > zone.High)
                    .ToList();
                foreach (var str in structures)
                {
                    double tpPrice = str.Low;
                    if (tpPrice <= entry) continue;
                    double distATR = Math.Abs(tpPrice - entry) / atr;
                    double potentialRR = (tpPrice - entry) / riskDistance;
                    int idxStruct = barData.GetBarIndexFromTime(str.TF, analysisTimeTP);
                    int ageBars = idxStruct >= 0 ? (idxStruct - str.CreatedAtBarIndex) : int.MaxValue;
                    double ageHours = (ageBars * str.TF) / 60.0;
                    allTPCandidates.Add(("P2_Structure", tpPrice, distATR, potentialRR, str.Score, ageHours, str.TF));
                    allCandidates.Add(Tuple.Create("P2", str.Type, str.Score, str.TF, tpPrice, distATR, ageBars));
                }
            }
            // P3: Recopilar Swings
            var allowedTFsP3 = new HashSet<int>(new[] { 15, 5, 60, 240 });
            foreach (var tf in _config.TimeframesToUse)
            {
                if (!allowedTFsP3.Contains(tf)) continue;
                var swings = coreEngine.GetAllStructures(tf)
                    .OfType<SwingInfo>()
                    .Where(s => s.IsActive && s.IsHigh && s.High > zone.High)
                    .ToList();
                foreach (var sw in swings)
                {
                    double tpPrice = sw.High;
                    if (tpPrice <= entry) continue;
                    double distATR = Math.Abs(tpPrice - entry) / atr;
                    double potentialRR = (tpPrice - entry) / riskDistance;
                    int idxStruct = barData.GetBarIndexFromTime(sw.TF, analysisTimeTP);
                    int ageBars = idxStruct >= 0 ? (idxStruct - sw.CreatedAtBarIndex) : int.MaxValue;
                    double ageHours = (ageBars * sw.TF) / 60.0;
                    allTPCandidates.Add(("P3_Swing", tpPrice, distATR, potentialRR, sw.Score, ageHours, sw.TF));
                    allCandidates.Add(Tuple.Create("P3", "Swing", sw.Score, sw.TF, tpPrice, distATR, ageBars));
                }
            }
            
            // P4: Fallback matemático
            allTPCandidates.Add(("P4_Fallback", fallbackTP, Math.Abs(fallbackTP - entry) / atr, 
                _config.MinRiskRewardRatio, 0.0, 0.0, -1));

            // Log todos los candidatos
            _logger.Info(string.Format("[DIAGNOSTICO][Risk] TP_CANDIDATES: Zone={0} Dir=BUY TotalCandidates={1}", zone.Id, allCandidates.Count));
            int idx = 0;
            foreach (var c in allCandidates
                .OrderBy(x => x.Item1, StringComparer.Ordinal)     // Priority (string)
                .ThenBy(x => x.Item6)                               // Age (int)
                .ThenBy(x => x.Item2, StringComparer.Ordinal)       // Type (string)
                .ThenByDescending(x => x.Item4)                     // TF (int)
                .ThenByDescending(x => x.Item3)                     // Score (double)
                .ThenBy(x => x.Item5))                              // Price (double)

            {
                idx++;
                double potentialRR = (c.Item5 - entry) / riskDistance;
                _logger.Info(string.Format(
                    "[DIAGNOSTICO][Risk] TP_CANDIDATE: Idx={0} Priority={1} Type={2} Score={3:F2} TF={4} DistATR={5:F2} Age={6} Price={7:F2} RR={8:F2}",
                    idx, c.Item1, c.Item2, c.Item3, c.Item4, c.Item6, c.Item7, c.Item5, potentialRR));
            }

            // PRIORIDAD INTRADÍA: P3 primero (TF≤15m), luego P3 TF>15m reciente y dentro de caps, luego P0 con caps
            try
            {
                double maxTPPoints = isHighVol ? _config.MaxTPDistancePoints_HighVol : _config.MaxTPDistancePoints_Normal;
                double maxTPATR = isHighVol ? _config.MaxTPDistanceATR_HighVol : _config.MaxTPDistanceATR;
                double tol = 1.0 + _config.ValidationTolerancePercent;
                // 1) P3 intradía (TF ≤ 15m) dentro de caps ATR/puntos
                var p3Intra = allTPCandidates
                    .Where(c => c.Type == "P3_Swing" && c.TF <= 15)
                    .Where(c => Math.Abs(c.Price - entry) <= maxTPPoints * tol && c.DistATR <= maxTPATR)
                    .ToList();
                if (p3Intra.Count > 0)
                    allTPCandidates = p3Intra;
                else
                {
                    // 2) P3 TF>15m recientes (<24h) y dentro de caps
                    var p3HighTFRecent = allTPCandidates
                        .Where(c => c.Type == "P3_Swing" && c.TF > 15)
                        .Where(c => c.AgeHours <= 24.0 && Math.Abs(c.Price - entry) <= maxTPPoints * tol && c.DistATR <= maxTPATR)
                        .ToList();
                    if (p3HighTFRecent.Count > 0)
                        allTPCandidates = p3HighTFRecent;
                    else
                    {
                        // 3) Fallback P0 con caps estrictos (ATR ≤ 6.0, puntos ≤ cap, RR ≥ MinRR)
                        var p0Constrained = allTPCandidates
                            .Where(c => c.Type.StartsWith("P0"))
                            .Where(c => c.DistATR <= 6.0 && Math.Abs(c.Price - entry) <= maxTPPoints * tol && c.RR >= _config.MinRiskRewardRatio)
                            .ToList();
                        if (p0Constrained.Count > 0)
                            allTPCandidates = p0Constrained;
                    }
                }
            }
            catch { /* defensivo: si algo falla, continuar con el pool completo */ }

            // (bloque duplicado eliminado)

            // ✅ SCORING INTELIGENTE: Evaluar TODOS los candidatos y elegir el mejor
            if (allTPCandidates.Count == 0)
            {
                _logger.Warning($"[RISK][TP_NO_CANDIDATES] Zone={zone.Id} Sin candidatos TP → REJECT");
                zone.Metadata["RiskCalculated"] = false;
                zone.Metadata["RejectReason"] = "Sin candidatos TP válidos";
                return fallbackTP;
            }
            
            var scoredTPCandidates = allTPCandidates.Select(c =>
            {
                double intelligentScore = CalculateTPIntelligentScore(
                    c.Type, c.Price - entry, c.DistATR, c.RR, c.StructuralScore, c.AgeHours, atr);
                
                _logger.Info($"[TP_SCORE] Zone={zone.Id} Type={c.Type} TF={c.TF} " +
                             $"DistATR={c.DistATR:F2} RR={c.RR:F2} StructScore={c.StructuralScore:F2} " +
                             $"AgeH={c.AgeHours:F1} FINAL={intelligentScore:F3} Price={c.Price:F2}");
                
                return (c.Type, c.Price, c.DistATR, c.RR, c.StructuralScore, c.AgeHours, c.TF, intelligentScore);
            }).OrderByDescending(x => x.intelligentScore).ToList();

            var bestTP = scoredTPCandidates.FirstOrDefault();

            if (bestTP.intelligentScore > 0)
            {
                _logger.Info($"[TP_PICK] Zone={zone.Id} WINNER: Type={bestTP.Type} TF={bestTP.TF} " +
                             $"DistATR={bestTP.DistATR:F2} RR={bestTP.RR:F2} FinalScore={bestTP.intelligentScore:F3} " +
                             $"Price={bestTP.Price:F2} Reason=BestIntelligentScore");
                // Duplicar tag para analizadores que esperan prefijo [RISK]
                _logger.Info($"[RISK][TP_PICK] Zone={zone.Id} WINNER: Type={bestTP.Type} TF={bestTP.TF} DistATR={bestTP.DistATR:F2} RR={bestTP.RR:F2} Score={bestTP.intelligentScore:F3} Price={bestTP.Price:F2}");
                
                zone.Metadata["TP_Structural"] = !bestTP.Type.Contains("P4_Fallback");
                zone.Metadata["TP_TargetTF"] = bestTP.TF;
                zone.Metadata["TP_Type"] = bestTP.Type;
                zone.Metadata["TP_Score"] = bestTP.intelligentScore;
                
                return bestTP.Price;
            }
            else
            {
                _logger.Warning($"[RISK][TP_NO_VALID] Zone={zone.Id} Sin candidatos TP con score>0 → REJECT");
                zone.Metadata["RiskCalculated"] = false;
                zone.Metadata["RejectReason"] = "Sin candidatos TP válidos con score>0";
                return fallbackTP;
            }
        }

        /// <summary>
        /// Calcula TP jerárquico para SELL:
        /// 1. Liquidity Grab/Void opuesto (abajo)
        /// 2. FVG/OB opuesto con Score > 0.7
        /// 3. Swing Low opuesto
        /// 4. Fallback: Entry - (SL - Entry) * MinRiskRewardRatio
        /// </summary>
        private double CalculateStructuralTP_Sell(HeatZone zone, DecisionSnapshot snapshot, CoreEngine coreEngine, IBarDataProvider barData, int currentBar, double entry, double stopLoss, bool isHighVol)
        {
            double riskDistance = stopLoss - entry;
            double fallbackTP = entry - (riskDistance * _config.MinRiskRewardRatio);

            _logger.Info(string.Format("[RiskCalculator] === CALCULANDO TP JERÁRQUICO (SELL) para HeatZone {0} ===", zone.Id));
            _logger.Info(string.Format("[RiskCalculator] Entry={0:F2}, SL={1:F2}, RiskDistance={2:F2}, FallbackTP={3:F2}",
                entry, stopLoss, riskDistance, fallbackTP));

            // LOG DETALLADO: Recopilar TODOS los candidatos TP para análisis post-mortem
            var allCandidates = new List<Tuple<string, string, double, int, double, double, int>>(); // (priority, type, score, tf, price, distATR, age)
            int decisionTF_local = _config.DecisionTimeframeMinutes;
            DateTime analysisTimeTP = barData.GetBarTime(decisionTF_local, currentBar);
            int idxAtrDom = barData.GetBarIndexFromTime(zone.TFDominante, analysisTimeTP);
            double atr = idxAtrDom >= 0 ? barData.GetATR(zone.TFDominante, 14, idxAtrDom) : 1.0;

            // ✅ V6.0j: Recolectar TODOS los candidatos TP (P0, P1, P2, P3, P4) para competencia con scoring inteligente
            var allTPCandidates = new List<(string Type, double Price, double DistATR, double RR, double StructuralScore, double AgeHours, int TF)>();

            // P0: Opposing Zone (ya NO retorna prematuramente, se añade a la lista)
            double? opposingTP = GetOpposingZoneTP_Sell(zone, snapshot, barData, coreEngine, analysisTimeTP, entry, riskDistance, atr);
            if (opposingTP.HasValue)
            {
                double ageP0 = 0; // P0 no tiene edad estructural definida
                allTPCandidates.Add(("P0_Zone", opposingTP.Value, Math.Abs(entry - opposingTP.Value) / atr, 
                    (entry - opposingTP.Value) / riskDistance, 0.5, ageP0, zone.TFDominante));
                _logger.Debug($"[TP_CANDIDATES] P0_Zone añadido: {opposingTP.Value:F2}, DistATR={(Math.Abs(entry - opposingTP.Value) / atr):F2}");
            }

            // P1: Recopilar Liquidity targets
            foreach (var tf in _config.TimeframesToUse.OrderByDescending(t => t))
            {
                var liquidities = coreEngine.GetAllStructures(tf)
                    .Where(s => (s.Type == "LiquidityGrabInfo" || s.Type == "LiquidityVoidInfo") && s.IsActive && s.High < zone.Low)
                    .ToList();
                foreach (var liq in liquidities)
                {
                    double tpPrice = liq.High;
                    if (tpPrice >= entry) continue;
                    double distATR = Math.Abs(entry - tpPrice) / atr;
                    double potentialRR = (entry - tpPrice) / riskDistance;
                    int idxStruct = barData.GetBarIndexFromTime(liq.TF, analysisTimeTP);
                    int ageBars = idxStruct >= 0 ? (idxStruct - liq.CreatedAtBarIndex) : int.MaxValue;
                    double ageHours = (ageBars * liq.TF) / 60.0;
                    allTPCandidates.Add(("P1_Liquidity", tpPrice, distATR, potentialRR, liq.Score, ageHours, liq.TF));
                    allCandidates.Add(Tuple.Create("P1", liq.Type, liq.Score, liq.TF, tpPrice, distATR, ageBars));
                }
            }
            // P2: Recopilar FVG/OB/POI opuestos
            double minScoreP2Sell = (zone.TFDominante <= 15) ? 0.60 : 0.70;
            foreach (var tf in _config.TimeframesToUse.OrderByDescending(t => t))
            {
                var structures = coreEngine.GetAllStructures(tf)
                    .Where(s => (s.Type == "FairValueGapInfo" || s.Type == "OrderBlockInfo" || s.Type == "PointOfInterestInfo") 
                             && s.IsActive && s.Score >= minScoreP2Sell && s.High < zone.Low)
                    .ToList();
                foreach (var str in structures)
                {
                    double tpPrice = str.High;
                    if (tpPrice >= entry) continue;
                    double distATR = Math.Abs(entry - tpPrice) / atr;
                    double potentialRR = (entry - tpPrice) / riskDistance;
                    int idxStruct = barData.GetBarIndexFromTime(str.TF, analysisTimeTP);
                    int ageBars = idxStruct >= 0 ? (idxStruct - str.CreatedAtBarIndex) : int.MaxValue;
                    double ageHours = (ageBars * str.TF) / 60.0;
                    allTPCandidates.Add(("P2_Structure", tpPrice, distATR, potentialRR, str.Score, ageHours, str.TF));
                    allCandidates.Add(Tuple.Create("P2", str.Type, str.Score, str.TF, tpPrice, distATR, ageBars));
                }
            }
            // P3: Recopilar Swings
            var allowedTFsP3Sell = new HashSet<int>(new[] { 15, 5, 60, 240 });
            foreach (var tf in _config.TimeframesToUse)
            {
                if (!allowedTFsP3Sell.Contains(tf)) continue;
                var swings = coreEngine.GetAllStructures(tf)
                    .OfType<SwingInfo>()
                    .Where(s => s.IsActive && !s.IsHigh && s.Low < zone.Low)
                    .ToList();
                foreach (var sw in swings)
                {
                    double tpPrice = sw.Low;
                    if (tpPrice >= entry) continue;
                    double distATR = Math.Abs(entry - tpPrice) / atr;
                    double potentialRR = (entry - tpPrice) / riskDistance;
                    int idxStruct = barData.GetBarIndexFromTime(sw.TF, analysisTimeTP);
                    int ageBars = idxStruct >= 0 ? (idxStruct - sw.CreatedAtBarIndex) : int.MaxValue;
                    double ageHours = (ageBars * sw.TF) / 60.0;
                    allTPCandidates.Add(("P3_Swing", tpPrice, distATR, potentialRR, sw.Score, ageHours, sw.TF));
                    allCandidates.Add(Tuple.Create("P3", "Swing", sw.Score, sw.TF, tpPrice, distATR, ageBars));
                }
            }
            
            // P4: Fallback matemático
            allTPCandidates.Add(("P4_Fallback", fallbackTP, Math.Abs(entry - fallbackTP) / atr, 
                _config.MinRiskRewardRatio, 0.0, 0.0, -1));

            // Log todos los candidatos
            _logger.Info(string.Format("[DIAGNOSTICO][Risk] TP_CANDIDATES: Zone={0} Dir=SELL TotalCandidates={1}", zone.Id, allCandidates.Count));
            int idx = 0;
            foreach (var c in allCandidates
                .OrderBy(x => x.Item1, StringComparer.Ordinal)     // Priority (string)
                .ThenBy(x => x.Item6)                               // Age (int)
                .ThenBy(x => x.Item2, StringComparer.Ordinal)       // Type (string)
                .ThenByDescending(x => x.Item4)                     // TF (int)
                .ThenByDescending(x => x.Item3)                     // Score (double)
                .ThenBy(x => x.Item5))                              // Price (double)

            {
                idx++;
                double potentialRR = (entry - c.Item5) / riskDistance;
                _logger.Info(string.Format(
                    "[DIAGNOSTICO][Risk] TP_CANDIDATE: Idx={0} Priority={1} Type={2} Score={3:F2} TF={4} DistATR={5:F2} Age={6} Price={7:F2} RR={8:F2}",
                    idx, c.Item1, c.Item2, c.Item3, c.Item4, c.Item6, c.Item7, c.Item5, potentialRR));
            }

            // ✅ SCORING INTELIGENTE: Evaluar TODOS los candidatos y elegir el mejor
            if (allTPCandidates.Count == 0)
            {
                _logger.Warning($"[RISK][TP_NO_CANDIDATES] Zone={zone.Id} Sin candidatos TP → REJECT");
                zone.Metadata["RiskCalculated"] = false;
                zone.Metadata["RejectReason"] = "Sin candidatos TP válidos";
                return fallbackTP;
            }
            
            var scoredTPCandidatesSell = allTPCandidates.Select(c =>
            {
                double intelligentScore = CalculateTPIntelligentScore(
                    c.Type, entry - c.Price, c.DistATR, c.RR, c.StructuralScore, c.AgeHours, atr);
                
                _logger.Info($"[TP_SCORE] Zone={zone.Id} Type={c.Type} TF={c.TF} " +
                             $"DistATR={c.DistATR:F2} RR={c.RR:F2} StructScore={c.StructuralScore:F2} " +
                             $"AgeH={c.AgeHours:F1} FINAL={intelligentScore:F3} Price={c.Price:F2}");
                
                return (c.Type, c.Price, c.DistATR, c.RR, c.StructuralScore, c.AgeHours, c.TF, intelligentScore);
            }).OrderByDescending(x => x.intelligentScore).ToList();

            var bestTPSell = scoredTPCandidatesSell.FirstOrDefault();

            if (bestTPSell.intelligentScore > 0)
            {
                _logger.Info($"[TP_PICK] Zone={zone.Id} WINNER: Type={bestTPSell.Type} TF={bestTPSell.TF} " +
                             $"DistATR={bestTPSell.DistATR:F2} RR={bestTPSell.RR:F2} FinalScore={bestTPSell.intelligentScore:F3} " +
                             $"Price={bestTPSell.Price:F2} Reason=BestIntelligentScore");
                // Duplicar tag para analizadores que esperan prefijo [RISK]
                _logger.Info($"[RISK][TP_PICK] Zone={zone.Id} WINNER: Type={bestTPSell.Type} TF={bestTPSell.TF} DistATR={bestTPSell.DistATR:F2} RR={bestTPSell.RR:F2} Score={bestTPSell.intelligentScore:F3} Price={bestTPSell.Price:F2}");
                
                zone.Metadata["TP_Structural"] = !bestTPSell.Type.Contains("P4_Fallback");
                zone.Metadata["TP_TargetTF"] = bestTPSell.TF;
                zone.Metadata["TP_Type"] = bestTPSell.Type;
                zone.Metadata["TP_Score"] = bestTPSell.intelligentScore;
                
                return bestTPSell.Price;
            }
            else
            {
                _logger.Warning($"[RISK][TP_NO_VALID] Zone={zone.Id} Sin candidatos TP con score>0 → REJECT");
                zone.Metadata["RiskCalculated"] = false;
                zone.Metadata["RejectReason"] = "Sin candidatos TP válidos con score>0";
                return fallbackTP;
            }
        }

       
        // ========================================================================
        // HELPERS: BÚSQUEDA DE ESTRUCTURAS PROTECTORAS Y OBJETIVOS
        // ========================================================================

        /// <summary>
        /// Busca el Swing Low más cercano DEBAJO de la HeatZone (protector para BUY)
        /// Busca en TFs altos (4H, 1D) para mayor robustez
        /// </summary>
        private Tuple<SwingInfo,int> FindProtectiveSwingLowBanded(HeatZone zone, CoreEngine coreEngine, IBarDataProvider barData, double atr, double entry, bool prioritizeTf, bool isHighVol)
        {
            // Priorizar TFs >= 60 si existen
            IEnumerable<int> tfs = prioritizeTf
                ? _config.TimeframesToUse.OrderBy(tf => tf >= 60 ? 1 : 0).ThenBy(tf => tf) // 15/5 primero, luego 60/240
                : _config.TimeframesToUse.AsEnumerable();
            var candidates = new List<Tuple<SwingInfo,int,double>>(); // (swing, tf, slDistAtr)
            int totalFound = 0;
            var swingsByTF = new Dictionary<int,int>();
            // int rejectedByAge = 0; // ✅ YA NO SE USA - Filtro eliminado
            // Bandas dinámicas según régimen
            double bandMin = isHighVol ? _config.SL_BandMin_HighVol : _config.SL_BandMin_Normal;
            double bandMax = isHighVol ? _config.SL_BandMax_HighVol : _config.SL_BandMax_Normal;
            double target  = isHighVol ? _config.SL_Target_HighVol  : _config.SL_Target_Normal;
            foreach (var tf in tfs)
            {
                if (isHighVol && _config.AllowedTFs_SL_HighVol != null && !_config.AllowedTFs_SL_HighVol.Contains(tf))
                    continue; // En HighVol, respetar TFs permitidos para SL
                var allStructures = coreEngine.GetAllStructures(tf);
                foreach (var s in allStructures.OfType<SwingInfo>())
                {
                    if (s.IsActive && !s.IsHigh && s.Low < zone.Low)
                    {
                        // Contabilizar candidatos brutos (antes de filtros)
                        totalFound++;
                        if (!swingsByTF.ContainsKey(tf)) swingsByTF[tf] = 0;
                        swingsByTF[tf]++;
                        
                        // ✅ FILTRO DE EDAD ELIMINADO - El scoring multi-factor ya penaliza por edad
                        // El recencyScore del scoring inteligente maneja esto mejor que un filtro binario
                        
                        // SL simulado para BUY: swing.Low - buffer*ATR
                        double simSL = s.Low - (_config.SL_BufferATR * atr);
                        double slDistPtsCand = Math.Abs(entry - simSL);
                        // ✅ Eliminado filtro pre-cap - El scoring multi-factor ya penaliza distancias grandes
                        // El scoring considera: calidad estructural, recencia, proximidad óptima a entry, TF preference
                        double slDistAtr = slDistPtsCand / atr;
                        candidates.Add(Tuple.Create(s, tf, slDistAtr));
                    }
                }
            }
            
            // ✅ Log de edad eliminado - Ya no rechazamos por edad pre-scoring
            // if (rejectedByAge > 0) { _logger.Info(...); }
            
            if (candidates.Count == 0)
            {
                _logger.Warning(string.Format("[RiskCalculator] ⚠ No candidates Swing Low protector para zona {0:F2}-{1:F2}", zone.Low, zone.High));
                string tfList = string.Join(",", swingsByTF.Select(kv => string.Format("{0}:{1}", kv.Key, kv.Value)));
                _logger.Info(string.Format(
                    "[DIAG][Risk] NO_SL_CANDIDATES: Zone={0} Entry={1:F2} TotalFound={2} SwingsByTF=[{3}]",
                    zone.Id, entry, totalFound, tfList));
                return null;
            }

            // ═══════════════════════════════════════════════════════════════════════
            // FASE 3: Jerarquía intradía con fallback inteligente
            // ═══════════════════════════════════════════════════════════════════════
            var intradayCandidates = candidates.Where(c => c.Item2 <= 15).ToList();
            if (intradayCandidates.Count > 0)
            {
                candidates = intradayCandidates;
                _logger.Debug($"[SL_SELECTION] Usando {candidates.Count} candidatos intradía (TF≤15m) - Descartados TF>15m");
            }
            else
            {
                // Fallback: permitir TF>15m solo si son recientes (<48h) - Sin filtro de cap artificial
                var higherTFRecent = candidates.Where(c =>
                {
                    int idxAtTime = barData.GetBarIndexFromTime(c.Item2, _currentAnalysisTime);
                    if (idxAtTime < 0) return false;
                    int ageBars = idxAtTime - c.Item1.CreatedAtBarIndex;
                    double ageHours = (ageBars * c.Item2) / 60.0;
                    // Solo filtro de recencia razonable (48h para permitir swings de 60m/240m)
                    return ageHours <= 48.0;
                }).ToList();

                if (higherTFRecent.Count > 0)
                {
                    candidates = higherTFRecent;
                    _logger.Debug($"[SL_SELECTION] Fallback: {candidates.Count} candidatos TF>15m (recientes <48h)");
                }
                else
                {
                    _logger.Info($"[RISK][SL_NO_VALID] Zone={zone.Id} Sin SL intradía ni TF>15m válido (reciente) → REJECT");
                    return null;
                }
            }

            // ═══════════════════════════════════════════════════════════════════════
            // SCORING ESPECÍFICO PARA SL (NO USA PROXIMITY)
            // ═══════════════════════════════════════════════════════════════════════
            _logger.Info($"[DIAGNOSTICO][Risk] SL_CANDIDATES: Zone={zone.Id} Dir=BUY TotalCandidates={candidates.Count}");

            double tickSize = barData.GetTickSize();
            var scoredCandidates = candidates.Select(c =>
            {
                // Calcular edad
                int idxAtTime = barData.GetBarIndexFromTime(c.Item2, _currentAnalysisTime);
                int ageBars = idxAtTime >= 0 ? (idxAtTime - c.Item1.CreatedAtBarIndex) : int.MaxValue;
                double ageHours = (ageBars * c.Item2) / 60.0;
                
                // Distancia al entry
                double simSL = c.Item1.Low - (_config.SL_BufferATR * atr);
                double distFromEntry = Math.Abs(entry - simSL);
                double distATR = distFromEntry / atr;
                
                // 1. Calidad estructural (NO usa proximity al precio actual)
                double structuralQuality = CalculateSLStructuralQuality(c.Item1, c.Item2, entry, atr, tickSize);
                
                // 2. Recencia (moderada, no extrema)
                double recencyScore = ageHours <= 12 ? 1.0 :
                                      ageHours <= 24 ? 0.8 :
                                      ageHours <= 48 ? 0.6 :
                                      ageHours <= 72 ? 0.4 : 0.2;
                
                // 3. Proximidad óptima al entry (gaussiana centrada en 2.0 ATR)
                double optimalDistATR = 2.0;
                double proximityToEntry = Math.Exp(-Math.Pow((distATR - optimalDistATR) / 1.5, 2));
                
                // 4. TF preference
                double tfPref = c.Item2 == 15 ? 1.0 : 
                                c.Item2 == 5 ? 0.95 : 
                                c.Item2 == 60 ? 0.7 : 
                                c.Item2 == 240 ? 0.4 : 0.2;
                
                // SCORE FINAL
                double finalScore = 
                    0.35 * structuralQuality +
                    0.25 * recencyScore +
                    0.20 * proximityToEntry +
                    0.20 * tfPref;
                
                // Logging detallado
                int totalTouches = c.Item1.TouchCount_Body + c.Item1.TouchCount_Wick;
                double swingSize = (c.Item1.High - c.Item1.Low) / atr;
                _logger.Info($"[SL_SCORE] Zone={zone.Id} Swing={c.Item1.Id.Substring(0,8)} TF={c.Item2} " +
                             $"DistATR={distATR:F2} AgeH={ageHours:F1} " +
                             $"StructQ={structuralQuality:F2} Recency={recencyScore:F2} " +
                             $"ProxEntry={proximityToEntry:F2} TFPref={tfPref:F2} " +
                             $"FINAL={finalScore:F3} " +
                             $"[Size={swingSize:F2}ATR Touches={totalTouches} Broken={c.Item1.IsBroken}]");
                
                return new { 
                    Swing = c.Item1, 
                    TF = c.Item2, 
                    DistATR = distATR, 
                    AgeHours = ageHours,
                    StructuralQuality = structuralQuality,
                    RecencyScore = recencyScore,
                    ProximityToEntry = proximityToEntry,
                    TFPref = tfPref,
                    FinalScore = finalScore 
                };
            }).OrderByDescending(x => x.FinalScore).ThenBy(x => x.TF).ToList();

            // Seleccionar mejor candidato
            var best = scoredCandidates.FirstOrDefault();

            if (best == null)
            {
                _logger.Info($"[RISK][SL_NO_CANDIDATES] Zone={zone.Id} Sin candidatos SL");
                return null;
            }

            // Guardia de calidad mínima con fallback inteligente
            if (best.StructuralQuality < 0.30)
            {
                if (best.StructuralQuality >= 0.20 && best.FinalScore >= 0.40)
                {
                    _logger.Warning($"[SL_QUALITY_RELAXED] Zone={zone.Id} StructQ={best.StructuralQuality:F2} < 0.30 pero FinalScore={best.FinalScore:F2} → ACEPTADO con warning");
                }
                else
                {
                    _logger.Info($"[RISK][SL_LOW_QUALITY] Zone={zone.Id} Best StructQ={best.StructuralQuality:F2} < 0.30, FinalScore={best.FinalScore:F2} → REJECT");
                    return null;
                }
            }

            // SL seleccionado
            _logger.Info($"[SL_PICK] Zone={zone.Id} WINNER: Swing={best.Swing.Id.Substring(0,8)} TF={best.TF} " +
                         $"DistATR={best.DistATR:F2} AgeH={best.AgeHours:F1} " +
                         $"StructQ={best.StructuralQuality:F2} FinalScore={best.FinalScore:F3} " +
                         $"Reason=BestScore");

            return Tuple.Create(best.Swing, best.TF);
        }

        /// <summary>
        /// Busca el Swing High más cercano ENCIMA de la HeatZone (protector para SELL)
        /// Busca en TFs altos (4H, 1D) para mayor robustez
        /// </summary>
        private Tuple<SwingInfo,int> FindProtectiveSwingHighBanded(HeatZone zone, CoreEngine coreEngine, IBarDataProvider barData, double atr, double entry, bool prioritizeTf, bool isHighVol)
        {
            IEnumerable<int> tfs = prioritizeTf
                ? _config.TimeframesToUse.OrderBy(tf => tf >= 60 ? 1 : 0).ThenBy(tf => tf) // 15/5 primero, luego 60/240
                : _config.TimeframesToUse.AsEnumerable();
            var candidates = new List<Tuple<SwingInfo,int,double>>(); // (swing, tf, slDistAtr)
            int totalFound = 0;
            var swingsByTF = new Dictionary<int,int>();
            // int rejectedByAge = 0; // ✅ YA NO SE USA - Filtro eliminado
            // Bandas dinámicas según régimen
            double bandMin = isHighVol ? _config.SL_BandMin_HighVol : _config.SL_BandMin_Normal;
            double bandMax = isHighVol ? _config.SL_BandMax_HighVol : _config.SL_BandMax_Normal;
            double target  = isHighVol ? _config.SL_Target_HighVol  : _config.SL_Target_Normal;
            foreach (var tf in tfs)
            {
                if (isHighVol && _config.AllowedTFs_SL_HighVol != null && !_config.AllowedTFs_SL_HighVol.Contains(tf))
                    continue; // En HighVol, respetar TFs permitidos para SL
                var allStructures = coreEngine.GetAllStructures(tf);
                foreach (var s in allStructures.OfType<SwingInfo>())
                {
                    if (s.IsActive && s.IsHigh && s.High > zone.High)
                    {
                        // Contabilizar candidatos brutos (antes de filtros)
                        totalFound++;
                        if (!swingsByTF.ContainsKey(tf)) swingsByTF[tf] = 0;
                        swingsByTF[tf]++;
                        
                        // ✅ FILTRO DE EDAD ELIMINADO - El scoring multi-factor ya penaliza por edad
                        // El recencyScore del scoring inteligente maneja esto mejor que un filtro binario
                        
                        // SL simulado para SELL: swing.High + buffer*ATR
                        double simSL = s.High + (_config.SL_BufferATR * atr);
                        double slDistPtsCand = Math.Abs(simSL - entry);
                        // ✅ Eliminado filtro pre-cap - El scoring multi-factor ya penaliza distancias grandes
                        // El scoring considera: calidad estructural, recencia, proximidad óptima a entry, TF preference
                        double slDistAtr = slDistPtsCand / atr;
                        candidates.Add(Tuple.Create(s, tf, slDistAtr));
                    }
                }
            }
            
            // ✅ Log de edad eliminado - Ya no rechazamos por edad pre-scoring
            // if (rejectedByAge > 0) { _logger.Info(...); }
            
            if (candidates.Count == 0)
            {
                _logger.Warning(string.Format("[RiskCalculator] ⚠ No candidates Swing High protector para zona {0:F2}-{1:F2}", zone.Low, zone.High));
                string tfList = string.Join(",", swingsByTF.Select(kv => string.Format("{0}:{1}", kv.Key, kv.Value)));
                _logger.Info(string.Format(
                    "[DIAG][Risk] NO_SL_CANDIDATES: Zone={0} Entry={1:F2} TotalFound={2} SwingsByTF=[{3}]",
                    zone.Id, entry, totalFound, tfList));
                return null;
            }

            // ═══════════════════════════════════════════════════════════════════════
            // FASE 3: Jerarquía intradía con fallback inteligente
            // ═══════════════════════════════════════════════════════════════════════
            var intradayCandidates = candidates.Where(c => c.Item2 <= 15).ToList();
            if (intradayCandidates.Count > 0)
            {
                candidates = intradayCandidates;
                _logger.Debug($"[SL_SELECTION] Usando {candidates.Count} candidatos intradía (TF≤15m) - Descartados TF>15m");
            }
            else
            {
                // Fallback: permitir TF>15m solo si son recientes (<48h) - Sin filtro de cap artificial
                var higherTFRecent = candidates.Where(c =>
                {
                    int idxAtTime = barData.GetBarIndexFromTime(c.Item2, _currentAnalysisTime);
                    if (idxAtTime < 0) return false;
                    int ageBars = idxAtTime - c.Item1.CreatedAtBarIndex;
                    double ageHours = (ageBars * c.Item2) / 60.0;
                    // Solo filtro de recencia razonable (48h para permitir swings de 60m/240m)
                    return ageHours <= 48.0;
                }).ToList();

                if (higherTFRecent.Count > 0)
                {
                    candidates = higherTFRecent;
                    _logger.Debug($"[SL_SELECTION] Fallback: {candidates.Count} candidatos TF>15m (recientes <48h)");
                }
                else
                {
                    _logger.Info($"[RISK][SL_NO_VALID] Zone={zone.Id} Sin SL intradía ni TF>15m válido (reciente) → REJECT");
                    return null;
                }
            }

            // ═══════════════════════════════════════════════════════════════════════
            // SCORING ESPECÍFICO PARA SL (NO USA PROXIMITY)
            // ═══════════════════════════════════════════════════════════════════════
            _logger.Info($"[DIAGNOSTICO][Risk] SL_CANDIDATES: Zone={zone.Id} Dir=SELL TotalCandidates={candidates.Count}");

            double tickSize = barData.GetTickSize();
            var scoredCandidates = candidates.Select(c =>
            {
                // Calcular edad
                int idxAtTime = barData.GetBarIndexFromTime(c.Item2, _currentAnalysisTime);
                int ageBars = idxAtTime >= 0 ? (idxAtTime - c.Item1.CreatedAtBarIndex) : int.MaxValue;
                double ageHours = (ageBars * c.Item2) / 60.0;
                
                // Distancia al entry
                double simSL = c.Item1.High + (_config.SL_BufferATR * atr);
                double distFromEntry = Math.Abs(simSL - entry);
                double distATR = distFromEntry / atr;
                
                // 1. Calidad estructural (NO usa proximity al precio actual)
                double structuralQuality = CalculateSLStructuralQuality(c.Item1, c.Item2, entry, atr, tickSize);
                
                // 2. Recencia (moderada, no extrema)
                double recencyScore = ageHours <= 12 ? 1.0 :
                                      ageHours <= 24 ? 0.8 :
                                      ageHours <= 48 ? 0.6 :
                                      ageHours <= 72 ? 0.4 : 0.2;
                
                // 3. Proximidad óptima al entry (gaussiana centrada en 2.0 ATR)
                double optimalDistATR = 2.0;
                double proximityToEntry = Math.Exp(-Math.Pow((distATR - optimalDistATR) / 1.5, 2));
                
                // 4. TF preference
                double tfPref = c.Item2 == 15 ? 1.0 : 
                                c.Item2 == 5 ? 0.95 : 
                                c.Item2 == 60 ? 0.7 : 
                                c.Item2 == 240 ? 0.4 : 0.2;
                
                // SCORE FINAL
                double finalScore = 
                    0.35 * structuralQuality +
                    0.25 * recencyScore +
                    0.20 * proximityToEntry +
                    0.20 * tfPref;
                
                // Logging detallado
                int totalTouches = c.Item1.TouchCount_Body + c.Item1.TouchCount_Wick;
                double swingSize = (c.Item1.High - c.Item1.Low) / atr;
                _logger.Info($"[SL_SCORE] Zone={zone.Id} Swing={c.Item1.Id.Substring(0,8)} TF={c.Item2} " +
                             $"DistATR={distATR:F2} AgeH={ageHours:F1} " +
                             $"StructQ={structuralQuality:F2} Recency={recencyScore:F2} " +
                             $"ProxEntry={proximityToEntry:F2} TFPref={tfPref:F2} " +
                             $"FINAL={finalScore:F3} " +
                             $"[Size={swingSize:F2}ATR Touches={totalTouches} Broken={c.Item1.IsBroken}]");
                
                return new { 
                    Swing = c.Item1, 
                    TF = c.Item2, 
                    DistATR = distATR, 
                    AgeHours = ageHours,
                    StructuralQuality = structuralQuality,
                    RecencyScore = recencyScore,
                    ProximityToEntry = proximityToEntry,
                    TFPref = tfPref,
                    FinalScore = finalScore 
                };
            }).OrderByDescending(x => x.FinalScore).ThenBy(x => x.TF).ToList();

            // Seleccionar mejor candidato
            var best = scoredCandidates.FirstOrDefault();

            if (best == null)
            {
                _logger.Info($"[RISK][SL_NO_CANDIDATES] Zone={zone.Id} Sin candidatos SL");
                return null;
            }

            // Guardia de calidad mínima con fallback inteligente
            if (best.StructuralQuality < 0.30)
            {
                if (best.StructuralQuality >= 0.20 && best.FinalScore >= 0.40)
                {
                    _logger.Warning($"[SL_QUALITY_RELAXED] Zone={zone.Id} StructQ={best.StructuralQuality:F2} < 0.30 pero FinalScore={best.FinalScore:F2} → ACEPTADO con warning");
                }
                else
                {
                    _logger.Info($"[RISK][SL_LOW_QUALITY] Zone={zone.Id} Best StructQ={best.StructuralQuality:F2} < 0.30, FinalScore={best.FinalScore:F2} → REJECT");
                    return null;
                }
            }

            // SL seleccionado
            _logger.Info($"[SL_PICK] Zone={zone.Id} WINNER: Swing={best.Swing.Id.Substring(0,8)} TF={best.TF} " +
                         $"DistATR={best.DistATR:F2} AgeH={best.AgeHours:F1} " +
                         $"StructQ={best.StructuralQuality:F2} FinalScore={best.FinalScore:F3} " +
                         $"Reason=BestScore");

            return Tuple.Create(best.Swing, best.TF);
        }

        /// <summary>
        /// Busca Liquidity Grab/Void ENCIMA de la zona (target para BUY)
        /// V5.7c: Con filtro de edad
        /// </summary>
        private StructureBase FindLiquidityTarget_Above(HeatZone zone, CoreEngine coreEngine, IBarDataProvider barData)
        {
            foreach (var tf in _config.TimeframesToUse.OrderByDescending(t => t))
            {
                var allStructures = coreEngine.GetAllStructures(tf)
                    .Where(s => (s.Type == "LiquidityGrabInfo" || s.Type == "LiquidityVoidInfo") 
                             && s.IsActive 
                             && s.Low > zone.High); // Encima
                
                foreach (var liquidity in allStructures.OrderBy(s => s.Low))
                {
                    // V5.7c: FILTRO DE EDAD para TP
                    int idxLiq = barData.GetBarIndexFromTime(liquidity.TF, _currentAnalysisTime);
                    int age = idxLiq >= 0 ? (idxLiq - liquidity.CreatedAtBarIndex) : int.MaxValue;
                    int maxAge = _config.MaxAgeForTP_ByTF.ContainsKey(liquidity.TF) ? _config.MaxAgeForTP_ByTF[liquidity.TF] : 100;
                    int effectiveMaxAge = _config.EnableRiskAgeBypassForDiagnostics
                        ? int.MaxValue
                        : (int)Math.Round(maxAge * Math.Max(1.0, _config.AgeFilterRelaxMultiplier));
                    
                    if (age <= effectiveMaxAge)
                        return liquidity; // Primera estructura válida por edad
                }
            }

            return null;
        }

        /// <summary>
        /// Busca Liquidity Grab/Void DEBAJO de la zona (target para SELL)
        /// V5.7c: Con filtro de edad
        /// </summary>
        private StructureBase FindLiquidityTarget_Below(HeatZone zone, CoreEngine coreEngine, IBarDataProvider barData)
        {
            foreach (var tf in _config.TimeframesToUse.OrderByDescending(t => t))
            {
                var allStructures = coreEngine.GetAllStructures(tf)
                    .Where(s => (s.Type == "LiquidityGrabInfo" || s.Type == "LiquidityVoidInfo") 
                             && s.IsActive 
                             && s.High < zone.Low); // Debajo
                
                foreach (var liquidity in allStructures.OrderByDescending(s => s.High))
                {
                    // V5.7c: FILTRO DE EDAD para TP
                    int idxLiqB = barData.GetBarIndexFromTime(liquidity.TF, _currentAnalysisTime);
                    int age = idxLiqB >= 0 ? (idxLiqB - liquidity.CreatedAtBarIndex) : int.MaxValue;
                    int maxAge = _config.MaxAgeForTP_ByTF.ContainsKey(liquidity.TF) ? _config.MaxAgeForTP_ByTF[liquidity.TF] : 100;
                    int effectiveMaxAge = _config.EnableRiskAgeBypassForDiagnostics
                        ? int.MaxValue
                        : (int)Math.Round(maxAge * Math.Max(1.0, _config.AgeFilterRelaxMultiplier));
                    
                    if (age <= effectiveMaxAge)
                        return liquidity; // Primera estructura válida por edad
                }
            }

            return null;
        }

        /// <summary>
        /// Busca FVG/OB opuesto ENCIMA con Score > minScore (target para BUY)
        /// V5.7c: Con filtro de edad
        /// </summary>
        private StructureBase FindOpposingStructure_Above(HeatZone zone, CoreEngine coreEngine, IBarDataProvider barData, double minScore)
        {
            foreach (var tf in _config.TimeframesToUse.OrderByDescending(t => t))
            {
                var allStructures = coreEngine.GetAllStructures(tf)
                    .Where(s => (s.Type == "FairValueGapInfo" || s.Type == "OrderBlockInfo" || s.Type == "PointOfInterestInfo") 
                             && s.IsActive 
                             && s.Score >= minScore
                             && s.Low > zone.High); // Encima
                
                foreach (var structure in allStructures.OrderBy(s => s.Low))
                {
                    // V5.7c: FILTRO DE EDAD para TP
                    int idxStr = barData.GetBarIndexFromTime(structure.TF, _currentAnalysisTime);
                    int age = idxStr >= 0 ? (idxStr - structure.CreatedAtBarIndex) : int.MaxValue;
                    int maxAge = _config.MaxAgeForTP_ByTF.ContainsKey(structure.TF) ? _config.MaxAgeForTP_ByTF[structure.TF] : 100;
                    int effectiveMaxAge = _config.EnableRiskAgeBypassForDiagnostics
                        ? int.MaxValue
                        : (int)Math.Round(maxAge * Math.Max(1.0, _config.AgeFilterRelaxMultiplier));
                    
                    if (age <= effectiveMaxAge)
                        return structure; // Primera estructura válida por edad
                }
            }

            return null;
        }

        /// <summary>
        /// Busca FVG/OB opuesto DEBAJO con Score > minScore (target para SELL)
        /// V5.7c: Con filtro de edad
        /// </summary>
        private StructureBase FindOpposingStructure_Below(HeatZone zone, CoreEngine coreEngine, IBarDataProvider barData, double minScore)
        {
            foreach (var tf in _config.TimeframesToUse.OrderByDescending(t => t))
            {
                var allStructures = coreEngine.GetAllStructures(tf)
                    .Where(s => (s.Type == "FairValueGapInfo" || s.Type == "OrderBlockInfo" || s.Type == "PointOfInterestInfo") 
                             && s.IsActive 
                             && s.Score >= minScore
                             && s.High < zone.Low); // Debajo
                
                foreach (var structure in allStructures.OrderByDescending(s => s.High))
                {
                    // V5.7c: FILTRO DE EDAD para TP
                    int idxStrB = barData.GetBarIndexFromTime(structure.TF, _currentAnalysisTime);
                    int age = idxStrB >= 0 ? (idxStrB - structure.CreatedAtBarIndex) : int.MaxValue;
                    int maxAge = _config.MaxAgeForTP_ByTF.ContainsKey(structure.TF) ? _config.MaxAgeForTP_ByTF[structure.TF] : 100;
                    int effectiveMaxAge = _config.EnableRiskAgeBypassForDiagnostics
                        ? int.MaxValue
                        : (int)Math.Round(maxAge * Math.Max(1.0, _config.AgeFilterRelaxMultiplier));
                    
                    if (age <= effectiveMaxAge)
                        return structure; // Primera estructura válida por edad
                }
            }

            return null;
        }

        /// <summary>
        /// Busca Swing High ENCIMA de la zona (target para BUY)
        /// PRIORIZA SWINGS CERCANOS de TODOS los TFs (no solo altos)
        /// V5.7c: Con filtro de edad
        /// </summary>
        private SwingInfo FindSwingHigh_Above(HeatZone zone, CoreEngine coreEngine, IBarDataProvider barData)
        {
            // Buscar en TODOS los TFs y recopilar todos los Swings válidos
            var allSwings = new List<SwingInfo>();
            
            foreach (var tf in _config.TimeframesToUse)
            {
                var swings = coreEngine.GetAllStructures(tf)
                    .OfType<SwingInfo>()
                    .Where(s => s.IsActive && s.IsHigh && s.High > zone.High); // Swing High encima
                
                foreach (var swing in swings)
                {
                    // V5.7c: FILTRO DE EDAD para TP
                    int idxSw = barData.GetBarIndexFromTime(swing.TF, _currentAnalysisTime);
                    int age = idxSw >= 0 ? (idxSw - swing.CreatedAtBarIndex) : int.MaxValue;
                    int maxAge = _config.MaxAgeForTP_ByTF.ContainsKey(swing.TF) ? _config.MaxAgeForTP_ByTF[swing.TF] : 100;
                    int effectiveMaxAge = _config.EnableRiskAgeBypassForDiagnostics
                        ? int.MaxValue
                        : (int)Math.Round(maxAge * Math.Max(1.0, _config.AgeFilterRelaxMultiplier));
                    
                    if (age <= effectiveMaxAge)
                        allSwings.Add(swing);
                }
            }

            // Priorizar por PROXIMIDAD (el más cercano por precio)
            return allSwings
                .OrderBy(s => s.High) // Más cercano por precio
                .FirstOrDefault();
        }

        /// <summary>
        /// Busca Swing Low DEBAJO de la zona (target para SELL)
        /// PRIORIZA SWINGS CERCANOS de TODOS los TFs (no solo altos)
        /// V5.7c: Con filtro de edad
        /// </summary>
        private SwingInfo FindSwingLow_Below(HeatZone zone, CoreEngine coreEngine, IBarDataProvider barData)
        {
            // Buscar en TODOS los TFs y recopilar todos los Swings válidos
            var allSwings = new List<SwingInfo>();
            
            foreach (var tf in _config.TimeframesToUse)
            {
                var swings = coreEngine.GetAllStructures(tf)
                    .OfType<SwingInfo>()
                    .Where(s => s.IsActive && !s.IsHigh && s.Low < zone.Low); // Swing Low debajo
                
                foreach (var swing in swings)
                {
                    // V5.7c: FILTRO DE EDAD para TP
                    int idxSwB = barData.GetBarIndexFromTime(swing.TF, _currentAnalysisTime);
                    int age = idxSwB >= 0 ? (idxSwB - swing.CreatedAtBarIndex) : int.MaxValue;
                    int maxAge = _config.MaxAgeForTP_ByTF.ContainsKey(swing.TF) ? _config.MaxAgeForTP_ByTF[swing.TF] : 100;
                    int effectiveMaxAge = _config.EnableRiskAgeBypassForDiagnostics
                        ? int.MaxValue
                        : (int)Math.Round(maxAge * Math.Max(1.0, _config.AgeFilterRelaxMultiplier));
                    
                    if (age <= effectiveMaxAge)
                        allSwings.Add(swing);
                }
            }

            // Priorizar por PROXIMIDAD (el más cercano por precio)
            return allSwings
                .OrderByDescending(s => s.Low) // Más cercano por precio (el más alto de los que están debajo)
                .FirstOrDefault();
        }

        // ========================================================================
        // FASE 2: OPPOSING HEATZONE PARA TP INTELIGENTE
        // ========================================================================

        /// <summary>
        /// V6.0f-FASE2: Calcula score para TP basado en HeatZone opuesta.
        /// Scoring multi-criterio: CoreScore (30%), ProximityFactor (20%), R:R óptimo (25%), DistanceATR óptimo (25%)
        /// </summary>
        private double CalculateTPScore(HeatZone opposingZone, double rr, double distanceATR)
        {
            // Extraer scores de la zona opuesta
            double coreScore = opposingZone.Metadata.ContainsKey("CoreScore") ? 
                (double)opposingZone.Metadata["CoreScore"] : 0.0;
            double proximityFactor = opposingZone.Metadata.ContainsKey("ProximityFactor") ? 
                (double)opposingZone.Metadata["ProximityFactor"] : 0.0;
            
            // Scoring de R:R óptimo [1.2, 3.0]
            double rrScore = (rr >= 1.2 && rr <= 3.0) ? 0.25 : 0.0;
            
            // Scoring de DistanceATR óptimo [6, 20]
            double distScore = (distanceATR >= 6.0 && distanceATR <= 20.0) ? 0.25 : 0.0;
            
            // Score final ponderado
            return (coreScore * 0.30) + (proximityFactor * 0.20) + rrScore + distScore;
        }

        /// <summary>
        /// V6.0f-FASE2: Busca HeatZone opuesta para TP (LONG).
        /// P0: Nearest opposing HeatZone (resistencia BEAR arriba del entry)
        /// Configuración: Borde más cercano (Low), ATR del TF opuesto, RR [1.2, 3.0], DistATR [6, 20]
        /// </summary>
        private double? GetOpposingZoneTP_Buy(
            HeatZone zone, 
            DecisionSnapshot snapshot, 
            IBarDataProvider barData,
            CoreEngine coreEngine,
            DateTime analysisTime,
            double entry, 
            double riskDistance,
            double atrFallback)
        {
            // DEBUG: Estadísticas iniciales
            int totalZones = snapshot.HeatZones.Count;
            var bearZones = snapshot.HeatZones.Where(z => z.Direction == "Bear").ToList();
            var bearZonesAbove = bearZones.Where(z => z.Low > entry).ToList();
            
            _logger.Info(string.Format("[RISK][P0_DEBUG] Zone={0} BUY: TotalZones={1} BearZones={2} BearAbove={3} Entry={4:F2}",
                zone.Id, totalZones, bearZones.Count, bearZonesAbove.Count, entry));
            
            // Buscar HeatZones BEAR (resistencias) por encima del entry
            var allCandidates = bearZonesAbove
                .Select(z => {
                    // 1A: Usar borde más cercano (Low de zona BEAR = primer contacto)
                    double tp = z.Low;
                    double distance = Math.Abs(tp - entry);
                    
                    // 2A: ATR del TF dominante de la zona opuesta
                    int opposingZoneTF = z.TFDominante;
                    int idxOpposing = barData.GetBarIndexFromTime(opposingZoneTF, analysisTime);
                    double atrOpposing = (idxOpposing >= 0) ? 
                        barData.GetATR(opposingZoneTF, 14, idxOpposing) : atrFallback;
                    if (atrOpposing <= 0) atrOpposing = atrFallback; // safety fallback
                    
                    double distanceATR = distance / atrOpposing;
                    double rr = distance / riskDistance;
                    
                    // Scoring multi-criterio
                    double score = CalculateTPScore(z, rr, distanceATR);
                    
                    return new {
                        Zone = z,
                        TP = tp,
                        Distance = distance,
                        DistanceATR = distanceATR,
                        RR = rr,
                        Score = score,
                        ATROpposing = atrOpposing
                    };
                })
                .ToList();
            
            // DEBUG: Mostrar todos los candidatos antes de filtrar
            foreach (var c in allCandidates.Take(5)) // Mostrar top 5
            {
                _logger.Info(string.Format("[RISK][P0_DEBUG] Candidate: ZoneId={0} TP={1:F2} RR={2:F2} DistATR={3:F2} Score={4:F2}",
                    c.Zone.Id, c.TP, c.RR, c.DistanceATR, c.Score));
            }
            
            var afterRRFilter = allCandidates.Where(c => c.RR >= 1.2 && c.RR <= 3.0).ToList();
            var opposingCandidates = afterRRFilter.Where(c => c.DistanceATR >= 6.0 && c.DistanceATR <= 20.0)
                .OrderByDescending(c => c.Score)
                .ToList();
            
            _logger.Info(string.Format("[RISK][P0_DEBUG] Zone={0} BUY Filters: AllCandidates={1} AfterRR[1.2-3.0]={2} AfterDistATR[6-20]={3}",
                zone.Id, allCandidates.Count, afterRRFilter.Count, opposingCandidates.Count));

            if (opposingCandidates.Any())
            {
                var best = opposingCandidates.First();
                
                // Log traza P0_OPPOSING
                _logger.Info(string.Format(
                    "[RISK][TP_POLICY] Zone={0} P0_OPPOSING: ZoneId={1} Dir={2} TF={3} Score={4:F2} RR={5:F2} DistATR={6:F2} TP={7:F2} ATR={8:F2}",
                    zone.Id, best.Zone.Id, best.Zone.Direction, best.Zone.TFDominante, 
                    best.Score, best.RR, best.DistanceATR, best.TP, best.ATROpposing
                ));
                
                // Marcar como estructural y guardar metadata
                zone.Metadata["TP_Structural"] = true;
                zone.Metadata["TP_TargetTF"] = best.Zone.TFDominante;
                zone.Metadata["TP_OpposingZone"] = true;
                zone.Metadata["TP_OpposingZoneId"] = best.Zone.Id;
                
                return best.TP;
            }
            
            // Diagnóstico adicional: conteos arriba/abajo y por dirección
            int anyZonesAbove = snapshot.HeatZones.Where(z => z.Low > entry).Count();
            int anyZonesBelow = snapshot.HeatZones.Where(z => z.High < entry).Count();
            int sameDirAbove = snapshot.HeatZones.Where(z => z.Direction == zone.Direction && z.Low > entry).Count();
            int sameDirBelow = snapshot.HeatZones.Where(z => z.Direction == zone.Direction && z.High < entry).Count();
            _logger.Info(string.Format("[RISK][P0_DEBUG] Zone={0} Direction={1} Entry={2:F2} | AnyAbove={3} AnyBelow={4} | SameDirAbove={5} SameDirBelow={6}",
                zone.Id, zone.Direction, entry, anyZonesAbove, anyZonesBelow, sameDirAbove, sameDirBelow));
            
            // P0b: si no hay opuesta, usar cualquier HeatZone por encima (sin filtrar dirección). Target = High.
            var anyCandidates = snapshot.HeatZones
                .Where(z => z.Id != zone.Id) // Excluir la propia zona
                .Where(z => z.Low > entry) // Por encima del entry
                .Select(z => {
                    double tp = z.High; // Borde de salida para BUY
                    double distance = Math.Abs(tp - entry);
                    
                    // Guardarraíl: descartar TPs muy cercanos (V6.0f-OPT-B)
                    // ✅ Sin cap máximo: P0 puede ser lejano si es una zona de calidad
                    if (distance < 0.50) return null;
                    
                    int tf = z.TFDominante;
                    int idx = barData.GetBarIndexFromTime(tf, analysisTime);
                    double atrZ = (idx >= 0) ? barData.GetATR(tf, 14, idx) : atrFallback;
                    if (atrZ <= 0) atrZ = atrFallback;
                    double distATR = distance / atrZ;
                    double rr = distance / riskDistance;
                    double score = CalculateTPScore(z, rr, distATR);
                    return new { Zone = z, TP = tp, RR = rr, DistanceATR = distATR, Score = score, ATRz = atrZ };
                })
                .Where(c => c != null) // Filtrar nulls del guardarraíl
                .ToList();
            
            var anyFiltered = anyCandidates
                .Where(c => c.RR >= 1.2 && c.RR <= 3.0)
                .Where(c => c.DistanceATR >= 1.5 && c.DistanceATR <= _config.MaxTPDistanceATR)
                .OrderByDescending(c => c.Score)
                .ToList();
            
            _logger.Info(string.Format("[RISK][P0_DEBUG] Zone={0} BUY AnyDir Filters: All={1} AfterRR={2} AfterDistATR={3}",
                zone.Id, anyCandidates.Count, anyCandidates.Count(c => c.RR >= 1.2 && c.RR <= 3.0), anyFiltered.Count));
            
            if (anyFiltered.Any())
            {
                var bestAny = anyFiltered.First();
                _logger.Info(string.Format(
                    "[RISK][TP_POLICY] Zone={0} P0_ANY_DIR: ZoneId={1} Dir={2} TF={3} Score={4:F2} RR={5:F2} DistATR={6:F2} TP={7:F2} ATR={8:F2} Cap={9:F2}",
                    zone.Id, bestAny.Zone.Id, bestAny.Zone.Direction, bestAny.Zone.TFDominante,
                    bestAny.Score, bestAny.RR, bestAny.DistanceATR, bestAny.TP, bestAny.ATRz
                ));
                zone.Metadata["TP_Structural"] = true;
                zone.Metadata["TP_TargetTF"] = bestAny.Zone.TFDominante;
                zone.Metadata["TP_OpposingZone"] = false;
                zone.Metadata["TP_AnyDirZoneId"] = bestAny.Zone.Id;
                return bestAny.TP;
            }
            
            // P0c: Swing Lite (respaldo final) - Solo TF 15/60 con criterios relajados
            var swingCandidates = new List<int> { 15, 60 }
                .SelectMany(tf => coreEngine.GetAllStructures(tf).OfType<SwingInfo>())
                .Where(s => s.IsActive && s.IsHigh && s.High > entry)
                .Select(s => {
                    double tp = s.High;
                    double distance = Math.Abs(tp - entry);
                    
                    // Guardarraíl: distancia mínima (V6.0f-OPT-B)
                    // V6.0k: Respeta MaxTPDistanceATR de config
                    if (distance < 0.50) return null;
                    
                    // ATR del swing seleccionado
                    int idx = barData.GetBarIndexFromTime(s.TF, analysisTime);
                    double atrSwing = (idx >= 0) ? barData.GetATR(s.TF, 14, idx) : atrFallback;
                    if (atrSwing <= 0) atrSwing = atrFallback;
                    
                    double distATR = distance / atrSwing;
                    double rr = distance / riskDistance;
                    
                    // Filtros P0c - Respeta MaxTPDistanceATR de config
                    if (distATR < 1.5 || distATR > _config.MaxTPDistanceATR || rr < 1.2) return null;
                    
                    return new { Swing = s, TP = tp, RR = rr, DistATR = distATR, ATRSwing = atrSwing };
                })
                .Where(c => c != null)
                .OrderByDescending(c => c.RR)
                .ThenBy(c => c.DistATR)
                .FirstOrDefault();
            
            if (swingCandidates != null)
            {
                _logger.Info(string.Format(
                    "[RISK][TP_POLICY] Zone={0} P0_SWING_LITE: TF={1} Score={2:F2} RR={3:F2} DistATR={4:F2} TP={5:F2} ATR={6:F2}",
                    zone.Id, swingCandidates.Swing.TF, swingCandidates.Swing.Score,
                    swingCandidates.RR, swingCandidates.DistATR, swingCandidates.TP, swingCandidates.ATRSwing
                ));
                zone.Metadata["TP_Structural"] = true;
                zone.Metadata["TP_TargetTF"] = swingCandidates.Swing.TF;
                zone.Metadata["TP_OpposingZone"] = false;
                zone.Metadata["TP_SwingLiteId"] = swingCandidates.Swing.Id;
                return swingCandidates.TP;
            }
            
            _logger.Info(string.Format("[RISK][P0_DEBUG] Zone={0} BUY: No opposing/any-dir/swing-lite found", zone.Id));
            return null; // No hay candidato P0
        }

        /// <summary>
        /// V6.0f-FASE2: Busca HeatZone opuesta para TP (SHORT).
        /// P0: Nearest opposing HeatZone (soporte BULL debajo del entry)
        /// Configuración: Borde más cercano (High), ATR del TF opuesto, RR [1.2, 3.0], DistATR [6, 20]
        /// </summary>
        private double? GetOpposingZoneTP_Sell(
            HeatZone zone, 
            DecisionSnapshot snapshot, 
            IBarDataProvider barData,
            CoreEngine coreEngine,
            DateTime analysisTime,
            double entry, 
            double riskDistance,
            double atrFallback)
        {
            // DEBUG: Estadísticas iniciales
            int totalZones = snapshot.HeatZones.Count;
            var bullZones = snapshot.HeatZones.Where(z => z.Direction == "Bull").ToList();
            var bullZonesBelow = bullZones.Where(z => z.High < entry).ToList();
            
            _logger.Info(string.Format("[RISK][P0_DEBUG] Zone={0} SELL: TotalZones={1} BullZones={2} BullBelow={3} Entry={4:F2}",
                zone.Id, totalZones, bullZones.Count, bullZonesBelow.Count, entry));
            
            // Buscar HeatZones BULL (soportes) por debajo del entry
            var allCandidates = bullZonesBelow
                .Select(z => {
                    // 1A: Usar borde más cercano (High de zona BULL = primer contacto)
                    double tp = z.High;
                    double distance = Math.Abs(entry - tp);
                    
                    // 2A: ATR del TF dominante de la zona opuesta
                    int opposingZoneTF = z.TFDominante;
                    int idxOpposing = barData.GetBarIndexFromTime(opposingZoneTF, analysisTime);
                    double atrOpposing = (idxOpposing >= 0) ? 
                        barData.GetATR(opposingZoneTF, 14, idxOpposing) : atrFallback;
                    if (atrOpposing <= 0) atrOpposing = atrFallback; // safety fallback
                    
                    double distanceATR = distance / atrOpposing;
                    double rr = distance / riskDistance;
                    
                    // Scoring multi-criterio
                    double score = CalculateTPScore(z, rr, distanceATR);
                    
                    return new {
                        Zone = z,
                        TP = tp,
                        Distance = distance,
                        DistanceATR = distanceATR,
                        RR = rr,
                        Score = score,
                        ATROpposing = atrOpposing
                    };
                })
                .ToList();
            
            // DEBUG: Mostrar todos los candidatos antes de filtrar
            foreach (var c in allCandidates.Take(5)) // Mostrar top 5
            {
                _logger.Info(string.Format("[RISK][P0_DEBUG] Candidate: ZoneId={0} TP={1:F2} RR={2:F2} DistATR={3:F2} Score={4:F2}",
                    c.Zone.Id, c.TP, c.RR, c.DistanceATR, c.Score));
            }
            
            var afterRRFilter = allCandidates.Where(c => c.RR >= 1.2 && c.RR <= 3.0).ToList();
            var opposingCandidates = afterRRFilter.Where(c => c.DistanceATR >= 6.0 && c.DistanceATR <= 20.0)
                .OrderByDescending(c => c.Score)
                .ToList();
            
            _logger.Info(string.Format("[RISK][P0_DEBUG] Zone={0} SELL Filters: AllCandidates={1} AfterRR[1.2-3.0]={2} AfterDistATR[6-20]={3}",
                zone.Id, allCandidates.Count, afterRRFilter.Count, opposingCandidates.Count));

            if (opposingCandidates.Any())
            {
                var best = opposingCandidates.First();
                
                // Log traza P0_OPPOSING
                _logger.Info(string.Format(
                    "[RISK][TP_POLICY] Zone={0} P0_OPPOSING: ZoneId={1} Dir={2} TF={3} Score={4:F2} RR={5:F2} DistATR={6:F2} TP={7:F2} ATR={8:F2}",
                    zone.Id, best.Zone.Id, best.Zone.Direction, best.Zone.TFDominante, 
                    best.Score, best.RR, best.DistanceATR, best.TP, best.ATROpposing
                ));
                
                // Marcar como estructural y guardar metadata
                zone.Metadata["TP_Structural"] = true;
                zone.Metadata["TP_TargetTF"] = best.Zone.TFDominante;
                zone.Metadata["TP_OpposingZone"] = true;
                zone.Metadata["TP_OpposingZoneId"] = best.Zone.Id;
                
                return best.TP;
            }
            
            // Diagnóstico adicional: conteos arriba/abajo y por dirección
            int anyZonesAbove = snapshot.HeatZones.Where(z => z.Low > entry).Count();
            int anyZonesBelow = snapshot.HeatZones.Where(z => z.High < entry).Count();
            int sameDirAbove = snapshot.HeatZones.Where(z => z.Direction == zone.Direction && z.Low > entry).Count();
            int sameDirBelow = snapshot.HeatZones.Where(z => z.Direction == zone.Direction && z.High < entry).Count();
            _logger.Info(string.Format("[RISK][P0_DEBUG] Zone={0} Direction={1} Entry={2:F2} | AnyAbove={3} AnyBelow={4} | SameDirAbove={5} SameDirBelow={6}",
                zone.Id, zone.Direction, entry, anyZonesAbove, anyZonesBelow, sameDirAbove, sameDirBelow));
            
            // P0b: si no hay opuesta, usar cualquier HeatZone por debajo (sin filtrar dirección). Target = Low.
            var anyCandidates = snapshot.HeatZones
                .Where(z => z.Id != zone.Id) // Excluir la propia zona
                .Where(z => z.High < entry) // Por debajo del entry
                .Select(z => {
                    double tp = z.Low; // Borde de salida para SELL
                    double distance = Math.Abs(entry - tp);
                    
                    // Guardarraíl: descartar TPs muy cercanos (V6.0f-OPT-B)
                    // ✅ Sin cap máximo: P0 puede ser lejano si es una zona de calidad
                    if (distance < 0.50) return null;
                    
                    int tf = z.TFDominante;
                    int idx = barData.GetBarIndexFromTime(tf, analysisTime);
                    double atrZ = (idx >= 0) ? barData.GetATR(tf, 14, idx) : atrFallback;
                    if (atrZ <= 0) atrZ = atrFallback;
                    double distATR = distance / atrZ;
                    double rr = distance / riskDistance;
                    double score = CalculateTPScore(z, rr, distATR);
                    return new { Zone = z, TP = tp, RR = rr, DistanceATR = distATR, Score = score, ATRz = atrZ };
                })
                .Where(c => c != null) // Filtrar nulls del guardarraíl
                .ToList();
            
            var anyFiltered = anyCandidates
                .Where(c => c.RR >= 1.2 && c.RR <= 3.0)
                .Where(c => c.DistanceATR >= 1.5 && c.DistanceATR <= _config.MaxTPDistanceATR)
                .OrderByDescending(c => c.Score)
                .ToList();
            
            _logger.Info(string.Format("[RISK][P0_DEBUG] Zone={0} SELL AnyDir Filters: All={1} AfterRR={2} AfterDistATR={3}",
                zone.Id, anyCandidates.Count, anyCandidates.Count(c => c.RR >= 1.2 && c.RR <= 3.0), anyFiltered.Count));
            
            if (anyFiltered.Any())
            {
                var bestAny = anyFiltered.First();
                _logger.Info(string.Format(
                    "[RISK][TP_POLICY] Zone={0} P0_ANY_DIR: ZoneId={1} Dir={2} TF={3} Score={4:F2} RR={5:F2} DistATR={6:F2} TP={7:F2} ATR={8:F2}",
                    zone.Id, bestAny.Zone.Id, bestAny.Zone.Direction, bestAny.Zone.TFDominante,
                    bestAny.Score, bestAny.RR, bestAny.DistanceATR, bestAny.TP, bestAny.ATRz
                ));
                zone.Metadata["TP_Structural"] = true;
                zone.Metadata["TP_TargetTF"] = bestAny.Zone.TFDominante;
                zone.Metadata["TP_OpposingZone"] = false;
                zone.Metadata["TP_AnyDirZoneId"] = bestAny.Zone.Id;
                return bestAny.TP;
            }
            
            // P0c: Swing Lite (respaldo final) - Solo TF 15/60 con criterios relajados
            var swingCandidates = new List<int> { 15, 60 }
                .SelectMany(tf => coreEngine.GetAllStructures(tf).OfType<SwingInfo>())
                .Where(s => s.IsActive && !s.IsHigh && s.Low < entry)
                .Select(s => {
                    double tp = s.Low;
                    double distance = Math.Abs(entry - tp);
                    
                    // Guardarraíl: distancia mínima (V6.0f-OPT-B)
                    // V6.0k: Respeta MaxTPDistanceATR de config
                    if (distance < 0.50) return null;
                    
                    // ATR del swing seleccionado
                    int idx = barData.GetBarIndexFromTime(s.TF, analysisTime);
                    double atrSwing = (idx >= 0) ? barData.GetATR(s.TF, 14, idx) : atrFallback;
                    if (atrSwing <= 0) atrSwing = atrFallback;
                    
                    double distATR = distance / atrSwing;
                    double rr = distance / riskDistance;
                    
                    // Filtros P0c - Respeta MaxTPDistanceATR de config
                    if (distATR < 1.5 || distATR > _config.MaxTPDistanceATR || rr < 1.2) return null;
                    
                    return new { Swing = s, TP = tp, RR = rr, DistATR = distATR, ATRSwing = atrSwing };
                })
                .Where(c => c != null)
                .OrderByDescending(c => c.RR)
                .ThenBy(c => c.DistATR)
                .FirstOrDefault();
            
            if (swingCandidates != null)
            {
                _logger.Info(string.Format(
                    "[RISK][TP_POLICY] Zone={0} P0_SWING_LITE: TF={1} Score={2:F2} RR={3:F2} DistATR={4:F2} TP={5:F2} ATR={6:F2}",
                    zone.Id, swingCandidates.Swing.TF, swingCandidates.Swing.Score,
                    swingCandidates.RR, swingCandidates.DistATR, swingCandidates.TP, swingCandidates.ATRSwing
                ));
                zone.Metadata["TP_Structural"] = true;
                zone.Metadata["TP_TargetTF"] = swingCandidates.Swing.TF;
                zone.Metadata["TP_OpposingZone"] = false;
                zone.Metadata["TP_SwingLiteId"] = swingCandidates.Swing.Id;
                return swingCandidates.TP;
            }
            
            _logger.Info(string.Format("[RISK][P0_DEBUG] Zone={0} SELL: No opposing/any-dir/swing-lite found", zone.Id));
            return null; // No hay candidato P0
        }

        // ========================================================================
        // SCORING ESPECÍFICO PARA SL (NO USA PROXIMITY AL PRECIO ACTUAL)
        // ========================================================================

        /// <summary>
        /// Calcula la calidad estructural de un swing para uso como SL.
        /// NO considera proximity al precio actual (eso es para entradas, no SLs).
        /// Se basa en: tamaño estructural, ratio vs ATR, toques, e integridad.
        /// </summary>
        private double CalculateSLStructuralQuality(SwingInfo swing, int tf, double entry, double atr, double tickSize)
        {
            // Guardia: swings muy pequeños son ruido
            if (swing.SwingSizeTicks < 5)
                return 0.0;
            
            // 1. Tamaño estructural (swing range respecto a ATR)
            double swingRange = Math.Abs(swing.High - swing.Low);
            double structuralQuality = Math.Min(1.0, (swingRange / atr) / 3.0);  // 3 ATR = 1.0
            
            // 2. Ratio de tamaño (SwingSizeTicks respecto ATR)
            double swingSize = swing.SwingSizeTicks * tickSize;
            double sizeRatio = Math.Min(1.0, (swingSize / atr) / 2.0);  // 2 ATR = 1.0
            
            // 3. Touch count (validación por mercado)
            int totalTouches = swing.TouchCount_Body + swing.TouchCount_Wick;
            double touchScore = Math.Min(1.0, totalTouches / 5.0);  // 5 toques = 1.0
            
            // 4. Integridad (no roto)
            double integrityScore = swing.IsBroken ? 0.3 : 1.0;
            
            // Score ponderado
            double quality = 0.35 * structuralQuality +
                             0.30 * sizeRatio +
                             0.20 * touchScore +
                             0.15 * integrityScore;
            
            return quality;
        }
        
        /// <summary>
        /// ✅ SCORING MULTI-FACTOR INTELIGENTE PARA TPs (SIN HARDCODEO)
        /// Evalúa TPs basándose en: Calidad Estructural, Alcanzabilidad, RR, Recencia
        /// Permite que TPs lejanos pero de calidad compitan con fallbacks cercanos
        /// </summary>
        private double CalculateTPIntelligentScore(string tpType, double distance, double distATR, double rr, 
            double structuralScore, double ageHours, double atr)
        {
            // 1. CALIDAD ESTRUCTURAL (0-1): Prioridad por tipo de estructura
            double qualityScore = 0.0;
            if (tpType == "P1_Liquidity")
                qualityScore = 1.0; // Máxima prioridad: zonas de liquidez
            else if (tpType == "P3_Swing")
                qualityScore = 0.7 + (structuralScore * 0.3); // Swings: 0.7-1.0 según score
            else if (tpType == "P2_Structure")
                qualityScore = 0.6 + (structuralScore * 0.3); // FVG/OB/POI: 0.6-0.9
            else if (tpType == "P0_Zone")
                qualityScore = 0.5 + (structuralScore * 0.2); // Zonas: 0.5-0.7
            else if (tpType == "P4_Fallback")
                qualityScore = 0.3; // Fallback matemático: 0.3
            
            // 2. ALCANZABILIDAD (0-1): Distancia óptima en ATR (gaussiana centrada en 2.5 ATR)
            double optimalATR = 2.5;
            double sigma = 1.5; // Desviación estándar
            double achievability = Math.Exp(-Math.Pow((distATR - optimalATR) / sigma, 2));
            // Ejemplos: 1.0 ATR=0.36, 2.0 ATR=0.72, 2.5 ATR=1.0, 3.0 ATR=0.72, 6.0 ATR=0.08
            
            // 3. RISK:REWARD (0-1): Óptimo entre 1.5-2.5
            double rrScore = 0.0;
            if (rr >= 1.5 && rr <= 2.5)
                rrScore = 1.0; // Rango óptimo
            else if (rr < 1.5)
                rrScore = Math.Max(0, rr / 1.5); // Penalizar RR bajo
            else
                rrScore = Math.Max(0, 1.0 - (rr - 2.5) / 4.0); // Penalizar RR extremo suavemente
            
            // 4. RECENCIA (0-1): Estructuras recientes son más confiables
            double recencyScore = 0.0;
            if (ageHours <= 12)
                recencyScore = 1.0;
            else if (ageHours <= 24)
                recencyScore = 0.8;
            else if (ageHours <= 48)
                recencyScore = 0.6;
            else if (ageHours <= 72)
                recencyScore = 0.4;
            else
                recencyScore = 0.2;
            
            // SCORE FINAL PONDERADO
            // Achievability tiene mayor peso porque un TP inalcanzable no vale nada
            double finalScore = 
                0.30 * qualityScore +      // Calidad de la estructura
                0.35 * achievability +      // Probabilidad de alcanzar
                0.20 * rrScore +            // RR razonable
                0.15 * recencyScore;        // Recencia
            
            return finalScore;
        }
    }
}
