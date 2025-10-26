// ============================================================================
// IBarDataProvider.cs
// PinkButterfly CoreBrain - Interface de proveedor de datos
// 
// Interface que abstrae el acceso a datos de barras, precios, volumen e indicadores.
// Esta separación permite:
// - Testing con MockBarDataProvider sin NinjaTrader
// - Migración futura a otros proveedores de datos o servicios externos
// - Independencia total del CoreEngine respecto a NinjaTrader
//
// El wrapper de NinjaTrader implementa esta interface
// ============================================================================

using System;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Interface que debe implementar el proveedor de datos de barras
    /// El CoreEngine depende SOLAMENTE de esta interface, nunca de NinjaTrader directamente
    /// 
    /// CONTRATOS IMPORTANTES:
    /// - Todos los tiempos deben ser UTC
    /// - Si un bar no existe, GetBarIndexFromTime debe devolver -1
    /// - Si un indicador (ATR, etc.) no está disponible, debe devolver 0.0
    /// - El provider debe ser thread-safe si se accede concurrentemente
    /// </summary>
    public interface IBarDataProvider
    {
        // ========================================================================
        // MAPEO Y TIEMPO
        // ========================================================================

        /// <summary>
        /// Obtiene el tiempo (UTC) de una barra específica
        /// </summary>
        /// <param name="tfMinutes">Timeframe en minutos (15, 60, 240, 1440, etc.)</param>
        /// <param name="barIndex">Índice de la barra (0 = barra más antigua)</param>
        /// <returns>DateTime en UTC</returns>
        DateTime GetBarTime(int tfMinutes, int barIndex);

        /// <summary>
        /// Obtiene el índice de barra correspondiente a un tiempo específico
        /// IMPORTANTE: Debe manejar correctamente búsqueda en timeframes diferentes
        /// </summary>
        /// <param name="tfMinutes">Timeframe en minutos</param>
        /// <param name="timeUtc">Tiempo en UTC</param>
        /// <returns>Índice de la barra, o -1 si no existe</returns>
        int GetBarIndexFromTime(int tfMinutes, DateTime timeUtc);

        /// <summary>
        /// Obtiene el índice de la última barra CERRADA en el timeframe
        /// (No incluye la barra actual en formación)
        /// </summary>
        /// <param name="tfMinutes">Timeframe en minutos</param>
        /// <returns>Índice de la última barra cerrada</returns>
        int GetCurrentBarIndex(int tfMinutes);

        // ========================================================================
        // PRECIOS OHLC
        // ========================================================================

        /// <summary>
        /// Obtiene el precio Open de una barra
        /// </summary>
        /// <param name="tfMinutes">Timeframe en minutos</param>
        /// <param name="barIndex">Índice de la barra</param>
        /// <returns>Precio de apertura</returns>
        double GetOpen(int tfMinutes, int barIndex);

        /// <summary>
        /// Obtiene el precio High de una barra
        /// </summary>
        /// <param name="tfMinutes">Timeframe en minutos</param>
        /// <param name="barIndex">Índice de la barra</param>
        /// <returns>Precio máximo</returns>
        double GetHigh(int tfMinutes, int barIndex);

        /// <summary>
        /// Obtiene el precio Low de una barra
        /// </summary>
        /// <param name="tfMinutes">Timeframe en minutos</param>
        /// <param name="barIndex">Índice de la barra</param>
        /// <returns>Precio mínimo</returns>
        double GetLow(int tfMinutes, int barIndex);

        /// <summary>
        /// Obtiene el precio Close de una barra
        /// </summary>
        /// <param name="tfMinutes">Timeframe en minutos</param>
        /// <param name="barIndex">Índice de la barra</param>
        /// <returns>Precio de cierre</returns>
        double GetClose(int tfMinutes, int barIndex);

        // ========================================================================
        // TICK SIZE Y PRECIO ACTUAL
        // ========================================================================

        /// <summary>
        /// Obtiene el tamaño del tick del instrumento
        /// Ejemplo: ES = 0.25, NQ = 0.25, EUR/USD = 0.00001
        /// </summary>
        /// <returns>Tamaño del tick</returns>
        double GetTickSize();

        /// <summary>
        /// Obtiene el valor monetario de un punto del instrumento
        /// Ejemplo: ES = $50, NQ = $20, EUR/USD = $100000
        /// </summary>
        /// <returns>Valor de un punto en la moneda base</returns>
        double GetPointValue();

        /// <summary>
        /// Obtiene el precio medio actual del mercado
        /// Preferiblemente (Bid + Ask) / 2
        /// Si no hay Bid/Ask disponible, usar (High + Low) / 2 de la última barra
        /// </summary>
        /// <returns>Precio medio actual</returns>
        double GetMidPrice();

        // ========================================================================
        // VOLUMEN
        // ========================================================================

        /// <summary>
        /// Obtiene el volumen de una barra
        /// IMPORTANTE: Puede devolver null si el volumen no está disponible
        /// (algunos instrumentos o feeds no proveen volumen)
        /// </summary>
        /// <param name="tfMinutes">Timeframe en minutos</param>
        /// <param name="barIndex">Índice de la barra</param>
        /// <returns>Volumen, o null si no disponible</returns>
        double? GetVolume(int tfMinutes, int barIndex);

        // ========================================================================
        // INDICADORES AUXILIARES
        // ========================================================================

        /// <summary>
        /// Calcula el ATR (Average True Range) para una barra específica
        /// El provider debe mantener instancias de ATR por timeframe
        /// 
        /// IMPORTANTE: El cálculo debe ser eficiente (usar cache si es posible)
        /// </summary>
        /// <param name="tfMinutes">Timeframe en minutos</param>
        /// <param name="period">Período del ATR (típicamente 14)</param>
        /// <param name="barIndex">Índice de la barra</param>
        /// <returns>Valor del ATR, o 0.0 si no disponible</returns>
        double GetATR(int tfMinutes, int period, int barIndex);

        // ========================================================================
        // INFORMACIÓN DEL INSTRUMENTO
        // ========================================================================

        /// <summary>
        /// Obtiene el nombre del instrumento actual
        /// Ejemplo: "ES 03-25", "NQ 03-25", "EURUSD"
        /// </summary>
        /// <returns>Nombre del instrumento</returns>
        string GetInstrumentName();
        
        /// <summary>
        /// Indica si el indicador está procesando datos históricos (State.Historical)
        /// o datos en tiempo real (State.Realtime)
        /// </summary>
        bool IsHistorical { get; }

        // ========================================================================
        // LOCKING CONTRACT (OPCIONAL)
        // ========================================================================

        /// <summary>
        /// Indica si el provider tiene locking interno para thread-safety
        /// Si es false, el CoreEngine usará su propio _stateLock
        /// Si es true, el provider garantiza thread-safety en todos los métodos
        /// </summary>
        bool HasInternalLocking { get; }
    }
}

