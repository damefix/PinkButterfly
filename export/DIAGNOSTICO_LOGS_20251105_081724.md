# Informe Diagnóstico de Logs - 2025-11-05 08:24:00

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251105_081724.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251105_081724.csv`

## DFM
- Eventos de evaluación: 1363
- Evaluaciones Bull: 2082 | Bear: 164
- Pasaron umbral (PassedThreshold): 1786
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:11, 4:48, 5:401, 6:874, 7:906, 8:6, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3933
- KeptAligned: 8938/10000 | KeptCounter: 1790/3008
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.513 | AvgProxCounter≈ 0.138
  - AvgDistATRAligned≈ 2.91 | AvgDistATRCounter≈ 0.64
- PreferAligned eventos: 3180 | Filtradas contra-bias: 876

### Proximity (Pre-PreferAligned)
- Eventos: 3933
- Aligned pre: 8938/10728 | Counter pre: 1790/10728
- AvgProxAligned(pre)≈ 0.513 | AvgDistATRAligned(pre)≈ 2.91

### Proximity Drivers
- Eventos: 3933
- Alineadas: n=8938 | BaseProx≈ 0.666 | ZoneATR≈ 4.69 | SizePenalty≈ 0.982 | FinalProx≈ 0.655
- Contra-bias: n=914 | BaseProx≈ 0.554 | ZoneATR≈ 4.60 | SizePenalty≈ 0.983 | FinalProx≈ 0.546

## Risk
- Eventos: 3619
- Accepted=2270 | RejSL=77 | RejTP=2036 | RejRR=1024 | RejEntry=0
### Risk – Validación Doble Cerrojo (V6.0d)
- **RejSL_Points:** 4137 (rechazados por >60pts)
- **RejTP_Points:** 36 (rechazados por >120pts)
- **RejSL_HighVol:** 0 (rechazados por ATR>15 y DistATR>10)

**Rechazos SL por TF:**

| TF | RejSL_Points |
|----|--------------|
| -1 | 9 |
| 5 | 15 |
| 15 | 37 |
| 60 | 2381 |
| 240 | 800 |
| 1440 | 895 |

**Rechazos TP por TF:**

| TF | RejTP_Points |
|----|--------------|
| 1440 | 36 |

### TP Policy (V6.0c)
- **FORCED_P3:** 4675 (47.5%)
- **P4_FALLBACK:** 5177 (52.5%)
- **FORCED_P3 por TF:**
  - TF5: 114 (2.4%)
  - TF15: 9 (0.2%)
  - TF60: 124 (2.7%)
  - TF240: 1166 (24.9%)
  - TF1440: 3262 (69.8%)

### Risk Drivers (Rechazos por SL)
- Alineadas: n=77 | SLDistATR≈ 16.62 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:75,20-25:1,25+:1
- HistSL Counter 0-10:0,10-15:0,15-20:0,20-25:0,25+:0

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 53 | Unmatched: 2217
- 0-10: Wins=4 Losses=47 WR=7.8% (n=51)
- 10-15: Wins=1 Losses=1 WR=50.0% (n=2)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=5 Losses=48 WR=9.4% (n=53)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2270 | Aligned=2221 (97.8%)
- Core≈ 1.00 | Prox≈ 0.68 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.57 | Confidence≈ 0.00
- SL_TF dist: {'-1': 15, '5': 95, '15': 350, '240': 164, '60': 1619, '1440': 27} | SL_Structural≈ 99.3%
- TP_TF dist: {'-1': 2070, '240': 127, '5': 16, '60': 38, '1440': 19} | TP_Structural≈ 8.8%

### SLPick por Bandas y TF
- Bandas: lt8=1851, 8-10=235, 10-12.5=115, 12.5-15=69, >15=0
- TF: 5m=95, 15m=350, 60m=1619, 240m=164, 1440m=27
- RR plan por bandas: 0-10≈ 1.58 (n=2086), 10-15≈ 1.51 (n=184)

## CancelBias (EMA200@60m)
- Eventos: 110
- Distribución Bias: {'Bullish': 108, 'Bearish': 2, 'Neutral': 0}
- Coherencia (Close>EMA): 108/110 (98.2%)

## StructureFusion
- Trazas por zona: 13008 | Zonas con Anchors: 12952
- Dir zonas (zona): Bull=7493 Bear=5515 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.5, DirBear≈ 1.1, DirNeutral≈ 0.0
- Razones de dirección: {'triggers-only': 54, 'tie-bias': 564, 'anchors+triggers': 12390}
- TF Triggers: {'5': 8339, '15': 4669}
- TF Anchors: {'240': 12719, '60': 12841, '1440': 10146}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3748, 'Bearish': 1252, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3748/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 12, 'score decayó a 0,33': 1, 'score decayó a 0,20': 1, 'score decayó a 0,18': 1, 'score decayó a 0,32': 1, 'score decayó a 0,50': 1}

## CSV de Trades
- Filas: 68 | Ejecutadas: 14 | Canceladas: 0 | Expiradas: 0
- BUY: 75 | SELL: 7

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1786
- Registered: 34
  - DEDUP_COOLDOWN: 1 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 35

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 2.0%
- RegRate = Registered / Intentos = 97.1%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 2.9%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 41.2%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 9822 | Total candidatos: 250696 | Seleccionados: 9768
- Candidatos por zona (promedio): 25.5
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.44
- **TF Candidatos**: {240: 85161, 60: 76088, 15: 41755, 1440: 24876, 5: 22816}
- **TF Seleccionados**: {5: 265, 15: 866, 240: 1335, 60: 6160, 1440: 1142}
- **DistATR** - Candidatos: avg=21.9 | Seleccionados: avg=9.9
- **Razones de selección**: {'Fallback<15': 2100, 'InBand[8,15]_TFPreference': 7668}
- **En banda [10,15] ATR**: 42396/250696 (16.9%)

### Take Profit (TP)
- Zonas analizadas: 9852 | Total candidatos: 135955 | Seleccionados: 9852
- Candidatos por zona (promedio): 13.8
- **Edad (barras)** - Candidatos: med=34, max=150 | Seleccionados: med=0, max=89
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.34
- **Priority Candidatos**: {'P3': 135955}
- **Priority Seleccionados**: {'P4_Fallback': 5177, 'P3': 4675}
- **Type Candidatos**: {'Swing': 135955}
- **Type Seleccionados**: {'Calculated': 5177, 'Swing': 4675}
- **TF Candidatos**: {240: 30944, 5: 29642, 15: 29228, 60: 26884, 1440: 19257}
- **TF Seleccionados**: {-1: 5177, 240: 1166, 5: 114, 60: 124, 1440: 3262, 15: 9}
- **DistATR** - Candidatos: avg=12.5 | Seleccionados: avg=18.8
- **RR** - Candidatos: avg=1.10 | Seleccionados: avg=1.57
- **Razones de selección**: {'NoStructuralTarget': 5177, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 305, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 525, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 132, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 228, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 215, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 359, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 347, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 64, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 249, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 19, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 36, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 28, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 8, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 7, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 8, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 136, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 191, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 130, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 103, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 223, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of20': 13, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of25': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 54, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of23': 21, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of26': 8, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of24': 12, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of17': 26, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 16, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of18': 26, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 45, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of22': 12, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of21': 19, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of27': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of28': 4, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 2, 'ForcedP3_NoFallback': 1087, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of30': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of29': 2}

### 🎯 Recomendaciones
- ⚠️ SL: 64% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 64% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.89.