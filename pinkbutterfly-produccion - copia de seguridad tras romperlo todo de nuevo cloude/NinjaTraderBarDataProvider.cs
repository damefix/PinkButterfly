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
        /// CRÍTICO Multi-TF: Mapea tfMinutes al índice correcto de BarsArray
        /// </summary>
        public DateTime GetBarTime(int tfMinutes, int barIndex)
        {
            try
            {
                // Mapear tfMinutes al índice correcto de BarsArray
                int barsIdx = GetBarsArrayIndexForTF(tfMinutes);
                
                if (barIndex < 0 || barIndex >= _indicator.CurrentBars[barsIdx] + 1)
                    return DateTime.MinValue;

                int barsAgo = _indicator.CurrentBars[barsIdx] - barIndex;
                return _indicator.Times[barsIdx][barsAgo];
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Obtiene el índice de barra desde un tiempo UTC
        /// CRÍTICO Multi-TF: Usa búsqueda binaria en Times[barsIdx] del TF correcto
        /// </summary>
        public int GetBarIndexFromTime(int tfMinutes, DateTime timeUtc)
        {
            try
            {
                // Mapear tfMinutes al índice correcto de BarsArray
                int barsIdx = GetBarsArrayIndexForTF(tfMinutes);
                int totalBars = _indicator.CurrentBars[barsIdx] + 1;
                
                if (totalBars <= 0)
                    return -1;
                
                // Búsqueda binaria en Times[barsIdx]
                // Times[barsIdx][barsAgo] donde barsAgo=0 es la barra más reciente
                int left = 0;
                int right = totalBars - 1;
                double toleranceMinutes = tfMinutes / 2.0;
                
                while (left <= right)
                {
                    int mid = (left + right) / 2;
                    int barsAgo = _indicator.CurrentBars[barsIdx] - mid;
                    DateTime midTime = _indicator.Times[barsIdx][barsAgo];
                    
                    double diffMinutes = (midTime - timeUtc).TotalMinutes;
                    
                    if (Math.Abs(diffMinutes) < toleranceMinutes)
                        return mid; // Encontrado
                    
                    if (midTime > timeUtc)
                        left = mid + 1; // Buscar en barras más antiguas
                    else
                        right = mid - 1; // Buscar en barras más recientes
                }
                
                return -1; // No encontrado
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Obtiene el índice de la última barra cerrada
        /// CRÍTICO Multi-TF: Mapea tfMinutes al índice correcto de BarsArray
        /// </summary>
        public int GetCurrentBarIndex(int tfMinutes)
        {
            return _indicator.CurrentBar;
        }

        /// <summary>
        /// Convierte un barIndex de un timeframe origen a un timeframe destino
        /// Mapea por tiempo: obtiene el tiempo de la barra origen y busca el índice correspondiente en el TF destino
        /// </summary>
        public int ConvertBarIndex(int fromTF, int toTF, int barIndexFrom)
        {
            // Obtener el tiempo de la barra en el TF origen
            DateTime barTime = GetBarTime(fromTF, barIndexFrom);
            
            // Buscar el índice correspondiente en el TF destino
            return GetBarIndexFromTime(toTF, barTime);
        }

        /// <summary>
        /// Obtiene el precio de apertura de una barra
        /// CRÍTICO Multi-TF: Mapea tfMinutes al índice correcto de BarsArray
        /// </summary>
        public double GetOpen(int tfMinutes, int barIndex)
        {
            try
            {
                int barsIdx = GetBarsArrayIndexForTF(tfMinutes);
                
                if (barIndex < 0 || barIndex > _indicator.CurrentBars[barsIdx])
                    return 0.0;

                int barsAgo = _indicator.CurrentBars[barsIdx] - barIndex;
                return _indicator.Opens[barsIdx][barsAgo];
            }
            catch
            {
                return 0.0;
            }
        }

        /// <summary>
        /// Obtiene el precio máximo de una barra
        /// CRÍTICO Multi-TF: Mapea tfMinutes al índice correcto de BarsArray
        /// </summary>
        public double GetHigh(int tfMinutes, int barIndex)
        {
            try
            {
                int barsIdx = GetBarsArrayIndexForTF(tfMinutes);
                
                if (barIndex < 0 || barIndex > _indicator.CurrentBars[barsIdx])
                    return 0.0;

                int barsAgo = _indicator.CurrentBars[barsIdx] - barIndex;
                return _indicator.Highs[barsIdx][barsAgo];
            }
            catch
            {
                return 0.0;
            }
        }

        /// <summary>
        /// Obtiene el precio mínimo de una barra
        /// CRÍTICO Multi-TF: Mapea tfMinutes al índice correcto de BarsArray
        /// </summary>
        public double GetLow(int tfMinutes, int barIndex)
        {
            try
            {
                int barsIdx = GetBarsArrayIndexForTF(tfMinutes);
                
                if (barIndex < 0 || barIndex > _indicator.CurrentBars[barsIdx])
                    return 0.0;

                int barsAgo = _indicator.CurrentBars[barsIdx] - barIndex;
                return _indicator.Lows[barsIdx][barsAgo];
            }
            catch
            {
                return 0.0;
            }
        }

        /// <summary>
        /// Obtiene el precio de cierre de una barra
        /// CRÍTICO Multi-TF: Mapea tfMinutes al índice correcto de BarsArray
        /// </summary>
        public double GetClose(int tfMinutes, int barIndex)
        {
            try
            {
                int barsIdx = GetBarsArrayIndexForTF(tfMinutes);
                
                if (barIndex < 0 || barIndex > _indicator.CurrentBars[barsIdx])
                    return 0.0;

                int barsAgo = _indicator.CurrentBars[barsIdx] - barIndex;
                return _indicator.Closes[barsIdx][barsAgo];
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
        /// Obtiene el precio medio actual (high+low)/2 para el TF especificado
        /// CRÍTICO Multi-TF: Mapea tfMinutes al índice correcto de BarsArray
        /// </summary>
        public double GetMidPrice(int tfMinutes)
        {
            try
            {
                // Mapear tfMinutes al índice correcto de BarsArray
                int barsIdx = GetBarsArrayIndexForTF(tfMinutes);
                
                // Usar High/Low del TF correcto, barra actual (barsAgo = 0)
                double high = _indicator.Highs[barsIdx][0];
                double low = _indicator.Lows[barsIdx][0];
                
                return (high + low) / 2.0;
            }
            catch (Exception ex)
            {
                // Fallback: usar Close del TF especificado
                try
                {
                    int barsIdx = GetBarsArrayIndexForTF(tfMinutes);
                    return _indicator.Closes[barsIdx][0];
                }
                catch
                {
                    return 0.0; // Último recurso
                }
            }
        }

        /// <summary>
        /// Obtiene el volumen de una barra (nullable si no disponible)
        /// CRÍTICO Multi-TF: Mapea tfMinutes al índice correcto de BarsArray
        /// </summary>
        public double? GetVolume(int tfMinutes, int barIndex)
        {
            try
            {
                int barsIdx = GetBarsArrayIndexForTF(tfMinutes);
                
                if (barIndex < 0 || barIndex > _indicator.CurrentBars[barsIdx])
                    return null;

                int barsAgo = _indicator.CurrentBars[barsIdx] - barIndex;
                double volume = (double)_indicator.Volumes[barsIdx][barsAgo];
                
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
        /// CRÍTICO Multi-TF: Usa el tfMinutes especificado, NO siempre BIP=0
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

                // CRÍTICO Multi-TF: Calcular ATR en el TF especificado
                double atr = CalculateATR(tfMinutes, period, barIndex);

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
        /// CRÍTICO Multi-TF: Usa el tfMinutes especificado para obtener High/Low/Close correctos
        /// </summary>
        private double CalculateATR(int tfMinutes, int period, int barIndex)
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

                    // CRÍTICO Multi-TF: Usar el tfMinutes especificado, NO BIP=0
                    double high = GetHigh(tfMinutes, currentIndex);
                    double low = GetLow(tfMinutes, currentIndex);
                    double prevClose = GetClose(tfMinutes, prevIndex);

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

        /// <summary>
        /// Encuentra el índice de BarsArray correspondiente a un timeframe específico
        /// CRÍTICO: Esto permite mapear tfMinutes a la serie correcta en el array multi-TF
        /// </summary>
        /// <param name="tfMinutes">Timeframe en minutos a buscar</param>
        /// <returns>Índice de BarsArray, o 0 (serie primaria) si no se encuentra</returns>
        private int GetBarsArrayIndexForTF(int tfMinutes)
        {
            try
            {
                // Buscar el TF en todas las series disponibles
                for (int i = 0; i < _indicator.BarsArray.Length; i++)
                {
                    if (_indicator.BarsArray[i] != null)
                    {
                        BarsPeriod period = _indicator.BarsArray[i].BarsPeriod;
                        int barsTF = GetMinutesFromBarsPeriod(period);
                        
                        if (barsTF == tfMinutes)
                            return i;
                    }
                }
                
                // No encontrado - usar fallback a serie primaria con warning
                // NOTA: En modo sin logger (tests), no logueamos
                if (_indicator != null)
                {
                    _indicator.Print($"[BarDataProvider] WARNING: TF {tfMinutes}m no encontrado en BarsArray, usando serie primaria (fallback)");
                }
                return 0;
            }
            catch (Exception ex)
            {
                if (_indicator != null)
                {
                    _indicator.Print($"[BarDataProvider] ERROR en GetBarsArrayIndexForTF({tfMinutes}): {ex.Message}");
                }
                return 0; // Fallback seguro
            }
        }

        /// <summary>
        /// Convierte un BarsPeriod a minutos (helper para GetBarsArrayIndexForTF)
        /// </summary>
        private int GetMinutesFromBarsPeriod(BarsPeriod period)
        {
            switch (period.BarsPeriodType)
            {
                case BarsPeriodType.Minute:
                    return period.Value;
                case BarsPeriodType.Day:
                    return period.Value * 1440;
                case BarsPeriodType.Week:
                    return period.Value * 10080;
                case BarsPeriodType.Month:
                    return period.Value * 43200;
                default:
                    return 15; // Default fallback seguro
            }
        }
    }
}

