# 🔍 ANÁLISIS DE CALIDAD DE ESTRUCTURAS

**Fecha**: 2025-10-27  
**Objetivo**: Diagnosticar por qué el sistema tiene WR 28% y PF 0.67 (perdedor)  
**Hipótesis**: Las estructuras base (FVG, OB, POI, Swings) tienen baja calidad

---

## 🚨 **HALLAZGO CRÍTICO**

### **Problema Identificado:**

```csharp
// src/Core/EngineConfig.cs línea 634
public double HeatZone_MinScore { get; set; } = 0.3;
```

**`HeatZone_MinScore = 0.3`** significa que `StructureFusion` acepta **estructuras con Score ≥ 30%**.

**Esto es DEMASIADO BAJO** ❌

---

## 📊 **EVIDENCIA DEL PROBLEMA**

### **1. Pruebas V5.7a y V5.7b (Filtrado por Confluencia)**

| Métrica | V5.7a (3+ estruct) | V5.7b (4+ estruct) | Cambio |
|---------|-------------------|-------------------|--------|
| **Win Rate** | 28.3% | 28.2% | -0.1% ❌ |
| **Profit Factor** | 0.67 | 0.67 | 0% ❌ |

**Conclusión**: Zonas con 4-5 estructuras **NO son mejores** que zonas con 2-3 estructuras → **El problema es la CALIDAD individual**, no la cantidad.

### **2. Observación del Usuario (Trading Manual)**

> "Veo en algunos casos unos TP configurados muy lejos y en puntos que yo como trader no pondría. Creo que no elige bien las estructuras, no sé si en los SL pasará algo parecido."

**Interpretación**:
- `RiskCalculator` usa estructuras (FVG, OB, POI, Swings) para calcular TP/SL
- Si los TP/SL están mal posicionados → **las estructuras base tienen baja calidad**
- Ejemplo: Una estructura con Score 0.35 puede estar en un punto irrelevante del gráfico

---

## 🔍 **SISTEMA DE SCORING (ACTUAL)**

### **Fórmula Multi-Dimensional:**

```
raw = TF_norm * freshness * proximity * typeNorm * touchFactor * confluence * momentumMultiplier

Si FillPercentage >= FillThreshold:
  raw = max(raw, ResidualScore)

score = clamp(raw * decay, 0.0, 1.0)
```

### **Componentes:**

1. **TF Weight**: TFs altos (Daily, 4H) → mayor peso
2. **Freshness**: Decay exponencial con la edad (lambda = FreshnessLambda)
3. **Proximity**: Distancia al precio actual (ProxMaxATRFactor)
4. **Type Weight**: FVG, OB, POI, Swing tienen pesos diferentes
5. **Touch Factor**: Bonus por toques previos
6. **Confluence**: Bonus por confluencias
7. **Momentum**: Multiplicador por alineación con bias
8. **Fill Handling**: Penalización por fills, pero mantiene ResidualScore

### **Problema:**

Con tantos factores multiplicándose, es **FÁCIL** que una estructura obtenga Score 0.3-0.5 incluso si:
- Está lejos del precio (proximity bajo)
- Es vieja (freshness bajo)
- Tiene pocos toques (touchFactor bajo)
- Está en un TF bajo (tfNorm bajo)

**Resultado**: `StructureFusion` acepta estructuras **DÉBILES** que generan TPs/SLs absurdos.

---

## 💡 **PROPUESTA DE SOLUCIÓN**

### **Fase 1: Endurecer `HeatZone_MinScore` (V5.8)**

**Cambio simple y directo:**

```csharp
// De:
public double HeatZone_MinScore { get; set; } = 0.3;

// A:
public double HeatZone_MinScore { get; set; } = 0.7;  // V5.8a - Fuerte
// O:
public double HeatZone_MinScore { get; set; } = 0.6;  // V5.8b - Moderado
```

**Expectativa:**
- `StructureFusion` solo creará HeatZones con estructuras de **Score ≥ 0.7 (70%)**
- Menos volumen, pero **mayor calidad**
- TPs/SLs mejor posicionados
- **WR ↑ 35-45%**, **PF ↑ 1.2-2.0** (sistema ganador)

**Ventajas:**
- ✅ **Config-only** (sin cambios de lógica)
- ✅ **Reversible** (fácil volver atrás)
- ✅ **Rápido de probar** (1 backtest)
- ✅ **Basado en datos** (V5.7a/b demostraron que cantidad ≠ calidad)

---

### **Fase 2: Revisar Parámetros de Scoring (Si V5.8 no es suficiente)**

Si V5.8 (HeatZone_MinScore = 0.7) no mejora lo suficiente, revisar:

1. **`FreshnessLambda`**: ¿Decay muy lento? → Estructuras viejas tienen scores altos
2. **`ProxMaxATRFactor`**: ¿Demasiado permisivo? → Estructuras lejanas tienen scores altos
3. **`ResidualScore`**: ¿Demasiado alto? → Estructuras filled mantienen scores altos
4. **`TouchBodyBonus`**: ¿Demasiado generoso? → Un toque da score alto
5. **TF Weights**: ¿TFs bajos (5m, 15m) tienen demasiado peso?

**Ejemplo de ajustes:**

```csharp
// Endurecer decay de frescura
public int FreshnessLambda { get; set; } = 200;  // De 300 a 200 (decay más rápido)

// Endurecer proximidad
public double ProxMaxATRFactor { get; set; } = 3.0;  // De 5.0 a 3.0 (más restrictivo)

// Reducir score residual de estructuras filled
public double ResidualScore { get; set; } = 0.2;  // De 0.3 a 0.2
```

---

### **Fase 3: Endurecer Detectores Base (Si Fase 2 no es suficiente)**

Revisar criterios de **creación** de estructuras en los detectores:

1. **`FVGDetector`**: Aumentar tamaño mínimo de gaps
2. **`OrderBlockDetector`**: Aumentar requisitos de volumen
3. **`POIDetector`**: Endurecer criterios de rechazo
4. **`SwingDetector`**: Aumentar distancia mínima entre swings

**Esto es más invasivo**, pero puede ser necesario si el scoring no es suficiente.

---

## 📋 **PLAN DE ACCIÓN RECOMENDADO**

### **PASO 1: Probar V5.8a (HeatZone_MinScore = 0.7)**

1. Cambiar `HeatZone_MinScore` de `0.3` → `0.7`
2. Ejecutar backtest (MES DEC, 5000 barras)
3. Generar diagnóstico y KPIs
4. Evaluar:
   - **WR > 35%?** → Éxito ✓
   - **PF > 1.0?** → Sistema ganador ✓
   - **TPs/SLs mejor posicionados?** (observación manual del usuario) → Éxito ✓

### **PASO 2A: Si V5.8a tiene éxito**

- Documentar como configuración definitiva
- Monitorizar en forward testing
- Ajustar fino si es necesario (ej: probar 0.65 para más volumen)

### **PASO 2B: Si V5.8a es demasiado restrictivo**

- Probar V5.8b con `HeatZone_MinScore = 0.6`
- O ajustar parámetros de scoring (Fase 2)

### **PASO 3: Si V5.8a/b NO mejora WR/PF**

- Entrar en Fase 2: Revisar parámetros de scoring
- O Fase 3: Endurecer detectores base

---

## 🎯 **EXPECTATIVAS V5.8a**

| Métrica | V5.7b (actual) | V5.8a (esperado) | Cambio esperado |
|---------|----------------|------------------|-----------------|
| **HeatZones creadas** | ~35,000 | ~10,000-15,000 | ↓ 57-71% |
| **Operaciones** | 262 | 80-150 | ↓ 43-69% |
| **Win Rate** | 28.2% | 35-45% | ↑ 24-60% |
| **Profit Factor** | 0.67 | 1.2-2.0 | ↑ 79-199% |
| **P&L** | -$2,427 | Positivo | Sistema ganador |

**Riesgo**: Si es demasiado restrictivo (muy pocas zonas), podemos bajar a 0.65 o 0.6.

---

## 📝 **NOTAS TÉCNICAS**

### **¿Por qué Score 0.3 es bajo?**

Con la fórmula multiplicativa:
- TF bajo (5m) → tfNorm ≈ 0.3
- Estructura vieja (500 barras) → freshness ≈ 0.5
- Lejana (5 ATR) → proximity ≈ 0.6
- Tipo medio (FVG) → typeNorm ≈ 0.7
- Sin toques → touchFactor = 1.0
- Sin confluencia → confluence = 1.0
- Sin momentum → momentum = 1.0

**Score = 0.3 * 0.5 * 0.6 * 0.7 = 0.063** → Con algunos factores bonus puede llegar a **0.3-0.4**

**Pero esta estructura es DÉBIL**: vieja, lejana, en TF bajo → No debería usarse para TPs/SLs.

---

## ✅ **SIGUIENTE ACCIÓN**

**¿Apruebas que implemente V5.8a con `HeatZone_MinScore = 0.7`?**

Si sí:
1. Cambio en `EngineConfig.cs`
2. Documentación en `cambios afinando DFM.md`
3. Backtest
4. Análisis de resultados

