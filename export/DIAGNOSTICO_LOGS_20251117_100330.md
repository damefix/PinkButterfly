# Informe Diagnóstico de Logs - 2025-11-17 10:14:50

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_100330.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251117_100330.csv`

## DFM
- Eventos de evaluación: 830
- Evaluaciones Bull: 64 | Bear: 526
- Pasaron umbral (PassedThreshold): 590
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:49, 6:213, 7:258, 8:70, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 25016
- KeptAligned: 1647/1647 | KeptCounter: 3213/3581
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.036 | AvgProxCounter≈ 0.033
  - AvgDistATRAligned≈ 0.13 | AvgDistATRCounter≈ 0.20
- PreferAligned eventos: 1234 | Filtradas contra-bias: 1238

### Proximity (Pre-PreferAligned)
- Eventos: 25016
- Aligned pre: 1647/4860 | Counter pre: 3213/4860
- AvgProxAligned(pre)≈ 0.036 | AvgDistATRAligned(pre)≈ 0.13

### Proximity Drivers
- Eventos: 25016
- Alineadas: n=1647 | BaseProx≈ 0.744 | ZoneATR≈ 5.12 | SizePenalty≈ 0.967 | FinalProx≈ 0.717
- Contra-bias: n=1975 | BaseProx≈ 0.491 | ZoneATR≈ 5.47 | SizePenalty≈ 0.970 | FinalProx≈ 0.475

## Risk
- Eventos: 2334
- Accepted=1036 | RejSL=0 | RejTP=0 | RejRR=180 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 111 (6.6% del total)
  - Avg Score: 0.41 | Avg R:R: 2.37 | Avg DistATR: 2.52
  - Por TF: TF15=111
- **P0_SWING_LITE:** 1560 (93.4% del total)
  - Avg Score: 0.43 | Avg R:R: 4.46 | Avg DistATR: 3.82
  - Por TF: TF15=158, TF60=1402


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 381 | Unmatched: 655
- 0-10: Wins=131 Losses=250 WR=34.4% (n=381)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=131 Losses=250 WR=34.4% (n=381)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1036 | Aligned=530 (51.2%)
- Core≈ 1.00 | Prox≈ 0.59 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.02 | Confidence≈ 0.00
- SL_TF dist: {'15': 1032, '240': 2, '60': 2} | SL_Structural≈ 100.0%
- TP_TF dist: {'240': 641, '15': 266, '60': 129} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1036, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=0, 15m=1032, 60m=2, 240m=2, 1440m=0
- RR plan por bandas: 0-10≈ 2.02 (n=1036), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 68565 | Zonas con Anchors: 68565
- Dir zonas (zona): Bull=40350 Bear=26522 Neutral=1693
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.6, DirBear≈ 1.0, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 66514, 'tie-bias': 2051}
- TF Triggers: {'5': 49118, '15': 19447}
- TF Anchors: {'60': 68565, '240': 68565, '1440': 68565}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)

## CSV de Trades
- Filas: 102 | Ejecutadas: 21 | Canceladas: 0 | Expiradas: 0
- BUY: 6 | SELL: 117

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 590
- Registered: 51
  - DEDUP_COOLDOWN: 189 | DEDUP_IDENTICAL: 112 | SKIP_CONCURRENCY: 1
- Intentos de registro: 353

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 59.8%
- RegRate = Registered / Intentos = 14.4%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 85.3%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.3%
- ExecRate = Ejecutadas / Registered = 41.2%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 3216 | Total candidatos: 20136 | Seleccionados: 0
- Candidatos por zona (promedio): 6.3

### Take Profit (TP)
- Zonas analizadas: 3216 | Total candidatos: 49708 | Seleccionados: 3216
- Candidatos por zona (promedio): 15.5
- **Edad (barras)** - Candidatos: med=37, max=91 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.69
- **Priority Candidatos**: {'P3': 8608}
- **Priority Seleccionados**: {'NA': 151, 'P3': 2978, 'P0': 87}
- **Type Candidatos**: {'Swing': 8608}
- **Type Seleccionados**: {'P4_Fallback': 151, 'P3_Swing': 2978, 'P0_Zone': 87}
- **TF Candidatos**: {240: 7130, 60: 1184, 15: 294}
- **TF Seleccionados**: {-1: 151, 240: 2299, 15: 456, 5: 145, 60: 165}
- **DistATR** - Candidatos: avg=14.7 | Seleccionados: avg=10.4
- **RR** - Candidatos: avg=3.24 | Seleccionados: avg=1.74
- **Razones de selección**: {'BestIntelligentScore': 3216}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.