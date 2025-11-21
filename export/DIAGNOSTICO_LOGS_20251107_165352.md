# Informe Diagnóstico de Logs - 2025-11-07 17:17:42

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251107_165352.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251107_165352.csv`

## DFM
- Eventos de evaluación: 0
- Evaluaciones Bull: 0 | Bear: 0
- Pasaron umbral (PassedThreshold): 0
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:0, 6:0, 7:0, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 27
- KeptAligned: 0/0 | KeptCounter: 38/39
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.000 | AvgProxCounter≈ 0.577
  - AvgDistATRAligned≈ 0.00 | AvgDistATRCounter≈ 2.15
- PreferAligned eventos: 0 | Filtradas contra-bias: 0

### Proximity (Pre-PreferAligned)
- Eventos: 27
- Aligned pre: 0/38 | Counter pre: 38/38
- AvgProxAligned(pre)≈ 0.000 | AvgDistATRAligned(pre)≈ 0.00

### Proximity Drivers
- Eventos: 27
- Contra-bias: n=38 | BaseProx≈ 0.580 | ZoneATR≈ 4.07 | SizePenalty≈ 0.996 | FinalProx≈ 0.577

## Risk
- Eventos: 27
- Accepted=0 | RejSL=0 | RejTP=0 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 0 (0.0%)
- **P4_FALLBACK:** 38 (100.0%)


## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 1414 | Zonas con Anchors: 1412
- Dir zonas (zona): Bull=0 Bear=1414 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 0.7, WithAnchors≈ 0.6, DirBull≈ 0.0, DirBear≈ 0.7, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 1412, 'triggers-only': 2}
- TF Triggers: {'15': 16, '5': 23}
- TF Anchors: {'60': 37, '240': 31, '1440': 3}

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
- Zonas analizadas: 38 | Total candidatos: 995 | Seleccionados: 38
- Candidatos por zona (promedio): 26.2
- **Edad (barras)** - Candidatos: med=31, max=84 | Seleccionados: med=49, max=69
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.36
- **TF Candidatos**: {240: 342, 60: 286, 15: 277, 5: 52, 1440: 38}
- **TF Seleccionados**: {60: 30, 240: 3, 1440: 5}
- **DistATR** - Candidatos: avg=10.4 | Seleccionados: avg=11.4
- **Razones de selección**: {'InBand[8,15]_TFPreference': 38}
- **En banda [10,15] ATR**: 180/995 (18.1%)

### Take Profit (TP)
- Zonas analizadas: 38 | Total candidatos: 56 | Seleccionados: 38
- Candidatos por zona (promedio): 1.5
- **Edad (barras)** - Candidatos: med=17, max=72 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 56}
- **Priority Seleccionados**: {'P4_Fallback': 38}
- **Type Candidatos**: {'Swing': 56}
- **Type Seleccionados**: {'Calculated': 38}
- **TF Candidatos**: {15: 29, 60: 13, 5: 8, 240: 6}
- **TF Seleccionados**: {-1: 38}
- **DistATR** - Candidatos: avg=1.1 | Seleccionados: avg=15.2
- **RR** - Candidatos: avg=0.08 | Seleccionados: avg=1.00
- **Razones de selección**: {'NoStructuralTarget': 38}

### 🎯 Recomendaciones
- ⚠️ SL: 84% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 100% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas