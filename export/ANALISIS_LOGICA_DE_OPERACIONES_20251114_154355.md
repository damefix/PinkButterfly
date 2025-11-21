# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-14 15:50:26
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_154355.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251114_154355.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 40
- **Win Rate:** 37.5% (15/40)
- **Profit Factor:** 1.26
- **Avg R:R Planeado:** 2.08
- **R:R Mínimo para Break-Even:** 1.67

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 41 puntos
   - TP máximo observado: 55 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.08
   - R:R necesario: 1.67
   - **Gap:** -0.41

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 7951 | 34.1% |
| Bearish | 6355 | 27.2% |
| Bullish | 9044 | 38.7% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.079
- **Score Min/Max:** [-0.990, 0.970]
- **Componentes (promedio):**
  - EMA20 Slope: 0.039
  - EMA50 Cross: 0.189
  - BOS Count: 0.008
  - Regression 24h: 0.087

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
| StructureFusion | 10450 | 100.0% | 100.0% |
| ProximityAnalyzer | 3276 | 31.3% | 31.3% |
| DFM_Evaluated | 767 | 23.4% | 7.3% |
| DFM_Passed | 767 | 100.0% | 7.3% |
| RiskCalculator | 6024 | 785.4% | 57.6% |
| Risk_Accepted | 1 | 0.0% | 0.0% |
| TradeManager | 40 | 4000.0% | 0.4% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 6023 señales, -100.0%)
- **Tasa de conversión final:** 0.38% (de 10450 zonas iniciales → 40 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,283 | 48.4% |
| NO_SL | 754 | 28.5% |
| ENTRY_TOO_FAR | 404 | 15.3% |
| TP_CHECK_FAIL | 208 | 7.9% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,283 rechazos, 48.4%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2570 | 89.1% |
| P0_ANY_DIR | 316 | 10.9% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 40.27 pts (máxima ganancia flotante)
- **MAE Promedio:** 28.00 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 3.19

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 17 | 42.5% |
| SL_FIRST (precio fue hacia SL) | 20 | 50.0% |
| NEUTRAL (sin dirección clara) | 3 | 7.5% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 50.0%
- **Entradas Malas (MAE > MFE):** 50.0%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 40 | 17 | 20 | 42.5% | 40.27 | 28.00 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0004 | BUY | 2.50 | 11.25 | 0.22 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0004_2 | BUY | 2.50 | 11.25 | 0.22 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0004_3 | BUY | 2.50 | 11.25 | 0.22 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0005 | SELL | 1.75 | 13.00 | 0.13 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0006 | SELL | 144.75 | 20.50 | 7.06 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0007 | SELL | 245.50 | 14.25 | 17.23 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0010 | SELL | 86.75 | 10.00 | 8.68 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0012 | SELL | 1.25 | 68.50 | 0.02 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0012_2 | SELL | 1.25 | 68.50 | 0.02 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0014 | SELL | 10.50 | 46.50 | 0.23 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0016 | SELL | 0.00 | 40.75 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0018 | SELL | 55.25 | 58.00 | 0.95 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0019 | SELL | 23.75 | 102.25 | 0.23 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0020 | SELL | 17.00 | 63.25 | 0.27 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0026 | SELL | 14.50 | 19.75 | 0.73 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0027 | SELL | 12.75 | 11.75 | 1.09 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0028 | SELL | 82.25 | 9.75 | 8.44 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0029 | SELL | 81.75 | 7.75 | 10.55 | NEUTRAL | CLOSED | ✅ Entrada excelente |
| T0033 | SELL | 0.00 | 37.00 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0034 | BUY | 44.25 | 7.50 | 5.90 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,263

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 464 | 26.7% | 58.4% | 3.09 | 43.3% | 2.01 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 246 | 51.6% | 55.7% | 2.71 | 54.5% | 1.94 | ⚠️ CALIDAD MEDIA - Revisar |
| 3.0-5.0 ATR (Media) | 324 | 69.8% | 59.9% | 2.43 | 63.0% | 2.04 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 218 | 72.9% | 72.0% | 0.61 | 72.5% | 2.26 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 11 | 36.4% | 36.4% | 0.00 | 36.4% | 3.10 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (464 oportunidades)

- **WR Teórico:** 26.7% (si se hubieran ejecutado)
- **TP_FIRST:** 58.4% (271 de 464)
- **SL_FIRST:** 38.4% (178 de 464)
- **MFE Promedio:** 44.34 pts
- **MAE Promedio:** 44.20 pts
- **MFE/MAE Ratio:** 3.09
- **Good Entries:** 43.3% (MFE > MAE)
- **R:R Promedio:** 2.01

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (246 oportunidades)

- **WR Teórico:** 51.6% (si se hubieran ejecutado)
- **TP_FIRST:** 55.7% (137 de 246)
- **SL_FIRST:** 43.5% (107 de 246)
- **MFE Promedio:** 65.19 pts
- **MAE Promedio:** 39.57 pts
- **MFE/MAE Ratio:** 2.71
- **Good Entries:** 54.5% (MFE > MAE)
- **R:R Promedio:** 1.94

**⚠️ CALIDAD MEDIA - Revisar**

**3.0-5.0 ATR (Media)** (324 oportunidades)

- **WR Teórico:** 69.8% (si se hubieran ejecutado)
- **TP_FIRST:** 59.9% (194 de 324)
- **SL_FIRST:** 39.8% (129 de 324)
- **MFE Promedio:** 82.03 pts
- **MAE Promedio:** 42.83 pts
- **MFE/MAE Ratio:** 2.43
- **Good Entries:** 63.0% (MFE > MAE)
- **R:R Promedio:** 2.04

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (218 oportunidades)

- **WR Teórico:** 72.9% (si se hubieran ejecutado)
- **TP_FIRST:** 72.0% (157 de 218)
- **SL_FIRST:** 28.0% (61 de 218)
- **MFE Promedio:** 91.30 pts
- **MAE Promedio:** 54.50 pts
- **MFE/MAE Ratio:** 0.61
- **Good Entries:** 72.5% (MFE > MAE)
- **R:R Promedio:** 2.26

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
| **Count** | 40 | 464 | 246 | 324 |
| **TP_FIRST %** | 42.5% | 58.4% | 55.7% | 59.9% |
| **Good Entries %** | 50.0% | 43.3% | 54.5% | 63.0% |
| **MFE/MAE Ratio** | 3.19 | 3.09 | 2.71 | 2.43 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 324 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 69.8%
   - Good Entries: 63.0%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 13.54 pts
- **Mediana:** 8.03 pts
- **Min/Max:** 2.34 / 41.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 8.03 |
| P70 | 20.48 |
| P80 | 22.17 |
| P90 | 34.79 |
| P95 | 37.42 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 24.62 pts
- **Mediana:** 17.62 pts
- **Min/Max:** 4.50 / 54.75 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 17.62 |
| P70 | 35.30 |
| P80 | 47.90 |
| P90 | 49.90 |
| P95 | 53.42 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 34; // Era 60
public int MaxTPDistancePoints { get; set; } = 49; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 34.8pts, TP: 49.9pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (37.5%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.375) / 0.375
R:R_min = 1.67
```

**Estado actual:** R:R promedio = 2.08
**Gap:** -0.41 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **34** (P90 real)
2. **MaxTPDistancePoints:** 120 → **49** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.08) < R:R mínimo (1.67)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=37.5%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-14 15:50:26*