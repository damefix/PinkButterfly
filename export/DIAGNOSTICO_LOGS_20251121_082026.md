# Informe Diagnóstico de Logs - 2025-11-21 08:29:37

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251121_082026.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251121_082026.csv`

## DFM
- Eventos de evaluación: 314
- Evaluaciones Bull: 0 | Bear: 309
- Pasaron umbral (PassedThreshold): 309
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:3, 6:116, 7:113, 8:73, 9:4

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3127
- KeptAligned: 4202/4202 | KeptCounter: 4641/4990
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.246 | AvgProxCounter≈ 0.245
  - AvgDistATRAligned≈ 0.97 | AvgDistATRCounter≈ 1.26
- PreferAligned eventos: 964 | Filtradas contra-bias: 297

### Proximity (Pre-PreferAligned)
- Eventos: 3127
- Aligned pre: 4202/8843 | Counter pre: 4641/8843
- AvgProxAligned(pre)≈ 0.246 | AvgDistATRAligned(pre)≈ 0.97

### Proximity Drivers
- Eventos: 3127
- Alineadas: n=4202 | BaseProx≈ 0.740 | ZoneATR≈ 4.16 | SizePenalty≈ 0.985 | FinalProx≈ 0.729
- Contra-bias: n=4344 | BaseProx≈ 0.510 | ZoneATR≈ 5.56 | SizePenalty≈ 0.967 | FinalProx≈ 0.492

## Risk
- Eventos: 2356
- Accepted=441 | RejSL=0 | RejTP=0 | RejRR=602 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 822 (28.6% del total)
  - Avg Score: 0.40 | Avg R:R: 1.68 | Avg DistATR: 4.01
  - Por TF: TF5=86, TF15=736
- **P0_SWING_LITE:** 2053 (71.4% del total)
  - Avg Score: 0.82 | Avg R:R: 2.72 | Avg DistATR: 4.07
  - Por TF: TF15=217, TF60=1836


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 12 | Unmatched: 429
- 0-10: Wins=12 Losses=0 WR=100.0% (n=12)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=12 Losses=0 WR=100.0% (n=12)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 441 | Aligned=244 (55.3%)
- Core≈ 1.00 | Prox≈ 0.63 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.07 | Confidence≈ 0.00
- SL_TF dist: {'15': 304, '5': 137} | SL_Structural≈ 100.0%
- TP_TF dist: {'240': 257, '5': 122, '15': 47, '60': 15} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=441, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=137, 15m=304, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.07 (n=441), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 52178 | Zonas con Anchors: 52178
- Dir zonas (zona): Bull=3588 Bear=47417 Neutral=1173
- Resumen por ciclo (promedios): TotHZ≈ 16.7, WithAnchors≈ 16.7, DirBull≈ 1.1, DirBear≈ 15.2, DirNeutral≈ 0.4
- Razones de dirección: {'anchors+triggers': 50348, 'tie-bias': 1830}
- TF Triggers: {'15': 19449, '5': 32729}
- TF Anchors: {'60': 52178, '240': 52178, '1440': 52178}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## Timing y Confluencia HTF
- [TIMING_ADAPT] eventos: 153 | Approach rejects: 107
- Score/Req promedio: 2.38/2.00
- [HTF_CONFL] muestras: 894 | ok=894 | rejects=0
- median≈ 0.127 | thr≈ 0.119
- [BIAS_FAST] muestras: 1511 | Bull=106 Bear=1288 Neutral=117 | rejects=7
- score promedio: -0.67
- [HTF_CONFL] muestras: 894 | ok=894 | rejects=0
- median≈ 0.127 | thr≈ 0.119
- [RC_SL_MINFLOOR] filtrados: 0

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,27': 1}

## CSV de Trades
- Filas: 18 | Ejecutadas: 6 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 24

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 309
- Registered: 9
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 9

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 2.9%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 66.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 8107 | Total candidatos: 161916 | Seleccionados: 76
- Candidatos por zona (promedio): 20.0

### Take Profit (TP)
- Zonas analizadas: 8099 | Total candidatos: 326092 | Seleccionados: 8099
- Candidatos por zona (promedio): 40.3
- **Edad (barras)** - Candidatos: med=1083, max=8001 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.80 | Seleccionados: avg=0.66
- **Priority Candidatos**: {'P3': 180705}
- **Priority Seleccionados**: {'P3': 7386, 'NA': 475, 'P0': 238}
- **Type Candidatos**: {'Swing': 180705}
- **Type Seleccionados**: {'P3_Swing': 7386, 'P4_Fallback': 475, 'P0_Zone': 238}
- **TF Candidatos**: {5: 86789, 15: 51251, 240: 23676, 60: 18989}
- **TF Seleccionados**: {5: 5168, 240: 1879, 15: 386, -1: 475, 60: 191}
- **DistATR** - Candidatos: avg=34.5 | Seleccionados: avg=16.7
- **RR** - Candidatos: avg=5.62 | Seleccionados: avg=1.23
- **Razones de selección**: {'BestIntelligentScore': 8099}

### 🎯 Recomendaciones
- ⚠️ SL: 100% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.