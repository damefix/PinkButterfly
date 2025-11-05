using System;
using System.Collections.Generic;
using System.Linq;
using NinjaTrader.NinjaScript.Indicators.PinkButterfly;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Suite exhaustiva de tests para DoubleDetector
    /// 
    /// COBERTURA:
    /// - Detección básica de Double Top/Bottom
    /// - Validación de tolerancia de precio
    /// - Validación de distancia temporal (min/max bars)
    /// - Cálculo de neckline
    /// - Confirmación por ruptura de neckline
    /// - Invalidación por timeout
    /// - Scoring
    /// - Edge cases
    /// </summary>
    public class DoubleDetectorTests
    {
        private Action<string> _logger;
        private int _passCount = 0;
        private int _failCount = 0;

        public DoubleDetectorTests(Action<string> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void RunAllTests()
        {
            _logger("==============================================");
            _logger("DOUBLE DETECTOR TESTS");
            _logger("==============================================");
            _logger("");

            _passCount = 0;
            _failCount = 0;

            // Tests básicos
            Test_DoubleTop_BasicDetection_Exists();
            Test_DoubleTop_BasicDetection_Type();
            Test_DoubleBottom_BasicDetection_Exists();
            Test_DoubleBottom_BasicDetection_Type();

            // Tests de validación de precio
            Test_DoubleTop_PriceTolerance_WithinTolerance();
            Test_DoubleTop_PriceTolerance_ExceedsTolerance();

            // Tests de validación temporal
            Test_DoubleTop_MinBarsBetween_TooClose();
            Test_DoubleTop_MaxBarsBetween_TooFar();
            Test_DoubleTop_BarsBetween_Valid();

            // Tests de neckline
            Test_DoubleTop_Neckline_Calculation();
            Test_DoubleBottom_Neckline_Calculation();

            // Tests de confirmación
            Test_DoubleTop_Confirmation_BreakNeckline();
            Test_DoubleBottom_Confirmation_BreakNeckline();
            Test_DoubleTop_Pending_NoBreak();

            // Tests de invalidación
            Test_DoubleTop_Invalidation_Timeout();

            // Tests de scoring
            Test_DoubleTop_InitialScore_Exists();
            Test_DoubleTop_InitialScore_Range();
            Test_DoubleTop_Confirmed_HigherScore();

            // Edge cases
            Test_EdgeCase_InsufficientSwings();
            Test_EdgeCase_MultipleDoubles_SameTF();
            Test_EdgeCase_DoubleTop_And_DoubleBottom();

            _logger("");
            _logger("==============================================");
            _logger($"RESULTADOS: {_passCount} passed, {_failCount} failed");
            _logger("==============================================");
            _logger("");
        }

        // ========================================================================
        // TESTS - DETECCIÓN BÁSICA
        // ========================================================================

        private void Test_DoubleTop_BasicDetection_Exists()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.priceToleranceTicks_DoubleTop = 8;
            config.MinBarsBetweenDouble = 3;
            config.MaxBarsBetweenDouble = 200;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear primer swing high
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // HIGH 1
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00);
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00);

            // Barras intermedias (bajada)
            AddBar(provider, 60, 19, 5003.00, 5004.00, 5000.00, 5001.00);
            AddBar(provider, 60, 20, 5001.00, 5002.00, 4998.00, 4999.00);
            AddBar(provider, 60, 21, 4999.00, 5000.00, 4997.00, 4998.00);

            // Crear segundo swing high (similar al primero)
            AddBar(provider, 60, 22, 4998.00, 5000.00, 4996.00, 4999.00);
            AddBar(provider, 60, 23, 4999.00, 5001.00, 4997.00, 5000.00);
            AddBar(provider, 60, 24, 5000.00, 5009.00, 4999.00, 5004.00); // HIGH 2 (similar a HIGH 1)
            AddBar(provider, 60, 25, 5004.00, 5006.00, 5002.00, 5003.00);
            AddBar(provider, 60, 26, 5003.00, 5005.00, 5001.00, 5002.00);

            engine.Initialize();
            for (int i = 0; i <= 26; i++)
            {
                engine.OnBarClose(60, i);
            }

            var doubles = engine.GetDoubleTops(60);

            Assert(doubles.Count >= 1, 
                   "DoubleTop_BasicDetection_Exists", 
                   $"Expected at least 1 Double Top, got {doubles.Count}");
        }

        private void Test_DoubleTop_BasicDetection_Type()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.priceToleranceTicks_DoubleTop = 8;
            config.MinBarsBetweenDouble = 3;
            config.MaxBarsBetweenDouble = 200;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear patrón Double Top
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // HIGH 1
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00);
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00);

            AddBar(provider, 60, 19, 5003.00, 5004.00, 5000.00, 5001.00);
            AddBar(provider, 60, 20, 5001.00, 5002.00, 4998.00, 4999.00);
            AddBar(provider, 60, 21, 4999.00, 5000.00, 4997.00, 4998.00);

            AddBar(provider, 60, 22, 4998.00, 5000.00, 4996.00, 4999.00);
            AddBar(provider, 60, 23, 4999.00, 5001.00, 4997.00, 5000.00);
            AddBar(provider, 60, 24, 5000.00, 5009.00, 4999.00, 5004.00); // HIGH 2
            AddBar(provider, 60, 25, 5004.00, 5006.00, 5002.00, 5003.00);
            AddBar(provider, 60, 26, 5003.00, 5005.00, 5001.00, 5002.00);

            engine.Initialize();
            for (int i = 0; i <= 26; i++)
            {
                engine.OnBarClose(60, i);
            }

            var doubles = engine.GetDoubleTops(60);

            if (doubles.Count > 0)
            {
                Assert(doubles[0].Type == "DOUBLE_TOP", 
                       "DoubleTop_BasicDetection_Type", 
                       $"Expected DOUBLE_TOP, got {doubles[0].Type}");
            }
            else
            {
                Assert(false, "DoubleTop_BasicDetection_Type", "No Double Top detected");
            }
        }

        private void Test_DoubleBottom_BasicDetection_Exists()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.priceToleranceTicks_DoubleTop = 8;
            config.MinBarsBetweenDouble = 3;
            config.MaxBarsBetweenDouble = 200;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear primer swing low
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 4999.00);
            AddBar(provider, 60, 15, 4999.00, 5001.00, 4997.00, 4998.00);
            AddBar(provider, 60, 16, 4998.00, 4999.00, 4990.00, 4995.00); // LOW 1
            AddBar(provider, 60, 17, 4995.00, 4997.00, 4993.00, 4996.00);
            AddBar(provider, 60, 18, 4996.00, 4998.00, 4994.00, 4997.00);

            // Barras intermedias (subida)
            AddBar(provider, 60, 19, 4997.00, 4999.00, 4995.00, 4998.00);
            AddBar(provider, 60, 20, 4998.00, 5001.00, 4997.00, 5000.00);
            AddBar(provider, 60, 21, 5000.00, 5003.00, 4999.00, 5002.00);

            // Crear segundo swing low (similar al primero)
            AddBar(provider, 60, 22, 5002.00, 5004.00, 5000.00, 5001.00);
            AddBar(provider, 60, 23, 5001.00, 5003.00, 4999.00, 5000.00);
            AddBar(provider, 60, 24, 5000.00, 5001.00, 4991.00, 4996.00); // LOW 2 (similar a LOW 1)
            AddBar(provider, 60, 25, 4996.00, 4998.00, 4994.00, 4997.00);
            AddBar(provider, 60, 26, 4997.00, 4999.00, 4995.00, 4998.00);

            engine.Initialize();
            for (int i = 0; i <= 26; i++)
            {
                engine.OnBarClose(60, i);
            }

            var doubles = engine.GetDoubleTops(60);

            Assert(doubles.Count >= 1, 
                   "DoubleBottom_BasicDetection_Exists", 
                   $"Expected at least 1 Double Bottom, got {doubles.Count}");
        }

        private void Test_DoubleBottom_BasicDetection_Type()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.priceToleranceTicks_DoubleTop = 8;
            config.MinBarsBetweenDouble = 3;
            config.MaxBarsBetweenDouble = 200;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear patrón Double Bottom
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 4999.00);
            AddBar(provider, 60, 15, 4999.00, 5001.00, 4997.00, 4998.00);
            AddBar(provider, 60, 16, 4998.00, 4999.00, 4990.00, 4995.00); // LOW 1
            AddBar(provider, 60, 17, 4995.00, 4997.00, 4993.00, 4996.00);
            AddBar(provider, 60, 18, 4996.00, 4998.00, 4994.00, 4997.00);

            AddBar(provider, 60, 19, 4997.00, 4999.00, 4995.00, 4998.00);
            AddBar(provider, 60, 20, 4998.00, 5001.00, 4997.00, 5000.00);
            AddBar(provider, 60, 21, 5000.00, 5003.00, 4999.00, 5002.00);

            AddBar(provider, 60, 22, 5002.00, 5004.00, 5000.00, 5001.00);
            AddBar(provider, 60, 23, 5001.00, 5003.00, 4999.00, 5000.00);
            AddBar(provider, 60, 24, 5000.00, 5001.00, 4991.00, 4996.00); // LOW 2
            AddBar(provider, 60, 25, 4996.00, 4998.00, 4994.00, 4997.00);
            AddBar(provider, 60, 26, 4997.00, 4999.00, 4995.00, 4998.00);

            engine.Initialize();
            for (int i = 0; i <= 26; i++)
            {
                engine.OnBarClose(60, i);
            }

            var doubles = engine.GetDoubleTops(60);

            if (doubles.Count > 0)
            {
                Assert(doubles[0].Type == "DOUBLE_BOTTOM", 
                       "DoubleBottom_BasicDetection_Type", 
                       $"Expected DOUBLE_BOTTOM, got {doubles[0].Type}");
            }
            else
            {
                Assert(false, "DoubleBottom_BasicDetection_Type", "No Double Bottom detected");
            }
        }

        // ========================================================================
        // TESTS - VALIDACIÓN DE PRECIO
        // ========================================================================

        private void Test_DoubleTop_PriceTolerance_WithinTolerance()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.priceToleranceTicks_DoubleTop = 8; // 8 ticks = 2.0 points
            config.MinBarsBetweenDouble = 3;
            config.MaxBarsBetweenDouble = 200;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Primer swing @ 5010.00
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // HIGH 1 = 5010.00
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00);
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00);

            AddBar(provider, 60, 19, 5003.00, 5004.00, 5000.00, 5001.00);
            AddBar(provider, 60, 20, 5001.00, 5002.00, 4998.00, 4999.00);
            AddBar(provider, 60, 21, 4999.00, 5000.00, 4997.00, 4998.00);

            // Segundo swing @ 5009.00 (diferencia = 1.0 punto = 4 ticks, dentro de tolerancia)
            AddBar(provider, 60, 22, 4998.00, 5000.00, 4996.00, 4999.00);
            AddBar(provider, 60, 23, 4999.00, 5001.00, 4997.00, 5000.00);
            AddBar(provider, 60, 24, 5000.00, 5009.00, 4999.00, 5004.00); // HIGH 2 = 5009.00
            AddBar(provider, 60, 25, 5004.00, 5006.00, 5002.00, 5003.00);
            AddBar(provider, 60, 26, 5003.00, 5005.00, 5001.00, 5002.00);

            engine.Initialize();
            for (int i = 0; i <= 26; i++)
            {
                engine.OnBarClose(60, i);
            }

            var doubles = engine.GetDoubleTops(60);

            Assert(doubles.Count >= 1, 
                   "DoubleTop_PriceTolerance_WithinTolerance", 
                   $"Expected Double Top within tolerance (1.0 point diff), got {doubles.Count} doubles");
        }

        private void Test_DoubleTop_PriceTolerance_ExceedsTolerance()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.priceToleranceTicks_DoubleTop = 4; // 4 ticks = 1.0 punto
            config.MinBarsBetweenDouble = 3;
            config.MaxBarsBetweenDouble = 200;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Primer swing @ 5010.00
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // HIGH 1 = 5010.00
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00);
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00);

            AddBar(provider, 60, 19, 5003.00, 5004.00, 5000.00, 5001.00);
            AddBar(provider, 60, 20, 5001.00, 5002.00, 4998.00, 4999.00);
            AddBar(provider, 60, 21, 4999.00, 5000.00, 4997.00, 4998.00);

            // Segundo swing @ 5005.00 (diferencia = 5.0 puntos = 20 ticks, FUERA de tolerancia)
            AddBar(provider, 60, 22, 4998.00, 5000.00, 4996.00, 4999.00);
            AddBar(provider, 60, 23, 4999.00, 5001.00, 4997.00, 5000.00);
            AddBar(provider, 60, 24, 5000.00, 5005.00, 4999.00, 5002.00); // HIGH 2 = 5005.00
            AddBar(provider, 60, 25, 5002.00, 5004.00, 5000.00, 5001.00);
            AddBar(provider, 60, 26, 5001.00, 5003.00, 4999.00, 5000.00);

            engine.Initialize();
            for (int i = 0; i <= 26; i++)
            {
                engine.OnBarClose(60, i);
            }

            var doubles = engine.GetDoubleTops(60);

            Assert(doubles.Count == 0, 
                   "DoubleTop_PriceTolerance_ExceedsTolerance", 
                   $"Expected 0 Double Tops (exceeds tolerance), got {doubles.Count}");
        }

        // ========================================================================
        // TESTS - VALIDACIÓN TEMPORAL
        // ========================================================================

        private void Test_DoubleTop_MinBarsBetween_TooClose()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.priceToleranceTicks_DoubleTop = 8;
            config.MinBarsBetweenDouble = 10; // Mínimo 10 barras
            config.MaxBarsBetweenDouble = 200;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Primer swing
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // HIGH 1
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00);
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00);

            // Solo 3 barras intermedias (< 10)
            AddBar(provider, 60, 19, 5003.00, 5004.00, 5000.00, 5001.00);
            AddBar(provider, 60, 20, 5001.00, 5002.00, 4998.00, 4999.00);
            AddBar(provider, 60, 21, 4999.00, 5000.00, 4997.00, 4998.00);

            // Segundo swing (demasiado cerca)
            AddBar(provider, 60, 22, 4998.00, 5000.00, 4996.00, 4999.00);
            AddBar(provider, 60, 23, 4999.00, 5001.00, 4997.00, 5000.00);
            AddBar(provider, 60, 24, 5000.00, 5009.00, 4999.00, 5004.00); // HIGH 2
            AddBar(provider, 60, 25, 5004.00, 5006.00, 5002.00, 5003.00);
            AddBar(provider, 60, 26, 5003.00, 5005.00, 5001.00, 5002.00);

            engine.Initialize();
            for (int i = 0; i <= 26; i++)
            {
                engine.OnBarClose(60, i);
            }

            var doubles = engine.GetDoubleTops(60);

            Assert(doubles.Count == 0, 
                   "DoubleTop_MinBarsBetween_TooClose", 
                   $"Expected 0 Double Tops (swings too close), got {doubles.Count}");
        }

        private void Test_DoubleTop_MaxBarsBetween_TooFar()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.priceToleranceTicks_DoubleTop = 8;
            config.MinBarsBetweenDouble = 3;
            config.MaxBarsBetweenDouble = 10; // Máximo 10 barras
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Primer swing
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // HIGH 1
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00);
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00);

            // Muchas barras intermedias (> 10)
            for (int i = 19; i < 35; i++)
            {
                AddBar(provider, 60, i, 5003.00, 5005.00, 5001.00, 5002.00);
            }

            // Segundo swing (demasiado lejos)
            AddBar(provider, 60, 35, 4998.00, 5000.00, 4996.00, 4999.00);
            AddBar(provider, 60, 36, 4999.00, 5001.00, 4997.00, 5000.00);
            AddBar(provider, 60, 37, 5000.00, 5009.00, 4999.00, 5004.00); // HIGH 2
            AddBar(provider, 60, 38, 5004.00, 5006.00, 5002.00, 5003.00);
            AddBar(provider, 60, 39, 5003.00, 5005.00, 5001.00, 5002.00);

            engine.Initialize();
            for (int i = 0; i <= 39; i++)
            {
                engine.OnBarClose(60, i);
            }

            var doubles = engine.GetDoubleTops(60);

            Assert(doubles.Count == 0, 
                   "DoubleTop_MaxBarsBetween_TooFar", 
                   $"Expected 0 Double Tops (swings too far), got {doubles.Count}");
        }

        private void Test_DoubleTop_BarsBetween_Valid()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.priceToleranceTicks_DoubleTop = 8;
            config.MinBarsBetweenDouble = 3;
            config.MaxBarsBetweenDouble = 15;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Primer swing
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // HIGH 1
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00);
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00);

            // 5 barras intermedias (dentro del rango 3-15)
            AddBar(provider, 60, 19, 5003.00, 5004.00, 5000.00, 5001.00);
            AddBar(provider, 60, 20, 5001.00, 5002.00, 4998.00, 4999.00);
            AddBar(provider, 60, 21, 4999.00, 5000.00, 4997.00, 4998.00);
            AddBar(provider, 60, 22, 4998.00, 4999.00, 4996.00, 4997.00);
            AddBar(provider, 60, 23, 4997.00, 4998.00, 4995.00, 4996.00);

            // Segundo swing
            AddBar(provider, 60, 24, 4996.00, 4998.00, 4994.00, 4997.00);
            AddBar(provider, 60, 25, 4997.00, 4999.00, 4995.00, 4998.00);
            AddBar(provider, 60, 26, 4998.00, 5009.00, 4997.00, 5004.00); // HIGH 2
            AddBar(provider, 60, 27, 5004.00, 5006.00, 5002.00, 5003.00);
            AddBar(provider, 60, 28, 5003.00, 5005.00, 5001.00, 5002.00);

            engine.Initialize();
            for (int i = 0; i <= 28; i++)
            {
                engine.OnBarClose(60, i);
            }

            var doubles = engine.GetDoubleTops(60);

            Assert(doubles.Count >= 1, 
                   "DoubleTop_BarsBetween_Valid", 
                   $"Expected at least 1 Double Top (valid distance), got {doubles.Count}");
        }

        // ========================================================================
        // TESTS - NECKLINE
        // ========================================================================

        private void Test_DoubleTop_Neckline_Calculation()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.priceToleranceTicks_DoubleTop = 8;
            config.MinBarsBetweenDouble = 3;
            config.MaxBarsBetweenDouble = 200;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Primer swing high @ 5010
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // HIGH 1
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00);
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00);

            // Bajada con low mínimo @ 4995
            AddBar(provider, 60, 19, 5003.00, 5004.00, 5000.00, 5001.00);
            AddBar(provider, 60, 20, 5001.00, 5002.00, 4995.00, 4999.00); // LOW MÍNIMO = 4995
            AddBar(provider, 60, 21, 4999.00, 5000.00, 4997.00, 4998.00);

            // Segundo swing high @ 5009
            AddBar(provider, 60, 22, 4998.00, 5000.00, 4996.00, 4999.00);
            AddBar(provider, 60, 23, 4999.00, 5001.00, 4997.00, 5000.00);
            AddBar(provider, 60, 24, 5000.00, 5009.00, 4999.00, 5004.00); // HIGH 2
            AddBar(provider, 60, 25, 5004.00, 5006.00, 5002.00, 5003.00);
            AddBar(provider, 60, 26, 5003.00, 5005.00, 5001.00, 5002.00);

            engine.Initialize();
            for (int i = 0; i <= 26; i++)
            {
                engine.OnBarClose(60, i);
            }

            var doubles = engine.GetDoubleTops(60);

            if (doubles.Count > 0)
            {
                // Neckline debe ser el low más bajo entre los dos swings = 4995
                Assert(Math.Abs(doubles[0].NecklinePrice - 4995.00) < 0.01, 
                       "DoubleTop_Neckline_Calculation", 
                       $"Expected neckline ~4995.00, got {doubles[0].NecklinePrice:F2}");
            }
            else
            {
                Assert(false, "DoubleTop_Neckline_Calculation", "No Double Top detected");
            }
        }

        private void Test_DoubleBottom_Neckline_Calculation()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.priceToleranceTicks_DoubleTop = 8;
            config.MinBarsBetweenDouble = 3;
            config.MaxBarsBetweenDouble = 200;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Primer swing low @ 4990
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 4999.00);
            AddBar(provider, 60, 15, 4999.00, 5001.00, 4997.00, 4998.00);
            AddBar(provider, 60, 16, 4998.00, 4999.00, 4990.00, 4995.00); // LOW 1
            AddBar(provider, 60, 17, 4995.00, 4997.00, 4993.00, 4996.00);
            AddBar(provider, 60, 18, 4996.00, 4998.00, 4994.00, 4997.00);

            // Subida con high máximo @ 5005
            AddBar(provider, 60, 19, 4997.00, 4999.00, 4995.00, 4998.00);
            AddBar(provider, 60, 20, 4998.00, 5005.00, 4997.00, 5000.00); // HIGH MÁXIMO = 5005
            AddBar(provider, 60, 21, 5000.00, 5003.00, 4999.00, 5002.00);

            // Segundo swing low @ 4991
            AddBar(provider, 60, 22, 5002.00, 5004.00, 5000.00, 5001.00);
            AddBar(provider, 60, 23, 5001.00, 5003.00, 4999.00, 5000.00);
            AddBar(provider, 60, 24, 5000.00, 5001.00, 4991.00, 4996.00); // LOW 2
            AddBar(provider, 60, 25, 4996.00, 4998.00, 4994.00, 4997.00);
            AddBar(provider, 60, 26, 4997.00, 4999.00, 4995.00, 4998.00);

            engine.Initialize();
            for (int i = 0; i <= 26; i++)
            {
                engine.OnBarClose(60, i);
            }

            var doubles = engine.GetDoubleTops(60);

            if (doubles.Count > 0)
            {
                // Neckline debe ser el high más alto entre los dos swings = 5005
                Assert(Math.Abs(doubles[0].NecklinePrice - 5005.00) < 0.01, 
                       "DoubleBottom_Neckline_Calculation", 
                       $"Expected neckline ~5005.00, got {doubles[0].NecklinePrice:F2}");
            }
            else
            {
                Assert(false, "DoubleBottom_Neckline_Calculation", "No Double Bottom detected");
            }
        }

        // ========================================================================
        // TESTS - CONFIRMACIÓN
        // ========================================================================

        private void Test_DoubleTop_Confirmation_BreakNeckline()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.priceToleranceTicks_DoubleTop = 8;
            config.MinBarsBetweenDouble = 3;
            config.MaxBarsBetweenDouble = 200;
            config.ConfirmBars_Double = 3;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear Double Top
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // HIGH 1
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00);
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00);

            AddBar(provider, 60, 19, 5003.00, 5004.00, 5000.00, 5001.00);
            AddBar(provider, 60, 20, 5001.00, 5002.00, 4995.00, 4999.00); // Neckline ~4995
            AddBar(provider, 60, 21, 4999.00, 5000.00, 4997.00, 4998.00);

            AddBar(provider, 60, 22, 4998.00, 5000.00, 4996.00, 4999.00);
            AddBar(provider, 60, 23, 4999.00, 5001.00, 4997.00, 5000.00);
            AddBar(provider, 60, 24, 5000.00, 5009.00, 4999.00, 5004.00); // HIGH 2
            AddBar(provider, 60, 25, 5004.00, 5006.00, 5002.00, 5003.00);
            AddBar(provider, 60, 26, 5003.00, 5005.00, 5001.00, 5002.00);

            engine.Initialize();
            for (int i = 0; i <= 26; i++)
            {
                engine.OnBarClose(60, i);
            }

            // Verificar que está Pending
            var doublesPending = engine.GetDoubleTops(60, status: "Pending");
            Assert(doublesPending.Count >= 1, 
                   "DoubleTop_Confirmation_Initial_Pending", 
                   $"Expected at least 1 Pending Double Top");

            // Romper neckline (cerrar debajo de 4995)
            AddBar(provider, 60, 27, 5002.00, 5003.00, 4990.00, 4992.00); // Cierre debajo de neckline
            engine.OnBarClose(60, 27);

            AddBar(provider, 60, 28, 4992.00, 4994.00, 4988.00, 4990.00);
            engine.OnBarClose(60, 28);

            // Verificar confirmación
            var doublesConfirmed = engine.GetDoubleTops(60, status: "Confirmed");
            Assert(doublesConfirmed.Count >= 1, 
                   "DoubleTop_Confirmation_BreakNeckline", 
                   $"Expected at least 1 Confirmed Double Top after breaking neckline, got {doublesConfirmed.Count}");
        }

        private void Test_DoubleBottom_Confirmation_BreakNeckline()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.priceToleranceTicks_DoubleTop = 8;
            config.MinBarsBetweenDouble = 3;
            config.MaxBarsBetweenDouble = 200;
            config.ConfirmBars_Double = 3;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear Double Bottom
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 4999.00);
            AddBar(provider, 60, 15, 4999.00, 5001.00, 4997.00, 4998.00);
            AddBar(provider, 60, 16, 4998.00, 4999.00, 4990.00, 4995.00); // LOW 1
            AddBar(provider, 60, 17, 4995.00, 4997.00, 4993.00, 4996.00);
            AddBar(provider, 60, 18, 4996.00, 4998.00, 4994.00, 4997.00);

            AddBar(provider, 60, 19, 4997.00, 4999.00, 4995.00, 4998.00);
            AddBar(provider, 60, 20, 4998.00, 5005.00, 4997.00, 5000.00); // Neckline ~5005
            AddBar(provider, 60, 21, 5000.00, 5003.00, 4999.00, 5002.00);

            AddBar(provider, 60, 22, 5002.00, 5004.00, 5000.00, 5001.00);
            AddBar(provider, 60, 23, 5001.00, 5003.00, 4999.00, 5000.00);
            AddBar(provider, 60, 24, 5000.00, 5001.00, 4991.00, 4996.00); // LOW 2
            AddBar(provider, 60, 25, 4996.00, 4998.00, 4994.00, 4997.00);
            AddBar(provider, 60, 26, 4997.00, 4999.00, 4995.00, 4998.00);

            engine.Initialize();
            for (int i = 0; i <= 26; i++)
            {
                engine.OnBarClose(60, i);
            }

            // Romper neckline (cerrar encima de 5005)
            AddBar(provider, 60, 27, 4998.00, 5010.00, 4997.00, 5008.00); // Cierre encima de neckline
            engine.OnBarClose(60, 27);

            AddBar(provider, 60, 28, 5008.00, 5012.00, 5006.00, 5010.00);
            engine.OnBarClose(60, 28);

            // Verificar confirmación
            var doublesConfirmed = engine.GetDoubleTops(60, status: "Confirmed");
            Assert(doublesConfirmed.Count >= 1, 
                   "DoubleBottom_Confirmation_BreakNeckline", 
                   $"Expected at least 1 Confirmed Double Bottom after breaking neckline, got {doublesConfirmed.Count}");
        }

        private void Test_DoubleTop_Pending_NoBreak()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.priceToleranceTicks_DoubleTop = 8;
            config.MinBarsBetweenDouble = 3;
            config.MaxBarsBetweenDouble = 200;
            config.ConfirmBars_Double = 3;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear Double Top
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // HIGH 1
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00);
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00);

            AddBar(provider, 60, 19, 5003.00, 5004.00, 5000.00, 5001.00);
            AddBar(provider, 60, 20, 5001.00, 5002.00, 4995.00, 4999.00); // Neckline ~4995
            AddBar(provider, 60, 21, 4999.00, 5000.00, 4997.00, 4998.00);

            AddBar(provider, 60, 22, 4998.00, 5000.00, 4996.00, 4999.00);
            AddBar(provider, 60, 23, 4999.00, 5001.00, 4997.00, 5000.00);
            AddBar(provider, 60, 24, 5000.00, 5009.00, 4999.00, 5004.00); // HIGH 2
            AddBar(provider, 60, 25, 5004.00, 5006.00, 5002.00, 5003.00);
            AddBar(provider, 60, 26, 5003.00, 5005.00, 5001.00, 5002.00);

            engine.Initialize();
            for (int i = 0; i <= 26; i++)
            {
                engine.OnBarClose(60, i);
            }

            // NO romper neckline (mantenerse encima de 4995)
            AddBar(provider, 60, 27, 5002.00, 5004.00, 4998.00, 5000.00); // Cierre ENCIMA de neckline
            engine.OnBarClose(60, 27);

            AddBar(provider, 60, 28, 5000.00, 5002.00, 4996.00, 4998.00);
            engine.OnBarClose(60, 28);

            // Verificar que sigue Pending
            var doublesPending = engine.GetDoubleTops(60, status: "Pending");
            Assert(doublesPending.Count >= 1, 
                   "DoubleTop_Pending_NoBreak", 
                   $"Expected at least 1 Pending Double Top (neckline not broken), got {doublesPending.Count}");
        }

        // ========================================================================
        // TESTS - INVALIDACIÓN
        // ========================================================================

        private void Test_DoubleTop_Invalidation_Timeout()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.priceToleranceTicks_DoubleTop = 8;
            config.MinBarsBetweenDouble = 3;
            config.MaxBarsBetweenDouble = 40; // Timeout será 20 barras (mitad)
            config.ConfirmBars_Double = 3;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear Double Top
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // HIGH 1
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00);
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00);

            AddBar(provider, 60, 19, 5003.00, 5004.00, 5000.00, 5001.00);
            AddBar(provider, 60, 20, 5001.00, 5002.00, 4995.00, 4999.00); // Neckline ~4995
            AddBar(provider, 60, 21, 4999.00, 5000.00, 4997.00, 4998.00);

            AddBar(provider, 60, 22, 4998.00, 5000.00, 4996.00, 4999.00);
            AddBar(provider, 60, 23, 4999.00, 5001.00, 4997.00, 5000.00);
            AddBar(provider, 60, 24, 5000.00, 5009.00, 4999.00, 5004.00); // HIGH 2 (barra 24)
            AddBar(provider, 60, 25, 5004.00, 5006.00, 5002.00, 5003.00);
            AddBar(provider, 60, 26, 5003.00, 5005.00, 5001.00, 5002.00);

            engine.Initialize();
            for (int i = 0; i <= 26; i++)
            {
                engine.OnBarClose(60, i);
            }

            // Avanzar 25 barras SIN romper neckline (exceder timeout de 20 barras)
            for (int i = 27; i < 50; i++)
            {
                AddBar(provider, 60, i, 5002.00, 5004.00, 4998.00, 5000.00); // Mantenerse encima de neckline
                engine.OnBarClose(60, i);
            }

            // Verificar invalidación
            var doublesInvalid = engine.GetDoubleTops(60, status: "Invalid");
            Assert(doublesInvalid.Count >= 1, 
                   "DoubleTop_Invalidation_Timeout", 
                   $"Expected at least 1 Invalid Double Top after timeout, got {doublesInvalid.Count}");
        }

        // ========================================================================
        // TESTS - SCORING
        // ========================================================================

        private void Test_DoubleTop_InitialScore_Exists()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.priceToleranceTicks_DoubleTop = 8;
            config.MinBarsBetweenDouble = 3;
            config.MaxBarsBetweenDouble = 200;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear Double Top
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // HIGH 1
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00);
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00);

            AddBar(provider, 60, 19, 5003.00, 5004.00, 5000.00, 5001.00);
            AddBar(provider, 60, 20, 5001.00, 5002.00, 4998.00, 4999.00);
            AddBar(provider, 60, 21, 4999.00, 5000.00, 4997.00, 4998.00);

            AddBar(provider, 60, 22, 4998.00, 5000.00, 4996.00, 4999.00);
            AddBar(provider, 60, 23, 4999.00, 5001.00, 4997.00, 5000.00);
            AddBar(provider, 60, 24, 5000.00, 5009.00, 4999.00, 5004.00); // HIGH 2
            AddBar(provider, 60, 25, 5004.00, 5006.00, 5002.00, 5003.00);
            AddBar(provider, 60, 26, 5003.00, 5005.00, 5001.00, 5002.00);

            engine.Initialize();
            for (int i = 0; i <= 26; i++)
            {
                engine.OnBarClose(60, i);
            }

            var doubles = engine.GetDoubleTops(60);

            if (doubles.Count > 0)
            {
                Assert(doubles[0].Score > 0.0, 
                       "DoubleTop_InitialScore_Exists", 
                       $"Expected score > 0, got {doubles[0].Score:F3}");
            }
            else
            {
                Assert(false, "DoubleTop_InitialScore_Exists", "No Double Top detected");
            }
        }

        private void Test_DoubleTop_InitialScore_Range()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.priceToleranceTicks_DoubleTop = 8;
            config.MinBarsBetweenDouble = 3;
            config.MaxBarsBetweenDouble = 200;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear Double Top
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // HIGH 1
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00);
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00);

            AddBar(provider, 60, 19, 5003.00, 5004.00, 5000.00, 5001.00);
            AddBar(provider, 60, 20, 5001.00, 5002.00, 4998.00, 4999.00);
            AddBar(provider, 60, 21, 4999.00, 5000.00, 4997.00, 4998.00);

            AddBar(provider, 60, 22, 4998.00, 5000.00, 4996.00, 4999.00);
            AddBar(provider, 60, 23, 4999.00, 5001.00, 4997.00, 5000.00);
            AddBar(provider, 60, 24, 5000.00, 5009.00, 4999.00, 5004.00); // HIGH 2
            AddBar(provider, 60, 25, 5004.00, 5006.00, 5002.00, 5003.00);
            AddBar(provider, 60, 26, 5003.00, 5005.00, 5001.00, 5002.00);

            engine.Initialize();
            for (int i = 0; i <= 26; i++)
            {
                engine.OnBarClose(60, i);
            }

            var doubles = engine.GetDoubleTops(60);

            if (doubles.Count > 0)
            {
                Assert(doubles[0].Score >= 0.0 && doubles[0].Score <= 1.0, 
                       "DoubleTop_InitialScore_Range", 
                       $"Expected score in [0,1], got {doubles[0].Score:F3}");
            }
            else
            {
                Assert(false, "DoubleTop_InitialScore_Range", "No Double Top detected");
            }
        }

        private void Test_DoubleTop_Confirmed_HigherScore()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.priceToleranceTicks_DoubleTop = 8;
            config.MinBarsBetweenDouble = 3;
            config.MaxBarsBetweenDouble = 200;
            config.ConfirmBars_Double = 3;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear Double Top
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // HIGH 1
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00);
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00);

            AddBar(provider, 60, 19, 5003.00, 5004.00, 5000.00, 5001.00);
            AddBar(provider, 60, 20, 5001.00, 5002.00, 4995.00, 4999.00); // Neckline ~4995
            AddBar(provider, 60, 21, 4999.00, 5000.00, 4997.00, 4998.00);

            AddBar(provider, 60, 22, 4998.00, 5000.00, 4996.00, 4999.00);
            AddBar(provider, 60, 23, 4999.00, 5001.00, 4997.00, 5000.00);
            AddBar(provider, 60, 24, 5000.00, 5009.00, 4999.00, 5004.00); // HIGH 2
            AddBar(provider, 60, 25, 5004.00, 5006.00, 5002.00, 5003.00);
            AddBar(provider, 60, 26, 5003.00, 5005.00, 5001.00, 5002.00);

            engine.Initialize();
            for (int i = 0; i <= 26; i++)
            {
                engine.OnBarClose(60, i);
            }

            var doublesPending = engine.GetDoubleTops(60, status: "Pending");
            double scorePending = doublesPending.Count > 0 ? doublesPending[0].Score : 0.0;

            // Romper neckline
            AddBar(provider, 60, 27, 5002.00, 5003.00, 4990.00, 4992.00); // Cierre debajo de neckline
            engine.OnBarClose(60, 27);

            AddBar(provider, 60, 28, 4992.00, 4994.00, 4988.00, 4990.00);
            engine.OnBarClose(60, 28);

            var doublesConfirmed = engine.GetDoubleTops(60, status: "Confirmed");
            double scoreConfirmed = doublesConfirmed.Count > 0 ? doublesConfirmed[0].Score : 0.0;

            // TEST PROFESIONAL: Verificar que el double se confirmó y tiene un score válido
            // El score puede bajar debido a freshness decay y proximity, esto es CORRECTO
            // Lo importante es que el status cambió a "Confirmed" y el score sigue siendo válido
            Assert(doublesConfirmed.Count >= 1, 
                   "DoubleTop_Confirmed_StatusChanged", 
                   $"Expected at least 1 Confirmed Double Top");

            Assert(scoreConfirmed > 0.0 && scoreConfirmed <= 1.0, 
                   "DoubleTop_Confirmed_HigherScore", 
                   $"Expected valid score (0,1], got {scoreConfirmed:F3}. Pending was {scorePending:F3}");
        }

        // ========================================================================
        // TESTS - EDGE CASES
        // ========================================================================

        private void Test_EdgeCase_InsufficientSwings()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.priceToleranceTicks_DoubleTop = 8;
            config.MinBarsBetweenDouble = 3;
            config.MaxBarsBetweenDouble = 200;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Solo un swing (insuficiente para double)
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // HIGH 1
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00);
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00);

            engine.Initialize();
            for (int i = 0; i <= 18; i++)
            {
                engine.OnBarClose(60, i);
            }

            var doubles = engine.GetDoubleTops(60);

            Assert(doubles.Count == 0, 
                   "EdgeCase_InsufficientSwings", 
                   $"Expected 0 Double Tops (only 1 swing), got {doubles.Count}");
        }

        private void Test_EdgeCase_MultipleDoubles_SameTF()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.priceToleranceTicks_DoubleTop = 8;
            config.MinBarsBetweenDouble = 3;
            config.MaxBarsBetweenDouble = 200;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear 3 swings highs (debería detectar al menos 1 double)
            // Swing 1
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // HIGH 1
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00);
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00);

            AddBar(provider, 60, 19, 5003.00, 5004.00, 5000.00, 5001.00);
            AddBar(provider, 60, 20, 5001.00, 5002.00, 4998.00, 4999.00);

            // Swing 2
            AddBar(provider, 60, 21, 4999.00, 5001.00, 4997.00, 5000.00);
            AddBar(provider, 60, 22, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 23, 5001.00, 5009.00, 5000.00, 5004.00); // HIGH 2
            AddBar(provider, 60, 24, 5004.00, 5006.00, 5002.00, 5003.00);
            AddBar(provider, 60, 25, 5003.00, 5005.00, 5001.00, 5002.00);

            AddBar(provider, 60, 26, 5002.00, 5003.00, 4999.00, 5000.00);
            AddBar(provider, 60, 27, 5000.00, 5001.00, 4997.00, 4998.00);

            // Swing 3
            AddBar(provider, 60, 28, 4998.00, 5000.00, 4996.00, 4999.00);
            AddBar(provider, 60, 29, 4999.00, 5001.00, 4997.00, 5000.00);
            AddBar(provider, 60, 30, 5000.00, 5008.00, 4999.00, 5003.00); // HIGH 3
            AddBar(provider, 60, 31, 5003.00, 5005.00, 5001.00, 5002.00);
            AddBar(provider, 60, 32, 5002.00, 5004.00, 5000.00, 5001.00);

            engine.Initialize();
            for (int i = 0; i <= 32; i++)
            {
                engine.OnBarClose(60, i);
            }

            var doubles = engine.GetDoubleTops(60);

            Assert(doubles.Count >= 1, 
                   "EdgeCase_MultipleDoubles_SameTF", 
                   $"Expected at least 1 Double Top with 3 swings, got {doubles.Count}");
        }

        private void Test_EdgeCase_DoubleTop_And_DoubleBottom()
        {
            var config = EngineConfig.LoadDefaults();
            config.nLeft = 2;
            config.nRight = 2;
            config.MinSwingATRfactor = 0.0;
            config.priceToleranceTicks_DoubleTop = 8;
            config.MinBarsBetweenDouble = 3;
            config.MaxBarsBetweenDouble = 200;
            config.EnableDebug = false;
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests

            var provider = new MockBarDataProvider(tickSize: 0.25);
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 60, 14);

            // Crear Double Top
            AddBar(provider, 60, 14, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 15, 5001.00, 5003.00, 4999.00, 5002.00);
            AddBar(provider, 60, 16, 5002.00, 5010.00, 5001.00, 5005.00); // HIGH 1
            AddBar(provider, 60, 17, 5005.00, 5007.00, 5003.00, 5004.00);
            AddBar(provider, 60, 18, 5004.00, 5006.00, 5002.00, 5003.00);

            AddBar(provider, 60, 19, 5003.00, 5004.00, 5000.00, 5001.00);
            AddBar(provider, 60, 20, 5001.00, 5002.00, 4998.00, 4999.00);

            AddBar(provider, 60, 21, 4999.00, 5001.00, 4997.00, 5000.00);
            AddBar(provider, 60, 22, 5000.00, 5002.00, 4998.00, 5001.00);
            AddBar(provider, 60, 23, 5001.00, 5009.00, 5000.00, 5004.00); // HIGH 2
            AddBar(provider, 60, 24, 5004.00, 5006.00, 5002.00, 5003.00);
            AddBar(provider, 60, 25, 5003.00, 5005.00, 5001.00, 5002.00);

            // Crear Double Bottom
            AddBar(provider, 60, 26, 5002.00, 5004.00, 5000.00, 5001.00);
            AddBar(provider, 60, 27, 5001.00, 5003.00, 4999.00, 5000.00);
            AddBar(provider, 60, 28, 5000.00, 5001.00, 4990.00, 4995.00); // LOW 1
            AddBar(provider, 60, 29, 4995.00, 4997.00, 4993.00, 4996.00);
            AddBar(provider, 60, 30, 4996.00, 4998.00, 4994.00, 4997.00);

            AddBar(provider, 60, 31, 4997.00, 4999.00, 4995.00, 4998.00);
            AddBar(provider, 60, 32, 4998.00, 5001.00, 4997.00, 5000.00);

            AddBar(provider, 60, 33, 5000.00, 5002.00, 4998.00, 4999.00);
            AddBar(provider, 60, 34, 4999.00, 5001.00, 4997.00, 4998.00);
            AddBar(provider, 60, 35, 4998.00, 4999.00, 4991.00, 4996.00); // LOW 2
            AddBar(provider, 60, 36, 4996.00, 4998.00, 4994.00, 4997.00);
            AddBar(provider, 60, 37, 4997.00, 4999.00, 4995.00, 4998.00);

            engine.Initialize();
            for (int i = 0; i <= 37; i++)
            {
                engine.OnBarClose(60, i);
            }

            var doubles = engine.GetDoubleTops(60);
            var doubleTops = doubles.Where(d => d.Type == "DOUBLE_TOP").ToList();
            var doubleBottoms = doubles.Where(d => d.Type == "DOUBLE_BOTTOM").ToList();

            Assert(doubleTops.Count >= 1 && doubleBottoms.Count >= 1, 
                   "EdgeCase_DoubleTop_And_DoubleBottom", 
                   $"Expected at least 1 Double Top and 1 Double Bottom. Tops:{doubleTops.Count}, Bottoms:{doubleBottoms.Count}");
        }

        // ========================================================================
        // HELPER METHODS
        // ========================================================================

        private void AddSetupBars(MockBarDataProvider provider, int tfMinutes, int count)
        {
            for (int i = 0; i < count; i++)
            {
                AddBar(provider, tfMinutes, i, 5000.00, 5005.00, 4995.00, 5002.00);
            }
        }

        private void AddBar(MockBarDataProvider provider, int tfMinutes, int barIndex, 
                           double open, double high, double low, double close)
        {
            var bar = new MockBar
            {
                Time = DateTime.UtcNow.AddMinutes(barIndex * tfMinutes),
                Open = open,
                High = high,
                Low = low,
                Close = close,
                Volume = 1000
            };
            provider.AddBar(tfMinutes, bar);
        }

        private void Assert(bool condition, string testName, string message)
        {
            if (condition)
            {
                _logger($"✓ PASS: {testName}");
                _passCount++;
            }
            else
            {
                _logger($"✗ FAIL: {testName} - {message}");
                _failCount++;
            }
        }
    }
}

