# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-10 11:49:53
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251110_114925.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251110_114925.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 90
- **Win Rate:** 0.0% (0/90)
- **Profit Factor:** 19.11
- **Avg R:R Planeado:** 1.70
- **R:R Mínimo para Break-Even:** 1.75

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 56 puntos
   - TP máximo observado: 108 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.70
   - R:R necesario: 1.75
   - **Gap:** 0.05

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

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline

| Paso | Zonas/Señales | % Supervivencia |
|------|---------------|-----------------|
| StructureFusion | 0 | 0.0% |
| ProximityAnalyzer | 864 | 0.0% |
| DFM | 0 | 0.0% |
| RiskCalculator | 0 | 0.0% |
| TradeManager | 90 | 0.0% |

**Análisis:**
- ⚠️ **No hay datos suficientes para análisis de waterfall**

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 70 | 54.3% |
| P0_ANY_DIR | 59 | 45.7% |

### 3.5 Análisis MFE/MAE (Excursión del Precio)

**Métricas Globales:**

- **MFE Promedio:** 68.52 pts (máxima ganancia flotante)
- **MAE Promedio:** 8.41 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 449.28

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 74 | 82.2% |
| SL_FIRST (precio fue hacia SL) | 12 | 13.3% |
| NEUTRAL (sin dirección clara) | 4 | 4.4% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 90.0%
- **Entradas Malas (MAE > MFE):** 10.0%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 7 | 3 | 3 | 42.9% | 64.86 | 15.21 |
| PENDING | 81 | 69 | 9 | 85.2% | 68.42 | 7.89 |
| CANCELLED | 2 | 2 | 0 | 100.0% | 85.12 | 5.38 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | BUY | 43.00 | 5.25 | 8.19 | NEUTRAL | PENDING | ✅ Entrada excelente |
| T0002 | BUY | 67.00 | 4.00 | 16.75 | NEUTRAL | CLOSED | ✅ Entrada excelente |
| T0003 | BUY | 99.50 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0004 | BUY | 99.50 | 0.00 | 999.00 | TP_FIRST | CANCELLED | ✅ Entrada excelente |
| T0005 | BUY | 99.50 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0006 | BUY | 99.50 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0007 | BUY | 99.50 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0008 | BUY | 99.50 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0009 | BUY | 69.75 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0010 | BUY | 70.75 | 10.75 | 6.58 | TP_FIRST | CANCELLED | ✅ Entrada excelente |
| T0011 | SELL | 37.50 | 13.25 | 2.83 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0012 | SELL | 37.50 | 13.25 | 2.83 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0013 | SELL | 33.25 | 13.25 | 2.51 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0014 | SELL | 54.75 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0015 | SELL | 54.75 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0016 | SELL | 50.25 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0017 | SELL | 53.75 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0018 | SELL | 61.50 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0019 | SELL | 31.25 | 20.50 | 1.52 | SL_FIRST | PENDING | ✅ Entrada excelente |
| T0020 | SELL | 31.25 | 17.50 | 1.79 | SL_FIRST | PENDING | ✅ Entrada excelente |

---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 26.32 pts
- **Mediana:** 25.30 pts
- **Min/Max:** 7.92 / 56.01 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 25.30 |
| P70 | 32.16 |
| P80 | 34.78 |
| P90 | 45.39 |
| P95 | 54.99 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 38.56 pts
- **Mediana:** 31.75 pts
- **Min/Max:** 15.25 / 107.75 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 31.75 |
| P70 | 40.35 |
| P80 | 57.55 |
| P90 | 75.50 |
| P95 | 80.50 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 45; // Era 60
public int MaxTPDistancePoints { get; set; } = 75; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 45.4pts, TP: 75.5pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (0.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.000) / 0.000
R:R_min = 1.75
```

**Estado actual:** R:R promedio = 1.70
**Gap:** 0.05 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **45** (P90 real)
2. **MaxTPDistancePoints:** 120 → **75** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.70) < R:R mínimo (1.75)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=0.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-10 11:49:53*