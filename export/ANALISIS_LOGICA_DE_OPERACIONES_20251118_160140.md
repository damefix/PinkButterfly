# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-18 16:05:41
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251118_160140.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251118_160140.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 11
- **Win Rate:** 45.5% (5/11)
- **Profit Factor:** 1.55
- **Avg R:R Planeado:** 1.98
- **R:R Mínimo para Break-Even:** 1.20

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 26 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.98
   - R:R necesario: 1.20
   - **Gap:** -0.78

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bearish | 200 | 31.9% |
| Neutral | 236 | 37.7% |
| Bullish | 190 | 30.4% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** -0.027
- **Score Min/Max:** [-0.980, 0.920]
- **Componentes (promedio):**
  - EMA20 Slope: -0.027
  - EMA50 Cross: -0.067
  - BOS Count: -0.013
  - Regression 24h: 0.007

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.920 (apenas supera threshold)
- Score mínimo observado: -0.980 (apenas supera threshold)
- **Consecuencia:** Sistema queda 37.7% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (37.7%)

**Posibles causas:**
- **BOS Score bajo (-0.013):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio -0.027 indica poca señal direccional
- **Mercado lateral:** Scores reales [-0.98, 0.92] muy cercanos a 0

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
| StructureFusion | 3222 | 100.0% | 100.0% |
| ProximityAnalyzer | 959 | 29.8% | 29.8% |
| DFM_Evaluated | 217 | 22.6% | 6.7% |
| DFM_Passed | 217 | 100.0% | 6.7% |
| RiskCalculator | 1919 | 884.3% | 59.6% |
| Risk_Accepted | 2 | 0.1% | 0.1% |
| TradeManager | 11 | 550.0% | 0.3% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 1917 señales, -99.9%)
- **Tasa de conversión final:** 0.34% (de 3222 zonas iniciales → 11 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 495 | 49.0% |
| NO_SL | 338 | 33.5% |
| ENTRY_TOO_FAR | 150 | 14.9% |
| TP_CHECK_FAIL | 27 | 2.7% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (495 rechazos, 49.0%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 694 | 88.5% |
| P0_ANY_DIR | 90 | 11.5% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 45.70 pts (máxima ganancia flotante)
- **MAE Promedio:** 26.86 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 454.49

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 8 | 72.7% |
| SL_FIRST (precio fue hacia SL) | 3 | 27.3% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 63.6%
- **Entradas Malas (MAE > MFE):** 36.4%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 11 | 8 | 3 | 72.7% | 45.70 | 26.86 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0005 | SELL | 16.50 | 32.75 | 0.50 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0007 | BUY | 16.00 | 10.75 | 1.49 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0008 | SELL | 21.00 | 11.25 | 1.87 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0011 | SELL | 131.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0014 | SELL | 0.00 | 103.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0014_2 | SELL | 0.00 | 103.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0018 | SELL | 17.75 | 34.25 | 0.52 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0023 | SELL | 64.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0023_2 | SELL | 64.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0023_3 | SELL | 64.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0027 | SELL | 106.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 351

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 128 | 25.8% | 61.7% | 2.50 | 52.3% | 2.30 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 68 | 45.6% | 61.8% | 1.70 | 51.5% | 2.43 | ⚠️ CALIDAD MEDIA - Revisar |
| 3.0-5.0 ATR (Media) | 113 | 76.1% | 70.8% | 7.46 | 70.8% | 2.45 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 42 | 85.7% | 81.0% | 1.74 | 85.7% | 2.20 | ✅ BUENA CALIDAD - Considerar incluir |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (128 oportunidades)

- **WR Teórico:** 25.8% (si se hubieran ejecutado)
- **TP_FIRST:** 61.7% (79 de 128)
- **SL_FIRST:** 35.9% (46 de 128)
- **MFE Promedio:** 43.70 pts
- **MAE Promedio:** 35.48 pts
- **MFE/MAE Ratio:** 2.50
- **Good Entries:** 52.3% (MFE > MAE)
- **R:R Promedio:** 2.30

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (68 oportunidades)

- **WR Teórico:** 45.6% (si se hubieran ejecutado)
- **TP_FIRST:** 61.8% (42 de 68)
- **SL_FIRST:** 38.2% (26 de 68)
- **MFE Promedio:** 57.62 pts
- **MAE Promedio:** 46.58 pts
- **MFE/MAE Ratio:** 1.70
- **Good Entries:** 51.5% (MFE > MAE)
- **R:R Promedio:** 2.43

**⚠️ CALIDAD MEDIA - Revisar**

**3.0-5.0 ATR (Media)** (113 oportunidades)

- **WR Teórico:** 76.1% (si se hubieran ejecutado)
- **TP_FIRST:** 70.8% (80 de 113)
- **SL_FIRST:** 28.3% (32 de 113)
- **MFE Promedio:** 78.38 pts
- **MAE Promedio:** 31.83 pts
- **MFE/MAE Ratio:** 7.46
- **Good Entries:** 70.8% (MFE > MAE)
- **R:R Promedio:** 2.45

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (42 oportunidades)

- **WR Teórico:** 85.7% (si se hubieran ejecutado)
- **TP_FIRST:** 81.0% (34 de 42)
- **SL_FIRST:** 16.7% (7 de 42)
- **MFE Promedio:** 102.63 pts
- **MAE Promedio:** 35.97 pts
- **MFE/MAE Ratio:** 1.74
- **Good Entries:** 85.7% (MFE > MAE)
- **R:R Promedio:** 2.20

**✅ BUENA CALIDAD - Considerar incluir**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 11 | 128 | 68 | 113 |
| **TP_FIRST %** | 72.7% | 61.7% | 61.8% | 70.8% |
| **Good Entries %** | 63.6% | 52.3% | 51.5% | 70.8% |
| **MFE/MAE Ratio** | 454.49 | 2.50 | 1.70 | 7.46 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 113 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 76.1%
   - Good Entries: 70.8%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 16.07 pts
- **Mediana:** 14.90 pts
- **Min/Max:** 5.14 / 26.24 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 14.90 |
| P70 | 21.52 |
| P80 | 25.15 |
| P90 | 26.24 |
| P95 | 26.24 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 30.80 pts
- **Mediana:** 36.25 pts
- **Min/Max:** 11.50 / 53.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 36.25 |
| P70 | 37.65 |
| P80 | 42.00 |
| P90 | 51.60 |
| P95 | 57.30 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 26; // Era 60
public int MaxTPDistancePoints { get; set; } = 51; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 26.2pts, TP: 51.6pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (45.5%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.455) / 0.455
R:R_min = 1.20
```

**Estado actual:** R:R promedio = 1.98
**Gap:** -0.78 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **51** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.98) < R:R mínimo (1.20)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=45.5%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-18 16:05:41*