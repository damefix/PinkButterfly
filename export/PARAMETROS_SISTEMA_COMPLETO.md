# PARÁMETROS DEL SISTEMA PINKBUTTERFLY - DOCUMENTACIÓN COMPLETA

**Generado:** 2025-11-11  
**Archivo fuente:** `pinkbutterfly-produccion/EngineConfig.cs`  
**Propósito:** Registro completo de todos los parámetros configurables para control científico de experimentos

---

## 📋 ÍNDICE

1. [TIMEFRAMES](#1-timeframes)
2. [DETECCIÓN DE ESTRUCTURAS](#2-detección-de-estructuras)
3. [SCORING Y PESOS](#3-scoring-y-pesos)
4. [FUSIÓN Y HEATZONES](#4-fusión-y-heatzones)
5. [DECISION FUSION MODEL (DFM)](#5-decision-fusion-model-dfm)
6. [GESTIÓN DE RIESGO (SL/TP)](#6-gestión-de-riesgo-sltp)
7. [FILTROS DE ENTRADA](#7-filtros-de-entrada)
8. [GESTIÓN DE OPERACIONES](#8-gestión-de-operaciones)
9. [PURGA Y MEMORIA](#9-purga-y-memoria)
10. [SISTEMA Y DEBUG](#10-sistema-y-debug)

---

## 1. TIMEFRAMES

| Parámetro | Valor Actual | Valor Default | Descripción | Estado |
|-----------|--------------|---------------|-------------|--------|
| `TimeframesToUse` | [5, 15, 60, 240, 1440] | [5, 15, 60, 240, 1440] | Timeframes activos (minutos) | ✅ Original |
| `DecisionTimeframeMinutes` | 15 | 15 | TF de decisión del DFM | ✅ Original |
| `BiasPaddingBars60` | 60 | 60 | Barras de padding para bias (60m) | ✅ Original |
| `BacktestBarsForAnalysis` | 2500 | 5000 | Barras históricas a procesar | 🔧 Modificado (reducido para velocidad) |

**Notas:**
- `BacktestBarsForAnalysis = 2500`: Reducido desde 5000 para acelerar tests durante afinación.
- TF 5m: más rápido, más ruido; TF 1440m: más lento, más limpio.

---

## 2. DETECCIÓN DE ESTRUCTURAS

### 2.1 FVG (Fair Value Gap)

| Parámetro | Valor Actual | Valor Default | Descripción | Estado |
|-----------|--------------|---------------|-------------|--------|
| `MinFVGSizeTicks` | 6 | 6 | Tamaño mínimo FVG (ticks absolutos) | ✅ Original |
| `MinFVGSizeATRfactor` | 0.20 | 0.10 | Tamaño mínimo FVG (× ATR) | 🔧 Optimizado (+100%) |
| `MergeConsecutiveFVGs` | true | true | Fusionar FVGs adyacentes | ✅ Original |
| `DetectNestedFVGs` | true | true | Detectar FVGs anidados | ✅ Original |

**Impacto:** `MinFVGSizeATRfactor = 0.20` reduce generación de FVGs en ~67%.

---

### 2.2 SWING

| Parámetro | Valor Actual | Valor Default | Descripción | Estado |
|-----------|--------------|---------------|-------------|--------|
| `MinSwingATRfactor` | 0.15 | 0.08 | Tamaño mínimo swing (× ATR) | 🔧 Optimizado (+87%) |
| `nLeft` | 2 | 2 | Barras izq. para validar swing | ✅ Original |
| `nRight` | 2 | 2 | Barras der. para validar swing | ✅ Original |

**Impacto:** `MinSwingATRfactor = 0.15` reduce Swings de ~9000 → ~3000 (-67%).

---

### 2.3 ORDER BLOCK (OB)

| Parámetro | Valor Actual | Valor Default | Descripción | Estado |
|-----------|--------------|---------------|-------------|--------|
| `OBBodyMinATR` | 0.80 | 0.80 | Cuerpo mínimo OB (× ATR) | ✅ Original |

---

### 2.4 DOUBLE TOP/BOTTOM

| Parámetro | Valor Actual | Valor Default | Descripción | Estado |
|-----------|--------------|---------------|-------------|--------|
| `priceToleranceTicks_DoubleTop` | 8 | 8 | Tolerancia precio (ticks) | ✅ Original |
| `MinBarsBetweenDouble` | 3 | 3 | Mín. barras entre swings | ✅ Original |
| `MaxBarsBetweenDouble` | 200 | 200 | Máx. barras entre swings | ✅ Original |
| `ConfirmBars_Double` | 3 | 3 | Barras para confirmar ruptura | ✅ Original |

---

### 2.5 BOS (BREAK OF STRUCTURE)

| Parámetro | Valor Actual | Valor Default | Descripción | Estado |
|-----------|--------------|---------------|-------------|--------|
| `BreakMomentumBodyFactor` | 0.6 | 0.6 | Factor cuerpo para momentum "Strong" | ✅ Original |
| `BreakMomentumMultiplierStrong` | 1.35 | 1.35 | Multiplicador score BOS fuerte | ✅ Original |
| `BreakMomentumMultiplierWeak` | 1.1 | 1.1 | Multiplicador score BOS débil | ✅ Original |
| `nConfirmBars_BOS` | 1 | 1 | Barras confirmación BOS | ✅ Original |
| `MaxRecentBreaksForBias` | 10 | 10 | BOS recientes para bias | ✅ Original |
| `BOSDebounceBarReq` | 1 | 1 | Cooldown BOS (barras) | ✅ Original |
| `EnableBOSDebounceInHighVolOnly` | true | true | Debounce solo en HighVol | ✅ Original |

---

### 2.6 POI (POINT OF INTEREST)

| Parámetro | Valor Actual | Valor Default | Descripción | Estado |
|-----------|--------------|---------------|-------------|--------|
| `OverlapToleranceATR` | 0.5 | 0.5 | Tolerancia overlap (× ATR) | ✅ Original |
| `MinStructuresForPOI` | 2 | 2 | Mín. estructuras para POI | ✅ Original |
| `POI_ConfluenceBonus` | 0.15 | 0.15 | Bonus por confluencia | ✅ Original |
| `POI_MaxConfluenceBonus` | 0.5 | 0.5 | Máx. bonus confluencia | ✅ Original |
| `POI_PremiumThreshold` | 0.618 | 0.618 | Umbral zona premium (Fibonacci) | ✅ Original |
| `POI_PremiumLookbackBars` | 50 | 50 | Barras lookback premium | ✅ Original |

---

### 2.7 LIQUIDITY VOID (LV)

| Parámetro | Valor Actual | Valor Default | Descripción | Estado |
|-----------|--------------|---------------|-------------|--------|
| `LV_RequireLowVolume` | false | false | Exigir volumen bajo | ✅ Original |
| `LV_VolumeThreshold` | 0.4 | 0.4 | Umbral volumen bajo | ✅ Original |
| `LV_VolumeAvgPeriod` | 20 | 20 | Período promedio volumen | ✅ Original |
| `LV_MinSizeATRFactor` | 0.15 | 0.15 | Tamaño mínimo (× ATR) | ✅ Original |
| `LV_EnableFusion` | true | true | Fusionar LVs cercanos | ✅ Original |
| `LV_FusionToleranceATR` | 0.3 | 0.3 | Tolerancia fusión (× ATR) | ✅ Original |
| `LV_FillThreshold` | 0.95 | 0.95 | Umbral llenado para expirar | ✅ Original |
| `LV_SizeWeight` | 0.4 | 0.4 | Peso tamaño en scoring | ✅ Original |
| `LV_DepthWeight` | 0.3 | 0.3 | Peso profundidad en scoring | ✅ Original |
| `LV_ProximityWeight` | 0.2 | 0.2 | Peso proximidad en scoring | ✅ Original |
| `LV_ConfluenceMultiplier` | 1.3 | 1.3 | Multiplicador confluencia | ✅ Original |

---

### 2.8 LIQUIDITY GRAB (LG)

| Parámetro | Valor Actual | Valor Default | Descripción | Estado |
|-----------|--------------|---------------|-------------|--------|
| `LG_BodyThreshold` | 0.6 | 0.6 | Umbral cuerpo (× ATR) | ✅ Original |
| `LG_RangeThreshold` | 1.2 | 1.2 | Umbral rango (× ATR) | ✅ Original |
| `LG_VolumeSpikeFactor` | 1.5 | 1.5 | Factor spike volumen | ✅ Original |
| `LG_VolumeAvgPeriod` | 20 | 20 | Período promedio volumen | ✅ Original |
| `LG_MaxBarsForReversal` | 3 | 3 | Máx. barras para reversión | ✅ Original |
| `LG_MaxAgeBars` | 20 | 20 | Edad máxima LG (barras) | ✅ Original |
| `LG_SweepStrengthWeight` | 0.3 | 0.3 | Peso fuerza sweep | ✅ Original |
| `LG_VolumeWeight` | 0.25 | 0.25 | Peso volumen | ✅ Original |
| `LG_ReversalWeight` | 0.3 | 0.3 | Peso reversión | ✅ Original |
| `LG_BiasWeight` | 0.15 | 0.15 | Peso bias | ✅ Original |
| `LG_ReversalSetupMultiplier` | 1.3 | 1.3 | Multiplicador setup reversión | ✅ Original |
| `EnableAggressivePurgeForLG` | true | true | Purga agresiva LG antiguas | ✅ Original |

---

## 3. SCORING Y PESOS

### 3.1 PESOS POR TIMEFRAME

| TF (min) | Peso Actual | Peso Default | Descripción | Estado |
|----------|-------------|--------------|-------------|--------|
| 5 | 0.15 | 0.00 | TF 5m (intradía rápido) | 🔧 Añadido |
| 15 | 1.00 | 1.00 | TF 15m (intradía principal) | ✅ Original |
| 60 | 0.85 | 0.85 | TF 60m (H1) | ✅ Original |
| 240 | 0.70 | 0.70 | TF 240m (H4) | ✅ Original |
| 1440 | 0.55 | 0.55 | TF 1440m (Daily) | ✅ Original |

**Nota:** TF 5m añadido recientemente para estructuras intradía rápidas.

---

### 3.2 FACTORES DE SCORING

| Parámetro | Valor Actual | Valor Default | Descripción | Estado |
|-----------|--------------|---------------|-------------|--------|
| `ProxMaxATRFactor` | 2.5 | 2.5 | Distancia máx. para proximidad (× ATR) | ✅ Original |
| `FreshnessLambda` | 20 | 20 | Decaimiento freshness (λ) | ✅ Original |
| `DecayLambda` | 100 | 100 | Decaimiento temporal (λ) | ✅ Original |
| `TouchBodyBonusPerTouch` | 0.12 | 0.12 | Bonus por touch del precio | ✅ Original |
| `MaxTouchBodyCap` | 5 | 5 | Máx. touches para bonus | ✅ Original |
| `ConfluenceWeight` | 0.18 | 0.18 | Peso confluencia en scoring | ✅ Original |

**Fórmulas:**
- **Freshness:** `exp(-age / FreshnessLambda)`
- **Decay:** `exp(-age / DecayLambda)`
- **Proximity:** `1.0 - (distance / (ProxMaxATRFactor × ATR))`

---

### 3.3 FILL Y LLENADO

| Parámetro | Valor Actual | Valor Default | Descripción | Estado |
|-----------|--------------|---------------|-------------|--------|
| `FillThreshold` | 0.90 | 0.90 | Umbral llenado (90%) | ✅ Original |
| `ResidualScore` | 0.05 | 0.05 | Score residual tras fill | ✅ Original |
| `FillPriceStayBars` | 1 | 1 | Barras para confirmar fill | ✅ Original |

---

## 4. FUSIÓN Y HEATZONES

| Parámetro | Valor Actual | Valor Default | Descripción | Estado |
|-----------|--------------|---------------|-------------|--------|
| `HeatZone_OverlapToleranceATR` | 0.5 | 0.5 | Tolerancia overlap fusión (× ATR) | ✅ Original |
| `MaxZoneSizeATR` | 16.0 | 16.0 | Tamaño máx. zona (× ATR) | ✅ Original |
| `HeatZone_MinConfluence` | 2 | 2 | Mín. estructuras para HeatZone | ✅ Original |
| `MaxConfluenceReference` | 5 | 5 | Confluencia de referencia para normalización | ✅ Original |
| `HeatZone_MinScore` | 0.12 | 0.20 | Score mínimo para crear HeatZone | 🔧 Modificado (diagnóstico) |

**Nota:** `HeatZone_MinScore = 0.12` bajado desde 0.20 para permitir estructuras de TF bajos durante diagnóstico.

---

## 5. DECISION FUSION MODEL (DFM)

### 5.1 PESOS DEL DFM

| Parámetro | Valor Actual | Valor Histórico | Descripción | Estado |
|-----------|--------------|-----------------|-------------|--------|
| `Weight_CoreScore` | 0.25 | 0.25 | Peso score estructural | ✅ Original |
| `Weight_Proximity` | 0.15 | 0.30 → 0.15 | Peso proximidad al precio | 🔧 Modificado V6.0k |
| `Weight_Confluence` | 0.00 | 0.15 → 0.00 | Peso confluencia | 🔧 Modificado V6.0k |
| `Weight_Type` | 0.00 | 0.00 | Peso tipo estructura | ✅ Original |
| `Weight_Bias` | 0.40 | 0.30 → 0.40 | Peso alineación bias | 🔧 Modificado V6.0k |
| `Weight_Momentum` | 0.20 | 0.00 → 0.10 → 0.20 | Peso momentum (BOS/CHoCH) | 🔧 Modificado V6.0k |

**Suma total:** 1.00 (obligatorio)

**Cambios V6.0k:**
- ✅ Aumentado `Weight_Bias` 0.30 → 0.40 (filtrar contra-tendencia)
- ✅ Aumentado `Weight_Momentum` 0.00 → 0.20 (priorizar rupturas)
- ✅ Reducido `Weight_Proximity` 0.30 → 0.15 (liberar peso)
- ✅ Eliminado `Weight_Confluence` 0.15 → 0.00 (datos MFE/MAE)

---

### 5.2 THRESHOLDS DE CONFIANZA

| Parámetro | Valor Actual | Valor Histórico | Descripción | Estado |
|-----------|--------------|-----------------|-------------|--------|
| `MinConfidenceForEntry` | 0.50 | 0.55 → 0.65 → 0.72 → 0.75 → 0.65 → **0.50** | Confidence mín. para BUY/SELL | 🔧 MODIFICADO HOY |
| `MinConfidenceForEntry_HighVol` | 0.55 | 0.70 → 0.78 → 0.68 → **0.55** | Confidence mín. HighVol | 🔧 MODIFICADO HOY |
| `MinConfidenceForWait` | 0.50 | 0.50 | Confidence mín. para WAIT | ✅ Original |
| `HV_StrictDistance_MinConfidence` | 0.81 | 0.81 | Confidence para entradas lejanas (>2.0 ATR) | ✅ Original |
| `HV_StrictDistanceGate_ATR` | 2.0 | 2.0 | Distancia gate para confidence estricto | ✅ Original |

**CAMBIO CRÍTICO HOY (2025-11-11):**
- `MinConfidenceForEntry: 0.65 → 0.50` (exploración del espectro completo de señales)
- `MinConfidenceForEntry_HighVol: 0.68 → 0.55` (coherencia +0.05)

**Justificación:** Análisis Waterfall mostró que señales generadas tienen Confidence ≈ 0.50-0.60 (bin 5-6), pero threshold 0.65 bloqueaba el 100% de las señales (PassedThreshold=0).

---

### 5.3 BIAS Y ALINEACIÓN

| Parámetro | Valor Actual | Valor Default | Descripción | Estado |
|-----------|--------------|---------------|-------------|--------|
| `EnableQuickBiasPenalty` | true | true | Penalizar contra-bias | ✅ Original |
| `QuickBiasPenaltyFactor` | 0.90 | 0.90 | Factor penalización (×0.90) | ✅ Original |
| `QuickBiasMomentumBullThreshold` | 0.60 | 0.60 | Umbral momentum alcista | ✅ Original |
| `QuickBiasMomentumBearThreshold` | 0.40 | 0.40 | Umbral momentum bajista | ✅ Original |
| `CounterBiasMinRR` | 2.60 | 2.60 | R:R mínimo para contra-bias | ✅ Original |

---

### 5.4 CONFLUENCE

| Parámetro | Valor Actual | Valor Default | Descripción | Estado |
|-----------|--------------|---------------|-------------|--------|
| `MinConfluenceGating` | 0.0 | 0.0 | Mín. confluencia para considerar entrada | ✅ Original |
| `MinConfluenceForAligned` | 0.0 | 0.0 | Mín. confluencia aligned | ✅ Original |
| `MinConfluenceForCounter` | 0.0 | 0.0 | Mín. confluencia counter | ✅ Original |

**Nota:** Confluence actualmente desactivada en DFM (`Weight_Confluence = 0.00`).

---

## 6. GESTIÓN DE RIESGO (SL/TP)

### 6.1 LÍMITES POR PUNTOS

| Parámetro | Valor Actual | Valor Histórico | Descripción | Estado |
|-----------|--------------|-----------------|-------------|--------|
| `MaxSLDistancePoints_Normal` | 25.0 | 25.0 | Máx. SL Normal (puntos) | ✅ Original |
| `MaxSLDistancePoints_HighVol` | 35.0 | 35.0 | Máx. SL HighVol (puntos) | ✅ Original |
| `MaxTPDistancePoints_Normal` | 25.0 | 25.0 | Máx. TP Normal (puntos) | ✅ Original |
| `MaxTPDistancePoints_HighVol` | 35.0 | 35.0 | Máx. TP HighVol (puntos) | ✅ Original |

---

### 6.2 LÍMITES POR ATR

| Parámetro | Valor Actual | Valor Histórico | Descripción | Estado |
|-----------|--------------|-----------------|-------------|--------|
| `MaxSLDistanceATR` | 4.0 | 15.0 → 8.0 → **4.0** | Máx. SL (× ATR) | 🔧 Modificado V6.0k |
| `MaxSLDistanceATR_HighVol` | 10.0 | 10.0 | Máx. SL HighVol (× ATR) | ✅ Original |
| `MaxTPDistanceATR` | 5.0 | 4.0 → 3.5 → **5.0** | Máx. TP (× ATR) | 🔧 Modificado V6.0k |
| `MaxTPDistanceATR_HighVol` | 6.0 | 20.0 → 8.0 → **6.0** | Máx. TP HighVol (× ATR) | 🔧 Modificado V6.0k |

**Cambios V6.0k:** Basados en datos reales de backtests (P90/P95).

---

### 6.3 TP PRIORITARIO (P3 - INTRADÍA)

| Parámetro | Valor Actual | Valor Default | Descripción | Estado |
|-----------|--------------|---------------|-------------|--------|
| `MinTPDistanceATR_P3` | 0.8 | 0.8 | Mín. TP P3 (× ATR) | ✅ Original |
| `MaxTPDistanceATR_P3` | 3.5 | 3.5 | Máx. TP P3 (× ATR) | ✅ Original |

**Nota:** P3 es TP prioritario intradía (zonas estructurales cercanas con buen R:R).

---

### 6.4 RISK:REWARD

| Parámetro | Valor Actual | Valor Histórico | Descripción | Estado |
|-----------|--------------|-----------------|-------------|--------|
| `SafetyValve_MinRR` | 1.75 | 1.5 → **1.75** | R:R mínimo global | 🔧 Modificado V6.0k |
| `MinRR_HighVol` | 1.0 | 1.0 | R:R mínimo HighVol | ✅ Original |
| `MaxRR_HighVol` | 1.6 | 1.6 | R:R máximo HighVol | ✅ Original |

**Cambio V6.0k:** `SafetyValve_MinRR = 1.75` basado en fórmula `(1-WR)/WR` con WR≈0%.

---

### 6.5 EDAD Y CANCELACIÓN

| Parámetro | Valor Actual | Valor Default | Descripción | Estado |
|-----------|--------------|---------------|-------------|--------|
| `MaxEntryAgeHours` | 36.0 | 168.0 → **36.0** | Edad máx. entry para intradía (h) | 🔧 Modificado |
| `StructuralInvalidationGraceBars` | 20 | 20 | Gracia antes de cancelar por invalidación | ✅ Original |
| `MaxDistanceToEntry_ATR_Cancel` | 999.0 | 2.0 → **999.0** | Gate distancia cancelación (diagnóstico) | 🔧 Modificado (diagnóstico) |
| `MaxDistanceToEntry_ATR_Cancel_HighVol` | 999.0 | 3.0 → **999.0** | Gate distancia HighVol (diagnóstico) | 🔧 Modificado (diagnóstico) |

**Nota:** Cancelación por distancia desactivada (999.0) para diagnóstico.

---

## 7. FILTROS DE ENTRADA

| Parámetro | Valor Actual | Valor Histórico | Descripción | Estado |
|-----------|--------------|-----------------|-------------|--------|
| `MaxDistanceToRegister_ATR_Normal` | 2.0 | 0.8 → **2.0** | Máx. distancia registro Normal (× ATR) | 🔧 Modificado |
| `MaxDistanceToRegister_ATR_HighVol` | 3.0 | 4.0 → **3.0** | Máx. distancia registro HighVol (× ATR) | 🔧 Modificado |
| `MaxDistanceToEntry_Points_HighVol` | 65.0 | 65.0 | Máx. distancia entry HighVol (puntos) | ✅ Original |

**Cambios:** Filtros de proximidad ajustados para intradía (2.0/3.0 ATR es razonable).

---

## 8. GESTIÓN DE OPERACIONES

| Parámetro | Valor Actual | Valor Histórico | Descripción | Estado |
|-----------|--------------|-----------------|-------------|--------|
| `MaxConcurrentTrades` | 1 | 20 → **1** | Máx. operaciones simultáneas | 🔧 Modificado |
| `MinBarsBetweenSameSignal` | 12 | 0 → **12** | Cooldown duplicados (barras) | 🔧 Modificado |
| `TradeCooldownBars` | 0 | 25 → **0** | Cooldown global operaciones | 🔧 Modificado (diagnóstico) |
| `MaxBarsToFillEntry` | 24 | 9999 → **24** | Máx. barras para fill | 🔧 Modificado |

**Cambios:**
- `MaxConcurrentTrades = 1`: Sin lógica de promedio, solo 1 operación a la vez.
- `MinBarsBetweenSameSignal = 12`: Prevenir duplicados (mismo Entry/SL/TP en barras consecutivas).
- `TradeCooldownBars = 0`: Desactivado para diagnóstico.

---

## 9. PURGA Y MEMORIA

### 9.1 PURGA AUTOMÁTICA

| Parámetro | Valor Actual | Valor Default | Descripción | Estado |
|-----------|--------------|---------------|-------------|--------|
| `EnableAutoPurge` | true | true | Purga automática activa | ✅ Original |
| `PurgeEveryNBars` | 25 | 25 | Frecuencia purga (barras) | ✅ Original |
| `MinScoreThreshold` | 0.08 | 0.15 → **0.08** | Score mínimo para no purgar | 🔧 Modificado (diagnóstico) |
| `MinScoreToKeep` | 0.20 | 0.20 | Score mínimo para retener | ✅ Original |

---

### 9.2 EDAD MÁXIMA POR TF

| TF (min) | Valor Actual (min) | Valor Actual (barras) | Descripción | Estado |
|----------|--------------------|-----------------------|-------------|--------|
| 5m | 2880 | 576 barras | 2 días | ✅ Original |
| 15m | 4320 | 288 barras | 3 días | ✅ Original |
| 60m | 14400 | 240 barras | 10 días | ✅ Original |
| 240m | 28800 | 120 barras | 20 días | ✅ Original |
| 1440m | 43200 | 30 barras | 30 días | ✅ Original |

**Nota:** Purga adaptativa por TF (TF más rápidos expiran antes).

---

### 9.3 LÍMITES POR TIPO DE ESTRUCTURA

| Tipo | Valor Actual | Valor Default | Descripción | Estado |
|------|--------------|---------------|-------------|--------|
| FVG | 100 | 100 | Máx. FVGs por TF | ✅ Original |
| OB | 80 | 80 | Máx. OBs por TF | ✅ Original |
| Swing | 150 | 150 | Máx. Swings por TF | ✅ Original |
| BOS | 50 | 50 | Máx. BOS por TF | ✅ Original |
| POI | 60 | 60 | Máx. POIs por TF | ✅ Original |
| LV | 40 | 40 | Máx. LVs por TF | ✅ Original |
| LG | 30 | 30 | Máx. LGs por TF | ✅ Original |
| Double | 40 | 40 | Máx. Doubles por TF | ✅ Original |

---

## 10. SISTEMA Y DEBUG

| Parámetro | Valor Actual | Valor Default | Descripción | Estado |
|-----------|--------------|---------------|-------------|--------|
| `EnableDebug` | false | false | Logs de debug | ✅ Original |
| `EnablePerfDiagnostics` | false | false | Diagnóstico rendimiento | ✅ Original |
| `DiagnosticsInterval` | 100 | 100 | Intervalo diagnóstico (barras) | ✅ Original |
| `ShowScoringBreakdown` | true | true | Mostrar breakdown scoring | ✅ Original |
| `EnableOHLCLogging` | true | false → **true** | Logs OHLC para MFE/MAE | 🔧 Activado |
| `LoggingThresholdBars` | 100 | 10000 → **100** | Umbral barras para logs detallados | 🔧 Modificado |
| `RiskDetailSamplingRate` | 20 | 20 | Sampling rate logs Risk | ✅ Original |
| `EnableHistoricalProcessing` | true | true | Procesar histórico | ✅ Original |
| `EnableFastLoadFromJSON` | false | false | Carga rápida JSON (no-determinista) | ✅ Original |

**Cambios:**
- `EnableOHLCLogging = true`: Activado para análisis MFE/MAE.
- `LoggingThresholdBars = 100`: Reducido para logs detallados en últimas barras.

---

## 11. PERSISTENCIA (NO USADO EN BACKTEST)

| Parámetro | Valor Actual | Descripción | Estado |
|-----------|--------------|-------------|--------|
| `AutoSaveEnabled` | false | Guardado automático estado | ✅ Desactivado (backtest) |
| `StateSaveIntervalSecs` | 600 | Intervalo guardado (seg) | ✅ N/A |
| `ValidateConfigHashOnLoad` | true | Validar hash al cargar | ✅ N/A |

**Nota:** Persistencia desactivada en backtest para garantizar determinismo.

---

## 📝 HISTORIAL DE CAMBIOS

### 2025-11-11 13:30 - EXPLORACIÓN THRESHOLD CONFIANZA

**Cambios:**
```csharp
MinConfidenceForEntry: 0.65 → 0.50  (-23%)
MinConfidenceForEntry_HighVol: 0.68 → 0.55  (-19%)
```

**Justificación:**
- Análisis Waterfall mostró que DFM genera señales con Confidence ≈ 0.50-0.60 (bin 5-6)
- Threshold 0.65 bloqueaba el 100% de las señales (PassedThreshold=0)
- Objetivo: Explorar espectro completo de señales y calibrar threshold científicamente

**Impacto esperado:**
- DFM_Passed: 0 → 100-200 señales
- Operaciones registradas: 14 (históricas) → 20-50
- Operaciones ejecutadas: 10-30

**Estado:** ⏳ Pendiente de test

---

### 2025-11-10 - OPTIMIZACIÓN DFM V6.0k

**Cambios:**
```csharp
Weight_Bias: 0.30 → 0.40  (+33%)
Weight_Momentum: 0.00 → 0.20  (+∞)
Weight_Proximity: 0.30 → 0.15  (-50%)
Weight_Confluence: 0.15 → 0.00  (-100%)
SafetyValve_MinRR: 1.5 → 1.75  (+17%)
MaxSLDistanceATR: 8.0 → 4.0  (-50%)
MaxTPDistanceATR: 3.5 → 5.0  (+43%)
MaxTPDistanceATR_HighVol: 8.0 → 6.0  (-25%)
```

**Justificación:** Basado en análisis MFE/MAE y datos reales de P90/P95.

**Estado:** ✅ Implementado

---

### 2025-11-09 - CORRECCIÓN BUG SCORING HISTÓRICO

**Cambios:**
- Corregido uso de `currentBarIndex` en lugar de `_provider.GetCurrentBarIndex()` en:
  - `CoreEngine.AddStructure`
  - `CoreEngine.UpdateStructure`
  - `CoreEngine.PurgeOldStructuresIfNeeded`
  - `CoreEngine.UpdateProximityScores`

**Impacto:** Estructuras ya no se purgan prematuramente, permitiendo procesamiento histórico correcto.

**Estado:** ✅ Implementado

---

## 🎯 PARÁMETROS CRÍTICOS PARA AJUSTE

### PRIORIDAD ALTA (Impacto directo en WR/PF)

1. **`MinConfidenceForEntry`** (actual: 0.50)
   - Controla cuántas señales pasan el DFM
   - Ajustar basándose en WR real después de exploración

2. **`Weight_Bias`** (actual: 0.40)
   - Filtra operaciones contra-tendencia
   - Aumentar si WR contra-bias es muy bajo

3. **`Weight_Momentum`** (actual: 0.20)
   - Prioriza rupturas de estructura (BOS/CHoCH)
   - Aumentar si operaciones en momento de ruptura tienen mejor WR

4. **`SafetyValve_MinRR`** (actual: 1.75)
   - Fórmula: `(1-WR) / WR`
   - Ajustar si WR cambia significativamente

5. **`MaxSLDistanceATR`** / **`MaxTPDistanceATR`** (actual: 4.0 / 5.0)
   - Controla tamaño de operaciones
   - Basarse en análisis P90/P95 de datos reales

---

### PRIORIDAD MEDIA (Impacto en calidad de señales)

6. **`HeatZone_MinScore`** (actual: 0.12)
   - Controla cuántas zonas se crean
   - Subir si hay demasiadas zonas de baja calidad

7. **`MinSwingATRfactor`** (actual: 0.15)
   - Controla cantidad de Swings detectados
   - Afecta directamente a SL estructural

8. **`MaxDistanceToRegister_ATR_Normal/HighVol`** (actual: 2.0 / 3.0)
   - Filtra zonas por distancia
   - Ajustar basándose en análisis Waterfall

---

### PRIORIDAD BAJA (Ajuste fino)

9. **`MinFVGSizeATRfactor`** (actual: 0.20)
10. **`Weight_Proximity`** (actual: 0.15)
11. **`MinBarsBetweenSameSignal`** (actual: 12)

---

## 📊 MÉTRICAS DE VALIDACIÓN

Para cada cambio de parámetro, registrar:

| Métrica | Objetivo | Cómo Medir |
|---------|----------|------------|
| **Win Rate** | >60% | Informe logica-operaciones |
| **Profit Factor** | >1.5 | Informe logica-operaciones |
| **R:R Promedio** | ≥1.75 | Informe logica-operaciones |
| **MFE/MAE Ratio** | >1.0 | Informe logica-operaciones (sección 3.5) |
| **TP_FIRST %** | >60% | Informe logica-operaciones (MFE/MAE) |
| **Operaciones/día** | 2-3 | Informe logica-operaciones |
| **Waterfall Conversion** | >1% | Informe logica-operaciones (sección 3.1) |
| **DFM_Passed** | >0 | Informe logica-operaciones (Waterfall) |

---

## 🔬 PROTOCOLO DE EXPERIMENTACIÓN

1. **Cambiar solo 1 parámetro a la vez**
2. **Ejecutar backtest completo**
3. **Regenerar 3 informes:**
   - `ANALISIS_LOGICA_DE_OPERACIONES.md`
   - `ANALISIS_DFM.md`
   - `DIAGNOSTICO_LOGS.md`
4. **Comparar métricas vs. baseline**
5. **Documentar cambio y resultado en este archivo**
6. **Si mejora → commitear; si empeora → revertir**

---

*Documento generado: 2025-11-11 13:30*  
*Última actualización: 2025-11-11 13:30*  
*Versión: 1.0*


