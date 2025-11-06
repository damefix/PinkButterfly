// ============================================================================
// IDetector.cs
// PinkButterfly CoreBrain - Interface base para detectores
// 
// Todos los detectores de estructuras implementan esta interface
// Detectores disponibles:
// - FVGDetector (Fair Value Gaps)
// - SwingDetector (Swing Highs/Lows)
// - DoubleDetector (Double Tops/Bottoms)
// - OrderBlockDetector (Order Blocks)
// - BOSDetector (Break of Structure / Change of Character)
// - POIDetector (Points of Interest / Confluencias)
//
// Los detectores son inyectables y configurables
// ============================================================================

using System;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Interface base para todos los detectores de estructuras
    /// Los detectores son componentes modulares que se inicializan,
    /// reaccionan a eventos de barras, y se pueden liberar recursos al finalizar
    /// </summary>
    public interface IDetector
    {
        /// <summary>
        /// Inicializa el detector con el proveedor de datos, configuración y logger
        /// Llamado una vez al inicio por el CoreEngine
        /// </summary>
        /// <param name="provider">Proveedor de datos de barras</param>
        /// <param name="config">Configuración del motor</param>
        /// <param name="logger">Logger para mensajes del detector</param>
        void Initialize(IBarDataProvider provider, EngineConfig config, ILogger logger);

        /// <summary>
        /// Método llamado cuando se cierra una barra en un timeframe específico
        /// El detector debe:
        /// 1. Analizar datos usando el provider
        /// 2. Detectar estructuras nuevas o actualizaciones
        /// 3. Llamar a engine.AddStructure() o engine.UpdateStructure() según corresponda
        /// 
        /// IMPORTANTE: Este método debe ser eficiente (< 1ms idealmente)
        /// No realizar operaciones bloqueantes o I/O
        /// </summary>
        /// <param name="tfMinutes">Timeframe en el que se cerró la barra</param>
        /// <param name="barIndex">Índice de la barra que se cerró</param>
        /// <param name="engine">Referencia al CoreEngine para agregar/actualizar estructuras</param>
        void OnBarClose(int tfMinutes, int barIndex, CoreEngine engine);

        /// <summary>
        /// Libera recursos del detector
        /// Llamado cuando el motor se cierra
        /// </summary>
        void Dispose();
    }
}

