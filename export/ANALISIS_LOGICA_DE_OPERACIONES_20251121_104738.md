# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-21 11:00:12
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251121_104738.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251121_104738.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 8
- **Win Rate:** 37.5% (3/8)
- **Profit Factor:** 2.83
- **Avg R:R Planeado:** 3.92
- **R:R Mínimo para Break-Even:** 1.67

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 18 puntos
   - TP máximo observado: 45 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 3.92
   - R:R necesario: 1.67
   - **Gap:** -2.25

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 832 | 26.6% |
| Bullish | 1113 | 35.6% |
| Bearish | 1184 | 37.8% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.011
- **Score Min/Max:** [-1.000, 1.000]
- **Componentes (promedio):**
  - EMA20 Slope: -0.011
  - EMA50 Cross: -0.026
  - BOS Count: 0.019
  - Regression 24h: 0.078

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 1.000 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 26.6% Neutral → bias no diferencia tendencias

### 2.3 Estado: Bias Compuesto Funcionando Correctamente

✅ **Threshold actual:** 0.3 (correcto)
✅ **Bias Neutral:** 26.6% (aceptable)
✅ **Score promedio:** 0.011

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 50668 | 100.0% | 100.0% |
| ProximityAnalyzer | 4089 | 8.1% | 8.1% |
| DFM_Evaluated | 311 | 7.6% | 0.6% |
| DFM_Passed | 311 | 100.0% | 0.6% |
| RiskCalculator | 8429 | 2710.3% | 16.6% |
| Risk_Accepted | 18 | 0.2% | 0.0% |
| TradeManager | 8 | 44.4% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 8411 señales, -99.8%)
- **Tasa de conversión final:** 0.02% (de 50668 zonas iniciales → 8 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 4,938 | 71.5% |
| ENTRY_TOO_FAR | 1,358 | 19.7% |
| TP_CHECK_FAIL | 380 | 5.5% |
| NO_SL | 228 | 3.3% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (4,938 rechazos, 71.5%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2206 | 69.3% |
| P0_ANY_DIR | 976 | 30.7% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 72.72 pts (máxima ganancia flotante)
- **MAE Promedio:** 7.56 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 505.08

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 6 | 75.0% |
| SL_FIRST (precio fue hacia SL) | 0 | 0.0% |
| NEUTRAL (sin dirección clara) | 2 | 25.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 100.0%
- **Entradas Malas (MAE > MFE):** 0.0%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 8 | 6 | 0 | 75.0% | 72.72 | 7.56 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0002 | SELL | 87.00 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0006 | SELL | 49.25 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0007 | SELL | 39.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0010 | SELL | 48.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0013 | SELL | 103.75 | 6.75 | 15.37 | NEUTRAL | CLOSED | ✅ Entrada excelente |
| T0013_2 | SELL | 103.75 | 6.75 | 15.37 | NEUTRAL | CLOSED | ✅ Entrada excelente |
| T0016 | SELL | 63.25 | 40.00 | 1.58 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0018 | SELL | 86.25 | 7.00 | 12.32 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 523

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 224 | 50.4% | 68.8% | 4.72 | 87.9% | 2.09 | ✅ BUENA CALIDAD - Considerar incluir |
| 2.0-3.0 ATR (Cerca) | 80 | 52.5% | 63.7% | 5.68 | 83.8% | 2.28 | ✅ BUENA CALIDAD - Considerar incluir |
| 3.0-5.0 ATR (Media) | 126 | 52.4% | 77.0% | 5.28 | 92.9% | 2.09 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 85 | 30.6% | 76.5% | 3.84 | 98.8% | 2.08 | ⚠️ CALIDAD MEDIA - Revisar |
| >10.0 ATR (Muy lejos) | 8 | 12.5% | 12.5% | 1.68 | 100.0% | 1.79 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (224 oportunidades)

- **WR Teórico:** 50.4% (si se hubieran ejecutado)
- **TP_FIRST:** 68.8% (154 de 224)
- **SL_FIRST:** 22.3% (50 de 224)
- **MFE Promedio:** 55.29 pts
- **MAE Promedio:** 13.22 pts
- **MFE/MAE Ratio:** 4.72
- **Good Entries:** 87.9% (MFE > MAE)
- **R:R Promedio:** 2.09

**✅ BUENA CALIDAD - Considerar incluir**

**2.0-3.0 ATR (Cerca)** (80 oportunidades)

- **WR Teórico:** 52.5% (si se hubieran ejecutado)
- **TP_FIRST:** 63.7% (51 de 80)
- **SL_FIRST:** 31.2% (25 de 80)
- **MFE Promedio:** 60.38 pts
- **MAE Promedio:** 17.42 pts
- **MFE/MAE Ratio:** 5.68
- **Good Entries:** 83.8% (MFE > MAE)
- **R:R Promedio:** 2.28

**✅ BUENA CALIDAD - Considerar incluir**

**3.0-5.0 ATR (Media)** (126 oportunidades)

- **WR Teórico:** 52.4% (si se hubieran ejecutado)
- **TP_FIRST:** 77.0% (97 de 126)
- **SL_FIRST:** 16.7% (21 de 126)
- **MFE Promedio:** 63.71 pts
- **MAE Promedio:** 10.26 pts
- **MFE/MAE Ratio:** 5.28
- **Good Entries:** 92.9% (MFE > MAE)
- **R:R Promedio:** 2.09

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (85 oportunidades)

- **WR Teórico:** 30.6% (si se hubieran ejecutado)
- **TP_FIRST:** 76.5% (65 de 85)
- **SL_FIRST:** 23.5% (20 de 85)
- **MFE Promedio:** 47.81 pts
- **MAE Promedio:** 10.92 pts
- **MFE/MAE Ratio:** 3.84
- **Good Entries:** 98.8% (MFE > MAE)
- **R:R Promedio:** 2.08

**⚠️ CALIDAD MEDIA - Revisar**

**>10.0 ATR (Muy lejos)** (8 oportunidades)

- **WR Teórico:** 12.5% (si se hubieran ejecutado)
- **TP_FIRST:** 12.5% (1 de 8)
- **SL_FIRST:** 87.5% (7 de 8)
- **MFE Promedio:** 25.97 pts
- **MAE Promedio:** 14.25 pts
- **MFE/MAE Ratio:** 1.68
- **Good Entries:** 100.0% (MFE > MAE)
- **R:R Promedio:** 1.79

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 8 | 224 | 80 | 126 |
| **TP_FIRST %** | 75.0% | 68.8% | 63.7% | 77.0% |
| **Good Entries %** | 100.0% | 87.9% | 83.8% | 92.9% |
| **MFE/MAE Ratio** | 505.08 | 4.72 | 5.68 | 5.28 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

🚨 **CRÍTICO: El rango 2.0-3.0 ATR contiene 80 oportunidades de BUENA CALIDAD**
   - WR Teórico: 52.5%
   - Good Entries: 83.8%
   - **ACCIÓN:** Considerar aumentar MaxDistanceToRegister_ATR_Normal de 2.0 a 3.0

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 126 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 52.4%
   - Good Entries: 92.9%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 9.74 pts
- **Mediana:** 6.95 pts
- **Min/Max:** 5.81 / 18.21 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 6.95 |
| P70 | 12.81 |
| P80 | 15.97 |
| P90 | 18.49 |
| P95 | 19.75 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 33.50 pts
- **Mediana:** 37.25 pts
- **Min/Max:** 14.00 / 44.75 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 37.25 |
| P70 | 40.60 |
| P80 | 43.95 |
| P90 | 44.85 |
| P95 | 45.30 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 18; // Era 60
public int MaxTPDistancePoints { get; set; } = 44; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 18.5pts, TP: 44.9pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (37.5%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.375) / 0.375
R:R_min = 1.67
```

**Estado actual:** R:R promedio = 3.92
**Gap:** -2.25 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **18** (P90 real)
2. **MaxTPDistancePoints:** 120 → **44** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (3.92) < R:R mínimo (1.67)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=37.5%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-21 11:00:12*