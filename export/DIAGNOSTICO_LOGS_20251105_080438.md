# Informe Diagnóstico de Logs - 2025-11-05 08:12:56

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251105_080438.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251105_080438.csv`

## DFM
- Eventos de evaluación: 1728
- Evaluaciones Bull: 2982 | Bear: 215
- Pasaron umbral (PassedThreshold): 2473
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:12, 4:104, 5:608, 6:1203, 7:1262, 8:8, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3932
- KeptAligned: 8931/9994 | KeptCounter: 1793/3010
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.513 | AvgProxCounter≈ 0.138
  - AvgDistATRAligned≈ 2.90 | AvgDistATRCounter≈ 0.64
- PreferAligned eventos: 3178 | Filtradas contra-bias: 877

### Proximity (Pre-PreferAligned)
- Eventos: 3932
- Aligned pre: 8931/10724 | Counter pre: 1793/10724
- AvgProxAligned(pre)≈ 0.513 | AvgDistATRAligned(pre)≈ 2.90

### Proximity Drivers
- Eventos: 3932
- Alineadas: n=8931 | BaseProx≈ 0.666 | ZoneATR≈ 4.69 | SizePenalty≈ 0.982 | FinalProx≈ 0.655
- Contra-bias: n=916 | BaseProx≈ 0.554 | ZoneATR≈ 4.60 | SizePenalty≈ 0.983 | FinalProx≈ 0.546

## Risk
- Eventos: 3619
- Accepted=3229 | RejSL=77 | RejTP=2030 | RejRR=0 | RejEntry=0
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
- **FORCED_P3:** 4668 (47.4%)
- **P4_FALLBACK:** 5179 (52.6%)
- **FORCED_P3 por TF:**
  - TF5: 114 (2.4%)
  - TF15: 9 (0.2%)
  - TF60: 124 (2.7%)
  - TF240: 1159 (24.8%)
  - TF1440: 3262 (69.9%)

### Risk Drivers (Rechazos por SL)
- Alineadas: n=77 | SLDistATR≈ 16.62 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:75,20-25:1,25+:1
- HistSL Counter 0-10:0,10-15:0,15-20:0,20-25:0,25+:0

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 74 | Unmatched: 3155
- 0-10: Wins=22 Losses=49 WR=31.0% (n=71)
- 10-15: Wins=1 Losses=2 WR=33.3% (n=3)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=23 Losses=51 WR=31.1% (n=74)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 3229 | Aligned=3167 (98.1%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.10 | Confidence≈ 0.00
- SL_TF dist: {'-1': 28, '5': 129, '15': 502, '240': 223, '60': 2309, '1440': 38} | SL_Structural≈ 99.1%
- TP_TF dist: {'-1': 2771, '240': 253, '5': 97, '60': 86, '1440': 19, '15': 3} | TP_Structural≈ 14.2%

### SLPick por Bandas y TF
- Bandas: lt8=2664, 8-10=321, 10-12.5=147, 12.5-15=97, >15=0
- TF: 5m=129, 15m=502, 60m=2309, 240m=223, 1440m=38
- RR plan por bandas: 0-10≈ 1.11 (n=2985), 10-15≈ 1.01 (n=244)

## CancelBias (EMA200@60m)
- Eventos: 100
- Distribución Bias: {'Bullish': 97, 'Bearish': 3, 'Neutral': 0}
- Coherencia (Close>EMA): 97/100 (97.0%)

## StructureFusion
- Trazas por zona: 13004 | Zonas con Anchors: 12944
- Dir zonas (zona): Bull=7495 Bear=5509 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.5, DirBear≈ 1.1, DirNeutral≈ 0.0
- Razones de dirección: {'triggers-only': 57, 'tie-bias': 566, 'anchors+triggers': 12381}
- TF Triggers: {'15': 4663, '5': 8341}
- TF Anchors: {'240': 12711, '60': 12832, '1440': 10145}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3748, 'Bearish': 1252, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3748/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 17, 'score decayó a 0,33': 1, 'score decayó a 0,32': 1, 'score decayó a 0,27': 1}

## CSV de Trades
- Filas: 92 | Ejecutadas: 20 | Canceladas: 0 | Expiradas: 0
- BUY: 105 | SELL: 7

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 2473
- Registered: 46
  - DEDUP_COOLDOWN: 1 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 47

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 1.9%
- RegRate = Registered / Intentos = 97.9%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 2.1%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 43.5%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 9816 | Total candidatos: 250638 | Seleccionados: 9762
- Candidatos por zona (promedio): 25.5
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.44
- **TF Candidatos**: {240: 85154, 60: 76071, 15: 41724, 1440: 24875, 5: 22814}
- **TF Seleccionados**: {5: 265, 15: 868, 240: 1327, 60: 6160, 1440: 1142}
- **DistATR** - Candidatos: avg=21.9 | Seleccionados: avg=9.9
- **Razones de selección**: {'Fallback<15': 2094, 'InBand[8,15]_TFPreference': 7668}
- **En banda [10,15] ATR**: 42399/250638 (16.9%)

### Take Profit (TP)
- Zonas analizadas: 9847 | Total candidatos: 135960 | Seleccionados: 9847
- Candidatos por zona (promedio): 13.8
- **Edad (barras)** - Candidatos: med=34, max=150 | Seleccionados: med=0, max=89
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.34
- **Priority Candidatos**: {'P3': 135960}
- **Priority Seleccionados**: {'P4_Fallback': 5179, 'P3': 4668}
- **Type Candidatos**: {'Swing': 135960}
- **Type Seleccionados**: {'Calculated': 5179, 'Swing': 4668}
- **TF Candidatos**: {240: 30937, 5: 29626, 15: 29262, 60: 26878, 1440: 19257}
- **TF Seleccionados**: {-1: 5179, 240: 1159, 5: 114, 60: 124, 1440: 3262, 15: 9}
- **DistATR** - Candidatos: avg=12.5 | Seleccionados: avg=15.5
- **RR** - Candidatos: avg=1.10 | Seleccionados: avg=1.30
- **Razones de selección**: {'NoStructuralTarget': 5179, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 298, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 348, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 524, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 132, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 228, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 215, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 359, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 64, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 249, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 19, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 36, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 28, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 8, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 7, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 8, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 136, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 191, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 130, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 103, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 223, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of20': 13, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of25': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 54, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of23': 21, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of26': 8, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of24': 12, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of17': 26, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 16, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of18': 26, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 45, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of22': 12, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of21': 19, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of27': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of28': 4, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 2, 'ForcedP3_NoFallback': 1087, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of30': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of29': 2}

### 🎯 Recomendaciones
- ⚠️ SL: 64% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 64% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.89.