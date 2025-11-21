# Informe Diagnóstico de Logs - 2025-11-19 15:43:37

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_153900.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_153900.csv`

## DFM
- Eventos de evaluación: 646
- Evaluaciones Bull: 0 | Bear: 622
- Pasaron umbral (PassedThreshold): 622
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:35, 6:203, 7:277, 8:107, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2921
- KeptAligned: 3277/3277 | KeptCounter: 2557/2723
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.193 | AvgProxCounter≈ 0.129
  - AvgDistATRAligned≈ 0.88 | AvgDistATRCounter≈ 0.72
- PreferAligned eventos: 776 | Filtradas contra-bias: 10

### Proximity (Pre-PreferAligned)
- Eventos: 2921
- Aligned pre: 3277/5834 | Counter pre: 2557/5834
- AvgProxAligned(pre)≈ 0.193 | AvgDistATRAligned(pre)≈ 0.88

### Proximity Drivers
- Eventos: 2921
- Alineadas: n=3277 | BaseProx≈ 0.712 | ZoneATR≈ 4.57 | SizePenalty≈ 0.982 | FinalProx≈ 0.699
- Contra-bias: n=2547 | BaseProx≈ 0.494 | ZoneATR≈ 5.85 | SizePenalty≈ 0.963 | FinalProx≈ 0.476

## Risk
- Eventos: 1581
- Accepted=942 | RejSL=0 | RejTP=0 | RejRR=1807 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 925 (19.3% del total)
  - Avg Score: 0.40 | Avg R:R: 1.87 | Avg DistATR: 3.75
  - Por TF: TF5=117, TF15=808
- **P0_SWING_LITE:** 3863 (80.7% del total)
  - Avg Score: 0.58 | Avg R:R: 3.59 | Avg DistATR: 3.92
  - Por TF: TF15=957, TF60=2906


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 83 | Unmatched: 859
- 0-10: Wins=16 Losses=67 WR=19.3% (n=83)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=16 Losses=67 WR=19.3% (n=83)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 942 | Aligned=573 (60.8%)
- Core≈ 1.00 | Prox≈ 0.61 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.24 | Confidence≈ 0.00
- SL_TF dist: {'15': 575, '5': 196, '240': 91, '60': 80} | SL_Structural≈ 100.0%
- TP_TF dist: {'240': 516, '15': 184, '60': 58, '5': 184} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=942, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=196, 15m=575, 60m=80, 240m=91, 1440m=0
- RR plan por bandas: 0-10≈ 2.24 (n=942), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 18943 | Zonas con Anchors: 18943
- Dir zonas (zona): Bull=43 Bear=18693 Neutral=207
- Resumen por ciclo (promedios): TotHZ≈ 6.1, WithAnchors≈ 6.1, DirBull≈ 0.0, DirBear≈ 6.0, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 18728, 'tie-bias': 215}
- TF Triggers: {'15': 11041, '5': 7902}
- TF Anchors: {'60': 18943, '240': 18943, '1440': 18943}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,36': 1, 'estructura no existe': 5, 'score decayó a 0,14': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 5} | por bias {'Bullish': 5, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 64 | Ejecutadas: 4 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 68

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 622
- Registered: 39
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 39

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 6.3%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 10.3%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5547 | Total candidatos: 88291 | Seleccionados: 0
- Candidatos por zona (promedio): 15.9

### Take Profit (TP)
- Zonas analizadas: 5547 | Total candidatos: 138539 | Seleccionados: 5547
- Candidatos por zona (promedio): 25.0
- **Edad (barras)** - Candidatos: med=92, max=785 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.53 | Seleccionados: avg=0.74
- **Priority Candidatos**: {'P3': 84062}
- **Priority Seleccionados**: {'P3': 4166, 'P0': 533, 'NA': 848}
- **Type Candidatos**: {'Swing': 84062}
- **Type Seleccionados**: {'P3_Swing': 4166, 'P0_Zone': 533, 'P4_Fallback': 848}
- **TF Candidatos**: {240: 46834, 15: 16917, 60: 12111, 5: 8200}
- **TF Seleccionados**: {240: 2503, 15: 1117, -1: 848, 60: 408, 5: 671}
- **DistATR** - Candidatos: avg=15.3 | Seleccionados: avg=5.0
- **RR** - Candidatos: avg=6.82 | Seleccionados: avg=1.56
- **Razones de selección**: {'BestIntelligentScore': 5547}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.