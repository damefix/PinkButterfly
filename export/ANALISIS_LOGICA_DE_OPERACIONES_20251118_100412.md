# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-18 10:09:14
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251118_100412.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251118_100412.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 12
- **Win Rate:** 41.7% (5/12)
- **Profit Factor:** 1.27
- **Avg R:R Planeado:** 1.83
- **R:R Mínimo para Break-Even:** 1.40

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 26 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.83
   - R:R necesario: 1.40
   - **Gap:** -0.43

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bearish | 201 | 32.1% |
| Neutral | 236 | 37.6% |
| Bullish | 190 | 30.3% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** -0.026
- **Score Min/Max:** [-0.980, 0.920]
- **Componentes (promedio):**
  - EMA20 Slope: -0.033
  - EMA50 Cross: -0.069
  - BOS Count: -0.022
  - Regression 24h: 0.032

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.920 (apenas supera threshold)
- Score mínimo observado: -0.980 (apenas supera threshold)
- **Consecuencia:** Sistema queda 37.6% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (37.6%)

**Posibles causas:**
- **BOS Score bajo (-0.022):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio -0.026 indica poca señal direccional
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
| StructureFusion | 3246 | 100.0% | 100.0% |
| ProximityAnalyzer | 984 | 30.3% | 30.3% |
| DFM_Evaluated | 229 | 23.3% | 7.1% |
| DFM_Passed | 229 | 100.0% | 7.1% |
| RiskCalculator | 1970 | 860.3% | 60.7% |
| Risk_Accepted | 33 | 1.7% | 1.0% |
| TradeManager | 12 | 36.4% | 0.4% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 1937 señales, -98.3%)
- **Tasa de conversión final:** 0.37% (de 3246 zonas iniciales → 12 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 473 | 46.6% |
| NO_SL | 337 | 33.2% |
| ENTRY_TOO_FAR | 177 | 17.4% |
| TP_CHECK_FAIL | 29 | 2.9% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (473 rechazos, 46.6%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 701 | 87.8% |
| P0_ANY_DIR | 97 | 12.2% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 39.44 pts (máxima ganancia flotante)
- **MAE Promedio:** 30.96 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 333.55

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 9 | 75.0% |
| SL_FIRST (precio fue hacia SL) | 3 | 25.0% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 66.7%
- **Entradas Malas (MAE > MFE):** 33.3%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 12 | 9 | 3 | 75.0% | 39.44 | 30.96 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0007 | SELL | 43.00 | 38.00 | 1.13 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0007_2 | SELL | 43.00 | 38.00 | 1.13 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0015 | SELL | 16.50 | 32.75 | 0.50 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0017 | BUY | 16.00 | 10.75 | 1.49 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0018 | SELL | 21.00 | 11.25 | 1.87 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0021 | SELL | 131.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0024 | SELL | 0.00 | 103.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0024_2 | SELL | 0.00 | 103.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0028 | SELL | 17.75 | 34.25 | 0.52 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0033 | SELL | 61.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0033_2 | SELL | 61.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0033_3 | SELL | 61.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 363

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 139 | 24.5% | 61.9% | 2.25 | 46.0% | 2.33 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 72 | 40.3% | 59.7% | 1.53 | 45.8% | 2.40 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 110 | 77.3% | 71.8% | 5.70 | 67.3% | 2.27 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 42 | 88.1% | 83.3% | 1.76 | 85.7% | 2.17 | ✅ BUENA CALIDAD - Considerar incluir |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (139 oportunidades)

- **WR Teórico:** 24.5% (si se hubieran ejecutado)
- **TP_FIRST:** 61.9% (86 de 139)
- **SL_FIRST:** 36.0% (50 de 139)
- **MFE Promedio:** 41.27 pts
- **MAE Promedio:** 38.30 pts
- **MFE/MAE Ratio:** 2.25
- **Good Entries:** 46.0% (MFE > MAE)
- **R:R Promedio:** 2.33

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (72 oportunidades)

- **WR Teórico:** 40.3% (si se hubieran ejecutado)
- **TP_FIRST:** 59.7% (43 de 72)
- **SL_FIRST:** 40.3% (29 de 72)
- **MFE Promedio:** 57.85 pts
- **MAE Promedio:** 49.79 pts
- **MFE/MAE Ratio:** 1.53
- **Good Entries:** 45.8% (MFE > MAE)
- **R:R Promedio:** 2.40

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (110 oportunidades)

- **WR Teórico:** 77.3% (si se hubieran ejecutado)
- **TP_FIRST:** 71.8% (79 de 110)
- **SL_FIRST:** 28.2% (31 de 110)
- **MFE Promedio:** 77.12 pts
- **MAE Promedio:** 36.52 pts
- **MFE/MAE Ratio:** 5.70
- **Good Entries:** 67.3% (MFE > MAE)
- **R:R Promedio:** 2.27

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (42 oportunidades)

- **WR Teórico:** 88.1% (si se hubieran ejecutado)
- **TP_FIRST:** 83.3% (35 de 42)
- **SL_FIRST:** 16.7% (7 de 42)
- **MFE Promedio:** 101.84 pts
- **MAE Promedio:** 38.25 pts
- **MFE/MAE Ratio:** 1.76
- **Good Entries:** 85.7% (MFE > MAE)
- **R:R Promedio:** 2.17

**✅ BUENA CALIDAD - Considerar incluir**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 12 | 139 | 72 | 110 |
| **TP_FIRST %** | 75.0% | 61.9% | 59.7% | 71.8% |
| **Good Entries %** | 66.7% | 46.0% | 45.8% | 67.3% |
| **MFE/MAE Ratio** | 333.55 | 2.25 | 1.53 | 5.70 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 110 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 77.3%
   - Good Entries: 67.3%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 17.02 pts
- **Mediana:** 19.52 pts
- **Min/Max:** 5.14 / 26.24 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 19.52 |
| P70 | 21.42 |
| P80 | 24.60 |
| P90 | 26.24 |
| P95 | 26.24 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 30.02 pts
- **Mediana:** 34.38 pts
- **Min/Max:** 11.50 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 34.38 |
| P70 | 36.75 |
| P80 | 37.65 |
| P90 | 49.15 |
| P95 | 58.58 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 26; // Era 60
public int MaxTPDistancePoints { get; set; } = 49; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 26.2pts, TP: 49.1pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (41.7%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.417) / 0.417
R:R_min = 1.40
```

**Estado actual:** R:R promedio = 1.83
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
1. **MaxSLDistancePoints:** 60 → **26** (P90 real)
2. **MaxTPDistancePoints:** 120 → **49** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.83) < R:R mínimo (1.40)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=41.7%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-18 10:09:14*