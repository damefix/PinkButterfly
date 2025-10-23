// ============================================================================
// LiquidityGrabDetector.cs
// PinkButterfly CoreBrain - Detector de Liquidity Grabs (Stop Hunts)
// 
// Detecta sweeps de liquidez (stop hunts) y confirma reversiones:
// - Detección de sweeps de swings (High/Low)
// - Validación por tamaño de vela (cuerpo y rango vs ATR)
// - Confirmación de reversión (cierre opuesto + no re-ruptura)
// - Tracking de grabs fallidos (TrueBreak)
// - Scoring basado en fuerza del sweep, volumen, reversión y bias
// - Purga rápida de grabs antiguos (relevancia efímera)
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Detector de Liquidity Grabs (LG) - Stop Hunts
    /// 
    /// REGLAS DE DETECCIÓN:
    /// - Identifica sweeps de swings previos (High > SwingHigh o Low < SwingLow)
    /// - Buy-Side Grab: High > SwingHigh && Close < SwingHigh (reversión inmediata)
    /// - Sell-Side Grab: Low < SwingLow && Close > SwingLow (reversión inmediata)
    /// - Validación: cuerpo >= LG_BodyThreshold * ATR
    /// - Validación: rango >= LG_RangeThreshold * ATR
    /// - Confirmación: precio no vuelve a romper GrabPrice en LG_MaxBarsForReversal barras
    /// - Purga: grabs antiguos (> LG_MaxAgeBars) se invalidan automáticamente
    /// </summary>
    public class LiquidityGrabDetector : IDetector
    {
        private IBarDataProvider _provider;
        private EngineConfig _config;
        private ILogger _logger;
        private CoreEngine _engine;

        // Cache de grabs por TF para tracking de confirmación
        private Dictionary<int, List<LiquidityGrabInfo>> _grabCacheByTF = new Dictionary<int, List<LiquidityGrabInfo>>();

        // Cache de swings procesados para evitar múltiples grabs del mismo swing
        private Dictionary<int, HashSet<string>> _processedSwingsByTF = new Dictionary<int, HashSet<string>>();

        public void Initialize(IBarDataProvider provider, EngineConfig config, ILogger logger)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _logger.Info("LiquidityGrabDetector: Inicializado");
        }

        public void OnBarClose(int tfMinutes, int barIndex, CoreEngine engine)
        {
            _engine = engine;

            // Necesitamos al menos 1 barra cerrada
            if (barIndex < 1)
                return;

            try
            {
                DetectGrabs(tfMinutes, barIndex);
                UpdateExistingGrabs(tfMinutes, barIndex);
                PurgeOldGrabs(tfMinutes, barIndex);
            }
            catch (Exception ex)
            {
                _logger.Exception($"LiquidityGrabDetector: Error en TF{tfMinutes} bar{barIndex}", ex);
            }
        }

        /// <summary>
        /// Detecta nuevos Liquidity Grabs en el cierre de la barra actual
        /// </summary>
        private void DetectGrabs(int tfMinutes, int barIndex)
        {
            // Obtener precios de la barra actual
            double currentOpen = _provider.GetOpen(tfMinutes, barIndex);
            double currentHigh = _provider.GetHigh(tfMinutes, barIndex);
            double currentLow = _provider.GetLow(tfMinutes, barIndex);
            double currentClose = _provider.GetClose(tfMinutes, barIndex);

            // Calcular ATR para validación
            double atr = _provider.GetATR(tfMinutes, 14, barIndex);
            if (atr <= 0)
            {
                _logger.Debug($"LiquidityGrabDetector: ATR inválido en TF{tfMinutes} bar{barIndex}");
                return;
            }

            double tickSize = _provider.GetTickSize();
            double bodySize = Math.Abs(currentClose - currentOpen);
            double rangeSize = currentHigh - currentLow;

            // Validar tamaño de vela
            if (bodySize < _config.LG_BodyThreshold * atr)
                return;

            if (rangeSize < _config.LG_RangeThreshold * atr)
                return;

            // Obtener swings activos para buscar sweeps
            var swings = _engine.GetRecentSwings(tfMinutes, maxCount: 50);

            foreach (var swing in swings)
            {
                if (!swing.IsActive || swing.IsBroken)
                    continue;

                // CORRECCIÓN 2: Verificar si ya procesamos este swing
                // Si ya existe un grab activo para este swing, ignorar (no crear duplicado ni invalidar el existente)
                if (IsSwingProcessed(tfMinutes, swing.Id))
                {
                    _logger.Debug($"LiquidityGrabDetector: Swing {swing.Id} ya procesado - ignorando segundo sweep");
                    continue;
                }

                // Detectar Buy-Side Grab (sweep de swing high)
                if (swing.IsHigh && currentHigh > swing.High)
                {
                    // Verificar reversión: cierre debe estar por debajo del swing high
                    if (currentClose < swing.High)
                    {
                        CreateGrab(tfMinutes, barIndex, "BuySide", swing, currentHigh, currentClose, 
                                  bodySize, rangeSize, atr, tickSize);
                        MarkSwingAsProcessed(tfMinutes, swing.Id);
                        break; // Solo un grab por barra
                    }
                }
                // Detectar Sell-Side Grab (sweep de swing low)
                else if (!swing.IsHigh && currentLow < swing.Low)
                {
                    // Verificar reversión: cierre debe estar por encima del swing low
                    if (currentClose > swing.Low)
                    {
                        CreateGrab(tfMinutes, barIndex, "SellSide", swing, currentLow, currentClose, 
                                  bodySize, rangeSize, atr, tickSize);
                        MarkSwingAsProcessed(tfMinutes, swing.Id);
                        break; // Solo un grab por barra
                    }
                }
            }
        }

        /// <summary>
        /// Crea un nuevo Liquidity Grab
        /// </summary>
        private void CreateGrab(int tfMinutes, int barIndex, string directionalBias, 
                                SwingInfo relatedSwing, double grabPrice, double closePrice,
                                double bodySize, double rangeSize, double atr, double tickSize)
        {
            double sweepDistance = directionalBias == "BuySide" 
                ? grabPrice - relatedSwing.High 
                : relatedSwing.Low - grabPrice;

            var grabInfo = new LiquidityGrabInfo
            {
                Id = $"LG_{tfMinutes}_{_provider.GetBarTime(tfMinutes, barIndex):yyyyMMdd_HHmmss}_{directionalBias}",
                TF = tfMinutes,
                DirectionalBias = directionalBias,
                GrabPrice = grabPrice,
                ClosePrice = closePrice,
                Low = directionalBias == "BuySide" ? relatedSwing.High : grabPrice,
                High = directionalBias == "BuySide" ? grabPrice : relatedSwing.Low,
                StartTime = _provider.GetBarTime(tfMinutes, barIndex),
                EndTime = _provider.GetBarTime(tfMinutes, barIndex),
                CreatedAtBarIndex = barIndex,
                LastUpdatedBarIndex = barIndex,
                IsActive = true,
                IsCompleted = false,
                RelatedSwingId = relatedSwing.Id,
                SweepDistanceTicks = sweepDistance / tickSize,
                SweepStrength = CalculateSweepStrength(bodySize, rangeSize, atr),
                VolumeAtGrab = _provider.GetVolume(tfMinutes, barIndex) ?? 0.0,
                VolumeRatio = CalculateVolumeRatio(tfMinutes, barIndex),
                ConfirmedReversal = false, // Se confirmará en barras siguientes
                FailedGrab = false,
                BarsSinceGrab = 0,
                Metadata = new StructureMetadata
                {
                    CreatedByDetector = "LiquidityGrabDetector",
                    VolumeAtCreation = _provider.GetVolume(tfMinutes, barIndex)
                }
            };

            // Calcular score inicial
            grabInfo.Score = CalculateGrabScore(grabInfo, tfMinutes, barIndex);

            // Añadir al engine y cache
            _engine.AddStructure(grabInfo);
            AddToCache(tfMinutes, grabInfo);

            _logger.Debug($"LiquidityGrabDetector: Nuevo {directionalBias} Grab en TF{tfMinutes} @ {grabPrice:F2} Score:{grabInfo.Score:F3}");
        }

        /// <summary>
        /// Calcula la fuerza del sweep basada en tamaño de vela vs ATR
        /// </summary>
        private double CalculateSweepStrength(double bodySize, double rangeSize, double atr)
        {
            // Fuerza basada en el cuerpo y rango relativos al ATR
            double bodyStrength = bodySize / atr;
            double rangeStrength = rangeSize / atr;

            // Promedio ponderado (cuerpo tiene más peso)
            double strength = (bodyStrength * 0.6) + (rangeStrength * 0.4);

            return Math.Min(strength, 1.0); // Clamp a 1.0
        }

        /// <summary>
        /// Calcula el ratio de volumen relativo al promedio
        /// </summary>
        private double CalculateVolumeRatio(int tfMinutes, int barIndex)
        {
            double? vol = _provider.GetVolume(tfMinutes, barIndex);
            if (!vol.HasValue)
                return 0.0;

            double avgVolume = CalculateAverageVolume(tfMinutes, barIndex, _config.LG_VolumeAvgPeriod);
            if (avgVolume <= 0)
                return 0.0;

            return vol.Value / avgVolume;
        }

        /// <summary>
        /// Calcula el volumen promedio de las últimas N barras
        /// </summary>
        private double CalculateAverageVolume(int tfMinutes, int endIndex, int period)
        {
            double sum = 0;
            int count = 0;

            for (int i = 0; i < period; i++)
            {
                int idx = endIndex - i;
                if (idx < 0) break;

                double? vol = _provider.GetVolume(tfMinutes, idx);
                if (vol.HasValue)
                {
                    sum += vol.Value;
                    count++;
                }
            }

            return count > 0 ? sum / count : 0;
        }

        /// <summary>
        /// Calcula el score total del grab
        /// Basado en: fuerza del sweep, volumen, confirmación de reversión, contexto de bias
        /// </summary>
        private double CalculateGrabScore(LiquidityGrabInfo grabInfo, int tfMinutes, int barIndex)
        {
            // Factor de fuerza del sweep
            double sweepScore = grabInfo.SweepStrength;

            // Factor de volumen (volumen alto = mayor score)
            double volumeScore = Math.Min(grabInfo.VolumeRatio / _config.LG_VolumeSpikeFactor, 1.0);

            // CORRECCIÓN 1: Factor de confirmación de reversión (aumentado para compensar decay)
            // Confirmado = 1.0, No confirmado = 0.3 (mayor diferencia para que el score suba al confirmar)
            double reversalScore = grabInfo.ConfirmedReversal ? 1.0 : 0.3;

            // Factor de contexto de bias (alineación con CurrentMarketBias)
            double biasScore = CalculateBiasAlignment(grabInfo, tfMinutes);

            // Score ponderado
            double score = (_config.LG_SweepStrengthWeight * sweepScore) +
                          (_config.LG_VolumeWeight * volumeScore) +
                          (_config.LG_ReversalWeight * reversalScore) +
                          (_config.LG_BiasWeight * biasScore);

            return Math.Max(0.0, Math.Min(1.0, score)); // Clamp entre 0 y 1
        }

        /// <summary>
        /// Calcula la alineación del grab con el bias del mercado
        /// </summary>
        private double CalculateBiasAlignment(LiquidityGrabInfo grabInfo, int tfMinutes)
        {
            string currentBias = _engine.CurrentMarketBias;

            // Si el grab es contrario al bias, tiene mayor score (indica reversión)
            // Buy-Side Grab (sweep de máximos) es bajista → alineado con bias Bearish
            // Sell-Side Grab (sweep de mínimos) es alcista → alineado con bias Bullish

            if (grabInfo.DirectionalBias == "BuySide" && currentBias == "Bearish")
                return 1.0; // Alineado: grab bajista con bias bajista

            if (grabInfo.DirectionalBias == "SellSide" && currentBias == "Bullish")
                return 1.0; // Alineado: grab alcista con bias alcista

            if (currentBias == "Neutral")
                return 0.5; // Neutral

            return 0.3; // Contrario al bias (menor score)
        }

        /// <summary>
        /// Actualiza grabs existentes (confirmación de reversión y tracking)
        /// </summary>
        private void UpdateExistingGrabs(int tfMinutes, int barIndex)
        {
            if (!_grabCacheByTF.ContainsKey(tfMinutes))
                return;

            var grabs = _grabCacheByTF[tfMinutes].ToList();
            double currentHigh = _provider.GetHigh(tfMinutes, barIndex);
            double currentLow = _provider.GetLow(tfMinutes, barIndex);

            foreach (var grabInfo in grabs)
            {
                if (!grabInfo.IsActive || grabInfo.FailedGrab)
                    continue;

                // Incrementar contador de barras desde el grab
                grabInfo.BarsSinceGrab = barIndex - grabInfo.CreatedAtBarIndex;

                // Verificar confirmación de reversión
                if (!grabInfo.ConfirmedReversal && grabInfo.BarsSinceGrab <= _config.LG_MaxBarsForReversal)
                {
                    // Buy-Side Grab: verificar que el precio no volvió a romper el GrabPrice
                    if (grabInfo.DirectionalBias == "BuySide")
                    {
                        if (currentHigh > grabInfo.GrabPrice)
                        {
                            // CORRECCIÓN 2: Solo invalidar si NO es un segundo sweep del mismo swing
                            // Verificar si el swing relacionado sigue activo y no roto
                            bool isSecondSweep = !string.IsNullOrEmpty(grabInfo.RelatedSwingId) && 
                                                 IsSwingProcessed(tfMinutes, grabInfo.RelatedSwingId);
                            
                            if (!isSecondSweep)
                            {
                                // Falló la reversión: el precio continuó hacia arriba (true break)
                                grabInfo.FailedGrab = true;
                                grabInfo.IsActive = false;
                                _logger.Debug($"LiquidityGrabDetector: Grab {grabInfo.Id} falló - TrueBreak");
                            }
                            else
                            {
                                _logger.Debug($"LiquidityGrabDetector: Grab {grabInfo.Id} - ignorando segundo sweep del mismo swing");
                            }
                        }
                        else if (grabInfo.BarsSinceGrab == _config.LG_MaxBarsForReversal)
                        {
                            // CORRECCIÓN 1: Confirmado - aplicar bonificación inmediata
                            grabInfo.ConfirmedReversal = true;
                            // Recalcular score AHORA con la bonificación de confirmación
                            double oldScore = grabInfo.Score;
                            grabInfo.Score = CalculateGrabScore(grabInfo, tfMinutes, barIndex);
                            _logger.Debug($"LiquidityGrabDetector: Grab {grabInfo.Id} confirmado - Score: {oldScore:F3} → {grabInfo.Score:F3}");
                        }
                    }
                    // Sell-Side Grab: verificar que el precio no volvió a romper el GrabPrice
                    else if (grabInfo.DirectionalBias == "SellSide")
                    {
                        if (currentLow < grabInfo.GrabPrice)
                        {
                            // CORRECCIÓN 2: Solo invalidar si NO es un segundo sweep del mismo swing
                            // Verificar si el swing relacionado sigue activo y no roto
                            bool isSecondSweep = !string.IsNullOrEmpty(grabInfo.RelatedSwingId) && 
                                                 IsSwingProcessed(tfMinutes, grabInfo.RelatedSwingId);
                            
                            if (!isSecondSweep)
                            {
                                // Falló la reversión: el precio continuó hacia abajo (true break)
                                grabInfo.FailedGrab = true;
                                grabInfo.IsActive = false;
                                _logger.Debug($"LiquidityGrabDetector: Grab {grabInfo.Id} falló - TrueBreak");
                            }
                            else
                            {
                                _logger.Debug($"LiquidityGrabDetector: Grab {grabInfo.Id} - ignorando segundo sweep del mismo swing");
                            }
                        }
                        else if (grabInfo.BarsSinceGrab == _config.LG_MaxBarsForReversal)
                        {
                            // CORRECCIÓN 1: Confirmado - aplicar bonificación inmediata
                            grabInfo.ConfirmedReversal = true;
                            // Recalcular score AHORA con la bonificación de confirmación
                            double oldScore = grabInfo.Score;
                            grabInfo.Score = CalculateGrabScore(grabInfo, tfMinutes, barIndex);
                            _logger.Debug($"LiquidityGrabDetector: Grab {grabInfo.Id} confirmado - Score: {oldScore:F3} → {grabInfo.Score:F3}");
                        }
                    }
                }
                // CORRECCIÓN 1: Solo recalcular score si NO se acaba de confirmar (evitar doble cálculo)
                else if (grabInfo.ConfirmedReversal)
                {
                    // Para grabs confirmados, NO aplicar decay adicional
                    // El score se mantiene estable después de la confirmación
                }
                else
                {
                    // Para grabs no confirmados, recalcular score normalmente
                    grabInfo.Score = CalculateGrabScore(grabInfo, tfMinutes, barIndex);
                }

                grabInfo.LastUpdatedBarIndex = barIndex;
                // CORRECCIÓN 1: No usar UpdateStructure para LiquidityGrabs porque recalcula el score
                // El score ya fue calculado manualmente en este detector
                // _engine.UpdateStructure(grabInfo);
            }
        }

        /// <summary>
        /// Purga grabs antiguos (relevancia efímera)
        /// </summary>
        private void PurgeOldGrabs(int tfMinutes, int barIndex)
        {
            if (!_grabCacheByTF.ContainsKey(tfMinutes))
                return;

            var grabs = _grabCacheByTF[tfMinutes].ToList();

            foreach (var grabInfo in grabs)
            {
                if (!grabInfo.IsActive)
                    continue;

                // Purgar si es muy antiguo
                if (grabInfo.BarsSinceGrab > _config.LG_MaxAgeBars)
                {
                    grabInfo.IsActive = false;
                    grabInfo.Score = 0.0;
                    _engine.UpdateStructure(grabInfo);
                    _logger.Debug($"LiquidityGrabDetector: Grab {grabInfo.Id} purgado por edad");
                }
            }

            // Limpiar cache de grabs inactivos
            _grabCacheByTF[tfMinutes] = grabs.Where(g => g.IsActive).ToList();
        }

        /// <summary>
        /// Verifica si un swing ya fue procesado (para evitar múltiples grabs del mismo swing)
        /// </summary>
        private bool IsSwingProcessed(int tfMinutes, string swingId)
        {
            if (!_processedSwingsByTF.ContainsKey(tfMinutes))
                return false;

            return _processedSwingsByTF[tfMinutes].Contains(swingId);
        }

        /// <summary>
        /// Marca un swing como procesado
        /// </summary>
        private void MarkSwingAsProcessed(int tfMinutes, string swingId)
        {
            if (!_processedSwingsByTF.ContainsKey(tfMinutes))
                _processedSwingsByTF[tfMinutes] = new HashSet<string>();

            _processedSwingsByTF[tfMinutes].Add(swingId);

            // Limitar tamaño del cache
            const int maxCacheSize = 100;
            if (_processedSwingsByTF[tfMinutes].Count > maxCacheSize)
            {
                // Remover los más antiguos (simplificado: remover la mitad)
                var toRemove = _processedSwingsByTF[tfMinutes].Take(maxCacheSize / 2).ToList();
                foreach (var id in toRemove)
                {
                    _processedSwingsByTF[tfMinutes].Remove(id);
                }
            }
        }

        /// <summary>
        /// Añade un grab al cache local
        /// </summary>
        private void AddToCache(int tfMinutes, LiquidityGrabInfo grabInfo)
        {
            if (!_grabCacheByTF.ContainsKey(tfMinutes))
                _grabCacheByTF[tfMinutes] = new List<LiquidityGrabInfo>();

            _grabCacheByTF[tfMinutes].Add(grabInfo);
        }

        public void Dispose()
        {
            _grabCacheByTF.Clear();
            _processedSwingsByTF.Clear();
            _logger.Info("LiquidityGrabDetector: Disposed");
        }
    }
}

