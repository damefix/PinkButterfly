# Informe Diagnóstico de Logs - 2025-11-05 20:51:25

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251105_204514.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251105_204514.csv`

## DFM
- Eventos de evaluación: 1115
- Evaluaciones Bull: 549 | Bear: 1097
- Pasaron umbral (PassedThreshold): 1242
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:6, 4:40, 5:218, 6:354, 7:512, 8:514, 9:2

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3965
- KeptAligned: 1549/1675 | KeptCounter: 7585/11481
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.139 | AvgProxCounter≈ 0.416
  - AvgDistATRAligned≈ 0.76 | AvgDistATRCounter≈ 1.81
- PreferAligned eventos: 859 | Filtradas contra-bias: 1019

### Proximity (Pre-PreferAligned)
- Eventos: 3965
- Aligned pre: 1549/9134 | Counter pre: 7585/9134
- AvgProxAligned(pre)≈ 0.139 | AvgDistATRAligned(pre)≈ 0.76

### Proximity Drivers
- Eventos: 3965
- Alineadas: n=1549 | BaseProx≈ 0.662 | ZoneATR≈ 4.90 | SizePenalty≈ 0.981 | FinalProx≈ 0.651
- Contra-bias: n=6566 | BaseProx≈ 0.563 | ZoneATR≈ 4.60 | SizePenalty≈ 0.984 | FinalProx≈ 0.555

## Risk
- Eventos: 3437
- Accepted=1653 | RejSL=0 | RejTP=557 | RejRR=0 | RejEntry=0
### Risk – Validación Doble Cerrojo (V6.0d)
- **RejSL_Points:** 2022 (rechazados por >60pts)
- **RejTP_Points:** 382 (rechazados por >120pts)
- **RejSL_HighVol:** 0 (rechazados por ATR>15 y DistATR>10)

**Rechazos SL por TF:**

| TF | RejSL_Points |
|----|--------------|
| -1 | 38 |
| 5 | 7 |
| 15 | 10 |
| 60 | 960 |
| 240 | 355 |
| 1440 | 652 |

**Rechazos TP por TF:**

| TF | RejTP_Points |
|----|--------------|
| -1 | 382 |

### TP Policy (V6.0c)
- **FORCED_P3:** 1025 (15.4%)
- **P4_FALLBACK:** 5634 (84.6%)
- **FORCED_P3 por TF:**
  - TF5: 56 (5.5%)
  - TF15: 2 (0.2%)
  - TF60: 49 (4.8%)
  - TF240: 642 (62.6%)
  - TF1440: 276 (26.9%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 107 (1.4% del total)
  - Avg Score: 0.61 | Avg R:R: 1.51 | Avg DistATR: 10.25
  - Por TF: TF5=84, TF15=23
- **P0_SWING_LITE:** 929 (12.1% del total)
  - Avg Score: 0.30 | Avg R:R: 2.06 | Avg DistATR: 10.39
  - Por TF: TF15=451, TF60=478


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 51 | Unmatched: 1602
- 0-10: Wins=26 Losses=25 WR=51.0% (n=51)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=26 Losses=25 WR=51.0% (n=51)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1653 | Aligned=277 (16.8%)
- Core≈ 1.00 | Prox≈ 0.64 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.37 | Confidence≈ 0.00
- SL_TF dist: {'-1': 101, '5': 39, '60': 1120, '15': 69, '240': 225, '1440': 99} | SL_Structural≈ 93.9%
- TP_TF dist: {'-1': 658, '15': 149, '60': 434, '5': 64, '240': 348} | TP_Structural≈ 60.2%

### SLPick por Bandas y TF
- Bandas: lt8=1583, 8-10=61, 10-12.5=6, 12.5-15=3, >15=0
- TF: 5m=39, 15m=69, 60m=1120, 240m=225, 1440m=99
- RR plan por bandas: 0-10≈ 1.37 (n=1644), 10-15≈ 1.56 (n=9)

## CancelBias (EMA200@60m)
- Eventos: 110
- Distribución Bias: {'Bullish': 79, 'Bearish': 31, 'Neutral': 0}
- Coherencia (Close>EMA): 79/110 (71.8%)

## StructureFusion
- Trazas por zona: 13156 | Zonas con Anchors: 13124
- Dir zonas (zona): Bull=7035 Bear=5543 Neutral=578
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.4, DirBear≈ 1.1, DirNeutral≈ 0.1
- Razones de dirección: {'triggers-only': 32, 'anchors+triggers': 12546, 'tie-bias': 578}
- TF Triggers: {'15': 4729, '5': 8427}
- TF Anchors: {'60': 13020, '240': 12723, '1440': 10315}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 10, 'score decayó a 0,32': 1, 'score decayó a 0,36': 1, 'score decayó a 0,31': 2, 'score decayó a 0,27': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 28} | por bias {'Bullish': 28, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 147 | Ejecutadas: 22 | Canceladas: 0 | Expiradas: 0
- BUY: 63 | SELL: 106

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1242
- Registered: 74
  - DEDUP_COOLDOWN: 11 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 85

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 6.8%
- RegRate = Registered / Intentos = 87.1%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 12.9%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 29.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7523 | Total candidatos: 182625 | Seleccionados: 7488
- Candidatos por zona (promedio): 24.3
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.46
- **TF Candidatos**: {240: 60977, 60: 55582, 15: 31561, 1440: 17648, 5: 16857}
- **TF Seleccionados**: {5: 182, 60: 4967, 15: 621, 1440: 914, 240: 804}
- **DistATR** - Candidatos: avg=20.0 | Seleccionados: avg=9.5
- **Razones de selección**: {'Fallback<15': 1874, 'InBand[8,15]_TFPreference': 5614}
- **En banda [10,15] ATR**: 29993/182625 (16.4%)

### Take Profit (TP)
- Zonas analizadas: 6659 | Total candidatos: 85772 | Seleccionados: 6659
- Candidatos por zona (promedio): 12.9
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=122
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.10
- **Priority Candidatos**: {'P3': 85772}
- **Priority Seleccionados**: {'P4_Fallback': 5634, 'P3': 1025}
- **Type Candidatos**: {'Swing': 85772}
- **Type Seleccionados**: {'Calculated': 5634, 'Swing': 1025}
- **TF Candidatos**: {240: 20267, 5: 18013, 15: 16587, 60: 16426, 1440: 14479}
- **TF Seleccionados**: {-1: 5634, 60: 49, 5: 56, 240: 642, 1440: 276, 15: 2}
- **DistATR** - Candidatos: avg=12.1 | Seleccionados: avg=12.6
- **RR** - Candidatos: avg=1.08 | Seleccionados: avg=1.05
- **Razones de selección**: {'NoStructuralTarget': 5634, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 229, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 172, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 12, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 99, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 61, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 16, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 71, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 195, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 20, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 23, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 38, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 7, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 37, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 3, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 62% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 85% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 0.92.