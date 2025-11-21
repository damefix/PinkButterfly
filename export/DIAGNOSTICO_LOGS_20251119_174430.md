# Informe Diagnóstico de Logs - 2025-11-19 17:57:17

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_174430.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_174430.csv`

## DFM
- Eventos de evaluación: 496
- Evaluaciones Bull: 0 | Bear: 373
- Pasaron umbral (PassedThreshold): 373
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:13, 6:146, 7:160, 8:54, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3108
- KeptAligned: 2997/2997 | KeptCounter: 2692/2866
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.199 | AvgProxCounter≈ 0.135
  - AvgDistATRAligned≈ 0.85 | AvgDistATRCounter≈ 0.74
- PreferAligned eventos: 869 | Filtradas contra-bias: 9

### Proximity (Pre-PreferAligned)
- Eventos: 3108
- Aligned pre: 2997/5689 | Counter pre: 2692/5689
- AvgProxAligned(pre)≈ 0.199 | AvgDistATRAligned(pre)≈ 0.85

### Proximity Drivers
- Eventos: 3108
- Alineadas: n=2997 | BaseProx≈ 0.717 | ZoneATR≈ 4.86 | SizePenalty≈ 0.981 | FinalProx≈ 0.704
- Contra-bias: n=2683 | BaseProx≈ 0.501 | ZoneATR≈ 5.87 | SizePenalty≈ 0.966 | FinalProx≈ 0.485

## Risk
- Eventos: 1747
- Accepted=605 | RejSL=0 | RejTP=0 | RejRR=1427 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 418 (10.6% del total)
  - Avg Score: 0.39 | Avg R:R: 1.88 | Avg DistATR: 3.97
  - Por TF: TF5=79, TF15=339
- **P0_SWING_LITE:** 3544 (89.4% del total)
  - Avg Score: 0.64 | Avg R:R: 3.26 | Avg DistATR: 3.86
  - Por TF: TF15=403, TF60=3141


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 23 | Unmatched: 582
- 0-10: Wins=3 Losses=20 WR=13.0% (n=23)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=3 Losses=20 WR=13.0% (n=23)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 605 | Aligned=305 (50.4%)
- Core≈ 1.00 | Prox≈ 0.59 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.45 | Confidence≈ 0.00
- SL_TF dist: {'15': 355, '5': 219, '60': 20, '240': 11} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 81, '240': 344, '60': 88, '5': 92} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=605, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=219, 15m=355, 60m=20, 240m=11, 1440m=0
- RR plan por bandas: 0-10≈ 2.46 (n=605), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 22420 | Zonas con Anchors: 22420
- Dir zonas (zona): Bull=98 Bear=22251 Neutral=71
- Resumen por ciclo (promedios): TotHZ≈ 7.2, WithAnchors≈ 7.2, DirBull≈ 0.0, DirBear≈ 7.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 22340, 'tie-bias': 80}
- TF Triggers: {'5': 10453, '15': 11967}
- TF Anchors: {'60': 22420, '240': 22420, '1440': 22420}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## Timing y Confluencia HTF
- [TIMING_ADAPT] eventos: 46 | Approach rejects: 175
- Score/Req promedio: 2.43/2.00
- [HTF_CONFL] muestras: 32 | ok=32 | rejects=0
- median≈ 0.000 | thr≈ 0.000
- [BIAS_FAST] muestras: 0 | Bull=0 Bear=0 Neutral=0 | rejects=14
- [RC_SL_MINFLOOR] filtrados: 0

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 4}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 6} | por bias {'Bullish': 6, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 48 | Ejecutadas: 5 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 53

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 373
- Registered: 25
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 25

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 6.7%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 20.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5386 | Total candidatos: 73648 | Seleccionados: 0
- Candidatos por zona (promedio): 13.7

### Take Profit (TP)
- Zonas analizadas: 5386 | Total candidatos: 134018 | Seleccionados: 5386
- Candidatos por zona (promedio): 24.9
- **Edad (barras)** - Candidatos: med=85, max=501 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.54 | Seleccionados: avg=0.71
- **Priority Candidatos**: {'P3': 75364}
- **Priority Seleccionados**: {'P3': 4156, 'NA': 832, 'P0': 398}
- **Type Candidatos**: {'Swing': 75364}
- **Type Seleccionados**: {'P3_Swing': 4156, 'P4_Fallback': 832, 'P0_Zone': 398}
- **TF Candidatos**: {240: 49383, 60: 10681, 15: 9334, 5: 5966}
- **TF Seleccionados**: {240: 2570, -1: 832, 15: 907, 60: 426, 5: 651}
- **DistATR** - Candidatos: avg=18.4 | Seleccionados: avg=6.2
- **RR** - Candidatos: avg=6.67 | Seleccionados: avg=1.55
- **Razones de selección**: {'BestIntelligentScore': 5386}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.