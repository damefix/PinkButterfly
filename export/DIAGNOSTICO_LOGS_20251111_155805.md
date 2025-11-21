# Informe Diagnóstico de Logs - 2025-11-11 16:02:40

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_155805.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_155805.csv`

## DFM
- Eventos de evaluación: 155
- Evaluaciones Bull: 44 | Bear: 128
- Pasaron umbral (PassedThreshold): 161
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:1, 4:10, 5:22, 6:86, 7:52, 8:1, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2182
- KeptAligned: 1769/1769 | KeptCounter: 1167/1167
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.376 | AvgProxCounter≈ 0.253
  - AvgDistATRAligned≈ 0.53 | AvgDistATRCounter≈ 0.38
- PreferAligned eventos: 914 | Filtradas contra-bias: 331

### Proximity (Pre-PreferAligned)
- Eventos: 2182
- Aligned pre: 1769/2936 | Counter pre: 1167/2936
- AvgProxAligned(pre)≈ 0.376 | AvgDistATRAligned(pre)≈ 0.53

### Proximity Drivers
- Eventos: 2182
- Alineadas: n=1769 | BaseProx≈ 0.918 | ZoneATR≈ 5.18 | SizePenalty≈ 0.976 | FinalProx≈ 0.896
- Contra-bias: n=836 | BaseProx≈ 0.837 | ZoneATR≈ 4.79 | SizePenalty≈ 0.978 | FinalProx≈ 0.818

## Risk
- Eventos: 1366
- Accepted=172 | RejSL=0 | RejTP=0 | RejRR=151 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 33 (8.4% del total)
  - Avg Score: 0.43 | Avg R:R: 1.80 | Avg DistATR: 3.32
  - Por TF: TF5=9, TF15=24
- **P0_SWING_LITE:** 362 (91.6% del total)
  - Avg Score: 0.61 | Avg R:R: 3.71 | Avg DistATR: 3.39
  - Por TF: TF15=86, TF60=276


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 73 | Unmatched: 100
- 0-10: Wins=25 Losses=48 WR=34.2% (n=73)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=25 Losses=48 WR=34.2% (n=73)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 173 | Aligned=113 (65.3%)
- Core≈ 1.00 | Prox≈ 0.89 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.68 | Confidence≈ 0.00
- SL_TF dist: {'15': 145, '5': 28} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 49, '5': 33, '15': 81, '240': 10} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=172, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=28, 15m=144, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.68 (n=172), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 42051 | Zonas con Anchors: 42039
- Dir zonas (zona): Bull=7970 Bear=33021 Neutral=1060
- Resumen por ciclo (promedios): TotHZ≈ 3.6, WithAnchors≈ 3.6, DirBull≈ 1.3, DirBear≈ 2.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 40864, 'tie-bias': 1175, 'triggers-only': 12}
- TF Triggers: {'5': 4970, '15': 3947}
- TF Anchors: {'60': 8869, '240': 5555, '1440': 387}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 7, 'score decayó a 0,21': 1, 'score decayó a 0,49': 1, 'score decayó a 0,48': 2, 'score decayó a 0,30': 1, 'score decayó a 0,29': 1, 'score decayó a 0,18': 1, 'score decayó a 0,50': 1}

## CSV de Trades
- Filas: 140 | Ejecutadas: 49 | Canceladas: 0 | Expiradas: 0
- BUY: 61 | SELL: 128

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 161
- Registered: 76
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 44 | SKIP_CONCURRENCY: 14
- Intentos de registro: 134

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 83.2%
- RegRate = Registered / Intentos = 56.7%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 32.8%
- Concurrency = SKIP_CONCURRENCY / Intentos = 10.4%
- ExecRate = Ejecutadas / Registered = 64.5%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 551 | Total candidatos: 9759 | Seleccionados: 0
- Candidatos por zona (promedio): 17.7

### Take Profit (TP)
- Zonas analizadas: 544 | Total candidatos: 4599 | Seleccionados: 0
- Candidatos por zona (promedio): 8.5
- **Edad (barras)** - Candidatos: med=53, max=250 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 4599}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 4599}
- **Type Seleccionados**: {}
- **TF Candidatos**: {60: 1413, 5: 1290, 15: 1225, 240: 671}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=9.1 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=4.12 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.