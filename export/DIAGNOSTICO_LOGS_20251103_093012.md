# Informe Diagnóstico de Logs - 2025-11-03 09:35:05

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251103_093012.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251103_093012.csv`

## DFM
- Eventos de evaluación: 1545
- Evaluaciones Bull: 1674 | Bear: 350
- Pasaron umbral (PassedThreshold): 730
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:232, 4:756, 5:548, 6:383, 7:105, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 3110/26851 | KeptCounter: 1601/14449
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.132 | AvgProxCounter≈ 0.054
  - AvgDistATRAligned≈ 2.28 | AvgDistATRCounter≈ 0.63
- PreferAligned eventos: 1924 | Filtradas contra-bias: 213

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 3110/4711 | Counter pre: 1601/4711
- AvgProxAligned(pre)≈ 0.132 | AvgDistATRAligned(pre)≈ 2.28

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=3110 | BaseProx≈ 0.441 | ZoneATR≈ 16.69 | SizePenalty≈ 0.777 | FinalProx≈ 0.342
- Contra-bias: n=1388 | BaseProx≈ 0.450 | ZoneATR≈ 31.32 | SizePenalty≈ 0.570 | FinalProx≈ 0.250

## Risk
- Eventos: 2830
- Accepted=2109 | RejSL=1568 | RejTP=55 | RejRR=766 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1070 | SLDistATR≈ 25.86 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=498 | SLDistATR≈ 25.56 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:520,20-25:287,25+:263
- HistSL Counter 0-10:0,10-15:0,15-20:210,20-25:95,25+:193

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 326 | Unmatched: 1783
- 0-10: Wins=38 Losses=52 WR=42.2% (n=90)
- 10-15: Wins=127 Losses=109 WR=53.8% (n=236)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=165 Losses=161 WR=50.6% (n=326)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2109 | Aligned=1377 (65.3%)
- Core≈ 1.00 | Prox≈ 0.34 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.01 | Confidence≈ 0.00
- SL_TF dist: {'-1': 739, '15': 1173, '5': 197} | SL_Structural≈ 65.0%
- TP_TF dist: {'15': 816, '-1': 1115, '5': 178} | TP_Structural≈ 47.1%

### SLPick por Bandas y TF
- Bandas: lt8=700, 8-10=311, 10-12.5=592, 12.5-15=506, >15=0
- TF: 5m=197, 15m=1173, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.61 (n=1011), 10-15≈ 1.45 (n=1098)

## CancelBias (EMA200@60m)
- Eventos: 274
- Distribución Bias: {'Bullish': 232, 'Bearish': 42, 'Neutral': 0}
- Coherencia (Close>EMA): 232/274 (84.7%)

## StructureFusion
- Trazas por zona: 41300 | Zonas con Anchors: 40940
- Dir zonas (zona): Bull=25868 Bear=15432 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.3, WithAnchors≈ 8.2, DirBull≈ 5.2, DirBear≈ 3.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 39813, 'tie-bias': 1164, 'triggers-only': 323}
- TF Triggers: {'60': 20248, '15': 16528, '5': 4524}
- TF Anchors: {'240': 40916, '1440': 35283}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 2, 'score decayó a 0,22': 1}
- Cancel_BOS (diag): por acción {'BUY': 8, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 8, 'Neutral': 0}

## CSV de Trades
- Filas: 150 | Ejecutadas: 62 | Canceladas: 0 | Expiradas: 0
- BUY: 180 | SELL: 32

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 3864 | Total candidatos: 35591 | Seleccionados: 2576
- Candidatos por zona (promedio): 9.2
- **Edad (barras)** - Candidatos: med=47, max=150 | Seleccionados: med=48, max=145
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.47
- **TF Candidatos**: {15: 26434}
- **TF Seleccionados**: {15: 2576}
- **DistATR** - Candidatos: avg=20.4 | Seleccionados: avg=10.5
- **Razones de selección**: {'Fallback<15': 936, 'InBand[10,15]': 1640}
- **En banda [10,15] ATR**: 4227/26434 (16.0%)

### Take Profit (TP)
- Zonas analizadas: 4498 | Total candidatos: 65359 | Seleccionados: 3803
- Candidatos por zona (promedio): 14.5
- **Edad (barras)** - Candidatos: med=18494, max=23176 | Seleccionados: med=9, max=150
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.23
- **Priority Candidatos**: {'P3': 61535}
- **Priority Seleccionados**: {'P3': 2036, 'P4_Fallback': 1767}
- **Type Candidatos**: {'Swing': 61535}
- **Type Seleccionados**: {'Swing': 2036, 'Calculated': 1767}
- **TF Candidatos**: {1440: 17361, 240: 17334, 60: 14659, 15: 12181}
- **TF Seleccionados**: {15: 2036, -1: 1767}
- **DistATR** - Candidatos: avg=117.5 | Seleccionados: avg=17.2
- **RR** - Candidatos: avg=9.04 | Seleccionados: avg=1.42
- **Razones de selección**: {'R:R_y_Distancia_OK': 1905, 'NoStructuralTarget': 1767, 'Distancia_OK_(R:R_ignorado)': 39, 'R:R_OK_(Distancia_ignorada)': 92}

### 🎯 Recomendaciones
- ⚠️ SL: 61% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 46% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.12.