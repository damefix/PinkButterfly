# Informe Diagnóstico de Logs - 2025-11-14 08:05:05

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_075955.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251114_075955.csv`

## DFM
- Eventos de evaluación: 922
- Evaluaciones Bull: 137 | Bear: 683
- Pasaron umbral (PassedThreshold): 820
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:64, 6:348, 7:346, 8:62, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2356
- KeptAligned: 4202/4202 | KeptCounter: 2715/2813
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.450 | AvgProxCounter≈ 0.222
  - AvgDistATRAligned≈ 1.56 | AvgDistATRCounter≈ 1.10
- PreferAligned eventos: 1297 | Filtradas contra-bias: 553

### Proximity (Pre-PreferAligned)
- Eventos: 2356
- Aligned pre: 4202/6917 | Counter pre: 2715/6917
- AvgProxAligned(pre)≈ 0.450 | AvgDistATRAligned(pre)≈ 1.56

### Proximity Drivers
- Eventos: 2356
- Alineadas: n=4202 | BaseProx≈ 0.750 | ZoneATR≈ 5.16 | SizePenalty≈ 0.975 | FinalProx≈ 0.732
- Contra-bias: n=2162 | BaseProx≈ 0.521 | ZoneATR≈ 4.86 | SizePenalty≈ 0.978 | FinalProx≈ 0.511

## Risk
- Eventos: 1955
- Accepted=1244 | RejSL=0 | RejTP=0 | RejRR=1293 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 333 (11.7% del total)
  - Avg Score: 0.39 | Avg R:R: 1.89 | Avg DistATR: 3.79
  - Por TF: TF5=87, TF15=246
- **P0_SWING_LITE:** 2515 (88.3% del total)
  - Avg Score: 0.59 | Avg R:R: 4.12 | Avg DistATR: 3.51
  - Por TF: TF15=576, TF60=1939


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 76 | Unmatched: 1207
- 0-10: Wins=37 Losses=39 WR=48.7% (n=76)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=37 Losses=39 WR=48.7% (n=76)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1283 | Aligned=782 (61.0%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.10 | Confidence≈ 0.00
- SL_TF dist: {'60': 159, '5': 205, '15': 899, '240': 20} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 291, '5': 338, '15': 476, '240': 178} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1244, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=197, 15m=876, 60m=155, 240m=16, 1440m=0
- RR plan por bandas: 0-10≈ 2.08 (n=1244), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10389 | Zonas con Anchors: 10375
- Dir zonas (zona): Bull=3707 Bear=6381 Neutral=301
- Resumen por ciclo (promedios): TotHZ≈ 4.2, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.6, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 10021, 'tie-bias': 354, 'triggers-only': 14}
- TF Triggers: {'5': 5491, '15': 4898}
- TF Anchors: {'60': 10301, '240': 6080, '1440': 176}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 27, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,42': 1, 'score decayó a 0,32': 1, 'score decayó a 0,34': 1, 'score decayó a 0,22': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 198 | Ejecutadas: 35 | Canceladas: 0 | Expiradas: 0
- BUY: 62 | SELL: 171

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 820
- Registered: 104
  - DEDUP_COOLDOWN: 11 | DEDUP_IDENTICAL: 94 | SKIP_CONCURRENCY: 100
- Intentos de registro: 309

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 37.7%
- RegRate = Registered / Intentos = 33.7%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 34.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 32.4%
- ExecRate = Ejecutadas / Registered = 33.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5492 | Total candidatos: 43093 | Seleccionados: 0
- Candidatos por zona (promedio): 7.8

### Take Profit (TP)
- Zonas analizadas: 5391 | Total candidatos: 50550 | Seleccionados: 5391
- Candidatos por zona (promedio): 9.4
- **Edad (barras)** - Candidatos: med=38, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 50550}
- **Priority Seleccionados**: {'P3': 3688, 'NA': 1387, 'P0': 316}
- **Type Candidatos**: {'Swing': 50550}
- **Type Seleccionados**: {'P3_Swing': 3688, 'P4_Fallback': 1387, 'P0_Zone': 316}
- **TF Candidatos**: {5: 15611, 15: 14018, 60: 13370, 240: 7551}
- **TF Seleccionados**: {60: 1031, 5: 1013, -1: 1387, 15: 1227, 240: 733}
- **DistATR** - Candidatos: avg=8.2 | Seleccionados: avg=5.6
- **RR** - Candidatos: avg=3.47 | Seleccionados: avg=1.33
- **Razones de selección**: {'BestIntelligentScore': 5391}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.