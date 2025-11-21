# Informe Diagnóstico de Logs - 2025-11-21 11:00:04

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251121_104738.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251121_104738.csv`

## DFM
- Eventos de evaluación: 352
- Evaluaciones Bull: 0 | Bear: 311
- Pasaron umbral (PassedThreshold): 311
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:6, 6:143, 7:90, 8:57, 9:15

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3129
- KeptAligned: 4089/4089 | KeptCounter: 4566/4907
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.241 | AvgProxCounter≈ 0.234
  - AvgDistATRAligned≈ 0.95 | AvgDistATRCounter≈ 1.25
- PreferAligned eventos: 953 | Filtradas contra-bias: 226

### Proximity (Pre-PreferAligned)
- Eventos: 3129
- Aligned pre: 4089/8655 | Counter pre: 4566/8655
- AvgProxAligned(pre)≈ 0.241 | AvgDistATRAligned(pre)≈ 0.95

### Proximity Drivers
- Eventos: 3129
- Alineadas: n=4089 | BaseProx≈ 0.741 | ZoneATR≈ 4.19 | SizePenalty≈ 0.985 | FinalProx≈ 0.730
- Contra-bias: n=4340 | BaseProx≈ 0.503 | ZoneATR≈ 5.59 | SizePenalty≈ 0.965 | FinalProx≈ 0.486

## Risk
- Eventos: 2317
- Accepted=521 | RejSL=0 | RejTP=0 | RejRR=616 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 976 (30.7% del total)
  - Avg Score: 0.39 | Avg R:R: 1.71 | Avg DistATR: 3.98
  - Por TF: TF5=126, TF15=850
- **P0_SWING_LITE:** 2206 (69.3% del total)
  - Avg Score: 0.82 | Avg R:R: 2.96 | Avg DistATR: 4.07
  - Por TF: TF15=261, TF60=1945


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 14 | Unmatched: 509
- 0-10: Wins=12 Losses=2 WR=85.7% (n=14)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=12 Losses=2 WR=85.7% (n=14)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 523 | Aligned=259 (49.5%)
- Core≈ 1.00 | Prox≈ 0.60 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.11 | Confidence≈ 0.00
- SL_TF dist: {'15': 308, '5': 215} | SL_Structural≈ 100.0%
- TP_TF dist: {'240': 202, '5': 180, '15': 120, '60': 21} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=521, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=214, 15m=307, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.12 (n=521), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 50668 | Zonas con Anchors: 50668
- Dir zonas (zona): Bull=3533 Bear=46465 Neutral=670
- Resumen por ciclo (promedios): TotHZ≈ 16.2, WithAnchors≈ 16.2, DirBull≈ 1.1, DirBear≈ 14.8, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 49666, 'tie-bias': 1002}
- TF Triggers: {'5': 32083, '15': 18585}
- TF Anchors: {'1440': 50668, '240': 50668, '60': 50668}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## Timing y Confluencia HTF
- [TIMING_ADAPT] eventos: 154 | Approach rejects: 106
- Score/Req promedio: 2.40/2.00
- [HTF_CONFL] muestras: 870 | ok=870 | rejects=0
- median≈ 0.127 | thr≈ 0.120
- [BIAS_FAST] muestras: 1472 | Bull=96 Bear=1240 Neutral=136 | rejects=6
- score promedio: -0.65
- [HTF_CONFL] muestras: 870 | ok=870 | rejects=0
- median≈ 0.127 | thr≈ 0.120
- [RC_SL_MINFLOOR] filtrados: 0

## TradeManager - Razones (desde log)

## CSV de Trades
- Filas: 35 | Ejecutadas: 8 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 43

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 311
- Registered: 18
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 18

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 5.8%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 44.4%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 8054 | Total candidatos: 161516 | Seleccionados: 92
- Candidatos por zona (promedio): 20.1

### Take Profit (TP)
- Zonas analizadas: 8040 | Total candidatos: 326830 | Seleccionados: 8040
- Candidatos por zona (promedio): 40.7
- **Edad (barras)** - Candidatos: med=1068, max=8124 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.80 | Seleccionados: avg=0.66
- **Priority Candidatos**: {'P3': 185986}
- **Priority Seleccionados**: {'P3': 7288, 'NA': 521, 'P0': 231}
- **Type Candidatos**: {'Swing': 185986}
- **Type Seleccionados**: {'P3_Swing': 7288, 'P4_Fallback': 521, 'P0_Zone': 231}
- **TF Candidatos**: {5: 89364, 15: 52976, 240: 24014, 60: 19632}
- **TF Seleccionados**: {5: 4941, 240: 1744, 15: 607, -1: 521, 60: 227}
- **DistATR** - Candidatos: avg=34.0 | Seleccionados: avg=16.2
- **RR** - Candidatos: avg=6.23 | Seleccionados: avg=1.30
- **Razones de selección**: {'BestIntelligentScore': 8040}

### 🎯 Recomendaciones
- ⚠️ SL: 100% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.