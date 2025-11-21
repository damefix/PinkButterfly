# Informe Diagnóstico de Logs - 2025-11-19 18:39:28

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_181206.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_181206.csv`

## DFM
- Eventos de evaluación: 597
- Evaluaciones Bull: 0 | Bear: 506
- Pasaron umbral (PassedThreshold): 506
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:18, 6:180, 7:210, 8:98, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3094
- KeptAligned: 2944/2944 | KeptCounter: 2504/2682
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.202 | AvgProxCounter≈ 0.139
  - AvgDistATRAligned≈ 0.85 | AvgDistATRCounter≈ 0.70
- PreferAligned eventos: 865 | Filtradas contra-bias: 6

### Proximity (Pre-PreferAligned)
- Eventos: 3094
- Aligned pre: 2944/5448 | Counter pre: 2504/5448
- AvgProxAligned(pre)≈ 0.202 | AvgDistATRAligned(pre)≈ 0.85

### Proximity Drivers
- Eventos: 3094
- Alineadas: n=2944 | BaseProx≈ 0.713 | ZoneATR≈ 4.75 | SizePenalty≈ 0.981 | FinalProx≈ 0.700
- Contra-bias: n=2498 | BaseProx≈ 0.509 | ZoneATR≈ 5.94 | SizePenalty≈ 0.964 | FinalProx≈ 0.490

## Risk
- Eventos: 1736
- Accepted=724 | RejSL=0 | RejTP=0 | RejRR=1351 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 484 (12.5% del total)
  - Avg Score: 0.38 | Avg R:R: 1.78 | Avg DistATR: 3.92
  - Por TF: TF5=79, TF15=405
- **P0_SWING_LITE:** 3379 (87.5% del total)
  - Avg Score: 0.64 | Avg R:R: 3.20 | Avg DistATR: 3.84
  - Por TF: TF15=421, TF60=2958


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 24 | Unmatched: 700
- 0-10: Wins=20 Losses=4 WR=83.3% (n=24)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=20 Losses=4 WR=83.3% (n=24)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 724 | Aligned=422 (58.3%)
- Core≈ 1.00 | Prox≈ 0.61 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.25 | Confidence≈ 0.00
- SL_TF dist: {'15': 474, '5': 96, '60': 139, '240': 15} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 191, '240': 359, '60': 108, '5': 66} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=724, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=96, 15m=474, 60m=139, 240m=15, 1440m=0
- RR plan por bandas: 0-10≈ 2.25 (n=724), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 21703 | Zonas con Anchors: 21703
- Dir zonas (zona): Bull=49 Bear=21555 Neutral=99
- Resumen por ciclo (promedios): TotHZ≈ 6.9, WithAnchors≈ 6.9, DirBull≈ 0.0, DirBear≈ 6.9, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 21602, 'tie-bias': 101}
- TF Triggers: {'15': 11559, '5': 10144}
- TF Anchors: {'60': 21703, '240': 21703, '1440': 21703}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## Timing y Confluencia HTF
- [TIMING_ADAPT] eventos: 184 | Approach rejects: 114
- Score/Req promedio: 2.46/2.00
- [HTF_CONFL] muestras: 462 | ok=462 | rejects=0
- median≈ 0.000 | thr≈ 0.000
- [BIAS_FAST] muestras: 0 | Bull=0 Bear=0 Neutral=0 | rejects=25
- [RC_SL_MINFLOOR] filtrados: 0

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,49': 2, 'score decayó a 0,37': 1, 'estructura no existe': 7}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 8} | por bias {'Bullish': 8, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 73 | Ejecutadas: 10 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 83

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 506
- Registered: 37
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 37

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 7.3%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 27.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5198 | Total candidatos: 75833 | Seleccionados: 0
- Candidatos por zona (promedio): 14.6

### Take Profit (TP)
- Zonas analizadas: 5198 | Total candidatos: 131174 | Seleccionados: 5198
- Candidatos por zona (promedio): 25.2
- **Edad (barras)** - Candidatos: med=84, max=311 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.55 | Seleccionados: avg=0.72
- **Priority Candidatos**: {'P3': 72612}
- **Priority Seleccionados**: {'P3': 4247, 'NA': 586, 'P0': 365}
- **Type Candidatos**: {'Swing': 72612}
- **Type Seleccionados**: {'P3_Swing': 4247, 'P4_Fallback': 586, 'P0_Zone': 365}
- **TF Candidatos**: {240: 48133, 60: 10646, 15: 8203, 5: 5630}
- **TF Seleccionados**: {240: 2471, -1: 586, 15: 815, 60: 746, 5: 580}
- **DistATR** - Candidatos: avg=17.9 | Seleccionados: avg=6.5
- **RR** - Candidatos: avg=7.80 | Seleccionados: avg=1.56
- **Razones de selección**: {'BestIntelligentScore': 5198}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.