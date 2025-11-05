# Informe Diagnóstico de Logs - 2025-11-04 12:27:18

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_120545.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_120545.csv`

## DFM
- Eventos de evaluación: 3854
- Evaluaciones Bull: 4557 | Bear: 828
- Pasaron umbral (PassedThreshold): 1576
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:466, 4:2470, 5:1477, 6:756, 7:215, 8:1, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 14876
- KeptAligned: 13334/97550 | KeptCounter: 4943/54260
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.142 | AvgProxCounter≈ 0.064
  - AvgDistATRAligned≈ 2.60 | AvgDistATRCounter≈ 0.61
- PreferAligned eventos: 6751 | Filtradas contra-bias: 966

### Proximity (Pre-PreferAligned)
- Eventos: 14876
- Aligned pre: 13334/18277 | Counter pre: 4943/18277
- AvgProxAligned(pre)≈ 0.142 | AvgDistATRAligned(pre)≈ 2.60

### Proximity Drivers
- Eventos: 14876
- Alineadas: n=13334 | BaseProx≈ 0.441 | ZoneATR≈ 19.46 | SizePenalty≈ 0.726 | FinalProx≈ 0.319
- Contra-bias: n=3977 | BaseProx≈ 0.463 | ZoneATR≈ 32.45 | SizePenalty≈ 0.605 | FinalProx≈ 0.277

## Risk
- Eventos: 9331
- Accepted=5498 | RejSL=7440 | RejTP=374 | RejRR=3999 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=5225 | SLDistATR≈ 24.56 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=2215 | SLDistATR≈ 27.20 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:2696,20-25:1033,25+:1496
- HistSL Counter 0-10:0,10-15:0,15-20:938,20-25:501,25+:776

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 653 | Unmatched: 4845
- 0-10: Wins=40 Losses=83 WR=32.5% (n=123)
- 10-15: Wins=245 Losses=285 WR=46.2% (n=530)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=285 Losses=368 WR=43.6% (n=653)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 5498 | Aligned=4278 (77.8%)
- Core≈ 1.00 | Prox≈ 0.31 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.23 | Confidence≈ 0.00
- SL_TF dist: {'60': 1759, '-1': 1041, '5': 1509, '15': 870, '240': 314, '1440': 5} | SL_Structural≈ 81.1%
- TP_TF dist: {'-1': 1490, '60': 956, '5': 1095, '15': 1354, '240': 489, '1440': 114} | TP_Structural≈ 72.9%

### SLPick por Bandas y TF
- Bandas: lt8=1401, 8-10=717, 10-12.5=1576, 12.5-15=1804, >15=0
- TF: 5m=1509, 15m=870, 60m=1759, 240m=314, 1440m=5
- RR plan por bandas: 0-10≈ 2.99 (n=2118), 10-15≈ 1.76 (n=3380)

## CancelBias (EMA200@60m)
- Eventos: 577
- Distribución Bias: {'Bullish': 453, 'Bearish': 124, 'Neutral': 0}
- Coherencia (Close>EMA): 453/577 (78.5%)

## StructureFusion
- Trazas por zona: 151810 | Zonas con Anchors: 149978
- Dir zonas (zona): Bull=77164 Bear=74646 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 10.2, WithAnchors≈ 10.1, DirBull≈ 5.2, DirBear≈ 5.0, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 143278, 'triggers-only': 1667, 'tie-bias': 6865}
- TF Triggers: {'5': 42706, '60': 57360, '15': 51744}
- TF Anchors: {'240': 148132, '1440': 134180}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 14876 | Distribución: {'Bullish': 9653, 'Bearish': 5223, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 9653/14876

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 10, 'score decayó a 0,21': 1, 'score decayó a 0,36': 1, 'score decayó a 0,43': 1, 'score decayó a 0,20': 1, 'score decayó a 0,46': 1}
- Cancel_BOS (diag): por acción {'BUY': 2, 'SELL': 3} | por bias {'Bullish': 3, 'Bearish': 2, 'Neutral': 0}

## CSV de Trades
- Filas: 412 | Ejecutadas: 182 | Canceladas: 0 | Expiradas: 0
- BUY: 492 | SELL: 102

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 17150 | Total candidatos: 509596 | Seleccionados: 15667
- Candidatos por zona (promedio): 29.7
- **Edad (barras)** - Candidatos: med=38, max=151 | Seleccionados: med=37, max=149
- **Score** - Candidatos: avg=0.48 | Seleccionados: avg=0.54
- **TF Candidatos**: {240: 145385, 60: 117357, 15: 98467, 5: 83540, 1440: 64847}
- **TF Seleccionados**: {60: 5495, 5: 5700, 15: 3092, 240: 1316, 1440: 64}
- **DistATR** - Candidatos: avg=92.6 | Seleccionados: avg=10.7
- **Razones de selección**: {'Fallback<15': 4696, 'InBand[10,15]': 10971}
- **En banda [10,15] ATR**: 41356/509596 (8.1%)

### Take Profit (TP)
- Zonas analizadas: 17311 | Total candidatos: 288481 | Seleccionados: 17311
- Candidatos por zona (promedio): 16.7
- **Edad (barras)** - Candidatos: med=41, max=151 | Seleccionados: med=45, max=150
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.36
- **Priority Candidatos**: {'P3': 288481}
- **Priority Seleccionados**: {'P4_Fallback': 2302, 'P3': 15009}
- **Type Candidatos**: {'Swing': 288481}
- **Type Seleccionados**: {'Calculated': 2302, 'Swing': 15009}
- **TF Candidatos**: {60: 73625, 240: 66575, 15: 63351, 1440: 49101, 5: 35829}
- **TF Seleccionados**: {-1: 2302, 5: 4633, 60: 3508, 15: 4689, 240: 1669, 1440: 510}
- **DistATR** - Candidatos: avg=102.8 | Seleccionados: avg=19.0
- **RR** - Candidatos: avg=7.81 | Seleccionados: avg=1.28
- **Razones de selección**: {'NoStructuralTarget': 2302, 'R:R_y_Distancia_OK': 13846, 'R:R_OK_(Distancia_ignorada)': 1102, 'Distancia_OK_(R:R_ignorado)': 61}

### 🎯 Recomendaciones
- ⚠️ SL: 47% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- KeptAligned ratio≈ 0.14.