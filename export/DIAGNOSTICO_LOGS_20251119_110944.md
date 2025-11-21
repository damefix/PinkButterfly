# Informe Diagnóstico de Logs - 2025-11-19 11:18:56

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_110944.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_110944.csv`

## DFM
- Eventos de evaluación: 932
- Evaluaciones Bull: 9 | Bear: 960
- Pasaron umbral (PassedThreshold): 969
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:32, 6:388, 7:390, 8:159, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3065
- KeptAligned: 2927/2927 | KeptCounter: 2376/2508
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.191 | AvgProxCounter≈ 0.142
  - AvgDistATRAligned≈ 0.85 | AvgDistATRCounter≈ 0.70
- PreferAligned eventos: 784 | Filtradas contra-bias: 37

### Proximity (Pre-PreferAligned)
- Eventos: 3065
- Aligned pre: 2927/5303 | Counter pre: 2376/5303
- AvgProxAligned(pre)≈ 0.191 | AvgDistATRAligned(pre)≈ 0.85

### Proximity Drivers
- Eventos: 3065
- Alineadas: n=2927 | BaseProx≈ 0.727 | ZoneATR≈ 4.74 | SizePenalty≈ 0.982 | FinalProx≈ 0.714
- Contra-bias: n=2339 | BaseProx≈ 0.512 | ZoneATR≈ 5.69 | SizePenalty≈ 0.967 | FinalProx≈ 0.496

## Risk
- Eventos: 1623
- Accepted=1398 | RejSL=0 | RejTP=0 | RejRR=1008 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 580 (14.5% del total)
  - Avg Score: 0.40 | Avg R:R: 1.88 | Avg DistATR: 4.00
  - Por TF: TF5=181, TF15=399
- **P0_SWING_LITE:** 3417 (85.5% del total)
  - Avg Score: 0.62 | Avg R:R: 4.53 | Avg DistATR: 3.89
  - Por TF: TF15=652, TF60=2765


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 375 | Unmatched: 1023
- 0-10: Wins=197 Losses=178 WR=52.5% (n=375)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=197 Losses=178 WR=52.5% (n=375)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1398 | Aligned=826 (59.1%)
- Core≈ 1.00 | Prox≈ 0.65 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.79 | Confidence≈ 0.00
- SL_TF dist: {'15': 904, '5': 327, '240': 59, '60': 108} | SL_Structural≈ 100.0%
- TP_TF dist: {'240': 532, '15': 419, '60': 233, '5': 214} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1398, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=327, 15m=904, 60m=108, 240m=59, 1440m=0
- RR plan por bandas: 0-10≈ 1.79 (n=1398), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 17697 | Zonas con Anchors: 17697
- Dir zonas (zona): Bull=640 Bear=16182 Neutral=875
- Resumen por ciclo (promedios): TotHZ≈ 5.7, WithAnchors≈ 5.7, DirBull≈ 0.2, DirBear≈ 5.2, DirNeutral≈ 0.3
- Razones de dirección: {'anchors+triggers': 16540, 'tie-bias': 1157}
- TF Triggers: {'5': 8498, '15': 9199}
- TF Anchors: {'60': 17697, '240': 17697, '1440': 17697}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,37': 2, 'score decayó a 0,41': 1, 'estructura no existe': 6, 'score decayó a 0,15': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 36} | por bias {'Bullish': 36, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 192 | Ejecutadas: 15 | Canceladas: 0 | Expiradas: 0
- BUY: 2 | SELL: 205

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 969
- Registered: 112
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 112

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 11.6%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 13.4%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4956 | Total candidatos: 84312 | Seleccionados: 0
- Candidatos por zona (promedio): 17.0

### Take Profit (TP)
- Zonas analizadas: 4956 | Total candidatos: 113060 | Seleccionados: 4956
- Candidatos por zona (promedio): 22.8
- **Edad (barras)** - Candidatos: med=90, max=683 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.57 | Seleccionados: avg=0.73
- **Priority Candidatos**: {'P3': 67211}
- **Priority Seleccionados**: {'P3': 3255, 'P0': 352, 'NA': 1349}
- **Type Candidatos**: {'Swing': 67211}
- **Type Seleccionados**: {'P3_Swing': 3255, 'P0_Zone': 352, 'P4_Fallback': 1349}
- **TF Candidatos**: {240: 38475, 15: 12519, 60: 9837, 5: 6380}
- **TF Seleccionados**: {240: 1775, 15: 810, -1: 1349, 60: 529, 5: 493}
- **DistATR** - Candidatos: avg=17.1 | Seleccionados: avg=4.6
- **RR** - Candidatos: avg=8.20 | Seleccionados: avg=1.43
- **Razones de selección**: {'BestIntelligentScore': 4956}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.