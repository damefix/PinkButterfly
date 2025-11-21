# Informe Diagnóstico de Logs - 2025-11-11 10:55:59

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_104912.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_104912.csv`

## DFM
- Eventos de evaluación: 217
- Evaluaciones Bull: 59 | Bear: 192
- Pasaron umbral (PassedThreshold): 62
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:28, 4:3, 5:61, 6:153, 7:6, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2185
- KeptAligned: 1860/1860 | KeptCounter: 1065/1065
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.383 | AvgProxCounter≈ 0.247
  - AvgDistATRAligned≈ 0.53 | AvgDistATRCounter≈ 0.38
- PreferAligned eventos: 932 | Filtradas contra-bias: 332

### Proximity (Pre-PreferAligned)
- Eventos: 2185
- Aligned pre: 1860/2925 | Counter pre: 1065/2925
- AvgProxAligned(pre)≈ 0.383 | AvgDistATRAligned(pre)≈ 0.53

### Proximity Drivers
- Eventos: 2185
- Alineadas: n=1860 | BaseProx≈ 0.918 | ZoneATR≈ 5.10 | SizePenalty≈ 0.977 | FinalProx≈ 0.897
- Contra-bias: n=733 | BaseProx≈ 0.832 | ZoneATR≈ 4.95 | SizePenalty≈ 0.977 | FinalProx≈ 0.814

## Risk
- Eventos: 1356
- Accepted=252 | RejSL=0 | RejTP=0 | RejRR=97 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 39 (9.5% del total)
  - Avg Score: 0.43 | Avg R:R: 1.80 | Avg DistATR: 3.38
  - Por TF: TF5=11, TF15=28
- **P0_SWING_LITE:** 373 (90.5% del total)
  - Avg Score: 0.62 | Avg R:R: 3.69 | Avg DistATR: 3.38
  - Por TF: TF15=90, TF60=283


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 3 | Unmatched: 249
- 0-10: Wins=1 Losses=2 WR=33.3% (n=3)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=1 Losses=2 WR=33.3% (n=3)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 252 | Aligned=170 (67.5%)
- Core≈ 1.00 | Prox≈ 0.89 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.10 | Confidence≈ 0.00
- SL_TF dist: {'15': 200, '5': 52} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 103, '60': 74, '5': 58, '240': 17} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=252, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=52, 15m=200, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.10 (n=252), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 41875 | Zonas con Anchors: 41863
- Dir zonas (zona): Bull=7734 Bear=33002 Neutral=1139
- Resumen por ciclo (promedios): TotHZ≈ 3.6, WithAnchors≈ 3.6, DirBull≈ 1.3, DirBear≈ 2.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 40724, 'tie-bias': 1139, 'triggers-only': 12}
- TF Triggers: {'5': 4985, '15': 3908}
- TF Anchors: {'60': 8845, '240': 5444, '1440': 276}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)

## CSV de Trades
- Filas: 10 | Ejecutadas: 3 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 13

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 62
- Registered: 7
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 7

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 11.3%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 42.9%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 569 | Total candidatos: 10311 | Seleccionados: 0
- Candidatos por zona (promedio): 18.1

### Take Profit (TP)
- Zonas analizadas: 563 | Total candidatos: 4575 | Seleccionados: 0
- Candidatos por zona (promedio): 8.1
- **Edad (barras)** - Candidatos: med=53, max=250 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 4575}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 4575}
- **Type Seleccionados**: {}
- **TF Candidatos**: {60: 1407, 5: 1311, 15: 1218, 240: 639}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=9.1 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=4.04 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.