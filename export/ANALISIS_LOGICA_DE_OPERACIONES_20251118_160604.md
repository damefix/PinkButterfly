# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-18 16:10:55
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251118_160604.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251118_160604.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 11
- **Win Rate:** 27.3% (3/11)
- **Profit Factor:** 0.85
- **Avg R:R Planeado:** 2.02
- **R:R Mínimo para Break-Even:** 2.67

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 26 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.02
   - R:R necesario: 2.67
   - **Gap:** 0.64

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bearish | 200 | 31.9% |
| Neutral | 235 | 37.5% |
| Bullish | 191 | 30.5% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** -0.027
- **Score Min/Max:** [-0.980, 0.920]
- **Componentes (promedio):**
  - EMA20 Slope: -0.027
  - EMA50 Cross: -0.067
  - BOS Count: -0.013
  - Regression 24h: 0.007

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.920 (apenas supera threshold)
- Score mínimo observado: -0.980 (apenas supera threshold)
- **Consecuencia:** Sistema queda 37.5% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (37.5%)

**Posibles causas:**
- **BOS Score bajo (-0.013):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio -0.027 indica poca señal direccional
- **Mercado lateral:** Scores reales [-0.98, 0.92] muy cercanos a 0

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
| StructureFusion | 3202 | 100.0% | 100.0% |
| ProximityAnalyzer | 958 | 29.9% | 29.9% |
| DFM_Evaluated | 202 | 21.1% | 6.3% |
| DFM_Passed | 202 | 100.0% | 6.3% |
| RiskCalculator | 1890 | 935.6% | 59.0% |
| Risk_Accepted | 2 | 0.1% | 0.1% |
| TradeManager | 11 | 550.0% | 0.3% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 1888 señales, -99.9%)
- **Tasa de conversión final:** 0.34% (de 3202 zonas iniciales → 11 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 498 | 49.8% |
| NO_SL | 332 | 33.2% |
| ENTRY_TOO_FAR | 146 | 14.6% |
| TP_CHECK_FAIL | 23 | 2.3% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (498 rechazos, 49.8%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 683 | 89.4% |
| P0_ANY_DIR | 81 | 10.6% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 41.45 pts (máxima ganancia flotante)
- **MAE Promedio:** 23.55 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 96.54

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 5 | 45.5% |
| SL_FIRST (precio fue hacia SL) | 5 | 45.5% |
| NEUTRAL (sin dirección clara) | 1 | 9.1% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 63.6%
- **Entradas Malas (MAE > MFE):** 36.4%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 11 | 5 | 5 | 45.5% | 41.45 | 23.55 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0005 | SELL | 16.50 | 5.50 | 3.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0007 | BUY | 16.00 | 6.25 | 2.56 | NEUTRAL | CLOSED | ✅ Entrada excelente |
| T0008 | SELL | 21.00 | 11.25 | 1.87 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0010 | SELL | 187.25 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0012 | SELL | 0.00 | 103.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0013 | SELL | 18.75 | 31.00 | 0.60 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0015 | SELL | 2.75 | 34.50 | 0.08 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0015_2 | SELL | 2.75 | 34.50 | 0.08 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0017 | SELL | 45.00 | 27.25 | 1.65 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0022 | SELL | 73.00 | 2.75 | 26.55 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0022_2 | SELL | 73.00 | 2.75 | 26.55 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 336

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 119 | 27.7% | 62.2% | 2.83 | 52.1% | 2.46 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 68 | 51.5% | 57.4% | 1.52 | 55.9% | 2.34 | ⚠️ CALIDAD MEDIA - Revisar |
| 3.0-5.0 ATR (Media) | 101 | 75.2% | 67.3% | 4.77 | 69.3% | 2.48 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 48 | 85.4% | 87.5% | 2.05 | 85.4% | 2.64 | ✅ BUENA CALIDAD - Considerar incluir |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (119 oportunidades)

- **WR Teórico:** 27.7% (si se hubieran ejecutado)
- **TP_FIRST:** 62.2% (74 de 119)
- **SL_FIRST:** 35.3% (42 de 119)
- **MFE Promedio:** 48.03 pts
- **MAE Promedio:** 36.87 pts
- **MFE/MAE Ratio:** 2.83
- **Good Entries:** 52.1% (MFE > MAE)
- **R:R Promedio:** 2.46

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (68 oportunidades)

- **WR Teórico:** 51.5% (si se hubieran ejecutado)
- **TP_FIRST:** 57.4% (39 de 68)
- **SL_FIRST:** 42.6% (29 de 68)
- **MFE Promedio:** 62.12 pts
- **MAE Promedio:** 40.62 pts
- **MFE/MAE Ratio:** 1.52
- **Good Entries:** 55.9% (MFE > MAE)
- **R:R Promedio:** 2.34

**⚠️ CALIDAD MEDIA - Revisar**

**3.0-5.0 ATR (Media)** (101 oportunidades)

- **WR Teórico:** 75.2% (si se hubieran ejecutado)
- **TP_FIRST:** 67.3% (68 de 101)
- **SL_FIRST:** 31.7% (32 de 101)
- **MFE Promedio:** 82.83 pts
- **MAE Promedio:** 35.80 pts
- **MFE/MAE Ratio:** 4.77
- **Good Entries:** 69.3% (MFE > MAE)
- **R:R Promedio:** 2.48

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (48 oportunidades)

- **WR Teórico:** 85.4% (si se hubieran ejecutado)
- **TP_FIRST:** 87.5% (42 de 48)
- **SL_FIRST:** 10.4% (5 de 48)
- **MFE Promedio:** 127.70 pts
- **MAE Promedio:** 38.45 pts
- **MFE/MAE Ratio:** 2.05
- **Good Entries:** 85.4% (MFE > MAE)
- **R:R Promedio:** 2.64

**✅ BUENA CALIDAD - Considerar incluir**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 11 | 119 | 68 | 101 |
| **TP_FIRST %** | 45.5% | 62.2% | 57.4% | 67.3% |
| **Good Entries %** | 63.6% | 52.1% | 55.9% | 69.3% |
| **MFE/MAE Ratio** | 96.54 | 2.83 | 1.52 | 4.77 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 101 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 75.2%
   - Good Entries: 69.3%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 14.93 pts
- **Mediana:** 11.92 pts
- **Min/Max:** 5.14 / 26.24 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 11.92 |
| P70 | 18.91 |
| P80 | 19.69 |
| P90 | 25.02 |
| P95 | 28.68 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 28.86 pts
- **Mediana:** 25.00 pts
- **Min/Max:** 11.50 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 25.00 |
| P70 | 37.55 |
| P80 | 38.75 |
| P90 | 50.55 |
| P95 | 59.40 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 25; // Era 60
public int MaxTPDistancePoints { get; set; } = 50; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 25.0pts, TP: 50.5pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (27.3%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.273) / 0.273
R:R_min = 2.67
```

**Estado actual:** R:R promedio = 2.02
**Gap:** 0.64 (necesitas mejorar R:R)

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

**Problema:** R:R actual (2.02) < R:R mínimo (2.67)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=27.3%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-18 16:10:55*