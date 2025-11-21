# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-12 16:31:54
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_162649.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251112_162649.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 52
- **Win Rate:** 30.8% (16/52)
- **Profit Factor:** 0.86
- **Avg R:R Planeado:** 2.10
- **R:R Mínimo para Break-Even:** 2.25

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 40 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.10
   - R:R necesario: 2.25
   - **Gap:** 0.15

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8085 | 34.6% |
| Bullish | 8976 | 38.4% |
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
| StructureFusion | 10189 | 100.0% | 100.0% |
| ProximityAnalyzer | 4202 | 41.2% | 41.2% |
| DFM_Evaluated | 1305 | 31.1% | 12.8% |
| DFM_Passed | 1136 | 87.0% | 11.1% |
| RiskCalculator | 6164 | 542.6% | 60.5% |
| Risk_Accepted | 140 | 2.3% | 1.4% |
| TradeManager | 52 | 37.1% | 0.5% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 6024 señales, -97.7%)
- **Tasa de conversión final:** 0.51% (de 10189 zonas iniciales → 52 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,624 | 60.7% |
| NO_SL | 412 | 15.4% |
| ENTRY_TOO_FAR | 381 | 14.2% |
| TP_CHECK_FAIL | 259 | 9.7% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,624 rechazos, 60.7%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2533 | 89.0% |
| P0_ANY_DIR | 313 | 11.0% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 32.75 pts (máxima ganancia flotante)
- **MAE Promedio:** 38.39 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 40.64

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 27 | 51.9% |
| SL_FIRST (precio fue hacia SL) | 24 | 46.2% |
| NEUTRAL (sin dirección clara) | 1 | 1.9% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 38.5%
- **Entradas Malas (MAE > MFE):** 61.5%

⚠️ **ALERTA:** >60% de entradas tienen MAE > MFE
- **Problema:** El precio va más en contra que a favor antes del cierre
- **Causas posibles:**
  1. Timing incorrecto (entramos antes de reversión)
  2. Bias desincronizado (operamos contra tendencia real)
  3. Zonas de baja calidad (sin confluence real)

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 52 | 27 | 24 | 51.9% | 32.75 | 38.39 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0003 | SELL | 7.00 | 19.25 | 0.36 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0006 | BUY | 0.00 | 49.75 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0012 | SELL | 13.00 | 18.75 | 0.69 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0014 | SELL | 0.00 | 43.00 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0016 | BUY | 1.75 | 11.00 | 0.16 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0018 | BUY | 14.50 | 26.50 | 0.55 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0019 | BUY | 13.75 | 35.00 | 0.39 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0021 | SELL | 42.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0024 | SELL | 0.75 | 13.00 | 0.06 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0027 | SELL | 145.75 | 19.50 | 7.47 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0027_2 | SELL | 145.75 | 19.50 | 7.47 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0028 | SELL | 248.50 | 11.25 | 22.09 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0033 | SELL | 0.00 | 68.50 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0035 | SELL | 33.75 | 18.00 | 1.88 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0039 | BUY | 51.50 | 63.75 | 0.81 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0040 | BUY | 9.50 | 66.75 | 0.14 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0044 | BUY | 29.25 | 6.25 | 4.68 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0045 | BUY | 14.25 | 104.25 | 0.14 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0046 | BUY | 9.50 | 85.00 | 0.11 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0049 | SELL | 55.25 | 58.00 | 0.95 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,331

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 483 | 37.5% | 58.2% | 4.15 | 44.7% | 1.97 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 265 | 50.9% | 57.7% | 2.53 | 49.1% | 1.83 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 351 | 67.2% | 55.8% | 2.45 | 57.5% | 2.00 | ⚠️ CALIDAD MEDIA - Revisar |
| 5.0-10.0 ATR (Lejos) | 214 | 64.5% | 57.0% | 0.97 | 63.1% | 2.33 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 18 | 55.6% | 38.9% | 0.26 | 44.4% | 3.39 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (483 oportunidades)

- **WR Teórico:** 37.5% (si se hubieran ejecutado)
- **TP_FIRST:** 58.2% (281 de 483)
- **SL_FIRST:** 39.3% (190 de 483)
- **MFE Promedio:** 45.47 pts
- **MAE Promedio:** 35.94 pts
- **MFE/MAE Ratio:** 4.15
- **Good Entries:** 44.7% (MFE > MAE)
- **R:R Promedio:** 1.97

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (265 oportunidades)

- **WR Teórico:** 50.9% (si se hubieran ejecutado)
- **TP_FIRST:** 57.7% (153 de 265)
- **SL_FIRST:** 41.9% (111 de 265)
- **MFE Promedio:** 48.37 pts
- **MAE Promedio:** 36.90 pts
- **MFE/MAE Ratio:** 2.53
- **Good Entries:** 49.1% (MFE > MAE)
- **R:R Promedio:** 1.83

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (351 oportunidades)

- **WR Teórico:** 67.2% (si se hubieran ejecutado)
- **TP_FIRST:** 55.8% (196 de 351)
- **SL_FIRST:** 43.9% (154 de 351)
- **MFE Promedio:** 68.27 pts
- **MAE Promedio:** 40.59 pts
- **MFE/MAE Ratio:** 2.45
- **Good Entries:** 57.5% (MFE > MAE)
- **R:R Promedio:** 2.00

**⚠️ CALIDAD MEDIA - Revisar**

**5.0-10.0 ATR (Lejos)** (214 oportunidades)

- **WR Teórico:** 64.5% (si se hubieran ejecutado)
- **TP_FIRST:** 57.0% (122 de 214)
- **SL_FIRST:** 42.1% (90 de 214)
- **MFE Promedio:** 79.42 pts
- **MAE Promedio:** 44.58 pts
- **MFE/MAE Ratio:** 0.97
- **Good Entries:** 63.1% (MFE > MAE)
- **R:R Promedio:** 2.33

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (18 oportunidades)

- **WR Teórico:** 55.6% (si se hubieran ejecutado)
- **TP_FIRST:** 38.9% (7 de 18)
- **SL_FIRST:** 61.1% (11 de 18)
- **MFE Promedio:** 76.45 pts
- **MAE Promedio:** 58.68 pts
- **MFE/MAE Ratio:** 0.26
- **Good Entries:** 44.4% (MFE > MAE)
- **R:R Promedio:** 3.39

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 52 | 483 | 265 | 351 |
| **TP_FIRST %** | 51.9% | 58.2% | 57.7% | 55.8% |
| **Good Entries %** | 38.5% | 44.7% | 49.1% | 57.5% |
| **MFE/MAE Ratio** | 40.64 | 4.15 | 2.53 | 2.45 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 351 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 67.2%
   - Good Entries: 57.5%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 11.86 pts
- **Mediana:** 9.71 pts
- **Min/Max:** 0.55 / 40.44 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 9.71 |
| P70 | 15.81 |
| P80 | 17.91 |
| P90 | 23.61 |
| P95 | 31.45 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 21.57 pts
- **Mediana:** 19.12 pts
- **Min/Max:** 3.25 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 19.12 |
| P70 | 26.62 |
| P80 | 33.70 |
| P90 | 49.52 |
| P95 | 52.60 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 23; // Era 60
public int MaxTPDistancePoints { get; set; } = 49; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 23.6pts, TP: 49.5pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (30.8%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.308) / 0.308
R:R_min = 2.25
```

**Estado actual:** R:R promedio = 2.10
**Gap:** 0.15 (necesitas mejorar R:R)

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

**Problema:** R:R actual (2.10) < R:R mínimo (2.25)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=30.8%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-12 16:31:54*