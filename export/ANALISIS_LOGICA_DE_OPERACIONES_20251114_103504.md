# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-14 10:40:51
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_103504.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251114_103504.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 38
- **Win Rate:** 26.3% (10/38)
- **Profit Factor:** 0.78
- **Avg R:R Planeado:** 2.14
- **R:R Mínimo para Break-Even:** 2.80

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 41 puntos
   - TP máximo observado: 55 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.14
   - R:R necesario: 2.80
   - **Gap:** 0.66

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 7962 | 34.1% |
| Bearish | 6331 | 27.1% |
| Bullish | 9037 | 38.7% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.079
- **Score Min/Max:** [-0.990, 0.970]
- **Componentes (promedio):**
  - EMA20 Slope: 0.040
  - EMA50 Cross: 0.190
  - BOS Count: 0.008
  - Regression 24h: 0.088

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.970 (apenas supera threshold)
- Score mínimo observado: -0.990 (apenas supera threshold)
- **Consecuencia:** Sistema queda 34.1% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.1%)

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
| StructureFusion | 10417 | 100.0% | 100.0% |
| ProximityAnalyzer | 3347 | 32.1% | 32.1% |
| DFM_Evaluated | 771 | 23.0% | 7.4% |
| DFM_Passed | 771 | 100.0% | 7.4% |
| RiskCalculator | 5854 | 759.3% | 56.2% |
| Risk_Accepted | 2 | 0.0% | 0.0% |
| TradeManager | 38 | 1900.0% | 0.4% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 5852 señales, -100.0%)
- **Tasa de conversión final:** 0.36% (de 10417 zonas iniciales → 38 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,294 | 51.4% |
| NO_SL | 648 | 25.7% |
| ENTRY_TOO_FAR | 366 | 14.5% |
| TP_CHECK_FAIL | 210 | 8.3% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,294 rechazos, 51.4%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2507 | 89.8% |
| P0_ANY_DIR | 286 | 10.2% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 32.89 pts (máxima ganancia flotante)
- **MAE Promedio:** 30.95 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 28.54

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 19 | 50.0% |
| SL_FIRST (precio fue hacia SL) | 18 | 47.4% |
| NEUTRAL (sin dirección clara) | 1 | 2.6% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 34.2%
- **Entradas Malas (MAE > MFE):** 65.8%

⚠️ **ALERTA:** >60% de entradas tienen MAE > MFE
- **Problema:** El precio va más en contra que a favor antes del cierre
- **Causas posibles:**
  1. Timing incorrecto (entramos antes de reversión)
  2. Bias desincronizado (operamos contra tendencia real)
  3. Zonas de baja calidad (sin confluence real)

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 38 | 19 | 18 | 50.0% | 32.89 | 30.95 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0002 | SELL | 2.75 | 32.00 | 0.09 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0004 | SELL | 8.75 | 29.75 | 0.29 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0009 | BUY | 2.50 | 11.25 | 0.22 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0009_2 | BUY | 2.50 | 11.25 | 0.22 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0009_3 | BUY | 2.50 | 11.25 | 0.22 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0009_4 | BUY | 2.50 | 11.25 | 0.22 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0010 | SELL | 1.75 | 13.00 | 0.13 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0011 | SELL | 144.75 | 20.50 | 7.06 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0012 | SELL | 245.50 | 14.25 | 17.23 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0015 | SELL | 86.75 | 10.00 | 8.68 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0017 | SELL | 1.25 | 68.50 | 0.02 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0017_2 | SELL | 1.25 | 68.50 | 0.02 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0019 | SELL | 10.50 | 46.50 | 0.23 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0020 | BUY | 48.00 | 63.75 | 0.75 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0022 | SELL | 0.00 | 40.75 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0024 | SELL | 55.25 | 58.00 | 0.95 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0025 | SELL | 23.75 | 102.25 | 0.23 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0026 | SELL | 17.00 | 63.25 | 0.27 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0033 | SELL | 14.50 | 19.75 | 0.73 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0034 | BUY | 10.75 | 13.00 | 0.83 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,254

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 461 | 26.9% | 52.5% | 2.62 | 37.3% | 2.03 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 244 | 53.3% | 50.4% | 2.90 | 45.1% | 1.89 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 326 | 64.1% | 55.5% | 3.62 | 56.4% | 2.02 | ⚠️ CALIDAD MEDIA - Revisar |
| 5.0-10.0 ATR (Lejos) | 212 | 71.7% | 68.9% | 0.58 | 68.9% | 2.29 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 11 | 36.4% | 36.4% | 0.00 | 36.4% | 3.15 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (461 oportunidades)

- **WR Teórico:** 26.9% (si se hubieran ejecutado)
- **TP_FIRST:** 52.5% (242 de 461)
- **SL_FIRST:** 42.7% (197 de 461)
- **MFE Promedio:** 39.58 pts
- **MAE Promedio:** 40.40 pts
- **MFE/MAE Ratio:** 2.62
- **Good Entries:** 37.3% (MFE > MAE)
- **R:R Promedio:** 2.03

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (244 oportunidades)

- **WR Teórico:** 53.3% (si se hubieran ejecutado)
- **TP_FIRST:** 50.4% (123 de 244)
- **SL_FIRST:** 49.2% (120 de 244)
- **MFE Promedio:** 58.08 pts
- **MAE Promedio:** 39.26 pts
- **MFE/MAE Ratio:** 2.90
- **Good Entries:** 45.1% (MFE > MAE)
- **R:R Promedio:** 1.89

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (326 oportunidades)

- **WR Teórico:** 64.1% (si se hubieran ejecutado)
- **TP_FIRST:** 55.5% (181 de 326)
- **SL_FIRST:** 41.7% (136 de 326)
- **MFE Promedio:** 75.68 pts
- **MAE Promedio:** 41.52 pts
- **MFE/MAE Ratio:** 3.62
- **Good Entries:** 56.4% (MFE > MAE)
- **R:R Promedio:** 2.02

**⚠️ CALIDAD MEDIA - Revisar**

**5.0-10.0 ATR (Lejos)** (212 oportunidades)

- **WR Teórico:** 71.7% (si se hubieran ejecutado)
- **TP_FIRST:** 68.9% (146 de 212)
- **SL_FIRST:** 29.2% (62 de 212)
- **MFE Promedio:** 89.87 pts
- **MAE Promedio:** 51.44 pts
- **MFE/MAE Ratio:** 0.58
- **Good Entries:** 68.9% (MFE > MAE)
- **R:R Promedio:** 2.29

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (11 oportunidades)

- **WR Teórico:** 36.4% (si se hubieran ejecutado)
- **TP_FIRST:** 36.4% (4 de 11)
- **SL_FIRST:** 63.6% (7 de 11)
- **MFE Promedio:** 116.75 pts
- **MAE Promedio:** 69.39 pts
- **MFE/MAE Ratio:** 0.00
- **Good Entries:** 36.4% (MFE > MAE)
- **R:R Promedio:** 3.15

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 38 | 461 | 244 | 326 |
| **TP_FIRST %** | 50.0% | 52.5% | 50.4% | 55.5% |
| **Good Entries %** | 34.2% | 37.3% | 45.1% | 56.4% |
| **MFE/MAE Ratio** | 28.54 | 2.62 | 2.90 | 3.62 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 326 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 64.1%
   - Good Entries: 56.4%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 12.40 pts
- **Mediana:** 8.10 pts
- **Min/Max:** 2.19 / 41.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 8.10 |
| P70 | 18.35 |
| P80 | 22.02 |
| P90 | 24.74 |
| P95 | 35.97 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 23.62 pts
- **Mediana:** 18.50 pts
- **Min/Max:** 4.50 / 54.75 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 18.50 |
| P70 | 33.73 |
| P80 | 39.70 |
| P90 | 49.30 |
| P95 | 53.56 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 24; // Era 60
public int MaxTPDistancePoints { get; set; } = 49; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 24.7pts, TP: 49.3pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (26.3%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.263) / 0.263
R:R_min = 2.80
```

**Estado actual:** R:R promedio = 2.14
**Gap:** 0.66 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **24** (P90 real)
2. **MaxTPDistancePoints:** 120 → **49** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.14) < R:R mínimo (2.80)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=26.3%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-14 10:40:51*