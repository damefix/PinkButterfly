# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-18 16:26:44
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251118_162243.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251118_162243.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 9
- **Win Rate:** 44.4% (4/9)
- **Profit Factor:** 1.00
- **Avg R:R Planeado:** 2.57
- **R:R Mínimo para Break-Even:** 1.25

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 20 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.57
   - R:R necesario: 1.25
   - **Gap:** -1.32

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bearish | 202 | 32.2% |
| Neutral | 230 | 36.7% |
| Bullish | 195 | 31.1% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** -0.026
- **Score Min/Max:** [-0.970, 0.970]
- **Componentes (promedio):**
  - EMA20 Slope: -0.029
  - EMA50 Cross: -0.069
  - BOS Count: -0.004
  - Regression 24h: 0.006

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.970 (apenas supera threshold)
- Score mínimo observado: -0.970 (apenas supera threshold)
- **Consecuencia:** Sistema queda 36.7% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (36.7%)

**Posibles causas:**
- **BOS Score bajo (-0.004):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio -0.026 indica poca señal direccional
- **Mercado lateral:** Scores reales [-0.97, 0.97] muy cercanos a 0

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
| StructureFusion | 2487 | 100.0% | 100.0% |
| ProximityAnalyzer | 794 | 31.9% | 31.9% |
| DFM_Evaluated | 172 | 21.7% | 6.9% |
| DFM_Passed | 172 | 100.0% | 6.9% |
| RiskCalculator | 1414 | 822.1% | 56.9% |
| Risk_Accepted | 2 | 0.1% | 0.1% |
| TradeManager | 9 | 450.0% | 0.4% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 1412 señales, -99.9%)
- **Tasa de conversión final:** 0.36% (de 2487 zonas iniciales → 9 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 507 | 65.6% |
| ENTRY_TOO_FAR | 164 | 21.2% |
| NO_SL | 89 | 11.5% |
| TP_CHECK_FAIL | 13 | 1.7% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (507 rechazos, 65.6%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 741 | 89.8% |
| P0_ANY_DIR | 84 | 10.2% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 41.61 pts (máxima ganancia flotante)
- **MAE Promedio:** 18.81 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 223.53

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 5 | 55.6% |
| SL_FIRST (precio fue hacia SL) | 4 | 44.4% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 55.6%
- **Entradas Malas (MAE > MFE):** 44.4%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 9 | 5 | 4 | 55.6% | 41.61 | 18.81 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0002 | SELL | 17.25 | 31.25 | 0.55 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0003 | SELL | 43.25 | 11.25 | 3.84 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0006 | SELL | 48.50 | 10.50 | 4.62 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0021 | SELL | 25.00 | 30.25 | 0.83 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0024 | SELL | 17.75 | 34.25 | 0.52 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0024_2 | SELL | 17.75 | 34.25 | 0.52 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0030 | SELL | 51.00 | 17.50 | 2.91 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0043 | SELL | 77.00 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0043_2 | SELL | 77.00 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 273

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 97 | 39.2% | 68.0% | 5.05 | 54.6% | 2.00 | ⚠️ CALIDAD MEDIA - Revisar |
| 2.0-3.0 ATR (Cerca) | 40 | 85.0% | 85.0% | 4.95 | 72.5% | 1.88 | ✅ BUENA CALIDAD - Considerar incluir |
| 3.0-5.0 ATR (Media) | 95 | 91.6% | 76.8% | 7.85 | 82.1% | 2.16 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 40 | 82.5% | 77.5% | 2.03 | 82.5% | 2.00 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 1 | 0.0% | 0.0% | 0.00 | 0.0% | 1.26 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (97 oportunidades)

- **WR Teórico:** 39.2% (si se hubieran ejecutado)
- **TP_FIRST:** 68.0% (66 de 97)
- **SL_FIRST:** 29.9% (29 de 97)
- **MFE Promedio:** 46.05 pts
- **MAE Promedio:** 41.29 pts
- **MFE/MAE Ratio:** 5.05
- **Good Entries:** 54.6% (MFE > MAE)
- **R:R Promedio:** 2.00

**⚠️ CALIDAD MEDIA - Revisar**

**2.0-3.0 ATR (Cerca)** (40 oportunidades)

- **WR Teórico:** 85.0% (si se hubieran ejecutado)
- **TP_FIRST:** 85.0% (34 de 40)
- **SL_FIRST:** 15.0% (6 de 40)
- **MFE Promedio:** 67.61 pts
- **MAE Promedio:** 42.86 pts
- **MFE/MAE Ratio:** 4.95
- **Good Entries:** 72.5% (MFE > MAE)
- **R:R Promedio:** 1.88

**✅ BUENA CALIDAD - Considerar incluir**

**3.0-5.0 ATR (Media)** (95 oportunidades)

- **WR Teórico:** 91.6% (si se hubieran ejecutado)
- **TP_FIRST:** 76.8% (73 de 95)
- **SL_FIRST:** 22.1% (21 de 95)
- **MFE Promedio:** 73.96 pts
- **MAE Promedio:** 24.82 pts
- **MFE/MAE Ratio:** 7.85
- **Good Entries:** 82.1% (MFE > MAE)
- **R:R Promedio:** 2.16

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (40 oportunidades)

- **WR Teórico:** 82.5% (si se hubieran ejecutado)
- **TP_FIRST:** 77.5% (31 de 40)
- **SL_FIRST:** 20.0% (8 de 40)
- **MFE Promedio:** 103.81 pts
- **MAE Promedio:** 63.41 pts
- **MFE/MAE Ratio:** 2.03
- **Good Entries:** 82.5% (MFE > MAE)
- **R:R Promedio:** 2.00

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (1 oportunidades)

- **WR Teórico:** 0.0% (si se hubieran ejecutado)
- **TP_FIRST:** 0.0% (0 de 1)
- **SL_FIRST:** 100.0% (1 de 1)
- **MFE Promedio:** 0.00 pts
- **MAE Promedio:** 187.50 pts
- **MFE/MAE Ratio:** 0.00
- **Good Entries:** 0.0% (MFE > MAE)
- **R:R Promedio:** 1.26

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 9 | 97 | 40 | 95 |
| **TP_FIRST %** | 55.6% | 68.0% | 85.0% | 76.8% |
| **Good Entries %** | 55.6% | 54.6% | 72.5% | 82.1% |
| **MFE/MAE Ratio** | 223.53 | 5.05 | 4.95 | 7.85 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

🚨 **CRÍTICO: El rango 2.0-3.0 ATR contiene 40 oportunidades de BUENA CALIDAD**
   - WR Teórico: 85.0%
   - Good Entries: 72.5%
   - **ACCIÓN:** Considerar aumentar MaxDistanceToRegister_ATR_Normal de 2.0 a 3.0

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 95 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 91.6%
   - Good Entries: 82.1%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 10.63 pts
- **Mediana:** 10.14 pts
- **Min/Max:** 5.14 / 20.23 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 10.14 |
| P70 | 11.67 |
| P80 | 12.83 |
| P90 | 20.23 |
| P95 | 23.93 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 28.06 pts
- **Mediana:** 28.25 pts
- **Min/Max:** 11.50 / 54.25 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 28.25 |
| P70 | 39.00 |
| P80 | 39.00 |
| P90 | 54.25 |
| P95 | 61.88 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 20; // Era 60
public int MaxTPDistancePoints { get; set; } = 54; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 20.2pts, TP: 54.2pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (44.4%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.444) / 0.444
R:R_min = 1.25
```

**Estado actual:** R:R promedio = 2.57
**Gap:** -1.32 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **20** (P90 real)
2. **MaxTPDistancePoints:** 120 → **54** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.57) < R:R mínimo (1.25)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=44.4%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-18 16:26:44*