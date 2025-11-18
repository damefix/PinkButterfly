// ============================================================================
// TradeManager.cs
// PinkButterfly CoreBrain - Gestor Institucional de Órdenes
// 
// Gestiona el ciclo de vida completo de órdenes Limit:
// - Estados: PENDING, EXECUTED, CANCELLED, SL_HIT, TP_HIT
// - Caducidad inteligente por BOS/CHoCH contradictorio
// - Caducidad por separación operativa (precio se aleja)
// - Tracking de ejecución y salida
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using NinjaTrader.NinjaScript.Indicators.PinkButterfly;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Estados posibles de una orden
    /// </summary>
    public enum TradeStatus
    {
        PENDING,      // Esperando que el precio llegue al Entry
        EXECUTED,     // Precio llegó al Entry, operación activa
        CANCELLED,    // Cancelada por invalidación estructural
        SL_HIT,       // Cerrada por Stop Loss
        TP_HIT        // Cerrada por Take Profit
    }

    /// <summary>
    /// Registro completo de una orden Limit
    /// </summary>
    public class TradeRecord
    {
        public string Id { get; set; }
        public int EntryBar { get; set; }           // Barra donde se generó la señal (actualizada en upgrades)
        public DateTime EntryBarTime { get; set; }  // Timestamp de la barra de entrada (actualizada en upgrades)
        public double Entry { get; set; }
        public double SL { get; set; }
        public double TP { get; set; }
        public string Action { get; set; }          // "BUY" o "SELL"
        public TradeStatus Status { get; set; }
        public int ExecutionBar { get; set; }       // Barra donde se ejecutó (si Status >= EXECUTED)
        public DateTime ExecutionBarTime { get; set; } // Timestamp de ejecución (V5.7e)
        public int ExitBar { get; set; }            // Barra donde se cerró/canceló
        public DateTime ExitBarTime { get; set; }   // Timestamp de la barra de salida
        public string ExitReason { get; set; }      // "SL", "TP", "BOS_CONTRARY", "PRICE_DEPARTED", etc.
        public int TFDominante { get; set; }        // TF dominante de la HeatZone
        public string SourceStructureId { get; set; } // ID de la estructura dominante que generó esta orden
        public double RegistrationPrice { get; set; } // Close cuando se registró la orden (para determinar LIMIT vs STOP)
        
        // V6.0n: Campos para upgrade inteligente y expiración adaptativa
        public double DistanceToEntryATR { get; set; } = -1.0;  // Distancia al Entry en ATR (actualizada en upgrades)
        public double StructureScore { get; set; } = 0.0;       // Score de estructura al registrar/upgrade
        public double QualityScore { get; set; } = 0.0;         // Score de calidad multidimensional
        public int DecisionTimeframe { get; set; } = 15;        // TF de decisión para ATR/bias
        public int LastUpgradedBar { get; set; } = -1;          // Última barra donde se hizo upgrade
        public DateTime? LastUpgradedTime { get; set; }         // Timestamp del último upgrade
        public int UpgradeCount { get; set; } = 0;              // Contador de upgrades
    }

    /// <summary>
    /// Gestor institucional de órdenes Limit
    /// Maneja ejecución, caducidad inteligente y tracking
    /// </summary>
    public class TradeManager
    {
        private readonly List<TradeRecord> _trades;
        private readonly ILogger _logger;
        private readonly TradeLogger _tradeLogger;
        private readonly EngineConfig _config;
        private readonly Dictionary<string, int> _cancelledOrdersCooldown; // Key: SourceStructureId, Value: BarExpiration
        private readonly Dictionary<string, int> _bosFirstDetection; // V6.0i.6: Key: TradeId, Value: BarIndex primera detección BOS
        private readonly double _contractSize;
        private readonly double _pointValue;

        public TradeManager(EngineConfig config, ILogger logger, TradeLogger tradeLogger = null, double contractSize = 1.0, double pointValue = 5.0)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tradeLogger = tradeLogger;
            _contractSize = contractSize;
            _pointValue = pointValue;
            _trades = new List<TradeRecord>();
            _cancelledOrdersCooldown = new Dictionary<string, int>();
            _bosFirstDetection = new Dictionary<string, int>(); // V6.0i.6
        }

        // Tracking de OHLC post-registro (ventana fija en 5m)
        private readonly Dictionary<string, int> _trackingWindows = new Dictionary<string, int>();
        private const int TRACKING_BARS_5M = 100;

        // Tracking de última barra registrada por ZoneId para DEDUP inteligente (V6.0m)
        private readonly Dictionary<string, int> _lastRegisteredBarByZoneId = new Dictionary<string, int>();

        // Contadores para REPLAY_SUMMARY (V6.0m)
        private int _replaySignalsCount = 0;
        private int _replayAcceptedCount = 0;
        private int _replayDedupIdenticalCount = 0;
        private int _replayDedupCooldownCount = 0;
        private int _replayRegisterTooFarCount = 0;
        private int _replayTpTooFarCount = 0;
        private int _replayLowRRCount = 0;
        private int _replayConcurrencyCount = 0;
        private int _replayLastProgressBar = -1;

        public bool HasActiveTrackingWindows()
        {
            return _trackingWindows.Count > 0;
        }

        public void DecrementTrackingWindowsForTF(int tfMinutes)
        {
            if (tfMinutes != 5) return;
            if (_trackingWindows.Count == 0) return;

            var toRemove = new List<string>();
            foreach (var kvp in _trackingWindows.ToList())
            {
                _trackingWindows[kvp.Key] = kvp.Value - 1;
                _logger.Debug($"[TRADE][TRACK_TICK] TradeID={kvp.Key} RemainingBars={_trackingWindows[kvp.Key]}");
                if (_trackingWindows[kvp.Key] <= 0)
                {
                    _logger.Info($"[TRADE][TRACK_END] TradeID={kvp.Key}");
                    toRemove.Add(kvp.Key);
                }
            }
            foreach (var id in toRemove) _trackingWindows.Remove(id);
        }

        /// <summary>
        /// Resetea contadores de replay para comenzar un nuevo análisis
        /// </summary>
        public void ResetReplayCounters()
        {
            _replaySignalsCount = 0;
            _replayAcceptedCount = 0;
            _replayDedupIdenticalCount = 0;
            _replayDedupCooldownCount = 0;
            _replayRegisterTooFarCount = 0;
            _replayTpTooFarCount = 0;
            _replayLowRRCount = 0;
            _replayConcurrencyCount = 0;
            _replayLastProgressBar = -1;
        }

        /// <summary>
        /// Imprime resumen de contadores de replay (throttle cada 250 barras)
        /// </summary>
        public void PrintReplaySummary(int currentBar, bool force = false)
        {
            if (!force && (currentBar - _replayLastProgressBar < 250)) return;
            
            _replayLastProgressBar = currentBar;
            _logger?.Info($"[REPLAY_SUMMARY] Bar={currentBar} Signals={_replaySignalsCount} Accepted={_replayAcceptedCount} " +
                          $"Rejected: DEDUP_IDENTICAL={_replayDedupIdenticalCount} DEDUP_COOLDOWN={_replayDedupCooldownCount} " +
                          $"REGISTER_TOO_FAR={_replayRegisterTooFarCount} TP_TOO_FAR={_replayTpTooFarCount} " +
                          $"LOW_RR={_replayLowRRCount} CONCURRENCY={_replayConcurrencyCount}");
        }

        /// <summary>
        /// Registra una nueva orden Limit
        /// </summary>
        public void RegisterTrade(string action, double entry, double sl, double tp, int entryBar, DateTime entryBarTime, int tfDominante, string sourceStructureId, double currentPrice, double distanceToEntryATR = -1.0, string currentRegime = "Normal", double structureScore = 0.0)
        {
            // LOG DE ENTRADA: Contar TODAS las llamadas a RegisterTrade
            _logger?.Info($"[TRADE][REGISTER_ATTEMPT] Bar={entryBar} {action} @{entry:F2} SL={sl:F2} TP={tp:F2} Zone={sourceStructureId} DistATR={distanceToEntryATR:F2}");
            
            // FILTRO 1: Verificar cooldown (estructura cancelada recientemente)
            if (!string.IsNullOrEmpty(sourceStructureId) && _cancelledOrdersCooldown.ContainsKey(sourceStructureId))
            {
                int barExpiration = _cancelledOrdersCooldown[sourceStructureId];
                if (entryBar < barExpiration)
                {
                    int barsRemaining = barExpiration - entryBar;
                    int pendingCount = _trades.Count(t => t.Status == TradeStatus.PENDING);
                    int executedCount = _trades.Count(t => t.Status == TradeStatus.EXECUTED);
                    _logger.Info($"[TRADE][REJECT_COOLDOWN_CANCELLED] Bar={entryBar} Zone={sourceStructureId} BarsRemain={barsRemaining} | State: PENDING={pendingCount} EXECUTED={executedCount}");
                    return;
                }
                else
                {
                    // Cooldown expirado, eliminar del diccionario
                    _cancelledOrdersCooldown.Remove(sourceStructureId);
                    _logger.Debug($"[TradeManager] ✅ Cooldown EXPIRADO para estructura {sourceStructureId}, orden permitida");
                }
            }
            
            // ========================================================================
            // Gate PRE-REGISTRO por distancia (ATR del TF de decisión)
            // ========================================================================
            double distancePoints = Math.Abs(currentPrice - entry);
            bool isHighVol = (currentRegime == "HighVol");
            bool hasValidATR = (distanceToEntryATR > 0 && distanceToEntryATR != 1.0 && !double.IsNaN(distanceToEntryATR));
            double registerGate = isHighVol ? _config.MaxDistanceToRegister_ATR_HighVol : _config.MaxDistanceToRegister_ATR_Normal;
            _logger.Debug($"[TRADE][GATE_INPUT] Regime={currentRegime} Entry={entry:F2} Current={currentPrice:F2} DistATR={distanceToEntryATR:F2} DistPts={distancePoints:F2} RegisterGate={registerGate:F2} ATRPtsGateHV={_config.MaxDistanceToEntry_Points_HighVol:F2}");
            if (hasValidATR)
            {
                if (distanceToEntryATR > registerGate)
                {
                    _replayRegisterTooFarCount++;
                    int pendingCount = _trades.Count(t => t.Status == TradeStatus.PENDING);
                    int executedCount = _trades.Count(t => t.Status == TradeStatus.EXECUTED);
                    _logger.Info($"[TRADE][REJECT_TOO_FAR] Bar={entryBar} DistATR={distanceToEntryATR:F2}>{registerGate:F2} (Reg={currentRegime}) | State: PENDING={pendingCount} EXECUTED={executedCount}");
                    PrintReplaySummary(entryBar);
                    return;
                }
            }
            else if (isHighVol && distancePoints > _config.MaxDistanceToEntry_Points_HighVol)
            {
                _replayRegisterTooFarCount++;
                int pendingCount = _trades.Count(t => t.Status == TradeStatus.PENDING);
                int executedCount = _trades.Count(t => t.Status == TradeStatus.EXECUTED);
                _logger.Info($"[TRADE][REJECT_TOO_FAR_FALLBACK] Bar={entryBar} DistPts={distancePoints:F2}>{_config.MaxDistanceToEntry_Points_HighVol:F2} | State: PENDING={pendingCount} EXECUTED={executedCount}");
                PrintReplaySummary(entryBar);
                return;
            }
            // ========================================================================
            // Gate adicional: Cap de TP por puntos (evitar objetivos "de swing" en intradía)
            double tpDistancePts = Math.Abs(tp - entry);
            double maxTpPoints = isHighVol ? _config.MaxTPDistancePoints_HighVol : _config.MaxTPDistancePoints_Normal;
            if (tpDistancePts > maxTpPoints * (1.0 + _config.ValidationTolerancePercent))
            {
                _replayTpTooFarCount++;
                int pendingCount = _trades.Count(t => t.Status == TradeStatus.PENDING);
                int executedCount = _trades.Count(t => t.Status == TradeStatus.EXECUTED);
                _logger.Info($"[TRADE][REJECT_TP_TOO_FAR] Bar={entryBar} TPDist={tpDistancePts:F2}pts > Cap={maxTpPoints:F2}pts (+{_config.ValidationTolerancePercent:P0}) | State: PENDING={pendingCount} EXECUTED={executedCount}");
                PrintReplaySummary(entryBar);
                return;
            }
            // ========================================================================
            // Gate pre-registro adicional: si RR < 1.30, NO registrar (guardarraíl mínimo)
            double rrPlanned = 0.0;
            if (action == "BUY")
            {
                double slDist = Math.Max(1e-9, entry - sl);
                double tpDist = Math.Max(0.0, tp - entry);
                rrPlanned = tpDist / slDist;
            }
            else
            {
                double slDist = Math.Max(1e-9, sl - entry);
                double tpDist = Math.Max(0.0, entry - tp);
                rrPlanned = tpDist / slDist;
            }
            if (rrPlanned < 1.30)
            {
                _replayLowRRCount++;
                int pendingCount = _trades.Count(t => t.Status == TradeStatus.PENDING);
                int executedCount = _trades.Count(t => t.Status == TradeStatus.EXECUTED);
                _logger.Info($"[TRADE][REJECT_LOW_RR] Bar={entryBar} RR={rrPlanned:F2} < 1.30 | State: PENDING={pendingCount} EXECUTED={executedCount}");
                PrintReplaySummary(entryBar);
                return;
            }
            // ========================================================================
            
            // ========================================================================
            // FILTRO 2: UPGRADE IN PLACE - V6.0n
            // Si ya existe una orden PENDING de la misma zona, evaluar si la nueva señal es mejor (Pareto 2-de-4)
            // Si es mejor: actualizar la PENDING existente
            // Si no es mejor: rechazar con REJECT_DEDUP_COOLDOWN
            // ========================================================================

            _replaySignalsCount++; // Contador para REPLAY_SUMMARY

            // Fallback estable para zoneKey si sourceStructureId viene vacío
            string zoneKey = !string.IsNullOrEmpty(sourceStructureId) 
                ? $"{sourceStructureId}_{action}"
                : $"{action}@{entry:F2}/{sl:F2}/{tp:F2}";
            
            // Buscar PENDING de la misma zona
            var pendingFromZone = _trades
                .Where(t => t.SourceStructureId == sourceStructureId 
                            && t.Action == action 
                            && t.Status == TradeStatus.PENDING)
                .OrderByDescending(t => t.EntryBar)
                .FirstOrDefault();

            if (pendingFromZone != null)
            {
                // Calcular métricas de la nueva señal
                const double EPSILON = 1e-9;
                double currentATR = Math.Max(EPSILON, distanceToEntryATR > 0 ? distanceToEntryATR : 1.0); // Fallback si ATR no está disponible
                double currentRR = rrPlanned; // Ya calculado arriba
                double structScore = structureScore; // Pasado como parámetro desde CoreEngine
                double qualityScore = CalculateSignalQuality(distanceToEntryATR, currentRR, structScore);
                
                // Evaluar si la nueva señal "domina" con regla Pareto 2-de-4
                bool closerToPrice = distanceToEntryATR < pendingFromZone.DistanceToEntryATR * 0.7;
                double lastRR = Math.Abs(pendingFromZone.TP - pendingFromZone.Entry) 
                                / Math.Max(EPSILON, Math.Abs(pendingFromZone.SL - pendingFromZone.Entry));
                bool betterRR = currentRR > lastRR * 1.2;
                bool structureImproved = structScore > pendingFromZone.StructureScore * 1.1;
                double entryDeltaATR = Math.Abs(entry - pendingFromZone.Entry) / Math.Max(EPSILON, currentATR);
                bool significantRepricing = entryDeltaATR > 0.5;
                
                int improvements = 0;
                if (closerToPrice) improvements++;
                if (betterRR) improvements++;
                if (structureImproved) improvements++;
                if (significantRepricing) improvements++;
                
                // Si NO domina (menos de 2 mejoras) → Rechazar
                if (improvements < 2)
                {
                    _replayDedupCooldownCount++;
                    int pendingCount = _trades.Count(t => t.Status == TradeStatus.PENDING);
                    int executedCount = _trades.Count(t => t.Status == TradeStatus.EXECUTED);
                    _logger.Info($"[TRADE][REJECT_DEDUP_COOLDOWN] Bar={entryBar} Zone={sourceStructureId} " +
                                 $"Improvements={improvements}/4 (Dist={closerToPrice}, Entry={significantRepricing}, " +
                                 $"RR={betterRR}, Struct={structureImproved}) | Razón: necesita 2+ mejoras | " +
                                 $"State: PENDING={pendingCount} EXECUTED={executedCount}");
                    PrintReplaySummary(entryBar);
                    return;
                }
                
                // Si domina → Verificar límite de 1 upgrade por barra
                if (pendingFromZone.LastUpgradedBar == entryBar)
                {
                    int pendingCount = _trades.Count(t => t.Status == TradeStatus.PENDING);
                    int executedCount = _trades.Count(t => t.Status == TradeStatus.EXECUTED);
                    _logger.Info($"[TRADE][REJECT_UPGRADE_LIMIT] Bar={entryBar} Zone={sourceStructureId} " +
                                 $"Ya se hizo 1 upgrade en esta barra | State: PENDING={pendingCount} EXECUTED={executedCount}");
                    PrintReplaySummary(entryBar);
                    return;
                }
                
                // UPGRADE IN PLACE: Actualizar PENDING con la nueva señal
                double oldEntry = pendingFromZone.Entry;
                double oldSL = pendingFromZone.SL;
                double oldTP = pendingFromZone.TP;
                double oldDistATR = pendingFromZone.DistanceToEntryATR;
                double oldQuality = pendingFromZone.QualityScore;
                
                // Actualizar todos los campos relevantes (incluido EntryBar para evitar staleness)
                pendingFromZone.EntryBar = entryBar;
                pendingFromZone.EntryBarTime = entryBarTime;
                pendingFromZone.Entry = entry;
                pendingFromZone.SL = sl;
                pendingFromZone.TP = tp;
                pendingFromZone.DistanceToEntryATR = distanceToEntryATR;
                pendingFromZone.StructureScore = structScore;
                pendingFromZone.QualityScore = qualityScore;
                pendingFromZone.RegistrationPrice = currentPrice;
                pendingFromZone.LastUpgradedBar = entryBar;
                pendingFromZone.LastUpgradedTime = entryBarTime;
                pendingFromZone.UpgradeCount++;
                
                _replayAcceptedCount++;
                _lastRegisteredBarByZoneId[zoneKey] = entryBar;
                
                _logger.Info($"[TRADE][UPGRADED] Bar={entryBar} Zone={sourceStructureId} TradeId={pendingFromZone.Id} " +
                             $"UpgradeCount={pendingFromZone.UpgradeCount} | " +
                             $"Old: Entry={oldEntry:F2} SL={oldSL:F2} TP={oldTP:F2} DistATR={oldDistATR:F2} RR={lastRR:F2} Q={oldQuality:F3} | " +
                             $"New: Entry={entry:F2} SL={sl:F2} TP={tp:F2} DistATR={distanceToEntryATR:F2} RR={currentRR:F2} Q={qualityScore:F3}");
                
                PrintReplaySummary(entryBar);
                return; // Upgrade exitoso, no crear nueva orden
            }
            
            // FILTRO 3: Verificar límite de operaciones concurrentes (V5.7d)
            // Solo contar operaciones EJECUTADAS (activas con riesgo real), NO PENDING
            int activeCount = _trades.Count(t => t.Status == TradeStatus.EXECUTED);
            int pendingCountNow = _trades.Count(t => t.Status == TradeStatus.PENDING);
            
            if (activeCount >= _config.MaxConcurrentTrades)
            {
                _replayConcurrencyCount++;
                _logger.Info($"[TRADE][REJECT_CONCURRENCY] Bar={entryBar} ActiveExecuted={activeCount}>={_config.MaxConcurrentTrades} | State: PENDING={pendingCountNow} EXECUTED={activeCount}");
                PrintReplaySummary(entryBar);
                return;
            }

            var trade = new TradeRecord
            {
                Id = Guid.NewGuid().ToString(),
                EntryBar = entryBar,
                EntryBarTime = entryBarTime,
                Entry = entry,
                SL = sl,
                TP = tp,
                Action = action,
                Status = TradeStatus.PENDING,
                ExecutionBar = -1,
                ExecutionBarTime = DateTime.MinValue,
                ExitBar = -1,
                ExitBarTime = DateTime.MinValue,
                ExitReason = null,
                TFDominante = tfDominante,
                SourceStructureId = sourceStructureId,
                RegistrationPrice = currentPrice,  // Guardar precio de registro para determinar LIMIT vs STOP
                // V6.0n: Campos nuevos
                DistanceToEntryATR = distanceToEntryATR,
                DecisionTimeframe = tfDominante,
                StructureScore = structureScore,  // Pasado desde CoreEngine
                QualityScore = CalculateSignalQuality(distanceToEntryATR, rrPlanned, structureScore),
                LastUpgradedBar = -1,
                LastUpgradedTime = null,
                UpgradeCount = 0
            };

            _trades.Add(trade);

            // Actualizar tracking de última barra registrada por ZoneId
            _lastRegisteredBarByZoneId[zoneKey] = entryBar;
            _replayAcceptedCount++;

            _logger.Info($"[TradeManager] 🎯 ORDEN REGISTRADA: {action} LIMIT @ {entry:F2} | SL={sl:F2}, TP={tp:F2} | Bar={entryBar} | Estructura={sourceStructureId}");
            PrintReplaySummary(entryBar);
            
            // Log to CSV
            _tradeLogger?.LogOrderRegistered(action, entry, sl, tp, entryBar, entryBarTime, sourceStructureId, _contractSize, _pointValue);

            // Tracking OHLC post-registro (independiente del estado de la orden)
            if (_config.EnableOHLCLogging)
            {
                _trackingWindows[trade.Id] = TRACKING_BARS_5M;
                _logger.Info($"[TRADE][TRACK_START] TradeID={trade.Id} BarsToTrack={TRACKING_BARS_5M}");
            }
        }

        /// <summary>
        /// Actualiza el estado de todas las órdenes en la barra actual
        /// V6.0i.5: Añadido parámetro currentRegime para aplicar gracia BOS en HighVol
        /// </summary>
        public void UpdateTrades(double currentHigh, double currentLow, int currentBar, DateTime currentBarTime, double currentPrice, 
                                 CoreEngine coreEngine, IBarDataProvider barData, string currentRegime = "Normal", int maxBarIndex = int.MaxValue)
        {
            var activeTrades = _trades.Where(t => 
                t.Status == TradeStatus.PENDING || t.Status == TradeStatus.EXECUTED
            ).ToList();

            // V6.0n: Contadores para log de estado
            int executedNow = 0;
            int expiredNow = 0;
            int cancelledNow = 0;
            int slHitNow = 0;
            int tpHitNow = 0;
            var expirationReasons = new Dictionary<string, int>();

            foreach (var trade in activeTrades)
            {
                var initialStatus = trade.Status; // V6.0n: Rastrear estado inicial
                
                // Gate extremo por antigüedad de la fuente del entry (intradia: > MaxEntryAgeHours)
                if (!string.IsNullOrEmpty(trade.SourceStructureId) && coreEngine != null && barData != null)
                {
                    var src = coreEngine.GetStructureById(trade.SourceStructureId);
                    if (src != null)
                    {
                        DateTime createdTime = barData.GetBarTime(src.TF, src.CreatedAtBarIndex);
                        if (createdTime != DateTime.MinValue)
                        {
                            double ageHours = (currentBarTime - createdTime).TotalHours;
                            if (ageHours > _config.MaxEntryAgeHours)
                            {
                                _logger.Info($"[TRADE][REJECT_ANCIENT] Source={src.Id} TF={src.TF} AgeH={ageHours:F1} > MaxEntryAgeHours({_config.MaxEntryAgeHours:F1}) → CANCEL");
                                trade.Status = TradeStatus.CANCELLED;
                                trade.ExitBar = currentBar;
                                trade.ExitBarTime = currentBarTime;
                                trade.ExitReason = "ANCIENT_ENTRY_SOURCE";
                                
                                // Validación de TIME_ANOMALY (V6.0m)
                                if (trade.ExitBarTime < trade.EntryBarTime)
                                {
                                    _logger.Warning($"[TIME_ANOMALY] Exit<Entry Zone={trade.SourceStructureId} Entry={trade.EntryBarTime:yyyy-MM-dd HH:mm:ss} Exit={trade.ExitBarTime:yyyy-MM-dd HH:mm:ss} EntryBar={trade.EntryBar} ExitBar={trade.ExitBar}");
                                }
                                
                                cancelledNow++; // V6.0n
                                if (!expirationReasons.ContainsKey("ANCIENT")) expirationReasons["ANCIENT"] = 0;
                                expirationReasons["ANCIENT"]++;
                                continue;
                            }
                        }
                    }
                }
                if (trade.Status == TradeStatus.PENDING)
                {
                    // PASO 1: Verificar caducidad inteligente ANTES de verificar ejecución
                    // V6.0n: Pasar maxBarIndex para corte temporal (determinismo MTF)
                    bool wasInvalidated = CheckInvalidation(trade, currentPrice, currentBar, currentBarTime, coreEngine, barData, currentRegime, maxBarIndex);
                    if (wasInvalidated)
                    {
                        // V6.0n: Rastrear expiración/cancelación por razón
                        if (trade.Status == TradeStatus.CANCELLED)
                        {
                            cancelledNow++;
                            string reason = trade.ExitReason ?? "UNKNOWN";
                            if (!expirationReasons.ContainsKey(reason)) expirationReasons[reason] = 0;
                            expirationReasons[reason]++;
                        }
                        continue; // La orden fue cancelada, pasar a la siguiente
                    }

                    // PASO 2: Determinar tipo de orden (LIMIT vs STOP) según precio de registro
                    bool isBuyLimit = (trade.Action == "BUY" && trade.RegistrationPrice > trade.Entry);
                    bool isSellLimit = (trade.Action == "SELL" && trade.RegistrationPrice < trade.Entry);
                    
                    string orderType = trade.Action == "BUY" 
                        ? (isBuyLimit ? "BUY LIMIT" : "BUY STOP")
                        : (isSellLimit ? "SELL LIMIT" : "SELL STOP");

                    _logger.Info($"[ENTRY_GATE] Trade={trade.Id} Type={orderType} Price={currentPrice:F2} Entry={trade.Entry:F2} High={currentHigh:F2} Low={currentLow:F2}");

                    // PASO 3: Verificar si el precio llegó al Entry según tipo de orden
                    bool entryHit = false;

                    if (trade.Action == "BUY")
                    {
                        if (isBuyLimit)
                            entryHit = currentLow <= trade.Entry;  // BUY LIMIT: precio baja hasta Entry
                        else
                            entryHit = currentHigh >= trade.Entry; // BUY STOP: precio sube hasta Entry
                    }
                    else if (trade.Action == "SELL")
                    {
                        if (isSellLimit)
                            entryHit = currentHigh >= trade.Entry; // SELL LIMIT: precio sube hasta Entry
                        else
                            entryHit = currentLow <= trade.Entry;  // SELL STOP: precio baja hasta Entry
                    }

                    if (entryHit)
                    {
                        // V6.0k: Verificar límite de concurrencia antes de ejecutar
                        int executedCount = _trades.Count(t => t.Status == TradeStatus.EXECUTED);
                        if (executedCount >= _config.MaxConcurrentTrades)
                        {
                            _logger.Info($"[TRADE][SKIP_EXEC] Order={trade.Id} Action={trade.Action} Entry={trade.Entry:F2} → MaxConcurrentTrades alcanzado ({executedCount}/{_config.MaxConcurrentTrades})");
                            continue; // No ejecutar, saltar a siguiente trade
                        }
                        
                        trade.Status = TradeStatus.EXECUTED;
                        trade.ExecutionBar = currentBar;
                        trade.ExecutionBarTime = currentBarTime; // V5.7e
                        executedNow++; // V6.0n
                        _logger.Info($"[TradeManager] ✅ ORDEN EJECUTADA: {orderType} @ {trade.Entry:F2} en barra {currentBar}");
                        _logger.Info($"[DEBUG-EXEC] Trade={trade.Id} ExecutionBar={currentBar} ExecutionBarTime={currentBarTime:yyyy-MM-dd HH:mm:ss} currentHigh={currentHigh:F2} currentLow={currentLow:F2} RegistrationPrice={trade.RegistrationPrice:F2}");
                    }
                }
                else if (trade.Status == TradeStatus.EXECUTED)
                {
                    // PASO 3: Verificar SL/TP
                    bool hitSL = false;
                    bool hitTP = false;

                    if (trade.Action == "BUY")
                    {
                        hitSL = currentLow <= trade.SL;
                        hitTP = currentHigh >= trade.TP;
                    }
                    else if (trade.Action == "SELL")
                    {
                        hitSL = currentHigh >= trade.SL;
                        hitTP = currentLow <= trade.TP;
                    }

                    if (hitSL)
                    {
                        trade.Status = TradeStatus.SL_HIT;
                        trade.ExitBar = currentBar;
                        trade.ExitBarTime = currentBarTime;
                        trade.ExitReason = "SL";
                        slHitNow++; // V6.0n
                        
                        // Validación de TIME_ANOMALY (V6.0m)
                        if (trade.ExitBarTime < trade.EntryBarTime)
                        {
                            _logger.Warning($"[TIME_ANOMALY] Exit<Entry Zone={trade.SourceStructureId} Entry={trade.EntryBarTime:yyyy-MM-dd HH:mm:ss} Exit={trade.ExitBarTime:yyyy-MM-dd HH:mm:ss} EntryBar={trade.EntryBar} ExitBar={trade.ExitBar}");
                        }
                        
                        _logger.Info($"[TradeManager] 🔴 CERRADA POR SL: {trade.Action} @ {trade.Entry:F2} en barra {currentBar}");
                        
                        // Log to CSV
                        _tradeLogger?.LogOrderClosedSL(trade.Action, trade.Entry, trade.SL, trade.EntryBar, trade.EntryBarTime, currentBar, currentBarTime, _contractSize, _pointValue);
                    }
                    else if (hitTP)
                    {
                        trade.Status = TradeStatus.TP_HIT;
                        trade.ExitBar = currentBar;
                        trade.ExitBarTime = currentBarTime;
                        trade.ExitReason = "TP";
                        tpHitNow++; // V6.0n
                        
                        // Validación de TIME_ANOMALY (V6.0m)
                        if (trade.ExitBarTime < trade.EntryBarTime)
                        {
                            _logger.Warning($"[TIME_ANOMALY] Exit<Entry Zone={trade.SourceStructureId} Entry={trade.EntryBarTime:yyyy-MM-dd HH:mm:ss} Exit={trade.ExitBarTime:yyyy-MM-dd HH:mm:ss} EntryBar={trade.EntryBar} ExitBar={trade.ExitBar}");
                        }
                        
                        _logger.Info($"[TradeManager] 🟢 CERRADA POR TP: {trade.Action} @ {trade.Entry:F2} en barra {currentBar}");
                        
                        // Log to CSV
                        _tradeLogger?.LogOrderClosedTP(trade.Action, trade.Entry, trade.TP, trade.EntryBar, trade.EntryBarTime, currentBar, currentBarTime, _contractSize, _pointValue);
                    }
                }
            }
            
            // V6.0n: PASO 4 - Log de estado cuando hay cambios
            int totalChanges = executedNow + cancelledNow + slHitNow + tpHitNow;
            if (totalChanges > 0)
            {
                int pending = _trades.Count(t => t.Status == TradeStatus.PENDING);
                int executed = _trades.Count(t => t.Status == TradeStatus.EXECUTED);
                
                var reasonsSummary = expirationReasons.Count > 0 
                    ? string.Join(", ", expirationReasons.Select(kv => $"{kv.Key}={kv.Value}"))
                    : "none";
                
                _logger.Info($"[PENDING_STATE] Bar={currentBar} Pending={pending} Executed={executed} | Changes: Exec+{executedNow} Cancel+{cancelledNow} SL+{slHitNow} TP+{tpHitNow} | Reasons: {reasonsSummary}");
            }
        }

        /// <summary>
        /// Verifica si una orden PENDING debe ser cancelada por invalidación estructural
        /// V6.0i.5: Añadido parámetro currentRegime para aplicar gracia BOS en HighVol
        /// </summary>
        private bool CheckInvalidation(TradeRecord trade, double currentPrice, int currentBar, DateTime currentBarTime,
                                       CoreEngine coreEngine, IBarDataProvider barData, string currentRegime, int maxBarIndex = int.MaxValue)
        {
            _logger.Info($"[INVALIDATION_CHECK] Trade={trade.Id} Src={trade.SourceStructureId} Status={trade.Status} CurrentBar={currentBar} EntryBar={trade.EntryBar}");
            
            // REGLA 1 (PRIORITARIA): Caducidad por Invalidación Estructural
            // La estructura que generó la orden ya no existe, está inactiva, o su Score decayó
            if (!string.IsNullOrEmpty(trade.SourceStructureId))
            {
                var sourceStructure = coreEngine.GetStructureById(trade.SourceStructureId);
                
                if (sourceStructure == null || !sourceStructure.IsActive || sourceStructure.Score < _config.MinConfidenceForWait)
                {
                    // V5.6.5: Gracia estructural
                    int grace = _config.StructuralInvalidationGraceBars;
                    bool priceApproaching = (trade.Action == "BUY" && currentPrice > trade.Entry) ? false
                                            : (trade.Action == "SELL" && currentPrice < trade.Entry) ? false
                                            : true;

                    // Si está acercándose al Entry o aún dentro de gracia, NO cancelar
                    if ((currentBar - trade.EntryBar) < grace || priceApproaching)
                    {
                        _logger.Debug($"[TradeManager] ⏳ Gracia estructural activa ({currentBar - trade.EntryBar}/{grace} barras), no se cancela {trade.Action}");
                        return false;
                    }

                    trade.Status = TradeStatus.CANCELLED;
                    trade.ExitBar = currentBar;
                    trade.ExitBarTime = currentBarTime;
                    trade.ExitReason = "STRUCTURAL_INVALIDATION";
                    
                    // Validación de TIME_ANOMALY (V6.0m)
                    if (trade.ExitBarTime < trade.EntryBarTime)
                    {
                        _logger.Warning($"[TIME_ANOMALY] Exit<Entry Zone={trade.SourceStructureId} Entry={trade.EntryBarTime:yyyy-MM-dd HH:mm:ss} Exit={trade.ExitBarTime:yyyy-MM-dd HH:mm:ss} EntryBar={trade.EntryBar} ExitBar={trade.ExitBar}");
                    }
                    
                    string reason = sourceStructure == null ? "estructura no existe" 
                        : !sourceStructure.IsActive ? "estructura inactiva" 
                        : $"score decayó a {sourceStructure.Score:F2}";
                    
                    _logger.Warning($"[TradeManager] ❌ ORDEN EXPIRADA (Estructural): {trade.Action} @ {trade.Entry:F2} | Razón: {reason}");
                    _tradeLogger?.LogOrderExpired(trade.Action, trade.Entry, currentBar, currentBarTime, reason);
                    AddToCooldown(trade.SourceStructureId, currentBar);
                    return true;
                }
            }

            // REGLA 2: Caducidad por BOS/CHoCH contradictorio
            // V6.0n: Pasar maxBarIndex para corte temporal (determinismo MTF)
            if (CheckBOSContradictory(trade, coreEngine, barData, currentBarTime, currentRegime, maxBarIndex))
            {
                trade.Status = TradeStatus.CANCELLED;
                trade.ExitBar = currentBar;
                trade.ExitBarTime = currentBarTime;
                trade.ExitReason = "BOS_CONTRARY";
                
                // Validación de TIME_ANOMALY (V6.0m)
                if (trade.ExitBarTime < trade.EntryBarTime)
                {
                    _logger.Warning($"[TIME_ANOMALY] Exit<Entry Zone={trade.SourceStructureId} Entry={trade.EntryBarTime:yyyy-MM-dd HH:mm:ss} Exit={trade.ExitBarTime:yyyy-MM-dd HH:mm:ss} EntryBar={trade.EntryBar} ExitBar={trade.ExitBar}");
                }
                
                _logger.Warning($"[TradeManager] ❌ ORDEN CANCELADA por BOS contradictorio: {trade.Action} @ {trade.Entry:F2}");
                
                // Log to CSV
                _tradeLogger?.LogOrderCancelled(trade.Action, trade.Entry, currentBar, currentBarTime, "BOS contradictorio");
                
                // Añadir estructura al cooldown
                AddToCooldown(trade.SourceStructureId, currentBar);
                return true;
            }

            // (Anti contra-bias en PENDING eliminado; confiamos en pre-registro, caps y staleness)

            // ========================================================================
            // REGLA 3 (V6.0n): ADAPTIVE EXPIRATION - Expiración adaptativa basada en contexto
            // ========================================================================
            const double EPSILON = 1e-9;
            
            // Calcular barras transcurridas en TF decisión
            int tf = _config.DecisionTimeframeMinutes;
            int currentIdx = barData.GetBarIndexFromTime(tf, currentBarTime);
            int entryIdx   = barData.GetBarIndexFromTime(tf, trade.EntryBarTime);
            int barsWaiting = Math.Max(0, currentIdx - entryIdx);
            
            // Calcular distancia actual al entry en ATR
            double currentDistanceATR = CalculateCurrentDistanceATR(trade, currentIdx, barData);
            
            // ========================================================================
            // 3A: DECAY ESTRUCTURAL RELATIVO (Score cae >50% desde registro/upgrade)
            // ========================================================================
            if (!string.IsNullOrEmpty(trade.SourceStructureId) && trade.StructureScore > EPSILON)
            {
                var sourceStructure = coreEngine.GetStructureById(trade.SourceStructureId);
                if (sourceStructure != null && sourceStructure.IsActive)
                {
                    double currentScore = sourceStructure.Score;
                    double decay = (trade.StructureScore - currentScore) / Math.Max(EPSILON, trade.StructureScore);
                    
                    if (decay > 0.5) // Score cayó >50%
                    {
                        trade.Status = TradeStatus.CANCELLED;
                        trade.ExitBar = currentBar;
                        trade.ExitBarTime = currentBarTime;
                        trade.ExitReason = "ADAPTIVE_DECAY";
                        
                        if (trade.ExitBarTime < trade.EntryBarTime)
                        {
                            _logger.Warning($"[TIME_ANOMALY] Exit<Entry Zone={trade.SourceStructureId} Entry={trade.EntryBarTime:yyyy-MM-dd HH:mm:ss} Exit={trade.ExitBarTime:yyyy-MM-dd HH:mm:ss}");
                        }
                        
                    _logger.Warning($"[TradeManager][ADAPTIVE_DECAY] Trade={trade.Id} {trade.Action} @ {trade.Entry:F2} ScoreReg={trade.StructureScore:F2} ScoreNow={currentScore:F2} Decay={decay:P1} → CANCEL");
                    _tradeLogger?.LogOrderExpired(trade.Action, trade.Entry, currentBar, currentBarTime, $"ADAPTIVE_DECAY: {(decay * 100).ToString("F1", System.Globalization.CultureInfo.InvariantCulture)}%");
                        AddToCooldown(trade.SourceStructureId, currentBar);
                        return true;
                    }
                }
            }
            
            // ========================================================================
            // 3B: ALEJAMIENTO RELATIVO (DistanceATR actual > 2× registro Y > 1.5)
            // ========================================================================
            if (currentDistanceATR > 0 && trade.DistanceToEntryATR > 0)
            {
                double distanceRatio = currentDistanceATR / Math.Max(EPSILON, trade.DistanceToEntryATR);
                
                if (distanceRatio > 2.0 && currentDistanceATR > 1.5)
                {
                    trade.Status = TradeStatus.CANCELLED;
                    trade.ExitBar = currentBar;
                    trade.ExitBarTime = currentBarTime;
                    trade.ExitReason = "ADAPTIVE_DEPARTURE";
                    
                    if (trade.ExitBarTime < trade.EntryBarTime)
                    {
                        _logger.Warning($"[TIME_ANOMALY] Exit<Entry Zone={trade.SourceStructureId} Entry={trade.EntryBarTime:yyyy-MM-dd HH:mm:ss} Exit={trade.ExitBarTime:yyyy-MM-dd HH:mm:ss}");
                    }
                    
                    _logger.Warning($"[TradeManager][ADAPTIVE_DEPARTURE] Trade={trade.Id} {trade.Action} @ {trade.Entry:F2} DistReg={trade.DistanceToEntryATR:F2}ATR DistNow={currentDistanceATR:F2}ATR Ratio={distanceRatio:F2}x → CANCEL");
                    _tradeLogger?.LogOrderExpired(trade.Action, trade.Entry, currentBar, currentBarTime, $"ADAPTIVE_DEPARTURE: {distanceRatio.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}x");
                    AddToCooldown(trade.SourceStructureId, currentBar);
                    return true;
                }
            }
            
            // ========================================================================
            // 3C: VELOCIDAD DE ALEJAMIENTO (ΔDistATR/barras > 0.2 ATR/bar)
            // ========================================================================
            if (currentDistanceATR > 0 && trade.DistanceToEntryATR >= 0 && barsWaiting > 0)
            {
                double deltaDistATR = currentDistanceATR - trade.DistanceToEntryATR;
                double velocity = deltaDistATR / Math.Max(1, barsWaiting);
                
                if (velocity > 0.2) // Alejándose >0.2 ATR por barra
                {
                    trade.Status = TradeStatus.CANCELLED;
                    trade.ExitBar = currentBar;
                    trade.ExitBarTime = currentBarTime;
                    trade.ExitReason = "ADAPTIVE_MOMENTUM";
                    
                    if (trade.ExitBarTime < trade.EntryBarTime)
                    {
                        _logger.Warning($"[TIME_ANOMALY] Exit<Entry Zone={trade.SourceStructureId} Entry={trade.EntryBarTime:yyyy-MM-dd HH:mm:ss} Exit={trade.ExitBarTime:yyyy-MM-dd HH:mm:ss}");
                    }
                    
                    _logger.Warning($"[TradeManager][ADAPTIVE_MOMENTUM] Trade={trade.Id} {trade.Action} @ {trade.Entry:F2} DistReg={trade.DistanceToEntryATR:F2} DistNow={currentDistanceATR:F2} Velocity={velocity:F3}ATR/bar Bars={barsWaiting} → CANCEL");
                    _tradeLogger?.LogOrderExpired(trade.Action, trade.Entry, currentBar, currentBarTime, $"ADAPTIVE_MOMENTUM: {velocity.ToString("F3", System.Globalization.CultureInfo.InvariantCulture)}ATR/bar");
                    AddToCooldown(trade.SourceStructureId, currentBar);
                    return true;
                }
            }
            
            // ========================================================================
            // 3D: BIAS EN CONTRA SOSTENIDO (>4 barras consecutivas)
            // ========================================================================
            // Obtener bias actual del TF de decisión
            string currentBias = coreEngine.CurrentMarketBias;
            bool biasAgainst = (trade.Action == "BUY" && currentBias == "Bearish") ||
                               (trade.Action == "SELL" && currentBias == "Bullish");
            
            if (biasAgainst && barsWaiting > 4)
            {
                trade.Status = TradeStatus.CANCELLED;
                trade.ExitBar = currentBar;
                trade.ExitBarTime = currentBarTime;
                trade.ExitReason = "ADAPTIVE_BIAS";
                
                if (trade.ExitBarTime < trade.EntryBarTime)
                {
                    _logger.Warning($"[TIME_ANOMALY] Exit<Entry Zone={trade.SourceStructureId} Entry={trade.EntryBarTime:yyyy-MM-dd HH:mm:ss} Exit={trade.ExitBarTime:yyyy-MM-dd HH:mm:ss}");
                }
                
                _logger.Warning($"[TradeManager][ADAPTIVE_BIAS] Trade={trade.Id} {trade.Action} @ {trade.Entry:F2} Bias={currentBias} Waiting={barsWaiting}bars → CANCEL");
                _tradeLogger?.LogOrderExpired(trade.Action, trade.Entry, currentBar, currentBarTime, $"ADAPTIVE_BIAS: {currentBias} {barsWaiting}bars");
                AddToCooldown(trade.SourceStructureId, currentBar);
                return true;
            }

            // ========================================================================
            // REGLA 4 (V6.0i.6): PENDING STALENESS - Tiempo y Distancia adaptativa (fallback)
            // ========================================================================
            
            // Límites adaptivos por régimen
            int maxBarsToFill = (currentRegime == "HighVol") 
                ? _config.MaxBarsToFillEntry_HighVol 
                : _config.MaxBarsToFillEntry;
            double maxDistanceATR_Cancel = (currentRegime == "HighVol")
                ? _config.MaxDistanceToEntry_ATR_Cancel_HighVol
                : _config.MaxDistanceToEntry_ATR_Cancel;
            
            // ========================================================================
            // 3A: Staleness por TIEMPO (barras esperando)
            // ========================================================================
            if (barsWaiting > maxBarsToFill)
            {
                trade.Status = TradeStatus.CANCELLED;
                trade.ExitBar = currentBar;
                trade.ExitBarTime = currentBarTime;
                trade.ExitReason = "PENDING_STALE_TIME";
                
                // Validación de TIME_ANOMALY (V6.0m)
                if (trade.ExitBarTime < trade.EntryBarTime)
                {
                    _logger.Warning($"[TIME_ANOMALY] Exit<Entry Zone={trade.SourceStructureId} Entry={trade.EntryBarTime:yyyy-MM-dd HH:mm:ss} Exit={trade.ExitBarTime:yyyy-MM-dd HH:mm:ss} EntryBar={trade.EntryBar} ExitBar={trade.ExitBar}");
                }
                
                _logger.Warning($"[TradeManager][PENDING_STALE_TIME] Trade={trade.Id} {trade.Action} @ {trade.Entry:F2} Regime={currentRegime} Waiting={barsWaiting}>{maxBarsToFill} → CANCEL");
                
                // Log to CSV
                _tradeLogger?.LogOrderExpired(trade.Action, trade.Entry, currentBar, currentBarTime, $"STALE_TIME: {barsWaiting}>{maxBarsToFill}bars");
                
                // Añadir estructura al cooldown
                AddToCooldown(trade.SourceStructureId, currentBar);
                return true;
            }
            
            // ========================================================================
            // 3B: Staleness por DISTANCIA (se aleja del entry en ATR del TF de decisión)
            // V6.0i.6b: Umbral fijo adaptativo por régimen calculado sobre el TF de decisión
            // ========================================================================
            int tfDecisionForStale = _config.DecisionTimeframeMinutes;
            int idxDec = barData.GetBarIndexFromTime(tfDecisionForStale, currentBarTime);
            double atrDec = barData.GetATR(tfDecisionForStale, 14, idxDec);
            double distanceToEntry = Math.Abs(currentPrice - trade.Entry);
            double distanceATR = (atrDec > 0) ? (distanceToEntry / atrDec) : 999.0;
            
            if (distanceATR > maxDistanceATR_Cancel)
            {
                trade.Status = TradeStatus.CANCELLED;
                trade.ExitBar = currentBar;
                trade.ExitBarTime = currentBarTime;
                trade.ExitReason = "PENDING_STALE_DIST";
                
                // Validación de TIME_ANOMALY (V6.0m)
                if (trade.ExitBarTime < trade.EntryBarTime)
                {
                    _logger.Warning($"[TIME_ANOMALY] Exit<Entry Zone={trade.SourceStructureId} Entry={trade.EntryBarTime:yyyy-MM-dd HH:mm:ss} Exit={trade.ExitBarTime:yyyy-MM-dd HH:mm:ss} EntryBar={trade.EntryBar} ExitBar={trade.ExitBar}");
                }
                
                _logger.Warning($"[TradeManager][PENDING_STALE_DIST] Trade={trade.Id} {trade.Action} @ {trade.Entry:F2} Regime={currentRegime} Dist={distanceATR:F2}ATR>{maxDistanceATR_Cancel:F2}ATR (TF={tfDecisionForStale}m) → CANCEL");
                
                // Log to CSV
                _tradeLogger?.LogOrderExpired(trade.Action, trade.Entry, currentBar, currentBarTime, $"STALE_DIST: {distanceATR:F2}>{maxDistanceATR_Cancel:F2}ATR");
                
                // Añadir estructura al cooldown
                AddToCooldown(trade.SourceStructureId, currentBar);
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Añade una estructura al cooldown para evitar re-registro inmediato
        /// </summary>
        private void AddToCooldown(string sourceStructureId, int currentBar)
        {
            if (string.IsNullOrEmpty(sourceStructureId))
                return;
            
            int barExpiration = currentBar + _config.TradeCooldownBars;
            
            // Si ya existe, actualizar con el nuevo tiempo de expiración (el más reciente)
            if (_cancelledOrdersCooldown.ContainsKey(sourceStructureId))
            {
                _cancelledOrdersCooldown[sourceStructureId] = barExpiration;
            }
            else
            {
                _cancelledOrdersCooldown.Add(sourceStructureId, barExpiration);
            }
            
            _logger.Debug($"[TradeManager] 🕒 Estructura {sourceStructureId} añadida al cooldown hasta barra {barExpiration} ({_config.TradeCooldownBars} barras)");
        }

        /// <summary>
        /// Verifica si hay un BOS/CHoCH contradictorio a la orden
        /// V6.0i.6: Debounce BOS - Confirmar que el BOS contradictorio persiste antes de cancelar
        /// </summary>
        private bool CheckBOSContradictory(TradeRecord trade, CoreEngine coreEngine, IBarDataProvider barData, DateTime currentBarTime, string currentRegime, int maxBarIndex = int.MaxValue)
        {
            // V6.0n: Calcular bias CON CORTE TEMPORAL para determinismo MTF
            // Solo considera BOS hasta maxBarIndex, no eventos futuros
            int tfDecision = _config.DecisionTimeframeMinutes;
            string currentBias = coreEngine.GetMarketBiasAtBar(tfDecision, maxBarIndex);
            if (_config.UseContextBiasForCancellations)
            {
                try
                {
                    int tf = 60; // 1H
                    // Alinear por TIEMPO de la barra de decisión
                    int index60 = barData.GetBarIndexFromTime(tf, currentBarTime);
                    if (index60 < 0)
                    {
                        // Fallback defensivo si no hay match exacto
                        // Alinear por tiempo de análisis (si está disponible via currentBarTime)
                        index60 = barData.GetBarIndexFromTime(tf, currentBarTime);
                    }

                    if (index60 >= 200)
                    {
                        double sum = 0.0;
                        for (int i = 0; i < 200; i++)
                        {
                            sum += barData.GetClose(tf, index60 - i);
                        }
                        double ema200approx = sum / 200.0; // SMA200 como aproximación para bias
                        double lastClose = barData.GetClose(tf, index60);
                        if (lastClose > ema200approx) currentBias = "Bullish";
                        else if (lastClose < ema200approx) currentBias = "Bearish";
                        else currentBias = "Neutral";

                        _logger.Info($"[DIAGNOSTICO][CancelBias] TF60 index={index60} Close={lastClose:F2} EMA200~={ema200approx:F2} Bias={currentBias}");
                    }
                    else
                    {
                        // Trazas ligeras para explicar por qué se omite la cancelación por falta de historial suficiente
                        _logger.Info($"[DIAGNOSTICO][CancelBias] SKIP idx60={index60} (<200) time={currentBarTime:yyyy-MM-dd HH:mm}");
                    }
                }
                catch { /* si falla, mantener currentMarketBias */ }
            }
            
            // ========================================================================
            // V6.0i.6: Detectar si hay BOS contradictorio
            // ========================================================================
            bool isBOSContradictory = false;
            
            // Para BUY, contradictorio si bias es Bearish
            if (trade.Action == "BUY" && currentBias == "Bearish")
                isBOSContradictory = true;
            
            // Para SELL, contradictorio si bias es Bullish
            if (trade.Action == "SELL" && currentBias == "Bullish")
                isBOSContradictory = true;

            // ========================================================================
            // V6.0i.6: DEBOUNCE BOS - Confirmar persistencia antes de cancelar
            // ========================================================================
            if (isBOSContradictory)
            {
                // Aplicar debounce solo en HighVol si está configurado
                bool applyDebounce = _config.EnableBOSDebounceInHighVolOnly ? (currentRegime == "HighVol") : true;
                
                if (applyDebounce && trade.Status == TradeStatus.PENDING)
                {
                    // Calcular índice actual en TF decisión
                    int tf = _config.DecisionTimeframeMinutes;
                    int currentIdx = barData.GetBarIndexFromTime(tf, currentBarTime);
                    
                    // Si es la primera detección, registrar y NO cancelar
                    if (!_bosFirstDetection.ContainsKey(trade.Id))
                    {
                        _bosFirstDetection[trade.Id] = currentIdx;
                        _logger.Info($"[TradeManager][BOS_DEBOUNCE_START] Trade={trade.Id} Action={trade.Action} Regime={currentRegime} FirstDetect={currentIdx} → Esperando confirmación");
                        return false; // NO cancelar en primera detección
                    }
                    
                    // Si ya está registrado, verificar si han pasado suficientes barras
                    int firstIdx = _bosFirstDetection[trade.Id];
                    int barsWithBOS = currentIdx - firstIdx;
                    
                    if (barsWithBOS >= _config.BOSDebounceBarReq)
                    {
                        // BOS persistió suficiente tiempo → CANCELAR
                        _bosFirstDetection.Remove(trade.Id); // Limpiar
                        _logger.Warning($"[TradeManager][BOS_DEBOUNCE_CANCEL] Trade={trade.Id} Action={trade.Action} Regime={currentRegime} BOS persistió {barsWithBOS} barras → CANCEL");
                        _logger.Info($"[DIAGNOSTICO][TM] Cancel_BOS Action={trade.Action} Bias={currentBias} Debounce={barsWithBOS}bars");
                        return true;
                    }
                    else
                    {
                        // BOS aún no persistió suficiente → NO cancelar
                        _logger.Info($"[TradeManager][BOS_DEBOUNCE_WAIT] Trade={trade.Id} Action={trade.Action} Regime={currentRegime} BOS={barsWithBOS}/{_config.BOSDebounceBarReq} → Esperando");
                        return false;
                    }
                }
                else
                {
                    // En Normal (o si debounce desactivado): cancelar inmediatamente
                    _logger.Warning($"[TradeManager] GlobalBias contradictorio: {trade.Action} @ {trade.Entry:F2} | Bias cambió a {currentBias} (Normal, no debounce)");
                    _logger.Info($"[DIAGNOSTICO][TM] Cancel_BOS Action={trade.Action} Bias={currentBias} Regime={currentRegime}");
                    return true;
                }
            }
            else
            {
                // NO hay BOS contradictorio → Limpiar tracking si existía
                if (_bosFirstDetection.ContainsKey(trade.Id))
                {
                    _bosFirstDetection.Remove(trade.Id);
                    _logger.Info($"[TradeManager][BOS_DEBOUNCE_CLEAR] Trade={trade.Id} BOS ya no es contradictorio → Limpiado tracking");
                }
                return false;
            }
        }

        /// <summary>
        /// Obtiene todas las órdenes (para visualización)
        /// </summary>
        public List<TradeRecord> GetAllTrades()
        {
            return _trades;
        }

        /// <summary>
        /// Obtiene solo las órdenes activas (PENDING o EXECUTED)
        /// </summary>
        public List<TradeRecord> GetActiveTrades()
        {
            return _trades.Where(t => 
                t.Status == TradeStatus.PENDING || t.Status == TradeStatus.EXECUTED
            ).ToList();
        }

        /// <summary>
        /// V6.0n: Calcula score de calidad de una señal sin parámetros artificiales
        /// </summary>
        private double CalculateSignalQuality(double distanceATR, double rr, double structureScore)
        {
            const double EPSILON = 1e-9;
            
            // 1. Proximidad (más cerca = mejor) - normalizada por distancia
            double proximityScore = 1.0 / (1.0 + Math.Max(0, distanceATR));
            
            // 2. R:R (mejor riesgo/recompensa) - saturación suave sin anchor
            double rrScore = Math.Max(0, rr) / (Math.Max(0, rr) + 1.0 + EPSILON); // Asíntota a 1.0
            
            // 3. Estructura (calidad intrínseca) - ya normalizado [0,1]
            double structScore = Math.Max(0, Math.Min(1.0, structureScore));
            
            // Ponderación equitativa (40% proximidad, 30% RR, 30% estructura)
            return (proximityScore * 0.4) + (rrScore * 0.3) + (structScore * 0.3);
        }

        /// <summary>
        /// V6.0n: Calcula distancia actual al Entry en ATR
        /// </summary>
        private double CalculateCurrentDistanceATR(TradeRecord trade, int currentBar, IBarDataProvider provider)
        {
            const double EPSILON = 1e-9;
            
            try
            {
                double currentPrice = provider.GetClose(trade.DecisionTimeframe, currentBar);
                double atr = provider.GetATR(trade.DecisionTimeframe, 14, currentBar); // period=14 por defecto
                
                if (atr < EPSILON)
                    return -1.0; // ATR inválido
                
                double distancePoints = Math.Abs(currentPrice - trade.Entry);
                return distancePoints / atr;
            }
            catch
            {
                return -1.0; // Error al calcular
            }
        }
    }
}

