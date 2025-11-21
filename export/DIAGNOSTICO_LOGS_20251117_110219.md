# Informe Diagnóstico de Logs - 2025-11-17 11:12:45

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_110219.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251117_110219.csv`

## DFM
- Eventos de evaluación: 774
- Evaluaciones Bull: 66 | Bear: 293
- Pasaron umbral (PassedThreshold): 359
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:39, 6:104, 7:168, 8:48, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 25848
- KeptAligned: 1739/1739 | KeptCounter: 3030/3240
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.036 | AvgProxCounter≈ 0.041
  - AvgDistATRAligned≈ 0.14 | AvgDistATRCounter≈ 0.19
- PreferAligned eventos: 1016 | Filtradas contra-bias: 957

### Proximity (Pre-PreferAligned)
- Eventos: 25848
- Aligned pre: 1739/4769 | Counter pre: 3030/4769
- AvgProxAligned(pre)≈ 0.036 | AvgDistATRAligned(pre)≈ 0.14

### Proximity Drivers
- Eventos: 25848
- Alineadas: n=1739 | BaseProx≈ 0.734 | ZoneATR≈ 4.84 | SizePenalty≈ 0.974 | FinalProx≈ 0.714
- Contra-bias: n=2073 | BaseProx≈ 0.521 | ZoneATR≈ 4.49 | SizePenalty≈ 0.973 | FinalProx≈ 0.506

## Risk
- Eventos: 2499
- Accepted=807 | RejSL=0 | RejTP=0 | RejRR=432 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 50 (3.0% del total)
  - Avg Score: 0.35 | Avg R:R: 2.50 | Avg DistATR: 3.97
  - Por TF: TF15=50
- **P0_SWING_LITE:** 1626 (97.0% del total)
  - Avg Score: 0.54 | Avg R:R: 6.65 | Avg DistATR: 3.84
  - Por TF: TF15=337, TF60=1289


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 308 | Unmatched: 499
- 0-10: Wins=34 Losses=274 WR=11.0% (n=308)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=34 Losses=274 WR=11.0% (n=308)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 807 | Aligned=319 (39.5%)
- Core≈ 1.00 | Prox≈ 0.60 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.06 | Confidence≈ 0.00
- SL_TF dist: {'15': 764, '1440': 7, '60': 16, '5': 20} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 81, '60': 9, '240': 112, '15': 605} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=807, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=20, 15m=764, 60m=16, 240m=0, 1440m=7
- RR plan por bandas: 0-10≈ 2.06 (n=807), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 79671 | Zonas con Anchors: 79671
- Dir zonas (zona): Bull=34423 Bear=30072 Neutral=15176
- Resumen por ciclo (promedios): TotHZ≈ 3.1, WithAnchors≈ 3.1, DirBull≈ 1.3, DirBear≈ 1.2, DirNeutral≈ 0.6
- Razones de dirección: {'anchors+triggers': 61767, 'tie-bias': 17904}
- TF Triggers: {'5': 61324, '15': 18347}
- TF Anchors: {'60': 79671, '240': 79671, '1440': 79671}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,27': 1, 'score decayó a 0,24': 2}

## CSV de Trades
- Filas: 42 | Ejecutadas: 12 | Canceladas: 0 | Expiradas: 0
- BUY: 16 | SELL: 38

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 359
- Registered: 21
  - DEDUP_COOLDOWN: 54 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 6
- Intentos de registro: 81

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 22.6%
- RegRate = Registered / Intentos = 25.9%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 66.7%
- Concurrency = SKIP_CONCURRENCY / Intentos = 7.4%
- ExecRate = Ejecutadas / Registered = 57.1%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 2953 | Total candidatos: 27415 | Seleccionados: 0
- Candidatos por zona (promedio): 9.3

### Take Profit (TP)
- Zonas analizadas: 2949 | Total candidatos: 50755 | Seleccionados: 2949
- Candidatos por zona (promedio): 17.2
- **Edad (barras)** - Candidatos: med=33, max=91 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.70
- **Priority Candidatos**: {'P3': 6777}
- **Priority Seleccionados**: {'P0': 205, 'NA': 726, 'P3': 2018}
- **Type Candidatos**: {'Swing': 6777}
- **Type Seleccionados**: {'P0_Zone': 205, 'P4_Fallback': 726, 'P3_Swing': 2018}
- **TF Candidatos**: {240: 5401, 60: 889, 15: 407, 5: 80}
- **TF Seleccionados**: {5: 97, -1: 726, 60: 11, 240: 1325, 15: 790}
- **DistATR** - Candidatos: avg=15.0 | Seleccionados: avg=9.3
- **RR** - Candidatos: avg=7.64 | Seleccionados: avg=1.58
- **Razones de selección**: {'BestIntelligentScore': 2949}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.