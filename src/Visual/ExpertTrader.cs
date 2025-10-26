// ============================================================================
// ExpertTrader.cs
// PinkButterfly CoreBrain - Indicador de Señales de Trading
// 
// Indicador NinjaScript que consume el DecisionEngine y visualiza:
// - Líneas de Entry, StopLoss, TakeProfit
// - Panel lateral con Bias y estado del mercado
// - Panel de órdenes pendientes
//
// Patrón: Observer/Consumer del DecisionEngine
// Diseño: Minimalista, enfocado en señales operativas
// ============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;

// Nota: LanguageOption ya está definido globalmente, no redefinir aquí

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// ExpertTrader - Indicador de Señales de Trading del PinkButterfly CoreBrain
    /// Consume DecisionEngine y dibuja Entry/SL/TP y panel de estado
    /// </summary>
    public class ExpertTrader : Indicator
    {
        #region Variables

        private CoreEngine _coreEngine;
        private DecisionEngine _decisionEngine;
        private IBarDataProvider _barDataProvider;
        private EngineConfig _config;
        private ILogger _logger;
        private FileLogger _fileLogger;
        private TradeLogger _tradeLogger;
        
        private TradeDecision _lastDecision;
        private List<HeatZone> _lastHeatZones;
        
        // ========================================================================
        // GESTOR DE OPERACIONES (TradeManager)
        // ========================================================================
        private TradeManager _tradeManager;
        
        // Tags para dibujo (para poder removerlos después)
        private const string TAG_ENTRY = "ENTRY_";
        private const string TAG_SL = "SL_";
        private const string TAG_TP = "TP_";
        private const string TAG_PANEL = "PANEL_";

        #endregion

        #region Properties (Configurables desde UI)

        [NinjaScriptProperty]
        [Display(Name = "Language / Idioma", Description = "Interface language / Idioma de la interfaz", Order = 0, GroupName = "General")]
        public LanguageOption Language { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Account Size", Description = "Tamaño de cuenta para calcular position size", Order = 1, GroupName = "Risk Management")]
        public double AccountSize { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Show Entry Lines", Description = "Mostrar líneas de Entry", Order = 2, GroupName = "Visual")]
        public bool ShowEntryLines { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Show SL/TP Lines", Description = "Mostrar líneas de SL/TP", Order = 3, GroupName = "Visual")]
        public bool ShowSLTPLines { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Show Panel", Description = "Mostrar panel lateral con información", Order = 4, GroupName = "Visual")]
        public bool ShowPanel { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Entry Line Width", Description = "Grosor de línea de Entry (1-5)", Order = 5, GroupName = "Visual")]
        [Range(1, 5)]
        public int EntryLineWidth { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "SL/TP Line Width", Description = "Grosor de líneas SL/TP (1-5)", Order = 6, GroupName = "Visual")]
        [Range(1, 5)]
        public int SLTPLineWidth { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Enable Fast Load (Solo DFM)", Description = "⚡ MODO RÁPIDO: Carga estructuras desde JSON (2-3 seg) y solo ejecuta DecisionEngine. Para calibrar parámetros del DFM (pesos, umbrales). REQUIERE brain_state.json previo.", Order = 7, GroupName = "Performance")]
        public bool EnableFastLoad { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Contratos por Operación", Description = "Número de contratos a operar (para cálculo de P&L)", Order = 8, GroupName = "Risk Management")]
        [Range(1, 100)]
        public int ContractSize { get; set; }

        #endregion

        #region NinjaScript Lifecycle

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = @"PinkButterfly ExpertTrader - Indicador de Señales de Trading";
                Name = "ExpertTrader";
                Calculate = Calculate.OnBarClose;
                IsOverlay = true;
                DisplayInDataBox = true;
                DrawOnPricePanel = true;
                DrawHorizontalGridLines = true;
                DrawVerticalGridLines = true;
                PaintPriceMarkers = true;
                ScaleJustification = NinjaTrader.Gui.Chart.ScaleJustification.Right;
                IsSuspendedWhileInactive = true;
                
                // Configurar output a Output Tab 2 (como el resto del sistema)
                PrintTo = PrintTo.OutputTab2;

                // Defaults
                Language = LanguageOption.Spanish;
                AccountSize = 100000;
                ShowEntryLines = true;
                ShowSLTPLines = true;
                ShowPanel = true;

                // Line widths (más gruesas para mejor visibilidad)
                EntryLineWidth = 3;
                SLTPLineWidth = 2;
                
                // Performance
                EnableFastLoad = false; // Por defecto desactivado (primera ejecución debe generar el JSON)
                
                // Risk Management
                ContractSize = 1; // 1 contrato por defecto
            }
            else if (State == State.Configure)
            {
                try
                {
                    Print("[ExpertTrader] State.Configure - Iniciando configuración MTF");
                    
                    var tempConfig = EngineConfig.LoadDefaults();
                    int chartTF = Bars.BarsPeriod.Value; // TF del gráfico principal
                    
                    Print($"[ExpertTrader] TF del gráfico principal: {chartTF}m");
                    Print($"[ExpertTrader] TFs configurados en Core: {string.Join(", ", tempConfig.TimeframesToUse)}");
                    
                    // Verificar si el TF del gráfico está en la configuración del Core
                    bool chartTFInCore = tempConfig.TimeframesToUse.Contains(chartTF);
                    
                    if (!chartTFInCore)
                    {
                        Print($"[ExpertTrader] ADVERTENCIA: TF {chartTF}m NO está en TimeframesToUse del Core");
                        Print($"[ExpertTrader] El gráfico se actualizará a {chartTF}m, pero el análisis usará los TFs configurados");
                    }
                    
                    // Agregar DataSeries para cada TF configurado (Multi-Timeframe)
                    // Solo añadir los que NO son el TF principal del gráfico
                    foreach (int tfMinutes in tempConfig.TimeframesToUse)
                    {
                        if (tfMinutes == chartTF)
                        {
                            Print($"[ExpertTrader] Saltando TF {tfMinutes}m (es el TF principal)");
                            continue;
                        }
                        
                        AddDataSeries(Instrument.FullName, new BarsPeriod 
                        { 
                            BarsPeriodType = BarsPeriodType.Minute, 
                            Value = tfMinutes 
                        });
                        Print($"[ExpertTrader] DataSeries agregada para TF {tfMinutes}m");
                    }
                    
                    Print("[ExpertTrader] State.Configure completado");
                }
                catch (Exception ex)
                {
                    Print($"[ExpertTrader] ERROR en State.Configure: {ex.Message}");
                    Print($"Stack: {ex.StackTrace}");
                }
            }
            else if (State == State.DataLoaded)
            {
                try
                {
                    Print("[ExpertTrader] State.DataLoaded - Iniciando inicialización");
                    
                    // Inicializar logger
                    _logger = new NinjaTraderLogger(this, LogLevel.Info);
                    Print("[ExpertTrader] Logger inicializado");

                    // Cargar configuración
                    _config = EngineConfig.LoadDefaults();
                    
                    // Aplicar Fast Load desde la UI
                    _config.EnableFastLoadFromJSON = EnableFastLoad;
                    
                    if (EnableFastLoad)
                    {
                        Print("═══════════════════════════════════════════════════════");
                        Print("⚡ FAST LOAD ACTIVADO (Solo DFM)");
                        Print("CoreEngine: Carga estructuras desde JSON (modo estático)");
                        Print("DecisionEngine: Se ejecutará normalmente barra por barra");
                        Print("Uso: Calibrar parámetros del DFM sin re-procesar CoreEngine");
                        Print("═══════════════════════════════════════════════════════");
                    }
                    
                    Print("[ExpertTrader] Configuración cargada");

                    // Crear provider
                    _barDataProvider = new NinjaTraderBarDataProvider(this);
                    Print("[ExpertTrader] BarDataProvider creado");

                    // Inicializar File Loggers
                    // Ruta: C:\Users\<USUARIO>\Documents\NinjaTrader 8\logs\
                    string userDataDir = NinjaTrader.Core.Globals.UserDataDir;
                    string logDirectory = System.IO.Path.Combine(userDataDir, "logs");
                    _fileLogger = new FileLogger(logDirectory, "backtest", _logger, true);
                    _tradeLogger = new TradeLogger(logDirectory, "trades", _logger, true);
                    Print($"[ExpertTrader] Loggers inicializados en: {logDirectory}");

                    // Inicializar CoreEngine
                    _coreEngine = new CoreEngine(_barDataProvider, _config, _fileLogger);
                    _coreEngine.Initialize();
                    Print("[ExpertTrader] CoreEngine inicializado");

                    // Inicializar DecisionEngine
                    _decisionEngine = new DecisionEngine(_config, _fileLogger);
                    Print("[ExpertTrader] DecisionEngine inicializado");

                    // Inicializar TradeManager con TradeLogger
                    double pointValue = Instrument.MasterInstrument.PointValue;
                    _tradeManager = new TradeManager(_config, _fileLogger, _tradeLogger, ContractSize, pointValue);
                    Print("[ExpertTrader] TradeManager inicializado");

                    // Inicializar variables
                    _lastHeatZones = new List<HeatZone>();
                    
                    // Inicializar tracking de progreso
                    int totalBars = Math.Min(BarsArray[0].Count, _config.BacktestBarsForAnalysis);
                    _coreEngine.StartProgressTracking(totalBars);
                    Print($"[ExpertTrader] ProgressTracker inicializado para {totalBars} barras");
                    
                    Print("[ExpertTrader] State.DataLoaded completado exitosamente");
                }
                catch (Exception ex)
                {
                    Print($"[ExpertTrader] ERROR en inicialización: {ex.Message}");
                    Print($"Stack: {ex.StackTrace}");
                }
            }
            else if (State == State.Terminated)
            {
                // Cleanup
                if (_coreEngine != null)
                {
                    // Finalizar tracking de progreso
                    _coreEngine.FinishProgressTracking();
                    
                    _coreEngine.Dispose();
                    _coreEngine = null;
                }
                
                // Cerrar loggers
                if (_tradeLogger != null)
                {
                    _tradeLogger.Close();
                    Print($"[ExpertTrader] TradeLogger cerrado: {_tradeLogger.GetCsvFilePath()}");
                }
                
                if (_fileLogger != null)
                {
                    _fileLogger.Close();
                    Print($"[ExpertTrader] FileLogger cerrado: {_fileLogger.GetLogFilePath()}");
                }
            }
        }

        protected override void OnBarUpdate()
        {
            try
            {
                // 1. Identificar qué TF se actualizó usando BarsInProgress
                int barsInProgressIndex = BarsInProgress;
                
                // 2. Obtener el TF en minutos del BarsArray que se actualizó
                BarsPeriod period = BarsArray[barsInProgressIndex].BarsPeriod;
                int tfMinutes = GetMinutesFromBarsPeriod(period);
                
                // 3. Obtener el índice de barra correcto para ESE TF
                int barIndex = CurrentBars[barsInProgressIndex];
                
                // 4. Verificar que este TF tenga suficientes datos (al menos 20 barras)
                if (barIndex < 20)
                    return;
                
                // 5. Control de carga histórica: solo procesar las últimas N barras
                int totalBars = BarsArray[barsInProgressIndex].Count;
                int barsToSkip = totalBars - _config.BacktestBarsForAnalysis;
                
                if (State == State.Historical && barIndex < barsToSkip)
                {
                    // Saltar barras antiguas en histórico para acelerar la carga
                    return;
                }
                
                // 6. Control de logging: solo loggear las últimas N barras del histórico
                bool enableLogging = (State == State.Realtime) || 
                                    (barIndex >= totalBars - _config.LoggingThresholdBars);
                
                if (barIndex == barsToSkip && State == State.Historical)
                {
                    Print($"[ExpertTrader] Iniciando procesamiento histórico desde barra {barIndex}/{totalBars} (TF {tfMinutes}m)");
                }
                
                // 7. CRÍTICO: Solo llamar a CoreEngine.OnBarClose si el TF está en TimeframesToUse
                // Esto protege la integridad del scoring y análisis MTF
                // NOTA: En modo Fast Load, OnBarClose() está bloqueado internamente (modo estático)
                if (_config.TimeframesToUse.Contains(tfMinutes))
                {
                    _coreEngine.OnBarClose(tfMinutes, barIndex);
                    
                    if (enableLogging)
                        _logger.Debug($"[ExpertTrader] OnBarClose({tfMinutes}m, bar {barIndex}) - BarsInProgress: {barsInProgressIndex}");
                }
                else if (barsInProgressIndex == 0)
                {
                    // Este es el TF del gráfico, pero NO está en TimeframesToUse
                    // Solo usarlo para dibujar, NO para análisis
                    if (enableLogging)
                        _logger.Debug($"[ExpertTrader] TF {tfMinutes}m (gráfico) - Solo dibujo, no análisis");
                }
                
                // FAST LOAD: Actualizar scores dinámicamente incluso en modo estático
                // Los scores de proximidad deben recalcularse en cada barra
                if (_config.EnableFastLoadFromJSON && _config.TimeframesToUse.Contains(tfMinutes))
                {
                    _coreEngine.UpdateScoresForFastLoad(tfMinutes, barIndex);
                }

                // 8. Solo en el TF principal (BarsInProgress == 0), generar decisión y dibujar
                // El análisis usa el TF más bajo de TimeframesToUse, no necesariamente el del gráfico
                if (BarsInProgress == 0 && barIndex >= 20)
                {
                    if (enableLogging)
                        _logger.Debug($"[ExpertTrader] Generando decisión para BarIndex: {barIndex}");
                    
                    // Usar el TF más bajo de TimeframesToUse como referencia para el análisis
                    int lowestTF = _config.TimeframesToUse.Min();
                    int analysisBarIndex = _barDataProvider.GetCurrentBarIndex(lowestTF);
                    
                    // Generar decisión con DecisionEngine usando el barIndex del TF de análisis
                    _lastDecision = _decisionEngine.GenerateDecision(_barDataProvider, _coreEngine, analysisBarIndex, AccountSize);

                    // LOG DETALLADO DE LA DECISIÓN (valores SL/TP)
                    if (_lastDecision != null && enableLogging)
                    {
                        double rr = _lastDecision.StopLoss > 0 ? Math.Abs(_lastDecision.TakeProfit - _lastDecision.Entry) / Math.Abs(_lastDecision.Entry - _lastDecision.StopLoss) : 0;
                        _logger.Debug($"[ExpertTrader] *** DECISIÓN *** Action={_lastDecision.Action}, Entry={_lastDecision.Entry:F2}, SL={_lastDecision.StopLoss:F2}, TP={_lastDecision.TakeProfit:F2}, Conf={_lastDecision.Confidence:F3}, R:R={rr:F2}");
                    }

                    // TRACKING DE OPERACIÓN ACTIVA
                    ProcessTradeTracking();

                    // Obtener HeatZones del snapshot (para el panel de información)
                    _lastHeatZones = GetTopHeatZones();

                    // Dibujar visualización
                    DrawVisualization();
                }
            }
            catch (Exception ex)
            {
                Print($"[ExpertTrader] ERROR en OnBarUpdate: {ex.Message}");
                Print($"Stack: {ex.StackTrace}");
                _logger.Error($"[ExpertTrader] Error en OnBarUpdate: {ex.Message}");
                _logger.Exception("OnBarUpdate exception", ex);
            }
        }

        /// <summary>
        /// Procesa el tracking de operaciones usando TradeManager
        /// </summary>
        private void ProcessTradeTracking()
        {
            if (_lastDecision == null)
                return;

            // PASO 1: Actualizar estado de todas las órdenes activas
            double currentPrice = Close[0];
            _tradeManager.UpdateTrades(High[0], Low[0], CurrentBar, currentPrice, _coreEngine, _barDataProvider);

            // PASO 2: Si llega una NUEVA señal BUY/SELL, registrarla
            bool isNewSignal = (_lastDecision.Action == "BUY" || _lastDecision.Action == "SELL");
            if (isNewSignal)
            {
                // Obtener el TF dominante de la última HeatZone (si existe)
                int tfDominante = _lastHeatZones != null && _lastHeatZones.Count > 0 
                    ? _lastHeatZones[0].TFDominante 
                    : _config.TimeframesToUse.Min();

                // Obtener el DominantStructureId de la decisión
                string sourceStructureId = _lastDecision.DominantStructureId ?? string.Empty;

                _tradeManager.RegisterTrade(
                    _lastDecision.Action,
                    _lastDecision.Entry,
                    _lastDecision.StopLoss,
                    _lastDecision.TakeProfit,
                    CurrentBar,
                    tfDominante,
                    sourceStructureId
                );
            }
        }

        /// <summary>
        /// Convierte un BarsPeriod a minutos
        /// </summary>
        private int GetMinutesFromBarsPeriod(BarsPeriod period)
        {
            switch (period.BarsPeriodType)
            {
                case BarsPeriodType.Minute:
                    return period.Value;
                case BarsPeriodType.Day:
                    return period.Value * 1440;
                case BarsPeriodType.Week:
                    return period.Value * 10080;
                case BarsPeriodType.Month:
                    return period.Value * 43200;
                default:
                    // Para otros tipos (Tick, Volume, Range, etc.), usar el TF principal
                    _logger.Warning($"[ExpertTrader] BarsPeriodType {period.BarsPeriodType} no soportado para MTF, usando TF principal");
                    return Bars.BarsPeriod.Value;
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Obtiene las top HeatZones desde el CoreEngine (para información del panel)
        /// </summary>
        private List<HeatZone> GetTopHeatZones()
        {
            var zones = new List<HeatZone>();

            try
            {
                // Obtener solo las zonas más relevantes (top 3) para mostrar en el panel
                var priorityTypes = new HashSet<string> { "OrderBlockInfo", "PointOfInterestInfo", "FairValueGapInfo", "LiquidityGrabInfo", "LiquidityVoidInfo" };
                
                var allValidStructures = new List<StructureBase>();
                
                foreach (int tf in _config.TimeframesToUse)
                {
                    var structures = _coreEngine.GetAllStructures(tf)
                        .Where(s => s.IsActive 
                                 && s.Score >= 0.6
                                 && priorityTypes.Contains(s.GetType().Name))
                        .ToList();
                    
                    allValidStructures.AddRange(structures);
                }

                var topStructures = allValidStructures
                    .OrderByDescending(s => s.Score)
                    .Take(3)
                    .ToList();

                foreach (var structure in topStructures)
                {
                    var zone = new HeatZone
                    {
                        Id = structure.Id,
                        Direction = GetStructureDirection(structure),
                        High = structure.High,
                        Low = structure.Low,
                        Score = structure.Score,
                        ConfluenceCount = 1,
                        DominantType = structure.Type,
                        TFDominante = structure.TF
                    };
                    zone.SourceStructureIds.Add(structure.Id);
                    zones.Add(zone);
                }
                
                _logger.Debug($"[ExpertTrader] {zones.Count} HeatZones obtenidas para panel");
            }
            catch (Exception ex)
            {
                _logger.Error($"[ExpertTrader] Error obteniendo HeatZones: {ex.Message}");
            }

            return zones;
        }

        /// <summary>
        /// Obtiene la dirección de una estructura (Bullish/Bearish/Neutral)
        /// </summary>
        private string GetStructureDirection(StructureBase structure)
        {
            if (structure is FVGInfo fvg)
                return fvg.Direction;
            else if (structure is OrderBlockInfo ob)
                return ob.Direction;
            else if (structure is StructureBreakInfo sb)
                return sb.Direction;
            else if (structure is LiquidityGrabInfo lg)
                return lg.DirectionalBias;
            else if (structure is SwingInfo swing)
                return swing.IsHigh ? "Bearish" : "Bullish";
            else
                return "Neutral";
        }

        #endregion

        #region Drawing Methods

        /// <summary>
        /// Dibuja toda la visualización: Entry/SL/TP, Panel
        /// </summary>
        private void DrawVisualization()
        {
            // 1. Dibujar líneas de Entry/SL/TP para TODAS las operaciones del historial
            if (ShowEntryLines)
                DrawEntryLine();
            
            if (ShowSLTPLines)
            {
                DrawSLLine();
                DrawTPLine();
            }

            // 2. Dibujar panel lateral
            if (ShowPanel)
                DrawPanel();
        }

        /// <summary>
        /// Dibuja las líneas de Entry para TODAS las operaciones
        /// - Operaciones EJECUTADAS/CERRADAS: desde ExecutionBar hasta ExitBar
        /// - Operaciones PENDIENTES: panel lateral fijo
        /// </summary>
        private void DrawEntryLine()
        {
            try
            {
                var allTrades = _tradeManager.GetAllTrades();
                var pendingTrades = allTrades.Where(t => t.Status == TradeStatus.PENDING).OrderBy(t => Math.Abs(t.Entry - Close[0])).ToList();
                
                // PARTE 1: Dibujar órdenes EJECUTADAS o CERRADAS (historial)
                foreach (var trade in allTrades.Where(t => t.Status != TradeStatus.PENDING))
                {
                    if (trade.Entry <= 0 || trade.ExecutionBar == -1)
                        continue;
                    
                    int startBarsAgo = CurrentBar - trade.ExecutionBar;
                    int endBarsAgo = trade.ExitBar > 0 ? CurrentBar - trade.ExitBar : 0;
                    
                    string tag = TAG_ENTRY + trade.Id;
                    
                    // RECTÁNGULO VERDE (TP zone) - Desde Entry hasta TP
                    Brush greenBrush = new SolidColorBrush(Colors.LimeGreen);
                    greenBrush.Opacity = 0.3;
                    greenBrush.Freeze();
                    
                    Draw.Rectangle(this, tag + "_TPRect", false,
                        startBarsAgo, trade.TP,
                        endBarsAgo, trade.Entry,
                        Brushes.Transparent, greenBrush, 30);
                    
                    // RECTÁNGULO ROJO (SL zone) - Desde Entry hasta SL
                    Brush redBrush = new SolidColorBrush(Colors.Red);
                    redBrush.Opacity = 0.3;
                    redBrush.Freeze();
                    
                    Draw.Rectangle(this, tag + "_SLRect", false,
                        startBarsAgo, trade.Entry,
                        endBarsAgo, trade.SL,
                        Brushes.Transparent, redBrush, 30);
                    
                    // Línea de Entry (blanca)
                    Draw.Line(this, tag, true, startBarsAgo, trade.Entry, endBarsAgo, trade.Entry, 
                        Brushes.White, DashStyleHelper.Solid, 3);
                    
                    // Etiqueta Entry (texto blanco)
                    string entryLabel = $"E: {trade.Entry:F2}";
                    Draw.Text(this, tag + "_LABEL", false, entryLabel,
                        startBarsAgo, trade.Entry + (4 * TickSize),
                        0, Brushes.White, new SimpleFont("Arial", 12), 
                        System.Windows.TextAlignment.Left, Brushes.Transparent, Brushes.Transparent, 0);
                }
                
                // PARTE 2: Dibujar órdenes PENDIENTES en panel lateral fijo (REPOSICIONADO)
                // CRÍTICO: Usar el precio de la última barra del gráfico, no la barra del loop histórico
                var currentPrice = Closes[0][0]; // Precio de cierre de la barra más reciente del TF principal
                var groupedPending = pendingTrades
                    .GroupBy(t => Math.Round(t.Entry, 2))
                    .Select(g => new { 
                        Entry = g.Key, 
                        Action = g.First().Action,
                        Count = g.Count(),
                        Distance = Math.Abs(g.Key - currentPrice)
                    })
                    .OrderBy(g => g.Distance)
                    .ToList();
                
                if (groupedPending.Count > 0)
                {
                    // Construir texto del panel
                    System.Text.StringBuilder panelText = new System.Text.StringBuilder();
                    panelText.AppendLine("═══ ÓRDENES PENDIENTES ═══");
                    panelText.AppendLine($"Última vela: {currentPrice:F2}");
                    panelText.AppendLine("─────────────────────────");
                    
                    foreach (var group in groupedPending)
                    {
                        string action = group.Action == "BUY" ? "BUY " : "SELL";
                        string countStr = group.Count > 1 ? $"({group.Count}x) " : "";
                        double dist = group.Entry - currentPrice;
                        string distStr = dist > 0 ? $"+{dist:F2}" : $"{dist:F2}";
                        
                        panelText.AppendLine($"{action} {countStr}@ {group.Entry:F2} [{distStr}]");
                    }
                    
                    // REPOSICIONADO: BottomRight para evitar solapamiento con panel principal
                    Draw.TextFixed(this, "PENDING_ORDERS_PANEL", panelText.ToString(),
                        TextPosition.BottomRight, 
                        Brushes.White, 
                        new SimpleFont("Courier New", 10), 
                        Brushes.Transparent, 
                        Brushes.DarkSlateGray, 
                        100);
                    
                    // Dibujar líneas horizontales para cada orden (opcional, para referencia visual)
                    foreach (var group in groupedPending)
                    {
                        string tag = TAG_ENTRY + "PENDING_LINE_" + group.Entry.ToString("F2").Replace(".", "_");
                        Brush brush = group.Action == "BUY" ? Brushes.LimeGreen : Brushes.Red;
                        
                        // Línea horizontal corta en las últimas 5 barras
                        Draw.Line(this, tag, true, 5, group.Entry, 0, group.Entry, 
                            brush, DashStyleHelper.Dash, 1);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"[ExpertTrader] Error dibujando Entry: {ex.Message}");
            }
        }

        /// <summary>
        /// Dibuja la línea de StopLoss (desde barra de entrada hasta barra actual o salida)
        /// </summary>
        private void DrawSLLine()
        {
            try
            {
                var allTrades = _tradeManager.GetAllTrades();
                
                // Solo dibujar SL para órdenes EJECUTADAS (no pendientes)
                foreach (var trade in allTrades.Where(t => t.Status != TradeStatus.PENDING && t.Status != TradeStatus.CANCELLED))
                {
                    if (trade.SL <= 0 || trade.ExecutionBar == -1)
                        continue;
                    
                    int startBarsAgo = CurrentBar - trade.ExecutionBar;
                    int endBarsAgo = trade.ExitBar > 0 ? CurrentBar - trade.ExitBar : 0;
                    
                    string tag = TAG_SL + trade.Id;
                    string label = $"SL: {trade.SL:F2}";
                    
                    // Línea SL (roja)
                    Draw.Line(this, tag, true, startBarsAgo, trade.SL, endBarsAgo, trade.SL, 
                        Brushes.Red, DashStyleHelper.Dash, 2);
                    
                    // Etiqueta SL (texto rojo)
                    Draw.Text(this, tag + "_LABEL", false, label,
                        startBarsAgo, trade.SL - (4 * TickSize),
                        0, Brushes.Red, new SimpleFont("Arial", 11), 
                        System.Windows.TextAlignment.Left, Brushes.Transparent, Brushes.Transparent, 0);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"[ExpertTrader] Error dibujando SL: {ex.Message}");
            }
        }

        /// <summary>
        /// Dibuja las líneas de TakeProfit para TODAS las operaciones ejecutadas
        /// </summary>
        private void DrawTPLine()
        {
            try
            {
                var allTrades = _tradeManager.GetAllTrades();
                
                // Solo dibujar TP para órdenes EJECUTADAS (no pendientes)
                foreach (var trade in allTrades.Where(t => t.Status != TradeStatus.PENDING && t.Status != TradeStatus.CANCELLED))
                {
                    if (trade.TP <= 0 || trade.ExecutionBar == -1)
                        continue;
                    
                    int startBarsAgo = CurrentBar - trade.ExecutionBar;
                    int endBarsAgo = trade.ExitBar > 0 ? CurrentBar - trade.ExitBar : 0;
                    
                    string tag = TAG_TP + trade.Id;
                    string label = $"TP: {trade.TP:F2}";
                    
                    // Línea TP (verde)
                    Draw.Line(this, tag, true, startBarsAgo, trade.TP, endBarsAgo, trade.TP, 
                        Brushes.LimeGreen, DashStyleHelper.Dash, 2);
                    
                    // Etiqueta TP (texto verde)
                    Draw.Text(this, tag + "_LABEL", false, label,
                        startBarsAgo, trade.TP + (4 * TickSize),
                        0, Brushes.LimeGreen, new SimpleFont("Arial", 11), 
                        System.Windows.TextAlignment.Left, Brushes.Transparent, Brushes.Transparent, 0);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"[ExpertTrader] Error dibujando TP: {ex.Message}");
            }
        }

        /// <summary>
        /// Dibuja el panel lateral con información del mercado y la decisión
        /// </summary>
        private void DrawPanel()
        {
            try
            {
                bool isSpanish = Language == LanguageOption.Spanish;
                double pointValue = Instrument.MasterInstrument.PointValue;
                
                // ================================================================
                // PANEL 1: PRÓXIMA OPERACIÓN
                // ================================================================
                var panel1 = new System.Text.StringBuilder();
                panel1.AppendLine("PRÓXIMA OPERACIÓN");
                panel1.AppendLine("─────────────────────────");
                
                // Obtener la próxima operación pendiente (la más cercana al precio)
                var pendingTrades = _tradeManager.GetAllTrades()
                    .Where(t => t.Status == TradeStatus.PENDING)
                    .OrderBy(t => Math.Abs(t.Entry - Close[0]))
                    .ToList();
                
                if (pendingTrades.Count > 0)
                {
                    var nextTrade = pendingTrades.First();
                    
                    // Sesgo (Alcista/Neutral/Bajista)
                    string bias = _coreEngine.CurrentMarketBias;
                    string biasES = bias == "Bullish" ? "Alcista" : bias == "Bearish" ? "Bajista" : "Neutral";
                    panel1.AppendLine($"Sesgo: {biasES}");
                    
                    // Acción (BUY en verde, SELL en rojo) - usaremos color en el texto completo
                    string action = nextTrade.Action;
                    panel1.AppendLine($"Acción: {action}");
                    
                    // Confianza
                    double confidence = _lastDecision != null ? _lastDecision.Confidence : 0;
                    panel1.AppendLine($"Confianza: {(confidence * 100):F0}%");
                    
                    // Entrada
                    panel1.AppendLine($"Entrada: {nextTrade.Entry:F2}");
                    
                    // TP con puntos
                    double tpPoints = Math.Abs(nextTrade.TP - nextTrade.Entry);
                    panel1.AppendLine($"TP: {nextTrade.TP:F2} - {tpPoints:F2} pts");
                    
                    // SL con puntos
                    double slPoints = Math.Abs(nextTrade.Entry - nextTrade.SL);
                    panel1.AppendLine($"SL: {nextTrade.SL:F2} - {slPoints:F2} pts");
                    
                    // Ganancia en $
                    double ganancia = tpPoints * pointValue * ContractSize;
                    panel1.AppendLine($"Ganancia: ${ganancia:F2}");
                    
                    // Pérdida en $
                    double perdida = slPoints * pointValue * ContractSize;
                    panel1.AppendLine($"Pérdida: ${perdida:F2}");
                }
                else
                {
                    // No hay operaciones pendientes
                    string bias = _coreEngine.CurrentMarketBias;
                    string biasES = bias == "Bullish" ? "Alcista" : bias == "Bearish" ? "Bajista" : "Neutral";
                    panel1.AppendLine($"Sesgo: {biasES}");
                    panel1.AppendLine();
                    panel1.AppendLine("No hay señales pendientes");
                }
                
                // Dibujar Panel 1 en TopRight
                Draw.TextFixed(this, TAG_PANEL + "TRADE", panel1.ToString(),
                    TextPosition.TopRight,
                    Brushes.White,
                    new Gui.Tools.SimpleFont("Courier New", 10),
                    Brushes.Transparent,
                    Brushes.DarkSlateGray,
                    80);
                
                // ================================================================
                // PANEL 2: DATOS DE SESIÓN
                // ================================================================
                var panel2 = new System.Text.StringBuilder();
                panel2.AppendLine("DATOS DE SESIÓN");
                panel2.AppendLine("─────────────────────────");
                
                // Obtener estadísticas del TradeManager
                var allTrades = _tradeManager.GetAllTrades();
                var executedTrades = allTrades.Where(t => t.Status == TradeStatus.TP_HIT || t.Status == TradeStatus.SL_HIT).ToList();
                var pendingCount = allTrades.Count(t => t.Status == TradeStatus.PENDING);
                
                int totalExecuted = executedTrades.Count;
                int wins = executedTrades.Count(t => t.Status == TradeStatus.TP_HIT);
                int losses = executedTrades.Count(t => t.Status == TradeStatus.SL_HIT);
                double winRate = totalExecuted > 0 ? (double)wins / totalExecuted * 100 : 0;
                
                // Calcular beneficios y pérdidas en $
                double totalBeneficios = 0;
                double totalPerdidas = 0;
                
                foreach (var trade in executedTrades)
                {
                    if (trade.Status == TradeStatus.TP_HIT)
                    {
                        double points = Math.Abs(trade.TP - trade.Entry);
                        totalBeneficios += points * pointValue * ContractSize;
                    }
                    else if (trade.Status == TradeStatus.SL_HIT)
                    {
                        double points = Math.Abs(trade.Entry - trade.SL);
                        totalPerdidas += points * pointValue * ContractSize;
                    }
                }
                
                double resultado = totalBeneficios - totalPerdidas;
                
                // Construir texto del panel
                panel2.AppendLine($"Ejecutadas: {totalExecuted}");
                panel2.AppendLine($"Pendientes: {pendingCount}");
                panel2.AppendLine($"Win Rate: {winRate:F0}%");
                panel2.AppendLine($"Total Beneficio: ${totalBeneficios:F2}");
                panel2.AppendLine($"Total Pérdidas: ${totalPerdidas:F2}");
                panel2.AppendLine($"RESULTADO: ${resultado:F2}");
                
                // Dibujar Panel 2 debajo del Panel 1
                // Nota: NinjaTrader no permite posicionar libremente, usaremos BottomRight
                Draw.TextFixed(this, TAG_PANEL + "STATS", panel2.ToString(),
                    TextPosition.TopLeft,
                    Brushes.White,
                    new Gui.Tools.SimpleFont("Courier New", 10),
                    Brushes.Transparent,
                    Brushes.DarkSlateGray,
                    80);
            }
            catch (Exception ex)
            {
                _logger.Error($"[ExpertTrader] Error dibujando Panel: {ex.Message}");
            }
        }

        #endregion
    }
}

