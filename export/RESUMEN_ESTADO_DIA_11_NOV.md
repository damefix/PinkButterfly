# 📋 RESUMEN DE ESTADO - 11 de Noviembre 2025

**Sistema:** PinkButterfly CoreBrain v6.0k  
**Última Ejecución:** backtest_20251111_214048  
**Generado:** 2025-11-11 23:05

---

## 🎯 ESTADO ACTUAL DEL SISTEMA

### Métricas Principales

| Métrica | Valor | Estado |
|---------|-------|--------|
| **Win Rate** | 22.2% (8/36) | 🔴 Muy Bajo |
| **Profit Factor** | 0.33 | 🔴 Sistema Perdedor |
| **P&L Total** | -$1,357.62 | 🔴 Negativo |
| **Avg R:R** | 1.74 | ⚠️ Insuficiente (necesita 3.50) |
| **Ops Ejecutadas** | 36 | ⚠️ Bajo volumen |
| **SL_FIRST** | 55.6% | 🔴 Malas entradas |
| **MFE/MAE Ratio** | 0.69 (31.3/45.4 pts) | 🔴 Precio va más contra que a favor |

### Pipeline (Embudo de Señales)

| Paso | Señales | % Retención | Observación |
|------|---------|-------------|-------------|
| StructureFusion | 39,889 | 100.0% | ✅ Zonas creadas |
| ProximityAnalyzer | 6 | 0.01% | 🔴 **CUELLO DE BOTELLA CRÍTICO** |
| DFM_Evaluated | 204 | 3400% | ✅ Evalúa las pocas que pasan |
| DFM_Passed | 185 | 90.7% | ✅ Threshold OK |
| RiskCalculator | 52 | 28.1% | 🔴 Rechaza 85% por R:R (RejRR=174) |
| TradeManager | 36 | 69.2% | ✅ Registra mayoría |

**Conclusión:** Sistema bloqueado en ProximityAnalyzer (99.98% filtrado) y RiskCalculator (85% rechazado).

---

## ✅ CAMBIOS APLICADOS HOY

### 1. **FIX CRÍTICO: BOS Score (ContextManager.cs)**

**Problema:** BOS Score siempre 0.00 → Bias Neutral alto (35%).

**Solución aplicada:**
- `BOSDetector.cs`: Establecer `Type = breakType` (línea 157)
- `ContextManager.cs`: 
  - Usar `GetStructureBreaks()` en vez de `GetAllStructures()`
  - Fix age calculation (usar DateTime en vez de bar indices)
  - **Temporal fix:** Usar `currentBar - 1` para evitar timing issue

**Resultado:**
- ✅ BOS Score ahora varía: [-0.990, +0.960]
- ✅ 10.9% del tiempo BOS != 0 (2,606 de 23,532 eventos)
- ⚠️ Promedio muy bajo (0.008) debido a lookback 24h corto

### 2. **FIX: Python Script (analizador-logica-operaciones.py)**

**Problema:** Script no parseaba logs `[DIAGNOSTICO][Context]` con `(barUsed=X)` → Eventos: 0

**Solución aplicada:**
- Regex actualizado para parsear `(barUsed=\d+)` como opcional
- Línea 293: `(?:\s+\(barUsed=\d+\))?` añadido al patrón

**Resultado:**
- ✅ Ahora parsea 23,532 eventos de Context (antes 0)
- ✅ Distribución Bias visible: Bullish 38.1%, Bearish 26.9%, Neutral 34.9%
- ✅ Componentes del Bias visibles en informe

### 3. **Ajustes de Configuración (EngineConfig.cs)**

**Cambios aplicados por el usuario:**

```csharp
// Pesos DFM (suma = 1.0)
Weight_Proximity = 0.30      // Era 0.12
Weight_Bias = 0.30           // Era 0.35
Weight_Confluence = 0.06     // Era 0.08
Weight_CoreScore = 0.20      // Era 0.25
Weight_Momentum = 0.18       // (sin cambio)
Weight_Type = 0.02           // (sin cambio)

// Proximity
MinProximityForEntry = 0.20  // Era 0.50

// Risk Management
MaxSLDistancePoints_Normal = 25.0     // Basado en P90 real
MaxSLDistancePoints_HighVol = 25.0
MaxTPDistancePoints_Normal = 47.0     // Basado en P90 real
MaxTPDistancePoints_HighVol = 47.0

// P4_Fallback Filter (RiskCalculator.cs línea 648)
// Solo acepta P4 si RR >= 1.80 y TPDistATR <= 2.5
```

**Impacto esperado:**
- Proximity mayor → más peso a cercanía del precio
- Caps SL/TP ajustados a datos reales (antes eran para swing trading)
- P4_Fallback más estricto → menos TPs fallback de baja calidad

**Impacto observado:**
- ProximityAnalyzer: de 6 zonas → sigue igual (problema persiste)
- RejRR: de 160 → 174 (empeoró ligeramente, probablemente por caps más estrictos)
- Win Rate: sin cambio significativo (22.2%)

---

## 🔴 PROBLEMAS CRÍTICOS IDENTIFICADOS

### 1. **ProximityAnalyzer: Filtrado Excesivo (99.98%)**

**Datos:**
- Entrada: 39,889 zonas
- Salida: 6 zonas (0.01%)
- **Pérdida: 39,883 zonas**

**Causas probables:**
1. `MinProximityForEntry = 0.20` combinado con distancias reales altas
2. `PreferAligned` filter + Bias Neutral alto (34.9%) → descarta ~35% adicional
3. `MaxDistanceToRegister_ATR = 1.0/1.5` (muy estricto?)

**Análisis de logs:**
```
AvgProxAligned ≈ 0.370
AvgDistATRAligned ≈ 0.53
```
→ Promedios parecen razonables, pero solo cuenta las que YA pasaron el filtro.

**Hipótesis:**
- El filtro `PreferAligned` con Bias Neutral alto está matando señales.
- Las zonas están realmente lejos (> 1.0 ATR) cuando se evalúan.

**MTF Bug descartado:**
- `[CTX_NO_DATA]` count: 94 (bajo, no es el problema principal)

### 2. **RiskCalculator: Rechazo Masivo por R:R (85%)**

**Datos:**
- Entrada: 204 señales aceptadas
- `RejRR=174` (85.3%)
- Salida: 52 señales con Risk calculado

**Causas:**
- SL estructurales (swings) están muy lejos
- TP estructurales no se encuentran o están muy cerca
- Resultado: `actualRR < MinRiskRewardRatio` (actualmente 1.10)

**Análisis de TPs:**
```
P4_Fallback: 266 (44.6%) - TF=-1, Avg RR=1.34
P3_Swing: 268 (45.0%) - Avg RR estimado ~1.50-2.00
P0_Zone: 62 (10.4%)
```
→ Demasiados TPs P4_Fallback con R:R bajo.

### 3. **Bias Neutral Alto (34.9%)**

**Datos:**
- Threshold: 0.30 / -0.30 (correcto)
- Bias Composite Score promedio: 0.076
- Componentes promedio:
  - EMA20 Slope: 0.040
  - EMA50 Cross: 0.182
  - **BOS Count: 0.008** ← MUY BAJO
  - Regression 24h: 0.085

**Causa raíz:**
- BOS Score promedio 0.008 porque lookback de 24h es muy corto
- Solo 10.9% del tiempo hay estructuras BOS/CHoCH recientes
- Estructuras en TF=60m se crean cada hora, lookback 24h solo ve ~24 barras

**Impacto:**
- 34.9% del tiempo el sistema opera sin dirección clara
- `PreferAligned` filter descarta zonas "counter-bias" incluso en mercado lateral

### 4. **Calidad de Entradas: SL_FIRST = 55.6%**

**Datos:**
- TP_FIRST: 15 ops (41.7%) ✅ Precio va hacia TP primero
- SL_FIRST: 20 ops (55.6%) ❌ Precio va hacia SL primero
- MFE promedio: 31.3 pts
- MAE promedio: 45.4 pts
- **Ratio MFE/MAE: 0.69** (precio va 44% más contra que a favor)

**Causas probables:**
1. Bias desincronizado → entramos contra tendencia real
2. Timing incorrecto → entramos antes de reversión
3. Zonas de baja calidad → no hay confluence real

---

## 📊 ANÁLISIS DEL BIAS COMPUESTO (DETALLADO)

### Componentes Actuales

| Componente | Valor Promedio | Rango | Peso Configurado | Contribución Real |
|------------|----------------|-------|------------------|-------------------|
| EMA20 Slope | 0.040 | N/A | 25% | Normalización OK |
| EMA50 Cross | 0.182 | [-1, +1] | 25% | Normalización OK |
| BOS Count | **0.008** | [-1, +1] | 25% | **❌ SATURADO EN 0** |
| Regression 24h | 0.085 | [-1, +1] | 25% | Normalización OK |

**Diagnóstico:**
- 3 de 4 componentes funcionan correctamente
- BOS Score contribuye casi nada (0.008 de 0.25 esperado)
- Composite Score = 0.040 + 0.182 + 0.008 + 0.085 = **0.315** promedio
- Con threshold 0.30, muchos scores quedan en rango [-0.30, +0.30] → Neutral

### Estadísticas de BOS Score

- **Total eventos Context:** 23,532
- **Eventos con BOS != 0:** 2,606 (10.9%)
- **Eventos con BOS = 0:** 20,926 (89.1%)
- **BOS Score rango:** [-0.990, +0.960]
- **BOS Score promedio:** 0.008

**Interpretación:**
- El código funciona ✅
- El lookback de 24h es insuficiente ⚠️
- Solo ~11% del tiempo hay estructuras BOS/CHoCH dentro de 24h

---

## 🚀 OPCIONES PARA MAÑANA

### Opción A: Extender Lookback de BOS (RECOMENDADO)

**Acción:**
```csharp
// ContextManager.cs línea ~282
int lookbackMinutes = 24 * 60; // ERA: 24h
// CAMBIAR A:
int lookbackMinutes = 48 * 60; // NUEVO: 48h (2 días)
// O MEJOR:
int lookbackMinutes = 72 * 60; // NUEVO: 72h (3 días)
```

**Impacto esperado:**
- Más estructuras BOS/CHoCH disponibles (x2-x3)
- BOS Score promedio sube de 0.008 a 0.020-0.050
- Bias Neutral baja de 34.9% a ~20-25%
- Más señales "Aligned" pasan `PreferAligned` filter
- ProximityAnalyzer deja pasar más zonas

**Riesgos:**
- BOS muy antiguas (72h = 3 días) pueden no reflejar momentum actual
- Necesitarás aplicar más `timeDecay` para compensar

**Prioridad:** ⭐⭐⭐⭐⭐ (ALTA)

---

### Opción B: Reducir Threshold del Bias

**Acción:**
```csharp
// EngineConfig.cs
BiasThreshold_Normal = 0.20;      // ERA: 0.30
BiasThreshold_HighVol = 0.20;     // ERA: 0.30
```

**Impacto esperado:**
- Bias Neutral baja (menos eventos con Score en [-0.30, +0.30])
- Más señales clasificadas como Bullish/Bearish
- `PreferAligned` filter menos estricto

**Riesgos:**
- Si BOS sigue en 0.008, el bias será más "ruidoso" (cambios por EMAs débiles)
- Podrías generar señales en mercado lateral sin confirmación estructural

**Prioridad:** ⭐⭐⭐ (MEDIA) - Solo si Opción A no funciona

---

### Opción C: Rebalancear Pesos del Bias Compuesto

**Acción:**
```csharp
// ContextManager.cs líneas ~198-201
double ema20Score = CalculateEMASlope(...);    // Weight: 25% → 30%
double emaCrossScore = CalculateEMACross(...); // Weight: 25% → 30%
double bosScore = CalculateBOSScore(...);      // Weight: 25% → 15%
double regressionScore = CalculateRegression24h(...); // Weight: 25% → 25%
```

**Rationale:**
- BOS Score aporta poco (0.008) → reducir su peso
- EMA/Regression siempre tienen valores → aumentar su peso

**Impacto esperado:**
- Composite Score más estable (menos dependiente de BOS)
- Bias Neutral puede bajar ligeramente (depende de distribución de EMAs)

**Riesgos:**
- Pierdes momentum/confirmación estructural
- Sistema podría generar señales sin "confirmation" real

**Prioridad:** ⭐⭐ (BAJA) - Solo como último recurso

---

### Opción D: Investigar ProximityAnalyzer (Filtros Geométricos)

**Acción:**
1. Leer `ProximityAnalyzer.cs` completo
2. Identificar por qué 99.98% de zonas no pasan
3. Proponer cambios quirúrgicos basados en logs

**Hipótesis a validar:**
- `MaxDistanceToRegister_ATR = 1.0/1.5` es demasiado estricto
- `PreferAligned` + Bias Neutral alto mata demasiadas señales
- Cálculo de distancia tiene bug MTF (aunque MTF bug fue descartado)

**Prioridad:** ⭐⭐⭐⭐ (ALTA) - Pero requiere más investigación

---

### Opción E: Relajar Filtro P4_Fallback (NO RECOMENDADO)

**Acción:**
```csharp
// RiskCalculator.cs línea 648
if (isFallbackTP && (actualRR < 1.60 || tpDistanceATR > 3.0)) // ERA: 1.80 / 2.5
```

**Rationale:**
- Permitir más TPs P4_Fallback con R:R más bajos

**Impacto esperado:**
- Más señales pasan RiskCalculator (RejRR baja)
- Pero Win Rate probablemente empeora (TPs menos alcanzables)

**Prioridad:** ⭐ (MUY BAJA) - Contraproducente

---

## 📁 ARCHIVOS CLAVE

### Código
- `pinkbutterfly-produccion/ContextManager.cs` - Bias compuesto, BOS Score
- `pinkbutterfly-produccion/BOSDetector.cs` - Detección BOS/CHoCH
- `pinkbutterfly-produccion/ProximityAnalyzer.cs` - Filtro geométrico (CUELLO DE BOTELLA)
- `pinkbutterfly-produccion/RiskCalculator.cs` - SL/TP, filtro R:R
- `pinkbutterfly-produccion/EngineConfig.cs` - Todos los parámetros

### Informes
- `export/ANALISIS_LOGICA_DE_OPERACIONES.md` - MFE/MAE, SL_FIRST, Waterfall
- `export/DIAGNOSTICO_LOGS.md` - Logs técnicos, embudo detallado
- `export/KPI_SUITE_COMPLETA.md` - KPIs financieros, P&L

### Scripts
- `export/analizador-logica-operaciones.py` - Generador del informe principal (✅ FIXED)

### Logs
- `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_214048.log`
- `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251111_214048.csv`

---

## 🎯 PLAN RECOMENDADO PARA MAÑANA

### Prioridad 1: Extender Lookback de BOS (30 min)

**Objetivo:** Bajar Bias Neutral de 34.9% a ~20-25%

**Pasos:**
1. Modificar `ContextManager.cs` línea ~282: `lookbackMinutes = 72 * 60`
2. Opcional: Ajustar `timeDecay` para compensar edad
3. Compilar y copiar a NinjaTrader
4. Ejecutar backtest (5-10 min)
5. Validar en logs:
   ```powershell
   Select-String -Pattern "\[BOS_DEBUG\] Score=" "backtest_YYYYMMDD.log" | Select -First 20
   ```
   → BOS Score debería subir de 0.008 a ~0.020-0.050

### Prioridad 2: Analizar ProximityAnalyzer (45 min)

**Objetivo:** Entender por qué 99.98% de zonas son descartadas

**Pasos:**
1. Leer `ProximityAnalyzer.cs` completo (función `Process`)
2. Identificar filtros que descartan zonas:
   - `MaxDistanceToRegister_ATR`
   - `PreferAligned`
   - `MinProximityForEntry`
3. Buscar en logs trazas de descarte:
   ```powershell
   Select-String -Pattern "\[Proximity\].*Descartada|Filtered" "backtest_YYYYMMDD.log" | Measure-Object
   ```
4. Proponer cambios quirúrgicos basados en datos reales

### Prioridad 3: Si Lookback no funciona → Threshold (15 min)

**Objetivo:** Reducir Bias Neutral sin esperar estructuras BOS

**Pasos:**
1. Solo si BOS Score sigue < 0.020 después de extender lookback
2. Modificar `EngineConfig.cs`: `BiasThreshold_Normal/HighVol = 0.20`
3. Backtest y validar distribución Bias en informe

---

## 📌 NOTAS IMPORTANTES

### ¿Por qué BOS Score está en 0.008?

**Respuesta corta:** Lookback de 24h es muy corto para TF=60m (solo ve 24 barras).

**Detalles:**
- `CalculateBOSScore` busca estructuras BOS/CHoCH en ventana de 24h
- Estructuras se crean en TF=60m (1 barra = 1 hora)
- 24h = solo 24 barras de TF=60m
- La mayoría del tiempo, no hay BOS/CHoCH en esas 24 barras recientes
- Cuando hay, el score sube correctamente (e.g., 0.450, -0.496)

**Solución:** Extender lookback a 48h-72h para ver más estructuras.

### ¿Por qué ProximityAnalyzer bloquea tanto?

**Hipótesis (a validar mañana):**
1. **`PreferAligned` + Bias Neutral alto:**
   - 34.9% del tiempo Bias = Neutral
   - `PreferAligned` descarta zonas "counter-bias"
   - Pero con Bias Neutral, casi todas son "counter" o "aligned" arbitrariamente
   - Resultado: descarte masivo por criterio débil

2. **`MaxDistanceToRegister_ATR = 1.0/1.5` muy estricto:**
   - Promedio `DistATRAligned = 0.53` (pero solo de las que YA pasaron)
   - Las 39,883 descartadas probablemente tienen `DistATR > 1.0/1.5`
   - Necesita validación en logs

3. **`MinProximityForEntry = 0.20`:**
   - Actualmente en 0.20 (relajado desde 0.50)
   - Pero si distancia es > 1.0 ATR, ProximityFactor se calcula mal
   - Necesita revisión de fórmula

### ¿Por qué RiskCalculator rechaza 85%?

**Respuesta corta:** SL estructurales muy lejos, TPs fallback muy cerca → R:R < 1.10

**Detalles:**
- `CalculateStructuralSL_Buy/Sell` busca swing protector
- Si swing está lejos (e.g., 15-20 pts), `riskDistance` es grande
- `CalculateStructuralTP_Buy/Sell` busca TPs estructurales (P0, P3)
- Si no encuentra, usa P4_Fallback (matemático)
- P4_Fallback suele estar cerca (e.g., 10-15 pts)
- Resultado: `actualRR = TPDist / SLDist = 15 / 20 = 0.75 < 1.10` → RECHAZADO

**Solución:**
- Caps de SL/TP ya ajustados (25/47 pts)
- P4_Fallback ya filtrado (RR >= 1.80, DistATR <= 2.5)
- Problema persiste → necesita revisar selección de SL (swings muy lejanos)

---

## ✅ TODO LIST PARA MAÑANA

- [ ] **Extender Lookback BOS a 72h** (ContextManager.cs línea ~282)
- [ ] **Compilar y backtest** (5-10 min)
- [ ] **Validar BOS Score en logs** (debe subir a ~0.020-0.050)
- [ ] **Analizar ProximityAnalyzer.cs** (leer función `Process`)
- [ ] **Buscar trazas de descarte en logs** (Proximity Filtered)
- [ ] **Proponer cambios quirúrgicos en ProximityAnalyzer**
- [ ] **Si BOS sigue bajo:** Reducir threshold a 0.20
- [ ] **Regenerar informes y comparar:**
  - Win Rate objetivo: >30%
  - Profit Factor objetivo: >0.80
  - SL_FIRST objetivo: <40%
  - ProximityAnalyzer kept: >100 zonas (vs 6 actual)

---

## 📞 CONTACTO Y SOPORTE

**Usuario:** meste  
**Sistema:** Windows 10.0.26200  
**NinjaTrader Path:** `C:\Users\meste\Documents\NinjaTrader 8`  
**Workspace:** `C:\Users\meste\Documents\trading\PinkButterfly`

---

*Informe generado automáticamente - 2025-11-11 23:05*  
*Versión del Sistema: PinkButterfly CoreBrain v6.0k*


