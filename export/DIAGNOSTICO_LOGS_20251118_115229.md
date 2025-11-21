# Informe Diagnóstico de Logs - 2025-11-18 12:00:58

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251118_115229.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251118_115229.csv`

## DFM
- Eventos de evaluación: 575
- Evaluaciones Bull: 58 | Bear: 404
- Pasaron umbral (PassedThreshold): 462
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:11, 6:138, 7:221, 8:92, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2595
- KeptAligned: 2836/2836 | KeptCounter: 3073/3208
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.301 | AvgProxCounter≈ 0.228
  - AvgDistATRAligned≈ 0.96 | AvgDistATRCounter≈ 1.23
- PreferAligned eventos: 951 | Filtradas contra-bias: 461

### Proximity (Pre-PreferAligned)
- Eventos: 2595
- Aligned pre: 2836/5909 | Counter pre: 3073/5909
- AvgProxAligned(pre)≈ 0.301 | AvgDistATRAligned(pre)≈ 0.96

### Proximity Drivers
- Eventos: 2595
- Alineadas: n=2836 | BaseProx≈ 0.764 | ZoneATR≈ 4.63 | SizePenalty≈ 0.980 | FinalProx≈ 0.750
- Contra-bias: n=2612 | BaseProx≈ 0.486 | ZoneATR≈ 4.85 | SizePenalty≈ 0.976 | FinalProx≈ 0.475

## Risk
- Eventos: 1884
- Accepted=728 | RejSL=0 | RejTP=0 | RejRR=886 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 270 (11.0% del total)
  - Avg Score: 0.39 | Avg R:R: 1.87 | Avg DistATR: 3.91
  - Por TF: TF5=40, TF15=230
- **P0_SWING_LITE:** 2184 (89.0% del total)
  - Avg Score: 594338273750413952.00 | Avg R:R: 4.40 | Avg DistATR: 3.95
  - Por TF: TF15=628, TF60=1556


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 111 | Unmatched: 629
- 0-10: Wins=55 Losses=56 WR=49.5% (n=111)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=55 Losses=56 WR=49.5% (n=111)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 740 | Aligned=382 (51.6%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.64 | Confidence≈ 0.00
- SL_TF dist: {'15': 514, '60': 116, '5': 71, '240': 39} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 428, '240': 173, '60': 53, '5': 86} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=728, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=69, 15m=508, 60m=116, 240m=35, 1440m=0
- RR plan por bandas: 0-10≈ 2.60 (n=728), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 12301 | Zonas con Anchors: 12301
- Dir zonas (zona): Bull=1415 Bear=10590 Neutral=296
- Resumen por ciclo (promedios): TotHZ≈ 3.9, WithAnchors≈ 3.9, DirBull≈ 0.5, DirBear≈ 3.4, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 11897, 'tie-bias': 404}
- TF Triggers: {'15': 8439, '5': 3862}
- TF Anchors: {'60': 10387, '240': 12301, '1440': 12301}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,47': 2, 'estructura no existe': 9, 'score decayó a 0,46': 1, 'score decayó a 0,18': 1, 'score decayó a 0,22': 1, 'score decayó a 0,24': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 117 | Ejecutadas: 24 | Canceladas: 0 | Expiradas: 0
- BUY: 22 | SELL: 119

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 462
- Registered: 60
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 60

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 13.0%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 40.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4652 | Total candidatos: 37879 | Seleccionados: 0
- Candidatos por zona (promedio): 8.1

### Take Profit (TP)
- Zonas analizadas: 4627 | Total candidatos: 98667 | Seleccionados: 4627
- Candidatos por zona (promedio): 21.3
- **Edad (barras)** - Candidatos: med=32, max=157 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=399646542697140928.00
- **Priority Candidatos**: {'P3': 33080}
- **Priority Seleccionados**: {'NA': 514, 'P3': 3979, 'P0': 134}
- **Type Candidatos**: {'Swing': 33080}
- **Type Seleccionados**: {'P4_Fallback': 514, 'P3_Swing': 3979, 'P0_Zone': 134}
- **TF Candidatos**: {240: 13446, 15: 10600, 60: 4661, 5: 4373}
- **TF Seleccionados**: {-1: 514, 15: 2402, 240: 1271, 60: 138, 5: 302}
- **DistATR** - Candidatos: avg=10.5 | Seleccionados: avg=8.7
- **RR** - Candidatos: avg=5.50 | Seleccionados: avg=3.63
- **Razones de selección**: {'BestIntelligentScore': 4627}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.