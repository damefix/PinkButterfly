# Informe Diagnóstico de Logs - 2025-11-11 17:12:18

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_170530.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_170530.csv`

## DFM
- Eventos de evaluación: 31
- Evaluaciones Bull: 8 | Bear: 23
- Pasaron umbral (PassedThreshold): 27
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:3, 4:1, 5:6, 6:16, 7:5, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2205
- KeptAligned: 909/909 | KeptCounter: 529/529
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.261 | AvgProxCounter≈ 0.152
  - AvgDistATRAligned≈ 0.19 | AvgDistATRCounter≈ 0.10
- PreferAligned eventos: 632 | Filtradas contra-bias: 102

### Proximity (Pre-PreferAligned)
- Eventos: 2205
- Aligned pre: 909/1438 | Counter pre: 529/1438
- AvgProxAligned(pre)≈ 0.261 | AvgDistATRAligned(pre)≈ 0.19

### Proximity Drivers
- Eventos: 2205
- Alineadas: n=909 | BaseProx≈ 0.930 | ZoneATR≈ 5.11 | SizePenalty≈ 0.976 | FinalProx≈ 0.908
- Contra-bias: n=427 | BaseProx≈ 0.888 | ZoneATR≈ 4.57 | SizePenalty≈ 0.980 | FinalProx≈ 0.870

## Risk
- Eventos: 931
- Accepted=31 | RejSL=0 | RejTP=0 | RejRR=37 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 3 (3.6% del total)
  - Avg Score: 0.43 | Avg R:R: 2.02 | Avg DistATR: 2.74
  - Por TF: TF5=1, TF15=2
- **P0_SWING_LITE:** 80 (96.4% del total)
  - Avg Score: 0.54 | Avg R:R: 4.47 | Avg DistATR: 3.45
  - Por TF: TF15=15, TF60=65


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 8 | Unmatched: 23
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
- Muestras: 31 | Aligned=21 (67.7%)
- Core≈ 1.00 | Prox≈ 0.91 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.83 | Confidence≈ 0.00
- SL_TF dist: {'15': 28, '5': 3} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 17, '5': 11, '240': 1, '60': 2} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=31, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=3, 15m=28, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.83 (n=31), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 39678 | Zonas con Anchors: 39664
- Dir zonas (zona): Bull=8615 Bear=30122 Neutral=941
- Resumen por ciclo (promedios): TotHZ≈ 3.7, WithAnchors≈ 3.7, DirBull≈ 1.4, DirBear≈ 2.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 38538, 'tie-bias': 1126, 'triggers-only': 14}
- TF Triggers: {'5': 4987, '15': 4263}
- TF Anchors: {'60': 9192, '240': 5530, '1440': 491}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)

## CSV de Trades
- Filas: 22 | Ejecutadas: 8 | Canceladas: 0 | Expiradas: 0
- BUY: 9 | SELL: 21

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 27
- Registered: 12
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 3 | SKIP_CONCURRENCY: 0
- Intentos de registro: 15

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 55.6%
- RegRate = Registered / Intentos = 80.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 20.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 66.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 108 | Total candidatos: 1722 | Seleccionados: 0
- Candidatos por zona (promedio): 15.9

### Take Profit (TP)
- Zonas analizadas: 103 | Total candidatos: 733 | Seleccionados: 0
- Candidatos por zona (promedio): 7.1
- **Edad (barras)** - Candidatos: med=49, max=180 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.39 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 733}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 733}
- **Type Seleccionados**: {}
- **TF Candidatos**: {5: 216, 60: 214, 15: 189, 240: 114}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=8.4 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=5.03 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.