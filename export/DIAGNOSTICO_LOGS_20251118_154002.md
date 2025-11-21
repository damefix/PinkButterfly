# Informe Diagnóstico de Logs - 2025-11-18 15:46:13

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251118_154002.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251118_154002.csv`

## DFM
- Eventos de evaluación: 251
- Evaluaciones Bull: 12 | Bear: 193
- Pasaron umbral (PassedThreshold): 205
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:3, 6:59, 7:88, 8:55, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 628
- KeptAligned: 967/967 | KeptCounter: 1086/1120
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.383 | AvgProxCounter≈ 0.252
  - AvgDistATRAligned≈ 1.33 | AvgDistATRCounter≈ 1.67
- PreferAligned eventos: 271 | Filtradas contra-bias: 153

### Proximity (Pre-PreferAligned)
- Eventos: 628
- Aligned pre: 967/2053 | Counter pre: 1086/2053
- AvgProxAligned(pre)≈ 0.383 | AvgDistATRAligned(pre)≈ 1.33

### Proximity Drivers
- Eventos: 628
- Alineadas: n=967 | BaseProx≈ 0.770 | ZoneATR≈ 4.94 | SizePenalty≈ 0.979 | FinalProx≈ 0.755
- Contra-bias: n=933 | BaseProx≈ 0.442 | ZoneATR≈ 4.67 | SizePenalty≈ 0.980 | FinalProx≈ 0.434

## Risk
- Eventos: 558
- Accepted=333 | RejSL=0 | RejTP=0 | RejRR=332 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 82 (10.6% del total)
  - Avg Score: 0.36 | Avg R:R: 1.87 | Avg DistATR: 3.71
  - Por TF: TF5=27, TF15=55
- **P0_SWING_LITE:** 692 (89.4% del total)
  - Avg Score: 0.62 | Avg R:R: 5.40 | Avg DistATR: 3.62
  - Por TF: TF15=153, TF60=539


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 17 | Unmatched: 323
- 0-10: Wins=2 Losses=15 WR=11.8% (n=17)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=2 Losses=15 WR=11.8% (n=17)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 340 | Aligned=155 (45.6%)
- Core≈ 1.00 | Prox≈ 0.61 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.48 | Confidence≈ 0.00
- SL_TF dist: {'15': 228, '60': 36, '5': 52, '240': 24} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 113, '5': 92, '240': 92, '60': 43} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=333, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=50, 15m=227, 60m=36, 240m=20, 1440m=0
- RR plan por bandas: 0-10≈ 2.38 (n=333), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 3217 | Zonas con Anchors: 3217
- Dir zonas (zona): Bull=374 Bear=2727 Neutral=116
- Resumen por ciclo (promedios): TotHZ≈ 5.1, WithAnchors≈ 5.1, DirBull≈ 0.6, DirBear≈ 4.3, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 3084, 'tie-bias': 133}
- TF Triggers: {'5': 1558, '15': 1659}
- TF Anchors: {'60': 3189, '240': 3217, '1440': 3217}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 3, 'score decayó a 0,22': 1, 'score decayó a 0,24': 1}

## CSV de Trades
- Filas: 45 | Ejecutadas: 12 | Canceladas: 0 | Expiradas: 0
- BUY: 7 | SELL: 50

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 205
- Registered: 24
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 24

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 11.7%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 50.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 1460 | Total candidatos: 12994 | Seleccionados: 0
- Candidatos por zona (promedio): 8.9

### Take Profit (TP)
- Zonas analizadas: 1446 | Total candidatos: 17778 | Seleccionados: 1446
- Candidatos por zona (promedio): 12.3
- **Edad (barras)** - Candidatos: med=37, max=157 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 17778}
- **Priority Seleccionados**: {'P3': 1091, 'NA': 301, 'P0': 54}
- **Type Candidatos**: {'Swing': 17778}
- **Type Seleccionados**: {'P3_Swing': 1091, 'P4_Fallback': 301, 'P0_Zone': 54}
- **TF Candidatos**: {240: 6422, 15: 4112, 5: 4096, 60: 3148}
- **TF Seleccionados**: {15: 274, -1: 301, 5: 284, 240: 439, 60: 148}
- **DistATR** - Candidatos: avg=12.1 | Seleccionados: avg=6.1
- **RR** - Candidatos: avg=5.80 | Seleccionados: avg=1.46
- **Razones de selección**: {'BestIntelligentScore': 1446}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.