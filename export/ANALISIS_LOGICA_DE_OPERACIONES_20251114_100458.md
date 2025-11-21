# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-14 10:11:28
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_100458.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251114_100458.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 41
- **Win Rate:** 31.7% (13/41)
- **Profit Factor:** 0.64
- **Avg R:R Planeado:** 1.96
- **R:R Mínimo para Break-Even:** 2.15

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 38 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.96
   - R:R necesario: 2.15
   - **Gap:** 0.19

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 7926 | 34.0% |
| Bearish | 6352 | 27.2% |
| Bullish | 9050 | 38.8% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.079
- **Score Min/Max:** [-0.990, 0.980]
- **Componentes (promedio):**
  - EMA20 Slope: 0.040
  - EMA50 Cross: 0.190
  - BOS Count: 0.007
  - Regression 24h: 0.088

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.980 (apenas supera threshold)
- Score mínimo observado: -0.990 (apenas supera threshold)
- **Consecuencia:** Sistema queda 34.0% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.0%)

**Posibles causas:**
- **BOS Score bajo (0.007):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio 0.079 indica poca señal direccional
- **Mercado lateral:** Scores reales [-0.99, 0.98] muy cercanos a 0

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
| StructureFusion | 10410 | 100.0% | 100.0% |
| ProximityAnalyzer | 4285 | 41.2% | 41.2% |
| DFM_Evaluated | 863 | 20.1% | 8.3% |
| DFM_Passed | 863 | 100.0% | 8.3% |
| RiskCalculator | 6487 | 751.7% | 62.3% |
| Risk_Accepted | 1 | 0.0% | 0.0% |
| TradeManager | 41 | 4100.0% | 0.4% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 6486 señales, -100.0%)
- **Tasa de conversión final:** 0.39% (de 10410 zonas iniciales → 41 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,653 | 57.8% |
| NO_SL | 544 | 19.0% |
| ENTRY_TOO_FAR | 407 | 14.2% |
| TP_CHECK_FAIL | 255 | 8.9% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,653 rechazos, 57.8%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2614 | 88.9% |
| P0_ANY_DIR | 326 | 11.1% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 29.43 pts (máxima ganancia flotante)
- **MAE Promedio:** 38.32 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 51.13

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 23 | 56.1% |
| SL_FIRST (precio fue hacia SL) | 17 | 41.5% |
| NEUTRAL (sin dirección clara) | 1 | 2.4% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 36.6%
- **Entradas Malas (MAE > MFE):** 63.4%

⚠️ **ALERTA:** >60% de entradas tienen MAE > MFE
- **Problema:** El precio va más en contra que a favor antes del cierre
- **Causas posibles:**
  1. Timing incorrecto (entramos antes de reversión)
  2. Bias desincronizado (operamos contra tendencia real)
  3. Zonas de baja calidad (sin confluence real)

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 41 | 23 | 17 | 56.1% | 29.43 | 38.32 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0003 | SELL | 4.75 | 30.00 | 0.16 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0004 | SELL | 0.00 | 38.50 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0005 | SELL | 1.00 | 37.25 | 0.03 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0008 | BUY | 1.75 | 34.25 | 0.05 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0009 | SELL | 42.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0011 | SELL | 144.75 | 20.50 | 7.06 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0015 | SELL | 1.25 | 68.50 | 0.02 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0015_2 | SELL | 1.25 | 68.50 | 0.02 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0017 | SELL | 14.25 | 42.75 | 0.33 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0020 | BUY | 19.75 | 95.50 | 0.21 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0020_2 | BUY | 19.75 | 95.50 | 0.21 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0023 | SELL | 55.25 | 58.00 | 0.95 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0024 | SELL | 26.75 | 88.50 | 0.30 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0026 | SELL | 0.25 | 77.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0028 | SELL | 0.00 | 103.75 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0032 | SELL | 12.75 | 25.25 | 0.50 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0034 | BUY | 6.50 | 34.50 | 0.19 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0036 | SELL | 82.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0039 | SELL | 10.75 | 23.25 | 0.46 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0041 | SELL | 19.25 | 27.50 | 0.70 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,353

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 502 | 39.8% | 68.5% | 4.92 | 51.0% | 2.05 | ⚠️ CALIDAD MEDIA - Revisar |
| 2.0-3.0 ATR (Cerca) | 247 | 53.4% | 59.5% | 2.92 | 53.8% | 1.93 | ⚠️ CALIDAD MEDIA - Revisar |
| 3.0-5.0 ATR (Media) | 356 | 72.8% | 61.2% | 2.90 | 67.4% | 2.09 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 233 | 78.1% | 72.5% | 1.82 | 76.8% | 2.30 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 15 | 53.3% | 40.0% | 0.13 | 40.0% | 2.91 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (502 oportunidades)

- **WR Teórico:** 39.8% (si se hubieran ejecutado)
- **TP_FIRST:** 68.5% (344 de 502)
- **SL_FIRST:** 27.3% (137 de 502)
- **MFE Promedio:** 49.59 pts
- **MAE Promedio:** 37.73 pts
- **MFE/MAE Ratio:** 4.92
- **Good Entries:** 51.0% (MFE > MAE)
- **R:R Promedio:** 2.05

**⚠️ CALIDAD MEDIA - Revisar**

**2.0-3.0 ATR (Cerca)** (247 oportunidades)

- **WR Teórico:** 53.4% (si se hubieran ejecutado)
- **TP_FIRST:** 59.5% (147 de 247)
- **SL_FIRST:** 39.7% (98 de 247)
- **MFE Promedio:** 67.20 pts
- **MAE Promedio:** 44.87 pts
- **MFE/MAE Ratio:** 2.92
- **Good Entries:** 53.8% (MFE > MAE)
- **R:R Promedio:** 1.93

**⚠️ CALIDAD MEDIA - Revisar**

**3.0-5.0 ATR (Media)** (356 oportunidades)

- **WR Teórico:** 72.8% (si se hubieran ejecutado)
- **TP_FIRST:** 61.2% (218 de 356)
- **SL_FIRST:** 36.8% (131 de 356)
- **MFE Promedio:** 84.73 pts
- **MAE Promedio:** 43.16 pts
- **MFE/MAE Ratio:** 2.90
- **Good Entries:** 67.4% (MFE > MAE)
- **R:R Promedio:** 2.09

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (233 oportunidades)

- **WR Teórico:** 78.1% (si se hubieran ejecutado)
- **TP_FIRST:** 72.5% (169 de 233)
- **SL_FIRST:** 25.8% (60 de 233)
- **MFE Promedio:** 92.33 pts
- **MAE Promedio:** 50.48 pts
- **MFE/MAE Ratio:** 1.82
- **Good Entries:** 76.8% (MFE > MAE)
- **R:R Promedio:** 2.30

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (15 oportunidades)

- **WR Teórico:** 53.3% (si se hubieran ejecutado)
- **TP_FIRST:** 40.0% (6 de 15)
- **SL_FIRST:** 60.0% (9 de 15)
- **MFE Promedio:** 89.88 pts
- **MAE Promedio:** 67.42 pts
- **MFE/MAE Ratio:** 0.13
- **Good Entries:** 40.0% (MFE > MAE)
- **R:R Promedio:** 2.91

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 41 | 502 | 247 | 356 |
| **TP_FIRST %** | 56.1% | 68.5% | 59.5% | 61.2% |
| **Good Entries %** | 36.6% | 51.0% | 53.8% | 67.4% |
| **MFE/MAE Ratio** | 51.13 | 4.92 | 2.92 | 2.90 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 356 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 72.8%
   - Good Entries: 67.4%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 15.94 pts
- **Mediana:** 12.18 pts
- **Min/Max:** 3.35 / 37.51 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 12.18 |
| P70 | 21.94 |
| P80 | 23.62 |
| P90 | 34.81 |
| P95 | 37.22 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 29.40 pts
- **Mediana:** 33.00 pts
- **Min/Max:** 6.25 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 33.00 |
| P70 | 42.15 |
| P80 | 49.00 |
| P90 | 50.00 |
| P95 | 52.62 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 34; // Era 60
public int MaxTPDistancePoints { get; set; } = 50; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 34.8pts, TP: 50.0pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (31.7%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.317) / 0.317
R:R_min = 2.15
```

**Estado actual:** R:R promedio = 1.96
**Gap:** 0.19 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **34** (P90 real)
2. **MaxTPDistancePoints:** 120 → **50** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.96) < R:R mínimo (2.15)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=31.7%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-14 10:11:28*