# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-20 09:24:12
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251120_091921.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251120_091921.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 6
- **Win Rate:** 50.0% (3/6)
- **Profit Factor:** 3.18
- **Avg R:R Planeado:** 2.19
- **R:R Mínimo para Break-Even:** 1.00

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 13 puntos
   - TP máximo observado: 34 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.19
   - R:R necesario: 1.00
   - **Gap:** -1.19

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bullish | 1178 | 37.7% |
| Neutral | 793 | 25.4% |
| Bearish | 1156 | 37.0% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.016
- **Score Min/Max:** [-1.000, 0.990]
- **Componentes (promedio):**
  - EMA20 Slope: 0.002
  - EMA50 Cross: 0.002
  - BOS Count: -0.011
  - Regression 24h: 0.087

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.990 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 25.4% Neutral → bias no diferencia tendencias

### 2.3 Estado: Bias Compuesto Funcionando Correctamente

✅ **Threshold actual:** 0.3 (correcto)
✅ **Bias Neutral:** 25.4% (aceptable)
✅ **Score promedio:** 0.016

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 21808 | 100.0% | 100.0% |
| ProximityAnalyzer | 2291 | 10.5% | 10.5% |
| DFM_Evaluated | 653 | 28.5% | 3.0% |
| DFM_Passed | 653 | 100.0% | 3.0% |
| RiskCalculator | 5147 | 788.2% | 23.6% |
| Risk_Accepted | 34 | 0.7% | 0.2% |
| TradeManager | 6 | 17.6% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 5113 señales, -99.3%)
- **Tasa de conversión final:** 0.03% (de 21808 zonas iniciales → 6 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,077 | 52.6% |
| ENTRY_TOO_FAR | 597 | 29.1% |
| NO_SL | 278 | 13.6% |
| TP_CHECK_FAIL | 97 | 4.7% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,077 rechazos, 52.6%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 3106 | 79.4% |
| P0_ANY_DIR | 808 | 20.6% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 31.12 pts (máxima ganancia flotante)
- **MAE Promedio:** 33.92 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 1.49

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 4 | 66.7% |
| SL_FIRST (precio fue hacia SL) | 2 | 33.3% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 83.3%
- **Entradas Malas (MAE > MFE):** 16.7%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 6 | 4 | 2 | 66.7% | 31.12 | 33.92 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0006 | SELL | 30.00 | 20.25 | 1.48 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0011 | SELL | 0.00 | 73.75 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0019 | SELL | 26.75 | 23.50 | 1.14 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0019_2 | SELL | 26.75 | 23.50 | 1.14 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0022 | SELL | 41.75 | 10.50 | 3.98 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0033 | SELL | 61.50 | 52.00 | 1.18 | TP_FIRST | CLOSED | 👍 Entrada correcta |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,214

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 297 | 17.8% | 34.3% | 2.52 | 20.9% | 2.18 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 183 | 15.3% | 42.1% | 0.59 | 15.8% | 2.01 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 384 | 22.4% | 46.4% | 0.57 | 22.4% | 2.04 | ❌ BAJA CALIDAD - Filtro correcto |
| 5.0-10.0 ATR (Lejos) | 264 | 23.9% | 48.9% | 0.99 | 27.7% | 2.23 | ❌ BAJA CALIDAD - Filtro correcto |
| >10.0 ATR (Muy lejos) | 86 | 11.6% | 14.0% | 0.24 | 11.6% | 2.67 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (297 oportunidades)

- **WR Teórico:** 17.8% (si se hubieran ejecutado)
- **TP_FIRST:** 34.3% (102 de 297)
- **SL_FIRST:** 65.3% (194 de 297)
- **MFE Promedio:** 37.50 pts
- **MAE Promedio:** 56.65 pts
- **MFE/MAE Ratio:** 2.52
- **Good Entries:** 20.9% (MFE > MAE)
- **R:R Promedio:** 2.18

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (183 oportunidades)

- **WR Teórico:** 15.3% (si se hubieran ejecutado)
- **TP_FIRST:** 42.1% (77 de 183)
- **SL_FIRST:** 57.4% (105 de 183)
- **MFE Promedio:** 32.94 pts
- **MAE Promedio:** 53.44 pts
- **MFE/MAE Ratio:** 0.59
- **Good Entries:** 15.8% (MFE > MAE)
- **R:R Promedio:** 2.01

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (384 oportunidades)

- **WR Teórico:** 22.4% (si se hubieran ejecutado)
- **TP_FIRST:** 46.4% (178 de 384)
- **SL_FIRST:** 53.6% (206 de 384)
- **MFE Promedio:** 45.50 pts
- **MAE Promedio:** 51.60 pts
- **MFE/MAE Ratio:** 0.57
- **Good Entries:** 22.4% (MFE > MAE)
- **R:R Promedio:** 2.04

**❌ BAJA CALIDAD - Filtro correcto**

**5.0-10.0 ATR (Lejos)** (264 oportunidades)

- **WR Teórico:** 23.9% (si se hubieran ejecutado)
- **TP_FIRST:** 48.9% (129 de 264)
- **SL_FIRST:** 51.1% (135 de 264)
- **MFE Promedio:** 33.16 pts
- **MAE Promedio:** 44.77 pts
- **MFE/MAE Ratio:** 0.99
- **Good Entries:** 27.7% (MFE > MAE)
- **R:R Promedio:** 2.23

**❌ BAJA CALIDAD - Filtro correcto**

**>10.0 ATR (Muy lejos)** (86 oportunidades)

- **WR Teórico:** 11.6% (si se hubieran ejecutado)
- **TP_FIRST:** 14.0% (12 de 86)
- **SL_FIRST:** 86.0% (74 de 86)
- **MFE Promedio:** 15.41 pts
- **MAE Promedio:** 51.41 pts
- **MFE/MAE Ratio:** 0.24
- **Good Entries:** 11.6% (MFE > MAE)
- **R:R Promedio:** 2.67

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 6 | 297 | 183 | 384 |
| **TP_FIRST %** | 66.7% | 34.3% | 42.1% | 46.4% |
| **Good Entries %** | 83.3% | 20.9% | 15.8% | 22.4% |
| **MFE/MAE Ratio** | 1.49 | 2.52 | 0.59 | 0.57 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

✅ **CORRECTO: El filtro 2.0 ATR está bloqueando 183 oportunidades de BAJA calidad**
   - WR Teórico: 15.3%
   - **ACCIÓN:** Mantener MaxDistanceToRegister_ATR_Normal = 2.0

✅ **CORRECTO: Las 384 oportunidades en 3.0-5.0 ATR tienen baja calidad**


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 10.56 pts
- **Mediana:** 10.50 pts
- **Min/Max:** 8.79 / 12.56 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 10.49 |
| P70 | 12.03 |
| P80 | 12.35 |
| P90 | 12.72 |
| P95 | 12.90 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 23.08 pts
- **Mediana:** 23.25 pts
- **Min/Max:** 14.75 / 33.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 23.25 |
| P70 | 28.95 |
| P80 | 31.70 |
| P90 | 34.85 |
| P95 | 36.42 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 12; // Era 60
public int MaxTPDistancePoints { get; set; } = 34; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 12.7pts, TP: 34.9pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (50.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.500) / 0.500
R:R_min = 1.00
```

**Estado actual:** R:R promedio = 2.19
**Gap:** -1.19 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **12** (P90 real)
2. **MaxTPDistancePoints:** 120 → **34** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.19) < R:R mínimo (1.00)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=50.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-20 09:24:12*