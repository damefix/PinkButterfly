# Informe Diagnóstico de Logs - 2025-11-04 09:00:01

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_083841.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_083841.csv`

## DFM
- Eventos de evaluación: 4927
- Evaluaciones Bull: 5164 | Bear: 1661
- Pasaron umbral (PassedThreshold): 2053
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:627, 4:3022, 5:1944, 6:960, 7:272, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 19600
- KeptAligned: 17618/123789 | KeptCounter: 7961/75754
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.140 | AvgProxCounter≈ 0.076
  - AvgDistATRAligned≈ 2.55 | AvgDistATRCounter≈ 0.71
- PreferAligned eventos: 8731 | Filtradas contra-bias: 1471

### Proximity (Pre-PreferAligned)
- Eventos: 19600
- Aligned pre: 17618/25579 | Counter pre: 7961/25579
- AvgProxAligned(pre)≈ 0.140 | AvgDistATRAligned(pre)≈ 2.55

### Proximity Drivers
- Eventos: 19600
- Alineadas: n=17618 | BaseProx≈ 0.441 | ZoneATR≈ 19.73 | SizePenalty≈ 0.727 | FinalProx≈ 0.321
- Contra-bias: n=6490 | BaseProx≈ 0.466 | ZoneATR≈ 29.43 | SizePenalty≈ 0.629 | FinalProx≈ 0.287

## Risk
- Eventos: 12735
- Accepted=6913 | RejSL=11508 | RejTP=628 | RejRR=5059 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=7966 | SLDistATR≈ 24.88 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=3542 | SLDistATR≈ 26.12 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:3930,20-25:1707,25+:2329
- HistSL Counter 0-10:0,10-15:0,15-20:1558,20-25:798,25+:1186

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 733 | Unmatched: 6180
- 0-10: Wins=65 Losses=85 WR=43.3% (n=150)
- 10-15: Wins=168 Losses=415 WR=28.8% (n=583)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=233 Losses=500 WR=31.8% (n=733)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 6913 | Aligned=4995 (72.3%)
- Core≈ 1.00 | Prox≈ 0.32 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.23 | Confidence≈ 0.00
- SL_TF dist: {'-1': 1356, '5': 1662, '60': 2242, '15': 897, '240': 690, '1440': 66} | SL_Structural≈ 80.4%
- TP_TF dist: {'-1': 1465, '5': 1535, '15': 1736, '60': 1232, '240': 765, '1440': 180} | TP_Structural≈ 78.8%

### SLPick por Bandas y TF
- Bandas: lt8=1720, 8-10=936, 10-12.5=1971, 12.5-15=2286, >15=0
- TF: 5m=1662, 15m=897, 60m=2242, 240m=690, 1440m=66
- RR plan por bandas: 0-10≈ 2.97 (n=2656), 10-15≈ 1.77 (n=4257)

## CancelBias (EMA200@60m)
- Eventos: 437
- Distribución Bias: {'Bullish': 400, 'Bearish': 37, 'Neutral': 0}
- Coherencia (Close>EMA): 400/437 (91.5%)

## StructureFusion
- Trazas por zona: 199543 | Zonas con Anchors: 194051
- Dir zonas (zona): Bull=85880 Bear=113663 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 10.2, WithAnchors≈ 9.9, DirBull≈ 4.4, DirBear≈ 5.8, DirNeutral≈ 0.0
- Razones de dirección: {'triggers-only': 4471, 'tie-bias': 9853, 'anchors+triggers': 185219}
- TF Triggers: {'5': 55384, '15': 67682, '60': 76477}
- TF Anchors: {'240': 191901, '1440': 166371}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 19601 | Distribución: {'Bullish': 12128, 'Bearish': 7473, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 12128/19601

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 7, 'score decayó a 0,21': 1, 'score decayó a 0,49': 1, 'score decayó a 0,36': 1, 'score decayó a 0,20': 1, 'score decayó a 0,46': 1}
- Cancel_BOS (diag): por acción {'BUY': 19, 'SELL': 4} | por bias {'Bullish': 4, 'Bearish': 19, 'Neutral': 0}

## CSV de Trades
- Filas: 319 | Ejecutadas: 123 | Canceladas: 0 | Expiradas: 0
- BUY: 411 | SELL: 31

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 23792 | Total candidatos: 662020 | Seleccionados: 21875
- Candidatos por zona (promedio): 27.8
- **Edad (barras)** - Candidatos: med=39, max=151 | Seleccionados: med=37, max=151
- **Score** - Candidatos: avg=0.48 | Seleccionados: avg=0.54
- **TF Candidatos**: {240: 194295, 60: 155494, 15: 127739, 5: 107990, 1440: 76502}
- **TF Seleccionados**: {5: 7429, 15: 3713, 60: 8034, 240: 2412, 1440: 287}
- **DistATR** - Candidatos: avg=82.0 | Seleccionados: avg=10.6
- **Razones de selección**: {'Fallback<15': 6873, 'InBand[10,15]': 15002}
- **En banda [10,15] ATR**: 54429/662020 (8.2%)

### Take Profit (TP)
- Zonas analizadas: 24108 | Total candidatos: 423793 | Seleccionados: 24108
- Candidatos por zona (promedio): 17.6
- **Edad (barras)** - Candidatos: med=41, max=151 | Seleccionados: med=46, max=150
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.37
- **Priority Candidatos**: {'P3': 423793}
- **Priority Seleccionados**: {'P4_Fallback': 2531, 'P3': 21577}
- **Type Candidatos**: {'Swing': 423793}
- **Type Seleccionados**: {'Calculated': 2531, 'Swing': 21577}
- **TF Candidatos**: {60: 108878, 240: 106272, 15: 88282, 1440: 71406, 5: 48955}
- **TF Seleccionados**: {-1: 2531, 5: 6749, 15: 6525, 60: 4795, 240: 2796, 1440: 712}
- **DistATR** - Candidatos: avg=97.8 | Seleccionados: avg=19.2
- **RR** - Candidatos: avg=7.35 | Seleccionados: avg=1.23
- **Razones de selección**: {'NoStructuralTarget': 2531, 'R:R_y_Distancia_OK': 19768, 'Distancia_OK_(R:R_ignorado)': 69, 'R:R_OK_(Distancia_ignorada)': 1740}

### 🎯 Recomendaciones
- ⚠️ SL: 48% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- KeptAligned ratio≈ 0.14.