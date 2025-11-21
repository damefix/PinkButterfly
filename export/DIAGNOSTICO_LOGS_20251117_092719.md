# Informe Diagnóstico de Logs - 2025-11-17 09:41:06

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_092719.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251117_092719.csv`

## DFM
- Eventos de evaluación: 695
- Evaluaciones Bull: 112 | Bear: 324
- Pasaron umbral (PassedThreshold): 436
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:24, 6:129, 7:245, 8:38, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 25090
- KeptAligned: 1987/1987 | KeptCounter: 4110/4464
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.044 | AvgProxCounter≈ 0.043
  - AvgDistATRAligned≈ 0.16 | AvgDistATRCounter≈ 0.21
- PreferAligned eventos: 1426 | Filtradas contra-bias: 1983

### Proximity (Pre-PreferAligned)
- Eventos: 25090
- Aligned pre: 1987/6097 | Counter pre: 4110/6097
- AvgProxAligned(pre)≈ 0.044 | AvgDistATRAligned(pre)≈ 0.16

### Proximity Drivers
- Eventos: 25090
- Alineadas: n=1987 | BaseProx≈ 0.752 | ZoneATR≈ 4.94 | SizePenalty≈ 0.976 | FinalProx≈ 0.732
- Contra-bias: n=2127 | BaseProx≈ 0.516 | ZoneATR≈ 4.75 | SizePenalty≈ 0.975 | FinalProx≈ 0.501

## Risk
- Eventos: 2491
- Accepted=841 | RejSL=0 | RejTP=0 | RejRR=519 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 91 (4.2% del total)
  - Avg Score: 0.38 | Avg R:R: 1.61 | Avg DistATR: 3.33
  - Por TF: TF5=27, TF15=64
- **P0_SWING_LITE:** 2075 (95.8% del total)
  - Avg Score: 0.49 | Avg R:R: 5.93 | Avg DistATR: 3.91
  - Por TF: TF15=307, TF60=1768


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 205 | Unmatched: 636
- 0-10: Wins=118 Losses=87 WR=57.6% (n=205)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=118 Losses=87 WR=57.6% (n=205)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 841 | Aligned=345 (41.0%)
- Core≈ 1.00 | Prox≈ 0.63 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.29 | Confidence≈ 0.00
- SL_TF dist: {'15': 841} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 31, '240': 439, '15': 287, '60': 84} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=841, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=0, 15m=841, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.29 (n=841), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 88141 | Zonas con Anchors: 88141
- Dir zonas (zona): Bull=42715 Bear=41747 Neutral=3679
- Resumen por ciclo (promedios): TotHZ≈ 3.4, WithAnchors≈ 3.4, DirBull≈ 1.6, DirBear≈ 1.6, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 83622, 'tie-bias': 4519}
- TF Triggers: {'5': 65489, '15': 22652}
- TF Anchors: {'60': 88141, '240': 88141, '1440': 88141}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,20': 4, 'score decayó a 0,48': 1}

## CSV de Trades
- Filas: 98 | Ejecutadas: 29 | Canceladas: 0 | Expiradas: 0
- BUY: 16 | SELL: 111

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 436
- Registered: 49
  - DEDUP_COOLDOWN: 45 | DEDUP_IDENTICAL: 110 | SKIP_CONCURRENCY: 14
- Intentos de registro: 218

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 50.0%
- RegRate = Registered / Intentos = 22.5%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 71.1%
- Concurrency = SKIP_CONCURRENCY / Intentos = 6.4%
- ExecRate = Ejecutadas / Registered = 59.2%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 3781 | Total candidatos: 23310 | Seleccionados: 0
- Candidatos por zona (promedio): 6.2

### Take Profit (TP)
- Zonas analizadas: 3781 | Total candidatos: 56997 | Seleccionados: 3781
- Candidatos por zona (promedio): 15.1
- **Edad (barras)** - Candidatos: med=37, max=91 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.66
- **Priority Candidatos**: {'P3': 7833}
- **Priority Seleccionados**: {'NA': 1227, 'P0': 111, 'P3': 2443}
- **Type Candidatos**: {'Swing': 7833}
- **Type Seleccionados**: {'P4_Fallback': 1227, 'P0_Zone': 111, 'P3_Swing': 2443}
- **TF Candidatos**: {240: 6282, 60: 1153, 15: 398}
- **TF Seleccionados**: {-1: 1227, 5: 45, 240: 1979, 15: 432, 60: 98}
- **DistATR** - Candidatos: avg=16.0 | Seleccionados: avg=9.8
- **RR** - Candidatos: avg=4.40 | Seleccionados: avg=1.61
- **Razones de selección**: {'BestIntelligentScore': 3781}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.