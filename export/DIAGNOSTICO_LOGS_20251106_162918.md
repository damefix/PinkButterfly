# Informe Diagnóstico de Logs - 2025-11-06 16:41:48

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251106_162918.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251106_162918.csv`

## DFM
- Eventos de evaluación: 442
- Evaluaciones Bull: 236 | Bear: 444
- Pasaron umbral (PassedThreshold): 536
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:1, 4:10, 5:65, 6:194, 7:233, 8:174, 9:3

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 1329
- KeptAligned: 582/611 | KeptCounter: 2640/3990
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.134 | AvgProxCounter≈ 0.387
  - AvgDistATRAligned≈ 0.67 | AvgDistATRCounter≈ 1.73
- PreferAligned eventos: 271 | Filtradas contra-bias: 285

### Proximity (Pre-PreferAligned)
- Eventos: 1329
- Aligned pre: 582/3222 | Counter pre: 2640/3222
- AvgProxAligned(pre)≈ 0.134 | AvgDistATRAligned(pre)≈ 0.67

### Proximity Drivers
- Eventos: 1329
- Alineadas: n=582 | BaseProx≈ 0.682 | ZoneATR≈ 5.13 | SizePenalty≈ 0.977 | FinalProx≈ 0.669
- Contra-bias: n=2355 | BaseProx≈ 0.556 | ZoneATR≈ 4.58 | SizePenalty≈ 0.985 | FinalProx≈ 0.548

## Risk
- Eventos: 1110
- Accepted=686 | RejSL=0 | RejTP=145 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 273 (10.6%)
- **P4_FALLBACK:** 2292 (89.4%)
- **FORCED_P3 por TF:**
  - TF5: 28 (10.3%)
  - TF15: 3 (1.1%)
  - TF60: 62 (22.7%)
  - TF240: 179 (65.6%)
  - TF1440: 1 (0.4%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 68 (2.4% del total)
  - Avg Score: 0.61 | Avg R:R: 1.66 | Avg DistATR: 8.35
  - Por TF: TF5=58, TF15=10
- **P0_SWING_LITE:** 191 (6.8% del total)
  - Avg Score: 0.32 | Avg R:R: 1.95 | Avg DistATR: 9.46
  - Por TF: TF15=134, TF60=57


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 17 | Unmatched: 669
- 0-10: Wins=13 Losses=4 WR=76.5% (n=17)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=13 Losses=4 WR=76.5% (n=17)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 686 | Aligned=132 (19.2%)
- Core≈ 1.00 | Prox≈ 0.62 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.31 | Confidence≈ 0.00
- SL_TF dist: {'60': 543, '-1': 79, '15': 37, '5': 19, '1440': 2, '240': 6} | SL_Structural≈ 88.5%
- TP_TF dist: {'-1': 375, '5': 71, '15': 108, '60': 110, '240': 22} | TP_Structural≈ 45.3%

### SLPick por Bandas y TF
- Bandas: lt8=657, 8-10=17, 10-12.5=8, 12.5-15=4, >15=0
- TF: 5m=19, 15m=37, 60m=543, 240m=6, 1440m=2
- RR plan por bandas: 0-10≈ 1.31 (n=674), 10-15≈ 1.19 (n=12)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 4601 | Zonas con Anchors: 4576
- Dir zonas (zona): Bull=2073 Bear=2385 Neutral=143
- Resumen por ciclo (promedios): TotHZ≈ 2.8, WithAnchors≈ 2.7, DirBull≈ 1.2, DirBear≈ 1.4, DirNeutral≈ 0.1
- Razones de dirección: {'triggers-only': 24, 'tie-bias': 143, 'anchors+triggers': 4434}
- TF Triggers: {'5': 2929, '15': 1672}
- TF Anchors: {'60': 4540, '240': 4050, '1440': 2428}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 11}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 74 | Ejecutadas: 8 | Canceladas: 0 | Expiradas: 0
- BUY: 24 | SELL: 58

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 536
- Registered: 37
  - DEDUP_COOLDOWN: 9 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 46

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 8.6%
- RegRate = Registered / Intentos = 80.4%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 19.6%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 21.6%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 2615 | Total candidatos: 51894 | Seleccionados: 2614
- Candidatos por zona (promedio): 19.8
- **Edad (barras)** - Candidatos: med=40, max=150 | Seleccionados: med=37, max=147
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.49
- **TF Candidatos**: {60: 19943, 15: 13240, 240: 10676, 5: 6226, 1440: 1809}
- **TF Seleccionados**: {60: 2146, 5: 41, 15: 208, 240: 106, 1440: 113}
- **DistATR** - Candidatos: avg=13.7 | Seleccionados: avg=8.8
- **Razones de selección**: {'InBand[8,15]_TFPreference': 1692, 'Fallback<15': 922}
- **En banda [10,15] ATR**: 7614/51894 (14.7%)

### Take Profit (TP)
- Zonas analizadas: 2565 | Total candidatos: 28402 | Seleccionados: 2565
- Candidatos por zona (promedio): 11.1
- **Edad (barras)** - Candidatos: med=35, max=150 | Seleccionados: med=0, max=81
- **Score** - Candidatos: avg=0.53 | Seleccionados: avg=0.07
- **Priority Candidatos**: {'P3': 28402}
- **Priority Seleccionados**: {'P3': 273, 'P4_Fallback': 2292}
- **Type Candidatos**: {'Swing': 28402}
- **Type Seleccionados**: {'Swing': 273, 'Calculated': 2292}
- **TF Candidatos**: {5: 7387, 15: 6532, 240: 6479, 60: 6073, 1440: 1931}
- **TF Seleccionados**: {60: 62, -1: 2292, 5: 28, 15: 3, 240: 179, 1440: 1}
- **DistATR** - Candidatos: avg=10.1 | Seleccionados: avg=11.7
- **RR** - Candidatos: avg=0.96 | Seleccionados: avg=1.02
- **Razones de selección**: {'SwingP3_TF>=60_RR>=1.0_Dist>=6': 52, 'NoStructuralTarget': 2292, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 16, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 69, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 3, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 7, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 23, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 2, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 17, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 24, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 9, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 13, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 29, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of11': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 60% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 89% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 0.95.