# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-19 11:19:01
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_110944.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_110944.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 15
- **Win Rate:** 40.0% (6/15)
- **Profit Factor:** 1.10
- **Avg R:R Planeado:** 1.70
- **R:R Mínimo para Break-Even:** 1.50

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 29 puntos
   - TP máximo observado: 52 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.70
   - R:R necesario: 1.50
   - **Gap:** -0.20

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bullish | 1186 | 37.9% |
| Neutral | 787 | 25.2% |
| Bearish | 1154 | 36.9% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.018
- **Score Min/Max:** [-1.000, 0.990]
- **Componentes (promedio):**
  - EMA20 Slope: -0.006
  - EMA50 Cross: 0.034
  - BOS Count: -0.020
  - Regression 24h: 0.083

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.990 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 25.2% Neutral → bias no diferencia tendencias

### 2.3 Estado: Bias Compuesto Funcionando Correctamente

✅ **Threshold actual:** 0.3 (correcto)
✅ **Bias Neutral:** 25.2% (aceptable)
✅ **Score promedio:** 0.018

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 17697 | 100.0% | 100.0% |
| ProximityAnalyzer | 2927 | 16.5% | 16.5% |
| DFM_Evaluated | 969 | 33.1% | 5.5% |
| DFM_Passed | 969 | 100.0% | 5.5% |
| RiskCalculator | 5266 | 543.4% | 29.8% |
| Risk_Accepted | 112 | 2.1% | 0.6% |
| TradeManager | 15 | 13.4% | 0.1% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 5154 señales, -97.9%)
- **Tasa de conversión final:** 0.08% (de 17697 zonas iniciales → 15 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,072 | 49.9% |
| ENTRY_TOO_FAR | 729 | 33.9% |
| NO_SL | 192 | 8.9% |
| TP_CHECK_FAIL | 156 | 7.3% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,072 rechazos, 49.9%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 3417 | 85.5% |
| P0_ANY_DIR | 580 | 14.5% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 5.72 pts (máxima ganancia flotante)
- **MAE Promedio:** 111.47 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 0.17

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 1 | 6.7% |
| SL_FIRST (precio fue hacia SL) | 14 | 93.3% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 6.7%
- **Entradas Malas (MAE > MFE):** 93.3%

⚠️ **ALERTA:** >60% de entradas tienen MAE > MFE
- **Problema:** El precio va más en contra que a favor antes del cierre
- **Causas posibles:**
  1. Timing incorrecto (entramos antes de reversión)
  2. Bias desincronizado (operamos contra tendencia real)
  3. Zonas de baja calidad (sin confluence real)

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 15 | 1 | 14 | 6.7% | 5.72 | 111.47 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | SELL | 0.00 | 189.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0004 | SELL | 0.00 | 189.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0006 | SELL | 0.00 | 164.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0009 | SELL | 0.00 | 189.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0010 | SELL | 0.00 | 164.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0012 | SELL | 0.00 | 135.75 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0020 | SELL | 0.00 | 52.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0021 | SELL | 4.50 | 45.75 | 0.10 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0028 | SELL | 0.00 | 52.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0029 | SELL | 0.00 | 52.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0031 | SELL | 0.00 | 72.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0032 | SELL | 0.00 | 135.75 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0034 | SELL | 0.00 | 164.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0046 | SELL | 21.25 | 30.00 | 0.71 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0086 | SELL | 60.00 | 35.25 | 1.70 | SL_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,398

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 400 | 16.5% | 20.2% | 0.69 | 21.5% | 1.78 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 239 | 27.2% | 28.9% | 1.05 | 24.7% | 1.77 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 429 | 28.4% | 28.0% | 0.54 | 27.0% | 1.84 | ❌ BAJA CALIDAD - Filtro correcto |
| 5.0-10.0 ATR (Lejos) | 280 | 18.6% | 19.3% | 0.12 | 19.6% | 1.74 | ❌ BAJA CALIDAD - Filtro correcto |
| >10.0 ATR (Muy lejos) | 50 | 0.0% | 0.0% | 0.01 | 0.0% | 1.84 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (400 oportunidades)

- **WR Teórico:** 16.5% (si se hubieran ejecutado)
- **TP_FIRST:** 20.2% (81 de 400)
- **SL_FIRST:** 78.8% (315 de 400)
- **MFE Promedio:** 49.74 pts
- **MAE Promedio:** 99.31 pts
- **MFE/MAE Ratio:** 0.69
- **Good Entries:** 21.5% (MFE > MAE)
- **R:R Promedio:** 1.78

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (239 oportunidades)

- **WR Teórico:** 27.2% (si se hubieran ejecutado)
- **TP_FIRST:** 28.9% (69 de 239)
- **SL_FIRST:** 71.1% (170 de 239)
- **MFE Promedio:** 55.31 pts
- **MAE Promedio:** 83.46 pts
- **MFE/MAE Ratio:** 1.05
- **Good Entries:** 24.7% (MFE > MAE)
- **R:R Promedio:** 1.77

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (429 oportunidades)

- **WR Teórico:** 28.4% (si se hubieran ejecutado)
- **TP_FIRST:** 28.0% (120 de 429)
- **SL_FIRST:** 72.0% (309 de 429)
- **MFE Promedio:** 61.27 pts
- **MAE Promedio:** 89.09 pts
- **MFE/MAE Ratio:** 0.54
- **Good Entries:** 27.0% (MFE > MAE)
- **R:R Promedio:** 1.84

**❌ BAJA CALIDAD - Filtro correcto**

**5.0-10.0 ATR (Lejos)** (280 oportunidades)

- **WR Teórico:** 18.6% (si se hubieran ejecutado)
- **TP_FIRST:** 19.3% (54 de 280)
- **SL_FIRST:** 80.7% (226 de 280)
- **MFE Promedio:** 67.22 pts
- **MAE Promedio:** 96.04 pts
- **MFE/MAE Ratio:** 0.12
- **Good Entries:** 19.6% (MFE > MAE)
- **R:R Promedio:** 1.74

**❌ BAJA CALIDAD - Filtro correcto**

**>10.0 ATR (Muy lejos)** (50 oportunidades)

- **WR Teórico:** 0.0% (si se hubieran ejecutado)
- **TP_FIRST:** 0.0% (0 de 50)
- **SL_FIRST:** 100.0% (50 de 50)
- **MFE Promedio:** 4.50 pts
- **MAE Promedio:** 81.07 pts
- **MFE/MAE Ratio:** 0.01
- **Good Entries:** 0.0% (MFE > MAE)
- **R:R Promedio:** 1.84

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 15 | 400 | 239 | 429 |
| **TP_FIRST %** | 6.7% | 20.2% | 28.9% | 28.0% |
| **Good Entries %** | 6.7% | 21.5% | 24.7% | 27.0% |
| **MFE/MAE Ratio** | 0.17 | 0.69 | 1.05 | 0.54 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

✅ **CORRECTO: El filtro 2.0 ATR está bloqueando 239 oportunidades de BAJA calidad**
   - WR Teórico: 27.2%
   - **ACCIÓN:** Mantener MaxDistanceToRegister_ATR_Normal = 2.0

✅ **CORRECTO: Las 429 oportunidades en 3.0-5.0 ATR tienen baja calidad**


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 21.03 pts
- **Mediana:** 17.84 pts
- **Min/Max:** 13.69 / 28.57 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 17.84 |
| P70 | 26.79 |
| P80 | 26.89 |
| P90 | 28.21 |
| P95 | 28.69 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 35.17 pts
- **Mediana:** 32.25 pts
- **Min/Max:** 20.00 / 52.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 32.25 |
| P70 | 39.50 |
| P80 | 46.50 |
| P90 | 52.00 |
| P95 | 52.00 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 28; // Era 60
public int MaxTPDistancePoints { get; set; } = 52; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 28.2pts, TP: 52.0pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (40.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.400) / 0.400
R:R_min = 1.50
```

**Estado actual:** R:R promedio = 1.70
**Gap:** -0.20 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **28** (P90 real)
2. **MaxTPDistancePoints:** 120 → **52** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.70) < R:R mínimo (1.50)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=40.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-19 11:19:01*