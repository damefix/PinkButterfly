# Informe Diagnóstico de Logs - 2025-11-04 18:02:34

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_175922.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_175922.csv`

## DFM
- Eventos de evaluación: 23
- Evaluaciones Bull: 27 | Bear: 9
- Pasaron umbral (PassedThreshold): 28
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:1, 4:1, 5:9, 6:12, 7:13, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5540
- KeptAligned: 2352/6986 | KeptCounter: 579/21533
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.071 | AvgProxCounter≈ 0.037
  - AvgDistATRAligned≈ 0.35 | AvgDistATRCounter≈ 0.33
- PreferAligned eventos: 589 | Filtradas contra-bias: 557

### Proximity (Pre-PreferAligned)
- Eventos: 5540
- Aligned pre: 2352/2931 | Counter pre: 579/2931
- AvgProxAligned(pre)≈ 0.071 | AvgDistATRAligned(pre)≈ 0.35

### Proximity Drivers
- Eventos: 5540
- Alineadas: n=2352 | BaseProx≈ 0.673 | ZoneATR≈ 3.91 | SizePenalty≈ 0.995 | FinalProx≈ 0.670
- Contra-bias: n=22 | BaseProx≈ 0.401 | ZoneATR≈ 5.79 | SizePenalty≈ 0.961 | FinalProx≈ 0.384

## Risk
- Eventos: 596
- Accepted=36 | RejSL=71 | RejTP=1125 | RejRR=1141 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=64 | SLDistATR≈ 17.46 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=7 | SLDistATR≈ 15.95 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:56,20-25:7,25+:1
- HistSL Counter 0-10:0,10-15:0,15-20:7,20-25:0,25+:0

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 6 | Unmatched: 30
- 0-10: Wins=0 Losses=1 WR=0.0% (n=1)
- 10-15: Wins=2 Losses=3 WR=40.0% (n=5)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=2 Losses=4 WR=33.3% (n=6)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 36 | Aligned=32 (88.9%)
- Core≈ 1.00 | Prox≈ 0.64 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.08 | Confidence≈ 0.00
- SL_TF dist: {'60': 23, '-1': 3, '240': 8, '5': 1, '15': 1} | SL_Structural≈ 91.7%
- TP_TF dist: {'-1': 30, '5': 2, '15': 1, '240': 3} | TP_Structural≈ 16.7%

### SLPick por Bandas y TF
- Bandas: lt8=7, 8-10=7, 10-12.5=4, 12.5-15=18, >15=0
- TF: 5m=1, 15m=1, 60m=23, 240m=8, 1440m=0
- RR plan por bandas: 0-10≈ 1.16 (n=14), 10-15≈ 1.03 (n=22)

## CancelBias (EMA200@60m)
- Eventos: 3
- Distribución Bias: {'Bullish': 3, 'Bearish': 0, 'Neutral': 0}
- Coherencia (Close>EMA): 3/3 (100.0%)

## StructureFusion
- Trazas por zona: 28519 | Zonas con Anchors: 28462
- Dir zonas (zona): Bull=14427 Bear=13083 Neutral=1009
- Resumen por ciclo (promedios): TotHZ≈ 5.1, WithAnchors≈ 5.1, DirBull≈ 2.6, DirBear≈ 2.4, DirNeutral≈ 0.2
- Razones de dirección: {'triggers-only': 48, 'tie-bias': 1337, 'anchors+triggers': 27134}
- TF Triggers: {'5': 12057, '15': 16462}
- TF Anchors: {'60': 28383, '240': 27977}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 1792 | Distribución: {'Bullish': 934, 'Bearish': 858, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 934/1792

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura inactiva': 1, 'estructura no existe': 1}
- Cancel_BOS (diag): por acción {'BUY': 2, 'SELL': 3} | por bias {'Bullish': 3, 'Bearish': 2, 'Neutral': 0}

## CSV de Trades
- Filas: 26 | Ejecutadas: 6 | Canceladas: 0 | Expiradas: 0
- BUY: 26 | SELL: 6

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 2370 | Total candidatos: 53028 | Seleccionados: 2368
- Candidatos por zona (promedio): 22.4
- **Edad (barras)** - Candidatos: med=40, max=149 | Seleccionados: med=21, max=93
- **Score** - Candidatos: avg=0.50 | Seleccionados: avg=0.64
- **TF Candidatos**: {60: 23188, 240: 14773, 15: 9019, 5: 6048}
- **TF Seleccionados**: {60: 656, 15: 8, 240: 1701, 5: 3}
- **DistATR** - Candidatos: avg=6.0 | Seleccionados: avg=8.7
- **Razones de selección**: {'Fallback<15': 1681, 'InBand[10,15]': 687}
- **En banda [10,15] ATR**: 8735/53028 (16.5%)

### Take Profit (TP)
- Zonas analizadas: 2374 | Total candidatos: 39717 | Seleccionados: 2374
- Candidatos por zona (promedio): 16.7
- **Edad (barras)** - Candidatos: med=49, max=147 | Seleccionados: med=60, max=127
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.44
- **Priority Candidatos**: {'P3': 39717}
- **Priority Seleccionados**: {'P4_Fallback': 48, 'P3': 2326}
- **Type Candidatos**: {'Swing': 39717}
- **Type Seleccionados**: {'Calculated': 48, 'Swing': 2326}
- **TF Candidatos**: {5: 15798, 240: 11940, 15: 7134, 60: 4845}
- **TF Seleccionados**: {-1: 48, 5: 601, 60: 584, 15: 28, 240: 1113}
- **DistATR** - Candidatos: avg=6.6 | Seleccionados: avg=3.4
- **RR** - Candidatos: avg=0.73 | Seleccionados: avg=0.27
- **Razones de selección**: {'NoStructuralTarget': 48, 'R:R_y_Distancia_OK': 2326}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- KeptAligned ratio≈ 0.34.