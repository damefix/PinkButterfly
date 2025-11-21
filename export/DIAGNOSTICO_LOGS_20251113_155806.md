# Informe Diagnóstico de Logs - 2025-11-13 16:02:24

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_155806.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_155806.csv`

## DFM
- Eventos de evaluación: 941
- Evaluaciones Bull: 158 | Bear: 680
- Pasaron umbral (PassedThreshold): 838
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:64, 6:379, 7:346, 8:49, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2355
- KeptAligned: 4122/4122 | KeptCounter: 2816/2920
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.441 | AvgProxCounter≈ 0.232
  - AvgDistATRAligned≈ 1.50 | AvgDistATRCounter≈ 1.16
- PreferAligned eventos: 1270 | Filtradas contra-bias: 566

### Proximity (Pre-PreferAligned)
- Eventos: 2355
- Aligned pre: 4122/6938 | Counter pre: 2816/6938
- AvgProxAligned(pre)≈ 0.441 | AvgDistATRAligned(pre)≈ 1.50

### Proximity Drivers
- Eventos: 2355
- Alineadas: n=4122 | BaseProx≈ 0.751 | ZoneATR≈ 5.18 | SizePenalty≈ 0.975 | FinalProx≈ 0.733
- Contra-bias: n=2250 | BaseProx≈ 0.522 | ZoneATR≈ 4.88 | SizePenalty≈ 0.977 | FinalProx≈ 0.512

## Risk
- Eventos: 1953
- Accepted=1272 | RejSL=0 | RejTP=0 | RejRR=1236 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 316 (11.2% del total)
  - Avg Score: 0.39 | Avg R:R: 1.90 | Avg DistATR: 3.77
  - Por TF: TF5=81, TF15=235
- **P0_SWING_LITE:** 2505 (88.8% del total)
  - Avg Score: 0.58 | Avg R:R: 4.06 | Avg DistATR: 3.49
  - Por TF: TF15=595, TF60=1910


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 89 | Unmatched: 1221
- 0-10: Wins=36 Losses=53 WR=40.4% (n=89)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=36 Losses=53 WR=40.4% (n=89)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1310 | Aligned=786 (60.0%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.08 | Confidence≈ 0.00
- SL_TF dist: {'15': 942, '60': 143, '5': 206, '240': 19} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 497, '5': 360, '60': 278, '240': 175} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1272, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=198, 15m=920, 60m=139, 240m=15, 1440m=0
- RR plan por bandas: 0-10≈ 2.06 (n=1272), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10358 | Zonas con Anchors: 10344
- Dir zonas (zona): Bull=3752 Bear=6278 Neutral=328
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9946, 'tie-bias': 398, 'triggers-only': 14}
- TF Triggers: {'5': 5445, '15': 4913}
- TF Anchors: {'60': 10270, '240': 5970, '1440': 542}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,27': 1, 'score decayó a 0,46': 1, 'estructura no existe': 25, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,42': 1, 'score decayó a 0,32': 1, 'score decayó a 0,34': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 199 | Ejecutadas: 37 | Canceladas: 0 | Expiradas: 0
- BUY: 67 | SELL: 169

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 838
- Registered: 105
  - DEDUP_COOLDOWN: 18 | DEDUP_IDENTICAL: 96 | SKIP_CONCURRENCY: 101
- Intentos de registro: 320

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 38.2%
- RegRate = Registered / Intentos = 32.8%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 35.6%
- Concurrency = SKIP_CONCURRENCY / Intentos = 31.6%
- ExecRate = Ejecutadas / Registered = 35.2%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5447 | Total candidatos: 42742 | Seleccionados: 0
- Candidatos por zona (promedio): 7.8

### Take Profit (TP)
- Zonas analizadas: 5339 | Total candidatos: 51599 | Seleccionados: 5339
- Candidatos por zona (promedio): 9.7
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 51599}
- **Priority Seleccionados**: {'P3': 3624, 'NA': 1397, 'P0': 318}
- **Type Candidatos**: {'Swing': 51599}
- **Type Seleccionados**: {'P3_Swing': 3624, 'P4_Fallback': 1397, 'P0_Zone': 318}
- **TF Candidatos**: {5: 15439, 15: 14122, 60: 13458, 240: 8580}
- **TF Seleccionados**: {60: 970, 5: 1012, 15: 1263, -1: 1397, 240: 697}
- **DistATR** - Candidatos: avg=8.7 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.57 | Seleccionados: avg=1.31
- **Razones de selección**: {'BestIntelligentScore': 5339}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.