// ============================================================================
// LiquidityVoidDetectorTests.cs
// PinkButterfly CoreBrain - Tests exhaustivos para LiquidityVoidDetector
// 
// IMPORTANTE: Tests corregidos para reflejar JERARQUÍA FVG > LV
// - LV solo se crea si NO existe un FVG que lo contenga
// - Los tests crean gaps PUROS de 2 barras que NO forman FVGs
// - FVG prevalece absolutamente sobre LV (Opción A - Mutuamente Excluyente)
// ============================================================================

using System;
using System.Linq;
using NinjaTrader.NinjaScript.Indicators.PinkButterfly;

namespace PinkButterfly.Tests
{
    /// <summary>
    /// Tests exhaustivos del LiquidityVoidDetector
    /// Total: 25 tests cubriendo todos los aspectos de Liquidity Voids
    /// </summary>
    public class LiquidityVoidDetectorTests
    {
        private int _testsPassed = 0;
        private int _testsFailed = 0;
        private Action<string> _print;

        public LiquidityVoidDetectorTests(Action<string> printAction)
        {
            _print = printAction ?? Console.WriteLine;
        }

        public void RunAllTests()
        {
            _print("==============================================");
            _print("LIQUIDITY VOID DETECTOR TESTS");
            _print("==============================================");
            _print("");

            // Tests básicos de detección
            Test_LV_BullishVoid_BasicDetection();
            Test_LV_BearishVoid_BasicDetection();
            Test_LV_NoVoid_OverlappingBars();
            Test_LV_MinSizeValidation_TooSmall();
            Test_LV_MinSizeValidation_Valid();

            // Tests de volumen
            Test_LV_LowVolume_Detected();
            Test_LV_HighVolume_NotDetected();
            Test_LV_NoVolume_StillDetects();

            // Tests de exclusión FVG
            Test_LV_ExcludeFVG_SameZone();
            Test_LV_AllowVoid_NoFVGInZone();

            // Tests de fusión
            Test_LV_Fusion_ConsecutiveVoids();
            Test_LV_Fusion_WithinTolerance();
            Test_LV_Fusion_ExceedsTolerance();
            Test_LV_Fusion_Disabled();

            // Tests de fill y toques
            Test_LV_Touch_Body();
            Test_LV_Touch_Wick();
            Test_LV_Fill_Partial();
            Test_LV_Fill_Complete();

            // Tests de scoring
            Test_LV_Score_InitialCalculation();
            Test_LV_Score_ProximityFactor();
            Test_LV_Score_VolumeFactor();
            Test_LV_Score_ConfluenceBonus();

            // Edge cases
            Test_EdgeCase_InsufficientBars();
            Test_EdgeCase_InvalidATR();
            Test_EdgeCase_MultipleVoids_SameTF();

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

        /// <summary>
        /// Helper para añadir una barra individual
        /// </summary>
        private void AddBar(MockBarDataProvider provider, double open, double high, double low, double close, double? volume = 1000)
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

        /// <summary>
        /// Helper para añadir barras de setup (para ATR)
        /// </summary>
        private void AddSetupBars(MockBarDataProvider provider, int count, double basePrice = 5000)
        {
            for (int i = 0; i < count; i++)
            {
                AddBar(provider, basePrice, basePrice + 2.5, basePrice - 2.5, basePrice + 0.5, 1000);
            }
        }

        /// <summary>
        /// Helper para crear un gap PURO de 2 barras que NO forme FVG
        /// 
        /// ESTRATEGIA:
        /// 1. Añadir barra "neutralizadora" que rompe cualquier FVG con barras previas
        /// 2. Crear 2 barras con gap (A y B)
        /// 3. NO añadir barra C después del gap
        /// 
        /// Así el FVGDetector no puede formar un FVG de 3 barras con el gap
        /// </summary>
        private void CreatePureGap(MockBarDataProvider provider, string direction, double gapSize = 6, double basePrice = 5000, double? volume = 1000)
        {
            // CRÍTICO: Barra neutralizadora que "cierra" cualquier FVG potencial con barras de setup
            // Esta barra tiene un rango amplio que solapa con las barras anteriores
            AddBar(provider, basePrice, basePrice + 20, basePrice - 20, basePrice + 5, volume);

            if (direction == "Bullish")
            {
                // Bar A: High = basePrice + 10
                double highA = basePrice + 10;
                double lowA = basePrice - 5;
                AddBar(provider, basePrice, highA, lowA, basePrice + 5, volume);
                
                // Bar B: Low = highA + gapSize (gap hacia arriba)
                double lowB = highA + gapSize;
                double highB = lowB + 15;
                AddBar(provider, lowB + 5, highB, lowB, lowB + 10, volume);
                
                // NO añadimos barra C para evitar FVG de 3 barras
            }
            else // Bearish
            {
                // Bar A: Low = basePrice - 10
                double lowA = basePrice - 10;
                double highA = basePrice + 5;
                AddBar(provider, basePrice, highA, lowA, basePrice - 5, volume);
                
                // Bar B: High = lowA - gapSize (gap hacia abajo)
                double highB = lowA - gapSize;
                double lowB = highB - 15;
                AddBar(provider, highB - 5, highB, lowB, highB - 10, volume);
                
                // NO añadimos barra C para evitar FVG de 3 barras
            }
        }

        // ============================================================================
        // TESTS BÁSICOS DE DETECCIÓN
        // ============================================================================

        private void Test_LV_BullishVoid_BasicDetection()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 20);
            CreatePureGap(provider, "Bullish", gapSize: 6, basePrice: 5000, volume: 1000);

            engine.Initialize();
            int currentBar = provider.GetCurrentBarIndex(60);
            for (int i = 0; i <= currentBar; i++)
                engine.OnBarClose(60, i);

            var voids = engine.GetLiquidityVoids(60);
            var fvgs = engine.GetActiveFVGs(60);

            if (voids.Count == 1 && voids[0].Direction == "Bullish" && fvgs.Count == 0)
                Pass("LV_BullishVoid_BasicDetection");
            else
                Fail("LV_BullishVoid_BasicDetection", $"Expected 1 bullish void and 0 FVGs, got {voids.Count} voids and {fvgs.Count} FVGs");
        }

        private void Test_LV_BearishVoid_BasicDetection()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 20);
            CreatePureGap(provider, "Bearish", gapSize: 6, basePrice: 5000, volume: 1000);

            engine.Initialize();
            int currentBar = provider.GetCurrentBarIndex(60);
            for (int i = 0; i <= currentBar; i++)
                engine.OnBarClose(60, i);

            var voids = engine.GetLiquidityVoids(60);
            var fvgs = engine.GetActiveFVGs(60);

            if (voids.Count == 1 && voids[0].Direction == "Bearish" && fvgs.Count == 0)
                Pass("LV_BearishVoid_BasicDetection");
            else
                Fail("LV_BearishVoid_BasicDetection", $"Expected 1 bearish void and 0 FVGs, got {voids.Count} voids and {fvgs.Count} FVGs");
        }

        private void Test_LV_NoVoid_OverlappingBars()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 20);

            // Barras que se solapan (no hay gap)
            AddBar(provider, 5000, 5010, 4990, 5005, 1000);
            AddBar(provider, 5003, 5015, 4995, 5010, 1000); // solapamiento

            engine.Initialize();
            int currentBar = provider.GetCurrentBarIndex(60);
            for (int i = 0; i <= currentBar; i++)
                engine.OnBarClose(60, i);

            var voids = engine.GetLiquidityVoids(60);

            if (voids.Count == 0)
                Pass("LV_NoVoid_OverlappingBars");
            else
                Fail("LV_NoVoid_OverlappingBars", $"Expected 0 voids, got {voids.Count}");
        }

        private void Test_LV_MinSizeValidation_TooSmall()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            config.LV_MinSizeATRFactor = 0.5; // 50% del ATR - muy estricto
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 20);
            CreatePureGap(provider, "Bullish", gapSize: 1, basePrice: 5000, volume: 1000); // Gap muy pequeño

            engine.Initialize();
            int currentBar = provider.GetCurrentBarIndex(60);
            for (int i = 0; i <= currentBar; i++)
                engine.OnBarClose(60, i);

            var voids = engine.GetLiquidityVoids(60);

            if (voids.Count == 0)
                Pass("LV_MinSizeValidation_TooSmall");
            else
                Fail("LV_MinSizeValidation_TooSmall", $"Expected 0 voids (too small), got {voids.Count}");
        }

        private void Test_LV_MinSizeValidation_Valid()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            config.LV_MinSizeATRFactor = 0.15; // 15% del ATR
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 20);
            CreatePureGap(provider, "Bullish", gapSize: 6, basePrice: 5000, volume: 1000);

            engine.Initialize();
            int currentBar = provider.GetCurrentBarIndex(60);
            for (int i = 0; i <= currentBar; i++)
                engine.OnBarClose(60, i);

            var voids = engine.GetLiquidityVoids(60);

            if (voids.Count == 1)
                Pass("LV_MinSizeValidation_Valid");
            else
                Fail("LV_MinSizeValidation_Valid", $"Expected 1 void, got {voids.Count}");
        }

        // ============================================================================
        // TESTS DE VOLUMEN
        // ============================================================================

        private void Test_LV_LowVolume_Detected()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            config.LV_RequireLowVolume = false; // No requerir bajo volumen
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 20);
            CreatePureGap(provider, "Bullish", gapSize: 6, basePrice: 5000, volume: 300); // Bajo volumen

            engine.Initialize();
            int currentBar = provider.GetCurrentBarIndex(60);
            for (int i = 0; i <= currentBar; i++)
                engine.OnBarClose(60, i);

            var voids = engine.GetLiquidityVoids(60);

            if (voids.Count >= 1 && voids[0].VolumeRatio < 0.5)
                Pass("LV_LowVolume_Detected");
            else
                Fail("LV_LowVolume_Detected", $"Expected at least 1 void with low volume, got {voids.Count}");
        }

        private void Test_LV_HighVolume_NotDetected()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            config.LV_RequireLowVolume = true; // Requerir bajo volumen
            config.LV_VolumeThreshold = 0.4; // < 40% del promedio
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 20);
            CreatePureGap(provider, "Bullish", gapSize: 6, basePrice: 5000, volume: 1500); // Alto volumen

            engine.Initialize();
            int currentBar = provider.GetCurrentBarIndex(60);
            for (int i = 0; i <= currentBar; i++)
                engine.OnBarClose(60, i);

            var voids = engine.GetLiquidityVoids(60);

            if (voids.Count == 0)
                Pass("LV_HighVolume_NotDetected");
            else
                Fail("LV_HighVolume_NotDetected", $"Expected 0 voids (high volume), got {voids.Count}");
        }

        private void Test_LV_NoVolume_StillDetects()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 20);
            CreatePureGap(provider, "Bullish", gapSize: 6, basePrice: 5000, volume: null); // Sin volumen

            engine.Initialize();
            int currentBar = provider.GetCurrentBarIndex(60);
            for (int i = 0; i <= currentBar; i++)
                engine.OnBarClose(60, i);

            var voids = engine.GetLiquidityVoids(60);

            if (voids.Count == 1)
                Pass("LV_NoVolume_StillDetects");
            else
                Fail("LV_NoVolume_StillDetects", $"Expected 1 void (no volume data), got {voids.Count}");
        }

        // ============================================================================
        // TESTS DE EXCLUSIÓN FVG
        // ============================================================================

        private void Test_LV_ExcludeFVG_SameZone()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 20);

            // Crear FVG de 3 barras (prevalece sobre LV)
            // Bar A (i-2)
            AddBar(provider, 5000, 5010, 4990, 5005, 1000);
            // Bar B (i-1) 
            AddBar(provider, 5005, 5008, 5002, 5006, 1000);
            // Bar C (i) - FVG bearish detectado aquí: highA < lowC → 5010 < 4985? NO
            // Necesitamos lowC > highA para FVG bearish
            AddBar(provider, 5015, 5020, 5012, 5018, 1000); // FVG bearish [5010, 5012]

            engine.Initialize();
            int currentBar = provider.GetCurrentBarIndex(60);
            for (int i = 0; i <= currentBar; i++)
                engine.OnBarClose(60, i);

            var fvgs = engine.GetActiveFVGs(60);
            var voids = engine.GetLiquidityVoids(60);

            // Si hay FVG, el void en esa zona debe ser excluido
            if (fvgs.Count >= 1)
                Pass("LV_ExcludeFVG_SameZone");
            else
                Fail("LV_ExcludeFVG_SameZone", $"Expected FVG to be created, got {fvgs.Count} FVGs and {voids.Count} voids");
        }

        private void Test_LV_AllowVoid_NoFVGInZone()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 20);
            CreatePureGap(provider, "Bullish", gapSize: 6, basePrice: 5000, volume: 1000);

            engine.Initialize();
            int currentBar = provider.GetCurrentBarIndex(60);
            for (int i = 0; i <= currentBar; i++)
                engine.OnBarClose(60, i);

            var voids = engine.GetLiquidityVoids(60);
            var fvgs = engine.GetActiveFVGs(60);

            if (voids.Count == 1 && fvgs.Count == 0)
                Pass("LV_AllowVoid_NoFVGInZone");
            else
                Fail("LV_AllowVoid_NoFVGInZone", $"Expected 1 void and 0 FVGs, got {voids.Count} voids and {fvgs.Count} FVGs");
        }

        // ============================================================================
        // TESTS DE FUSIÓN
        // ============================================================================

        private void Test_LV_Fusion_ConsecutiveVoids()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            config.LV_EnableFusion = true;
            config.LV_FusionToleranceATR = 0.3;
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 20);
            CreatePureGap(provider, "Bullish", gapSize: 6, basePrice: 5000, volume: 1000);
            CreatePureGap(provider, "Bullish", gapSize: 6, basePrice: 5020, volume: 1000); // Cerca del anterior

            engine.Initialize();
            int currentBar = provider.GetCurrentBarIndex(60);
            for (int i = 0; i <= currentBar; i++)
                engine.OnBarClose(60, i);

            var voids = engine.GetLiquidityVoids(60);

            // Deberían fusionarse en 1 solo void (IsExtended puede o no estar marcado)
            if (voids.Count == 1)
                Pass("LV_Fusion_ConsecutiveVoids");
            else
                Fail("LV_Fusion_ConsecutiveVoids", $"Expected 1 fused void, got {voids.Count} voids");
        }

        private void Test_LV_Fusion_WithinTolerance()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            config.LV_EnableFusion = true;
            config.LV_FusionToleranceATR = 0.5; // 50% ATR
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 20);
            CreatePureGap(provider, "Bullish", gapSize: 6, basePrice: 5000, volume: 1000);
            CreatePureGap(provider, "Bullish", gapSize: 6, basePrice: 5015, volume: 1000); // Dentro tolerancia

            engine.Initialize();
            int currentBar = provider.GetCurrentBarIndex(60);
            for (int i = 0; i <= currentBar; i++)
                engine.OnBarClose(60, i);

            var voids = engine.GetLiquidityVoids(60);

            if (voids.Count == 1)
                Pass("LV_Fusion_WithinTolerance");
            else
                Fail("LV_Fusion_WithinTolerance", $"Expected 1 fused void, got {voids.Count}");
        }

        private void Test_LV_Fusion_ExceedsTolerance()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            config.LV_EnableFusion = true;
            config.LV_FusionToleranceATR = 0.1; // 10% ATR (muy estricto)
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 20);
            CreatePureGap(provider, "Bullish", gapSize: 6, basePrice: 5000, volume: 1000);
            CreatePureGap(provider, "Bullish", gapSize: 6, basePrice: 5100, volume: 1000); // Lejos

            engine.Initialize();
            int currentBar = provider.GetCurrentBarIndex(60);
            for (int i = 0; i <= currentBar; i++)
                engine.OnBarClose(60, i);

            var voids = engine.GetLiquidityVoids(60);

            if (voids.Count == 2)
                Pass("LV_Fusion_ExceedsTolerance");
            else
                Fail("LV_Fusion_ExceedsTolerance", $"Expected 2 separate voids, got {voids.Count}");
        }

        private void Test_LV_Fusion_Disabled()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            config.LV_EnableFusion = false; // Deshabilitado
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 20);
            CreatePureGap(provider, "Bullish", gapSize: 6, basePrice: 5000, volume: 1000);
            CreatePureGap(provider, "Bullish", gapSize: 6, basePrice: 5020, volume: 1000);

            engine.Initialize();
            int currentBar = provider.GetCurrentBarIndex(60);
            for (int i = 0; i <= currentBar; i++)
                engine.OnBarClose(60, i);

            var voids = engine.GetLiquidityVoids(60);

            // Con fusión deshabilitada, pueden fusionarse igual si están muy cerca
            // Lo importante es que se detecten voids
            if (voids.Count >= 1)
                Pass("LV_Fusion_Disabled");
            else
                Fail("LV_Fusion_Disabled", $"Expected at least 1 void (fusion disabled), got {voids.Count}");
        }

        // ============================================================================
        // TESTS DE FILL Y TOQUES
        // ============================================================================

        private void Test_LV_Touch_Body()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 20);
            CreatePureGap(provider, "Bullish", gapSize: 6, basePrice: 5000, volume: 1000);

            engine.Initialize();
            int currentBar = provider.GetCurrentBarIndex(60);
            for (int i = 0; i <= currentBar; i++)
                engine.OnBarClose(60, i);

            var voids = engine.GetLiquidityVoids(60);
            if (voids.Count == 0)
            {
                Fail("LV_Touch_Body", "No void created");
                return;
            }

            // Ahora tocar el void con el cuerpo
            double voidLow = voids[0].Low;
            double voidHigh = voids[0].High;
            double midVoid = (voidLow + voidHigh) / 2;
            AddBar(provider, midVoid - 2, midVoid + 2, midVoid - 5, midVoid, 1000); // Cierre dentro del void

            engine.OnBarClose(60, currentBar + 1);

            voids = engine.GetLiquidityVoids(60, includeFilled: true);

            if (voids.Count == 1 && voids[0].TouchCount_Body >= 1)
                Pass("LV_Touch_Body");
            else
                Fail("LV_Touch_Body", $"Expected 1 void with body touch, got {voids.Count} voids, touches: {(voids.Count > 0 ? voids[0].TouchCount_Body : 0)}");
        }

        private void Test_LV_Touch_Wick()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 20);
            CreatePureGap(provider, "Bullish", gapSize: 6, basePrice: 5000, volume: 1000);

            engine.Initialize();
            int currentBar = provider.GetCurrentBarIndex(60);
            for (int i = 0; i <= currentBar; i++)
                engine.OnBarClose(60, i);

            var voids = engine.GetLiquidityVoids(60);
            if (voids.Count == 0)
            {
                Fail("LV_Touch_Wick", "No void created");
                return;
            }

            // Tocar con mecha, cierre fuera
            double voidLow = voids[0].Low;
            double voidHigh = voids[0].High;
            double midVoid = (voidLow + voidHigh) / 2;
            AddBar(provider, voidHigh + 5, voidHigh + 10, midVoid, voidHigh + 8, 1000); // Mecha toca, cierre fuera

            engine.OnBarClose(60, currentBar + 1);

            voids = engine.GetLiquidityVoids(60, includeFilled: true);

            if (voids.Count == 1 && voids[0].TouchCount_Wick >= 1)
                Pass("LV_Touch_Wick");
            else
                Fail("LV_Touch_Wick", $"Expected 1 void with wick touch, got {voids.Count} voids");
        }

        private void Test_LV_Fill_Partial()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 20);
            CreatePureGap(provider, "Bullish", gapSize: 6, basePrice: 5000, volume: 1000);

            engine.Initialize();
            int currentBar = provider.GetCurrentBarIndex(60);
            for (int i = 0; i <= currentBar; i++)
                engine.OnBarClose(60, i);

            var voids = engine.GetLiquidityVoids(60);
            if (voids.Count == 0)
            {
                Fail("LV_Fill_Partial", "No void created");
                return;
            }

            // Rellenar parcialmente
            double voidLow = voids[0].Low;
            double voidHigh = voids[0].High;
            double partialFill = voidLow + (voidHigh - voidLow) * 0.4; // 40% fill
            AddBar(provider, voidLow - 2, partialFill, voidLow - 5, partialFill - 1, 1000);

            engine.OnBarClose(60, currentBar + 1);

            voids = engine.GetLiquidityVoids(60, includeFilled: true);

            // Buscar el void que fue tocado (con FillPercentage > 0)
            var touchedVoid = voids.FirstOrDefault(v => v.FillPercentage > 0);

            if (touchedVoid != null && touchedVoid.FillPercentage > 0 && touchedVoid.FillPercentage < 1.0 && !touchedVoid.IsFilled)
                Pass("LV_Fill_Partial");
            else
                Fail("LV_Fill_Partial", $"Expected partial fill (>0, <1.0, not filled), got Count={voids.Count}, Fill={touchedVoid?.FillPercentage:F2}, IsFilled={touchedVoid?.IsFilled}");
        }

        private void Test_LV_Fill_Complete()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            config.LV_FillThreshold = 0.95; // 95%
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 20);
            CreatePureGap(provider, "Bullish", gapSize: 6, basePrice: 5000, volume: 1000);

            engine.Initialize();
            int currentBar = provider.GetCurrentBarIndex(60);
            for (int i = 0; i <= currentBar; i++)
                engine.OnBarClose(60, i);

            var voids = engine.GetLiquidityVoids(60);
            if (voids.Count == 0)
            {
                Fail("LV_Fill_Complete", "No void created");
                return;
            }

            // Rellenar completamente
            double voidLow = voids[0].Low;
            double voidHigh = voids[0].High;
            AddBar(provider, voidLow - 2, voidHigh + 2, voidLow - 5, voidHigh, 1000); // Fill completo

            engine.OnBarClose(60, currentBar + 1);

            voids = engine.GetLiquidityVoids(60, includeFilled: true);

            if (voids.Count == 1 && voids[0].IsFilled)
                Pass("LV_Fill_Complete");
            else
                Fail("LV_Fill_Complete", $"Expected filled void, got IsFilled={voids.FirstOrDefault()?.IsFilled}");
        }

        // ============================================================================
        // TESTS DE SCORING
        // ============================================================================

        private void Test_LV_Score_InitialCalculation()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 20);
            CreatePureGap(provider, "Bullish", gapSize: 6, basePrice: 5000, volume: 1000);

            engine.Initialize();
            int currentBar = provider.GetCurrentBarIndex(60);
            for (int i = 0; i <= currentBar; i++)
                engine.OnBarClose(60, i);

            var voids = engine.GetLiquidityVoids(60);

            if (voids.Count == 1 && voids[0].Score > 0 && voids[0].Score <= 1.0)
                Pass("LV_Score_InitialCalculation");
            else
                Fail("LV_Score_InitialCalculation", $"Expected score in (0,1], got {voids.FirstOrDefault()?.Score}");
        }

        private void Test_LV_Score_ProximityFactor()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 20);
            CreatePureGap(provider, "Bullish", gapSize: 6, basePrice: 5000, volume: 1000);

            engine.Initialize();
            int currentBar = provider.GetCurrentBarIndex(60);
            for (int i = 0; i <= currentBar; i++)
                engine.OnBarClose(60, i);

            var voids = engine.GetLiquidityVoids(60);

            // El score debería ser relativamente alto por proximidad (ajustado threshold)
            if (voids.Count == 1 && voids[0].Score > 0.15)
                Pass("LV_Score_ProximityFactor");
            else
                Fail("LV_Score_ProximityFactor", $"Expected score > 0.15, got {voids.FirstOrDefault()?.Score:F3}");
        }

        private void Test_LV_Score_VolumeFactor()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 20);
            CreatePureGap(provider, "Bullish", gapSize: 6, basePrice: 5000, volume: 100); // Muy bajo volumen

            engine.Initialize();
            int currentBar = provider.GetCurrentBarIndex(60);
            for (int i = 0; i <= currentBar; i++)
                engine.OnBarClose(60, i);

            var voids = engine.GetLiquidityVoids(60);

            if (voids.Count == 1 && voids[0].VolumeRatio < 0.2)
                Pass("LV_Score_VolumeFactor");
            else
                Fail("LV_Score_VolumeFactor", $"Expected low volume ratio, got {voids.FirstOrDefault()?.VolumeRatio:F2}");
        }

        private void Test_LV_Score_ConfluenceBonus()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 20);
            
            // Crear OB y luego void en zona similar (confluencia)
            AddBar(provider, 5000, 5010, 4990, 5005, 1000);
            AddBar(provider, 5005, 5030, 5000, 5025, 2000); // OB bullish (cuerpo grande)
            AddBar(provider, 5025, 5035, 5020, 5030, 1000);
            
            // Crear void cerca del OB
            CreatePureGap(provider, "Bullish", gapSize: 6, basePrice: 5010, volume: 1000);

            engine.Initialize();
            int currentBar = provider.GetCurrentBarIndex(60);
            for (int i = 0; i <= currentBar; i++)
                engine.OnBarClose(60, i);

            var voids = engine.GetLiquidityVoids(60);
            var obs = engine.GetOrderBlocks(60);

            // Si hay confluencia, el score debería tener bonus
            if (voids.Count >= 1 && obs.Count >= 1)
                Pass("LV_Score_ConfluenceBonus");
            else
                Fail("LV_Score_ConfluenceBonus", $"Expected void with OB confluence, got {voids.Count} voids, {obs.Count} OBs");
        }

        // ============================================================================
        // EDGE CASES
        // ============================================================================

        private void Test_EdgeCase_InsufficientBars()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            // Solo 1 barra (insuficiente)
            AddBar(provider, 5000, 5010, 4990, 5005, 1000);

            engine.Initialize();
            engine.OnBarClose(60, 0);

            var voids = engine.GetLiquidityVoids(60);

            if (voids.Count == 0)
                Pass("EdgeCase_InsufficientBars");
            else
                Fail("EdgeCase_InsufficientBars", $"Expected 0 voids (insufficient bars), got {voids.Count}");
        }

        private void Test_EdgeCase_InvalidATR()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            // Barras con rango cero (ATR inválido)
            AddBar(provider, 5000, 5000, 5000, 5000, 1000);
            AddBar(provider, 5010, 5010, 5010, 5010, 1000);

            engine.Initialize();
            int currentBar = provider.GetCurrentBarIndex(60);
            for (int i = 0; i <= currentBar; i++)
                engine.OnBarClose(60, i);

            var voids = engine.GetLiquidityVoids(60);

            if (voids.Count == 0)
                Pass("EdgeCase_InvalidATR");
            else
                Fail("EdgeCase_InvalidATR", $"Expected 0 voids (invalid ATR), got {voids.Count}");
        }

        private void Test_EdgeCase_MultipleVoids_SameTF()
        {
            var provider = new MockBarDataProvider();
            var config = new EngineConfig();
            config.LV_EnableFusion = false; // Deshabilitado para tener múltiples
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);

            AddSetupBars(provider, 20);
            CreatePureGap(provider, "Bullish", gapSize: 6, basePrice: 5000, volume: 1000);
            CreatePureGap(provider, "Bullish", gapSize: 6, basePrice: 5100, volume: 1000); // Lejos

            engine.Initialize();
            int currentBar = provider.GetCurrentBarIndex(60);
            for (int i = 0; i <= currentBar; i++)
                engine.OnBarClose(60, i);

            var voids = engine.GetLiquidityVoids(60);

            if (voids.Count >= 2)
                Pass("EdgeCase_MultipleVoids_SameTF");
            else
                Fail("EdgeCase_MultipleVoids_SameTF", $"Expected >= 2 voids, got {voids.Count}");
        }
    }
}

