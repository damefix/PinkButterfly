# Informe Diagnóstico de Logs - 2025-11-12 09:23:06

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_090750.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251112_090750.csv`

## DFM
- Eventos de evaluación: 175
- Evaluaciones Bull: 64 | Bear: 126
- Pasaron umbral (PassedThreshold): 177
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:2, 4:10, 5:21, 6:88, 7:60, 8:9, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2174
- KeptAligned: 1901/1901 | KeptCounter: 1072/1072
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.373 | AvgProxCounter≈ 0.202
  - AvgDistATRAligned≈ 0.54 | AvgDistATRCounter≈ 0.32
- PreferAligned eventos: 832 | Filtradas contra-bias: 137

### Proximity (Pre-PreferAligned)
- Eventos: 2174
- Aligned pre: 1901/2973 | Counter pre: 1072/2973
- AvgProxAligned(pre)≈ 0.373 | AvgDistATRAligned(pre)≈ 0.54

### Proximity Drivers
- Eventos: 2174
- Alineadas: n=1901 | BaseProx≈ 0.872 | ZoneATR≈ 4.96 | SizePenalty≈ 0.980 | FinalProx≈ 0.855
- Contra-bias: n=935 | BaseProx≈ 0.766 | ZoneATR≈ 4.80 | SizePenalty≈ 0.979 | FinalProx≈ 0.750

## Risk
- Eventos: 1348
- Accepted=190 | RejSL=0 | RejTP=0 | RejRR=148 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 31 (7.7% del total)
  - Avg Score: 0.41 | Avg R:R: 1.99 | Avg DistATR: 3.63
  - Por TF: TF5=8, TF15=23
- **P0_SWING_LITE:** 371 (92.3% del total)
  - Avg Score: 0.53 | Avg R:R: 3.59 | Avg DistATR: 3.31
  - Por TF: TF15=75, TF60=296


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 53 | Unmatched: 138
- 0-10: Wins=19 Losses=34 WR=35.8% (n=53)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=19 Losses=34 WR=35.8% (n=53)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 191 | Aligned=111 (58.1%)
- Core≈ 1.00 | Prox≈ 0.81 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.73 | Confidence≈ 0.00
- SL_TF dist: {'60': 10, '5': 37, '15': 144} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 85, '60': 59, '5': 38, '240': 9} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=190, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=36, 15m=144, 60m=10, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.73 (n=190), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 18566 | Zonas con Anchors: 18448
- Dir zonas (zona): Bull=6873 Bear=11024 Neutral=669
- Resumen por ciclo (promedios): TotHZ≈ 3.3, WithAnchors≈ 3.3, DirBull≈ 1.4, DirBear≈ 1.8, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 17731, 'tie-bias': 720, 'triggers-only': 115}
- TF Triggers: {'15': 3557, '5': 4797}
- TF Anchors: {'60': 8241, '240': 4652, '1440': 121}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 88 | Ejecutadas: 33 | Canceladas: 0 | Expiradas: 0
- BUY: 45 | SELL: 76

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 177
- Registered: 46
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 25 | SKIP_CONCURRENCY: 8
- Intentos de registro: 79

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 44.6%
- RegRate = Registered / Intentos = 58.2%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 31.6%
- Concurrency = SKIP_CONCURRENCY / Intentos = 10.1%
- ExecRate = Ejecutadas / Registered = 71.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 561 | Total candidatos: 9464 | Seleccionados: 0
- Candidatos por zona (promedio): 16.9

### Take Profit (TP)
- Zonas analizadas: 550 | Total candidatos: 4239 | Seleccionados: 550
- Candidatos por zona (promedio): 7.7
- **Edad (barras)** - Candidatos: med=52, max=182 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.40 | Seleccionados: avg=0.71
- **Priority Candidatos**: {'P3': 4239}
- **Priority Seleccionados**: {'P0': 50, 'P3': 259, 'NA': 241}
- **Type Candidatos**: {'Swing': 4239}
- **Type Seleccionados**: {'P0_Zone': 50, 'P3_Swing': 259, 'P4_Fallback': 241}
- **TF Candidatos**: {60: 1414, 15: 1101, 5: 1011, 240: 713}
- **TF Seleccionados**: {15: 124, 60: 90, -1: 241, 5: 69, 240: 26}
- **DistATR** - Candidatos: avg=9.9 | Seleccionados: avg=3.5
- **RR** - Candidatos: avg=4.65 | Seleccionados: avg=1.31
- **Razones de selección**: {'BestIntelligentScore': 550}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.