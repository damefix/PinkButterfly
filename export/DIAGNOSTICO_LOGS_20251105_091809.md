# Informe Diagnóstico de Logs - 2025-11-05 09:25:33

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251105_091809.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251105_091809.csv`

## DFM
- Eventos de evaluación: 1739
- Evaluaciones Bull: 2982 | Bear: 251
- Pasaron umbral (PassedThreshold): 1911
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:12, 4:107, 5:609, 6:1216, 7:1281, 8:8, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3946
- KeptAligned: 8989/10052 | KeptCounter: 1791/3015
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.514 | AvgProxCounter≈ 0.138
  - AvgDistATRAligned≈ 2.90 | AvgDistATRCounter≈ 0.64
- PreferAligned eventos: 3195 | Filtradas contra-bias: 879

### Proximity (Pre-PreferAligned)
- Eventos: 3946
- Aligned pre: 8989/10780 | Counter pre: 1791/10780
- AvgProxAligned(pre)≈ 0.514 | AvgDistATRAligned(pre)≈ 2.90

### Proximity Drivers
- Eventos: 3946
- Alineadas: n=8989 | BaseProx≈ 0.666 | ZoneATR≈ 4.70 | SizePenalty≈ 0.982 | FinalProx≈ 0.656
- Contra-bias: n=912 | BaseProx≈ 0.555 | ZoneATR≈ 4.60 | SizePenalty≈ 0.983 | FinalProx≈ 0.547

## Risk
- Eventos: 3632
- Accepted=3267 | RejSL=77 | RejTP=2042 | RejRR=0 | RejEntry=0
### Risk – Validación Doble Cerrojo (V6.0d)
- **RejSL_Points:** 4140 (rechazados por >60pts)
- **RejTP_Points:** 36 (rechazados por >120pts)
- **RejSL_HighVol:** 0 (rechazados por ATR>15 y DistATR>10)

**Rechazos SL por TF:**

| TF | RejSL_Points |
|----|--------------|
| -1 | 9 |
| 5 | 15 |
| 15 | 37 |
| 60 | 2384 |
| 240 | 800 |
| 1440 | 895 |

**Rechazos TP por TF:**

| TF | RejTP_Points |
|----|--------------|
| 1440 | 36 |

### TP Policy (V6.0c)
- **FORCED_P3:** 4687 (47.3%)
- **P4_FALLBACK:** 5214 (52.7%)
- **FORCED_P3 por TF:**
  - TF5: 115 (2.5%)
  - TF15: 9 (0.2%)
  - TF60: 124 (2.6%)
  - TF240: 1174 (25.0%)
  - TF1440: 3265 (69.7%)

### Risk Drivers (Rechazos por SL)
- Alineadas: n=77 | SLDistATR≈ 16.62 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:75,20-25:1,25+:1
- HistSL Counter 0-10:0,10-15:0,15-20:0,20-25:0,25+:0

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 68 | Unmatched: 3199
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
- Muestras: 3267 | Aligned=3207 (98.2%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.10 | Confidence≈ 0.00
- SL_TF dist: {'-1': 29, '5': 134, '15': 503, '240': 252, '60': 2311, '1440': 38} | SL_Structural≈ 99.1%
- TP_TF dist: {'-1': 2806, '5': 98, '240': 255, '60': 86, '1440': 19, '15': 3} | TP_Structural≈ 14.1%

### SLPick por Bandas y TF
- Bandas: lt8=2702, 8-10=321, 10-12.5=147, 12.5-15=97, >15=0
- TF: 5m=134, 15m=503, 60m=2311, 240m=252, 1440m=38
- RR plan por bandas: 0-10≈ 1.11 (n=3023), 10-15≈ 1.01 (n=244)

## CancelBias (EMA200@60m)
- Eventos: 146
- Distribución Bias: {'Bullish': 110, 'Bearish': 36, 'Neutral': 0}
- Coherencia (Close>EMA): 110/146 (75.3%)

## StructureFusion
- Trazas por zona: 13067 | Zonas con Anchors: 13015
- Dir zonas (zona): Bull=7500 Bear=5567 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.5, DirBear≈ 1.1, DirNeutral≈ 0.0
- Razones de dirección: {'triggers-only': 50, 'tie-bias': 569, 'anchors+triggers': 12448}
- TF Triggers: {'5': 8373, '15': 4694}
- TF Anchors: {'240': 12782, '60': 12899, '1440': 10157}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3748, 'Bearish': 1252, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3748/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,41': 1, 'estructura no existe': 15, 'score decayó a 0,33': 1, 'score decayó a 0,32': 1, 'score decayó a 0,27': 1, 'score decayó a 0,26': 1}

## CSV de Trades
- Filas: 86 | Ejecutadas: 19 | Canceladas: 0 | Expiradas: 0
- BUY: 96 | SELL: 9

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1911
- Registered: 43
  - DEDUP_COOLDOWN: 2 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 45

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 2.4%
- RegRate = Registered / Intentos = 95.6%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 4.4%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 44.2%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 9871 | Total candidatos: 251053 | Seleccionados: 9817
- Candidatos por zona (promedio): 25.4
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 85223, 60: 76138, 15: 41787, 1440: 24880, 5: 23025}
- **TF Seleccionados**: {5: 269, 15: 870, 240: 1371, 60: 6165, 1440: 1142}
- **DistATR** - Candidatos: avg=21.9 | Seleccionados: avg=9.9
- **Razones de selección**: {'Fallback<15': 2138, 'InBand[8,15]_TFPreference': 7679}
- **En banda [10,15] ATR**: 42419/251053 (16.9%)

### Take Profit (TP)
- Zonas analizadas: 9901 | Total candidatos: 136134 | Seleccionados: 9901
- Candidatos por zona (promedio): 13.7
- **Edad (barras)** - Candidatos: med=34, max=150 | Seleccionados: med=0, max=89
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.34
- **Priority Candidatos**: {'P3': 136134}
- **Priority Seleccionados**: {'P4_Fallback': 5214, 'P3': 4687}
- **Type Candidatos**: {'Swing': 136134}
- **Type Seleccionados**: {'Calculated': 5214, 'Swing': 4687}
- **TF Candidatos**: {240: 30973, 5: 29711, 15: 29277, 60: 26889, 1440: 19284}
- **TF Seleccionados**: {-1: 5214, 5: 115, 240: 1174, 60: 124, 1440: 3265, 15: 9}
- **DistATR** - Candidatos: avg=12.5 | Seleccionados: avg=15.5
- **RR** - Candidatos: avg=1.10 | Seleccionados: avg=1.30
- **Razones de selección**: {'NoStructuralTarget': 5214, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 37, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 529, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 309, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 192, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 228, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 215, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 362, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 347, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 64, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 249, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 19, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 28, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 8, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 7, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 8, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 136, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 131, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 130, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 103, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 223, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of20': 13, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of25': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 54, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of23': 21, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of26': 8, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of24': 12, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of17': 26, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 16, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of18': 26, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 45, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of22': 12, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of21': 19, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of27': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of28': 4, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 2, 'ForcedP3_NoFallback': 1087, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of30': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of29': 2}

### 🎯 Recomendaciones
- ⚠️ SL: 64% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 64% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.89.