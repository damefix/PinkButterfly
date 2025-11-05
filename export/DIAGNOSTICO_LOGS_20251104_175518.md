# Informe Diagnóstico de Logs - 2025-11-04 17:58:54

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_175518.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_175518.csv`

## DFM
- Eventos de evaluación: 23
- Evaluaciones Bull: 27 | Bear: 9
- Pasaron umbral (PassedThreshold): 28
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:1, 4:1, 5:9, 6:12, 7:13, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 6656
- KeptAligned: 8474/13109 | KeptCounter: 1695/23765
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.182 | AvgProxCounter≈ 0.115
  - AvgDistATRAligned≈ 0.75 | AvgDistATRCounter≈ 0.70
- PreferAligned eventos: 1705 | Filtradas contra-bias: 1673

### Proximity (Pre-PreferAligned)
- Eventos: 6656
- Aligned pre: 8474/10169 | Counter pre: 1695/10169
- AvgProxAligned(pre)≈ 0.182 | AvgDistATRAligned(pre)≈ 0.75

### Proximity Drivers
- Eventos: 6656
- Alineadas: n=8474 | BaseProx≈ 0.714 | ZoneATR≈ 3.48 | SizePenalty≈ 0.997 | FinalProx≈ 0.712
- Contra-bias: n=22 | BaseProx≈ 0.401 | ZoneATR≈ 5.79 | SizePenalty≈ 0.961 | FinalProx≈ 0.384

## Risk
- Eventos: 1712
- Accepted=36 | RejSL=71 | RejTP=5015 | RejRR=3373 | RejEntry=0
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
- Trazas por zona: 36874 | Zonas con Anchors: 36817
- Dir zonas (zona): Bull=16659 Bear=19206 Neutral=1009
- Resumen por ciclo (promedios): TotHZ≈ 5.5, WithAnchors≈ 5.5, DirBull≈ 2.5, DirBear≈ 2.9, DirNeutral≈ 0.2
- Razones de dirección: {'triggers-only': 48, 'tie-bias': 1337, 'anchors+triggers': 35489}
- TF Triggers: {'5': 14289, '15': 22585}
- TF Anchors: {'60': 36738, '240': 36332}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 2908 | Distribución: {'Bullish': 934, 'Bearish': 1974, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 934/2908

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura inactiva': 1, 'estructura no existe': 1}
- Cancel_BOS (diag): por acción {'BUY': 2, 'SELL': 3} | por bias {'Bullish': 3, 'Bearish': 2, 'Neutral': 0}

## CSV de Trades
- Filas: 26 | Ejecutadas: 6 | Canceladas: 0 | Expiradas: 0
- BUY: 26 | SELL: 6

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 8492 | Total candidatos: 212040 | Seleccionados: 8490
- Candidatos por zona (promedio): 25.0
- **Edad (barras)** - Candidatos: med=40, max=149 | Seleccionados: med=21, max=93
- **Score** - Candidatos: avg=0.50 | Seleccionados: avg=0.68
- **TF Candidatos**: {60: 84408, 240: 51505, 15: 40681, 5: 35446}
- **TF Seleccionados**: {60: 114, 15: 8, 240: 8365, 5: 3}
- **DistATR** - Candidatos: avg=5.5 | Seleccionados: avg=9.3
- **Razones de selección**: {'Fallback<15': 5029, 'InBand[10,15]': 3461}
- **En banda [10,15] ATR**: 25475/212040 (12.0%)

### Take Profit (TP)
- Zonas analizadas: 8496 | Total candidatos: 126669 | Seleccionados: 8496
- Candidatos por zona (promedio): 14.9
- **Edad (barras)** - Candidatos: med=49, max=147 | Seleccionados: med=60, max=127
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.46
- **Priority Candidatos**: {'P3': 126669}
- **Priority Seleccionados**: {'P4_Fallback': 48, 'P3': 8448}
- **Type Candidatos**: {'Swing': 126669}
- **Type Seleccionados**: {'Calculated': 48, 'Swing': 8448}
- **TF Candidatos**: {5: 47046, 240: 43666, 15: 22184, 60: 13773}
- **TF Seleccionados**: {-1: 48, 5: 1717, 60: 42, 15: 3344, 240: 3345}
- **DistATR** - Candidatos: avg=6.7 | Seleccionados: avg=3.2
- **RR** - Candidatos: avg=0.73 | Seleccionados: avg=0.26
- **Razones de selección**: {'NoStructuralTarget': 48, 'R:R_y_Distancia_OK': 8448}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- KeptAligned ratio≈ 0.65.