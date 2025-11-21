# Informe Diagnóstico de Logs - 2025-11-14 16:44:35

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_163459.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251114_163459.csv`

## DFM
- Eventos de evaluación: 887
- Evaluaciones Bull: 100 | Bear: 661
- Pasaron umbral (PassedThreshold): 761
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:44, 6:315, 7:309, 8:93, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2356
- KeptAligned: 3288/3288 | KeptCounter: 3240/3383
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.351 | AvgProxCounter≈ 0.254
  - AvgDistATRAligned≈ 1.27 | AvgDistATRCounter≈ 1.34
- PreferAligned eventos: 1044 | Filtradas contra-bias: 503

### Proximity (Pre-PreferAligned)
- Eventos: 2356
- Aligned pre: 3288/6528 | Counter pre: 3240/6528
- AvgProxAligned(pre)≈ 0.351 | AvgDistATRAligned(pre)≈ 1.27

### Proximity Drivers
- Eventos: 2356
- Alineadas: n=3288 | BaseProx≈ 0.757 | ZoneATR≈ 5.01 | SizePenalty≈ 0.977 | FinalProx≈ 0.741
- Contra-bias: n=2737 | BaseProx≈ 0.501 | ZoneATR≈ 4.96 | SizePenalty≈ 0.977 | FinalProx≈ 0.491

## Risk
- Eventos: 1944
- Accepted=1201 | RejSL=0 | RejTP=0 | RejRR=1294 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 320 (11.1% del total)
  - Avg Score: 0.39 | Avg R:R: 1.94 | Avg DistATR: 3.81
  - Por TF: TF5=97, TF15=223
- **P0_SWING_LITE:** 2554 (88.9% del total)
  - Avg Score: 0.55 | Avg R:R: 4.79 | Avg DistATR: 3.66
  - Por TF: TF15=487, TF60=2067


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 70 | Unmatched: 1171
- 0-10: Wins=28 Losses=42 WR=40.0% (n=70)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=28 Losses=42 WR=40.0% (n=70)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1241 | Aligned=662 (53.3%)
- Core≈ 1.00 | Prox≈ 0.64 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.04 | Confidence≈ 0.00
- SL_TF dist: {'15': 883, '60': 162, '5': 163, '240': 33} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 232, '5': 295, '15': 422, '240': 292} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1201, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=157, 15m=858, 60m=159, 240m=27, 1440m=0
- RR plan por bandas: 0-10≈ 2.02 (n=1201), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10470 | Zonas con Anchors: 10461
- Dir zonas (zona): Bull=2808 Bear=7322 Neutral=340
- Resumen por ciclo (promedios): TotHZ≈ 4.2, WithAnchors≈ 4.2, DirBull≈ 1.1, DirBear≈ 2.9, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 10031, 'tie-bias': 430, 'triggers-only': 9}
- TF Triggers: {'15': 4944, '5': 5526}
- TF Anchors: {'60': 10388, '240': 9808, '1440': 8567}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 15, 'score decayó a 0,21': 2, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,22': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 168 | Ejecutadas: 41 | Canceladas: 0 | Expiradas: 0
- BUY: 43 | SELL: 166

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 761
- Registered: 89
  - DEDUP_COOLDOWN: 13 | DEDUP_IDENTICAL: 64 | SKIP_CONCURRENCY: 100
- Intentos de registro: 266

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 35.0%
- RegRate = Registered / Intentos = 33.5%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 28.9%
- Concurrency = SKIP_CONCURRENCY / Intentos = 37.6%
- ExecRate = Ejecutadas / Registered = 46.1%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5036 | Total candidatos: 41055 | Seleccionados: 0
- Candidatos por zona (promedio): 8.2

### Take Profit (TP)
- Zonas analizadas: 4969 | Total candidatos: 77776 | Seleccionados: 4969
- Candidatos por zona (promedio): 15.7
- **Edad (barras)** - Candidatos: med=36, max=192 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.50 | Seleccionados: avg=0.69
- **Priority Candidatos**: {'P3': 77776}
- **Priority Seleccionados**: {'P3': 3455, 'NA': 1309, 'P0': 205}
- **Type Candidatos**: {'Swing': 77776}
- **Type Seleccionados**: {'P3_Swing': 3455, 'P4_Fallback': 1309, 'P0_Zone': 205}
- **TF Candidatos**: {240: 34821, 60: 15020, 5: 14133, 15: 13802}
- **TF Seleccionados**: {60: 656, -1: 1309, 5: 882, 15: 1051, 240: 1071}
- **DistATR** - Candidatos: avg=14.6 | Seleccionados: avg=5.2
- **RR** - Candidatos: avg=6.40 | Seleccionados: avg=1.38
- **Razones de selección**: {'BestIntelligentScore': 4969}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.