# Informe Diagnóstico de Logs - 2025-11-05 18:31:28

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251105_181644.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251105_181644.csv`

## DFM
- Eventos de evaluación: 1115
- Evaluaciones Bull: 554 | Bear: 1100
- Pasaron umbral (PassedThreshold): 1251
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:6, 4:41, 5:215, 6:356, 7:515, 8:519, 9:2

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3964
- KeptAligned: 1560/1686 | KeptCounter: 7586/11448
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.140 | AvgProxCounter≈ 0.416
  - AvgDistATRAligned≈ 0.77 | AvgDistATRCounter≈ 1.81
- PreferAligned eventos: 865 | Filtradas contra-bias: 1022

### Proximity (Pre-PreferAligned)
- Eventos: 3964
- Aligned pre: 1560/9146 | Counter pre: 7586/9146
- AvgProxAligned(pre)≈ 0.140 | AvgDistATRAligned(pre)≈ 0.77

### Proximity Drivers
- Eventos: 3964
- Alineadas: n=1560 | BaseProx≈ 0.662 | ZoneATR≈ 4.90 | SizePenalty≈ 0.981 | FinalProx≈ 0.651
- Contra-bias: n=6564 | BaseProx≈ 0.563 | ZoneATR≈ 4.60 | SizePenalty≈ 0.984 | FinalProx≈ 0.555

## Risk
- Eventos: 3440
- Accepted=1659 | RejSL=0 | RejTP=555 | RejRR=0 | RejEntry=0
### Risk – Validación Doble Cerrojo (V6.0d)
- **RejSL_Points:** 2019 (rechazados por >60pts)
- **RejTP_Points:** 382 (rechazados por >120pts)
- **RejSL_HighVol:** 0 (rechazados por ATR>15 y DistATR>10)

**Rechazos SL por TF:**

| TF | RejSL_Points |
|----|--------------|
| -1 | 38 |
| 5 | 7 |
| 15 | 10 |
| 60 | 959 |
| 240 | 355 |
| 1440 | 650 |

**Rechazos TP por TF:**

| TF | RejTP_Points |
|----|--------------|
| -1 | 382 |

### TP Policy (V6.0c)
- **FORCED_P3:** 1022 (15.3%)
- **P4_FALLBACK:** 5641 (84.7%)
- **FORCED_P3 por TF:**
  - TF5: 56 (5.5%)
  - TF15: 2 (0.2%)
  - TF60: 49 (4.8%)
  - TF240: 640 (62.6%)
  - TF1440: 275 (26.9%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 107 (1.4% del total)
  - Avg Score: 0.61 | Avg R:R: 1.51 | Avg DistATR: 10.22
  - Por TF: TF5=84, TF15=23
- **P0_SWING_LITE:** 923 (12.0% del total)
  - Avg Score: 0.30 | Avg R:R: 2.07 | Avg DistATR: 10.40
  - Por TF: TF15=445, TF60=478


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 44 | Unmatched: 1615
- 0-10: Wins=23 Losses=21 WR=52.3% (n=44)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=23 Losses=21 WR=52.3% (n=44)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1659 | Aligned=278 (16.8%)
- Core≈ 1.00 | Prox≈ 0.63 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.37 | Confidence≈ 0.00
- SL_TF dist: {'-1': 132, '5': 39, '60': 1096, '15': 69, '240': 225, '1440': 98} | SL_Structural≈ 92.0%
- TP_TF dist: {'-1': 668, '5': 65, '15': 144, '60': 434, '240': 348} | TP_Structural≈ 59.7%

### SLPick por Bandas y TF
- Bandas: lt8=1581, 8-10=69, 10-12.5=6, 12.5-15=3, >15=0
- TF: 5m=39, 15m=69, 60m=1096, 240m=225, 1440m=98
- RR plan por bandas: 0-10≈ 1.37 (n=1650), 10-15≈ 1.56 (n=9)

## CancelBias (EMA200@60m)
- Eventos: 110
- Distribución Bias: {'Bullish': 65, 'Bearish': 45, 'Neutral': 0}
- Coherencia (Close>EMA): 65/110 (59.1%)

## StructureFusion
- Trazas por zona: 13134 | Zonas con Anchors: 13080
- Dir zonas (zona): Bull=7027 Bear=5516 Neutral=591
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.4, DirBear≈ 1.1, DirNeutral≈ 0.1
- Razones de dirección: {'triggers-only': 45, 'tie-bias': 591, 'anchors+triggers': 12498}
- TF Triggers: {'5': 8402, '15': 4732}
- TF Anchors: {'60': 12976, '240': 12677, '1440': 10269}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 11, 'score decayó a 0,21': 1, 'score decayó a 0,27': 1, 'score decayó a 0,31': 1}
- Cancel_BOS (diag): por acción {'BUY': 1, 'SELL': 27} | por bias {'Bullish': 27, 'Bearish': 1, 'Neutral': 0}

## CSV de Trades
- Filas: 135 | Ejecutadas: 20 | Canceladas: 0 | Expiradas: 0
- BUY: 63 | SELL: 92

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1251
- Registered: 68
  - DEDUP_COOLDOWN: 10 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 78

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 6.2%
- RegRate = Registered / Intentos = 87.2%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 12.8%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 29.4%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7490 | Total candidatos: 182475 | Seleccionados: 7455
- Candidatos por zona (promedio): 24.4
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.46
- **TF Candidatos**: {240: 60923, 60: 55528, 15: 31527, 1440: 17578, 5: 16919}
- **TF Seleccionados**: {5: 182, 60: 4937, 15: 621, 1440: 911, 240: 804}
- **DistATR** - Candidatos: avg=20.0 | Seleccionados: avg=9.6
- **Razones de selección**: {'Fallback<15': 1854, 'InBand[8,15]_TFPreference': 5601}
- **En banda [10,15] ATR**: 29942/182475 (16.4%)

### Take Profit (TP)
- Zonas analizadas: 6663 | Total candidatos: 85701 | Seleccionados: 6663
- Candidatos por zona (promedio): 12.9
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=122
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.10
- **Priority Candidatos**: {'P3': 85701}
- **Priority Seleccionados**: {'P4_Fallback': 5641, 'P3': 1022}
- **Type Candidatos**: {'Swing': 85701}
- **Type Seleccionados**: {'Calculated': 5641, 'Swing': 1022}
- **TF Candidatos**: {240: 20232, 5: 18024, 15: 16598, 60: 16383, 1440: 14464}
- **TF Seleccionados**: {-1: 5641, 60: 49, 5: 56, 240: 640, 1440: 275, 15: 2}
- **DistATR** - Candidatos: avg=12.1 | Seleccionados: avg=12.6
- **RR** - Candidatos: avg=1.08 | Seleccionados: avg=1.05
- **Razones de selección**: {'NoStructuralTarget': 5641, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 229, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 172, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 12, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 98, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 60, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 16, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 71, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 195, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 20, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 23, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 37, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 7, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 37, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 3, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 62% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 85% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 0.93.