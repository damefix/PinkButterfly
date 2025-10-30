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
        public int EntryBar { get; set; }           // Barra donde se generó la señal
        public DateTime EntryBarTime { get; set; }  // Timestamp de la barra de entrada
        public double Entry { get; set; }
        public double SL { get; set; }
        public double TP { get; set; }
        public string Action { get; set; }          // "BUY" o "SELL"
        public TradeStatus Status { get; set; }
        public int ExecutionBar { get; set; }       // Barra donde se ejecutó (si Status >= EXECUTED)
        public DateTime ExecutionBarTime { get; set; }  // Timestamp de la barra de ejecución
        public int ExitBar { get; set; }            // Barra donde se cerró/canceló
        public DateTime ExitBarTime { get; set; }   // Timestamp de la barra de salida
        public string ExitReason { get; set; }      // "SL", "TP", "BOS_CONTRARY", "PRICE_DEPARTED", etc.
        public int TFDominante { get; set; }        // TF dominante de la HeatZone
        public string SourceStructureId { get; set; } // ID de la estructura dominante que generó esta orden
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
        }

        /// <summary>
        /// Registra una nueva orden Limit
        /// </summary>
        public void RegisterTrade(string action, double entry, double sl, double tp, int entryBar, DateTime entryBarTime, int tfDominante, string sourceStructureId)
        {
            // FILTRO 0: Verificar límite de operaciones concurrentes
            if (_config.MaxConcurrentTrades > 0)
            {
                int activeTrades = _trades.Count(t => t.Status == TradeStatus.PENDING || t.Status == TradeStatus.EXECUTED);
                if (activeTrades >= _config.MaxConcurrentTrades)
                {
                    _logger.Debug($"[TradeManager] ⛔ Límite de operaciones concurrentes alcanzado ({activeTrades}/{_config.MaxConcurrentTrades}) → orden rechazada");
                    return;
                }
            }
            
            // FILTRO 1: Verificar cooldown (estructura cancelada recientemente)
            if (!string.IsNullOrEmpty(sourceStructureId) && _cancelledOrdersCooldown.ContainsKey(sourceStructureId))
            {
                int barExpiration = _cancelledOrdersCooldown[sourceStructureId];
                if (entryBar < barExpiration)
                {
                    int barsRemaining = barExpiration - entryBar;
                    _logger.Debug($"[TradeManager] ⏳ Orden en COOLDOWN: {action} @ {entry:F2} | Estructura={sourceStructureId} | Barras restantes: {barsRemaining}");
                    return;
                }
                else
                {
                    // Cooldown expirado, eliminar del diccionario
                    _cancelledOrdersCooldown.Remove(sourceStructureId);
                    _logger.Debug($"[TradeManager] ✅ Cooldown EXPIRADO para estructura {sourceStructureId}, orden permitida");
                }
            }
            
            // FILTRO 2: Verificar si ya existe una orden idéntica RECIENTE (margen estricto 0.01 + cooldown)
            // CRÍTICO: Incluir TODAS las órdenes (incluso cerradas) para evitar re-registrar la misma señal
            const double tol = 0.01; // Tolerancia muy estricta en ES (<< 1 tick)
            
            var lastSimilar = _trades
                .Where(t =>
                    t.Action == action &&
                    Math.Abs(t.Entry - entry) < tol &&
                    Math.Abs(t.SL - sl) < tol &&
                    Math.Abs(t.TP - tp) < tol)
                .OrderByDescending(t => t.EntryBar)
                .FirstOrDefault();

            if (lastSimilar != null)
            {
                int minBars = _config.MinBarsBetweenSameSignal; // p.ej. 12
                if (entryBar - lastSimilar.EntryBar < minBars)
                {
                    _logger.Debug($"[TradeManager] Señal duplicada en ventana ({entryBar - lastSimilar.EntryBar}/{minBars}) | Status={lastSimilar.Status} → ignorada");
                    return;
                }
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
                ExitBar = -1,
                ExitBarTime = DateTime.MinValue,
                ExitReason = null,
                TFDominante = tfDominante,
                SourceStructureId = sourceStructureId
            };

            _trades.Add(trade);
            _logger.Info($"[TradeManager] 🎯 ORDEN REGISTRADA: {action} LIMIT @ {entry:F2} | SL={sl:F2}, TP={tp:F2} | Bar={entryBar} | Estructura={sourceStructureId}");
            
            // Log to CSV
            _tradeLogger?.LogOrderRegistered(action, entry, sl, tp, entryBar, entryBarTime, sourceStructureId, _contractSize, _pointValue);
        }

        /// <summary>
        /// Actualiza el estado de todas las órdenes en la barra actual
        /// </summary>
        public void UpdateTrades(double currentHigh, double currentLow, int currentBar, DateTime currentBarTime, double currentPrice, 
                                 CoreEngine coreEngine, IBarDataProvider barData)
        {
            var activeTrades = _trades.Where(t => 
                t.Status == TradeStatus.PENDING || t.Status == TradeStatus.EXECUTED
            ).ToList();

            foreach (var trade in activeTrades)
            {
                if (trade.Status == TradeStatus.PENDING)
                {
                    // PASO 1: Verificar caducidad inteligente ANTES de verificar ejecución
                    if (CheckInvalidation(trade, currentPrice, currentBar, currentBarTime, coreEngine, barData))
                        continue; // La orden fue cancelada, pasar a la siguiente

                    // PASO 2: Verificar si el precio llegó al Entry
                    bool entryHit = false;
                    if (trade.Action == "BUY")
                        entryHit = currentLow <= trade.Entry;
                    else if (trade.Action == "SELL")
                        entryHit = currentHigh >= trade.Entry;

                    if (entryHit)
                    {
                        trade.Status = TradeStatus.EXECUTED;
                        trade.ExecutionBar = currentBar;
                        trade.ExecutionBarTime = currentBarTime;
                        _logger.Info($"[TradeManager] ✅ ORDEN EJECUTADA: {trade.Action} @ {trade.Entry:F2} en barra {currentBar}");
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
                        _logger.Info($"[TradeManager] 🟢 CERRADA POR TP: {trade.Action} @ {trade.Entry:F2} en barra {currentBar}");
                        
                        // Log to CSV
                        _tradeLogger?.LogOrderClosedTP(trade.Action, trade.Entry, trade.TP, trade.EntryBar, trade.EntryBarTime, currentBar, currentBarTime, _contractSize, _pointValue);
                    }
                }
            }
        }

        /// <summary>
        /// Verifica si una orden PENDING debe ser cancelada por invalidación estructural
        /// </summary>
        private bool CheckInvalidation(TradeRecord trade, double currentPrice, int currentBar, DateTime currentBarTime,
                                       CoreEngine coreEngine, IBarDataProvider barData)
        {
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
            if (CheckBOSContradictory(trade, coreEngine, barData, currentBar))
            {
                trade.Status = TradeStatus.CANCELLED;
                trade.ExitBar = currentBar;
                trade.ExitBarTime = currentBarTime;
                trade.ExitReason = "BOS_CONTRARY";
                _logger.Warning($"[TradeManager] ❌ ORDEN CANCELADA por BOS contradictorio: {trade.Action} @ {trade.Entry:F2}");
                
                // Log to CSV
                _tradeLogger?.LogOrderCancelled(trade.Action, trade.Entry, currentBar, currentBarTime, "BOS contradictorio");
                
                // Añadir estructura al cooldown
                AddToCooldown(trade.SourceStructureId, currentBar);
                return true;
            }

            // REGLA 3 (FAIL-SAFE): Caducidad por Tiempo/Distancia absoluta
            double atr = barData.GetATR(trade.TFDominante, currentBar, 14);
            double distanceToEntry = Math.Abs(currentPrice - trade.Entry);
            int barsWaiting = currentBar - trade.EntryBar;
            
            double maxBarsWaiting = 100; // ~5 horas en TF 5m, ~25 horas en TF 15m
            double maxAbsoluteDistance = 30.0 * atr; // 30 ATR de distancia absoluta

            if (barsWaiting > maxBarsWaiting)
            {
                trade.Status = TradeStatus.CANCELLED;
                trade.ExitBar = currentBar;
                trade.ExitBarTime = currentBarTime;
                trade.ExitReason = "EXPIRED_TIME";
                _logger.Warning($"[TradeManager] ❌ ORDEN EXPIRADA (Tiempo): {trade.Action} @ {trade.Entry:F2} | Barras esperando: {barsWaiting}");
                
                // Log to CSV
                _tradeLogger?.LogOrderExpired(trade.Action, trade.Entry, currentBar, currentBarTime, $"Tiempo: {barsWaiting} barras");
                
                // Añadir estructura al cooldown
                AddToCooldown(trade.SourceStructureId, currentBar);
                return true;
            }
            
            if (distanceToEntry > maxAbsoluteDistance)
            {
                trade.Status = TradeStatus.CANCELLED;
                trade.ExitBar = currentBar;
                trade.ExitBarTime = currentBarTime;
                trade.ExitReason = "EXPIRED_DISTANCE";
                _logger.Warning($"[TradeManager] ❌ ORDEN EXPIRADA (Distancia): {trade.Action} @ {trade.Entry:F2} | Distancia: {distanceToEntry:F2} > {maxAbsoluteDistance:F2}");
                
                // Log to CSV
                _tradeLogger?.LogOrderExpired(trade.Action, trade.Entry, currentBar, currentBarTime, $"Distancia: {distanceToEntry:F2} pts");
                
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
        /// </summary>
        private bool CheckBOSContradictory(TradeRecord trade, CoreEngine coreEngine, IBarDataProvider barData, int currentBar)
        {
            // V5.6.6: Sesgo único con cálculo directo EMA200@60 para cancelaciones si está habilitado
            string currentBias = coreEngine.CurrentMarketBias;
            if (_config.UseContextBiasForCancellations)
            {
                try
                {
                    int tf = 60; // 1H
                    int index60 = barData.GetCurrentBarIndex(tf);
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
                }
                catch { /* si falla, mantener currentMarketBias */ }
            }
            
            // Para BUY LIMIT, cancelar solo si el bias cambió a Bearish
            if (trade.Action == "BUY" && currentBias == "Bearish")
            {
                _logger.Warning($"[TradeManager] GlobalBias contradictorio: {trade.Action} @ {trade.Entry:F2} | Bias cambió a {currentBias}");
                _logger.Info($"[DIAGNOSTICO][TM] Cancel_BOS Action={trade.Action} Bias={currentBias}");
                return true;
            }

            // Para SELL LIMIT, cancelar solo si el bias cambió a Bullish
            if (trade.Action == "SELL" && currentBias == "Bullish")
            {
                _logger.Warning($"[TradeManager] GlobalBias contradictorio: {trade.Action} @ {trade.Entry:F2} | Bias cambió a {currentBias}");
                _logger.Info($"[DIAGNOSTICO][TM] Cancel_BOS Action={trade.Action} Bias={currentBias}");
                return true;
            }

            return false;
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
    }
}

