// ============================================================================
// CoreBrainIndicator.cs
// PinkButterfly CoreBrain - NinjaTrader 8 Wrapper/Indicador
// 
// Indicador invisible que expone el motor CoreBrain como Singleton
// Otros indicadores/estrategias acceden vía CoreBrainIndicator.Instance
//
// FASE 1 (Skeleton): Compila el motor, no hace nada funcional todavía
// FASE 3 (Completo): Implementará IBarDataProvider y multi-timeframe
// ============================================================================

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// PinkButterfly CoreBrain - Motor analítico profesional multi-timeframe
    /// 
    /// Este indicador es INVISIBLE (no dibuja nada en el gráfico)
    /// Actúa como motor central que otros indicadores/estrategias consumen vía Instance
    /// 
    /// ESTADO ACTUAL: Skeleton compilable - Fase 3 implementará funcionalidad completa
    /// </summary>
    public class CoreBrainIndicator : Indicator
    {
        // ========================================================================
        // SINGLETON INSTANCE
        // ========================================================================
        
        /// <summary>
        /// Instancia singleton del CoreBrain
        /// Otros indicadores acceden vía: CoreBrainIndicator.Instance
        /// </summary>
        public static CoreBrainIndicator Instance { get; private set; }

        // ========================================================================
        // CAMPOS PRIVADOS
        // ========================================================================

        private CoreEngine _engine;
        private EngineConfig _config;

        // ========================================================================
        // PROPIEDADES CONFIGURABLES (expuestas en UI de NinjaTrader)
        // ========================================================================

        [NinjaScriptProperty]
        [Display(Name = "Enable Debug", Description = "Activa logs de debug en Output window", Order = 1, GroupName = "Configuración")]
        public bool EnableDebug { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Max Structures Per TF", Description = "Máximo número de estructuras por timeframe", Order = 2, GroupName = "Configuración")]
        [Range(100, 2000)]
        public int MaxStructuresPerTF { get; set; }

        // ========================================================================
        // LIFECYCLE NINJATRADER
        // ========================================================================

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                // ============================================================
                // CONFIGURACIÓN DEL INDICADOR
                // ============================================================
                
                Description = @"PinkButterfly CoreBrain - Motor analítico profesional multi-timeframe. Detecta FVGs, Swings, Order Blocks, BOS/CHoCH, POI y más. Indicador invisible que expone API para consumo.";
                Name = "CoreBrain";
                Calculate = Calculate.OnBarClose;
                IsOverlay = true;
                DisplayInDataBox = false;
                DrawOnPricePanel = true;
                PaintPriceMarkers = false;
                ScaleJustification = ScaleJustification.Right;
                IsSuspendedWhileInactive = false; // Importante: mantener activo siempre

                // Configurar output a Output Tab 2
                PrintTo = PrintTo.OutputTab2;

                // Defaults de configuración
                EnableDebug = false;
                MaxStructuresPerTF = 500;
            }
            else if (State == State.Configure)
            {
                // ============================================================
                // FASE 3: Aquí se configurarán los multi-timeframes
                // TODO: AddDataSeries() para cada TF en EngineConfig.TimeframesToUse
                // ============================================================
                
                Print("CoreBrain: State.Configure");
            }
            else if (State == State.DataLoaded)
            {
                // ============================================================
                // INICIALIZACIÓN DEL MOTOR
                // ============================================================
                
                try
                {
                    Print("CoreBrain: Inicializando motor...");

                    // Crear configuración
                    _config = EngineConfig.LoadDefaults();
                    _config.EnableDebug = EnableDebug;
                    _config.MaxStructuresPerTF = MaxStructuresPerTF;

                    // FASE 3: Aquí crearemos el IBarDataProvider real
                    // Por ahora, el motor no se inicializa (falta provider)
                    // _engine = new CoreEngine(provider, _config, new NinjaTraderLogger(this));
                    // _engine.Initialize();

                    // Registrar singleton
                    Instance = this;

                    Print($"CoreBrain: Inicializado (Skeleton - Fase 1)");
                    Print($"CoreBrain: Configuración - EnableDebug={EnableDebug}, MaxStructures={MaxStructuresPerTF}");
                }
                catch (Exception ex)
                {
                    Print($"ERROR CoreBrain: {ex.Message}");
                }
            }
            else if (State == State.Terminated)
            {
                // ============================================================
                // LIMPIEZA
                // ============================================================
                
                if (_engine != null)
                {
                    Print("CoreBrain: Disposing engine...");
                    _engine.Dispose();
                    _engine = null;
                }

                if (Instance == this)
                {
                    Instance = null;
                }

                Print("CoreBrain: Terminated");
            }
        }

        protected override void OnBarUpdate()
        {
            // ============================================================
            // FASE 3: Aquí se llamará a _engine.OnBarClose() por cada TF
            // ============================================================
            
            // Por ahora no hace nada (skeleton)
            
            // TODO Fase 3:
            // if (_engine != null && _engine.IsInitialized)
            // {
            //     int tfMinutes = GetTimeframeMinutes(BarsInProgress);
            //     _engine.OnBarClose(tfMinutes, CurrentBar);
            // }
        }

        // ========================================================================
        // API PÚBLICA (para consumo desde otros indicadores)
        // ========================================================================

        /// <summary>
        /// Obtiene el motor CoreEngine (si está inicializado)
        /// Uso desde otros indicadores:
        /// var core = CoreBrainIndicator.Instance?.GetEngine();
        /// if (core != null) { ... }
        /// </summary>
        public CoreEngine GetEngine()
        {
            return _engine;
        }

        /// <summary>
        /// Verifica si el motor está listo para operar
        /// </summary>
        public bool IsEngineReady()
        {
            return _engine != null && _engine.IsInitialized;
        }

        // ========================================================================
        // MÉTODOS DE UTILIDAD
        // ========================================================================

        /// <summary>
        /// Convierte BarsInProgress a minutos del timeframe
        /// TODO Fase 3: Implementar mapeo correcto multi-TF
        /// </summary>
        private int GetTimeframeMinutes(int barsInProgress)
        {
            // Placeholder - Fase 3 implementará correctamente
            return 60; 
        }
    }

    // ============================================================================
    // LOGGER PARA NINJATRADER
    // ============================================================================

    /// <summary>
    /// Implementación de ILogger que escribe en NinjaTrader Output window
    /// </summary>
    public class NinjaTraderLogger : ILogger
    {
        private readonly Indicator _indicator;
        public LogLevel MinLevel { get; set; }

        public NinjaTraderLogger(Indicator indicator, LogLevel minLevel = LogLevel.Info)
        {
            _indicator = indicator;
            MinLevel = minLevel;
        }

        public void Debug(string message)
        {
            if (MinLevel <= LogLevel.Debug)
                _indicator.Print($"[DEBUG] {message}");
        }

        public void Info(string message)
        {
            if (MinLevel <= LogLevel.Info)
                _indicator.Print($"[INFO] {message}");
        }

        public void Warning(string message)
        {
            if (MinLevel <= LogLevel.Warning)
                _indicator.Print($"[WARN] {message}");
        }

        public void Error(string message)
        {
            if (MinLevel <= LogLevel.Error)
            {
                _indicator.Print($"[ERROR] {message}");
            }
        }

        public void Exception(string message, Exception exception)
        {
            if (MinLevel <= LogLevel.Error)
            {
                _indicator.Print($"[EXCEPTION] {message}: {exception.Message}");
                _indicator.Print($"  Stack: {exception.StackTrace}");
            }
        }
    }
}

// ============================================================================
// NOTAS DE IMPLEMENTACIÓN
// ============================================================================
// 
// FASE 1 (ACTUAL): Skeleton compilable
// - Indicador se carga en NinjaTrader sin errores
// - Expone Instance singleton
// - Logs de estado en Output window
// - Motor NO se inicializa todavía (falta IBarDataProvider)
//
// FASE 3 (PRÓXIMA): Integración completa
// - Implementar NinjaTraderBarDataProvider
// - Configurar multi-timeframes con AddDataSeries()
// - Inicializar motor CoreEngine
// - Llamar OnBarClose() por cada TF
// - Gestionar mapeo de BarsInProgress a timeframes
// - Implementar sincronización de barras multi-TF
//
// CONSUMO DESDE OTROS INDICADORES:
// ---------------------------------
// var core = CoreBrainIndicator.Instance;
// if (core != null && core.IsEngineReady())
// {
//     var engine = core.GetEngine();
//     var fvgs = engine.GetActiveFVGs(60, minScore: 0.3);
//     foreach (var fvg in fvgs)
//     {
//         // Dibujar o usar datos...
//     }
// }
//
// ============================================================================

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private PinkButterfly.CoreBrainIndicator[] cacheCoreBrainIndicator;
		public PinkButterfly.CoreBrainIndicator CoreBrainIndicator(bool enableDebug, int maxStructuresPerTF)
		{
			return CoreBrainIndicator(Input, enableDebug, maxStructuresPerTF);
		}

		public PinkButterfly.CoreBrainIndicator CoreBrainIndicator(ISeries<double> input, bool enableDebug, int maxStructuresPerTF)
		{
			if (cacheCoreBrainIndicator != null)
				for (int idx = 0; idx < cacheCoreBrainIndicator.Length; idx++)
					if (cacheCoreBrainIndicator[idx] != null && cacheCoreBrainIndicator[idx].EnableDebug == enableDebug && cacheCoreBrainIndicator[idx].MaxStructuresPerTF == maxStructuresPerTF && cacheCoreBrainIndicator[idx].EqualsInput(input))
						return cacheCoreBrainIndicator[idx];
			return CacheIndicator<PinkButterfly.CoreBrainIndicator>(new PinkButterfly.CoreBrainIndicator(){ EnableDebug = enableDebug, MaxStructuresPerTF = maxStructuresPerTF }, input, ref cacheCoreBrainIndicator);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.PinkButterfly.CoreBrainIndicator CoreBrainIndicator(bool enableDebug, int maxStructuresPerTF)
		{
			return indicator.CoreBrainIndicator(Input, enableDebug, maxStructuresPerTF);
		}

		public Indicators.PinkButterfly.CoreBrainIndicator CoreBrainIndicator(ISeries<double> input , bool enableDebug, int maxStructuresPerTF)
		{
			return indicator.CoreBrainIndicator(input, enableDebug, maxStructuresPerTF);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.PinkButterfly.CoreBrainIndicator CoreBrainIndicator(bool enableDebug, int maxStructuresPerTF)
		{
			return indicator.CoreBrainIndicator(Input, enableDebug, maxStructuresPerTF);
		}

		public Indicators.PinkButterfly.CoreBrainIndicator CoreBrainIndicator(ISeries<double> input , bool enableDebug, int maxStructuresPerTF)
		{
			return indicator.CoreBrainIndicator(input, enableDebug, maxStructuresPerTF);
		}
	}
}

#endregion
