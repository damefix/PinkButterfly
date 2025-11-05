# Informe Diagnóstico de Logs - 2025-11-04 08:13:02

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_080820.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_080820.csv`

## DFM
- Eventos de evaluación: 1732
- Evaluaciones Bull: 2252 | Bear: 284
- Pasaron umbral (PassedThreshold): 928
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:86, 4:995, 5:857, 6:461, 7:137, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5428
- KeptAligned: 7135/44225 | KeptCounter: 1266/12372
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.212 | AvgProxCounter≈ 0.046
  - AvgDistATRAligned≈ 3.73 | AvgDistATRCounter≈ 0.44
- PreferAligned eventos: 3469 | Filtradas contra-bias: 296

### Proximity (Pre-PreferAligned)
- Eventos: 5428
- Aligned pre: 7135/8401 | Counter pre: 1266/8401
- AvgProxAligned(pre)≈ 0.212 | AvgDistATRAligned(pre)≈ 3.73

### Proximity Drivers
- Eventos: 5428
- Alineadas: n=7135 | BaseProx≈ 0.430 | ZoneATR≈ 16.97 | SizePenalty≈ 0.771 | FinalProx≈ 0.330
- Contra-bias: n=970 | BaseProx≈ 0.439 | ZoneATR≈ 27.05 | SizePenalty≈ 0.647 | FinalProx≈ 0.283

## Risk
- Eventos: 4062
- Accepted=2653 | RejSL=3176 | RejTP=353 | RejRR=1923 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=2550 | SLDistATR≈ 23.88 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=626 | SLDistATR≈ 24.02 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:1411,20-25:586,25+:553
- HistSL Counter 0-10:0,10-15:0,15-20:274,20-25:175,25+:177

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 435 | Unmatched: 2218
- 0-10: Wins=34 Losses=65 WR=34.3% (n=99)
- 10-15: Wins=161 Losses=175 WR=47.9% (n=336)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=195 Losses=240 WR=44.8% (n=435)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2653 | Aligned=2405 (90.7%)
- Core≈ 0.99 | Prox≈ 0.34 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.02 | Confidence≈ 0.00
- SL_TF dist: {'-1': 480, '5': 838, '60': 769, '15': 470, '240': 93, '1440': 3} | SL_Structural≈ 81.9%
- TP_TF dist: {'-1': 861, '5': 475, '15': 624, '60': 440, '240': 151, '1440': 102} | TP_Structural≈ 67.5%

### SLPick por Bandas y TF
- Bandas: lt8=686, 8-10=299, 10-12.5=850, 12.5-15=818, >15=0
- TF: 5m=838, 15m=470, 60m=769, 240m=93, 1440m=3
- RR plan por bandas: 0-10≈ 2.35 (n=985), 10-15≈ 1.82 (n=1668)

## CancelBias (EMA200@60m)
- Eventos: 297
- Distribución Bias: {'Bullish': 252, 'Bearish': 45, 'Neutral': 0}
- Coherencia (Close>EMA): 252/297 (84.8%)

## StructureFusion
- Trazas por zona: 56597 | Zonas con Anchors: 55918
- Dir zonas (zona): Bull=31070 Bear=25527 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 10.4, WithAnchors≈ 10.3, DirBull≈ 5.7, DirBear≈ 4.7, DirNeutral≈ 0.0
- Razones de dirección: {'tie-bias': 1362, 'triggers-only': 606, 'anchors+triggers': 54629}
- TF Triggers: {'15': 18656, '5': 15478, '60': 22463}
- TF Anchors: {'240': 55890, '1440': 46806}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5430 | Distribución: {'Bullish': 3736, 'Bearish': 1694, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3736/5430

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,42': 1, 'score decayó a 0,39': 1, 'estructura no existe': 2}
- Cancel_BOS (diag): por acción {'BUY': 1, 'SELL': 0} | por bias {'Bullish': 0, 'Bearish': 1, 'Neutral': 0}

## CSV de Trades
- Filas: 208 | Ejecutadas: 97 | Canceladas: 0 | Expiradas: 0
- BUY: 270 | SELL: 35

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 8017 | Total candidatos: 232356 | Seleccionados: 7497
- Candidatos por zona (promedio): 29.0
- **Edad (barras)** - Candidatos: med=38, max=151 | Seleccionados: med=39, max=151
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.54
- **TF Candidatos**: {240: 69554, 60: 62634, 15: 44276, 5: 36186, 1440: 19706}
- **TF Seleccionados**: {5: 2614, 15: 1516, 60: 2613, 240: 706, 1440: 48}
- **DistATR** - Candidatos: avg=65.1 | Seleccionados: avg=10.7
- **Razones de selección**: {'Fallback<15': 2213, 'InBand[10,15]': 5284}
- **En banda [10,15] ATR**: 19128/232356 (8.2%)

### Take Profit (TP)
- Zonas analizadas: 8105 | Total candidatos: 99052 | Seleccionados: 8105
- Candidatos por zona (promedio): 12.2
- **Edad (barras)** - Candidatos: med=36, max=151 | Seleccionados: med=42, max=150
- **Score** - Candidatos: avg=0.50 | Seleccionados: avg=0.37
- **Priority Candidatos**: {'P3': 99052}
- **Priority Seleccionados**: {'P3': 6786, 'P4_Fallback': 1319}
- **Type Candidatos**: {'Swing': 99052}
- **Type Seleccionados**: {'Swing': 6786, 'Calculated': 1319}
- **TF Candidatos**: {60: 24482, 15: 24085, 240: 21392, 5: 15387, 1440: 13706}
- **TF Seleccionados**: {15: 1983, -1: 1319, 5: 2223, 60: 1665, 240: 681, 1440: 234}
- **DistATR** - Candidatos: avg=58.6 | Seleccionados: avg=17.2
- **RR** - Candidatos: avg=4.18 | Seleccionados: avg=1.18
- **Razones de selección**: {'R:R_y_Distancia_OK': 6355, 'NoStructuralTarget': 1319, 'R:R_OK_(Distancia_ignorada)': 412, 'Distancia_OK_(R:R_ignorado)': 19}

### 🎯 Recomendaciones
- ⚠️ SL: 51% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- KeptAligned ratio≈ 0.16.