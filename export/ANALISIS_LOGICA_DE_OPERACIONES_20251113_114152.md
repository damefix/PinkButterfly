# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-13 11:47:01
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_114152.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251113_114152.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 34
- **Win Rate:** 32.4% (11/34)
- **Profit Factor:** 1.04
- **Avg R:R Planeado:** 2.62
- **R:R Mínimo para Break-Even:** 2.09

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 40 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.62
   - R:R necesario: 2.09
   - **Gap:** -0.53

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8031 | 34.4% |
| Bearish | 6272 | 26.9% |
| Bullish | 9032 | 38.7% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.081
- **Score Min/Max:** [-0.990, 0.970]
- **Componentes (promedio):**
  - EMA20 Slope: 0.043
  - EMA50 Cross: 0.188
  - BOS Count: 0.012
  - Regression 24h: 0.090

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.970 (apenas supera threshold)
- Score mínimo observado: -0.990 (apenas supera threshold)
- **Consecuencia:** Sistema queda 34.4% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.4%)

**Posibles causas:**
- **BOS Score bajo (0.012):** BOS/CHoCH no se detectan correctamente
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
| StructureFusion | 10337 | 100.0% | 100.0% |
| ProximityAnalyzer | 4164 | 40.3% | 40.3% |
| DFM_Evaluated | 838 | 20.1% | 8.1% |
| DFM_Passed | 838 | 100.0% | 8.1% |
| RiskCalculator | 6321 | 754.3% | 61.1% |
| Risk_Accepted | 1 | 0.0% | 0.0% |
| TradeManager | 34 | 3400.0% | 0.3% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 6320 señales, -100.0%)
- **Tasa de conversión final:** 0.33% (de 10337 zonas iniciales → 34 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,655 | 58.7% |
| NO_SL | 520 | 18.4% |
| ENTRY_TOO_FAR | 384 | 13.6% |
| TP_CHECK_FAIL | 260 | 9.2% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,655 rechazos, 58.7%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2555 | 89.3% |
| P0_ANY_DIR | 307 | 10.7% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 45.87 pts (máxima ganancia flotante)
- **MAE Promedio:** 30.18 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 62.01

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 23 | 67.6% |
| SL_FIRST (precio fue hacia SL) | 10 | 29.4% |
| NEUTRAL (sin dirección clara) | 1 | 2.9% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 47.1%
- **Entradas Malas (MAE > MFE):** 52.9%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 34 | 23 | 10 | 67.6% | 45.87 | 30.18 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0003 | BUY | 15.00 | 26.75 | 0.56 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0006 | SELL | 13.00 | 18.75 | 0.69 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0010 | BUY | 1.75 | 11.00 | 0.16 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0012 | BUY | 14.50 | 26.50 | 0.55 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0013 | SELL | 42.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0014 | SELL | 2.25 | 13.00 | 0.17 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0016 | SELL | 166.00 | 14.00 | 11.86 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0016_2 | SELL | 166.00 | 14.00 | 11.86 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0017 | SELL | 248.50 | 11.25 | 22.09 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0021 | SELL | 0.00 | 68.50 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0022 | SELL | 33.75 | 18.00 | 1.88 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0026 | BUY | 51.50 | 63.75 | 0.81 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0027 | BUY | 9.50 | 66.75 | 0.14 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0032 | SELL | 55.25 | 58.00 | 0.95 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0033 | SELL | 36.00 | 75.50 | 0.48 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0044 | BUY | 27.00 | 4.00 | 6.75 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0046 | SELL | 83.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0047 | SELL | 26.75 | 40.75 | 0.66 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0051 | BUY | 11.25 | 10.00 | 1.12 | SL_FIRST | CLOSED | 👍 Entrada correcta |
| T0058 | BUY | 16.50 | 2.25 | 7.33 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,308

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 483 | 38.1% | 59.4% | 4.01 | 45.8% | 2.05 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 252 | 51.2% | 51.6% | 1.70 | 49.6% | 1.97 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 359 | 68.8% | 56.8% | 2.96 | 61.0% | 2.05 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 198 | 70.7% | 66.7% | 0.76 | 68.7% | 2.30 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 16 | 50.0% | 37.5% | 0.12 | 37.5% | 3.28 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (483 oportunidades)

- **WR Teórico:** 38.1% (si se hubieran ejecutado)
- **TP_FIRST:** 59.4% (287 de 483)
- **SL_FIRST:** 38.1% (184 de 483)
- **MFE Promedio:** 49.16 pts
- **MAE Promedio:** 38.94 pts
- **MFE/MAE Ratio:** 4.01
- **Good Entries:** 45.8% (MFE > MAE)
- **R:R Promedio:** 2.05

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (252 oportunidades)

- **WR Teórico:** 51.2% (si se hubieran ejecutado)
- **TP_FIRST:** 51.6% (130 de 252)
- **SL_FIRST:** 48.4% (122 de 252)
- **MFE Promedio:** 56.59 pts
- **MAE Promedio:** 41.74 pts
- **MFE/MAE Ratio:** 1.70
- **Good Entries:** 49.6% (MFE > MAE)
- **R:R Promedio:** 1.97

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (359 oportunidades)

- **WR Teórico:** 68.8% (si se hubieran ejecutado)
- **TP_FIRST:** 56.8% (204 de 359)
- **SL_FIRST:** 43.2% (155 de 359)
- **MFE Promedio:** 70.44 pts
- **MAE Promedio:** 42.91 pts
- **MFE/MAE Ratio:** 2.96
- **Good Entries:** 61.0% (MFE > MAE)
- **R:R Promedio:** 2.05

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (198 oportunidades)

- **WR Teórico:** 70.7% (si se hubieran ejecutado)
- **TP_FIRST:** 66.7% (132 de 198)
- **SL_FIRST:** 33.3% (66 de 198)
- **MFE Promedio:** 81.16 pts
- **MAE Promedio:** 49.12 pts
- **MFE/MAE Ratio:** 0.76
- **Good Entries:** 68.7% (MFE > MAE)
- **R:R Promedio:** 2.30

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (16 oportunidades)

- **WR Teórico:** 50.0% (si se hubieran ejecutado)
- **TP_FIRST:** 37.5% (6 de 16)
- **SL_FIRST:** 62.5% (10 de 16)
- **MFE Promedio:** 81.44 pts
- **MAE Promedio:** 64.33 pts
- **MFE/MAE Ratio:** 0.12
- **Good Entries:** 37.5% (MFE > MAE)
- **R:R Promedio:** 3.28

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 34 | 483 | 252 | 359 |
| **TP_FIRST %** | 67.6% | 59.4% | 51.6% | 56.8% |
| **Good Entries %** | 47.1% | 45.8% | 49.6% | 61.0% |
| **MFE/MAE Ratio** | 62.01 | 4.01 | 1.70 | 2.96 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 359 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 68.8%
   - Good Entries: 61.0%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 12.36 pts
- **Mediana:** 8.52 pts
- **Min/Max:** 0.55 / 40.44 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 8.53 |
| P70 | 17.84 |
| P80 | 21.88 |
| P90 | 26.00 |
| P95 | 38.24 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 24.30 pts
- **Mediana:** 20.62 pts
- **Min/Max:** 3.25 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 20.62 |
| P70 | 36.12 |
| P80 | 40.00 |
| P90 | 51.12 |
| P95 | 53.31 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 25; // Era 60
public int MaxTPDistancePoints { get; set; } = 51; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 26.0pts, TP: 51.1pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (32.4%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.324) / 0.324
R:R_min = 2.09
```

**Estado actual:** R:R promedio = 2.62
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
1. **MaxSLDistancePoints:** 60 → **25** (P90 real)
2. **MaxTPDistancePoints:** 120 → **51** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.62) < R:R mínimo (2.09)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=32.4%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-13 11:47:01*