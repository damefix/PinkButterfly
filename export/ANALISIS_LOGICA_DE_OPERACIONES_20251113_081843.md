# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-13 08:24:15
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_081843.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251113_081843.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 45
- **Win Rate:** 35.6% (16/45)
- **Profit Factor:** 0.77
- **Avg R:R Planeado:** 1.88
- **R:R Mínimo para Break-Even:** 1.81

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 42 puntos
   - TP máximo observado: 55 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.88
   - R:R necesario: 1.81
   - **Gap:** -0.06

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8034 | 34.4% |
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
- **Consecuencia:** Sistema queda 34.4% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.4%)

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
| StructureFusion | 10198 | 100.0% | 100.0% |
| ProximityAnalyzer | 4078 | 40.0% | 40.0% |
| DFM_Evaluated | 862 | 21.1% | 8.5% |
| DFM_Passed | 862 | 100.0% | 8.5% |
| RiskCalculator | 6315 | 732.6% | 61.9% |
| Risk_Accepted | 108 | 1.7% | 1.1% |
| TradeManager | 45 | 41.7% | 0.4% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 6207 señales, -98.3%)
- **Tasa de conversión final:** 0.44% (de 10198 zonas iniciales → 45 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,690 | 59.0% |
| NO_SL | 499 | 17.4% |
| ENTRY_TOO_FAR | 403 | 14.1% |
| TP_CHECK_FAIL | 271 | 9.5% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,690 rechazos, 59.0%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2606 | 89.3% |
| P0_ANY_DIR | 313 | 10.7% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 50.11 pts (máxima ganancia flotante)
- **MAE Promedio:** 28.09 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 91.27

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 26 | 57.8% |
| SL_FIRST (precio fue hacia SL) | 18 | 40.0% |
| NEUTRAL (sin dirección clara) | 1 | 2.2% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 51.1%
- **Entradas Malas (MAE > MFE):** 48.9%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 45 | 26 | 18 | 57.8% | 50.11 | 28.09 |

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
| T0005 | BUY | 21.25 | 26.75 | 0.79 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0009 | SELL | 13.00 | 18.75 | 0.69 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0014 | BUY | 10.00 | 28.25 | 0.35 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0014_2 | BUY | 10.00 | 28.25 | 0.35 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0015 | SELL | 42.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0015_2 | SELL | 42.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0016 | SELL | 1.25 | 13.00 | 0.10 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0018 | SELL | 137.50 | 27.75 | 4.95 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0019 | SELL | 160.25 | 27.75 | 5.77 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0020 | SELL | 244.50 | 15.25 | 16.03 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0021 | SELL | 238.50 | 18.75 | 12.72 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0021_2 | SELL | 238.50 | 18.75 | 12.72 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0024 | SELL | 0.00 | 68.50 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0027 | BUY | 54.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0030 | BUY | 9.50 | 66.75 | 0.14 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0031 | BUY | 8.25 | 66.75 | 0.12 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0033 | BUY | 14.25 | 104.25 | 0.14 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0037 | SELL | 55.25 | 58.00 | 0.95 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,262

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 454 | 35.0% | 56.4% | 2.97 | 43.6% | 1.97 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 246 | 45.1% | 49.6% | 2.33 | 45.9% | 1.94 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 341 | 68.9% | 54.5% | 3.66 | 63.0% | 1.96 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 208 | 71.2% | 63.5% | 1.11 | 72.1% | 2.08 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 13 | 53.8% | 38.5% | 0.38 | 53.8% | 2.71 | ⚠️ CALIDAD MEDIA - Revisar |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (454 oportunidades)

- **WR Teórico:** 35.0% (si se hubieran ejecutado)
- **TP_FIRST:** 56.4% (256 de 454)
- **SL_FIRST:** 41.4% (188 de 454)
- **MFE Promedio:** 43.55 pts
- **MAE Promedio:** 37.39 pts
- **MFE/MAE Ratio:** 2.97
- **Good Entries:** 43.6% (MFE > MAE)
- **R:R Promedio:** 1.97

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (246 oportunidades)

- **WR Teórico:** 45.1% (si se hubieran ejecutado)
- **TP_FIRST:** 49.6% (122 de 246)
- **SL_FIRST:** 50.0% (123 de 246)
- **MFE Promedio:** 49.24 pts
- **MAE Promedio:** 39.26 pts
- **MFE/MAE Ratio:** 2.33
- **Good Entries:** 45.9% (MFE > MAE)
- **R:R Promedio:** 1.94

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (341 oportunidades)

- **WR Teórico:** 68.9% (si se hubieran ejecutado)
- **TP_FIRST:** 54.5% (186 de 341)
- **SL_FIRST:** 45.5% (155 de 341)
- **MFE Promedio:** 70.83 pts
- **MAE Promedio:** 39.28 pts
- **MFE/MAE Ratio:** 3.66
- **Good Entries:** 63.0% (MFE > MAE)
- **R:R Promedio:** 1.96

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (208 oportunidades)

- **WR Teórico:** 71.2% (si se hubieran ejecutado)
- **TP_FIRST:** 63.5% (132 de 208)
- **SL_FIRST:** 36.1% (75 de 208)
- **MFE Promedio:** 78.41 pts
- **MAE Promedio:** 40.11 pts
- **MFE/MAE Ratio:** 1.11
- **Good Entries:** 72.1% (MFE > MAE)
- **R:R Promedio:** 2.08

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (13 oportunidades)

- **WR Teórico:** 53.8% (si se hubieran ejecutado)
- **TP_FIRST:** 38.5% (5 de 13)
- **SL_FIRST:** 61.5% (8 de 13)
- **MFE Promedio:** 78.29 pts
- **MAE Promedio:** 52.22 pts
- **MFE/MAE Ratio:** 0.38
- **Good Entries:** 53.8% (MFE > MAE)
- **R:R Promedio:** 2.71

**⚠️ CALIDAD MEDIA - Revisar**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 45 | 454 | 246 | 341 |
| **TP_FIRST %** | 57.8% | 56.4% | 49.6% | 54.5% |
| **Good Entries %** | 51.1% | 43.6% | 45.9% | 63.0% |
| **MFE/MAE Ratio** | 91.27 | 2.97 | 2.33 | 3.66 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 341 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 68.9%
   - Good Entries: 63.0%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 14.22 pts
- **Mediana:** 10.27 pts
- **Min/Max:** 0.55 / 41.65 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 10.27 |
| P70 | 18.23 |
| P80 | 21.37 |
| P90 | 27.35 |
| P95 | 39.56 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 25.09 pts
- **Mediana:** 19.50 pts
- **Min/Max:** 3.25 / 55.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 19.50 |
| P70 | 37.55 |
| P80 | 44.75 |
| P90 | 50.60 |
| P95 | 53.42 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 27; // Era 60
public int MaxTPDistancePoints { get; set; } = 50; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 27.3pts, TP: 50.6pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (35.6%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.356) / 0.356
R:R_min = 1.81
```

**Estado actual:** R:R promedio = 1.88
**Gap:** -0.06 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **27** (P90 real)
2. **MaxTPDistancePoints:** 120 → **50** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.88) < R:R mínimo (1.81)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=35.6%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-13 08:24:15*