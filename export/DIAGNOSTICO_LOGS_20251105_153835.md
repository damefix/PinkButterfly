# Informe Diagnóstico de Logs - 2025-11-05 15:56:25

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251105_153835.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251105_153835.csv`

## DFM
- Eventos de evaluación: 1226
- Evaluaciones Bull: 626 | Bear: 1136
- Pasaron umbral (PassedThreshold): 1308
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:11, 4:43, 5:229, 6:410, 7:505, 8:563, 9:1

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
- Accepted=1766 | RejSL=0 | RejTP=686 | RejRR=0 | RejEntry=0
### Risk – Validación Doble Cerrojo (V6.0d)
- **RejSL_Points:** 1630 (rechazados por >60pts)
- **RejTP_Points:** 408 (rechazados por >120pts)
- **RejSL_HighVol:** 0 (rechazados por ATR>15 y DistATR>10)

**Rechazos SL por TF:**

| TF | RejSL_Points |
|----|--------------|
| -1 | 61 |
| 5 | 19 |
| 15 | 21 |
| 60 | 1026 |
| 240 | 172 |
| 1440 | 331 |

**Rechazos TP por TF:**

| TF | RejTP_Points |
|----|--------------|
| -1 | 408 |

### TP Policy (V6.0c)
- **FORCED_P3:** 1210 (18.4%)
- **P4_FALLBACK:** 5356 (81.6%)
- **FORCED_P3 por TF:**
  - TF5: 81 (6.7%)
  - TF60: 54 (4.5%)
  - TF240: 737 (60.9%)
  - TF1440: 338 (27.9%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 176 (2.3% del total)
  - Avg Score: 0.62 | Avg R:R: 1.69 | Avg DistATR: 9.29
  - Por TF: TF5=142, TF15=34
- **P0_SWING_LITE:** 975 (12.6% del total)
  - Avg Score: 0.30 | Avg R:R: 2.22 | Avg DistATR: 10.26
  - Por TF: TF15=514, TF60=461


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 53 | Unmatched: 1713
- 0-10: Wins=26 Losses=27 WR=49.1% (n=53)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=26 Losses=27 WR=49.1% (n=53)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1766 | Aligned=263 (14.9%)
- Core≈ 1.00 | Prox≈ 0.62 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.40 | Confidence≈ 0.00
- SL_TF dist: {'60': 1081, '-1': 116, '240': 327, '5': 59, '15': 93, '1440': 90} | SL_Structural≈ 93.4%
- TP_TF dist: {'-1': 680, '240': 358, '5': 127, '15': 178, '60': 423} | TP_Structural≈ 61.5%

### SLPick por Bandas y TF
- Bandas: lt8=1697, 8-10=62, 10-12.5=5, 12.5-15=2, >15=0
- TF: 5m=59, 15m=93, 60m=1081, 240m=327, 1440m=90
- RR plan por bandas: 0-10≈ 1.40 (n=1759), 10-15≈ 1.58 (n=7)

## CancelBias (EMA200@60m)
- Eventos: 150
- Distribución Bias: {'Bullish': 104, 'Bearish': 46, 'Neutral': 0}
- Coherencia (Close>EMA): 104/150 (69.3%)

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
- Expiraciones: {'estructura no existe': 14, 'score decayó a 0,21': 1, 'score decayó a 0,27': 1, 'score decayó a 0,31': 1, 'score decayó a 0,20': 1, 'score decayó a 0,48': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 32} | por bias {'Bullish': 32, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 169 | Ejecutadas: 24 | Canceladas: 0 | Expiradas: 0
- BUY: 71 | SELL: 122

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1308
- Registered: 85
  - DEDUP_COOLDOWN: 12 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 97

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 7.4%
- RegRate = Registered / Intentos = 87.6%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 12.4%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 28.2%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7511 | Total candidatos: 168936 | Seleccionados: 7437
- Candidatos por zona (promedio): 22.5
- **Edad (barras)** - Candidatos: med=40, max=150 | Seleccionados: med=41, max=150
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.44
- **TF Candidatos**: {60: 55760, 240: 49494, 15: 31946, 5: 17068, 1440: 14668}
- **TF Seleccionados**: {60: 5070, 5: 284, 240: 789, 15: 759, 1440: 535}
- **DistATR** - Candidatos: avg=20.6 | Seleccionados: avg=8.8
- **Razones de selección**: {'Fallback<15': 1786, 'InBand[4,8]_TFPreference': 965, 'InBand[8,15]_TFPreference': 4686}
- **En banda [10,15] ATR**: 26133/168936 (15.5%)

### Take Profit (TP)
- Zonas analizadas: 6566 | Total candidatos: 82265 | Seleccionados: 6566
- Candidatos por zona (promedio): 12.5
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=122
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.12
- **Priority Candidatos**: {'P3': 82265}
- **Priority Seleccionados**: {'P4_Fallback': 5356, 'P3': 1210}
- **Type Candidatos**: {'Swing': 82265}
- **Type Seleccionados**: {'Calculated': 5356, 'Swing': 1210}
- **TF Candidatos**: {240: 19641, 5: 17817, 15: 15631, 60: 15549, 1440: 13627}
- **TF Seleccionados**: {-1: 5356, 240: 737, 5: 81, 60: 54, 1440: 338}
- **DistATR** - Candidatos: avg=11.7 | Seleccionados: avg=12.1
- **RR** - Candidatos: avg=1.15 | Seleccionados: avg=1.06
- **Razones de selección**: {'NoStructuralTarget': 5356, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 265, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 236, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 70, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 221, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 102, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 19, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 12, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 70, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 21, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 25, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 9, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 27, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 43, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 39, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 7, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 3, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 12, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of17': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 64% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 82% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 0.92.