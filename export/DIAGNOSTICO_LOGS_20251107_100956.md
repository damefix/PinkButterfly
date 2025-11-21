# Informe Diagnóstico de Logs - 2025-11-07 10:32:48

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251107_100956.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251107_100956.csv`

## DFM
- Eventos de evaluación: 184
- Evaluaciones Bull: 278 | Bear: 0
- Pasaron umbral (PassedThreshold): 181
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:1, 4:22, 5:48, 6:99, 7:43, 8:65, 9:0

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
- Accepted=278 | RejSL=0 | RejTP=74 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 269 (48.6%)
- **P4_FALLBACK:** 285 (51.4%)
- **FORCED_P3 por TF:**
  - TF5: 4 (1.5%)
  - TF60: 81 (30.1%)
  - TF240: 184 (68.4%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_SWING_LITE:** 24 (4.2% del total)
  - Avg Score: 0.76 | Avg R:R: 2.31 | Avg DistATR: 6.53
  - Por TF: TF60=24


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 88 | Unmatched: 190
- 0-10: Wins=66 Losses=22 WR=75.0% (n=88)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=66 Losses=22 WR=75.0% (n=88)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 278 | Aligned=50 (18.0%)
- Core≈ 1.00 | Prox≈ 0.58 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.51 | Confidence≈ 0.00
- SL_TF dist: {'-1': 278} | SL_Structural≈ 0.0%
- TP_TF dist: {'240': 101, '60': 102, '-1': 71, '5': 4} | TP_Structural≈ 74.5%

### SLPick por Bandas y TF
- Bandas: lt8=256, 8-10=22, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=0, 15m=0, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.51 (n=278), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 150435 | Zonas con Anchors: 150435
- Dir zonas (zona): Bull=150435 Bear=0 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 0.1, WithAnchors≈ 0.1, DirBull≈ 0.1, DirBear≈ 0.0, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 150435}
- TF Triggers: {'5': 3145}
- TF Anchors: {'60': 3145, '240': 3145, '1440': 2308}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,30': 3}

## CSV de Trades
- Filas: 44 | Ejecutadas: 6 | Canceladas: 0 | Expiradas: 0
- BUY: 50 | SELL: 0

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 181
- Registered: 22
  - DEDUP_COOLDOWN: 36 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 58

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 32.0%
- RegRate = Registered / Intentos = 37.9%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 62.1%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 27.3%

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
- Zonas analizadas: 554 | Total candidatos: 8292 | Seleccionados: 554
- Candidatos por zona (promedio): 15.0
- **Edad (barras)** - Candidatos: med=740, max=40567 | Seleccionados: med=0, max=38190
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.28
- **Priority Candidatos**: {'P3': 8292}
- **Priority Seleccionados**: {'P4_Fallback': 285, 'P3': 269}
- **Type Candidatos**: {'Swing': 8292}
- **Type Seleccionados**: {'Calculated': 285, 'Swing': 269}
- **TF Candidatos**: {240: 3715, 5: 2199, 60: 1824, 1440: 554}
- **TF Seleccionados**: {-1: 285, 240: 184, 60: 81, 5: 4}
- **DistATR** - Candidatos: avg=7.4 | Seleccionados: avg=7.9
- **RR** - Candidatos: avg=1.67 | Seleccionados: avg=1.29
- **Razones de selección**: {'NoStructuralTarget': 285, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 122, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 38, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 19, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 15, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 19, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 18, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 19, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 3, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 4, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 1}

### 🎯 Recomendaciones
- ⚠️ SL: Estructuras muy antiguas (max 38190 barras). Considerar filtro de edad máxima.
- ⚠️ TP: Estructuras muy antiguas (max 38190 barras). Considerar filtro de edad máxima.
- ⚠️ SL: 100% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 51% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.36.