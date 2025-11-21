# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-20 19:48:27
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251120_193930.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251120_193930.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 3
- **Win Rate:** 66.7% (2/3)
- **Profit Factor:** 2.77
- **Avg R:R Planeado:** 1.76
- **R:R Mínimo para Break-Even:** 0.50

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 18 puntos
   - TP máximo observado: 34 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.76
   - R:R necesario: 0.50
   - **Gap:** -1.26

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bullish | 1069 | 34.2% |
| Neutral | 902 | 28.8% |
| Bearish | 1158 | 37.0% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.001
- **Score Min/Max:** [-1.000, 0.990]
- **Componentes (promedio):**
  - EMA20 Slope: 0.007
  - EMA50 Cross: 0.003
  - BOS Count: -0.082
  - Regression 24h: 0.090

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.990 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 28.8% Neutral → bias no diferencia tendencias

### 2.3 Estado: Bias Compuesto Funcionando Correctamente

✅ **Threshold actual:** 0.3 (correcto)
✅ **Bias Neutral:** 28.8% (aceptable)
✅ **Score promedio:** 0.001

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 26381 | 100.0% | 100.0% |
| ProximityAnalyzer | 2881 | 10.9% | 10.9% |
| DFM_Evaluated | 515 | 17.9% | 2.0% |
| DFM_Passed | 515 | 100.0% | 2.0% |
| RiskCalculator | 5741 | 1114.8% | 21.8% |
| Risk_Accepted | 1 | 0.0% | 0.0% |
| TradeManager | 3 | 300.0% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 5740 señales, -100.0%)
- **Tasa de conversión final:** 0.01% (de 26381 zonas iniciales → 3 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 2,339 | 64.9% |
| ENTRY_TOO_FAR | 861 | 23.9% |
| NO_SL | 223 | 6.2% |
| TP_CHECK_FAIL | 179 | 5.0% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (2,339 rechazos, 64.9%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2138 | 68.9% |
| P0_ANY_DIR | 966 | 31.1% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 68.25 pts (máxima ganancia flotante)
- **MAE Promedio:** 18.33 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 6.51

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 3 | 100.0% |
| SL_FIRST (precio fue hacia SL) | 0 | 0.0% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 100.0%
- **Entradas Malas (MAE > MFE):** 0.0%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 3 | 3 | 0 | 100.0% | 68.25 | 18.33 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0016 | SELL | 49.75 | 38.00 | 1.31 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0018 | SELL | 77.50 | 8.50 | 9.12 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0018_2 | SELL | 77.50 | 8.50 | 9.12 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 788

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 206 | 8.7% | 60.7% | 2.94 | 65.0% | 2.39 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 107 | 14.0% | 48.6% | 1.57 | 49.5% | 2.35 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 220 | 16.4% | 50.0% | 1.36 | 48.2% | 2.45 | ❌ BAJA CALIDAD - Filtro correcto |
| 5.0-10.0 ATR (Lejos) | 193 | 9.3% | 36.8% | 1.20 | 36.3% | 2.56 | ❌ BAJA CALIDAD - Filtro correcto |
| >10.0 ATR (Muy lejos) | 62 | 0.0% | 59.7% | 1.63 | 59.7% | 2.80 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (206 oportunidades)

- **WR Teórico:** 8.7% (si se hubieran ejecutado)
- **TP_FIRST:** 60.7% (125 de 206)
- **SL_FIRST:** 28.6% (59 de 206)
- **MFE Promedio:** 27.42 pts
- **MAE Promedio:** 17.42 pts
- **MFE/MAE Ratio:** 2.94
- **Good Entries:** 65.0% (MFE > MAE)
- **R:R Promedio:** 2.39

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (107 oportunidades)

- **WR Teórico:** 14.0% (si se hubieran ejecutado)
- **TP_FIRST:** 48.6% (52 de 107)
- **SL_FIRST:** 34.6% (37 de 107)
- **MFE Promedio:** 29.74 pts
- **MAE Promedio:** 21.09 pts
- **MFE/MAE Ratio:** 1.57
- **Good Entries:** 49.5% (MFE > MAE)
- **R:R Promedio:** 2.35

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (220 oportunidades)

- **WR Teórico:** 16.4% (si se hubieran ejecutado)
- **TP_FIRST:** 50.0% (110 de 220)
- **SL_FIRST:** 37.7% (83 de 220)
- **MFE Promedio:** 33.54 pts
- **MAE Promedio:** 22.61 pts
- **MFE/MAE Ratio:** 1.36
- **Good Entries:** 48.2% (MFE > MAE)
- **R:R Promedio:** 2.45

**❌ BAJA CALIDAD - Filtro correcto**

**5.0-10.0 ATR (Lejos)** (193 oportunidades)

- **WR Teórico:** 9.3% (si se hubieran ejecutado)
- **TP_FIRST:** 36.8% (71 de 193)
- **SL_FIRST:** 42.0% (81 de 193)
- **MFE Promedio:** 27.79 pts
- **MAE Promedio:** 21.97 pts
- **MFE/MAE Ratio:** 1.20
- **Good Entries:** 36.3% (MFE > MAE)
- **R:R Promedio:** 2.56

**❌ BAJA CALIDAD - Filtro correcto**

**>10.0 ATR (Muy lejos)** (62 oportunidades)

- **WR Teórico:** 0.0% (si se hubieran ejecutado)
- **TP_FIRST:** 59.7% (37 de 62)
- **SL_FIRST:** 19.4% (12 de 62)
- **MFE Promedio:** 21.11 pts
- **MAE Promedio:** 17.14 pts
- **MFE/MAE Ratio:** 1.63
- **Good Entries:** 59.7% (MFE > MAE)
- **R:R Promedio:** 2.80

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 3 | 206 | 107 | 220 |
| **TP_FIRST %** | 100.0% | 60.7% | 48.6% | 50.0% |
| **Good Entries %** | 100.0% | 65.0% | 49.5% | 48.2% |
| **MFE/MAE Ratio** | 6.51 | 2.94 | 1.57 | 1.36 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

✅ **CORRECTO: El filtro 2.0 ATR está bloqueando 107 oportunidades de BAJA calidad**
   - WR Teórico: 14.0%
   - **ACCIÓN:** Mantener MaxDistanceToRegister_ATR_Normal = 2.0

✅ **CORRECTO: Las 220 oportunidades en 3.0-5.0 ATR tienen baja calidad**


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 17.99 pts
- **Mediana:** 18.24 pts
- **Min/Max:** 17.50 / 18.24 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 18.24 |
| P70 | 18.24 |
| P80 | 18.24 |
| P90 | 18.24 |
| P95 | 18.24 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 31.58 pts
- **Mediana:** 30.25 pts
- **Min/Max:** 30.25 / 34.25 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 30.25 |
| P70 | 33.45 |
| P80 | 35.05 |
| P90 | 36.65 |
| P95 | 37.45 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 18; // Era 60
public int MaxTPDistancePoints { get; set; } = 36; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 18.2pts, TP: 36.6pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (66.7%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.667) / 0.667
R:R_min = 0.50
```

**Estado actual:** R:R promedio = 1.76
**Gap:** -1.26 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **36** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.76) < R:R mínimo (0.50)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=66.7%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-20 19:48:27*