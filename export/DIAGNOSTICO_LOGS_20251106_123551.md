# Informe Diagnóstico de Logs - 2025-11-06 12:43:41

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251106_123551.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251106_123551.csv`

## DFM
- Eventos de evaluación: 1307
- Evaluaciones Bull: 744 | Bear: 1188
- Pasaron umbral (PassedThreshold): 1416
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:10, 4:52, 5:278, 6:444, 7:549, 8:594, 9:5

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 4000
- KeptAligned: 1493/1619 | KeptCounter: 7690/11636
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.135 | AvgProxCounter≈ 0.416
  - AvgDistATRAligned≈ 0.79 | AvgDistATRCounter≈ 1.83
- PreferAligned eventos: 859 | Filtradas contra-bias: 1034

### Proximity (Pre-PreferAligned)
- Eventos: 4000
- Aligned pre: 1493/9183 | Counter pre: 7690/9183
- AvgProxAligned(pre)≈ 0.135 | AvgDistATRAligned(pre)≈ 0.79

### Proximity Drivers
- Eventos: 4000
- Alineadas: n=1493 | BaseProx≈ 0.643 | ZoneATR≈ 5.06 | SizePenalty≈ 0.978 | FinalProx≈ 0.630
- Contra-bias: n=6656 | BaseProx≈ 0.561 | ZoneATR≈ 4.60 | SizePenalty≈ 0.984 | FinalProx≈ 0.553

## Risk
- Eventos: 3466
- Accepted=1945 | RejSL=0 | RejTP=600 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 1097 (16.4%)
- **P4_FALLBACK:** 5594 (83.6%)
- **FORCED_P3 por TF:**
  - TF5: 58 (5.3%)
  - TF15: 2 (0.2%)
  - TF60: 46 (4.2%)
  - TF240: 675 (61.5%)
  - TF1440: 316 (28.8%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 103 (1.3% del total)
  - Avg Score: 0.61 | Avg R:R: 1.52 | Avg DistATR: 10.04
  - Por TF: TF5=79, TF15=24
- **P0_SWING_LITE:** 932 (12.1% del total)
  - Avg Score: 0.30 | Avg R:R: 2.06 | Avg DistATR: 10.40
  - Por TF: TF15=452, TF60=480


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 56 | Unmatched: 1889
- 0-10: Wins=26 Losses=27 WR=49.1% (n=53)
- 10-15: Wins=2 Losses=1 WR=66.7% (n=3)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=28 Losses=28 WR=50.0% (n=56)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1945 | Aligned=244 (12.5%)
- Core≈ 1.00 | Prox≈ 0.61 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.44 | Confidence≈ 0.00
- SL_TF dist: {'-1': 126, '5': 64, '240': 265, '60': 1265, '15': 105, '1440': 120} | SL_Structural≈ 93.5%
- TP_TF dist: {'-1': 750, '5': 102, '15': 273, '60': 455, '240': 365} | TP_Structural≈ 61.4%

### SLPick por Bandas y TF
- Bandas: lt8=1834, 8-10=82, 10-12.5=26, 12.5-15=3, >15=0
- TF: 5m=64, 15m=105, 60m=1265, 240m=265, 1440m=120
- RR plan por bandas: 0-10≈ 1.44 (n=1916), 10-15≈ 1.19 (n=29)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 13255 | Zonas con Anchors: 13211
- Dir zonas (zona): Bull=7100 Bear=5572 Neutral=583
- Resumen por ciclo (promedios): TotHZ≈ 2.7, WithAnchors≈ 2.6, DirBull≈ 1.4, DirBear≈ 1.1, DirNeutral≈ 0.1
- Razones de dirección: {'triggers-only': 41, 'tie-bias': 583, 'anchors+triggers': 12631}
- TF Triggers: {'5': 8507, '15': 4748}
- TF Anchors: {'240': 12976, '60': 13094, '1440': 10458}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 21}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 248 | Ejecutadas: 27 | Canceladas: 0 | Expiradas: 0
- BUY: 176 | SELL: 99

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1416
- Registered: 124
  - DEDUP_COOLDOWN: 38 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 162

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 11.4%
- RegRate = Registered / Intentos = 76.5%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 23.5%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 21.8%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7533 | Total candidatos: 183540 | Seleccionados: 7495
- Candidatos por zona (promedio): 24.4
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 61540, 60: 55386, 15: 31692, 1440: 17737, 5: 17185}
- **TF Seleccionados**: {5: 225, 240: 847, 60: 4866, 15: 645, 1440: 912}
- **DistATR** - Candidatos: avg=20.0 | Seleccionados: avg=9.6
- **Razones de selección**: {'Fallback<15': 1848, 'InBand[8,15]_TFPreference': 5647}
- **En banda [10,15] ATR**: 30218/183540 (16.5%)

### Take Profit (TP)
- Zonas analizadas: 6691 | Total candidatos: 87030 | Seleccionados: 6691
- Candidatos por zona (promedio): 13.0
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=122
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.11
- **Priority Candidatos**: {'P3': 87030}
- **Priority Seleccionados**: {'P4_Fallback': 5594, 'P3': 1097}
- **Type Candidatos**: {'Swing': 87030}
- **Type Seleccionados**: {'Calculated': 5594, 'Swing': 1097}
- **TF Candidatos**: {240: 20773, 5: 17688, 60: 16751, 15: 16713, 1440: 15105}
- **TF Seleccionados**: {-1: 5594, 240: 675, 5: 58, 60: 46, 1440: 316, 15: 2}
- **DistATR** - Candidatos: avg=12.5 | Seleccionados: avg=12.8
- **RR** - Candidatos: avg=1.10 | Seleccionados: avg=1.05
- **Razones de selección**: {'NoStructuralTarget': 5594, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 180, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 21, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 232, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 13, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 77, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 69, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 16, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 211, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 116, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 23, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 38, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 7, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 49, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 3, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 63% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 84% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.92.