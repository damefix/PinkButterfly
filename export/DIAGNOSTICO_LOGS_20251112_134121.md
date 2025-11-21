# Informe Diagnóstico de Logs - 2025-11-12 13:43:45

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_134121.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251112_134121.csv`

## DFM
- Eventos de evaluación: 238
- Evaluaciones Bull: 96 | Bear: 177
- Pasaron umbral (PassedThreshold): 258
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:4, 4:11, 5:32, 6:132, 7:82, 8:12, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2346
- KeptAligned: 2149/2149 | KeptCounter: 1428/1428
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.381 | AvgProxCounter≈ 0.231
  - AvgDistATRAligned≈ 0.57 | AvgDistATRCounter≈ 0.39
- PreferAligned eventos: 943 | Filtradas contra-bias: 233

### Proximity (Pre-PreferAligned)
- Eventos: 2346
- Aligned pre: 2149/3577 | Counter pre: 1428/3577
- AvgProxAligned(pre)≈ 0.381 | AvgDistATRAligned(pre)≈ 0.57

### Proximity Drivers
- Eventos: 2346
- Alineadas: n=2149 | BaseProx≈ 0.870 | ZoneATR≈ 4.80 | SizePenalty≈ 0.980 | FinalProx≈ 0.853
- Contra-bias: n=1195 | BaseProx≈ 0.751 | ZoneATR≈ 4.68 | SizePenalty≈ 0.981 | FinalProx≈ 0.737

## Risk
- Eventos: 1552
- Accepted=273 | RejSL=0 | RejTP=0 | RejRR=191 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 38 (7.6% del total)
  - Avg Score: 0.40 | Avg R:R: 1.77 | Avg DistATR: 3.40
  - Por TF: TF5=10, TF15=28
- **P0_SWING_LITE:** 464 (92.4% del total)
  - Avg Score: 0.54 | Avg R:R: 4.52 | Avg DistATR: 3.34
  - Por TF: TF15=123, TF60=341


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 57 | Unmatched: 216
- 0-10: Wins=20 Losses=37 WR=35.1% (n=57)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=20 Losses=37 WR=35.1% (n=57)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 273 | Aligned=137 (50.2%)
- Core≈ 1.00 | Prox≈ 0.80 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.87 | Confidence≈ 0.00
- SL_TF dist: {'60': 21, '5': 58, '15': 194} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 150, '60': 51, '5': 61, '240': 11} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=273, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=58, 15m=194, 60m=21, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.87 (n=273), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10196 | Zonas con Anchors: 10182
- Dir zonas (zona): Bull=3887 Bear=5945 Neutral=364
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.6, DirBear≈ 2.4, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9752, 'tie-bias': 430, 'triggers-only': 14}
- TF Triggers: {'5': 5461, '15': 4735}
- TF Anchors: {'60': 10108, '240': 5840, '1440': 691}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 7}
- Cancel_BOS (diag): por acción {'BUY': 1, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 1, 'Neutral': 0}

## CSV de Trades
- Filas: 123 | Ejecutadas: 35 | Canceladas: 0 | Expiradas: 0
- BUY: 72 | SELL: 86

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 258
- Registered: 66
  - DEDUP_COOLDOWN: 6 | DEDUP_IDENTICAL: 39 | SKIP_CONCURRENCY: 22
- Intentos de registro: 133

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 51.6%
- RegRate = Registered / Intentos = 49.6%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 33.8%
- Concurrency = SKIP_CONCURRENCY / Intentos = 16.5%
- ExecRate = Ejecutadas / Registered = 53.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 720 | Total candidatos: 9006 | Seleccionados: 0
- Candidatos por zona (promedio): 12.5

### Take Profit (TP)
- Zonas analizadas: 712 | Total candidatos: 4821 | Seleccionados: 712
- Candidatos por zona (promedio): 6.8
- **Edad (barras)** - Candidatos: med=44, max=156 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.71
- **Priority Candidatos**: {'P3': 4821}
- **Priority Seleccionados**: {'P0': 98, 'P3': 312, 'NA': 302}
- **Type Candidatos**: {'Swing': 4821}
- **Type Seleccionados**: {'P0_Zone': 98, 'P3_Swing': 312, 'P4_Fallback': 302}
- **TF Candidatos**: {15: 1426, 60: 1387, 5: 1071, 240: 937}
- **TF Seleccionados**: {15: 202, 60: 86, -1: 302, 5: 88, 240: 34}
- **DistATR** - Candidatos: avg=9.1 | Seleccionados: avg=3.7
- **RR** - Candidatos: avg=4.51 | Seleccionados: avg=1.38
- **Razones de selección**: {'BestIntelligentScore': 712}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.