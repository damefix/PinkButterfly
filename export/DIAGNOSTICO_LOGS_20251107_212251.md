# Informe Diagnóstico de Logs - 2025-11-07 21:23:16

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251107_212251.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251107_212251.csv`

## DFM
- Eventos de evaluación: 3
- Evaluaciones Bull: 0 | Bear: 4
- Pasaron umbral (PassedThreshold): 4
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:0, 6:0, 7:1, 8:3, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 105
- KeptAligned: 1/1 | KeptCounter: 202/207
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.005 | AvgProxCounter≈ 0.649
  - AvgDistATRAligned≈ 0.08 | AvgDistATRCounter≈ 2.70
- PreferAligned eventos: 1 | Filtradas contra-bias: 1

### Proximity (Pre-PreferAligned)
- Eventos: 105
- Aligned pre: 1/203 | Counter pre: 202/203
- AvgProxAligned(pre)≈ 0.005 | AvgDistATRAligned(pre)≈ 0.08

### Proximity Drivers
- Eventos: 105
- Alineadas: n=1 | BaseProx≈ 0.493 | ZoneATR≈ 1.49 | SizePenalty≈ 1.000 | FinalProx≈ 0.493
- Contra-bias: n=201 | BaseProx≈ 0.671 | ZoneATR≈ 4.96 | SizePenalty≈ 0.979 | FinalProx≈ 0.657

## Risk
- Eventos: 105
- Accepted=4 | RejSL=0 | RejTP=0 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 4 (2.0%)
- **P4_FALLBACK:** 197 (98.0%)
- **FORCED_P3 por TF:**
  - TF5: 4 (100.0%)


### SLPick por Bandas y TF
- Bandas: lt8=4, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=0, 15m=3, 60m=1, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.08 (n=4), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 4394 | Zonas con Anchors: 4390
- Dir zonas (zona): Bull=123 Bear=4234 Neutral=37
- Resumen por ciclo (promedios): TotHZ≈ 1.9, WithAnchors≈ 1.8, DirBull≈ 0.0, DirBear≈ 1.8, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 4354, 'triggers-only': 3, 'tie-bias': 37}
- TF Triggers: {'15': 144, '5': 64}
- TF Anchors: {'60': 204, '240': 195, '1440': 48}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)

## CSV de Trades
- Filas: 2 | Ejecutadas: 0 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 2

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 4
- Registered: 1
  - DEDUP_COOLDOWN: 2 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 3

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 75.0%
- RegRate = Registered / Intentos = 33.3%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 66.7%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 0.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 201 | Total candidatos: 4058 | Seleccionados: 201
- Candidatos por zona (promedio): 20.2
- **Edad (barras)** - Candidatos: med=62, max=145 | Seleccionados: med=50, max=144
- **Score** - Candidatos: avg=0.38 | Seleccionados: avg=0.46
- **TF Candidatos**: {60: 2222, 15: 1254, 5: 526, 240: 50, 1440: 6}
- **TF Seleccionados**: {60: 160, 240: 2, 1440: 5, 15: 33, 5: 1}
- **DistATR** - Candidatos: avg=7.0 | Seleccionados: avg=6.1
- **Razones de selección**: {'InBand[8,15]_TFPreference': 6, 'Fallback<15': 2, 'InBand[4,8]_TFPreference': 193}
- **En banda [10,15] ATR**: 1113/4058 (27.4%)

### Take Profit (TP)
- Zonas analizadas: 201 | Total candidatos: 711 | Seleccionados: 201
- Candidatos por zona (promedio): 3.5
- **Edad (barras)** - Candidatos: med=24, max=141 | Seleccionados: med=0, max=66
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.01
- **Priority Candidatos**: {'P3': 711}
- **Priority Seleccionados**: {'P4_Fallback': 197, 'P3': 4}
- **Type Candidatos**: {'Swing': 711}
- **Type Seleccionados**: {'Calculated': 197, 'Swing': 4}
- **TF Candidatos**: {15: 288, 5: 260, 60: 99, 240: 62, 1440: 2}
- **TF Seleccionados**: {-1: 197, 5: 4}
- **DistATR** - Candidatos: avg=3.7 | Seleccionados: avg=9.8
- **RR** - Candidatos: avg=0.54 | Seleccionados: avg=1.00
- **Razones de selección**: {'NoStructuralTarget': 197, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 3, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 55% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 98% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.