# Informe Diagnóstico de Logs - 2025-11-11 20:54:18

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_204907.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_204907.csv`

## DFM
- Eventos de evaluación: 185
- Evaluaciones Bull: 66 | Bear: 136
- Pasaron umbral (PassedThreshold): 186
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:4, 4:12, 5:15, 6:104, 7:61, 8:6, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2219
- KeptAligned: 1855/1855 | KeptCounter: 1278/1278
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.369 | AvgProxCounter≈ 0.234
  - AvgDistATRAligned≈ 0.53 | AvgDistATRCounter≈ 0.38
- PreferAligned eventos: 844 | Filtradas contra-bias: 241

### Proximity (Pre-PreferAligned)
- Eventos: 2219
- Aligned pre: 1855/3133 | Counter pre: 1278/3133
- AvgProxAligned(pre)≈ 0.369 | AvgDistATRAligned(pre)≈ 0.53

### Proximity Drivers
- Eventos: 2219
- Alineadas: n=1855 | BaseProx≈ 0.872 | ZoneATR≈ 5.02 | SizePenalty≈ 0.978 | FinalProx≈ 0.853
- Contra-bias: n=1037 | BaseProx≈ 0.764 | ZoneATR≈ 4.63 | SizePenalty≈ 0.981 | FinalProx≈ 0.750

## Risk
- Eventos: 1411
- Accepted=202 | RejSL=0 | RejTP=0 | RejRR=180 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 32 (7.3% del total)
  - Avg Score: 0.41 | Avg R:R: 1.93 | Avg DistATR: 3.51
  - Por TF: TF5=8, TF15=24
- **P0_SWING_LITE:** 407 (92.7% del total)
  - Avg Score: 0.50 | Avg R:R: 3.92 | Avg DistATR: 3.43
  - Por TF: TF15=72, TF60=335


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 56 | Unmatched: 147
- 0-10: Wins=20 Losses=36 WR=35.7% (n=56)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=20 Losses=36 WR=35.7% (n=56)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 203 | Aligned=121 (59.6%)
- Core≈ 1.00 | Prox≈ 0.81 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.77 | Confidence≈ 0.00
- SL_TF dist: {'60': 16, '15': 149, '5': 38} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 38, '15': 107, '60': 48, '240': 10} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=202, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=37, 15m=149, 60m=16, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.77 (n=202), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 39755 | Zonas con Anchors: 39741
- Dir zonas (zona): Bull=8870 Bear=29924 Neutral=961
- Resumen por ciclo (promedios): TotHZ≈ 3.7, WithAnchors≈ 3.7, DirBull≈ 1.5, DirBear≈ 2.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 38544, 'tie-bias': 1197, 'triggers-only': 14}
- TF Triggers: {'5': 4999, '15': 4302}
- TF Anchors: {'60': 9243, '240': 5466, '1440': 556}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 2}

## CSV de Trades
- Filas: 95 | Ejecutadas: 35 | Canceladas: 0 | Expiradas: 0
- BUY: 55 | SELL: 75

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 186
- Registered: 51
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 28 | SKIP_CONCURRENCY: 6
- Intentos de registro: 85

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 45.7%
- RegRate = Registered / Intentos = 60.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 32.9%
- Concurrency = SKIP_CONCURRENCY / Intentos = 7.1%
- ExecRate = Ejecutadas / Registered = 68.6%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 611 | Total candidatos: 10075 | Seleccionados: 0
- Candidatos por zona (promedio): 16.5

### Take Profit (TP)
- Zonas analizadas: 598 | Total candidatos: 4595 | Seleccionados: 598
- Candidatos por zona (promedio): 7.7
- **Edad (barras)** - Candidatos: med=53, max=182 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.40 | Seleccionados: avg=0.70
- **Priority Candidatos**: {'P3': 4595}
- **Priority Seleccionados**: {'NA': 267, 'P0': 65, 'P3': 266}
- **Type Candidatos**: {'Swing': 4595}
- **Type Seleccionados**: {'P4_Fallback': 267, 'P0_Zone': 65, 'P3_Swing': 266}
- **TF Candidatos**: {60: 1437, 15: 1205, 5: 1141, 240: 812}
- **TF Seleccionados**: {-1: 267, 5: 72, 15: 150, 60: 85, 240: 24}
- **DistATR** - Candidatos: avg=9.1 | Seleccionados: avg=3.6
- **RR** - Candidatos: avg=4.64 | Seleccionados: avg=1.33
- **Razones de selección**: {'BestIntelligentScore': 598}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.