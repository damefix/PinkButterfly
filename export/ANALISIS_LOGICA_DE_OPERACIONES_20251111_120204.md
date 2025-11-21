# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-11 12:05:24
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_120204.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251111_120204.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 17
- **Win Rate:** 0.0% (0/17)
- **Profit Factor:** 0.75
- **Avg R:R Planeado:** 1.49
- **R:R Mínimo para Break-Even:** 1.75

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 58 puntos
   - TP máximo observado: 66 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.49
   - R:R necesario: 1.75
   - **Gap:** 0.26

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
| ProximityAnalyzer | 5852 | 0.0% |
| DFM | 0 | 0.0% |
| RiskCalculator | 0 | 0.0% |
| TradeManager | 17 | 0.0% |

**Análisis:**
- ⚠️ **No hay datos suficientes para análisis de waterfall**

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 367 | 91.1% |
| P0_ANY_DIR | 36 | 8.9% |

### 3.5 Análisis MFE/MAE (Excursión del Precio)

**Métricas Globales:**

- **MFE Promedio:** 52.19 pts (máxima ganancia flotante)
- **MAE Promedio:** 52.12 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 4.13

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 11 | 64.7% |
| SL_FIRST (precio fue hacia SL) | 6 | 35.3% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 47.1%
- **Entradas Malas (MAE > MFE):** 52.9%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 17 | 11 | 6 | 64.7% | 52.19 | 52.12 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | SELL | 0.00 | 44.50 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0002 | SELL | 137.50 | 27.75 | 4.95 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0003 | SELL | 234.50 | 25.75 | 9.11 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0008 | SELL | 58.75 | 68.75 | 0.85 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0010 | SELL | 9.50 | 60.25 | 0.16 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0010_2 | SELL | 9.50 | 60.25 | 0.16 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0011 | SELL | 70.50 | 3.50 | 20.14 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0013 | SELL | 21.25 | 90.25 | 0.24 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0013_2 | SELL | 21.25 | 90.25 | 0.24 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0014 | SELL | 10.75 | 138.00 | 0.08 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0015 | SELL | 65.50 | 8.50 | 7.71 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0017 | SELL | 26.75 | 162.50 | 0.16 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0018 | SELL | 42.50 | 13.25 | 3.21 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0020 | SELL | 19.25 | 13.25 | 1.45 | SL_FIRST | CLOSED | 👍 Entrada correcta |
| T0021 | SELL | 56.75 | 13.00 | 4.37 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0023 | SELL | 63.00 | 3.75 | 16.80 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0026 | SELL | 40.00 | 62.50 | 0.64 | TP_FIRST | CLOSED | ❌ Entrada muy mala |

---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 25.34 pts
- **Mediana:** 28.05 pts
- **Min/Max:** 4.68 / 58.44 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 28.05 |
| P70 | 35.13 |
| P80 | 37.48 |
| P90 | 44.04 |
| P95 | 60.24 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 32.26 pts
- **Mediana:** 29.25 pts
- **Min/Max:** 6.25 / 66.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 29.25 |
| P70 | 42.95 |
| P80 | 54.85 |
| P90 | 59.00 |
| P95 | 66.88 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 44; // Era 60
public int MaxTPDistancePoints { get; set; } = 59; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 44.0pts, TP: 59.0pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (0.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.000) / 0.000
R:R_min = 1.75
```

**Estado actual:** R:R promedio = 1.49
**Gap:** 0.26 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **44** (P90 real)
2. **MaxTPDistancePoints:** 120 → **59** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.49) < R:R mínimo (1.75)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=0.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-11 12:05:24*