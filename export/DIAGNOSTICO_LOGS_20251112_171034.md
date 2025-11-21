# Informe Diagnóstico de Logs - 2025-11-12 17:14:53

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_171034.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251112_171034.csv`

## DFM
- Eventos de evaluación: 955
- Evaluaciones Bull: 165 | Bear: 699
- Pasaron umbral (PassedThreshold): 864
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:74, 6:381, 7:356, 8:53, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2346
- KeptAligned: 4195/4195 | KeptCounter: 2568/2668
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.445 | AvgProxCounter≈ 0.236
  - AvgDistATRAligned≈ 1.52 | AvgDistATRCounter≈ 1.10
- PreferAligned eventos: 1289 | Filtradas contra-bias: 570

### Proximity (Pre-PreferAligned)
- Eventos: 2346
- Aligned pre: 4195/6763 | Counter pre: 2568/6763
- AvgProxAligned(pre)≈ 0.445 | AvgDistATRAligned(pre)≈ 1.52

### Proximity Drivers
- Eventos: 2346
- Alineadas: n=4195 | BaseProx≈ 0.753 | ZoneATR≈ 5.20 | SizePenalty≈ 0.974 | FinalProx≈ 0.735
- Contra-bias: n=1998 | BaseProx≈ 0.552 | ZoneATR≈ 5.04 | SizePenalty≈ 0.976 | FinalProx≈ 0.541

## Risk
- Eventos: 1956
- Accepted=1298 | RejSL=0 | RejTP=0 | RejRR=1279 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 320 (11.3% del total)
  - Avg Score: 0.39 | Avg R:R: 1.90 | Avg DistATR: 3.75
  - Por TF: TF5=82, TF15=238
- **P0_SWING_LITE:** 2512 (88.7% del total)
  - Avg Score: 0.57 | Avg R:R: 4.11 | Avg DistATR: 3.46
  - Por TF: TF15=528, TF60=1984


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 91 | Unmatched: 1235
- 0-10: Wins=36 Losses=55 WR=39.6% (n=91)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=36 Losses=55 WR=39.6% (n=91)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1326 | Aligned=822 (62.0%)
- Core≈ 1.00 | Prox≈ 0.68 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.01 | Confidence≈ 0.00
- SL_TF dist: {'60': 155, '5': 186, '15': 981, '240': 4} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 311, '15': 505, '5': 334, '240': 176} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1298, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=181, 15m=962, 60m=151, 240m=4, 1440m=0
- RR plan por bandas: 0-10≈ 2.01 (n=1298), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10211 | Zonas con Anchors: 10197
- Dir zonas (zona): Bull=3868 Bear=5988 Neutral=355
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.4, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9780, 'tie-bias': 417, 'triggers-only': 14}
- TF Triggers: {'5': 5467, '15': 4744}
- TF Anchors: {'60': 10123, '240': 5928, '1440': 732}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 26, 'score decayó a 0,27': 1, 'score decayó a 0,46': 1, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,42': 1, 'score decayó a 0,32': 1, 'score decayó a 0,34': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 193 | Ejecutadas: 33 | Canceladas: 0 | Expiradas: 0
- BUY: 69 | SELL: 157

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 864
- Registered: 102
  - DEDUP_COOLDOWN: 17 | DEDUP_IDENTICAL: 105 | SKIP_CONCURRENCY: 102
- Intentos de registro: 326

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 37.7%
- RegRate = Registered / Intentos = 31.3%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 37.4%
- Concurrency = SKIP_CONCURRENCY / Intentos = 31.3%
- ExecRate = Ejecutadas / Registered = 32.4%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5443 | Total candidatos: 43647 | Seleccionados: 0
- Candidatos por zona (promedio): 8.0

### Take Profit (TP)
- Zonas analizadas: 5348 | Total candidatos: 50785 | Seleccionados: 5348
- Candidatos por zona (promedio): 9.5
- **Edad (barras)** - Candidatos: med=41, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 50785}
- **Priority Seleccionados**: {'P3': 3634, 'P0': 338, 'NA': 1376}
- **Type Candidatos**: {'Swing': 50785}
- **Type Seleccionados**: {'P3_Swing': 3634, 'P0_Zone': 338, 'P4_Fallback': 1376}
- **TF Candidatos**: {5: 14846, 15: 13686, 60: 13376, 240: 8877}
- **TF Seleccionados**: {60: 1015, 5: 998, 15: 1240, -1: 1376, 240: 719}
- **DistATR** - Candidatos: avg=8.7 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.40 | Seleccionados: avg=1.29
- **Razones de selección**: {'BestIntelligentScore': 5348}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.