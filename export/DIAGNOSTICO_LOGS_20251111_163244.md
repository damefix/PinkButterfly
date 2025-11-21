# Informe Diagnóstico de Logs - 2025-11-11 16:37:12

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_163244.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_163244.csv`

## DFM
- Eventos de evaluación: 30
- Evaluaciones Bull: 6 | Bear: 24
- Pasaron umbral (PassedThreshold): 24
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:3, 4:2, 5:4, 6:16, 7:5, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2206
- KeptAligned: 928/928 | KeptCounter: 529/529
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.264 | AvgProxCounter≈ 0.153
  - AvgDistATRAligned≈ 0.20 | AvgDistATRCounter≈ 0.10
- PreferAligned eventos: 638 | Filtradas contra-bias: 105

### Proximity (Pre-PreferAligned)
- Eventos: 2206
- Aligned pre: 928/1457 | Counter pre: 529/1457
- AvgProxAligned(pre)≈ 0.264 | AvgDistATRAligned(pre)≈ 0.20

### Proximity Drivers
- Eventos: 2206
- Alineadas: n=928 | BaseProx≈ 0.930 | ZoneATR≈ 5.11 | SizePenalty≈ 0.977 | FinalProx≈ 0.908
- Contra-bias: n=424 | BaseProx≈ 0.886 | ZoneATR≈ 4.62 | SizePenalty≈ 0.979 | FinalProx≈ 0.868

## Risk
- Eventos: 941
- Accepted=30 | RejSL=0 | RejTP=0 | RejRR=38 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 3 (3.6% del total)
  - Avg Score: 0.43 | Avg R:R: 2.02 | Avg DistATR: 2.74
  - Por TF: TF5=1, TF15=2
- **P0_SWING_LITE:** 80 (96.4% del total)
  - Avg Score: 0.55 | Avg R:R: 4.39 | Avg DistATR: 3.41
  - Por TF: TF15=16, TF60=64


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 8 | Unmatched: 22
- 0-10: Wins=0 Losses=8 WR=0.0% (n=8)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=0 Losses=8 WR=0.0% (n=8)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 30 | Aligned=18 (60.0%)
- Core≈ 1.00 | Prox≈ 0.91 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.79 | Confidence≈ 0.00
- SL_TF dist: {'15': 27, '5': 3} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 11, '5': 11, '60': 6, '240': 2} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=30, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=3, 15m=27, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.79 (n=30), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 39552 | Zonas con Anchors: 39538
- Dir zonas (zona): Bull=8461 Bear=30137 Neutral=954
- Resumen por ciclo (promedios): TotHZ≈ 3.7, WithAnchors≈ 3.7, DirBull≈ 1.4, DirBear≈ 2.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 38396, 'tie-bias': 1142, 'triggers-only': 14}
- TF Triggers: {'5': 4994, '15': 4241}
- TF Anchors: {'60': 9177, '240': 5530, '1440': 478}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)

## CSV de Trades
- Filas: 22 | Ejecutadas: 8 | Canceladas: 0 | Expiradas: 0
- BUY: 6 | SELL: 24

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 24
- Registered: 12
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 2 | SKIP_CONCURRENCY: 0
- Intentos de registro: 14

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 58.3%
- RegRate = Registered / Intentos = 85.7%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 14.3%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 66.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 107 | Total candidatos: 1701 | Seleccionados: 0
- Candidatos por zona (promedio): 15.9

### Take Profit (TP)
- Zonas analizadas: 103 | Total candidatos: 744 | Seleccionados: 0
- Candidatos por zona (promedio): 7.2
- **Edad (barras)** - Candidatos: med=49, max=180 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.39 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 744}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 744}
- **Type Seleccionados**: {}
- **TF Candidatos**: {60: 221, 5: 216, 15: 192, 240: 115}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=8.6 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=5.11 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.