# Informe Diagnóstico de Logs - 2025-11-21 11:53:22

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251121_114550.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251121_114550.csv`

## DFM
- Eventos de evaluación: 370
- Evaluaciones Bull: 0 | Bear: 286
- Pasaron umbral (PassedThreshold): 286
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:3, 6:95, 7:118, 8:70, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3129
- KeptAligned: 4139/4139 | KeptCounter: 4553/4894
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.241 | AvgProxCounter≈ 0.237
  - AvgDistATRAligned≈ 0.98 | AvgDistATRCounter≈ 1.24
- PreferAligned eventos: 962 | Filtradas contra-bias: 239

### Proximity (Pre-PreferAligned)
- Eventos: 3129
- Aligned pre: 4139/8692 | Counter pre: 4553/8692
- AvgProxAligned(pre)≈ 0.241 | AvgDistATRAligned(pre)≈ 0.98

### Proximity Drivers
- Eventos: 3129
- Alineadas: n=4139 | BaseProx≈ 0.736 | ZoneATR≈ 4.13 | SizePenalty≈ 0.986 | FinalProx≈ 0.725
- Contra-bias: n=4314 | BaseProx≈ 0.514 | ZoneATR≈ 5.61 | SizePenalty≈ 0.966 | FinalProx≈ 0.496

## Risk
- Eventos: 2325
- Accepted=474 | RejSL=0 | RejTP=0 | RejRR=632 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 1031 (31.5% del total)
  - Avg Score: 0.40 | Avg R:R: 1.62 | Avg DistATR: 3.98
  - Por TF: TF5=81, TF15=950
- **P0_SWING_LITE:** 2243 (68.5% del total)
  - Avg Score: 0.82 | Avg R:R: 3.14 | Avg DistATR: 4.05
  - Por TF: TF15=254, TF60=1989


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
- Trazas por zona: 51016 | Zonas con Anchors: 51016
- Dir zonas (zona): Bull=3496 Bear=46858 Neutral=662
- Resumen por ciclo (promedios): TotHZ≈ 16.3, WithAnchors≈ 16.3, DirBull≈ 1.1, DirBear≈ 15.0, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 50014, 'tie-bias': 1002}
- TF Triggers: {'5': 32749, '15': 18267}
- TF Anchors: {'1440': 51016, '240': 51016, '60': 51016}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## Timing y Confluencia HTF
- [TIMING_ADAPT] eventos: 131 | Approach rejects: 76
- Score/Req promedio: 2.41/2.00
- [HTF_CONFL] muestras: 716 | ok=716 | rejects=0
- median≈ 0.131 | thr≈ 0.122
- [BIAS_FAST] muestras: 1226 | Bull=65 Bear=1071 Neutral=90 | rejects=8
- score promedio: -0.65
- [HTF_CONFL] muestras: 716 | ok=716 | rejects=0
- median≈ 0.131 | thr≈ 0.122
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
- Zonas analizadas: 8081 | Total candidatos: 168639 | Seleccionados: 86
- Candidatos por zona (promedio): 20.9

### Take Profit (TP)
- Zonas analizadas: 8070 | Total candidatos: 325079 | Seleccionados: 8070
- Candidatos por zona (promedio): 40.3
- **Edad (barras)** - Candidatos: med=1079, max=8124 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.80 | Seleccionados: avg=0.66
- **Priority Candidatos**: {'P3': 185959}
- **Priority Seleccionados**: {'P3': 7237, 'NA': 551, 'P0': 282}
- **Type Candidatos**: {'Swing': 185959}
- **Type Seleccionados**: {'P3_Swing': 7237, 'P4_Fallback': 551, 'P0_Zone': 282}
- **TF Candidatos**: {5: 89136, 15: 53108, 240: 24102, 60: 19613}
- **TF Seleccionados**: {5: 4823, 240: 1938, 15: 523, -1: 551, 60: 235}
- **DistATR** - Candidatos: avg=34.3 | Seleccionados: avg=15.9
- **RR** - Candidatos: avg=6.30 | Seleccionados: avg=1.27
- **Razones de selección**: {'BestIntelligentScore': 8070}

### 🎯 Recomendaciones
- ⚠️ SL: 100% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.