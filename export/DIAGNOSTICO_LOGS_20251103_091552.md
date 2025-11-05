# Informe Diagnóstico de Logs - 2025-11-03 09:20:29

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251103_091552.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251103_091552.csv`

## DFM
- Eventos de evaluación: 1505
- Evaluaciones Bull: 1616 | Bear: 337
- Pasaron umbral (PassedThreshold): 694
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:213, 4:740, 5:534, 6:358, 7:108, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 2947/26821 | KeptCounter: 1509/14406
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.127 | AvgProxCounter≈ 0.052
  - AvgDistATRAligned≈ 2.08 | AvgDistATRCounter≈ 0.57
- PreferAligned eventos: 1849 | Filtradas contra-bias: 174

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 2947/4456 | Counter pre: 1509/4456
- AvgProxAligned(pre)≈ 0.127 | AvgDistATRAligned(pre)≈ 2.08

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=2947 | BaseProx≈ 0.446 | ZoneATR≈ 17.19 | SizePenalty≈ 0.771 | FinalProx≈ 0.344
- Contra-bias: n=1335 | BaseProx≈ 0.455 | ZoneATR≈ 31.27 | SizePenalty≈ 0.573 | FinalProx≈ 0.253

## Risk
- Eventos: 2730
- Accepted=2044 | RejSL=1470 | RejTP=52 | RejRR=716 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=996 | SLDistATR≈ 26.34 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=474 | SLDistATR≈ 25.88 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:483,20-25:245,25+:268
- HistSL Counter 0-10:0,10-15:0,15-20:195,20-25:91,25+:188

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 337 | Unmatched: 1707
- 0-10: Wins=38 Losses=57 WR=40.0% (n=95)
- 10-15: Wins=133 Losses=109 WR=55.0% (n=242)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=171 Losses=166 WR=50.7% (n=337)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2044 | Aligned=1339 (65.5%)
- Core≈ 1.00 | Prox≈ 0.34 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.00 | Confidence≈ 0.00
- SL_TF dist: {'-1': 710, '15': 1144, '5': 190} | SL_Structural≈ 65.3%
- TP_TF dist: {'15': 810, '-1': 1073, '5': 161} | TP_Structural≈ 47.5%

### SLPick por Bandas y TF
- Bandas: lt8=677, 8-10=294, 10-12.5=579, 12.5-15=494, >15=0
- TF: 5m=190, 15m=1144, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.64 (n=971), 10-15≈ 1.42 (n=1073)

## CancelBias (EMA200@60m)
- Eventos: 250
- Distribución Bias: {'Bullish': 209, 'Bearish': 41, 'Neutral': 0}
- Coherencia (Close>EMA): 209/250 (83.6%)

## StructureFusion
- Trazas por zona: 41227 | Zonas con Anchors: 40871
- Dir zonas (zona): Bull=25711 Bear=15516 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.2, WithAnchors≈ 8.2, DirBull≈ 5.1, DirBear≈ 3.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 39755, 'tie-bias': 1154, 'triggers-only': 318}
- TF Triggers: {'60': 20252, '15': 16472, '5': 4503}
- TF Anchors: {'240': 40845, '1440': 35219}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 2, 'score decayó a 0,44': 1}
- Cancel_BOS (diag): por acción {'BUY': 7, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 7, 'Neutral': 0}

## CSV de Trades
- Filas: 148 | Ejecutadas: 62 | Canceladas: 0 | Expiradas: 0
- BUY: 181 | SELL: 29

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 3669 | Total candidatos: 33666 | Seleccionados: 2447
- Candidatos por zona (promedio): 9.2
- **Edad (barras)** - Candidatos: med=46, max=150 | Seleccionados: med=48, max=146
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.47
- **TF Candidatos**: {15: 24990}
- **TF Seleccionados**: {15: 2447}
- **DistATR** - Candidatos: avg=20.2 | Seleccionados: avg=10.5
- **Razones de selección**: {'Fallback<15': 875, 'InBand[10,15]': 1572}
- **En banda [10,15] ATR**: 4013/24990 (16.1%)

### Take Profit (TP)
- Zonas analizadas: 4282 | Total candidatos: 61721 | Seleccionados: 3640
- Candidatos por zona (promedio): 14.4
- **Edad (barras)** - Candidatos: med=18563, max=23176 | Seleccionados: med=9, max=150
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.23
- **Priority Candidatos**: {'P3': 58126}
- **Priority Seleccionados**: {'P3': 1944, 'P4_Fallback': 1696}
- **Type Candidatos**: {'Swing': 58126}
- **Type Seleccionados**: {'Swing': 1944, 'Calculated': 1696}
- **TF Candidatos**: {1440: 16628, 240: 16323, 60: 13805, 15: 11370}
- **TF Seleccionados**: {15: 1944, -1: 1696}
- **DistATR** - Candidatos: avg=119.2 | Seleccionados: avg=17.4
- **RR** - Candidatos: avg=9.26 | Seleccionados: avg=1.43
- **Razones de selección**: {'R:R_y_Distancia_OK': 1817, 'NoStructuralTarget': 1696, 'Distancia_OK_(R:R_ignorado)': 38, 'R:R_OK_(Distancia_ignorada)': 89}

### 🎯 Recomendaciones
- ⚠️ SL: 61% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 47% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.11.