# Informe Diagnóstico de Logs - 2025-11-17 19:19:51

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_191727.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251117_191727.csv`

## DFM
- Eventos de evaluación: 180
- Evaluaciones Bull: 5 | Bear: 171
- Pasaron umbral (PassedThreshold): 176
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:1, 6:42, 7:96, 8:37, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 1266
- KeptAligned: 1048/1048 | KeptCounter: 611/682
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.268 | AvgProxCounter≈ 0.130
  - AvgDistATRAligned≈ 0.97 | AvgDistATRCounter≈ 0.73
- PreferAligned eventos: 358 | Filtradas contra-bias: 128

### Proximity (Pre-PreferAligned)
- Eventos: 1266
- Aligned pre: 1048/1659 | Counter pre: 611/1659
- AvgProxAligned(pre)≈ 0.268 | AvgDistATRAligned(pre)≈ 0.97

### Proximity Drivers
- Eventos: 1266
- Alineadas: n=1048 | BaseProx≈ 0.757 | ZoneATR≈ 6.25 | SizePenalty≈ 0.956 | FinalProx≈ 0.725
- Contra-bias: n=483 | BaseProx≈ 0.552 | ZoneATR≈ 5.79 | SizePenalty≈ 0.966 | FinalProx≈ 0.532

## Risk
- Eventos: 632
- Accepted=212 | RejSL=0 | RejTP=0 | RejRR=389 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_SWING_LITE:** 837 (100.0% del total)
  - Avg Score: 0.63 | Avg R:R: 3.15 | Avg DistATR: 3.96
  - Por TF: TF15=310, TF60=527


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 33 | Unmatched: 179
- 0-10: Wins=33 Losses=0 WR=100.0% (n=33)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=33 Losses=0 WR=100.0% (n=33)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 212 | Aligned=163 (76.9%)
- Core≈ 1.00 | Prox≈ 0.69 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.58 | Confidence≈ 0.00
- SL_TF dist: {'15': 197, '1440': 1, '60': 13, '5': 1} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 125, '240': 41, '60': 22, '5': 24} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=212, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=1, 15m=197, 60m=13, 240m=0, 1440m=1
- RR plan por bandas: 0-10≈ 2.58 (n=212), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 4364 | Zonas con Anchors: 4364
- Dir zonas (zona): Bull=289 Bear=2819 Neutral=1256
- Resumen por ciclo (promedios): TotHZ≈ 1.7, WithAnchors≈ 1.7, DirBull≈ 0.1, DirBear≈ 1.1, DirNeutral≈ 0.5
- Razones de dirección: {'tie-bias': 1764, 'anchors+triggers': 2600}
- TF Triggers: {'5': 2531, '15': 1833}
- TF Anchors: {'60': 4364, '240': 4364, '1440': 4364}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,40': 1, 'score decayó a 0,47': 4}

## CSV de Trades
- Filas: 29 | Ejecutadas: 2 | Canceladas: 0 | Expiradas: 0
- BUY: 6 | SELL: 25

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 176
- Registered: 16
  - DEDUP_COOLDOWN: 30 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 41
- Intentos de registro: 87

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 49.4%
- RegRate = Registered / Intentos = 18.4%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 34.5%
- Concurrency = SKIP_CONCURRENCY / Intentos = 47.1%
- ExecRate = Ejecutadas / Registered = 12.5%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 1248 | Total candidatos: 6902 | Seleccionados: 0
- Candidatos por zona (promedio): 5.5

### Take Profit (TP)
- Zonas analizadas: 1246 | Total candidatos: 18158 | Seleccionados: 1246
- Candidatos por zona (promedio): 14.6
- **Edad (barras)** - Candidatos: med=25, max=93 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.55 | Seleccionados: avg=0.73
- **Priority Candidatos**: {'P3': 2426}
- **Priority Seleccionados**: {'P3': 1037, 'P0': 56, 'NA': 153}
- **Type Candidatos**: {'Swing': 2426}
- **Type Seleccionados**: {'P3_Swing': 1037, 'P0_Zone': 56, 'P4_Fallback': 153}
- **TF Candidatos**: {240: 1710, 60: 393, 15: 176, 5: 147}
- **TF Seleccionados**: {5: 361, 15: 261, 240: 314, 60: 157, -1: 153}
- **DistATR** - Candidatos: avg=7.3 | Seleccionados: avg=5.7
- **RR** - Candidatos: avg=3.02 | Seleccionados: avg=1.32
- **Razones de selección**: {'BestIntelligentScore': 1246}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.