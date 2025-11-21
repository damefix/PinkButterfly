# Informe Diagnóstico de Logs - 2025-11-10 11:05:00

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251110_110441.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251110_110441.csv`

## DFM
- Eventos de evaluación: 83
- Evaluaciones Bull: 2 | Bear: 153
- Pasaron umbral (PassedThreshold): 155
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:2, 5:0, 6:4, 7:84, 8:65, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 114
- KeptAligned: 275/275 | KeptCounter: 14/14
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.724 | AvgProxCounter≈ 0.059
  - AvgDistATRAligned≈ 2.42 | AvgDistATRCounter≈ 0.51
- PreferAligned eventos: 101 | Filtradas contra-bias: 1

### Proximity (Pre-PreferAligned)
- Eventos: 114
- Aligned pre: 275/289 | Counter pre: 14/289
- AvgProxAligned(pre)≈ 0.724 | AvgDistATRAligned(pre)≈ 2.42

### Proximity Drivers
- Eventos: 114
- Alineadas: n=275 | BaseProx≈ 0.829 | ZoneATR≈ 4.15 | SizePenalty≈ 0.987 | FinalProx≈ 0.818
- Contra-bias: n=13 | BaseProx≈ 0.478 | ZoneATR≈ 3.01 | SizePenalty≈ 1.000 | FinalProx≈ 0.478

## Risk
- Eventos: 114
- Accepted=155 | RejSL=0 | RejTP=0 | RejRR=59 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 50 (60.2% del total)
  - Avg Score: 0.67 | Avg R:R: 2.05 | Avg DistATR: 7.29
  - Por TF: TF5=23, TF15=27
- **P0_SWING_LITE:** 33 (39.8% del total)
  - Avg Score: 0.49 | Avg R:R: 6.44 | Avg DistATR: 8.63
  - Por TF: TF15=20, TF60=13


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 8 | Unmatched: 147
- 0-10: Wins=4 Losses=4 WR=50.0% (n=8)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=4 Losses=4 WR=50.0% (n=8)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 155 | Aligned=153 (98.7%)
- Core≈ 1.00 | Prox≈ 0.82 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.73 | Confidence≈ 0.00
- SL_TF dist: {'15': 149, '5': 6} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 11, '240': 70, '15': 66, '5': 8} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=122, 8-10=15, 10-12.5=12, 12.5-15=6, >15=0
- TF: 5m=6, 15m=149, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.62 (n=137), 10-15≈ 2.56 (n=18)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 719 | Zonas con Anchors: 719
- Dir zonas (zona): Bull=8 Bear=705 Neutral=6
- Resumen por ciclo (promedios): TotHZ≈ 2.4, WithAnchors≈ 2.4, DirBull≈ 0.1, DirBear≈ 2.3, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 713, 'tie-bias': 6}
- TF Triggers: {'15': 236, '5': 53}
- TF Anchors: {'60': 289, '240': 289, '1440': 157}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)

## CSV de Trades
- Filas: 82 | Ejecutadas: 7 | Canceladas: 0 | Expiradas: 0
- BUY: 6 | SELL: 83

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 155
- Registered: 75
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 75

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 48.4%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 9.3%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 280 | Total candidatos: 2135 | Seleccionados: 0
- Candidatos por zona (promedio): 7.6

### Take Profit (TP)
- Zonas analizadas: 280 | Total candidatos: 2960 | Seleccionados: 0
- Candidatos por zona (promedio): 10.6
- **Edad (barras)** - Candidatos: med=43, max=146 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 2960}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 2960}
- **Type Seleccionados**: {}
- **TF Candidatos**: {15: 1128, 240: 972, 60: 442, 5: 418}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=7.2 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=2.91 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.