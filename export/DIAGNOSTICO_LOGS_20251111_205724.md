# Informe Diagnóstico de Logs - 2025-11-11 21:00:29

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_205724.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_205724.csv`

## DFM
- Eventos de evaluación: 183
- Evaluaciones Bull: 64 | Bear: 136
- Pasaron umbral (PassedThreshold): 184
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:4, 4:12, 5:15, 6:102, 7:61, 8:6, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2219
- KeptAligned: 1841/1841 | KeptCounter: 1274/1274
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.368 | AvgProxCounter≈ 0.235
  - AvgDistATRAligned≈ 0.53 | AvgDistATRCounter≈ 0.38
- PreferAligned eventos: 837 | Filtradas contra-bias: 240

### Proximity (Pre-PreferAligned)
- Eventos: 2219
- Aligned pre: 1841/3115 | Counter pre: 1274/3115
- AvgProxAligned(pre)≈ 0.368 | AvgDistATRAligned(pre)≈ 0.53

### Proximity Drivers
- Eventos: 2219
- Alineadas: n=1841 | BaseProx≈ 0.872 | ZoneATR≈ 5.02 | SizePenalty≈ 0.978 | FinalProx≈ 0.853
- Contra-bias: n=1034 | BaseProx≈ 0.763 | ZoneATR≈ 4.67 | SizePenalty≈ 0.981 | FinalProx≈ 0.748

## Risk
- Eventos: 1413
- Accepted=200 | RejSL=0 | RejTP=0 | RejRR=181 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 31 (7.1% del total)
  - Avg Score: 0.41 | Avg R:R: 1.91 | Avg DistATR: 3.47
  - Por TF: TF5=8, TF15=23
- **P0_SWING_LITE:** 408 (92.9% del total)
  - Avg Score: 0.50 | Avg R:R: 3.95 | Avg DistATR: 3.45
  - Por TF: TF15=70, TF60=338


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 54 | Unmatched: 147
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
- Muestras: 201 | Aligned=118 (58.7%)
- Core≈ 1.00 | Prox≈ 0.81 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.78 | Confidence≈ 0.00
- SL_TF dist: {'60': 16, '15': 147, '5': 38} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 39, '15': 104, '60': 48, '240': 10} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=200, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=37, 15m=147, 60m=16, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.78 (n=200), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 39796 | Zonas con Anchors: 39782
- Dir zonas (zona): Bull=8971 Bear=29859 Neutral=966
- Resumen por ciclo (promedios): TotHZ≈ 3.7, WithAnchors≈ 3.7, DirBull≈ 1.4, DirBear≈ 2.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 38583, 'tie-bias': 1199, 'triggers-only': 14}
- TF Triggers: {'5': 5000, '15': 4300}
- TF Anchors: {'60': 9242, '240': 5469, '1440': 559}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 2}

## CSV de Trades
- Filas: 95 | Ejecutadas: 36 | Canceladas: 0 | Expiradas: 0
- BUY: 56 | SELL: 75

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 184
- Registered: 51
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 25 | SKIP_CONCURRENCY: 6
- Intentos de registro: 82

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 44.6%
- RegRate = Registered / Intentos = 62.2%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 30.5%
- Concurrency = SKIP_CONCURRENCY / Intentos = 7.3%
- ExecRate = Ejecutadas / Registered = 70.6%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 605 | Total candidatos: 9952 | Seleccionados: 0
- Candidatos por zona (promedio): 16.4

### Take Profit (TP)
- Zonas analizadas: 595 | Total candidatos: 4586 | Seleccionados: 595
- Candidatos por zona (promedio): 7.7
- **Edad (barras)** - Candidatos: med=53, max=182 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.40 | Seleccionados: avg=0.70
- **Priority Candidatos**: {'P3': 4586}
- **Priority Seleccionados**: {'NA': 269, 'P0': 63, 'P3': 263}
- **Type Candidatos**: {'Swing': 4586}
- **Type Seleccionados**: {'P4_Fallback': 269, 'P0_Zone': 63, 'P3_Swing': 263}
- **TF Candidatos**: {60: 1448, 15: 1198, 5: 1129, 240: 811}
- **TF Seleccionados**: {-1: 269, 5: 74, 15: 145, 60: 83, 240: 24}
- **DistATR** - Candidatos: avg=9.2 | Seleccionados: avg=3.6
- **RR** - Candidatos: avg=4.68 | Seleccionados: avg=1.34
- **Razones de selección**: {'BestIntelligentScore': 595}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.