# Informe Diagnóstico de Logs - 2025-11-09 18:59:28

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251109_185901.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251109_185901.csv`

## DFM
- Eventos de evaluación: 3
- Evaluaciones Bull: 0 | Bear: 3
- Pasaron umbral (PassedThreshold): 3
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:0, 6:0, 7:0, 8:3, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 78
- KeptAligned: 97/97 | KeptCounter: 3/3
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.812 | AvgProxCounter≈ 0.038
  - AvgDistATRAligned≈ 2.14 | AvgDistATRCounter≈ 0.00
- PreferAligned eventos: 75 | Filtradas contra-bias: 0

### Proximity (Pre-PreferAligned)
- Eventos: 78
- Aligned pre: 97/100 | Counter pre: 3/100
- AvgProxAligned(pre)≈ 0.812 | AvgDistATRAligned(pre)≈ 2.14

### Proximity Drivers
- Eventos: 78
- Alineadas: n=97 | BaseProx≈ 0.862 | ZoneATR≈ 5.07 | SizePenalty≈ 0.981 | FinalProx≈ 0.846
- Contra-bias: n=3 | BaseProx≈ 0.997 | ZoneATR≈ 2.49 | SizePenalty≈ 1.000 | FinalProx≈ 0.997

## Risk
- Eventos: 78
- Accepted=3 | RejSL=0 | RejTP=12 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 0 (0.0%)
- **P4_FALLBACK:** 85 (100.0%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 5 (5.6% del total)
  - Avg Score: 0.62 | Avg R:R: 1.89 | Avg DistATR: 11.36
  - Por TF: TF5=5


### SLPick por Bandas y TF
- Bandas: lt8=3, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=0, 15m=3, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.14 (n=3), 10-15≈ 0.00 (n=0)

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

## CSV de Trades
- Filas: 4 | Ejecutadas: 0 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 4

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 3
- Registered: 2
  - DEDUP_COOLDOWN: 1 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 3

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 100.0%
- RegRate = Registered / Intentos = 66.7%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 33.3%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 0.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 99 | Total candidatos: 399 | Seleccionados: 98
- Candidatos por zona (promedio): 4.0
- **Edad (barras)** - Candidatos: med=61, max=150 | Seleccionados: med=58, max=150
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.42
- **TF Candidatos**: {60: 227, 15: 162, 5: 10}
- **TF Seleccionados**: {60: 54, 15: 44}
- **DistATR** - Candidatos: avg=1.7 | Seleccionados: avg=1.5
- **Razones de selección**: {'InBand[1,3]_TFPreference': 98}
- **En banda [10,15] ATR**: 311/399 (77.9%)

### Take Profit (TP)
- Zonas analizadas: 93 | Total candidatos: 386 | Seleccionados: 93
- Candidatos por zona (promedio): 4.2
- **Edad (barras)** - Candidatos: med=49, max=150 | Seleccionados: med=0, max=87
- **Score** - Candidatos: avg=0.43 | Seleccionados: avg=0.04
- **Priority Candidatos**: {'P3': 386}
- **Priority Seleccionados**: {'P3': 8, 'P4_Fallback': 85}
- **Type Candidatos**: {'Swing': 386}
- **Type Seleccionados**: {'Swing': 8, 'Calculated': 85}
- **TF Candidatos**: {15: 167, 5: 112, 60: 71, 240: 34, 1440: 2}
- **TF Seleccionados**: {60: 5, -1: 85, 15: 3}
- **DistATR** - Candidatos: avg=7.5 | Seleccionados: avg=2.8
- **RR** - Candidatos: avg=3.53 | Seleccionados: avg=1.05
- **Razones de selección**: {'Intradía(15→5→60→240)': 8, 'NoStructuralTarget': 85}

### 🎯 Recomendaciones
- ⚠️ SL: 43% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 91% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.