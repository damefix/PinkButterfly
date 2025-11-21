# Informe Diagnóstico de Logs - 2025-11-06 19:50:37

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251106_194425.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251106_194425.csv`

## DFM
- Eventos de evaluación: 1290
- Evaluaciones Bull: 723 | Bear: 1163
- Pasaron umbral (PassedThreshold): 1368
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:10, 4:53, 5:280, 6:435, 7:514, 8:589, 9:5

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 4000
- KeptAligned: 1508/1637 | KeptCounter: 7698/11641
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.137 | AvgProxCounter≈ 0.418
  - AvgDistATRAligned≈ 0.80 | AvgDistATRCounter≈ 1.81
- PreferAligned eventos: 874 | Filtradas contra-bias: 1089

### Proximity (Pre-PreferAligned)
- Eventos: 4000
- Aligned pre: 1508/9206 | Counter pre: 7698/9206
- AvgProxAligned(pre)≈ 0.137 | AvgDistATRAligned(pre)≈ 0.80

### Proximity Drivers
- Eventos: 4000
- Alineadas: n=1508 | BaseProx≈ 0.641 | ZoneATR≈ 5.04 | SizePenalty≈ 0.979 | FinalProx≈ 0.629
- Contra-bias: n=6609 | BaseProx≈ 0.563 | ZoneATR≈ 4.61 | SizePenalty≈ 0.984 | FinalProx≈ 0.555

## Risk
- Eventos: 3461
- Accepted=1895 | RejSL=0 | RejTP=604 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 1094 (16.4%)
- **P4_FALLBACK:** 5566 (83.6%)
- **FORCED_P3 por TF:**
  - TF5: 41 (3.7%)
  - TF15: 25 (2.3%)
  - TF60: 61 (5.6%)
  - TF240: 655 (59.9%)
  - TF1440: 312 (28.5%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 99 (1.3% del total)
  - Avg Score: 0.61 | Avg R:R: 1.52 | Avg DistATR: 9.98
  - Por TF: TF5=77, TF15=22
- **P0_SWING_LITE:** 924 (12.0% del total)
  - Avg Score: 0.30 | Avg R:R: 2.06 | Avg DistATR: 10.44
  - Por TF: TF15=459, TF60=465


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 53 | Unmatched: 1842
- 0-10: Wins=27 Losses=25 WR=51.9% (n=52)
- 10-15: Wins=1 Losses=0 WR=100.0% (n=1)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=28 Losses=25 WR=52.8% (n=53)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1895 | Aligned=256 (13.5%)
- Core≈ 1.00 | Prox≈ 0.61 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.45 | Confidence≈ 0.00
- SL_TF dist: {'-1': 114, '15': 154, '60': 777, '5': 21, '240': 434, '1440': 395} | SL_Structural≈ 94.0%
- TP_TF dist: {'-1': 704, '15': 308, '5': 90, '60': 452, '240': 341} | TP_Structural≈ 62.8%

### SLPick por Bandas y TF
- Bandas: lt8=1803, 8-10=75, 10-12.5=16, 12.5-15=1, >15=0
- TF: 5m=21, 15m=154, 60m=777, 240m=434, 1440m=395
- RR plan por bandas: 0-10≈ 1.45 (n=1878), 10-15≈ 1.22 (n=17)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 13278 | Zonas con Anchors: 13246
- Dir zonas (zona): Bull=7064 Bear=5611 Neutral=603
- Resumen por ciclo (promedios): TotHZ≈ 2.7, WithAnchors≈ 2.6, DirBull≈ 1.4, DirBear≈ 1.1, DirNeutral≈ 0.1
- Razones de dirección: {'triggers-only': 31, 'anchors+triggers': 12644, 'tie-bias': 603}
- TF Triggers: {'5': 8526, '15': 4752}
- TF Anchors: {'60': 13142, '240': 12847, '1440': 10559}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 20}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 230 | Ejecutadas: 20 | Canceladas: 0 | Expiradas: 0
- BUY: 127 | SELL: 123

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1368
- Registered: 115
  - DEDUP_COOLDOWN: 39 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 154

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 11.3%
- RegRate = Registered / Intentos = 74.7%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 25.3%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 17.4%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7505 | Total candidatos: 182199 | Seleccionados: 7467
- Candidatos por zona (promedio): 24.3
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=27, max=150
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.54
- **TF Candidatos**: {240: 60528, 60: 55313, 15: 31431, 1440: 17816, 5: 17111}
- **TF Seleccionados**: {15: 754, 60: 3871, 5: 103, 240: 923, 1440: 1816}
- **DistATR** - Candidatos: avg=19.8 | Seleccionados: avg=9.7
- **Razones de selección**: {'Fallback<15': 1803, 'InBand[8,15]_TFPreference': 5664}
- **En banda [10,15] ATR**: 30188/182199 (16.6%)

### Take Profit (TP)
- Zonas analizadas: 6660 | Total candidatos: 87468 | Seleccionados: 6660
- Candidatos por zona (promedio): 13.1
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=122
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.11
- **Priority Candidatos**: {'P3': 87468}
- **Priority Seleccionados**: {'P4_Fallback': 5566, 'P3': 1094}
- **Type Candidatos**: {'Swing': 87468}
- **Type Seleccionados**: {'Calculated': 5566, 'Swing': 1094}
- **TF Candidatos**: {240: 20689, 5: 17636, 60: 16888, 15: 16638, 1440: 15617}
- **TF Seleccionados**: {-1: 5566, 15: 25, 5: 41, 60: 61, 240: 655, 1440: 312}
- **DistATR** - Candidatos: avg=12.6 | Seleccionados: avg=12.8
- **RR** - Candidatos: avg=1.10 | Seleccionados: avg=1.05
- **Razones de selección**: {'NoStructuralTarget': 5566, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 25, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 15, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 236, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 75, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 67, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 14, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 196, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 187, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 117, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 24, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 39, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 7, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 47, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 3, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 51% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 84% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.92.