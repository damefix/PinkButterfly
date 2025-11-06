# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-05 11:43:42
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251105_112151.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251105_112151.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 82
- **Win Rate:** 0.0% (0/82)
- **Profit Factor:** 1.11
- **Avg R:R Planeado:** 44.65
- **R:R Mínimo para Break-Even:** 1.75

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 99 puntos
   - TP máximo observado: 93 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 44.65
   - R:R necesario: 1.75
   - **Gap:** -42.90

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 4972 | 99.4% |
| Bullish | 20 | 0.4% |
| Bearish | 8 | 0.2% |

### 2.2 Diagnóstico

**Problema detectado:** CRÍTICO: Bias Compuesto 99.4% Neutral - threshold 0.5/-0.5 DEMASIADO ALTO. Score real [-0.55, 0.54]. REDUCIR threshold a 0.3/-0.3.

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.036
- **Score Min/Max:** [-0.550, 0.540]
- **Componentes (promedio):**
  - EMA20 Slope: 0.020
  - EMA50 Cross: 0.250
  - BOS Count: 0.000
  - Regression 24h: -0.162

**Análisis:**
- Threshold actual: 0.5/-0.5
- Score máximo observado: 0.540 (apenas supera threshold)
- Score mínimo observado: -0.550 (apenas supera threshold)
- **Consecuencia:** Sistema queda 99.4% Neutral → bias no diferencia tendencias

### 2.3 Recomendación: Ajustar Threshold del Bias Compuesto

**Solución Inmediata:** Reducir threshold de 0.5/-0.5 a **0.3/-0.3**

**Archivo:** `pinkbutterfly-produccion/ContextManager.cs` (línea ~207)

```csharp
// ANTES:
if (compositeScore > 0.5) { ... }

// DESPUÉS:
if (compositeScore > 0.3) { ... }  // Más sensible
elif (compositeScore < -0.3) { ... }
```

**Justificación:**
- Scores reales: [-0.55, 0.54]
- Score promedio: 0.036
- Threshold 0.5 requiere 100% alineación de componentes (poco realista)
- Threshold 0.3 requiere 60% alineación (más realista)

**Impacto esperado:**
- Neutral actual: 99.4% → ~60-70% (objetivo)
- Bullish/Bearish: ~0.5% → ~15-20% cada uno
- Sistema empezará a usar el bias para filtrar operaciones

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline

| Paso | Zonas/Señales | % Supervivencia |
|------|---------------|-----------------|
| StructureFusion | 0 | 0.0% |
| ProximityAnalyzer | 17676 | 0.0% |
| DFM | 0 | 0.0% |
| RiskCalculator | 0 | 0.0% |
| TradeManager | 82 | 0.0% |

**Análisis:**
- ⚠️ **No hay datos suficientes para análisis de waterfall**

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 874 | 90.0% |
| P0_ANY_DIR | 97 | 10.0% |

---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 51.79 pts
- **Mediana:** 47.17 pts
- **Min/Max:** 3.67 / 98.64 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 47.17 |
| P70 | 73.66 |
| P80 | 81.07 |
| P90 | 86.37 |
| P95 | 91.37 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 55.29 pts
- **Mediana:** 61.70 pts
- **Min/Max:** 0.12 / 93.45 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 61.70 |
| P70 | 75.30 |
| P80 | 75.48 |
| P90 | 77.63 |
| P95 | 83.98 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 86; // Era 60
public int MaxTPDistancePoints { get; set; } = 77; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 86.4pts, TP: 77.6pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (0.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.000) / 0.000
R:R_min = 1.75
```

**Estado actual:** R:R promedio = 44.65
**Gap:** -42.90 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **86** (P90 real)
2. **MaxTPDistancePoints:** 120 → **77** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (44.65) < R:R mínimo (1.75)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=0.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-05 11:43:42*