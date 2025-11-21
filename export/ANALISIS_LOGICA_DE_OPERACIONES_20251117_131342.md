# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-17 13:22:10
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_131342.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251117_131342.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 13
- **Win Rate:** 7.7% (1/13)
- **Profit Factor:** 0.39
- **Avg R:R Planeado:** 2.31
- **R:R Mínimo para Break-Even:** 12.00

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 26 puntos
   - TP máximo observado: 52 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.31
   - R:R necesario: 12.00
   - **Gap:** 9.69

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bullish | 10100 | 39.0% |
| Neutral | 8768 | 33.8% |
| Bearish | 7039 | 27.2% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.080
- **Score Min/Max:** [-1.000, 1.000]
- **Componentes (promedio):**
  - EMA20 Slope: 0.041
  - EMA50 Cross: 0.179
  - BOS Count: 0.015
  - Regression 24h: 0.093

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 1.000 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 33.8% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (33.8%)

**Posibles causas:**
- **BOS Score bajo (0.015):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio 0.080 indica poca señal direccional
- **Mercado lateral:** Scores reales [-1.00, 1.00] muy cercanos a 0

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
| StructureFusion | 77714 | 100.0% | 100.0% |
| ProximityAnalyzer | 1503 | 1.9% | 1.9% |
| DFM_Evaluated | 376 | 25.0% | 0.5% |
| DFM_Passed | 376 | 100.0% | 0.5% |
| RiskCalculator | 3386 | 900.5% | 4.4% |
| Risk_Accepted | 1 | 0.0% | 0.0% |
| TradeManager | 13 | 1300.0% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 3385 señales, -100.0%)
- **Tasa de conversión final:** 0.02% (de 77714 zonas iniciales → 13 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 635 | 41.5% |
| ENTRY_TOO_FAR | 436 | 28.5% |
| TP_CHECK_FAIL | 268 | 17.5% |
| NO_SL | 190 | 12.4% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (635 rechazos, 41.5%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 1956 | 97.8% |
| P0_ANY_DIR | 43 | 2.2% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 23.52 pts (máxima ganancia flotante)
- **MAE Promedio:** 25.02 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 2.40

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 5 | 38.5% |
| SL_FIRST (precio fue hacia SL) | 8 | 61.5% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 38.5%
- **Entradas Malas (MAE > MFE):** 61.5%

⚠️ **ALERTA:** >60% de entradas tienen MAE > MFE
- **Problema:** El precio va más en contra que a favor antes del cierre
- **Causas posibles:**
  1. Timing incorrecto (entramos antes de reversión)
  2. Bias desincronizado (operamos contra tendencia real)
  3. Zonas de baja calidad (sin confluence real)

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 13 | 5 | 8 | 38.5% | 23.52 | 25.02 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0002 | SELL | 7.75 | 16.00 | 0.48 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0006 | SELL | 11.00 | 3.50 | 3.14 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0007 | SELL | 4.75 | 33.75 | 0.14 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0008 | SELL | 4.50 | 33.75 | 0.13 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0009 | SELL | 4.50 | 34.25 | 0.13 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0010 | SELL | 33.75 | 20.75 | 1.63 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0013 | BUY | 6.25 | 22.00 | 0.28 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0014 | BUY | 6.25 | 19.00 | 0.33 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0015 | BUY | 1.25 | 31.75 | 0.04 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0016 | SELL | 21.50 | 51.25 | 0.42 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0018 | SELL | 27.75 | 26.50 | 1.05 | SL_FIRST | CLOSED | 👍 Entrada correcta |
| T0020 | SELL | 80.25 | 4.00 | 20.06 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0023 | SELL | 96.25 | 28.75 | 3.35 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 776

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 246 | 37.8% | 57.7% | 18.41 | 56.5% | 2.00 | ⚠️ CALIDAD MEDIA - Revisar |
| 2.0-3.0 ATR (Cerca) | 98 | 39.8% | 44.9% | 4.39 | 48.0% | 2.04 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 180 | 45.0% | 40.0% | 4.96 | 57.8% | 1.84 | ⚠️ CALIDAD MEDIA - Revisar |
| 5.0-10.0 ATR (Lejos) | 208 | 44.7% | 30.3% | 1.62 | 62.5% | 1.94 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 44 | 65.9% | 34.1% | 3.87 | 63.6% | 2.03 | ✅ BUENA CALIDAD - Considerar incluir |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (246 oportunidades)

- **WR Teórico:** 37.8% (si se hubieran ejecutado)
- **TP_FIRST:** 57.7% (142 de 246)
- **SL_FIRST:** 34.1% (84 de 246)
- **MFE Promedio:** 58.82 pts
- **MAE Promedio:** 31.83 pts
- **MFE/MAE Ratio:** 18.41
- **Good Entries:** 56.5% (MFE > MAE)
- **R:R Promedio:** 2.00

**⚠️ CALIDAD MEDIA - Revisar**

**2.0-3.0 ATR (Cerca)** (98 oportunidades)

- **WR Teórico:** 39.8% (si se hubieran ejecutado)
- **TP_FIRST:** 44.9% (44 de 98)
- **SL_FIRST:** 53.1% (52 de 98)
- **MFE Promedio:** 69.94 pts
- **MAE Promedio:** 41.17 pts
- **MFE/MAE Ratio:** 4.39
- **Good Entries:** 48.0% (MFE > MAE)
- **R:R Promedio:** 2.04

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (180 oportunidades)

- **WR Teórico:** 45.0% (si se hubieran ejecutado)
- **TP_FIRST:** 40.0% (72 de 180)
- **SL_FIRST:** 56.7% (102 de 180)
- **MFE Promedio:** 49.25 pts
- **MAE Promedio:** 36.01 pts
- **MFE/MAE Ratio:** 4.96
- **Good Entries:** 57.8% (MFE > MAE)
- **R:R Promedio:** 1.84

**⚠️ CALIDAD MEDIA - Revisar**

**5.0-10.0 ATR (Lejos)** (208 oportunidades)

- **WR Teórico:** 44.7% (si se hubieran ejecutado)
- **TP_FIRST:** 30.3% (63 de 208)
- **SL_FIRST:** 67.8% (141 de 208)
- **MFE Promedio:** 37.71 pts
- **MAE Promedio:** 40.68 pts
- **MFE/MAE Ratio:** 1.62
- **Good Entries:** 62.5% (MFE > MAE)
- **R:R Promedio:** 1.94

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (44 oportunidades)

- **WR Teórico:** 65.9% (si se hubieran ejecutado)
- **TP_FIRST:** 34.1% (15 de 44)
- **SL_FIRST:** 65.9% (29 de 44)
- **MFE Promedio:** 51.74 pts
- **MAE Promedio:** 36.08 pts
- **MFE/MAE Ratio:** 3.87
- **Good Entries:** 63.6% (MFE > MAE)
- **R:R Promedio:** 2.03

**✅ BUENA CALIDAD - Considerar incluir**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 13 | 246 | 98 | 180 |
| **TP_FIRST %** | 38.5% | 57.7% | 44.9% | 40.0% |
| **Good Entries %** | 38.5% | 56.5% | 48.0% | 57.8% |
| **MFE/MAE Ratio** | 2.40 | 18.41 | 4.39 | 4.96 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 180 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 45.0%
   - Good Entries: 57.8%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 11.70 pts
- **Mediana:** 6.94 pts
- **Min/Max:** 1.88 / 26.24 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 6.94 |
| P70 | 24.06 |
| P80 | 25.73 |
| P90 | 26.20 |
| P95 | 26.27 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 22.52 pts
- **Mediana:** 17.75 pts
- **Min/Max:** 5.00 / 52.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 17.75 |
| P70 | 38.50 |
| P80 | 42.75 |
| P90 | 48.30 |
| P95 | 54.77 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 26; // Era 60
public int MaxTPDistancePoints { get; set; } = 48; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 26.2pts, TP: 48.3pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (7.7%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.077) / 0.077
R:R_min = 12.00
```

**Estado actual:** R:R promedio = 2.31
**Gap:** 9.69 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **26** (P90 real)
2. **MaxTPDistancePoints:** 120 → **48** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.31) < R:R mínimo (12.00)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=7.7%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-17 13:22:10*