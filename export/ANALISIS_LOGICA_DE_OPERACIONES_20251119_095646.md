# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-19 10:01:47
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_095646.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_095646.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 13
- **Win Rate:** 23.1% (3/13)
- **Profit Factor:** 0.70
- **Avg R:R Planeado:** 1.87
- **R:R Mínimo para Break-Even:** 3.33

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 27 puntos
   - TP máximo observado: 44 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.87
   - R:R necesario: 3.33
   - **Gap:** 1.46

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bullish | 188 | 30.0% |
| Neutral | 181 | 28.9% |
| Bearish | 257 | 41.1% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** -0.090
- **Score Min/Max:** [-0.950, 0.930]
- **Componentes (promedio):**
  - EMA20 Slope: -0.143
  - EMA50 Cross: -0.093
  - BOS Count: -0.055
  - Regression 24h: -0.050

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.930 (apenas supera threshold)
- Score mínimo observado: -0.950 (apenas supera threshold)
- **Consecuencia:** Sistema queda 28.9% Neutral → bias no diferencia tendencias

### 2.3 Estado: Bias Compuesto Funcionando Correctamente

✅ **Threshold actual:** 0.3 (correcto)
✅ **Bias Neutral:** 28.9% (aceptable)
✅ **Score promedio:** -0.090

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 4220 | 100.0% | 100.0% |
| ProximityAnalyzer | 1275 | 30.2% | 30.2% |
| DFM_Evaluated | 300 | 23.5% | 7.1% |
| DFM_Passed | 300 | 100.0% | 7.1% |
| RiskCalculator | 2135 | 711.7% | 50.6% |
| Risk_Accepted | 44 | 2.1% | 1.0% |
| TradeManager | 13 | 29.5% | 0.3% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 2091 señales, -97.9%)
- **Tasa de conversión final:** 0.31% (de 4220 zonas iniciales → 13 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 705 | 59.8% |
| ENTRY_TOO_FAR | 271 | 23.0% |
| NO_SL | 166 | 14.1% |
| TP_CHECK_FAIL | 37 | 3.1% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (705 rechazos, 59.8%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 1219 | 80.5% |
| P0_ANY_DIR | 295 | 19.5% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 39.29 pts (máxima ganancia flotante)
- **MAE Promedio:** 35.21 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 79.74

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 7 | 53.8% |
| SL_FIRST (precio fue hacia SL) | 5 | 38.5% |
| NEUTRAL (sin dirección clara) | 1 | 7.7% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 46.2%
- **Entradas Malas (MAE > MFE):** 53.8%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 13 | 7 | 5 | 53.8% | 39.29 | 35.21 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0009 | SELL | 21.25 | 30.00 | 0.71 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0009_2 | SELL | 21.25 | 30.00 | 0.71 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0009_3 | SELL | 21.25 | 30.00 | 0.71 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0009_4 | SELL | 21.25 | 30.00 | 0.71 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0011 | SELL | 39.50 | 11.25 | 3.51 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0015 | SELL | 86.00 | 4.00 | 21.50 | NEUTRAL | CLOSED | ✅ Entrada excelente |
| T0022 | SELL | 0.00 | 97.75 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0022_2 | SELL | 0.00 | 97.75 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0024 | SELL | 9.75 | 45.50 | 0.21 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0025 | SELL | 69.75 | 35.25 | 1.98 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0025_2 | SELL | 69.75 | 35.25 | 1.98 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0027 | SELL | 89.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0043 | SELL | 61.50 | 11.00 | 5.59 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 456

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 162 | 69.1% | 75.3% | 4.17 | 77.2% | 1.98 | ✅ BUENA CALIDAD - Considerar incluir |
| 2.0-3.0 ATR (Cerca) | 85 | 81.2% | 84.7% | 4.65 | 72.9% | 2.03 | ✅ BUENA CALIDAD - Considerar incluir |
| 3.0-5.0 ATR (Media) | 145 | 86.2% | 85.5% | 10.83 | 82.8% | 1.97 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 59 | 81.4% | 79.7% | 1.14 | 81.4% | 2.14 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 5 | 0.0% | 0.0% | 0.00 | 0.0% | 1.34 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (162 oportunidades)

- **WR Teórico:** 69.1% (si se hubieran ejecutado)
- **TP_FIRST:** 75.3% (122 de 162)
- **SL_FIRST:** 20.4% (33 de 162)
- **MFE Promedio:** 71.43 pts
- **MAE Promedio:** 43.17 pts
- **MFE/MAE Ratio:** 4.17
- **Good Entries:** 77.2% (MFE > MAE)
- **R:R Promedio:** 1.98

**✅ BUENA CALIDAD - Considerar incluir**

**2.0-3.0 ATR (Cerca)** (85 oportunidades)

- **WR Teórico:** 81.2% (si se hubieran ejecutado)
- **TP_FIRST:** 84.7% (72 de 85)
- **SL_FIRST:** 15.3% (13 de 85)
- **MFE Promedio:** 73.97 pts
- **MAE Promedio:** 35.29 pts
- **MFE/MAE Ratio:** 4.65
- **Good Entries:** 72.9% (MFE > MAE)
- **R:R Promedio:** 2.03

**✅ BUENA CALIDAD - Considerar incluir**

**3.0-5.0 ATR (Media)** (145 oportunidades)

- **WR Teórico:** 86.2% (si se hubieran ejecutado)
- **TP_FIRST:** 85.5% (124 de 145)
- **SL_FIRST:** 14.5% (21 de 145)
- **MFE Promedio:** 87.48 pts
- **MAE Promedio:** 40.89 pts
- **MFE/MAE Ratio:** 10.83
- **Good Entries:** 82.8% (MFE > MAE)
- **R:R Promedio:** 1.97

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (59 oportunidades)

- **WR Teórico:** 81.4% (si se hubieran ejecutado)
- **TP_FIRST:** 79.7% (47 de 59)
- **SL_FIRST:** 20.3% (12 de 59)
- **MFE Promedio:** 108.81 pts
- **MAE Promedio:** 63.95 pts
- **MFE/MAE Ratio:** 1.14
- **Good Entries:** 81.4% (MFE > MAE)
- **R:R Promedio:** 2.14

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (5 oportunidades)

- **WR Teórico:** 0.0% (si se hubieran ejecutado)
- **TP_FIRST:** 0.0% (0 de 5)
- **SL_FIRST:** 100.0% (5 de 5)
- **MFE Promedio:** 0.00 pts
- **MAE Promedio:** 129.65 pts
- **MFE/MAE Ratio:** 0.00
- **Good Entries:** 0.0% (MFE > MAE)
- **R:R Promedio:** 1.34

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 13 | 162 | 85 | 145 |
| **TP_FIRST %** | 53.8% | 75.3% | 84.7% | 85.5% |
| **Good Entries %** | 46.2% | 77.2% | 72.9% | 82.8% |
| **MFE/MAE Ratio** | 79.74 | 4.17 | 4.65 | 10.83 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

🚨 **CRÍTICO: El rango 2.0-3.0 ATR contiene 85 oportunidades de BUENA CALIDAD**
   - WR Teórico: 81.2%
   - Good Entries: 72.9%
   - **ACCIÓN:** Considerar aumentar MaxDistanceToRegister_ATR_Normal de 2.0 a 3.0

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 145 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 86.2%
   - Good Entries: 82.8%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 15.78 pts
- **Mediana:** 15.20 pts
- **Min/Max:** 5.15 / 27.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 15.20 |
| P70 | 17.86 |
| P80 | 20.75 |
| P90 | 24.50 |
| P95 | 28.88 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 28.67 pts
- **Mediana:** 26.50 pts
- **Min/Max:** 11.50 / 44.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 26.50 |
| P70 | 35.50 |
| P80 | 42.25 |
| P90 | 43.30 |
| P95 | 44.52 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 24; // Era 60
public int MaxTPDistancePoints { get; set; } = 43; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 24.5pts, TP: 43.3pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (23.1%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.231) / 0.231
R:R_min = 3.33
```

**Estado actual:** R:R promedio = 1.87
**Gap:** 1.46 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **24** (P90 real)
2. **MaxTPDistancePoints:** 120 → **43** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.87) < R:R mínimo (3.33)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=23.1%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-19 10:01:47*