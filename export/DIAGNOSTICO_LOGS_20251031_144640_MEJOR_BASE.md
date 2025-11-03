# Informe Diagnóstico de Logs - 2025-10-31 12:30:09

- Log: `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251031_121934.log`
- CSV: `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251031_121934.csv`

## DFM
- Eventos de evaluación: 2770
- Evaluaciones Bull: 1831 | Bear: 2315
- Pasaron umbral (PassedThreshold): 3443
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:299, 4:326, 5:138, 6:1370, 7:1047, 8:866, 9:100

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 6249
- KeptAligned: 6167/29419 | KeptCounter: 1735/16478
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.168 | AvgProxCounter≈ 0.051
  - AvgDistATRAligned≈ 2.90 | AvgDistATRCounter≈ 0.48
- PreferAligned eventos: 3142 | Filtradas contra-bias: 274

### Proximity (Pre-PreferAligned)
- Eventos: 6249
- Aligned pre: 6167/7902 | Counter pre: 1735/7902
- AvgProxAligned(pre)≈ 0.168 | AvgDistATRAligned(pre)≈ 2.90

### Proximity Drivers
- Eventos: 6249
- Alineadas: n=6167 | BaseProx≈ 0.430 | ZoneATR≈ 15.28 | SizePenalty≈ 0.802 | FinalProx≈ 0.338
- Contra-bias: n=1461 | BaseProx≈ 0.476 | ZoneATR≈ 33.58 | SizePenalty≈ 0.581 | FinalProx≈ 0.271

## Risk
- Eventos: 4088
- Accepted=4233 | RejSL=1773 | RejTP=454 | RejRR=1168 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1227 | SLDistATR≈ 25.96 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=546 | SLDistATR≈ 31.75 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:645,20-25:282,25+:300
- HistSL Counter 0-10:0,10-15:0,15-20:203,20-25:101,25+:242

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 244 | Unmatched: 3989
- 0-10: Wins=42 Losses=89 WR=32.1% (n=131)
- 10-15: Wins=40 Losses=72 WR=35.7% (n=112)
- 15-20: Wins=1 Losses=0 WR=100.0% (n=1)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=83 Losses=161 WR=34.0% (n=244)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 4233 | Aligned=3437 (81.2%)
- Core≈ 0.99 | Prox≈ 0.34 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.51 | Confidence≈ 0.00
- SL_TF dist: {'-1': 2068, '15': 1757, '5': 408} | SL_Structural≈ 51.1%
- TP_TF dist: {'-1': 3030, '15': 1002, '5': 201} | TP_Structural≈ 28.4%

### SLPick por Bandas y TF
- Bandas: lt8=906, 8-10=462, 10-12.5=764, 12.5-15=2101, >15=0
- TF: 5m=408, 15m=1757, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.32 (n=1368), 10-15≈ 1.12 (n=2865)

## CancelBias (EMA200@60m)
- Eventos: 404
- Distribución Bias: {'Bullish': 300, 'Bearish': 104, 'Neutral': 0}
- Coherencia (Close>EMA): 300/404 (74.3%)

## StructureFusion
- Trazas por zona: 45897 | Zonas con Anchors: 45517
- Dir zonas (zona): Bull=26651 Bear=19246 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 7.3, WithAnchors≈ 7.3, DirBull≈ 4.3, DirBear≈ 3.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 44297, 'tie-bias': 1278, 'triggers-only': 322}
- TF Triggers: {'60': 22785, '15': 17224, '5': 5888}
- TF Anchors: {'240': 45450, '1440': 37800}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 6249 | Distribución: {'Bullish': 3055, 'Bearish': 3194, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3055/6249

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,41': 1, 'score decayó a 0,47': 2, 'score decayó a 0,30': 1, 'estructura no existe': 7, 'score decayó a 0,36': 1, 'score decayó a 0,38': 1, 'score decayó a 0,28': 1, 'score decayó a 0,40': 1}
- Cancel_BOS (diag): por acción {'BUY': 14, 'SELL': 4} | por bias {'Bullish': 4, 'Bearish': 14, 'Neutral': 0}

## CSV de Trades
- Filas: 231 | Ejecutadas: 81 | Canceladas: 0 | Expiradas: 0
- BUY: 221 | SELL: 91

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5245 | Total candidatos: 32492 | Seleccionados: 3133
- Candidatos por zona (promedio): 6.2
- **Edad (barras)** - Candidatos: med=32, max=80 | Seleccionados: med=26, max=69
- **Score** - Candidatos: avg=0.42 | Seleccionados: avg=0.44
- **TF Candidatos**: {15: 22638}
- **TF Seleccionados**: {15: 3133}
- **DistATR** - Candidatos: avg=16.8 | Seleccionados: avg=10.4
- **Razones de selección**: {'Fallback<15': 1136, 'InBand[10,15]': 1997}
- **En banda [10,15] ATR**: 4620/22638 (20.4%)

### Take Profit (TP)
- Zonas analizadas: 7628 | Total candidatos: 106520 | Seleccionados: 6803
- Candidatos por zona (promedio): 14.0
- **Edad (barras)** - Candidatos: med=21892, max=23295 | Seleccionados: med=0, max=71
- **Score** - Candidatos: avg=0.42 | Seleccionados: avg=0.16
- **Priority Candidatos**: {'P3': 102664}
- **Priority Seleccionados**: {'P4_Fallback': 3941, 'P3': 2862}
- **Type Candidatos**: {'Swing': 102664}
- **Type Seleccionados**: {'Calculated': 3941, 'Swing': 2862}
- **TF Candidatos**: {1440: 39801, 240: 28827, 60: 20816, 15: 13220}
- **TF Seleccionados**: {-1: 3941, 15: 2862}
- **DistATR** - Candidatos: avg=150.3 | Seleccionados: avg=14.8
- **RR** - Candidatos: avg=12.11 | Seleccionados: avg=1.17
- **Razones de selección**: {'NoStructuralTarget': 3941, 'R:R_y_Distancia_OK': 2761, 'R:R_OK_(Distancia_ignorada)': 76, 'Distancia_OK_(R:R_ignorado)': 25}

### 🎯 Recomendaciones
- ⚠️ SL: 57% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 58% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.21.