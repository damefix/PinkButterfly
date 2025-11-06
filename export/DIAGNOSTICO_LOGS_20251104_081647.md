# Informe Diagnóstico de Logs - 2025-11-04 08:21:34

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_081647.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_081647.csv`

## DFM
- Eventos de evaluación: 1640
- Evaluaciones Bull: 2229 | Bear: 189
- Pasaron umbral (PassedThreshold): 914
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:91, 4:982, 5:763, 6:471, 7:111, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5832
- KeptAligned: 7872/51088 | KeptCounter: 1236/12452
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.267 | AvgProxCounter≈ 0.039
  - AvgDistATRAligned≈ 3.64 | AvgDistATRCounter≈ 0.41
- PreferAligned eventos: 3968 | Filtradas contra-bias: 274

### Proximity (Pre-PreferAligned)
- Eventos: 5832
- Aligned pre: 7872/9108 | Counter pre: 1236/9108
- AvgProxAligned(pre)≈ 0.267 | AvgDistATRAligned(pre)≈ 3.64

### Proximity Drivers
- Eventos: 5832
- Alineadas: n=7872 | BaseProx≈ 0.459 | ZoneATR≈ 15.62 | SizePenalty≈ 0.791 | FinalProx≈ 0.369
- Contra-bias: n=962 | BaseProx≈ 0.438 | ZoneATR≈ 27.27 | SizePenalty≈ 0.646 | FinalProx≈ 0.278

## Risk
- Eventos: 4548
- Accepted=2526 | RejSL=3390 | RejTP=136 | RejRR=2782 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=2774 | SLDistATR≈ 22.95 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=616 | SLDistATR≈ 23.99 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:1673,20-25:564,25+:537
- HistSL Counter 0-10:0,10-15:0,15-20:282,20-25:166,25+:168

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 332 | Unmatched: 2194
- 0-10: Wins=45 Losses=49 WR=47.9% (n=94)
- 10-15: Wins=115 Losses=123 WR=48.3% (n=238)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=160 Losses=172 WR=48.2% (n=332)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2526 | Aligned=2283 (90.4%)
- Core≈ 0.99 | Prox≈ 0.34 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.89 | Confidence≈ 0.00
- SL_TF dist: {'-1': 476, '5': 740, '15': 448, '60': 768, '240': 90, '1440': 4} | SL_Structural≈ 81.2%
- TP_TF dist: {'-1': 853, '15': 601, '5': 469, '60': 444, '240': 156, '1440': 3} | TP_Structural≈ 66.2%

### SLPick por Bandas y TF
- Bandas: lt8=690, 8-10=284, 10-12.5=751, 12.5-15=801, >15=0
- TF: 5m=740, 15m=448, 60m=768, 240m=90, 1440m=4
- RR plan por bandas: 0-10≈ 2.39 (n=974), 10-15≈ 1.57 (n=1552)

## CancelBias (EMA200@60m)
- Eventos: 261
- Distribución Bias: {'Bullish': 204, 'Bearish': 57, 'Neutral': 0}
- Coherencia (Close>EMA): 204/261 (78.2%)

## StructureFusion
- Trazas por zona: 63540 | Zonas con Anchors: 62838
- Dir zonas (zona): Bull=31090 Bear=32450 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 10.9, WithAnchors≈ 10.8, DirBull≈ 5.3, DirBear≈ 5.6, DirNeutral≈ 0.0
- Razones de dirección: {'tie-bias': 1341, 'triggers-only': 627, 'anchors+triggers': 61572}
- TF Triggers: {'5': 16409, '15': 22246, '60': 24885}
- TF Anchors: {'240': 62809, '1440': 53749}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5832 | Distribución: {'Bullish': 3736, 'Bearish': 2096, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3736/5832

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 3, 'score decayó a 0,39': 1}
- Cancel_BOS (diag): por acción {'BUY': 3, 'SELL': 0} | por bias {'Bullish': 0, 'Bearish': 3, 'Neutral': 0}

## CSV de Trades
- Filas: 179 | Ejecutadas: 77 | Canceladas: 0 | Expiradas: 0
- BUY: 232 | SELL: 24

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 8753 | Total candidatos: 247629 | Seleccionados: 7748
- Candidatos por zona (promedio): 28.3
- **Edad (barras)** - Candidatos: med=38, max=150 | Seleccionados: med=37, max=148
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.55
- **TF Candidatos**: {60: 70744, 240: 70454, 15: 48621, 5: 37494, 1440: 20316}
- **TF Seleccionados**: {5: 2839, 60: 2666, 15: 1494, 240: 701, 1440: 48}
- **DistATR** - Candidatos: avg=66.1 | Seleccionados: avg=10.7
- **Razones de selección**: {'Fallback<15': 2493, 'InBand[10,15]': 5255}
- **En banda [10,15] ATR**: 19062/247629 (7.7%)

### Take Profit (TP)
- Zonas analizadas: 8834 | Total candidatos: 130153 | Seleccionados: 8834
- Candidatos por zona (promedio): 14.7
- **Edad (barras)** - Candidatos: med=25, max=150 | Seleccionados: med=37, max=150
- **Score** - Candidatos: avg=0.58 | Seleccionados: avg=0.42
- **Priority Candidatos**: {'P3': 130153}
- **Priority Seleccionados**: {'P4_Fallback': 1308, 'P3': 7526}
- **Type Candidatos**: {'Swing': 130153}
- **Type Seleccionados**: {'Calculated': 1308, 'Swing': 7526}
- **TF Candidatos**: {5: 39039, 60: 24615, 15: 24438, 240: 21703, 1440: 20358}
- **TF Seleccionados**: {-1: 1308, 5: 2840, 15: 1975, 60: 1658, 240: 670, 1440: 383}
- **DistATR** - Candidatos: avg=57.2 | Seleccionados: avg=16.9
- **RR** - Candidatos: avg=4.68 | Seleccionados: avg=1.15
- **Razones de selección**: {'NoStructuralTarget': 1308, 'R:R_y_Distancia_OK': 6953, 'R:R_OK_(Distancia_ignorada)': 554, 'Distancia_OK_(R:R_ignorado)': 19}

### 🎯 Recomendaciones
- ⚠️ SL: 48% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- KeptAligned ratio≈ 0.15.