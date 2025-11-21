# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-12 13:43:47
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_134121.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251112_134121.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 35
- **Win Rate:** 31.4% (11/35)
- **Profit Factor:** 0.65
- **Avg R:R Planeado:** 2.46
- **R:R Mínimo para Break-Even:** 2.18

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 40 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.46
   - R:R necesario: 2.18
   - **Gap:** -0.27

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8188 | 34.8% |
| Bullish | 9020 | 38.4% |
| Bearish | 6292 | 26.8% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.080
- **Score Min/Max:** [-0.990, 0.970]
- **Componentes (promedio):**
  - EMA20 Slope: 0.042
  - EMA50 Cross: 0.188
  - BOS Count: 0.009
  - Regression 24h: 0.089

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.970 (apenas supera threshold)
- Score mínimo observado: -0.990 (apenas supera threshold)
- **Consecuencia:** Sistema queda 34.8% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.8%)

**Posibles causas:**
- **BOS Score bajo (0.009):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio 0.080 indica poca señal direccional
- **Mercado lateral:** Scores reales [-0.99, 0.97] muy cercanos a 0

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
| StructureFusion | 10196 | 100.0% | 100.0% |
| ProximityAnalyzer | 7 | 0.1% | 0.1% |
| DFM_Evaluated | 273 | 3900.0% | 2.7% |
| DFM_Passed | 258 | 94.5% | 2.5% |
| RiskCalculator | 66 | 25.6% | 0.6% |
| TradeManager | 35 | 53.0% | 0.3% |

**Análisis:**
- **Mayor caída:** ProximityAnalyzer (pierde 10189 señales, -99.9%)
- **Tasa de conversión final:** 0.34% (de 10196 zonas iniciales → 35 operaciones)

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 464 | 92.4% |
| P0_ANY_DIR | 38 | 7.6% |

### 3.5 Análisis MFE/MAE (Excursión del Precio)

**Métricas Globales:**

- **MFE Promedio:** 32.30 pts (máxima ganancia flotante)
- **MAE Promedio:** 34.74 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 6.12

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 14 | 40.0% |
| SL_FIRST (precio fue hacia SL) | 19 | 54.3% |
| NEUTRAL (sin dirección clara) | 2 | 5.7% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 37.1%
- **Entradas Malas (MAE > MFE):** 62.9%

⚠️ **ALERTA:** >60% de entradas tienen MAE > MFE
- **Problema:** El precio va más en contra que a favor antes del cierre
- **Causas posibles:**
  1. Timing incorrecto (entramos antes de reversión)
  2. Bias desincronizado (operamos contra tendencia real)
  3. Zonas de baja calidad (sin confluence real)

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 35 | 14 | 19 | 40.0% | 32.30 | 34.74 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | BUY | 24.75 | 6.50 | 3.81 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0003 | BUY | 4.00 | 21.75 | 0.18 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0004 | BUY | 4.00 | 37.00 | 0.11 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0005 | BUY | 0.00 | 39.00 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0008 | SELL | 144.75 | 20.50 | 7.06 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0009 | SELL | 248.50 | 11.25 | 22.09 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0012 | SELL | 0.00 | 68.50 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0013 | SELL | 33.75 | 18.00 | 1.88 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0014 | BUY | 8.25 | 22.50 | 0.37 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0015 | BUY | 29.25 | 7.00 | 4.18 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0016 | BUY | 9.50 | 85.00 | 0.11 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0018 | SELL | 19.75 | 87.50 | 0.23 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0025 | BUY | 12.75 | 15.75 | 0.81 | SL_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0028 | BUY | 12.00 | 12.00 | 1.00 | SL_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0029 | BUY | 4.00 | 80.75 | 0.05 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0032 | SELL | 26.75 | 163.25 | 0.16 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0034 | BUY | 26.00 | 7.50 | 3.47 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0036 | BUY | 16.50 | 2.25 | 7.33 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0037 | BUY | 16.00 | 48.25 | 0.33 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0038 | BUY | 30.25 | 10.00 | 3.02 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 14.02 pts
- **Mediana:** 13.40 pts
- **Min/Max:** 1.67 / 40.44 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 13.40 |
| P70 | 17.66 |
| P80 | 20.77 |
| P90 | 23.89 |
| P95 | 38.10 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 27.64 pts
- **Mediana:** 21.75 pts
- **Min/Max:** 6.75 / 54.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 21.75 |
| P70 | 42.50 |
| P80 | 48.70 |
| P90 | 51.30 |
| P95 | 54.00 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 23; // Era 60
public int MaxTPDistancePoints { get; set; } = 51; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 23.9pts, TP: 51.3pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (31.4%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.314) / 0.314
R:R_min = 2.18
```

**Estado actual:** R:R promedio = 2.46
**Gap:** -0.27 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **23** (P90 real)
2. **MaxTPDistancePoints:** 120 → **51** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.46) < R:R mínimo (2.18)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=31.4%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-12 13:43:47*