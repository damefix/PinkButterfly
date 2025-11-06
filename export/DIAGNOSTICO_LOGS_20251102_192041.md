# Informe Diagnóstico de Logs - 2025-11-02 19:24:45

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251102_192041.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251102_192041.csv`

## DFM
- Eventos de evaluación: 1681
- Evaluaciones Bull: 1815 | Bear: 399
- Pasaron umbral (PassedThreshold): 1527
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:232, 4:386, 5:120, 6:539, 7:450, 8:368, 9:119

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 3451/27109 | KeptCounter: 1750/14877
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.138 | AvgProxCounter≈ 0.058
  - AvgDistATRAligned≈ 2.71 | AvgDistATRCounter≈ 0.75
- PreferAligned eventos: 2021 | Filtradas contra-bias: 246

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 3451/5201 | Counter pre: 1750/5201
- AvgProxAligned(pre)≈ 0.138 | AvgDistATRAligned(pre)≈ 2.71

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=3451 | BaseProx≈ 0.442 | ZoneATR≈ 17.04 | SizePenalty≈ 0.767 | FinalProx≈ 0.342
- Contra-bias: n=1504 | BaseProx≈ 0.460 | ZoneATR≈ 32.03 | SizePenalty≈ 0.566 | FinalProx≈ 0.256

## Risk
- Eventos: 2986
- Accepted=2279 | RejSL=1795 | RejTP=65 | RejRR=816 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1223 | SLDistATR≈ 25.52 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=572 | SLDistATR≈ 26.47 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:575,20-25:347,25+:301
- HistSL Counter 0-10:0,10-15:0,15-20:255,20-25:97,25+:220

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 235 | Unmatched: 2044
- 0-10: Wins=18 Losses=40 WR=31.0% (n=58)
- 10-15: Wins=80 Losses=96 WR=45.5% (n=176)
- 15-20: Wins=1 Losses=0 WR=100.0% (n=1)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=99 Losses=136 WR=42.1% (n=235)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2279 | Aligned=1509 (66.2%)
- Core≈ 1.00 | Prox≈ 0.34 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.94 | Confidence≈ 0.00
- SL_TF dist: {'-1': 711, '15': 1328, '5': 240} | SL_Structural≈ 68.8%
- TP_TF dist: {'-1': 1175, '15': 892, '5': 212} | TP_Structural≈ 48.4%

### SLPick por Bandas y TF
- Bandas: lt8=667, 8-10=294, 10-12.5=635, 12.5-15=683, >15=0
- TF: 5m=240, 15m=1328, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.65 (n=961), 10-15≈ 1.41 (n=1318)

## CancelBias (EMA200@60m)
- Eventos: 441
- Distribución Bias: {'Bullish': 339, 'Bearish': 102, 'Neutral': 0}
- Coherencia (Close>EMA): 339/441 (76.9%)

## StructureFusion
- Trazas por zona: 41986 | Zonas con Anchors: 41638
- Dir zonas (zona): Bull=26321 Bear=15665 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.4, WithAnchors≈ 8.3, DirBull≈ 5.3, DirBear≈ 3.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 40502, 'tie-bias': 1179, 'triggers-only': 305}
- TF Triggers: {'60': 20412, '15': 16963, '5': 4611}
- TF Anchors: {'240': 41613, '1440': 35992}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 3, 'score decayó a 0,47': 2, 'score decayó a 0,44': 1, 'score decayó a 0,22': 1, 'score decayó a 0,46': 1, 'score decayó a 0,25': 2, 'score decayó a 0,36': 1}
- Cancel_BOS (diag): por acción {'BUY': 16, 'SELL': 3} | por bias {'Bullish': 3, 'Bearish': 16, 'Neutral': 0}

## CSV de Trades
- Filas: 186 | Ejecutadas: 61 | Canceladas: 0 | Expiradas: 0
- BUY: 178 | SELL: 69

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4350 | Total candidatos: 43431 | Seleccionados: 2894
- Candidatos por zona (promedio): 10.0
- **Edad (barras)** - Candidatos: med=52, max=150 | Seleccionados: med=51, max=145
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.47
- **TF Candidatos**: {15: 32266}
- **TF Seleccionados**: {15: 2894}
- **DistATR** - Candidatos: avg=21.8 | Seleccionados: avg=10.5
- **Razones de selección**: {'Fallback<15': 1025, 'InBand[10,15]': 1869}
- **En banda [10,15] ATR**: 4889/32266 (15.2%)

### Take Profit (TP)
- Zonas analizadas: 4955 | Total candidatos: 78445 | Seleccionados: 4122
- Candidatos por zona (promedio): 15.8
- **Edad (barras)** - Candidatos: med=18686, max=23183 | Seleccionados: med=8, max=150
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.23
- **Priority Candidatos**: {'P3': 73320}
- **Priority Seleccionados**: {'P4_Fallback': 1938, 'P3': 2184}
- **Type Candidatos**: {'Swing': 73320}
- **Type Seleccionados**: {'Calculated': 1938, 'Swing': 2184}
- **TF Candidatos**: {240: 21144, 1440: 20549, 60: 16962, 15: 14665}
- **TF Seleccionados**: {-1: 1938, 15: 2184}
- **DistATR** - Candidatos: avg=126.3 | Seleccionados: avg=18.0
- **RR** - Candidatos: avg=9.45 | Seleccionados: avg=1.40
- **Razones de selección**: {'NoStructuralTarget': 1938, 'R:R_y_Distancia_OK': 2053, 'Distancia_OK_(R:R_ignorado)': 32, 'R:R_OK_(Distancia_ignorada)': 99}

### 🎯 Recomendaciones
- ⚠️ SL: 64% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 47% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.13.