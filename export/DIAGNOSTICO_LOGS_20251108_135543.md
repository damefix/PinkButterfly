# Informe Diagnóstico de Logs - 2025-11-08 13:56:04

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251108_135543.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251108_135543.csv`

## DFM
- Eventos de evaluación: 3
- Evaluaciones Bull: 3 | Bear: 0
- Pasaron umbral (PassedThreshold): 3
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:0, 6:3, 7:0, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 111
- KeptAligned: 1/1 | KeptCounter: 200/201
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.007 | AvgProxCounter≈ 0.666
  - AvgDistATRAligned≈ 0.03 | AvgDistATRCounter≈ 2.52
- PreferAligned eventos: 1 | Filtradas contra-bias: 0

### Proximity (Pre-PreferAligned)
- Eventos: 111
- Aligned pre: 1/201 | Counter pre: 200/201
- AvgProxAligned(pre)≈ 0.007 | AvgDistATRAligned(pre)≈ 0.03

### Proximity Drivers
- Eventos: 111
- Alineadas: n=1 | BaseProx≈ 0.786 | ZoneATR≈ 3.45 | SizePenalty≈ 1.000 | FinalProx≈ 0.786
- Contra-bias: n=200 | BaseProx≈ 0.687 | ZoneATR≈ 4.47 | SizePenalty≈ 0.983 | FinalProx≈ 0.676

## Risk
- Eventos: 111
- Accepted=3 | RejSL=0 | RejTP=0 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 0 (0.0%)
- **P4_FALLBACK:** 197 (100.0%)


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 1 | Unmatched: 2
- 0-10: Wins=0 Losses=1 WR=0.0% (n=1)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=0 Losses=1 WR=0.0% (n=1)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 3 | Aligned=0 (0.0%)
- Core≈ 1.00 | Prox≈ 0.48 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.34 | Confidence≈ 0.00
- SL_TF dist: {'240': 3} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 1, '60': 2} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=3, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=0, 15m=0, 60m=0, 240m=3, 1440m=0
- RR plan por bandas: 0-10≈ 1.34 (n=3), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 2744 | Zonas con Anchors: 2744
- Dir zonas (zona): Bull=3 Bear=2740 Neutral=1
- Resumen por ciclo (promedios): TotHZ≈ 1.7, WithAnchors≈ 1.7, DirBull≈ 0.0, DirBear≈ 1.7, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 2743, 'tie-bias': 1}
- TF Triggers: {'15': 175, '5': 27}
- TF Anchors: {'60': 202, '240': 200, '1440': 105}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)

## CSV de Trades
- Filas: 2 | Ejecutadas: 1 | Canceladas: 0 | Expiradas: 0
- BUY: 3 | SELL: 0

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 3
- Registered: 1
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 2
- Intentos de registro: 3

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 100.0%
- RegRate = Registered / Intentos = 33.3%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 66.7%
- ExecRate = Ejecutadas / Registered = 100.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 200 | Total candidatos: 3608 | Seleccionados: 200
- Candidatos por zona (promedio): 18.0
- **Edad (barras)** - Candidatos: med=64, max=150 | Seleccionados: med=65, max=148
- **Score** - Candidatos: avg=0.38 | Seleccionados: avg=0.41
- **TF Candidatos**: {60: 2158, 15: 1283, 5: 101, 240: 59, 1440: 7}
- **TF Seleccionados**: {60: 138, 1440: 6, 240: 3, 15: 51, 5: 2}
- **DistATR** - Candidatos: avg=6.7 | Seleccionados: avg=6.2
- **Razones de selección**: {'InBand[8,15]_TFPreference': 7, 'Fallback<15': 3, 'InBand[4,8]_TFPreference': 190}
- **En banda [10,15] ATR**: 954/3608 (26.4%)

### Take Profit (TP)
- Zonas analizadas: 200 | Total candidatos: 1083 | Seleccionados: 200
- Candidatos por zona (promedio): 5.4
- **Edad (barras)** - Candidatos: med=26, max=118 | Seleccionados: med=0, max=34
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.01
- **Priority Candidatos**: {'P3': 1083}
- **Priority Seleccionados**: {'P4_Fallback': 197, 'P3': 3}
- **Type Candidatos**: {'Swing': 1083}
- **Type Seleccionados**: {'Calculated': 197, 'Swing': 3}
- **TF Candidatos**: {15: 521, 5: 266, 60: 189, 240: 104, 1440: 3}
- **TF Seleccionados**: {-1: 197, 15: 1, 60: 2}
- **DistATR** - Candidatos: avg=4.3 | Seleccionados: avg=9.0
- **RR** - Candidatos: avg=0.67 | Seleccionados: avg=1.01
- **Razones de selección**: {'NoStructuralTarget': 197, 'Intradía(15→5→60→240)': 3}

### 🎯 Recomendaciones
- ⚠️ SL: 64% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 98% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 1.00.