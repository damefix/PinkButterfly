# Informe Diagnóstico de Logs - 2025-11-14 07:52:49

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_074909.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251114_074909.csv`

## DFM
- Eventos de evaluación: 922
- Evaluaciones Bull: 140 | Bear: 686
- Pasaron umbral (PassedThreshold): 826
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:67, 6:352, 7:347, 8:60, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2356
- KeptAligned: 4200/4200 | KeptCounter: 2716/2814
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.450 | AvgProxCounter≈ 0.222
  - AvgDistATRAligned≈ 1.56 | AvgDistATRCounter≈ 1.11
- PreferAligned eventos: 1298 | Filtradas contra-bias: 554

### Proximity (Pre-PreferAligned)
- Eventos: 2356
- Aligned pre: 4200/6916 | Counter pre: 2716/6916
- AvgProxAligned(pre)≈ 0.450 | AvgDistATRAligned(pre)≈ 1.56

### Proximity Drivers
- Eventos: 2356
- Alineadas: n=4200 | BaseProx≈ 0.750 | ZoneATR≈ 5.16 | SizePenalty≈ 0.975 | FinalProx≈ 0.732
- Contra-bias: n=2162 | BaseProx≈ 0.520 | ZoneATR≈ 4.86 | SizePenalty≈ 0.977 | FinalProx≈ 0.510

## Risk
- Eventos: 1957
- Accepted=1248 | RejSL=0 | RejTP=0 | RejRR=1293 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 332 (11.7% del total)
  - Avg Score: 0.39 | Avg R:R: 1.89 | Avg DistATR: 3.80
  - Por TF: TF5=88, TF15=244
- **P0_SWING_LITE:** 2513 (88.3% del total)
  - Avg Score: 0.59 | Avg R:R: 4.08 | Avg DistATR: 3.50
  - Por TF: TF15=579, TF60=1934


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 79 | Unmatched: 1208
- 0-10: Wins=37 Losses=42 WR=46.8% (n=79)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=37 Losses=42 WR=46.8% (n=79)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1287 | Aligned=787 (61.1%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.09 | Confidence≈ 0.00
- SL_TF dist: {'60': 158, '5': 208, '15': 901, '240': 20} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 296, '5': 339, '15': 476, '240': 176} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1248, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=200, 15m=878, 60m=154, 240m=16, 1440m=0
- RR plan por bandas: 0-10≈ 2.07 (n=1248), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10382 | Zonas con Anchors: 10368
- Dir zonas (zona): Bull=3703 Bear=6380 Neutral=299
- Resumen por ciclo (promedios): TotHZ≈ 4.2, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.6, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 10016, 'tie-bias': 352, 'triggers-only': 14}
- TF Triggers: {'5': 5492, '15': 4890}
- TF Anchors: {'60': 10294, '240': 6073, '1440': 171}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 27, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,42': 1, 'score decayó a 0,32': 1, 'score decayó a 0,34': 1, 'score decayó a 0,22': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 196 | Ejecutadas: 35 | Canceladas: 0 | Expiradas: 0
- BUY: 62 | SELL: 169

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 826
- Registered: 103
  - DEDUP_COOLDOWN: 11 | DEDUP_IDENTICAL: 92 | SKIP_CONCURRENCY: 102
- Intentos de registro: 308

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 37.3%
- RegRate = Registered / Intentos = 33.4%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 33.4%
- Concurrency = SKIP_CONCURRENCY / Intentos = 33.1%
- ExecRate = Ejecutadas / Registered = 34.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5492 | Total candidatos: 43136 | Seleccionados: 0
- Candidatos por zona (promedio): 7.9

### Take Profit (TP)
- Zonas analizadas: 5392 | Total candidatos: 50570 | Seleccionados: 5392
- Candidatos por zona (promedio): 9.4
- **Edad (barras)** - Candidatos: med=38, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 50570}
- **Priority Seleccionados**: {'P3': 3695, 'NA': 1379, 'P0': 318}
- **Type Candidatos**: {'Swing': 50570}
- **Type Seleccionados**: {'P3_Swing': 3695, 'P4_Fallback': 1379, 'P0_Zone': 318}
- **TF Candidatos**: {5: 15619, 15: 14029, 60: 13371, 240: 7551}
- **TF Seleccionados**: {60: 1033, 5: 1023, 15: 1227, -1: 1379, 240: 730}
- **DistATR** - Candidatos: avg=8.2 | Seleccionados: avg=5.6
- **RR** - Candidatos: avg=3.43 | Seleccionados: avg=1.32
- **Razones de selección**: {'BestIntelligentScore': 5392}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.