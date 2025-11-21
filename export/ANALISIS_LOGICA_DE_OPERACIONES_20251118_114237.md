# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-18 11:52:01
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251118_114237.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251118_114237.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 25
- **Win Rate:** 44.0% (11/25)
- **Profit Factor:** 0.98
- **Avg R:R Planeado:** 2.20
- **R:R Mínimo para Break-Even:** 1.27

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 26 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.20
   - R:R necesario: 1.27
   - **Gap:** -0.93

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 925 | 29.6% |
| Bearish | 942 | 30.1% |
| Bullish | 1260 | 40.3% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.075
- **Score Min/Max:** [-1.000, 1.000]
- **Componentes (promedio):**
  - EMA20 Slope: 0.028
  - EMA50 Cross: 0.077
  - BOS Count: 0.098
  - Regression 24h: 0.114

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 1.000 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 29.6% Neutral → bias no diferencia tendencias

### 2.3 Estado: Bias Compuesto Funcionando Correctamente

✅ **Threshold actual:** 0.3 (correcto)
✅ **Bias Neutral:** 29.6% (aceptable)
✅ **Score promedio:** 0.075

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 12317 | 100.0% | 100.0% |
| ProximityAnalyzer | 2835 | 23.0% | 23.0% |
| DFM_Evaluated | 481 | 17.0% | 3.9% |
| DFM_Passed | 481 | 100.0% | 3.9% |
| RiskCalculator | 5475 | 1138.3% | 44.5% |
| Risk_Accepted | 67 | 1.2% | 0.5% |
| TradeManager | 25 | 37.3% | 0.2% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 5408 señales, -98.8%)
- **Tasa de conversión final:** 0.20% (de 12317 zonas iniciales → 25 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,441 | 41.8% |
| TP_CHECK_FAIL | 998 | 29.0% |
| NO_SL | 616 | 17.9% |
| ENTRY_TOO_FAR | 391 | 11.3% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,441 rechazos, 41.8%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2202 | 88.8% |
| P0_ANY_DIR | 278 | 11.2% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 38.28 pts (máxima ganancia flotante)
- **MAE Promedio:** 28.49 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 167.57

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 17 | 68.0% |
| SL_FIRST (precio fue hacia SL) | 8 | 32.0% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 52.0%
- **Entradas Malas (MAE > MFE):** 48.0%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 25 | 17 | 8 | 68.0% | 38.28 | 28.49 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0002 | SELL | 7.75 | 17.75 | 0.44 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0003 | SELL | 2.50 | 28.75 | 0.09 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0005 | SELL | 15.75 | 16.00 | 0.98 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0009 | BUY | 45.25 | 53.75 | 0.84 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0016 | SELL | 78.00 | 13.00 | 6.00 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0017 | SELL | 78.00 | 13.00 | 6.00 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0023 | SELL | 59.00 | 0.50 | 118.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0026 | SELL | 24.25 | 61.75 | 0.39 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0029 | SELL | 36.25 | 3.00 | 12.08 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0030 | SELL | 36.25 | 2.25 | 16.11 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0031 | SELL | 50.00 | 2.25 | 22.22 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0033 | SELL | 113.25 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0041 | SELL | 37.75 | 64.75 | 0.58 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0041_2 | SELL | 37.75 | 64.75 | 0.58 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0041_3 | SELL | 37.75 | 64.75 | 0.58 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0044 | SELL | 16.50 | 32.75 | 0.50 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0046 | BUY | 16.00 | 10.75 | 1.49 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0047 | SELL | 21.00 | 11.25 | 1.87 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0053 | SELL | 41.75 | 10.50 | 3.98 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0058 | SELL | 0.00 | 103.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 759

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 282 | 33.3% | 52.5% | 5.09 | 52.1% | 2.26 | ⚠️ CALIDAD MEDIA - Revisar |
| 2.0-3.0 ATR (Cerca) | 136 | 45.6% | 60.3% | 3.10 | 56.6% | 2.54 | ⚠️ CALIDAD MEDIA - Revisar |
| 3.0-5.0 ATR (Media) | 219 | 71.7% | 65.8% | 4.41 | 69.4% | 2.41 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 111 | 71.2% | 61.3% | 1.42 | 66.7% | 3.51 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 11 | 0.0% | 0.0% | 0.00 | 0.0% | 6.11 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (282 oportunidades)

- **WR Teórico:** 33.3% (si se hubieran ejecutado)
- **TP_FIRST:** 52.5% (148 de 282)
- **SL_FIRST:** 46.1% (130 de 282)
- **MFE Promedio:** 46.79 pts
- **MAE Promedio:** 35.31 pts
- **MFE/MAE Ratio:** 5.09
- **Good Entries:** 52.1% (MFE > MAE)
- **R:R Promedio:** 2.26

**⚠️ CALIDAD MEDIA - Revisar**

**2.0-3.0 ATR (Cerca)** (136 oportunidades)

- **WR Teórico:** 45.6% (si se hubieran ejecutado)
- **TP_FIRST:** 60.3% (82 de 136)
- **SL_FIRST:** 39.7% (54 de 136)
- **MFE Promedio:** 62.93 pts
- **MAE Promedio:** 44.27 pts
- **MFE/MAE Ratio:** 3.10
- **Good Entries:** 56.6% (MFE > MAE)
- **R:R Promedio:** 2.54

**⚠️ CALIDAD MEDIA - Revisar**

**3.0-5.0 ATR (Media)** (219 oportunidades)

- **WR Teórico:** 71.7% (si se hubieran ejecutado)
- **TP_FIRST:** 65.8% (144 de 219)
- **SL_FIRST:** 34.2% (75 de 219)
- **MFE Promedio:** 86.01 pts
- **MAE Promedio:** 43.30 pts
- **MFE/MAE Ratio:** 4.41
- **Good Entries:** 69.4% (MFE > MAE)
- **R:R Promedio:** 2.41

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (111 oportunidades)

- **WR Teórico:** 71.2% (si se hubieran ejecutado)
- **TP_FIRST:** 61.3% (68 de 111)
- **SL_FIRST:** 38.7% (43 de 111)
- **MFE Promedio:** 115.34 pts
- **MAE Promedio:** 64.69 pts
- **MFE/MAE Ratio:** 1.42
- **Good Entries:** 66.7% (MFE > MAE)
- **R:R Promedio:** 3.51

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (11 oportunidades)

- **WR Teórico:** 0.0% (si se hubieran ejecutado)
- **TP_FIRST:** 0.0% (0 de 11)
- **SL_FIRST:** 100.0% (11 de 11)
- **MFE Promedio:** 0.00 pts
- **MAE Promedio:** 86.34 pts
- **MFE/MAE Ratio:** 0.00
- **Good Entries:** 0.0% (MFE > MAE)
- **R:R Promedio:** 6.11

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 25 | 282 | 136 | 219 |
| **TP_FIRST %** | 68.0% | 52.5% | 60.3% | 65.8% |
| **Good Entries %** | 52.0% | 52.1% | 56.6% | 69.4% |
| **MFE/MAE Ratio** | 167.57 | 5.09 | 3.10 | 4.41 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 219 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 71.7%
   - Good Entries: 69.4%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 12.70 pts
- **Mediana:** 12.79 pts
- **Min/Max:** 3.09 / 26.24 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 12.79 |
| P70 | 17.05 |
| P80 | 19.92 |
| P90 | 23.64 |
| P95 | 26.24 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 25.48 pts
- **Mediana:** 22.25 pts
- **Min/Max:** 8.75 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 22.25 |
| P70 | 36.35 |
| P80 | 38.55 |
| P90 | 48.90 |
| P95 | 53.20 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 23; // Era 60
public int MaxTPDistancePoints { get; set; } = 48; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 23.6pts, TP: 48.9pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (44.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.440) / 0.440
R:R_min = 1.27
```

**Estado actual:** R:R promedio = 2.20
**Gap:** -0.93 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **23** (P90 real)
2. **MaxTPDistancePoints:** 120 → **48** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.20) < R:R mínimo (1.27)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=44.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-18 11:52:01*