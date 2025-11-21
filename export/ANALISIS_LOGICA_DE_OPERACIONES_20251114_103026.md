# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-14 10:34:56
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_103026.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251114_103026.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 42
- **Win Rate:** 31.0% (13/42)
- **Profit Factor:** 0.61
- **Avg R:R Planeado:** 2.04
- **R:R Mínimo para Break-Even:** 2.23

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 38 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.04
   - R:R necesario: 2.23
   - **Gap:** 0.19

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 7925 | 34.0% |
| Bearish | 6354 | 27.2% |
| Bullish | 9051 | 38.8% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.079
- **Score Min/Max:** [-0.990, 0.980]
- **Componentes (promedio):**
  - EMA20 Slope: 0.040
  - EMA50 Cross: 0.190
  - BOS Count: 0.007
  - Regression 24h: 0.088

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.980 (apenas supera threshold)
- Score mínimo observado: -0.990 (apenas supera threshold)
- **Consecuencia:** Sistema queda 34.0% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.0%)

**Posibles causas:**
- **BOS Score bajo (0.007):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio 0.079 indica poca señal direccional
- **Mercado lateral:** Scores reales [-0.99, 0.98] muy cercanos a 0

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
| StructureFusion | 10420 | 100.0% | 100.0% |
| ProximityAnalyzer | 3658 | 35.1% | 35.1% |
| DFM_Evaluated | 842 | 23.0% | 8.1% |
| DFM_Passed | 842 | 100.0% | 8.1% |
| RiskCalculator | 5969 | 708.9% | 57.3% |
| Risk_Accepted | 2 | 0.0% | 0.0% |
| TradeManager | 42 | 2100.0% | 0.4% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 5967 señales, -100.0%)
- **Tasa de conversión final:** 0.40% (de 10420 zonas iniciales → 42 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,292 | 52.0% |
| NO_SL | 594 | 23.9% |
| ENTRY_TOO_FAR | 371 | 14.9% |
| TP_CHECK_FAIL | 230 | 9.2% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,292 rechazos, 52.0%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2540 | 89.3% |
| P0_ANY_DIR | 304 | 10.7% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 36.51 pts (máxima ganancia flotante)
- **MAE Promedio:** 41.46 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 73.70

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 19 | 45.2% |
| SL_FIRST (precio fue hacia SL) | 21 | 50.0% |
| NEUTRAL (sin dirección clara) | 2 | 4.8% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 35.7%
- **Entradas Malas (MAE > MFE):** 64.3%

⚠️ **ALERTA:** >60% de entradas tienen MAE > MFE
- **Problema:** El precio va más en contra que a favor antes del cierre
- **Causas posibles:**
  1. Timing incorrecto (entramos antes de reversión)
  2. Bias desincronizado (operamos contra tendencia real)
  3. Zonas de baja calidad (sin confluence real)

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 42 | 19 | 21 | 45.2% | 36.51 | 41.46 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0002 | SELL | 2.75 | 32.00 | 0.09 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0006 | BUY | 1.75 | 34.25 | 0.05 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0007 | SELL | 42.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0009 | SELL | 2.25 | 13.00 | 0.17 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0011 | SELL | 174.75 | 27.75 | 6.30 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0011_2 | SELL | 174.75 | 27.75 | 6.30 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0014 | SELL | 72.50 | 18.00 | 4.03 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0016 | SELL | 1.25 | 68.50 | 0.02 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0016_2 | SELL | 1.25 | 68.50 | 0.02 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0018 | SELL | 14.25 | 83.75 | 0.17 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0019 | BUY | 48.50 | 63.75 | 0.76 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0022 | SELL | 53.25 | 33.00 | 1.61 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0022_2 | SELL | 53.25 | 33.00 | 1.61 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0024 | SELL | 55.25 | 58.00 | 0.95 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0026 | SELL | 26.75 | 88.50 | 0.30 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0026_2 | SELL | 26.75 | 88.50 | 0.30 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0028 | SELL | 0.25 | 77.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0030 | SELL | 0.00 | 103.75 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0034 | SELL | 12.75 | 25.25 | 0.50 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0036 | BUY | 9.25 | 28.50 | 0.32 | TP_FIRST | CLOSED | ❌ Entrada muy mala |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,335

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 502 | 34.5% | 57.4% | 3.64 | 45.2% | 2.12 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 263 | 55.1% | 56.7% | 4.08 | 54.8% | 1.92 | ⚠️ CALIDAD MEDIA - Revisar |
| 3.0-5.0 ATR (Media) | 338 | 72.2% | 62.4% | 4.35 | 66.9% | 2.10 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 220 | 75.5% | 72.7% | 1.42 | 74.5% | 2.31 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 12 | 50.0% | 50.0% | 0.00 | 50.0% | 3.01 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (502 oportunidades)

- **WR Teórico:** 34.5% (si se hubieran ejecutado)
- **TP_FIRST:** 57.4% (288 de 502)
- **SL_FIRST:** 37.8% (190 de 502)
- **MFE Promedio:** 45.80 pts
- **MAE Promedio:** 41.23 pts
- **MFE/MAE Ratio:** 3.64
- **Good Entries:** 45.2% (MFE > MAE)
- **R:R Promedio:** 2.12

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (263 oportunidades)

- **WR Teórico:** 55.1% (si se hubieran ejecutado)
- **TP_FIRST:** 56.7% (149 de 263)
- **SL_FIRST:** 42.6% (112 de 263)
- **MFE Promedio:** 71.67 pts
- **MAE Promedio:** 44.04 pts
- **MFE/MAE Ratio:** 4.08
- **Good Entries:** 54.8% (MFE > MAE)
- **R:R Promedio:** 1.92

**⚠️ CALIDAD MEDIA - Revisar**

**3.0-5.0 ATR (Media)** (338 oportunidades)

- **WR Teórico:** 72.2% (si se hubieran ejecutado)
- **TP_FIRST:** 62.4% (211 de 338)
- **SL_FIRST:** 34.9% (118 de 338)
- **MFE Promedio:** 85.57 pts
- **MAE Promedio:** 45.89 pts
- **MFE/MAE Ratio:** 4.35
- **Good Entries:** 66.9% (MFE > MAE)
- **R:R Promedio:** 2.10

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (220 oportunidades)

- **WR Teórico:** 75.5% (si se hubieran ejecutado)
- **TP_FIRST:** 72.7% (160 de 220)
- **SL_FIRST:** 25.5% (56 de 220)
- **MFE Promedio:** 89.10 pts
- **MAE Promedio:** 54.32 pts
- **MFE/MAE Ratio:** 1.42
- **Good Entries:** 74.5% (MFE > MAE)
- **R:R Promedio:** 2.31

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (12 oportunidades)

- **WR Teórico:** 50.0% (si se hubieran ejecutado)
- **TP_FIRST:** 50.0% (6 de 12)
- **SL_FIRST:** 50.0% (6 de 12)
- **MFE Promedio:** 105.67 pts
- **MAE Promedio:** 69.04 pts
- **MFE/MAE Ratio:** 0.00
- **Good Entries:** 50.0% (MFE > MAE)
- **R:R Promedio:** 3.01

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 42 | 502 | 263 | 338 |
| **TP_FIRST %** | 45.2% | 57.4% | 56.7% | 62.4% |
| **Good Entries %** | 35.7% | 45.2% | 54.8% | 66.9% |
| **MFE/MAE Ratio** | 73.70 | 3.64 | 4.08 | 4.35 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 338 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 72.2%
   - Good Entries: 66.9%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 14.72 pts
- **Mediana:** 13.07 pts
- **Min/Max:** 3.10 / 37.51 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 13.07 |
| P70 | 19.11 |
| P80 | 22.87 |
| P90 | 27.27 |
| P95 | 37.17 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 27.87 pts
- **Mediana:** 27.12 pts
- **Min/Max:** 4.50 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 27.12 |
| P70 | 37.50 |
| P80 | 44.10 |
| P90 | 49.70 |
| P95 | 52.56 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 27; // Era 60
public int MaxTPDistancePoints { get; set; } = 49; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 27.3pts, TP: 49.7pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (31.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.310) / 0.310
R:R_min = 2.23
```

**Estado actual:** R:R promedio = 2.04
**Gap:** 0.19 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **27** (P90 real)
2. **MaxTPDistancePoints:** 120 → **49** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.04) < R:R mínimo (2.23)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=31.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-14 10:34:56*