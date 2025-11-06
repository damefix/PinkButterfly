# Informe Diagnóstico de Logs - 2025-11-03 09:42:40

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251103_093754.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251103_093754.csv`

## DFM
- Eventos de evaluación: 1547
- Evaluaciones Bull: 1684 | Bear: 346
- Pasaron umbral (PassedThreshold): 748
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:212, 4:768, 5:559, 6:380, 7:111, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 3190/26852 | KeptCounter: 1605/14447
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.133 | AvgProxCounter≈ 0.056
  - AvgDistATRAligned≈ 2.38 | AvgDistATRCounter≈ 0.63
- PreferAligned eventos: 1965 | Filtradas contra-bias: 228

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 3190/4795 | Counter pre: 1605/4795
- AvgProxAligned(pre)≈ 0.133 | AvgDistATRAligned(pre)≈ 2.38

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=3190 | BaseProx≈ 0.438 | ZoneATR≈ 16.74 | SizePenalty≈ 0.776 | FinalProx≈ 0.340
- Contra-bias: n=1377 | BaseProx≈ 0.460 | ZoneATR≈ 31.43 | SizePenalty≈ 0.571 | FinalProx≈ 0.257

## Risk
- Eventos: 2866
- Accepted=2116 | RejSL=1607 | RejTP=61 | RejRR=783 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1108 | SLDistATR≈ 25.80 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=499 | SLDistATR≈ 25.76 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:533,20-25:305,25+:270
- HistSL Counter 0-10:0,10-15:0,15-20:210,20-25:94,25+:195

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 331 | Unmatched: 1785
- 0-10: Wins=40 Losses=53 WR=43.0% (n=93)
- 10-15: Wins=133 Losses=105 WR=55.9% (n=238)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=173 Losses=158 WR=52.3% (n=331)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2116 | Aligned=1398 (66.1%)
- Core≈ 1.00 | Prox≈ 0.35 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.99 | Confidence≈ 0.00
- SL_TF dist: {'-1': 750, '15': 1169, '5': 197} | SL_Structural≈ 64.6%
- TP_TF dist: {'15': 807, '-1': 1128, '5': 181} | TP_Structural≈ 46.7%

### SLPick por Bandas y TF
- Bandas: lt8=704, 8-10=307, 10-12.5=596, 12.5-15=509, >15=0
- TF: 5m=197, 15m=1169, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.59 (n=1011), 10-15≈ 1.44 (n=1105)

## CancelBias (EMA200@60m)
- Eventos: 290
- Distribución Bias: {'Bullish': 236, 'Bearish': 54, 'Neutral': 0}
- Coherencia (Close>EMA): 236/290 (81.4%)

## StructureFusion
- Trazas por zona: 41299 | Zonas con Anchors: 40939
- Dir zonas (zona): Bull=25848 Bear=15451 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.3, WithAnchors≈ 8.2, DirBull≈ 5.2, DirBear≈ 3.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 39811, 'tie-bias': 1166, 'triggers-only': 322}
- TF Triggers: {'60': 20232, '15': 16536, '5': 4531}
- TF Anchors: {'240': 40915, '1440': 35301}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 2, 'score decayó a 0,22': 1}
- Cancel_BOS (diag): por acción {'BUY': 8, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 8, 'Neutral': 0}

## CSV de Trades
- Filas: 154 | Ejecutadas: 64 | Canceladas: 0 | Expiradas: 0
- BUY: 180 | SELL: 38

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 3935 | Total candidatos: 36215 | Seleccionados: 2615
- Candidatos por zona (promedio): 9.2
- **Edad (barras)** - Candidatos: med=47, max=150 | Seleccionados: med=47, max=145
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.47
- **TF Candidatos**: {15: 26973}
- **TF Seleccionados**: {15: 2615}
- **DistATR** - Candidatos: avg=20.6 | Seleccionados: avg=10.5
- **Razones de selección**: {'Fallback<15': 948, 'InBand[10,15]': 1667}
- **En banda [10,15] ATR**: 4303/26973 (16.0%)

### Take Profit (TP)
- Zonas analizadas: 4567 | Total candidatos: 66622 | Seleccionados: 3852
- Candidatos por zona (promedio): 14.6
- **Edad (barras)** - Candidatos: med=18571, max=23176 | Seleccionados: med=8, max=150
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.23
- **Priority Candidatos**: {'P3': 62659}
- **Priority Seleccionados**: {'P3': 2039, 'P4_Fallback': 1813}
- **Type Candidatos**: {'Swing': 62659}
- **Type Seleccionados**: {'Swing': 2039, 'Calculated': 1813}
- **TF Candidatos**: {240: 18062, 1440: 17519, 60: 14752, 15: 12326}
- **TF Seleccionados**: {15: 2039, -1: 1813}
- **DistATR** - Candidatos: avg=116.7 | Seleccionados: avg=17.2
- **RR** - Candidatos: avg=8.92 | Seleccionados: avg=1.40
- **Razones de selección**: {'R:R_y_Distancia_OK': 1910, 'NoStructuralTarget': 1813, 'Distancia_OK_(R:R_ignorado)': 37, 'R:R_OK_(Distancia_ignorada)': 92}

### 🎯 Recomendaciones
- ⚠️ SL: 61% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 47% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.12.