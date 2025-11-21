# Informe Diagnóstico de Logs - 2025-11-13 08:01:32

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_075807.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_075807.csv`

## DFM
- Eventos de evaluación: 943
- Evaluaciones Bull: 165 | Bear: 673
- Pasaron umbral (PassedThreshold): 838
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:80, 6:353, 7:355, 8:50, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2356
- KeptAligned: 4139/4139 | KeptCounter: 2734/2849
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.439 | AvgProxCounter≈ 0.227
  - AvgDistATRAligned≈ 1.51 | AvgDistATRCounter≈ 1.13
- PreferAligned eventos: 1277 | Filtradas contra-bias: 566

### Proximity (Pre-PreferAligned)
- Eventos: 2356
- Aligned pre: 4139/6873 | Counter pre: 2734/6873
- AvgProxAligned(pre)≈ 0.439 | AvgDistATRAligned(pre)≈ 1.51

### Proximity Drivers
- Eventos: 2356
- Alineadas: n=4139 | BaseProx≈ 0.747 | ZoneATR≈ 5.17 | SizePenalty≈ 0.975 | FinalProx≈ 0.730
- Contra-bias: n=2168 | BaseProx≈ 0.518 | ZoneATR≈ 4.83 | SizePenalty≈ 0.978 | FinalProx≈ 0.509

## Risk
- Eventos: 1937
- Accepted=1278 | RejSL=0 | RejTP=0 | RejRR=1253 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 313 (10.9% del total)
  - Avg Score: 0.39 | Avg R:R: 1.88 | Avg DistATR: 3.73
  - Por TF: TF5=81, TF15=232
- **P0_SWING_LITE:** 2567 (89.1% del total)
  - Avg Score: 0.57 | Avg R:R: 4.18 | Avg DistATR: 3.48
  - Por TF: TF15=555, TF60=2012


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 102 | Unmatched: 1216
- 0-10: Wins=41 Losses=61 WR=40.2% (n=102)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=41 Losses=61 WR=40.2% (n=102)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1318 | Aligned=812 (61.6%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.07 | Confidence≈ 0.00
- SL_TF dist: {'5': 202, '60': 161, '15': 944, '240': 11} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 303, '5': 347, '15': 496, '240': 172} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1278, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=195, 15m=918, 60m=158, 240m=7, 1440m=0
- RR plan por bandas: 0-10≈ 2.05 (n=1278), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10296 | Zonas con Anchors: 10282
- Dir zonas (zona): Bull=3768 Bear=6184 Neutral=344
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9876, 'tie-bias': 406, 'triggers-only': 14}
- TF Triggers: {'5': 5460, '15': 4836}
- TF Anchors: {'60': 10208, '240': 5949, '1440': 443}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,46': 1, 'estructura no existe': 24, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,43': 1, 'score decayó a 0,42': 1, 'score decayó a 0,33': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 214 | Ejecutadas: 43 | Canceladas: 0 | Expiradas: 0
- BUY: 73 | SELL: 184

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 838
- Registered: 112
  - DEDUP_COOLDOWN: 25 | DEDUP_IDENTICAL: 116 | SKIP_CONCURRENCY: 76
- Intentos de registro: 329

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 39.3%
- RegRate = Registered / Intentos = 34.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 42.9%
- Concurrency = SKIP_CONCURRENCY / Intentos = 23.1%
- ExecRate = Ejecutadas / Registered = 38.4%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5427 | Total candidatos: 43044 | Seleccionados: 0
- Candidatos por zona (promedio): 7.9

### Take Profit (TP)
- Zonas analizadas: 5318 | Total candidatos: 51053 | Seleccionados: 5318
- Candidatos por zona (promedio): 9.6
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.50 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 51053}
- **Priority Seleccionados**: {'P3': 3631, 'NA': 1371, 'P0': 316}
- **Type Candidatos**: {'Swing': 51053}
- **Type Seleccionados**: {'P3_Swing': 3631, 'P4_Fallback': 1371, 'P0_Zone': 316}
- **TF Candidatos**: {5: 15362, 15: 13913, 60: 13669, 240: 8109}
- **TF Seleccionados**: {60: 1007, 5: 1008, -1: 1371, 15: 1226, 240: 706}
- **DistATR** - Candidatos: avg=8.6 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.58 | Seleccionados: avg=1.32
- **Razones de selección**: {'BestIntelligentScore': 5318}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.