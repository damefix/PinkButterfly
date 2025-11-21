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
        private readonly Dictionary<int, int> _tfToIndex = new Dictionary<int, int>(); // tfMinutes -> BarsArray index

        /// <summary>
        /// Constructor del provider
        /// </summary>
        /// <param name="indicator">Instancia del indicador NinjaScript (para acceder a Bars, Instrument, etc.)</param>
        public NinjaTraderBarDataProvider(IndicatorBase indicator)
        {
            _indicator = indicator ?? throw new ArgumentNullException(nameof(indicator));
            _atrCache = new Dictionary<string, double>();
            BuildTfIndexMap();
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
        /// Obtiene el tiempo de una barra específica (ÍNDICE ABSOLUTO)
        /// </summary>
        public DateTime GetBarTime(int tfMinutes, int barIndex)
        {
            try
            {
                int i = GetSeriesIndexForTF(tfMinutes);
                if (i < 0)
                    return DateTime.MinValue;

                var ba = _indicator.BarsArray[i];
                if (barIndex < 0 || barIndex >= ba.Count)
                    return DateTime.MinValue;

                return ba.GetTime(barIndex);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Obtiene el índice de barra desde un tiempo UTC (ÍNDICE ABSOLUTO)
        /// </summary>
        public int GetBarIndexFromTime(int tfMinutes, DateTime timeUtc)
        {
            try
            {
                int i = GetSeriesIndexForTF(tfMinutes);
                if (i < 0)
                    return -1;

                var ba = _indicator.BarsArray[i];
                int count = ba.Count;
                if (count == 0)
                    return -1;

                int left = 0;
                int right = count - 1;

                // Binary search: último índice donde GetTime(index) <= timeUtc (at-or-before)
                int result = -1;
                while (left <= right)
                {
                    int mid = left + ((right - left) / 2);
                    DateTime t = ba.GetTime(mid);
                    if (t <= timeUtc)
                    {
                        result = mid;      // candidato válido (at-or-before)
                        left = mid + 1;    // buscar si hay uno más reciente que también cumpla
                    }
                    else
                    {
                        right = mid - 1;   // mover hacia índices más antiguos
                    }
                }

                return result;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Obtiene el índice de la última barra cerrada (ÍNDICE ABSOLUTO)
        /// </summary>
        public int GetCurrentBarIndex(int tfMinutes)
        {
            int i = GetSeriesIndexForTF(tfMinutes);
            if (i < 0) return -1;
            
            var ba = _indicator.BarsArray[i];
            return ba.Count > 0 ? ba.Count - 1 : -1;
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
        /// Obtiene el precio de apertura de una barra (ÍNDICE ABSOLUTO)
        /// </summary>
        public double GetOpen(int tfMinutes, int barIndex)
        {
            try
            {
                int i = GetSeriesIndexForTF(tfMinutes);
                if (i < 0) return 0.0;

                var ba = _indicator.BarsArray[i];
                if (barIndex < 0 || barIndex >= ba.Count)
                    return 0.0;

                return ba.GetOpen(barIndex);
            }
            catch
            {
                return 0.0;
            }
        }

        /// <summary>
        /// Obtiene el precio máximo de una barra (ÍNDICE ABSOLUTO)
        /// </summary>
        public double GetHigh(int tfMinutes, int barIndex)
        {
            try
            {
                int i = GetSeriesIndexForTF(tfMinutes);
                if (i < 0) return 0.0;

                var ba = _indicator.BarsArray[i];
                if (barIndex < 0 || barIndex >= ba.Count)
                    return 0.0;

                return ba.GetHigh(barIndex);
            }
            catch
            {
                return 0.0;
            }
        }

        /// <summary>
        /// Obtiene el precio mínimo de una barra (ÍNDICE ABSOLUTO)
        /// </summary>
        public double GetLow(int tfMinutes, int barIndex)
        {
            try
            {
                int i = GetSeriesIndexForTF(tfMinutes);
                if (i < 0) return 0.0;

                var ba = _indicator.BarsArray[i];
                if (barIndex < 0 || barIndex >= ba.Count)
                    return 0.0;

                return ba.GetLow(barIndex);
            }
            catch
            {
                return 0.0;
            }
        }

        /// <summary>
        /// Obtiene el precio de cierre de una barra (ÍNDICE ABSOLUTO)
        /// </summary>
        public double GetClose(int tfMinutes, int barIndex)
        {
            try
            {
                int i = GetSeriesIndexForTF(tfMinutes);
                if (i < 0) return 0.0;

                var ba = _indicator.BarsArray[i];
                if (barIndex < 0 || barIndex >= ba.Count)
                    return 0.0;

                return ba.GetClose(barIndex);
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
                // Usar el TF más bajo disponible para un mid consistente e independiente del gráfico
                if (_tfToIndex.Count == 0)
                    BuildTfIndexMap();
                int minTf = _tfToIndex.Keys.Count > 0 ? _tfToIndex.Keys.Min() : -1;
                int i = minTf >= 0 ? _tfToIndex[minTf] : 0;
                return (_indicator.Highs[i][0] + _indicator.Lows[i][0]) / 2.0;
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
                int i = GetSeriesIndexForTF(tfMinutes);
                if (i < 0) return null;

                var ba = _indicator.BarsArray[i];
                if (barIndex < 0 || barIndex >= ba.Count)
                    return null;

                double volume = (double)ba.GetVolume(barIndex);
                
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

        // ============================================================
        // Helpers de mapeo TF → BarsArray index
        // ============================================================
        private void BuildTfIndexMap()
        {
            try
            {
                _tfToIndex.Clear();
                int n = _indicator.BarsArray != null ? _indicator.BarsArray.Length : 0;
                for (int i = 0; i < n; i++)
                {
                    var bp = _indicator.BarsArray[i].BarsPeriod;
                    int minutes = BarsPeriodToMinutes(bp);
                    if (!_tfToIndex.ContainsKey(minutes))
                        _tfToIndex[minutes] = i;
                }
                // Fallback: asegurar al menos el primario
                if (!_tfToIndex.ContainsKey(_indicator.Bars.BarsPeriod.Value))
                {
                    _tfToIndex[_indicator.Bars.BarsPeriod.Value] = 0;
                }
            }
            catch
            {
                // silencioso; se reconstruirá en el primer acceso
            }
        }

        private int GetSeriesIndexForTF(int tfMinutes)
        {
            if (_tfToIndex.Count == 0)
                BuildTfIndexMap();

            if (_tfToIndex.TryGetValue(tfMinutes, out int idx))
                return idx;

            // Elegir el TF más cercano disponible si no hay coincidencia exacta
            if (_tfToIndex.Count > 0)
            {
                int nearest = -1;
                int bestDiff = int.MaxValue;
                foreach (var kv in _tfToIndex)
                {
                    int diff = Math.Abs(kv.Key - tfMinutes);
                    if (diff < bestDiff)
                    {
                        bestDiff = diff;
                        nearest = kv.Value;
                    }
                }
                return nearest;
            }

            return 0; // Fallback al primario
        }

        private int BarsPeriodToMinutes(BarsPeriod bp)
        {
            switch (bp.BarsPeriodType)
            {
                case BarsPeriodType.Minute:
                    return bp.Value;
                case BarsPeriodType.Day:
                    return bp.Value * 1440;
                case BarsPeriodType.Week:
                    return bp.Value * 10080;
                case BarsPeriodType.Month:
                    return bp.Value * 43200;
                default:
                    // Otros tipos: usar el TF del primario
                    return _indicator.Bars.BarsPeriod.Value;
            }
        }
    }
}
