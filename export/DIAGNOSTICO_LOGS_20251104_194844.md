# Informe Diagnóstico de Logs - 2025-11-04 19:51:37

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_194844.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_194844.csv`

## DFM
- Eventos de evaluación: 49
- Evaluaciones Bull: 84 | Bear: 38
- Pasaron umbral (PassedThreshold): 90
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:6, 4:8, 5:31, 6:48, 7:29, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 4999
- KeptAligned: 184/4823 | KeptCounter: 37/20450
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.005 | AvgProxCounter≈ 0.001
  - AvgDistATRAligned≈ 0.05 | AvgDistATRCounter≈ 0.01
- PreferAligned eventos: 47 | Filtradas contra-bias: 15

### Proximity (Pre-PreferAligned)
- Eventos: 4999
- Aligned pre: 184/221 | Counter pre: 37/221
- AvgProxAligned(pre)≈ 0.005 | AvgDistATRAligned(pre)≈ 0.05

### Proximity Drivers
- Eventos: 4999
- Alineadas: n=184 | BaseProx≈ 0.521 | ZoneATR≈ 5.96 | SizePenalty≈ 0.958 | FinalProx≈ 0.501
- Contra-bias: n=22 | BaseProx≈ 0.401 | ZoneATR≈ 5.79 | SizePenalty≈ 0.961 | FinalProx≈ 0.384

## Risk
- Eventos: 54
- Accepted=122 | RejSL=72 | RejTP=0 | RejRR=0 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=65 | SLDistATR≈ 17.46 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=7 | SLDistATR≈ 15.95 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:57,20-25:7,25+:1
- HistSL Counter 0-10:0,10-15:0,15-20:7,20-25:0,25+:0

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 8 | Unmatched: 114
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
- Muestras: 122 | Aligned=107 (87.7%)
- Core≈ 1.00 | Prox≈ 0.58 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.20 | Confidence≈ 0.00
- SL_TF dist: {'60': 75, '-1': 5, '15': 2, '240': 37, '5': 3} | SL_Structural≈ 95.9%
- TP_TF dist: {'-1': 85, '240': 37} | TP_Structural≈ 30.3%

### SLPick por Bandas y TF
- Bandas: lt8=18, 8-10=18, 10-12.5=34, 12.5-15=52, >15=0
- TF: 5m=3, 15m=2, 60m=75, 240m=37, 1440m=0
- RR plan por bandas: 0-10≈ 1.51 (n=36), 10-15≈ 1.08 (n=86)

## CancelBias (EMA200@60m)
- Eventos: 21
- Distribución Bias: {'Bullish': 13, 'Bearish': 8, 'Neutral': 0}
- Coherencia (Close>EMA): 13/21 (61.9%)

## StructureFusion
- Trazas por zona: 25273 | Zonas con Anchors: 25232
- Dir zonas (zona): Bull=13339 Bear=10936 Neutral=998
- Resumen por ciclo (promedios): TotHZ≈ 5.1, WithAnchors≈ 5.0, DirBull≈ 2.7, DirBear≈ 2.2, DirNeutral≈ 0.2
- Razones de dirección: {'triggers-only': 41, 'anchors+triggers': 23909, 'tie-bias': 1323}
- TF Triggers: {'15': 14294, '5': 10979}
- TF Anchors: {'60': 25153, '240': 24756}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 1250 | Distribución: {'Bullish': 934, 'Bearish': 316, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 934/1250

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,42': 1, 'score decayó a 0,47': 1, 'score decayó a 0,26': 1, 'estructura no existe': 2, 'score decayó a 0,29': 1}
- Cancel_BOS (diag): por acción {'BUY': 3, 'SELL': 3} | por bias {'Bullish': 3, 'Bearish': 3, 'Neutral': 0}

## CSV de Trades
- Filas: 42 | Ejecutadas: 8 | Canceladas: 0 | Expiradas: 0
- BUY: 34 | SELL: 16

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 90
- Registered: 21
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 20
- Intentos de registro: 41

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 45.6%
- RegRate = Registered / Intentos = 51.2%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 48.8%
- ExecRate = Ejecutadas / Registered = 38.1%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 202 | Total candidatos: 4785 | Seleccionados: 200
- Candidatos por zona (promedio): 23.7
- **Edad (barras)** - Candidatos: med=43, max=149 | Seleccionados: med=48, max=99
- **Score** - Candidatos: avg=0.43 | Seleccionados: avg=0.40
- **TF Candidatos**: {240: 1765, 60: 1508, 15: 885, 5: 627}
- **TF Seleccionados**: {60: 132, 15: 9, 240: 56, 5: 3}
- **DistATR** - Candidatos: avg=14.2 | Seleccionados: avg=10.8
- **Razones de selección**: {'Fallback<15': 55, 'InBand[10,15]_TFPreference': 145}
- **En banda [10,15] ATR**: 605/4785 (12.6%)

### Take Profit (TP)
- Zonas analizadas: 206 | Total candidatos: 2319 | Seleccionados: 206
- Candidatos por zona (promedio): 11.3
- **Edad (barras)** - Candidatos: med=38, max=147 | Seleccionados: med=0, max=108
- **Score** - Candidatos: avg=0.50 | Seleccionados: avg=0.18
- **Priority Candidatos**: {'P3': 2319}
- **Priority Seleccionados**: {'P4_Fallback': 144, 'P3': 62}
- **Type Candidatos**: {'Swing': 2319}
- **Type Seleccionados**: {'Calculated': 144, 'Swing': 62}
- **TF Candidatos**: {15: 630, 5: 622, 240: 558, 60: 509}
- **TF Seleccionados**: {-1: 144, 240: 62}
- **DistATR** - Candidatos: avg=8.9 | Seleccionados: avg=15.1
- **RR** - Candidatos: avg=0.77 | Seleccionados: avg=1.16
- **Razones de selección**: {'NoStructuralTarget': 144, 'SwingP3_TF>=60_RR>=Min_Dist>=12': 62}

### 🎯 Recomendaciones
- ⚠️ SL: 75% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 70% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.04.