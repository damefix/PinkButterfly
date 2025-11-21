# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-14 18:16:21
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_181300.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251114_181300.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 42
- **Win Rate:** 38.1% (16/42)
- **Profit Factor:** 1.27
- **Avg R:R Planeado:** 2.05
- **R:R Mínimo para Break-Even:** 1.63

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 41 puntos
   - TP máximo observado: 55 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.05
   - R:R necesario: 1.63
   - **Gap:** -0.43

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8065 | 34.3% |
| Bearish | 6356 | 27.0% |
| Bullish | 9098 | 38.7% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.079
- **Score Min/Max:** [-0.990, 0.970]
- **Componentes (promedio):**
  - EMA20 Slope: 0.039
  - EMA50 Cross: 0.194
  - BOS Count: 0.008
  - Regression 24h: 0.087

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.970 (apenas supera threshold)
- Score mínimo observado: -0.990 (apenas supera threshold)
- **Consecuencia:** Sistema queda 34.3% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.3%)

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
| StructureFusion | 10499 | 100.0% | 100.0% |
| ProximityAnalyzer | 3316 | 31.6% | 31.6% |
| DFM_Evaluated | 774 | 23.3% | 7.4% |
| DFM_Passed | 774 | 100.0% | 7.4% |
| RiskCalculator | 6041 | 780.5% | 57.5% |
| Risk_Accepted | 88 | 1.5% | 0.8% |
| TradeManager | 42 | 47.7% | 0.4% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 5953 señales, -98.5%)
- **Tasa de conversión final:** 0.40% (de 10499 zonas iniciales → 42 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,292 | 48.5% |
| NO_SL | 757 | 28.4% |
| ENTRY_TOO_FAR | 405 | 15.2% |
| TP_CHECK_FAIL | 210 | 7.9% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,292 rechazos, 48.5%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2560 | 88.8% |
| P0_ANY_DIR | 324 | 11.2% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 40.84 pts (máxima ganancia flotante)
- **MAE Promedio:** 29.24 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 26.82

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 19 | 45.2% |
| SL_FIRST (precio fue hacia SL) | 20 | 47.6% |
| NEUTRAL (sin dirección clara) | 3 | 7.1% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 50.0%
- **Entradas Malas (MAE > MFE):** 50.0%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 42 | 19 | 20 | 45.2% | 40.84 | 29.24 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | SELL | 6.75 | 34.25 | 0.20 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0003 | BUY | 1.75 | 18.75 | 0.09 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0004 | SELL | 1.75 | 13.00 | 0.13 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0005 | SELL | 144.75 | 20.50 | 7.06 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0006 | SELL | 245.50 | 14.25 | 17.23 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0009 | SELL | 86.75 | 10.00 | 8.68 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0011 | SELL | 1.25 | 68.50 | 0.02 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0011_2 | SELL | 1.25 | 68.50 | 0.02 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0013 | SELL | 10.50 | 46.50 | 0.23 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0015 | SELL | 0.00 | 40.75 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0017 | SELL | 55.25 | 58.00 | 0.95 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0018 | SELL | 23.75 | 102.25 | 0.23 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0019 | SELL | 17.00 | 63.25 | 0.27 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0025 | SELL | 14.50 | 19.75 | 0.73 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0026 | SELL | 13.00 | 11.50 | 1.13 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0027 | SELL | 82.25 | 9.75 | 8.44 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0028 | SELL | 81.75 | 7.75 | 10.55 | NEUTRAL | CLOSED | ✅ Entrada excelente |
| T0030 | SELL | 89.00 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0035 | SELL | 0.00 | 37.00 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0036 | BUY | 44.25 | 7.50 | 5.90 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,257

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 465 | 26.2% | 57.8% | 3.40 | 42.2% | 2.01 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 247 | 50.6% | 55.5% | 2.60 | 51.4% | 1.93 | ⚠️ CALIDAD MEDIA - Revisar |
| 3.0-5.0 ATR (Media) | 323 | 69.3% | 59.8% | 2.45 | 62.8% | 2.04 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 211 | 75.4% | 74.4% | 0.95 | 74.4% | 2.21 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 11 | 36.4% | 36.4% | 0.00 | 36.4% | 3.10 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (465 oportunidades)

- **WR Teórico:** 26.2% (si se hubieran ejecutado)
- **TP_FIRST:** 57.8% (269 de 465)
- **SL_FIRST:** 38.9% (181 de 465)
- **MFE Promedio:** 43.55 pts
- **MAE Promedio:** 44.57 pts
- **MFE/MAE Ratio:** 3.40
- **Good Entries:** 42.2% (MFE > MAE)
- **R:R Promedio:** 2.01

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (247 oportunidades)

- **WR Teórico:** 50.6% (si se hubieran ejecutado)
- **TP_FIRST:** 55.5% (137 de 247)
- **SL_FIRST:** 43.7% (108 de 247)
- **MFE Promedio:** 64.96 pts
- **MAE Promedio:** 41.55 pts
- **MFE/MAE Ratio:** 2.60
- **Good Entries:** 51.4% (MFE > MAE)
- **R:R Promedio:** 1.93

**⚠️ CALIDAD MEDIA - Revisar**

**3.0-5.0 ATR (Media)** (323 oportunidades)

- **WR Teórico:** 69.3% (si se hubieran ejecutado)
- **TP_FIRST:** 59.8% (193 de 323)
- **SL_FIRST:** 39.9% (129 de 323)
- **MFE Promedio:** 82.53 pts
- **MAE Promedio:** 43.62 pts
- **MFE/MAE Ratio:** 2.45
- **Good Entries:** 62.8% (MFE > MAE)
- **R:R Promedio:** 2.04

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (211 oportunidades)

- **WR Teórico:** 75.4% (si se hubieran ejecutado)
- **TP_FIRST:** 74.4% (157 de 211)
- **SL_FIRST:** 25.6% (54 de 211)
- **MFE Promedio:** 91.30 pts
- **MAE Promedio:** 50.49 pts
- **MFE/MAE Ratio:** 0.95
- **Good Entries:** 74.4% (MFE > MAE)
- **R:R Promedio:** 2.21

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (11 oportunidades)

- **WR Teórico:** 36.4% (si se hubieran ejecutado)
- **TP_FIRST:** 36.4% (4 de 11)
- **SL_FIRST:** 63.6% (7 de 11)
- **MFE Promedio:** 116.44 pts
- **MAE Promedio:** 84.96 pts
- **MFE/MAE Ratio:** 0.00
- **Good Entries:** 36.4% (MFE > MAE)
- **R:R Promedio:** 3.10

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 42 | 465 | 247 | 323 |
| **TP_FIRST %** | 45.2% | 57.8% | 55.5% | 59.8% |
| **Good Entries %** | 50.0% | 42.2% | 51.4% | 62.8% |
| **MFE/MAE Ratio** | 26.82 | 3.40 | 2.60 | 2.45 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 323 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 69.3%
   - Good Entries: 62.8%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 14.61 pts
- **Mediana:** 9.87 pts
- **Min/Max:** 2.34 / 41.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 9.87 |
| P70 | 21.55 |
| P80 | 24.25 |
| P90 | 33.97 |
| P95 | 37.24 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 26.40 pts
- **Mediana:** 18.62 pts
- **Min/Max:** 4.50 / 54.75 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 18.62 |
| P70 | 37.62 |
| P80 | 49.00 |
| P90 | 51.50 |
| P95 | 53.27 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 33; // Era 60
public int MaxTPDistancePoints { get; set; } = 51; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 34.0pts, TP: 51.5pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (38.1%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.381) / 0.381
R:R_min = 1.63
```

**Estado actual:** R:R promedio = 2.05
**Gap:** -0.43 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **33** (P90 real)
2. **MaxTPDistancePoints:** 120 → **51** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.05) < R:R mínimo (1.63)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=38.1%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-14 18:16:21*