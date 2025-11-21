# Informe Diagnóstico de Logs - 2025-11-13 11:15:26

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_111058.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_111058.csv`

## DFM
- Eventos de evaluación: 919
- Evaluaciones Bull: 156 | Bear: 675
- Pasaron umbral (PassedThreshold): 831
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:60, 6:367, 7:368, 8:36, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2351
- KeptAligned: 4124/4124 | KeptCounter: 2756/2866
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.439 | AvgProxCounter≈ 0.236
  - AvgDistATRAligned≈ 1.53 | AvgDistATRCounter≈ 1.12
- PreferAligned eventos: 1267 | Filtradas contra-bias: 566

### Proximity (Pre-PreferAligned)
- Eventos: 2351
- Aligned pre: 4124/6880 | Counter pre: 2756/6880
- AvgProxAligned(pre)≈ 0.439 | AvgDistATRAligned(pre)≈ 1.53

### Proximity Drivers
- Eventos: 2351
- Alineadas: n=4124 | BaseProx≈ 0.748 | ZoneATR≈ 5.20 | SizePenalty≈ 0.974 | FinalProx≈ 0.729
- Contra-bias: n=2190 | BaseProx≈ 0.530 | ZoneATR≈ 4.81 | SizePenalty≈ 0.978 | FinalProx≈ 0.520

## Risk
- Eventos: 1941
- Accepted=1262 | RejSL=0 | RejTP=0 | RejRR=1238 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 296 (10.4% del total)
  - Avg Score: 0.38 | Avg R:R: 1.91 | Avg DistATR: 3.70
  - Por TF: TF5=83, TF15=213
- **P0_SWING_LITE:** 2553 (89.6% del total)
  - Avg Score: 0.57 | Avg R:R: 4.39 | Avg DistATR: 3.53
  - Por TF: TF15=514, TF60=2039


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 86 | Unmatched: 1219
- 0-10: Wins=30 Losses=56 WR=34.9% (n=86)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=30 Losses=56 WR=34.9% (n=86)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1305 | Aligned=809 (62.0%)
- Core≈ 1.00 | Prox≈ 0.68 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.16 | Confidence≈ 0.00
- SL_TF dist: {'60': 147, '15': 948, '5': 192, '240': 18} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 291, '15': 471, '5': 355, '240': 188} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1262, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=184, 15m=922, 60m=144, 240m=12, 1440m=0
- RR plan por bandas: 0-10≈ 2.13 (n=1262), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10268 | Zonas con Anchors: 10249
- Dir zonas (zona): Bull=3701 Bear=6227 Neutral=340
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'tie-bias': 421, 'anchors+triggers': 9828, 'triggers-only': 19}
- TF Triggers: {'5': 5458, '15': 4810}
- TF Anchors: {'60': 10172, '240': 5990, '1440': 468}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 17, 'score decayó a 0,41': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 208 | Ejecutadas: 43 | Canceladas: 0 | Expiradas: 0
- BUY: 71 | SELL: 180

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 831
- Registered: 108
  - DEDUP_COOLDOWN: 18 | DEDUP_IDENTICAL: 96 | SKIP_CONCURRENCY: 97
- Intentos de registro: 319

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 38.4%
- RegRate = Registered / Intentos = 33.9%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 35.7%
- Concurrency = SKIP_CONCURRENCY / Intentos = 30.4%
- ExecRate = Ejecutadas / Registered = 39.8%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5451 | Total candidatos: 42461 | Seleccionados: 0
- Candidatos por zona (promedio): 7.8

### Take Profit (TP)
- Zonas analizadas: 5346 | Total candidatos: 52849 | Seleccionados: 5346
- Candidatos por zona (promedio): 9.9
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 52849}
- **Priority Seleccionados**: {'P3': 3636, 'NA': 1412, 'P0': 298}
- **Type Candidatos**: {'Swing': 52849}
- **Type Seleccionados**: {'P3_Swing': 3636, 'P4_Fallback': 1412, 'P0_Zone': 298}
- **TF Candidatos**: {5: 15910, 15: 14327, 60: 14048, 240: 8564}
- **TF Seleccionados**: {60: 994, -1: 1412, 15: 1231, 5: 1003, 240: 706}
- **DistATR** - Candidatos: avg=8.5 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.69 | Seleccionados: avg=1.33
- **Razones de selección**: {'BestIntelligentScore': 5346}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.