# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-19 20:57:17
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_204639.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_204639.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 11
- **Win Rate:** 36.4% (4/11)
- **Profit Factor:** 1.38
- **Avg R:R Planeado:** 2.18
- **R:R Mínimo para Break-Even:** 1.75

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 19 puntos
   - TP máximo observado: 37 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.18
   - R:R necesario: 1.75
   - **Gap:** -0.43

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 814 | 26.0% |
| Bullish | 1159 | 37.0% |
| Bearish | 1156 | 36.9% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.010
- **Score Min/Max:** [-1.000, 0.990]
- **Componentes (promedio):**
  - EMA20 Slope: -0.007
  - EMA50 Cross: 0.010
  - BOS Count: -0.021
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
| StructureFusion | 24651 | 100.0% | 100.0% |
| ProximityAnalyzer | 3256 | 13.2% | 13.2% |
| DFM_Evaluated | 610 | 18.7% | 2.5% |
| DFM_Passed | 610 | 100.0% | 2.5% |
| RiskCalculator | 5987 | 981.5% | 24.3% |
| Risk_Accepted | 34 | 0.6% | 0.1% |
| TradeManager | 11 | 32.4% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 5953 señales, -99.4%)
- **Tasa de conversión final:** 0.04% (de 24651 zonas iniciales → 11 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,533 | 51.6% |
| ENTRY_TOO_FAR | 756 | 25.5% |
| TP_CHECK_FAIL | 393 | 13.2% |
| NO_SL | 288 | 9.7% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,533 rechazos, 51.6%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 3541 | 80.8% |
| P0_ANY_DIR | 843 | 19.2% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 27.48 pts (máxima ganancia flotante)
- **MAE Promedio:** 39.34 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 1.93

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 2 | 18.2% |
| SL_FIRST (precio fue hacia SL) | 9 | 81.8% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 63.6%
- **Entradas Malas (MAE > MFE):** 36.4%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 11 | 2 | 9 | 18.2% | 27.48 | 39.34 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0008 | SELL | 0.00 | 65.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0012 | SELL | 0.00 | 152.75 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0014 | SELL | 26.75 | 23.50 | 1.14 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0019 | SELL | 47.25 | 10.50 | 4.50 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0024 | SELL | 9.75 | 45.50 | 0.21 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0024_2 | SELL | 9.75 | 45.50 | 0.21 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0025 | SELL | 49.75 | 23.50 | 2.12 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0026 | SELL | 50.50 | 9.25 | 5.46 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0032 | SELL | 61.50 | 11.00 | 5.59 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0034 | SELL | 23.50 | 23.00 | 1.02 | SL_FIRST | CLOSED | 👍 Entrada correcta |
| T0034_2 | SELL | 23.50 | 23.00 | 1.02 | SL_FIRST | CLOSED | 👍 Entrada correcta |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 928

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 296 | 5.7% | 23.0% | 0.53 | 19.9% | 2.15 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 124 | 10.5% | 34.7% | 0.82 | 28.2% | 2.37 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 292 | 21.6% | 43.8% | 1.05 | 40.8% | 2.22 | ❌ BAJA CALIDAD - Filtro correcto |
| 5.0-10.0 ATR (Lejos) | 175 | 18.3% | 40.6% | 0.68 | 32.6% | 2.31 | ❌ BAJA CALIDAD - Filtro correcto |
| >10.0 ATR (Muy lejos) | 41 | 0.0% | 4.9% | 0.02 | 0.0% | 2.63 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (296 oportunidades)

- **WR Teórico:** 5.7% (si se hubieran ejecutado)
- **TP_FIRST:** 23.0% (68 de 296)
- **SL_FIRST:** 76.4% (226 de 296)
- **MFE Promedio:** 36.48 pts
- **MAE Promedio:** 96.38 pts
- **MFE/MAE Ratio:** 0.53
- **Good Entries:** 19.9% (MFE > MAE)
- **R:R Promedio:** 2.15

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (124 oportunidades)

- **WR Teórico:** 10.5% (si se hubieran ejecutado)
- **TP_FIRST:** 34.7% (43 de 124)
- **SL_FIRST:** 65.3% (81 de 124)
- **MFE Promedio:** 33.88 pts
- **MAE Promedio:** 69.04 pts
- **MFE/MAE Ratio:** 0.82
- **Good Entries:** 28.2% (MFE > MAE)
- **R:R Promedio:** 2.37

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (292 oportunidades)

- **WR Teórico:** 21.6% (si se hubieran ejecutado)
- **TP_FIRST:** 43.8% (128 de 292)
- **SL_FIRST:** 56.2% (164 de 292)
- **MFE Promedio:** 50.39 pts
- **MAE Promedio:** 60.34 pts
- **MFE/MAE Ratio:** 1.05
- **Good Entries:** 40.8% (MFE > MAE)
- **R:R Promedio:** 2.22

**❌ BAJA CALIDAD - Filtro correcto**

**5.0-10.0 ATR (Lejos)** (175 oportunidades)

- **WR Teórico:** 18.3% (si se hubieran ejecutado)
- **TP_FIRST:** 40.6% (71 de 175)
- **SL_FIRST:** 59.4% (104 de 175)
- **MFE Promedio:** 41.49 pts
- **MAE Promedio:** 52.33 pts
- **MFE/MAE Ratio:** 0.68
- **Good Entries:** 32.6% (MFE > MAE)
- **R:R Promedio:** 2.31

**❌ BAJA CALIDAD - Filtro correcto**

**>10.0 ATR (Muy lejos)** (41 oportunidades)

- **WR Teórico:** 0.0% (si se hubieran ejecutado)
- **TP_FIRST:** 4.9% (2 de 41)
- **SL_FIRST:** 95.1% (39 de 41)
- **MFE Promedio:** 16.25 pts
- **MAE Promedio:** 64.57 pts
- **MFE/MAE Ratio:** 0.02
- **Good Entries:** 0.0% (MFE > MAE)
- **R:R Promedio:** 2.63

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 11 | 296 | 124 | 292 |
| **TP_FIRST %** | 18.2% | 23.0% | 34.7% | 43.8% |
| **Good Entries %** | 63.6% | 19.9% | 28.2% | 40.8% |
| **MFE/MAE Ratio** | 1.93 | 0.53 | 0.82 | 1.05 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

✅ **CORRECTO: El filtro 2.0 ATR está bloqueando 124 oportunidades de BAJA calidad**
   - WR Teórico: 10.5%
   - **ACCIÓN:** Mantener MaxDistanceToRegister_ATR_Normal = 2.0

✅ **CORRECTO: Las 292 oportunidades en 3.0-5.0 ATR tienen baja calidad**


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 13.12 pts
- **Mediana:** 13.31 pts
- **Min/Max:** 8.79 / 18.52 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 13.31 |
| P70 | 15.54 |
| P80 | 17.33 |
| P90 | 18.52 |
| P95 | 18.52 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 27.23 pts
- **Mediana:** 29.75 pts
- **Min/Max:** 14.00 / 36.75 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 29.75 |
| P70 | 30.35 |
| P80 | 32.30 |
| P90 | 36.10 |
| P95 | 38.05 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 18; // Era 60
public int MaxTPDistancePoints { get; set; } = 36; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 18.5pts, TP: 36.1pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (36.4%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.364) / 0.364
R:R_min = 1.75
```

**Estado actual:** R:R promedio = 2.18
**Gap:** -0.43 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **36** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.18) < R:R mínimo (1.75)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=36.4%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-19 20:57:17*