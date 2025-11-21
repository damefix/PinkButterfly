# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-17 11:12:56
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_110219.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251117_110219.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 12
- **Win Rate:** 16.7% (2/12)
- **Profit Factor:** 0.49
- **Avg R:R Planeado:** 2.33
- **R:R Mínimo para Break-Even:** 5.00

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 30 puntos
   - TP máximo observado: 40 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.33
   - R:R necesario: 5.00
   - **Gap:** 2.67

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bullish | 10116 | 39.1% |
| Neutral | 8743 | 33.8% |
| Bearish | 7039 | 27.2% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.080
- **Score Min/Max:** [-1.000, 1.000]
- **Componentes (promedio):**
  - EMA20 Slope: 0.041
  - EMA50 Cross: 0.180
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
| StructureFusion | 79671 | 100.0% | 100.0% |
| ProximityAnalyzer | 1739 | 2.2% | 2.2% |
| DFM_Evaluated | 359 | 20.6% | 0.5% |
| DFM_Passed | 359 | 100.0% | 0.5% |
| RiskCalculator | 3812 | 1061.8% | 4.8% |
| Risk_Accepted | 1 | 0.0% | 0.0% |
| TradeManager | 12 | 1200.0% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 3811 señales, -100.0%)
- **Tasa de conversión final:** 0.02% (de 79671 zonas iniciales → 12 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 791 | 52.2% |
| ENTRY_TOO_FAR | 414 | 27.3% |
| NO_SL | 162 | 10.7% |
| TP_CHECK_FAIL | 148 | 9.8% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (791 rechazos, 52.2%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 1626 | 97.0% |
| P0_ANY_DIR | 50 | 3.0% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 37.38 pts (máxima ganancia flotante)
- **MAE Promedio:** 18.06 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 3.16

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 6 | 50.0% |
| SL_FIRST (precio fue hacia SL) | 6 | 50.0% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 83.3%
- **Entradas Malas (MAE > MFE):** 16.7%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 12 | 6 | 6 | 50.0% | 37.38 | 18.06 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | BUY | 31.50 | 19.00 | 1.66 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0006 | SELL | 137.00 | 28.25 | 4.85 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0007 | BUY | 17.00 | 14.00 | 1.21 | SL_FIRST | CLOSED | 👍 Entrada correcta |
| T0008 | BUY | 17.00 | 8.75 | 1.94 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0009 | BUY | 15.25 | 8.75 | 1.74 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0010 | SELL | 12.75 | 10.50 | 1.21 | SL_FIRST | CLOSED | 👍 Entrada correcta |
| T0011 | SELL | 87.50 | 4.50 | 19.44 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0013 | SELL | 23.50 | 17.00 | 1.38 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0015 | SELL | 32.00 | 20.75 | 1.54 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0015_2 | SELL | 32.00 | 20.75 | 1.54 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0018 | SELL | 19.00 | 27.00 | 0.70 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0019 | SELL | 24.00 | 37.50 | 0.64 | SL_FIRST | CLOSED | ❌ Entrada muy mala |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 807

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 265 | 46.0% | 35.5% | 5.16 | 46.8% | 2.28 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 99 | 78.8% | 51.5% | 7.32 | 78.8% | 1.91 | ✅ BUENA CALIDAD - Considerar incluir |
| 3.0-5.0 ATR (Media) | 249 | 63.1% | 55.4% | 3.14 | 61.0% | 2.05 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 129 | 57.4% | 54.3% | 3.22 | 62.0% | 1.78 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 65 | 100.0% | 27.7% | 1.11 | 67.7% | 1.93 | ✅ BUENA CALIDAD - Considerar incluir |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (265 oportunidades)

- **WR Teórico:** 46.0% (si se hubieran ejecutado)
- **TP_FIRST:** 35.5% (94 de 265)
- **SL_FIRST:** 54.7% (145 de 265)
- **MFE Promedio:** 58.57 pts
- **MAE Promedio:** 45.26 pts
- **MFE/MAE Ratio:** 5.16
- **Good Entries:** 46.8% (MFE > MAE)
- **R:R Promedio:** 2.28

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (99 oportunidades)

- **WR Teórico:** 78.8% (si se hubieran ejecutado)
- **TP_FIRST:** 51.5% (51 de 99)
- **SL_FIRST:** 43.4% (43 de 99)
- **MFE Promedio:** 65.84 pts
- **MAE Promedio:** 18.91 pts
- **MFE/MAE Ratio:** 7.32
- **Good Entries:** 78.8% (MFE > MAE)
- **R:R Promedio:** 1.91

**✅ BUENA CALIDAD - Considerar incluir**

**3.0-5.0 ATR (Media)** (249 oportunidades)

- **WR Teórico:** 63.1% (si se hubieran ejecutado)
- **TP_FIRST:** 55.4% (138 de 249)
- **SL_FIRST:** 37.3% (93 de 249)
- **MFE Promedio:** 62.82 pts
- **MAE Promedio:** 28.42 pts
- **MFE/MAE Ratio:** 3.14
- **Good Entries:** 61.0% (MFE > MAE)
- **R:R Promedio:** 2.05

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (129 oportunidades)

- **WR Teórico:** 57.4% (si se hubieran ejecutado)
- **TP_FIRST:** 54.3% (70 de 129)
- **SL_FIRST:** 41.1% (53 de 129)
- **MFE Promedio:** 77.57 pts
- **MAE Promedio:** 22.61 pts
- **MFE/MAE Ratio:** 3.22
- **Good Entries:** 62.0% (MFE > MAE)
- **R:R Promedio:** 1.78

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (65 oportunidades)

- **WR Teórico:** 100.0% (si se hubieran ejecutado)
- **TP_FIRST:** 27.7% (18 de 65)
- **SL_FIRST:** 72.3% (47 de 65)
- **MFE Promedio:** 30.62 pts
- **MAE Promedio:** 28.22 pts
- **MFE/MAE Ratio:** 1.11
- **Good Entries:** 67.7% (MFE > MAE)
- **R:R Promedio:** 1.93

**✅ BUENA CALIDAD - Considerar incluir**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 12 | 265 | 99 | 249 |
| **TP_FIRST %** | 50.0% | 35.5% | 51.5% | 55.4% |
| **Good Entries %** | 83.3% | 46.8% | 78.8% | 61.0% |
| **MFE/MAE Ratio** | 3.16 | 5.16 | 7.32 | 3.14 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

🚨 **CRÍTICO: El rango 2.0-3.0 ATR contiene 99 oportunidades de BUENA CALIDAD**
   - WR Teórico: 78.8%
   - Good Entries: 78.8%
   - **ACCIÓN:** Considerar aumentar MaxDistanceToRegister_ATR_Normal de 2.0 a 3.0

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 249 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 63.1%
   - Good Entries: 61.0%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 11.29 pts
- **Mediana:** 4.16 pts
- **Min/Max:** 2.48 / 30.27 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 4.16 |
| P70 | 17.16 |
| P80 | 22.49 |
| P90 | 29.89 |
| P95 | 30.71 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 19.98 pts
- **Mediana:** 12.50 pts
- **Min/Max:** 6.75 / 40.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 12.50 |
| P70 | 26.05 |
| P80 | 40.20 |
| P90 | 40.50 |
| P95 | 40.50 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 29; // Era 60
public int MaxTPDistancePoints { get; set; } = 40; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 29.9pts, TP: 40.5pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (16.7%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.167) / 0.167
R:R_min = 5.00
```

**Estado actual:** R:R promedio = 2.33
**Gap:** 2.67 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **29** (P90 real)
2. **MaxTPDistancePoints:** 120 → **40** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.33) < R:R mínimo (5.00)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=16.7%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-17 11:12:56*