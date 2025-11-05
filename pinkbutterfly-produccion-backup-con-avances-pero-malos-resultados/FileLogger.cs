using System;
using System.IO;
using System.Text;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Logger que escribe a archivos en disco con timestamp
    /// Permite capturar logs completos sin limitaciones del Output de NinjaTrader
    /// </summary>
    public class FileLogger : ILogger
    {
        private readonly string _logFilePath;
        private readonly object _lockObject = new object();
        private readonly bool _enableFileLogging;
        private readonly ILogger _consoleLogger; // Logger original para mantener output en consola
        
        public LogLevel MinLevel { get; set; } = LogLevel.Info;

        /// <summary>
        /// Constructor del FileLogger
        /// </summary>
        /// <param name="logDirectory">Directorio donde se guardarán los logs (ej: "logs/")</param>
        /// <param name="logPrefix">Prefijo para el nombre del archivo (ej: "backtest")</param>
        /// <param name="consoleLogger">Logger de consola para mantener output en NinjaTrader</param>
        /// <param name="enableFileLogging">Si false, solo usa el consoleLogger</param>
        public FileLogger(string logDirectory, string logPrefix, ILogger consoleLogger, bool enableFileLogging = true)
        {
            _consoleLogger = consoleLogger;
            _enableFileLogging = enableFileLogging;

            if (_enableFileLogging)
            {
                try
                {
                    // Crear directorio si no existe
                    if (!Directory.Exists(logDirectory))
                    {
                        Directory.CreateDirectory(logDirectory);
                    }

                    // Generar nombre de archivo con timestamp
                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    string fileName = $"{logPrefix}_{timestamp}.log";
                    _logFilePath = Path.Combine(logDirectory, fileName);

                    // Escribir header del archivo
                    WriteToFile($"=== PinkButterfly Backtest Log ===");
                    WriteToFile($"Inicio: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    WriteToFile($"Archivo: {_logFilePath}");
                    WriteToFile($"========================================\n");

                    _consoleLogger?.Info($"[FileLogger] Logging activado: {_logFilePath}");
                }
                catch (Exception ex)
                {
                    _consoleLogger?.Error($"[FileLogger] Error inicializando archivo de log: {ex.Message}");
                    _enableFileLogging = false;
                }
            }
        }

        /// <summary>
        /// Escribe una línea al archivo de log (thread-safe)
        /// </summary>
        private void WriteToFile(string message)
        {
            if (!_enableFileLogging) return;

            lock (_lockObject)
            {
                try
                {
                    File.AppendAllText(_logFilePath, message + Environment.NewLine, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    _consoleLogger?.Error($"[FileLogger] Error escribiendo al archivo: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Formatea el mensaje con timestamp y nivel
        /// </summary>
        private string FormatMessage(string level, string message)
        {
            return $"[{DateTime.Now:HH:mm:ss.fff}] [{level}] {message}";
        }

        // Implementación de ILogger
        public void Info(string message)
        {
            string formatted = FormatMessage("INFO", message);
            WriteToFile(formatted);
            _consoleLogger?.Info(message); // También escribir a consola
        }

        public void Warning(string message)
        {
            string formatted = FormatMessage("WARN", message);
            WriteToFile(formatted);
            _consoleLogger?.Warning(message);
        }

        public void Error(string message)
        {
            string formatted = FormatMessage("ERROR", message);
            WriteToFile(formatted);
            _consoleLogger?.Error(message);
        }

        public void Debug(string message)
        {
            if (MinLevel <= LogLevel.Debug)
            {
                string formatted = FormatMessage("DEBUG", message);
                WriteToFile(formatted);
                _consoleLogger?.Debug(message);
            }
        }
        
        public void Exception(string message, System.Exception exception)
        {
            if (MinLevel <= LogLevel.Error)
            {
                string formatted = FormatMessage("EXCEPTION", message);
                WriteToFile(formatted);
                WriteToFile($"  {exception.GetType().Name}: {exception.Message}");
                WriteToFile($"  Stack: {exception.StackTrace}");
                _consoleLogger?.Exception(message, exception);
            }
        }

        /// <summary>
        /// Escribe un separador visual en el log
        /// </summary>
        public void WriteSeparator(string title = "")
        {
            string separator = string.IsNullOrEmpty(title)
                ? "=========================================="
                : $"========== {title} ==========";
            
            WriteToFile(separator);
            _consoleLogger?.Info(separator);
        }

        /// <summary>
        /// Escribe el footer y cierra el log
        /// </summary>
        public void Close()
        {
            if (_enableFileLogging)
            {
                WriteSeparator();
                WriteToFile($"Fin: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                WriteToFile($"========================================");
                _consoleLogger?.Info($"[FileLogger] Log guardado: {_logFilePath}");
            }
        }

        /// <summary>
        /// Obtiene la ruta del archivo de log actual
        /// </summary>
        public string GetLogFilePath()
        {
            return _logFilePath;
        }
    }
}

