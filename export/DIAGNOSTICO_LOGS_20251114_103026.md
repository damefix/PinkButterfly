# Informe Diagnóstico de Logs - 2025-11-14 10:34:48

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_103026.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251114_103026.csv`

## DFM
- Eventos de evaluación: 949
- Evaluaciones Bull: 140 | Bear: 702
- Pasaron umbral (PassedThreshold): 842
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:58, 6:377, 7:327, 8:80, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2369
- KeptAligned: 3658/3658 | KeptCounter: 2828/2962
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.407 | AvgProxCounter≈ 0.228
  - AvgDistATRAligned≈ 1.47 | AvgDistATRCounter≈ 1.25
- PreferAligned eventos: 1188 | Filtradas contra-bias: 517

### Proximity (Pre-PreferAligned)
- Eventos: 2369
- Aligned pre: 3658/6486 | Counter pre: 2828/6486
- AvgProxAligned(pre)≈ 0.407 | AvgDistATRAligned(pre)≈ 1.47

### Proximity Drivers
- Eventos: 2369
- Alineadas: n=3658 | BaseProx≈ 0.756 | ZoneATR≈ 4.98 | SizePenalty≈ 0.977 | FinalProx≈ 0.740
- Contra-bias: n=2311 | BaseProx≈ 0.485 | ZoneATR≈ 4.89 | SizePenalty≈ 0.977 | FinalProx≈ 0.476

## Risk
- Eventos: 1948
- Accepted=1297 | RejSL=0 | RejTP=0 | RejRR=1282 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 304 (10.7% del total)
  - Avg Score: 0.40 | Avg R:R: 1.93 | Avg DistATR: 3.80
  - Por TF: TF5=102, TF15=202
- **P0_SWING_LITE:** 2540 (89.3% del total)
  - Avg Score: 0.60 | Avg R:R: 4.43 | Avg DistATR: 3.48
  - Por TF: TF15=533, TF60=2007


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 70 | Unmatched: 1265
- 0-10: Wins=26 Losses=44 WR=37.1% (n=70)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=26 Losses=44 WR=37.1% (n=70)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1335 | Aligned=795 (59.6%)
- Core≈ 1.00 | Prox≈ 0.65 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.11 | Confidence≈ 0.00
- SL_TF dist: {'60': 205, '5': 200, '15': 901, '240': 29} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 249, '5': 320, '15': 471, '240': 295} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1297, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=195, 15m=877, 60m=201, 240m=24, 1440m=0
- RR plan por bandas: 0-10≈ 2.09 (n=1297), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10420 | Zonas con Anchors: 10412
- Dir zonas (zona): Bull=3643 Bear=6396 Neutral=381
- Resumen por ciclo (promedios): TotHZ≈ 4.2, WithAnchors≈ 4.2, DirBull≈ 1.5, DirBear≈ 2.6, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 9934, 'tie-bias': 478, 'triggers-only': 8}
- TF Triggers: {'5': 5502, '15': 4918}
- TF Anchors: {'60': 10318, '240': 9669, '1440': 8118}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 24, 'score decayó a 0,21': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,27': 1, 'score decayó a 0,32': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 189 | Ejecutadas: 42 | Canceladas: 0 | Expiradas: 0
- BUY: 59 | SELL: 172

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 842
- Registered: 99
  - DEDUP_COOLDOWN: 15 | DEDUP_IDENTICAL: 100 | SKIP_CONCURRENCY: 93
- Intentos de registro: 307

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 36.5%
- RegRate = Registered / Intentos = 32.2%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 37.5%
- Concurrency = SKIP_CONCURRENCY / Intentos = 30.3%
- ExecRate = Ejecutadas / Registered = 42.4%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5060 | Total candidatos: 45519 | Seleccionados: 0
- Candidatos por zona (promedio): 9.0

### Take Profit (TP)
- Zonas analizadas: 4978 | Total candidatos: 90097 | Seleccionados: 4978
- Candidatos por zona (promedio): 18.1
- **Edad (barras)** - Candidatos: med=35, max=182 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.69
- **Priority Candidatos**: {'P3': 90097}
- **Priority Seleccionados**: {'P3': 3513, 'NA': 1232, 'P0': 233}
- **Type Candidatos**: {'Swing': 90097}
- **Type Seleccionados**: {'P3_Swing': 3513, 'P4_Fallback': 1232, 'P0_Zone': 233}
- **TF Candidatos**: {240: 33435, 60: 23263, 5: 19092, 15: 14307}
- **TF Seleccionados**: {60: 644, -1: 1232, 5: 950, 15: 1156, 240: 996}
- **DistATR** - Candidatos: avg=12.3 | Seleccionados: avg=5.2
- **RR** - Candidatos: avg=5.24 | Seleccionados: avg=1.35
- **Razones de selección**: {'BestIntelligentScore': 4978}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.