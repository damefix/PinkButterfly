# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-14 11:06:24
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_110105.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251114_110105.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 42
- **Win Rate:** 35.7% (15/42)
- **Profit Factor:** 1.22
- **Avg R:R Planeado:** 2.09
- **R:R Mínimo para Break-Even:** 1.80

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 41 puntos
   - TP máximo observado: 55 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.09
   - R:R necesario: 1.80
   - **Gap:** -0.29

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 7971 | 34.2% |
| Bearish | 6324 | 27.1% |
| Bullish | 9037 | 38.7% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.079
- **Score Min/Max:** [-0.990, 0.970]
- **Componentes (promedio):**
  - EMA20 Slope: 0.040
  - EMA50 Cross: 0.190
  - BOS Count: 0.008
  - Regression 24h: 0.088

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.970 (apenas supera threshold)
- Score mínimo observado: -0.990 (apenas supera threshold)
- **Consecuencia:** Sistema queda 34.2% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.2%)

**Posibles causas:**
- **BOS Score bajo (0.008):** BOS/CHoCH no se detectan correctamente
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
| StructureFusion | 10410 | 100.0% | 100.0% |
| ProximityAnalyzer | 3141 | 30.2% | 30.2% |
| DFM_Evaluated | 759 | 24.2% | 7.3% |
| DFM_Passed | 759 | 100.0% | 7.3% |
| RiskCalculator | 5930 | 781.3% | 57.0% |
| Risk_Accepted | 1 | 0.0% | 0.0% |
| TradeManager | 42 | 4200.0% | 0.4% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 5929 señales, -100.0%)
- **Tasa de conversión final:** 0.40% (de 10410 zonas iniciales → 42 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,235 | 48.3% |
| NO_SL | 748 | 29.3% |
| ENTRY_TOO_FAR | 380 | 14.9% |
| TP_CHECK_FAIL | 193 | 7.6% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,235 rechazos, 48.3%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2529 | 89.2% |
| P0_ANY_DIR | 305 | 10.8% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 38.48 pts (máxima ganancia flotante)
- **MAE Promedio:** 28.01 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 26.56

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 18 | 42.9% |
| SL_FIRST (precio fue hacia SL) | 21 | 50.0% |
| NEUTRAL (sin dirección clara) | 3 | 7.1% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 47.6%
- **Entradas Malas (MAE > MFE):** 52.4%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 42 | 18 | 21 | 42.9% | 38.48 | 28.01 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0002 | SELL | 1.75 | 36.75 | 0.05 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0003 | SELL | 10.25 | 28.00 | 0.37 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0007 | BUY | 2.50 | 11.25 | 0.22 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0007_2 | BUY | 2.50 | 11.25 | 0.22 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0007_3 | BUY | 2.50 | 11.25 | 0.22 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0008 | SELL | 1.75 | 13.00 | 0.13 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0009 | SELL | 144.75 | 20.50 | 7.06 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0010 | SELL | 245.50 | 14.25 | 17.23 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0013 | SELL | 86.75 | 10.00 | 8.68 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0015 | SELL | 1.25 | 68.50 | 0.02 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0015_2 | SELL | 1.25 | 68.50 | 0.02 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0017 | SELL | 10.50 | 46.50 | 0.23 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0019 | SELL | 0.00 | 40.75 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0021 | SELL | 55.25 | 58.00 | 0.95 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0022 | SELL | 23.75 | 102.25 | 0.23 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0023 | SELL | 17.00 | 63.25 | 0.27 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0029 | SELL | 14.50 | 19.75 | 0.73 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0030 | SELL | 12.75 | 11.75 | 1.09 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0031 | SELL | 82.25 | 9.75 | 8.44 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0032 | SELL | 81.75 | 7.75 | 10.55 | NEUTRAL | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,253

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 464 | 26.1% | 53.7% | 2.76 | 39.2% | 2.01 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 241 | 51.5% | 52.3% | 2.53 | 50.6% | 1.95 | ⚠️ CALIDAD MEDIA - Revisar |
| 3.0-5.0 ATR (Media) | 326 | 64.7% | 55.8% | 2.34 | 58.6% | 2.04 | ⚠️ CALIDAD MEDIA - Revisar |
| 5.0-10.0 ATR (Lejos) | 211 | 72.0% | 69.7% | 0.75 | 71.1% | 2.24 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 11 | 36.4% | 36.4% | 0.00 | 36.4% | 3.14 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (464 oportunidades)

- **WR Teórico:** 26.1% (si se hubieran ejecutado)
- **TP_FIRST:** 53.7% (249 de 464)
- **SL_FIRST:** 41.2% (191 de 464)
- **MFE Promedio:** 40.91 pts
- **MAE Promedio:** 44.00 pts
- **MFE/MAE Ratio:** 2.76
- **Good Entries:** 39.2% (MFE > MAE)
- **R:R Promedio:** 2.01

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (241 oportunidades)

- **WR Teórico:** 51.5% (si se hubieran ejecutado)
- **TP_FIRST:** 52.3% (126 de 241)
- **SL_FIRST:** 46.5% (112 de 241)
- **MFE Promedio:** 62.68 pts
- **MAE Promedio:** 39.64 pts
- **MFE/MAE Ratio:** 2.53
- **Good Entries:** 50.6% (MFE > MAE)
- **R:R Promedio:** 1.95

**⚠️ CALIDAD MEDIA - Revisar**

**3.0-5.0 ATR (Media)** (326 oportunidades)

- **WR Teórico:** 64.7% (si se hubieran ejecutado)
- **TP_FIRST:** 55.8% (182 de 326)
- **SL_FIRST:** 41.4% (135 de 326)
- **MFE Promedio:** 76.96 pts
- **MAE Promedio:** 42.69 pts
- **MFE/MAE Ratio:** 2.34
- **Good Entries:** 58.6% (MFE > MAE)
- **R:R Promedio:** 2.04

**⚠️ CALIDAD MEDIA - Revisar**

**5.0-10.0 ATR (Lejos)** (211 oportunidades)

- **WR Teórico:** 72.0% (si se hubieran ejecutado)
- **TP_FIRST:** 69.7% (147 de 211)
- **SL_FIRST:** 27.5% (58 de 211)
- **MFE Promedio:** 86.88 pts
- **MAE Promedio:** 50.70 pts
- **MFE/MAE Ratio:** 0.75
- **Good Entries:** 71.1% (MFE > MAE)
- **R:R Promedio:** 2.24

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (11 oportunidades)

- **WR Teórico:** 36.4% (si se hubieran ejecutado)
- **TP_FIRST:** 36.4% (4 de 11)
- **SL_FIRST:** 63.6% (7 de 11)
- **MFE Promedio:** 116.44 pts
- **MAE Promedio:** 84.71 pts
- **MFE/MAE Ratio:** 0.00
- **Good Entries:** 36.4% (MFE > MAE)
- **R:R Promedio:** 3.14

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 42 | 464 | 241 | 326 |
| **TP_FIRST %** | 42.9% | 53.7% | 52.3% | 55.8% |
| **Good Entries %** | 47.6% | 39.2% | 50.6% | 58.6% |
| **MFE/MAE Ratio** | 26.56 | 2.76 | 2.53 | 2.34 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 326 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 64.7%
   - Good Entries: 58.6%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 12.69 pts
- **Mediana:** 8.03 pts
- **Min/Max:** 2.34 / 41.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 8.03 |
| P70 | 18.21 |
| P80 | 22.02 |
| P90 | 25.93 |
| P95 | 37.24 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 23.43 pts
- **Mediana:** 17.25 pts
- **Min/Max:** 4.50 / 54.75 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 17.25 |
| P70 | 33.12 |
| P80 | 40.65 |
| P90 | 49.70 |
| P95 | 53.27 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 25; // Era 60
public int MaxTPDistancePoints { get; set; } = 49; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 25.9pts, TP: 49.7pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (35.7%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.357) / 0.357
R:R_min = 1.80
```

**Estado actual:** R:R promedio = 2.09
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
1. **MaxSLDistancePoints:** 60 → **25** (P90 real)
2. **MaxTPDistancePoints:** 120 → **49** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.09) < R:R mínimo (1.80)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=35.7%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-14 11:06:24*