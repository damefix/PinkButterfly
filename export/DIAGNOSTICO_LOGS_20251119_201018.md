# Informe Diagnóstico de Logs - 2025-11-19 20:30:27

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_201018.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_201018.csv`

## DFM
- Eventos de evaluación: 767
- Evaluaciones Bull: 0 | Bear: 610
- Pasaron umbral (PassedThreshold): 610
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:21, 6:216, 7:287, 8:86, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3122
- KeptAligned: 3254/3254 | KeptCounter: 2736/2969
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.203 | AvgProxCounter≈ 0.140
  - AvgDistATRAligned≈ 0.87 | AvgDistATRCounter≈ 0.81
- PreferAligned eventos: 883 | Filtradas contra-bias: 10

### Proximity (Pre-PreferAligned)
- Eventos: 3122
- Aligned pre: 3254/5990 | Counter pre: 2736/5990
- AvgProxAligned(pre)≈ 0.203 | AvgDistATRAligned(pre)≈ 0.87

### Proximity Drivers
- Eventos: 3122
- Alineadas: n=3254 | BaseProx≈ 0.714 | ZoneATR≈ 4.31 | SizePenalty≈ 0.986 | FinalProx≈ 0.704
- Contra-bias: n=2726 | BaseProx≈ 0.478 | ZoneATR≈ 5.34 | SizePenalty≈ 0.974 | FinalProx≈ 0.466

## Risk
- Eventos: 1819
- Accepted=928 | RejSL=0 | RejTP=0 | RejRR=1478 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 843 (19.2% del total)
  - Avg Score: 0.38 | Avg R:R: 1.91 | Avg DistATR: 3.79
  - Por TF: TF5=114, TF15=729
- **P0_SWING_LITE:** 3537 (80.8% del total)
  - Avg Score: 0.63 | Avg R:R: 3.43 | Avg DistATR: 3.75
  - Por TF: TF15=485, TF60=3052


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 60 | Unmatched: 868
- 0-10: Wins=22 Losses=38 WR=36.7% (n=60)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=22 Losses=38 WR=36.7% (n=60)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 928 | Aligned=492 (53.0%)
- Core≈ 1.00 | Prox≈ 0.61 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.25 | Confidence≈ 0.00
- SL_TF dist: {'15': 668, '60': 150, '5': 100, '240': 10} | SL_Structural≈ 100.0%
- TP_TF dist: {'240': 292, '15': 352, '60': 207, '5': 77} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=928, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=100, 15m=668, 60m=150, 240m=10, 1440m=0
- RR plan por bandas: 0-10≈ 2.25 (n=928), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 24640 | Zonas con Anchors: 24640
- Dir zonas (zona): Bull=106 Bear=24434 Neutral=100
- Resumen por ciclo (promedios): TotHZ≈ 7.9, WithAnchors≈ 7.9, DirBull≈ 0.0, DirBear≈ 7.8, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 24535, 'tie-bias': 105}
- TF Triggers: {'5': 11983, '15': 12657}
- TF Anchors: {'60': 24640, '240': 24640, '1440': 24640}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## Timing y Confluencia HTF
- [TIMING_ADAPT] eventos: 233 | Approach rejects: 169
- Score/Req promedio: 2.40/2.00
- [HTF_CONFL] muestras: 594 | ok=594 | rejects=0
- median≈ 0.000 | thr≈ 0.000
- [BIAS_FAST] muestras: 0 | Bull=0 Bear=0 Neutral=0 | rejects=38
- (proxy) Solo hay rejects: Bull=38 Bear=0 Neutral=0
- [HTF_CONFL] muestras: 594 | ok=594 | rejects=0
- median≈ 0.000 | thr≈ 0.000
- [RC_SL_MINFLOOR] filtrados: 0

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 6}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 3} | por bias {'Bullish': 3, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 68 | Ejecutadas: 11 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 79

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 610
- Registered: 34
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 34

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 5.6%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 32.4%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5650 | Total candidatos: 89375 | Seleccionados: 128
- Candidatos por zona (promedio): 15.8

### Take Profit (TP)
- Zonas analizadas: 5650 | Total candidatos: 138056 | Seleccionados: 5650
- Candidatos por zona (promedio): 24.4
- **Edad (barras)** - Candidatos: med=80, max=468 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.72
- **Priority Candidatos**: {'P3': 72907}
- **Priority Seleccionados**: {'P3': 4208, 'NA': 1034, 'P0': 408}
- **Type Candidatos**: {'Swing': 72907}
- **Type Seleccionados**: {'P3_Swing': 4208, 'P4_Fallback': 1034, 'P0_Zone': 408}
- **TF Candidatos**: {240: 48931, 60: 10986, 15: 7252, 5: 5738}
- **TF Seleccionados**: {240: 1939, -1: 1034, 15: 1085, 60: 1049, 5: 543}
- **DistATR** - Candidatos: avg=18.0 | Seleccionados: avg=5.9
- **RR** - Candidatos: avg=6.69 | Seleccionados: avg=1.57
- **Razones de selección**: {'BestIntelligentScore': 5650}

### 🎯 Recomendaciones
- ⚠️ SL: 100% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.