# Informe Diagnóstico de Logs - 2025-11-19 16:45:21

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_163911.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_163911.csv`

## DFM
- Eventos de evaluación: 546
- Evaluaciones Bull: 0 | Bear: 480
- Pasaron umbral (PassedThreshold): 480
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:13, 6:166, 7:187, 8:114, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3094
- KeptAligned: 2282/2282 | KeptCounter: 2254/2406
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.182 | AvgProxCounter≈ 0.134
  - AvgDistATRAligned≈ 0.77 | AvgDistATRCounter≈ 0.65
- PreferAligned eventos: 793 | Filtradas contra-bias: 8

### Proximity (Pre-PreferAligned)
- Eventos: 3094
- Aligned pre: 2282/4536 | Counter pre: 2254/4536
- AvgProxAligned(pre)≈ 0.182 | AvgDistATRAligned(pre)≈ 0.77

### Proximity Drivers
- Eventos: 3094
- Alineadas: n=2282 | BaseProx≈ 0.724 | ZoneATR≈ 4.94 | SizePenalty≈ 0.980 | FinalProx≈ 0.710
- Contra-bias: n=2246 | BaseProx≈ 0.522 | ZoneATR≈ 6.09 | SizePenalty≈ 0.962 | FinalProx≈ 0.504

## Risk
- Eventos: 1612
- Accepted=690 | RejSL=0 | RejTP=0 | RejRR=1137 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 475 (13.6% del total)
  - Avg Score: 0.40 | Avg R:R: 1.81 | Avg DistATR: 3.59
  - Por TF: TF5=36, TF15=439
- **P0_SWING_LITE:** 3030 (86.4% del total)
  - Avg Score: 0.60 | Avg R:R: 3.42 | Avg DistATR: 3.94
  - Por TF: TF15=522, TF60=2508


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 137 | Unmatched: 553
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
- Muestras: 690 | Aligned=414 (60.0%)
- Core≈ 1.00 | Prox≈ 0.61 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.08 | Confidence≈ 0.00
- SL_TF dist: {'15': 349, '5': 294, '60': 35, '240': 12} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 190, '240': 375, '5': 84, '60': 41} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=690, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=294, 15m=349, 60m=35, 240m=12, 1440m=0
- RR plan por bandas: 0-10≈ 2.08 (n=690), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 16333 | Zonas con Anchors: 16333
- Dir zonas (zona): Bull=41 Bear=16232 Neutral=60
- Resumen por ciclo (promedios): TotHZ≈ 5.2, WithAnchors≈ 5.2, DirBull≈ 0.0, DirBear≈ 5.2, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 16263, 'tie-bias': 70}
- TF Triggers: {'15': 8279, '5': 8054}
- TF Anchors: {'60': 16333, '240': 16333, '1440': 16333}

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
- DFM Señales (PassedThreshold): 480
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
- Zonas analizadas: 4345 | Total candidatos: 65368 | Seleccionados: 0
- Candidatos por zona (promedio): 15.0

### Take Profit (TP)
- Zonas analizadas: 4345 | Total candidatos: 98528 | Seleccionados: 4345
- Candidatos por zona (promedio): 22.7
- **Edad (barras)** - Candidatos: med=84, max=468 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.54 | Seleccionados: avg=0.72
- **Priority Candidatos**: {'P3': 59481}
- **Priority Seleccionados**: {'P3': 3177, 'P0': 383, 'NA': 785}
- **Type Candidatos**: {'Swing': 59481}
- **Type Seleccionados**: {'P3_Swing': 3177, 'P0_Zone': 383, 'P4_Fallback': 785}
- **TF Candidatos**: {240: 38403, 60: 9685, 15: 6842, 5: 4551}
- **TF Seleccionados**: {240: 2233, 15: 651, -1: 785, 60: 258, 5: 418}
- **DistATR** - Candidatos: avg=17.5 | Seleccionados: avg=5.6
- **RR** - Candidatos: avg=7.09 | Seleccionados: avg=1.59
- **Razones de selección**: {'BestIntelligentScore': 4345}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.