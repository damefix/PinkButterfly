# Informe Diagnóstico de Logs - 2025-11-02 20:16:28

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251102_201221.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251102_201221.csv`

## DFM
- Eventos de evaluación: 1675
- Evaluaciones Bull: 1852 | Bear: 378
- Pasaron umbral (PassedThreshold): 1558
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:231, 4:374, 5:116, 6:513, 7:486, 8:383, 9:127

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 3559/26918 | KeptCounter: 1779/14482
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.144 | AvgProxCounter≈ 0.059
  - AvgDistATRAligned≈ 2.80 | AvgDistATRCounter≈ 0.75
- PreferAligned eventos: 2105 | Filtradas contra-bias: 271

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 3559/5338 | Counter pre: 1779/5338
- AvgProxAligned(pre)≈ 0.144 | AvgDistATRAligned(pre)≈ 2.80

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=3559 | BaseProx≈ 0.447 | ZoneATR≈ 17.23 | SizePenalty≈ 0.768 | FinalProx≈ 0.343
- Contra-bias: n=1508 | BaseProx≈ 0.455 | ZoneATR≈ 32.29 | SizePenalty≈ 0.561 | FinalProx≈ 0.250

## Risk
- Eventos: 3059
- Accepted=2290 | RejSL=1896 | RejTP=60 | RejRR=821 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1294 | SLDistATR≈ 25.49 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=602 | SLDistATR≈ 26.14 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:631,20-25:357,25+:306
- HistSL Counter 0-10:0,10-15:0,15-20:274,20-25:90,25+:238

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 195 | Unmatched: 2095
- 0-10: Wins=13 Losses=39 WR=25.0% (n=52)
- 10-15: Wins=86 Losses=57 WR=60.1% (n=143)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=99 Losses=96 WR=50.8% (n=195)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2290 | Aligned=1539 (67.2%)
- Core≈ 1.00 | Prox≈ 0.34 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.96 | Confidence≈ 0.00
- SL_TF dist: {'-1': 782, '15': 1284, '5': 224} | SL_Structural≈ 65.9%
- TP_TF dist: {'15': 872, '-1': 1227, '5': 191} | TP_Structural≈ 46.4%

### SLPick por Bandas y TF
- Bandas: lt8=727, 8-10=328, 10-12.5=639, 12.5-15=596, >15=0
- TF: 5m=224, 15m=1284, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.56 (n=1055), 10-15≈ 1.45 (n=1235)

## CancelBias (EMA200@60m)
- Eventos: 442
- Distribución Bias: {'Bullish': 361, 'Bearish': 81, 'Neutral': 0}
- Coherencia (Close>EMA): 361/442 (81.7%)

## StructureFusion
- Trazas por zona: 41400 | Zonas con Anchors: 41044
- Dir zonas (zona): Bull=25924 Bear=15476 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.3, WithAnchors≈ 8.2, DirBull≈ 5.2, DirBear≈ 3.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 39798, 'tie-bias': 1282, 'triggers-only': 320}
- TF Triggers: {'60': 20232, '15': 16635, '5': 4533}
- TF Anchors: {'240': 41020, '1440': 35388}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 5, 'score decayó a 0,47': 1, 'score decayó a 0,25': 3, 'score decayó a 0,24': 2}
- Cancel_BOS (diag): por acción {'BUY': 14, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 14, 'Neutral': 0}

## CSV de Trades
- Filas: 166 | Ejecutadas: 55 | Canceladas: 0 | Expiradas: 0
- BUY: 159 | SELL: 62

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4388 | Total candidatos: 41669 | Seleccionados: 2948
- Candidatos por zona (promedio): 9.5
- **Edad (barras)** - Candidatos: med=49, max=190 | Seleccionados: med=49, max=177
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.47
- **TF Candidatos**: {15: 31103}
- **TF Seleccionados**: {15: 2948}
- **DistATR** - Candidatos: avg=20.9 | Seleccionados: avg=10.5
- **Razones de selección**: {'Fallback<15': 1043, 'InBand[10,15]': 1905}
- **En banda [10,15] ATR**: 4957/31103 (15.9%)

### Take Profit (TP)
- Zonas analizadas: 5067 | Total candidatos: 75086 | Seleccionados: 4250
- Candidatos por zona (promedio): 14.8
- **Edad (barras)** - Candidatos: med=18623, max=23176 | Seleccionados: med=5, max=189
- **Score** - Candidatos: avg=0.48 | Seleccionados: avg=0.22
- **Priority Candidatos**: {'P3': 70311}
- **Priority Seleccionados**: {'P3': 2186, 'P4_Fallback': 2064}
- **Type Candidatos**: {'Swing': 70311}
- **Type Seleccionados**: {'Swing': 2186, 'Calculated': 2064}
- **TF Candidatos**: {240: 20220, 1440: 19707, 60: 16478, 15: 13906}
- **TF Seleccionados**: {15: 2186, -1: 2064}
- **DistATR** - Candidatos: avg=117.4 | Seleccionados: avg=17.7
- **RR** - Candidatos: avg=8.91 | Seleccionados: avg=1.39
- **Razones de selección**: {'R:R_y_Distancia_OK': 2050, 'NoStructuralTarget': 2064, 'Distancia_OK_(R:R_ignorado)': 33, 'R:R_OK_(Distancia_ignorada)': 103}

### 🎯 Recomendaciones
- ⚠️ SL: 62% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 49% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.13.