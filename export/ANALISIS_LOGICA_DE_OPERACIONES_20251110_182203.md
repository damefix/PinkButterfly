# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-10 18:48:53
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251110_182203.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251110_182203.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 1793
- **Win Rate:** 0.0% (0/1793)
- **Profit Factor:** 1.09
- **Avg R:R Planeado:** 1.72
- **R:R Mínimo para Break-Even:** 1.75

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 88 puntos
   - TP máximo observado: 136 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.72
   - R:R necesario: 1.75
   - **Gap:** 0.03

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
| ProximityAnalyzer | 62508 | 0.0% |
| DFM | 0 | 0.0% |
| RiskCalculator | 0 | 0.0% |
| TradeManager | 1793 | 0.0% |

**Análisis:**
- ⚠️ **No hay datos suficientes para análisis de waterfall**

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 4208 | 59.3% |
| P0_ANY_DIR | 2889 | 40.7% |

### 3.5 Análisis MFE/MAE (Excursión del Precio)

**Métricas Globales:**

- **MFE Promedio:** 38.22 pts (máxima ganancia flotante)
- **MAE Promedio:** 12.67 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 526.51

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 1504 | 83.9% |
| SL_FIRST (precio fue hacia SL) | 284 | 15.8% |
| NEUTRAL (sin dirección clara) | 5 | 0.3% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 75.5%
- **Entradas Malas (MAE > MFE):** 24.5%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 161 | 111 | 45 | 68.9% | 28.33 | 21.20 |
| PENDING | 1129 | 950 | 179 | 84.1% | 37.91 | 13.07 |
| EXPIRED | 417 | 389 | 28 | 93.3% | 46.83 | 5.65 |
| CANCELLED | 86 | 54 | 32 | 62.8% | 19.04 | 25.50 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

🚨 **CRÍTICO: Los filtros de expiración están bloqueando señales BUENAS**
- 417 señales expiradas tienen 93.3% TP_FIRST
- **Acción requerida:** Relajar filtros de expiración (`MaxDistanceToEntry_ATR_Cancel`, `STALE_TIME`)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | BUY | 13.50 | 6.25 | 2.16 | SL_FIRST | PENDING | ✅ Entrada excelente |
| T0002 | BUY | 12.25 | 7.50 | 1.63 | SL_FIRST | PENDING | ✅ Entrada excelente |
| T0003 | BUY | 24.50 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0004 | BUY | 31.25 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0005 | BUY | 31.75 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0006 | BUY | 31.75 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0007 | BUY | 31.00 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0008 | BUY | 32.50 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0009 | BUY | 31.00 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0010 | BUY | 31.00 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0011 | BUY | 32.50 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0012 | BUY | 32.50 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0013 | BUY | 32.50 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0014 | BUY | 32.50 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0015 | BUY | 32.50 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0016 | BUY | 41.00 | 0.00 | 999.00 | TP_FIRST | PENDING | ✅ Entrada excelente |
| T0017 | BUY | 45.50 | 0.00 | 999.00 | TP_FIRST | EXPIRED | ✅ Entrada excelente |
| T0018 | BUY | 48.00 | 0.00 | 999.00 | TP_FIRST | EXPIRED | ✅ Entrada excelente |
| T0019 | BUY | 45.25 | 0.00 | 999.00 | TP_FIRST | EXPIRED | ✅ Entrada excelente |
| T0020 | BUY | 45.25 | 0.00 | 999.00 | TP_FIRST | EXPIRED | ✅ Entrada excelente |

---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 14.08 pts
- **Mediana:** 10.93 pts
- **Min/Max:** 0.57 / 88.47 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 10.93 |
| P70 | 16.71 |
| P80 | 20.77 |
| P90 | 26.86 |
| P95 | 36.02 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 18.27 pts
- **Mediana:** 14.50 pts
- **Min/Max:** 2.25 / 135.75 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 14.50 |
| P70 | 19.50 |
| P80 | 22.50 |
| P90 | 29.75 |
| P95 | 51.08 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 26; // Era 60
public int MaxTPDistancePoints { get; set; } = 29; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 26.9pts, TP: 29.8pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (0.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.000) / 0.000
R:R_min = 1.75
```

**Estado actual:** R:R promedio = 1.72
**Gap:** 0.03 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **29** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.72) < R:R mínimo (1.75)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=0.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-10 18:48:53*