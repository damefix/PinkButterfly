# ISSUES DE CÓDIGO IDENTIFICADOS - FASE 2.5
**Fecha**: 2025-10-30  
**Estado**: PENDIENTES DE FIX

Estos son los bugs identificados durante la auditoría profesional de tests que requieren corrección en el código de producción.

---

## **🐛 ISSUE #1: RiskCalculator usa 3.0 ATR hardcoded en lugar de config.SL_BufferATR**

**Archivo**: `src/Decision/RiskCalculator.cs`  
**Línea**: 480 (aprox)  
**Severidad**: ⚠️ MEDIA  
**Impacto**: Pérdida de configurabilidad

### **Problema:**
Cuando no hay estructuras (coreEngine=null o no encuentra Swing Low protector), el código usa un SL mínimo de seguridad **hardcoded** de `3.0 ATR`:

```csharp
double minSL = entry - (3.0 * atr); // SL mínimo de seguridad
```

Esto ignora el parámetro configurable `config.SL_BufferATR` (default 0.2).

### **Comportamiento Esperado:**
Cuando no hay estructuras, el SL debe calcularse como:
```csharp
double fallbackSL = entry - (config.SL_BufferATR * atr);
```

Con guardarraíles:
- Aplicar snapping a tick
- Respetar `MaxSLDistanceATR`
- Rechazar si viola `MinSLDistanceATR` o `MinRiskRewardRatio`

### **Evidencia:**
- Test: `Test_RiskCalculator_SL_WithBuffer` - Actualmente marcado como XFAIL
- Usuario confirmó: "honrar el parámetro de configuración"

### **Fix Propuesto:**
```csharp
// En CalculateStructuralSL_Buy (línea 477):
double entry = zone.Low;

if (swingLow != null)
{
    // SL estructural existente (código actual, correcto)
    double structuralSL = swingLow.Low - (_config.SL_BufferATR * atr);
    double finalSL = Math.Min(structuralSL, entry - (3.0 * atr)); // Límite de seguridad
    return finalSL;
}
else
{
    // FALLBACK: Usar SL_BufferATR configurable con guardarraíles
    double fallbackSL = entry - (_config.SL_BufferATR * atr);
    
    // Guardarraíl: Mínimo absoluto de seguridad
    double minAbsoluteSL = entry - (3.0 * atr);
    fallbackSL = Math.Min(fallbackSL, minAbsoluteSL);
    
    // Guardarraíl: Máximo permitido
    double maxSL = entry - (_config.MaxSLDistanceATR * atr);
    if (fallbackSL < maxSL)
    {
        zone.Metadata["RiskCalculated"] = false;
        zone.Metadata["RejectReason"] = "SL fallback excede MaxSLDistanceATR";
        return 0;
    }
    
    return fallbackSL;
}
```

---

## **🐛 ISSUE #2: RiskCalculator permite TP<SL (R:R negativo) cuando no hay estructuras**

**Archivo**: `src/Decision/RiskCalculator.cs`  
**Línea**: 581 (CalculateStructuralTP_Buy con coreEngine=null)  
**Severidad**: 🔴 CRÍTICA  
**Impacto**: Operaciones inválidas

### **Problema:**
Cuando `coreEngine=null`, `CalculateStructuralTP_Buy` retorna `0`:

```csharp
private double CalculateStructuralTP_Buy(...)
{
    if (coreEngine == null)
        return 0; // ❌ Esto causa TP inválido
    ...
}
```

Esto resulta en:
- TP = 0
- SL = 4940
- Entry = 5000
- R:R = (0-5000)/(5000-4940) = **-83.33** 🚨

### **Comportamiento Esperado:**
Si no hay estructuras válidas para TP, el sistema debe:
1. Calcular TP fallback: `TP = entry + (riskDistance * MinRiskRewardRatio)`
2. Si TP fallback es incoherente (TP<entry para BUY), **RECHAZAR la operación** por R:R inválido

### **Evidencia:**
- Test: `Test_RiskCalculator_TP_RiskReward` - Actualmente falla con R:R = -1666.67
- Usuario confirmó: "garantizar TP coherente con la dirección y MinRiskRewardRatio (nunca producir TP<SL o R:R negativo)"

### **Fix Propuesto:**
```csharp
private double CalculateStructuralTP_Buy(...)
{
    if (coreEngine == null)
    {
        // FALLBACK: TP basado en R:R mínimo
        double riskDistance = entry - stopLoss;
        double fallbackTP = entry + (riskDistance * _config.MinRiskRewardRatio);
        
        _logger.Warning($"[RiskCalculator] No hay estructuras para TP (BUY), usando fallback: {fallbackTP:F2}");
        
        // VALIDACIÓN: TP debe ser coherente con dirección
        if (fallbackTP <= entry)
        {
            _logger.Error($"[RiskCalculator] TP fallback incoherente (TP={fallbackTP:F2} <= Entry={entry:F2}), RECHAZAR");
            return 0; // Esto causará rechazo en validación de R:R
        }
        
        return fallbackTP;
    }
    
    // ... resto del código existente
}
```

Además, añadir validación final en `CalculateStructuralRiskLevels`:
```csharp
// Después de calcular SL y TP, validar coherencia
if (zone.Direction == "Bullish")
{
    if (takeProfit <= entry || takeProfit <= stopLoss)
    {
        zone.Metadata["RiskCalculated"] = false;
        zone.Metadata["RejectReason"] = "TP incoherente (TP<=Entry o TP<=SL)";
        return;
    }
}
else // Bearish
{
    if (takeProfit >= entry || takeProfit >= stopLoss)
    {
        zone.Metadata["RiskCalculated"] = false;
        zone.Metadata["RejectReason"] = "TP incoherente (TP>=Entry o TP>=SL)";
        return;
    }
}
```

---

## **🐛 ISSUE #3: LiquidityGrabDetector - Score confirmado < no confirmado**

**Archivo**: `src/Detectors/LiquidityGrabDetector.cs` o `src/Core/ScoringEngine.cs`  
**Severidad**: ⚠️ MEDIA  
**Impacto**: Scoring incorrecto de LGs

### **Problema:**
El test `LG_Score_ConfirmedVsUnconfirmed` muestra:
- LG **confirmado**: Score = 0.228
- LG **no confirmado**: Score = 0.321

Esto es **INCORRECTO**. Un LG confirmado debe tener **mayor score** que uno no confirmado, ya que la confirmación aumenta la confianza.

### **Comportamiento Esperado:**
Un LiquityGrab confirmado (sin re-break) debe tener score > uno no confirmado.

### **Hipótesis:**
- El ScoringEngine NO está aplicando bonus por confirmación
- O está aplicando una penalización incorrecta

### **Acción Requerida:**
1. Revisar cómo se calcula el score de LGs en `LiquidityGrabDetector`
2. Verificar si hay metadata `IsConfirmed` que el ScoringEngine deba leer
3. Aplicar bonus de score apropiado para LGs confirmados (ej. +0.15)

### **Test Afectado:**
- `Test_LG_Score_ConfirmedVsUnconfirmed` - Actualmente falla

---

## **🐛 ISSUE #4: OrderBlockDetector - Mitigación y Breaker no funcionan**

**Archivo**: `src/Detectors/OrderBlockDetector.cs`  
**Severidad**: 🔴 ALTA  
**Impacto**: Funcionalidad completa no operativa

### **Problema:**
Los siguientes tests fallan:
1. `OrderBlock_NoTouch_Count` - Registra toques cuando no debería
2. `OrderBlock_Bullish_Mitigated` - No marca IsMitigated=true
3. `OrderBlock_Bearish_Mitigated` - No marca IsMitigated=true
4. `OrderBlock_NotMitigated` - Marca como mitigado cuando no debería
5. `OrderBlock_Bullish_Breaker` - No marca IsBreaker=true
6. `OrderBlock_Bearish_Breaker` - No marca IsBreaker=true
7. `OrderBlock_NotBreaker` - Marca como breaker cuando no debería
8. `EdgeCase_MultipleOBs_SameTF` - Solo detecta 1 de 2 OBs
9. `EdgeCase_OB_And_Breaker_SameTF` - No detecta breaker

### **Código Revisado:**
El código de mitigación (línea 302) y breaker (línea 320) **SÍ existe** y parece correcto:

```csharp
// Mitigación
if (!ob.IsMitigated && ob.HasLeftZone && priceInZone)
{
    ob.IsMitigated = true;
    updated = true;
}

// Breaker
if (!ob.IsBreaker) { ... }
```

### **Hipótesis:**
1. **HasLeftZone no se está marcando correctamente** - La condición `priceCompletelyOut` puede ser demasiado estricta
2. **Los tests usan EnableAutoPurge=false AHORA**, puede revelar otros bugs
3. **Timing**: Las condiciones de mitigación/breaker pueden requerir más barras

### **Acción Requerida:**
1. **EJECUTAR TESTS** con EnableAutoPurge=false (ya aplicado) y ver si mejoran
2. Si siguen fallando, añadir **logging debug** en OrderBlockDetector para trace completo:
   - Cuándo se marca HasLeftZone
   - Cuándo se evalúa mitigación
   - Por qué no se cumple la condición
3. Revisar la lógica de `priceCompletelyOut` (puede ser demasiado estricta)

### **Tests Afectados:**
- 9 tests de OrderBlockDetector

---

## **🐛 ISSUE #5: LiquidityVoidDetector - Fusión incorrecta**

**Archivo**: `src/Detectors/LiquidityVoidDetector.cs`  
**Severidad**: ⚠️ MEDIA  
**Impacto**: Voids fusionados incorrectamente

### **Problema:**
Los siguientes tests fallan:
1. `LV_Fusion_ExceedsTolerance` - Esperaba 2 voids separados, obtiene 1 (fusionado)
2. `EdgeCase_MultipleVoids_SameTF` - Esperaba >= 2 voids, obtiene 1

### **Hipótesis:**
- La tolerancia de fusión (`LV_FusionToleranceATR=0.1`) no se está respetando
- Está fusionando voids que están **muy lejos** (5000 vs 5100 = 100 puntos de distancia)
- EnableAutoPurge puede estar eliminando uno de los voids (ya corregido con EnableAutoPurge=false)

### **Acción Requerida:**
1. **EJECUTAR TESTS** con EnableAutoPurge=false (ya aplicado) y ver si mejoran
2. Si siguen fallando, revisar lógica de fusión en `LiquidityVoidDetector`
3. Verificar cálculo de distancia entre voids vs tolerancia

### **Tests Afectados:**
- 2 tests de LiquidityVoidDetector

---

## **📊 PRIORIDAD DE FIXES**

| Prioridad | Issue | Severidad | Esfuerzo | Impacto |
|-----------|-------|-----------|----------|---------|
| **1** | #2 - TP<SL (R:R negativo) | 🔴 CRÍTICA | 2h | ALTO |
| **2** | #4 - OrderBlock mitigación/breaker | 🔴 ALTA | 4h | ALTO |
| **3** | #1 - SL hardcoded 3.0 ATR | ⚠️ MEDIA | 1h | MEDIO |
| **4** | #3 - LG Score confirmado | ⚠️ MEDIA | 2h | MEDIO |
| **5** | #5 - LV Fusión incorrecta | ⚠️ MEDIA | 2h | BAJO |

**Total estimado**: ~11 horas de desarrollo + testing

---

## **🔄 WORKFLOW RECOMENDADO**

1. **AHORA**: Ejecutar tests con cambios aplicados (EnableAutoPurge=false, etc.)
2. **Si mejoran**: Marcar issues #4 y #5 como "parcialmente resueltos"
3. **Empezar fixes por prioridad**: #2 → #4 → #1 → #3 → #5
4. **Cada fix**: Escribir test específico, implementar fix, validar test pasa, ejecutar suite completa
5. **Commit intermedio** después de cada fix mayor

---

## **✅ CRITERIO DE ÉXITO**

**236 / 251 tests pasando** → **251 / 251 tests pasando (100%)**

Tests profesionales que validan funcionalidad correcta y nos ayudan a construir el mejor sistema de trading del mundo. 🎯

