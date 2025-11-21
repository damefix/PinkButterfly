# Informe Diagnóstico de Logs - 2025-11-11 08:32:51

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_082926.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_082926.csv`

## DFM
- Eventos de evaluación: 190
- Evaluaciones Bull: 44 | Bear: 178
- Pasaron umbral (PassedThreshold): 207
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:6, 5:20, 6:16, 7:64, 8:116, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2184
- KeptAligned: 1851/1851 | KeptCounter: 1054/1054
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.380 | AvgProxCounter≈ 0.244
  - AvgDistATRAligned≈ 0.53 | AvgDistATRCounter≈ 0.37
- PreferAligned eventos: 925 | Filtradas contra-bias: 317

### Proximity (Pre-PreferAligned)
- Eventos: 2184
- Aligned pre: 1851/2905 | Counter pre: 1054/2905
- AvgProxAligned(pre)≈ 0.380 | AvgDistATRAligned(pre)≈ 0.53

### Proximity Drivers
- Eventos: 2184
- Alineadas: n=1851 | BaseProx≈ 0.918 | ZoneATR≈ 5.10 | SizePenalty≈ 0.977 | FinalProx≈ 0.897
- Contra-bias: n=737 | BaseProx≈ 0.833 | ZoneATR≈ 4.92 | SizePenalty≈ 0.977 | FinalProx≈ 0.814

## Risk
- Eventos: 1356
- Accepted=223 | RejSL=0 | RejTP=0 | RejRR=160 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 31 (18.0% del total)
  - Avg Score: 0.67 | Avg R:R: 2.00 | Avg DistATR: 7.61
  - Por TF: TF5=13, TF15=18
- **P0_SWING_LITE:** 141 (82.0% del total)
  - Avg Score: 0.33 | Avg R:R: 9.07 | Avg DistATR: 9.66
  - Por TF: TF15=77, TF60=64


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 68 | Unmatched: 155
- 0-10: Wins=27 Losses=41 WR=39.7% (n=68)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=27 Losses=41 WR=39.7% (n=68)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 223 | Aligned=146 (65.5%)
- Core≈ 1.00 | Prox≈ 0.89 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.30 | Confidence≈ 0.00
- SL_TF dist: {'60': 2, '15': 177, '5': 44} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 42, '15': 79, '60': 86, '240': 16} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=223, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=44, 15m=177, 60m=2, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.30 (n=223), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 42126 | Zonas con Anchors: 42114
- Dir zonas (zona): Bull=7837 Bear=33131 Neutral=1158
- Resumen por ciclo (promedios): TotHZ≈ 3.5, WithAnchors≈ 3.5, DirBull≈ 1.3, DirBear≈ 2.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 40956, 'tie-bias': 1158, 'triggers-only': 12}
- TF Triggers: {'5': 4975, '15': 3880}
- TF Anchors: {'60': 8807, '240': 5390, '1440': 223}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,21': 1, 'score decayó a 0,38': 1, 'score decayó a 0,30': 1, 'score decayó a 0,48': 1, 'score decayó a 0,49': 1, 'estructura no existe': 7, 'score decayó a 0,29': 1, 'score decayó a 0,18': 1, 'score decayó a 0,43': 1}
- Cancel_BOS (diag): por acción {'BUY': 3, 'SELL': 5} | por bias {'Bullish': 5, 'Bearish': 3, 'Neutral': 0}

## CSV de Trades
- Filas: 150 | Ejecutadas: 44 | Canceladas: 0 | Expiradas: 0
- BUY: 63 | SELL: 131

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 207
- Registered: 83
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 26 | SKIP_CONCURRENCY: 63
- Intentos de registro: 172

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 83.1%
- RegRate = Registered / Intentos = 48.3%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 15.1%
- Concurrency = SKIP_CONCURRENCY / Intentos = 36.6%
- ExecRate = Ejecutadas / Registered = 53.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 567 | Total candidatos: 10272 | Seleccionados: 0
- Candidatos por zona (promedio): 18.1

### Take Profit (TP)
- Zonas analizadas: 567 | Total candidatos: 4599 | Seleccionados: 0
- Candidatos por zona (promedio): 8.1
- **Edad (barras)** - Candidatos: med=53, max=250 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 4599}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 4599}
- **Type Seleccionados**: {}
- **TF Candidatos**: {60: 1416, 5: 1318, 15: 1236, 240: 629}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=9.1 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=4.02 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.