# Informe Diagnóstico de Logs - 2025-11-11 13:54:08

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_133224.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_133224.csv`

## DFM
- Eventos de evaluación: 204
- Evaluaciones Bull: 54 | Bear: 185
- Pasaron umbral (PassedThreshold): 213
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:21, 4:5, 5:54, 6:107, 7:51, 8:1, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2182
- KeptAligned: 1754/1754 | KeptCounter: 1166/1166
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.368 | AvgProxCounter≈ 0.251
  - AvgDistATRAligned≈ 0.52 | AvgDistATRCounter≈ 0.38
- PreferAligned eventos: 895 | Filtradas contra-bias: 312

### Proximity (Pre-PreferAligned)
- Eventos: 2182
- Aligned pre: 1754/2920 | Counter pre: 1166/2920
- AvgProxAligned(pre)≈ 0.368 | AvgDistATRAligned(pre)≈ 0.52

### Proximity Drivers
- Eventos: 2182
- Alineadas: n=1754 | BaseProx≈ 0.918 | ZoneATR≈ 5.14 | SizePenalty≈ 0.976 | FinalProx≈ 0.896
- Contra-bias: n=854 | BaseProx≈ 0.836 | ZoneATR≈ 4.82 | SizePenalty≈ 0.978 | FinalProx≈ 0.817

## Risk
- Eventos: 1355
- Accepted=240 | RejSL=0 | RejTP=0 | RejRR=92 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 34 (8.7% del total)
  - Avg Score: 0.42 | Avg R:R: 1.76 | Avg DistATR: 3.29
  - Por TF: TF5=13, TF15=21
- **P0_SWING_LITE:** 355 (91.3% del total)
  - Avg Score: 0.61 | Avg R:R: 3.71 | Avg DistATR: 3.38
  - Por TF: TF15=86, TF60=269


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 64 | Unmatched: 178
- 0-10: Wins=26 Losses=38 WR=40.6% (n=64)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=26 Losses=38 WR=40.6% (n=64)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 242 | Aligned=151 (62.4%)
- Core≈ 1.00 | Prox≈ 0.88 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.03 | Confidence≈ 0.00
- SL_TF dist: {'60': 1, '15': 192, '5': 49} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 56, '15': 101, '60': 73, '240': 12} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=240, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=48, 15m=191, 60m=1, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.04 (n=240), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 41897 | Zonas con Anchors: 41885
- Dir zonas (zona): Bull=7899 Bear=32959 Neutral=1039
- Resumen por ciclo (promedios): TotHZ≈ 3.6, WithAnchors≈ 3.6, DirBull≈ 1.3, DirBear≈ 2.1, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 40731, 'tie-bias': 1154, 'triggers-only': 12}
- TF Triggers: {'5': 4974, '15': 3939}
- TF Anchors: {'60': 8865, '240': 5448, '1440': 331}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 7, 'score decayó a 0,38': 1, 'score decayó a 0,21': 1, 'score decayó a 0,49': 1, 'score decayó a 0,48': 2, 'score decayó a 0,30': 1, 'score decayó a 0,29': 1, 'score decayó a 0,18': 1, 'score decayó a 0,43': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 118 | Ejecutadas: 37 | Canceladas: 0 | Expiradas: 0
- BUY: 50 | SELL: 105

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 213
- Registered: 64
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 54 | SKIP_CONCURRENCY: 46
- Intentos de registro: 164

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 77.0%
- RegRate = Registered / Intentos = 39.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 32.9%
- Concurrency = SKIP_CONCURRENCY / Intentos = 28.0%
- ExecRate = Ejecutadas / Registered = 57.8%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 548 | Total candidatos: 9629 | Seleccionados: 0
- Candidatos por zona (promedio): 17.6

### Take Profit (TP)
- Zonas analizadas: 541 | Total candidatos: 4544 | Seleccionados: 0
- Candidatos por zona (promedio): 8.4
- **Edad (barras)** - Candidatos: med=53, max=250 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 4544}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 4544}
- **Type Seleccionados**: {}
- **TF Candidatos**: {60: 1393, 5: 1270, 15: 1224, 240: 657}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=9.1 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=4.07 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.