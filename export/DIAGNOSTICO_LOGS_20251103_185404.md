# Informe Diagnóstico de Logs - 2025-11-03 18:59:50

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251103_185404.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251103_185404.csv`

## DFM
- Eventos de evaluación: 1739
- Evaluaciones Bull: 2358 | Bear: 179
- Pasaron umbral (PassedThreshold): 947
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:91, 4:985, 5:864, 6:460, 7:136, 8:1, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 6452
- KeptAligned: 7085/42232 | KeptCounter: 1273/17480
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.168 | AvgProxCounter≈ 0.038
  - AvgDistATRAligned≈ 2.96 | AvgDistATRCounter≈ 0.38
- PreferAligned eventos: 3289 | Filtradas contra-bias: 332

### Proximity (Pre-PreferAligned)
- Eventos: 6452
- Aligned pre: 7085/8358 | Counter pre: 1273/8358
- AvgProxAligned(pre)≈ 0.168 | AvgDistATRAligned(pre)≈ 2.96

### Proximity Drivers
- Eventos: 6452
- Alineadas: n=7085 | BaseProx≈ 0.433 | ZoneATR≈ 17.27 | SizePenalty≈ 0.766 | FinalProx≈ 0.331
- Contra-bias: n=941 | BaseProx≈ 0.448 | ZoneATR≈ 27.93 | SizePenalty≈ 0.629 | FinalProx≈ 0.280

## Risk
- Eventos: 3872
- Accepted=2669 | RejSL=3223 | RejTP=130 | RejRR=2004 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=2611 | SLDistATR≈ 23.71 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=612 | SLDistATR≈ 23.76 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:1447,20-25:598,25+:566
- HistSL Counter 0-10:0,10-15:0,15-20:280,20-25:168,25+:164

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 110 | Unmatched: 2559
- 0-10: Wins=7 Losses=48 WR=12.7% (n=55)
- 10-15: Wins=25 Losses=30 WR=45.5% (n=55)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=32 Losses=78 WR=29.1% (n=110)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2669 | Aligned=2431 (91.1%)
- Core≈ 0.99 | Prox≈ 0.35 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.81 | Confidence≈ 0.00
- SL_TF dist: {'15': 514, '-1': 452, '60': 809, '5': 725, '240': 166, '1440': 3} | SL_Structural≈ 83.1%
- TP_TF dist: {'-1': 871, '5': 530, '240': 178, '60': 452, '15': 635, '1440': 3} | TP_Structural≈ 67.4%

### SLPick por Bandas y TF
- Bandas: lt8=687, 8-10=311, 10-12.5=798, 12.5-15=873, >15=0
- TF: 5m=725, 15m=514, 60m=809, 240m=166, 1440m=3
- RR plan por bandas: 0-10≈ 2.24 (n=998), 10-15≈ 1.56 (n=1671)

## CancelBias (EMA200@60m)
- Eventos: 80
- Distribución Bias: {'Bullish': 56, 'Bearish': 24, 'Neutral': 0}
- Coherencia (Close>EMA): 56/80 (70.0%)

## StructureFusion
- Trazas por zona: 59712 | Zonas con Anchors: 59025
- Dir zonas (zona): Bull=35241 Bear=24471 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 9.3, WithAnchors≈ 9.1, DirBull≈ 5.5, DirBear≈ 3.8, DirNeutral≈ 0.0
- Razones de dirección: {'triggers-only': 607, 'tie-bias': 1561, 'anchors+triggers': 57544}
- TF Triggers: {'5': 15084, '15': 22263, '60': 22365}
- TF Anchors: {'240': 58993, '1440': 49417}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 6454 | Distribución: {'Bullish': 5202, 'Bearish': 1252, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 5202/6454

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,43': 1}
- Cancel_BOS (diag): por acción {'BUY': 3, 'SELL': 0} | por bias {'Bullish': 0, 'Bearish': 3, 'Neutral': 0}

## CSV de Trades
- Filas: 51 | Ejecutadas: 20 | Canceladas: 0 | Expiradas: 0
- BUY: 62 | SELL: 9

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7931 | Total candidatos: 243039 | Seleccionados: 7452
- Candidatos por zona (promedio): 30.6
- **Edad (barras)** - Candidatos: med=37, max=151 | Seleccionados: med=43, max=151
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.52
- **TF Candidatos**: {240: 72640, 60: 69063, 15: 44420, 5: 36110, 1440: 20806}
- **TF Seleccionados**: {15: 1570, 60: 2656, 240: 844, 5: 2334, 1440: 48}
- **DistATR** - Candidatos: avg=63.7 | Seleccionados: avg=11.0
- **Razones de selección**: {'Fallback<15': 1948, 'InBand[10,15]': 5504}
- **En banda [10,15] ATR**: 20059/243039 (8.3%)

### Take Profit (TP)
- Zonas analizadas: 8026 | Total candidatos: 106451 | Seleccionados: 8026
- Candidatos por zona (promedio): 13.3
- **Edad (barras)** - Candidatos: med=33, max=151 | Seleccionados: med=43, max=150
- **Score** - Candidatos: avg=0.53 | Seleccionados: avg=0.36
- **Priority Candidatos**: {'P3': 106451}
- **Priority Seleccionados**: {'P4_Fallback': 1343, 'P3': 6683}
- **Type Candidatos**: {'Swing': 106451}
- **Type Seleccionados**: {'Calculated': 1343, 'Swing': 6683}
- **TF Candidatos**: {60: 25012, 15: 24977, 240: 22228, 1440: 18613, 5: 15621}
- **TF Seleccionados**: {-1: 1343, 5: 2120, 240: 720, 15: 2056, 60: 1657, 1440: 130}
- **DistATR** - Candidatos: avg=53.8 | Seleccionados: avg=16.9
- **RR** - Candidatos: avg=3.71 | Seleccionados: avg=1.14
- **Razones de selección**: {'NoStructuralTarget': 1343, 'R:R_y_Distancia_OK': 6343, 'R:R_OK_(Distancia_ignorada)': 325, 'Distancia_OK_(R:R_ignorado)': 15}

### 🎯 Recomendaciones
- ⚠️ SL: 53% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- KeptAligned ratio≈ 0.17.