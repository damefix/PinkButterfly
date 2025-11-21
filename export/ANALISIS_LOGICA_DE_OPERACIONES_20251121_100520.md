# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-21 10:12:52
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251121_100520.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251121_100520.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 9
- **Win Rate:** 33.3% (3/9)
- **Profit Factor:** 2.11
- **Avg R:R Planeado:** 3.78
- **R:R Mínimo para Break-Even:** 2.00

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 18 puntos
   - TP máximo observado: 45 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 3.78
   - R:R necesario: 2.00
   - **Gap:** -1.78

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 832 | 26.6% |
| Bullish | 1113 | 35.6% |
| Bearish | 1181 | 37.8% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.011
- **Score Min/Max:** [-1.000, 1.000]
- **Componentes (promedio):**
  - EMA20 Slope: -0.010
  - EMA50 Cross: -0.025
  - BOS Count: 0.019
  - Regression 24h: 0.079

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
| StructureFusion | 50556 | 100.0% | 100.0% |
| ProximityAnalyzer | 4071 | 8.1% | 8.1% |
| DFM_Evaluated | 313 | 7.7% | 0.6% |
| DFM_Passed | 313 | 100.0% | 0.6% |
| RiskCalculator | 8410 | 2686.9% | 16.6% |
| Risk_Accepted | 18 | 0.2% | 0.0% |
| TradeManager | 9 | 50.0% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 8392 señales, -99.8%)
- **Tasa de conversión final:** 0.02% (de 50556 zonas iniciales → 9 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 4,928 | 71.5% |
| ENTRY_TOO_FAR | 1,356 | 19.7% |
| TP_CHECK_FAIL | 382 | 5.5% |
| NO_SL | 225 | 3.3% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (4,928 rechazos, 71.5%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2184 | 69.2% |
| P0_ANY_DIR | 973 | 30.8% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 76.47 pts (máxima ganancia flotante)
- **MAE Promedio:** 10.92 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 449.27

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 6 | 66.7% |
| SL_FIRST (precio fue hacia SL) | 1 | 11.1% |
| NEUTRAL (sin dirección clara) | 2 | 22.2% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 100.0%
- **Entradas Malas (MAE > MFE):** 0.0%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 9 | 6 | 1 | 66.7% | 76.47 | 10.92 |

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
| T0017 | SELL | 106.50 | 37.75 | 2.82 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0018 | SELL | 86.25 | 7.00 | 12.32 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 512

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 216 | 54.2% | 65.7% | 4.37 | 92.6% | 2.01 | ✅ BUENA CALIDAD - Considerar incluir |
| 2.0-3.0 ATR (Cerca) | 76 | 61.8% | 63.2% | 6.14 | 94.7% | 2.22 | ✅ BUENA CALIDAD - Considerar incluir |
| 3.0-5.0 ATR (Media) | 125 | 54.4% | 76.8% | 5.28 | 96.8% | 2.11 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 87 | 32.2% | 59.8% | 3.88 | 100.0% | 2.07 | ⚠️ CALIDAD MEDIA - Revisar |
| >10.0 ATR (Muy lejos) | 8 | 12.5% | 12.5% | 1.68 | 100.0% | 1.79 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (216 oportunidades)

- **WR Teórico:** 54.2% (si se hubieran ejecutado)
- **TP_FIRST:** 65.7% (142 de 216)
- **SL_FIRST:** 24.1% (52 de 216)
- **MFE Promedio:** 58.16 pts
- **MAE Promedio:** 13.13 pts
- **MFE/MAE Ratio:** 4.37
- **Good Entries:** 92.6% (MFE > MAE)
- **R:R Promedio:** 2.01

**✅ BUENA CALIDAD - Considerar incluir**

**2.0-3.0 ATR (Cerca)** (76 oportunidades)

- **WR Teórico:** 61.8% (si se hubieran ejecutado)
- **TP_FIRST:** 63.2% (48 de 76)
- **SL_FIRST:** 32.9% (25 de 76)
- **MFE Promedio:** 69.79 pts
- **MAE Promedio:** 17.23 pts
- **MFE/MAE Ratio:** 6.14
- **Good Entries:** 94.7% (MFE > MAE)
- **R:R Promedio:** 2.22

**✅ BUENA CALIDAD - Considerar incluir**

**3.0-5.0 ATR (Media)** (125 oportunidades)

- **WR Teórico:** 54.4% (si se hubieran ejecutado)
- **TP_FIRST:** 76.8% (96 de 125)
- **SL_FIRST:** 16.8% (21 de 125)
- **MFE Promedio:** 68.66 pts
- **MAE Promedio:** 10.26 pts
- **MFE/MAE Ratio:** 5.28
- **Good Entries:** 96.8% (MFE > MAE)
- **R:R Promedio:** 2.11

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (87 oportunidades)

- **WR Teórico:** 32.2% (si se hubieran ejecutado)
- **TP_FIRST:** 59.8% (52 de 87)
- **SL_FIRST:** 23.0% (20 de 87)
- **MFE Promedio:** 52.80 pts
- **MAE Promedio:** 10.83 pts
- **MFE/MAE Ratio:** 3.88
- **Good Entries:** 100.0% (MFE > MAE)
- **R:R Promedio:** 2.07

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
| **Count** | 9 | 216 | 76 | 125 |
| **TP_FIRST %** | 66.7% | 65.7% | 63.2% | 76.8% |
| **Good Entries %** | 100.0% | 92.6% | 94.7% | 96.8% |
| **MFE/MAE Ratio** | 449.27 | 4.37 | 6.14 | 5.28 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

🚨 **CRÍTICO: El rango 2.0-3.0 ATR contiene 76 oportunidades de BUENA CALIDAD**
   - WR Teórico: 61.8%
   - Good Entries: 94.7%
   - **ACCIÓN:** Considerar aumentar MaxDistanceToRegister_ATR_Normal de 2.0 a 3.0

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 125 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 54.4%
   - Good Entries: 96.8%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 10.07 pts
- **Mediana:** 7.43 pts
- **Min/Max:** 5.81 / 18.21 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 7.43 |
| P70 | 12.70 |
| P80 | 15.41 |
| P90 | 18.21 |
| P95 | 19.61 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 33.53 pts
- **Mediana:** 35.25 pts
- **Min/Max:** 14.00 / 44.75 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 35.25 |
| P70 | 39.25 |
| P80 | 43.75 |
| P90 | 44.75 |
| P95 | 45.25 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 18; // Era 60
public int MaxTPDistancePoints { get; set; } = 44; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 18.2pts, TP: 44.8pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (33.3%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.333) / 0.333
R:R_min = 2.00
```

**Estado actual:** R:R promedio = 3.78
**Gap:** -1.78 (necesitas mejorar R:R)

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

**Problema:** R:R actual (3.78) < R:R mínimo (2.00)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=33.3%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-21 10:12:52*