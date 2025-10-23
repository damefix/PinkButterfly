// ============================================================================
// TestRunnerIndicator.cs
// PinkButterfly CoreBrain - Indicador para ejecutar tests
// 
// Este indicador ejecuta los tests del IntervalTree y del CoreEngine
// cuando se carga en un gráfico.
// ============================================================================

using System;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.NinjaScript;
using NinjaTrader.NinjaScript.Indicators;
using PinkButterfly.Tests;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Indicador de prueba que ejecuta tests del CoreBrain
    /// </summary>
    public class CoreBrainTestRunner : Indicator
    {
        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = @"Ejecuta tests del CoreBrain (IntervalTree, etc.)";
                Name = "CoreBrainTestRunner";
                Calculate = Calculate.OnBarClose;
                IsOverlay = true;
                DisplayInDataBox = false;
                
                // Configurar output a Output Tab 2
                PrintTo = PrintTo.OutputTab2;
            }
            else if (State == State.DataLoaded)
            {
                Print("==============================================");
                Print("COREBRAIN TEST RUNNER");
                Print("==============================================");
                Print("");
                
                RunTests();
            }
        }

        protected override void OnBarUpdate()
        {
            // No hacemos nada en OnBarUpdate
        }

        private void RunTests()
        {
            try
            {
                // ============================================================
                // INTERVAL TREE TESTS
                // ============================================================
                Print(">>> Ejecutando IntervalTree Tests...");
                Print("");
                
                var intervalTreeTester = new IntervalTreeTests(Print);
                intervalTreeTester.RunAllTests();
                
                Print("");
                Print("");

                // ============================================================
                // FVG DETECTOR & SCORING TESTS (BÁSICOS)
                // ============================================================
                Print(">>> Ejecutando FVGDetector & Scoring Tests (Básicos)...");
                Print("");
                
                var fvgTester = new FVGDetectorTests(Print);
                fvgTester.RunAllTests();
                
                Print("");
                Print("");

                // ============================================================
                // FVG DETECTOR ADVANCED TESTS
                // ============================================================
                Print(">>> Ejecutando FVGDetector Advanced Tests...");
                Print("");
                
                var fvgAdvancedTester = new FVGDetectorAdvancedTests(Print);
                fvgAdvancedTester.RunAllTests();
                
                Print("");
                Print("");

                // ============================================================
                // SWING DETECTOR TESTS
                // ============================================================
                Print(">>> Ejecutando SwingDetector Tests...");
                Print("");
                
                var swingTester = new SwingDetectorTests(Print);
                swingTester.RunAllTests();
                
                Print("");
                Print("");

                // ============================================================
                // DOUBLE DETECTOR TESTS
                // ============================================================
                Print(">>> Ejecutando DoubleDetector Tests...");
                Print("");
                
                var doubleTester = new DoubleDetectorTests(Print);
                doubleTester.RunAllTests();
                
                Print("");
                Print(">>> Todos los tests completados!");
                Print("");
                Print("==============================================");
            }
            catch (Exception ex)
            {
                Print($"ERROR ejecutando tests: {ex.Message}");
                Print($"Stack: {ex.StackTrace}");
            }
        }
    }
}

// ============================================================================
// INSTRUCCIONES DE USO
// ============================================================================
// 
// 1. Compila este indicador en NinjaTrader (F5)
// 2. Abre cualquier gráfico
// 3. Añade el indicador "CoreBrainTestRunner"
// 4. Mira la ventana "Output Tab 2" para ver los resultados
// 5. Deberías ver algo como:
//    ✓ PASS: Insert_BasicFunctionality
//    ✓ PASS: QueryOverlap_NoResults
//    ...
//    RESULTADOS: 10 passed, 0 failed
//
// ============================================================================

