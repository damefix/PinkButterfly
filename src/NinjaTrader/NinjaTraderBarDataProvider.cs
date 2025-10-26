// ============================================================================
// NinjaTraderBarDataProvider.cs
// PinkButterfly CoreBrain - Implementación real de IBarDataProvider para NT8
// 
// Adaptador que conecta el CoreEngine con los datos reales de NinjaTrader
// Implementa IBarDataProvider usando las APIs de Bars, Instrument, etc.
//
// Thread-safety: NinjaTrader garantiza acceso thread-safe a Bars
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Implementación de IBarDataProvider que consume datos reales de NinjaTrader
    /// Adaptador entre CoreEngine y NinjaTrader APIs
    /// </summary>
    public class NinjaTraderBarDataProvider : IBarDataProvider
    {
        private readonly IndicatorBase _indicator;
        private readonly Dictionary<string, double> _atrCache;
        private readonly object _lock = new object();

        /// <summary>
        /// Constructor del provider
        /// </summary>
        /// <param name="indicator">Instancia del indicador NinjaScript (para acceder a Bars, Instrument, etc.)</param>
        public NinjaTraderBarDataProvider(IndicatorBase indicator)
        {
            _indicator = indicator ?? throw new ArgumentNullException(nameof(indicator));
            _atrCache = new Dictionary<string, double>();
        }

        /// <summary>
        /// NinjaTrader garantiza thread-safety en acceso a Bars
        /// </summary>
        public bool HasInternalLocking => true;
        
        /// <summary>
        /// Indica si el indicador está procesando datos históricos o tiempo real
        /// </summary>
        public bool IsHistorical => _indicator.State == State.Historical;

        // ========================================================================
        // IMPLEMENTACIÓN DE IBarDataProvider
        // ========================================================================

        /// <summary>
        /// Obtiene el tiempo de una barra específica
        /// </summary>
        public DateTime GetBarTime(int tfMinutes, int barIndex)
        {
            try
            {
                // TODO: Implementar mapeo multi-TF cuando se agreguen DataSeries
                // Por ahora, usar el timeframe principal
                if (barIndex < 0 || barIndex >= _indicator.CurrentBar + 1)
                    return DateTime.MinValue;

                return _indicator.Time[_indicator.CurrentBar - barIndex];
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Obtiene el índice de barra desde un tiempo UTC
        /// </summary>
        public int GetBarIndexFromTime(int tfMinutes, DateTime timeUtc)
        {
            try
            {
                // TODO: Implementar búsqueda binaria eficiente
                // Por ahora, búsqueda lineal simple
                for (int i = 0; i <= _indicator.CurrentBar; i++)
                {
                    DateTime barTime = _indicator.Time[_indicator.CurrentBar - i];
                    if (Math.Abs((barTime - timeUtc).TotalMinutes) < tfMinutes / 2.0)
                        return i;
                }
                return -1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Obtiene el índice de la última barra cerrada
        /// </summary>
        public int GetCurrentBarIndex(int tfMinutes)
        {
            return _indicator.CurrentBar;
        }

        /// <summary>
        /// Obtiene el precio de apertura de una barra
        /// </summary>
        public double GetOpen(int tfMinutes, int barIndex)
        {
            try
            {
                if (barIndex < 0 || barIndex > _indicator.CurrentBar)
                    return 0.0;

                int barsAgo = _indicator.CurrentBar - barIndex;
                return _indicator.Open[barsAgo];
            }
            catch
            {
                return 0.0;
            }
        }

        /// <summary>
        /// Obtiene el precio máximo de una barra
        /// </summary>
        public double GetHigh(int tfMinutes, int barIndex)
        {
            try
            {
                if (barIndex < 0 || barIndex > _indicator.CurrentBar)
                    return 0.0;

                int barsAgo = _indicator.CurrentBar - barIndex;
                return _indicator.High[barsAgo];
            }
            catch
            {
                return 0.0;
            }
        }

        /// <summary>
        /// Obtiene el precio mínimo de una barra
        /// </summary>
        public double GetLow(int tfMinutes, int barIndex)
        {
            try
            {
                if (barIndex < 0 || barIndex > _indicator.CurrentBar)
                    return 0.0;

                int barsAgo = _indicator.CurrentBar - barIndex;
                return _indicator.Low[barsAgo];
            }
            catch
            {
                return 0.0;
            }
        }

        /// <summary>
        /// Obtiene el precio de cierre de una barra
        /// </summary>
        public double GetClose(int tfMinutes, int barIndex)
        {
            try
            {
                if (barIndex < 0 || barIndex > _indicator.CurrentBar)
                    return 0.0;

                int barsAgo = _indicator.CurrentBar - barIndex;
                return _indicator.Close[barsAgo];
            }
            catch
            {
                return 0.0;
            }
        }

        /// <summary>
        /// Obtiene el tamaño del tick del instrumento
        /// </summary>
        public double GetTickSize()
        {
            try
            {
                return _indicator.Instrument.MasterInstrument.TickSize;
            }
            catch
            {
                return 0.25; // Default para ES
            }
        }

        /// <summary>
        /// Obtiene el precio medio actual (bid+ask)/2 o (high+low)/2
        /// </summary>
        public double GetMidPrice()
        {
            try
            {
                // Intentar obtener bid/ask si está disponible
                // Si no, usar (high+low)/2 de la barra actual
                return (_indicator.High[0] + _indicator.Low[0]) / 2.0;
            }
            catch
            {
                return _indicator.Close[0];
            }
        }

        /// <summary>
        /// Obtiene el volumen de una barra (nullable si no disponible)
        /// </summary>
        public double? GetVolume(int tfMinutes, int barIndex)
        {
            try
            {
                if (barIndex < 0 || barIndex > _indicator.CurrentBar)
                    return null;

                int barsAgo = _indicator.CurrentBar - barIndex;
                double volume = (double)_indicator.Volume[barsAgo];
                
                return volume > 0 ? volume : (double?)null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Calcula el ATR (Average True Range) para un timeframe y periodo
        /// Usa caché para evitar recalcular en cada llamada
        /// </summary>
        public double GetATR(int tfMinutes, int period, int barIndex)
        {
            try
            {
                string cacheKey = $"{tfMinutes}_{period}_{barIndex}";
                
                lock (_lock)
                {
                    if (_atrCache.ContainsKey(cacheKey))
                        return _atrCache[cacheKey];
                }

                // Calcular ATR manualmente
                double atr = CalculateATR(period, barIndex);

                lock (_lock)
                {
                    _atrCache[cacheKey] = atr;
                    
                    // Limpiar caché si crece demasiado
                    if (_atrCache.Count > 1000)
                        _atrCache.Clear();
                }

                return atr;
            }
            catch
            {
                return 1.0; // Default fallback
            }
        }

        /// <summary>
        /// Obtiene el valor monetario de un punto del instrumento
        /// Ejemplo: ES = $50, NQ = $20, EUR/USD = $100000
        /// </summary>
        public double GetPointValue()
        {
            try
            {
                return _indicator.Instrument.MasterInstrument.PointValue;
            }
            catch
            {
                return 50.0; // Default para ES
            }
        }

        /// <summary>
        /// Obtiene el nombre del instrumento actual
        /// </summary>
        public string GetInstrumentName()
        {
            try
            {
                return _indicator.Instrument.FullName;
            }
            catch
            {
                return "UNKNOWN";
            }
        }

        // ========================================================================
        // MÉTODOS PRIVADOS
        // ========================================================================

        /// <summary>
        /// Calcula el ATR manualmente usando True Range
        /// </summary>
        private double CalculateATR(int period, int barIndex)
        {
            try
            {
                if (barIndex < period)
                    return 1.0; // No hay suficientes barras

                double sum = 0.0;
                int count = 0;

                for (int i = 0; i < period; i++)
                {
                    int currentIndex = barIndex - i;
                    int prevIndex = currentIndex - 1;

                    if (prevIndex < 0)
                        break;

                    double high = GetHigh(0, currentIndex);
                    double low = GetLow(0, currentIndex);
                    double prevClose = GetClose(0, prevIndex);

                    // True Range = max(high-low, abs(high-prevClose), abs(low-prevClose))
                    double tr1 = high - low;
                    double tr2 = Math.Abs(high - prevClose);
                    double tr3 = Math.Abs(low - prevClose);
                    double tr = Math.Max(tr1, Math.Max(tr2, tr3));

                    sum += tr;
                    count++;
                }

                return count > 0 ? sum / count : 1.0;
            }
            catch
            {
                return 1.0;
            }
        }
    }
}

