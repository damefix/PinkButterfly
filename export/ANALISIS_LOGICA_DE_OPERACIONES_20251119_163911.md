# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-19 16:45:26
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_163911.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_163911.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 11
- **Win Rate:** 45.5% (5/11)
- **Profit Factor:** 2.80
- **Avg R:R Planeado:** 22.50
- **R:R Mínimo para Break-Even:** 1.20

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 24 puntos
   - TP máximo observado: 48 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 22.50
   - R:R necesario: 1.20
   - **Gap:** -21.30

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bullish | 1166 | 37.3% |
| Neutral | 807 | 25.8% |
| Bearish | 1155 | 36.9% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.011
- **Score Min/Max:** [-1.000, 0.990]
- **Componentes (promedio):**
  - EMA20 Slope: -0.009
  - EMA50 Cross: 0.020
  - BOS Count: -0.028
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
| StructureFusion | 16333 | 100.0% | 100.0% |
| ProximityAnalyzer | 2282 | 14.0% | 14.0% |
| DFM_Evaluated | 480 | 21.0% | 2.9% |
| DFM_Passed | 480 | 100.0% | 2.9% |
| RiskCalculator | 4528 | 943.3% | 27.7% |
| Risk_Accepted | 44 | 1.0% | 0.3% |
| TradeManager | 11 | 25.0% | 0.1% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 4484 señales, -99.0%)
- **Tasa de conversión final:** 0.07% (de 16333 zonas iniciales → 11 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,455 | 63.4% |
| ENTRY_TOO_FAR | 539 | 23.5% |
| NO_SL | 161 | 7.0% |
| TP_CHECK_FAIL | 141 | 6.1% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,455 rechazos, 63.4%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 3030 | 86.4% |
| P0_ANY_DIR | 475 | 13.6% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 29.18 pts (máxima ganancia flotante)
- **MAE Promedio:** 48.66 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 92.21

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 3 | 27.3% |
| SL_FIRST (precio fue hacia SL) | 8 | 72.7% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 45.5%
- **Entradas Malas (MAE > MFE):** 54.5%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 11 | 3 | 8 | 27.3% | 29.18 | 48.66 |

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
| T0035 | SELL | 48.50 | 45.25 | 1.07 | SL_FIRST | CLOSED | 👍 Entrada correcta |
| T0036 | SELL | 49.75 | 23.50 | 2.12 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0037 | SELL | 50.50 | 9.25 | 5.46 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0043 | SELL | 61.50 | 11.00 | 5.59 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 690

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 214 | 7.9% | 11.7% | 0.19 | 9.8% | 2.00 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 116 | 21.6% | 25.9% | 0.56 | 19.8% | 1.98 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 235 | 23.4% | 26.4% | 0.58 | 23.0% | 2.11 | ❌ BAJA CALIDAD - Filtro correcto |
| 5.0-10.0 ATR (Lejos) | 109 | 26.6% | 35.8% | 0.24 | 24.8% | 2.20 | ❌ BAJA CALIDAD - Filtro correcto |
| >10.0 ATR (Muy lejos) | 16 | 0.0% | 0.0% | 0.01 | 0.0% | 2.51 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (214 oportunidades)

- **WR Teórico:** 7.9% (si se hubieran ejecutado)
- **TP_FIRST:** 11.7% (25 de 214)
- **SL_FIRST:** 88.3% (189 de 214)
- **MFE Promedio:** 56.38 pts
- **MAE Promedio:** 89.59 pts
- **MFE/MAE Ratio:** 0.19
- **Good Entries:** 9.8% (MFE > MAE)
- **R:R Promedio:** 2.00

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (116 oportunidades)

- **WR Teórico:** 21.6% (si se hubieran ejecutado)
- **TP_FIRST:** 25.9% (30 de 116)
- **SL_FIRST:** 74.1% (86 de 116)
- **MFE Promedio:** 67.48 pts
- **MAE Promedio:** 80.48 pts
- **MFE/MAE Ratio:** 0.56
- **Good Entries:** 19.8% (MFE > MAE)
- **R:R Promedio:** 1.98

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (235 oportunidades)

- **WR Teórico:** 23.4% (si se hubieran ejecutado)
- **TP_FIRST:** 26.4% (62 de 235)
- **SL_FIRST:** 73.6% (173 de 235)
- **MFE Promedio:** 79.86 pts
- **MAE Promedio:** 72.10 pts
- **MFE/MAE Ratio:** 0.58
- **Good Entries:** 23.0% (MFE > MAE)
- **R:R Promedio:** 2.11

**❌ BAJA CALIDAD - Filtro correcto**

**5.0-10.0 ATR (Lejos)** (109 oportunidades)

- **WR Teórico:** 26.6% (si se hubieran ejecutado)
- **TP_FIRST:** 35.8% (39 de 109)
- **SL_FIRST:** 64.2% (70 de 109)
- **MFE Promedio:** 82.08 pts
- **MAE Promedio:** 74.47 pts
- **MFE/MAE Ratio:** 0.24
- **Good Entries:** 24.8% (MFE > MAE)
- **R:R Promedio:** 2.20

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
| **Count** | 11 | 214 | 116 | 235 |
| **TP_FIRST %** | 27.3% | 11.7% | 25.9% | 26.4% |
| **Good Entries %** | 45.5% | 9.8% | 19.8% | 23.0% |
| **MFE/MAE Ratio** | 92.21 | 0.19 | 0.56 | 0.58 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

✅ **CORRECTO: El filtro 2.0 ATR está bloqueando 116 oportunidades de BAJA calidad**
   - WR Teórico: 21.6%
   - **ACCIÓN:** Mantener MaxDistanceToRegister_ATR_Normal = 2.0

✅ **CORRECTO: Las 235 oportunidades en 3.0-5.0 ATR tienen baja calidad**


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 10.71 pts
- **Mediana:** 10.89 pts
- **Min/Max:** 0.25 / 24.24 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 10.89 |
| P70 | 13.64 |
| P80 | 16.73 |
| P90 | 23.10 |
| P95 | 26.53 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 30.52 pts
- **Mediana:** 30.25 pts
- **Min/Max:** 10.25 / 47.75 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 30.25 |
| P70 | 34.90 |
| P80 | 40.60 |
| P90 | 46.80 |
| P95 | 49.65 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 23; // Era 60
public int MaxTPDistancePoints { get; set; } = 46; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 23.1pts, TP: 46.8pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (45.5%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.455) / 0.455
R:R_min = 1.20
```

**Estado actual:** R:R promedio = 22.50
**Gap:** -21.30 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **23** (P90 real)
2. **MaxTPDistancePoints:** 120 → **46** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (22.50) < R:R mínimo (1.20)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=45.5%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-19 16:45:26*