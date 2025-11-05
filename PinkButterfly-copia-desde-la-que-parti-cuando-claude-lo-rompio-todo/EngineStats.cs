// ============================================================================
// EngineStats.cs
// PinkButterfly CoreBrain - Estadísticas del motor
// 
// Modelo de datos para estadísticas del motor:
// - Total de estructuras por tipo
// - Memoria utilizada
// - Tasa de detección por detector
// - Tasa de purga
// - Tiempo promedio de procesamiento
// - Última carga/guardado
// - Información de persistencia
//
// Serializable a JSON con Newtonsoft.Json
// ============================================================================

using System;
using System.Collections.Generic;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Estadísticas del motor CoreBrain
    /// Proporciona información sobre el estado actual del sistema
    /// </summary>
    public class EngineStats
    {
        /// <summary>Timestamp de generación de estas estadísticas (UTC)</summary>
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        
        /// <summary>Versión del motor</summary>
        public string EngineVersion { get; set; }
        
        /// <summary>Instrumento analizado</summary>
        public string Instrument { get; set; }
        
        /// <summary>Indica si el motor está inicializado</summary>
        public bool IsInitialized { get; set; }
        
        // ========================================================================
        // ESTADÍSTICAS DE ESTRUCTURAS
        // ========================================================================
        
        /// <summary>Total de estructuras en memoria</summary>
        public int TotalStructures { get; set; }
        
        /// <summary>Total de estructuras activas (IsActive=true)</summary>
        public int TotalActiveStructures { get; set; }
        
        /// <summary>Total de estructuras completadas (IsCompleted=true)</summary>
        public int TotalCompletedStructures { get; set; }
        
        /// <summary>
        /// Número de estructuras por tipo
        /// Clave: tipo de estructura (FVG, OB, SWING, etc.)
        /// Valor: cantidad
        /// </summary>
        public Dictionary<string, int> StructuresByType { get; set; } = new Dictionary<string, int>();
        
        /// <summary>
        /// Número de estructuras por timeframe
        /// Clave: timeframe en minutos
        /// Valor: cantidad
        /// </summary>
        public Dictionary<int, int> StructuresByTF { get; set; } = new Dictionary<int, int>();
        
        /// <summary>Score promedio de todas las estructuras</summary>
        public double AverageScore { get; set; }
        
        /// <summary>Score máximo entre todas las estructuras</summary>
        public double MaxScore { get; set; }
        
        /// <summary>Score mínimo entre todas las estructuras</summary>
        public double MinScore { get; set; }
        
        // ========================================================================
        // ESTADÍSTICAS DE DETECCIÓN
        // ========================================================================
        
        /// <summary>
        /// Número de estructuras detectadas por cada detector desde el inicio
        /// Clave: nombre del detector (FVGDetector, SwingDetector, etc.)
        /// Valor: cantidad detectada
        /// </summary>
        public Dictionary<string, int> DetectionsByDetector { get; set; } = new Dictionary<string, int>();
        
        /// <summary>Total de estructuras detectadas desde el inicio</summary>
        public int TotalDetectionsSinceStart { get; set; }
        
        // ========================================================================
        // ESTADÍSTICAS DE PURGA
        // ========================================================================
        
        /// <summary>Total de estructuras purgadas desde el inicio</summary>
        public int TotalPurgedSinceStart { get; set; }
        
        /// <summary>
        /// Número de estructuras purgadas por tipo
        /// Clave: tipo de estructura
        /// Valor: cantidad purgada
        /// </summary>
        public Dictionary<string, int> PurgedByType { get; set; } = new Dictionary<string, int>();
        
        /// <summary>Timestamp de la última purga (UTC)</summary>
        public DateTime? LastPurgeTime { get; set; }
        
        /// <summary>Número de estructuras purgadas en la última purga</summary>
        public int LastPurgeCount { get; set; }
        
        // ========================================================================
        // ESTADÍSTICAS DE PERSISTENCIA
        // ========================================================================
        
        /// <summary>Timestamp del último guardado exitoso (UTC)</summary>
        public DateTime? LastSaveTime { get; set; }
        
        /// <summary>Timestamp de la última carga exitosa (UTC)</summary>
        public DateTime? LastLoadTime { get; set; }
        
        /// <summary>Número de guardados exitosos desde el inicio</summary>
        public int TotalSavesSinceStart { get; set; }
        
        /// <summary>Número de cargas exitosas desde el inicio</summary>
        public int TotalLoadsSinceStart { get; set; }
        
        /// <summary>Indica si el último guardado fue exitoso</summary>
        public bool LastSaveSuccessful { get; set; }
        
        /// <summary>Indica si la última carga fue exitosa</summary>
        public bool LastLoadSuccessful { get; set; }
        
        /// <summary>Mensaje de error del último guardado (si falló)</summary>
        public string LastSaveError { get; set; }
        
        /// <summary>Mensaje de error de la última carga (si falló)</summary>
        public string LastLoadError { get; set; }
        
        /// <summary>Hash de configuración actual</summary>
        public string CurrentConfigHash { get; set; }
        
        /// <summary>Hash de configuración del último estado cargado</summary>
        public string LoadedConfigHash { get; set; }
        
        /// <summary>Indica si el hash de configuración coincidió al cargar</summary>
        public bool ConfigHashMatched { get; set; }
        
        // ========================================================================
        // ESTADÍSTICAS DE PERFORMANCE
        // ========================================================================
        
        /// <summary>Tiempo promedio de procesamiento por barra (milisegundos)</summary>
        public double AverageProcessingTimeMs { get; set; }
        
        /// <summary>Tiempo máximo de procesamiento por barra (milisegundos)</summary>
        public double MaxProcessingTimeMs { get; set; }
        
        /// <summary>Número de barras procesadas desde el inicio</summary>
        public int TotalBarsProcessed { get; set; }
        
        /// <summary>Memoria estimada utilizada (bytes)</summary>
        public long EstimatedMemoryBytes { get; set; }
        
        /// <summary>Memoria estimada utilizada (MB)</summary>
        public double EstimatedMemoryMB => EstimatedMemoryBytes / (1024.0 * 1024.0);
        
        // ========================================================================
        // ESTADÍSTICAS DE BIAS
        // ========================================================================
        
        /// <summary>Bias de mercado actual: "Bullish", "Bearish", "Neutral"</summary>
        public string CurrentMarketBias { get; set; }
        
        /// <summary>Timestamp del último cambio de bias (UTC)</summary>
        public DateTime? LastBiasChangeTime { get; set; }
        
        /// <summary>Número de cambios de bias desde el inicio</summary>
        public int TotalBiasChangesSinceStart { get; set; }
        
        // ========================================================================
        // MÉTODOS
        // ========================================================================
        
        /// <summary>
        /// Genera un resumen de texto de las estadísticas
        /// Útil para logging y debugging
        /// </summary>
        public string GetSummary()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("=== COREBRAIN ENGINE STATS ===");
            sb.AppendLine($"Generated: {GeneratedAt:yyyy-MM-dd HH:mm:ss} UTC");
            sb.AppendLine($"Version: {EngineVersion}");
            sb.AppendLine($"Instrument: {Instrument}");
            sb.AppendLine($"Initialized: {IsInitialized}");
            sb.AppendLine();
            
            sb.AppendLine("--- STRUCTURES ---");
            sb.AppendLine($"Total: {TotalStructures} (Active: {TotalActiveStructures}, Completed: {TotalCompletedStructures})");
            sb.AppendLine($"Score: Avg={AverageScore:F3}, Min={MinScore:F3}, Max={MaxScore:F3}");
            sb.AppendLine("By Type:");
            foreach (var kv in StructuresByType)
            {
                sb.AppendLine($"  {kv.Key}: {kv.Value}");
            }
            sb.AppendLine("By TF:");
            foreach (var kv in StructuresByTF)
            {
                sb.AppendLine($"  {kv.Key}m: {kv.Value}");
            }
            sb.AppendLine();
            
            sb.AppendLine("--- DETECTION ---");
            sb.AppendLine($"Total Detections: {TotalDetectionsSinceStart}");
            foreach (var kv in DetectionsByDetector)
            {
                sb.AppendLine($"  {kv.Key}: {kv.Value}");
            }
            sb.AppendLine();
            
            sb.AppendLine("--- PURGE ---");
            sb.AppendLine($"Total Purged: {TotalPurgedSinceStart}");
            sb.AppendLine($"Last Purge: {LastPurgeTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "Never"} ({LastPurgeCount} items)");
            sb.AppendLine();
            
            sb.AppendLine("--- PERSISTENCE ---");
            sb.AppendLine($"Last Save: {LastSaveTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "Never"} (Success: {LastSaveSuccessful})");
            sb.AppendLine($"Last Load: {LastLoadTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "Never"} (Success: {LastLoadSuccessful})");
            sb.AppendLine($"Total Saves: {TotalSavesSinceStart}, Total Loads: {TotalLoadsSinceStart}");
            sb.AppendLine($"Config Hash Matched: {ConfigHashMatched}");
            sb.AppendLine();
            
            sb.AppendLine("--- PERFORMANCE ---");
            sb.AppendLine($"Bars Processed: {TotalBarsProcessed}");
            sb.AppendLine($"Processing Time: Avg={AverageProcessingTimeMs:F2}ms, Max={MaxProcessingTimeMs:F2}ms");
            sb.AppendLine($"Memory: {EstimatedMemoryMB:F2} MB");
            sb.AppendLine();
            
            sb.AppendLine("--- BIAS ---");
            sb.AppendLine($"Current Bias: {CurrentMarketBias}");
            sb.AppendLine($"Last Change: {LastBiasChangeTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "Never"}");
            sb.AppendLine($"Total Changes: {TotalBiasChangesSinceStart}");
            
            return sb.ToString();
        }
    }
}



