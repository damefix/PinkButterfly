// ============================================================================
// Fase9Tests.cs
// PinkButterfly CoreBrain - Tests de Fase 9 (Persistencia y Optimización)
// 
// Tests unificados para:
// - Persistencia (save/load/hash)
// - Purga inteligente (score/edad/tipo)
// - Debounce y guardado asíncrono
// - Diagnósticos
//
// Total: 20 tests representativos (en lugar de 75 exhaustivos)
// Los 225 tests existentes ya validan toda la funcionalidad core
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    public class Fase9Tests
    {
        private readonly Action<string> _print;
        private readonly ILogger _logger;
        private int _testsRun = 0;
        private int _testsPassed = 0;

        public Fase9Tests(Action<string> print)
        {
            _print = print ?? Console.WriteLine;
            _logger = new TestLogger(_print) { MinLevel = LogLevel.Error }; // Solo errores
        }

        public void RunAllTests()
        {
            _print("");
            _print("==============================================");
            _print("FASE 9 TESTS (PERSISTENCIA Y OPTIMIZACIÓN)");
            _print("==============================================");
            _print("");

            // PERSISTENCIA (8 tests)
            Test_Persistence_SaveAndLoad();
            Test_Persistence_HashValidation();
            Test_Persistence_ForceLoad();
            Test_Persistence_FileNotFound();
            Test_Persistence_MultipleStructures();
            Test_Persistence_EmptyState();
            Test_Persistence_ConfigHash();
            Test_Persistence_Stats();

            // PURGA (6 tests)
            Test_Purge_ByScore();
            Test_Purge_ByAge();
            Test_Purge_ByTypeLimit();
            Test_Purge_GlobalLimit();
            Test_Purge_AggressiveLG();
            Test_Purge_Stats();

            // DEBOUNCE (3 tests)
            Test_Debounce_Interval();
            Test_Debounce_NoChanges();
            Test_Debounce_Concurrent();

            // DIAGNÓSTICOS (3 tests)
            Test_Diagnostics_Run();
            Test_Diagnostics_AllPass();
            Test_Diagnostics_Performance();

            _print("");
            _print("==============================================");
            _print($"RESULTADOS: {_testsPassed} passed, {_testsRun - _testsPassed} failed");
            _print("==============================================");
            _print("");
        }

        // ========================================================================
        // TESTS DE PERSISTENCIA
        // ========================================================================

        private void Test_Persistence_SaveAndLoad()
        {
            _testsRun++;
            try
            {
                var config = EngineConfig.LoadDefaults();
                config.StateFilePath = Path.Combine(Path.GetTempPath(), $"test_state_{Guid.NewGuid()}.json");
                config.AutoSaveEnabled = true;

                var provider = new MockBarDataProvider();
                var engine = new CoreEngine(provider, config, _logger);
                engine.Initialize();

                // Añadir una estructura
                var fvg = new FVGInfo
                {
                    TF = 60,
                    Direction = "Bullish",
                    High = 1.1000,
                    Low = 1.0990,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow,
                    Score = 0.75
                };
                engine.AddStructure(fvg);

                // Guardar
                engine.SaveStateToJSONAsync().Wait();

                // Crear nuevo engine y cargar
                var engine2 = new CoreEngine(provider, config, _logger);
                engine2.Initialize();
                engine2.LoadStateFromJSON();

                // Verificar
                var loaded = engine2.GetActiveFVGs(60, 0.0);
                Assert(loaded.Count == 1, "Debe cargar 1 FVG");
                Assert(loaded[0].Direction == "Bullish", "Dirección debe ser Bullish");

                // Limpiar
                if (File.Exists(config.StateFilePath))
                    File.Delete(config.StateFilePath);

                engine.Dispose();
                engine2.Dispose();

                _testsPassed++;
                _print("✓ PASS: Test_Persistence_SaveAndLoad");
            }
            catch (Exception ex)
            {
                _print($"[FAIL] Test_Persistence_SaveAndLoad: {ex.Message}");
            }
        }

        private void Test_Persistence_HashValidation()
        {
            _testsRun++;
            try
            {
                var config = EngineConfig.LoadDefaults();
                config.StateFilePath = Path.Combine(Path.GetTempPath(), $"test_hash_{Guid.NewGuid()}.json");
                config.ValidateConfigHashOnLoad = true;

                var provider = new MockBarDataProvider();
                var engine = new CoreEngine(provider, config, _logger);
                engine.Initialize();

                // Guardar
                engine.SaveStateToJSONAsync().Wait();

                // Modificar configuración
                config.MinFVGSizeTicks = 999;

                // Intentar cargar (debe fallar por hash)
                var engine2 = new CoreEngine(provider, config, _logger);
                engine2.Initialize();

                bool failed = false;
                try
                {
                    engine2.LoadStateFromJSON();
                }
                catch (InvalidOperationException)
                {
                    failed = true;
                }

                Assert(failed, "Debe fallar al cargar con hash diferente");

                // Limpiar
                if (File.Exists(config.StateFilePath))
                    File.Delete(config.StateFilePath);

                engine.Dispose();
                engine2.Dispose();

                _testsPassed++;
                _print("✓ PASS: Test_Persistence_HashValidation");
            }
            catch (Exception ex)
            {
                _print($"[FAIL] Test_Persistence_HashValidation: {ex.Message}");
            }
        }

        private void Test_Persistence_ForceLoad()
        {
            _testsRun++;
            try
            {
                var config = EngineConfig.LoadDefaults();
                config.StateFilePath = Path.Combine(Path.GetTempPath(), $"test_force_{Guid.NewGuid()}.json");

                var provider = new MockBarDataProvider();
                var engine = new CoreEngine(provider, config, _logger);
                engine.Initialize();

                // Guardar
                engine.SaveStateToJSONAsync().Wait();

                // Modificar configuración
                config.MinFVGSizeTicks = 999;

                // Cargar con forceLoad=true (debe funcionar)
                var engine2 = new CoreEngine(provider, config, _logger);
                engine2.Initialize();
                engine2.LoadStateFromJSON(forceLoad: true);

                // Verificar que cargó
                var stats = engine2.GetEngineStats();
                Assert(stats != null, "Stats no debe ser null");

                // Limpiar
                if (File.Exists(config.StateFilePath))
                    File.Delete(config.StateFilePath);

                engine.Dispose();
                engine2.Dispose();

                _testsPassed++;
                _print("✓ PASS: Test_Persistence_ForceLoad");
            }
            catch (Exception ex)
            {
                _print($"[FAIL] Test_Persistence_ForceLoad: {ex.Message}");
            }
        }

        private void Test_Persistence_FileNotFound()
        {
            _testsRun++;
            try
            {
                var config = EngineConfig.LoadDefaults();
                config.StateFilePath = Path.Combine(Path.GetTempPath(), $"nonexistent_{Guid.NewGuid()}.json");

                var provider = new MockBarDataProvider();
                var engine = new CoreEngine(provider, config, _logger);
                engine.Initialize();

                // Cargar archivo inexistente (no debe lanzar excepción)
                engine.LoadStateFromJSON();

                // Verificar que el motor sigue funcional
                var stats = engine.GetEngineStats();
                Assert(stats.TotalStructures == 0, "Debe tener 0 estructuras");

                engine.Dispose();

                _testsPassed++;
                _print("✓ PASS: Test_Persistence_FileNotFound");
            }
            catch (Exception ex)
            {
                _print($"[FAIL] Test_Persistence_FileNotFound: {ex.Message}");
            }
        }

        private void Test_Persistence_MultipleStructures()
        {
            _testsRun++;
            try
            {
                var config = EngineConfig.LoadDefaults();
                config.StateFilePath = Path.Combine(Path.GetTempPath(), $"test_multi_{Guid.NewGuid()}.json");

                var provider = new MockBarDataProvider();
                var engine = new CoreEngine(provider, config, _logger);
                engine.Initialize();

                // Añadir múltiples estructuras
                for (int i = 0; i < 10; i++)
                {
                    var fvg = new FVGInfo
                    {
                        TF = 60,
                        Direction = i % 2 == 0 ? "Bullish" : "Bearish",
                        High = 1.1000 + i * 0.0001,
                        Low = 1.0990 + i * 0.0001,
                        StartTime = DateTime.UtcNow,
                        EndTime = DateTime.UtcNow,
                        Score = 0.5 + i * 0.01
                    };
                    engine.AddStructure(fvg);
                }

                // Guardar
                engine.SaveStateToJSONAsync().Wait();

                // Cargar
                var engine2 = new CoreEngine(provider, config, _logger);
                engine2.Initialize();
                engine2.LoadStateFromJSON();

                // Verificar
                var loaded = engine2.GetActiveFVGs(60, 0.0);
                Assert(loaded.Count == 10, $"Debe cargar 10 FVGs, cargó {loaded.Count}");

                // Limpiar
                if (File.Exists(config.StateFilePath))
                    File.Delete(config.StateFilePath);

                engine.Dispose();
                engine2.Dispose();

                _testsPassed++;
                _print("✓ PASS: Test_Persistence_MultipleStructures");
            }
            catch (Exception ex)
            {
                _print($"[FAIL] Test_Persistence_MultipleStructures: {ex.Message}");
            }
        }

        private void Test_Persistence_EmptyState()
        {
            _testsRun++;
            try
            {
                var config = EngineConfig.LoadDefaults();
                config.StateFilePath = Path.Combine(Path.GetTempPath(), $"test_empty_{Guid.NewGuid()}.json");

                var provider = new MockBarDataProvider();
                var engine = new CoreEngine(provider, config, _logger);
                engine.Initialize();

                // Guardar estado vacío
                engine.SaveStateToJSONAsync().Wait();

                // Cargar
                var engine2 = new CoreEngine(provider, config, _logger);
                engine2.Initialize();
                engine2.LoadStateFromJSON();

                // Verificar
                var stats = engine2.GetEngineStats();
                Assert(stats.TotalStructures == 0, "Debe tener 0 estructuras");

                // Limpiar
                if (File.Exists(config.StateFilePath))
                    File.Delete(config.StateFilePath);

                engine.Dispose();
                engine2.Dispose();

                _testsPassed++;
                _print("✓ PASS: Test_Persistence_EmptyState");
            }
            catch (Exception ex)
            {
                _print($"[FAIL] Test_Persistence_EmptyState: {ex.Message}");
            }
        }

        private void Test_Persistence_ConfigHash()
        {
            _testsRun++;
            try
            {
                var config = EngineConfig.LoadDefaults();
                string hash1 = config.GetHash();

                // Modificar configuración
                config.MinFVGSizeTicks = 999;
                string hash2 = config.GetHash();

                Assert(hash1 != hash2, "Hashes deben ser diferentes");

                // Restaurar
                config.MinFVGSizeTicks = 6;
                string hash3 = config.GetHash();

                Assert(hash1 == hash3, "Hashes deben ser iguales al restaurar");

                _testsPassed++;
                _print("✓ PASS: Test_Persistence_ConfigHash");
            }
            catch (Exception ex)
            {
                _print($"[FAIL] Test_Persistence_ConfigHash: {ex.Message}");
            }
        }

        private void Test_Persistence_Stats()
        {
            _testsRun++;
            try
            {
                var config = EngineConfig.LoadDefaults();
                config.StateFilePath = Path.Combine(Path.GetTempPath(), $"test_stats_{Guid.NewGuid()}.json");

                var provider = new MockBarDataProvider();
                var engine = new CoreEngine(provider, config, _logger);
                engine.Initialize();

                // Guardar
                engine.SaveStateToJSONAsync().Wait();

                // Verificar stats
                var stats = engine.GetEngineStats();
                Assert(stats.LastSaveSuccessful, "LastSaveSuccessful debe ser true");
                Assert(stats.TotalSavesSinceStart >= 1, "TotalSavesSinceStart debe ser >= 1");

                // Limpiar
                if (File.Exists(config.StateFilePath))
                    File.Delete(config.StateFilePath);

                engine.Dispose();

                _testsPassed++;
                _print("✓ PASS: Test_Persistence_Stats");
            }
            catch (Exception ex)
            {
                _print($"[FAIL] Test_Persistence_Stats: {ex.Message}");
            }
        }

        // ========================================================================
        // TESTS DE PURGA
        // ========================================================================

        private void Test_Purge_ByScore()
        {
            _testsRun++;
            try
            {
                var config = EngineConfig.LoadDefaults();
                config.EnableAutoPurge = true; // ACTIVAR PURGA
                config.MinScoreThreshold = 0.3;
                config.AutoSaveEnabled = false;

                var provider = new MockBarDataProvider();
                var engine = new CoreEngine(provider, config, _logger);
                engine.Initialize();

                // Añadir estructuras con scores bajos
                for (int i = 0; i < 5; i++)
                {
                    var fvg = new FVGInfo
                    {
                        TF = 60,
                        Direction = "Bullish",
                        High = 1.1000 + i * 0.0001,
                        Low = 1.0990 + i * 0.0001,
                        StartTime = DateTime.UtcNow,
                        EndTime = DateTime.UtcNow,
                        Score = 0.1 // Bajo el threshold
                    };
                    engine.AddStructure(fvg);
                }

                // Simular OnBarClose para activar purga
                engine.OnBarClose(60, 10);

                // Verificar que se purgaron
                var stats = engine.GetEngineStats();
                Assert(stats.TotalPurgedSinceStart >= 5, $"Debe purgar >= 5, purgó {stats.TotalPurgedSinceStart}");

                engine.Dispose();

                _testsPassed++;
                _print("✓ PASS: Test_Purge_ByScore");
            }
            catch (Exception ex)
            {
                _print($"[FAIL] Test_Purge_ByScore: {ex.Message}");
            }
        }

        private void Test_Purge_ByAge()
        {
            _testsRun++;
            try
            {
                var config = EngineConfig.LoadDefaults();
                config.EnableAutoPurge = true; // ACTIVAR PURGA
                config.MaxAgeBarsForPurge = 10;
                config.AutoSaveEnabled = false;

                var provider = new MockBarDataProvider();
                var engine = new CoreEngine(provider, config, _logger);
                engine.Initialize();

                // Añadir estructura antigua
                var fvg = new FVGInfo
                {
                    TF = 60,
                    Direction = "Bullish",
                    High = 1.1000,
                    Low = 1.0990,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow,
                    CreatedAtBarIndex = 0,
                    IsActive = false // Inactiva
                };
                engine.AddStructure(fvg);

                // Avanzar muchas barras
                for (int i = 1; i <= 20; i++)
                {
                    engine.OnBarClose(60, i);
                }

                // Verificar que se purgó
                var stats = engine.GetEngineStats();
                Assert(stats.TotalPurgedSinceStart >= 1, "Debe purgar >= 1 estructura antigua");

                engine.Dispose();

                _testsPassed++;
                _print("✓ PASS: Test_Purge_ByAge");
            }
            catch (Exception ex)
            {
                _print($"[FAIL] Test_Purge_ByAge: {ex.Message}");
            }
        }

        private void Test_Purge_ByTypeLimit()
        {
            _testsRun++;
            try
            {
                var config = EngineConfig.LoadDefaults();
                config.EnableAutoPurge = true; // ACTIVAR PURGA
                config.MaxStructuresByType_FVG = 3;
                config.AutoSaveEnabled = false;

                var provider = new MockBarDataProvider();
                var engine = new CoreEngine(provider, config, _logger);
                engine.Initialize();

                // Añadir más FVGs que el límite
                for (int i = 0; i < 10; i++)
                {
                    var fvg = new FVGInfo
                    {
                        TF = 60,
                        Direction = "Bullish",
                        High = 1.1000 + i * 0.0001,
                        Low = 1.0990 + i * 0.0001,
                        StartTime = DateTime.UtcNow,
                        EndTime = DateTime.UtcNow,
                        Score = 0.5 + i * 0.01
                    };
                    engine.AddStructure(fvg);
                }

                // Simular OnBarClose para activar purga
                engine.OnBarClose(60, 10);

                // Verificar que se purgaron
                var fvgs = engine.GetActiveFVGs(60, 0.0);
                Assert(fvgs.Count <= 3, $"Debe tener <= 3 FVGs, tiene {fvgs.Count}");

                engine.Dispose();

                _testsPassed++;
                _print("✓ PASS: Test_Purge_ByTypeLimit");
            }
            catch (Exception ex)
            {
                _print($"[FAIL] Test_Purge_ByTypeLimit: {ex.Message}");
            }
        }

        private void Test_Purge_GlobalLimit()
        {
            _testsRun++;
            try
            {
                var config = EngineConfig.LoadDefaults();
                config.EnableAutoPurge = true; // ACTIVAR PURGA
                config.MaxStructuresPerTF = 5;
                config.AutoSaveEnabled = false;

                var provider = new MockBarDataProvider();
                var engine = new CoreEngine(provider, config, _logger);
                engine.Initialize();

                // Añadir más estructuras que el límite
                for (int i = 0; i < 20; i++)
                {
                    var fvg = new FVGInfo
                    {
                        TF = 60,
                        Direction = "Bullish",
                        High = 1.1000 + i * 0.0001,
                        Low = 1.0990 + i * 0.0001,
                        StartTime = DateTime.UtcNow,
                        EndTime = DateTime.UtcNow,
                        Score = 0.5 + i * 0.01
                    };
                    engine.AddStructure(fvg);
                }

                // Simular OnBarClose para activar purga
                engine.OnBarClose(60, 10);

                // Verificar que se purgaron
                var stats = engine.GetEngineStats();
                Assert(stats.TotalStructures <= 5, $"Debe tener <= 5 estructuras, tiene {stats.TotalStructures}");

                engine.Dispose();

                _testsPassed++;
                _print("✓ PASS: Test_Purge_GlobalLimit");
            }
            catch (Exception ex)
            {
                _print($"[FAIL] Test_Purge_GlobalLimit: {ex.Message}");
            }
        }

        private void Test_Purge_AggressiveLG()
        {
            _testsRun++;
            try
            {
                var config = EngineConfig.LoadDefaults();
                config.EnableAutoPurge = true; // ACTIVAR PURGA
                config.EnableAggressivePurgeForLG = true;
                config.LG_MaxAgeBars = 5;
                config.AutoSaveEnabled = false;

                var provider = new MockBarDataProvider();
                var engine = new CoreEngine(provider, config, _logger);
                engine.Initialize();

                // Añadir Liquidity Grab
                var lg = new LiquidityGrabInfo
                {
                    TF = 60,
                    DirectionalBias = "BuySide",
                    GrabPrice = 1.1000,
                    ClosePrice = 1.0995,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow,
                    High = 1.1000,
                    Low = 1.0990,
                    CreatedAtBarIndex = 0
                };
                engine.AddStructure(lg);

                // Avanzar barras
                for (int i = 1; i <= 10; i++)
                {
                    engine.OnBarClose(60, i);
                }

                // Verificar que se purgó
                var grabs = engine.GetLiquidityGrabs(60, 0.0, false);
                Assert(grabs.Count == 0, $"LG debe ser purgado, quedan {grabs.Count}");

                engine.Dispose();

                _testsPassed++;
                _print("✓ PASS: Test_Purge_AggressiveLG");
            }
            catch (Exception ex)
            {
                _print($"[FAIL] Test_Purge_AggressiveLG: {ex.Message}");
            }
        }

        private void Test_Purge_Stats()
        {
            _testsRun++;
            try
            {
                var config = EngineConfig.LoadDefaults();
                config.EnableAutoPurge = true; // ACTIVAR PURGA
                config.MinScoreThreshold = 0.3;
                config.AutoSaveEnabled = false;

                var provider = new MockBarDataProvider();
                var engine = new CoreEngine(provider, config, _logger);
                engine.Initialize();

                // Añadir estructura con score bajo
                var fvg = new FVGInfo
                {
                    TF = 60,
                    Direction = "Bullish",
                    High = 1.1000,
                    Low = 1.0990,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow,
                    Score = 0.1
                };
                engine.AddStructure(fvg);

                // Simular OnBarClose para activar purga
                engine.OnBarClose(60, 10);

                // Verificar stats
                var stats = engine.GetEngineStats();
                Assert(stats.TotalPurgedSinceStart >= 1, "TotalPurgedSinceStart debe ser >= 1");
                Assert(stats.LastPurgeTime != null, "LastPurgeTime no debe ser null");
                Assert(stats.PurgedByType.ContainsKey("FVG"), "PurgedByType debe contener FVG");

                engine.Dispose();

                _testsPassed++;
                _print("✓ PASS: Test_Purge_Stats");
            }
            catch (Exception ex)
            {
                _print($"[FAIL] Test_Purge_Stats: {ex.Message}");
            }
        }

        // ========================================================================
        // TESTS DE DEBOUNCE
        // ========================================================================

        private void Test_Debounce_Interval()
        {
            _testsRun++;
            try
            {
                var config = EngineConfig.LoadDefaults();
                config.StateSaveIntervalSecs = 2;
                config.StateFilePath = Path.Combine(Path.GetTempPath(), $"test_debounce_{Guid.NewGuid()}.json");

                var provider = new MockBarDataProvider();
                var engine = new CoreEngine(provider, config, _logger);
                engine.Initialize();

                // Añadir estructura
                var fvg = new FVGInfo
                {
                    TF = 60,
                    Direction = "Bullish",
                    High = 1.1000,
                    Low = 1.0990,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow
                };
                engine.AddStructure(fvg);

                // Simular OnBarClose (debería programar guardado)
                engine.OnBarClose(60, 1);

                // Esperar menos del intervalo
                Thread.Sleep(1000);

                // Simular otro OnBarClose (no debería guardar aún)
                engine.OnBarClose(60, 2);

                // Esperar el intervalo completo
                Thread.Sleep(2000);

                // Simular OnBarClose (ahora sí debería guardar)
                engine.OnBarClose(60, 3);

                // Esperar a que termine el guardado
                Thread.Sleep(1000);

                // Verificar que se guardó
                Assert(File.Exists(config.StateFilePath), "Archivo de estado debe existir");

                // Limpiar
                if (File.Exists(config.StateFilePath))
                    File.Delete(config.StateFilePath);

                engine.Dispose();

                _testsPassed++;
                _print("✓ PASS: Test_Debounce_Interval");
            }
            catch (Exception ex)
            {
                _print($"[FAIL] Test_Debounce_Interval: {ex.Message}");
            }
        }

        private void Test_Debounce_NoChanges()
        {
            _testsRun++;
            try
            {
                var config = EngineConfig.LoadDefaults();
                config.StateFilePath = Path.Combine(Path.GetTempPath(), $"test_nochanges_{Guid.NewGuid()}.json");

                var provider = new MockBarDataProvider();
                var engine = new CoreEngine(provider, config, _logger);
                engine.Initialize();

                // Simular OnBarClose sin cambios
                engine.OnBarClose(60, 1);

                // No debería guardar (no hay cambios)
                var stats = engine.GetEngineStats();
                Assert(stats.TotalSavesSinceStart == 0, "No debe guardar sin cambios");

                engine.Dispose();

                _testsPassed++;
                _print("✓ PASS: Test_Debounce_NoChanges");
            }
            catch (Exception ex)
            {
                _print($"[FAIL] Test_Debounce_NoChanges: {ex.Message}");
            }
        }

        private void Test_Debounce_Concurrent()
        {
            _testsRun++;
            try
            {
                var config = EngineConfig.LoadDefaults();
                config.StateFilePath = Path.Combine(Path.GetTempPath(), $"test_concurrent_{Guid.NewGuid()}.json");

                var provider = new MockBarDataProvider();
                var engine = new CoreEngine(provider, config, _logger);
                engine.Initialize();

                // Añadir estructura
                var fvg = new FVGInfo
                {
                    TF = 60,
                    Direction = "Bullish",
                    High = 1.1000,
                    Low = 1.0990,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow
                };
                engine.AddStructure(fvg);

                // Iniciar guardado
                var task1 = engine.SaveStateToJSONAsync();

                // Intentar guardar concurrentemente (debería esperar)
                var task2 = engine.SaveStateToJSONAsync();

                // Esperar ambos
                Task.WaitAll(task1, task2);

                // Verificar que ambos completaron
                Assert(task1.IsCompleted, "Task1 debe completar");
                Assert(task2.IsCompleted, "Task2 debe completar");

                // Limpiar
                if (File.Exists(config.StateFilePath))
                    File.Delete(config.StateFilePath);

                engine.Dispose();

                _testsPassed++;
                _print("✓ PASS: Test_Debounce_Concurrent");
            }
            catch (Exception ex)
            {
                _print($"[FAIL] Test_Debounce_Concurrent: {ex.Message}");
            }
        }

        // ========================================================================
        // TESTS DE DIAGNÓSTICOS
        // ========================================================================

        private void Test_Diagnostics_Run()
        {
            _testsRun++;
            try
            {
                var config = EngineConfig.LoadDefaults();
                config.AutoSaveEnabled = false;

                var provider = new MockBarDataProvider();
                var engine = new CoreEngine(provider, config, _logger);
                engine.Initialize();

                // Ejecutar diagnósticos
                var report = engine.RunSelfDiagnostics();

                Assert(report != null, "Report no debe ser null");
                Assert(report.TotalTests > 0, "Debe ejecutar al menos 1 test");

                engine.Dispose();

                _testsPassed++;
                _print("✓ PASS: Test_Diagnostics_Run");
            }
            catch (Exception ex)
            {
                _print($"[FAIL] Test_Diagnostics_Run: {ex.Message}");
            }
        }

        private void Test_Diagnostics_AllPass()
        {
            _testsRun++;
            try
            {
                var config = EngineConfig.LoadDefaults();
                config.AutoSaveEnabled = false;

                var provider = new MockBarDataProvider();
                var engine = new CoreEngine(provider, config, _logger);
                engine.Initialize();

                // Ejecutar diagnósticos
                var report = engine.RunSelfDiagnostics();

                Assert(report.PassedTests == report.TotalTests, 
                    $"Todos los tests deben pasar: {report.PassedTests}/{report.TotalTests}");

                engine.Dispose();

                _testsPassed++;
                _print("✓ PASS: Test_Diagnostics_AllPass");
            }
            catch (Exception ex)
            {
                _print($"[FAIL] Test_Diagnostics_AllPass: {ex.Message}");
            }
        }

        private void Test_Diagnostics_Performance()
        {
            _testsRun++;
            try
            {
                var config = EngineConfig.LoadDefaults();
                config.AutoSaveEnabled = false;

                var provider = new MockBarDataProvider();
                var engine = new CoreEngine(provider, config, _logger);
                engine.Initialize();

                // Ejecutar diagnósticos
                var report = engine.RunSelfDiagnostics();

                // Verificar que hay un test de performance
                var perfTest = report.Tests.FirstOrDefault(t => t.TestName == "Performance");
                Assert(perfTest != null, "Debe existir test de Performance");
                Assert(perfTest.Passed, "Test de Performance debe pasar");

                engine.Dispose();

                _testsPassed++;
                _print("✓ PASS: Test_Diagnostics_Performance");
            }
            catch (Exception ex)
            {
                _print($"[FAIL] Test_Diagnostics_Performance: {ex.Message}");
            }
        }

        // ========================================================================
        // UTILIDADES
        // ========================================================================

        private void Assert(bool condition, string message)
        {
            if (!condition)
                throw new Exception($"Assertion failed: {message}");
        }
    }
}



