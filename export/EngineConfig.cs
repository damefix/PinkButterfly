// ============================================================================
// EngineConfig.cs
// PinkButterfly CoreBrain - Configuración del motor
// 
// Contiene TODOS los parámetros configurables del sistema:
// - Timeframes a utilizar
// - Parámetros de detección (FVG, Swing, Double, OB, BOS, POI)
// - Pesos de scoring (TF, tipos de estructura)
// - Parámetros de scoring (freshness, decay, proximity, etc.)
// - Configuración de sistema (persistencia, límites, debug)
//
// Serializable a JSON con Newtonsoft.Json
// Requiere referencia a Newtonsoft.Json.dll en NinjaTrader
// Incluye GetHash() para validar configuraciones en persistencia
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Configuración completa del CoreEngine
    /// Todos los parámetros son serializables a JSON
    /// Incluye método GetHash() para validación de configuración en persistencia
    /// </summary>
    public class EngineConfig
    {
        // ========================================================================
        // TIMEFRAMES
        // ========================================================================
        
        /// <summary>
        /// Timeframes a utilizar por el motor (en minutos)
        /// Por defecto: [5, 15, 60, 240, 1440] = 5m, 15m, 1H, 4H, Daily
        /// </summary>
        public List<int> TimeframesToUse { get; set; } = new List<int> { 5, 15, 60, 240, 1440 };

        // ========================================================================
        // PARÁMETROS FVG (FAIR VALUE GAP)
        // ========================================================================
        
        /// <summary>Tamaño mínimo de FVG en ticks (absoluto)</summary>
        public double MinFVGSizeTicks { get; set; } = 6;
        
        /// <summary>Tamaño mínimo de FVG como factor del ATR (relativo a volatilidad)</summary>
        /// OPTIMIZACIÓN: Aumentado a 0.20 para reducir generación de estructuras (-67% FVGs)
        public double MinFVGSizeATRfactor { get; set; } = 0.20;
        
        /// <summary>
        /// Si es true, FVGs consecutivos o adyacentes se fusionan en uno solo
        /// Si es false, cada FVG se mantiene independiente
        /// </summary>
        public bool MergeConsecutiveFVGs { get; set; } = true;
        
        /// <summary>
        /// Si es true, detecta FVGs anidados dentro de FVGs más grandes
        /// (Útil para entradas precisas en zonas grandes)
        /// </summary>
        public bool DetectNestedFVGs { get; set; } = true;

        // ========================================================================
        // PARÁMETROS SWING
        // ========================================================================
        
        /// <summary>Tamaño mínimo del swing como factor del ATR</summary>
        /// OPTIMIZACIÓN CRÍTICA: Aumentado a 0.15 para reducir Swings (-200%, de 9000 a ~3000)
        public double MinSwingATRfactor { get; set; } = 0.15;
        
        /// <summary>Número de barras a la izquierda para validar swing</summary>
        public int nLeft { get; set; } = 2;
        
        /// <summary>Número de barras a la derecha para validar swing</summary>
        public int nRight { get; set; } = 2;

        // ========================================================================
        // PARÁMETROS DOUBLE TOP/BOTTOM
        // ========================================================================
        
        /// <summary>Tolerancia en ticks para considerar dos swings al mismo nivel</summary>
        public double priceToleranceTicks_DoubleTop { get; set; } = 8;
        
        /// <summary>Mínimo número de barras entre los dos swings</summary>
        public int MinBarsBetweenDouble { get; set; } = 3;
        
        /// <summary>Máximo número de barras entre los dos swings</summary>
        public int MaxBarsBetweenDouble { get; set; } = 200;
        
        /// <summary>Número de barras para confirmar ruptura de neckline</summary>
        public int ConfirmBars_Double { get; set; } = 3;

        // ========================================================================
        // PARÁMETROS ORDER BLOCK
        // ========================================================================
        
        /// <summary>
        /// Tamaño mínimo del cuerpo de la vela OB como factor del ATR
        /// Ejemplo: 0.8 = el cuerpo debe ser al menos 80% del ATR(14)
        /// OPTIMIZACIÓN: Aumentado a 0.80 para reducir OrderBlocks (-33%)
        /// </summary>
        public double OBBodyMinATR { get; set; } = 0.80;

        // ========================================================================
        // PESOS DE SCORING - TIMEFRAMES
        // ========================================================================
        
        /// <summary>
        /// Pesos de scoring por timeframe
        /// Timeframes más altos (Daily, 4H) tienen más peso que timeframes bajos (15m)
        /// Clave: minutos del TF, Valor: peso (0.0 - 1.0)
        /// </summary>
        public Dictionary<int, double> TFWeights { get; set; } = new Dictionary<int, double>
        {
            { 1440, 1.0 },   // Daily (peso máximo)
            { 240, 0.7 },    // 4 horas
            { 60, 0.45 },    // 1 hora
            { 15, 0.25 }     // 15 minutos
        };

        // ========================================================================
        // PESOS DE SCORING - TIPOS DE ESTRUCTURA
        // ========================================================================
        
        /// <summary>
        /// Pesos de scoring por tipo de estructura
        /// POIs (confluencias) tienen más peso que estructuras individuales
        /// Clave: tipo de estructura, Valor: peso (0.0 - 1.2)
        /// </summary>
        public Dictionary<string, double> TypeWeights { get; set; } = new Dictionary<string, double>
        {
            { "POI", 1.2 },         // Point of Interest (confluencia) - peso más alto
            { "OB", 0.9 },          // Order Block
            { "FVG", 1.0 },         // Fair Value Gap
            { "BOS", 0.85 },        // Break of Structure
            { "SWING", 0.8 },       // Swing High/Low
            { "DOUBLE_TOP", 0.75 }, // Double Top/Bottom
            { "CHoCH", 0.85 }       // Change of Character
        };

        // ========================================================================
        // PARÁMETROS DE SCORING - PROXIMIDAD Y FRESCURA
        // ========================================================================
        
        /// <summary>
        /// Máxima distancia considerada para proximity (como factor del ATR)
        /// Ejemplo: 2.5 = estructuras a más de 2.5 * ATR tienen proximity = 0
        /// </summary>
        public double ProxMaxATRFactor { get; set; } = 2.5;
        
        /// <summary>
        /// Lambda para decay exponencial de frescura
        /// freshness = exp(-ageBars / FreshnessLambda)
        /// Valores más altos = estructuras "frescas" por más tiempo
        /// </summary>
        public double FreshnessLambda { get; set; } = 20;
        
        /// <summary>
        /// Lambda para decay temporal general del score
        /// score *= exp(-deltaBars / DecayLambda)
        /// Valores más altos = decay más lento
        /// </summary>
        public double DecayLambda { get; set; } = 100;

        // ========================================================================
        // PARÁMETROS DE SCORING - TOQUES
        // ========================================================================
        
        /// <summary>
        /// Bonus por cada toque del cuerpo de una vela a la estructura
        /// touchFactor = 1 + TouchBodyBonusPerTouch * min(TouchCount_Body, MaxTouchBodyCap)
        /// </summary>
        public double TouchBodyBonusPerTouch { get; set; } = 0.12;
        
        /// <summary>
        /// Máximo número de toques de cuerpo que se consideran para el bonus
        /// (Evita que estructuras con muchos toques tengan scores excesivos)
        /// </summary>
        public int MaxTouchBodyCap { get; set; } = 5;

        // ========================================================================
        // PARÁMETROS DE SCORING - CONFLUENCIA
        // ========================================================================
        
        /// <summary>
        /// Peso de confluencia para el multiplier
        /// confluence = 1 + ConfluenceWeight * (confluenceCount - 1)
        /// Ejemplo: 0.18 = cada estructura adicional en confluencia suma 18% al score
        /// </summary>
        public double ConfluenceWeight { get; set; } = 0.18;

        // ========================================================================
        // PARÁMETROS DE RELLENO (FILL)
        // ========================================================================
        
        /// <summary>
        /// Threshold para considerar una estructura "rellenada" (0.0 - 1.0)
        /// 0.90 = 90% del rango debe ser penetrado por el precio
        /// </summary>
        public double FillThreshold { get; set; } = 0.90;
        
        /// <summary>
        /// Score residual mínimo para estructuras rellenadas
        /// Estructuras rellenadas mantienen este score mínimo (no llegan a 0)
        /// Útil para mantener contexto histórico de zonas importantes
        /// </summary>
        public double ResidualScore { get; set; } = 0.05;
        
        /// <summary>
        /// Número de barras que el precio debe permanecer en la zona para confirmar fill
        /// 1 = basta un cierre dentro de la zona
        /// </summary>
        public int FillPriceStayBars { get; set; } = 1;

        // ========================================================================
        // PARÁMETROS BOS/CHOCH Y MOMENTUM
        // ========================================================================
        
        /// <summary>
        /// Factor del cuerpo para determinar momentum fuerte en ruptura
        /// bodySize >= BreakMomentumBodyFactor * ATR => "Strong"
        /// </summary>
        public double BreakMomentumBodyFactor { get; set; } = 0.6;
        
        /// <summary>
        /// Multiplier de score cuando hay momentum fuerte alineado con la estructura
        /// Ejemplo: 1.35 = score se multiplica por 1.35 si hay momentum strong
        /// </summary>
        public double BreakMomentumMultiplierStrong { get; set; } = 1.35;
        
        /// <summary>
        /// Multiplier de score cuando hay momentum débil alineado con la estructura
        /// </summary>
        public double BreakMomentumMultiplierWeak { get; set; } = 1.1;
        
        /// <summary>
        /// Número de barras para confirmar la ruptura de un swing
        /// El precio debe cerrar más allá del swing durante N barras consecutivas
        /// </summary>
        public int nConfirmBars_BOS { get; set; } = 1;
        
        /// <summary>
        /// Número máximo de breaks recientes a considerar para calcular CurrentMarketBias
        /// El bias se calcula por votación ponderada de los últimos N breaks
        /// Breaks con momentum "Strong" tienen más peso en la votación
        /// </summary>
        public int MaxRecentBreaksForBias { get; set; } = 10;

        // ========================================================================
        // PARÁMETROS POI (POINTS OF INTEREST)
        // ========================================================================
        
        /// <summary>
        /// Tolerancia de overlap para considerar confluencia (como factor del ATR)
        /// Ejemplo: 0.5 = estructuras a menos de 0.5 * ATR se consideran en confluencia
        /// </summary>
        public double OverlapToleranceATR { get; set; } = 0.5;
        
        /// <summary>
        /// Número mínimo de estructuras para crear un POI
        /// Default: 2 (al menos 2 estructuras deben confluir)
        /// </summary>
        public int MinStructuresForPOI { get; set; } = 2;
        
        /// <summary>
        /// Bonus por cada estructura adicional en la confluencia
        /// Ejemplo: 0.15 = cada estructura suma 15% al score compuesto
        /// </summary>
        public double POI_ConfluenceBonus { get; set; } = 0.15;
        
        /// <summary>
        /// Máximo bonus por confluencia (para evitar scores excesivos)
        /// Ejemplo: 0.5 = máximo 50% de bonus por confluencia
        /// </summary>
        public double POI_MaxConfluenceBonus { get; set; } = 0.5;
        
        /// <summary>
        /// Threshold para considerar un POI como "Premium" (0.0 - 1.0)
        /// 0.618 = 61.8% (nivel de Fibonacci) - POIs por encima son Premium
        /// POIs por debajo son Discount
        /// </summary>
        public double POI_PremiumThreshold { get; set; } = 0.618;
        
        /// <summary>
        /// Número de barras hacia atrás para calcular el rango del mercado
        /// Usado para determinar si un POI es Premium o Discount
        /// </summary>
        public int POI_PremiumLookbackBars { get; set; } = 50;

        // ========================================================================
        // PARÁMETROS LIQUIDITY VOIDS (LV)
        // ========================================================================
        
        /// <summary>
        /// Si true, requiere que el void tenga bajo volumen para ser detectado
        /// Si false, ignora la validación de volumen (más permisivo)
        /// </summary>
        public bool LV_RequireLowVolume { get; set; } = false;
        
        /// <summary>
        /// Threshold de volumen para considerar un void (como factor del volumen promedio)
        /// Ejemplo: 0.4 = volumen debe ser menor a 40% del promedio para ser void
        /// Solo aplica si LV_RequireLowVolume = true
        /// </summary>
        public double LV_VolumeThreshold { get; set; } = 0.4;
        
        /// <summary>
        /// Período para calcular el volumen promedio
        /// Usado para detectar zonas de bajo volumen
        /// </summary>
        public int LV_VolumeAvgPeriod { get; set; } = 20;
        
        /// <summary>
        /// Tamaño mínimo del void como factor del ATR
        /// Ejemplo: 0.15 = void debe ser al menos 15% del ATR
        /// </summary>
        public double LV_MinSizeATRFactor { get; set; } = 0.15;
        
        /// <summary>
        /// Habilita la fusión de voids consecutivos que se solapan
        /// true = fusionar en Extended Liquidity Voids
        /// </summary>
        public bool LV_EnableFusion { get; set; } = true;
        
        /// <summary>
        /// Tolerancia para fusionar voids (como factor del ATR)
        /// Ejemplo: 0.3 = voids a menos de 0.3 * ATR se fusionan
        /// </summary>
        public double LV_FusionToleranceATR { get; set; } = 0.3;
        
        /// <summary>
        /// Threshold de relleno para considerar un void como "filled"
        /// Ejemplo: 0.95 = 95% del rango debe ser rellenado
        /// </summary>
        public double LV_FillThreshold { get; set; } = 0.95;
        
        /// <summary>
        /// Peso del tamaño relativo en el scoring del void
        /// </summary>
        public double LV_SizeWeight { get; set; } = 0.4;
        
        /// <summary>
        /// Peso de la profundidad del hueco (volumen) en el scoring
        /// </summary>
        public double LV_DepthWeight { get; set; } = 0.3;
        
        /// <summary>
        /// Peso de la proximidad al precio actual en el scoring
        /// </summary>
        public double LV_ProximityWeight { get; set; } = 0.2;
        
        /// <summary>
        /// Multiplicador cuando el void confluye con FVG/OB
        /// Ejemplo: 1.3 = 30% de bonus por confluencia
        /// </summary>
        public double LV_ConfluenceMultiplier { get; set; } = 1.3;

        // ========================================================================
        // PARÁMETROS LIQUIDITY GRABS (LG)
        // ========================================================================
        
        /// <summary>
        /// Threshold de cuerpo para considerar un grab (como factor del ATR)
        /// Ejemplo: 0.6 = cuerpo debe ser al menos 60% del ATR
        /// </summary>
        public double LG_BodyThreshold { get; set; } = 0.6;
        
        /// <summary>
        /// Threshold de rango para considerar un grab (como factor del ATR)
        /// Ejemplo: 1.2 = rango total debe ser al menos 120% del ATR
        /// </summary>
        public double LG_RangeThreshold { get; set; } = 1.2;
        
        /// <summary>
        /// Factor de volumen para confirmación de grab
        /// Ejemplo: 1.5 = volumen debe ser 1.5x el promedio
        /// </summary>
        public double LG_VolumeSpikeFactor { get; set; } = 1.5;
        
        /// <summary>
        /// Período para calcular el volumen promedio
        /// </summary>
        public int LG_VolumeAvgPeriod { get; set; } = 20;
        
        /// <summary>
        /// Número máximo de barras para confirmar reversión después del grab
        /// Si no revierte en N barras, se marca como FailedGrab
        /// </summary>
        public int LG_MaxBarsForReversal { get; set; } = 3;
        
        /// <summary>
        /// Edad máxima en barras para un grab (purga rápida)
        /// Los grabs pierden relevancia rápidamente si no son retesteados
        /// </summary>
        public int LG_MaxAgeBars { get; set; } = 20;
        
        /// <summary>
        /// Peso de la fuerza del sweep en el scoring
        /// </summary>
        public double LG_SweepStrengthWeight { get; set; } = 0.3;
        
        /// <summary>
        /// Peso del volumen en el scoring
        /// </summary>
        public double LG_VolumeWeight { get; set; } = 0.25;
        
        /// <summary>
        /// Peso de la confirmación de reversión en el scoring
        /// </summary>
        public double LG_ReversalWeight { get; set; } = 0.3;
        
        /// <summary>
        /// Peso del contexto de bias en el scoring
        /// </summary>
        public double LG_BiasWeight { get; set; } = 0.15;
        
        /// <summary>
        /// Multiplicador de confianza cuando un grab se usa en setup de reversión
        /// Ejemplo: 1.3 = 30% de bonus en el Decision Fusion Model
        /// </summary>
        public double LG_ReversalSetupMultiplier { get; set; } = 1.3;

        // ========================================================================
        // CONFIGURACIÓN DE SISTEMA - PERSISTENCIA
        // ========================================================================
        
        /// <summary>
        /// Ruta del archivo de estado JSON
        /// Por defecto: Documents/NinjaTrader 8/PinkButterfly/brain_state.json
        /// </summary>
        public string StateFilePath { get; set; } = "Documents/NinjaTrader 8/PinkButterfly/brain_state.json";
        
        /// <summary>
        /// Habilita el guardado automático del estado
        /// </summary>
        public bool AutoSaveEnabled { get; set; } = true; // Activado para guardar el nuevo JSON con Fusión Jerárquica
        
        /// <summary>
        /// Intervalo en segundos para guardado automático del estado
        /// OPTIMIZACIÓN: Aumentado a 600 (10 min) para reducir I/O y mejorar velocidad
        /// </summary>
        public int StateSaveIntervalSecs { get; set; } = 600;
        
        // ========================================================================
        // PURGA AUTOMÁTICA DE ESTRUCTURAS (OPTIMIZACIÓN CRÍTICA)
        // ========================================================================
        
        /// <summary>
        /// Habilita la purga automática de estructuras obsoletas
        /// CRÍTICO: Previene degradación exponencial del rendimiento O(n²)
        /// </summary>
        public bool EnableAutoPurge { get; set; } = true;
        
        /// <summary>
        /// Frecuencia de purga en número de barras
        /// OPTIMIZACIÓN: Cada 25 barras para mantener memoria bajo control
        /// </summary>
        public int PurgeEveryNBars { get; set; } = 25;
        
        /// <summary>
        /// Edad máxima de una estructura en barras
        /// Estructuras más antiguas se eliminan automáticamente
        /// OPTIMIZACIÓN: 150 barras para balance entre contexto y rendimiento
        /// </summary>
        public int MaxStructureAgeBars { get; set; } = 150;
        
        /// <summary>
        /// Score mínimo para mantener una estructura
        /// Estructuras con score menor se eliminan
        /// OPTIMIZACIÓN: 0.20 para eliminar estructuras de baja calidad
        /// </summary>
        public double MinScoreToKeep { get; set; } = 0.20;
        
        /// <summary>
        /// Número máximo de estructuras activas por timeframe
        /// Si se excede, se eliminan las de menor score
        /// OPTIMIZACIÓN: 300 por TF (vs 500 antes) = 1500 total (vs 18,897)
        /// </summary>
        public int MaxStructuresPerTF { get; set; } = 300;
        
        /// <summary>
        /// Valida que el hash de configuración coincida al cargar estado
        /// Si es false, carga el estado sin validar (útil para migración)
        /// </summary>
        public bool ValidateConfigHashOnLoad { get; set; } = true;
        
        // ========================================================================
        // CONFIGURACIÓN DE SISTEMA - PURGA Y LÍMITES
        // ========================================================================
        
        /// <summary>
        /// Habilita la purga automática de estructuras
        /// Si es false, no se ejecuta ninguna purga (útil para tests)
        /// OPTIMIZACIÓN: Activado (true) para prevenir degradación exponencial
        /// </summary>
        public bool EnableAutoPurge { get; set; } = true;
        
        /// <summary>
        /// Máximo número de estructuras por timeframe
        /// Cuando se excede, se purgan las estructuras con score más bajo
        /// OPTIMIZACIÓN: Reducido a 300 (era 500) para mantener memoria bajo control
        /// </summary>
        public int MaxStructuresPerTF { get; set; } = 300;
        
        /// <summary>
        /// Score mínimo para mantener una estructura (0.0 - 1.0)
        /// Estructuras con score menor se purgan automáticamente
        /// OPTIMIZACIÓN: Aumentado a 0.20 (era 0.10) para eliminar estructuras de baja calidad
        /// </summary>
        public double MinScoreThreshold { get; set; } = 0.20;
        
        /// <summary>
        /// Edad máxima en barras para purgar estructuras inactivas
        /// Estructuras más antiguas se purgan si no son relevantes
        /// OPTIMIZACIÓN: Reducido a 150 (era 500) para purga más agresiva
        /// </summary>
        public int MaxAgeBarsForPurge { get; set; } = 150;
        
        /// <summary>
        /// Habilita purga agresiva para Liquidity Grabs
        /// Los LG pierden relevancia rápidamente (ya tienen LG_MaxAgeBars)
        /// </summary>
        public bool EnableAggressivePurgeForLG { get; set; } = true;
        
        // ========================================================================
        // CONFIGURACIÓN DE SISTEMA - LÍMITES POR TIPO
        // ========================================================================
        
        /// <summary>Máximo número de FVGs por timeframe</summary>
        public int MaxStructuresByType_FVG { get; set; } = 100;
        
        /// <summary>Máximo número de Order Blocks por timeframe</summary>
        public int MaxStructuresByType_OB { get; set; } = 80;
        
        /// <summary>Máximo número de Swings por timeframe</summary>
        public int MaxStructuresByType_Swing { get; set; } = 150;
        
        /// <summary>Máximo número de BOS/CHoCH por timeframe</summary>
        public int MaxStructuresByType_BOS { get; set; } = 50;
        
        /// <summary>Máximo número de POIs por timeframe</summary>
        public int MaxStructuresByType_POI { get; set; } = 60;
        
        /// <summary>Máximo número de Liquidity Voids por timeframe</summary>
        public int MaxStructuresByType_LV { get; set; } = 40;
        
        /// <summary>Máximo número de Liquidity Grabs por timeframe</summary>
        public int MaxStructuresByType_LG { get; set; } = 30;
        
        /// <summary>Máximo número de Double Tops/Bottoms por timeframe</summary>
        public int MaxStructuresByType_Double { get; set; } = 40;
        
        // ========================================================================
        // CONFIGURACIÓN DE SISTEMA - GENERAL
        // ========================================================================
        
        /// <summary>
        /// Activa logs de debug en NinjaTrader Output window
        /// ADVERTENCIA: Puede impactar performance en producción
        /// OPTIMIZACIÓN: Desactivado (false) para mejorar velocidad
        /// </summary>
        public bool EnableDebug { get; set; } = false;
        
        /// <summary>
        /// Activa el desglose detallado de scoring en cada decisión
        /// OPTIMIZACIÓN: Desactivado (false) para reducir spam de logs y mejorar velocidad
        /// </summary>
        public bool ShowScoringBreakdown { get; set; } = false;
        
        /// <summary>Versión del motor (para compatibilidad de persistencia)</summary>
        public string EngineVersion { get; set; } = "1.0.0";

        // ========================================================================
        // RENDIMIENTO Y CARGA HISTÓRICA
        // ========================================================================
        
        /// <summary>
        /// Máximo de barras históricas que el CoreEngine procesará (aplicado al TF más bajo)
        /// Ejemplo: 500 barras de 15m = ~5 días de datos
        /// Reduce el tiempo de carga inicial sin perder contexto operativo
        /// </summary>
        public int BacktestBarsForAnalysis { get; set; } = 5000; // Backtest largo para validación estadística (~35 días en 15m)
        
        /// <summary>
        /// Habilita/deshabilita el procesamiento de barras históricas
        /// Si es false, el sistema solo procesa desde el momento de carga en adelante
        /// </summary>
        public bool EnableHistoricalProcessing { get; set; } = true;
        
        /// <summary>
        /// FAST LOAD: Si es true, carga el estado desde brain_state.json en lugar de procesar histórico
        /// Acelera las pruebas de calibración del DecisionEngine (carga en 2-3 segundos)
        /// IMPORTANTE: Solo funciona si ya existe un brain_state.json previo
        /// </summary>
        public bool EnableFastLoadFromJSON { get; set; } = false;
        
        /// <summary>
        /// Número de barras de "cooldown" (enfriamiento) para órdenes canceladas
        /// Si una orden es cancelada, no se puede volver a registrar la misma estructura
        /// durante N barras. Evita ciclos infinitos de registro/cancelación.
        /// Ejemplo: 25 barras de 15m = ~6 horas de cooldown
        /// </summary>
        public int TradeCooldownBars { get; set; } = 25;
        
        /// <summary>
        /// Solo habilitar logging detallado (DEBUG/INFO) en las últimas N barras del histórico
        /// Reduce significativamente el tiempo de carga y el spam de logs
        /// Ejemplo: 100 = solo loggear las últimas 100 barras del histórico
        /// </summary>
        public int LoggingThresholdBars { get; set; } = 100;

        // ========================================================================
        // DECISION FUSION MODEL (DFM) PARAMETERS
        // ========================================================================
        
        // --------------------------------------------------------------------
        // HEATZONE DETECTION
        // --------------------------------------------------------------------
        
        /// <summary>
        /// Tolerancia de overlap para fusionar estructuras en HeatZones (como factor del ATR)
        /// Ejemplo: 0.5 = estructuras a menos de 0.5 * ATR se fusionan en una HeatZone
        /// </summary>
        public double HeatZone_OverlapToleranceATR { get; set; } = 0.5;
        
        /// <summary>
        /// Número mínimo de estructuras para crear una HeatZone
        /// Default: 2 (al menos 2 estructuras deben confluir)
        /// </summary>
        public int HeatZone_MinConfluence { get; set; } = 2;
        
        /// <summary>
        /// Referencia máxima de confluencia para normalización (saturación)
        /// Usado para calcular ConfluenceFactor = min(1.0, ConfluenceCount / MaxConfluenceReference)
        /// Default: 5 (confluencias de 5+ estructuras tienen el mismo peso máximo)
        /// </summary>
        public int MaxConfluenceReference { get; set; } = 5;
        
        /// <summary>
        /// Score mínimo para considerar una estructura en la creación de HeatZones
        /// Estructuras con score menor se ignoran para HeatZones
        /// </summary>
        public double HeatZone_MinScore { get; set; } = 0.3;
        
        // --------------------------------------------------------------------
        // DECISION SCORING WEIGHTS
        // --------------------------------------------------------------------
        
        /// <summary>
        /// Peso del score base de las estructuras en la decisión final
        /// CRÍTICO: La suma de todos los Weight_* debe ser exactamente 1.0
        /// OPTIMIZACIÓN: Aumentado a 0.50 (contribución real: 0.35, factor más importante)
        /// </summary>
        public double Weight_CoreScore { get; set; } = 0.50;
        
        /// <summary>
        /// Peso de la proximidad al precio actual en la decisión final
        /// OPTIMIZACIÓN: Reducido a 0.10 (contribución real: 0.08, estaba sobreponderado 3x)
        /// </summary>
        public double Weight_Proximity { get; set; } = 0.10;
        
        /// <summary>
        /// Peso de la confluencia de estructuras en la decisión final
        /// OPTIMIZACIÓN: Mantenido en 0.10 (contribución real: 0.05, bien calibrado)
        /// </summary>
        public double Weight_Confluence { get; set; } = 0.10;
        
        /// <summary>
        /// Peso del tipo de estructura (OB > FVG, etc.) en la decisión final
        /// OPTIMIZACIÓN: Mantenido en 0.10 (contribución real: 0.07, razonable)
        /// </summary>
        public double Weight_Type { get; set; } = 0.10;
        
        /// <summary>
        /// Peso del alineamiento con el bias global en la decisión final
        /// OPTIMIZACIÓN: Reducido a 0.10 (contribución real: 0.06, estaba sobreponderado 3.3x)
        /// </summary>
        public double Weight_Bias { get; set; } = 0.10;
        
        /// <summary>
        /// Peso del momentum (BOS/CHoCH) en la decisión final
        /// OPTIMIZACIÓN: Activado en 0.10 para aprovechar momentum estructural
        /// </summary>
        public double Weight_Momentum { get; set; } = 0.10;
        
        /// <summary>
        /// Peso del volumen en la decisión final
        /// CALIBRADO: Reducido a 0.00 para simplificar y asegurar suma = 1.0
        /// </summary>
        public double Weight_Volume { get; set; } = 0.00;
        
        // --------------------------------------------------------------------
        // TYPE HIERARCHY (para Weight_Type)
        // --------------------------------------------------------------------
        
        /// <summary>
        /// Multiplicadores por tipo de estructura para el cálculo de Type Contribution
        /// Estructuras más fuertes (OB, POI) tienen multiplicadores más altos
        /// </summary>
        public Dictionary<string, double> TypeMultipliers { get; set; } = new Dictionary<string, double>
        {
            { "OB", 1.5 },
            { "POI", 1.4 },
            { "LG", 1.3 },
            { "BOS", 1.2 },
            { "FVG", 1.0 },
            { "LV", 0.9 },
            { "Swing", 0.8 },
            { "Double", 1.1 }
        };
        
        // --------------------------------------------------------------------
        // RISK MANAGEMENT
        // --------------------------------------------------------------------
        
        /// <summary>
        /// Porcentaje del capital a arriesgar por trade (0.0 - 100.0)
        /// Default: 0.5% (conservador y profesional)
        /// </summary>
        public double RiskPercentPerTrade { get; set; } = 0.5;
        
        /// <summary>
        /// Ratio mínimo de Risk:Reward para entrar en un trade
        /// Default: 1.5 (realista para operativa profesional)
        /// </summary>
        public double MinRiskRewardRatio { get; set; } = 1.5;
        
        /// <summary>
        /// Buffer adicional para el Stop Loss (como factor del ATR)
        /// Ejemplo: 0.2 = añadir 20% del ATR al SL estructural
        /// </summary>
        public double SL_BufferATR { get; set; } = 0.2;
        
        // --------------------------------------------------------------------
        // DECISION THRESHOLDS
        // --------------------------------------------------------------------
        
        /// <summary>
        /// Confidence mínima para generar señal de BUY/SELL (0.0 - 1.0)
        /// CALIBRACIÓN: Ajustado a 0.55 para aumentar frecuencia de señales (Test 5000 barras)
        /// </summary>
        public double MinConfidenceForEntry { get; set; } = 0.55;
        
        /// <summary>
        /// Factor de proximidad máxima para validar Entry estructural
        /// Distancia máxima permitida = MaxEntryProximityFactor × ATR
        /// CALIBRADO: Aumentado de 15.0 a 20.0 para permitir pullbacks más profundos
        /// </summary>
        public double MaxEntryProximityFactor { get; set; } = 20.0;
        
        // ========================================================================
        // LÍMITES DE RISK:REWARD (OPTIMIZACIÓN CRÍTICA)
        // ========================================================================
        
        /// <summary>
        /// Distancia máxima permitida para el Stop Loss (en múltiplos de ATR)
        /// OPTIMIZACIÓN: Límite de 15 ATR para evitar SLs absurdos (79-107 puntos)
        /// Operaciones con SL > 15 ATR se rechazan
        /// </summary>
        public double MaxSLDistanceATR { get; set; } = 15.0;
        
        /// <summary>
        /// Distancia mínima requerida para el Take Profit (en múltiplos de ATR)
        /// OPTIMIZACIÓN: Mínimo 2 ATR para asegurar recompensa razonable
        /// </summary>
        public double MinTPDistanceATR { get; set; } = 2.0;
        
        /// <summary>
        /// Risk:Reward mínimo requerido para ejecutar una operación
        /// OPTIMIZACIÓN: R:R mínimo de 1.0 para eliminar operaciones con R:R absurdo (0.05-0.18)
        /// Operaciones con R:R < 1.0 se rechazan
        /// </summary>
        public double MinRiskRewardRatio { get; set; } = 1.0;
        
        /// <summary>
        /// Confidence mínima para generar señal de WAIT (0.0 - 1.0)
        /// Si Confidence < MinConfidenceForWait, se genera NO_SIGNAL
        /// </summary>
        public double MinConfidenceForWait { get; set; } = 0.50; // PRODUCTIVO: Ajustado después de calibración
        
        /// <summary>
        /// Distancia máxima para considerar una zona "cercana" (como factor del ATR)
        /// CALIBRADO: Aumentado de 1.5 a 5.0 para normalizar proximidad en mercados volátiles
        /// Ejemplo: 5.0 = zonas a más de 5.0 * ATR tienen ProximityScore = 0
        /// </summary>
        public double ProximityThresholdATR { get; set; } = 5.0;
        
        /// <summary>
        /// Factor de penalización para trades contra el bias global
        /// Si TradeDirection != GlobalBias, Confidence requerida *= (1 / BiasOverrideConfidenceFactor)
        /// Ejemplo: 0.85 = requiere ~18% más de confidence para operar contra tendencia
        /// </summary>
        public double BiasOverrideConfidenceFactor { get; set; } = 0.85;
        
        // --------------------------------------------------------------------
        // MARKET CLARITY
        // --------------------------------------------------------------------
        
        /// <summary>
        /// Número mínimo de estructuras activas para considerar el mercado "claro"
        /// Menos estructuras = mercado confuso = menor confidence
        /// </summary>
        public double MarketClarity_MinStructures { get; set; } = 5;
        
        /// <summary>
        /// Edad máxima en barras para considerar una estructura "reciente"
        /// Estructuras más antiguas no contribuyen a la claridad del mercado
        /// </summary>
        public double MarketClarity_MaxAge { get; set; } = 100;

        // ========================================================================
        // MÉTODOS
        // ========================================================================

        /// <summary>
        /// Calcula el hash SHA256 de la configuración
        /// Usado para validar que el estado guardado corresponde a la misma configuración
        /// </summary>
        /// <returns>Hash SHA256 en formato hexadecimal</returns>
        public string GetHash()
        {
            // Serializar configuración a JSON (determinístico)
            string json = ToJson(indented: false);
            
            // Calcular SHA256
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(json));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        /// <summary>
        /// Obtiene el peso máximo de timeframe configurado
        /// Usado para normalización en scoring
        /// </summary>
        [JsonIgnore]
        public double MaxTFWeight => TFWeights.Values.Max();

        /// <summary>
        /// Obtiene el peso máximo de tipo de estructura configurado
        /// Usado para normalización en scoring
        /// </summary>
        [JsonIgnore]
        public double MaxTypeWeight => TypeWeights.Values.Max();

        /// <summary>
        /// Carga configuración por defecto
        /// Útil para inicialización y tests
        /// </summary>
        public static EngineConfig LoadDefaults()
        {
            return new EngineConfig();
        }

        /// <summary>
        /// Carga configuración desde JSON
        /// </summary>
        public static EngineConfig LoadFromJson(string json)
        {
            return JsonConvert.DeserializeObject<EngineConfig>(json);
        }

        /// <summary>
        /// Exporta configuración a JSON
        /// </summary>
        public string ToJson(bool indented = true)
        {
            var formatting = indented ? Formatting.Indented : Formatting.None;
            return JsonConvert.SerializeObject(this, formatting);
        }

        // ========================================================================
        // SISTEMA DE PROGRESO (Progress Tracking)
        // ========================================================================
        
        /// <summary>
        /// Mostrar barra de progreso durante procesamiento histórico
        /// </summary>
        public bool ShowProgressBar { get; set; } = true;
        
        /// <summary>
        /// Reportar progreso cada N barras procesadas
        /// CALIBRACIÓN: Ajustado a 200 barras para reportes más frecuentes (cada ~1 min)
        /// </summary>
        public int ProgressReportEveryNBars { get; set; } = 200;
        
        /// <summary>
        /// Reportar progreso cada X minutos (para procesos muy lentos)
        /// CALIBRACIÓN: Ajustado a 1 minuto para mejor visibilidad en logs largos
        /// </summary>
        public int ProgressReportEveryMinutes { get; set; } = 1;
        
        /// <summary>
        /// Mostrar información detallada en reportes de progreso
        /// (estructuras, guardados, velocidad, ETA)
        /// </summary>
        public bool ShowDetailedProgress { get; set; } = true;

        // ========================================================================
        // VALIDACIÓN
        // ========================================================================

        /// <summary>
        /// Valida que la configuración sea coherente
        /// Lanza excepciones si hay valores inválidos
        /// </summary>
        public void Validate()
        {
            if (TimeframesToUse == null || TimeframesToUse.Count == 0)
                throw new ArgumentException("TimeframesToUse no puede estar vacío");

            if (TimeframesToUse.Any(tf => tf <= 0))
                throw new ArgumentException("Todos los timeframes deben ser > 0");

            if (MinFVGSizeTicks < 0)
                throw new ArgumentException("MinFVGSizeTicks debe ser >= 0");

            if (MinFVGSizeATRfactor < 0)
                throw new ArgumentException("MinFVGSizeATRfactor debe ser >= 0");

            if (FillThreshold < 0 || FillThreshold > 1)
                throw new ArgumentException("FillThreshold debe estar en rango [0, 1]");

            if (ResidualScore < 0 || ResidualScore > 1)
                throw new ArgumentException("ResidualScore debe estar en rango [0, 1]");

            if (MaxStructuresPerTF <= 0)
                throw new ArgumentException("MaxStructuresPerTF debe ser > 0");

            // Validar que todos los TFWeights tengan valores válidos
            foreach (var kv in TFWeights)
            {
                if (kv.Value < 0 || kv.Value > 2)
                    throw new ArgumentException($"TFWeights[{kv.Key}] debe estar en rango [0, 2]");
            }

            // Validar TypeWeights
            foreach (var kv in TypeWeights)
            {
                if (kv.Value < 0 || kv.Value > 2)
                    throw new ArgumentException($"TypeWeights[{kv.Key}] debe estar en rango [0, 2]");
            }
            
            // Validar parámetros de progreso
            if (ProgressReportEveryNBars <= 0)
                throw new ArgumentException("ProgressReportEveryNBars debe ser > 0");
                
            if (ProgressReportEveryMinutes <= 0)
                throw new ArgumentException("ProgressReportEveryMinutes debe ser > 0");
            
            // Validar parámetros de purga automática
            if (EnableAutoPurge)
            {
                if (PurgeEveryNBars <= 0)
                    throw new ArgumentException("PurgeEveryNBars debe ser > 0");
                
                if (MaxStructureAgeBars <= 0)
                    throw new ArgumentException("MaxStructureAgeBars debe ser > 0");
                
                if (MinScoreToKeep < 0 || MinScoreToKeep > 1)
                    throw new ArgumentException("MinScoreToKeep debe estar en rango [0, 1]");
                
                if (MaxStructuresPerTF <= 0)
                    throw new ArgumentException("MaxStructuresPerTF debe ser > 0");
            }
            
            // Validar límites de Risk:Reward
            if (MaxSLDistanceATR <= 0)
                throw new ArgumentException("MaxSLDistanceATR debe ser > 0");
            
            if (MinTPDistanceATR <= 0)
                throw new ArgumentException("MinTPDistanceATR debe ser > 0");
            
            if (MinRiskRewardRatio < 0)
                throw new ArgumentException("MinRiskRewardRatio debe ser >= 0");
        }
    }
}

