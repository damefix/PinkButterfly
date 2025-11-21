# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-17 10:14:59
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_100330.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251117_100330.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 21
- **Win Rate:** 19.0% (4/21)
- **Profit Factor:** 0.49
- **Avg R:R Planeado:** 2.17
- **R:R Mínimo para Break-Even:** 4.25

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 17 puntos
   - TP máximo observado: 50 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.17
   - R:R necesario: 4.25
   - **Gap:** 2.08

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bullish | 10120 | 39.1% |
| Neutral | 8735 | 33.7% |
| Bearish | 7039 | 27.2% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.080
- **Score Min/Max:** [-1.000, 1.000]
- **Componentes (promedio):**
  - EMA20 Slope: 0.041
  - EMA50 Cross: 0.181
  - BOS Count: 0.015
  - Regression 24h: 0.093

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 1.000 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 33.7% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (33.7%)

**Posibles causas:**
- **BOS Score bajo (0.015):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio 0.080 indica poca señal direccional
- **Mercado lateral:** Scores reales [-1.00, 1.00] muy cercanos a 0

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
| StructureFusion | 68564 | 100.0% | 100.0% |
| ProximityAnalyzer | 1647 | 2.4% | 2.4% |
| DFM_Evaluated | 590 | 35.8% | 0.9% |
| DFM_Passed | 590 | 100.0% | 0.9% |
| RiskCalculator | 3622 | 613.9% | 5.3% |
| Risk_Accepted | 51 | 1.4% | 0.1% |
| TradeManager | 21 | 41.2% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 3571 señales, -98.6%)
- **Tasa de conversión final:** 0.03% (de 68564 zonas iniciales → 21 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,066 | 48.5% |
| ENTRY_TOO_FAR | 585 | 26.6% |
| NO_SL | 291 | 13.2% |
| TP_CHECK_FAIL | 256 | 11.6% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,066 rechazos, 48.5%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 1560 | 93.4% |
| P0_ANY_DIR | 111 | 6.6% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 42.52 pts (máxima ganancia flotante)
- **MAE Promedio:** 5.82 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 9.29

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 21 | 100.0% |
| SL_FIRST (precio fue hacia SL) | 0 | 0.0% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 95.2%
- **Entradas Malas (MAE > MFE):** 4.8%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 21 | 21 | 0 | 100.0% | 42.52 | 5.82 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0049 | SELL | 43.75 | 4.50 | 9.72 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_2 | SELL | 43.75 | 4.50 | 9.72 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_3 | SELL | 43.75 | 4.50 | 9.72 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_4 | SELL | 43.75 | 4.50 | 9.72 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_5 | SELL | 43.75 | 4.50 | 9.72 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_6 | SELL | 43.75 | 4.50 | 9.72 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_7 | SELL | 43.75 | 4.50 | 9.72 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_8 | SELL | 43.75 | 4.50 | 9.72 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_9 | SELL | 43.75 | 4.50 | 9.72 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_10 | SELL | 43.75 | 4.50 | 9.72 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_11 | SELL | 43.75 | 4.50 | 9.72 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_12 | SELL | 43.75 | 4.50 | 9.72 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_13 | SELL | 43.75 | 4.50 | 9.72 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_14 | SELL | 43.75 | 4.50 | 9.72 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_15 | SELL | 43.75 | 4.50 | 9.72 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_16 | SELL | 43.75 | 4.50 | 9.72 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_17 | SELL | 43.75 | 4.50 | 9.72 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_18 | SELL | 43.75 | 4.50 | 9.72 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_19 | SELL | 43.75 | 4.50 | 9.72 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_20 | SELL | 43.75 | 4.50 | 9.72 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,036

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 357 | 75.1% | 93.6% | 16.82 | 94.4% | 1.88 | ✅ BUENA CALIDAD - Considerar incluir |
| 2.0-3.0 ATR (Cerca) | 146 | 80.1% | 94.5% | 19.53 | 94.5% | 1.95 | ✅ BUENA CALIDAD - Considerar incluir |
| 3.0-5.0 ATR (Media) | 293 | 73.7% | 90.1% | 14.07 | 90.1% | 1.88 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 194 | 80.4% | 94.8% | 11.06 | 94.8% | 2.37 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 46 | 100.0% | 100.0% | 15.08 | 100.0% | 2.82 | ✅ BUENA CALIDAD - Considerar incluir |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (357 oportunidades)

- **WR Teórico:** 75.1% (si se hubieran ejecutado)
- **TP_FIRST:** 93.6% (334 de 357)
- **SL_FIRST:** 6.2% (22 de 357)
- **MFE Promedio:** 43.54 pts
- **MAE Promedio:** 6.51 pts
- **MFE/MAE Ratio:** 16.82
- **Good Entries:** 94.4% (MFE > MAE)
- **R:R Promedio:** 1.88

**✅ BUENA CALIDAD - Considerar incluir**

**2.0-3.0 ATR (Cerca)** (146 oportunidades)

- **WR Teórico:** 80.1% (si se hubieran ejecutado)
- **TP_FIRST:** 94.5% (138 de 146)
- **SL_FIRST:** 5.5% (8 de 146)
- **MFE Promedio:** 46.13 pts
- **MAE Promedio:** 5.34 pts
- **MFE/MAE Ratio:** 19.53
- **Good Entries:** 94.5% (MFE > MAE)
- **R:R Promedio:** 1.95

**✅ BUENA CALIDAD - Considerar incluir**

**3.0-5.0 ATR (Media)** (293 oportunidades)

- **WR Teórico:** 73.7% (si se hubieran ejecutado)
- **TP_FIRST:** 90.1% (264 de 293)
- **SL_FIRST:** 9.9% (29 de 293)
- **MFE Promedio:** 41.00 pts
- **MAE Promedio:** 8.20 pts
- **MFE/MAE Ratio:** 14.07
- **Good Entries:** 90.1% (MFE > MAE)
- **R:R Promedio:** 1.88

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (194 oportunidades)

- **WR Teórico:** 80.4% (si se hubieran ejecutado)
- **TP_FIRST:** 94.8% (184 de 194)
- **SL_FIRST:** 5.2% (10 de 194)
- **MFE Promedio:** 47.69 pts
- **MAE Promedio:** 8.62 pts
- **MFE/MAE Ratio:** 11.06
- **Good Entries:** 94.8% (MFE > MAE)
- **R:R Promedio:** 2.37

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (46 oportunidades)

- **WR Teórico:** 100.0% (si se hubieran ejecutado)
- **TP_FIRST:** 100.0% (46 de 46)
- **SL_FIRST:** 0.0% (0 de 46)
- **MFE Promedio:** 53.24 pts
- **MAE Promedio:** 3.00 pts
- **MFE/MAE Ratio:** 15.08
- **Good Entries:** 100.0% (MFE > MAE)
- **R:R Promedio:** 2.82

**✅ BUENA CALIDAD - Considerar incluir**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 21 | 357 | 146 | 293 |
| **TP_FIRST %** | 100.0% | 93.6% | 94.5% | 90.1% |
| **Good Entries %** | 95.2% | 94.4% | 94.5% | 90.1% |
| **MFE/MAE Ratio** | 9.29 | 16.82 | 19.53 | 14.07 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

🚨 **CRÍTICO: El rango 2.0-3.0 ATR contiene 146 oportunidades de BUENA CALIDAD**
   - WR Teórico: 80.1%
   - Good Entries: 94.5%
   - **ACCIÓN:** Considerar aumentar MaxDistanceToRegister_ATR_Normal de 2.0 a 3.0

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 293 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 73.7%
   - Good Entries: 90.1%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 13.46 pts
- **Mediana:** 13.29 pts
- **Min/Max:** 13.29 / 16.81 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 13.29 |
| P70 | 13.29 |
| P80 | 13.29 |
| P90 | 13.29 |
| P95 | 16.46 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 29.26 pts
- **Mediana:** 28.25 pts
- **Min/Max:** 28.25 / 49.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 28.25 |
| P70 | 28.25 |
| P80 | 28.25 |
| P90 | 28.25 |
| P95 | 47.38 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 13; // Era 60
public int MaxTPDistancePoints { get; set; } = 28; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 13.3pts, TP: 28.2pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (19.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.190) / 0.190
R:R_min = 4.25
```

**Estado actual:** R:R promedio = 2.17
**Gap:** 2.08 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **13** (P90 real)
2. **MaxTPDistancePoints:** 120 → **28** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.17) < R:R mínimo (4.25)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=19.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-17 10:14:59*