# Informe Diagnóstico de Logs - 2025-11-18 15:39:11

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251118_153445.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251118_153445.csv`

## DFM
- Eventos de evaluación: 251
- Evaluaciones Bull: 12 | Bear: 206
- Pasaron umbral (PassedThreshold): 218
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:3, 6:57, 7:96, 8:62, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 628
- KeptAligned: 967/967 | KeptCounter: 1108/1147
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.379 | AvgProxCounter≈ 0.255
  - AvgDistATRAligned≈ 1.29 | AvgDistATRCounter≈ 1.72
- PreferAligned eventos: 268 | Filtradas contra-bias: 147

### Proximity (Pre-PreferAligned)
- Eventos: 628
- Aligned pre: 967/2075 | Counter pre: 1108/2075
- AvgProxAligned(pre)≈ 0.379 | AvgDistATRAligned(pre)≈ 1.29

### Proximity Drivers
- Eventos: 628
- Alineadas: n=967 | BaseProx≈ 0.770 | ZoneATR≈ 4.89 | SizePenalty≈ 0.980 | FinalProx≈ 0.755
- Contra-bias: n=961 | BaseProx≈ 0.440 | ZoneATR≈ 4.69 | SizePenalty≈ 0.980 | FinalProx≈ 0.432

## Risk
- Eventos: 561
- Accepted=345 | RejSL=0 | RejTP=0 | RejRR=332 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 92 (11.6% del total)
  - Avg Score: 0.37 | Avg R:R: 1.84 | Avg DistATR: 3.70
  - Por TF: TF5=32, TF15=60
- **P0_SWING_LITE:** 701 (88.4% del total)
  - Avg Score: 0.62 | Avg R:R: 5.16 | Avg DistATR: 3.61
  - Por TF: TF15=161, TF60=540


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 18 | Unmatched: 335
- 0-10: Wins=5 Losses=13 WR=27.8% (n=18)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=5 Losses=13 WR=27.8% (n=18)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 353 | Aligned=157 (44.5%)
- Core≈ 1.00 | Prox≈ 0.61 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.37 | Confidence≈ 0.00
- SL_TF dist: {'15': 243, '60': 34, '5': 53, '240': 23} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 95, '15': 112, '240': 105, '60': 41} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=345, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=51, 15m=241, 60m=34, 240m=19, 1440m=0
- RR plan por bandas: 0-10≈ 2.28 (n=345), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 3236 | Zonas con Anchors: 3236
- Dir zonas (zona): Bull=370 Bear=2750 Neutral=116
- Resumen por ciclo (promedios): TotHZ≈ 5.2, WithAnchors≈ 5.2, DirBull≈ 0.6, DirBear≈ 4.4, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 3103, 'tie-bias': 133}
- TF Triggers: {'5': 1577, '15': 1659}
- TF Anchors: {'60': 3204, '240': 3236, '1440': 3236}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 6, 'score decayó a 0,22': 1, 'score decayó a 0,24': 1}

## CSV de Trades
- Filas: 56 | Ejecutadas: 13 | Canceladas: 0 | Expiradas: 0
- BUY: 7 | SELL: 62

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 218
- Registered: 29
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 29

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 13.3%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 44.8%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 1482 | Total candidatos: 13428 | Seleccionados: 0
- Candidatos por zona (promedio): 9.1

### Take Profit (TP)
- Zonas analizadas: 1467 | Total candidatos: 18044 | Seleccionados: 1467
- Candidatos por zona (promedio): 12.3
- **Edad (barras)** - Candidatos: med=37, max=157 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.68
- **Priority Candidatos**: {'P3': 18044}
- **Priority Seleccionados**: {'P3': 1108, 'NA': 306, 'P0': 53}
- **Type Candidatos**: {'Swing': 18044}
- **Type Seleccionados**: {'P3_Swing': 1108, 'P4_Fallback': 306, 'P0_Zone': 53}
- **TF Candidatos**: {240: 6479, 15: 4220, 5: 4142, 60: 3203}
- **TF Seleccionados**: {15: 282, -1: 306, 5: 287, 240: 443, 60: 149}
- **DistATR** - Candidatos: avg=12.2 | Seleccionados: avg=6.1
- **RR** - Candidatos: avg=5.43 | Seleccionados: avg=1.43
- **Razones de selección**: {'BestIntelligentScore': 1467}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.