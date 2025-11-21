# Informe Diagnóstico de Logs - 2025-11-12 15:15:07

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_151054.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251112_151054.csv`

## DFM
- Eventos de evaluación: 233
- Evaluaciones Bull: 90 | Bear: 177
- Pasaron umbral (PassedThreshold): 251
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:4, 4:12, 5:32, 6:125, 7:82, 8:12, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2347
- KeptAligned: 2157/2157 | KeptCounter: 1418/1418
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.380 | AvgProxCounter≈ 0.229
  - AvgDistATRAligned≈ 0.57 | AvgDistATRCounter≈ 0.39
- PreferAligned eventos: 943 | Filtradas contra-bias: 232

### Proximity (Pre-PreferAligned)
- Eventos: 2347
- Aligned pre: 2157/3575 | Counter pre: 1418/3575
- AvgProxAligned(pre)≈ 0.380 | AvgDistATRAligned(pre)≈ 0.57

### Proximity Drivers
- Eventos: 2347
- Alineadas: n=2157 | BaseProx≈ 0.870 | ZoneATR≈ 4.79 | SizePenalty≈ 0.980 | FinalProx≈ 0.852
- Contra-bias: n=1186 | BaseProx≈ 0.750 | ZoneATR≈ 4.68 | SizePenalty≈ 0.981 | FinalProx≈ 0.736

## Risk
- Eventos: 1547
- Accepted=267 | RejSL=0 | RejTP=0 | RejRR=188 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 38 (7.6% del total)
  - Avg Score: 0.40 | Avg R:R: 1.77 | Avg DistATR: 3.40
  - Por TF: TF5=10, TF15=28
- **P0_SWING_LITE:** 462 (92.4% del total)
  - Avg Score: 0.54 | Avg R:R: 4.54 | Avg DistATR: 3.34
  - Por TF: TF15=123, TF60=339


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 55 | Unmatched: 212
- 0-10: Wins=19 Losses=36 WR=34.5% (n=55)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=19 Losses=36 WR=34.5% (n=55)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 267 | Aligned=134 (50.2%)
- Core≈ 1.00 | Prox≈ 0.80 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.87 | Confidence≈ 0.00
- SL_TF dist: {'15': 190, '5': 58, '60': 19} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 148, '60': 48, '5': 60, '240': 11} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=267, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=58, 15m=190, 60m=19, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.87 (n=267), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10212 | Zonas con Anchors: 10198
- Dir zonas (zona): Bull=3859 Bear=5992 Neutral=361
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.4, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9771, 'tie-bias': 427, 'triggers-only': 14}
- TF Triggers: {'15': 4750, '5': 5462}
- TF Anchors: {'60': 10124, '240': 5887, '1440': 707}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 6}
- Cancel_BOS (diag): por acción {'BUY': 1, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 1, 'Neutral': 0}

## CSV de Trades
- Filas: 121 | Ejecutadas: 34 | Canceladas: 0 | Expiradas: 0
- BUY: 68 | SELL: 87

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 251
- Registered: 65
  - DEDUP_COOLDOWN: 6 | DEDUP_IDENTICAL: 38 | SKIP_CONCURRENCY: 19
- Intentos de registro: 128

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 51.0%
- RegRate = Registered / Intentos = 50.8%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 34.4%
- Concurrency = SKIP_CONCURRENCY / Intentos = 14.8%
- ExecRate = Ejecutadas / Registered = 52.3%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 711 | Total candidatos: 8894 | Seleccionados: 0
- Candidatos por zona (promedio): 12.5

### Take Profit (TP)
- Zonas analizadas: 705 | Total candidatos: 4823 | Seleccionados: 705
- Candidatos por zona (promedio): 6.8
- **Edad (barras)** - Candidatos: med=44, max=156 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.70
- **Priority Candidatos**: {'P3': 4823}
- **Priority Seleccionados**: {'P0': 97, 'P3': 304, 'NA': 304}
- **Type Candidatos**: {'Swing': 4823}
- **Type Seleccionados**: {'P0_Zone': 97, 'P3_Swing': 304, 'P4_Fallback': 304}
- **TF Candidatos**: {15: 1425, 60: 1383, 5: 1062, 240: 953}
- **TF Seleccionados**: {15: 199, 60: 80, -1: 304, 5: 88, 240: 34}
- **DistATR** - Candidatos: avg=9.2 | Seleccionados: avg=3.7
- **RR** - Candidatos: avg=4.55 | Seleccionados: avg=1.38
- **Razones de selección**: {'BestIntelligentScore': 705}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.