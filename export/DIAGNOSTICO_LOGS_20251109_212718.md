# Informe Diagnóstico de Logs - 2025-11-09 21:28:00

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251109_212718.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251109_212718.csv`

## DFM
- Eventos de evaluación: 29
- Evaluaciones Bull: 0 | Bear: 63
- Pasaron umbral (PassedThreshold): 63
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:0, 6:9, 7:17, 8:37, 9:0

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
- Accepted=63 | RejSL=0 | RejTP=22 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 0 (0.0%)
- **P4_FALLBACK:** 146 (100.0%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 42 (20.2% del total)
  - Avg Score: 0.68 | Avg R:R: 1.99 | Avg DistATR: 8.47
  - Por TF: TF5=10, TF15=32
- **P0_SWING_LITE:** 20 (9.6% del total)
  - Avg Score: 0.48 | Avg R:R: 7.24 | Avg DistATR: 7.14
  - Por TF: TF15=20


### SLPick por Bandas y TF
- Bandas: lt8=63, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=3, 15m=60, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 3.65 (n=63), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 1010 | Zonas con Anchors: 1010
- Dir zonas (zona): Bull=3 Bear=1006 Neutral=1
- Resumen por ciclo (promedios): TotHZ≈ 2.1, WithAnchors≈ 2.1, DirBull≈ 0.0, DirBear≈ 2.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 1009, 'tie-bias': 1}
- TF Triggers: {'15': 211, '5': 38}
- TF Anchors: {'60': 249, '240': 247, '1440': 136}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 3 | Ejecutadas: 0 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 3

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 63
- Registered: 2
  - DEDUP_COOLDOWN: 1 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 7
- Intentos de registro: 10

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 15.9%
- RegRate = Registered / Intentos = 20.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 10.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 70.0%
- ExecRate = Ejecutadas / Registered = 0.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 216 | Total candidatos: 1525 | Seleccionados: 0
- Candidatos por zona (promedio): 7.1

### Take Profit (TP)
- Zonas analizadas: 154 | Total candidatos: 350 | Seleccionados: 154
- Candidatos por zona (promedio): 2.3
- **Edad (barras)** - Candidatos: med=66, max=110 | Seleccionados: med=0, max=66
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.02
- **Priority Candidatos**: {'P3': 350}
- **Priority Seleccionados**: {'P4_Fallback': 146, 'P3': 8}
- **Type Candidatos**: {'Swing': 350}
- **Type Seleccionados**: {'Calculated': 146, 'Swing': 8}
- **TF Candidatos**: {15: 202, 60: 80, 240: 51, 5: 17}
- **TF Seleccionados**: {-1: 146, 60: 7, 15: 1}
- **DistATR** - Candidatos: avg=3.3 | Seleccionados: avg=3.2
- **RR** - Candidatos: avg=1.56 | Seleccionados: avg=1.04
- **Razones de selección**: {'NoStructuralTarget': 146, 'Intradía(15→5→60→240)': 8}

### 🎯 Recomendaciones
- ⚠️ TP: 95% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.