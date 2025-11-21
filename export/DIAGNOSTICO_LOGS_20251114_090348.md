# Informe Diagnóstico de Logs - 2025-11-14 09:07:35

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_090348.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251114_090348.csv`

## DFM
- Eventos de evaluación: 929
- Evaluaciones Bull: 129 | Bear: 702
- Pasaron umbral (PassedThreshold): 831
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:72, 6:361, 7:337, 8:61, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2353
- KeptAligned: 4222/4222 | KeptCounter: 2730/2824
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.448 | AvgProxCounter≈ 0.224
  - AvgDistATRAligned≈ 1.57 | AvgDistATRCounter≈ 1.11
- PreferAligned eventos: 1295 | Filtradas contra-bias: 547

### Proximity (Pre-PreferAligned)
- Eventos: 2353
- Aligned pre: 4222/6952 | Counter pre: 2730/6952
- AvgProxAligned(pre)≈ 0.448 | AvgDistATRAligned(pre)≈ 1.57

### Proximity Drivers
- Eventos: 2353
- Alineadas: n=4222 | BaseProx≈ 0.749 | ZoneATR≈ 5.17 | SizePenalty≈ 0.975 | FinalProx≈ 0.731
- Contra-bias: n=2183 | BaseProx≈ 0.518 | ZoneATR≈ 4.86 | SizePenalty≈ 0.977 | FinalProx≈ 0.508

## Risk
- Eventos: 1953
- Accepted=1273 | RejSL=0 | RejTP=0 | RejRR=1295 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 326 (11.4% del total)
  - Avg Score: 0.39 | Avg R:R: 1.92 | Avg DistATR: 3.81
  - Por TF: TF5=83, TF15=243
- **P0_SWING_LITE:** 2532 (88.6% del total)
  - Avg Score: 0.58 | Avg R:R: 4.19 | Avg DistATR: 3.51
  - Por TF: TF15=546, TF60=1986


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 73 | Unmatched: 1240
- 0-10: Wins=22 Losses=51 WR=30.1% (n=73)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=22 Losses=51 WR=30.1% (n=73)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1313 | Aligned=800 (60.9%)
- Core≈ 1.00 | Prox≈ 0.66 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.06 | Confidence≈ 0.00
- SL_TF dist: {'60': 185, '15': 893, '5': 210, '240': 25} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 301, '15': 504, '5': 329, '240': 179} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1273, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=202, 15m=868, 60m=182, 240m=21, 1440m=0
- RR plan por bandas: 0-10≈ 2.04 (n=1273), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10407 | Zonas con Anchors: 10396
- Dir zonas (zona): Bull=3737 Bear=6362 Neutral=308
- Resumen por ciclo (promedios): TotHZ≈ 4.2, WithAnchors≈ 4.2, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 10030, 'tie-bias': 366, 'triggers-only': 11}
- TF Triggers: {'15': 4906, '5': 5501}
- TF Anchors: {'60': 10328, '240': 6090, '1440': 186}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 19, 'score decayó a 0,21': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,41': 1, 'score decayó a 0,15': 1, 'score decayó a 0,22': 1, 'score decayó a 0,28': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 185 | Ejecutadas: 39 | Canceladas: 0 | Expiradas: 0
- BUY: 59 | SELL: 165

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 831
- Registered: 97
  - DEDUP_COOLDOWN: 18 | DEDUP_IDENTICAL: 93 | SKIP_CONCURRENCY: 98
- Intentos de registro: 306

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 36.8%
- RegRate = Registered / Intentos = 31.7%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 36.3%
- Concurrency = SKIP_CONCURRENCY / Intentos = 32.0%
- ExecRate = Ejecutadas / Registered = 40.2%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5499 | Total candidatos: 42874 | Seleccionados: 0
- Candidatos por zona (promedio): 7.8

### Take Profit (TP)
- Zonas analizadas: 5396 | Total candidatos: 51201 | Seleccionados: 5396
- Candidatos por zona (promedio): 9.5
- **Edad (barras)** - Candidatos: med=38, max=192 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 51201}
- **Priority Seleccionados**: {'P3': 3720, 'NA': 1373, 'P0': 303}
- **Type Candidatos**: {'Swing': 51201}
- **Type Seleccionados**: {'P3_Swing': 3720, 'P4_Fallback': 1373, 'P0_Zone': 303}
- **TF Candidatos**: {5: 15772, 15: 14252, 60: 13600, 240: 7577}
- **TF Seleccionados**: {60: 995, 15: 1262, -1: 1373, 5: 1016, 240: 750}
- **DistATR** - Candidatos: avg=8.2 | Seleccionados: avg=5.6
- **RR** - Candidatos: avg=3.42 | Seleccionados: avg=1.30
- **Razones de selección**: {'BestIntelligentScore': 5396}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.