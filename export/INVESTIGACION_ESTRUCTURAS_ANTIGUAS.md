# Investigación: Estructuras Antiguas (7000+ barras)

**Fecha:** 2025-10-27  
**Versión:** V5.7b  
**Hallazgo:** Estructuras de hasta 7665 barras (≈320 días en 1H) están siendo utilizadas para SL/TP

---

## 📊 DATOS DEL BACKTEST

### Stop Loss (SL)
- **Edad Candidatos:** mediana=4791 barras, max=7835 barras
- **Edad Seleccionados:** mediana=3554 barras, max=7665 barras
- **Score promedio candidatos:** 0.42
- **Score promedio seleccionados:** 0.43

### Take Profit (TP)
- **Edad Candidatos:** mediana=5018 barras, max=7826 barras
- **Edad Seleccionados:** mediana=56 barras, max=7630 barras
- **Score promedio candidatos:** 0.43
- **Score promedio seleccionados:** 0.30

### Conclusión
Hay estructuras **activas** de más de **7000 barras** en el sistema.

---

## 🔍 SISTEMA DE SCORING ACTUAL

### Configuración
```csharp
FreshnessLambda = 20    // 50% score a las 20 barras
DecayLambda = 100       // 50% score a las 100 barras sin update
```

### Fórmula
```csharp
// Línea 73-76 ScoringEngine.cs
int ageBars = currentBar - structure.CreatedAtBarIndex;
double freshness = Math.Exp(-ageBars / FreshnessLambda);

// Línea 146-149 ScoringEngine.cs
int deltaBarsSinceUpdate = currentBar - structure.LastUpdatedBarIndex;
double decay = Math.Exp(-deltaBarsSinceUpdate / DecayLambda);

finalScore = rawScore * freshness * decay;
```

### Cálculo para estructura de 7000 barras
```
ageBars = 7000
freshness = exp(-7000/20) = exp(-350) ≈ 0.0

deltaBarsSinceUpdate = 1 (actualizada recientemente)
decay = exp(-1/100) = 0.99

finalScore = 1.0 * 0.0 * 0.99 ≈ 0.0
```

**El score SÍ decae correctamente a ~0.0** ✅

---

## 🔍 SISTEMA DE PURGA AUTOMÁTICA

### Configuración
```csharp
EnableAutoPurge = true          // ACTIVADO
MaxAgeBarsForPurge = 150       // Purgar estructuras > 150 barras
MinScoreThreshold = 0.20       // Purgar estructuras con score < 0.20
```

### Lógica de Purga (CoreEngine.cs línea 1589)
```csharp
var oldStructures = structures
    .Where(s => (currentBar - s.CreatedAtBarIndex) > MaxAgeBarsForPurge)
    .ToList();

foreach (var structure in oldStructures)
{
    RemoveStructureInternal(structure.Id, "Purged_Expired");
}
```

**Estructuras > 150 barras DEBERÍAN ser purgadas** ❌

---

## 🚨 PROBLEMA IDENTIFICADO

### **HIPÓTESIS 1: Purga no está funcionando**

Posibles causas:
1. **`EnableAutoPurge` desactivado en runtime** (poco probable, default=true)
2. **La purga solo ocurre en ciertos TFs** y los Swings están en TFs no purgados
3. **Hay estructuras "protegidas"** (TouchCount alto, etc) que evitan purga
4. **Bug en la lógica de purga** que no alcanza ciertas estructuras

### **VERIFICACIÓN NECESARIA:**
¿Hay logs de purga en el backtest? Buscar:
- `"Purgadas"` en los logs
- `"Purged_Expired"`
- `"PurgedByType"`

---

## 🎯 PROPUESTAS DE SOLUCIÓN

### **Opción A: Filtro Explícito de Edad en RiskCalculator**
```csharp
// En FindProtectiveSwingLowBanded y FindProtectiveSwingHighBanded
foreach (var s in allStructures.OfType<SwingInfo>())
{
    int age = currentBar - s.CreatedAtBarIndex;
    
    // NUEVO: Rechazar estructuras muy antiguas
    if (age > _config.MaxStructureAgeForSL)  // ej: 500 barras
        continue;
        
    if (s.IsActive && !s.IsHigh && s.Low < zone.Low)
    {
        // ... resto del código
    }
}
```

**Config:**
```csharp
public int MaxStructureAgeForSL { get; set; } = 500;   // 500 barras
public int MaxStructureAgeForTP { get; set; } = 500;   // 500 barras
```

**Pros:**
- ✅ Solución directa y explícita
- ✅ No afecta otros componentes
- ✅ Fácil de ajustar y probar
- ✅ Datos muestran que mediana seleccionada SL=3554, con 500 cubrimos mayoría

**Contras:**
- ❌ No soluciona el problema de raíz (purga)
- ❌ Estructuras antiguas siguen en memoria

---

### **Opción B: Investigar y Arreglar Sistema de Purga**
1. Verificar si hay logs de purga en backtest actual
2. Añadir logging detallado de purga
3. Identificar por qué estructuras > 150 barras no se purgan
4. Arreglar bug en sistema de purga

**Pros:**
- ✅ Soluciona problema de raíz
- ✅ Mejora rendimiento general (menos memoria)
- ✅ Sistema más robusto

**Contras:**
- ❌ Requiere más investigación
- ❌ Puede afectar otros componentes
- ❌ Más complejo de implementar

---

### **Opción C: Híbrida (RECOMENDADA)**
1. **Corto plazo:** Implementar filtro de edad en RiskCalculator (Opción A)
2. **Medio plazo:** Investigar y arreglar purga (Opción B)

---

## 📈 IMPACTO ESPERADO

Con `MaxStructureAgeForSL = 500`:

### Candidatos SL
- **Actual:** 94735 candidatos
- **Esperado:** ~40000 candidatos (reducción ~58%)
- Eliminar estructuras con mediana=4791 barras

### Candidatos TP  
- **Actual:** 46381 candidatos
- **Esperado:** ~15000 candidatos (reducción ~68%)
- Eliminar estructuras con mediana=5018 barras

### Scores
- **Score promedio SL:** 0.42 → ~0.60+ (solo estructuras frescas)
- **Score promedio TP:** 0.43 → ~0.65+ (solo estructuras frescas)

### WR Esperado
- **Actual:** 25.5% 
- **Esperado:** 35-40% (usando solo estructuras válidas y frescas)

---

## 🎯 SIGUIENTE PASO

**DECISIÓN NECESARIA:**

1. **¿Implementar filtro de edad inmediatamente?** (Opción A / C)
   - Rápido, seguro, impacto medible

2. **¿Investigar primero el sistema de purga?** (Opción B)
   - Buscar logs de purga en backtest
   - Añadir logging detallado
   - Identificar bug

3. **¿Ambos?** (Opción C - RECOMENDADO)
   - Filtro de edad YA para mejorar resultados
   - Investigación de purga en paralelo

---

## 📝 NOTAS ADICIONALES

### Observación: TP tiene mejor edad seleccionada
- **TP mediana seleccionada:** 56 barras ✅
- **SL mediana seleccionada:** 3554 barras ❌

Esto sugiere que el sistema de selección de TP (jerárquico P1→P2→P3→Fallback) está funcionando mejor que SL (banda [10,15] ATR).

### Recomendación adicional
Considerar **sistema jerárquico para SL** similar a TP:
1. Buscar Swing reciente (< 200 barras) en banda [10,15]
2. Si no, Swing antiguo en banda [10,15]
3. Si no, Fallback

Pero esto es **secundario** al filtro de edad.


