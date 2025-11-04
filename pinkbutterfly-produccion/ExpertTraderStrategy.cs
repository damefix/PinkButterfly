// ============================================================================
// ExpertTraderStrategy.cs
// PinkButterfly CoreBrain - Strategy Wrapper para Backtesting con Fechas
// 
// Strategy que instancia y ejecuta el indicador ExpertTrader.
// NO replica lógica, solo permite usar Strategy Analyzer con control de fechas.
//
// USO:
// 1. Compilar (F5)
// 2. Tools → Strategy Analyzer
// 3. Seleccionar "ExpertTrader Strategy"
// 4. Configurar fechas From/To
// 5. Run Backtest
// 6. Revisar CSVs en PinkButterfly\logs\
// ============================================================================

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.NinjaScript.Indicators.PinkButterfly;

namespace NinjaTrader.NinjaScript.Strategies.PinkButterfly
{
    /// <summary>
    /// Strategy wrapper para ejecutar ExpertTrader con control de fechas en Strategy Analyzer
    /// NO contiene lógica de trading, solo instancia el indicador real.
    /// </summary>
    public class ExpertTraderStrategy : Strategy
    {
        #region Variables
        
        private ExpertTrader _indicator;
        
        #endregion
        
        #region Properties (Exponer parámetros del indicador)
        
        [NinjaScriptProperty]
        [Display(Name = "Language / Idioma", Description = "Interface language / Idioma de la interfaz", Order = 1, GroupName = "1. General")]
        public LanguageOption Language { get; set; }
        
        [NinjaScriptProperty]
        [Display(Name = "Account Size", Description = "Tamaño de cuenta para calcular position size", Order = 2, GroupName = "2. Risk Management")]
        public double AccountSize { get; set; }
        
        [NinjaScriptProperty]
        [Display(Name = "Contratos por Operación", Description = "Número de contratos a operar (para cálculo de P&L)", Order = 3, GroupName = "2. Risk Management")]
        [Range(1, 100)]
        public int ContractSize { get; set; }
        
        [NinjaScriptProperty]
        [Display(Name = "Enable Fast Load (Solo DFM)", Description = "⚡ MODO RÁPIDO: Carga estructuras desde JSON (2-3 seg). REQUIERE brain_state.json previo.", Order = 4, GroupName = "3. Performance")]
        public bool EnableFastLoad { get; set; }
        
        [NinjaScriptProperty]
        [Display(Name = "Enable Output Logging", Description = "Activar logs en Output window de NinjaTrader", Order = 5, GroupName = "4. Logging")]
        public bool EnableOutputLogging { get; set; }
        
        [NinjaScriptProperty]
        [Display(Name = "Enable File Logging", Description = "Activar logs en archivo de disco", Order = 6, GroupName = "4. Logging")]
        public bool EnableFileLogging { get; set; }
        
        [NinjaScriptProperty]
        [Display(Name = "Enable Trade CSV", Description = "Activar registro de operaciones en archivo CSV (RECOMENDADO)", Order = 7, GroupName = "4. Logging")]
        public bool EnableTradeCSV { get; set; }
        
        #endregion
        
        #region NinjaScript Lifecycle
        
        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                // ============================================================
                // CONFIGURACIÓN DE LA STRATEGY
                // ============================================================
                
                Description = @"PinkButterfly ExpertTrader Strategy - Wrapper para backtesting con control de fechas en Strategy Analyzer. Usa el indicador ExpertTrader sin modificar su lógica.";
                Name = "ExpertTrader Strategy";
                
                // IMPORTANTE: OnBarClose para backtesting consistente
                Calculate = Calculate.OnBarClose;
                
                // No ejecutar órdenes reales (solo análisis)
                EntriesPerDirection = 0;
                EntryHandling = EntryHandling.AllEntries;
                IsExitOnSessionCloseStrategy = false;
                ExitOnSessionCloseSeconds = 30;
                IsFillLimitOnTouch = false;
                MaximumBarsLookBack = MaximumBarsLookBack.TwoHundredFiftySix;
                OrderFillResolution = OrderFillResolution.Standard;
                Slippage = 0;
                StartBehavior = StartBehavior.WaitUntilFlat;
                TimeInForce = TimeInForce.Gtc;
                TraceOrders = false;
                RealtimeErrorHandling = RealtimeErrorHandling.StopCancelClose;
                StopTargetHandling = StopTargetHandling.PerEntryExecution;
                BarsRequiredToTrade = 20;
                IsInstantiatedOnEachOptimizationIteration = true;
                
                // Configurar output a Output Tab 2
                PrintTo = PrintTo.OutputTab2;
                
                // ============================================================
                // DEFAULTS (mismos que ExpertTrader)
                // ============================================================
                
                Language = LanguageOption.Spanish;
                AccountSize = 100000;
                ContractSize = 1;
                EnableFastLoad = false;
                EnableOutputLogging = false;
                EnableFileLogging = true;
                EnableTradeCSV = true;
            }
            else if (State == State.Configure)
            {
                // No necesitamos configurar nada aquí
                // El indicador maneja su propia configuración multi-timeframe
            }
            else if (State == State.DataLoaded)
            {
                // ============================================================
                // INSTANCIAR INDICADOR REAL (100% de la lógica)
                // ============================================================
                
                Print("═══════════════════════════════════════════════════════════");
                Print("  PinkButterfly ExpertTrader Strategy");
                Print("  Wrapper para backtesting con fechas");
                Print("═══════════════════════════════════════════════════════════");
                Print("");
                Print("Configuración:");
                Print($"  - Account Size: ${AccountSize:N0}");
                Print($"  - Contract Size: {ContractSize}");
                Print($"  - Fast Load: {EnableFastLoad}");
                Print($"  - Trade CSV: {EnableTradeCSV}");
                Print("");
                Print("Instanciando indicador ExpertTrader...");
                
                // Crear instancia del indicador REAL con sus parámetros
                // Sintaxis NinjaTrader: los parámetros deben estar en el orden exacto de la declaración
                _indicator = ExpertTrader(
                    this.Language,              // Language
                    this.AccountSize,           // AccountSize
                    false,                      // ShowEntryLines (no dibujar en Strategy Analyzer)
                    false,                      // ShowSLTPLines
                    false,                      // ShowPanel
                    2,                          // EntryLineWidth
                    2,                          // SLTPLineWidth
                    this.EnableFastLoad,        // EnableFastLoad
                    this.ContractSize,          // ContractSize
                    this.EnableOutputLogging,   // EnableOutputLogging
                    this.EnableFileLogging,     // EnableFileLogging
                    this.EnableTradeCSV         // EnableTradeCSV
                );
                
                // Añadir el indicador (esto hace que se ejecute automáticamente)
                AddChartIndicator(_indicator);
                
                Print("Indicador ExpertTrader instanciado correctamente.");
                Print("La strategy NO ejecutará órdenes, solo análisis.");
                Print("Revisa los logs CSV en: Documents\\NinjaTrader 8\\PinkButterfly\\logs\\");
                Print("═══════════════════════════════════════════════════════════");
            }
            else if (State == State.Terminated)
            {
                Print("═══════════════════════════════════════════════════════════");
                Print("  PinkButterfly ExpertTrader Strategy - TERMINADA");
                Print("  Revisa los archivos CSV generados:");
                Print("  - backtest_YYYYMMDD_hhmmss.log");
                Print("  - trades_YYYYMMDD_hhmmss.csv");
                Print("═══════════════════════════════════════════════════════════");
            }
        }
        
        protected override void OnBarUpdate()
        {
            // ============================================================
            // NO HACER NADA AQUÍ
            // ============================================================
            
            // El indicador ExpertTrader ya hace todo:
            // - Procesa las barras
            // - Genera decisiones
            // - Escribe logs
            // - Genera CSV de trades
            
            // Esta strategy es solo un wrapper para usar Strategy Analyzer
        }
        
        #endregion
    }
}

