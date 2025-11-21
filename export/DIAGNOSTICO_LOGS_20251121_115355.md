# Informe Diagnóstico de Logs - 2025-11-21 12:02:30

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251121_115355.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251121_115355.csv`

## DFM
- Eventos de evaluación: 370
- Evaluaciones Bull: 0 | Bear: 286
- Pasaron umbral (PassedThreshold): 286
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:3, 6:95, 7:118, 8:70, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3129
- KeptAligned: 4138/4138 | KeptCounter: 4552/4893
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.240 | AvgProxCounter≈ 0.237
  - AvgDistATRAligned≈ 0.97 | AvgDistATRCounter≈ 1.24
- PreferAligned eventos: 961 | Filtradas contra-bias: 239

### Proximity (Pre-PreferAligned)
- Eventos: 3129
- Aligned pre: 4138/8690 | Counter pre: 4552/8690
- AvgProxAligned(pre)≈ 0.240 | AvgDistATRAligned(pre)≈ 0.97

### Proximity Drivers
- Eventos: 3129
- Alineadas: n=4138 | BaseProx≈ 0.736 | ZoneATR≈ 4.14 | SizePenalty≈ 0.986 | FinalProx≈ 0.725
- Contra-bias: n=4313 | BaseProx≈ 0.514 | ZoneATR≈ 5.61 | SizePenalty≈ 0.966 | FinalProx≈ 0.496

## Risk
- Eventos: 2324
- Accepted=474 | RejSL=0 | RejTP=0 | RejRR=630 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 1032 (31.5% del total)
  - Avg Score: 0.40 | Avg R:R: 1.63 | Avg DistATR: 3.99
  - Por TF: TF5=80, TF15=952
- **P0_SWING_LITE:** 2239 (68.5% del total)
  - Avg Score: 0.82 | Avg R:R: 3.15 | Avg DistATR: 4.05
  - Por TF: TF15=251, TF60=1988


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 6 | Unmatched: 470
- 0-10: Wins=5 Losses=1 WR=83.3% (n=6)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=5 Losses=1 WR=83.3% (n=6)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 476 | Aligned=236 (49.6%)
- Core≈ 1.00 | Prox≈ 0.61 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.19 | Confidence≈ 0.00
- SL_TF dist: {'15': 267, '5': 209} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 113, '15': 139, '240': 205, '60': 19} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=474, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=207, 15m=267, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.20 (n=474), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 51012 | Zonas con Anchors: 51012
- Dir zonas (zona): Bull=3497 Bear=46853 Neutral=662
- Resumen por ciclo (promedios): TotHZ≈ 16.3, WithAnchors≈ 16.3, DirBull≈ 1.1, DirBear≈ 15.0, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 50010, 'tie-bias': 1002}
- TF Triggers: {'5': 32746, '15': 18266}
- TF Anchors: {'1440': 51012, '240': 51012, '60': 51012}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## Timing y Confluencia HTF
- [TIMING_ADAPT] eventos: 151 | Approach rejects: 100
- Score/Req promedio: 2.40/2.00
- [HTF_CONFL] muestras: 852 | ok=852 | rejects=0
- median≈ 0.128 | thr≈ 0.120
- [BIAS_FAST] muestras: 1458 | Bull=92 Bear=1252 Neutral=114 | rejects=11
- score promedio: -0.69
- [HTF_CONFL] muestras: 852 | ok=852 | rejects=0
- median≈ 0.128 | thr≈ 0.120
- [RC_SL_MINFLOOR] filtrados: 0

## TradeManager - Razones (desde log)

## CSV de Trades
- Filas: 23 | Ejecutadas: 7 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 30

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 286
- Registered: 12
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 12

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 4.2%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 58.3%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 8079 | Total candidatos: 168551 | Seleccionados: 86
- Candidatos por zona (promedio): 20.9

### Take Profit (TP)
- Zonas analizadas: 8068 | Total candidatos: 325028 | Seleccionados: 8068
- Candidatos por zona (promedio): 40.3
- **Edad (barras)** - Candidatos: med=1079, max=8124 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.80 | Seleccionados: avg=0.66
- **Priority Candidatos**: {'P3': 185908}
- **Priority Seleccionados**: {'P3': 7232, 'NA': 557, 'P0': 279}
- **Type Candidatos**: {'Swing': 185908}
- **Type Seleccionados**: {'P3_Swing': 7232, 'P4_Fallback': 557, 'P0_Zone': 279}
- **TF Candidatos**: {5: 89112, 15: 53094, 240: 24096, 60: 19606}
- **TF Seleccionados**: {5: 4818, 240: 1938, 15: 522, -1: 557, 60: 233}
- **DistATR** - Candidatos: avg=34.3 | Seleccionados: avg=15.9
- **RR** - Candidatos: avg=6.30 | Seleccionados: avg=1.27
- **Razones de selección**: {'BestIntelligentScore': 8068}

### 🎯 Recomendaciones
- ⚠️ SL: 100% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.