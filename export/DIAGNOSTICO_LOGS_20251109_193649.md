# Informe Diagnóstico de Logs - 2025-11-09 19:37:15

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251109_193649.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251109_193649.csv`

## DFM
- Eventos de evaluación: 0
- Evaluaciones Bull: 0 | Bear: 0
- Pasaron umbral (PassedThreshold): 0
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:0, 6:0, 7:0, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 106
- KeptAligned: 28/28 | KeptCounter: 4/4
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.168 | AvgProxCounter≈ 0.038
  - AvgDistATRAligned≈ 0.13 | AvgDistATRCounter≈ 0.00
- PreferAligned eventos: 19 | Filtradas contra-bias: 0

### Proximity (Pre-PreferAligned)
- Eventos: 106
- Aligned pre: 28/32 | Counter pre: 4/32
- AvgProxAligned(pre)≈ 0.168 | AvgDistATRAligned(pre)≈ 0.13

### Proximity Drivers
- Eventos: 106
- Alineadas: n=28 | BaseProx≈ 0.953 | ZoneATR≈ 4.53 | SizePenalty≈ 0.985 | FinalProx≈ 0.940
- Contra-bias: n=4 | BaseProx≈ 0.996 | ZoneATR≈ 2.40 | SizePenalty≈ 1.000 | FinalProx≈ 0.996

## Risk
- Eventos: 23
- Accepted=0 | RejSL=0 | RejTP=7 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 0 (0.0%)
- **P4_FALLBACK:** 4 (100.0%)


## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 3675 | Zonas con Anchors: 3675
- Dir zonas (zona): Bull=3 Bear=3671 Neutral=1
- Resumen por ciclo (promedios): TotHZ≈ 1.5, WithAnchors≈ 1.5, DirBull≈ 0.0, DirBear≈ 1.5, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 3674, 'tie-bias': 1}
- TF Triggers: {'15': 158, '5': 22}
- TF Anchors: {'60': 180, '240': 178, '1440': 92}

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
- Zonas analizadas: 10 | Total candidatos: 56 | Seleccionados: 10
- Candidatos por zona (promedio): 5.6
- **Edad (barras)** - Candidatos: med=84, max=101 | Seleccionados: med=54, max=101
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.50
- **TF Candidatos**: {15: 56}
- **TF Seleccionados**: {15: 10}
- **DistATR** - Candidatos: avg=1.5 | Seleccionados: avg=1.5
- **Razones de selección**: {'InBand[1,3]_TFPreference': 10}
- **En banda [10,15] ATR**: 45/56 (80.4%)

### Take Profit (TP)
- Zonas analizadas: 10 | Total candidatos: 168 | Seleccionados: 10
- Candidatos por zona (promedio): 16.8
- **Edad (barras)** - Candidatos: med=49, max=110 | Seleccionados: med=66, max=107
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.27
- **Priority Candidatos**: {'P3': 168}
- **Priority Seleccionados**: {'P3': 6, 'P4_Fallback': 4}
- **Type Candidatos**: {'Swing': 168}
- **Type Seleccionados**: {'Swing': 6, 'Calculated': 4}
- **TF Candidatos**: {15: 97, 5: 38, 60: 22, 240: 11}
- **TF Seleccionados**: {60: 5, -1: 4, 15: 1}
- **DistATR** - Candidatos: avg=9.5 | Seleccionados: avg=2.9
- **RR** - Candidatos: avg=5.15 | Seleccionados: avg=1.50
- **Razones de selección**: {'Intradía(15→5→60→240)': 6, 'NoStructuralTarget': 4}

### 🎯 Recomendaciones
- ⚠️ TP: 40% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 1.00.