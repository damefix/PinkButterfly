// ============================================================================
// MockBarDataProvider.cs
// PinkButterfly CoreBrain - Mock provider para testing
// 
// Implementación de IBarDataProvider que simula datos de barras
// Permite testear el CoreEngine y detectores sin NinjaTrader
//
// Características:
// - Datos sintéticos configurables
// - Soporte multi-timeframe
// - Generación de escenarios específicos (FVG, Swings, etc.)
// - Thread-safe
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Barra sintética para testing
    /// </summary>
    public class MockBar
    {
        public DateTime Time { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }
    }

    /// <summary>
    /// Provider mock que simula datos de barras para testing
    /// </summary>
    public class MockBarDataProvider : IBarDataProvider
    {
        private readonly Dictionary<int, List<MockBar>> _barsByTF;
        private readonly double _tickSize;
        private readonly string _instrumentName;
        private readonly object _lock = new object();

        // Caches para ATR
        private readonly Dictionary<string, double> _atrCache;

        /// <summary>
        /// Constructor del mock provider
        /// </summary>
        /// <param name="tickSize">Tamaño del tick (ej: 0.25 para ES)</param>
        /// <param name="instrumentName">Nombre del instrumento</param>
        public MockBarDataProvider(double tickSize = 0.25, string instrumentName = "MOCK")
        {
            _tickSize = tickSize;
            _instrumentName = instrumentName;
            _barsByTF = new Dictionary<int, List<MockBar>>();
            _atrCache = new Dictionary<string, double>();
        }

        /// <summary>
        /// Thread-safety del provider
        /// </summary>
        public bool HasInternalLocking => true;

        // ========================================================================
        // GESTIÓN DE DATOS DE PRUEBA
        // ========================================================================

        /// <summary>
        /// Agrega una barra sintética a un timeframe
        /// </summary>
        public void AddBar(int tfMinutes, MockBar bar)
        {
            lock (_lock)
            {
                if (!_barsByTF.ContainsKey(tfMinutes))
                    _barsByTF[tfMinutes] = new List<MockBar>();

                _barsByTF[tfMinutes].Add(bar);
            }
        }

        /// <summary>
        /// Agrega múltiples barras sintéticas
        /// </summary>
        public void AddBars(int tfMinutes, IEnumerable<MockBar> bars)
        {
            lock (_lock)
            {
                if (!_barsByTF.ContainsKey(tfMinutes))
                    _barsByTF[tfMinutes] = new List<MockBar>();

                _barsByTF[tfMinutes].AddRange(bars);
            }
        }

        /// <summary>
        /// Limpia todas las barras de un timeframe
        /// </summary>
        public void ClearBars(int tfMinutes)
        {
            lock (_lock)
            {
                if (_barsByTF.ContainsKey(tfMinutes))
                    _barsByTF[tfMinutes].Clear();
            }
        }

        /// <summary>
        /// Limpia todas las barras de todos los timeframes
        /// </summary>
        public void ClearAll()
        {
            lock (_lock)
            {
                _barsByTF.Clear();
                _atrCache.Clear();
            }
        }

        // ========================================================================
        // GENERADORES DE ESCENARIOS
        // ========================================================================

        /// <summary>
        /// Genera un escenario de FVG bullish
        /// Retorna lista de 3 barras que forman un FVG
        /// </summary>
        public static List<MockBar> GenerateBullishFVGScenario(
            DateTime startTime, 
            double basePrice = 5000, 
            double gapSize = 5.0)
        {
            return new List<MockBar>
            {
                // Barra A: Alcista
                new MockBar 
                { 
                    Time = startTime, 
                    Open = basePrice, 
                    High = basePrice + 10, 
                    Low = basePrice - 2, 
                    Close = basePrice + 8,
                    Volume = 10000
                },
                // Barra B: Fuerte alcista (crea el gap)
                new MockBar 
                { 
                    Time = startTime.AddMinutes(1), 
                    Open = basePrice + 8, 
                    High = basePrice + 25, 
                    Low = basePrice + 7, 
                    Close = basePrice + 23,
                    Volume = 15000
                },
                // Barra C: Consolidación (confirma el gap)
                new MockBar 
                { 
                    Time = startTime.AddMinutes(2), 
                    Open = basePrice + 23, 
                    High = basePrice + 24, 
                    Low = basePrice + 8 + gapSize,  // Low de C está por encima de High de A
                    Close = basePrice + 22,
                    Volume = 8000
                }
            };
        }

        /// <summary>
        /// Genera un escenario de Swing High
        /// Retorna lista de barras con un swing high en el medio
        /// </summary>
        public static List<MockBar> GenerateSwingHighScenario(
            DateTime startTime,
            double basePrice = 5000,
            int nLeft = 2,
            int nRight = 2,
            double swingHeight = 15.0)
        {
            var bars = new List<MockBar>();
            int totalBars = nLeft + 1 + nRight;

            for (int i = 0; i < totalBars; i++)
            {
                double high, low, close;

                if (i == nLeft) // Barra del swing
                {
                    high = basePrice + swingHeight;
                    low = basePrice + swingHeight - 5;
                    close = basePrice + swingHeight - 2;
                }
                else // Barras alrededor
                {
                    high = basePrice + (swingHeight * 0.6);
                    low = basePrice - 3;
                    close = basePrice + 2;
                }

                bars.Add(new MockBar
                {
                    Time = startTime.AddMinutes(i),
                    Open = basePrice,
                    High = high,
                    Low = low,
                    Close = close,
                    Volume = 10000
                });
            }

            return bars;
        }

        // ========================================================================
        // IMPLEMENTACIÓN DE IBarDataProvider
        // ========================================================================

        public DateTime GetBarTime(int tfMinutes, int barIndex)
        {
            lock (_lock)
            {
                if (!_barsByTF.ContainsKey(tfMinutes) || barIndex < 0 || barIndex >= _barsByTF[tfMinutes].Count)
                    return DateTime.MinValue;

                return _barsByTF[tfMinutes][barIndex].Time;
            }
        }

        public int GetBarIndexFromTime(int tfMinutes, DateTime timeUtc)
        {
            lock (_lock)
            {
                if (!_barsByTF.ContainsKey(tfMinutes))
                    return -1;

                for (int i = 0; i < _barsByTF[tfMinutes].Count; i++)
                {
                    if (_barsByTF[tfMinutes][i].Time == timeUtc)
                        return i;
                }

                return -1;
            }
        }

        public int GetCurrentBarIndex(int tfMinutes)
        {
            lock (_lock)
            {
                if (!_barsByTF.ContainsKey(tfMinutes))
                    return -1;

                return Math.Max(0, _barsByTF[tfMinutes].Count - 1);
            }
        }

        public double GetOpen(int tfMinutes, int barIndex)
        {
            lock (_lock)
            {
                if (!_barsByTF.ContainsKey(tfMinutes) || barIndex < 0 || barIndex >= _barsByTF[tfMinutes].Count)
                    return 0;

                return _barsByTF[tfMinutes][barIndex].Open;
            }
        }

        public double GetHigh(int tfMinutes, int barIndex)
        {
            lock (_lock)
            {
                if (!_barsByTF.ContainsKey(tfMinutes) || barIndex < 0 || barIndex >= _barsByTF[tfMinutes].Count)
                    return 0;

                return _barsByTF[tfMinutes][barIndex].High;
            }
        }

        public double GetLow(int tfMinutes, int barIndex)
        {
            lock (_lock)
            {
                if (!_barsByTF.ContainsKey(tfMinutes) || barIndex < 0 || barIndex >= _barsByTF[tfMinutes].Count)
                    return 0;

                return _barsByTF[tfMinutes][barIndex].Low;
            }
        }

        public double GetClose(int tfMinutes, int barIndex)
        {
            lock (_lock)
            {
                if (!_barsByTF.ContainsKey(tfMinutes) || barIndex < 0 || barIndex >= _barsByTF[tfMinutes].Count)
                    return 0;

                return _barsByTF[tfMinutes][barIndex].Close;
            }
        }

        public double GetTickSize()
        {
            return _tickSize;
        }

        public double GetMidPrice()
        {
            lock (_lock)
            {
                // Retornar mid price de la última barra del primer TF disponible
                if (_barsByTF.Count == 0)
                    return 0;

                var firstTF = _barsByTF.Keys.First();
                var bars = _barsByTF[firstTF];
                
                if (bars.Count == 0)
                    return 0;

                var lastBar = bars[bars.Count - 1];
                return (lastBar.High + lastBar.Low) / 2.0;
            }
        }

        public double? GetVolume(int tfMinutes, int barIndex)
        {
            lock (_lock)
            {
                if (!_barsByTF.ContainsKey(tfMinutes) || barIndex < 0 || barIndex >= _barsByTF[tfMinutes].Count)
                    return null;

                return _barsByTF[tfMinutes][barIndex].Volume;
            }
        }

        public double GetATR(int tfMinutes, int period, int barIndex)
        {
            lock (_lock)
            {
                string cacheKey = $"{tfMinutes}_{period}_{barIndex}";

                if (_atrCache.ContainsKey(cacheKey))
                    return _atrCache[cacheKey];

                if (!_barsByTF.ContainsKey(tfMinutes) || barIndex < period - 1)
                    return 0;

                // Cálculo simplificado de ATR para mock
                // ATR real usa True Range promedio, aquí usamos rango promedio simple
                double sumRange = 0;
                int count = 0;

                for (int i = barIndex - period + 1; i <= barIndex; i++)
                {
                    if (i >= 0 && i < _barsByTF[tfMinutes].Count)
                    {
                        var bar = _barsByTF[tfMinutes][i];
                        sumRange += (bar.High - bar.Low);
                        count++;
                    }
                }

                double atr = count > 0 ? sumRange / count : 0;
                _atrCache[cacheKey] = atr;

                return atr;
            }
        }

        public string GetInstrumentName()
        {
            return _instrumentName;
        }
    }
}

