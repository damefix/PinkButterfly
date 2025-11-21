# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-19 13:42:06
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_133723.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_133723.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 4
- **Win Rate:** 75.0% (3/4)
- **Profit Factor:** 22.47
- **Avg R:R Planeado:** 3.06
- **R:R Mínimo para Break-Even:** 0.33

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 20 puntos
   - TP máximo observado: 31 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 3.06
   - R:R necesario: 0.33
   - **Gap:** -2.72

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bullish | 1178 | 37.7% |
| Neutral | 795 | 25.4% |
| Bearish | 1155 | 36.9% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.014
- **Score Min/Max:** [-1.000, 0.990]
- **Componentes (promedio):**
  - EMA20 Slope: -0.008
  - EMA50 Cross: 0.028
  - BOS Count: -0.029
  - Regression 24h: 0.081

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.990 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 25.4% Neutral → bias no diferencia tendencias

### 2.3 Estado: Bias Compuesto Funcionando Correctamente

✅ **Threshold actual:** 0.3 (correcto)
✅ **Bias Neutral:** 25.4% (aceptable)
✅ **Score promedio:** 0.014

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 18523 | 100.0% | 100.0% |
| ProximityAnalyzer | 3233 | 17.5% | 17.5% |
| DFM_Evaluated | 555 | 17.2% | 3.0% |
| DFM_Passed | 555 | 100.0% | 3.0% |
| RiskCalculator | 5860 | 1055.9% | 31.6% |
| Risk_Accepted | 44 | 0.8% | 0.2% |
| TradeManager | 4 | 9.1% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 5816 señales, -99.2%)
- **Tasa de conversión final:** 0.02% (de 18523 zonas iniciales → 4 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,779 | 60.3% |
| ENTRY_TOO_FAR | 717 | 24.3% |
| TP_CHECK_FAIL | 264 | 8.9% |
| NO_SL | 192 | 6.5% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,779 rechazos, 60.3%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 3210 | 79.4% |
| P0_ANY_DIR | 833 | 20.6% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 15.25 pts (máxima ganancia flotante)
- **MAE Promedio:** 60.75 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 1.38

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 1 | 25.0% |
| SL_FIRST (precio fue hacia SL) | 3 | 75.0% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 25.0%
- **Entradas Malas (MAE > MFE):** 75.0%

⚠️ **ALERTA:** >60% de entradas tienen MAE > MFE
- **Problema:** El precio va más en contra que a favor antes del cierre
- **Causas posibles:**
  1. Timing incorrecto (entramos antes de reversión)
  2. Bias desincronizado (operamos contra tendencia real)
  3. Zonas de baja calidad (sin confluence real)

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 4 | 1 | 3 | 25.0% | 15.25 | 60.75 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | SELL | 0.00 | 142.00 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0005 | SELL | 2.00 | 48.25 | 0.04 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0012 | SELL | 7.25 | 43.00 | 0.17 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0025 | SELL | 51.75 | 9.75 | 5.31 | SL_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 872

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 266 | 28.2% | 27.4% | 1.05 | 29.7% | 2.24 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 133 | 36.1% | 35.3% | 0.70 | 33.1% | 2.20 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 267 | 34.1% | 41.2% | 0.60 | 33.0% | 2.27 | ❌ BAJA CALIDAD - Filtro correcto |
| 5.0-10.0 ATR (Lejos) | 182 | 20.3% | 42.9% | 0.18 | 20.3% | 2.36 | ❌ BAJA CALIDAD - Filtro correcto |
| >10.0 ATR (Muy lejos) | 24 | 0.0% | 4.2% | 0.02 | 0.0% | 2.26 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (266 oportunidades)

- **WR Teórico:** 28.2% (si se hubieran ejecutado)
- **TP_FIRST:** 27.4% (73 de 266)
- **SL_FIRST:** 64.3% (171 de 266)
- **MFE Promedio:** 62.53 pts
- **MAE Promedio:** 74.85 pts
- **MFE/MAE Ratio:** 1.05
- **Good Entries:** 29.7% (MFE > MAE)
- **R:R Promedio:** 2.24

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (133 oportunidades)

- **WR Teórico:** 36.1% (si se hubieran ejecutado)
- **TP_FIRST:** 35.3% (47 de 133)
- **SL_FIRST:** 60.2% (80 de 133)
- **MFE Promedio:** 62.64 pts
- **MAE Promedio:** 69.11 pts
- **MFE/MAE Ratio:** 0.70
- **Good Entries:** 33.1% (MFE > MAE)
- **R:R Promedio:** 2.20

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (267 oportunidades)

- **WR Teórico:** 34.1% (si se hubieran ejecutado)
- **TP_FIRST:** 41.2% (110 de 267)
- **SL_FIRST:** 52.8% (141 de 267)
- **MFE Promedio:** 57.63 pts
- **MAE Promedio:** 61.72 pts
- **MFE/MAE Ratio:** 0.60
- **Good Entries:** 33.0% (MFE > MAE)
- **R:R Promedio:** 2.27

**❌ BAJA CALIDAD - Filtro correcto**

**5.0-10.0 ATR (Lejos)** (182 oportunidades)

- **WR Teórico:** 20.3% (si se hubieran ejecutado)
- **TP_FIRST:** 42.9% (78 de 182)
- **SL_FIRST:** 46.7% (85 de 182)
- **MFE Promedio:** 40.11 pts
- **MAE Promedio:** 57.83 pts
- **MFE/MAE Ratio:** 0.18
- **Good Entries:** 20.3% (MFE > MAE)
- **R:R Promedio:** 2.36

**❌ BAJA CALIDAD - Filtro correcto**

**>10.0 ATR (Muy lejos)** (24 oportunidades)

- **WR Teórico:** 0.0% (si se hubieran ejecutado)
- **TP_FIRST:** 4.2% (1 de 24)
- **SL_FIRST:** 87.5% (21 de 24)
- **MFE Promedio:** 5.25 pts
- **MAE Promedio:** 62.36 pts
- **MFE/MAE Ratio:** 0.02
- **Good Entries:** 0.0% (MFE > MAE)
- **R:R Promedio:** 2.26

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 4 | 266 | 133 | 267 |
| **TP_FIRST %** | 25.0% | 27.4% | 35.3% | 41.2% |
| **Good Entries %** | 25.0% | 29.7% | 33.1% | 33.0% |
| **MFE/MAE Ratio** | 1.38 | 1.05 | 0.70 | 0.60 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

✅ **CORRECTO: Las 267 oportunidades en 3.0-5.0 ATR tienen baja calidad**


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 9.93 pts
- **Mediana:** 8.32 pts
- **Min/Max:** 3.26 / 19.79 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 8.32 |
| P70 | 14.29 |
| P80 | 19.79 |
| P90 | 25.29 |
| P95 | 28.03 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 22.94 pts
- **Mediana:** 23.50 pts
- **Min/Max:** 13.50 / 31.25 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 23.50 |
| P70 | 29.88 |
| P80 | 31.25 |
| P90 | 32.62 |
| P95 | 33.31 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 25; // Era 60
public int MaxTPDistancePoints { get; set; } = 32; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 25.3pts, TP: 32.6pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (75.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.750) / 0.750
R:R_min = 0.33
```

**Estado actual:** R:R promedio = 3.06
**Gap:** -2.72 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **32** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (3.06) < R:R mínimo (0.33)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=75.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-19 13:42:06*