# Informe Diagnóstico de Logs - 2025-11-04 10:24:50

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_101851.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_101851.csv`

## DFM
- Eventos de evaluación: 1683
- Evaluaciones Bull: 2313 | Bear: 174
- Pasaron umbral (PassedThreshold): 925
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:102, 4:991, 5:806, 6:446, 7:142, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 6041
- KeptAligned: 7249/51024 | KeptCounter: 1265/12327
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.183 | AvgProxCounter≈ 0.041
  - AvgDistATRAligned≈ 3.47 | AvgDistATRCounter≈ 0.39
- PreferAligned eventos: 3506 | Filtradas contra-bias: 288

### Proximity (Pre-PreferAligned)
- Eventos: 6041
- Aligned pre: 7249/8514 | Counter pre: 1265/8514
- AvgProxAligned(pre)≈ 0.183 | AvgDistATRAligned(pre)≈ 3.47

### Proximity Drivers
- Eventos: 6041
- Alineadas: n=7249 | BaseProx≈ 0.426 | ZoneATR≈ 17.11 | SizePenalty≈ 0.768 | FinalProx≈ 0.325
- Contra-bias: n=977 | BaseProx≈ 0.448 | ZoneATR≈ 27.05 | SizePenalty≈ 0.643 | FinalProx≈ 0.285

## Risk
- Eventos: 4103
- Accepted=2608 | RejSL=3483 | RejTP=124 | RejRR=2011 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=2884 | SLDistATR≈ 23.25 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=599 | SLDistATR≈ 23.86 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:1765,20-25:564,25+:555
- HistSL Counter 0-10:0,10-15:0,15-20:265,20-25:167,25+:167

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 422 | Unmatched: 2186
- 0-10: Wins=37 Losses=71 WR=34.3% (n=108)
- 10-15: Wins=131 Losses=183 WR=41.7% (n=314)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=168 Losses=254 WR=39.8% (n=422)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2608 | Aligned=2330 (89.3%)
- Core≈ 0.99 | Prox≈ 0.35 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.92 | Confidence≈ 0.00
- SL_TF dist: {'-1': 489, '5': 751, '60': 806, '15': 459, '240': 100, '1440': 3} | SL_Structural≈ 81.2%
- TP_TF dist: {'-1': 831, '60': 485, '15': 610, '5': 499, '1440': 42, '240': 141} | TP_Structural≈ 68.1%

### SLPick por Bandas y TF
- Bandas: lt8=713, 8-10=301, 10-12.5=779, 12.5-15=815, >15=0
- TF: 5m=751, 15m=459, 60m=806, 240m=100, 1440m=3
- RR plan por bandas: 0-10≈ 2.45 (n=1014), 10-15≈ 1.59 (n=1594)

## CancelBias (EMA200@60m)
- Eventos: 305
- Distribución Bias: {'Bullish': 257, 'Bearish': 48, 'Neutral': 0}
- Coherencia (Close>EMA): 257/305 (84.3%)

## StructureFusion
- Trazas por zona: 63351 | Zonas con Anchors: 62771
- Dir zonas (zona): Bull=31230 Bear=32121 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 10.5, WithAnchors≈ 10.4, DirBull≈ 5.2, DirBear≈ 5.3, DirNeutral≈ 0.0
- Razones de dirección: {'triggers-only': 523, 'anchors+triggers': 61591, 'tie-bias': 1237}
- TF Triggers: {'5': 14576, '15': 23794, '60': 24981}
- TF Anchors: {'240': 62654, '1440': 54485}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 6042 | Distribución: {'Bullish': 3736, 'Bearish': 2306, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3736/6042

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,39': 1, 'estructura no existe': 3}

## CSV de Trades
- Filas: 210 | Ejecutadas: 99 | Canceladas: 0 | Expiradas: 0
- BUY: 274 | SELL: 35

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 8163 | Total candidatos: 235887 | Seleccionados: 7614
- Candidatos por zona (promedio): 28.9
- **Edad (barras)** - Candidatos: med=38, max=151 | Seleccionados: med=39, max=151
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.54
- **TF Candidatos**: {240: 70966, 60: 63969, 15: 45428, 5: 35107, 1440: 20417}
- **TF Seleccionados**: {5: 2353, 60: 2612, 15: 1918, 1440: 50, 240: 681}
- **DistATR** - Candidatos: avg=65.5 | Seleccionados: avg=10.7
- **Razones de selección**: {'InBand[10,15]': 5330, 'Fallback<15': 2284}
- **En banda [10,15] ATR**: 19507/235887 (8.3%)

### Take Profit (TP)
- Zonas analizadas: 8226 | Total candidatos: 102463 | Seleccionados: 8226
- Candidatos por zona (promedio): 12.5
- **Edad (barras)** - Candidatos: med=35, max=151 | Seleccionados: med=41, max=150
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.38
- **Priority Candidatos**: {'P3': 102463}
- **Priority Seleccionados**: {'P4_Fallback': 1277, 'P3': 6949}
- **Type Candidatos**: {'Swing': 102463}
- **Type Seleccionados**: {'Calculated': 1277, 'Swing': 6949}
- **TF Candidatos**: {60: 25120, 15: 24566, 240: 21546, 1440: 15663, 5: 15568}
- **TF Seleccionados**: {-1: 1277, 5: 2016, 60: 1671, 15: 1960, 1440: 668, 240: 634}
- **DistATR** - Candidatos: avg=58.7 | Seleccionados: avg=18.7
- **RR** - Candidatos: avg=4.09 | Seleccionados: avg=1.26
- **Razones de selección**: {'NoStructuralTarget': 1277, 'R:R_y_Distancia_OK': 6249, 'R:R_OK_(Distancia_ignorada)': 679, 'Distancia_OK_(R:R_ignorado)': 21}

### 🎯 Recomendaciones
- ⚠️ SL: 49% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- KeptAligned ratio≈ 0.14.