# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-10 09:01:22
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251110_084023.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251110_084023.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 38
- **Win Rate:** 0.0% (0/38)
- **Profit Factor:** 0.00
- **Avg R:R Planeado:** 1.57
- **R:R Mínimo para Break-Even:** 1.75

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 49 puntos
   - TP máximo observado: 111 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.57
   - R:R necesario: 1.75
   - **Gap:** 0.18

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
| ProximityAnalyzer | 498 | 0.0% |
| DFM | 0 | 0.0% |
| RiskCalculator | 0 | 0.0% |
| TradeManager | 38 | 0.0% |

**Análisis:**
- ⚠️ **No hay datos suficientes para análisis de waterfall**

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_ANY_DIR | 51 | 73.9% |
| P0_SWING_LITE | 18 | 26.1% |

### 3.5 Análisis MFE/MAE (Excursión del Precio)

**Métricas Globales:**

- **MFE Promedio:** 78.89 pts (máxima ganancia flotante)
- **MAE Promedio:** 3.66 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 815.33

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 36 | 94.7% |
| SL_FIRST (precio fue hacia SL) | 2 | 5.3% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 92.1%
- **Entradas Malas (MAE > MFE):** 7.9%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 1 | 1 | 0 | 100.0% | 59.00 | 7.00 |
| PENDING | 15 | 14 | 1 | 93.3% | 54.20 | 7.97 |
| EXPIRED | 22 | 21 | 1 | 95.5% | 96.64 | 0.57 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

🚨 **CRÍTICO: Los filtros de expiración están bloqueando señales BUENAS**
- 22 señales expiradas tienen 95.5% TP_FIRST
- **Acción requerida:** Relajar filtros de expiración (`MaxDistanceToEntry_ATR_Cancel`, `STALE_TIME`)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | SELL | 59.00 | 7.00 | 8.43 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0002 | SELL | 60.00 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0003 | SELL | 58.00 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0004 | SELL | 58.00 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0005 | SELL | 60.00 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0006 | SELL | 60.00 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0007 | SELL | 60.00 | 0.00 | 999.00 | TP_FIRST | EXPIRED | ✅ Entrada excelente |
| T0008 | SELL | 60.00 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0009 | SELL | 60.00 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0010 | SELL | 68.50 | 0.00 | 999.00 | TP_FIRST | EXPIRED | ✅ Entrada excelente |
| T0011 | SELL | 78.25 | 0.00 | 999.00 | TP_FIRST | EXPIRED | ✅ Entrada excelente |
| T0012 | SELL | 71.50 | 0.00 | 999.00 | TP_FIRST | EXPIRED | ✅ Entrada excelente |
| T0013 | SELL | 108.50 | 0.00 | 999.00 | TP_FIRST | EXPIRED | ✅ Entrada excelente |
| T0014 | SELL | 108.50 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0015 | SELL | 108.50 | 0.00 | 999.00 | TP_FIRST | EXPIRED | ✅ Entrada excelente |
| T0016 | SELL | 108.50 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0017 | SELL | 122.75 | 0.00 | 999.00 | TP_FIRST | EXPIRED | ✅ Entrada excelente |
| T0018 | SELL | 121.75 | 0.00 | 999.00 | TP_FIRST | EXPIRED | ✅ Entrada excelente |
| T0019 | SELL | 121.75 | 0.00 | 999.00 | TP_FIRST | EXPIRED | ✅ Entrada excelente |
| T0020 | SELL | 121.75 | 0.00 | 999.00 | TP_FIRST | EXPIRED | ✅ Entrada excelente |

---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 35.84 pts
- **Mediana:** 33.73 pts
- **Min/Max:** 8.80 / 49.48 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 33.73 |
| P70 | 48.55 |
| P80 | 49.23 |
| P90 | 49.48 |
| P95 | 49.48 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 56.20 pts
- **Mediana:** 44.00 pts
- **Min/Max:** 15.50 / 110.75 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 44.00 |
| P70 | 64.00 |
| P80 | 91.30 |
| P90 | 109.28 |
| P95 | 110.75 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 49; // Era 60
public int MaxTPDistancePoints { get; set; } = 109; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 49.5pts, TP: 109.3pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (0.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.000) / 0.000
R:R_min = 1.75
```

**Estado actual:** R:R promedio = 1.57
**Gap:** 0.18 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **49** (P90 real)
2. **MaxTPDistancePoints:** 120 → **109** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.57) < R:R mínimo (1.75)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=0.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-10 09:01:22*