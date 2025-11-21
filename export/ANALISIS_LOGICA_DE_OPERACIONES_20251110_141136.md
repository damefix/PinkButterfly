# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-10 14:31:09
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251110_141136.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251110_141136.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 1732
- **Win Rate:** 0.0% (0/1732)
- **Profit Factor:** 1.00
- **Avg R:R Planeado:** 1.63
- **R:R Mínimo para Break-Even:** 1.75

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 88 puntos
   - TP máximo observado: 87 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.63
   - R:R necesario: 1.75
   - **Gap:** 0.12

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
| ProximityAnalyzer | 36580 | 0.0% |
| DFM | 0 | 0.0% |
| RiskCalculator | 0 | 0.0% |
| TradeManager | 1732 | 0.0% |

**Análisis:**
- ⚠️ **No hay datos suficientes para análisis de waterfall**

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 1957 | 58.7% |
| P0_ANY_DIR | 1379 | 41.3% |

### 3.5 Análisis MFE/MAE (Excursión del Precio)

**Métricas Globales:**

- **MFE Promedio:** 31.80 pts (máxima ganancia flotante)
- **MAE Promedio:** 13.87 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 428.16

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 1320 | 76.2% |
| SL_FIRST (precio fue hacia SL) | 397 | 22.9% |
| NEUTRAL (sin dirección clara) | 15 | 0.9% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 68.8%
- **Entradas Malas (MAE > MFE):** 31.2%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 272 | 207 | 59 | 76.1% | 33.47 | 15.88 |
| PENDING | 1355 | 1051 | 295 | 77.6% | 32.39 | 12.64 |
| CANCELLED | 105 | 62 | 43 | 59.0% | 19.80 | 24.49 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | BUY | 59.25 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0002 | BUY | 59.25 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0003 | SELL | 1.50 | 39.00 | 0.04 | SL_FIRST | CANCELLED | ❌ Entrada muy mala |
| T0004 | BUY | 34.75 | 1.50 | 23.17 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0005 | BUY | 34.75 | 1.50 | 23.17 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0006 | BUY | 34.75 | 1.50 | 23.17 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0007 | BUY | 0.00 | 20.00 | 0.00 | SL_FIRST | PENDING | ❌ Entrada muy mala |
| T0008 | BUY | 0.00 | 20.00 | 0.00 | SL_FIRST | PENDING | ❌ Entrada muy mala |
| T0009 | BUY | 12.25 | 7.50 | 1.63 | SL_FIRST | PENDING | ✅ Entrada excelente |
| T0010 | BUY | 31.25 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0011 | BUY | 31.75 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0012 | BUY | 18.50 | 9.25 | 2.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0013 | BUY | 32.50 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0014 | BUY | 31.00 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0015 | BUY | 31.00 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0016 | BUY | 32.50 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0017 | BUY | 32.50 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0018 | BUY | 31.00 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0019 | BUY | 31.00 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0020 | BUY | 32.50 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |

---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 13.53 pts
- **Mediana:** 10.54 pts
- **Min/Max:** 0.62 / 88.30 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 10.54 |
| P70 | 15.50 |
| P80 | 19.76 |
| P90 | 26.47 |
| P95 | 34.87 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 17.30 pts
- **Mediana:** 14.00 pts
- **Min/Max:** 1.75 / 87.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 14.00 |
| P70 | 19.00 |
| P80 | 22.50 |
| P90 | 30.50 |
| P95 | 46.09 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 26; // Era 60
public int MaxTPDistancePoints { get; set; } = 30; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 26.5pts, TP: 30.5pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (0.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.000) / 0.000
R:R_min = 1.75
```

**Estado actual:** R:R promedio = 1.63
**Gap:** 0.12 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **26** (P90 real)
2. **MaxTPDistancePoints:** 120 → **30** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.63) < R:R mínimo (1.75)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=0.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-10 14:31:09*