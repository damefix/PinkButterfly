# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-12 16:23:01
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_162002.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251112_162002.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 45
- **Win Rate:** 28.9% (13/45)
- **Profit Factor:** 0.74
- **Avg R:R Planeado:** 1.81
- **R:R Mínimo para Break-Even:** 2.46

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 40 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.81
   - R:R necesario: 2.46
   - **Gap:** 0.65

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8093 | 34.7% |
| Bullish | 8968 | 38.4% |
| Bearish | 6292 | 26.9% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.079
- **Score Min/Max:** [-0.990, 0.970]
- **Componentes (promedio):**
  - EMA20 Slope: 0.042
  - EMA50 Cross: 0.183
  - BOS Count: 0.009
  - Regression 24h: 0.089

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.970 (apenas supera threshold)
- Score mínimo observado: -0.990 (apenas supera threshold)
- **Consecuencia:** Sistema queda 34.7% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.7%)

**Posibles causas:**
- **BOS Score bajo (0.009):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio 0.079 indica poca señal direccional
- **Mercado lateral:** Scores reales [-0.99, 0.97] muy cercanos a 0

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
| StructureFusion | 10191 | 100.0% | 100.0% |
| ProximityAnalyzer | 2844 | 27.9% | 27.9% |
| DFM_Evaluated | 973 | 34.2% | 9.5% |
| DFM_Passed | 895 | 92.0% | 8.8% |
| RiskCalculator | 4368 | 488.0% | 42.9% |
| Risk_Accepted | 103 | 2.4% | 1.0% |
| TradeManager | 45 | 43.7% | 0.4% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 4265 señales, -97.6%)
- **Tasa de conversión final:** 0.44% (de 10191 zonas iniciales → 45 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 997 | 58.3% |
| NO_SL | 288 | 16.8% |
| ENTRY_TOO_FAR | 278 | 16.3% |
| TP_CHECK_FAIL | 147 | 8.6% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (997 rechazos, 58.3%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 1885 | 90.1% |
| P0_ANY_DIR | 206 | 9.9% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 30.83 pts (máxima ganancia flotante)
- **MAE Promedio:** 42.84 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 2.25

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 22 | 48.9% |
| SL_FIRST (precio fue hacia SL) | 22 | 48.9% |
| NEUTRAL (sin dirección clara) | 1 | 2.2% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 31.1%
- **Entradas Malas (MAE > MFE):** 68.9%

⚠️ **ALERTA:** >60% de entradas tienen MAE > MFE
- **Problema:** El precio va más en contra que a favor antes del cierre
- **Causas posibles:**
  1. Timing incorrecto (entramos antes de reversión)
  2. Bias desincronizado (operamos contra tendencia real)
  3. Zonas de baja calidad (sin confluence real)

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 45 | 22 | 22 | 48.9% | 30.83 | 42.84 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0003 | SELL | 30.50 | 4.00 | 7.62 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0007 | SELL | 2.75 | 14.50 | 0.19 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0011 | BUY | 1.75 | 11.00 | 0.16 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0012 | BUY | 4.00 | 37.00 | 0.11 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0013 | BUY | 13.00 | 35.75 | 0.36 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0014 | BUY | 0.00 | 39.00 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0019 | SELL | 145.75 | 19.50 | 7.47 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0019_2 | SELL | 145.75 | 19.50 | 7.47 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0020 | SELL | 248.50 | 11.25 | 22.09 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0023 | SELL | 0.00 | 68.50 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0024 | SELL | 33.75 | 28.00 | 1.21 | SL_FIRST | CLOSED | 👍 Entrada correcta |
| T0026 | BUY | 9.50 | 66.75 | 0.14 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0027 | BUY | 8.25 | 66.75 | 0.12 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0028 | BUY | 29.25 | 6.25 | 4.68 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0029 | BUY | 13.25 | 105.25 | 0.13 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0030 | BUY | 38.50 | 80.00 | 0.48 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0034 | SELL | 56.50 | 65.50 | 0.86 | SL_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0035 | SELL | 27.25 | 80.00 | 0.34 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0044 | BUY | 15.00 | 16.00 | 0.94 | SL_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0047 | BUY | 12.00 | 12.00 | 1.00 | SL_FIRST | CLOSED | ⚠️ Entrada dudosa |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 988

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 369 | 37.1% | 58.5% | 4.46 | 46.1% | 1.93 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 196 | 52.0% | 60.2% | 2.75 | 48.0% | 1.81 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 259 | 66.0% | 56.8% | 1.46 | 56.0% | 2.04 | ⚠️ CALIDAD MEDIA - Revisar |
| 5.0-10.0 ATR (Lejos) | 159 | 59.1% | 53.5% | 0.77 | 57.9% | 2.36 | ⚠️ CALIDAD MEDIA - Revisar |
| >10.0 ATR (Muy lejos) | 5 | 0.0% | 0.0% | 0.00 | 0.0% | 1.73 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (369 oportunidades)

- **WR Teórico:** 37.1% (si se hubieran ejecutado)
- **TP_FIRST:** 58.5% (216 de 369)
- **SL_FIRST:** 38.2% (141 de 369)
- **MFE Promedio:** 42.64 pts
- **MAE Promedio:** 43.43 pts
- **MFE/MAE Ratio:** 4.46
- **Good Entries:** 46.1% (MFE > MAE)
- **R:R Promedio:** 1.93

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (196 oportunidades)

- **WR Teórico:** 52.0% (si se hubieran ejecutado)
- **TP_FIRST:** 60.2% (118 de 196)
- **SL_FIRST:** 39.3% (77 de 196)
- **MFE Promedio:** 49.08 pts
- **MAE Promedio:** 43.72 pts
- **MFE/MAE Ratio:** 2.75
- **Good Entries:** 48.0% (MFE > MAE)
- **R:R Promedio:** 1.81

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (259 oportunidades)

- **WR Teórico:** 66.0% (si se hubieran ejecutado)
- **TP_FIRST:** 56.8% (147 de 259)
- **SL_FIRST:** 42.5% (110 de 259)
- **MFE Promedio:** 72.15 pts
- **MAE Promedio:** 44.89 pts
- **MFE/MAE Ratio:** 1.46
- **Good Entries:** 56.0% (MFE > MAE)
- **R:R Promedio:** 2.04

**⚠️ CALIDAD MEDIA - Revisar**

**5.0-10.0 ATR (Lejos)** (159 oportunidades)

- **WR Teórico:** 59.1% (si se hubieran ejecutado)
- **TP_FIRST:** 53.5% (85 de 159)
- **SL_FIRST:** 45.3% (72 de 159)
- **MFE Promedio:** 87.27 pts
- **MAE Promedio:** 63.74 pts
- **MFE/MAE Ratio:** 0.77
- **Good Entries:** 57.9% (MFE > MAE)
- **R:R Promedio:** 2.36

**⚠️ CALIDAD MEDIA - Revisar**

**>10.0 ATR (Muy lejos)** (5 oportunidades)

- **WR Teórico:** 0.0% (si se hubieran ejecutado)
- **TP_FIRST:** 0.0% (0 de 5)
- **SL_FIRST:** 100.0% (5 de 5)
- **MFE Promedio:** 0.00 pts
- **MAE Promedio:** 58.25 pts
- **MFE/MAE Ratio:** 0.00
- **Good Entries:** 0.0% (MFE > MAE)
- **R:R Promedio:** 1.73

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 45 | 369 | 196 | 259 |
| **TP_FIRST %** | 48.9% | 58.5% | 60.2% | 56.8% |
| **Good Entries %** | 31.1% | 46.1% | 48.0% | 56.0% |
| **MFE/MAE Ratio** | 2.25 | 4.46 | 2.75 | 1.46 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 259 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 66.0%
   - Good Entries: 56.0%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 12.88 pts
- **Mediana:** 10.14 pts
- **Min/Max:** 3.64 / 40.44 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 10.14 |
| P70 | 17.75 |
| P80 | 19.39 |
| P90 | 23.59 |
| P95 | 33.40 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 22.86 pts
- **Mediana:** 18.75 pts
- **Min/Max:** 6.25 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 18.75 |
| P70 | 27.85 |
| P80 | 37.85 |
| P90 | 49.40 |
| P95 | 52.95 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 23; // Era 60
public int MaxTPDistancePoints { get; set; } = 49; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 23.6pts, TP: 49.4pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (28.9%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.289) / 0.289
R:R_min = 2.46
```

**Estado actual:** R:R promedio = 1.81
**Gap:** 0.65 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **49** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.81) < R:R mínimo (2.46)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=28.9%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-12 16:23:01*