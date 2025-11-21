using System;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Cbi;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    public class DepuraDeterminismo : Indicator
    {
        // Core
        private CoreEngine _coreEngine;
        private DecisionEngine _decisionEngine;
        private IBarDataProvider _barDataProvider;
        private EngineConfig _config;

        // Logging
        private ILogger _logger;            // hacia Output (si lo activas)
        private FileLogger _fileLogger;     // hacia archivo DepuraDeterminismo_*.log
        private TradeLogger _tradeLogger;   // CSV de trades (opcional)

        // Trade manager (para poder consultar estado y que queden registradas decisiones)
        private TradeManager _tradeManager;

        // Índice BarsArray del TF de decisión
        private int _decisionTFIndex = 0;

        // Throttles de log
        [NinjaScriptProperty]
        [DisplayName("Enable Output Logging")]
        [Category("Logging")]
        public bool EnableOutputLogging { get; set; }

        [NinjaScriptProperty]
        [DisplayName("Enable File Logging")]
        [Category("Logging")]
        public bool EnableFileLogging { get; set; }

        [NinjaScriptProperty]
        [DisplayName("Enable Trade CSV")]
        [Category("Logging")]
        public bool EnableTradeCSV { get; set; }

        [NinjaScriptProperty]
        [DisplayName("Account Size")]
        [Category("Risk")]
        public double AccountSize { get; set; }

        [NinjaScriptProperty]
        [DisplayName("Contract Size")]
        [Category("Risk")]
        public int ContractSize { get; set; }

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Name = "DepuraDeterminismo";
                Description = "Arnés diagnóstico: no dibuja; llama al Core y traza cada paso para depurar determinismo MTF.";
                IsOverlay = true;
                Calculate = Calculate.OnEachTick;
                IsSuspendedWhileInactive = false;
                PrintTo = PrintTo.OutputTab2;

                // Defaults
                EnableOutputLogging = false;
                EnableFileLogging = true;
                EnableTradeCSV = false;

                AccountSize = 100000;
                ContractSize = 1;
            }
            else if (State == State.Configure)
            {
                // Añadir series MTF exactamente como hace ExpertTrader (desde EngineConfig)
                var tempConfig = EngineConfig.LoadDefaults();
                int chartTF = Bars.BarsPeriod.Value;

                foreach (int tfMinutes in tempConfig.TimeframesToUse)
                {
                    if (tfMinutes == chartTF)
                        continue;

                    AddDataSeries(Instrument.FullName, new BarsPeriod
                    {
                        BarsPeriodType = BarsPeriodType.Minute,
                        Value = tfMinutes
                    });
                }
            }
            else if (State == State.DataLoaded)
            {
                // 1) Cargar config
                _config = EngineConfig.LoadDefaults();

                // 2) Provider
                _barDataProvider = new NinjaTraderBarDataProvider(this);

                // 3) Loggers (mismo directorio de logs que ExpertTrader)
                string userDataDir = NinjaTrader.Core.Globals.UserDataDir;
                string logDirectory = System.IO.Path.Combine(userDataDir, "PinkButterfly", "logs");

                _logger = EnableOutputLogging ? new NinjaTraderLogger(this, LogLevel.Info) : new SilentLogger();
                _fileLogger = new FileLogger(logDirectory, "DepuraDeterminismo", _logger, EnableFileLogging);
                _tradeLogger = new TradeLogger(logDirectory, "trades", _logger, EnableTradeCSV);

                _fileLogger.Info("═══════════════════════════════════════════════════════");
                _fileLogger.Info("[HARNESS][INIT] DepuraDeterminismo iniciado");
                _fileLogger.Info($"[HARNESS][PATH] LogFile={_fileLogger.GetLogFilePath()}");
                _fileLogger.Info($"[HARNESS][CONFIG] DecisionTF={_config.DecisionTimeframeMinutes} Timeframes={string.Join(",", _config.TimeframesToUse)}");
                _fileLogger.Info("═══════════════════════════════════════════════════════");

                // 4) CoreEngine
                _coreEngine = new CoreEngine(_barDataProvider, _config, _fileLogger);
                _coreEngine.Initialize();

                // 5) DecisionEngine
                _decisionEngine = new DecisionEngine(_config, _fileLogger);
                _coreEngine.SetDecisionEngine(_decisionEngine, AccountSize);

                // 6) TradeManager
                double pointValue = Instrument.MasterInstrument.PointValue;
                _tradeManager = new TradeManager(_config, _fileLogger, _tradeLogger, ContractSize, pointValue);
                _coreEngine.SetTradeManager(_tradeManager);

                // 7) Ubicar índice BarsArray del TF de decisión
                _decisionTFIndex = 0;
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

                // 8) Progreso (como ExpertTrader)
                int totalBars = Math.Min(BarsArray[0].Count, _config.BacktestBarsForAnalysis);
                _coreEngine.StartProgressTracking(totalBars);

                _fileLogger.Info($"[HARNESS][INIT_DONE] decisionTFIndex={_decisionTFIndex} totalBars={totalBars}");
            }
            else if (State == State.Terminated)
            {
                _coreEngine?.FinishProgressTracking();
                _coreEngine?.Dispose();
                _coreEngine = null;

                if (_tradeLogger != null)
                {
                    _tradeLogger.Close();
                    _fileLogger?.Info($"[HARNESS] TradeLogger cerrado: {_tradeLogger.GetCsvFilePath()}");
                }

                if (_fileLogger != null)
                {
                    _fileLogger.Close();
                    Print($"[HARNESS] FileLogger cerrado: {_fileLogger.GetLogFilePath()}");
                }
            }
        }

        protected override void OnBarUpdate()
        {
            try
            {
                int bip = BarsInProgress;
                int tfMinutes = GetMinutesFromBarsPeriod(BarsArray[bip].BarsPeriod);
                int barIndex = CurrentBars[bip];

                // Tiempo de la barra desde el provider (independiente del TF del gráfico)
                DateTime barTime = _barDataProvider.GetBarTime(tfMinutes, barIndex);

                // Entrada al arnés
                _fileLogger.Info($"[HARNESS][ENTRY] BIP={bip} TF={tfMinutes} Bar={barIndex} Time={barTime:yyyy-MM-dd HH:mm}");

                // Estado de series disponibles (conteo)
                if (barIndex % 100 == 0 || tfMinutes == _config.DecisionTimeframeMinutes)
                {
                    string counts = string.Join(", ",
                        _config.TimeframesToUse.Select(tf =>
                            $"TF{tf}={_barDataProvider.GetCurrentBarIndex(tf) + 1}"));
                    _fileLogger.Info($"[HARNESS][COUNTS] {counts}");
                }

                // Llamada directa al core (el core hace gating/ventana/scheduler)
                _coreEngine.OnBarClose(tfMinutes, barIndex);

                // Snapshot mínimo tras la llamada
                if (tfMinutes == _config.DecisionTimeframeMinutes)
                {
                    var all = _tradeManager.GetAllTrades();
                    int pending = all.Count(t => t.Status == TradeStatus.PENDING);
                    int active = all.Count(t => t.Status == TradeStatus.EXECUTED);
                    int closed = all.Count(t => t.Status == TradeStatus.TP_HIT || t.Status == TradeStatus.SL_HIT || t.Status == TradeStatus.CANCELLED);

                    _fileLogger.Info($"[HARNESS][STATE] TF={tfMinutes} Bar={barIndex} Pending={pending} Active={active} Closed={closed} Bias={_coreEngine.CurrentMarketBias}");
                }

                // Contadores de ADAPTIVE_* (si aparecen, el log del core ya los imprime)
                // Aquí solo añadimos un eco por cada N barras para saber que se está avanzando
                if (barIndex % 250 == 0)
                    _fileLogger.Info($"[HARNESS][PROGRESS] TF={tfMinutes} Bar={barIndex}");
            }
            catch (Exception ex)
            {
                Print($"[HARNESS] ERROR OnBarUpdate: {ex.Message}");
                _fileLogger?.Exception("[HARNESS] OnBarUpdate exception", ex);
            }
        }

        private int GetMinutesFromBarsPeriod(BarsPeriod period)
        {
            switch (period.BarsPeriodType)
            {
                case BarsPeriodType.Minute: return period.Value;
                case BarsPeriodType.Day:    return period.Value * 1440;
                case BarsPeriodType.Week:   return period.Value * 10080;
                case BarsPeriodType.Month:  return period.Value * 43200;
                default: return Bars.BarsPeriod.Value;
            }
        }
    }
}