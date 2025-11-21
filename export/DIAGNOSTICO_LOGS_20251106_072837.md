# Informe Diagnóstico de Logs - 2025-11-06 07:39:19

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251106_072837.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251106_072837.csv`

## DFM
- Eventos de evaluación: 1289
- Evaluaciones Bull: 700 | Bear: 1172
- Pasaron umbral (PassedThreshold): 1353
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:10, 4:52, 5:276, 6:447, 7:504, 8:578, 9:5

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3984
- KeptAligned: 1468/1594 | KeptCounter: 7628/11555
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.133 | AvgProxCounter≈ 0.415
  - AvgDistATRAligned≈ 0.79 | AvgDistATRCounter≈ 1.84
- PreferAligned eventos: 847 | Filtradas contra-bias: 1030

### Proximity (Pre-PreferAligned)
- Eventos: 3984
- Aligned pre: 1468/9096 | Counter pre: 7628/9096
- AvgProxAligned(pre)≈ 0.133 | AvgDistATRAligned(pre)≈ 0.79

### Proximity Drivers
- Eventos: 3984
- Alineadas: n=1468 | BaseProx≈ 0.638 | ZoneATR≈ 5.05 | SizePenalty≈ 0.978 | FinalProx≈ 0.626
- Contra-bias: n=6598 | BaseProx≈ 0.557 | ZoneATR≈ 4.62 | SizePenalty≈ 0.984 | FinalProx≈ 0.549

## Risk
- Eventos: 3452
- Accepted=1885 | RejSL=0 | RejTP=580 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 1056 (16.0%)
- **P4_FALLBACK:** 5531 (84.0%)
- **FORCED_P3 por TF:**
  - TF5: 61 (5.8%)
  - TF15: 2 (0.2%)
  - TF60: 49 (4.6%)
  - TF240: 654 (61.9%)
  - TF1440: 290 (27.5%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 104 (1.4% del total)
  - Avg Score: 0.61 | Avg R:R: 1.53 | Avg DistATR: 10.03
  - Por TF: TF5=80, TF15=24
- **P0_SWING_LITE:** 950 (12.4% del total)
  - Avg Score: 0.30 | Avg R:R: 2.05 | Avg DistATR: 10.33
  - Por TF: TF15=453, TF60=497


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 62 | Unmatched: 1823
- 0-10: Wins=32 Losses=30 WR=51.6% (n=62)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=32 Losses=30 WR=51.6% (n=62)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1885 | Aligned=237 (12.6%)
- Core≈ 1.00 | Prox≈ 0.60 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.45 | Confidence≈ 0.00
- SL_TF dist: {'-1': 122, '5': 53, '60': 1241, '15': 105, '240': 245, '1440': 119} | SL_Structural≈ 93.5%
- TP_TF dist: {'-1': 692, '5': 106, '15': 267, '60': 472, '240': 348} | TP_Structural≈ 63.3%

### SLPick por Bandas y TF
- Bandas: lt8=1783, 8-10=81, 10-12.5=18, 12.5-15=3, >15=0
- TF: 5m=53, 15m=105, 60m=1241, 240m=245, 1440m=119
- RR plan por bandas: 0-10≈ 1.45 (n=1864), 10-15≈ 1.29 (n=21)

## CancelBias (EMA200@60m)
- Eventos: 144
- Distribución Bias: {'Bullish': 49, 'Bearish': 95, 'Neutral': 0}
- Coherencia (Close>EMA): 49/144 (34.0%)

## StructureFusion
- Trazas por zona: 13149 | Zonas con Anchors: 13103
- Dir zonas (zona): Bull=7069 Bear=5499 Neutral=581
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.4, DirBear≈ 1.1, DirNeutral≈ 0.1
- Razones de dirección: {'tie-bias': 581, 'triggers-only': 43, 'anchors+triggers': 12525}
- TF Triggers: {'5': 8473, '15': 4676}
- TF Anchors: {'60': 12998, '240': 12839, '1440': 10399}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 12, 'score decayó a 0,19': 1, 'score decayó a 0,25': 1, 'score decayó a 0,31': 1, 'score decayó a 0,18': 1}
- Cancel_BOS (diag): por acción {'BUY': 6, 'SELL': 19} | por bias {'Bullish': 19, 'Bearish': 6, 'Neutral': 0}

## CSV de Trades
- Filas: 139 | Ejecutadas: 21 | Canceladas: 0 | Expiradas: 0
- BUY: 65 | SELL: 95

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1353
- Registered: 70
  - DEDUP_COOLDOWN: 10 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 80

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 5.9%
- RegRate = Registered / Intentos = 87.5%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 12.5%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 30.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7441 | Total candidatos: 182814 | Seleccionados: 7403
- Candidatos por zona (promedio): 24.6
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 61274, 60: 55352, 15: 31574, 1440: 17688, 5: 16926}
- **TF Seleccionados**: {60: 4847, 5: 205, 15: 634, 1440: 912, 240: 805}
- **DistATR** - Candidatos: avg=20.0 | Seleccionados: avg=9.7
- **Razones de selección**: {'Fallback<15': 1782, 'InBand[8,15]_TFPreference': 5621}
- **En banda [10,15] ATR**: 30122/182814 (16.5%)

### Take Profit (TP)
- Zonas analizadas: 6587 | Total candidatos: 86083 | Seleccionados: 6587
- Candidatos por zona (promedio): 13.1
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=122
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.11
- **Priority Candidatos**: {'P3': 86083}
- **Priority Seleccionados**: {'P4_Fallback': 5531, 'P3': 1056}
- **Type Candidatos**: {'Swing': 86083}
- **Type Seleccionados**: {'Calculated': 5531, 'Swing': 1056}
- **TF Candidatos**: {240: 20514, 5: 17606, 15: 16616, 60: 16505, 1440: 14842}
- **TF Seleccionados**: {-1: 5531, 60: 49, 240: 654, 5: 61, 1440: 290, 15: 2}
- **DistATR** - Candidatos: avg=12.4 | Seleccionados: avg=12.8
- **RR** - Candidatos: avg=1.10 | Seleccionados: avg=1.05
- **Razones de selección**: {'NoStructuralTarget': 5531, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 103, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 229, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 22, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 15, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 65, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 75, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 16, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 199, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 172, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 23, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 38, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 7, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 47, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 3, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 63% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 84% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 0.92.