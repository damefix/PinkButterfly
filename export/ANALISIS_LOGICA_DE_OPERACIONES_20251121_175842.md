# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-21 18:00:27
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251121_175842.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251121_175842.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 3
- **Win Rate:** 33.3% (1/3)
- **Profit Factor:** 1.23
- **Avg R:R Planeado:** 2.40
- **R:R Mínimo para Break-Even:** 2.00

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 29 puntos
   - TP máximo observado: 47 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.40
   - R:R necesario: 2.00
   - **Gap:** -0.40

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 177 | 40.5% |
| Bearish | 188 | 43.0% |
| Bullish | 72 | 16.5% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** -0.137
- **Score Min/Max:** [-0.840, 0.960]
- **Componentes (promedio):**
  - EMA20 Slope: -0.232
  - EMA50 Cross: -0.140
  - BOS Count: -0.011
  - Regression 24h: -0.147

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.960 (apenas supera threshold)
- Score mínimo observado: -0.840 (apenas supera threshold)
- **Consecuencia:** Sistema queda 40.5% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (40.5%)

**Posibles causas:**
- **BOS Score bajo (-0.011):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio -0.137 indica poca señal direccional
- **Mercado lateral:** Scores reales [-0.84, 0.96] muy cercanos a 0

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
| StructureFusion | 2685 | 100.0% | 100.0% |
| ProximityAnalyzer | 428 | 15.9% | 15.9% |
| DFM_Evaluated | 21 | 4.9% | 0.8% |
| DFM_Passed | 21 | 100.0% | 0.8% |
| RiskCalculator | 861 | 4100.0% | 32.1% |
| Risk_Accepted | 7 | 0.8% | 0.3% |
| TradeManager | 3 | 42.9% | 0.1% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 854 señales, -99.2%)
- **Tasa de conversión final:** 0.11% (de 2685 zonas iniciales → 3 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 416 | 63.3% |
| ENTRY_TOO_FAR | 129 | 19.6% |
| NO_SL | 94 | 14.3% |
| TP_CHECK_FAIL | 18 | 2.7% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (416 rechazos, 63.3%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 270 | 90.3% |
| P0_ANY_DIR | 29 | 9.7% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 41.08 pts (máxima ganancia flotante)
- **MAE Promedio:** 34.58 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 4.68

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 2 | 66.7% |
| SL_FIRST (precio fue hacia SL) | 1 | 33.3% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 33.3%
- **Entradas Malas (MAE > MFE):** 66.7%

⚠️ **ALERTA:** >60% de entradas tienen MAE > MFE
- **Problema:** El precio va más en contra que a favor antes del cierre
- **Causas posibles:**
  1. Timing incorrecto (entramos antes de reversión)
  2. Bias desincronizado (operamos contra tendencia real)
  3. Zonas de baja calidad (sin confluence real)

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 3 | 2 | 1 | 66.7% | 41.08 | 34.58 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0002 | SELL | 67.25 | 5.25 | 12.81 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0003 | SELL | 24.00 | 32.75 | 0.73 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0007 | SELL | 32.00 | 65.75 | 0.49 | SL_FIRST | CLOSED | ❌ Entrada muy mala |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 53

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 20 | 70.0% | 80.0% | 28.87 | 75.0% | 1.86 | ✅ BUENA CALIDAD - Considerar incluir |
| 2.0-3.0 ATR (Cerca) | 13 | 76.9% | 76.9% | 36.43 | 76.9% | 1.85 | ✅ BUENA CALIDAD - Considerar incluir |
| 3.0-5.0 ATR (Media) | 17 | 82.4% | 76.5% | 39.23 | 82.4% | 2.51 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 3 | 100.0% | 100.0% | 0.00 | 100.0% | 5.95 | ✅ BUENA CALIDAD - Considerar incluir |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (20 oportunidades)

- **WR Teórico:** 70.0% (si se hubieran ejecutado)
- **TP_FIRST:** 80.0% (16 de 20)
- **SL_FIRST:** 20.0% (4 de 20)
- **MFE Promedio:** 78.78 pts
- **MAE Promedio:** 21.61 pts
- **MFE/MAE Ratio:** 28.87
- **Good Entries:** 75.0% (MFE > MAE)
- **R:R Promedio:** 1.86

**✅ BUENA CALIDAD - Considerar incluir**

**2.0-3.0 ATR (Cerca)** (13 oportunidades)

- **WR Teórico:** 76.9% (si se hubieran ejecutado)
- **TP_FIRST:** 76.9% (10 de 13)
- **SL_FIRST:** 23.1% (3 de 13)
- **MFE Promedio:** 79.56 pts
- **MAE Promedio:** 22.40 pts
- **MFE/MAE Ratio:** 36.43
- **Good Entries:** 76.9% (MFE > MAE)
- **R:R Promedio:** 1.85

**✅ BUENA CALIDAD - Considerar incluir**

**3.0-5.0 ATR (Media)** (17 oportunidades)

- **WR Teórico:** 82.4% (si se hubieran ejecutado)
- **TP_FIRST:** 76.5% (13 de 17)
- **SL_FIRST:** 23.5% (4 de 17)
- **MFE Promedio:** 81.44 pts
- **MAE Promedio:** 19.12 pts
- **MFE/MAE Ratio:** 39.23
- **Good Entries:** 82.4% (MFE > MAE)
- **R:R Promedio:** 2.51

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (3 oportunidades)

- **WR Teórico:** 100.0% (si se hubieran ejecutado)
- **TP_FIRST:** 100.0% (3 de 3)
- **SL_FIRST:** 0.0% (0 de 3)
- **MFE Promedio:** 149.08 pts
- **MAE Promedio:** 0.00 pts
- **MFE/MAE Ratio:** 0.00
- **Good Entries:** 100.0% (MFE > MAE)
- **R:R Promedio:** 5.95

**✅ BUENA CALIDAD - Considerar incluir**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 3 | 20 | 13 | 17 |
| **TP_FIRST %** | 66.7% | 80.0% | 76.9% | 76.5% |
| **Good Entries %** | 33.3% | 75.0% | 76.9% | 82.4% |
| **MFE/MAE Ratio** | 4.68 | 28.87 | 36.43 | 39.23 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

🚨 **CRÍTICO: El rango 2.0-3.0 ATR contiene 13 oportunidades de BUENA CALIDAD**
   - WR Teórico: 76.9%
   - Good Entries: 76.9%
   - **ACCIÓN:** Considerar aumentar MaxDistanceToRegister_ATR_Normal de 2.0 a 3.0

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 17 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 82.4%
   - Good Entries: 82.4%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 22.40 pts
- **Mediana:** 27.18 pts
- **Min/Max:** 11.04 / 28.98 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 27.18 |
| P70 | 28.62 |
| P80 | 29.34 |
| P90 | 30.06 |
| P95 | 30.42 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 45.33 pts
- **Mediana:** 46.50 pts
- **Min/Max:** 42.75 / 46.75 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 46.50 |
| P70 | 46.70 |
| P80 | 46.80 |
| P90 | 46.90 |
| P95 | 46.95 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 30; // Era 60
public int MaxTPDistancePoints { get; set; } = 46; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 30.1pts, TP: 46.9pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (33.3%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.333) / 0.333
R:R_min = 2.00
```

**Estado actual:** R:R promedio = 2.40
**Gap:** -0.40 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **30** (P90 real)
2. **MaxTPDistancePoints:** 120 → **46** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.40) < R:R mínimo (2.00)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=33.3%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-21 18:00:27*