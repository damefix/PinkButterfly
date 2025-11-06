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
        public DateTime ExecutionBarTime { get; set; } // Timestamp de ejecución (V5.7e)
        public int ExitBar { get; set; }            // Barra donde se cerró/canceló
        public DateTime ExitBarTime { get; set; }   // Timestamp de la barra de salida
        public string ExitReason { get; set; }      // "SL", "TP", "BOS_CONTRARY", "PRICE_DEPARTED", etc.
        public int TFDominante { get; set; }        // TF dominante de la HeatZone
        public string SourceStructureId { get; set; } // ID de la estructura dominante que generó esta orden
        public double RegistrationPrice { get; set; } // Close cuando se registró la orden (para determinar LIMIT vs STOP)
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

        /// <summary>
        /// Registra una nueva orden Limit
        /// </summary>
        public void RegisterTrade(string action, double entry, double sl, double tp, int entryBar, DateTime entryBarTime, int tfDominante, string sourceStructureId, double currentPrice)
        {
            // FILTRO 1: Verificar cooldown (estructura cancelada recientemente)
            if (!string.IsNullOrEmpty(sourceStructureId) && _cancelledOrdersCooldown.ContainsKey(sourceStructureId))
            {
                int barExpiration = _cancelledOrdersCooldown[sourceStructureId];
                if (entryBar < barExpiration)
                {
                    int barsRemaining = barExpiration - entryBar;
                    _logger.Info($"[TRADE][DEDUP] COOLDOWN Zone={sourceStructureId} Action={action} Key={entry:F2}/{sl:F2}/{tp:F2} DomTF={tfDominante} CurrentBar={entryBar} BarsRemain={barsRemaining} UntilBar={barExpiration}");
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
            var identicalCandidates = _trades.Where(t =>
                (t.Status == TradeStatus.PENDING || t.Status == TradeStatus.EXECUTED) &&
                t.Action == action &&
                Math.Abs(t.Entry - entry) < 0.5 &&
                Math.Abs(t.SL - sl) < 0.5 &&
                Math.Abs(t.TP - tp) < 0.5
            );
            bool hasIdentical = identicalCandidates.Any();

            if (hasIdentical)
            {
                var lastSimilar = identicalCandidates.OrderByDescending(t => t.EntryBar).FirstOrDefault();
                int lastBar = lastSimilar != null ? lastSimilar.EntryBar : -1;
                int deltaBars = lastSimilar != null ? (entryBar - lastBar) : -1;
                string lastId = lastSimilar != null ? lastSimilar.Id : "";
                double keyE = Math.Round(entry, 2);
                double keyS = Math.Round(sl, 2);
                double keyT = Math.Round(tp, 2);

                int minBars = Math.Max(0, _config.MinBarsBetweenSameSignal);
                if (deltaBars >= 0 && deltaBars < minBars)
                {
                    _logger.Info($"[TRADE][DEDUP] IDENTICAL Zone={sourceStructureId} Action={action} Key={keyE:F2}/{keyS:F2}/{keyT:F2} DomTF={tfDominante} LastSimilar={lastId} LastBar={lastBar} CurrentBar={entryBar} DeltaBars={deltaBars} Tolerance=0.50");
                    return;
                }
                // Si ha pasado el cooldown, permitimos re-registrar sin return
            }
            
            // FILTRO 3: Verificar límite de operaciones concurrentes (V5.7d)
            int activeCount = _trades.Count(t => 
                t.Status == TradeStatus.PENDING || t.Status == TradeStatus.EXECUTED
            );
            
            if (activeCount >= _config.MaxConcurrentTrades)
            {
                _logger.Info($"[TRADE][SKIP] CONCURRENCY_LIMIT: Action={action} Entry={entry:F2} SL={sl:F2} TP={tp:F2} Active={activeCount}/{_config.MaxConcurrentTrades}");
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
                RegistrationPrice = currentPrice  // Guardar precio de registro para determinar LIMIT vs STOP
            };

            _trades.Add(trade);
            _logger.Info($"[TradeManager] 🎯 ORDEN REGISTRADA: {action} LIMIT @ {entry:F2} | SL={sl:F2}, TP={tp:F2} | Bar={entryBar} | Estructura={sourceStructureId}");
            
            // Log to CSV
            _tradeLogger?.LogOrderRegistered(action, entry, sl, tp, entryBar, entryBarTime, sourceStructureId, _contractSize, _pointValue);
        }

        /// <summary>
        /// Actualiza el estado de todas las órdenes en la barra actual
        /// V6.0i.5: Añadido parámetro currentRegime para aplicar gracia BOS en HighVol
        /// </summary>
        public void UpdateTrades(double currentHigh, double currentLow, int currentBar, DateTime currentBarTime, double currentPrice, 
                                 CoreEngine coreEngine, IBarDataProvider barData, string currentRegime = "Normal")
        {
            var activeTrades = _trades.Where(t => 
                t.Status == TradeStatus.PENDING || t.Status == TradeStatus.EXECUTED
            ).ToList();

            foreach (var trade in activeTrades)
            {
                if (trade.Status == TradeStatus.PENDING)
                {
                    // PASO 1: Verificar caducidad inteligente ANTES de verificar ejecución
                    if (CheckInvalidation(trade, currentPrice, currentBar, currentBarTime, coreEngine, barData, currentRegime))
                        continue; // La orden fue cancelada, pasar a la siguiente

                    // PASO 2: Determinar tipo de orden (LIMIT vs STOP) según precio de registro
                    bool isBuyLimit = (trade.Action == "BUY" && trade.RegistrationPrice > trade.Entry);
                    bool isSellLimit = (trade.Action == "SELL" && trade.RegistrationPrice < trade.Entry);
                    
                    string orderType = trade.Action == "BUY" 
                        ? (isBuyLimit ? "BUY LIMIT" : "BUY STOP")
                        : (isSellLimit ? "SELL LIMIT" : "SELL STOP");

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
                        trade.Status = TradeStatus.EXECUTED;
                        trade.ExecutionBar = currentBar;
                        trade.ExecutionBarTime = currentBarTime; // V5.7e
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
        /// V6.0i.5: Añadido parámetro currentRegime para aplicar gracia BOS en HighVol
        /// </summary>
        private bool CheckInvalidation(TradeRecord trade, double currentPrice, int currentBar, DateTime currentBarTime,
                                       CoreEngine coreEngine, IBarDataProvider barData, string currentRegime)
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
            if (CheckBOSContradictory(trade, coreEngine, barData, currentBarTime, currentRegime))
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

            // ========================================================================
            // REGLA 3 (V6.0i.6): PENDING STALENESS - Tiempo y Distancia adaptativa
            // ========================================================================
            
            // Calcular barras transcurridas en TF decisión
            int tf = _config.DecisionTimeframeMinutes;
            int currentIdx = barData.GetBarIndexFromTime(tf, currentBarTime);
            int entryIdx   = barData.GetBarIndexFromTime(tf, trade.EntryBarTime);
            int barsWaiting = Math.Max(0, currentIdx - entryIdx);
            
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
                _logger.Warning($"[TradeManager][PENDING_STALE_TIME] Trade={trade.Id} {trade.Action} @ {trade.Entry:F2} Regime={currentRegime} Waiting={barsWaiting}>{maxBarsToFill} → CANCEL");
                
                // Log to CSV
                _tradeLogger?.LogOrderExpired(trade.Action, trade.Entry, currentBar, currentBarTime, $"STALE_TIME: {barsWaiting}>{maxBarsToFill}bars");
                
                // Añadir estructura al cooldown
                AddToCooldown(trade.SourceStructureId, currentBar);
                return true;
            }
            
            // ========================================================================
            // 3B: Staleness por DISTANCIA (se aleja del entry en ATR60)
            // V6.0i.6b: Umbral fijo adaptativo por régimen (V6.0i.6c curva revertida)
            // ========================================================================
            double atr60 = barData.GetATR(60, barData.GetBarIndexFromTime(60, currentBarTime), 14);
            double distanceToEntry = Math.Abs(currentPrice - trade.Entry);
            double distanceATR = (atr60 > 0) ? (distanceToEntry / atr60) : 999.0;
            
            if (distanceATR > maxDistanceATR_Cancel)
            {
                trade.Status = TradeStatus.CANCELLED;
                trade.ExitBar = currentBar;
                trade.ExitBarTime = currentBarTime;
                trade.ExitReason = "PENDING_STALE_DIST";
                _logger.Warning($"[TradeManager][PENDING_STALE_DIST] Trade={trade.Id} {trade.Action} @ {trade.Entry:F2} Regime={currentRegime} Dist={distanceATR:F2}ATR>{maxDistanceATR_Cancel:F2}ATR → CANCEL");
                
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
        private bool CheckBOSContradictory(TradeRecord trade, CoreEngine coreEngine, IBarDataProvider barData, DateTime currentBarTime, string currentRegime)
        {
            // V5.6.6: Sesgo único con cálculo directo EMA200@60 para cancelaciones si está habilitado
            string currentBias = coreEngine.CurrentMarketBias;
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
    }
}

