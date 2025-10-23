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
}

