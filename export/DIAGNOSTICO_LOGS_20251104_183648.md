# Informe Diagnóstico de Logs - 2025-11-04 18:39:43

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_183648.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_183648.csv`

## DFM
- Eventos de evaluación: 628
- Evaluaciones Bull: 84 | Bear: 2933
- Pasaron umbral (PassedThreshold): 2985
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:6, 4:8, 5:982, 6:834, 7:1187, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5578
- KeptAligned: 3079/9448 | KeptCounter: 1195/22188
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.071 | AvgProxCounter≈ 0.063
  - AvgDistATRAligned≈ 0.42 | AvgDistATRCounter≈ 0.23
- PreferAligned eventos: 626 | Filtradas contra-bias: 1173

### Proximity (Pre-PreferAligned)
- Eventos: 5578
- Aligned pre: 3079/4274 | Counter pre: 1195/4274
- AvgProxAligned(pre)≈ 0.071 | AvgDistATRAligned(pre)≈ 0.42

### Proximity Drivers
- Eventos: 5578
- Alineadas: n=3079 | BaseProx≈ 0.637 | ZoneATR≈ 2.22 | SizePenalty≈ 0.998 | FinalProx≈ 0.636
- Contra-bias: n=22 | BaseProx≈ 0.401 | ZoneATR≈ 5.79 | SizePenalty≈ 0.961 | FinalProx≈ 0.384

## Risk
- Eventos: 633
- Accepted=3017 | RejSL=72 | RejTP=0 | RejRR=0 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=65 | SLDistATR≈ 17.46 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=7 | SLDistATR≈ 15.95 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:57,20-25:7,25+:1
- HistSL Counter 0-10:0,10-15:0,15-20:7,20-25:0,25+:0

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 8 | Unmatched: 3009
- 0-10: Wins=0 Losses=2 WR=0.0% (n=2)
- 10-15: Wins=3 Losses=3 WR=50.0% (n=6)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=3 Losses=5 WR=37.5% (n=8)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 3017 | Aligned=3002 (99.5%)
- Core≈ 1.00 | Prox≈ 0.64 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.72 | Confidence≈ 0.00
- SL_TF dist: {'60': 75, '-1': 5, '15': 2, '240': 2932, '5': 3} | SL_Structural≈ 99.8%
- TP_TF dist: {'-1': 85, '240': 2932} | TP_Structural≈ 97.2%

### SLPick por Bandas y TF
- Bandas: lt8=597, 8-10=1755, 10-12.5=613, 12.5-15=52, >15=0
- TF: 5m=3, 15m=2, 60m=75, 240m=2932, 1440m=0
- RR plan por bandas: 0-10≈ 1.74 (n=2352), 10-15≈ 1.65 (n=665)

## CancelBias (EMA200@60m)
- Eventos: 21
- Distribución Bias: {'Bullish': 13, 'Bearish': 8, 'Neutral': 0}
- Coherencia (Close>EMA): 13/21 (61.9%)

## StructureFusion
- Trazas por zona: 31636 | Zonas con Anchors: 31588
- Dir zonas (zona): Bull=15079 Bear=15552 Neutral=1005
- Resumen por ciclo (promedios): TotHZ≈ 5.7, WithAnchors≈ 5.7, DirBull≈ 2.7, DirBear≈ 2.8, DirNeutral≈ 0.2
- Razones de dirección: {'triggers-only': 46, 'tie-bias': 1331, 'anchors+triggers': 30259}
- TF Triggers: {'5': 13296, '15': 18340}
- TF Anchors: {'60': 31509, '240': 31111}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 1829 | Distribución: {'Bullish': 934, 'Bearish': 895, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 934/1829

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,42': 1, 'score decayó a 0,47': 1, 'score decayó a 0,26': 1, 'estructura no existe': 2, 'score decayó a 0,29': 1}
- Cancel_BOS (diag): por acción {'BUY': 3, 'SELL': 3} | por bias {'Bullish': 3, 'Bearish': 3, 'Neutral': 0}

## CSV de Trades
- Filas: 43 | Ejecutadas: 8 | Canceladas: 0 | Expiradas: 0
- BUY: 34 | SELL: 17

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 3097 | Total candidatos: 59795 | Seleccionados: 3095
- Candidatos por zona (promedio): 19.3
- **Edad (barras)** - Candidatos: med=34, max=149 | Seleccionados: med=21, max=99
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.67
- **TF Candidatos**: {60: 31616, 240: 19135, 15: 8416, 5: 628}
- **TF Seleccionados**: {60: 132, 15: 9, 240: 2951, 5: 3}
- **DistATR** - Candidatos: avg=4.6 | Seleccionados: avg=7.8
- **Razones de selección**: {'Fallback<15': 2950, 'InBand[10,15]_TFPreference': 145}
- **En banda [10,15] ATR**: 605/59795 (1.0%)

### Take Profit (TP)
- Zonas analizadas: 3101 | Total candidatos: 74115 | Seleccionados: 3101
- Candidatos por zona (promedio): 23.9
- **Edad (barras)** - Candidatos: med=74, max=2147483647 | Seleccionados: med=54, max=108
- **Score** - Candidatos: avg=0.53 | Seleccionados: avg=0.25
- **Priority Candidatos**: {'P3': 74115}
- **Priority Seleccionados**: {'P4_Fallback': 144, 'P3': 2957}
- **Type Candidatos**: {'Swing': 74115}
- **Type Seleccionados**: {'Calculated': 144, 'Swing': 2957}
- **TF Candidatos**: {5: 34783, 240: 17349, 15: 13947, 60: 8036}
- **TF Seleccionados**: {-1: 144, 240: 2957}
- **DistATR** - Candidatos: avg=4.7 | Seleccionados: avg=15.7
- **RR** - Candidatos: avg=0.55 | Seleccionados: avg=1.70
- **Razones de selección**: {'NoStructuralTarget': 144, 'SwingP3_TF>=60_RR>=Min_Dist>=12': 2957}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- KeptAligned ratio≈ 0.33.