# Informe Diagnóstico de Logs - 2025-11-07 21:43:57

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251107_214326.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251107_214326.csv`

## DFM
- Eventos de evaluación: 2
- Evaluaciones Bull: 2 | Bear: 0
- Pasaron umbral (PassedThreshold): 2
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:0, 6:1, 7:1, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 106
- KeptAligned: 3/3 | KeptCounter: 203/208
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.019 | AvgProxCounter≈ 0.646
  - AvgDistATRAligned≈ 0.15 | AvgDistATRCounter≈ 2.57
- PreferAligned eventos: 3 | Filtradas contra-bias: 1

### Proximity (Pre-PreferAligned)
- Eventos: 106
- Aligned pre: 3/206 | Counter pre: 203/206
- AvgProxAligned(pre)≈ 0.019 | AvgDistATRAligned(pre)≈ 0.15

### Proximity Drivers
- Eventos: 106
- Alineadas: n=3 | BaseProx≈ 0.671 | ZoneATR≈ 2.44 | SizePenalty≈ 1.000 | FinalProx≈ 0.671
- Contra-bias: n=202 | BaseProx≈ 0.682 | ZoneATR≈ 4.95 | SizePenalty≈ 0.980 | FinalProx≈ 0.668

## Risk
- Eventos: 106
- Accepted=2 | RejSL=0 | RejTP=0 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 0 (0.0%)
- **P4_FALLBACK:** 200 (100.0%)


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 1 | Unmatched: 1
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
- Muestras: 2 | Aligned=0 (0.0%)
- Core≈ 1.00 | Prox≈ 0.49 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.35 | Confidence≈ 0.00
- SL_TF dist: {'240': 2} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 2} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=2, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=0, 15m=0, 60m=0, 240m=2, 1440m=0
- RR plan por bandas: 0-10≈ 1.35 (n=2), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 3855 | Zonas con Anchors: 3854
- Dir zonas (zona): Bull=66 Bear=3756 Neutral=33
- Resumen por ciclo (promedios): TotHZ≈ 1.9, WithAnchors≈ 1.9, DirBull≈ 0.0, DirBear≈ 1.8, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 3822, 'tie-bias': 33}
- TF Triggers: {'15': 150, '5': 61}
- TF Anchors: {'60': 210, '240': 201, '1440': 58}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)

## CSV de Trades
- Filas: 2 | Ejecutadas: 1 | Canceladas: 0 | Expiradas: 0
- BUY: 3 | SELL: 0

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 2
- Registered: 1
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 1
- Intentos de registro: 2

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 100.0%
- RegRate = Registered / Intentos = 50.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 50.0%
- ExecRate = Ejecutadas / Registered = 100.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 202 | Total candidatos: 4074 | Seleccionados: 202
- Candidatos por zona (promedio): 20.2
- **Edad (barras)** - Candidatos: med=64, max=149 | Seleccionados: med=52, max=145
- **Score** - Candidatos: avg=0.38 | Seleccionados: avg=0.46
- **TF Candidatos**: {60: 2224, 15: 1261, 5: 533, 240: 50, 1440: 6}
- **TF Seleccionados**: {60: 159, 240: 2, 1440: 5, 15: 35, 5: 1}
- **DistATR** - Candidatos: avg=6.9 | Seleccionados: avg=6.1
- **Razones de selección**: {'InBand[8,15]_TFPreference': 6, 'Fallback<15': 2, 'InBand[4,8]_TFPreference': 194}
- **En banda [10,15] ATR**: 1078/4074 (26.5%)

### Take Profit (TP)
- Zonas analizadas: 202 | Total candidatos: 797 | Seleccionados: 202
- Candidatos por zona (promedio): 3.9
- **Edad (barras)** - Candidatos: med=25, max=145 | Seleccionados: med=0, max=11
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.01
- **Priority Candidatos**: {'P3': 797}
- **Priority Seleccionados**: {'P4_Fallback': 200, 'P3': 2}
- **Type Candidatos**: {'Swing': 797}
- **Type Seleccionados**: {'Calculated': 200, 'Swing': 2}
- **TF Candidatos**: {15: 323, 5: 302, 60: 106, 240: 64, 1440: 2}
- **TF Seleccionados**: {-1: 200, 60: 2}
- **DistATR** - Candidatos: avg=3.7 | Seleccionados: avg=9.6
- **RR** - Candidatos: avg=0.54 | Seleccionados: avg=1.00
- **Razones de selección**: {'NoStructuralTarget': 200, 'Intradía(15→5→60→240)': 2}

### 🎯 Recomendaciones
- ⚠️ SL: 58% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 99% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 1.00.