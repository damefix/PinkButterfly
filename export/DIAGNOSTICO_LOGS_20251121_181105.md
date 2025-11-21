# Informe Diagnóstico de Logs - 2025-11-21 18:21:14

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251121_181105.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251121_181105.csv`

## DFM
- Eventos de evaluación: 361
- Evaluaciones Bull: 0 | Bear: 279
- Pasaron umbral (PassedThreshold): 279
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:9, 6:96, 7:149, 8:25, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3127
- KeptAligned: 3866/3866 | KeptCounter: 4415/4712
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.247 | AvgProxCounter≈ 0.225
  - AvgDistATRAligned≈ 0.99 | AvgDistATRCounter≈ 1.21
- PreferAligned eventos: 995 | Filtradas contra-bias: 332

### Proximity (Pre-PreferAligned)
- Eventos: 3127
- Aligned pre: 3866/8281 | Counter pre: 4415/8281
- AvgProxAligned(pre)≈ 0.247 | AvgDistATRAligned(pre)≈ 0.99

### Proximity Drivers
- Eventos: 3127
- Alineadas: n=3866 | BaseProx≈ 0.733 | ZoneATR≈ 4.70 | SizePenalty≈ 0.978 | FinalProx≈ 0.716
- Contra-bias: n=4083 | BaseProx≈ 0.509 | ZoneATR≈ 5.86 | SizePenalty≈ 0.962 | FinalProx≈ 0.490

## Risk
- Eventos: 2261
- Accepted=467 | RejSL=0 | RejTP=0 | RejRR=807 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 679 (21.4% del total)
  - Avg Score: 0.39 | Avg R:R: 1.88 | Avg DistATR: 3.96
  - Por TF: TF5=109, TF15=570
- **P0_SWING_LITE:** 2496 (78.6% del total)
  - Avg Score: 0.84 | Avg R:R: 3.25 | Avg DistATR: 4.05
  - Por TF: TF15=201, TF60=2295


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 28 | Unmatched: 439
- 0-10: Wins=9 Losses=19 WR=32.1% (n=28)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=9 Losses=19 WR=32.1% (n=28)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 467 | Aligned=208 (44.5%)
- Core≈ 1.00 | Prox≈ 0.58 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.37 | Confidence≈ 0.00
- SL_TF dist: {'15': 365, '5': 102} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 203, '240': 85, '15': 52, '60': 127} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=467, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=102, 15m=365, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.37 (n=467), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 48139 | Zonas con Anchors: 48139
- Dir zonas (zona): Bull=3467 Bear=43579 Neutral=1093
- Resumen por ciclo (promedios): TotHZ≈ 15.4, WithAnchors≈ 15.4, DirBull≈ 1.1, DirBear≈ 13.9, DirNeutral≈ 0.3
- Razones de dirección: {'anchors+triggers': 46528, 'tie-bias': 1611}
- TF Triggers: {'5': 31762, '15': 16377}
- TF Anchors: {'60': 48139, '240': 48139, '1440': 48139}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## Timing y Confluencia HTF
- [TIMING_ADAPT] eventos: 95 | Approach rejects: 92
- Score/Req promedio: 2.44/2.00
- [HTF_CONFL] muestras: 628 | ok=628 | rejects=0
- median≈ 0.126 | thr≈ 0.121
- [BIAS_FAST] muestras: 1054 | Bull=108 Bear=845 Neutral=101 | rejects=6
- score promedio: -0.46
- [HTF_CONFL] muestras: 628 | ok=628 | rejects=0
- median≈ 0.126 | thr≈ 0.121
- [RC_SL_MINFLOOR] filtrados: 0

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,27': 1}

## CSV de Trades
- Filas: 26 | Ejecutadas: 8 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 34

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 279
- Registered: 13
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 13

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 4.7%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 61.5%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7545 | Total candidatos: 195630 | Seleccionados: 82
- Candidatos por zona (promedio): 25.9

### Take Profit (TP)
- Zonas analizadas: 7545 | Total candidatos: 299463 | Seleccionados: 7545
- Candidatos por zona (promedio): 39.7
- **Edad (barras)** - Candidatos: med=1083, max=8124 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.80 | Seleccionados: avg=0.66
- **Priority Candidatos**: {'P3': 170668}
- **Priority Seleccionados**: {'P3': 6673, 'NA': 657, 'P0': 215}
- **Type Candidatos**: {'Swing': 170668}
- **Type Seleccionados**: {'P3_Swing': 6673, 'P4_Fallback': 657, 'P0_Zone': 215}
- **TF Candidatos**: {5: 81965, 15: 48228, 240: 22291, 60: 18184}
- **TF Seleccionados**: {5: 4499, 240: 1323, 15: 506, 60: 560, -1: 657}
- **DistATR** - Candidatos: avg=34.0 | Seleccionados: avg=14.5
- **RR** - Candidatos: avg=7.86 | Seleccionados: avg=1.34
- **Razones de selección**: {'BestIntelligentScore': 7545}

### 🎯 Recomendaciones
- ⚠️ SL: 100% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.