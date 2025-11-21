# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-12 11:20:12
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_111602.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251112_111602.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 37
- **Win Rate:** 27.0% (10/37)
- **Profit Factor:** 0.45
- **Avg R:R Planeado:** 1.99
- **R:R Mínimo para Break-Even:** 2.70

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 37 puntos
   - TP máximo observado: 55 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 1.99
   - R:R necesario: 2.70
   - **Gap:** 0.71

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8197 | 34.9% |
| Bullish | 8995 | 38.3% |
| Bearish | 6299 | 26.8% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.079
- **Score Min/Max:** [-0.990, 0.960]
- **Componentes (promedio):**
  - EMA20 Slope: 0.042
  - EMA50 Cross: 0.188
  - BOS Count: 0.009
  - Regression 24h: 0.088

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.960 (apenas supera threshold)
- Score mínimo observado: -0.990 (apenas supera threshold)
- **Consecuencia:** Sistema queda 34.9% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (34.9%)

**Posibles causas:**
- **BOS Score bajo (0.009):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio 0.079 indica poca señal direccional
- **Mercado lateral:** Scores reales [-0.99, 0.96] muy cercanos a 0

**Recomendaciones:**
1. ✅ Verificar que `BOSDetector.cs` establece `Type = breakType` (bug conocido)
2. ✅ Revisar logs para confirmar que BOS Score != 0.0
3. ⚠️ Si BOS sigue en ~0, investigar detección de BOS/CHoCH
4. ⚠️ Considerar bajar threshold a 0.2 SOLO si los 3 pasos anteriores están OK

---

## 2.5 ANÁLISIS DE SEÑALES RECHAZADAS (FILTRO CONTRA-BIAS)

⚠️ **No se encontraron trazas `[DFM][REJECTED]` en el log**

Para activar este análisis, las trazas deben estar presentes en el log.

---

## 3. PILAR 2: ENTRADAS/ZONAS

### 3.1 Waterfall del Pipeline (Embudo de Señales)

| Paso | Zonas/Señales | % vs Anterior | % vs Total |
|------|---------------|---------------|------------|
| StructureFusion | 9896 | 100.0% | 100.0% |
| ProximityAnalyzer | 7 | 0.1% | 0.1% |
| DFM_Evaluated | 206 | 2942.9% | 2.1% |
| DFM_Passed | 188 | 91.3% | 1.9% |
| RiskCalculator | 59 | 31.4% | 0.6% |
| TradeManager | 37 | 62.7% | 0.4% |

**Análisis:**
- **Mayor caída:** ProximityAnalyzer (pierde 9889 señales, -99.9%)
- **Tasa de conversión final:** 0.37% (de 9896 zonas iniciales → 37 operaciones)

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 405 | 92.5% |
| P0_ANY_DIR | 33 | 7.5% |

### 3.5 Análisis MFE/MAE (Excursión del Precio)

**Métricas Globales:**

- **MFE Promedio:** 42.16 pts (máxima ganancia flotante)
- **MAE Promedio:** 39.72 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 30.20

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 12 | 32.4% |
| SL_FIRST (precio fue hacia SL) | 25 | 67.6% |
| NEUTRAL (sin dirección clara) | 0 | 0.0% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 45.9%
- **Entradas Malas (MAE > MFE):** 54.1%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 37 | 12 | 25 | 32.4% | 42.16 | 39.72 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0002 | BUY | 11.75 | 43.75 | 0.27 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0006 | BUY | 4.00 | 21.75 | 0.18 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0008 | BUY | 4.25 | 44.50 | 0.10 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0008_2 | BUY | 4.25 | 44.50 | 0.10 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0008_3 | BUY | 4.25 | 44.50 | 0.10 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0008_4 | BUY | 4.25 | 44.50 | 0.10 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0010 | SELL | 144.00 | 21.25 | 6.78 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0011 | SELL | 245.50 | 14.25 | 17.23 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0011_2 | SELL | 245.50 | 14.25 | 17.23 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0012 | BUY | 36.50 | 125.75 | 0.29 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0014 | SELL | 38.25 | 68.00 | 0.56 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0014_2 | SELL | 38.25 | 68.00 | 0.56 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0018 | SELL | 3.25 | 82.75 | 0.04 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0019 | SELL | 0.00 | 103.75 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0023 | SELL | 65.50 | 8.50 | 7.71 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0023_2 | SELL | 65.50 | 8.50 | 7.71 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0026 | SELL | 13.00 | 53.00 | 0.25 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0028 | SELL | 0.00 | 148.50 | 0.00 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0030 | BUY | 18.50 | 10.50 | 1.76 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0033 | BUY | 23.00 | 43.25 | 0.53 | TP_FIRST | CLOSED | ❌ Entrada muy mala |

---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 12.18 pts
- **Mediana:** 9.05 pts
- **Min/Max:** 3.63 / 36.93 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 9.05 |
| P70 | 14.39 |
| P80 | 19.14 |
| P90 | 24.86 |
| P95 | 36.86 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 22.04 pts
- **Mediana:** 16.75 pts
- **Min/Max:** 5.75 / 55.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 16.75 |
| P70 | 24.40 |
| P80 | 35.00 |
| P90 | 50.35 |
| P95 | 51.85 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 24; // Era 60
public int MaxTPDistancePoints { get; set; } = 50; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 24.9pts, TP: 50.4pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (27.0%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.270) / 0.270
R:R_min = 2.70
```

**Estado actual:** R:R promedio = 1.99
**Gap:** 0.71 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **24** (P90 real)
2. **MaxTPDistancePoints:** 120 → **50** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (1.99) < R:R mínimo (2.70)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=27.0%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-12 11:20:12*