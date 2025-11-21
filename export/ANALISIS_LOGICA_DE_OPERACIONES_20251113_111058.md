# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-13 11:15:29
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_111058.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251113_111058.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 43
- **Win Rate:** 27.9% (12/43)
- **Profit Factor:** 0.48
- **Avg R:R Planeado:** 2.00
- **R:R Mínimo para Break-Even:** 2.58

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 38 puntos
   - TP máximo observado: 56 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.00
   - R:R necesario: 2.58
   - **Gap:** 0.59

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8038 | 34.4% |
| Bearish | 6272 | 26.9% |
| Bullish | 9023 | 38.7% |

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
| StructureFusion | 10268 | 100.0% | 100.0% |
| ProximityAnalyzer | 4124 | 40.2% | 40.2% |
| DFM_Evaluated | 831 | 20.2% | 8.1% |
| DFM_Passed | 831 | 100.0% | 8.1% |
| RiskCalculator | 6314 | 759.8% | 61.5% |
| Risk_Accepted | 108 | 1.7% | 1.1% |
| TradeManager | 43 | 39.8% | 0.4% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 6206 señales, -98.3%)
- **Tasa de conversión final:** 0.42% (de 10268 zonas iniciales → 43 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,656 | 58.7% |
| NO_SL | 524 | 18.6% |
| ENTRY_TOO_FAR | 382 | 13.5% |
| TP_CHECK_FAIL | 258 | 9.1% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,656 rechazos, 58.7%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2553 | 89.6% |
| P0_ANY_DIR | 296 | 10.4% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 45.59 pts (máxima ganancia flotante)
- **MAE Promedio:** 37.95 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 48.63

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 25 | 58.1% |
| SL_FIRST (precio fue hacia SL) | 18 | 41.9% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 37.2%
- **Entradas Malas (MAE > MFE):** 62.8%

⚠️ **ALERTA:** >60% de entradas tienen MAE > MFE
- **Problema:** El precio va más en contra que a favor antes del cierre
- **Causas posibles:**
  1. Timing incorrecto (entramos antes de reversión)
  2. Bias desincronizado (operamos contra tendencia real)
  3. Zonas de baja calidad (sin confluence real)

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 43 | 25 | 18 | 58.1% | 45.59 | 37.95 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | SELL | 22.00 | 21.75 | 1.01 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0005 | SELL | 12.75 | 19.00 | 0.67 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0007 | SELL | 0.00 | 38.00 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0010 | BUY | 1.75 | 21.25 | 0.08 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0010_2 | BUY | 1.75 | 21.25 | 0.08 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0012 | SELL | 0.75 | 13.00 | 0.06 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0012_2 | SELL | 0.75 | 13.00 | 0.06 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0015 | SELL | 245.00 | 21.25 | 11.53 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0016 | SELL | 244.50 | 21.75 | 11.24 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0016_2 | SELL | 244.50 | 21.75 | 11.24 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0016_3 | SELL | 244.50 | 21.75 | 11.24 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0021 | SELL | 2.75 | 67.00 | 0.04 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0023 | SELL | 3.25 | 71.00 | 0.05 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0026 | BUY | 65.75 | 24.75 | 2.66 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0027 | BUY | 13.00 | 105.50 | 0.12 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0028 | SELL | 129.25 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0029 | SELL | 55.25 | 58.00 | 0.95 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0032 | SELL | 36.50 | 75.00 | 0.49 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0032_2 | SELL | 36.50 | 75.00 | 0.49 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0037 | SELL | 0.00 | 103.75 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,305

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 472 | 36.2% | 55.9% | 3.97 | 43.9% | 2.07 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 249 | 53.0% | 55.4% | 2.14 | 51.0% | 2.18 | ⚠️ CALIDAD MEDIA - Revisar |
| 3.0-5.0 ATR (Media) | 367 | 68.1% | 52.0% | 2.15 | 59.1% | 2.15 | ⚠️ CALIDAD MEDIA - Revisar |
| 5.0-10.0 ATR (Lejos) | 198 | 71.7% | 62.1% | 1.44 | 67.2% | 2.24 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 19 | 73.7% | 57.9% | 0.38 | 63.2% | 3.57 | ✅ BUENA CALIDAD - Considerar incluir |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (472 oportunidades)

- **WR Teórico:** 36.2% (si se hubieran ejecutado)
- **TP_FIRST:** 55.9% (264 de 472)
- **SL_FIRST:** 40.7% (192 de 472)
- **MFE Promedio:** 44.26 pts
- **MAE Promedio:** 41.26 pts
- **MFE/MAE Ratio:** 3.97
- **Good Entries:** 43.9% (MFE > MAE)
- **R:R Promedio:** 2.07

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (249 oportunidades)

- **WR Teórico:** 53.0% (si se hubieran ejecutado)
- **TP_FIRST:** 55.4% (138 de 249)
- **SL_FIRST:** 44.2% (110 de 249)
- **MFE Promedio:** 52.05 pts
- **MAE Promedio:** 42.46 pts
- **MFE/MAE Ratio:** 2.14
- **Good Entries:** 51.0% (MFE > MAE)
- **R:R Promedio:** 2.18

**⚠️ CALIDAD MEDIA - Revisar**

**3.0-5.0 ATR (Media)** (367 oportunidades)

- **WR Teórico:** 68.1% (si se hubieran ejecutado)
- **TP_FIRST:** 52.0% (191 de 367)
- **SL_FIRST:** 47.7% (175 de 367)
- **MFE Promedio:** 71.72 pts
- **MAE Promedio:** 44.12 pts
- **MFE/MAE Ratio:** 2.15
- **Good Entries:** 59.1% (MFE > MAE)
- **R:R Promedio:** 2.15

**⚠️ CALIDAD MEDIA - Revisar**

**5.0-10.0 ATR (Lejos)** (198 oportunidades)

- **WR Teórico:** 71.7% (si se hubieran ejecutado)
- **TP_FIRST:** 62.1% (123 de 198)
- **SL_FIRST:** 37.4% (74 de 198)
- **MFE Promedio:** 74.51 pts
- **MAE Promedio:** 45.29 pts
- **MFE/MAE Ratio:** 1.44
- **Good Entries:** 67.2% (MFE > MAE)
- **R:R Promedio:** 2.24

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (19 oportunidades)

- **WR Teórico:** 73.7% (si se hubieran ejecutado)
- **TP_FIRST:** 57.9% (11 de 19)
- **SL_FIRST:** 42.1% (8 de 19)
- **MFE Promedio:** 86.93 pts
- **MAE Promedio:** 64.69 pts
- **MFE/MAE Ratio:** 0.38
- **Good Entries:** 63.2% (MFE > MAE)
- **R:R Promedio:** 3.57

**✅ BUENA CALIDAD - Considerar incluir**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 43 | 472 | 249 | 367 |
| **TP_FIRST %** | 58.1% | 55.9% | 55.4% | 52.0% |
| **Good Entries %** | 37.2% | 43.9% | 51.0% | 59.1% |
| **MFE/MAE Ratio** | 48.63 | 3.97 | 2.14 | 2.15 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 367 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 68.1%
   - Good Entries: 59.1%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 13.91 pts
- **Mediana:** 13.16 pts
- **Min/Max:** 0.80 / 37.61 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 13.16 |
| P70 | 18.70 |
| P80 | 21.16 |
| P90 | 27.01 |
| P95 | 36.89 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 24.80 pts
- **Mediana:** 23.25 pts
- **Min/Max:** 4.00 / 55.75 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 23.25 |
| P70 | 35.75 |
| P80 | 43.05 |
| P90 | 50.40 |
| P95 | 54.30 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 27; // Era 60
public int MaxTPDistancePoints { get; set; } = 50; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 27.0pts, TP: 50.4pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (27.9%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.279) / 0.279
R:R_min = 2.58
```

**Estado actual:** R:R promedio = 2.00
**Gap:** 0.59 (necesitas mejorar R:R)

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

**Problema:** R:R actual (2.00) < R:R mínimo (2.58)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=27.9%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-13 11:15:29*