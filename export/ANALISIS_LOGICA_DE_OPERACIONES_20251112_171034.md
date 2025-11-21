# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-12 17:14:57
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_171034.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251112_171034.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 33
- **Win Rate:** 33.3% (11/33)
- **Profit Factor:** 1.11
- **Avg R:R Planeado:** 2.49
- **R:R Mínimo para Break-Even:** 2.00

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 40 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.49
   - R:R necesario: 2.00
   - **Gap:** -0.49

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8084 | 34.6% |
| Bullish | 8980 | 38.4% |
| Bearish | 6292 | 26.9% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.079
- **Score Min/Max:** [-0.990, 0.970]
- **Componentes (promedio):**
  - EMA20 Slope: 0.042
  - EMA50 Cross: 0.183
  - BOS Count: 0.009
  - Regression 24h: 0.089

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.970 (apenas supera threshold)
- Score mínimo observado: -0.990 (apenas supera threshold)
- **Consecuencia:** Sistema queda 34.6% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.6%)

**Posibles causas:**
- **BOS Score bajo (0.009):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio 0.079 indica poca señal direccional
- **Mercado lateral:** Scores reales [-0.99, 0.97] muy cercanos a 0

**Recomendaciones:**
1. ✅ Verificar que `BOSDetector.cs` establece `Type = breakType` (bug conocido)
2. ✅ Revisar logs para confirmar que BOS Score != 0.0
3. ⚠️ Si BOS sigue en ~0, investigar detección de BOS/CHoCH
4. ⚠️ Considerar bajar threshold a 0.2 SOLO si los 3 pasos anteriores están OK

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 10211 | 100.0% | 100.0% |
| ProximityAnalyzer | 4195 | 41.1% | 41.1% |
| DFM_Evaluated | 864 | 20.6% | 8.5% |
| DFM_Passed | 864 | 100.0% | 8.5% |
| RiskCalculator | 6193 | 716.8% | 60.7% |
| Risk_Accepted | 102 | 1.6% | 1.0% |
| TradeManager | 33 | 32.4% | 0.3% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 6091 señales, -98.4%)
- **Tasa de conversión final:** 0.32% (de 10211 zonas iniciales → 33 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,631 | 60.7% |
| NO_SL | 423 | 15.7% |
| ENTRY_TOO_FAR | 385 | 14.3% |
| TP_CHECK_FAIL | 250 | 9.3% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,631 rechazos, 60.7%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2512 | 88.7% |
| P0_ANY_DIR | 320 | 11.3% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 45.82 pts (máxima ganancia flotante)
- **MAE Promedio:** 31.20 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 93.81

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 21 | 63.6% |
| SL_FIRST (precio fue hacia SL) | 11 | 33.3% |
| NEUTRAL (sin dirección clara) | 1 | 3.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 42.4%
- **Entradas Malas (MAE > MFE):** 57.6%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 33 | 21 | 11 | 63.6% | 45.82 | 31.20 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | BUY | 14.00 | 17.25 | 0.81 | SL_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0004 | SELL | 7.25 | 21.50 | 0.34 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0011 | SELL | 13.00 | 18.75 | 0.69 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0015 | BUY | 1.75 | 11.00 | 0.16 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0017 | BUY | 14.50 | 26.50 | 0.55 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0018 | SELL | 42.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0019 | SELL | 2.25 | 13.00 | 0.17 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0021 | SELL | 166.00 | 14.00 | 11.86 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0021_2 | SELL | 166.00 | 14.00 | 11.86 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0022 | SELL | 248.50 | 11.25 | 22.09 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0026 | SELL | 0.00 | 68.50 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0027 | SELL | 33.75 | 18.00 | 1.88 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0031 | BUY | 51.50 | 63.75 | 0.81 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0032 | BUY | 9.50 | 66.75 | 0.14 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0036 | SELL | 55.25 | 58.00 | 0.95 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0037 | SELL | 36.00 | 75.50 | 0.48 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0046 | BUY | 15.25 | 8.25 | 1.85 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0048 | SELL | 83.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049 | SELL | 26.75 | 40.75 | 0.66 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0053 | BUY | 11.25 | 10.00 | 1.12 | SL_FIRST | CLOSED | 👍 Entrada correcta |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,326

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 483 | 38.9% | 60.2% | 4.18 | 46.8% | 1.97 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 266 | 54.1% | 58.6% | 2.27 | 49.6% | 1.82 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 355 | 69.6% | 60.0% | 2.97 | 61.7% | 1.99 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 208 | 70.2% | 65.4% | 0.89 | 68.8% | 2.27 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 14 | 57.1% | 42.9% | 0.15 | 42.9% | 3.29 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (483 oportunidades)

- **WR Teórico:** 38.9% (si se hubieran ejecutado)
- **TP_FIRST:** 60.2% (291 de 483)
- **SL_FIRST:** 37.5% (181 de 483)
- **MFE Promedio:** 49.22 pts
- **MAE Promedio:** 38.88 pts
- **MFE/MAE Ratio:** 4.18
- **Good Entries:** 46.8% (MFE > MAE)
- **R:R Promedio:** 1.97

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (266 oportunidades)

- **WR Teórico:** 54.1% (si se hubieran ejecutado)
- **TP_FIRST:** 58.6% (156 de 266)
- **SL_FIRST:** 41.0% (109 de 266)
- **MFE Promedio:** 53.02 pts
- **MAE Promedio:** 39.46 pts
- **MFE/MAE Ratio:** 2.27
- **Good Entries:** 49.6% (MFE > MAE)
- **R:R Promedio:** 1.82

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (355 oportunidades)

- **WR Teórico:** 69.6% (si se hubieran ejecutado)
- **TP_FIRST:** 60.0% (213 de 355)
- **SL_FIRST:** 39.7% (141 de 355)
- **MFE Promedio:** 71.68 pts
- **MAE Promedio:** 42.80 pts
- **MFE/MAE Ratio:** 2.97
- **Good Entries:** 61.7% (MFE > MAE)
- **R:R Promedio:** 1.99

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (208 oportunidades)

- **WR Teórico:** 70.2% (si se hubieran ejecutado)
- **TP_FIRST:** 65.4% (136 de 208)
- **SL_FIRST:** 33.7% (70 de 208)
- **MFE Promedio:** 78.93 pts
- **MAE Promedio:** 47.14 pts
- **MFE/MAE Ratio:** 0.89
- **Good Entries:** 68.8% (MFE > MAE)
- **R:R Promedio:** 2.27

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (14 oportunidades)

- **WR Teórico:** 57.1% (si se hubieran ejecutado)
- **TP_FIRST:** 42.9% (6 de 14)
- **SL_FIRST:** 57.1% (8 de 14)
- **MFE Promedio:** 77.03 pts
- **MAE Promedio:** 67.41 pts
- **MFE/MAE Ratio:** 0.15
- **Good Entries:** 42.9% (MFE > MAE)
- **R:R Promedio:** 3.29

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 33 | 483 | 266 | 355 |
| **TP_FIRST %** | 63.6% | 60.2% | 58.6% | 60.0% |
| **Good Entries %** | 42.4% | 46.8% | 49.6% | 61.7% |
| **MFE/MAE Ratio** | 93.81 | 4.18 | 2.27 | 2.97 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 355 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 69.6%
   - Good Entries: 61.7%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 12.50 pts
- **Mediana:** 9.57 pts
- **Min/Max:** 0.55 / 40.44 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 9.57 |
| P70 | 17.37 |
| P80 | 19.46 |
| P90 | 23.76 |
| P95 | 38.39 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 24.53 pts
- **Mediana:** 21.25 pts
- **Min/Max:** 3.25 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 21.25 |
| P70 | 32.40 |
| P80 | 45.55 |
| P90 | 51.35 |
| P95 | 53.33 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 23; // Era 60
public int MaxTPDistancePoints { get; set; } = 51; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 23.8pts, TP: 51.4pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (33.3%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.333) / 0.333
R:R_min = 2.00
```

**Estado actual:** R:R promedio = 2.49
**Gap:** -0.49 (necesitas mejorar R:R)

---

## 5. CONCLUSIONES Y PLAN DE ACCIÓN PRIORIZADO

### Prioridad 1: CORREGIR BIAS (CRÍTICO)

**Problema:** Bias alcista con gráfico bajista → entradas contra-tendencia

**Solución:**
1. Reemplazar EMA200@60m por **bias compuesto rápido**
2. Componentes:
   - EMA20@60m (tendencia 20h)
   - EMA50@60m (tendencia 50h)
   - BOS/CHoCH count (cambios estructura)
   - Regresión lineal 24h
3. Pesos sugeridos: 30%, 25%, 25%, 20%

**Impacto esperado:** +15-25% WR (entradas alineadas con movimiento real)

### Prioridad 2: LÍMITES SL/TP DINÁMICOS

**Problema:** Límites actuales son para swing, no intradía

**Solución:**
1. **MaxSLDistancePoints:** 60 → **23** (P90 real)
2. **MaxTPDistancePoints:** 120 → **51** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.49) < R:R mínimo (2.00)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=33.3%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-12 17:14:57*