# Informe Diagnóstico de Logs - 2025-11-05 11:52:56

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251105_114618.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251105_114618.csv`

## DFM
- Eventos de evaluación: 2244
- Evaluaciones Bull: 2385 | Bear: 1509
- Pasaron umbral (PassedThreshold): 2968
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:9, 4:69, 5:383, 6:1071, 7:1171, 8:1177, 9:14

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3957
- KeptAligned: 1554/1688 | KeptCounter: 7568/11421
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.137 | AvgProxCounter≈ 0.415
  - AvgDistATRAligned≈ 0.76 | AvgDistATRCounter≈ 1.81
- PreferAligned eventos: 847 | Filtradas contra-bias: 975

### Proximity (Pre-PreferAligned)
- Eventos: 3957
- Aligned pre: 1554/9122 | Counter pre: 7568/9122
- AvgProxAligned(pre)≈ 0.137 | AvgDistATRAligned(pre)≈ 0.76

### Proximity Drivers
- Eventos: 3957
- Alineadas: n=1554 | BaseProx≈ 0.650 | ZoneATR≈ 4.92 | SizePenalty≈ 0.980 | FinalProx≈ 0.639
- Contra-bias: n=6593 | BaseProx≈ 0.563 | ZoneATR≈ 4.60 | SizePenalty≈ 0.984 | FinalProx≈ 0.555

## Risk
- Eventos: 3436
- Accepted=3908 | RejSL=54 | RejTP=641 | RejRR=0 | RejEntry=0
### Risk – Validación Doble Cerrojo (V6.0d)
- **RejSL_Points:** 2014 (rechazados por >60pts)
- **RejTP_Points:** 384 (rechazados por >120pts)
- **RejSL_HighVol:** 0 (rechazados por ATR>15 y DistATR>10)

**Rechazos SL por TF:**

| TF | RejSL_Points |
|----|--------------|
| -1 | 37 |
| 5 | 8 |
| 15 | 11 |
| 60 | 971 |
| 240 | 388 |
| 1440 | 599 |

**Rechazos TP por TF:**

| TF | RejTP_Points |
|----|--------------|
| -1 | 384 |

### TP Policy (V6.0c)
- **FORCED_P3:** 1110 (16.5%)
- **P4_FALLBACK:** 5610 (83.5%)
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

### Risk Drivers (Rechazos por SL)
- Alineadas: n=12 | SLDistATR≈ 16.18 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=42 | SLDistATR≈ 16.40 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:12,20-25:0,25+:0
- HistSL Counter 0-10:0,10-15:0,15-20:41,20-25:0,25+:1

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 76 | Unmatched: 3832
- 0-10: Wins=30 Losses=43 WR=41.1% (n=73)
- 10-15: Wins=2 Losses=1 WR=66.7% (n=3)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=32 Losses=44 WR=42.1% (n=76)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 3908 | Aligned=609 (15.6%)
- Core≈ 1.00 | Prox≈ 0.62 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.30 | Confidence≈ 0.00
- SL_TF dist: {'240': 490, '60': 2631, '5': 90, '-1': 126, '15': 403, '1440': 168} | SL_Structural≈ 96.8%
- TP_TF dist: {'-1': 2437, '240': 361, '5': 145, '15': 462, '60': 503} | TP_Structural≈ 37.6%

### SLPick por Bandas y TF
- Bandas: lt8=3285, 8-10=362, 10-12.5=183, 12.5-15=78, >15=0
- TF: 5m=90, 15m=403, 60m=2631, 240m=490, 1440m=168
- RR plan por bandas: 0-10≈ 1.32 (n=3647), 10-15≈ 1.15 (n=261)

## CancelBias (EMA200@60m)
- Eventos: 188
- Distribución Bias: {'Bullish': 163, 'Bearish': 25, 'Neutral': 0}
- Coherencia (Close>EMA): 163/188 (86.7%)

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
- Expiraciones: {'estructura no existe': 10, 'score decayó a 0,32': 2, 'score decayó a 0,18': 2, 'score decayó a 0,27': 1, 'score decayó a 0,46': 1, 'score decayó a 0,47': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 25} | por bias {'Bullish': 25, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 135 | Ejecutadas: 22 | Canceladas: 0 | Expiradas: 0
- BUY: 62 | SELL: 95

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 2968
- Registered: 68
  - DEDUP_COOLDOWN: 8 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 76

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 2.6%
- RegRate = Registered / Intentos = 89.5%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 10.5%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 32.4%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7578 | Total candidatos: 184921 | Seleccionados: 7539
- Candidatos por zona (promedio): 24.4
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 62502, 60: 55882, 15: 32018, 1440: 17438, 5: 17081}
- **TF Seleccionados**: {240: 1091, 5: 180, 60: 4785, 15: 629, 1440: 854}
- **DistATR** - Candidatos: avg=20.1 | Seleccionados: avg=9.5
- **Razones de selección**: {'Fallback<15': 1922, 'InBand[8,15]_TFPreference': 5617}
- **En banda [10,15] ATR**: 30121/184921 (16.3%)

### Take Profit (TP)
- Zonas analizadas: 6720 | Total candidatos: 86057 | Seleccionados: 6720
- Candidatos por zona (promedio): 12.8
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=122
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.11
- **Priority Candidatos**: {'P3': 86057}
- **Priority Seleccionados**: {'P4_Fallback': 5610, 'P3': 1110}
- **Type Candidatos**: {'Swing': 86057}
- **Type Seleccionados**: {'Calculated': 5610, 'Swing': 1110}
- **TF Candidatos**: {240: 20520, 5: 18337, 15: 16518, 60: 16403, 1440: 14279}
- **TF Seleccionados**: {-1: 5610, 240: 729, 5: 56, 60: 46, 1440: 278, 15: 1}
- **DistATR** - Candidatos: avg=12.0 | Seleccionados: avg=12.7
- **RR** - Candidatos: avg=1.08 | Seleccionados: avg=1.05
- **Razones de selección**: {'NoStructuralTarget': 5610, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 241, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 236, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 63, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 197, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 100, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 12, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 8, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 66, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 17, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 21, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 23, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 39, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 40, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 7, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 3, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 62% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 83% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.92.