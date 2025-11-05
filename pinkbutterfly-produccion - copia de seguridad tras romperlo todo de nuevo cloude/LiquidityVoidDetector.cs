// ============================================================================
// LiquidityVoidDetector.cs
// PinkButterfly CoreBrain - Detector de Liquidity Voids
// 
// Detecta zonas de ineficiencia sin liquidez (Liquidity Voids):
// - Detección de gaps entre dos velas sin solapamiento
// - Validación por tamaño (ATR) y volumen
// - Fusión de voids consecutivos en Extended Voids
// - Tracking de toques y fill percentage
// - Scoring basado en tamaño, profundidad (volumen) y proximidad
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Detector de Liquidity Voids (LV)
    /// 
    /// REGLAS DE DETECCIÓN:
    /// - Requiere 2 barras consecutivas: A (i-1), B (i)
    /// - Bullish Void: highA < lowB (gap hacia arriba sin solapamiento)
    /// - Bearish Void: lowA > highB (gap hacia abajo sin solapamiento)
    /// - Validación: size >= LV_MinSizeATRFactor * ATR
    /// - Validación: volume < LV_VolumeThreshold * avgVolume (opcional)
    /// - Fusión: si LV_EnableFusion=true, fusionar voids adyacentes
    /// - Exclusión: NO crear LV si ya existe un FVG en la misma zona
    /// </summary>
    public class LiquidityVoidDetector : IDetector
    {
        private IBarDataProvider _provider;
        private EngineConfig _config;
        private ILogger _logger;
        private CoreEngine _engine;

        // Cache de voids por TF para fusión
        private Dictionary<int, List<LiquidityVoidInfo>> _voidCacheByTF = new Dictionary<int, List<LiquidityVoidInfo>>();

        public void Initialize(IBarDataProvider provider, EngineConfig config, ILogger logger)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _logger.Info("LiquidityVoidDetector: Inicializado");
        }

        public void OnBarClose(int tfMinutes, int barIndex, CoreEngine engine)
        {
            _engine = engine;

            // Necesitamos al menos 2 barras cerradas (índices i-1, i)
            if (barIndex < 1)
                return;

            try
            {
                DetectVoids(tfMinutes, barIndex);
                UpdateExistingVoids(tfMinutes, barIndex);
            }
            catch (Exception ex)
            {
                _logger.Exception($"LiquidityVoidDetector: Error en TF{tfMinutes} bar{barIndex}", ex);
            }
        }

        /// <summary>
        /// Detecta nuevos Liquidity Voids en el cierre de la barra actual
        /// </summary>
        private void DetectVoids(int tfMinutes, int barIndex)
        {
            // Índices de las 2 barras: A=i-1, B=i
            int idxA = barIndex - 1;
            int idxB = barIndex;

            // Obtener precios de las 2 barras
            double highA = _provider.GetHigh(tfMinutes, idxA);
            double lowA = _provider.GetLow(tfMinutes, idxA);
            double highB = _provider.GetHigh(tfMinutes, idxB);
            double lowB = _provider.GetLow(tfMinutes, idxB);

            if (_config.EnableDebug) _logger.Debug($"LV: TF{tfMinutes} bar{barIndex} - A[{idxA}]: H={highA:F2} L={lowA:F2}, B[{idxB}]: H={highB:F2} L={lowB:F2}");

            // Calcular ATR para validación
            double atr = _provider.GetATR(tfMinutes, 14, barIndex);
            if (atr <= 0)
            {
                if (_config.EnableDebug) _logger.Debug($"LV: ATR inválido={atr} en TF{tfMinutes} bar{barIndex}");
                return;
            }

            double tickSize = _provider.GetTickSize();
            double minVoidSize = _config.LV_MinSizeATRFactor * atr;
            if (_config.EnableDebug) _logger.Debug($"LV: ATR={atr:F2}, minVoidSize={minVoidSize:F2} (factor={_config.LV_MinSizeATRFactor})");

            // Detectar Bullish Void: highA < lowB (gap hacia arriba)
            if (highA < lowB)
            {
                double voidSize = lowB - highA;
                if (_config.EnableDebug) _logger.Debug($"LV: Bullish gap detected! voidSize={voidSize:F2}, minRequired={minVoidSize:F2}");
                
                if (voidSize >= minVoidSize)
                {
                    // Validar volumen solo si está habilitado
                    bool volumeOk = !_config.LV_RequireLowVolume || IsLowVolumeZone(tfMinutes, idxA, idxB);
                    if (_config.EnableDebug) _logger.Debug($"LV: Volume check - RequireLowVolume={_config.LV_RequireLowVolume}, volumeOk={volumeOk}");
                    
                    if (volumeOk)
                    {
                        // EXCLUSIÓN JERÁRQUICA: Si existe un FVG que contiene completamente este void, prevalece el FVG
                        bool fvgContainsVoid = ExistsFVGInZone(tfMinutes, highA, lowB);
                        
                        if (!fvgContainsVoid)
                        {
                            _logger.Info($"LV: ✓ Creating Bullish void at [{highA:F2}, {lowB:F2}], size={voidSize:F2}");
                            CreateVoid(tfMinutes, barIndex, "Bullish", highA, lowB, voidSize, atr, tickSize);
                        }
                        else
                        {
                            if (_config.EnableDebug) _logger.Debug($"LV: ✗ Bullish void rejected - FVG structure prevails (hierarchical exclusion)");
                        }
                    }
                    else
                    {
                        if (_config.EnableDebug) _logger.Debug($"LV: ✗ Bullish void rejected - volume too high");
                    }
                }
                else
                {
                    if (_config.EnableDebug) _logger.Debug($"LV: ✗ Bullish gap too small ({voidSize:F2} < {minVoidSize:F2})");
                }
            }
            // Detectar Bearish Void: lowA > highB (gap hacia abajo)
            else if (lowA > highB)
            {
                double voidSize = lowA - highB;
                if (_config.EnableDebug) _logger.Debug($"LV: Bearish gap detected! voidSize={voidSize:F2}, minRequired={minVoidSize:F2}");
                
                if (voidSize >= minVoidSize)
                {
                    // Validar volumen solo si está habilitado
                    bool volumeOk = !_config.LV_RequireLowVolume || IsLowVolumeZone(tfMinutes, idxA, idxB);
                    if (_config.EnableDebug) _logger.Debug($"LV: Volume check - RequireLowVolume={_config.LV_RequireLowVolume}, volumeOk={volumeOk}");
                    
                    if (volumeOk)
                    {
                        // EXCLUSIÓN JERÁRQUICA: Si existe un FVG que contiene completamente este void, prevalece el FVG
                        bool fvgContainsVoid = ExistsFVGInZone(tfMinutes, highB, lowA);
                        
                        if (!fvgContainsVoid)
                        {
                            _logger.Info($"LV: ✓ Creating Bearish void at [{highB:F2}, {lowA:F2}], size={voidSize:F2}");
                            CreateVoid(tfMinutes, barIndex, "Bearish", highB, lowA, voidSize, atr, tickSize);
                        }
                        else
                        {
                            if (_config.EnableDebug) _logger.Debug($"LV: ✗ Bearish void rejected - FVG structure prevails (hierarchical exclusion)");
                        }
                    }
                    else
                    {
                        if (_config.EnableDebug) _logger.Debug($"LV: ✗ Bearish void rejected - volume too high");
                    }
                }
                else
                {
                    if (_config.EnableDebug) _logger.Debug($"LV: ✗ Bearish gap too small ({voidSize:F2} < {minVoidSize:F2})");
                }
            }
        }

        /// <summary>
        /// Verifica si la zona tiene bajo volumen (opcional)
        /// </summary>
        private bool IsLowVolumeZone(int tfMinutes, int idxA, int idxB)
        {
            // Si no hay volumen disponible, asumir que sí es low volume
            double? volA = _provider.GetVolume(tfMinutes, idxA);
            double? volB = _provider.GetVolume(tfMinutes, idxB);

            if (!volA.HasValue || !volB.HasValue)
                return true; // Sin volumen, aceptar el void

            // Calcular volumen promedio
            double avgVolume = CalculateAverageVolume(tfMinutes, idxB, _config.LV_VolumeAvgPeriod);
            if (avgVolume <= 0)
                return true;

            // Verificar si el volumen de ambas barras está por debajo del threshold
            double avgVoidVolume = (volA.Value + volB.Value) / 2.0;
            return avgVoidVolume < (avgVolume * _config.LV_VolumeThreshold);
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
        /// Verifica si existe un FVG ACTIVO Y CONFIRMADO en la zona especificada (regla de exclusión jerárquica)
        /// 
        /// LÓGICA JERÁRQUICA:
        /// - Si el gap de 2 barras es parte de un FVG de 3 barras YA CONFIRMADO, prevalece el FVG
        /// - Un FVG es más fuerte (estructura validada) que un Void (ineficiencia simple)
        /// - Solo excluye si el FVG cubre COMPLETAMENTE la zona del void
        /// </summary>
        private bool ExistsFVGInZone(int tfMinutes, double zoneLow, double zoneHigh)
        {
            var fvgs = _engine.GetActiveFVGs(tfMinutes, minScore: 0.0);
            
            foreach (var fvg in fvgs)
            {
                // Verificar si el FVG cubre COMPLETAMENTE la zona del void
                // (no solo solapamiento parcial, sino que el void está contenido en el FVG)
                bool fvgContainsVoid = (fvg.Low <= zoneLow && fvg.High >= zoneHigh);
                
                if (fvgContainsVoid)
                {
                    if (_config.EnableDebug) _logger.Debug($"LV: FVG [{fvg.Low:F2}, {fvg.High:F2}] contiene completamente void [{zoneLow:F2}, {zoneHigh:F2}]");
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Crea un nuevo Liquidity Void
        /// </summary>
        private void CreateVoid(int tfMinutes, int barIndex, string direction, 
                                double low, double high, double voidSize, double atr, double tickSize)
        {
            var voidInfo = new LiquidityVoidInfo
            {
                Id = $"LV_{tfMinutes}_{_provider.GetBarTime(tfMinutes, barIndex):yyyyMMdd_HHmmss}_{direction}",
                TF = tfMinutes,
                Direction = direction,
                Low = low,
                High = high,
                StartTime = _provider.GetBarTime(tfMinutes, barIndex - 1),
                EndTime = _provider.GetBarTime(tfMinutes, barIndex),
                CreatedAtBarIndex = barIndex,
                LastUpdatedBarIndex = barIndex,
                IsActive = true,
                IsCompleted = false,
                SizeTicks = voidSize / tickSize,
                VolumeRatio = CalculateVolumeRatio(tfMinutes, barIndex),
                InefficiencyScore = CalculateInefficiencyScore(voidSize, atr),
                Metadata = new StructureMetadata
                {
                    CreatedByDetector = "LiquidityVoidDetector",
                    VolumeAtCreation = _provider.GetVolume(tfMinutes, barIndex)
                }
            };

            // Calcular score inicial
            voidInfo.Score = CalculateVoidScore(voidInfo, tfMinutes, barIndex, atr);

            // Intentar fusionar con voids existentes si está habilitado
            if (_config.LV_EnableFusion)
            {
                var mergedVoid = TryMergeWithExistingVoids(voidInfo, tfMinutes, atr);
                if (mergedVoid != null)
                {
                    if (_config.EnableDebug) _logger.Debug($"LiquidityVoidDetector: Void fusionado en TF{tfMinutes} - {mergedVoid.Id}");
                    return; // Ya se fusionó, no crear nuevo
                }
            }

            // Añadir al engine y cache
            _engine.AddStructure(voidInfo);
            AddToCache(tfMinutes, voidInfo);

            if (_config.EnableDebug) _logger.Debug($"LiquidityVoidDetector: Nuevo {direction} Void en TF{tfMinutes} [{low:F2}-{high:F2}] Score:{voidInfo.Score:F3}");
        }

        /// <summary>
        /// Calcula el ratio de volumen relativo al promedio
        /// </summary>
        private double CalculateVolumeRatio(int tfMinutes, int barIndex)
        {
            double? vol = _provider.GetVolume(tfMinutes, barIndex);
            if (!vol.HasValue)
                return 0.0;

            double avgVolume = CalculateAverageVolume(tfMinutes, barIndex, _config.LV_VolumeAvgPeriod);
            if (avgVolume <= 0)
                return 0.0;

            return vol.Value / avgVolume;
        }

        /// <summary>
        /// Calcula el score de ineficiencia basado en tamaño vs ATR
        /// </summary>
        private double CalculateInefficiencyScore(double voidSize, double atr)
        {
            // Score basado en tamaño relativo al ATR
            // Voids más grandes tienen mayor score de ineficiencia
            double sizeRatio = voidSize / atr;
            return Math.Min(sizeRatio, 1.0); // Clamp a 1.0
        }

        /// <summary>
        /// Calcula el score total del void
        /// Basado en: tamaño, profundidad (volumen), proximidad
        /// </summary>
        private double CalculateVoidScore(LiquidityVoidInfo voidInfo, int tfMinutes, int barIndex, double atr)
        {
            double midPrice = _provider.GetMidPrice(tfMinutes);
            double voidCenter = (voidInfo.Low + voidInfo.High) / 2.0;
            double distance = Math.Abs(midPrice - voidCenter);

            // Factor de tamaño (normalizado por ATR)
            double sizeScore = Math.Min(voidInfo.InefficiencyScore, 1.0);

            // Factor de profundidad (volumen bajo = mayor score)
            double depthScore = 1.0 - Math.Min(voidInfo.VolumeRatio, 1.0);

            // Factor de proximidad (más cerca = mayor score)
            double proxMax = _config.ProxMaxATRFactor * atr;
            double proximityScore = 1.0 - Math.Min(distance / proxMax, 1.0);

            // Score ponderado
            double score = (_config.LV_SizeWeight * sizeScore) +
                          (_config.LV_DepthWeight * depthScore) +
                          (_config.LV_ProximityWeight * proximityScore);

            // Aplicar multiplicador de confluencia si coincide con FVG/OB
            if (HasConfluenceWithStructures(voidInfo, tfMinutes))
            {
                score *= _config.LV_ConfluenceMultiplier;
            }

            return Math.Max(0.0, Math.Min(1.0, score)); // Clamp entre 0 y 1
        }

        /// <summary>
        /// Verifica si el void tiene confluencia con otras estructuras (FVG, OB)
        /// </summary>
        private bool HasConfluenceWithStructures(LiquidityVoidInfo voidInfo, int tfMinutes)
        {
            // Verificar confluencia con OBs
            var obs = _engine.GetOrderBlocks(tfMinutes);
            foreach (var ob in obs)
            {
                if (!ob.IsActive) continue;
                
                bool overlaps = !(ob.High < voidInfo.Low || ob.Low > voidInfo.High);
                if (overlaps)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Intenta fusionar el void con voids existentes cercanos
        /// </summary>
        private LiquidityVoidInfo TryMergeWithExistingVoids(LiquidityVoidInfo newVoid, int tfMinutes, double atr)
        {
            if (!_voidCacheByTF.ContainsKey(tfMinutes))
                return null;

            var existingVoids = _voidCacheByTF[tfMinutes];
            double fusionTolerance = _config.LV_FusionToleranceATR * atr;

            foreach (var existingVoid in existingVoids.ToList())
            {
                if (!existingVoid.IsActive || existingVoid.Direction != newVoid.Direction)
                    continue;

                // Verificar si están lo suficientemente cerca para fusionar
                double distance = Math.Min(
                    Math.Abs(newVoid.Low - existingVoid.High),
                    Math.Abs(existingVoid.Low - newVoid.High)
                );

                if (distance <= fusionTolerance)
                {
                    // Fusionar: extender el rango del void existente
                    existingVoid.Low = Math.Min(existingVoid.Low, newVoid.Low);
                    existingVoid.High = Math.Max(existingVoid.High, newVoid.High);
                    existingVoid.EndTime = newVoid.EndTime;
                    existingVoid.LastUpdatedBarIndex = newVoid.CreatedAtBarIndex;
                    existingVoid.IsExtended = true;
                    existingVoid.SizeTicks = (existingVoid.High - existingVoid.Low) / _provider.GetTickSize();

                    // Recalcular score
                    existingVoid.Score = CalculateVoidScore(existingVoid, tfMinutes, newVoid.CreatedAtBarIndex, atr);

                    // Verificar existencia antes de actualizar
                    if (_engine.GetStructureById(existingVoid.Id) != null)
                    {
                        _engine.UpdateStructure(existingVoid);
                    }
                    
                    return existingVoid;
                }
            }

            return null;
        }

        /// <summary>
        /// Actualiza voids existentes (tracking de toques y fill)
        /// </summary>
        private void UpdateExistingVoids(int tfMinutes, int barIndex)
        {
            if (!_voidCacheByTF.ContainsKey(tfMinutes))
                return;

            var voids = _voidCacheByTF[tfMinutes].ToList();
            var voidsToRemove = new List<LiquidityVoidInfo>();
            double currentHigh = _provider.GetHigh(tfMinutes, barIndex);
            double currentLow = _provider.GetLow(tfMinutes, barIndex);
            double currentClose = _provider.GetClose(tfMinutes, barIndex);

            foreach (var voidInfo in voids)
            {
                if (!voidInfo.IsActive || voidInfo.IsFilled)
                    continue;

                // Verificar toques
                bool bodyTouch = currentClose >= voidInfo.Low && currentClose <= voidInfo.High;
                bool wickTouch = (currentHigh >= voidInfo.Low && currentLow <= voidInfo.High) && !bodyTouch;

                if (bodyTouch)
                {
                    voidInfo.TouchCount_Body++;
                }
                else if (wickTouch)
                {
                    voidInfo.TouchCount_Wick++;
                }

                // Calcular fill percentage
                if (bodyTouch || wickTouch)
                {
                    double fillLow = Math.Max(currentLow, voidInfo.Low);
                    double fillHigh = Math.Min(currentHigh, voidInfo.High);
                    double filledRange = fillHigh - fillLow;
                    double totalRange = voidInfo.High - voidInfo.Low;

                    if (totalRange > 0)
                    {
                        double currentFill = filledRange / totalRange;
                        voidInfo.FillPercentage = Math.Max(voidInfo.FillPercentage, currentFill);

                        // Marcar como filled si supera el threshold
                        if (voidInfo.FillPercentage >= _config.LV_FillThreshold)
                        {
                            voidInfo.IsFilled = true;
                            voidInfo.IsCompleted = true;
                            if (_config.EnableDebug) _logger.Debug($"LiquidityVoidDetector: Void {voidInfo.Id} rellenado ({voidInfo.FillPercentage:P0})");
                        }
                    }
                }

                // Actualizar score (recalcular con nueva proximidad)
                double atr = _provider.GetATR(tfMinutes, 14, barIndex);
                if (atr > 0)
                {
                    voidInfo.Score = CalculateVoidScore(voidInfo, tfMinutes, barIndex, atr);
                }

                voidInfo.LastUpdatedBarIndex = barIndex;
                
                // Verificar existencia antes de actualizar
                if (_engine.GetStructureById(voidInfo.Id) != null)
                {
                    _engine.UpdateStructure(voidInfo);
                }
                else
                {
                    // Void fue purgado, marcar para remover del caché local
                    voidsToRemove.Add(voidInfo);
                }
            }

            // Remover purgados del caché local
            if (voidsToRemove.Count > 0)
            {
                foreach (var rem in voidsToRemove)
                {
                    _voidCacheByTF[tfMinutes].Remove(rem);
                }
            }
        }

        /// <summary>
        /// Añade un void al cache local
        /// </summary>
        private void AddToCache(int tfMinutes, LiquidityVoidInfo voidInfo)
        {
            if (!_voidCacheByTF.ContainsKey(tfMinutes))
                _voidCacheByTF[tfMinutes] = new List<LiquidityVoidInfo>();

            _voidCacheByTF[tfMinutes].Add(voidInfo);

            // Limitar tamaño del cache (mantener solo los últimos N voids activos)
            const int maxCacheSize = 50;
            if (_voidCacheByTF[tfMinutes].Count > maxCacheSize)
            {
                var toRemove = _voidCacheByTF[tfMinutes]
                    .Where(v => !v.IsActive || v.IsFilled)
                    .OrderBy(v => v.CreatedAtBarIndex)
                    .Take(_voidCacheByTF[tfMinutes].Count - maxCacheSize)
                    .ToList();

                foreach (var v in toRemove)
                {
                    _voidCacheByTF[tfMinutes].Remove(v);
                }
            }
        }

        public void Dispose()
        {
            _voidCacheByTF.Clear();
            _logger.Info("LiquidityVoidDetector: Disposed");
        }
    }
}

