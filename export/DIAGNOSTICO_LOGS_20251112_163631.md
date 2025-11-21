# Informe Diagnóstico de Logs - 2025-11-12 16:40:00

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_163631.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251112_163631.csv`

## DFM
- Eventos de evaluación: 1053
- Evaluaciones Bull: 511 | Bear: 959
- Pasaron umbral (PassedThreshold): 1227
- ConfidenceBins acumulado: 0:0, 1:0, 2:13, 3:18, 4:140, 5:358, 6:526, 7:362, 8:53, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2346
- KeptAligned: 5357/5435 | KeptCounter: 2577/3677
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.421 | AvgProxCounter≈ 0.234
  - AvgDistATRAligned≈ 2.46 | AvgDistATRCounter≈ 1.11
- PreferAligned eventos: 1418 | Filtradas contra-bias: 674

### Proximity (Pre-PreferAligned)
- Eventos: 2346
- Aligned pre: 5357/7934 | Counter pre: 2577/7934
- AvgProxAligned(pre)≈ 0.421 | AvgDistATRAligned(pre)≈ 2.46

### Proximity Drivers
- Eventos: 2346
- Alineadas: n=5357 | BaseProx≈ 0.652 | ZoneATR≈ 5.37 | SizePenalty≈ 0.971 | FinalProx≈ 0.635
- Contra-bias: n=1903 | BaseProx≈ 0.546 | ZoneATR≈ 5.01 | SizePenalty≈ 0.976 | FinalProx≈ 0.536

## Risk
- Eventos: 2030
- Accepted=1470 | RejSL=0 | RejTP=0 | RejRR=1429 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 373 (11.4% del total)
  - Avg Score: 0.38 | Avg R:R: 1.89 | Avg DistATR: 3.76
  - Por TF: TF5=101, TF15=272
- **P0_SWING_LITE:** 2909 (88.6% del total)
  - Avg Score: 0.58 | Avg R:R: 4.14 | Avg DistATR: 3.46
  - Por TF: TF15=610, TF60=2299


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 139 | Unmatched: 1363
- 0-10: Wins=29 Losses=110 WR=20.9% (n=139)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=29 Losses=110 WR=20.9% (n=139)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1502 | Aligned=1003 (66.8%)
- Core≈ 1.00 | Prox≈ 0.63 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.02 | Confidence≈ 0.00
- SL_TF dist: {'60': 193, '15': 1095, '5': 209, '240': 5} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 346, '15': 574, '5': 388, '240': 194} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1470, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=203, 15m=1074, 60m=188, 240m=5, 1440m=0
- RR plan por bandas: 0-10≈ 2.03 (n=1470), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10198 | Zonas con Anchors: 10184
- Dir zonas (zona): Bull=3849 Bear=5983 Neutral=366
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.4, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9755, 'tie-bias': 429, 'triggers-only': 14}
- TF Triggers: {'15': 4739, '5': 5459}
- TF Anchors: {'60': 10111, '240': 5911, '1440': 727}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,17': 1, 'estructura no existe': 41, 'score decayó a 0,27': 1, 'score decayó a 0,46': 1, 'score decayó a 0,42': 2, 'score decayó a 0,29': 1, 'score decayó a 0,18': 1, 'score decayó a 0,44': 1, 'score decayó a 0,32': 1, 'score decayó a 0,34': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 304 | Ejecutadas: 57 | Canceladas: 0 | Expiradas: 0
- BUY: 179 | SELL: 182

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1227
- Registered: 158
  - DEDUP_COOLDOWN: 24 | DEDUP_IDENTICAL: 176 | SKIP_CONCURRENCY: 177
- Intentos de registro: 535

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 43.6%
- RegRate = Registered / Intentos = 29.5%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 37.4%
- Concurrency = SKIP_CONCURRENCY / Intentos = 33.1%
- ExecRate = Ejecutadas / Registered = 36.1%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 6397 | Total candidatos: 49626 | Seleccionados: 0
- Candidatos por zona (promedio): 7.8

### Take Profit (TP)
- Zonas analizadas: 6291 | Total candidatos: 61735 | Seleccionados: 6291
- Candidatos por zona (promedio): 9.8
- **Edad (barras)** - Candidatos: med=41, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 61735}
- **Priority Seleccionados**: {'P3': 4313, 'NA': 1574, 'P0': 404}
- **Type Candidatos**: {'Swing': 61735}
- **Type Seleccionados**: {'P3_Swing': 4313, 'P4_Fallback': 1574, 'P0_Zone': 404}
- **TF Candidatos**: {5: 19081, 15: 16885, 60: 15298, 240: 10471}
- **TF Seleccionados**: {60: 1122, 15: 1515, 5: 1231, -1: 1574, 240: 849}
- **DistATR** - Candidatos: avg=8.6 | Seleccionados: avg=5.8
- **RR** - Candidatos: avg=3.34 | Seleccionados: avg=1.29
- **Razones de selección**: {'BestIntelligentScore': 6291}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 0.99.