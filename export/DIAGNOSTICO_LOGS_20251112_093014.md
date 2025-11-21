# Informe Diagnóstico de Logs - 2025-11-12 09:33:25

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_093014.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251112_093014.csv`

## DFM
- Eventos de evaluación: 154
- Evaluaciones Bull: 57 | Bear: 113
- Pasaron umbral (PassedThreshold): 160
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:1, 4:8, 5:21, 6:77, 7:53, 8:10, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2159
- KeptAligned: 1815/1815 | KeptCounter: 1049/1049
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.363 | AvgProxCounter≈ 0.196
  - AvgDistATRAligned≈ 0.54 | AvgDistATRCounter≈ 0.32
- PreferAligned eventos: 822 | Filtradas contra-bias: 134

### Proximity (Pre-PreferAligned)
- Eventos: 2159
- Aligned pre: 1815/2864 | Counter pre: 1049/2864
- AvgProxAligned(pre)≈ 0.363 | AvgDistATRAligned(pre)≈ 0.54

### Proximity Drivers
- Eventos: 2159
- Alineadas: n=1815 | BaseProx≈ 0.869 | ZoneATR≈ 5.07 | SizePenalty≈ 0.979 | FinalProx≈ 0.851
- Contra-bias: n=915 | BaseProx≈ 0.762 | ZoneATR≈ 4.84 | SizePenalty≈ 0.978 | FinalProx≈ 0.745

## Risk
- Eventos: 1328
- Accepted=170 | RejSL=0 | RejTP=0 | RejRR=141 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 31 (8.3% del total)
  - Avg Score: 0.41 | Avg R:R: 2.02 | Avg DistATR: 3.75
  - Por TF: TF5=7, TF15=24
- **P0_SWING_LITE:** 342 (91.7% del total)
  - Avg Score: 0.53 | Avg R:R: 3.70 | Avg DistATR: 3.32
  - Por TF: TF15=69, TF60=273


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 43 | Unmatched: 128
- 0-10: Wins=17 Losses=26 WR=39.5% (n=43)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=17 Losses=26 WR=39.5% (n=43)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 171 | Aligned=103 (60.2%)
- Core≈ 0.99 | Prox≈ 0.80 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.73 | Confidence≈ 0.00
- SL_TF dist: {'60': 9, '5': 40, '15': 122} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 79, '60': 51, '5': 32, '240': 9} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=170, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=39, 15m=122, 60m=9, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.73 (n=170), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 14914 | Zonas con Anchors: 14706
- Dir zonas (zona): Bull=5821 Bear=8560 Neutral=533
- Resumen por ciclo (promedios): TotHZ≈ 3.1, WithAnchors≈ 3.1, DirBull≈ 1.3, DirBear≈ 1.7, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 14121, 'tie-bias': 587, 'triggers-only': 206}
- TF Triggers: {'5': 4627, '15': 3116}
- TF Anchors: {'60': 7581, '240': 4253, '1440': 65}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 2, 'score decayó a 0,33': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 82 | Ejecutadas: 29 | Canceladas: 0 | Expiradas: 0
- BUY: 51 | SELL: 60

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 160
- Registered: 43
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 18 | SKIP_CONCURRENCY: 8
- Intentos de registro: 69

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 43.1%
- RegRate = Registered / Intentos = 62.3%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 26.1%
- Concurrency = SKIP_CONCURRENCY / Intentos = 11.6%
- ExecRate = Ejecutadas / Registered = 67.4%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 516 | Total candidatos: 8641 | Seleccionados: 0
- Candidatos por zona (promedio): 16.7

### Take Profit (TP)
- Zonas analizadas: 505 | Total candidatos: 3792 | Seleccionados: 505
- Candidatos por zona (promedio): 7.5
- **Edad (barras)** - Candidatos: med=52, max=182 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.40 | Seleccionados: avg=0.70
- **Priority Candidatos**: {'P3': 3792}
- **Priority Seleccionados**: {'P0': 47, 'P3': 239, 'NA': 219}
- **Type Candidatos**: {'Swing': 3792}
- **Type Seleccionados**: {'P0_Zone': 47, 'P3_Swing': 239, 'P4_Fallback': 219}
- **TF Candidatos**: {60: 1225, 15: 980, 5: 918, 240: 669}
- **TF Seleccionados**: {15: 122, 60: 75, -1: 219, 5: 63, 240: 26}
- **DistATR** - Candidatos: avg=9.7 | Seleccionados: avg=3.5
- **RR** - Candidatos: avg=5.06 | Seleccionados: avg=1.32
- **Razones de selección**: {'BestIntelligentScore': 505}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.