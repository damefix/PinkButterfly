# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-11 16:02:43
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_155805.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251111_155805.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 49
- **Win Rate:** 0.0% (0/49)
- **Profit Factor:** 0.49
- **Avg R:R Planeado:** 1.68
- **R:R Mínimo para Break-Even:** 1.75

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 57 puntos
   - TP máximo observado: 139 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.68
   - R:R necesario: 1.75
   - **Gap:** 0.07

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8209 | 34.9% |
| Bearish | 6359 | 27.0% |
| Bullish | 8941 | 38.0% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.076
- **Score Min/Max:** [-0.960, 0.960]
- **Componentes (promedio):**
  - EMA20 Slope: 0.040
  - EMA50 Cross: 0.181
  - BOS Count: 0.007
  - Regression 24h: 0.085

**Análisis:**
- Threshold actual: 0.5/-0.5
- Score máximo observado: 0.960 (apenas supera threshold)
- Score mínimo observado: -0.960 (apenas supera threshold)
- **Consecuencia:** Sistema queda 34.9% Neutral → bias no diferencia tendencias

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
- Score promedio: 0.076
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
| StructureFusion | 42050 | 100.0% | 100.0% |
| ProximityAnalyzer | 6 | 0.0% | 0.0% |
| DFM_Evaluated | 172 | 2866.7% | 0.4% |
| DFM_Passed | 0 | 0.0% | 0.0% |
| RiskCalculator | 0 | 0.0% | 0.0% |
| TradeManager | 49 | 0.0% | 0.1% |

**Análisis:**
- **Mayor caída:** DFM_Passed (pierde 172 señales, -100.0%)
- **Tasa de conversión final:** 0.12% (de 42050 zonas iniciales → 49 operaciones)

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 362 | 91.6% |
| P0_ANY_DIR | 33 | 8.4% |

### 3.5 Análisis MFE/MAE (Excursión del Precio)

**Métricas Globales:**

- **MFE Promedio:** 32.56 pts (máxima ganancia flotante)
- **MAE Promedio:** 43.83 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 2.07

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 10 | 20.4% |
| SL_FIRST (precio fue hacia SL) | 39 | 79.6% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 36.7%
- **Entradas Malas (MAE > MFE):** 63.3%

⚠️ **ALERTA:** >60% de entradas tienen MAE > MFE
- **Problema:** El precio va más en contra que a favor antes del cierre
- **Causas posibles:**
  1. Timing incorrecto (entramos antes de reversión)
  2. Bias desincronizado (operamos contra tendencia real)
  3. Zonas de baja calidad (sin confluence real)

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 49 | 10 | 39 | 20.4% | 32.56 | 43.83 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0004 | SELL | 2.75 | 34.25 | 0.08 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0005 | SELL | 2.75 | 35.75 | 0.08 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0006 | SELL | 0.00 | 44.50 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0008 | BUY | 4.25 | 44.50 | 0.10 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0008_2 | BUY | 4.25 | 44.50 | 0.10 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0009 | BUY | 0.00 | 39.00 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0010 | BUY | 8.75 | 11.50 | 0.76 | SL_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0011 | SELL | 248.00 | 11.75 | 21.11 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0019 | SELL | 58.75 | 68.75 | 0.85 | TP_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0020 | SELL | 7.50 | 80.75 | 0.09 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0020_2 | SELL | 7.50 | 80.75 | 0.09 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0023 | SELL | 17.25 | 34.50 | 0.50 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0023_2 | SELL | 17.25 | 34.50 | 0.50 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0027 | BUY | 5.75 | 96.50 | 0.06 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0027_2 | BUY | 5.75 | 96.50 | 0.06 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0028 | SELL | 70.50 | 3.50 | 20.14 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0029 | SELL | 37.25 | 69.00 | 0.54 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0032 | SELL | 0.00 | 93.25 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0033 | SELL | 12.50 | 73.50 | 0.17 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0034 | SELL | 13.00 | 48.25 | 0.27 | TP_FIRST | CLOSED | ❌ Entrada muy mala |

---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 20.64 pts
- **Mediana:** 21.33 pts
- **Min/Max:** 3.25 / 57.30 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 21.33 |
| P70 | 27.42 |
| P80 | 34.75 |
| P90 | 40.61 |
| P95 | 54.65 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 34.28 pts
- **Mediana:** 26.00 pts
- **Min/Max:** 5.75 / 139.25 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 26.00 |
| P70 | 42.00 |
| P80 | 54.75 |
| P90 | 66.00 |
| P95 | 101.75 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 40; // Era 60
public int MaxTPDistancePoints { get; set; } = 66; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 40.6pts, TP: 66.0pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (0.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.000) / 0.000
R:R_min = 1.75
```

**Estado actual:** R:R promedio = 1.68
**Gap:** 0.07 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **40** (P90 real)
2. **MaxTPDistancePoints:** 120 → **66** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.68) < R:R mínimo (1.75)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=0.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-11 16:02:43*