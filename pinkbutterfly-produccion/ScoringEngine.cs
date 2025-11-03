// ============================================================================
// ScoringEngine.cs
// PinkButterfly CoreBrain - Motor de puntuación de estructuras
// 
// Calcula scores dinámicos multi-dimensionales para estructuras de precio
// basándose en múltiples factores:
// - TF Weight (peso del timeframe)
// - Freshness (frescura / edad)
// - Proximity (distancia al precio actual)
// - Type Weight (peso del tipo de estructura)
// - Touch Factor (bonus por toques)
// - Confluence (bonus por confluencia con otras estructuras)
// - Momentum (multiplicador por alineación con bias de mercado)
// - Fill Handling (penalización por fill, pero mantiene residual score)
// - Decay (decay exponencial con el tiempo)
//
// IMPORTANTE: Scores internos en rango 0.0-1.0
// ============================================================================

using System;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Motor de scoring para estructuras de precio
    /// Implementa el algoritmo multi-dimensional de puntuación
    /// </summary>
    public class ScoringEngine
    {
        private readonly EngineConfig _config;
        private readonly IBarDataProvider _provider;
        private readonly ILogger _logger;

        // Pesos máximos para normalización
        private readonly double _maxTFWeight;
        private readonly double _maxTypeWeight;

        public ScoringEngine(EngineConfig config, IBarDataProvider provider, ILogger logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // Calcular pesos máximos para normalización
            _maxTFWeight = CalculateMaxTFWeight();
            _maxTypeWeight = CalculateMaxTypeWeight();
        }

        /// <summary>
        /// Calcula el score de una estructura
        /// 
        /// FÓRMULA:
        /// raw = TF_norm * freshness * proximity * typeNorm * touchFactor * confluence * momentumMultiplier
        /// 
        /// Si FillPercentage >= FillThreshold:
        ///   raw = max(raw, ResidualScore)
        /// 
        /// score = clamp(raw * decay, 0.0, 1.0)
        /// </summary>
        public double CalculateScore(StructureBase structure, int currentBarIndex, string marketBias, int confluenceCount = 1)
        {
            try
            {
                // ============================================================
                // 1. TF WEIGHT NORMALIZATION
                // ============================================================
                double tfWeight = GetTFWeight(structure.TF);
                double tfNorm = tfWeight / _maxTFWeight;

                // ============================================================
                // 2. FRESHNESS (decay exponencial por edad)
                // ============================================================
                int ageBars = currentBarIndex - structure.CreatedAtBarIndex;
                if (ageBars < 0) ageBars = 0;
                
                // Freshness: para FVGs aplicamos un lambda más bajo (mayor decaimiento en edades altas)
                double freshLambda = (structure is FVGInfo) ? Math.Max(5.0, _config.FreshnessLambda * 0.5) : _config.FreshnessLambda;
                double freshness = Math.Exp(-ageBars / freshLambda);

                // ============================================================
                // 3. PROXIMITY (distancia al precio actual)
                // ============================================================
                // Fuente de precio para proximidad (Close por TF o Mid global)
                string priceSrc = (_config.ProximityPriceSource ?? "Close").ToLowerInvariant();
                double currentPrice = priceSrc == "mid"
                    ? _provider.GetMidPrice()
                    : _provider.GetClose(structure.TF, currentBarIndex);
                // Para FVGs, medir proximidad contra el borde de entrada (más estricto); resto usa centro
                double referencePrice = structure.CenterPrice;

                if (structure is FVGInfo fvgRef)
                {
                    if (_config.UseNearestEdgeForFVGProximity)
                    {
                        double distLow = Math.Abs(currentPrice - fvgRef.Low);
                        double distHigh = Math.Abs(currentPrice - fvgRef.High);
                        referencePrice = distLow <= distHigh ? fvgRef.Low : fvgRef.High;
                    }
                    else
                    {
                        // Comportamiento base: usar el centro de la zona
                        referencePrice = structure.CenterPrice;
                    }
                }

                double distanceTicks = Math.Abs(currentPrice - referencePrice) / _provider.GetTickSize();

                double atr = _provider.GetATR(structure.TF, 14, currentBarIndex);
                double proxMaxTicks = (_config.ProxMaxATRFactor * atr) / _provider.GetTickSize();

                // Proximity con hard cut por distancia extrema y decaimiento cúbico
                double proximity;
                if (_config.EnableProximityHardCut && proxMaxTicks > 0 && distanceTicks >= proxMaxTicks)
                {
                    proximity = 0.0;
                }
                else
                {
                    double rel = proxMaxTicks > 0 ? Math.Min(distanceTicks / proxMaxTicks, 1.0) : 1.0;
                    proximity = 1.0 - (rel * rel * rel);
                }
                if (proximity < 0) proximity = 0;

                // ============================================================
                // 4. TYPE WEIGHT NORMALIZATION
                // ============================================================
                double typeWeight = GetTypeWeight(structure.Type);
                double typeNorm = typeWeight / _maxTypeWeight;

                // ============================================================
                // 5. TOUCH FACTOR (bonus por toques)
                // ============================================================
                int touchCount = Math.Min(structure.TouchCount_Body, _config.MaxTouchBodyCap);
                double touchFactor = 1.0 + (_config.TouchBodyBonusPerTouch * touchCount);

                // ============================================================
                // 6. CONFLUENCE (bonus por confluencia)
                // ============================================================
                double confluence = 1.0 + (_config.ConfluenceWeight * (confluenceCount - 1));

                // ============================================================
                // 7. MOMENTUM MULTIPLIER (alineación con bias de mercado)
                // ============================================================
                double momentumMultiplier = CalculateMomentumMultiplier(structure, marketBias);

                // ============================================================
                // 8. CÁLCULO BASE
                // ============================================================
                double rawScore = tfNorm * freshness * proximity * typeNorm * touchFactor * confluence * momentumMultiplier;

                // ============================================================
                // 9. FILL HANDLING
                // ============================================================
                if (structure is FVGInfo fvg)
                {
                    if (fvg.FillPercentage >= _config.FillThreshold)
                    {
                        // Estructura filled: aplicar residual score mínimo
                        rawScore = Math.Max(rawScore, _config.ResidualScore);
                    }
                }

                // ============================================================
                // 10. BROKEN SWING HANDLING (Swings rotos)
                // ============================================================
                if (structure is SwingInfo swing)
                {
                    if (swing.IsBroken)
                    {
                        // Swing roto: penalización drástica (10% del score original)
                        // Los swings rotos pierden relevancia pero mantienen valor histórico
                        rawScore *= 0.10;
                    }
                }

                // ============================================================
                // 11. DECAY (decay adicional por tiempo desde última actualización)
                // ============================================================
                int deltaBarsSinceUpdate = currentBarIndex - structure.LastUpdatedBarIndex;
                if (deltaBarsSinceUpdate < 0) deltaBarsSinceUpdate = 0;
                
                double decay = Math.Exp(-deltaBarsSinceUpdate / (double)_config.DecayLambda);
                double finalScore = rawScore * decay;

                // Penalización adicional para FVGs muy antiguos
                if (structure is FVGInfo fvgOld && _config.EnableFVGAgePenalty200)
                {
                    int ageBarsAll = currentBarIndex - fvgOld.CreatedAtBarIndex;
                    if (ageBarsAll >= 200)
                    {
                        finalScore *= 0.05; // aún más agresivo para FVG muy antiguos
                    }
                }

                // Pequeño bonus por TF alto en FVG para reflejar mayor solidez (aplicado tras decay y antes del clamp final)
                if (structure is FVGInfo fvgTfBonus && _config.EnableFVGTFBonus)
                {
                    if (fvgTfBonus.TF >= 240) finalScore += 0.05;
                    else if (fvgTfBonus.TF >= 60) finalScore += 0.02;
                }

                // Clamp final
                finalScore = Math.Max(0.0, Math.Min(1.0, finalScore));

                // Fuerza de proximidad extrema para FVG (garantizar score muy bajo si distancia supera el umbral de proximidad)
                if (structure is FVGInfo fvgProx && _config.EnableProximityHardCut)
                {
                    if (proxMaxTicks > 0 && distanceTicks >= proxMaxTicks)
                    {
                        finalScore = Math.Min(finalScore, 0.03);
                    }
                }

                // ============================================================
                // 12. CLAMP 0.0-1.0
                // ============================================================
                finalScore = Math.Max(0.0, Math.Min(1.0, finalScore));

                // Debug logging
                if (_config.EnableDebug && _logger != null)
                {
                    _logger.Debug($"Score {structure.Id}: TF={tfNorm:F2} Fresh={freshness:F2} Prox={proximity:F2} " +
                                 $"Type={typeNorm:F2} Touch={touchFactor:F2} Conf={confluence:F2} " +
                                 $"Mom={momentumMultiplier:F2} Decay={decay:F2} → {finalScore:F3}");

                    // FVG trace específica para diagnosticar proximidad (solo Debug aquí)
                    if (structure is FVGInfo)
                    {
                        double distancePoints = Math.Abs(currentPrice - referencePrice);
                        double distanceAtr = atr > 0 ? (distancePoints / atr) : 0.0;
                        _logger.Debug($"[FVG][TRACE] TF={structure.TF} Close={currentPrice:F2} Ref={referencePrice:F2} ATR={atr:F2} Dist={distancePoints:F2}pts ({distanceAtr:F2} ATR) Prox={proximity:F3} Final={finalScore:F3}");
                    }
                }

                // (trazas de diagnóstico forzadas eliminadas)

                return finalScore;
            }
            catch (Exception ex)
            {
                _logger?.Exception($"ScoringEngine: Error calculando score para {structure.Id}", ex);
                return 0.0;
            }
        }

        /// <summary>
        /// Obtiene el peso del timeframe desde la configuración
        /// </summary>
        private double GetTFWeight(int tfMinutes)
        {
            if (_config.TFWeights.ContainsKey(tfMinutes))
                return _config.TFWeights[tfMinutes];
            
            // Default: peso proporcional al TF (mayor TF = mayor peso)
            return tfMinutes / 1440.0;
        }

        /// <summary>
        /// Obtiene el peso del tipo de estructura desde la configuración
        /// </summary>
        private double GetTypeWeight(string type)
        {
            if (_config.TypeWeights.ContainsKey(type))
                return _config.TypeWeights[type];
            
            // Default: peso igual para todos
            return 1.0;
        }

        /// <summary>
        /// Calcula el multiplicador de momentum basado en alineación con bias de mercado
        /// </summary>
        private double CalculateMomentumMultiplier(StructureBase structure, string marketBias)
        {
            // Por defecto, sin multiplicador
            double multiplier = 1.0;

            // Determinar dirección de la estructura
            string structureDirection = GetStructureDirection(structure);
            if (string.IsNullOrEmpty(structureDirection))
                return multiplier;

            // Si la estructura está alineada con el bias de mercado
            bool aligned = (structureDirection == "Bullish" && marketBias == "Bullish") ||
                          (structureDirection == "Bearish" && marketBias == "Bearish");

            if (aligned)
            {
                // Verificar si hay break momentum fuerte
                if (structure is StructureBreakInfo breakInfo)
                {
                    if (breakInfo.BreakMomentum == "Strong")
                        multiplier = _config.BreakMomentumMultiplierStrong;
                    else if (breakInfo.BreakMomentum == "Weak")
                        multiplier = _config.BreakMomentumMultiplierWeak;
                }
                else
                {
                    // Para otras estructuras alineadas, usar multiplicador débil
                    multiplier = _config.BreakMomentumMultiplierWeak;
                }
            }

            return multiplier;
        }

        /// <summary>
        /// Obtiene la dirección de una estructura (Bullish/Bearish/null)
        /// </summary>
        private string GetStructureDirection(StructureBase structure)
        {
            if (structure is FVGInfo fvg)
                return fvg.Direction;
            
            if (structure is OrderBlockInfo ob)
                return ob.Direction;
            
            if (structure is StructureBreakInfo sb)
                return sb.Direction;
            
            if (structure is SwingInfo swing)
                return swing.IsHigh ? "Bearish" : "Bullish"; // Swing high = resistencia = bearish
            
            return null;
        }

        /// <summary>
        /// Calcula el peso máximo de TF para normalización
        /// </summary>
        private double CalculateMaxTFWeight()
        {
            if (_config.TFWeights == null || _config.TFWeights.Count == 0)
                return 1.0;
            
            double max = 0;
            foreach (var weight in _config.TFWeights.Values)
            {
                if (weight > max)
                    max = weight;
            }
            
            return max > 0 ? max : 1.0;
        }

        /// <summary>
        /// Calcula el peso máximo de tipo para normalización
        /// </summary>
        private double CalculateMaxTypeWeight()
        {
            if (_config.TypeWeights == null || _config.TypeWeights.Count == 0)
                return 1.0;
            
            double max = 0;
            foreach (var weight in _config.TypeWeights.Values)
            {
                if (weight > max)
                    max = weight;
            }
            
            return max > 0 ? max : 1.0;
        }

        /// <summary>
        /// Recalcula el score de una estructura (usado cuando cambia el contexto)
        /// </summary>
        public void RecalculateScore(StructureBase structure, int currentBarIndex, string marketBias, int confluenceCount = 1)
        {
            structure.Score = CalculateScore(structure, currentBarIndex, marketBias, confluenceCount);
        }

        /// <summary>
        /// Recalcula scores de todas las estructuras en una lista
        /// </summary>
        public void RecalculateScores(System.Collections.Generic.List<StructureBase> structures, int currentBarIndex, string marketBias)
        {
            foreach (var structure in structures)
            {
                // TODO: Calcular confluenceCount real usando IntervalTree
                RecalculateScore(structure, currentBarIndex, marketBias, confluenceCount: 1);
            }
        }
    }
}

// ============================================================================
// NOTAS DE IMPLEMENTACIÓN
// ============================================================================
//
// 1. NORMALIZACIÓN:
//    - TF Weight: normalizado por maxTFWeight (típicamente 1.0 para D1)
//    - Type Weight: normalizado por maxTypeWeight
//    - Proximity: normalizado por ProxMaxATRFactor * ATR
//
// 2. FRESHNESS:
//    - Decay exponencial: exp(-ageBars / FreshnessLambda)
//    - FreshnessLambda=20 → 50% score a las 20 barras
//
// 3. PROXIMITY:
//    - Distancia en ticks desde precio actual al centro de la estructura
//    - ProxMaxATRFactor=2.5 → estructuras a más de 2.5*ATR tienen proximity=0
//
// 4. TOUCH FACTOR:
//    - Bonus por cada toque de body: TouchBodyBonusPerTouch (default 0.12 = +12%)
//    - Cap en MaxTouchBodyCap (default 5 toques)
//
// 5. FILL HANDLING:
//    - Si FillPercentage >= FillThreshold (0.90): score mínimo = ResidualScore (0.05)
//    - Esto mantiene la estructura visible pero con score bajo
//
// 6. DECAY:
//    - Decay adicional por tiempo sin actualizaciones
//    - DecayLambda=100 → 50% score a las 100 barras sin update
//
// 7. MOMENTUM:
//    - Si estructura alineada con marketBias:
//      - Strong break: multiplier = 1.35
//      - Weak break: multiplier = 1.1
//
// 8. PERFORMANCE:
//    - Cálculo O(1) por estructura
//    - Sin I/O ni operaciones bloqueantes
//    - Típicamente < 0.01ms por estructura
//
// ============================================================================

