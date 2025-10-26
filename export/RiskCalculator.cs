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
            foreach (var zone in snapshot.HeatZones)
            {
                CalculateStructuralRiskLevels(zone, barData, coreEngine, currentBar, accountSize);
            }

            _logger.Debug(string.Format("[RiskCalculator] Riesgo calculado para {0} HeatZones", snapshot.HeatZones.Count));
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

            // Calcular Entry, SL, TP según la dirección de la zona
            if (zone.Direction == "Bullish")
            {
                // BUY Limit Order: Entry en el borde inferior de la zona
                entry = zone.Low;
                
                // SL ESTRUCTURAL: Buscar Swing Low protector
                stopLoss = CalculateStructuralSL_Buy(zone, coreEngine, barData, currentBar, atr);
                
                // TP JERÁRQUICO: Buscar objetivo estructural
                takeProfit = CalculateStructuralTP_Buy(zone, coreEngine, barData, currentBar, entry, stopLoss);
            }
            else if (zone.Direction == "Bearish")
            {
                // SELL Limit Order: Entry en el borde superior de la zona
                entry = zone.High;
                
                // SL ESTRUCTURAL: Buscar Swing High protector
                stopLoss = CalculateStructuralSL_Sell(zone, coreEngine, barData, currentBar, atr);
                
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
            double tickSize = barData.GetTickSize();
            
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

            // ========================================================================
            // VALIDACIONES DE RISK:REWARD (OPTIMIZACIÓN CRÍTICA)
            // ========================================================================
            
            // 1. Validar SL máximo (en ATR)
            double slDistanceATR = riskDistance / atr;
            if (slDistanceATR > _config.MaxSLDistanceATR)
            {
                _logger.Warning(string.Format(
                    "[RiskCalculator] ⚠ HeatZone {0} RECHAZADA: SL demasiado lejano ({1:F2} ATR > límite {2:F2} ATR)",
                    zone.Id, slDistanceATR, _config.MaxSLDistanceATR));
                
                zone.Metadata["RiskCalculated"] = false;
                zone.Metadata["RejectReason"] = string.Format("SL absurdo: {0:F2} ATR (límite: {1:F2})", slDistanceATR, _config.MaxSLDistanceATR);
                return;
            }
            
            // 2. Validar TP mínimo (en ATR)
            double tpDistanceATR = rewardDistance / atr;
            if (tpDistanceATR < _config.MinTPDistanceATR)
            {
                _logger.Warning(string.Format(
                    "[RiskCalculator] ⚠ HeatZone {0} RECHAZADA: TP demasiado cercano ({1:F2} ATR < mínimo {2:F2} ATR)",
                    zone.Id, tpDistanceATR, _config.MinTPDistanceATR));
                
                zone.Metadata["RiskCalculated"] = false;
                zone.Metadata["RejectReason"] = string.Format("TP insuficiente: {0:F2} ATR (mínimo: {1:F2})", tpDistanceATR, _config.MinTPDistanceATR);
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
            var swingLow = FindProtectiveSwingLow(zone, coreEngine);

            if (swingLow != null)
            {
                // SL = Swing Low - (ATR * buffer)
                double structuralSL = swingLow.Low - (_config.SL_BufferATR * atr);
                
                // Aplicar filtro de mínimo
                double finalSL = Math.Min(structuralSL, minSL);
                
                _logger.Info(string.Format("[RiskCalculator] ✓ SL estructural (BUY): SwingLow={0:F2}, StructuralSL={1:F2}, FinalSL={2:F2}, MinSL={3:F2}",
                    swingLow.Low, structuralSL, finalSL, minSL));
                
                return finalSL;
            }
            else
            {
                // No se encontró Swing protector, usar SL mínimo
                _logger.Warning(string.Format("[RiskCalculator] ⚠ No se encontró Swing Low protector para HeatZone {0} (TF={1}), usando SL mínimo: {2:F2}",
                    zone.Id, zone.TFDominante, minSL));
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
            var swingHigh = FindProtectiveSwingHigh(zone, coreEngine);

            if (swingHigh != null)
            {
                // SL = Swing High + (ATR * buffer)
                double structuralSL = swingHigh.High + (_config.SL_BufferATR * atr);
                
                // Aplicar filtro de mínimo
                double finalSL = Math.Max(structuralSL, minSL);
                
                _logger.Info(string.Format("[RiskCalculator] ✓ SL estructural (SELL): SwingHigh={0:F2}, StructuralSL={1:F2}, FinalSL={2:F2}, MinSL={3:F2}",
                    swingHigh.High, structuralSL, finalSL, minSL));
                
                return finalSL;
            }
            else
            {
                // No se encontró Swing protector, usar SL mínimo
                _logger.Warning(string.Format("[RiskCalculator] ⚠ No se encontró Swing High protector para HeatZone {0} (TF={1}), usando SL mínimo: {2:F2}",
                    zone.Id, zone.TFDominante, minSL));
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

            // 1. PRIORIDAD 1: Buscar Liquidity Grab/Void opuesto (arriba)
            _logger.Debug("[RiskCalculator] [P1] Buscando Liquidity Grab/Void encima de la zona...");
            var liquidityTarget = FindLiquidityTarget_Above(zone, coreEngine);
            if (liquidityTarget != null)
            {
                double tp = liquidityTarget.Low; // Target en el borde inferior del LG/LV
                if (tp > entry)
                {
                    _logger.Info(string.Format("[RiskCalculator] ✓ [P1] TP SELECCIONADO: Liquidity ({0}) @ {1:F2}, R:R={2:F2}",
                        liquidityTarget.Type, tp, (tp - entry) / riskDistance));
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
            var structureTarget = FindOpposingStructure_Above(zone, coreEngine, 0.7);
            if (structureTarget != null)
            {
                double tp = structureTarget.Low; // Target en el borde inferior
                if (tp > entry)
                {
                    _logger.Info(string.Format("[RiskCalculator] ✓ [P2] TP SELECCIONADO: Structure ({0}, Score={1:F2}) @ {2:F2}, R:R={3:F2}",
                        structureTarget.Type, structureTarget.Score, tp, (tp - entry) / riskDistance));
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
            var swingTarget = FindSwingHigh_Above(zone, coreEngine);
            if (swingTarget != null)
            {
                double tp = swingTarget.High;
                double potentialRR = (tp - entry) / riskDistance;
                
                // FILTRO CRÍTICO: Rechazar TPs con R:R absurdo (> 10)
                // Si R:R es razonable (< 10), aceptar el TP sin importar la distancia
                // Solo aplicar filtro de distancia si R:R es muy alto
                double atr = barData.GetATR(zone.TFDominante, currentBar, 14);
                double maxTPDistance = 50.0 * atr; // Máximo 50 × ATR de distancia (más realista para ES)
                double tpDistance = tp - entry;
                
                // Aceptar si: TP válido Y (R:R razonable O distancia aceptable)
                bool validRR = potentialRR <= 10.0;
                bool validDistance = tpDistance <= maxTPDistance;
                
                if (tp > entry && (validRR || validDistance))
                {
                    string reason = validRR && validDistance ? "R:R y Distancia OK" 
                        : validRR ? "R:R OK (Distancia ignorada)" 
                        : "Distancia OK (R:R ignorado)";
                    _logger.Info(string.Format("[RiskCalculator] ✓ [P3] TP SELECCIONADO: Swing High @ {0:F2}, R:R={1:F2} ({2})",
                        tp, potentialRR, reason));
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

            // 1. PRIORIDAD 1: Buscar Liquidity Grab/Void opuesto (abajo)
            _logger.Debug("[RiskCalculator] [P1] Buscando Liquidity Grab/Void debajo de la zona...");
            var liquidityTarget = FindLiquidityTarget_Below(zone, coreEngine);
            if (liquidityTarget != null)
            {
                double tp = liquidityTarget.High; // Target en el borde superior del LG/LV
                if (tp < entry)
                {
                    _logger.Info(string.Format("[RiskCalculator] ✓ [P1] TP SELECCIONADO: Liquidity ({0}) @ {1:F2}, R:R={2:F2}",
                        liquidityTarget.Type, tp, (entry - tp) / riskDistance));
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
            var structureTarget = FindOpposingStructure_Below(zone, coreEngine, 0.7);
            if (structureTarget != null)
            {
                double tp = structureTarget.High; // Target en el borde superior
                if (tp < entry)
                {
                    _logger.Info(string.Format("[RiskCalculator] ✓ [P2] TP SELECCIONADO: Structure ({0}, Score={1:F2}) @ {2:F2}, R:R={3:F2}",
                        structureTarget.Type, structureTarget.Score, tp, (entry - tp) / riskDistance));
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
            var swingTarget = FindSwingLow_Below(zone, coreEngine);
            if (swingTarget != null)
            {
                double tp = swingTarget.Low;
                double potentialRR = (entry - tp) / riskDistance;
                
                // FILTRO CRÍTICO: Rechazar TPs con R:R absurdo (> 10)
                // Si R:R es razonable (< 10), aceptar el TP sin importar la distancia
                // Solo aplicar filtro de distancia si R:R es muy alto
                double atr = barData.GetATR(zone.TFDominante, currentBar, 14);
                double maxTPDistance = 50.0 * atr; // Máximo 50 × ATR de distancia (más realista para ES)
                double tpDistance = entry - tp;
                
                // Aceptar si: TP válido Y (R:R razonable O distancia aceptable)
                bool validRR = potentialRR <= 10.0;
                bool validDistance = tpDistance <= maxTPDistance;
                
                if (tp < entry && (validRR || validDistance))
                {
                    string reason = validRR && validDistance ? "R:R y Distancia OK" 
                        : validRR ? "R:R OK (Distancia ignorada)" 
                        : "Distancia OK (R:R ignorado)";
                    _logger.Info(string.Format("[RiskCalculator] ✓ [P3] TP SELECCIONADO: Swing Low @ {0:F2}, R:R={1:F2} ({2})",
                        tp, potentialRR, reason));
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
            return fallbackTP;
        }

        // ========================================================================
        // HELPERS: BÚSQUEDA DE ESTRUCTURAS PROTECTORAS Y OBJETIVOS
        // ========================================================================

        /// <summary>
        /// Busca el Swing Low más cercano DEBAJO de la HeatZone (protector para BUY)
        /// Busca en TFs altos (4H, 1D) para mayor robustez
        /// </summary>
        private SwingInfo FindProtectiveSwingLow(HeatZone zone, CoreEngine coreEngine)
        {
            // Buscar en TFs superiores (4H, 1D) para mayor robustez
            var higherTFs = _config.TimeframesToUse.Where(tf => tf >= 240).OrderByDescending(tf => tf);

            foreach (var tf in higherTFs)
            {
                // Obtener todas las estructuras y filtrar por tipo SwingInfo
                var allStructures = coreEngine.GetAllStructures(tf);
                
                _logger.Debug(string.Format("[RiskCalculator] Buscando Swing Low protector en TF {0}: {1} estructuras totales",
                    tf, allStructures.Count()));

                var swings = allStructures
                    .OfType<SwingInfo>() // Usar OfType para casting seguro
                    .Where(s => s.IsActive && !s.IsHigh && s.Low < zone.Low) // Swing Low activo debajo de la zona
                    .OrderByDescending(s => s.Low) // Más cercano (el más alto de los que están debajo)
                    .ToList();

                _logger.Debug(string.Format("[RiskCalculator] Encontrados {0} Swing Lows debajo de zona {1:F2}",
                    swings.Count, zone.Low));

                if (swings.Any())
                {
                    var swing = swings.First();
                    _logger.Info(string.Format("[RiskCalculator] ✓ Swing Low protector encontrado: Low={0:F2}, High={1:F2} (TF={2})",
                        swing.Low, swing.High, tf));
                    return swing;
                }
            }

            _logger.Warning(string.Format("[RiskCalculator] ⚠ No se encontró ningún Swing Low protector en TFs altos para zona {0:F2}-{1:F2}",
                zone.Low, zone.High));
            return null;
        }

        /// <summary>
        /// Busca el Swing High más cercano ENCIMA de la HeatZone (protector para SELL)
        /// Busca en TFs altos (4H, 1D) para mayor robustez
        /// </summary>
        private SwingInfo FindProtectiveSwingHigh(HeatZone zone, CoreEngine coreEngine)
        {
            // Buscar en TFs superiores (4H, 1D) para mayor robustez
            var higherTFs = _config.TimeframesToUse.Where(tf => tf >= 240).OrderByDescending(tf => tf);

            foreach (var tf in higherTFs)
            {
                // Obtener todas las estructuras y filtrar por tipo SwingInfo
                var allStructures = coreEngine.GetAllStructures(tf);
                
                _logger.Debug(string.Format("[RiskCalculator] Buscando Swing High protector en TF {0}: {1} estructuras totales",
                    tf, allStructures.Count()));

                var swings = allStructures
                    .OfType<SwingInfo>() // Usar OfType para casting seguro
                    .Where(s => s.IsActive && s.IsHigh && s.High > zone.High) // Swing High activo encima de la zona
                    .OrderBy(s => s.High) // Más cercano (el más bajo de los que están encima)
                    .ToList();

                _logger.Debug(string.Format("[RiskCalculator] Encontrados {0} Swing Highs encima de zona {1:F2}",
                    swings.Count, zone.High));

                if (swings.Any())
                {
                    var swing = swings.First();
                    _logger.Info(string.Format("[RiskCalculator] ✓ Swing High protector encontrado: Low={0:F2}, High={1:F2} (TF={2})",
                        swing.Low, swing.High, tf));
                    return swing;
                }
            }

            _logger.Warning(string.Format("[RiskCalculator] ⚠ No se encontró ningún Swing High protector en TFs altos para zona {0:F2}-{1:F2}",
                zone.Low, zone.High));
            return null;
        }

        /// <summary>
        /// Busca Liquidity Grab/Void ENCIMA de la zona (target para BUY)
        /// </summary>
        private StructureBase FindLiquidityTarget_Above(HeatZone zone, CoreEngine coreEngine)
        {
            foreach (var tf in _config.TimeframesToUse.OrderByDescending(t => t))
            {
                var liquidity = coreEngine.GetAllStructures(tf)
                    .Where(s => (s.Type == "LiquidityGrabInfo" || s.Type == "LiquidityVoidInfo") 
                             && s.IsActive 
                             && s.Low > zone.High) // Encima
                    .OrderBy(s => s.Low) // Más cercano
                    .FirstOrDefault();

                if (liquidity != null)
                    return liquidity;
            }

            return null;
        }

        /// <summary>
        /// Busca Liquidity Grab/Void DEBAJO de la zona (target para SELL)
        /// </summary>
        private StructureBase FindLiquidityTarget_Below(HeatZone zone, CoreEngine coreEngine)
        {
            foreach (var tf in _config.TimeframesToUse.OrderByDescending(t => t))
            {
                var liquidity = coreEngine.GetAllStructures(tf)
                    .Where(s => (s.Type == "LiquidityGrabInfo" || s.Type == "LiquidityVoidInfo") 
                             && s.IsActive 
                             && s.High < zone.Low) // Debajo
                    .OrderByDescending(s => s.High) // Más cercano
                    .FirstOrDefault();

                if (liquidity != null)
                    return liquidity;
            }

            return null;
        }

        /// <summary>
        /// Busca FVG/OB opuesto ENCIMA con Score > minScore (target para BUY)
        /// </summary>
        private StructureBase FindOpposingStructure_Above(HeatZone zone, CoreEngine coreEngine, double minScore)
        {
            foreach (var tf in _config.TimeframesToUse.OrderByDescending(t => t))
            {
                var structure = coreEngine.GetAllStructures(tf)
                    .Where(s => (s.Type == "FairValueGapInfo" || s.Type == "OrderBlockInfo" || s.Type == "PointOfInterestInfo") 
                             && s.IsActive 
                             && s.Score >= minScore
                             && s.Low > zone.High) // Encima
                    .OrderBy(s => s.Low) // Más cercano
                    .FirstOrDefault();

                if (structure != null)
                    return structure;
            }

            return null;
        }

        /// <summary>
        /// Busca FVG/OB opuesto DEBAJO con Score > minScore (target para SELL)
        /// </summary>
        private StructureBase FindOpposingStructure_Below(HeatZone zone, CoreEngine coreEngine, double minScore)
        {
            foreach (var tf in _config.TimeframesToUse.OrderByDescending(t => t))
            {
                var structure = coreEngine.GetAllStructures(tf)
                    .Where(s => (s.Type == "FairValueGapInfo" || s.Type == "OrderBlockInfo" || s.Type == "PointOfInterestInfo") 
                             && s.IsActive 
                             && s.Score >= minScore
                             && s.High < zone.Low) // Debajo
                    .OrderByDescending(s => s.High) // Más cercano
                    .FirstOrDefault();

                if (structure != null)
                    return structure;
            }

            return null;
        }

        /// <summary>
        /// Busca Swing High ENCIMA de la zona (target para BUY)
        /// PRIORIZA SWINGS CERCANOS de TODOS los TFs (no solo altos)
        /// </summary>
        private SwingInfo FindSwingHigh_Above(HeatZone zone, CoreEngine coreEngine)
        {
            // Buscar en TODOS los TFs y recopilar todos los Swings válidos
            var allSwings = new List<SwingInfo>();
            
            foreach (var tf in _config.TimeframesToUse)
            {
                var swings = coreEngine.GetAllStructures(tf)
                    .OfType<SwingInfo>()
                    .Where(s => s.IsActive && s.IsHigh && s.High > zone.High) // Swing High encima
                    .ToList();
                
                allSwings.AddRange(swings);
            }

            // Priorizar por PROXIMIDAD (el más cercano por precio)
            return allSwings
                .OrderBy(s => s.High) // Más cercano por precio
                .FirstOrDefault();
        }

        /// <summary>
        /// Busca Swing Low DEBAJO de la zona (target para SELL)
        /// PRIORIZA SWINGS CERCANOS de TODOS los TFs (no solo altos)
        /// </summary>
        private SwingInfo FindSwingLow_Below(HeatZone zone, CoreEngine coreEngine)
        {
            // Buscar en TODOS los TFs y recopilar todos los Swings válidos
            var allSwings = new List<SwingInfo>();
            
            foreach (var tf in _config.TimeframesToUse)
            {
                var swings = coreEngine.GetAllStructures(tf)
                    .OfType<SwingInfo>()
                    .Where(s => s.IsActive && !s.IsHigh && s.Low < zone.Low) // Swing Low debajo
                    .ToList();
                
                allSwings.AddRange(swings);
            }

            // Priorizar por PROXIMIDAD (el más cercano por precio)
            return allSwings
                .OrderByDescending(s => s.Low) // Más cercano por precio (el más alto de los que están debajo)
                .FirstOrDefault();
        }
    }
}
