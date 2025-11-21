# Informe Diagnóstico de Logs - 2025-11-07 13:15:11

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251107_130646.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251107_130646.csv`

## DFM
- Eventos de evaluación: 183
- Evaluaciones Bull: 271 | Bear: 0
- Pasaron umbral (PassedThreshold): 176
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:1, 4:21, 5:47, 6:94, 7:43, 8:65, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 1186
- KeptAligned: 96/269 | KeptCounter: 482/2876
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.011 | AvgProxCounter≈ 0.088
  - AvgDistATRAligned≈ 0.21 | AvgDistATRCounter≈ 0.39
- PreferAligned eventos: 38 | Filtradas contra-bias: 0

### Proximity (Pre-PreferAligned)
- Eventos: 1186
- Aligned pre: 96/578 | Counter pre: 482/578
- AvgProxAligned(pre)≈ 0.011 | AvgDistATRAligned(pre)≈ 0.21

### Proximity Drivers
- Eventos: 1186
- Alineadas: n=96 | BaseProx≈ 0.361 | ZoneATR≈ 5.58 | SizePenalty≈ 0.971 | FinalProx≈ 0.351
- Contra-bias: n=482 | BaseProx≈ 0.556 | ZoneATR≈ 5.57 | SizePenalty≈ 0.972 | FinalProx≈ 0.540

## Risk
- Eventos: 237
- Accepted=271 | RejSL=0 | RejTP=81 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 264 (48.1%)
- **P4_FALLBACK:** 285 (51.9%)
- **FORCED_P3 por TF:**
  - TF5: 4 (1.5%)
  - TF60: 71 (26.9%)
  - TF240: 189 (71.6%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_SWING_LITE:** 29 (5.0% del total)
  - Avg Score: 0.76 | Avg R:R: 2.49 | Avg DistATR: 6.55
  - Por TF: TF60=29


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 86 | Unmatched: 185
- 0-10: Wins=71 Losses=15 WR=82.6% (n=86)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=71 Losses=15 WR=82.6% (n=86)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 271 | Aligned=49 (18.1%)
- Core≈ 1.00 | Prox≈ 0.58 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.53 | Confidence≈ 0.00
- SL_TF dist: {'-1': 271} | SL_Structural≈ 0.0%
- TP_TF dist: {'240': 99, '60': 97, '-1': 71, '5': 4} | TP_Structural≈ 73.8%

### SLPick por Bandas y TF
- Bandas: lt8=249, 8-10=22, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=0, 15m=0, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.53 (n=271), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 150441 | Zonas con Anchors: 150441
- Dir zonas (zona): Bull=150441 Bear=0 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 0.1, WithAnchors≈ 0.1, DirBull≈ 0.1, DirBear≈ 0.0, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 150441}
- TF Triggers: {'5': 3145}
- TF Anchors: {'60': 3145, '240': 3145, '1440': 2722}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)

## CSV de Trades
- Filas: 16 | Ejecutadas: 6 | Canceladas: 0 | Expiradas: 0
- BUY: 22 | SELL: 0

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 176
- Registered: 8
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 4
- Intentos de registro: 12

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 6.8%
- RegRate = Registered / Intentos = 66.7%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 33.3%
- ExecRate = Ejecutadas / Registered = 75.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 1 | Total candidatos: 24 | Seleccionados: 1
- Candidatos por zona (promedio): 24.0
- **Edad (barras)** - Candidatos: med=76, max=312 | Seleccionados: med=76, max=76
- **Score** - Candidatos: avg=0.48 | Seleccionados: avg=0.28
- **TF Candidatos**: {60: 12, 5: 7, 240: 5}
- **TF Seleccionados**: {60: 1}
- **DistATR** - Candidatos: avg=8.2 | Seleccionados: avg=11.8
- **Razones de selección**: {'InBand[8,15]_TFPreference': 1}
- **En banda [10,15] ATR**: 12/24 (50.0%)

### Take Profit (TP)
- Zonas analizadas: 549 | Total candidatos: 8147 | Seleccionados: 549
- Candidatos por zona (promedio): 14.8
- **Edad (barras)** - Candidatos: med=726, max=40567 | Seleccionados: med=0, max=38190
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.29
- **Priority Candidatos**: {'P3': 8147}
- **Priority Seleccionados**: {'P4_Fallback': 285, 'P3': 264}
- **Type Candidatos**: {'Swing': 8147}
- **Type Seleccionados**: {'Calculated': 285, 'Swing': 264}
- **TF Candidatos**: {240: 3670, 5: 2159, 60: 1769, 1440: 549}
- **TF Seleccionados**: {-1: 285, 240: 189, 60: 71, 5: 4}
- **DistATR** - Candidatos: avg=7.4 | Seleccionados: avg=7.9
- **RR** - Candidatos: avg=1.69 | Seleccionados: avg=1.30
- **Razones de selección**: {'NoStructuralTarget': 285, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 122, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 28, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 19, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 28, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 19, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 14, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 19, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 3, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 4, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 1}

### 🎯 Recomendaciones
- ⚠️ SL: Estructuras muy antiguas (max 38190 barras). Considerar filtro de edad máxima.
- ⚠️ TP: Estructuras muy antiguas (max 38190 barras). Considerar filtro de edad máxima.
- ⚠️ SL: 100% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 52% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.36.