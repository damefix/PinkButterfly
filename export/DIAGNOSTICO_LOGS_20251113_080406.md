# Informe Diagnóstico de Logs - 2025-11-13 08:08:50

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_080406.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_080406.csv`

## DFM
- Eventos de evaluación: 945
- Evaluaciones Bull: 167 | Bear: 674
- Pasaron umbral (PassedThreshold): 841
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:85, 6:356, 7:351, 8:49, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2355
- KeptAligned: 4130/4130 | KeptCounter: 2732/2847
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.439 | AvgProxCounter≈ 0.228
  - AvgDistATRAligned≈ 1.50 | AvgDistATRCounter≈ 1.13
- PreferAligned eventos: 1275 | Filtradas contra-bias: 568

### Proximity (Pre-PreferAligned)
- Eventos: 2355
- Aligned pre: 4130/6862 | Counter pre: 2732/6862
- AvgProxAligned(pre)≈ 0.439 | AvgDistATRAligned(pre)≈ 1.50

### Proximity Drivers
- Eventos: 2355
- Alineadas: n=4130 | BaseProx≈ 0.748 | ZoneATR≈ 5.16 | SizePenalty≈ 0.975 | FinalProx≈ 0.730
- Contra-bias: n=2164 | BaseProx≈ 0.520 | ZoneATR≈ 4.83 | SizePenalty≈ 0.978 | FinalProx≈ 0.510

## Risk
- Eventos: 1939
- Accepted=1279 | RejSL=0 | RejTP=0 | RejRR=1235 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 307 (10.7% del total)
  - Avg Score: 0.39 | Avg R:R: 1.88 | Avg DistATR: 3.74
  - Por TF: TF5=81, TF15=226
- **P0_SWING_LITE:** 2563 (89.3% del total)
  - Avg Score: 0.57 | Avg R:R: 4.23 | Avg DistATR: 3.48
  - Por TF: TF15=551, TF60=2012


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 96 | Unmatched: 1224
- 0-10: Wins=41 Losses=55 WR=42.7% (n=96)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=41 Losses=55 WR=42.7% (n=96)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1320 | Aligned=814 (61.7%)
- Core≈ 1.00 | Prox≈ 0.66 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.10 | Confidence≈ 0.00
- SL_TF dist: {'5': 203, '60': 158, '15': 948, '240': 11} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 297, '5': 350, '15': 499, '240': 174} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1279, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=196, 15m=921, 60m=155, 240m=7, 1440m=0
- RR plan por bandas: 0-10≈ 2.08 (n=1279), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10304 | Zonas con Anchors: 10290
- Dir zonas (zona): Bull=3761 Bear=6191 Neutral=352
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9877, 'tie-bias': 413, 'triggers-only': 14}
- TF Triggers: {'5': 5459, '15': 4845}
- TF Anchors: {'60': 10217, '240': 5958, '1440': 441}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,46': 1, 'estructura no existe': 25, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,43': 1, 'score decayó a 0,42': 1, 'score decayó a 0,32': 1, 'score decayó a 0,33': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 216 | Ejecutadas: 43 | Canceladas: 0 | Expiradas: 0
- BUY: 73 | SELL: 186

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 841
- Registered: 113
  - DEDUP_COOLDOWN: 23 | DEDUP_IDENTICAL: 117 | SKIP_CONCURRENCY: 77
- Intentos de registro: 330

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 39.2%
- RegRate = Registered / Intentos = 34.2%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 42.4%
- Concurrency = SKIP_CONCURRENCY / Intentos = 23.3%
- ExecRate = Ejecutadas / Registered = 38.1%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5420 | Total candidatos: 43005 | Seleccionados: 0
- Candidatos por zona (promedio): 7.9

### Take Profit (TP)
- Zonas analizadas: 5311 | Total candidatos: 50886 | Seleccionados: 5311
- Candidatos por zona (promedio): 9.6
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.50 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 50886}
- **Priority Seleccionados**: {'P3': 3634, 'NA': 1361, 'P0': 316}
- **Type Candidatos**: {'Swing': 50886}
- **Type Seleccionados**: {'P3_Swing': 3634, 'P4_Fallback': 1361, 'P0_Zone': 316}
- **TF Candidatos**: {5: 15283, 15: 13890, 60: 13604, 240: 8109}
- **TF Seleccionados**: {60: 1003, 5: 1001, -1: 1361, 15: 1242, 240: 704}
- **DistATR** - Candidatos: avg=8.6 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.62 | Seleccionados: avg=1.33
- **Razones de selección**: {'BestIntelligentScore': 5311}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.