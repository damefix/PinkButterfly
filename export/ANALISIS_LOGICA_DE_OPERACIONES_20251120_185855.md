# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-20 19:06:12
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251120_185855.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251120_185855.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 7
- **Win Rate:** 57.1% (4/7)
- **Profit Factor:** 2.75
- **Avg R:R Planeado:** 2.48
- **R:R Mínimo para Break-Even:** 0.75

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 27 puntos
   - TP máximo observado: 45 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.48
   - R:R necesario: 0.75
   - **Gap:** -1.73

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bullish | 1024 | 32.8% |
| Neutral | 1078 | 34.5% |
| Bearish | 1024 | 32.8% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.021
- **Score Min/Max:** [-0.750, 0.750]
- **Componentes (promedio):**
  - EMA20 Slope: 0.008
  - EMA50 Cross: 0.002
  - BOS Count: 0.000
  - Regression 24h: 0.090

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.750 (apenas supera threshold)
- Score mínimo observado: -0.750 (apenas supera threshold)
- **Consecuencia:** Sistema queda 34.5% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.5%)

**Posibles causas:**
- **BOS Score bajo (0.000):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio 0.021 indica poca señal direccional
- **Mercado lateral:** Scores reales [-0.75, 0.75] muy cercanos a 0

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
| StructureFusion | 44909 | 100.0% | 100.0% |
| ProximityAnalyzer | 3850 | 8.6% | 8.6% |
| DFM_Evaluated | 528 | 13.7% | 1.2% |
| DFM_Passed | 528 | 100.0% | 1.2% |
| RiskCalculator | 8286 | 1569.3% | 18.5% |
| Risk_Accepted | 3 | 0.0% | 0.0% |
| TradeManager | 7 | 233.3% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 8283 señales, -100.0%)
- **Tasa de conversión final:** 0.02% (de 44909 zonas iniciales → 7 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 4,758 | 71.6% |
| ENTRY_TOO_FAR | 1,593 | 24.0% |
| TP_CHECK_FAIL | 296 | 4.5% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (4,758 rechazos, 71.6%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 2697 | 75.4% |
| P0_ANY_DIR | 881 | 24.6% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 63.61 pts (máxima ganancia flotante)
- **MAE Promedio:** 21.07 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 145.62

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 3 | 42.9% |
| SL_FIRST (precio fue hacia SL) | 4 | 57.1% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 85.7%
- **Entradas Malas (MAE > MFE):** 14.3%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 7 | 3 | 4 | 42.9% | 63.61 | 21.07 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0002 | SELL | 62.25 | 15.25 | 4.08 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0006 | SELL | 52.50 | 72.50 | 0.72 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0008 | SELL | 59.25 | 13.75 | 4.31 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0011 | SELL | 58.25 | 15.00 | 3.88 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0013 | SELL | 56.75 | 15.50 | 3.66 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0013_2 | SELL | 56.75 | 15.50 | 3.66 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0020 | SELL | 99.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 849

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 274 | 71.5% | 57.3% | 4.69 | 81.8% | 1.88 | ✅ BUENA CALIDAD - Considerar incluir |
| 2.0-3.0 ATR (Cerca) | 128 | 76.6% | 53.9% | 4.64 | 85.2% | 1.85 | ✅ BUENA CALIDAD - Considerar incluir |
| 3.0-5.0 ATR (Media) | 210 | 80.5% | 59.0% | 3.22 | 91.0% | 2.17 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 172 | 76.7% | 40.1% | 2.59 | 95.9% | 2.06 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 65 | 96.9% | 53.8% | 4.07 | 100.0% | 2.23 | ✅ BUENA CALIDAD - Considerar incluir |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (274 oportunidades)

- **WR Teórico:** 71.5% (si se hubieran ejecutado)
- **TP_FIRST:** 57.3% (157 de 274)
- **SL_FIRST:** 30.3% (83 de 274)
- **MFE Promedio:** 70.35 pts
- **MAE Promedio:** 22.84 pts
- **MFE/MAE Ratio:** 4.69
- **Good Entries:** 81.8% (MFE > MAE)
- **R:R Promedio:** 1.88

**✅ BUENA CALIDAD - Considerar incluir**

**2.0-3.0 ATR (Cerca)** (128 oportunidades)

- **WR Teórico:** 76.6% (si se hubieran ejecutado)
- **TP_FIRST:** 53.9% (69 de 128)
- **SL_FIRST:** 35.2% (45 de 128)
- **MFE Promedio:** 74.39 pts
- **MAE Promedio:** 22.16 pts
- **MFE/MAE Ratio:** 4.64
- **Good Entries:** 85.2% (MFE > MAE)
- **R:R Promedio:** 1.85

**✅ BUENA CALIDAD - Considerar incluir**

**3.0-5.0 ATR (Media)** (210 oportunidades)

- **WR Teórico:** 80.5% (si se hubieran ejecutado)
- **TP_FIRST:** 59.0% (124 de 210)
- **SL_FIRST:** 36.7% (77 de 210)
- **MFE Promedio:** 75.44 pts
- **MAE Promedio:** 25.60 pts
- **MFE/MAE Ratio:** 3.22
- **Good Entries:** 91.0% (MFE > MAE)
- **R:R Promedio:** 2.17

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (172 oportunidades)

- **WR Teórico:** 76.7% (si se hubieran ejecutado)
- **TP_FIRST:** 40.1% (69 de 172)
- **SL_FIRST:** 57.0% (98 de 172)
- **MFE Promedio:** 71.24 pts
- **MAE Promedio:** 26.78 pts
- **MFE/MAE Ratio:** 2.59
- **Good Entries:** 95.9% (MFE > MAE)
- **R:R Promedio:** 2.06

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (65 oportunidades)

- **WR Teórico:** 96.9% (si se hubieran ejecutado)
- **TP_FIRST:** 53.8% (35 de 65)
- **SL_FIRST:** 46.2% (30 de 65)
- **MFE Promedio:** 58.59 pts
- **MAE Promedio:** 18.91 pts
- **MFE/MAE Ratio:** 4.07
- **Good Entries:** 100.0% (MFE > MAE)
- **R:R Promedio:** 2.23

**✅ BUENA CALIDAD - Considerar incluir**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 7 | 274 | 128 | 210 |
| **TP_FIRST %** | 42.9% | 57.3% | 53.9% | 59.0% |
| **Good Entries %** | 85.7% | 81.8% | 85.2% | 91.0% |
| **MFE/MAE Ratio** | 145.62 | 4.69 | 4.64 | 3.22 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

🚨 **CRÍTICO: El rango 2.0-3.0 ATR contiene 128 oportunidades de BUENA CALIDAD**
   - WR Teórico: 76.6%
   - Good Entries: 85.2%
   - **ACCIÓN:** Considerar aumentar MaxDistanceToRegister_ATR_Normal de 2.0 a 3.0

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 210 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 80.5%
   - Good Entries: 91.0%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 15.96 pts
- **Mediana:** 13.97 pts
- **Min/Max:** 9.58 / 26.76 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 13.97 |
| P70 | 19.47 |
| P80 | 23.51 |
| P90 | 27.84 |
| P95 | 30.01 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 36.21 pts
- **Mediana:** 34.00 pts
- **Min/Max:** 26.00 / 45.25 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 34.00 |
| P70 | 41.05 |
| P80 | 44.95 |
| P90 | 45.35 |
| P95 | 45.55 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 27; // Era 60
public int MaxTPDistancePoints { get; set; } = 45; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 27.8pts, TP: 45.4pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (57.1%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.571) / 0.571
R:R_min = 0.75
```

**Estado actual:** R:R promedio = 2.48
**Gap:** -1.73 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **45** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.48) < R:R mínimo (0.75)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=57.1%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-20 19:06:12*