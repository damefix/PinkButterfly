# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-12 16:04:27
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_155538.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251112_155538.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 36
- **Win Rate:** 33.3% (12/36)
- **Profit Factor:** 0.78
- **Avg R:R Planeado:** 2.34
- **R:R Mínimo para Break-Even:** 2.00

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 40 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.34
   - R:R necesario: 2.00
   - **Gap:** -0.34

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8093 | 34.7% |
| Bullish | 8967 | 38.4% |
| Bearish | 6292 | 26.9% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.078
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
- **Consecuencia:** Sistema queda 34.7% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.7%)

**Posibles causas:**
- **BOS Score bajo (0.009):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio 0.078 indica poca señal direccional
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
| StructureFusion | 10192 | 100.0% | 100.0% |
| ProximityAnalyzer | 2161 | 21.2% | 21.2% |
| DFM_Evaluated | 761 | 35.2% | 7.5% |
| DFM_Passed | 725 | 95.3% | 7.1% |
| RiskCalculator | 3328 | 459.0% | 32.7% |
| Risk_Accepted | 76 | 2.3% | 0.7% |
| TradeManager | 36 | 47.4% | 0.4% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 3252 señales, -97.7%)
- **Tasa de conversión final:** 0.35% (de 10192 zonas iniciales → 36 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 685 | 54.7% |
| ENTRY_TOO_FAR | 237 | 18.9% |
| NO_SL | 213 | 17.0% |
| TP_CHECK_FAIL | 117 | 9.3% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (685 rechazos, 54.7%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 1513 | 91.3% |
| P0_ANY_DIR | 144 | 8.7% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 44.24 pts (máxima ganancia flotante)
- **MAE Promedio:** 39.01 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 6.85

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 17 | 47.2% |
| SL_FIRST (precio fue hacia SL) | 17 | 47.2% |
| NEUTRAL (sin dirección clara) | 2 | 5.6% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 47.2%
- **Entradas Malas (MAE > MFE):** 52.8%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 36 | 17 | 17 | 47.2% | 44.24 | 39.01 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0003 | SELL | 29.75 | 21.25 | 1.40 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0007 | BUY | 1.75 | 11.00 | 0.16 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0008 | BUY | 4.00 | 37.00 | 0.11 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0009 | BUY | 0.00 | 39.00 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0013 | BUY | 15.75 | 149.50 | 0.11 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0015 | SELL | 245.00 | 21.25 | 11.53 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0016 | SELL | 248.50 | 11.25 | 22.09 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0016_2 | SELL | 248.50 | 11.25 | 22.09 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0018 | SELL | 0.00 | 68.50 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0019 | SELL | 33.75 | 28.00 | 1.21 | SL_FIRST | CLOSED | 👍 Entrada correcta |
| T0020 | BUY | 44.50 | 31.75 | 1.40 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0021 | BUY | 29.25 | 6.25 | 4.68 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0022 | BUY | 38.50 | 98.75 | 0.39 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0024 | SELL | 27.25 | 80.00 | 0.34 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0032 | BUY | 15.50 | 15.50 | 1.00 | SL_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0035 | BUY | 12.00 | 12.00 | 1.00 | SL_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0036 | BUY | 13.50 | 9.75 | 1.38 | SL_FIRST | CLOSED | 👍 Entrada correcta |
| T0038 | BUY | 7.00 | 84.00 | 0.08 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0040 | SELL | 26.75 | 164.00 | 0.16 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0043 | BUY | 28.25 | 7.50 | 3.77 | SL_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 765

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 281 | 36.7% | 60.5% | 3.89 | 43.8% | 2.00 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 163 | 49.7% | 57.7% | 1.85 | 44.2% | 1.79 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 208 | 69.2% | 57.7% | 1.32 | 61.1% | 2.00 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 107 | 61.7% | 52.3% | 0.86 | 59.8% | 2.22 | ⚠️ CALIDAD MEDIA - Revisar |
| >10.0 ATR (Muy lejos) | 6 | 33.3% | 0.0% | 0.61 | 33.3% | 2.56 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (281 oportunidades)

- **WR Teórico:** 36.7% (si se hubieran ejecutado)
- **TP_FIRST:** 60.5% (170 de 281)
- **SL_FIRST:** 37.7% (106 de 281)
- **MFE Promedio:** 41.36 pts
- **MAE Promedio:** 44.50 pts
- **MFE/MAE Ratio:** 3.89
- **Good Entries:** 43.8% (MFE > MAE)
- **R:R Promedio:** 2.00

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (163 oportunidades)

- **WR Teórico:** 49.7% (si se hubieran ejecutado)
- **TP_FIRST:** 57.7% (94 de 163)
- **SL_FIRST:** 41.7% (68 de 163)
- **MFE Promedio:** 41.45 pts
- **MAE Promedio:** 41.32 pts
- **MFE/MAE Ratio:** 1.85
- **Good Entries:** 44.2% (MFE > MAE)
- **R:R Promedio:** 1.79

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (208 oportunidades)

- **WR Teórico:** 69.2% (si se hubieran ejecutado)
- **TP_FIRST:** 57.7% (120 de 208)
- **SL_FIRST:** 42.3% (88 de 208)
- **MFE Promedio:** 82.48 pts
- **MAE Promedio:** 46.21 pts
- **MFE/MAE Ratio:** 1.32
- **Good Entries:** 61.1% (MFE > MAE)
- **R:R Promedio:** 2.00

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (107 oportunidades)

- **WR Teórico:** 61.7% (si se hubieran ejecutado)
- **TP_FIRST:** 52.3% (56 de 107)
- **SL_FIRST:** 45.8% (49 de 107)
- **MFE Promedio:** 99.28 pts
- **MAE Promedio:** 64.07 pts
- **MFE/MAE Ratio:** 0.86
- **Good Entries:** 59.8% (MFE > MAE)
- **R:R Promedio:** 2.22

**⚠️ CALIDAD MEDIA - Revisar**

**>10.0 ATR (Muy lejos)** (6 oportunidades)

- **WR Teórico:** 33.3% (si se hubieran ejecutado)
- **TP_FIRST:** 0.0% (0 de 6)
- **SL_FIRST:** 100.0% (6 de 6)
- **MFE Promedio:** 43.25 pts
- **MAE Promedio:** 58.71 pts
- **MFE/MAE Ratio:** 0.61
- **Good Entries:** 33.3% (MFE > MAE)
- **R:R Promedio:** 2.56

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 36 | 281 | 163 | 208 |
| **TP_FIRST %** | 47.2% | 60.5% | 57.7% | 57.7% |
| **Good Entries %** | 47.2% | 43.8% | 44.2% | 61.1% |
| **MFE/MAE Ratio** | 6.85 | 3.89 | 1.85 | 1.32 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 208 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 69.2%
   - Good Entries: 61.1%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 13.34 pts
- **Mediana:** 10.41 pts
- **Min/Max:** 1.67 / 40.44 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 10.40 |
| P70 | 17.07 |
| P80 | 19.48 |
| P90 | 25.73 |
| P95 | 37.95 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 24.24 pts
- **Mediana:** 17.88 pts
- **Min/Max:** 6.75 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 17.88 |
| P70 | 29.65 |
| P80 | 44.45 |
| P90 | 50.00 |
| P95 | 53.29 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 25; // Era 60
public int MaxTPDistancePoints { get; set; } = 50; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 25.7pts, TP: 50.0pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (33.3%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.333) / 0.333
R:R_min = 2.00
```

**Estado actual:** R:R promedio = 2.34
**Gap:** -0.34 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **25** (P90 real)
2. **MaxTPDistancePoints:** 120 → **50** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.34) < R:R mínimo (2.00)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=33.3%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-12 16:04:27*