# Informe Diagnóstico de Logs - 2025-11-17 16:07:43

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_160027.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251117_160027.csv`

## DFM
- Eventos de evaluación: 444
- Evaluaciones Bull: 5 | Bear: 276
- Pasaron umbral (PassedThreshold): 281
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:7, 6:106, 7:116, 8:51, 9:1

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 22101
- KeptAligned: 1324/1324 | KeptCounter: 1571/1674
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.020 | AvgProxCounter≈ 0.019
  - AvgDistATRAligned≈ 0.07 | AvgDistATRCounter≈ 0.09
- PreferAligned eventos: 460 | Filtradas contra-bias: 193

### Proximity (Pre-PreferAligned)
- Eventos: 22101
- Aligned pre: 1324/2895 | Counter pre: 1571/2895
- AvgProxAligned(pre)≈ 0.020 | AvgDistATRAligned(pre)≈ 0.07

### Proximity Drivers
- Eventos: 22101
- Alineadas: n=1324 | BaseProx≈ 0.754 | ZoneATR≈ 6.33 | SizePenalty≈ 0.954 | FinalProx≈ 0.721
- Contra-bias: n=1378 | BaseProx≈ 0.525 | ZoneATR≈ 7.11 | SizePenalty≈ 0.936 | FinalProx≈ 0.489

## Risk
- Eventos: 1227
- Accepted=588 | RejSL=0 | RejTP=0 | RejRR=298 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 63 (4.7% del total)
  - Avg Score: 0.39 | Avg R:R: 1.77 | Avg DistATR: 4.25
  - Por TF: TF5=39, TF15=24
- **P0_SWING_LITE:** 1269 (95.3% del total)
  - Avg Score: 0.59 | Avg R:R: 4.15 | Avg DistATR: 3.69
  - Por TF: TF15=135, TF60=1134


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 188 | Unmatched: 400
- 0-10: Wins=3 Losses=185 WR=1.6% (n=188)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=3 Losses=185 WR=1.6% (n=188)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 588 | Aligned=214 (36.4%)
- Core≈ 1.00 | Prox≈ 0.58 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.30 | Confidence≈ 0.00
- SL_TF dist: {'5': 184, '15': 365, '240': 1, '60': 38} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 86, '60': 273, '5': 29, '240': 200} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=588, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=184, 15m=365, 60m=38, 240m=1, 1440m=0
- RR plan por bandas: 0-10≈ 2.30 (n=588), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 80183 | Zonas con Anchors: 80183
- Dir zonas (zona): Bull=3065 Bear=65977 Neutral=11141
- Resumen por ciclo (promedios): TotHZ≈ 3.4, WithAnchors≈ 3.4, DirBull≈ 0.1, DirBear≈ 2.8, DirNeutral≈ 0.5
- Razones de dirección: {'anchors+triggers': 67434, 'tie-bias': 12749}
- TF Triggers: {'5': 66798, '15': 13385}
- TF Anchors: {'60': 80183, '240': 80183, '1440': 80183}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)

## CSV de Trades
- Filas: 58 | Ejecutadas: 13 | Canceladas: 0 | Expiradas: 0
- BUY: 4 | SELL: 67

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 281
- Registered: 29
  - DEDUP_COOLDOWN: 62 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 14
- Intentos de registro: 105

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 37.4%
- RegRate = Registered / Intentos = 27.6%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 59.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 13.3%
- ExecRate = Ejecutadas / Registered = 44.8%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 2377 | Total candidatos: 22354 | Seleccionados: 0
- Candidatos por zona (promedio): 9.4

### Take Profit (TP)
- Zonas analizadas: 2377 | Total candidatos: 31222 | Seleccionados: 2377
- Candidatos por zona (promedio): 13.1
- **Edad (barras)** - Candidatos: med=17, max=93 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.53 | Seleccionados: avg=0.70
- **Priority Candidatos**: {'P3': 2290}
- **Priority Seleccionados**: {'NA': 327, 'P3': 1998, 'P0': 52}
- **Type Candidatos**: {'Swing': 2290}
- **Type Seleccionados**: {'P4_Fallback': 327, 'P3_Swing': 1998, 'P0_Zone': 52}
- **TF Candidatos**: {240: 1809, 60: 383, 15: 68, 5: 30}
- **TF Seleccionados**: {-1: 327, 15: 352, 240: 928, 60: 624, 5: 146}
- **DistATR** - Candidatos: avg=9.2 | Seleccionados: avg=9.7
- **RR** - Candidatos: avg=3.60 | Seleccionados: avg=1.49
- **Razones de selección**: {'BestIntelligentScore': 2377}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.