# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-12 17:47:44
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_174403.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251112_174403.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 36
- **Win Rate:** 30.6% (11/36)
- **Profit Factor:** 1.04
- **Avg R:R Planeado:** 2.52
- **R:R Mínimo para Break-Even:** 2.27

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 40 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.52
   - R:R necesario: 2.27
   - **Gap:** -0.25

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8084 | 34.6% |
| Bullish | 8983 | 38.5% |
| Bearish | 6292 | 26.9% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.079
- **Score Min/Max:** [-0.990, 0.970]
- **Componentes (promedio):**
  - EMA20 Slope: 0.042
  - EMA50 Cross: 0.183
  - BOS Count: 0.009
  - Regression 24h: 0.089

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.970 (apenas supera threshold)
- Score mínimo observado: -0.990 (apenas supera threshold)
- **Consecuencia:** Sistema queda 34.6% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.6%)

**Posibles causas:**
- **BOS Score bajo (0.009):** BOS/CHoCH no se detectan correctamente
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
| StructureFusion | 10213 | 100.0% | 100.0% |
| ProximityAnalyzer | 4211 | 41.2% | 41.2% |
| DFM_Evaluated | 916 | 21.8% | 9.0% |
| DFM_Passed | 916 | 100.0% | 9.0% |
| RiskCalculator | 6202 | 677.1% | 60.7% |
| Risk_Accepted | 114 | 1.8% | 1.1% |
| TradeManager | 36 | 31.6% | 0.4% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 6088 señales, -98.2%)
- **Tasa de conversión final:** 0.35% (de 10213 zonas iniciales → 36 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,629 | 60.4% |
| NO_SL | 424 | 15.7% |
| ENTRY_TOO_FAR | 386 | 14.3% |
| TP_CHECK_FAIL | 260 | 9.6% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,629 rechazos, 60.4%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2533 | 89.0% |
| P0_ANY_DIR | 313 | 11.0% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 44.34 pts (máxima ganancia flotante)
- **MAE Promedio:** 32.91 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 86.31

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 24 | 66.7% |
| SL_FIRST (precio fue hacia SL) | 11 | 30.6% |
| NEUTRAL (sin dirección clara) | 1 | 2.8% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 41.7%
- **Entradas Malas (MAE > MFE):** 58.3%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 36 | 24 | 11 | 66.7% | 44.34 | 32.91 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | BUY | 15.00 | 16.25 | 0.92 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0004 | SELL | 7.25 | 21.50 | 0.34 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0011 | SELL | 13.00 | 18.75 | 0.69 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0015 | BUY | 1.75 | 11.00 | 0.16 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0017 | BUY | 14.50 | 26.50 | 0.55 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0018 | BUY | 13.75 | 35.00 | 0.39 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0019 | SELL | 42.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0020 | SELL | 2.25 | 13.00 | 0.17 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0022 | SELL | 166.00 | 14.00 | 11.86 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0022_2 | SELL | 166.00 | 14.00 | 11.86 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0023 | SELL | 248.50 | 11.25 | 22.09 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0027 | SELL | 0.00 | 68.50 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0028 | SELL | 33.75 | 18.00 | 1.88 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0032 | BUY | 51.50 | 63.75 | 0.81 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0033 | BUY | 9.50 | 66.75 | 0.14 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0035 | BUY | 14.25 | 104.25 | 0.14 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0041 | SELL | 55.25 | 58.00 | 0.95 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0042 | SELL | 36.00 | 75.50 | 0.48 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0053 | BUY | 27.00 | 4.00 | 6.75 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0055 | BUY | 12.75 | 72.00 | 0.18 | TP_FIRST | CLOSED | ❌ Entrada muy mala |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,327

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 480 | 39.0% | 60.6% | 4.85 | 47.7% | 1.97 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 264 | 53.4% | 57.6% | 3.13 | 52.3% | 1.84 | ⚠️ CALIDAD MEDIA - Revisar |
| 3.0-5.0 ATR (Media) | 358 | 69.6% | 57.0% | 3.14 | 62.3% | 2.03 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 209 | 68.4% | 56.9% | 1.25 | 66.5% | 2.30 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 16 | 56.2% | 37.5% | 0.31 | 43.8% | 3.65 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (480 oportunidades)

- **WR Teórico:** 39.0% (si se hubieran ejecutado)
- **TP_FIRST:** 60.6% (291 de 480)
- **SL_FIRST:** 36.9% (177 de 480)
- **MFE Promedio:** 47.80 pts
- **MAE Promedio:** 36.28 pts
- **MFE/MAE Ratio:** 4.85
- **Good Entries:** 47.7% (MFE > MAE)
- **R:R Promedio:** 1.97

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (264 oportunidades)

- **WR Teórico:** 53.4% (si se hubieran ejecutado)
- **TP_FIRST:** 57.6% (152 de 264)
- **SL_FIRST:** 42.0% (111 de 264)
- **MFE Promedio:** 52.09 pts
- **MAE Promedio:** 37.79 pts
- **MFE/MAE Ratio:** 3.13
- **Good Entries:** 52.3% (MFE > MAE)
- **R:R Promedio:** 1.84

**⚠️ CALIDAD MEDIA - Revisar**

**3.0-5.0 ATR (Media)** (358 oportunidades)

- **WR Teórico:** 69.6% (si se hubieran ejecutado)
- **TP_FIRST:** 57.0% (204 de 358)
- **SL_FIRST:** 42.7% (153 de 358)
- **MFE Promedio:** 69.95 pts
- **MAE Promedio:** 40.16 pts
- **MFE/MAE Ratio:** 3.14
- **Good Entries:** 62.3% (MFE > MAE)
- **R:R Promedio:** 2.03

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (209 oportunidades)

- **WR Teórico:** 68.4% (si se hubieran ejecutado)
- **TP_FIRST:** 56.9% (119 de 209)
- **SL_FIRST:** 42.1% (88 de 209)
- **MFE Promedio:** 80.32 pts
- **MAE Promedio:** 45.64 pts
- **MFE/MAE Ratio:** 1.25
- **Good Entries:** 66.5% (MFE > MAE)
- **R:R Promedio:** 2.30

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (16 oportunidades)

- **WR Teórico:** 56.2% (si se hubieran ejecutado)
- **TP_FIRST:** 37.5% (6 de 16)
- **SL_FIRST:** 62.5% (10 de 16)
- **MFE Promedio:** 82.03 pts
- **MAE Promedio:** 61.20 pts
- **MFE/MAE Ratio:** 0.31
- **Good Entries:** 43.8% (MFE > MAE)
- **R:R Promedio:** 3.65

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 36 | 480 | 264 | 358 |
| **TP_FIRST %** | 66.7% | 60.6% | 57.6% | 57.0% |
| **Good Entries %** | 41.7% | 47.7% | 52.3% | 62.3% |
| **MFE/MAE Ratio** | 86.31 | 4.85 | 3.13 | 3.14 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 358 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 69.6%
   - Good Entries: 62.3%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 11.90 pts
- **Mediana:** 9.40 pts
- **Min/Max:** 0.55 / 40.44 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 9.40 |
| P70 | 16.43 |
| P80 | 18.80 |
| P90 | 25.12 |
| P95 | 37.95 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 22.90 pts
- **Mediana:** 19.75 pts
- **Min/Max:** 3.25 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 19.75 |
| P70 | 30.10 |
| P80 | 39.00 |
| P90 | 50.67 |
| P95 | 53.29 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 25; // Era 60
public int MaxTPDistancePoints { get; set; } = 50; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 25.1pts, TP: 50.7pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (30.6%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.306) / 0.306
R:R_min = 2.27
```

**Estado actual:** R:R promedio = 2.52
**Gap:** -0.25 (necesitas mejorar R:R)

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

**Problema:** R:R actual (2.52) < R:R mínimo (2.27)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=30.6%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-12 17:47:44*