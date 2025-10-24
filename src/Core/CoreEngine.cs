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

        // ========================================================================
        // EVENTOS PÚBLICOS
        // ========================================================================

        /// <summary>Evento disparado cuando se agrega una nueva estructura</summary>
        public event Action<StructureBase> OnStructureAdded;

        /// <summary>Evento disparado cuando se actualiza una estructura existente</summary>
        public event Action<StructureBase> OnStructureUpdated;

        /// <summary>Evento disparado cuando se elimina una estructura</summary>
        public event Action<StructureBase> OnStructureRemoved;

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
            if (!_isInitialized)
            {
                _logger.Warning($"OnBarClose llamado antes de Initialize() - TF:{tfMinutes} Bar:{barIndex}");
                return;
            }

            if (_config.EnableDebug)
                _logger.Debug($"OnBarClose - TF:{tfMinutes} Bar:{barIndex}");

            try
            {
                // Ejecutar todos los detectores para este timeframe
                foreach (var detector in _detectors)
                {
                    detector.OnBarClose(tfMinutes, barIndex, this);
                }

                // Actualizar scores de estructuras afectadas por proximidad
                UpdateProximityScores(tfMinutes);

                // Purgar estructuras antiguas si está habilitado
                if (_config.EnableAutoPurge)
                {
                PurgeOldStructuresIfNeeded(tfMinutes);
                    PurgeAggressiveLiquidityGrabs(tfMinutes);
                }

                // Programar guardado asíncrono si hay cambios
                ScheduleSaveIfNeeded();
            }
            catch (Exception ex)
            {
                _logger.Exception($"Error en OnBarClose - TF:{tfMinutes} Bar:{barIndex}", ex);
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

                // Calcular score inicial
                int currentBarIndex = _provider.GetCurrentBarIndex(structure.TF);
                structure.Score = _scoringEngine.CalculateScore(structure, currentBarIndex, _currentMarketBias);

                _stateChanged = true;

                if (_config.EnableDebug)
                    _logger.Debug($"Estructura agregada: {structure.Type} {structure.Id} TF:{structure.TF} " +
                                 $"[{structure.Low:F2}-{structure.High:F2}]");

                // Disparar evento
                OnStructureAdded?.Invoke(structure);
            }
            finally
            {
                _stateLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Actualiza una estructura existente
        /// </summary>
        public void UpdateStructure(StructureBase structure)
        {
            if (structure == null)
                throw new ArgumentNullException(nameof(structure));

            _stateLock.EnterWriteLock();
            try
            {
                if (!_structuresById.ContainsKey(structure.Id))
                {
                    _logger.Warning($"UpdateStructure: estructura {structure.Id} no existe - use AddStructure()");
                    return;
                }

                // Actualizar referencia (ya está en las colecciones)
                _structuresById[structure.Id] = structure;

                // Recalcular score
                int currentBarIndex = _provider.GetCurrentBarIndex(structure.TF);
                structure.Score = _scoringEngine.CalculateScore(structure, currentBarIndex, _currentMarketBias);

                _stateChanged = true;

                if (_config.EnableDebug)
                    _logger.Debug($"Estructura actualizada: {structure.Type} {structure.Id} Score:{structure.Score:F3}");

                // Disparar evento
                OnStructureUpdated?.Invoke(structure);
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
                return RemoveStructureInternal(id);
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
        private bool RemoveStructureInternal(string id)
            {
                if (!_structuresById.TryGetValue(id, out var structure))
                    return false;

                // Remover de lista
                _structuresListByTF[structure.TF].Remove(structure);

                // Remover de interval tree
                _intervalTreesByTF[structure.TF].RemoveByData(structure.Low, structure.High, structure);

                // Remover de diccionario
                _structuresById.Remove(id);

                _stateChanged = true;

                if (_config.EnableDebug)
                    _logger.Debug($"Estructura eliminada: {structure.Type} {id}");

                // Disparar evento
                OnStructureRemoved?.Invoke(structure);

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

                return query.OrderByDescending(d => d.Score).ToList();
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
        private void UpdateProximityScores(int tfMinutes)
        {
            // TODO Fase 2: Implementar actualización de scores por proximidad
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

                // Estructuras por tipo
                _stats.StructuresByType.Clear();
                foreach (var structure in _structuresById.Values)
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
        /// Purga estructuras antiguas cuando se excede MaxStructuresPerTF
        /// Implementa purga inteligente por score, edad y tipo
        /// </summary>
        private void PurgeOldStructuresIfNeeded(int tfMinutes)
        {
            _stateLock.EnterUpgradeableReadLock();
            try
            {
                var structures = _structuresListByTF[tfMinutes];
                int currentBar = _provider.GetCurrentBarIndex(tfMinutes);
                
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
                            RemoveStructureInternal(structure.Id);
                            
                            // Actualizar estadísticas
                            _stats.TotalPurgedSinceStart++;
                            if (!_stats.PurgedByType.ContainsKey(structure.Type))
                                _stats.PurgedByType[structure.Type] = 0;
                            _stats.PurgedByType[structure.Type]++;
                        }

                        _stats.LastPurgeTime = DateTime.UtcNow;
                        _stats.LastPurgeCount = lowScoreStructures.Count;

                        _logger.Info($"Purgadas {lowScoreStructures.Count} estructuras de TF:{tfMinutes} por score bajo (< {_config.MinScoreThreshold})");
                    }
                    finally
                    {
                        _stateLock.ExitWriteLock();
                    }
                }

                // 2. Purga por edad (estructuras muy antiguas)
                // Purgar estructuras antiguas sin importar estado
                var oldStructures = structures
                    .Where(s => (currentBar - s.CreatedAtBarIndex) > _config.MaxAgeBarsForPurge)
                    .ToList();

                if (oldStructures.Count > 0)
                {
                    _stateLock.EnterWriteLock();
                    try
                    {
                        foreach (var structure in oldStructures)
                        {
                            RemoveStructureInternal(structure.Id);
                            
                            _stats.TotalPurgedSinceStart++;
                            if (!_stats.PurgedByType.ContainsKey(structure.Type))
                                _stats.PurgedByType[structure.Type] = 0;
                            _stats.PurgedByType[structure.Type]++;
                        }

                        _stats.LastPurgeTime = DateTime.UtcNow;
                        _stats.LastPurgeCount += oldStructures.Count;

                        _logger.Info($"Purgadas {oldStructures.Count} estructuras de TF:{tfMinutes} por edad (> {_config.MaxAgeBarsForPurge} barras)");
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
                        
                        // Purgar las estructuras con score más bajo
                        var toRemove = structures
                            .OrderBy(s => s.Score)
                            .Take(countToPurge)
                            .ToList();

                        foreach (var structure in toRemove)
                        {
                            RemoveStructureInternal(structure.Id);
                            
                            _stats.TotalPurgedSinceStart++;
                            if (!_stats.PurgedByType.ContainsKey(structure.Type))
                                _stats.PurgedByType[structure.Type] = 0;
                            _stats.PurgedByType[structure.Type]++;
                        }

                        _stats.LastPurgeTime = DateTime.UtcNow;
                        _stats.LastPurgeCount += countToPurge;

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

                    // Purgar las estructuras con score más bajo de este tipo
                    var toRemove = group
                        .OrderBy(s => s.Score)
                        .Take(countToPurge)
                        .ToList();

                    _stateLock.EnterWriteLock();
                    try
                    {
                        foreach (var structure in toRemove)
                        {
                            RemoveStructureInternal(structure.Id);
                            
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
                            RemoveStructureInternal(grab.Id);
                            
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

