using System;
using System.Collections.Generic;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// HeatZone: Zona de confluencia de estructuras.
    /// Es una abstracción creada por el DecisionEngine para agrupar múltiples estructuras solapadas.
    /// NO hereda de StructureBase porque es una estructura de fusión, no de detección.
    /// </summary>
    public class HeatZone
    {
        public string Id { get; set; }
        public string Direction { get; set; } // "Bullish", "Bearish", "Neutral"
        public double High { get; set; }
        public double Low { get; set; }
        public double CenterPrice => (High + Low) / 2.0;
        public double Score { get; set; } // Score agregado de estructuras
        public int ConfluenceCount { get; set; } // Número de estructuras en la zona
        public List<string> SourceStructureIds { get; set; } // IDs de estructuras que forman la zona
        
        // CRÍTICO: Campos para anclar el SL y definir la personalidad de la zona
        public string DominantType { get; set; } // Tipo de la estructura dominante (FVG, OB, POI, etc.)
        public int TFDominante { get; set; } // Timeframe de la estructura más fuerte
        public string DominantStructureId { get; set; } // ID del StructureBase que ancla la zona
        
        public Dictionary<string, object> Metadata { get; set; } // TF, tipo dominante, etc.

        public HeatZone()
        {
            SourceStructureIds = new List<string>();
            Metadata = new Dictionary<string, object>();
        }
    }

    /// <summary>
    /// TradeDecision: Decisión de trading completa con Entry, SL, TP, Confidence, Rationale.
    /// Incluye PositionSizeContracts para gestión de riesgo.
    /// </summary>
    public class TradeDecision
    {
        public string Id { get; set; }
        public string Action { get; set; } // "BUY", "SELL", "WAIT", "CLOSE", "PARTIAL_TAKE"
        public double Confidence { get; set; } // 0.0 - 1.0
        public double Entry { get; set; }
        public double StopLoss { get; set; }
        public double TakeProfit { get; set; }
        public double PositionSizeContracts { get; set; } // CRÍTICO: Tamaño de posición calculado
        public string Rationale { get; set; } // Explicación corta
        public DecisionScoreBreakdown Explainability { get; set; }
        public List<string> SourceStructureIds { get; set; }
        public string DominantStructureId { get; set; } // ID de la estructura dominante (para TradeManager)
        public DateTime GeneratedAt { get; set; }

        public TradeDecision()
        {
            SourceStructureIds = new List<string>();
            GeneratedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// DecisionScoreBreakdown: Desglose de scoring para explainability.
    /// Permite auditar cómo se calculó la Confidence de una decisión.
    /// </summary>
    public class DecisionScoreBreakdown
    {
        public double CoreScoreContribution { get; set; } // Peso del score de estructuras
        public double ProximityContribution { get; set; } // Peso de la proximidad al precio
        public double ConfluenceContribution { get; set; } // Peso de la confluencia de estructuras
        public double TypeContribution { get; set; } // Peso del tipo de estructura (OB > FVG, etc.)
        public double BiasContribution { get; set; } // Peso del alineamiento con el bias global
        public double MomentumContribution { get; set; } // Peso del momentum (BOS/CHoCH)
        public double VolumeContribution { get; set; } // Peso del volumen
        public double ModelProbability { get; set; } // Para modo adaptativo (ML)
        public double FinalConfidence { get; set; } // Confidence final (0.0 - 1.0)

        public DecisionScoreBreakdown()
        {
            // Valores por defecto
            CoreScoreContribution = 0.0;
            ProximityContribution = 0.0;
            ConfluenceContribution = 0.0;
            TypeContribution = 0.0;
            BiasContribution = 0.0;
            MomentumContribution = 0.0;
            VolumeContribution = 0.0;
            ModelProbability = 0.0;
            FinalConfidence = 0.0;
        }
    }

    /// <summary>
    /// DecisionSnapshot: Input completo para el DFM.
    /// Contiene todas las HeatZones, el bias global, y métricas agregadas del mercado.
    /// </summary>
    public class DecisionSnapshot
    {
        public DateTime GeneratedAt { get; set; }
        public string Instrument { get; set; }
        public List<HeatZone> HeatZones { get; set; }
        public string GlobalBias { get; set; } // "Bullish", "Bearish", "Neutral"
        public double GlobalBiasStrength { get; set; } // 0.0 - 1.0
        public double MarketClarity { get; set; } // 0.0 - 1.0 (alta = estructuras claras)
        public double MarketVolatilityNormalized { get; set; } // 0.0 - 1.0 (normalizado por ATR)
        public string MarketRegime { get; set; } // V6.0i: "Normal" o "HighVol" (con histéresis)
        public DecisionSummary Summary { get; set; }
        public Dictionary<string, object> Metadata { get; set; } // Metadata adicional para el pipeline

        public DecisionSnapshot()
        {
            GeneratedAt = DateTime.UtcNow;
            HeatZones = new List<HeatZone>();
            Summary = new DecisionSummary();
            Metadata = new Dictionary<string, object>();
        }
    }

    /// <summary>
    /// DecisionSummary: Métricas agregadas del mercado.
    /// Proporciona contexto global para el DFM.
    /// </summary>
    public class DecisionSummary
    {
        public double CurrentPrice { get; set; }
        public Dictionary<int, double> ATRByTF { get; set; } // TF -> ATR
        public int TotalStructures { get; set; }
        public int TotalActiveStructures { get; set; }
        public Dictionary<string, int> StructuresByType { get; set; } // Tipo -> Count

        public DecisionSummary()
        {
            ATRByTF = new Dictionary<int, double>();
            StructuresByType = new Dictionary<string, int>();
        }
    }
}

