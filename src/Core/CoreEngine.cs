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

        /// <summary>Marca si ha habido cambios desde el último guardado</summary>
        private volatile bool _stateChanged;

        /// <summary>Tarea de guardado asíncrono actual (para debounce)</summary>
        private Task _saveTask;

        /// <summary>Token de cancelación para guardado asíncrono</summary>
        private CancellationTokenSource _saveCancellationTokenSource;

        /// <summary>Timestamp del último guardado</summary>
        private DateTime _lastSaveTime;

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

                // Purgar estructuras antiguas si se excede el límite
                PurgeOldStructuresIfNeeded(tfMinutes);

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
        /// Elimina una estructura por ID
        /// </summary>
        public bool RemoveStructure(string id)
        {
            _stateLock.EnterWriteLock();
            try
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
            finally
            {
                _stateLock.ExitWriteLock();
            }
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
                    .Where(poi => poi.IsActive && poi.Score >= minScore)
                    .OrderByDescending(poi => poi.CompositeScore)
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
        // PERSISTENCIA (Skeleton - se implementará en Fase 5)
        // ========================================================================

        /// <summary>
        /// Guarda el estado completo a JSON de forma asíncrona
        /// </summary>
        public Task SaveStateToJSONAsync(string path)
        {
            _logger.Info($"SaveStateToJSONAsync: {path} (TODO Fase 5)");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Carga el estado desde JSON
        /// </summary>
        public void LoadStateFromJSON(string path)
        {
            _logger.Info($"LoadStateFromJSON: {path} (TODO Fase 5)");
        }

        /// <summary>
        /// Programa un guardado asíncrono si hay cambios y ha pasado el intervalo
        /// </summary>
        private void ScheduleSaveIfNeeded()
        {
            // TODO Fase 5: Implementar debounce y guardado asíncrono
        }

        // ========================================================================
        // DIAGNÓSTICOS (Skeleton - se implementará en Fase 2)
        // ========================================================================

        /// <summary>
        /// Ejecuta diagnósticos sintéticos del sistema
        /// Genera escenarios de prueba y valida detección correcta
        /// </summary>
        public void RunSelfDiagnostics()
        {
            _logger.Info("RunSelfDiagnostics() - TODO Fase 2");
        }

        // ========================================================================
        // MANTENIMIENTO
        // ========================================================================

        /// <summary>
        /// Purga estructuras antiguas cuando se excede MaxStructuresPerTF
        /// Elimina estructuras con score más bajo primero
        /// </summary>
        private void PurgeOldStructuresIfNeeded(int tfMinutes)
        {
            _stateLock.EnterUpgradeableReadLock();
            try
            {
                var structures = _structuresListByTF[tfMinutes];
                
                if (structures.Count > _config.MaxStructuresPerTF)
                {
                    _stateLock.EnterWriteLock();
                    try
                    {
                        int countToPurge = structures.Count - _config.MaxStructuresPerTF;
                        
                        var toRemove = structures
                            .OrderBy(s => s.Score)
                            .Take(countToPurge)
                            .ToList();

                        foreach (var structure in toRemove)
                        {
                            RemoveStructure(structure.Id);
                        }

                        _logger.Info($"Purgadas {countToPurge} estructuras de TF:{tfMinutes} (límite: {_config.MaxStructuresPerTF})");
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
            // TODO Fase 5: SaveStateToJSONAsync(...).Wait();

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

