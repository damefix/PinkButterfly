# Informe Diagnóstico de Logs - 2025-11-17 18:24:12

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_181420.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251117_181420.csv`

## DFM
- Eventos de evaluación: 299
- Evaluaciones Bull: 37 | Bear: 170
- Pasaron umbral (PassedThreshold): 207
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:16, 6:70, 7:93, 8:28, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 20736
- KeptAligned: 1064/1064 | KeptCounter: 1354/1495
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.020 | AvgProxCounter≈ 0.017
  - AvgDistATRAligned≈ 0.08 | AvgDistATRCounter≈ 0.12
- PreferAligned eventos: 547 | Filtradas contra-bias: 353

### Proximity (Pre-PreferAligned)
- Eventos: 20736
- Aligned pre: 1064/2418 | Counter pre: 1354/2418
- AvgProxAligned(pre)≈ 0.020 | AvgDistATRAligned(pre)≈ 0.08

### Proximity Drivers
- Eventos: 20736
- Alineadas: n=1064 | BaseProx≈ 0.721 | ZoneATR≈ 6.26 | SizePenalty≈ 0.949 | FinalProx≈ 0.685
- Contra-bias: n=1001 | BaseProx≈ 0.452 | ZoneATR≈ 7.43 | SizePenalty≈ 0.935 | FinalProx≈ 0.422

## Risk
- Eventos: 1121
- Accepted=355 | RejSL=0 | RejTP=0 | RejRR=345 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 21 (1.7% del total)
  - Avg Score: 0.40 | Avg R:R: 2.38 | Avg DistATR: 3.95
  - Por TF: TF5=6, TF15=15
- **P0_SWING_LITE:** 1184 (98.3% del total)
  - Avg Score: 0.68 | Avg R:R: 3.18 | Avg DistATR: 3.92
  - Por TF: TF15=281, TF60=903


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 89 | Unmatched: 268
- 0-10: Wins=34 Losses=55 WR=38.2% (n=89)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=34 Losses=55 WR=38.2% (n=89)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 357 | Aligned=213 (59.7%)
- Core≈ 1.00 | Prox≈ 0.58 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.88 | Confidence≈ 0.00
- SL_TF dist: {'5': 34, '15': 291, '1440': 3, '60': 25, '240': 4} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 89, '15': 225, '240': 35, '5': 8} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=355, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=34, 15m=289, 60m=25, 240m=4, 1440m=3
- RR plan por bandas: 0-10≈ 1.87 (n=355), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 54369 | Zonas con Anchors: 54369
- Dir zonas (zona): Bull=15232 Bear=34703 Neutral=4434
- Resumen por ciclo (promedios): TotHZ≈ 2.3, WithAnchors≈ 2.3, DirBull≈ 0.7, DirBear≈ 1.5, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 49034, 'tie-bias': 5335}
- TF Triggers: {'5': 41284, '15': 13085}
- TF Anchors: {'60': 54369, '240': 54369, '1440': 54369}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,31': 1}

## CSV de Trades
- Filas: 48 | Ejecutadas: 7 | Canceladas: 0 | Expiradas: 0
- BUY: 12 | SELL: 43

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 207
- Registered: 24
  - DEDUP_COOLDOWN: 38 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 18
- Intentos de registro: 80

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 38.6%
- RegRate = Registered / Intentos = 30.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 47.5%
- Concurrency = SKIP_CONCURRENCY / Intentos = 22.5%
- ExecRate = Ejecutadas / Registered = 29.2%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 1890 | Total candidatos: 8724 | Seleccionados: 0
- Candidatos por zona (promedio): 4.6

### Take Profit (TP)
- Zonas analizadas: 1854 | Total candidatos: 34282 | Seleccionados: 1854
- Candidatos por zona (promedio): 18.5
- **Edad (barras)** - Candidatos: med=28, max=93 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.68
- **Priority Candidatos**: {'P3': 2791}
- **Priority Seleccionados**: {'P3': 1495, 'NA': 231, 'P0': 128}
- **Type Candidatos**: {'Swing': 2791}
- **Type Seleccionados**: {'P3_Swing': 1495, 'P4_Fallback': 231, 'P0_Zone': 128}
- **TF Candidatos**: {240: 2099, 60: 328, 15: 249, 5: 115}
- **TF Seleccionados**: {5: 210, -1: 231, 240: 456, 15: 736, 60: 221}
- **DistATR** - Candidatos: avg=9.7 | Seleccionados: avg=6.8
- **RR** - Candidatos: avg=3.58 | Seleccionados: avg=1.41
- **Razones de selección**: {'BestIntelligentScore': 1854}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.