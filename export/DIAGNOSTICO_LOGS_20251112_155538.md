# Informe Diagnóstico de Logs - 2025-11-12 16:04:23

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_155538.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251112_155538.csv`

## DFM
- Eventos de evaluación: 620
- Evaluaciones Bull: 240 | Bear: 521
- Pasaron umbral (PassedThreshold): 725
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:3, 4:26, 5:78, 6:307, 7:300, 8:47, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2347
- KeptAligned: 2161/2161 | KeptCounter: 1416/1416
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.383 | AvgProxCounter≈ 0.231
  - AvgDistATRAligned≈ 0.57 | AvgDistATRCounter≈ 0.40
- PreferAligned eventos: 950 | Filtradas contra-bias: 249

### Proximity (Pre-PreferAligned)
- Eventos: 2347
- Aligned pre: 2161/3577 | Counter pre: 1416/3577
- AvgProxAligned(pre)≈ 0.383 | AvgDistATRAligned(pre)≈ 0.57

### Proximity Drivers
- Eventos: 2347
- Alineadas: n=2161 | BaseProx≈ 0.870 | ZoneATR≈ 4.79 | SizePenalty≈ 0.980 | FinalProx≈ 0.853
- Contra-bias: n=1167 | BaseProx≈ 0.751 | ZoneATR≈ 4.66 | SizePenalty≈ 0.981 | FinalProx≈ 0.738

## Risk
- Eventos: 1525
- Accepted=761 | RejSL=0 | RejTP=0 | RejRR=830 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 144 (8.7% del total)
  - Avg Score: 0.42 | Avg R:R: 1.98 | Avg DistATR: 3.61
  - Por TF: TF5=41, TF15=103
- **P0_SWING_LITE:** 1513 (91.3% del total)
  - Avg Score: 0.57 | Avg R:R: 4.06 | Avg DistATR: 3.47
  - Por TF: TF15=350, TF60=1163


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 66 | Unmatched: 699
- 0-10: Wins=17 Losses=49 WR=25.8% (n=66)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=17 Losses=49 WR=25.8% (n=66)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 765 | Aligned=454 (59.3%)
- Core≈ 1.00 | Prox≈ 0.82 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.99 | Confidence≈ 0.00
- SL_TF dist: {'15': 606, '60': 56, '5': 99, '240': 4} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 192, '60': 169, '15': 315, '240': 89} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=761, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=99, 15m=602, 60m=56, 240m=4, 1440m=0
- RR plan por bandas: 0-10≈ 1.99 (n=761), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10192 | Zonas con Anchors: 10178
- Dir zonas (zona): Bull=3846 Bear=5987 Neutral=359
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.4, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9756, 'tie-bias': 422, 'triggers-only': 14}
- TF Triggers: {'5': 5466, '15': 4726}
- TF Anchors: {'60': 10105, '240': 5902, '1440': 716}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 7, 'score decayó a 0,42': 2}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 140 | Ejecutadas: 36 | Canceladas: 0 | Expiradas: 0
- BUY: 79 | SELL: 97

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 725
- Registered: 76
  - DEDUP_COOLDOWN: 7 | DEDUP_IDENTICAL: 65 | SKIP_CONCURRENCY: 53
- Intentos de registro: 201

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 27.7%
- RegRate = Registered / Intentos = 37.8%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 35.8%
- Concurrency = SKIP_CONCURRENCY / Intentos = 26.4%
- ExecRate = Ejecutadas / Registered = 47.4%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 2942 | Total candidatos: 26393 | Seleccionados: 0
- Candidatos por zona (promedio): 9.0

### Take Profit (TP)
- Zonas analizadas: 2908 | Total candidatos: 26564 | Seleccionados: 2908
- Candidatos por zona (promedio): 9.1
- **Edad (barras)** - Candidatos: med=40, max=183 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.70
- **Priority Candidatos**: {'P3': 26564}
- **Priority Seleccionados**: {'P3': 1967, 'NA': 728, 'P0': 213}
- **Type Candidatos**: {'Swing': 26564}
- **Type Seleccionados**: {'P3_Swing': 1967, 'P4_Fallback': 728, 'P0_Zone': 213}
- **TF Candidatos**: {5: 7659, 60: 7325, 15: 6789, 240: 4791}
- **TF Seleccionados**: {60: 626, 5: 520, 15: 673, -1: 728, 240: 361}
- **DistATR** - Candidatos: avg=8.9 | Seleccionados: avg=4.8
- **RR** - Candidatos: avg=3.48 | Seleccionados: avg=1.32
- **Razones de selección**: {'BestIntelligentScore': 2908}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.