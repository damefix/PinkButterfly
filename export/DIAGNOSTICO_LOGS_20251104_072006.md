# Informe Diagnóstico de Logs - 2025-11-04 07:24:02

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_072006.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_072006.csv`

## DFM
- Eventos de evaluación: 1832
- Evaluaciones Bull: 1485 | Bear: 693
- Pasaron umbral (PassedThreshold): 525
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:70, 4:1283, 5:491, 6:268, 7:66, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5784
- KeptAligned: 4542/37490 | KeptCounter: 972/10758
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.133 | AvgProxCounter≈ 0.030
  - AvgDistATRAligned≈ 3.24 | AvgDistATRCounter≈ 0.34
- PreferAligned eventos: 2930 | Filtradas contra-bias: 163

### Proximity (Pre-PreferAligned)
- Eventos: 5784
- Aligned pre: 4542/5514 | Counter pre: 972/5514
- AvgProxAligned(pre)≈ 0.133 | AvgDistATRAligned(pre)≈ 3.24

### Proximity Drivers
- Eventos: 5784
- Alineadas: n=4542 | BaseProx≈ 0.395 | ZoneATR≈ 20.61 | SizePenalty≈ 0.715 | FinalProx≈ 0.275
- Contra-bias: n=809 | BaseProx≈ 0.433 | ZoneATR≈ 29.97 | SizePenalty≈ 0.612 | FinalProx≈ 0.258

## Risk
- Eventos: 3453
- Accepted=2248 | RejSL=2114 | RejTP=57 | RejRR=932 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1547 | SLDistATR≈ 25.87 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=567 | SLDistATR≈ 24.15 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:723,20-25:370,25+:454
- HistSL Counter 0-10:0,10-15:0,15-20:242,20-25:164,25+:161

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 329 | Unmatched: 1919
- 0-10: Wins=23 Losses=106 WR=17.8% (n=129)
- 10-15: Wins=136 Losses=64 WR=68.0% (n=200)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=159 Losses=170 WR=48.3% (n=329)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2248 | Aligned=2074 (92.3%)
- Core≈ 1.00 | Prox≈ 0.26 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.81 | Confidence≈ 0.00
- SL_TF dist: {'-1': 841, '15': 587, '60': 621, '240': 74, '5': 121, '1440': 4} | SL_Structural≈ 62.6%
- TP_TF dist: {'-1': 589, '15': 536, '240': 120, '60': 383, '5': 62, '1440': 558} | TP_Structural≈ 73.8%

### SLPick por Bandas y TF
- Bandas: lt8=423, 8-10=161, 10-12.5=523, 12.5-15=1141, >15=0
- TF: 5m=121, 15m=587, 60m=621, 240m=74, 1440m=4
- RR plan por bandas: 0-10≈ 2.72 (n=584), 10-15≈ 2.84 (n=1664)

## CancelBias (EMA200@60m)
- Eventos: 227
- Distribución Bias: {'Bullish': 186, 'Bearish': 41, 'Neutral': 0}
- Coherencia (Close>EMA): 186/227 (81.9%)

## StructureFusion
- Trazas por zona: 48248 | Zonas con Anchors: 47804
- Dir zonas (zona): Bull=24315 Bear=23933 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.3, WithAnchors≈ 8.3, DirBull≈ 4.2, DirBear≈ 4.1, DirNeutral≈ 0.0
- Razones de dirección: {'tie-bias': 1029, 'triggers-only': 396, 'anchors+triggers': 46823}
- TF Triggers: {'15': 18739, '60': 23636, '5': 5873}
- TF Anchors: {'240': 47778, '1440': 40839}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5789 | Distribución: {'Bullish': 3736, 'Bearish': 2053, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3736/5789

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 1}

## CSV de Trades
- Filas: 109 | Ejecutadas: 53 | Canceladas: 0 | Expiradas: 0
- BUY: 146 | SELL: 16

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5296 | Total candidatos: 138937 | Seleccionados: 4410
- Candidatos por zona (promedio): 26.2
- **Edad (barras)** - Candidatos: med=37, max=150 | Seleccionados: med=39, max=137
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.56
- **TF Candidatos**: {240: 43948, 60: 41766, 15: 30104, 1440: 13763, 5: 9356}
- **TF Seleccionados**: {15: 1445, 60: 1703, 240: 557, 5: 657, 1440: 48}
- **DistATR** - Candidatos: avg=71.9 | Seleccionados: avg=11.0
- **Razones de selección**: {'Fallback<15': 1236, 'InBand[10,15]': 3174}
- **En banda [10,15] ATR**: 9755/138937 (7.0%)

### Take Profit (TP)
- Zonas analizadas: 5351 | Total candidatos: 62655 | Seleccionados: 5351
- Candidatos por zona (promedio): 11.7
- **Edad (barras)** - Candidatos: med=34, max=150 | Seleccionados: med=33, max=147
- **Score** - Candidatos: avg=0.50 | Seleccionados: avg=0.41
- **Priority Candidatos**: {'P3': 62655}
- **Priority Seleccionados**: {'P4_Fallback': 851, 'P3': 4500}
- **Type Candidatos**: {'Swing': 62655}
- **Type Seleccionados**: {'Calculated': 851, 'Swing': 4500}
- **TF Candidatos**: {60: 15616, 240: 15355, 1440: 14112, 15: 13575, 5: 3997}
- **TF Seleccionados**: {-1: 851, 15: 1597, 60: 1214, 240: 497, 5: 502, 1440: 690}
- **DistATR** - Candidatos: avg=83.7 | Seleccionados: avg=26.1
- **RR** - Candidatos: avg=5.57 | Seleccionados: avg=1.73
- **Razones de selección**: {'NoStructuralTarget': 851, 'R:R_y_Distancia_OK': 3616, 'R:R_OK_(Distancia_ignorada)': 866, 'Distancia_OK_(R:R_ignorado)': 18}

### 🎯 Recomendaciones
- ⚠️ SL: 47% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- KeptAligned ratio≈ 0.12.