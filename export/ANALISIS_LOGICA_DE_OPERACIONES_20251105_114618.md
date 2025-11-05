# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-05 11:52:58
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251105_114618.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251105_114618.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 68
- **Win Rate:** 0.0% (0/68)
- **Profit Factor:** 1.38
- **Avg R:R Planeado:** 46.26
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
   - R:R actual: 46.26
   - R:R necesario: 1.75
   - **Gap:** -44.51

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 4048 | 81.0% |
| Bearish | 368 | 7.4% |
| Bullish | 584 | 11.7% |

### 2.2 Diagnóstico

**Problema detectado:** 

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
- **Consecuencia:** Sistema queda 81.0% Neutral → bias no diferencia tendencias

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
| ProximityAnalyzer | 18244 | 0.0% |
| DFM | 0 | 0.0% |
| RiskCalculator | 0 | 0.0% |
| TradeManager | 68 | 0.0% |

**Análisis:**
- ⚠️ **No hay datos suficientes para análisis de waterfall**

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 902 | 88.8% |
| P0_ANY_DIR | 114 | 11.2% |

---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 56.80 pts
- **Mediana:** 68.66 pts
- **Min/Max:** 3.67 / 98.64 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 68.66 |
| P70 | 78.67 |
| P80 | 83.87 |
| P90 | 86.77 |
| P95 | 92.77 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 51.00 pts
- **Mediana:** 50.36 pts
- **Min/Max:** 0.12 / 93.39 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 50.36 |
| P70 | 75.28 |
| P80 | 75.41 |
| P90 | 75.60 |
| P95 | 79.88 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 86; // Era 60
public int MaxTPDistancePoints { get; set; } = 75; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 86.8pts, TP: 75.6pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (0.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.000) / 0.000
R:R_min = 1.75
```

**Estado actual:** R:R promedio = 46.26
**Gap:** -44.51 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **75** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (46.26) < R:R mínimo (1.75)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=0.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-05 11:52:58*