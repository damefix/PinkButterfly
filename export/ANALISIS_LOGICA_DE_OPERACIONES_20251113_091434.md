# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-13 09:20:22
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_091434.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251113_091434.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 35
- **Win Rate:** 34.3% (12/35)
- **Profit Factor:** 1.05
- **Avg R:R Planeado:** 2.45
- **R:R Mínimo para Break-Even:** 1.92

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 40 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.45
   - R:R necesario: 1.92
   - **Gap:** -0.53

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8033 | 34.4% |
| Bearish | 6272 | 26.9% |
| Bullish | 9020 | 38.7% |

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
| StructureFusion | 10322 | 100.0% | 100.0% |
| ProximityAnalyzer | 4155 | 40.3% | 40.3% |
| DFM_Evaluated | 848 | 20.4% | 8.2% |
| DFM_Passed | 848 | 100.0% | 8.2% |
| RiskCalculator | 6319 | 745.2% | 61.2% |
| Risk_Accepted | 101 | 1.6% | 1.0% |
| TradeManager | 35 | 34.7% | 0.3% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 6218 señales, -98.4%)
- **Tasa de conversión final:** 0.34% (de 10322 zonas iniciales → 35 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,652 | 58.8% |
| NO_SL | 513 | 18.3% |
| ENTRY_TOO_FAR | 384 | 13.7% |
| TP_CHECK_FAIL | 259 | 9.2% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,652 rechazos, 58.8%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2556 | 89.1% |
| P0_ANY_DIR | 313 | 10.9% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 44.46 pts (máxima ganancia flotante)
- **MAE Promedio:** 30.14 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 63.51

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 23 | 65.7% |
| SL_FIRST (precio fue hacia SL) | 11 | 31.4% |
| NEUTRAL (sin dirección clara) | 1 | 2.9% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 45.7%
- **Entradas Malas (MAE > MFE):** 54.3%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 35 | 23 | 11 | 65.7% | 44.46 | 30.14 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | SELL | 10.50 | 18.25 | 0.58 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0007 | SELL | 13.00 | 18.75 | 0.69 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0009 | SELL | 0.00 | 43.00 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
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
| T0049 | BUY | 11.25 | 10.00 | 1.12 | SL_FIRST | CLOSED | 👍 Entrada correcta |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,309

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 476 | 37.6% | 60.1% | 4.05 | 45.8% | 2.01 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 249 | 50.2% | 51.4% | 1.63 | 47.0% | 1.91 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 365 | 69.3% | 57.5% | 2.89 | 60.5% | 2.07 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 203 | 69.0% | 64.5% | 0.74 | 67.5% | 2.40 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 16 | 62.5% | 50.0% | 0.15 | 50.0% | 3.22 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (476 oportunidades)

- **WR Teórico:** 37.6% (si se hubieran ejecutado)
- **TP_FIRST:** 60.1% (286 de 476)
- **SL_FIRST:** 37.6% (179 de 476)
- **MFE Promedio:** 49.93 pts
- **MAE Promedio:** 39.30 pts
- **MFE/MAE Ratio:** 4.05
- **Good Entries:** 45.8% (MFE > MAE)
- **R:R Promedio:** 2.01

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (249 oportunidades)

- **WR Teórico:** 50.2% (si se hubieran ejecutado)
- **TP_FIRST:** 51.4% (128 de 249)
- **SL_FIRST:** 48.6% (121 de 249)
- **MFE Promedio:** 57.60 pts
- **MAE Promedio:** 42.28 pts
- **MFE/MAE Ratio:** 1.63
- **Good Entries:** 47.0% (MFE > MAE)
- **R:R Promedio:** 1.91

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (365 oportunidades)

- **WR Teórico:** 69.3% (si se hubieran ejecutado)
- **TP_FIRST:** 57.5% (210 de 365)
- **SL_FIRST:** 42.5% (155 de 365)
- **MFE Promedio:** 69.69 pts
- **MAE Promedio:** 42.93 pts
- **MFE/MAE Ratio:** 2.89
- **Good Entries:** 60.5% (MFE > MAE)
- **R:R Promedio:** 2.07

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (203 oportunidades)

- **WR Teórico:** 69.0% (si se hubieran ejecutado)
- **TP_FIRST:** 64.5% (131 de 203)
- **SL_FIRST:** 35.5% (72 de 203)
- **MFE Promedio:** 79.51 pts
- **MAE Promedio:** 52.73 pts
- **MFE/MAE Ratio:** 0.74
- **Good Entries:** 67.5% (MFE > MAE)
- **R:R Promedio:** 2.40

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (16 oportunidades)

- **WR Teórico:** 62.5% (si se hubieran ejecutado)
- **TP_FIRST:** 50.0% (8 de 16)
- **SL_FIRST:** 50.0% (8 de 16)
- **MFE Promedio:** 77.40 pts
- **MAE Promedio:** 67.41 pts
- **MFE/MAE Ratio:** 0.15
- **Good Entries:** 50.0% (MFE > MAE)
- **R:R Promedio:** 3.22

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 35 | 476 | 249 | 365 |
| **TP_FIRST %** | 65.7% | 60.1% | 51.4% | 57.5% |
| **Good Entries %** | 45.7% | 45.8% | 47.0% | 60.5% |
| **MFE/MAE Ratio** | 63.51 | 4.05 | 1.63 | 2.89 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 365 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 69.3%
   - Good Entries: 60.5%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 11.89 pts
- **Mediana:** 8.06 pts
- **Min/Max:** 0.55 / 40.44 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 8.06 |
| P70 | 16.88 |
| P80 | 21.27 |
| P90 | 25.56 |
| P95 | 38.10 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 23.09 pts
- **Mediana:** 18.75 pts
- **Min/Max:** 3.25 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 18.75 |
| P70 | 33.35 |
| P80 | 39.80 |
| P90 | 50.90 |
| P95 | 53.30 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 25; // Era 60
public int MaxTPDistancePoints { get; set; } = 50; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 25.6pts, TP: 50.9pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (34.3%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.343) / 0.343
R:R_min = 1.92
```

**Estado actual:** R:R promedio = 2.45
**Gap:** -0.53 (necesitas mejorar R:R)

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

**Problema:** R:R actual (2.45) < R:R mínimo (1.92)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=34.3%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-13 09:20:22*