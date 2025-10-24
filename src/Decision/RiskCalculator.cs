// ============================================================================
// RiskCalculator.cs
// PinkButterfly CoreBrain - Componente 4 del DFM
// 
// Responsabilidades:
// - Para cada HeatZone relevante, calcular niveles de SL y TP estructurales
// - SL estructural: Basado en el borde de la zona + buffer ATR
// - TP: Basado en Risk:Reward ratio mínimo
// - Calcular PositionSizeContracts basado en RiskPercentPerTrade y distancia al SL
// - Validar que el R:R sea >= MinRiskRewardRatio
//
// Lógica de Entry/SL (Limit Order estructural para maximizar R:R):
//   BUY:  Entry = zone.Low, SL = zone.Low - (SL_BufferATR * ATR), TP = Entry + (Entry - SL) * R:R
//   SELL: Entry = zone.High, SL = zone.High + (SL_BufferATR * ATR), TP = Entry - (SL - Entry) * R:R
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
    /// Calcula niveles de SL, TP y position size para cada HeatZone
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
            _logger.Debug("[RiskCalculator] Inicializado");
        }

        public void Process(DecisionSnapshot snapshot, IBarDataProvider barData, CoreEngine coreEngine, int currentBar, double accountSize)
        {
            if (snapshot == null)
                throw new ArgumentNullException(nameof(snapshot));
            if (barData == null)
                throw new ArgumentNullException(nameof(barData));
            if (accountSize <= 0)
            {
                _logger.Warning("[RiskCalculator] AccountSize <= 0, no se puede calcular position size");
                return;
            }

            _logger.Debug("[RiskCalculator] Calculando riesgo para HeatZones...");

            if (snapshot.HeatZones == null || snapshot.HeatZones.Count == 0)
            {
                _logger.Debug("[RiskCalculator] No hay HeatZones para calcular riesgo");
                return;
            }

            // Procesar cada HeatZone
            foreach (var zone in snapshot.HeatZones)
            {
                CalculateRiskLevels(zone, barData, currentBar, accountSize);
            }

            _logger.Debug(string.Format("[RiskCalculator] Riesgo calculado para {0} HeatZones", snapshot.HeatZones.Count));
        }

        /// <summary>
        /// Calcula Entry, SL, TP y PositionSize para una HeatZone
        /// Añade los resultados a zone.Metadata
        /// </summary>
        private void CalculateRiskLevels(HeatZone zone, IBarDataProvider barData, int currentBar, double accountSize)
        {
            // Obtener ATR del TF Dominante
            double atr = barData.GetATR(zone.TFDominante, currentBar, 14);
            if (atr <= 0)
            {
                _logger.Warning(string.Format("[RiskCalculator] ATR({0}) es 0 para HeatZone {1}, usando ATR=1.0",
                    zone.TFDominante, zone.Id));
                atr = 1.0;
            }

            double entry, stopLoss, takeProfit;

            // Calcular Entry, SL, TP según la dirección de la zona
            if (zone.Direction == "Bullish")
            {
                // BUY Limit Order: Entry en el borde inferior de la zona
                entry = zone.Low;
                stopLoss = zone.Low - (_config.SL_BufferATR * atr);
                
                // TP basado en R:R mínimo
                double riskDistance = entry - stopLoss;
                takeProfit = entry + (riskDistance * _config.MinRiskRewardRatio);
            }
            else if (zone.Direction == "Bearish")
            {
                // SELL Limit Order: Entry en el borde superior de la zona
                entry = zone.High;
                stopLoss = zone.High + (_config.SL_BufferATR * atr);
                
                // TP basado en R:R mínimo
                double riskDistance = stopLoss - entry;
                takeProfit = entry - (riskDistance * _config.MinRiskRewardRatio);
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
            double riskDistance2 = Math.Abs(entry - stopLoss);
            double actualRR = riskDistance2 > 0 ? rewardDistance / riskDistance2 : 0.0;

            // Añadir a Metadata
            zone.Metadata["Entry"] = entry;
            zone.Metadata["StopLoss"] = stopLoss;
            zone.Metadata["TakeProfit"] = takeProfit;
            zone.Metadata["PositionSizeContracts"] = positionSizeContracts;
            zone.Metadata["RiskPerContract"] = riskPerContract;
            zone.Metadata["AccountRisk"] = accountRisk;
            zone.Metadata["ActualRR"] = actualRR;
            zone.Metadata["RiskCalculated"] = true;

            _logger.Debug(string.Format(
                "[RiskCalculator] HeatZone {0} ({1}): Entry={2:F2}, SL={3:F2}, TP={4:F2}, Size={5}, R:R={6:F2}",
                zone.Id, zone.Direction, entry, stopLoss, takeProfit, positionSizeContracts, actualRR
            ));
        }
    }
}

