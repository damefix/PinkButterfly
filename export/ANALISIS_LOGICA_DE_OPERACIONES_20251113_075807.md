# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-13 08:01:36
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_075807.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251113_075807.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 43
- **Win Rate:** 30.2% (13/43)
- **Profit Factor:** 0.69
- **Avg R:R Planeado:** 2.19
- **R:R Mínimo para Break-Even:** 2.31

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 40 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.19
   - R:R necesario: 2.31
   - **Gap:** 0.12

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8034 | 34.5% |
| Bearish | 6273 | 26.9% |
| Bullish | 9013 | 38.6% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.080
- **Score Min/Max:** [-0.990, 0.970]
- **Componentes (promedio):**
  - EMA20 Slope: 0.043
  - EMA50 Cross: 0.188
  - BOS Count: 0.011
  - Regression 24h: 0.090

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.970 (apenas supera threshold)
- Score mínimo observado: -0.990 (apenas supera threshold)
- **Consecuencia:** Sistema queda 34.5% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.5%)

**Posibles causas:**
- **BOS Score bajo (0.011):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio 0.080 indica poca señal direccional
- **Mercado lateral:** Scores reales [-0.99, 0.97] muy cercanos a 0

**Recomendaciones:**
1. ✅ Verificar que `BOSDetector.cs` establece `Type = breakType` (bug conocido)
2. ✅ Revisar logs para confirmar que BOS Score != 0.0
3. ⚠️ Si BOS sigue en ~0, investigar detección de BOS/CHoCH
4. ⚠️ Considerar bajar threshold a 0.2 SOLO si los 3 pasos anteriores están OK

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 10296 | 100.0% | 100.0% |
| ProximityAnalyzer | 4139 | 40.2% | 40.2% |
| DFM_Evaluated | 838 | 20.2% | 8.1% |
| DFM_Passed | 838 | 100.0% | 8.1% |
| RiskCalculator | 6307 | 752.6% | 61.3% |
| Risk_Accepted | 112 | 1.8% | 1.1% |
| TradeManager | 43 | 38.4% | 0.4% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 6195 señales, -98.2%)
- **Tasa de conversión final:** 0.42% (de 10296 zonas iniciales → 43 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,647 | 59.0% |
| NO_SL | 516 | 18.5% |
| ENTRY_TOO_FAR | 375 | 13.4% |
| TP_CHECK_FAIL | 252 | 9.0% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,647 rechazos, 59.0%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2567 | 89.1% |
| P0_ANY_DIR | 313 | 10.9% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 44.89 pts (máxima ganancia flotante)
- **MAE Promedio:** 28.76 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 28.83

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 23 | 53.5% |
| SL_FIRST (precio fue hacia SL) | 19 | 44.2% |
| NEUTRAL (sin dirección clara) | 1 | 2.3% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 46.5%
- **Entradas Malas (MAE > MFE):** 53.5%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 43 | 23 | 19 | 53.5% | 44.89 | 28.76 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0002 | BUY | 22.25 | 6.50 | 3.42 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0003 | SELL | 7.00 | 21.75 | 0.32 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0005 | BUY | 21.25 | 26.75 | 0.79 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0009 | SELL | 13.00 | 18.75 | 0.69 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0014 | BUY | 1.75 | 11.00 | 0.16 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0016 | BUY | 14.50 | 26.50 | 0.55 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0016_2 | BUY | 14.50 | 26.50 | 0.55 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0017 | SELL | 2.25 | 13.00 | 0.17 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0020 | SELL | 160.25 | 27.75 | 5.77 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0020_2 | SELL | 160.25 | 27.75 | 5.77 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0021 | SELL | 248.50 | 11.25 | 22.09 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0021_2 | SELL | 248.50 | 11.25 | 22.09 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0026 | SELL | 0.00 | 68.50 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0026_2 | SELL | 0.00 | 68.50 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0027 | SELL | 33.75 | 18.00 | 1.88 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0031 | BUY | 51.50 | 63.75 | 0.81 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0035 | SELL | 55.25 | 58.00 | 0.95 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0036 | SELL | 27.25 | 80.00 | 0.34 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0045 | BUY | 15.25 | 8.25 | 1.85 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0047 | SELL | 83.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,318

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 482 | 35.9% | 56.0% | 3.93 | 44.4% | 2.02 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 257 | 48.6% | 50.2% | 1.71 | 46.3% | 1.93 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 369 | 66.7% | 55.3% | 2.75 | 59.9% | 2.09 | ⚠️ CALIDAD MEDIA - Revisar |
| 5.0-10.0 ATR (Lejos) | 198 | 71.2% | 65.2% | 0.84 | 68.2% | 2.29 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 12 | 50.0% | 33.3% | 0.15 | 33.3% | 3.25 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (482 oportunidades)

- **WR Teórico:** 35.9% (si se hubieran ejecutado)
- **TP_FIRST:** 56.0% (270 de 482)
- **SL_FIRST:** 41.1% (198 de 482)
- **MFE Promedio:** 46.96 pts
- **MAE Promedio:** 38.35 pts
- **MFE/MAE Ratio:** 3.93
- **Good Entries:** 44.4% (MFE > MAE)
- **R:R Promedio:** 2.02

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (257 oportunidades)

- **WR Teórico:** 48.6% (si se hubieran ejecutado)
- **TP_FIRST:** 50.2% (129 de 257)
- **SL_FIRST:** 49.8% (128 de 257)
- **MFE Promedio:** 53.35 pts
- **MAE Promedio:** 42.24 pts
- **MFE/MAE Ratio:** 1.71
- **Good Entries:** 46.3% (MFE > MAE)
- **R:R Promedio:** 1.93

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (369 oportunidades)

- **WR Teórico:** 66.7% (si se hubieran ejecutado)
- **TP_FIRST:** 55.3% (204 de 369)
- **SL_FIRST:** 44.4% (164 de 369)
- **MFE Promedio:** 67.19 pts
- **MAE Promedio:** 41.90 pts
- **MFE/MAE Ratio:** 2.75
- **Good Entries:** 59.9% (MFE > MAE)
- **R:R Promedio:** 2.09

**⚠️ CALIDAD MEDIA - Revisar**

**5.0-10.0 ATR (Lejos)** (198 oportunidades)

- **WR Teórico:** 71.2% (si se hubieran ejecutado)
- **TP_FIRST:** 65.2% (129 de 198)
- **SL_FIRST:** 34.8% (69 de 198)
- **MFE Promedio:** 77.48 pts
- **MAE Promedio:** 47.66 pts
- **MFE/MAE Ratio:** 0.84
- **Good Entries:** 68.2% (MFE > MAE)
- **R:R Promedio:** 2.29

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (12 oportunidades)

- **WR Teórico:** 50.0% (si se hubieran ejecutado)
- **TP_FIRST:** 33.3% (4 de 12)
- **SL_FIRST:** 66.7% (8 de 12)
- **MFE Promedio:** 89.12 pts
- **MAE Promedio:** 75.62 pts
- **MFE/MAE Ratio:** 0.15
- **Good Entries:** 33.3% (MFE > MAE)
- **R:R Promedio:** 3.25

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 43 | 482 | 257 | 369 |
| **TP_FIRST %** | 53.5% | 56.0% | 50.2% | 55.3% |
| **Good Entries %** | 46.5% | 44.4% | 46.3% | 59.9% |
| **MFE/MAE Ratio** | 28.83 | 3.93 | 1.71 | 2.75 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 369 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 66.7%
   - Good Entries: 59.9%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 11.30 pts
- **Mediana:** 8.06 pts
- **Min/Max:** 0.55 / 40.44 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 8.06 |
| P70 | 13.04 |
| P80 | 18.28 |
| P90 | 23.03 |
| P95 | 35.64 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 22.30 pts
- **Mediana:** 15.75 pts
- **Min/Max:** 3.25 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 15.75 |
| P70 | 31.70 |
| P80 | 40.50 |
| P90 | 49.60 |
| P95 | 52.80 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 23; // Era 60
public int MaxTPDistancePoints { get; set; } = 49; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 23.0pts, TP: 49.6pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (30.2%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.302) / 0.302
R:R_min = 2.31
```

**Estado actual:** R:R promedio = 2.19
**Gap:** 0.12 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **49** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.19) < R:R mínimo (2.31)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=30.2%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-13 08:01:36*