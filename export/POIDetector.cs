// ============================================================================
// POIDetector.cs
// PinkButterfly CoreBrain - Detector de Points of Interest (POI)
// 
// Detecta zonas de confluencia donde múltiples estructuras se solapan:
// - Busca overlaps entre FVGs, Swings, Order Blocks, etc.
// - Crea POIs cuando 2+ estructuras coinciden en precio
// - Calcula CompositeScore como suma ponderada
// - Determina Bias (BuySide/SellSide/Neutral) según estructuras componentes
// - Clasifica como Premium/Discount según posición en el rango del mercado
// 
// IMPORTANTE: Este detector se ejecuta DESPUÉS de todos los demás detectores
// para poder analizar las estructuras ya creadas
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Detector de Points of Interest (POI) - Zonas de confluencia
    /// 
    /// REGLAS DE DETECCIÓN:
    /// 1. Obtener todas las estructuras activas del timeframe
    /// 2. Para cada estructura, buscar otras que se solapen (usando IntervalTree)
    /// 3. Si 2+ estructuras se solapan dentro de OverlapToleranceATR:
    ///    - Crear PointOfInterestInfo
    ///    - SourceIds = IDs de las estructuras que confluyen
    ///    - CompositeScore = weighted sum de scores
    ///    - Bias = mayoría de estructuras alcistas/bajistas
    ///    - IsPremium = comparar con rango reciente del mercado
    /// 4. Evitar duplicados: si ya existe un POI con las mismas SourceIds, actualizar
    /// 5. Purgar POIs obsoletos (cuando las estructuras fuente se invalidan)
    /// </summary>
    public class POIDetector : IDetector
    {
        private IBarDataProvider _provider;
        private EngineConfig _config;
        private ILogger _logger;
        private CoreEngine _engine;

        // Cache de POIs por TF para tracking
        private Dictionary<int, List<PointOfInterestInfo>> _poiCacheByTF = new Dictionary<int, List<PointOfInterestInfo>>();

        // Cache de estructuras procesadas para evitar re-análisis innecesario
        private Dictionary<int, int> _lastProcessedBarByTF = new Dictionary<int, int>();

        public void Initialize(IBarDataProvider provider, EngineConfig config, ILogger logger)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _logger.Info("POIDetector: Inicializado");
        }

        public void OnBarClose(int tfMinutes, int barIndex, CoreEngine engine)
        {
            _engine = engine;

            // Inicializar cache si no existe
            if (!_poiCacheByTF.ContainsKey(tfMinutes))
                _poiCacheByTF[tfMinutes] = new List<PointOfInterestInfo>();

            if (!_lastProcessedBarByTF.ContainsKey(tfMinutes))
                _lastProcessedBarByTF[tfMinutes] = -1;

            // Necesitamos al menos algunas barras para calcular ATR y rango del mercado
            if (barIndex < 20)
                return;

            try
            {
                // Ejecutar detección de POIs
                // NOTA: Este detector se ejecuta después de todos los demás,
                // por lo que las estructuras ya están creadas y disponibles
                DetectPointsOfInterest(tfMinutes, barIndex);

                // Actualizar POIs existentes (purgar obsoletos, recalcular scores)
                UpdateExistingPOIs(tfMinutes, barIndex);

                _lastProcessedBarByTF[tfMinutes] = barIndex;
            }
            catch (Exception ex)
            {
                _logger.Exception($"POIDetector: Error en TF{tfMinutes} bar{barIndex}", ex);
            }
        }

        /// <summary>
        /// Detecta nuevos Points of Interest basados en confluencias de estructuras
        /// </summary>
        private void DetectPointsOfInterest(int tfMinutes, int barIndex)
        {
            // Obtener todas las estructuras activas del timeframe
            var allStructures = _engine.GetAllStructures(tfMinutes)
                .Where(s => s.IsActive && s.Type != "POI") // Excluir POIs para evitar recursión
                .ToList();

            if (allStructures.Count < _config.MinStructuresForPOI)
                return;

            // Calcular ATR para determinar tolerancia de overlap
            double atr = _provider.GetATR(tfMinutes, 14, barIndex);
            if (atr <= 0)
                return;

            double overlapTolerance = _config.OverlapToleranceATR * atr;

            // Buscar confluencias
            var confluences = FindConfluences(allStructures, overlapTolerance);

            // Crear POIs para cada confluencia encontrada
            foreach (var confluence in confluences)
            {
                if (confluence.Count < _config.MinStructuresForPOI)
                    continue;

                // Verificar si ya existe un POI con estas estructuras fuente
                var existingPOI = FindExistingPOI(tfMinutes, confluence);
                if (existingPOI != null)
                {
                    // Ya existe, actualizar en lugar de crear nuevo
                    UpdatePOI(existingPOI, confluence, tfMinutes, barIndex, atr);
                    continue;
                }

                // Crear nuevo POI
                CreatePOI(confluence, tfMinutes, barIndex, atr);
            }
        }

        /// <summary>
        /// Encuentra confluencias de estructuras que se solapan
        /// </summary>
        private List<List<StructureBase>> FindConfluences(List<StructureBase> structures, double tolerance)
        {
            var confluences = new List<List<StructureBase>>();
            var processed = new HashSet<string>();

            foreach (var structure in structures)
            {
                if (processed.Contains(structure.Id))
                    continue;

                // Buscar TODAS las estructuras que se solapen con esta
                var overlapping = new List<StructureBase> { structure };

                foreach (var other in structures)
                {
                    if (other.Id == structure.Id)
                        continue;

                    // Verificar overlap considerando tolerancia
                    if (IsOverlapping(structure, other, tolerance))
                    {
                        overlapping.Add(other);
                    }
                }

                // Si encontramos confluencia (2+ estructuras), añadir a la lista
                if (overlapping.Count >= 2)
                {
                    // Marcar todas las estructuras de esta confluencia como procesadas
                    foreach (var s in overlapping)
                        processed.Add(s.Id);

                    confluences.Add(overlapping);
                }
            }

            return confluences;
        }

        /// <summary>
        /// Verifica si dos estructuras se solapan considerando una tolerancia
        /// </summary>
        private bool IsOverlapping(StructureBase s1, StructureBase s2, double tolerance)
        {
            // Expandir rangos con tolerancia
            double s1Low = s1.Low - tolerance;
            double s1High = s1.High + tolerance;
            double s2Low = s2.Low - tolerance;
            double s2High = s2.High + tolerance;

            // Verificar overlap
            return !(s1High < s2Low || s2High < s1Low);
        }

        /// <summary>
        /// Busca un POI existente que tenga las mismas estructuras fuente
        /// </summary>
        private PointOfInterestInfo FindExistingPOI(int tfMinutes, List<StructureBase> confluence)
        {
            var sourceIds = confluence.Select(s => s.Id).OrderBy(id => id).ToList();

            // Buscar en el motor (fuente de verdad)
            var allPOIs = _engine.GetPOIs(tfMinutes);

            foreach (var poi in allPOIs)
            {
                var poiSourceIds = poi.SourceIds.OrderBy(id => id).ToList();

                // Comparar listas de IDs
                if (sourceIds.Count == poiSourceIds.Count &&
                    sourceIds.SequenceEqual(poiSourceIds))
                {
                    return poi;
                }
            }

            return null;
        }

        /// <summary>
        /// Snap price to valid tick grid (V5.7e)
        /// </summary>
        private double SnapToTick(double price)
        {
            double tick = _provider.GetTickSize();
            return Math.Round(price / tick) * tick;
        }

        /// <summary>
        /// Crea un nuevo POI a partir de una confluencia de estructuras
        /// </summary>
        private void CreatePOI(List<StructureBase> confluence, int tfMinutes, int barIndex, double atr)
        {
            // 1) Calcular intersección de todas las estructuras
            double poiLow = confluence.Max(s => s.Low);
            double poiHigh = confluence.Min(s => s.High);

            // 2) Si no hay intersección real, usar el rango promedio
            if (poiLow > poiHigh)
            {
                double avgLow = confluence.Average(s => s.Low);
                double avgHigh = confluence.Average(s => s.High);
                poiLow = avgLow;
                poiHigh = avgHigh;
            }

            // 3) Snap SIEMPRE al grid de ticks (V5.7e: evitar precios inexistentes)
            poiLow = SnapToTick(poiLow);
            poiHigh = SnapToTick(poiHigh);

            // Crear POI
            var poi = new PointOfInterestInfo
            {
                TF = tfMinutes,
                StartTime = confluence.Min(s => s.StartTime),
                EndTime = confluence.Max(s => s.EndTime),
                High = poiHigh,
                Low = poiLow,
                CreatedAtBarIndex = barIndex,
                LastUpdatedBarIndex = barIndex,
                IsActive = true,
                IsCompleted = true,
                SourceIds = confluence.Select(s => s.Id).ToList()
            };

            // Calcular CompositeScore (weighted sum)
            poi.CompositeScore = CalculateCompositeScore(confluence);

            // Determinar Bias
            poi.Bias = DetermineBias(confluence);

            // Determinar si es Premium o Discount
            poi.IsPremium = DetermineIfPremium(poi, tfMinutes, barIndex, atr);

            // Metadata
            poi.Metadata.CreatedByDetector = "POIDetector";
            poi.Metadata.Tags["SourceCount"] = confluence.Count.ToString();
            poi.Metadata.Tags["StructureTypes"] = string.Join(",", confluence.Select(s => s.Type).Distinct());

            // Añadir al motor
            _engine.AddStructure(poi);

            // Añadir a cache
            _poiCacheByTF[tfMinutes].Add(poi);

            if (_config.EnableDebug)
            {
                _logger.Debug($"POIDetector: POI creado en TF{tfMinutes} bar{barIndex} - " +
                             $"Sources: {confluence.Count}, Bias: {poi.Bias}, " +
                             $"Premium: {poi.IsPremium}, Score: {poi.CompositeScore:F3}");
            }
        }

        /// <summary>
        /// Actualiza un POI existente con nueva información
        /// </summary>
        private void UpdatePOI(PointOfInterestInfo poi, List<StructureBase> confluence, int tfMinutes, int barIndex, double atr)
        {
            // Recalcular CompositeScore
            double newScore = CalculateCompositeScore(confluence);

            // Solo actualizar si el score cambió (threshold muy bajo para detectar cambios sutiles)
            if (Math.Abs(newScore - poi.CompositeScore) > 0.001)
            {
                poi.CompositeScore = newScore;
                poi.LastUpdatedBarIndex = barIndex;

                // Recalcular Bias y Premium
                poi.Bias = DetermineBias(confluence);
                poi.IsPremium = DetermineIfPremium(poi, tfMinutes, barIndex, atr);

                // Notificar al motor del cambio (verificar existencia primero)
                if (_engine.GetStructureById(poi.Id) != null)
                {
                    _engine.UpdateStructure(poi);
                }
                else
                {
                    // POI fue purgado, no hacer nada (será removido en la siguiente iteración)
                    return;
                }

                if (_config.EnableDebug)
                {
                    _logger.Debug($"POIDetector: POI actualizado {poi.Id} - Score: {poi.CompositeScore:F3}");
                }
            }
        }

        /// <summary>
        /// Calcula el score compuesto de un POI como weighted sum de las estructuras fuente
        /// </summary>
        private double CalculateCompositeScore(List<StructureBase> confluence)
        {
            if (confluence.Count == 0)
                return 0.0;

            // Promedio simple de scores de las estructuras
            double avgScore = confluence.Average(s => s.Score);

            // Aplicar bonus por confluencia
            double confluenceBonus = Math.Min((confluence.Count - 1) * _config.POI_ConfluenceBonus, _config.POI_MaxConfluenceBonus);

            // Score final = promedio + bonus
            return Math.Min(avgScore * (1.0 + confluenceBonus), 1.0);
        }

        /// <summary>
        /// Determina el bias del POI basado en las estructuras componentes
        /// </summary>
        private string DetermineBias(List<StructureBase> confluence)
        {
            int bullishCount = 0;
            int bearishCount = 0;

            foreach (var structure in confluence)
            {
                // Determinar bias de cada estructura
                string structureBias = GetStructureBias(structure);

                if (structureBias == "Bullish")
                    bullishCount++;
                else if (structureBias == "Bearish")
                    bearishCount++;
            }

            // Determinar bias por mayoría
            if (bullishCount > bearishCount * 1.5) // 60% threshold
                return "BuySide";
            else if (bearishCount > bullishCount * 1.5)
                return "SellSide";
            else
                return "Neutral";
        }

        /// <summary>
        /// Obtiene el bias de una estructura individual
        /// </summary>
        private string GetStructureBias(StructureBase structure)
        {
            switch (structure.Type)
            {
                case "FVG":
                    var fvg = structure as FVGInfo;
                    return fvg?.Direction ?? "Neutral";

                case "OB":
                    var ob = structure as OrderBlockInfo;
                    return ob?.Direction ?? "Neutral";

                case "SWING":
                    var swing = structure as SwingInfo;
                    return swing != null ? (swing.IsHigh ? "Bearish" : "Bullish") : "Neutral";

                case "DOUBLE_TOP":
                case "DOUBLE_BOTTOM":
                    var dbl = structure as DoubleTopInfo;
                    return dbl != null ? (dbl.Type == "DOUBLE_TOP" ? "Bearish" : "Bullish") : "Neutral";

                case "BOS":
                case "CHoCH":
                    var brk = structure as StructureBreakInfo;
                    return brk?.Direction ?? "Neutral";

                default:
                    return "Neutral";
            }
        }

        /// <summary>
        /// Determina si el POI está en zona Premium o Discount
        /// </summary>
        private bool DetermineIfPremium(PointOfInterestInfo poi, int tfMinutes, int barIndex, double atr)
        {
            // Calcular rango reciente del mercado (últimas N barras)
            int lookbackBars = _config.POI_PremiumLookbackBars;
            double marketHigh = double.MinValue;
            double marketLow = double.MaxValue;

            for (int i = Math.Max(0, barIndex - lookbackBars); i <= barIndex; i++)
            {
                double high = _provider.GetHigh(tfMinutes, i);
                double low = _provider.GetLow(tfMinutes, i);

                if (high > marketHigh)
                    marketHigh = high;
                if (low < marketLow)
                    marketLow = low;
            }

            // Calcular nivel del POI en el rango
            double poiCenter = poi.CenterPrice;
            double marketRange = marketHigh - marketLow;

            if (marketRange == 0)
                return false;

            double poiLevelInRange = (poiCenter - marketLow) / marketRange;

            // Premium si está por encima del threshold (default: 0.618 = 61.8%)
            return poiLevelInRange >= _config.POI_PremiumThreshold;
        }

        /// <summary>
        /// Actualiza POIs existentes y purga obsoletos
        /// </summary>
        private void UpdateExistingPOIs(int tfMinutes, int barIndex)
        {
            var poisToRemove = new List<PointOfInterestInfo>();

            foreach (var poi in _poiCacheByTF[tfMinutes])
            {
                if (!poi.IsActive)
                    continue;

                // Verificar si las estructuras fuente siguen activas
                bool allSourcesActive = true;
                var sourceStructures = new List<StructureBase>();

                foreach (var sourceId in poi.SourceIds)
                {
                    var source = _engine.GetStructureById(sourceId);
                    if (source == null || !source.IsActive)
                    {
                        allSourcesActive = false;
                        break;
                    }
                    sourceStructures.Add(source);
                }

                // Si alguna estructura fuente se invalidó, invalidar el POI
                if (!allSourcesActive)
                {
                    poi.IsActive = false;
                    
                    // Verificar existencia antes de actualizar
                    if (_engine.GetStructureById(poi.Id) != null)
                    {
                        _engine.UpdateStructure(poi);
                    }
                    
                    poisToRemove.Add(poi);

                    if (_config.EnableDebug)
                    {
                        _logger.Debug($"POIDetector: POI {poi.Id} invalidado (estructuras fuente inactivas)");
                    }
                    continue;
                }

                // Recalcular score y premium status
                double atr = _provider.GetATR(tfMinutes, 14, barIndex);
                if (atr > 0)
                {
                    poi.CompositeScore = CalculateCompositeScore(sourceStructures);
                    poi.IsPremium = DetermineIfPremium(poi, tfMinutes, barIndex, atr);
                    poi.LastUpdatedBarIndex = barIndex;
                    
                    // Verificar existencia antes de actualizar
                    if (_engine.GetStructureById(poi.Id) != null)
                    {
                        _engine.UpdateStructure(poi);
                    }
                }
            }

            // Purgar POIs invalidados del cache
            foreach (var poi in poisToRemove)
            {
                _poiCacheByTF[tfMinutes].Remove(poi);
            }
        }

        public void Dispose()
        {
            _poiCacheByTF?.Clear();
            _lastProcessedBarByTF?.Clear();
            _logger?.Info("POIDetector: Disposed");
        }
    }
}

