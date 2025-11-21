# Informe Diagnóstico de Logs - 2025-11-19 18:49:40

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_184226.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_184226.csv`

## DFM
- Eventos de evaluación: 588
- Evaluaciones Bull: 0 | Bear: 498
- Pasaron umbral (PassedThreshold): 498
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:19, 6:175, 7:202, 8:102, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3123
- KeptAligned: 2966/2966 | KeptCounter: 2600/2784
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.202 | AvgProxCounter≈ 0.141
  - AvgDistATRAligned≈ 0.86 | AvgDistATRCounter≈ 0.72
- PreferAligned eventos: 875 | Filtradas contra-bias: 7

### Proximity (Pre-PreferAligned)
- Eventos: 3123
- Aligned pre: 2966/5566 | Counter pre: 2600/5566
- AvgProxAligned(pre)≈ 0.202 | AvgDistATRAligned(pre)≈ 0.86

### Proximity Drivers
- Eventos: 3123
- Alineadas: n=2966 | BaseProx≈ 0.717 | ZoneATR≈ 4.70 | SizePenalty≈ 0.981 | FinalProx≈ 0.704
- Contra-bias: n=2593 | BaseProx≈ 0.503 | ZoneATR≈ 5.84 | SizePenalty≈ 0.965 | FinalProx≈ 0.485

## Risk
- Eventos: 1773
- Accepted=703 | RejSL=0 | RejTP=0 | RejRR=1375 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 528 (13.4% del total)
  - Avg Score: 0.39 | Avg R:R: 1.75 | Avg DistATR: 3.86
  - Por TF: TF5=80, TF15=448
- **P0_SWING_LITE:** 3407 (86.6% del total)
  - Avg Score: 0.64 | Avg R:R: 3.26 | Avg DistATR: 3.81
  - Por TF: TF15=394, TF60=3013


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 27 | Unmatched: 676
- 0-10: Wins=20 Losses=7 WR=74.1% (n=27)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=20 Losses=7 WR=74.1% (n=27)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 703 | Aligned=419 (59.6%)
- Core≈ 1.00 | Prox≈ 0.61 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.23 | Confidence≈ 0.00
- SL_TF dist: {'15': 452, '5': 104, '60': 132, '240': 15} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 187, '240': 353, '60': 93, '5': 70} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=703, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=104, 15m=452, 60m=132, 240m=15, 1440m=0
- RR plan por bandas: 0-10≈ 2.23 (n=703), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 21890 | Zonas con Anchors: 21890
- Dir zonas (zona): Bull=48 Bear=21742 Neutral=100
- Resumen por ciclo (promedios): TotHZ≈ 7.0, WithAnchors≈ 7.0, DirBull≈ 0.0, DirBear≈ 7.0, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 21787, 'tie-bias': 103}
- TF Triggers: {'15': 11744, '5': 10146}
- TF Anchors: {'60': 21890, '240': 21890, '1440': 21890}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## Timing y Confluencia HTF
- [TIMING_ADAPT] eventos: 183 | Approach rejects: 106
- Score/Req promedio: 2.44/2.00
- [HTF_CONFL] muestras: 452 | ok=452 | rejects=0
- median≈ 0.000 | thr≈ 0.000
- [BIAS_FAST] muestras: 0 | Bull=0 Bear=0 Neutral=0 | rejects=26
- [RC_SL_MINFLOOR] filtrados: 0

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,49': 2, 'score decayó a 0,37': 1, 'estructura no existe': 7}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 5} | por bias {'Bullish': 5, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 66 | Ejecutadas: 11 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 77

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 498
- Registered: 33
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 33

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 6.6%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 33.3%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5293 | Total candidatos: 77547 | Seleccionados: 0
- Candidatos por zona (promedio): 14.7

### Take Profit (TP)
- Zonas analizadas: 5293 | Total candidatos: 130769 | Seleccionados: 5293
- Candidatos por zona (promedio): 24.7
- **Edad (barras)** - Candidatos: med=83, max=311 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.54 | Seleccionados: avg=0.72
- **Priority Candidatos**: {'P3': 72207}
- **Priority Seleccionados**: {'P3': 4320, 'NA': 623, 'P0': 350}
- **Type Candidatos**: {'Swing': 72207}
- **Type Seleccionados**: {'P3_Swing': 4320, 'P4_Fallback': 623, 'P0_Zone': 350}
- **TF Candidatos**: {240: 48137, 60: 10590, 15: 7742, 5: 5738}
- **TF Seleccionados**: {240: 2526, -1: 623, 15: 783, 60: 771, 5: 590}
- **DistATR** - Candidatos: avg=18.0 | Seleccionados: avg=6.5
- **RR** - Candidatos: avg=7.78 | Seleccionados: avg=1.55
- **Razones de selección**: {'BestIntelligentScore': 5293}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.