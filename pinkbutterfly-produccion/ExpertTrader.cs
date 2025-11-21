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
using System.Diagnostics;
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

// Enum para selección de idioma
public enum LanguageOption
{
    English,
    Spanish
}

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
        private System.Windows.Media.ImageSource _logoImage;
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

        // Índice del TF de decisión en BarsArray
        private int _decisionTFIndex;
        // Evitar múltiples decisiones en la misma barra del TF de decisión
        private int _lastProcessedBarOfDecision = -1;

        // [DIAG] Acumuladores de rendimiento (solo TF de decisión)
        private int _diagBarCounter;
        private double _accumOnBarCloseMs;

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

        [NinjaScriptProperty]
        [Display(Name = "Enable Output Logging", Description = "Activar logs en Output window de NinjaTrader", Order = 10, GroupName = "Logging")]
        public bool EnableOutputLogging { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Enable File Logging", Description = "Activar logs en archivo de disco (puede crecer mucho en tiempo real)", Order = 11, GroupName = "Logging")]
        public bool EnableFileLogging { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Enable Trade CSV", Description = "Activar registro de operaciones en archivo CSV", Order = 12, GroupName = "Logging")]
        public bool EnableTradeCSV { get; set; }

        #endregion

        #region NinjaScript Lifecycle

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = @"PinkButterfly ExpertTrader - Indicador de Señales de Trading";
                Name = "ExpertTrader";
                Calculate = Calculate.OnEachTick;  // Actualizar en cada tick para tiempo real
                IsOverlay = true;
                DisplayInDataBox = true;
                DrawOnPricePanel = true;
                DrawHorizontalGridLines = true;
                DrawVerticalGridLines = true;
                PaintPriceMarkers = true;
                ScaleJustification = NinjaTrader.Gui.Chart.ScaleJustification.Right;
                IsSuspendedWhileInactive = false;  // Continuar actualizando en tiempo real
                
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
                
                // Logging (por defecto TODO ACTIVADO para mantener comportamiento actual)
                EnableOutputLogging = true;
                EnableFileLogging = true;
                EnableTradeCSV = true;
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
                    
                    // Inicializar logger base (Output window)
                    if (EnableOutputLogging)
                    {
                        _logger = new NinjaTraderLogger(this, LogLevel.Info);
                        Print("[ExpertTrader] ✅ Output logging ACTIVADO");
                    }
                    else
                    {
                        _logger = new SilentLogger();
                        Print("[ExpertTrader] ⚠️ Output logging DESACTIVADO");
                    }

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
                    // Ruta: C:\Users\<USUARIO>\Documents\NinjaTrader 8\PinkButterfly\logs\
                    string userDataDir = NinjaTrader.Core.Globals.UserDataDir;
                    string logDirectory = System.IO.Path.Combine(userDataDir, "PinkButterfly", "logs");
                    
                    // File Logger (archivo de log)
                    if (EnableFileLogging)
                    {
                        _fileLogger = new FileLogger(logDirectory, "backtest", _logger, true);
                        Print($"[ExpertTrader] ✅ File logging ACTIVADO: {logDirectory}");
                    }
                    else
                    {
                        _fileLogger = new FileLogger(logDirectory, "backtest", _logger, false);
                        Print("[ExpertTrader] ⚠️ File logging DESACTIVADO (no se escribirá a disco)");
                    }
                    
                    // Trade Logger (CSV de operaciones)
                    if (EnableTradeCSV)
                    {
                        _tradeLogger = new TradeLogger(logDirectory, "trades", _logger, true);
                        Print("[ExpertTrader] ✅ Trade CSV ACTIVADO");
                    }
                    else
                    {
                        _tradeLogger = new TradeLogger(logDirectory, "trades", _logger, false);
                        Print("[ExpertTrader] ⚠️ Trade CSV DESACTIVADO (no se registrarán operaciones)");
                    }

                    // Inicializar CoreEngine
                    _coreEngine = new CoreEngine(_barDataProvider, _config, _fileLogger);
                    _coreEngine.Initialize();
                    Print("[ExpertTrader] CoreEngine inicializado");

                    // Inicializar DecisionEngine
                    _decisionEngine = new DecisionEngine(_config, _fileLogger);
                    Print("[ExpertTrader] DecisionEngine inicializado");
                    
                    // Inyectar DecisionEngine al CoreEngine para replay de decisiones históricas
                    _coreEngine.SetDecisionEngine(_decisionEngine, AccountSize);
                    Print($"[ExpertTrader] DecisionEngine inyectado al CoreEngine (AccountSize={AccountSize})");

                    // Inicializar TradeManager con TradeLogger
                    double pointValue = Instrument.MasterInstrument.PointValue;
                    _tradeManager = new TradeManager(_config, _fileLogger, _tradeLogger, ContractSize, pointValue);
                    Print("[ExpertTrader] TradeManager inicializado");
                    
                    // Inyectar TradeManager al CoreEngine para entrega de decisiones durante replay
                    _coreEngine.SetTradeManager(_tradeManager);
                    Print("[ExpertTrader] TradeManager inyectado al CoreEngine");

                    // Inicializar variables
                    _lastHeatZones = new List<HeatZone>();
                    
                    // Cargar logo
                    try
                    {
                        string logoPath = @"C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\images\pinkbutterfly.png";
                        if (System.IO.File.Exists(logoPath))
                        {
                            _logoImage = new System.Windows.Media.Imaging.BitmapImage(new Uri(logoPath));
                            Print($"[ExpertTrader] Logo cargado: {logoPath}");
                        }
                        else
                        {
                            Print($"[ExpertTrader] ADVERTENCIA: Logo no encontrado en {logoPath}");
                        }
                    }
                    catch (Exception logoEx)
                    {
                        Print($"[ExpertTrader] Error cargando logo: {logoEx.Message}");
                    }
                    
                    // Inicializar tracking de progreso
                    int totalBars = Math.Min(BarsArray[0].Count, _config.BacktestBarsForAnalysis);
                    _coreEngine.StartProgressTracking(totalBars);
                    Print($"[ExpertTrader] ProgressTracker inicializado para {totalBars} barras");
                    
                    // ========================================================================
                    // VENTANA HISTÓRICA DETERMINISTA (V6.0i.7+ Arquitectura Correcta)
                    // ========================================================================
                    try
                    {
                        // Encontrar índice del TF de decisión en BarsArray
                        _decisionTFIndex = -1;
                        int decisionTF = _config.DecisionTimeframeMinutes;
                        for (int i = 0; i < BarsArray.Length; i++)
                        {
                            int tfMin = GetMinutesFromBarsPeriod(BarsArray[i].BarsPeriod);
                            if (tfMin == decisionTF)
                            {
                                _decisionTFIndex = i;
                                break;
                            }
                        }
                        if (_decisionTFIndex < 0)
                            _decisionTFIndex = 0; // fallback
                        
                        Print($"[ExpertTrader] TF decisión index={_decisionTFIndex}");
                        
                        // ========================================================================
                        // DIAGNÓSTICO: Mapeo BarsArray → TF (para detectar datasets desactualizados)
                        // ========================================================================
                        if (_fileLogger != null)
                        {
                            _fileLogger.Info("[DIAG][BARSARRAY] Mapeo de series históricas:");
                            for (int i = 0; i < BarsArray.Length; i++)
                            {
                                int tfMin = GetMinutesFromBarsPeriod(BarsArray[i].BarsPeriod);
                                int count = BarsArray[i].Count;
                                
                                // Obtener primero y último tiempo si hay datos
                                // NOTA: En State.DataLoaded las series de tiempo aún no están disponibles
                                string firstTime = "N/A", lastTime = "N/A";
                                if (count > 0 && State == State.Realtime)
                                {
                                    try
                                    {
                                        firstTime = Times[i][0].ToString("yyyy-MM-dd HH:mm");
                                        lastTime = Times[i][count - 1].ToString("yyyy-MM-dd HH:mm");
                                    }
                                    catch
                                    {
                                        firstTime = "N/A";
                                        lastTime = "N/A";
                                    }
                                }
                                
                                _fileLogger.Info($"[DIAG][BARSARRAY] i={i} → TF={tfMin}m | Count={count} | First={firstTime} | Last={lastTime}");
                            }
                        }
                        
                        Print("[ExpertTrader] ⚠️ Ventana histórica se configurará en el primer OnBarUpdate (las barras aún no están disponibles)");
                    }
                    catch (Exception exWindow)
                    {
                        Print($"[ExpertTrader] ERROR configurando índice TF decisión: {exWindow.Message}");
                        throw;
                    }

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
                // ========================================================================
                // ARQUITECTURA CORRECTA (V6.0i.7+ Determinismo Completo)
                // El indicador SOLO reenvía OnBarClose al motor y DIBUJA
                // No contiene lógica de anclaje, gating, ni catch-up
                // ========================================================================
                
                // 1. Identificar qué TF se actualizó
                int barsInProgressIndex = BarsInProgress;
                BarsPeriod period = BarsArray[barsInProgressIndex].BarsPeriod;
                int tfMinutes = GetMinutesFromBarsPeriod(period);
                int barIndex = CurrentBars[barsInProgressIndex];
                
                // TRAZA: OnBarUpdate llamándose
                if (tfMinutes == _config.DecisionTimeframeMinutes)
                    _logger.Info($"[DIAG][ExpertTrader][ONBARUPDATE] TF={tfMinutes} Bar={barIndex} BarsInProgress={barsInProgressIndex}");
                
                // 2. Reenviar al CoreEngine (el motor se autoconfigura y aplica gate internamente)
                _coreEngine.OnBarClose(tfMinutes, barIndex);
                
                // NUEVO: tracking OHLC con ventana fija post-registro
                if (_config.EnableOHLCLogging && _tradeManager != null)
                {
                    // Decremento de ventanas en 5m
                    if (tfMinutes == 5)
                        _tradeManager.DecrementTrackingWindowsForTF(tfMinutes);

                    // Log OHLC si hay ventanas activas (5m y 15m)
                    if (_tradeManager.HasActiveTrackingWindows() && (tfMinutes == 5 || tfMinutes == 15))
                        _coreEngine.LogBarOHLC(tfMinutes, barIndex);
                }
                
                // 3. FAST LOAD: Actualizar scores dinámicamente si está en modo estático
                if (_config.EnableFastLoadFromJSON && barsInProgressIndex == _decisionTFIndex)
                {
                    _coreEngine.UpdateScoresForFastLoad(tfMinutes, barIndex);
                }
                
                // 4. Generar decisión SOLO cuando el TF de decisión se actualiza
                if (barsInProgressIndex == _decisionTFIndex)
                {
                    // TRAZA: Llegamos al bloque de decisión
                    _logger.Info($"[DIAG][ExpertTrader][DECISION_BLOCK] TF={tfMinutes} Bar={barIndex} _decisionTFIndex={_decisionTFIndex}");
                    
                    // Guard: una sola decisión por barra del TF de decisión
                    if (barIndex == _lastProcessedBarOfDecision)
                    {
                        _logger.Info($"[DIAG][ExpertTrader] TF={tfMinutes} Bar={barIndex} SKIPPED (ya procesada: last={_lastProcessedBarOfDecision})");
                        return;
                    }
                    _lastProcessedBarOfDecision = barIndex;

                    // Null-guards y validaciones
                    if (_decisionEngine == null || _coreEngine == null || _barDataProvider == null)
                    {
                        _logger.Error("[ExpertTrader] Componentes nulos: DecisionEngine/CoreEngine/BarDataProvider.");
                        return;
                    }
                    if (barIndex < 0)
                    {
                        _logger.Error($"[ExpertTrader] barIndex inválido ({barIndex}) para TF {tfMinutes}m.");
                        return;
                    }
                    
                    // Recalcular métricas locales para logs
                    int totalBars = BarsArray[barsInProgressIndex].Count;
                    bool enableLogging = (State == State.Realtime) || (barIndex >= totalBars - _config.LoggingThresholdBars);

                    if (enableLogging)
                        _logger.Debug($"[ExpertTrader] Generando decisión para BarIndex: {barIndex}");
                    
                    // Gate: no ejecutar pipeline hasta que el Core ancle la ventana
                    if (!_coreEngine.IsHistoricalWindowConfigured)
                    {
                        _logger.Info($"[DIAG][ExpertTrader] TF={tfMinutes} Bar={barIndex} SKIPPED (ventana no configurada)");
                        return;
                    }
                    
                    // Gate: solo ejecutar pipeline para barras DENTRO de la ventana histórica
                    if (!_coreEngine.IsBarInHistoricalWindow(tfMinutes, barIndex))
                    {
                        _logger.Info($"[DIAG][ExpertTrader] TF={tfMinutes} Bar={barIndex} SKIPPED (fuera de ventana histórica)");
                        return;
                    }
                    
                    // Nota: La generación de decisiones se mueve al CoreEngine.OnBarClose
                    // El indicador solo notifica barras y dibuja; el Core decide y registra
                    _logger.Debug($"[ExpertTrader] TF={tfMinutes} Bar={barIndex} - Core maneja decisiones");
                }
                
                // 5. DIBUJAR VISUALIZACIÓN en el TF principal (BarsInProgress == 0)
                if (BarsInProgress == 0)
                {
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
        /// V5.7e: Usa el TF y barIndex del análisis, no del gráfico
        /// </summary>
        private void ProcessTradeTracking(int analysisTF, int analysisBarIndex)
        {
            if (_lastDecision == null)
                return;

            // Obtener High/Low/Time del TF de análisis (V5.7e)
            double currentHigh = _barDataProvider.GetHigh(analysisTF, analysisBarIndex);
            double currentLow = _barDataProvider.GetLow(analysisTF, analysisBarIndex);
            double currentPrice = _barDataProvider.GetClose(analysisTF, analysisBarIndex);
            DateTime currentTime = _barDataProvider.GetBarTime(analysisTF, analysisBarIndex);

            // V6.0i.5: Pasar régimen "HighVol" por defecto para aplicar gracia BOS
            // La lógica de si se aplica o no está controlada por EnableBOSGraceInHighVolOnly en TradeManager
            // TODO: Exponer régimen actual desde CoreEngine/ContextManager para mayor precisión
            string currentRegime = "HighVol";

            // PASO 1: Actualizar estado de todas las órdenes activas
            _tradeManager.UpdateTrades(currentHigh, currentLow, analysisBarIndex, currentTime, currentPrice, _coreEngine, _barDataProvider, currentRegime);

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

                // V6.0i.9: Loguear bloqueo por concurrencia pero SIEMPRE pasar a TradeManager
                int activeCount = _tradeManager.GetActiveTrades().Count;
                if (activeCount >= _config.MaxConcurrentTrades)
                {
                    string action = _lastDecision?.Action ?? "UNKNOWN";
                    double entry = _lastDecision?.Entry ?? 0.0;
                    string structId = !string.IsNullOrEmpty(sourceStructureId) ? $" StructId={sourceStructureId}" : "";
                    _logger.Info($"[CONCURRENCY_BLOCK] Signal {action} @ {entry:F2}{structId} blocked by {activeCount}/{_config.MaxConcurrentTrades} active trades - forwarding to TradeManager for gate evaluation");
                    // NO return - TradeManager decidirá con toda la información (puede rechazar por distancia)
                }

                // V6.0i.9: Extraer DistATR real desde TradeDecision (ya calculado por RiskCalculator → OutputAdapter)
                double distanceToEntryATR = _lastDecision?.DistanceToEntryATR ?? -1.0;

                _tradeManager.RegisterTrade(
                    _lastDecision.Action,
                    _lastDecision.Entry,
                    _lastDecision.StopLoss,
                    _lastDecision.TakeProfit,
                    analysisBarIndex,
                    currentTime,
                    tfDominante,
                    sourceStructureId,
                    currentPrice,  // Precio de registro para determinar LIMIT vs STOP
                    distanceToEntryATR,  // V6.0i.9: -1.0 = no disponible, usar fallback
                    currentRegime  // V6.0i.9: Pasar régimen actual
                );
            }
        }

        /// <summary>
        /// Convierte un tiempo a BarsAgo en el TF del gráfico (V5.7e)
        /// </summary>
        private int GetBarsAgoFromTime(DateTime time)
        {
            if (time == DateTime.MinValue)
            {
                return -1;
            }
            
            // Buscar la barra más cercana al tiempo dado
            // Time[0] es la barra MÁS RECIENTE, Time[CurrentBar] es la MÁS ANTIGUA
            for (int i = 0; i < Math.Min(CurrentBar, 5000); i++)
            {
                if (Time[i] <= time)
                {
                    return i;
                }
            }
            
            return -1; // No encontrado
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

        /// <summary>
        /// Busca el primer índice de barra (0 = más antigua, Count-1 = más reciente) cuyo tiempo >= targetTime
        /// Usa Times[] directamente del seriesIdx (independiente del provider/CurrentBars)
        /// </summary>
        private int FindBarIndexFromTime(int seriesIdx, DateTime targetTime)
        {
            int count = BarsArray[seriesIdx].Count;
            if (count <= 0)
                return -1;

            int left = 0;
            int right = count - 1;
            int result = -1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                int barsAgo = (count - 1) - mid; // Times[0] = más reciente
                DateTime t = Times[seriesIdx][barsAgo];

                if (t >= targetTime)
                {
                    result = mid; // candidato, buscar más antiguo a la izquierda
                    right = mid - 1;
                }
                else
                {
                    left = mid + 1;
                }
            }

            return result >= 0 ? result : 0;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Inicialización perezosa: si algún componente crítico es null, intenta inicializarlo aquí.
        /// Evita estados nulos por timing entre DataLoaded y primeras llamadas a OnBarUpdate.
        /// </summary>
        private void EnsureInitializedLazy()
        {
            try
            {
                if (_logger == null)
                {
                    if (EnableOutputLogging)
                    {
                        _logger = new NinjaTraderLogger(this, LogLevel.Info);
                        Print("[ExpertTrader] LazyInit: Output logging ACTIVADO");
                    }
                    else
                    {
                        _logger = new SilentLogger();
                        Print("[ExpertTrader] LazyInit: Output logging DESACTIVADO");
                    }
                }

                if (_config == null)
                {
                    _config = EngineConfig.LoadDefaults();
                    _config.EnableFastLoadFromJSON = EnableFastLoad;
                    Print("[ExpertTrader] LazyInit: Config cargada");
                }

                if (_barDataProvider == null)
                {
                    _barDataProvider = new NinjaTraderBarDataProvider(this);
                    Print("[ExpertTrader] LazyInit: BarDataProvider creado");
                }

                if (_fileLogger == null || _tradeLogger == null)
                {
                    string userDataDir = NinjaTrader.Core.Globals.UserDataDir;
                    string logDirectory = System.IO.Path.Combine(userDataDir, "PinkButterfly", "logs");
                    
                    if (_fileLogger == null)
                    {
                        _fileLogger = new FileLogger(logDirectory, "backtest", _logger, EnableFileLogging);
                        Print($"[ExpertTrader] LazyInit: File logging {(EnableFileLogging ? "ACTIVADO" : "DESACTIVADO")}");
                    }
                    
                    if (_tradeLogger == null)
                    {
                        _tradeLogger = new TradeLogger(logDirectory, "trades", _logger, EnableTradeCSV);
                        Print($"[ExpertTrader] LazyInit: Trade CSV {(EnableTradeCSV ? "ACTIVADO" : "DESACTIVADO")}");
                    }
                }

                if (_coreEngine == null)
                {
                    _coreEngine = new CoreEngine(_barDataProvider, _config, _fileLogger);
                    _coreEngine.Initialize();
                    Print("[ExpertTrader] LazyInit: CoreEngine inicializado");
                }

                if (_decisionEngine == null)
                {
                    _decisionEngine = new DecisionEngine(_config, _fileLogger);
                    Print("[ExpertTrader] LazyInit: DecisionEngine inicializado");
                }
                
                // Inyectar DecisionEngine al CoreEngine (si ambos existen y aún no se ha inyectado)
                if (_coreEngine != null && _decisionEngine != null)
                {
                    _coreEngine.SetDecisionEngine(_decisionEngine, AccountSize);
                    Print($"[ExpertTrader] LazyInit: DecisionEngine inyectado al CoreEngine (AccountSize={AccountSize})");
                }

                if (_tradeManager == null)
                {
                    double pointValue = Instrument.MasterInstrument.PointValue;
                    _tradeManager = new TradeManager(_config, _fileLogger, _tradeLogger, ContractSize, pointValue);
                    Print("[ExpertTrader] LazyInit: TradeManager inicializado");
                }
            }
            catch (Exception ex)
            {
                Print($"[ExpertTrader] LazyInit ERROR: {ex.Message}");
                Print($"Stack: {ex.StackTrace}");
            }
        }

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
            
            // SL/TP ya se dibujan en DrawEntryLine() con las zonas de color
            // if (ShowSLTPLines)
            // {
            //     DrawSLLine();
            //     DrawTPLine();
            // }

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
                    
                    // V5.7e: Convertir ExecutionBar del TF de análisis al TF del gráfico usando tiempo
                    int barsAgo1 = GetBarsAgoFromTime(trade.ExecutionBarTime);
                    int barsAgo2 = trade.ExitBar > 0 ? GetBarsAgoFromTime(trade.ExitBarTime) : 0;
                    
                    // Validar que los índices sean válidos
                    if (barsAgo1 < 0 || (trade.ExitBar > 0 && barsAgo2 < 0))
                        continue;
                    
                    // Asegurar que startBarsAgo sea el MAYOR (más antiguo/izquierda) y endBarsAgo el MENOR (más reciente/derecha)
                    // para que el rectángulo siempre se dibuje de izquierda a derecha
                    int startBarsAgo = Math.Max(barsAgo1, barsAgo2);
                    int endBarsAgo = Math.Min(barsAgo1, barsAgo2);
                    
                    string tag = TAG_ENTRY + trade.Id;
                    
                    // Calcular estadísticas del trade
                    double pointValue = Instrument.MasterInstrument.PointValue;
                    double tpPoints = Math.Abs(trade.TP - trade.Entry);
                    double slPoints = Math.Abs(trade.Entry - trade.SL);
                    double tpDollars = tpPoints * pointValue * ContractSize;
                    double slDollars = slPoints * pointValue * ContractSize;
                    
                    // Calcular R:R de esta operación
                    double riskReward = slPoints > 0 ? tpPoints / slPoints : 0;
                    
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
                    
                    // Textos sobre TP y SL según dirección de la orden
                    if (trade.Action == "BUY")
                    {
                        // BUY: TP arriba, SL abajo
                        // Texto sobre TP (encima de zona verde)
                        string tpText = $"+${tpDollars:F0}\nR:R {riskReward:F1}:1\nTP: {trade.TP:F2}";
                        Draw.Text(this, tag + "_TP_INFO", false, tpText,
                            startBarsAgo, trade.TP + (10 * TickSize),
                            0, Brushes.LimeGreen, new SimpleFont("Arial", 10), 
                            System.Windows.TextAlignment.Left, Brushes.Transparent, Brushes.Transparent, 0);
                        
                        // Texto bajo SL (debajo de zona roja)
                        string slText = $"SL: {trade.SL:F2}\n-${slDollars:F0}";
                        Draw.Text(this, tag + "_SL_INFO", false, slText,
                            startBarsAgo, trade.SL - (15 * TickSize),
                            0, Brushes.Red, new SimpleFont("Arial", 10), 
                            System.Windows.TextAlignment.Left, Brushes.Transparent, Brushes.Transparent, 0);
                    }
                    else // SELL
                    {
                        // SELL: SL arriba, TP abajo
                        // Texto sobre SL (encima de zona roja)
                        string slText = $"-${slDollars:F0}\nSL: {trade.SL:F2}";
                        Draw.Text(this, tag + "_SL_INFO", false, slText,
                            startBarsAgo, trade.SL + (10 * TickSize),
                            0, Brushes.Red, new SimpleFont("Arial", 10), 
                            System.Windows.TextAlignment.Left, Brushes.Transparent, Brushes.Transparent, 0);
                        
                        // Texto bajo TP (debajo de zona verde)
                        string tpText = $"TP: {trade.TP:F2}\nR:R {riskReward:F1}:1\n+${tpDollars:F0}";
                        Draw.Text(this, tag + "_TP_INFO", false, tpText,
                            startBarsAgo, trade.TP - (15 * TickSize),
                            0, Brushes.LimeGreen, new SimpleFont("Arial", 10), 
                            System.Windows.TextAlignment.Left, Brushes.Transparent, Brushes.Transparent, 0);
                    }
                    
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
                
                // Panel de órdenes pendientes movido a DrawPanel()
                // Las líneas de puntos eliminadas para evitar confusión visual
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
                // LOGO: Texto estilizado (Draw.Image no está disponible en NT8)
                // ================================================================
                var logoPanel = new System.Text.StringBuilder();
                logoPanel.AppendLine("╔═══════════════════════════╗");
                logoPanel.AppendLine("║  🦋 PinkButterfly 🦋     ║");
                logoPanel.AppendLine("║  Smart Trading System     ║");
                logoPanel.AppendLine("╚═══════════════════════════╝");
                
                // ================================================================
                // PANEL 1: PRÓXIMA OPERACIÓN (con logo arriba)
                // ================================================================
                var panel1 = new System.Text.StringBuilder();
                // Añadir logo al inicio
                panel1.Append(logoPanel.ToString());
                panel1.AppendLine("");  // Espacio
                panel1.AppendLine("═══════════════════════════════");
                panel1.AppendLine("   PRÓXIMA OPERACIÓN");
                panel1.AppendLine("═══════════════════════════════");
                
                // Obtener la próxima operación pendiente (la más cercana al precio)
                var nextPendingTrades = _tradeManager.GetAllTrades()
                    .Where(t => t.Status == TradeStatus.PENDING)
                    .OrderBy(t => Math.Abs(t.Entry - Close[0]))
                    .ToList();
                
                if (nextPendingTrades.Count > 0)
                {
                    var nextTrade = nextPendingTrades.First();
                    
                    // Sesgo (Alcista/Neutral/Bajista)
                    string bias = _coreEngine.CurrentMarketBias;
                    string biasES = bias == "Bullish" ? "Alcista" : bias == "Bearish" ? "Bajista" : "Neutral";
                    panel1.AppendLine($" Sentimiento: {biasES}");
                    
                    // Acción (BUY en verde, SELL en rojo)
                    string action = nextTrade.Action;
                    panel1.AppendLine($" Acción: {action}");
                    
                    // Confianza
                    double confidence = _lastDecision != null ? _lastDecision.Confidence : 0;
                    panel1.AppendLine($" Confianza: {(confidence * 100):F0}%");
                    
                    // Entrada
                    panel1.AppendLine($" Entrada: {nextTrade.Entry:F2}");
                    
                    // TP con puntos
                    double tpPoints = Math.Abs(nextTrade.TP - nextTrade.Entry);
                    panel1.AppendLine($" TP: {nextTrade.TP:F2} - {tpPoints:F2} pts");
                    
                    // SL con puntos
                    double slPoints = Math.Abs(nextTrade.Entry - nextTrade.SL);
                    panel1.AppendLine($" SL: {nextTrade.SL:F2} - {slPoints:F2} pts");
                    
                    // Ganancia en $
                    double ganancia = tpPoints * pointValue * ContractSize;
                    panel1.AppendLine($" Ganancia: ${ganancia:F2}");
                    
                    // Pérdida en $
                    double perdida = slPoints * pointValue * ContractSize;
                    panel1.AppendLine($" Pérdida: ${perdida:F2}");
                }
                else
                {
                    // No hay operaciones pendientes
                    string bias = _coreEngine.CurrentMarketBias;
                    string biasES = bias == "Bullish" ? "Alcista" : bias == "Bearish" ? "Bajista" : "Neutral";
                    panel1.AppendLine($" Sentimiento: {biasES}");
                    panel1.AppendLine("");
                    panel1.AppendLine(" No hay señales pendientes");
                }
                
                panel1.AppendLine("═══════════════════════════════");
                
                // ================================================================
                // PANEL 2: DATOS DE SESIÓN (continuar en panel1 para que quede debajo)
                // ================================================================
                panel1.AppendLine("");  // Espacio entre paneles
                panel1.AppendLine("═══════════════════════════════");
                panel1.AppendLine("   DATOS DE SESIÓN");
                panel1.AppendLine("═══════════════════════════════");
                
                // Obtener estadísticas del TradeManager
                var allTrades = _tradeManager.GetAllTrades();
                var executedTrades = allTrades.Where(t => t.Status == TradeStatus.TP_HIT || t.Status == TradeStatus.SL_HIT).ToList();
                var pendingCount = allTrades.Count(t => t.Status == TradeStatus.PENDING);
                var activeCount  = allTrades.Count(t => t.Status == TradeStatus.EXECUTED);
                
                int totalExecuted = executedTrades.Count;
                int wins = executedTrades.Count(t => t.Status == TradeStatus.TP_HIT);
                int losses = executedTrades.Count(t => t.Status == TradeStatus.SL_HIT);
                double winRate = totalExecuted > 0 ? (double)wins / totalExecuted * 100 : 0;
                
                // Calcular ganancias y pérdidas en $
                double totalGanadas = 0;
                double totalPerdidas = 0;
                
                foreach (var trade in executedTrades)
                {
                    if (trade.Status == TradeStatus.TP_HIT)
                    {
                        double points = Math.Abs(trade.TP - trade.Entry);
                        totalGanadas += points * pointValue * ContractSize;
                    }
                    else if (trade.Status == TradeStatus.SL_HIT)
                    {
                        double points = Math.Abs(trade.Entry - trade.SL);
                        totalPerdidas += points * pointValue * ContractSize;
                    }
                }
                
                double resultado = totalGanadas - totalPerdidas;
                
                // Construir texto del panel (continuar en panel1)
                panel1.AppendLine($" Cerradas: {totalExecuted}");
                panel1.AppendLine($" Activas: {activeCount}");
                panel1.AppendLine($" Pendientes: {pendingCount}");
                panel1.AppendLine($" Win Rate: {winRate:F0}%");
                panel1.AppendLine($" Total Ganadas:");
                panel1.AppendLine($"   ${totalGanadas:F2}");
                panel1.AppendLine($" Total Pérdidas:");
                panel1.AppendLine($"   ${totalPerdidas:F2}");
                panel1.AppendLine($" RESULTADO: ${resultado:F2}");
                panel1.AppendLine("═══════════════════════════════");
                
                // ================================================================
                // PANEL 3: ÓRDENES PENDIENTES (pegado al final)
                // ================================================================
                var allPendingTrades = _tradeManager.GetAllTrades()
                    .Where(t => t.Status == TradeStatus.PENDING)
                    .OrderBy(t => Math.Abs(t.Entry - Close[0]))
                    .ToList();
                
                if (allPendingTrades.Count > 0)
                {
                    panel1.AppendLine("");  // Espacio
                    panel1.AppendLine("═══════════════════════════════");
                    panel1.AppendLine("   ÓRDENES PENDIENTES");
                    panel1.AppendLine("═══════════════════════════════");
                    
                    double currentPrice = Close[0];
                    var groupedPending = allPendingTrades
                        .GroupBy(t => Math.Round(t.Entry, 2))
                        .Select(g => new { 
                            Entry = g.Key, 
                            Count = g.Count(),
                            Action = g.First().Action,
                            Distance = Math.Abs(g.Key - currentPrice)
                        })
                        .OrderBy(g => g.Distance)
                        .ToList();
                    
                    foreach (var group in groupedPending)
                    {
                        string action = group.Action == "BUY" ? "BUY " : "SELL";
                        string countStr = group.Count > 1 ? $"({group.Count}x) " : "";
                        double dist = group.Entry - currentPrice;
                        string distStr = dist > 0 ? $"+{dist:F2}" : $"{dist:F2}";
                        
                        panel1.AppendLine($" {action} {countStr}@ {group.Entry:F2} [{distStr}]");
                    }
                    
                    panel1.AppendLine("═══════════════════════════════");
                }
                
                // Crear brush rosa para el fondo
                var pinkBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 105, 180));  // Hot Pink
                pinkBrush.Opacity = 0.85;
                pinkBrush.Freeze();
                
                // Dibujar Panel combinado en TopRight con fondo rosa
                Draw.TextFixed(this, TAG_PANEL + "TRADE", panel1.ToString(),
                    TextPosition.TopRight,
                    Brushes.White,
                    new Gui.Tools.SimpleFont("Courier New", 10),
                    Brushes.Transparent,
                    pinkBrush,
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

 
