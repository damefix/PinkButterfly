// ============================================================================
// BOSDetector.cs
// PinkButterfly CoreBrain - Detector de Break of Structure y Change of Character
// 
// Detecta, clasifica y trackea rupturas de estructura del mercado:
// - BOS (Break of Structure): Ruptura que continúa la tendencia
// - CHoCH (Change of Character): Ruptura que indica reversión de tendencia
// - Clasificación de momentum (Strong/Weak) basado en tamaño de vela y volumen
// - Actualización automática del CurrentMarketBias del motor
// 
// IMPORTANTE: Este detector depende de SwingDetector
// Los breaks se detectan cuando el precio cierra más allá de un swing previo
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Detector de Break of Structure (BOS) y Change of Character (CHoCH)
    /// 
    /// REGLAS DE DETECCIÓN:
    /// 1. Obtener swings activos del timeframe
    /// 2. Para cada swing, verificar si el precio ha cerrado más allá:
    ///    - Swing High roto: Close > High del swing (bullish break)
    ///    - Swing Low roto: Close < Low del swing (bearish break)
    /// 3. Confirmar ruptura durante nConfirmBars barras consecutivas
    /// 4. Clasificar como BOS o CHoCH según CurrentMarketBias:
    ///    - BOS: Ruptura en dirección del bias actual (continúa tendencia)
    ///    - CHoCH: Ruptura contra el bias actual (cambio de carácter)
    /// 5. Determinar momentum (Strong/Weak):
    ///    - Strong: bodySize >= BreakMomentumBodyFactor * ATR
    ///    - Weak: bodySize < BreakMomentumBodyFactor * ATR
    /// 6. Actualizar CurrentMarketBias del motor
    /// </summary>
    public class BOSDetector : IDetector
    {
        private IBarDataProvider _provider;
        private EngineConfig _config;
        private ILogger _logger;
        private CoreEngine _engine;

        // Cache de swings por TF para evitar procesar el mismo swing múltiples veces
        private Dictionary<int, HashSet<string>> _processedSwingsByTF = new Dictionary<int, HashSet<string>>();

        // Cache de breaks por TF para tracking
        private Dictionary<int, List<StructureBreakInfo>> _breakCacheByTF = new Dictionary<int, List<StructureBreakInfo>>();

        public void Initialize(IBarDataProvider provider, EngineConfig config, ILogger logger)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _logger.Info("BOSDetector: Inicializado");
        }

        public void OnBarClose(int tfMinutes, int barIndex, CoreEngine engine)
        {
            _engine = engine;

            // Inicializar cache si no existe
            if (!_processedSwingsByTF.ContainsKey(tfMinutes))
                _processedSwingsByTF[tfMinutes] = new HashSet<string>();

            if (!_breakCacheByTF.ContainsKey(tfMinutes))
                _breakCacheByTF[tfMinutes] = new List<StructureBreakInfo>();

            // Necesitamos al menos algunas barras para calcular ATR
            if (barIndex < 14)
                return;

            try
            {
                // Detectar nuevos breaks
                DetectStructureBreaks(tfMinutes, barIndex);
            }
            catch (Exception ex)
            {
                _logger.Exception($"BOSDetector: Error en TF{tfMinutes} bar{barIndex}", ex);
            }
        }

        /// <summary>
        /// Detecta rupturas de estructura (BOS/CHoCH)
        /// </summary>
        private void DetectStructureBreaks(int tfMinutes, int barIndex)
        {
            // Obtener swings activos del timeframe
            var swings = _engine.GetRecentSwings(tfMinutes, maxCount: 100);

            if (swings.Count == 0)
                return;

            // Obtener datos de la barra actual
            double currentClose = _provider.GetClose(tfMinutes, barIndex);
            double currentOpen = _provider.GetOpen(tfMinutes, barIndex);
            double currentHigh = _provider.GetHigh(tfMinutes, barIndex);
            double currentLow = _provider.GetLow(tfMinutes, barIndex);
            DateTime currentTime = _provider.GetBarTime(tfMinutes, barIndex);

            // Calcular ATR para determinar momentum
            double atr = _provider.GetATR(tfMinutes, 14, barIndex);
            if (atr <= 0)
                return;

            // Obtener bias actual del mercado
            string currentBias = _engine.CurrentMarketBias;

            // Procesar cada swing para detectar rupturas
            foreach (var swing in swings)
            {
                // Ignorar swings ya procesados (solo procesamos cada swing una vez)
                if (_processedSwingsByTF[tfMinutes].Contains(swing.Id))
                    continue;

                // Verificar ruptura
                bool isBreak = false;
                string breakDirection = "";

                if (swing.IsHigh)
                {
                    // Swing High roto: precio cierra por encima
                    if (currentClose > swing.High)
                    {
                        isBreak = true;
                        breakDirection = "Bullish";
                    }
                }
                else
                {
                    // Swing Low roto: precio cierra por debajo
                    if (currentClose < swing.Low)
                    {
                        isBreak = true;
                        breakDirection = "Bearish";
                    }
                }

                if (!isBreak)
                    continue;

                // Confirmar ruptura (nConfirmBars)
                if (!ConfirmBreak(tfMinutes, barIndex, swing, breakDirection))
                    continue;

                // Determinar si es BOS o CHoCH
                string breakType = DetermineBreakType(breakDirection, currentBias);

                // Calcular momentum
                double bodySize = Math.Abs(currentClose - currentOpen);
                string breakMomentum = bodySize >= (_config.BreakMomentumBodyFactor * atr) ? "Strong" : "Weak";

                // Crear StructureBreakInfo
                var breakInfo = new StructureBreakInfo
                {
                    BreakType = breakType,
                    BrokenSwingId = swing.Id,
                    BreakPrice = currentClose,
                    Direction = breakDirection,
                    BreakMomentum = breakMomentum,
                    TF = tfMinutes,
                    StartTime = currentTime,
                    EndTime = currentTime,
                    High = swing.IsHigh ? currentHigh : swing.High,
                    Low = swing.IsHigh ? swing.Low : currentLow,
                    CreatedAtBarIndex = barIndex,
                    LastUpdatedBarIndex = barIndex,
                    IsActive = true,
                    IsCompleted = true
                };

                // Metadata
                breakInfo.Metadata.CreatedByDetector = "BOSDetector";
                breakInfo.Metadata.VolumeAtCreation = _provider.GetVolume(tfMinutes, barIndex);
                breakInfo.Metadata.Tags["SwingId"] = swing.Id;
                breakInfo.Metadata.Tags["SwingType"] = swing.IsHigh ? "High" : "Low";

                // Agregar al motor
                _engine.AddStructure(breakInfo);

                // Agregar a cache
                _breakCacheByTF[tfMinutes].Add(breakInfo);

                // Marcar swing como procesado
                _processedSwingsByTF[tfMinutes].Add(swing.Id);

                // Actualizar CurrentMarketBias
                _engine.UpdateCurrentMarketBias(tfMinutes);

                if (_config.EnableDebug)
                {
                    _logger.Debug($"BOSDetector: {breakType} {breakDirection} detectado en TF{tfMinutes} " +
                                 $"bar{barIndex} - Swing {(swing.IsHigh ? "High" : "Low")} @ {(swing.IsHigh ? swing.High : swing.Low):F2} " +
                                 $"roto por close @ {currentClose:F2} - Momentum: {breakMomentum}");
                }
            }
        }

        /// <summary>
        /// Confirma la ruptura verificando que se mantiene durante nConfirmBars barras
        /// </summary>
        private bool ConfirmBreak(int tfMinutes, int barIndex, SwingInfo swing, string breakDirection)
        {
            int nConfirm = _config.nConfirmBars_BOS;

            // Si nConfirm = 1, la ruptura se confirma inmediatamente
            if (nConfirm <= 1)
                return true;

            // Verificar barras anteriores
            for (int i = 1; i < nConfirm; i++)
            {
                int checkIndex = barIndex - i;
                if (checkIndex < 0)
                    return false;

                double checkClose = _provider.GetClose(tfMinutes, checkIndex);

                if (breakDirection == "Bullish")
                {
                    // Para break bullish, todas las barras deben cerrar por encima del swing high
                    if (checkClose <= swing.High)
                        return false;
                }
                else // Bearish
                {
                    // Para break bearish, todas las barras deben cerrar por debajo del swing low
                    if (checkClose >= swing.Low)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determina si la ruptura es BOS (continúa tendencia) o CHoCH (reversión)
        /// </summary>
        /// <param name="breakDirection">Dirección de la ruptura: "Bullish" o "Bearish"</param>
        /// <param name="currentBias">Bias actual del mercado: "Bullish", "Bearish", "Neutral"</param>
        /// <returns>"BOS" si continúa tendencia, "CHoCH" si es reversión</returns>
        private string DetermineBreakType(string breakDirection, string currentBias)
        {
            // Si no hay bias definido, asumimos que es BOS (inicio de tendencia)
            if (currentBias == "Neutral")
                return "BOS";

            // BOS: Ruptura en la misma dirección del bias
            // CHoCH: Ruptura en dirección contraria al bias
            if (breakDirection == currentBias)
                return "BOS";
            else
                return "CHoCH";
        }

        public void Dispose()
        {
            _processedSwingsByTF?.Clear();
            _breakCacheByTF?.Clear();
            _logger?.Info("BOSDetector: Disposed");
        }
    }
}
