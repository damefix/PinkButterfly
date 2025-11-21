# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-17 15:44:27
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_153549.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251117_153549.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 12
- **Win Rate:** 58.3% (7/12)
- **Profit Factor:** 2.20
- **Avg R:R Planeado:** 1.81
- **R:R Mínimo para Break-Even:** 0.71

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 13 puntos
   - TP máximo observado: 20 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.81
   - R:R necesario: 0.71
   - **Gap:** -1.09

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8115 | 34.7% |
| Bullish | 9088 | 38.8% |
| Bearish | 6213 | 26.5% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.083
- **Score Min/Max:** [-1.000, 1.000]
- **Componentes (promedio):**
  - EMA20 Slope: 0.044
  - EMA50 Cross: 0.197
  - BOS Count: 0.008
  - Regression 24h: 0.092

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 1.000 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 34.7% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.7%)

**Posibles causas:**
- **BOS Score bajo (0.008):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio 0.083 indica poca señal direccional
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
| StructureFusion | 107359 | 100.0% | 100.0% |
| ProximityAnalyzer | 1201 | 1.1% | 1.1% |
| DFM_Evaluated | 255 | 21.2% | 0.2% |
| DFM_Passed | 255 | 100.0% | 0.2% |
| RiskCalculator | 2861 | 1122.0% | 2.7% |
| Risk_Accepted | 3 | 0.1% | 0.0% |
| TradeManager | 12 | 400.0% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 2858 señales, -99.9%)
- **Tasa de conversión final:** 0.01% (de 107359 zonas iniciales → 12 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 523 | 42.1% |
| ENTRY_TOO_FAR | 364 | 29.3% |
| TP_CHECK_FAIL | 279 | 22.4% |
| NO_SL | 77 | 6.2% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (523 rechazos, 42.1%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 1664 | 99.2% |
| P0_ANY_DIR | 14 | 0.8% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 84.46 pts (máxima ganancia flotante)
- **MAE Promedio:** 15.48 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 8.39

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 4 | 33.3% |
| SL_FIRST (precio fue hacia SL) | 8 | 66.7% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 66.7%
- **Entradas Malas (MAE > MFE):** 33.3%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 12 | 4 | 8 | 33.3% | 84.46 | 15.48 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0003 | SELL | 23.75 | 12.25 | 1.94 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0008 | SELL | 11.75 | 31.50 | 0.37 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0009 | SELL | 11.75 | 31.50 | 0.37 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0014 | SELL | 116.00 | 31.25 | 3.71 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0017 | SELL | 243.50 | 13.75 | 17.71 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0017_2 | SELL | 243.50 | 13.75 | 17.71 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0020 | SELL | 12.00 | 12.50 | 0.96 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0022 | SELL | 10.25 | 15.25 | 0.67 | SL_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0023 | SELL | 85.50 | 6.50 | 13.15 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0024 | SELL | 84.50 | 6.50 | 13.00 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0026 | SELL | 85.50 | 5.50 | 15.55 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0026_2 | SELL | 85.50 | 5.50 | 15.55 | SL_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 724

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 159 | 53.5% | 40.9% | 3.04 | 49.1% | 4.18 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 107 | 53.3% | 42.1% | 2.26 | 46.7% | 4.18 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 195 | 54.9% | 48.7% | 8.86 | 54.4% | 3.47 | ⚠️ CALIDAD MEDIA - Revisar |
| 5.0-10.0 ATR (Lejos) | 173 | 45.1% | 34.1% | 1.10 | 48.0% | 3.89 | ❌ BAJA CALIDAD - Filtro correcto |
| >10.0 ATR (Muy lejos) | 90 | 35.6% | 24.4% | 0.40 | 12.2% | 3.03 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (159 oportunidades)

- **WR Teórico:** 53.5% (si se hubieran ejecutado)
- **TP_FIRST:** 40.9% (65 de 159)
- **SL_FIRST:** 45.3% (72 de 159)
- **MFE Promedio:** 47.63 pts
- **MAE Promedio:** 25.13 pts
- **MFE/MAE Ratio:** 3.04
- **Good Entries:** 49.1% (MFE > MAE)
- **R:R Promedio:** 4.18

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (107 oportunidades)

- **WR Teórico:** 53.3% (si se hubieran ejecutado)
- **TP_FIRST:** 42.1% (45 de 107)
- **SL_FIRST:** 49.5% (53 de 107)
- **MFE Promedio:** 51.57 pts
- **MAE Promedio:** 27.82 pts
- **MFE/MAE Ratio:** 2.26
- **Good Entries:** 46.7% (MFE > MAE)
- **R:R Promedio:** 4.18

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (195 oportunidades)

- **WR Teórico:** 54.9% (si se hubieran ejecutado)
- **TP_FIRST:** 48.7% (95 de 195)
- **SL_FIRST:** 46.2% (90 de 195)
- **MFE Promedio:** 44.36 pts
- **MAE Promedio:** 29.06 pts
- **MFE/MAE Ratio:** 8.86
- **Good Entries:** 54.4% (MFE > MAE)
- **R:R Promedio:** 3.47

**⚠️ CALIDAD MEDIA - Revisar**

**5.0-10.0 ATR (Lejos)** (173 oportunidades)

- **WR Teórico:** 45.1% (si se hubieran ejecutado)
- **TP_FIRST:** 34.1% (59 de 173)
- **SL_FIRST:** 62.4% (108 de 173)
- **MFE Promedio:** 52.26 pts
- **MAE Promedio:** 35.57 pts
- **MFE/MAE Ratio:** 1.10
- **Good Entries:** 48.0% (MFE > MAE)
- **R:R Promedio:** 3.89

**❌ BAJA CALIDAD - Filtro correcto**

**>10.0 ATR (Muy lejos)** (90 oportunidades)

- **WR Teórico:** 35.6% (si se hubieran ejecutado)
- **TP_FIRST:** 24.4% (22 de 90)
- **SL_FIRST:** 75.6% (68 de 90)
- **MFE Promedio:** 30.63 pts
- **MAE Promedio:** 68.70 pts
- **MFE/MAE Ratio:** 0.40
- **Good Entries:** 12.2% (MFE > MAE)
- **R:R Promedio:** 3.03

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 12 | 159 | 107 | 195 |
| **TP_FIRST %** | 33.3% | 40.9% | 42.1% | 48.7% |
| **Good Entries %** | 66.7% | 49.1% | 46.7% | 54.4% |
| **MFE/MAE Ratio** | 8.39 | 3.04 | 2.26 | 8.86 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

✅ **CORRECTO: Las 195 oportunidades en 3.0-5.0 ATR tienen baja calidad**


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 5.90 pts
- **Mediana:** 4.59 pts
- **Min/Max:** 3.25 / 13.18 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 4.59 |
| P70 | 4.74 |
| P80 | 8.80 |
| P90 | 13.18 |
| P95 | 13.18 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 10.15 pts
- **Mediana:** 8.00 pts
- **Min/Max:** 6.50 / 19.75 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 8.00 |
| P70 | 11.25 |
| P80 | 14.65 |
| P90 | 19.75 |
| P95 | 19.75 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 13; // Era 60
public int MaxTPDistancePoints { get; set; } = 19; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 13.2pts, TP: 19.8pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (58.3%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.583) / 0.583
R:R_min = 0.71
```

**Estado actual:** R:R promedio = 1.81
**Gap:** -1.09 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **19** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.81) < R:R mínimo (0.71)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=58.3%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-17 15:44:27*