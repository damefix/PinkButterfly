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
                Print(">>> Ejecutando IntervalTree Tests...");
                Print("");
                
                // Crear tester pasándole el método Print como logger
                var tester = new IntervalTreeTests(Print);
                tester.RunAllTests();
                
                Print("");
                Print(">>> Tests completados!");
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

