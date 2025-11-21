# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-14 16:34:31
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_162737.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251114_162737.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 46
- **Win Rate:** 37.0% (17/46)
- **Profit Factor:** 0.80
- **Avg R:R Planeado:** 1.98
- **R:R Mínimo para Break-Even:** 1.71

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 38 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.98
   - R:R necesario: 1.71
   - **Gap:** -0.27

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8016 | 34.1% |
| Bearish | 6384 | 27.2% |
| Bullish | 9112 | 38.8% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.079
- **Score Min/Max:** [-0.990, 0.980]
- **Componentes (promedio):**
  - EMA20 Slope: 0.039
  - EMA50 Cross: 0.195
  - BOS Count: 0.007
  - Regression 24h: 0.087

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.980 (apenas supera threshold)
- Score mínimo observado: -0.990 (apenas supera threshold)
- **Consecuencia:** Sistema queda 34.1% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.1%)

**Posibles causas:**
- **BOS Score bajo (0.007):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio 0.079 indica poca señal direccional
- **Mercado lateral:** Scores reales [-0.99, 0.98] muy cercanos a 0

**Recomendaciones:**
1. ✅ Verificar que `BOSDetector.cs` establece `Type = breakType` (bug conocido)
2. ✅ Revisar logs para confirmar que BOS Score != 0.0
3. ⚠️ Si BOS sigue en ~0, investigar detección de BOS/CHoCH
4. ⚠️ Considerar bajar threshold a 0.2 SOLO si los 3 pasos anteriores están OK

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 10471 | 100.0% | 100.0% |
| ProximityAnalyzer | 3640 | 34.8% | 34.8% |
| DFM_Evaluated | 852 | 23.4% | 8.1% |
| DFM_Passed | 852 | 100.0% | 8.1% |
| RiskCalculator | 6115 | 717.7% | 58.4% |
| Risk_Accepted | 2 | 0.0% | 0.0% |
| TradeManager | 46 | 2300.0% | 0.4% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 6113 señales, -100.0%)
- **Tasa de conversión final:** 0.44% (de 10471 zonas iniciales → 46 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,356 | 51.2% |
| NO_SL | 663 | 25.0% |
| ENTRY_TOO_FAR | 406 | 15.3% |
| TP_CHECK_FAIL | 226 | 8.5% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,356 rechazos, 51.2%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2533 | 88.5% |
| P0_ANY_DIR | 329 | 11.5% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 45.94 pts (máxima ganancia flotante)
- **MAE Promedio:** 36.27 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 68.09

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 19 | 41.3% |
| SL_FIRST (precio fue hacia SL) | 25 | 54.3% |
| NEUTRAL (sin dirección clara) | 2 | 4.3% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 45.7%
- **Entradas Malas (MAE > MFE):** 54.3%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 46 | 19 | 25 | 41.3% | 45.94 | 36.27 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | SELL | 7.00 | 31.50 | 0.22 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0004 | BUY | 1.75 | 34.25 | 0.05 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0005 | SELL | 42.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0007 | SELL | 2.25 | 13.00 | 0.17 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0009 | SELL | 174.75 | 27.75 | 6.30 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0010 | SELL | 238.50 | 19.00 | 12.55 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0010_2 | SELL | 238.50 | 19.00 | 12.55 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0013 | SELL | 72.50 | 18.00 | 4.03 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0015 | SELL | 1.25 | 68.50 | 0.02 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0015_2 | SELL | 1.25 | 68.50 | 0.02 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0017 | SELL | 14.25 | 42.75 | 0.33 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0020 | SELL | 53.25 | 33.00 | 1.61 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0020_2 | SELL | 53.25 | 33.00 | 1.61 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0022 | SELL | 55.25 | 58.00 | 0.95 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0024 | SELL | 26.75 | 88.50 | 0.30 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0024_2 | SELL | 26.75 | 88.50 | 0.30 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0026 | SELL | 0.25 | 77.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0028 | SELL | 0.00 | 103.75 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0032 | SELL | 12.75 | 25.25 | 0.50 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0034 | BUY | 11.50 | 13.00 | 0.88 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 1,308

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 487 | 30.2% | 56.3% | 4.65 | 45.0% | 2.09 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 265 | 49.4% | 51.3% | 2.62 | 54.3% | 1.96 | ⚠️ CALIDAD MEDIA - Revisar |
| 3.0-5.0 ATR (Media) | 330 | 74.8% | 64.8% | 2.79 | 70.9% | 2.03 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 215 | 77.7% | 74.4% | 1.59 | 78.1% | 2.21 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 11 | 45.5% | 45.5% | 0.00 | 45.5% | 3.07 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (487 oportunidades)

- **WR Teórico:** 30.2% (si se hubieran ejecutado)
- **TP_FIRST:** 56.3% (274 de 487)
- **SL_FIRST:** 40.2% (196 de 487)
- **MFE Promedio:** 49.71 pts
- **MAE Promedio:** 44.40 pts
- **MFE/MAE Ratio:** 4.65
- **Good Entries:** 45.0% (MFE > MAE)
- **R:R Promedio:** 2.09

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (265 oportunidades)

- **WR Teórico:** 49.4% (si se hubieran ejecutado)
- **TP_FIRST:** 51.3% (136 de 265)
- **SL_FIRST:** 48.3% (128 de 265)
- **MFE Promedio:** 74.09 pts
- **MAE Promedio:** 41.90 pts
- **MFE/MAE Ratio:** 2.62
- **Good Entries:** 54.3% (MFE > MAE)
- **R:R Promedio:** 1.96

**⚠️ CALIDAD MEDIA - Revisar**

**3.0-5.0 ATR (Media)** (330 oportunidades)

- **WR Teórico:** 74.8% (si se hubieran ejecutado)
- **TP_FIRST:** 64.8% (214 de 330)
- **SL_FIRST:** 35.2% (116 de 330)
- **MFE Promedio:** 92.71 pts
- **MAE Promedio:** 45.03 pts
- **MFE/MAE Ratio:** 2.79
- **Good Entries:** 70.9% (MFE > MAE)
- **R:R Promedio:** 2.03

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (215 oportunidades)

- **WR Teórico:** 77.7% (si se hubieran ejecutado)
- **TP_FIRST:** 74.4% (160 de 215)
- **SL_FIRST:** 25.6% (55 de 215)
- **MFE Promedio:** 94.05 pts
- **MAE Promedio:** 52.32 pts
- **MFE/MAE Ratio:** 1.59
- **Good Entries:** 78.1% (MFE > MAE)
- **R:R Promedio:** 2.21

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (11 oportunidades)

- **WR Teórico:** 45.5% (si se hubieran ejecutado)
- **TP_FIRST:** 45.5% (5 de 11)
- **SL_FIRST:** 54.5% (6 de 11)
- **MFE Promedio:** 108.55 pts
- **MAE Promedio:** 80.79 pts
- **MFE/MAE Ratio:** 0.00
- **Good Entries:** 45.5% (MFE > MAE)
- **R:R Promedio:** 3.07

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 46 | 487 | 265 | 330 |
| **TP_FIRST %** | 41.3% | 56.3% | 51.3% | 64.8% |
| **Good Entries %** | 45.7% | 45.0% | 54.3% | 70.9% |
| **MFE/MAE Ratio** | 68.09 | 4.65 | 2.62 | 2.79 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 330 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 74.8%
   - Good Entries: 70.9%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 14.08 pts
- **Mediana:** 11.16 pts
- **Min/Max:** 2.44 / 37.51 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 11.16 |
| P70 | 18.02 |
| P80 | 22.02 |
| P90 | 27.27 |
| P95 | 36.95 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 25.96 pts
- **Mediana:** 23.00 pts
- **Min/Max:** 4.50 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 23.00 |
| P70 | 35.58 |
| P80 | 40.95 |
| P90 | 49.30 |
| P95 | 52.31 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 27; // Era 60
public int MaxTPDistancePoints { get; set; } = 49; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 27.3pts, TP: 49.3pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (37.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.370) / 0.370
R:R_min = 1.71
```

**Estado actual:** R:R promedio = 1.98
**Gap:** -0.27 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **49** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.98) < R:R mínimo (1.71)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=37.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-14 16:34:31*