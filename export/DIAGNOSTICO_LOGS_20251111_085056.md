# Informe Diagnóstico de Logs - 2025-11-11 08:55:58

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_085056.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_085056.csv`

## DFM
- Eventos de evaluación: 191
- Evaluaciones Bull: 45 | Bear: 179
- Pasaron umbral (PassedThreshold): 209
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:6, 5:20, 6:17, 7:65, 8:116, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2185
- KeptAligned: 1852/1852 | KeptCounter: 1051/1051
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.379 | AvgProxCounter≈ 0.244
  - AvgDistATRAligned≈ 0.53 | AvgDistATRCounter≈ 0.38
- PreferAligned eventos: 923 | Filtradas contra-bias: 317

### Proximity (Pre-PreferAligned)
- Eventos: 2185
- Aligned pre: 1852/2903 | Counter pre: 1051/2903
- AvgProxAligned(pre)≈ 0.379 | AvgDistATRAligned(pre)≈ 0.53

### Proximity Drivers
- Eventos: 2185
- Alineadas: n=1852 | BaseProx≈ 0.918 | ZoneATR≈ 5.10 | SizePenalty≈ 0.977 | FinalProx≈ 0.897
- Contra-bias: n=734 | BaseProx≈ 0.832 | ZoneATR≈ 4.94 | SizePenalty≈ 0.977 | FinalProx≈ 0.813

## Risk
- Eventos: 1356
- Accepted=225 | RejSL=0 | RejTP=0 | RejRR=156 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 32 (18.4% del total)
  - Avg Score: 0.67 | Avg R:R: 2.02 | Avg DistATR: 7.66
  - Por TF: TF5=14, TF15=18
- **P0_SWING_LITE:** 142 (81.6% del total)
  - Avg Score: 0.33 | Avg R:R: 8.98 | Avg DistATR: 9.67
  - Por TF: TF15=77, TF60=65


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 55 | Unmatched: 170
- 0-10: Wins=27 Losses=28 WR=49.1% (n=55)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=27 Losses=28 WR=49.1% (n=55)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 225 | Aligned=147 (65.3%)
- Core≈ 1.00 | Prox≈ 0.88 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.29 | Confidence≈ 0.00
- SL_TF dist: {'60': 2, '15': 180, '5': 43} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 43, '15': 79, '60': 86, '240': 17} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=225, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=43, 15m=180, 60m=2, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.29 (n=225), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 41867 | Zonas con Anchors: 41855
- Dir zonas (zona): Bull=7756 Bear=32982 Neutral=1129
- Resumen por ciclo (promedios): TotHZ≈ 3.5, WithAnchors≈ 3.5, DirBull≈ 1.3, DirBear≈ 2.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 40726, 'tie-bias': 1129, 'triggers-only': 12}
- TF Triggers: {'5': 4979, '15': 3882}
- TF Anchors: {'60': 8813, '240': 5404, '1440': 231}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,21': 1, 'score decayó a 0,43': 2, 'score decayó a 0,48': 2, 'score decayó a 0,38': 1, 'score decayó a 0,30': 1, 'estructura no existe': 5, 'score decayó a 0,29': 1, 'score decayó a 0,18': 1}
- Cancel_BOS (diag): por acción {'BUY': 3, 'SELL': 4} | por bias {'Bullish': 4, 'Bearish': 3, 'Neutral': 0}

## CSV de Trades
- Filas: 123 | Ejecutadas: 33 | Canceladas: 0 | Expiradas: 0
- BUY: 44 | SELL: 112

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 209
- Registered: 69
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 55 | SKIP_CONCURRENCY: 48
- Intentos de registro: 172

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 82.3%
- RegRate = Registered / Intentos = 40.1%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 32.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 27.9%
- ExecRate = Ejecutadas / Registered = 47.8%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 569 | Total candidatos: 10297 | Seleccionados: 0
- Candidatos por zona (promedio): 18.1

### Take Profit (TP)
- Zonas analizadas: 569 | Total candidatos: 4651 | Seleccionados: 0
- Candidatos por zona (promedio): 8.2
- **Edad (barras)** - Candidatos: med=53, max=250 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 4651}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 4651}
- **Type Seleccionados**: {}
- **TF Candidatos**: {60: 1435, 5: 1334, 15: 1246, 240: 636}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=9.0 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=3.98 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.