# Informe Diagnóstico de Logs - 2025-11-03 07:34:09

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251103_073117.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251103_073117.csv`

## DFM
- Eventos de evaluación: 1665
- Evaluaciones Bull: 1814 | Bear: 376
- Pasaron umbral (PassedThreshold): 1523
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:230, 4:369, 5:119, 6:506, 7:473, 8:372, 9:121

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 3557/26915 | KeptCounter: 1772/14502
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.143 | AvgProxCounter≈ 0.059
  - AvgDistATRAligned≈ 2.79 | AvgDistATRCounter≈ 0.75
- PreferAligned eventos: 2096 | Filtradas contra-bias: 266

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 3557/5329 | Counter pre: 1772/5329
- AvgProxAligned(pre)≈ 0.143 | AvgDistATRAligned(pre)≈ 2.79

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=3557 | BaseProx≈ 0.447 | ZoneATR≈ 17.20 | SizePenalty≈ 0.768 | FinalProx≈ 0.344
- Contra-bias: n=1506 | BaseProx≈ 0.456 | ZoneATR≈ 32.19 | SizePenalty≈ 0.563 | FinalProx≈ 0.252

## Risk
- Eventos: 3045
- Accepted=2286 | RejSL=1892 | RejTP=58 | RejRR=827 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1297 | SLDistATR≈ 25.46 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=595 | SLDistATR≈ 26.06 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:633,20-25:358,25+:306
- HistSL Counter 0-10:0,10-15:0,15-20:275,20-25:87,25+:233

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 187 | Unmatched: 2099
- 0-10: Wins=16 Losses=39 WR=29.1% (n=55)
- 10-15: Wins=85 Losses=47 WR=64.4% (n=132)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=101 Losses=86 WR=54.0% (n=187)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2286 | Aligned=1532 (67.0%)
- Core≈ 1.00 | Prox≈ 0.34 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.96 | Confidence≈ 0.00
- SL_TF dist: {'-1': 786, '15': 1281, '5': 219} | SL_Structural≈ 65.6%
- TP_TF dist: {'15': 876, '-1': 1215, '5': 195} | TP_Structural≈ 46.9%

### SLPick por Bandas y TF
- Bandas: lt8=736, 8-10=330, 10-12.5=631, 12.5-15=589, >15=0
- TF: 5m=219, 15m=1281, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.55 (n=1066), 10-15≈ 1.45 (n=1220)

## CancelBias (EMA200@60m)
- Eventos: 438
- Distribución Bias: {'Bullish': 355, 'Bearish': 83, 'Neutral': 0}
- Coherencia (Close>EMA): 355/438 (81.1%)

## StructureFusion
- Trazas por zona: 41417 | Zonas con Anchors: 41061
- Dir zonas (zona): Bull=25955 Bear=15462 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.3, WithAnchors≈ 8.2, DirBull≈ 5.2, DirBear≈ 3.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 39830, 'tie-bias': 1267, 'triggers-only': 320}
- TF Triggers: {'60': 20248, '15': 16637, '5': 4532}
- TF Anchors: {'240': 41037, '1440': 35387}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 5, 'score decayó a 0,47': 1, 'score decayó a 0,25': 3, 'score decayó a 0,24': 2}
- Cancel_BOS (diag): por acción {'BUY': 16, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 16, 'Neutral': 0}

## CSV de Trades
- Filas: 164 | Ejecutadas: 52 | Canceladas: 0 | Expiradas: 0
- BUY: 154 | SELL: 62

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4367 | Total candidatos: 41142 | Seleccionados: 2937
- Candidatos por zona (promedio): 9.4
- **Edad (barras)** - Candidatos: med=49, max=150 | Seleccionados: med=48, max=146
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.47
- **TF Candidatos**: {15: 30717}
- **TF Seleccionados**: {15: 2937}
- **DistATR** - Candidatos: avg=20.9 | Seleccionados: avg=10.5
- **Razones de selección**: {'Fallback<15': 1048, 'InBand[10,15]': 1889}
- **En banda [10,15] ATR**: 4891/30717 (15.9%)

### Take Profit (TP)
- Zonas analizadas: 5063 | Total candidatos: 74936 | Seleccionados: 4241
- Candidatos por zona (promedio): 14.8
- **Edad (barras)** - Candidatos: med=18607, max=23176 | Seleccionados: med=5, max=144
- **Score** - Candidatos: avg=0.48 | Seleccionados: avg=0.22
- **Priority Candidatos**: {'P3': 70164}
- **Priority Seleccionados**: {'P3': 2186, 'P4_Fallback': 2055}
- **Type Candidatos**: {'Swing': 70164}
- **Type Seleccionados**: {'Swing': 2186, 'Calculated': 2055}
- **TF Candidatos**: {240: 20156, 1440: 19631, 60: 16439, 15: 13938}
- **TF Seleccionados**: {15: 2186, -1: 2055}
- **DistATR** - Candidatos: avg=117.4 | Seleccionados: avg=17.7
- **RR** - Candidatos: avg=8.93 | Seleccionados: avg=1.39
- **Razones de selección**: {'R:R_y_Distancia_OK': 2051, 'NoStructuralTarget': 2055, 'Distancia_OK_(R:R_ignorado)': 33, 'R:R_OK_(Distancia_ignorada)': 102}

### 🎯 Recomendaciones
- ⚠️ SL: 62% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 48% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.13.