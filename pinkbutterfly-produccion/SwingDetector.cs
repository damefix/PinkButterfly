// ============================================================================
// SwingDetector.cs
// PinkButterfly CoreBrain - Detector de Swing Highs y Swing Lows
// 
// Detecta, valida y trackea Swings (pivots) según reglas SMC:
// - Detección de Swing Highs/Lows con validación nLeft/nRight
// - Validación por tamaño (ATR)
// - Tracking de rupturas (IsBroken)
// - Metadata completa (LeftN, RightN, SwingSizeTicks)
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Detector de Swing Highs y Swing Lows
    /// 
    /// REGLAS DE DETECCIÓN:
    /// - Swing High: High[i] > High[i-k] para k=1..nLeft Y High[i] > High[i+k] para k=1..nRight
    /// - Swing Low: Low[i] < Low[i-k] para k=1..nLeft Y Low[i] < Low[i+k] para k=1..nRight
    /// - Validación: swingRange >= MinSwingATRfactor * ATR(tf,14)
    /// - Ruptura: precio cierra más allá del swing con confirmación
    /// </summary>
    public class SwingDetector : IDetector
    {
        private IBarDataProvider _provider;
        private EngineConfig _config;
        private ILogger _logger;
        private CoreEngine _engine;

        // Cache de swings por TF para tracking de rupturas
        private Dictionary<int, List<SwingInfo>> _swingCacheByTF = new Dictionary<int, List<SwingInfo>>();

        public void Initialize(IBarDataProvider provider, EngineConfig config, ILogger logger)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _logger.Info("SwingDetector: Inicializado");
        }

        public void OnBarClose(int tfMinutes, int barIndex, CoreEngine engine)
        {
            _engine = engine;

            // Necesitamos al menos nLeft barras antes y nRight barras después
            // Como estamos en OnBarClose, solo podemos detectar swings que ya tienen nRight barras confirmadas
            int nLeft = _config.nLeft;
            int nRight = _config.nRight;

            // Necesitamos al menos nLeft barras antes del índice a evaluar
            if (barIndex < nLeft)
                return;

            try
            {
                // El swing se detecta en la barra (barIndex - nRight) porque necesitamos nRight barras después
                int swingCandidateIndex = barIndex - nRight;
                
                if (swingCandidateIndex < nLeft)
                    return;

                DetectSwings(tfMinutes, swingCandidateIndex, nLeft, nRight);
                UpdateExistingSwings(tfMinutes, barIndex);

                // Muestreo diagnóstico de conteo de swings activos
                if (barIndex % 50 == 0)
                {
                    try
                    {
                        int total = 0, nHigh = 0, nLow = 0;
                        if (_swingCacheByTF.ContainsKey(tfMinutes))
                        {
                            var list = _swingCacheByTF[tfMinutes].Where(s => s.IsActive).ToList();
                            total = list.Count;
                            nHigh = list.Count(s => s.IsHigh);
                            nLow = total - nHigh;
                        }
                        _logger.Info(string.Format("[DIAG][SwingDetector] TF={0} Bar={1} ActiveSwings={2} (High={3} Low={4})",
                            tfMinutes, barIndex, total, nHigh, nLow));
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                _logger.Exception($"SwingDetector: Error en TF{tfMinutes} bar{barIndex}", ex);
            }
        }

        /// <summary>
        /// Detecta nuevos swings en el índice especificado
        /// </summary>
        private void DetectSwings(int tfMinutes, int swingIndex, int nLeft, int nRight)
        {
            // Detectar Swing High
            if (IsSwingHigh(tfMinutes, swingIndex, nLeft, nRight))
            {
                CreateSwing(tfMinutes, swingIndex, true, nLeft, nRight);
            }

            // Detectar Swing Low
            if (IsSwingLow(tfMinutes, swingIndex, nLeft, nRight))
            {
                CreateSwing(tfMinutes, swingIndex, false, nLeft, nRight);
            }
        }

        /// <summary>
        /// Valida si el índice dado es un Swing High
        /// Regla: High[i] debe ser estrictamente mayor que todos los highs en nLeft y nRight
        /// </summary>
        private bool IsSwingHigh(int tfMinutes, int swingIndex, int nLeft, int nRight)
        {
            double centerHigh = _provider.GetHigh(tfMinutes, swingIndex);

            // Validar barras a la izquierda (nLeft)
            // Center debe ser estrictamente mayor que todos los left
            for (int k = 1; k <= nLeft; k++)
            {
                double leftHigh = _provider.GetHigh(tfMinutes, swingIndex - k);
                if (leftHigh >= centerHigh)
                    return false;
            }

            // Validar barras a la derecha (nRight)
            // Center debe ser estrictamente mayor que todos los right
            for (int k = 1; k <= nRight; k++)
            {
                double rightHigh = _provider.GetHigh(tfMinutes, swingIndex + k);
                if (rightHigh >= centerHigh) // Cambiado a >= para consistencia
                    return false;
            }

            // Validar tamaño del swing (rango desde high hasta el low más bajo en la ventana)
            double minLow = double.MaxValue;
            for (int k = -nLeft; k <= nRight; k++)
            {
                double low = _provider.GetLow(tfMinutes, swingIndex + k);
                if (low < minLow)
                    minLow = low;
            }

            double swingRange = centerHigh - minLow;
            double atr = _provider.GetATR(tfMinutes, 14, swingIndex);
            double minSwingSize = _config.MinSwingATRfactor * atr;

            if (swingRange < minSwingSize)
            {
                if (_config.EnableDebug)
                    _logger.Debug($"SwingDetector: Swing High rechazado por tamaño insuficiente. Range={swingRange:F5}, MinSize={minSwingSize:F5}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Valida si el índice dado es un Swing Low
        /// Regla: Low[i] debe ser estrictamente menor que todos los lows en nLeft y nRight
        /// </summary>
        private bool IsSwingLow(int tfMinutes, int swingIndex, int nLeft, int nRight)
        {
            double centerLow = _provider.GetLow(tfMinutes, swingIndex);

            // Validar barras a la izquierda (nLeft)
            // Center debe ser estrictamente menor que todos los left
            for (int k = 1; k <= nLeft; k++)
            {
                double leftLow = _provider.GetLow(tfMinutes, swingIndex - k);
                if (leftLow <= centerLow)
                    return false;
            }

            // Validar barras a la derecha (nRight)
            // Center debe ser estrictamente menor que todos los right
            for (int k = 1; k <= nRight; k++)
            {
                double rightLow = _provider.GetLow(tfMinutes, swingIndex + k);
                if (rightLow <= centerLow) // Cambiado a <= para consistencia
                    return false;
            }

            // Validar tamaño del swing (rango desde low hasta el high más alto en la ventana)
            double maxHigh = double.MinValue;
            for (int k = -nLeft; k <= nRight; k++)
            {
                double high = _provider.GetHigh(tfMinutes, swingIndex + k);
                if (high > maxHigh)
                    maxHigh = high;
            }

            double swingRange = maxHigh - centerLow;
            double atr = _provider.GetATR(tfMinutes, 14, swingIndex);
            double minSwingSize = _config.MinSwingATRfactor * atr;

            if (swingRange < minSwingSize)
            {
                if (_config.EnableDebug)
                    _logger.Debug($"SwingDetector: Swing Low rechazado por tamaño insuficiente. Range={swingRange:F5}, MinSize={minSwingSize:F5}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Crea un nuevo Swing y lo añade al engine
        /// </summary>
        private void CreateSwing(int tfMinutes, int swingIndex, bool isHigh, int nLeft, int nRight)
        {
            double tickSize = _provider.GetTickSize();
            double swingPrice;
            double oppositePrice;
            double swingRange;

            if (isHigh)
            {
                swingPrice = _provider.GetHigh(tfMinutes, swingIndex);
                
                // Encontrar el low más bajo en la ventana
                double minLow = double.MaxValue;
                for (int k = -nLeft; k <= nRight; k++)
                {
                    double low = _provider.GetLow(tfMinutes, swingIndex + k);
                    if (low < minLow)
                        minLow = low;
                }
                oppositePrice = minLow;
                swingRange = swingPrice - oppositePrice;
            }
            else
            {
                swingPrice = _provider.GetLow(tfMinutes, swingIndex);
                
                // Encontrar el high más alto en la ventana
                double maxHigh = double.MinValue;
                for (int k = -nLeft; k <= nRight; k++)
                {
                    double high = _provider.GetHigh(tfMinutes, swingIndex + k);
                    if (high > maxHigh)
                        maxHigh = high;
                }
                oppositePrice = maxHigh;
                swingRange = oppositePrice - swingPrice;
            }

            // Crear SwingInfo
            var swing = new SwingInfo
            {
                Id = Guid.NewGuid().ToString(),
                Type = "SWING",
                TF = tfMinutes,
                StartTime = _provider.GetBarTime(tfMinutes, swingIndex - nLeft),
                EndTime = _provider.GetBarTime(tfMinutes, swingIndex + nRight),
                High = isHigh ? swingPrice : oppositePrice,
                Low = isHigh ? oppositePrice : swingPrice,
                IsActive = true,
                IsCompleted = true, // Los swings se completan inmediatamente al detectarse
                CreatedAtBarIndex = swingIndex,
                LastUpdatedBarIndex = swingIndex,
                Score = 0.0, // Se calculará por el engine
                TouchCount_Body = 0,
                TouchCount_Wick = 0,
                IsHigh = isHigh,
                LeftN = nLeft,
                RightN = nRight,
                SwingSizeTicks = (int)Math.Round(swingRange / tickSize),
                IsBroken = false
            };

            // Metadata
            double? volume = _provider.GetVolume(tfMinutes, swingIndex);
            swing.Metadata.VolumeAtCreation = volume;
            swing.Metadata.CreatedByDetector = "SwingDetector";

            // Añadir al engine
            _engine.AddStructure(swing);

            // Añadir al cache local
            if (!_swingCacheByTF.ContainsKey(tfMinutes))
                _swingCacheByTF[tfMinutes] = new List<SwingInfo>();
            
            _swingCacheByTF[tfMinutes].Add(swing);

            if (_config.EnableDebug)
                _logger.Debug($"SwingDetector: Created {(isHigh ? "High" : "Low")} Swing TF{tfMinutes} @ {swingPrice:F5} (size={swing.SwingSizeTicks} ticks)");
        }

        /// <summary>
        /// Actualiza swings existentes: detecta rupturas
        /// </summary>
        private void UpdateExistingSwings(int tfMinutes, int barIndex)
        {
            if (!_swingCacheByTF.ContainsKey(tfMinutes))
                return;

            double currentClose = _provider.GetClose(tfMinutes, barIndex);
            
            var activeSwings = _swingCacheByTF[tfMinutes]
                .Where(s => s.IsActive && !s.IsBroken)
                .ToList();
            var swingsToRemove = new List<SwingInfo>();

            foreach (var swing in activeSwings)
            {
                bool broken = false;

                if (swing.IsHigh)
                {
                    // Swing High se rompe cuando el precio cierra por encima
                    if (currentClose > swing.High)
                    {
                        broken = true;
                    }
                }
                else
                {
                    // Swing Low se rompe cuando el precio cierra por debajo
                    if (currentClose < swing.Low)
                    {
                        broken = true;
                    }
                }

                if (broken)
                {
                    swing.IsBroken = true;
                    swing.LastUpdatedBarIndex = barIndex;
                    
                    // Verificar existencia antes de actualizar
                    if (_engine.GetStructureById(swing.Id) != null)
                    {
                        _engine.UpdateStructure(swing);
                    }
                    else
                    {
                        // Swing fue purgado, marcar para remover del caché local
                        swingsToRemove.Add(swing);
                    }

                    if (_config.EnableDebug)
                        _logger.Debug($"SwingDetector: Swing {(swing.IsHigh ? "High" : "Low")} {swing.Id} BROKEN at bar {barIndex}");
                }
            }
            // Remover purgados del caché local
            if (swingsToRemove.Count > 0)
            {
                foreach (var rem in swingsToRemove)
                {
                    _swingCacheByTF[tfMinutes].Remove(rem);
                }
            }
        }

        public void Dispose()
        {
            _swingCacheByTF.Clear();
        }
    }
}

