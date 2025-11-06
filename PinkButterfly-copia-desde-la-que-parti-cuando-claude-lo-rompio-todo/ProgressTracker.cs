// ============================================================================
// ProgressTracker.cs
// PinkButterfly CoreBrain - Sistema de seguimiento de progreso
// 
// Proporciona información en tiempo real sobre el progreso del procesamiento:
// - Porcentaje de avance
// - Tiempo transcurrido y estimado restante
// - Velocidad de procesamiento (barras/min)
// - Estadísticas de estructuras y guardados
// - Barra de progreso visual ASCII
//
// Diseñado para procesos largos de backtesting/análisis histórico
// ============================================================================

using System;
using System.Text;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Tracker de progreso para procesos largos de análisis histórico
    /// Proporciona información detallada sobre avance, tiempo y rendimiento
    /// </summary>
    public class ProgressTracker
    {
        // ========================================================================
        // CAMPOS PRIVADOS
        // ========================================================================

        private readonly int _totalBars;
        private readonly DateTime _startTime;
        private readonly ILogger _logger;
        private readonly EngineConfig _config;

        private int _lastReportedBar;
        private DateTime _lastReportTime;

        // ========================================================================
        // PROPIEDADES PÚBLICAS
        // ========================================================================

        /// <summary>Barra actual siendo procesada</summary>
        public int CurrentBar { get; private set; }

        /// <summary>Total de barras a procesar</summary>
        public int TotalBars => _totalBars;

        /// <summary>Porcentaje de progreso (0.0 - 100.0)</summary>
        public double ProgressPercentage => _totalBars > 0 ? (CurrentBar * 100.0 / _totalBars) : 0.0;

        /// <summary>Tiempo transcurrido desde el inicio</summary>
        public TimeSpan ElapsedTime => DateTime.Now - _startTime;

        /// <summary>Tiempo estimado restante</summary>
        public TimeSpan EstimatedTimeRemaining
        {
            get
            {
                if (CurrentBar <= 0 || _totalBars <= 0) return TimeSpan.Zero;
                
                double elapsed = ElapsedTime.TotalSeconds;
                double rate = CurrentBar / elapsed; // barras por segundo
                int remaining = _totalBars - CurrentBar;
                
                return TimeSpan.FromSeconds(remaining / rate);
            }
        }

        /// <summary>Velocidad de procesamiento (barras por minuto)</summary>
        public double BarsPerMinute
        {
            get
            {
                double elapsed = ElapsedTime.TotalMinutes;
                return elapsed > 0 ? CurrentBar / elapsed : 0.0;
            }
        }

        /// <summary>Hora estimada de finalización (ETA)</summary>
        public DateTime EstimatedCompletionTime => DateTime.Now + EstimatedTimeRemaining;

        // ========================================================================
        // CONSTRUCTOR
        // ========================================================================

        /// <summary>
        /// Constructor del ProgressTracker
        /// </summary>
        /// <param name="totalBars">Total de barras a procesar</param>
        /// <param name="logger">Logger para output</param>
        /// <param name="config">Configuración del motor</param>
        public ProgressTracker(int totalBars, ILogger logger, EngineConfig config)
        {
            _totalBars = totalBars;
            _logger = logger;
            _config = config;
            _startTime = DateTime.Now;
            _lastReportTime = _startTime;
            CurrentBar = 0;
            _lastReportedBar = 0;
        }

        // ========================================================================
        // MÉTODOS PÚBLICOS
        // ========================================================================

        /// <summary>
        /// Actualiza el progreso a la barra actual
        /// </summary>
        /// <param name="currentBar">Índice de barra actual</param>
        public void Update(int currentBar)
        {
            CurrentBar = currentBar;
        }

        /// <summary>
        /// Verifica si debe reportar progreso basado en configuración
        /// </summary>
        /// <returns>True si debe reportar</returns>
        public bool ShouldReport()
        {
            if (!_config.ShowProgressBar) return false;

            // Reportar cada N barras
            bool barThreshold = (CurrentBar - _lastReportedBar) >= _config.ProgressReportEveryNBars;

            // Reportar cada X minutos
            bool timeThreshold = (DateTime.Now - _lastReportTime).TotalMinutes >= _config.ProgressReportEveryMinutes;

            return barThreshold || timeThreshold;
        }

        /// <summary>
        /// Genera reporte de progreso y lo envía al logger
        /// </summary>
        /// <param name="structureCount">Cantidad de estructuras detectadas</param>
        /// <param name="saveCount">Cantidad de guardados realizados</param>
        public void Report(int structureCount, int saveCount)
        {
            if (!_config.ShowProgressBar) return;

            _lastReportedBar = CurrentBar;
            _lastReportTime = DateTime.Now;

            string report = GenerateProgressReport(structureCount, saveCount);
            _logger.Info(report);
        }

        /// <summary>
        /// Genera reporte final al completar el procesamiento
        /// </summary>
        /// <param name="structureCount">Total de estructuras detectadas</param>
        /// <param name="saveCount">Total de guardados realizados</param>
        public void ReportCompletion(int structureCount, int saveCount)
        {
            if (!_config.ShowProgressBar) return;

            string report = GenerateCompletionReport(structureCount, saveCount);
            _logger.Info(report);
        }

        // ========================================================================
        // MÉTODOS PRIVADOS - GENERACIÓN DE REPORTES
        // ========================================================================

        /// <summary>
        /// Genera el reporte de progreso con toda la información
        /// </summary>
        private string GenerateProgressReport(int structureCount, int saveCount)
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("╔════════════════════════════════════════════════════════════════╗");
            sb.AppendLine("║                    PROGRESO DE ANÁLISIS                        ║");
            sb.AppendLine("╠════════════════════════════════════════════════════════════════╣");

            // Barra de progreso visual
            string progressBar = GenerateProgressBar(50);
            sb.AppendLine($"║ {progressBar} ║");

            // Porcentaje y barras
            sb.AppendLine($"║ Progreso: {ProgressPercentage:F1}% | Barra {CurrentBar:N0}/{TotalBars:N0}".PadRight(63) + "║");

            // Tiempo
            string elapsed = FormatTimeSpan(ElapsedTime);
            string remaining = FormatTimeSpan(EstimatedTimeRemaining);
            sb.AppendLine($"║ ⏱️  Tiempo: {elapsed} | Restante: ~{remaining}".PadRight(63) + "║");

            // ETA
            string eta = EstimatedCompletionTime.ToString("HH:mm:ss");
            sb.AppendLine($"║ 🎯 ETA: {eta}".PadRight(63) + "║");

            // Estadísticas
            sb.AppendLine($"║ 📊 Estructuras: {structureCount:N0} | Guardados: {saveCount:N0}".PadRight(63) + "║");

            // Velocidad
            sb.AppendLine($"║ 🚀 Velocidad: {BarsPerMinute:F1} barras/min".PadRight(63) + "║");

            sb.AppendLine("╚════════════════════════════════════════════════════════════════╝");

            return sb.ToString();
        }

        /// <summary>
        /// Genera el reporte de finalización
        /// </summary>
        private string GenerateCompletionReport(int structureCount, int saveCount)
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("╔════════════════════════════════════════════════════════════════╗");
            sb.AppendLine("║                  ✅ ANÁLISIS COMPLETADO                        ║");
            sb.AppendLine("╠════════════════════════════════════════════════════════════════╣");

            // Barra completa
            string progressBar = GenerateProgressBar(50, true);
            sb.AppendLine($"║ {progressBar} ║");

            // Resumen
            sb.AppendLine($"║ 📊 Barras procesadas: {TotalBars:N0}".PadRight(63) + "║");
            sb.AppendLine($"║ 🔍 Estructuras detectadas: {structureCount:N0}".PadRight(63) + "║");
            sb.AppendLine($"║ 💾 Guardados realizados: {saveCount:N0}".PadRight(63) + "║");

            // Tiempo total
            string totalTime = FormatTimeSpan(ElapsedTime);
            sb.AppendLine($"║ ⏱️  Tiempo total: {totalTime}".PadRight(63) + "║");

            // Velocidad promedio
            sb.AppendLine($"║ 🚀 Velocidad promedio: {BarsPerMinute:F1} barras/min".PadRight(63) + "║");

            // Recomendación si hay muchos guardados
            if (saveCount > 50)
            {
                sb.AppendLine("║                                                                ║");
                sb.AppendLine($"║ ⚠️  OPTIMIZACIÓN: {saveCount} guardados es excesivo".PadRight(63) + "║");
                sb.AppendLine("║    Recomendado: Reducir frecuencia a ~10 guardados            ║");
                sb.AppendLine("║    Esto mejorará el rendimiento ~47x                          ║");
            }

            sb.AppendLine("╚════════════════════════════════════════════════════════════════╝");

            return sb.ToString();
        }

        /// <summary>
        /// Genera barra de progreso visual ASCII
        /// </summary>
        /// <param name="width">Ancho de la barra en caracteres</param>
        /// <param name="forceComplete">Forzar barra completa (para reporte final)</param>
        /// <returns>Barra de progreso formateada</returns>
        private string GenerateProgressBar(int width, bool forceComplete = false)
        {
            double percentage = forceComplete ? 100.0 : ProgressPercentage;
            // Clamp porcentaje y longitudes para evitar valores negativos o overflow
            if (percentage < 0.0) percentage = 0.0;
            if (percentage > 100.0) percentage = 100.0;

            int filled = (int)Math.Round(width * percentage / 100.0);
            if (filled < 0) filled = 0;
            if (filled > width) filled = width;

            int empty = width - filled;
            if (empty < 0) empty = 0;

            string bar = new string('█', filled) + new string('░', empty);
            return $"{bar} {percentage:F1}%";
        }

        /// <summary>
        /// Formatea un TimeSpan de manera legible
        /// </summary>
        /// <param name="ts">TimeSpan a formatear</param>
        /// <returns>String formateado (ej: "2h 35m" o "45m 12s")</returns>
        private string FormatTimeSpan(TimeSpan ts)
        {
            if (ts.TotalHours >= 1)
                return $"{(int)ts.TotalHours}h {ts.Minutes}m";
            else if (ts.TotalMinutes >= 1)
                return $"{(int)ts.TotalMinutes}m {ts.Seconds}s";
            else
                return $"{ts.Seconds}s";
        }
    }
}

