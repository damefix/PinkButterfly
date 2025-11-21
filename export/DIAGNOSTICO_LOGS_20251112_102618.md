# Informe Diagnóstico de Logs - 2025-11-12 10:29:40

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_102618.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251112_102618.csv`

## DFM
- Eventos de evaluación: 157
- Evaluaciones Bull: 58 | Bear: 114
- Pasaron umbral (PassedThreshold): 162
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:1, 4:9, 5:18, 6:81, 7:53, 8:10, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 1785
- KeptAligned: 1206/1206 | KeptCounter: 503/503
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.349 | AvgProxCounter≈ 0.153
  - AvgDistATRAligned≈ 0.54 | AvgDistATRCounter≈ 0.24
- PreferAligned eventos: 626 | Filtradas contra-bias: 49

### Proximity (Pre-PreferAligned)
- Eventos: 1785
- Aligned pre: 1206/1709 | Counter pre: 503/1709
- AvgProxAligned(pre)≈ 0.349 | AvgDistATRAligned(pre)≈ 0.54

### Proximity Drivers
- Eventos: 1785
- Alineadas: n=1206 | BaseProx≈ 0.871 | ZoneATR≈ 4.86 | SizePenalty≈ 0.980 | FinalProx≈ 0.854
- Contra-bias: n=454 | BaseProx≈ 0.761 | ZoneATR≈ 4.29 | SizePenalty≈ 0.984 | FinalProx≈ 0.750

## Risk
- Eventos: 981
- Accepted=172 | RejSL=0 | RejTP=0 | RejRR=144 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 32 (8.5% del total)
  - Avg Score: 0.41 | Avg R:R: 2.04 | Avg DistATR: 3.83
  - Por TF: TF5=8, TF15=24
- **P0_SWING_LITE:** 344 (91.5% del total)
  - Avg Score: 0.52 | Avg R:R: 3.68 | Avg DistATR: 3.32
  - Por TF: TF15=68, TF60=276


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 49 | Unmatched: 124
- 0-10: Wins=17 Losses=32 WR=34.7% (n=49)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=17 Losses=32 WR=34.7% (n=49)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 173 | Aligned=105 (60.7%)
- Core≈ 0.99 | Prox≈ 0.81 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.71 | Confidence≈ 0.00
- SL_TF dist: {'60': 8, '15': 127, '5': 38} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 86, '60': 47, '5': 31, '240': 9} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=172, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=37, 15m=127, 60m=8, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.71 (n=172), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 8492 | Zonas con Anchors: 8375
- Dir zonas (zona): Bull=3421 Bear=4755 Neutral=316
- Resumen por ciclo (promedios): TotHZ≈ 1.7, WithAnchors≈ 1.7, DirBull≈ 0.6, DirBear≈ 1.0, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 8019, 'tie-bias': 358, 'triggers-only': 115}
- TF Triggers: {'5': 2835, '15': 1515}
- TF Anchors: {'60': 4246, '240': 2374, '1440': 31}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 2, 'score decayó a 0,33': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 91 | Ejecutadas: 33 | Canceladas: 0 | Expiradas: 0
- BUY: 54 | SELL: 70

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 162
- Registered: 47
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 19 | SKIP_CONCURRENCY: 5
- Intentos de registro: 71

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 43.8%
- RegRate = Registered / Intentos = 66.2%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 26.8%
- Concurrency = SKIP_CONCURRENCY / Intentos = 7.0%
- ExecRate = Ejecutadas / Registered = 70.2%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 517 | Total candidatos: 8796 | Seleccionados: 0
- Candidatos por zona (promedio): 17.0

### Take Profit (TP)
- Zonas analizadas: 509 | Total candidatos: 3717 | Seleccionados: 509
- Candidatos por zona (promedio): 7.3
- **Edad (barras)** - Candidatos: med=52, max=182 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.40 | Seleccionados: avg=0.70
- **Priority Candidatos**: {'P3': 3717}
- **Priority Seleccionados**: {'P0': 50, 'P3': 233, 'NA': 226}
- **Type Candidatos**: {'Swing': 3717}
- **Type Seleccionados**: {'P0_Zone': 50, 'P3_Swing': 233, 'P4_Fallback': 226}
- **TF Candidatos**: {60: 1232, 15: 954, 5: 860, 240: 671}
- **TF Seleccionados**: {15: 125, 60: 69, -1: 226, 5: 63, 240: 26}
- **DistATR** - Candidatos: avg=9.7 | Seleccionados: avg=3.5
- **RR** - Candidatos: avg=5.03 | Seleccionados: avg=1.31
- **Razones de selección**: {'BestIntelligentScore': 509}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.