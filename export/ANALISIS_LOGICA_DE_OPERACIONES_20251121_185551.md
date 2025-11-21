# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-21 19:04:27
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251121_185551.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251121_185551.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 14
- **Win Rate:** 71.4% (10/14)
- **Profit Factor:** 5.33
- **Avg R:R Planeado:** 2.38
- **R:R Mínimo para Break-Even:** 0.40

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 27 puntos
   - TP máximo observado: 46 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.38
   - R:R necesario: 0.40
   - **Gap:** -1.98

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bullish | 1091 | 34.9% |
| Neutral | 815 | 26.1% |
| Bearish | 1219 | 39.0% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** -0.006
- **Score Min/Max:** [-1.000, 1.000]
- **Componentes (promedio):**
  - EMA20 Slope: -0.021
  - EMA50 Cross: -0.048
  - BOS Count: -0.011
  - Regression 24h: 0.073

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 1.000 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 26.1% Neutral → bias no diferencia tendencias

### 2.3 Estado: Bias Compuesto Funcionando Correctamente

✅ **Threshold actual:** 0.3 (correcto)
✅ **Bias Neutral:** 26.1% (aceptable)
✅ **Score promedio:** -0.006

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 49528 | 100.0% | 100.0% |
| ProximityAnalyzer | 4381 | 8.8% | 8.8% |
| DFM_Evaluated | 551 | 12.6% | 1.1% |
| DFM_Passed | 551 | 100.0% | 1.1% |
| RiskCalculator | 8379 | 1520.7% | 16.9% |
| Risk_Accepted | 1 | 0.0% | 0.0% |
| TradeManager | 14 | 1400.0% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 8378 señales, -100.0%)
- **Tasa de conversión final:** 0.03% (de 49528 zonas iniciales → 14 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 3,837 | 65.2% |
| ENTRY_TOO_FAR | 1,272 | 21.6% |
| TP_CHECK_FAIL | 581 | 9.9% |
| NO_SL | 198 | 3.4% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (3,837 rechazos, 65.2%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2793 | 73.2% |
| P0_ANY_DIR | 1021 | 26.8% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 42.93 pts (máxima ganancia flotante)
- **MAE Promedio:** 11.07 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 430.51

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 12 | 85.7% |
| SL_FIRST (precio fue hacia SL) | 2 | 14.3% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 78.6%
- **Entradas Malas (MAE > MFE):** 21.4%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 14 | 12 | 2 | 85.7% | 42.93 | 11.07 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0004 | SELL | 40.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0005 | SELL | 40.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0006 | SELL | 40.25 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0009 | SELL | 40.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0010 | SELL | 40.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0013 | SELL | 40.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0014 | SELL | 11.25 | 28.50 | 0.39 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0016 | SELL | 18.25 | 24.00 | 0.76 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0016_2 | SELL | 18.25 | 24.00 | 0.76 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0021 | SELL | 31.00 | 4.75 | 6.53 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0022 | SELL | 79.00 | 17.25 | 4.58 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0026 | SELL | 35.00 | 31.00 | 1.13 | SL_FIRST | CLOSED | 👍 Entrada correcta |
| T0028 | SELL | 95.75 | 20.75 | 4.61 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0030 | SELL | 68.50 | 4.75 | 14.42 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 730

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 315 | 65.7% | 94.9% | 2.97 | 87.0% | 2.24 | ✅ BUENA CALIDAD - Considerar incluir |
| 2.0-3.0 ATR (Cerca) | 128 | 68.0% | 99.2% | 1.84 | 87.5% | 2.07 | ✅ BUENA CALIDAD - Considerar incluir |
| 3.0-5.0 ATR (Media) | 159 | 66.0% | 93.1% | 2.20 | 82.4% | 1.99 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 115 | 66.1% | 91.3% | 1.04 | 75.7% | 1.90 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 13 | 61.5% | 100.0% | 1.03 | 61.5% | 1.72 | ✅ BUENA CALIDAD - Considerar incluir |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (315 oportunidades)

- **WR Teórico:** 65.7% (si se hubieran ejecutado)
- **TP_FIRST:** 94.9% (299 de 315)
- **SL_FIRST:** 4.4% (14 de 315)
- **MFE Promedio:** 51.06 pts
- **MAE Promedio:** 21.51 pts
- **MFE/MAE Ratio:** 2.97
- **Good Entries:** 87.0% (MFE > MAE)
- **R:R Promedio:** 2.24

**✅ BUENA CALIDAD - Considerar incluir**

**2.0-3.0 ATR (Cerca)** (128 oportunidades)

- **WR Teórico:** 68.0% (si se hubieran ejecutado)
- **TP_FIRST:** 99.2% (127 de 128)
- **SL_FIRST:** 0.8% (1 de 128)
- **MFE Promedio:** 65.81 pts
- **MAE Promedio:** 23.40 pts
- **MFE/MAE Ratio:** 1.84
- **Good Entries:** 87.5% (MFE > MAE)
- **R:R Promedio:** 2.07

**✅ BUENA CALIDAD - Considerar incluir**

**3.0-5.0 ATR (Media)** (159 oportunidades)

- **WR Teórico:** 66.0% (si se hubieran ejecutado)
- **TP_FIRST:** 93.1% (148 de 159)
- **SL_FIRST:** 6.9% (11 de 159)
- **MFE Promedio:** 84.39 pts
- **MAE Promedio:** 28.36 pts
- **MFE/MAE Ratio:** 2.20
- **Good Entries:** 82.4% (MFE > MAE)
- **R:R Promedio:** 1.99

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (115 oportunidades)

- **WR Teórico:** 66.1% (si se hubieran ejecutado)
- **TP_FIRST:** 91.3% (105 de 115)
- **SL_FIRST:** 8.7% (10 de 115)
- **MFE Promedio:** 73.55 pts
- **MAE Promedio:** 29.34 pts
- **MFE/MAE Ratio:** 1.04
- **Good Entries:** 75.7% (MFE > MAE)
- **R:R Promedio:** 1.90

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (13 oportunidades)

- **WR Teórico:** 61.5% (si se hubieran ejecutado)
- **TP_FIRST:** 100.0% (13 de 13)
- **SL_FIRST:** 0.0% (0 de 13)
- **MFE Promedio:** 28.35 pts
- **MAE Promedio:** 23.50 pts
- **MFE/MAE Ratio:** 1.03
- **Good Entries:** 61.5% (MFE > MAE)
- **R:R Promedio:** 1.72

**✅ BUENA CALIDAD - Considerar incluir**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 14 | 315 | 128 | 159 |
| **TP_FIRST %** | 85.7% | 94.9% | 99.2% | 93.1% |
| **Good Entries %** | 78.6% | 87.0% | 87.5% | 82.4% |
| **MFE/MAE Ratio** | 430.51 | 2.97 | 1.84 | 2.20 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

🚨 **CRÍTICO: El rango 2.0-3.0 ATR contiene 128 oportunidades de BUENA CALIDAD**
   - WR Teórico: 68.0%
   - Good Entries: 87.5%
   - **ACCIÓN:** Considerar aumentar MaxDistanceToRegister_ATR_Normal de 2.0 a 3.0

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 159 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 66.0%
   - Good Entries: 82.4%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 13.45 pts
- **Mediana:** 12.75 pts
- **Min/Max:** 3.60 / 26.91 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 12.76 |
| P70 | 15.39 |
| P80 | 16.89 |
| P90 | 23.40 |
| P95 | 28.66 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 31.21 pts
- **Mediana:** 35.50 pts
- **Min/Max:** 9.25 / 46.25 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 35.50 |
| P70 | 38.75 |
| P80 | 39.00 |
| P90 | 43.75 |
| P95 | 47.50 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 23; // Era 60
public int MaxTPDistancePoints { get; set; } = 43; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 23.4pts, TP: 43.8pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (71.4%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.714) / 0.714
R:R_min = 0.40
```

**Estado actual:** R:R promedio = 2.38
**Gap:** -1.98 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **43** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.38) < R:R mínimo (0.40)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=71.4%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-21 19:04:27*