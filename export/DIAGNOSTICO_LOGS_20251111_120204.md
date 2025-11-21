# Informe Diagnóstico de Logs - 2025-11-11 12:05:23

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_120204.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_120204.csv`

## DFM
- Eventos de evaluación: 216
- Evaluaciones Bull: 58 | Bear: 193
- Pasaron umbral (PassedThreshold): 95
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:21, 4:4, 5:49, 6:110, 7:67, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2183
- KeptAligned: 1782/1782 | KeptCounter: 1144/1144
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.379 | AvgProxCounter≈ 0.250
  - AvgDistATRAligned≈ 0.53 | AvgDistATRCounter≈ 0.37
- PreferAligned eventos: 921 | Filtradas contra-bias: 335

### Proximity (Pre-PreferAligned)
- Eventos: 2183
- Aligned pre: 1782/2926 | Counter pre: 1144/2926
- AvgProxAligned(pre)≈ 0.379 | AvgDistATRAligned(pre)≈ 0.53

### Proximity Drivers
- Eventos: 2183
- Alineadas: n=1782 | BaseProx≈ 0.918 | ZoneATR≈ 5.07 | SizePenalty≈ 0.977 | FinalProx≈ 0.897
- Contra-bias: n=809 | BaseProx≈ 0.835 | ZoneATR≈ 4.86 | SizePenalty≈ 0.978 | FinalProx≈ 0.817

## Risk
- Eventos: 1353
- Accepted=252 | RejSL=0 | RejTP=0 | RejRR=100 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 36 (8.9% del total)
  - Avg Score: 0.43 | Avg R:R: 1.83 | Avg DistATR: 3.35
  - Por TF: TF5=11, TF15=25
- **P0_SWING_LITE:** 367 (91.1% del total)
  - Avg Score: 0.62 | Avg R:R: 3.65 | Avg DistATR: 3.35
  - Por TF: TF15=90, TF60=277


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 22 | Unmatched: 232
- 0-10: Wins=9 Losses=13 WR=40.9% (n=22)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=9 Losses=13 WR=40.9% (n=22)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 254 | Aligned=187 (73.6%)
- Core≈ 1.00 | Prox≈ 0.89 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.06 | Confidence≈ 0.00
- SL_TF dist: {'15': 200, '5': 54} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 108, '60': 70, '5': 57, '240': 19} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=252, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=53, 15m=199, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.07 (n=252), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 41963 | Zonas con Anchors: 41951
- Dir zonas (zona): Bull=7921 Bear=32968 Neutral=1074
- Resumen por ciclo (promedios): TotHZ≈ 3.6, WithAnchors≈ 3.6, DirBull≈ 1.3, DirBear≈ 2.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 40806, 'tie-bias': 1145, 'triggers-only': 12}
- TF Triggers: {'5': 4987, '15': 3933}
- TF Anchors: {'60': 8872, '240': 5416, '1440': 299}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,48': 1, 'estructura no existe': 2, 'score decayó a 0,29': 1}

## CSV de Trades
- Filas: 47 | Ejecutadas: 17 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 64

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 95
- Registered: 26
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 14 | SKIP_CONCURRENCY: 19
- Intentos de registro: 59

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 62.1%
- RegRate = Registered / Intentos = 44.1%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 23.7%
- Concurrency = SKIP_CONCURRENCY / Intentos = 32.2%
- ExecRate = Ejecutadas / Registered = 65.4%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 570 | Total candidatos: 10154 | Seleccionados: 0
- Candidatos por zona (promedio): 17.8

### Take Profit (TP)
- Zonas analizadas: 562 | Total candidatos: 4587 | Seleccionados: 0
- Candidatos por zona (promedio): 8.2
- **Edad (barras)** - Candidatos: med=53, max=250 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 4587}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 4587}
- **Type Seleccionados**: {}
- **TF Candidatos**: {60: 1372, 5: 1317, 15: 1246, 240: 652}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=9.0 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=4.01 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.