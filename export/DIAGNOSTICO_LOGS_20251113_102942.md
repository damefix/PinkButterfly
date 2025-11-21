# Informe Diagnóstico de Logs - 2025-11-13 10:33:50

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_102942.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_102942.csv`

## DFM
- Eventos de evaluación: 919
- Evaluaciones Bull: 154 | Bear: 659
- Pasaron umbral (PassedThreshold): 813
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:72, 6:347, 7:353, 8:41, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2351
- KeptAligned: 4112/4112 | KeptCounter: 2766/2869
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.438 | AvgProxCounter≈ 0.234
  - AvgDistATRAligned≈ 1.50 | AvgDistATRCounter≈ 1.15
- PreferAligned eventos: 1270 | Filtradas contra-bias: 579

### Proximity (Pre-PreferAligned)
- Eventos: 2351
- Aligned pre: 4112/6878 | Counter pre: 2766/6878
- AvgProxAligned(pre)≈ 0.438 | AvgDistATRAligned(pre)≈ 1.50

### Proximity Drivers
- Eventos: 2351
- Alineadas: n=4112 | BaseProx≈ 0.754 | ZoneATR≈ 5.20 | SizePenalty≈ 0.975 | FinalProx≈ 0.736
- Contra-bias: n=2187 | BaseProx≈ 0.526 | ZoneATR≈ 4.76 | SizePenalty≈ 0.979 | FinalProx≈ 0.517

## Risk
- Eventos: 1951
- Accepted=1253 | RejSL=0 | RejTP=0 | RejRR=1310 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 310 (11.0% del total)
  - Avg Score: 0.38 | Avg R:R: 1.89 | Avg DistATR: 3.74
  - Por TF: TF5=87, TF15=223
- **P0_SWING_LITE:** 2498 (89.0% del total)
  - Avg Score: 0.57 | Avg R:R: 4.13 | Avg DistATR: 3.53
  - Por TF: TF15=535, TF60=1963


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 98 | Unmatched: 1186
- 0-10: Wins=21 Losses=77 WR=21.4% (n=98)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=21 Losses=77 WR=21.4% (n=98)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1284 | Aligned=781 (60.8%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.07 | Confidence≈ 0.00
- SL_TF dist: {'60': 160, '15': 927, '5': 178, '240': 19} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 305, '5': 344, '15': 478, '240': 157} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1253, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=173, 15m=908, 60m=156, 240m=16, 1440m=0
- RR plan por bandas: 0-10≈ 2.05 (n=1253), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10290 | Zonas con Anchors: 10278
- Dir zonas (zona): Bull=3743 Bear=6228 Neutral=319
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9905, 'tie-bias': 373, 'triggers-only': 12}
- TF Triggers: {'5': 5453, '15': 4837}
- TF Anchors: {'60': 10199, '240': 5976, '1440': 445}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 24, 'score decayó a 0,46': 1, 'score decayó a 0,28': 1, 'score decayó a 0,21': 1, 'score decayó a 0,20': 1, 'score decayó a 0,42': 1, 'score decayó a 0,40': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 219 | Ejecutadas: 43 | Canceladas: 0 | Expiradas: 0
- BUY: 76 | SELL: 186

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 813
- Registered: 114
  - DEDUP_COOLDOWN: 14 | DEDUP_IDENTICAL: 103 | SKIP_CONCURRENCY: 80
- Intentos de registro: 311

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 38.3%
- RegRate = Registered / Intentos = 36.7%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 37.6%
- Concurrency = SKIP_CONCURRENCY / Intentos = 25.7%
- ExecRate = Ejecutadas / Registered = 37.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5423 | Total candidatos: 42802 | Seleccionados: 0
- Candidatos por zona (promedio): 7.9

### Take Profit (TP)
- Zonas analizadas: 5323 | Total candidatos: 52180 | Seleccionados: 5323
- Candidatos por zona (promedio): 9.8
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 52180}
- **Priority Seleccionados**: {'P3': 3644, 'NA': 1371, 'P0': 308}
- **Type Candidatos**: {'Swing': 52180}
- **Type Seleccionados**: {'P3_Swing': 3644, 'P4_Fallback': 1371, 'P0_Zone': 308}
- **TF Candidatos**: {5: 15654, 15: 14103, 60: 13835, 240: 8588}
- **TF Seleccionados**: {60: 1004, -1: 1371, 5: 989, 15: 1243, 240: 716}
- **DistATR** - Candidatos: avg=8.5 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.43 | Seleccionados: avg=1.29
- **Razones de selección**: {'BestIntelligentScore': 5323}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.