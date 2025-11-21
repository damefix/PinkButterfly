# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-17 17:48:27
**LOG:** `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_173931.log`
**CSV:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251117_173931.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 1
- **Win Rate:** 100.0% (1/1)
- **Profit Factor:** 0.00
- **Avg R:R Planeado:** 2.53
- **R:R Mínimo para Break-Even:** 0.00

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 20 puntos
   - TP máximo observado: 52 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.53
   - R:R necesario: 0.00
   - **Gap:** -2.53

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8122 | 34.7% |
| Bullish | 9085 | 38.8% |
| Bearish | 6218 | 26.5% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.082
- **Score Min/Max:** [-1.000, 1.000]
- **Componentes (promedio):**
  - EMA20 Slope: 0.044
  - EMA50 Cross: 0.196
  - BOS Count: 0.007
  - Regression 24h: 0.092

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 1.000 (apenas supera threshold)
- Score mínimo observado: -1.000 (apenas supera threshold)
- **Consecuencia:** Sistema queda 34.7% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.7%)

**Posibles causas:**
- **BOS Score bajo (0.007):** BOS/CHoCH no se detectan correctamente
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
| StructureFusion | 66613 | 100.0% | 100.0% |
| ProximityAnalyzer | 1218 | 1.8% | 1.8% |
| DFM_Evaluated | 176 | 14.4% | 0.3% |
| DFM_Passed | 176 | 100.0% | 0.3% |
| RiskCalculator | 2329 | 1323.3% | 3.5% |
| Risk_Accepted | 1 | 0.0% | 0.0% |
| TradeManager | 1 | 100.0% | 0.0% |

**Análisis:**
- **Mayor caída:** Risk_Accepted (pierde 2328 señales, -100.0%)
- **Tasa de conversión final:** 0.00% (de 66613 zonas iniciales → 1 operaciones)

### 3.1.1 Razones de Rechazo en RiskCalculator

| Razón | Cantidad | % del Total Rechazado |
|-------|----------|----------------------|
| SL_CHECK_FAIL | 922 | 61.8% |
| ENTRY_TOO_FAR | 394 | 26.4% |
| TP_CHECK_FAIL | 145 | 9.7% |
| NO_SL | 32 | 2.1% |

**Análisis:**
- **Razón dominante:** SL_CHECK_FAIL (922 rechazos, 61.8%)
- **Problema:** Stop Loss demasiado lejano (supera `MaxSLDistanceATR`)
- **Acción recomendada:** Revisar swings protectores o ajustar límite dinámico

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 998 | 99.8% |
| P0_ANY_DIR | 2 | 0.2% |

### 3.5 Análisis MFE/MAE (Excursión del Precio) - TRADES EJECUTADOS

**Métricas Globales:**

- **MFE Promedio:** 45.75 pts (máxima ganancia flotante)
- **MAE Promedio:** 32.75 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 1.40

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
| CLOSED | 1 | 1 | 0 | 100.0% | 45.75 | 32.75 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0006 | SELL | 45.75 | 32.75 | 1.40 | TP_FIRST | CLOSED | 👍 Entrada correcta |

---

### 3.6 Análisis de PHANTOM OPPORTUNITIES (Oportunidades NO Ejecutadas)

**📊 NUEVO: Análisis completo de oportunidades procesadas por RiskCalculator pero NO ejecutadas por TradeManager**

Este análisis permite evaluar si el filtro de distancia (MaxDistanceToRegister_ATR) está rechazando oportunidades de buena calidad.

**Total de Phantom Opportunities analizadas:** 243

**Calidad por Rango de Distancia:**

| Rango Distancia | Count | WR Teórico | TP_FIRST % | MFE/MAE Ratio | Good Entries % | Avg R:R | Conclusión |
|-----------------|-------|------------|------------|---------------|----------------|---------|------------|
| 0.0-2.0 ATR (Muy cerca) | 83 | 12.0% | 27.7% | 1.08 | 44.6% | 3.10 | ❌ BAJA CALIDAD - Filtro correcto |
| 2.0-3.0 ATR (Cerca) | 53 | 30.2% | 37.7% | 0.87 | 49.1% | 3.58 | ❌ BAJA CALIDAD - Filtro correcto |
| 3.0-5.0 ATR (Media) | 63 | 28.6% | 47.6% | 1.19 | 36.5% | 2.86 | ❌ BAJA CALIDAD - Filtro correcto |
| 5.0-10.0 ATR (Lejos) | 43 | 55.8% | 65.1% | 2.40 | 51.2% | 2.66 | ⚠️ CALIDAD MEDIA - Revisar |
| >10.0 ATR (Muy lejos) | 1 | 100.0% | 100.0% | 0.00 | 100.0% | 1.14 | ✅ BUENA CALIDAD - Considerar incluir |

**📈 Análisis Detallado por Rango:**

**0.0-2.0 ATR (Muy cerca)** (83 oportunidades)

- **WR Teórico:** 12.0% (si se hubieran ejecutado)
- **TP_FIRST:** 27.7% (23 de 83)
- **SL_FIRST:** 68.7% (57 de 83)
- **MFE Promedio:** 46.54 pts
- **MAE Promedio:** 65.00 pts
- **MFE/MAE Ratio:** 1.08
- **Good Entries:** 44.6% (MFE > MAE)
- **R:R Promedio:** 3.10

**❌ BAJA CALIDAD - Filtro correcto**

**2.0-3.0 ATR (Cerca)** (53 oportunidades)

- **WR Teórico:** 30.2% (si se hubieran ejecutado)
- **TP_FIRST:** 37.7% (20 de 53)
- **SL_FIRST:** 54.7% (29 de 53)
- **MFE Promedio:** 54.06 pts
- **MAE Promedio:** 76.05 pts
- **MFE/MAE Ratio:** 0.87
- **Good Entries:** 49.1% (MFE > MAE)
- **R:R Promedio:** 3.58

**❌ BAJA CALIDAD - Filtro correcto**

**3.0-5.0 ATR (Media)** (63 oportunidades)

- **WR Teórico:** 28.6% (si se hubieran ejecutado)
- **TP_FIRST:** 47.6% (30 de 63)
- **SL_FIRST:** 41.3% (26 de 63)
- **MFE Promedio:** 53.09 pts
- **MAE Promedio:** 80.22 pts
- **MFE/MAE Ratio:** 1.19
- **Good Entries:** 36.5% (MFE > MAE)
- **R:R Promedio:** 2.86

**❌ BAJA CALIDAD - Filtro correcto**

**5.0-10.0 ATR (Lejos)** (43 oportunidades)

- **WR Teórico:** 55.8% (si se hubieran ejecutado)
- **TP_FIRST:** 65.1% (28 de 43)
- **SL_FIRST:** 32.6% (14 de 43)
- **MFE Promedio:** 96.51 pts
- **MAE Promedio:** 64.18 pts
- **MFE/MAE Ratio:** 2.40
- **Good Entries:** 51.2% (MFE > MAE)
- **R:R Promedio:** 2.66

**⚠️ CALIDAD MEDIA - Revisar**

**>10.0 ATR (Muy lejos)** (1 oportunidades)

- **WR Teórico:** 100.0% (si se hubieran ejecutado)
- **TP_FIRST:** 100.0% (1 de 1)
- **SL_FIRST:** 0.0% (0 de 1)
- **MFE Promedio:** 159.25 pts
- **MAE Promedio:** 0.00 pts
- **MFE/MAE Ratio:** 0.00
- **Good Entries:** 100.0% (MFE > MAE)
- **R:R Promedio:** 1.14

**✅ BUENA CALIDAD - Considerar incluir**

**🔍 Comparativa: Phantom Opportunities vs. Trades Ejecutados**

| Métrica | Trades Ejecutados | Phantoms 0-2 ATR | Phantoms 2-3 ATR | Phantoms 3-5 ATR |
|---------|-------------------|------------------|------------------|------------------|
| **Count** | 1 | 83 | 53 | 63 |
| **TP_FIRST %** | 100.0% | 27.7% | 37.7% | 47.6% |
| **Good Entries %** | 100.0% | 44.6% | 49.1% | 36.5% |
| **MFE/MAE Ratio** | 1.40 | 1.08 | 0.87 | 1.19 |

**💡 RECOMENDACIONES BASADAS EN DATOS:**

✅ **CORRECTO: Las 63 oportunidades en 3.0-5.0 ATR tienen baja calidad**


---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 20.48 pts
- **Mediana:** 20.48 pts
- **Min/Max:** 20.48 / 20.48 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 0.00 |
| P70 | 0.00 |
| P80 | 0.00 |
| P90 | 0.00 |
| P95 | 0.00 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 51.75 pts
- **Mediana:** 51.75 pts
- **Min/Max:** 51.75 / 51.75 pts

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

**Estado actual:** R:R promedio = 2.53
**Gap:** -2.53 (necesitas mejorar R:R)

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

**Problema:** R:R actual (2.53) < R:R mínimo (0.00)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=100.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-17 17:48:27*