# Informe Diagnóstico de Logs - 2025-11-13 07:54:45

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_074936.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_074936.csv`

## DFM
- Eventos de evaluación: 952
- Evaluaciones Bull: 171 | Bear: 689
- Pasaron umbral (PassedThreshold): 860
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:77, 6:375, 7:358, 8:50, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2355
- KeptAligned: 4156/4156 | KeptCounter: 2721/2827
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.446 | AvgProxCounter≈ 0.230
  - AvgDistATRAligned≈ 1.51 | AvgDistATRCounter≈ 1.13
- PreferAligned eventos: 1280 | Filtradas contra-bias: 576

### Proximity (Pre-PreferAligned)
- Eventos: 2355
- Aligned pre: 4156/6877 | Counter pre: 2721/6877
- AvgProxAligned(pre)≈ 0.446 | AvgDistATRAligned(pre)≈ 1.51

### Proximity Drivers
- Eventos: 2355
- Alineadas: n=4156 | BaseProx≈ 0.753 | ZoneATR≈ 5.17 | SizePenalty≈ 0.975 | FinalProx≈ 0.735
- Contra-bias: n=2145 | BaseProx≈ 0.527 | ZoneATR≈ 4.85 | SizePenalty≈ 0.978 | FinalProx≈ 0.518

## Risk
- Eventos: 1938
- Accepted=1279 | RejSL=0 | RejTP=0 | RejRR=1241 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 314 (10.9% del total)
  - Avg Score: 0.39 | Avg R:R: 1.88 | Avg DistATR: 3.72
  - Por TF: TF5=80, TF15=234
- **P0_SWING_LITE:** 2563 (89.1% del total)
  - Avg Score: 0.57 | Avg R:R: 4.20 | Avg DistATR: 3.48
  - Por TF: TF15=545, TF60=2018


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 86 | Unmatched: 1232
- 0-10: Wins=36 Losses=50 WR=41.9% (n=86)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=36 Losses=50 WR=41.9% (n=86)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1318 | Aligned=815 (61.8%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.09 | Confidence≈ 0.00
- SL_TF dist: {'5': 204, '60': 151, '15': 952, '240': 11} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 303, '5': 352, '15': 489, '240': 174} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1279, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=196, 15m=929, 60m=147, 240m=7, 1440m=0
- RR plan por bandas: 0-10≈ 2.07 (n=1279), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10297 | Zonas con Anchors: 10283
- Dir zonas (zona): Bull=3772 Bear=6184 Neutral=341
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9874, 'tie-bias': 409, 'triggers-only': 14}
- TF Triggers: {'5': 5444, '15': 4853}
- TF Anchors: {'60': 10209, '240': 5948, '1440': 442}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,46': 1, 'estructura no existe': 26, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,42': 1, 'score decayó a 0,34': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 202 | Ejecutadas: 36 | Canceladas: 0 | Expiradas: 0
- BUY: 76 | SELL: 162

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 860
- Registered: 106
  - DEDUP_COOLDOWN: 23 | DEDUP_IDENTICAL: 108 | SKIP_CONCURRENCY: 100
- Intentos de registro: 337

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 39.2%
- RegRate = Registered / Intentos = 31.5%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 38.9%
- Concurrency = SKIP_CONCURRENCY / Intentos = 29.7%
- ExecRate = Ejecutadas / Registered = 34.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5439 | Total candidatos: 43095 | Seleccionados: 0
- Candidatos por zona (promedio): 7.9

### Take Profit (TP)
- Zonas analizadas: 5333 | Total candidatos: 51056 | Seleccionados: 5333
- Candidatos por zona (promedio): 9.6
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 51056}
- **Priority Seleccionados**: {'P3': 3639, 'NA': 1375, 'P0': 319}
- **Type Candidatos**: {'Swing': 51056}
- **Type Seleccionados**: {'P3_Swing': 3639, 'P4_Fallback': 1375, 'P0_Zone': 319}
- **TF Candidatos**: {5: 15393, 15: 13866, 60: 13637, 240: 8160}
- **TF Seleccionados**: {60: 1008, 5: 1024, -1: 1375, 15: 1221, 240: 705}
- **DistATR** - Candidatos: avg=8.6 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.58 | Seleccionados: avg=1.33
- **Razones de selección**: {'BestIntelligentScore': 5333}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.