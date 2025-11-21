# Informe Diagnóstico de Logs - 2025-11-09 13:33:53

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251109_133320.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251109_133320.csv`

## DFM
- Eventos de evaluación: 0
- Evaluaciones Bull: 0 | Bear: 0
- Pasaron umbral (PassedThreshold): 0
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:0, 6:0, 7:0, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 111
- KeptAligned: 198/198 | KeptCounter: 4/4
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.801 | AvgProxCounter≈ 0.036
  - AvgDistATRAligned≈ 2.38 | AvgDistATRCounter≈ 0.00
- PreferAligned eventos: 107 | Filtradas contra-bias: 0

### Proximity (Pre-PreferAligned)
- Eventos: 111
- Aligned pre: 198/202 | Counter pre: 4/202
- AvgProxAligned(pre)≈ 0.801 | AvgDistATRAligned(pre)≈ 2.38

### Proximity Drivers
- Eventos: 111
- Alineadas: n=198 | BaseProx≈ 0.844 | ZoneATR≈ 4.51 | SizePenalty≈ 0.983 | FinalProx≈ 0.831
- Contra-bias: n=4 | BaseProx≈ 0.996 | ZoneATR≈ 2.40 | SizePenalty≈ 1.000 | FinalProx≈ 0.996

## Risk
- Eventos: 111
- Accepted=0 | RejSL=0 | RejTP=0 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 0 (0.0%)
- **P4_FALLBACK:** 91 (100.0%)


## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 2744 | Zonas con Anchors: 2744
- Dir zonas (zona): Bull=3 Bear=2740 Neutral=1
- Resumen por ciclo (promedios): TotHZ≈ 1.7, WithAnchors≈ 1.7, DirBull≈ 0.0, DirBear≈ 1.7, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 2743, 'tie-bias': 1}
- TF Triggers: {'15': 175, '5': 27}
- TF Anchors: {'60': 202, '240': 200, '1440': 105}

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
- Zonas analizadas: 97 | Total candidatos: 1956 | Seleccionados: 97
- Candidatos por zona (promedio): 20.2
- **Edad (barras)** - Candidatos: med=75, max=150 | Seleccionados: med=75, max=150
- **Score** - Candidatos: avg=0.41 | Seleccionados: avg=0.37
- **TF Candidatos**: {60: 950, 15: 903, 5: 103}
- **TF Seleccionados**: {60: 59, 15: 38}
- **DistATR** - Candidatos: avg=7.1 | Seleccionados: avg=5.9
- **Razones de selección**: {'InBand[4,8]_TFPreference': 97}
- **En banda [10,15] ATR**: 457/1956 (23.4%)

### Take Profit (TP)
- Zonas analizadas: 97 | Total candidatos: 388 | Seleccionados: 97
- Candidatos por zona (promedio): 4.0
- **Edad (barras)** - Candidatos: med=22, max=101 | Seleccionados: med=0, max=53
- **Score** - Candidatos: avg=0.38 | Seleccionados: avg=0.01
- **Priority Candidatos**: {'P3': 388}
- **Priority Seleccionados**: {'P4_Fallback': 91, 'P3': 6}
- **Type Candidatos**: {'Swing': 388}
- **Type Seleccionados**: {'Calculated': 91, 'Swing': 6}
- **TF Candidatos**: {5: 193, 15: 164, 60: 27, 240: 4}
- **TF Seleccionados**: {-1: 91, 5: 6}
- **DistATR** - Candidatos: avg=5.0 | Seleccionados: avg=8.3
- **RR** - Candidatos: avg=0.83 | Seleccionados: avg=1.02
- **Razones de selección**: {'NoStructuralTarget': 91, 'Intradía(15→5→60→240)': 6}

### 🎯 Recomendaciones
- ⚠️ SL: 74% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 94% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 1.00.