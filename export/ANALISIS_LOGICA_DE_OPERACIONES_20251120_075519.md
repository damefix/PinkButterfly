# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-20 08:19:36
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251120_075519.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251120_075519.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 11
- **Win Rate:** 36.4% (4/11)
- **Profit Factor:** 1.33
- **Avg R:R Planeado:** 2.11
- **R:R Mínimo para Break-Even:** 1.75

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 19 puntos
   - TP máximo observado: 34 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.11
   - R:R necesario: 1.75
   - **Gap:** -0.36

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bullish | 1177 | 37.7% |
| Neutral | 792 | 25.3% |
| Bearish | 1157 | 37.0% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.013
- **Score Min/Max:** [-1.000, 0.990]
- **Componentes (promedio):**
  - EMA20 Slope: 0.000
  - EMA50 Cross: 0.002
  - BOS Count: -0.019
  - Regression 24h: 0.086

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.990 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 25.3% Neutral → bias no diferencia tendencias

### 2.3 Estado: Bias Compuesto Funcionando Correctamente

✅ **Threshold actual:** 0.3 (correcto)
✅ **Bias Neutral:** 25.3% (aceptable)
✅ **Score promedio:** 0.013

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 17328 | 100.0% | 100.0% |
| ProximityAnalyzer | 2506 | 14.5% | 14.5% |
| DFM_Evaluated | 539 | 21.5% | 3.1% |
| DFM_Passed | 539 | 100.0% | 3.1% |
| RiskCalculator | 4681 | 868.5% | 27.0% |
| Risk_Accepted | 35 | 0.7% | 0.2% |
| TradeManager | 11 | 31.4% | 0.1% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 4646 señales, -99.3%)
- **Tasa de conversión final:** 0.06% (de 17328 zonas iniciales → 11 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,094 | 51.4% |
| ENTRY_TOO_FAR | 570 | 26.8% |
| NO_SL | 315 | 14.8% |
| TP_CHECK_FAIL | 151 | 7.1% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,094 rechazos, 51.4%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2805 | 86.6% |
| P0_ANY_DIR | 433 | 13.4% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 37.93 pts (máxima ganancia flotante)
- **MAE Promedio:** 41.57 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 2.09

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 3 | 27.3% |
| SL_FIRST (precio fue hacia SL) | 8 | 72.7% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 72.7%
- **Entradas Malas (MAE > MFE):** 27.3%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 11 | 3 | 8 | 27.3% | 37.93 | 41.57 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0004 | SELL | 32.50 | 17.75 | 1.83 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0007 | SELL | 0.00 | 75.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0014 | SELL | 26.75 | 23.50 | 1.14 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0019 | SELL | 41.75 | 10.50 | 3.98 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0024 | SELL | 53.75 | 45.50 | 1.18 | SL_FIRST | CLOSED | 👍 Entrada correcta |
| T0024_2 | SELL | 53.75 | 45.50 | 1.18 | SL_FIRST | CLOSED | 👍 Entrada correcta |
| T0025 | SELL | 49.75 | 23.50 | 2.12 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0026 | SELL | 50.50 | 9.25 | 5.46 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0032 | SELL | 61.50 | 11.00 | 5.59 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0034 | SELL | 23.50 | 97.75 | 0.24 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0034_2 | SELL | 23.50 | 97.75 | 0.24 | SL_FIRST | CLOSED | ❌ Entrada muy mala |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 768

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 182 | 22.5% | 53.3% | 2.94 | 26.9% | 2.44 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 121 | 19.8% | 57.9% | 0.71 | 23.1% | 2.56 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 269 | 30.9% | 58.4% | 0.69 | 30.9% | 2.32 | ❌ BAJA CALIDAD - Filtro correcto |
| 5.0-10.0 ATR (Lejos) | 167 | 28.7% | 68.3% | 0.55 | 26.9% | 2.46 | ❌ BAJA CALIDAD - Filtro correcto |
| >10.0 ATR (Muy lejos) | 29 | 17.2% | 96.6% | 0.73 | 17.2% | 2.51 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (182 oportunidades)

- **WR Teórico:** 22.5% (si se hubieran ejecutado)
- **TP_FIRST:** 53.3% (97 de 182)
- **SL_FIRST:** 45.6% (83 de 182)
- **MFE Promedio:** 32.11 pts
- **MAE Promedio:** 46.47 pts
- **MFE/MAE Ratio:** 2.94
- **Good Entries:** 26.9% (MFE > MAE)
- **R:R Promedio:** 2.44

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (121 oportunidades)

- **WR Teórico:** 19.8% (si se hubieran ejecutado)
- **TP_FIRST:** 57.9% (70 de 121)
- **SL_FIRST:** 42.1% (51 de 121)
- **MFE Promedio:** 33.26 pts
- **MAE Promedio:** 46.09 pts
- **MFE/MAE Ratio:** 0.71
- **Good Entries:** 23.1% (MFE > MAE)
- **R:R Promedio:** 2.56

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (269 oportunidades)

- **WR Teórico:** 30.9% (si se hubieran ejecutado)
- **TP_FIRST:** 58.4% (157 de 269)
- **SL_FIRST:** 41.6% (112 de 269)
- **MFE Promedio:** 51.70 pts
- **MAE Promedio:** 48.19 pts
- **MFE/MAE Ratio:** 0.69
- **Good Entries:** 30.9% (MFE > MAE)
- **R:R Promedio:** 2.32

**❌ BAJA CALIDAD - Filtro correcto**

**5.0-10.0 ATR (Lejos)** (167 oportunidades)

- **WR Teórico:** 28.7% (si se hubieran ejecutado)
- **TP_FIRST:** 68.3% (114 de 167)
- **SL_FIRST:** 31.7% (53 de 167)
- **MFE Promedio:** 42.73 pts
- **MAE Promedio:** 44.61 pts
- **MFE/MAE Ratio:** 0.55
- **Good Entries:** 26.9% (MFE > MAE)
- **R:R Promedio:** 2.46

**❌ BAJA CALIDAD - Filtro correcto**

**>10.0 ATR (Muy lejos)** (29 oportunidades)

- **WR Teórico:** 17.2% (si se hubieran ejecutado)
- **TP_FIRST:** 96.6% (28 de 29)
- **SL_FIRST:** 3.4% (1 de 29)
- **MFE Promedio:** 19.98 pts
- **MAE Promedio:** 31.82 pts
- **MFE/MAE Ratio:** 0.73
- **Good Entries:** 17.2% (MFE > MAE)
- **R:R Promedio:** 2.51

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 11 | 182 | 121 | 269 |
| **TP_FIRST %** | 27.3% | 53.3% | 57.9% | 58.4% |
| **Good Entries %** | 72.7% | 26.9% | 23.1% | 30.9% |
| **MFE/MAE Ratio** | 2.09 | 2.94 | 0.71 | 0.69 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

✅ **CORRECTO: El filtro 2.0 ATR está bloqueando 121 oportunidades de BAJA calidad**
   - WR Teórico: 19.8%
   - **ACCIÓN:** Mantener MaxDistanceToRegister_ATR_Normal = 2.0

✅ **CORRECTO: Las 269 oportunidades en 3.0-5.0 ATR tienen baja calidad**


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 13.17 pts
- **Mediana:** 12.56 pts
- **Min/Max:** 8.79 / 18.52 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 12.56 |
| P70 | 15.54 |
| P80 | 17.33 |
| P90 | 18.52 |
| P95 | 18.52 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 26.59 pts
- **Mediana:** 29.75 pts
- **Min/Max:** 14.00 / 33.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 29.75 |
| P70 | 30.35 |
| P80 | 32.30 |
| P90 | 33.50 |
| P95 | 33.50 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 18; // Era 60
public int MaxTPDistancePoints { get; set; } = 33; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 18.5pts, TP: 33.5pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (36.4%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.364) / 0.364
R:R_min = 1.75
```

**Estado actual:** R:R promedio = 2.11
**Gap:** -0.36 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **18** (P90 real)
2. **MaxTPDistancePoints:** 120 → **33** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.11) < R:R mínimo (1.75)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=36.4%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-20 08:19:36*