# Informe Diagnóstico de Logs - 2025-11-04 07:41:44

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_073214.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_073214.csv`

## DFM
- Eventos de evaluación: 1684
- Evaluaciones Bull: 2263 | Bear: 235
- Pasaron umbral (PassedThreshold): 891
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:95, 4:1077, 5:762, 6:436, 7:128, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 6739
- KeptAligned: 6760/62806 | KeptCounter: 1206/12368
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.150 | AvgProxCounter≈ 0.034
  - AvgDistATRAligned≈ 2.75 | AvgDistATRCounter≈ 0.35
- PreferAligned eventos: 3144 | Filtradas contra-bias: 271

### Proximity (Pre-PreferAligned)
- Eventos: 6739
- Aligned pre: 6760/7966 | Counter pre: 1206/7966
- AvgProxAligned(pre)≈ 0.150 | AvgDistATRAligned(pre)≈ 2.75

### Proximity Drivers
- Eventos: 6739
- Alineadas: n=6760 | BaseProx≈ 0.425 | ZoneATR≈ 17.29 | SizePenalty≈ 0.765 | FinalProx≈ 0.324
- Contra-bias: n=935 | BaseProx≈ 0.441 | ZoneATR≈ 27.36 | SizePenalty≈ 0.640 | FinalProx≈ 0.281

## Risk
- Eventos: 3713
- Accepted=2611 | RejSL=3075 | RejTP=112 | RejRR=1897 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=2476 | SLDistATR≈ 23.59 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=599 | SLDistATR≈ 23.93 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:1403,20-25:563,25+:510
- HistSL Counter 0-10:0,10-15:0,15-20:264,20-25:170,25+:165

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 83 | Unmatched: 2528
- 0-10: Wins=12 Losses=28 WR=30.0% (n=40)
- 10-15: Wins=16 Losses=27 WR=37.2% (n=43)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=28 Losses=55 WR=33.7% (n=83)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2611 | Aligned=2365 (90.6%)
- Core≈ 1.00 | Prox≈ 0.33 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.95 | Confidence≈ 0.00
- SL_TF dist: {'-1': 544, '5': 701, '15': 470, '60': 799, '240': 94, '1440': 3} | SL_Structural≈ 79.2%
- TP_TF dist: {'-1': 841, '15': 619, '5': 464, '60': 484, '240': 150, '1440': 53} | TP_Structural≈ 67.8%

### SLPick por Bandas y TF
- Bandas: lt8=706, 8-10=296, 10-12.5=737, 12.5-15=872, >15=0
- TF: 5m=701, 15m=470, 60m=799, 240m=94, 1440m=3
- RR plan por bandas: 0-10≈ 2.33 (n=1002), 10-15≈ 1.70 (n=1609)

## CancelBias (EMA200@60m)
- Eventos: 108
- Distribución Bias: {'Bullish': 76, 'Bearish': 32, 'Neutral': 0}
- Coherencia (Close>EMA): 76/108 (70.4%)

## StructureFusion
- Trazas por zona: 75174 | Zonas con Anchors: 74496
- Dir zonas (zona): Bull=30733 Bear=44441 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 11.2, WithAnchors≈ 11.1, DirBull≈ 4.6, DirBear≈ 6.6, DirNeutral≈ 0.0
- Razones de dirección: {'triggers-only': 600, 'tie-bias': 1384, 'anchors+triggers': 73190}
- TF Triggers: {'5': 21995, '15': 24924, '60': 28255}
- TF Anchors: {'240': 74471, '1440': 65559}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 6739 | Distribución: {'Bullish': 3736, 'Bearish': 3003, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3736/6739

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,45': 1, 'score decayó a 0,42': 1}
- Cancel_BOS (diag): por acción {'BUY': 1, 'SELL': 0} | por bias {'Bullish': 0, 'Bearish': 1, 'Neutral': 0}

## CSV de Trades
- Filas: 43 | Ejecutadas: 18 | Canceladas: 0 | Expiradas: 0
- BUY: 48 | SELL: 13

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7605 | Total candidatos: 220921 | Seleccionados: 7026
- Candidatos por zona (promedio): 29.0
- **Edad (barras)** - Candidatos: med=38, max=151 | Seleccionados: med=41, max=151
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.52
- **TF Candidatos**: {240: 68179, 60: 58713, 15: 41671, 5: 33267, 1440: 19091}
- **TF Seleccionados**: {5: 2213, 15: 1480, 60: 2600, 240: 686, 1440: 47}
- **DistATR** - Candidatos: avg=64.9 | Seleccionados: avg=11.0
- **Razones de selección**: {'Fallback<15': 1883, 'InBand[10,15]': 5143}
- **En banda [10,15] ATR**: 18598/220921 (8.4%)

### Take Profit (TP)
- Zonas analizadas: 7695 | Total candidatos: 94470 | Seleccionados: 7695
- Candidatos por zona (promedio): 12.3
- **Edad (barras)** - Candidatos: med=36, max=151 | Seleccionados: med=43, max=150
- **Score** - Candidatos: avg=0.50 | Seleccionados: avg=0.36
- **Priority Candidatos**: {'P3': 94470}
- **Priority Seleccionados**: {'P4_Fallback': 1299, 'P3': 6396}
- **Type Candidatos**: {'Swing': 94470}
- **Type Seleccionados**: {'Calculated': 1299, 'Swing': 6396}
- **TF Candidatos**: {60: 23870, 15: 23714, 240: 20978, 5: 15005, 1440: 10903}
- **TF Seleccionados**: {-1: 1299, 5: 1950, 15: 1957, 60: 1661, 240: 650, 1440: 178}
- **DistATR** - Candidatos: avg=54.2 | Seleccionados: avg=17.4
- **RR** - Candidatos: avg=3.76 | Seleccionados: avg=1.19
- **Razones de selección**: {'NoStructuralTarget': 1299, 'R:R_y_Distancia_OK': 6018, 'R:R_OK_(Distancia_ignorada)': 359, 'Distancia_OK_(R:R_ignorado)': 19}

### 🎯 Recomendaciones
- ⚠️ SL: 52% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- KeptAligned ratio≈ 0.11.