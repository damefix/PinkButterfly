# Informe Diagnóstico de Logs - 2025-11-03 08:59:09

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251103_085545.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251103_085545.csv`

## DFM
- Eventos de evaluación: 1400
- Evaluaciones Bull: 1382 | Bear: 327
- Pasaron umbral (PassedThreshold): 622
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:155, 4:652, 5:493, 6:321, 7:88, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 2566/26817 | KeptCounter: 1246/14333
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.117 | AvgProxCounter≈ 0.049
  - AvgDistATRAligned≈ 1.72 | AvgDistATRCounter≈ 0.42
- PreferAligned eventos: 1718 | Filtradas contra-bias: 117

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 2566/3812 | Counter pre: 1246/3812
- AvgProxAligned(pre)≈ 0.117 | AvgDistATRAligned(pre)≈ 1.72

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=2566 | BaseProx≈ 0.444 | ZoneATR≈ 17.27 | SizePenalty≈ 0.771 | FinalProx≈ 0.340
- Contra-bias: n=1129 | BaseProx≈ 0.492 | ZoneATR≈ 31.08 | SizePenalty≈ 0.575 | FinalProx≈ 0.276

## Risk
- Eventos: 2517
- Accepted=1799 | RejSL=1242 | RejTP=43 | RejRR=611 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=842 | SLDistATR≈ 26.79 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=400 | SLDistATR≈ 25.73 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:405,20-25:203,25+:234
- HistSL Counter 0-10:0,10-15:0,15-20:174,20-25:78,25+:148

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 306 | Unmatched: 1493
- 0-10: Wins=34 Losses=64 WR=34.7% (n=98)
- 10-15: Wins=116 Losses=92 WR=55.8% (n=208)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=150 Losses=156 WR=49.0% (n=306)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1799 | Aligned=1192 (66.3%)
- Core≈ 0.99 | Prox≈ 0.35 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.97 | Confidence≈ 0.00
- SL_TF dist: {'-1': 583, '15': 1030, '5': 186} | SL_Structural≈ 67.6%
- TP_TF dist: {'15': 702, '-1': 961, '5': 136} | TP_Structural≈ 46.6%

### SLPick por Bandas y TF
- Bandas: lt8=582, 8-10=249, 10-12.5=495, 12.5-15=473, >15=0
- TF: 5m=186, 15m=1030, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.57 (n=831), 10-15≈ 1.45 (n=968)

## CancelBias (EMA200@60m)
- Eventos: 239
- Distribución Bias: {'Bullish': 205, 'Bearish': 34, 'Neutral': 0}
- Coherencia (Close>EMA): 205/239 (85.8%)

## StructureFusion
- Trazas por zona: 41150 | Zonas con Anchors: 40792
- Dir zonas (zona): Bull=25610 Bear=15540 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.2, WithAnchors≈ 8.2, DirBull≈ 5.1, DirBear≈ 3.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 39756, 'tie-bias': 1079, 'triggers-only': 315}
- TF Triggers: {'60': 20208, '15': 16446, '5': 4496}
- TF Anchors: {'240': 40769, '1440': 35086}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 3, 'score decayó a 0,30': 1, 'score decayó a 0,44': 2}
- Cancel_BOS (diag): por acción {'BUY': 6, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 6, 'Neutral': 0}

## CSV de Trades
- Filas: 136 | Ejecutadas: 54 | Canceladas: 0 | Expiradas: 0
- BUY: 164 | SELL: 26

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 3194 | Total candidatos: 29032 | Seleccionados: 2144
- Candidatos por zona (promedio): 9.1
- **Edad (barras)** - Candidatos: med=46, max=150 | Seleccionados: med=48, max=145
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.46
- **TF Candidatos**: {15: 21666}
- **TF Seleccionados**: {15: 2144}
- **DistATR** - Candidatos: avg=20.1 | Seleccionados: avg=10.5
- **Razones de selección**: {'Fallback<15': 762, 'InBand[10,15]': 1382}
- **En banda [10,15] ATR**: 3462/21666 (16.0%)

### Take Profit (TP)
- Zonas analizadas: 3695 | Total candidatos: 51901 | Seleccionados: 3146
- Candidatos por zona (promedio): 14.0
- **Edad (barras)** - Candidatos: med=18774, max=23176 | Seleccionados: med=6, max=145
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.22
- **Priority Candidatos**: {'P3': 48874}
- **Priority Seleccionados**: {'P3': 1645, 'P4_Fallback': 1501}
- **Type Candidatos**: {'Swing': 48874}
- **Type Seleccionados**: {'Swing': 1645, 'Calculated': 1501}
- **TF Candidatos**: {1440: 14650, 240: 13437, 60: 11460, 15: 9327}
- **TF Seleccionados**: {15: 1645, -1: 1501}
- **DistATR** - Candidatos: avg=122.7 | Seleccionados: avg=17.7
- **RR** - Candidatos: avg=9.63 | Seleccionados: avg=1.43
- **Razones de selección**: {'R:R_y_Distancia_OK': 1528, 'NoStructuralTarget': 1501, 'Distancia_OK_(R:R_ignorado)': 27, 'R:R_OK_(Distancia_ignorada)': 90}

### 🎯 Recomendaciones
- ⚠️ SL: 62% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 48% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.10.