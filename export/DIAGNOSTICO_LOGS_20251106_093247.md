# Informe Diagnóstico de Logs - 2025-11-06 09:39:29

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251106_093247.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251106_093247.csv`

## DFM
- Eventos de evaluación: 1298
- Evaluaciones Bull: 736 | Bear: 1185
- Pasaron umbral (PassedThreshold): 1404
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:10, 4:52, 5:277, 6:449, 7:534, 8:594, 9:5

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3990
- KeptAligned: 1503/1633 | KeptCounter: 7671/11590
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.137 | AvgProxCounter≈ 0.415
  - AvgDistATRAligned≈ 0.79 | AvgDistATRCounter≈ 1.83
- PreferAligned eventos: 866 | Filtradas contra-bias: 1044

### Proximity (Pre-PreferAligned)
- Eventos: 3990
- Aligned pre: 1503/9174 | Counter pre: 7671/9174
- AvgProxAligned(pre)≈ 0.137 | AvgDistATRAligned(pre)≈ 0.79

### Proximity Drivers
- Eventos: 3990
- Alineadas: n=1503 | BaseProx≈ 0.643 | ZoneATR≈ 5.04 | SizePenalty≈ 0.979 | FinalProx≈ 0.631
- Contra-bias: n=6627 | BaseProx≈ 0.560 | ZoneATR≈ 4.61 | SizePenalty≈ 0.984 | FinalProx≈ 0.552

## Risk
- Eventos: 3461
- Accepted=1927 | RejSL=0 | RejTP=589 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 1066 (16.1%)
- **P4_FALLBACK:** 5573 (83.9%)
- **FORCED_P3 por TF:**
  - TF5: 59 (5.5%)
  - TF15: 2 (0.2%)
  - TF60: 48 (4.5%)
  - TF240: 654 (61.4%)
  - TF1440: 303 (28.4%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 103 (1.3% del total)
  - Avg Score: 0.61 | Avg R:R: 1.52 | Avg DistATR: 10.07
  - Por TF: TF5=79, TF15=24
- **P0_SWING_LITE:** 949 (12.3% del total)
  - Avg Score: 0.30 | Avg R:R: 2.05 | Avg DistATR: 10.34
  - Por TF: TF15=452, TF60=497


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 40 | Unmatched: 1887
- 0-10: Wins=14 Losses=23 WR=37.8% (n=37)
- 10-15: Wins=2 Losses=1 WR=66.7% (n=3)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=16 Losses=24 WR=40.0% (n=40)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1927 | Aligned=244 (12.7%)
- Core≈ 1.00 | Prox≈ 0.61 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.44 | Confidence≈ 0.00
- SL_TF dist: {'60': 1287, '-1': 112, '5': 56, '15': 106, '240': 246, '1440': 120} | SL_Structural≈ 94.2%
- TP_TF dist: {'-1': 731, '5': 103, '15': 273, '60': 472, '240': 348} | TP_Structural≈ 62.1%

### SLPick por Bandas y TF
- Bandas: lt8=1822, 8-10=81, 10-12.5=20, 12.5-15=4, >15=0
- TF: 5m=56, 15m=106, 60m=1287, 240m=246, 1440m=120
- RR plan por bandas: 0-10≈ 1.44 (n=1903), 10-15≈ 1.25 (n=24)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 13223 | Zonas con Anchors: 13198
- Dir zonas (zona): Bull=7089 Bear=5530 Neutral=604
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.4, DirBear≈ 1.1, DirNeutral≈ 0.1
- Razones de dirección: {'tie-bias': 604, 'anchors+triggers': 12597, 'triggers-only': 22}
- TF Triggers: {'5': 8478, '15': 4745}
- TF Anchors: {'60': 13093, '240': 12918, '1440': 10422}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 18, 'score decayó a 0,25': 1}

## CSV de Trades
- Filas: 233 | Ejecutadas: 22 | Canceladas: 0 | Expiradas: 0
- BUY: 184 | SELL: 71

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1404
- Registered: 117
  - DEDUP_COOLDOWN: 28 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 145

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 10.3%
- RegRate = Registered / Intentos = 80.7%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 19.3%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 18.8%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7511 | Total candidatos: 183543 | Seleccionados: 7473
- Candidatos por zona (promedio): 24.4
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 61409, 60: 55558, 15: 31735, 1440: 17702, 5: 17139}
- **TF Seleccionados**: {60: 4889, 5: 220, 15: 637, 1440: 912, 240: 815}
- **DistATR** - Candidatos: avg=20.0 | Seleccionados: avg=9.6
- **Razones de selección**: {'Fallback<15': 1836, 'InBand[8,15]_TFPreference': 5637}
- **En banda [10,15] ATR**: 30203/183543 (16.5%)

### Take Profit (TP)
- Zonas analizadas: 6639 | Total candidatos: 86389 | Seleccionados: 6639
- Candidatos por zona (promedio): 13.0
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=122
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.11
- **Priority Candidatos**: {'P3': 86389}
- **Priority Seleccionados**: {'P4_Fallback': 5573, 'P3': 1066}
- **Type Candidatos**: {'Swing': 86389}
- **Type Seleccionados**: {'Calculated': 5573, 'Swing': 1066}
- **TF Candidatos**: {240: 20624, 5: 17573, 15: 16618, 60: 16606, 1440: 14968}
- **TF Seleccionados**: {-1: 5573, 60: 48, 240: 654, 5: 59, 1440: 303, 15: 2}
- **DistATR** - Candidatos: avg=12.5 | Seleccionados: avg=12.8
- **RR** - Candidatos: avg=1.10 | Seleccionados: avg=1.05
- **Razones de selección**: {'NoStructuralTarget': 5573, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 174, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 201, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 22, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 13, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 232, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 68, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 75, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 16, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 103, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 23, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 38, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 7, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 49, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 3, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 63% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 84% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.92.