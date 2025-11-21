# Informe Diagnóstico de Logs - 2025-11-06 07:54:17

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251106_074924.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251106_074924.csv`

## DFM
- Eventos de evaluación: 1291
- Evaluaciones Bull: 699 | Bear: 1185
- Pasaron umbral (PassedThreshold): 1364
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:10, 4:52, 5:278, 6:446, 7:519, 8:574, 9:5

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3985
- KeptAligned: 1469/1595 | KeptCounter: 7638/11569
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.133 | AvgProxCounter≈ 0.414
  - AvgDistATRAligned≈ 0.79 | AvgDistATRCounter≈ 1.84
- PreferAligned eventos: 848 | Filtradas contra-bias: 1034

### Proximity (Pre-PreferAligned)
- Eventos: 3985
- Aligned pre: 1469/9107 | Counter pre: 7638/9107
- AvgProxAligned(pre)≈ 0.133 | AvgDistATRAligned(pre)≈ 0.79

### Proximity Drivers
- Eventos: 3985
- Alineadas: n=1469 | BaseProx≈ 0.638 | ZoneATR≈ 5.06 | SizePenalty≈ 0.978 | FinalProx≈ 0.626
- Contra-bias: n=6604 | BaseProx≈ 0.557 | ZoneATR≈ 4.62 | SizePenalty≈ 0.984 | FinalProx≈ 0.549

## Risk
- Eventos: 3451
- Accepted=1896 | RejSL=0 | RejTP=582 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 1058 (16.0%)
- **P4_FALLBACK:** 5538 (84.0%)
- **FORCED_P3 por TF:**
  - TF5: 61 (5.8%)
  - TF15: 2 (0.2%)
  - TF60: 48 (4.5%)
  - TF240: 654 (61.8%)
  - TF1440: 293 (27.7%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 104 (1.4% del total)
  - Avg Score: 0.61 | Avg R:R: 1.53 | Avg DistATR: 10.03
  - Por TF: TF5=80, TF15=24
- **P0_SWING_LITE:** 950 (12.4% del total)
  - Avg Score: 0.30 | Avg R:R: 2.05 | Avg DistATR: 10.33
  - Por TF: TF15=453, TF60=497


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 50 | Unmatched: 1846
- 0-10: Wins=26 Losses=22 WR=54.2% (n=48)
- 10-15: Wins=2 Losses=0 WR=100.0% (n=2)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=28 Losses=22 WR=56.0% (n=50)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1896 | Aligned=237 (12.5%)
- Core≈ 1.00 | Prox≈ 0.60 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.45 | Confidence≈ 0.00
- SL_TF dist: {'-1': 125, '60': 1248, '5': 52, '15': 105, '240': 246, '1440': 120} | SL_Structural≈ 93.4%
- TP_TF dist: {'-1': 696, '5': 106, '15': 274, '60': 472, '240': 348} | TP_Structural≈ 63.3%

### SLPick por Bandas y TF
- Bandas: lt8=1796, 8-10=79, 10-12.5=18, 12.5-15=3, >15=0
- TF: 5m=52, 15m=105, 60m=1248, 240m=246, 1440m=120
- RR plan por bandas: 0-10≈ 1.45 (n=1875), 10-15≈ 1.29 (n=21)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 13164 | Zonas con Anchors: 13120
- Dir zonas (zona): Bull=7079 Bear=5505 Neutral=580
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.4, DirBear≈ 1.1, DirNeutral≈ 0.1
- Razones de dirección: {'triggers-only': 43, 'tie-bias': 580, 'anchors+triggers': 12541}
- TF Triggers: {'5': 8476, '15': 4688}
- TF Anchors: {'60': 13015, '240': 12852, '1440': 10403}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 15, 'score decayó a 0,20': 1, 'score decayó a 0,36': 1, 'score decayó a 0,25': 1, 'score decayó a 0,18': 1}
- Cancel_BOS (diag): por acción {'BUY': 4, 'SELL': 15} | por bias {'Bullish': 15, 'Bearish': 4, 'Neutral': 0}

## CSV de Trades
- Filas: 123 | Ejecutadas: 19 | Canceladas: 0 | Expiradas: 0
- BUY: 57 | SELL: 85

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1364
- Registered: 62
  - DEDUP_COOLDOWN: 5 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 67

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 4.9%
- RegRate = Registered / Intentos = 92.5%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 7.5%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 30.6%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7444 | Total candidatos: 182880 | Seleccionados: 7406
- Candidatos por zona (promedio): 24.6
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 61298, 60: 55380, 15: 31567, 1440: 17691, 5: 16944}
- **TF Seleccionados**: {60: 4850, 5: 203, 15: 634, 1440: 912, 240: 807}
- **DistATR** - Candidatos: avg=20.0 | Seleccionados: avg=9.7
- **Razones de selección**: {'Fallback<15': 1784, 'InBand[8,15]_TFPreference': 5622}
- **En banda [10,15] ATR**: 30128/182880 (16.5%)

### Take Profit (TP)
- Zonas analizadas: 6596 | Total candidatos: 86156 | Seleccionados: 6596
- Candidatos por zona (promedio): 13.1
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=122
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.11
- **Priority Candidatos**: {'P3': 86156}
- **Priority Seleccionados**: {'P4_Fallback': 5538, 'P3': 1058}
- **Type Candidatos**: {'Swing': 86156}
- **Type Seleccionados**: {'Calculated': 5538, 'Swing': 1058}
- **TF Candidatos**: {240: 20533, 5: 17610, 15: 16623, 60: 16521, 1440: 14869}
- **TF Seleccionados**: {-1: 5538, 60: 48, 240: 654, 5: 61, 1440: 293, 15: 2}
- **DistATR** - Candidatos: avg=12.4 | Seleccionados: avg=12.8
- **RR** - Candidatos: avg=1.10 | Seleccionados: avg=1.05
- **Razones de selección**: {'NoStructuralTarget': 5538, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 103, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 230, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 22, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 15, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 65, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 75, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 16, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 199, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 172, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 23, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 38, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 7, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 48, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 3, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 63% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 84% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 0.92.