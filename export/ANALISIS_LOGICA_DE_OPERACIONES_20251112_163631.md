# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-12 16:40:05
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_163631.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251112_163631.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 57
- **Win Rate:** 29.8% (17/57)
- **Profit Factor:** 0.84
- **Avg R:R Planeado:** 2.27
- **R:R Mínimo para Break-Even:** 2.35

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 38 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.27
   - R:R necesario: 2.35
   - **Gap:** 0.09

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8085 | 34.6% |
| Bullish | 8977 | 38.4% |
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
| StructureFusion | 10198 | 100.0% | 100.0% |
| ProximityAnalyzer | 5357 | 52.5% | 52.5% |
| DFM_Evaluated | 1470 | 27.4% | 14.4% |
| DFM_Passed | 1227 | 83.5% | 12.0% |
| RiskCalculator | 7260 | 591.7% | 71.2% |
| Risk_Accepted | 158 | 2.2% | 1.5% |
| TradeManager | 57 | 36.1% | 0.6% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 7102 señales, -97.8%)
- **Tasa de conversión final:** 0.56% (de 10198 zonas iniciales → 57 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 2,057 | 62.6% |
| ENTRY_TOO_FAR | 473 | 14.4% |
| NO_SL | 433 | 13.2% |
| TP_CHECK_FAIL | 325 | 9.9% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (2,057 rechazos, 62.6%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2909 | 88.6% |
| P0_ANY_DIR | 373 | 11.4% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 34.66 pts (máxima ganancia flotante)
- **MAE Promedio:** 38.80 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 72.20

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 30 | 52.6% |
| SL_FIRST (precio fue hacia SL) | 26 | 45.6% |
| NEUTRAL (sin dirección clara) | 1 | 1.8% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 42.1%
- **Entradas Malas (MAE > MFE):** 57.9%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 57 | 30 | 26 | 52.6% | 34.66 | 38.80 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | BUY | 11.25 | 20.00 | 0.56 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0004 | SELL | 7.00 | 18.75 | 0.37 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0011 | SELL | 13.00 | 18.75 | 0.69 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0015 | BUY | 1.75 | 11.00 | 0.16 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0017 | BUY | 14.50 | 26.50 | 0.55 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0018 | BUY | 13.75 | 35.00 | 0.39 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0020 | SELL | 42.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0023 | SELL | 0.75 | 13.00 | 0.06 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0026 | SELL | 145.75 | 19.50 | 7.47 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0026_2 | SELL | 145.75 | 19.50 | 7.47 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0027 | SELL | 248.50 | 11.25 | 22.09 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0031 | BUY | 22.00 | 75.75 | 0.29 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0033 | SELL | 0.00 | 68.50 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0036 | SELL | 33.75 | 18.00 | 1.88 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0041 | BUY | 81.75 | 17.00 | 4.81 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0042 | BUY | 9.50 | 66.75 | 0.14 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0043 | BUY | 8.25 | 66.75 | 0.12 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0046 | BUY | 29.25 | 6.25 | 4.68 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0048 | BUY | 14.25 | 104.25 | 0.14 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0048_2 | BUY | 14.25 | 104.25 | 0.14 | SL_FIRST | CLOSED | ❌ Entrada muy mala |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,502

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 523 | 39.0% | 55.6% | 4.20 | 46.8% | 1.96 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 280 | 53.2% | 55.7% | 2.64 | 50.4% | 1.81 | ⚠️ CALIDAD MEDIA - Revisar |
| 3.0-5.0 ATR (Media) | 394 | 66.0% | 52.3% | 2.29 | 56.3% | 2.00 | ⚠️ CALIDAD MEDIA - Revisar |
| 5.0-10.0 ATR (Lejos) | 281 | 58.7% | 49.8% | 0.98 | 57.3% | 2.28 | ⚠️ CALIDAD MEDIA - Revisar |
| >10.0 ATR (Muy lejos) | 24 | 58.3% | 45.8% | 0.21 | 50.0% | 3.00 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (523 oportunidades)

- **WR Teórico:** 39.0% (si se hubieran ejecutado)
- **TP_FIRST:** 55.6% (291 de 523)
- **SL_FIRST:** 41.7% (218 de 523)
- **MFE Promedio:** 45.65 pts
- **MAE Promedio:** 35.37 pts
- **MFE/MAE Ratio:** 4.20
- **Good Entries:** 46.8% (MFE > MAE)
- **R:R Promedio:** 1.96

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (280 oportunidades)

- **WR Teórico:** 53.2% (si se hubieran ejecutado)
- **TP_FIRST:** 55.7% (156 de 280)
- **SL_FIRST:** 44.3% (124 de 280)
- **MFE Promedio:** 51.25 pts
- **MAE Promedio:** 35.62 pts
- **MFE/MAE Ratio:** 2.64
- **Good Entries:** 50.4% (MFE > MAE)
- **R:R Promedio:** 1.81

**⚠️ CALIDAD MEDIA - Revisar**

**3.0-5.0 ATR (Media)** (394 oportunidades)

- **WR Teórico:** 66.0% (si se hubieran ejecutado)
- **TP_FIRST:** 52.3% (206 de 394)
- **SL_FIRST:** 47.7% (188 de 394)
- **MFE Promedio:** 68.46 pts
- **MAE Promedio:** 40.08 pts
- **MFE/MAE Ratio:** 2.29
- **Good Entries:** 56.3% (MFE > MAE)
- **R:R Promedio:** 2.00

**⚠️ CALIDAD MEDIA - Revisar**

**5.0-10.0 ATR (Lejos)** (281 oportunidades)

- **WR Teórico:** 58.7% (si se hubieran ejecutado)
- **TP_FIRST:** 49.8% (140 de 281)
- **SL_FIRST:** 46.6% (131 de 281)
- **MFE Promedio:** 78.40 pts
- **MAE Promedio:** 43.25 pts
- **MFE/MAE Ratio:** 0.98
- **Good Entries:** 57.3% (MFE > MAE)
- **R:R Promedio:** 2.28

**⚠️ CALIDAD MEDIA - Revisar**

**>10.0 ATR (Muy lejos)** (24 oportunidades)

- **WR Teórico:** 58.3% (si se hubieran ejecutado)
- **TP_FIRST:** 45.8% (11 de 24)
- **SL_FIRST:** 45.8% (11 de 24)
- **MFE Promedio:** 72.66 pts
- **MAE Promedio:** 59.73 pts
- **MFE/MAE Ratio:** 0.21
- **Good Entries:** 50.0% (MFE > MAE)
- **R:R Promedio:** 3.00

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 57 | 523 | 280 | 394 |
| **TP_FIRST %** | 52.6% | 55.6% | 55.7% | 52.3% |
| **Good Entries %** | 42.1% | 46.8% | 50.4% | 56.3% |
| **MFE/MAE Ratio** | 72.20 | 4.20 | 2.64 | 2.29 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 394 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 66.0%
   - Good Entries: 56.3%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 11.81 pts
- **Mediana:** 10.14 pts
- **Min/Max:** 0.55 / 37.51 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 10.14 |
| P70 | 16.97 |
| P80 | 18.75 |
| P90 | 21.99 |
| P95 | 23.70 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 23.00 pts
- **Mediana:** 19.50 pts
- **Min/Max:** 3.25 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 19.50 |
| P70 | 27.90 |
| P80 | 35.85 |
| P90 | 49.15 |
| P95 | 51.58 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 21; // Era 60
public int MaxTPDistancePoints { get; set; } = 49; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 22.0pts, TP: 49.1pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (29.8%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.298) / 0.298
R:R_min = 2.35
```

**Estado actual:** R:R promedio = 2.27
**Gap:** 0.09 (necesitas mejorar R:R)

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

**Problema:** R:R actual (2.27) < R:R mínimo (2.35)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=29.8%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-12 16:40:05*