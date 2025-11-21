# Informe Diagnóstico de Logs - 2025-11-13 10:25:54

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_102104.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_102104.csv`

## DFM
- Eventos de evaluación: 921
- Evaluaciones Bull: 152 | Bear: 657
- Pasaron umbral (PassedThreshold): 809
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:73, 6:345, 7:350, 8:41, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2350
- KeptAligned: 4118/4118 | KeptCounter: 2752/2855
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.441 | AvgProxCounter≈ 0.233
  - AvgDistATRAligned≈ 1.51 | AvgDistATRCounter≈ 1.14
- PreferAligned eventos: 1276 | Filtradas contra-bias: 592

### Proximity (Pre-PreferAligned)
- Eventos: 2350
- Aligned pre: 4118/6870 | Counter pre: 2752/6870
- AvgProxAligned(pre)≈ 0.441 | AvgDistATRAligned(pre)≈ 1.51

### Proximity Drivers
- Eventos: 2350
- Alineadas: n=4118 | BaseProx≈ 0.754 | ZoneATR≈ 5.20 | SizePenalty≈ 0.975 | FinalProx≈ 0.735
- Contra-bias: n=2160 | BaseProx≈ 0.525 | ZoneATR≈ 4.75 | SizePenalty≈ 0.979 | FinalProx≈ 0.516

## Risk
- Eventos: 1951
- Accepted=1249 | RejSL=0 | RejTP=0 | RejRR=1302 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 309 (11.0% del total)
  - Avg Score: 0.38 | Avg R:R: 1.88 | Avg DistATR: 3.75
  - Por TF: TF5=87, TF15=222
- **P0_SWING_LITE:** 2498 (89.0% del total)
  - Avg Score: 0.57 | Avg R:R: 4.16 | Avg DistATR: 3.52
  - Por TF: TF15=534, TF60=1964


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 95 | Unmatched: 1185
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
- Muestras: 1280 | Aligned=781 (61.0%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.09 | Confidence≈ 0.00
- SL_TF dist: {'60': 160, '15': 926, '5': 177, '240': 17} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 301, '5': 346, '15': 475, '240': 158} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1249, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=172, 15m=907, 60m=156, 240m=14, 1440m=0
- RR plan por bandas: 0-10≈ 2.07 (n=1249), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10290 | Zonas con Anchors: 10278
- Dir zonas (zona): Bull=3755 Bear=6216 Neutral=319
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9905, 'tie-bias': 373, 'triggers-only': 12}
- TF Triggers: {'5': 5448, '15': 4842}
- TF Anchors: {'60': 10199, '240': 5969, '1440': 440}

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
- DFM Señales (PassedThreshold): 809
- Registered: 114
  - DEDUP_COOLDOWN: 13 | DEDUP_IDENTICAL: 105 | SKIP_CONCURRENCY: 79
- Intentos de registro: 311

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 38.4%
- RegRate = Registered / Intentos = 36.7%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 37.9%
- Concurrency = SKIP_CONCURRENCY / Intentos = 25.4%
- ExecRate = Ejecutadas / Registered = 37.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5415 | Total candidatos: 42668 | Seleccionados: 0
- Candidatos por zona (promedio): 7.9

### Take Profit (TP)
- Zonas analizadas: 5318 | Total candidatos: 51772 | Seleccionados: 5318
- Candidatos por zona (promedio): 9.7
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 51772}
- **Priority Seleccionados**: {'P3': 3637, 'NA': 1371, 'P0': 310}
- **Type Candidatos**: {'Swing': 51772}
- **Type Seleccionados**: {'P3_Swing': 3637, 'P4_Fallback': 1371, 'P0_Zone': 310}
- **TF Candidatos**: {5: 15623, 15: 14069, 60: 13745, 240: 8335}
- **TF Seleccionados**: {60: 1008, -1: 1371, 5: 991, 15: 1236, 240: 712}
- **DistATR** - Candidatos: avg=8.5 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.46 | Seleccionados: avg=1.30
- **Razones de selección**: {'BestIntelligentScore': 5318}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.