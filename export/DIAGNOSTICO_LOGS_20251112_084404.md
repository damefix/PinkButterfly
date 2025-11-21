# Informe Diagnóstico de Logs - 2025-11-12 08:58:21

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_084404.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251112_084404.csv`

## DFM
- Eventos de evaluación: 182
- Evaluaciones Bull: 62 | Bear: 135
- Pasaron umbral (PassedThreshold): 177
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:3, 4:17, 5:16, 6:94, 7:58, 8:9, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2176
- KeptAligned: 1802/1802 | KeptCounter: 1166/1166
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.362 | AvgProxCounter≈ 0.222
  - AvgDistATRAligned≈ 0.52 | AvgDistATRCounter≈ 0.34
- PreferAligned eventos: 821 | Filtradas contra-bias: 209

### Proximity (Pre-PreferAligned)
- Eventos: 2176
- Aligned pre: 1802/2968 | Counter pre: 1166/2968
- AvgProxAligned(pre)≈ 0.362 | AvgDistATRAligned(pre)≈ 0.52

### Proximity Drivers
- Eventos: 2176
- Alineadas: n=1802 | BaseProx≈ 0.873 | ZoneATR≈ 5.01 | SizePenalty≈ 0.980 | FinalProx≈ 0.855
- Contra-bias: n=957 | BaseProx≈ 0.766 | ZoneATR≈ 4.63 | SizePenalty≈ 0.982 | FinalProx≈ 0.752

## Risk
- Eventos: 1350
- Accepted=197 | RejSL=0 | RejTP=0 | RejRR=166 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 29 (6.8% del total)
  - Avg Score: 0.41 | Avg R:R: 2.04 | Avg DistATR: 3.57
  - Por TF: TF5=8, TF15=21
- **P0_SWING_LITE:** 396 (93.2% del total)
  - Avg Score: 0.52 | Avg R:R: 4.04 | Avg DistATR: 3.41
  - Por TF: TF15=71, TF60=325


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 54 | Unmatched: 144
- 0-10: Wins=20 Losses=34 WR=37.0% (n=54)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=20 Losses=34 WR=37.0% (n=54)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 198 | Aligned=109 (55.1%)
- Core≈ 1.00 | Prox≈ 0.80 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.75 | Confidence≈ 0.00
- SL_TF dist: {'60': 14, '5': 36, '15': 148} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 90, '60': 61, '5': 37, '240': 10} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=197, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=35, 15m=148, 60m=14, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.75 (n=197), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 18484 | Zonas con Anchors: 18470
- Dir zonas (zona): Bull=5657 Bear=12232 Neutral=595
- Resumen por ciclo (promedios): TotHZ≈ 3.3, WithAnchors≈ 3.3, DirBull≈ 1.3, DirBear≈ 1.9, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 17821, 'tie-bias': 649, 'triggers-only': 14}
- TF Triggers: {'15': 3544, '5': 4792}
- TF Anchors: {'60': 8278, '240': 5022, '1440': 529}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 88 | Ejecutadas: 33 | Canceladas: 0 | Expiradas: 0
- BUY: 50 | SELL: 71

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 177
- Registered: 46
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 23 | SKIP_CONCURRENCY: 7
- Intentos de registro: 76

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 42.9%
- RegRate = Registered / Intentos = 60.5%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 30.3%
- Concurrency = SKIP_CONCURRENCY / Intentos = 9.2%
- ExecRate = Ejecutadas / Registered = 71.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 588 | Total candidatos: 9686 | Seleccionados: 0
- Candidatos por zona (promedio): 16.5

### Take Profit (TP)
- Zonas analizadas: 580 | Total candidatos: 4411 | Seleccionados: 580
- Candidatos por zona (promedio): 7.6
- **Edad (barras)** - Candidatos: med=51, max=182 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.40 | Seleccionados: avg=0.70
- **Priority Candidatos**: {'P3': 4411}
- **Priority Seleccionados**: {'P0': 52, 'P3': 267, 'NA': 261}
- **Type Candidatos**: {'Swing': 4411}
- **Type Seleccionados**: {'P0_Zone': 52, 'P3_Swing': 267, 'P4_Fallback': 261}
- **TF Candidatos**: {60: 1400, 15: 1143, 5: 1048, 240: 820}
- **TF Seleccionados**: {15: 128, 60: 93, -1: 261, 5: 74, 240: 24}
- **DistATR** - Candidatos: avg=9.7 | Seleccionados: avg=3.6
- **RR** - Candidatos: avg=4.86 | Seleccionados: avg=1.34
- **Razones de selección**: {'BestIntelligentScore': 580}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.