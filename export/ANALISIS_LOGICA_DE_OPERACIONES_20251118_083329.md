# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-18 08:39:10
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251118_083329.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251118_083329.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 0
- **Win Rate:** 0.0% (0/0)
- **Profit Factor:** 0.00
- **Avg R:R Planeado:** 0.00
- **R:R Mínimo para Break-Even:** 1.75

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 0 puntos
   - TP máximo observado: 0 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 0.00
   - R:R necesario: 1.75
   - **Gap:** 1.75

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 947 | 30.3% |
| Bearish | 975 | 31.2% |
| Bullish | 1205 | 38.5% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.052
- **Score Min/Max:** [-1.000, 0.990]
- **Componentes (promedio):**
  - EMA20 Slope: 0.028
  - EMA50 Cross: 0.077
  - BOS Count: 0.007
  - Regression 24h: 0.114

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.990 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 30.3% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (30.3%)

**Posibles causas:**
- **BOS Score bajo (0.007):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio 0.052 indica poca señal direccional
- **Mercado lateral:** Scores reales [-1.00, 0.99] muy cercanos a 0

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
| StructureFusion | 11129 | 100.0% | 100.0% |
| ProximityAnalyzer | 2007 | 18.0% | 18.0% |
| DFM_Evaluated | 574 | 28.6% | 5.2% |
| DFM_Passed | 574 | 100.0% | 5.2% |
| RiskCalculator | 4255 | 741.3% | 38.2% |
| Risk_Accepted | 2 | 0.0% | 0.0% |
| TradeManager | 0 | 0.0% | 0.0% |

**Análisis:**
- **Mayor caída:** TradeManager (pierde 2 señales, -100.0%)
- **Tasa de conversión final:** 0.00% (de 11129 zonas iniciales → 0 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 861 | 46.3% |
| ENTRY_TOO_FAR | 456 | 24.5% |
| NO_SL | 337 | 18.1% |
| TP_CHECK_FAIL | 205 | 11.0% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (861 rechazos, 46.3%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 1623 | 88.6% |
| P0_ANY_DIR | 208 | 11.4% |

### 3.5 Análisis MFE/MAE (Excursión del Precio)

⚠️ **No hay datos OHLC disponibles ([PIPE] logs)**

Para activar este análisis:
1. En `EngineConfig.cs`: `EnableOHLCLogging = true`
2. Ejecutar backtest
3. El log generará trazas `[PIPE] TF=X O=Y H=Z L=W C=V`

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 945

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 315 | 55.9% | 57.5% | 7.45 | 57.5% | 2.56 | ⚠️ CALIDAD MEDIA - Revisar |
| 2.0-3.0 ATR (Cerca) | 155 | 54.8% | 56.1% | 0.00 | 56.1% | 2.34 | ⚠️ CALIDAD MEDIA - Revisar |
| 3.0-5.0 ATR (Media) | 297 | 65.0% | 65.0% | 0.00 | 65.0% | 2.41 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 131 | 70.2% | 70.2% | 0.00 | 70.2% | 2.08 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 47 | 100.0% | 100.0% | 0.00 | 100.0% | 2.97 | ✅ BUENA CALIDAD - Considerar incluir |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (315 oportunidades)

- **WR Teórico:** 55.9% (si se hubieran ejecutado)
- **TP_FIRST:** 57.5% (181 de 315)
- **SL_FIRST:** 0.0% (0 de 315)
- **MFE Promedio:** 138.54 pts
- **MAE Promedio:** 2.75 pts
- **MFE/MAE Ratio:** 7.45
- **Good Entries:** 57.5% (MFE > MAE)
- **R:R Promedio:** 2.56

**⚠️ CALIDAD MEDIA - Revisar**

**2.0-3.0 ATR (Cerca)** (155 oportunidades)

- **WR Teórico:** 54.8% (si se hubieran ejecutado)
- **TP_FIRST:** 56.1% (87 de 155)
- **SL_FIRST:** 0.0% (0 de 155)
- **MFE Promedio:** 136.43 pts
- **MAE Promedio:** 0.00 pts
- **MFE/MAE Ratio:** 0.00
- **Good Entries:** 56.1% (MFE > MAE)
- **R:R Promedio:** 2.34

**⚠️ CALIDAD MEDIA - Revisar**

**3.0-5.0 ATR (Media)** (297 oportunidades)

- **WR Teórico:** 65.0% (si se hubieran ejecutado)
- **TP_FIRST:** 65.0% (193 de 297)
- **SL_FIRST:** 0.0% (0 de 297)
- **MFE Promedio:** 148.53 pts
- **MAE Promedio:** 0.00 pts
- **MFE/MAE Ratio:** 0.00
- **Good Entries:** 65.0% (MFE > MAE)
- **R:R Promedio:** 2.41

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (131 oportunidades)

- **WR Teórico:** 70.2% (si se hubieran ejecutado)
- **TP_FIRST:** 70.2% (92 de 131)
- **SL_FIRST:** 0.0% (0 de 131)
- **MFE Promedio:** 158.24 pts
- **MAE Promedio:** 0.00 pts
- **MFE/MAE Ratio:** 0.00
- **Good Entries:** 70.2% (MFE > MAE)
- **R:R Promedio:** 2.08

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (47 oportunidades)

- **WR Teórico:** 100.0% (si se hubieran ejecutado)
- **TP_FIRST:** 100.0% (47 de 47)
- **SL_FIRST:** 0.0% (0 de 47)
- **MFE Promedio:** 168.09 pts
- **MAE Promedio:** 0.00 pts
- **MFE/MAE Ratio:** 0.00
- **Good Entries:** 100.0% (MFE > MAE)
- **R:R Promedio:** 2.97

**✅ BUENA CALIDAD - Considerar incluir**

**💡 RECOMENDACIONES BASADAS EN DATOS:**

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 297 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 65.0%
   - Good Entries: 65.0%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

### 4.2 Distribución Take Profit (Puntos)

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

### 4.4 R:R Óptimo

**Para Win Rate actual (0.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.000) / 0.000
R:R_min = 1.75
```

**Estado actual:** R:R promedio = 0.00
**Gap:** 1.75 (necesitas mejorar R:R)

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
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (0.00) < R:R mínimo (1.75)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=0.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-18 08:39:10*