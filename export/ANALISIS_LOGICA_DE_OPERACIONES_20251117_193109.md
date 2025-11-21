# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-17 19:34:19
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_193109.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251117_193109.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 2
- **Win Rate:** 100.0% (2/2)
- **Profit Factor:** 0.00
- **Avg R:R Planeado:** 1.54
- **R:R Mínimo para Break-Even:** 0.00

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 36 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.54
   - R:R necesario: 0.00
   - **Gap:** -1.54

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Bullish | 985 | 39.4% |
| Neutral | 680 | 27.2% |
| Bearish | 838 | 33.5% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.040
- **Score Min/Max:** [-1.000, 1.000]
- **Componentes (promedio):**
  - EMA20 Slope: 0.018
  - EMA50 Cross: -0.007
  - BOS Count: 0.069
  - Regression 24h: 0.096

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 1.000 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 27.2% Neutral → bias no diferencia tendencias

### 2.3 Estado: Bias Compuesto Funcionando Correctamente

✅ **Threshold actual:** 0.3 (correcto)
✅ **Bias Neutral:** 27.2% (aceptable)
✅ **Score promedio:** 0.040

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 4368 | 100.0% | 100.0% |
| ProximityAnalyzer | 1052 | 24.1% | 24.1% |
| DFM_Evaluated | 176 | 16.7% | 4.0% |
| DFM_Passed | 176 | 100.0% | 4.0% |
| RiskCalculator | 1535 | 872.2% | 35.1% |
| Risk_Accepted | 16 | 1.0% | 0.4% |
| TradeManager | 2 | 12.5% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 1519 señales, -99.0%)
- **Tasa de conversión final:** 0.05% (de 4368 zonas iniciales → 2 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 314 | 53.1% |
| ENTRY_TOO_FAR | 216 | 36.5% |
| TP_CHECK_FAIL | 42 | 7.1% |
| NO_SL | 19 | 3.2% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (314 rechazos, 53.1%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 837 | 100.0% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 53.88 pts (máxima ganancia flotante)
- **MAE Promedio:** 26.50 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 3.16

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 1 | 50.0% |
| SL_FIRST (precio fue hacia SL) | 1 | 50.0% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 50.0%
- **Entradas Malas (MAE > MFE):** 50.0%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 2 | 1 | 1 | 50.0% | 53.88 | 26.50 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0007 | SELL | 84.25 | 14.75 | 5.71 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0011 | SELL | 23.50 | 38.25 | 0.61 | SL_FIRST | CLOSED | ❌ Entrada muy mala |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 212

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 83 | 53.0% | 54.2% | 2.64 | 60.2% | 2.56 | ✅ BUENA CALIDAD - Considerar incluir |
| 2.0-3.0 ATR (Cerca) | 34 | 85.3% | 79.4% | 2.82 | 76.5% | 3.03 | ✅ BUENA CALIDAD - Considerar incluir |
| 3.0-5.0 ATR (Media) | 66 | 71.2% | 80.3% | 4.78 | 75.8% | 2.53 | ✅ BUENA CALIDAD - Considerar incluir |
| 5.0-10.0 ATR (Lejos) | 23 | 69.6% | 60.9% | 3.44 | 78.3% | 2.41 | ✅ BUENA CALIDAD - Considerar incluir |
| >10.0 ATR (Muy lejos) | 6 | 0.0% | 0.0% | 0.27 | 0.0% | 1.59 | ❌ BAJA CALIDAD - Filtro correcto |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (83 oportunidades)

- **WR Teórico:** 53.0% (si se hubieran ejecutado)
- **TP_FIRST:** 54.2% (45 de 83)
- **SL_FIRST:** 42.2% (35 de 83)
- **MFE Promedio:** 63.77 pts
- **MAE Promedio:** 38.76 pts
- **MFE/MAE Ratio:** 2.64
- **Good Entries:** 60.2% (MFE > MAE)
- **R:R Promedio:** 2.56

**✅ BUENA CALIDAD - Considerar incluir**

**2.0-3.0 ATR (Cerca)** (34 oportunidades)

- **WR Teórico:** 85.3% (si se hubieran ejecutado)
- **TP_FIRST:** 79.4% (27 de 34)
- **SL_FIRST:** 20.6% (7 de 34)
- **MFE Promedio:** 82.58 pts
- **MAE Promedio:** 51.52 pts
- **MFE/MAE Ratio:** 2.82
- **Good Entries:** 76.5% (MFE > MAE)
- **R:R Promedio:** 3.03

**✅ BUENA CALIDAD - Considerar incluir**

**3.0-5.0 ATR (Media)** (66 oportunidades)

- **WR Teórico:** 71.2% (si se hubieran ejecutado)
- **TP_FIRST:** 80.3% (53 de 66)
- **SL_FIRST:** 19.7% (13 de 66)
- **MFE Promedio:** 106.76 pts
- **MAE Promedio:** 50.49 pts
- **MFE/MAE Ratio:** 4.78
- **Good Entries:** 75.8% (MFE > MAE)
- **R:R Promedio:** 2.53

**✅ BUENA CALIDAD - Considerar incluir**

**5.0-10.0 ATR (Lejos)** (23 oportunidades)

- **WR Teórico:** 69.6% (si se hubieran ejecutado)
- **TP_FIRST:** 60.9% (14 de 23)
- **SL_FIRST:** 39.1% (9 de 23)
- **MFE Promedio:** 86.16 pts
- **MAE Promedio:** 36.82 pts
- **MFE/MAE Ratio:** 3.44
- **Good Entries:** 78.3% (MFE > MAE)
- **R:R Promedio:** 2.41

**✅ BUENA CALIDAD - Considerar incluir**

**>10.0 ATR (Muy lejos)** (6 oportunidades)

- **WR Teórico:** 0.0% (si se hubieran ejecutado)
- **TP_FIRST:** 0.0% (0 de 6)
- **SL_FIRST:** 100.0% (6 de 6)
- **MFE Promedio:** 25.21 pts
- **MAE Promedio:** 95.79 pts
- **MFE/MAE Ratio:** 0.27
- **Good Entries:** 0.0% (MFE > MAE)
- **R:R Promedio:** 1.59

**❌ BAJA CALIDAD - Filtro correcto**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 2 | 83 | 34 | 66 |
| **TP_FIRST %** | 50.0% | 54.2% | 79.4% | 80.3% |
| **Good Entries %** | 50.0% | 60.2% | 76.5% | 75.8% |
| **MFE/MAE Ratio** | 3.16 | 2.64 | 2.82 | 4.78 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

🚨 **CRÍTICO: El rango 2.0-3.0 ATR contiene 34 oportunidades de BUENA CALIDAD**
   - WR Teórico: 85.3%
   - Good Entries: 76.5%
   - **ACCIÓN:** Considerar aumentar MaxDistanceToRegister_ATR_Normal de 2.0 a 3.0

⚠️ **ATENCIÓN: El rango 3.0-5.0 ATR contiene 66 oportunidades con calidad MEDIA-ALTA**
   - WR Teórico: 71.2%
   - Good Entries: 75.8%
   - **ACCIÓN:** Evaluar aumentar MaxDistanceToRegister_ATR_Normal a 4.0-5.0 si se necesitan más operaciones


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 35.33 pts
- **Mediana:** 35.33 pts
- **Min/Max:** 35.13 / 35.52 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 35.33 |
| P70 | 35.56 |
| P80 | 35.68 |
| P90 | 35.79 |
| P95 | 35.85 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 54.50 pts
- **Mediana:** 54.50 pts
- **Min/Max:** 54.50 / 54.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 54.50 |
| P70 | 54.50 |
| P80 | 54.50 |
| P90 | 54.50 |
| P95 | 54.50 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 35; // Era 60
public int MaxTPDistancePoints { get; set; } = 54; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 35.8pts, TP: 54.5pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (100.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 1.000) / 1.000
R:R_min = 0.00
```

**Estado actual:** R:R promedio = 1.54
**Gap:** -1.54 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **35** (P90 real)
2. **MaxTPDistancePoints:** 120 → **54** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.54) < R:R mínimo (0.00)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=100.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-17 19:34:19*