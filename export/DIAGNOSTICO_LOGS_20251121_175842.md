# Informe Diagnóstico de Logs - 2025-11-21 18:00:26

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251121_175842.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251121_175842.csv`

## DFM
- Eventos de evaluación: 52
- Evaluaciones Bull: 2 | Bear: 19
- Pasaron umbral (PassedThreshold): 21
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:6, 6:4, 7:11, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 437
- KeptAligned: 428/428 | KeptCounter: 625/700
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.375 | AvgProxCounter≈ 0.331
  - AvgDistATRAligned≈ 1.55 | AvgDistATRCounter≈ 1.57
- PreferAligned eventos: 204 | Filtradas contra-bias: 192

### Proximity (Pre-PreferAligned)
- Eventos: 437
- Aligned pre: 428/1053 | Counter pre: 625/1053
- AvgProxAligned(pre)≈ 0.375 | AvgDistATRAligned(pre)≈ 1.55

### Proximity Drivers
- Eventos: 437
- Alineadas: n=428 | BaseProx≈ 0.722 | ZoneATR≈ 5.13 | SizePenalty≈ 0.973 | FinalProx≈ 0.701
- Contra-bias: n=433 | BaseProx≈ 0.539 | ZoneATR≈ 4.61 | SizePenalty≈ 0.983 | FinalProx≈ 0.530

## Risk
- Eventos: 396
- Accepted=53 | RejSL=0 | RejTP=0 | RejRR=61 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 29 (9.7% del total)
  - Avg Score: 0.41 | Avg R:R: 1.78 | Avg DistATR: 4.24
  - Por TF: TF5=16, TF15=13
- **P0_SWING_LITE:** 270 (90.3% del total)
  - Avg Score: 0.88 | Avg R:R: 3.12 | Avg DistATR: 3.30
  - Por TF: TF15=5, TF60=265


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 4 | Unmatched: 49
- 0-10: Wins=1 Losses=3 WR=25.0% (n=4)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=1 Losses=3 WR=25.0% (n=4)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 53 | Aligned=28 (52.8%)
- Core≈ 0.99 | Prox≈ 0.70 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.30 | Confidence≈ 0.00
- SL_TF dist: {'5': 51, '60': 2} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 31, '60': 18, '15': 3, '240': 1} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=53, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=51, 15m=0, 60m=2, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.30 (n=53), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 2685 | Zonas con Anchors: 2576
- Dir zonas (zona): Bull=1440 Bear=970 Neutral=275
- Resumen por ciclo (promedios): TotHZ≈ 6.1, WithAnchors≈ 5.9, DirBull≈ 3.3, DirBear≈ 2.2, DirNeutral≈ 0.6
- Razones de dirección: {'triggers-only': 97, 'tie-bias': 394, 'anchors+triggers': 2194}
- TF Triggers: {'5': 2312, '15': 373}
- TF Anchors: {'60': 2575, '240': 2160}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## Timing y Confluencia HTF
- [TIMING_ADAPT] eventos: 11 | Approach rejects: 1
- Score/Req promedio: 2.64/2.00
- [HTF_CONFL] muestras: 48 | ok=48 | rejects=0
- median≈ 0.302 | thr≈ 0.272
- [BIAS_FAST] muestras: 83 | Bull=0 Bear=83 Neutral=0 | rejects=0
- score promedio: -1.13
- [HTF_CONFL] muestras: 48 | ok=48 | rejects=0
- median≈ 0.302 | thr≈ 0.272
- [RC_SL_MINFLOOR] filtrados: 0

## TradeManager - Razones (desde log)

## CSV de Trades
- Filas: 12 | Ejecutadas: 3 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 15

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 21
- Registered: 7
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 7

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 33.3%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 42.9%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 725 | Total candidatos: 5992 | Seleccionados: 6
- Candidatos por zona (promedio): 8.3

### Take Profit (TP)
- Zonas analizadas: 725 | Total candidatos: 4580 | Seleccionados: 725
- Candidatos por zona (promedio): 6.3
- **Edad (barras)** - Candidatos: med=382, max=1168 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.84 | Seleccionados: avg=0.60
- **Priority Candidatos**: {'P3': 4580}
- **Priority Seleccionados**: {'NA': 271, 'P3': 422, 'P0': 32}
- **Type Candidatos**: {'Swing': 4580}
- **Type Seleccionados**: {'P4_Fallback': 271, 'P3_Swing': 422, 'P0_Zone': 32}
- **TF Candidatos**: {5: 3375, 60: 838, 240: 345, 15: 22}
- **TF Seleccionados**: {-1: 271, 5: 309, 60: 87, 240: 43, 15: 15}
- **DistATR** - Candidatos: avg=11.7 | Seleccionados: avg=8.0
- **RR** - Candidatos: avg=3.03 | Seleccionados: avg=1.45
- **Razones de selección**: {'BestIntelligentScore': 725}

### 🎯 Recomendaciones
- ⚠️ SL: 100% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.