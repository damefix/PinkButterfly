# Informe Diagnóstico de Logs - 2025-11-09 19:28:19

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251109_192755.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251109_192755.csv`

## DFM
- Eventos de evaluación: 0
- Evaluaciones Bull: 0 | Bear: 0
- Pasaron umbral (PassedThreshold): 0
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:0, 6:0, 7:0, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 78
- KeptAligned: 23/23 | KeptCounter: 3/3
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.206 | AvgProxCounter≈ 0.038
  - AvgDistATRAligned≈ 0.13 | AvgDistATRCounter≈ 0.00
- PreferAligned eventos: 17 | Filtradas contra-bias: 0

### Proximity (Pre-PreferAligned)
- Eventos: 78
- Aligned pre: 23/26 | Counter pre: 3/26
- AvgProxAligned(pre)≈ 0.206 | AvgDistATRAligned(pre)≈ 0.13

### Proximity Drivers
- Eventos: 78
- Alineadas: n=23 | BaseProx≈ 0.958 | ZoneATR≈ 4.92 | SizePenalty≈ 0.983 | FinalProx≈ 0.942
- Contra-bias: n=3 | BaseProx≈ 0.997 | ZoneATR≈ 2.49 | SizePenalty≈ 1.000 | FinalProx≈ 0.997

## Risk
- Eventos: 20
- Accepted=0 | RejSL=0 | RejTP=1 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 0 (0.0%)
- **P4_FALLBACK:** 8 (100.0%)


## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 7155 | Zonas con Anchors: 7155
- Dir zonas (zona): Bull=3 Bear=7151 Neutral=1
- Resumen por ciclo (promedios): TotHZ≈ 0.8, WithAnchors≈ 0.8, DirBull≈ 0.0, DirBear≈ 0.8, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 7154, 'tie-bias': 1}
- TF Triggers: {'15': 88, '5': 12}
- TF Anchors: {'60': 100, '240': 100, '1440': 46}

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
- Zonas analizadas: 9 | Total candidatos: 36 | Seleccionados: 9
- Candidatos por zona (promedio): 4.0
- **Edad (barras)** - Candidatos: med=84, max=101 | Seleccionados: med=101, max=101
- **Score** - Candidatos: avg=0.43 | Seleccionados: avg=0.42
- **TF Candidatos**: {15: 36}
- **TF Seleccionados**: {15: 9}
- **DistATR** - Candidatos: avg=1.1 | Seleccionados: avg=1.4
- **Razones de selección**: {'InBand[1,3]_TFPreference': 9}
- **En banda [10,15] ATR**: 21/36 (58.3%)

### Take Profit (TP)
- Zonas analizadas: 9 | Total candidatos: 55 | Seleccionados: 9
- Candidatos por zona (promedio): 6.1
- **Edad (barras)** - Candidatos: med=37, max=110 | Seleccionados: med=0, max=66
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.05
- **Priority Candidatos**: {'P3': 55}
- **Priority Seleccionados**: {'P4_Fallback': 8, 'P3': 1}
- **Type Candidatos**: {'Swing': 55}
- **Type Seleccionados**: {'Calculated': 8, 'Swing': 1}
- **TF Candidatos**: {15: 29, 5: 18, 60: 6, 240: 2}
- **TF Seleccionados**: {-1: 8, 60: 1}
- **DistATR** - Candidatos: avg=11.2 | Seleccionados: avg=3.0
- **RR** - Candidatos: avg=4.94 | Seleccionados: avg=1.10
- **Razones de selección**: {'NoStructuralTarget': 8, 'Intradía(15→5→60→240)': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 67% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 89% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 1.00.