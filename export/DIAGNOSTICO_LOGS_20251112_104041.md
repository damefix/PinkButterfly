# Informe Diagnóstico de Logs - 2025-11-12 11:10:41

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_104041.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251112_104041.csv`

## DFM
- Eventos de evaluación: 793
- Evaluaciones Bull: 8615 | Bear: 25765
- Pasaron umbral (PassedThreshold): 28383
- ConfidenceBins acumulado: 0:0, 1:0, 2:37, 3:205, 4:3100, 5:10045, 6:14043, 7:6594, 8:356, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2060
- KeptAligned: 229286/230886 | KeptCounter: 101766/159223
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.481 | AvgProxCounter≈ 0.217
  - AvgDistATRAligned≈ 2.98 | AvgDistATRCounter≈ 1.25
- PreferAligned eventos: 1258 | Filtradas contra-bias: 27595

### Proximity (Pre-PreferAligned)
- Eventos: 2060
- Aligned pre: 229286/331052 | Counter pre: 101766/331052
- AvgProxAligned(pre)≈ 0.481 | AvgDistATRAligned(pre)≈ 2.98

### Proximity Drivers
- Eventos: 2060
- Alineadas: n=229286 | BaseProx≈ 0.659 | ZoneATR≈ 8.17 | SizePenalty≈ 0.934 | FinalProx≈ 0.621
- Contra-bias: n=74171 | BaseProx≈ 0.477 | ZoneATR≈ 6.82 | SizePenalty≈ 0.959 | FinalProx≈ 0.460

## Risk
- Eventos: 1759
- Accepted=34380 | RejSL=0 | RejTP=0 | RejRR=36963 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 3478 (4.3% del total)
  - Avg Score: 0.42 | Avg R:R: 1.89 | Avg DistATR: 4.49
  - Por TF: TF5=1371, TF15=2107
- **P0_SWING_LITE:** 78085 (95.7% del total)
  - Avg Score: 0.50 | Avg R:R: 2.61 | Avg DistATR: 3.84
  - Por TF: TF15=7243, TF60=70842


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 3772 | Unmatched: 31159
- 0-10: Wins=1137 Losses=2635 WR=30.1% (n=3772)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=1137 Losses=2635 WR=30.1% (n=3772)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 34931 | Aligned=24803 (71.0%)
- Core≈ 1.00 | Prox≈ 0.58 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.92 | Confidence≈ 0.00
- SL_TF dist: {'60': 5251, '15': 29054, '5': 598, '240': 28} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 10493, '15': 9099, '5': 6411, '240': 8928} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=34380, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=553, 15m=28669, 60m=5130, 240m=28, 1440m=0
- RR plan por bandas: 0-10≈ 1.93 (n=34380), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 446382 | Zonas con Anchors: 446306
- Dir zonas (zona): Bull=167294 Bear=254671 Neutral=24417
- Resumen por ciclo (promedios): TotHZ≈ 180.5, WithAnchors≈ 180.4, DirBull≈ 69.7, DirBear≈ 100.6, DirNeutral≈ 10.2
- Razones de dirección: {'anchors+triggers': 420260, 'tie-bias': 26050, 'triggers-only': 72}
- TF Triggers: {'5': 282939, '15': 142238}
- TF Anchors: {'60': 424370, '240': 253307}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 18, 'score decayó a 0,21': 1, 'estructura inactiva': 1}
- Cancel_BOS (diag): por acción {'BUY': 1, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 1, 'Neutral': 0}

## CSV de Trades
- Filas: 218 | Ejecutadas: 45 | Canceladas: 0 | Expiradas: 0
- BUY: 115 | SELL: 148

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 28383
- Registered: 110
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 160 | SKIP_CONCURRENCY: 90
- Intentos de registro: 360

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 1.3%
- RegRate = Registered / Intentos = 30.6%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 44.4%
- Concurrency = SKIP_CONCURRENCY / Intentos = 25.0%
- ExecRate = Ejecutadas / Registered = 40.9%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 212469 | Total candidatos: 1144601 | Seleccionados: 0
- Candidatos por zona (promedio): 5.4

### Take Profit (TP)
- Zonas analizadas: 212414 | Total candidatos: 969003 | Seleccionados: 212414
- Candidatos por zona (promedio): 4.6
- **Edad (barras)** - Candidatos: med=52, max=217 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.55
- **Priority Candidatos**: {'P3': 969003}
- **Priority Seleccionados**: {'NA': 76858, 'P3': 117727, 'P0': 17829}
- **Type Candidatos**: {'Swing': 969003}
- **Type Seleccionados**: {'P4_Fallback': 76858, 'P3_Swing': 117727, 'P0_Zone': 17829}
- **TF Candidatos**: {60: 402535, 15: 204356, 5: 182009, 240: 180103}
- **TF Seleccionados**: {-1: 76858, 15: 35195, 60: 34356, 5: 33520, 240: 32485}
- **DistATR** - Candidatos: avg=10.0 | Seleccionados: avg=8.4
- **RR** - Candidatos: avg=2.28 | Seleccionados: avg=1.20
- **Razones de selección**: {'BestIntelligentScore': 212414}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 0.99.