# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-19 16:32:34
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_162605.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_162605.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 11
- **Win Rate:** 45.5% (5/11)
- **Profit Factor:** 2.18
- **Avg R:R Planeado:** 2.71
- **R:R Mínimo para Break-Even:** 1.20

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 19 puntos
   - TP máximo observado: 48 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.71
   - R:R necesario: 1.20
   - **Gap:** -1.51

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bullish | 1166 | 37.3% |
| Neutral | 806 | 25.8% |
| Bearish | 1155 | 36.9% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.011
- **Score Min/Max:** [-1.000, 0.990]
- **Componentes (promedio):**
  - EMA20 Slope: -0.009
  - EMA50 Cross: 0.021
  - BOS Count: -0.029
  - Regression 24h: 0.078

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.990 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 25.8% Neutral → bias no diferencia tendencias

### 2.3 Estado: Bias Compuesto Funcionando Correctamente

✅ **Threshold actual:** 0.3 (correcto)
✅ **Bias Neutral:** 25.8% (aceptable)
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
| StructureFusion | 16436 | 100.0% | 100.0% |
| ProximityAnalyzer | 2320 | 14.1% | 14.1% |
| DFM_Evaluated | 491 | 21.2% | 3.0% |
| DFM_Passed | 491 | 100.0% | 3.0% |
| RiskCalculator | 4551 | 926.9% | 27.7% |
| Risk_Accepted | 45 | 1.0% | 0.3% |
| TradeManager | 11 | 24.4% | 0.1% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 4506 señales, -99.0%)
- **Tasa de conversión final:** 0.07% (de 16436 zonas iniciales → 11 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,507 | 64.8% |
| ENTRY_TOO_FAR | 544 | 23.4% |
| NO_SL | 162 | 7.0% |
| TP_CHECK_FAIL | 114 | 4.9% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,507 rechazos, 64.8%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2994 | 86.0% |
| P0_ANY_DIR | 489 | 14.0% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 25.66 pts (máxima ganancia flotante)
- **MAE Promedio:** 48.68 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 92.13

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 3 | 27.3% |
| SL_FIRST (precio fue hacia SL) | 8 | 72.7% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 36.4%
- **Entradas Malas (MAE > MFE):** 63.6%

⚠️ **ALERTA:** >60% de entradas tienen MAE > MFE
- **Problema:** El precio va más en contra que a favor antes del cierre
- **Causas posibles:**
  1. Timing incorrecto (entramos antes de reversión)
  2. Bias desincronizado (operamos contra tendencia real)
  3. Zonas de baja calidad (sin confluence real)

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 11 | 3 | 8 | 27.3% | 25.66 | 48.68 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0005 | SELL | 0.00 | 51.00 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0007 | SELL | 0.00 | 61.00 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0010 | SELL | 0.00 | 130.00 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0011 | SELL | 0.00 | 130.00 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0018 | SELL | 23.50 | 28.75 | 0.82 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0027 | SELL | 77.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0034 | SELL | 9.75 | 45.50 | 0.21 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0034_2 | SELL | 9.75 | 45.50 | 0.21 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0037 | SELL | 49.75 | 23.50 | 2.12 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0038 | SELL | 50.50 | 9.25 | 5.46 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0044 | SELL | 61.50 | 11.00 | 5.59 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 708

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 220 | 8.2% | 11.4% | 1.78 | 10.9% | 2.01 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 117 | 19.7% | 23.1% | 0.69 | 17.1% | 1.97 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 243 | 25.1% | 27.2% | 0.57 | 24.3% | 2.06 | ❌ BAJA CALIDAD - Filtro correcto |
| 5.0-10.0 ATR (Lejos) | 112 | 25.9% | 34.8% | 0.37 | 24.1% | 2.14 | ❌ BAJA CALIDAD - Filtro correcto |
| >10.0 ATR (Muy lejos) | 16 | 0.0% | 0.0% | 0.01 | 0.0% | 2.51 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (220 oportunidades)

- **WR Teórico:** 8.2% (si se hubieran ejecutado)
- **TP_FIRST:** 11.4% (25 de 220)
- **SL_FIRST:** 88.2% (194 de 220)
- **MFE Promedio:** 53.01 pts
- **MAE Promedio:** 86.30 pts
- **MFE/MAE Ratio:** 1.78
- **Good Entries:** 10.9% (MFE > MAE)
- **R:R Promedio:** 2.01

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (117 oportunidades)

- **WR Teórico:** 19.7% (si se hubieran ejecutado)
- **TP_FIRST:** 23.1% (27 de 117)
- **SL_FIRST:** 76.9% (90 de 117)
- **MFE Promedio:** 61.95 pts
- **MAE Promedio:** 78.81 pts
- **MFE/MAE Ratio:** 0.69
- **Good Entries:** 17.1% (MFE > MAE)
- **R:R Promedio:** 1.97

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (243 oportunidades)

- **WR Teórico:** 25.1% (si se hubieran ejecutado)
- **TP_FIRST:** 27.2% (66 de 243)
- **SL_FIRST:** 72.8% (177 de 243)
- **MFE Promedio:** 81.17 pts
- **MAE Promedio:** 71.70 pts
- **MFE/MAE Ratio:** 0.57
- **Good Entries:** 24.3% (MFE > MAE)
- **R:R Promedio:** 2.06

**❌ BAJA CALIDAD - Filtro correcto**

**5.0-10.0 ATR (Lejos)** (112 oportunidades)

- **WR Teórico:** 25.9% (si se hubieran ejecutado)
- **TP_FIRST:** 34.8% (39 de 112)
- **SL_FIRST:** 65.2% (73 de 112)
- **MFE Promedio:** 75.44 pts
- **MAE Promedio:** 73.24 pts
- **MFE/MAE Ratio:** 0.37
- **Good Entries:** 24.1% (MFE > MAE)
- **R:R Promedio:** 2.14

**❌ BAJA CALIDAD - Filtro correcto**

**>10.0 ATR (Muy lejos)** (16 oportunidades)

- **WR Teórico:** 0.0% (si se hubieran ejecutado)
- **TP_FIRST:** 0.0% (0 de 16)
- **SL_FIRST:** 100.0% (16 de 16)
- **MFE Promedio:** 4.00 pts
- **MAE Promedio:** 80.80 pts
- **MFE/MAE Ratio:** 0.01
- **Good Entries:** 0.0% (MFE > MAE)
- **R:R Promedio:** 2.51

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 11 | 220 | 117 | 243 |
| **TP_FIRST %** | 27.3% | 11.4% | 23.1% | 27.2% |
| **Good Entries %** | 36.4% | 10.9% | 17.1% | 24.3% |
| **MFE/MAE Ratio** | 92.13 | 1.78 | 0.69 | 0.57 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

✅ **CORRECTO: El filtro 2.0 ATR está bloqueando 117 oportunidades de BAJA calidad**
   - WR Teórico: 19.7%
   - **ACCIÓN:** Mantener MaxDistanceToRegister_ATR_Normal = 2.0

✅ **CORRECTO: Las 243 oportunidades en 3.0-5.0 ATR tienen baja calidad**


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 11.80 pts
- **Mediana:** 10.89 pts
- **Min/Max:** 3.58 / 18.52 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 10.89 |
| P70 | 13.64 |
| P80 | 16.73 |
| P90 | 18.52 |
| P95 | 18.52 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 29.91 pts
- **Mediana:** 30.25 pts
- **Min/Max:** 10.25 / 47.75 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 30.25 |
| P70 | 31.70 |
| P80 | 39.20 |
| P90 | 46.80 |
| P95 | 49.65 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 18; // Era 60
public int MaxTPDistancePoints { get; set; } = 46; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 18.5pts, TP: 46.8pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (45.5%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.455) / 0.455
R:R_min = 1.20
```

**Estado actual:** R:R promedio = 2.71
**Gap:** -1.51 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **46** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.71) < R:R mínimo (1.20)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=45.5%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-19 16:32:34*