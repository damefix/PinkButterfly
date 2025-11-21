# Informe Diagnóstico de Logs - 2025-11-13 11:37:14

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_113305.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_113305.csv`

## DFM
- Eventos de evaluación: 945
- Evaluaciones Bull: 159 | Bear: 679
- Pasaron umbral (PassedThreshold): 838
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:69, 6:362, 7:358, 8:49, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2355
- KeptAligned: 4153/4153 | KeptCounter: 2742/2848
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.444 | AvgProxCounter≈ 0.231
  - AvgDistATRAligned≈ 1.51 | AvgDistATRCounter≈ 1.14
- PreferAligned eventos: 1275 | Filtradas contra-bias: 561

### Proximity (Pre-PreferAligned)
- Eventos: 2355
- Aligned pre: 4153/6895 | Counter pre: 2742/6895
- AvgProxAligned(pre)≈ 0.444 | AvgDistATRAligned(pre)≈ 1.51

### Proximity Drivers
- Eventos: 2355
- Alineadas: n=4153 | BaseProx≈ 0.752 | ZoneATR≈ 5.18 | SizePenalty≈ 0.975 | FinalProx≈ 0.734
- Contra-bias: n=2181 | BaseProx≈ 0.526 | ZoneATR≈ 4.84 | SizePenalty≈ 0.978 | FinalProx≈ 0.517

## Risk
- Eventos: 1946
- Accepted=1268 | RejSL=0 | RejTP=0 | RejRR=1241 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 312 (10.9% del total)
  - Avg Score: 0.39 | Avg R:R: 1.89 | Avg DistATR: 3.77
  - Por TF: TF5=78, TF15=234
- **P0_SWING_LITE:** 2548 (89.1% del total)
  - Avg Score: 0.58 | Avg R:R: 4.21 | Avg DistATR: 3.48
  - Por TF: TF15=540, TF60=2008


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 83 | Unmatched: 1223
- 0-10: Wins=36 Losses=47 WR=43.4% (n=83)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=36 Losses=47 WR=43.4% (n=83)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1306 | Aligned=800 (61.3%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.10 | Confidence≈ 0.00
- SL_TF dist: {'5': 190, '60': 148, '15': 955, '240': 13} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 295, '5': 350, '15': 488, '240': 173} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1268, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=182, 15m=933, 60m=144, 240m=9, 1440m=0
- RR plan por bandas: 0-10≈ 2.08 (n=1268), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10331 | Zonas con Anchors: 10317
- Dir zonas (zona): Bull=3779 Bear=6214 Neutral=338
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9914, 'tie-bias': 403, 'triggers-only': 14}
- TF Triggers: {'5': 5460, '15': 4871}
- TF Anchors: {'60': 10244, '240': 6005, '1440': 478}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,46': 1, 'estructura no existe': 26, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,42': 1, 'score decayó a 0,34': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 194 | Ejecutadas: 34 | Canceladas: 0 | Expiradas: 0
- BUY: 69 | SELL: 159

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 838
- Registered: 102
  - DEDUP_COOLDOWN: 20 | DEDUP_IDENTICAL: 103 | SKIP_CONCURRENCY: 93
- Intentos de registro: 318

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 37.9%
- RegRate = Registered / Intentos = 32.1%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 38.7%
- Concurrency = SKIP_CONCURRENCY / Intentos = 29.2%
- ExecRate = Ejecutadas / Registered = 33.3%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5438 | Total candidatos: 43011 | Seleccionados: 0
- Candidatos por zona (promedio): 7.9

### Take Profit (TP)
- Zonas analizadas: 5328 | Total candidatos: 51185 | Seleccionados: 5328
- Candidatos por zona (promedio): 9.6
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 51185}
- **Priority Seleccionados**: {'P3': 3646, 'NA': 1366, 'P0': 316}
- **Type Candidatos**: {'Swing': 51185}
- **Type Seleccionados**: {'P3_Swing': 3646, 'P4_Fallback': 1366, 'P0_Zone': 316}
- **TF Candidatos**: {5: 15318, 15: 13918, 60: 13613, 240: 8336}
- **TF Seleccionados**: {60: 990, -1: 1366, 5: 1010, 15: 1258, 240: 704}
- **DistATR** - Candidatos: avg=8.7 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.58 | Seleccionados: avg=1.33
- **Razones de selección**: {'BestIntelligentScore': 5328}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.