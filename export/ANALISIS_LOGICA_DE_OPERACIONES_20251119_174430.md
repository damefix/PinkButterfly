# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-19 17:57:22
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_174430.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_174430.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 5
- **Win Rate:** 40.0% (2/5)
- **Profit Factor:** 1.50
- **Avg R:R Planeado:** 2.83
- **R:R Mínimo para Break-Even:** 1.50

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 24 puntos
   - TP máximo observado: 49 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.83
   - R:R necesario: 1.50
   - **Gap:** -1.33

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bullish | 1163 | 37.2% |
| Neutral | 811 | 25.9% |
| Bearish | 1155 | 36.9% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.011
- **Score Min/Max:** [-1.000, 0.990]
- **Componentes (promedio):**
  - EMA20 Slope: -0.008
  - EMA50 Cross: 0.018
  - BOS Count: -0.027
  - Regression 24h: 0.077

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.990 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 25.9% Neutral → bias no diferencia tendencias

### 2.3 Estado: Bias Compuesto Funcionando Correctamente

✅ **Threshold actual:** 0.3 (correcto)
✅ **Bias Neutral:** 25.9% (aceptable)
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
| StructureFusion | 22420 | 100.0% | 100.0% |
| ProximityAnalyzer | 2997 | 13.4% | 13.4% |
| DFM_Evaluated | 373 | 12.4% | 1.7% |
| DFM_Passed | 373 | 100.0% | 1.7% |
| RiskCalculator | 5680 | 1522.8% | 25.3% |
| Risk_Accepted | 1 | 0.0% | 0.0% |
| TradeManager | 5 | 500.0% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 5679 señales, -100.0%)
- **Tasa de conversión final:** 0.02% (de 22420 zonas iniciales → 5 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,978 | 60.3% |
| ENTRY_TOO_FAR | 698 | 21.3% |
| TP_CHECK_FAIL | 336 | 10.2% |
| NO_SL | 267 | 8.1% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,978 rechazos, 60.3%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 3544 | 89.4% |
| P0_ANY_DIR | 418 | 10.6% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 46.95 pts (máxima ganancia flotante)
- **MAE Promedio:** 26.70 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 202.81

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 2 | 40.0% |
| SL_FIRST (precio fue hacia SL) | 2 | 40.0% |
| NEUTRAL (sin dirección clara) | 1 | 20.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 60.0%
- **Entradas Malas (MAE > MFE):** 40.0%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 5 | 2 | 2 | 40.0% | 46.95 | 26.70 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0004 | SELL | 2.00 | 48.25 | 0.04 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0008 | SELL | 11.25 | 40.00 | 0.28 | NEUTRAL | CLOSED | ❌ Entrada muy mala |
| T0016 | SELL | 77.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0019 | SELL | 76.75 | 40.00 | 1.92 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0022 | SELL | 67.25 | 5.25 | 12.81 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 605

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 182 | 13.2% | 14.8% | 0.77 | 15.9% | 2.45 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 98 | 20.4% | 23.5% | 0.45 | 21.4% | 2.33 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 195 | 33.3% | 43.1% | 0.68 | 35.9% | 2.54 | ❌ BAJA CALIDAD - Filtro correcto |
| 5.0-10.0 ATR (Lejos) | 110 | 31.8% | 58.2% | 0.45 | 38.2% | 2.48 | ❌ BAJA CALIDAD - Filtro correcto |
| >10.0 ATR (Muy lejos) | 20 | 0.0% | 0.0% | 0.00 | 0.0% | 2.08 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (182 oportunidades)

- **WR Teórico:** 13.2% (si se hubieran ejecutado)
- **TP_FIRST:** 14.8% (27 de 182)
- **SL_FIRST:** 83.0% (151 de 182)
- **MFE Promedio:** 49.95 pts
- **MAE Promedio:** 80.42 pts
- **MFE/MAE Ratio:** 0.77
- **Good Entries:** 15.9% (MFE > MAE)
- **R:R Promedio:** 2.45

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (98 oportunidades)

- **WR Teórico:** 20.4% (si se hubieran ejecutado)
- **TP_FIRST:** 23.5% (23 de 98)
- **SL_FIRST:** 76.5% (75 de 98)
- **MFE Promedio:** 48.16 pts
- **MAE Promedio:** 72.57 pts
- **MFE/MAE Ratio:** 0.45
- **Good Entries:** 21.4% (MFE > MAE)
- **R:R Promedio:** 2.33

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (195 oportunidades)

- **WR Teórico:** 33.3% (si se hubieran ejecutado)
- **TP_FIRST:** 43.1% (84 de 195)
- **SL_FIRST:** 56.4% (110 de 195)
- **MFE Promedio:** 58.08 pts
- **MAE Promedio:** 60.66 pts
- **MFE/MAE Ratio:** 0.68
- **Good Entries:** 35.9% (MFE > MAE)
- **R:R Promedio:** 2.54

**❌ BAJA CALIDAD - Filtro correcto**

**5.0-10.0 ATR (Lejos)** (110 oportunidades)

- **WR Teórico:** 31.8% (si se hubieran ejecutado)
- **TP_FIRST:** 58.2% (64 de 110)
- **SL_FIRST:** 41.8% (46 de 110)
- **MFE Promedio:** 60.04 pts
- **MAE Promedio:** 50.66 pts
- **MFE/MAE Ratio:** 0.45
- **Good Entries:** 38.2% (MFE > MAE)
- **R:R Promedio:** 2.48

**❌ BAJA CALIDAD - Filtro correcto**

**>10.0 ATR (Muy lejos)** (20 oportunidades)

- **WR Teórico:** 0.0% (si se hubieran ejecutado)
- **TP_FIRST:** 0.0% (0 de 20)
- **SL_FIRST:** 100.0% (20 de 20)
- **MFE Promedio:** 0.00 pts
- **MAE Promedio:** 56.27 pts
- **MFE/MAE Ratio:** 0.00
- **Good Entries:** 0.0% (MFE > MAE)
- **R:R Promedio:** 2.08

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 5 | 182 | 98 | 195 |
| **TP_FIRST %** | 40.0% | 14.8% | 23.5% | 43.1% |
| **Good Entries %** | 60.0% | 15.9% | 21.4% | 35.9% |
| **MFE/MAE Ratio** | 202.81 | 0.77 | 0.45 | 0.68 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

✅ **CORRECTO: El filtro 2.0 ATR está bloqueando 98 oportunidades de BAJA calidad**
   - WR Teórico: 20.4%
   - **ACCIÓN:** Mantener MaxDistanceToRegister_ATR_Normal = 2.0

✅ **CORRECTO: Las 195 oportunidades en 3.0-5.0 ATR tienen baja calidad**


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 12.30 pts
- **Mediana:** 13.56 pts
- **Min/Max:** 3.58 / 23.90 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 13.56 |
| P70 | 16.06 |
| P80 | 21.94 |
| P90 | 27.82 |
| P95 | 30.76 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 31.30 pts
- **Mediana:** 31.50 pts
- **Min/Max:** 10.25 / 49.25 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 31.50 |
| P70 | 41.05 |
| P80 | 47.20 |
| P90 | 53.35 |
| P95 | 56.42 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 27; // Era 60
public int MaxTPDistancePoints { get; set; } = 53; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 27.8pts, TP: 53.4pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (40.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.400) / 0.400
R:R_min = 1.50
```

**Estado actual:** R:R promedio = 2.83
**Gap:** -1.33 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **53** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.83) < R:R mínimo (1.50)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=40.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-19 17:57:22*