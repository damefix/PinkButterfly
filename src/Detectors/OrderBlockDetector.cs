// ============================================================================
// OrderBlockDetector.cs
// PinkButterfly CoreBrain - Detector de Order Blocks
// 
// Detecta, valida y trackea Order Blocks (OB) según reglas SMC:
// - Detección por tamaño de cuerpo (>= OBBodyMinATR * ATR)
// - Detección opcional por volumen spike (si disponible)
// - Tracking de mitigación (precio retorna a la zona)
// - Detección de Breaker Blocks (OB roto que actúa en dirección contraria)
// - Metadata con volumen y contexto
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Detector de Order Blocks (OB)
    /// 
    /// REGLAS DE DETECCIÓN:
    /// - Vela con cuerpo grande: bodySize >= OBBodyMinATR * ATR(14)
    /// - Opcional: volumen spike (si disponible): volume > avgVolume * volSpikeFactor
    /// - Rango OB = cuerpo de la vela (Open/Close)
    /// - Direction: "Bullish" si Close > Open, "Bearish" si Close < Open
    /// 
    /// MITIGACIÓN:
    /// - IsMitigated=true cuando precio retorna a la zona OB
    /// - Fill percentage tracking similar a FVG
    /// 
    /// BREAKER BLOCKS:
    /// - IsBreaker=true cuando OB es roto completamente y luego retesteado
    /// - Breaker actúa en dirección contraria al OB original
    /// </summary>
    public class OrderBlockDetector : IDetector
    {
        private IBarDataProvider _provider;
        private EngineConfig _config;
        private ILogger _logger;
        private CoreEngine _engine;

        // Cache de OBs por TF para tracking de mitigación y breakers
        private Dictionary<int, List<OrderBlockInfo>> _obCacheByTF = new Dictionary<int, List<OrderBlockInfo>>();

        // Parámetros adicionales (no en EngineConfig por ahora)
        private const double VOL_SPIKE_FACTOR = 1.5; // volumen > 1.5x promedio
        private const int VOL_AVG_PERIOD = 20; // período para calcular volumen promedio

        public void Initialize(IBarDataProvider provider, EngineConfig config, ILogger logger)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _logger.Info("OrderBlockDetector: Inicializado");
        }

        public void OnBarClose(int tfMinutes, int barIndex, CoreEngine engine)
        {
            _engine = engine;

            // Necesitamos al menos VOL_AVG_PERIOD barras para calcular volumen promedio
            if (barIndex < VOL_AVG_PERIOD)
                return;

            try
            {
                // Detectar nuevos OBs
                DetectOrderBlocks(tfMinutes, barIndex);
                
                // Actualizar OBs existentes
                UpdateExistingOrderBlocks(tfMinutes, barIndex);
            }
            catch (Exception ex)
            {
                _logger.Exception($"OrderBlockDetector: Error en TF{tfMinutes} bar{barIndex}", ex);
            }
        }

        /// <summary>
        /// Detecta nuevos Order Blocks en el cierre de la barra actual
        /// </summary>
        private void DetectOrderBlocks(int tfMinutes, int barIndex)
        {
            double open = _provider.GetOpen(tfMinutes, barIndex);
            double close = _provider.GetClose(tfMinutes, barIndex);
            double high = _provider.GetHigh(tfMinutes, barIndex);
            double low = _provider.GetLow(tfMinutes, barIndex);
            double atr = _provider.GetATR(tfMinutes, 14, barIndex);

            // ================================================================
            // VALIDACIÓN 1: TAMAÑO DE CUERPO
            // ================================================================
            double bodySize = Math.Abs(close - open);
            double minBodySize = _config.OBBodyMinATR * atr;

            if (_config.EnableDebug)
                _logger.Debug($"OrderBlockDetector: Bar {barIndex} - O:{open} C:{close} Body:{bodySize:F2} MinBody:{minBodySize:F2} ATR:{atr:F2}");

            if (bodySize < minBodySize)
            {
                if (_config.EnableDebug)
                    _logger.Debug($"OrderBlockDetector: Body too small, skipping");
                return; // Cuerpo muy pequeño, no es OB
            }

            // ================================================================
            // VALIDACIÓN 2: VOLUMEN SPIKE (OPCIONAL)
            // ================================================================
            bool volumeConfirmed = false;
            double? currentVolume = _provider.GetVolume(tfMinutes, barIndex);

            if (currentVolume.HasValue)
            {
                double avgVolume = CalculateAverageVolume(tfMinutes, barIndex, VOL_AVG_PERIOD);
                
                if (avgVolume > 0 && currentVolume.Value > avgVolume * VOL_SPIKE_FACTOR)
                {
                    volumeConfirmed = true;
                    
                    if (_config.EnableDebug)
                        _logger.Debug($"OrderBlockDetector: Volume spike detected - " +
                                     $"Current:{currentVolume.Value:F0} Avg:{avgVolume:F0} " +
                                     $"(Factor:{VOL_SPIKE_FACTOR})");
                }
            }

            // ================================================================
            // CREAR ORDER BLOCK
            // ================================================================
            string direction = close > open ? "Bullish" : "Bearish";
            
            // Rango OB = cuerpo (Open/Close)
            double obLow = Math.Min(open, close);
            double obHigh = Math.Max(open, close);

            var ob = new OrderBlockInfo
            {
                Id = Guid.NewGuid().ToString(),
                Type = "OB",
                Direction = direction,
                TF = tfMinutes,
                StartTime = _provider.GetBarTime(tfMinutes, barIndex),
                EndTime = _provider.GetBarTime(tfMinutes, barIndex),
                High = obHigh,
                Low = obLow,
                OpenPrice = open,
                ClosePrice = close,
                IsActive = true,
                IsCompleted = false,
                IsBreaker = false,
                IsMitigated = false,
                CreatedAtBarIndex = barIndex,
                LastUpdatedBarIndex = barIndex,
                Score = 0.0,
                TouchCount_Body = 0,
                TouchCount_Wick = 0
            };

            // ================================================================
            // METADATA
            // ================================================================
            ob.Metadata.VolumeAtCreation = currentVolume;
            ob.Metadata.AverageRangeDuringFormation = bodySize;
            ob.Metadata.CreatedByDetector = "OrderBlockDetector";
            
            // Tag adicional si hay confirmación por volumen
            if (volumeConfirmed)
            {
                ob.Metadata.Tags["VolumeConfirmed"] = "true";
                ob.Metadata.Tags["VolumeSpikeFactor"] = VOL_SPIKE_FACTOR.ToString("F1");
            }

            // ================================================================
            // AÑADIR AL ENGINE Y CACHE
            // ================================================================
            _engine.AddStructure(ob);

            if (!_obCacheByTF.ContainsKey(tfMinutes))
                _obCacheByTF[tfMinutes] = new List<OrderBlockInfo>();
            
            _obCacheByTF[tfMinutes].Add(ob);

            if (_config.EnableDebug)
                _logger.Debug($"OrderBlockDetector: Created {ob.Direction} OB {ob.Id} TF{tfMinutes} " +
                             $"[{ob.Low:F2}-{ob.High:F2}] Body:{bodySize:F2} MinBody:{minBodySize:F2} " +
                             $"VolConfirmed:{volumeConfirmed}");
        }

        /// <summary>
        /// Calcula el volumen promedio de las últimas N barras
        /// </summary>
        private double CalculateAverageVolume(int tfMinutes, int barIndex, int period)
        {
            double sum = 0;
            int count = 0;

            for (int i = barIndex - period + 1; i <= barIndex; i++)
            {
                if (i < 0)
                    continue;

                double? vol = _provider.GetVolume(tfMinutes, i);
                if (vol.HasValue)
                {
                    sum += vol.Value;
                    count++;
                }
            }

            return count > 0 ? sum / count : 0;
        }

        /// <summary>
        /// Actualiza Order Blocks existentes:
        /// - Tracking de mitigación (precio retorna a la zona)
        /// - Detección de Breaker Blocks (OB roto y retesteado)
        /// </summary>
        private void UpdateExistingOrderBlocks(int tfMinutes, int barIndex)
        {
            if (!_obCacheByTF.ContainsKey(tfMinutes))
                return;

            double currentClose = _provider.GetClose(tfMinutes, barIndex);
            double currentHigh = _provider.GetHigh(tfMinutes, barIndex);
            double currentLow = _provider.GetLow(tfMinutes, barIndex);

            var activeOBs = _obCacheByTF[tfMinutes]
                .Where(ob => ob.IsActive)
                .ToList();

            foreach (var ob in activeOBs)
            {
                // No procesar OBs creados en esta misma barra (evitar auto-touch/auto-mitigación)
                if (ob.CreatedAtBarIndex == barIndex)
                    continue;

                bool updated = false;

                // ============================================================
                // VERIFICAR SI EL PRECIO ESTÁ DENTRO O FUERA DE LA ZONA
                // ============================================================
                
                // Precio está en zona si CUALQUIER parte de la barra toca la zona del OB
                bool priceInZone = (currentClose >= ob.Low && currentClose <= ob.High) ||
                                   (currentHigh >= ob.Low && currentHigh <= ob.High) ||
                                   (currentLow >= ob.Low && currentLow <= ob.High);

                // Precio ha salido completamente si TODA la barra está fuera de la zona
                bool priceCompletelyOut = (currentHigh < ob.Low) || (currentLow > ob.High);

                // ============================================================
                // TRACKING DE SALIDA DE ZONA (HasLeftZone)
                // ============================================================
                
                // Solo marcar como "salido" si el precio está COMPLETAMENTE fuera
                if (priceCompletelyOut && !ob.HasLeftZone)
                {
                    ob.HasLeftZone = true;
                    updated = true;

                    if (_config.EnableDebug)
                        _logger.Debug($"OrderBlockDetector: OB {ob.Id} - Price has left zone at bar {barIndex}");
                }

                // ============================================================
                // TOUCH DETECTION (solo si el precio está en la zona)
                // ============================================================
                
                if (priceInZone)
                {
                    // Body touch: el close está dentro del OB
                    if (currentClose >= ob.Low && currentClose <= ob.High)
                    {
                        ob.TouchCount_Body++;
                        updated = true;

                        if (_config.EnableDebug)
                            _logger.Debug($"OrderBlockDetector: Body touch on {ob.Id} (count={ob.TouchCount_Body})");
                    }
                    // Wick touch: high/low toca el OB pero close no
                    else if ((currentHigh >= ob.Low && currentHigh <= ob.High) ||
                             (currentLow >= ob.Low && currentLow <= ob.High))
                    {
                        ob.TouchCount_Wick++;
                        updated = true;

                        if (_config.EnableDebug)
                            _logger.Debug($"OrderBlockDetector: Wick touch on {ob.Id} (count={ob.TouchCount_Wick})");
                    }
                }

                // ============================================================
                // MITIGACIÓN DETECTION (PROFESIONAL)
                // ============================================================
                
                // LÓGICA CORRECTA: El OB solo se mitiga cuando:
                // 1. El precio ha salido de la zona (HasLeftZone = true)
                // 2. Y ahora retorna a la zona (priceInZone = true)
                
                if (!ob.IsMitigated && ob.HasLeftZone && priceInZone)
                {
                    ob.IsMitigated = true;
                    updated = true;

                    if (_config.EnableDebug)
                        _logger.Debug($"OrderBlockDetector: OB {ob.Id} MITIGATED at bar {barIndex} " +
                                     $"(HasLeftZone=true, PriceReturned=true)");
                }

                // ============================================================
                // BREAKER BLOCK DETECTION
                // ============================================================
                
                // Breaker: OB que fue completamente roto y ahora actúa en dirección contraria
                // Para bullish OB: roto si close < ob.Low, breaker si luego retestea desde abajo
                // Para bearish OB: roto si close > ob.High, breaker si luego retestea desde arriba
                
                if (!ob.IsBreaker)
                {
                    bool breaker = false;

                    if (ob.Direction == "Bullish")
                    {
                        // Bullish OB roto: close por debajo de ob.Low
                        // Breaker: precio retorna desde abajo y toca la zona
                        if (currentLow < ob.Low && currentHigh >= ob.Low)
                        {
                            // Precio cruzó de abajo hacia arriba, tocando la zona
                            breaker = true;
                        }
                    }
                    else // Bearish
                    {
                        // Bearish OB roto: close por encima de ob.High
                        // Breaker: precio retorna desde arriba y toca la zona
                        if (currentHigh > ob.High && currentLow <= ob.High)
                        {
                            // Precio cruzó de arriba hacia abajo, tocando la zona
                            breaker = true;
                        }
                    }

                    if (breaker)
                    {
                        ob.IsBreaker = true;
                        ob.Metadata.Tags["BreakerDetectedAt"] = barIndex.ToString();
                        updated = true;

                        if (_config.EnableDebug)
                            _logger.Debug($"OrderBlockDetector: OB {ob.Id} became BREAKER at bar {barIndex}");
                    }
                }

                // ============================================================
                // UPDATE ENGINE
                // ============================================================
                
                if (updated)
                {
                    ob.LastUpdatedBarIndex = barIndex;
                    
                    // Verificar que la estructura aún existe antes de actualizar
                    if (_engine.GetStructureById(ob.Id) != null)
                    {
                        _engine.UpdateStructure(ob);
                    }
                    else
                    {
                        // La estructura fue purgada, removerla de la lista local
                        obsToRemove.Add(ob);
                    }
                }
            }
        }

        public void Dispose()
        {
            _obCacheByTF.Clear();
            _logger.Info("OrderBlockDetector: Disposed");
        }
    }
}

// ============================================================================
// NOTAS DE IMPLEMENTACIÓN
// ============================================================================
//
// 1. DETECCIÓN:
//    - Requiere cuerpo >= OBBodyMinATR * ATR(14)
//    - Opcional: volumen > avgVolume * VOL_SPIKE_FACTOR
//    - Rango OB = cuerpo (min/max de Open/Close)
//    - Direction: Bullish si Close > Open, Bearish si Close < Open
//
// 2. MITIGACIÓN:
//    - IsMitigated=true cuando precio retorna a la zona OB
//    - Bullish OB: mitigado cuando precio baja y toca desde arriba
//    - Bearish OB: mitigado cuando precio sube y toca desde abajo
//    - Similar al concepto de "fill" en FVGs
//
// 3. BREAKER BLOCKS:
//    - IsBreaker=true cuando OB es roto completamente y luego retesteado
//    - Bullish OB → Breaker: close < ob.Low, luego retestea desde abajo
//    - Bearish OB → Breaker: close > ob.High, luego retestea desde arriba
//    - Breaker actúa en dirección contraria al OB original
//
// 4. VOLUMEN:
//    - Si disponible, usa volumen como confirmación adicional
//    - VOL_SPIKE_FACTOR = 1.5 (volumen > 1.5x promedio)
//    - VOL_AVG_PERIOD = 20 barras para calcular promedio
//    - Tag "VolumeConfirmed" en metadata si hay spike
//
// 5. SCORING:
//    - Score calculado automáticamente por ScoringEngine
//    - Factores: TF weight, freshness, proximity, type weight, touches
//    - OBs mitigados mantienen score pero con penalización
//    - Breakers pueden tener score más alto (inversión de rol)
//
// 6. PERFORMANCE:
//    - Cache local (_obCacheByTF) para tracking eficiente
//    - Solo itera sobre OBs activos del mismo TF
//    - O(n) donde n = OBs activos en el TF (típicamente < 30)
//
// 7. DIFERENCIAS CON FVG:
//    - OB es una sola vela (no requiere 3 barras)
//    - OB se basa en cuerpo, FVG en mechas
//    - OB tiene concepto de Breaker (cambio de rol)
//    - OB usa volumen como confirmación adicional
//
// ============================================================================

