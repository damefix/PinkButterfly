# Informe Diagnóstico de Logs - 2025-11-04 19:04:48

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_185340.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_185340.csv`

## DFM
- Eventos de evaluación: 534
- Evaluaciones Bull: 84 | Bear: 1978
- Pasaron umbral (PassedThreshold): 2030
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:6, 4:8, 5:516, 6:1503, 7:29, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5484
- KeptAligned: 2124/8218 | KeptCounter: 522/21416
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.056 | AvgProxCounter≈ 0.069
  - AvgDistATRAligned≈ 0.42 | AvgDistATRCounter≈ 0.12
- PreferAligned eventos: 532 | Filtradas contra-bias: 500

### Proximity (Pre-PreferAligned)
- Eventos: 5484
- Aligned pre: 2124/2646 | Counter pre: 522/2646
- AvgProxAligned(pre)≈ 0.056 | AvgDistATRAligned(pre)≈ 0.42

### Proximity Drivers
- Eventos: 5484
- Alineadas: n=2124 | BaseProx≈ 0.578 | ZoneATR≈ 3.14 | SizePenalty≈ 0.996 | FinalProx≈ 0.576
- Contra-bias: n=22 | BaseProx≈ 0.401 | ZoneATR≈ 5.79 | SizePenalty≈ 0.961 | FinalProx≈ 0.384

## Risk
- Eventos: 539
- Accepted=2062 | RejSL=72 | RejTP=0 | RejRR=0 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=65 | SLDistATR≈ 17.46 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=7 | SLDistATR≈ 15.95 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:57,20-25:7,25+:1
- HistSL Counter 0-10:0,10-15:0,15-20:7,20-25:0,25+:0

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 8 | Unmatched: 2054
- 0-10: Wins=0 Losses=2 WR=0.0% (n=2)
- 10-15: Wins=3 Losses=3 WR=50.0% (n=6)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=3 Losses=5 WR=37.5% (n=8)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2062 | Aligned=2047 (99.3%)
- Core≈ 1.00 | Prox≈ 0.58 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.97 | Confidence≈ 0.00
- SL_TF dist: {'60': 75, '-1': 5, '15': 2, '240': 1977, '5': 3} | SL_Structural≈ 99.8%
- TP_TF dist: {'-1': 85, '240': 1977} | TP_Structural≈ 95.9%

### SLPick por Bandas y TF
- Bandas: lt8=988, 8-10=503, 10-12.5=519, 12.5-15=52, >15=0
- TF: 5m=3, 15m=2, 60m=75, 240m=1977, 1440m=0
- RR plan por bandas: 0-10≈ 2.17 (n=1491), 10-15≈ 1.44 (n=571)

## CancelBias (EMA200@60m)
- Eventos: 505
- Distribución Bias: {'Bullish': 13, 'Bearish': 492, 'Neutral': 0}
- Coherencia (Close>EMA): 13/505 (2.6%)

## StructureFusion
- Trazas por zona: 29634 | Zonas con Anchors: 29587
- Dir zonas (zona): Bull=14313 Bear=14317 Neutral=1004
- Resumen por ciclo (promedios): TotHZ≈ 5.4, WithAnchors≈ 5.4, DirBull≈ 2.6, DirBear≈ 2.6, DirNeutral≈ 0.2
- Razones de dirección: {'tie-bias': 1330, 'triggers-only': 43, 'anchors+triggers': 28261}
- TF Triggers: {'5': 12920, '15': 16714}
- TF Anchors: {'60': 29508, '240': 29110}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 1735 | Distribución: {'Bullish': 934, 'Bearish': 801, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 934/1735

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,42': 1, 'score decayó a 0,47': 1, 'score decayó a 0,26': 1, 'estructura no existe': 2, 'score decayó a 0,29': 1}
- Cancel_BOS (diag): por acción {'BUY': 3, 'SELL': 3} | por bias {'Bullish': 3, 'Bearish': 3, 'Neutral': 0}

## CSV de Trades
- Filas: 43 | Ejecutadas: 8 | Canceladas: 0 | Expiradas: 0
- BUY: 34 | SELL: 17

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 2030
- Registered: 22
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 242 | SKIP_CONCURRENCY: 20
- Intentos de registro: 284

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 14.0%
- RegRate = Registered / Intentos = 7.7%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 85.2%
- Concurrency = SKIP_CONCURRENCY / Intentos = 7.0%
- ExecRate = Ejecutadas / Registered = 36.4%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 2142 | Total candidatos: 39225 | Seleccionados: 2140
- Candidatos por zona (promedio): 18.3
- **Edad (barras)** - Candidatos: med=39, max=149 | Seleccionados: med=21, max=99
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.66
- **TF Candidatos**: {60: 20908, 240: 13405, 15: 4284, 5: 628}
- **TF Seleccionados**: {60: 132, 15: 9, 240: 1996, 5: 3}
- **DistATR** - Candidatos: avg=4.5 | Seleccionados: avg=6.9
- **Razones de selección**: {'Fallback<15': 1995, 'InBand[10,15]_TFPreference': 145}
- **En banda [10,15] ATR**: 605/39225 (1.5%)

### Take Profit (TP)
- Zonas analizadas: 2146 | Total candidatos: 51304 | Seleccionados: 2146
- Candidatos por zona (promedio): 23.9
- **Edad (barras)** - Candidatos: med=60, max=2147483647 | Seleccionados: med=74, max=108
- **Score** - Candidatos: avg=0.55 | Seleccionados: avg=0.20
- **Priority Candidatos**: {'P3': 51304}
- **Priority Seleccionados**: {'P4_Fallback': 144, 'P3': 2002}
- **Type Candidatos**: {'Swing': 51304}
- **Type Seleccionados**: {'Calculated': 144, 'Swing': 2002}
- **TF Candidatos**: {5: 21962, 240: 12683, 15: 10330, 60: 6329}
- **TF Seleccionados**: {-1: 144, 240: 2002}
- **DistATR** - Candidatos: avg=4.4 | Seleccionados: avg=17.3
- **RR** - Candidatos: avg=0.55 | Seleccionados: avg=1.94
- **Razones de selección**: {'NoStructuralTarget': 144, 'SwingP3_TF>=60_RR>=Min_Dist>=12': 2002}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- KeptAligned ratio≈ 0.26.