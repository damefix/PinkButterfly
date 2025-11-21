# Informe Diagnóstico de Logs - 2025-11-19 19:42:53

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_192349.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_192349.csv`

## DFM
- Eventos de evaluación: 793
- Evaluaciones Bull: 0 | Bear: 669
- Pasaron umbral (PassedThreshold): 669
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:22, 6:245, 7:297, 8:105, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3123
- KeptAligned: 3214/3214 | KeptCounter: 2756/2927
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.201 | AvgProxCounter≈ 0.133
  - AvgDistATRAligned≈ 0.85 | AvgDistATRCounter≈ 0.76
- PreferAligned eventos: 867 | Filtradas contra-bias: 12

### Proximity (Pre-PreferAligned)
- Eventos: 3123
- Aligned pre: 3214/5970 | Counter pre: 2756/5970
- AvgProxAligned(pre)≈ 0.201 | AvgDistATRAligned(pre)≈ 0.85

### Proximity Drivers
- Eventos: 3123
- Alineadas: n=3214 | BaseProx≈ 0.714 | ZoneATR≈ 4.29 | SizePenalty≈ 0.984 | FinalProx≈ 0.702
- Contra-bias: n=2744 | BaseProx≈ 0.482 | ZoneATR≈ 5.46 | SizePenalty≈ 0.969 | FinalProx≈ 0.467

## Risk
- Eventos: 1754
- Accepted=1044 | RejSL=0 | RejTP=0 | RejRR=1402 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 807 (18.7% del total)
  - Avg Score: 0.39 | Avg R:R: 1.75 | Avg DistATR: 3.69
  - Por TF: TF5=64, TF15=743
- **P0_SWING_LITE:** 3515 (81.3% del total)
  - Avg Score: 0.64 | Avg R:R: 3.51 | Avg DistATR: 3.79
  - Por TF: TF15=415, TF60=3100


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 87 | Unmatched: 957
- 0-10: Wins=52 Losses=35 WR=59.8% (n=87)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=52 Losses=35 WR=59.8% (n=87)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1044 | Aligned=560 (53.6%)
- Core≈ 1.00 | Prox≈ 0.60 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.13 | Confidence≈ 0.00
- SL_TF dist: {'15': 647, '5': 185, '60': 197, '240': 15} | SL_Structural≈ 100.0%
- TP_TF dist: {'240': 340, '15': 535, '5': 81, '60': 88} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1044, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=185, 15m=647, 60m=197, 240m=15, 1440m=0
- RR plan por bandas: 0-10≈ 2.13 (n=1044), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 22953 | Zonas con Anchors: 22953
- Dir zonas (zona): Bull=55 Bear=22777 Neutral=121
- Resumen por ciclo (promedios): TotHZ≈ 7.3, WithAnchors≈ 7.3, DirBull≈ 0.0, DirBear≈ 7.3, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 22809, 'tie-bias': 144}
- TF Triggers: {'15': 12062, '5': 10891}
- TF Anchors: {'60': 22953, '240': 22953, '1440': 22953}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## Timing y Confluencia HTF
- [TIMING_ADAPT] eventos: 220 | Approach rejects: 149
- Score/Req promedio: 2.46/2.00
- [HTF_CONFL] muestras: 551 | ok=551 | rejects=0
- median≈ 0.000 | thr≈ 0.000
- [BIAS_FAST] muestras: 0 | Bull=0 Bear=0 Neutral=0 | rejects=29
- (proxy) Solo hay rejects: Bull=29 Bear=0 Neutral=0
- [HTF_CONFL] muestras: 551 | ok=551 | rejects=0
- median≈ 0.000 | thr≈ 0.000
- [RC_SL_MINFLOOR] filtrados: 0

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,21': 2, 'estructura no existe': 7, 'score decayó a 0,49': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 6} | por bias {'Bullish': 6, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 80 | Ejecutadas: 16 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 96

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 669
- Registered: 40
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 40

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 6.0%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 40.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5684 | Total candidatos: 82714 | Seleccionados: 120
- Candidatos por zona (promedio): 14.6

### Take Profit (TP)
- Zonas analizadas: 5684 | Total candidatos: 146822 | Seleccionados: 5684
- Candidatos por zona (promedio): 25.8
- **Edad (barras)** - Candidatos: med=82, max=311 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.53 | Seleccionados: avg=0.73
- **Priority Candidatos**: {'P3': 79003}
- **Priority Seleccionados**: {'P3': 4608, 'NA': 757, 'P0': 319}
- **Type Candidatos**: {'Swing': 79003}
- **Type Seleccionados**: {'P3_Swing': 4608, 'P4_Fallback': 757, 'P0_Zone': 319}
- **TF Candidatos**: {240: 53574, 60: 11590, 15: 8044, 5: 5795}
- **TF Seleccionados**: {240: 2315, -1: 757, 15: 1237, 60: 864, 5: 511}
- **DistATR** - Candidatos: avg=17.7 | Seleccionados: avg=5.9
- **RR** - Candidatos: avg=6.93 | Seleccionados: avg=1.54
- **Razones de selección**: {'BestIntelligentScore': 5684}

### 🎯 Recomendaciones
- ⚠️ SL: 100% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.