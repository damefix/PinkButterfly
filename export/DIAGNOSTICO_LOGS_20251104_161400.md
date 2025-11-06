# Informe Diagnóstico de Logs - 2025-11-04 17:03:00

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_161400.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_161400.csv`

## DFM
- Eventos de evaluación: 2103
- Evaluaciones Bull: 1455 | Bear: 2783
- Pasaron umbral (PassedThreshold): 1184
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:17, 4:1841, 5:1720, 6:439, 7:218, 8:3, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5934
- KeptAligned: 11164/29237 | KeptCounter: 1300/7511
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.269 | AvgProxCounter≈ 0.060
  - AvgDistATRAligned≈ 3.75 | AvgDistATRCounter≈ 0.38
- PreferAligned eventos: 4401 | Filtradas contra-bias: 613

### Proximity (Pre-PreferAligned)
- Eventos: 5934
- Aligned pre: 11164/12464 | Counter pre: 1300/12464
- AvgProxAligned(pre)≈ 0.269 | AvgDistATRAligned(pre)≈ 3.75

### Proximity Drivers
- Eventos: 5934
- Alineadas: n=11164 | BaseProx≈ 0.496 | ZoneATR≈ 19.39 | SizePenalty≈ 0.709 | FinalProx≈ 0.343
- Contra-bias: n=687 | BaseProx≈ 0.517 | ZoneATR≈ 15.61 | SizePenalty≈ 0.778 | FinalProx≈ 0.405

## Risk
- Eventos: 4879
- Accepted=4252 | RejSL=5159 | RejTP=188 | RejRR=2026 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=4688 | SLDistATR≈ 25.26 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=471 | SLDistATR≈ 25.13 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:1509,20-25:1880,25+:1299
- HistSL Counter 0-10:0,10-15:0,15-20:186,20-25:126,25+:159

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 216 | Unmatched: 4036
- 0-10: Wins=31 Losses=82 WR=27.4% (n=113)
- 10-15: Wins=29 Losses=74 WR=28.2% (n=103)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=60 Losses=156 WR=27.8% (n=216)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 4252 | Aligned=4200 (98.8%)
- Core≈ 1.00 | Prox≈ 0.32 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.61 | Confidence≈ 0.00
- SL_TF dist: {'-1': 2904, '5': 324, '60': 695, '15': 289, '240': 40} | SL_Structural≈ 31.7%
- TP_TF dist: {'5': 3148, '15': 429, '60': 191, '-1': 369, '240': 115} | TP_Structural≈ 91.3%

### SLPick por Bandas y TF
- Bandas: lt8=1460, 8-10=190, 10-12.5=361, 12.5-15=2241, >15=0
- TF: 5m=324, 15m=289, 60m=695, 240m=40, 1440m=0
- RR plan por bandas: 0-10≈ 3.67 (n=1650), 10-15≈ 1.94 (n=2602)

## CancelBias (EMA200@60m)
- Eventos: 1151
- Distribución Bias: {'Bullish': 219, 'Bearish': 932, 'Neutral': 0}
- Coherencia (Close>EMA): 219/1151 (19.0%)

## StructureFusion
- Trazas por zona: 36748 | Zonas con Anchors: 36617
- Dir zonas (zona): Bull=16562 Bear=20186 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 6.2, WithAnchors≈ 6.2, DirBull≈ 2.8, DirBear≈ 3.4, DirNeutral≈ 0.0
- Razones de dirección: {'triggers-only': 115, 'tie-bias': 1538, 'anchors+triggers': 35095}
- TF Triggers: {'15': 21449, '5': 15299}
- TF Anchors: {'60': 36447, '240': 35908}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5937 | Distribución: {'Bullish': 3736, 'Bearish': 2201, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3736/5937

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,17': 1, 'estructura no existe': 18, 'score decayó a 0,20': 1, 'score decayó a 0,19': 2, 'score decayó a 0,30': 1, 'score decayó a 0,49': 1, 'score decayó a 0,42': 1}

## CSV de Trades
- Filas: 211 | Ejecutadas: 65 | Canceladas: 0 | Expiradas: 0
- BUY: 204 | SELL: 72

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 11781 | Total candidatos: 253199 | Seleccionados: 7798
- Candidatos por zona (promedio): 21.5
- **Edad (barras)** - Candidatos: med=41, max=151 | Seleccionados: med=37, max=151
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.50
- **TF Candidatos**: {60: 102646, 240: 75983, 15: 46228, 5: 28342}
- **TF Seleccionados**: {5: 1610, 60: 3861, 15: 1834, 240: 493}
- **DistATR** - Candidatos: avg=52.7 | Seleccionados: avg=10.9
- **Razones de selección**: {'Fallback<15': 2258, 'InBand[10,15]': 5540}
- **En banda [10,15] ATR**: 18886/253199 (7.5%)

### Take Profit (TP)
- Zonas analizadas: 11851 | Total candidatos: 889109 | Seleccionados: 11851
- Candidatos por zona (promedio): 75.0
- **Edad (barras)** - Candidatos: med=2, max=151 | Seleccionados: med=61, max=150
- **Score** - Candidatos: avg=0.78 | Seleccionados: avg=0.31
- **Priority Candidatos**: {'P3': 889109}
- **Priority Seleccionados**: {'P3': 10572, 'P4_Fallback': 1279}
- **Type Candidatos**: {'Swing': 889109}
- **Type Seleccionados**: {'Swing': 10572, 'Calculated': 1279}
- **TF Candidatos**: {240: 530659, 15: 283409, 5: 49222, 60: 25819}
- **TF Seleccionados**: {5: 6115, -1: 1279, 15: 2013, 60: 1805, 240: 639}
- **DistATR** - Candidatos: avg=42.9 | Seleccionados: avg=18.4
- **RR** - Candidatos: avg=4.72 | Seleccionados: avg=1.41
- **Razones de selección**: {'R:R_y_Distancia_OK': 10437, 'NoStructuralTarget': 1279, 'R:R_OK_(Distancia_ignorada)': 132, 'Distancia_OK_(R:R_ignorado)': 3}

### 🎯 Recomendaciones
- ⚠️ SL: 51% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- KeptAligned ratio≈ 0.38.