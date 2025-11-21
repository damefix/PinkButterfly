# Informe Diagnóstico de Logs - 2025-11-13 15:57:37

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_155348.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_155348.csv`

## DFM
- Eventos de evaluación: 941
- Evaluaciones Bull: 159 | Bear: 681
- Pasaron umbral (PassedThreshold): 840
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:66, 6:379, 7:346, 8:49, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2355
- KeptAligned: 4126/4126 | KeptCounter: 2816/2920
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.441 | AvgProxCounter≈ 0.232
  - AvgDistATRAligned≈ 1.50 | AvgDistATRCounter≈ 1.16
- PreferAligned eventos: 1269 | Filtradas contra-bias: 563

### Proximity (Pre-PreferAligned)
- Eventos: 2355
- Aligned pre: 4126/6942 | Counter pre: 2816/6942
- AvgProxAligned(pre)≈ 0.441 | AvgDistATRAligned(pre)≈ 1.50

### Proximity Drivers
- Eventos: 2355
- Alineadas: n=4126 | BaseProx≈ 0.751 | ZoneATR≈ 5.18 | SizePenalty≈ 0.975 | FinalProx≈ 0.733
- Contra-bias: n=2253 | BaseProx≈ 0.523 | ZoneATR≈ 4.87 | SizePenalty≈ 0.977 | FinalProx≈ 0.513

## Risk
- Eventos: 1952
- Accepted=1274 | RejSL=0 | RejTP=0 | RejRR=1228 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 313 (11.1% del total)
  - Avg Score: 0.39 | Avg R:R: 1.90 | Avg DistATR: 3.77
  - Por TF: TF5=79, TF15=234
- **P0_SWING_LITE:** 2519 (88.9% del total)
  - Avg Score: 0.58 | Avg R:R: 4.10 | Avg DistATR: 3.49
  - Por TF: TF15=594, TF60=1925


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 88 | Unmatched: 1224
- 0-10: Wins=36 Losses=52 WR=40.9% (n=88)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=36 Losses=52 WR=40.9% (n=88)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1312 | Aligned=782 (59.6%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.10 | Confidence≈ 0.00
- SL_TF dist: {'15': 945, '60': 143, '5': 205, '240': 19} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 500, '5': 361, '60': 277, '240': 174} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1274, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=197, 15m=923, 60m=139, 240m=15, 1440m=0
- RR plan por bandas: 0-10≈ 2.08 (n=1274), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10364 | Zonas con Anchors: 10350
- Dir zonas (zona): Bull=3755 Bear=6279 Neutral=330
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9950, 'tie-bias': 400, 'triggers-only': 14}
- TF Triggers: {'5': 5446, '15': 4918}
- TF Anchors: {'60': 10276, '240': 5974, '1440': 542}

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
- DFM Señales (PassedThreshold): 840
- Registered: 105
  - DEDUP_COOLDOWN: 18 | DEDUP_IDENTICAL: 95 | SKIP_CONCURRENCY: 101
- Intentos de registro: 319

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 38.0%
- RegRate = Registered / Intentos = 32.9%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 35.4%
- Concurrency = SKIP_CONCURRENCY / Intentos = 31.7%
- ExecRate = Ejecutadas / Registered = 35.2%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5453 | Total candidatos: 42766 | Seleccionados: 0
- Candidatos por zona (promedio): 7.8

### Take Profit (TP)
- Zonas analizadas: 5344 | Total candidatos: 51702 | Seleccionados: 5344
- Candidatos por zona (promedio): 9.7
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 51702}
- **Priority Seleccionados**: {'P3': 3639, 'NA': 1395, 'P0': 310}
- **Type Candidatos**: {'Swing': 51702}
- **Type Seleccionados**: {'P3_Swing': 3639, 'P4_Fallback': 1395, 'P0_Zone': 310}
- **TF Candidatos**: {5: 15475, 15: 14156, 60: 13491, 240: 8580}
- **TF Seleccionados**: {60: 971, 5: 1015, 15: 1267, -1: 1395, 240: 696}
- **DistATR** - Candidatos: avg=8.7 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.61 | Seleccionados: avg=1.33
- **Razones de selección**: {'BestIntelligentScore': 5344}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.