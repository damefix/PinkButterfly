using System;
using System.IO;
using System.Text;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Logger especializado para operaciones de trading
    /// Escribe un CSV con todas las operaciones para análisis posterior
    /// </summary>
    public class TradeLogger
    {
        private readonly string _csvFilePath;
        private readonly object _lockObject = new object();
        private readonly bool _enableLogging;
        private readonly ILogger _logger;
        private int _tradeCounter = 0;

        /// <summary>
        /// Constructor del TradeLogger
        /// </summary>
        public TradeLogger(string logDirectory, string logPrefix, ILogger logger, bool enableLogging = true)
        {
            _logger = logger;
            _enableLogging = enableLogging;

            if (_enableLogging)
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
                    string fileName = $"{logPrefix}_{timestamp}.csv";
                    _csvFilePath = Path.Combine(logDirectory, fileName);

                    // Escribir header del CSV
                    string header = "TradeID,Timestamp,Action,Direction,Entry,SL,TP,RiskPoints,RewardPoints,RR,Bar,EntryBarTime,StructureID,Status,ExitReason,ExitBar,ExitBarTime,ExitPrice,PnLPoints,PnLDollars";
                    WriteToFile(header);

                    _logger?.Info($"[TradeLogger] CSV creado: {_csvFilePath}");
                }
                catch (Exception ex)
                {
                    _logger?.Error($"[TradeLogger] Error creando CSV: {ex.Message}");
                    _enableLogging = false;
                }
            }
        }

        /// <summary>
        /// Escribe una línea al CSV (thread-safe)
        /// </summary>
        private void WriteToFile(string line)
        {
            if (!_enableLogging) return;

            lock (_lockObject)
            {
                try
                {
                    File.AppendAllText(_csvFilePath, line + Environment.NewLine, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    _logger?.Error($"[TradeLogger] Error escribiendo al CSV: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Registra una orden nueva
        /// </summary>
        public void LogOrderRegistered(string direction, double entry, double sl, double tp, int bar, DateTime barTime, string structureId, double contractSize = 1.0, double pointValue = 5.0)
        {
            if (!_enableLogging) return;

            _tradeCounter++;
            string tradeId = $"T{_tradeCounter:D4}";
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string entryBarTime = barTime.ToString("yyyy-MM-dd HH:mm:ss");

            double riskPoints = Math.Abs(entry - sl);
            double rewardPoints = Math.Abs(tp - entry);
            double rr = riskPoints > 0 ? rewardPoints / riskPoints : 0;

            string line = $"{tradeId},{timestamp},REGISTERED,{direction},{entry:F2},{sl:F2},{tp:F2},{riskPoints:F2},{rewardPoints:F2},{rr:F2},{bar},{entryBarTime},{structureId},PENDING,,-,-,-,-,-";
            WriteToFile(line);
        }

        /// <summary>
        /// Registra una orden ejecutada
        /// </summary>
        public void LogOrderExecuted(string direction, double entry, int bar)
        {
            if (!_enableLogging) return;

            string tradeId = $"T{_tradeCounter:D4}";
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Actualizar el estado (escribir nueva línea con estado actualizado)
            _logger?.Info($"[TradeLogger] Orden ejecutada: {tradeId} {direction} @ {entry:F2} (Bar {bar})");
        }

        /// <summary>
        /// Registra una orden cerrada por TP
        /// </summary>
        public void LogOrderClosedTP(string direction, double entry, double tp, int entryBar, DateTime entryBarTime, int exitBar, DateTime exitBarTime, double contractSize = 1.0, double pointValue = 5.0)
        {
            if (!_enableLogging) return;

            string tradeId = $"T{_tradeCounter:D4}";
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string entryTimeStr = entryBarTime.ToString("yyyy-MM-dd HH:mm:ss");
            string exitTimeStr = exitBarTime.ToString("yyyy-MM-dd HH:mm:ss");

            double pnlPoints = Math.Abs(tp - entry);
            double pnlDollars = pnlPoints * contractSize * pointValue;

            string line = $"{tradeId},{timestamp},CLOSED,{direction},{entry:F2},-,{tp:F2},-,-,-,{entryBar},{entryTimeStr},-,TP_HIT,TP,{exitBar},{exitTimeStr},{tp:F2},{pnlPoints:F2},{pnlDollars:F2}";
            WriteToFile(line);

            _logger?.Info($"[TradeLogger] ✅ TP HIT: {tradeId} | P&L: {pnlPoints:F2} pts / ${pnlDollars:F2}");
        }

        /// <summary>
        /// Registra una orden cerrada por SL
        /// </summary>
        public void LogOrderClosedSL(string direction, double entry, double sl, int entryBar, DateTime entryBarTime, int exitBar, DateTime exitBarTime, double contractSize = 1.0, double pointValue = 5.0)
        {
            if (!_enableLogging) return;

            string tradeId = $"T{_tradeCounter:D4}";
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string entryTimeStr = entryBarTime.ToString("yyyy-MM-dd HH:mm:ss");
            string exitTimeStr = exitBarTime.ToString("yyyy-MM-dd HH:mm:ss");

            double pnlPoints = -Math.Abs(sl - entry);
            double pnlDollars = pnlPoints * contractSize * pointValue;

            string line = $"{tradeId},{timestamp},CLOSED,{direction},{entry:F2},{sl:F2},-,-,-,-,{entryBar},{entryTimeStr},-,SL_HIT,SL,{exitBar},{exitTimeStr},{sl:F2},{pnlPoints:F2},{pnlDollars:F2}";
            WriteToFile(line);

            _logger?.Info($"[TradeLogger] ❌ SL HIT: {tradeId} | P&L: {pnlPoints:F2} pts / ${pnlDollars:F2}");
        }

        /// <summary>
        /// Registra una orden cancelada
        /// </summary>
        public void LogOrderCancelled(string direction, double entry, int bar, DateTime barTime, string reason)
        {
            if (!_enableLogging) return;

            string tradeId = $"T{_tradeCounter:D4}";
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string barTimeStr = barTime.ToString("yyyy-MM-dd HH:mm:ss");

            string line = $"{tradeId},{timestamp},CANCELLED,{direction},{entry:F2},-,-,-,-,-,{bar},{barTimeStr},-,CANCELLED,{reason},-,-,-,-,-";
            WriteToFile(line);
        }

        /// <summary>
        /// Registra una orden expirada
        /// </summary>
        public void LogOrderExpired(string direction, double entry, int bar, DateTime barTime, string reason)
        {
            if (!_enableLogging) return;

            string tradeId = $"T{_tradeCounter:D4}";
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string barTimeStr = barTime.ToString("yyyy-MM-dd HH:mm:ss");

            string line = $"{tradeId},{timestamp},EXPIRED,{direction},{entry:F2},-,-,-,-,-,{bar},{barTimeStr},-,EXPIRED,{reason},-,-,-,-,-";
            WriteToFile(line);
        }

        /// <summary>
        /// Cierra el logger y escribe estadísticas finales
        /// </summary>
        public void Close()
        {
            if (_enableLogging)
            {
                _logger?.Info($"[TradeLogger] Total operaciones registradas: {_tradeCounter}");
                _logger?.Info($"[TradeLogger] CSV guardado: {_csvFilePath}");
            }
        }

        /// <summary>
        /// Obtiene la ruta del archivo CSV
        /// </summary>
        public string GetCsvFilePath()
        {
            return _csvFilePath;
        }
    }
}

