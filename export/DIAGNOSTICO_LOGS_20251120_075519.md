# Informe Diagnóstico de Logs - 2025-11-20 08:19:31

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251120_075519.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251120_075519.csv`

## DFM
- Eventos de evaluación: 615
- Evaluaciones Bull: 0 | Bear: 539
- Pasaron umbral (PassedThreshold): 539
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:9, 6:187, 7:252, 8:91, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3123
- KeptAligned: 2506/2506 | KeptCounter: 2191/2339
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.174 | AvgProxCounter≈ 0.103
  - AvgDistATRAligned≈ 0.67 | AvgDistATRCounter≈ 0.71
- PreferAligned eventos: 738 | Filtradas contra-bias: 16

### Proximity (Pre-PreferAligned)
- Eventos: 3123
- Aligned pre: 2506/4697 | Counter pre: 2191/4697
- AvgProxAligned(pre)≈ 0.174 | AvgDistATRAligned(pre)≈ 0.67

### Proximity Drivers
- Eventos: 3123
- Alineadas: n=2506 | BaseProx≈ 0.749 | ZoneATR≈ 4.00 | SizePenalty≈ 0.987 | FinalProx≈ 0.739
- Contra-bias: n=2175 | BaseProx≈ 0.458 | ZoneATR≈ 4.86 | SizePenalty≈ 0.977 | FinalProx≈ 0.448

## Risk
- Eventos: 1494
- Accepted=768 | RejSL=0 | RejTP=0 | RejRR=1567 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 433 (13.4% del total)
  - Avg Score: 0.39 | Avg R:R: 1.90 | Avg DistATR: 3.88
  - Por TF: TF5=28, TF15=405
- **P0_SWING_LITE:** 2805 (86.6% del total)
  - Avg Score: 0.61 | Avg R:R: 3.58 | Avg DistATR: 3.99
  - Por TF: TF15=106, TF60=2699


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 24 | Unmatched: 744
- 0-10: Wins=18 Losses=6 WR=75.0% (n=24)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=18 Losses=6 WR=75.0% (n=24)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 768 | Aligned=449 (58.5%)
- Core≈ 1.00 | Prox≈ 0.64 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.42 | Confidence≈ 0.00
- SL_TF dist: {'15': 633, '60': 38, '5': 87, '240': 10} | SL_Structural≈ 100.0%
- TP_TF dist: {'240': 468, '15': 136, '60': 89, '5': 75} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=768, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=87, 15m=633, 60m=38, 240m=10, 1440m=0
- RR plan por bandas: 0-10≈ 2.42 (n=768), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 17328 | Zonas con Anchors: 17328
- Dir zonas (zona): Bull=220 Bear=17020 Neutral=88
- Resumen por ciclo (promedios): TotHZ≈ 5.5, WithAnchors≈ 5.5, DirBull≈ 0.1, DirBear≈ 5.4, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 17224, 'tie-bias': 104}
- TF Triggers: {'5': 6292, '15': 11036}
- TF Anchors: {'60': 17328, '240': 17328, '1440': 17328}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## Timing y Confluencia HTF
- [TIMING_ADAPT] eventos: 193 | Approach rejects: 141
- Score/Req promedio: 2.47/2.00
- [HTF_CONFL] muestras: 1002 | ok=1002 | rejects=0
- median≈ 0.169 | thr≈ 0.159
- [BIAS_FAST] muestras: 1763 | Bull=245 Bear=1436 Neutral=82 | rejects=23
- score promedio: -0.60
- [HTF_CONFL] muestras: 1002 | ok=1002 | rejects=0
- median≈ 0.169 | thr≈ 0.159
- [RC_SL_MINFLOOR] filtrados: 0

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,25': 2, 'estructura no existe': 7, 'score decayó a 0,49': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 4} | por bias {'Bullish': 4, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 69 | Ejecutadas: 11 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 80

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 539
- Registered: 35
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 35

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 6.5%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 31.4%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4332 | Total candidatos: 73059 | Seleccionados: 104
- Candidatos por zona (promedio): 16.9

### Take Profit (TP)
- Zonas analizadas: 4332 | Total candidatos: 115614 | Seleccionados: 4332
- Candidatos por zona (promedio): 26.7
- **Edad (barras)** - Candidatos: med=80, max=481 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.56 | Seleccionados: avg=0.74
- **Priority Candidatos**: {'P3': 67589}
- **Priority Seleccionados**: {'P3': 3828, 'NA': 405, 'P0': 99}
- **Type Candidatos**: {'Swing': 67589}
- **Type Seleccionados**: {'P3_Swing': 3828, 'P4_Fallback': 405, 'P0_Zone': 99}
- **TF Candidatos**: {240: 43919, 60: 12066, 5: 6217, 15: 5387}
- **TF Seleccionados**: {240: 2213, -1: 405, 15: 638, 60: 442, 5: 634}
- **DistATR** - Candidatos: avg=16.5 | Seleccionados: avg=5.7
- **RR** - Candidatos: avg=6.07 | Seleccionados: avg=1.43
- **Razones de selección**: {'BestIntelligentScore': 4332}

### 🎯 Recomendaciones
- ⚠️ SL: 100% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.