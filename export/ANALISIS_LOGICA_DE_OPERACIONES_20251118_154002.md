# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-18 15:46:15
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251118_154002.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251118_154002.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 12
- **Win Rate:** 16.7% (2/12)
- **Profit Factor:** 0.59
- **Avg R:R Planeado:** 1.98
- **R:R Mínimo para Break-Even:** 5.00

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 26 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.98
   - R:R necesario: 5.00
   - **Gap:** 3.02

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bearish | 202 | 32.2% |
| Neutral | 235 | 37.4% |
| Bullish | 191 | 30.4% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** -0.030
- **Score Min/Max:** [-0.980, 0.920]
- **Componentes (promedio):**
  - EMA20 Slope: -0.030
  - EMA50 Cross: -0.070
  - BOS Count: -0.015
  - Regression 24h: 0.005

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.920 (apenas supera threshold)
- Score mínimo observado: -0.980 (apenas supera threshold)
- **Consecuencia:** Sistema queda 37.4% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (37.4%)

**Posibles causas:**
- **BOS Score bajo (-0.015):** BOS/CHoCH no se detectan correctamente
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
| StructureFusion | 3217 | 100.0% | 100.0% |
| ProximityAnalyzer | 967 | 30.1% | 30.1% |
| DFM_Evaluated | 205 | 21.2% | 6.4% |
| DFM_Passed | 205 | 100.0% | 6.4% |
| RiskCalculator | 1900 | 926.8% | 59.1% |
| Risk_Accepted | 24 | 1.3% | 0.7% |
| TradeManager | 12 | 50.0% | 0.4% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 1876 señales, -98.7%)
- **Tasa de conversión final:** 0.37% (de 3217 zonas iniciales → 12 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 501 | 50.0% |
| NO_SL | 332 | 33.1% |
| ENTRY_TOO_FAR | 147 | 14.7% |
| TP_CHECK_FAIL | 22 | 2.2% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (501 rechazos, 50.0%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 692 | 89.4% |
| P0_ANY_DIR | 82 | 10.6% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 35.75 pts (máxima ganancia flotante)
- **MAE Promedio:** 27.69 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 85.26

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 6 | 50.0% |
| SL_FIRST (precio fue hacia SL) | 5 | 41.7% |
| NEUTRAL (sin dirección clara) | 1 | 8.3% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 66.7%
- **Entradas Malas (MAE > MFE):** 33.3%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 12 | 6 | 5 | 50.0% | 35.75 | 27.69 |

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
| T0007 | SELL | 16.50 | 5.50 | 3.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0009 | BUY | 16.00 | 6.25 | 2.56 | NEUTRAL | CLOSED | ✅ Entrada excelente |
| T0010 | SELL | 21.00 | 11.25 | 1.87 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0012 | SELL | 187.25 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0014 | SELL | 0.00 | 103.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0015 | SELL | 18.75 | 31.00 | 0.60 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0017 | SELL | 2.75 | 34.50 | 0.08 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0017_2 | SELL | 2.75 | 34.50 | 0.08 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0019 | SELL | 45.00 | 27.25 | 1.65 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0024 | SELL | 33.00 | 2.75 | 12.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 340

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 122 | 27.0% | 63.9% | 2.75 | 50.8% | 2.48 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 69 | 47.8% | 56.5% | 1.45 | 53.6% | 2.34 | ⚠️ CALIDAD MEDIA - Revisar |
| 3.0-5.0 ATR (Media) | 102 | 75.5% | 67.6% | 2.97 | 67.6% | 2.47 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 47 | 83.0% | 89.4% | 2.05 | 87.2% | 2.67 | ✅ BUENA CALIDAD - Considerar incluir |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (122 oportunidades)

- **WR Teórico:** 27.0% (si se hubieran ejecutado)
- **TP_FIRST:** 63.9% (78 de 122)
- **SL_FIRST:** 33.6% (41 de 122)
- **MFE Promedio:** 46.31 pts
- **MAE Promedio:** 36.50 pts
- **MFE/MAE Ratio:** 2.75
- **Good Entries:** 50.8% (MFE > MAE)
- **R:R Promedio:** 2.48

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (69 oportunidades)

- **WR Teórico:** 47.8% (si se hubieran ejecutado)
- **TP_FIRST:** 56.5% (39 de 69)
- **SL_FIRST:** 43.5% (30 de 69)
- **MFE Promedio:** 59.19 pts
- **MAE Promedio:** 40.31 pts
- **MFE/MAE Ratio:** 1.45
- **Good Entries:** 53.6% (MFE > MAE)
- **R:R Promedio:** 2.34

**⚠️ CALIDAD MEDIA - Revisar**

**3.0-5.0 ATR (Media)** (102 oportunidades)

- **WR Teórico:** 75.5% (si se hubieran ejecutado)
- **TP_FIRST:** 67.6% (69 de 102)
- **SL_FIRST:** 31.4% (32 de 102)
- **MFE Promedio:** 77.70 pts
- **MAE Promedio:** 36.26 pts
- **MFE/MAE Ratio:** 2.97
- **Good Entries:** 67.6% (MFE > MAE)
- **R:R Promedio:** 2.47

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (47 oportunidades)

- **WR Teórico:** 83.0% (si se hubieran ejecutado)
- **TP_FIRST:** 89.4% (42 de 47)
- **SL_FIRST:** 10.6% (5 de 47)
- **MFE Promedio:** 123.79 pts
- **MAE Promedio:** 38.45 pts
- **MFE/MAE Ratio:** 2.05
- **Good Entries:** 87.2% (MFE > MAE)
- **R:R Promedio:** 2.67

**✅ BUENA CALIDAD - Considerar incluir**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 12 | 122 | 69 | 102 |
| **TP_FIRST %** | 50.0% | 63.9% | 56.5% | 67.6% |
| **Good Entries %** | 66.7% | 50.8% | 53.6% | 67.6% |
| **MFE/MAE Ratio** | 85.26 | 2.75 | 1.45 | 2.97 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 102 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 75.5%
   - Good Entries: 67.6%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 16.31 pts
- **Mediana:** 18.43 pts
- **Min/Max:** 5.14 / 26.24 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 18.43 |
| P70 | 20.25 |
| P80 | 21.19 |
| P90 | 24.73 |
| P95 | 28.01 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 30.44 pts
- **Mediana:** 32.75 pts
- **Min/Max:** 11.50 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 32.75 |
| P70 | 36.95 |
| P80 | 38.75 |
| P90 | 49.08 |
| P95 | 58.66 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 24; // Era 60
public int MaxTPDistancePoints { get; set; } = 49; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 24.7pts, TP: 49.1pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (16.7%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.167) / 0.167
R:R_min = 5.00
```

**Estado actual:** R:R promedio = 1.98
**Gap:** 3.02 (necesitas mejorar R:R)

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

**Problema:** R:R actual (1.98) < R:R mínimo (5.00)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=16.7%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-18 15:46:15*