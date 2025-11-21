# Informe Diagnóstico de Logs - 2025-11-06 19:17:48

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251106_191217.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251106_191217.csv`

## DFM
- Eventos de evaluación: 1276
- Evaluaciones Bull: 710 | Bear: 1154
- Pasaron umbral (PassedThreshold): 1352
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:9, 4:52, 5:277, 6:434, 7:501, 8:586, 9:5

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 4002
- KeptAligned: 1499/1628 | KeptCounter: 7710/11666
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.136 | AvgProxCounter≈ 0.417
  - AvgDistATRAligned≈ 0.80 | AvgDistATRCounter≈ 1.82
- PreferAligned eventos: 871 | Filtradas contra-bias: 1080

### Proximity (Pre-PreferAligned)
- Eventos: 4002
- Aligned pre: 1499/9209 | Counter pre: 7710/9209
- AvgProxAligned(pre)≈ 0.136 | AvgDistATRAligned(pre)≈ 0.80

### Proximity Drivers
- Eventos: 4002
- Alineadas: n=1499 | BaseProx≈ 0.639 | ZoneATR≈ 5.06 | SizePenalty≈ 0.978 | FinalProx≈ 0.627
- Contra-bias: n=6630 | BaseProx≈ 0.563 | ZoneATR≈ 4.60 | SizePenalty≈ 0.984 | FinalProx≈ 0.554

## Risk
- Eventos: 3463
- Accepted=1873 | RejSL=0 | RejTP=604 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 1102 (16.5%)
- **P4_FALLBACK:** 5586 (83.5%)
- **FORCED_P3 por TF:**
  - TF5: 63 (5.7%)
  - TF15: 2 (0.2%)
  - TF60: 64 (5.8%)
  - TF240: 658 (59.7%)
  - TF1440: 315 (28.6%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 96 (1.2% del total)
  - Avg Score: 0.61 | Avg R:R: 1.53 | Avg DistATR: 9.92
  - Por TF: TF5=72, TF15=24
- **P0_SWING_LITE:** 914 (11.9% del total)
  - Avg Score: 0.30 | Avg R:R: 2.07 | Avg DistATR: 10.47
  - Por TF: TF15=452, TF60=462


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 50 | Unmatched: 1823
- 0-10: Wins=24 Losses=24 WR=50.0% (n=48)
- 10-15: Wins=2 Losses=0 WR=100.0% (n=2)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=26 Losses=24 WR=52.0% (n=50)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1873 | Aligned=245 (13.1%)
- Core≈ 1.00 | Prox≈ 0.61 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.45 | Confidence≈ 0.00
- SL_TF dist: {'15': 113, '-1': 107, '60': 1236, '5': 51, '240': 248, '1440': 118} | SL_Structural≈ 94.3%
- TP_TF dist: {'-1': 700, '5': 101, '15': 275, '60': 453, '240': 344} | TP_Structural≈ 62.6%

### SLPick por Bandas y TF
- Bandas: lt8=1771, 8-10=79, 10-12.5=17, 12.5-15=6, >15=0
- TF: 5m=51, 15m=113, 60m=1236, 240m=248, 1440m=118
- RR plan por bandas: 0-10≈ 1.45 (n=1850), 10-15≈ 1.23 (n=23)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 13294 | Zonas con Anchors: 13259
- Dir zonas (zona): Bull=7066 Bear=5624 Neutral=604
- Resumen por ciclo (promedios): TotHZ≈ 2.7, WithAnchors≈ 2.7, DirBull≈ 1.4, DirBear≈ 1.1, DirNeutral≈ 0.1
- Razones de dirección: {'tie-bias': 604, 'triggers-only': 31, 'anchors+triggers': 12659}
- TF Triggers: {'5': 8544, '15': 4750}
- TF Anchors: {'60': 13155, '240': 12854, '1440': 10564}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 19}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 200 | Ejecutadas: 22 | Canceladas: 0 | Expiradas: 0
- BUY: 112 | SELL: 110

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1352
- Registered: 100
  - DEDUP_COOLDOWN: 25 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 125

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 9.2%
- RegRate = Registered / Intentos = 80.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 20.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 22.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7528 | Total candidatos: 182536 | Seleccionados: 7490
- Candidatos por zona (promedio): 24.2
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 60608, 60: 55390, 15: 31503, 1440: 17841, 5: 17194}
- **TF Seleccionados**: {15: 656, 60: 4850, 5: 215, 1440: 937, 240: 832}
- **DistATR** - Candidatos: avg=19.8 | Seleccionados: avg=9.7
- **Razones de selección**: {'Fallback<15': 1809, 'InBand[8,15]_TFPreference': 5681}
- **En banda [10,15] ATR**: 30279/182536 (16.6%)

### Take Profit (TP)
- Zonas analizadas: 6688 | Total candidatos: 87975 | Seleccionados: 6688
- Candidatos por zona (promedio): 13.2
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=122
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.11
- **Priority Candidatos**: {'P3': 87975}
- **Priority Seleccionados**: {'P4_Fallback': 5586, 'P3': 1102}
- **Type Candidatos**: {'Swing': 87975}
- **Type Seleccionados**: {'Calculated': 5586, 'Swing': 1102}
- **TF Candidatos**: {240: 20805, 5: 17790, 60: 16986, 15: 16714, 1440: 15680}
- **TF Seleccionados**: {-1: 5586, 5: 63, 60: 64, 240: 658, 1440: 315, 15: 2}
- **DistATR** - Candidatos: avg=12.6 | Seleccionados: avg=12.8
- **RR** - Candidatos: avg=1.10 | Seleccionados: avg=1.05
- **Razones de selección**: {'NoStructuralTarget': 5586, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 24, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 6, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 3, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 13, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 240, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 75, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 68, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 16, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 197, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 186, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 117, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 24, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 38, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 7, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 49, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 3, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 63% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 84% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.92.