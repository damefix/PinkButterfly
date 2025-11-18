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
            BuildDecisionSummary(snapshot, barData, coreEngine, timeframeMinutes, currentBar);
            
            // 2. Detectar régimen de volatilidad con histéresis (V6.0i) - ANTES del bias
            DetectRegime(snapshot, barData, timeframeMinutes, currentBar);

            // 3. Calcular GlobalBias y GlobalBiasStrength (usa régimen para threshold)
            CalculateGlobalBias(snapshot, coreEngine, timeframeMinutes, currentBar);

            // 4. Calcular MarketClarity
            CalculateMarketClarity(snapshot, coreEngine, currentBar);

            // 5. Calcular MarketVolatilityNormalized
            CalculateMarketVolatility(snapshot, barData, timeframeMinutes, currentBar);

            _logger.Debug(string.Format(
                "[ContextManager] Contexto completado - Regime: {0}, Bias: {1} ({2:F2}), Clarity: {3:F2}, Volatility: {4:F2}",
                snapshot.MarketRegime, snapshot.GlobalBias, snapshot.GlobalBiasStrength, snapshot.MarketClarity, snapshot.MarketVolatilityNormalized
            ));
        }

        /// <summary>
        /// Construye el DecisionSummary con métricas agregadas del mercado
        /// </summary>
        private void BuildDecisionSummary(DecisionSnapshot snapshot, IBarDataProvider barData, CoreEngine coreEngine, int timeframeMinutes, int currentBar)
        {
            var summary = new DecisionSummary();

            // CurrentPrice: usar el precio del TF MÁS ALTO, alineado por tiempo con el TF REAL del ciclo actual
            int decisionTF = _config.DecisionTimeframeMinutes;
            // Anclar al TF REAL del ciclo actual (timeframeMinutes), no al decisionTF
            DateTime analysisTime = barData.GetBarTime(timeframeMinutes, currentBar);
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
        private void CalculateGlobalBias(DecisionSnapshot snapshot, CoreEngine coreEngine, int timeframeMinutes, int currentBar)
        {
            // V6.0g: BIAS COMPUESTO MULTI-SEÑAL para mayor sensibilidad intradía
            // Combina 4 componentes con pesos específicos (suma ponderada = score [-1, +1])
            // Score > 0.5: Bullish | Score < -0.5: Bearish | Resto: Neutral
            
            int primaryTF = 60; // 1H para EMA y regresión
            int decisionTF = _config.DecisionTimeframeMinutes;
            // Anclar al ciclo actual
            DateTime analysisTime = _barData.GetBarTime(timeframeMinutes, currentBar);
            _logger.Debug($"[CTX] analysisTime={analysisTime:yyyy-MM-dd HH:mm} (tf={timeframeMinutes}, idx={currentBar})");
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
            
            // Componente 2: EMA20 vs EMA50 Cross (25%) - V6.0k: Corregido (era precio vs EMA50)
            double emaCrossScore = CalculateEMACross(idx60, primaryTF, 20, 50);
            
            // Componente 3: BOS/CHoCH Count (25%) - Cambios de estructura recientes
            // V6.0k TEMP: Bar anterior por timing issue (BOSDetector se ejecuta después)
            int bosBar = Math.Max(0, currentBar - 1);
            double bosScore = CalculateBOSScore(coreEngine, bosBar, out DateTime bosCalcTime);
            
            // Componente 4: Regresión Lineal 24h (20%) - Dirección últimas 24h
            double regressionScore = CalculateRegression24h(idx60, primaryTF);
            
            // SCORE COMPUESTO (rango [-1, +1])
            double compositeScore = (ema20Score * 0.30) + (emaCrossScore * 0.25) + 
                                    (bosScore * 0.25) + (regressionScore * 0.20);
            
            // V6.0i: Threshold adaptativo según régimen (configurable)
            double biasThreshold = (snapshot.MarketRegime == "HighVol") 
                ? _config.BiasThreshold_HighVol 
                : _config.BiasThreshold_Normal;
            
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
                "[DIAGNOSTICO][Context] V6.0k Regime={0} BiasComposite={1} Score={2:F2} Threshold={3:F2} EMA20Slope={4:F2} EMACross={5:F2} BOS={6:F2} (barUsed={7}) Reg24h={8:F2}",
                snapshot.MarketRegime, snapshot.GlobalBias, compositeScore, biasThreshold, ema20Score, emaCrossScore, bosScore, bosBar, regressionScore
            ));
            
            // ✅ FIX: Actualizar CoreEngine con el bias compuesto calculado
            // Esto sincroniza el bias mostrado en el gráfico con el usado en la lógica
            coreEngine.UpdateMarketBias(snapshot.GlobalBias);
        }
        
        // ========================================================================
        // COMPONENTES DE BIAS COMPUESTO (V6.0g)
        // ========================================================================
        
        /// <summary>
        /// Calcula slope de EMA20@60m comparando EMA actual vs EMA hace 5 barras.
        /// V6.0k: Normalización sin saturación (0.3% → 1.0, no ×100)
        /// Retorna: +1 (subida fuerte), 0 (plano), -1 (bajada fuerte)
        /// </summary>
        private double CalculateEMASlope(int idx60, int tf, int period)
        {
            if (idx60 < period) return 0.0;
            
            double emaCurrent = CalculateEMA(idx60, tf, period);
            double emaPast = CalculateEMA(idx60 - 5, tf, period);
            
            if (emaCurrent == 0 || emaPast == 0) return 0.0;
            
            double slopePct = (emaCurrent - emaPast) / Math.Max(1e-9, emaPast);
            double norm = slopePct / 0.003; // 0.3% → 1.0 (calibrable)
            return Math.Max(-1.0, Math.Min(1.0, norm));
        }
        
        /// <summary>
        /// Detecta cross de EMA rápida vs EMA lenta.
        /// V6.0k: Corregido para comparar EMA20 vs EMA50 (no precio vs EMA)
        /// Retorna: +1 (EMA rápida > EMA lenta), -1 (EMA rápida < EMA lenta)
        /// </summary>
        private double CalculateEMACross(int idx60, int tf, int fastPeriod, int slowPeriod)
        {
            if (idx60 < Math.Max(fastPeriod, slowPeriod)) return 0.0;
            
            double emaFast = CalculateEMA(idx60, tf, fastPeriod);
            double emaSlow = CalculateEMA(idx60, tf, slowPeriod);
            
            if (emaFast == 0 || emaSlow == 0) return 0.0;
            
            return emaFast > emaSlow ? 1.0 : -1.0;
        }
        
        /// <summary>
        /// Cuenta BOS/CHoCH recientes con dirección real y ponderación por momentum/tiempo.
        /// V6.0k: Corregido para usar Direction property y ventana temporal (24h)
        /// V6.0k TEMP FIX: currentBar puede ser bar-1 por timing issue con BOSDetector
        /// TODO: Reordenar pipeline para ejecutar BOSDetector antes de ContextManager
        /// BOS Bull > BOS Bear → +1 | BOS Bear > BOS Bull → -1
        /// </summary>
        private double CalculateBOSScore(CoreEngine coreEngine, int currentBar, out DateTime calculationTime)
        {
            double bullScore = 0.0;
            double bearScore = 0.0;
            int lookbackMinutes = 1440; // 24h en minutos (ventana temporal fija)
            
            // V6.0k: Usar bar especificado (puede ser bar anterior por timing issue)
            DateTime currentTime = _barData.GetBarTime(_config.DecisionTimeframeMinutes, currentBar);
            calculationTime = currentTime; // Output para logging
            
            int totalStructures = 0;
            int validAgeStructures = 0;
            int filteredByAge = 0;
            int bullishCount = 0;
            int bearishCount = 0;
            
            // Log detallado cada 500 barras
            bool detailedLog = (currentBar % 500 == 0);
            
            foreach (int tf in _config.TimeframesToUse)
            {
                // V6.0k FIX: Usar GetStructureBreaks() en vez de GetAllStructures()
                // Este método devuelve específicamente BOS/CHoCH (ya son StructureBreakInfo)
                var bosChochStructures = coreEngine.GetStructureBreaks(tf, breakType: null, maxCount: 200);
                int countThisTF = bosChochStructures.Count;
                totalStructures += countThisTF;
                
                if (detailedLog && countThisTF > 0)
                {
                    _logger.Info(string.Format("[BOS_DETAIL] TF={0} devolvió {1} estructuras BOS/CHoCH (maxCount=200, currentTime={2:yyyy-MM-dd HH:mm})",
                        tf, countThisTF, currentTime));
                    
                    // Log de las primeras 3 estructuras de este TF
                    int shown = 0;
                    foreach (var s in bosChochStructures.Take(3))
                    {
                        _logger.Info(string.Format("[BOS_DETAIL] TF={0} Struct[{1}]: Type={2} Dir={3} StartTime={4:yyyy-MM-dd HH:mm} IsActive={5} Age={6:F1}min",
                            tf, shown, s.Type, s.Direction, s.StartTime, s.IsActive, (currentTime - s.StartTime).TotalMinutes));
                        shown++;
                    }
                }
                
                // Procesar estructuras BOS/CHoCH recientes
                foreach (var bosInfo in bosChochStructures)
                {
                    // V6.0k FIX: Calcular edad en minutos usando DateTime (no índices de barras)
                    TimeSpan ageSpan = currentTime - bosInfo.StartTime;
                    double ageMinutes = ageSpan.TotalMinutes;
                    if (ageMinutes > lookbackMinutes || ageMinutes < 0)
                    {
                        filteredByAge++;
                        if (detailedLog && filteredByAge <= 3)
                            _logger.Info(string.Format("[BOS_DETAIL] Filtered by age: TF={0} Type={1} ageMinutes={2:F1} > lookback={3} (currentTime={4:yyyy-MM-dd HH:mm}, StartTime={5:yyyy-MM-dd HH:mm})",
                                tf, bosInfo.Type, ageMinutes, lookbackMinutes, currentTime, bosInfo.StartTime));
                        continue;
                    }
                    validAgeStructures++;
                    
                    // V6.0k: Usar Direction real (no heurística High/Low)
                    bool isBullish = bosInfo.Direction == "Bullish";
                    
                    // V6.0k: Ponderación por momentum y tiempo
                    double momentumWeight = (bosInfo.BreakMomentum == "Strong") ? 1.5 : 1.0;
                    double timeDecay = Math.Exp(-ageMinutes / (lookbackMinutes * 0.5)); // Decae a e^-2 al final
                    double weight = momentumWeight * timeDecay;
                    
                    if (isBullish)
                    {
                        bullScore += weight;
                        bullishCount++;
                    }
                    else
                    {
                        bearScore += weight;
                        bearishCount++;
                    }
                    
                    if (detailedLog && validAgeStructures <= 5)
                    {
                        _logger.Info(string.Format("[BOS_DETAIL] Procesada: TF={0} Type={1} Dir={2} Age={3:F1}min Momentum={4} Weight={5:F3} (Bull={6:F3} Bear={7:F3})",
                            tf, bosInfo.Type, bosInfo.Direction, ageMinutes, bosInfo.BreakMomentum, weight, bullScore, bearScore));
                    }
                }
            }
            
            if (bullScore + bearScore == 0.0)
            {
                if (detailedLog) // Log detallado cada 500 barras
                    _logger.Info(string.Format("[BOS_DEBUG] Score=0.00 | TotalRetrieved={0}, FilteredByAge={1}, ValidAge={2}, Bull={3}, Bear={4}, currentBar={5}, currentTime={6:yyyy-MM-dd HH:mm}",
                        totalStructures, filteredByAge, validAgeStructures, bullishCount, bearishCount, currentBar, currentTime));
                return 0.0;
            }
            
            double ratio = (bullScore - bearScore) / (bullScore + bearScore);
            double finalScore = Math.Max(-1.0, Math.Min(1.0, ratio));
            
            if (detailedLog)
            {
                _logger.Info(string.Format("[BOS_DEBUG] Score={0:F3} | TotalRetrieved={1}, ValidAge={2}, Bull={3} (score={4:F3}), Bear={5} (score={6:F3}), currentBar={7}, currentTime={8:yyyy-MM-dd HH:mm}",
                    finalScore, totalStructures, validAgeStructures, bullishCount, bullScore, bearishCount, bearScore, currentBar, currentTime));
            }
            
            return finalScore;
        }
        
        /// <summary>
        /// Regresión lineal sobre últimas 24 barras (24h en TF60).
        /// V6.0k: Normalizada por volatilidad (no saturación ×1000) y ponderada por R²
        /// Pendiente positiva → +1 | Pendiente negativa → -1
        /// </summary>
        private double CalculateRegression24h(int idx60, int tf)
        {
            int lookback = 24; // 24 barras de 60m = 24 horas
            if (idx60 < lookback) return 0.0;
            
            double sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0, sumY2 = 0;
            int n = lookback;
            
            // Calcular slope y R²
            for (int i = 0; i < n; i++)
            {
                double x = lookback - i - 1;
                double y = _barData.GetClose(tf, idx60 - i);
                sumX += x;
                sumY += y;
                sumXY += x * y;
                sumX2 += x * x;
                sumY2 += y * y;
            }
            
            double slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
            
            // V6.0k: Calcular R² (calidad de ajuste)
            double meanY = sumY / n;
            double ssTotal = sumY2 - n * meanY * meanY;
            double ssResidual = sumY2 - (sumY * sumY / n) - slope * (sumXY - (sumX * sumY / n));
            double rSquared = Math.Max(0.0, 1.0 - (ssResidual / Math.Max(1e-9, ssTotal)));
            
            // V6.0k: Normalizar por volatilidad de precios (no saturación ×1000)
            double priceStd = 0.0;
            for (int i = 0; i < n; i++)
            {
                double y = _barData.GetClose(tf, idx60 - i);
                priceStd += Math.Pow(y - meanY, 2);
            }
            priceStd = Math.Sqrt(priceStd / Math.Max(1, n - 1));
            
            // Normalizar slope: cambio por hora como % de volatilidad
            double slopeNorm = (slope / Math.Max(1e-9, priceStd)) * 10.0; // Escalar conservador
            
            // Ponderar por R² (descartar ruido con bajo ajuste)
            double score = slopeNorm * rSquared;
            
            return Math.Max(-1.0, Math.Min(1.0, score));
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
        private void CalculateMarketVolatility(DecisionSnapshot snapshot, IBarDataProvider barData, int timeframeMinutes, int currentBar)
        {
            // Usar el timeframe principal (más alto) para calcular volatilidad
            int primaryTF = _config.TimeframesToUse.OrderByDescending(tf => tf).FirstOrDefault();
            if (primaryTF == 0)
                primaryTF = 60; // Fallback a 1H

            // Alinear índice del TF primario al tiempo del TF del ciclo actual
            int decisionTF_forVol = _config.DecisionTimeframeMinutes;
            DateTime analysisTime_forVol = _barData.GetBarTime(timeframeMinutes, currentBar);
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
        private void DetectRegime(DecisionSnapshot snapshot, IBarDataProvider barData, int timeframeMinutes, int currentBar)
        {
            if (!_config.UseAdaptiveRegime)
            {
                snapshot.MarketRegime = "Normal";
                return;
            }
            
            // Calcular ATR60 (base para detección de régimen)
            int primaryTF = 60;
            int decisionTF = _config.DecisionTimeframeMinutes;
            DateTime analysisTime = barData.GetBarTime(timeframeMinutes, currentBar);
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

