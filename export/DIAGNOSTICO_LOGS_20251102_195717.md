# Informe Diagnóstico de Logs - 2025-11-02 20:00:09

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251102_195717.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251102_195717.csv`

## DFM
- Eventos de evaluación: 1682
- Evaluaciones Bull: 1868 | Bear: 381
- Pasaron umbral (PassedThreshold): 1569
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:238, 4:374, 5:122, 6:498, 7:489, 8:388, 9:140

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 3554/27032 | KeptCounter: 1770/14324
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.145 | AvgProxCounter≈ 0.059
  - AvgDistATRAligned≈ 2.83 | AvgDistATRCounter≈ 0.74
- PreferAligned eventos: 2114 | Filtradas contra-bias: 262

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 3554/5324 | Counter pre: 1770/5324
- AvgProxAligned(pre)≈ 0.145 | AvgDistATRAligned(pre)≈ 2.83

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=3554 | BaseProx≈ 0.447 | ZoneATR≈ 17.30 | SizePenalty≈ 0.771 | FinalProx≈ 0.347
- Contra-bias: n=1508 | BaseProx≈ 0.457 | ZoneATR≈ 32.04 | SizePenalty≈ 0.567 | FinalProx≈ 0.257

## Risk
- Eventos: 3055
- Accepted=2317 | RejSL=1861 | RejTP=57 | RejRR=827 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1286 | SLDistATR≈ 26.00 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=575 | SLDistATR≈ 26.56 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:601,20-25:347,25+:338
- HistSL Counter 0-10:0,10-15:0,15-20:260,20-25:90,25+:225

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 238 | Unmatched: 2079
- 0-10: Wins=19 Losses=70 WR=21.3% (n=89)
- 10-15: Wins=79 Losses=70 WR=53.0% (n=149)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=98 Losses=140 WR=41.2% (n=238)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2317 | Aligned=1555 (67.1%)
- Core≈ 0.99 | Prox≈ 0.35 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.99 | Confidence≈ 0.00
- SL_TF dist: {'-1': 827, '15': 1280, '5': 210} | SL_Structural≈ 64.3%
- TP_TF dist: {'15': 886, '-1': 1242, '5': 189} | TP_Structural≈ 46.4%

### SLPick por Bandas y TF
- Bandas: lt8=724, 8-10=363, 10-12.5=642, 12.5-15=588, >15=0
- TF: 5m=210, 15m=1280, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.63 (n=1087), 10-15≈ 1.42 (n=1230)

## CancelBias (EMA200@60m)
- Eventos: 553
- Distribución Bias: {'Bullish': 438, 'Bearish': 115, 'Neutral': 0}
- Coherencia (Close>EMA): 438/553 (79.2%)

## StructureFusion
- Trazas por zona: 41356 | Zonas con Anchors: 40991
- Dir zonas (zona): Bull=25948 Bear=15408 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.3, WithAnchors≈ 8.2, DirBull≈ 5.2, DirBear≈ 3.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 39858, 'tie-bias': 1174, 'triggers-only': 324}
- TF Triggers: {'60': 20156, '15': 16613, '5': 4587}
- TF Anchors: {'240': 40965, '1440': 35346}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 5, 'score decayó a 0,47': 2, 'score decayó a 0,25': 2, 'score decayó a 0,33': 1, 'score decayó a 0,48': 1}
- Cancel_BOS (diag): por acción {'BUY': 20, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 20, 'Neutral': 0}

## CSV de Trades
- Filas: 192 | Ejecutadas: 59 | Canceladas: 0 | Expiradas: 0
- BUY: 185 | SELL: 66

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4339 | Total candidatos: 37914 | Seleccionados: 2912
- Candidatos por zona (promedio): 8.7
- **Edad (barras)** - Candidatos: med=45, max=100 | Seleccionados: med=46, max=100
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.49
- **TF Candidatos**: {15: 28351}
- **TF Seleccionados**: {15: 2912}
- **DistATR** - Candidatos: avg=20.8 | Seleccionados: avg=10.4
- **Razones de selección**: {'Fallback<15': 1072, 'InBand[10,15]': 1840}
- **En banda [10,15] ATR**: 4541/28351 (16.0%)

### Take Profit (TP)
- Zonas analizadas: 5062 | Total candidatos: 72572 | Seleccionados: 4260
- Candidatos por zona (promedio): 14.3
- **Edad (barras)** - Candidatos: med=18671, max=23176 | Seleccionados: med=6, max=100
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.23
- **Priority Candidatos**: {'P3': 68151}
- **Priority Seleccionados**: {'P3': 2223, 'P4_Fallback': 2037}
- **Type Candidatos**: {'Swing': 68151}
- **Type Seleccionados**: {'Swing': 2223, 'Calculated': 2037}
- **TF Candidatos**: {1440: 19927, 240: 19046, 60: 15858, 15: 13320}
- **TF Seleccionados**: {15: 2223, -1: 2037}
- **DistATR** - Candidatos: avg=120.0 | Seleccionados: avg=17.6
- **RR** - Candidatos: avg=9.07 | Seleccionados: avg=1.41
- **Razones de selección**: {'R:R_y_Distancia_OK': 2096, 'NoStructuralTarget': 2037, 'Distancia_OK_(R:R_ignorado)': 34, 'R:R_OK_(Distancia_ignorada)': 93}

### 🎯 Recomendaciones
- ⚠️ SL: 58% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 48% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.13.