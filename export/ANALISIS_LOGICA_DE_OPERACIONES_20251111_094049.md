# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-11 10:13:44
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_094049.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251111_094049.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 27
- **Win Rate:** 0.0% (0/27)
- **Profit Factor:** 0.65
- **Avg R:R Planeado:** 1.47
- **R:R Mínimo para Break-Even:** 1.75

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 61 puntos
   - TP máximo observado: 64 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.47
   - R:R necesario: 1.75
   - **Gap:** 0.28

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
| ProximityAnalyzer | 5822 | 0.0% |
| DFM | 0 | 0.0% |
| RiskCalculator | 0 | 0.0% |
| TradeManager | 27 | 0.0% |

**Análisis:**
- ⚠️ **No hay datos suficientes para análisis de waterfall**

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 144 | 81.8% |
| P0_ANY_DIR | 32 | 18.2% |

### 3.5 Análisis MFE/MAE (Excursión del Precio)

**Métricas Globales:**

- **MFE Promedio:** 29.30 pts (máxima ganancia flotante)
- **MAE Promedio:** 46.41 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 2.36

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 12 | 44.4% |
| SL_FIRST (precio fue hacia SL) | 15 | 55.6% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 37.0%
- **Entradas Malas (MAE > MFE):** 63.0%

⚠️ **ALERTA:** >60% de entradas tienen MAE > MFE
- **Problema:** El precio va más en contra que a favor antes del cierre
- **Causas posibles:**
  1. Timing incorrecto (entramos antes de reversión)
  2. Bias desincronizado (operamos contra tendencia real)
  3. Zonas de baja calidad (sin confluence real)

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 27 | 12 | 15 | 44.4% | 29.30 | 46.41 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | BUY | 17.25 | 12.25 | 1.41 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0005 | BUY | 15.75 | 39.75 | 0.40 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0006 | SELL | 1.25 | 37.25 | 0.03 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0007 | BUY | 8.00 | 40.75 | 0.20 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0009 | SELL | 136.25 | 9.00 | 15.14 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0016 | SELL | 9.50 | 60.25 | 0.16 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0016_2 | SELL | 9.50 | 60.25 | 0.16 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0018 | SELL | 29.00 | 32.25 | 0.90 | SL_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0019 | SELL | 30.00 | 33.25 | 0.90 | SL_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0020 | SELL | 86.25 | 15.00 | 5.75 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0022 | SELL | 21.25 | 90.25 | 0.24 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0022_2 | SELL | 21.25 | 90.25 | 0.24 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0023 | SELL | 10.75 | 123.50 | 0.09 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0024 | BUY | 3.50 | 87.50 | 0.04 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0027 | SELL | 26.75 | 163.25 | 0.16 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0029 | BUY | 23.00 | 10.50 | 2.19 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0029_2 | BUY | 23.00 | 10.50 | 2.19 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0032 | SELL | 42.50 | 13.25 | 3.21 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0035 | SELL | 57.25 | 8.50 | 6.74 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0036 | SELL | 42.75 | 13.75 | 3.11 | SL_FIRST | CLOSED | ✅ Entrada excelente |

---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 22.24 pts
- **Mediana:** 22.27 pts
- **Min/Max:** 4.10 / 60.95 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 22.27 |
| P70 | 30.34 |
| P80 | 34.27 |
| P90 | 44.04 |
| P95 | 59.95 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 28.81 pts
- **Mediana:** 25.75 pts
- **Min/Max:** 5.75 / 64.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 25.75 |
| P70 | 37.40 |
| P80 | 50.55 |
| P90 | 57.25 |
| P95 | 61.30 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 44; // Era 60
public int MaxTPDistancePoints { get; set; } = 57; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 44.0pts, TP: 57.2pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (0.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.000) / 0.000
R:R_min = 1.75
```

**Estado actual:** R:R promedio = 1.47
**Gap:** 0.28 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **57** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.47) < R:R mínimo (1.75)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=0.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-11 10:13:44*