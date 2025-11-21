# Informe Diagnóstico de Logs - 2025-11-19 16:56:56

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_165055.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_165055.csv`

## DFM
- Eventos de evaluación: 545
- Evaluaciones Bull: 0 | Bear: 476
- Pasaron umbral (PassedThreshold): 476
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:9, 6:164, 7:189, 8:114, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3095
- KeptAligned: 2285/2285 | KeptCounter: 2256/2408
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.182 | AvgProxCounter≈ 0.134
  - AvgDistATRAligned≈ 0.78 | AvgDistATRCounter≈ 0.65
- PreferAligned eventos: 793 | Filtradas contra-bias: 8

### Proximity (Pre-PreferAligned)
- Eventos: 3095
- Aligned pre: 2285/4541 | Counter pre: 2256/4541
- AvgProxAligned(pre)≈ 0.182 | AvgDistATRAligned(pre)≈ 0.78

### Proximity Drivers
- Eventos: 3095
- Alineadas: n=2285 | BaseProx≈ 0.724 | ZoneATR≈ 4.95 | SizePenalty≈ 0.979 | FinalProx≈ 0.709
- Contra-bias: n=2248 | BaseProx≈ 0.522 | ZoneATR≈ 6.08 | SizePenalty≈ 0.962 | FinalProx≈ 0.503

## Risk
- Eventos: 1613
- Accepted=687 | RejSL=0 | RejTP=0 | RejRR=1136 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 474 (13.6% del total)
  - Avg Score: 0.40 | Avg R:R: 1.81 | Avg DistATR: 3.59
  - Por TF: TF5=35, TF15=439
- **P0_SWING_LITE:** 3023 (86.4% del total)
  - Avg Score: 0.60 | Avg R:R: 3.40 | Avg DistATR: 3.94
  - Por TF: TF15=522, TF60=2501


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 137 | Unmatched: 550
- 0-10: Wins=111 Losses=26 WR=81.0% (n=137)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=111 Losses=26 WR=81.0% (n=137)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 687 | Aligned=410 (59.7%)
- Core≈ 1.00 | Prox≈ 0.61 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.05 | Confidence≈ 0.00
- SL_TF dist: {'15': 346, '5': 294, '60': 35, '240': 12} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 195, '240': 376, '5': 83, '60': 33} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=687, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=294, 15m=346, 60m=35, 240m=12, 1440m=0
- RR plan por bandas: 0-10≈ 2.05 (n=687), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 16340 | Zonas con Anchors: 16340
- Dir zonas (zona): Bull=41 Bear=16237 Neutral=62
- Resumen por ciclo (promedios): TotHZ≈ 5.2, WithAnchors≈ 5.2, DirBull≈ 0.0, DirBear≈ 5.2, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 16268, 'tie-bias': 72}
- TF Triggers: {'15': 8283, '5': 8057}
- TF Anchors: {'60': 16340, '240': 16340, '1440': 16340}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 5, 'score decayó a 0,49': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 10} | por bias {'Bullish': 10, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 85 | Ejecutadas: 11 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 96

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 476
- Registered: 44
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 44

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 9.2%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 25.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4348 | Total candidatos: 65416 | Seleccionados: 0
- Candidatos por zona (promedio): 15.0

### Take Profit (TP)
- Zonas analizadas: 4348 | Total candidatos: 98532 | Seleccionados: 4348
- Candidatos por zona (promedio): 22.7
- **Edad (barras)** - Candidatos: med=83, max=468 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.54 | Seleccionados: avg=0.72
- **Priority Candidatos**: {'P3': 59485}
- **Priority Seleccionados**: {'P3': 3177, 'P0': 386, 'NA': 785}
- **Type Candidatos**: {'Swing': 59485}
- **Type Seleccionados**: {'P3_Swing': 3177, 'P0_Zone': 386, 'P4_Fallback': 785}
- **TF Candidatos**: {240: 38387, 60: 9667, 15: 6832, 5: 4599}
- **TF Seleccionados**: {240: 2234, 15: 662, -1: 785, 60: 261, 5: 406}
- **DistATR** - Candidatos: avg=17.5 | Seleccionados: avg=5.6
- **RR** - Candidatos: avg=7.04 | Seleccionados: avg=1.58
- **Razones de selección**: {'BestIntelligentScore': 4348}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.