# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-19 18:49:50
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_184226.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_184226.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 11
- **Win Rate:** 36.4% (4/11)
- **Profit Factor:** 1.33
- **Avg R:R Planeado:** 2.28
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
   - R:R actual: 2.28
   - R:R necesario: 1.75
   - **Gap:** -0.53

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
- **Score Promedio:** 0.011
- **Score Min/Max:** [-1.000, 0.990]
- **Componentes (promedio):**
  - EMA20 Slope: -0.008
  - EMA50 Cross: 0.015
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
✅ **Score promedio:** 0.011

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 21889 | 100.0% | 100.0% |
| ProximityAnalyzer | 2966 | 13.6% | 13.6% |
| DFM_Evaluated | 498 | 16.8% | 2.3% |
| DFM_Passed | 498 | 100.0% | 2.3% |
| RiskCalculator | 5559 | 1116.3% | 25.4% |
| Risk_Accepted | 1 | 0.0% | 0.0% |
| TradeManager | 11 | 1100.0% | 0.1% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 5558 señales, -100.0%)
- **Tasa de conversión final:** 0.05% (de 21889 zonas iniciales → 11 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,935 | 60.7% |
| ENTRY_TOO_FAR | 640 | 20.1% |
| TP_CHECK_FAIL | 396 | 12.4% |
| NO_SL | 219 | 6.9% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,935 rechazos, 60.7%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 3407 | 86.6% |
| P0_ANY_DIR | 528 | 13.4% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 45.52 pts (máxima ganancia flotante)
- **MAE Promedio:** 24.07 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 3.20

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 2 | 18.2% |
| SL_FIRST (precio fue hacia SL) | 9 | 81.8% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 81.8%
- **Entradas Malas (MAE > MFE):** 18.2%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 11 | 2 | 9 | 18.2% | 45.52 | 24.07 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0005 | SELL | 6.75 | 43.50 | 0.16 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0012 | SELL | 25.25 | 26.00 | 0.97 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0015 | SELL | 110.75 | 9.00 | 12.31 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0018 | SELL | 41.75 | 10.50 | 3.98 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0023 | SELL | 53.75 | 45.50 | 1.18 | SL_FIRST | CLOSED | 👍 Entrada correcta |
| T0023_2 | SELL | 53.75 | 45.50 | 1.18 | SL_FIRST | CLOSED | 👍 Entrada correcta |
| T0024 | SELL | 49.75 | 23.50 | 2.12 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0025 | SELL | 50.50 | 9.25 | 5.46 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0031 | SELL | 61.50 | 11.00 | 5.59 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0033 | SELL | 23.50 | 20.50 | 1.15 | SL_FIRST | CLOSED | 👍 Entrada correcta |
| T0033_2 | SELL | 23.50 | 20.50 | 1.15 | SL_FIRST | CLOSED | 👍 Entrada correcta |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 703

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 205 | 7.8% | 29.3% | 0.63 | 24.9% | 2.22 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 112 | 15.2% | 20.5% | 0.61 | 20.5% | 2.19 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 263 | 27.4% | 43.7% | 1.06 | 43.3% | 2.24 | ❌ BAJA CALIDAD - Filtro correcto |
| 5.0-10.0 ATR (Lejos) | 119 | 31.1% | 57.1% | 1.07 | 52.1% | 2.27 | ⚠️ CALIDAD MEDIA - Revisar |
| >10.0 ATR (Muy lejos) | 4 | 25.0% | 25.0% | 0.14 | 0.0% | 1.97 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (205 oportunidades)

- **WR Teórico:** 7.8% (si se hubieran ejecutado)
- **TP_FIRST:** 29.3% (60 de 205)
- **SL_FIRST:** 70.2% (144 de 205)
- **MFE Promedio:** 40.45 pts
- **MAE Promedio:** 81.86 pts
- **MFE/MAE Ratio:** 0.63
- **Good Entries:** 24.9% (MFE > MAE)
- **R:R Promedio:** 2.22

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (112 oportunidades)

- **WR Teórico:** 15.2% (si se hubieran ejecutado)
- **TP_FIRST:** 20.5% (23 de 112)
- **SL_FIRST:** 79.5% (89 de 112)
- **MFE Promedio:** 44.49 pts
- **MAE Promedio:** 85.86 pts
- **MFE/MAE Ratio:** 0.61
- **Good Entries:** 20.5% (MFE > MAE)
- **R:R Promedio:** 2.19

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (263 oportunidades)

- **WR Teórico:** 27.4% (si se hubieran ejecutado)
- **TP_FIRST:** 43.7% (115 de 263)
- **SL_FIRST:** 56.3% (148 de 263)
- **MFE Promedio:** 58.23 pts
- **MAE Promedio:** 66.03 pts
- **MFE/MAE Ratio:** 1.06
- **Good Entries:** 43.3% (MFE > MAE)
- **R:R Promedio:** 2.24

**❌ BAJA CALIDAD - Filtro correcto**

**5.0-10.0 ATR (Lejos)** (119 oportunidades)

- **WR Teórico:** 31.1% (si se hubieran ejecutado)
- **TP_FIRST:** 57.1% (68 de 119)
- **SL_FIRST:** 42.9% (51 de 119)
- **MFE Promedio:** 53.33 pts
- **MAE Promedio:** 46.33 pts
- **MFE/MAE Ratio:** 1.07
- **Good Entries:** 52.1% (MFE > MAE)
- **R:R Promedio:** 2.27

**⚠️ CALIDAD MEDIA - Revisar**

**>10.0 ATR (Muy lejos)** (4 oportunidades)

- **WR Teórico:** 25.0% (si se hubieran ejecutado)
- **TP_FIRST:** 25.0% (1 de 4)
- **SL_FIRST:** 75.0% (3 de 4)
- **MFE Promedio:** 9.62 pts
- **MAE Promedio:** 48.12 pts
- **MFE/MAE Ratio:** 0.14
- **Good Entries:** 0.0% (MFE > MAE)
- **R:R Promedio:** 1.97

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 11 | 205 | 112 | 263 |
| **TP_FIRST %** | 18.2% | 29.3% | 20.5% | 43.7% |
| **Good Entries %** | 81.8% | 24.9% | 20.5% | 43.3% |
| **MFE/MAE Ratio** | 3.20 | 0.63 | 0.61 | 1.06 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

✅ **CORRECTO: El filtro 2.0 ATR está bloqueando 112 oportunidades de BAJA calidad**
   - WR Teórico: 15.2%
   - **ACCIÓN:** Mantener MaxDistanceToRegister_ATR_Normal = 2.0

✅ **CORRECTO: Las 263 oportunidades en 3.0-5.0 ATR tienen baja calidad**


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 12.79 pts
- **Mediana:** 12.63 pts
- **Min/Max:** 3.30 / 18.52 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 12.63 |
| P70 | 15.54 |
| P80 | 17.33 |
| P90 | 18.52 |
| P95 | 18.52 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 26.11 pts
- **Mediana:** 28.50 pts
- **Min/Max:** 13.00 / 33.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 28.50 |
| P70 | 30.25 |
| P80 | 30.40 |
| P90 | 32.90 |
| P95 | 34.70 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 18; // Era 60
public int MaxTPDistancePoints { get; set; } = 32; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 18.5pts, TP: 32.9pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (36.4%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.364) / 0.364
R:R_min = 1.75
```

**Estado actual:** R:R promedio = 2.28
**Gap:** -0.53 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **32** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.28) < R:R mínimo (1.75)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=36.4%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-19 18:49:50*