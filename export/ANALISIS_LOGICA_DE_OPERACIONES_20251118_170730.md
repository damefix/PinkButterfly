# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-18 17:13:57
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251118_170730.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251118_170730.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 3
- **Win Rate:** 33.3% (1/3)
- **Profit Factor:** 0.78
- **Avg R:R Planeado:** 2.01
- **R:R Mínimo para Break-Even:** 2.00

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 12 puntos
   - TP máximo observado: 39 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.01
   - R:R necesario: 2.00
   - **Gap:** -0.01

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bearish | 201 | 32.1% |
| Neutral | 230 | 36.7% |
| Bullish | 195 | 31.2% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** -0.025
- **Score Min/Max:** [-0.970, 0.970]
- **Componentes (promedio):**
  - EMA20 Slope: -0.027
  - EMA50 Cross: -0.067
  - BOS Count: -0.005
  - Regression 24h: 0.006

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.970 (apenas supera threshold)
- Score mínimo observado: -0.970 (apenas supera threshold)
- **Consecuencia:** Sistema queda 36.7% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (36.7%)

**Posibles causas:**
- **BOS Score bajo (-0.005):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio -0.025 indica poca señal direccional
- **Mercado lateral:** Scores reales [-0.97, 0.97] muy cercanos a 0

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
| StructureFusion | 2415 | 100.0% | 100.0% |
| ProximityAnalyzer | 730 | 30.2% | 30.2% |
| DFM_Evaluated | 132 | 18.1% | 5.5% |
| DFM_Passed | 132 | 100.0% | 5.5% |
| RiskCalculator | 1328 | 1006.1% | 55.0% |
| Risk_Accepted | 1 | 0.1% | 0.0% |
| TradeManager | 3 | 300.0% | 0.1% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 1327 señales, -99.9%)
- **Tasa de conversión final:** 0.12% (de 2415 zonas iniciales → 3 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 489 | 66.9% |
| ENTRY_TOO_FAR | 147 | 20.1% |
| NO_SL | 89 | 12.2% |
| TP_CHECK_FAIL | 6 | 0.8% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (489 rechazos, 66.9%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 706 | 92.8% |
| P0_ANY_DIR | 55 | 7.2% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 31.00 pts (máxima ganancia flotante)
- **MAE Promedio:** 22.75 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 2.09

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 1 | 33.3% |
| SL_FIRST (precio fue hacia SL) | 2 | 66.7% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 66.7%
- **Entradas Malas (MAE > MFE):** 33.3%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 3 | 1 | 2 | 33.3% | 31.00 | 22.75 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0010 | SELL | 26.75 | 23.50 | 1.14 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0014 | SELL | 48.50 | 10.50 | 4.62 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0027 | SELL | 17.75 | 34.25 | 0.52 | SL_FIRST | CLOSED | ❌ Entrada muy mala |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 194

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 59 | 44.1% | 64.4% | 3.80 | 52.5% | 2.03 | ⚠️ CALIDAD MEDIA - Revisar |
| 2.0-3.0 ATR (Cerca) | 27 | 77.8% | 81.5% | 5.69 | 63.0% | 1.82 | ✅ BUENA CALIDAD - Considerar incluir |
| 3.0-5.0 ATR (Media) | 79 | 92.4% | 74.7% | 8.63 | 81.0% | 2.10 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 29 | 89.7% | 82.8% | 2.35 | 89.7% | 1.82 | ✅ BUENA CALIDAD - Considerar incluir |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (59 oportunidades)

- **WR Teórico:** 44.1% (si se hubieran ejecutado)
- **TP_FIRST:** 64.4% (38 de 59)
- **SL_FIRST:** 32.2% (19 de 59)
- **MFE Promedio:** 50.43 pts
- **MAE Promedio:** 41.24 pts
- **MFE/MAE Ratio:** 3.80
- **Good Entries:** 52.5% (MFE > MAE)
- **R:R Promedio:** 2.03

**⚠️ CALIDAD MEDIA - Revisar**

**2.0-3.0 ATR (Cerca)** (27 oportunidades)

- **WR Teórico:** 77.8% (si se hubieran ejecutado)
- **TP_FIRST:** 81.5% (22 de 27)
- **SL_FIRST:** 18.5% (5 de 27)
- **MFE Promedio:** 62.06 pts
- **MAE Promedio:** 50.73 pts
- **MFE/MAE Ratio:** 5.69
- **Good Entries:** 63.0% (MFE > MAE)
- **R:R Promedio:** 1.82

**✅ BUENA CALIDAD - Considerar incluir**

**3.0-5.0 ATR (Media)** (79 oportunidades)

- **WR Teórico:** 92.4% (si se hubieran ejecutado)
- **TP_FIRST:** 74.7% (59 de 79)
- **SL_FIRST:** 25.3% (20 de 79)
- **MFE Promedio:** 70.14 pts
- **MAE Promedio:** 23.79 pts
- **MFE/MAE Ratio:** 8.63
- **Good Entries:** 81.0% (MFE > MAE)
- **R:R Promedio:** 2.10

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (29 oportunidades)

- **WR Teórico:** 89.7% (si se hubieran ejecutado)
- **TP_FIRST:** 82.8% (24 de 29)
- **SL_FIRST:** 13.8% (4 de 29)
- **MFE Promedio:** 105.24 pts
- **MAE Promedio:** 40.39 pts
- **MFE/MAE Ratio:** 2.35
- **Good Entries:** 89.7% (MFE > MAE)
- **R:R Promedio:** 1.82

**✅ BUENA CALIDAD - Considerar incluir**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 3 | 59 | 27 | 79 |
| **TP_FIRST %** | 33.3% | 64.4% | 81.5% | 74.7% |
| **Good Entries %** | 66.7% | 52.5% | 63.0% | 81.0% |
| **MFE/MAE Ratio** | 2.09 | 3.80 | 5.69 | 8.63 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

🚨 **CRÍTICO: El rango 2.0-3.0 ATR contiene 27 oportunidades de BUENA CALIDAD**
   - WR Teórico: 77.8%
   - Good Entries: 63.0%
   - **ACCIÓN:** Considerar aumentar MaxDistanceToRegister_ATR_Normal de 2.0 a 3.0

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 79 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 92.4%
   - Good Entries: 81.0%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 10.67 pts
- **Mediana:** 10.26 pts
- **Min/Max:** 10.07 / 11.67 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 10.26 |
| P70 | 11.39 |
| P80 | 11.95 |
| P90 | 12.52 |
| P95 | 12.80 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 22.17 pts
- **Mediana:** 14.00 pts
- **Min/Max:** 13.50 / 39.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 14.00 |
| P70 | 34.00 |
| P80 | 44.00 |
| P90 | 54.00 |
| P95 | 59.00 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 12; // Era 60
public int MaxTPDistancePoints { get; set; } = 54; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 12.5pts, TP: 54.0pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (33.3%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.333) / 0.333
R:R_min = 2.00
```

**Estado actual:** R:R promedio = 2.01
**Gap:** -0.01 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **12** (P90 real)
2. **MaxTPDistancePoints:** 120 → **54** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.01) < R:R mínimo (2.00)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=33.3%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-18 17:13:57*