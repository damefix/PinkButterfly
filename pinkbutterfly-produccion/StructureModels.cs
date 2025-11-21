// ============================================================================
// StructureModels.cs
// PinkButterfly CoreBrain - Modelos de datos para estructuras de mercado
// 
// Contiene todas las clases de estructuras detectadas por el motor:
// - StructureBase (clase abstracta base)
// - FVGInfo (Fair Value Gaps)
// - SwingInfo (Swing Highs/Lows)
// - DoubleTopInfo (Double Tops/Bottoms)
// - OrderBlockInfo (Order Blocks)
// - StructureBreakInfo (BOS/CHoCH)
// - PointOfInterestInfo (POI - Confluencias)
//
// Serialización polimórfica configurada para Newtonsoft.Json
// Requiere referencia a Newtonsoft.Json.dll en NinjaTrader
// ============================================================================

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    // ============================================================================
    // CLASE BASE ABSTRACTA
    // ============================================================================
    
    /// <summary>
    /// Clase base abstracta para todas las estructuras de mercado detectadas.
    /// Contiene propiedades comunes a todas las estructuras (precio, tiempo, scoring, toques, etc.)
    /// 
    /// IMPORTANTE: La serialización polimórfica se maneja con Newtonsoft.Json
    /// usando TypeNameHandling.Auto en JsonSerializerSettings
    /// </summary>
    public abstract class StructureBase
    {
        /// <summary>ID único de la estructura (GUID)</summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>Tipo de estructura: "FVG", "SWING", "DOUBLE_TOP", "OB", "POI", "BOS", "CHoCH"</summary>
        public string Type { get; set; }
        
        /// <summary>Timeframe en minutos (15, 60, 240, 1440, etc.)</summary>
        public int TF { get; set; }
        
        /// <summary>Tiempo de inicio de la estructura (UTC)</summary>
        public DateTime StartTime { get; set; }
        
        /// <summary>Tiempo de fin de la estructura (UTC)</summary>
        public DateTime EndTime { get; set; }
        
        /// <summary>Precio máximo de la estructura (límite superior de la zona)</summary>
        public double High { get; set; }
        
        /// <summary>Precio mínimo de la estructura (límite inferior de la zona)</summary>
        public double Low { get; set; }
        
        /// <summary>Calcula el precio central de la estructura</summary>
        /// <returns>Precio medio entre High y Low</returns>
        [JsonIgnore]
        public double CenterPrice => (High + Low) / 2.0;
        
        /// <summary>Indica si la estructura está activa (no ha sido invalidada)</summary>
        public bool IsActive { get; set; } = true;
        
        /// <summary>Indica si la estructura está completada (formación finalizada)</summary>
        public bool IsCompleted { get; set; } = false;
        
        /// <summary>Índice de barra en la que se creó la estructura</summary>
        public int CreatedAtBarIndex { get; set; }
        
        /// <summary>Índice de barra de la última actualización</summary>
        public int LastUpdatedBarIndex { get; set; }
        
        /// <summary>
        /// Score de la estructura (0.0 - 1.0)
        /// Calculado dinámicamente por el motor de scoring basado en:
        /// - Peso del timeframe
        /// - Frescura (freshness)
        /// - Proximidad al precio actual
        /// - Peso del tipo de estructura
        /// - Factor de toques
        /// - Confluencia
        /// - Momentum multiplier
        /// </summary>
        public double Score { get; set; } = 0.0;
        
        /// <summary>Número de veces que el cuerpo de una vela ha tocado la estructura</summary>
        public int TouchCount_Body { get; set; } = 0;
        
        /// <summary>Número de veces que la mecha de una vela ha tocado la estructura</summary>
        public int TouchCount_Wick { get; set; } = 0;
        
        /// <summary>IDs de estructuras relacionadas (para confluencias, etc.)</summary>
        public List<string> RelatedStructureIds { get; set; } = new List<string>();
        
        /// <summary>Metadata adicional (volumen, rango, tags personalizados, etc.)</summary>
        public StructureMetadata Metadata { get; set; } = new StructureMetadata();
    }

    // ============================================================================
    // METADATA
    // ============================================================================
    
    /// <summary>
    /// Metadata adicional para estructuras
    /// Permite almacenar información contextual que puede ser útil para análisis
    /// pero que no es parte del modelo core de la estructura
    /// </summary>
    public class StructureMetadata
    {
        /// <summary>Volumen en el momento de creación de la estructura (nullable si no disponible)</summary>
        public double? VolumeAtCreation { get; set; }
        
        /// <summary>Rango promedio de las barras durante la formación de la estructura</summary>
        public double? AverageRangeDuringFormation { get; set; }
        
        /// <summary>Nombre del detector que creó esta estructura</summary>
        public string CreatedByDetector { get; set; }
        
        /// <summary>Tags personalizados (clave-valor) para clasificación o filtrado</summary>
        public Dictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();
        
        // ========================================================================
        // PROXIMITY SCORING (Actualizado dinámicamente en Fast Load Mode)
        // ========================================================================
        
        /// <summary>Score de proximidad (0.0-1.0) - Recalculado en cada barra</summary>
        public double ProximityScore { get; set; } = 0.0;
        
        /// <summary>Factor de proximidad (0.0-1.0) - Alias de ProximityScore</summary>
        public double ProximityFactor { get; set; } = 0.0;
        
        /// <summary>Distancia al precio actual en ATR</summary>
        public double DistanceATR { get; set; } = 0.0;
        
        /// <summary>Penalización por tamaño de zona (0.1-1.0)</summary>
        public double SizePenalty { get; set; } = 1.0;
        
        /// <summary>Altura de la zona en ATR</summary>
        public double ZoneHeightATR { get; set; } = 0.0;
        
        /// <summary>Índice de barra de la última actualización de proximidad</summary>
        public int LastProximityUpdate { get; set; } = -1;
    }

    // ============================================================================
    // ESTRUCTURAS ESPECÍFICAS
    // ============================================================================

    /// <summary>
    /// Fair Value Gap (FVG) - Zona de ineficiencia de precio
    /// Detectada cuando hay un gap entre las mechas de 3 barras consecutivas
    /// </summary>
    public class FVGInfo : StructureBase
    {
        public FVGInfo()
        {
            Type = "FVG";
        }

        /// <summary>Dirección del FVG: "Bullish" o "Bearish"</summary>
        public string Direction { get; set; }
        
        /// <summary>
        /// Porcentaje de relleno del FVG (0.0 - 1.0)
        /// 0.0 = no tocado, 1.0 = completamente rellenado
        /// </summary>
        public double FillPercentage { get; set; } = 0.0;
        
        /// <summary>Indica si el FVG fue tocado recientemente (últimas N barras)</summary>
        public bool TouchedRecently { get; set; } = false;
        
        /// <summary>
        /// ID del FVG padre si este es un FVG anidado
        /// null si es un FVG de nivel superior
        /// </summary>
        public string ParentId { get; set; }
        
        /// <summary>
        /// Nivel de profundidad en la jerarquía de FVGs anidados
        /// 0 = nivel superior, 1+ = anidado
        /// </summary>
        public int DepthLevel { get; set; } = 0;
    }

    /// <summary>
    /// Swing High o Swing Low
    /// Detectado usando algoritmo de n-left y n-right bars
    /// </summary>
    public class SwingInfo : StructureBase
    {
        public SwingInfo()
        {
            Type = "SWING";
        }

        /// <summary>true = Swing High, false = Swing Low</summary>
        public bool IsHigh { get; set; }
        
        /// <summary>Número de barras a la izquierda usadas para detección</summary>
        public int LeftN { get; set; }
        
        /// <summary>Número de barras a la derecha usadas para detección</summary>
        public int RightN { get; set; }
        
        /// <summary>Tamaño del swing en ticks</summary>
        public double SwingSizeTicks { get; set; }
        
        /// <summary>Indica si el swing ha sido roto (price closed beyond)</summary>
        public bool IsBroken { get; set; } = false;
    }

    /// <summary>
    /// Double Top o Double Bottom
    /// Patrón de reversión clásico con dos swings a precio similar y neckline
    /// </summary>
    public class DoubleTopInfo : StructureBase
    {
        public DoubleTopInfo()
        {
            Type = "DOUBLE_TOP";
        }

        /// <summary>Tiempo del primer swing</summary>
        public DateTime Swing1Time { get; set; }
        
        /// <summary>Tiempo del segundo swing</summary>
        public DateTime Swing2Time { get; set; }
        
        /// <summary>Precio de la línea de cuello (neckline)</summary>
        public double NecklinePrice { get; set; }
        
        /// <summary>
        /// Estado del patrón:
        /// - "Pending": En formación, esperando confirmación
        /// - "Confirmed": Neckline rota, patrón confirmado
        /// - "Invalid": Invalidado por movimiento contrario
        /// </summary>
        public string Status { get; set; } = "Pending";
    }

    /// <summary>
    /// Order Block (OB) - Zona de actividad institucional
    /// Detectado por velas con cuerpo grande y/o volumen spike
    /// </summary>
    public class OrderBlockInfo : StructureBase
    {
        public OrderBlockInfo()
        {
            Type = "OB";
        }

        /// <summary>Dirección: "Bullish" o "Bearish"</summary>
        public string Direction { get; set; }
        
        /// <summary>Precio de apertura de la vela OB</summary>
        public double OpenPrice { get; set; }
        
        /// <summary>Precio de cierre de la vela OB</summary>
        public double ClosePrice { get; set; }
        
        /// <summary>
        /// Indica si es un Breaker Block (OB que fue roto y ahora actúa en dirección contraria)
        /// </summary>
        public bool IsBreaker { get; set; } = false;
        
        /// <summary>
        /// Indica si el OB ha sido mitigado (precio retornó a la zona)
        /// </summary>
        public bool IsMitigated { get; set; } = false;
        
        /// <summary>
        /// Indica si el precio ha salido de la zona del OB
        /// Necesario para detectar correctamente la mitigación (retorno a la zona)
        /// </summary>
        public bool HasLeftZone { get; set; } = false;
    }

    /// <summary>
    /// Break of Structure (BOS) o Change of Character (CHoCH)
    /// Indica cambios en la estructura del mercado
    /// </summary>
    public class StructureBreakInfo : StructureBase
    {
        public StructureBreakInfo()
        {
            Type = "BOS"; // Puede cambiar a "CHoCH"
        }

        /// <summary>Tipo de ruptura: "BOS" o "CHoCH"</summary>
        public string BreakType { get; set; }
        
        /// <summary>ID del swing que fue roto</summary>
        public string BrokenSwingId { get; set; }
        
        /// <summary>Precio al que ocurrió la ruptura</summary>
        public double BreakPrice { get; set; }
        
        /// <summary>Dirección de la ruptura: "Bullish" o "Bearish"</summary>
        public string Direction { get; set; }
        
        /// <summary>
        /// Momentum de la ruptura: "Strong" o "Weak"
        /// Strong: Vela grande con cuerpo >= BreakMomentumBodyFactor * ATR
        /// Weak: Ruptura con vela pequeña o bajo volumen
        /// </summary>
        public string BreakMomentum { get; set; } = "Weak";
        
        /// <summary>Índice de barra donde se formó el swing que fue roto</summary>
        public int SwingBarIndex { get; set; }
        
        /// <summary>Precio del swing que fue roto (High si es Swing High, Low si es Swing Low)</summary>
        public double SwingPrice { get; set; }
    }

    /// <summary>
    /// Point of Interest (POI) - Zona de confluencia
    /// Creado cuando múltiples estructuras se superponen en una zona
    /// </summary>
    public class PointOfInterestInfo : StructureBase
    {
        public PointOfInterestInfo()
        {
            Type = "POI";
        }

        /// <summary>IDs de las estructuras que forman esta confluencia</summary>
        public List<string> SourceIds { get; set; } = new List<string>();
        
        /// <summary>
        /// Score compuesto basado en las estructuras fuente
        /// Calculado como weighted sum de los scores de las estructuras componentes
        /// </summary>
        public double CompositeScore { get; set; } = 0.0;
        
        /// <summary>
        /// Bias del POI: "BuySide", "SellSide" o "Neutral"
        /// Determinado por la mayoría de estructuras alcistas/bajistas en la confluencia
        /// </summary>
        public string Bias { get; set; } = "Neutral";
        
        /// <summary>
        /// Indica si este POI está en zona premium (por encima del 50% del rango reciente)
        /// o discount (por debajo del 50%)
        /// </summary>
        public bool IsPremium { get; set; } = false;
    }

    /// <summary>
    /// Liquidity Void (LV) - Zona de ineficiencia sin liquidez
    /// Detectado cuando hay ausencia significativa de negociación entre dos velas
    /// (gap sin solapamiento de mechas o con volumen extremadamente bajo)
    /// </summary>
    public class LiquidityVoidInfo : StructureBase
    {
        public LiquidityVoidInfo()
        {
            Type = "LIQUIDITY_VOID";
        }

        /// <summary>Dirección del void: "Bullish" o "Bearish"</summary>
        public string Direction { get; set; }
        
        /// <summary>
        /// Score de ineficiencia (0.0 - 1.0)
        /// Combina la falta de solapamiento y la baja actividad de volumen/delta
        /// </summary>
        public double InefficiencyScore { get; set; } = 0.0;
        
        /// <summary>
        /// Ratio de volumen relativo al promedio
        /// Valores bajos (<0.4) indican mayor ineficiencia
        /// </summary>
        public double VolumeRatio { get; set; } = 0.0;
        
        /// <summary>
        /// Indica si este void se ha rellenado completamente (100% del rango)
        /// </summary>
        public bool IsFilled { get; set; } = false;
        
        /// <summary>
        /// Porcentaje de relleno del void (0.0 - 1.0)
        /// 0.0 = no tocado, 1.0 = completamente rellenado
        /// </summary>
        public double FillPercentage { get; set; } = 0.0;
        
        /// <summary>
        /// Indica si este void es parte de un Extended Liquidity Void
        /// (fusión de múltiples voids consecutivos)
        /// </summary>
        public bool IsExtended { get; set; } = false;
        
        /// <summary>
        /// ID del void padre si este es parte de un Extended Void
        /// null si es un void independiente
        /// </summary>
        public string ParentVoidId { get; set; }
        
        /// <summary>
        /// Tamaño del void en ticks
        /// </summary>
        public double SizeTicks { get; set; } = 0.0;
    }

    /// <summary>
    /// Liquidity Grab (LG) - Stop Hunt / Sweep de liquidez
    /// Movimiento brusco que supera un swing previo y revierte inmediatamente
    /// Indica absorción de liquidez pasiva (stops) y posible reversión
    /// </summary>
    public class LiquidityGrabInfo : StructureBase
    {
        public LiquidityGrabInfo()
        {
            Type = "LIQUIDITY_GRAB";
        }

        /// <summary>
        /// Tipo de liquidez capturada:
        /// - "BuySide": Sweep de máximos (liquidez por encima)
        /// - "SellSide": Sweep de mínimos (liquidez por debajo)
        /// </summary>
        public string DirectionalBias { get; set; }
        
        /// <summary>
        /// Precio al que ocurrió el sweep (el extremo de la mecha)
        /// </summary>
        public double GrabPrice { get; set; }
        
        /// <summary>
        /// Precio de cierre de la vela que hizo el sweep
        /// </summary>
        public double ClosePrice { get; set; }
        
        /// <summary>
        /// ID del swing que fue barrido (sweep)
        /// </summary>
        public string RelatedSwingId { get; set; }
        
        /// <summary>
        /// Volumen en el momento del grab
        /// </summary>
        public double VolumeAtGrab { get; set; } = 0.0;
        
        /// <summary>
        /// Ratio de volumen relativo al promedio
        /// Valores altos (>1.5) indican mayor fuerza del grab
        /// </summary>
        public double VolumeRatio { get; set; } = 0.0;
        
        /// <summary>
        /// Indica si se confirmó la reversión
        /// true si el precio cerró en dirección opuesta al sweep y no volvió a romper el GrabPrice
        /// </summary>
        public bool ConfirmedReversal { get; set; } = false;
        
        /// <summary>
        /// Indica si el grab falló (el precio continuó en la dirección del sweep)
        /// Marca un "TrueBreak" en lugar de un grab
        /// </summary>
        public bool FailedGrab { get; set; } = false;
        
        /// <summary>
        /// Distancia del sweep más allá del swing (en ticks)
        /// </summary>
        public double SweepDistanceTicks { get; set; } = 0.0;
        
        /// <summary>
        /// Fuerza del sweep (0.0 - 1.0)
        /// Basado en el tamaño del cuerpo y rango de la vela vs ATR
        /// </summary>
        public double SweepStrength { get; set; } = 0.0;
        
        /// <summary>
        /// Número de barras desde el grab
        /// Usado para purga rápida (LG pierde relevancia rápidamente)
        /// </summary>
        public int BarsSinceGrab { get; set; } = 0;
    }
}

