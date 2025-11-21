# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-11 13:54:10
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_133224.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251111_133224.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 37
- **Win Rate:** 0.0% (0/37)
- **Profit Factor:** 0.58
- **Avg R:R Planeado:** 1.49
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
   - R:R actual: 1.49
   - R:R necesario: 1.75
   - **Gap:** 0.26

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8628 | 37.0% |
| Bearish | 5922 | 25.4% |
| Bullish | 8798 | 37.7% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.079
- **Score Min/Max:** [-0.960, 0.960]
- **Componentes (promedio):**
  - EMA20 Slope: 0.040
  - EMA50 Cross: 0.188
  - BOS Count: 0.008
  - Regression 24h: 0.087

**Análisis:**
- Threshold actual: 0.5/-0.5
- Score máximo observado: 0.960 (apenas supera threshold)
- Score mínimo observado: -0.960 (apenas supera threshold)
- **Consecuencia:** Sistema queda 37.0% Neutral → bias no diferencia tendencias

### 2.3 Recomendación: Ajustar Threshold del Bias Compuesto

**Solución Inmediata:** Reducir threshold de 0.5/-0.5 a **0.3/-0.3**

**Archivo:** `pinkbutterfly-produccion/ContextManager.cs` (línea ~207)

```csharp
// ANTES:
if (compositeScore > 0.5) { ... }

// DESPUÉS:
if (compositeScore > 0.3) { ... }  // Más sensible
elif (compositeScore < -0.3) { ... }
```

**Justificación:**
- Scores reales: [-0.96, 0.96]
- Score promedio: 0.079
- Threshold 0.5 requiere 100% alineación de componentes (poco realista)
- Threshold 0.3 requiere 60% alineación (más realista)

**Impacto esperado:**
- Neutral actual: 99.4% → ~60-70% (objetivo)
- Bullish/Bearish: ~0.5% → ~15-20% cada uno
- Sistema empezará a usar el bias para filtrar operaciones

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 41897 | 100.0% | 100.0% |
| ProximityAnalyzer | 6 | 0.0% | 0.0% |
| DFM_Evaluated | 239 | 3983.3% | 0.6% |
| DFM_Passed | 0 | 0.0% | 0.0% |
| RiskCalculator | 0 | 0.0% | 0.0% |
| TradeManager | 37 | 0.0% | 0.1% |

**Análisis:**
- **Mayor caída:** DFM_Passed (pierde 239 señales, -100.0%)
- **Tasa de conversión final:** 0.09% (de 41897 zonas iniciales → 37 operaciones)

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 355 | 91.3% |
| P0_ANY_DIR | 34 | 8.7% |

### 3.5 Análisis MFE/MAE (Excursión del Precio)

**Métricas Globales:**

- **MFE Promedio:** 41.16 pts (máxima ganancia flotante)
- **MAE Promedio:** 41.18 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 2.91

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 14 | 37.8% |
| SL_FIRST (precio fue hacia SL) | 23 | 62.2% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 45.9%
- **Entradas Malas (MAE > MFE):** 54.1%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 37 | 14 | 23 | 37.8% | 41.16 | 41.18 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | BUY | 17.25 | 12.25 | 1.41 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0003 | SELL | 2.75 | 34.25 | 0.08 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0004 | SELL | 2.75 | 35.75 | 0.08 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0006 | BUY | 4.00 | 21.75 | 0.18 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0008 | BUY | 8.00 | 40.75 | 0.20 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0009 | BUY | 0.00 | 39.00 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0010 | BUY | 19.25 | 146.00 | 0.13 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0011 | SELL | 137.50 | 27.75 | 4.95 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0013 | SELL | 248.00 | 11.75 | 21.11 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0013_2 | SELL | 248.00 | 11.75 | 21.11 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0020 | SELL | 58.75 | 68.75 | 0.85 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0022 | SELL | 9.50 | 60.25 | 0.16 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0022_2 | SELL | 9.50 | 60.25 | 0.16 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0024 | SELL | 29.00 | 28.00 | 1.04 | SL_FIRST | CLOSED | 👍 Entrada correcta |
| T0026 | SELL | 30.00 | 33.25 | 0.90 | SL_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0027 | SELL | 86.25 | 15.00 | 5.75 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0029 | SELL | 21.25 | 90.25 | 0.24 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0029_2 | SELL | 21.25 | 90.25 | 0.24 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0030 | SELL | 10.75 | 69.50 | 0.15 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0032 | BUY | 4.00 | 15.50 | 0.26 | SL_FIRST | CLOSED | ❌ Entrada muy mala |

---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 18.34 pts
- **Mediana:** 10.11 pts
- **Min/Max:** 3.82 / 60.95 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 10.11 |
| P70 | 25.34 |
| P80 | 31.86 |
| P90 | 37.70 |
| P95 | 58.69 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 23.75 pts
- **Mediana:** 18.50 pts
- **Min/Max:** 5.00 / 64.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 18.50 |
| P70 | 29.40 |
| P80 | 34.75 |
| P90 | 54.05 |
| P95 | 57.92 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 37; // Era 60
public int MaxTPDistancePoints { get; set; } = 54; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 37.7pts, TP: 54.0pts)

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
1. **MaxSLDistancePoints:** 60 → **37** (P90 real)
2. **MaxTPDistancePoints:** 120 → **54** (P90 real)
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
*Fecha: 2025-11-11 13:54:10*