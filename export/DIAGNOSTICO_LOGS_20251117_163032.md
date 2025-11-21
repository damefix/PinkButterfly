# Informe Diagnóstico de Logs - 2025-11-17 16:36:55

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_163032.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251117_163032.csv`

## DFM
- Eventos de evaluación: 409
- Evaluaciones Bull: 12 | Bear: 239
- Pasaron umbral (PassedThreshold): 251
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:10, 6:98, 7:96, 8:47, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 22103
- KeptAligned: 1231/1231 | KeptCounter: 1895/2035
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.024 | AvgProxCounter≈ 0.025
  - AvgDistATRAligned≈ 0.10 | AvgDistATRCounter≈ 0.14
- PreferAligned eventos: 629 | Filtradas contra-bias: 536

### Proximity (Pre-PreferAligned)
- Eventos: 22103
- Aligned pre: 1231/3126 | Counter pre: 1895/3126
- AvgProxAligned(pre)≈ 0.024 | AvgDistATRAligned(pre)≈ 0.10

### Proximity Drivers
- Eventos: 22103
- Alineadas: n=1231 | BaseProx≈ 0.737 | ZoneATR≈ 6.66 | SizePenalty≈ 0.948 | FinalProx≈ 0.699
- Contra-bias: n=1359 | BaseProx≈ 0.493 | ZoneATR≈ 7.32 | SizePenalty≈ 0.933 | FinalProx≈ 0.458

## Risk
- Eventos: 1465
- Accepted=540 | RejSL=0 | RejTP=0 | RejRR=264 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 42 (2.9% del total)
  - Avg Score: 0.40 | Avg R:R: 1.66 | Avg DistATR: 4.41
  - Por TF: TF5=27, TF15=15
- **P0_SWING_LITE:** 1398 (97.1% del total)
  - Avg Score: 0.63 | Avg R:R: 3.77 | Avg DistATR: 3.73
  - Por TF: TF15=194, TF60=1204


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 137 | Unmatched: 403
- 0-10: Wins=80 Losses=57 WR=58.4% (n=137)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=80 Losses=57 WR=58.4% (n=137)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 540 | Aligned=196 (36.3%)
- Core≈ 1.00 | Prox≈ 0.57 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.42 | Confidence≈ 0.00
- SL_TF dist: {'5': 205, '15': 304, '1440': 5, '240': 1, '60': 25} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 93, '60': 225, '5': 40, '240': 182} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=540, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=205, 15m=304, 60m=25, 240m=1, 1440m=5
- RR plan por bandas: 0-10≈ 2.42 (n=540), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 80190 | Zonas con Anchors: 80190
- Dir zonas (zona): Bull=17297 Bear=51649 Neutral=11244
- Resumen por ciclo (promedios): TotHZ≈ 3.4, WithAnchors≈ 3.4, DirBull≈ 0.7, DirBear≈ 2.2, DirNeutral≈ 0.5
- Razones de dirección: {'anchors+triggers': 67217, 'tie-bias': 12973}
- TF Triggers: {'5': 66804, '15': 13386}
- TF Anchors: {'60': 80190, '240': 80190, '1440': 80190}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,47': 1}

## CSV de Trades
- Filas: 56 | Ejecutadas: 9 | Canceladas: 0 | Expiradas: 0
- BUY: 4 | SELL: 61

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 251
- Registered: 28
  - DEDUP_COOLDOWN: 62 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 11
- Intentos de registro: 101

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 40.2%
- RegRate = Registered / Intentos = 27.7%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 61.4%
- Concurrency = SKIP_CONCURRENCY / Intentos = 10.9%
- ExecRate = Ejecutadas / Registered = 32.1%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 2329 | Total candidatos: 18211 | Seleccionados: 0
- Candidatos por zona (promedio): 7.8

### Take Profit (TP)
- Zonas analizadas: 2316 | Total candidatos: 37620 | Seleccionados: 2316
- Candidatos por zona (promedio): 16.2
- **Edad (barras)** - Candidatos: med=28, max=93 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.68
- **Priority Candidatos**: {'P3': 2534}
- **Priority Seleccionados**: {'P3': 1813, 'NA': 276, 'P0': 227}
- **Type Candidatos**: {'Swing': 2534}
- **Type Seleccionados**: {'P3_Swing': 1813, 'P4_Fallback': 276, 'P0_Zone': 227}
- **TF Candidatos**: {240: 2022, 60: 388, 15: 81, 5: 43}
- **TF Seleccionados**: {15: 462, -1: 276, 240: 901, 60: 507, 5: 170}
- **DistATR** - Candidatos: avg=10.8 | Seleccionados: avg=12.2
- **RR** - Candidatos: avg=4.19 | Seleccionados: avg=1.63
- **Razones de selección**: {'BestIntelligentScore': 2316}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.