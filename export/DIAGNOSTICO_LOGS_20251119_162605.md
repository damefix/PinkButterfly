# Informe Diagnóstico de Logs - 2025-11-19 16:32:30

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_162605.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_162605.csv`

## DFM
- Eventos de evaluación: 560
- Evaluaciones Bull: 0 | Bear: 491
- Pasaron umbral (PassedThreshold): 491
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:9, 6:166, 7:201, 8:115, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3098
- KeptAligned: 2320/2320 | KeptCounter: 2237/2387
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.182 | AvgProxCounter≈ 0.132
  - AvgDistATRAligned≈ 0.79 | AvgDistATRCounter≈ 0.65
- PreferAligned eventos: 799 | Filtradas contra-bias: 6

### Proximity (Pre-PreferAligned)
- Eventos: 3098
- Aligned pre: 2320/4557 | Counter pre: 2237/4557
- AvgProxAligned(pre)≈ 0.182 | AvgDistATRAligned(pre)≈ 0.79

### Proximity Drivers
- Eventos: 3098
- Alineadas: n=2320 | BaseProx≈ 0.722 | ZoneATR≈ 4.95 | SizePenalty≈ 0.979 | FinalProx≈ 0.707
- Contra-bias: n=2231 | BaseProx≈ 0.517 | ZoneATR≈ 6.09 | SizePenalty≈ 0.962 | FinalProx≈ 0.498

## Risk
- Eventos: 1617
- Accepted=708 | RejSL=0 | RejTP=0 | RejRR=1125 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 489 (14.0% del total)
  - Avg Score: 0.40 | Avg R:R: 1.80 | Avg DistATR: 3.60
  - Por TF: TF5=36, TF15=453
- **P0_SWING_LITE:** 2994 (86.0% del total)
  - Avg Score: 0.60 | Avg R:R: 3.44 | Avg DistATR: 3.96
  - Por TF: TF15=526, TF60=2468


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 137 | Unmatched: 571
- 0-10: Wins=111 Losses=26 WR=81.0% (n=137)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=111 Losses=26 WR=81.0% (n=137)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 708 | Aligned=419 (59.2%)
- Core≈ 1.00 | Prox≈ 0.61 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.06 | Confidence≈ 0.00
- SL_TF dist: {'15': 373, '5': 288, '60': 35, '240': 12} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 204, '240': 377, '5': 92, '60': 35} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=708, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=288, 15m=373, 60m=35, 240m=12, 1440m=0
- RR plan por bandas: 0-10≈ 2.06 (n=708), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 16436 | Zonas con Anchors: 16436
- Dir zonas (zona): Bull=40 Bear=16335 Neutral=61
- Resumen por ciclo (promedios): TotHZ≈ 5.3, WithAnchors≈ 5.3, DirBull≈ 0.0, DirBear≈ 5.2, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 16365, 'tie-bias': 71}
- TF Triggers: {'15': 8384, '5': 8052}
- TF Anchors: {'60': 16436, '240': 16436, '1440': 16436}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 5, 'score decayó a 0,49': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 10} | por bias {'Bullish': 10, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 86 | Ejecutadas: 11 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 97

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 491
- Registered: 45
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 45

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 9.2%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 24.4%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4367 | Total candidatos: 65781 | Seleccionados: 0
- Candidatos por zona (promedio): 15.1

### Take Profit (TP)
- Zonas analizadas: 4367 | Total candidatos: 99130 | Seleccionados: 4367
- Candidatos por zona (promedio): 22.7
- **Edad (barras)** - Candidatos: med=83, max=468 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.54 | Seleccionados: avg=0.72
- **Priority Candidatos**: {'P3': 60083}
- **Priority Seleccionados**: {'P3': 3240, 'P0': 361, 'NA': 766}
- **Type Candidatos**: {'Swing': 60083}
- **Type Seleccionados**: {'P3_Swing': 3240, 'P0_Zone': 361, 'P4_Fallback': 766}
- **TF Candidatos**: {240: 38423, 60: 9853, 15: 6917, 5: 4890}
- **TF Seleccionados**: {240: 2253, 15: 653, -1: 766, 60: 282, 5: 413}
- **DistATR** - Candidatos: avg=17.4 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=6.99 | Seleccionados: avg=1.58
- **Razones de selección**: {'BestIntelligentScore': 4367}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.