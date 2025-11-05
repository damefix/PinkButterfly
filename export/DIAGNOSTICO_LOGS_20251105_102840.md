# Informe Diagnóstico de Logs - 2025-11-05 10:37:51

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251105_102840.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251105_102840.csv`

## DFM
- Eventos de evaluación: 2363
- Evaluaciones Bull: 3411 | Bear: 918
- Pasaron umbral (PassedThreshold): 2340
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:53, 4:303, 5:906, 6:1516, 7:1543, 8:8, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3957
- KeptAligned: 9014/10074 | KeptCounter: 1812/3040
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.515 | AvgProxCounter≈ 0.139
  - AvgDistATRAligned≈ 2.90 | AvgDistATRCounter≈ 0.64
- PreferAligned eventos: 3202 | Filtradas contra-bias: 890

### Proximity (Pre-PreferAligned)
- Eventos: 3957
- Aligned pre: 9014/10826 | Counter pre: 1812/10826
- AvgProxAligned(pre)≈ 0.515 | AvgDistATRAligned(pre)≈ 2.90

### Proximity Drivers
- Eventos: 3957
- Alineadas: n=9014 | BaseProx≈ 0.668 | ZoneATR≈ 4.69 | SizePenalty≈ 0.982 | FinalProx≈ 0.657
- Contra-bias: n=922 | BaseProx≈ 0.552 | ZoneATR≈ 4.61 | SizePenalty≈ 0.983 | FinalProx≈ 0.544

## Risk
- Eventos: 3645
- Accepted=4357 | RejSL=77 | RejTP=1000 | RejRR=0 | RejEntry=0
### Risk – Validación Doble Cerrojo (V6.0d)
- **RejSL_Points:** 4146 (rechazados por >60pts)
- **RejTP_Points:** 0 (rechazados por >120pts)
- **RejSL_HighVol:** 0 (rechazados por ATR>15 y DistATR>10)

**Rechazos SL por TF:**

| TF | RejSL_Points |
|----|--------------|
| -1 | 9 |
| 5 | 15 |
| 15 | 37 |
| 60 | 2389 |
| 240 | 801 |
| 1440 | 895 |

### TP Policy (V6.0c)
- **FORCED_P3:** 2141 (25.4%)
- **P4_FALLBACK:** 6283 (74.6%)
- **FORCED_P3 por TF:**
  - TF5: 54 (2.5%)
  - TF60: 51 (2.4%)
  - TF240: 807 (37.7%)
  - TF1440: 1229 (57.4%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 334 (3.4% del total)
  - Avg Score: 0.63 | Avg R:R: 1.53 | Avg DistATR: 10.70
  - Por TF: TF5=206, TF15=128
- **P0_SWING_LITE:** 1178 (11.9% del total)
  - Avg Score: 0.33 | Avg R:R: 2.05 | Avg DistATR: 10.31
  - Por TF: TF15=532, TF60=646

### Risk Drivers (Rechazos por SL)
- Alineadas: n=77 | SLDistATR≈ 16.62 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:75,20-25:1,25+:1
- HistSL Counter 0-10:0,10-15:0,15-20:0,20-25:0,25+:0

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 86 | Unmatched: 4271
- 0-10: Wins=16 Losses=66 WR=19.5% (n=82)
- 10-15: Wins=3 Losses=1 WR=75.0% (n=4)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=19 Losses=67 WR=22.1% (n=86)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 4357 | Aligned=4011 (92.1%)
- Core≈ 1.00 | Prox≈ 0.65 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.34 | Confidence≈ 0.00
- SL_TF dist: {'240': 475, '60': 2900, '-1': 37, '5': 170, '15': 600, '1440': 175} | SL_Structural≈ 99.2%
- TP_TF dist: {'-1': 2817, '240': 151, '5': 218, '15': 607, '60': 552, '1440': 12} | TP_Structural≈ 35.3%

### SLPick por Bandas y TF
- Bandas: lt8=3730, 8-10=347, 10-12.5=175, 12.5-15=105, >15=0
- TF: 5m=170, 15m=600, 60m=2900, 240m=475, 1440m=175
- RR plan por bandas: 0-10≈ 1.35 (n=4077), 10-15≈ 1.14 (n=280)

## CancelBias (EMA200@60m)
- Eventos: 135
- Distribución Bias: {'Bullish': 110, 'Bearish': 25, 'Neutral': 0}
- Coherencia (Close>EMA): 110/135 (81.5%)

## StructureFusion
- Trazas por zona: 13114 | Zonas con Anchors: 13089
- Dir zonas (zona): Bull=7514 Bear=5600 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.5, DirBear≈ 1.1, DirNeutral≈ 0.0
- Razones de dirección: {'triggers-only': 25, 'anchors+triggers': 12505, 'tie-bias': 584}
- TF Triggers: {'5': 8389, '15': 4725}
- TF Anchors: {'240': 12856, '60': 12977, '1440': 10171}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3748, 'Bearish': 1252, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3748/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 20, 'score decayó a 0,32': 1, 'score decayó a 0,27': 1, 'score decayó a 0,26': 1}

## CSV de Trades
- Filas: 98 | Ejecutadas: 22 | Canceladas: 0 | Expiradas: 0
- BUY: 101 | SELL: 19

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 2340
- Registered: 49
  - DEDUP_COOLDOWN: 2 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 51

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 2.2%
- RegRate = Registered / Intentos = 96.1%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 3.9%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 44.9%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 9911 | Total candidatos: 251221 | Seleccionados: 9857
- Candidatos por zona (promedio): 25.3
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 85300, 60: 76233, 15: 41896, 1440: 24891, 5: 22901}
- **TF Seleccionados**: {240: 1426, 60: 6169, 5: 257, 15: 863, 1440: 1142}
- **DistATR** - Candidatos: avg=21.9 | Seleccionados: avg=9.8
- **Razones de selección**: {'Fallback<15': 2178, 'InBand[8,15]_TFPreference': 7679}
- **En banda [10,15] ATR**: 42407/251221 (16.9%)

### Take Profit (TP)
- Zonas analizadas: 8424 | Total candidatos: 100662 | Seleccionados: 8424
- Candidatos por zona (promedio): 11.9
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=78
- **Score** - Candidatos: avg=0.53 | Seleccionados: avg=0.18
- **Priority Candidatos**: {'P3': 100662}
- **Priority Seleccionados**: {'P4_Fallback': 6283, 'P3': 2141}
- **Type Candidatos**: {'Swing': 100662}
- **Type Seleccionados**: {'Calculated': 6283, 'Swing': 2141}
- **TF Candidatos**: {5: 23889, 240: 22094, 15: 21336, 60: 18009, 1440: 15334}
- **TF Seleccionados**: {-1: 6283, 240: 807, 5: 54, 60: 51, 1440: 1229}
- **DistATR** - Candidatos: avg=10.5 | Seleccionados: avg=13.8
- **RR** - Candidatos: avg=0.89 | Seleccionados: avg=1.11
- **Razones de selección**: {'NoStructuralTarget': 6283, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 354, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 91, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 308, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 290, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 12, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 11, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 144, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 41, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 174, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 504, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 14, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 16, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 19, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 7, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 24, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 95, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 12, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 2, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of24': 3, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 3, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 2, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of18': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 4, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 64% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 75% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.89.