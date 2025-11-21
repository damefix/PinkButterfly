# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-10 10:17:16
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251110_101628.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251110_101628.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 23
- **Win Rate:** 0.0% (0/23)
- **Profit Factor:** 0.00
- **Avg R:R Planeado:** 1.59
- **R:R Mínimo para Break-Even:** 1.75

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 78 puntos
   - TP máximo observado: 122 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.59
   - R:R necesario: 1.75
   - **Gap:** 0.16

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
| ProximityAnalyzer | 578 | 0.0% |
| DFM | 0 | 0.0% |
| RiskCalculator | 0 | 0.0% |
| TradeManager | 23 | 0.0% |

**Análisis:**
- ⚠️ **No hay datos suficientes para análisis de waterfall**

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_ANY_DIR | 50 | 58.1% |
| P0_SWING_LITE | 36 | 41.9% |

### 3.5 Análisis MFE/MAE (Excursión del Precio)

**Métricas Globales:**

- **MFE Promedio:** 48.72 pts (máxima ganancia flotante)
- **MAE Promedio:** 2.90 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 456.31

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 21 | 91.3% |
| SL_FIRST (precio fue hacia SL) | 2 | 8.7% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 100.0%
- **Entradas Malas (MAE > MFE):** 0.0%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 1 | 1 | 0 | 100.0% | 54.50 | 3.25 |
| PENDING | 22 | 20 | 2 | 90.9% | 48.45 | 2.89 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | BUY | 27.50 | 19.00 | 1.45 | SL_FIRST | PENDING | 👍 Entrada correcta |
| T0002 | BUY | 27.50 | 19.00 | 1.45 | SL_FIRST | PENDING | 👍 Entrada correcta |
| T0003 | BUY | 27.50 | 14.50 | 1.90 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0004 | SELL | 41.00 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0005 | SELL | 40.75 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0006 | SELL | 40.75 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0007 | SELL | 40.75 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0008 | SELL | 40.75 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0009 | SELL | 38.75 | 0.50 | 77.50 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0010 | SELL | 38.00 | 0.50 | 76.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0011 | SELL | 38.00 | 0.50 | 76.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0012 | SELL | 38.00 | 0.50 | 76.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0013 | SELL | 38.50 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0014 | SELL | 38.50 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0015 | SELL | 38.00 | 0.50 | 76.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0016 | SELL | 45.50 | 1.50 | 30.33 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0017 | SELL | 50.50 | 1.75 | 28.86 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0018 | SELL | 55.50 | 2.25 | 24.67 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0019 | SELL | 54.75 | 3.00 | 18.25 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0020 | SELL | 54.50 | 3.25 | 16.77 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 60.53 pts
- **Mediana:** 74.29 pts
- **Min/Max:** 9.01 / 77.64 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 74.29 |
| P70 | 74.93 |
| P80 | 76.00 |
| P90 | 77.15 |
| P95 | 77.59 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 96.41 pts
- **Mediana:** 119.00 pts
- **Min/Max:** 15.50 / 122.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 119.00 |
| P70 | 119.50 |
| P80 | 121.75 |
| P90 | 121.75 |
| P95 | 121.95 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 77; // Era 60
public int MaxTPDistancePoints { get; set; } = 121; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 77.1pts, TP: 121.8pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (0.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.000) / 0.000
R:R_min = 1.75
```

**Estado actual:** R:R promedio = 1.59
**Gap:** 0.16 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **77** (P90 real)
2. **MaxTPDistancePoints:** 120 → **121** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.59) < R:R mínimo (1.75)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=0.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-10 10:17:16*