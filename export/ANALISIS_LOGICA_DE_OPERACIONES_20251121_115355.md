# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-21 12:02:37
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251121_115355.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251121_115355.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 7
- **Win Rate:** 28.6% (2/7)
- **Profit Factor:** 1.58
- **Avg R:R Planeado:** 4.16
- **R:R Mínimo para Break-Even:** 2.50

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 23 puntos
   - TP máximo observado: 45 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 4.16
   - R:R necesario: 2.50
   - **Gap:** -1.66

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 828 | 26.5% |
| Bullish | 1113 | 35.6% |
| Bearish | 1188 | 38.0% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.009
- **Score Min/Max:** [-1.000, 1.000]
- **Componentes (promedio):**
  - EMA20 Slope: -0.012
  - EMA50 Cross: -0.028
  - BOS Count: 0.019
  - Regression 24h: 0.077

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 1.000 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 26.5% Neutral → bias no diferencia tendencias

### 2.3 Estado: Bias Compuesto Funcionando Correctamente

✅ **Threshold actual:** 0.3 (correcto)
✅ **Bias Neutral:** 26.5% (aceptable)
✅ **Score promedio:** 0.009

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 51012 | 100.0% | 100.0% |
| ProximityAnalyzer | 4138 | 8.1% | 8.1% |
| DFM_Evaluated | 286 | 6.9% | 0.6% |
| DFM_Passed | 286 | 100.0% | 0.6% |
| RiskCalculator | 8451 | 2954.9% | 16.6% |
| Risk_Accepted | 12 | 0.1% | 0.0% |
| TradeManager | 7 | 58.3% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 8439 señales, -99.9%)
- **Tasa de conversión final:** 0.01% (de 51012 zonas iniciales → 7 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 5,086 | 73.1% |
| ENTRY_TOO_FAR | 1,384 | 19.9% |
| TP_CHECK_FAIL | 251 | 3.6% |
| NO_SL | 233 | 3.4% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (5,086 rechazos, 73.1%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2239 | 68.5% |
| P0_ANY_DIR | 1032 | 31.5% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 94.04 pts (máxima ganancia flotante)
- **MAE Promedio:** 20.93 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 148.42

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 6 | 85.7% |
| SL_FIRST (precio fue hacia SL) | 1 | 14.3% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 85.7%
- **Entradas Malas (MAE > MFE):** 14.3%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 7 | 6 | 1 | 85.7% | 94.04 | 20.93 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | SELL | 107.00 | 14.50 | 7.38 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0002 | SELL | 132.00 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0004 | SELL | 107.00 | 14.50 | 7.38 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0008 | SELL | 105.00 | 16.50 | 6.36 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0009 | SELL | 77.00 | 13.00 | 5.92 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0010 | SELL | 44.00 | 81.00 | 0.54 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0012 | SELL | 86.25 | 7.00 | 12.32 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 476

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 199 | 89.4% | 87.4% | 7.81 | 91.5% | 2.20 | ✅ BUENA CALIDAD - Considerar incluir |
| 2.0-3.0 ATR (Cerca) | 74 | 85.1% | 74.3% | 7.56 | 85.1% | 2.26 | ✅ BUENA CALIDAD - Considerar incluir |
| 3.0-5.0 ATR (Media) | 99 | 84.8% | 87.9% | 13.95 | 89.9% | 2.05 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 76 | 97.4% | 97.4% | 11.40 | 97.4% | 2.17 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 28 | 100.0% | 100.0% | 9.31 | 100.0% | 2.53 | ✅ BUENA CALIDAD - Considerar incluir |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (199 oportunidades)

- **WR Teórico:** 89.4% (si se hubieran ejecutado)
- **TP_FIRST:** 87.4% (174 de 199)
- **SL_FIRST:** 5.5% (11 de 199)
- **MFE Promedio:** 134.33 pts
- **MAE Promedio:** 16.93 pts
- **MFE/MAE Ratio:** 7.81
- **Good Entries:** 91.5% (MFE > MAE)
- **R:R Promedio:** 2.20

**✅ BUENA CALIDAD - Considerar incluir**

**2.0-3.0 ATR (Cerca)** (74 oportunidades)

- **WR Teórico:** 85.1% (si se hubieran ejecutado)
- **TP_FIRST:** 74.3% (55 de 74)
- **SL_FIRST:** 21.6% (16 de 74)
- **MFE Promedio:** 123.39 pts
- **MAE Promedio:** 23.00 pts
- **MFE/MAE Ratio:** 7.56
- **Good Entries:** 85.1% (MFE > MAE)
- **R:R Promedio:** 2.26

**✅ BUENA CALIDAD - Considerar incluir**

**3.0-5.0 ATR (Media)** (99 oportunidades)

- **WR Teórico:** 84.8% (si se hubieran ejecutado)
- **TP_FIRST:** 87.9% (87 de 99)
- **SL_FIRST:** 2.0% (2 de 99)
- **MFE Promedio:** 137.82 pts
- **MAE Promedio:** 12.94 pts
- **MFE/MAE Ratio:** 13.95
- **Good Entries:** 89.9% (MFE > MAE)
- **R:R Promedio:** 2.05

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (76 oportunidades)

- **WR Teórico:** 97.4% (si se hubieran ejecutado)
- **TP_FIRST:** 97.4% (74 de 76)
- **SL_FIRST:** 1.3% (1 de 76)
- **MFE Promedio:** 129.88 pts
- **MAE Promedio:** 15.38 pts
- **MFE/MAE Ratio:** 11.40
- **Good Entries:** 97.4% (MFE > MAE)
- **R:R Promedio:** 2.17

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (28 oportunidades)

- **WR Teórico:** 100.0% (si se hubieran ejecutado)
- **TP_FIRST:** 100.0% (28 de 28)
- **SL_FIRST:** 0.0% (0 de 28)
- **MFE Promedio:** 111.27 pts
- **MAE Promedio:** 11.83 pts
- **MFE/MAE Ratio:** 9.31
- **Good Entries:** 100.0% (MFE > MAE)
- **R:R Promedio:** 2.53

**✅ BUENA CALIDAD - Considerar incluir**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 7 | 199 | 74 | 99 |
| **TP_FIRST %** | 85.7% | 87.4% | 74.3% | 87.9% |
| **Good Entries %** | 85.7% | 91.5% | 85.1% | 89.9% |
| **MFE/MAE Ratio** | 148.42 | 7.81 | 7.56 | 13.95 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

🚨 **CRÍTICO: El rango 2.0-3.0 ATR contiene 74 oportunidades de BUENA CALIDAD**
   - WR Teórico: 85.1%
   - Good Entries: 85.1%
   - **ACCIÓN:** Considerar aumentar MaxDistanceToRegister_ATR_Normal de 2.0 a 3.0

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 99 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 84.8%
   - Good Entries: 89.9%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 11.34 pts
- **Mediana:** 10.80 pts
- **Min/Max:** 2.38 / 23.49 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 10.80 |
| P70 | 12.45 |
| P80 | 17.05 |
| P90 | 25.64 |
| P95 | 29.93 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 35.64 pts
- **Mediana:** 38.75 pts
- **Min/Max:** 23.75 / 45.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 38.75 |
| P70 | 44.20 |
| P80 | 45.00 |
| P90 | 45.00 |
| P95 | 45.00 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 25; // Era 60
public int MaxTPDistancePoints { get; set; } = 45; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 25.6pts, TP: 45.0pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (28.6%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.286) / 0.286
R:R_min = 2.50
```

**Estado actual:** R:R promedio = 4.16
**Gap:** -1.66 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **45** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (4.16) < R:R mínimo (2.50)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=28.6%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-21 12:02:37*