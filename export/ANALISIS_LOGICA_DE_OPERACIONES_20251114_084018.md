# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-14 08:42:35
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_084018.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251114_084018.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 35
- **Win Rate:** 34.3% (12/35)
- **Profit Factor:** 1.03
- **Avg R:R Planeado:** 2.42
- **R:R Mínimo para Break-Even:** 1.92

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 40 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.42
   - R:R necesario: 1.92
   - **Gap:** -0.50

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 7965 | 34.2% |
| Bearish | 6324 | 27.1% |
| Bullish | 9033 | 38.7% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.079
- **Score Min/Max:** [-0.990, 0.970]
- **Componentes (promedio):**
  - EMA20 Slope: 0.040
  - EMA50 Cross: 0.191
  - BOS Count: 0.008
  - Regression 24h: 0.088

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.970 (apenas supera threshold)
- Score mínimo observado: -0.990 (apenas supera threshold)
- **Consecuencia:** Sistema queda 34.2% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.2%)

**Posibles causas:**
- **BOS Score bajo (0.008):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio 0.079 indica poca señal direccional
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
| StructureFusion | 10404 | 100.0% | 100.0% |
| ProximityAnalyzer | 4214 | 40.5% | 40.5% |
| DFM_Evaluated | 832 | 19.7% | 8.0% |
| DFM_Passed | 832 | 100.0% | 8.0% |
| RiskCalculator | 6378 | 766.6% | 61.3% |
| Risk_Accepted | 2 | 0.0% | 0.0% |
| TradeManager | 35 | 1750.0% | 0.3% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 6376 señales, -100.0%)
- **Tasa de conversión final:** 0.34% (de 10404 zonas iniciales → 35 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,664 | 57.9% |
| NO_SL | 531 | 18.5% |
| ENTRY_TOO_FAR | 420 | 14.6% |
| TP_CHECK_FAIL | 260 | 9.0% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,664 rechazos, 57.9%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2519 | 88.4% |
| P0_ANY_DIR | 331 | 11.6% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 45.13 pts (máxima ganancia flotante)
- **MAE Promedio:** 30.09 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 92.05

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 23 | 65.7% |
| SL_FIRST (precio fue hacia SL) | 11 | 31.4% |
| NEUTRAL (sin dirección clara) | 1 | 2.9% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 51.4%
- **Entradas Malas (MAE > MFE):** 48.6%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 35 | 23 | 11 | 65.7% | 45.13 | 30.09 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | SELL | 12.00 | 5.00 | 2.40 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0004 | SELL | 6.00 | 28.75 | 0.21 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0005 | SELL | 2.50 | 36.00 | 0.07 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0006 | SELL | 1.25 | 37.00 | 0.03 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0009 | BUY | 1.75 | 11.00 | 0.16 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0010 | BUY | 4.00 | 37.00 | 0.11 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0011 | SELL | 42.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0013 | SELL | 166.00 | 14.00 | 11.86 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0013_2 | SELL | 166.00 | 14.00 | 11.86 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0014 | SELL | 248.50 | 11.25 | 22.09 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0018 | SELL | 0.00 | 68.50 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0019 | SELL | 33.75 | 18.00 | 1.88 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0023 | BUY | 51.50 | 63.75 | 0.81 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0024 | BUY | 9.50 | 66.75 | 0.14 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0029 | SELL | 55.25 | 58.00 | 0.95 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0030 | SELL | 36.00 | 75.50 | 0.48 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0039 | BUY | 15.25 | 8.25 | 1.85 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0041 | SELL | 83.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0042 | SELL | 26.75 | 36.00 | 0.74 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0045 | BUY | 11.25 | 10.00 | 1.12 | SL_FIRST | CLOSED | 👍 Entrada correcta |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,301

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 477 | 39.2% | 60.2% | 4.34 | 45.9% | 2.06 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 237 | 53.6% | 54.9% | 2.27 | 49.8% | 1.90 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 366 | 69.7% | 59.3% | 2.14 | 61.5% | 2.02 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 208 | 76.4% | 71.2% | 1.20 | 74.5% | 2.42 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 13 | 53.8% | 38.5% | 0.15 | 38.5% | 3.44 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (477 oportunidades)

- **WR Teórico:** 39.2% (si se hubieran ejecutado)
- **TP_FIRST:** 60.2% (287 de 477)
- **SL_FIRST:** 36.3% (173 de 477)
- **MFE Promedio:** 50.76 pts
- **MAE Promedio:** 42.58 pts
- **MFE/MAE Ratio:** 4.34
- **Good Entries:** 45.9% (MFE > MAE)
- **R:R Promedio:** 2.06

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (237 oportunidades)

- **WR Teórico:** 53.6% (si se hubieran ejecutado)
- **TP_FIRST:** 54.9% (130 de 237)
- **SL_FIRST:** 44.7% (106 de 237)
- **MFE Promedio:** 61.49 pts
- **MAE Promedio:** 45.24 pts
- **MFE/MAE Ratio:** 2.27
- **Good Entries:** 49.8% (MFE > MAE)
- **R:R Promedio:** 1.90

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (366 oportunidades)

- **WR Teórico:** 69.7% (si se hubieran ejecutado)
- **TP_FIRST:** 59.3% (217 de 366)
- **SL_FIRST:** 38.8% (142 de 366)
- **MFE Promedio:** 77.82 pts
- **MAE Promedio:** 47.09 pts
- **MFE/MAE Ratio:** 2.14
- **Good Entries:** 61.5% (MFE > MAE)
- **R:R Promedio:** 2.02

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (208 oportunidades)

- **WR Teórico:** 76.4% (si se hubieran ejecutado)
- **TP_FIRST:** 71.2% (148 de 208)
- **SL_FIRST:** 28.8% (60 de 208)
- **MFE Promedio:** 91.14 pts
- **MAE Promedio:** 58.51 pts
- **MFE/MAE Ratio:** 1.20
- **Good Entries:** 74.5% (MFE > MAE)
- **R:R Promedio:** 2.42

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (13 oportunidades)

- **WR Teórico:** 53.8% (si se hubieran ejecutado)
- **TP_FIRST:** 38.5% (5 de 13)
- **SL_FIRST:** 61.5% (8 de 13)
- **MFE Promedio:** 88.21 pts
- **MAE Promedio:** 67.41 pts
- **MFE/MAE Ratio:** 0.15
- **Good Entries:** 38.5% (MFE > MAE)
- **R:R Promedio:** 3.44

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 35 | 477 | 237 | 366 |
| **TP_FIRST %** | 65.7% | 60.2% | 54.9% | 59.3% |
| **Good Entries %** | 51.4% | 45.9% | 49.8% | 61.5% |
| **MFE/MAE Ratio** | 92.05 | 4.34 | 2.27 | 2.14 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 366 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 69.7%
   - Good Entries: 61.5%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 12.16 pts
- **Mediana:** 8.06 pts
- **Min/Max:** 0.83 / 40.44 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 8.06 |
| P70 | 16.88 |
| P80 | 21.27 |
| P90 | 25.56 |
| P95 | 38.10 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 23.62 pts
- **Mediana:** 19.50 pts
- **Min/Max:** 5.00 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 19.50 |
| P70 | 33.35 |
| P80 | 39.80 |
| P90 | 50.90 |
| P95 | 53.30 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 25; // Era 60
public int MaxTPDistancePoints { get; set; } = 50; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 25.6pts, TP: 50.9pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (34.3%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.343) / 0.343
R:R_min = 1.92
```

**Estado actual:** R:R promedio = 2.42
**Gap:** -0.50 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **50** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.42) < R:R mínimo (1.92)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=34.3%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-14 08:42:35*