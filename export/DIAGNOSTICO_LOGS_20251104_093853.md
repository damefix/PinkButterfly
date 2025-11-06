# Informe Diagnóstico de Logs - 2025-11-04 09:48:15

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_093853.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_093854.csv`

## DFM
- Eventos de evaluación: 1630
- Evaluaciones Bull: 2226 | Bear: 177
- Pasaron umbral (PassedThreshold): 886
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:96, 4:991, 5:754, 6:446, 7:116, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 6524
- KeptAligned: 7137/57719 | KeptCounter: 1247/12549
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.158 | AvgProxCounter≈ 0.034
  - AvgDistATRAligned≈ 3.29 | AvgDistATRCounter≈ 0.37
- PreferAligned eventos: 3458 | Filtradas contra-bias: 266

### Proximity (Pre-PreferAligned)
- Eventos: 6524
- Aligned pre: 7137/8384 | Counter pre: 1247/8384
- AvgProxAligned(pre)≈ 0.158 | AvgDistATRAligned(pre)≈ 3.29

### Proximity Drivers
- Eventos: 6524
- Alineadas: n=7137 | BaseProx≈ 0.412 | ZoneATR≈ 17.32 | SizePenalty≈ 0.759 | FinalProx≈ 0.313
- Contra-bias: n=981 | BaseProx≈ 0.437 | ZoneATR≈ 27.25 | SizePenalty≈ 0.645 | FinalProx≈ 0.277

## Risk
- Eventos: 4045
- Accepted=2509 | RejSL=3477 | RejTP=135 | RejRR=1997 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=2858 | SLDistATR≈ 23.10 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=619 | SLDistATR≈ 24.00 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:1469,20-25:858,25+:531
- HistSL Counter 0-10:0,10-15:0,15-20:282,20-25:165,25+:172

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 328 | Unmatched: 2181
- 0-10: Wins=46 Losses=45 WR=50.5% (n=91)
- 10-15: Wins=109 Losses=128 WR=46.0% (n=237)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=155 Losses=173 WR=47.3% (n=328)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2509 | Aligned=2250 (89.7%)
- Core≈ 0.99 | Prox≈ 0.34 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.93 | Confidence≈ 0.00
- SL_TF dist: {'-1': 483, '5': 717, '15': 438, '60': 777, '240': 90, '1440': 4} | SL_Structural≈ 80.7%
- TP_TF dist: {'-1': 815, '5': 485, '15': 566, '60': 443, '1440': 45, '240': 155} | TP_Structural≈ 67.5%

### SLPick por Bandas y TF
- Bandas: lt8=714, 8-10=276, 10-12.5=739, 12.5-15=780, >15=0
- TF: 5m=717, 15m=438, 60m=777, 240m=90, 1440m=4
- RR plan por bandas: 0-10≈ 2.46 (n=990), 10-15≈ 1.58 (n=1519)

## CancelBias (EMA200@60m)
- Eventos: 249
- Distribución Bias: {'Bullish': 201, 'Bearish': 48, 'Neutral': 0}
- Coherencia (Close>EMA): 201/249 (80.7%)

## StructureFusion
- Trazas por zona: 70268 | Zonas con Anchors: 69669
- Dir zonas (zona): Bull=31368 Bear=38900 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 10.8, WithAnchors≈ 10.7, DirBull≈ 4.8, DirBear≈ 6.0, DirNeutral≈ 0.0
- Razones de dirección: {'tie-bias': 1233, 'triggers-only': 538, 'anchors+triggers': 68497}
- TF Triggers: {'5': 17992, '15': 24739, '60': 27537}
- TF Anchors: {'240': 69558, '1440': 61435}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 6524 | Distribución: {'Bullish': 3736, 'Bearish': 2788, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3736/6524

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 3, 'score decayó a 0,39': 1}
- Cancel_BOS (diag): por acción {'BUY': 3, 'SELL': 0} | por bias {'Bullish': 0, 'Bearish': 3, 'Neutral': 0}

## CSV de Trades
- Filas: 177 | Ejecutadas: 76 | Canceladas: 0 | Expiradas: 0
- BUY: 226 | SELL: 27

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 8056 | Total candidatos: 228977 | Seleccionados: 7173
- Candidatos por zona (promedio): 28.4
- **Edad (barras)** - Candidatos: med=38, max=150 | Seleccionados: med=40, max=148
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.52
- **TF Candidatos**: {240: 69643, 60: 62698, 15: 42482, 5: 34194, 1440: 19960}
- **TF Seleccionados**: {5: 2241, 15: 1505, 60: 2676, 1440: 50, 240: 701}
- **DistATR** - Candidatos: avg=65.2 | Seleccionados: avg=11.0
- **Razones de selección**: {'Fallback<15': 1921, 'InBand[10,15]': 5252}
- **En banda [10,15] ATR**: 19057/228977 (8.3%)

### Take Profit (TP)
- Zonas analizadas: 8118 | Total candidatos: 101370 | Seleccionados: 8118
- Candidatos por zona (promedio): 12.5
- **Edad (barras)** - Candidatos: med=35, max=150 | Seleccionados: med=40, max=150
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.38
- **Priority Candidatos**: {'P3': 101370}
- **Priority Seleccionados**: {'P4_Fallback': 1254, 'P3': 6864}
- **Type Candidatos**: {'Swing': 101370}
- **Type Seleccionados**: {'Calculated': 1254, 'Swing': 6864}
- **TF Candidatos**: {60: 24886, 15: 24423, 240: 21423, 5: 15421, 1440: 15217}
- **TF Seleccionados**: {-1: 1254, 5: 2001, 15: 1886, 60: 1660, 1440: 653, 240: 664}
- **DistATR** - Candidatos: avg=58.0 | Seleccionados: avg=18.8
- **RR** - Candidatos: avg=3.97 | Seleccionados: avg=1.24
- **Razones de selección**: {'NoStructuralTarget': 1254, 'R:R_y_Distancia_OK': 6191, 'R:R_OK_(Distancia_ignorada)': 654, 'Distancia_OK_(R:R_ignorado)': 19}

### 🎯 Recomendaciones
- ⚠️ SL: 52% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- KeptAligned ratio≈ 0.12.