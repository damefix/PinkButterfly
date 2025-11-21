# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-13 10:39:37
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_103602.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251113_103602.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 43
- **Win Rate:** 27.9% (12/43)
- **Profit Factor:** 0.58
- **Avg R:R Planeado:** 2.09
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
   - R:R actual: 2.09
   - R:R necesario: 2.58
   - **Gap:** 0.49

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
| StructureFusion | 10292 | 100.0% | 100.0% |
| ProximityAnalyzer | 4115 | 40.0% | 40.0% |
| DFM_Evaluated | 813 | 19.8% | 7.9% |
| DFM_Passed | 813 | 100.0% | 7.9% |
| RiskCalculator | 6280 | 772.4% | 61.0% |
| Risk_Accepted | 114 | 1.8% | 1.1% |
| TradeManager | 43 | 37.7% | 0.4% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 6166 señales, -98.2%)
- **Tasa de conversión final:** 0.42% (de 10292 zonas iniciales → 43 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,643 | 58.7% |
| NO_SL | 532 | 19.0% |
| ENTRY_TOO_FAR | 374 | 13.4% |
| TP_CHECK_FAIL | 252 | 9.0% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,643 rechazos, 58.7%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2499 | 89.0% |
| P0_ANY_DIR | 309 | 11.0% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 43.97 pts (máxima ganancia flotante)
- **MAE Promedio:** 27.76 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 71.73

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
| CLOSED | 43 | 25 | 18 | 58.1% | 43.97 | 27.76 |

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
| T0047 | BUY | 27.00 | 4.00 | 6.75 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,282

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 461 | 34.9% | 60.3% | 3.75 | 46.9% | 2.06 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 241 | 49.4% | 48.1% | 1.80 | 48.5% | 1.96 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 362 | 68.8% | 54.1% | 2.22 | 62.7% | 2.01 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 201 | 64.2% | 56.7% | 0.65 | 61.2% | 2.33 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 17 | 41.2% | 17.6% | 0.36 | 29.4% | 3.36 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (461 oportunidades)

- **WR Teórico:** 34.9% (si se hubieran ejecutado)
- **TP_FIRST:** 60.3% (278 de 461)
- **SL_FIRST:** 37.1% (171 de 461)
- **MFE Promedio:** 39.63 pts
- **MAE Promedio:** 36.97 pts
- **MFE/MAE Ratio:** 3.75
- **Good Entries:** 46.9% (MFE > MAE)
- **R:R Promedio:** 2.06

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (241 oportunidades)

- **WR Teórico:** 49.4% (si se hubieran ejecutado)
- **TP_FIRST:** 48.1% (116 de 241)
- **SL_FIRST:** 51.9% (125 de 241)
- **MFE Promedio:** 58.07 pts
- **MAE Promedio:** 43.00 pts
- **MFE/MAE Ratio:** 1.80
- **Good Entries:** 48.5% (MFE > MAE)
- **R:R Promedio:** 1.96

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (362 oportunidades)

- **WR Teórico:** 68.8% (si se hubieran ejecutado)
- **TP_FIRST:** 54.1% (196 de 362)
- **SL_FIRST:** 45.6% (165 de 362)
- **MFE Promedio:** 69.29 pts
- **MAE Promedio:** 41.75 pts
- **MFE/MAE Ratio:** 2.22
- **Good Entries:** 62.7% (MFE > MAE)
- **R:R Promedio:** 2.01

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (201 oportunidades)

- **WR Teórico:** 64.2% (si se hubieran ejecutado)
- **TP_FIRST:** 56.7% (114 de 201)
- **SL_FIRST:** 42.8% (86 de 201)
- **MFE Promedio:** 83.50 pts
- **MAE Promedio:** 46.88 pts
- **MFE/MAE Ratio:** 0.65
- **Good Entries:** 61.2% (MFE > MAE)
- **R:R Promedio:** 2.33

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (17 oportunidades)

- **WR Teórico:** 41.2% (si se hubieran ejecutado)
- **TP_FIRST:** 17.6% (3 de 17)
- **SL_FIRST:** 82.4% (14 de 17)
- **MFE Promedio:** 96.14 pts
- **MAE Promedio:** 63.04 pts
- **MFE/MAE Ratio:** 0.36
- **Good Entries:** 29.4% (MFE > MAE)
- **R:R Promedio:** 3.36

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 43 | 461 | 241 | 362 |
| **TP_FIRST %** | 58.1% | 60.3% | 48.1% | 54.1% |
| **Good Entries %** | 48.8% | 46.9% | 48.5% | 62.7% |
| **MFE/MAE Ratio** | 71.73 | 3.75 | 1.80 | 2.22 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 362 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 68.8%
   - Good Entries: 62.7%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 12.12 pts
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

- **Media:** 21.35 pts
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

**Estado actual:** R:R promedio = 2.09
**Gap:** 0.49 (necesitas mejorar R:R)

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

**Problema:** R:R actual (2.09) < R:R mínimo (2.58)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=27.9%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-13 10:39:37*