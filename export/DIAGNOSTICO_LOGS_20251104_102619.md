# Informe Diagnóstico de Logs - 2025-11-04 10:33:48

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_102619.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_102619.csv`

## DFM
- Eventos de evaluación: 1682
- Evaluaciones Bull: 2313 | Bear: 173
- Pasaron umbral (PassedThreshold): 921
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:104, 4:999, 5:806, 6:456, 7:121, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5845
- KeptAligned: 6878/49521 | KeptCounter: 1231/12376
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.174 | AvgProxCounter≈ 0.037
  - AvgDistATRAligned≈ 3.10 | AvgDistATRCounter≈ 0.41
- PreferAligned eventos: 3117 | Filtradas contra-bias: 264

### Proximity (Pre-PreferAligned)
- Eventos: 5845
- Aligned pre: 6878/8109 | Counter pre: 1231/8109
- AvgProxAligned(pre)≈ 0.174 | AvgDistATRAligned(pre)≈ 3.10

### Proximity Drivers
- Eventos: 5845
- Alineadas: n=6878 | BaseProx≈ 0.432 | ZoneATR≈ 17.26 | SizePenalty≈ 0.764 | FinalProx≈ 0.329
- Contra-bias: n=967 | BaseProx≈ 0.446 | ZoneATR≈ 27.58 | SizePenalty≈ 0.639 | FinalProx≈ 0.278

## Risk
- Eventos: 3699
- Accepted=2596 | RejSL=3099 | RejTP=139 | RejRR=2011 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=2511 | SLDistATR≈ 23.71 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=588 | SLDistATR≈ 23.90 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:1418,20-25:554,25+:539
- HistSL Counter 0-10:0,10-15:0,15-20:267,20-25:161,25+:160

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 325 | Unmatched: 2271
- 0-10: Wins=47 Losses=43 WR=52.2% (n=90)
- 10-15: Wins=87 Losses=148 WR=37.0% (n=235)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=134 Losses=191 WR=41.2% (n=325)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2596 | Aligned=2320 (89.4%)
- Core≈ 1.00 | Prox≈ 0.34 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.95 | Confidence≈ 0.00
- SL_TF dist: {'-1': 489, '5': 754, '15': 444, '60': 808, '240': 97, '1440': 4} | SL_Structural≈ 81.2%
- TP_TF dist: {'-1': 830, '60': 484, '15': 600, '5': 496, '1440': 43, '240': 143} | TP_Structural≈ 68.0%

### SLPick por Bandas y TF
- Bandas: lt8=725, 8-10=287, 10-12.5=775, 12.5-15=809, >15=0
- TF: 5m=754, 15m=444, 60m=808, 240m=97, 1440m=4
- RR plan por bandas: 0-10≈ 2.51 (n=1012), 10-15≈ 1.59 (n=1584)

## CancelBias (EMA200@60m)
- Eventos: 243
- Distribución Bias: {'Bullish': 191, 'Bearish': 52, 'Neutral': 0}
- Coherencia (Close>EMA): 191/243 (78.6%)

## StructureFusion
- Trazas por zona: 61897 | Zonas con Anchors: 61298
- Dir zonas (zona): Bull=31247 Bear=30650 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 10.6, WithAnchors≈ 10.5, DirBull≈ 5.3, DirBear≈ 5.2, DirNeutral≈ 0.0
- Razones de dirección: {'triggers-only': 536, 'anchors+triggers': 60115, 'tie-bias': 1246}
- TF Triggers: {'5': 15257, '15': 22647, '60': 23993}
- TF Anchors: {'240': 60266, '1440': 53043}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5845 | Distribución: {'Bullish': 3736, 'Bearish': 2109, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3736/5845

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 3, 'score decayó a 0,39': 1}
- Cancel_BOS (diag): por acción {'BUY': 2, 'SELL': 0} | por bias {'Bullish': 0, 'Bearish': 2, 'Neutral': 0}

## CSV de Trades
- Filas: 173 | Ejecutadas: 75 | Canceladas: 0 | Expiradas: 0
- BUY: 227 | SELL: 21

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7785 | Total candidatos: 227701 | Seleccionados: 7240
- Candidatos por zona (promedio): 29.2
- **Edad (barras)** - Candidatos: med=38, max=150 | Seleccionados: med=40, max=148
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.52
- **TF Candidatos**: {240: 69867, 60: 59613, 15: 43106, 5: 35137, 1440: 19978}
- **TF Seleccionados**: {5: 2308, 60: 2669, 15: 1530, 1440: 50, 240: 683}
- **DistATR** - Candidatos: avg=64.6 | Seleccionados: avg=11.0
- **Razones de selección**: {'InBand[10,15]': 5328, 'Fallback<15': 1912}
- **En banda [10,15] ATR**: 19536/227701 (8.6%)

### Take Profit (TP)
- Zonas analizadas: 7845 | Total candidatos: 99303 | Seleccionados: 7845
- Candidatos por zona (promedio): 12.7
- **Edad (barras)** - Candidatos: med=36, max=150 | Seleccionados: med=43, max=150
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.37
- **Priority Candidatos**: {'P3': 99303}
- **Priority Seleccionados**: {'P4_Fallback': 1274, 'P3': 6571}
- **Type Candidatos**: {'Swing': 99303}
- **Type Seleccionados**: {'Calculated': 1274, 'Swing': 6571}
- **TF Candidatos**: {60: 25311, 15: 24886, 240: 21457, 5: 15567, 1440: 12082}
- **TF Seleccionados**: {-1: 1274, 5: 2012, 60: 1671, 15: 1966, 1440: 296, 240: 626}
- **DistATR** - Candidatos: avg=52.7 | Seleccionados: avg=16.8
- **RR** - Candidatos: avg=3.78 | Seleccionados: avg=1.18
- **Razones de selección**: {'NoStructuralTarget': 1274, 'R:R_y_Distancia_OK': 6238, 'R:R_OK_(Distancia_ignorada)': 312, 'Distancia_OK_(R:R_ignorado)': 21}

### 🎯 Recomendaciones
- ⚠️ SL: 51% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- KeptAligned ratio≈ 0.14.