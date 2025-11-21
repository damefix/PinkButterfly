# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-18 12:14:59
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251118_121008.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251118_121008.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 24
- **Win Rate:** 33.3% (8/24)
- **Profit Factor:** 0.65
- **Avg R:R Planeado:** 2.29
- **R:R Mínimo para Break-Even:** 2.00

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 26 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.29
   - R:R necesario: 2.00
   - **Gap:** -0.29

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 924 | 29.5% |
| Bearish | 942 | 30.1% |
| Bullish | 1261 | 40.3% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.075
- **Score Min/Max:** [-1.000, 1.000]
- **Componentes (promedio):**
  - EMA20 Slope: 0.028
  - EMA50 Cross: 0.077
  - BOS Count: 0.098
  - Regression 24h: 0.114

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 1.000 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 29.5% Neutral → bias no diferencia tendencias

### 2.3 Estado: Bias Compuesto Funcionando Correctamente

✅ **Threshold actual:** 0.3 (correcto)
✅ **Bias Neutral:** 29.5% (aceptable)
✅ **Score promedio:** 0.075

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 12301 | 100.0% | 100.0% |
| ProximityAnalyzer | 2836 | 23.1% | 23.1% |
| DFM_Evaluated | 462 | 16.3% | 3.8% |
| DFM_Passed | 462 | 100.0% | 3.8% |
| RiskCalculator | 5448 | 1179.2% | 44.3% |
| Risk_Accepted | 60 | 1.1% | 0.5% |
| TradeManager | 24 | 40.0% | 0.2% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 5388 señales, -98.9%)
- **Tasa de conversión final:** 0.20% (de 12301 zonas iniciales → 24 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,445 | 42.0% |
| TP_CHECK_FAIL | 996 | 29.0% |
| NO_SL | 611 | 17.8% |
| ENTRY_TOO_FAR | 387 | 11.3% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,445 rechazos, 42.0%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2184 | 89.0% |
| P0_ANY_DIR | 270 | 11.0% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 34.83 pts (máxima ganancia flotante)
- **MAE Promedio:** 24.55 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 86.68

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 13 | 54.2% |
| SL_FIRST (precio fue hacia SL) | 10 | 41.7% |
| NEUTRAL (sin dirección clara) | 1 | 4.2% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 62.5%
- **Entradas Malas (MAE > MFE):** 37.5%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 24 | 13 | 10 | 54.2% | 34.83 | 24.55 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0002 | SELL | 7.75 | 17.75 | 0.44 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0003 | SELL | 2.50 | 28.75 | 0.09 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0005 | SELL | 15.75 | 15.75 | 1.00 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0009 | BUY | 44.50 | 53.75 | 0.83 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0016 | SELL | 78.00 | 13.00 | 6.00 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0017 | SELL | 78.00 | 13.00 | 6.00 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0023 | SELL | 59.00 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0026 | SELL | 24.25 | 61.75 | 0.39 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0029 | SELL | 36.25 | 3.00 | 12.08 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0030 | SELL | 36.25 | 2.25 | 16.11 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0031 | SELL | 50.00 | 2.25 | 22.22 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0033 | SELL | 113.25 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0040 | SELL | 43.00 | 38.00 | 1.13 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0040_2 | SELL | 43.00 | 38.00 | 1.13 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0040_3 | SELL | 43.00 | 38.00 | 1.13 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0043 | SELL | 16.50 | 5.50 | 3.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0045 | BUY | 16.00 | 6.25 | 2.56 | NEUTRAL | CLOSED | ✅ Entrada excelente |
| T0046 | SELL | 21.00 | 11.25 | 1.87 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0051 | SELL | 41.75 | 10.50 | 3.98 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0055 | SELL | 0.00 | 103.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 740

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 269 | 34.9% | 52.8% | 5.01 | 52.8% | 2.32 | ⚠️ CALIDAD MEDIA - Revisar |
| 2.0-3.0 ATR (Cerca) | 136 | 46.3% | 56.6% | 3.01 | 57.4% | 2.50 | ⚠️ CALIDAD MEDIA - Revisar |
| 3.0-5.0 ATR (Media) | 208 | 70.2% | 63.9% | 2.07 | 68.3% | 2.49 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 116 | 71.6% | 64.7% | 1.39 | 66.4% | 3.49 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 11 | 0.0% | 0.0% | 0.00 | 0.0% | 6.11 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (269 oportunidades)

- **WR Teórico:** 34.9% (si se hubieran ejecutado)
- **TP_FIRST:** 52.8% (142 de 269)
- **SL_FIRST:** 45.0% (121 de 269)
- **MFE Promedio:** 48.78 pts
- **MAE Promedio:** 35.38 pts
- **MFE/MAE Ratio:** 5.01
- **Good Entries:** 52.8% (MFE > MAE)
- **R:R Promedio:** 2.32

**⚠️ CALIDAD MEDIA - Revisar**

**2.0-3.0 ATR (Cerca)** (136 oportunidades)

- **WR Teórico:** 46.3% (si se hubieran ejecutado)
- **TP_FIRST:** 56.6% (77 de 136)
- **SL_FIRST:** 43.4% (59 de 136)
- **MFE Promedio:** 62.92 pts
- **MAE Promedio:** 39.79 pts
- **MFE/MAE Ratio:** 3.01
- **Good Entries:** 57.4% (MFE > MAE)
- **R:R Promedio:** 2.50

**⚠️ CALIDAD MEDIA - Revisar**

**3.0-5.0 ATR (Media)** (208 oportunidades)

- **WR Teórico:** 70.2% (si se hubieran ejecutado)
- **TP_FIRST:** 63.9% (133 de 208)
- **SL_FIRST:** 36.1% (75 de 208)
- **MFE Promedio:** 85.96 pts
- **MAE Promedio:** 47.61 pts
- **MFE/MAE Ratio:** 2.07
- **Good Entries:** 68.3% (MFE > MAE)
- **R:R Promedio:** 2.49

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (116 oportunidades)

- **WR Teórico:** 71.6% (si se hubieran ejecutado)
- **TP_FIRST:** 64.7% (75 de 116)
- **SL_FIRST:** 35.3% (41 de 116)
- **MFE Promedio:** 115.12 pts
- **MAE Promedio:** 68.38 pts
- **MFE/MAE Ratio:** 1.39
- **Good Entries:** 66.4% (MFE > MAE)
- **R:R Promedio:** 3.49

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (11 oportunidades)

- **WR Teórico:** 0.0% (si se hubieran ejecutado)
- **TP_FIRST:** 0.0% (0 de 11)
- **SL_FIRST:** 100.0% (11 de 11)
- **MFE Promedio:** 0.00 pts
- **MAE Promedio:** 86.39 pts
- **MFE/MAE Ratio:** 0.00
- **Good Entries:** 0.0% (MFE > MAE)
- **R:R Promedio:** 6.11

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 24 | 269 | 136 | 208 |
| **TP_FIRST %** | 54.2% | 52.8% | 56.6% | 63.9% |
| **Good Entries %** | 62.5% | 52.8% | 57.4% | 68.3% |
| **MFE/MAE Ratio** | 86.68 | 5.01 | 3.01 | 2.07 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 208 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 70.2%
   - Good Entries: 68.3%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 13.86 pts
- **Mediana:** 14.74 pts
- **Min/Max:** 3.09 / 26.24 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 14.74 |
| P70 | 19.58 |
| P80 | 21.19 |
| P90 | 21.68 |
| P95 | 25.16 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 28.07 pts
- **Mediana:** 32.25 pts
- **Min/Max:** 8.75 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 32.25 |
| P70 | 36.50 |
| P80 | 38.75 |
| P90 | 49.50 |
| P95 | 53.25 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 21; // Era 60
public int MaxTPDistancePoints { get; set; } = 49; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 21.7pts, TP: 49.5pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (33.3%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.333) / 0.333
R:R_min = 2.00
```

**Estado actual:** R:R promedio = 2.29
**Gap:** -0.29 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **21** (P90 real)
2. **MaxTPDistancePoints:** 120 → **49** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.29) < R:R mínimo (2.00)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=33.3%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-18 12:14:59*