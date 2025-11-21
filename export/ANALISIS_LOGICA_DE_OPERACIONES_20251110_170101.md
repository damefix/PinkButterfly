# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-10 17:14:07
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251110_170101.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251110_170101.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 14
- **Win Rate:** 0.0% (0/14)
- **Profit Factor:** 7.55
- **Avg R:R Planeado:** 1.54
- **R:R Mínimo para Break-Even:** 1.75

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 34 puntos
   - TP máximo observado: 50 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.54
   - R:R necesario: 1.75
   - **Gap:** 0.21

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
| ProximityAnalyzer | 62472 | 0.0% |
| DFM | 0 | 0.0% |
| RiskCalculator | 0 | 0.0% |
| TradeManager | 14 | 0.0% |

**Análisis:**
- ⚠️ **No hay datos suficientes para análisis de waterfall**

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 4174 | 59.3% |
| P0_ANY_DIR | 2859 | 40.7% |

### 3.5 Análisis MFE/MAE (Excursión del Precio)

**Métricas Globales:**

- **MFE Promedio:** 40.39 pts (máxima ganancia flotante)
- **MAE Promedio:** 23.23 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 428.89

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 9 | 64.3% |
| SL_FIRST (precio fue hacia SL) | 4 | 28.6% |
| NEUTRAL (sin dirección clara) | 1 | 7.1% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 64.3%
- **Entradas Malas (MAE > MFE):** 35.7%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 9 | 7 | 2 | 77.8% | 52.83 | 21.78 |
| PENDING | 2 | 1 | 0 | 50.0% | 37.50 | 5.50 |
| CANCELLED | 3 | 1 | 2 | 33.3% | 5.00 | 39.42 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | BUY | 17.00 | 11.00 | 1.55 | NEUTRAL | PENDING | ✅ Entrada excelente |
| T0002 | BUY | 24.50 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0003 | SELL | 14.00 | 22.50 | 0.62 | TP_FIRST | CANCELLED | ❌ Entrada muy mala |
| T0004 | SELL | 1.00 | 43.50 | 0.02 | SL_FIRST | CANCELLED | ❌ Entrada muy mala |
| T0005 | BUY | 54.75 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0006 | SELL | 0.00 | 52.25 | 0.00 | SL_FIRST | CANCELLED | ❌ Entrada muy mala |
| T0007 | BUY | 60.75 | 65.25 | 0.93 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0008 | SELL | 119.25 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0009 | SELL | 30.00 | 6.00 | 5.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0010 | SELL | 50.00 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0011 | SELL | 57.25 | 0.00 | 999.00 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0012 | BUY | 0.00 | 91.50 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0013 | BUY | 79.00 | 33.25 | 2.38 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0014 | BUY | 58.00 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |

---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 17.55 pts
- **Mediana:** 14.48 pts
- **Min/Max:** 4.29 / 33.97 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 14.48 |
| P70 | 26.89 |
| P80 | 30.63 |
| P90 | 32.62 |
| P95 | 34.64 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 22.11 pts
- **Mediana:** 21.25 pts
- **Min/Max:** 7.75 / 50.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 21.25 |
| P70 | 25.25 |
| P80 | 31.25 |
| P90 | 41.00 |
| P95 | 54.50 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 32; // Era 60
public int MaxTPDistancePoints { get; set; } = 41; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 32.6pts, TP: 41.0pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (0.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.000) / 0.000
R:R_min = 1.75
```

**Estado actual:** R:R promedio = 1.54
**Gap:** 0.21 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **32** (P90 real)
2. **MaxTPDistancePoints:** 120 → **41** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.54) < R:R mínimo (1.75)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=0.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-10 17:14:07*