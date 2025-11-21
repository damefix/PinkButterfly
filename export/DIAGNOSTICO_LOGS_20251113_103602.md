# Informe Diagnóstico de Logs - 2025-11-13 10:39:33

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_103602.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_103602.csv`

## DFM
- Eventos de evaluación: 920
- Evaluaciones Bull: 152 | Bear: 661
- Pasaron umbral (PassedThreshold): 813
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:73, 6:345, 7:354, 8:41, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2351
- KeptAligned: 4115/4115 | KeptCounter: 2760/2863
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.441 | AvgProxCounter≈ 0.233
  - AvgDistATRAligned≈ 1.50 | AvgDistATRCounter≈ 1.14
- PreferAligned eventos: 1276 | Filtradas contra-bias: 595

### Proximity (Pre-PreferAligned)
- Eventos: 2351
- Aligned pre: 4115/6875 | Counter pre: 2760/6875
- AvgProxAligned(pre)≈ 0.441 | AvgDistATRAligned(pre)≈ 1.50

### Proximity Drivers
- Eventos: 2351
- Alineadas: n=4115 | BaseProx≈ 0.754 | ZoneATR≈ 5.21 | SizePenalty≈ 0.975 | FinalProx≈ 0.735
- Contra-bias: n=2165 | BaseProx≈ 0.525 | ZoneATR≈ 4.75 | SizePenalty≈ 0.979 | FinalProx≈ 0.516

## Risk
- Eventos: 1951
- Accepted=1251 | RejSL=0 | RejTP=0 | RejRR=1299 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 309 (11.0% del total)
  - Avg Score: 0.39 | Avg R:R: 1.88 | Avg DistATR: 3.75
  - Por TF: TF5=87, TF15=222
- **P0_SWING_LITE:** 2499 (89.0% del total)
  - Avg Score: 0.58 | Avg R:R: 4.15 | Avg DistATR: 3.52
  - Por TF: TF15=534, TF60=1965


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 95 | Unmatched: 1187
- 0-10: Wins=21 Losses=74 WR=22.1% (n=95)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=21 Losses=74 WR=22.1% (n=95)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1282 | Aligned=781 (60.9%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.09 | Confidence≈ 0.00
- SL_TF dist: {'60': 160, '15': 927, '5': 178, '240': 17} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 303, '5': 346, '15': 475, '240': 158} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1251, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=173, 15m=908, 60m=156, 240m=14, 1440m=0
- RR plan por bandas: 0-10≈ 2.07 (n=1251), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10292 | Zonas con Anchors: 10280
- Dir zonas (zona): Bull=3754 Bear=6216 Neutral=322
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9904, 'tie-bias': 376, 'triggers-only': 12}
- TF Triggers: {'5': 5449, '15': 4843}
- TF Anchors: {'60': 10201, '240': 5974, '1440': 445}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 24, 'score decayó a 0,46': 1, 'score decayó a 0,21': 1, 'score decayó a 0,20': 1, 'score decayó a 0,42': 1, 'score decayó a 0,40': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 219 | Ejecutadas: 43 | Canceladas: 0 | Expiradas: 0
- BUY: 76 | SELL: 186

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 813
- Registered: 114
  - DEDUP_COOLDOWN: 13 | DEDUP_IDENTICAL: 105 | SKIP_CONCURRENCY: 80
- Intentos de registro: 312

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 38.4%
- RegRate = Registered / Intentos = 36.5%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 37.8%
- Concurrency = SKIP_CONCURRENCY / Intentos = 25.6%
- ExecRate = Ejecutadas / Registered = 37.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5414 | Total candidatos: 42629 | Seleccionados: 0
- Candidatos por zona (promedio): 7.9

### Take Profit (TP)
- Zonas analizadas: 5317 | Total candidatos: 51803 | Seleccionados: 5317
- Candidatos por zona (promedio): 9.7
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 51803}
- **Priority Seleccionados**: {'P3': 3642, 'NA': 1365, 'P0': 310}
- **Type Candidatos**: {'Swing': 51803}
- **Type Seleccionados**: {'P3_Swing': 3642, 'P4_Fallback': 1365, 'P0_Zone': 310}
- **TF Candidatos**: {5: 15629, 15: 14074, 60: 13740, 240: 8360}
- **TF Seleccionados**: {60: 1011, -1: 1365, 5: 991, 15: 1238, 240: 712}
- **DistATR** - Candidatos: avg=8.5 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.45 | Seleccionados: avg=1.30
- **Razones de selección**: {'BestIntelligentScore': 5317}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.