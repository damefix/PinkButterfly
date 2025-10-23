using System;
using System.Collections.Generic;
using System.Linq;
using NinjaTrader.NinjaScript.Indicators.PinkButterfly;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Detector de Double Tops y Double Bottoms
    /// 
    /// LÓGICA PROFESIONAL:
    /// - Detecta cuando 2 swings del mismo tipo (High/High o Low/Low) forman un patrón Double
    /// - Valida proximidad de precio (priceToleranceTicks)
    /// - Valida distancia temporal (minBarsBetween, maxBarsBetween)
    /// - Calcula neckline automáticamente
    /// - Trackea confirmación (precio rompe neckline en dirección esperada)
    /// - Invalida si no confirma dentro de maxWaitBars
    /// 
    /// ESTADOS:
    /// - "Pending": Detectado pero no confirmado
    /// - "Confirmed": Precio rompió neckline en dirección esperada
    /// - "Invalid": Invalidado por timeout o movimiento contrario
    /// </summary>
    public class DoubleDetector : IDetector
    {
        private IBarDataProvider _provider;
        private EngineConfig _config;
        private ILogger _logger;
        private CoreEngine _engine;

        // Cache de doubles por TF para performance
        private Dictionary<int, List<DoubleTopInfo>> _doubleCacheByTF = new Dictionary<int, List<DoubleTopInfo>>();

        public void Initialize(IBarDataProvider provider, EngineConfig config, ILogger logger)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (_config.EnableDebug)
                _logger.Debug("DoubleDetector: Initialized");
        }

        public void OnBarClose(int tfMinutes, int barIndex, CoreEngine engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));

            try
            {
                // 1. Detectar nuevos Double Tops/Bottoms basados en swings recientes
                DetectNewDoubles(tfMinutes, barIndex);

                // 2. Actualizar doubles existentes (confirmación/invalidación)
                UpdateExistingDoubles(tfMinutes, barIndex);
            }
            catch (Exception ex)
            {
                _logger.Exception($"DoubleDetector: Error en TF{tfMinutes} bar{barIndex}", ex);
            }
        }

        /// <summary>
        /// Detecta nuevos Double Tops/Bottoms cuando se crea un nuevo swing
        /// </summary>
        private void DetectNewDoubles(int tfMinutes, int barIndex)
        {
            // Obtener swings recientes del mismo TF
            var recentSwings = _engine.GetRecentSwings(tfMinutes, _config.MaxBarsBetweenDouble + 10);

            if (recentSwings.Count < 2)
                return;

            // Ordenar por fecha (más reciente primero)
            var sortedSwings = recentSwings.OrderByDescending(s => s.CreatedAtBarIndex).ToList();

            // El swing más reciente es el candidato
            var candidateSwing = sortedSwings[0];

            // Buscar swings previos del mismo tipo (High o Low)
            var previousSwings = sortedSwings
                .Skip(1)
                .Where(s => s.IsHigh == candidateSwing.IsHigh && s.IsActive)
                .ToList();

            foreach (var previousSwing in previousSwings)
            {
                // Validar distancia temporal
                int barsBetween = candidateSwing.CreatedAtBarIndex - previousSwing.CreatedAtBarIndex;

                if (barsBetween < _config.MinBarsBetweenDouble)
                    continue; // Demasiado cerca

                if (barsBetween > _config.MaxBarsBetweenDouble)
                    break; // Demasiado lejos (y los siguientes serán aún más viejos)

                // Validar proximidad de precio
                double price1 = candidateSwing.IsHigh ? candidateSwing.High : candidateSwing.Low;
                double price2 = previousSwing.IsHigh ? previousSwing.High : previousSwing.Low;
                double priceDiff = Math.Abs(price1 - price2);
                double maxDiff = _config.priceToleranceTicks_DoubleTop * _provider.GetTickSize();

                if (priceDiff > maxDiff)
                    continue; // Precios demasiado diferentes

                // ¡DOUBLE DETECTADO!
                CreateDouble(tfMinutes, barIndex, candidateSwing, previousSwing);

                // Solo crear un double por swing (el más cercano en precio)
                break;
            }
        }

        /// <summary>
        /// Crea un nuevo Double Top o Double Bottom
        /// </summary>
        private void CreateDouble(int tfMinutes, int barIndex, SwingInfo swing1, SwingInfo swing2)
        {
            bool isDoubleTop = swing1.IsHigh;
            double necklinePrice;

            if (isDoubleTop)
            {
                // Para Double Top: neckline es el mínimo entre los dos tops
                // Buscar el low más bajo entre los dos swings
                necklinePrice = FindLowestLowBetween(tfMinutes, swing2.CreatedAtBarIndex, swing1.CreatedAtBarIndex);
            }
            else
            {
                // Para Double Bottom: neckline es el máximo entre los dos bottoms
                // Buscar el high más alto entre los dos swings
                necklinePrice = FindHighestHighBetween(tfMinutes, swing2.CreatedAtBarIndex, swing1.CreatedAtBarIndex);
            }

            var doublePattern = new DoubleTopInfo
            {
                Id = Guid.NewGuid().ToString(),
                Type = isDoubleTop ? "DOUBLE_TOP" : "DOUBLE_BOTTOM",
                TF = tfMinutes,
                StartTime = swing2.StartTime, // El swing más viejo
                EndTime = swing1.EndTime,     // El swing más reciente
                High = isDoubleTop ? Math.Max(swing1.High, swing2.High) : necklinePrice,
                Low = isDoubleTop ? necklinePrice : Math.Min(swing1.Low, swing2.Low),
                IsActive = true,
                IsCompleted = false, // No está completo hasta confirmar
                CreatedAtBarIndex = barIndex,
                LastUpdatedBarIndex = barIndex,
                Score = 0.0, // Se calculará por el engine
                TouchCount_Body = 0,
                TouchCount_Wick = 0,
                Swing1Time = swing2.StartTime,
                Swing2Time = swing1.StartTime,
                NecklinePrice = necklinePrice,
                Status = "Pending"
            };

            // Agregar IDs de swings relacionados
            doublePattern.RelatedStructureIds.Add(swing1.Id);
            doublePattern.RelatedStructureIds.Add(swing2.Id);

            // Metadata
            doublePattern.Metadata.CreatedByDetector = "DoubleDetector";
            doublePattern.Metadata.Tags["DoubleType"] = isDoubleTop ? "Top" : "Bottom";
            doublePattern.Metadata.Tags["BarsBetweenSwings"] = (swing1.CreatedAtBarIndex - swing2.CreatedAtBarIndex).ToString();

            // Agregar al cache
            if (!_doubleCacheByTF.ContainsKey(tfMinutes))
                _doubleCacheByTF[tfMinutes] = new List<DoubleTopInfo>();

            _doubleCacheByTF[tfMinutes].Add(doublePattern);

            // Agregar al engine
            _engine.AddStructure(doublePattern);

            if (_config.EnableDebug)
            {
                string type = isDoubleTop ? "Double Top" : "Double Bottom";
                _logger.Debug($"DoubleDetector: Created {type} TF{tfMinutes} @ {(isDoubleTop ? doublePattern.High : doublePattern.Low):F5}, Neckline={necklinePrice:F5}");
            }
        }

        /// <summary>
        /// Encuentra el low más bajo entre dos índices de barra
        /// </summary>
        private double FindLowestLowBetween(int tfMinutes, int startIndex, int endIndex)
        {
            double lowestLow = double.MaxValue;

            for (int i = startIndex; i <= endIndex; i++)
            {
                double low = _provider.GetLow(tfMinutes, i);
                if (low < lowestLow)
                    lowestLow = low;
            }

            return lowestLow;
        }

        /// <summary>
        /// Encuentra el high más alto entre dos índices de barra
        /// </summary>
        private double FindHighestHighBetween(int tfMinutes, int startIndex, int endIndex)
        {
            double highestHigh = double.MinValue;

            for (int i = startIndex; i <= endIndex; i++)
            {
                double high = _provider.GetHigh(tfMinutes, i);
                if (high > highestHigh)
                    highestHigh = high;
            }

            return highestHigh;
        }

        /// <summary>
        /// Actualiza doubles existentes: confirmación o invalidación
        /// </summary>
        private void UpdateExistingDoubles(int tfMinutes, int barIndex)
        {
            if (!_doubleCacheByTF.ContainsKey(tfMinutes))
                return;

            var pendingDoubles = _doubleCacheByTF[tfMinutes]
                .Where(d => d.Status == "Pending" && d.IsActive)
                .ToList();

            foreach (var doublePattern in pendingDoubles)
            {
                bool isDoubleTop = doublePattern.Type == "DOUBLE_TOP";
                int ageInBars = barIndex - doublePattern.CreatedAtBarIndex;

                // Verificar confirmación (precio rompe neckline)
                bool confirmed = CheckConfirmation(tfMinutes, barIndex, doublePattern, isDoubleTop);

                if (confirmed)
                {
                    doublePattern.Status = "Confirmed";
                    doublePattern.IsCompleted = true;
                    doublePattern.LastUpdatedBarIndex = barIndex;
                    _engine.UpdateStructure(doublePattern);

                    if (_config.EnableDebug)
                    {
                        string type = isDoubleTop ? "Double Top" : "Double Bottom";
                        _logger.Debug($"DoubleDetector: {type} {doublePattern.Id} CONFIRMED at bar {barIndex}");
                    }
                }
                else
                {
                    // Verificar invalidación por timeout
                    // Usamos MaxBarsBetweenDouble como timeout (puede ajustarse con un parámetro específico)
                    int maxWaitBars = _config.MaxBarsBetweenDouble / 2; // Timeout = mitad de la distancia máxima

                    if (ageInBars > maxWaitBars)
                    {
                        doublePattern.Status = "Invalid";
                        doublePattern.IsActive = false;
                        doublePattern.LastUpdatedBarIndex = barIndex;
                        _engine.UpdateStructure(doublePattern);

                        if (_config.EnableDebug)
                        {
                            string type = isDoubleTop ? "Double Top" : "Double Bottom";
                            _logger.Debug($"DoubleDetector: {type} {doublePattern.Id} INVALIDATED by timeout at bar {barIndex}");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Verifica si el double ha sido confirmado por ruptura de neckline
        /// </summary>
        private bool CheckConfirmation(int tfMinutes, int barIndex, DoubleTopInfo doublePattern, bool isDoubleTop)
        {
            // Necesitamos al menos ConfirmBars_Double barras para confirmar
            if (barIndex < _config.ConfirmBars_Double)
                return false;

            // Verificar las últimas ConfirmBars_Double barras
            int barsToCheck = Math.Min(_config.ConfirmBars_Double, barIndex);

            for (int i = 0; i < barsToCheck; i++)
            {
                int checkIndex = barIndex - i;
                double closePrice = _provider.GetClose(tfMinutes, checkIndex);

                if (isDoubleTop)
                {
                    // Double Top se confirma cuando el precio cierra DEBAJO de la neckline
                    if (closePrice < doublePattern.NecklinePrice)
                        return true;
                }
                else
                {
                    // Double Bottom se confirma cuando el precio cierra ENCIMA de la neckline
                    if (closePrice > doublePattern.NecklinePrice)
                        return true;
                }
            }

            return false;
        }

        public void Dispose()
        {
            _doubleCacheByTF.Clear();
        }
    }
}

