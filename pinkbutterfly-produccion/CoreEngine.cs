// ============================================================================
// CoreEngine.cs
// PinkButterfly CoreBrain - Motor principal del sistema
// 
// Orquestador central que:
// - Gestiona el estado de todas las estructuras (thread-safe con ReaderWriterLockSlim)
// - Coordina los detectores (FVG, Swing, OB, BOS, POI, etc.)
// - Calcula scores dinámicos multi-dimensionales
// - Expone API pública para consultas
// - Gestiona persistencia asíncrona con debounce
// - Mantiene índices espaciales (IntervalTree) por timeframe
//
// IMPORTANTE: Este motor es POCO (Plain Old C# Object) - SIN dependencias de NinjaTrader
// Toda interacción con datos de barras se hace a través de IBarDataProvider
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Motor principal del CoreBrain
    /// Thread-safe, modular, testeable y migrable a servicios externos
    /// </summary>
    public class CoreEngine : IDisposable
    {
        // ========================================================================
        // CAMPOS PRIVADOS - STATE MANAGEMENT
        // ========================================================================

        /// <summary>Lock para proteger acceso concurrente al estado</summary>
        private readonly ReaderWriterLockSlim _stateLock = new ReaderWriterLockSlim();

        /// <summary>Estructuras indexadas por timeframe (minutos) -> lista ordenada por CreatedAtBarIndex</summary>
        private readonly Dictionary<int, List<StructureBase>> _structuresListByTF;

        /// <summary>Índices espaciales por timeframe para consultas de overlap O(log n + k)</summary>
        private readonly Dictionary<int, IntervalTree<StructureBase>> _intervalTreesByTF;

        /// <summary>Diccionario rápido de estructuras por ID para GetStructureById()</summary>
        private readonly Dictionary<string, StructureBase> _structuresById;

        /// <summary>Lista de detectores activos (inyectados)</summary>
        private readonly List<IDetector> _detectors;

        /// <summary>Proveedor de datos de barras (implementado por wrapper NinjaTrader o mock)</summary>
        private readonly IBarDataProvider _provider;

        /// <summary>Configuración del motor</summary>
        private readonly EngineConfig _config;

        /// <summary>Logger para debugging y errores</summary>
        private readonly ILogger _logger;

        /// <summary>Motor de scoring para cálculo de puntuaciones</summary>
        private readonly ScoringEngine _scoringEngine;

        /// <summary>Gestor de persistencia para save/load JSON</summary>
        private readonly PersistenceManager _persistenceManager;

        /// <summary>Marca si ha habido cambios desde el último guardado</summary>
        private volatile bool _stateChanged;

        /// <summary>Tarea de guardado asíncrono actual (para debounce)</summary>
        private Task _saveTask;

        /// <summary>Token de cancelación para guardado asíncrono</summary>
        private CancellationTokenSource _saveCancellationTokenSource;

        /// <summary>Timestamp del último guardado</summary>
        private DateTime _lastSaveTime;

        /// <summary>Estadísticas del motor (detecciones, purgas, performance)</summary>
        private EngineStats _stats;

        /// <summary>
        /// Bias de mercado actual: "Bullish", "Bearish", "Neutral"
        /// Actualizado por BOSDetector basado en breaks recientes
        /// </summary>
        private string _currentMarketBias = "Neutral";

        /// <summary>Indica si el motor ha sido inicializado</summary>
        private bool _isInitialized;

        /// <summary>Indica si el motor está disposed</summary>
        private bool _isDisposed;

        /// <summary>Tracker de progreso para procesamiento histórico</summary>
        private ProgressTracker _progressTracker;

        /// <summary>Contador de guardados realizados (para reporte de progreso)</summary>
        private int _saveCounter;

        /// <summary>Contador de muestreo para trazas de proximidad [DIAG][PROX]</summary>
        private int _proxDiagSampleCounter;

        // ========================================================================
        // VENTANA HISTÓRICA DETERMINISTA (V6.0i.7+)
        // ========================================================================
        
        /// <summary>Indica si la ventana histórica ya fue configurada</summary>
        private bool _windowConfigured;
        
        /// <summary>Indica si estamos en modo replay histórico (solo actualizar detectores base, sin pipeline decisiones)</summary>
        private bool _isReplay = false;
        
        /// <summary>Índice inicial (skip) por TF para procesamiento histórico</summary>
        private readonly Dictionary<int, int> _barsToSkipPerTF = new Dictionary<int, int>();
        
        /// <summary>Índice final (end) por TF para procesamiento histórico</summary>
        private readonly Dictionary<int, int> _barsEndPerTF = new Dictionary<int, int>();
        
        /// <summary>Último índice procesado por TF (para catch-up multi-TF)</summary>
        private readonly Dictionary<int, int> _lastProcessedBarByTF = new Dictionary<int, int>();
        
        /// <summary>Estabilización multi-TF - último total observado por TF</summary>
        private readonly Dictionary<int, int> _lastTotalByTF = new Dictionary<int, int>();
        
        /// <summary>Estabilización multi-TF - contador de invocaciones consecutivas con mismo total por TF</summary>
        private readonly Dictionary<int, int> _stableCountByTF = new Dictionary<int, int>();

        /// <summary>Referencia al DecisionEngine para replay de decisiones históricas (inyectado desde ExpertTrader)</summary>
        private dynamic _decisionEngine;
        
        /// <summary>Tamaño de cuenta para replay de decisiones históricas (inyectado desde ExpertTrader)</summary>
        private double _accountSize;
        
        /// <summary>Referencia al TradeManager para entrega de decisiones durante replay (inyectado desde ExpertTrader)</summary>
        private TradeManager _tradeManager;

        // ========================================================================
        // EVENTOS PÚBLICOS
        // ========================================================================

        /// <summary>
        /// Evento disparado cuando se agrega una nueva estructura
        /// Proporciona información detallada sobre la estructura añadida, TF, bar index y detector
        /// </summary>
        public event EventHandler<StructureAddedEventArgs> OnStructureAdded;

        /// <summary>
        /// Evento disparado cuando se actualiza una estructura existente
        /// Proporciona información sobre el tipo de actualización y cambios de score
        /// </summary>
        public event EventHandler<StructureUpdatedEventArgs> OnStructureUpdated;

        /// <summary>
        /// Evento disparado cuando se elimina una estructura
        /// Proporciona información sobre la estructura eliminada y la razón de eliminación
        /// </summary>
        public event EventHandler<StructureRemovedEventArgs> OnStructureRemoved;

        // ========================================================================
        // PROPIEDADES PÚBLICAS
        // ========================================================================

        /// <summary>Configuración del motor (solo lectura)</summary>
        public EngineConfig Config => _config;

        /// <summary>Bias de mercado actual</summary>
        public string CurrentMarketBias
        {
            get
            {
                _stateLock.EnterReadLock();
                try
                {
                    return _currentMarketBias;
                }
                finally
                {
                    _stateLock.ExitReadLock();
                }
            }
        }

        /// <summary>Indica si el motor está inicializado y listo para operar</summary>
        public bool IsInitialized => _isInitialized;

        /// <summary>Indica si el motor está en modo estático (no procesa nuevas barras, solo sirve estructuras cargadas)</summary>
        private bool _isStaticMode = false;
        
        /// <summary>Modo estático: Si es true, OnBarClose() no hace nada (estructuras cargadas desde JSON)</summary>
        public bool IsStaticMode => _isStaticMode;

        /// <summary>Indica si la ventana histórica determinista ya fue configurada</summary>
        public bool IsHistoricalWindowConfigured => _windowConfigured;
        
        /// <summary>Indica si estamos en modo replay histórico (solo detectores base, sin pipeline decisiones)</summary>
        public bool IsInReplayMode => _isReplay;

        /// <summary>
        /// Verifica si una barra está dentro de la ventana histórica configurada para un TF dado
        /// </summary>
        public bool IsBarInHistoricalWindow(int tfMinutes, int barIndex)
        {
            if (!_windowConfigured)
                return false;

            if (_barsToSkipPerTF.TryGetValue(tfMinutes, out int skip))
            {
                // Permitir procesar barras >= skip (sin límite superior)
                // El límite superior (end) solo se usa durante el replay en BuildHistoricalState
                return barIndex >= skip;
            }

            // Si no hay skip configurado para este TF, considerarlo dentro
            return true;
        }

        /// <summary>Número total de estructuras en memoria</summary>
        public int TotalStructureCount
        {
            get
            {
                _stateLock.EnterReadLock();
                try
                {
                    return _structuresById.Count;
                }
                finally
                {
                    _stateLock.ExitReadLock();
                }
            }
        }

        // ========================================================================
        // CONSTRUCTOR
        // ========================================================================

        /// <summary>
        /// Constructor del CoreEngine
        /// </summary>
        /// <param name="provider">Proveedor de datos de barras (implementa IBarDataProvider)</param>
        /// <param name="config">Configuración del motor</param>
        /// <param name="logger">Logger (opcional, usa ConsoleLogger si es null)</param>
        public CoreEngine(IBarDataProvider provider, EngineConfig config, ILogger logger = null)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? new ConsoleLogger();

            // Validar configuración
            _config.Validate();

            // Inicializar estructuras de datos
            _structuresListByTF = new Dictionary<int, List<StructureBase>>();
            _intervalTreesByTF = new Dictionary<int, IntervalTree<StructureBase>>();
            _structuresById = new Dictionary<string, StructureBase>();
            _detectors = new List<IDetector>();

            // Inicializar estructuras por cada timeframe configurado
            foreach (var tf in _config.TimeframesToUse)
            {
                _structuresListByTF[tf] = new List<StructureBase>();
                _intervalTreesByTF[tf] = new IntervalTree<StructureBase>();
                _lastProcessedBarByTF[tf] = -1;
            }

            _lastSaveTime = DateTime.UtcNow;
            _saveCancellationTokenSource = new CancellationTokenSource();

            // Inicializar motor de scoring
            _scoringEngine = new ScoringEngine(_config, _provider, _logger);

            // Inicializar gestor de persistencia
            _persistenceManager = new PersistenceManager(_config, _logger);

            // Inicializar estadísticas
            _stats = new EngineStats
            {
                EngineVersion = _config.EngineVersion,
                Instrument = "Unknown", // Se actualizará en Initialize()
                IsInitialized = false
            };

            _logger.Info($"CoreEngine creado con {_config.TimeframesToUse.Count} timeframes: " +
                        $"[{string.Join(", ", _config.TimeframesToUse)}]");
        }

        /// <summary>
        /// Inyecta el DecisionEngine para permitir el replay de decisiones históricas.
        /// Llamado desde ExpertTrader después de crear el DecisionEngine.
        /// </summary>
        /// <param name="decisionEngine">Instancia del DecisionEngine</param>
        /// <param name="accountSize">Tamaño de la cuenta para cálculo de posiciones</param>
        public void SetDecisionEngine(object decisionEngine, double accountSize)
        {
            _decisionEngine = decisionEngine;
            _accountSize = accountSize;
            _logger?.Info($"[CoreEngine] DecisionEngine inyectado (AccountSize={accountSize:F0})");
        }
        
        /// <summary>
        /// Inyecta el TradeManager para que las decisiones del replay histórico se procesen.
        /// </summary>
        /// <param name="tradeManager">Instancia del TradeManager</param>
        public void SetTradeManager(TradeManager tradeManager)
        {
            _tradeManager = tradeManager;
            _logger?.Info($"[CoreEngine] TradeManager inyectado");
        }

        // ========================================================================
        // INICIALIZACIÓN
        // ========================================================================

        /// <summary>
        /// Inicializa el motor:
        /// - Configura detectores
        /// - Carga estado persistido (si existe y configuración coincide)
        /// - Prepara índices
        /// 
        /// Debe llamarse una vez después del constructor antes de usar el motor
        /// </summary>
        public void Initialize()
        {
            if (_isInitialized)
            {
                _logger.Warning("Initialize() llamado múltiples veces - ignorando");
                return;
            }

            _logger.Info("Inicializando CoreEngine...");

            try
            {
                // Inicializar detectores
                InitializeDetectors();

                // Fingerprint de configuración (hash corto + flags críticos)
                try
                {
                    string cfgHash = _config.GetHash();
                    string shortHash = !string.IsNullOrEmpty(cfgHash) && cfgHash.Length >= 8 ? cfgHash.Substring(0, 8) : cfgHash;
                    _logger.Info($"[CFG] Hash={shortHash} ProxSrc={_config.ProximityPriceSource} NearestEdge={_config.UseNearestEdgeForFVGProximity} HardCut={_config.EnableProximityHardCut} DFMHardCut={_config.EnableProximityHardCutInDFM} CxlCtxBias={_config.UseContextBiasForCancellations} DirPolicy={_config.EnforceDirectionalPolicy} Purge(MinTh={_config.MinScoreThreshold:F2},MaxTF={_config.MaxStructuresPerTF},Age={_config.MaxAgeBarsForPurge}) Age200={_config.EnableFVGAgePenalty200} TFBonus={_config.EnableFVGTFBonus} FVGDeleg={_config.EnableFVGDelegatedScoring} LGNoDecay={_config.EnableLGConfirmedNoDecayBonus} RiskAgeBypass={_config.EnableRiskAgeBypassForDiagnostics} AgeRelax={_config.AgeFilterRelaxMultiplier:F2} Weights(Core={_config.Weight_CoreScore:F2},Prox={_config.Weight_Proximity:F2},Conf={_config.Weight_Confluence:F2},Bias={_config.Weight_Bias:F2}) ProxThrATR={_config.ProximityThresholdATR:F2} MinProx={_config.MinProximityForEntry:F2}");
                }
                catch {}

                // ========================================================================
                // FAST LOAD: Cargar estado desde JSON si está habilitado
                // ========================================================================
                if (_config.EnableFastLoadFromJSON)
                {
                    _logger.Info("═══════════════════════════════════════════════════════");
                    _logger.Info("⚡ FAST LOAD MODE ACTIVADO (Solo DFM)");
                    _logger.Info("═══════════════════════════════════════════════════════");
                    
                    try
                    {
                        _logger.Info($"[FAST LOAD] Intentando cargar desde: {_config.StateFilePath}");
                        var startTime = DateTime.UtcNow;
                        
                        // LoadStateFromJSON maneja la expansión de ruta internamente
                        LoadStateFromJSON(_config.StateFilePath, true);
                        
                        var loadTime = (DateTime.UtcNow - startTime).TotalSeconds;
                        
                        _logger.Info($"[FAST LOAD] ✅ Estructuras cargadas en {loadTime:F2} segundos");
                        _logger.Info($"[FAST LOAD] Total estructuras: {TotalStructureCount}");
                        _logger.Info($"[FAST LOAD] CoreEngine en MODO ESTÁTICO (no procesará nuevas barras)");
                        _logger.Info($"[FAST LOAD] DecisionEngine se ejecutará normalmente sobre estructuras cargadas");
                        _logger.Info("═══════════════════════════════════════════════════════");
                        
                        // Activar modo estático: no procesar nuevas barras
                        SetStaticMode(true);
                    }
                    catch (FileNotFoundException ex)
                    {
                        _logger.Warning($"[FAST LOAD] ⚠️ Archivo no encontrado: {ex.Message}");
                        _logger.Warning("[FAST LOAD] Ejecuta primero con Fast Load desactivado para generar el archivo");
                        _logger.Warning("[FAST LOAD] Continuando con procesamiento normal...");
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"[FAST LOAD] ❌ Error cargando estado: {ex.Message}");
                        _logger.Warning("[FAST LOAD] Continuando sin estado previo (procesamiento normal)");
                    }
                }

                _isInitialized = true;
                _logger.Info("CoreEngine inicializado correctamente");
            }
            catch (Exception ex)
            {
                _logger.Exception("Error durante inicialización del CoreEngine", ex);
                throw;
            }
        }

        /// <summary>
        /// Configura la ventana histórica cuando los totales están estables y suficientes.
        /// Llamado automáticamente por OnBarClose hasta que se complete la configuración.
        /// CRÍTICO MTF: Autodetecta estabilización de TODOS los TFs para garantizar independencia del TF base del gráfico.
        /// </summary>
        private void MaybeConfigureHistoricalWindow(IBarDataProvider barData)
        {
            // PASO 1: Estabilizar TODOS los TFs (no solo el de decisión)
            bool allStable = true;
            foreach (int tfMinutes in _config.TimeframesToUse)
            {
                int total = barData.GetCurrentBarIndex(tfMinutes) + 1;
                if (total <= 0)
                {
                    allStable = false;
                    continue;
                }

                // Detectar estabilización del total por TF (evita anclar con totales "en carga")
                if (!_lastTotalByTF.ContainsKey(tfMinutes))
                    _lastTotalByTF[tfMinutes] = -1;
                if (!_stableCountByTF.ContainsKey(tfMinutes))
                    _stableCountByTF[tfMinutes] = 0;

                if (total == _lastTotalByTF[tfMinutes])
                {
                    _stableCountByTF[tfMinutes]++;
                }
                else
                {
                    _lastTotalByTF[tfMinutes] = total;
                    _stableCountByTF[tfMinutes] = 1;
                }

                // Telemetría de estabilización por TF
                if (_stableCountByTF[tfMinutes] == 1)
                    _logger?.Debug($"[ANCHOR_STABILITY] TF={tfMinutes}m total={total} stable=1");

                // Requerir total estable durante 2 invocaciones para CADA TF
                if (_stableCountByTF[tfMinutes] < 2)
                {
                    allStable = false;
                }
            }

            // PASO 2: Solo anclar cuando TODOS los TFs estén estables
            if (!allStable)
                return;

            // Loguear los totales estabilizados (para validación MTF)
            var stabilityInfo = string.Join(", ", _config.TimeframesToUse.Select(tf => 
                $"{tf}m={_lastTotalByTF[tf]}(x{_stableCountByTF[tf]})"));
            _logger.Info($"[ANCHOR_FINAL] Todos los TFs estables: {stabilityInfo}");

            // PASO 3: Anclar por TIEMPO COMÚN (no por totales)
            int decisionTF = _config.DecisionTimeframeMinutes;
            int totalDecision = _lastTotalByTF[decisionTF];
            
            // 1) Calcular anchor final común (mínimo lastTime de TODOS los TF) y REDONDEARLO hacia abajo a 60m
            DateTime anchorEnd = DateTime.MaxValue;
            foreach (int tf in _config.TimeframesToUse)
            {
                int tot = _lastTotalByTF[tf];
                if (tot <= 0) continue;
                DateTime lastT = barData.GetBarTime(tf, tot - 1);
                if (lastT < anchorEnd) anchorEnd = lastT;
            }
            // Floor a 60m: garantiza corte idéntico entre runs con el mismo dataset
            int roundToMins = 60;
            var flooredMinutes = (anchorEnd.Minute / roundToMins) * roundToMins;
            DateTime anchorTime = new DateTime(anchorEnd.Year, anchorEnd.Month, anchorEnd.Day, anchorEnd.Hour, flooredMinutes, 0, anchorEnd.Kind);

            _logger.Info($"[ANCHOR] ⏱ anchorEnd={anchorTime:O} barsForAnalysis={_config.BacktestBarsForAnalysis}");

            // 2) Construir ventana por TF calculando barras necesarias, dejando margen incremental
            // Calcular margen incremental en el TF de decisión
            int incMarginDec = Math.Max(
                _config.MinIncrementalBarsAfterReplay,
                (int)Math.Round(_config.BacktestBarsForAnalysis * _config.IncrementalMarginRatio)
            );
            
            _logger.Info($"[WINDOW_CALC] Margen incremental TF decisión: {incMarginDec} barras ({_config.IncrementalMarginRatio:P0} de {_config.BacktestBarsForAnalysis})");
            
            DateTime latestLastTime = DateTime.MinValue;
            foreach (int tfMinutes in _config.TimeframesToUse)
            {
                int total = _lastTotalByTF[tfMinutes];
                if (total <= 0)
                    continue;

                // Proyectar anchorEnd a índice de este TF
                int endIdx = barData.GetBarIndexFromTime(tfMinutes, anchorTime);
                if (endIdx < 0)
                    endIdx = Math.Max(0, total - 1);
                
                // Calcular barras necesarias en este TF (proporcional al TF de decisión)
                int barsNeeded = Math.Max(1, (int)Math.Ceiling(_config.BacktestBarsForAnalysis * (double)decisionTF / tfMinutes));
                
                // Calcular margen incremental proporcional para este TF
                int incMarginTF = (int)Math.Round(incMarginDec * (double)decisionTF / tfMinutes);
                
                // Restar margen incremental de endIdx para dejar barras disponibles
                endIdx = Math.Max(0, endIdx - incMarginTF);
                
                // Restar barras desde endIdx (NO usar GetBarIndexFromTime con tiempo antiguo)
                int startIdx = Math.Max(0, endIdx - (barsNeeded - 1));

                // Asegurar que end >= start
                endIdx = Math.Max(startIdx, endIdx);

                // Aplicar padding para TF60 (bias histórico)
                if (tfMinutes == 60)
                {
                    int pad = Math.Max(0, _config.BiasPaddingBars60);
                    startIdx = Math.Max(0, startIdx - pad);
                }

                _barsToSkipPerTF[tfMinutes] = startIdx;
                _barsEndPerTF[tfMinutes] = endIdx;

                DateTime firstTime = barData.GetBarTime(tfMinutes, startIdx);
                DateTime lastTime = barData.GetBarTime(tfMinutes, endIdx);
                int window = endIdx - startIdx + 1;
                int available = total - 1;
                int incremental = available - endIdx;
                _logger.Info($"[WINDOW_IDX] TF={tfMinutes}m start={startIdx} end={endIdx} window={window} incremental={incremental} (de {total}, barsNeeded={barsNeeded}, margin={incMarginTF}) firstTime={firstTime:O} lastTime={lastTime:O}");

                if (lastTime > latestLastTime)
                    latestLastTime = lastTime;
            }

            // SANITY CHECK: Dataset desactualizado (>30 días)
            DateTime now = DateTime.Now;
            TimeSpan datasetAge = now - latestLastTime;
            if (datasetAge.TotalDays > 30)
            {
                _logger.Error($"[FATAL][WINDOW] Dataset desactualizado: lastTime={latestLastTime:yyyy-MM-dd HH:mm} (hace {datasetAge.TotalDays:F0} días). Verifica contrato/histórico en NinjaTrader.");
                _logger.Error($"[FATAL][WINDOW] Se esperaba lastTime cercano a {now:yyyy-MM-dd HH:mm}. Backtest NO continuará con datos de {latestLastTime.Year}.");
                // NO configurar ventana - dejar _windowConfigured = false para evitar procesamiento
                return;
            }

            _windowConfigured = true;
            _logger.Info($"[ANCHOR] ✅ Ventana configurada por tiempo común para {_barsToSkipPerTF.Count} timeframes (anchorTime={anchorTime:yyyy-MM-dd HH:mm})");
            
            // REPLAY HISTÓRICO: construir estado detector-por-detector desde skip hasta end
            BuildHistoricalState(barData);
            
            // RESET CRÍTICO: Permitir que OnBarClose ejecute pipeline en TODA la ventana histórica
            // Solo reseteamos el TF de decisión; los demás TF mantienen sus detectores precargados
            if (_barsToSkipPerTF.TryGetValue(decisionTF, out int skipDecisionReset))
            {
                _lastProcessedBarByTF[decisionTF] = skipDecisionReset - 1;
                _logger?.Info($"[RESET_DECISION_TF] TF={decisionTF} lastProcessed reseteado a {skipDecisionReset - 1} para reprocesar ventana histórica con pipeline completo");
            }
        }

        /// <summary>
        /// V6.0n: Procesa barras de TODOS los TFs en orden temporal estricto para garantizar determinismo MTF.
        /// Construye una cola global ordenada por tiempo y procesa secuencialmente.
        /// </summary>
        /// <param name="barData">Provider de datos de barras</param>
        /// <param name="analysisTime">Tiempo hasta el cual procesar</param>
        /// <param name="runPipelineForDecision">Si ejecutar pipeline completo para TF de decisión</param>
        private void ProcessBarsInStrictTemporalOrder(IBarDataProvider barData, DateTime analysisTime, bool runPipelineForDecision)
        {
            // Construir cola con tuplas (tf, idx, time) para todas las barras pendientes
            var queue = new List<(int tf, int idx, DateTime time)>();
            
            foreach (var tf in _config.TimeframesToUse)
            {
                // Obtener límites de ventana
                if (!_barsToSkipPerTF.TryGetValue(tf, out int skip)) continue;
                if (!_barsEndPerTF.TryGetValue(tf, out int end)) continue;
                
                // Obtener última barra procesada
                int last = _lastProcessedBarByTF.TryGetValue(tf, out var lp) ? lp : (skip - 1);
                int start = Math.Max(last + 1, skip);
                
                // Obtener índice final (hasta analysisTime, sin exceder end)
                int target = barData.GetBarIndexFromTime(tf, analysisTime);
                if (target < 0) continue;
                if (target > end) target = end;
                
                // Añadir todas las barras pendientes a la cola
                for (int i = start; i <= target; i++)
                {
                    DateTime barTime = barData.GetBarTime(tf, i);
                    if (barTime <= analysisTime)
                    {
                        queue.Add((tf, i, barTime));
                    }
                }
            }
            
            // ORDENAR POR TIEMPO ESTRICTO: time ASC → tf ASC → idx ASC (determinista)
            queue = queue.OrderBy(x => x.time).ThenBy(x => x.tf).ThenBy(x => x.idx).ToList();
            
            if (queue.Count > 0)
            {
                _logger?.Info($"[SCHED] Drenando {queue.Count} barras hasta {analysisTime:yyyy-MM-dd HH:mm:ss}");
                _logger?.Info($"[SCHED_SEQ] Primera: TF={queue[0].tf}m Idx={queue[0].idx} Time={queue[0].time:yyyy-MM-dd HH:mm:ss}");
                _logger?.Info($"[SCHED_SEQ] Última: TF={queue.Last().tf}m Idx={queue.Last().idx} Time={queue.Last().time:yyyy-MM-dd HH:mm:ss}");
            }
            
            // Drenar cola en orden temporal estricto
            int count = 0;
            foreach (var (tf, idx, time) in queue)
            {
                bool runPipeline = (tf == _config.DecisionTimeframeMinutes && runPipelineForDecision);
                ProcessBarInternal(tf, idx, runPipeline);
                _lastProcessedBarByTF[tf] = idx;
                count++;
                
                // Log progreso cada 1000 barras
                if (count % 1000 == 0)
                {
                    _logger?.Info($"[SCHED_PROGRESS] Procesadas {count}/{queue.Count} barras ({count*100/queue.Count}%)");
                }
            }
            
            if (queue.Count > 0)
            {
                _logger?.Info($"[SCHED_DONE] ✅ {queue.Count} barras procesadas en orden temporal estricto");
            }
        }

        /// <summary>
        /// Construye el estado histórico de todos los detectores procesando cronológicamente 
        /// cada barra del TF de decisión y sincronizando todos los TF hasta ese punto temporal.
        /// V6.0n: USA ORDEN TEMPORAL ESTRICTO para garantizar determinismo MTF.
        /// </summary>
        private void BuildHistoricalState(IBarDataProvider barData)
        {
            // Solo actualizar detectores base (swings, liquidity, BOS) sin ejecutar pipeline
            // El pipeline se ejecuta después en ReplayHistoricalDecisions
            
            int decisionTF = _config.DecisionTimeframeMinutes;
            int skipDecision = _barsToSkipPerTF[decisionTF];
            int endDecision  = _barsEndPerTF[decisionTF];

            // Inicializar _lastProcessedBarByTF por TF al inicio de su ventana (SIEMPRE, sin condicional)
            foreach (var tf in _config.TimeframesToUse)
            {
                if (!_barsToSkipPerTF.TryGetValue(tf, out int skipTf))
                    continue;
                
                _lastProcessedBarByTF[tf] = skipTf - 1; // CRÍTICO: siempre sobrescribir (no usar 'if ContainsKey')
            }

            _logger.Info($"[REPLAY_HIST] Iniciando replay histórico desde TF={decisionTF} barra {skipDecision} hasta {endDecision}");

            // Log de ventanas por TF para diagnóstico
            foreach (var tf in _config.TimeframesToUse)
            {
                if (_barsToSkipPerTF.TryGetValue(tf, out int s) && _barsEndPerTF.TryGetValue(tf, out int e))
                {
                    _logger.Info($"[REPLAY_WINDOW] TF={tf} skip={s} end={e} window={e-s+1}");
                }
            }

            // V6.0n: Usar scheduler global con orden temporal estricto
            DateTime endTime = barData.GetBarTime(decisionTF, endDecision);
            ProcessBarsInStrictTemporalOrder(barData, endTime, runPipelineForDecision: false);
            
            _logger.Info($"[REPLAY_HIST] ✅ Replay histórico completado usando orden temporal estricto");
            
            // Log final de lastProcessed por TF CON VERIFICACIÓN
            _logger.Info($"[REPLAY_FINAL] ========== RESUMEN LASTPROCESSED ==========");
            foreach (var tf in _config.TimeframesToUse)
            {
                if (_lastProcessedBarByTF.TryGetValue(tf, out int last))
                {
                    int skip = 0;
                    int end = 0;
                    bool hasWindow = _barsToSkipPerTF.TryGetValue(tf, out skip) && _barsEndPerTF.TryGetValue(tf, out end);
                    string windowInfo = hasWindow ? $" (ventana: skip={skip} end={end} window={end-skip+1})" : " (sin ventana)";
                    _logger.Info($"[REPLAY_FINAL] TF={tf} lastProcessed={last}{windowInfo}");
                }
                else
                {
                    _logger.Warning($"[REPLAY_FINAL] TF={tf} NO TIENE lastProcessed! ⚠️");
                }
            }
            _logger.Info($"[REPLAY_FINAL] Dictionary count: {_lastProcessedBarByTF.Count} keys");
            _logger.Info($"[REPLAY_FINAL] ==============================================");
            
            // VERIFICACIÓN CRÍTICA: Asegurar que los diccionarios de ventana siguen configurados después del replay
            _logger.Info($"[REPLAY_FINAL] ========== VERIFICACIÓN DICCIONARIOS ==========");
            _logger.Info($"[REPLAY_FINAL] _barsToSkipPerTF.Count = {_barsToSkipPerTF.Count}");
            _logger.Info($"[REPLAY_FINAL] _barsEndPerTF.Count = {_barsEndPerTF.Count}");
            foreach (var tf in _config.TimeframesToUse)
            {
                bool hasSkip = _barsToSkipPerTF.ContainsKey(tf);
                bool hasEnd = _barsEndPerTF.ContainsKey(tf);
                _logger.Info($"[REPLAY_FINAL] TF={tf} hasSkip={hasSkip} hasEnd={hasEnd}");
            }
            _logger.Info($"[REPLAY_FINAL] =============================================");
        }

        /// <summary>
        /// Inicializa y registra los detectores de estructuras
        /// </summary>
        private void InitializeDetectors()
        {
            _logger.Info("Inicializando detectores...");

            // FASE 2: FVGDetector
            var fvgDetector = new FVGDetector();
            fvgDetector.Initialize(_provider, _config, _logger);
            _detectors.Add(fvgDetector);
            _logger.Info("  ✓ FVGDetector registrado");

            // FASE 3: SwingDetector
            var swingDetector = new SwingDetector();
            swingDetector.Initialize(_provider, _config, _logger);
            _detectors.Add(swingDetector);
            _logger.Info("  ✓ SwingDetector registrado");

            // FASE 4: DoubleDetector
            var doubleDetector = new DoubleDetector();
            doubleDetector.Initialize(_provider, _config, _logger);
            _detectors.Add(doubleDetector);
            _logger.Info("  ✓ DoubleDetector registrado");

            // FASE 5: OrderBlockDetector
            var orderBlockDetector = new OrderBlockDetector();
            orderBlockDetector.Initialize(_provider, _config, _logger);
            _detectors.Add(orderBlockDetector);
            _logger.Info("  ✓ OrderBlockDetector registrado");

            // FASE 6: BOSDetector (Break of Structure / Change of Character)
            var bosDetector = new BOSDetector();
            bosDetector.Initialize(_provider, _config, _logger);
            _detectors.Add(bosDetector);
            _logger.Info("  ✓ BOSDetector registrado");

            // FASE 7: POIDetector (Points of Interest - Confluencias)
            var poiDetector = new POIDetector();
            poiDetector.Initialize(_provider, _config, _logger);
            _detectors.Add(poiDetector);
            _logger.Info("  ✓ POIDetector registrado");

            // FASE 8: LiquidityVoidDetector (Zonas sin liquidez)
            // NOTA: Se ejecuta DESPUÉS de SwingDetector (no depende de swings)
            var lvDetector = new LiquidityVoidDetector();
            lvDetector.Initialize(_provider, _config, _logger);
            _detectors.Add(lvDetector);
            _logger.Info("  ✓ LiquidityVoidDetector registrado");

            // FASE 8: LiquidityGrabDetector (Stop Hunts)
            // NOTA: Se ejecuta DESPUÉS de SwingDetector (depende de swings para detectar sweeps)
            var lgDetector = new LiquidityGrabDetector();
            lgDetector.Initialize(_provider, _config, _logger);
            _detectors.Add(lgDetector);
            _logger.Info("  ✓ LiquidityGrabDetector registrado");

            _logger.Info($"Total detectores registrados: {_detectors.Count}");
        }

        // ========================================================================
        // SISTEMA DE PROGRESO
        // ========================================================================

        /// <summary>
        /// Inicializa el sistema de seguimiento de progreso para procesamiento histórico
        /// Debe llamarse ANTES de empezar a procesar barras históricas
        /// </summary>
        /// <param name="totalBars">Total de barras que se van a procesar</param>
        public void StartProgressTracking(int totalBars)
        {
            if (totalBars <= 0)
            {
                _logger.Warning($"StartProgressTracking: totalBars inválido ({totalBars}), progreso deshabilitado");
                return;
            }

            _progressTracker = new ProgressTracker(totalBars, _logger, _config);
            _saveCounter = 0;
            
            _logger.Info($"═══════════════════════════════════════════════════════");
            _logger.Info($"📊 SISTEMA DE PROGRESO ACTIVADO");
            _logger.Info($"═══════════════════════════════════════════════════════");
            _logger.Info($"Total de barras a procesar: {totalBars:N0}");
            _logger.Info($"Reporte cada {_config.ProgressReportEveryNBars} barras o {_config.ProgressReportEveryMinutes} minutos");
            _logger.Info($"═══════════════════════════════════════════════════════");
        }

        /// <summary>
        /// Finaliza el seguimiento de progreso y muestra reporte final
        /// Debe llamarse DESPUÉS de terminar el procesamiento histórico
        /// </summary>
        public void FinishProgressTracking()
        {
            if (_progressTracker != null)
            {
                int structureCount = TotalStructureCount;
                _progressTracker.ReportCompletion(structureCount, _saveCounter);
                _progressTracker = null;
            }
        }

        // ========================================================================
        // DETECCIÓN - ON BAR CLOSE
        // ========================================================================

        /// <summary>
        /// Método principal llamado cuando se cierra una barra en un timeframe
        /// Orquesta la ejecución de todos los detectores
        /// </summary>
        /// <param name="tfMinutes">Timeframe en minutos</param>
        /// <param name="barIndex">Índice de la barra que se cerró</param>
        public void OnBarClose(int tfMinutes, int barIndex)
        {
            // Log ENTRADA para rastrear todas las llamadas
            _logger?.Info($"[ONBARCLOSE_ENTRY] TF={tfMinutes} Bar={barIndex} isInit={_isInitialized} isStatic={_isStaticMode} windowConfigured={_windowConfigured}");
            
            if (!_isInitialized)
            {
                _logger?.Error("[CoreEngine] OnBarClose llamado antes de Initialize");
                return;
            }

            if (_isStaticMode)
            {
                _logger?.Debug($"[CoreEngine] OnBarClose TF={tfMinutes} Bar={barIndex} - Modo estático, sin actualización");
                return;
            }

            // ═══════════════════════════════════════════════════════════════
            // AUTOCONFIGURACIÓN: Ventana histórica determinista
            // ═══════════════════════════════════════════════════════════════
            if (!_windowConfigured)
            {
                MaybeConfigureHistoricalWindow(_provider);
                if (!_windowConfigured)
                    return; // Aún no estable → no procesar
            }

            // ═══════════════════════════════════════════════════════════════
            // GATE: Ventana histórica determinista
            // Solo bloquea barras ANTES de la ventana (< skip)
            // Permite barras >= skip (tanto dentro de ventana como incrementales post-replay)
            // ═══════════════════════════════════════════════════════════════
            bool hasSkip = _barsToSkipPerTF.TryGetValue(tfMinutes, out int skip);
            bool hasEnd = _barsEndPerTF.TryGetValue(tfMinutes, out int end);
            
            _logger?.Info($"[WINDOW_CHECK] TF={tfMinutes} Bar={barIndex} hasSkip={hasSkip} hasEnd={hasEnd} skip={skip} end={end}");
            
            if (hasSkip)
            {
                // Solo bloquear barras ANTES de la ventana histórica
                if (barIndex < skip)
                {
                    _logger?.Info($"[WINDOW_BLOCK] TF={tfMinutes} Bar={barIndex} BLOQUEADA (antes de ventana: skip={skip})");
                    return;
                }
                // Permitir barras >= skip (tanto dentro de ventana como incrementales)
            }
            else
            {
                _logger?.Warning($"[WINDOW_MISSING] TF={tfMinutes} Bar={barIndex} NO TIENE ventana configurada! hasSkip={hasSkip}");
            }

            // TRAZA DIAGNÓSTICA: Barra procesándose
            DateTime barTime = _provider.GetBarTime(tfMinutes, barIndex);
            _logger?.Info($"[DIAG][OnBarClose] TF={tfMinutes} Bar={barIndex} Time={barTime:yyyy-MM-dd HH:mm} Processing=True");

            // ═══════════════════════════════════════════════════════════════
            // CATCH-UP MULTI-TF DETERMINISTA (sincronización por tiempo)
            // ═══════════════════════════════════════════════════════════════
            // Cuando llega una barra del TF de decisión, sincronizar todos los demás TFs
            // hasta el mismo analysisTime para garantizar independencia del TF del gráfico
            // V6.0n: Usar scheduler global con orden temporal estricto (SOLO detectores base)
            _logger?.Info($"[CATCHUP_START] TF={tfMinutes} Bar={barIndex} Time={barTime:yyyy-MM-dd HH:mm} - Usando orden temporal estricto");
            ProcessBarsInStrictTemporalOrder(_provider, barTime, runPipelineForDecision: false);

            // ═══════════════════════════════════════════════════════════════
            // V6.0n: SCHEDULER DE DECISIONES POR TIEMPO (determinismo MTF)
            // ═══════════════════════════════════════════════════════════════
            // Drenar TODAS las barras del TF de decisión hasta barTime, generando decisiones
            // Esto garantiza que 15m y 60m procesen las MISMAS barras de 15m con decisiones
            if (!_isReplay && _decisionEngine != null && _tradeManager != null)
            {
                int decisionTF = _config.DecisionTimeframeMinutes;
                int targetDecisionIdx = _provider.GetBarIndexFromTime(decisionTF, barTime);
                
                if (!_lastProcessedBarByTF.TryGetValue(decisionTF, out int lastDecisionIdx))
                    lastDecisionIdx = _barsToSkipPerTF.ContainsKey(decisionTF) ? _barsToSkipPerTF[decisionTF] - 1 : -1;
                
                int decisionsGenerated = 0;
                for (int idx = lastDecisionIdx + 1; idx <= targetDecisionIdx; idx++)
                {
                    _logger?.Info($"[DECISION_SCHEDULER] Drenando barra TF={decisionTF} Idx={idx}");
                    
                    dynamic decisionEngineInstance = _decisionEngine;
                    var decision = decisionEngineInstance.GenerateDecision(_provider, this, idx, decisionTF, _accountSize);
                    
                    if (decision != null && (decision.Action == "BUY" || decision.Action == "SELL"))
                    {
                        DateTime entryBarTime = _provider.GetBarTime(decisionTF, idx);
                        double currentPrice = _provider.GetClose(decisionTF, idx);
                        
                        // Obtener structureScore para upgrade inteligente
                        double structureScore = 0.0;
                        if (!string.IsNullOrEmpty(decision.DominantStructureId))
                        {
                            var structure = GetStructureById(decision.DominantStructureId);
                            if (structure != null)
                                structureScore = structure.Score;
                        }
                        
                        _tradeManager.RegisterTrade(
                            decision.Action,
                            decision.Entry,
                            decision.StopLoss,
                            decision.TakeProfit,
                            idx,                    // entryBar = barra de 15m real
                            entryBarTime,           // entryBarTime de 15m real
                            decisionTF,             // tfDominante = 15m
                            decision.DominantStructureId ?? "",
                            currentPrice,
                            decision.DistanceToEntryATR,
                            "Normal",
                            structureScore
                        );
                        
                        decisionsGenerated++;
                    }
                    
                    // V6.0n: UpdateTrades con CORTE TEMPORAL para determinismo MTF
                    // Solo evalúa estructuras/BOS hasta idx, no eventos futuros
                    if (_tradeManager != null)
                    {
                        double ch = _provider.GetHigh(decisionTF, idx);
                        double cl = _provider.GetLow(decisionTF, idx);
                        double cp = _provider.GetClose(decisionTF, idx);
                        DateTime idxTime = _provider.GetBarTime(decisionTF, idx);
                        
                        _tradeManager.UpdateTrades(ch, cl, idx, idxTime, cp, this, _provider, "Normal", maxBarIndex: idx);
                    }
                    
                    // Marcar la barra de 15m como procesada por el core
                    _lastProcessedBarByTF[decisionTF] = idx;
                }
                
                if (decisionsGenerated > 0)
                {
                    _logger?.Info($"[DECISION_SCHEDULER] ✅ Generadas {decisionsGenerated} decisiones hasta barTime={barTime:yyyy-MM-dd HH:mm}");
                }
            }

            // ═══════════════════════════════════════════════════════════════
            // PROCESAMIENTO INCREMENTAL: Una barra, un TF
            // ═══════════════════════════════════════════════════════════════
            try
            {
                // V6.0n: Guard anti-duplicado - El scheduler de decisiones ya procesó estas barras
                bool wasProcessedByScheduler = _lastProcessedBarByTF.TryGetValue(tfMinutes, out int lastAfterSched) 
                    && barIndex <= lastAfterSched;
                
                _logger?.Info($"[GUARD_CHECK] TF={tfMinutes} Bar={barIndex} lastAfterScheduler={lastAfterSched} wasProcessedByScheduler={wasProcessedByScheduler}");
                
                if (wasProcessedByScheduler)
                {
                    _logger?.Info($"[GUARD_BLOCK_SCHEDULER] TF={tfMinutes} Bar={barIndex} YA procesada por scheduler (last={lastAfterSched})");
                    
                    // Aún necesitamos UpdateTrades para esta barra si es TF de decisión
                    if (tfMinutes == _config.DecisionTimeframeMinutes && !_isReplay && _tradeManager != null)
                    {
                        double ch = _provider.GetHigh(tfMinutes, barIndex);
                        double cl = _provider.GetLow(tfMinutes, barIndex);
                        double cp = _provider.GetClose(tfMinutes, barIndex);
                        DateTime ct = _provider.GetBarTime(tfMinutes, barIndex);
                        
                        _tradeManager.UpdateTrades(ch, cl, barIndex, ct, cp, this, _provider, "Normal");
                    }
                    return;
                }

                // Log de transición al salir del replay
                if (_windowConfigured && tfMinutes == _config.DecisionTimeframeMinutes && 
                    _barsEndPerTF.TryGetValue(tfMinutes, out int endDecision) && 
                    barIndex == endDecision + 1)
                {
                    _logger?.Info($"[EXEC_START] TF={tfMinutes} Bar={barIndex} - Iniciando procesamiento incremental tras replay");
                }

                _logger?.Info($"[PROCESS_BAR] TF={tfMinutes} Bar={barIndex} - Procesando (NO fue procesada por scheduler)");
                
                // Ejecutar pipeline solo en TF de decisión; en otros TF solo detectores base
                bool isDecisionTF = (tfMinutes == _config.DecisionTimeframeMinutes);
                ProcessBarInternal(tfMinutes, barIndex, runPipeline: isDecisionTF);
                _lastProcessedBarByTF[tfMinutes] = barIndex;
                
                _logger?.Info($"[PROCESS_BAR_DONE] TF={tfMinutes} Bar={barIndex} - Actualizado lastProcessed={barIndex} (pipeline={isDecisionTF})");
                
                // Si es TF de decisión y NO estamos en replay, ejecutar la decisión del incremental
                if (isDecisionTF && !_isReplay && _decisionEngine != null)
                {
                    // 3.1 ELIMINADO: Las decisiones del replay ya fueron registradas en su momento temporal
                    // UpdateTrades (abajo) las procesará igual que las del incremental
                    
                    // 3.2 Generar decisión para la barra incremental actual
                    _logger?.Info($"[CORE_DECISION] TF={tfMinutes} Bar={barIndex} - Generando decisión");
                    
                    var decision = _decisionEngine.GenerateDecision(_provider, this, barIndex, tfMinutes, _accountSize);
                    
                    if (decision != null && (decision.Action == "BUY" || decision.Action == "SELL"))
                    {
                        _logger?.Info($"[CORE_DECISION] Decisión generada: {decision.Action} @ {decision.Entry:F2}");
                        
                        // Registrar trade en TradeManager
                        if (_tradeManager != null)
                        {
                            DateTime entryBarTime = _provider.GetBarTime(tfMinutes, barIndex);
                            double currentPrice = _provider.GetClose(tfMinutes, barIndex);
                            
                            // V6.0n: Obtener structureScore para upgrade inteligente
                            double structureScore = 0.0;
                            if (!string.IsNullOrEmpty(decision.DominantStructureId))
                            {
                                var structure = GetStructureById(decision.DominantStructureId);
                                if (structure != null)
                                    structureScore = structure.Score;
                            }
                            
                            _tradeManager.RegisterTrade(
                                decision.Action,           // action (BUY/SELL)
                                decision.Entry,            // entry price
                                decision.StopLoss,         // sl
                                decision.TakeProfit,       // tp
                                barIndex,                  // entryBar
                                entryBarTime,              // entryBarTime
                                tfMinutes,                 // tfDominante
                                decision.DominantStructureId ?? "",  // sourceStructureId
                                currentPrice,              // currentPrice
                                decision.DistanceToEntryATR,  // distanceToEntryATR
                                "Normal",                  // currentRegime (default)
                                structureScore             // V6.0n: score de la estructura
                            );
                        }
                    }
                    
                    // 3.3 Actualizar estado de todas las órdenes (PENDING → EXECUTED, verificar SL/TP)
                    if (_tradeManager != null)
                    {
                        double ch = _provider.GetHigh(tfMinutes, barIndex);
                        double cl = _provider.GetLow(tfMinutes, barIndex);
                        double cp = _provider.GetClose(tfMinutes, barIndex);
                        DateTime ct = _provider.GetBarTime(tfMinutes, barIndex);
                        
                        _logger?.Info($"[UPDATE_SUMMARY][BEFORE] Bar={barIndex} Pending={_tradeManager.GetActiveTrades().Count(t=>t.Status==TradeStatus.PENDING)} Executed={_tradeManager.GetActiveTrades().Count(t=>t.Status==TradeStatus.EXECUTED)}");
                        _tradeManager.UpdateTrades(ch, cl, barIndex, ct, cp, this, _provider, "Normal");
                        _logger?.Info($"[UPDATE_SUMMARY][AFTER] Bar={barIndex} Pending={_tradeManager.GetActiveTrades().Count(t=>t.Status==TradeStatus.PENDING)} Executed={_tradeManager.GetActiveTrades().Count(t=>t.Status==TradeStatus.EXECUTED)}");
                    }
                }
                
                // TRAZA: Estructuras detectadas en esta barra
                int totalStructuresAfter = _structuresById.Count;
                _logger?.Info($"[DIAG][OnBarClose] TF={tfMinutes} Bar={barIndex} TotalStructures={totalStructuresAfter}");
            }
            catch (Exception ex)
            {
                _logger?.Error($"[CoreEngine] ERROR en OnBarClose TF={tfMinutes} Bar={barIndex}: {ex.Message}");
            }
        }

        /// <summary>
        /// Procesa una barra interna (detectores, scores, purgas, guardado)
        /// Método interno para evitar duplicar la lógica en catch-up
        /// </summary>
        private void ProcessBarInternal(int tfMinutes, int barIndex, bool runPipeline = true)
        {
            // Actualizar progreso SOLO en el TF de decisión si el tracker está activo
            if (_progressTracker != null && tfMinutes == _config.DecisionTimeframeMinutes)
            {
                _progressTracker.Update(barIndex);
                
                if (_progressTracker.ShouldReport())
                {
                    int structureCount = TotalStructureCount;
                    _progressTracker.Report(structureCount, _saveCounter);
                }
            }

            // SIEMPRE ejecutar detectores base (swings, liquidity, BOS)
            foreach (var detector in _detectors)
            {
                detector.OnBarClose(tfMinutes, barIndex, this);
            }

            // Actualizar scores de estructuras afectadas por proximidad
            UpdateProximityScores(tfMinutes, barIndex);

            // Purgar estructuras antiguas si está habilitado
            if (_config.EnableAutoPurge)
            {
                PurgeOldStructuresIfNeeded(tfMinutes, barIndex);
                PurgeAggressiveLiquidityGrabs(tfMinutes);
            }

            // Programar guardado asíncrono si hay cambios (solo en TF de decisión)
            if (tfMinutes == _config.DecisionTimeframeMinutes)
                ScheduleSaveIfNeeded();
            
            // Si runPipeline==false, detener aquí (solo detectores base)
            // Esto se usa durante BuildHistoricalState para construir estado sin ejecutar pipeline
            // El pipeline (StructureFusion → Proximity → DecisionEngine) se ejecuta vía DecisionEngine.GenerateDecision()
            if (!runPipeline)
                return;
        }

        /// <summary>
        /// Registra datos OHLC de la barra actual para análisis MFE/MAE
        /// </summary>
        public void LogBarOHLC(int tfMinutes, int barIndex)
        {
            if (!_config.EnableOHLCLogging) return;

            try
            {
                double open  = _provider.GetOpen(tfMinutes, barIndex);
                double high  = _provider.GetHigh(tfMinutes, barIndex);
                double low   = _provider.GetLow(tfMinutes, barIndex);
                double close = _provider.GetClose(tfMinutes, barIndex);
                DateTime time = _provider.GetBarTime(tfMinutes, barIndex);

                _logger.Info(string.Format(
                    "[{0:yyyy-MM-dd HH:mm:ss}] [PIPE] TF={1} Bar={2} O={3:F2} H={4:F2} L={5:F2} C={6:F2}",
                    time, tfMinutes, barIndex, open, high, low, close));
            }
            catch (Exception ex)
            {
                _logger.Error($"[ERROR] LogBarOHLC TF={tfMinutes} Bar={barIndex}: {ex.Message}");
            }
        }

        /// <summary>
        /// Activa el modo estático: el CoreEngine no procesará nuevas barras
        /// Solo sirve las estructuras ya cargadas desde JSON
        /// </summary>
        public void SetStaticMode(bool enabled)
        {
            _isStaticMode = enabled;
            if (enabled)
            {
                _logger.Info("[CoreEngine] ⚡ MODO ESTÁTICO ACTIVADO - No se procesarán nuevas barras");
                _logger.Info("[CoreEngine] Las estructuras cargadas desde JSON se usarán para el DecisionEngine");
            }
            else
            {
                _logger.Info("[CoreEngine] Modo estático desactivado - Procesamiento normal");
            }
        }

        /// <summary>
        /// Actualiza los scores dinámicos en modo Fast Load
        /// Recalcula proximidad, frescura y otros scores que dependen del precio actual
        /// </summary>
        /// <param name="tfMinutes">Timeframe en minutos</param>
        /// <param name="barIndex">Índice de la barra actual</param>
        public void UpdateScoresForFastLoad(int tfMinutes, int barIndex)
        {
            if (!_isStaticMode)
                return; // Solo en modo estático (Fast Load)

            try
            {
                // Actualizar scores de proximidad para todas las estructuras activas
                UpdateProximityScores(tfMinutes, barIndex);

                if (_config.EnableDebug)
                    _logger.Debug($"[FAST LOAD] Scores actualizados para TF:{tfMinutes} Bar:{barIndex}");
            }
            catch (Exception ex)
            {
                _logger.Exception($"Error actualizando scores en Fast Load - TF:{tfMinutes} Bar:{barIndex}", ex);
            }
        }

        // ========================================================================
        // API PÚBLICA - GESTIÓN DE ESTRUCTURAS
        // ========================================================================

        /// <summary>
        /// Agrega una nueva estructura al motor
        /// Llamado por detectores cuando encuentran una estructura nueva
        /// </summary>
        public void AddStructure(StructureBase structure)
        {
            if (structure == null)
                throw new ArgumentNullException(nameof(structure));

            _stateLock.EnterWriteLock();
            try
            {
                // Agregar a lista ordenada
                if (!_structuresListByTF.ContainsKey(structure.TF))
                {
                    _logger.Warning($"TF {structure.TF} no está en TimeframesToUse - ignorando estructura {structure.Id}");
                    return;
                }

                _structuresListByTF[structure.TF].Add(structure);
                _structuresById[structure.Id] = structure;

                // Agregar a interval tree
                _intervalTreesByTF[structure.TF].Insert(structure.Low, structure.High, structure);

                // Calcular score inicial usando el bar de creación, no el último disponible
                // Esto evita proximity=0 cuando la estructura se crea en el pasado histórico
                structure.Score = _scoringEngine.CalculateScore(structure, structure.CreatedAtBarIndex, _currentMarketBias);

                _stateChanged = true;

                // DIAGNÓSTICO: Loguear score de estructuras TF bajos para debug de purga
                if (structure.TF <= 15 && _config.EnablePerfDiagnostics)
                    _logger.Info($"[DIAG][ADD_STRUCTURE] Type={structure.Type} TF={structure.TF} InitialScore={structure.Score:F4} Age=0bars");

                if (_config.EnableDebug)
                    _logger.Debug($"Estructura agregada: {structure.Type} {structure.Id} TF:{structure.TF} " +
                                 $"[{structure.Low:F2}-{structure.High:F2}]");

                // Disparar evento con información detallada
                OnStructureAdded?.Invoke(this, new StructureAddedEventArgs(
                    structure, 
                    structure.TF, 
                    structure.CreatedAtBarIndex, 
                    structure.Metadata?.CreatedByDetector ?? "Unknown"
                ));
            }
            finally
            {
                _stateLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Actualiza una estructura existente
        /// </summary>
        public void UpdateStructure(StructureBase structure, int currentBarIndex)
        {
            if (structure == null)
                throw new ArgumentNullException(nameof(structure));

            _stateLock.EnterWriteLock();
            try
            {
                if (!_structuresById.ContainsKey(structure.Id))
                {
                    _logger.Debug($"UpdateStructure: estructura {structure.Id} no existe - use AddStructure()");
                    return;
                }

                // Actualizar referencia (ya está en las colecciones)
                _structuresById[structure.Id] = structure;

                // Guardar score anterior para el evento
                double previousScore = structure.Score;

                // Recalcular score usando el índice de la barra actual siendo procesada, no el último disponible
                structure.Score = _scoringEngine.CalculateScore(structure, currentBarIndex, _currentMarketBias);

                _stateChanged = true;

                if (_config.EnableDebug)
                    _logger.Debug($"Estructura actualizada: {structure.Type} {structure.Id} Score:{structure.Score:F3}");

                // Disparar evento con información detallada
                OnStructureUpdated?.Invoke(this, new StructureUpdatedEventArgs(
                    structure, 
                    structure.TF, 
                    currentBarIndex, 
                    "ScoreUpdated", 
                    previousScore, 
                    structure.Score
                ));
            }
            finally
            {
                _stateLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Actualiza el CurrentMarketBias basado en los breaks recientes
        /// Llamado por BOSDetector cuando se detecta un nuevo break
        /// 
        /// Algoritmo de votación ponderada:
        /// - Se consideran los últimos MaxRecentBreaksForBias breaks
        /// - Breaks con momentum "Strong" tienen peso 2.0
        /// - Breaks con momentum "Weak" tienen peso 1.0
        /// - Se suman los pesos por dirección (Bullish/Bearish)
        /// - El bias se determina por la dirección con más peso total
        /// - Si la diferencia es < 20%, el bias es "Neutral"
        /// </summary>
        /// <param name="tfMinutes">Timeframe en el que calcular el bias</param>
        /// <summary>
        /// V6.0n: Calcula el Market Bias hasta una barra específica (corte temporal para determinismo)
        /// </summary>
        public string GetMarketBiasAtBar(int tfMinutes, int maxBarIndex)
        {
            _stateLock.EnterReadLock();
            try
            {
                if (!_structuresListByTF.ContainsKey(tfMinutes))
                    return "Neutral";

                // Obtener los últimos N breaks HASTA maxBarIndex (corte temporal)
                var recentBreaks = _structuresListByTF[tfMinutes]
                    .OfType<StructureBreakInfo>()
                    .Where(sb => sb.IsActive && sb.CreatedAtBarIndex <= maxBarIndex)
                    .OrderByDescending(sb => sb.StartTime)
                    .ThenByDescending(sb => sb.TF)
                    .ThenBy(sb => sb.CreatedAtBarIndex)
                    .ThenBy(sb => sb.BreakType, StringComparer.Ordinal)
                    .ThenBy(sb => sb.Direction, StringComparer.Ordinal)
                    .Take(_config.MaxRecentBreaksForBias)
                    .ToList();

                if (recentBreaks.Count == 0)
                    return "Neutral";

                // Votación ponderada
                double bullishWeight = 0.0;
                double bearishWeight = 0.0;

                foreach (var sb in recentBreaks)
                {
                    double weight = sb.BreakMomentum == "Strong" ? 2.0 : 1.0;

                    if (sb.Direction == "Bullish")
                        bullishWeight += weight;
                    else if (sb.Direction == "Bearish")
                        bearishWeight += weight;
                }

                double totalWeight = bullishWeight + bearishWeight;
                if (totalWeight == 0)
                    return "Neutral";

                // Calcular porcentajes
                double bullishPercent = bullishWeight / totalWeight;
                double bearishPercent = bearishWeight / totalWeight;

                // Determinar bias (requiere > 60% para ser definitivo)
                if (bullishPercent >= 0.6)
                    return "Bullish";
                else if (bearishPercent >= 0.6)
                    return "Bearish";
                else
                    return "Neutral";
            }
            finally
            {
                _stateLock.ExitReadLock();
            }
        }
        
        public void UpdateCurrentMarketBias(int tfMinutes)
        {
            _stateLock.EnterWriteLock();
            try
            {
                if (!_structuresListByTF.ContainsKey(tfMinutes))
                    return;

                // Obtener los últimos N breaks
                var recentBreaks = _structuresListByTF[tfMinutes]
                    .OfType<StructureBreakInfo>()
                    .Where(sb => sb.IsActive)
                    .OrderByDescending(sb => sb.StartTime)
                    .ThenByDescending(sb => sb.TF)
                    .ThenBy(sb => sb.CreatedAtBarIndex)
                    .ThenBy(sb => sb.BreakType, StringComparer.Ordinal)
                    .ThenBy(sb => sb.Direction, StringComparer.Ordinal)
                    .Take(_config.MaxRecentBreaksForBias)
                    .ToList();

                if (recentBreaks.Count == 0)
                {
                    _currentMarketBias = "Neutral";
                    return;
                }

                // Votación ponderada
                double bullishWeight = 0.0;
                double bearishWeight = 0.0;

                foreach (var sb in recentBreaks)
                {
                    double weight = sb.BreakMomentum == "Strong" ? 2.0 : 1.0;

                    if (sb.Direction == "Bullish")
                        bullishWeight += weight;
                    else if (sb.Direction == "Bearish")
                        bearishWeight += weight;
                }

                double totalWeight = bullishWeight + bearishWeight;
                if (totalWeight == 0)
                {
                    _currentMarketBias = "Neutral";
                    return;
                }

                // Calcular porcentajes
                double bullishPercent = bullishWeight / totalWeight;
                double bearishPercent = bearishWeight / totalWeight;

                // Determinar bias (requiere > 60% para ser definitivo)
                if (bullishPercent >= 0.6)
                    _currentMarketBias = "Bullish";
                else if (bearishPercent >= 0.6)
                    _currentMarketBias = "Bearish";
                else
                    _currentMarketBias = "Neutral";

                if (_config.EnableDebug)
                    _logger.Debug($"CurrentMarketBias actualizado a '{_currentMarketBias}' " +
                                 $"(Bullish:{bullishPercent:P0}, Bearish:{bearishPercent:P0}, " +
                                 $"basado en {recentBreaks.Count} breaks recientes)");
            }
            finally
            {
                _stateLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Elimina una estructura por ID
        /// </summary>
        public bool RemoveStructure(string id)
        {
            _stateLock.EnterWriteLock();
            try
            {
                return RemoveStructureInternal(id, "Manual");
            }
            finally
            {
                _stateLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Elimina una estructura por ID (versión interna sin lock)
        /// SOLO usar cuando ya se tiene WriteLock
        /// </summary>
        /// <param name="id">ID de la estructura a eliminar</param>
        /// <param name="reason">Razón de la eliminación (Purged, Invalidated, Manual, Expired)</param>
        private bool RemoveStructureInternal(string id, string reason = "Manual")
            {
                if (!_structuresById.TryGetValue(id, out var structure))
                    return false;

                // Guardar información para el evento antes de eliminar
                string structureType = structure.Type;
                int tfMinutes = structure.TF;
                double lastScore = structure.Score;
                int currentBarIndex = _provider.GetCurrentBarIndex(tfMinutes);

                // Remover de lista
                _structuresListByTF[structure.TF].Remove(structure);

                // Remover de interval tree
                _intervalTreesByTF[structure.TF].RemoveByData(structure.Low, structure.High, structure);

                // Remover de diccionario
                _structuresById.Remove(id);

                _stateChanged = true;

                if (_config.EnableDebug)
                    _logger.Debug($"Estructura eliminada: {structure.Type} {id} Razón:{reason}");

                // Disparar evento con información detallada
                OnStructureRemoved?.Invoke(this, new StructureRemovedEventArgs(
                    id, 
                    structureType, 
                    tfMinutes, 
                    currentBarIndex, 
                    reason, 
                    lastScore
                ));

                return true;
        }

        // ========================================================================
        // API PÚBLICA - CONSULTAS
        // ========================================================================

        /// <summary>
        /// Obtiene FVGs activos en un timeframe con score mínimo
        /// </summary>
        public IReadOnlyList<FVGInfo> GetActiveFVGs(int tfMinutes, double minScore = 0.0)
        {
            _stateLock.EnterReadLock();
            try
            {
                if (!_structuresListByTF.ContainsKey(tfMinutes))
                    return new List<FVGInfo>();

                return _structuresListByTF[tfMinutes]
                    .OfType<FVGInfo>()
                    .Where(f => f.IsActive && f.Score >= minScore)
                    .OrderByDescending(f => f.Score)
                    .ThenByDescending(f => f.TF)
                    .ThenBy(f => f.CreatedAtBarIndex)
                    .ThenBy(f => f.StartTime)
                    .ThenBy(f => f.Low)
                    .ThenBy(f => f.High)
                    .ThenBy(f => f.Type, StringComparer.Ordinal)
                    .ToList();
            }
            finally
            {
                _stateLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Obtiene Swings recientes en un timeframe
        /// </summary>
        public IReadOnlyList<SwingInfo> GetRecentSwings(int tfMinutes, int maxCount = 50)
        {
            _stateLock.EnterReadLock();
            try
            {
                if (!_structuresListByTF.ContainsKey(tfMinutes))
                    return new List<SwingInfo>();

                return _structuresListByTF[tfMinutes]
                    .OfType<SwingInfo>()
                    .Where(s => s.IsActive)
                    .OrderByDescending(s => s.CreatedAtBarIndex)
                    .ThenByDescending(s => s.TF)
                    .ThenBy(s => s.StartTime)
                    .ThenBy(s => s.Low)
                    .ThenBy(s => s.High)
                    .Take(maxCount)
                    .ToList();
            }
            finally
            {
                _stateLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Obtiene Double Tops/Bottoms en un timeframe
        /// </summary>
        public IReadOnlyList<DoubleTopInfo> GetDoubleTops(int tfMinutes, string status = null)
        {
            _stateLock.EnterReadLock();
            try
            {
                if (!_structuresListByTF.ContainsKey(tfMinutes))
                    return new List<DoubleTopInfo>();

                var query = _structuresListByTF[tfMinutes].OfType<DoubleTopInfo>();

                if (!string.IsNullOrEmpty(status))
                    query = query.Where(d => d.Status == status);

                return query
                    .OrderByDescending(d => d.Score)
                    .ThenByDescending(d => d.TF)
                    .ThenBy(d => d.CreatedAtBarIndex)
                    .ThenBy(d => d.StartTime)
                    .ThenBy(d => d.Low)
                    .ThenBy(d => d.High)
                    .ToList();
            }
            finally
            {
                _stateLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Obtiene TODAS las estructuras en un timeframe (sin filtros)
        /// </summary>
        public IReadOnlyList<StructureBase> GetAllStructures(int tfMinutes)
        {
            _stateLock.EnterReadLock();
            try
            {
                if (!_structuresListByTF.ContainsKey(tfMinutes))
                    return new List<StructureBase>();

                return _structuresListByTF[tfMinutes].ToList();
            }
            finally
            {
                _stateLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Obtiene Order Blocks en un timeframe
        /// </summary>
        public IReadOnlyList<OrderBlockInfo> GetOrderBlocks(int tfMinutes, double minScore = 0.0)
        {
            _stateLock.EnterReadLock();
            try
            {
                if (!_structuresListByTF.ContainsKey(tfMinutes))
                    return new List<OrderBlockInfo>();

                return _structuresListByTF[tfMinutes]
                    .OfType<OrderBlockInfo>()
                    .Where(ob => ob.IsActive && !ob.IsMitigated && ob.Score >= minScore)
                    .OrderByDescending(ob => ob.Score)
                    .ThenByDescending(ob => ob.TF)
                    .ThenBy(ob => ob.CreatedAtBarIndex)
                    .ThenBy(ob => ob.StartTime)
                    .ThenBy(ob => ob.Low)
                    .ThenBy(ob => ob.High)
                    .ToList();
            }
            finally
            {
                _stateLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Obtiene Structure Breaks (BOS/CHoCH) ordenados por fecha
        /// </summary>
        /// <param name="tfMinutes">Timeframe en minutos</param>
        /// <param name="breakType">Filtrar por tipo: "BOS", "CHoCH", o null para ambos</param>
        /// <param name="maxCount">Número máximo de breaks a retornar (más recientes primero)</param>
        /// <returns>Lista de StructureBreakInfo ordenada por fecha descendente</returns>
        public IReadOnlyList<StructureBreakInfo> GetStructureBreaks(int tfMinutes, string breakType = null, int maxCount = 50)
        {
            _stateLock.EnterReadLock();
            try
            {
                if (!_structuresListByTF.ContainsKey(tfMinutes))
                    return new List<StructureBreakInfo>();

                var query = _structuresListByTF[tfMinutes]
                    .OfType<StructureBreakInfo>()
                    .Where(sb => sb.IsActive);

                // Filtrar por tipo si se especifica
                if (!string.IsNullOrEmpty(breakType))
                    query = query.Where(sb => sb.BreakType == breakType);

                return query
                    .OrderByDescending(sb => sb.StartTime)
                    .ThenByDescending(sb => sb.TF)
                    .ThenBy(sb => sb.CreatedAtBarIndex)
                    .ThenBy(sb => sb.BreakType, StringComparer.Ordinal)
                    .ThenBy(sb => sb.Direction, StringComparer.Ordinal)
                    .Take(maxCount)
                    .ToList();
            }
            finally
            {
                _stateLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Obtiene Points of Interest (confluencias)
        /// </summary>
        public IReadOnlyList<PointOfInterestInfo> GetPOIs(int tfMinutes, double minScore = 0.0)
        {
            _stateLock.EnterReadLock();
            try
            {
                if (!_structuresListByTF.ContainsKey(tfMinutes))
                    return new List<PointOfInterestInfo>();

                return _structuresListByTF[tfMinutes]
                    .OfType<PointOfInterestInfo>()
                    .Where(poi => poi.IsActive && poi.CompositeScore >= minScore)
                    .OrderByDescending(poi => poi.CompositeScore)
                    .ThenByDescending(poi => poi.TF)
                    .ThenBy(poi => poi.CreatedAtBarIndex)
                    .ThenBy(poi => poi.StartTime)
                    .ThenBy(poi => poi.Low)
                    .ThenBy(poi => poi.High)
                    .ToList();
            }
            finally
            {
                _stateLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Obtiene Liquidity Voids (zonas sin liquidez)
        /// </summary>
        /// <param name="tfMinutes">Timeframe en minutos</param>
        /// <param name="minScore">Score mínimo (0.0 - 1.0)</param>
        /// <param name="includeF illed">Si true, incluye voids rellenados</param>
        /// <returns>Lista de Liquidity Voids ordenados por score</returns>
        public IReadOnlyList<LiquidityVoidInfo> GetLiquidityVoids(int tfMinutes, double minScore = 0.0, bool includeFilled = false)
        {
            _stateLock.EnterReadLock();
            try
            {
                if (!_structuresListByTF.ContainsKey(tfMinutes))
                    return new List<LiquidityVoidInfo>();

                return _structuresListByTF[tfMinutes]
                    .OfType<LiquidityVoidInfo>()
                    .Where(lv => lv.IsActive && lv.Score >= minScore && (includeFilled || !lv.IsFilled))
                    .OrderByDescending(lv => lv.Score)
                    .ThenByDescending(lv => lv.TF)
                    .ThenBy(lv => lv.CreatedAtBarIndex)
                    .ThenBy(lv => lv.StartTime)
                    .ThenBy(lv => lv.Low)
                    .ThenBy(lv => lv.High)
                    .ToList();
            }
            finally
            {
                _stateLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Obtiene Liquidity Grabs (stop hunts / sweeps)
        /// </summary>
        /// <param name="tfMinutes">Timeframe en minutos</param>
        /// <param name="minScore">Score mínimo (0.0 - 1.0)</param>
        /// <param name="confirmedOnly">Si true, solo devuelve grabs con reversión confirmada</param>
        /// <returns>Lista de Liquidity Grabs ordenados por score</returns>
        public IReadOnlyList<LiquidityGrabInfo> GetLiquidityGrabs(int tfMinutes, double minScore = 0.0, bool confirmedOnly = false)
        {
            _stateLock.EnterReadLock();
            try
            {
                if (!_structuresListByTF.ContainsKey(tfMinutes))
                    return new List<LiquidityGrabInfo>();

                return _structuresListByTF[tfMinutes]
                    .OfType<LiquidityGrabInfo>()
                    .Where(lg => lg.IsActive && lg.Score >= minScore && !lg.FailedGrab && (confirmedOnly == false || lg.ConfirmedReversal))
                    .OrderByDescending(lg => lg.Score)
                    .ThenByDescending(lg => lg.TF)
                    .ThenBy(lg => lg.CreatedAtBarIndex)
                    .ThenBy(lg => lg.StartTime)
                    .ThenBy(lg => lg.Low)
                    .ThenBy(lg => lg.High)
                    .ToList();
            }
            finally
            {
                _stateLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Obtiene una estructura específica por ID
        /// </summary>
        public StructureBase GetStructureById(string id)
        {
            _stateLock.EnterReadLock();
            try
            {
                return _structuresById.TryGetValue(id, out var structure) ? structure : null;
            }
            finally
            {
                _stateLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Consulta estructuras que se superponen con un rango de precio en un TF
        /// Usa IntervalTree para O(log n + k)
        /// </summary>
        public IReadOnlyList<StructureBase> QueryOverlappingStructures(int tfMinutes, double low, double high)
        {
            _stateLock.EnterReadLock();
            try
            {
                if (!_intervalTreesByTF.ContainsKey(tfMinutes))
                    return new List<StructureBase>();

                return _intervalTreesByTF[tfMinutes].QueryOverlap(low, high).ToList();
            }
            finally
            {
                _stateLock.ExitReadLock();
            }
        }

        // ========================================================================
        // SCORING (Skeleton - se implementará completamente en Fase 2)
        // ========================================================================

        /// <summary>
        /// Calcula el score de una estructura basado en múltiples factores
        /// TODO Fase 2: Implementar fórmula completa
        /// </summary>
        private double CalculateScore(StructureBase structure, int currentBarIndex)
        {
            // Skeleton - retorna score básico por ahora
            // En Fase 2 se implementará la fórmula completa con:
            // - TF normalization
            // - Freshness
            // - Proximity
            // - Type normalization
            // - Touch factor
            // - Confluence
            // - Momentum multiplier
            // - Fill handling
            // - Decay

            return 0.5; // Placeholder
        }

        /// <summary>
        /// Actualiza scores de estructuras basado en proximidad al precio actual
        /// Llamado después de cada OnBarClose
        /// </summary>
        private void UpdateProximityScores(int tfMinutes, int barIndex)
        {
            // Usar el barIndex de la barra siendo procesada, no el último disponible
            if (barIndex < 0)
                return;
            
            // Fuente de precio para proximidad
            string priceSrc = (_config.ProximityPriceSource ?? "Close").ToLowerInvariant();
            double currentPrice;
            if (priceSrc == "mid")
            {
                // Mid alineado al TF/índice actual: (High+Low)/2 en tfMinutes/barIndex
                double h = _provider.GetHigh(tfMinutes, barIndex);
                double l = _provider.GetLow(tfMinutes, barIndex);
                currentPrice = (h + l) / 2.0;
            }
            else
            {
                currentPrice = _provider.GetClose(tfMinutes, barIndex);
            }
            if (currentPrice <= 0)
                return;
            
            // Obtener ATR del TF para normalización (firma correcta: tf, period, barIndex)
            double atr = _provider.GetATR(tfMinutes, 14, barIndex);
            if (atr <= 0)
                atr = 1.0; // Fallback
            
            _stateLock.EnterWriteLock();
            try
            {
                if (!_structuresListByTF.ContainsKey(tfMinutes))
                    return;
                
                var structures = _structuresListByTF[tfMinutes];
                int updatedCount = 0;
                
                foreach (var structure in structures)
                {
                    if (!structure.IsActive)
                        continue;
                    
                    // 1. Calcular distancia al ENTRY ESTRUCTURAL
                    double entryPrice;
                    
                    // Para estructuras direccionales, usar el borde de entrada
                    if (structure is OrderBlockInfo ob)
                    {
                        entryPrice = ob.Direction == "Bullish" ? ob.Low : ob.High;
                    }
                    else if (structure is FVGInfo fvg)
                    {
                        if (_config.UseNearestEdgeForFVGProximity)
                        {
                            double distLow = Math.Abs(currentPrice - fvg.Low);
                            double distHigh = Math.Abs(currentPrice - fvg.High);
                            entryPrice = distLow <= distHigh ? fvg.Low : fvg.High;
                        }
                        else
                        {
                            // Comportamiento base: centro de la zona
                            entryPrice = (fvg.High + fvg.Low) / 2.0;
                        }
                    }
                    else if (structure is PointOfInterestInfo poi)
                    {
                        // POI no tiene Direction, usar el centro o determinar por tipo
                        entryPrice = (poi.High + poi.Low) / 2.0;
                    }
                    else if (structure is SwingInfo swing)
                    {
                        // Swings: usar el precio del swing
                        entryPrice = swing.Type == "SwingHigh" ? swing.High : swing.Low;
                    }
                    else
                    {
                        // Para otras estructuras, usar el centro
                        entryPrice = (structure.High + structure.Low) / 2.0;
                    }
                    
                    // 2. Calcular distancia
                    double distance = Math.Abs(currentPrice - entryPrice);
                    double distanceATR = atr > 0 ? (distance / atr) : double.PositiveInfinity;
                    
                    // 3. Calcular factor de proximidad base (lineal)
                    double baseProximityFactor = Math.Max(0.0, 1.0 - (distanceATR / _config.ProximityThresholdATR));
                    
                    // 4. Penalización por tamaño de zona (zonas grandes son menos precisas)
                    // CALIBRACIÓN V5: Penalización suavizada
                    double zoneHeight = structure.High - structure.Low;
                    double zoneHeightATR = zoneHeight / atr;
                    
                    double sizePenalty;
                    if (zoneHeightATR <= 5.0)
                    {
                        sizePenalty = 1.0; // Sin penalización para zonas pequeñas
                    }
                    else if (zoneHeightATR <= 15.0)
                    {
                        // CALIBRACIÓN V5: Penalización MUY leve: 1.0 -> 0.80 (antes era 1.0 -> 0.5)
                        sizePenalty = 1.0 - ((zoneHeightATR - 5.0) / 50.0);
                    }
                    else if (zoneHeightATR <= 30.0)
                    {
                        // Penalización moderada: 0.80 -> 0.30
                        sizePenalty = 0.80 - ((zoneHeightATR - 15.0) / 30.0);
                    }
                    else
                    {
                        // Penalización máxima para zonas gigantes
                        sizePenalty = 0.30;
                    }
                    
                    // 5. Factor de proximidad final (con penalización)
                    double proximityFactor = baseProximityFactor * sizePenalty;
                    
                    // Hard cut: FVGs demasiado lejos (≥ ProxMaxATRFactor) deben puntuar casi 0
                    if (structure is FVGInfo && _config.EnableProximityHardCutInDFM && distanceATR >= _config.ProxMaxATRFactor)
                    {
                        proximityFactor = 0.0;
                    }

                    // 6. Actualizar el Score de la estructura
                    // Regla:
                    //  - LiquidityGrabInfo: no tocar score (lo gestiona su detector)
                    //  - FVGInfo: recalcular usando ScoringEngine completo (respeta clamps/edad/TF)
                    //  - Otros: fórmula rápida (freshness 70% + proximity 30%)
                    
                    // Calcular freshness (edad de la estructura) para la fórmula rápida
                    int age = barIndex - structure.CreatedAtBarIndex;
                    double freshness = CalculateFreshness(age, tfMinutes);

                    if (structure is LiquidityGrabInfo)
                    {
                        // Mantener score establecido por el detector; solo actualizar metadata abajo
                    }
                    else if (structure is FVGInfo)
                    {
                        if (_config.EnableFVGDelegatedScoring)
                        {
                            // Delegar completamente en ScoringEngine para el scoring de FVG
                            structure.Score = _scoringEngine.CalculateScore(structure, barIndex, _currentMarketBias);
                        }
                        else
                        {
                            // Usar fórmula rápida como el resto
                            structure.Score = (freshness * 0.7) + (proximityFactor * 0.3);
                        }
                    }
                    else
                    {
                        // Fórmula rápida para el resto de estructuras
                        structure.Score = (freshness * 0.7) + (proximityFactor * 0.3);
                    }

                    // Bonus leve para FVGs de TF alto para desempatar (garantiza TF240 > TF60)
                    if (structure is FVGInfo fvgBonus && _config.EnableFVGTFBonus && fvgBonus.TF >= 240)
                    {
                        structure.Score = Math.Min(1.0, structure.Score + 0.02);
                    }

                    // Penalización fuerte para FVGs muy antiguos (≥200 barras)
                    if (structure is FVGInfo && _config.EnableFVGAgePenalty200 && age >= 200)
                    {
                        structure.Score = Math.Min(structure.Score, 0.08);
                    }
                    
                    // Añadir metadata para debugging
                    structure.Metadata.ProximityScore = proximityFactor;
                    structure.Metadata.ProximityFactor = proximityFactor;
                    structure.Metadata.DistanceATR = distanceATR;
                    structure.Metadata.SizePenalty = sizePenalty;
                    structure.Metadata.ZoneHeightATR = zoneHeightATR;
                    structure.Metadata.LastProximityUpdate = barIndex;
                    
                    updatedCount++;

                    // Muestreo de drivers de proximidad para diagnósticos
                    _proxDiagSampleCounter++;
                    int sampleRate = 400; // reducir ruido en histórico
                    if (_config.EnablePerfDiagnostics)
                        sampleRate = 100; // más granular si se pide perf diag
                    if ((_proxDiagSampleCounter % sampleRate) == 0)
                    {
                        try
                        {
                            // Tipo lógico para trazas compactas
                            string type = structure.Type;
                            _logger.Info(string.Format(
                                "[DIAG][PROX] Type={0} TF={1} DistATR={2:F2} BaseProx={3:F3} SizePenalty={4:F3} FinalProx={5:F3} ZoneATR={6:F2}",
                                type, tfMinutes, distanceATR, baseProximityFactor, sizePenalty, proximityFactor, zoneHeightATR));
                        }
                        catch { }
                    }
                }
                
                if (_config.EnableDebug && updatedCount > 0)
                {
                    _logger.Debug($"[FAST LOAD] Proximity actualizado: {updatedCount} estructuras en TF {tfMinutes}");
                }

                // [PIPE][PROX] resumen agregado cada N barras SOLO en TF de decisión
                if (tfMinutes == _config.DecisionTimeframeMinutes)
                {
                    int interval = Math.Max(1, _config.DiagnosticsInterval);
                    if ((barIndex % interval) == 0)
                    {
                        _logger.Info($"[PIPE][PROX] TF={tfMinutes} Bar={barIndex} Updated={updatedCount} TotalStructures={_structuresById.Count}");
                    }
                }
            }
            finally
            {
                _stateLock.ExitWriteLock();
            }
        }
        
        /// <summary>
        /// Calcula el factor de frescura (freshness) basado en la edad de la estructura
        /// </summary>
        private double CalculateFreshness(int age, int tfMinutes)
        {
            // Freshness decay exponencial
            // Zonas recientes (< 10 barras) = 1.0
            // Zonas viejas (> 100 barras) = ~0.1
            int decayPeriod = 50; // Default: 50 barras
            double freshness = Math.Exp(-age / (double)decayPeriod);
            return Math.Max(0.01, freshness); // Mínimo 1%
        }

        /// <summary>
        /// Actualiza el bias de mercado basado en BOS/CHoCH recientes
        /// Llamado por BOSDetector
        /// </summary>
        public void UpdateMarketBias(string newBias)
        {
            _stateLock.EnterWriteLock();
            try
            {
                if (_currentMarketBias != newBias)
                {
                    _logger.Info($"Market Bias cambió: {_currentMarketBias} -> {newBias}");
                    _currentMarketBias = newBias;
                    _stateChanged = true;
                }
            }
            finally
            {
                _stateLock.ExitWriteLock();
            }
        }

        // ========================================================================
        // PERSISTENCIA - IMPLEMENTACIÓN COMPLETA (FASE 9)
        // ========================================================================

        /// <summary>
        /// Guarda el estado completo a JSON de forma asíncrona
        /// Usa el path configurado en EngineConfig.StateFilePath si no se especifica
        /// </summary>
        public async Task SaveStateToJSONAsync(string path = null)
        {
            if (!_config.AutoSaveEnabled && path == null)
            {
                _logger.Info("AutoSave deshabilitado - guardado omitido");
                return;
            }

            string filePath = path ?? _config.StateFilePath;

            try
            {
                _logger.Info($"Iniciando guardado de estado a: {filePath}");

                // Actualizar estadísticas antes de guardar
                UpdateEngineStats();

                // Copiar estructuras con read lock (minimizar tiempo de bloqueo)
                Dictionary<int, List<StructureBase>> structuresCopy;
                string currentBias;

                _stateLock.EnterReadLock();
                try
                {
                    // Crear copia profunda de estructuras
                    structuresCopy = new Dictionary<int, List<StructureBase>>();
                    foreach (var kv in _structuresListByTF)
                    {
                        structuresCopy[kv.Key] = new List<StructureBase>(kv.Value);
                    }
                    currentBias = _currentMarketBias;
                }
                finally
                {
                    _stateLock.ExitReadLock();
                }

                // Guardar (fuera del lock para no bloquear el motor)
                await _persistenceManager.SaveStateToFileAsync(
                    structuresCopy,
                    _stats.Instrument,
                    currentBias,
                    _stats,
                    filePath
                ).ConfigureAwait(false);

                // Actualizar estadísticas de guardado
                _lastSaveTime = DateTime.UtcNow;
                _stateChanged = false;
                _stats.LastSaveTime = _lastSaveTime;
                _stats.LastSaveSuccessful = true;
                _stats.LastSaveError = null;
                _stats.TotalSavesSinceStart++;
                
                // Incrementar contador para reporte de progreso
                _saveCounter++;

                _logger.Info($"Estado guardado exitosamente: {filePath}");
            }
            catch (Exception ex)
            {
                _logger.Exception($"Error guardando estado a {filePath}", ex);
                
                _stats.LastSaveSuccessful = false;
                _stats.LastSaveError = ex.Message;
                
                throw;
            }
        }

        /// <summary>
        /// Carga el estado desde JSON
        /// Usa el path configurado en EngineConfig.StateFilePath si no se especifica
        /// </summary>
        /// <param name="path">Ruta del archivo (opcional, usa StateFilePath si es null)</param>
        /// <param name="forceLoad">Si true, carga sin validar hash de configuración</param>
        public void LoadStateFromJSON(string path = null, bool forceLoad = false)
        {
            string filePath = path ?? _config.StateFilePath;

            try
            {
                _logger.Info($"Iniciando carga de estado desde: {filePath}");

                // Verificar si el archivo existe
                if (!_persistenceManager.StateFileExists(filePath))
                {
                    _logger.Warning($"Archivo de estado no encontrado: {filePath} - iniciando con estado vacío");
                    return;
                }

                // Cargar estado
                var state = _persistenceManager.LoadStateFromFile(filePath, forceLoad);

                if (state == null)
                {
                    _logger.Warning("Estado cargado es null - iniciando con estado vacío");
                    return;
                }

                // Aplicar estado al motor
                _stateLock.EnterWriteLock();
                try
                {
                    // Limpiar estado actual
                    _structuresListByTF.Clear();
                    _intervalTreesByTF.Clear();
                    _structuresById.Clear();

                    // Reinicializar estructuras por timeframe
                    foreach (var tf in _config.TimeframesToUse)
                    {
                        _structuresListByTF[tf] = new List<StructureBase>();
                        _intervalTreesByTF[tf] = new IntervalTree<StructureBase>();
                    }

                    // Cargar estructuras
                    int totalLoaded = 0;
                    foreach (var kv in state.StructuresByTF)
                    {
                        int tfMinutes = kv.Key;
                        var structures = kv.Value;

                        // Verificar que el TF esté configurado
                        if (!_config.TimeframesToUse.Contains(tfMinutes))
                        {
                            _logger.Warning($"TF {tfMinutes} no está en TimeframesToUse - omitiendo {structures.Count} estructuras");
                            continue;
                        }

                        foreach (var structure in structures)
                        {
                            // Añadir a lista
                            _structuresListByTF[tfMinutes].Add(structure);

                            // Añadir a índice espacial
                            _intervalTreesByTF[tfMinutes].Insert(structure.Low, structure.High, structure);

                            // Añadir a diccionario por ID
                            _structuresById[structure.Id] = structure;

                            totalLoaded++;
                        }
                    }

                    // Restaurar bias
                    _currentMarketBias = state.CurrentMarketBias ?? "Neutral";

                    // Restaurar estadísticas si están disponibles
                    if (state.Stats != null)
                    {
                        _stats = state.Stats;
                        _stats.IsInitialized = _isInitialized;
                    }

                    _stateChanged = false;

                    _logger.Info($"Estado cargado: {totalLoaded} estructuras, Bias={_currentMarketBias}");
                }
                finally
                {
                    _stateLock.ExitWriteLock();
                }

                // Actualizar estadísticas de carga
                _stats.LastLoadTime = DateTime.UtcNow;
                _stats.LastLoadSuccessful = true;
                _stats.LastLoadError = null;
                _stats.TotalLoadsSinceStart++;
                _stats.CurrentConfigHash = _config.GetHash();
                _stats.LoadedConfigHash = state.EngineConfigHash;
                _stats.ConfigHashMatched = (_stats.CurrentConfigHash == _stats.LoadedConfigHash);

                _logger.Info($"Carga completada exitosamente desde: {filePath}");
            }
            catch (Exception ex)
            {
                _logger.Exception($"Error cargando estado desde {filePath}", ex);
                
                _stats.LastLoadSuccessful = false;
                _stats.LastLoadError = ex.Message;
                
                throw;
            }
        }

        /// <summary>
        /// Programa un guardado asíncrono si hay cambios y ha pasado el intervalo
        /// Implementa debounce para evitar guardados excesivos
        /// </summary>
        private void ScheduleSaveIfNeeded()
        {
            // Si no hay cambios o el auto-save está deshabilitado, salir
            if (!_stateChanged || !_config.AutoSaveEnabled)
                return;

            // Verificar si ha pasado el intervalo desde el último guardado
            var timeSinceLastSave = (DateTime.UtcNow - _lastSaveTime).TotalSeconds;
            if (timeSinceLastSave < _config.StateSaveIntervalSecs)
                return;

            // Si ya hay una tarea de guardado en progreso, salir
            if (_saveTask != null && !_saveTask.IsCompleted)
            {
                _logger.Debug("Guardado ya en progreso - omitiendo");
                return;
            }

            // Iniciar guardado asíncrono
            _logger.Debug($"Programando guardado (cambios detectados, {timeSinceLastSave:F1}s desde último guardado)");

            _saveTask = Task.Run(async () =>
            {
                try
                {
                    await SaveStateToJSONAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.Exception("Error en guardado asíncrono programado", ex);
                }
            }, _saveCancellationTokenSource.Token);
        }

        // ========================================================================
        // DIAGNÓSTICOS - IMPLEMENTACIÓN COMPLETA (FASE 9)
        // ========================================================================

        /// <summary>
        /// Ejecuta diagnósticos sintéticos del sistema
        /// Valida inicialización, estadísticas, persistencia, purga, thread-safety y performance
        /// </summary>
        /// <returns>Reporte de diagnósticos con resultados de todos los tests</returns>
        public DiagnosticReport RunSelfDiagnostics()
        {
            _logger.Info("Ejecutando diagnósticos del sistema...");

            var diagnostics = new Diagnostics(this, _provider, _logger);
            var report = diagnostics.RunAllDiagnostics();

            _logger.Info($"Diagnósticos completados: {report.PassedTests}/{report.TotalTests} tests pasados ({report.PassRate:F1}%)");

            return report;
        }

        // ========================================================================
        // ESTADÍSTICAS
        // ========================================================================

        /// <summary>
        /// Obtiene las estadísticas actuales del motor
        /// </summary>
        public EngineStats GetEngineStats()
        {
            UpdateEngineStats();
            return _stats;
        }

        /// <summary>
        /// Actualiza las estadísticas del motor con datos actuales
        /// </summary>
        private void UpdateEngineStats()
        {
            _stateLock.EnterReadLock();
            try
            {
                _stats.GeneratedAt = DateTime.UtcNow;
                _stats.IsInitialized = _isInitialized;
                _stats.CurrentMarketBias = _currentMarketBias;

                // Total de estructuras
                _stats.TotalStructures = _structuresById.Count;
                _stats.TotalActiveStructures = _structuresById.Values.Count(s => s.IsActive);
                _stats.TotalCompletedStructures = _structuresById.Values.Count(s => s.IsCompleted);

                // Estructuras por tipo (ordenado determinísticamente)
                _stats.StructuresByType.Clear();
                foreach (var structure in _structuresById.Values
                .OrderBy(s => s.Type, StringComparer.Ordinal)
                .ThenBy(s => s.StartTime)
                .ThenBy(s => s.CreatedAtBarIndex))
                {
                    if (!_stats.StructuresByType.ContainsKey(structure.Type))
                        _stats.StructuresByType[structure.Type] = 0;
                    
                    _stats.StructuresByType[structure.Type]++;
                }

                // Estructuras por TF
                _stats.StructuresByTF.Clear();
                foreach (var kv in _structuresListByTF)
                {
                    _stats.StructuresByTF[kv.Key] = kv.Value.Count;
                }

                // Scores
                if (_structuresById.Count > 0)
                {
                    var scores = _structuresById.Values.Select(s => s.Score).ToList();
                    _stats.AverageScore = scores.Average();
                    _stats.MaxScore = scores.Max();
                    _stats.MinScore = scores.Min();
                }
                else
                {
                    _stats.AverageScore = 0;
                    _stats.MaxScore = 0;
                    _stats.MinScore = 0;
                }

                // Memoria estimada (aproximación)
                _stats.EstimatedMemoryBytes = _structuresById.Count * 1024; // ~1KB por estructura (estimación conservadora)
            }
            finally
            {
                _stateLock.ExitReadLock();
            }
        }

        // ========================================================================
        // MANTENIMIENTO Y PURGA INTELIGENTE (FASE 9)
        // ========================================================================

        /// <summary>
        /// Calcula la edad máxima en BARRAS según el TF de la estructura
        /// Convierte minutos configurados a barras del TF específico
        /// Permite que estructuras de TF altos (60m, 240m, 1440m) sobrevivan más tiempo
        /// </summary>
        private int GetMaxAgeBarsForTF(int tfMinutes)
        {
            int maxAgeMinutes = tfMinutes switch
            {
                5 => _config.MaxAgeMinutesForPurge_5m,
                15 => _config.MaxAgeMinutesForPurge_15m,
                60 => _config.MaxAgeMinutesForPurge_60m,
                240 => _config.MaxAgeMinutesForPurge_240m,
                1440 => _config.MaxAgeMinutesForPurge_1440m,
                _ => 4320 // Fallback: 3 días para TFs no configurados
            };
            
            int maxBars = maxAgeMinutes / tfMinutes;
            
            if (_config.EnablePerfDiagnostics)
                _logger.Debug($"[PURGE][AGE_CALC] TF={tfMinutes} MaxAgeMinutes={maxAgeMinutes} → MaxBars={maxBars}");
            
            return maxBars;
        }

        /// <summary>
        /// Purga estructuras antiguas cuando se excede MaxStructuresPerTF
        /// Implementa purga inteligente por score, edad y tipo
        /// </summary>
        private void PurgeOldStructuresIfNeeded(int tfMinutes, int barIndex)
        {
            _stateLock.EnterUpgradeableReadLock();
            try
            {
                var structures = _structuresListByTF[tfMinutes];
                int currentBar = barIndex; // Usar el índice de la barra siendo procesada, no el último disponible
                
                // 1. Purga por score mínimo (prioridad alta)
                // Purgar TODAS las estructuras con score bajo (sin importar estado)
                // Una estructura con score < threshold no aporta valor al sistema
                var lowScoreStructures = structures
                    .Where(s => s.Score < _config.MinScoreThreshold)
                    .ToList();

                if (lowScoreStructures.Count > 0)
                {
                    _stateLock.EnterWriteLock();
                    try
                    {
                        foreach (var structure in lowScoreStructures)
                        {
                            RemoveStructureInternal(structure.Id, "Purged_LowScore");
                            
                            // Actualizar estadísticas
                            _stats.TotalPurgedSinceStart++;
                            if (!_stats.PurgedByType.ContainsKey(structure.Type))
                                _stats.PurgedByType[structure.Type] = 0;
                            _stats.PurgedByType[structure.Type]++;
                        }

                        _stats.LastPurgeTime = DateTime.UtcNow;
                        _stats.LastPurgeCount = lowScoreStructures.Count;

                        if (_config.EnablePerfDiagnostics)
                            _logger.Info($"Purgadas {lowScoreStructures.Count} estructuras de TF:{tfMinutes} por score bajo (< {_config.MinScoreThreshold})");
                    }
                    finally
                    {
                        _stateLock.ExitWriteLock();
                    }
                }

                // 2. Purga por edad (estructuras muy antiguas) - PONDERADA POR TF
                int maxAgeBarsForThisTF = GetMaxAgeBarsForTF(tfMinutes);
                var oldStructures = structures
                    .Where(s => (currentBar - s.CreatedAtBarIndex) > maxAgeBarsForThisTF)
                    .ToList();

                if (oldStructures.Count > 0)
                {
                    _stateLock.EnterWriteLock();
                    try
                    {
                        foreach (var structure in oldStructures)
                        {
                            int ageInBars = currentBar - structure.CreatedAtBarIndex;
                            int ageInMinutes = ageInBars * tfMinutes;
                            
                            if (_config.EnablePerfDiagnostics)
                                _logger.Debug($"[PURGE][AGE] TF={tfMinutes} Type={structure.Type} Age={ageInBars}bars ({ageInMinutes}min) > Max={maxAgeBarsForThisTF}bars Score={structure.Score:F2}");
                            
                            RemoveStructureInternal(structure.Id, "Purged_Expired");
                            
                            _stats.TotalPurgedSinceStart++;
                            if (!_stats.PurgedByType.ContainsKey(structure.Type))
                                _stats.PurgedByType[structure.Type] = 0;
                            _stats.PurgedByType[structure.Type]++;
                        }

                        _stats.LastPurgeTime = DateTime.UtcNow;
                        _stats.LastPurgeCount += oldStructures.Count;

                        if (_config.EnablePerfDiagnostics)
                            _logger.Info($"Purgadas {oldStructures.Count} estructuras de TF:{tfMinutes} por edad (> {maxAgeBarsForThisTF} barras = {maxAgeBarsForThisTF * tfMinutes} minutos)");
                    }
                    finally
                    {
                        _stateLock.ExitWriteLock();
                    }
                }

                // 3. Purga por límite de tipo (granular)
                PurgeByTypeLimit(tfMinutes);

                // 4. Purga por límite global (si aún se excede)
                structures = _structuresListByTF[tfMinutes]; // Refrescar después de purgas anteriores
                
                if (structures.Count > _config.MaxStructuresPerTF)
                {
                    _stateLock.EnterWriteLock();
                    try
                    {
                        int countToPurge = structures.Count - _config.MaxStructuresPerTF;
                        
                        // Purgar las estructuras con score más bajo (determinista)
                        var toRemove = structures
                            .OrderBy(s => s.Score)
                            .ThenBy(s => s.TF)                        // TF más bajo primero
                            .ThenByDescending(s => s.CreatedAtBarIndex) // Más antiguo primero
                            .ThenByDescending(s => s.StartTime)       // Más antiguo por tiempo
                            .ThenBy(s => s.Low)
                            .ThenBy(s => s.High)
                            .ThenBy(s => s.Type, StringComparer.Ordinal)
                            .Take(countToPurge)
                            .ToList();

                        foreach (var structure in toRemove)
                        {
                            RemoveStructureInternal(structure.Id, "Purged_GlobalLimit");
                            
                            _stats.TotalPurgedSinceStart++;
                            if (!_stats.PurgedByType.ContainsKey(structure.Type))
                                _stats.PurgedByType[structure.Type] = 0;
                            _stats.PurgedByType[structure.Type]++;
                        }

                        _stats.LastPurgeTime = DateTime.UtcNow;
                        _stats.LastPurgeCount += countToPurge;

                        if (_config.EnablePerfDiagnostics)
                            _logger.Info($"Purgadas {countToPurge} estructuras de TF:{tfMinutes} por límite global (límite: {_config.MaxStructuresPerTF})");
                    }
                    finally
                    {
                        _stateLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                _stateLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Purga estructuras por límite de tipo
        /// Cada tipo de estructura tiene su propio límite máximo
        /// </summary>
        private void PurgeByTypeLimit(int tfMinutes)
        {
            var structures = _structuresListByTF[tfMinutes];

            // Agrupar por tipo
            var byType = structures.GroupBy(s => s.Type).ToList();

            foreach (var group in byType)
            {
                string type = group.Key;
                int count = group.Count();
                int maxForType = GetMaxStructuresByType(type);

                if (count > maxForType)
                {
                    int countToPurge = count - maxForType;

                    // Purgar las estructuras con score más bajo de este tipo (determinista)
                    var toRemove = group
                        .OrderBy(s => s.Score)
                        .ThenBy(s => s.TF)                        // TF más bajo primero
                        .ThenByDescending(s => s.CreatedAtBarIndex) // Más antiguo primero
                        .ThenByDescending(s => s.StartTime)       // Más antiguo por tiempo
                        .ThenBy(s => s.Low)
                        .ThenBy(s => s.High)
                        .ThenBy(s => s.Type, StringComparer.Ordinal)
                        .Take(countToPurge)
                        .ToList();

                    _stateLock.EnterWriteLock();
                    try
                    {
                        foreach (var structure in toRemove)
                        {
                            RemoveStructureInternal(structure.Id, "Purged_TypeLimit");
                            
                            _stats.TotalPurgedSinceStart++;
                            if (!_stats.PurgedByType.ContainsKey(structure.Type))
                                _stats.PurgedByType[structure.Type] = 0;
                            _stats.PurgedByType[structure.Type]++;
                        }

                        _stats.LastPurgeTime = DateTime.UtcNow;
                        _stats.LastPurgeCount += countToPurge;

                        _logger.Info($"Purgadas {countToPurge} estructuras de tipo {type} en TF:{tfMinutes} (límite: {maxForType})");
                    }
                    finally
                    {
                        _stateLock.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Obtiene el límite máximo de estructuras para un tipo específico
        /// </summary>
        private int GetMaxStructuresByType(string type)
        {
            switch (type)
            {
                case "FVG":
                    return _config.MaxStructuresByType_FVG;
                case "OB":
                    return _config.MaxStructuresByType_OB;
                case "SWING":
                    return _config.MaxStructuresByType_Swing;
                case "BOS":
                case "CHoCH":
                    return _config.MaxStructuresByType_BOS;
                case "POI":
                    return _config.MaxStructuresByType_POI;
                case "LIQUIDITY_VOID":
                    return _config.MaxStructuresByType_LV;
                case "LIQUIDITY_GRAB":
                    return _config.MaxStructuresByType_LG;
                case "DOUBLE_TOP":
                    return _config.MaxStructuresByType_Double;
                default:
                    return _config.MaxStructuresPerTF; // Fallback al límite global
            }
        }

        /// <summary>
        /// Purga agresiva de Liquidity Grabs antiguos
        /// Los LG pierden relevancia rápidamente
        /// </summary>
        private void PurgeAggressiveLiquidityGrabs(int tfMinutes)
        {
            if (!_config.EnableAggressivePurgeForLG)
                return;

            _stateLock.EnterUpgradeableReadLock();
            try
            {
                var structures = _structuresListByTF[tfMinutes];
                int currentBar = _provider.GetCurrentBarIndex(tfMinutes);

                var oldGrabs = structures
                    .OfType<LiquidityGrabInfo>()
                    .Where(lg => (currentBar - lg.CreatedAtBarIndex) > _config.LG_MaxAgeBars)
                    .ToList();

                if (oldGrabs.Count > 0)
                {
                    _stateLock.EnterWriteLock();
                    try
                    {
                        foreach (var grab in oldGrabs)
                        {
                            RemoveStructureInternal(grab.Id, "Purged_AggressiveLG");
                            
                            _stats.TotalPurgedSinceStart++;
                            if (!_stats.PurgedByType.ContainsKey(grab.Type))
                                _stats.PurgedByType[grab.Type] = 0;
                            _stats.PurgedByType[grab.Type]++;
                        }

                        _stats.LastPurgeTime = DateTime.UtcNow;
                        _stats.LastPurgeCount += oldGrabs.Count;

                        _logger.Info($"Purga agresiva: {oldGrabs.Count} Liquidity Grabs antiguos en TF:{tfMinutes} (> {_config.LG_MaxAgeBars} barras)");
                    }
                    finally
                    {
                        _stateLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                _stateLock.ExitUpgradeableReadLock();
            }
        }

        // ========================================================================
        // DISPOSAL
        // ========================================================================

        /// <summary>
        /// Libera recursos y guarda estado final (sincrónico)
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed) return;

            _logger.Info("Disposing CoreEngine...");

            // Cancelar guardado asíncrono en progreso
            _saveCancellationTokenSource?.Cancel();

            // Esperar a que termine la tarea de guardado
            try
            {
                _saveTask?.Wait(TimeSpan.FromSeconds(5));
            }
            catch (Exception ex)
            {
                _logger.Exception("Error esperando a guardado final", ex);
            }

            // Guardar estado final sincrónicamente
            if (_config.AutoSaveEnabled && _stateChanged)
            {
                try
                {
                    _logger.Info("Guardando estado final antes de dispose...");
                    SaveStateToJSONAsync().Wait(TimeSpan.FromSeconds(10));
                }
                catch (Exception ex)
                {
                    _logger.Exception("Error guardando estado final", ex);
                }
            }

            // Liberar detectores
            foreach (var detector in _detectors)
            {
                detector.Dispose();
            }

            _stateLock?.Dispose();
            _saveCancellationTokenSource?.Dispose();

            _isDisposed = true;
            _logger.Info("CoreEngine disposed");
        }
    }
}

