# Informe Diagnóstico de Logs - 2025-11-06 19:10:43

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251106_190611.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251106_190611.csv`

## DFM
- Eventos de evaluación: 1277
- Evaluaciones Bull: 711 | Bear: 1156
- Pasaron umbral (PassedThreshold): 1356
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:9, 4:53, 5:275, 6:433, 7:503, 8:589, 9:5

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 4002
- KeptAligned: 1503/1632 | KeptCounter: 7707/11661
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.136 | AvgProxCounter≈ 0.417
  - AvgDistATRAligned≈ 0.80 | AvgDistATRCounter≈ 1.82
- PreferAligned eventos: 872 | Filtradas contra-bias: 1079

### Proximity (Pre-PreferAligned)
- Eventos: 4002
- Aligned pre: 1503/9210 | Counter pre: 7707/9210
- AvgProxAligned(pre)≈ 0.136 | AvgDistATRAligned(pre)≈ 0.80

### Proximity Drivers
- Eventos: 4002
- Alineadas: n=1503 | BaseProx≈ 0.640 | ZoneATR≈ 5.06 | SizePenalty≈ 0.978 | FinalProx≈ 0.627
- Contra-bias: n=6628 | BaseProx≈ 0.563 | ZoneATR≈ 4.60 | SizePenalty≈ 0.984 | FinalProx≈ 0.555

## Risk
- Eventos: 3464
- Accepted=1876 | RejSL=0 | RejTP=603 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 1102 (16.5%)
- **P4_FALLBACK:** 5579 (83.5%)
- **FORCED_P3 por TF:**
  - TF5: 64 (5.8%)
  - TF15: 3 (0.3%)
  - TF60: 64 (5.8%)
  - TF240: 656 (59.5%)
  - TF1440: 315 (28.6%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 99 (1.3% del total)
  - Avg Score: 0.61 | Avg R:R: 1.53 | Avg DistATR: 9.95
  - Por TF: TF5=75, TF15=24
- **P0_SWING_LITE:** 919 (11.9% del total)
  - Avg Score: 0.30 | Avg R:R: 2.07 | Avg DistATR: 10.46
  - Por TF: TF15=455, TF60=464


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 49 | Unmatched: 1827
- 0-10: Wins=24 Losses=23 WR=51.1% (n=47)
- 10-15: Wins=2 Losses=0 WR=100.0% (n=2)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=26 Losses=23 WR=53.1% (n=49)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1876 | Aligned=247 (13.2%)
- Core≈ 1.00 | Prox≈ 0.61 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.45 | Confidence≈ 0.00
- SL_TF dist: {'15': 113, '-1': 107, '60': 1238, '5': 51, '240': 249, '1440': 118} | SL_Structural≈ 94.3%
- TP_TF dist: {'-1': 696, '5': 103, '15': 279, '60': 455, '240': 343} | TP_Structural≈ 62.9%

### SLPick por Bandas y TF
- Bandas: lt8=1775, 8-10=78, 10-12.5=17, 12.5-15=6, >15=0
- TF: 5m=51, 15m=113, 60m=1238, 240m=249, 1440m=118
- RR plan por bandas: 0-10≈ 1.45 (n=1853), 10-15≈ 1.23 (n=23)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 13293 | Zonas con Anchors: 13258
- Dir zonas (zona): Bull=7068 Bear=5621 Neutral=604
- Resumen por ciclo (promedios): TotHZ≈ 2.7, WithAnchors≈ 2.7, DirBull≈ 1.4, DirBear≈ 1.1, DirNeutral≈ 0.1
- Razones de dirección: {'tie-bias': 604, 'triggers-only': 31, 'anchors+triggers': 12658}
- TF Triggers: {'5': 8546, '15': 4747}
- TF Anchors: {'60': 13154, '240': 12853, '1440': 10563}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 19}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 202 | Ejecutadas: 21 | Canceladas: 0 | Expiradas: 0
- BUY: 113 | SELL: 110

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1356
- Registered: 101
  - DEDUP_COOLDOWN: 25 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 126

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 9.3%
- RegRate = Registered / Intentos = 80.2%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 19.8%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 20.8%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7529 | Total candidatos: 182630 | Seleccionados: 7491
- Candidatos por zona (promedio): 24.3
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 60634, 60: 55401, 15: 31514, 1440: 17850, 5: 17231}
- **TF Seleccionados**: {15: 656, 60: 4849, 5: 216, 1440: 938, 240: 832}
- **DistATR** - Candidatos: avg=19.8 | Seleccionados: avg=9.7
- **Razones de selección**: {'Fallback<15': 1809, 'InBand[8,15]_TFPreference': 5682}
- **En banda [10,15] ATR**: 30300/182630 (16.6%)

### Take Profit (TP)
- Zonas analizadas: 6681 | Total candidatos: 87879 | Seleccionados: 6681
- Candidatos por zona (promedio): 13.2
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=122
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.11
- **Priority Candidatos**: {'P3': 87879}
- **Priority Seleccionados**: {'P4_Fallback': 5579, 'P3': 1102}
- **Type Candidatos**: {'Swing': 87879}
- **Type Seleccionados**: {'Calculated': 5579, 'Swing': 1102}
- **TF Candidatos**: {240: 20788, 5: 17751, 60: 16976, 15: 16694, 1440: 15670}
- **TF Seleccionados**: {-1: 5579, 5: 64, 60: 64, 240: 656, 1440: 315, 15: 3}
- **DistATR** - Candidatos: avg=12.5 | Seleccionados: avg=12.8
- **RR** - Candidatos: avg=1.10 | Seleccionados: avg=1.05
- **Razones de selección**: {'NoStructuralTarget': 5579, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 24, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 6, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 3, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 14, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 239, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 74, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 68, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 16, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 195, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 187, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 117, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 25, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 39, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 8, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 49, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 3, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 63% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 84% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.92.