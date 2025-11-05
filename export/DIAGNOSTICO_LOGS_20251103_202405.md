# Informe Diagnóstico de Logs - 2025-11-03 20:33:49

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251103_202405.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251103_202405.csv`

## DFM
- Eventos de evaluación: 3172
- Evaluaciones Bull: 6125 | Bear: 401
- Pasaron umbral (PassedThreshold): 5222
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:19, 4:753, 5:1330, 6:2057, 7:2357, 8:10, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5473
- KeptAligned: 32680/38434 | KeptCounter: 12195/16248
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.505 | AvgProxCounter≈ 0.233
  - AvgDistATRAligned≈ 4.27 | AvgDistATRCounter≈ 1.44
- PreferAligned eventos: 5129 | Filtradas contra-bias: 9234

### Proximity (Pre-PreferAligned)
- Eventos: 5473
- Aligned pre: 32680/44875 | Counter pre: 12195/44875
- AvgProxAligned(pre)≈ 0.505 | AvgDistATRAligned(pre)≈ 4.27

### Proximity Drivers
- Eventos: 5473
- Alineadas: n=32680 | BaseProx≈ 0.572 | ZoneATR≈ 4.94 | SizePenalty≈ 0.969 | FinalProx≈ 0.560
- Contra-bias: n=2961 | BaseProx≈ 0.510 | ZoneATR≈ 4.90 | SizePenalty≈ 0.967 | FinalProx≈ 0.496

## Risk
- Eventos: 5473
- Accepted=6673 | RejSL=19734 | RejTP=2165 | RejRR=6500 | RejEntry=569
### Risk Drivers (Rechazos por SL)
- Alineadas: n=17823 | SLDistATR≈ 37.84 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=1911 | SLDistATR≈ 36.81 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:5126,20-25:2549,25+:10148
- HistSL Counter 0-10:0,10-15:0,15-20:437,20-25:321,25+:1153

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 282 | Unmatched: 6391
- 0-10: Wins=70 Losses=18 WR=79.5% (n=88)
- 10-15: Wins=18 Losses=176 WR=9.3% (n=194)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=88 Losses=194 WR=31.2% (n=282)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 6673 | Aligned=6488 (97.2%)
- Core≈ 1.00 | Prox≈ 0.62 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.38 | Confidence≈ 0.00
- SL_TF dist: {'-1': 1439, '60': 2848, '15': 839, '5': 834, '240': 700, '1440': 13} | SL_Structural≈ 78.4%
- TP_TF dist: {'-1': 2083, '60': 1657, '5': 933, '15': 1284, '240': 639, '1440': 77} | TP_Structural≈ 68.8%

### SLPick por Bandas y TF
- Bandas: lt8=2167, 8-10=782, 10-12.5=1688, 12.5-15=2036, >15=0
- TF: 5m=834, 15m=839, 60m=2848, 240m=700, 1440m=13
- RR plan por bandas: 0-10≈ 3.32 (n=2949), 10-15≈ 1.63 (n=3724)

## CancelBias (EMA200@60m)
- Eventos: 288
- Distribución Bias: {'Bullish': 234, 'Bearish': 54, 'Neutral': 0}
- Coherencia (Close>EMA): 234/288 (81.2%)

## StructureFusion
- Trazas por zona: 54682 | Zonas con Anchors: 53962
- Dir zonas (zona): Bull=31292 Bear=23390 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 10.0, WithAnchors≈ 9.9, DirBull≈ 5.7, DirBear≈ 4.3, DirNeutral≈ 0.0
- Razones de dirección: {'triggers-only': 641, 'tie-bias': 1531, 'anchors+triggers': 52510}
- TF Triggers: {'5': 14732, '15': 17878, '60': 22072}
- TF Anchors: {'240': 53930, '1440': 44408}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5475 | Distribución: {'Bullish': 4223, 'Bearish': 1252, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 4223/5475

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,48': 1, 'score decayó a 0,40': 3, 'score decayó a 0,36': 1, 'score decayó a 0,29': 1, 'estructura no existe': 4, 'score decayó a 0,46': 3, 'score decayó a 0,32': 1, 'score decayó a 0,41': 1}
- Cancel_BOS (diag): por acción {'BUY': 1, 'SELL': 0} | por bias {'Bullish': 0, 'Bearish': 1, 'Neutral': 0}

## CSV de Trades
- Filas: 149 | Ejecutadas: 34 | Canceladas: 0 | Expiradas: 0
- BUY: 154 | SELL: 29

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 34164 | Total candidatos: 893897 | Seleccionados: 29255
- Candidatos por zona (promedio): 26.2
- **Edad (barras)** - Candidatos: med=35, max=151 | Seleccionados: med=44, max=151
- **Score** - Candidatos: avg=0.50 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 331716, 60: 212717, 1440: 128640, 15: 128353, 5: 92471}
- **TF Seleccionados**: {60: 14845, 15: 3672, 5: 3391, 240: 6611, 1440: 736}
- **DistATR** - Candidatos: avg=55.1 | Seleccionados: avg=10.4
- **Razones de selección**: {'Fallback<15': 10199, 'InBand[10,15]': 19056}
- **En banda [10,15] ATR**: 77265/893897 (8.6%)

### Take Profit (TP)
- Zonas analizadas: 35072 | Total candidatos: 546945 | Seleccionados: 35072
- Candidatos por zona (promedio): 15.6
- **Edad (barras)** - Candidatos: med=32, max=151 | Seleccionados: med=41, max=151
- **Score** - Candidatos: avg=0.55 | Seleccionados: avg=0.35
- **Priority Candidatos**: {'P3': 546945}
- **Priority Seleccionados**: {'P4_Fallback': 6134, 'P3': 28938}
- **Type Candidatos**: {'Swing': 546945}
- **Type Seleccionados**: {'Calculated': 6134, 'Swing': 28938}
- **TF Candidatos**: {15: 128779, 5: 122506, 60: 108421, 240: 94040, 1440: 93199}
- **TF Seleccionados**: {-1: 6134, 5: 8397, 60: 7741, 15: 6951, 240: 4847, 1440: 1002}
- **DistATR** - Candidatos: avg=55.3 | Seleccionados: avg=24.8
- **RR** - Candidatos: avg=2.76 | Seleccionados: avg=1.00
- **Razones de selección**: {'NoStructuralTarget': 6134, 'R:R_y_Distancia_OK': 26319, 'R:R_OK_(Distancia_ignorada)': 2445, 'Distancia_OK_(R:R_ignorado)': 174}

### 🎯 Recomendaciones
- ⚠️ SL: 61% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- KeptAligned ratio≈ 0.85.