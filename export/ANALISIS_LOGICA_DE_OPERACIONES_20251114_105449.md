# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-14 11:00:19
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_105449.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251114_105449.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 46
- **Win Rate:** 34.8% (16/46)
- **Profit Factor:** 0.69
- **Avg R:R Planeado:** 1.98
- **R:R Mínimo para Break-Even:** 1.88

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 38 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.98
   - R:R necesario: 1.88
   - **Gap:** -0.11

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 7933 | 34.0% |
| Bearish | 6347 | 27.2% |
| Bullish | 9052 | 38.8% |

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
| StructureFusion | 10428 | 100.0% | 100.0% |
| ProximityAnalyzer | 3495 | 33.5% | 33.5% |
| DFM_Evaluated | 862 | 24.7% | 8.3% |
| DFM_Passed | 862 | 100.0% | 8.3% |
| RiskCalculator | 6019 | 698.3% | 57.7% |
| Risk_Accepted | 2 | 0.0% | 0.0% |
| TradeManager | 46 | 2300.0% | 0.4% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 6017 señales, -100.0%)
- **Tasa de conversión final:** 0.44% (de 10428 zonas iniciales → 46 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,275 | 49.9% |
| NO_SL | 660 | 25.9% |
| ENTRY_TOO_FAR | 383 | 15.0% |
| TP_CHECK_FAIL | 235 | 9.2% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,275 rechazos, 49.9%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2521 | 88.8% |
| P0_ANY_DIR | 319 | 11.2% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 43.50 pts (máxima ganancia flotante)
- **MAE Promedio:** 35.70 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 89.78

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 19 | 41.3% |
| SL_FIRST (precio fue hacia SL) | 25 | 54.3% |
| NEUTRAL (sin dirección clara) | 2 | 4.3% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 45.7%
- **Entradas Malas (MAE > MFE):** 54.3%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 46 | 19 | 25 | 41.3% | 43.50 | 35.70 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0002 | SELL | 2.75 | 32.00 | 0.09 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0007 | BUY | 1.75 | 34.25 | 0.05 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0008 | SELL | 42.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0008_2 | SELL | 42.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0010 | SELL | 2.25 | 13.00 | 0.17 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0012 | SELL | 174.75 | 27.75 | 6.30 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0013 | SELL | 238.50 | 19.00 | 12.55 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0013_2 | SELL | 238.50 | 19.00 | 12.55 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0016 | SELL | 72.50 | 18.00 | 4.03 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0018 | SELL | 1.25 | 68.50 | 0.02 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0018_2 | SELL | 1.25 | 68.50 | 0.02 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0020 | SELL | 14.25 | 42.75 | 0.33 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0023 | SELL | 53.25 | 33.00 | 1.61 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0023_2 | SELL | 53.25 | 33.00 | 1.61 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0025 | SELL | 55.25 | 58.00 | 0.95 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0027 | SELL | 26.75 | 88.50 | 0.30 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0027_2 | SELL | 26.75 | 88.50 | 0.30 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0029 | SELL | 0.25 | 77.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0031 | SELL | 0.00 | 103.75 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0035 | SELL | 12.75 | 25.25 | 0.50 | SL_FIRST | CLOSED | ❌ Entrada muy mala |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,329

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 502 | 29.7% | 53.4% | 4.39 | 41.6% | 2.09 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 264 | 49.6% | 49.6% | 2.36 | 50.4% | 1.96 | ⚠️ CALIDAD MEDIA - Revisar |
| 3.0-5.0 ATR (Media) | 335 | 69.3% | 59.1% | 2.50 | 63.9% | 2.04 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 216 | 74.1% | 69.9% | 1.59 | 74.5% | 2.27 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 12 | 41.7% | 41.7% | 0.00 | 41.7% | 2.96 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (502 oportunidades)

- **WR Teórico:** 29.7% (si se hubieran ejecutado)
- **TP_FIRST:** 53.4% (268 de 502)
- **SL_FIRST:** 41.6% (209 de 502)
- **MFE Promedio:** 43.15 pts
- **MAE Promedio:** 43.53 pts
- **MFE/MAE Ratio:** 4.39
- **Good Entries:** 41.6% (MFE > MAE)
- **R:R Promedio:** 2.09

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (264 oportunidades)

- **WR Teórico:** 49.6% (si se hubieran ejecutado)
- **TP_FIRST:** 49.6% (131 de 264)
- **SL_FIRST:** 49.6% (131 de 264)
- **MFE Promedio:** 66.11 pts
- **MAE Promedio:** 41.90 pts
- **MFE/MAE Ratio:** 2.36
- **Good Entries:** 50.4% (MFE > MAE)
- **R:R Promedio:** 1.96

**⚠️ CALIDAD MEDIA - Revisar**

**3.0-5.0 ATR (Media)** (335 oportunidades)

- **WR Teórico:** 69.3% (si se hubieran ejecutado)
- **TP_FIRST:** 59.1% (198 de 335)
- **SL_FIRST:** 38.2% (128 de 335)
- **MFE Promedio:** 84.46 pts
- **MAE Promedio:** 44.84 pts
- **MFE/MAE Ratio:** 2.50
- **Good Entries:** 63.9% (MFE > MAE)
- **R:R Promedio:** 2.04

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (216 oportunidades)

- **WR Teórico:** 74.1% (si se hubieran ejecutado)
- **TP_FIRST:** 69.9% (151 de 216)
- **SL_FIRST:** 27.3% (59 de 216)
- **MFE Promedio:** 87.45 pts
- **MAE Promedio:** 53.64 pts
- **MFE/MAE Ratio:** 1.59
- **Good Entries:** 74.5% (MFE > MAE)
- **R:R Promedio:** 2.27

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (12 oportunidades)

- **WR Teórico:** 41.7% (si se hubieran ejecutado)
- **TP_FIRST:** 41.7% (5 de 12)
- **SL_FIRST:** 58.3% (7 de 12)
- **MFE Promedio:** 108.55 pts
- **MAE Promedio:** 79.11 pts
- **MFE/MAE Ratio:** 0.00
- **Good Entries:** 41.7% (MFE > MAE)
- **R:R Promedio:** 2.96

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 46 | 502 | 264 | 335 |
| **TP_FIRST %** | 41.3% | 53.4% | 49.6% | 59.1% |
| **Good Entries %** | 45.7% | 41.6% | 50.4% | 63.9% |
| **MFE/MAE Ratio** | 89.78 | 4.39 | 2.36 | 2.50 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 335 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 69.3%
   - Good Entries: 63.9%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 13.98 pts
- **Mediana:** 10.09 pts
- **Min/Max:** 3.10 / 37.51 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 10.09 |
| P70 | 18.02 |
| P80 | 22.02 |
| P90 | 27.27 |
| P95 | 36.95 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 25.94 pts
- **Mediana:** 21.50 pts
- **Min/Max:** 4.50 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 21.50 |
| P70 | 35.58 |
| P80 | 40.95 |
| P90 | 49.30 |
| P95 | 52.31 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 27; // Era 60
public int MaxTPDistancePoints { get; set; } = 49; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 27.3pts, TP: 49.3pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (34.8%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.348) / 0.348
R:R_min = 1.88
```

**Estado actual:** R:R promedio = 1.98
**Gap:** -0.11 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **49** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.98) < R:R mínimo (1.88)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=34.8%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-14 11:00:19*