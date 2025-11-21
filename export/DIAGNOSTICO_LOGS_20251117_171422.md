# Informe Diagnóstico de Logs - 2025-11-17 17:28:54

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_171422.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251117_171422.csv`

## DFM
- Eventos de evaluación: 221
- Evaluaciones Bull: 7 | Bear: 167
- Pasaron umbral (PassedThreshold): 174
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:5, 6:57, 7:87, 8:25, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 22106
- KeptAligned: 1211/1211 | KeptCounter: 1482/1568
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.021 | AvgProxCounter≈ 0.021
  - AvgDistATRAligned≈ 0.08 | AvgDistATRCounter≈ 0.11
- PreferAligned eventos: 566 | Filtradas contra-bias: 371

### Proximity (Pre-PreferAligned)
- Eventos: 22106
- Aligned pre: 1211/2693 | Counter pre: 1482/2693
- AvgProxAligned(pre)≈ 0.021 | AvgDistATRAligned(pre)≈ 0.08

### Proximity Drivers
- Eventos: 22106
- Alineadas: n=1211 | BaseProx≈ 0.745 | ZoneATR≈ 6.34 | SizePenalty≈ 0.952 | FinalProx≈ 0.710
- Contra-bias: n=1111 | BaseProx≈ 0.503 | ZoneATR≈ 7.76 | SizePenalty≈ 0.931 | FinalProx≈ 0.468

## Risk
- Eventos: 1305
- Accepted=241 | RejSL=0 | RejTP=0 | RejRR=265 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 2 (0.2% del total)
  - Avg Score: 0.44 | Avg R:R: 1.29 | Avg DistATR: 4.00
  - Por TF: TF5=2
- **P0_SWING_LITE:** 996 (99.8% del total)
  - Avg Score: 0.61 | Avg R:R: 3.61 | Avg DistATR: 3.88
  - Por TF: TF15=208, TF60=788


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 6 | Unmatched: 235
- 0-10: Wins=6 Losses=0 WR=100.0% (n=6)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=6 Losses=0 WR=100.0% (n=6)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 241 | Aligned=136 (56.4%)
- Core≈ 1.00 | Prox≈ 0.60 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 3.05 | Confidence≈ 0.00
- SL_TF dist: {'15': 188, '1440': 34, '60': 14, '5': 5} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 67, '60': 23, '240': 147, '5': 4} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=241, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=5, 15m=188, 60m=14, 240m=0, 1440m=34
- RR plan por bandas: 0-10≈ 3.05 (n=241), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 66605 | Zonas con Anchors: 66605
- Dir zonas (zona): Bull=11457 Bear=47590 Neutral=7558
- Resumen por ciclo (promedios): TotHZ≈ 2.8, WithAnchors≈ 2.8, DirBull≈ 0.5, DirBear≈ 2.0, DirNeutral≈ 0.3
- Razones de dirección: {'tie-bias': 8429, 'anchors+triggers': 58176}
- TF Triggers: {'5': 53621, '15': 12984}
- TF Anchors: {'60': 66605, '240': 66605, '1440': 66605}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,22': 1}

## CSV de Trades
- Filas: 18 | Ejecutadas: 1 | Canceladas: 0 | Expiradas: 0
- BUY: 4 | SELL: 15

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 174
- Registered: 9
  - DEDUP_COOLDOWN: 19 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 2
- Intentos de registro: 30

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 17.2%
- RegRate = Registered / Intentos = 30.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 63.3%
- Concurrency = SKIP_CONCURRENCY / Intentos = 6.7%
- ExecRate = Ejecutadas / Registered = 11.1%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 2110 | Total candidatos: 19446 | Seleccionados: 0
- Candidatos por zona (promedio): 9.2

### Take Profit (TP)
- Zonas analizadas: 2110 | Total candidatos: 32949 | Seleccionados: 2110
- Candidatos por zona (promedio): 15.6
- **Edad (barras)** - Candidatos: med=24, max=92 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 3238}
- **Priority Seleccionados**: {'P0': 179, 'P3': 1641, 'NA': 290}
- **Type Candidatos**: {'Swing': 3238}
- **Type Seleccionados**: {'P0_Zone': 179, 'P3_Swing': 1641, 'P4_Fallback': 290}
- **TF Candidatos**: {240: 2713, 60: 368, 15: 105, 5: 52}
- **TF Seleccionados**: {15: 480, 240: 1007, 5: 224, -1: 290, 60: 109}
- **DistATR** - Candidatos: avg=11.0 | Seleccionados: avg=11.2
- **RR** - Candidatos: avg=3.84 | Seleccionados: avg=1.66
- **Razones de selección**: {'BestIntelligentScore': 2110}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.