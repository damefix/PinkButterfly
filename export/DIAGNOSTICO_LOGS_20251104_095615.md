# Informe Diagnóstico de Logs - 2025-11-04 10:03:14

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_095615.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_095615.csv`

## DFM
- Eventos de evaluación: 1687
- Evaluaciones Bull: 2313 | Bear: 181
- Pasaron umbral (PassedThreshold): 930
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:102, 4:992, 5:807, 6:449, 7:144, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5799
- KeptAligned: 7284/47328 | KeptCounter: 1265/12327
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.182 | AvgProxCounter≈ 0.042
  - AvgDistATRAligned≈ 3.78 | AvgDistATRCounter≈ 0.41
- PreferAligned eventos: 3548 | Filtradas contra-bias: 288

### Proximity (Pre-PreferAligned)
- Eventos: 5799
- Aligned pre: 7284/8549 | Counter pre: 1265/8549
- AvgProxAligned(pre)≈ 0.182 | AvgDistATRAligned(pre)≈ 3.78

### Proximity Drivers
- Eventos: 5799
- Alineadas: n=7284 | BaseProx≈ 0.416 | ZoneATR≈ 17.40 | SizePenalty≈ 0.759 | FinalProx≈ 0.317
- Contra-bias: n=977 | BaseProx≈ 0.448 | ZoneATR≈ 27.05 | SizePenalty≈ 0.643 | FinalProx≈ 0.285

## Risk
- Eventos: 4145
- Accepted=2614 | RejSL=3499 | RejTP=126 | RejRR=2022 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=2900 | SLDistATR≈ 23.57 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=599 | SLDistATR≈ 23.86 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:1398,20-25:949,25+:553
- HistSL Counter 0-10:0,10-15:0,15-20:265,20-25:167,25+:167

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 425 | Unmatched: 2189
- 0-10: Wins=38 Losses=73 WR=34.2% (n=111)
- 10-15: Wins=131 Losses=183 WR=41.7% (n=314)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=169 Losses=256 WR=39.8% (n=425)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2614 | Aligned=2336 (89.4%)
- Core≈ 0.99 | Prox≈ 0.35 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.92 | Confidence≈ 0.00
- SL_TF dist: {'-1': 493, '5': 753, '60': 806, '15': 459, '240': 100, '1440': 3} | SL_Structural≈ 81.1%
- TP_TF dist: {'-1': 833, '5': 504, '15': 610, '60': 484, '1440': 42, '240': 141} | TP_Structural≈ 68.1%

### SLPick por Bandas y TF
- Bandas: lt8=717, 8-10=303, 10-12.5=776, 12.5-15=818, >15=0
- TF: 5m=753, 15m=459, 60m=806, 240m=100, 1440m=3
- RR plan por bandas: 0-10≈ 2.44 (n=1020), 10-15≈ 1.59 (n=1594)

## CancelBias (EMA200@60m)
- Eventos: 307
- Distribución Bias: {'Bullish': 257, 'Bearish': 50, 'Neutral': 0}
- Coherencia (Close>EMA): 257/307 (83.7%)

## StructureFusion
- Trazas por zona: 59655 | Zonas con Anchors: 59077
- Dir zonas (zona): Bull=31230 Bear=28425 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 10.3, WithAnchors≈ 10.2, DirBull≈ 5.4, DirBear≈ 4.9, DirNeutral≈ 0.0
- Razones de dirección: {'triggers-only': 525, 'anchors+triggers': 57888, 'tie-bias': 1242}
- TF Triggers: {'5': 16591, '15': 19308, '60': 23756}
- TF Anchors: {'240': 58959, '1440': 50796}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5799 | Distribución: {'Bullish': 3736, 'Bearish': 2063, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3736/5799

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,39': 1, 'estructura no existe': 3}

## CSV de Trades
- Filas: 212 | Ejecutadas: 100 | Canceladas: 0 | Expiradas: 0
- BUY: 274 | SELL: 38

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 8194 | Total candidatos: 234761 | Seleccionados: 7252
- Candidatos por zona (promedio): 28.7
- **Edad (barras)** - Candidatos: med=38, max=151 | Seleccionados: med=41, max=151
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.52
- **TF Candidatos**: {240: 70937, 60: 64354, 15: 43161, 5: 35863, 1440: 20446}
- **TF Seleccionados**: {5: 2366, 15: 1545, 60: 2610, 1440: 50, 240: 681}
- **DistATR** - Candidatos: avg=65.4 | Seleccionados: avg=10.9
- **Razones de selección**: {'Fallback<15': 1938, 'InBand[10,15]': 5314}
- **En banda [10,15] ATR**: 19420/234761 (8.3%)

### Take Profit (TP)
- Zonas analizadas: 8261 | Total candidatos: 103255 | Seleccionados: 8261
- Candidatos por zona (promedio): 12.5
- **Edad (barras)** - Candidatos: med=35, max=151 | Seleccionados: med=40, max=150
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.38
- **Priority Candidatos**: {'P3': 103255}
- **Priority Seleccionados**: {'P4_Fallback': 1279, 'P3': 6982}
- **Type Candidatos**: {'Swing': 103255}
- **Type Seleccionados**: {'Calculated': 1279, 'Swing': 6982}
- **TF Candidatos**: {60: 25112, 15: 25066, 240: 21587, 1440: 15996, 5: 15494}
- **TF Seleccionados**: {-1: 1279, 5: 2033, 15: 1956, 60: 1665, 1440: 694, 240: 634}
- **DistATR** - Candidatos: avg=59.1 | Seleccionados: avg=18.8
- **RR** - Candidatos: avg=4.09 | Seleccionados: avg=1.24
- **Razones de selección**: {'NoStructuralTarget': 1279, 'R:R_y_Distancia_OK': 6256, 'R:R_OK_(Distancia_ignorada)': 705, 'Distancia_OK_(R:R_ignorado)': 21}

### 🎯 Recomendaciones
- ⚠️ SL: 52% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- KeptAligned ratio≈ 0.15.