// ============================================================================
// ContextManager.cs
// PinkButterfly CoreBrain - Componente 1 del DFM
// 
// Responsabilidades:
// - Construir el DecisionSummary (CurrentPrice, ATRByTF, TotalStructures, etc.)
// - Calcular el GlobalBias consultando CoreEngine.GetCurrentMarketBias()
// - Calcular GlobalBiasStrength basado en la proporción de BOS/CHoCH recientes
// - Calcular MarketClarity basado en estructuras activas y recientes de alta jerarquía
// - Calcular MarketVolatilityNormalized comparando ATR actual vs ATR de largo plazo
//
// Fórmulas:
//   MarketClarity = (min(ActiveStructures/MinStructures, 1.0) + min(RecentStructures/MinStructures, 1.0)) / 2.0
//   VolatilityNormalized = min(ATR(Actual) / ATR(LargoPlazo), 2.0) / 2.0
//
// Filtro de estructuras de alta jerarquía: FVG, OB, POI, BOS, CHoCH, LG
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// ContextManager: Primer componente del pipeline DFM
    /// Construye el snapshot inicial del mercado con métricas globales
    /// </summary>
    public class ContextManager : IDecisionComponent
    {
        private EngineConfig _config;
        private ILogger _logger;

        public string ComponentName => "ContextManager";

        public void Initialize(EngineConfig config, ILogger logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug("[ContextManager] Inicializado");
        }

        public void Process(DecisionSnapshot snapshot, IBarDataProvider barData, CoreEngine coreEngine, int currentBar, double accountSize)
        {
            if (snapshot == null)
                throw new ArgumentNullException(nameof(snapshot));
            if (barData == null)
                throw new ArgumentNullException(nameof(barData));
            if (coreEngine == null)
                throw new ArgumentNullException(nameof(coreEngine));

            _logger.Debug("[ContextManager] Construyendo contexto del mercado...");

            // 1. Construir DecisionSummary
            BuildDecisionSummary(snapshot, barData, coreEngine, currentBar);

            // 2. Calcular GlobalBias y GlobalBiasStrength
            CalculateGlobalBias(snapshot, coreEngine, currentBar);

            // 3. Calcular MarketClarity
            CalculateMarketClarity(snapshot, coreEngine, currentBar);

            // 4. Calcular MarketVolatilityNormalized
            CalculateMarketVolatility(snapshot, barData, currentBar);

            _logger.Debug(string.Format(
                "[ContextManager] Contexto completado - Bias: {0} ({1:F2}), Clarity: {2:F2}, Volatility: {3:F2}",
                snapshot.GlobalBias, snapshot.GlobalBiasStrength, snapshot.MarketClarity, snapshot.MarketVolatilityNormalized
            ));
        }

        /// <summary>
        /// Construye el DecisionSummary con métricas agregadas del mercado
        /// </summary>
        private void BuildDecisionSummary(DecisionSnapshot snapshot, IBarDataProvider barData, CoreEngine coreEngine, int currentBar)
        {
            var summary = new DecisionSummary();

            // CurrentPrice: Precio de cierre de la barra actual en el TF principal
            int primaryTF = _config.TimeframesToUse.OrderByDescending(tf => tf).FirstOrDefault();
            if (primaryTF == 0)
                primaryTF = 60; // Fallback a 1H

            summary.CurrentPrice = barData.GetClose(primaryTF, currentBar);

            // ATRByTF: Calcular ATR para cada timeframe configurado
            summary.ATRByTF = new Dictionary<int, double>();
            foreach (int tf in _config.TimeframesToUse)
            {
                double atr = barData.GetATR(tf, currentBar, 14);
                summary.ATRByTF[tf] = atr;
            }

            // TotalStructures y TotalActiveStructures (obtener de todos los TFs)
            var allStructures = new List<StructureBase>();
            foreach (int tf in _config.TimeframesToUse)
            {
                allStructures.AddRange(coreEngine.GetAllStructures(tf));
            }
            
            summary.TotalStructures = allStructures.Count;
            summary.TotalActiveStructures = allStructures.Count(s => s.IsActive);

            // StructuresByType: Contar estructuras por tipo
            summary.StructuresByType = new Dictionary<string, int>();
            foreach (var structure in allStructures)
            {
                string type = structure.GetType().Name;
                if (!summary.StructuresByType.ContainsKey(type))
                    summary.StructuresByType[type] = 0;
                summary.StructuresByType[type]++;
            }

            snapshot.Summary = summary;

            _logger.Debug(string.Format(
                "[ContextManager] Summary - Price: {0:F2}, Structures: {1} (Active: {2})",
                summary.CurrentPrice, summary.TotalStructures, summary.TotalActiveStructures
            ));
        }

        /// <summary>
        /// CALIBRACIÓN V5 (FINAL - ÓPTIMA): Calcula el GlobalBias y GlobalBiasStrength basado en BOS/CHoCH recientes
        /// Este método original demostró ser el más efectivo (PF 2.00, Win Rate 42.9%)
        /// </summary>
        private void CalculateGlobalBias(DecisionSnapshot snapshot, CoreEngine coreEngine, int currentBar)
        {
            // Obtener el bias del CoreEngine (propiedad calculada por BOSDetector)
            snapshot.GlobalBias = coreEngine.CurrentMarketBias; // "Bullish", "Bearish", "Neutral"

            // GlobalBiasStrength: Proporción de breaks en la dirección del bias
            // Rango: 0.0 (neutral/mixto) a 1.0 (todos los breaks en la misma dirección)
            // Obtener BOS/CHoCH de todos los TFs
            var recentBreaks = new List<StructureBase>();
            foreach (int tf in _config.TimeframesToUse)
            {
                var structures = coreEngine.GetAllStructures(tf);
                recentBreaks.AddRange(structures.Where(s =>
                    (s.GetType().Name == "BOS" || s.GetType().Name == "CHoCH") &&
                    (currentBar - s.CreatedAtBarIndex) <= _config.MaxRecentBreaksForBias
                ));
            }
            
            recentBreaks = recentBreaks
                .OrderByDescending(s => s.CreatedAtBarIndex)
                .Take(_config.MaxRecentBreaksForBias)
                .ToList();

            if (recentBreaks.Count == 0)
            {
                snapshot.GlobalBiasStrength = 0.0;
                _logger.Debug("[ContextManager] No hay breaks recientes, BiasStrength = 0.0");
                return;
            }

            // Contar breaks bullish vs bearish
            int bullishBreaks = recentBreaks.Count(s => GetStructureDirection(s) == "Bullish");
            int bearishBreaks = recentBreaks.Count(s => GetStructureDirection(s) == "Bearish");
            int totalBreaks = recentBreaks.Count;

            // Strength = proporción del tipo dominante
            if (snapshot.GlobalBias == "Bullish")
                snapshot.GlobalBiasStrength = (double)bullishBreaks / totalBreaks;
            else if (snapshot.GlobalBias == "Bearish")
                snapshot.GlobalBiasStrength = (double)bearishBreaks / totalBreaks;
            else
                snapshot.GlobalBiasStrength = 0.5; // Neutral = 50%

            _logger.Debug(string.Format(
                "[ContextManager] GlobalBias: {0}, Strength: {1:F2} (Bullish: {2}, Bearish: {3})",
                snapshot.GlobalBias, snapshot.GlobalBiasStrength, bullishBreaks, bearishBreaks
            ));
        }

        /// <summary>
        /// Calcula MarketClarity basado en estructuras activas y recientes de alta jerarquía
        /// Fórmula: (min(Active/Min, 1.0) + min(Recent/Min, 1.0)) / 2.0
        /// Filtro: Solo FVG, OB, POI, BOS, CHoCH, LG
        /// </summary>
        private void CalculateMarketClarity(DecisionSnapshot snapshot, CoreEngine coreEngine, int currentBar)
        {
            // Tipos de estructura de alta jerarquía
            var highHierarchyTypes = new HashSet<string> { "FVG", "OB", "POI", "BOS", "CHoCH", "LG" };

            // Obtener todas las estructuras de todos los TFs
            var allStructures = new List<StructureBase>();
            foreach (int tf in _config.TimeframesToUse)
            {
                allStructures.AddRange(coreEngine.GetAllStructures(tf));
            }

            // Filtrar solo estructuras de alta jerarquía
            var relevantStructures = allStructures.Where(s =>
            {
                string type = s.GetType().Name;
                return highHierarchyTypes.Contains(type);
            }).ToList();

            // Contar estructuras activas
            int activeStructures = relevantStructures.Count(s => s.IsActive);

            // Contar estructuras recientes (edad <= MarketClarity_MaxAge)
            int recentStructures = relevantStructures.Count(s =>
                (currentBar - s.CreatedAtBarIndex) <= _config.MarketClarity_MaxAge
            );

            // Calcular factores
            double minStructures = _config.MarketClarity_MinStructures;
            double structuresFactor = Math.Min(activeStructures / minStructures, 1.0);
            double recentFactor = Math.Min(recentStructures / minStructures, 1.0);

            // MarketClarity = promedio de ambos factores
            snapshot.MarketClarity = (structuresFactor + recentFactor) / 2.0;

            _logger.Debug(string.Format(
                "[ContextManager] MarketClarity: {0:F2} (Active: {1}/{2:F0}, Recent: {3}/{4:F0})",
                snapshot.MarketClarity, activeStructures, minStructures, recentStructures, minStructures
            ));
        }

        /// <summary>
        /// Calcula MarketVolatilityNormalized comparando ATR actual vs ATR de largo plazo
        /// Fórmula: min(ATR(Actual) / ATR(LargoPlazo), 2.0) / 2.0
        /// Rango: 0.0 (baja volatilidad) a 1.0 (alta volatilidad)
        /// </summary>
        private void CalculateMarketVolatility(DecisionSnapshot snapshot, IBarDataProvider barData, int currentBar)
        {
            // Usar el timeframe principal (más alto) para calcular volatilidad
            int primaryTF = _config.TimeframesToUse.OrderByDescending(tf => tf).FirstOrDefault();
            if (primaryTF == 0)
                primaryTF = 60; // Fallback a 1H

            // ATR actual (14 períodos)
            double atrActual = barData.GetATR(primaryTF, currentBar, 14);

            // ATR de largo plazo (200 períodos)
            double atrLongTerm = barData.GetATR(primaryTF, currentBar, 200);

            // Evitar división por cero
            if (atrLongTerm <= 0)
            {
                snapshot.MarketVolatilityNormalized = 0.5; // Valor neutral
                _logger.Warning("[ContextManager] ATR de largo plazo es 0, usando volatilidad neutral (0.5)");
                return;
            }

            // Calcular ratio y normalizar
            double ratio = atrActual / atrLongTerm;
            snapshot.MarketVolatilityNormalized = Math.Min(ratio, 2.0) / 2.0;

            _logger.Debug(string.Format(
                "[ContextManager] Volatility: {0:F2} (ATR Actual: {1:F2}, ATR LongTerm: {2:F2}, Ratio: {3:F2})",
                snapshot.MarketVolatilityNormalized, atrActual, atrLongTerm, ratio
            ));
        }

        /// <summary>
        /// Obtiene la dirección de una estructura de forma segura (cast a tipos específicos)
        /// </summary>
        private string GetStructureDirection(StructureBase structure)
        {
            if (structure is FVGInfo fvg)
                return fvg.Direction;
            if (structure is StructureBreakInfo sb)
                return sb.Direction;
            if (structure is OrderBlockInfo ob)
                return ob.Direction;
            if (structure is LiquidityGrabInfo lg)
                return lg.DirectionalBias; // LG usa DirectionalBias, no Direction
            
            return "Neutral";
        }
    }
}

