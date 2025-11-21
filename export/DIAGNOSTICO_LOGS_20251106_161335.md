# Informe Diagnóstico de Logs - 2025-11-06 16:21:57

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251106_161335.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251106_161335.csv`

## DFM
- Eventos de evaluación: 1728
- Evaluaciones Bull: 1169 | Bear: 1502
- Pasaron umbral (PassedThreshold): 1868
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:17, 4:81, 5:420, 6:696, 7:753, 8:697, 9:7

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5381
- KeptAligned: 2134/2323 | KeptCounter: 9842/15547
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.138 | AvgProxCounter≈ 0.391
  - AvgDistATRAligned≈ 0.78 | AvgDistATRCounter≈ 1.73
- PreferAligned eventos: 1169 | Filtradas contra-bias: 1211

### Proximity (Pre-PreferAligned)
- Eventos: 5381
- Aligned pre: 2134/11976 | Counter pre: 9842/11976
- AvgProxAligned(pre)≈ 0.138 | AvgDistATRAligned(pre)≈ 0.78

### Proximity Drivers
- Eventos: 5381
- Alineadas: n=2134 | BaseProx≈ 0.657 | ZoneATR≈ 4.92 | SizePenalty≈ 0.980 | FinalProx≈ 0.645
- Contra-bias: n=8631 | BaseProx≈ 0.559 | ZoneATR≈ 4.58 | SizePenalty≈ 0.984 | FinalProx≈ 0.551

## Risk
- Eventos: 4508
- Accepted=2681 | RejSL=0 | RejTP=935 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 1666 (18.8%)
- **P4_FALLBACK:** 7181 (81.2%)
- **FORCED_P3 por TF:**
  - TF5: 79 (4.7%)
  - TF15: 1 (0.1%)
  - TF60: 203 (12.2%)
  - TF240: 735 (44.1%)
  - TF1440: 648 (38.9%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 215 (2.1% del total)
  - Avg Score: 0.61 | Avg R:R: 1.71 | Avg DistATR: 9.59
  - Por TF: TF5=179, TF15=36
- **P0_SWING_LITE:** 1137 (11.1% del total)
  - Avg Score: 0.30 | Avg R:R: 2.13 | Avg DistATR: 10.01
  - Por TF: TF15=526, TF60=611


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 89 | Unmatched: 2592
- 0-10: Wins=44 Losses=43 WR=50.6% (n=87)
- 10-15: Wins=2 Losses=0 WR=100.0% (n=2)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=46 Losses=43 WR=51.7% (n=89)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2681 | Aligned=481 (17.9%)
- Core≈ 1.00 | Prox≈ 0.59 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.43 | Confidence≈ 0.00
- SL_TF dist: {'-1': 212, '5': 60, '15': 267, '60': 1691, '240': 304, '1440': 147} | SL_Structural≈ 92.1%
- TP_TF dist: {'-1': 1025, '5': 191, '15': 321, '60': 728, '240': 416} | TP_Structural≈ 61.8%

### SLPick por Bandas y TF
- Bandas: lt8=2527, 8-10=119, 10-12.5=23, 12.5-15=12, >15=0
- TF: 5m=60, 15m=267, 60m=1691, 240m=304, 1440m=147
- RR plan por bandas: 0-10≈ 1.43 (n=2646), 10-15≈ 1.33 (n=35)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 17870 | Zonas con Anchors: 17812
- Dir zonas (zona): Bull=9894 Bear=7209 Neutral=767
- Resumen por ciclo (promedios): TotHZ≈ 2.7, WithAnchors≈ 2.7, DirBull≈ 1.5, DirBear≈ 1.1, DirNeutral≈ 0.1
- Razones de dirección: {'triggers-only': 53, 'tie-bias': 767, 'anchors+triggers': 17050}
- TF Triggers: {'5': 11446, '15': 6424}
- TF Anchors: {'60': 17684, '240': 16637, '1440': 13313}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 26, 'score decayó a 0,32': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 334 | Ejecutadas: 36 | Canceladas: 0 | Expiradas: 0
- BUY: 201 | SELL: 169

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1868
- Registered: 167
  - DEDUP_COOLDOWN: 30 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 197

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 10.5%
- RegRate = Registered / Intentos = 84.8%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 15.2%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 21.6%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 9924 | Total candidatos: 233130 | Seleccionados: 9865
- Candidatos por zona (promedio): 23.5
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 75329, 60: 69175, 15: 42134, 5: 23986, 1440: 22506}
- **TF Seleccionados**: {5: 230, 15: 941, 60: 6482, 240: 1087, 1440: 1125}
- **DistATR** - Candidatos: avg=20.2 | Seleccionados: avg=9.6
- **Razones de selección**: {'Fallback<15': 2533, 'InBand[8,15]_TFPreference': 7332}
- **En banda [10,15] ATR**: 37186/233130 (16.0%)

### Take Profit (TP)
- Zonas analizadas: 8847 | Total candidatos: 116641 | Seleccionados: 8847
- Candidatos por zona (promedio): 13.2
- **Edad (barras)** - Candidatos: med=35, max=150 | Seleccionados: med=0, max=122
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.12
- **Priority Candidatos**: {'P3': 116641}
- **Priority Seleccionados**: {'P4_Fallback': 7181, 'P3': 1666}
- **Type Candidatos**: {'Swing': 116641}
- **Type Seleccionados**: {'Calculated': 7181, 'Swing': 1666}
- **TF Candidatos**: {240: 27322, 60: 24765, 5: 22947, 15: 21594, 1440: 20013}
- **TF Seleccionados**: {-1: 7181, 60: 203, 5: 79, 240: 735, 1440: 648, 15: 1}
- **DistATR** - Candidatos: avg=12.9 | Seleccionados: avg=12.8
- **RR** - Candidatos: avg=1.14 | Seleccionados: avg=1.06
- **Razones de selección**: {'NoStructuralTarget': 7181, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 100, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 12, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 7, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 4, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 31, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 8, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 14, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 137, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 239, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 424, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 77, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 76, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 312, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 8, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 44, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 24, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of17': 2, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 42, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 31, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 2, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 17, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 8, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of24': 2, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 15, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 13, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of20': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of18': 7, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 63% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 81% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.92.