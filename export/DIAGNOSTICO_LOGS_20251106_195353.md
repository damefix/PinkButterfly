# Informe Diagnóstico de Logs - 2025-11-06 19:58:56

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251106_195353.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251106_195353.csv`

## DFM
- Eventos de evaluación: 1286
- Evaluaciones Bull: 724 | Bear: 1157
- Pasaron umbral (PassedThreshold): 1365
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:10, 4:52, 5:278, 6:434, 7:517, 8:585, 9:5

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 4004
- KeptAligned: 1506/1643 | KeptCounter: 7722/11669
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.137 | AvgProxCounter≈ 0.418
  - AvgDistATRAligned≈ 0.80 | AvgDistATRCounter≈ 1.82
- PreferAligned eventos: 876 | Filtradas contra-bias: 1099

### Proximity (Pre-PreferAligned)
- Eventos: 4004
- Aligned pre: 1506/9228 | Counter pre: 7722/9228
- AvgProxAligned(pre)≈ 0.137 | AvgDistATRAligned(pre)≈ 0.80

### Proximity Drivers
- Eventos: 4004
- Alineadas: n=1506 | BaseProx≈ 0.641 | ZoneATR≈ 5.04 | SizePenalty≈ 0.979 | FinalProx≈ 0.629
- Contra-bias: n=6623 | BaseProx≈ 0.563 | ZoneATR≈ 4.61 | SizePenalty≈ 0.984 | FinalProx≈ 0.555

## Risk
- Eventos: 3465
- Accepted=1892 | RejSL=0 | RejTP=601 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 1093 (16.4%)
- **P4_FALLBACK:** 5582 (83.6%)
- **FORCED_P3 por TF:**
  - TF5: 43 (3.9%)
  - TF15: 23 (2.1%)
  - TF60: 61 (5.6%)
  - TF240: 654 (59.8%)
  - TF1440: 312 (28.5%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 98 (1.3% del total)
  - Avg Score: 0.61 | Avg R:R: 1.54 | Avg DistATR: 9.89
  - Por TF: TF5=75, TF15=23
- **P0_SWING_LITE:** 922 (12.0% del total)
  - Avg Score: 0.30 | Avg R:R: 2.07 | Avg DistATR: 10.45
  - Por TF: TF15=456, TF60=466


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 51 | Unmatched: 1841
- 0-10: Wins=25 Losses=25 WR=50.0% (n=50)
- 10-15: Wins=1 Losses=0 WR=100.0% (n=1)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=26 Losses=25 WR=51.0% (n=51)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1892 | Aligned=256 (13.5%)
- Core≈ 1.00 | Prox≈ 0.61 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.45 | Confidence≈ 0.00
- SL_TF dist: {'-1': 112, '15': 147, '60': 778, '5': 22, '240': 435, '1440': 398} | SL_Structural≈ 94.1%
- TP_TF dist: {'-1': 701, '15': 304, '5': 89, '60': 455, '240': 343} | TP_Structural≈ 62.9%

### SLPick por Bandas y TF
- Bandas: lt8=1800, 8-10=75, 10-12.5=16, 12.5-15=1, >15=0
- TF: 5m=22, 15m=147, 60m=778, 240m=435, 1440m=398
- RR plan por bandas: 0-10≈ 1.45 (n=1875), 10-15≈ 1.22 (n=17)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 13312 | Zonas con Anchors: 13284
- Dir zonas (zona): Bull=7089 Bear=5622 Neutral=601
- Resumen por ciclo (promedios): TotHZ≈ 2.7, WithAnchors≈ 2.7, DirBull≈ 1.4, DirBear≈ 1.1, DirNeutral≈ 0.1
- Razones de dirección: {'triggers-only': 28, 'anchors+triggers': 12683, 'tie-bias': 601}
- TF Triggers: {'15': 4760, '5': 8552}
- TF Anchors: {'60': 13180, '240': 12880, '1440': 10594}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 19}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 196 | Ejecutadas: 22 | Canceladas: 0 | Expiradas: 0
- BUY: 116 | SELL: 102

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1365
- Registered: 98
  - DEDUP_COOLDOWN: 31 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 129

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 9.5%
- RegRate = Registered / Intentos = 76.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 24.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 22.4%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7520 | Total candidatos: 182544 | Seleccionados: 7482
- Candidatos por zona (promedio): 24.3
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=27, max=150
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.54
- **TF Candidatos**: {240: 60637, 60: 55411, 15: 31482, 1440: 17836, 5: 17178}
- **TF Seleccionados**: {15: 756, 60: 3878, 5: 105, 240: 923, 1440: 1820}
- **DistATR** - Candidatos: avg=19.8 | Seleccionados: avg=9.7
- **Razones de selección**: {'Fallback<15': 1802, 'InBand[8,15]_TFPreference': 5680}
- **En banda [10,15] ATR**: 30240/182544 (16.6%)

### Take Profit (TP)
- Zonas analizadas: 6675 | Total candidatos: 87628 | Seleccionados: 6675
- Candidatos por zona (promedio): 13.1
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=122
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.11
- **Priority Candidatos**: {'P3': 87628}
- **Priority Seleccionados**: {'P4_Fallback': 5582, 'P3': 1093}
- **Type Candidatos**: {'Swing': 87628}
- **Type Seleccionados**: {'Calculated': 5582, 'Swing': 1093}
- **TF Candidatos**: {240: 20731, 5: 17689, 60: 16923, 15: 16645, 1440: 15640}
- **TF Seleccionados**: {-1: 5582, 15: 23, 5: 43, 60: 61, 240: 654, 1440: 312}
- **DistATR** - Candidatos: avg=12.6 | Seleccionados: avg=12.8
- **RR** - Candidatos: avg=1.10 | Seleccionados: avg=1.05
- **Razones de selección**: {'NoStructuralTarget': 5582, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 25, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 8, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 6, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 13, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 236, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 74, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 67, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 14, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 192, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 188, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 116, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 25, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 39, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 50, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 3, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 51% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 84% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.92.