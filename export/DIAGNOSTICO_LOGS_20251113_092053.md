# Informe Diagnóstico de Logs - 2025-11-13 09:25:02

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_092053.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_092053.csv`

## DFM
- Eventos de evaluación: 936
- Evaluaciones Bull: 162 | Bear: 683
- Pasaron umbral (PassedThreshold): 845
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:70, 6:370, 7:356, 8:49, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2355
- KeptAligned: 4152/4152 | KeptCounter: 2735/2841
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.445 | AvgProxCounter≈ 0.231
  - AvgDistATRAligned≈ 1.52 | AvgDistATRCounter≈ 1.13
- PreferAligned eventos: 1281 | Filtradas contra-bias: 567

### Proximity (Pre-PreferAligned)
- Eventos: 2355
- Aligned pre: 4152/6887 | Counter pre: 2735/6887
- AvgProxAligned(pre)≈ 0.445 | AvgDistATRAligned(pre)≈ 1.52

### Proximity Drivers
- Eventos: 2355
- Alineadas: n=4152 | BaseProx≈ 0.752 | ZoneATR≈ 5.17 | SizePenalty≈ 0.975 | FinalProx≈ 0.734
- Contra-bias: n=2168 | BaseProx≈ 0.530 | ZoneATR≈ 4.85 | SizePenalty≈ 0.978 | FinalProx≈ 0.520

## Risk
- Eventos: 1947
- Accepted=1262 | RejSL=0 | RejTP=0 | RejRR=1267 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 312 (10.9% del total)
  - Avg Score: 0.39 | Avg R:R: 1.89 | Avg DistATR: 3.74
  - Por TF: TF5=80, TF15=232
- **P0_SWING_LITE:** 2562 (89.1% del total)
  - Avg Score: 0.57 | Avg R:R: 4.18 | Avg DistATR: 3.48
  - Por TF: TF15=535, TF60=2027


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 85 | Unmatched: 1217
- 0-10: Wins=37 Losses=48 WR=43.5% (n=85)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=37 Losses=48 WR=43.5% (n=85)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1302 | Aligned=807 (62.0%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.08 | Confidence≈ 0.00
- SL_TF dist: {'60': 154, '15': 946, '5': 191, '240': 11} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 291, '15': 489, '5': 350, '240': 172} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1262, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=183, 15m=922, 60m=150, 240m=7, 1440m=0
- RR plan por bandas: 0-10≈ 2.06 (n=1262), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10329 | Zonas con Anchors: 10315
- Dir zonas (zona): Bull=3766 Bear=6227 Neutral=336
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9915, 'tie-bias': 400, 'triggers-only': 14}
- TF Triggers: {'5': 5460, '15': 4869}
- TF Anchors: {'60': 10242, '240': 5980, '1440': 451}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,46': 1, 'estructura no existe': 24, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,42': 1, 'score decayó a 0,34': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 190 | Ejecutadas: 35 | Canceladas: 0 | Expiradas: 0
- BUY: 67 | SELL: 158

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 845
- Registered: 100
  - DEDUP_COOLDOWN: 21 | DEDUP_IDENTICAL: 102 | SKIP_CONCURRENCY: 101
- Intentos de registro: 324

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 38.3%
- RegRate = Registered / Intentos = 30.9%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 38.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 31.2%
- ExecRate = Ejecutadas / Registered = 35.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5455 | Total candidatos: 43293 | Seleccionados: 0
- Candidatos por zona (promedio): 7.9

### Take Profit (TP)
- Zonas analizadas: 5349 | Total candidatos: 51294 | Seleccionados: 5349
- Candidatos por zona (promedio): 9.6
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 51294}
- **Priority Seleccionados**: {'P3': 3661, 'NA': 1372, 'P0': 316}
- **Type Candidatos**: {'Swing': 51294}
- **Type Seleccionados**: {'P3_Swing': 3661, 'P4_Fallback': 1372, 'P0_Zone': 316}
- **TF Candidatos**: {5: 15409, 15: 13943, 60: 13732, 240: 8210}
- **TF Seleccionados**: {60: 1005, 5: 1023, -1: 1372, 15: 1246, 240: 703}
- **DistATR** - Candidatos: avg=8.6 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.57 | Seleccionados: avg=1.32
- **Razones de selección**: {'BestIntelligentScore': 5349}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.