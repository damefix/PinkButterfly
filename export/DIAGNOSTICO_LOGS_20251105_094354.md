# Informe Diagnóstico de Logs - 2025-11-05 09:48:43

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251105_094354.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251105_094354.csv`

## DFM
- Eventos de evaluación: 1810
- Evaluaciones Bull: 3116 | Bear: 286
- Pasaron umbral (PassedThreshold): 1978
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:13, 4:149, 5:656, 6:1247, 7:1329, 8:8, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3946
- KeptAligned: 8976/10038 | KeptCounter: 1792/3018
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.514 | AvgProxCounter≈ 0.138
  - AvgDistATRAligned≈ 2.90 | AvgDistATRCounter≈ 0.64
- PreferAligned eventos: 3194 | Filtradas contra-bias: 880

### Proximity (Pre-PreferAligned)
- Eventos: 3946
- Aligned pre: 8976/10768 | Counter pre: 1792/10768
- AvgProxAligned(pre)≈ 0.514 | AvgDistATRAligned(pre)≈ 2.90

### Proximity Drivers
- Eventos: 3946
- Alineadas: n=8976 | BaseProx≈ 0.667 | ZoneATR≈ 4.70 | SizePenalty≈ 0.982 | FinalProx≈ 0.656
- Contra-bias: n=912 | BaseProx≈ 0.555 | ZoneATR≈ 4.60 | SizePenalty≈ 0.983 | FinalProx≈ 0.547

## Risk
- Eventos: 3631
- Accepted=3432 | RejSL=77 | RejTP=1863 | RejRR=0 | RejEntry=0
### Risk – Validación Doble Cerrojo (V6.0d)
- **RejSL_Points:** 4142 (rechazados por >60pts)
- **RejTP_Points:** 37 (rechazados por >120pts)
- **RejSL_HighVol:** 0 (rechazados por ATR>15 y DistATR>10)

**Rechazos SL por TF:**

| TF | RejSL_Points |
|----|--------------|
| -1 | 9 |
| 5 | 15 |
| 15 | 37 |
| 60 | 2386 |
| 240 | 800 |
| 1440 | 895 |

**Rechazos TP por TF:**

| TF | RejTP_Points |
|----|--------------|
| 15 | 2 |
| 1440 | 35 |

### TP Policy (V6.0c)
- **FORCED_P3:** 4336 (45.5%)
- **P4_FALLBACK:** 5191 (54.5%)
- **FORCED_P3 por TF:**
  - TF5: 58 (1.3%)
  - TF15: 7 (0.2%)
  - TF60: 105 (2.4%)
  - TF240: 1053 (24.3%)
  - TF1440: 3113 (71.8%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 361 (3.7% del total)
  - Avg Score: 0.63 | Avg R:R: 1.54 | Avg DistATR: 10.62
  - Por TF: TF5=214, TF15=147

### Risk Drivers (Rechazos por SL)
- Alineadas: n=77 | SLDistATR≈ 16.62 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:75,20-25:1,25+:1
- HistSL Counter 0-10:0,10-15:0,15-20:0,20-25:0,25+:0

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 72 | Unmatched: 3360
- 0-10: Wins=15 Losses=53 WR=22.1% (n=68)
- 10-15: Wins=3 Losses=1 WR=75.0% (n=4)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=18 Losses=54 WR=25.0% (n=72)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 3432 | Aligned=3360 (97.9%)
- Core≈ 1.00 | Prox≈ 0.66 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.12 | Confidence≈ 0.00
- SL_TF dist: {'-1': 34, '5': 133, '15': 511, '240': 293, '60': 2421, '1440': 40} | SL_Structural≈ 99.0%
- TP_TF dist: {'-1': 2785, '15': 112, '240': 223, '5': 225, '60': 72, '1440': 15} | TP_Structural≈ 18.9%

### SLPick por Bandas y TF
- Bandas: lt8=2867, 8-10=321, 10-12.5=147, 12.5-15=97, >15=0
- TF: 5m=133, 15m=511, 60m=2421, 240m=293, 1440m=40
- RR plan por bandas: 0-10≈ 1.13 (n=3188), 10-15≈ 1.01 (n=244)

## CancelBias (EMA200@60m)
- Eventos: 146
- Distribución Bias: {'Bullish': 110, 'Bearish': 36, 'Neutral': 0}
- Coherencia (Close>EMA): 110/146 (75.3%)

## StructureFusion
- Trazas por zona: 13056 | Zonas con Anchors: 13012
- Dir zonas (zona): Bull=7503 Bear=5553 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.5, DirBear≈ 1.1, DirNeutral≈ 0.0
- Razones de dirección: {'triggers-only': 41, 'tie-bias': 571, 'anchors+triggers': 12444}
- TF Triggers: {'15': 4696, '5': 8360}
- TF Anchors: {'240': 12779, '60': 12900, '1440': 10162}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3748, 'Bearish': 1252, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3748/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,41': 1, 'estructura no existe': 18, 'score decayó a 0,33': 1, 'score decayó a 0,32': 1, 'score decayó a 0,27': 1, 'score decayó a 0,26': 1}

## CSV de Trades
- Filas: 92 | Ejecutadas: 19 | Canceladas: 0 | Expiradas: 0
- BUY: 98 | SELL: 13

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1978
- Registered: 46
  - DEDUP_COOLDOWN: 2 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 48

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 2.4%
- RegRate = Registered / Intentos = 95.8%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 4.2%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 41.3%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 9860 | Total candidatos: 251065 | Seleccionados: 9806
- Candidatos por zona (promedio): 25.5
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 85229, 60: 76155, 15: 41791, 1440: 24883, 5: 23007}
- **TF Seleccionados**: {5: 264, 15: 866, 240: 1369, 60: 6165, 1440: 1142}
- **DistATR** - Candidatos: avg=21.9 | Seleccionados: avg=9.9
- **Razones de selección**: {'Fallback<15': 2132, 'InBand[8,15]_TFPreference': 7674}
- **En banda [10,15] ATR**: 42397/251065 (16.9%)

### Take Profit (TP)
- Zonas analizadas: 9527 | Total candidatos: 128232 | Seleccionados: 9527
- Candidatos por zona (promedio): 13.5
- **Edad (barras)** - Candidatos: med=34, max=150 | Seleccionados: med=0, max=78
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.33
- **Priority Candidatos**: {'P3': 128232}
- **Priority Seleccionados**: {'P4_Fallback': 5191, 'P3': 4336}
- **Type Candidatos**: {'Swing': 128232}
- **Type Seleccionados**: {'Calculated': 5191, 'Swing': 4336}
- **TF Candidatos**: {240: 29545, 5: 27294, 15: 27287, 60: 25449, 1440: 18657}
- **TF Seleccionados**: {-1: 5191, 15: 7, 240: 1053, 5: 58, 60: 105, 1440: 3113}
- **DistATR** - Candidatos: avg=12.7 | Seleccionados: avg=15.6
- **RR** - Candidatos: avg=1.08 | Seleccionados: avg=1.28
- **Razones de selección**: {'NoStructuralTarget': 5191, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 13, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 526, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 309, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 175, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 211, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 191, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 353, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 338, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 12, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 6, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 116, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 115, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 85, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 122, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 236, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 219, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of20': 12, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 18, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 40, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 44, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of18': 16, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 29, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of17': 12, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of22': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of25': 2, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of24': 10, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of27': 3, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of28': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 9, 'ForcedP3_NoFallback': 1062, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of23': 15, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of21': 15, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of26': 4, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 2, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of30': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 2}

### 🎯 Recomendaciones
- ⚠️ SL: 64% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 66% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.89.