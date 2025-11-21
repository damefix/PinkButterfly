# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-21 15:57:57
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251121_154612.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251121_154612.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 8
- **Win Rate:** 62.5% (5/8)
- **Profit Factor:** 4.35
- **Avg R:R Planeado:** 2.34
- **R:R Mínimo para Break-Even:** 0.60

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 22 puntos
   - TP máximo observado: 47 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.34
   - R:R necesario: 0.60
   - **Gap:** -1.74

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 818 | 26.1% |
| Bullish | 1107 | 35.4% |
| Bearish | 1204 | 38.5% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.005
- **Score Min/Max:** [-1.000, 1.000]
- **Componentes (promedio):**
  - EMA20 Slope: -0.018
  - EMA50 Cross: -0.039
  - BOS Count: 0.019
  - Regression 24h: 0.074

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 1.000 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 26.1% Neutral → bias no diferencia tendencias

### 2.3 Estado: Bias Compuesto Funcionando Correctamente

✅ **Threshold actual:** 0.3 (correcto)
✅ **Bias Neutral:** 26.1% (aceptable)
✅ **Score promedio:** 0.005

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 49365 | 100.0% | 100.0% |
| ProximityAnalyzer | 3946 | 8.0% | 8.0% |
| DFM_Evaluated | 393 | 10.0% | 0.8% |
| DFM_Passed | 393 | 100.0% | 0.8% |
| RiskCalculator | 8067 | 2052.7% | 16.3% |
| Risk_Accepted | 1 | 0.0% | 0.0% |
| TradeManager | 8 | 800.0% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 8066 señales, -100.0%)
- **Tasa de conversión final:** 0.02% (de 49365 zonas iniciales → 8 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 4,221 | 68.6% |
| ENTRY_TOO_FAR | 1,276 | 20.7% |
| TP_CHECK_FAIL | 422 | 6.9% |
| NO_SL | 231 | 3.8% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (4,221 rechazos, 68.6%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2419 | 77.0% |
| P0_ANY_DIR | 724 | 23.0% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 56.75 pts (máxima ganancia flotante)
- **MAE Promedio:** 17.84 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 254.59

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 7 | 87.5% |
| SL_FIRST (precio fue hacia SL) | 1 | 12.5% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 75.0%
- **Entradas Malas (MAE > MFE):** 25.0%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 8 | 7 | 1 | 87.5% | 56.75 | 17.84 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0003 | SELL | 48.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0005 | SELL | 45.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0009 | SELL | 16.75 | 21.50 | 0.78 | SL_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0014 | SELL | 37.00 | 15.25 | 2.43 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0017 | SELL | 79.00 | 5.25 | 15.05 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0018 | SELL | 44.00 | 81.00 | 0.54 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0020 | SELL | 96.75 | 12.75 | 7.59 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0022 | SELL | 86.25 | 7.00 | 12.32 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 611

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 222 | 49.5% | 66.7% | 4.16 | 70.3% | 2.19 | ✅ BUENA CALIDAD - Considerar incluir |
| 2.0-3.0 ATR (Cerca) | 113 | 57.5% | 66.4% | 3.41 | 71.7% | 2.30 | ✅ BUENA CALIDAD - Considerar incluir |
| 3.0-5.0 ATR (Media) | 140 | 57.9% | 67.1% | 3.10 | 68.6% | 2.17 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 108 | 57.4% | 60.2% | 3.16 | 60.2% | 2.12 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 28 | 42.9% | 42.9% | 0.78 | 42.9% | 2.27 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (222 oportunidades)

- **WR Teórico:** 49.5% (si se hubieran ejecutado)
- **TP_FIRST:** 66.7% (148 de 222)
- **SL_FIRST:** 24.8% (55 de 222)
- **MFE Promedio:** 53.84 pts
- **MAE Promedio:** 19.63 pts
- **MFE/MAE Ratio:** 4.16
- **Good Entries:** 70.3% (MFE > MAE)
- **R:R Promedio:** 2.19

**✅ BUENA CALIDAD - Considerar incluir**

**2.0-3.0 ATR (Cerca)** (113 oportunidades)

- **WR Teórico:** 57.5% (si se hubieran ejecutado)
- **TP_FIRST:** 66.4% (75 de 113)
- **SL_FIRST:** 31.0% (35 de 113)
- **MFE Promedio:** 48.61 pts
- **MAE Promedio:** 21.93 pts
- **MFE/MAE Ratio:** 3.41
- **Good Entries:** 71.7% (MFE > MAE)
- **R:R Promedio:** 2.30

**✅ BUENA CALIDAD - Considerar incluir**

**3.0-5.0 ATR (Media)** (140 oportunidades)

- **WR Teórico:** 57.9% (si se hubieran ejecutado)
- **TP_FIRST:** 67.1% (94 de 140)
- **SL_FIRST:** 29.3% (41 de 140)
- **MFE Promedio:** 55.87 pts
- **MAE Promedio:** 23.93 pts
- **MFE/MAE Ratio:** 3.10
- **Good Entries:** 68.6% (MFE > MAE)
- **R:R Promedio:** 2.17

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (108 oportunidades)

- **WR Teórico:** 57.4% (si se hubieran ejecutado)
- **TP_FIRST:** 60.2% (65 de 108)
- **SL_FIRST:** 39.8% (43 de 108)
- **MFE Promedio:** 50.12 pts
- **MAE Promedio:** 21.31 pts
- **MFE/MAE Ratio:** 3.16
- **Good Entries:** 60.2% (MFE > MAE)
- **R:R Promedio:** 2.12

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (28 oportunidades)

- **WR Teórico:** 42.9% (si se hubieran ejecutado)
- **TP_FIRST:** 42.9% (12 de 28)
- **SL_FIRST:** 57.1% (16 de 28)
- **MFE Promedio:** 30.35 pts
- **MAE Promedio:** 21.50 pts
- **MFE/MAE Ratio:** 0.78
- **Good Entries:** 42.9% (MFE > MAE)
- **R:R Promedio:** 2.27

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 8 | 222 | 113 | 140 |
| **TP_FIRST %** | 87.5% | 66.7% | 66.4% | 67.1% |
| **Good Entries %** | 75.0% | 70.3% | 71.7% | 68.6% |
| **MFE/MAE Ratio** | 254.59 | 4.16 | 3.41 | 3.10 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

🚨 **CRÍTICO: El rango 2.0-3.0 ATR contiene 113 oportunidades de BUENA CALIDAD**
   - WR Teórico: 57.5%
   - Good Entries: 71.7%
   - **ACCIÓN:** Considerar aumentar MaxDistanceToRegister_ATR_Normal de 2.0 a 3.0

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 140 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 57.9%
   - Good Entries: 68.6%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 14.70 pts
- **Mediana:** 14.34 pts
- **Min/Max:** 7.43 / 22.42 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 14.34 |
| P70 | 18.61 |
| P80 | 21.56 |
| P90 | 22.53 |
| P95 | 23.01 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 32.19 pts
- **Mediana:** 30.12 pts
- **Min/Max:** 18.75 / 47.25 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 30.12 |
| P70 | 40.02 |
| P80 | 45.25 |
| P90 | 47.50 |
| P95 | 48.62 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 22; // Era 60
public int MaxTPDistancePoints { get; set; } = 47; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 22.5pts, TP: 47.5pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (62.5%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.625) / 0.625
R:R_min = 0.60
```

**Estado actual:** R:R promedio = 2.34
**Gap:** -1.74 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **22** (P90 real)
2. **MaxTPDistancePoints:** 120 → **47** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.34) < R:R mínimo (0.60)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=62.5%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-21 15:57:57*