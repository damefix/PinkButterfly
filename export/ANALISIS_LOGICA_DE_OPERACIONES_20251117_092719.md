# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-17 09:41:16
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_092719.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251117_092719.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 29
- **Win Rate:** 41.4% (12/29)
- **Profit Factor:** 0.84
- **Avg R:R Planeado:** 1.69
- **R:R Mínimo para Break-Even:** 1.42

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 24 puntos
   - TP máximo observado: 46 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.69
   - R:R necesario: 1.42
   - **Gap:** -0.27

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bullish | 10124 | 39.1% |
| Neutral | 8729 | 33.7% |
| Bearish | 7039 | 27.2% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.080
- **Score Min/Max:** [-1.000, 1.000]
- **Componentes (promedio):**
  - EMA20 Slope: 0.041
  - EMA50 Cross: 0.181
  - BOS Count: 0.015
  - Regression 24h: 0.093

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 1.000 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 33.7% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (33.7%)

**Posibles causas:**
- **BOS Score bajo (0.015):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio 0.080 indica poca señal direccional
- **Mercado lateral:** Scores reales [-1.00, 1.00] muy cercanos a 0

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
| StructureFusion | 88141 | 100.0% | 100.0% |
| ProximityAnalyzer | 1987 | 2.3% | 2.3% |
| DFM_Evaluated | 436 | 21.9% | 0.5% |
| DFM_Passed | 436 | 100.0% | 0.5% |
| RiskCalculator | 4114 | 943.6% | 4.7% |
| Risk_Accepted | 49 | 1.2% | 0.1% |
| TradeManager | 29 | 59.2% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 4065 señales, -98.8%)
- **Tasa de conversión final:** 0.03% (de 88141 zonas iniciales → 29 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,183 | 60.9% |
| ENTRY_TOO_FAR | 503 | 25.9% |
| TP_CHECK_FAIL | 136 | 7.0% |
| NO_SL | 122 | 6.3% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,183 rechazos, 60.9%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2075 | 95.8% |
| P0_ANY_DIR | 91 | 4.2% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 72.77 pts (máxima ganancia flotante)
- **MAE Promedio:** 19.03 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 5.17

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 24 | 82.8% |
| SL_FIRST (precio fue hacia SL) | 5 | 17.2% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 82.8%
- **Entradas Malas (MAE > MFE):** 17.2%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 29 | 24 | 5 | 82.8% | 72.77 | 19.03 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0026 | SELL | 70.50 | 17.25 | 4.09 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0026_2 | SELL | 70.50 | 17.25 | 4.09 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0026_3 | SELL | 70.50 | 17.25 | 4.09 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0026_4 | SELL | 70.50 | 17.25 | 4.09 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0026_5 | SELL | 70.50 | 17.25 | 4.09 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0026_6 | SELL | 70.50 | 17.25 | 4.09 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0026_7 | SELL | 70.50 | 17.25 | 4.09 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0026_8 | SELL | 70.50 | 17.25 | 4.09 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0026_9 | SELL | 70.50 | 17.25 | 4.09 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0026_10 | SELL | 70.50 | 17.25 | 4.09 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0026_11 | SELL | 70.50 | 17.25 | 4.09 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0026_12 | SELL | 70.50 | 17.25 | 4.09 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0026_13 | SELL | 70.50 | 17.25 | 4.09 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0026_14 | SELL | 70.50 | 17.25 | 4.09 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0026_15 | SELL | 70.50 | 17.25 | 4.09 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0026_16 | SELL | 70.50 | 17.25 | 4.09 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0026_17 | SELL | 70.50 | 17.25 | 4.09 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0030 | SELL | 121.00 | 26.25 | 4.61 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0031 | SELL | 183.50 | 19.00 | 9.66 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0032 | SELL | 245.50 | 14.25 | 17.23 | SL_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 841

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 285 | 34.4% | 47.0% | 17.87 | 56.1% | 2.46 | ⚠️ CALIDAD MEDIA - Revisar |
| 2.0-3.0 ATR (Cerca) | 134 | 50.7% | 64.2% | 11.52 | 71.6% | 2.04 | ✅ BUENA CALIDAD - Considerar incluir |
| 3.0-5.0 ATR (Media) | 267 | 42.7% | 55.4% | 3.97 | 60.7% | 2.25 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 124 | 25.8% | 40.3% | 2.23 | 46.8% | 2.28 | ❌ BAJA CALIDAD - Filtro correcto |
| >10.0 ATR (Muy lejos) | 31 | 0.0% | 0.0% | 0.79 | 0.0% | 2.28 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (285 oportunidades)

- **WR Teórico:** 34.4% (si se hubieran ejecutado)
- **TP_FIRST:** 47.0% (134 de 285)
- **SL_FIRST:** 50.2% (143 de 285)
- **MFE Promedio:** 62.85 pts
- **MAE Promedio:** 40.83 pts
- **MFE/MAE Ratio:** 17.87
- **Good Entries:** 56.1% (MFE > MAE)
- **R:R Promedio:** 2.46

**⚠️ CALIDAD MEDIA - Revisar**

**2.0-3.0 ATR (Cerca)** (134 oportunidades)

- **WR Teórico:** 50.7% (si se hubieran ejecutado)
- **TP_FIRST:** 64.2% (86 de 134)
- **SL_FIRST:** 31.3% (42 de 134)
- **MFE Promedio:** 61.36 pts
- **MAE Promedio:** 22.78 pts
- **MFE/MAE Ratio:** 11.52
- **Good Entries:** 71.6% (MFE > MAE)
- **R:R Promedio:** 2.04

**✅ BUENA CALIDAD - Considerar incluir**

**3.0-5.0 ATR (Media)** (267 oportunidades)

- **WR Teórico:** 42.7% (si se hubieran ejecutado)
- **TP_FIRST:** 55.4% (148 de 267)
- **SL_FIRST:** 37.1% (99 de 267)
- **MFE Promedio:** 81.37 pts
- **MAE Promedio:** 29.04 pts
- **MFE/MAE Ratio:** 3.97
- **Good Entries:** 60.7% (MFE > MAE)
- **R:R Promedio:** 2.25

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (124 oportunidades)

- **WR Teórico:** 25.8% (si se hubieran ejecutado)
- **TP_FIRST:** 40.3% (50 de 124)
- **SL_FIRST:** 54.8% (68 de 124)
- **MFE Promedio:** 55.89 pts
- **MAE Promedio:** 37.76 pts
- **MFE/MAE Ratio:** 2.23
- **Good Entries:** 46.8% (MFE > MAE)
- **R:R Promedio:** 2.28

**❌ BAJA CALIDAD - Filtro correcto**

**>10.0 ATR (Muy lejos)** (31 oportunidades)

- **WR Teórico:** 0.0% (si se hubieran ejecutado)
- **TP_FIRST:** 0.0% (0 de 31)
- **SL_FIRST:** 100.0% (31 de 31)
- **MFE Promedio:** 35.09 pts
- **MAE Promedio:** 44.15 pts
- **MFE/MAE Ratio:** 0.79
- **Good Entries:** 0.0% (MFE > MAE)
- **R:R Promedio:** 2.28

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 29 | 285 | 134 | 267 |
| **TP_FIRST %** | 82.8% | 47.0% | 64.2% | 55.4% |
| **Good Entries %** | 82.8% | 56.1% | 71.6% | 60.7% |
| **MFE/MAE Ratio** | 5.17 | 17.87 | 11.52 | 3.97 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

🚨 **CRÍTICO: El rango 2.0-3.0 ATR contiene 134 oportunidades de BUENA CALIDAD**
   - WR Teórico: 50.7%
   - Good Entries: 71.6%
   - **ACCIÓN:** Considerar aumentar MaxDistanceToRegister_ATR_Normal de 2.0 a 3.0

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 267 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 42.7%
   - Good Entries: 60.7%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 17.08 pts
- **Mediana:** 19.61 pts
- **Min/Max:** 2.46 / 24.46 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 19.61 |
| P70 | 19.61 |
| P80 | 19.61 |
| P90 | 19.61 |
| P95 | 22.04 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 27.48 pts
- **Mediana:** 28.25 pts
- **Min/Max:** 6.75 / 46.25 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 28.25 |
| P70 | 28.25 |
| P80 | 28.25 |
| P90 | 40.00 |
| P95 | 43.12 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 19; // Era 60
public int MaxTPDistancePoints { get; set; } = 40; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 19.6pts, TP: 40.0pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (41.4%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.414) / 0.414
R:R_min = 1.42
```

**Estado actual:** R:R promedio = 1.69
**Gap:** -0.27 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **19** (P90 real)
2. **MaxTPDistancePoints:** 120 → **40** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.69) < R:R mínimo (1.42)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=41.4%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-17 09:41:16*