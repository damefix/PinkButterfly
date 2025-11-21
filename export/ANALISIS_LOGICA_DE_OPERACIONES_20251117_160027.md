# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-17 16:07:51
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_160027.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251117_160027.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 13
- **Win Rate:** 30.8% (4/13)
- **Profit Factor:** 1.01
- **Avg R:R Planeado:** 1.60
- **R:R Mínimo para Break-Even:** 2.25

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 21 puntos
   - TP máximo observado: 49 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.60
   - R:R necesario: 2.25
   - **Gap:** 0.65

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8139 | 34.8% |
| Bullish | 9066 | 38.7% |
| Bearish | 6213 | 26.5% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.082
- **Score Min/Max:** [-1.000, 1.000]
- **Componentes (promedio):**
  - EMA20 Slope: 0.044
  - EMA50 Cross: 0.197
  - BOS Count: 0.004
  - Regression 24h: 0.092

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 1.000 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 34.8% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.8%)

**Posibles causas:**
- **BOS Score bajo (0.004):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio 0.082 indica poca señal direccional
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
| StructureFusion | 80183 | 100.0% | 100.0% |
| ProximityAnalyzer | 1324 | 1.7% | 1.7% |
| DFM_Evaluated | 281 | 21.2% | 0.4% |
| DFM_Passed | 281 | 100.0% | 0.4% |
| RiskCalculator | 2702 | 961.6% | 3.4% |
| Risk_Accepted | 1 | 0.0% | 0.0% |
| TradeManager | 13 | 1300.0% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 2701 señales, -100.0%)
- **Tasa de conversión final:** 0.02% (de 80183 zonas iniciales → 13 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 849 | 63.3% |
| ENTRY_TOO_FAR | 350 | 26.1% |
| TP_CHECK_FAIL | 125 | 9.3% |
| NO_SL | 17 | 1.3% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (849 rechazos, 63.3%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 1269 | 95.3% |
| P0_ANY_DIR | 63 | 4.7% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 66.50 pts (máxima ganancia flotante)
- **MAE Promedio:** 29.52 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 3.03

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 3 | 23.1% |
| SL_FIRST (precio fue hacia SL) | 10 | 76.9% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 53.8%
- **Entradas Malas (MAE > MFE):** 46.2%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 13 | 3 | 10 | 23.1% | 66.50 | 29.52 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0003 | SELL | 5.50 | 28.50 | 0.19 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0005 | SELL | 65.00 | 24.75 | 2.63 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0006 | SELL | 122.50 | 24.75 | 4.95 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0007 | SELL | 192.25 | 24.75 | 7.77 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0008 | SELL | 241.50 | 24.75 | 9.76 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0011 | SELL | 63.25 | 21.50 | 2.94 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0013 | SELL | 30.00 | 31.75 | 0.94 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0014 | SELL | 5.50 | 43.25 | 0.13 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0016 | SELL | 53.25 | 7.00 | 7.61 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0019 | SELL | 30.75 | 50.25 | 0.61 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0026 | SELL | 13.75 | 41.50 | 0.33 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0027 | SELL | 13.75 | 36.00 | 0.38 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0029 | SELL | 27.50 | 25.00 | 1.10 | SL_FIRST | CLOSED | 👍 Entrada correcta |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 588

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 202 | 46.5% | 51.5% | 6.07 | 67.8% | 2.27 | ✅ BUENA CALIDAD - Considerar incluir |
| 2.0-3.0 ATR (Cerca) | 100 | 64.0% | 52.0% | 6.58 | 75.0% | 2.34 | ✅ BUENA CALIDAD - Considerar incluir |
| 3.0-5.0 ATR (Media) | 139 | 61.2% | 47.5% | 5.78 | 69.1% | 2.48 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 121 | 57.0% | 42.1% | 3.66 | 52.1% | 2.18 | ⚠️ CALIDAD MEDIA - Revisar |
| >10.0 ATR (Muy lejos) | 26 | 0.0% | 3.8% | 0.07 | 3.8% | 1.90 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (202 oportunidades)

- **WR Teórico:** 46.5% (si se hubieran ejecutado)
- **TP_FIRST:** 51.5% (104 de 202)
- **SL_FIRST:** 41.1% (83 de 202)
- **MFE Promedio:** 49.30 pts
- **MAE Promedio:** 27.58 pts
- **MFE/MAE Ratio:** 6.07
- **Good Entries:** 67.8% (MFE > MAE)
- **R:R Promedio:** 2.27

**✅ BUENA CALIDAD - Considerar incluir**

**2.0-3.0 ATR (Cerca)** (100 oportunidades)

- **WR Teórico:** 64.0% (si se hubieran ejecutado)
- **TP_FIRST:** 52.0% (52 de 100)
- **SL_FIRST:** 37.0% (37 de 100)
- **MFE Promedio:** 67.72 pts
- **MAE Promedio:** 37.12 pts
- **MFE/MAE Ratio:** 6.58
- **Good Entries:** 75.0% (MFE > MAE)
- **R:R Promedio:** 2.34

**✅ BUENA CALIDAD - Considerar incluir**

**3.0-5.0 ATR (Media)** (139 oportunidades)

- **WR Teórico:** 61.2% (si se hubieran ejecutado)
- **TP_FIRST:** 47.5% (66 de 139)
- **SL_FIRST:** 43.2% (60 de 139)
- **MFE Promedio:** 72.34 pts
- **MAE Promedio:** 41.09 pts
- **MFE/MAE Ratio:** 5.78
- **Good Entries:** 69.1% (MFE > MAE)
- **R:R Promedio:** 2.48

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (121 oportunidades)

- **WR Teórico:** 57.0% (si se hubieran ejecutado)
- **TP_FIRST:** 42.1% (51 de 121)
- **SL_FIRST:** 54.5% (66 de 121)
- **MFE Promedio:** 70.73 pts
- **MAE Promedio:** 47.75 pts
- **MFE/MAE Ratio:** 3.66
- **Good Entries:** 52.1% (MFE > MAE)
- **R:R Promedio:** 2.18

**⚠️ CALIDAD MEDIA - Revisar**

**>10.0 ATR (Muy lejos)** (26 oportunidades)

- **WR Teórico:** 0.0% (si se hubieran ejecutado)
- **TP_FIRST:** 3.8% (1 de 26)
- **SL_FIRST:** 96.2% (25 de 26)
- **MFE Promedio:** 9.94 pts
- **MAE Promedio:** 77.83 pts
- **MFE/MAE Ratio:** 0.07
- **Good Entries:** 3.8% (MFE > MAE)
- **R:R Promedio:** 1.90

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 13 | 202 | 100 | 139 |
| **TP_FIRST %** | 23.1% | 51.5% | 52.0% | 47.5% |
| **Good Entries %** | 53.8% | 67.8% | 75.0% | 69.1% |
| **MFE/MAE Ratio** | 3.03 | 6.07 | 6.58 | 5.78 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

🚨 **CRÍTICO: El rango 2.0-3.0 ATR contiene 100 oportunidades de BUENA CALIDAD**
   - WR Teórico: 64.0%
   - Good Entries: 75.0%
   - **ACCIÓN:** Considerar aumentar MaxDistanceToRegister_ATR_Normal de 2.0 a 3.0

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 139 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 61.2%
   - Good Entries: 69.1%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 13.34 pts
- **Mediana:** 16.57 pts
- **Min/Max:** 6.50 / 21.43 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 16.57 |
| P70 | 17.27 |
| P80 | 17.33 |
| P90 | 19.89 |
| P95 | 22.59 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 21.71 pts
- **Mediana:** 25.75 pts
- **Min/Max:** 10.25 / 49.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 25.75 |
| P70 | 25.75 |
| P80 | 26.10 |
| P90 | 40.40 |
| P95 | 55.45 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 19; // Era 60
public int MaxTPDistancePoints { get; set; } = 40; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 19.9pts, TP: 40.4pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (30.8%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.308) / 0.308
R:R_min = 2.25
```

**Estado actual:** R:R promedio = 1.60
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
1. **MaxSLDistancePoints:** 60 → **19** (P90 real)
2. **MaxTPDistancePoints:** 120 → **40** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.60) < R:R mínimo (2.25)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=30.8%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-17 16:07:51*