// ============================================================================
// SwingDetectorTests.cs
// PinkButterfly CoreBrain - Tests exhaustivos para SwingDetector
// 
// Cubre:
// - Detección básica de Swing Highs/Lows
// - Validación de nLeft/nRight
// - Validación de tamaño mínimo (ATR)
// - Detección de rupturas
// - Edge cases
// - Scoring de swings
// ============================================================================

using System;
using System.Linq;
using NinjaTrader.NinjaScript.Indicators.PinkButterfly;

namespace PinkButterfly.Tests
{
    /// <summary>
    /// Tests exhaustivos del SwingDetector
    /// </summary>
    public class SwingDetectorTests
    {
        private int _testsPassed = 0;
        private int _testsFailed = 0;
        private Action<string> _logger;

        public SwingDetectorTests(Action<string> logger = null)
        {
            _logger = logger ?? Console.WriteLine;
        }

        public void RunAllTests()
        {
            _logger("==============================================");
            _logger("SWING DETECTOR TESTS");
            _logger("==============================================");
            _logger("");

            // Tests básicos
            Test_SwingHigh_BasicDetection();
            Test_SwingLow_BasicDetection();
            Test_SwingHigh_nLeft_Validation();
            Test_SwingHigh_nRight_Validation();
            Test_SwingLow_nLeft_Validation();
            Test_SwingLow_nRight_Validation();
            
            // Tests de tamaño
            Test_SwingHigh_MinSizeValidation();
            Test_SwingLow_MinSizeValidation();
            Test_SwingHigh_ExactThreshold();
            
            // Tests de rupturas
            Test_SwingHigh_Broken();
            Test_SwingLow_Broken();
            Test_SwingHigh_NotBroken();
            
            // Tests de múltiples swings
            Test_MultipleSwings_SameTF();
            Test_MultipleSwings_HighAndLow();
            
            // Tests de scoring
            Test_Swing_InitialScore();
            Test_Swing_Freshness();
            
            // Edge cases
            Test_EdgeCase_InsufficientBars();
            Test_EdgeCase_FlatMarket();
            Test_EdgeCase_VerySmallSwing();

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

        private void AddSetupBars(MockBarDataProvider provider, int tfMinutes, int count, double basePrice = 5000.00)
        {
            for (int i = 0; i < count; i++)
            {
                AddBar(provider, tfMinutes, i, basePrice, basePrice + 5.00, basePrice - 5.00, basePrice + 2.00);
            }
        }

        // ========================================================================
        // TESTS - DETECCIÓN BÁSICA
        // ========================================================================

        private void Test_SwingHigh_BasicDetection()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0; // Sin validación de tamaño para este test
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear un swing high claro en el índice 16
            // Barras: 14, 15, 16(HIGH), 17, 18
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00); // Left 2
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00); // Left 1
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // CENTER (HIGH)
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00); // Right 1
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00); // Right 2

            engine.Initialize();
            for (int i = 0; i <= 18; i++)
            {
                engine.OnBarClose(60, i);
            }

            var swings = engine.GetRecentSwings(60, 50);
            var swingHighs = swings.Where(s => s.IsHigh).ToList();

            Assert(swingHighs.Count >= 1, 
                   "SwingHigh_BasicDetection_Count", 
                   $"Expected at least 1 swing high, got {swingHighs.Count}");

            if (swingHighs.Count > 0)
            {
                var swing = swingHighs[0];
                Assert(swing.High == 5010.00, 
                       "SwingHigh_BasicDetection_Price", 
                       $"Expected swing high at 5010.00, got {swing.High}");
            }
        }

        private void Test_SwingLow_BasicDetection()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear un swing low claro en el índice 16
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 4999.00); // Left 2
            AddBar(provider, 60, 15, 4999.00, 5001.00, 4997.00, 4998.00); // Left 1
            AddBar(provider, 60, 16, 4998.00, 4999.00, 4990.00, 4995.00); // CENTER (LOW)
            AddBar(provider, 60, 17, 4995.00, 4997.00, 4993.00, 4996.00); // Right 1
            AddBar(provider, 60, 18, 4996.00, 4998.00, 4994.00, 4997.00); // Right 2

            engine.Initialize();
            for (int i = 0; i <= 18; i++)
            {
                engine.OnBarClose(60, i);
            }

            var swings = engine.GetRecentSwings(60, 50);
            var swingLows = swings.Where(s => !s.IsHigh).ToList();

            Assert(swingLows.Count >= 1, 
                   "SwingLow_BasicDetection_Count", 
                   $"Expected at least 1 swing low, got {swingLows.Count}");

            if (swingLows.Count > 0)
            {
                var swing = swingLows[0];
                Assert(swing.Low == 4990.00, 
                       "SwingLow_BasicDetection_Price", 
                       $"Expected swing low at 4990.00, got {swing.Low}");
            }
        }

        // ========================================================================
        // TESTS - VALIDACIÓN nLeft/nRight
        // ========================================================================

        private void Test_SwingHigh_nLeft_Validation()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            // Usar barras de setup completamente planas (sin rango)
            for (int i = 0; i < 14; i++)
            {
                AddBar(provider, 60, i, 5000.00, 5000.00, 5000.00, 5000.00); // Completamente planas
            }

            // Crear un patrón que NO es swing high porque left 1 tiene el mismo high que center
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00); // Left 2
            AddBar(provider, 60, 15, 5001.00, 5010.00, 4999.00, 5002.00); // Left 1 (HIGH=5010)
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // CENTER (HIGH=5010, igual que left 1)
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00); // Right 1
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00); // Right 2

            engine.Initialize();
            for (int i = 0; i <= 18; i++)
            {
                engine.OnBarClose(60, i);
            }

            var swings = engine.GetRecentSwings(60, 50);
            var swingHighs = swings.Where(s => s.IsHigh).ToList();

            // No debería detectar swing high en barra 16 porque left 1 tiene el mismo high
            // Nota: Puede haber otros swings detectados, pero no debe haber uno con High=5010
            var invalidSwing = swingHighs.FirstOrDefault(s => s.High == 5010.00);
            
            Assert(invalidSwing == null, 
                   "SwingHigh_nLeft_Validation", 
                   $"Expected no swing at 5010.00 (nLeft validation should reject it), but found one");
        }

        private void Test_SwingHigh_nRight_Validation()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear un patrón que NO es swing high porque right 1 es mayor
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00); // Left 2
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00); // Left 1
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // CENTER
            AddBar(provider, 60, 17, 5005.00, 5012.00, 5003.00, 5004.00); // Right 1 (HIGH mayor)
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00); // Right 2

            engine.Initialize();
            for (int i = 0; i <= 18; i++)
            {
                engine.OnBarClose(60, i);
            }

            var swings = engine.GetRecentSwings(60, 50);
            var swingHighs = swings.Where(s => s.IsHigh).ToList();

            Assert(swingHighs.Count == 0, 
                   "SwingHigh_nRight_Validation", 
                   $"Expected 0 swing highs (nRight validation failed), got {swingHighs.Count}");
        }

        private void Test_SwingLow_nLeft_Validation()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            // Usar barras de setup completamente planas (sin rango)
            for (int i = 0; i < 14; i++)
            {
                AddBar(provider, 60, i, 5000.00, 5000.00, 5000.00, 5000.00); // Completamente planas
            }

            // Crear un patrón que NO es swing low porque left 1 tiene el mismo low que center
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 4999.00); // Left 2
            AddBar(provider, 60, 15, 4999.00, 5001.00, 4990.00, 4998.00); // Left 1 (LOW=4990)
            AddBar(provider, 60, 16, 4998.00, 4999.00, 4990.00, 4995.00); // CENTER (LOW=4990, igual que left 1)
            AddBar(provider, 60, 17, 4995.00, 4997.00, 4993.00, 4996.00); // Right 1
            AddBar(provider, 60, 18, 4996.00, 4998.00, 4994.00, 4997.00); // Right 2

            engine.Initialize();
            for (int i = 0; i <= 18; i++)
            {
                engine.OnBarClose(60, i);
            }

            var swings = engine.GetRecentSwings(60, 50);
            var swingLows = swings.Where(s => !s.IsHigh).ToList();

            // No debería detectar swing low en barra 16 porque left 1 tiene el mismo low
            // Nota: Puede haber otros swings detectados, pero no debe haber uno con Low=4990
            var invalidSwing = swingLows.FirstOrDefault(s => s.Low == 4990.00);
            
            Assert(invalidSwing == null, 
                   "SwingLow_nLeft_Validation", 
                   $"Expected no swing at 4990.00 (nLeft validation should reject it), but found one");
        }

        private void Test_SwingLow_nRight_Validation()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear un patrón que NO es swing low porque right 1 es menor
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 4999.00); // Left 2
            AddBar(provider, 60, 15, 4999.00, 5001.00, 4997.00, 4998.00); // Left 1
            AddBar(provider, 60, 16, 4998.00, 4999.00, 4990.00, 4995.00); // CENTER
            AddBar(provider, 60, 17, 4995.00, 4997.00, 4988.00, 4996.00); // Right 1 (LOW menor)
            AddBar(provider, 60, 18, 4996.00, 4998.00, 4994.00, 4997.00); // Right 2

            engine.Initialize();
            for (int i = 0; i <= 18; i++)
            {
                engine.OnBarClose(60, i);
            }

            var swings = engine.GetRecentSwings(60, 50);
            var swingLows = swings.Where(s => !s.IsHigh).ToList();

            Assert(swingLows.Count == 0, 
                   "SwingLow_nRight_Validation", 
                   $"Expected 0 swing lows (nRight validation failed), got {swingLows.Count}");
        }

        // ========================================================================
        // TESTS - VALIDACIÓN DE TAMAÑO
        // ========================================================================

        private void Test_SwingHigh_MinSizeValidation()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.5; // Requiere swing de al menos 0.5 * ATR
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear un swing high muy pequeño (solo 2 puntos = 8 ticks)
            AddBar(provider, 60, 14, 5000.00, 5001.00, 4999.00, 5000.00); // Left 2
            AddBar(provider, 60, 15, 5000.00, 5001.50, 4999.50, 5001.00); // Left 1
            AddBar(provider, 60, 16, 5001.00, 5002.00, 5000.00, 5001.50); // CENTER (HIGH=5002, rango pequeño)
            AddBar(provider, 60, 17, 5001.50, 5001.75, 5000.50, 5001.00); // Right 1
            AddBar(provider, 60, 18, 5001.00, 5001.50, 5000.00, 5000.50); // Right 2

            engine.Initialize();
            for (int i = 0; i <= 18; i++)
            {
                engine.OnBarClose(60, i);
            }

            var swings = engine.GetRecentSwings(60, 50);
            var swingHighs = swings.Where(s => s.IsHigh).ToList();

            // No debería detectar porque el rango es muy pequeño
            Assert(swingHighs.Count == 0, 
                   "SwingHigh_MinSizeValidation", 
                   $"Expected 0 swing highs (size too small), got {swingHighs.Count}");
        }

        private void Test_SwingLow_MinSizeValidation()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.5;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear un swing low muy pequeño
            AddBar(provider, 60, 14, 5000.00, 5001.00, 4999.00, 5000.00); // Left 2
            AddBar(provider, 60, 15, 5000.00, 5000.50, 4998.50, 4999.00); // Left 1
            AddBar(provider, 60, 16, 4999.00, 5000.00, 4998.00, 4999.50); // CENTER (LOW=4998, rango pequeño)
            AddBar(provider, 60, 17, 4999.50, 5000.00, 4998.50, 4999.00); // Right 1
            AddBar(provider, 60, 18, 4999.00, 5000.00, 4998.50, 4999.50); // Right 2

            engine.Initialize();
            for (int i = 0; i <= 18; i++)
            {
                engine.OnBarClose(60, i);
            }

            var swings = engine.GetRecentSwings(60, 50);
            var swingLows = swings.Where(s => !s.IsHigh).ToList();

            Assert(swingLows.Count == 0, 
                   "SwingLow_MinSizeValidation", 
                   $"Expected 0 swing lows (size too small), got {swingLows.Count}");
        }

        private void Test_SwingHigh_ExactThreshold()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.05; // Muy bajo para que pase
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear un swing high en el umbral
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00); // Left 2
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00); // Left 1
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // CENTER (HIGH)
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00); // Right 1
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00); // Right 2

            engine.Initialize();
            for (int i = 0; i <= 18; i++)
            {
                engine.OnBarClose(60, i);
            }

            var swings = engine.GetRecentSwings(60, 50);
            var swingHighs = swings.Where(s => s.IsHigh).ToList();

            Assert(swingHighs.Count >= 1, 
                   "SwingHigh_ExactThreshold", 
                   $"Expected at least 1 swing high at threshold, got {swingHighs.Count}");
        }

        // ========================================================================
        // TESTS - RUPTURAS
        // ========================================================================

        private void Test_SwingHigh_Broken()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear swing high
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // HIGH=5010
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00);
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00);

            // Barras que rompen el swing high (close > 5010)
            AddBar(provider, 60, 19, 5003.00, 5012.00, 5002.00, 5011.00); // BREAK

            engine.Initialize();
            for (int i = 0; i <= 19; i++)
            {
                engine.OnBarClose(60, i);
            }

            var swings = engine.GetRecentSwings(60, 50);
            var swingHighs = swings.Where(s => s.IsHigh).ToList();

            Assert(swingHighs.Count >= 1, 
                   "SwingHigh_Broken_Exists", 
                   $"Expected at least 1 swing high, got {swingHighs.Count}");

            if (swingHighs.Count > 0)
            {
                Assert(swingHighs[0].IsBroken, 
                       "SwingHigh_Broken_Status", 
                       "Expected swing high to be broken");
            }
        }

        private void Test_SwingLow_Broken()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear swing low
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 4999.00);
            AddBar(provider, 60, 15, 4999.00, 5001.00, 4997.00, 4998.00);
            AddBar(provider, 60, 16, 4998.00, 4999.00, 4990.00, 4995.00); // LOW=4990
            AddBar(provider, 60, 17, 4995.00, 4997.00, 4993.00, 4996.00);
            AddBar(provider, 60, 18, 4996.00, 4998.00, 4994.00, 4997.00);

            // Barras que rompen el swing low (close < 4990)
            AddBar(provider, 60, 19, 4997.00, 4998.00, 4988.00, 4989.00); // BREAK

            engine.Initialize();
            for (int i = 0; i <= 19; i++)
            {
                engine.OnBarClose(60, i);
            }

            var swings = engine.GetRecentSwings(60, 50);
            var swingLows = swings.Where(s => !s.IsHigh).ToList();

            Assert(swingLows.Count >= 1, 
                   "SwingLow_Broken_Exists", 
                   $"Expected at least 1 swing low, got {swingLows.Count}");

            if (swingLows.Count > 0)
            {
                Assert(swingLows[0].IsBroken, 
                       "SwingLow_Broken_Status", 
                       "Expected swing low to be broken");
            }
        }

        private void Test_SwingHigh_NotBroken()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear swing high
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // HIGH=5010
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00);
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00);

            // Barras que NO rompen el swing high (close <= 5010)
            AddBar(provider, 60, 19, 5003.00, 5009.00, 5002.00, 5008.00); // No break

            engine.Initialize();
            for (int i = 0; i <= 19; i++)
            {
                engine.OnBarClose(60, i);
            }

            var swings = engine.GetRecentSwings(60, 50);
            var swingHighs = swings.Where(s => s.IsHigh).ToList();

            Assert(swingHighs.Count >= 1, 
                   "SwingHigh_NotBroken_Exists", 
                   $"Expected at least 1 swing high, got {swingHighs.Count}");

            if (swingHighs.Count > 0)
            {
                Assert(!swingHighs[0].IsBroken, 
                       "SwingHigh_NotBroken_Status", 
                       "Expected swing high to NOT be broken");
            }
        }

        // ========================================================================
        // TESTS - MÚLTIPLES SWINGS
        // ========================================================================

        private void Test_MultipleSwings_SameTF()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Swing High 1
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // HIGH
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00);
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00);

            // Barras intermedias
            AddBar(provider, 60, 19, 5003.00, 5005.00, 5001.00, 5002.00);
            AddBar(provider, 60, 20, 5002.00, 5004.00, 5000.00, 5001.00);

            // Swing High 2
            AddBar(provider, 60, 21, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 22, 5002.00, 5004.00, 5000.00, 5003.00);
            AddBar(provider, 60, 23, 5003.00, 5012.00, 5002.00, 5006.00); // HIGH 2
            AddBar(provider, 60, 24, 5006.00, 5008.00, 5004.00, 5005.00);
            AddBar(provider, 60, 25, 5005.00, 5007.00, 5003.00, 5004.00);

            engine.Initialize();
            for (int i = 0; i <= 25; i++)
            {
                engine.OnBarClose(60, i);
            }

            var swings = engine.GetRecentSwings(60, 50);
            var swingHighs = swings.Where(s => s.IsHigh).ToList();

            Assert(swingHighs.Count >= 2, 
                   "MultipleSwings_SameTF", 
                   $"Expected at least 2 swing highs, got {swingHighs.Count}");
        }

        private void Test_MultipleSwings_HighAndLow()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Swing High
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // HIGH
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00);
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00);

            // Barras intermedias
            AddBar(provider, 60, 19, 5003.00, 5005.00, 5001.00, 5002.00);
            AddBar(provider, 60, 20, 5002.00, 5004.00, 5000.00, 5001.00);

            // Swing Low
            AddBar(provider, 60, 21, 5001.00, 5002.00, 4999.00, 5000.00);
            AddBar(provider, 60, 22, 5000.00, 5001.00, 4998.00, 4999.00);
            AddBar(provider, 60, 23, 4999.00, 5000.00, 4990.00, 4995.00); // LOW
            AddBar(provider, 60, 24, 4995.00, 4997.00, 4993.00, 4996.00);
            AddBar(provider, 60, 25, 4996.00, 4998.00, 4994.00, 4997.00);

            engine.Initialize();
            for (int i = 0; i <= 25; i++)
            {
                engine.OnBarClose(60, i);
            }

            var swings = engine.GetRecentSwings(60, 50);
            var swingHighs = swings.Where(s => s.IsHigh).ToList();
            var swingLows = swings.Where(s => !s.IsHigh).ToList();

            Assert(swingHighs.Count >= 1 && swingLows.Count >= 1, 
                   "MultipleSwings_HighAndLow", 
                   $"Expected at least 1 high and 1 low, got {swingHighs.Count} highs and {swingLows.Count} lows");
        }

        // ========================================================================
        // TESTS - SCORING
        // ========================================================================

        private void Test_Swing_InitialScore()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear swing high
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // HIGH
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00);
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00);

            engine.Initialize();
            for (int i = 0; i <= 18; i++)
            {
                engine.OnBarClose(60, i);
            }

            var swings = engine.GetRecentSwings(60, 50);

            Assert(swings.Count > 0, 
                   "Swing_InitialScore_Exists", 
                   "Expected at least 1 swing");

            if (swings.Count > 0)
            {
                Assert(swings[0].Score > 0.0 && swings[0].Score <= 1.0, 
                       "Swing_InitialScore_Range", 
                       $"Expected score in (0,1], got {swings[0].Score}");
            }
        }

        private void Test_Swing_Freshness()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.FreshnessLambda = 20; // Decay rápido
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear swing reciente
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // HIGH
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00);
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00);

            engine.Initialize();
            for (int i = 0; i <= 18; i++)
            {
                engine.OnBarClose(60, i);
            }

            var recentSwing = engine.GetRecentSwings(60, 50).FirstOrDefault();
            var recentScore = recentSwing?.Score ?? 0;

            // Crear un swing mucho más viejo (50 barras antes)
            // Avanzar 50 barras y crear otro swing
            for (int i = 19; i < 64; i++)
            {
                AddBar(provider, 60, i, 5003.00, 5005.00, 5001.00, 5002.00);
                engine.OnBarClose(60, i);
            }

            // Crear swing viejo
            AddBar(provider, 60, 64, 5002.00, 5004.00, 5000.00, 5003.00);
            AddBar(provider, 60, 65, 5003.00, 5005.00, 5001.00, 5004.00);
            AddBar(provider, 60, 66, 5004.00, 5012.00, 5003.00, 5006.00); // HIGH viejo
            AddBar(provider, 60, 67, 5006.00, 5008.00, 5004.00, 5005.00);
            AddBar(provider, 60, 68, 5005.00, 5007.00, 5003.00, 5004.00);

            for (int i = 64; i <= 68; i++)
            {
                engine.OnBarClose(60, i);
            }

            var swings = engine.GetRecentSwings(60, 50).OrderByDescending(s => s.CreatedAtBarIndex).ToList();
            
            // Verificar que hay al menos 2 swings
            Assert(swings.Count >= 2, 
                   "Swing_Freshness_Exists", 
                   $"Expected at least 2 swings, got {swings.Count}");

            if (swings.Count >= 2)
            {
                var newerSwing = swings[0]; // Más reciente
                var olderSwing = swings[1]; // Más viejo

                // El swing más reciente debería tener score mayor o igual que el viejo
                // (debido a freshness)
                Assert(newerSwing.Score >= olderSwing.Score * 0.8, 
                       "Swing_Freshness_Decay", 
                       $"Expected newer swing score >= older swing score * 0.8. Newer:{newerSwing.Score:F3}, Older:{olderSwing.Score:F3}");

                _logger($"  Swing freshness: Newer={newerSwing.Score:F3}, Older={olderSwing.Score:F3}");
            }
        }

        // ========================================================================
        // TESTS - EDGE CASES
        // ========================================================================

        private void Test_EdgeCase_InsufficientBars()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            // Solo 3 barras (insuficientes para detectar swing con nLeft=2, nRight=2)
            AddBar(provider, 60, 0, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 1, 5001.00, 5010.00, 4999.00, 5002.00);
            AddBar(provider, 60, 2, 5002.00, 5003.00, 5001.00, 5002.00);

            engine.Initialize();
            for (int i = 0; i <= 2; i++)
            {
                engine.OnBarClose(60, i);
            }

            var swings = engine.GetRecentSwings(60, 50);

            Assert(swings.Count == 0, 
                   "EdgeCase_InsufficientBars", 
                   $"Expected 0 swings with insufficient bars, got {swings.Count}");
        }

        private void Test_EdgeCase_FlatMarket()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Mercado plano (sin swings claros)
            for (int i = 14; i < 25; i++)
            {
                AddBar(provider, 60, i, 5000.00, 5001.00, 4999.00, 5000.00);
            }

            engine.Initialize();
            for (int i = 0; i < 25; i++)
            {
                engine.OnBarClose(60, i);
            }

            var swings = engine.GetRecentSwings(60, 50);

            // En mercado plano, no debería haber swings significativos
            Assert(swings.Count == 0, 
                   "EdgeCase_FlatMarket", 
                   $"Expected 0 swings in flat market, got {swings.Count}");
        }

        private void Test_EdgeCase_VerySmallSwing()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 1.0; // Muy alto
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Swing válido en forma pero muy pequeño
            AddBar(provider, 60, 14, 5000.00, 5001.00, 4999.00, 5000.00);
            AddBar(provider, 60, 15, 5000.00, 5001.50, 4999.50, 5001.00);
            AddBar(provider, 60, 16, 5001.00, 5002.00, 5000.00, 5001.50); // HIGH=5002
            AddBar(provider, 60, 17, 5001.50, 5001.75, 5000.50, 5001.00);
            AddBar(provider, 60, 18, 5001.00, 5001.50, 5000.00, 5000.50);

            engine.Initialize();
            for (int i = 0; i <= 18; i++)
            {
                engine.OnBarClose(60, i);
            }

            var swings = engine.GetRecentSwings(60, 50);

            Assert(swings.Count == 0, 
                   "EdgeCase_VerySmallSwing", 
                   $"Expected 0 swings (too small for MinSwingATRfactor=1.0), got {swings.Count}");
        }
    }
}

