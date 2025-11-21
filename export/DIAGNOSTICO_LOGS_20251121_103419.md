# Informe Diagnóstico de Logs - 2025-11-21 10:46:59

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251121_103419.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251121_103419.csv`

## DFM
- Eventos de evaluación: 342
- Evaluaciones Bull: 0 | Bear: 313
- Pasaron umbral (PassedThreshold): 313
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:8, 6:138, 7:96, 8:56, 9:15

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3128
- KeptAligned: 4071/4071 | KeptCounter: 4558/4895
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.239 | AvgProxCounter≈ 0.233
  - AvgDistATRAligned≈ 0.94 | AvgDistATRCounter≈ 1.25
- PreferAligned eventos: 945 | Filtradas contra-bias: 219

### Proximity (Pre-PreferAligned)
- Eventos: 3128
- Aligned pre: 4071/8629 | Counter pre: 4558/8629
- AvgProxAligned(pre)≈ 0.239 | AvgDistATRAligned(pre)≈ 0.94

### Proximity Drivers
- Eventos: 3128
- Alineadas: n=4071 | BaseProx≈ 0.742 | ZoneATR≈ 4.18 | SizePenalty≈ 0.985 | FinalProx≈ 0.730
- Contra-bias: n=4339 | BaseProx≈ 0.502 | ZoneATR≈ 5.61 | SizePenalty≈ 0.965 | FinalProx≈ 0.485

## Risk
- Eventos: 2315
- Accepted=512 | RejSL=0 | RejTP=0 | RejRR=621 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 973 (30.8% del total)
  - Avg Score: 0.39 | Avg R:R: 1.72 | Avg DistATR: 3.99
  - Por TF: TF5=126, TF15=847
- **P0_SWING_LITE:** 2183 (69.2% del total)
  - Avg Score: 0.82 | Avg R:R: 2.95 | Avg DistATR: 4.09
  - Por TF: TF15=262, TF60=1921


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 15 | Unmatched: 497
- 0-10: Wins=12 Losses=3 WR=80.0% (n=15)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=12 Losses=3 WR=80.0% (n=15)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 512 | Aligned=260 (50.8%)
- Core≈ 1.00 | Prox≈ 0.60 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.07 | Confidence≈ 0.00
- SL_TF dist: {'15': 316, '5': 196} | SL_Structural≈ 100.0%
- TP_TF dist: {'240': 202, '5': 178, '15': 108, '60': 24} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=512, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=196, 15m=316, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.07 (n=512), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 50579 | Zonas con Anchors: 50579
- Dir zonas (zona): Bull=3523 Bear=46396 Neutral=660
- Resumen por ciclo (promedios): TotHZ≈ 16.2, WithAnchors≈ 16.2, DirBull≈ 1.1, DirBear≈ 14.8, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 49587, 'tie-bias': 992}
- TF Triggers: {'5': 32078, '15': 18501}
- TF Anchors: {'1440': 50579, '240': 50579, '60': 50579}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## Timing y Confluencia HTF
- [TIMING_ADAPT] eventos: 129 | Approach rejects: 82
- Score/Req promedio: 2.43/2.00
- [HTF_CONFL] muestras: 712 | ok=712 | rejects=0
- median≈ 0.131 | thr≈ 0.123
- [BIAS_FAST] muestras: 1202 | Bull=69 Bear=1021 Neutral=112 | rejects=3
- score promedio: -0.62
- [HTF_CONFL] muestras: 712 | ok=712 | rejects=0
- median≈ 0.131 | thr≈ 0.123
- [RC_SL_MINFLOOR] filtrados: 0

## TradeManager - Razones (desde log)

## CSV de Trades
- Filas: 36 | Ejecutadas: 9 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 45

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 313
- Registered: 18
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 18

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 5.8%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 50.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 8034 | Total candidatos: 160240 | Seleccionados: 92
- Candidatos por zona (promedio): 19.9

### Take Profit (TP)
- Zonas analizadas: 8023 | Total candidatos: 326790 | Seleccionados: 8023
- Candidatos por zona (promedio): 40.7
- **Edad (barras)** - Candidatos: med=1063, max=8124 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.80 | Seleccionados: avg=0.66
- **Priority Candidatos**: {'P3': 185946}
- **Priority Seleccionados**: {'P3': 7279, 'NA': 528, 'P0': 216}
- **Type Candidatos**: {'Swing': 185946}
- **Type Seleccionados**: {'P3_Swing': 7279, 'P4_Fallback': 528, 'P0_Zone': 216}
- **TF Candidatos**: {5: 89438, 15: 52901, 240: 23998, 60: 19609}
- **TF Seleccionados**: {5: 4932, 240: 1744, 15: 601, -1: 528, 60: 218}
- **DistATR** - Candidatos: avg=34.1 | Seleccionados: avg=16.3
- **RR** - Candidatos: avg=6.25 | Seleccionados: avg=1.29
- **Razones de selección**: {'BestIntelligentScore': 8023}

### 🎯 Recomendaciones
- ⚠️ SL: 100% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.