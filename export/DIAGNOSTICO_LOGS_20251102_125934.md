# Informe Diagnóstico de Logs - 2025-11-02 13:02:43

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251102_125934.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251102_125934.csv`

## DFM
- Eventos de evaluación: 1107
- Evaluaciones Bull: 1068 | Bear: 211
- Pasaron umbral (PassedThreshold): 916
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:146, 4:192, 5:36, 6:285, 7:302, 8:250, 9:68

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 3453/28113 | KeptCounter: 1775/15071
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.140 | AvgProxCounter≈ 0.059
  - AvgDistATRAligned≈ 2.77 | AvgDistATRCounter≈ 0.75
- PreferAligned eventos: 2069 | Filtradas contra-bias: 291

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 3453/5228 | Counter pre: 1775/5228
- AvgProxAligned(pre)≈ 0.140 | AvgDistATRAligned(pre)≈ 2.77

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=3453 | BaseProx≈ 0.445 | ZoneATR≈ 17.32 | SizePenalty≈ 0.760 | FinalProx≈ 0.341
- Contra-bias: n=1484 | BaseProx≈ 0.460 | ZoneATR≈ 34.90 | SizePenalty≈ 0.567 | FinalProx≈ 0.257

## Risk
- Eventos: 3011
- Accepted=1309 | RejSL=2964 | RejTP=52 | RejRR=612 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1214 | SLDistATR≈ 25.01 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=557 | SLDistATR≈ 28.27 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:590,20-25:325,25+:299
- HistSL Counter 0-10:0,10-15:0,15-20:231,20-25:104,25+:222

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 169 | Unmatched: 1140
- 0-10: Wins=0 Losses=0 WR=0.0% (n=0)
- 10-15: Wins=99 Losses=69 WR=58.9% (n=168)
- 15-20: Wins=0 Losses=1 WR=0.0% (n=1)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=99 Losses=70 WR=58.6% (n=169)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1309 | Aligned=936 (71.5%)
- Core≈ 0.99 | Prox≈ 0.33 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.44 | Confidence≈ 0.00
- SL_TF dist: {'-1': 234, '15': 885, '5': 190} | SL_Structural≈ 82.1%
- TP_TF dist: {'-1': 743, '15': 464, '5': 102} | TP_Structural≈ 43.2%

### SLPick por Bandas y TF
- Bandas: lt8=0, 8-10=0, 10-12.5=638, 12.5-15=671, >15=0
- TF: 5m=190, 15m=885, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 0.00 (n=0), 10-15≈ 1.44 (n=1309)

## CancelBias (EMA200@60m)
- Eventos: 339
- Distribución Bias: {'Bullish': 279, 'Bearish': 60, 'Neutral': 0}
- Coherencia (Close>EMA): 279/339 (82.3%)

## StructureFusion
- Trazas por zona: 43184 | Zonas con Anchors: 42828
- Dir zonas (zona): Bull=27417 Bear=15767 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.6, WithAnchors≈ 8.6, DirBull≈ 5.5, DirBear≈ 3.2, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 41497, 'tie-bias': 1362, 'triggers-only': 325}
- TF Triggers: {'60': 21068, '15': 17288, '5': 4828}
- TF Anchors: {'240': 42796, '1440': 37089}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,47': 2, 'estructura no existe': 3, 'score decayó a 0,49': 1, 'score decayó a 0,42': 1}
- Cancel_BOS (diag): por acción {'BUY': 6, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 6, 'Neutral': 0}

## CSV de Trades
- Filas: 127 | Ejecutadas: 46 | Canceladas: 0 | Expiradas: 0
- BUY: 131 | SELL: 42

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4405 | Total candidatos: 48830 | Seleccionados: 2831
- Candidatos por zona (promedio): 11.1
- **Edad (barras)** - Candidatos: med=58, max=150 | Seleccionados: med=53, max=149
- **Score** - Candidatos: avg=0.41 | Seleccionados: avg=0.46
- **TF Candidatos**: {15: 36513}
- **TF Seleccionados**: {15: 2831}
- **DistATR** - Candidatos: avg=24.0 | Seleccionados: avg=10.5
- **Razones de selección**: {'Fallback<15': 969, 'InBand[10,15]': 1862}
- **En banda [10,15] ATR**: 5068/36513 (13.9%)

### Take Profit (TP)
- Zonas analizadas: 4937 | Total candidatos: 85072 | Seleccionados: 4015
- Candidatos por zona (promedio): 17.2
- **Edad (barras)** - Candidatos: med=18875, max=23284 | Seleccionados: med=6, max=148
- **Score** - Candidatos: avg=0.43 | Seleccionados: avg=0.21
- **Priority Candidatos**: {'P3': 78993}
- **Priority Seleccionados**: {'P4_Fallback': 1910, 'P3': 2105}
- **Type Candidatos**: {'Swing': 78993}
- **Type Seleccionados**: {'Calculated': 1910, 'Swing': 2105}
- **TF Candidatos**: {240: 22504, 1440: 22319, 60: 18166, 15: 16004}
- **TF Seleccionados**: {-1: 1910, 15: 2105}
- **DistATR** - Candidatos: avg=144.1 | Seleccionados: avg=18.3
- **RR** - Candidatos: avg=10.61 | Seleccionados: avg=1.42
- **Razones de selección**: {'NoStructuralTarget': 1910, 'R:R_y_Distancia_OK': 1963, 'Distancia_OK_(R:R_ignorado)': 32, 'R:R_OK_(Distancia_ignorada)': 110}

### 🎯 Recomendaciones
- ⚠️ SL: 66% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 48% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.12.