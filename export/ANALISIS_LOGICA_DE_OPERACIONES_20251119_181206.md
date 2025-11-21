# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-19 18:39:34
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_181206.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_181206.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 10
- **Win Rate:** 40.0% (4/10)
- **Profit Factor:** 1.44
- **Avg R:R Planeado:** 2.63
- **R:R Mínimo para Break-Even:** 1.50

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 19 puntos
   - TP máximo observado: 46 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.63
   - R:R necesario: 1.50
   - **Gap:** -1.13

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 813 | 26.0% |
| Bullish | 1159 | 37.1% |
| Bearish | 1155 | 36.9% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.010
- **Score Min/Max:** [-1.000, 0.990]
- **Componentes (promedio):**
  - EMA20 Slope: -0.008
  - EMA50 Cross: 0.016
  - BOS Count: -0.027
  - Regression 24h: 0.077

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.990 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 26.0% Neutral → bias no diferencia tendencias

### 2.3 Estado: Bias Compuesto Funcionando Correctamente

✅ **Threshold actual:** 0.3 (correcto)
✅ **Bias Neutral:** 26.0% (aceptable)
✅ **Score promedio:** 0.010

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 21703 | 100.0% | 100.0% |
| ProximityAnalyzer | 2944 | 13.6% | 13.6% |
| DFM_Evaluated | 506 | 17.2% | 2.3% |
| DFM_Passed | 506 | 100.0% | 2.3% |
| RiskCalculator | 5442 | 1075.5% | 25.1% |
| Risk_Accepted | 1 | 0.0% | 0.0% |
| TradeManager | 10 | 1000.0% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 5441 señales, -100.0%)
- **Tasa de conversión final:** 0.05% (de 21703 zonas iniciales → 10 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,860 | 60.2% |
| ENTRY_TOO_FAR | 640 | 20.7% |
| TP_CHECK_FAIL | 395 | 12.8% |
| NO_SL | 197 | 6.4% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,860 rechazos, 60.2%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 3379 | 87.5% |
| P0_ANY_DIR | 484 | 12.5% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 46.40 pts (máxima ganancia flotante)
- **MAE Promedio:** 22.32 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 3.53

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 3 | 30.0% |
| SL_FIRST (precio fue hacia SL) | 7 | 70.0% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 80.0%
- **Entradas Malas (MAE > MFE):** 20.0%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 10 | 3 | 7 | 30.0% | 46.40 | 22.32 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0002 | SELL | 36.00 | 14.25 | 2.53 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0005 | SELL | 36.00 | 14.25 | 2.53 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0007 | SELL | 6.75 | 43.50 | 0.16 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0019 | SELL | 110.75 | 9.00 | 12.31 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0022 | SELL | 41.75 | 10.50 | 3.98 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0028 | SELL | 57.75 | 45.50 | 1.27 | SL_FIRST | CLOSED | 👍 Entrada correcta |
| T0028_2 | SELL | 57.75 | 45.50 | 1.27 | SL_FIRST | CLOSED | 👍 Entrada correcta |
| T0029 | SELL | 50.50 | 9.25 | 5.46 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0035 | SELL | 61.50 | 11.00 | 5.59 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0037 | SELL | 5.25 | 20.50 | 0.26 | SL_FIRST | CLOSED | ❌ Entrada muy mala |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 724

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 223 | 12.1% | 30.0% | 1.01 | 30.9% | 2.23 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 113 | 13.3% | 21.2% | 0.74 | 21.2% | 2.19 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 260 | 25.0% | 42.7% | 1.06 | 42.7% | 2.27 | ❌ BAJA CALIDAD - Filtro correcto |
| 5.0-10.0 ATR (Lejos) | 124 | 29.8% | 58.1% | 1.08 | 54.0% | 2.29 | ❌ BAJA CALIDAD - Filtro correcto |
| >10.0 ATR (Muy lejos) | 4 | 0.0% | 0.0% | 0.07 | 0.0% | 1.97 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (223 oportunidades)

- **WR Teórico:** 12.1% (si se hubieran ejecutado)
- **TP_FIRST:** 30.0% (67 de 223)
- **SL_FIRST:** 69.1% (154 de 223)
- **MFE Promedio:** 58.75 pts
- **MAE Promedio:** 78.15 pts
- **MFE/MAE Ratio:** 1.01
- **Good Entries:** 30.9% (MFE > MAE)
- **R:R Promedio:** 2.23

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (113 oportunidades)

- **WR Teórico:** 13.3% (si se hubieran ejecutado)
- **TP_FIRST:** 21.2% (24 de 113)
- **SL_FIRST:** 78.8% (89 de 113)
- **MFE Promedio:** 50.76 pts
- **MAE Promedio:** 86.29 pts
- **MFE/MAE Ratio:** 0.74
- **Good Entries:** 21.2% (MFE > MAE)
- **R:R Promedio:** 2.19

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (260 oportunidades)

- **WR Teórico:** 25.0% (si se hubieran ejecutado)
- **TP_FIRST:** 42.7% (111 de 260)
- **SL_FIRST:** 57.3% (149 de 260)
- **MFE Promedio:** 58.80 pts
- **MAE Promedio:** 66.19 pts
- **MFE/MAE Ratio:** 1.06
- **Good Entries:** 42.7% (MFE > MAE)
- **R:R Promedio:** 2.27

**❌ BAJA CALIDAD - Filtro correcto**

**5.0-10.0 ATR (Lejos)** (124 oportunidades)

- **WR Teórico:** 29.8% (si se hubieran ejecutado)
- **TP_FIRST:** 58.1% (72 de 124)
- **SL_FIRST:** 41.9% (52 de 124)
- **MFE Promedio:** 58.56 pts
- **MAE Promedio:** 46.19 pts
- **MFE/MAE Ratio:** 1.08
- **Good Entries:** 54.0% (MFE > MAE)
- **R:R Promedio:** 2.29

**❌ BAJA CALIDAD - Filtro correcto**

**>10.0 ATR (Muy lejos)** (4 oportunidades)

- **WR Teórico:** 0.0% (si se hubieran ejecutado)
- **TP_FIRST:** 0.0% (0 de 4)
- **SL_FIRST:** 100.0% (4 de 4)
- **MFE Promedio:** 5.50 pts
- **MAE Promedio:** 48.12 pts
- **MFE/MAE Ratio:** 0.07
- **Good Entries:** 0.0% (MFE > MAE)
- **R:R Promedio:** 1.97

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 10 | 223 | 113 | 260 |
| **TP_FIRST %** | 30.0% | 30.0% | 21.2% | 42.7% |
| **Good Entries %** | 80.0% | 30.9% | 21.2% | 42.7% |
| **MFE/MAE Ratio** | 3.53 | 1.01 | 0.74 | 1.06 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

✅ **CORRECTO: El filtro 2.0 ATR está bloqueando 113 oportunidades de BAJA calidad**
   - WR Teórico: 13.3%
   - **ACCIÓN:** Mantener MaxDistanceToRegister_ATR_Normal = 2.0

✅ **CORRECTO: Las 260 oportunidades en 3.0-5.0 ATR tienen baja calidad**


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 12.66 pts
- **Mediana:** 12.86 pts
- **Min/Max:** 3.30 / 18.52 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 12.86 |
| P70 | 15.24 |
| P80 | 17.92 |
| P90 | 18.52 |
| P95 | 18.52 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 30.45 pts
- **Mediana:** 30.25 pts
- **Min/Max:** 13.00 / 45.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 30.25 |
| P70 | 32.60 |
| P80 | 43.10 |
| P90 | 45.50 |
| P95 | 45.50 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 18; // Era 60
public int MaxTPDistancePoints { get; set; } = 45; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 18.5pts, TP: 45.5pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (40.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.400) / 0.400
R:R_min = 1.50
```

**Estado actual:** R:R promedio = 2.63
**Gap:** -1.13 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **45** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.63) < R:R mínimo (1.50)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=40.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-19 18:39:34*