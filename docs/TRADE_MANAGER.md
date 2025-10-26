# TradeManager - Gestor Institucional de Órdenes

## 📋 Descripción

El `TradeManager` es un módulo de gestión de órdenes límite (Limit Orders) de nivel institucional para el sistema **PinkButterfly CoreBrain**. 

Gestiona el ciclo de vida completo de las órdenes generadas por el `DecisionEngine`, desde su creación como órdenes pendientes hasta su ejecución, cierre por SL/TP, o cancelación inteligente basada en invalidación estructural.

---

## 🎯 Responsabilidades

1. **Registro de Órdenes**: Almacena nuevas órdenes límite generadas por el `DecisionEngine`.
2. **Tracking de Ejecución**: Detecta cuándo el precio llega al nivel de entrada y marca la orden como ejecutada.
3. **Gestión de Salidas**: Detecta cuándo se alcanza el Stop Loss o Take Profit y cierra la operación.
4. **Caducidad Inteligente**: Cancela órdenes pendientes basándose en:
   - **Invalidación Estructural** (Regla 1 - Prioritaria)
   - **BOS/CHoCH Contradictorio** (Regla 2)
   - **Tiempo/Distancia Absoluta** (Regla 3 - Fail-safe)

---

## 🏗️ Arquitectura

### Ubicación
```
src/Execution/TradeManager.cs
```

### Clases Principales

#### `TradeStatus` (Enum)
Estados posibles de una orden:
- `PENDING`: Esperando que el precio llegue al Entry
- `EXECUTED`: Precio llegó al Entry, operación activa
- `CANCELLED`: Cancelada por invalidación estructural
- `SL_HIT`: Cerrada por Stop Loss
- `TP_HIT`: Cerrada por Take Profit

#### `TradeRecord` (Clase)
Registro completo de una orden límite:

```csharp
public class TradeRecord
{
    public string Id { get; set; }
    public int EntryBar { get; set; }           // Barra donde se generó la señal
    public double Entry { get; set; }
    public double SL { get; set; }
    public double TP { get; set; }
    public string Action { get; set; }          // "BUY" o "SELL"
    public TradeStatus Status { get; set; }
    public int ExecutionBar { get; set; }       // Barra donde se ejecutó
    public int ExitBar { get; set; }            // Barra donde se cerró/canceló
    public string ExitReason { get; set; }      // "SL", "TP", "BOS_CONTRARY", etc.
    public int TFDominante { get; set; }        // TF dominante de la HeatZone
    public string SourceStructureId { get; set; } // ID de la estructura que generó la orden
}
```

#### `TradeManager` (Clase Principal)
Gestor de órdenes:

```csharp
public class TradeManager
{
    public TradeManager(EngineConfig config, ILogger logger);
    
    // Registra una nueva orden límite
    public void RegisterTrade(string action, double entry, double sl, double tp, 
                             int entryBar, int tfDominante, string sourceStructureId);
    
    // Actualiza el estado de todas las órdenes en la barra actual
    public void UpdateTrades(double currentHigh, double currentLow, int currentBar, 
                            double currentPrice, CoreEngine coreEngine, IBarDataProvider barData);
    
    // Obtiene todas las órdenes (para visualización)
    public List<TradeRecord> GetAllTrades();
    
    // Obtiene solo las órdenes activas (PENDING o EXECUTED)
    public List<TradeRecord> GetActiveTrades();
}
```

---

## 🔄 Flujo de Trabajo

### 1. Registro de Orden (PENDING)
Cuando el `DecisionEngine` genera una señal BUY/SELL:

```csharp
_tradeManager.RegisterTrade(
    action: "BUY",
    entry: 6750.00,
    sl: 6720.00,
    tp: 6800.00,
    entryBar: 1234,
    tfDominante: 60,
    sourceStructureId: "FVG_abc123"
);
```

**Filtros aplicados:**
- ✅ Rechaza órdenes duplicadas (mismo Action, Entry, SL, TP)

**Estado inicial:** `PENDING`

---

### 2. Actualización en Cada Barra
En cada barra nueva, el `VisualLayerNinja` llama a:

```csharp
_tradeManager.UpdateTrades(
    currentHigh: High[0],
    currentLow: Low[0],
    currentBar: CurrentBar,
    currentPrice: Close[0],
    coreEngine: _coreEngine,
    barData: _barDataProvider
);
```

**Proceso interno:**

#### Para órdenes PENDING:
1. **Verificar Caducidad** (ver Reglas de Caducidad)
2. **Verificar Ejecución**: ¿El precio llegó al Entry?
   - BUY: `currentLow <= Entry`
   - SELL: `currentHigh >= Entry`
3. Si se ejecuta → Cambiar estado a `EXECUTED` y guardar `ExecutionBar`

#### Para órdenes EXECUTED:
1. **Verificar SL**: ¿El precio tocó el Stop Loss?
   - BUY: `currentLow <= SL`
   - SELL: `currentHigh >= SL`
2. **Verificar TP**: ¿El precio tocó el Take Profit?
   - BUY: `currentHigh >= TP`
   - SELL: `currentLow <= TP`
3. Si se cierra → Cambiar estado a `SL_HIT` o `TP_HIT` y guardar `ExitBar` y `ExitReason`

---

## 🛡️ Reglas de Caducidad Inteligente + Cooldown

El `TradeManager` implementa **3 reglas de caducidad** para órdenes PENDING, en orden de prioridad, más un sistema de **cooldown** para evitar ciclos infinitos de registro/cancelación:

### REGLA 1 (PRIORITARIA): Invalidación Estructural
**Objetivo:** Cancelar la orden si la estructura que la generó ya no es válida.

**Condiciones:**
- La estructura con ID `SourceStructureId` ya no existe en el `CoreEngine`
- La estructura está inactiva (`IsActive = false`)
- El `Score` de la estructura decayó por debajo de `MinConfidenceForWait` (default: 0.40)

**Razón:** Si la zona de entrada (FVG, OB, etc.) ya no es relevante, la orden no tiene sentido.

**ExitReason:** `"STRUCTURAL_INVALIDATION"`

**Ejemplo de log:**
```
[TradeManager] ❌ ORDEN EXPIRADA (Estructural): SELL @ 6812.25 | Razón: score decayó a 0.35
```

---

### REGLA 2: BOS/CHoCH Contradictorio
**Objetivo:** Cancelar la orden si el mercado rompió estructura en dirección opuesta.

**Condiciones:**
- Para BUY LIMIT: Se detectó un BOS/CHoCH bajista en el TF dominante
- Para SELL LIMIT: Se detectó un BOS/CHoCH alcista en el TF dominante

**Razón:** Un cambio de estructura contradice la hipótesis de la orden.

**ExitReason:** `"BOS_CONTRARY"`

**Ejemplo de log:**
```
[TradeManager] ❌ ORDEN CANCELADA por BOS contradictorio: BUY @ 6750.00
```

---

### REGLA 3 (FAIL-SAFE): Tiempo/Distancia Absoluta
**Objetivo:** Cancelar órdenes que llevan demasiado tiempo pendientes o están demasiado lejos del precio.

**Condiciones:**
- **Tiempo:** La orden lleva más de **100 barras** pendiente
- **Distancia:** El precio está a más de **30 ATR** del Entry

**Razón:** Evitar que órdenes antiguas o muy lejanas saturen el sistema.

**ExitReason:** `"EXPIRED_TIME"` o `"EXPIRED_DISTANCE"`

**Ejemplo de log:**
```
[TradeManager] ❌ ORDEN EXPIRADA (Tiempo): SELL @ 6812.25 | Barras esperando: 105
[TradeManager] ❌ ORDEN EXPIRADA (Distancia): BUY @ 6500.00 | Distancia: 350.50 > 30.00
```

---

### SISTEMA DE COOLDOWN (Prevención de Ciclos)

**Objetivo:** Evitar que la misma estructura genere órdenes repetitivas después de ser cancelada.

**Funcionamiento:**
1. Cuando una orden es **cancelada** por cualquiera de las 3 reglas, su `SourceStructureId` se añade a un diccionario de cooldown
2. La estructura queda "bloqueada" durante **N barras** (configurable, default: 25)
3. Si el `DecisionEngine` intenta generar una nueva orden de la misma estructura durante el cooldown, se rechaza automáticamente
4. Una vez expirado el cooldown, la estructura puede volver a generar órdenes

**Parámetro de configuración:**
- `EngineConfig.TradeCooldownBars` (default: 25)
  - Ejemplo: 25 barras de 15m = ~6 horas de cooldown

**Ventajas:**
- ✅ Previene ciclos infinitos de registro/cancelación
- ✅ Reduce el spam de logs
- ✅ Mejora la eficiencia del sistema
- ✅ Mantiene la separación de responsabilidades (DFM no necesita saber del cooldown)

**Ejemplo de log:**
```
[TradeManager] ❌ ORDEN CANCELADA por BOS contradictorio: BUY @ 6716,10
[TradeManager] 🕒 Estructura 3d2d0830-ac16-4343-88a2-bbbeec282942 añadida al cooldown hasta barra 5506 (25 barras)
[TradeManager] ⏳ Orden en COOLDOWN: BUY @ 6716,10 | Estructura=3d2d0830-ac16-4343-88a2-bbbeec282942 | Barras restantes: 18
[TradeManager] ✅ Cooldown EXPIRADO para estructura 3d2d0830-ac16-4343-88a2-bbbeec282942, orden permitida
```

---

## 🎨 Integración con VisualLayerNinja

El `VisualLayerNinja` consume el `TradeManager` para dibujar las órdenes en el gráfico:

### Órdenes EJECUTADAS/CERRADAS
- Se dibujan desde `ExecutionBar` hasta `ExitBar`
- Líneas sólidas con colores estándar (Entry, SL, TP)

### Órdenes PENDIENTES
- Se dibujan desde `CurrentBar` hacia el futuro (derecha del gráfico)
- **Colores:**
  - 🟢 **Verde (`Brushes.LimeGreen`)** para BUY LIMIT
  - 🔴 **Rojo (`Brushes.Red`)** para SELL LIMIT
- **Agrupación:** Órdenes con el mismo precio se agrupan con un contador (ej. "LIMIT (3x): 6750.00")
- **Escalonamiento:** Órdenes se dibujan en "abanico" hacia la derecha para evitar solapamiento

---

## 📊 Métricas y Logging

### Logs de Registro
```
[TradeManager] 🎯 ORDEN REGISTRADA: BUY LIMIT @ 6750.00 | SL=6720.00, TP=6800.00 | Bar=1234 | Estructura=FVG_abc123
```

### Logs de Ejecución
```
[TradeManager] ✅ ORDEN EJECUTADA: BUY @ 6750.00 en barra 1250
```

### Logs de Cierre
```
[TradeManager] 🟢 CERRADA POR TP: BUY @ 6750.00 en barra 1280
[TradeManager] 🔴 CERRADA POR SL: SELL @ 6812.25 en barra 1265
```

### Logs de Caducidad y Cooldown
```
[TradeManager] ❌ ORDEN EXPIRADA (Estructural): SELL @ 6812.25 | Razón: estructura no existe
[TradeManager] 🕒 Estructura abc123 añadida al cooldown hasta barra 5506 (25 barras)
[TradeManager] ❌ ORDEN CANCELADA por BOS contradictorio: BUY @ 6750.00
[TradeManager] 🕒 Estructura def456 añadida al cooldown hasta barra 5507 (25 barras)
[TradeManager] ⏳ Orden en COOLDOWN: BUY @ 6750.00 | Estructura=def456 | Barras restantes: 18
[TradeManager] ✅ Cooldown EXPIRADO para estructura def456, orden permitida
[TradeManager] ❌ ORDEN EXPIRADA (Tiempo): SELL @ 6812.25 | Barras esperando: 105
```

---

## 🧪 Testing

### Tests Manuales
1. **Registro de Orden**: Verificar que las órdenes se registran correctamente
2. **Ejecución**: Verificar que las órdenes se ejecutan cuando el precio llega al Entry
3. **SL/TP**: Verificar que las órdenes se cierran correctamente
4. **Caducidad Estructural**: Verificar que las órdenes se cancelan cuando la estructura decae
5. **Caducidad por BOS**: Verificar que las órdenes se cancelan con BOS contradictorio
6. **Caducidad por Tiempo/Distancia**: Verificar que las órdenes antiguas/lejanas se cancelan

### Tests Automatizados
*Pendiente de implementación*

---

## 🔧 Configuración

### Parámetros en `EngineConfig`
- `MinConfidenceForWait` (default: 0.40): Umbral mínimo de Score para mantener una orden pendiente
- `TradeCooldownBars` (default: 25): Número de barras de cooldown para estructuras canceladas

### Parámetros Hardcoded (TradeManager.cs)
- `maxBarsWaiting` (default: 100): Máximo de barras que una orden puede estar pendiente
- `maxAbsoluteDistance` (default: 30.0 * ATR): Máxima distancia absoluta del Entry al precio actual

---

## 📝 Notas de Diseño

### ¿Por qué 3 Reglas de Caducidad?
- **Regla 1 (Estructural)**: Es la más inteligente y profesional. Usa el conocimiento del `CoreEngine`.
- **Regla 2 (BOS)**: Es un filtro de contexto de mercado. Detecta cambios de dirección.
- **Regla 3 (Tiempo/Distancia)**: Es un fail-safe mecánico. Evita que órdenes antiguas saturen el sistema.

### ¿Por qué `SourceStructureId`?
Vincula la orden a la estructura que la generó. Esto permite:
- Cancelar la orden si la estructura decae
- Auditar qué estructuras generan las mejores operaciones
- Mejorar el sistema con feedback estructural

### ¿Por qué `ExecutionBar` y `ExitBar`?
Permite:
- Dibujar líneas históricas precisas en el gráfico
- Calcular métricas de duración de operaciones
- Auditar el comportamiento del sistema en backtesting

---

## 🚀 Próximas Mejoras

1. **Tests Automatizados**: Crear suite de tests para `TradeManager`
2. **Métricas de Performance**: Calcular win rate, profit factor, etc.
3. **Persistencia**: Guardar historial de órdenes en archivo JSON
4. **Configuración Dinámica**: Permitir ajustar `maxBarsWaiting` y `maxAbsoluteDistance` desde `EngineConfig`
5. **Trailing Stop**: Implementar trailing stop para órdenes ejecutadas
6. **Partial Take Profit**: Implementar cierre parcial en niveles intermedios

---

## 📚 Referencias

- **Fase 11**: Visual Integrator (Especificación original)
- **DecisionEngine**: Generador de señales de trading
- **VisualLayerNinja**: Capa visual de NinjaTrader
- **CoreEngine**: Motor de detección de estructuras

---

**Última actualización:** 2025-10-25  
**Versión:** 1.0  
**Autor:** PinkButterfly CoreBrain Team

