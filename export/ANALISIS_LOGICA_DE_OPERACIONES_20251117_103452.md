# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-17 10:46:18
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_103452.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251117_103452.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 44
- **Win Rate:** 25.0% (11/44)
- **Profit Factor:** 0.49
- **Avg R:R Planeado:** 1.90
- **R:R Mínimo para Break-Even:** 3.00

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 29 puntos
   - TP máximo observado: 50 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.90
   - R:R necesario: 3.00
   - **Gap:** 1.10

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bullish | 10120 | 39.1% |
| Neutral | 8737 | 33.7% |
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
| StructureFusion | 68571 | 100.0% | 100.0% |
| ProximityAnalyzer | 1647 | 2.4% | 2.4% |
| DFM_Evaluated | 590 | 35.8% | 0.9% |
| DFM_Passed | 590 | 100.0% | 0.9% |
| RiskCalculator | 3626 | 614.6% | 5.3% |
| Risk_Accepted | 130 | 3.6% | 0.2% |
| TradeManager | 44 | 33.8% | 0.1% |

**Análisis:**
- **Mayor caída:** ProximityAnalyzer (pierde 66924 señales, -97.6%)
- **Tasa de conversión final:** 0.06% (de 68571 zonas iniciales → 44 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,067 | 48.5% |
| ENTRY_TOO_FAR | 585 | 26.6% |
| NO_SL | 291 | 13.2% |
| TP_CHECK_FAIL | 259 | 11.8% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,067 rechazos, 48.5%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 1562 | 93.4% |
| P0_ANY_DIR | 111 | 6.6% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 45.38 pts (máxima ganancia flotante)
- **MAE Promedio:** 13.76 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 460.50

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 36 | 81.8% |
| SL_FIRST (precio fue hacia SL) | 8 | 18.2% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 86.4%
- **Entradas Malas (MAE > MFE):** 13.6%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 44 | 36 | 8 | 81.8% | 45.38 | 13.76 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0049 | SELL | 43.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_2 | SELL | 43.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_3 | SELL | 43.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_4 | SELL | 43.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_5 | SELL | 43.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_6 | SELL | 43.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_7 | SELL | 43.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_8 | SELL | 43.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_9 | SELL | 43.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_10 | SELL | 43.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_11 | SELL | 43.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_12 | SELL | 43.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_13 | SELL | 43.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_14 | SELL | 43.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_15 | SELL | 43.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_16 | SELL | 43.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_17 | SELL | 43.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_18 | SELL | 43.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_19 | SELL | 43.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0049_20 | SELL | 43.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,036

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 357 | 51.5% | 56.6% | 10.19 | 76.8% | 1.88 | ✅ BUENA CALIDAD - Considerar incluir |
| 2.0-3.0 ATR (Cerca) | 146 | 63.0% | 76.7% | 8.42 | 82.2% | 1.95 | ✅ BUENA CALIDAD - Considerar incluir |
| 3.0-5.0 ATR (Media) | 293 | 71.7% | 79.2% | 12.61 | 79.9% | 1.88 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 194 | 77.3% | 90.7% | 4.18 | 91.8% | 2.37 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 46 | 69.6% | 60.9% | 2.95 | 100.0% | 2.82 | ✅ BUENA CALIDAD - Considerar incluir |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (357 oportunidades)

- **WR Teórico:** 51.5% (si se hubieran ejecutado)
- **TP_FIRST:** 56.6% (202 de 357)
- **SL_FIRST:** 40.3% (144 de 357)
- **MFE Promedio:** 52.61 pts
- **MAE Promedio:** 21.38 pts
- **MFE/MAE Ratio:** 10.19
- **Good Entries:** 76.8% (MFE > MAE)
- **R:R Promedio:** 1.88

**✅ BUENA CALIDAD - Considerar incluir**

**2.0-3.0 ATR (Cerca)** (146 oportunidades)

- **WR Teórico:** 63.0% (si se hubieran ejecutado)
- **TP_FIRST:** 76.7% (112 de 146)
- **SL_FIRST:** 23.3% (34 de 146)
- **MFE Promedio:** 59.60 pts
- **MAE Promedio:** 19.66 pts
- **MFE/MAE Ratio:** 8.42
- **Good Entries:** 82.2% (MFE > MAE)
- **R:R Promedio:** 1.95

**✅ BUENA CALIDAD - Considerar incluir**

**3.0-5.0 ATR (Media)** (293 oportunidades)

- **WR Teórico:** 71.7% (si se hubieran ejecutado)
- **TP_FIRST:** 79.2% (232 de 293)
- **SL_FIRST:** 17.4% (51 de 293)
- **MFE Promedio:** 66.77 pts
- **MAE Promedio:** 25.38 pts
- **MFE/MAE Ratio:** 12.61
- **Good Entries:** 79.9% (MFE > MAE)
- **R:R Promedio:** 1.88

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (194 oportunidades)

- **WR Teórico:** 77.3% (si se hubieran ejecutado)
- **TP_FIRST:** 90.7% (176 de 194)
- **SL_FIRST:** 7.2% (14 de 194)
- **MFE Promedio:** 83.28 pts
- **MAE Promedio:** 21.23 pts
- **MFE/MAE Ratio:** 4.18
- **Good Entries:** 91.8% (MFE > MAE)
- **R:R Promedio:** 2.37

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (46 oportunidades)

- **WR Teórico:** 69.6% (si se hubieran ejecutado)
- **TP_FIRST:** 60.9% (28 de 46)
- **SL_FIRST:** 39.1% (18 de 46)
- **MFE Promedio:** 80.14 pts
- **MAE Promedio:** 15.69 pts
- **MFE/MAE Ratio:** 2.95
- **Good Entries:** 100.0% (MFE > MAE)
- **R:R Promedio:** 2.82

**✅ BUENA CALIDAD - Considerar incluir**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 44 | 357 | 146 | 293 |
| **TP_FIRST %** | 81.8% | 56.6% | 76.7% | 79.2% |
| **Good Entries %** | 86.4% | 76.8% | 82.2% | 79.9% |
| **MFE/MAE Ratio** | 460.50 | 10.19 | 8.42 | 12.61 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

🚨 **CRÍTICO: El rango 2.0-3.0 ATR contiene 146 oportunidades de BUENA CALIDAD**
   - WR Teórico: 63.0%
   - Good Entries: 82.2%
   - **ACCIÓN:** Considerar aumentar MaxDistanceToRegister_ATR_Normal de 2.0 a 3.0

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 293 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 71.7%
   - Good Entries: 79.9%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 13.49 pts
- **Mediana:** 13.29 pts
- **Min/Max:** 9.50 / 28.70 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 13.29 |
| P70 | 13.29 |
| P80 | 13.29 |
| P90 | 23.31 |
| P95 | 27.70 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 25.34 pts
- **Mediana:** 28.25 pts
- **Min/Max:** 15.25 / 49.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 28.25 |
| P70 | 28.25 |
| P80 | 28.25 |
| P90 | 37.00 |
| P95 | 41.69 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 23; // Era 60
public int MaxTPDistancePoints { get; set; } = 37; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 23.3pts, TP: 37.0pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (25.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.250) / 0.250
R:R_min = 3.00
```

**Estado actual:** R:R promedio = 1.90
**Gap:** 1.10 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **37** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.90) < R:R mínimo (3.00)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=25.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-17 10:46:18*