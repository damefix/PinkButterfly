# Informe Diagnóstico de Logs - 2025-11-19 11:56:55

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_114853.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_114853.csv`

## DFM
- Eventos de evaluación: 664
- Evaluaciones Bull: 9 | Bear: 662
- Pasaron umbral (PassedThreshold): 671
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:26, 6:208, 7:288, 8:149, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3067
- KeptAligned: 2928/2928 | KeptCounter: 2378/2510
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.191 | AvgProxCounter≈ 0.142
  - AvgDistATRAligned≈ 0.85 | AvgDistATRCounter≈ 0.70
- PreferAligned eventos: 785 | Filtradas contra-bias: 37

### Proximity (Pre-PreferAligned)
- Eventos: 3067
- Aligned pre: 2928/5306 | Counter pre: 2378/5306
- AvgProxAligned(pre)≈ 0.191 | AvgDistATRAligned(pre)≈ 0.85

### Proximity Drivers
- Eventos: 3067
- Alineadas: n=2928 | BaseProx≈ 0.727 | ZoneATR≈ 4.74 | SizePenalty≈ 0.982 | FinalProx≈ 0.714
- Contra-bias: n=2341 | BaseProx≈ 0.512 | ZoneATR≈ 5.69 | SizePenalty≈ 0.967 | FinalProx≈ 0.496

## Risk
- Eventos: 1626
- Accepted=919 | RejSL=0 | RejTP=0 | RejRR=1435 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 580 (14.5% del total)
  - Avg Score: 0.40 | Avg R:R: 1.88 | Avg DistATR: 4.00
  - Por TF: TF5=181, TF15=399
- **P0_SWING_LITE:** 3420 (85.5% del total)
  - Avg Score: 0.62 | Avg R:R: 4.53 | Avg DistATR: 3.89
  - Por TF: TF15=652, TF60=2768


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 18 | Unmatched: 901
- 0-10: Wins=18 Losses=0 WR=100.0% (n=18)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=18 Losses=0 WR=100.0% (n=18)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 919 | Aligned=612 (66.6%)
- Core≈ 1.00 | Prox≈ 0.66 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.13 | Confidence≈ 0.00
- SL_TF dist: {'15': 528, '5': 228, '240': 113, '60': 50} | SL_Structural≈ 100.0%
- TP_TF dist: {'240': 446, '60': 160, '15': 172, '5': 141} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=919, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=228, 15m=528, 60m=50, 240m=113, 1440m=0
- RR plan por bandas: 0-10≈ 2.13 (n=919), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 17710 | Zonas con Anchors: 17710
- Dir zonas (zona): Bull=640 Bear=16195 Neutral=875
- Resumen por ciclo (promedios): TotHZ≈ 5.7, WithAnchors≈ 5.7, DirBull≈ 0.2, DirBear≈ 5.2, DirNeutral≈ 0.3
- Razones de dirección: {'anchors+triggers': 16553, 'tie-bias': 1157}
- TF Triggers: {'5': 8505, '15': 9205}
- TF Anchors: {'60': 17710, '240': 17710, '1440': 17710}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 14 | Ejecutadas: 1 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 15

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 671
- Registered: 7
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 7

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 1.0%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 14.3%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4959 | Total candidatos: 84407 | Seleccionados: 0
- Candidatos por zona (promedio): 17.0

### Take Profit (TP)
- Zonas analizadas: 4959 | Total candidatos: 113070 | Seleccionados: 4959
- Candidatos por zona (promedio): 22.8
- **Edad (barras)** - Candidatos: med=90, max=683 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.57 | Seleccionados: avg=0.73
- **Priority Candidatos**: {'P3': 67221}
- **Priority Seleccionados**: {'P3': 3621, 'P0': 401, 'NA': 937}
- **Type Candidatos**: {'Swing': 67221}
- **Type Seleccionados**: {'P3_Swing': 3621, 'P0_Zone': 401, 'P4_Fallback': 937}
- **TF Candidatos**: {240: 38478, 15: 12519, 60: 9842, 5: 6382}
- **TF Seleccionados**: {240: 2055, 15: 854, -1: 937, 60: 569, 5: 544}
- **DistATR** - Candidatos: avg=17.1 | Seleccionados: avg=5.0
- **RR** - Candidatos: avg=8.20 | Seleccionados: avg=1.58
- **Razones de selección**: {'BestIntelligentScore': 4959}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.