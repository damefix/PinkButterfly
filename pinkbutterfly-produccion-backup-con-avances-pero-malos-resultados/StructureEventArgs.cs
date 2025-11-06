// ============================================================================
// StructureEventArgs.cs
// PinkButterfly CoreBrain - Modelos de eventos para estructuras
// 
// Define los argumentos de eventos que se disparan cuando:
// - Se añade una estructura (OnStructureAdded)
// - Se actualiza una estructura (OnStructureUpdated)
// - Se elimina una estructura (OnStructureRemoved)
// ============================================================================

using System;

namespace NinjaTrader.NinjaScript.Indicators.PinkButterfly
{
    /// <summary>
    /// Argumentos del evento OnStructureAdded
    /// Se dispara cuando se añade una nueva estructura al engine
    /// </summary>
    public class StructureAddedEventArgs : EventArgs
    {
        /// <summary>
        /// Estructura que fue añadida
        /// </summary>
        public StructureBase Structure { get; set; }

        /// <summary>
        /// Timeframe en minutos donde se añadió la estructura
        /// </summary>
        public int TimeframeMinutes { get; set; }

        /// <summary>
        /// Índice de barra donde se creó la estructura
        /// </summary>
        public int BarIndex { get; set; }

        /// <summary>
        /// Timestamp UTC del evento
        /// </summary>
        public DateTime EventTimeUTC { get; set; }

        /// <summary>
        /// Detector que creó la estructura
        /// </summary>
        public string CreatedByDetector { get; set; }

        public StructureAddedEventArgs(StructureBase structure, int tfMinutes, int barIndex, string detector)
        {
            Structure = structure ?? throw new ArgumentNullException(nameof(structure));
            TimeframeMinutes = tfMinutes;
            BarIndex = barIndex;
            EventTimeUTC = DateTime.UtcNow;
            CreatedByDetector = detector ?? "Unknown";
        }
    }

    /// <summary>
    /// Argumentos del evento OnStructureUpdated
    /// Se dispara cuando se actualiza una estructura existente
    /// </summary>
    public class StructureUpdatedEventArgs : EventArgs
    {
        /// <summary>
        /// Estructura que fue actualizada
        /// </summary>
        public StructureBase Structure { get; set; }

        /// <summary>
        /// Timeframe en minutos donde se actualizó la estructura
        /// </summary>
        public int TimeframeMinutes { get; set; }

        /// <summary>
        /// Índice de barra donde se actualizó la estructura
        /// </summary>
        public int BarIndex { get; set; }

        /// <summary>
        /// Timestamp UTC del evento
        /// </summary>
        public DateTime EventTimeUTC { get; set; }

        /// <summary>
        /// Tipo de actualización realizada
        /// Ejemplos: "ScoreUpdated", "TouchAdded", "Filled", "Confirmed", "Broken", "Mitigated"
        /// </summary>
        public string UpdateType { get; set; }

        /// <summary>
        /// Score anterior (antes de la actualización)
        /// </summary>
        public double? PreviousScore { get; set; }

        /// <summary>
        /// Score nuevo (después de la actualización)
        /// </summary>
        public double? NewScore { get; set; }

        public StructureUpdatedEventArgs(StructureBase structure, int tfMinutes, int barIndex, string updateType, double? prevScore = null, double? newScore = null)
        {
            Structure = structure ?? throw new ArgumentNullException(nameof(structure));
            TimeframeMinutes = tfMinutes;
            BarIndex = barIndex;
            EventTimeUTC = DateTime.UtcNow;
            UpdateType = updateType ?? "Unknown";
            PreviousScore = prevScore;
            NewScore = newScore;
        }
    }

    /// <summary>
    /// Argumentos del evento OnStructureRemoved
    /// Se dispara cuando se elimina una estructura del engine
    /// </summary>
    public class StructureRemovedEventArgs : EventArgs
    {
        /// <summary>
        /// ID de la estructura que fue eliminada
        /// </summary>
        public string StructureId { get; set; }

        /// <summary>
        /// Tipo de estructura eliminada (FVG, SWING, OB, etc.)
        /// </summary>
        public string StructureType { get; set; }

        /// <summary>
        /// Timeframe en minutos donde se eliminó la estructura
        /// </summary>
        public int TimeframeMinutes { get; set; }

        /// <summary>
        /// Índice de barra donde se eliminó la estructura
        /// </summary>
        public int BarIndex { get; set; }

        /// <summary>
        /// Timestamp UTC del evento
        /// </summary>
        public DateTime EventTimeUTC { get; set; }

        /// <summary>
        /// Razón de la eliminación
        /// Ejemplos: "Purged", "Invalidated", "Manual", "Expired"
        /// </summary>
        public string RemovalReason { get; set; }

        /// <summary>
        /// Score que tenía la estructura antes de ser eliminada
        /// </summary>
        public double LastScore { get; set; }

        public StructureRemovedEventArgs(string structureId, string structureType, int tfMinutes, int barIndex, string reason, double lastScore)
        {
            StructureId = structureId ?? throw new ArgumentNullException(nameof(structureId));
            StructureType = structureType ?? "Unknown";
            TimeframeMinutes = tfMinutes;
            BarIndex = barIndex;
            EventTimeUTC = DateTime.UtcNow;
            RemovalReason = reason ?? "Unknown";
            LastScore = lastScore;
        }
    }
}

