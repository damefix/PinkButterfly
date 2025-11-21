# Informe Diagnóstico de Logs - 2025-11-11 18:09:44

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_172424.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_172424.csv`

## DFM
- Eventos de evaluación: 32
- Evaluaciones Bull: 9 | Bear: 23
- Pasaron umbral (PassedThreshold): 28
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:3, 4:1, 5:7, 6:16, 7:5, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2208
- KeptAligned: 931/931 | KeptCounter: 537/537
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.263 | AvgProxCounter≈ 0.155
  - AvgDistATRAligned≈ 0.20 | AvgDistATRCounter≈ 0.10
- PreferAligned eventos: 638 | Filtradas contra-bias: 102

### Proximity (Pre-PreferAligned)
- Eventos: 2208
- Aligned pre: 931/1468 | Counter pre: 537/1468
- AvgProxAligned(pre)≈ 0.263 | AvgDistATRAligned(pre)≈ 0.20

### Proximity Drivers
- Eventos: 2208
- Alineadas: n=931 | BaseProx≈ 0.930 | ZoneATR≈ 5.05 | SizePenalty≈ 0.977 | FinalProx≈ 0.909
- Contra-bias: n=435 | BaseProx≈ 0.888 | ZoneATR≈ 4.64 | SizePenalty≈ 0.979 | FinalProx≈ 0.870

## Risk
- Eventos: 950
- Accepted=32 | RejSL=0 | RejTP=0 | RejRR=38 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 3 (3.6% del total)
  - Avg Score: 0.43 | Avg R:R: 2.02 | Avg DistATR: 2.74
  - Por TF: TF5=1, TF15=2
- **P0_SWING_LITE:** 81 (96.4% del total)
  - Avg Score: 0.54 | Avg R:R: 4.46 | Avg DistATR: 3.44
  - Por TF: TF15=16, TF60=65


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 9 | Unmatched: 23
- 0-10: Wins=0 Losses=9 WR=0.0% (n=9)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=0 Losses=9 WR=0.0% (n=9)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 32 | Aligned=22 (68.8%)
- Core≈ 1.00 | Prox≈ 0.91 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.89 | Confidence≈ 0.00
- SL_TF dist: {'15': 29, '5': 3} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 18, '5': 11, '240': 1, '60': 2} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=32, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=3, 15m=29, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.89 (n=32), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 39569 | Zonas con Anchors: 39555
- Dir zonas (zona): Bull=8493 Bear=30130 Neutral=946
- Resumen por ciclo (promedios): TotHZ≈ 3.7, WithAnchors≈ 3.7, DirBull≈ 1.4, DirBear≈ 2.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 38426, 'tie-bias': 1129, 'triggers-only': 14}
- TF Triggers: {'5': 4984, '15': 4265}
- TF Anchors: {'60': 9191, '240': 5542, '1440': 502}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)

## CSV de Trades
- Filas: 24 | Ejecutadas: 9 | Canceladas: 0 | Expiradas: 0
- BUY: 12 | SELL: 21

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 28
- Registered: 13
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 3 | SKIP_CONCURRENCY: 0
- Intentos de registro: 16

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 57.1%
- RegRate = Registered / Intentos = 81.2%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 18.8%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 69.2%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 110 | Total candidatos: 1746 | Seleccionados: 0
- Candidatos por zona (promedio): 15.9

### Take Profit (TP)
- Zonas analizadas: 106 | Total candidatos: 758 | Seleccionados: 106
- Candidatos por zona (promedio): 7.2
- **Edad (barras)** - Candidatos: med=50, max=180 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.39 | Seleccionados: avg=0.71
- **Priority Candidatos**: {'P3': 758}
- **Priority Seleccionados**: {'P3': 33, 'NA': 59, 'P0': 14}
- **Type Candidatos**: {'Swing': 758}
- **Type Seleccionados**: {'P3_Swing': 33, 'P4_Fallback': 59, 'P0_Zone': 14}
- **TF Candidatos**: {60: 224, 5: 223, 15: 190, 240: 121}
- **TF Seleccionados**: {15: 24, -1: 59, 5: 13, 60: 3, 240: 7}
- **DistATR** - Candidatos: avg=8.4 | Seleccionados: avg=3.1
- **RR** - Candidatos: avg=5.00 | Seleccionados: avg=1.32
- **Razones de selección**: {'BestIntelligentScore': 106}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.