# Informe Diagnóstico de Logs - 2025-11-14 18:16:17

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_181300.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251114_181300.csv`

## DFM
- Eventos de evaluación: 892
- Evaluaciones Bull: 100 | Bear: 674
- Pasaron umbral (PassedThreshold): 774
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:44, 6:317, 7:318, 8:95, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2355
- KeptAligned: 3316/3316 | KeptCounter: 3247/3394
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.354 | AvgProxCounter≈ 0.253
  - AvgDistATRAligned≈ 1.27 | AvgDistATRCounter≈ 1.36
- PreferAligned eventos: 1050 | Filtradas contra-bias: 522

### Proximity (Pre-PreferAligned)
- Eventos: 2355
- Aligned pre: 3316/6563 | Counter pre: 3247/6563
- AvgProxAligned(pre)≈ 0.354 | AvgDistATRAligned(pre)≈ 1.27

### Proximity Drivers
- Eventos: 2355
- Alineadas: n=3316 | BaseProx≈ 0.758 | ZoneATR≈ 4.99 | SizePenalty≈ 0.977 | FinalProx≈ 0.742
- Contra-bias: n=2725 | BaseProx≈ 0.498 | ZoneATR≈ 4.98 | SizePenalty≈ 0.977 | FinalProx≈ 0.488

## Risk
- Eventos: 1947
- Accepted=1218 | RejSL=0 | RejTP=0 | RejRR=1295 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 324 (11.2% del total)
  - Avg Score: 0.39 | Avg R:R: 1.93 | Avg DistATR: 3.79
  - Por TF: TF5=96, TF15=228
- **P0_SWING_LITE:** 2560 (88.8% del total)
  - Avg Score: 0.55 | Avg R:R: 4.77 | Avg DistATR: 3.65
  - Por TF: TF15=490, TF60=2070


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 73 | Unmatched: 1184
- 0-10: Wins=28 Losses=45 WR=38.4% (n=73)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=28 Losses=45 WR=38.4% (n=73)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1257 | Aligned=678 (53.9%)
- Core≈ 1.00 | Prox≈ 0.64 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.05 | Confidence≈ 0.00
- SL_TF dist: {'60': 163, '5': 171, '15': 890, '240': 33} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 236, '5': 302, '15': 424, '240': 295} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1218, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=165, 15m=866, 60m=160, 240m=27, 1440m=0
- RR plan por bandas: 0-10≈ 2.03 (n=1218), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10499 | Zonas con Anchors: 10490
- Dir zonas (zona): Bull=2835 Bear=7312 Neutral=352
- Resumen por ciclo (promedios): TotHZ≈ 4.2, WithAnchors≈ 4.2, DirBull≈ 1.1, DirBear≈ 2.9, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 10049, 'tie-bias': 441, 'triggers-only': 9}
- TF Triggers: {'5': 5536, '15': 4963}
- TF Anchors: {'60': 10413, '240': 9859, '1440': 8618}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 15, 'score decayó a 0,21': 2, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,22': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 168 | Ejecutadas: 42 | Canceladas: 0 | Expiradas: 0
- BUY: 43 | SELL: 167

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 774
- Registered: 88
  - DEDUP_COOLDOWN: 13 | DEDUP_IDENTICAL: 64 | SKIP_CONCURRENCY: 100
- Intentos de registro: 265

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 34.2%
- RegRate = Registered / Intentos = 33.2%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 29.1%
- Concurrency = SKIP_CONCURRENCY / Intentos = 37.7%
- ExecRate = Ejecutadas / Registered = 47.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5056 | Total candidatos: 41310 | Seleccionados: 0
- Candidatos por zona (promedio): 8.2

### Take Profit (TP)
- Zonas analizadas: 4993 | Total candidatos: 77962 | Seleccionados: 4993
- Candidatos por zona (promedio): 15.6
- **Edad (barras)** - Candidatos: med=36, max=192 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.50 | Seleccionados: avg=0.69
- **Priority Candidatos**: {'P3': 77962}
- **Priority Seleccionados**: {'NA': 1314, 'P3': 3467, 'P0': 212}
- **Type Candidatos**: {'Swing': 77962}
- **Type Seleccionados**: {'P4_Fallback': 1314, 'P3_Swing': 3467, 'P0_Zone': 212}
- **TF Candidatos**: {240: 34892, 60: 14926, 5: 14241, 15: 13903}
- **TF Seleccionados**: {-1: 1314, 60: 646, 5: 895, 15: 1059, 240: 1079}
- **DistATR** - Candidatos: avg=14.6 | Seleccionados: avg=5.2
- **RR** - Candidatos: avg=6.39 | Seleccionados: avg=1.38
- **Razones de selección**: {'BestIntelligentScore': 4993}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.