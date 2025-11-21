# ANÁLISIS LOGICA DE OPERACIONES - PinkButterfly CoreBrain

**Fecha:** 2025-11-12 11:11:17
**LOG:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_104041.log`
**CSV:** `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251112_104041.csv`

---

## 1. RESUMEN EJECUTIVO

### KPIs Principales

- **Operaciones Ejecutadas:** 45
- **Win Rate:** 28.9% (13/45)
- **Profit Factor:** 0.70
- **Avg R:R Planeado:** 2.28
- **R:R Mínimo para Break-Even:** 2.46

### 🚨 Problemas Críticos Identificados

1. **BIAS DESINCRONIZADO:** Bias alcista (75%) no refleja movimiento bajista reciente
   - Causa: EMA200@60m demasiado lenta (8+ días)
   - Impacto: Entradas contra-tendencia inmediata

2. **LÍMITES SL/TP NO CALIBRADOS PARA INTRADÍA:**
   - SL máximo observado: 41 puntos
   - TP máximo observado: 55 puntos
   - **120 puntos es swing trading, no intradía** (1.74% del precio)

3. **R:R INSUFICIENTE PARA WR ACTUAL:**
   - R:R actual: 2.28
   - R:R necesario: 2.46
   - **Gap:** 0.18

---

## 2. PILAR 1: BIAS/SENTIMIENTO

### 2.1 Distribución del Bias

| Bias | Eventos | % |
|------|---------|---|
| Neutral | 8178 | 35.0% |
| Bullish | 8901 | 38.1% |
| Bearish | 6295 | 26.9% |

### 2.2 Diagnóstico

**Problema detectado:** 

**Bias Compuesto (V6.0g) - Estadísticas:**
- **Score Promedio:** 0.077
- **Score Min/Max:** [-0.990, 0.960]
- **Componentes (promedio):**
  - EMA20 Slope: 0.040
  - EMA50 Cross: 0.184
  - BOS Count: 0.007
  - Regression 24h: 0.087

**Análisis:**
- Threshold actual: 0.3/-0.3
- Score máximo observado: 0.960 (apenas supera threshold)
- Score mínimo observado: -0.990 (apenas supera threshold)
- **Consecuencia:** Sistema queda 35.0% Neutral → bias no diferencia tendencias

### 2.3 Diagnóstico: Bias Neutral Alto con Threshold Correcto

**Situación:** Threshold ya está en 0.3 (correcto), pero Bias Neutral sigue alto (35.0%)

**Posibles causas:**
- **BOS Score bajo (0.007):** BOS/CHoCH no se detectan correctamente
- **Componentes débiles:** Score promedio 0.077 indica poca señal direccional
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
| StructureFusion | 453453 | 100.0% | 100.0% |
| ProximityAnalyzer | 394 | 0.1% | 0.1% |
| DFM_Evaluated | 34381 | 8726.1% | 7.6% |
| DFM_Passed | 28383 | 82.6% | 6.3% |
| RiskCalculator | 110 | 0.4% | 0.0% |
| TradeManager | 45 | 40.9% | 0.0% |

**Análisis:**
- **Mayor caída:** ProximityAnalyzer (pierde 453059 señales, -99.9%)
- **Tasa de conversión final:** 0.01% (de 453453 zonas iniciales → 45 operaciones)

### 3.2 Distribución por Tipo de TP

| Tipo TP | Count | % |
|---------|-------|---|
| P0_SWING_LITE | 80747 | 95.9% |
| P0_ANY_DIR | 3478 | 4.1% |

### 3.5 Análisis MFE/MAE (Excursión del Precio)

**Métricas Globales:**

- **MFE Promedio:** 43.53 pts (máxima ganancia flotante)
- **MAE Promedio:** 36.65 pts (máxima pérdida flotante)
- **Ratio MFE/MAE:** 48.56

**Dirección Inicial (primeras 3 barras @ 5m):**

| Dirección | Count | % |
|-----------|-------|---|
| TP_FIRST (precio fue hacia TP) | 36 | 80.0% |
| SL_FIRST (precio fue hacia SL) | 7 | 15.6% |
| NEUTRAL (sin dirección clara) | 2 | 4.4% |

**Calidad de Entradas:**

- **Entradas Buenas (MFE > MAE):** 48.9%
- **Entradas Malas (MAE > MFE):** 51.1%

**🔍 Análisis por Estado (Modo Diagnóstico):**

| Estado | Count | TP_FIRST | SL_FIRST | TP_FIRST % | MFE Avg | MAE Avg |
|--------|-------|----------|----------|------------|---------|---------|
| CLOSED | 45 | 36 | 7 | 80.0% | 43.53 | 36.65 |

**💡 Interpretación del Modo Diagnóstico:**

- **TP_FIRST > 50%**: Señales de buena calidad (el precio va primero hacia TP)
- **SL_FIRST > 50%**: Señales de mala calidad (el precio va primero hacia SL)
- **EXPIRED con TP_FIRST alto**: Filtros de expiración demasiado estrictos (están bloqueando buenas señales)
- **EXPIRED con SL_FIRST alto**: Filtros de expiración correctos (bloquean señales malas)

**Detalle por Trade (Top 20):**

| Trade ID | Dir | MFE (pts) | MAE (pts) | Ratio | Initial Dir | Resultado | Diagnóstico |
|----------|-----|-----------|-----------|-------|-------------|-----------|-------------|
| T0003 | BUY | 24.75 | 6.50 | 3.81 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0003_2 | BUY | 24.75 | 6.50 | 3.81 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0004 | BUY | 5.75 | 42.25 | 0.14 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0013 | BUY | 13.75 | 27.25 | 0.50 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0013_2 | BUY | 13.75 | 27.25 | 0.50 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0014 | BUY | 8.25 | 23.00 | 0.36 | SL_FIRST | CLOSED | ❌ Entrada muy mala |
| T0017 | BUY | 19.50 | 14.50 | 1.34 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0019 | SELL | 147.75 | 17.50 | 8.44 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0020 | SELL | 250.50 | 15.75 | 15.90 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0020_2 | SELL | 250.50 | 15.75 | 15.90 | SL_FIRST | CLOSED | ✅ Entrada excelente |
| T0032 | SELL | 71.25 | 64.75 | 1.10 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0032_2 | SELL | 71.25 | 64.75 | 1.10 | TP_FIRST | CLOSED | 👍 Entrada correcta |
| T0035 | BUY | 25.75 | 79.75 | 0.32 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0035_2 | BUY | 25.75 | 79.75 | 0.32 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0039 | BUY | 22.50 | 96.00 | 0.23 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0040 | BUY | 27.50 | 139.25 | 0.20 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0040_2 | BUY | 27.50 | 139.25 | 0.20 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0041 | SELL | 23.75 | 133.50 | 0.18 | TP_FIRST | CLOSED | ❌ Entrada muy mala |
| T0043 | BUY | 27.00 | 4.00 | 6.75 | TP_FIRST | CLOSED | ✅ Entrada excelente |
| T0046 | BUY | 10.75 | 81.25 | 0.13 | TP_FIRST | CLOSED | ❌ Entrada muy mala |

---

## 4. PILAR 3: SL/TP (GESTIÓN DE RIESGO)

### 4.1 Distribución Stop Loss (Puntos)

- **Media:** 17.46 pts
- **Mediana:** 19.30 pts
- **Min/Max:** 1.68 / 41.00 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 19.30 |
| P70 | 22.51 |
| P80 | 24.95 |
| P90 | 28.86 |
| P95 | 33.49 |

### 4.2 Distribución Take Profit (Puntos)

- **Media:** 31.70 pts
- **Mediana:** 32.75 pts
- **Min/Max:** 8.00 / 54.75 pts

**Percentiles:**

| Percentil | Valor (pts) |
|-----------|-------------|
| P50 | 32.75 |
| P70 | 39.05 |
| P80 | 45.15 |
| P90 | 49.00 |
| P95 | 53.25 |

### 4.3 Límites Dinámicos Recomendados (Data-Driven)

**Basado en percentil 90 de operaciones reales:**

```csharp
// En EngineConfig.cs
public int MaxSLDistancePoints { get; set; } = 28; // Era 60
public int MaxTPDistancePoints { get; set; } = 49; // Era 120
```

**Rationale:** Basado en percentil 90 de operaciones reales (SL: 28.9pts, TP: 49.0pts)

### 4.4 R:R Óptimo

**Para Win Rate actual (28.9%), el R:R mínimo es:**

```
R:R_min = (1 - WR) / WR
R:R_min = (1 - 0.289) / 0.289
R:R_min = 2.46
```

**Estado actual:** R:R promedio = 2.28
**Gap:** 0.18 (necesitas mejorar R:R)

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
1. **MaxSLDistancePoints:** 60 → **28** (P90 real)
2. **MaxTPDistancePoints:** 120 → **49** (P90 real)
3. **Límite dinámico por volatilidad:**
   ```
   MaxTPDynamic = min(k * ATR60, MaxTPDistancePoints)
   donde k ≈ 3.0
   ```

**Impacto esperado:** -20% fallback P4, +15% TP estructural

### Prioridad 3: MEJORAR R:R

**Problema:** R:R actual (2.28) < R:R mínimo (2.46)

**Solución:**
1. Aumentar `MinRiskRewardRatio` para fallback P4: 1.0 → **1.5**
2. Forzar selección de TPs más lejanos (P0/P3) sobre fallback
3. Rechazar operaciones con R:R < 1.3 (umbral mínimo)

**Impacto esperado:** Sistema break-even con WR=28.9%

---

*Análisis generado automáticamente por analizador-logica-operaciones.py*
*Fecha: 2025-11-12 11:11:17*