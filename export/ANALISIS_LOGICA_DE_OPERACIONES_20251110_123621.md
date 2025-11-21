# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-10 12:37:22
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251110_123621.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251110_123621.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 160
- **Win Rate:** 0.0% (0/160)
- **Profit Factor:** 1.00
- **Avg R:R Planeado:** 1.83
- **R:R Mínimo para Break-Even:** 1.75

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 77 puntos
   - TP máximo observado: 118 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.83
   - R:R necesario: 1.75
   - **Gap:** -0.08

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
| ProximityAnalyzer | 1644 | 0.0% |
| DFM | 0 | 0.0% |
| RiskCalculator | 0 | 0.0% |
| TradeManager | 160 | 0.0% |

**Análisis:**
- ⚠️ **No hay datos suficientes para análisis de waterfall**

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 124 | 51.0% |
| P0_ANY_DIR | 119 | 49.0% |

### 3.5 Análisis MFE/MAE (Excursión del Precio)

**Métricas Globales:**

- **MFE Promedio:** 48.86 pts (máxima ganancia flotante)
- **MAE Promedio:** 18.20 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 327.81

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 105 | 65.6% |
| SL_FIRST (precio fue hacia SL) | 51 | 31.9% |
| NEUTRAL (sin dirección clara) | 4 | 2.5% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 68.1%
- **Entradas Malas (MAE > MFE):** 31.9%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 19 | 13 | 6 | 68.4% | 53.49 | 18.75 |
| PENDING | 138 | 91 | 43 | 65.9% | 47.97 | 18.22 |
| CANCELLED | 3 | 1 | 2 | 33.3% | 60.50 | 13.33 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | SELL | 67.25 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0002 | SELL | 67.25 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0003 | SELL | 67.25 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0004 | SELL | 67.25 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0005 | SELL | 67.25 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0006 | SELL | 67.25 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0007 | SELL | 67.25 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0008 | SELL | 67.25 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0009 | SELL | 67.25 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0010 | SELL | 67.25 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0011 | SELL | 67.25 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0012 | SELL | 66.75 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0013 | SELL | 12.50 | 17.00 | 0.74 | TP_FIRST | PENDING | ⚠️ Entrada dudosa |
| T0014 | SELL | 12.50 | 17.00 | 0.74 | NEUTRAL | PENDING | ⚠️ Entrada dudosa |
| T0015 | SELL | 12.50 | 17.00 | 0.74 | SL_FIRST | PENDING | ⚠️ Entrada dudosa |
| T0016 | SELL | 12.50 | 17.00 | 0.74 | SL_FIRST | PENDING | ⚠️ Entrada dudosa |
| T0017 | SELL | 12.50 | 18.75 | 0.67 | SL_FIRST | PENDING | ⚠️ Entrada dudosa |
| T0018 | SELL | 12.50 | 23.75 | 0.53 | SL_FIRST | PENDING | ❌ Entrada muy mala |
| T0019 | SELL | 12.50 | 23.75 | 0.53 | SL_FIRST | PENDING | ❌ Entrada muy mala |
| T0020 | SELL | 12.50 | 23.75 | 0.53 | SL_FIRST | PENDING | ❌ Entrada muy mala |

---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 26.37 pts
- **Mediana:** 27.64 pts
- **Min/Max:** 2.38 / 77.02 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 27.64 |
| P70 | 34.51 |
| P80 | 36.13 |
| P90 | 48.19 |
| P95 | 54.82 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 38.05 pts
- **Mediana:** 29.50 pts
- **Min/Max:** 11.50 / 118.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 29.50 |
| P70 | 43.75 |
| P80 | 55.75 |
| P90 | 78.25 |
| P95 | 80.50 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 48; // Era 60
public int MaxTPDistancePoints { get; set; } = 78; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 48.2pts, TP: 78.2pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (0.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.000) / 0.000
R:R_min = 1.75
```

**Estado actual:** R:R promedio = 1.83
**Gap:** -0.08 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **48** (P90 real)
2. **MaxTPDistancePoints:** 120 → **78** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.83) < R:R mínimo (1.75)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=0.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-10 12:37:22*