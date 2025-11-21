# Informe Diagnóstico de Logs - 2025-11-05 21:17:03

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251105_210745.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251105_210745.csv`

## DFM
- Eventos de evaluación: 1424
- Evaluaciones Bull: 739 | Bear: 1526
- Pasaron umbral (PassedThreshold): 1722
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:8, 4:63, 5:291, 6:459, 7:721, 8:718, 9:5

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3964
- KeptAligned: 1547/1673 | KeptCounter: 7587/11481
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.139 | AvgProxCounter≈ 0.416
  - AvgDistATRAligned≈ 0.76 | AvgDistATRCounter≈ 1.82
- PreferAligned eventos: 859 | Filtradas contra-bias: 1021

### Proximity (Pre-PreferAligned)
- Eventos: 3964
- Aligned pre: 1547/9134 | Counter pre: 7587/9134
- AvgProxAligned(pre)≈ 0.139 | AvgDistATRAligned(pre)≈ 0.76

### Proximity Drivers
- Eventos: 3964
- Alineadas: n=1547 | BaseProx≈ 0.662 | ZoneATR≈ 4.91 | SizePenalty≈ 0.980 | FinalProx≈ 0.651
- Contra-bias: n=6566 | BaseProx≈ 0.563 | ZoneATR≈ 4.60 | SizePenalty≈ 0.984 | FinalProx≈ 0.555

## Risk
- Eventos: 3436
- Accepted=2274 | RejSL=0 | RejTP=572 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 1025 (15.4%)
- **P4_FALLBACK:** 5633 (84.6%)
- **FORCED_P3 por TF:**
  - TF5: 56 (5.5%)
  - TF15: 2 (0.2%)
  - TF60: 47 (4.6%)
  - TF240: 644 (62.8%)
  - TF1440: 276 (26.9%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 103 (1.3% del total)
  - Avg Score: 0.61 | Avg R:R: 1.52 | Avg DistATR: 10.25
  - Por TF: TF5=80, TF15=23
- **P0_SWING_LITE:** 928 (12.1% del total)
  - Avg Score: 0.30 | Avg R:R: 2.06 | Avg DistATR: 10.39
  - Por TF: TF15=451, TF60=477


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 59 | Unmatched: 2215
- 0-10: Wins=27 Losses=30 WR=47.4% (n=57)
- 10-15: Wins=2 Losses=0 WR=100.0% (n=2)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=29 Losses=30 WR=49.2% (n=59)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2274 | Aligned=394 (17.3%)
- Core≈ 1.00 | Prox≈ 0.64 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.38 | Confidence≈ 0.00
- SL_TF dist: {'-1': 121, '60': 1432, '5': 68, '15': 125, '240': 400, '1440': 128} | SL_Structural≈ 94.7%
- TP_TF dist: {'-1': 1073, '15': 301, '60': 455, '5': 98, '240': 347} | TP_Structural≈ 52.8%

### SLPick por Bandas y TF
- Bandas: lt8=2139, 8-10=93, 10-12.5=30, 12.5-15=6, >15=6
- TF: 5m=68, 15m=125, 60m=1432, 240m=400, 1440m=128
- RR plan por bandas: 0-10≈ 1.38 (n=2232), 10-15≈ 1.61 (n=36)

## CancelBias (EMA200@60m)
- Eventos: 120
- Distribución Bias: {'Bullish': 56, 'Bearish': 64, 'Neutral': 0}
- Coherencia (Close>EMA): 56/120 (46.7%)

## StructureFusion
- Trazas por zona: 13154 | Zonas con Anchors: 13117
- Dir zonas (zona): Bull=7041 Bear=5530 Neutral=583
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.4, DirBear≈ 1.1, DirNeutral≈ 0.1
- Razones de dirección: {'triggers-only': 36, 'tie-bias': 583, 'anchors+triggers': 12535}
- TF Triggers: {'15': 4726, '5': 8428}
- TF Anchors: {'60': 13013, '240': 12726, '1440': 10318}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 19, 'score decayó a 0,19': 1, 'score decayó a 0,31': 2, 'score decayó a 0,44': 1}
- Cancel_BOS (diag): por acción {'BUY': 8, 'SELL': 10} | por bias {'Bullish': 10, 'Bearish': 8, 'Neutral': 0}

## CSV de Trades
- Filas: 160 | Ejecutadas: 26 | Canceladas: 0 | Expiradas: 0
- BUY: 93 | SELL: 93

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1722
- Registered: 80
  - DEDUP_COOLDOWN: 9 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 89

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 5.2%
- RegRate = Registered / Intentos = 89.9%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 10.1%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 32.5%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7516 | Total candidatos: 182492 | Seleccionados: 7477
- Candidatos por zona (promedio): 24.3
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=38, max=150
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.46
- **TF Candidatos**: {240: 60976, 60: 55453, 15: 31565, 1440: 17648, 5: 16850}
- **TF Seleccionados**: {60: 4957, 5: 181, 15: 621, 1440: 914, 240: 804}
- **DistATR** - Candidatos: avg=20.0 | Seleccionados: avg=9.5
- **Razones de selección**: {'Fallback<15': 1872, 'InBand[8,15]_TFPreference': 5605}
- **En banda [10,15] ATR**: 29924/182492 (16.4%)

### Take Profit (TP)
- Zonas analizadas: 6658 | Total candidatos: 85708 | Seleccionados: 6658
- Candidatos por zona (promedio): 12.9
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=122
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.10
- **Priority Candidatos**: {'P3': 85708}
- **Priority Seleccionados**: {'P4_Fallback': 5633, 'P3': 1025}
- **Type Candidatos**: {'Swing': 85708}
- **Type Seleccionados**: {'Calculated': 5633, 'Swing': 1025}
- **TF Candidatos**: {240: 20267, 5: 17987, 15: 16576, 60: 16398, 1440: 14480}
- **TF Seleccionados**: {-1: 5633, 60: 47, 5: 56, 240: 644, 1440: 276, 15: 2}
- **DistATR** - Candidatos: avg=12.1 | Seleccionados: avg=12.6
- **RR** - Candidatos: avg=1.08 | Seleccionados: avg=1.05
- **Razones de selección**: {'NoStructuralTarget': 5633, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 227, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 170, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 12, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 99, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 63, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 73, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 16, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 195, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 20, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 23, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 38, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 7, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 37, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 3, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 62% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 85% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.92.