# Informe Diagnóstico de Logs - 2025-11-12 10:20:54

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_101729.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251112_101729.csv`

## DFM
- Eventos de evaluación: 146
- Evaluaciones Bull: 36 | Bear: 125
- Pasaron umbral (PassedThreshold): 147
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:1, 4:13, 5:9, 6:73, 7:53, 8:12, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 1842
- KeptAligned: 1410/1410 | KeptCounter: 599/599
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.382 | AvgProxCounter≈ 0.172
  - AvgDistATRAligned≈ 0.58 | AvgDistATRCounter≈ 0.27
- PreferAligned eventos: 610 | Filtradas contra-bias: 66

### Proximity (Pre-PreferAligned)
- Eventos: 1842
- Aligned pre: 1410/2009 | Counter pre: 599/2009
- AvgProxAligned(pre)≈ 0.382 | AvgDistATRAligned(pre)≈ 0.58

### Proximity Drivers
- Eventos: 1842
- Alineadas: n=1410 | BaseProx≈ 0.875 | ZoneATR≈ 5.71 | SizePenalty≈ 0.970 | FinalProx≈ 0.848
- Contra-bias: n=533 | BaseProx≈ 0.772 | ZoneATR≈ 4.85 | SizePenalty≈ 0.978 | FinalProx≈ 0.755

## Risk
- Eventos: 1074
- Accepted=161 | RejSL=0 | RejTP=0 | RejRR=141 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 31 (9.1% del total)
  - Avg Score: 0.41 | Avg R:R: 1.97 | Avg DistATR: 3.90
  - Por TF: TF5=11, TF15=20
- **P0_SWING_LITE:** 308 (90.9% del total)
  - Avg Score: 0.51 | Avg R:R: 3.96 | Avg DistATR: 3.47
  - Por TF: TF15=56, TF60=252


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 39 | Unmatched: 123
- 0-10: Wins=8 Losses=31 WR=20.5% (n=39)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=8 Losses=31 WR=20.5% (n=39)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 162 | Aligned=91 (56.2%)
- Core≈ 1.00 | Prox≈ 0.80 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.74 | Confidence≈ 0.00
- SL_TF dist: {'60': 8, '15': 119, '5': 35} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 75, '60': 48, '5': 27, '240': 12} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=161, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=34, 15m=119, 60m=8, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.74 (n=161), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 108993 | Zonas con Anchors: 108050
- Dir zonas (zona): Bull=39713 Bear=68384 Neutral=896
- Resumen por ciclo (promedios): TotHZ≈ 2.0, WithAnchors≈ 1.9, DirBull≈ 0.7, DirBear≈ 1.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 107075, 'tie-bias': 1025, 'triggers-only': 893}
- TF Triggers: {'15': 1744, '5': 3164}
- TF Anchors: {'60': 4800, '240': 2663, '1440': 32}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 2}
- Cancel_BOS (diag): por acción {'BUY': 1, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 1, 'Neutral': 0}

## CSV de Trades
- Filas: 89 | Ejecutadas: 34 | Canceladas: 0 | Expiradas: 0
- BUY: 40 | SELL: 83

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 147
- Registered: 46
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 11 | SKIP_CONCURRENCY: 4
- Intentos de registro: 61

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 41.5%
- RegRate = Registered / Intentos = 75.4%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 18.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 6.6%
- ExecRate = Ejecutadas / Registered = 73.9%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 492 | Total candidatos: 7893 | Seleccionados: 0
- Candidatos por zona (promedio): 16.0

### Take Profit (TP)
- Zonas analizadas: 483 | Total candidatos: 3615 | Seleccionados: 483
- Candidatos por zona (promedio): 7.5
- **Edad (barras)** - Candidatos: med=53, max=180 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.38 | Seleccionados: avg=0.69
- **Priority Candidatos**: {'P3': 3615}
- **Priority Seleccionados**: {'P0': 51, 'P3': 225, 'NA': 207}
- **Type Candidatos**: {'Swing': 3615}
- **Type Seleccionados**: {'P0_Zone': 51, 'P3_Swing': 225, 'P4_Fallback': 207}
- **TF Candidatos**: {60: 1293, 15: 917, 5: 783, 240: 622}
- **TF Seleccionados**: {15: 117, 60: 76, -1: 207, 5: 54, 240: 29}
- **DistATR** - Candidatos: avg=11.8 | Seleccionados: avg=3.6
- **RR** - Candidatos: avg=5.88 | Seleccionados: avg=1.31
- **Razones de selección**: {'BestIntelligentScore': 483}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.