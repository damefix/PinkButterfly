# Informe Diagnóstico de Logs - 2025-11-09 18:47:20

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251109_184646.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251109_184646.csv`

## DFM
- Eventos de evaluación: 7
- Evaluaciones Bull: 0 | Bear: 7
- Pasaron umbral (PassedThreshold): 7
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:0, 6:0, 7:3, 8:4, 9:0

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
- Accepted=7 | RejSL=0 | RejTP=3 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 0 (0.0%)
- **P4_FALLBACK:** 146 (100.0%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 17 (10.4% del total)
  - Avg Score: 0.66 | Avg R:R: 1.77 | Avg DistATR: 9.62
  - Por TF: TF5=17


### SLPick por Bandas y TF
- Bandas: lt8=7, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=0, 15m=7, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.17 (n=7), 10-15≈ 0.00 (n=0)

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

## CSV de Trades
- Filas: 5 | Ejecutadas: 0 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 5

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 7
- Registered: 3
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 3

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 42.9%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 0.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 201 | Total candidatos: 1014 | Seleccionados: 199
- Candidatos por zona (promedio): 5.0
- **Edad (barras)** - Candidatos: med=61, max=150 | Seleccionados: med=58, max=150
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.39
- **TF Candidatos**: {15: 543, 60: 387, 5: 84}
- **TF Seleccionados**: {60: 84, 15: 103, 5: 12}
- **DistATR** - Candidatos: avg=1.7 | Seleccionados: avg=1.5
- **Razones de selección**: {'InBand[1,3]_TFPreference': 197, 'Fallback<15': 2}
- **En banda [10,15] ATR**: 881/1014 (86.9%)

### Take Profit (TP)
- Zonas analizadas: 184 | Total candidatos: 769 | Seleccionados: 184
- Candidatos por zona (promedio): 4.2
- **Edad (barras)** - Candidatos: med=58, max=150 | Seleccionados: med=0, max=150
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.07
- **Priority Candidatos**: {'P3': 769}
- **Priority Seleccionados**: {'P4_Fallback': 146, 'P3': 38}
- **Type Candidatos**: {'Swing': 769}
- **Type Seleccionados**: {'Calculated': 146, 'Swing': 38}
- **TF Candidatos**: {15: 347, 5: 188, 60: 149, 240: 82, 1440: 3}
- **TF Seleccionados**: {-1: 146, 15: 24, 5: 14}
- **DistATR** - Candidatos: avg=6.0 | Seleccionados: avg=4.4
- **RR** - Candidatos: avg=1.66 | Seleccionados: avg=1.10
- **Razones de selección**: {'NoStructuralTarget': 146, 'Intradía(15→5→60→240)': 38}

### 🎯 Recomendaciones
- ⚠️ SL: 50% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 79% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.