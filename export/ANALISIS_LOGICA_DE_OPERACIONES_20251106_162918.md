# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-06 16:41:48
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251106_162918.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251106_162918.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 37
- **Win Rate:** 0.0% (0/37)
- **Profit Factor:** 2.30
- **Avg R:R Planeado:** 51.83
- **R:R Mínimo para Break-Even:** 1.75

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 99 puntos
   - TP máximo observado: 99 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 51.83
   - R:R necesario: 1.75
   - **Gap:** -50.08

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|

### 2.2 Diagnóstico

**Problema detectado:** 

**Contexto:**
- EMA200@60m refleja últimas **200 horas** (~8+ días)
- NO captura movimientos intradía (últimas 4-24 horas)
- Gráfico muestra caída reciente, pero bias sigue 'Bullish'

### 2.3 Recomendación: Bias Compuesto Rápido

**Propuesta:** Cambiar de EMA200@60m a señal compuesta:

```
BiasScore = (
    0.30 * EMA20@60m_slope  // Tendencia inmediata (20h)
  + 0.25 * EMA50@60m_cross  // Tendencia media (50h)
  + 0.25 * BOS_CHoCH_count  // Cambios de estructura recientes
  + 0.20 * Regression_24h   // Dirección últimas 24h
)

if BiasScore > 0.5: Bullish
elif BiasScore < -0.5: Bearish
else: Neutral
```

**Ventajas:**
- ✅ Responde en 4-24 horas (intradía)
- ✅ Combina múltiples señales (robusto)
- ✅ Detecta cambios de estructura (BOS/CHoCH)

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline

| Paso | Zonas/Señales | % Supervivencia |
|------|---------------|-----------------|
| StructureFusion | 0 | 0.0% |
| ProximityAnalyzer | 6444 | 0.0% |
| DFM | 0 | 0.0% |
| RiskCalculator | 0 | 0.0% |
| TradeManager | 37 | 0.0% |

**Análisis:**
- ⚠️ **No hay datos suficientes para análisis de waterfall**

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 191 | 73.7% |
| P0_ANY_DIR | 68 | 26.3% |

---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 44.24 pts
- **Mediana:** 41.68 pts
- **Min/Max:** 1.69 / 98.66 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 41.68 |
| P70 | 55.27 |
| P80 | 71.28 |
| P90 | 86.48 |
| P95 | 95.08 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 52.76 pts
- **Mediana:** 54.72 pts
- **Min/Max:** 0.47 / 98.68 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 54.72 |
| P70 | 75.26 |
| P80 | 75.37 |
| P90 | 79.70 |
| P95 | 98.45 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 86; // Era 60
public int MaxTPDistancePoints { get; set; } = 79; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 86.5pts, TP: 79.7pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (0.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.000) / 0.000
R:R_min = 1.75
```

**Estado actual:** R:R promedio = 51.83
**Gap:** -50.08 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **79** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (51.83) < R:R mínimo (1.75)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=0.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-06 16:41:48*