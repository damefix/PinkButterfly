# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-17 18:55:45
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_184947.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251117_184947.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 7
- **Win Rate:** 42.9% (3/7)
- **Profit Factor:** 0.70
- **Avg R:R Planeado:** 1.86
- **R:R Mínimo para Break-Even:** 1.33

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 20 puntos
   - TP máximo observado: 42 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.86
   - R:R necesario: 1.33
   - **Gap:** -0.53

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8122 | 34.7% |
| Bullish | 9085 | 38.8% |
| Bearish | 6222 | 26.6% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.082
- **Score Min/Max:** [-1.000, 1.000]
- **Componentes (promedio):**
  - EMA20 Slope: 0.044
  - EMA50 Cross: 0.196
  - BOS Count: 0.007
  - Regression 24h: 0.092

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 1.000 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 34.7% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.7%)

**Posibles causas:**
- **BOS Score bajo (0.007):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio 0.082 indica poca señal direccional
- **Mercado lateral:** Scores reales [-1.00, 1.00] muy cercanos a 0

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
| StructureFusion | 54378 | 100.0% | 100.0% |
| ProximityAnalyzer | 1066 | 2.0% | 2.0% |
| DFM_Evaluated | 209 | 19.6% | 0.4% |
| DFM_Passed | 209 | 100.0% | 0.4% |
| RiskCalculator | 2067 | 989.0% | 3.8% |
| Risk_Accepted | 1 | 0.0% | 0.0% |
| TradeManager | 7 | 700.0% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 2066 señales, -100.0%)
- **Tasa de conversión final:** 0.01% (de 54378 zonas iniciales → 7 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 462 | 43.6% |
| ENTRY_TOO_FAR | 341 | 32.2% |
| TP_CHECK_FAIL | 230 | 21.7% |
| NO_SL | 26 | 2.5% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (462 rechazos, 43.6%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 1186 | 98.3% |
| P0_ANY_DIR | 21 | 1.7% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 47.50 pts (máxima ganancia flotante)
- **MAE Promedio:** 15.21 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 18.02

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 5 | 71.4% |
| SL_FIRST (precio fue hacia SL) | 2 | 28.6% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 100.0%
- **Entradas Malas (MAE > MFE):** 0.0%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 7 | 5 | 2 | 71.4% | 47.50 | 15.21 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0005 | SELL | 40.00 | 14.50 | 2.76 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0013 | SELL | 34.25 | 18.50 | 1.85 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0014 | SELL | 30.00 | 17.00 | 1.76 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0016 | SELL | 66.75 | 14.25 | 4.68 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0019 | SELL | 83.50 | 0.75 | 111.33 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0023 | SELL | 39.00 | 20.75 | 1.88 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0024 | SELL | 39.00 | 20.75 | 1.88 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 359

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 126 | 29.4% | 54.8% | 6.98 | 63.5% | 1.80 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 69 | 52.2% | 59.4% | 8.96 | 66.7% | 1.96 | ✅ BUENA CALIDAD - Considerar incluir |
| 3.0-5.0 ATR (Media) | 87 | 43.7% | 52.9% | 1.31 | 57.5% | 1.95 | ⚠️ CALIDAD MEDIA - Revisar |
| 5.0-10.0 ATR (Lejos) | 66 | 59.1% | 66.7% | 3.71 | 66.7% | 1.93 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 11 | 63.6% | 45.5% | 2.19 | 81.8% | 1.53 | ✅ BUENA CALIDAD - Considerar incluir |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (126 oportunidades)

- **WR Teórico:** 29.4% (si se hubieran ejecutado)
- **TP_FIRST:** 54.8% (69 de 126)
- **SL_FIRST:** 42.9% (54 de 126)
- **MFE Promedio:** 42.63 pts
- **MAE Promedio:** 32.82 pts
- **MFE/MAE Ratio:** 6.98
- **Good Entries:** 63.5% (MFE > MAE)
- **R:R Promedio:** 1.80

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (69 oportunidades)

- **WR Teórico:** 52.2% (si se hubieran ejecutado)
- **TP_FIRST:** 59.4% (41 de 69)
- **SL_FIRST:** 40.6% (28 de 69)
- **MFE Promedio:** 47.65 pts
- **MAE Promedio:** 33.55 pts
- **MFE/MAE Ratio:** 8.96
- **Good Entries:** 66.7% (MFE > MAE)
- **R:R Promedio:** 1.96

**✅ BUENA CALIDAD - Considerar incluir**

**3.0-5.0 ATR (Media)** (87 oportunidades)

- **WR Teórico:** 43.7% (si se hubieran ejecutado)
- **TP_FIRST:** 52.9% (46 de 87)
- **SL_FIRST:** 46.0% (40 de 87)
- **MFE Promedio:** 55.28 pts
- **MAE Promedio:** 37.76 pts
- **MFE/MAE Ratio:** 1.31
- **Good Entries:** 57.5% (MFE > MAE)
- **R:R Promedio:** 1.95

**⚠️ CALIDAD MEDIA - Revisar**

**5.0-10.0 ATR (Lejos)** (66 oportunidades)

- **WR Teórico:** 59.1% (si se hubieran ejecutado)
- **TP_FIRST:** 66.7% (44 de 66)
- **SL_FIRST:** 33.3% (22 de 66)
- **MFE Promedio:** 61.28 pts
- **MAE Promedio:** 33.50 pts
- **MFE/MAE Ratio:** 3.71
- **Good Entries:** 66.7% (MFE > MAE)
- **R:R Promedio:** 1.93

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (11 oportunidades)

- **WR Teórico:** 63.6% (si se hubieran ejecutado)
- **TP_FIRST:** 45.5% (5 de 11)
- **SL_FIRST:** 54.5% (6 de 11)
- **MFE Promedio:** 42.89 pts
- **MAE Promedio:** 33.18 pts
- **MFE/MAE Ratio:** 2.19
- **Good Entries:** 81.8% (MFE > MAE)
- **R:R Promedio:** 1.53

**✅ BUENA CALIDAD - Considerar incluir**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 7 | 126 | 69 | 87 |
| **TP_FIRST %** | 71.4% | 54.8% | 59.4% | 52.9% |
| **Good Entries %** | 100.0% | 63.5% | 66.7% | 57.5% |
| **MFE/MAE Ratio** | 18.02 | 6.98 | 8.96 | 1.31 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

🚨 **CRÍTICO: El rango 2.0-3.0 ATR contiene 69 oportunidades de BUENA CALIDAD**
   - WR Teórico: 52.2%
   - Good Entries: 66.7%
   - **ACCIÓN:** Considerar aumentar MaxDistanceToRegister_ATR_Normal de 2.0 a 3.0

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 87 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 43.7%
   - Good Entries: 57.5%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 12.49 pts
- **Mediana:** 8.68 pts
- **Min/Max:** 8.60 / 19.83 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 8.68 |
| P70 | 17.19 |
| P80 | 19.82 |
| P90 | 19.83 |
| P95 | 19.84 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 24.21 pts
- **Mediana:** 15.75 pts
- **Min/Max:** 13.75 / 42.25 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 15.75 |
| P70 | 35.75 |
| P80 | 42.25 |
| P90 | 42.25 |
| P95 | 42.25 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 19; // Era 60
public int MaxTPDistancePoints { get; set; } = 42; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 19.8pts, TP: 42.2pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (42.9%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.429) / 0.429
R:R_min = 1.33
```

**Estado actual:** R:R promedio = 1.86
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
1. **MaxSLDistancePoints:** 60 → **19** (P90 real)
2. **MaxTPDistancePoints:** 120 → **42** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.86) < R:R mínimo (1.33)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=42.9%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-17 18:55:45*