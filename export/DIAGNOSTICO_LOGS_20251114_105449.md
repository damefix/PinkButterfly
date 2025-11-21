# Informe Diagnóstico de Logs - 2025-11-14 11:00:15

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_105449.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251114_105449.csv`

## DFM
- Eventos de evaluación: 928
- Evaluaciones Bull: 120 | Bear: 742
- Pasaron umbral (PassedThreshold): 862
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:54, 6:361, 7:362, 8:85, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2369
- KeptAligned: 3495/3495 | KeptCounter: 3018/3152
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.374 | AvgProxCounter≈ 0.243
  - AvgDistATRAligned≈ 1.38 | AvgDistATRCounter≈ 1.34
- PreferAligned eventos: 1121 | Filtradas contra-bias: 494

### Proximity (Pre-PreferAligned)
- Eventos: 2369
- Aligned pre: 3495/6513 | Counter pre: 3018/6513
- AvgProxAligned(pre)≈ 0.374 | AvgDistATRAligned(pre)≈ 1.38

### Proximity Drivers
- Eventos: 2369
- Alineadas: n=3495 | BaseProx≈ 0.754 | ZoneATR≈ 5.03 | SizePenalty≈ 0.977 | FinalProx≈ 0.738
- Contra-bias: n=2524 | BaseProx≈ 0.496 | ZoneATR≈ 4.95 | SizePenalty≈ 0.976 | FinalProx≈ 0.486

## Risk
- Eventos: 1955
- Accepted=1294 | RejSL=0 | RejTP=0 | RejRR=1303 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 319 (11.2% del total)
  - Avg Score: 0.40 | Avg R:R: 1.92 | Avg DistATR: 3.78
  - Por TF: TF5=107, TF15=212
- **P0_SWING_LITE:** 2521 (88.8% del total)
  - Avg Score: 0.59 | Avg R:R: 4.60 | Avg DistATR: 3.50
  - Por TF: TF15=531, TF60=1990


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 85 | Unmatched: 1244
- 0-10: Wins=32 Losses=53 WR=37.6% (n=85)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=32 Losses=53 WR=37.6% (n=85)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1329 | Aligned=736 (55.4%)
- Core≈ 1.00 | Prox≈ 0.65 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.09 | Confidence≈ 0.00
- SL_TF dist: {'60': 196, '5': 203, '15': 900, '240': 30} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 269, '5': 320, '15': 444, '240': 296} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1294, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=198, 15m=879, 60m=192, 240m=25, 1440m=0
- RR plan por bandas: 0-10≈ 2.07 (n=1294), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10428 | Zonas con Anchors: 10420
- Dir zonas (zona): Bull=3075 Bear=6980 Neutral=373
- Resumen por ciclo (promedios): TotHZ≈ 4.2, WithAnchors≈ 4.2, DirBull≈ 1.2, DirBear≈ 2.8, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9932, 'tie-bias': 488, 'triggers-only': 8}
- TF Triggers: {'15': 4927, '5': 5501}
- TF Anchors: {'60': 10324, '240': 9824, '1440': 8446}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 21, 'score decayó a 0,21': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,27': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 191 | Ejecutadas: 46 | Canceladas: 0 | Expiradas: 0
- BUY: 54 | SELL: 183

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 862
- Registered: 100
  - DEDUP_COOLDOWN: 18 | DEDUP_IDENTICAL: 107 | SKIP_CONCURRENCY: 91
- Intentos de registro: 316

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 36.7%
- RegRate = Registered / Intentos = 31.6%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 39.6%
- Concurrency = SKIP_CONCURRENCY / Intentos = 28.8%
- ExecRate = Ejecutadas / Registered = 46.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5078 | Total candidatos: 46001 | Seleccionados: 0
- Candidatos por zona (promedio): 9.1

### Take Profit (TP)
- Zonas analizadas: 5005 | Total candidatos: 91202 | Seleccionados: 5005
- Candidatos por zona (promedio): 18.2
- **Edad (barras)** - Candidatos: med=36, max=182 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.69
- **Priority Candidatos**: {'P3': 91202}
- **Priority Seleccionados**: {'P3': 3518, 'NA': 1260, 'P0': 227}
- **Type Candidatos**: {'Swing': 91202}
- **Type Seleccionados**: {'P3_Swing': 3518, 'P4_Fallback': 1260, 'P0_Zone': 227}
- **TF Candidatos**: {240: 34391, 60: 24296, 5: 18412, 15: 14103}
- **TF Seleccionados**: {60: 684, -1: 1260, 5: 925, 15: 1100, 240: 1036}
- **DistATR** - Candidatos: avg=13.3 | Seleccionados: avg=5.3
- **RR** - Candidatos: avg=5.83 | Seleccionados: avg=1.38
- **Razones de selección**: {'BestIntelligentScore': 5005}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.