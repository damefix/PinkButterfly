// ============================================================================
// DecisionEngine.cs
// PinkButterfly CoreBrain - Motor de Decisiones (DFM Orchestrator)
// 
// Orquestador principal del Decision Fusion Model.
// Ejecuta el pipeline de componentes en orden secuencial:
//   1. ContextManager: Construye el snapshot inicial del mercado
//   2. StructureFusion: Fusiona estructuras en HeatZones
//   3. ProximityAnalyzer: Calcula distancias y relevancia de zonas
//   4. RiskCalculator: Calcula SL, TP, position size
//   5. DecisionFusionModel: Genera la decisión final (BUY/SELL/WAIT)
//   6. OutputAdapter: Formatea la salida y genera el rationale
//
// Patrón: Chain of Responsibility / Pipeline
// Valida la integridad de la configuración (pesos deben sumar 1.0)
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Motor de Decisiones - Orquestador del Decision Fusion Model
    /// Ejecuta el pipeline de componentes y genera TradeDecisions
    /// </summary>
    public class DecisionEngine
    {
        private readonly EngineConfig _config;
        private readonly ILogger _logger;
        private readonly List<IDecisionComponent> _components;

        /// <summary>
        /// Constructor del DecisionEngine
        /// Valida la configuración y registra los componentes del pipeline
        /// </summary>
        /// <param name="config">Configuración del CoreEngine (incluye parámetros del DFM)</param>
        /// <param name="logger">Logger para debug, info, warning, error</param>
        public DecisionEngine(EngineConfig config, ILogger logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _components = new List<IDecisionComponent>();

            // VALIDACIÓN CRÍTICA: Los pesos de scoring deben sumar exactamente 1.0
            ValidateScoringWeights();

            // Registrar componentes en orden de ejecución
            RegisterComponents();

            _logger.Info("[DecisionEngine] Inicializado correctamente con " + _components.Count + " componentes");
        }

        /// <summary>
        /// Valida que la suma de los pesos de scoring sea exactamente 1.0
        /// Lanza excepción si la validación falla (fail-fast principle)
        /// </summary>
        private void ValidateScoringWeights()
        {
            double sumWeights = _config.Weight_CoreScore
                              + _config.Weight_Proximity
                              + _config.Weight_Confluence
                              + _config.Weight_Type
                              + _config.Weight_Bias
                              + _config.Weight_Momentum
                              + _config.Weight_Volume;

            // Tolerancia de 0.001 para errores de punto flotante
            const double tolerance = 0.001;
            double diff = Math.Abs(sumWeights - 1.0);

            if (diff > tolerance)
            {
                string errorMsg = string.Format(
                    "[DecisionEngine] VALIDACIÓN CRÍTICA FALLIDA: La suma de los pesos de scoring es {0:F4}, debe ser 1.0 (diff: {1:F4})",
                    sumWeights, diff
                );
                _logger.Error(errorMsg);
                throw new InvalidOperationException(errorMsg);
            }

            _logger.Info(string.Format("[DecisionEngine] Validación de pesos OK: suma = {0:F4}", sumWeights));
        }

        /// <summary>
        /// Registra los componentes del pipeline en orden de ejecución
        /// El orden es CRÍTICO: cada componente depende de los anteriores
        /// </summary>
        private void RegisterComponents()
        {
            // ORDEN DE EJECUCIÓN (CRÍTICO - NO MODIFICAR):
            // 1. ContextManager: Construye snapshot inicial (GlobalBias, Summary, ATR)
            _components.Add(new ContextManager());
            
            // 2. StructureFusion: Fusiona estructuras en HeatZones
            _components.Add(new StructureFusion());
            
            // 3. ProximityAnalyzer: Calcula distancias y relevancia de zonas
            _components.Add(new ProximityAnalyzer());
            
            // 4. RiskCalculator: Calcula SL, TP, position size
            _components.Add(new RiskCalculator());
            
            // 5. DecisionFusionModel: Genera la decisión final (BUY/SELL/WAIT)
            _components.Add(new DecisionFusionModel());
            
            // 6. OutputAdapter: Formatea la salida y genera el rationale
            _components.Add(new OutputAdapter());

            // Inicializar todos los componentes registrados
            foreach (var component in _components)
            {
                _logger.Debug("[DecisionEngine] Inicializando componente: " + component.ComponentName);
                component.Initialize(_config, _logger);
            }
        }

        /// <summary>
        /// Genera una decisión de trading basada en el estado actual del mercado
        /// Ejecuta el pipeline completo de componentes
        /// </summary>
        /// <param name="barData">Proveedor de datos de barras (para ATR, precio, volumen)</param>
        /// <param name="coreEngine">Instancia del CoreEngine (para consultar estructuras)</param>
        /// <param name="currentBar">Índice de la barra actual</param>
        /// <param name="accountSize">Tamaño de la cuenta para calcular position size</param>
        /// <returns>TradeDecision con Action, Confidence, Entry, SL, TP, Rationale</returns>
        public TradeDecision GenerateDecision(IBarDataProvider barData, CoreEngine coreEngine, int currentBar, double accountSize)
        {
            if (barData == null)
                throw new ArgumentNullException(nameof(barData));
            if (coreEngine == null)
                throw new ArgumentNullException(nameof(coreEngine));

            _logger.Debug(string.Format("[DecisionEngine] GenerateDecision() llamado en barra {0}", currentBar));

            // 1. Crear snapshot vacío
            var snapshot = new DecisionSnapshot
            {
                Instrument = "UNKNOWN", // TODO: obtener del barData o pasar como parámetro
                GeneratedAt = DateTime.UtcNow
            };

            // 2. Ejecutar pipeline de componentes
            foreach (var component in _components)
            {
                _logger.Debug("[DecisionEngine] Ejecutando componente: " + component.ComponentName);
                
                try
                {
                    component.Process(snapshot, barData, coreEngine, currentBar, accountSize);
                }
                catch (Exception ex)
                {
                    _logger.Error(string.Format(
                        "[DecisionEngine] Error en componente {0}: {1}",
                        component.ComponentName, ex.Message
                    ));
                    _logger.Exception("Error en pipeline", ex);
                    
                    // Retornar decisión WAIT por error
                    return new TradeDecision
                    {
                        Id = Guid.NewGuid().ToString(),
                        Action = "WAIT",
                        Confidence = 0.0,
                        Entry = 0.0,
                        StopLoss = 0.0,
                        TakeProfit = 0.0,
                        PositionSizeContracts = 0.0,
                        Rationale = "Error en pipeline: " + component.ComponentName,
                        GeneratedAt = DateTime.UtcNow
                    };
                }
            }

            // 3. Obtener la decisión final del snapshot (generada por OutputAdapter)
            TradeDecision finalDecision = snapshot.Metadata.ContainsKey("FinalDecision")
                ? snapshot.Metadata["FinalDecision"] as TradeDecision
                : null;

            if (finalDecision == null)
            {
                _logger.Error("[DecisionEngine] OutputAdapter no generó una decisión final");
                return new TradeDecision
                {
                    Id = Guid.NewGuid().ToString(),
                    Action = "WAIT",
                    Confidence = 0.0,
                    Entry = 0.0,
                    StopLoss = 0.0,
                    TakeProfit = 0.0,
                    PositionSizeContracts = 0.0,
                    Rationale = "Error: No se pudo generar decisión",
                    GeneratedAt = DateTime.UtcNow
                };
            }

            _logger.Debug(string.Format("[DecisionEngine] Pipeline completado. Decisión: {0} @ {1:F2}",
                finalDecision.Action, finalDecision.Entry));

            return finalDecision;
        }

        /// <summary>
        /// Retorna el número de componentes registrados en el pipeline
        /// Útil para tests y debugging
        /// </summary>
        public int ComponentCount => _components.Count;

        /// <summary>
        /// Retorna los nombres de los componentes registrados
        /// Útil para tests y debugging
        /// </summary>
        public List<string> GetComponentNames()
        {
            return _components.Select(c => c.ComponentName).ToList();
        }
    }
}

