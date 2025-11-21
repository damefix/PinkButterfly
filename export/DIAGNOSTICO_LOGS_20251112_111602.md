# Informe Diagnóstico de Logs - 2025-11-12 11:20:07

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_111602.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251112_111602.csv`

## DFM
- Eventos de evaluación: 184
- Evaluaciones Bull: 58 | Bear: 148
- Pasaron umbral (PassedThreshold): 188
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:17, 5:6, 6:77, 7:85, 8:21, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2218
- KeptAligned: 1816/1816 | KeptCounter: 1218/1218
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.359 | AvgProxCounter≈ 0.265
  - AvgDistATRAligned≈ 0.50 | AvgDistATRCounter≈ 0.36
- PreferAligned eventos: 789 | Filtradas contra-bias: 190

### Proximity (Pre-PreferAligned)
- Eventos: 2218
- Aligned pre: 1816/3034 | Counter pre: 1218/3034
- AvgProxAligned(pre)≈ 0.359 | AvgDistATRAligned(pre)≈ 0.50

### Proximity Drivers
- Eventos: 2218
- Alineadas: n=1816 | BaseProx≈ 0.933 | ZoneATR≈ 5.14 | SizePenalty≈ 0.974 | FinalProx≈ 0.909
- Contra-bias: n=1028 | BaseProx≈ 0.876 | ZoneATR≈ 4.90 | SizePenalty≈ 0.976 | FinalProx≈ 0.855

## Risk
- Eventos: 1355
- Accepted=206 | RejSL=0 | RejTP=0 | RejRR=141 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 33 (7.5% del total)
  - Avg Score: 0.43 | Avg R:R: 2.00 | Avg DistATR: 3.77
  - Por TF: TF5=7, TF15=26
- **P0_SWING_LITE:** 405 (92.5% del total)
  - Avg Score: 0.57 | Avg R:R: 3.95 | Avg DistATR: 3.53
  - Por TF: TF15=81, TF60=324


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 55 | Unmatched: 152
- 0-10: Wins=19 Losses=36 WR=34.5% (n=55)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=19 Losses=36 WR=34.5% (n=55)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 207 | Aligned=124 (59.9%)
- Core≈ 1.00 | Prox≈ 0.90 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.75 | Confidence≈ 0.00
- SL_TF dist: {'15': 166, '5': 41} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 96, '60': 50, '5': 48, '240': 13} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=206, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=41, 15m=165, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.76 (n=206), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 9896 | Zonas con Anchors: 9883
- Dir zonas (zona): Bull=3769 Bear=5742 Neutral=385
- Resumen por ciclo (promedios): TotHZ≈ 4.0, WithAnchors≈ 4.0, DirBull≈ 1.5, DirBear≈ 2.3, DirNeutral≈ 0.2
- Razones de dirección: {'tie-bias': 432, 'anchors+triggers': 9451, 'triggers-only': 13}
- TF Triggers: {'5': 5789, '15': 4107}
- TF Anchors: {'60': 9845, '240': 5989, '1440': 742}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,39': 2, 'estructura no existe': 2, 'score decayó a 0,45': 1, 'score decayó a 0,23': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 114 | Ejecutadas: 37 | Canceladas: 0 | Expiradas: 0
- BUY: 54 | SELL: 97

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 188
- Registered: 59
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 31 | SKIP_CONCURRENCY: 2
- Intentos de registro: 92

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 48.9%
- RegRate = Registered / Intentos = 64.1%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 33.7%
- Concurrency = SKIP_CONCURRENCY / Intentos = 2.2%
- ExecRate = Ejecutadas / Registered = 62.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 576 | Total candidatos: 10974 | Seleccionados: 0
- Candidatos por zona (promedio): 19.1

### Take Profit (TP)
- Zonas analizadas: 566 | Total candidatos: 5105 | Seleccionados: 566
- Candidatos por zona (promedio): 9.0
- **Edad (barras)** - Candidatos: med=55, max=256 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.71
- **Priority Candidatos**: {'P3': 5105}
- **Priority Seleccionados**: {'P0': 58, 'P3': 278, 'NA': 230}
- **Type Candidatos**: {'Swing': 5105}
- **Type Seleccionados**: {'P0_Zone': 58, 'P3_Swing': 278, 'P4_Fallback': 230}
- **TF Candidatos**: {60: 1741, 15: 1344, 5: 1235, 240: 785}
- **TF Seleccionados**: {5: 80, 60: 86, 15: 141, -1: 230, 240: 29}
- **DistATR** - Candidatos: avg=9.1 | Seleccionados: avg=3.9
- **RR** - Candidatos: avg=4.26 | Seleccionados: avg=1.37
- **Razones de selección**: {'BestIntelligentScore': 566}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.