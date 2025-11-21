# Informe Diagnóstico de Logs - 2025-11-21 09:05:36

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251121_085828.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251121_085828.csv`

## DFM
- Eventos de evaluación: 288
- Evaluaciones Bull: 0 | Bear: 282
- Pasaron umbral (PassedThreshold): 282
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:6, 6:110, 7:112, 8:46, 9:8

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3126
- KeptAligned: 3857/3857 | KeptCounter: 4490/4819
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.233 | AvgProxCounter≈ 0.236
  - AvgDistATRAligned≈ 0.88 | AvgDistATRCounter≈ 1.20
- PreferAligned eventos: 931 | Filtradas contra-bias: 195

### Proximity (Pre-PreferAligned)
- Eventos: 3126
- Aligned pre: 3857/8347 | Counter pre: 4490/8347
- AvgProxAligned(pre)≈ 0.233 | AvgDistATRAligned(pre)≈ 0.88

### Proximity Drivers
- Eventos: 3126
- Alineadas: n=3857 | BaseProx≈ 0.747 | ZoneATR≈ 4.33 | SizePenalty≈ 0.984 | FinalProx≈ 0.735
- Contra-bias: n=4295 | BaseProx≈ 0.517 | ZoneATR≈ 5.59 | SizePenalty≈ 0.967 | FinalProx≈ 0.500

## Risk
- Eventos: 2310
- Accepted=400 | RejSL=0 | RejTP=0 | RejRR=569 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 823 (29.4% del total)
  - Avg Score: 0.39 | Avg R:R: 1.62 | Avg DistATR: 3.97
  - Por TF: TF5=112, TF15=711
- **P0_SWING_LITE:** 1980 (70.6% del total)
  - Avg Score: 0.82 | Avg R:R: 2.83 | Avg DistATR: 4.11
  - Por TF: TF15=265, TF60=1715


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 27 | Unmatched: 373
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
- Muestras: 400 | Aligned=235 (58.8%)
- Core≈ 1.00 | Prox≈ 0.65 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.07 | Confidence≈ 0.00
- SL_TF dist: {'15': 262, '5': 138} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 151, '240': 197, '15': 38, '60': 14} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=400, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=138, 15m=262, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.07 (n=400), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 49387 | Zonas con Anchors: 49387
- Dir zonas (zona): Bull=3391 Bear=45444 Neutral=552
- Resumen por ciclo (promedios): TotHZ≈ 15.8, WithAnchors≈ 15.8, DirBull≈ 1.1, DirBear≈ 14.5, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 48597, 'tie-bias': 790}
- TF Triggers: {'5': 31279, '15': 18108}
- TF Anchors: {'60': 49387, '240': 49387, '1440': 49387}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## Timing y Confluencia HTF
- [TIMING_ADAPT] eventos: 132 | Approach rejects: 111
- Score/Req promedio: 2.39/2.00
- [HTF_CONFL] muestras: 808 | ok=808 | rejects=0
- median≈ 0.112 | thr≈ 0.106
- [BIAS_FAST] muestras: 1363 | Bull=89 Bear=1150 Neutral=124 | rejects=8
- score promedio: -0.61
- [HTF_CONFL] muestras: 808 | ok=808 | rejects=0
- median≈ 0.112 | thr≈ 0.106
- [RC_SL_MINFLOOR] filtrados: 0

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,27': 1}

## CSV de Trades
- Filas: 18 | Ejecutadas: 6 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 24

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 282
- Registered: 9
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 9

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 3.2%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 66.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7801 | Total candidatos: 151469 | Seleccionados: 44
- Candidatos por zona (promedio): 19.4

### Take Profit (TP)
- Zonas analizadas: 7793 | Total candidatos: 310082 | Seleccionados: 7793
- Candidatos por zona (promedio): 39.8
- **Edad (barras)** - Candidatos: med=1059, max=8001 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.80 | Seleccionados: avg=0.65
- **Priority Candidatos**: {'P3': 177615}
- **Priority Seleccionados**: {'P3': 7043, 'NA': 522, 'P0': 228}
- **Type Candidatos**: {'Swing': 177615}
- **Type Seleccionados**: {'P3_Swing': 7043, 'P4_Fallback': 522, 'P0_Zone': 228}
- **TF Candidatos**: {5: 85409, 15: 50347, 240: 23146, 60: 18713}
- **TF Seleccionados**: {5: 5183, 240: 1618, 15: 289, -1: 522, 60: 181}
- **DistATR** - Candidatos: avg=34.7 | Seleccionados: avg=17.0
- **RR** - Candidatos: avg=5.68 | Seleccionados: avg=1.26
- **Razones de selección**: {'BestIntelligentScore': 7793}

### 🎯 Recomendaciones
- ⚠️ SL: 100% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.