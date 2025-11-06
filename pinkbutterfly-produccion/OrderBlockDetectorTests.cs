// ============================================================================
// OrderBlockDetectorTests.cs
// PinkButterfly CoreBrain - Tests exhaustivos para OrderBlockDetector
// 
// Suite completa de tests que valida:
// - Detección básica de OBs (Bullish/Bearish)
// - Validación de tamaño mínimo (ATR)
// - Detección por volumen spike
// - Tracking de toques (body/wick)
// - Detección de mitigación
// - Detección de Breaker Blocks
// - Scoring inicial y decay
// - Edge cases y escenarios complejos
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using NinjaTrader.NinjaScript.Indicators.PinkButterfly;

namespace NinjaTrader.NinjaScript.Indicators
{
    /// <summary>
    /// Tests exhaustivos para OrderBlockDetector
    /// 24 tests que cubren todos los aspectos del detector
    /// </summary>
    public class OrderBlockDetectorTests
    {
        private int _passCount = 0;
        private int _failCount = 0;
        private Action<string> _print;

        public OrderBlockDetectorTests(Action<string> printAction)
        {
            _print = printAction;
        }

        public void RunAll()
        {
            _print("==============================================");
            _print("ORDER BLOCK DETECTOR TESTS");
            _print("==============================================");

            _passCount = 0;
            _failCount = 0;

            // ================================================================
            // DETECCIÓN BÁSICA
            // ================================================================
            Test_OrderBlock_BullishDetection_Exists();
            Test_OrderBlock_BullishDetection_Direction();
            Test_OrderBlock_BearishDetection_Exists();
            Test_OrderBlock_BearishDetection_Direction();

            // ================================================================
            // VALIDACIÓN DE TAMAÑO
            // ================================================================
            Test_OrderBlock_MinBodySize_Valid();
            Test_OrderBlock_MinBodySize_TooSmall();
            Test_OrderBlock_BodyRange_Calculation();

            // ================================================================
            // VOLUMEN
            // ================================================================
            Test_OrderBlock_VolumeSpike_Detected();
            Test_OrderBlock_VolumeSpike_NotDetected();
            Test_OrderBlock_NoVolume_StillDetects();

            // ================================================================
            // TOQUES
            // ================================================================
            Test_OrderBlock_BodyTouch_Count();
            Test_OrderBlock_WickTouch_Count();
            Test_OrderBlock_NoTouch_Count();

            // ================================================================
            // MITIGACIÓN
            // ================================================================
            Test_OrderBlock_Bullish_Mitigated();
            Test_OrderBlock_Bearish_Mitigated();
            Test_OrderBlock_NotMitigated();

            // ================================================================
            // BREAKER BLOCKS
            // ================================================================
            Test_OrderBlock_Bullish_Breaker();
            Test_OrderBlock_Bearish_Breaker();
            Test_OrderBlock_NotBreaker();

            // ================================================================
            // SCORING
            // ================================================================
            Test_OrderBlock_InitialScore_Exists();
            Test_OrderBlock_InitialScore_Range();

            // ================================================================
            // EDGE CASES
            // ================================================================
            Test_EdgeCase_InsufficientBars();
            Test_EdgeCase_MultipleOBs_SameTF();
            Test_EdgeCase_OB_And_Breaker_SameTF();

            // ================================================================
            // RESULTADOS
            // ================================================================
            _print("==============================================");
            _print($"RESULTADOS: {_passCount} passed, {_failCount} failed");
            _print("==============================================");
        }

        // ====================================================================
        // HELPERS
        // ====================================================================

        private void Pass(string testName)
        {
            _passCount++;
            _print($"✓ PASS: {testName}");
        }

        private void Fail(string testName, string reason)
        {
            _failCount++;
            _print($"✗ FAIL: {testName} - {reason}");
        }

        private void AddBar(MockBarDataProvider provider, double open, double high, double low, double close, double? volume = null)
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
                double price = basePrice + (i * 0.5); // Incremento muy pequeño
                // Barras con rango pequeño (5 puntos) pero body casi nulo (0.1 puntos)
                // Esto genera ATR válido sin crear OBs (body muy pequeño)
                // Volumen = 1000 para tener un promedio válido
                AddBar(provider, price, price + 2.5, price - 2.5, price + 0.05, 1000.0);
            }
        }

        // ====================================================================
        // TESTS - DETECCIÓN BÁSICA
        // ====================================================================

        private void Test_OrderBlock_BullishDetection_Exists()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            var config = EngineConfig.LoadDefaults();
            config.OBBodyMinATR = 0.6;
            config.EnableDebug = true; // ACTIVAR DEBUG
            config.EnableAutoPurge = false; // FASE 2.5: Desactivar purga en tests
            var logger = new ConsoleLogger { MinLevel = LogLevel.Debug }; // CAMBIAR A DEBUG
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            AddSetupBars(provider, 25, 5000);

            // Crear OB Bullish: cuerpo grande (50 puntos)
            // ATR ~5, entonces 0.6 * 5 = 3 puntos mínimo
            AddBar(provider, 5040, 5095, 5040, 5090, 2000); // Cuerpo = 50 puntos

            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var obs = engine.GetOrderBlocks(60);
            
            if (obs.Count > 0)
                Pass("OrderBlock_BullishDetection_Exists");
            else
                Fail("OrderBlock_BullishDetection_Exists", $"Expected OB, got {obs.Count}");
        }

        private void Test_OrderBlock_BullishDetection_Direction()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            var config = EngineConfig.LoadDefaults();
            config.OBBodyMinATR = 0.6;
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            AddSetupBars(provider, 25, 5000);
            AddBar(provider, 5040, 5095, 5040, 5090, 2000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var obs = engine.GetOrderBlocks(60);
            
            if (obs.Count > 0 && obs[0].Direction == "Bullish")
                Pass("OrderBlock_BullishDetection_Direction");
            else
                Fail("OrderBlock_BullishDetection_Direction", 
                     $"Expected Bullish, got {(obs.Count > 0 ? obs[0].Direction : "none")}");
        }

        private void Test_OrderBlock_BearishDetection_Exists()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            var config = EngineConfig.LoadDefaults();
            config.OBBodyMinATR = 0.6;
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            AddSetupBars(provider, 25, 5000);

            // Crear OB Bearish: cuerpo grande (50 puntos)
            AddBar(provider, 5090, 5095, 5040, 5040, 2000); // Cuerpo = 50 puntos

            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var obs = engine.GetOrderBlocks(60);
            
            if (obs.Count > 0)
                Pass("OrderBlock_BearishDetection_Exists");
            else
                Fail("OrderBlock_BearishDetection_Exists", $"Expected OB, got {obs.Count}");
        }

        private void Test_OrderBlock_BearishDetection_Direction()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            var config = EngineConfig.LoadDefaults();
            config.OBBodyMinATR = 0.6;
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            AddSetupBars(provider, 25, 5000);
            AddBar(provider, 5090, 5095, 5040, 5040, 2000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var obs = engine.GetOrderBlocks(60);
            
            if (obs.Count > 0 && obs[0].Direction == "Bearish")
                Pass("OrderBlock_BearishDetection_Direction");
            else
                Fail("OrderBlock_BearishDetection_Direction", 
                     $"Expected Bearish, got {(obs.Count > 0 ? obs[0].Direction : "none")}");
        }

        // ====================================================================
        // TESTS - VALIDACIÓN DE TAMAÑO
        // ====================================================================

        private void Test_OrderBlock_MinBodySize_Valid()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            var config = EngineConfig.LoadDefaults();
            config.OBBodyMinATR = 0.6;
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            AddSetupBars(provider, 25, 5000);

            // ATR ~5, minBody = 0.6 * 5 = 3
            // Crear vela con cuerpo = 50 (muy por encima del mínimo)
            AddBar(provider, 5040, 5095, 5040, 5090, 2000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var obs = engine.GetOrderBlocks(60);
            
            if (obs.Count > 0)
                Pass("OrderBlock_MinBodySize_Valid");
            else
                Fail("OrderBlock_MinBodySize_Valid", "OB should be detected with large body");
        }

        private void Test_OrderBlock_MinBodySize_TooSmall()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            var config = EngineConfig.LoadDefaults();
            config.OBBodyMinATR = 0.6;
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            AddSetupBars(provider, 25, 5000);

            // ATR ~5, minBody = 0.6 * 5 = 3
            // Crear vela con cuerpo = 1 (por debajo del mínimo)
            AddBar(provider, 5040, 5045, 5035, 5041, 1000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var obs = engine.GetOrderBlocks(60);
            
            if (obs.Count == 0)
                Pass("OrderBlock_MinBodySize_TooSmall");
            else
                Fail("OrderBlock_MinBodySize_TooSmall", $"OB should not be detected, got {obs.Count}");
        }

        private void Test_OrderBlock_BodyRange_Calculation()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            var config = EngineConfig.LoadDefaults();
            config.OBBodyMinATR = 0.6;
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            AddSetupBars(provider, 25, 5000);

            // Bullish: Open=5040, Close=5090 → Low=5040, High=5090
            AddBar(provider, 5040, 5095, 5035, 5090, 2000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var obs = engine.GetOrderBlocks(60);
            
            if (obs.Count > 0 && obs[0].Low == 5040 && obs[0].High == 5090)
                Pass("OrderBlock_BodyRange_Calculation");
            else
                Fail("OrderBlock_BodyRange_Calculation", 
                     $"Expected [5040-5090], got [{obs[0].Low}-{obs[0].High}]");
        }

        // ====================================================================
        // TESTS - VOLUMEN
        // ====================================================================

        private void Test_OrderBlock_VolumeSpike_Detected()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            var config = EngineConfig.LoadDefaults();
            config.OBBodyMinATR = 0.6;
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            // Setup con volumen promedio = 1000
            AddSetupBars(provider, 25, 5000);

            // OB con volumen spike (3000 > 1.5 * 1000)
            AddBar(provider, 5040, 5095, 5040, 5090, 3000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var obs = engine.GetOrderBlocks(60);
            
            if (obs.Count > 0 && obs[0].Metadata.Tags.ContainsKey("VolumeConfirmed"))
                Pass("OrderBlock_VolumeSpike_Detected");
            else
                Fail("OrderBlock_VolumeSpike_Detected", "Volume spike should be detected");
        }

        private void Test_OrderBlock_VolumeSpike_NotDetected()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            var config = EngineConfig.LoadDefaults();
            config.OBBodyMinATR = 0.6;
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            // Setup con volumen promedio = 1000
            AddSetupBars(provider, 25, 5000);

            // OB sin volumen spike (1200 < 1.5 * 1000)
            AddBar(provider, 5040, 5095, 5040, 5090, 1200);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var obs = engine.GetOrderBlocks(60);
            
            if (obs.Count > 0 && !obs[0].Metadata.Tags.ContainsKey("VolumeConfirmed"))
                Pass("OrderBlock_VolumeSpike_NotDetected");
            else
                Fail("OrderBlock_VolumeSpike_NotDetected", "Volume spike should not be detected");
        }

        private void Test_OrderBlock_NoVolume_StillDetects()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            var config = EngineConfig.LoadDefaults();
            config.OBBodyMinATR = 0.6;
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            AddSetupBars(provider, 25, 5000);

            // OB sin volumen (null)
            AddBar(provider, 5040, 5095, 5040, 5090, null);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var obs = engine.GetOrderBlocks(60);
            
            if (obs.Count > 0)
                Pass("OrderBlock_NoVolume_StillDetects");
            else
                Fail("OrderBlock_NoVolume_StillDetects", "OB should be detected even without volume");
        }

        // ====================================================================
        // TESTS - TOQUES
        // ====================================================================

        private void Test_OrderBlock_BodyTouch_Count()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            var config = EngineConfig.LoadDefaults();
            config.OBBodyMinATR = 0.6;
            var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            AddSetupBars(provider, 25, 5000);

            // Crear OB Bullish [5040-5090]
            AddBar(provider, 5040, 5095, 5040, 5090, 2000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Barra que toca con el cuerpo (close dentro del OB)
            AddBar(provider, 5100, 5105, 5055, 5060, 1000); // Close=5060 dentro de [5040-5090]
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var obs = engine.GetOrderBlocks(60);
            
            // Buscar el OB correcto (el que creamos en la barra 25: [5040-5090])
            var targetOB = obs.FirstOrDefault(ob => ob.Low == 5040 && ob.High == 5090);
            
            if (targetOB != null && targetOB.TouchCount_Body == 1)
                Pass("OrderBlock_BodyTouch_Count");
            else
                Fail("OrderBlock_BodyTouch_Count", 
                     $"Expected TouchCount_Body=1, got {(targetOB != null ? targetOB.TouchCount_Body : -1)}");
        }

        private void Test_OrderBlock_WickTouch_Count()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            var config = EngineConfig.LoadDefaults();
            config.OBBodyMinATR = 0.6;
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            AddSetupBars(provider, 25, 5000);

            // Crear OB Bullish [5040-5090]
            AddBar(provider, 5040, 5095, 5040, 5090, 2000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Barra que toca solo con la mecha (close fuera, low dentro)
            AddBar(provider, 5100, 5105, 5050, 5102, 1000); // Low=5050 dentro, Close=5102 fuera
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var obs = engine.GetOrderBlocks(60);
            
            if (obs.Count > 0 && obs[0].TouchCount_Wick == 1)
                Pass("OrderBlock_WickTouch_Count");
            else
                Fail("OrderBlock_WickTouch_Count", 
                     $"Expected TouchCount_Wick=1, got {(obs.Count > 0 ? obs[0].TouchCount_Wick : 0)}");
        }

        private void Test_OrderBlock_NoTouch_Count()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            var config = EngineConfig.LoadDefaults();
            config.OBBodyMinATR = 0.6;
            config.EnableAutoPurge = false; // CRÍTICO: Evitar que purga elimine OBs antes de validarlos
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            AddSetupBars(provider, 25, 5000);

            // Crear OB Bullish [5040-5090]
            AddBar(provider, 5040, 5095, 5040, 5090, 2000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Barra que no toca el OB
            AddBar(provider, 5100, 5110, 5095, 5105, 1000); // Completamente fuera
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var obs = engine.GetOrderBlocks(60);
            
            if (obs.Count > 0 && obs[0].TouchCount_Body == 0 && obs[0].TouchCount_Wick == 0)
                Pass("OrderBlock_NoTouch_Count");
            else
                Fail("OrderBlock_NoTouch_Count", "No touches should be registered");
        }

        // ====================================================================
        // TESTS - MITIGACIÓN
        // ====================================================================

        private void Test_OrderBlock_Bullish_Mitigated()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            var config = EngineConfig.LoadDefaults();
            config.OBBodyMinATR = 0.6;
            config.EnableAutoPurge = false; // CRÍTICO: Evitar que purga elimine OBs antes de que se mitiguen
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            AddSetupBars(provider, 25, 5000);

            // Crear OB Bullish [5040-5090]
            AddBar(provider, 5040, 5095, 5040, 5090, 2000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Precio sube (sale del OB)
            AddBar(provider, 5100, 5110, 5095, 5105, 1000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Precio retorna y toca el OB desde arriba (mitigación)
            AddBar(provider, 5095, 5100, 5050, 5060, 1000); // Low=5050 toca OB
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Obtener TODOS los OBs (incluyendo mitigados)
            var allStructures = engine.GetAllStructures(60);
            var allOBs = allStructures.OfType<OrderBlockInfo>().ToList();
            
            // Buscar el OB correcto (el que creamos: [5040-5090])
            var targetOB = allOBs.FirstOrDefault(ob => ob.Low == 5040 && ob.High == 5090);
            
            if (targetOB != null && targetOB.IsMitigated)
                Pass("OrderBlock_Bullish_Mitigated");
            else
                Fail("OrderBlock_Bullish_Mitigated", $"OB should be mitigated. Found: IsMitigated={targetOB?.IsMitigated}, HasLeftZone={targetOB?.HasLeftZone}");
        }

        private void Test_OrderBlock_Bearish_Mitigated()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            var config = EngineConfig.LoadDefaults();
            config.OBBodyMinATR = 0.6;
            config.EnableAutoPurge = false; // CRÍTICO: Evitar que purga elimine OBs antes de que se mitiguen
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            AddSetupBars(provider, 25, 5000);

            // Crear OB Bearish [5040-5090]
            AddBar(provider, 5090, 5095, 5040, 5040, 2000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Precio baja (sale del OB)
            AddBar(provider, 5030, 5035, 5020, 5025, 1000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Precio retorna y toca el OB desde abajo (mitigación)
            AddBar(provider, 5030, 5060, 5025, 5055, 1000); // High=5060 toca OB
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Obtener TODOS los OBs (incluyendo mitigados)
            var allStructures = engine.GetAllStructures(60);
            var allOBs = allStructures.OfType<OrderBlockInfo>().ToList();
            
            // Buscar el OB correcto (el que creamos: [5040-5090])
            var targetOB = allOBs.FirstOrDefault(ob => ob.Low == 5040 && ob.High == 5090);
            
            if (targetOB != null && targetOB.IsMitigated)
                Pass("OrderBlock_Bearish_Mitigated");
            else
                Fail("OrderBlock_Bearish_Mitigated", $"OB should be mitigated. Found: IsMitigated={targetOB?.IsMitigated}, HasLeftZone={targetOB?.HasLeftZone}");
        }

        private void Test_OrderBlock_NotMitigated()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            var config = EngineConfig.LoadDefaults();
            config.OBBodyMinATR = 0.6;
            config.EnableAutoPurge = false; // CRÍTICO: Evitar que purga interfiera con la validación
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            AddSetupBars(provider, 25, 5000);

            // Crear OB Bullish [5040-5090]
            AddBar(provider, 5040, 5095, 5040, 5090, 2000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Precio sube y se mantiene arriba (no retorna)
            AddBar(provider, 5100, 5110, 5095, 5105, 1000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));
            AddBar(provider, 5105, 5115, 5100, 5110, 1000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var obs = engine.GetOrderBlocks(60);
            
            if (obs.Count > 0 && !obs[0].IsMitigated)
                Pass("OrderBlock_NotMitigated");
            else
                Fail("OrderBlock_NotMitigated", "OB should not be mitigated");
        }

        // ====================================================================
        // TESTS - BREAKER BLOCKS
        // ====================================================================

        private void Test_OrderBlock_Bullish_Breaker()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            var config = EngineConfig.LoadDefaults();
            config.OBBodyMinATR = 0.6;
            config.EnableAutoPurge = false; // CRÍTICO: Evitar que purga elimine OBs antes de que se conviertan en breaker
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            AddSetupBars(provider, 25, 5000);

            // Crear OB Bullish [5040-5090]
            AddBar(provider, 5040, 5095, 5040, 5090, 2000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Precio rompe el OB hacia abajo (close < ob.Low)
            AddBar(provider, 5050, 5055, 5020, 5025, 1000); // Close=5025 < 5040
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Precio retorna y retestea desde abajo (breaker)
            AddBar(provider, 5025, 5050, 5020, 5045, 1000); // Low < 5040, High >= 5040
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Obtener TODOS los OBs (incluyendo mitigados)
            var allStructures = engine.GetAllStructures(60);
            var allOBs = allStructures.OfType<OrderBlockInfo>().ToList();
            
            // Buscar el OB correcto (el que creamos: [5040-5090])
            var targetOB = allOBs.FirstOrDefault(ob => ob.Low == 5040 && ob.High == 5090);
            
            if (targetOB != null && targetOB.IsBreaker)
                Pass("OrderBlock_Bullish_Breaker");
            else
                Fail("OrderBlock_Bullish_Breaker", $"OB should become breaker. Found: IsBreaker={targetOB?.IsBreaker}, IsMitigated={targetOB?.IsMitigated}");
        }

        private void Test_OrderBlock_Bearish_Breaker()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            var config = EngineConfig.LoadDefaults();
            config.OBBodyMinATR = 0.6;
            config.EnableAutoPurge = false; // CRÍTICO: Evitar que purga elimine OBs antes de que se conviertan en breaker
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            AddSetupBars(provider, 25, 5000);

            // Crear OB Bearish [5040-5090]
            AddBar(provider, 5090, 5095, 5040, 5040, 2000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Precio rompe el OB hacia arriba (close > ob.High)
            AddBar(provider, 5080, 5110, 5075, 5105, 1000); // Close=5105 > 5090
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Precio retorna y retestea desde arriba (breaker)
            AddBar(provider, 5105, 5110, 5080, 5095, 1000); // High > 5090, Low <= 5090
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Obtener TODOS los OBs (incluyendo mitigados)
            var allStructures = engine.GetAllStructures(60);
            var allOBs = allStructures.OfType<OrderBlockInfo>().ToList();
            
            // Buscar el OB correcto (el que creamos: [5040-5090])
            var targetOB = allOBs.FirstOrDefault(ob => ob.Low == 5040 && ob.High == 5090);
            
            if (targetOB != null && targetOB.IsBreaker)
                Pass("OrderBlock_Bearish_Breaker");
            else
                Fail("OrderBlock_Bearish_Breaker", $"OB should become breaker. Found: IsBreaker={targetOB?.IsBreaker}, IsMitigated={targetOB?.IsMitigated}");
        }

        private void Test_OrderBlock_NotBreaker()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            var config = EngineConfig.LoadDefaults();
            config.OBBodyMinATR = 0.6;
            config.EnableAutoPurge = false; // CRÍTICO: Evitar que purga interfiera con la validación
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            AddSetupBars(provider, 25, 5000);

            // Crear OB Bullish [5040-5090]
            AddBar(provider, 5040, 5095, 5040, 5090, 2000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Precio se mantiene arriba (no rompe)
            AddBar(provider, 5100, 5110, 5095, 5105, 1000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var obs = engine.GetOrderBlocks(60);
            
            if (obs.Count > 0 && !obs[0].IsBreaker)
                Pass("OrderBlock_NotBreaker");
            else
                Fail("OrderBlock_NotBreaker", "OB should not be breaker");
        }

        // ====================================================================
        // TESTS - SCORING
        // ====================================================================

        private void Test_OrderBlock_InitialScore_Exists()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            var config = EngineConfig.LoadDefaults();
            config.OBBodyMinATR = 0.6;
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            AddSetupBars(provider, 25, 5000);
            AddBar(provider, 5040, 5095, 5040, 5090, 2000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var obs = engine.GetOrderBlocks(60);
            
            if (obs.Count > 0 && obs[0].Score > 0)
                Pass("OrderBlock_InitialScore_Exists");
            else
                Fail("OrderBlock_InitialScore_Exists", "OB should have initial score > 0");
        }

        private void Test_OrderBlock_InitialScore_Range()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            var config = EngineConfig.LoadDefaults();
            config.OBBodyMinATR = 0.6;
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            AddSetupBars(provider, 25, 5000);
            AddBar(provider, 5040, 5095, 5040, 5090, 2000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var obs = engine.GetOrderBlocks(60);
            
            if (obs.Count > 0 && obs[0].Score >= 0 && obs[0].Score <= 1.0)
                Pass("OrderBlock_InitialScore_Range");
            else
                Fail("OrderBlock_InitialScore_Range", 
                     $"Score should be in [0,1], got {(obs.Count > 0 ? obs[0].Score : -1)}");
        }

        // ====================================================================
        // TESTS - EDGE CASES
        // ====================================================================

        private void Test_EdgeCase_InsufficientBars()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            var config = EngineConfig.LoadDefaults();
            config.OBBodyMinATR = 0.6;
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            // Solo 5 barras (< VOL_AVG_PERIOD=20)
            AddSetupBars(provider, 5, 5000);
            AddBar(provider, 5040, 5095, 5040, 5090, 2000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var obs = engine.GetOrderBlocks(60);
            
            // No debería detectar OB porque no hay suficientes barras
            if (obs.Count == 0)
                Pass("EdgeCase_InsufficientBars");
            else
                Fail("EdgeCase_InsufficientBars", $"Should not detect OB with insufficient bars, got {obs.Count}");
        }

        private void Test_EdgeCase_MultipleOBs_SameTF()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            var config = EngineConfig.LoadDefaults();
            config.OBBodyMinATR = 0.6;
            config.EnableAutoPurge = false; // CRÍTICO: Evitar que purga elimine OBs antes de contar
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            AddSetupBars(provider, 25, 5000);

            // OB 1: Bullish
            AddBar(provider, 5040, 5095, 5040, 5090, 2000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Barras intermedias
            AddBar(provider, 5100, 5105, 5095, 5100, 1000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // OB 2: Bearish
            AddBar(provider, 5150, 5155, 5100, 5100, 2000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var obs = engine.GetOrderBlocks(60);
            
            if (obs.Count == 2)
                Pass("EdgeCase_MultipleOBs_SameTF");
            else
                Fail("EdgeCase_MultipleOBs_SameTF", $"Expected 2 OBs, got {obs.Count}");
        }

        private void Test_EdgeCase_OB_And_Breaker_SameTF()
        {
            var provider = new MockBarDataProvider(tickSize: 0.25);
            var config = EngineConfig.LoadDefaults();
            config.OBBodyMinATR = 0.6;
            config.EnableAutoPurge = false; // CRÍTICO: Evitar que purga elimine OBs antes de validar breaker
            var logger = new ConsoleLogger { MinLevel = LogLevel.Error };
            var engine = new CoreEngine(provider, config, logger);
            engine.Initialize();

            AddSetupBars(provider, 25, 5000);

            // OB 1: Bullish (se convertirá en breaker)
            AddBar(provider, 5040, 5095, 5040, 5090, 2000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Romper OB 1 (body pequeño para no crear OB)
            AddBar(provider, 5027, 5055, 5020, 5025, 1000); // Body=2 (muy pequeño)
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // Retestear OB 1 (breaker, body pequeño para no crear OB)
            AddBar(provider, 5027, 5050, 5020, 5025, 1000); // Body=2 (muy pequeño)
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            // OB 2: Bearish (normal)
            AddBar(provider, 5150, 5155, 5100, 5100, 2000);
            engine.OnBarClose(60, provider.GetCurrentBarIndex(60));

            var obs = engine.GetOrderBlocks(60);
            
            int breakerCount = obs.Count(ob => ob.IsBreaker);
            int normalCount = obs.Count(ob => !ob.IsBreaker);
            
            if (obs.Count == 2 && breakerCount == 1 && normalCount == 1)
                Pass("EdgeCase_OB_And_Breaker_SameTF");
            else
                Fail("EdgeCase_OB_And_Breaker_SameTF", 
                     $"Expected 1 breaker + 1 normal, got {breakerCount} breakers + {normalCount} normal");
        }
    }
}

// ============================================================================
// NOTAS DE TESTING
// ============================================================================
//
// 1. COBERTURA:
//    - 24 tests exhaustivos
//    - Detección básica (Bullish/Bearish)
//    - Validación de tamaño (ATR)
//    - Volumen spike
//    - Toques (body/wick)
//    - Mitigación
//    - Breaker Blocks
//    - Scoring
//    - Edge cases
//
// 2. DATOS REALISTAS:
//    - Precios de futuros (5000-5150)
//    - ATR calculado dinámicamente
//    - Volumen promedio = 1000
//    - Tick size = 0.25
//
// 3. PROFESIONALISMO:
//    - Tests independientes (cada uno crea su propio engine)
//    - Setup bars para ATR válido
//    - Assertions claras con mensajes descriptivos
//    - Cobertura de casos positivos y negativos
//
// 4. BREAKER BLOCKS:
//    - Tests específicos para conversión OB → Breaker
//    - Validación de dirección de ruptura y retesteo
//    - Casos para Bullish y Bearish
//
// 5. MITIGACIÓN:
//    - Tests para retorno de precio a zona OB
//    - Validación de dirección (desde arriba/abajo)
//    - Casos donde NO hay mitigación
//
// ============================================================================

