# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-11 12:55:00
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_121149.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251111_121149.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 14
- **Win Rate:** 0.0% (0/14)
- **Profit Factor:** 0.72
- **Avg R:R Planeado:** 1.29
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
   - R:R actual: 1.29
   - R:R necesario: 1.75
   - **Gap:** 0.46

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8618 | 36.9% |
| Bearish | 5922 | 25.4% |
| Bullish | 8802 | 37.7% |

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
- **Consecuencia:** Sistema queda 36.9% Neutral → bias no diferencia tendencias

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
| StructureFusion | 42180 | 100.0% | 100.0% |
| ProximityAnalyzer | 6 | 0.0% | 0.0% |
| DFM_Evaluated | 416 | 6933.3% | 1.0% |
| DFM_Passed | 0 | 0.0% | 0.0% |
| RiskCalculator | 0 | 0.0% | 0.0% |
| TradeManager | 14 | 0.0% | 0.0% |

**Análisis:**
- **Mayor caída:** DFM_Passed (pierde 416 señales, -100.0%)
- **Tasa de conversión final:** 0.03% (de 42180 zonas iniciales → 14 operaciones)

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 362 | 91.9% |
| P0_ANY_DIR | 32 | 8.1% |

### 3.5 Análisis MFE/MAE (Excursión del Precio)

**Métricas Globales:**

- **MFE Promedio:** 39.18 pts (máxima ganancia flotante)
- **MAE Promedio:** 53.79 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 3.59

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 10 | 71.4% |
| SL_FIRST (precio fue hacia SL) | 4 | 28.6% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 35.7%
- **Entradas Malas (MAE > MFE):** 64.3%

⚠️ **ALERTA:** >60% de entradas tienen MAE > MFE
- **Problema:** El precio va más en contra que a favor antes del cierre
- **Causas posibles:**
  1. Timing incorrecto (entramos antes de reversión)
  2. Bias desincronizado (operamos contra tendencia real)
  3. Zonas de baja calidad (sin confluence real)

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 14 | 10 | 4 | 71.4% | 39.18 | 53.79 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | SELL | 118.00 | 37.25 | 3.17 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0006 | SELL | 58.75 | 68.75 | 0.85 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0008 | SELL | 9.50 | 60.25 | 0.16 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0008_2 | SELL | 9.50 | 60.25 | 0.16 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0009 | SELL | 70.50 | 3.50 | 20.14 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0011 | SELL | 21.25 | 90.25 | 0.24 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0011_2 | SELL | 21.25 | 90.25 | 0.24 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0012 | SELL | 10.75 | 75.50 | 0.14 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0014 | SELL | 26.75 | 163.25 | 0.16 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0015 | BUY | 0.00 | 11.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0016 | SELL | 42.50 | 13.25 | 3.21 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0018 | SELL | 56.75 | 13.00 | 4.37 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0020 | SELL | 63.00 | 3.75 | 16.80 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0023 | SELL | 40.00 | 62.50 | 0.64 | TP_FIRST | CLOSED | ❌ Entrada muy mala |

---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 29.17 pts
- **Mediana:** 31.86 pts
- **Min/Max:** 4.37 / 58.44 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 31.86 |
| P70 | 37.02 |
| P80 | 38.18 |
| P90 | 49.44 |
| P95 | 62.94 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 35.96 pts
- **Mediana:** 31.88 pts
- **Min/Max:** 5.75 / 66.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 31.88 |
| P70 | 51.00 |
| P80 | 57.25 |
| P90 | 61.62 |
| P95 | 68.19 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 49; // Era 60
public int MaxTPDistancePoints { get; set; } = 61; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 49.4pts, TP: 61.6pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (0.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.000) / 0.000
R:R_min = 1.75
```

**Estado actual:** R:R promedio = 1.29
**Gap:** 0.46 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **61** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.29) < R:R mínimo (1.75)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=0.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-11 12:55:00*