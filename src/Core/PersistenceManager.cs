// ============================================================================
// PersistenceManager.cs
// PinkButterfly CoreBrain - Gestor de persistencia
// 
// Responsable de:
// - Serialización/deserialización de estado a JSON
// - Validación de hash de configuración
// - Manejo de versiones y compatibilidad
// - Escritura/lectura asíncrona de archivos
// - Manejo de errores y logging
//
// Usa Newtonsoft.Json con TypeNameHandling.Auto para polimorfismo
// Compatible con .NET Framework 4.8
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Gestor de persistencia del estado del CoreEngine
    /// Maneja serialización/deserialización JSON con validación de configuración
    /// </summary>
    public class PersistenceManager
    {
        private readonly ILogger _logger;
        private readonly EngineConfig _config;
        private readonly object _fileLock = new object(); // Lock para escritura concurrente

        // ========================================================================
        // CONSTRUCTOR
        // ========================================================================

        public PersistenceManager(EngineConfig config, ILogger logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // ========================================================================
        // MODELOS DE PERSISTENCIA
        // ========================================================================

        /// <summary>
        /// Modelo raíz del archivo JSON de estado
        /// Contiene metadata y todas las estructuras organizadas por timeframe
        /// </summary>
        public class BrainState
        {
            /// <summary>Versión del formato de persistencia</summary>
            public string Version { get; set; } = "1.1";
            
            /// <summary>Instrumento analizado</summary>
            public string Instrument { get; set; }
            
            /// <summary>Hash SHA256 de la configuración del motor</summary>
            public string EngineConfigHash { get; set; }
            
            /// <summary>Timestamp de la última actualización (UTC)</summary>
            public DateTime LastUpdatedUTC { get; set; }
            
            /// <summary>Bias de mercado actual</summary>
            public string CurrentMarketBias { get; set; }
            
            /// <summary>
            /// Estructuras organizadas por timeframe
            /// Clave: timeframe en minutos
            /// Valor: lista de estructuras
            /// </summary>
            public Dictionary<int, List<StructureBase>> StructuresByTF { get; set; } = new Dictionary<int, List<StructureBase>>();
            
            /// <summary>Estadísticas del motor al momento del guardado</summary>
            public EngineStats Stats { get; set; }
        }

        // ========================================================================
        // SERIALIZACIÓN
        // ========================================================================

        /// <summary>
        /// Serializa el estado del motor a JSON
        /// Usa TypeNameHandling.Auto para polimorfismo de StructureBase
        /// </summary>
        public string SerializeState(
            Dictionary<int, List<StructureBase>> structuresByTF,
            string instrument,
            string currentMarketBias,
            EngineStats stats)
        {
            try
            {
                var state = new BrainState
                {
                    Version = "1.1",
                    Instrument = instrument,
                    EngineConfigHash = _config.GetHash(),
                    LastUpdatedUTC = DateTime.UtcNow,
                    CurrentMarketBias = currentMarketBias,
                    StructuresByTF = structuresByTF,
                    Stats = stats
                };

                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto, // Polimorfismo para StructureBase
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                };

                string json = JsonConvert.SerializeObject(state, settings);
                
                _logger.Info($"Estado serializado: {structuresByTF.Sum(kv => kv.Value.Count)} estructuras, {json.Length} bytes");
                
                return json;
            }
            catch (Exception ex)
            {
                _logger.Exception("Error serializando estado", ex);
                throw;
            }
        }

        /// <summary>
        /// Deserializa el estado desde JSON
        /// Valida el hash de configuración si está habilitado
        /// </summary>
        public BrainState DeserializeState(string json, bool forceLoad = false)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    // Ignorar diferencias de versión del assembly
                    SerializationBinder = new IgnoreAssemblyVersionBinder()
                };

                var state = JsonConvert.DeserializeObject<BrainState>(json, settings);
                
                if (state == null)
                {
                    throw new InvalidOperationException("Estado deserializado es null");
                }

                _logger.Info($"Estado deserializado: Version={state.Version}, Instrument={state.Instrument}, " +
                            $"Structures={state.StructuresByTF.Sum(kv => kv.Value.Count)}, " +
                            $"LastUpdated={state.LastUpdatedUTC:yyyy-MM-dd HH:mm:ss}");

                // Validar hash de configuración
                if (_config.ValidateConfigHashOnLoad && !forceLoad)
                {
                    string currentHash = _config.GetHash();
                    
                    if (state.EngineConfigHash != currentHash)
                    {
                        string errorMsg = $"Hash de configuración no coincide. " +
                                        $"Guardado: {state.EngineConfigHash?.Substring(0, 8)}..., " +
                                        $"Actual: {currentHash.Substring(0, 8)}... " +
                                        $"Usa forceLoad=true para cargar de todos modos.";
                        
                        _logger.Warning(errorMsg);
                        throw new InvalidOperationException(errorMsg);
                    }
                    
                    _logger.Info("Hash de configuración validado correctamente");
                }
                else if (forceLoad)
                {
                    _logger.Warning("Carga forzada: se omitió la validación de hash de configuración");
                }

                return state;
            }
            catch (JsonException ex)
            {
                _logger.Exception("Error deserializando JSON", ex);
                throw new InvalidOperationException("Archivo de estado corrupto o incompatible", ex);
            }
            catch (Exception ex)
            {
                _logger.Exception("Error deserializando estado", ex);
                throw;
            }
        }

        // ========================================================================
        // GUARDADO ASÍNCRONO
        // ========================================================================

        /// <summary>
        /// Guarda el estado a archivo JSON de forma asíncrona
        /// Crea el directorio si no existe
        /// </summary>
        public async Task SaveStateToFileAsync(
            Dictionary<int, List<StructureBase>> structuresByTF,
            string instrument,
            string currentMarketBias,
            EngineStats stats,
            string filePath)
        {
            // Lock para evitar escrituras concurrentes al mismo archivo
            lock (_fileLock)
            {
                try
                {
                    // Expandir ruta si es relativa
                    string fullPath = ExpandPath(filePath);
                    
                    _logger.Info($"Guardando estado a: {fullPath}");

                    // Serializar estado
                    string json = SerializeState(structuresByTF, instrument, currentMarketBias, stats);

                    // Crear directorio si no existe
                    string directory = Path.GetDirectoryName(fullPath);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                        _logger.Info($"Directorio creado: {directory}");
                    }

                    // Escribir archivo síncronamente (dentro del lock)
                    File.WriteAllText(fullPath, json, Encoding.UTF8);

                    _logger.Info($"Estado guardado exitosamente: {fullPath} ({json.Length} bytes)");
                }
                catch (Exception ex)
                {
                    _logger.Exception($"Error guardando estado a {filePath}", ex);
                    throw;
                }
            }

            // Retornar tarea completada
            await Task.CompletedTask;
        }

        // ========================================================================
        // CARGA SÍNCRONA
        // ========================================================================

        /// <summary>
        /// Carga el estado desde archivo JSON
        /// </summary>
        public BrainState LoadStateFromFile(string filePath, bool forceLoad = false)
        {
            try
            {
                // Expandir ruta si es relativa
                string fullPath = ExpandPath(filePath);
                
                _logger.Info($"Cargando estado desde: {fullPath}");

                if (!File.Exists(fullPath))
                {
                    throw new FileNotFoundException($"Archivo de estado no encontrado: {fullPath}");
                }

                // Leer archivo
                string json = File.ReadAllText(fullPath, Encoding.UTF8);
                
                _logger.Info($"Archivo leído: {json.Length} bytes");

                // Deserializar
                var state = DeserializeState(json, forceLoad);
                
                _logger.Info($"Estado cargado exitosamente: {state.StructuresByTF.Sum(kv => kv.Value.Count)} estructuras");

                return state;
            }
            catch (Exception ex)
            {
                _logger.Exception($"Error cargando estado desde {filePath}", ex);
                throw;
            }
        }

        // ========================================================================
        // UTILIDADES
        // ========================================================================

        /// <summary>
        /// Expande rutas relativas a rutas absolutas
        /// Reemplaza "Documents" por la ruta real de Documents del usuario
        /// </summary>
        private string ExpandPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            // Si es ruta absoluta, retornar tal cual
            if (Path.IsPathRooted(path))
                return path;

            // Reemplazar "Documents" por la ruta real
            if (path.StartsWith("Documents", StringComparison.OrdinalIgnoreCase))
            {
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                path = path.Replace("Documents", documentsPath);
                path = path.Replace("/", "\\"); // Normalizar separadores en Windows
            }

            return path;
        }

        /// <summary>
        /// Verifica si existe un archivo de estado
        /// </summary>
        public bool StateFileExists(string filePath)
        {
            try
            {
                string fullPath = ExpandPath(filePath);
                return File.Exists(fullPath);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Obtiene información del archivo de estado (tamaño, fecha modificación)
        /// </summary>
        public FileInfo GetStateFileInfo(string filePath)
        {
            string fullPath = ExpandPath(filePath);
            
            if (!File.Exists(fullPath))
                return null;

            return new FileInfo(fullPath);
        }

        /// <summary>
        /// Crea un backup del archivo de estado actual
        /// </summary>
        public void BackupStateFile(string filePath)
        {
            try
            {
                string fullPath = ExpandPath(filePath);
                
                if (!File.Exists(fullPath))
                {
                    _logger.Warning($"No se puede hacer backup: archivo no existe ({fullPath})");
                    return;
                }

                string backupPath = fullPath + $".backup_{DateTime.Now:yyyyMMdd_HHmmss}";
                File.Copy(fullPath, backupPath, overwrite: false);
                
                _logger.Info($"Backup creado: {backupPath}");
            }
            catch (Exception ex)
            {
                _logger.Exception($"Error creando backup de {filePath}", ex);
            }
        }
    }

    /// <summary>
    /// SerializationBinder personalizado que ignora diferencias de versión y nombre de ensamblado
    /// Permite deserializar JSON guardado con versiones anteriores del código
    /// </summary>
    internal class IgnoreAssemblyVersionBinder : Newtonsoft.Json.Serialization.ISerializationBinder
    {
        public Type BindToType(string assemblyName, string typeName)
        {
            // Extraer solo el nombre del tipo (sin el ensamblado)
            // Ejemplo: "NinjaTrader.NinjaScript.Indicators.PinkButterfly.OrderBlockInfo"
            string typeOnly = typeName;
            
            // Buscar el tipo en el ensamblado actual (ignorando el nombre del ensamblado del JSON)
            Type type = Type.GetType($"{typeOnly}, {typeof(PersistenceManager).Assembly.FullName}");
            
            if (type == null)
            {
                // Si no se encuentra, intentar sin especificar ensamblado (búsqueda en todos los ensamblados cargados)
                type = Type.GetType(typeOnly);
            }
            
            return type;
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = serializedType.FullName;
        }
    }
}



