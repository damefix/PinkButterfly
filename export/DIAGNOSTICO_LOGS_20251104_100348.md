# Informe Diagnóstico de Logs - 2025-11-04 10:10:39

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_100348.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_100348.csv`

## DFM
- Eventos de evaluación: 1683
- Evaluaciones Bull: 2313 | Bear: 176
- Pasaron umbral (PassedThreshold): 922
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:104, 4:1000, 5:806, 6:458, 7:121, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 6175
- KeptAligned: 6896/56027 | KeptCounter: 1231/12377
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.167 | AvgProxCounter≈ 0.035
  - AvgDistATRAligned≈ 2.97 | AvgDistATRCounter≈ 0.38
- PreferAligned eventos: 3149 | Filtradas contra-bias: 264

### Proximity (Pre-PreferAligned)
- Eventos: 6175
- Aligned pre: 6896/8127 | Counter pre: 1231/8127
- AvgProxAligned(pre)≈ 0.167 | AvgDistATRAligned(pre)≈ 2.97

### Proximity Drivers
- Eventos: 6175
- Alineadas: n=6896 | BaseProx≈ 0.432 | ZoneATR≈ 17.18 | SizePenalty≈ 0.765 | FinalProx≈ 0.329
- Contra-bias: n=967 | BaseProx≈ 0.446 | ZoneATR≈ 27.58 | SizePenalty≈ 0.639 | FinalProx≈ 0.278

## Risk
- Eventos: 3731
- Accepted=2597 | RejSL=3087 | RejTP=139 | RejRR=2040 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=2499 | SLDistATR≈ 23.70 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=588 | SLDistATR≈ 23.90 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:1415,20-25:548,25+:536
- HistSL Counter 0-10:0,10-15:0,15-20:267,20-25:161,25+:160

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 328 | Unmatched: 2269
- 0-10: Wins=51 Losses=43 WR=54.3% (n=94)
- 10-15: Wins=86 Losses=148 WR=36.8% (n=234)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=137 Losses=191 WR=41.8% (n=328)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2597 | Aligned=2321 (89.4%)
- Core≈ 0.99 | Prox≈ 0.34 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.95 | Confidence≈ 0.00
- SL_TF dist: {'5': 756, '-1': 488, '15': 444, '60': 808, '240': 97, '1440': 4} | SL_Structural≈ 81.2%
- TP_TF dist: {'-1': 827, '15': 604, '5': 497, '60': 483, '1440': 43, '240': 143} | TP_Structural≈ 68.2%

### SLPick por Bandas y TF
- Bandas: lt8=727, 8-10=288, 10-12.5=772, 12.5-15=810, >15=0
- TF: 5m=756, 15m=444, 60m=808, 240m=97, 1440m=4
- RR plan por bandas: 0-10≈ 2.51 (n=1015), 10-15≈ 1.59 (n=1582)

## CancelBias (EMA200@60m)
- Eventos: 235
- Distribución Bias: {'Bullish': 191, 'Bearish': 44, 'Neutral': 0}
- Coherencia (Close>EMA): 191/235 (81.3%)

## StructureFusion
- Trazas por zona: 68404 | Zonas con Anchors: 67811
- Dir zonas (zona): Bull=31248 Bear=37156 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 11.1, WithAnchors≈ 11.0, DirBull≈ 5.1, DirBear≈ 6.0, DirNeutral≈ 0.0
- Razones de dirección: {'triggers-only': 534, 'anchors+triggers': 66619, 'tie-bias': 1251}
- TF Triggers: {'5': 18018, '15': 24743, '60': 25643}
- TF Anchors: {'240': 67695, '1440': 59561}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 6176 | Distribución: {'Bullish': 3736, 'Bearish': 2440, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3736/6176

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 3, 'score decayó a 0,39': 1}
- Cancel_BOS (diag): por acción {'BUY': 2, 'SELL': 0} | por bias {'Bullish': 0, 'Bearish': 2, 'Neutral': 0}

## CSV de Trades
- Filas: 173 | Ejecutadas: 75 | Canceladas: 0 | Expiradas: 0
- BUY: 227 | SELL: 21

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7803 | Total candidatos: 228233 | Seleccionados: 7257
- Candidatos por zona (promedio): 29.2
- **Edad (barras)** - Candidatos: med=38, max=150 | Seleccionados: med=40, max=148
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.52
- **TF Candidatos**: {240: 70015, 60: 59896, 15: 43130, 5: 35198, 1440: 19994}
- **TF Seleccionados**: {5: 2331, 60: 2669, 15: 1524, 1440: 50, 240: 683}
- **DistATR** - Candidatos: avg=64.7 | Seleccionados: avg=10.9
- **Razones de selección**: {'Fallback<15': 1944, 'InBand[10,15]': 5313}
- **En banda [10,15] ATR**: 19466/228233 (8.5%)

### Take Profit (TP)
- Zonas analizadas: 7863 | Total candidatos: 99870 | Seleccionados: 7863
- Candidatos por zona (promedio): 12.7
- **Edad (barras)** - Candidatos: med=36, max=150 | Seleccionados: med=43, max=150
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.37
- **Priority Candidatos**: {'P3': 99870}
- **Priority Seleccionados**: {'P3': 6599, 'P4_Fallback': 1264}
- **Type Candidatos**: {'Swing': 99870}
- **Type Seleccionados**: {'Swing': 6599, 'Calculated': 1264}
- **TF Candidatos**: {60: 25304, 15: 24958, 240: 21566, 5: 15708, 1440: 12334}
- **TF Seleccionados**: {15: 1973, -1: 1264, 5: 2041, 60: 1663, 1440: 296, 240: 626}
- **DistATR** - Candidatos: avg=53.2 | Seleccionados: avg=16.8
- **RR** - Candidatos: avg=3.90 | Seleccionados: avg=1.17
- **Razones de selección**: {'R:R_y_Distancia_OK': 6266, 'NoStructuralTarget': 1264, 'R:R_OK_(Distancia_ignorada)': 312, 'Distancia_OK_(R:R_ignorado)': 21}

### 🎯 Recomendaciones
- ⚠️ SL: 51% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- KeptAligned ratio≈ 0.12.