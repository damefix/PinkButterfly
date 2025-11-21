# Informe Diagnóstico de Logs - 2025-11-06 20:27:59

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251106_202130.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251106_202130.csv`

## DFM
- Eventos de evaluación: 1284
- Evaluaciones Bull: 724 | Bear: 1154
- Pasaron umbral (PassedThreshold): 1361
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:9, 4:54, 5:278, 6:438, 7:518, 8:576, 9:5

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 4004
- KeptAligned: 1523/1657 | KeptCounter: 7697/11663
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.140 | AvgProxCounter≈ 0.415
  - AvgDistATRAligned≈ 0.80 | AvgDistATRCounter≈ 1.82
- PreferAligned eventos: 884 | Filtradas contra-bias: 1086

### Proximity (Pre-PreferAligned)
- Eventos: 4004
- Aligned pre: 1523/9220 | Counter pre: 7697/9220
- AvgProxAligned(pre)≈ 0.140 | AvgDistATRAligned(pre)≈ 0.80

### Proximity Drivers
- Eventos: 4004
- Alineadas: n=1523 | BaseProx≈ 0.645 | ZoneATR≈ 4.99 | SizePenalty≈ 0.979 | FinalProx≈ 0.634
- Contra-bias: n=6611 | BaseProx≈ 0.562 | ZoneATR≈ 4.60 | SizePenalty≈ 0.984 | FinalProx≈ 0.554

## Risk
- Eventos: 3467
- Accepted=1887 | RejSL=0 | RejTP=608 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 1099 (16.5%)
- **P4_FALLBACK:** 5558 (83.5%)
- **FORCED_P3 por TF:**
  - TF5: 39 (3.5%)
  - TF15: 24 (2.2%)
  - TF60: 64 (5.8%)
  - TF240: 655 (59.6%)
  - TF1440: 317 (28.8%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 100 (1.3% del total)
  - Avg Score: 0.61 | Avg R:R: 1.53 | Avg DistATR: 9.75
  - Por TF: TF5=75, TF15=25
- **P0_SWING_LITE:** 928 (12.1% del total)
  - Avg Score: 0.30 | Avg R:R: 2.06 | Avg DistATR: 10.43
  - Por TF: TF15=464, TF60=464


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 58 | Unmatched: 1829
- 0-10: Wins=29 Losses=28 WR=50.9% (n=57)
- 10-15: Wins=1 Losses=0 WR=100.0% (n=1)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=30 Losses=28 WR=51.7% (n=58)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1887 | Aligned=262 (13.9%)
- Core≈ 1.00 | Prox≈ 0.61 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.45 | Confidence≈ 0.00
- SL_TF dist: {'-1': 114, '5': 23, '15': 154, '60': 765, '240': 436, '1440': 395} | SL_Structural≈ 94.0%
- TP_TF dist: {'-1': 687, '5': 88, '15': 315, '60': 455, '240': 342} | TP_Structural≈ 63.6%

### SLPick por Bandas y TF
- Bandas: lt8=1799, 8-10=72, 10-12.5=15, 12.5-15=1, >15=0
- TF: 5m=23, 15m=154, 60m=765, 240m=436, 1440m=395
- RR plan por bandas: 0-10≈ 1.45 (n=1871), 10-15≈ 1.24 (n=16)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 13320 | Zonas con Anchors: 13284
- Dir zonas (zona): Bull=7111 Bear=5589 Neutral=620
- Resumen por ciclo (promedios): TotHZ≈ 2.7, WithAnchors≈ 2.7, DirBull≈ 1.4, DirBear≈ 1.1, DirNeutral≈ 0.1
- Razones de dirección: {'triggers-only': 36, 'anchors+triggers': 12664, 'tie-bias': 620}
- TF Triggers: {'5': 8548, '15': 4772}
- TF Anchors: {'60': 13180, '240': 12895, '1440': 10601}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 20}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 222 | Ejecutadas: 23 | Canceladas: 0 | Expiradas: 0
- BUY: 135 | SELL: 110

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1361
- Registered: 111
  - DEDUP_COOLDOWN: 23 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 134

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 9.8%
- RegRate = Registered / Intentos = 82.8%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 17.2%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 20.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7511 | Total candidatos: 182700 | Seleccionados: 7466
- Candidatos por zona (promedio): 24.3
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=27, max=150
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.54
- **TF Candidatos**: {240: 60774, 60: 55520, 15: 31496, 1440: 17901, 5: 17009}
- **TF Seleccionados**: {5: 91, 15: 759, 60: 3868, 240: 924, 1440: 1824}
- **DistATR** - Candidatos: avg=19.8 | Seleccionados: avg=9.7
- **Razones de selección**: {'Fallback<15': 1785, 'InBand[8,15]_TFPreference': 5681}
- **En banda [10,15] ATR**: 30274/182700 (16.6%)

### Take Profit (TP)
- Zonas analizadas: 6657 | Total candidatos: 88026 | Seleccionados: 6657
- Candidatos por zona (promedio): 13.2
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=122
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.11
- **Priority Candidatos**: {'P3': 88026}
- **Priority Seleccionados**: {'P4_Fallback': 5558, 'P3': 1099}
- **Type Candidatos**: {'Swing': 88026}
- **Type Seleccionados**: {'Calculated': 5558, 'Swing': 1099}
- **TF Candidatos**: {240: 20831, 5: 17760, 60: 16975, 15: 16715, 1440: 15745}
- **TF Seleccionados**: {-1: 5558, 60: 64, 240: 655, 5: 39, 1440: 317, 15: 24}
- **DistATR** - Candidatos: avg=12.6 | Seleccionados: avg=12.8
- **RR** - Candidatos: avg=1.10 | Seleccionados: avg=1.05
- **Razones de selección**: {'NoStructuralTarget': 5558, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 240, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 74, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 67, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 16, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 196, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 13, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 187, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 118, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 22, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 7, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 24, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 39, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 7, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 48, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 3, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 51% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 83% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.92.