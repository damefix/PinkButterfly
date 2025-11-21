# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-19 11:56:59
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_114853.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_114853.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 1
- **Win Rate:** 100.0% (1/1)
- **Profit Factor:** 0.00
- **Avg R:R Planeado:** 1.92
- **R:R Mínimo para Break-Even:** 0.00

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 17 puntos
   - TP máximo observado: 32 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.92
   - R:R necesario: 0.00
   - **Gap:** -1.92

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bullish | 1186 | 37.9% |
| Neutral | 788 | 25.2% |
| Bearish | 1155 | 36.9% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.018
- **Score Min/Max:** [-1.000, 0.990]
- **Componentes (promedio):**
  - EMA20 Slope: -0.006
  - EMA50 Cross: 0.033
  - BOS Count: -0.020
  - Regression 24h: 0.083

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.990 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 25.2% Neutral → bias no diferencia tendencias

### 2.3 Estado: Bias Compuesto Funcionando Correctamente

✅ **Threshold actual:** 0.3 (correcto)
✅ **Bias Neutral:** 25.2% (aceptable)
✅ **Score promedio:** 0.018

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 17710 | 100.0% | 100.0% |
| ProximityAnalyzer | 2928 | 16.5% | 16.5% |
| DFM_Evaluated | 671 | 22.9% | 3.8% |
| DFM_Passed | 671 | 100.0% | 3.8% |
| RiskCalculator | 5269 | 785.2% | 29.8% |
| Risk_Accepted | 7 | 0.1% | 0.0% |
| TradeManager | 1 | 14.3% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 5262 señales, -99.9%)
- **Tasa de conversión final:** 0.01% (de 17710 zonas iniciales → 1 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 1,072 | 46.7% |
| ENTRY_TOO_FAR | 732 | 31.9% |
| TP_CHECK_FAIL | 300 | 13.1% |
| NO_SL | 192 | 8.4% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (1,072 rechazos, 46.7%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 3420 | 85.5% |
| P0_ANY_DIR | 580 | 14.5% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 157.50 pts (máxima ganancia flotante)
- **MAE Promedio:** 0.00 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 999.00

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 1 | 100.0% |
| SL_FIRST (precio fue hacia SL) | 0 | 0.0% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 100.0%
- **Entradas Malas (MAE > MFE):** 0.0%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 1 | 1 | 0 | 100.0% | 157.50 | 0.00 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0006 | SELL | 157.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 919

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 276 | 47.5% | 82.6% | 1.11 | 61.6% | 2.10 | ✅ BUENA CALIDAD - Considerar incluir |
| 2.0-3.0 ATR (Cerca) | 129 | 64.3% | 88.4% | 1.64 | 79.1% | 2.16 | ✅ BUENA CALIDAD - Considerar incluir |
| 3.0-5.0 ATR (Media) | 289 | 61.6% | 80.6% | 1.70 | 74.7% | 2.18 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 190 | 80.0% | 88.4% | 1.87 | 86.8% | 2.06 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 35 | 100.0% | 100.0% | 0.00 | 100.0% | 2.16 | ✅ BUENA CALIDAD - Considerar incluir |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (276 oportunidades)

- **WR Teórico:** 47.5% (si se hubieran ejecutado)
- **TP_FIRST:** 82.6% (228 de 276)
- **SL_FIRST:** 6.5% (18 de 276)
- **MFE Promedio:** 98.04 pts
- **MAE Promedio:** 66.02 pts
- **MFE/MAE Ratio:** 1.11
- **Good Entries:** 61.6% (MFE > MAE)
- **R:R Promedio:** 2.10

**✅ BUENA CALIDAD - Considerar incluir**

**2.0-3.0 ATR (Cerca)** (129 oportunidades)

- **WR Teórico:** 64.3% (si se hubieran ejecutado)
- **TP_FIRST:** 88.4% (114 de 129)
- **SL_FIRST:** 1.6% (2 de 129)
- **MFE Promedio:** 120.55 pts
- **MAE Promedio:** 52.79 pts
- **MFE/MAE Ratio:** 1.64
- **Good Entries:** 79.1% (MFE > MAE)
- **R:R Promedio:** 2.16

**✅ BUENA CALIDAD - Considerar incluir**

**3.0-5.0 ATR (Media)** (289 oportunidades)

- **WR Teórico:** 61.6% (si se hubieran ejecutado)
- **TP_FIRST:** 80.6% (233 de 289)
- **SL_FIRST:** 1.4% (4 de 289)
- **MFE Promedio:** 127.73 pts
- **MAE Promedio:** 50.00 pts
- **MFE/MAE Ratio:** 1.70
- **Good Entries:** 74.7% (MFE > MAE)
- **R:R Promedio:** 2.18

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (190 oportunidades)

- **WR Teórico:** 80.0% (si se hubieran ejecutado)
- **TP_FIRST:** 88.4% (168 de 190)
- **SL_FIRST:** 0.0% (0 de 190)
- **MFE Promedio:** 147.12 pts
- **MAE Promedio:** 47.74 pts
- **MFE/MAE Ratio:** 1.87
- **Good Entries:** 86.8% (MFE > MAE)
- **R:R Promedio:** 2.06

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (35 oportunidades)

- **WR Teórico:** 100.0% (si se hubieran ejecutado)
- **TP_FIRST:** 100.0% (35 de 35)
- **SL_FIRST:** 0.0% (0 de 35)
- **MFE Promedio:** 148.81 pts
- **MAE Promedio:** 0.00 pts
- **MFE/MAE Ratio:** 0.00
- **Good Entries:** 100.0% (MFE > MAE)
- **R:R Promedio:** 2.16

**✅ BUENA CALIDAD - Considerar incluir**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 1 | 276 | 129 | 289 |
| **TP_FIRST %** | 100.0% | 82.6% | 88.4% | 80.6% |
| **Good Entries %** | 100.0% | 61.6% | 79.1% | 74.7% |
| **MFE/MAE Ratio** | 999.00 | 1.11 | 1.64 | 1.70 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

🚨 **CRÍTICO: El rango 2.0-3.0 ATR contiene 129 oportunidades de BUENA CALIDAD**
   - WR Teórico: 64.3%
   - Good Entries: 79.1%
   - **ACCIÓN:** Considerar aumentar MaxDistanceToRegister_ATR_Normal de 2.0 a 3.0

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 289 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 61.6%
   - Good Entries: 74.7%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 16.63 pts
- **Mediana:** 16.63 pts
- **Min/Max:** 16.63 / 16.63 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 0.00 |
| P70 | 0.00 |
| P80 | 0.00 |
| P90 | 0.00 |
| P95 | 0.00 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 32.00 pts
- **Mediana:** 32.00 pts
- **Min/Max:** 32.00 / 32.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 0.00 |
| P70 | 0.00 |
| P80 | 0.00 |
| P90 | 0.00 |
| P95 | 0.00 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 0; // Era 60
public int MaxTPDistancePoints { get; set; } = 0; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 0.0pts, TP: 0.0pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (100.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 1.000) / 1.000
R:R_min = 0.00
```

**Estado actual:** R:R promedio = 1.92
**Gap:** -1.92 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **0** (P90 real)
2. **MaxTPDistancePoints:** 120 → **0** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.92) < R:R mínimo (0.00)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=100.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-19 11:56:59*