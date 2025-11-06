# 🔍 AUDITORÍA COMPLETA DE NO-DETERMINISMO
**PinkButterfly CoreBrain - Sistema de Trading**

**Fecha:** 2025-11-06  
**Rama:** `fix/determinismo-completo`  
**Objetivo:** Identificar y corregir TODAS las fuentes de no-determinismo que causan que backtests con el mismo código produzcan resultados diferentes.

---

## 📊 RESUMEN EJECUTIVO

### Evidencia del Problema
- **Backtest 1** (12:43, V6.0i.7): 27 operaciones cerradas, primera en 2025-08-22 03:45
- **Backtest 2** (18:09, V6.0i.7 revertido): 53 operaciones cerradas, primera en 2025-08-22 06:45
- **Resultado:** Operaciones COMPLETAMENTE DIFERENTES con el mismo código

### Causas Identificadas
1. **OrderBy sin desempates deterministas** (36 ubicaciones críticas)
2. **Enumeración de Dictionary.Values sin orden** (4 ubicaciones)
3. **Persistencia de estado entre ejecuciones** (`brain_state.json`)
4. **Anclaje temporal con fallback dependiente del timing**

---

## 🚨 CATEGORÍA P0: IMPACTO CRÍTICO EN DECISIONES DE TRADING

Estas fuentes afectan **DIRECTAMENTE** qué operaciones se generan.

### P0.1: `StructureFusion.cs` - Orden de procesamiento de Triggers
**Línea:** 110  
**Código actual:**
```csharp
triggers = triggers.OrderByDescending(s => s.Score).ToList();
```

**Problema:**  
- Si dos triggers tienen el mismo Score, el orden entre ellos es **NO determinista**
- Esto afecta qué trigger se procesa primero en la fusión jerárquica
- **Impacto:** Diferentes HeatZones → Diferentes operaciones

**Solución:**
```csharp
triggers = triggers
    .OrderByDescending(s => s.Score)
    .ThenByDescending(s => s.TF)              // Desempate 1: TF más alto
    .ThenBy(s => s.CreatedAtBarIndex)         // Desempate 2: más antiguo
    .ThenBy(s => s.Id)                        // Desempate 3: por ID
    .ToList();
```

**Prioridad:** 🔴 CRÍTICA  
**Test:** Verificar que con mismos triggers, se generen mismas HeatZones

---

### P0.2: `StructureFusion.cs` - Selección de estructura dominante
**Línea:** 407  
**Código actual:**
```csharp
var dominantStructure = structures.OrderByDescending(s => s.Score).First();
```

**Problema:**  
- Múltiples estructuras con mismo Score → selección no determinista
- **Impacto:** Diferente `DominantStructureId` → Diferentes metadatos de HeatZone → Diferentes decisiones

**Solución:**
```csharp
var dominantStructure = structures
    .OrderByDescending(s => s.Score)
    .ThenByDescending(s => s.TF)
    .ThenBy(s => s.CreatedAtBarIndex)
    .ThenBy(s => s.Id)
    .First();
```

**Prioridad:** 🔴 CRÍTICA

---

### P0.3: `StructureFusion.cs` - Selección de trigger dominante
**Líneas:** 310-320  
**Código actual:**
```csharp
var dominantTrigger = heatZone.SourceStructureIds
    .Select(id => coreEngine.GetStructure(id))
    .Where(s => s != null && isTrigger(s))
    .Select(t => new {
        Structure = t,
        Weight = t.Score * (_config.TFWeights.ContainsKey(t.TF) ? _config.TFWeights[t.TF] : 1.0),
        Age = ...
    })
    .OrderByDescending(x => x.Weight)      // Primero: mejor Score × TFWeight
    .ThenByDescending(x => x.Structure.TF) // Desempate: TF más alto
    .ThenBy(x => x.Age)                    // Desempate: más fresco
    .First();
```

**Problema:**  
- Si Weight empata y TF empata y Age empata → no determinista
- Falta desempate final por `Id`

**Solución:**
```csharp
    .OrderByDescending(x => x.Weight)
    .ThenByDescending(x => x.Structure.TF)
    .ThenBy(x => x.Age)
    .ThenBy(x => x.Structure.Id)          // ← AGREGAR
    .First();
```

**Prioridad:** 🔴 CRÍTICA

---

### P0.4: `RiskCalculator.cs` - Selección de TP (múltiples ubicaciones)

#### P0.4a: Líneas 976-980 (Buy, Primary TP)
**Código actual:**
```csharp
var primaryTPBuy = swingCandidatesBuy
    .Where(c => c.Item2 >= 60 && c.Item4 >= 1.0 && c.Item3 >= 6.0)
    .OrderByDescending(c => c.Item2) // TF alto primero
    .ThenBy(c => c.Item3)           // más cerca
    .FirstOrDefault();
```

**Problema:** Si TF y DistATR empatan → no determinista

**Solución:** Agregar `.ThenBy(c => c.Item6)` (createdAtBarIndex) como desempate final

#### P0.4b: Líneas 1011-1013 (Buy, Ordered candidates)
**Tiene desempates pero falta ID final**

**Solución:**
```csharp
.OrderByDescending(c => c.Item2)      // TF
.ThenBy(c => c.Item3)                 // DistATR
.ThenByDescending(c => c.Item4)       // RR
.ThenBy(c => c.Item6);                // CreatedAtBarIndex (AGREGAR)
```

**Prioridad:** 🔴 CRÍTICA  
**Nota:** Se repite para SELL en líneas 1281-1285, 1315-1317

---

### P0.5: `RiskCalculator.cs` - Selección de SL (múltiples ubicaciones)

#### P0.5a: Líneas 1486-1488 (Buy, In-band selection)
**Código actual:**
```csharp
var inBand = candidates.Where(c => c.Item3 >= bandMin && c.Item3 <= bandMax)
    .OrderBy(c => Math.Abs(c.Item3 - target))
    .ThenByDescending(c => c.Item3)
    .ToList();
```

**Problema:** Si distancia al target empata → no determinista

**Solución:**
```csharp
.OrderBy(c => Math.Abs(c.Item3 - target))
.ThenByDescending(c => c.Item3)
.ThenByDescending(c => c.Item2)        // TF (AGREGAR)
.ThenBy(c => c.Item1.CreatedAtBarIndex) // Edad (AGREGAR)
.ToList();
```

**Prioridad:** 🔴 CRÍTICA

---

### P0.6: `CoreEngine.cs` - Consultas de estructuras (múltiples métodos)

Métodos afectados:
- `GetRecentBreaks()` - Línea 653
- `GetActiveFVGs()` - Línea 785
- `GetActiveSwings()` - Línea 808
- `GetActiveDoubles()` - Línea 834
- `GetActiveOrderBlocks()` - Línea 875
- `GetStructureBreaks()` - Línea 908
- `GetActivePOIs()` - Línea 932
- `GetActiveLiquidityVoids()` - Línea 959
- `GetActiveLiquidityGrabs()` - Línea 986

**Problema común:** OrderBy solo por Score o Time sin desempates

**Ejemplo** (línea 785):
```csharp
return _structuresListByTF[tfMinutes]
    .OfType<FVGInfo>()
    .Where(f => f.IsActive && f.Score >= minScore)
    .OrderByDescending(f => f.Score)  // ← Sin desempate
    .ToList();
```

**Solución genérica:**
```csharp
.OrderByDescending(f => f.Score)
.ThenByDescending(f => f.TF)
.ThenBy(f => f.CreatedAtBarIndex)
.ThenBy(f => f.Id)
.ToList();
```

**Prioridad:** 🔴 CRÍTICA  
**Cantidad:** 9 métodos afectados

---

## 🟠 CATEGORÍA P1: IMPACTO ALTO EN ORDEN DE PROCESAMIENTO

### P1.1: `CoreEngine.cs` - Enumeración de Dictionary.Values

**Líneas:** 1598, 1616  
**Código actual:**
```csharp
foreach (var structure in _structuresById.Values)
{
    // Procesar estructura
}

var scores = _structuresById.Values.Select(s => s.Score).ToList();
```

**Problema:**  
- `Dictionary<>.Values` no garantiza orden
- Diferentes ejecuciones → diferente orden de procesamiento

**Solución:**
```csharp
foreach (var structure in _structuresById.Values
    .OrderBy(s => s.CreatedAtBarIndex)
    .ThenBy(s => s.Id))
{
    // Procesar estructura
}

var scores = _structuresById.Values
    .OrderBy(s => s.Id)  // Orden determinista
    .Select(s => s.Score)
    .ToList();
```

**Prioridad:** 🟠 ALTA  
**Impacto:** Afecta cálculo de estadísticas, puede afectar purga de estructuras

---

### P1.2: `CoreEngine.cs` - Purga de estructuras con score bajo

**Líneas:** 1736-1738, 1791-1793  
**Código actual:**
```csharp
var toRemove = structures
    .OrderBy(s => s.Score)  // ← Sin desempate
    .Take(countToPurge)
    .ToList();
```

**Problema:**  
- Si múltiples estructuras tienen mismo Score → cuáles se purgan es no determinista
- **Impacto:** Estado del CoreEngine diferente → decisiones posteriores diferentes

**Solución:**
```csharp
var toRemove = structures
    .OrderBy(s => s.Score)
    .ThenBy(s => s.CreatedAtBarIndex)    // Purgar las más antiguas primero
    .ThenBy(s => s.Id)
    .Take(countToPurge)
    .ToList();
```

**Prioridad:** 🟠 ALTA

---

## 🔵 CATEGORÍA P2: IMPACTO MEDIO - VISUALIZACIÓN Y MÉTRICAS

### P2.1: `ExpertTrader.cs` - Visualización de estructuras top

**Línea:** 910  
**Código actual:**
```csharp
var topStructures = allValidStructures
    .OrderByDescending(s => s.Score)  // ← Sin desempate
    .Take(3)
    .ToList();
```

**Impacto:** Solo afecta qué se dibuja en pantalla, no afecta decisiones

**Solución:**
```csharp
var topStructures = allValidStructures
    .OrderByDescending(s => s.Score)
    .ThenByDescending(s => s.TF)
    .ThenBy(s => s.CreatedAtBarIndex)
    .Take(3)
    .ToList();
```

**Prioridad:** 🔵 MEDIA

---

### P2.2: `ExpertTrader.cs` - Ordenar pending trades por distancia

**Líneas:** 995, 1228, 1333  
**Código actual:**
```csharp
var pendingTrades = allTrades
    .Where(t => t.Status == TradeStatus.PENDING)
    .OrderBy(t => Math.Abs(t.Entry - Close[0]))  // ← Sin desempate
    .ToList();
```

**Impacto:** Solo afecta orden de dibujo en pantalla

**Solución:**
```csharp
.OrderBy(t => Math.Abs(t.Entry - Close[0]))
.ThenBy(t => t.EntryBarTime)
.ThenBy(t => t.Id)
.ToList();
```

**Prioridad:** 🔵 MEDIA

---

## ⚡ CATEGORÍA P0+: FACTORES ESTRUCTURALES

### P0+.1: Persistencia de estado (`brain_state.json`)

**Archivo:** `EngineConfig.cs` línea 508  
**Código actual:**
```csharp
public bool AutoSaveEnabled { get; set; } = true;
```

**Problema:**  
- Estado guardado en backtest anterior puede cargarse en nuevo backtest
- Contamina el estado inicial con estructuras/contadores previos
- **Impacto:** Primer backtest ≠ Segundo backtest aunque código sea idéntico

**Solución para backtesting:**
```csharp
// Durante backtest, desactivar AutoSave
public bool AutoSaveEnabled { get; set; } = false;  // Cambiar default para BT
```

**O bien, en `ExpertTrader.cs` al iniciar backtest:**
```csharp
if (State == State.Historical)
{
    _config.AutoSaveEnabled = false;
    // Borrar archivo previo
    if (File.Exists(_config.StateFilePath))
        File.Delete(_config.StateFilePath);
}
```

**Prioridad:** 🔴 CRÍTICA  
**Debe validarse ANTES de cualquier backtest**

---

### P0+.2: Anclaje temporal con fallback por índices

**Archivo:** `ExpertTrader.cs` líneas 461-482  
**Problema:**  
- Anclaje depende de `Times[]` que puede variar según timing de inicialización
- Fallback usa índices absolutos en el momento exacto del anclaje
- **Impacto:** Ventana histórica diferente → Estructuras diferentes → Operaciones diferentes

**Solución propuesta:**
1. Eliminar dependencia de `Times[]`
2. Anclar siempre por índices desde el final:
   ```csharp
   // TF de decisión (ej: 15m)
   int countDecision = BarsArray[_decisionTFIndex].Count;
   int barsRequired = (_config.BacktestBarsForAnalysis * lowestTF) / decisionTF;
   int startIdxDecision = Math.Max(0, countDecision - barsRequired);
   int endIdxDecision = countDecision - 1;
   ```
3. Propagar por sincronización de barras entre TFs (sin depender de `FindBarIndexFromTime`)

**Prioridad:** 🔴 CRÍTICA  
**Requiere:** Refactorización más profunda del sistema de anclaje

---

## 📋 PLAN DE CORRECCIÓN PRIORIZADO

### FASE 1: Fixes P0 - Decisiones de Trading (INMEDIATO)
1. ✅ P0.1: `StructureFusion.cs` línea 110 (triggers)
2. ✅ P0.2: `StructureFusion.cs` línea 407 (dominantStructure)
3. ✅ P0.3: `StructureFusion.cs` líneas 310-320 (dominantTrigger)
4. ✅ P0.4: `RiskCalculator.cs` - Selección TP (6 ubicaciones)
5. ✅ P0.5: `RiskCalculator.cs` - Selección SL (4 ubicaciones)
6. ✅ P0.6: `CoreEngine.cs` - 9 métodos de consulta

**Total:** 24 fixes quirúrgicos

### FASE 2: Fixes P0+ - Factores Estructurales (CRÍTICO)
1. ✅ P0+.1: Desactivar AutoSave en backtests
2. ⚠️ P0+.2: Refactorizar anclaje temporal (REQUIERE DISEÑO)

### FASE 3: Fixes P1 - Orden de Procesamiento
1. ✅ P1.1: `CoreEngine.cs` - Dictionary.Values (2 ubicaciones)
2. ✅ P1.2: `CoreEngine.cs` - Purga de estructuras (2 ubicaciones)

### FASE 4: Fixes P2 - Visualización (OPCIONAL)
1. ⚪ P2.1: `ExpertTrader.cs` - Top structures
2. ⚪ P2.2: `ExpertTrader.cs` - Pending trades (3 ubicaciones)

---

## ✅ PROTOCOLO DE VALIDACIÓN

### Pre-test:
1. ✅ Desactivar AutoSave
2. ✅ Borrar `brain_state.json`
3. ✅ Reiniciar NinjaTrader (estado limpio)
4. ✅ Verificar `BacktestBarsForAnalysis = 5000`

### Test de Determinismo:
1. Ejecutar **Backtest 1** → Guardar `trades_test1.csv`
2. **Sin modificar código ni parámetros**, ejecutar **Backtest 2** → Guardar `trades_test2.csv`
3. Ejecutar **Backtest 3** → Guardar `trades_test3.csv`

### Validación:
```powershell
# Los 3 archivos deben ser IDÉNTICOS línea por línea
$csv1 = Import-Csv "trades_test1.csv"
$csv2 = Import-Csv "trades_test2.csv"
$csv3 = Import-Csv "trades_test3.csv"

# Comparar conteos
Write-Output "Test1: $($csv1.Count) líneas"
Write-Output "Test2: $($csv2.Count) líneas"
Write-Output "Test3: $($csv3.Count) líneas"

# Comparar primeras 5 operaciones cerradas
$closed1 = $csv1 | Where {$_.Action -eq "CLOSED"} | Sort {[datetime]$_.Status} | Select -First 5
$closed2 = $csv2 | Where {$_.Action -eq "CLOSED"} | Sort {[datetime]$_.Status} | Select -First 5
$closed3 = $csv3 | Where {$_.Action -eq "CLOSED"} | Sort {[datetime]$_.Status} | Select -First 5

# DEBEN SER IDÉNTICOS
Compare-Object $closed1 $closed2 -Property TradeID,Status,Direction,Entry,SL,TP
Compare-Object $closed1 $closed3 -Property TradeID,Status,Direction,Entry,SL,TP
```

**Criterio de éxito:**
- ✅ `Compare-Object` no debe devolver diferencias
- ✅ Mismas operaciones, mismas horas, mismos precios
- ✅ Mismo P&L, mismo Win Rate, mismo Profit Factor

---

## 📊 RESUMEN DE IMPACTO

| Categoría | Cantidad | Impacto | Esfuerzo |
|-----------|----------|---------|----------|
| **P0** | 24 fixes | 🔴 CRÍTICO | Bajo (agregar ThenBy) |
| **P0+** | 2 fixes | 🔴 CRÍTICO | Medio (P0+.1) / Alto (P0+.2) |
| **P1** | 4 fixes | 🟠 ALTO | Bajo |
| **P2** | 4 fixes | 🔵 MEDIO | Bajo |
| **TOTAL** | 34 fixes | - | ~2-4 horas |

---

## 🎯 SIGUIENTE PASO

**Esperando aprobación del usuario para:**
1. Aplicar fixes P0 (24 cambios quirúrgicos)
2. Aplicar fix P0+.1 (desactivar AutoSave)
3. Diseñar solución para P0+.2 (anclaje temporal)

**Todos los cambios se harán en la rama `fix/determinismo-completo`**

---

## 🔬 HALLAZGOS ADICIONALES (AUDITORÍA CIENTÍFICA EXTENDIDA)

### ⚡ FOCOS CRÍTICOS NO CONTEMPLADOS INICIALMENTE

#### 1. Estado Estático y Cachés
**Prioridad:** 🔴 P0+  
**Búsqueda:** `static` en detectores/engine/managers

**Riesgo:**  
- Colecciones/contadores estáticos persisten entre ejecuciones
- Contaminan el siguiente run incluso si borras `brain_state.json`

**Acción:**
- Auditar todos los `static` en:
  - Detectores (Swing, FVG, OB, LG, LV, POI, BOS)
  - CoreEngine
  - Managers (Trade, Context, Persistence)
- Limpiar/rehidratar en `State.DataLoaded` o eliminar si no son necesarios

**Resultado auditoría:**
✅ No se encontraron colecciones estáticas compartidas en core de decisiones  
✅ Solo singletons y métodos estáticos utilitarios → Riesgo bajo

---

#### 2. Generación de IDs y Desempates
**Prioridad:** 🔴 P0  
**Problema:**  
- Si `Id = Guid.NewGuid()` en momento de creación, usarlo como desempate puede fijar una decisión a un orden de creación no determinista

**Solución:**
- Mantener `Id` como **último recurso** en desempates
- Preferir desempates con claves deterministas:
  ```
  Score desc → TF desc → CreatedAtBarIndex asc → StartTime asc → Id asc
  ```
- Si es posible, derivar `Id` determinísticamente (hash de propiedades inmutables)

**Ubicaciones con Guid.NewGuid():**
- `StructureFusion.cs`: líneas 221, 354
- Todos los detectores (Swing, FVG, OB, etc.)

**Acción:** ✅ Ya contemplado en solución (Id como último desempate)

---

#### 3. Cultura/Zonas Horarias y NaN en Ordenaciones
**Prioridad:** 🟠 P1  

**3a. UTC e InvariantCulture**
**Acción:**
- Forzar `DateTimeKind.Utc` para todos los tiempos
- Usar `CultureInfo.InvariantCulture` en parse/format (ya se cumple en logs)
- Validar en serialización JSON

**Resultado auditoría:**
✅ No hay usos explícitos problemáticos de CultureInfo  
✅ JSON/serialización estable  
⚠️ Confirmar: todo tiempo en UTC (ya se cumple en datos de engine)

**3b. NaN en Ordenaciones**
**Problema:**  
- Ordenaciones por `double` (Score/DistATR) no definen dónde cae `NaN`

**Solución:**
```csharp
// Antes de ordenar por Score/DistATR:
var validItems = items.Where(x => !double.IsNaN(x.Score)).ToList();

// Luego ordenar normalmente
validItems.OrderByDescending(x => x.Score)...
```

**Acción:** Agregar filtros anti-NaN en ordenaciones críticas (P0)

---

#### 4. Paralelismo/Asincronía
**Prioridad:** 🔵 P2 (bajo riesgo actual)  

**Búsqueda:** `Task`, `Parallel.ForEach`, `AsParallel`, timers/eventos

**Resultado auditoría:**
✅ No hay `Parallel.ForEach/AsParallel` en core de decisiones  
✅ `Task.Run` solo en SaveState y Diagnostics (reporting) → No afecta decisiones  
✅ Sin timers/eventos en pipeline de decisión

**Conclusión:** Riesgo bajo actual, sin acción necesaria

---

#### 5. Anclaje por Índices (ELEVAR PRIORIDAD)
**Prioridad:** 🔴 P0+ → **INMEDIATA**  
**Justificación:** Explica la deriva al pasar de 5000 a 20000 barras

**Propuesta actualizada:**
1. Eliminar dependencia de `Times[]` completamente
2. Si `Times[]` no cuadra → **LOG y ABORTAR** (no usar fallback)
3. Anclar **SIEMPRE** por índices desde el final
4. Fijar TF base explícitamente al más bajo (5m)

**Acción:** Ejecutar INMEDIATAMENTE después de P0 fixes

---

#### 6. Persistencia - Protocolo Riguroso
**Prioridad:** 🔴 P0+  

**Acción reforzada:**
```csharp
// En ExpertTrader.cs, inicio de backtest
if (State == State.Historical)
{
    // CRÍTICO: Desactivar AutoSave Y Fast Load
    _config.AutoSaveEnabled = false;
    _config.ValidateConfigHashOnLoad = true;
    
    // Borrar archivo ANTES de inicializar engine
    if (File.Exists(_config.StateFilePath))
    {
        File.Delete(_config.StateFilePath);
        _logger.Info("[BT] STATE_DISABLED_FOR_BT=true, archivo borrado");
    }
    
    // Si por alguna razón se carga estado en BT, ABORTAR
    if (coreEngine.IsStateLoaded && coreEngine.StateConfigHash != _config.GetHash())
    {
        throw new InvalidOperationException("Estado con hash diferente detectado en BT - ABORTANDO");
    }
}
```

**Nota adicional para indicador (ExpertTrader.cs):**
```csharp
[NinjaScriptProperty]
[Display(Name = "Enable Fast Load (Solo DFM)", ...)]
public bool EnableFastLoad { get; set; }

// En Configure() o State.SetDefaults:
if (State == State.SetDefaults)
{
    EnableFastLoad = false;  // ← FORZAR false en backtesting
}

// Al iniciar backtest, validar:
if (State == State.Historical && EnableFastLoad)
{
    _logger.Warning("[BT] Fast Load desactivado forzosamente para determinismo");
    EnableFastLoad = false;
}
```

---

### 📍 UBICACIONES ADICIONALES ENCONTRADAS

#### P0: OrderBy sin desempates NO listados inicialmente

**ProximityAnalyzer.cs línea 117:**
```csharp
// ACTUAL:
.OrderByDescending(z => z.ProximityFactor)

// PROPUESTO:
.OrderByDescending(z => z.ProximityFactor)
.ThenByDescending(z => z.HeatZone.TFDominante)
.ThenBy(z => z.HeatZone.Metadata.ContainsKey("CreatedAtBar") ? (int)z.HeatZone.Metadata["CreatedAtBar"] : 0)
.ThenBy(z => z.HeatZone.Id)
```

**DoubleDetector.cs línea 74:**
```csharp
// ACTUAL:
.OrderByDescending(s => s.CreatedAtBarIndex)

// PROPUESTO:
.OrderByDescending(s => s.CreatedAtBarIndex)
.ThenBy(s => s.Id)
```

**RiskCalculator.cs - Ubicaciones adicionales:**
- Línea 1795: `OrderBy` en loop de iteración
- Línea 1832: `OrderBy` en candidatos
- Línea 1926: `OrderBy` en swings
- Línea 1986: `OrderBy` en TP candidates
- Línea 2032: `OrderBy` en SL candidates
- Línea 2118: `OrderByDescending` en liquidities
- Línea 2178: `OrderByDescending` en estructuras opuestas
- Línea 2224: `OrderBy` en candidatos finales

**Total adicional:** +10 ubicaciones

---

## 📊 RESUMEN ACTUALIZADO

| Categoría | Cantidad Original | Adicionales | Total | Prioridad |
|-----------|-------------------|-------------|-------|-----------|
| **P0: Decisiones** | 24 | +10 | 34 | 🔴 INMEDIATA |
| **P0+: Estructurales** | 2 | +4 | 6 | 🔴 INMEDIATA |
| **P1: Procesamiento** | 4 | +1 | 5 | 🟠 ALTA |
| **P2: Visualización** | 4 | +0 | 4 | 🔵 MEDIA |
| **TOTAL** | 34 | +15 | **49 fixes** | - |

---

## ✅ REGLA GLOBAL DE DESEMPATES (CIENTÍFICA)

### Plantilla Universal de Ordenación

**Para TODOS los `OrderBy` en código de decisión:**

```csharp
// PLANTILLA UNIVERSAL:
.OrderBy[Descending](x => ClavePrincipal)          // Score, Time, Distance, etc.
.ThenByDescending(x => x.TF)                       // TF más alto primero
.ThenBy(x => x.CreatedAtBarIndex)                  // Más antiguo primero
.ThenBy(x => x.StartTime)                          // Si aplica (tiempo de inicio)
.ThenBy(x => x.Id, StringComparer.Ordinal)         // Último recurso (Guid) - COMPARADOR EXPLÍCITO
```

**Excepción:** Si ordenas por algo ya determinista (ej: `EntryBarTime`), puedes omitir desempates intermedios, pero **SIEMPRE** terminar en `.ThenBy(x => x.Id, StringComparer.Ordinal)`

---

### Regla Crítica: Comparadores de String Explícitos

**Problema:** Ordenaciones por `string` sin comparador explícito pueden variar según cultura/locale del sistema.

**Solución:** Usar **SIEMPRE** `StringComparer.Ordinal` para claves string deterministas.

```csharp
// ❌ MAL - Dependiente de cultura:
.OrderBy(x => x.Id)
.ThenBy(x => x.Name)

// ✅ BIEN - Ordinal explícito:
.OrderBy(x => x.Id, StringComparer.Ordinal)
.ThenBy(x => x.Name, StringComparer.Ordinal)
```

**Aplicar en:**
- Ordenaciones por `Id` (Guid.ToString())
- Ordenaciones por `Type` (nombre de tipo)
- Ordenaciones por cualquier string generado
- Comparaciones en `Dictionary<string, ...>`, `HashSet<string>`

**Ejemplo completo:**
```csharp
// Ordenación determinista completa con strings:
var sorted = structures
    .OrderByDescending(s => s.Score)                           // 1. Score (double)
    .ThenByDescending(s => s.TF)                               // 2. TF (int)
    .ThenBy(s => s.CreatedAtBarIndex)                          // 3. Edad (int)
    .ThenBy(s => s.Type, StringComparer.Ordinal)               // 4. Tipo (string) ← COMPARADOR
    .ThenBy(s => s.Id, StringComparer.Ordinal)                 // 5. Id (string) ← COMPARADOR
    .ToList();
```

**Para colecciones:**
```csharp
// Dictionary con comparador Ordinal:
var dict = new Dictionary<string, StructureBase>(StringComparer.Ordinal);

// HashSet con comparador Ordinal:
var set = new HashSet<string>(StringComparer.Ordinal);
```

---

### Reglas Adicionales Críticas

#### 1. Orden Tras Operaciones Set-Like

**Operaciones que rompen orden:** `Distinct()`, `Union()`, `Concat()`, `GroupBy().SelectMany(...)`

**Regla:** Después de estas operaciones, **SIEMPRE** imponer orden total antes de `First()`, `Take()`, o cualquier consumo secuencial.

```csharp
// ❌ MAL:
var items = list1.Union(list2).First();

// ✅ BIEN:
var items = list1.Union(list2)
    .OrderBy(x => x.CreatedAtBarIndex)
    .ThenBy(x => x.Id)
    .First();
```

**Ejemplos críticos:**
```csharp
// Concat:
var all = triggers.Concat(anchors)
    .OrderByDescending(s => s.Score)  // ← OBLIGATORIO después de Concat
    .ThenByDescending(s => s.TF)
    .ThenBy(s => s.CreatedAtBarIndex)
    .ThenBy(s => s.Id)
    .ToList();

// GroupBy + SelectMany:
var flattened = items
    .GroupBy(x => x.Type)
    .SelectMany(g => g)
    .OrderBy(x => x.CreatedAtBarIndex)  // ← OBLIGATORIO después de SelectMany
    .ThenBy(x => x.Id)
    .ToList();

// Distinct:
var unique = items
    .Distinct()
    .OrderBy(x => x.CreatedAtBarIndex)  // ← OBLIGATORIO después de Distinct
    .ThenBy(x => x.Id)
    .ToList();
```

---

#### 2. First/Last Sin Orden Previo

**Regla:** NUNCA usar `First()`, `FirstOrDefault()`, `Last()`, `LastOrDefault()` sobre fuentes sin orden explícito.

**Auditoría necesaria:**
```regex
// Buscar patrones peligrosos:
\.First\(\)(?!.*OrderBy)
\.FirstOrDefault\(\)(?!.*OrderBy)
\.Last\(\)(?!.*OrderBy)
```

**Ejemplos:**
```csharp
// ❌ MAL - First sobre Dictionary.Values:
var first = _structuresById.Values.First();

// ✅ BIEN:
var first = _structuresById.Values
    .OrderBy(s => s.CreatedAtBarIndex)
    .ThenBy(s => s.Id)
    .First();

// ❌ MAL - First sobre lista sin orden:
var best = candidates.First();

// ✅ BIEN:
var best = candidates
    .OrderByDescending(c => c.Score)
    .ThenBy(c => c.TF)
    .ThenBy(c => c.Id)
    .First();
```

**Acción:** Barrido de todos los `.First(` / `.Last(` en archivos de decisión

---

#### 3. Guardia Automática en Build

**Objetivo:** Prevenir regresiones futuras detectando patrones no deterministas en tiempo de compilación.

**Script de validación** (`tools/validate-determinism.ps1`):

```powershell
# Validar que OrderBy tenga desempates
$violations = @()

# 1. OrderBy sin ThenBy
$orderByNoThen = Select-String -Path "pinkbutterfly-produccion\*.cs" `
    -Pattern '\.OrderBy(Descending)?\([^)]+\)(?!\s*\.Then)' `
    -AllMatches

foreach ($match in $orderByNoThen) {
    # Excepciones: OrderBy en comentarios, strings, o ya con múltiples ThenBy
    if ($match.Line -notmatch '^\s*//' -and $match.Line -notmatch '".*OrderBy') {
        $violations += "WARN: OrderBy sin ThenBy en $($match.Filename):$($match.LineNumber)"
    }
}

# 2. Dictionary.Values sin OrderBy posterior
$dictValues = Select-String -Path "pinkbutterfly-produccion\*.cs" `
    -Pattern '\.Values(?!\s*\.OrderBy)' `
    -Context 0,2

foreach ($match in $dictValues) {
    if ($match.Context.PostContext -notmatch 'OrderBy' -and 
        $match.Line -match '_structures.*\.Values') {
        $violations += "WARN: Dictionary.Values sin OrderBy en $($match.Filename):$($match.LineNumber)"
    }
}

# 3. First/Last sin OrderBy previo en mismo statement
$firstNoOrder = Select-String -Path "pinkbutterfly-produccion\*.cs" `
    -Pattern '\.(First|Last)(OrDefault)?\(\)' `
    -Context 2,0

foreach ($match in $firstNoOrder) {
    if ($match.Context.PreContext -notmatch 'OrderBy' -and
        $match.Line -notmatch '^\s*//') {
        $violations += "WARN: First/Last sin OrderBy en $($match.Filename):$($match.LineNumber)"
    }
}

# Resultado
if ($violations.Count -gt 0) {
    Write-Output "❌ FALLOS DE DETERMINISMO DETECTADOS:"
    $violations | ForEach-Object { Write-Output "  $_" }
    exit 1
} else {
    Write-Output "✅ Validación de determinismo: PASS"
    exit 0
}
```

**Integración en build:**
```xml
<!-- En NinjaTrader .csproj (si es posible) -->
<Target Name="ValidateDeterminism" BeforeTargets="CoreCompile">
  <Exec Command="powershell -File tools/validate-determinism.ps1" />
</Target>
```

**O manualmente antes de cada commit:**
```bash
# En pre-commit hook
.\tools\validate-determinism.ps1
if ($LASTEXITCODE -ne 0) {
    Write-Error "Commit bloqueado: violaciones de determinismo detectadas"
    exit 1
}
```

**Acción:** Crear script y documentar su uso

---

## 🎯 PLAN DE CORRECCIÓN ACTUALIZADO

### FASE 2A: Fixes P0 - Decisiones (APROBADO)
**Esfuerzo:** 2 horas  
**Commits:** 5 grupos temáticos

1. ✅ **StructureFusion.cs** (3 fixes)
   - Línea 110: triggers
   - Línea 407: dominantStructure
   - Línea 318: dominantTrigger

2. ✅ **CoreEngine.cs** (9 fixes)
   - GetRecentBreaks, GetActiveFVGs, GetActiveSwings, etc.

3. ✅ **RiskCalculator.cs** (20 fixes)
   - TP selection (buy/sell): 10 ubicaciones
   - SL selection (buy/sell): 10 ubicaciones

4. ✅ **ProximityAnalyzer.cs** (1 fix)
   - Línea 117: ordenación por ProximityFactor

5. ✅ **DoubleDetector.cs** (1 fix)
   - Línea 74: ordenación por CreatedAtBarIndex

**Total:** 34 fixes quirúrgicos

---

### FASE 2B: Fixes P0+ - Estructurales (APROBADO)
**Esfuerzo:** 30 min  
**Commits:** 2 individuales

1. ✅ **Persistencia rigurosa** (`ExpertTrader.cs`, `EngineConfig.cs`)
   - Desactivar AutoSave en BT
   - Borrar brain_state.json al inicio
   - Validar hash si se carga
   - LOG "STATE_DISABLED_FOR_BT=true"

2. ⏸️ **Anclaje por índices** (`ExpertTrader.cs`)
   - **POSTERGAR** hasta después de validar P0 fixes
   - Requiere diseño más profundo
   - Prioridad INMEDIATA pero secuencial

---

### FASE 2C: Fixes P1 - Procesamiento (OPCIONAL POST-VALIDACIÓN)
**Esfuerzo:** 15 min

1. ✅ Dictionary.Values ordenado (CoreEngine.cs)
2. ✅ Purga determinista (CoreEngine.cs)
3. ✅ Anti-NaN filters en ordenaciones

---

### FASE 3: VALIDACIÓN CIENTÍFICA
**Protocolo riguroso:**

1. **Pre-test (CRÍTICO - VERIFICAR ANTES DE CADA RUN):**
   ```powershell
   # 1. Borrar estado persistente
   Remove-Item "C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\data\brain_state.json" -ErrorAction SilentlyContinue
   
   # 2. Verificar configuración en código
   Write-Output "✅ Verificar en EngineConfig.cs:"
   Write-Output "   - AutoSaveEnabled = false"
   Write-Output "   - BacktestBarsForAnalysis = 5000"
   
   Write-Output "✅ Verificar en ExpertTrader.cs (parámetros del indicador):"
   Write-Output "   - EnableFastLoad = false  ← CRÍTICO"
   
   # 3. Reiniciar NinjaTrader
   Write-Output "⚠️ REINICIAR NinjaTrader para limpiar memoria"
   ```

   **Checklist visual antes de ejecutar:**
   - [ ] `brain_state.json` borrado
   - [ ] `AutoSaveEnabled = false` (EngineConfig.cs línea 508)
   - [ ] `EnableFastLoad = false` (Parámetro del indicador en gráfico)
   - [ ] `BacktestBarsForAnalysis = 5000` (EngineConfig.cs línea 681)
   - [ ] NinjaTrader reiniciado

2. **Test 3x (SIN MODIFICAR NADA ENTRE RUNS):**
   - Ejecutar **Backtest 1** → Guardar CSV como `test1.csv`
   - Sin cerrar/abrir gráfico, ejecutar **Backtest 2** → Guardar CSV como `test2.csv`
   - Sin cerrar/abrir gráfico, ejecutar **Backtest 3** → Guardar CSV como `test3.csv`

3. **Validación (Criterio: Hashes SHA256 idénticos):**
   ```powershell
   # Comparar hashes byte-por-byte
   $hash1 = Get-FileHash "C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\test1.csv" -Algorithm SHA256
   $hash2 = Get-FileHash "C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\test2.csv" -Algorithm SHA256
   $hash3 = Get-FileHash "C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\test3.csv" -Algorithm SHA256
   
   Write-Output "Hash Test1: $($hash1.Hash)"
   Write-Output "Hash Test2: $($hash2.Hash)"
   Write-Output "Hash Test3: $($hash3.Hash)"
   
   if ($hash1.Hash -eq $hash2.Hash -and $hash2.Hash -eq $hash3.Hash) {
       Write-Output ""
       Write-Output "✅✅✅ DETERMINISMO CONFIRMADO ✅✅✅"
       Write-Output "Los 3 backtests son BYTE-POR-BYTE idénticos"
   } else {
       Write-Output ""
       Write-Output "❌❌❌ PERSISTE NO-DETERMINISMO ❌❌❌"
       Write-Output "Los archivos difieren - investigar causa"
       
       # Comparación detallada
       if ($hash1.Hash -ne $hash2.Hash) { Write-Output "  Test1 ≠ Test2" }
       if ($hash1.Hash -ne $hash3.Hash) { Write-Output "  Test1 ≠ Test3" }
       if ($hash2.Hash -ne $hash3.Hash) { Write-Output "  Test2 ≠ Test3" }
   }
   ```

4. **Validación adicional (Primeras operaciones):**
   ```powershell
   # Comparar las primeras 5 operaciones cerradas
   $csv1 = Import-Csv "test1.csv"
   $csv2 = Import-Csv "test2.csv"
   $csv3 = Import-Csv "test3.csv"
   
   $closed1 = $csv1 | Where-Object {$_.Action -eq "CLOSED"} | Select-Object -First 5
   $closed2 = $csv2 | Where-Object {$_.Action -eq "CLOSED"} | Select-Object -First 5
   $closed3 = $csv3 | Where-Object {$_.Action -eq "CLOSED"} | Select-Object -First 5
   
   Write-Output ""
   Write-Output "Primeras 5 operaciones Test1:"
   $closed1 | ForEach-Object { Write-Output "  $($_.TradeID) | $($_.Status) | $($_.Direction) @ $($_.Entry)" }
   
   Write-Output ""
   Write-Output "Primeras 5 operaciones Test2:"
   $closed2 | ForEach-Object { Write-Output "  $($_.TradeID) | $($_.Status) | $($_.Direction) @ $($_.Entry)" }
   
   Write-Output ""
   Write-Output "Primeras 5 operaciones Test3:"
   $closed3 | ForEach-Object { Write-Output "  $($_.TradeID) | $($_.Status) | $($_.Direction) @ $($_.Entry)" }
   ```

---

*Auditoría generada y actualizada con hallazgos científicos adicionales - 2025-11-06*
*Rama: fix/determinismo-completo*

