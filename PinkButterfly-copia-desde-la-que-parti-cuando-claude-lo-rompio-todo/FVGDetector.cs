// ============================================================================
// FVGDetector.cs
// PinkButterfly CoreBrain - Detector de Fair Value Gaps
// 
// Detecta, valida, mergea y trackea FVGs (Fair Value Gaps) según reglas SMC:
// - Detección de gaps bullish/bearish
// - Validación por tamaño (ticks + ATR)
// - Merge de FVGs consecutivos
// - Detección de FVGs anidados
// - Tracking de toques (body/wick)
// - Cálculo de Fill Percentage
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Detector de Fair Value Gaps (FVG)
    /// 
    /// REGLAS DE DETECCIÓN:
    /// - Requiere 3 barras: A (i-2), B (i-1), C (i)
    /// - Bullish FVG: lowA > highC (gap entre mechas)
    /// - Bearish FVG: highA < lowC
    /// - Validación: size >= max(MinFVGSizeTicks * TickSize, MinFVGSizeATRfactor * ATR)
    /// - Merge: si MergeConsecutiveFVGs=true, fusionar FVGs adyacentes
    /// - Nested: si DetectNestedFVGs=true, detectar FVGs dentro de FVGs
    /// </summary>
    public class FVGDetector : IDetector
    {
        private IBarDataProvider _provider;
        private EngineConfig _config;
        private ILogger _logger;
        private CoreEngine _engine;

        // Cache de FVGs por TF para merge/nested detection
        private Dictionary<int, List<FVGInfo>> _fvgCacheByTF = new Dictionary<int, List<FVGInfo>>();

        public void Initialize(IBarDataProvider provider, EngineConfig config, ILogger logger)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _logger.Info("FVGDetector: Inicializado");
        }

        public void OnBarClose(int tfMinutes, int barIndex, CoreEngine engine)
        {
            _engine = engine;

            // Necesitamos al menos 3 barras cerradas (índices i-2, i-1, i)
            if (barIndex < 2)
                return;

            try
            {
                DetectFVGs(tfMinutes, barIndex);
                UpdateExistingFVGs(tfMinutes, barIndex);
            }
            catch (Exception ex)
            {
                _logger.Exception($"FVGDetector: Error en TF{tfMinutes} bar{barIndex}", ex);
            }
        }

        /// <summary>
        /// Detecta nuevos FVGs en el cierre de la barra actual
        /// </summary>
        private void DetectFVGs(int tfMinutes, int barIndex)
        {
            // Índices de las 3 barras: A=i-2, B=i-1, C=i
            int idxA = barIndex - 2;
            int idxB = barIndex - 1;
            int idxC = barIndex;

            // Obtener precios de las 3 barras
            double highA = _provider.GetHigh(tfMinutes, idxA);
            double lowA = _provider.GetLow(tfMinutes, idxA);
            double highB = _provider.GetHigh(tfMinutes, idxB);
            double lowB = _provider.GetLow(tfMinutes, idxB);
            double highC = _provider.GetHigh(tfMinutes, idxC);
            double lowC = _provider.GetLow(tfMinutes, idxC);

            double tickSize = _provider.GetTickSize();
            double atr = _provider.GetATR(tfMinutes, 14, barIndex);

            // ================================================================
            // DETECCIÓN BULLISH FVG: lowA > highC
            // ================================================================
            if (lowA > highC)
            {
                double gapSize = lowA - highC;
                if (IsValidFVGSize(gapSize, atr, tickSize))
                {
                    CreateFVG(tfMinutes, barIndex, "Bullish", highC, lowA, idxA, idxB, idxC);
                }
            }

            // ================================================================
            // DETECCIÓN BEARISH FVG: highA < lowC
            // ================================================================
            if (highA < lowC)
            {
                double gapSize = lowC - highA;
                if (IsValidFVGSize(gapSize, atr, tickSize))
                {
                    CreateFVG(tfMinutes, barIndex, "Bearish", highA, lowC, idxA, idxB, idxC);
                }
            }
        }

        /// <summary>
        /// Valida que el tamaño del gap cumpla los requisitos mínimos
        /// </summary>
        private bool IsValidFVGSize(double gapSize, double atr, double tickSize)
        {
            double minSizeTicks = _config.MinFVGSizeTicks * tickSize;
            double minSizeATR = _config.MinFVGSizeATRfactor * atr;
            double minSize = Math.Max(minSizeTicks, minSizeATR);

            return gapSize >= minSize;
        }

        /// <summary>
        /// Crea un nuevo FVG y lo añade al engine (con merge/nested logic)
        /// </summary>
        private void CreateFVG(int tfMinutes, int barIndex, string direction, 
                               double low, double high, int idxA, int idxB, int idxC)
        {
            // Crear FVGInfo
            var fvg = new FVGInfo
            {
                Id = Guid.NewGuid().ToString(),
                Type = "FVG",
                Direction = direction,
                TF = tfMinutes,
                StartTime = _provider.GetBarTime(tfMinutes, idxA),
                EndTime = _provider.GetBarTime(tfMinutes, idxC),
                High = high,
                Low = low,
                IsActive = true,
                IsCompleted = false,
                CreatedAtBarIndex = barIndex,
                LastUpdatedBarIndex = barIndex,
                Score = 0.0,
                TouchCount_Body = 0,
                TouchCount_Wick = 0,
                FillPercentage = 0.0,
                TouchedRecently = false,
                ParentId = null,
                DepthLevel = 0
            };

            // Metadata
            double? volume = _provider.GetVolume(tfMinutes, barIndex);
            fvg.Metadata.VolumeAtCreation = volume;
            fvg.Metadata.CreatedByDetector = "FVGDetector";

            // ================================================================
            // MERGE LOGIC: Si MergeConsecutiveFVGs=true
            // ================================================================
            if (_config.MergeConsecutiveFVGs)
            {
                var merged = TryMergeWithExisting(fvg, tfMinutes);
                if (merged != null)
                {
                    // Se mergeó con uno existente, actualizar en engine
                    _engine.UpdateStructure(merged);
                    
                    if (_config.EnableDebug)
                        _logger.Debug($"FVGDetector: Merged FVG {fvg.Direction} TF{tfMinutes} into {merged.Id}");
                    
                    return;
                }
            }

            // ================================================================
            // NESTED LOGIC: Si DetectNestedFVGs=true
            // ================================================================
            if (_config.DetectNestedFVGs)
            {
                DetectNested(fvg, tfMinutes);
            }

            // ================================================================
            // AÑADIR AL ENGINE
            // ================================================================
            _engine.AddStructure(fvg);

            // Añadir al cache local
            if (!_fvgCacheByTF.ContainsKey(tfMinutes))
                _fvgCacheByTF[tfMinutes] = new List<FVGInfo>();
            
            _fvgCacheByTF[tfMinutes].Add(fvg);

            if (_config.EnableDebug)
                _logger.Debug($"FVGDetector: Created {fvg.Direction} FVG {fvg.Id} TF{tfMinutes} [{fvg.Low:F5}-{fvg.High:F5}]");
        }

        /// <summary>
        /// Intenta mergear el FVG con uno existente adyacente o solapado
        /// Retorna el FVG mergeado si tuvo éxito, null si no
        /// </summary>
        private FVGInfo TryMergeWithExisting(FVGInfo newFvg, int tfMinutes)
        {
            if (!_fvgCacheByTF.ContainsKey(tfMinutes))
                return null;

            var existingFvgs = _fvgCacheByTF[tfMinutes]
                .Where(f => f.Direction == newFvg.Direction && f.IsActive)
                .ToList();

            foreach (var existing in existingFvgs)
            {
                // Verificar si son adyacentes o se solapan
                bool overlaps = !(newFvg.High < existing.Low || newFvg.Low > existing.High);
                bool adjacent = Math.Abs(newFvg.Low - existing.High) < _provider.GetTickSize() * 2 ||
                                Math.Abs(newFvg.High - existing.Low) < _provider.GetTickSize() * 2;

                if (overlaps || adjacent)
                {
                    // Mergear: extender el rango
                    existing.Low = Math.Min(existing.Low, newFvg.Low);
                    existing.High = Math.Max(existing.High, newFvg.High);
                    existing.EndTime = newFvg.EndTime;
                    existing.LastUpdatedBarIndex = newFvg.CreatedAtBarIndex;

                    return existing;
                }
            }

            return null;
        }

        /// <summary>
        /// Detecta si el FVG está anidado dentro de otro FVG más grande
        /// Busca el FVG padre más específico (el más pequeño que lo contenga)
        /// </summary>
        private void DetectNested(FVGInfo newFvg, int tfMinutes)
        {
            if (!_fvgCacheByTF.ContainsKey(tfMinutes))
                return;

            var existingFvgs = _fvgCacheByTF[tfMinutes]
                .Where(f => f.Direction == newFvg.Direction && f.IsActive)
                .ToList();

            // Buscar el FVG más pequeño (más específico) que contenga al nuevo FVG
            FVGInfo bestParent = null;
            double smallestSize = double.MaxValue;

            foreach (var existing in existingFvgs)
            {
                // Verificar si newFvg está completamente dentro de existing
                if (newFvg.Low >= existing.Low && newFvg.High <= existing.High)
                {
                    double existingSize = existing.High - existing.Low;
                    
                    // Si es más pequeño que el mejor candidato actual, usarlo
                    if (existingSize < smallestSize)
                    {
                        bestParent = existing;
                        smallestSize = existingSize;
                    }
                }
            }

            // Asignar el mejor padre encontrado
            if (bestParent != null)
            {
                newFvg.ParentId = bestParent.Id;
                newFvg.DepthLevel = bestParent.DepthLevel + 1;
                
                if (_config.EnableDebug)
                    _logger.Debug($"FVGDetector: Nested FVG {newFvg.Id} inside {bestParent.Id} (depth={newFvg.DepthLevel})");
            }
        }

        /// <summary>
        /// Actualiza FVGs existentes: toques, fill percentage, etc.
        /// </summary>
        private void UpdateExistingFVGs(int tfMinutes, int barIndex)
        {
            if (!_fvgCacheByTF.ContainsKey(tfMinutes))
                return;

            double currentClose = _provider.GetClose(tfMinutes, barIndex);
            double currentHigh = _provider.GetHigh(tfMinutes, barIndex);
            double currentLow = _provider.GetLow(tfMinutes, barIndex);

            var activeFvgs = _fvgCacheByTF[tfMinutes]
                .Where(f => f.IsActive)
                .ToList();

            foreach (var fvg in activeFvgs)
            {
                bool updated = false;

                // ============================================================
                // TOUCH DETECTION
                // ============================================================
                
                // Body touch: el close está dentro del FVG
                if (currentClose >= fvg.Low && currentClose <= fvg.High)
                {
                    fvg.TouchCount_Body++;
                    fvg.TouchedRecently = true;
                    updated = true;

                    if (_config.EnableDebug)
                        _logger.Debug($"FVGDetector: Body touch on {fvg.Id} (count={fvg.TouchCount_Body})");
                }
                // Wick touch: high/low toca el FVG pero close no
                else if ((currentHigh >= fvg.Low && currentHigh <= fvg.High) ||
                         (currentLow >= fvg.Low && currentLow <= fvg.High))
                {
                    fvg.TouchCount_Wick++;
                    fvg.TouchedRecently = true;
                    updated = true;

                    if (_config.EnableDebug)
                        _logger.Debug($"FVGDetector: Wick touch on {fvg.Id} (count={fvg.TouchCount_Wick})");
                }

                // ============================================================
                // FILL PERCENTAGE CALCULATION
                // ============================================================
                
                // Calcular cuánto del FVG ha sido "rellenado" por el precio
                double fillPerc = CalculateFillPercentage(fvg, currentClose, currentHigh, currentLow);
                
                if (fillPerc > fvg.FillPercentage)
                {
                    fvg.FillPercentage = fillPerc;
                    updated = true;

                    // Si se alcanza el threshold de fill, marcar como completado
                    if (fillPerc >= _config.FillThreshold)
                    {
                        fvg.IsCompleted = true;
                        
                        if (_config.EnableDebug)
                            _logger.Debug($"FVGDetector: FVG {fvg.Id} filled {fillPerc:P0} (threshold={_config.FillThreshold:P0})");
                    }
                }

                // ============================================================
                // UPDATE ENGINE
                // ============================================================
                
                if (updated)
                {
                    fvg.LastUpdatedBarIndex = barIndex;
                    
                    // Verificar que la estructura aún existe antes de actualizar
                    // (puede haber sido purgada por el sistema de purga inteligente)
                    if (_engine.GetStructureById(fvg.Id) != null)
                    {
                        _engine.UpdateStructure(fvg);
                    }
                    else
                    {
                        // La estructura fue purgada, removerla de la lista local
                        fvgsToRemove.Add(fvg);
                    }
                }
            }
        }

        /// <summary>
        /// Calcula el porcentaje de fill del FVG basado en el precio actual
        /// 
        /// LÓGICA:
        /// - Si el precio cruza completamente el FVG → 100%
        /// - Si entra parcialmente → porcentaje proporcional
        /// - Para bullish FVG: fill desde abajo (highC hacia lowA)
        /// - Para bearish FVG: fill desde arriba (lowC hacia highA)
        /// </summary>
        private double CalculateFillPercentage(FVGInfo fvg, double close, double high, double low)
        {
            double fvgRange = fvg.High - fvg.Low;
            if (fvgRange <= 0)
                return 0.0;

            if (fvg.Direction == "Bullish")
            {
                // Bullish FVG: fill desde abajo
                // Si close cruza por encima de fvg.High → 100%
                if (close >= fvg.High)
                    return 1.0;
                
                // Si close está dentro del FVG, calcular proporción
                if (close > fvg.Low && close < fvg.High)
                {
                    double filled = close - fvg.Low;
                    return filled / fvgRange;
                }

                // Si solo wick toca, considerar fill parcial menor
                if (high >= fvg.Low && high < fvg.High)
                {
                    double wickFilled = high - fvg.Low;
                    return (wickFilled / fvgRange) * 0.5; // 50% de peso para wick
                }
            }
            else // Bearish
            {
                // Bearish FVG: fill desde arriba
                // Si close cruza por debajo de fvg.Low → 100%
                if (close <= fvg.Low)
                    return 1.0;
                
                // Si close está dentro del FVG, calcular proporción
                if (close < fvg.High && close > fvg.Low)
                {
                    double filled = fvg.High - close;
                    return filled / fvgRange;
                }

                // Si solo wick toca, considerar fill parcial menor
                if (low <= fvg.High && low > fvg.Low)
                {
                    double wickFilled = fvg.High - low;
                    return (wickFilled / fvgRange) * 0.5; // 50% de peso para wick
                }
            }

            return fvg.FillPercentage; // Mantener el valor anterior
        }

        public void Dispose()
        {
            _fvgCacheByTF.Clear();
            _logger.Info("FVGDetector: Disposed");
        }
    }
}

// ============================================================================
// NOTAS DE IMPLEMENTACIÓN
// ============================================================================
//
// 1. DETECCIÓN:
//    - Requiere 3 barras cerradas (A, B, C)
//    - Bullish: gap entre lowA y highC
//    - Bearish: gap entre highA y lowC
//    - Validación: max(MinFVGSizeTicks * TickSize, MinFVGSizeATRfactor * ATR)
//
// 2. MERGE:
//    - Si MergeConsecutiveFVGs=true, fusiona FVGs adyacentes/solapados
//    - Extiende el rango (min Low, max High)
//    - Actualiza EndTime y LastUpdatedBarIndex
//
// 3. NESTED:
//    - Si DetectNestedFVGs=true, detecta FVGs dentro de otros
//    - Asigna ParentId y DepthLevel
//    - Útil para identificar zonas de alta confluencia
//
// 4. TOQUES:
//    - Body touch: close dentro del FVG → TouchCount_Body++
//    - Wick touch: high/low toca pero close no → TouchCount_Wick++
//    - TouchedRecently flag para scoring
//
// 5. FILL:
//    - Calcula porcentaje de fill basado en penetración del precio
//    - Bullish: fill desde abajo (close sube hacia High)
//    - Bearish: fill desde arriba (close baja hacia Low)
//    - Si FillPercentage >= FillThreshold → IsCompleted=true
//    - Score se ajusta a ResidualScore cuando está filled
//
// 6. PERFORMANCE:
//    - Cache local (_fvgCacheByTF) para merge/nested detection
//    - Solo itera sobre FVGs activos del mismo TF
//    - O(n) donde n = FVGs activos en el TF (típicamente < 50)
//
// ============================================================================

