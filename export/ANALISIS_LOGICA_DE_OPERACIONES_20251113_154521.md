# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-13 15:48:50
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_154521.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251113_154521.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 37
- **Win Rate:** 29.7% (11/37)
- **Profit Factor:** 1.01
- **Avg R:R Planeado:** 2.72
- **R:R Mínimo para Break-Even:** 2.36

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 40 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.72
   - R:R necesario: 2.36
   - **Gap:** -0.36

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8059 | 34.5% |
| Bearish | 6266 | 26.8% |
| Bullish | 9026 | 38.7% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.081
- **Score Min/Max:** [-0.990, 0.970]
- **Componentes (promedio):**
  - EMA20 Slope: 0.042
  - EMA50 Cross: 0.189
  - BOS Count: 0.011
  - Regression 24h: 0.090

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.970 (apenas supera threshold)
- Score mínimo observado: -0.990 (apenas supera threshold)
- **Consecuencia:** Sistema queda 34.5% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.5%)

**Posibles causas:**
- **BOS Score bajo (0.011):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio 0.081 indica poca señal direccional
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
| StructureFusion | 10362 | 100.0% | 100.0% |
| ProximityAnalyzer | 4136 | 39.9% | 39.9% |
| DFM_Evaluated | 842 | 20.4% | 8.1% |
| DFM_Passed | 842 | 100.0% | 8.1% |
| RiskCalculator | 6364 | 755.8% | 61.4% |
| Risk_Accepted | 2 | 0.0% | 0.0% |
| TradeManager | 37 | 1850.0% | 0.4% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 6362 señales, -100.0%)
- **Tasa de conversión final:** 0.36% (de 10362 zonas iniciales → 37 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,672 | 58.1% |
| NO_SL | 559 | 19.4% |
| ENTRY_TOO_FAR | 387 | 13.4% |
| TP_CHECK_FAIL | 260 | 9.0% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,672 rechazos, 58.1%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2521 | 89.0% |
| P0_ANY_DIR | 310 | 11.0% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 45.00 pts (máxima ganancia flotante)
- **MAE Promedio:** 27.82 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 114.25

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 24 | 64.9% |
| SL_FIRST (precio fue hacia SL) | 11 | 29.7% |
| NEUTRAL (sin dirección clara) | 2 | 5.4% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 54.1%
- **Entradas Malas (MAE > MFE):** 45.9%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 37 | 24 | 11 | 64.9% | 45.00 | 27.82 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | BUY | 17.75 | 37.75 | 0.47 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0004 | SELL | 9.50 | 6.00 | 1.58 | NEUTRAL | CLOSED | ✅ Entrada excelente |
| T0005 | SELL | 9.50 | 6.25 | 1.52 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0006 | SELL | 3.50 | 35.00 | 0.10 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0013 | BUY | 1.75 | 11.00 | 0.16 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0015 | BUY | 14.50 | 26.50 | 0.55 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0016 | SELL | 42.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0016_2 | SELL | 42.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0017 | SELL | 2.25 | 13.00 | 0.17 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0019 | SELL | 166.00 | 14.00 | 11.86 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0019_2 | SELL | 166.00 | 14.00 | 11.86 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0020 | SELL | 248.50 | 11.25 | 22.09 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0024 | SELL | 0.00 | 68.50 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0025 | SELL | 33.75 | 18.00 | 1.88 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0029 | BUY | 51.50 | 63.75 | 0.81 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0030 | BUY | 9.50 | 66.75 | 0.14 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0035 | SELL | 55.25 | 58.00 | 0.95 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0036 | SELL | 36.00 | 75.50 | 0.48 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0047 | BUY | 27.00 | 4.00 | 6.75 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049 | SELL | 83.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,317

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 480 | 38.3% | 61.0% | 4.23 | 47.3% | 2.05 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 248 | 52.0% | 53.2% | 1.88 | 49.6% | 1.91 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 361 | 66.5% | 56.0% | 2.26 | 58.2% | 2.05 | ⚠️ CALIDAD MEDIA - Revisar |
| 5.0-10.0 ATR (Lejos) | 213 | 70.0% | 66.2% | 0.73 | 67.1% | 2.36 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 15 | 46.7% | 33.3% | 0.12 | 33.3% | 3.61 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (480 oportunidades)

- **WR Teórico:** 38.3% (si se hubieran ejecutado)
- **TP_FIRST:** 61.0% (293 de 480)
- **SL_FIRST:** 36.2% (174 de 480)
- **MFE Promedio:** 49.84 pts
- **MAE Promedio:** 41.77 pts
- **MFE/MAE Ratio:** 4.23
- **Good Entries:** 47.3% (MFE > MAE)
- **R:R Promedio:** 2.05

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (248 oportunidades)

- **WR Teórico:** 52.0% (si se hubieran ejecutado)
- **TP_FIRST:** 53.2% (132 de 248)
- **SL_FIRST:** 46.8% (116 de 248)
- **MFE Promedio:** 57.08 pts
- **MAE Promedio:** 44.84 pts
- **MFE/MAE Ratio:** 1.88
- **Good Entries:** 49.6% (MFE > MAE)
- **R:R Promedio:** 1.91

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (361 oportunidades)

- **WR Teórico:** 66.5% (si se hubieran ejecutado)
- **TP_FIRST:** 56.0% (202 de 361)
- **SL_FIRST:** 44.0% (159 de 361)
- **MFE Promedio:** 72.00 pts
- **MAE Promedio:** 46.54 pts
- **MFE/MAE Ratio:** 2.26
- **Good Entries:** 58.2% (MFE > MAE)
- **R:R Promedio:** 2.05

**⚠️ CALIDAD MEDIA - Revisar**

**5.0-10.0 ATR (Lejos)** (213 oportunidades)

- **WR Teórico:** 70.0% (si se hubieran ejecutado)
- **TP_FIRST:** 66.2% (141 de 213)
- **SL_FIRST:** 32.9% (70 de 213)
- **MFE Promedio:** 80.22 pts
- **MAE Promedio:** 58.95 pts
- **MFE/MAE Ratio:** 0.73
- **Good Entries:** 67.1% (MFE > MAE)
- **R:R Promedio:** 2.36

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (15 oportunidades)

- **WR Teórico:** 46.7% (si se hubieran ejecutado)
- **TP_FIRST:** 33.3% (5 de 15)
- **SL_FIRST:** 66.7% (10 de 15)
- **MFE Promedio:** 88.21 pts
- **MAE Promedio:** 64.33 pts
- **MFE/MAE Ratio:** 0.12
- **Good Entries:** 33.3% (MFE > MAE)
- **R:R Promedio:** 3.61

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 37 | 480 | 248 | 361 |
| **TP_FIRST %** | 64.9% | 61.0% | 53.2% | 56.0% |
| **Good Entries %** | 54.1% | 47.3% | 49.6% | 58.2% |
| **MFE/MAE Ratio** | 114.25 | 4.23 | 1.88 | 2.26 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 361 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 66.5%
   - Good Entries: 58.2%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 11.59 pts
- **Mediana:** 8.06 pts
- **Min/Max:** 0.83 / 40.44 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 8.06 |
| P70 | 15.61 |
| P80 | 20.06 |
| P90 | 24.68 |
| P95 | 37.80 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 23.27 pts
- **Mediana:** 19.50 pts
- **Min/Max:** 4.50 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 19.50 |
| P70 | 30.40 |
| P80 | 39.40 |
| P90 | 50.45 |
| P95 | 53.27 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 24; // Era 60
public int MaxTPDistancePoints { get; set; } = 50; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 24.7pts, TP: 50.5pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (29.7%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.297) / 0.297
R:R_min = 2.36
```

**Estado actual:** R:R promedio = 2.72
**Gap:** -0.36 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **24** (P90 real)
2. **MaxTPDistancePoints:** 120 → **50** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.72) < R:R mínimo (2.36)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=29.7%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-13 15:48:50*