# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-11 19:09:10
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_182510.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251111_182510.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 35
- **Win Rate:** 22.9% (8/35)
- **Profit Factor:** 0.38
- **Avg R:R Planeado:** 1.71
- **R:R Mínimo para Break-Even:** 3.38

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 31 puntos
   - TP máximo observado: 50 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.71
   - R:R necesario: 3.38
   - **Gap:** 1.66

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8207 | 34.9% |
| Bearish | 6357 | 27.0% |
| Bullish | 8958 | 38.1% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.076
- **Score Min/Max:** [-0.960, 0.960]
- **Componentes (promedio):**
  - EMA20 Slope: 0.040
  - EMA50 Cross: 0.181
  - BOS Count: 0.008
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
| StructureFusion | 39641 | 100.0% | 100.0% |
| ProximityAnalyzer | 7 | 0.0% | 0.0% |
| DFM_Evaluated | 191 | 2728.6% | 0.5% |
| DFM_Passed | 172 | 90.1% | 0.4% |
| RiskCalculator | 48 | 27.9% | 0.1% |
| TradeManager | 35 | 72.9% | 0.1% |

**Análisis:**
- **Mayor caída:** ProximityAnalyzer (pierde 39634 señales, -100.0%)
- **Tasa de conversión final:** 0.09% (de 39641 zonas iniciales → 35 operaciones)

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 378 | 92.6% |
| P0_ANY_DIR | 30 | 7.4% |

### 3.5 Análisis MFE/MAE (Excursión del Precio)

**Métricas Globales:**

- **MFE Promedio:** 30.72 pts (máxima ganancia flotante)
- **MAE Promedio:** 43.36 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 1.51

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 11 | 31.4% |
| SL_FIRST (precio fue hacia SL) | 23 | 65.7% |
| NEUTRAL (sin dirección clara) | 1 | 2.9% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 31.4%
- **Entradas Malas (MAE > MFE):** 68.6%

⚠️ **ALERTA:** >60% de entradas tienen MAE > MFE
- **Problema:** El precio va más en contra que a favor antes del cierre
- **Causas posibles:**
  1. Timing incorrecto (entramos antes de reversión)
  2. Bias desincronizado (operamos contra tendencia real)
  3. Zonas de baja calidad (sin confluence real)

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 35 | 11 | 23 | 31.4% | 30.72 | 43.36 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0004 | SELL | 35.25 | 20.25 | 1.74 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0004_2 | SELL | 35.25 | 20.25 | 1.74 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0005 | SELL | 2.75 | 29.00 | 0.09 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0006 | SELL | 2.50 | 38.25 | 0.07 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0007 | BUY | 4.00 | 21.75 | 0.18 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0008 | BUY | 4.25 | 36.75 | 0.12 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0012 | SELL | 137.50 | 27.75 | 4.95 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0012_2 | SELL | 137.50 | 27.75 | 4.95 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0012_3 | SELL | 137.50 | 27.75 | 4.95 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0013 | SELL | 19.50 | 49.00 | 0.40 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0014 | SELL | 29.00 | 31.75 | 0.91 | SL_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0015 | BUY | 9.50 | 85.00 | 0.11 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0017 | SELL | 0.00 | 90.75 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0020 | BUY | 14.75 | 16.25 | 0.91 | SL_FIRST | CLOSED | ⚠️ Entrada dudosa |
| T0021 | BUY | 12.00 | 60.25 | 0.20 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0024 | SELL | 16.00 | 34.75 | 0.46 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0025 | SELL | 5.25 | 163.00 | 0.03 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0025_2 | SELL | 5.25 | 163.00 | 0.03 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0027 | BUY | 27.25 | 10.50 | 2.60 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0028 | BUY | 16.50 | 2.25 | 7.33 | TP_FIRST | CLOSED | ✅ Entrada excelente |

---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 13.03 pts
- **Mediana:** 10.98 pts
- **Min/Max:** 3.25 / 31.04 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 10.98 |
| P70 | 17.90 |
| P80 | 22.80 |
| P90 | 26.41 |
| P95 | 28.22 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 21.83 pts
- **Mediana:** 16.75 pts
- **Min/Max:** 5.75 / 50.50 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 16.75 |
| P70 | 27.05 |
| P80 | 35.65 |
| P90 | 49.85 |
| P95 | 50.10 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 26; // Era 60
public int MaxTPDistancePoints { get; set; } = 49; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 26.4pts, TP: 49.9pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (22.9%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.229) / 0.229
R:R_min = 3.38
```

**Estado actual:** R:R promedio = 1.71
**Gap:** 1.66 (necesitas mejorar R:R)

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
2. **MaxTPDistancePoints:** 120 → **49** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.71) < R:R mínimo (3.38)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=22.9%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-11 19:09:10*