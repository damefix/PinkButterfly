# Informe Diagnóstico de Logs - 2025-11-04 07:56:01

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_074812.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_074812.csv`

## DFM
- Eventos de evaluación: 2145
- Evaluaciones Bull: 2259 | Bear: 695
- Pasaron umbral (PassedThreshold): 943
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:94, 4:1144, 5:1154, 6:435, 7:127, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5644
- KeptAligned: 7312/44741 | KeptCounter: 1202/12190
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.207 | AvgProxCounter≈ 0.041
  - AvgDistATRAligned≈ 3.89 | AvgDistATRCounter≈ 0.41
- PreferAligned eventos: 3686 | Filtradas contra-bias: 267

### Proximity (Pre-PreferAligned)
- Eventos: 5644
- Aligned pre: 7312/8514 | Counter pre: 1202/8514
- AvgProxAligned(pre)≈ 0.207 | AvgDistATRAligned(pre)≈ 3.89

### Proximity Drivers
- Eventos: 5644
- Alineadas: n=7312 | BaseProx≈ 0.422 | ZoneATR≈ 17.27 | SizePenalty≈ 0.765 | FinalProx≈ 0.322
- Contra-bias: n=935 | BaseProx≈ 0.441 | ZoneATR≈ 27.35 | SizePenalty≈ 0.641 | FinalProx≈ 0.281

## Risk
- Eventos: 4255
- Accepted=3072 | RejSL=3172 | RejTP=112 | RejRR=1891 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=2574 | SLDistATR≈ 23.61 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=598 | SLDistATR≈ 23.94 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:1482,20-25:561,25+:531
- HistSL Counter 0-10:0,10-15:0,15-20:263,20-25:170,25+:165

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 81 | Unmatched: 2991
- 0-10: Wins=12 Losses=28 WR=30.0% (n=40)
- 10-15: Wins=14 Losses=27 WR=34.1% (n=41)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=26 Losses=55 WR=32.1% (n=81)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 3072 | Aligned=2825 (92.0%)
- Core≈ 1.00 | Prox≈ 0.33 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.60 | Confidence≈ 0.00
- SL_TF dist: {'-1': 1005, '5': 701, '15': 470, '60': 799, '240': 94, '1440': 3} | SL_Structural≈ 67.3%
- TP_TF dist: {'-1': 839, '5': 465, '15': 619, '60': 484, '240': 150, '1440': 515} | TP_Structural≈ 72.7%

### SLPick por Bandas y TF
- Bandas: lt8=704, 8-10=300, 10-12.5=1235, 12.5-15=833, >15=0
- TF: 5m=701, 15m=470, 60m=799, 240m=94, 1440m=3
- RR plan por bandas: 0-10≈ 2.33 (n=1004), 10-15≈ 2.72 (n=2068)

## CancelBias (EMA200@60m)
- Eventos: 101
- Distribución Bias: {'Bullish': 76, 'Bearish': 25, 'Neutral': 0}
- Coherencia (Close>EMA): 76/101 (75.2%)

## StructureFusion
- Trazas por zona: 56931 | Zonas con Anchors: 56261
- Dir zonas (zona): Bull=30542 Bear=26389 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 10.1, WithAnchors≈ 10.0, DirBull≈ 5.4, DirBear≈ 4.7, DirNeutral≈ 0.0
- Razones de dirección: {'tie-bias': 1381, 'triggers-only': 595, 'anchors+triggers': 54955}
- TF Triggers: {'5': 14856, '15': 19464, '60': 22611}
- TF Anchors: {'240': 56236, '1440': 47374}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5646 | Distribución: {'Bullish': 3736, 'Bearish': 1910, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3736/5646

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,45': 1, 'score decayó a 0,42': 1}
- Cancel_BOS (diag): por acción {'BUY': 1, 'SELL': 0} | por bias {'Bullish': 0, 'Bearish': 1, 'Neutral': 0}

## CSV de Trades
- Filas: 41 | Ejecutadas: 17 | Canceladas: 0 | Expiradas: 0
- BUY: 48 | SELL: 10

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 8160 | Total candidatos: 232600 | Seleccionados: 7043
- Candidatos por zona (promedio): 28.5
- **Edad (barras)** - Candidatos: med=38, max=151 | Seleccionados: med=41, max=151
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.52
- **TF Candidatos**: {240: 69372, 60: 65246, 15: 45134, 5: 33217, 1440: 19631}
- **TF Seleccionados**: {5: 2211, 15: 1498, 60: 2601, 240: 686, 1440: 47}
- **DistATR** - Candidatos: avg=65.6 | Seleccionados: avg=11.0
- **Razones de selección**: {'Fallback<15': 1883, 'InBand[10,15]': 5160}
- **En banda [10,15] ATR**: 18767/232600 (8.1%)

### Take Profit (TP)
- Zonas analizadas: 8247 | Total candidatos: 99346 | Seleccionados: 8247
- Candidatos por zona (promedio): 12.0
- **Edad (barras)** - Candidatos: med=35, max=151 | Seleccionados: med=40, max=150
- **Score** - Candidatos: avg=0.50 | Seleccionados: avg=0.38
- **Priority Candidatos**: {'P3': 99346}
- **Priority Seleccionados**: {'P4_Fallback': 1297, 'P3': 6950}
- **Type Candidatos**: {'Swing': 99346}
- **Type Seleccionados**: {'Calculated': 1297, 'Swing': 6950}
- **TF Candidatos**: {60: 23891, 15: 23726, 240: 20986, 1440: 15763, 5: 14980}
- **TF Seleccionados**: {-1: 1297, 5: 1948, 15: 1974, 60: 1660, 240: 650, 1440: 718}
- **DistATR** - Candidatos: avg=62.7 | Seleccionados: avg=21.0
- **RR** - Candidatos: avg=4.50 | Seleccionados: avg=1.50
- **Razones de selección**: {'NoStructuralTarget': 1297, 'R:R_y_Distancia_OK': 6032, 'R:R_OK_(Distancia_ignorada)': 899, 'Distancia_OK_(R:R_ignorado)': 19}

### 🎯 Recomendaciones
- ⚠️ SL: 52% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- KeptAligned ratio≈ 0.16.