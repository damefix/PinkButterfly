# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-18 15:39:13
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251118_153445.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251118_153445.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 13
- **Win Rate:** 38.5% (5/13)
- **Profit Factor:** 1.25
- **Avg R:R Planeado:** 1.92
- **R:R Mínimo para Break-Even:** 1.60

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 26 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.92
   - R:R necesario: 1.60
   - **Gap:** -0.32

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bearish | 202 | 32.2% |
| Neutral | 236 | 37.6% |
| Bullish | 190 | 30.3% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** -0.030
- **Score Min/Max:** [-0.980, 0.920]
- **Componentes (promedio):**
  - EMA20 Slope: -0.030
  - EMA50 Cross: -0.070
  - BOS Count: -0.016
  - Regression 24h: 0.005

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.920 (apenas supera threshold)
- Score mínimo observado: -0.980 (apenas supera threshold)
- **Consecuencia:** Sistema queda 37.6% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (37.6%)

**Posibles causas:**
- **BOS Score bajo (-0.016):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio -0.030 indica poca señal direccional
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
| StructureFusion | 3236 | 100.0% | 100.0% |
| ProximityAnalyzer | 967 | 29.9% | 29.9% |
| DFM_Evaluated | 218 | 22.5% | 6.7% |
| DFM_Passed | 218 | 100.0% | 6.7% |
| RiskCalculator | 1928 | 884.4% | 59.6% |
| Risk_Accepted | 29 | 1.5% | 0.9% |
| TradeManager | 13 | 44.8% | 0.4% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 1899 señales, -98.5%)
- **Tasa de conversión final:** 0.40% (de 3236 zonas iniciales → 13 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 498 | 49.2% |
| NO_SL | 338 | 33.4% |
| ENTRY_TOO_FAR | 151 | 14.9% |
| TP_CHECK_FAIL | 25 | 2.5% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (498 rechazos, 49.2%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 701 | 88.4% |
| P0_ANY_DIR | 92 | 11.6% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 43.29 pts (máxima ganancia flotante)
- **MAE Promedio:** 28.58 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 384.74

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 10 | 76.9% |
| SL_FIRST (precio fue hacia SL) | 3 | 23.1% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 69.2%
- **Entradas Malas (MAE > MFE):** 30.8%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 13 | 10 | 3 | 76.9% | 43.29 | 28.58 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0003 | SELL | 43.00 | 38.00 | 1.13 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0003_2 | SELL | 43.00 | 38.00 | 1.13 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0007 | SELL | 16.50 | 32.75 | 0.50 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0009 | BUY | 16.00 | 10.75 | 1.49 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0010 | SELL | 21.00 | 11.25 | 1.87 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0013 | SELL | 131.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0016 | SELL | 0.00 | 103.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0016_2 | SELL | 0.00 | 103.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0020 | SELL | 17.75 | 34.25 | 0.52 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0025 | SELL | 64.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0025_2 | SELL | 64.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0025_3 | SELL | 64.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0029 | SELL | 80.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 353

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 131 | 25.2% | 62.6% | 2.44 | 51.1% | 2.33 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 68 | 44.1% | 61.8% | 1.68 | 50.0% | 2.43 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 113 | 77.0% | 70.8% | 6.51 | 69.9% | 2.45 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 41 | 82.9% | 82.9% | 1.74 | 87.8% | 2.22 | ✅ BUENA CALIDAD - Considerar incluir |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (131 oportunidades)

- **WR Teórico:** 25.2% (si se hubieran ejecutado)
- **TP_FIRST:** 62.6% (82 de 131)
- **SL_FIRST:** 35.1% (46 de 131)
- **MFE Promedio:** 42.25 pts
- **MAE Promedio:** 35.04 pts
- **MFE/MAE Ratio:** 2.44
- **Good Entries:** 51.1% (MFE > MAE)
- **R:R Promedio:** 2.33

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (68 oportunidades)

- **WR Teórico:** 44.1% (si se hubieran ejecutado)
- **TP_FIRST:** 61.8% (42 de 68)
- **SL_FIRST:** 38.2% (26 de 68)
- **MFE Promedio:** 55.57 pts
- **MAE Promedio:** 45.89 pts
- **MFE/MAE Ratio:** 1.68
- **Good Entries:** 50.0% (MFE > MAE)
- **R:R Promedio:** 2.43

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (113 oportunidades)

- **WR Teórico:** 77.0% (si se hubieran ejecutado)
- **TP_FIRST:** 70.8% (80 de 113)
- **SL_FIRST:** 28.3% (32 de 113)
- **MFE Promedio:** 75.59 pts
- **MAE Promedio:** 31.71 pts
- **MFE/MAE Ratio:** 6.51
- **Good Entries:** 69.9% (MFE > MAE)
- **R:R Promedio:** 2.45

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (41 oportunidades)

- **WR Teórico:** 82.9% (si se hubieran ejecutado)
- **TP_FIRST:** 82.9% (34 de 41)
- **SL_FIRST:** 17.1% (7 de 41)
- **MFE Promedio:** 101.10 pts
- **MAE Promedio:** 35.97 pts
- **MFE/MAE Ratio:** 1.74
- **Good Entries:** 87.8% (MFE > MAE)
- **R:R Promedio:** 2.22

**✅ BUENA CALIDAD - Considerar incluir**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 13 | 131 | 68 | 113 |
| **TP_FIRST %** | 76.9% | 62.6% | 61.8% | 70.8% |
| **Good Entries %** | 69.2% | 51.1% | 50.0% | 69.9% |
| **MFE/MAE Ratio** | 384.74 | 2.44 | 1.68 | 6.51 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 113 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 77.0%
   - Good Entries: 69.9%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 16.86 pts
- **Mediana:** 18.84 pts
- **Min/Max:** 5.14 / 26.24 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 18.84 |
| P70 | 21.19 |
| P80 | 24.06 |
| P90 | 26.24 |
| P95 | 26.24 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 31.10 pts
- **Mediana:** 36.00 pts
- **Min/Max:** 11.50 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 36.00 |
| P70 | 36.75 |
| P80 | 40.00 |
| P90 | 49.70 |
| P95 | 56.35 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 26; // Era 60
public int MaxTPDistancePoints { get; set; } = 49; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 26.2pts, TP: 49.7pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (38.5%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.385) / 0.385
R:R_min = 1.60
```

**Estado actual:** R:R promedio = 1.92
**Gap:** -0.32 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **49** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.92) < R:R mínimo (1.60)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=38.5%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-18 15:39:13*