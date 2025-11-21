# INVENTARIO COMPLETO DE CAMBIOS - FASE 1 Y FASE 2

**Fecha:** 2025-11-21
**Fuente:** cursor_cloude_pinkbutterfly.md (líneas 90630-158469)

## ESTADO: ✅ INVENTARIO COMPLETO - LECTURA FINALIZADA

**Líneas analizadas:** 90630-158469 del archivo cursor_cloude_pinkbutterfly.md
**Total de líneas procesadas:** ~68,000 líneas de conversación
**Fecha de análisis:** 2025-11-21

---

## CAMBIOS DETECTADOS:

### ✅ FASE 1 - COMPLETADA (líneas 90680-105439)

**Aprobación:** Línea 90680: "Adelante! con la fase 1"
**Ejecución:** Línea 105264: "ejecuta la copia y hago los 2 test"
**Resultado:** Línea 105322: "FASE 1 COMPLETADA - ANÁLISIS DE RESULTADOS"

**ARCHIVOS MODIFICADOS:**

#### 1. `EngineConfig.cs` (2 cambios):
- Línea ~1176: `MaxStructuresByType_BOS` de `50` → `200`
- Línea ~653: Añadir `FreshnessNoDecayForUnbrokenSwings = true`

#### 2. `CoreEngine.cs` (3 anclas):
- **ANCLA 1** (línea ~2238): Calcular `freshness` = 1.0 si swing NO roto
- **ANCLA 2** (línea ~2471): Calcular `fresh` = 1.0 en HTF si swing NO roto
- **ANCLA 3** (línea ~2509): Calcular `fresh` = 1.0 en gate confluencia si swing NO roto

**VALIDACIÓN:**
- ✅ Determinismo: 100% (10 ops, WR 83.3%, P&L $728.75)
- ✅ Telemetría: 480,770 trazas `[FRESH_ADAPT]` con `freshness=1.00`

---

---

### ❌ FASE 2 - APLICADA Y LUEGO REVERTIDA (líneas 126030-158469)

**Inicio Fase 2:** Se añadieron 3 métodos adaptativos en CoreEngine.cs (no en el plan original)
**Problema:** Causó no-determinismo (línea 126063: "NO HAY DETERMINISMO")

**ARCHIVOS MODIFICADOS DURANTE FASE 2 (LUEGO REVERTIDOS):**

#### 1. `EngineConfig.cs` - Añadidos pero NO eran parte del plan Fase 1:
- `EnableAdaptiveFreshness = false`
- `FreshnessFloor = 0.05`
- `FreshnessMult_SwingUnbroken = 4.0`
- `FreshnessMult_BOS = 2.0`
- `FreshnessMult_OB = 0.9`
- `FreshnessMult_FVG_Intraday = 0.5`
- `FreshnessMult_FVG_HTF = 0.8`
- `FreshnessMult_Liquidity = 0.6`
- `FreshnessMult_POI = 1.2`
- NOTA: DecayBasePeriod_240m y 1440m NO se cambiaron (confirmado)

#### 2. `CoreEngine.cs` - 3 métodos nuevos (NO estaban en plan Fase 1):
- `GetEffectiveAgeBars()` (línea ~2704)
- `GetAdaptiveFreshnessLambda()` (línea ~2713)
- `GetAdaptiveFreshness()` (línea ~2736)
- Modificación de 3 anclas para usar estos métodos con flag `EnableAdaptiveFreshness`

#### 3. `RiskCalculator.cs` - 6 tie-breakers añadidos (línea 142125):
- RC-1: FindLiquidityTarget_Above
- RC-2: FindLiquidityTarget_Below
- RC-3: FindOpposingStructure_Above
- RC-4: FindOpposingStructure_Below
- RC-5: FindSwingHigh_Above
- RC-6: FindSwingLow_Below
- TAMBIÉN se añadió filtro `!s.IsBroken` para swings (Fase 2)

#### 4. `StructureFusion.cs` - Tie-breakers añadidos (línea 151517):
- SF-1: Ordenar `anchors`
- SF-2: Ordenar `supportingAnchors` y `nearbyTriggers`
- SF-3: Ordenar `HeatZones` en snapshot (REVERTIDO)
- SF-4: Priorizar frescura en listas auxiliares (REVERTIDO)

#### 5. `CoreEngine.cs` - Tie-breakers añadidos (línea 151517):
- CE-1: GetAllStructures ordenado determinísticamente (CAUSÓ DEGRADACIÓN)
- CE-2: GroupBy determinista en purga

**PROBLEMAS:**
- ✅ Determinismo parcialmente conseguido con tie-breakers
- ❌ Degradación brutal de resultados (WR 83.3% → 33.3%)
- ❌ Causa: GetAllStructures ordenado por Type primero (línea 151610)

**DECISIÓN (línea ~157000+):** REVERTIR TODOS LOS CAMBIOS DE FASE 2

---

## ✅ CONCLUSIÓN DEL INVENTARIO:

### CAMBIOS QUE DEBEN PERMANECER (Fase 1):
1. **EngineConfig.cs:**
   - `MaxStructuresByType_BOS = 200`
   - `FreshnessNoDecayForUnbrokenSwings = true`

2. **CoreEngine.cs (3 anclas con lógica simple if/else):**
   - ANCLA 1, 2, 3: `if` swing NO roto → `freshness = 1.0`

### CAMBIOS QUE DEBEN REVERTIRSE (Fase 2):
1. **EngineConfig.cs:** Eliminar todos los parámetros adaptativos añadidos
2. **CoreEngine.cs:** Eliminar 3 métodos adaptativos y revertir anclas a if/else
3. **RiskCalculator.cs:** Eliminar tie-breakers Y filtro `!s.IsBroken`
4. **StructureFusion.cs:** Eliminar todos los tie-breakers
5. **CoreEngine.cs:** Revertir GetAllStructures y GroupBy ordenado


