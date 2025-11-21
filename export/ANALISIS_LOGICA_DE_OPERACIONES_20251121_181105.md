# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-21 18:21:24
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251121_181105.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251121_181105.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 8
- **Win Rate:** 62.5% (5/8)
- **Profit Factor:** 3.33
- **Avg R:R Planeado:** 2.29
- **R:R Mínimo para Break-Even:** 0.60

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 27 puntos
   - TP máximo observado: 46 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.29
   - R:R necesario: 0.60
   - **Gap:** -1.69

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 818 | 26.2% |
| Bullish | 1091 | 34.9% |
| Bearish | 1218 | 39.0% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** -0.006
- **Score Min/Max:** [-1.000, 1.000]
- **Componentes (promedio):**
  - EMA20 Slope: -0.021
  - EMA50 Cross: -0.046
  - BOS Count: -0.012
  - Regression 24h: 0.073

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 1.000 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 26.2% Neutral → bias no diferencia tendencias

### 2.3 Estado: Bias Compuesto Funcionando Correctamente

✅ **Threshold actual:** 0.3 (correcto)
✅ **Bias Neutral:** 26.2% (aceptable)
✅ **Score promedio:** -0.006

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 48139 | 100.0% | 100.0% |
| ProximityAnalyzer | 3866 | 8.0% | 8.0% |
| DFM_Evaluated | 279 | 7.2% | 0.6% |
| DFM_Passed | 279 | 100.0% | 0.6% |
| RiskCalculator | 7949 | 2849.1% | 16.5% |
| Risk_Accepted | 13 | 0.2% | 0.0% |
| TradeManager | 8 | 61.5% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 7936 señales, -99.8%)
- **Tasa de conversión final:** 0.02% (de 48139 zonas iniciales → 8 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 4,228 | 67.5% |
| ENTRY_TOO_FAR | 1,295 | 20.7% |
| TP_CHECK_FAIL | 503 | 8.0% |
| NO_SL | 240 | 3.8% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (4,228 rechazos, 67.5%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2496 | 78.6% |
| P0_ANY_DIR | 679 | 21.4% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 55.56 pts (máxima ganancia flotante)
- **MAE Promedio:** 11.59 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 377.99

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 5 | 62.5% |
| SL_FIRST (precio fue hacia SL) | 3 | 37.5% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 87.5%
- **Entradas Malas (MAE > MFE):** 12.5%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 8 | 5 | 3 | 62.5% | 55.56 | 11.59 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0003 | SELL | 30.25 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0004 | SELL | 30.25 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0005 | SELL | 30.25 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0006 | SELL | 3.25 | 26.50 | 0.12 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0007 | SELL | 89.50 | 13.25 | 6.75 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0010 | SELL | 79.00 | 25.25 | 3.13 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0011 | SELL | 95.75 | 20.75 | 4.61 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0013 | SELL | 86.25 | 7.00 | 12.32 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 467

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 135 | 45.2% | 47.4% | 10.77 | 78.5% | 2.19 | ✅ BUENA CALIDAD - Considerar incluir |
| 2.0-3.0 ATR (Cerca) | 75 | 36.0% | 33.3% | 2.50 | 64.0% | 2.36 | ✅ BUENA CALIDAD - Considerar incluir |
| 3.0-5.0 ATR (Media) | 120 | 42.5% | 42.5% | 0.93 | 56.7% | 2.34 | ⚠️ CALIDAD MEDIA - Revisar |
| 5.0-10.0 ATR (Lejos) | 111 | 34.2% | 39.6% | 0.54 | 48.6% | 2.50 | ❌ BAJA CALIDAD - Filtro correcto |
| >10.0 ATR (Muy lejos) | 26 | 0.0% | 0.0% | 0.43 | 11.5% | 2.89 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (135 oportunidades)

- **WR Teórico:** 45.2% (si se hubieran ejecutado)
- **TP_FIRST:** 47.4% (64 de 135)
- **SL_FIRST:** 40.7% (55 de 135)
- **MFE Promedio:** 43.79 pts
- **MAE Promedio:** 17.49 pts
- **MFE/MAE Ratio:** 10.77
- **Good Entries:** 78.5% (MFE > MAE)
- **R:R Promedio:** 2.19

**✅ BUENA CALIDAD - Considerar incluir**

**2.0-3.0 ATR (Cerca)** (75 oportunidades)

- **WR Teórico:** 36.0% (si se hubieran ejecutado)
- **TP_FIRST:** 33.3% (25 de 75)
- **SL_FIRST:** 62.7% (47 de 75)
- **MFE Promedio:** 35.44 pts
- **MAE Promedio:** 20.00 pts
- **MFE/MAE Ratio:** 2.50
- **Good Entries:** 64.0% (MFE > MAE)
- **R:R Promedio:** 2.36

**✅ BUENA CALIDAD - Considerar incluir**

**3.0-5.0 ATR (Media)** (120 oportunidades)

- **WR Teórico:** 42.5% (si se hubieran ejecutado)
- **TP_FIRST:** 42.5% (51 de 120)
- **SL_FIRST:** 47.5% (57 de 120)
- **MFE Promedio:** 52.03 pts
- **MAE Promedio:** 25.48 pts
- **MFE/MAE Ratio:** 0.93
- **Good Entries:** 56.7% (MFE > MAE)
- **R:R Promedio:** 2.34

**⚠️ CALIDAD MEDIA - Revisar**

**5.0-10.0 ATR (Lejos)** (111 oportunidades)

- **WR Teórico:** 34.2% (si se hubieran ejecutado)
- **TP_FIRST:** 39.6% (44 de 111)
- **SL_FIRST:** 56.8% (63 de 111)
- **MFE Promedio:** 47.42 pts
- **MAE Promedio:** 22.99 pts
- **MFE/MAE Ratio:** 0.54
- **Good Entries:** 48.6% (MFE > MAE)
- **R:R Promedio:** 2.50

**❌ BAJA CALIDAD - Filtro correcto**

**>10.0 ATR (Muy lejos)** (26 oportunidades)

- **WR Teórico:** 0.0% (si se hubieran ejecutado)
- **TP_FIRST:** 0.0% (0 de 26)
- **SL_FIRST:** 100.0% (26 de 26)
- **MFE Promedio:** 7.14 pts
- **MAE Promedio:** 22.61 pts
- **MFE/MAE Ratio:** 0.43
- **Good Entries:** 11.5% (MFE > MAE)
- **R:R Promedio:** 2.89

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 8 | 135 | 75 | 120 |
| **TP_FIRST %** | 62.5% | 47.4% | 33.3% | 42.5% |
| **Good Entries %** | 87.5% | 78.5% | 64.0% | 56.7% |
| **MFE/MAE Ratio** | 377.99 | 10.77 | 2.50 | 0.93 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

🚨 **CRÍTICO: El rango 2.0-3.0 ATR contiene 75 oportunidades de BUENA CALIDAD**
   - WR Teórico: 36.0%
   - Good Entries: 64.0%
   - **ACCIÓN:** Considerar aumentar MaxDistanceToRegister_ATR_Normal de 2.0 a 3.0

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 120 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 42.5%
   - Good Entries: 56.7%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 15.93 pts
- **Mediana:** 16.06 pts
- **Min/Max:** 7.43 / 26.91 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 16.06 |
| P70 | 17.82 |
| P80 | 20.79 |
| P90 | 27.68 |
| P95 | 31.12 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 34.62 pts
- **Mediana:** 37.25 pts
- **Min/Max:** 18.75 / 46.25 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 37.25 |
| P70 | 41.27 |
| P80 | 43.25 |
| P90 | 46.62 |
| P95 | 48.31 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 27; // Era 60
public int MaxTPDistancePoints { get; set; } = 46; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 27.7pts, TP: 46.6pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (62.5%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.625) / 0.625
R:R_min = 0.60
```

**Estado actual:** R:R promedio = 2.29
**Gap:** -1.69 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **27** (P90 real)
2. **MaxTPDistancePoints:** 120 → **46** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.29) < R:R mínimo (0.60)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=62.5%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-21 18:21:24*