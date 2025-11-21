# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-14 09:07:39
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_090348.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251114_090348.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 39
- **Win Rate:** 30.8% (12/39)
- **Profit Factor:** 0.78
- **Avg R:R Planeado:** 2.14
- **R:R Mínimo para Break-Even:** 2.25

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 40 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.14
   - R:R necesario: 2.25
   - **Gap:** 0.11

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 7964 | 34.1% |
| Bearish | 6325 | 27.1% |
| Bullish | 9035 | 38.7% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.079
- **Score Min/Max:** [-0.990, 0.970]
- **Componentes (promedio):**
  - EMA20 Slope: 0.040
  - EMA50 Cross: 0.191
  - BOS Count: 0.007
  - Regression 24h: 0.088

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.970 (apenas supera threshold)
- Score mínimo observado: -0.990 (apenas supera threshold)
- **Consecuencia:** Sistema queda 34.1% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.1%)

**Posibles causas:**
- **BOS Score bajo (0.007):** BOS/CHoCH no se detectan correctamente
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
| StructureFusion | 10407 | 100.0% | 100.0% |
| ProximityAnalyzer | 4222 | 40.6% | 40.6% |
| DFM_Evaluated | 831 | 19.7% | 8.0% |
| DFM_Passed | 831 | 100.0% | 8.0% |
| RiskCalculator | 6405 | 770.8% | 61.5% |
| Risk_Accepted | 2 | 0.0% | 0.0% |
| TradeManager | 39 | 1950.0% | 0.4% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 6403 señales, -100.0%)
- **Tasa de conversión final:** 0.37% (de 10407 zonas iniciales → 39 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,673 | 58.5% |
| NO_SL | 538 | 18.8% |
| ENTRY_TOO_FAR | 409 | 14.3% |
| TP_CHECK_FAIL | 239 | 8.4% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,673 rechazos, 58.5%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2532 | 88.6% |
| P0_ANY_DIR | 326 | 11.4% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 32.64 pts (máxima ganancia flotante)
- **MAE Promedio:** 29.74 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 53.10

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 22 | 56.4% |
| SL_FIRST (precio fue hacia SL) | 16 | 41.0% |
| NEUTRAL (sin dirección clara) | 1 | 2.6% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 43.6%
- **Entradas Malas (MAE > MFE):** 56.4%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 39 | 22 | 16 | 56.4% | 32.64 | 29.74 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0003 | SELL | 9.75 | 22.00 | 0.44 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0004 | SELL | 2.50 | 36.00 | 0.07 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0005 | SELL | 8.75 | 29.75 | 0.29 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0010 | BUY | 2.50 | 11.25 | 0.22 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0010_2 | BUY | 2.50 | 11.25 | 0.22 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0010_3 | BUY | 2.50 | 11.25 | 0.22 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0010_4 | BUY | 2.50 | 11.25 | 0.22 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0011 | SELL | 144.75 | 20.50 | 7.06 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0012 | SELL | 245.50 | 14.25 | 17.23 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0016 | SELL | 1.25 | 68.50 | 0.02 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0016_2 | SELL | 1.25 | 68.50 | 0.02 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0018 | SELL | 10.50 | 46.50 | 0.23 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0022 | BUY | 48.50 | 63.75 | 0.76 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0022_2 | BUY | 48.50 | 63.75 | 0.76 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0024 | SELL | 0.00 | 40.75 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0026 | SELL | 55.25 | 58.00 | 0.95 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0027 | SELL | 19.75 | 87.50 | 0.23 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0028 | SELL | 17.00 | 63.25 | 0.27 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0034 | SELL | 14.50 | 19.75 | 0.73 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0035 | BUY | 6.50 | 31.50 | 0.21 | TP_FIRST | CLOSED | ❌ Entrada muy mala |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,313

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 464 | 38.6% | 64.4% | 4.89 | 48.7% | 2.01 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 253 | 57.7% | 54.5% | 3.89 | 55.7% | 1.86 | ⚠️ CALIDAD MEDIA - Revisar |
| 3.0-5.0 ATR (Media) | 347 | 70.0% | 57.9% | 3.97 | 65.1% | 2.04 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 236 | 75.8% | 72.5% | 2.25 | 74.6% | 2.31 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 13 | 46.2% | 30.8% | 0.13 | 30.8% | 3.06 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (464 oportunidades)

- **WR Teórico:** 38.6% (si se hubieran ejecutado)
- **TP_FIRST:** 64.4% (299 de 464)
- **SL_FIRST:** 31.9% (148 de 464)
- **MFE Promedio:** 55.22 pts
- **MAE Promedio:** 43.03 pts
- **MFE/MAE Ratio:** 4.89
- **Good Entries:** 48.7% (MFE > MAE)
- **R:R Promedio:** 2.01

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (253 oportunidades)

- **WR Teórico:** 57.7% (si se hubieran ejecutado)
- **TP_FIRST:** 54.5% (138 de 253)
- **SL_FIRST:** 45.5% (115 de 253)
- **MFE Promedio:** 76.49 pts
- **MAE Promedio:** 42.72 pts
- **MFE/MAE Ratio:** 3.89
- **Good Entries:** 55.7% (MFE > MAE)
- **R:R Promedio:** 1.86

**⚠️ CALIDAD MEDIA - Revisar**

**3.0-5.0 ATR (Media)** (347 oportunidades)

- **WR Teórico:** 70.0% (si se hubieran ejecutado)
- **TP_FIRST:** 57.9% (201 de 347)
- **SL_FIRST:** 40.6% (141 de 347)
- **MFE Promedio:** 85.49 pts
- **MAE Promedio:** 46.80 pts
- **MFE/MAE Ratio:** 3.97
- **Good Entries:** 65.1% (MFE > MAE)
- **R:R Promedio:** 2.04

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (236 oportunidades)

- **WR Teórico:** 75.8% (si se hubieran ejecutado)
- **TP_FIRST:** 72.5% (171 de 236)
- **SL_FIRST:** 26.7% (63 de 236)
- **MFE Promedio:** 93.72 pts
- **MAE Promedio:** 59.32 pts
- **MFE/MAE Ratio:** 2.25
- **Good Entries:** 74.6% (MFE > MAE)
- **R:R Promedio:** 2.31

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (13 oportunidades)

- **WR Teórico:** 46.2% (si se hubieran ejecutado)
- **TP_FIRST:** 30.8% (4 de 13)
- **SL_FIRST:** 69.2% (9 de 13)
- **MFE Promedio:** 91.79 pts
- **MAE Promedio:** 82.94 pts
- **MFE/MAE Ratio:** 0.13
- **Good Entries:** 30.8% (MFE > MAE)
- **R:R Promedio:** 3.06

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 39 | 464 | 253 | 347 |
| **TP_FIRST %** | 56.4% | 64.4% | 54.5% | 57.9% |
| **Good Entries %** | 43.6% | 48.7% | 55.7% | 65.1% |
| **MFE/MAE Ratio** | 53.10 | 4.89 | 3.89 | 3.97 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 347 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 70.0%
   - Good Entries: 65.1%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 12.85 pts
- **Mediana:** 8.13 pts
- **Min/Max:** 1.16 / 40.44 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 8.13 |
| P70 | 21.19 |
| P80 | 22.02 |
| P90 | 24.59 |
| P95 | 37.51 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 24.13 pts
- **Mediana:** 15.50 pts
- **Min/Max:** 5.00 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 15.50 |
| P70 | 36.50 |
| P80 | 43.50 |
| P90 | 50.75 |
| P95 | 53.25 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 24; // Era 60
public int MaxTPDistancePoints { get; set; } = 50; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 24.6pts, TP: 50.8pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (30.8%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.308) / 0.308
R:R_min = 2.25
```

**Estado actual:** R:R promedio = 2.14
**Gap:** 0.11 (necesitas mejorar R:R)

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

**Problema:** R:R actual (2.14) < R:R mínimo (2.25)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=30.8%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-14 09:07:39*