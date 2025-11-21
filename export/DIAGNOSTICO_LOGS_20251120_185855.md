# Informe Diagnóstico de Logs - 2025-11-20 19:06:05

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251120_185855.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251120_185855.csv`

## DFM
- Eventos de evaluación: 607
- Evaluaciones Bull: 0 | Bear: 528
- Pasaron umbral (PassedThreshold): 528
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:20, 6:245, 7:249, 8:14, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3126
- KeptAligned: 3850/3850 | KeptCounter: 4497/4745
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.233 | AvgProxCounter≈ 0.277
  - AvgDistATRAligned≈ 0.81 | AvgDistATRCounter≈ 1.30
- PreferAligned eventos: 955 | Filtradas contra-bias: 61

### Proximity (Pre-PreferAligned)
- Eventos: 3126
- Aligned pre: 3850/8347 | Counter pre: 4497/8347
- AvgProxAligned(pre)≈ 0.233 | AvgDistATRAligned(pre)≈ 0.81

### Proximity Drivers
- Eventos: 3126
- Alineadas: n=3850 | BaseProx≈ 0.729 | ZoneATR≈ 3.30 | SizePenalty≈ 0.992 | FinalProx≈ 0.723
- Contra-bias: n=4436 | BaseProx≈ 0.507 | ZoneATR≈ 3.88 | SizePenalty≈ 0.984 | FinalProx≈ 0.499

## Risk
- Eventos: 2576
- Accepted=849 | RejSL=0 | RejTP=0 | RejRR=498 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 881 (24.6% del total)
  - Avg Score: 0.39 | Avg R:R: 1.78 | Avg DistATR: 3.90
  - Por TF: TF5=102, TF15=779
- **P0_SWING_LITE:** 2697 (75.4% del total)
  - Avg Score: 0.58 | Avg R:R: 3.16 | Avg DistATR: 3.90
  - Por TF: TF15=320, TF60=2377


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 11 | Unmatched: 838
- 0-10: Wins=8 Losses=3 WR=72.7% (n=11)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=8 Losses=3 WR=72.7% (n=11)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 849 | Aligned=407 (47.9%)
- Core≈ 1.00 | Prox≈ 0.62 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.01 | Confidence≈ 0.00
- SL_TF dist: {'5': 255, '15': 594} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 87, '15': 265, '240': 100, '60': 397} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=849, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=255, 15m=594, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.01 (n=849), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 44909 | Zonas con Anchors: 44909
- Dir zonas (zona): Bull=65 Bear=44549 Neutral=295
- Resumen por ciclo (promedios): TotHZ≈ 14.4, WithAnchors≈ 14.4, DirBull≈ 0.0, DirBear≈ 14.3, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 44583, 'tie-bias': 326}
- TF Triggers: {'15': 15643, '5': 29266}
- TF Anchors: {'60': 44909, '240': 44909, '1440': 44909}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## Timing y Confluencia HTF
- [TIMING_ADAPT] eventos: 149 | Approach rejects: 130
- Score/Req promedio: 2.54/2.00
- [HTF_CONFL] muestras: 898 | ok=898 | rejects=0
- median≈ 0.123 | thr≈ 0.120
- [BIAS_FAST] muestras: 1534 | Bull=154 Bear=1238 Neutral=142 | rejects=13
- score promedio: -0.46
- [HTF_CONFL] muestras: 898 | ok=898 | rejects=0
- median≈ 0.123 | thr≈ 0.120
- [RC_SL_MINFLOOR] filtrados: 0

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,19': 1, 'estructura no existe': 2, 'score decayó a 0,33': 1}

## CSV de Trades
- Filas: 39 | Ejecutadas: 7 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 46

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 528
- Registered: 21
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 21

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 4.0%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 33.3%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 8255 | Total candidatos: 191307 | Seleccionados: 56
- Candidatos por zona (promedio): 23.2

### Take Profit (TP)
- Zonas analizadas: 8255 | Total candidatos: 226518 | Seleccionados: 8255
- Candidatos por zona (promedio): 27.4
- **Edad (barras)** - Candidatos: med=364, max=3449 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.21 | Seleccionados: avg=0.65
- **Priority Candidatos**: {'P3': 111724}
- **Priority Seleccionados**: {'P3': 7236, 'NA': 531, 'P0': 488}
- **Type Candidatos**: {'Swing': 111724}
- **Type Seleccionados**: {'P3_Swing': 7236, 'P4_Fallback': 531, 'P0_Zone': 488}
- **TF Candidatos**: {15: 49252, 240: 23202, 5: 20059, 60: 19211}
- **TF Seleccionados**: {240: 3826, 15: 1725, 60: 1063, 5: 1110, -1: 531}
- **DistATR** - Candidatos: avg=29.0 | Seleccionados: avg=14.1
- **RR** - Candidatos: avg=6.07 | Seleccionados: avg=1.20
- **Razones de selección**: {'BestIntelligentScore': 8255}

### 🎯 Recomendaciones
- ⚠️ SL: 100% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.