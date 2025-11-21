# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-19 15:43:42
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_153900.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_153900.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 4
- **Win Rate:** 50.0% (2/4)
- **Profit Factor:** 1.99
- **Avg R:R Planeado:** 2.35
- **R:R Mínimo para Break-Even:** 1.00

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 11 puntos
   - TP máximo observado: 28 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.35
   - R:R necesario: 1.00
   - **Gap:** -1.35

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bullish | 1170 | 37.4% |
| Neutral | 803 | 25.7% |
| Bearish | 1155 | 36.9% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.012
- **Score Min/Max:** [-1.000, 0.990]
- **Componentes (promedio):**
  - EMA20 Slope: -0.009
  - EMA50 Cross: 0.023
  - BOS Count: -0.028
  - Regression 24h: 0.079

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.990 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 25.7% Neutral → bias no diferencia tendencias

### 2.3 Estado: Bias Compuesto Funcionando Correctamente

✅ **Threshold actual:** 0.3 (correcto)
✅ **Bias Neutral:** 25.7% (aceptable)
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
| StructureFusion | 18943 | 100.0% | 100.0% |
| ProximityAnalyzer | 3277 | 17.3% | 17.3% |
| DFM_Evaluated | 622 | 19.0% | 3.3% |
| DFM_Passed | 622 | 100.0% | 3.3% |
| RiskCalculator | 5824 | 936.3% | 30.7% |
| Risk_Accepted | 39 | 0.7% | 0.2% |
| TradeManager | 4 | 10.3% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 5785 señales, -99.3%)
- **Tasa de conversión final:** 0.02% (de 18943 zonas iniciales → 4 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,407 | 53.7% |
| ENTRY_TOO_FAR | 766 | 29.3% |
| TP_CHECK_FAIL | 241 | 9.2% |
| NO_SL | 204 | 7.8% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,407 rechazos, 53.7%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 3863 | 80.7% |
| P0_ANY_DIR | 925 | 19.3% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 67.38 pts (máxima ganancia flotante)
- **MAE Promedio:** 26.38 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 3.21

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 0 | 0.0% |
| SL_FIRST (precio fue hacia SL) | 4 | 100.0% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 100.0%
- **Entradas Malas (MAE > MFE):** 0.0%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 4 | 0 | 4 | 0.0% | 67.38 | 26.38 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0008 | SELL | 66.25 | 38.25 | 1.73 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0009 | SELL | 83.25 | 21.25 | 3.92 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0012 | SELL | 68.25 | 36.25 | 1.88 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0018 | SELL | 51.75 | 9.75 | 5.31 | SL_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 942

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 258 | 54.7% | 19.8% | 2.96 | 58.9% | 2.25 | ⚠️ CALIDAD MEDIA - Revisar |
| 2.0-3.0 ATR (Cerca) | 169 | 62.1% | 29.6% | 3.32 | 69.2% | 2.16 | ✅ BUENA CALIDAD - Considerar incluir |
| 3.0-5.0 ATR (Media) | 318 | 68.2% | 32.7% | 4.34 | 78.3% | 2.24 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 161 | 86.3% | 36.0% | 5.53 | 95.7% | 2.18 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 36 | 100.0% | 0.0% | 3.79 | 100.0% | 2.77 | ✅ BUENA CALIDAD - Considerar incluir |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (258 oportunidades)

- **WR Teórico:** 54.7% (si se hubieran ejecutado)
- **TP_FIRST:** 19.8% (51 de 258)
- **SL_FIRST:** 78.7% (203 de 258)
- **MFE Promedio:** 76.44 pts
- **MAE Promedio:** 61.79 pts
- **MFE/MAE Ratio:** 2.96
- **Good Entries:** 58.9% (MFE > MAE)
- **R:R Promedio:** 2.25

**⚠️ CALIDAD MEDIA - Revisar**

**2.0-3.0 ATR (Cerca)** (169 oportunidades)

- **WR Teórico:** 62.1% (si se hubieran ejecutado)
- **TP_FIRST:** 29.6% (50 de 169)
- **SL_FIRST:** 69.8% (118 de 169)
- **MFE Promedio:** 87.21 pts
- **MAE Promedio:** 54.71 pts
- **MFE/MAE Ratio:** 3.32
- **Good Entries:** 69.2% (MFE > MAE)
- **R:R Promedio:** 2.16

**✅ BUENA CALIDAD - Considerar incluir**

**3.0-5.0 ATR (Media)** (318 oportunidades)

- **WR Teórico:** 68.2% (si se hubieran ejecutado)
- **TP_FIRST:** 32.7% (104 de 318)
- **SL_FIRST:** 67.3% (214 de 318)
- **MFE Promedio:** 83.13 pts
- **MAE Promedio:** 47.60 pts
- **MFE/MAE Ratio:** 4.34
- **Good Entries:** 78.3% (MFE > MAE)
- **R:R Promedio:** 2.24

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (161 oportunidades)

- **WR Teórico:** 86.3% (si se hubieran ejecutado)
- **TP_FIRST:** 36.0% (58 de 161)
- **SL_FIRST:** 64.0% (103 de 161)
- **MFE Promedio:** 92.75 pts
- **MAE Promedio:** 29.48 pts
- **MFE/MAE Ratio:** 5.53
- **Good Entries:** 95.7% (MFE > MAE)
- **R:R Promedio:** 2.18

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (36 oportunidades)

- **WR Teórico:** 100.0% (si se hubieran ejecutado)
- **TP_FIRST:** 0.0% (0 de 36)
- **SL_FIRST:** 100.0% (36 de 36)
- **MFE Promedio:** 78.47 pts
- **MAE Promedio:** 26.03 pts
- **MFE/MAE Ratio:** 3.79
- **Good Entries:** 100.0% (MFE > MAE)
- **R:R Promedio:** 2.77

**✅ BUENA CALIDAD - Considerar incluir**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 4 | 258 | 169 | 318 |
| **TP_FIRST %** | 0.0% | 19.8% | 29.6% | 32.7% |
| **Good Entries %** | 100.0% | 58.9% | 69.2% | 78.3% |
| **MFE/MAE Ratio** | 3.21 | 2.96 | 3.32 | 4.34 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

🚨 **CRÍTICO: El rango 2.0-3.0 ATR contiene 169 oportunidades de BUENA CALIDAD**
   - WR Teórico: 62.1%
   - Good Entries: 69.2%
   - **ACCIÓN:** Considerar aumentar MaxDistanceToRegister_ATR_Normal de 2.0 a 3.0

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 318 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 68.2%
   - Good Entries: 78.3%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 9.19 pts
- **Mediana:** 9.19 pts
- **Min/Max:** 7.13 / 11.24 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 9.19 |
| P70 | 10.40 |
| P80 | 11.24 |
| P90 | 12.07 |
| P95 | 12.49 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 21.69 pts
- **Mediana:** 22.62 pts
- **Min/Max:** 13.00 / 28.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 22.62 |
| P70 | 27.25 |
| P80 | 28.50 |
| P90 | 29.75 |
| P95 | 30.38 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 12; // Era 60
public int MaxTPDistancePoints { get; set; } = 29; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 12.1pts, TP: 29.8pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (50.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.500) / 0.500
R:R_min = 1.00
```

**Estado actual:** R:R promedio = 2.35
**Gap:** -1.35 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **29** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.35) < R:R mínimo (1.00)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=50.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-19 15:43:42*