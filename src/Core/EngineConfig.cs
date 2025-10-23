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
        /// Por defecto: [15, 60, 240, 1440] = 15m, 1H, 4H, Daily
        /// </summary>
        public List<int> TimeframesToUse { get; set; } = new List<int> { 15, 60, 240, 1440 };

        // ========================================================================
        // PARÁMETROS FVG (FAIR VALUE GAP)
        // ========================================================================
        
        /// <summary>Tamaño mínimo de FVG en ticks (absoluto)</summary>
        public double MinFVGSizeTicks { get; set; } = 6;
        
        /// <summary>Tamaño mínimo de FVG como factor del ATR (relativo a volatilidad)</summary>
        public double MinFVGSizeATRfactor { get; set; } = 0.12;
        
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
        public double MinSwingATRfactor { get; set; } = 0.05;
        
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
        /// Ejemplo: 0.6 = el cuerpo debe ser al menos 60% del ATR(14)
        /// </summary>
        public double OBBodyMinATR { get; set; } = 0.6;

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
        // CONFIGURACIÓN DE SISTEMA
        // ========================================================================
        
        /// <summary>
        /// Intervalo en segundos para guardado automático del estado
        /// Guardado asíncrono con debounce
        /// </summary>
        public int StateSaveIntervalSecs { get; set; } = 30;
        
        /// <summary>
        /// Máximo número de estructuras por timeframe
        /// Cuando se excede, se purgan las estructuras con score más bajo
        /// </summary>
        public int MaxStructuresPerTF { get; set; } = 500;
        
        /// <summary>
        /// Activa logs de debug en NinjaTrader Output window
        /// ADVERTENCIA: Puede impactar performance en producción
        /// </summary>
        public bool EnableDebug { get; set; } = false;
        
        /// <summary>Versión del motor (para compatibilidad de persistencia)</summary>
        public string EngineVersion { get; set; } = "1.0.0";

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
        }
    }
}

