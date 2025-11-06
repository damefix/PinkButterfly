# Informe Diagnóstico de Logs - 2025-11-04 20:26:06

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_202257.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_202257.csv`

## DFM
- Eventos de evaluación: 51
- Evaluaciones Bull: 93 | Bear: 45
- Pasaron umbral (PassedThreshold): 100
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:6, 4:13, 5:35, 6:55, 7:29, 8:0, 9:0

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
- Accepted=138 | RejSL=57 | RejTP=0 | RejRR=0 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=53 | SLDistATR≈ 17.30 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=4 | SLDistATR≈ 15.38 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:48,20-25:5,25+:0
- HistSL Counter 0-10:0,10-15:0,15-20:4,20-25:0,25+:0

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 9 | Unmatched: 129
- 0-10: Wins=1 Losses=2 WR=33.3% (n=3)
- 10-15: Wins=2 Losses=4 WR=33.3% (n=6)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=3 Losses=6 WR=33.3% (n=9)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 138 | Aligned=120 (87.0%)
- Core≈ 1.00 | Prox≈ 0.56 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.18 | Confidence≈ 0.00
- SL_TF dist: {'60': 97, '-1': 5, '240': 30, '5': 4, '15': 2} | SL_Structural≈ 96.4%
- TP_TF dist: {'-1': 86, '240': 50, '5': 1, '60': 1} | TP_Structural≈ 37.7%

### SLPick por Bandas y TF
- Bandas: lt8=18, 8-10=28, 10-12.5=51, 12.5-15=41, >15=0
- TF: 5m=4, 15m=2, 60m=97, 240m=30, 1440m=0
- RR plan por bandas: 0-10≈ 1.31 (n=46), 10-15≈ 1.11 (n=92)

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
- Expiraciones: {'score decayó a 0,42': 1, 'score decayó a 0,47': 1, 'score decayó a 0,26': 1, 'estructura no existe': 1, 'score decayó a 0,29': 1}
- Cancel_BOS (diag): por acción {'BUY': 4, 'SELL': 3} | por bias {'Bullish': 3, 'Bearish': 4, 'Neutral': 0}

## CSV de Trades
- Filas: 44 | Ejecutadas: 9 | Canceladas: 0 | Expiradas: 0
- BUY: 37 | SELL: 16

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 100
- Registered: 22
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 19
- Intentos de registro: 41

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 41.0%
- RegRate = Registered / Intentos = 53.7%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 46.3%
- ExecRate = Ejecutadas / Registered = 40.9%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 202 | Total candidatos: 4785 | Seleccionados: 200
- Candidatos por zona (promedio): 23.7
- **Edad (barras)** - Candidatos: med=43, max=149 | Seleccionados: med=47, max=99
- **Score** - Candidatos: avg=0.43 | Seleccionados: avg=0.40
- **TF Candidatos**: {240: 1765, 60: 1508, 15: 885, 5: 627}
- **TF Seleccionados**: {60: 150, 240: 39, 15: 7, 5: 4}
- **DistATR** - Candidatos: avg=14.2 | Seleccionados: avg=10.1
- **Razones de selección**: {'Fallback<15': 33, 'InBand[8,15]_TFPreference': 167}
- **En banda [10,15] ATR**: 966/4785 (20.2%)

### Take Profit (TP)
- Zonas analizadas: 206 | Total candidatos: 2319 | Seleccionados: 206
- Candidatos por zona (promedio): 11.3
- **Edad (barras)** - Candidatos: med=38, max=147 | Seleccionados: med=0, max=108
- **Score** - Candidatos: avg=0.50 | Seleccionados: avg=0.21
- **Priority Candidatos**: {'P3': 2319}
- **Priority Seleccionados**: {'P4_Fallback': 135, 'P3': 71}
- **Type Candidatos**: {'Swing': 2319}
- **Type Seleccionados**: {'Calculated': 135, 'Swing': 71}
- **TF Candidatos**: {15: 630, 5: 622, 240: 558, 60: 509}
- **TF Seleccionados**: {-1: 135, 240: 69, 5: 1, 60: 1}
- **DistATR** - Candidatos: avg=8.9 | Seleccionados: avg=14.2
- **RR** - Candidatos: avg=0.81 | Seleccionados: avg=1.14
- **Razones de selección**: {'NoStructuralTarget': 135, 'SwingP3_TF>=60_RR>=Min_Dist>=8': 70, 'SwingP3_ANYTF_RR>=Min_Dist>=8': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 72% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 66% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.04.