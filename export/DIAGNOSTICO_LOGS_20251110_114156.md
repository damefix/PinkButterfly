# Informe Diagnóstico de Logs - 2025-11-10 11:43:04

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251110_114156.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251110_114156.csv`

## DFM
- Eventos de evaluación: 93
- Evaluaciones Bull: 11 | Bear: 145
- Pasaron umbral (PassedThreshold): 156
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:6, 4:2, 5:3, 6:5, 7:72, 8:68, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 204
- KeptAligned: 408/408 | KeptCounter: 23/33
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.638 | AvgProxCounter≈ 0.032
  - AvgDistATRAligned≈ 3.21 | AvgDistATRCounter≈ 0.65
- PreferAligned eventos: 173 | Filtradas contra-bias: 2

### Proximity (Pre-PreferAligned)
- Eventos: 204
- Aligned pre: 408/431 | Counter pre: 23/431
- AvgProxAligned(pre)≈ 0.638 | AvgDistATRAligned(pre)≈ 3.21

### Proximity Drivers
- Eventos: 204
- Alineadas: n=408 | BaseProx≈ 0.781 | ZoneATR≈ 4.48 | SizePenalty≈ 0.984 | FinalProx≈ 0.769
- Contra-bias: n=21 | BaseProx≈ 0.286 | ZoneATR≈ 1.58 | SizePenalty≈ 1.000 | FinalProx≈ 0.286

## Risk
- Eventos: 194
- Accepted=156 | RejSL=0 | RejTP=0 | RejRR=141 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 60 (46.2% del total)
  - Avg Score: 0.65 | Avg R:R: 2.10 | Avg DistATR: 7.73
  - Por TF: TF5=26, TF15=34
- **P0_SWING_LITE:** 70 (53.8% del total)
  - Avg Score: 0.34 | Avg R:R: 9.47 | Avg DistATR: 9.06
  - Por TF: TF15=45, TF60=25


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 46 | Unmatched: 110
- 0-10: Wins=42 Losses=3 WR=93.3% (n=45)
- 10-15: Wins=1 Losses=0 WR=100.0% (n=1)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=43 Losses=3 WR=93.5% (n=46)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 156 | Aligned=145 (92.9%)
- Core≈ 1.00 | Prox≈ 0.74 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.75 | Confidence≈ 0.00
- SL_TF dist: {'15': 110, '5': 46} | SL_Structural≈ 100.0%
- TP_TF dist: {'240': 70, '15': 71, '60': 10, '5': 5} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=148, 8-10=7, 10-12.5=1, 12.5-15=0, >15=0
- TF: 5m=46, 15m=110, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.75 (n=155), 10-15≈ 1.52 (n=1)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 2907 | Zonas con Anchors: 2907
- Dir zonas (zona): Bull=494 Bear=2409 Neutral=4
- Resumen por ciclo (promedios): TotHZ≈ 1.7, WithAnchors≈ 1.7, DirBull≈ 0.1, DirBear≈ 1.5, DirNeutral≈ 0.0
- Razones de dirección: {'tie-bias': 4, 'anchors+triggers': 2903}
- TF Triggers: {'15': 335, '5': 106}
- TF Anchors: {'60': 441, '240': 441, '1440': 197}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Cancel_BOS (diag): por acción {'BUY': 3, 'SELL': 0} | por bias {'Bullish': 0, 'Bearish': 3, 'Neutral': 0}

## CSV de Trades
- Filas: 123 | Ejecutadas: 30 | Canceladas: 0 | Expiradas: 0
- BUY: 17 | SELL: 136

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 156
- Registered: 90
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 90

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 57.7%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 33.3%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 426 | Total candidatos: 5991 | Seleccionados: 0
- Candidatos por zona (promedio): 14.1

### Take Profit (TP)
- Zonas analizadas: 426 | Total candidatos: 4755 | Seleccionados: 0
- Candidatos por zona (promedio): 11.2
- **Edad (barras)** - Candidatos: med=48, max=333 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.40 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 4755}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 4755}
- **Type Seleccionados**: {}
- **TF Candidatos**: {240: 1660, 15: 1543, 60: 852, 5: 700}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=8.6 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=4.06 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.