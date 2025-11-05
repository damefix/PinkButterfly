# Informe Diagnóstico de Logs - 2025-11-04 09:25:10

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_091810.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_091810.csv`

## DFM
- Eventos de evaluación: 1621
- Evaluaciones Bull: 2238 | Bear: 173
- Pasaron umbral (PassedThreshold): 895
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:90, 4:991, 5:757, 6:438, 7:135, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5379
- KeptAligned: 6809/43656 | KeptCounter: 1282/12494
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.190 | AvgProxCounter≈ 0.046
  - AvgDistATRAligned≈ 3.37 | AvgDistATRCounter≈ 0.45
- PreferAligned eventos: 3125 | Filtradas contra-bias: 293

### Proximity (Pre-PreferAligned)
- Eventos: 5379
- Aligned pre: 6809/8091 | Counter pre: 1282/8091
- AvgProxAligned(pre)≈ 0.190 | AvgDistATRAligned(pre)≈ 3.37

### Proximity Drivers
- Eventos: 5379
- Alineadas: n=6809 | BaseProx≈ 0.433 | ZoneATR≈ 17.45 | SizePenalty≈ 0.762 | FinalProx≈ 0.328
- Contra-bias: n=989 | BaseProx≈ 0.440 | ZoneATR≈ 27.13 | SizePenalty≈ 0.645 | FinalProx≈ 0.282

## Risk
- Eventos: 3722
- Accepted=2529 | RejSL=3168 | RejTP=126 | RejRR=1975 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=2539 | SLDistATR≈ 23.87 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=629 | SLDistATR≈ 24.05 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:1413,20-25:573,25+:553
- HistSL Counter 0-10:0,10-15:0,15-20:274,20-25:174,25+:181

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 411 | Unmatched: 2118
- 0-10: Wins=33 Losses=66 WR=33.3% (n=99)
- 10-15: Wins=135 Losses=177 WR=43.3% (n=312)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=168 Losses=243 WR=40.9% (n=411)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2529 | Aligned=2268 (89.7%)
- Core≈ 0.99 | Prox≈ 0.34 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.91 | Confidence≈ 0.00
- SL_TF dist: {'-1': 485, '5': 720, '60': 774, '15': 454, '240': 93, '1440': 3} | SL_Structural≈ 80.8%
- TP_TF dist: {'-1': 827, '5': 482, '15': 584, '60': 440, '1440': 44, '240': 152} | TP_Structural≈ 67.3%

### SLPick por Bandas y TF
- Bandas: lt8=702, 8-10=294, 10-12.5=744, 12.5-15=789, >15=0
- TF: 5m=720, 15m=454, 60m=774, 240m=93, 1440m=3
- RR plan por bandas: 0-10≈ 2.42 (n=996), 10-15≈ 1.58 (n=1533)

## CancelBias (EMA200@60m)
- Eventos: 306
- Distribución Bias: {'Bullish': 251, 'Bearish': 55, 'Neutral': 0}
- Coherencia (Close>EMA): 251/306 (82.0%)

## StructureFusion
- Trazas por zona: 56150 | Zonas con Anchors: 55567
- Dir zonas (zona): Bull=31360 Bear=24790 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 10.4, WithAnchors≈ 10.3, DirBull≈ 5.8, DirBear≈ 4.6, DirNeutral≈ 0.0
- Razones de dirección: {'tie-bias': 1231, 'triggers-only': 526, 'anchors+triggers': 54393}
- TF Triggers: {'5': 15228, '15': 19058, '60': 21864}
- TF Anchors: {'240': 55457, '1440': 47304}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5380 | Distribución: {'Bullish': 3736, 'Bearish': 1644, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3736/5380

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,42': 1, 'estructura no existe': 3, 'score decayó a 0,39': 1}
- Cancel_BOS (diag): por acción {'BUY': 1, 'SELL': 0} | por bias {'Bullish': 0, 'Bearish': 1, 'Neutral': 0}

## CSV de Trades
- Filas: 212 | Ejecutadas: 98 | Canceladas: 0 | Expiradas: 0
- BUY: 272 | SELL: 38

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7733 | Total candidatos: 224933 | Seleccionados: 7189
- Candidatos por zona (promedio): 29.1
- **Edad (barras)** - Candidatos: med=38, max=151 | Seleccionados: med=41, max=151
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.52
- **TF Candidatos**: {240: 69314, 60: 59120, 15: 42395, 5: 34358, 1440: 19746}
- **TF Seleccionados**: {5: 2285, 15: 1528, 60: 2620, 1440: 50, 240: 706}
- **DistATR** - Candidatos: avg=64.9 | Seleccionados: avg=11.0
- **Razones de selección**: {'Fallback<15': 1900, 'InBand[10,15]': 5289}
- **En banda [10,15] ATR**: 19184/224933 (8.5%)

### Take Profit (TP)
- Zonas analizadas: 7798 | Total candidatos: 97534 | Seleccionados: 7798
- Candidatos por zona (promedio): 12.5
- **Edad (barras)** - Candidatos: med=36, max=151 | Seleccionados: med=42, max=150
- **Score** - Candidatos: avg=0.50 | Seleccionados: avg=0.37
- **Priority Candidatos**: {'P3': 97534}
- **Priority Seleccionados**: {'P4_Fallback': 1273, 'P3': 6525}
- **Type Candidatos**: {'Swing': 97534}
- **Type Seleccionados**: {'Calculated': 1273, 'Swing': 6525}
- **TF Candidatos**: {60: 24701, 15: 24048, 240: 21425, 5: 15143, 1440: 12217}
- **TF Seleccionados**: {-1: 1273, 5: 1986, 15: 1884, 60: 1670, 1440: 308, 240: 677}
- **DistATR** - Candidatos: avg=53.3 | Seleccionados: avg=17.0
- **RR** - Candidatos: avg=3.73 | Seleccionados: avg=1.16
- **Razones de selección**: {'NoStructuralTarget': 1273, 'R:R_y_Distancia_OK': 6191, 'R:R_OK_(Distancia_ignorada)': 314, 'Distancia_OK_(R:R_ignorado)': 20}

### 🎯 Recomendaciones
- ⚠️ SL: 53% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- KeptAligned ratio≈ 0.16.