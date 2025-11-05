# Informe Diagnóstico de Logs - 2025-11-05 07:54:26

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251105_074708.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251105_074708.csv`

## DFM
- Eventos de evaluación: 1758
- Evaluaciones Bull: 2982 | Bear: 259
- Pasaron umbral (PassedThreshold): 2483
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:23, 4:119, 5:616, 6:1213, 7:1262, 8:8, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3933
- KeptAligned: 8934/9996 | KeptCounter: 1792/3006
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.513 | AvgProxCounter≈ 0.138
  - AvgDistATRAligned≈ 2.90 | AvgDistATRCounter≈ 0.64
- PreferAligned eventos: 3178 | Filtradas contra-bias: 876

### Proximity (Pre-PreferAligned)
- Eventos: 3933
- Aligned pre: 8934/10726 | Counter pre: 1792/10726
- AvgProxAligned(pre)≈ 0.513 | AvgDistATRAligned(pre)≈ 2.90

### Proximity Drivers
- Eventos: 3933
- Alineadas: n=8934 | BaseProx≈ 0.666 | ZoneATR≈ 4.69 | SizePenalty≈ 0.982 | FinalProx≈ 0.655
- Contra-bias: n=916 | BaseProx≈ 0.553 | ZoneATR≈ 4.60 | SizePenalty≈ 0.983 | FinalProx≈ 0.546

## Risk
- Eventos: 3619
- Accepted=3271 | RejSL=77 | RejTP=1992 | RejRR=0 | RejEntry=0
### Risk – Validación Doble Cerrojo (V6.0d)
- **RejSL_Points:** 4135 (rechazados por >60pts)
- **RejTP_Points:** 36 (rechazados por >120pts)
- **RejSL_HighVol:** 0 (rechazados por ATR>15 y DistATR>10)

**Rechazos SL por TF:**

| TF | RejSL_Points |
|----|--------------|
| -1 | 10 |
| 5 | 15 |
| 15 | 37 |
| 60 | 2380 |
| 240 | 798 |
| 1440 | 895 |

**Rechazos TP por TF:**

| TF | RejTP_Points |
|----|--------------|
| 1440 | 36 |

### TP Policy (V6.0c)
- **FORCED_P3:** 4667 (47.4%)
- **P4_FALLBACK:** 5183 (52.6%)
- **FORCED_P3 por TF:**
  - TF5: 152 (3.3%)
  - TF15: 35 (0.7%)
  - TF60: 191 (4.1%)
  - TF240: 1028 (22.0%)
  - TF1440: 3261 (69.9%)

### Risk Drivers (Rechazos por SL)
- Alineadas: n=77 | SLDistATR≈ 16.62 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:75,20-25:1,25+:1
- HistSL Counter 0-10:0,10-15:0,15-20:0,20-25:0,25+:0

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 77 | Unmatched: 3194
- 0-10: Wins=26 Losses=49 WR=34.7% (n=75)
- 10-15: Wins=0 Losses=2 WR=0.0% (n=2)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=26 Losses=51 WR=33.8% (n=77)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 3271 | Aligned=3173 (97.0%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.11 | Confidence≈ 0.00
- SL_TF dist: {'-1': 32, '5': 131, '15': 505, '240': 223, '60': 2342, '1440': 38} | SL_Structural≈ 99.0%
- TP_TF dist: {'-1': 2776, '240': 236, '5': 104, '60': 109, '15': 27, '1440': 19} | TP_Structural≈ 15.1%

### SLPick por Bandas y TF
- Bandas: lt8=2704, 8-10=323, 10-12.5=147, 12.5-15=97, >15=0
- TF: 5m=131, 15m=505, 60m=2342, 240m=223, 1440m=38
- RR plan por bandas: 0-10≈ 1.11 (n=3027), 10-15≈ 1.01 (n=244)

## CancelBias (EMA200@60m)
- Eventos: 114
- Distribución Bias: {'Bullish': 111, 'Bearish': 3, 'Neutral': 0}
- Coherencia (Close>EMA): 111/114 (97.4%)

## StructureFusion
- Trazas por zona: 13002 | Zonas con Anchors: 12940
- Dir zonas (zona): Bull=7491 Bear=5511 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.5, DirBear≈ 1.1, DirNeutral≈ 0.0
- Razones de dirección: {'triggers-only': 56, 'tie-bias': 569, 'anchors+triggers': 12377}
- TF Triggers: {'5': 8340, '15': 4662}
- TF Anchors: {'240': 12707, '60': 12831, '1440': 10144}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3748, 'Bearish': 1252, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3748/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 19, 'score decayó a 0,33': 1, 'score decayó a 0,32': 1, 'score decayó a 0,39': 1}

## CSV de Trades
- Filas: 98 | Ejecutadas: 22 | Canceladas: 0 | Expiradas: 0
- BUY: 108 | SELL: 12

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 2483
- Registered: 49
  - DEDUP_COOLDOWN: 1 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 50

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 2.0%
- RegRate = Registered / Intentos = 98.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 2.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 44.9%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 9816 | Total candidatos: 250607 | Seleccionados: 9762
- Candidatos por zona (promedio): 25.5
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.44
- **TF Candidatos**: {240: 85146, 60: 76061, 15: 41718, 1440: 24874, 5: 22808}
- **TF Seleccionados**: {5: 267, 15: 867, 240: 1327, 60: 6159, 1440: 1142}
- **DistATR** - Candidatos: avg=21.9 | Seleccionados: avg=9.9
- **Razones de selección**: {'Fallback<15': 2098, 'InBand[8,15]_TFPreference': 7664}
- **En banda [10,15] ATR**: 42388/250607 (16.9%)

### Take Profit (TP)
- Zonas analizadas: 9850 | Total candidatos: 135940 | Seleccionados: 9850
- Candidatos por zona (promedio): 13.8
- **Edad (barras)** - Candidatos: med=34, max=150 | Seleccionados: med=0, max=87
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.33
- **Priority Candidatos**: {'P3': 135940}
- **Priority Seleccionados**: {'P4_Fallback': 5183, 'P3': 4667}
- **Type Candidatos**: {'Swing': 135940}
- **Type Seleccionados**: {'Calculated': 5183, 'Swing': 4667}
- **TF Candidatos**: {240: 30933, 5: 29622, 15: 29260, 60: 26877, 1440: 19248}
- **TF Seleccionados**: {-1: 5183, 240: 1028, 5: 152, 60: 191, 1440: 3261, 15: 35}
- **DistATR** - Candidatos: avg=12.5 | Seleccionados: avg=15.5
- **RR** - Candidatos: avg=1.10 | Seleccionados: avg=1.31
- **Razones de selección**: {'NoStructuralTarget': 5183, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 298, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 348, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 524, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 132, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 229, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 215, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 354, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 64, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 248, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 19, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 36, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 28, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 8, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 7, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 8, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 136, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 191, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 130, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 103, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 227, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of20': 13, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of25': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 54, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of23': 21, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of26': 8, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of24': 12, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of17': 26, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 16, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of18': 26, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 45, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of22': 12, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of21': 19, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of27': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of28': 4, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 2, 'ForcedP3_NoFallback': 1087, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of30': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of29': 2}

### 🎯 Recomendaciones
- ⚠️ SL: 64% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 64% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.89.