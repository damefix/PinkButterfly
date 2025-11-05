# Informe Diagnóstico de Logs - 2025-11-05 15:09:57

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251105_143621.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251105_143621.csv`

## DFM
- Eventos de evaluación: 1111
- Evaluaciones Bull: 574 | Bear: 1020
- Pasaron umbral (PassedThreshold): 1195
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:6, 4:40, 5:209, 6:360, 7:454, 8:523, 9:2

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3957
- KeptAligned: 1532/1660 | KeptCounter: 7571/11449
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.136 | AvgProxCounter≈ 0.415
  - AvgDistATRAligned≈ 0.76 | AvgDistATRCounter≈ 1.81
- PreferAligned eventos: 843 | Filtradas contra-bias: 975

### Proximity (Pre-PreferAligned)
- Eventos: 3957
- Aligned pre: 1532/9103 | Counter pre: 7571/9103
- AvgProxAligned(pre)≈ 0.136 | AvgDistATRAligned(pre)≈ 0.76

### Proximity Drivers
- Eventos: 3957
- Alineadas: n=1532 | BaseProx≈ 0.656 | ZoneATR≈ 4.92 | SizePenalty≈ 0.980 | FinalProx≈ 0.644
- Contra-bias: n=6596 | BaseProx≈ 0.563 | ZoneATR≈ 4.60 | SizePenalty≈ 0.984 | FinalProx≈ 0.555

## Risk
- Eventos: 3435
- Accepted=1598 | RejSL=0 | RejTP=635 | RejRR=0 | RejEntry=0
### Risk – Validación Doble Cerrojo (V6.0d)
- **RejSL_Points:** 1995 (rechazados por >60pts)
- **RejTP_Points:** 384 (rechazados por >120pts)
- **RejSL_HighVol:** 0 (rechazados por ATR>15 y DistATR>10)

**Rechazos SL por TF:**

| TF | RejSL_Points |
|----|--------------|
| -1 | 37 |
| 5 | 8 |
| 15 | 10 |
| 60 | 953 |
| 240 | 388 |
| 1440 | 599 |

**Rechazos TP por TF:**

| TF | RejTP_Points |
|----|--------------|
| -1 | 384 |

### TP Policy (V6.0c)
- **FORCED_P3:** 1110 (16.6%)
- **P4_FALLBACK:** 5591 (83.4%)
- **FORCED_P3 por TF:**
  - TF5: 56 (5.0%)
  - TF15: 1 (0.1%)
  - TF60: 46 (4.1%)
  - TF240: 729 (65.7%)
  - TF1440: 278 (25.0%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 114 (1.5% del total)
  - Avg Score: 0.61 | Avg R:R: 1.53 | Avg DistATR: 10.10
  - Por TF: TF5=89, TF15=25
- **P0_SWING_LITE:** 902 (11.7% del total)
  - Avg Score: 0.30 | Avg R:R: 2.07 | Avg DistATR: 10.39
  - Por TF: TF15=441, TF60=461


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 42 | Unmatched: 1556
- 0-10: Wins=22 Losses=20 WR=52.4% (n=42)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=22 Losses=20 WR=52.4% (n=42)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1598 | Aligned=226 (14.1%)
- Core≈ 1.00 | Prox≈ 0.63 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.40 | Confidence≈ 0.00
- SL_TF dist: {'240': 366, '60': 935, '5': 37, '-1': 92, '15': 71, '1440': 97} | SL_Structural≈ 94.2%
- TP_TF dist: {'-1': 601, '240': 361, '5': 70, '15': 146, '60': 420} | TP_Structural≈ 62.4%

### SLPick por Bandas y TF
- Bandas: lt8=1529, 8-10=62, 10-12.5=5, 12.5-15=2, >15=0
- TF: 5m=37, 15m=71, 60m=935, 240m=366, 1440m=97
- RR plan por bandas: 0-10≈ 1.40 (n=1591), 10-15≈ 1.58 (n=7)

## CancelBias (EMA200@60m)
- Eventos: 92
- Distribución Bias: {'Bullish': 65, 'Bearish': 27, 'Neutral': 0}
- Coherencia (Close>EMA): 65/92 (70.7%)

## StructureFusion
- Trazas por zona: 13109 | Zonas con Anchors: 13087
- Dir zonas (zona): Bull=7149 Bear=5375 Neutral=585
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.4, DirBear≈ 1.1, DirNeutral≈ 0.1
- Razones de dirección: {'tie-bias': 585, 'anchors+triggers': 12502, 'triggers-only': 22}
- TF Triggers: {'15': 4729, '5': 8380}
- TF Anchors: {'240': 12854, '60': 12975, '1440': 10178}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 11, 'score decayó a 0,21': 1, 'score decayó a 0,27': 1, 'score decayó a 0,31': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 27} | por bias {'Bullish': 27, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 131 | Ejecutadas: 19 | Canceladas: 0 | Expiradas: 0
- BUY: 61 | SELL: 89

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1195
- Registered: 66
  - DEDUP_COOLDOWN: 10 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 76

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 6.4%
- RegRate = Registered / Intentos = 86.8%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 13.2%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 28.8%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7559 | Total candidatos: 184551 | Seleccionados: 7520
- Candidatos por zona (promedio): 24.4
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 62349, 60: 55760, 15: 31946, 1440: 17428, 5: 17068}
- **TF Seleccionados**: {240: 1091, 5: 180, 60: 4767, 15: 628, 1440: 854}
- **DistATR** - Candidatos: avg=20.1 | Seleccionados: avg=9.5
- **Razones de selección**: {'Fallback<15': 1905, 'InBand[8,15]_TFPreference': 5615}
- **En banda [10,15] ATR**: 30079/184551 (16.3%)

### Take Profit (TP)
- Zonas analizadas: 6701 | Total candidatos: 85795 | Seleccionados: 6701
- Candidatos por zona (promedio): 12.8
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=122
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.11
- **Priority Candidatos**: {'P3': 85795}
- **Priority Seleccionados**: {'P4_Fallback': 5591, 'P3': 1110}
- **Type Candidatos**: {'Swing': 85795}
- **Type Seleccionados**: {'Calculated': 5591, 'Swing': 1110}
- **TF Candidatos**: {240: 20468, 5: 18247, 15: 16483, 60: 16388, 1440: 14209}
- **TF Seleccionados**: {-1: 5591, 240: 729, 5: 56, 60: 46, 1440: 278, 15: 1}
- **DistATR** - Candidatos: avg=12.0 | Seleccionados: avg=12.7
- **RR** - Candidatos: avg=1.08 | Seleccionados: avg=1.05
- **Razones de selección**: {'NoStructuralTarget': 5591, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 241, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 236, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 63, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 197, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 100, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 12, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 8, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 66, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 17, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 21, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 23, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 39, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 40, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 7, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 3, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 62% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 83% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 0.92.