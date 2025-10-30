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
        
        // Contadores para estadísticas de SyncGate
        private int _totalBarsProcessed = 0;
        private int _barsOmittedBySyncGate = 0;
        
        // Tracking de última barra procesada por TF (evita llamadas duplicadas a OnBarClose)
        private Dictionary<int, int> _lastProcessedBarByTF = new Dictionary<int, int>();

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
        [Display(Name = "Días de Backtest", Description = "Número de días históricos a analizar (10 días = tests rápidos ~5-8 min, 52 días = completo ~25-30 min)", Order = 8, GroupName = "Performance")]
        [Range(5, 200)]
        public int BacktestDays { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Contratos por Operación", Description = "Número de contratos a operar (para cálculo de P&L)", Order = 9, GroupName = "Risk Management")]
        [Range(1, 100)]
        public int ContractSize { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Enable Output Logging", Description = "Activar logs en Output window de NinjaTrader", Order = 11, GroupName = "Logging")]
        public bool EnableOutputLogging { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Enable File Logging", Description = "Activar logs en archivo de disco (puede crecer mucho en tiempo real)", Order = 12, GroupName = "Logging")]
        public bool EnableFileLogging { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Enable Trade CSV", Description = "Activar registro de operaciones en archivo CSV", Order = 13, GroupName = "Logging")]
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
                BacktestDays = 10; // Por defecto 10 días (~3000 barras en TF 5m) para tests rápidos
                
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
                    
                    // Convertir días a barras según el TF más bajo
                    int lowestTF = _config.TimeframesToUse.Min();
                    int barsPorDia = 1440 / lowestTF; // 1440 minutos en un día
                    _config.BacktestBarsForAnalysis = BacktestDays * barsPorDia;
                    
                    Print($"[ExpertTrader] Backtest configurado: {BacktestDays} días = {_config.BacktestBarsForAnalysis} barras (TF base: {lowestTF}m, {barsPorDia} barras/día)");
                    
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
                    
                    // DIAGNÓSTICO: Mostrar rangos temporales de cada TF
                    LogTFRanges();

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

                    // Inicializar TradeManager con TradeLogger
                    double pointValue = Instrument.MasterInstrument.PointValue;
                    _tradeManager = new TradeManager(_config, _fileLogger, _tradeLogger, ContractSize, pointValue);
                    Print("[ExpertTrader] TradeManager inicializado");

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
                    
                    // Diagnóstico de rangos temporales
                    LogTFRanges();
                    
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
                // Imprimir estadísticas de SyncGate
                int totalBars = _totalBarsProcessed + _barsOmittedBySyncGate;
                if (totalBars > 0)
                {
                    double omittedPct = (_barsOmittedBySyncGate * 100.0) / totalBars;
                    double processedPct = (_totalBarsProcessed * 100.0) / totalBars;
                    
                    Print("╔════════════════════════════════════════════════════════════════╗");
                    Print("║           📊 ESTADÍSTICAS DE SYNCGATE (MULTI-TF)             ║");
                    Print("╠════════════════════════════════════════════════════════════════╣");
                    Print($"║ 📈 Barras intentadas:    {totalBars,6}                             ║");
                    Print($"║ ✅ Barras procesadas:    {_totalBarsProcessed,6}  ({processedPct,5:F1}%)                   ║");
                    Print($"║ ⚠️  Barras omitidas:     {_barsOmittedBySyncGate,6}  ({omittedPct,5:F1}%)                   ║");
                    Print("╚════════════════════════════════════════════════════════════════╝");
                    
                    // También al log
                    if (_fileLogger != null)
                    {
                        _fileLogger.Info("========================================");
                        _fileLogger.Info("[ESTADISTICAS SYNCGATE]");
                        _fileLogger.Info($"Barras intentadas:  {totalBars}");
                        _fileLogger.Info($"Barras procesadas:  {_totalBarsProcessed} ({processedPct:F1}%)");
                        _fileLogger.Info($"Barras omitidas:    {_barsOmittedBySyncGate} ({omittedPct:F1}%)");
                        _fileLogger.Info("========================================");
                    }
                }
                
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
                // CORRECCIÓN Multi-TF: Usar el lowestTF para el cálculo, no el TF del gráfico
                int lowestTF = _config.TimeframesToUse.Min();
                int lowestTFIndex = Array.FindIndex(BarsArray, b => b != null && (int)b.BarsPeriod.Value == lowestTF);
                
                if (lowestTFIndex >= 0 && State == State.Historical)
                {
                    int totalBarsLowestTF = BarsArray[lowestTFIndex].Count;
                    int barsToSkip = totalBarsLowestTF - _config.BacktestBarsForAnalysis;
                    
                    // CRÍTICO: Obtener el barIndex del lowestTF correspondiente al TIEMPO de esta barra
                    // NO usar GetCurrentBarIndex (que devuelve el más reciente), sino mapear por tiempo
                    DateTime currentTime = Time[0]; // Tiempo de la barra actual que disparó OnBarUpdate
                    int lowestBarIndex = _barDataProvider.GetBarIndexFromTime(lowestTF, currentTime);
                    
                    if (lowestBarIndex >= 0 && lowestBarIndex < barsToSkip)
                    {
                        // Saltar barras antiguas en histórico para acelerar la carga
                        if (lowestBarIndex == barsToSkip - 1 && _fileLogger != null)
                        {
                            _fileLogger.Info($"[ExpertTrader] ⏭️ Saltadas {barsToSkip} barras históricas del lowestTF ({lowestTF}m). Iniciando procesamiento desde barra {lowestBarIndex}/{totalBarsLowestTF}");
                        }
                        return;
                    }
                }
                
                // 6. Control de logging: solo loggear las últimas N barras del histórico
                int currentTFTotalBars = BarsArray[barsInProgressIndex].Count;
                bool enableLogging = (State == State.Realtime) || 
                                    (barIndex >= currentTFTotalBars - _config.LoggingThresholdBars);
                
                // Log de inicio de procesamiento (solo una vez al llegar al umbral)
                if (lowestTFIndex >= 0 && State == State.Historical)
                {
                    int totalBarsLowestTF = BarsArray[lowestTFIndex].Count;
                    int barsToSkip = totalBarsLowestTF - _config.BacktestBarsForAnalysis;
                    int lowestBarIndex = _barDataProvider.GetCurrentBarIndex(lowestTF);
                    
                    if (lowestBarIndex == barsToSkip)
                    {
                        Print($"[ExpertTrader] Iniciando procesamiento histórico desde barra {lowestBarIndex}/{totalBarsLowestTF} del lowestTF ({lowestTF}m)");
                    }
                }
                
                // 7. CRÍTICO Multi-TF: Actualizar el TF que disparó OnBarUpdate
                // IMPORTANTE: Aplicar tracking TAMBIÉN aquí para evitar procesamiento duplicado
                if (_config.TimeframesToUse.Contains(tfMinutes))
                {
                    // Solo procesar si es una barra nueva para este TF
                    if (!_lastProcessedBarByTF.ContainsKey(tfMinutes) || _lastProcessedBarByTF[tfMinutes] < barIndex)
                    {
                        _coreEngine.OnBarClose(tfMinutes, barIndex);
                        _lastProcessedBarByTF[tfMinutes] = barIndex; // Actualizar tracking
                        
                        // Debug reducido: solo cada 1000 barras
                        if (enableLogging && _fileLogger != null && barIndex % 1000 == 0)
                            _fileLogger.Debug($"[ExpertTrader] OnBarClose({tfMinutes}m, bar {barIndex}) - BIP: {barsInProgressIndex}");
                    }
                    else if (enableLogging && _fileLogger != null && barIndex % 1000 == 0)
                    {
                        _fileLogger.Debug($"[ExpertTrader] OnBarClose({tfMinutes}m, bar {barIndex}) OMITIDA (ya procesada)");
                    }
                }
                
                // FAST LOAD: Actualizar scores dinámicamente incluso en modo estático
                if (_config.EnableFastLoadFromJSON && _config.TimeframesToUse.Contains(tfMinutes))
                {
                    _coreEngine.UpdateScoresForFastLoad(tfMinutes, barIndex);
                }

                // 8. SOLUCIÓN MULTI-TF: Cuando el TF más bajo se actualiza, actualizar TODOS los TFs
                // Esto garantiza que todas las estructuras (15m, 60m, 240m, 1440m) estén sincronizadas
                // (lowestTF ya está definido en línea 423)
                bool isLowestTF = tfMinutes == lowestTF;
                
                if (isLowestTF)
                {
                    // Obtener el chartTime del lowestTF (referencia temporal)
                    DateTime chartTime = _barDataProvider.GetBarTime(lowestTF, barIndex);
                    
                    // TRAZA TEMPORAL: Verificar que el sync se ejecuta (REDUCIDO: solo cada 1000 barras)
                    if (_fileLogger != null && barIndex % 1000 == 0)
                    {
                        _fileLogger.Info($"[ExpertTrader] 🔄 SYNC Multi-TF: lowestTF={lowestTF}m, barIndex={barIndex}, chartTime={chartTime:yyyy-MM-dd HH:mm}");
                    }
                    
                    // Actualizar TODOS los demás TFs con sus índices correspondientes al chartTime
                    foreach (int tf in _config.TimeframesToUse)
                    {
                        if (tf == lowestTF) continue; // Ya se actualizó arriba
                        
                        int tfBarIndex = _barDataProvider.GetBarIndexFromTime(tf, chartTime);
                        
                        if (tfBarIndex >= 0)
                        {
                            // PROTECCIÓN: Solo procesar si es una barra nueva (evita 2.8M warnings)
                            if (!_lastProcessedBarByTF.ContainsKey(tf) || _lastProcessedBarByTF[tf] < tfBarIndex)
                            {
                                _coreEngine.OnBarClose(tf, tfBarIndex);
                                _lastProcessedBarByTF[tf] = tfBarIndex; // Actualizar tracking
                                
                                // TRAZA TEMPORAL: Reducida drásticamente para no inflar el log
                                // Solo loggear cada 2000 barras o si falla
                            }
                            // Debug de YA PROCESADA: comentado para reducir log
                            // else if (_fileLogger != null && barIndex % 1000 == 0)
                            // {
                            //     _fileLogger.Debug($"[ExpertTrader] 🔄   → OnBarClose({tf}m, {tfBarIndex}) [YA PROCESADA, omitida]");
                            // }
                        }
                        else if (_fileLogger != null && barIndex % 1000 == 0)
                        {
                            _fileLogger.Warning($"[ExpertTrader] 🔄   → ⚠️ No se pudo mapear {tf}m para chartTime={chartTime:yyyy-MM-dd HH:mm}");
                        }
                    }
                }
                
                if (isLowestTF && barIndex >= 20)
                {
                    // Asegurar inicialización perezosa si algún componente es nulo (protege contra carreras del ciclo de vida)
                    EnsureInitializedLazy();

                    if (enableLogging)
                        _logger.Debug($"[ExpertTrader] Generando decisión para BarIndex: {barIndex} (TF {tfMinutes}m)");
                    
                    // Null-guards y validaciones
                    if (_decisionEngine == null || _coreEngine == null || _barDataProvider == null)
                    {
                        _logger.Error("[ExpertTrader] Componentes nulos: DecisionEngine/CoreEngine/BarDataProvider. Abortando GenerateDecision.");
                        return;
                    }
                    
                    // Usar el barIndex del TF actual (que es el lowestTF)
                    int lowestBarIndex = barIndex;
                    DateTime chartTime = _barDataProvider.GetBarTime(lowestTF, lowestBarIndex);
                    
                    // Gate de sincronización: exigir que TODOS los TFs tengan índice válido en chartTime
                    var tfMappings = _config.TimeframesToUse
                        .Select(tf => new { TF = tf, Index = _barDataProvider.GetBarIndexFromTime(tf, chartTime) })
                        .ToList();
                    
                    bool allMapped = tfMappings.All(m => m.Index >= 0);
                    
                    if (!allMapped)
                    {
                        _barsOmittedBySyncGate++;
                        
                        var failedTFs = tfMappings.Where(m => m.Index < 0).Select(m => m.TF).ToList();
                        
                        if (enableLogging && _fileLogger != null)
                            _fileLogger.Warning($"[ExpertTrader] ⚠️ SyncGate OMITE: chartTime={chartTime:yyyy-MM-dd HH:mm} | TFs fallidos=[{string.Join(", ", failedTFs)}]m | Omitidas hasta ahora={_barsOmittedBySyncGate}");
                        return;
                    }
                    
                    _totalBarsProcessed++;
                    
                    // Log de mapeo exitoso SOLO en las primeras 10 barras o cada 1000 barras
                    if (_fileLogger != null && (_totalBarsProcessed <= 10 || _totalBarsProcessed % 1000 == 0))
                    {
                        var mappingStr = string.Join(", ", tfMappings.Select(m => $"{m.TF}m@{m.Index}"));
                        _fileLogger.Info($"[ExpertTrader] ✅ SyncGate OK: chartTime={chartTime:yyyy-MM-dd HH:mm} | Mapeos=[{mappingStr}] | Procesadas={_totalBarsProcessed}");
                    }
                    
                    // Mostrar estadísticas cada 1000 barras procesadas (reducido de 100)
                    if (_totalBarsProcessed % 1000 == 0 && _fileLogger != null)
                    {
                        int totalAttempts = _totalBarsProcessed + _barsOmittedBySyncGate;
                        double processedPct = (_totalBarsProcessed * 100.0) / totalAttempts;
                        double omittedPct = (_barsOmittedBySyncGate * 100.0) / totalAttempts;
                        _fileLogger.Info($"[ExpertTrader] 📊 STATS SyncGate: Intentadas={totalAttempts} | Procesadas={_totalBarsProcessed} ({processedPct:F1}%) | Omitidas={_barsOmittedBySyncGate} ({omittedPct:F1}%)");
                    }
                    
                    // Índice del TF de análisis mapeado por tiempo (no "último índice")
                    int analysisBarIndex = _barDataProvider.GetBarIndexFromTime(lowestTF, chartTime);
                    if (analysisBarIndex < 0)
                    {
                        _logger.Error($"[ExpertTrader] ❌ analysisBarIndex inválido en {lowestTF}m para chartTime {chartTime:yyyy-MM-dd HH:mm} (esto NO debería pasar tras SyncGate).");
                        return;
                    }
                    
                    // Generar decisión con DecisionEngine usando el barIndex del TF de análisis
                    _lastDecision = _decisionEngine.GenerateDecision(_barDataProvider, _coreEngine, analysisBarIndex, lowestTF, AccountSize);

                    // LOG de señales BUY/SELL generadas (SIEMPRE al archivo, no solo con enableLogging)
                    if (_lastDecision != null && _lastDecision.Action != "WAIT" && _fileLogger != null)
                    {
                        _fileLogger.Info($"[ExpertTrader] 🎯 SEÑAL GENERADA | ID={_lastDecision.Id} | {_lastDecision.Action} @ {_lastDecision.Entry:F2} | Conf={_lastDecision.Confidence:F3} | SL={_lastDecision.StopLoss:F2} | TP={_lastDecision.TakeProfit:F2} | chartTime={chartTime:yyyy-MM-dd HH:mm} | analysisBar={analysisBarIndex}");
                    }
                    
                    // LOG DETALLADO DE LA DECISIÓN (valores SL/TP)
                    if (_lastDecision != null && enableLogging)
                    {
                        double rr = _lastDecision.StopLoss > 0 ? Math.Abs(_lastDecision.TakeProfit - _lastDecision.Entry) / Math.Abs(_lastDecision.Entry - _lastDecision.StopLoss) : 0;
                        _logger.Debug($"[ExpertTrader] *** DECISIÓN *** Action={_lastDecision.Action}, Entry={_lastDecision.Entry:F2}, SL={_lastDecision.StopLoss:F2}, TP={_lastDecision.TakeProfit:F2}, Conf={_lastDecision.Confidence:F3}, R:R={rr:F2}");
                    }

                    // TRACKING DE OPERACIÓN ACTIVA (V5.7e: usar TF de análisis, no TF del gráfico)
                    ProcessTradeTracking(lowestTF, analysisBarIndex);

                    // Obtener HeatZones del snapshot (para el panel de información)
                    _lastHeatZones = GetTopHeatZones();
                }
                
                // 9. DIBUJAR VISUALIZACIÓN SIEMPRE en el TF principal
                // Esto asegura actualización constante (en cada tick con Calculate.OnEachTick)
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

            // PASO 1: Actualizar estado de todas las órdenes activas
            _tradeManager.UpdateTrades(currentHigh, currentLow, analysisBarIndex, currentTime, currentPrice, _coreEngine, _barDataProvider);

            // PASO 2: Si llega una NUEVA señal BUY/SELL, delegarla directamente a TradeManager
            // TradeManager tiene el filtro de duplicados robusto (cooldown + tolerancia)
            bool isNewSignal = (_lastDecision.Action == "BUY" || _lastDecision.Action == "SELL");
            
            if (isNewSignal)
            {
                // Obtener el TF dominante de la última HeatZone (si existe)
                int tfDominante = _lastHeatZones != null && _lastHeatZones.Count > 0 
                    ? _lastHeatZones[0].TFDominante 
                    : _config.TimeframesToUse.Min();

                // Obtener el DominantStructureId de la decisión
                string sourceStructureId = _lastDecision.DominantStructureId ?? string.Empty;

                // DELEGAR a TradeManager: él filtrará duplicados con cooldown + tolerancia
                // Este es el barIndex del lowestTF (5m), no del gráfico (15m)
                _tradeManager.RegisterTrade(
                    _lastDecision.Action,
                    _lastDecision.Entry,
                    _lastDecision.StopLoss,
                    _lastDecision.TakeProfit,
                    analysisBarIndex,  // Este ya es del lowestTF (viene de ProcessTradeTracking)
                    currentTime,
                    tfDominante,
                    sourceStructureId
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
                    
                    // Convertir días a barras según el TF más bajo
                    int lowestTF = _config.TimeframesToUse.Min();
                    int barsPorDia = 1440 / lowestTF; // 1440 minutos en un día
                    _config.BacktestBarsForAnalysis = BacktestDays * barsPorDia;
                    
                    Print($"[ExpertTrader] LazyInit: Backtest configurado: {BacktestDays} días = {_config.BacktestBarsForAnalysis} barras");
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
                
                // PARTE 1: Dibujar solo EJECUTADAS o CERRADAS (SL/TP) - NO dibujar PENDING ni CANCELLED
                foreach (var trade in allTrades.Where(t =>
                    t.Status == TradeStatus.EXECUTED ||
                    t.Status == TradeStatus.SL_HIT ||
                    t.Status == TradeStatus.TP_HIT))
                {
                    if (trade.Entry <= 0 || trade.ExecutionBar == -1)
                        continue;
                    
                    // V5.7e: Convertir ExecutionBar del TF de análisis al TF del gráfico usando tiempo
                    int barsAgo1 = GetBarsAgoFromTime(trade.ExecutionBarTime);
                    int barsAgo2 = trade.ExitBar > 0 ? GetBarsAgoFromTime(trade.ExitBarTime) : 0;
                    
                    // TRAZA CRÍTICA: Diagnóstico de dibujo
                    if (_fileLogger != null && (State == State.Realtime || CurrentBar >= Count - 100))
                    {
                        _fileLogger.Info($"[DrawEntry] 🎨 Trade {trade.Id}: Entry={trade.Entry:F2} | ExecutionBar={trade.ExecutionBar} | ExecutionTime={trade.ExecutionBarTime:yyyy-MM-dd HH:mm}");
                        _fileLogger.Info($"[DrawEntry] 🎨   barsAgo1={barsAgo1} | Time[{barsAgo1}]={Time[barsAgo1]:yyyy-MM-dd HH:mm} | High={High[barsAgo1]:F2} | Low={Low[barsAgo1]:F2}");
                        _fileLogger.Info($"[DrawEntry] 🎨   Status={trade.Status} | Entry dentro de rango? {trade.Entry >= Low[barsAgo1] && trade.Entry <= High[barsAgo1]}");
                    }
                    
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
                
                // Limpieza: remover dibujos de operaciones CANCELLED (si los hubiera)
                foreach (var cancelled in allTrades.Where(t => t.Status == TradeStatus.CANCELLED))
                {
                    string tag = TAG_ENTRY + cancelled.Id;
                    RemoveDrawObject(tag);
                    RemoveDrawObject(tag + "_TPRect");
                    RemoveDrawObject(tag + "_SLRect");
                    RemoveDrawObject(tag + "_TP_INFO");
                    RemoveDrawObject(tag + "_SL_INFO");
                    RemoveDrawObject(tag + "_LABEL");
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
                // LOGO: Texto estilizado (Draw.Image no está disponible en NT8)
                // ================================================================
                var logoPanel = new System.Text.StringBuilder();
                logoPanel.AppendLine("╔═══════════════════════════╗");
                logoPanel.AppendLine("║  🦋 PinkButterfly 🦋      ║");
                logoPanel.AppendLine("║  Smart Trading System     ║");
                logoPanel.AppendLine("╚═══════════════════════════╝");
                
                // ================================================================
                // PANEL 1: PRÓXIMA OPERACIÓN (con logo arriba)
                // ================================================================
                var panel1 = new System.Text.StringBuilder();
                // Añadir logo al inicio
                panel1.Append(logoPanel.ToString());
                panel1.AppendLine("");  // Espacio
                panel1.AppendLine("═════════════════════════════");
                panel1.AppendLine("   PRÓXIMA OPERACIÓN");
                panel1.AppendLine("═════════════════════════════");
                
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
                
                panel1.AppendLine("═════════════════════════════");
                
                // ================================================================
                // PANEL 2: DATOS DE SESIÓN (continuar en panel1 para que quede debajo)
                // ================================================================
                panel1.AppendLine("");  // Espacio entre paneles
                panel1.AppendLine("═════════════════════════════");
                panel1.AppendLine("   DATOS DE SESIÓN");
                panel1.AppendLine("═════════════════════════════");
                
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
                
                // Construir texto del panel (continuar en panel1)
                panel1.AppendLine($" Ejecutadas: {totalExecuted}");
                panel1.AppendLine($" Pendientes: {pendingCount}");
                panel1.AppendLine($" Win Rate: {winRate:F0}%");
                panel1.AppendLine($" Total Beneficio:");
                panel1.AppendLine($"   ${totalBeneficios:F2}");
                panel1.AppendLine($" Total Pérdidas:");
                panel1.AppendLine($"   ${totalPerdidas:F2}");
                panel1.AppendLine($" RESULTADO: ${resultado:F2}");
                panel1.AppendLine("═════════════════════════════");
                
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
                    panel1.AppendLine("═════════════════════════════");
                    panel1.AppendLine("   ÓRDENES PENDIENTES");
                    panel1.AppendLine("═════════════════════════════");
                    
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
                    
                    panel1.AppendLine("═════════════════════════════");
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
        
        #region Diagnóstico Multi-TF
        
        /// <summary>
        /// Loguea los rangos temporales de cada TF para diagnóstico
        /// </summary>
        private void LogTFRanges()
        {
            try
            {
                // Solo loggear a archivo (no al OUTPUT para evitar llenar el buffer)
                if (_fileLogger == null)
                    return;
                
                if (BarsArray == null || BarsArray.Length == 0)
                {
                    _fileLogger.Info("[DIAGNOSTICO] ERROR: BarsArray no disponible");
                    return;
                }
                
                _fileLogger.Info("========================================");
                _fileLogger.Info("[DIAGNOSTICO] RANGOS TEMPORALES DE CADA TF:");
                _fileLogger.Info("========================================");
                
                for (int bip = 0; bip < BarsArray.Length; bip++)
                {
                    if (BarsArray[bip] == null)
                        continue;
                    
                    var bars = BarsArray[bip];
                    int totalBars = bars.Count;
                    string tfName = bars.BarsPeriod.ToString();
                    
                    if (totalBars > 0)
                    {
                        DateTime firstBar = bars.GetTime(totalBars - 1); // Más antigua
                        DateTime lastBar = bars.GetTime(0);              // Más reciente
                        TimeSpan range = lastBar - firstBar;
                        
                        string msg = $"BIP={bip} TF={tfName} | Barras={totalBars} | {firstBar:yyyy-MM-dd HH:mm} → {lastBar:yyyy-MM-dd HH:mm} ({range.TotalDays:F1} días)";
                        _fileLogger.Info($"[DIAGNOSTICO] {msg}");
                    }
                    else
                    {
                        _fileLogger.Info($"[DIAGNOSTICO] BIP={bip} TF={tfName} | SIN BARRAS");
                    }
                }
                
                _fileLogger.Info("========================================");
            }
            catch (Exception ex)
            {
                if (_fileLogger != null)
                    _fileLogger.Error($"[DIAGNOSTICO] Error en LogTFRanges: {ex.Message}");
            }
        }
        
        #endregion
    }
}
