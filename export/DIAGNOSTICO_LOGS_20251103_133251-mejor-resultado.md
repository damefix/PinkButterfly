# Informe Diagnóstico de Logs - 2025-11-03 13:55:10

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251103_133251.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251103_133251.csv`

## DFM
- Eventos de evaluación: 1520
- Evaluaciones Bull: 1623 | Bear: 347
- Pasaron umbral (PassedThreshold): 637
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:291, 4:763, 5:491, 6:323, 7:102, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 2970/26832 | KeptCounter: 1511/14394
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.128 | AvgProxCounter≈ 0.052
  - AvgDistATRAligned≈ 2.08 | AvgDistATRCounter≈ 0.57
- PreferAligned eventos: 1849 | Filtradas contra-bias: 173

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 2970/4481 | Counter pre: 1511/4481
- AvgProxAligned(pre)≈ 0.128 | AvgDistATRAligned(pre)≈ 2.08

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=2970 | BaseProx≈ 0.446 | ZoneATR≈ 17.20 | SizePenalty≈ 0.771 | FinalProx≈ 0.344
- Contra-bias: n=1338 | BaseProx≈ 0.455 | ZoneATR≈ 31.27 | SizePenalty≈ 0.573 | FinalProx≈ 0.253

## Risk
- Eventos: 2734
- Accepted=2065 | RejSL=1479 | RejTP=52 | RejRR=712 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1007 | SLDistATR≈ 26.27 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=472 | SLDistATR≈ 25.92 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:492,20-25:246,25+:269
- HistSL Counter 0-10:0,10-15:0,15-20:194,20-25:90,25+:188

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 347 | Unmatched: 1718
- 0-10: Wins=38 Losses=56 WR=40.4% (n=94)
- 10-15: Wins=137 Losses=116 WR=54.2% (n=253)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=175 Losses=172 WR=50.4% (n=347)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2065 | Aligned=1356 (65.7%)
- Core≈ 0.99 | Prox≈ 0.34 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.01 | Confidence≈ 0.00
- SL_TF dist: {'-1': 714, '15': 1157, '5': 194} | SL_Structural≈ 65.4%
- TP_TF dist: {'15': 828, '-1': 1074, '5': 163} | TP_Structural≈ 48.0%

### SLPick por Bandas y TF
- Bandas: lt8=684, 8-10=294, 10-12.5=587, 12.5-15=500, >15=0
- TF: 5m=194, 15m=1157, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.66 (n=978), 10-15≈ 1.43 (n=1087)

## CancelBias (EMA200@60m)
- Eventos: 203
- Distribución Bias: {'Bullish': 166, 'Bearish': 37, 'Neutral': 0}
- Coherencia (Close>EMA): 166/203 (81.8%)

## StructureFusion
- Trazas por zona: 41226 | Zonas con Anchors: 40870
- Dir zonas (zona): Bull=25709 Bear=15517 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.2, WithAnchors≈ 8.2, DirBull≈ 5.1, DirBear≈ 3.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 39748, 'tie-bias': 1160, 'triggers-only': 318}
- TF Triggers: {'60': 20252, '15': 16469, '5': 4505}
- TF Anchors: {'240': 40844, '1440': 35217}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 2, 'score decayó a 0,26': 1, 'score decayó a 0,44': 1}
- Cancel_BOS (diag): por acción {'BUY': 6, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 6, 'Neutral': 0}

## CSV de Trades
- Filas: 144 | Ejecutadas: 61 | Canceladas: 0 | Expiradas: 0
- BUY: 176 | SELL: 29

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 3691 | Total candidatos: 33691 | Seleccionados: 2467
- Candidatos por zona (promedio): 9.1
- **Edad (barras)** - Candidatos: med=46, max=150 | Seleccionados: med=47, max=146
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.47
- **TF Candidatos**: {15: 24992}
- **TF Seleccionados**: {15: 2467}
- **DistATR** - Candidatos: avg=20.2 | Seleccionados: avg=10.5
- **Razones de selección**: {'Fallback<15': 868, 'InBand[10,15]': 1599}
- **En banda [10,15] ATR**: 4044/24992 (16.2%)

### Take Profit (TP)
- Zonas analizadas: 4308 | Total candidatos: 62340 | Seleccionados: 3657
- Candidatos por zona (promedio): 14.5
- **Edad (barras)** - Candidatos: med=18593, max=23176 | Seleccionados: med=9, max=150
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.23
- **Priority Candidatos**: {'P3': 58681}
- **Priority Seleccionados**: {'P3': 1960, 'P4_Fallback': 1697}
- **Type Candidatos**: {'Swing': 58681}
- **Type Seleccionados**: {'Swing': 1960, 'Calculated': 1697}
- **TF Candidatos**: {1440: 16862, 240: 16512, 60: 13893, 15: 11414}
- **TF Seleccionados**: {15: 1960, -1: 1697}
- **DistATR** - Candidatos: avg=119.6 | Seleccionados: avg=17.4
- **RR** - Candidatos: avg=9.30 | Seleccionados: avg=1.44
- **Razones de selección**: {'R:R_y_Distancia_OK': 1832, 'NoStructuralTarget': 1697, 'Distancia_OK_(R:R_ignorado)': 39, 'R:R_OK_(Distancia_ignorada)': 89}

### 🎯 Recomendaciones
- ⚠️ SL: 61% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 46% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.11.