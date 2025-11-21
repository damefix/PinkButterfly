# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-21 09:47:04
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251121_093934.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251121_093934.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 6
- **Win Rate:** 83.3% (5/6)
- **Profit Factor:** 16.64
- **Avg R:R Planeado:** 2.43
- **R:R Mínimo para Break-Even:** 0.20

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 27 puntos
   - TP máximo observado: 46 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.43
   - R:R necesario: 0.20
   - **Gap:** -2.23

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
- **Score Promedio:** 0.012
- **Score Min/Max:** [-1.000, 1.000]
- **Componentes (promedio):**
  - EMA20 Slope: -0.010
  - EMA50 Cross: -0.023
  - BOS Count: 0.020
  - Regression 24h: 0.079

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 1.000 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 26.6% Neutral → bias no diferencia tendencias

### 2.3 Estado: Bias Compuesto Funcionando Correctamente

✅ **Threshold actual:** 0.3 (correcto)
✅ **Bias Neutral:** 26.6% (aceptable)
✅ **Score promedio:** 0.012

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 49427 | 100.0% | 100.0% |
| ProximityAnalyzer | 3859 | 7.8% | 7.8% |
| DFM_Evaluated | 283 | 7.3% | 0.6% |
| DFM_Passed | 283 | 100.0% | 0.6% |
| RiskCalculator | 8154 | 2881.3% | 16.5% |
| Risk_Accepted | 1 | 0.0% | 0.0% |
| TradeManager | 6 | 600.0% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 8153 señales, -100.0%)
- **Tasa de conversión final:** 0.01% (de 49427 zonas iniciales → 6 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 4,977 | 72.5% |
| ENTRY_TOO_FAR | 1,286 | 18.7% |
| TP_CHECK_FAIL | 378 | 5.5% |
| NO_SL | 220 | 3.2% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (4,977 rechazos, 72.5%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 1980 | 70.6% |
| P0_ANY_DIR | 824 | 29.4% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 109.42 pts (máxima ganancia flotante)
- **MAE Promedio:** 9.75 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 337.85

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 5 | 83.3% |
| SL_FIRST (precio fue hacia SL) | 0 | 0.0% |
| NEUTRAL (sin dirección clara) | 1 | 16.7% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 100.0%
- **Entradas Malas (MAE > MFE):** 0.0%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 6 | 5 | 0 | 83.3% | 109.42 | 9.75 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | SELL | 153.00 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0002 | SELL | 135.00 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0004 | SELL | 107.50 | 16.50 | 6.52 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0006 | SELL | 79.00 | 19.75 | 4.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0007 | SELL | 95.75 | 15.25 | 6.28 | NEUTRAL | CLOSED | ✅ Entrada excelente |
| T0009 | SELL | 86.25 | 7.00 | 12.32 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 401

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 169 | 97.0% | 91.7% | 13.31 | 99.4% | 2.00 | ✅ BUENA CALIDAD - Considerar incluir |
| 2.0-3.0 ATR (Cerca) | 69 | 92.8% | 81.2% | 14.74 | 100.0% | 2.12 | ✅ BUENA CALIDAD - Considerar incluir |
| 3.0-5.0 ATR (Media) | 83 | 95.2% | 96.4% | 16.19 | 100.0% | 2.17 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 57 | 98.2% | 100.0% | 13.12 | 100.0% | 2.01 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 23 | 100.0% | 100.0% | 12.54 | 100.0% | 2.22 | ✅ BUENA CALIDAD - Considerar incluir |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (169 oportunidades)

- **WR Teórico:** 97.0% (si se hubieran ejecutado)
- **TP_FIRST:** 91.7% (155 de 169)
- **SL_FIRST:** 7.7% (13 de 169)
- **MFE Promedio:** 125.11 pts
- **MAE Promedio:** 10.76 pts
- **MFE/MAE Ratio:** 13.31
- **Good Entries:** 99.4% (MFE > MAE)
- **R:R Promedio:** 2.00

**✅ BUENA CALIDAD - Considerar incluir**

**2.0-3.0 ATR (Cerca)** (69 oportunidades)

- **WR Teórico:** 92.8% (si se hubieran ejecutado)
- **TP_FIRST:** 81.2% (56 de 69)
- **SL_FIRST:** 18.8% (13 de 69)
- **MFE Promedio:** 116.45 pts
- **MAE Promedio:** 11.27 pts
- **MFE/MAE Ratio:** 14.74
- **Good Entries:** 100.0% (MFE > MAE)
- **R:R Promedio:** 2.12

**✅ BUENA CALIDAD - Considerar incluir**

**3.0-5.0 ATR (Media)** (83 oportunidades)

- **WR Teórico:** 95.2% (si se hubieran ejecutado)
- **TP_FIRST:** 96.4% (80 de 83)
- **SL_FIRST:** 3.6% (3 de 83)
- **MFE Promedio:** 137.02 pts
- **MAE Promedio:** 10.67 pts
- **MFE/MAE Ratio:** 16.19
- **Good Entries:** 100.0% (MFE > MAE)
- **R:R Promedio:** 2.17

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (57 oportunidades)

- **WR Teórico:** 98.2% (si se hubieran ejecutado)
- **TP_FIRST:** 100.0% (57 de 57)
- **SL_FIRST:** 0.0% (0 de 57)
- **MFE Promedio:** 132.33 pts
- **MAE Promedio:** 9.73 pts
- **MFE/MAE Ratio:** 13.12
- **Good Entries:** 100.0% (MFE > MAE)
- **R:R Promedio:** 2.01

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (23 oportunidades)

- **WR Teórico:** 100.0% (si se hubieran ejecutado)
- **TP_FIRST:** 100.0% (23 de 23)
- **SL_FIRST:** 0.0% (0 de 23)
- **MFE Promedio:** 114.48 pts
- **MAE Promedio:** 9.52 pts
- **MFE/MAE Ratio:** 12.54
- **Good Entries:** 100.0% (MFE > MAE)
- **R:R Promedio:** 2.22

**✅ BUENA CALIDAD - Considerar incluir**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 6 | 169 | 69 | 83 |
| **TP_FIRST %** | 83.3% | 91.7% | 81.2% | 96.4% |
| **Good Entries %** | 100.0% | 99.4% | 100.0% | 100.0% |
| **MFE/MAE Ratio** | 337.85 | 13.31 | 14.74 | 16.19 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

🚨 **CRÍTICO: El rango 2.0-3.0 ATR contiene 69 oportunidades de BUENA CALIDAD**
   - WR Teórico: 92.8%
   - Good Entries: 100.0%
   - **ACCIÓN:** Considerar aumentar MaxDistanceToRegister_ATR_Normal de 2.0 a 3.0

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 83 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 95.2%
   - Good Entries: 100.0%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 15.53 pts
- **Mediana:** 12.62 pts
- **Min/Max:** 7.43 / 26.91 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 12.62 |
| P70 | 22.61 |
| P80 | 25.54 |
| P90 | 27.94 |
| P95 | 29.13 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 34.00 pts
- **Mediana:** 35.62 pts
- **Min/Max:** 18.75 / 46.25 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 35.62 |
| P70 | 42.30 |
| P80 | 44.95 |
| P90 | 47.23 |
| P95 | 48.36 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 27; // Era 60
public int MaxTPDistancePoints { get; set; } = 47; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 27.9pts, TP: 47.2pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (83.3%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.833) / 0.833
R:R_min = 0.20
```

**Estado actual:** R:R promedio = 2.43
**Gap:** -2.23 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **47** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.43) < R:R mínimo (0.20)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=83.3%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-21 09:47:04*