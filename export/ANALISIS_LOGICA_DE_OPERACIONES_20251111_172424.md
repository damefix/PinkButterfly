# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-11 18:09:52
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_172424.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251111_172424.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 9
- **Win Rate:** 0.0% (0/9)
- **Profit Factor:** 0.00
- **Avg R:R Planeado:** 2.21
- **R:R Mínimo para Break-Even:** 1.75

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 26 puntos
   - TP máximo observado: 50 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.21
   - R:R necesario: 1.75
   - **Gap:** -0.46

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8214 | 34.9% |
| Bearish | 6357 | 27.0% |
| Bullish | 8947 | 38.0% |

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
| StructureFusion | 39569 | 100.0% | 100.0% |
| ProximityAnalyzer | 5 | 0.0% | 0.0% |
| DFM_Evaluated | 32 | 640.0% | 0.1% |
| DFM_Passed | 0 | 0.0% | 0.0% |
| RiskCalculator | 0 | 0.0% | 0.0% |
| TradeManager | 9 | 0.0% | 0.0% |

**Análisis:**
- **Mayor caída:** DFM_Passed (pierde 32 señales, -100.0%)
- **Tasa de conversión final:** 0.02% (de 39569 zonas iniciales → 9 operaciones)

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 81 | 96.4% |
| P0_ANY_DIR | 3 | 3.6% |

### 3.5 Análisis MFE/MAE (Excursión del Precio)

**Métricas Globales:**

- **MFE Promedio:** 54.56 pts (máxima ganancia flotante)
- **MAE Promedio:** 39.25 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 2.26

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 5 | 55.6% |
| SL_FIRST (precio fue hacia SL) | 4 | 44.4% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 55.6%
- **Entradas Malas (MAE > MFE):** 44.4%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 9 | 5 | 4 | 55.6% | 54.56 | 39.25 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0001 | BUY | 18.75 | 15.75 | 1.19 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0003 | SELL | 137.50 | 27.75 | 4.95 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0003_2 | SELL | 137.50 | 27.75 | 4.95 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0004 | SELL | 19.50 | 52.75 | 0.37 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0005 | BUY | 9.50 | 85.00 | 0.11 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0008 | BUY | 22.25 | 47.50 | 0.47 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0009 | SELL | 53.50 | 21.50 | 2.49 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0011 | SELL | 85.00 | 15.00 | 5.67 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0013 | SELL | 7.50 | 60.25 | 0.12 | SL_FIRST | CLOSED | ❌ Entrada muy mala |

---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 11.66 pts
- **Mediana:** 9.61 pts
- **Min/Max:** 3.25 / 25.67 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 9.61 |
| P70 | 19.45 |
| P80 | 23.43 |
| P90 | 25.67 |
| P95 | 26.79 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 24.22 pts
- **Mediana:** 19.50 pts
- **Min/Max:** 6.25 / 49.75 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 19.50 |
| P70 | 36.00 |
| P80 | 48.75 |
| P90 | 49.75 |
| P95 | 50.25 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 25; // Era 60
public int MaxTPDistancePoints { get; set; } = 49; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 25.7pts, TP: 49.8pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (0.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.000) / 0.000
R:R_min = 1.75
```

**Estado actual:** R:R promedio = 2.21
**Gap:** -0.46 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **25** (P90 real)
2. **MaxTPDistancePoints:** 120 → **49** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.21) < R:R mínimo (1.75)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=0.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-11 18:09:52*