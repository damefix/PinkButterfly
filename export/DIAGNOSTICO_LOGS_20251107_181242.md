# Informe Diagnóstico de Logs - 2025-11-07 18:13:27

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251107_181242.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251107_181242.csv`

## DFM
- Eventos de evaluación: 0
- Evaluaciones Bull: 0 | Bear: 0
- Pasaron umbral (PassedThreshold): 0
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:0, 6:0, 7:0, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 24
- KeptAligned: 0/0 | KeptCounter: 29/32
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.000 | AvgProxCounter≈ 0.694
  - AvgDistATRAligned≈ 0.00 | AvgDistATRCounter≈ 1.31
- PreferAligned eventos: 0 | Filtradas contra-bias: 0

### Proximity (Pre-PreferAligned)
- Eventos: 24
- Aligned pre: 0/29 | Counter pre: 29/29
- AvgProxAligned(pre)≈ 0.000 | AvgDistATRAligned(pre)≈ 0.00

### Proximity Drivers
- Eventos: 24
- Contra-bias: n=29 | BaseProx≈ 0.717 | ZoneATR≈ 4.88 | SizePenalty≈ 0.989 | FinalProx≈ 0.709

## Risk
- Eventos: 23
- Accepted=0 | RejSL=0 | RejTP=0 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 0 (0.0%)
- **P4_FALLBACK:** 29 (100.0%)


## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 1293 | Zonas con Anchors: 1292
- Dir zonas (zona): Bull=0 Bear=1293 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 0.6, WithAnchors≈ 0.6, DirBull≈ 0.0, DirBear≈ 0.6, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 1292, 'triggers-only': 1}
- TF Triggers: {'15': 23, '5': 9}
- TF Anchors: {'60': 32, '240': 27, '1440': 10}

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
- Zonas analizadas: 29 | Total candidatos: 593 | Seleccionados: 29
- Candidatos por zona (promedio): 20.4
- **Edad (barras)** - Candidatos: med=29, max=83 | Seleccionados: med=56, max=68
- **Score** - Candidatos: avg=0.43 | Seleccionados: avg=0.37
- **TF Candidatos**: {240: 261, 60: 195, 15: 102, 1440: 29, 5: 6}
- **TF Seleccionados**: {60: 20, 240: 5, 1440: 4}
- **DistATR** - Candidatos: avg=10.3 | Seleccionados: avg=11.4
- **Razones de selección**: {'InBand[8,15]_TFPreference': 29}
- **En banda [10,15] ATR**: 171/593 (28.8%)

### Take Profit (TP)
- Zonas analizadas: 29 | Total candidatos: 10 | Seleccionados: 29
- Candidatos por zona (promedio): 0.3
- **Edad (barras)** - Candidatos: med=9, max=44 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.40 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 10}
- **Priority Seleccionados**: {'P4_Fallback': 29}
- **Type Candidatos**: {'Swing': 10}
- **Type Seleccionados**: {'Calculated': 29}
- **TF Candidatos**: {5: 7, 15: 3}
- **TF Seleccionados**: {-1: 29}
- **DistATR** - Candidatos: avg=1.6 | Seleccionados: avg=15.6
- **RR** - Candidatos: avg=0.13 | Seleccionados: avg=1.00
- **Razones de selección**: {'NoStructuralTarget': 29}

### 🎯 Recomendaciones
- ⚠️ SL: 86% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 100% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas