// ============================================================================
// IDecisionComponent.cs
// PinkButterfly CoreBrain - Interfaz base para componentes del DFM
// 
// Define el contrato para todos los componentes del Decision Fusion Model:
// - ContextManager: Construye el snapshot inicial del mercado
// - StructureFusion: Fusiona estructuras en HeatZones
// - ProximityAnalyzer: Calcula distancias y relevancia de zonas
// - RiskCalculator: Calcula SL, TP, position size
// - DecisionFusionModel: Genera la decisión final (BUY/SELL/WAIT)
// - OutputAdapter: Formatea la salida y genera el rationale
//
// Patrón: Chain of Responsibility / Pipeline
// Cada componente procesa el DecisionSnapshot y añade/modifica información
// ============================================================================

using NinjaTrader.NinjaScript.Indicators.PinkButterfly;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Interfaz base para todos los componentes del Decision Fusion Model
    /// Cada componente es responsable de una fase del pipeline de decisión
    /// </summary>
    public interface IDecisionComponent
    {
        /// <summary>
        /// Nombre del componente (para logging y debugging)
        /// Ejemplo: "ContextManager", "StructureFusion", "ProximityAnalyzer"
        /// </summary>
        string ComponentName { get; }
        
        /// <summary>
        /// Inicializa el componente con la configuración del motor y el logger
        /// Se llama una vez al inicio, antes de cualquier llamada a Process()
        /// </summary>
        /// <param name="config">Configuración del CoreEngine (incluye parámetros del DFM)</param>
        /// <param name="logger">Logger para debug, info, warning, error</param>
        void Initialize(EngineConfig config, ILogger logger);
        
        /// <summary>
        /// Procesa el snapshot y contribuye a la decisión
        /// Cada componente modifica el snapshot o añade información
        /// El snapshot se pasa por referencia y se modifica in-place
        /// </summary>
        /// <param name="snapshot">Snapshot del mercado (modificado in-place)</param>
        /// <param name="barData">Proveedor de datos de barras (para ATR, precio, volumen)</param>
        /// <param name="coreEngine">Instancia del CoreEngine (para consultar estructuras)</param>
        /// <param name="currentBar">Índice de la barra actual</param>
        /// <param name="accountSize">Tamaño de la cuenta para calcular position size</param>
        void Process(DecisionSnapshot snapshot, IBarDataProvider barData, CoreEngine coreEngine, int currentBar, double accountSize);
    }
}

