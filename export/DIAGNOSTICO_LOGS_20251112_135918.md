# Informe Diagnóstico de Logs - 2025-11-12 14:01:47

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_135918.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251112_135918.csv`

## DFM
- Eventos de evaluación: 237
- Evaluaciones Bull: 95 | Bear: 177
- Pasaron umbral (PassedThreshold): 257
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:4, 4:11, 5:32, 6:131, 7:82, 8:12, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2347
- KeptAligned: 2140/2140 | KeptCounter: 1427/1427
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.379 | AvgProxCounter≈ 0.231
  - AvgDistATRAligned≈ 0.57 | AvgDistATRCounter≈ 0.39
- PreferAligned eventos: 941 | Filtradas contra-bias: 228

### Proximity (Pre-PreferAligned)
- Eventos: 2347
- Aligned pre: 2140/3567 | Counter pre: 1427/3567
- AvgProxAligned(pre)≈ 0.379 | AvgDistATRAligned(pre)≈ 0.57

### Proximity Drivers
- Eventos: 2347
- Alineadas: n=2140 | BaseProx≈ 0.870 | ZoneATR≈ 4.80 | SizePenalty≈ 0.980 | FinalProx≈ 0.853
- Contra-bias: n=1199 | BaseProx≈ 0.750 | ZoneATR≈ 4.68 | SizePenalty≈ 0.981 | FinalProx≈ 0.737

## Risk
- Eventos: 1554
- Accepted=272 | RejSL=0 | RejTP=0 | RejRR=189 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 38 (7.6% del total)
  - Avg Score: 0.40 | Avg R:R: 1.77 | Avg DistATR: 3.40
  - Por TF: TF5=10, TF15=28
- **P0_SWING_LITE:** 464 (92.4% del total)
  - Avg Score: 0.54 | Avg R:R: 4.55 | Avg DistATR: 3.34
  - Por TF: TF15=122, TF60=342


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 56 | Unmatched: 216
- 0-10: Wins=21 Losses=35 WR=37.5% (n=56)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=21 Losses=35 WR=37.5% (n=56)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 272 | Aligned=136 (50.0%)
- Core≈ 1.00 | Prox≈ 0.80 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.86 | Confidence≈ 0.00
- SL_TF dist: {'60': 21, '5': 59, '15': 192} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 149, '60': 51, '5': 61, '240': 11} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=272, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=59, 15m=192, 60m=21, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.87 (n=272), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10221 | Zonas con Anchors: 10207
- Dir zonas (zona): Bull=3890 Bear=5975 Neutral=356
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.6, DirBear≈ 2.4, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9785, 'tie-bias': 422, 'triggers-only': 14}
- TF Triggers: {'15': 4751, '5': 5470}
- TF Anchors: {'60': 10133, '240': 5865, '1440': 696}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 7}
- Cancel_BOS (diag): por acción {'BUY': 1, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 1, 'Neutral': 0}

## CSV de Trades
- Filas: 123 | Ejecutadas: 35 | Canceladas: 0 | Expiradas: 0
- BUY: 72 | SELL: 86

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 257
- Registered: 66
  - DEDUP_COOLDOWN: 6 | DEDUP_IDENTICAL: 38 | SKIP_CONCURRENCY: 22
- Intentos de registro: 132

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 51.4%
- RegRate = Registered / Intentos = 50.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 33.3%
- Concurrency = SKIP_CONCURRENCY / Intentos = 16.7%
- ExecRate = Ejecutadas / Registered = 53.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 718 | Total candidatos: 8972 | Seleccionados: 0
- Candidatos por zona (promedio): 12.5

### Take Profit (TP)
- Zonas analizadas: 712 | Total candidatos: 4826 | Seleccionados: 712
- Candidatos por zona (promedio): 6.8
- **Edad (barras)** - Candidatos: med=44, max=156 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.71
- **Priority Candidatos**: {'P3': 4826}
- **Priority Seleccionados**: {'P0': 96, 'P3': 311, 'NA': 305}
- **Type Candidatos**: {'Swing': 4826}
- **Type Seleccionados**: {'P0_Zone': 96, 'P3_Swing': 311, 'P4_Fallback': 305}
- **TF Candidatos**: {15: 1424, 60: 1391, 5: 1068, 240: 943}
- **TF Seleccionados**: {15: 198, 60: 86, -1: 305, 5: 89, 240: 34}
- **DistATR** - Candidatos: avg=9.2 | Seleccionados: avg=3.7
- **RR** - Candidatos: avg=4.54 | Seleccionados: avg=1.38
- **Razones de selección**: {'BestIntelligentScore': 712}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.