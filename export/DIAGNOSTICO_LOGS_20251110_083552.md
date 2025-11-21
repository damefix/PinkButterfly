# Informe Diagnóstico de Logs - 2025-11-10 08:36:15

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251110_083552.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251110_083552.csv`

## DFM
- Eventos de evaluación: 54
- Evaluaciones Bull: 0 | Bear: 75
- Pasaron umbral (PassedThreshold): 75
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:0, 6:24, 7:28, 8:23, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 114
- KeptAligned: 245/245 | KeptCounter: 4/4
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.802 | AvgProxCounter≈ 0.035
  - AvgDistATRAligned≈ 2.39 | AvgDistATRCounter≈ 0.00
- PreferAligned eventos: 110 | Filtradas contra-bias: 0

### Proximity (Pre-PreferAligned)
- Eventos: 114
- Aligned pre: 245/249 | Counter pre: 4/249
- AvgProxAligned(pre)≈ 0.802 | AvgDistATRAligned(pre)≈ 2.39

### Proximity Drivers
- Eventos: 114
- Alineadas: n=245 | BaseProx≈ 0.847 | ZoneATR≈ 4.27 | SizePenalty≈ 0.986 | FinalProx≈ 0.836
- Contra-bias: n=4 | BaseProx≈ 0.996 | ZoneATR≈ 2.40 | SizePenalty≈ 1.000 | FinalProx≈ 0.996

## Risk
- Eventos: 114
- Accepted=75 | RejSL=0 | RejTP=0 | RejRR=22 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 51 (73.9% del total)
  - Avg Score: 0.67 | Avg R:R: 2.02 | Avg DistATR: 8.69
  - Por TF: TF5=19, TF15=32
- **P0_SWING_LITE:** 18 (26.1% del total)
  - Avg Score: 0.55 | Avg R:R: 8.73 | Avg DistATR: 7.15
  - Por TF: TF15=14, TF60=4


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 3 | Unmatched: 72
- 0-10: Wins=3 Losses=0 WR=100.0% (n=3)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=3 Losses=0 WR=100.0% (n=3)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 75 | Aligned=75 (100.0%)
- Core≈ 1.00 | Prox≈ 0.89 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.57 | Confidence≈ 0.00
- SL_TF dist: {'15': 75} | SL_Structural≈ 100.0%
- TP_TF dist: {'240': 18, '15': 49, '5': 3, '60': 5} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=75, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=0, 15m=75, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.58 (n=75), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 1007 | Zonas con Anchors: 1007
- Dir zonas (zona): Bull=3 Bear=1003 Neutral=1
- Resumen por ciclo (promedios): TotHZ≈ 2.1, WithAnchors≈ 2.1, DirBull≈ 0.0, DirBear≈ 2.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 1006, 'tie-bias': 1}
- TF Triggers: {'15': 211, '5': 38}
- TF Anchors: {'60': 249, '240': 247, '1440': 136}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,33': 8, 'score decayó a 0,35': 2, 'score decayó a 0,47': 1}

## CSV de Trades
- Filas: 42 | Ejecutadas: 1 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 43

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 75
- Registered: 24
  - DEDUP_COOLDOWN: 14 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 38

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 50.7%
- RegRate = Registered / Intentos = 63.2%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 36.8%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 4.2%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 245 | Total candidatos: 1850 | Seleccionados: 0
- Candidatos por zona (promedio): 7.6

### Take Profit (TP)
- Zonas analizadas: 245 | Total candidatos: 1468 | Seleccionados: 0
- Candidatos por zona (promedio): 6.0
- **Edad (barras)** - Candidatos: med=53, max=146 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 1468}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 1468}
- **Type Seleccionados**: {}
- **TF Candidatos**: {15: 770, 5: 353, 60: 226, 240: 119}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=6.2 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=2.74 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.