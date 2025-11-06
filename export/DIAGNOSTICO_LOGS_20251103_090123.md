# Informe Diagnóstico de Logs - 2025-11-03 09:05:37

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251103_090123.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251103_090123.csv`

## DFM
- Eventos de evaluación: 1532
- Evaluaciones Bull: 1666 | Bear: 341
- Pasaron umbral (PassedThreshold): 762
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:202, 4:752, 5:549, 6:393, 7:111, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 3185/26813 | KeptCounter: 1580/14453
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.135 | AvgProxCounter≈ 0.057
  - AvgDistATRAligned≈ 2.38 | AvgDistATRCounter≈ 0.63
- PreferAligned eventos: 1954 | Filtradas contra-bias: 217

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 3185/4765 | Counter pre: 1580/4765
- AvgProxAligned(pre)≈ 0.135 | AvgDistATRAligned(pre)≈ 2.38

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=3185 | BaseProx≈ 0.447 | ZoneATR≈ 16.77 | SizePenalty≈ 0.776 | FinalProx≈ 0.346
- Contra-bias: n=1363 | BaseProx≈ 0.469 | ZoneATR≈ 31.37 | SizePenalty≈ 0.574 | FinalProx≈ 0.264

## Risk
- Eventos: 2853
- Accepted=2092 | RejSL=1624 | RejTP=57 | RejRR=775 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1103 | SLDistATR≈ 25.79 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=521 | SLDistATR≈ 25.42 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:532,20-25:303,25+:268
- HistSL Counter 0-10:0,10-15:0,15-20:233,20-25:85,25+:203

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 313 | Unmatched: 1779
- 0-10: Wins=41 Losses=38 WR=51.9% (n=79)
- 10-15: Wins=136 Losses=98 WR=58.1% (n=234)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=177 Losses=136 WR=56.5% (n=313)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2092 | Aligned=1402 (67.0%)
- Core≈ 1.00 | Prox≈ 0.35 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.00 | Confidence≈ 0.00
- SL_TF dist: {'-1': 741, '15': 1158, '5': 193} | SL_Structural≈ 64.6%
- TP_TF dist: {'15': 817, '-1': 1100, '5': 175} | TP_Structural≈ 47.4%

### SLPick por Bandas y TF
- Bandas: lt8=705, 8-10=305, 10-12.5=584, 12.5-15=498, >15=0
- TF: 5m=193, 15m=1158, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.61 (n=1010), 10-15≈ 1.43 (n=1082)

## CancelBias (EMA200@60m)
- Eventos: 305
- Distribución Bias: {'Bullish': 249, 'Bearish': 56, 'Neutral': 0}
- Coherencia (Close>EMA): 249/305 (81.6%)

## StructureFusion
- Trazas por zona: 41266 | Zonas con Anchors: 40906
- Dir zonas (zona): Bull=25866 Bear=15400 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.3, WithAnchors≈ 8.2, DirBull≈ 5.2, DirBear≈ 3.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 39801, 'tie-bias': 1143, 'triggers-only': 322}
- TF Triggers: {'60': 20212, '15': 16521, '5': 4533}
- TF Anchors: {'240': 40882, '1440': 35280}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 2, 'score decayó a 0,26': 1, 'score decayó a 0,23': 1}
- Cancel_BOS (diag): por acción {'BUY': 10, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 10, 'Neutral': 0}

## CSV de Trades
- Filas: 152 | Ejecutadas: 61 | Canceladas: 0 | Expiradas: 0
- BUY: 175 | SELL: 38

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 3915 | Total candidatos: 36303 | Seleccionados: 2607
- Candidatos por zona (promedio): 9.3
- **Edad (barras)** - Candidatos: med=47, max=150 | Seleccionados: med=47, max=145
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.47
- **TF Candidatos**: {15: 27025}
- **TF Seleccionados**: {15: 2607}
- **DistATR** - Candidatos: avg=20.7 | Seleccionados: avg=10.5
- **Razones de selección**: {'Fallback<15': 938, 'InBand[10,15]': 1669}
- **En banda [10,15] ATR**: 4325/27025 (16.0%)

### Take Profit (TP)
- Zonas analizadas: 4548 | Total candidatos: 66500 | Seleccionados: 3834
- Candidatos por zona (promedio): 14.6
- **Edad (barras)** - Candidatos: med=18570, max=23176 | Seleccionados: med=8, max=144
- **Score** - Candidatos: avg=0.48 | Seleccionados: avg=0.23
- **Priority Candidatos**: {'P3': 62525}
- **Priority Seleccionados**: {'P3': 2032, 'P4_Fallback': 1802}
- **Type Candidatos**: {'Swing': 62525}
- **Type Seleccionados**: {'Swing': 2032, 'Calculated': 1802}
- **TF Candidatos**: {240: 18021, 1440: 17437, 60: 14734, 15: 12333}
- **TF Seleccionados**: {15: 2032, -1: 1802}
- **DistATR** - Candidatos: avg=116.6 | Seleccionados: avg=17.2
- **RR** - Candidatos: avg=8.91 | Seleccionados: avg=1.40
- **Razones de selección**: {'R:R_y_Distancia_OK': 1907, 'NoStructuralTarget': 1802, 'Distancia_OK_(R:R_ignorado)': 37, 'R:R_OK_(Distancia_ignorada)': 88}

### 🎯 Recomendaciones
- ⚠️ SL: 61% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 47% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.12.