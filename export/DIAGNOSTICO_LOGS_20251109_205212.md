# Informe Diagnóstico de Logs - 2025-11-09 20:54:56

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251109_205212.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251109_205212.csv`

## DFM
- Eventos de evaluación: 20
- Evaluaciones Bull: 0 | Bear: 21
- Pasaron umbral (PassedThreshold): 21
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:0, 6:19, 7:1, 8:1, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 114
- KeptAligned: 220/220 | KeptCounter: 4/4
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.816 | AvgProxCounter≈ 0.035
  - AvgDistATRAligned≈ 2.16 | AvgDistATRCounter≈ 0.00
- PreferAligned eventos: 110 | Filtradas contra-bias: 0

### Proximity (Pre-PreferAligned)
- Eventos: 114
- Aligned pre: 220/224 | Counter pre: 4/224
- AvgProxAligned(pre)≈ 0.816 | AvgDistATRAligned(pre)≈ 2.16

### Proximity Drivers
- Eventos: 114
- Alineadas: n=220 | BaseProx≈ 0.871 | ZoneATR≈ 4.29 | SizePenalty≈ 0.985 | FinalProx≈ 0.859
- Contra-bias: n=4 | BaseProx≈ 0.996 | ZoneATR≈ 2.40 | SizePenalty≈ 1.000 | FinalProx≈ 0.996

## Risk
- Eventos: 114
- Accepted=21 | RejSL=0 | RejTP=36 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 0 (0.0%)
- **P4_FALLBACK:** 142 (100.0%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 9 (6.0% del total)
  - Avg Score: 0.69 | Avg R:R: 1.94 | Avg DistATR: 11.80
  - Por TF: TF5=9


### SLPick por Bandas y TF
- Bandas: lt8=21, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=0, 15m=21, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.71 (n=21), 10-15≈ 0.00 (n=0)

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

## CSV de Trades
- Filas: 1 | Ejecutadas: 0 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 1

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 21
- Registered: 1
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 1

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 4.8%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 0.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 181 | Total candidatos: 825 | Seleccionados: 0
- Candidatos por zona (promedio): 4.6

### Take Profit (TP)
- Zonas analizadas: 172 | Total candidatos: 780 | Seleccionados: 172
- Candidatos por zona (promedio): 4.5
- **Edad (barras)** - Candidatos: med=53, max=110 | Seleccionados: med=0, max=107
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.07
- **Priority Candidatos**: {'P3': 780}
- **Priority Seleccionados**: {'P3': 30, 'P4_Fallback': 142}
- **Type Candidatos**: {'Swing': 780}
- **Type Seleccionados**: {'Swing': 30, 'Calculated': 142}
- **TF Candidatos**: {15: 423, 5: 176, 60: 116, 240: 65}
- **TF Seleccionados**: {60: 15, -1: 142, 15: 15}
- **DistATR** - Candidatos: avg=6.5 | Seleccionados: avg=3.2
- **RR** - Candidatos: avg=2.95 | Seleccionados: avg=1.10
- **Razones de selección**: {'Intradía(15→5→60→240)': 30, 'NoStructuralTarget': 142}

### 🎯 Recomendaciones
- ⚠️ TP: 83% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.