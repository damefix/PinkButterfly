// ============================================================================
// OutputAdapter.cs
// PinkButterfly CoreBrain - Componente 6 del DFM (ÚLTIMO)
// 
// Responsabilidades:
// - Tomar la mejor zona del snapshot (BestZone, BestConfidence)
// - Generar Action basado en thresholds:
//   * Confidence >= MinConfidenceForEntry → BUY/SELL
//   * Confidence < MinConfidenceForEntry → WAIT
// - Crear el TradeDecision final con Entry, SL, TP, PositionSize
// - Generar Rationale legible explicando la decisión (con explainability)
//
// Formato de Rationale:
//   "BUY Limit @ 4500.00 (HeatZone HZ_abc123: Bullish, 3 structures, Score: 0.72)
//   - Confidence: 0.68 (Core: 0.28, Proximity: 0.18, Confluence: 0.09, Type: 0.08, Bias: 0.05)
//   - Dominant Factor: FVG 4H (Score: 0.92)
//   - SL: 4485.00 (-15.00), TP: 4522.50 (+22.50), R:R: 1.50
//   - Position: 2 contracts (Risk: $750 / 0.5% of account)
//   - Aligned with Bullish bias (Strength: 0.75)"
// ============================================================================

using System;
using System.Text;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// OutputAdapter: Sexto y último componente del pipeline DFM
    /// Genera el TradeDecision final con Action, Entry, SL, TP y Rationale explicativo
    /// </summary>
    public class OutputAdapter : IDecisionComponent
    {
        private EngineConfig _config;
        private ILogger _logger;

        public string ComponentName => "OutputAdapter";

        public void Initialize(EngineConfig config, ILogger logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug("[OutputAdapter] Inicializado");
        }

        public void Process(DecisionSnapshot snapshot, IBarDataProvider barData, CoreEngine coreEngine, int currentBar, double accountSize)
        {
            if (snapshot == null)
                throw new ArgumentNullException(nameof(snapshot));

            _logger.Debug("[OutputAdapter] Generando TradeDecision final...");

            // Obtener la mejor zona del snapshot
            HeatZone bestZone = snapshot.Metadata.ContainsKey("BestZone")
                ? snapshot.Metadata["BestZone"] as HeatZone
                : null;

            double bestConfidence = snapshot.Metadata.ContainsKey("BestConfidence")
                ? (double)snapshot.Metadata["BestConfidence"]
                : 0.0;

            DecisionScoreBreakdown breakdown = snapshot.Metadata.ContainsKey("BestBreakdown")
                ? snapshot.Metadata["BestBreakdown"] as DecisionScoreBreakdown
                : null;

            // Crear TradeDecision
            TradeDecision decision;

            if (bestZone == null || bestConfidence < _config.MinConfidenceForEntry)
            {
                // No hay zona válida o confidence insuficiente → WAIT
                decision = CreateWaitDecision(snapshot, bestZone, bestConfidence, breakdown);
            }
            else
            {
                // Zona válida con confidence suficiente → BUY/SELL
                decision = CreateTradeDecision(bestZone, bestConfidence, breakdown, snapshot);
            }

            // Almacenar decisión en snapshot
            snapshot.Metadata["FinalDecision"] = decision;

            _logger.Info(string.Format(
                "[OutputAdapter] Decisión generada: {0} @ {1:F2}, Confidence: {2:F3}",
                decision.Action, decision.Entry, decision.Confidence
            ));
        }

        /// <summary>
        /// Crea una decisión WAIT cuando no hay zona válida o confidence insuficiente
        /// </summary>
        private TradeDecision CreateWaitDecision(DecisionSnapshot snapshot, HeatZone bestZone, double bestConfidence, DecisionScoreBreakdown breakdown)
        {
            var rationale = new StringBuilder();
            rationale.AppendLine("WAIT - No hay oportunidad de trading clara");

            if (bestZone == null)
            {
                rationale.AppendLine("- No se encontraron HeatZones válidas");
            }
            else
            {
                rationale.AppendFormat("- Mejor zona: {0} ({1}), Confidence: {2:F3} (< {3:F2} requerido)\n",
                    bestZone.Id, bestZone.Direction, bestConfidence, _config.MinConfidenceForEntry);
            }

            rationale.AppendFormat("- Market Clarity: {0:F2}, Volatility: {1:F2}\n",
                snapshot.MarketClarity, snapshot.MarketVolatilityNormalized);
            rationale.AppendFormat("- Global Bias: {0} (Strength: {1:F2})",
                snapshot.GlobalBias, snapshot.GlobalBiasStrength);

            return new TradeDecision
            {
                Id = Guid.NewGuid().ToString(),
                Action = "WAIT",
                Confidence = bestConfidence,
                Entry = 0.0,
                StopLoss = 0.0,
                TakeProfit = 0.0,
                PositionSizeContracts = 0.0,
                Rationale = rationale.ToString(),
                Explainability = breakdown ?? new DecisionScoreBreakdown(),
                SourceStructureIds = bestZone?.SourceStructureIds ?? new System.Collections.Generic.List<string>(),
                GeneratedAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Crea una decisión BUY/SELL basada en la mejor zona
        /// </summary>
        private TradeDecision CreateTradeDecision(HeatZone zone, double confidence, DecisionScoreBreakdown breakdown, DecisionSnapshot snapshot)
        {
            // Obtener datos de riesgo de la zona
            double entry = (double)zone.Metadata["Entry"];
            double stopLoss = (double)zone.Metadata["StopLoss"];
            double takeProfit = (double)zone.Metadata["TakeProfit"];
            int positionSize = (int)zone.Metadata["PositionSizeContracts"];
            double actualRR = (double)zone.Metadata["ActualRR"];
            double accountRisk = (double)zone.Metadata["AccountRisk"];

            // Determinar Action
            string action = zone.Direction == "Bullish" ? "BUY" : "SELL";

            // Generar Rationale
            string rationale = GenerateRationale(action, entry, stopLoss, takeProfit, positionSize, actualRR,
                accountRisk, zone, confidence, breakdown, snapshot);

            return new TradeDecision
            {
                Id = Guid.NewGuid().ToString(),
                Action = action,
                Confidence = confidence,
                Entry = entry,
                StopLoss = stopLoss,
                TakeProfit = takeProfit,
                PositionSizeContracts = positionSize,
                Rationale = rationale,
                Explainability = breakdown,
                SourceStructureIds = zone.SourceStructureIds,
                GeneratedAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Genera el Rationale explicativo de la decisión
        /// Formato profesional con explainability completa
        /// </summary>
        private string GenerateRationale(string action, double entry, double stopLoss, double takeProfit, int positionSize,
            double actualRR, double accountRisk, HeatZone zone, double confidence, DecisionScoreBreakdown breakdown, DecisionSnapshot snapshot)
        {
            var sb = new StringBuilder();

            // Línea 1: Acción principal
            sb.AppendFormat("{0} Limit @ {1:F2} (HeatZone {2}: {3}, {4} structures, Score: {5:F2})\n",
                action, entry, zone.Id, zone.Direction, zone.ConfluenceCount, zone.Score);

            // Línea 2: Confidence con desglose de contribuciones
            sb.AppendFormat("- Confidence: {0:F3} (Core: {1:F3}, Prox: {2:F3}, Conf: {3:F3}, Type: {4:F3}, Bias: {5:F3}, Mom: {6:F3}, Vol: {7:F3})\n",
                confidence,
                breakdown.CoreScoreContribution,
                breakdown.ProximityContribution,
                breakdown.ConfluenceContribution,
                breakdown.TypeContribution,
                breakdown.BiasContribution,
                breakdown.MomentumContribution,
                breakdown.VolumeContribution);

            // Línea 3: Factor Dominante (estructura con mayor contribución al score)
            string dominantFactor = GetDominantFactor(breakdown);
            sb.AppendFormat("- Dominant Factor: {0} {1} (Score: {2:F2})\n",
                zone.DominantType, GetTimeframeLabel(zone.TFDominante), zone.Score);

            // Línea 4: Niveles de riesgo (SL, TP, R:R)
            double slDistance = Math.Abs(entry - stopLoss);
            double tpDistance = Math.Abs(takeProfit - entry);
            sb.AppendFormat("- SL: {0:F2} ({1}{2:F2}), TP: {3:F2} (+{4:F2}), R:R: {5:F2}\n",
                stopLoss, action == "BUY" ? "-" : "+", slDistance,
                takeProfit, tpDistance, actualRR);

            // Línea 5: Position size y riesgo
            sb.AppendFormat("- Position: {0} contract{1} (Risk: ${2:F0} / {3:F2}% of account)\n",
                positionSize, positionSize > 1 ? "s" : "", accountRisk, _config.RiskPercentPerTrade);

            // Línea 6: Alineamiento con bias global
            string biasAlignment = zone.Direction == snapshot.GlobalBias
                ? "Aligned with"
                : "Against";
            sb.AppendFormat("- {0} {1} bias (Strength: {2:F2})",
                biasAlignment, snapshot.GlobalBias, snapshot.GlobalBiasStrength);

            return sb.ToString();
        }

        /// <summary>
        /// Identifica el factor dominante (contribución más alta)
        /// </summary>
        private string GetDominantFactor(DecisionScoreBreakdown breakdown)
        {
            double maxContribution = 0.0;
            string dominantFactor = "Unknown";

            if (breakdown.CoreScoreContribution > maxContribution)
            {
                maxContribution = breakdown.CoreScoreContribution;
                dominantFactor = "Core Score";
            }
            if (breakdown.ProximityContribution > maxContribution)
            {
                maxContribution = breakdown.ProximityContribution;
                dominantFactor = "Proximity";
            }
            if (breakdown.ConfluenceContribution > maxContribution)
            {
                maxContribution = breakdown.ConfluenceContribution;
                dominantFactor = "Confluence";
            }
            if (breakdown.TypeContribution > maxContribution)
            {
                maxContribution = breakdown.TypeContribution;
                dominantFactor = "Structure Type";
            }
            if (breakdown.BiasContribution > maxContribution)
            {
                maxContribution = breakdown.BiasContribution;
                dominantFactor = "Bias Alignment";
            }
            if (breakdown.MomentumContribution > maxContribution)
            {
                maxContribution = breakdown.MomentumContribution;
                dominantFactor = "Momentum";
            }
            if (breakdown.VolumeContribution > maxContribution)
            {
                dominantFactor = "Volume";
            }

            return dominantFactor;
        }

        /// <summary>
        /// Convierte minutos de timeframe a etiqueta legible
        /// </summary>
        private string GetTimeframeLabel(int minutes)
        {
            if (minutes >= 1440)
                return (minutes / 1440) + "D";
            if (minutes >= 60)
                return (minutes / 60) + "H";
            return minutes + "m";
        }
    }
}

