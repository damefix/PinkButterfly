# Informe Diagnóstico de Logs - 2025-11-07 21:00:17

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251107_205821.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251107_205821.csv`

## DFM
- Eventos de evaluación: 1
- Evaluaciones Bull: 0 | Bear: 1
- Pasaron umbral (PassedThreshold): 0
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:1, 6:0, 7:0, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 96
- KeptAligned: 2/2 | KeptCounter: 173/187
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.008 | AvgProxCounter≈ 0.523
  - AvgDistATRAligned≈ 0.13 | AvgDistATRCounter≈ 2.38
- PreferAligned eventos: 2 | Filtradas contra-bias: 4

### Proximity (Pre-PreferAligned)
- Eventos: 96
- Aligned pre: 2/175 | Counter pre: 173/175
- AvgProxAligned(pre)≈ 0.008 | AvgDistATRAligned(pre)≈ 0.13

### Proximity Drivers
- Eventos: 96
- Alineadas: n=2 | BaseProx≈ 0.377 | ZoneATR≈ 2.40 | SizePenalty≈ 1.000 | FinalProx≈ 0.377
- Contra-bias: n=169 | BaseProx≈ 0.545 | ZoneATR≈ 5.07 | SizePenalty≈ 0.980 | FinalProx≈ 0.535

## Risk
- Eventos: 96
- Accepted=1 | RejSL=0 | RejTP=0 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 2 (1.2%)
- **P4_FALLBACK:** 167 (98.8%)
- **FORCED_P3 por TF:**
  - TF15: 2 (100.0%)


### SLPick por Bandas y TF
- Bandas: lt8=1, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=0, 15m=0, 60m=1, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.01 (n=1), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 3799 | Zonas con Anchors: 3798
- Dir zonas (zona): Bull=206 Bear=3575 Neutral=18
- Resumen por ciclo (promedios): TotHZ≈ 1.9, WithAnchors≈ 1.8, DirBull≈ 0.0, DirBear≈ 1.8, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 3781, 'tie-bias': 18}
- TF Triggers: {'15': 134, '5': 55}
- TF Anchors: {'60': 188, '240': 179, '1440': 48}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 0
- Registered: 0
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 0

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 0.0%
- RegRate = Registered / Intentos = 0.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 0.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 169 | Total candidatos: 3735 | Seleccionados: 169
- Candidatos por zona (promedio): 22.1
- **Edad (barras)** - Candidatos: med=36, max=140 | Seleccionados: med=27, max=73
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 1504, 60: 1043, 15: 632, 5: 387, 1440: 169}
- **TF Seleccionados**: {60: 166, 240: 3}
- **DistATR** - Candidatos: avg=8.8 | Seleccionados: avg=5.7
- **Razones de selección**: {'InBand[4,8]_TFPreference': 165, 'Fallback<15': 4}
- **En banda [10,15] ATR**: 854/3735 (22.9%)

### Take Profit (TP)
- Zonas analizadas: 169 | Total candidatos: 587 | Seleccionados: 169
- Candidatos por zona (promedio): 3.5
- **Edad (barras)** - Candidatos: med=12, max=137 | Seleccionados: med=0, max=13
- **Score** - Candidatos: avg=0.53 | Seleccionados: avg=0.01
- **Priority Candidatos**: {'P3': 587}
- **Priority Seleccionados**: {'P4_Fallback': 167, 'P3': 2}
- **Type Candidatos**: {'Swing': 587}
- **Type Seleccionados**: {'Calculated': 167, 'Swing': 2}
- **TF Candidatos**: {15: 213, 5: 177, 60: 158, 240: 39}
- **TF Seleccionados**: {-1: 167, 15: 2}
- **DistATR** - Candidatos: avg=4.1 | Seleccionados: avg=9.5
- **RR** - Candidatos: avg=0.54 | Seleccionados: avg=1.00
- **Razones de selección**: {'NoStructuralTarget': 167, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 74% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 99% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 1.00.