# Informe Diagnóstico de Logs - 2025-11-20 09:15:55

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251120_091106.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251120_091106.csv`

## DFM
- Eventos de evaluación: 888
- Evaluaciones Bull: 0 | Bear: 653
- Pasaron umbral (PassedThreshold): 653
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:12, 6:250, 7:278, 8:113, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3119
- KeptAligned: 2291/2291 | KeptCounter: 2888/3090
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.167 | AvgProxCounter≈ 0.148
  - AvgDistATRAligned≈ 0.70 | AvgDistATRCounter≈ 0.94
- PreferAligned eventos: 717 | Filtradas contra-bias: 32

### Proximity (Pre-PreferAligned)
- Eventos: 3119
- Aligned pre: 2291/5179 | Counter pre: 2888/5179
- AvgProxAligned(pre)≈ 0.167 | AvgDistATRAligned(pre)≈ 0.70

### Proximity Drivers
- Eventos: 3119
- Alineadas: n=2291 | BaseProx≈ 0.728 | ZoneATR≈ 3.50 | SizePenalty≈ 0.992 | FinalProx≈ 0.722
- Contra-bias: n=2856 | BaseProx≈ 0.475 | ZoneATR≈ 4.65 | SizePenalty≈ 0.981 | FinalProx≈ 0.466

## Risk
- Eventos: 1745
- Accepted=1212 | RejSL=0 | RejTP=0 | RejRR=1625 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 808 (20.6% del total)
  - Avg Score: 0.38 | Avg R:R: 1.90 | Avg DistATR: 3.96
  - Por TF: TF5=83, TF15=725
- **P0_SWING_LITE:** 3106 (79.4% del total)
  - Avg Score: 0.64 | Avg R:R: 3.64 | Avg DistATR: 4.00
  - Por TF: TF15=64, TF60=3042


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 197 | Unmatched: 1017
- 0-10: Wins=193 Losses=4 WR=98.0% (n=197)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=193 Losses=4 WR=98.0% (n=197)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1214 | Aligned=517 (42.6%)
- Core≈ 1.00 | Prox≈ 0.59 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.15 | Confidence≈ 0.00
- SL_TF dist: {'15': 1087, '60': 29, '5': 83, '240': 15} | SL_Structural≈ 100.0%
- TP_TF dist: {'240': 551, '15': 497, '60': 78, '5': 88} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1212, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=81, 15m=1087, 60m=29, 240m=15, 1440m=0
- RR plan por bandas: 0-10≈ 2.15 (n=1212), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 21808 | Zonas con Anchors: 21808
- Dir zonas (zona): Bull=416 Bear=21326 Neutral=66
- Resumen por ciclo (promedios): TotHZ≈ 7.0, WithAnchors≈ 7.0, DirBull≈ 0.1, DirBear≈ 6.8, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 21668, 'tie-bias': 140}
- TF Triggers: {'5': 7054, '15': 14754}
- TF Anchors: {'60': 21808, '240': 21808, '1440': 21808}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## Timing y Confluencia HTF
- [TIMING_ADAPT] eventos: 210 | Approach rejects: 155
- Score/Req promedio: 2.49/2.00
- [HTF_CONFL] muestras: 1074 | ok=1074 | rejects=0
- median≈ 0.170 | thr≈ 0.159
- [BIAS_FAST] muestras: 1899 | Bull=304 Bear=1500 Neutral=95 | rejects=28
- score promedio: -0.59
- [HTF_CONFL] muestras: 1074 | ok=1074 | rejects=0
- median≈ 0.170 | thr≈ 0.159
- [RC_SL_MINFLOOR] filtrados: 0

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,27': 1, 'estructura no existe': 5, 'score decayó a 0,49': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 8} | por bias {'Bullish': 8, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 67 | Ejecutadas: 6 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 73

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 653
- Registered: 34
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 34

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 5.2%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 17.6%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4829 | Total candidatos: 86675 | Seleccionados: 232
- Candidatos por zona (promedio): 17.9

### Take Profit (TP)
- Zonas analizadas: 4826 | Total candidatos: 136903 | Seleccionados: 4826
- Candidatos por zona (promedio): 28.4
- **Edad (barras)** - Candidatos: med=80, max=552 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.56 | Seleccionados: avg=0.75
- **Priority Candidatos**: {'P3': 81826}
- **Priority Seleccionados**: {'P3': 4258, 'NA': 444, 'P0': 124}
- **Type Candidatos**: {'Swing': 81826}
- **Type Seleccionados**: {'P3_Swing': 4258, 'P4_Fallback': 444, 'P0_Zone': 124}
- **TF Candidatos**: {240: 53926, 60: 14801, 15: 7755, 5: 5344}
- **TF Seleccionados**: {240: 2150, 15: 1421, -1: 444, 60: 327, 5: 484}
- **DistATR** - Candidatos: avg=18.8 | Seleccionados: avg=4.9
- **RR** - Candidatos: avg=6.66 | Seleccionados: avg=1.42
- **Razones de selección**: {'BestIntelligentScore': 4826}

### 🎯 Recomendaciones
- ⚠️ SL: 100% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.