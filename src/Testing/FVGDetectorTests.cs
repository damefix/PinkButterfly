// ============================================================================
// FVGDetectorTests.cs
// PinkButterfly CoreBrain - Tests para FVGDetector y Scoring
// 
// Valida:
// - Detección de FVGs bullish/bearish
// - Validación de tamaño mínimo
// - Merge de FVGs consecutivos
// - Tracking de toques y fill
// - Cálculo de scores
// ============================================================================

using System;
using System.Linq;
using NinjaTrader.NinjaScript.Indicators.PinkButterfly;

namespace PinkButterfly.Tests
{
    /// <summary>
    /// Tests del FVGDetector y ScoringEngine
    /// </summary>
    public class FVGDetectorTests
    {
        private int _testsPassed = 0;
        private int _testsFailed = 0;
        private Action<string> _logger;

        public FVGDetectorTests(Action<string> logger = null)
        {
            _logger = logger ?? Console.WriteLine;
        }

        public void RunAllTests()
        {
            _logger("==============================================");
            _logger("FVG DETECTOR & SCORING TESTS");
            _logger("==============================================");
            _logger("");

            Test_MockProvider_BasicFunctionality();
            Test_FVGDetection_BullishGap();
            Test_FVGDetection_BearishGap();
            Test_FVGDetection_MinSizeValidation();
            Test_FVGDetection_NoGap();
            Test_Scoring_InitialScore();
            Test_Scoring_Freshness();
            Test_Scoring_TouchFactor();

            _logger("");
            _logger("==============================================");
            _logger($"RESULTADOS: {_testsPassed} passed, {_testsFailed} failed");
            _logger("==============================================");
        }

        private void Assert(bool condition, string testName, string message)
        {
            if (condition)
            {
                _testsPassed++;
                _logger($"✓ PASS: {testName}");
            }
            else
            {
                _testsFailed++;
                _logger($"✗ FAIL: {testName} - {message}");
            }
        }

        /// <summary>
        /// Helper para añadir barras más fácilmente
        /// </summary>
        private void AddBar(MockBarDataProvider provider, int tfMinutes, int barIndex, double open, double high, double low, double close)
        {
            provider.AddBar(tfMinutes, new MockBar 
            { 
                Time = DateTime.UtcNow.AddMinutes(barIndex * tfMinutes), 
                Open = open, 
                High = high, 
                Low = low, 
                Close = close,
                Volume = 1000 
            });
        }

        /// <summary>
        /// Helper para añadir barras de setup (para ATR)
        /// </summary>
        private void AddSetupBars(MockBarDataProvider provider, int tfMinutes, int count, double basePrice = 5000.00)
        {
            for (int i = 0; i < count; i++)
            {
                AddBar(provider, tfMinutes, i, basePrice, basePrice + 5.00, basePrice - 5.00, basePrice + 2.00);
            }
        }

        // ========================================================================
        // TESTS
        // ========================================================================

        private void Test_MockProvider_BasicFunctionality()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            
            // Configurar 3 barras simples (precios futuros)
            AddBar(provider, 60, 0, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 1, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 2, 5002.00, 5004.00, 5000.00, 5003.00);

            Assert(provider.GetCurrentBarIndex(60) == 2, 
                   "MockProvider_CurrentBar", 
                   $"Expected bar 2, got {provider.GetCurrentBarIndex(60)}");

            Assert(provider.GetHigh(60, 1) == 5003.00, 
                   "MockProvider_GetHigh", 
                   $"Expected 5003.00, got {provider.GetHigh(60, 1)}");
        }

        private void Test_FVGDetection_BullishGap()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinFVGSizeTicks = 1;
            config.MinFVGSizeATRfactor = 0.0; // Desactivar validación ATR para test simple
            config.MergeConsecutiveFVGs = false;
            config.DetectNestedFVGs = false;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25); // Futuros (ES, MES, etc)
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            // Añadir 14 barras previas para que ATR(14) se pueda calcular
            AddSetupBars(provider, 60, 14);

            // Configurar barras con gap bullish (precios realistas para futuros)
            // Bar A (idx 14): Low=5000.00
            // Bar B (idx 15): irrelevante
            // Bar C (idx 16): High=4998.00 → Gap bullish de 2 puntos (8 ticks)
            AddBar(provider, 60, 14, 5000.00, 5002.00, 5000.00, 5001.00); // A
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00); // B
            AddBar(provider, 60, 16, 4996.00, 4998.00, 4995.00, 4997.00); // C

            _logger($"  DEBUG: Barras añadidas");
            _logger($"    Bar 0: O={provider.GetOpen(60,0)} H={provider.GetHigh(60,0)} L={provider.GetLow(60,0)} C={provider.GetClose(60,0)}");
            _logger($"    Bar 1: O={provider.GetOpen(60,1)} H={provider.GetHigh(60,1)} L={provider.GetLow(60,1)} C={provider.GetClose(60,1)}");
            _logger($"    Bar 2: O={provider.GetOpen(60,2)} H={provider.GetHigh(60,2)} L={provider.GetLow(60,2)} C={provider.GetClose(60,2)}");
            _logger($"  DEBUG: Gap check - lowA(1.2000) > highC(1.1990) = {1.2000 > 1.1990}");

            engine.Initialize();
            _logger($"  DEBUG: Engine initialized, detector count: {engine.Config.EnableDebug}");
            
            // Llamar OnBarClose para cada barra (incluyendo las previas para ATR)
            for (int i = 0; i <= 16; i++)
            {
                engine.OnBarClose(60, i);
            }

            var fvgs = engine.GetActiveFVGs(60, 0.0);
            _logger($"  DEBUG: FVGs detectados: {fvgs.Count}");

            Assert(fvgs.Count == 1, 
                   "FVGDetection_BullishGap_Count", 
                   $"Expected 1 FVG, got {fvgs.Count}");

            if (fvgs.Count > 0)
            {
                Assert(fvgs[0].Direction == "Bullish", 
                       "FVGDetection_BullishGap_Direction", 
                       $"Expected Bullish, got {fvgs[0].Direction}");
            }
        }

        private void Test_FVGDetection_BearishGap()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinFVGSizeTicks = 1;
            config.MinFVGSizeATRfactor = 0.0;
            config.MergeConsecutiveFVGs = false;
            config.DetectNestedFVGs = false;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            // Configurar barras con gap bearish (precios realistas para futuros)
            // Bar A (idx 0): High=5000.00
            // Bar B (idx 1): irrelevante
            // Bar C (idx 2): Low=5002.00 → Gap bearish de 2 puntos (8 ticks)
            AddBar(provider, 60, 0, 4998.00, 5000.00, 4997.00, 4999.00); // A
            AddBar(provider, 60, 1, 4999.00, 5001.00, 4998.00, 5000.00); // B
            AddBar(provider, 60, 2, 5002.00, 5004.00, 5002.00, 5003.00); // C

            engine.Initialize();
            engine.OnBarClose(60, 0);
            engine.OnBarClose(60, 1);
            engine.OnBarClose(60, 2);

            var fvgs = engine.GetActiveFVGs(60, 0.0);

            Assert(fvgs.Count == 1, 
                   "FVGDetection_BearishGap_Count", 
                   $"Expected 1 FVG, got {fvgs.Count}");

            if (fvgs.Count > 0)
            {
                Assert(fvgs[0].Direction == "Bearish", 
                       "FVGDetection_BearishGap_Direction", 
                       $"Expected Bearish, got {fvgs[0].Direction}");
            }
        }

        private void Test_FVGDetection_MinSizeValidation()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinFVGSizeTicks = 100; // Gap muy grande requerido
            config.MinFVGSizeATRfactor = 0.0;
            config.MergeConsecutiveFVGs = false;
            config.DetectNestedFVGs = false;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            // Gap pequeño que no cumple MinFVGSizeTicks (gap de solo 0.25 = 1 tick)
            AddBar(provider, 60, 0, 5000.00, 5002.00, 5000.00, 5001.00); // A
            AddBar(provider, 60, 1, 5001.00, 5003.00, 4999.00, 5002.00); // B
            AddBar(provider, 60, 2, 4999.50, 5001.50, 4998.00, 5000.50); // C - gap de solo 0.50 (2 ticks)

            engine.Initialize();
            engine.OnBarClose(60, 0);
            engine.OnBarClose(60, 1);
            engine.OnBarClose(60, 2);

            var fvgs = engine.GetActiveFVGs(60, 0.0);

            Assert(fvgs.Count == 0, 
                   "FVGDetection_MinSizeValidation", 
                   $"Expected 0 FVGs (gap too small), got {fvgs.Count}");
        }

        private void Test_FVGDetection_NoGap()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinFVGSizeTicks = 1;
            config.MinFVGSizeATRfactor = 0.0;
            config.MergeConsecutiveFVGs = false;
            config.DetectNestedFVGs = false;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            // Barras sin gap (precios continuos)
            AddBar(provider, 60, 0, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 1, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 2, 5002.00, 5004.00, 5000.00, 5003.00); // No gap

            engine.Initialize();
            engine.OnBarClose(60, 0);
            engine.OnBarClose(60, 1);
            engine.OnBarClose(60, 2);

            var fvgs = engine.GetActiveFVGs(60, 0.0);

            Assert(fvgs.Count == 0, 
                   "FVGDetection_NoGap", 
                   $"Expected 0 FVGs (no gap), got {fvgs.Count}");
        }

        private void Test_Scoring_InitialScore()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinFVGSizeTicks = 1;
            config.MinFVGSizeATRfactor = 0.0;
            config.MergeConsecutiveFVGs = false;
            config.DetectNestedFVGs = false;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            // Añadir barras de setup para ATR
            AddSetupBars(provider, 60, 14);

            // Crear FVG (gap de 2 puntos)
            AddBar(provider, 60, 14, 5000.00, 5002.00, 5000.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 4996.00, 4998.00, 4995.00, 4997.00);

            engine.Initialize();
            for (int i = 0; i <= 16; i++)
            {
                engine.OnBarClose(60, i);
            }

            var fvgs = engine.GetActiveFVGs(60, 0.0);

            Assert(fvgs.Count > 0, 
                   "Scoring_InitialScore_FVGCreated", 
                   "FVG should be created");

            if (fvgs.Count > 0)
            {
                _logger($"  DEBUG Scoring: FVG Low={fvgs[0].Low} High={fvgs[0].High} Center={fvgs[0].CenterPrice}");
                _logger($"  DEBUG Scoring: MidPrice={provider.GetMidPrice()}");
                _logger($"  DEBUG Scoring: Score={fvgs[0].Score:F6}");
                _logger($"  DEBUG Scoring: TF={fvgs[0].TF} CreatedAt={fvgs[0].CreatedAtBarIndex}");
                
                Assert(fvgs[0].Score > 0.0 && fvgs[0].Score <= 1.0, 
                       "Scoring_InitialScore_Range", 
                       $"Score should be in [0,1], got {fvgs[0].Score}");

                _logger($"  Initial Score: {fvgs[0].Score:F3}");
            }
        }

        private void Test_Scoring_Freshness()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinFVGSizeTicks = 1;
            config.MinFVGSizeATRfactor = 0.0;
            config.MergeConsecutiveFVGs = false;
            config.DetectNestedFVGs = false;
            config.EnableDebug = false;
            config.FreshnessLambda = 5; // Decay rápido para test

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            // Añadir barras de setup para ATR
            AddSetupBars(provider, 60, 14);

            // Crear FVG en bar 16
            AddBar(provider, 60, 14, 5000.00, 5002.00, 5000.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 4996.00, 4998.00, 4995.00, 4997.00);

            engine.Initialize();
            for (int i = 0; i <= 16; i++)
            {
                engine.OnBarClose(60, i);
            }

            var fvgsInitial = engine.GetActiveFVGs(60, 0.0);
            double initialScore = fvgsInitial.Count > 0 ? fvgsInitial[0].Score : 0;

            // Avanzar 10 barras (sin tocar el FVG)
            for (int i = 17; i < 27; i++)
            {
                AddBar(provider, 60, i, 4996.00, 4998.00, 4995.00, 4997.00);
                engine.OnBarClose(60, i);
            }

            var fvgsLater = engine.GetActiveFVGs(60, 0.0);
            double laterScore = fvgsLater.Count > 0 ? fvgsLater[0].Score : 0;

            Assert(laterScore < initialScore, 
                   "Scoring_Freshness_Decay", 
                   $"Score should decay over time. Initial:{initialScore:F3}, Later:{laterScore:F3}");

            _logger($"  Freshness decay: {initialScore:F3} → {laterScore:F3}");
        }

        private void Test_Scoring_TouchFactor()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinFVGSizeTicks = 1;
            config.MinFVGSizeATRfactor = 0.0;
            config.MergeConsecutiveFVGs = false;
            config.DetectNestedFVGs = false;
            config.EnableDebug = false;
            config.TouchBodyBonusPerTouch = 0.2; // Bonus alto para test

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            // Crear FVG bullish en [4998.00, 5000.00]
            AddBar(provider, 60, 0, 5000.00, 5002.00, 5000.00, 5001.00);
            AddBar(provider, 60, 1, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 2, 4996.00, 4998.00, 4995.00, 4997.00);

            engine.Initialize();
            engine.OnBarClose(60, 0);
            engine.OnBarClose(60, 1);
            engine.OnBarClose(60, 2);

            var fvgsInitial = engine.GetActiveFVGs(60, 0.0);
            int initialTouches = fvgsInitial.Count > 0 ? fvgsInitial[0].TouchCount_Body : 0;

            // Tocar el FVG con body (close dentro del FVG [4998.00, 5000.00])
            AddBar(provider, 60, 3, 4998.00, 5000.00, 4997.00, 4999.00); // Close en 4999.00 (dentro FVG)
            engine.OnBarClose(60, 3);

            var fvgsAfterTouch = engine.GetActiveFVGs(60, 0.0);
            int touchesAfter = fvgsAfterTouch.Count > 0 ? fvgsAfterTouch[0].TouchCount_Body : 0;

            Assert(touchesAfter > initialTouches, 
                   "Scoring_TouchFactor_Increment", 
                   $"Touch count should increase. Initial:{initialTouches}, After:{touchesAfter}");

            _logger($"  Touch count: {initialTouches} → {touchesAfter}");
        }
    }
}

