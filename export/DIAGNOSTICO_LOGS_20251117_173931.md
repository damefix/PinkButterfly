# Informe Diagnóstico de Logs - 2025-11-17 17:48:23

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_173931.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251117_173931.csv`

## DFM
- Eventos de evaluación: 223
- Evaluaciones Bull: 7 | Bear: 169
- Pasaron umbral (PassedThreshold): 176
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:5, 6:58, 7:88, 8:25, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 22108
- KeptAligned: 1218/1218 | KeptCounter: 1483/1569
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.021 | AvgProxCounter≈ 0.021
  - AvgDistATRAligned≈ 0.08 | AvgDistATRCounter≈ 0.11
- PreferAligned eventos: 568 | Filtradas contra-bias: 372

### Proximity (Pre-PreferAligned)
- Eventos: 22108
- Aligned pre: 1218/2701 | Counter pre: 1483/2701
- AvgProxAligned(pre)≈ 0.021 | AvgDistATRAligned(pre)≈ 0.08

### Proximity Drivers
- Eventos: 22108
- Alineadas: n=1218 | BaseProx≈ 0.746 | ZoneATR≈ 6.33 | SizePenalty≈ 0.952 | FinalProx≈ 0.711
- Contra-bias: n=1111 | BaseProx≈ 0.503 | ZoneATR≈ 7.76 | SizePenalty≈ 0.931 | FinalProx≈ 0.468

## Risk
- Eventos: 1307
- Accepted=243 | RejSL=0 | RejTP=0 | RejRR=266 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 2 (0.2% del total)
  - Avg Score: 0.44 | Avg R:R: 1.29 | Avg DistATR: 4.00
  - Por TF: TF5=2
- **P0_SWING_LITE:** 998 (99.8% del total)
  - Avg Score: 0.61 | Avg R:R: 3.62 | Avg DistATR: 3.88
  - Por TF: TF15=209, TF60=789


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 6 | Unmatched: 237
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
- Muestras: 243 | Aligned=138 (56.8%)
- Core≈ 1.00 | Prox≈ 0.60 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 3.06 | Confidence≈ 0.00
- SL_TF dist: {'15': 190, '1440': 34, '60': 14, '5': 5} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 68, '60': 23, '240': 148, '5': 4} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=243, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=5, 15m=190, 60m=14, 240m=0, 1440m=34
- RR plan por bandas: 0-10≈ 3.06 (n=243), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 66613 | Zonas con Anchors: 66613
- Dir zonas (zona): Bull=11457 Bear=47597 Neutral=7559
- Resumen por ciclo (promedios): TotHZ≈ 2.8, WithAnchors≈ 2.8, DirBull≈ 0.5, DirBear≈ 2.0, DirNeutral≈ 0.3
- Razones de dirección: {'tie-bias': 8430, 'anchors+triggers': 58183}
- TF Triggers: {'5': 53625, '15': 12988}
- TF Anchors: {'60': 66613, '240': 66613, '1440': 66613}

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
- DFM Señales (PassedThreshold): 176
- Registered: 9
  - DEDUP_COOLDOWN: 19 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 2
- Intentos de registro: 30

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 17.0%
- RegRate = Registered / Intentos = 30.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 63.3%
- Concurrency = SKIP_CONCURRENCY / Intentos = 6.7%
- ExecRate = Ejecutadas / Registered = 11.1%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 2113 | Total candidatos: 19474 | Seleccionados: 0
- Candidatos por zona (promedio): 9.2

### Take Profit (TP)
- Zonas analizadas: 2113 | Total candidatos: 32987 | Seleccionados: 2113
- Candidatos por zona (promedio): 15.6
- **Edad (barras)** - Candidatos: med=24, max=92 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 3276}
- **Priority Seleccionados**: {'P0': 179, 'P3': 1643, 'NA': 291}
- **Type Candidatos**: {'Swing': 3276}
- **Type Seleccionados**: {'P0_Zone': 179, 'P3_Swing': 1643, 'P4_Fallback': 291}
- **TF Candidatos**: {240: 2723, 60: 374, 15: 116, 5: 63}
- **TF Seleccionados**: {15: 481, 240: 1008, 5: 224, -1: 291, 60: 109}
- **DistATR** - Candidatos: avg=10.9 | Seleccionados: avg=11.2
- **RR** - Candidatos: avg=3.88 | Seleccionados: avg=1.66
- **Razones de selección**: {'BestIntelligentScore': 2113}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.