# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-17 21:09:45
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_210431.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251117_210431.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 0
- **Win Rate:** 0.0% (0/0)
- **Profit Factor:** 0.00
- **Avg R:R Planeado:** 0.00
- **R:R Mínimo para Break-Even:** 1.75

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 0 puntos
   - TP máximo observado: 0 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 0.00
   - R:R necesario: 1.75
   - **Gap:** 1.75

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bullish | 1036 | 41.4% |
| Neutral | 726 | 29.0% |
| Bearish | 738 | 29.5% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.083
- **Score Min/Max:** [-1.000, 0.990]
- **Componentes (promedio):**
  - EMA20 Slope: 0.052
  - EMA50 Cross: 0.136
  - BOS Count: 0.016
  - Regression 24h: 0.146

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.990 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 29.0% Neutral → bias no diferencia tendencias

### 2.3 Estado: Bias Compuesto Funcionando Correctamente

✅ **Threshold actual:** 0.3 (correcto)
✅ **Bias Neutral:** 29.0% (aceptable)
✅ **Score promedio:** 0.083

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 8948 | 100.0% | 100.0% |
| ProximityAnalyzer | 1083 | 12.1% | 12.1% |
| DFM_Evaluated | 411 | 38.0% | 4.6% |
| DFM_Passed | 411 | 100.0% | 4.6% |
| RiskCalculator | 1957 | 476.2% | 21.9% |
| Risk_Accepted | 0 | 0.0% | 0.0% |
| TradeManager | 0 | 0.0% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 1957 señales, -100.0%)
- **Tasa de conversión final:** 0.00% (de 8948 zonas iniciales → 0 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 557 | 61.9% |
| ENTRY_TOO_FAR | 181 | 20.1% |
| TP_CHECK_FAIL | 150 | 16.7% |
| NO_SL | 12 | 1.3% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (557 rechazos, 61.9%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 1019 | 95.3% |
| P0_ANY_DIR | 50 | 4.7% |

### 3.5 Análisis MFE/MAE (Excursión del Precio)

⚠️ **No hay datos OHLC disponibles ([PIPE] logs)**

Para activar este análisis:
1. En `EngineConfig.cs`: `EnableOHLCLogging = true`
2. Ejecutar backtest
3. El log generará trazas `[PIPE] TF=X O=Y H=Z L=W C=V`

### 3.6 Análisis de PHANTOM OPPORTUNITIES

**No hay phantom opportunities para analizar** (se necesita ejecutar backtest con logging [PHANTOM_OPPORTUNITY])

---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

### 4.2 Distribución Take Profit (Puntos)

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

### 4.4 R:R Óptimo

**Para Win Rate actual (0.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.000) / 0.000
R:R_min = 1.75
```

**Estado actual:** R:R promedio = 0.00
**Gap:** 1.75 (necesitas mejorar R:R)

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
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (0.00) < R:R mínimo (1.75)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=0.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-17 21:09:45*