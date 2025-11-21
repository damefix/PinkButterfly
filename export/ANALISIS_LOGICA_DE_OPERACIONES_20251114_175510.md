# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-14 18:02:25
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_175510.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251114_175510.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 49
- **Win Rate:** 32.7% (16/49)
- **Profit Factor:** 0.64
- **Avg R:R Planeado:** 1.98
- **R:R Mínimo para Break-Even:** 2.06

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 38 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.98
   - R:R necesario: 2.06
   - **Gap:** 0.09

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8028 | 34.1% |
| Bearish | 6377 | 27.1% |
| Bullish | 9112 | 38.7% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.079
- **Score Min/Max:** [-0.990, 0.980]
- **Componentes (promedio):**
  - EMA20 Slope: 0.039
  - EMA50 Cross: 0.194
  - BOS Count: 0.007
  - Regression 24h: 0.087

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.980 (apenas supera threshold)
- Score mínimo observado: -0.990 (apenas supera threshold)
- **Consecuencia:** Sistema queda 34.1% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.1%)

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
| StructureFusion | 10485 | 100.0% | 100.0% |
| ProximityAnalyzer | 3678 | 35.1% | 35.1% |
| DFM_Evaluated | 860 | 23.4% | 8.2% |
| DFM_Passed | 860 | 100.0% | 8.2% |
| RiskCalculator | 6133 | 713.1% | 58.5% |
| Risk_Accepted | 1 | 0.0% | 0.0% |
| TradeManager | 49 | 4900.0% | 0.5% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 6132 señales, -100.0%)
- **Tasa de conversión final:** 0.47% (de 10485 zonas iniciales → 49 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,364 | 51.3% |
| NO_SL | 661 | 24.9% |
| ENTRY_TOO_FAR | 407 | 15.3% |
| TP_CHECK_FAIL | 227 | 8.5% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,364 rechazos, 51.3%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2522 | 88.3% |
| P0_ANY_DIR | 335 | 11.7% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 46.62 pts (máxima ganancia flotante)
- **MAE Promedio:** 38.00 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 64.16

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 22 | 44.9% |
| SL_FIRST (precio fue hacia SL) | 25 | 51.0% |
| NEUTRAL (sin dirección clara) | 2 | 4.1% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 42.9%
- **Entradas Malas (MAE > MFE):** 57.1%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 49 | 22 | 25 | 44.9% | 46.62 | 38.00 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | SELL | 9.00 | 34.25 | 0.26 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0004 | BUY | 1.75 | 34.25 | 0.05 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0005 | SELL | 42.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0006 | SELL | 2.25 | 13.00 | 0.17 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0008 | SELL | 174.75 | 27.75 | 6.30 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0009 | SELL | 245.50 | 14.25 | 17.23 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0009_2 | SELL | 245.50 | 14.25 | 17.23 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0012 | SELL | 72.50 | 18.00 | 4.03 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0014 | SELL | 1.25 | 68.50 | 0.02 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0014_2 | SELL | 1.25 | 68.50 | 0.02 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0016 | SELL | 14.25 | 42.75 | 0.33 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0019 | SELL | 53.25 | 33.00 | 1.61 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0019_2 | SELL | 53.25 | 33.00 | 1.61 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0021 | SELL | 55.25 | 58.00 | 0.95 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0023 | SELL | 26.75 | 88.50 | 0.30 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0023_2 | SELL | 26.75 | 88.50 | 0.30 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0025 | SELL | 0.25 | 77.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0027 | SELL | 0.00 | 103.75 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0031 | SELL | 12.75 | 25.25 | 0.50 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0033 | BUY | 11.50 | 13.00 | 0.88 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,313

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 494 | 29.6% | 55.7% | 4.53 | 43.9% | 2.12 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 265 | 49.8% | 51.7% | 2.54 | 53.2% | 1.97 | ⚠️ CALIDAD MEDIA - Revisar |
| 3.0-5.0 ATR (Media) | 332 | 74.7% | 64.8% | 2.78 | 69.6% | 2.03 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 211 | 78.2% | 74.9% | 1.95 | 78.7% | 2.21 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 11 | 45.5% | 45.5% | 0.00 | 45.5% | 3.07 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (494 oportunidades)

- **WR Teórico:** 29.6% (si se hubieran ejecutado)
- **TP_FIRST:** 55.7% (275 de 494)
- **SL_FIRST:** 41.1% (203 de 494)
- **MFE Promedio:** 49.19 pts
- **MAE Promedio:** 44.62 pts
- **MFE/MAE Ratio:** 4.53
- **Good Entries:** 43.9% (MFE > MAE)
- **R:R Promedio:** 2.12

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (265 oportunidades)

- **WR Teórico:** 49.8% (si se hubieran ejecutado)
- **TP_FIRST:** 51.7% (137 de 265)
- **SL_FIRST:** 48.3% (128 de 265)
- **MFE Promedio:** 74.68 pts
- **MAE Promedio:** 43.33 pts
- **MFE/MAE Ratio:** 2.54
- **Good Entries:** 53.2% (MFE > MAE)
- **R:R Promedio:** 1.97

**⚠️ CALIDAD MEDIA - Revisar**

**3.0-5.0 ATR (Media)** (332 oportunidades)

- **WR Teórico:** 74.7% (si se hubieran ejecutado)
- **TP_FIRST:** 64.8% (215 de 332)
- **SL_FIRST:** 35.2% (117 de 332)
- **MFE Promedio:** 92.53 pts
- **MAE Promedio:** 45.17 pts
- **MFE/MAE Ratio:** 2.78
- **Good Entries:** 69.6% (MFE > MAE)
- **R:R Promedio:** 2.03

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (211 oportunidades)

- **WR Teórico:** 78.2% (si se hubieran ejecutado)
- **TP_FIRST:** 74.9% (158 de 211)
- **SL_FIRST:** 25.1% (53 de 211)
- **MFE Promedio:** 94.62 pts
- **MAE Promedio:** 49.49 pts
- **MFE/MAE Ratio:** 1.95
- **Good Entries:** 78.7% (MFE > MAE)
- **R:R Promedio:** 2.21

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (11 oportunidades)

- **WR Teórico:** 45.5% (si se hubieran ejecutado)
- **TP_FIRST:** 45.5% (5 de 11)
- **SL_FIRST:** 54.5% (6 de 11)
- **MFE Promedio:** 108.55 pts
- **MAE Promedio:** 80.79 pts
- **MFE/MAE Ratio:** 0.00
- **Good Entries:** 45.5% (MFE > MAE)
- **R:R Promedio:** 3.07

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 49 | 494 | 265 | 332 |
| **TP_FIRST %** | 44.9% | 55.7% | 51.7% | 64.8% |
| **Good Entries %** | 42.9% | 43.9% | 53.2% | 69.6% |
| **MFE/MAE Ratio** | 64.16 | 4.53 | 2.54 | 2.78 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 332 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 74.7%
   - Good Entries: 69.6%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 14.64 pts
- **Mediana:** 10.15 pts
- **Min/Max:** 3.10 / 37.51 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 10.15 |
| P70 | 18.84 |
| P80 | 23.31 |
| P90 | 27.68 |
| P95 | 36.79 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 26.90 pts
- **Mediana:** 26.50 pts
- **Min/Max:** 4.50 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 26.50 |
| P70 | 37.50 |
| P80 | 44.50 |
| P90 | 49.00 |
| P95 | 52.12 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 27; // Era 60
public int MaxTPDistancePoints { get; set; } = 49; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 27.7pts, TP: 49.0pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (32.7%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.327) / 0.327
R:R_min = 2.06
```

**Estado actual:** R:R promedio = 1.98
**Gap:** 0.09 (necesitas mejorar R:R)

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

**Problema:** R:R actual (1.98) < R:R mínimo (2.06)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=32.7%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-14 18:02:25*