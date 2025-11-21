# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-17 20:26:47
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_202347.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251117_202347.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 8
- **Win Rate:** 62.5% (5/8)
- **Profit Factor:** 2.13
- **Avg R:R Planeado:** 1.60
- **R:R Mínimo para Break-Even:** 0.60

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 35 puntos
   - TP máximo observado: 48 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.60
   - R:R necesario: 0.60
   - **Gap:** -1.00

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bullish | 1962 | 39.2% |
| Neutral | 1360 | 27.2% |
| Bearish | 1680 | 33.6% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.039
- **Score Min/Max:** [-1.000, 1.000]
- **Componentes (promedio):**
  - EMA20 Slope: 0.017
  - EMA50 Cross: -0.009
  - BOS Count: 0.069
  - Regression 24h: 0.096

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 1.000 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 27.2% Neutral → bias no diferencia tendencias

### 2.3 Estado: Bias Compuesto Funcionando Correctamente

✅ **Threshold actual:** 0.3 (correcto)
✅ **Bias Neutral:** 27.2% (aceptable)
✅ **Score promedio:** 0.039

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 11105 | 100.0% | 100.0% |
| ProximityAnalyzer | 2596 | 23.4% | 23.4% |
| DFM_Evaluated | 572 | 22.0% | 5.2% |
| DFM_Passed | 572 | 100.0% | 5.2% |
| RiskCalculator | 4160 | 727.3% | 37.5% |
| Risk_Accepted | 16 | 0.4% | 0.1% |
| TradeManager | 8 | 50.0% | 0.1% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 4144 señales, -99.6%)
- **Tasa de conversión final:** 0.07% (de 11105 zonas iniciales → 8 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,262 | 60.5% |
| ENTRY_TOO_FAR | 656 | 31.5% |
| TP_CHECK_FAIL | 104 | 5.0% |
| NO_SL | 63 | 3.0% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,262 rechazos, 60.5%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 1499 | 98.2% |
| P0_ANY_DIR | 28 | 1.8% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 71.72 pts (máxima ganancia flotante)
- **MAE Promedio:** 22.62 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 3.85

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 3 | 37.5% |
| SL_FIRST (precio fue hacia SL) | 5 | 62.5% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 87.5%
- **Entradas Malas (MAE > MFE):** 12.5%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 8 | 3 | 5 | 37.5% | 71.72 | 22.62 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | SELL | 232.50 | 25.00 | 9.30 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0003 | SELL | 63.25 | 18.25 | 3.47 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0005 | SELL | 29.25 | 11.25 | 2.60 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0006 | SELL | 5.50 | 47.00 | 0.12 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0007 | SELL | 50.50 | 9.75 | 5.18 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0011 | SELL | 81.25 | 13.25 | 6.13 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0013 | SELL | 50.50 | 31.50 | 1.60 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0014 | SELL | 61.00 | 25.00 | 2.44 | SL_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 710

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 276 | 79.7% | 66.7% | 11.05 | 84.8% | 1.57 | ✅ BUENA CALIDAD - Considerar incluir |
| 2.0-3.0 ATR (Cerca) | 120 | 80.0% | 68.3% | 3.80 | 85.0% | 1.70 | ✅ BUENA CALIDAD - Considerar incluir |
| 3.0-5.0 ATR (Media) | 180 | 85.6% | 64.4% | 5.28 | 87.8% | 1.69 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 106 | 79.2% | 67.9% | 3.96 | 66.0% | 1.75 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 28 | 57.1% | 35.7% | 1.00 | 35.7% | 1.66 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (276 oportunidades)

- **WR Teórico:** 79.7% (si se hubieran ejecutado)
- **TP_FIRST:** 66.7% (184 de 276)
- **SL_FIRST:** 31.9% (88 de 276)
- **MFE Promedio:** 69.61 pts
- **MAE Promedio:** 20.66 pts
- **MFE/MAE Ratio:** 11.05
- **Good Entries:** 84.8% (MFE > MAE)
- **R:R Promedio:** 1.57

**✅ BUENA CALIDAD - Considerar incluir**

**2.0-3.0 ATR (Cerca)** (120 oportunidades)

- **WR Teórico:** 80.0% (si se hubieran ejecutado)
- **TP_FIRST:** 68.3% (82 de 120)
- **SL_FIRST:** 30.0% (36 de 120)
- **MFE Promedio:** 78.20 pts
- **MAE Promedio:** 32.08 pts
- **MFE/MAE Ratio:** 3.80
- **Good Entries:** 85.0% (MFE > MAE)
- **R:R Promedio:** 1.70

**✅ BUENA CALIDAD - Considerar incluir**

**3.0-5.0 ATR (Media)** (180 oportunidades)

- **WR Teórico:** 85.6% (si se hubieran ejecutado)
- **TP_FIRST:** 64.4% (116 de 180)
- **SL_FIRST:** 35.6% (64 de 180)
- **MFE Promedio:** 85.58 pts
- **MAE Promedio:** 28.78 pts
- **MFE/MAE Ratio:** 5.28
- **Good Entries:** 87.8% (MFE > MAE)
- **R:R Promedio:** 1.69

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (106 oportunidades)

- **WR Teórico:** 79.2% (si se hubieran ejecutado)
- **TP_FIRST:** 67.9% (72 de 106)
- **SL_FIRST:** 32.1% (34 de 106)
- **MFE Promedio:** 74.70 pts
- **MAE Promedio:** 30.58 pts
- **MFE/MAE Ratio:** 3.96
- **Good Entries:** 66.0% (MFE > MAE)
- **R:R Promedio:** 1.75

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (28 oportunidades)

- **WR Teórico:** 57.1% (si se hubieran ejecutado)
- **TP_FIRST:** 35.7% (10 de 28)
- **SL_FIRST:** 64.3% (18 de 28)
- **MFE Promedio:** 32.18 pts
- **MAE Promedio:** 35.32 pts
- **MFE/MAE Ratio:** 1.00
- **Good Entries:** 35.7% (MFE > MAE)
- **R:R Promedio:** 1.66

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 8 | 276 | 120 | 180 |
| **TP_FIRST %** | 37.5% | 66.7% | 68.3% | 64.4% |
| **Good Entries %** | 87.5% | 84.8% | 85.0% | 87.8% |
| **MFE/MAE Ratio** | 3.85 | 11.05 | 3.80 | 5.28 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

🚨 **CRÍTICO: El rango 2.0-3.0 ATR contiene 120 oportunidades de BUENA CALIDAD**
   - WR Teórico: 80.0%
   - Good Entries: 85.0%
   - **ACCIÓN:** Considerar aumentar MaxDistanceToRegister_ATR_Normal de 2.0 a 3.0

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 180 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 85.6%
   - Good Entries: 87.8%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 26.41 pts
- **Mediana:** 27.85 pts
- **Min/Max:** 7.55 / 34.75 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 27.85 |
| P70 | 30.54 |
| P80 | 31.53 |
| P90 | 35.15 |
| P95 | 36.97 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 39.84 pts
- **Mediana:** 40.00 pts
- **Min/Max:** 18.25 / 48.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 40.00 |
| P70 | 46.95 |
| P80 | 48.00 |
| P90 | 48.00 |
| P95 | 48.00 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 35; // Era 60
public int MaxTPDistancePoints { get; set; } = 48; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 35.2pts, TP: 48.0pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (62.5%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.625) / 0.625
R:R_min = 0.60
```

**Estado actual:** R:R promedio = 1.60
**Gap:** -1.00 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **35** (P90 real)
2. **MaxTPDistancePoints:** 120 → **48** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.60) < R:R mínimo (0.60)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=62.5%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-17 20:26:47*