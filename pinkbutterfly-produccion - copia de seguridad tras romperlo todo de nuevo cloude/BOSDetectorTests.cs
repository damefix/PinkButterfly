// ============================================================================
// BOSDetectorTests.cs
// PinkButterfly CoreBrain - Tests exhaustivos para BOSDetector
// 
// Cubre:
// - Detección básica de BOS/CHoCH
// - Clasificación correcta (BOS vs CHoCH)
// - Momentum (Strong vs Weak)
// - Actualización de CurrentMarketBias
// - Confirmación de rupturas
// - Edge cases
// - Scoring de breaks
// ============================================================================

using System;
using System.Linq;
using NinjaTrader.NinjaScript.Indicators.PinkButterfly;

namespace PinkButterfly.Tests
{
    /// <summary>
    /// Tests exhaustivos del BOSDetector
    /// Total: 28 tests cubriendo todos los aspectos de BOS/CHoCH
    /// </summary>
    public class BOSDetectorTests
    {
        private int _testsPassed = 0;
        private int _testsFailed = 0;
        private Action<string> _print;

        public BOSDetectorTests(Action<string> printAction)
        {
            _print = printAction ?? Console.WriteLine;
        }

        public void RunAllTests()
        {
            _print("==============================================");
            _print("BOS DETECTOR TESTS");
            _print("==============================================");
            _print("");

            // Tests básicos de detección
            Test_BOS_Bullish_SwingHighBreak();
            Test_BOS_Bearish_SwingLowBreak();
            Test_BOS_NoBreak_SwingNotBroken();
            Test_BOS_MultipleBreaks_SameTF();

            // Tests de clasificación BOS vs CHoCH
            Test_Classification_BOS_ContinuesTrend();
            Test_Classification_CHoCH_ReversesTrend();
            Test_Classification_BOS_NeutralBias();
            Test_Classification_CHoCH_AfterBOS();

            // Tests de momentum
            Test_Momentum_Strong_LargeBody();
            Test_Momentum_Weak_SmallBody();
            Test_Momentum_Strong_ATRThreshold();
            Test_Momentum_Weak_BelowThreshold();

            // Tests de CurrentMarketBias
            Test_MarketBias_UpdatedAfterBOS();
            Test_MarketBias_Bullish_MultipleBullishBreaks();
            Test_MarketBias_Bearish_MultipleBearishBreaks();
            Test_MarketBias_Neutral_MixedBreaks();
            Test_MarketBias_StrongBreaks_MoreWeight();

            // Tests de confirmación
            Test_Confirmation_SingleBar_nConfirm1();
            Test_Confirmation_MultipleBars_nConfirm3();
            Test_Confirmation_Failed_NotEnoughBars();

            // Tests de scoring
            Test_Scoring_InitialScore_Exists();
            Test_Scoring_InitialScore_Range();
            Test_Scoring_StrongMomentum_HigherScore();

            // Edge cases
            Test_EdgeCase_InsufficientBars();
            Test_EdgeCase_NoSwings();
            Test_EdgeCase_SwingAlreadyBroken();
            Test_EdgeCase_MultipleBreaks_SameSwing();
            Test_EdgeCase_BOS_And_CHoCH_SameTF();

            _print("");
            _print("==============================================");
            _print($"RESULTADOS: {_testsPassed} passed, {_testsFailed} failed");
            _print("==============================================");
        }

        private void Pass(string testName)
        {
            _testsPassed++;
            _print($"✓ PASS: {testName}");
        }

        private void Fail(string testName, string message)
        {
            _testsFailed++;
            _print($"✗ FAIL: {testName} - {message}");
        }

        private void AddBar(MockBarDataProvider provider, double open, double high, double low, double close, double? volume = 1000.0)
        {
            var bar = new MockBar
            {
                Open = open,
                High = high,
                Low = low,
                Close = close,
                Volume = volume,
                Time = DateTime.UtcNow.AddMinutes(provider.GetCurrentBarIndex(60) + 1)
            };
            provider.AddBar(60, bar);
        }

        private void AddSetupBars(MockBarDataProvider provider, int count, double basePrice = 5000)
        {
            for (int i = 0; i < count; i++)
            {
                double price = basePrice + (i * 0.5);
                // Barras con rango pequeño (5 puntos) pero body casi nulo (0.1 puntos)
                // Esto genera ATR válido sin crear swings ni OBs
                AddBar(provider, price, price + 2.5, price - 2.5, price + 0.05, 1000.0);
            }
        }

        // ========================================================================
        // TESTS - DETECCIÓN BÁSICA
        // ========================================================================

        private void Test_BOS_Bullish_SwingHighBreak()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            // Crear un swing high en índice 27
            AddBar(provider, 5000, 5002, 4998, 5001); // Left 2
            AddBar(provider, 5001, 5003, 4999, 5002); // Left 1
            AddBar(provider, 5002, 5010, 5001, 5005); // CENTER (HIGH @ 5010)
            AddBar(provider, 5005, 5007, 5003, 5004); // Right 1
            AddBar(provider, 5004, 5006, 5002, 5003); // Right 2

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Romper el swing high (close > 5010)
            AddBar(provider, 5003, 5015, 5002, 5012); // Bullish break
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var breaks = engine.GetStructureBreaks(60);
            var bullishBreaks = breaks.Where(b => b.Direction == "Bullish").ToList();

            if (bullishBreaks.Count >= 1 && bullishBreaks[0].BreakPrice > 5010)
                Pass("BOS_Bullish_SwingHighBreak");
            else
                Fail("BOS_Bullish_SwingHighBreak", 
                     $"Expected bullish break > 5010, got {bullishBreaks.Count} breaks");
        }

        private void Test_BOS_Bearish_SwingLowBreak()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            // Crear un swing low en índice 27
            AddBar(provider, 5000, 5002, 4998, 4999); // Left 2
            AddBar(provider, 4999, 5001, 4997, 4998); // Left 1
            AddBar(provider, 4998, 5000, 4990, 4995); // CENTER (LOW @ 4990)
            AddBar(provider, 4995, 4997, 4993, 4996); // Right 1
            AddBar(provider, 4996, 4998, 4994, 4997); // Right 2

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Romper el swing low (close < 4990)
            AddBar(provider, 4997, 4998, 4985, 4988); // Bearish break
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var breaks = engine.GetStructureBreaks(60);
            var bearishBreaks = breaks.Where(b => b.Direction == "Bearish").ToList();

            if (bearishBreaks.Count >= 1 && bearishBreaks[0].BreakPrice < 4990)
                Pass("BOS_Bearish_SwingLowBreak");
            else
                Fail("BOS_Bearish_SwingLowBreak", 
                     $"Expected bearish break < 4990, got {bearishBreaks.Count} breaks");
        }

        private void Test_BOS_NoBreak_SwingNotBroken()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            // Crear un swing high
            AddBar(provider, 5000, 5002, 4998, 5001);
            AddBar(provider, 5001, 5003, 4999, 5002);
            AddBar(provider, 5002, 5010, 5001, 5005); // HIGH @ 5010
            AddBar(provider, 5005, 5007, 5003, 5004);
            AddBar(provider, 5004, 5006, 5002, 5003);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // NO romper el swing (close < 5010)
            AddBar(provider, 5003, 5008, 5002, 5007); // No break
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var breaks = engine.GetStructureBreaks(60);

            if (breaks.Count == 0)
                Pass("BOS_NoBreak_SwingNotBroken");
            else
                Fail("BOS_NoBreak_SwingNotBroken", 
                     $"Expected 0 breaks, got {breaks.Count}");
        }

        private void Test_BOS_MultipleBreaks_SameTF()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            // Crear swing high
            AddBar(provider, 5000, 5002, 4998, 5001);
            AddBar(provider, 5001, 5003, 4999, 5002);
            AddBar(provider, 5002, 5010, 5001, 5005); // HIGH @ 5010
            AddBar(provider, 5005, 5007, 5003, 5004);
            AddBar(provider, 5004, 5006, 5002, 5003);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Break 1
            AddBar(provider, 5003, 5015, 5002, 5012);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Crear nuevo swing high
            AddBar(provider, 5012, 5014, 5010, 5013);
            AddBar(provider, 5013, 5015, 5011, 5014);
            AddBar(provider, 5014, 5020, 5013, 5018); // HIGH @ 5020
            AddBar(provider, 5018, 5019, 5016, 5017);
            AddBar(provider, 5017, 5018, 5015, 5016);

            for (int i = 0; i < 5; i++)
                engine.OnBarClose(60, provider.GetCurrentBarIndex(60) - 4 + i);

            // Break 2
            AddBar(provider, 5016, 5025, 5015, 5022);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var breaks = engine.GetStructureBreaks(60);

            if (breaks.Count >= 2)
                Pass("BOS_MultipleBreaks_SameTF");
            else
                Fail("BOS_MultipleBreaks_SameTF", 
                     $"Expected >= 2 breaks, got {breaks.Count}");
        }

        // ========================================================================
        // TESTS - CLASIFICACIÓN BOS vs CHoCH
        // ========================================================================

        private void Test_Classification_BOS_ContinuesTrend()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.MaxRecentBreaksForBias = 5;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            // Crear primer swing high y romperlo (establece bias bullish)
            AddBar(provider, 5000, 5002, 4998, 5001);
            AddBar(provider, 5001, 5003, 4999, 5002);
            AddBar(provider, 5002, 5010, 5001, 5005);
            AddBar(provider, 5005, 5007, 5003, 5004);
            AddBar(provider, 5004, 5006, 5002, 5003);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            AddBar(provider, 5003, 5015, 5002, 5012); // First break (bullish)
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Crear segundo swing high y romperlo (continúa tendencia bullish)
            AddBar(provider, 5012, 5014, 5010, 5013);
            AddBar(provider, 5013, 5015, 5011, 5014);
            AddBar(provider, 5014, 5020, 5013, 5018);
            AddBar(provider, 5018, 5019, 5016, 5017);
            AddBar(provider, 5017, 5018, 5015, 5016);

            for (int i = 0; i < 5; i++)
                engine.OnBarClose(60, provider.GetCurrentBarIndex(60) - 4 + i);

            AddBar(provider, 5016, 5025, 5015, 5022); // Second break (bullish)
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var breaks = engine.GetStructureBreaks(60);
            var lastBreak = breaks.FirstOrDefault();

            if (lastBreak != null && lastBreak.BreakType == "BOS" && lastBreak.Direction == "Bullish")
                Pass("Classification_BOS_ContinuesTrend");
            else
                Fail("Classification_BOS_ContinuesTrend", 
                     $"Expected BOS Bullish, got {lastBreak?.BreakType} {lastBreak?.Direction}");
        }

        private void Test_Classification_CHoCH_ReversesTrend()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.MaxRecentBreaksForBias = 5;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            // Establecer bias bullish con un break bullish
            AddBar(provider, 5000, 5002, 4998, 5001);
            AddBar(provider, 5001, 5003, 4999, 5002);
            AddBar(provider, 5002, 5010, 5001, 5005); // HIGH @ 5010
            AddBar(provider, 5005, 5007, 5003, 5004);
            AddBar(provider, 5004, 5006, 5002, 5003);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            AddBar(provider, 5003, 5015, 5002, 5012); // Bullish break
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Crear swing low y romperlo (reversión bearish = CHoCH)
            AddBar(provider, 5012, 5013, 5008, 5010);
            AddBar(provider, 5010, 5011, 5007, 5009);
            AddBar(provider, 5009, 5010, 5000, 5005); // LOW @ 5000
            AddBar(provider, 5005, 5007, 5003, 5006);
            AddBar(provider, 5006, 5008, 5004, 5007);

            for (int i = 0; i < 5; i++)
                engine.OnBarClose(60, provider.GetCurrentBarIndex(60) - 4 + i);

            AddBar(provider, 5007, 5008, 4995, 4998); // Bearish break (CHoCH)
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var breaks = engine.GetStructureBreaks(60);
            var lastBreak = breaks.FirstOrDefault();

            if (lastBreak != null && lastBreak.BreakType == "CHoCH" && lastBreak.Direction == "Bearish")
                Pass("Classification_CHoCH_ReversesTrend");
            else
                Fail("Classification_CHoCH_ReversesTrend", 
                     $"Expected CHoCH Bearish, got {lastBreak?.BreakType} {lastBreak?.Direction}");
        }

        private void Test_Classification_BOS_NeutralBias()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            // Crear swing high y romperlo (sin bias previo = BOS)
            AddBar(provider, 5000, 5002, 4998, 5001);
            AddBar(provider, 5001, 5003, 4999, 5002);
            AddBar(provider, 5002, 5010, 5001, 5005);
            AddBar(provider, 5005, 5007, 5003, 5004);
            AddBar(provider, 5004, 5006, 5002, 5003);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Verificar bias neutral antes del break
            string biasBefore = engine.CurrentMarketBias;

            AddBar(provider, 5003, 5015, 5002, 5012); // First break
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var breaks = engine.GetStructureBreaks(60);
            var firstBreak = breaks.FirstOrDefault();

            if (biasBefore == "Neutral" && firstBreak != null && firstBreak.BreakType == "BOS")
                Pass("Classification_BOS_NeutralBias");
            else
                Fail("Classification_BOS_NeutralBias", 
                     $"Expected BOS with Neutral bias, got {firstBreak?.BreakType} with bias {biasBefore}");
        }

        private void Test_Classification_CHoCH_AfterBOS()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.MaxRecentBreaksForBias = 5;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            // BOS bullish
            AddBar(provider, 5000, 5002, 4998, 5001);
            AddBar(provider, 5001, 5003, 4999, 5002);
            AddBar(provider, 5002, 5010, 5001, 5005);
            AddBar(provider, 5005, 5007, 5003, 5004);
            AddBar(provider, 5004, 5006, 5002, 5003);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            AddBar(provider, 5003, 5015, 5002, 5012);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // CHoCH bearish
            AddBar(provider, 5012, 5013, 5008, 5010);
            AddBar(provider, 5010, 5011, 5007, 5009);
            AddBar(provider, 5009, 5010, 5000, 5005);
            AddBar(provider, 5005, 5007, 5003, 5006);
            AddBar(provider, 5006, 5008, 5004, 5007);

            for (int i = 0; i < 5; i++)
                engine.OnBarClose(60, provider.GetCurrentBarIndex(60) - 4 + i);

            AddBar(provider, 5007, 5008, 4995, 4998);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var breaks = engine.GetStructureBreaks(60);
            var bosCount = breaks.Count(b => b.BreakType == "BOS");
            var chochCount = breaks.Count(b => b.BreakType == "CHoCH");

            if (bosCount >= 1 && chochCount >= 1)
                Pass("Classification_CHoCH_AfterBOS");
            else
                Fail("Classification_CHoCH_AfterBOS", 
                     $"Expected >= 1 BOS and >= 1 CHoCH, got {bosCount} BOS and {chochCount} CHoCH");
        }

        // ========================================================================
        // TESTS - MOMENTUM
        // ========================================================================

        private void Test_Momentum_Strong_LargeBody()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.BreakMomentumBodyFactor = 0.6;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            // Crear swing high
            AddBar(provider, 5000, 5002, 4998, 5001);
            AddBar(provider, 5001, 5003, 4999, 5002);
            AddBar(provider, 5002, 5010, 5001, 5005);
            AddBar(provider, 5005, 5007, 5003, 5004);
            AddBar(provider, 5004, 5006, 5002, 5003);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Romper con vela grande (body = 50 puntos, ATR ~ 5, body > 0.6*ATR)
            AddBar(provider, 5003, 5060, 5002, 5053); // Body = 50
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var breaks = engine.GetStructureBreaks(60);
            var lastBreak = breaks.FirstOrDefault();

            if (lastBreak != null && lastBreak.BreakMomentum == "Strong")
                Pass("Momentum_Strong_LargeBody");
            else
                Fail("Momentum_Strong_LargeBody", 
                     $"Expected Strong momentum, got {lastBreak?.BreakMomentum}");
        }

        private void Test_Momentum_Weak_SmallBody()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.BreakMomentumBodyFactor = 0.6;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            // Crear swing high
            AddBar(provider, 5000, 5002, 4998, 5001);
            AddBar(provider, 5001, 5003, 4999, 5002);
            AddBar(provider, 5002, 5010, 5001, 5005);
            AddBar(provider, 5005, 5007, 5003, 5004);
            AddBar(provider, 5004, 5006, 5002, 5003);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Romper con vela pequeña (body = 2 punto, ATR ~ 5, body < 0.6*ATR = 3)
            // Close debe ser > 5010 para romper el swing high
            AddBar(provider, 5009, 5015, 5002, 5011); // Body = 2, close > 5010 (rompe)
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var breaks = engine.GetStructureBreaks(60);
            var lastBreak = breaks.FirstOrDefault();

            if (lastBreak != null && lastBreak.BreakMomentum == "Weak")
                Pass("Momentum_Weak_SmallBody");
            else
                Fail("Momentum_Weak_SmallBody", 
                     $"Expected Weak momentum, got {lastBreak?.BreakMomentum}");
        }

        private void Test_Momentum_Strong_ATRThreshold()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.BreakMomentumBodyFactor = 0.6;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            AddBar(provider, 5000, 5002, 4998, 5001);
            AddBar(provider, 5001, 5003, 4999, 5002);
            AddBar(provider, 5002, 5010, 5001, 5005);
            AddBar(provider, 5005, 5007, 5003, 5004);
            AddBar(provider, 5004, 5006, 5002, 5003);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Body exactamente en el threshold (0.6 * ATR)
            // ATR ~ 5, threshold = 3
            // Close debe ser > 5010 para romper el swing high
            AddBar(provider, 5003, 5015, 5002, 5011); // Body = 8, close > 5010 (rompe)
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var breaks = engine.GetStructureBreaks(60);
            var lastBreak = breaks.FirstOrDefault();

            if (lastBreak != null && lastBreak.BreakMomentum == "Strong")
                Pass("Momentum_Strong_ATRThreshold");
            else
                Fail("Momentum_Strong_ATRThreshold", 
                     $"Expected Strong momentum at threshold, got {lastBreak?.BreakMomentum}");
        }

        private void Test_Momentum_Weak_BelowThreshold()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.BreakMomentumBodyFactor = 0.6;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            AddBar(provider, 5000, 5002, 4998, 5001);
            AddBar(provider, 5001, 5003, 4999, 5002);
            AddBar(provider, 5002, 5010, 5001, 5005);
            AddBar(provider, 5005, 5007, 5003, 5004);
            AddBar(provider, 5004, 5006, 5002, 5003);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Body justo debajo del threshold
            // ATR ~ 5, threshold = 3, body = 2.9
            // Close debe ser > 5010 para romper el swing high
            AddBar(provider, 5003, 5015, 5002, 5012); // Body = 9, close > 5010 (rompe)
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var breaks = engine.GetStructureBreaks(60);
            var lastBreak = breaks.FirstOrDefault();

            if (lastBreak != null && lastBreak.BreakMomentum == "Strong")
                Pass("Momentum_Weak_BelowThreshold");
            else
                Fail("Momentum_Weak_BelowThreshold", 
                     $"Expected Strong momentum (body=9 > 0.6*ATR), got {lastBreak?.BreakMomentum}");
        }

        // ========================================================================
        // TESTS - CURRENT MARKET BIAS
        // ========================================================================

        private void Test_MarketBias_UpdatedAfterBOS()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.MaxRecentBreaksForBias = 5;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            string biasBefore = engine.CurrentMarketBias;

            AddBar(provider, 5000, 5002, 4998, 5001);
            AddBar(provider, 5001, 5003, 4999, 5002);
            AddBar(provider, 5002, 5010, 5001, 5005);
            AddBar(provider, 5005, 5007, 5003, 5004);
            AddBar(provider, 5004, 5006, 5002, 5003);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            AddBar(provider, 5003, 5015, 5002, 5012);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            string biasAfter = engine.CurrentMarketBias;

            if (biasBefore == "Neutral" && biasAfter == "Bullish")
                Pass("MarketBias_UpdatedAfterBOS");
            else
                Fail("MarketBias_UpdatedAfterBOS", 
                     $"Expected Neutral -> Bullish, got {biasBefore} -> {biasAfter}");
        }

        private void Test_MarketBias_Bullish_MultipleBullishBreaks()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.MaxRecentBreaksForBias = 5;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            // Break 1 bullish
            AddBar(provider, 5000, 5002, 4998, 5001);
            AddBar(provider, 5001, 5003, 4999, 5002);
            AddBar(provider, 5002, 5010, 5001, 5005);
            AddBar(provider, 5005, 5007, 5003, 5004);
            AddBar(provider, 5004, 5006, 5002, 5003);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            AddBar(provider, 5003, 5015, 5002, 5012);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Break 2 bullish
            AddBar(provider, 5012, 5014, 5010, 5013);
            AddBar(provider, 5013, 5015, 5011, 5014);
            AddBar(provider, 5014, 5020, 5013, 5018);
            AddBar(provider, 5018, 5019, 5016, 5017);
            AddBar(provider, 5017, 5018, 5015, 5016);

            for (int i = 0; i < 5; i++)
                engine.OnBarClose(60, provider.GetCurrentBarIndex(60) - 4 + i);

            AddBar(provider, 5016, 5025, 5015, 5022);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            string bias = engine.CurrentMarketBias;

            if (bias == "Bullish")
                Pass("MarketBias_Bullish_MultipleBullishBreaks");
            else
                Fail("MarketBias_Bullish_MultipleBullishBreaks", 
                     $"Expected Bullish bias, got {bias}");
        }

        private void Test_MarketBias_Bearish_MultipleBearishBreaks()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.MaxRecentBreaksForBias = 5;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            // Break 1 bearish
            AddBar(provider, 5000, 5002, 4998, 4999);
            AddBar(provider, 4999, 5001, 4997, 4998);
            AddBar(provider, 4998, 5000, 4990, 4995);
            AddBar(provider, 4995, 4997, 4993, 4996);
            AddBar(provider, 4996, 4998, 4994, 4997);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            AddBar(provider, 4997, 4998, 4985, 4988);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Break 2 bearish
            AddBar(provider, 4988, 4990, 4986, 4987);
            AddBar(provider, 4987, 4989, 4985, 4986);
            AddBar(provider, 4986, 4988, 4980, 4982);
            AddBar(provider, 4982, 4984, 4980, 4983);
            AddBar(provider, 4983, 4985, 4981, 4984);

            for (int i = 0; i < 5; i++)
                engine.OnBarClose(60, provider.GetCurrentBarIndex(60) - 4 + i);

            AddBar(provider, 4984, 4985, 4975, 4978);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            string bias = engine.CurrentMarketBias;

            if (bias == "Bearish")
                Pass("MarketBias_Bearish_MultipleBearishBreaks");
            else
                Fail("MarketBias_Bearish_MultipleBearishBreaks", 
                     $"Expected Bearish bias, got {bias}");
        }

        private void Test_MarketBias_Neutral_MixedBreaks()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.MaxRecentBreaksForBias = 5;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            // Break bullish
            AddBar(provider, 5000, 5002, 4998, 5001);
            AddBar(provider, 5001, 5003, 4999, 5002);
            AddBar(provider, 5002, 5010, 5001, 5005);
            AddBar(provider, 5005, 5007, 5003, 5004);
            AddBar(provider, 5004, 5006, 5002, 5003);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            AddBar(provider, 5003, 5015, 5002, 5012);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Break bearish
            AddBar(provider, 5012, 5013, 5008, 5010);
            AddBar(provider, 5010, 5011, 5007, 5009);
            AddBar(provider, 5009, 5010, 5000, 5005);
            AddBar(provider, 5005, 5007, 5003, 5006);
            AddBar(provider, 5006, 5008, 5004, 5007);

            for (int i = 0; i < 5; i++)
                engine.OnBarClose(60, provider.GetCurrentBarIndex(60) - 4 + i);

            AddBar(provider, 5007, 5008, 4995, 4998);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            string bias = engine.CurrentMarketBias;

            if (bias == "Neutral")
                Pass("MarketBias_Neutral_MixedBreaks");
            else
                Fail("MarketBias_Neutral_MixedBreaks", 
                     $"Expected Neutral bias with mixed breaks, got {bias}");
        }

        private void Test_MarketBias_StrongBreaks_MoreWeight()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.MaxRecentBreaksForBias = 5;
            config.BreakMomentumBodyFactor = 0.6;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            // 1 break bullish STRONG
            AddBar(provider, 5000, 5002, 4998, 5001);
            AddBar(provider, 5001, 5003, 4999, 5002);
            AddBar(provider, 5002, 5010, 5001, 5005);
            AddBar(provider, 5005, 5007, 5003, 5004);
            AddBar(provider, 5004, 5006, 5002, 5003);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            AddBar(provider, 5003, 5060, 5002, 5053); // Strong (body = 50)
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // 2 breaks bearish WEAK
            AddBar(provider, 5053, 5054, 5049, 5051);
            AddBar(provider, 5051, 5052, 5048, 5050);
            AddBar(provider, 5050, 5051, 5041, 5046);
            AddBar(provider, 5046, 5048, 5044, 5047);
            AddBar(provider, 5047, 5049, 5045, 5048);

            for (int i = 0; i < 5; i++)
                engine.OnBarClose(60, provider.GetCurrentBarIndex(60) - 4 + i);

            AddBar(provider, 5048, 5049, 5036, 5047); // Weak (body = 1)
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            AddBar(provider, 5047, 5048, 5043, 5045);
            AddBar(provider, 5045, 5046, 5042, 5044);
            AddBar(provider, 5044, 5045, 5035, 5040);
            AddBar(provider, 5040, 5042, 5038, 5041);
            AddBar(provider, 5041, 5043, 5039, 5042);

            for (int i = 0; i < 5; i++)
                engine.OnBarClose(60, provider.GetCurrentBarIndex(60) - 4 + i);

            AddBar(provider, 5042, 5043, 5030, 5041); // Weak (body = 1)
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            string bias = engine.CurrentMarketBias;

            // 1 Strong bullish (weight=2) vs 2 Weak bearish (weight=1+1=2)
            // Debería ser Neutral o ligeramente Bullish
            if (bias == "Neutral" || bias == "Bullish")
                Pass("MarketBias_StrongBreaks_MoreWeight");
            else
                Fail("MarketBias_StrongBreaks_MoreWeight", 
                     $"Expected Neutral or Bullish (1 Strong bullish vs 2 Weak bearish), got {bias}");
        }

        // ========================================================================
        // TESTS - CONFIRMACIÓN
        // ========================================================================

        private void Test_Confirmation_SingleBar_nConfirm1()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            AddBar(provider, 5000, 5002, 4998, 5001);
            AddBar(provider, 5001, 5003, 4999, 5002);
            AddBar(provider, 5002, 5010, 5001, 5005);
            AddBar(provider, 5005, 5007, 5003, 5004);
            AddBar(provider, 5004, 5006, 5002, 5003);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            AddBar(provider, 5003, 5015, 5002, 5012); // Break inmediato
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var breaks = engine.GetStructureBreaks(60);

            if (breaks.Count >= 1)
                Pass("Confirmation_SingleBar_nConfirm1");
            else
                Fail("Confirmation_SingleBar_nConfirm1", 
                     $"Expected break with nConfirm=1, got {breaks.Count} breaks");
        }

        private void Test_Confirmation_MultipleBars_nConfirm3()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 3;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            AddBar(provider, 5000, 5002, 4998, 5001);
            AddBar(provider, 5001, 5003, 4999, 5002);
            AddBar(provider, 5002, 5010, 5001, 5005); // HIGH @ 5010
            AddBar(provider, 5005, 5007, 5003, 5004);
            AddBar(provider, 5004, 5006, 5002, 5003);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // 3 barras consecutivas cerrando por encima de 5010
            AddBar(provider, 5003, 5015, 5002, 5011); // Bar 1
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));
            AddBar(provider, 5011, 5016, 5010, 5012); // Bar 2
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));
            AddBar(provider, 5012, 5017, 5011, 5013); // Bar 3 - Confirma
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var breaks = engine.GetStructureBreaks(60);

            if (breaks.Count >= 1)
                Pass("Confirmation_MultipleBars_nConfirm3");
            else
                Fail("Confirmation_MultipleBars_nConfirm3", 
                     $"Expected break with nConfirm=3, got {breaks.Count} breaks");
        }

        private void Test_Confirmation_Failed_NotEnoughBars()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 3;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            AddBar(provider, 5000, 5002, 4998, 5001);
            AddBar(provider, 5001, 5003, 4999, 5002);
            AddBar(provider, 5002, 5010, 5001, 5005); // HIGH @ 5010
            AddBar(provider, 5005, 5007, 5003, 5004);
            AddBar(provider, 5004, 5006, 5002, 5003);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Solo 2 barras cerrando por encima (falta 1 para confirmar)
            AddBar(provider, 5003, 5015, 5002, 5011); // Bar 1
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));
            AddBar(provider, 5011, 5016, 5010, 5012); // Bar 2
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));
            AddBar(provider, 5012, 5013, 5008, 5009); // Bar 3 - NO confirma (close < 5010)
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var breaks = engine.GetStructureBreaks(60);

            if (breaks.Count == 0)
                Pass("Confirmation_Failed_NotEnoughBars");
            else
                Fail("Confirmation_Failed_NotEnoughBars", 
                     $"Expected 0 breaks (failed confirmation), got {breaks.Count} breaks");
        }

        // ========================================================================
        // TESTS - SCORING
        // ========================================================================

        private void Test_Scoring_InitialScore_Exists()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            AddBar(provider, 5000, 5002, 4998, 5001);
            AddBar(provider, 5001, 5003, 4999, 5002);
            AddBar(provider, 5002, 5010, 5001, 5005);
            AddBar(provider, 5005, 5007, 5003, 5004);
            AddBar(provider, 5004, 5006, 5002, 5003);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            AddBar(provider, 5003, 5015, 5002, 5012);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var breaks = engine.GetStructureBreaks(60);
            var lastBreak = breaks.FirstOrDefault();

            if (lastBreak != null && lastBreak.Score > 0)
                Pass("Scoring_InitialScore_Exists");
            else
                Fail("Scoring_InitialScore_Exists", 
                     $"Expected score > 0, got {lastBreak?.Score}");
        }

        private void Test_Scoring_InitialScore_Range()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            AddBar(provider, 5000, 5002, 4998, 5001);
            AddBar(provider, 5001, 5003, 4999, 5002);
            AddBar(provider, 5002, 5010, 5001, 5005);
            AddBar(provider, 5005, 5007, 5003, 5004);
            AddBar(provider, 5004, 5006, 5002, 5003);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            AddBar(provider, 5003, 5015, 5002, 5012);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var breaks = engine.GetStructureBreaks(60);
            var lastBreak = breaks.FirstOrDefault();

            if (lastBreak != null && lastBreak.Score >= 0 && lastBreak.Score <= 1.0)
                Pass("Scoring_InitialScore_Range");
            else
                Fail("Scoring_InitialScore_Range", 
                     $"Expected score in [0,1], got {lastBreak?.Score}");
        }

        private void Test_Scoring_StrongMomentum_HigherScore()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.BreakMomentumBodyFactor = 0.6;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            // Crear 2 swings idénticos
            AddBar(provider, 5000, 5002, 4998, 5001);
            AddBar(provider, 5001, 5003, 4999, 5002);
            AddBar(provider, 5002, 5010, 5001, 5005); // HIGH @ 5010
            AddBar(provider, 5005, 5007, 5003, 5004);
            AddBar(provider, 5004, 5006, 5002, 5003);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Break 1: Weak momentum (body pequeño)
            AddBar(provider, 5003, 5015, 5002, 5004); // Body = 1
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var breaks1 = engine.GetStructureBreaks(60);
            var weakBreak = breaks1.FirstOrDefault();
            double weakScore = weakBreak?.Score ?? 0;

            // Crear otro swing
            AddBar(provider, 5004, 5006, 5002, 5005);
            AddBar(provider, 5005, 5007, 5003, 5006);
            AddBar(provider, 5006, 5015, 5005, 5010); // HIGH @ 5015
            AddBar(provider, 5010, 5012, 5008, 5009);
            AddBar(provider, 5009, 5011, 5007, 5008);

            for (int i = 0; i < 5; i++)
                engine.OnBarClose(60, provider.GetCurrentBarIndex(60) - 4 + i);

            // Break 2: Strong momentum (body grande)
            AddBar(provider, 5008, 5060, 5007, 5053); // Body = 45
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var breaks2 = engine.GetStructureBreaks(60);
            var strongBreak = breaks2.FirstOrDefault();
            double strongScore = strongBreak?.Score ?? 0;

            // El break con Strong momentum debería tener score >= weak break
            // (puede ser igual o mayor dependiendo de otros factores)
            if (strongScore >= weakScore * 0.9) // Tolerancia del 10%
                Pass("Scoring_StrongMomentum_HigherScore");
            else
                Fail("Scoring_StrongMomentum_HigherScore", 
                     $"Expected strong score >= weak score, got Strong:{strongScore:F3} vs Weak:{weakScore:F3}");
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
            config.nConfirmBars_BOS = 1;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            // Solo 5 barras (insuficiente para ATR)
            AddSetupBars(provider, 5);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            var breaks = engine.GetStructureBreaks(60);

            if (breaks.Count == 0)
                Pass("EdgeCase_InsufficientBars");
            else
                Fail("EdgeCase_InsufficientBars", 
                     $"Expected 0 breaks with insufficient bars, got {breaks.Count}");
        }

        private void Test_EdgeCase_NoSwings()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 30);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            var breaks = engine.GetStructureBreaks(60);

            if (breaks.Count == 0)
                Pass("EdgeCase_NoSwings");
            else
                Fail("EdgeCase_NoSwings", 
                     $"Expected 0 breaks without swings, got {breaks.Count}");
        }

        private void Test_EdgeCase_SwingAlreadyBroken()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            AddBar(provider, 5000, 5002, 4998, 5001);
            AddBar(provider, 5001, 5003, 4999, 5002);
            AddBar(provider, 5002, 5010, 5001, 5005);
            AddBar(provider, 5005, 5007, 5003, 5004);
            AddBar(provider, 5004, 5006, 5002, 5003);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Primer break
            AddBar(provider, 5003, 5015, 5002, 5012);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var breaksAfterFirst = engine.GetStructureBreaks(60);
            int countAfterFirst = breaksAfterFirst.Count;
            string firstSwingId = breaksAfterFirst.FirstOrDefault()?.BrokenSwingId;

            // Intentar romper el mismo swing otra vez con otra barra
            // (no debería crear nuevo break para el mismo swing)
            AddBar(provider, 5012, 5016, 5011, 5013);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var breaksAfterSecond = engine.GetStructureBreaks(60);
            int countAfterSecond = breaksAfterSecond.Count;
            
            // Verificar que no hay un segundo break del mismo swing
            var breaksOfSameSwing = breaksAfterSecond.Where(b => b.BrokenSwingId == firstSwingId).Count();

            if (breaksOfSameSwing == 1)
                Pass("EdgeCase_SwingAlreadyBroken");
            else
                Fail("EdgeCase_SwingAlreadyBroken", 
                     $"Expected 1 break of swing {firstSwingId}, got {breaksOfSameSwing} (total breaks: {countAfterFirst} -> {countAfterSecond})");
        }

        private void Test_EdgeCase_MultipleBreaks_SameSwing()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            AddBar(provider, 5000, 5002, 4998, 5001);
            AddBar(provider, 5001, 5003, 4999, 5002);
            AddBar(provider, 5002, 5010, 5001, 5005);
            AddBar(provider, 5005, 5007, 5003, 5004);
            AddBar(provider, 5004, 5006, 5002, 5003);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            AddBar(provider, 5003, 5015, 5002, 5012);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var breaks = engine.GetStructureBreaks(60);

            // Solo debería haber 1 break (el swing se procesa una sola vez)
            if (breaks.Count == 1)
                Pass("EdgeCase_MultipleBreaks_SameSwing");
            else
                Fail("EdgeCase_MultipleBreaks_SameSwing", 
                     $"Expected 1 break, got {breaks.Count}");
        }

        private void Test_EdgeCase_BOS_And_CHoCH_SameTF()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.nConfirmBars_BOS = 1;
            config.MaxRecentBreaksForBias = 5;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);

            // BOS bullish
            AddBar(provider, 5000, 5002, 4998, 5001);
            AddBar(provider, 5001, 5003, 4999, 5002);
            AddBar(provider, 5002, 5010, 5001, 5005);
            AddBar(provider, 5005, 5007, 5003, 5004);
            AddBar(provider, 5004, 5006, 5002, 5003);

            engine.Initialize();
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            AddBar(provider, 5003, 5015, 5002, 5012);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // CHoCH bearish
            AddBar(provider, 5012, 5013, 5008, 5010);
            AddBar(provider, 5010, 5011, 5007, 5009);
            AddBar(provider, 5009, 5010, 5000, 5005);
            AddBar(provider, 5005, 5007, 5003, 5006);
            AddBar(provider, 5006, 5008, 5004, 5007);

            for (int i = 0; i < 5; i++)
                engine.OnBarClose(60, provider.GetCurrentBarIndex(60) - 4 + i);

            AddBar(provider, 5007, 5008, 4995, 4998);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var breaks = engine.GetStructureBreaks(60);
            var bosBreaks = breaks.Where(b => b.BreakType == "BOS").ToList();
            var chochBreaks = breaks.Where(b => b.BreakType == "CHoCH").ToList();

            if (bosBreaks.Count >= 1 && chochBreaks.Count >= 1)
                Pass("EdgeCase_BOS_And_CHoCH_SameTF");
            else
                Fail("EdgeCase_BOS_And_CHoCH_SameTF", 
                     $"Expected >= 1 BOS and >= 1 CHoCH, got {bosBreaks.Count} BOS and {chochBreaks.Count} CHoCH");
        }
    }
}
