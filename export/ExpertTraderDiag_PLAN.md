# PLAN: ExpertTraderDiag.cs - Indicador de Diagnóstico Visual

## OBJETIVO
Crear una **copia** de `ExpertTrader.cs` llamada `ExpertTraderDiag.cs` que pinte en **AMARILLO** todas las señales rechazadas con sus SL/TP propuestos, permitiendo al usuario visualizar gráficamente qué zonas fueron filtradas y entender por qué.

## CARACTERÍSTICAS

### 1. **Heredar de ExpertTrader.cs**
- Copiar `ExpertTrader.cs` → `ExpertTraderDiag.cs`
- Cambiar `class ExpertTrader` → `class ExpertTraderDiag`
- Cambiar `Name = "ExpertTrader"` → `Name = "ExpertTraderDiag"`
- Cambiar `Description` para indicar que es versión diagnóstico

### 2. **Nueva Funcionalidad: Pintar Señales Rechazadas**

#### **Logs a Analizar:**
- `[RISK][REJECT_REASON]` → Zonas rechazadas por RiskCalculator
- `[DFM][REJECT]` → Zonas rechazadas por DecisionFusionModel
- `[TRADE][REGISTER_TOO_FAR]` → Zonas rechazadas por TradeManager (distancia)

#### **Datos a Extraer de Logs:**
- **Entry**: Precio de entrada propuesto
- **SL**: Stop Loss propuesto
- **TP**: Take Profit propuesto
- **RejectReason**: Motivo del rechazo
- **Bar**: Índice de barra donde se rechazó
- **ZoneId**: ID de la HeatZone

#### **Visualización:**
- **Línea de Entry**: Amarilla, sólida, grosor 2
- **Línea de SL**: Amarilla, punteada, grosor 1
- **Línea de TP**: Amarilla, punteada, grosor 1
- **Etiqueta**: Texto amarillo con `RejectReason` al lado del Entry
- **Flecha**: Flecha amarilla indicando dirección (BUY=arriba, SELL=abajo)

### 3. **Estructura de Código**

```csharp
// Nueva clase para almacenar señales rechazadas
private class RejectedSignal
{
    public string ZoneId { get; set; }
    public int BarIndex { get; set; }
    public string Action { get; set; } // "BUY" o "SELL"
    public double Entry { get; set; }
    public double SL { get; set; }
    public double TP { get; set; }
    public string RejectReason { get; set; }
}

// Lista de señales rechazadas
private List<RejectedSignal> _rejectedSignals = new List<RejectedSignal>();

// Parsear logs y extraer señales rechazadas
private void ParseRejectedSignalsFromLogs()
{
    // Leer el log actual
    // Buscar líneas con [RISK][REJECT_REASON] o [DFM][REJECT]
    // Extraer ZoneId, Entry, SL, TP, RejectReason
    // Añadir a _rejectedSignals
}

// Pintar señales rechazadas en el gráfico
private void DrawRejectedSignals()
{
    foreach (var signal in _rejectedSignals)
    {
        // Dibujar línea de Entry (amarilla, sólida)
        Draw.Line(this, $"RejEntry_{signal.ZoneId}", signal.BarIndex, signal.Entry, 
                  CurrentBar, signal.Entry, Brushes.Yellow);
        
        // Dibujar línea de SL (amarilla, punteada)
        Draw.Line(this, $"RejSL_{signal.ZoneId}", signal.BarIndex, signal.SL, 
                  CurrentBar, signal.SL, Brushes.Yellow, DashStyleHelper.Dot, 1);
        
        // Dibujar línea de TP (amarilla, punteada)
        Draw.Line(this, $"RejTP_{signal.ZoneId}", signal.BarIndex, signal.TP, 
                  CurrentBar, signal.TP, Brushes.Yellow, DashStyleHelper.Dot, 1);
        
        // Dibujar etiqueta con motivo de rechazo
        Draw.Text(this, $"RejReason_{signal.ZoneId}", signal.RejectReason, 
                  signal.BarIndex, signal.Entry, Brushes.Yellow);
        
        // Dibujar flecha indicando dirección
        if (signal.Action == "BUY")
            Draw.ArrowUp(this, $"RejArrow_{signal.ZoneId}", true, signal.BarIndex, signal.Entry, Brushes.Yellow);
        else
            Draw.ArrowDown(this, $"RejArrow_{signal.ZoneId}", true, signal.BarIndex, signal.Entry, Brushes.Yellow);
    }
}
```

### 4. **Integración en el Flujo**
- En `OnBarUpdate()` o `OnBarClose()`:
  - Llamar a `ParseRejectedSignalsFromLogs()` periódicamente (cada X barras)
  - Llamar a `DrawRejectedSignals()` después de pintar señales normales

## VENTAJAS
- **Visibilidad Completa**: El usuario verá TODAS las señales, no solo las ejecutadas
- **Diagnóstico Visual**: Motivos de rechazo directamente en el gráfico
- **Experiencia de Trader**: El usuario puede usar su experiencia visual para validar si los rechazos son correctos

## PRÓXIMOS PASOS
1. Copiar `ExpertTrader.cs` → `ExpertTraderDiag.cs`
2. Implementar parsing de logs de señales rechazadas
3. Implementar visualización en amarillo
4. Probar en NinjaTrader con un backtest

**¿Te parece bien este plan?**

