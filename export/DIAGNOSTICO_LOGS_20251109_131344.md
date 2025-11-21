# Informe Diagnóstico de Logs - 2025-11-09 13:14:14

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251109_131344.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251109_131344.csv`

## DFM
- Eventos de evaluación: 6
- Evaluaciones Bull: 3 | Bear: 3
- Pasaron umbral (PassedThreshold): 6
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:0, 6:3, 7:3, 8:0, 9:0

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
- Accepted=6 | RejSL=0 | RejTP=0 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 0 (0.0%)
- **P4_FALLBACK:** 189 (100.0%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 1 (0.5% del total)
  - Avg Score: 0.65 | Avg R:R: 1.27 | Avg DistATR: 6.15
  - Por TF: TF5=1


### SLPick por Bandas y TF
- Bandas: lt8=6, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=0, 15m=0, 60m=6, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.50 (n=6), 10-15≈ 0.00 (n=0)

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
- Filas: 1 | Ejecutadas: 0 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 1

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 6
- Registered: 1
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 1

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 16.7%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 0.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 201 | Total candidatos: 3191 | Seleccionados: 201
- Candidatos por zona (promedio): 15.9
- **Edad (barras)** - Candidatos: med=79, max=150 | Seleccionados: med=75, max=150
- **Score** - Candidatos: avg=0.39 | Seleccionados: avg=0.36
- **TF Candidatos**: {60: 1789, 15: 1299, 5: 103}
- **TF Seleccionados**: {60: 161, 15: 40}
- **DistATR** - Candidatos: avg=6.2 | Seleccionados: avg=6.1
- **Razones de selección**: {'InBand[4,8]_TFPreference': 198, 'Fallback<15': 3}
- **En banda [10,15] ATR**: 831/3191 (26.0%)

### Take Profit (TP)
- Zonas analizadas: 200 | Total candidatos: 1058 | Seleccionados: 200
- Candidatos por zona (promedio): 5.3
- **Edad (barras)** - Candidatos: med=27, max=150 | Seleccionados: med=0, max=150
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.01
- **Priority Candidatos**: {'P3': 1058}
- **Priority Seleccionados**: {'P4_Fallback': 189, 'P3': 11}
- **Type Candidatos**: {'Swing': 1058}
- **Type Seleccionados**: {'Calculated': 189, 'Swing': 11}
- **TF Candidatos**: {15: 506, 5: 262, 60: 185, 240: 102, 1440: 3}
- **TF Seleccionados**: {-1: 189, 15: 3, 5: 8}
- **DistATR** - Candidatos: avg=4.4 | Seleccionados: avg=10.2
- **RR** - Candidatos: avg=0.77 | Seleccionados: avg=1.02
- **Razones de selección**: {'NoStructuralTarget': 189, 'Intradía(15→5→60→240)': 11}

### 🎯 Recomendaciones
- ⚠️ SL: 82% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 94% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 1.00.