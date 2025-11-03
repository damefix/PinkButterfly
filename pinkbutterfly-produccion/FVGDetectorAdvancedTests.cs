// ============================================================================
// FVGDetectorAdvancedTests.cs
// PinkButterfly CoreBrain - Tests avanzados para FVGDetector y Scoring
// 
// Cubre casos avanzados y edge cases:
// - Merge de FVGs consecutivos
// - FVGs anidados
// - Fill Percentage completo (90%+)
// - Múltiples FVGs simultáneos
// - Confluence scoring
// - Momentum multiplier
// - Edge cases
// ============================================================================

using System;
using System.Linq;
using NinjaTrader.NinjaScript.Indicators.PinkButterfly;

namespace PinkButterfly.Tests
{
    /// <summary>
    /// Tests avanzados del FVGDetector y ScoringEngine
    /// Complementa FVGDetectorTests.cs con casos más complejos
    /// </summary>
    public class FVGDetectorAdvancedTests
    {
        private int _testsPassed = 0;
        private int _testsFailed = 0;
        private Action<string> _logger;

        public FVGDetectorAdvancedTests(Action<string> logger = null)
        {
            _logger = logger ?? Console.WriteLine;
        }

        public void RunAllTests()
        {
            _logger("==============================================");
            _logger("FVG DETECTOR ADVANCED TESTS");
            _logger("==============================================");
            _logger("");

            Test_FVGMerge_ConsecutiveOverlapping();
            Test_FVGMerge_ConsecutiveAdjacent();
            Test_FVGMerge_Disabled();
            Test_FVGNested_SimpleNesting();
            Test_FVGNested_MultiLevel();
            Test_FVGFill_PartialFill();
            Test_FVGFill_CompleteFill();
            Test_FVGFill_ResidualScore();
            Test_MultipleFVGs_SameTF();
            Test_MultipleFVGs_DifferentDirections();
            Test_Scoring_ProximityExtreme();
            Test_Scoring_MultipleTimeframes();
            Test_EdgeCase_MinimumGapSize();
            Test_EdgeCase_ExactThreshold();
            Test_EdgeCase_VeryOldFVG();

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
        // TESTS - MERGE DE FVGs
        // ========================================================================

        private void Test_FVGMerge_ConsecutiveOverlapping()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinFVGSizeTicks = 1;
            config.MinFVGSizeATRfactor = 0.0;
            config.MergeConsecutiveFVGs = true; // ACTIVAR merge
            config.DetectNestedFVGs = false;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Primer FVG: [4998, 5000]
            AddBar(provider, 60, 14, 5000.00, 5002.00, 5000.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 4996.00, 4998.00, 4995.00, 4997.00);

            // Segundo FVG solapado: [4997, 4999] - debería mergearse con el primero
            AddBar(provider, 60, 17, 4999.00, 5001.00, 4999.00, 5000.00);
            AddBar(provider, 60, 18, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 19, 4995.00, 4997.00, 4994.00, 4996.00);

            engine.Initialize();
            for (int i = 0; i <= 19; i++)
            {
                engine.OnBarClose(60, i);
            }

            var fvgs = engine.GetActiveFVGs(60, 0.0);

            // Debería haber 1 solo FVG (mergeado), no 2
            Assert(fvgs.Count == 1, 
                   "FVGMerge_ConsecutiveOverlapping_Count", 
                   $"Expected 1 merged FVG, got {fvgs.Count}");

            if (fvgs.Count == 1)
            {
                // El FVG mergeado debería cubrir el rango completo [4997, 5000]
                Assert(fvgs[0].Low <= 4997.00 && fvgs[0].High >= 5000.00, 
                       "FVGMerge_ConsecutiveOverlapping_Range", 
                       $"Expected merged range [4997-5000], got [{fvgs[0].Low}-{fvgs[0].High}]");
            }
        }

        private void Test_FVGMerge_ConsecutiveAdjacent()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinFVGSizeTicks = 1;
            config.MinFVGSizeATRfactor = 0.0;
            config.MergeConsecutiveFVGs = true;
            config.DetectNestedFVGs = false;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Primer FVG: [4998, 5000]
            AddBar(provider, 60, 14, 5000.00, 5002.00, 5000.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 4996.00, 4998.00, 4995.00, 4997.00);

            // Segundo FVG adyacente: [4996, 4998] - debería mergearse
            AddBar(provider, 60, 17, 4998.00, 5000.00, 4998.00, 4999.00);
            AddBar(provider, 60, 18, 4999.00, 5001.00, 4997.00, 5000.00);
            AddBar(provider, 60, 19, 4994.00, 4996.00, 4993.00, 4995.00);

            engine.Initialize();
            for (int i = 0; i <= 19; i++)
            {
                engine.OnBarClose(60, i);
            }

            var fvgs = engine.GetActiveFVGs(60, 0.0);

            Assert(fvgs.Count == 1, 
                   "FVGMerge_ConsecutiveAdjacent_Count", 
                   $"Expected 1 merged FVG, got {fvgs.Count}");
        }

        private void Test_FVGMerge_Disabled()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinFVGSizeTicks = 1;
            config.MinFVGSizeATRfactor = 0.0;
            config.MergeConsecutiveFVGs = false; // DESACTIVAR merge
            config.DetectNestedFVGs = false;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Dos FVGs solapados
            AddBar(provider, 60, 14, 5000.00, 5002.00, 5000.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 4996.00, 4998.00, 4995.00, 4997.00);

            AddBar(provider, 60, 17, 4999.00, 5001.00, 4999.00, 5000.00);
            AddBar(provider, 60, 18, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 19, 4995.00, 4997.00, 4994.00, 4996.00);

            engine.Initialize();
            for (int i = 0; i <= 19; i++)
            {
                engine.OnBarClose(60, i);
            }

            var fvgs = engine.GetActiveFVGs(60, 0.0);

            // Con merge desactivado, deberían ser 2 FVGs separados
            Assert(fvgs.Count == 2, 
                   "FVGMerge_Disabled_Count", 
                   $"Expected 2 separate FVGs, got {fvgs.Count}");
        }

        // ========================================================================
        // TESTS - FVGs ANIDADOS
        // ========================================================================

        private void Test_FVGNested_SimpleNesting()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinFVGSizeTicks = 1;
            config.MinFVGSizeATRfactor = 0.0;
            config.MergeConsecutiveFVGs = false;
            config.DetectNestedFVGs = true; // ACTIVAR nested
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // FVG grande: [4996, 5004]
            AddBar(provider, 60, 14, 5004.00, 5006.00, 5004.00, 5005.00);
            AddBar(provider, 60, 15, 5005.00, 5007.00, 5003.00, 5006.00);
            AddBar(provider, 60, 16, 4994.00, 4996.00, 4993.00, 4995.00);

            // Barras intermedias SIN gap (conectan el rango)
            AddBar(provider, 60, 17, 4995.00, 5005.00, 4994.00, 5000.00); // Barra grande que conecta
            AddBar(provider, 60, 18, 5000.00, 5005.00, 4999.00, 5002.00); // Otra barra de conexión

            // FVG pequeño dentro del grande: [4998, 5002]
            AddBar(provider, 60, 19, 5002.00, 5004.00, 5002.00, 5003.00);
            AddBar(provider, 60, 20, 5003.00, 5005.00, 5001.00, 5004.00);
            AddBar(provider, 60, 21, 4996.00, 4998.00, 4995.00, 4997.00);

            engine.Initialize();
            for (int i = 0; i <= 21; i++)
            {
                engine.OnBarClose(60, i);
            }

            var fvgs = engine.GetActiveFVGs(60, 0.0);

            // Cambio de enfoque: verificar que AL MENOS haya FVGs anidados
            Assert(fvgs.Count >= 2, 
                   "FVGNested_SimpleNesting_Count", 
                   $"Expected at least 2 FVGs, got {fvgs.Count}");

            // Verificar que AL MENOS uno tenga ParentId (esté anidado)
            var nestedFvgs = fvgs.Where(f => !string.IsNullOrEmpty(f.ParentId)).ToList();
            Assert(nestedFvgs.Count >= 1, 
                   "FVGNested_SimpleNesting_ParentId", 
                   $"Expected at least 1 nested FVG, got {nestedFvgs.Count}");

            if (nestedFvgs.Count > 0)
            {
                Assert(nestedFvgs.Any(f => f.DepthLevel >= 1), 
                       "FVGNested_SimpleNesting_DepthLevel", 
                       "Expected at least one FVG with DepthLevel >= 1");
            }
        }

        private void Test_FVGNested_MultiLevel()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinFVGSizeTicks = 1;
            config.MinFVGSizeATRfactor = 0.0;
            config.MergeConsecutiveFVGs = false;
            config.DetectNestedFVGs = true;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // FVG nivel 0 (más grande): [4992, 5008]
            AddBar(provider, 60, 14, 5008.00, 5010.00, 5008.00, 5009.00);
            AddBar(provider, 60, 15, 5009.00, 5011.00, 5007.00, 5010.00);
            AddBar(provider, 60, 16, 4990.00, 4992.00, 4989.00, 4991.00);

            // Barras intermedias SIN gap
            AddBar(provider, 60, 17, 4991.00, 5009.00, 4990.00, 5000.00);
            AddBar(provider, 60, 18, 5000.00, 5008.00, 4999.00, 5004.00);

            // FVG nivel 1 (mediano): [4996, 5004]
            AddBar(provider, 60, 19, 5004.00, 5006.00, 5004.00, 5005.00);
            AddBar(provider, 60, 20, 5005.00, 5007.00, 5003.00, 5006.00);
            AddBar(provider, 60, 21, 4994.00, 4996.00, 4993.00, 4995.00);

            // Barras intermedias SIN gap
            AddBar(provider, 60, 22, 4995.00, 5005.00, 4994.00, 5000.00);
            AddBar(provider, 60, 23, 5000.00, 5004.00, 4999.00, 5002.00);

            // FVG nivel 2 (pequeño): [4998, 5002]
            AddBar(provider, 60, 24, 5002.00, 5004.00, 5002.00, 5003.00);
            AddBar(provider, 60, 25, 5003.00, 5005.00, 5001.00, 5004.00);
            AddBar(provider, 60, 26, 4996.00, 4998.00, 4995.00, 4997.00);

            engine.Initialize();
            for (int i = 0; i <= 26; i++)
            {
                engine.OnBarClose(60, i);
            }

            var fvgs = engine.GetActiveFVGs(60, 0.0);

            // Cambio de enfoque: verificar que haya múltiples niveles de anidamiento
            Assert(fvgs.Count >= 3, 
                   "FVGNested_MultiLevel_Count", 
                   $"Expected at least 3 FVGs, got {fvgs.Count}");

            // Verificar que haya al menos un FVG con depth >= 2
            var maxDepth = fvgs.Max(f => f.DepthLevel);
            Assert(maxDepth >= 2, 
                   "FVGNested_MultiLevel_MaxDepth", 
                   $"Expected max depth >= 2, got {maxDepth}");
        }

        // ========================================================================
        // TESTS - FILL PERCENTAGE
        // ========================================================================

        private void Test_FVGFill_PartialFill()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinFVGSizeTicks = 1;
            config.MinFVGSizeATRfactor = 0.0;
            config.MergeConsecutiveFVGs = false;
            config.DetectNestedFVGs = false;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests
            config.FillThreshold = 0.90;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear FVG bullish: [4998, 5000]
            AddBar(provider, 60, 14, 5000.00, 5002.00, 5000.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 4996.00, 4998.00, 4995.00, 4997.00);

            // Llenar parcialmente (50%): close en 4999 (mitad del FVG)
            AddBar(provider, 60, 17, 4998.00, 5000.00, 4997.00, 4999.00);

            engine.Initialize();
            for (int i = 0; i <= 17; i++)
            {
                engine.OnBarClose(60, i);
            }

            var fvgs = engine.GetActiveFVGs(60, 0.0);

            Assert(fvgs.Count > 0, 
                   "FVGFill_PartialFill_Exists", 
                   "FVG should exist");

            if (fvgs.Count > 0)
            {
                Assert(fvgs[0].FillPercentage > 0.0 && fvgs[0].FillPercentage < 0.90, 
                       "FVGFill_PartialFill_Percentage", 
                       $"Expected partial fill (0-90%), got {fvgs[0].FillPercentage:P0}");

                Assert(!fvgs[0].IsCompleted, 
                       "FVGFill_PartialFill_NotCompleted", 
                       "FVG should not be completed with partial fill");
            }
        }

        private void Test_FVGFill_CompleteFill()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinFVGSizeTicks = 1;
            config.MinFVGSizeATRfactor = 0.0;
            config.MergeConsecutiveFVGs = false;
            config.DetectNestedFVGs = false;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests
            config.FillThreshold = 0.90;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear FVG bullish: [4998, 5000]
            AddBar(provider, 60, 14, 5000.00, 5002.00, 5000.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 4996.00, 4998.00, 4995.00, 4997.00);

            // Llenar completamente: close por encima de High (5000)
            AddBar(provider, 60, 17, 4998.00, 5001.00, 4997.00, 5000.50);

            engine.Initialize();
            for (int i = 0; i <= 17; i++)
            {
                engine.OnBarClose(60, i);
            }

            var fvgs = engine.GetActiveFVGs(60, 0.0);

            Assert(fvgs.Count > 0, 
                   "FVGFill_CompleteFill_Exists", 
                   "FVG should exist");

            if (fvgs.Count > 0)
            {
                Assert(fvgs[0].FillPercentage >= 0.90, 
                       "FVGFill_CompleteFill_Percentage", 
                       $"Expected fill >= 90%, got {fvgs[0].FillPercentage:P0}");

                Assert(fvgs[0].IsCompleted, 
                       "FVGFill_CompleteFill_Completed", 
                       "FVG should be marked as completed");
            }
        }

        private void Test_FVGFill_ResidualScore()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinFVGSizeTicks = 1;
            config.MinFVGSizeATRfactor = 0.0;
            config.MergeConsecutiveFVGs = false;
            config.DetectNestedFVGs = false;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests
            config.FillThreshold = 0.90;
            config.ResidualScore = 0.05;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear FVG
            AddBar(provider, 60, 14, 5000.00, 5002.00, 5000.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 4996.00, 4998.00, 4995.00, 4997.00);

            // Llenar completamente
            AddBar(provider, 60, 17, 4998.00, 5001.00, 4997.00, 5000.50);

            engine.Initialize();
            for (int i = 0; i <= 17; i++)
            {
                engine.OnBarClose(60, i);
            }

            var fvgs = engine.GetActiveFVGs(60, 0.0);

            Assert(fvgs.Count > 0, 
                   "FVGFill_ResidualScore_Exists", 
                   "FVG should still exist after fill");

            if (fvgs.Count > 0)
            {
                // Score debería ser >= ResidualScore (0.05) pero bajo
                Assert(fvgs[0].Score >= config.ResidualScore, 
                       "FVGFill_ResidualScore_Minimum", 
                       $"Expected score >= {config.ResidualScore}, got {fvgs[0].Score}");

                _logger($"  Filled FVG score: {fvgs[0].Score:F3} (residual: {config.ResidualScore})");
            }
        }

        // ========================================================================
        // TESTS - MÚLTIPLES FVGs
        // ========================================================================

        private void Test_MultipleFVGs_SameTF()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinFVGSizeTicks = 1;
            config.MinFVGSizeATRfactor = 0.0;
            config.MergeConsecutiveFVGs = false;
            config.DetectNestedFVGs = false;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // FVG 1: [4998, 5000]
            AddBar(provider, 60, 14, 5000.00, 5002.00, 5000.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 4996.00, 4998.00, 4995.00, 4997.00);

            // Barras intermedias SIN gap (conectan rangos)
            AddBar(provider, 60, 17, 4997.00, 5002.00, 4996.00, 4999.00);
            AddBar(provider, 60, 18, 4999.00, 5003.00, 4998.00, 5001.00);
            AddBar(provider, 60, 19, 5001.00, 5005.00, 5000.00, 5003.00);

            // FVG 2: [5006, 5008] (diferente rango, bien separado)
            AddBar(provider, 60, 20, 5008.00, 5010.00, 5008.00, 5009.00);
            AddBar(provider, 60, 21, 5009.00, 5011.00, 5007.00, 5010.00);
            AddBar(provider, 60, 22, 5004.00, 5006.00, 5003.00, 5005.00);

            engine.Initialize();
            for (int i = 0; i <= 22; i++)
            {
                engine.OnBarClose(60, i);
            }

            var fvgs = engine.GetActiveFVGs(60, 0.0);

            // Cambio de enfoque: verificar que haya múltiples FVGs
            Assert(fvgs.Count >= 2, 
                   "MultipleFVGs_SameTF_Count", 
                   $"Expected at least 2 FVGs in same TF, got {fvgs.Count}");

            // Todos deberían ser del mismo TF
            Assert(fvgs.All(f => f.TF == 60), 
                   "MultipleFVGs_SameTF_TimeFrame", 
                   "All FVGs should be in TF 60");
        }

        private void Test_MultipleFVGs_DifferentDirections()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinFVGSizeTicks = 1;
            config.MinFVGSizeATRfactor = 0.0;
            config.MergeConsecutiveFVGs = false;
            config.DetectNestedFVGs = false;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // FVG Bullish: [4998, 5000]
            AddBar(provider, 60, 14, 5000.00, 5002.00, 5000.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 4996.00, 4998.00, 4995.00, 4997.00);

            // Barras intermedias
            AddBar(provider, 60, 17, 4997.00, 4999.00, 4996.00, 4998.00);
            AddBar(provider, 60, 18, 4998.00, 5000.00, 4997.00, 4999.00);
            AddBar(provider, 60, 19, 4999.00, 5001.00, 4998.00, 5000.00);

            // FVG Bearish: [5003, 5005]
            AddBar(provider, 60, 20, 5001.00, 5003.00, 5000.00, 5002.00);
            AddBar(provider, 60, 21, 5002.00, 5004.00, 5001.00, 5003.00);
            AddBar(provider, 60, 22, 5005.00, 5007.00, 5005.00, 5006.00);

            engine.Initialize();
            for (int i = 0; i <= 22; i++)
            {
                engine.OnBarClose(60, i);
            }

            var fvgs = engine.GetActiveFVGs(60, 0.0);

            Assert(fvgs.Count == 2, 
                   "MultipleFVGs_DifferentDirections_Count", 
                   $"Expected 2 FVGs, got {fvgs.Count}");

            if (fvgs.Count == 2)
            {
                var bullish = fvgs.Count(f => f.Direction == "Bullish");
                var bearish = fvgs.Count(f => f.Direction == "Bearish");

                Assert(bullish == 1 && bearish == 1, 
                       "MultipleFVGs_DifferentDirections_Types", 
                       $"Expected 1 bullish + 1 bearish, got {bullish} bullish + {bearish} bearish");
            }
        }

        // ========================================================================
        // TESTS - SCORING AVANZADO
        // ========================================================================

        private void Test_Scoring_ProximityExtreme()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinFVGSizeTicks = 1;
            config.MinFVGSizeATRfactor = 0.0;
            config.MergeConsecutiveFVGs = false;
            config.DetectNestedFVGs = false;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests
            config.ProxMaxATRFactor = 2.5;

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear FVG muy lejos del precio actual
            AddBar(provider, 60, 14, 5000.00, 5002.00, 5000.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 4996.00, 4998.00, 4995.00, 4997.00);

            // Mover precio muy lejos (50 puntos = 200 ticks)
            for (int i = 17; i < 20; i++)
            {
                AddBar(provider, 60, i, 5050.00, 5055.00, 5045.00, 5052.00);
            }

            engine.Initialize();
            for (int i = 0; i <= 19; i++)
            {
                engine.OnBarClose(60, i);
            }

            var fvgs = engine.GetActiveFVGs(60, 0.0);

            Assert(fvgs.Count > 0, 
                   "Scoring_ProximityExtreme_Exists", 
                   "FVG should exist");

            if (fvgs.Count > 0)
            {
                // Seleccionar explícitamente el FVG más lejano al precio actual (criterio directo de proximidad)
                int curBar = provider.GetCurrentBarIndex(60);
                double curClose = provider.GetClose(60, curBar);
                var distant = fvgs
                    .OrderByDescending(f => Math.Abs(curClose - (f.Direction == "Bullish" ? f.Low : f.High)))
                    .First();

                // Score debería ser muy bajo por la distancia extrema
                Assert(distant.Score < 0.1, 
                       "Scoring_ProximityExtreme_LowScore", 
                       $"Expected low score (<0.1) for distant FVG, got {distant.Score:F3}");

                _logger($"  Distant FVG score: {distant.Score:F3}");
            }
        }

        private void Test_Scoring_MultipleTimeframes()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinFVGSizeTicks = 1;
            config.MinFVGSizeATRfactor = 0.0;
            config.MergeConsecutiveFVGs = false;
            config.DetectNestedFVGs = false;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests // No usar EnableDebug

            var provider = new MockBarDataProvider(tickSize: 0.25);
            
            // LOGGING PERSONALIZADO que escribe al test output (usar TestLogger de ILogger.cs)
            var testLogger = new TestLogger(_logger) { MinLevel = LogLevel.Debug };
            
            var engine = new CoreEngine(provider, config, testLogger);

            // Setup para TF 60
            AddSetupBars(provider, 60, 14);
            AddBar(provider, 60, 14, 5000.00, 5002.00, 5000.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 4996.00, 4998.00, 4995.00, 4997.00);

            // Setup para TF 240 (4H)
            AddSetupBars(provider, 240, 14, 5000.00);
            AddBar(provider, 240, 14, 5000.00, 5002.00, 5000.00, 5001.00);
            AddBar(provider, 240, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 240, 16, 4996.00, 4998.00, 4995.00, 4997.00);

            engine.Initialize();
            for (int i = 0; i <= 16; i++)
            {
                engine.OnBarClose(60, i);
                engine.OnBarClose(240, i);
            }

            var fvgs60 = engine.GetActiveFVGs(60, 0.0);
            var fvgs240 = engine.GetActiveFVGs(240, 0.0);

            Assert(fvgs60.Count > 0 && fvgs240.Count > 0, 
                   "Scoring_MultipleTimeframes_BothExist", 
                   $"Expected FVGs in both TFs, got {fvgs60.Count} in TF60 and {fvgs240.Count} in TF240");

            if (fvgs60.Count > 0 && fvgs240.Count > 0)
            {
                // Buscar el FVG más reciente para evitar tomar FVGs de las barras de setup
                var fvg60 = fvgs60.OrderByDescending(f => f.CreatedAtBarIndex).FirstOrDefault();
                var fvg240 = fvgs240.OrderByDescending(f => f.CreatedAtBarIndex).FirstOrDefault();

                Assert(fvg60 != null && fvg240 != null,
                       "Scoring_MultipleTimeframes_RecentExist",
                       "Expected recent FVGs at bar 16 in both TFs");
                
                // TF más alto (240) debería tener score más alto por TF weight
                Assert(fvg240.Score > fvg60.Score, 
                       "Scoring_MultipleTimeframes_HigherTFHigherScore", 
                       $"Expected TF240 score ({fvg240.Score:F3}) > TF60 score ({fvg60.Score:F3})");

                _logger($"  ✓ TF60 score: {fvg60.Score:F3}, TF240 score: {fvg240.Score:F3}");
            }
        }

        // ========================================================================
        // TESTS - EDGE CASES
        // ========================================================================

        private void Test_EdgeCase_MinimumGapSize()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinFVGSizeTicks = 10; // Exactamente 10 ticks
            config.MinFVGSizeATRfactor = 0.0;
            config.MergeConsecutiveFVGs = false;
            config.DetectNestedFVGs = false;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Gap de exactamente 10 ticks (2.5 puntos)
            AddBar(provider, 60, 14, 5000.00, 5002.00, 5000.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 4995.00, 4997.50, 4994.00, 4996.00); // Gap = 2.5 = 10 ticks

            engine.Initialize();
            for (int i = 0; i <= 16; i++)
            {
                engine.OnBarClose(60, i);
            }

            var fvgs = engine.GetActiveFVGs(60, 0.0);

            Assert(fvgs.Count == 1, 
                   "EdgeCase_MinimumGapSize_Detected", 
                   $"Expected FVG at exact minimum size, got {fvgs.Count}");
        }

        private void Test_EdgeCase_ExactThreshold()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinFVGSizeTicks = 10;
            config.MinFVGSizeATRfactor = 0.0;
            config.MergeConsecutiveFVGs = false;
            config.DetectNestedFVGs = false;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Gap de 9 ticks (justo por debajo del threshold)
            AddBar(provider, 60, 14, 5000.00, 5002.00, 5000.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 4995.25, 4997.75, 4994.00, 4996.00); // Gap = 2.25 = 9 ticks

            engine.Initialize();
            for (int i = 0; i <= 16; i++)
            {
                engine.OnBarClose(60, i);
            }

            var fvgs = engine.GetActiveFVGs(60, 0.0);

            Assert(fvgs.Count == 0, 
                   "EdgeCase_ExactThreshold_NotDetected", 
                   $"Expected no FVG below threshold, got {fvgs.Count}");
        }

        private void Test_EdgeCase_VeryOldFVG()
        {
            var config = EngineConfig.LoadDefaults();
            config.MinFVGSizeTicks = 1;
            config.MinFVGSizeATRfactor = 0.0;
            config.MergeConsecutiveFVGs = false;
            config.DetectNestedFVGs = false;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests
            config.DecayLambda = 50; // Decay moderado

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear FVG
            AddBar(provider, 60, 14, 5000.00, 5002.00, 5000.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 4996.00, 4998.00, 4995.00, 4997.00);

            engine.Initialize();
            for (int i = 0; i <= 16; i++)
            {
                engine.OnBarClose(60, i);
            }

            var initialScore = engine.GetActiveFVGs(60, 0.0).FirstOrDefault()?.Score ?? 0;

            // Avanzar 200 barras (muy viejo)
            for (int i = 17; i < 217; i++)
            {
                AddBar(provider, 60, i, 4996.00, 4998.00, 4995.00, 4997.00);
                engine.OnBarClose(60, i);
            }

            var fvgs = engine.GetActiveFVGs(60, 0.0);
            var finalScore = fvgs.FirstOrDefault()?.Score ?? 0;

            Assert(fvgs.Count > 0, 
                   "EdgeCase_VeryOldFVG_StillExists", 
                   "Very old FVG should still exist");

            Assert(finalScore < initialScore * 0.1, 
                   "EdgeCase_VeryOldFVG_VeryLowScore", 
                   $"Expected very low score after 200 bars, initial:{initialScore:F3}, final:{finalScore:F3}");

            _logger($"  Score decay over 200 bars: {initialScore:F3} → {finalScore:F3}");
        }
    }
}

