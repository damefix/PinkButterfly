# Informe Diagnóstico de Logs - 2025-11-12 17:47:40

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_174403.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251112_174403.csv`

## DFM
- Eventos de evaluación: 956
- Evaluaciones Bull: 193 | Bear: 723
- Pasaron umbral (PassedThreshold): 916
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:127, 6:381, 7:355, 8:53, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2347
- KeptAligned: 4211/4211 | KeptCounter: 2574/2674
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.449 | AvgProxCounter≈ 0.235
  - AvgDistATRAligned≈ 1.53 | AvgDistATRCounter≈ 1.10
- PreferAligned eventos: 1298 | Filtradas contra-bias: 583

### Proximity (Pre-PreferAligned)
- Eventos: 2347
- Aligned pre: 4211/6785 | Counter pre: 2574/6785
- AvgProxAligned(pre)≈ 0.449 | AvgDistATRAligned(pre)≈ 1.53

### Proximity Drivers
- Eventos: 2347
- Alineadas: n=4211 | BaseProx≈ 0.753 | ZoneATR≈ 5.22 | SizePenalty≈ 0.974 | FinalProx≈ 0.735
- Contra-bias: n=1991 | BaseProx≈ 0.549 | ZoneATR≈ 5.03 | SizePenalty≈ 0.976 | FinalProx≈ 0.539

## Risk
- Eventos: 1958
- Accepted=1299 | RejSL=0 | RejTP=0 | RejRR=1273 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 313 (11.0% del total)
  - Avg Score: 0.39 | Avg R:R: 1.90 | Avg DistATR: 3.75
  - Por TF: TF5=78, TF15=235
- **P0_SWING_LITE:** 2533 (89.0% del total)
  - Avg Score: 0.57 | Avg R:R: 4.17 | Avg DistATR: 3.46
  - Por TF: TF15=540, TF60=1993


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 110 | Unmatched: 1217
- 0-10: Wins=36 Losses=74 WR=32.7% (n=110)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=36 Losses=74 WR=32.7% (n=110)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1327 | Aligned=829 (62.5%)
- Core≈ 1.00 | Prox≈ 0.68 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.03 | Confidence≈ 0.00
- SL_TF dist: {'60': 157, '5': 187, '15': 979, '240': 4} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 318, '15': 499, '5': 338, '240': 172} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1299, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=182, 15m=960, 60m=153, 240m=4, 1440m=0
- RR plan por bandas: 0-10≈ 2.04 (n=1299), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10213 | Zonas con Anchors: 10199
- Dir zonas (zona): Bull=3888 Bear=5970 Neutral=355
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.6, DirBear≈ 2.4, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9781, 'tie-bias': 418, 'triggers-only': 14}
- TF Triggers: {'15': 4752, '5': 5461}
- TF Anchors: {'60': 10125, '240': 5930, '1440': 741}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 29, 'score decayó a 0,27': 1, 'score decayó a 0,46': 1, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,42': 1, 'score decayó a 0,32': 1, 'score decayó a 0,34': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 217 | Ejecutadas: 36 | Canceladas: 0 | Expiradas: 0
- BUY: 87 | SELL: 166

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 916
- Registered: 114
  - DEDUP_COOLDOWN: 23 | DEDUP_IDENTICAL: 110 | SKIP_CONCURRENCY: 99
- Intentos de registro: 346

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 37.8%
- RegRate = Registered / Intentos = 32.9%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 38.4%
- Concurrency = SKIP_CONCURRENCY / Intentos = 28.6%
- ExecRate = Ejecutadas / Registered = 31.6%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5449 | Total candidatos: 43482 | Seleccionados: 0
- Candidatos por zona (promedio): 8.0

### Take Profit (TP)
- Zonas analizadas: 5355 | Total candidatos: 50949 | Seleccionados: 5355
- Candidatos por zona (promedio): 9.5
- **Edad (barras)** - Candidatos: med=41, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 50949}
- **Priority Seleccionados**: {'P3': 3646, 'P0': 339, 'NA': 1370}
- **Type Candidatos**: {'Swing': 50949}
- **Type Seleccionados**: {'P3_Swing': 3646, 'P0_Zone': 339, 'P4_Fallback': 1370}
- **TF Candidatos**: {5: 14986, 15: 13745, 60: 13362, 240: 8856}
- **TF Seleccionados**: {60: 1034, 5: 1000, 15: 1234, -1: 1370, 240: 717}
- **DistATR** - Candidatos: avg=8.7 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.45 | Seleccionados: avg=1.31
- **Razones de selección**: {'BestIntelligentScore': 5355}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.