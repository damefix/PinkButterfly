# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-11 21:00:32
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_205724.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251111_205724.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 36
- **Win Rate:** 22.2% (8/36)
- **Profit Factor:** 0.35
- **Avg R:R Planeado:** 1.76
- **R:R Mínimo para Break-Even:** 3.50

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 28 puntos
   - TP máximo observado: 54 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.76
   - R:R necesario: 3.50
   - **Gap:** 1.74

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8227 | 35.0% |
| Bearish | 6336 | 26.9% |
| Bullish | 8967 | 38.1% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.076
- **Score Min/Max:** [-0.990, 0.930]
- **Componentes (promedio):**
  - EMA20 Slope: 0.040
  - EMA50 Cross: 0.182
  - BOS Count: 0.008
  - Regression 24h: 0.085

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.930 (apenas supera threshold)
- Score mínimo observado: -0.990 (apenas supera threshold)
- **Consecuencia:** Sistema queda 35.0% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (35.0%)

**Posibles causas:**
- **BOS Score bajo (0.008):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio 0.076 indica poca señal direccional
- **Mercado lateral:** Scores reales [-0.99, 0.93] muy cercanos a 0

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
| StructureFusion | 39796 | 100.0% | 100.0% |
| ProximityAnalyzer | 7 | 0.0% | 0.0% |
| DFM_Evaluated | 200 | 2857.1% | 0.5% |
| DFM_Passed | 184 | 92.0% | 0.5% |
| RiskCalculator | 51 | 27.7% | 0.1% |
| TradeManager | 36 | 70.6% | 0.1% |

**Análisis:**
- **Mayor caída:** ProximityAnalyzer (pierde 39789 señales, -100.0%)
- **Tasa de conversión final:** 0.09% (de 39796 zonas iniciales → 36 operaciones)

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 408 | 92.9% |
| P0_ANY_DIR | 31 | 7.1% |

### 3.5 Análisis MFE/MAE (Excursión del Precio)

**Métricas Globales:**

- **MFE Promedio:** 30.96 pts (máxima ganancia flotante)
- **MAE Promedio:** 45.31 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 1.46

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 14 | 38.9% |
| SL_FIRST (precio fue hacia SL) | 21 | 58.3% |
| NEUTRAL (sin dirección clara) | 1 | 2.8% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 27.8%
- **Entradas Malas (MAE > MFE):** 72.2%

⚠️ **ALERTA:** >60% de entradas tienen MAE > MFE
- **Problema:** El precio va más en contra que a favor antes del cierre
- **Causas posibles:**
  1. Timing incorrecto (entramos antes de reversión)
  2. Bias desincronizado (operamos contra tendencia real)
  3. Zonas de baja calidad (sin confluence real)

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 36 | 14 | 21 | 38.9% | 30.96 | 45.31 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0004 | SELL | 35.25 | 20.25 | 1.74 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0005 | SELL | 2.75 | 29.00 | 0.09 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0006 | SELL | 2.50 | 38.25 | 0.07 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0007 | BUY | 4.00 | 21.75 | 0.18 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0008 | BUY | 4.25 | 36.75 | 0.12 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0012 | SELL | 137.50 | 27.75 | 4.95 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0012_2 | SELL | 137.50 | 27.75 | 4.95 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0012_3 | SELL | 137.50 | 27.75 | 4.95 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0013 | SELL | 19.50 | 49.00 | 0.40 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0014 | SELL | 29.00 | 42.25 | 0.69 | SL_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0015 | BUY | 36.50 | 82.00 | 0.45 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0017 | SELL | 0.00 | 90.75 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0020 | BUY | 13.00 | 15.50 | 0.84 | SL_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0021 | BUY | 14.75 | 16.25 | 0.91 | SL_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0022 | BUY | 12.00 | 60.25 | 0.20 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0025 | SELL | 16.00 | 34.75 | 0.46 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0026 | SELL | 5.50 | 162.75 | 0.03 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0026_2 | SELL | 5.50 | 162.75 | 0.03 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0028 | BUY | 27.25 | 10.50 | 2.60 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0029 | BUY | 16.50 | 2.25 | 7.33 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 12.72 pts
- **Mediana:** 10.54 pts
- **Min/Max:** 3.25 / 27.51 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 10.54 |
| P70 | 18.61 |
| P80 | 20.71 |
| P90 | 25.60 |
| P95 | 27.51 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 22.10 pts
- **Mediana:** 17.62 pts
- **Min/Max:** 5.75 / 54.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 17.62 |
| P70 | 27.25 |
| P80 | 35.30 |
| P90 | 50.00 |
| P95 | 54.00 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 25; // Era 60
public int MaxTPDistancePoints { get; set; } = 50; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 25.6pts, TP: 50.0pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (22.2%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.222) / 0.222
R:R_min = 3.50
```

**Estado actual:** R:R promedio = 1.76
**Gap:** 1.74 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **25** (P90 real)
2. **MaxTPDistancePoints:** 120 → **50** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.76) < R:R mínimo (3.50)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=22.2%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-11 21:00:32*