# Informe Diagnóstico de Logs - 2025-11-03 21:09:17

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251103_205720.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251103_205720.csv`

## DFM
- Eventos de evaluación: 4082
- Evaluaciones Bull: 3258 | Bear: 4033
- Pasaron umbral (PassedThreshold): 3428
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:1352, 4:1734, 5:1518, 6:1491, 7:1195, 8:1, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 6511
- KeptAligned: 27814/33414 | KeptCounter: 14328/19081
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.391 | AvgProxCounter≈ 0.236
  - AvgDistATRAligned≈ 3.32 | AvgDistATRCounter≈ 1.62
- PreferAligned eventos: 4728 | Filtradas contra-bias: 4775

### Proximity (Pre-PreferAligned)
- Eventos: 6511
- Aligned pre: 27814/42142 | Counter pre: 14328/42142
- AvgProxAligned(pre)≈ 0.391 | AvgDistATRAligned(pre)≈ 3.32

### Proximity Drivers
- Eventos: 6511
- Alineadas: n=27814 | BaseProx≈ 0.550 | ZoneATR≈ 5.31 | SizePenalty≈ 0.965 | FinalProx≈ 0.535
- Contra-bias: n=9553 | BaseProx≈ 0.434 | ZoneATR≈ 4.89 | SizePenalty≈ 0.971 | FinalProx≈ 0.425

## Risk
- Eventos: 6511
- Accepted=7375 | RejSL=7423 | RejTP=12779 | RejRR=9790 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=7053 | SLDistATR≈ 17.31 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=370 | SLDistATR≈ 17.68 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:6368,20-25:625,25+:60
- HistSL Counter 0-10:0,10-15:0,15-20:328,20-25:31,25+:11

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 67 | Unmatched: 7308
- 0-10: Wins=17 Losses=26 WR=39.5% (n=43)
- 10-15: Wins=3 Losses=21 WR=12.5% (n=24)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=20 Losses=47 WR=29.9% (n=67)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 7375 | Aligned=4101 (55.6%)
- Core≈ 1.00 | Prox≈ 0.44 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.06 | Confidence≈ 0.00
- SL_TF dist: {'-1': 1464, '15': 225, '240': 1890, '5': 74, '60': 1778, '1440': 1944} | SL_Structural≈ 80.1%
- TP_TF dist: {'-1': 6244, '5': 184, '15': 218, '60': 202, '240': 326, '1440': 201} | TP_Structural≈ 15.3%

### SLPick por Bandas y TF
- Bandas: lt8=1423, 8-10=3199, 10-12.5=1092, 12.5-15=1661, >15=0
- TF: 5m=74, 15m=225, 60m=1778, 240m=1890, 1440m=1944
- RR plan por bandas: 0-10≈ 1.08 (n=4622), 10-15≈ 1.02 (n=2753)

## CancelBias (EMA200@60m)
- Eventos: 216
- Distribución Bias: {'Bullish': 119, 'Bearish': 97, 'Neutral': 0}
- Coherencia (Close>EMA): 119/216 (55.1%)

## StructureFusion
- Trazas por zona: 52495 | Zonas con Anchors: 52009
- Dir zonas (zona): Bull=27043 Bear=25452 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.1, WithAnchors≈ 8.0, DirBull≈ 4.2, DirBear≈ 3.9, DirNeutral≈ 0.0
- Razones de dirección: {'tie-bias': 1445, 'triggers-only': 440, 'anchors+triggers': 50610}
- TF Triggers: {'5': 12535, '15': 17433, '60': 22527}
- TF Anchors: {'240': 51986, '1440': 44151}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 6512 | Distribución: {'Bullish': 5260, 'Bearish': 1252, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 5260/6512

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,39': 1, 'score decayó a 0,47': 2, 'estructura no existe': 4, 'score decayó a 0,50': 1}
- Cancel_BOS (diag): por acción {'BUY': 2, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 2, 'Neutral': 0}

## CSV de Trades
- Filas: 65 | Ejecutadas: 17 | Canceladas: 0 | Expiradas: 0
- BUY: 58 | SELL: 24

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 33898 | Total candidatos: 1859973 | Seleccionados: 33775
- Candidatos por zona (promedio): 54.9
- **Edad (barras)** - Candidatos: med=2, max=151 | Seleccionados: med=39, max=135
- **Score** - Candidatos: avg=0.75 | Seleccionados: avg=0.48
- **TF Candidatos**: {1440: 759966, 240: 682248, 60: 218988, 15: 124075, 5: 74696}
- **TF Seleccionados**: {15: 1100, 5: 290, 240: 15539, 60: 10903, 1440: 5943}
- **DistATR** - Candidatos: avg=9.0 | Seleccionados: avg=9.3
- **Razones de selección**: {'Fallback<15': 15587, 'InBand[10,15]': 18188}
- **En banda [10,15] ATR**: 112640/1859973 (6.1%)

### Take Profit (TP)
- Zonas analizadas: 37367 | Total candidatos: 520391 | Seleccionados: 37367
- Candidatos por zona (promedio): 13.9
- **Edad (barras)** - Candidatos: med=28, max=151 | Seleccionados: med=32, max=151
- **Score** - Candidatos: avg=0.57 | Seleccionados: avg=0.35
- **Priority Candidatos**: {'P3': 520391}
- **Priority Seleccionados**: {'P4_Fallback': 8633, 'P3': 28734}
- **Type Candidatos**: {'Swing': 520391}
- **Type Seleccionados**: {'Calculated': 8633, 'Swing': 28734}
- **TF Candidatos**: {15: 186559, 5: 105747, 60: 101331, 240: 80832, 1440: 45922}
- **TF Seleccionados**: {-1: 8633, 15: 7111, 5: 9110, 60: 7078, 240: 4446, 1440: 989}
- **DistATR** - Candidatos: avg=7.2 | Seleccionados: avg=5.3
- **RR** - Candidatos: avg=0.76 | Seleccionados: avg=0.47
- **Razones de selección**: {'NoStructuralTarget': 8633, 'R:R_y_Distancia_OK': 28721, 'R:R_OK_(Distancia_ignorada)': 13}

### 🎯 Recomendaciones
- ⚠️ SL: 60% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- KeptAligned ratio≈ 0.83.