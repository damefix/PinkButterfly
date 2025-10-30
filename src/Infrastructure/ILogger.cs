// ============================================================================
// ILogger.cs
// PinkButterfly CoreBrain - Interface de logging
// 
// Sistema de logging simple para el CoreEngine y detectores
// Niveles: Debug, Info, Warning, Error
// 
// Implementación por defecto usa NinjaTrader Output window
// Puede reemplazarse con implementaciones que escriban a archivo, etc.
// ============================================================================

using System;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Niveles de logging
    /// </summary>
    public enum LogLevel
    {
        Debug = 0,
        Info = 1,
        Warning = 2,
        Error = 3
    }

    /// <summary>
    /// Interface de logging para el CoreEngine y detectores
    /// Permite abstraer el destino de los logs (Output window, archivo, etc.)
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log de nivel Debug (solo si EnableDebug = true)
        /// Usado para información detallada de debugging
        /// </summary>
        /// <param name="message">Mensaje a loggear</param>
        void Debug(string message);

        /// <summary>
        /// Log de nivel Info
        /// Usado para información general del funcionamiento del sistema
        /// </summary>
        /// <param name="message">Mensaje a loggear</param>
        void Info(string message);

        /// <summary>
        /// Log de nivel Warning
        /// Usado para situaciones anómalas pero no críticas
        /// </summary>
        /// <param name="message">Mensaje a loggear</param>
        void Warning(string message);

        /// <summary>
        /// Log de nivel Error
        /// Usado para errores que deben ser investigados
        /// </summary>
        /// <param name="message">Mensaje a loggear</param>
        void Error(string message);

        /// <summary>
        /// Log de excepción con stack trace
        /// </summary>
        /// <param name="message">Mensaje descriptivo</param>
        /// <param name="exception">Excepción capturada</param>
        void Exception(string message, Exception exception);

        /// <summary>
        /// Nivel mínimo de logging activo
        /// Mensajes con nivel inferior a este serán ignorados
        /// </summary>
        LogLevel MinLevel { get; set; }
    }

    /// <summary>
    /// Logger simple que imprime a consola (para testing)
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        public LogLevel MinLevel { get; set; } = LogLevel.Info;

        public void Debug(string message)
        {
            if (MinLevel <= LogLevel.Debug)
                Console.WriteLine($"[DEBUG] {DateTime.Now:HH:mm:ss.fff} - {message}");
        }

        public void Info(string message)
        {
            if (MinLevel <= LogLevel.Info)
                Console.WriteLine($"[INFO]  {DateTime.Now:HH:mm:ss.fff} - {message}");
        }

        public void Warning(string message)
        {
            if (MinLevel <= LogLevel.Warning)
                Console.WriteLine($"[WARN]  {DateTime.Now:HH:mm:ss.fff} - {message}");
        }

        public void Error(string message)
        {
            if (MinLevel <= LogLevel.Error)
                Console.WriteLine($"[ERROR] {DateTime.Now:HH:mm:ss.fff} - {message}");
        }

        public void Exception(string message, Exception exception)
        {
            if (MinLevel <= LogLevel.Error)
            {
                Console.WriteLine($"[EXCEPTION] {DateTime.Now:HH:mm:ss.fff} - {message}");
                Console.WriteLine($"  {exception.GetType().Name}: {exception.Message}");
                Console.WriteLine($"  Stack: {exception.StackTrace}");
            }
        }
    }

    /// <summary>
    /// Logger para tests que usa Action<string> para output
    /// Compatible con NinjaTrader Print()
    /// </summary>
    public class TestLogger : ILogger
    {
        private readonly Action<string> _printAction;
        public LogLevel MinLevel { get; set; } = LogLevel.Info;

        public TestLogger(Action<string> printAction)
        {
            _printAction = printAction ?? throw new ArgumentNullException(nameof(printAction));
        }

        public void Debug(string message)
        {
            if (MinLevel <= LogLevel.Debug)
                _printAction($"  [DEBUG] {message}");
        }

        public void Info(string message)
        {
            if (MinLevel <= LogLevel.Info)
                _printAction($"  [INFO]  {message}");
        }

        public void Warning(string message)
        {
            if (MinLevel <= LogLevel.Warning)
                _printAction($"  [WARN]  {message}");
        }

        public void Error(string message)
        {
            if (MinLevel <= LogLevel.Error)
                _printAction($"  [ERROR] {message}");
        }

        public void Exception(string message, Exception exception)
        {
            if (MinLevel <= LogLevel.Error)
            {
                _printAction($"  [EXCEPTION] {message}");
                _printAction($"    {exception.GetType().Name}: {exception.Message}");
            }
        }
    }
}

