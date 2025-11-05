# Informe Diagnóstico de Logs - 2025-11-05 08:41:08

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251105_083327.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251105_083327.csv`

## DFM
- Eventos de evaluación: 1725
- Evaluaciones Bull: 2982 | Bear: 221
- Pasaron umbral (PassedThreshold): 1892
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:12, 4:104, 5:607, 6:1207, 7:1265, 8:8, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3932
- KeptAligned: 8945/10008 | KeptCounter: 1790/3005
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.513 | AvgProxCounter≈ 0.138
  - AvgDistATRAligned≈ 2.90 | AvgDistATRCounter≈ 0.64
- PreferAligned eventos: 3178 | Filtradas contra-bias: 876

### Proximity (Pre-PreferAligned)
- Eventos: 3932
- Aligned pre: 8945/10735 | Counter pre: 1790/10735
- AvgProxAligned(pre)≈ 0.513 | AvgDistATRAligned(pre)≈ 2.90

### Proximity Drivers
- Eventos: 3932
- Alineadas: n=8945 | BaseProx≈ 0.666 | ZoneATR≈ 4.70 | SizePenalty≈ 0.982 | FinalProx≈ 0.655
- Contra-bias: n=914 | BaseProx≈ 0.554 | ZoneATR≈ 4.60 | SizePenalty≈ 0.983 | FinalProx≈ 0.546

## Risk
- Eventos: 3617
- Accepted=3232 | RejSL=77 | RejTP=2035 | RejRR=0 | RejEntry=0
### Risk – Validación Doble Cerrojo (V6.0d)
- **RejSL_Points:** 4138 (rechazados por >60pts)
- **RejTP_Points:** 36 (rechazados por >120pts)
- **RejSL_HighVol:** 0 (rechazados por ATR>15 y DistATR>10)

**Rechazos SL por TF:**

| TF | RejSL_Points |
|----|--------------|
| -1 | 9 |
| 5 | 15 |
| 15 | 37 |
| 60 | 2382 |
| 240 | 800 |
| 1440 | 895 |

**Rechazos TP por TF:**

| TF | RejTP_Points |
|----|--------------|
| 1440 | 36 |

### TP Policy (V6.0c)
- **FORCED_P3:** 4676 (47.4%)
- **P4_FALLBACK:** 5183 (52.6%)
- **FORCED_P3 por TF:**
  - TF5: 115 (2.5%)
  - TF15: 9 (0.2%)
  - TF60: 124 (2.7%)
  - TF240: 1165 (24.9%)
  - TF1440: 3263 (69.8%)

### Risk Drivers (Rechazos por SL)
- Alineadas: n=77 | SLDistATR≈ 16.62 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:75,20-25:1,25+:1
- HistSL Counter 0-10:0,10-15:0,15-20:0,20-25:0,25+:0

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 68 | Unmatched: 3164
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
- Muestras: 3232 | Aligned=3172 (98.1%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.10 | Confidence≈ 0.00
- SL_TF dist: {'-1': 25, '5': 131, '15': 502, '240': 227, '60': 2309, '1440': 38} | SL_Structural≈ 99.2%
- TP_TF dist: {'-1': 2773, '5': 98, '240': 253, '60': 86, '1440': 19, '15': 3} | TP_Structural≈ 14.2%

### SLPick por Bandas y TF
- Bandas: lt8=2667, 8-10=321, 10-12.5=147, 12.5-15=97, >15=0
- TF: 5m=131, 15m=502, 60m=2309, 240m=227, 1440m=38
- RR plan por bandas: 0-10≈ 1.11 (n=2988), 10-15≈ 1.01 (n=244)

## CancelBias (EMA200@60m)
- Eventos: 119
- Distribución Bias: {'Bullish': 110, 'Bearish': 9, 'Neutral': 0}
- Coherencia (Close>EMA): 110/119 (92.4%)

## StructureFusion
- Trazas por zona: 13013 | Zonas con Anchors: 12958
- Dir zonas (zona): Bull=7490 Bear=5523 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.5, DirBear≈ 1.1, DirNeutral≈ 0.0
- Razones de dirección: {'triggers-only': 54, 'tie-bias': 564, 'anchors+triggers': 12395}
- TF Triggers: {'5': 8342, '15': 4671}
- TF Anchors: {'240': 12725, '60': 12845, '1440': 10148}

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
- DFM Señales (PassedThreshold): 1892
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
- Zonas analizadas: 9831 | Total candidatos: 250775 | Seleccionados: 9777
- Candidatos por zona (promedio): 25.5
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.44
- **TF Candidatos**: {240: 85174, 60: 76102, 15: 41783, 1440: 24878, 5: 22838}
- **TF Seleccionados**: {5: 266, 15: 869, 240: 1339, 60: 6161, 1440: 1142}
- **DistATR** - Candidatos: avg=21.9 | Seleccionados: avg=9.9
- **Razones de selección**: {'Fallback<15': 2106, 'InBand[8,15]_TFPreference': 7671}
- **En banda [10,15] ATR**: 42406/250775 (16.9%)

### Take Profit (TP)
- Zonas analizadas: 9859 | Total candidatos: 135996 | Seleccionados: 9859
- Candidatos por zona (promedio): 13.8
- **Edad (barras)** - Candidatos: med=34, max=150 | Seleccionados: med=0, max=89
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.34
- **Priority Candidatos**: {'P3': 135996}
- **Priority Seleccionados**: {'P4_Fallback': 5183, 'P3': 4676}
- **Type Candidatos**: {'Swing': 135996}
- **Type Seleccionados**: {'Calculated': 5183, 'Swing': 4676}
- **TF Candidatos**: {240: 30950, 5: 29657, 15: 29237, 60: 26886, 1440: 19266}
- **TF Seleccionados**: {-1: 5183, 5: 115, 240: 1165, 60: 124, 1440: 3263, 15: 9}
- **DistATR** - Candidatos: avg=12.5 | Seleccionados: avg=15.5
- **RR** - Candidatos: avg=1.10 | Seleccionados: avg=1.30
- **Razones de selección**: {'NoStructuralTarget': 5183, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 37, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 304, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 525, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 132, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 228, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 215, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 360, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 347, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 64, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 249, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 19, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 28, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 8, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 7, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 8, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 136, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 191, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 130, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 103, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 223, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of20': 13, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of25': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 54, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of23': 21, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of26': 8, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of24': 12, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of17': 26, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 16, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of18': 26, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 45, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of22': 12, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of21': 19, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of27': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of28': 4, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 2, 'ForcedP3_NoFallback': 1087, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of30': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of29': 2}

### 🎯 Recomendaciones
- ⚠️ SL: 64% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 64% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.89.