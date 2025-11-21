# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-13 11:08:58
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_110431.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251113_110431.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 44
- **Win Rate:** 27.3% (12/44)
- **Profit Factor:** 0.47
- **Avg R:R Planeado:** 1.98
- **R:R Mínimo para Break-Even:** 2.67

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 38 puntos
   - TP máximo observado: 56 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.98
   - R:R necesario: 2.67
   - **Gap:** 0.68

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8037 | 34.4% |
| Bearish | 6272 | 26.9% |
| Bullish | 9023 | 38.7% |

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
| StructureFusion | 10274 | 100.0% | 100.0% |
| ProximityAnalyzer | 4128 | 40.2% | 40.2% |
| DFM_Evaluated | 828 | 20.1% | 8.1% |
| DFM_Passed | 828 | 100.0% | 8.1% |
| RiskCalculator | 6313 | 762.4% | 61.4% |
| Risk_Accepted | 1 | 0.0% | 0.0% |
| TradeManager | 44 | 4400.0% | 0.4% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 6312 señales, -100.0%)
- **Tasa de conversión final:** 0.43% (de 10274 zonas iniciales → 44 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,654 | 58.9% |
| NO_SL | 522 | 18.6% |
| ENTRY_TOO_FAR | 381 | 13.6% |
| TP_CHECK_FAIL | 253 | 9.0% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,654 rechazos, 58.9%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2537 | 89.4% |
| P0_ANY_DIR | 300 | 10.6% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 45.03 pts (máxima ganancia flotante)
- **MAE Promedio:** 37.49 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 47.54

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 26 | 59.1% |
| SL_FIRST (precio fue hacia SL) | 18 | 40.9% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 36.4%
- **Entradas Malas (MAE > MFE):** 63.6%

⚠️ **ALERTA:** >60% de entradas tienen MAE > MFE
- **Problema:** El precio va más en contra que a favor antes del cierre
- **Causas posibles:**
  1. Timing incorrecto (entramos antes de reversión)
  2. Bias desincronizado (operamos contra tendencia real)
  3. Zonas de baja calidad (sin confluence real)

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 44 | 26 | 18 | 59.1% | 45.03 | 37.49 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | SELL | 22.00 | 21.75 | 1.01 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0003 | BUY | 21.25 | 26.75 | 0.79 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0006 | SELL | 12.75 | 19.00 | 0.67 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0008 | SELL | 0.00 | 38.00 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0011 | BUY | 1.75 | 21.25 | 0.08 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0011_2 | BUY | 1.75 | 21.25 | 0.08 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0013 | SELL | 0.75 | 13.00 | 0.06 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0013_2 | SELL | 0.75 | 13.00 | 0.06 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0016 | SELL | 245.00 | 21.25 | 11.53 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0017 | SELL | 244.50 | 21.75 | 11.24 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0017_2 | SELL | 244.50 | 21.75 | 11.24 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0017_3 | SELL | 244.50 | 21.75 | 11.24 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0022 | SELL | 2.75 | 67.00 | 0.04 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0024 | SELL | 3.25 | 62.25 | 0.05 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0028 | BUY | 65.75 | 24.75 | 2.66 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0029 | BUY | 13.00 | 105.50 | 0.12 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0030 | SELL | 129.25 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0031 | SELL | 55.25 | 58.00 | 0.95 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0034 | SELL | 36.50 | 75.00 | 0.49 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0034_2 | SELL | 36.50 | 75.00 | 0.49 | TP_FIRST | CLOSED | ❌ Entrada muy mala |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,305

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 470 | 36.0% | 56.2% | 3.94 | 43.6% | 2.07 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 252 | 54.0% | 56.0% | 2.17 | 52.0% | 2.20 | ⚠️ CALIDAD MEDIA - Revisar |
| 3.0-5.0 ATR (Media) | 367 | 67.8% | 51.5% | 2.15 | 59.1% | 2.12 | ⚠️ CALIDAD MEDIA - Revisar |
| 5.0-10.0 ATR (Lejos) | 198 | 70.2% | 60.6% | 1.40 | 65.7% | 2.25 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 18 | 72.2% | 61.1% | 0.17 | 61.1% | 3.19 | ✅ BUENA CALIDAD - Considerar incluir |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (470 oportunidades)

- **WR Teórico:** 36.0% (si se hubieran ejecutado)
- **TP_FIRST:** 56.2% (264 de 470)
- **SL_FIRST:** 40.6% (191 de 470)
- **MFE Promedio:** 44.02 pts
- **MAE Promedio:** 40.86 pts
- **MFE/MAE Ratio:** 3.94
- **Good Entries:** 43.6% (MFE > MAE)
- **R:R Promedio:** 2.07

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (252 oportunidades)

- **WR Teórico:** 54.0% (si se hubieran ejecutado)
- **TP_FIRST:** 56.0% (141 de 252)
- **SL_FIRST:** 43.7% (110 de 252)
- **MFE Promedio:** 52.45 pts
- **MAE Promedio:** 41.58 pts
- **MFE/MAE Ratio:** 2.17
- **Good Entries:** 52.0% (MFE > MAE)
- **R:R Promedio:** 2.20

**⚠️ CALIDAD MEDIA - Revisar**

**3.0-5.0 ATR (Media)** (367 oportunidades)

- **WR Teórico:** 67.8% (si se hubieran ejecutado)
- **TP_FIRST:** 51.5% (189 de 367)
- **SL_FIRST:** 48.2% (177 de 367)
- **MFE Promedio:** 72.12 pts
- **MAE Promedio:** 43.03 pts
- **MFE/MAE Ratio:** 2.15
- **Good Entries:** 59.1% (MFE > MAE)
- **R:R Promedio:** 2.12

**⚠️ CALIDAD MEDIA - Revisar**

**5.0-10.0 ATR (Lejos)** (198 oportunidades)

- **WR Teórico:** 70.2% (si se hubieran ejecutado)
- **TP_FIRST:** 60.6% (120 de 198)
- **SL_FIRST:** 38.4% (76 de 198)
- **MFE Promedio:** 74.72 pts
- **MAE Promedio:** 44.36 pts
- **MFE/MAE Ratio:** 1.40
- **Good Entries:** 65.7% (MFE > MAE)
- **R:R Promedio:** 2.25

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (18 oportunidades)

- **WR Teórico:** 72.2% (si se hubieran ejecutado)
- **TP_FIRST:** 61.1% (11 de 18)
- **SL_FIRST:** 38.9% (7 de 18)
- **MFE Promedio:** 87.46 pts
- **MAE Promedio:** 68.11 pts
- **MFE/MAE Ratio:** 0.17
- **Good Entries:** 61.1% (MFE > MAE)
- **R:R Promedio:** 3.19

**✅ BUENA CALIDAD - Considerar incluir**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 44 | 470 | 252 | 367 |
| **TP_FIRST %** | 59.1% | 56.2% | 56.0% | 51.5% |
| **Good Entries %** | 36.4% | 43.6% | 52.0% | 59.1% |
| **MFE/MAE Ratio** | 47.54 | 3.94 | 2.17 | 2.15 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 367 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 67.8%
   - Good Entries: 59.1%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 13.79 pts
- **Mediana:** 11.56 pts
- **Min/Max:** 0.80 / 37.61 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 11.56 |
| P70 | 18.49 |
| P80 | 20.88 |
| P90 | 26.57 |
| P95 | 36.75 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 24.51 pts
- **Mediana:** 23.25 pts
- **Min/Max:** 4.00 / 55.75 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 23.25 |
| P70 | 35.75 |
| P80 | 42.50 |
| P90 | 50.12 |
| P95 | 54.25 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 26; // Era 60
public int MaxTPDistancePoints { get; set; } = 50; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 26.6pts, TP: 50.1pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (27.3%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.273) / 0.273
R:R_min = 2.67
```

**Estado actual:** R:R promedio = 1.98
**Gap:** 0.68 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **26** (P90 real)
2. **MaxTPDistancePoints:** 120 → **50** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.98) < R:R mínimo (2.67)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=27.3%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-13 11:08:58*