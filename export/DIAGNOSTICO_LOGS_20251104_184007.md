# Informe Diagnóstico de Logs - 2025-11-04 18:42:29

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_184007.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_184007.csv`

## DFM
- Eventos de evaluación: 690
- Evaluaciones Bull: 84 | Bear: 2602
- Pasaron umbral (PassedThreshold): 2654
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:6, 4:8, 5:1855, 6:788, 7:29, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5639
- KeptAligned: 3389/8021 | KeptCounter: 678/21086
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.062 | AvgProxCounter≈ 0.082
  - AvgDistATRAligned≈ 0.60 | AvgDistATRCounter≈ 0.17
- PreferAligned eventos: 688 | Filtradas contra-bias: 656

### Proximity (Pre-PreferAligned)
- Eventos: 5639
- Aligned pre: 3389/4067 | Counter pre: 678/4067
- AvgProxAligned(pre)≈ 0.062 | AvgDistATRAligned(pre)≈ 0.60

### Proximity Drivers
- Eventos: 5639
- Alineadas: n=3389 | BaseProx≈ 0.517 | ZoneATR≈ 4.28 | SizePenalty≈ 0.984 | FinalProx≈ 0.509
- Contra-bias: n=22 | BaseProx≈ 0.401 | ZoneATR≈ 5.79 | SizePenalty≈ 0.961 | FinalProx≈ 0.384

## Risk
- Eventos: 695
- Accepted=2686 | RejSL=713 | RejTP=0 | RejRR=0 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=706 | SLDistATR≈ 17.27 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=7 | SLDistATR≈ 15.95 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:698,20-25:7,25+:1
- HistSL Counter 0-10:0,10-15:0,15-20:7,20-25:0,25+:0

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 8 | Unmatched: 2678
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
- Muestras: 2686 | Aligned=2671 (99.4%)
- Core≈ 1.00 | Prox≈ 0.53 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.54 | Confidence≈ 0.00
- SL_TF dist: {'60': 716, '-1': 5, '15': 2, '240': 1960, '5': 3} | SL_Structural≈ 99.8%
- TP_TF dist: {'-1': 85, '240': 2601} | TP_Structural≈ 96.8%

### SLPick por Bandas y TF
- Bandas: lt8=659, 8-10=659, 10-12.5=675, 12.5-15=693, >15=0
- TF: 5m=3, 15m=2, 60m=716, 240m=1960, 1440m=0
- RR plan por bandas: 0-10≈ 1.71 (n=1318), 10-15≈ 1.37 (n=1368)

## CancelBias (EMA200@60m)
- Eventos: 22
- Distribución Bias: {'Bullish': 13, 'Bearish': 9, 'Neutral': 0}
- Coherencia (Close>EMA): 13/22 (59.1%)

## StructureFusion
- Trazas por zona: 29107 | Zonas con Anchors: 29060
- Dir zonas (zona): Bull=13982 Bear=14119 Neutral=1006
- Resumen por ciclo (promedios): TotHZ≈ 5.2, WithAnchors≈ 5.2, DirBull≈ 2.5, DirBear≈ 2.5, DirNeutral≈ 0.2
- Razones de dirección: {'tie-bias': 1973, 'triggers-only': 43, 'anchors+triggers': 27091}
- TF Triggers: {'5': 12259, '15': 16848}
- TF Anchors: {'60': 28981, '240': 28583}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 1891 | Distribución: {'Bullish': 934, 'Bearish': 957, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 934/1891

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,42': 1, 'score decayó a 0,47': 1, 'score decayó a 0,26': 1, 'estructura no existe': 2, 'score decayó a 0,29': 1}
- Cancel_BOS (diag): por acción {'BUY': 3, 'SELL': 3} | por bias {'Bullish': 3, 'Bearish': 3, 'Neutral': 0}

## CSV de Trades
- Filas: 43 | Ejecutadas: 8 | Canceladas: 0 | Expiradas: 0
- BUY: 34 | SELL: 17

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 3407 | Total candidatos: 63121 | Seleccionados: 3405
- Candidatos por zona (promedio): 18.5
- **Edad (barras)** - Candidatos: med=39, max=149 | Seleccionados: med=21, max=99
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.53
- **TF Candidatos**: {60: 33558, 240: 20995, 15: 7940, 5: 628}
- **TF Seleccionados**: {60: 1414, 15: 9, 240: 1979, 5: 3}
- **DistATR** - Candidatos: avg=5.9 | Seleccionados: avg=8.6
- **Razones de selección**: {'Fallback<15': 1978, 'InBand[10,15]_TFPreference': 1427}
- **En banda [10,15] ATR**: 4451/63121 (7.1%)

### Take Profit (TP)
- Zonas analizadas: 3411 | Total candidatos: 66419 | Seleccionados: 3411
- Candidatos por zona (promedio): 19.5
- **Edad (barras)** - Candidatos: med=49, max=147 | Seleccionados: med=54, max=108
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.26
- **Priority Candidatos**: {'P3': 66419}
- **Priority Seleccionados**: {'P4_Fallback': 144, 'P3': 3267}
- **Type Candidatos**: {'Swing': 66419}
- **Type Seleccionados**: {'Calculated': 144, 'Swing': 3267}
- **TF Candidatos**: {5: 30749, 240: 17224, 15: 11527, 60: 6919}
- **TF Seleccionados**: {-1: 144, 240: 3267}
- **DistATR** - Candidatos: avg=5.7 | Seleccionados: avg=18.2
- **RR** - Candidatos: avg=0.60 | Seleccionados: avg=1.59
- **Razones de selección**: {'NoStructuralTarget': 144, 'SwingP3_TF>=60_RR>=Min_Dist>=12': 3267}

### 🎯 Recomendaciones
- ⚠️ SL: 42% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- KeptAligned ratio≈ 0.42.