# Informe Diagnóstico de Logs - 2025-11-18 16:10:53

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251118_160604.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251118_160604.csv`

## DFM
- Eventos de evaluación: 249
- Evaluaciones Bull: 12 | Bear: 190
- Pasaron umbral (PassedThreshold): 202
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:3, 6:60, 7:84, 8:55, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 626
- KeptAligned: 958/958 | KeptCounter: 1085/1120
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.383 | AvgProxCounter≈ 0.251
  - AvgDistATRAligned≈ 1.32 | AvgDistATRCounter≈ 1.68
- PreferAligned eventos: 269 | Filtradas contra-bias: 153

### Proximity (Pre-PreferAligned)
- Eventos: 626
- Aligned pre: 958/2043 | Counter pre: 1085/2043
- AvgProxAligned(pre)≈ 0.383 | AvgDistATRAligned(pre)≈ 1.32

### Proximity Drivers
- Eventos: 626
- Alineadas: n=958 | BaseProx≈ 0.772 | ZoneATR≈ 4.93 | SizePenalty≈ 0.980 | FinalProx≈ 0.756
- Contra-bias: n=932 | BaseProx≈ 0.440 | ZoneATR≈ 4.68 | SizePenalty≈ 0.980 | FinalProx≈ 0.432

## Risk
- Eventos: 556
- Accepted=329 | RejSL=0 | RejTP=0 | RejRR=325 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 81 (10.6% del total)
  - Avg Score: 0.36 | Avg R:R: 1.87 | Avg DistATR: 3.72
  - Por TF: TF5=28, TF15=53
- **P0_SWING_LITE:** 683 (89.4% del total)
  - Avg Score: 0.62 | Avg R:R: 5.44 | Avg DistATR: 3.61
  - Por TF: TF15=142, TF60=541


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 15 | Unmatched: 321
- 0-10: Wins=3 Losses=12 WR=20.0% (n=15)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=3 Losses=12 WR=20.0% (n=15)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 336 | Aligned=152 (45.2%)
- Core≈ 1.00 | Prox≈ 0.61 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.47 | Confidence≈ 0.00
- SL_TF dist: {'60': 35, '15': 226, '5': 51, '240': 24} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 43, '15': 111, '5': 91, '240': 91} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=329, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=49, 15m=225, 60m=35, 240m=20, 1440m=0
- RR plan por bandas: 0-10≈ 2.37 (n=329), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 3202 | Zonas con Anchors: 3202
- Dir zonas (zona): Bull=371 Bear=2716 Neutral=115
- Resumen por ciclo (promedios): TotHZ≈ 5.1, WithAnchors≈ 5.1, DirBull≈ 0.6, DirBear≈ 4.3, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 3070, 'tie-bias': 132}
- TF Triggers: {'15': 1652, '5': 1550}
- TF Anchors: {'60': 3174, '240': 3202, '1440': 3202}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 3, 'score decayó a 0,22': 1, 'score decayó a 0,24': 1}

## CSV de Trades
- Filas: 45 | Ejecutadas: 11 | Canceladas: 0 | Expiradas: 0
- BUY: 7 | SELL: 49

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 202
- Registered: 23
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 23

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 11.4%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 47.8%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 1450 | Total candidatos: 12701 | Seleccionados: 0
- Candidatos por zona (promedio): 8.8

### Take Profit (TP)
- Zonas analizadas: 1436 | Total candidatos: 17630 | Seleccionados: 1436
- Candidatos por zona (promedio): 12.3
- **Edad (barras)** - Candidatos: med=37, max=157 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 17630}
- **Priority Seleccionados**: {'P3': 1082, 'NA': 299, 'P0': 55}
- **Type Candidatos**: {'Swing': 17630}
- **Type Seleccionados**: {'P3_Swing': 1082, 'P4_Fallback': 299, 'P0_Zone': 55}
- **TF Candidatos**: {240: 6409, 5: 4053, 15: 4013, 60: 3155}
- **TF Seleccionados**: {60: 147, -1: 299, 15: 268, 5: 282, 240: 440}
- **DistATR** - Candidatos: avg=12.2 | Seleccionados: avg=6.1
- **RR** - Candidatos: avg=5.82 | Seleccionados: avg=1.46
- **Razones de selección**: {'BestIntelligentScore': 1436}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.