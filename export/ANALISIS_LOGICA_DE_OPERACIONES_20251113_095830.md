# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-13 10:02:48
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_095830.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251113_095830.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 36
- **Win Rate:** 30.6% (11/36)
- **Profit Factor:** 0.98
- **Avg R:R Planeado:** 2.42
- **R:R Mínimo para Break-Even:** 2.27

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 40 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.42
   - R:R necesario: 2.27
   - **Gap:** -0.14

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8033 | 34.4% |
| Bearish | 6273 | 26.9% |
| Bullish | 9022 | 38.7% |

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
| StructureFusion | 10315 | 100.0% | 100.0% |
| ProximityAnalyzer | 4156 | 40.3% | 40.3% |
| DFM_Evaluated | 847 | 20.4% | 8.2% |
| DFM_Passed | 847 | 100.0% | 8.2% |
| RiskCalculator | 6327 | 747.0% | 61.3% |
| Risk_Accepted | 101 | 1.6% | 1.0% |
| TradeManager | 36 | 35.6% | 0.3% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 6226 señales, -98.4%)
- **Tasa de conversión final:** 0.35% (de 10315 zonas iniciales → 36 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,663 | 58.8% |
| NO_SL | 515 | 18.2% |
| ENTRY_TOO_FAR | 386 | 13.6% |
| TP_CHECK_FAIL | 265 | 9.4% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,663 rechazos, 58.8%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2551 | 88.9% |
| P0_ANY_DIR | 320 | 11.1% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 43.54 pts (máxima ganancia flotante)
- **MAE Promedio:** 29.69 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 58.45

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 25 | 69.4% |
| SL_FIRST (precio fue hacia SL) | 10 | 27.8% |
| NEUTRAL (sin dirección clara) | 1 | 2.8% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 44.4%
- **Entradas Malas (MAE > MFE):** 55.6%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 36 | 25 | 10 | 69.4% | 43.54 | 29.69 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0002 | SELL | 9.50 | 19.25 | 0.49 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0002_2 | SELL | 9.50 | 19.25 | 0.49 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0004 | BUY | 21.25 | 26.75 | 0.79 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0007 | SELL | 13.00 | 18.75 | 0.69 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0011 | BUY | 1.75 | 11.00 | 0.16 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0013 | BUY | 14.50 | 26.50 | 0.55 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0014 | SELL | 42.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0015 | SELL | 2.25 | 13.00 | 0.17 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0017 | SELL | 166.00 | 14.00 | 11.86 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0017_2 | SELL | 166.00 | 14.00 | 11.86 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0018 | SELL | 248.50 | 11.25 | 22.09 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0022 | SELL | 0.00 | 68.50 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0023 | SELL | 33.75 | 18.00 | 1.88 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0027 | BUY | 51.50 | 63.75 | 0.81 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0028 | BUY | 9.50 | 66.75 | 0.14 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0032 | SELL | 55.25 | 58.00 | 0.95 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0033 | SELL | 36.00 | 75.50 | 0.48 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0042 | BUY | 15.25 | 8.25 | 1.85 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0044 | SELL | 83.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0045 | SELL | 26.75 | 40.75 | 0.66 | TP_FIRST | CLOSED | ❌ Entrada muy mala |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,303

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 483 | 38.5% | 59.6% | 3.93 | 45.5% | 2.01 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 249 | 50.2% | 52.2% | 1.64 | 47.8% | 1.91 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 360 | 69.2% | 56.9% | 2.91 | 60.8% | 2.05 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 197 | 69.5% | 65.5% | 0.88 | 67.5% | 2.32 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 14 | 57.1% | 42.9% | 0.15 | 42.9% | 3.17 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (483 oportunidades)

- **WR Teórico:** 38.5% (si se hubieran ejecutado)
- **TP_FIRST:** 59.6% (288 de 483)
- **SL_FIRST:** 38.1% (184 de 483)
- **MFE Promedio:** 49.30 pts
- **MAE Promedio:** 38.81 pts
- **MFE/MAE Ratio:** 3.93
- **Good Entries:** 45.5% (MFE > MAE)
- **R:R Promedio:** 2.01

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (249 oportunidades)

- **WR Teórico:** 50.2% (si se hubieran ejecutado)
- **TP_FIRST:** 52.2% (130 de 249)
- **SL_FIRST:** 47.8% (119 de 249)
- **MFE Promedio:** 57.10 pts
- **MAE Promedio:** 42.44 pts
- **MFE/MAE Ratio:** 1.64
- **Good Entries:** 47.8% (MFE > MAE)
- **R:R Promedio:** 1.91

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (360 oportunidades)

- **WR Teórico:** 69.2% (si se hubieran ejecutado)
- **TP_FIRST:** 56.9% (205 de 360)
- **SL_FIRST:** 43.1% (155 de 360)
- **MFE Promedio:** 70.35 pts
- **MAE Promedio:** 42.93 pts
- **MFE/MAE Ratio:** 2.91
- **Good Entries:** 60.8% (MFE > MAE)
- **R:R Promedio:** 2.05

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (197 oportunidades)

- **WR Teórico:** 69.5% (si se hubieran ejecutado)
- **TP_FIRST:** 65.5% (129 de 197)
- **SL_FIRST:** 34.5% (68 de 197)
- **MFE Promedio:** 80.50 pts
- **MAE Promedio:** 50.16 pts
- **MFE/MAE Ratio:** 0.88
- **Good Entries:** 67.5% (MFE > MAE)
- **R:R Promedio:** 2.32

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (14 oportunidades)

- **WR Teórico:** 57.1% (si se hubieran ejecutado)
- **TP_FIRST:** 42.9% (6 de 14)
- **SL_FIRST:** 57.1% (8 de 14)
- **MFE Promedio:** 76.66 pts
- **MAE Promedio:** 67.41 pts
- **MFE/MAE Ratio:** 0.15
- **Good Entries:** 42.9% (MFE > MAE)
- **R:R Promedio:** 3.17

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 36 | 483 | 249 | 360 |
| **TP_FIRST %** | 69.4% | 59.6% | 52.2% | 56.9% |
| **Good Entries %** | 44.4% | 45.5% | 47.8% | 60.8% |
| **MFE/MAE Ratio** | 58.45 | 3.93 | 1.64 | 2.91 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 360 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 69.2%
   - Good Entries: 60.8%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 12.15 pts
- **Mediana:** 8.47 pts
- **Min/Max:** 0.55 / 40.44 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 8.47 |
| P70 | 17.46 |
| P80 | 20.66 |
| P90 | 25.12 |
| P95 | 37.95 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 23.44 pts
- **Mediana:** 19.12 pts
- **Min/Max:** 3.25 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 19.12 |
| P70 | 34.58 |
| P80 | 39.60 |
| P90 | 50.67 |
| P95 | 53.29 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 25; // Era 60
public int MaxTPDistancePoints { get; set; } = 50; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 25.1pts, TP: 50.7pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (30.6%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.306) / 0.306
R:R_min = 2.27
```

**Estado actual:** R:R promedio = 2.42
**Gap:** -0.14 (necesitas mejorar R:R)

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

**Problema:** R:R actual (2.42) < R:R mínimo (2.27)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=30.6%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-13 10:02:48*