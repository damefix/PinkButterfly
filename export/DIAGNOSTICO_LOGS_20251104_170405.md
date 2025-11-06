# Informe Diagnóstico de Logs - 2025-11-04 17:09:06

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_170405.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_170405.csv`

## DFM
- Eventos de evaluación: 1150
- Evaluaciones Bull: 1427 | Bear: 107
- Pasaron umbral (PassedThreshold): 863
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:15, 4:414, 5:504, 6:420, 7:177, 8:4, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5386
- KeptAligned: 7874/24579 | KeptCounter: 1314/7604
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.254 | AvgProxCounter≈ 0.065
  - AvgDistATRAligned≈ 3.65 | AvgDistATRCounter≈ 0.43
- PreferAligned eventos: 3756 | Filtradas contra-bias: 623

### Proximity (Pre-PreferAligned)
- Eventos: 5386
- Aligned pre: 7874/9188 | Counter pre: 1314/9188
- AvgProxAligned(pre)≈ 0.254 | AvgDistATRAligned(pre)≈ 3.65

### Proximity Drivers
- Eventos: 5386
- Alineadas: n=7874 | BaseProx≈ 0.502 | ZoneATR≈ 18.01 | SizePenalty≈ 0.730 | FinalProx≈ 0.377
- Contra-bias: n=691 | BaseProx≈ 0.503 | ZoneATR≈ 15.38 | SizePenalty≈ 0.783 | FinalProx≈ 0.393

## Risk
- Eventos: 4234
- Accepted=1549 | RejSL=4543 | RejTP=185 | RejRR=2063 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=4072 | SLDistATR≈ 25.12 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=471 | SLDistATR≈ 25.35 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:1824,20-25:940,25+:1308
- HistSL Counter 0-10:0,10-15:0,15-20:187,20-25:121,25+:163

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 157 | Unmatched: 1392
- 0-10: Wins=33 Losses=35 WR=48.5% (n=68)
- 10-15: Wins=18 Losses=71 WR=20.2% (n=89)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=51 Losses=106 WR=32.5% (n=157)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1549 | Aligned=1496 (96.6%)
- Core≈ 1.00 | Prox≈ 0.44 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.92 | Confidence≈ 0.00
- SL_TF dist: {'-1': 224, '60': 696, '5': 328, '15': 268, '240': 33} | SL_Structural≈ 85.5%
- TP_TF dist: {'5': 460, '15': 422, '60': 199, '-1': 362, '240': 106} | TP_Structural≈ 76.6%

### SLPick por Bandas y TF
- Bandas: lt8=531, 8-10=180, 10-12.5=376, 12.5-15=462, >15=0
- TF: 5m=328, 15m=268, 60m=696, 240m=33, 1440m=0
- RR plan por bandas: 0-10≈ 2.32 (n=711), 10-15≈ 1.58 (n=838)

## CancelBias (EMA200@60m)
- Eventos: 198
- Distribución Bias: {'Bullish': 188, 'Bearish': 10, 'Neutral': 0}
- Coherencia (Close>EMA): 188/198 (94.9%)

## StructureFusion
- Trazas por zona: 32183 | Zonas con Anchors: 32092
- Dir zonas (zona): Bull=16697 Bear=15486 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 6.0, WithAnchors≈ 6.0, DirBull≈ 3.1, DirBear≈ 2.9, DirNeutral≈ 0.0
- Razones de dirección: {'tie-bias': 1479, 'triggers-only': 78, 'anchors+triggers': 30626}
- TF Triggers: {'15': 17943, '5': 14240}
- TF Anchors: {'60': 31955, '240': 31362}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5387 | Distribución: {'Bullish': 3736, 'Bearish': 1651, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3736/5387

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,17': 1, 'estructura no existe': 21, 'score decayó a 0,18': 2, 'score decayó a 0,30': 1, 'score decayó a 0,19': 1, 'score decayó a 0,49': 1, 'score decayó a 0,42': 1}

## CSV de Trades
- Filas: 166 | Ejecutadas: 42 | Canceladas: 0 | Expiradas: 0
- BUY: 196 | SELL: 12

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 8493 | Total candidatos: 257090 | Seleccionados: 8135
- Candidatos por zona (promedio): 30.3
- **Edad (barras)** - Candidatos: med=34, max=150 | Seleccionados: med=38, max=148
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.49
- **TF Candidatos**: {15: 90000, 240: 71427, 60: 66911, 5: 28752}
- **TF Seleccionados**: {5: 1893, 60: 3934, 15: 1817, 240: 491}
- **DistATR** - Candidatos: avg=45.9 | Seleccionados: avg=10.5
- **Razones de selección**: {'Fallback<15': 2603, 'InBand[10,15]': 5532}
- **En banda [10,15] ATR**: 18709/257090 (7.3%)

### Take Profit (TP)
- Zonas analizadas: 8565 | Total candidatos: 96174 | Seleccionados: 8565
- Candidatos por zona (promedio): 11.2
- **Edad (barras)** - Candidatos: med=37, max=150 | Seleccionados: med=43, max=150
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.34
- **Priority Candidatos**: {'P3': 96174}
- **Priority Seleccionados**: {'P3': 7001, 'P4_Fallback': 1564}
- **Type Candidatos**: {'Swing': 96174}
- **Type Seleccionados**: {'Swing': 7001, 'Calculated': 1564}
- **TF Candidatos**: {15: 25325, 5: 24456, 240: 23921, 60: 22472}
- **TF Seleccionados**: {5: 2547, 15: 2013, -1: 1564, 60: 1821, 240: 620}
- **DistATR** - Candidatos: avg=32.4 | Seleccionados: avg=13.8
- **RR** - Candidatos: avg=2.27 | Seleccionados: avg=0.88
- **Razones de selección**: {'R:R_y_Distancia_OK': 6870, 'NoStructuralTarget': 1564, 'R:R_OK_(Distancia_ignorada)': 129, 'Distancia_OK_(R:R_ignorado)': 2}

### 🎯 Recomendaciones
- ⚠️ SL: 53% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- KeptAligned ratio≈ 0.32.