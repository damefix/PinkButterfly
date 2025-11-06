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
        private IBarDataProvider _barData;
        
        // V6.0i: Estado del régimen de volatilidad con histéresis
        private string _currentRegime = "Normal"; // Inicial: Normal
        private DateTime _lastRegimeChange = DateTime.MinValue;

        public string ComponentName => "ContextManager";

        public void Initialize(EngineConfig config, ILogger logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug("[ContextManager] Inicializado");
        }

        public void Process(DecisionSnapshot snapshot, IBarDataProvider barData, CoreEngine coreEngine, int currentBar, int timeframeMinutes, double accountSize)
        {
            if (snapshot == null)
                throw new ArgumentNullException(nameof(snapshot));
            if (barData == null)
                throw new ArgumentNullException(nameof(barData));
            if (coreEngine == null)
                throw new ArgumentNullException(nameof(coreEngine));

            _barData = barData; // Guardar para uso en CalculateGlobalBias

            _logger.Debug("[ContextManager] Construyendo contexto del mercado...");

            // 1. Construir DecisionSummary
            BuildDecisionSummary(snapshot, barData, coreEngine, currentBar);
            
            // 2. Detectar régimen de volatilidad con histéresis (V6.0i) - ANTES del bias
            DetectRegime(snapshot, barData, currentBar);

            // 3. Calcular GlobalBias y GlobalBiasStrength (usa régimen para threshold)
            CalculateGlobalBias(snapshot, coreEngine, currentBar);

            // 4. Calcular MarketClarity
            CalculateMarketClarity(snapshot, coreEngine, currentBar);

            // 5. Calcular MarketVolatilityNormalized
            CalculateMarketVolatility(snapshot, barData, currentBar);

            _logger.Debug(string.Format(
                "[ContextManager] Contexto completado - Regime: {0}, Bias: {1} ({2:F2}), Clarity: {3:F2}, Volatility: {4:F2}",
                snapshot.MarketRegime, snapshot.GlobalBias, snapshot.GlobalBiasStrength, snapshot.MarketClarity, snapshot.MarketVolatilityNormalized
            ));
        }

        /// <summary>
        /// Construye el DecisionSummary con métricas agregadas del mercado
        /// </summary>
        private void BuildDecisionSummary(DecisionSnapshot snapshot, IBarDataProvider barData, CoreEngine coreEngine, int currentBar)
        {
            var summary = new DecisionSummary();

            // CurrentPrice: usar el precio del TF MÁS ALTO, alineado por tiempo con el TF de decisión
            int decisionTF = _config.DecisionTimeframeMinutes;
            DateTime analysisTime = barData.GetBarTime(decisionTF, currentBar);
            int primaryTF = _config.TimeframesToUse.OrderByDescending(tf => tf).FirstOrDefault();
            if (primaryTF == 0)
                primaryTF = 60; // Fallback a 1H
            int idxPrim = barData.GetBarIndexFromTime(primaryTF, analysisTime);
            if (idxPrim < 0)
            {
                // Fallback: usar DecisionTF (siempre disponible en este ciclo)
                idxPrim = barData.GetBarIndexFromTime(decisionTF, analysisTime);
                if (idxPrim < 0)
                {
                    _logger.Warning($"[CTX_NO_DATA] Sin datos para CurrentPrice en TF={primaryTF} ni {decisionTF} para time={analysisTime:yyyy-MM-dd HH:mm}");
                    summary.CurrentPrice = 0.0;
                    snapshot.Summary = summary;
                    return; // no continuar sin datos
                }
                summary.CurrentPrice = barData.GetClose(decisionTF, idxPrim);
                _logger.Info($"[CTX_FALLBACK] CurrentPrice desde TF={decisionTF} (primaryTF={primaryTF} no disponible)");
            }
            else
            {
                summary.CurrentPrice = barData.GetClose(primaryTF, idxPrim);
            }

            // ATRByTF: Calcular ATR para cada timeframe configurado
            summary.ATRByTF = new Dictionary<int, double>();
            // Alinear por tiempo con el TF de decisión para obtener índices correctos por TF
            foreach (int tf in _config.TimeframesToUse)
            {
                int idx = barData.GetBarIndexFromTime(tf, analysisTime);
                // Firma correcta: GetATR(tfMinutes, period, barIndex)
                double atr = idx >= 0 ? barData.GetATR(tf, 14, idx) : 0.0;
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
        /// CALIBRACIÓN V5.5: Calcula el GlobalBias y GlobalBiasStrength usando promedio de precios
        /// Si Precio > Promedio(200 barras) => Bullish con Strength 1.0
        /// Si Precio < Promedio(200 barras) => Bearish con Strength 1.0
        /// 
        /// FIX V5.5: Usar TF de 1H (60m) en lugar de Daily para cálculo más sensible
        /// 200 barras de 1H = ~8 días (vs 200 días con Daily)
        /// </summary>
        private void CalculateGlobalBias(DecisionSnapshot snapshot, CoreEngine coreEngine, int currentBar)
        {
            // V6.0g: BIAS COMPUESTO MULTI-SEÑAL para mayor sensibilidad intradía
            // Combina 4 componentes con pesos específicos (suma ponderada = score [-1, +1])
            // Score > 0.5: Bullish | Score < -0.5: Bearish | Resto: Neutral
            
            int primaryTF = 60; // 1H para EMA y regresión
            int decisionTF = _config.DecisionTimeframeMinutes;
            DateTime analysisTime = _barData.GetBarTime(decisionTF, currentBar);
            int idx60 = _barData.GetBarIndexFromTime(primaryTF, analysisTime);
            
            if (idx60 < 0)
            {
                _logger.Warning($"[CTX_NO_DATA] Sin índice 60m para bias compuesto. time={analysisTime:yyyy-MM-dd HH:mm}");
                snapshot.GlobalBias = "Neutral";
                snapshot.GlobalBiasStrength = 0.0;
                return;
            }

            // Componente 1: EMA20@60m Slope (30%) - Tendencia inmediata (20h)
            double ema20Score = CalculateEMASlope(idx60, primaryTF, 20);
            
            // Componente 2: EMA50@60m Cross (25%) - Tendencia media (50h)
            double ema50Score = CalculateEMACross(idx60, primaryTF, 50);
            
            // Componente 3: BOS/CHoCH Count (25%) - Cambios de estructura recientes
            double bosScore = CalculateBOSScore(coreEngine, currentBar);
            
            // Componente 4: Regresión Lineal 24h (20%) - Dirección últimas 24h
            double regressionScore = CalculateRegression24h(idx60, primaryTF);
            
            // SCORE COMPUESTO (rango [-1, +1])
            double compositeScore = (ema20Score * 0.30) + (ema50Score * 0.25) + 
                                    (bosScore * 0.25) + (regressionScore * 0.20);
            
            // V6.0i: Threshold adaptativo según régimen
            // Normal: 0.3 (V6.0h) | HighVol: 0.35 (más estricto para evitar contras en picos)
            double biasThreshold = (snapshot.MarketRegime == "HighVol") 
                ? _config.BiasThreshold_HighVol 
                : 0.3;
            
            // Determinar bias final
            if (compositeScore > biasThreshold)
            {
                snapshot.GlobalBias = "Bullish";
                snapshot.GlobalBiasStrength = Math.Min(compositeScore, 1.0);
            }
            else if (compositeScore < -biasThreshold)
            {
                snapshot.GlobalBias = "Bearish";
                snapshot.GlobalBiasStrength = Math.Min(Math.Abs(compositeScore), 1.0);
            }
            else
            {
                snapshot.GlobalBias = "Neutral";
                snapshot.GlobalBiasStrength = Math.Abs(compositeScore);
            }
            
            _logger.Info(string.Format(
                "[DIAGNOSTICO][Context] V6.0i Regime={0} BiasComposite={1} Score={2:F2} Threshold={3:F2} EMA20={4:F2} EMA50={5:F2} BOS={6:F2} Reg24h={7:F2}",
                snapshot.MarketRegime, snapshot.GlobalBias, compositeScore, biasThreshold, ema20Score, ema50Score, bosScore, regressionScore
            ));
        }
        
        // ========================================================================
        // COMPONENTES DE BIAS COMPUESTO (V6.0g)
        // ========================================================================
        
        /// <summary>
        /// Calcula slope de EMA20@60m comparando EMA actual vs EMA hace 5 barras.
        /// Retorna: +1 (subida fuerte), 0 (plano), -1 (bajada fuerte)
        /// </summary>
        private double CalculateEMASlope(int idx60, int tf, int period)
        {
            if (idx60 < period) return 0.0;
            
            double emaCurrent = CalculateEMA(idx60, tf, period);
            double emaPast = CalculateEMA(idx60 - 5, tf, period);
            
            if (emaCurrent == 0 || emaPast == 0) return 0.0;
            
            double slope = (emaCurrent - emaPast) / emaPast;
            return Math.Max(-1.0, Math.Min(1.0, slope * 100)); // Escalar y limitar
        }
        
        /// <summary>
        /// Detecta cross de precio con EMA50@60m.
        /// Retorna: +1 (precio > EMA50), -1 (precio < EMA50)
        /// </summary>
        private double CalculateEMACross(int idx60, int tf, int period)
        {
            if (idx60 < period) return 0.0;
            
            double currentPrice = _barData.GetClose(tf, idx60);
            double ema50 = CalculateEMA(idx60, tf, period);
            
            if (ema50 == 0) return 0.0;
            
            return currentPrice > ema50 ? 1.0 : -1.0;
        }
        
        /// <summary>
        /// Cuenta BOS/CHoCH recientes (últimas 50 barras de decisión TF) con decaimiento.
        /// BOS Bull > BOS Bear → +1 | BOS Bear > BOS Bull → -1
        /// </summary>
        private double CalculateBOSScore(CoreEngine coreEngine, int currentBar)
        {
            int bullCount = 0;
            int bearCount = 0;
            int lookbackBars = 50;
            
            foreach (int tf in _config.TimeframesToUse)
            {
                var allStructures = coreEngine.GetAllStructures(tf);
                var bosStructures = allStructures.Where(s => 
                    (s.GetType().Name == "BOS" || s.GetType().Name == "CHoCH") &&
                    (currentBar - s.CreatedAtBarIndex) <= lookbackBars
                );
                
                foreach (var bos in bosStructures)
                {
                    // Asumimos que BOS tiene una propiedad "Direction" o similar
                    // Si no existe, usar heurística basada en high/low
                    if (bos.High > bos.Low * 1.01) bullCount++;
                    else if (bos.Low < bos.High * 0.99) bearCount++;
                }
            }
            
            if (bullCount + bearCount == 0) return 0.0;
            
            double ratio = (double)(bullCount - bearCount) / (bullCount + bearCount);
            return Math.Max(-1.0, Math.Min(1.0, ratio));
        }
        
        /// <summary>
        /// Regresión lineal simple sobre últimas 24 barras (24h en TF60).
        /// Pendiente positiva → +1 | Pendiente negativa → -1
        /// </summary>
        private double CalculateRegression24h(int idx60, int tf)
        {
            int lookback = 24; // 24 barras de 60m = 24 horas
            if (idx60 < lookback) return 0.0;
            
            double sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;
            int n = lookback;
            
            for (int i = 0; i < n; i++)
            {
                double x = i;
                double y = _barData.GetClose(tf, idx60 - i);
                sumX += x;
                sumY += y;
                sumXY += x * y;
                sumX2 += x * x;
            }
            
            double slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
            
            // Normalizar slope (típicamente muy pequeño) a rango [-1, +1]
            return Math.Max(-1.0, Math.Min(1.0, slope * 1000));
        }
        
        /// <summary>
        /// Calcula EMA (Exponential Moving Average) simple.
        /// </summary>
        private double CalculateEMA(int idx, int tf, int period)
        {
            if (idx < period) return 0.0;
            
            double multiplier = 2.0 / (period + 1);
            double ema = _barData.GetClose(tf, idx - period + 1); // Seed con primera barra
            
            for (int i = period - 2; i >= 0; i--)
            {
                double close = _barData.GetClose(tf, idx - i);
                ema = (close - ema) * multiplier + ema;
            }
            
            return ema;
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

            // Alinear índice del TF primario al tiempo del TF de decisión
            int decisionTF_forVol = _config.DecisionTimeframeMinutes;
            DateTime analysisTime_forVol = _barData.GetBarTime(decisionTF_forVol, currentBar);
            int idxPrim = _barData.GetBarIndexFromTime(primaryTF, analysisTime_forVol);
            if (idxPrim < 0)
            {
                _logger.Warning($"[CTX_NO_DATA] Sin índice primario para volatilidad en time={analysisTime_forVol:yyyy-MM-dd HH:mm}. Vol=0.5");
                snapshot.MarketVolatilityNormalized = 0.5;
                return;
            }

            // ATR actual (14 períodos) - firma correcta
            double atrActual = barData.GetATR(primaryTF, 14, idxPrim);

            // ATR de largo plazo (200 períodos) - firma correcta
            double atrLongTerm = barData.GetATR(primaryTF, 200, idxPrim);

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
        
        /// <summary>
        /// V6.0i: Detecta régimen de volatilidad con histéresis para evitar flip-flop.
        /// Usa ATR60 como indicador base.
        /// - Entrar a HighVol si ATR60 > EnterThreshold (P70 ~17.0)
        /// - Salir de HighVol si ATR60 < ExitThreshold (P60 ~13.0)
        /// </summary>
        private void DetectRegime(DecisionSnapshot snapshot, IBarDataProvider barData, int currentBar)
        {
            if (!_config.UseAdaptiveRegime)
            {
                snapshot.MarketRegime = "Normal";
                return;
            }
            
            // Calcular ATR60 (base para detección de régimen)
            int primaryTF = 60;
            int decisionTF = _config.DecisionTimeframeMinutes;
            DateTime analysisTime = barData.GetBarTime(decisionTF, currentBar);
            int idx60 = barData.GetBarIndexFromTime(primaryTF, analysisTime);
            
            if (idx60 < 0)
            {
                _logger.Warning($"[REGIME] Sin índice 60m para detección de régimen. time={analysisTime:yyyy-MM-dd HH:mm}");
                snapshot.MarketRegime = _currentRegime; // Mantener régimen anterior
                return;
            }
            
            double atr60 = barData.GetATR(primaryTF, 14, idx60);
            
            if (atr60 <= 0)
            {
                _logger.Warning($"[REGIME] ATR60=0, usando régimen previo: {_currentRegime}");
                snapshot.MarketRegime = _currentRegime;
                return;
            }
            
            // Lógica de histéresis
            string prevRegime = _currentRegime;
            
            if (_currentRegime == "Normal")
            {
                // Estamos en Normal → solo cambiar a HighVol si ATR60 > EnterThreshold
                if (atr60 > _config.HighVolatilityATR_EnterThreshold)
                {
                    _currentRegime = "HighVol";
                    _lastRegimeChange = analysisTime;
                    _logger.Info(string.Format(
                        "[REGIME][TRANSITION] Normal → HighVol | ATR60={0:F2} > Threshold={1:F2} | Time={2:yyyy-MM-dd HH:mm}",
                        atr60, _config.HighVolatilityATR_EnterThreshold, analysisTime));
                }
            }
            else if (_currentRegime == "HighVol")
            {
                // Estamos en HighVol → solo cambiar a Normal si ATR60 < ExitThreshold
                if (atr60 < _config.HighVolatilityATR_ExitThreshold)
                {
                    _currentRegime = "Normal";
                    _lastRegimeChange = analysisTime;
                    _logger.Info(string.Format(
                        "[REGIME][TRANSITION] HighVol → Normal | ATR60={0:F2} < Threshold={1:F2} | Time={2:yyyy-MM-dd HH:mm}",
                        atr60, _config.HighVolatilityATR_ExitThreshold, analysisTime));
                }
            }
            
            // Actualizar snapshot
            snapshot.MarketRegime = _currentRegime;
            
            // Log periódico (cada 100 barras o cambio de régimen)
            if (prevRegime != _currentRegime || currentBar % 100 == 0)
            {
                _logger.Info(string.Format(
                    "[DIAGNOSTICO][Context] V6.0i Regime={0} ATR60={1:F2} Thresholds=[Enter>{2:F2}, Exit<{3:F2}]",
                    _currentRegime, atr60, _config.HighVolatilityATR_EnterThreshold, _config.HighVolatilityATR_ExitThreshold));
            }
        }
    }
}

