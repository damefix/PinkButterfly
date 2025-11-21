# Informe Diagnóstico de Logs - 2025-11-09 19:43:55

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251109_194324.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251109_194324.csv`

## DFM
- Eventos de evaluación: 0
- Evaluaciones Bull: 0 | Bear: 0
- Pasaron umbral (PassedThreshold): 0
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:0, 6:0, 7:0, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 114
- KeptAligned: 36/36 | KeptCounter: 4/4
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.194 | AvgProxCounter≈ 0.035
  - AvgDistATRAligned≈ 0.11 | AvgDistATRCounter≈ 0.00
- PreferAligned eventos: 23 | Filtradas contra-bias: 0

### Proximity (Pre-PreferAligned)
- Eventos: 114
- Aligned pre: 36/40 | Counter pre: 4/40
- AvgProxAligned(pre)≈ 0.194 | AvgDistATRAligned(pre)≈ 0.11

### Proximity Drivers
- Eventos: 114
- Alineadas: n=36 | BaseProx≈ 0.963 | ZoneATR≈ 3.48 | SizePenalty≈ 0.991 | FinalProx≈ 0.954
- Contra-bias: n=4 | BaseProx≈ 0.996 | ZoneATR≈ 2.40 | SizePenalty≈ 1.000 | FinalProx≈ 0.996

## Risk
- Eventos: 27
- Accepted=0 | RejSL=0 | RejTP=17 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 0 (0.0%)
- **P4_FALLBACK:** 6 (100.0%)


## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 1007 | Zonas con Anchors: 1007
- Dir zonas (zona): Bull=3 Bear=1003 Neutral=1
- Resumen por ciclo (promedios): TotHZ≈ 2.1, WithAnchors≈ 2.1, DirBull≈ 0.0, DirBear≈ 2.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 1006, 'tie-bias': 1}
- TF Triggers: {'15': 211, '5': 38}
- TF Anchors: {'60': 249, '240': 247, '1440': 136}

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
- Zonas analizadas: 20 | Total candidatos: 125 | Seleccionados: 20
- Candidatos por zona (promedio): 6.2
- **Edad (barras)** - Candidatos: med=75, max=142 | Seleccionados: med=54, max=92
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.50
- **TF Candidatos**: {15: 122, 5: 3}
- **TF Seleccionados**: {15: 20}
- **DistATR** - Candidatos: avg=1.5 | Seleccionados: avg=1.5
- **Razones de selección**: {'InBand[1,3]_TFPreference': 20}
- **En banda [10,15] ATR**: 102/125 (81.6%)

### Take Profit (TP)
- Zonas analizadas: 20 | Total candidatos: 343 | Seleccionados: 20
- Candidatos por zona (promedio): 17.1
- **Edad (barras)** - Candidatos: med=53, max=110 | Seleccionados: med=66, max=107
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.32
- **Priority Candidatos**: {'P3': 343}
- **Priority Seleccionados**: {'P3': 14, 'P4_Fallback': 6}
- **Type Candidatos**: {'Swing': 343}
- **Type Seleccionados**: {'Swing': 14, 'Calculated': 6}
- **TF Candidatos**: {15: 207, 5: 58, 60: 51, 240: 27}
- **TF Seleccionados**: {60: 12, 15: 2, -1: 6}
- **DistATR** - Candidatos: avg=8.0 | Seleccionados: avg=2.9
- **RR** - Candidatos: avg=4.66 | Seleccionados: avg=1.59
- **Razones de selección**: {'Intradía(15→5→60→240)': 14, 'NoStructuralTarget': 6}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- KeptAligned ratio≈ 1.00.