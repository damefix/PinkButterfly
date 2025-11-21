# Informe Diagnóstico de Logs - 2025-11-13 11:08:54

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_110431.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_110431.csv`

## DFM
- Eventos de evaluación: 919
- Evaluaciones Bull: 154 | Bear: 674
- Pasaron umbral (PassedThreshold): 828
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:60, 6:365, 7:367, 8:36, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2350
- KeptAligned: 4128/4128 | KeptCounter: 2751/2861
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.440 | AvgProxCounter≈ 0.235
  - AvgDistATRAligned≈ 1.53 | AvgDistATRCounter≈ 1.12
- PreferAligned eventos: 1267 | Filtradas contra-bias: 566

### Proximity (Pre-PreferAligned)
- Eventos: 2350
- Aligned pre: 4128/6879 | Counter pre: 2751/6879
- AvgProxAligned(pre)≈ 0.440 | AvgDistATRAligned(pre)≈ 1.53

### Proximity Drivers
- Eventos: 2350
- Alineadas: n=4128 | BaseProx≈ 0.748 | ZoneATR≈ 5.20 | SizePenalty≈ 0.974 | FinalProx≈ 0.730
- Contra-bias: n=2185 | BaseProx≈ 0.529 | ZoneATR≈ 4.81 | SizePenalty≈ 0.978 | FinalProx≈ 0.520

## Risk
- Eventos: 1939
- Accepted=1262 | RejSL=0 | RejTP=0 | RejRR=1239 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 300 (10.6% del total)
  - Avg Score: 0.38 | Avg R:R: 1.91 | Avg DistATR: 3.72
  - Por TF: TF5=86, TF15=214
- **P0_SWING_LITE:** 2537 (89.4% del total)
  - Avg Score: 0.57 | Avg R:R: 4.39 | Avg DistATR: 3.53
  - Por TF: TF15=513, TF60=2024


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 89 | Unmatched: 1216
- 0-10: Wins=30 Losses=59 WR=33.7% (n=89)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=30 Losses=59 WR=33.7% (n=89)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1305 | Aligned=806 (61.8%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.15 | Confidence≈ 0.00
- SL_TF dist: {'60': 148, '15': 947, '5': 192, '240': 18} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 289, '15': 471, '5': 358, '240': 187} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1262, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=184, 15m=921, 60m=145, 240m=12, 1440m=0
- RR plan por bandas: 0-10≈ 2.12 (n=1262), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10274 | Zonas con Anchors: 10255
- Dir zonas (zona): Bull=3707 Bear=6222 Neutral=345
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'tie-bias': 425, 'anchors+triggers': 9830, 'triggers-only': 19}
- TF Triggers: {'5': 5460, '15': 4814}
- TF Anchors: {'60': 10178, '240': 5986, '1440': 464}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 18, 'score decayó a 0,41': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 212 | Ejecutadas: 44 | Canceladas: 0 | Expiradas: 0
- BUY: 76 | SELL: 180

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 828
- Registered: 110
  - DEDUP_COOLDOWN: 18 | DEDUP_IDENTICAL: 97 | SKIP_CONCURRENCY: 96
- Intentos de registro: 321

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 38.8%
- RegRate = Registered / Intentos = 34.3%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 35.8%
- Concurrency = SKIP_CONCURRENCY / Intentos = 29.9%
- ExecRate = Ejecutadas / Registered = 40.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5444 | Total candidatos: 42458 | Seleccionados: 0
- Candidatos por zona (promedio): 7.8

### Take Profit (TP)
- Zonas analizadas: 5338 | Total candidatos: 52708 | Seleccionados: 5338
- Candidatos por zona (promedio): 9.9
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 52708}
- **Priority Seleccionados**: {'P3': 3631, 'NA': 1407, 'P0': 300}
- **Type Candidatos**: {'Swing': 52708}
- **Type Seleccionados**: {'P3_Swing': 3631, 'P4_Fallback': 1407, 'P0_Zone': 300}
- **TF Candidatos**: {5: 15872, 15: 14303, 60: 14028, 240: 8505}
- **TF Seleccionados**: {60: 990, -1: 1407, 15: 1232, 5: 1002, 240: 707}
- **DistATR** - Candidatos: avg=8.5 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.66 | Seleccionados: avg=1.32
- **Razones de selección**: {'BestIntelligentScore': 5338}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.