# Informe Diagnóstico de Logs - 2025-11-13 09:48:03

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_094410.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_094410.csv`

## DFM
- Eventos de evaluación: 939
- Evaluaciones Bull: 163 | Bear: 683
- Pasaron umbral (PassedThreshold): 846
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:72, 6:372, 7:352, 8:50, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2356
- KeptAligned: 4165/4165 | KeptCounter: 2722/2828
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.448 | AvgProxCounter≈ 0.231
  - AvgDistATRAligned≈ 1.54 | AvgDistATRCounter≈ 1.12
- PreferAligned eventos: 1289 | Filtradas contra-bias: 579

### Proximity (Pre-PreferAligned)
- Eventos: 2356
- Aligned pre: 4165/6887 | Counter pre: 2722/6887
- AvgProxAligned(pre)≈ 0.448 | AvgDistATRAligned(pre)≈ 1.54

### Proximity Drivers
- Eventos: 2356
- Alineadas: n=4165 | BaseProx≈ 0.751 | ZoneATR≈ 5.18 | SizePenalty≈ 0.975 | FinalProx≈ 0.733
- Contra-bias: n=2143 | BaseProx≈ 0.528 | ZoneATR≈ 4.85 | SizePenalty≈ 0.978 | FinalProx≈ 0.518

## Risk
- Eventos: 1948
- Accepted=1267 | RejSL=0 | RejTP=0 | RejRR=1251 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 308 (10.7% del total)
  - Avg Score: 0.39 | Avg R:R: 1.89 | Avg DistATR: 3.74
  - Por TF: TF5=76, TF15=232
- **P0_SWING_LITE:** 2568 (89.3% del total)
  - Avg Score: 0.57 | Avg R:R: 4.12 | Avg DistATR: 3.47
  - Por TF: TF15=544, TF60=2024


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 82 | Unmatched: 1224
- 0-10: Wins=37 Losses=45 WR=45.1% (n=82)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=37 Losses=45 WR=45.1% (n=82)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1306 | Aligned=811 (62.1%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.08 | Confidence≈ 0.00
- SL_TF dist: {'60': 150, '15': 950, '5': 195, '240': 11} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 289, '15': 494, '5': 352, '240': 171} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1267, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=187, 15m=927, 60m=146, 240m=7, 1440m=0
- RR plan por bandas: 0-10≈ 2.06 (n=1267), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10327 | Zonas con Anchors: 10313
- Dir zonas (zona): Bull=3782 Bear=6211 Neutral=334
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9913, 'tie-bias': 400, 'triggers-only': 14}
- TF Triggers: {'5': 5457, '15': 4870}
- TF Anchors: {'60': 10239, '240': 5975, '1440': 457}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,46': 1, 'estructura no existe': 26, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,42': 1, 'score decayó a 0,32': 1, 'score decayó a 0,34': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 198 | Ejecutadas: 35 | Canceladas: 0 | Expiradas: 0
- BUY: 71 | SELL: 162

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 846
- Registered: 104
  - DEDUP_COOLDOWN: 21 | DEDUP_IDENTICAL: 100 | SKIP_CONCURRENCY: 101
- Intentos de registro: 326

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 38.5%
- RegRate = Registered / Intentos = 31.9%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 37.1%
- Concurrency = SKIP_CONCURRENCY / Intentos = 31.0%
- ExecRate = Ejecutadas / Registered = 33.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5450 | Total candidatos: 43149 | Seleccionados: 0
- Candidatos por zona (promedio): 7.9

### Take Profit (TP)
- Zonas analizadas: 5346 | Total candidatos: 51318 | Seleccionados: 5346
- Candidatos por zona (promedio): 9.6
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 51318}
- **Priority Seleccionados**: {'P3': 3655, 'NA': 1374, 'P0': 317}
- **Type Candidatos**: {'Swing': 51318}
- **Type Seleccionados**: {'P3_Swing': 3655, 'P4_Fallback': 1374, 'P0_Zone': 317}
- **TF Candidatos**: {5: 15459, 15: 13975, 60: 13658, 240: 8226}
- **TF Seleccionados**: {60: 1005, 5: 1014, -1: 1374, 15: 1246, 240: 707}
- **DistATR** - Candidatos: avg=8.6 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.53 | Seleccionados: avg=1.31
- **Razones de selección**: {'BestIntelligentScore': 5346}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.