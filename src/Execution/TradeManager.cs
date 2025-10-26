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
        public double Entry { get; set; }
        public double SL { get; set; }
        public double TP { get; set; }
        public string Action { get; set; }          // "BUY" o "SELL"
        public TradeStatus Status { get; set; }
        public int ExecutionBar { get; set; }       // Barra donde se ejecutó (si Status >= EXECUTED)
        public int ExitBar { get; set; }            // Barra donde se cerró/canceló
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
        public void RegisterTrade(string action, double entry, double sl, double tp, int entryBar, int tfDominante, string sourceStructureId)
        {
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
            
            // FILTRO 2: Verificar si ya existe una orden idéntica activa
            bool hasIdentical = _trades.Any(t =>
                (t.Status == TradeStatus.PENDING || t.Status == TradeStatus.EXECUTED) &&
                t.Action == action &&
                Math.Abs(t.Entry - entry) < 0.5 &&
                Math.Abs(t.SL - sl) < 0.5 &&
                Math.Abs(t.TP - tp) < 0.5
            );

            if (hasIdentical)
            {
                _logger.Debug($"[TradeManager] ⚠ Orden duplicada rechazada: {action} @ {entry:F2}");
                return;
            }

            var trade = new TradeRecord
            {
                Id = Guid.NewGuid().ToString(),
                EntryBar = entryBar,
                Entry = entry,
                SL = sl,
                TP = tp,
                Action = action,
                Status = TradeStatus.PENDING,
                ExecutionBar = -1,
                ExitBar = -1,
                ExitReason = null,
                TFDominante = tfDominante,
                SourceStructureId = sourceStructureId
            };

            _trades.Add(trade);
            _logger.Info($"[TradeManager] 🎯 ORDEN REGISTRADA: {action} LIMIT @ {entry:F2} | SL={sl:F2}, TP={tp:F2} | Bar={entryBar} | Estructura={sourceStructureId}");
            
            // Log to CSV
            _tradeLogger?.LogOrderRegistered(action, entry, sl, tp, entryBar, sourceStructureId, _contractSize, _pointValue);
        }

        /// <summary>
        /// Actualiza el estado de todas las órdenes en la barra actual
        /// </summary>
        public void UpdateTrades(double currentHigh, double currentLow, int currentBar, double currentPrice, 
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
                    if (CheckInvalidation(trade, currentPrice, currentBar, coreEngine, barData))
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
                        trade.ExitReason = "SL";
                        _logger.Info($"[TradeManager] 🔴 CERRADA POR SL: {trade.Action} @ {trade.Entry:F2} en barra {currentBar}");
                        
                        // Log to CSV
                        _tradeLogger?.LogOrderClosedSL(trade.Action, trade.Entry, trade.SL, trade.EntryBar, currentBar, _contractSize, _pointValue);
                    }
                    else if (hitTP)
                    {
                        trade.Status = TradeStatus.TP_HIT;
                        trade.ExitBar = currentBar;
                        trade.ExitReason = "TP";
                        _logger.Info($"[TradeManager] 🟢 CERRADA POR TP: {trade.Action} @ {trade.Entry:F2} en barra {currentBar}");
                        
                        // Log to CSV
                        _tradeLogger?.LogOrderClosedTP(trade.Action, trade.Entry, trade.TP, trade.EntryBar, currentBar, _contractSize, _pointValue);
                    }
                }
            }
        }

        /// <summary>
        /// Verifica si una orden PENDING debe ser cancelada por invalidación estructural
        /// </summary>
        private bool CheckInvalidation(TradeRecord trade, double currentPrice, int currentBar, 
                                       CoreEngine coreEngine, IBarDataProvider barData)
        {
            // REGLA 1 (PRIORITARIA): Caducidad por Invalidación Estructural
            // La estructura que generó la orden ya no existe, está inactiva, o su Score decayó
            if (!string.IsNullOrEmpty(trade.SourceStructureId))
            {
                var sourceStructure = coreEngine.GetStructureById(trade.SourceStructureId);
                
                if (sourceStructure == null || !sourceStructure.IsActive || sourceStructure.Score < _config.MinConfidenceForWait)
                {
                    trade.Status = TradeStatus.CANCELLED;
                    trade.ExitBar = currentBar;
                    trade.ExitReason = "STRUCTURAL_INVALIDATION";
                    
                    string reason = sourceStructure == null ? "estructura no existe" 
                        : !sourceStructure.IsActive ? "estructura inactiva" 
                        : $"score decayó a {sourceStructure.Score:F2}";
                    
                    _logger.Warning($"[TradeManager] ❌ ORDEN EXPIRADA (Estructural): {trade.Action} @ {trade.Entry:F2} | Razón: {reason}");
                    
                    // Log to CSV
                    _tradeLogger?.LogOrderExpired(trade.Action, trade.Entry, currentBar, reason);
                    
                    // Añadir estructura al cooldown
                    AddToCooldown(trade.SourceStructureId, currentBar);
                    return true;
                }
            }

            // REGLA 2: Caducidad por BOS/CHoCH contradictorio
            if (CheckBOSContradictory(trade, coreEngine))
            {
                trade.Status = TradeStatus.CANCELLED;
                trade.ExitBar = currentBar;
                trade.ExitReason = "BOS_CONTRARY";
                _logger.Warning($"[TradeManager] ❌ ORDEN CANCELADA por BOS contradictorio: {trade.Action} @ {trade.Entry:F2}");
                
                // Log to CSV
                _tradeLogger?.LogOrderCancelled(trade.Action, trade.Entry, currentBar, "BOS contradictorio");
                
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
                trade.ExitReason = "EXPIRED_TIME";
                _logger.Warning($"[TradeManager] ❌ ORDEN EXPIRADA (Tiempo): {trade.Action} @ {trade.Entry:F2} | Barras esperando: {barsWaiting}");
                
                // Log to CSV
                _tradeLogger?.LogOrderExpired(trade.Action, trade.Entry, currentBar, $"Tiempo: {barsWaiting} barras");
                
                // Añadir estructura al cooldown
                AddToCooldown(trade.SourceStructureId, currentBar);
                return true;
            }
            
            if (distanceToEntry > maxAbsoluteDistance)
            {
                trade.Status = TradeStatus.CANCELLED;
                trade.ExitBar = currentBar;
                trade.ExitReason = "EXPIRED_DISTANCE";
                _logger.Warning($"[TradeManager] ❌ ORDEN EXPIRADA (Distancia): {trade.Action} @ {trade.Entry:F2} | Distancia: {distanceToEntry:F2} > {maxAbsoluteDistance:F2}");
                
                // Log to CSV
                _tradeLogger?.LogOrderExpired(trade.Action, trade.Entry, currentBar, $"Distancia: {distanceToEntry:F2} pts");
                
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
        private bool CheckBOSContradictory(TradeRecord trade, CoreEngine coreEngine)
        {
            // CALIBRACIÓN V4: Solo cancelar si el GlobalBias (del ContextManager) CAMBIA de dirección
            // Esto evita cancelar trades rentables por micro-BOS que son ruido de mercado
            
            // Obtener el GlobalBias actual del CoreEngine
            string currentBias = coreEngine.CurrentMarketBias;
            
            // Para BUY LIMIT, cancelar solo si el bias cambió a Bearish
            if (trade.Action == "BUY" && currentBias == "Bearish")
            {
                _logger.Warning($"[TradeManager] GlobalBias contradictorio: {trade.Action} @ {trade.Entry:F2} | Bias cambió a {currentBias}");
                return true;
            }

            // Para SELL LIMIT, cancelar solo si el bias cambió a Bullish
            if (trade.Action == "SELL" && currentBias == "Bullish")
            {
                _logger.Warning($"[TradeManager] GlobalBias contradictorio: {trade.Action} @ {trade.Entry:F2} | Bias cambió a {currentBias}");
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

