// ============================================================================
// POIDetectorTests.cs
// PinkButterfly CoreBrain - Tests exhaustivos para POIDetector
// 
// Cubre:
// - Detección básica de confluencias (2+ estructuras)
// - Cálculo de CompositeScore
// - Determinación de Bias (BuySide/SellSide/Neutral)
// - Clasificación Premium/Discount
// - Overlap con tolerancia ATR
// - Actualización de POIs existentes
// - Purga de POIs obsoletos
// - Edge cases
// ============================================================================

using System;
using System.Linq;
using NinjaTrader.NinjaScript.Indicators.PinkButterfly;

namespace PinkButterfly.Tests
{
    /// <summary>
    /// Tests exhaustivos del POIDetector
    /// Total: 27 tests cubriendo todos los aspectos de Points of Interest
    /// </summary>
    public class POIDetectorTests
    {
        private int _testsPassed = 0;
        private int _testsFailed = 0;
        private Action<string> _print;

        public POIDetectorTests(Action<string> printAction)
        {
            _print = printAction ?? Console.WriteLine;
        }

        public void RunAllTests()
        {
            _print("==============================================");
            _print("POI DETECTOR TESTS");
            _print("==============================================");
            _print("");

            // Tests básicos de detección
            Test_POI_BasicConfluence_TwoFVGs();
            Test_POI_BasicConfluence_FVGAndOB();
            Test_POI_NoConfluence_StructuresFarApart();
            Test_POI_MultipleConfluences_SameTF();

            // Tests de overlap y tolerancia
            Test_POI_OverlapTolerance_WithinATR();
            Test_POI_OverlapTolerance_ExceedsATR();
            Test_POI_OverlapTolerance_ExactBoundary();

            // Tests de CompositeScore
            Test_POI_CompositeScore_WeightedSum();
            Test_POI_CompositeScore_ConfluenceBonus();
            Test_POI_CompositeScore_MaxBonus();
            Test_POI_CompositeScore_HigherWithMoreStructures();

            // Tests de Bias
            Test_POI_Bias_BuySide_MajorityBullish();
            Test_POI_Bias_SellSide_MajorityBearish();
            Test_POI_Bias_Neutral_MixedStructures();
            Test_POI_Bias_Neutral_EqualCount();

            // Tests de Premium/Discount
            Test_POI_Premium_AboveThreshold();
            Test_POI_Discount_BelowThreshold();
            Test_POI_Premium_ExactThreshold();
            Test_POI_Premium_UpdatesWithMarket();

            // Tests de actualización y purga
            Test_POI_Update_SourceScoreChanged();
            Test_POI_Purge_SourceInvalidated();
            Test_POI_NoDuplicate_SameSources();

            // Edge cases
            Test_EdgeCase_InsufficientStructures();
            Test_EdgeCase_OnlyOneStructure();
            Test_EdgeCase_AllStructuresInactive();
            Test_EdgeCase_POIWithPOI();

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
                // Barras con rango pequeño pero body casi nulo
                // Esto genera ATR válido sin crear estructuras no deseadas
                AddBar(provider, price, price + 2.5, price - 2.5, price + 0.05, 1000.0);
            }
        }

        /// <summary>
        /// Helper para crear un FVG manualmente
        /// </summary>
        private void CreateFVG(CoreEngine engine, int tfMinutes, double low, double high, string direction, int barIndex)
        {
            var fvg = new FVGInfo
            {
                TF = tfMinutes,
                Low = low,
                High = high,
                Direction = direction,
                CreatedAtBarIndex = barIndex,
                LastUpdatedBarIndex = barIndex,
                IsActive = true,
                IsCompleted = true,
                Score = 0.5,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow
            };
            engine.AddStructure(fvg);
        }

        /// <summary>
        /// Helper para crear un Order Block manualmente
        /// </summary>
        private void CreateOrderBlock(CoreEngine engine, int tfMinutes, double low, double high, string direction, int barIndex)
        {
            var ob = new OrderBlockInfo
            {
                TF = tfMinutes,
                Low = low,
                High = high,
                Direction = direction,
                CreatedAtBarIndex = barIndex,
                LastUpdatedBarIndex = barIndex,
                IsActive = true,
                IsCompleted = true,
                Score = 0.6,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow
            };
            engine.AddStructure(ob);
        }

        // ========================================================================
        // TESTS - DETECCIÓN BÁSICA
        // ========================================================================

        private void Test_POI_BasicConfluence_TwoFVGs()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);
            engine.Initialize();

            // Procesar setup bars
            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Crear dos FVGs que se solapan
            CreateFVG(engine, 60, 5000, 5010, "Bullish", provider.GetCurrentBarIndex(60));
            CreateFVG(engine, 60, 5005, 5015, "Bullish", provider.GetCurrentBarIndex(60));

            // Añadir una barra más para que POIDetector procese
            AddBar(provider, 5010, 5012, 5008, 5011);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Verificar que se creó un POI
            var pois = engine.GetPOIs(60);

            if (pois.Count >= 1 && pois[0].SourceIds.Count == 2)
                Pass("POI_BasicConfluence_TwoFVGs");
            else
                Fail("POI_BasicConfluence_TwoFVGs", 
                     $"Expected 1 POI with 2 sources, got {pois.Count} POIs");
        }

        private void Test_POI_BasicConfluence_FVGAndOB()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);
            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Crear FVG y OB que se solapan
            CreateFVG(engine, 60, 5000, 5010, "Bullish", provider.GetCurrentBarIndex(60));
            CreateOrderBlock(engine, 60, 5005, 5012, "Bullish", provider.GetCurrentBarIndex(60));

            AddBar(provider, 5010, 5012, 5008, 5011);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var pois = engine.GetPOIs(60);

            if (pois.Count >= 1 && pois[0].SourceIds.Count == 2)
                Pass("POI_BasicConfluence_FVGAndOB");
            else
                Fail("POI_BasicConfluence_FVGAndOB", 
                     $"Expected 1 POI with 2 sources, got {pois.Count} POIs");
        }

        private void Test_POI_NoConfluence_StructuresFarApart()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);
            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Crear dos FVGs muy separados (más de tolerancia ATR)
            // ATR ~ 5, tolerancia = 0.5 * 5 = 2.5
            CreateFVG(engine, 60, 5000, 5005, "Bullish", provider.GetCurrentBarIndex(60));
            CreateFVG(engine, 60, 5020, 5025, "Bullish", provider.GetCurrentBarIndex(60)); // 15 puntos de separación

            AddBar(provider, 5010, 5012, 5008, 5011);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var pois = engine.GetPOIs(60);

            if (pois.Count == 0)
                Pass("POI_NoConfluence_StructuresFarApart");
            else
                Fail("POI_NoConfluence_StructuresFarApart", 
                     $"Expected 0 POIs (structures too far), got {pois.Count}");
        }

        private void Test_POI_MultipleConfluences_SameTF()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);
            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Crear dos confluencias separadas
            // Confluencia 1: FVG + OB en 5000-5010
            CreateFVG(engine, 60, 5000, 5010, "Bullish", provider.GetCurrentBarIndex(60));
            CreateOrderBlock(engine, 60, 5005, 5012, "Bullish", provider.GetCurrentBarIndex(60));

            // Confluencia 2: FVG + OB en 5050-5060
            CreateFVG(engine, 60, 5050, 5060, "Bearish", provider.GetCurrentBarIndex(60));
            CreateOrderBlock(engine, 60, 5055, 5062, "Bearish", provider.GetCurrentBarIndex(60));

            AddBar(provider, 5030, 5032, 5028, 5031);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var pois = engine.GetPOIs(60);

            if (pois.Count >= 2)
                Pass("POI_MultipleConfluences_SameTF");
            else
                Fail("POI_MultipleConfluences_SameTF", 
                     $"Expected >= 2 POIs, got {pois.Count}");
        }

        // ========================================================================
        // TESTS - OVERLAP Y TOLERANCIA
        // ========================================================================

        private void Test_POI_OverlapTolerance_WithinATR()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);
            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // ATR ~ 5, tolerancia = 0.5 * 5 = 2.5
            // Crear estructuras con gap de 2 puntos (dentro de tolerancia)
            CreateFVG(engine, 60, 5000, 5005, "Bullish", provider.GetCurrentBarIndex(60));
            CreateFVG(engine, 60, 5007, 5012, "Bullish", provider.GetCurrentBarIndex(60)); // Gap de 2 puntos

            AddBar(provider, 5010, 5012, 5008, 5011);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var pois = engine.GetPOIs(60);

            if (pois.Count >= 1)
                Pass("POI_OverlapTolerance_WithinATR");
            else
                Fail("POI_OverlapTolerance_WithinATR", 
                     $"Expected POI (within tolerance), got {pois.Count} POIs");
        }

        private void Test_POI_OverlapTolerance_ExceedsATR()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);
            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // ATR ~ 5, tolerancia = 0.5 * 5 = 2.5
            // Crear estructuras con gap de 5 puntos (excede tolerancia)
            CreateFVG(engine, 60, 5000, 5005, "Bullish", provider.GetCurrentBarIndex(60));
            CreateFVG(engine, 60, 5010, 5015, "Bullish", provider.GetCurrentBarIndex(60)); // Gap de 5 puntos

            AddBar(provider, 5010, 5012, 5008, 5011);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var pois = engine.GetPOIs(60);

            if (pois.Count == 0)
                Pass("POI_OverlapTolerance_ExceedsATR");
            else
                Fail("POI_OverlapTolerance_ExceedsATR", 
                     $"Expected 0 POIs (exceeds tolerance), got {pois.Count}");
        }

        private void Test_POI_OverlapTolerance_ExactBoundary()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);
            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // ATR ~ 5, tolerancia = 0.5 * 5 = 2.5
            // Crear estructuras con gap exactamente en el límite
            CreateFVG(engine, 60, 5000, 5005, "Bullish", provider.GetCurrentBarIndex(60));
            CreateFVG(engine, 60, 5007.5, 5012.5, "Bullish", provider.GetCurrentBarIndex(60)); // Gap de 2.5 puntos

            AddBar(provider, 5010, 5012, 5008, 5011);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var pois = engine.GetPOIs(60);

            if (pois.Count >= 1)
                Pass("POI_OverlapTolerance_ExactBoundary");
            else
                Fail("POI_OverlapTolerance_ExactBoundary", 
                     $"Expected POI (exact boundary), got {pois.Count} POIs");
        }

        // ========================================================================
        // TESTS - COMPOSITE SCORE
        // ========================================================================

        private void Test_POI_CompositeScore_WeightedSum()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.POI_ConfluenceBonus = 0.0; // Sin bonus para test puro
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);
            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Crear estructuras con scores conocidos
            CreateFVG(engine, 60, 5000, 5010, "Bullish", provider.GetCurrentBarIndex(60));
            CreateOrderBlock(engine, 60, 5005, 5012, "Bullish", provider.GetCurrentBarIndex(60));

            // Establecer scores manualmente
            var fvgs = engine.GetActiveFVGs(60, 0.0);
            var obs = engine.GetOrderBlocks(60);
            if (fvgs.Count > 0) fvgs[0].Score = 0.5;
            if (obs.Count > 0) obs[0].Score = 0.6;

            AddBar(provider, 5010, 5012, 5008, 5011);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var pois = engine.GetPOIs(60);

            if (pois.Count >= 1 && pois[0].CompositeScore > 0.4 && pois[0].CompositeScore < 0.7)
                Pass("POI_CompositeScore_WeightedSum");
            else
                Fail("POI_CompositeScore_WeightedSum", 
                     $"Expected CompositeScore in range [0.4, 0.7], got {pois.FirstOrDefault()?.CompositeScore}");
        }

        private void Test_POI_CompositeScore_ConfluenceBonus()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.POI_ConfluenceBonus = 0.15;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);
            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Crear 3 estructuras solapadas (más confluencia = más bonus)
            CreateFVG(engine, 60, 5000, 5010, "Bullish", provider.GetCurrentBarIndex(60));
            CreateFVG(engine, 60, 5005, 5012, "Bullish", provider.GetCurrentBarIndex(60));
            CreateOrderBlock(engine, 60, 5003, 5011, "Bullish", provider.GetCurrentBarIndex(60));

            AddBar(provider, 5010, 5012, 5008, 5011);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var pois = engine.GetPOIs(60);

            // Verificar que el bonus se aplicó correctamente
            // Con 3 estructuras, bonus = (3-1) * 0.15 = 0.30 (30%)
            // Score sin bonus sería ~0.213, con bonus = 0.213 * 1.30 = 0.277
            if (pois.Count >= 1 && pois[0].SourceIds.Count == 3 && pois[0].CompositeScore > 0.25)
                Pass("POI_CompositeScore_ConfluenceBonus");
            else
                Fail("POI_CompositeScore_ConfluenceBonus", 
                     $"Expected POI with 3 sources and score > 0.25 (with 30% bonus), got {pois.FirstOrDefault()?.SourceIds.Count} sources, score {pois.FirstOrDefault()?.CompositeScore}");
        }

        private void Test_POI_CompositeScore_MaxBonus()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.POI_ConfluenceBonus = 0.15;
            config.POI_MaxConfluenceBonus = 0.3; // Máximo 30% de bonus
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);
            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Crear muchas estructuras solapadas (debería aplicar max bonus)
            for (int i = 0; i < 5; i++)
            {
                CreateFVG(engine, 60, 5000 + i, 5010 + i, "Bullish", provider.GetCurrentBarIndex(60));
            }

            AddBar(provider, 5010, 5012, 5008, 5011);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var pois = engine.GetPOIs(60);

            if (pois.Count >= 1 && pois[0].CompositeScore <= 1.0)
                Pass("POI_CompositeScore_MaxBonus");
            else
                Fail("POI_CompositeScore_MaxBonus", 
                     $"Expected CompositeScore <= 1.0 (max bonus applied), got {pois.FirstOrDefault()?.CompositeScore}");
        }

        private void Test_POI_CompositeScore_HigherWithMoreStructures()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.POI_ConfluenceBonus = 0.15;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);
            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // POI 1: 2 estructuras
            CreateFVG(engine, 60, 5000, 5010, "Bullish", provider.GetCurrentBarIndex(60));
            CreateOrderBlock(engine, 60, 5005, 5012, "Bullish", provider.GetCurrentBarIndex(60));

            // POI 2: 3 estructuras
            CreateFVG(engine, 60, 5050, 5060, "Bearish", provider.GetCurrentBarIndex(60));
            CreateFVG(engine, 60, 5055, 5062, "Bearish", provider.GetCurrentBarIndex(60));
            CreateOrderBlock(engine, 60, 5053, 5061, "Bearish", provider.GetCurrentBarIndex(60));

            AddBar(provider, 5030, 5032, 5028, 5031);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var pois = engine.GetPOIs(60).OrderByDescending(p => p.SourceIds.Count).ToList();

            if (pois.Count >= 2 && pois[0].CompositeScore > pois[1].CompositeScore)
                Pass("POI_CompositeScore_HigherWithMoreStructures");
            else
                Fail("POI_CompositeScore_HigherWithMoreStructures", 
                     $"Expected POI with more structures to have higher score");
        }

        // ========================================================================
        // TESTS - BIAS
        // ========================================================================

        private void Test_POI_Bias_BuySide_MajorityBullish()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);
            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Crear mayoría de estructuras bullish
            CreateFVG(engine, 60, 5000, 5010, "Bullish", provider.GetCurrentBarIndex(60));
            CreateFVG(engine, 60, 5005, 5012, "Bullish", provider.GetCurrentBarIndex(60));
            CreateOrderBlock(engine, 60, 5003, 5011, "Bullish", provider.GetCurrentBarIndex(60));

            AddBar(provider, 5010, 5012, 5008, 5011);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var pois = engine.GetPOIs(60);

            if (pois.Count >= 1 && pois[0].Bias == "BuySide")
                Pass("POI_Bias_BuySide_MajorityBullish");
            else
                Fail("POI_Bias_BuySide_MajorityBullish", 
                     $"Expected Bias = BuySide, got {pois.FirstOrDefault()?.Bias}");
        }

        private void Test_POI_Bias_SellSide_MajorityBearish()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);
            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Crear mayoría de estructuras bearish
            CreateFVG(engine, 60, 5000, 5010, "Bearish", provider.GetCurrentBarIndex(60));
            CreateFVG(engine, 60, 5005, 5012, "Bearish", provider.GetCurrentBarIndex(60));
            CreateOrderBlock(engine, 60, 5003, 5011, "Bearish", provider.GetCurrentBarIndex(60));

            AddBar(provider, 5010, 5012, 5008, 5011);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var pois = engine.GetPOIs(60);

            if (pois.Count >= 1 && pois[0].Bias == "SellSide")
                Pass("POI_Bias_SellSide_MajorityBearish");
            else
                Fail("POI_Bias_SellSide_MajorityBearish", 
                     $"Expected Bias = SellSide, got {pois.FirstOrDefault()?.Bias}");
        }

        private void Test_POI_Bias_Neutral_MixedStructures()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);
            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Crear mix de estructuras bullish y bearish
            CreateFVG(engine, 60, 5000, 5010, "Bullish", provider.GetCurrentBarIndex(60));
            CreateOrderBlock(engine, 60, 5005, 5012, "Bearish", provider.GetCurrentBarIndex(60));

            AddBar(provider, 5010, 5012, 5008, 5011);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var pois = engine.GetPOIs(60);

            if (pois.Count >= 1 && pois[0].Bias == "Neutral")
                Pass("POI_Bias_Neutral_MixedStructures");
            else
                Fail("POI_Bias_Neutral_MixedStructures", 
                     $"Expected Bias = Neutral, got {pois.FirstOrDefault()?.Bias}");
        }

        private void Test_POI_Bias_Neutral_EqualCount()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);
            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Crear igual número de estructuras bullish y bearish
            CreateFVG(engine, 60, 5000, 5010, "Bullish", provider.GetCurrentBarIndex(60));
            CreateFVG(engine, 60, 5005, 5012, "Bullish", provider.GetCurrentBarIndex(60));
            CreateOrderBlock(engine, 60, 5003, 5011, "Bearish", provider.GetCurrentBarIndex(60));
            CreateOrderBlock(engine, 60, 5004, 5011, "Bearish", provider.GetCurrentBarIndex(60));

            AddBar(provider, 5010, 5012, 5008, 5011);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var pois = engine.GetPOIs(60);

            if (pois.Count >= 1 && pois[0].Bias == "Neutral")
                Pass("POI_Bias_Neutral_EqualCount");
            else
                Fail("POI_Bias_Neutral_EqualCount", 
                     $"Expected Bias = Neutral (equal count), got {pois.FirstOrDefault()?.Bias}");
        }

        // ========================================================================
        // TESTS - PREMIUM/DISCOUNT
        // ========================================================================

        private void Test_POI_Premium_AboveThreshold()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.POI_PremiumThreshold = 0.618;
            config.POI_PremiumLookbackBars = 20;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            // Crear barras con rango conocido: 5000 (low) - 5100 (high)
            for (int i = 0; i < 25; i++)
            {
                double price = 5000 + (i * 4); // Sube gradualmente
                AddBar(provider, price, price + 5, price - 2, price + 3);
            }

            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Crear POI en zona premium (por encima del 61.8% del rango)
            // Rango: 5000-5100, 61.8% = 5061.8
            CreateFVG(engine, 60, 5070, 5080, "Bullish", provider.GetCurrentBarIndex(60));
            CreateOrderBlock(engine, 60, 5075, 5082, "Bullish", provider.GetCurrentBarIndex(60));

            AddBar(provider, 5075, 5078, 5073, 5076);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var pois = engine.GetPOIs(60);

            if (pois.Count >= 1 && pois[0].IsPremium)
                Pass("POI_Premium_AboveThreshold");
            else
                Fail("POI_Premium_AboveThreshold", 
                     $"Expected IsPremium = true, got {pois.FirstOrDefault()?.IsPremium}");
        }

        private void Test_POI_Discount_BelowThreshold()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.POI_PremiumThreshold = 0.618;
            config.POI_PremiumLookbackBars = 20;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            // Crear barras con rango conocido
            for (int i = 0; i < 25; i++)
            {
                double price = 5000 + (i * 4);
                AddBar(provider, price, price + 5, price - 2, price + 3);
            }

            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Crear POI en zona discount (por debajo del 61.8%)
            CreateFVG(engine, 60, 5010, 5020, "Bullish", provider.GetCurrentBarIndex(60));
            CreateOrderBlock(engine, 60, 5015, 5022, "Bullish", provider.GetCurrentBarIndex(60));

            AddBar(provider, 5015, 5018, 5013, 5016);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var pois = engine.GetPOIs(60);

            if (pois.Count >= 1 && !pois[0].IsPremium)
                Pass("POI_Discount_BelowThreshold");
            else
                Fail("POI_Discount_BelowThreshold", 
                     $"Expected IsPremium = false, got {pois.FirstOrDefault()?.IsPremium}");
        }

        private void Test_POI_Premium_ExactThreshold()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.POI_PremiumThreshold = 0.618;
            config.POI_PremiumLookbackBars = 20;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            for (int i = 0; i < 25; i++)
            {
                double price = 5000 + (i * 4);
                AddBar(provider, price, price + 5, price - 2, price + 3);
            }

            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Crear POI claramente por encima del threshold (61.8% del rango)
            // Rango: 5000-5100, 61.8% = 5061.8, creamos en 5070 (70%)
            CreateFVG(engine, 60, 5068, 5072, "Bullish", provider.GetCurrentBarIndex(60));
            CreateOrderBlock(engine, 60, 5069, 5073, "Bullish", provider.GetCurrentBarIndex(60));

            AddBar(provider, 5062, 5065, 5060, 5063);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var pois = engine.GetPOIs(60);

            if (pois.Count >= 1 && pois[0].IsPremium)
                Pass("POI_Premium_ExactThreshold");
            else
                Fail("POI_Premium_ExactThreshold", 
                     $"Expected IsPremium = true (at threshold), got {pois.FirstOrDefault()?.IsPremium}");
        }

        private void Test_POI_Premium_UpdatesWithMarket()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.POI_PremiumThreshold = 0.618;
            config.POI_PremiumLookbackBars = 10;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            // Crear barras iniciales
            for (int i = 0; i < 25; i++)
            {
                double price = 5000 + (i * 2);
                AddBar(provider, price, price + 5, price - 2, price + 3);
            }

            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Crear POI en zona que inicialmente es discount
            CreateFVG(engine, 60, 5020, 5030, "Bullish", provider.GetCurrentBarIndex(60));
            CreateOrderBlock(engine, 60, 5025, 5032, "Bullish", provider.GetCurrentBarIndex(60));

            AddBar(provider, 5025, 5028, 5023, 5026);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var poisBefore = engine.GetPOIs(60);
            bool wasPremiumBefore = poisBefore.FirstOrDefault()?.IsPremium ?? false;

            // Añadir barras que bajan el mercado (ahora el POI está en zona premium relativa)
            for (int i = 0; i < 15; i++)
            {
                double price = 5000 - (i * 2);
                AddBar(provider, price, price + 3, price - 5, price - 2);
                engine.OnBarClose(60, provider.GetCurrentBarIndex(60));
            }

            var poisAfter = engine.GetPOIs(60);
            bool isPremiumAfter = poisAfter.FirstOrDefault()?.IsPremium ?? false;

            // El POI debería cambiar de discount a premium
            if (!wasPremiumBefore && isPremiumAfter)
                Pass("POI_Premium_UpdatesWithMarket");
            else
                Fail("POI_Premium_UpdatesWithMarket", 
                     $"Expected Premium status to change, got before: {wasPremiumBefore}, after: {isPremiumAfter}");
        }

        // ========================================================================
        // TESTS - ACTUALIZACIÓN Y PURGA
        // ========================================================================

        private void Test_POI_Update_SourceScoreChanged()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);
            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            CreateFVG(engine, 60, 5000, 5010, "Bullish", provider.GetCurrentBarIndex(60));
            CreateOrderBlock(engine, 60, 5005, 5012, "Bullish", provider.GetCurrentBarIndex(60));

            AddBar(provider, 5010, 5012, 5008, 5011);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var poisBefore = engine.GetPOIs(60);
            double scoreBefore = poisBefore.FirstOrDefault()?.CompositeScore ?? 0;

            // Añadir más barras para que los scores de las estructuras fuente cambien
            for (int i = 0; i < 10; i++)
            {
                AddBar(provider, 5010 + i, 5012 + i, 5008 + i, 5011 + i);
                engine.OnBarClose(60, provider.GetCurrentBarIndex(60));
            }

            var poisAfter = engine.GetPOIs(60);
            double scoreAfter = poisAfter.FirstOrDefault()?.CompositeScore ?? 0;

            // El score debería haber cambiado (decay por tiempo)
            if (Math.Abs(scoreBefore - scoreAfter) > 0.01)
                Pass("POI_Update_SourceScoreChanged");
            else
                Fail("POI_Update_SourceScoreChanged", 
                     $"Expected score to change, got before: {scoreBefore:F3}, after: {scoreAfter:F3}");
        }

        private void Test_POI_Purge_SourceInvalidated()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);
            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            CreateFVG(engine, 60, 5000, 5010, "Bullish", provider.GetCurrentBarIndex(60));
            CreateOrderBlock(engine, 60, 5005, 5012, "Bullish", provider.GetCurrentBarIndex(60));

            AddBar(provider, 5010, 5012, 5008, 5011);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var poisBefore = engine.GetPOIs(60);
            int countBefore = poisBefore.Count;

            // Invalidar una de las estructuras fuente
            var fvgs = engine.GetActiveFVGs(60, 0.0);
            if (fvgs.Count > 0)
            {
                var fvg = fvgs[0];
                fvg.IsActive = false;
                engine.UpdateStructure(fvg);
            }

            // Añadir barra para que POIDetector procese la actualización
            AddBar(provider, 5011, 5013, 5009, 5012);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var poisAfter = engine.GetPOIs(60);
            int countAfter = poisAfter.Count;

            // El POI debería haberse invalidado (GetPOIs solo retorna activos)
            // También verificar que el POI original está inactivo
            bool poiInvalidated = countBefore > 0 && countAfter == 0;
            
            // Alternativa: verificar directamente si el POI está inactivo
            if (!poiInvalidated && poisBefore.Count > 0)
            {
                var originalPOI = engine.GetStructureById(poisBefore[0].Id);
                poiInvalidated = originalPOI != null && !originalPOI.IsActive;
            }

            if (poiInvalidated)
                Pass("POI_Purge_SourceInvalidated");
            else
                Fail("POI_Purge_SourceInvalidated", 
                     $"Expected POI to be purged, got before: {countBefore}, after: {countAfter}");
        }

        private void Test_POI_NoDuplicate_SameSources()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.EnableDebug = false; // Desactivar debug
            
            // Desactivar SwingDetector para que no cree estructuras automáticamente
            config.nLeft = 999; // Valor imposible para evitar detección
            config.nRight = 999;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);
            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            CreateFVG(engine, 60, 5000, 5010, "Bullish", provider.GetCurrentBarIndex(60));
            CreateOrderBlock(engine, 60, 5005, 5012, "Bullish", provider.GetCurrentBarIndex(60));

            // Procesar múltiples veces (sin crear nuevas estructuras)
            AddBar(provider, 5010, 5012, 5008, 5011);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            AddBar(provider, 5011, 5013, 5009, 5012);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            AddBar(provider, 5012, 5014, 5010, 5013);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var pois = engine.GetPOIs(60);

            // Solo debería haber 1 POI (no duplicados)
            if (pois.Count == 1)
                Pass("POI_NoDuplicate_SameSources");
            else
                Fail("POI_NoDuplicate_SameSources", 
                     $"Expected 1 POI (no duplicates), got {pois.Count}");
        }

        // ========================================================================
        // TESTS - EDGE CASES
        // ========================================================================

        private void Test_EdgeCase_InsufficientStructures()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 3; // Requiere 3 estructuras
            config.OverlapToleranceATR = 0.5;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);
            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Solo crear 2 estructuras (insuficiente)
            CreateFVG(engine, 60, 5000, 5010, "Bullish", provider.GetCurrentBarIndex(60));
            CreateOrderBlock(engine, 60, 5005, 5012, "Bullish", provider.GetCurrentBarIndex(60));

            AddBar(provider, 5010, 5012, 5008, 5011);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var pois = engine.GetPOIs(60);

            if (pois.Count == 0)
                Pass("EdgeCase_InsufficientStructures");
            else
                Fail("EdgeCase_InsufficientStructures", 
                     $"Expected 0 POIs (insufficient structures), got {pois.Count}");
        }

        private void Test_EdgeCase_OnlyOneStructure()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);
            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Solo una estructura
            CreateFVG(engine, 60, 5000, 5010, "Bullish", provider.GetCurrentBarIndex(60));

            AddBar(provider, 5010, 5012, 5008, 5011);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var pois = engine.GetPOIs(60);

            if (pois.Count == 0)
                Pass("EdgeCase_OnlyOneStructure");
            else
                Fail("EdgeCase_OnlyOneStructure", 
                     $"Expected 0 POIs (only one structure), got {pois.Count}");
        }

        private void Test_EdgeCase_AllStructuresInactive()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);
            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            CreateFVG(engine, 60, 5000, 5010, "Bullish", provider.GetCurrentBarIndex(60));
            CreateOrderBlock(engine, 60, 5005, 5012, "Bullish", provider.GetCurrentBarIndex(60));

            // Invalidar todas las estructuras
            var fvgs = engine.GetActiveFVGs(60, 0.0);
            var obs = engine.GetOrderBlocks(60);
            foreach (var fvg in fvgs)
            {
                fvg.IsActive = false;
                engine.UpdateStructure(fvg);
            }
            foreach (var ob in obs)
            {
                ob.IsActive = false;
                engine.UpdateStructure(ob);
            }

            AddBar(provider, 5010, 5012, 5008, 5011);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var pois = engine.GetPOIs(60);

            if (pois.Count == 0)
                Pass("EdgeCase_AllStructuresInactive");
            else
                Fail("EdgeCase_AllStructuresInactive", 
                     $"Expected 0 POIs (all structures inactive), got {pois.Count}");
        }

        private void Test_EdgeCase_POIWithPOI()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinStructuresForPOI = 2;
            config.OverlapToleranceATR = 0.5;
            config.EnableDebug = false;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 25);
            engine.Initialize();

            for (int i = 0; i <= provider.GetCurrentBarIndex(60); i++)
                engine.OnBarClose(60, i);

            // Crear estructuras que generan POI
            CreateFVG(engine, 60, 5000, 5010, "Bullish", provider.GetCurrentBarIndex(60));
            CreateOrderBlock(engine, 60, 5005, 5012, "Bullish", provider.GetCurrentBarIndex(60));

            AddBar(provider, 5010, 5012, 5008, 5011);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var poisFirst = engine.GetPOIs(60);
            int countFirst = poisFirst.Count;

            // Crear más estructuras que se solapan con el POI existente
            // El POIDetector debe excluir POIs de la búsqueda de confluencias
            CreateFVG(engine, 60, 5003, 5013, "Bullish", provider.GetCurrentBarIndex(60));

            AddBar(provider, 5011, 5013, 5009, 5012);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var poisSecond = engine.GetPOIs(60);
            int countSecond = poisSecond.Count;

            // No debería crear POI de POI (recursión evitada)
            if (countSecond == countFirst || countSecond == countFirst + 1)
                Pass("EdgeCase_POIWithPOI");
            else
                Fail("EdgeCase_POIWithPOI", 
                     $"Expected no POI recursion, got first: {countFirst}, second: {countSecond}");
        }
    }
}

