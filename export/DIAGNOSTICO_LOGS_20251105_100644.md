# Informe Diagnóstico de Logs - 2025-11-05 10:16:16

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251105_100644.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251105_100644.csv`

## DFM
- Eventos de evaluación: 2344
- Evaluaciones Bull: 3414 | Bear: 896
- Pasaron umbral (PassedThreshold): 2335
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:53, 4:297, 5:897, 6:1511, 7:1544, 8:8, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3958
- KeptAligned: 9009/10069 | KeptCounter: 1819/3048
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.514 | AvgProxCounter≈ 0.139
  - AvgDistATRAligned≈ 2.89 | AvgDistATRCounter≈ 0.65
- PreferAligned eventos: 3200 | Filtradas contra-bias: 892

### Proximity (Pre-PreferAligned)
- Eventos: 3958
- Aligned pre: 9009/10828 | Counter pre: 1819/10828
- AvgProxAligned(pre)≈ 0.514 | AvgDistATRAligned(pre)≈ 2.89

### Proximity Drivers
- Eventos: 3958
- Alineadas: n=9009 | BaseProx≈ 0.668 | ZoneATR≈ 4.69 | SizePenalty≈ 0.982 | FinalProx≈ 0.657
- Contra-bias: n=927 | BaseProx≈ 0.550 | ZoneATR≈ 4.61 | SizePenalty≈ 0.983 | FinalProx≈ 0.542

## Risk
- Eventos: 3647
- Accepted=4339 | RejSL=77 | RejTP=993 | RejRR=0 | RejEntry=0
### Risk – Validación Doble Cerrojo (V6.0d)
- **RejSL_Points:** 4147 (rechazados por >60pts)
- **RejTP_Points:** 37 (rechazados por >120pts)
- **RejSL_HighVol:** 0 (rechazados por ATR>15 y DistATR>10)

**Rechazos SL por TF:**

| TF | RejSL_Points |
|----|--------------|
| -1 | 9 |
| 5 | 15 |
| 15 | 37 |
| 60 | 2389 |
| 240 | 802 |
| 1440 | 895 |

**Rechazos TP por TF:**

| TF | RejTP_Points |
|----|--------------|
| 15 | 2 |
| 1440 | 35 |

### TP Policy (V6.0c)
- **FORCED_P3:** 3187 (37.9%)
- **P4_FALLBACK:** 5217 (62.1%)
- **FORCED_P3 por TF:**
  - TF5: 50 (1.6%)
  - TF15: 4 (0.1%)
  - TF60: 51 (1.6%)
  - TF240: 798 (25.0%)
  - TF1440: 2284 (71.7%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 357 (3.6% del total)
  - Avg Score: 0.63 | Avg R:R: 1.54 | Avg DistATR: 10.62
  - Por TF: TF5=210, TF15=147
- **P0_SWING_LITE:** 1175 (11.8% del total)
  - Avg Score: 0.33 | Avg R:R: 2.05 | Avg DistATR: 10.32
  - Por TF: TF15=529, TF60=646

### Risk Drivers (Rechazos por SL)
- Alineadas: n=77 | SLDistATR≈ 16.62 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:75,20-25:1,25+:1
- HistSL Counter 0-10:0,10-15:0,15-20:0,20-25:0,25+:0

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 103 | Unmatched: 4236
- 0-10: Wins=14 Losses=73 WR=16.1% (n=87)
- 10-15: Wins=3 Losses=13 WR=18.8% (n=16)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=17 Losses=86 WR=16.5% (n=103)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 4339 | Aligned=4008 (92.4%)
- Core≈ 1.00 | Prox≈ 0.65 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.34 | Confidence≈ 0.00
- SL_TF dist: {'-1': 39, '240': 477, '60': 2878, '5': 170, '15': 600, '1440': 175} | SL_Structural≈ 99.1%
- TP_TF dist: {'-1': 2803, '240': 149, '5': 213, '15': 610, '60': 552, '1440': 12} | TP_Structural≈ 35.4%

### SLPick por Bandas y TF
- Bandas: lt8=3712, 8-10=347, 10-12.5=175, 12.5-15=105, >15=0
- TF: 5m=170, 15m=600, 60m=2878, 240m=477, 1440m=175
- RR plan por bandas: 0-10≈ 1.35 (n=4059), 10-15≈ 1.14 (n=280)

## CancelBias (EMA200@60m)
- Eventos: 137
- Distribución Bias: {'Bullish': 118, 'Bearish': 19, 'Neutral': 0}
- Coherencia (Close>EMA): 118/137 (86.1%)

## StructureFusion
- Trazas por zona: 13117 | Zonas con Anchors: 13088
- Dir zonas (zona): Bull=7522 Bear=5595 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.5, DirBear≈ 1.1, DirNeutral≈ 0.0
- Razones de dirección: {'triggers-only': 29, 'anchors+triggers': 12516, 'tie-bias': 572}
- TF Triggers: {'5': 8389, '15': 4728}
- TF Anchors: {'240': 12855, '60': 12976, '1440': 10168}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3748, 'Bearish': 1252, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3748/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,20': 1, 'estructura no existe': 16, 'score decayó a 0,24': 1, 'score decayó a 0,27': 1, 'score decayó a 0,26': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 94 | Ejecutadas: 22 | Canceladas: 0 | Expiradas: 0
- BUY: 88 | SELL: 28

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 2335
- Registered: 47
  - DEDUP_COOLDOWN: 2 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 49

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 2.1%
- RegRate = Registered / Intentos = 95.9%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 4.1%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 46.8%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 9908 | Total candidatos: 251184 | Seleccionados: 9854
- Candidatos por zona (promedio): 25.4
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 85289, 60: 76220, 15: 41889, 1440: 24888, 5: 22898}
- **TF Seleccionados**: {240: 1423, 5: 258, 60: 6168, 15: 863, 1440: 1142}
- **DistATR** - Candidatos: avg=21.9 | Seleccionados: avg=9.8
- **Razones de selección**: {'Fallback<15': 2173, 'InBand[8,15]_TFPreference': 7681}
- **En banda [10,15] ATR**: 42407/251184 (16.9%)

### Take Profit (TP)
- Zonas analizadas: 8404 | Total candidatos: 100019 | Seleccionados: 8404
- Candidatos por zona (promedio): 11.9
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=78
- **Score** - Candidatos: avg=0.53 | Seleccionados: avg=0.26
- **Priority Candidatos**: {'P3': 100019}
- **Priority Seleccionados**: {'P4_Fallback': 5217, 'P3': 3187}
- **Type Candidatos**: {'Swing': 100019}
- **Type Seleccionados**: {'Calculated': 5217, 'Swing': 3187}
- **TF Candidatos**: {5: 23728, 240: 21936, 15: 21232, 60: 17907, 1440: 15216}
- **TF Seleccionados**: {-1: 5217, 240: 798, 5: 50, 60: 51, 1440: 2284, 15: 4}
- **DistATR** - Candidatos: avg=10.5 | Seleccionados: avg=14.7
- **RR** - Candidatos: avg=0.89 | Seleccionados: avg=1.19
- **Razones de selección**: {'NoStructuralTarget': 5217, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 347, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 91, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 303, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 293, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 10, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 8, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 143, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 41, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 174, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 505, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 14, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 16, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 18, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 7, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 23, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 94, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 12, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 2, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of24': 3, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 3, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 1, 'ForcedP3_NoFallback': 1063, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 2, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of18': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 4, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 64% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 75% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.89.