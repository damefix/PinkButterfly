# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-13 10:33:53
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_102942.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251113_102942.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 43
- **Win Rate:** 27.9% (12/43)
- **Profit Factor:** 0.56
- **Avg R:R Planeado:** 1.98
- **R:R Mínimo para Break-Even:** 2.58

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 31 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.98
   - R:R necesario: 2.58
   - **Gap:** 0.61

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8036 | 34.4% |
| Bearish | 6272 | 26.9% |
| Bullish | 9022 | 38.7% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.081
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
- **Consecuencia:** Sistema queda 34.4% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.4%)

**Posibles causas:**
- **BOS Score bajo (0.011):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio 0.081 indica poca señal direccional
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
| StructureFusion | 10290 | 100.0% | 100.0% |
| ProximityAnalyzer | 4112 | 40.0% | 40.0% |
| DFM_Evaluated | 813 | 19.8% | 7.9% |
| DFM_Passed | 813 | 100.0% | 7.9% |
| RiskCalculator | 6299 | 774.8% | 61.2% |
| Risk_Accepted | 2 | 0.0% | 0.0% |
| TradeManager | 43 | 2150.0% | 0.4% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 6297 señales, -100.0%)
- **Tasa de conversión final:** 0.42% (de 10290 zonas iniciales → 43 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,646 | 58.7% |
| NO_SL | 540 | 19.3% |
| ENTRY_TOO_FAR | 373 | 13.3% |
| TP_CHECK_FAIL | 243 | 8.7% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,646 rechazos, 58.7%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2498 | 89.0% |
| P0_ANY_DIR | 310 | 11.0% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 43.69 pts (máxima ganancia flotante)
- **MAE Promedio:** 27.86 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 71.61

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 25 | 58.1% |
| SL_FIRST (precio fue hacia SL) | 18 | 41.9% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 48.8%
- **Entradas Malas (MAE > MFE):** 51.2%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 43 | 25 | 18 | 58.1% | 43.69 | 27.86 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0002 | SELL | 12.25 | 16.50 | 0.74 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0003 | SELL | 22.00 | 21.75 | 1.01 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0005 | BUY | 21.25 | 26.75 | 0.79 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0010 | SELL | 13.00 | 18.75 | 0.69 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0011 | SELL | 0.00 | 38.50 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0014 | BUY | 1.75 | 11.00 | 0.16 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0016 | BUY | 14.50 | 26.50 | 0.55 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0016_2 | BUY | 14.50 | 26.50 | 0.55 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0017 | SELL | 1.50 | 13.00 | 0.12 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0018 | SELL | 137.50 | 27.75 | 4.95 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0020 | SELL | 244.50 | 21.75 | 11.24 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0020_2 | SELL | 244.50 | 21.75 | 11.24 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0024 | SELL | 0.00 | 68.50 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0031 | BUY | 63.00 | 35.75 | 1.76 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0032 | BUY | 57.75 | 31.75 | 1.82 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0033 | BUY | 12.00 | 106.50 | 0.11 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0036 | SELL | 55.25 | 58.00 | 0.95 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0037 | SELL | 61.50 | 64.50 | 0.95 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0038 | SELL | 17.00 | 63.25 | 0.27 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0046 | BUY | 15.25 | 8.25 | 1.85 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,284

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 456 | 34.2% | 59.2% | 3.80 | 47.1% | 2.05 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 242 | 49.6% | 46.7% | 1.77 | 47.9% | 1.95 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 365 | 67.9% | 53.7% | 2.19 | 61.9% | 1.99 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 203 | 65.0% | 57.6% | 0.67 | 61.6% | 2.27 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 18 | 38.9% | 16.7% | 0.34 | 27.8% | 3.53 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (456 oportunidades)

- **WR Teórico:** 34.2% (si se hubieran ejecutado)
- **TP_FIRST:** 59.2% (270 de 456)
- **SL_FIRST:** 38.2% (174 de 456)
- **MFE Promedio:** 40.29 pts
- **MAE Promedio:** 37.40 pts
- **MFE/MAE Ratio:** 3.80
- **Good Entries:** 47.1% (MFE > MAE)
- **R:R Promedio:** 2.05

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (242 oportunidades)

- **WR Teórico:** 49.6% (si se hubieran ejecutado)
- **TP_FIRST:** 46.7% (113 de 242)
- **SL_FIRST:** 53.3% (129 de 242)
- **MFE Promedio:** 57.45 pts
- **MAE Promedio:** 42.80 pts
- **MFE/MAE Ratio:** 1.77
- **Good Entries:** 47.9% (MFE > MAE)
- **R:R Promedio:** 1.95

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (365 oportunidades)

- **WR Teórico:** 67.9% (si se hubieran ejecutado)
- **TP_FIRST:** 53.7% (196 de 365)
- **SL_FIRST:** 45.8% (167 de 365)
- **MFE Promedio:** 69.40 pts
- **MAE Promedio:** 42.65 pts
- **MFE/MAE Ratio:** 2.19
- **Good Entries:** 61.9% (MFE > MAE)
- **R:R Promedio:** 1.99

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (203 oportunidades)

- **WR Teórico:** 65.0% (si se hubieran ejecutado)
- **TP_FIRST:** 57.6% (117 de 203)
- **SL_FIRST:** 41.4% (84 de 203)
- **MFE Promedio:** 82.43 pts
- **MAE Promedio:** 45.07 pts
- **MFE/MAE Ratio:** 0.67
- **Good Entries:** 61.6% (MFE > MAE)
- **R:R Promedio:** 2.27

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (18 oportunidades)

- **WR Teórico:** 38.9% (si se hubieran ejecutado)
- **TP_FIRST:** 16.7% (3 de 18)
- **SL_FIRST:** 83.3% (15 de 18)
- **MFE Promedio:** 96.14 pts
- **MAE Promedio:** 65.37 pts
- **MFE/MAE Ratio:** 0.34
- **Good Entries:** 27.8% (MFE > MAE)
- **R:R Promedio:** 3.53

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 43 | 456 | 242 | 365 |
| **TP_FIRST %** | 58.1% | 59.2% | 46.7% | 53.7% |
| **Good Entries %** | 48.8% | 47.1% | 47.9% | 61.9% |
| **MFE/MAE Ratio** | 71.61 | 3.80 | 1.77 | 2.19 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 365 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 67.9%
   - Good Entries: 61.9%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 12.30 pts
- **Mediana:** 10.04 pts
- **Min/Max:** 0.55 / 30.64 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 10.04 |
| P70 | 18.35 |
| P80 | 20.09 |
| P90 | 23.04 |
| P95 | 25.37 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 21.46 pts
- **Mediana:** 18.50 pts
- **Min/Max:** 3.25 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 18.50 |
| P70 | 28.25 |
| P80 | 33.50 |
| P90 | 44.00 |
| P95 | 48.95 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 23; // Era 60
public int MaxTPDistancePoints { get; set; } = 44; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 23.0pts, TP: 44.0pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (27.9%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.279) / 0.279
R:R_min = 2.58
```

**Estado actual:** R:R promedio = 1.98
**Gap:** 0.61 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **44** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.98) < R:R mínimo (2.58)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=27.9%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-13 10:33:53*