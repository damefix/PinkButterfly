// ============================================================================
// ExpertIndicator.cs
// PinkButterfly CoreBrain - Indicador de Visualización de Estructuras
// 
// Visualiza TODAS las estructuras detectadas por CoreEngine:
// - Swings (Swing Highs/Lows) ← PASO 1
// - BOS/CHoCH (Break of Structure)
// - Order Blocks
// - FVGs (Fair Value Gaps)
// - Liquidity Grabs
// - POI (Points of Interest)
// - Doubles (Double Tops/Bottoms)
// - Liquidity Voids
//
// Propósito: Validación visual sistemática, NO trading
// ============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Data;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.NinjaScript;
using SharpDX;
using SharpDX.Direct2D1;

// Enum para selección de timeframe
public enum TimeframeOption
{
    TF_5m = 5,
    TF_15m = 15,
    TF_60m = 60,
    TF_240m = 240,
    TF_1440m = 1440
}

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    public class ExpertIndicator : Indicator
    {
        // ====================================================================
        // INSTANCIAS CORE
        // ====================================================================
        
        private CoreEngine _engine;
        private IBarDataProvider _barProvider;
        private EngineConfig _engineConfig;
        private ILogger _logger;
        private System.IO.StreamWriter _logWriter;
        private string _logFilePath;
        private int _renderCallCount = 0; // Contador para limitar trazas
        
        // Cache para BOS/CHoCH
        private IReadOnlyList<StructureBreakInfo> _cachedBOS = null;
        private int _lastBOSUpdateBar = -1;
        
        // ====================================================================
        // BOS/CHoCH (Break of Structure / Change of Character)
        // ====================================================================
        
        [NinjaScriptProperty]
        [Display(Name = "Mostrar BOS/CHoCH", Order = 5, GroupName = "00. BOS/CHoCH")]
        public bool ShowBOS { get; set; }
        
        [NinjaScriptProperty]
        [Display(Name = "Mostrar Etiquetas (↑↓ / TRN)", Order = 6, GroupName = "00. BOS/CHoCH")]
        public bool ShowBOSLabels { get; set; }
        
        [NinjaScriptProperty]
        [Display(Name = "Mostrar Score (% Fuerza)", Order = 7, GroupName = "00. BOS/CHoCH")]
        public bool ShowBOSScoreLabels { get; set; }
        
        [XmlIgnore]
        [Display(Name = "Color BOS/CHoCH Bullish", Order = 8, GroupName = "00. BOS/CHoCH")]
        public System.Windows.Media.Brush BOSBullishColor { get; set; }
        
        [Browsable(false)]
        public string BOSBullishColorSerialize
        {
            get { return Serialize.BrushToString(BOSBullishColor); }
            set { BOSBullishColor = Serialize.StringToBrush(value); }
        }
        
        [XmlIgnore]
        [Display(Name = "Color BOS/CHoCH Bearish", Order = 9, GroupName = "00. BOS/CHoCH")]
        public System.Windows.Media.Brush BOSBearishColor { get; set; }
        
        [Browsable(false)]
        public string BOSBearishColorSerialize
        {
            get { return Serialize.BrushToString(BOSBearishColor); }
            set { BOSBearishColor = Serialize.StringToBrush(value); }
        }
        
        [Range(1, 5)]
        [NinjaScriptProperty]
        [Display(Name = "Grosor de Línea", Order = 10, GroupName = "00. BOS/CHoCH")]
        public float BOSLineThickness { get; set; }
        
        [Range(5, 30)]
        [NinjaScriptProperty]
        [Display(Name = "Tamaño de Flecha Gráfica", Order = 11, GroupName = "00. BOS/CHoCH")]
        public float BOSArrowSize { get; set; }
        
        [Range(10, 24)]
        [NinjaScriptProperty]
        [Display(Name = "Tamaño de Fuente Texto", Order = 12, GroupName = "00. BOS/CHoCH")]
        public int BOSLabelFontSize { get; set; }
        
        [Range(12, 36)]
        [NinjaScriptProperty]
        [Display(Name = "Tamaño de Símbolo Flecha (↑↓)", Order = 13, GroupName = "00. BOS/CHoCH")]
        public int BOSArrowSymbolSize { get; set; }
        
        [NinjaScriptProperty]
        [Display(Name = "Timeframe a Visualizar (BOS)", Order = 14, GroupName = "00. BOS/CHoCH")]
        public TimeframeOption BOSTimeframeToVisualize { get; set; }
        
        [NinjaScriptProperty]
        [Display(Name = "Máximo de BOS/CHoCH a Mostrar", Order = 15, GroupName = "00. BOS/CHoCH")]
        [Range(10, 300)]
        public int MaxBOSToShow { get; set; }
        
        // ====================================================================
        // TOGGLES DE VISUALIZACIÓN (SWINGS)
        // ====================================================================
        
        [NinjaScriptProperty]
        [Display(Name = "1. Mostrar Swings", Order = 10, GroupName = "01. SWINGS (CRÍTICO)")]
        public bool ShowSwings { get; set; }
        
        [NinjaScriptProperty]
        [Display(Name = "Mostrar Swing Highs", Order = 11, GroupName = "01. SWINGS (CRÍTICO)")]
        public bool ShowSwingHighs { get; set; }
        
        [NinjaScriptProperty]
        [Display(Name = "Mostrar Swing Lows", Order = 12, GroupName = "01. SWINGS (CRÍTICO)")]
        public bool ShowSwingLows { get; set; }
        
        [NinjaScriptProperty]
        [Display(Name = "Mostrar Etiquetas de Precio", Order = 13, GroupName = "01. SWINGS (CRÍTICO)")]
        public bool ShowSwingPriceLabels { get; set; }
        
        [NinjaScriptProperty]
        [Display(Name = "Mostrar Score (% Fuerza)", Order = 14, GroupName = "01. SWINGS (CRÍTICO)")]
        public bool ShowSwingScoreLabels { get; set; }
        
        [NinjaScriptProperty]
        [Display(Name = "Mostrar SOLO Swings Activos", Order = 15, GroupName = "01. SWINGS (CRÍTICO)")]
        public bool ShowOnlyActiveSwings { get; set; }
        
        [NinjaScriptProperty]
        [Display(Name = "Timeframe a Visualizar", Order = 16, GroupName = "01. SWINGS (CRÍTICO)")]
        public TimeframeOption TimeframeToVisualize { get; set; }
        
        [NinjaScriptProperty]
        [Display(Name = "Máximo de Swings a Mostrar", Order = 17, GroupName = "01. SWINGS (CRÍTICO)")]
        [Range(10, 500)]
        public int MaxSwingsToShow { get; set; }
        
        // ====================================================================
        // COLORES SWINGS
        // ====================================================================
        
        [XmlIgnore]
        [Display(Name = "Color Swing Highs", Order = 20, GroupName = "02. COLORES SWINGS")]
        public System.Windows.Media.Brush SwingHighColor { get; set; }
        
        [Browsable(false)]
        public string SwingHighColorSerializable
        {
            get { return Serialize.BrushToString(SwingHighColor); }
            set { SwingHighColor = Serialize.StringToBrush(value); }
        }
        
        [XmlIgnore]
        [Display(Name = "Color Swing Lows", Order = 21, GroupName = "02. COLORES SWINGS")]
        public System.Windows.Media.Brush SwingLowColor { get; set; }
        
        [Browsable(false)]
        public string SwingLowColorSerializable
        {
            get { return Serialize.BrushToString(SwingLowColor); }
            set { SwingLowColor = Serialize.StringToBrush(value); }
        }
        
        [Range(1, 10)]
        [Display(Name = "Grosor de Líneas Swings", Order = 22, GroupName = "02. COLORES SWINGS")]
        public int SwingLineThickness { get; set; }
        
        [Range(10, 24)]
        [NinjaScriptProperty]
        [Display(Name = "Tamaño de Fuente Etiquetas", Order = 23, GroupName = "02. COLORES SWINGS")]
        public int SwingLabelFontSize { get; set; }
        
        // ====================================================================
        // LOGGING A ARCHIVO
        // ====================================================================
        
        private void LogToFile(string message)
        {
            if (_logWriter != null)
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                _logWriter.WriteLine($"[{timestamp}] {message}");
            }
        }
        
        // ====================================================================
        // INICIALIZACIÓN
        // ====================================================================
        
        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = @"ExpertIndicator - Visualización sistemática de estructuras de mercado para validación profesional";
                Name = "ExpertIndicator";
                Calculate = Calculate.OnBarClose;
                IsOverlay = true;
                DisplayInDataBox = false;
                DrawOnPricePanel = true;
                ScaleJustification = NinjaTrader.Gui.Chart.ScaleJustification.Right;
                IsSuspendedWhileInactive = true;
                
                // ✅ Enviar todas las trazas al Output Tab 2
                PrintTo = PrintTo.OutputTab2;
                
                // Swings por defecto
                ShowSwings = true;
                ShowSwingHighs = true;
                ShowSwingLows = true;
                ShowSwingPriceLabels = false;
                ShowSwingScoreLabels = true;  // Mostrar % fuerza por defecto
                ShowOnlyActiveSwings = false;  // Por defecto mostrar TODOS (activos + rotos) para validación
                
                // BOS/CHoCH por defecto (habilitado)
                ShowBOS = true;
                ShowBOSLabels = true;
                ShowBOSScoreLabels = true;  // Mostrar % fuerza por defecto
                TimeframeToVisualize = TimeframeOption.TF_60m;  // 1H por defecto
                MaxSwingsToShow = 200;  // Aumentado para ver más historial
                
                // Colores por defecto - Swings
                SwingHighColor = Brushes.DodgerBlue;
                SwingLowColor = Brushes.Orange;
                SwingLineThickness = 2;
                SwingLabelFontSize = 14;  // Tamaño de fuente para etiquetas de swing
                
                // Colores por defecto - BOS/CHoCH
                BOSBullishColor = Brushes.LimeGreen;
                BOSBearishColor = Brushes.Red;
                BOSLineThickness = 2;
                BOSArrowSize = 14;  // Tamaño de flecha gráfica (V dibujada en la barra)
                BOSLabelFontSize = 15;  // Tamaño de fuente para texto "BOS"/"CHoCH"
                BOSArrowSymbolSize = 32;  // Tamaño del símbolo ↑↓ (más grande que el texto)
                BOSTimeframeToVisualize = TimeframeOption.TF_60m;
                MaxBOSToShow = 200;  // Mostrar más historial
            }
            else if (State == State.Configure)
            {
                try
                {
                    Print("[ExpertIndicator] State.Configure - Inicializando...");
                    
                    // Cargar configuración temporal para saber qué TFs necesitamos
                    EngineConfig tempConfig = EngineConfig.LoadDefaults();
                    
                    int chartTF = (int)BarsPeriod.Value;
                    Print($"[ExpertIndicator] TF del gráfico: {chartTF}m");
                    
                    // Agregar DataSeries para cada TF configurado que NO sea el TF principal
                    foreach (int tfMinutes in tempConfig.TimeframesToUse)
                    {
                        if (tfMinutes == chartTF)
                        {
                            Print($"[ExpertIndicator] Saltando TF {tfMinutes}m (es el TF principal)");
                            continue;
                        }
                        
                        AddDataSeries(Instrument.FullName, new BarsPeriod 
                        { 
                            BarsPeriodType = BarsPeriodType.Minute, 
                            Value = tfMinutes 
                        });
                        Print($"[ExpertIndicator] DataSeries agregada para TF {tfMinutes}m");
                    }
                    
                    Print("[ExpertIndicator] State.Configure completado");
                }
                catch (Exception ex)
                {
                    Print($"[ExpertIndicator] ERROR en State.Configure: {ex.Message}");
                }
            }
            else if (State == State.DataLoaded)
            {
                try
                {
                    // Crear archivo de log con timestamp
                    string userDataDir = NinjaTrader.Core.Globals.UserDataDir;
                    string logDirectory = System.IO.Path.Combine(userDataDir, "PinkButterfly", "logs");
                    if (!System.IO.Directory.Exists(logDirectory))
                        System.IO.Directory.CreateDirectory(logDirectory);
                    
                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    _logFilePath = System.IO.Path.Combine(logDirectory, $"ExpertIndicator_{timestamp}.log");
                    _logWriter = new System.IO.StreamWriter(_logFilePath, true);
                    _logWriter.AutoFlush = true; // Flush inmediato para no perder trazas
                    
                    LogToFile("[ExpertIndicator] State.DataLoaded - Inicializando...");
                    LogToFile($"[ExpertIndicator] Log file: {_logFilePath}");
                    
                    // Inicializar logger (silencioso para no saturar Output)
                    _logger = new SilentLogger();
                    
                    // Cargar configuración
                    _engineConfig = EngineConfig.LoadDefaults();
                    LogToFile("[ExpertIndicator] Configuración cargada");
                    
                    // Crear BarDataProvider
                    _barProvider = new NinjaTraderBarDataProvider(this);
                    LogToFile("[ExpertIndicator] BarDataProvider creado");
                    
                    // Crear CoreEngine (sin file logging para visualización)
                    var silentFileLogger = new FileLogger(logDirectory, "visualization", _logger, false);
                    
                    _engine = new CoreEngine(_barProvider, _engineConfig, silentFileLogger);
                    _engine.Initialize();
                    
                    // Procesar barras históricas del TF visualizado para actualizar estado de swings
                    int tfMinutes = GetTFMinutes(TimeframeToVisualize);
                    int barCount = BarsArray[GetBarsInSlot(tfMinutes)].Count;
                    
                    LogToFile($"[ExpertIndicator] Procesando {barCount} barras históricas de TF={tfMinutes}m para actualizar swings...");
                    
                    for (int i = 0; i < barCount; i++)
                    {
                        _engine.OnBarClose(tfMinutes, i);
                    }
                    
                    LogToFile($"[ExpertIndicator] ✅ CoreEngine inicializado (TF visualización: {TimeframeToVisualize})");
                    LogToFile($"[ExpertIndicator] TF del gráfico: {BarsPeriod.Value}m");
                }
                catch (Exception ex)
                {
                    LogToFile($"[ExpertIndicator] ERROR CRÍTICO en inicialización: {ex.Message}");
                    LogToFile($"Stack: {ex.StackTrace}");
                }
            }
            else if (State == State.Terminated)
            {
                LogToFile("[ExpertIndicator] Terminado");
                if (_logWriter != null)
                {
                    _logWriter.Close();
                    _logWriter.Dispose();
                    _logWriter = null;
                }
            }
        }
        
        // ====================================================================
        // PROCESAMIENTO DE BARRAS
        // ====================================================================
        
        protected override void OnBarUpdate()
        {
            if (CurrentBar < 20)
                return;
            
            try
            {
                // Solo procesar al cierre de barra (no en cada tick)
                if (_engine != null && _barProvider != null)
                {
                    int tfMinutes = (int)BarsPeriod.Value;
                    int barIndex = CurrentBar;
                    
                    // Procesar barra en CoreEngine SOLO al cierre
                    _engine.OnBarClose(tfMinutes, barIndex);
                    
                    // Actualizar cache de BOS/CHoCH DESPUÉS del procesamiento
                    if (ShowBOS && CurrentBar != _lastBOSUpdateBar)
                    {
                        int bosTF = GetTFMinutes(BOSTimeframeToVisualize);
                        _cachedBOS = _engine.GetStructureBreaks(bosTF, maxCount: MaxBOSToShow);
                        _lastBOSUpdateBar = CurrentBar;
                    }
                }
            }
            catch (Exception ex)
            {
                LogToFile($"[ExpertIndicator] Error en OnBarUpdate bar={CurrentBar}: {ex.Message}");
            }
        }
        
        // ====================================================================
        // RENDERIZADO (OnRender)
        // ====================================================================
        
        protected override void OnRender(ChartControl chartControl, ChartScale chartScale)
        {
            base.OnRender(chartControl, chartScale);
            
            if (_engine == null || ChartBars == null)
            {
                LogToFile($"[ExpertIndicator][OnRender] ❌ Engine o ChartBars null");
                return;
            }
            
            // ✅ CRITICAL: Esperar a que CoreEngine construya el historial completo
            if (!_engine.IsHistoricalWindowConfigured)
            {
                LogToFile($"[ExpertIndicator][OnRender] ⏳ Esperando historial... (IsHistoricalWindowConfigured=false)");
                return;
            }
            
            try
            {
                // PASO 1: Renderizar Swings
                if (ShowSwings)
                {
                    RenderSwings(chartControl, chartScale);
                }
                
                // PASO 2: Renderizar BOS/CHoCH
                if (ShowBOS)
                {
                    RenderBOSandCHoCH(chartControl, chartScale);
                }
                
                // TODO: Próximos pasos
                // if (ShowOrderBlocks) RenderOrderBlocks(chartControl, chartScale);
                // if (ShowFVGs) RenderFVGs(chartControl, chartScale);
                // ... etc
            }
            catch (Exception ex)
            {
                LogToFile($"[ExpertIndicator] Error en OnRender: {ex.Message}");
            }
        }
        
        // ====================================================================
        // RENDERIZADO DE SWINGS
        // ====================================================================
        
        private void RenderSwings(ChartControl chartControl, ChartScale chartScale)
        {
            try
            {
                _renderCallCount++;
                bool isFirstCall = (_renderCallCount == 1);
                bool shouldLogDetails = (isFirstCall || _renderCallCount % 50 == 0); // Solo logear detalles cada 50 llamadas
                
                if (shouldLogDetails)
                {
                    LogToFile($"[ExpertIndicator][RenderSwings] ✅ Llamado #{_renderCallCount} en bar={CurrentBar}, TF={TimeframeToVisualize}");
                }
                
                // Obtener swings del timeframe seleccionado (convertir enum a int)
                int tfMinutes = (int)TimeframeToVisualize;
                var swings = _engine.GetRecentSwings(tfMinutes, MaxSwingsToShow);
                
                // 🔍 DIAGNÓSTICO: Ver cuántos swings devuelve el Core
                if (swings != null && swings.Count > 0)
                {
                    int totalSwings = swings.Count;
                    int activeSwings = swings.Count(s => s.IsActive && !s.IsBroken);
                    int brokenSwings = swings.Count(s => s.IsBroken);
                    int highSwings = swings.Count(s => s.IsHigh);
                    int lowSwings = swings.Count(s => !s.IsHigh);
                    
                    if (shouldLogDetails)
                    {
                        LogToFile($"[ExpertIndicator][DIAG_SWINGS] TF={tfMinutes}m | Pedidos={MaxSwingsToShow} | Devueltos={totalSwings} | Activos={activeSwings} | Rotos={brokenSwings} | Highs={highSwings} | Lows={lowSwings}");
                    }
                }
                else
                {
                    if (shouldLogDetails)
                    {
                        LogToFile($"[ExpertIndicator][DIAG_SWINGS] TF={tfMinutes}m | ❌ NO HAY SWINGS (null o count=0)");
                    }
                    return;
                }
                
                // Preparar recursos de renderizado
                using (var swingHighBrushBase = SwingHighColor.ToDxBrush(RenderTarget))
                using (var swingLowBrushBase = SwingLowColor.ToDxBrush(RenderTarget))
                using (var swingHighBrush = swingHighBrushBase as SharpDX.Direct2D1.SolidColorBrush)
                using (var swingLowBrush = swingLowBrushBase as SharpDX.Direct2D1.SolidColorBrush)
                using (var swingHighBrokenBrush = new SharpDX.Direct2D1.SolidColorBrush(RenderTarget, new SharpDX.Color(
                    (byte)((System.Windows.Media.SolidColorBrush)SwingHighColor).Color.R,
                    (byte)((System.Windows.Media.SolidColorBrush)SwingHighColor).Color.G,
                    (byte)((System.Windows.Media.SolidColorBrush)SwingHighColor).Color.B,
                    (byte)255)))  // 100% opacidad (sin transparencia, igual que activos)
                using (var swingLowBrokenBrush = new SharpDX.Direct2D1.SolidColorBrush(RenderTarget, new SharpDX.Color(
                    (byte)((System.Windows.Media.SolidColorBrush)SwingLowColor).Color.R,
                    (byte)((System.Windows.Media.SolidColorBrush)SwingLowColor).Color.G,
                    (byte)((System.Windows.Media.SolidColorBrush)SwingLowColor).Color.B,
                    (byte)255)))  // 100% opacidad (sin transparencia, igual que activos)
                using (var textFormat = new SharpDX.DirectWrite.TextFormat(
                    NinjaTrader.Core.Globals.DirectWriteFactory,
                    "Arial",
                    SharpDX.DirectWrite.FontWeight.Bold,
                    SharpDX.DirectWrite.FontStyle.Normal,
                    SharpDX.DirectWrite.FontStretch.Normal,
                    SwingLabelFontSize))  // Tamaño configurable
                {
                    int swingsProcessed = 0;
                    int swingsFiltered = 0;
                    int swingsOutOfRange = 0;
                    int swingsDrawn = 0;
                    
                    foreach (var swing in swings)
                    {
                        swingsProcessed++;
                        
                        // ✅ Filtrar por "Solo Activos" si está activado
                        if (ShowOnlyActiveSwings && (!swing.IsActive || swing.IsBroken))
                        {
                            swingsFiltered++;
                            continue;
                        }
                        
                        // Filtrar por tipo de swing
                        if (swing.IsHigh && !ShowSwingHighs)
                        {
                            swingsFiltered++;
                            continue;
                        }
                        if (!swing.IsHigh && !ShowSwingLows)
                        {
                            swingsFiltered++;
                            continue;
                        }
                        
                        // ✅ FIX CRÍTICO: Calcular el tiempo del PIVOT real
                        // StartTime apunta a la primera barra de la ventana izquierda (swingIndex - nLeft)
                        // El pivot real está LeftN barras después
                        int tfMinutesSwing = swing.TF;
                        DateTime pivotTime = swing.StartTime.AddMinutes(swing.LeftN * tfMinutesSwing);
                        
                        // Convertir el tiempo del pivot al índice del gráfico actual
                        int chartBarIndex = ChartBars.GetBarIdxByTime(chartControl, pivotTime);
                        
                        // 🔍 TRAZA: Ver qué índices se obtienen (solo en primera llamada)
                        if (isFirstCall)
                        {
                            string swingPrice = swing.IsHigh ? $"High={swing.High:F2}" : $"Low={swing.Low:F2}";
                            string swingState = swing.IsBroken ? "[BROKEN]" : "[ACTIVE]";
                            LogToFile($"[RenderSwings] Swing {(swing.IsHigh ? "HIGH" : "LOW")} {swingState} @ {pivotTime:yyyy-MM-dd HH:mm} | {swingPrice} | chartBarIndex={chartBarIndex}");
                        }
                        
                        if (chartBarIndex < 0)
                        {
                            swingsOutOfRange++;
                            if (isFirstCall)
                            {
                                LogToFile($"[RenderSwings] ❌ Swing fuera de rango (chartBarIndex={chartBarIndex})");
                            }
                            continue;
                        }
                        
                        int x = chartControl.GetXByBarIndex(ChartBars, chartBarIndex);
                        float y = chartScale.GetYByValue(swing.IsHigh ? swing.High : swing.Low);
                        
                        swingsDrawn++;
                        
                        // ✅ Seleccionar color según tipo Y estado (activo/roto)
                        SharpDX.Direct2D1.Brush brush;
                        if (swing.IsBroken)
                        {
                            // Swings rotos: sin transparencia
                            brush = swing.IsHigh ? swingHighBrokenBrush : swingLowBrokenBrush;
                        }
                        else
                        {
                            // Swings activos: color normal
                            brush = swing.IsHigh ? (SharpDX.Direct2D1.Brush)swingHighBrush : (SharpDX.Direct2D1.Brush)swingLowBrush;
                        }
                        
                        // ✅ Grosor variable basado en score (% fuerza)
                        float lineThickness;
                        if (swing.Score > 0.7)
                        {
                            lineThickness = SwingLineThickness * 2.0f;  // Línea gruesa (score alto)
                        }
                        else if (swing.Score >= 0.4)
                        {
                            lineThickness = SwingLineThickness;  // Línea normal
                        }
                        else
                        {
                            lineThickness = SwingLineThickness * 0.5f;  // Línea fina (score bajo)
                        }
                        
                        // Dibujar línea horizontal
                        float x1 = x;  // Comienza en la vela del swing (no antes)
                        float x2;
                        
                        if (swing.IsBroken)
                        {
                            // Swings cerrados: línea pequeña (30 pixels a la derecha)
                            x2 = x + 30;
                        }
                        else
                        {
                            // Swings activos: línea extendida hasta el borde derecho del gráfico
                            // Obtener el último bar visible en el gráfico
                            int lastVisibleBar = ChartBars.ToIndex;
                            x2 = chartControl.GetXByBarIndex(ChartBars, lastVisibleBar);
                        }
                        
                        RenderTarget.DrawLine(
                            new SharpDX.Vector2(x1, y),
                            new SharpDX.Vector2(x2, y),
                            brush,
                            lineThickness
                        );
                        
                        // Dibujar etiquetas (precio y/o score)
                        if (ShowSwingPriceLabels || ShowSwingScoreLabels)
                        {
                            // ✅ Construir etiqueta según toggles
                            string statusTag = swing.IsBroken ? " [B]" : "";
                            string priceLabel = ShowSwingPriceLabels ? $"{(swing.IsHigh ? "SH" : "SL")} {(swing.IsHigh ? swing.High : swing.Low):F2}{statusTag}" : "";
                            string scoreLabel = ShowSwingScoreLabels ? $"{(swing.Score * 100):F0}%" : "";
                            
                            // Combinar etiquetas
                            string label = "";
                            if (ShowSwingPriceLabels && ShowSwingScoreLabels)
                            {
                                // Ambos: precio arriba, score abajo
                                label = $"{priceLabel}\n{scoreLabel}";
                            }
                            else if (ShowSwingPriceLabels)
                            {
                                label = priceLabel;
                            }
                            else if (ShowSwingScoreLabels)
                            {
                                label = scoreLabel;
                            }
                            
                            if (!string.IsNullOrEmpty(label))
                            {
                                // Calcular tamaño del texto
                                var textLayout = new SharpDX.DirectWrite.TextLayout(
                                    NinjaTrader.Core.Globals.DirectWriteFactory,
                                    label,
                                    textFormat,
                                    250,
                                    60);  // Altura aumentada para 2 líneas
                                
                                float textWidth = textLayout.Metrics.Width;
                                float textHeight = textLayout.Metrics.Height;
                                textLayout.Dispose();
                                
                                // Posición del texto (ENCIMA de la línea, alineado al INICIO/IZQUIERDA)
                                float textX = x1;
                                float textY = y - textHeight - 5;  // 5 pixels arriba de la línea
                                
                                // Fondo semi-transparente
                                var bgRect = new RectangleF(textX - 2, textY - 2, textWidth + 4, textHeight + 4);
                                var bgBrush = new SharpDX.Direct2D1.SolidColorBrush(RenderTarget, new SharpDX.Color(0, 0, 0, 128));
                                RenderTarget.FillRectangle(bgRect, bgBrush);
                                bgBrush.Dispose();
                                
                                // Dibujar texto
                                RenderTarget.DrawText(
                                    label,
                                    textFormat,
                                    new RectangleF(textX, textY, 250, 60),
                                    brush
                                );
                            }
                        }
                    }
                    
                    // 📊 RESUMEN de renderizado (solo si hay detalles o si no se dibujó nada)
                    if (shouldLogDetails || swingsDrawn == 0)
                    {
                        LogToFile($"[RenderSwings] RESUMEN: Procesados={swingsProcessed}, Filtrados={swingsFiltered}, FueraDeRango={swingsOutOfRange}, Dibujados={swingsDrawn}");
                    }
                }
            }
            catch (Exception ex)
            {
                LogToFile($"[ExpertIndicator] Error en RenderSwings: {ex.Message}");
                LogToFile($"Stack: {ex.StackTrace}");
            }
        }
        
        private void RenderBOSandCHoCH(ChartControl chartControl, ChartScale chartScale)
        {
            if (!ShowBOS || _engine == null || chartControl == null)
                return;
            
            try
            {
                // Usar cache en lugar de consultar el engine cada vez
                var breaks = _cachedBOS;
                
                if (breaks == null || breaks.Count == 0)
                    return;
                
                // Log diagnóstico
                LogToFile($"[RenderBOS] Cargadas {breaks.Count} estructuras BOS/CHoCH del caché");
                
                // Crear brushes
                var bullishBrush = new SharpDX.Direct2D1.SolidColorBrush(RenderTarget, 
                    new SharpDX.Color((byte)((System.Windows.Media.Color)BOSBullishColor.GetValue(System.Windows.Media.SolidColorBrush.ColorProperty)).R,
                                     (byte)((System.Windows.Media.Color)BOSBullishColor.GetValue(System.Windows.Media.SolidColorBrush.ColorProperty)).G,
                                     (byte)((System.Windows.Media.Color)BOSBullishColor.GetValue(System.Windows.Media.SolidColorBrush.ColorProperty)).B,
                                     (byte)255));
                
                var bearishBrush = new SharpDX.Direct2D1.SolidColorBrush(RenderTarget,
                    new SharpDX.Color((byte)((System.Windows.Media.Color)BOSBearishColor.GetValue(System.Windows.Media.SolidColorBrush.ColorProperty)).R,
                                     (byte)((System.Windows.Media.Color)BOSBearishColor.GetValue(System.Windows.Media.SolidColorBrush.ColorProperty)).G,
                                     (byte)((System.Windows.Media.Color)BOSBearishColor.GetValue(System.Windows.Media.SolidColorBrush.ColorProperty)).B,
                                     (byte)255));
                
                // Crear estilo de línea discontinua
                var dashStyle = new SharpDX.Direct2D1.StrokeStyleProperties
                {
                    DashStyle = SharpDX.Direct2D1.DashStyle.Dash
                };
                var strokeStyle = new SharpDX.Direct2D1.StrokeStyle(RenderTarget.Factory, dashStyle);
                
                // Crear formato de texto
                var textFormat = new SharpDX.DirectWrite.TextFormat(
                    NinjaTrader.Core.Globals.DirectWriteFactory,
                    "Arial",
                    SharpDX.DirectWrite.FontWeight.Bold,
                    SharpDX.DirectWrite.FontStyle.Normal,
                    BOSLabelFontSize);
                
                int drawnCount = 0;
                int skippedOutOfRange = 0;
                
                foreach (var breakInfo in breaks)
                {
                    // Convertir SwingBarIndex (del TF del swing) al TF del gráfico
                    DateTime swingTime = _barProvider.GetBarTime(breakInfo.TF, breakInfo.SwingBarIndex);
                    int chartBarIndexSwing = ChartBars.GetBarIdxByTime(chartControl, swingTime);
                    
                    // Convertir tiempo de ruptura a índice del gráfico
                    int chartBarIndexBreak = ChartBars.GetBarIdxByTime(chartControl, breakInfo.StartTime);
                    
                    if (chartBarIndexSwing < 0 || chartBarIndexBreak < 0 || 
                        chartBarIndexSwing >= ChartBars.Count || chartBarIndexBreak >= ChartBars.Count)
                    {
                        skippedOutOfRange++;
                        continue;
                    }
                    
                    float xStart = chartControl.GetXByBarIndex(ChartBars, chartBarIndexSwing);
                    float xEnd = chartControl.GetXByBarIndex(ChartBars, chartBarIndexBreak);
                    
                    // Log detallado para diagnóstico
                    if (drawnCount < 5 || drawnCount >= breaks.Count - 5)  // Primeras 5 y últimas 5
                    {
                        LogToFile($"[RenderBOS] #{drawnCount} {breakInfo.BreakType} Score={breakInfo.Score:F2} " +
                                 $"BarBreak={chartBarIndexBreak} xEnd={xEnd:F0} " +
                                 $"Time={breakInfo.StartTime:HH:mm}");
                    }
                    float ySwingPrice = chartScale.GetYByValue(breakInfo.SwingPrice);
                    float yBreak = chartScale.GetYByValue(breakInfo.BreakPrice);
                    
                    // Para BOS: la línea y flecha van en la misma dirección
                    // Para CHoCH: la línea es del swing roto, la flecha indica la NUEVA dirección (opuesta)
                    bool isChoch = (breakInfo.BreakType == "CHoCH");
                    bool swingDirection = (breakInfo.Direction == "Bullish");
                    bool arrowDirection = isChoch ? !swingDirection : swingDirection;  // Invertir para CHoCH
                    
                    // Color de la línea: siempre según el swing roto
                    var lineBrush = swingDirection ? bullishBrush : bearishBrush;
                    
                    // Color de la flecha: según la nueva dirección (invertida en CHoCH)
                    var arrowBrush = arrowDirection ? bullishBrush : bearishBrush;
                    
                    // Grosor variable por score
                    float lineThickness;
                    float arrowSize;
                    if (breakInfo.Score > 0.7)
                    {
                        lineThickness = BOSLineThickness * 1.5f;
                        arrowSize = BOSArrowSize * 1.5f;
                    }
                    else if (breakInfo.Score >= 0.4)
                    {
                        lineThickness = BOSLineThickness;
                        arrowSize = BOSArrowSize;
                    }
                    else
                    {
                        lineThickness = BOSLineThickness * 0.7f;
                        arrowSize = BOSArrowSize * 0.7f;
                    }
                    
                    // 1. Dibujar línea HORIZONTAL desde swing hasta ruptura
                    // SOLID para BOS, DASHED para CHoCH
                    RenderTarget.DrawLine(
                        new SharpDX.Vector2(xStart, ySwingPrice),
                        new SharpDX.Vector2(xEnd, ySwingPrice),
                        lineBrush,  // Color del swing roto
                        lineThickness,
                        breakInfo.BreakType == "BOS" ? null : strokeStyle);
                    
                    // 2. Dibujar flecha en el punto de ruptura (al final de la línea horizontal)
                    // Solo para CHoCH (Change of Character), no para BOS
                    if (breakInfo.BreakType == "CHoCH")
                    {
                        if (arrowDirection)  // Flecha indica la NUEVA dirección
                        {
                            // Flecha hacia arriba: V invertida
                            // Línea izquierda
                            RenderTarget.DrawLine(
                                new SharpDX.Vector2(xEnd - arrowSize/2, ySwingPrice),
                                new SharpDX.Vector2(xEnd, ySwingPrice - arrowSize),
                                arrowBrush,  // Color de la nueva dirección
                                lineThickness * 1.5f);
                            // Línea derecha
                            RenderTarget.DrawLine(
                                new SharpDX.Vector2(xEnd, ySwingPrice - arrowSize),
                                new SharpDX.Vector2(xEnd + arrowSize/2, ySwingPrice),
                                arrowBrush,
                                lineThickness * 1.5f);
                        }
                        else
                        {
                            // Flecha hacia abajo: V normal
                            // Línea izquierda
                            RenderTarget.DrawLine(
                                new SharpDX.Vector2(xEnd - arrowSize/2, ySwingPrice),
                                new SharpDX.Vector2(xEnd, ySwingPrice + arrowSize),
                                arrowBrush,
                                lineThickness * 1.5f);
                            // Línea derecha
                            RenderTarget.DrawLine(
                                new SharpDX.Vector2(xEnd, ySwingPrice + arrowSize),
                                new SharpDX.Vector2(xEnd + arrowSize/2, ySwingPrice),
                                arrowBrush,
                                lineThickness * 1.5f);
                        }
                    }
                    
                    // 3. Dibujar etiquetas
                    // BOS: Solo flecha ↑↓
                    // CHoCH: "TRN" (sin flecha)
                    if (ShowBOSLabels || ShowBOSScoreLabels)
                    {
                        float currentX = xEnd + 5;
                        // Usar un offset fijo para las etiquetas, sin escalar por score
                        float labelOffset = BOSArrowSize * 1.5f;  // Offset fijo = 14 * 1.5 = 21 píxeles
                        float currentY = arrowDirection ? (ySwingPrice - labelOffset - 5) : (ySwingPrice + labelOffset + 5);
                        bool isBOS = (breakInfo.BreakType == "BOS");
                        
                        // Log para las primeras/últimas 3 estructuras
                        if (drawnCount < 3 || drawnCount >= breaks.Count - 3)
                        {
                            LogToFile($"[RenderBOS] #{drawnCount} Renderizando etiqueta: " +
                                     $"isBOS={isBOS} currentX={currentX:F0} currentY={currentY:F0} " +
                                     $"ShowLabels={ShowBOSLabels} ShowScore={ShowBOSScoreLabels}");
                        }
                        
                        // Renderizar etiqueta principal si está activado
                        if (ShowBOSLabels)
                        {
                            if (isBOS)
                            {
                                // BOS: Solo símbolo ↑↓ (sin texto "BOS")
                                string arrowSymbol = arrowDirection ? "↑" : "↓";
                                
                                var arrowFormat = new SharpDX.DirectWrite.TextFormat(
                                    NinjaTrader.Core.Globals.DirectWriteFactory,
                                    "Arial",
                                    SharpDX.DirectWrite.FontWeight.Bold,
                                    SharpDX.DirectWrite.FontStyle.Normal,
                                    BOSArrowSymbolSize);
                                
                                var arrowLayout = new SharpDX.DirectWrite.TextLayout(
                                    NinjaTrader.Core.Globals.DirectWriteFactory,
                                    arrowSymbol,
                                    arrowFormat,
                                    50,
                                    50);
                                
                                float arrowSymbolWidth = arrowLayout.Metrics.Width;
                                float arrowSymbolHeight = arrowLayout.Metrics.Height;
                                arrowLayout.Dispose();
                                
                                float arrowSymbolY = arrowDirection ? (currentY - arrowSymbolHeight) : currentY;
                                
                                // Fondo semi-transparente
                                var arrowBgRect = new RectangleF(currentX - 2, arrowSymbolY - 2, arrowSymbolWidth + 4, arrowSymbolHeight + 4);
                                var arrowBgBrush = new SharpDX.Direct2D1.SolidColorBrush(RenderTarget, new SharpDX.Color(0, 0, 0, 128));
                                RenderTarget.FillRectangle(arrowBgRect, arrowBgBrush);
                                arrowBgBrush.Dispose();
                                
                                RenderTarget.DrawText(
                                    arrowSymbol,
                                    arrowFormat,
                                    new RectangleF(currentX, arrowSymbolY, 50, 50),
                                    arrowBrush);
                                
                                arrowFormat.Dispose();
                                
                                // Actualizar Y para el score (debajo)
                                if (arrowDirection)
                                {
                                    currentY -= arrowSymbolHeight + 5;
                                }
                                else
                                {
                                    currentY += arrowSymbolHeight + 5;
                                }
                            }
                            else
                            {
                                // CHoCH: Texto "TRN" (sin flecha ↑↓)
                                string typeText = "TRN";
                                
                                var textLayout = new SharpDX.DirectWrite.TextLayout(
                                    NinjaTrader.Core.Globals.DirectWriteFactory,
                                    typeText,
                                    textFormat,
                                    100,
                                    30);
                                
                                float textWidth = textLayout.Metrics.Width;
                                float textHeight = textLayout.Metrics.Height;
                                textLayout.Dispose();
                                
                                float textY = arrowDirection ? (currentY - textHeight) : currentY;
                                
                                // Fondo semi-transparente
                                var bgRect = new RectangleF(currentX - 2, textY - 2, textWidth + 4, textHeight + 4);
                                var bgBrush = new SharpDX.Direct2D1.SolidColorBrush(RenderTarget, new SharpDX.Color(0, 0, 0, 128));
                                RenderTarget.FillRectangle(bgRect, bgBrush);
                                bgBrush.Dispose();
                                
                                RenderTarget.DrawText(
                                    typeText,
                                    textFormat,
                                    new RectangleF(currentX, textY, 100, 30),
                                    arrowBrush);
                                
                                // Actualizar Y para el score (debajo)
                                if (arrowDirection)
                                {
                                    currentY -= textHeight + 5;
                                }
                                else
                                {
                                    currentY += textHeight + 5;
                                }
                            }
                            
                            // Resetear X para el score
                            currentX = xEnd + 5;
                        }
                        
                        // Renderizar score si está activado
                        if (ShowBOSScoreLabels)
                        {
                            string scoreLabel = $"{(breakInfo.Score * 100):F0}%";
                            
                            var scoreLayout = new SharpDX.DirectWrite.TextLayout(
                                NinjaTrader.Core.Globals.DirectWriteFactory,
                                scoreLabel,
                                textFormat,
                                100,
                                30);
                            
                            float scoreWidth = scoreLayout.Metrics.Width;
                            float scoreHeight = scoreLayout.Metrics.Height;
                            scoreLayout.Dispose();
                            
                            float scoreY = arrowDirection ? (currentY - scoreHeight) : currentY;
                            
                            // Fondo semi-transparente
                            var scoreBgRect = new RectangleF(currentX - 2, scoreY - 2, scoreWidth + 4, scoreHeight + 4);
                            var scoreBgBrush = new SharpDX.Direct2D1.SolidColorBrush(RenderTarget, new SharpDX.Color(0, 0, 0, 128));
                            RenderTarget.FillRectangle(scoreBgRect, scoreBgBrush);
                            scoreBgBrush.Dispose();
                            
                            RenderTarget.DrawText(
                                scoreLabel,
                                textFormat,
                                new RectangleF(currentX, scoreY, 100, 30),
                                arrowBrush);
                        }
                    }
                    
                    drawnCount++;
                }
                
                // Log resumen
                LogToFile($"[RenderBOS] RESUMEN: Total={breaks.Count} Dibujadas={drawnCount} FueraDeRango={skippedOutOfRange}");
                
                // Limpiar recursos
                bullishBrush.Dispose();
                bearishBrush.Dispose();
                strokeStyle.Dispose();
                textFormat.Dispose();
            }
            catch (Exception ex)
            {
                LogToFile($"[ExpertIndicator] Error en RenderBOSandCHoCH: {ex.Message}");
                LogToFile($"Stack: {ex.StackTrace}");
            }
        }
        
        // ====================================================================
        // HELPERS
        // ====================================================================
        
        private int GetTFMinutes(TimeframeOption tf)
        {
            switch (tf)
            {
                case TimeframeOption.TF_5m: return 5;
                case TimeframeOption.TF_15m: return 15;
                case TimeframeOption.TF_60m: return 60;
                case TimeframeOption.TF_240m: return 240;
                case TimeframeOption.TF_1440m: return 1440;
                default: return 60;
            }
        }
        
        private int GetBarsInSlot(int tfMinutes)
        {
            // BarsArray[0] es el gráfico principal
            // BarsArray[1..N] son las series añadidas en Configure
            // Orden: 5m, 60m, 240m, 1440m (saltando el TF principal)
            
            int chartTF = BarsPeriod.Value;
            if (tfMinutes == chartTF) return 0;
            
            // Calcular slot según el orden en que se añadieron en Configure
            int slot = 0;
            int[] tfs = { 5, 15, 60, 240, 1440 };
            foreach (int tf in tfs)
            {
                if (tf == chartTF) continue; // Saltar el principal
                slot++;
                if (tf == tfMinutes) return slot;
            }
            
            return 0; // Fallback
        }
    }
}

