# Informe Diagnóstico de Logs - 2025-11-05 09:14:31

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251105_091006.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251105_091006.csv`

## DFM
- Eventos de evaluación: 1734
- Evaluaciones Bull: 2982 | Bear: 236
- Pasaron umbral (PassedThreshold): 1897
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:12, 4:107, 5:609, 6:1213, 7:1269, 8:8, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3944
- KeptAligned: 8966/10029 | KeptCounter: 1791/3017
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.512 | AvgProxCounter≈ 0.138
  - AvgDistATRAligned≈ 2.90 | AvgDistATRCounter≈ 0.64
- PreferAligned eventos: 3187 | Filtradas contra-bias: 877

### Proximity (Pre-PreferAligned)
- Eventos: 3944
- Aligned pre: 8966/10757 | Counter pre: 1791/10757
- AvgProxAligned(pre)≈ 0.512 | AvgDistATRAligned(pre)≈ 2.90

### Proximity Drivers
- Eventos: 3944
- Alineadas: n=8966 | BaseProx≈ 0.666 | ZoneATR≈ 4.70 | SizePenalty≈ 0.982 | FinalProx≈ 0.655
- Contra-bias: n=914 | BaseProx≈ 0.554 | ZoneATR≈ 4.60 | SizePenalty≈ 0.983 | FinalProx≈ 0.546

## Risk
- Eventos: 3626
- Accepted=3251 | RejSL=77 | RejTP=2036 | RejRR=0 | RejEntry=0
### Risk – Validación Doble Cerrojo (V6.0d)
- **RejSL_Points:** 4139 (rechazados por >60pts)
- **RejTP_Points:** 36 (rechazados por >120pts)
- **RejSL_HighVol:** 0 (rechazados por ATR>15 y DistATR>10)

**Rechazos SL por TF:**

| TF | RejSL_Points |
|----|--------------|
| -1 | 9 |
| 5 | 15 |
| 15 | 37 |
| 60 | 2382 |
| 240 | 801 |
| 1440 | 895 |

**Rechazos TP por TF:**

| TF | RejTP_Points |
|----|--------------|
| 1440 | 36 |

### TP Policy (V6.0c)
- **FORCED_P3:** 4678 (47.3%)
- **P4_FALLBACK:** 5202 (52.7%)
- **FORCED_P3 por TF:**
  - TF5: 115 (2.5%)
  - TF15: 9 (0.2%)
  - TF60: 124 (2.7%)
  - TF240: 1166 (24.9%)
  - TF1440: 3264 (69.8%)

### Risk Drivers (Rechazos por SL)
- Alineadas: n=77 | SLDistATR≈ 16.62 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:75,20-25:1,25+:1
- HistSL Counter 0-10:0,10-15:0,15-20:0,20-25:0,25+:0

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 68 | Unmatched: 3183
- 0-10: Wins=11 Losses=53 WR=17.2% (n=64)
- 10-15: Wins=3 Losses=1 WR=75.0% (n=4)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=14 Losses=54 WR=20.6% (n=68)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 3251 | Aligned=3191 (98.2%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.10 | Confidence≈ 0.00
- SL_TF dist: {'15': 508, '-1': 23, '5': 128, '240': 243, '60': 2311, '1440': 38} | SL_Structural≈ 99.3%
- TP_TF dist: {'-1': 2791, '5': 98, '240': 254, '60': 86, '1440': 19, '15': 3} | TP_Structural≈ 14.1%

### SLPick por Bandas y TF
- Bandas: lt8=2686, 8-10=321, 10-12.5=147, 12.5-15=97, >15=0
- TF: 5m=128, 15m=508, 60m=2311, 240m=243, 1440m=38
- RR plan por bandas: 0-10≈ 1.11 (n=3007), 10-15≈ 1.01 (n=244)

## CancelBias (EMA200@60m)
- Eventos: 134
- Distribución Bias: {'Bullish': 110, 'Bearish': 24, 'Neutral': 0}
- Coherencia (Close>EMA): 110/134 (82.1%)

## StructureFusion
- Trazas por zona: 13046 | Zonas con Anchors: 12992
- Dir zonas (zona): Bull=7502 Bear=5544 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.5, DirBear≈ 1.1, DirNeutral≈ 0.0
- Razones de dirección: {'triggers-only': 52, 'tie-bias': 565, 'anchors+triggers': 12429}
- TF Triggers: {'15': 4675, '5': 8371}
- TF Anchors: {'240': 12759, '60': 12876, '1440': 10153}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3748, 'Bearish': 1252, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3748/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 15, 'score decayó a 0,33': 1, 'score decayó a 0,32': 1, 'score decayó a 0,27': 1, 'score decayó a 0,26': 1}

## CSV de Trades
- Filas: 84 | Ejecutadas: 19 | Canceladas: 0 | Expiradas: 0
- BUY: 96 | SELL: 7

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1897
- Registered: 42
  - DEDUP_COOLDOWN: 2 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 44

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 2.3%
- RegRate = Registered / Intentos = 95.5%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 4.5%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 45.2%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 9855 | Total candidatos: 250867 | Seleccionados: 9801
- Candidatos por zona (promedio): 25.5
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 85202, 60: 76120, 15: 41819, 1440: 24879, 5: 22847}
- **TF Seleccionados**: {15: 875, 5: 263, 240: 1357, 60: 6164, 1440: 1142}
- **DistATR** - Candidatos: avg=21.9 | Seleccionados: avg=9.9
- **Razones de selección**: {'Fallback<15': 2122, 'InBand[8,15]_TFPreference': 7679}
- **En banda [10,15] ATR**: 42426/250867 (16.9%)

### Take Profit (TP)
- Zonas analizadas: 9880 | Total candidatos: 136093 | Seleccionados: 9880
- Candidatos por zona (promedio): 13.8
- **Edad (barras)** - Candidatos: med=34, max=150 | Seleccionados: med=0, max=89
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.34
- **Priority Candidatos**: {'P3': 136093}
- **Priority Seleccionados**: {'P4_Fallback': 5202, 'P3': 4678}
- **Type Candidatos**: {'Swing': 136093}
- **Type Seleccionados**: {'Calculated': 5202, 'Swing': 4678}
- **TF Candidatos**: {240: 30956, 5: 29706, 15: 29266, 60: 26890, 1440: 19275}
- **TF Seleccionados**: {-1: 5202, 5: 115, 240: 1166, 60: 124, 1440: 3264, 15: 9}
- **DistATR** - Candidatos: avg=12.5 | Seleccionados: avg=15.5
- **RR** - Candidatos: avg=1.10 | Seleccionados: avg=1.30
- **Razones de selección**: {'NoStructuralTarget': 5202, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 37, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 304, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 526, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 132, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 228, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 215, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 361, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 347, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 64, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 249, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 19, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 28, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 8, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 7, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 8, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 136, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 191, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 130, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 103, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 223, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of20': 13, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of25': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 54, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of23': 21, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of26': 8, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of24': 12, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of17': 26, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 16, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of18': 26, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 45, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of22': 12, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of21': 19, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of27': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of28': 4, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 2, 'ForcedP3_NoFallback': 1087, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of30': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of29': 2}

### 🎯 Recomendaciones
- ⚠️ SL: 64% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 64% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.89.