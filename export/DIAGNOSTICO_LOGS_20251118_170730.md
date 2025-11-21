# Informe Diagnóstico de Logs - 2025-11-18 17:13:54

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251118_170730.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251118_170730.csv`

## DFM
- Eventos de evaluación: 172
- Evaluaciones Bull: 0 | Bear: 132
- Pasaron umbral (PassedThreshold): 132
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:0, 6:36, 7:57, 8:39, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 615
- KeptAligned: 730/730 | KeptCounter: 652/692
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.314 | AvgProxCounter≈ 0.256
  - AvgDistATRAligned≈ 1.08 | AvgDistATRCounter≈ 1.48
- PreferAligned eventos: 234 | Filtradas contra-bias: 54

### Proximity (Pre-PreferAligned)
- Eventos: 615
- Aligned pre: 730/1382 | Counter pre: 652/1382
- AvgProxAligned(pre)≈ 0.314 | AvgDistATRAligned(pre)≈ 1.08

### Proximity Drivers
- Eventos: 615
- Alineadas: n=730 | BaseProx≈ 0.768 | ZoneATR≈ 5.65 | SizePenalty≈ 0.972 | FinalProx≈ 0.747
- Contra-bias: n=598 | BaseProx≈ 0.474 | ZoneATR≈ 5.51 | SizePenalty≈ 0.970 | FinalProx≈ 0.461

## Risk
- Eventos: 532
- Accepted=194 | RejSL=0 | RejTP=0 | RejRR=224 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 55 (7.2% del total)
  - Avg Score: 0.39 | Avg R:R: 1.99 | Avg DistATR: 4.17
  - Por TF: TF5=39, TF15=16
- **P0_SWING_LITE:** 706 (92.8% del total)
  - Avg Score: 0.60 | Avg R:R: 4.87 | Avg DistATR: 3.68
  - Por TF: TF15=228, TF60=478


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 4 | Unmatched: 190
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
- Muestras: 194 | Aligned=104 (53.6%)
- Core≈ 1.00 | Prox≈ 0.65 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.00 | Confidence≈ 0.00
- SL_TF dist: {'60': 43, '15': 30, '5': 105, '240': 16} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 33, '15': 21, '5': 113, '240': 27} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=194, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=105, 15m=30, 60m=43, 240m=16, 1440m=0
- RR plan por bandas: 0-10≈ 2.00 (n=194), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 2415 | Zonas con Anchors: 2415
- Dir zonas (zona): Bull=310 Bear=2046 Neutral=59
- Resumen por ciclo (promedios): TotHZ≈ 3.9, WithAnchors≈ 3.9, DirBull≈ 0.5, DirBear≈ 3.3, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 2325, 'tie-bias': 90}
- TF Triggers: {'15': 849, '5': 1566}
- TF Anchors: {'60': 2306, '240': 2415, '1440': 2415}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 4}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 33} | por bias {'Bullish': 33, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 82 | Ejecutadas: 3 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 85

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 132
- Registered: 42
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 42

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 31.8%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 7.1%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 1204 | Total candidatos: 15528 | Seleccionados: 0
- Candidatos por zona (promedio): 12.9

### Take Profit (TP)
- Zonas analizadas: 1204 | Total candidatos: 22986 | Seleccionados: 1204
- Candidatos por zona (promedio): 19.1
- **Edad (barras)** - Candidatos: med=226, max=661 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.66
- **Priority Candidatos**: {'P3': 22986}
- **Priority Seleccionados**: {'P3': 750, 'NA': 391, 'P0': 63}
- **Type Candidatos**: {'Swing': 22986}
- **Type Seleccionados**: {'P3_Swing': 750, 'P4_Fallback': 391, 'P0_Zone': 63}
- **TF Candidatos**: {15: 13958, 240: 4037, 5: 2919, 60: 2072}
- **TF Seleccionados**: {60: 115, -1: 391, 15: 169, 5: 316, 240: 213}
- **DistATR** - Candidatos: avg=18.4 | Seleccionados: avg=6.4
- **RR** - Candidatos: avg=7.60 | Seleccionados: avg=1.30
- **Razones de selección**: {'BestIntelligentScore': 1204}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.