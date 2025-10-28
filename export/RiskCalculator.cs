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

        public string ComponentName => "RiskCalculator";

        public void Initialize(EngineConfig config, ILogger logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug("[RiskCalculator] Inicializado con lógica estructural inteligente");
        }

        public void Process(DecisionSnapshot snapshot, IBarDataProvider barData, CoreEngine coreEngine, int currentBar, double accountSize)
        {
            if (snapshot == null)
                throw new ArgumentNullException(nameof(snapshot));
            if (barData == null)
                throw new ArgumentNullException(nameof(barData));
            if (coreEngine == null)
                throw new ArgumentNullException(nameof(coreEngine));
            if (accountSize <= 0)
            {
                _logger.Warning("[RiskCalculator] AccountSize <= 0, no se puede calcular position size");
                return;
            }

            _logger.Debug("[RiskCalculator] Calculando riesgo estructural para HeatZones...");

            if (snapshot.HeatZones == null || snapshot.HeatZones.Count == 0)
            {
                _logger.Debug("[RiskCalculator] No hay HeatZones para calcular riesgo");
                return;
            }

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
                CalculateStructuralRiskLevels(zone, barData, coreEngine, currentBar, accountSize);
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

            // Resumen diagnóstico
            _logger.Info(string.Format("[DIAGNOSTICO][Risk] Accepted={0} RejSL={1} RejTP={2} RejRR={3} RejEntry={4}",
                accepted, rejSLTooFar, rejTPTooNear, rejRRLow, rejEntryTooFar));

            // Resumen histograma SL por alineación
            _logger.Info(string.Format("[DIAGNOSTICO][Risk] HistSL Aligned=0-10:{0},10-15:{1},15-20:{2},20-25:{3},25+:{4} | Counter=0-10:{5},10-15:{6},15-20:{7},20-25:{8},25+:{9}",
                binsAligned[0], binsAligned[1], binsAligned[2], binsAligned[3], binsAligned[4],
                binsCounter[0], binsCounter[1], binsCounter[2], binsCounter[3], binsCounter[4]));

            // Resumen diagnóstico: SLPick por bandas y TF
            _logger.Info(string.Format("[DIAGNOSTICO][Risk] SLPickBands: lt8:{0},8-10:{1},10-12.5:{2},12.5-15:{3},gt15:{4} | TF 5:{5},15:{6},60:{7},240:{8},1440:{9}",
                band_lt8, band_8_10, band_10_12_5, band_12_5_15, band_gt15, tf_5, tf_15, tf_60, tf_240, tf_1440));
            // Resumen RR planned por bandas 0-10 y 10-15
            _logger.Info(string.Format("[DIAGNOSTICO][Risk] RRPlanBands: 0-10={0:F2}(n={1}),10-15={2:F2}(n={3})",
                rr_n_0_10 > 0 ? rr_sum_0_10 / rr_n_0_10 : 0.0, rr_n_0_10,
                rr_n_10_15 > 0 ? rr_sum_10_15 / rr_n_10_15 : 0.0, rr_n_10_15));
        }

        /// <summary>
        /// Calcula Entry, SL, TP y PositionSize para una HeatZone usando ESTRUCTURA INTELIGENTE
        /// Añade los resultados a zone.Metadata
        /// </summary>
        private void CalculateStructuralRiskLevels(HeatZone zone, IBarDataProvider barData, CoreEngine coreEngine, int currentBar, double accountSize)
        {
            // Obtener ATR del TF Dominante
            double atr = barData.GetATR(zone.TFDominante, currentBar, 14);
            if (atr <= 0)
            {
                _logger.Warning(string.Format("[RiskCalculator] ATR({0}) es 0 para HeatZone {1}, usando ATR=1.0",
                    zone.TFDominante, zone.Id));
                atr = 1.0;
            }

            // VALIDACIÓN CRÍTICA: Verificar proximidad del Entry SOLO en tiempo real (no en backtest histórico)
            // En backtest, necesitamos ver TODAS las señales históricas para auditar el sistema
            if (!barData.IsHistorical)
            {
                double currentPrice = barData.GetClose(zone.TFDominante, currentBar);
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
                }
                else
                {
                    // Fallback a envolvente si no existe (robustez)
                    entryRaw = zone.Low;
                    _logger.Warning(string.Format("[RiskCalculator] HZ={0} DominantStructure not found, using zone envelope", zone.Id));
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
                
                // SL ESTRUCTURAL: Buscar Swing Low protector
                stopLoss = CalculateStructuralSL_Buy(zone, coreEngine, barData, currentBar, atr);
                if (zone.Metadata.ContainsKey("RejectLowTFTightSL") && (bool)zone.Metadata["RejectLowTFTightSL"])
                {
                    _logger.Info(string.Format("[DIAGNOSTICO][Risk] REJECT BUY: AllLowTFBelow10 (zona {0})", zone.Id));
                    zone.Metadata["RiskCalculated"] = false;
                    zone.Metadata["RejectReason"] = "SL demasiado ajustado (<10 ATR) - AllLowTFBelow10";
                    return;
                }
                
                // TP JERÁRQUICO: Buscar objetivo estructural
                takeProfit = CalculateStructuralTP_Buy(zone, coreEngine, barData, currentBar, entry, stopLoss);
            }
            else if (zone.Direction == "Bearish")
            {
                // SELL Limit Order: Entry anclado a estructura dominante + snap conservador (V5.7d)
                var dominantStructure = coreEngine.GetStructureById(zone.DominantStructureId);
                
                double entryRaw;
                if (dominantStructure != null)
                {
                    // Entry desde estructura dominante (SELL: usar Low = borde superior de la zona)
                    entryRaw = dominantStructure.Low;
                }
                else
                {
                    // Fallback a envolvente si no existe (robustez)
                    entryRaw = zone.High;
                    _logger.Warning(string.Format("[RiskCalculator] HZ={0} DominantStructure not found, using zone envelope", zone.Id));
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
                
                // SL ESTRUCTURAL: Buscar Swing High protector
                stopLoss = CalculateStructuralSL_Sell(zone, coreEngine, barData, currentBar, atr);
                if (zone.Metadata.ContainsKey("RejectLowTFTightSL") && (bool)zone.Metadata["RejectLowTFTightSL"])
                {
                    _logger.Info(string.Format("[DIAGNOSTICO][Risk] REJECT SELL: AllLowTFBelow10 (zona {0})", zone.Id));
                    zone.Metadata["RiskCalculated"] = false;
                    zone.Metadata["RejectReason"] = "SL demasiado ajustado (<10 ATR) - AllLowTFBelow10";
                    return;
                }
                
                // TP JERÁRQUICO: Buscar objetivo estructural
                takeProfit = CalculateStructuralTP_Sell(zone, coreEngine, barData, currentBar, entry, stopLoss);
            }
            else
            {
                // Zona Neutral: No se puede calcular riesgo
                _logger.Debug(string.Format("[RiskCalculator] HeatZone {0} es Neutral, no se calcula riesgo", zone.Id));
                zone.Metadata["RiskCalculated"] = false;
                return;
            }

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
            double slDistanceATR = riskDistance / atr;
            double tpDistanceATR = rewardDistance / atr;
            // Guardar drivers SIEMPRE (antes de posibles returns por rechazo)
            zone.Metadata["SLDistanceATR"] = slDistanceATR;
            zone.Metadata["TPDistanceATR"] = tpDistanceATR;

            // ========================================================================
            // VALIDACIONES DE RISK:REWARD (OPTIMIZACIÓN CRÍTICA)
            // ========================================================================
            
            // 1. Validar SL máximo (en ATR)
            if (slDistanceATR > _config.MaxSLDistanceATR)
            {
                // Clasificación por bin y alineación antes de return
                bool aligned = zone.Metadata.ContainsKey("AlignedWithBias") && (bool)zone.Metadata["AlignedWithBias"];
                int binIdx = 0;
                if (slDistanceATR < 10.0) binIdx = 0;
                else if (slDistanceATR < 15.0) binIdx = 1;
                else if (slDistanceATR < 20.0) binIdx = 2;
                else if (slDistanceATR < 25.0) binIdx = 3;
                else binIdx = 4;
                zone.Metadata["SLRejectedBin"] = binIdx;
                zone.Metadata["RejectedAligned"] = aligned;
                double proxDbg = zone.Metadata.ContainsKey("ProximityFactor") ? (double)zone.Metadata["ProximityFactor"] : 0.0;
                _logger.Info(string.Format(
                    "[DIAGNOSTICO][Risk] RejSL: Dir={0} Aligned={1} SLDistATR={2:F2} Bin={3} Prox={4:F3} Core={5:F3}",
                    zone.Direction, aligned, slDistanceATR, binIdx, proxDbg, zone.Score));
                _logger.Warning(string.Format(
                    "[RiskCalculator] ⚠ HeatZone {0} RECHAZADA: SL demasiado lejano ({1:F2} ATR > límite {2:F2} ATR)",
                    zone.Id, slDistanceATR, _config.MaxSLDistanceATR));
                
                zone.Metadata["RiskCalculated"] = false;
                zone.Metadata["RejectReason"] = string.Format("SL absurdo: {0:F2} ATR (límite: {1:F2})", slDistanceATR, _config.MaxSLDistanceATR);
                // Muestreo forense opcional
                if (_config.RiskDetailSamplingRate > 0 && (_riskRejectionCounter % _config.RiskDetailSamplingRate == 0))
                {
                    double currentPriceDbg = barData.GetClose(zone.TFDominante, currentBar);
                    _logger.Info(string.Format(
                        "[DIAGNOSTICO][Risk] DETALLE FORENSE: Zone={0} Dir={1} Entry={2:F2} SL={3:F2} TP={4:F2} Current={5:F2}",
                        zone.Id, zone.Direction, entry, stopLoss, takeProfit, currentPriceDbg));
                }
                _riskRejectionCounter++;
                return;
            }
            
            // 2. Validar TP mínimo (en ATR)
            if (tpDistanceATR < _config.MinTPDistanceATR)
            {
                _logger.Warning(string.Format(
                    "[RiskCalculator] ⚠ HeatZone {0} RECHAZADA: TP demasiado cercano ({1:F2} ATR < mínimo {2:F2} ATR)",
                    zone.Id, tpDistanceATR, _config.MinTPDistanceATR));
                
                zone.Metadata["RiskCalculated"] = false;
                zone.Metadata["RejectReason"] = string.Format("TP insuficiente: {0:F2} ATR (mínimo: {1:F2})", tpDistanceATR, _config.MinTPDistanceATR);
                if (_config.RiskDetailSamplingRate > 0 && (_riskRejectionCounter % _config.RiskDetailSamplingRate == 0))
                {
                    double currentPriceDbg = barData.GetClose(zone.TFDominante, currentBar);
                    _logger.Info(string.Format(
                        "[DIAGNOSTICO][Risk] DETALLE FORENSE: Zone={0} Dir={1} Entry={2:F2} SL={3:F2} TP={4:F2} Current={5:F2}",
                        zone.Id, zone.Direction, entry, stopLoss, takeProfit, currentPriceDbg));
                }
                _riskRejectionCounter++;
                return;
            }
            
            // 3. Validar R:R mínimo
            if (actualRR < _config.MinRiskRewardRatio)
            {
                _logger.Warning(string.Format(
                    "[RiskCalculator] ⚠ HeatZone {0} RECHAZADA: R:R insuficiente ({1:F2} < mínimo {2:F2})",
                    zone.Id, actualRR, _config.MinRiskRewardRatio));
                
                zone.Metadata["RiskCalculated"] = false;
                zone.Metadata["RejectReason"] = string.Format("R:R absurdo: {0:F2} (mínimo: {1:F2})", actualRR, _config.MinRiskRewardRatio);
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
        }

        // ========================================================================
        // CÁLCULO DE SL ESTRUCTURAL (SWING-BASED)
        // ========================================================================

        /// <summary>
        /// Calcula SL estructural para BUY: Swing Low protector - (ATR * buffer)
        /// Mínimo: Entry - (3.0 * ATR) [AUMENTADO PARA OPERABILIDAD]
        /// </summary>
        private double CalculateStructuralSL_Buy(HeatZone zone, CoreEngine coreEngine, IBarDataProvider barData, int currentBar, double atr)
        {
            double entry = zone.Low;
            double minSL = entry - (3.0 * atr); // SL mínimo de seguridad (AUMENTADO de 1.5 a 3.0)

            // Buscar Swing Low protector (debajo de la zona)
            var swingLowPick = FindProtectiveSwingLowBanded(zone, coreEngine, barData, atr, entry, true);
            var swingLow = swingLowPick != null ? swingLowPick.Item1 : null;
            int swingTf = swingLowPick != null ? swingLowPick.Item2 : -1;

            if (swingLow != null)
            {
                // SL = Swing Low - (ATR * buffer)
                double structuralSL = swingLow.Low - (_config.SL_BufferATR * atr);
                
                // Aplicar filtro de mínimo
                double finalSL = Math.Min(structuralSL, minSL);
                // Diagnóstico selección de swing (BUY)
                double slDistAtrPick = Math.Abs(entry - finalSL) / atr;
                zone.Metadata["SL_SwingTF"] = swingTf;
                zone.Metadata["SL_Structural"] = true;
                zone.Metadata["SL_SwingPrice"] = swingLow.Low;
                _logger.Info(string.Format("[DIAGNOSTICO][Risk] SLPick BUY: Zone={0} SwingTF={1} SwingPrice={2:F2} SLDistATR={3:F2}",
                    zone.Id, swingTf, swingLow.Low, slDistAtrPick));
                
                _logger.Info(string.Format("[RiskCalculator] ✓ SL estructural (BUY): SwingLow={0:F2}, StructuralSL={1:F2}, FinalSL={2:F2}, MinSL={3:F2}",
                    swingLow.Low, structuralSL, finalSL, minSL));
                
                return finalSL;
            }
            else
            {
                // No se encontró Swing protector, usar SL mínimo
                _logger.Warning(string.Format("[RiskCalculator] ⚠ No se encontró Swing Low protector para HeatZone {0} (TF={1}), usando SL mínimo: {2:F2}",
                    zone.Id, zone.TFDominante, minSL));
                // SL mínimo implica que no se encontró swing protector válido
                zone.Metadata["SL_Structural"] = false;
                zone.Metadata["SL_SwingTF"] = -1;
                return minSL;
            }
        }

        /// <summary>
        /// Calcula SL estructural para SELL: Swing High protector + (ATR * buffer)
        /// Mínimo: Entry + (3.0 * ATR) [AUMENTADO PARA OPERABILIDAD]
        /// </summary>
        private double CalculateStructuralSL_Sell(HeatZone zone, CoreEngine coreEngine, IBarDataProvider barData, int currentBar, double atr)
        {
            double entry = zone.High;
            double minSL = entry + (3.0 * atr); // SL mínimo de seguridad (AUMENTADO de 1.5 a 3.0)

            // Buscar Swing High protector (encima de la zona)
            var swingHighPick = FindProtectiveSwingHighBanded(zone, coreEngine, barData, atr, entry, true);
            var swingHigh = swingHighPick != null ? swingHighPick.Item1 : null;
            int swingTf = swingHighPick != null ? swingHighPick.Item2 : -1;

            if (swingHigh != null)
            {
                // SL = Swing High + (ATR * buffer)
                double structuralSL = swingHigh.High + (_config.SL_BufferATR * atr);
                
                // Aplicar filtro de mínimo
                double finalSL = Math.Max(structuralSL, minSL);
                // Diagnóstico selección de swing (SELL)
                double slDistAtrPick = Math.Abs(finalSL - entry) / atr;
                zone.Metadata["SL_SwingTF"] = swingTf;
                zone.Metadata["SL_Structural"] = true;
                zone.Metadata["SL_SwingPrice"] = swingHigh.High;
                _logger.Info(string.Format("[DIAGNOSTICO][Risk] SLPick SELL: Zone={0} SwingTF={1} SwingPrice={2:F2} SLDistATR={3:F2}",
                    zone.Id, swingTf, swingHigh.High, slDistAtrPick));
                
                _logger.Info(string.Format("[RiskCalculator] ✓ SL estructural (SELL): SwingHigh={0:F2}, StructuralSL={1:F2}, FinalSL={2:F2}, MinSL={3:F2}",
                    swingHigh.High, structuralSL, finalSL, minSL));
                
                return finalSL;
            }
            else
            {
                // No se encontró Swing protector, usar SL mínimo
                _logger.Warning(string.Format("[RiskCalculator] ⚠ No se encontró Swing High protector para HeatZone {0} (TF={1}), usando SL mínimo: {2:F2}",
                    zone.Id, zone.TFDominante, minSL));
                zone.Metadata["SL_Structural"] = false;
                zone.Metadata["SL_SwingTF"] = -1;
                return minSL;
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
        private double CalculateStructuralTP_Buy(HeatZone zone, CoreEngine coreEngine, IBarDataProvider barData, int currentBar, double entry, double stopLoss)
        {
            double riskDistance = entry - stopLoss;
            double fallbackTP = entry + (riskDistance * _config.MinRiskRewardRatio);

            _logger.Info(string.Format("[RiskCalculator] === CALCULANDO TP JERÁRQUICO (BUY) para HeatZone {0} ===", zone.Id));
            _logger.Info(string.Format("[RiskCalculator] Entry={0:F2}, SL={1:F2}, RiskDistance={2:F2}, FallbackTP={3:F2}",
                entry, stopLoss, riskDistance, fallbackTP));

            // LOG DETALLADO: Recopilar TODOS los candidatos TP para análisis post-mortem
            var allCandidates = new List<Tuple<string, string, double, int, double, double, int>>(); // (priority, type, score, tf, price, distATR, age)
            double atr = barData.GetATR(zone.TFDominante, currentBar, 14);

            // Recopilar Liquidity targets
            foreach (var tf in _config.TimeframesToUse.OrderByDescending(t => t))
            {
                var liquidities = coreEngine.GetAllStructures(tf)
                    .Where(s => (s.Type == "LiquidityGrabInfo" || s.Type == "LiquidityVoidInfo") && s.IsActive && s.Low > zone.High)
                    .ToList();
                foreach (var liq in liquidities)
                {
                    double tpPrice = liq.Low;
                    double distATR = Math.Abs(tpPrice - entry) / atr;
                    double potentialRR = (tpPrice - entry) / riskDistance;
                    int currentBarInStructureTF = barData.GetCurrentBarIndex(liq.TF);
                    int age = currentBarInStructureTF - liq.CreatedAtBarIndex;
                    allCandidates.Add(Tuple.Create("P1", liq.Type, liq.Score, liq.TF, tpPrice, distATR, age));
                }
            }
            // Recopilar FVG/OB/POI opuestos
            foreach (var tf in _config.TimeframesToUse.OrderByDescending(t => t))
            {
                var structures = coreEngine.GetAllStructures(tf)
                    .Where(s => (s.Type == "FairValueGapInfo" || s.Type == "OrderBlockInfo" || s.Type == "PointOfInterestInfo") 
                             && s.IsActive && s.Score >= 0.7 && s.Low > zone.High)
                    .ToList();
                foreach (var str in structures)
                {
                    double tpPrice = str.Low;
                    double distATR = Math.Abs(tpPrice - entry) / atr;
                    double potentialRR = (tpPrice - entry) / riskDistance;
                    int currentBarInStructureTF = barData.GetCurrentBarIndex(str.TF);
                    int age = currentBarInStructureTF - str.CreatedAtBarIndex;
                    allCandidates.Add(Tuple.Create("P2", str.Type, str.Score, str.TF, tpPrice, distATR, age));
                }
            }
            // Recopilar Swings
            foreach (var tf in _config.TimeframesToUse)
            {
                var swings = coreEngine.GetAllStructures(tf)
                    .OfType<SwingInfo>()
                    .Where(s => s.IsActive && s.IsHigh && s.High > zone.High)
                    .ToList();
                foreach (var sw in swings)
                {
                    double tpPrice = sw.High;
                    double distATR = Math.Abs(tpPrice - entry) / atr;
                    double potentialRR = (tpPrice - entry) / riskDistance;
                    int currentBarInStructureTF = barData.GetCurrentBarIndex(sw.TF);
                    int age = currentBarInStructureTF - sw.CreatedAtBarIndex;
                    allCandidates.Add(Tuple.Create("P3", "Swing", sw.Score, sw.TF, tpPrice, distATR, age));
                }
            }

            // Log todos los candidatos
            _logger.Info(string.Format("[DIAGNOSTICO][Risk] TP_CANDIDATES: Zone={0} Dir=BUY TotalCandidates={1}", zone.Id, allCandidates.Count));
            int idx = 0;
            foreach (var c in allCandidates.OrderBy(x => x.Item1).ThenBy(x => x.Item6))
            {
                idx++;
                double potentialRR = (c.Item5 - entry) / riskDistance;
                _logger.Info(string.Format(
                    "[DIAGNOSTICO][Risk] TP_CANDIDATE: Idx={0} Priority={1} Type={2} Score={3:F2} TF={4} DistATR={5:F2} Age={6} Price={7:F2} RR={8:F2}",
                    idx, c.Item1, c.Item2, c.Item3, c.Item4, c.Item6, c.Item7, c.Item5, potentialRR));
            }

            // 1. PRIORIDAD 1: Buscar Liquidity Grab/Void opuesto (arriba)
            _logger.Debug("[RiskCalculator] [P1] Buscando Liquidity Grab/Void encima de la zona...");
            var liquidityTarget = FindLiquidityTarget_Above(zone, coreEngine, barData);
            if (liquidityTarget != null)
            {
                double tp = liquidityTarget.Low; // Target en el borde inferior del LG/LV
                if (tp > entry)
                {
                    // CRÍTICO: Calcular edad en barras del TF de la estructura, no del gráfico
                    int currentBarInStructureTF = barData.GetCurrentBarIndex(liquidityTarget.TF);
                    int ageSelected = currentBarInStructureTF - liquidityTarget.CreatedAtBarIndex;
                    double distATRSelected = Math.Abs(tp - entry) / atr;
                    double rrSelected = (tp - entry) / riskDistance;
                    _logger.Info(string.Format("[RiskCalculator] ✓ [P1] TP SELECCIONADO: Liquidity ({0}) @ {1:F2}, R:R={2:F2}",
                        liquidityTarget.Type, tp, rrSelected));
                    _logger.Info(string.Format("[DIAGNOSTICO][Risk] TP_SELECTED: Zone={0} Priority=P1 Type={1} Score={2:F2} TF={3} DistATR={4:F2} Age={5} Price={6:F2} RR={7:F2} Reason=Liquidity",
                        zone.Id, liquidityTarget.Type, liquidityTarget.Score, liquidityTarget.TF, distATRSelected, ageSelected, tp, rrSelected));
                    zone.Metadata["TP_Structural"] = true;
                    zone.Metadata["TP_TargetTF"] = liquidityTarget.TF;
                    return tp;
                }
                else
                {
                    _logger.Debug(string.Format("[RiskCalculator] [P1] Liquidity encontrado pero TP={0:F2} <= Entry={1:F2}, descartado",
                        tp, entry));
                }
            }
            else
            {
                _logger.Debug("[RiskCalculator] [P1] No se encontró Liquidity Grab/Void encima");
            }

            // 2. PRIORIDAD 2: Buscar FVG/OB opuesto con Score > 0.7
            _logger.Debug("[RiskCalculator] [P2] Buscando FVG/OB/POI opuesto (Score>0.7) encima de la zona...");
            var structureTarget = FindOpposingStructure_Above(zone, coreEngine, barData, 0.7);
            if (structureTarget != null)
            {
                double tp = structureTarget.Low; // Target en el borde inferior
                if (tp > entry)
                {
                    // CRÍTICO: Calcular edad en barras del TF de la estructura, no del gráfico
                    int currentBarInStructureTF = barData.GetCurrentBarIndex(structureTarget.TF);
                    int ageSelected = currentBarInStructureTF - structureTarget.CreatedAtBarIndex;
                    double distATRSelected = Math.Abs(tp - entry) / atr;
                    double rrSelected = (tp - entry) / riskDistance;
                    _logger.Info(string.Format("[RiskCalculator] ✓ [P2] TP SELECCIONADO: Structure ({0}, Score={1:F2}) @ {2:F2}, R:R={3:F2}",
                        structureTarget.Type, structureTarget.Score, tp, rrSelected));
                    _logger.Info(string.Format("[DIAGNOSTICO][Risk] TP_SELECTED: Zone={0} Priority=P2 Type={1} Score={2:F2} TF={3} DistATR={4:F2} Age={5} Price={6:F2} RR={7:F2} Reason=Structure_HighScore",
                        zone.Id, structureTarget.Type, structureTarget.Score, structureTarget.TF, distATRSelected, ageSelected, tp, rrSelected));
                    zone.Metadata["TP_Structural"] = true;
                    zone.Metadata["TP_TargetTF"] = structureTarget.TF;
                    return tp;
                }
                else
                {
                    _logger.Debug(string.Format("[RiskCalculator] [P2] Structure encontrado pero TP={0:F2} <= Entry={1:F2}, descartado",
                        tp, entry));
                }
            }
            else
            {
                _logger.Debug("[RiskCalculator] [P2] No se encontró FVG/OB/POI opuesto con Score>0.7");
            }

            // 3. PRIORIDAD 3: Buscar Swing High opuesto
            _logger.Debug("[RiskCalculator] [P3] Buscando Swing High encima de la zona...");
            var swingTarget = FindSwingHigh_Above(zone, coreEngine, barData);
            if (swingTarget != null)
            {
                double tp = swingTarget.High;
                double potentialRR = (tp - entry) / riskDistance;
                
                // FILTRO CRÍTICO: Rechazar TPs con R:R absurdo (> 10)
                // Si R:R es razonable (< 10), aceptar el TP sin importar la distancia
                // Solo aplicar filtro de distancia si R:R es muy alto
                double maxTPDistance = 50.0 * atr; // Máximo 50 × ATR de distancia (más realista para ES)
                double tpDistance = tp - entry;
                
                // Aceptar si: TP válido Y (R:R razonable O distancia aceptable)
                bool validRR = potentialRR <= 10.0;
                bool validDistance = tpDistance <= maxTPDistance;
                
                if (tp > entry && (validRR || validDistance))
                {
                    // CRÍTICO: Calcular edad en barras del TF de la estructura, no del gráfico
                    int currentBarInStructureTF = barData.GetCurrentBarIndex(swingTarget.TF);
                    int ageSelected = currentBarInStructureTF - swingTarget.CreatedAtBarIndex;
                    double distATRSelected = Math.Abs(tp - entry) / atr;
                    string reason = validRR && validDistance ? "R:R y Distancia OK" 
                        : validRR ? "R:R OK (Distancia ignorada)" 
                        : "Distancia OK (R:R ignorado)";
                    _logger.Info(string.Format("[RiskCalculator] ✓ [P3] TP SELECCIONADO: Swing High @ {0:F2}, R:R={1:F2} ({2})",
                        tp, potentialRR, reason));
                    _logger.Info(string.Format("[DIAGNOSTICO][Risk] TP_SELECTED: Zone={0} Priority=P3 Type=Swing Score={1:F2} TF={2} DistATR={3:F2} Age={4} Price={5:F2} RR={6:F2} Reason={7}",
                        zone.Id, swingTarget.Score, swingTarget.TF, distATRSelected, ageSelected, tp, potentialRR, reason.Replace(" ", "_")));
                    zone.Metadata["TP_Structural"] = true;
                    zone.Metadata["TP_TargetTF"] = swingTarget.TF;
                    return tp;
                }
                else
                {
                    _logger.Warning(string.Format("[RiskCalculator] [P3] Swing High encontrado @ {0:F2} pero RECHAZADO: R:R={1:F2} (max 10.0) Y Distancia={2:F2} (max {3:F2}) - AMBOS FALLAN",
                        tp, potentialRR, tpDistance, maxTPDistance));
                }
            }
            else
            {
                _logger.Debug("[RiskCalculator] [P3] No se encontró Swing High encima");
            }

            // 4. FALLBACK: R:R mínimo
            _logger.Warning(string.Format("[RiskCalculator] ⚠ [P4] FALLBACK: No se encontró target estructural válido, usando R:R mínimo @ {0:F2}",
                fallbackTP));
            double distATRFallback = Math.Abs(fallbackTP - entry) / atr;
            _logger.Info(string.Format("[DIAGNOSTICO][Risk] TP_SELECTED: Zone={0} Priority=P4_Fallback Type=Calculated Score=0.00 TF=-1 DistATR={1:F2} Age=0 Price={2:F2} RR={3:F2} Reason=NoStructuralTarget",
                zone.Id, distATRFallback, fallbackTP, _config.MinRiskRewardRatio));
            zone.Metadata["TP_Structural"] = false;
            zone.Metadata["TP_TargetTF"] = -1;
            return fallbackTP;
        }

        /// <summary>
        /// Calcula TP jerárquico para SELL:
        /// 1. Liquidity Grab/Void opuesto (abajo)
        /// 2. FVG/OB opuesto con Score > 0.7
        /// 3. Swing Low opuesto
        /// 4. Fallback: Entry - (SL - Entry) * MinRiskRewardRatio
        /// </summary>
        private double CalculateStructuralTP_Sell(HeatZone zone, CoreEngine coreEngine, IBarDataProvider barData, int currentBar, double entry, double stopLoss)
        {
            double riskDistance = stopLoss - entry;
            double fallbackTP = entry - (riskDistance * _config.MinRiskRewardRatio);

            _logger.Info(string.Format("[RiskCalculator] === CALCULANDO TP JERÁRQUICO (SELL) para HeatZone {0} ===", zone.Id));
            _logger.Info(string.Format("[RiskCalculator] Entry={0:F2}, SL={1:F2}, RiskDistance={2:F2}, FallbackTP={3:F2}",
                entry, stopLoss, riskDistance, fallbackTP));

            // LOG DETALLADO: Recopilar TODOS los candidatos TP para análisis post-mortem
            var allCandidates = new List<Tuple<string, string, double, int, double, double, int>>(); // (priority, type, score, tf, price, distATR, age)
            double atr = barData.GetATR(zone.TFDominante, currentBar, 14);

            // Recopilar Liquidity targets
            foreach (var tf in _config.TimeframesToUse.OrderByDescending(t => t))
            {
                var liquidities = coreEngine.GetAllStructures(tf)
                    .Where(s => (s.Type == "LiquidityGrabInfo" || s.Type == "LiquidityVoidInfo") && s.IsActive && s.High < zone.Low)
                    .ToList();
                foreach (var liq in liquidities)
                {
                    double tpPrice = liq.High;
                    double distATR = Math.Abs(entry - tpPrice) / atr;
                    int currentBarInStructureTF = barData.GetCurrentBarIndex(liq.TF);
                    int age = currentBarInStructureTF - liq.CreatedAtBarIndex;
                    allCandidates.Add(Tuple.Create("P1", liq.Type, liq.Score, liq.TF, tpPrice, distATR, age));
                }
            }
            // Recopilar FVG/OB/POI opuestos
            foreach (var tf in _config.TimeframesToUse.OrderByDescending(t => t))
            {
                var structures = coreEngine.GetAllStructures(tf)
                    .Where(s => (s.Type == "FairValueGapInfo" || s.Type == "OrderBlockInfo" || s.Type == "PointOfInterestInfo") 
                             && s.IsActive && s.Score >= 0.7 && s.High < zone.Low)
                    .ToList();
                foreach (var str in structures)
                {
                    double tpPrice = str.High;
                    double distATR = Math.Abs(entry - tpPrice) / atr;
                    int currentBarInStructureTF = barData.GetCurrentBarIndex(str.TF);
                    int age = currentBarInStructureTF - str.CreatedAtBarIndex;
                    allCandidates.Add(Tuple.Create("P2", str.Type, str.Score, str.TF, tpPrice, distATR, age));
                }
            }
            // Recopilar Swings
            foreach (var tf in _config.TimeframesToUse)
            {
                var swings = coreEngine.GetAllStructures(tf)
                    .OfType<SwingInfo>()
                    .Where(s => s.IsActive && !s.IsHigh && s.Low < zone.Low)
                    .ToList();
                foreach (var sw in swings)
                {
                    double tpPrice = sw.Low;
                    double distATR = Math.Abs(entry - tpPrice) / atr;
                    int currentBarInStructureTF = barData.GetCurrentBarIndex(sw.TF);
                    int age = currentBarInStructureTF - sw.CreatedAtBarIndex;
                    allCandidates.Add(Tuple.Create("P3", "Swing", sw.Score, sw.TF, tpPrice, distATR, age));
                }
            }

            // Log todos los candidatos
            _logger.Info(string.Format("[DIAGNOSTICO][Risk] TP_CANDIDATES: Zone={0} Dir=SELL TotalCandidates={1}", zone.Id, allCandidates.Count));
            int idx = 0;
            foreach (var c in allCandidates.OrderBy(x => x.Item1).ThenBy(x => x.Item6))
            {
                idx++;
                double potentialRR = (entry - c.Item5) / riskDistance;
                _logger.Info(string.Format(
                    "[DIAGNOSTICO][Risk] TP_CANDIDATE: Idx={0} Priority={1} Type={2} Score={3:F2} TF={4} DistATR={5:F2} Age={6} Price={7:F2} RR={8:F2}",
                    idx, c.Item1, c.Item2, c.Item3, c.Item4, c.Item6, c.Item7, c.Item5, potentialRR));
            }

            // 1. PRIORIDAD 1: Buscar Liquidity Grab/Void opuesto (abajo)
            _logger.Debug("[RiskCalculator] [P1] Buscando Liquidity Grab/Void debajo de la zona...");
            var liquidityTarget = FindLiquidityTarget_Below(zone, coreEngine, barData);
            if (liquidityTarget != null)
            {
                double tp = liquidityTarget.High; // Target en el borde superior del LG/LV
                if (tp < entry)
                {
                    // CRÍTICO: Calcular edad en barras del TF de la estructura, no del gráfico
                    int currentBarInStructureTF = barData.GetCurrentBarIndex(liquidityTarget.TF);
                    int ageSelected = currentBarInStructureTF - liquidityTarget.CreatedAtBarIndex;
                    double distATRSelected = Math.Abs(entry - tp) / atr;
                    double rrSelected = (entry - tp) / riskDistance;
                    _logger.Info(string.Format("[RiskCalculator] ✓ [P1] TP SELECCIONADO: Liquidity ({0}) @ {1:F2}, R:R={2:F2}",
                        liquidityTarget.Type, tp, rrSelected));
                    _logger.Info(string.Format("[DIAGNOSTICO][Risk] TP_SELECTED: Zone={0} Priority=P1 Type={1} Score={2:F2} TF={3} DistATR={4:F2} Age={5} Price={6:F2} RR={7:F2} Reason=Liquidity",
                        zone.Id, liquidityTarget.Type, liquidityTarget.Score, liquidityTarget.TF, distATRSelected, ageSelected, tp, rrSelected));
                    zone.Metadata["TP_Structural"] = true;
                    zone.Metadata["TP_TargetTF"] = liquidityTarget.TF;
                    return tp;
                }
                else
                {
                    _logger.Debug(string.Format("[RiskCalculator] [P1] Liquidity encontrado pero TP={0:F2} >= Entry={1:F2}, descartado",
                        tp, entry));
                }
            }
            else
            {
                _logger.Debug("[RiskCalculator] [P1] No se encontró Liquidity Grab/Void debajo");
            }

            // 2. PRIORIDAD 2: Buscar FVG/OB opuesto con Score > 0.7
            _logger.Debug("[RiskCalculator] [P2] Buscando FVG/OB/POI opuesto (Score>0.7) debajo de la zona...");
            var structureTarget = FindOpposingStructure_Below(zone, coreEngine, barData, 0.7);
            if (structureTarget != null)
            {
                double tp = structureTarget.High; // Target en el borde superior
                if (tp < entry)
                {
                    // CRÍTICO: Calcular edad en barras del TF de la estructura, no del gráfico
                    int currentBarInStructureTF = barData.GetCurrentBarIndex(structureTarget.TF);
                    int ageSelected = currentBarInStructureTF - structureTarget.CreatedAtBarIndex;
                    double distATRSelected = Math.Abs(entry - tp) / atr;
                    double rrSelected = (entry - tp) / riskDistance;
                    _logger.Info(string.Format("[RiskCalculator] ✓ [P2] TP SELECCIONADO: Structure ({0}, Score={1:F2}) @ {2:F2}, R:R={3:F2}",
                        structureTarget.Type, structureTarget.Score, tp, rrSelected));
                    _logger.Info(string.Format("[DIAGNOSTICO][Risk] TP_SELECTED: Zone={0} Priority=P2 Type={1} Score={2:F2} TF={3} DistATR={4:F2} Age={5} Price={6:F2} RR={7:F2} Reason=Structure_HighScore",
                        zone.Id, structureTarget.Type, structureTarget.Score, structureTarget.TF, distATRSelected, ageSelected, tp, rrSelected));
                    zone.Metadata["TP_Structural"] = true;
                    zone.Metadata["TP_TargetTF"] = structureTarget.TF;
                    return tp;
                }
                else
                {
                    _logger.Debug(string.Format("[RiskCalculator] [P2] Structure encontrado pero TP={0:F2} >= Entry={1:F2}, descartado",
                        tp, entry));
                }
            }
            else
            {
                _logger.Debug("[RiskCalculator] [P2] No se encontró FVG/OB/POI opuesto con Score>0.7");
            }

            // 3. PRIORIDAD 3: Buscar Swing Low opuesto
            _logger.Debug("[RiskCalculator] [P3] Buscando Swing Low debajo de la zona...");
            var swingTarget = FindSwingLow_Below(zone, coreEngine, barData);
            if (swingTarget != null)
            {
                double tp = swingTarget.Low;
                double potentialRR = (entry - tp) / riskDistance;
                
                // FILTRO CRÍTICO: Rechazar TPs con R:R absurdo (> 10)
                // Si R:R es razonable (< 10), aceptar el TP sin importar la distancia
                // Solo aplicar filtro de distancia si R:R es muy alto
                double maxTPDistance = 50.0 * atr; // Máximo 50 × ATR de distancia (más realista para ES)
                double tpDistance = entry - tp;
                
                // Aceptar si: TP válido Y (R:R razonable O distancia aceptable)
                bool validRR = potentialRR <= 10.0;
                bool validDistance = tpDistance <= maxTPDistance;
                
                if (tp < entry && (validRR || validDistance))
                {
                    // CRÍTICO: Calcular edad en barras del TF de la estructura, no del gráfico
                    int currentBarInStructureTF = barData.GetCurrentBarIndex(swingTarget.TF);
                    int ageSelected = currentBarInStructureTF - swingTarget.CreatedAtBarIndex;
                    double distATRSelected = Math.Abs(entry - tp) / atr;
                    string reason = validRR && validDistance ? "R:R y Distancia OK" 
                        : validRR ? "R:R OK (Distancia ignorada)" 
                        : "Distancia OK (R:R ignorado)";
                    _logger.Info(string.Format("[RiskCalculator] ✓ [P3] TP SELECCIONADO: Swing Low @ {0:F2}, R:R={1:F2} ({2})",
                        tp, potentialRR, reason));
                    _logger.Info(string.Format("[DIAGNOSTICO][Risk] TP_SELECTED: Zone={0} Priority=P3 Type=Swing Score={1:F2} TF={2} DistATR={3:F2} Age={4} Price={5:F2} RR={6:F2} Reason={7}",
                        zone.Id, swingTarget.Score, swingTarget.TF, distATRSelected, ageSelected, tp, potentialRR, reason.Replace(" ", "_")));
                    zone.Metadata["TP_Structural"] = true;
                    zone.Metadata["TP_TargetTF"] = swingTarget.TF;
                    return tp;
                }
                else
                {
                    _logger.Warning(string.Format("[RiskCalculator] [P3] Swing Low encontrado @ {0:F2} pero RECHAZADO: R:R={1:F2} (max 10.0) Y Distancia={2:F2} (max {3:F2}) - AMBOS FALLAN",
                        tp, potentialRR, tpDistance, maxTPDistance));
                }
            }
            else
            {
                _logger.Debug("[RiskCalculator] [P3] No se encontró Swing Low debajo");
            }

            // 4. FALLBACK: R:R mínimo
            _logger.Warning(string.Format("[RiskCalculator] ⚠ [P4] FALLBACK: No se encontró target estructural válido, usando R:R mínimo @ {0:F2}",
                fallbackTP));
            double distATRFallback = Math.Abs(entry - fallbackTP) / atr;
            _logger.Info(string.Format("[DIAGNOSTICO][Risk] TP_SELECTED: Zone={0} Priority=P4_Fallback Type=Calculated Score=0.00 TF=-1 DistATR={1:F2} Age=0 Price={2:F2} RR={3:F2} Reason=NoStructuralTarget",
                zone.Id, distATRFallback, fallbackTP, _config.MinRiskRewardRatio));
            zone.Metadata["TP_Structural"] = false;
            zone.Metadata["TP_TargetTF"] = -1;
            return fallbackTP;
        }

        // ========================================================================
        // HELPERS: BÚSQUEDA DE ESTRUCTURAS PROTECTORAS Y OBJETIVOS
        // ========================================================================

        /// <summary>
        /// Busca el Swing Low más cercano DEBAJO de la HeatZone (protector para BUY)
        /// Busca en TFs altos (4H, 1D) para mayor robustez
        /// </summary>
        private Tuple<SwingInfo,int> FindProtectiveSwingLowBanded(HeatZone zone, CoreEngine coreEngine, IBarDataProvider barData, double atr, double entry, bool prioritizeTf)
        {
            // Priorizar TFs >= 60 si existen
            IEnumerable<int> tfs = prioritizeTf
                ? _config.TimeframesToUse.OrderBy(tf => tf < 60 ? 1 : 0).ThenBy(tf => tf)
                : _config.TimeframesToUse.AsEnumerable();
            var candidates = new List<Tuple<SwingInfo,int,double>>(); // (swing, tf, slDistAtr)
            int rejectedByAge = 0;
            foreach (var tf in tfs)
            {
                var allStructures = coreEngine.GetAllStructures(tf);
                foreach (var s in allStructures.OfType<SwingInfo>())
                {
                    if (s.IsActive && !s.IsHigh && s.Low < zone.Low)
                    {
                        // V5.7c: FILTRO DE EDAD - Rechazar estructuras obsoletas
                        int currentBarInStructureTF = barData.GetCurrentBarIndex(s.TF);
                        int age = currentBarInStructureTF - s.CreatedAtBarIndex;
                        int maxAge = _config.MaxAgeForSL_ByTF.ContainsKey(s.TF) ? _config.MaxAgeForSL_ByTF[s.TF] : 100;
                        
                        if (age > maxAge)
                        {
                            rejectedByAge++;
                            continue; // Rechazar estructura por edad excesiva
                        }
                        
                        // SL simulado para BUY: swing.Low - buffer*ATR
                        double simSL = s.Low - (_config.SL_BufferATR * atr);
                        double slDistAtr = Math.Abs(entry - simSL) / atr;
                        candidates.Add(Tuple.Create(s, tf, slDistAtr));
                    }
                }
            }
            
            if (rejectedByAge > 0)
            {
                _logger.Info(string.Format("[DIAGNOSTICO][Risk] SL_AGE_FILTER: Zone={0} RejectedByAge={1}", zone.Id, rejectedByAge));
            }
            if (candidates.Count == 0)
            {
                _logger.Warning(string.Format("[RiskCalculator] ⚠ No candidates Swing Low protector para zona {0:F2}-{1:F2}", zone.Low, zone.High));
                return null;
            }

            // LOG DETALLADO: Todos los candidatos SL para análisis post-mortem
            _logger.Info(string.Format("[DIAGNOSTICO][Risk] SL_CANDIDATES: Zone={0} Dir=BUY TotalCandidates={1}", zone.Id, candidates.Count));
            int idx = 0;
            foreach (var c in candidates.OrderBy(x => x.Item3))
            {
                idx++;
                // CRÍTICO: Calcular edad en barras del TF de la estructura, no del gráfico
                int currentBarInStructureTF = barData.GetCurrentBarIndex(c.Item2);
                int age = currentBarInStructureTF - c.Item1.CreatedAtBarIndex;
                bool isInBand = (c.Item3 >= 10.0 && c.Item3 <= 15.0);
                _logger.Info(string.Format(
                    "[DIAGNOSTICO][Risk] SL_CANDIDATE: Idx={0} Type=Swing Score={1:F2} TF={2} DistATR={3:F2} Age={4} Price={5:F2} InBand={6}",
                    idx, c.Item1.Score, c.Item2, c.Item3, age, c.Item1.Low, isInBand));
            }
            // Banda objetivo [10,15], target 12.5 (actualizado en V5.6.9g)
            const double bandMin = 10.0, bandMax = 15.0, target = 12.5;
            var inBand = candidates.Where(c => c.Item3 >= bandMin && c.Item3 <= bandMax)
                                   .OrderBy(c => Math.Abs(c.Item3 - target))
                                   .ThenByDescending(c => c.Item3) // preferir más cercano a bandMax si empata
                                   .ToList();
            if (inBand.Count > 0)
            {
                var best = inBand.First();
                int currentBarInStructureTF = barData.GetCurrentBarIndex(best.Item2);
                int ageSelected = currentBarInStructureTF - best.Item1.CreatedAtBarIndex;
                _logger.Info(string.Format("[DIAGNOSTICO][Risk] SLPick BUY: Zone={0} SwingTF={1} SLDistATR={2:F2} Target={3:F1} Banda=[{4:F0},{5:F0}]",
                    zone.Id, best.Item2, best.Item3, target, bandMin, bandMax));
                _logger.Info(string.Format("[DIAGNOSTICO][Risk] SL_SELECTED: Zone={0} Type=Swing Score={1:F2} TF={2} DistATR={3:F2} Age={4} Price={5:F2} Reason=InBand[{6:F0},{7:F0}]",
                    zone.Id, best.Item1.Score, best.Item2, best.Item3, ageSelected, best.Item1.Low, bandMin, bandMax));
                return Tuple.Create(best.Item1, best.Item2);
            }
            // Fallback: mejor <15 (más cercano a 15)
            var belowMax = candidates.Where(c => c.Item3 < bandMax).OrderByDescending(c => c.Item3).FirstOrDefault();
            if (belowMax != null)
            {
                int currentBarInStructureTF = barData.GetCurrentBarIndex(belowMax.Item2);
                int ageSelected = currentBarInStructureTF - belowMax.Item1.CreatedAtBarIndex;
                _logger.Info(string.Format("[DIAGNOSTICO][Risk] SLPick BUY Fallback<15: Zone={0} SwingTF={1} SLDistATR={2:F2}", zone.Id, belowMax.Item2, belowMax.Item3));
                _logger.Info(string.Format("[DIAGNOSTICO][Risk] SL_SELECTED: Zone={0} Type=Swing Score={1:F2} TF={2} DistATR={3:F2} Age={4} Price={5:F2} Reason=Fallback<15",
                    zone.Id, belowMax.Item1.Score, belowMax.Item2, belowMax.Item3, ageSelected, belowMax.Item1.Low));
                return Tuple.Create(belowMax.Item1, belowMax.Item2);
            }
            _logger.Warning(string.Format("[DIAGNOSTICO][Risk] REJECT BUY: Todos los swings dan SL>15 ATR (zona {0})", zone.Id));
            return null;
        }

        /// <summary>
        /// Busca el Swing High más cercano ENCIMA de la HeatZone (protector para SELL)
        /// Busca en TFs altos (4H, 1D) para mayor robustez
        /// </summary>
        private Tuple<SwingInfo,int> FindProtectiveSwingHighBanded(HeatZone zone, CoreEngine coreEngine, IBarDataProvider barData, double atr, double entry, bool prioritizeTf)
        {
            IEnumerable<int> tfs = prioritizeTf
                ? _config.TimeframesToUse.OrderBy(tf => tf < 60 ? 1 : 0).ThenBy(tf => tf)
                : _config.TimeframesToUse.AsEnumerable();
            var candidates = new List<Tuple<SwingInfo,int,double>>(); // (swing, tf, slDistAtr)
            int rejectedByAge = 0;
            foreach (var tf in tfs)
            {
                var allStructures = coreEngine.GetAllStructures(tf);
                foreach (var s in allStructures.OfType<SwingInfo>())
                {
                    if (s.IsActive && s.IsHigh && s.High > zone.High)
                    {
                        // V5.7c: FILTRO DE EDAD - Rechazar estructuras obsoletas
                        int currentBarInStructureTF = barData.GetCurrentBarIndex(s.TF);
                        int age = currentBarInStructureTF - s.CreatedAtBarIndex;
                        int maxAge = _config.MaxAgeForSL_ByTF.ContainsKey(s.TF) ? _config.MaxAgeForSL_ByTF[s.TF] : 100;
                        
                        if (age > maxAge)
                        {
                            rejectedByAge++;
                            continue; // Rechazar estructura por edad excesiva
                        }
                        
                        // SL simulado para SELL: swing.High + buffer*ATR
                        double simSL = s.High + (_config.SL_BufferATR * atr);
                        double slDistAtr = Math.Abs(simSL - entry) / atr;
                        candidates.Add(Tuple.Create(s, tf, slDistAtr));
                    }
                }
            }
            
            if (rejectedByAge > 0)
            {
                _logger.Info(string.Format("[DIAGNOSTICO][Risk] SL_AGE_FILTER: Zone={0} RejectedByAge={1}", zone.Id, rejectedByAge));
            }
            if (candidates.Count == 0)
            {
                _logger.Warning(string.Format("[RiskCalculator] ⚠ No candidates Swing High protector para zona {0:F2}-{1:F2}", zone.Low, zone.High));
                return null;
            }

            // LOG DETALLADO: Todos los candidatos SL para análisis post-mortem
            _logger.Info(string.Format("[DIAGNOSTICO][Risk] SL_CANDIDATES: Zone={0} Dir=SELL TotalCandidates={1}", zone.Id, candidates.Count));
            int idx = 0;
            foreach (var c in candidates.OrderBy(x => x.Item3))
            {
                idx++;
                // CRÍTICO: Calcular edad en barras del TF de la estructura, no del gráfico
                int currentBarInStructureTF = barData.GetCurrentBarIndex(c.Item2);
                int age = currentBarInStructureTF - c.Item1.CreatedAtBarIndex;
                bool isInBand = (c.Item3 >= 10.0 && c.Item3 <= 15.0);
                _logger.Info(string.Format(
                    "[DIAGNOSTICO][Risk] SL_CANDIDATE: Idx={0} Type=Swing Score={1:F2} TF={2} DistATR={3:F2} Age={4} Price={5:F2} InBand={6}",
                    idx, c.Item1.Score, c.Item2, c.Item3, age, c.Item1.High, isInBand));
            }

            const double bandMin = 10.0, bandMax = 15.0, target = 12.5;
            var inBand = candidates.Where(c => c.Item3 >= bandMin && c.Item3 <= bandMax)
                                   .OrderBy(c => Math.Abs(c.Item3 - target))
                                   .ThenByDescending(c => c.Item3)
                                   .ToList();
            if (inBand.Count > 0)
            {
                var best = inBand.First();
                // CRÍTICO: Calcular edad en barras del TF de la estructura, no del gráfico
                int currentBarInStructureTF = barData.GetCurrentBarIndex(best.Item2);
                int ageSelected = currentBarInStructureTF - best.Item1.CreatedAtBarIndex;
                _logger.Info(string.Format("[DIAGNOSTICO][Risk] SLPick SELL: Zone={0} SwingTF={1} SLDistATR={2:F2} Target={3:F1} Banda=[{4:F0},{5:F0}]",
                    zone.Id, best.Item2, best.Item3, target, bandMin, bandMax));
                _logger.Info(string.Format("[DIAGNOSTICO][Risk] SL_SELECTED: Zone={0} Type=Swing Score={1:F2} TF={2} DistATR={3:F2} Age={4} Price={5:F2} Reason=InBand[{6:F0},{7:F0}]",
                    zone.Id, best.Item1.Score, best.Item2, best.Item3, ageSelected, best.Item1.High, bandMin, bandMax));
                return Tuple.Create(best.Item1, best.Item2);
            }
            var belowMax = candidates.Where(c => c.Item3 < bandMax).OrderByDescending(c => c.Item3).FirstOrDefault();
            if (belowMax != null)
            {
                // CRÍTICO: Calcular edad en barras del TF de la estructura, no del gráfico
                int currentBarInStructureTF = barData.GetCurrentBarIndex(belowMax.Item2);
                int ageSelected = currentBarInStructureTF - belowMax.Item1.CreatedAtBarIndex;
                _logger.Info(string.Format("[DIAGNOSTICO][Risk] SLPick SELL Fallback<15: Zone={0} SwingTF={1} SLDistATR={2:F2}", zone.Id, belowMax.Item2, belowMax.Item3));
                _logger.Info(string.Format("[DIAGNOSTICO][Risk] SL_SELECTED: Zone={0} Type=Swing Score={1:F2} TF={2} DistATR={3:F2} Age={4} Price={5:F2} Reason=Fallback<15",
                    zone.Id, belowMax.Item1.Score, belowMax.Item2, belowMax.Item3, ageSelected, belowMax.Item1.High));
                return Tuple.Create(belowMax.Item1, belowMax.Item2);
            }
            _logger.Warning(string.Format("[DIAGNOSTICO][Risk] REJECT SELL: Todos los swings dan SL>15 ATR (zona {0})", zone.Id));
            return null;
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
                    int currentBarInStructureTF = barData.GetCurrentBarIndex(liquidity.TF);
                    int age = currentBarInStructureTF - liquidity.CreatedAtBarIndex;
                    int maxAge = _config.MaxAgeForTP_ByTF.ContainsKey(liquidity.TF) ? _config.MaxAgeForTP_ByTF[liquidity.TF] : 100;
                    
                    if (age <= maxAge)
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
                    int currentBarInStructureTF = barData.GetCurrentBarIndex(liquidity.TF);
                    int age = currentBarInStructureTF - liquidity.CreatedAtBarIndex;
                    int maxAge = _config.MaxAgeForTP_ByTF.ContainsKey(liquidity.TF) ? _config.MaxAgeForTP_ByTF[liquidity.TF] : 100;
                    
                    if (age <= maxAge)
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
                    int currentBarInStructureTF = barData.GetCurrentBarIndex(structure.TF);
                    int age = currentBarInStructureTF - structure.CreatedAtBarIndex;
                    int maxAge = _config.MaxAgeForTP_ByTF.ContainsKey(structure.TF) ? _config.MaxAgeForTP_ByTF[structure.TF] : 100;
                    
                    if (age <= maxAge)
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
                    int currentBarInStructureTF = barData.GetCurrentBarIndex(structure.TF);
                    int age = currentBarInStructureTF - structure.CreatedAtBarIndex;
                    int maxAge = _config.MaxAgeForTP_ByTF.ContainsKey(structure.TF) ? _config.MaxAgeForTP_ByTF[structure.TF] : 100;
                    
                    if (age <= maxAge)
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
                    int currentBarInStructureTF = barData.GetCurrentBarIndex(swing.TF);
                    int age = currentBarInStructureTF - swing.CreatedAtBarIndex;
                    int maxAge = _config.MaxAgeForTP_ByTF.ContainsKey(swing.TF) ? _config.MaxAgeForTP_ByTF[swing.TF] : 100;
                    
                    if (age <= maxAge)
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
                    int currentBarInStructureTF = barData.GetCurrentBarIndex(swing.TF);
                    int age = currentBarInStructureTF - swing.CreatedAtBarIndex;
                    int maxAge = _config.MaxAgeForTP_ByTF.ContainsKey(swing.TF) ? _config.MaxAgeForTP_ByTF[swing.TF] : 100;
                    
                    if (age <= maxAge)
                        allSwings.Add(swing);
                }
            }

            // Priorizar por PROXIMIDAD (el más cercano por precio)
            return allSwings
                .OrderByDescending(s => s.Low) // Más cercano por precio (el más alto de los que están debajo)
                .FirstOrDefault();
        }
    }
}
