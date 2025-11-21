# Informe Diagnóstico de Logs - 2025-11-21 15:57:47

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251121_154612.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251121_154612.csv`

## DFM
- Eventos de evaluación: 396
- Evaluaciones Bull: 0 | Bear: 393
- Pasaron umbral (PassedThreshold): 393
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:15, 6:141, 7:158, 8:79, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3129
- KeptAligned: 3946/3946 | KeptCounter: 4426/4721
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.252 | AvgProxCounter≈ 0.227
  - AvgDistATRAligned≈ 1.00 | AvgDistATRCounter≈ 1.23
- PreferAligned eventos: 988 | Filtradas contra-bias: 305

### Proximity (Pre-PreferAligned)
- Eventos: 3129
- Aligned pre: 3946/8372 | Counter pre: 4426/8372
- AvgProxAligned(pre)≈ 0.252 | AvgDistATRAligned(pre)≈ 1.00

### Proximity Drivers
- Eventos: 3129
- Alineadas: n=3946 | BaseProx≈ 0.740 | ZoneATR≈ 4.66 | SizePenalty≈ 0.979 | FinalProx≈ 0.725
- Contra-bias: n=4121 | BaseProx≈ 0.494 | ZoneATR≈ 5.87 | SizePenalty≈ 0.963 | FinalProx≈ 0.475

## Risk
- Eventos: 2286
- Accepted=611 | RejSL=0 | RejTP=0 | RejRR=598 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 724 (23.0% del total)
  - Avg Score: 0.40 | Avg R:R: 1.93 | Avg DistATR: 3.71
  - Por TF: TF5=123, TF15=601
- **P0_SWING_LITE:** 2419 (77.0% del total)
  - Avg Score: 0.84 | Avg R:R: 3.82 | Avg DistATR: 4.06
  - Por TF: TF15=235, TF60=2184


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 38 | Unmatched: 573
- 0-10: Wins=36 Losses=2 WR=94.7% (n=38)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=36 Losses=2 WR=94.7% (n=38)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 611 | Aligned=334 (54.7%)
- Core≈ 1.00 | Prox≈ 0.61 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.20 | Confidence≈ 0.00
- SL_TF dist: {'15': 444, '5': 167} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 94, '5': 315, '60': 172, '240': 30} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=611, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=167, 15m=444, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.20 (n=611), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 49365 | Zonas con Anchors: 49365
- Dir zonas (zona): Bull=3559 Bear=44845 Neutral=961
- Resumen por ciclo (promedios): TotHZ≈ 15.8, WithAnchors≈ 15.8, DirBull≈ 1.1, DirBear≈ 14.3, DirNeutral≈ 0.3
- Razones de dirección: {'anchors+triggers': 47914, 'tie-bias': 1451}
- TF Triggers: {'15': 16944, '5': 32421}
- TF Anchors: {'240': 49365, '1440': 49365, '60': 49365}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## Timing y Confluencia HTF
- [TIMING_ADAPT] eventos: 134 | Approach rejects: 94
- Score/Req promedio: 2.49/2.00
- [HTF_CONFL] muestras: 766 | ok=766 | rejects=0
- median≈ 0.125 | thr≈ 0.117
- [BIAS_FAST] muestras: 1298 | Bull=75 Bear=1116 Neutral=107 | rejects=6
- score promedio: -0.59
- [HTF_CONFL] muestras: 766 | ok=766 | rejects=0
- median≈ 0.125 | thr≈ 0.117
- [RC_SL_MINFLOOR] filtrados: 0

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,27': 1}

## CSV de Trades
- Filas: 43 | Ejecutadas: 8 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 51

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 393
- Registered: 22
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 22

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 5.6%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 36.4%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7645 | Total candidatos: 169601 | Seleccionados: 174
- Candidatos por zona (promedio): 22.2

### Take Profit (TP)
- Zonas analizadas: 7635 | Total candidatos: 302608 | Seleccionados: 7635
- Candidatos por zona (promedio): 39.6
- **Edad (barras)** - Candidatos: med=1073, max=8124 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.80 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 175978}
- **Priority Seleccionados**: {'P3': 6454, 'NA': 950, 'P0': 231}
- **Type Candidatos**: {'Swing': 175978}
- **Type Seleccionados**: {'P3_Swing': 6454, 'P4_Fallback': 950, 'P0_Zone': 231}
- **TF Candidatos**: {5: 84310, 15: 50183, 240: 22834, 60: 18651}
- **TF Seleccionados**: {5: 4279, 240: 1019, 60: 711, -1: 950, 15: 676}
- **DistATR** - Candidatos: avg=34.7 | Seleccionados: avg=13.9
- **RR** - Candidatos: avg=9.20 | Seleccionados: avg=1.31
- **Razones de selección**: {'BestIntelligentScore': 7635}

### 🎯 Recomendaciones
- ⚠️ SL: 100% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.