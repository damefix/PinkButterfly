# Informe Diagnóstico de Logs - 2025-11-10 09:45:13

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251110_094424.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251110_094424.csv`

## DFM
- Eventos de evaluación: 90
- Evaluaciones Bull: 8 | Bear: 156
- Pasaron umbral (PassedThreshold): 164
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:8, 6:16, 7:91, 8:49, 9:0

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
- Accepted=164 | RejSL=0 | RejTP=0 | RejRR=54 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 50 (58.1% del total)
  - Avg Score: 0.67 | Avg R:R: 2.05 | Avg DistATR: 7.29
  - Por TF: TF5=23, TF15=27
- **P0_SWING_LITE:** 36 (41.9% del total)
  - Avg Score: 0.46 | Avg R:R: 6.34 | Avg DistATR: 8.79
  - Por TF: TF15=20, TF60=16


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 15 | Unmatched: 149
- 0-10: Wins=5 Losses=10 WR=33.3% (n=15)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=5 Losses=10 WR=33.3% (n=15)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 164 | Aligned=156 (95.1%)
- Core≈ 1.00 | Prox≈ 0.83 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.54 | Confidence≈ 0.00
- SL_TF dist: {'60': 5, '15': 153, '5': 6} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 68, '60': 15, '240': 73, '5': 8} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=131, 8-10=15, 10-12.5=12, 12.5-15=6, >15=0
- TF: 5m=6, 15m=153, 60m=5, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.47 (n=146), 10-15≈ 2.08 (n=18)

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
- Expiraciones: {'score decayó a 0,41': 5, 'score decayó a 0,43': 3, 'score decayó a 0,44': 4, 'score decayó a 0,46': 2, 'score decayó a 0,33': 7}

## CSV de Trades
- Filas: 90 | Ejecutadas: 14 | Canceladas: 0 | Expiradas: 0
- BUY: 24 | SELL: 80

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 164
- Registered: 55
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 27
- Intentos de registro: 82

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 50.0%
- RegRate = Registered / Intentos = 67.1%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 32.9%
- ExecRate = Ejecutadas / Registered = 25.5%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 274 | Total candidatos: 2110 | Seleccionados: 0
- Candidatos por zona (promedio): 7.7

### Take Profit (TP)
- Zonas analizadas: 274 | Total candidatos: 4283 | Seleccionados: 0
- Candidatos por zona (promedio): 15.6
- **Edad (barras)** - Candidatos: med=67, max=146 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.39 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 4283}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 4283}
- **Type Seleccionados**: {}
- **TF Candidatos**: {240: 2251, 15: 1135, 60: 479, 5: 418}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=9.2 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=3.37 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.