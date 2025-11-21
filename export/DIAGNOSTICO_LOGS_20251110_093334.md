# Informe Diagnóstico de Logs - 2025-11-10 09:34:31

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251110_093334.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251110_093334.csv`

## DFM
- Eventos de evaluación: 75
- Evaluaciones Bull: 3 | Bear: 114
- Pasaron umbral (PassedThreshold): 117
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:3, 5:6, 6:46, 7:43, 8:19, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 114
- KeptAligned: 266/266 | KeptCounter: 23/23
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.657 | AvgProxCounter≈ 0.174
  - AvgDistATRAligned≈ 2.23 | AvgDistATRCounter≈ 0.15
- PreferAligned eventos: 92 | Filtradas contra-bias: 0

### Proximity (Pre-PreferAligned)
- Eventos: 114
- Aligned pre: 266/289 | Counter pre: 23/289
- AvgProxAligned(pre)≈ 0.657 | AvgDistATRAligned(pre)≈ 2.23

### Proximity Drivers
- Eventos: 114
- Alineadas: n=266 | BaseProx≈ 0.828 | ZoneATR≈ 4.18 | SizePenalty≈ 0.986 | FinalProx≈ 0.817
- Contra-bias: n=23 | BaseProx≈ 0.898 | ZoneATR≈ 3.13 | SizePenalty≈ 1.000 | FinalProx≈ 0.898

## Risk
- Eventos: 114
- Accepted=117 | RejSL=0 | RejTP=0 | RejRR=53 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 60 (72.3% del total)
  - Avg Score: 0.67 | Avg R:R: 2.05 | Avg DistATR: 8.81
  - Por TF: TF5=28, TF15=32
- **P0_SWING_LITE:** 23 (27.7% del total)
  - Avg Score: 0.55 | Avg R:R: 8.92 | Avg DistATR: 7.10
  - Por TF: TF15=12, TF60=11


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 21 | Unmatched: 96
- 0-10: Wins=16 Losses=5 WR=76.2% (n=21)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=16 Losses=5 WR=76.2% (n=21)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 117 | Aligned=114 (97.4%)
- Core≈ 1.00 | Prox≈ 0.84 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.40 | Confidence≈ 0.00
- SL_TF dist: {'15': 113, '5': 4} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 18, '240': 57, '15': 42} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=117, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=4, 15m=113, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.40 (n=117), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 719 | Zonas con Anchors: 719
- Dir zonas (zona): Bull=14 Bear=696 Neutral=9
- Resumen por ciclo (promedios): TotHZ≈ 2.4, WithAnchors≈ 2.4, DirBull≈ 0.1, DirBear≈ 2.2, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 710, 'tie-bias': 9}
- TF Triggers: {'15': 236, '5': 53}
- TF Anchors: {'60': 289, '240': 289, '1440': 289}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,35': 1, 'score decayó a 0,33': 12, 'estructura no existe': 4, 'score decayó a 0,46': 1, 'score decayó a 0,47': 1}

## CSV de Trades
- Filas: 79 | Ejecutadas: 12 | Canceladas: 0 | Expiradas: 0
- BUY: 6 | SELL: 85

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 117
- Registered: 48
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 48

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 41.0%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 25.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 269 | Total candidatos: 2103 | Seleccionados: 0
- Candidatos por zona (promedio): 7.8

### Take Profit (TP)
- Zonas analizadas: 269 | Total candidatos: 4131 | Seleccionados: 0
- Candidatos por zona (promedio): 15.4
- **Edad (barras)** - Candidatos: med=81, max=150 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.39 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 4131}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 4131}
- **Type Seleccionados**: {}
- **TF Candidatos**: {240: 2186, 15: 1120, 5: 418, 60: 407}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=8.7 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=3.62 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.