# Informe Diagnóstico de Logs - 2025-11-06 18:10:14

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251106_180201.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251106_180201.csv`

## DFM
- Eventos de evaluación: 2252
- Evaluaciones Bull: 1223 | Bear: 2049
- Pasaron umbral (PassedThreshold): 2368
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:18, 4:92, 5:478, 6:750, 7:875, 8:1026, 9:9

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 6907
- KeptAligned: 2529/2755 | KeptCounter: 13240/20138
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.132 | AvgProxCounter≈ 0.416
  - AvgDistATRAligned≈ 0.79 | AvgDistATRCounter≈ 1.83
- PreferAligned eventos: 1511 | Filtradas contra-bias: 1855

### Proximity (Pre-PreferAligned)
- Eventos: 7056
- Aligned pre: 2650/16277 | Counter pre: 13627/16277
- AvgProxAligned(pre)≈ 0.135 | AvgDistATRAligned(pre)≈ 0.81

### Proximity Drivers
- Eventos: 6854
- Alineadas: n=2539 | BaseProx≈ 0.639 | ZoneATR≈ 5.08 | SizePenalty≈ 0.978 | FinalProx≈ 0.627
- Contra-bias: n=11411 | BaseProx≈ 0.560 | ZoneATR≈ 4.60 | SizePenalty≈ 0.984 | FinalProx≈ 0.552

## Risk
- Eventos: 6005
- Accepted=3242 | RejSL=0 | RejTP=1044 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 1956 (16.7%)
- **P4_FALLBACK:** 9759 (83.3%)
- **FORCED_P3 por TF:**
  - TF5: 101 (5.2%)
  - TF15: 3 (0.2%)
  - TF60: 118 (6.0%)
  - TF240: 1168 (59.7%)
  - TF1440: 566 (28.9%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 185 (1.4% del total)
  - Avg Score: 0.61 | Avg R:R: 1.52 | Avg DistATR: 10.11
  - Por TF: TF5=141, TF15=44
- **P0_SWING_LITE:** 1582 (11.7% del total)
  - Avg Score: 0.30 | Avg R:R: 2.07 | Avg DistATR: 10.44
  - Por TF: TF15=794, TF60=788


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 95 | Unmatched: 3170
- 0-10: Wins=51 Losses=40 WR=56.0% (n=91)
- 10-15: Wins=3 Losses=1 WR=75.0% (n=4)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=54 Losses=41 WR=56.8% (n=95)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 3265 | Aligned=421 (12.9%)
- Core≈ 1.00 | Prox≈ 0.61 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.44 | Confidence≈ 0.00
- SL_TF dist: {'-1': 202, '5': 114, '60': 2126, '15': 183, '240': 429, '1440': 211} | SL_Structural≈ 93.8%
- TP_TF dist: {'-1': 1227, '5': 184, '60': 783, '15': 478, '240': 593} | TP_Structural≈ 62.4%

### SLPick por Bandas y TF
- Bandas: lt8=3036, 8-10=146, 10-12.5=40, 12.5-15=10, >15=0
- TF: 5m=107, 15m=184, 60m=2103, 240m=421, 1440m=209
- RR plan por bandas: 0-10≈ 1.44 (n=3189), 10-15≈ 1.17 (n=50)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 23441 | Zonas con Anchors: 23377
- Dir zonas (zona): Bull=12487 Bear=9908 Neutral=1046
- Resumen por ciclo (promedios): TotHZ≈ 2.7, WithAnchors≈ 2.7, DirBull≈ 1.4, DirBear≈ 1.1, DirNeutral≈ 0.1
- Razones de dirección: {'triggers-only': 63, 'tie-bias': 1046, 'anchors+triggers': 22332}
- TF Triggers: {'5': 14932, '15': 8282}
- TF Anchors: {'60': 22965, '240': 22446, '1440': 18487}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 40}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 4} | por bias {'Bullish': 4, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 507 | Ejecutadas: 53 | Canceladas: 0 | Expiradas: 0
- BUY: 342 | SELL: 218

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 2368
- Registered: 218
  - DEDUP_COOLDOWN: 68 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 286

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 12.1%
- RegRate = Registered / Intentos = 76.2%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 23.8%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 24.3%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 13138 | Total candidatos: 318065 | Seleccionados: 12891
- Candidatos por zona (promedio): 24.2
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 103899, 60: 94976, 15: 54026, 1440: 30763, 5: 29565}
- **TF Seleccionados**: {5: 411, 60: 8358, 15: 1128, 1440: 1599, 240: 1395}
- **DistATR** - Candidatos: avg=19.9 | Seleccionados: avg=9.7
- **Razones de selección**: {'Fallback<15': 3109, 'InBand[8,15]_TFPreference': 9782}
- **En banda [10,15] ATR**: 51853/313229 (16.6%)

### Take Profit (TP)
- Zonas analizadas: 11693 | Total candidatos: 153751 | Seleccionados: 11584
- Candidatos por zona (promedio): 13.1
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=122
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.11
- **Priority Candidatos**: {'P3': 150513}
- **Priority Seleccionados**: {'P4_Fallback': 9651, 'P3': 1933}
- **Type Candidatos**: {'Swing': 150513}
- **Type Seleccionados**: {'Calculated': 9651, 'Swing': 1933}
- **TF Candidatos**: {240: 35617, 5: 30457, 60: 29093, 15: 28677, 1440: 26669}
- **TF Seleccionados**: {-1: 9651, 5: 107, 60: 113, 240: 1160, 1440: 549, 15: 4}
- **DistATR** - Candidatos: avg=12.6 | Seleccionados: avg=12.8
- **RR** - Candidatos: avg=1.11 | Seleccionados: avg=1.05
- **Razones de selección**: {'NoStructuralTarget': 9651, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 40, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 429, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 24, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 134, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 118, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 30, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 348, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 333, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 197, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 10, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 40, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 62, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 13, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 8, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 9, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 9, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 7, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 87, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 11, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 3, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 4, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 9, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 2, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 2}

### 🎯 Recomendaciones
- ⚠️ SL: 63% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 83% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.92.