# Informe Diagnóstico de Logs - 2025-11-06 09:03:39

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251106_085040.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251106_085040.csv`

## DFM
- Eventos de evaluación: 1290
- Evaluaciones Bull: 710 | Bear: 1181
- Pasaron umbral (PassedThreshold): 1370
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:10, 4:52, 5:278, 6:448, 7:514, 8:584, 9:5

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3993
- KeptAligned: 1476/1602 | KeptCounter: 7644/11572
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.133 | AvgProxCounter≈ 0.413
  - AvgDistATRAligned≈ 0.79 | AvgDistATRCounter≈ 1.84
- PreferAligned eventos: 852 | Filtradas contra-bias: 1022

### Proximity (Pre-PreferAligned)
- Eventos: 3993
- Aligned pre: 1476/9120 | Counter pre: 7644/9120
- AvgProxAligned(pre)≈ 0.133 | AvgDistATRAligned(pre)≈ 0.79

### Proximity Drivers
- Eventos: 3993
- Alineadas: n=1476 | BaseProx≈ 0.638 | ZoneATR≈ 5.08 | SizePenalty≈ 0.978 | FinalProx≈ 0.626
- Contra-bias: n=6622 | BaseProx≈ 0.558 | ZoneATR≈ 4.62 | SizePenalty≈ 0.984 | FinalProx≈ 0.550

## Risk
- Eventos: 3462
- Accepted=1897 | RejSL=0 | RejTP=587 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 1063 (16.1%)
- **P4_FALLBACK:** 5559 (83.9%)
- **FORCED_P3 por TF:**
  - TF5: 59 (5.6%)
  - TF15: 2 (0.2%)
  - TF60: 50 (4.7%)
  - TF240: 655 (61.6%)
  - TF1440: 297 (27.9%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 104 (1.4% del total)
  - Avg Score: 0.61 | Avg R:R: 1.52 | Avg DistATR: 10.04
  - Por TF: TF5=80, TF15=24
- **P0_SWING_LITE:** 949 (12.4% del total)
  - Avg Score: 0.30 | Avg R:R: 2.05 | Avg DistATR: 10.34
  - Por TF: TF15=452, TF60=497


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 43 | Unmatched: 1854
- 0-10: Wins=19 Losses=21 WR=47.5% (n=40)
- 10-15: Wins=2 Losses=1 WR=66.7% (n=3)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=21 Losses=22 WR=48.8% (n=43)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1897 | Aligned=240 (12.7%)
- Core≈ 1.00 | Prox≈ 0.60 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.45 | Confidence≈ 0.00
- SL_TF dist: {'60': 1262, '-1': 111, '5': 53, '15': 105, '240': 246, '1440': 120} | SL_Structural≈ 94.1%
- TP_TF dist: {'-1': 700, '5': 104, '15': 273, '60': 472, '240': 348} | TP_Structural≈ 63.1%

### SLPick por Bandas y TF
- Bandas: lt8=1794, 8-10=80, 10-12.5=20, 12.5-15=3, >15=0
- TF: 5m=53, 15m=105, 60m=1262, 240m=246, 1440m=120
- RR plan por bandas: 0-10≈ 1.45 (n=1874), 10-15≈ 1.26 (n=23)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 13174 | Zonas con Anchors: 13143
- Dir zonas (zona): Bull=7071 Bear=5522 Neutral=581
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.4, DirBear≈ 1.1, DirNeutral≈ 0.1
- Razones de dirección: {'triggers-only': 30, 'tie-bias': 581, 'anchors+triggers': 12563}
- TF Triggers: {'5': 8477, '15': 4697}
- TF Anchors: {'60': 13038, '240': 12875, '1440': 10411}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,26': 1, 'estructura no existe': 15, 'score decayó a 0,32': 1, 'score decayó a 0,36': 1, 'score decayó a 0,25': 1, 'score decayó a 0,27': 1, 'score decayó a 0,18': 1}
- Cancel_BOS (diag): por acción {'BUY': 2, 'SELL': 7} | por bias {'Bullish': 7, 'Bearish': 2, 'Neutral': 0}

## CSV de Trades
- Filas: 113 | Ejecutadas: 20 | Canceladas: 0 | Expiradas: 0
- BUY: 62 | SELL: 71

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1370
- Registered: 57
  - DEDUP_COOLDOWN: 3 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 60

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 4.4%
- RegRate = Registered / Intentos = 95.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 5.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 35.1%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7488 | Total candidatos: 183268 | Seleccionados: 7450
- Candidatos por zona (promedio): 24.5
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 61336, 60: 55474, 15: 31669, 1440: 17695, 5: 17094}
- **TF Seleccionados**: {15: 637, 60: 4873, 5: 218, 1440: 912, 240: 810}
- **DistATR** - Candidatos: avg=20.0 | Seleccionados: avg=9.7
- **Razones de selección**: {'Fallback<15': 1812, 'InBand[8,15]_TFPreference': 5638}
- **En banda [10,15] ATR**: 30223/183268 (16.5%)

### Take Profit (TP)
- Zonas analizadas: 6622 | Total candidatos: 86251 | Seleccionados: 6622
- Candidatos por zona (promedio): 13.0
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=122
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.11
- **Priority Candidatos**: {'P3': 86251}
- **Priority Seleccionados**: {'P4_Fallback': 5559, 'P3': 1063}
- **Type Candidatos**: {'Swing': 86251}
- **Type Seleccionados**: {'Calculated': 5559, 'Swing': 1063}
- **TF Candidatos**: {240: 20584, 5: 17572, 15: 16622, 60: 16568, 1440: 14905}
- **TF Seleccionados**: {-1: 5559, 60: 50, 240: 655, 5: 59, 1440: 297, 15: 2}
- **DistATR** - Candidatos: avg=12.4 | Seleccionados: avg=12.8
- **RR** - Candidatos: avg=1.10 | Seleccionados: avg=1.05
- **Razones de selección**: {'NoStructuralTarget': 5559, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 176, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 202, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 22, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 13, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 228, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 65, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 75, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 16, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 104, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 23, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 38, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 7, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 49, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 3, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 63% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 84% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 0.92.