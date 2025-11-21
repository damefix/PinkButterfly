# Informe Diagnóstico de Logs - 2025-11-10 09:11:51

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251110_091117.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251110_091117.csv`

## DFM
- Eventos de evaluación: 60
- Evaluaciones Bull: 3 | Bear: 89
- Pasaron umbral (PassedThreshold): 92
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:3, 5:0, 6:35, 7:31, 8:23, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 114
- KeptAligned: 226/226 | KeptCounter: 23/23
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.664 | AvgProxCounter≈ 0.174
  - AvgDistATRAligned≈ 2.08 | AvgDistATRCounter≈ 0.15
- PreferAligned eventos: 92 | Filtradas contra-bias: 0

### Proximity (Pre-PreferAligned)
- Eventos: 114
- Aligned pre: 226/249 | Counter pre: 23/249
- AvgProxAligned(pre)≈ 0.664 | AvgDistATRAligned(pre)≈ 2.08

### Proximity Drivers
- Eventos: 114
- Alineadas: n=226 | BaseProx≈ 0.845 | ZoneATR≈ 4.35 | SizePenalty≈ 0.984 | FinalProx≈ 0.832
- Contra-bias: n=23 | BaseProx≈ 0.898 | ZoneATR≈ 3.13 | SizePenalty≈ 1.000 | FinalProx≈ 0.898

## Risk
- Eventos: 114
- Accepted=92 | RejSL=0 | RejTP=0 | RejRR=58 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 51 (70.8% del total)
  - Avg Score: 0.67 | Avg R:R: 2.02 | Avg DistATR: 8.69
  - Por TF: TF5=19, TF15=32
- **P0_SWING_LITE:** 21 (29.2% del total)
  - Avg Score: 0.50 | Avg R:R: 8.44 | Avg DistATR: 7.03
  - Por TF: TF15=14, TF60=7


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 14 | Unmatched: 78
- 0-10: Wins=11 Losses=3 WR=78.6% (n=14)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=11 Losses=3 WR=78.6% (n=14)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 92 | Aligned=89 (96.7%)
- Core≈ 1.00 | Prox≈ 0.87 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.51 | Confidence≈ 0.00
- SL_TF dist: {'15': 92} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 8, '240': 32, '15': 49, '5': 3} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=92, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=0, 15m=92, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.51 (n=92), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 1010 | Zonas con Anchors: 1010
- Dir zonas (zona): Bull=14 Bear=987 Neutral=9
- Resumen por ciclo (promedios): TotHZ≈ 2.1, WithAnchors≈ 2.1, DirBull≈ 0.1, DirBear≈ 1.9, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 1001, 'tie-bias': 9}
- TF Triggers: {'15': 211, '5': 38}
- TF Anchors: {'60': 249, '240': 249, '1440': 249}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,35': 1, 'score decayó a 0,41': 2, 'score decayó a 0,33': 8, 'estructura no existe': 1, 'score decayó a 0,47': 1}

## CSV de Trades
- Filas: 80 | Ejecutadas: 7 | Canceladas: 0 | Expiradas: 0
- BUY: 6 | SELL: 81

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 92
- Registered: 43
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 43

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 46.7%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 16.3%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 229 | Total candidatos: 1796 | Seleccionados: 0
- Candidatos por zona (promedio): 7.8

### Take Profit (TP)
- Zonas analizadas: 229 | Total candidatos: 3129 | Seleccionados: 0
- Candidatos por zona (promedio): 13.7
- **Edad (barras)** - Candidatos: med=81, max=150 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.38 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 3129}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 3129}
- **Type Seleccionados**: {}
- **TF Candidatos**: {240: 1737, 15: 774, 5: 353, 60: 265}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=8.9 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=3.57 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.