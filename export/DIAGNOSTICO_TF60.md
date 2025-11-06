# Informe Diagnóstico de Logs - 2025-11-03 15:56:30

- Log: `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251103_154256.log`
- CSV: `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251103_154256.csv`

## DFM
- Eventos de evaluación: 1502
- Evaluaciones Bull: 1602 | Bear: 347
- Pasaron umbral (PassedThreshold): 627
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:295, 4:756, 5:486, 6:325, 7:87, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 2923/26873 | KeptCounter: 1501/14396
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.125 | AvgProxCounter≈ 0.050
  - AvgDistATRAligned≈ 2.08 | AvgDistATRCounter≈ 0.58
- PreferAligned eventos: 1828 | Filtradas contra-bias: 164

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 2923/4424 | Counter pre: 1501/4424
- AvgProxAligned(pre)≈ 0.125 | AvgDistATRAligned(pre)≈ 2.08

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=2923 | BaseProx≈ 0.439 | ZoneATR≈ 17.04 | SizePenalty≈ 0.773 | FinalProx≈ 0.339
- Contra-bias: n=1337 | BaseProx≈ 0.450 | ZoneATR≈ 31.45 | SizePenalty≈ 0.573 | FinalProx≈ 0.249

## Risk
- Eventos: 2703
- Accepted=2042 | RejSL=1465 | RejTP=57 | RejRR=696 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=985 | SLDistATR≈ 26.19 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=480 | SLDistATR≈ 25.85 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:481,20-25:240,25+:264
- HistSL Counter 0-10:0,10-15:0,15-20:197,20-25:99,25+:184

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 259 | Unmatched: 1783
- 0-10: Wins=42 Losses=55 WR=43.3% (n=97)
- 10-15: Wins=99 Losses=63 WR=61.1% (n=162)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=141 Losses=118 WR=54.4% (n=259)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2042 | Aligned=1343 (65.8%)
- Core≈ 1.00 | Prox≈ 0.34 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.02 | Confidence≈ 0.00
- SL_TF dist: {'-1': 699, '15': 1142, '5': 201} | SL_Structural≈ 65.8%
- TP_TF dist: {'15': 818, '-1': 1074, '5': 150} | TP_Structural≈ 47.4%

### SLPick por Bandas y TF
- Bandas: lt8=678, 8-10=283, 10-12.5=570, 12.5-15=511, >15=0
- TF: 5m=201, 15m=1142, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.70 (n=961), 10-15≈ 1.42 (n=1081)

## CancelBias (EMA200@60m)
- Eventos: 145
- Distribución Bias: {'Bullish': 130, 'Bearish': 15, 'Neutral': 0}
- Coherencia (Close>EMA): 130/145 (89.7%)

## StructureFusion
- Trazas por zona: 41269 | Zonas con Anchors: 40902
- Dir zonas (zona): Bull=25699 Bear=15570 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.3, WithAnchors≈ 8.2, DirBull≈ 5.1, DirBear≈ 3.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 39787, 'tie-bias': 1150, 'triggers-only': 332}
- TF Triggers: {'60': 20243, '15': 16487, '5': 4539}
- TF Anchors: {'240': 40873, '1440': 35267}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 2, 'score decayó a 0,26': 1, 'score decayó a 0,44': 1}
- Cancel_BOS (diag): por acción {'BUY': 2, 'SELL': 0} | por bias {'Bullish': 0, 'Bearish': 2, 'Neutral': 0}

## CSV de Trades
- Filas: 109 | Ejecutadas: 48 | Canceladas: 0 | Expiradas: 0
- BUY: 138 | SELL: 19

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 3659 | Total candidatos: 33525 | Seleccionados: 2435
- Candidatos por zona (promedio): 9.2
- **Edad (barras)** - Candidatos: med=46, max=150 | Seleccionados: med=47, max=141
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.47
- **TF Candidatos**: {15: 24802}
- **TF Seleccionados**: {15: 2435}
- **DistATR** - Candidatos: avg=20.2 | Seleccionados: avg=10.5
- **Razones de selección**: {'Fallback<15': 865, 'InBand[10,15]': 1570}
- **En banda [10,15] ATR**: 4032/24802 (16.3%)

### Take Profit (TP)
- Zonas analizadas: 4260 | Total candidatos: 62011 | Seleccionados: 3611
- Candidatos por zona (promedio): 14.6
- **Edad (barras)** - Candidatos: med=18608, max=23176 | Seleccionados: med=8, max=150
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.23
- **Priority Candidatos**: {'P3': 58319}
- **Priority Seleccionados**: {'P3': 1930, 'P4_Fallback': 1681}
- **Type Candidatos**: {'Swing': 58319}
- **Type Seleccionados**: {'Swing': 1930, 'Calculated': 1681}
- **TF Candidatos**: {1440: 16815, 240: 16388, 60: 13761, 15: 11355}
- **TF Seleccionados**: {15: 1930, -1: 1681}
- **DistATR** - Candidatos: avg=120.0 | Seleccionados: avg=17.5
- **RR** - Candidatos: avg=9.35 | Seleccionados: avg=1.45
- **Razones de selección**: {'R:R_y_Distancia_OK': 1805, 'NoStructuralTarget': 1681, 'Distancia_OK_(R:R_ignorado)': 41, 'R:R_OK_(Distancia_ignorada)': 84}

### 🎯 Recomendaciones
- ⚠️ SL: 60% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 47% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.11.