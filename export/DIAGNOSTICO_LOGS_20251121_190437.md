# Informe Diagnóstico de Logs - 2025-11-21 19:19:55

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251121_190437.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251121_190437.csv`

## DFM
- Eventos de evaluación: 430
- Evaluaciones Bull: 0 | Bear: 551
- Pasaron umbral (PassedThreshold): 551
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:4, 6:181, 7:255, 8:111, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3127
- KeptAligned: 4382/4382 | KeptCounter: 4763/5101
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.287 | AvgProxCounter≈ 0.239
  - AvgDistATRAligned≈ 1.12 | AvgDistATRCounter≈ 1.36
- PreferAligned eventos: 1090 | Filtradas contra-bias: 760

### Proximity (Pre-PreferAligned)
- Eventos: 3127
- Aligned pre: 4382/9145 | Counter pre: 4763/9145
- AvgProxAligned(pre)≈ 0.287 | AvgDistATRAligned(pre)≈ 1.12

### Proximity Drivers
- Eventos: 3127
- Alineadas: n=4382 | BaseProx≈ 0.749 | ZoneATR≈ 4.58 | SizePenalty≈ 0.980 | FinalProx≈ 0.734
- Contra-bias: n=4003 | BaseProx≈ 0.498 | ZoneATR≈ 5.56 | SizePenalty≈ 0.965 | FinalProx≈ 0.481

## Risk
- Eventos: 2279
- Accepted=731 | RejSL=0 | RejTP=0 | RejRR=935 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 1021 (26.8% del total)
  - Avg Score: 0.39 | Avg R:R: 1.78 | Avg DistATR: 3.84
  - Por TF: TF5=259, TF15=762
- **P0_SWING_LITE:** 2794 (73.2% del total)
  - Avg Score: 0.84 | Avg R:R: 3.38 | Avg DistATR: 4.01
  - Por TF: TF15=244, TF60=2550


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 176 | Unmatched: 555
- 0-10: Wins=175 Losses=1 WR=99.4% (n=176)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=175 Losses=1 WR=99.4% (n=176)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 731 | Aligned=439 (60.1%)
- Core≈ 1.00 | Prox≈ 0.70 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.09 | Confidence≈ 0.00
- SL_TF dist: {'15': 592, '5': 139} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 420, '60': 124, '240': 81, '5': 106} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=731, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=139, 15m=592, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.09 (n=731), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 49556 | Zonas con Anchors: 49556
- Dir zonas (zona): Bull=4503 Bear=42976 Neutral=2077
- Resumen por ciclo (promedios): TotHZ≈ 15.8, WithAnchors≈ 15.8, DirBull≈ 1.4, DirBear≈ 13.7, DirNeutral≈ 0.7
- Razones de dirección: {'anchors+triggers': 46540, 'tie-bias': 3016}
- TF Triggers: {'5': 31824, '15': 17732}
- TF Anchors: {'60': 49556, '240': 49556, '1440': 49556}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## Timing y Confluencia HTF
- [TIMING_ADAPT] eventos: 206 | Approach rejects: 181
- Score/Req promedio: 2.36/2.00
- [HTF_CONFL] muestras: 1254 | ok=1254 | rejects=0
- median≈ 0.123 | thr≈ 0.116
- [BIAS_FAST] muestras: 2091 | Bull=156 Bear=1791 Neutral=144 | rejects=3
- score promedio: -0.69
- [HTF_CONFL] muestras: 1254 | ok=1254 | rejects=0
- median≈ 0.123 | thr≈ 0.116
- [RC_SL_MINFLOOR] filtrados: 0

## TradeManager - Razones (desde log)
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 65 | Ejecutadas: 14 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 79

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 551
- Registered: 33
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 33

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 6.0%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 42.4%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7876 | Total candidatos: 217294 | Seleccionados: 190
- Candidatos por zona (promedio): 27.6

### Take Profit (TP)
- Zonas analizadas: 7876 | Total candidatos: 315674 | Seleccionados: 7876
- Candidatos por zona (promedio): 40.1
- **Edad (barras)** - Candidatos: med=1079, max=8001 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.81 | Seleccionados: avg=0.68
- **Priority Candidatos**: {'P3': 180531}
- **Priority Seleccionados**: {'P3': 6583, 'NA': 942, 'P0': 351}
- **Type Candidatos**: {'Swing': 180531}
- **Type Seleccionados**: {'P3_Swing': 6583, 'P4_Fallback': 942, 'P0_Zone': 351}
- **TF Candidatos**: {5: 86043, 15: 51060, 240: 23486, 60: 19942}
- **TF Seleccionados**: {5: 3665, 240: 914, 60: 651, 15: 1704, -1: 942}
- **DistATR** - Candidatos: avg=30.5 | Seleccionados: avg=13.1
- **RR** - Candidatos: avg=7.52 | Seleccionados: avg=1.33
- **Razones de selección**: {'BestIntelligentScore': 7876}

### 🎯 Recomendaciones
- ⚠️ SL: 100% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.