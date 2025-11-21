# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-13 08:33:42
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_082924.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251113_082924.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 36
- **Win Rate:** 33.3% (12/36)
- **Profit Factor:** 1.05
- **Avg R:R Planeado:** 2.59
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
   - R:R actual: 2.59
   - R:R necesario: 2.00
   - **Gap:** -0.59

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8035 | 34.5% |
| Bearish | 6273 | 26.9% |
| Bullish | 9014 | 38.7% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.081
- **Score Min/Max:** [-0.990, 0.970]
- **Componentes (promedio):**
  - EMA20 Slope: 0.043
  - EMA50 Cross: 0.188
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
| StructureFusion | 10316 | 100.0% | 100.0% |
| ProximityAnalyzer | 4156 | 40.3% | 40.3% |
| DFM_Evaluated | 857 | 20.6% | 8.3% |
| DFM_Passed | 857 | 100.0% | 8.3% |
| RiskCalculator | 6303 | 735.5% | 61.1% |
| Risk_Accepted | 106 | 1.7% | 1.0% |
| TradeManager | 36 | 34.0% | 0.3% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 6197 señales, -98.3%)
- **Tasa de conversión final:** 0.35% (de 10316 zonas iniciales → 36 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,656 | 58.9% |
| NO_SL | 506 | 18.0% |
| ENTRY_TOO_FAR | 386 | 13.7% |
| TP_CHECK_FAIL | 263 | 9.4% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,656 rechazos, 58.9%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2561 | 89.1% |
| P0_ANY_DIR | 314 | 10.9% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 43.92 pts (máxima ganancia flotante)
- **MAE Promedio:** 29.46 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 61.96

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 24 | 66.7% |
| SL_FIRST (precio fue hacia SL) | 11 | 30.6% |
| NEUTRAL (sin dirección clara) | 1 | 2.8% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 47.2%
- **Entradas Malas (MAE > MFE):** 52.8%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 36 | 24 | 11 | 66.7% | 43.92 | 29.46 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0002 | BUY | 22.25 | 6.50 | 3.42 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0003 | SELL | 7.00 | 21.75 | 0.32 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0009 | SELL | 13.00 | 18.75 | 0.69 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0011 | SELL | 0.00 | 43.00 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0013 | BUY | 1.75 | 11.00 | 0.16 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0015 | BUY | 14.50 | 26.50 | 0.55 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0016 | SELL | 42.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0017 | SELL | 2.25 | 13.00 | 0.17 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0019 | SELL | 166.00 | 14.00 | 11.86 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0019_2 | SELL | 166.00 | 14.00 | 11.86 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0020 | SELL | 248.50 | 11.25 | 22.09 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0024 | SELL | 0.00 | 68.50 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0025 | SELL | 33.75 | 18.00 | 1.88 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0029 | BUY | 51.50 | 63.75 | 0.81 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0030 | BUY | 9.50 | 66.75 | 0.14 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0034 | SELL | 55.25 | 58.00 | 0.95 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0035 | SELL | 36.00 | 75.50 | 0.48 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0046 | BUY | 27.00 | 4.00 | 6.75 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0048 | SELL | 83.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049 | SELL | 26.75 | 40.75 | 0.66 | TP_FIRST | CLOSED | ❌ Entrada muy mala |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,315

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 482 | 39.0% | 60.4% | 3.93 | 45.0% | 2.02 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 249 | 49.4% | 52.6% | 1.69 | 47.0% | 1.93 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 365 | 69.6% | 57.3% | 2.88 | 60.8% | 2.07 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 203 | 69.0% | 64.0% | 0.72 | 66.5% | 2.38 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 16 | 56.2% | 43.8% | 0.13 | 43.8% | 3.53 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (482 oportunidades)

- **WR Teórico:** 39.0% (si se hubieran ejecutado)
- **TP_FIRST:** 60.4% (291 de 482)
- **SL_FIRST:** 37.6% (181 de 482)
- **MFE Promedio:** 49.27 pts
- **MAE Promedio:** 38.99 pts
- **MFE/MAE Ratio:** 3.93
- **Good Entries:** 45.0% (MFE > MAE)
- **R:R Promedio:** 2.02

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (249 oportunidades)

- **WR Teórico:** 49.4% (si se hubieran ejecutado)
- **TP_FIRST:** 52.6% (131 de 249)
- **SL_FIRST:** 47.4% (118 de 249)
- **MFE Promedio:** 56.92 pts
- **MAE Promedio:** 42.31 pts
- **MFE/MAE Ratio:** 1.69
- **Good Entries:** 47.0% (MFE > MAE)
- **R:R Promedio:** 1.93

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (365 oportunidades)

- **WR Teórico:** 69.6% (si se hubieran ejecutado)
- **TP_FIRST:** 57.3% (209 de 365)
- **SL_FIRST:** 42.7% (156 de 365)
- **MFE Promedio:** 69.49 pts
- **MAE Promedio:** 42.66 pts
- **MFE/MAE Ratio:** 2.88
- **Good Entries:** 60.8% (MFE > MAE)
- **R:R Promedio:** 2.07

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (203 oportunidades)

- **WR Teórico:** 69.0% (si se hubieran ejecutado)
- **TP_FIRST:** 64.0% (130 de 203)
- **SL_FIRST:** 36.0% (73 de 203)
- **MFE Promedio:** 80.29 pts
- **MAE Promedio:** 52.31 pts
- **MFE/MAE Ratio:** 0.72
- **Good Entries:** 66.5% (MFE > MAE)
- **R:R Promedio:** 2.38

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (16 oportunidades)

- **WR Teórico:** 56.2% (si se hubieran ejecutado)
- **TP_FIRST:** 43.8% (7 de 16)
- **SL_FIRST:** 56.2% (9 de 16)
- **MFE Promedio:** 77.33 pts
- **MAE Promedio:** 67.75 pts
- **MFE/MAE Ratio:** 0.13
- **Good Entries:** 43.8% (MFE > MAE)
- **R:R Promedio:** 3.53

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 36 | 482 | 249 | 365 |
| **TP_FIRST %** | 66.7% | 60.4% | 52.6% | 57.3% |
| **Good Entries %** | 47.2% | 45.0% | 47.0% | 60.8% |
| **MFE/MAE Ratio** | 61.96 | 3.93 | 1.69 | 2.88 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 365 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 69.6%
   - Good Entries: 60.8%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 11.51 pts
- **Mediana:** 8.03 pts
- **Min/Max:** 0.55 / 40.44 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 8.03 |
| P70 | 16.22 |
| P80 | 20.66 |
| P90 | 25.12 |
| P95 | 37.95 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 22.62 pts
- **Mediana:** 17.88 pts
- **Min/Max:** 3.25 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 17.88 |
| P70 | 32.35 |
| P80 | 39.60 |
| P90 | 50.67 |
| P95 | 53.29 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 25; // Era 60
public int MaxTPDistancePoints { get; set; } = 50; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 25.1pts, TP: 50.7pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (33.3%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.333) / 0.333
R:R_min = 2.00
```

**Estado actual:** R:R promedio = 2.59
**Gap:** -0.59 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **25** (P90 real)
2. **MaxTPDistancePoints:** 120 → **50** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.59) < R:R mínimo (2.00)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=33.3%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-13 08:33:42*