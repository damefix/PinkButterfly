# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-19 19:42:59
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_192349.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_192349.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 16
- **Win Rate:** 43.8% (7/16)
- **Profit Factor:** 1.96
- **Avg R:R Planeado:** 2.35
- **R:R Mínimo para Break-Even:** 1.29

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 19 puntos
   - TP máximo observado: 37 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.35
   - R:R necesario: 1.29
   - **Gap:** -1.07

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 814 | 26.0% |
| Bullish | 1159 | 37.1% |
| Bearish | 1155 | 36.9% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.010
- **Score Min/Max:** [-1.000, 0.990]
- **Componentes (promedio):**
  - EMA20 Slope: -0.007
  - EMA50 Cross: 0.013
  - BOS Count: -0.026
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
| StructureFusion | 22953 | 100.0% | 100.0% |
| ProximityAnalyzer | 3214 | 14.0% | 14.0% |
| DFM_Evaluated | 669 | 20.8% | 2.9% |
| DFM_Passed | 669 | 100.0% | 2.9% |
| RiskCalculator | 5958 | 890.6% | 26.0% |
| Risk_Accepted | 1 | 0.0% | 0.0% |
| TradeManager | 16 | 1600.0% | 0.1% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 5957 señales, -100.0%)
- **Tasa de conversión final:** 0.07% (de 22953 zonas iniciales → 16 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,674 | 54.4% |
| ENTRY_TOO_FAR | 773 | 25.1% |
| TP_CHECK_FAIL | 411 | 13.4% |
| NO_SL | 220 | 7.1% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,674 rechazos, 54.4%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 3515 | 81.3% |
| P0_ANY_DIR | 807 | 18.7% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 29.55 pts (máxima ganancia flotante)
- **MAE Promedio:** 38.30 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 2.45

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 3 | 18.8% |
| SL_FIRST (precio fue hacia SL) | 13 | 81.2% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 50.0%
- **Entradas Malas (MAE > MFE):** 50.0%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 16 | 3 | 13 | 18.8% | 29.55 | 38.30 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0002 | SELL | 16.25 | 30.75 | 0.53 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0006 | SELL | 0.00 | 70.00 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0007 | SELL | 0.00 | 66.00 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0007_2 | SELL | 0.00 | 66.00 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0011 | SELL | 0.00 | 149.50 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0016 | SELL | 25.25 | 26.00 | 0.97 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0019 | SELL | 110.75 | 9.00 | 12.31 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0022 | SELL | 41.75 | 10.50 | 3.98 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0027 | SELL | 9.75 | 45.50 | 0.21 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0027_2 | SELL | 9.75 | 45.50 | 0.21 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0030 | SELL | 49.75 | 23.50 | 2.12 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0032 | SELL | 50.50 | 9.25 | 5.46 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0032_2 | SELL | 50.50 | 9.25 | 5.46 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0038 | SELL | 61.50 | 11.00 | 5.59 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0040 | SELL | 23.50 | 20.50 | 1.15 | SL_FIRST | CLOSED | 👍 Entrada correcta |
| T0040_2 | SELL | 23.50 | 20.50 | 1.15 | SL_FIRST | CLOSED | 👍 Entrada correcta |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,044

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 344 | 4.7% | 24.7% | 1.40 | 23.8% | 2.01 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 148 | 11.5% | 32.4% | 0.83 | 31.1% | 2.21 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 309 | 23.6% | 39.5% | 0.90 | 37.2% | 2.16 | ❌ BAJA CALIDAD - Filtro correcto |
| 5.0-10.0 ATR (Lejos) | 235 | 15.7% | 33.2% | 0.63 | 28.9% | 2.20 | ❌ BAJA CALIDAD - Filtro correcto |
| >10.0 ATR (Muy lejos) | 8 | 12.5% | 12.5% | 0.08 | 0.0% | 2.01 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (344 oportunidades)

- **WR Teórico:** 4.7% (si se hubieran ejecutado)
- **TP_FIRST:** 24.7% (85 de 344)
- **SL_FIRST:** 74.7% (257 de 344)
- **MFE Promedio:** 27.79 pts
- **MAE Promedio:** 85.61 pts
- **MFE/MAE Ratio:** 1.40
- **Good Entries:** 23.8% (MFE > MAE)
- **R:R Promedio:** 2.01

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (148 oportunidades)

- **WR Teórico:** 11.5% (si se hubieran ejecutado)
- **TP_FIRST:** 32.4% (48 de 148)
- **SL_FIRST:** 66.9% (99 de 148)
- **MFE Promedio:** 27.51 pts
- **MAE Promedio:** 63.18 pts
- **MFE/MAE Ratio:** 0.83
- **Good Entries:** 31.1% (MFE > MAE)
- **R:R Promedio:** 2.21

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (309 oportunidades)

- **WR Teórico:** 23.6% (si se hubieran ejecutado)
- **TP_FIRST:** 39.5% (122 de 309)
- **SL_FIRST:** 60.2% (186 de 309)
- **MFE Promedio:** 41.12 pts
- **MAE Promedio:** 54.32 pts
- **MFE/MAE Ratio:** 0.90
- **Good Entries:** 37.2% (MFE > MAE)
- **R:R Promedio:** 2.16

**❌ BAJA CALIDAD - Filtro correcto**

**5.0-10.0 ATR (Lejos)** (235 oportunidades)

- **WR Teórico:** 15.7% (si se hubieran ejecutado)
- **TP_FIRST:** 33.2% (78 de 235)
- **SL_FIRST:** 66.8% (157 de 235)
- **MFE Promedio:** 35.10 pts
- **MAE Promedio:** 52.70 pts
- **MFE/MAE Ratio:** 0.63
- **Good Entries:** 28.9% (MFE > MAE)
- **R:R Promedio:** 2.20

**❌ BAJA CALIDAD - Filtro correcto**

**>10.0 ATR (Muy lejos)** (8 oportunidades)

- **WR Teórico:** 12.5% (si se hubieran ejecutado)
- **TP_FIRST:** 12.5% (1 de 8)
- **SL_FIRST:** 87.5% (7 de 8)
- **MFE Promedio:** 5.69 pts
- **MAE Promedio:** 52.16 pts
- **MFE/MAE Ratio:** 0.08
- **Good Entries:** 0.0% (MFE > MAE)
- **R:R Promedio:** 2.01

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 16 | 344 | 148 | 309 |
| **TP_FIRST %** | 18.8% | 24.7% | 32.4% | 39.5% |
| **Good Entries %** | 50.0% | 23.8% | 31.1% | 37.2% |
| **MFE/MAE Ratio** | 2.45 | 1.40 | 0.83 | 0.90 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

✅ **CORRECTO: El filtro 2.0 ATR está bloqueando 148 oportunidades de BAJA calidad**
   - WR Teórico: 11.5%
   - **ACCIÓN:** Mantener MaxDistanceToRegister_ATR_Normal = 2.0

✅ **CORRECTO: Las 309 oportunidades en 3.0-5.0 ATR tienen baja calidad**


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 12.90 pts
- **Mediana:** 13.28 pts
- **Min/Max:** 3.30 / 18.52 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 13.28 |
| P70 | 15.47 |
| P80 | 15.54 |
| P90 | 18.52 |
| P95 | 18.52 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 27.69 pts
- **Mediana:** 29.62 pts
- **Min/Max:** 13.00 / 36.75 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 29.62 |
| P70 | 30.48 |
| P80 | 32.30 |
| P90 | 34.48 |
| P95 | 37.24 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 18; // Era 60
public int MaxTPDistancePoints { get; set; } = 34; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 18.5pts, TP: 34.5pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (43.8%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.438) / 0.438
R:R_min = 1.29
```

**Estado actual:** R:R promedio = 2.35
**Gap:** -1.07 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **34** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.35) < R:R mínimo (1.29)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=43.8%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-19 19:42:59*