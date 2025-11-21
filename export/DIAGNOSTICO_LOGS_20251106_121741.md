# Informe Diagnóstico de Logs - 2025-11-06 12:25:01

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251106_121741.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251106_121741.csv`

## DFM
- Eventos de evaluación: 817
- Evaluaciones Bull: 305 | Bear: 824
- Pasaron umbral (PassedThreshold): 690
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:14, 4:51, 5:254, 6:257, 7:263, 8:288, 9:2

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 4001
- KeptAligned: 1495/1621 | KeptCounter: 7689/11631
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.135 | AvgProxCounter≈ 0.416
  - AvgDistATRAligned≈ 0.79 | AvgDistATRCounter≈ 1.83
- PreferAligned eventos: 861 | Filtradas contra-bias: 1035

### Proximity (Pre-PreferAligned)
- Eventos: 4001
- Aligned pre: 1495/9184 | Counter pre: 7689/9184
- AvgProxAligned(pre)≈ 0.135 | AvgDistATRAligned(pre)≈ 0.79

### Proximity Drivers
- Eventos: 4001
- Alineadas: n=1495 | BaseProx≈ 0.643 | ZoneATR≈ 5.06 | SizePenalty≈ 0.978 | FinalProx≈ 0.631
- Contra-bias: n=6654 | BaseProx≈ 0.561 | ZoneATR≈ 4.60 | SizePenalty≈ 0.984 | FinalProx≈ 0.553

## Risk
- Eventos: 3467
- Accepted=1131 | RejSL=0 | RejTP=743 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 1127 (17.2%)
- **P4_FALLBACK:** 5416 (82.8%)
- **FORCED_P3 por TF:**
  - TF5: 47 (4.2%)
  - TF60: 15 (1.3%)
  - TF240: 562 (49.9%)
  - TF1440: 503 (44.6%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 114 (1.5% del total)
  - Avg Score: 0.61 | Avg R:R: 1.52 | Avg DistATR: 9.91
  - Por TF: TF5=87, TF15=27
- **P0_SWING_LITE:** 1067 (13.8% del total)
  - Avg Score: 0.31 | Avg R:R: 2.35 | Avg DistATR: 10.58
  - Por TF: TF15=417, TF60=650


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 47 | Unmatched: 1084
- 0-10: Wins=18 Losses=29 WR=38.3% (n=47)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=18 Losses=29 WR=38.3% (n=47)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1131 | Aligned=144 (12.7%)
- Core≈ 1.00 | Prox≈ 0.55 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.82 | Confidence≈ 0.00
- SL_TF dist: {'60': 741, '5': 13, '-1': 65, '15': 44, '240': 161, '1440': 107} | SL_Structural≈ 94.3%
- TP_TF dist: {'5': 98, '15': 277, '60': 477, '240': 279} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1099, 8-10=21, 10-12.5=9, 12.5-15=2, >15=0
- TF: 5m=13, 15m=44, 60m=741, 240m=161, 1440m=107
- RR plan por bandas: 0-10≈ 1.82 (n=1120), 10-15≈ 1.73 (n=11)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 13252 | Zonas con Anchors: 13210
- Dir zonas (zona): Bull=7097 Bear=5570 Neutral=585
- Resumen por ciclo (promedios): TotHZ≈ 2.7, WithAnchors≈ 2.6, DirBull≈ 1.4, DirBear≈ 1.1, DirNeutral≈ 0.1
- Razones de dirección: {'triggers-only': 40, 'tie-bias': 585, 'anchors+triggers': 12627}
- TF Triggers: {'5': 8504, '15': 4748}
- TF Anchors: {'240': 12975, '60': 13093, '1440': 10454}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 21}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 170 | Ejecutadas: 19 | Canceladas: 0 | Expiradas: 0
- BUY: 65 | SELL: 124

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 690
- Registered: 85
  - DEDUP_COOLDOWN: 14 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 99

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 14.3%
- RegRate = Registered / Intentos = 85.9%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 14.1%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 22.4%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7528 | Total candidatos: 183521 | Seleccionados: 7490
- Candidatos por zona (promedio): 24.4
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 61535, 60: 55384, 15: 31686, 1440: 17743, 5: 17173}
- **TF Seleccionados**: {5: 223, 240: 844, 60: 4867, 15: 644, 1440: 912}
- **DistATR** - Candidatos: avg=20.0 | Seleccionados: avg=9.6
- **Razones de selección**: {'Fallback<15': 1843, 'InBand[8,15]_TFPreference': 5647}
- **En banda [10,15] ATR**: 30216/183521 (16.5%)

### Take Profit (TP)
- Zonas analizadas: 6543 | Total candidatos: 83586 | Seleccionados: 6543
- Candidatos por zona (promedio): 12.8
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=107
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.12
- **Priority Candidatos**: {'P3': 83586}
- **Priority Seleccionados**: {'P4_Fallback': 5416, 'P3': 1127}
- **Type Candidatos**: {'Swing': 83586}
- **Type Seleccionados**: {'Calculated': 5416, 'Swing': 1127}
- **TF Candidatos**: {240: 19760, 5: 17245, 15: 16193, 60: 15797, 1440: 14591}
- **TF Seleccionados**: {-1: 5416, 240: 562, 5: 47, 60: 15, 1440: 503}
- **DistATR** - Candidatos: avg=12.2 | Seleccionados: avg=13.0
- **RR** - Candidatos: avg=1.06 | Seleccionados: avg=1.07
- **Razones de selección**: {'NoStructuralTarget': 5416, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 130, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 19, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 227, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 8, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 86, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 72, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 30, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 236, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 137, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 15, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 67, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 6, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 56, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 3, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 3, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 63% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 83% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 0.92.