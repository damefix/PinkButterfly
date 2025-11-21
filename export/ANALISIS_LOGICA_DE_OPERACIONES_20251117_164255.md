# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-17 16:51:34
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_164255.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251117_164255.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 9
- **Win Rate:** 33.3% (3/9)
- **Profit Factor:** 1.11
- **Avg R:R Planeado:** 1.65
- **R:R Mínimo para Break-Even:** 2.00

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 18 puntos
   - TP máximo observado: 32 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.65
   - R:R necesario: 2.00
   - **Gap:** 0.35

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8119 | 34.7% |
| Bullish | 9088 | 38.8% |
| Bearish | 6214 | 26.5% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.083
- **Score Min/Max:** [-1.000, 1.000]
- **Componentes (promedio):**
  - EMA20 Slope: 0.044
  - EMA50 Cross: 0.196
  - BOS Count: 0.008
  - Regression 24h: 0.092

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 1.000 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 34.7% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.7%)

**Posibles causas:**
- **BOS Score bajo (0.008):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio 0.083 indica poca señal direccional
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
| StructureFusion | 80191 | 100.0% | 100.0% |
| ProximityAnalyzer | 1233 | 1.5% | 1.5% |
| DFM_Evaluated | 251 | 20.4% | 0.3% |
| DFM_Passed | 251 | 100.0% | 0.3% |
| RiskCalculator | 2592 | 1032.7% | 3.2% |
| Risk_Accepted | 28 | 1.1% | 0.0% |
| TradeManager | 9 | 32.1% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 2564 señales, -98.9%)
- **Tasa de conversión final:** 0.01% (de 80191 zonas iniciales → 9 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 929 | 66.5% |
| ENTRY_TOO_FAR | 358 | 25.6% |
| TP_CHECK_FAIL | 89 | 6.4% |
| NO_SL | 22 | 1.6% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (929 rechazos, 66.5%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 1398 | 97.1% |
| P0_ANY_DIR | 42 | 2.9% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 33.25 pts (máxima ganancia flotante)
- **MAE Promedio:** 31.81 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 1.37

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 4 | 44.4% |
| SL_FIRST (precio fue hacia SL) | 5 | 55.6% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 44.4%
- **Entradas Malas (MAE > MFE):** 55.6%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 9 | 4 | 5 | 44.4% | 33.25 | 31.81 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0006 | SELL | 93.25 | 20.00 | 4.66 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0009 | SELL | 63.25 | 21.50 | 2.94 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0011 | SELL | 30.00 | 31.75 | 0.94 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0012 | SELL | 5.50 | 43.25 | 0.13 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0015 | SELL | 21.50 | 17.00 | 1.26 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0018 | SELL | 30.75 | 50.25 | 0.61 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0025 | SELL | 13.75 | 41.50 | 0.33 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0026 | SELL | 13.75 | 36.00 | 0.38 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0028 | SELL | 27.50 | 25.00 | 1.10 | SL_FIRST | CLOSED | 👍 Entrada correcta |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 540

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 184 | 49.5% | 60.3% | 4.04 | 70.1% | 2.39 | ✅ BUENA CALIDAD - Considerar incluir |
| 2.0-3.0 ATR (Cerca) | 101 | 61.4% | 56.4% | 6.09 | 73.3% | 2.36 | ✅ BUENA CALIDAD - Considerar incluir |
| 3.0-5.0 ATR (Media) | 130 | 56.9% | 48.5% | 5.92 | 66.2% | 2.63 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 111 | 69.4% | 47.7% | 4.58 | 64.0% | 2.29 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 14 | 42.9% | 7.1% | 0.79 | 14.3% | 2.26 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (184 oportunidades)

- **WR Teórico:** 49.5% (si se hubieran ejecutado)
- **TP_FIRST:** 60.3% (111 de 184)
- **SL_FIRST:** 34.2% (63 de 184)
- **MFE Promedio:** 47.97 pts
- **MAE Promedio:** 30.93 pts
- **MFE/MAE Ratio:** 4.04
- **Good Entries:** 70.1% (MFE > MAE)
- **R:R Promedio:** 2.39

**✅ BUENA CALIDAD - Considerar incluir**

**2.0-3.0 ATR (Cerca)** (101 oportunidades)

- **WR Teórico:** 61.4% (si se hubieran ejecutado)
- **TP_FIRST:** 56.4% (57 de 101)
- **SL_FIRST:** 33.7% (34 de 101)
- **MFE Promedio:** 59.95 pts
- **MAE Promedio:** 38.06 pts
- **MFE/MAE Ratio:** 6.09
- **Good Entries:** 73.3% (MFE > MAE)
- **R:R Promedio:** 2.36

**✅ BUENA CALIDAD - Considerar incluir**

**3.0-5.0 ATR (Media)** (130 oportunidades)

- **WR Teórico:** 56.9% (si se hubieran ejecutado)
- **TP_FIRST:** 48.5% (63 de 130)
- **SL_FIRST:** 41.5% (54 de 130)
- **MFE Promedio:** 57.72 pts
- **MAE Promedio:** 42.92 pts
- **MFE/MAE Ratio:** 5.92
- **Good Entries:** 66.2% (MFE > MAE)
- **R:R Promedio:** 2.63

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (111 oportunidades)

- **WR Teórico:** 69.4% (si se hubieran ejecutado)
- **TP_FIRST:** 47.7% (53 de 111)
- **SL_FIRST:** 48.6% (54 de 111)
- **MFE Promedio:** 69.71 pts
- **MAE Promedio:** 47.34 pts
- **MFE/MAE Ratio:** 4.58
- **Good Entries:** 64.0% (MFE > MAE)
- **R:R Promedio:** 2.29

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (14 oportunidades)

- **WR Teórico:** 42.9% (si se hubieran ejecutado)
- **TP_FIRST:** 7.1% (1 de 14)
- **SL_FIRST:** 92.9% (13 de 14)
- **MFE Promedio:** 38.55 pts
- **MAE Promedio:** 75.65 pts
- **MFE/MAE Ratio:** 0.79
- **Good Entries:** 14.3% (MFE > MAE)
- **R:R Promedio:** 2.26

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 9 | 184 | 101 | 130 |
| **TP_FIRST %** | 44.4% | 60.3% | 56.4% | 48.5% |
| **Good Entries %** | 44.4% | 70.1% | 73.3% | 66.2% |
| **MFE/MAE Ratio** | 1.37 | 4.04 | 6.09 | 5.92 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

🚨 **CRÍTICO: El rango 2.0-3.0 ATR contiene 101 oportunidades de BUENA CALIDAD**
   - WR Teórico: 61.4%
   - Good Entries: 73.3%
   - **ACCIÓN:** Considerar aumentar MaxDistanceToRegister_ATR_Normal de 2.0 a 3.0

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 130 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 56.9%
   - Good Entries: 66.2%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 15.37 pts
- **Mediana:** 16.63 pts
- **Min/Max:** 6.52 / 17.58 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 16.63 |
| P70 | 17.27 |
| P80 | 17.27 |
| P90 | 17.58 |
| P95 | 17.73 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 24.97 pts
- **Mediana:** 25.75 pts
- **Min/Max:** 10.25 / 32.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 25.75 |
| P70 | 25.75 |
| P80 | 27.50 |
| P90 | 32.50 |
| P95 | 35.00 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 17; // Era 60
public int MaxTPDistancePoints { get; set; } = 32; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 17.6pts, TP: 32.5pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (33.3%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.333) / 0.333
R:R_min = 2.00
```

**Estado actual:** R:R promedio = 1.65
**Gap:** 0.35 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **17** (P90 real)
2. **MaxTPDistancePoints:** 120 → **32** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.65) < R:R mínimo (2.00)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=33.3%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-17 16:51:34*