# Informe Diagnóstico de Logs - 2025-11-04 20:49:57

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_204656.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_204656.csv`

## DFM
- Eventos de evaluación: 52
- Evaluaciones Bull: 103 | Bear: 35
- Pasaron umbral (PassedThreshold): 97
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:7, 4:16, 5:33, 6:51, 7:31, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 4998
- KeptAligned: 184/4826 | KeptCounter: 37/20444
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.005 | AvgProxCounter≈ 0.001
  - AvgDistATRAligned≈ 0.05 | AvgDistATRCounter≈ 0.01
- PreferAligned eventos: 47 | Filtradas contra-bias: 15

### Proximity (Pre-PreferAligned)
- Eventos: 4998
- Aligned pre: 184/221 | Counter pre: 37/221
- AvgProxAligned(pre)≈ 0.005 | AvgDistATRAligned(pre)≈ 0.05

### Proximity Drivers
- Eventos: 4998
- Alineadas: n=184 | BaseProx≈ 0.524 | ZoneATR≈ 6.02 | SizePenalty≈ 0.957 | FinalProx≈ 0.504
- Contra-bias: n=22 | BaseProx≈ 0.365 | ZoneATR≈ 5.42 | SizePenalty≈ 0.965 | FinalProx≈ 0.349

## Risk
- Eventos: 54
- Accepted=138 | RejSL=60 | RejTP=0 | RejRR=0 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=56 | SLDistATR≈ 17.26 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=4 | SLDistATR≈ 15.38 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:51,20-25:5,25+:0
- HistSL Counter 0-10:0,10-15:0,15-20:4,20-25:0,25+:0

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 8 | Unmatched: 130
- 0-10: Wins=1 Losses=2 WR=33.3% (n=3)
- 10-15: Wins=2 Losses=3 WR=40.0% (n=5)
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
- Muestras: 138 | Aligned=120 (87.0%)
- Core≈ 1.00 | Prox≈ 0.55 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.27 | Confidence≈ 0.00
- SL_TF dist: {'1440': 16, '-1': 5, '60': 87, '240': 22, '15': 2, '5': 6} | SL_Structural≈ 96.4%
- TP_TF dist: {'-1': 81, '1440': 44, '240': 11, '5': 1, '60': 1} | TP_Structural≈ 41.3%

### SLPick por Bandas y TF
- Bandas: lt8=18, 8-10=28, 10-12.5=48, 12.5-15=44, >15=0
- TF: 5m=6, 15m=2, 60m=87, 240m=22, 1440m=16
- RR plan por bandas: 0-10≈ 1.53 (n=46), 10-15≈ 1.14 (n=92)

## CancelBias (EMA200@60m)
- Eventos: 21
- Distribución Bias: {'Bullish': 13, 'Bearish': 8, 'Neutral': 0}
- Coherencia (Close>EMA): 13/21 (61.9%)

## StructureFusion
- Trazas por zona: 25270 | Zonas con Anchors: 25249
- Dir zonas (zona): Bull=14840 Bear=9774 Neutral=656
- Resumen por ciclo (promedios): TotHZ≈ 5.1, WithAnchors≈ 5.0, DirBull≈ 3.0, DirBear≈ 2.0, DirNeutral≈ 0.1
- Razones de dirección: {'tie-bias': 875, 'anchors+triggers': 24375, 'triggers-only': 20}
- TF Triggers: {'5': 10982, '15': 14288}
- TF Anchors: {'60': 25164, '240': 24777, '1440': 20815}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 1250 | Distribución: {'Bullish': 937, 'Bearish': 313, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 937/1250

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,42': 1, 'estructura no existe': 2, 'score decayó a 0,47': 1, 'score decayó a 0,27': 1, 'score decayó a 0,26': 1, 'score decayó a 0,50': 1, 'score decayó a 0,29': 1}
- Cancel_BOS (diag): por acción {'BUY': 4, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 4, 'Neutral': 0}

## CSV de Trades
- Filas: 46 | Ejecutadas: 8 | Canceladas: 0 | Expiradas: 0
- BUY: 39 | SELL: 15

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 97
- Registered: 23
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 18
- Intentos de registro: 41

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 42.3%
- RegRate = Registered / Intentos = 56.1%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 43.9%
- ExecRate = Ejecutadas / Registered = 34.8%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 204 | Total candidatos: 5365 | Seleccionados: 201
- Candidatos por zona (promedio): 26.3
- **Edad (barras)** - Candidatos: med=38, max=149 | Seleccionados: med=43, max=99
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.44
- **TF Candidatos**: {240: 1811, 60: 1467, 15: 862, 5: 643, 1440: 582}
- **TF Seleccionados**: {1440: 25, 60: 134, 240: 29, 15: 7, 5: 6}
- **DistATR** - Candidatos: avg=18.6 | Seleccionados: avg=10.3
- **Razones de selección**: {'InBand[8,15]_TFPreference': 170, 'Fallback<15': 31}
- **En banda [10,15] ATR**: 1013/5365 (18.9%)

### Take Profit (TP)
- Zonas analizadas: 206 | Total candidatos: 2668 | Seleccionados: 206
- Candidatos por zona (promedio): 13.0
- **Edad (barras)** - Candidatos: med=33, max=147 | Seleccionados: med=0, max=33
- **Score** - Candidatos: avg=0.53 | Seleccionados: avg=0.29
- **Priority Candidatos**: {'P3': 2668}
- **Priority Seleccionados**: {'P4_Fallback': 127, 'P3': 79}
- **Type Candidatos**: {'Swing': 2668}
- **Type Seleccionados**: {'Calculated': 127, 'Swing': 79}
- **TF Candidatos**: {15: 631, 5: 603, 60: 540, 240: 516, 1440: 378}
- **TF Seleccionados**: {-1: 127, 1440: 64, 240: 13, 5: 1, 60: 1}
- **DistATR** - Candidatos: avg=11.1 | Seleccionados: avg=15.4
- **RR** - Candidatos: avg=1.01 | Seleccionados: avg=1.24
- **Razones de selección**: {'NoStructuralTarget': 127, 'SwingP3_TF>=60_RR>=Min_Dist>=8': 78, 'SwingP3_ANYTF_RR>=Min_Dist>=8': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 65% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 62% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.04.