# Informe Diagnóstico de Logs - 2025-11-04 19:08:11

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_190028.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_190028.csv`

## DFM
- Eventos de evaluación: 702
- Evaluaciones Bull: 84 | Bear: 2650
- Pasaron umbral (PassedThreshold): 2239
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:6, 4:8, 5:684, 6:2007, 7:29, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5652
- KeptAligned: 2796/9392 | KeptCounter: 690/21758
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.066 | AvgProxCounter≈ 0.095
  - AvgDistATRAligned≈ 0.59 | AvgDistATRCounter≈ 0.12
- PreferAligned eventos: 700 | Filtradas contra-bias: 668

### Proximity (Pre-PreferAligned)
- Eventos: 5652
- Aligned pre: 2796/3486 | Counter pre: 690/3486
- AvgProxAligned(pre)≈ 0.066 | AvgDistATRAligned(pre)≈ 0.59

### Proximity Drivers
- Eventos: 5652
- Alineadas: n=2796 | BaseProx≈ 0.537 | ZoneATR≈ 3.01 | SizePenalty≈ 0.996 | FinalProx≈ 0.535
- Contra-bias: n=22 | BaseProx≈ 0.401 | ZoneATR≈ 5.79 | SizePenalty≈ 0.961 | FinalProx≈ 0.384

## Risk
- Eventos: 707
- Accepted=2734 | RejSL=72 | RejTP=0 | RejRR=0 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=65 | SLDistATR≈ 17.46 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=7 | SLDistATR≈ 15.95 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:57,20-25:7,25+:1
- HistSL Counter 0-10:0,10-15:0,15-20:7,20-25:0,25+:0

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 8 | Unmatched: 2726
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
- Muestras: 2734 | Aligned=2719 (99.5%)
- Core≈ 1.00 | Prox≈ 0.54 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.64 | Confidence≈ 0.00
- SL_TF dist: {'60': 75, '-1': 5, '15': 2, '240': 2649, '5': 3} | SL_Structural≈ 99.8%
- TP_TF dist: {'-1': 85, '240': 2649} | TP_Structural≈ 96.9%

### SLPick por Bandas y TF
- Bandas: lt8=671, 8-10=671, 10-12.5=687, 12.5-15=705, >15=0
- TF: 5m=3, 15m=2, 60m=75, 240m=2649, 1440m=0
- RR plan por bandas: 0-10≈ 1.60 (n=1342), 10-15≈ 1.68 (n=1392)

## CancelBias (EMA200@60m)
- Eventos: 673
- Distribución Bias: {'Bullish': 13, 'Bearish': 660, 'Neutral': 0}
- Coherencia (Close>EMA): 13/673 (1.9%)

## StructureFusion
- Trazas por zona: 31150 | Zonas con Anchors: 31105
- Dir zonas (zona): Bull=14649 Bear=15504 Neutral=997
- Resumen por ciclo (promedios): TotHZ≈ 5.5, WithAnchors≈ 5.5, DirBull≈ 2.6, DirBear≈ 2.7, DirNeutral≈ 0.2
- Razones de dirección: {'tie-bias': 1975, 'triggers-only': 43, 'anchors+triggers': 29132}
- TF Triggers: {'5': 13592, '15': 17558}
- TF Anchors: {'60': 31026, '240': 30629}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 1903 | Distribución: {'Bullish': 934, 'Bearish': 969, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 934/1903

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,42': 1, 'score decayó a 0,47': 1, 'score decayó a 0,26': 1, 'estructura no existe': 2, 'score decayó a 0,29': 1}
- Cancel_BOS (diag): por acción {'BUY': 3, 'SELL': 3} | por bias {'Bullish': 3, 'Bearish': 3, 'Neutral': 0}

## CSV de Trades
- Filas: 43 | Ejecutadas: 8 | Canceladas: 0 | Expiradas: 0
- BUY: 34 | SELL: 17

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 2239
- Registered: 22
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 326 | SKIP_CONCURRENCY: 20
- Intentos de registro: 368

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 16.4%
- RegRate = Registered / Intentos = 6.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 88.6%
- Concurrency = SKIP_CONCURRENCY / Intentos = 5.4%
- ExecRate = Ejecutadas / Registered = 36.4%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 2814 | Total candidatos: 49191 | Seleccionados: 2812
- Candidatos por zona (promedio): 17.5
- **Edad (barras)** - Candidatos: med=34, max=149 | Seleccionados: med=21, max=99
- **Score** - Candidatos: avg=0.50 | Seleccionados: avg=0.67
- **TF Candidatos**: {60: 27628, 240: 17437, 15: 3498, 5: 628}
- **TF Seleccionados**: {60: 132, 15: 9, 240: 2668, 5: 3}
- **DistATR** - Candidatos: avg=4.9 | Seleccionados: avg=7.8
- **Razones de selección**: {'Fallback<15': 2667, 'InBand[10,15]_TFPreference': 145}
- **En banda [10,15] ATR**: 605/49191 (1.2%)

### Take Profit (TP)
- Zonas analizadas: 2818 | Total candidatos: 70231 | Seleccionados: 2818
- Candidatos por zona (promedio): 24.9
- **Edad (barras)** - Candidatos: med=66, max=2147483647 | Seleccionados: med=54, max=108
- **Score** - Candidatos: avg=0.55 | Seleccionados: avg=0.25
- **Priority Candidatos**: {'P3': 70231}
- **Priority Seleccionados**: {'P4_Fallback': 144, 'P3': 2674}
- **Type Candidatos**: {'Swing': 70231}
- **Type Seleccionados**: {'Calculated': 144, 'Swing': 2674}
- **TF Candidatos**: {5: 31313, 240: 16883, 15: 13690, 60: 8345}
- **TF Seleccionados**: {-1: 144, 240: 2674}
- **DistATR** - Candidatos: avg=4.8 | Seleccionados: avg=16.3
- **RR** - Candidatos: avg=0.55 | Seleccionados: avg=1.62
- **Razones de selección**: {'NoStructuralTarget': 144, 'SwingP3_TF>=60_RR>=Min_Dist>=12': 2674}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- KeptAligned ratio≈ 0.30.