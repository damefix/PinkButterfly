# Informe Diagnóstico de Logs - 2025-11-10 12:41:03

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251110_124014.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251110_124014.csv`

## DFM
- Eventos de evaluación: 164
- Evaluaciones Bull: 16 | Bear: 318
- Pasaron umbral (PassedThreshold): 334
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:3, 5:10, 6:54, 7:137, 8:130, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 273
- KeptAligned: 618/618 | KeptCounter: 204/251
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.644 | AvgProxCounter≈ 0.296
  - AvgDistATRAligned≈ 2.59 | AvgDistATRCounter≈ 1.86
- PreferAligned eventos: 223 | Filtradas contra-bias: 129

### Proximity (Pre-PreferAligned)
- Eventos: 273
- Aligned pre: 618/822 | Counter pre: 204/822
- AvgProxAligned(pre)≈ 0.644 | AvgDistATRAligned(pre)≈ 2.59

### Proximity Drivers
- Eventos: 273
- Alineadas: n=618 | BaseProx≈ 0.788 | ZoneATR≈ 4.96 | SizePenalty≈ 0.976 | FinalProx≈ 0.772
- Contra-bias: n=75 | BaseProx≈ 0.683 | ZoneATR≈ 5.34 | SizePenalty≈ 0.980 | FinalProx≈ 0.666

## Risk
- Eventos: 270
- Accepted=334 | RejSL=0 | RejTP=0 | RejRR=172 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 121 (49.4% del total)
  - Avg Score: 0.65 | Avg R:R: 1.96 | Avg DistATR: 8.83
  - Por TF: TF5=72, TF15=49
- **P0_SWING_LITE:** 124 (50.6% del total)
  - Avg Score: 0.25 | Avg R:R: 7.91 | Avg DistATR: 9.45
  - Por TF: TF15=78, TF60=46


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 107 | Unmatched: 227
- 0-10: Wins=61 Losses=45 WR=57.5% (n=106)
- 10-15: Wins=1 Losses=0 WR=100.0% (n=1)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=62 Losses=45 WR=57.9% (n=107)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 334 | Aligned=305 (91.3%)
- Core≈ 1.00 | Prox≈ 0.76 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.66 | Confidence≈ 0.00
- SL_TF dist: {'60': 44, '15': 225, '5': 65} | SL_Structural≈ 100.0%
- TP_TF dist: {'240': 136, '60': 51, '15': 134, '5': 13} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=318, 8-10=14, 10-12.5=2, 12.5-15=0, >15=0
- TF: 5m=65, 15m=225, 60m=44, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.65 (n=332), 10-15≈ 1.79 (n=2)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 8165 | Zonas con Anchors: 8165
- Dir zonas (zona): Bull=997 Bear=7027 Neutral=141
- Resumen por ciclo (promedios): TotHZ≈ 3.0, WithAnchors≈ 3.0, DirBull≈ 0.6, DirBear≈ 2.2, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 8024, 'tie-bias': 141}
- TF Triggers: {'15': 658, '5': 211}
- TF Anchors: {'60': 869, '240': 869, '1440': 869}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Cancel_BOS (diag): por acción {'BUY': 6, 'SELL': 18} | por bias {'Bullish': 18, 'Bearish': 6, 'Neutral': 0}

## CSV de Trades
- Filas: 267 | Ejecutadas: 83 | Canceladas: 0 | Expiradas: 0
- BUY: 13 | SELL: 337

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 334
- Registered: 160
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 160

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 47.9%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 51.9%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 652 | Total candidatos: 8003 | Seleccionados: 0
- Candidatos por zona (promedio): 12.3

### Take Profit (TP)
- Zonas analizadas: 652 | Total candidatos: 9951 | Seleccionados: 0
- Candidatos por zona (promedio): 15.3
- **Edad (barras)** - Candidatos: med=68, max=333 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.36 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 9951}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 9951}
- **Type Seleccionados**: {}
- **TF Candidatos**: {240: 5197, 15: 2281, 60: 1656, 5: 817}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=12.0 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=4.60 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.