# Informe Diagnóstico de Logs - 2025-11-21 09:46:55

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251121_093934.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251121_093934.csv`

## DFM
- Eventos de evaluación: 289
- Evaluaciones Bull: 0 | Bear: 283
- Pasaron umbral (PassedThreshold): 283
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:6, 6:110, 7:112, 8:47, 9:8

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3129
- KeptAligned: 3859/3859 | KeptCounter: 4490/4819
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.233 | AvgProxCounter≈ 0.236
  - AvgDistATRAligned≈ 0.88 | AvgDistATRCounter≈ 1.20
- PreferAligned eventos: 932 | Filtradas contra-bias: 195

### Proximity (Pre-PreferAligned)
- Eventos: 3129
- Aligned pre: 3859/8349 | Counter pre: 4490/8349
- AvgProxAligned(pre)≈ 0.233 | AvgDistATRAligned(pre)≈ 0.88

### Proximity Drivers
- Eventos: 3129
- Alineadas: n=3859 | BaseProx≈ 0.747 | ZoneATR≈ 4.33 | SizePenalty≈ 0.984 | FinalProx≈ 0.735
- Contra-bias: n=4295 | BaseProx≈ 0.517 | ZoneATR≈ 5.59 | SizePenalty≈ 0.967 | FinalProx≈ 0.500

## Risk
- Eventos: 2311
- Accepted=401 | RejSL=0 | RejTP=0 | RejRR=570 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 824 (29.4% del total)
  - Avg Score: 0.39 | Avg R:R: 1.62 | Avg DistATR: 3.97
  - Por TF: TF5=113, TF15=711
- **P0_SWING_LITE:** 1980 (70.6% del total)
  - Avg Score: 0.82 | Avg R:R: 2.83 | Avg DistATR: 4.11
  - Por TF: TF15=265, TF60=1715


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 27 | Unmatched: 374
- 0-10: Wins=27 Losses=0 WR=100.0% (n=27)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=27 Losses=0 WR=100.0% (n=27)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 401 | Aligned=236 (58.9%)
- Core≈ 1.00 | Prox≈ 0.65 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.07 | Confidence≈ 0.00
- SL_TF dist: {'15': 262, '5': 139} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 152, '240': 197, '15': 38, '60': 14} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=401, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=139, 15m=262, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.07 (n=401), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 49428 | Zonas con Anchors: 49428
- Dir zonas (zona): Bull=3391 Bear=45485 Neutral=552
- Resumen por ciclo (promedios): TotHZ≈ 15.8, WithAnchors≈ 15.8, DirBull≈ 1.1, DirBear≈ 14.5, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 48638, 'tie-bias': 790}
- TF Triggers: {'5': 31301, '15': 18127}
- TF Anchors: {'60': 49428, '240': 49428, '1440': 49428}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## Timing y Confluencia HTF
- [TIMING_ADAPT] eventos: 133 | Approach rejects: 111
- Score/Req promedio: 2.39/2.00
- [HTF_CONFL] muestras: 814 | ok=814 | rejects=0
- median≈ 0.112 | thr≈ 0.106
- [BIAS_FAST] muestras: 1373 | Bull=89 Bear=1160 Neutral=124 | rejects=8
- score promedio: -0.62
- [HTF_CONFL] muestras: 814 | ok=814 | rejects=0
- median≈ 0.112 | thr≈ 0.106
- [RC_SL_MINFLOOR] filtrados: 0

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,27': 1}

## CSV de Trades
- Filas: 19 | Ejecutadas: 6 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 25

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 283
- Registered: 10
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 10

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 3.5%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 60.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7803 | Total candidatos: 151573 | Seleccionados: 46
- Candidatos por zona (promedio): 19.4

### Take Profit (TP)
- Zonas analizadas: 7795 | Total candidatos: 310088 | Seleccionados: 7795
- Candidatos por zona (promedio): 39.8
- **Edad (barras)** - Candidatos: med=1059, max=8120 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.80 | Seleccionados: avg=0.65
- **Priority Candidatos**: {'P3': 177621}
- **Priority Seleccionados**: {'P3': 7044, 'NA': 522, 'P0': 229}
- **Type Candidatos**: {'Swing': 177621}
- **Type Seleccionados**: {'P3_Swing': 7044, 'P4_Fallback': 522, 'P0_Zone': 229}
- **TF Candidatos**: {5: 85411, 15: 50348, 240: 23147, 60: 18715}
- **TF Seleccionados**: {5: 5184, 240: 1618, 15: 289, -1: 522, 60: 182}
- **DistATR** - Candidatos: avg=34.7 | Seleccionados: avg=16.9
- **RR** - Candidatos: avg=5.68 | Seleccionados: avg=1.26
- **Razones de selección**: {'BestIntelligentScore': 7795}

### 🎯 Recomendaciones
- ⚠️ SL: 100% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.