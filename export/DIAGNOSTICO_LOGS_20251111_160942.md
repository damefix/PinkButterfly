# Informe Diagnóstico de Logs - 2025-11-11 16:14:12

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_160942.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_160942.csv`

## DFM
- Eventos de evaluación: 29
- Evaluaciones Bull: 6 | Bear: 23
- Pasaron umbral (PassedThreshold): 24
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:3, 4:1, 5:4, 6:16, 7:5, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2204
- KeptAligned: 918/918 | KeptCounter: 522/522
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.263 | AvgProxCounter≈ 0.151
  - AvgDistATRAligned≈ 0.19 | AvgDistATRCounter≈ 0.10
- PreferAligned eventos: 635 | Filtradas contra-bias: 102

### Proximity (Pre-PreferAligned)
- Eventos: 2204
- Aligned pre: 918/1440 | Counter pre: 522/1440
- AvgProxAligned(pre)≈ 0.263 | AvgDistATRAligned(pre)≈ 0.19

### Proximity Drivers
- Eventos: 2204
- Alineadas: n=918 | BaseProx≈ 0.930 | ZoneATR≈ 5.10 | SizePenalty≈ 0.977 | FinalProx≈ 0.908
- Contra-bias: n=420 | BaseProx≈ 0.887 | ZoneATR≈ 4.62 | SizePenalty≈ 0.979 | FinalProx≈ 0.869

## Risk
- Eventos: 934
- Accepted=29 | RejSL=0 | RejTP=0 | RejRR=42 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 3 (3.6% del total)
  - Avg Score: 0.43 | Avg R:R: 2.02 | Avg DistATR: 2.74
  - Por TF: TF5=1, TF15=2
- **P0_SWING_LITE:** 80 (96.4% del total)
  - Avg Score: 0.54 | Avg R:R: 4.47 | Avg DistATR: 3.45
  - Por TF: TF15=15, TF60=65


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 10 | Unmatched: 19
- 0-10: Wins=0 Losses=10 WR=0.0% (n=10)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=0 Losses=10 WR=0.0% (n=10)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 29 | Aligned=18 (62.1%)
- Core≈ 1.00 | Prox≈ 0.91 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.81 | Confidence≈ 0.00
- SL_TF dist: {'15': 26, '5': 3} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 11, '5': 11, '60': 5, '240': 2} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=29, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=3, 15m=26, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.81 (n=29), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 39845 | Zonas con Anchors: 39831
- Dir zonas (zona): Bull=8668 Bear=30231 Neutral=946
- Resumen por ciclo (promedios): TotHZ≈ 3.7, WithAnchors≈ 3.7, DirBull≈ 1.4, DirBear≈ 2.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 38701, 'tie-bias': 1130, 'triggers-only': 14}
- TF Triggers: {'5': 4981, '15': 4241}
- TF Anchors: {'60': 9164, '240': 5508, '1440': 467}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)

## CSV de Trades
- Filas: 30 | Ejecutadas: 10 | Canceladas: 0 | Expiradas: 0
- BUY: 6 | SELL: 34

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 24
- Registered: 17
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 4 | SKIP_CONCURRENCY: 0
- Intentos de registro: 21

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 87.5%
- RegRate = Registered / Intentos = 81.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 19.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 58.8%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 108 | Total candidatos: 1715 | Seleccionados: 0
- Candidatos por zona (promedio): 15.9

### Take Profit (TP)
- Zonas analizadas: 104 | Total candidatos: 742 | Seleccionados: 0
- Candidatos por zona (promedio): 7.1
- **Edad (barras)** - Candidatos: med=50, max=180 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.38 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 742}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 742}
- **Type Seleccionados**: {}
- **TF Candidatos**: {60: 222, 5: 216, 15: 189, 240: 115}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=8.5 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=5.04 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.