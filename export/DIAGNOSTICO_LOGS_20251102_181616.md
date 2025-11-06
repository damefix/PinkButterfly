# Informe Diagnóstico de Logs - 2025-11-02 18:20:22

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251102_181616.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251102_181616.csv`

## DFM
- Eventos de evaluación: 2014
- Evaluaciones Bull: 2213 | Bear: 506
- Pasaron umbral (PassedThreshold): 1909
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:294, 4:444, 5:119, 6:727, 7:596, 8:415, 9:124

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
- Accepted=2778 | RejSL=948 | RejTP=129 | RejRR=1082 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=624 | SLDistATR≈ 32.31 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=324 | SLDistATR≈ 36.13 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:0,20-25:325,25+:299
- HistSL Counter 0-10:0,10-15:0,15-20:0,20-25:102,25+:222

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 224 | Unmatched: 2554
- 0-10: Wins=51 Losses=13 WR=79.7% (n=64)
- 10-15: Wins=76 Losses=50 WR=60.3% (n=126)
- 15-20: Wins=10 Losses=24 WR=29.4% (n=34)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=137 Losses=87 WR=61.2% (n=224)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2778 | Aligned=1889 (68.0%)
- Core≈ 1.00 | Prox≈ 0.32 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.81 | Confidence≈ 0.00
- SL_TF dist: {'-1': 782, '15': 1688, '5': 308} | SL_Structural≈ 71.9%
- TP_TF dist: {'-1': 1498, '15': 1041, '5': 239} | TP_Structural≈ 46.1%

### SLPick por Bandas y TF
- Bandas: lt8=664, 8-10=313, 10-12.5=638, 12.5-15=671, >15=492
- TF: 5m=308, 15m=1688, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.55 (n=977), 10-15≈ 1.44 (n=1309)

## CancelBias (EMA200@60m)
- Eventos: 374
- Distribución Bias: {'Bullish': 265, 'Bearish': 109, 'Neutral': 0}
- Coherencia (Close>EMA): 265/374 (70.9%)

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
- Expiraciones: {'estructura no existe': 3, 'score decayó a 0,47': 1, 'score decayó a 0,33': 1, 'score decayó a 0,26': 1, 'score decayó a 0,42': 1, 'score decayó a 0,21': 1}
- Cancel_BOS (diag): por acción {'BUY': 14, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 14, 'Neutral': 0}

## CSV de Trades
- Filas: 177 | Ejecutadas: 59 | Canceladas: 0 | Expiradas: 0
- BUY: 169 | SELL: 67

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
- **Edad (barras)** - Candidatos: med=18786, max=23193 | Seleccionados: med=6, max=148
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