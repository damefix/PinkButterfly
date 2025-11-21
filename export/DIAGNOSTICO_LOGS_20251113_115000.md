# Informe Diagnóstico de Logs - 2025-11-13 11:55:13

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_115000.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_115000.csv`

## DFM
- Eventos de evaluación: 948
- Evaluaciones Bull: 164 | Bear: 680
- Pasaron umbral (PassedThreshold): 844
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:71, 6:365, 7:359, 8:49, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2356
- KeptAligned: 4161/4161 | KeptCounter: 2737/2843
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.444 | AvgProxCounter≈ 0.230
  - AvgDistATRAligned≈ 1.51 | AvgDistATRCounter≈ 1.15
- PreferAligned eventos: 1279 | Filtradas contra-bias: 564

### Proximity (Pre-PreferAligned)
- Eventos: 2356
- Aligned pre: 4161/6898 | Counter pre: 2737/6898
- AvgProxAligned(pre)≈ 0.444 | AvgDistATRAligned(pre)≈ 1.51

### Proximity Drivers
- Eventos: 2356
- Alineadas: n=4161 | BaseProx≈ 0.752 | ZoneATR≈ 5.17 | SizePenalty≈ 0.975 | FinalProx≈ 0.734
- Contra-bias: n=2173 | BaseProx≈ 0.525 | ZoneATR≈ 4.85 | SizePenalty≈ 0.978 | FinalProx≈ 0.515

## Risk
- Eventos: 1947
- Accepted=1271 | RejSL=0 | RejTP=0 | RejRR=1251 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 314 (11.0% del total)
  - Avg Score: 0.39 | Avg R:R: 1.89 | Avg DistATR: 3.75
  - Por TF: TF5=79, TF15=235
- **P0_SWING_LITE:** 2546 (89.0% del total)
  - Avg Score: 0.58 | Avg R:R: 4.13 | Avg DistATR: 3.48
  - Por TF: TF15=546, TF60=2000


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 91 | Unmatched: 1218
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
- Muestras: 1309 | Aligned=811 (62.0%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.07 | Confidence≈ 0.00
- SL_TF dist: {'60': 148, '5': 193, '15': 955, '240': 13} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 294, '5': 352, '15': 487, '240': 176} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1271, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=185, 15m=933, 60m=144, 240m=9, 1440m=0
- RR plan por bandas: 0-10≈ 2.05 (n=1271), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10326 | Zonas con Anchors: 10312
- Dir zonas (zona): Bull=3792 Bear=6202 Neutral=332
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9916, 'tie-bias': 396, 'triggers-only': 14}
- TF Triggers: {'5': 5451, '15': 4875}
- TF Anchors: {'60': 10238, '240': 6013, '1440': 484}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,46': 1, 'estructura no existe': 25, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,42': 1, 'score decayó a 0,32': 1, 'score decayó a 0,34': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 196 | Ejecutadas: 35 | Canceladas: 0 | Expiradas: 0
- BUY: 70 | SELL: 161

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 844
- Registered: 103
  - DEDUP_COOLDOWN: 20 | DEDUP_IDENTICAL: 103 | SKIP_CONCURRENCY: 99
- Intentos de registro: 325

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 38.5%
- RegRate = Registered / Intentos = 31.7%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 37.8%
- Concurrency = SKIP_CONCURRENCY / Intentos = 30.5%
- ExecRate = Ejecutadas / Registered = 34.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5441 | Total candidatos: 43049 | Seleccionados: 0
- Candidatos por zona (promedio): 7.9

### Take Profit (TP)
- Zonas analizadas: 5332 | Total candidatos: 51310 | Seleccionados: 5332
- Candidatos por zona (promedio): 9.6
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 51310}
- **Priority Seleccionados**: {'NA': 1365, 'P3': 3651, 'P0': 316}
- **Type Candidatos**: {'Swing': 51310}
- **Type Seleccionados**: {'P4_Fallback': 1365, 'P3_Swing': 3651, 'P0_Zone': 316}
- **TF Candidatos**: {5: 15384, 15: 13953, 60: 13611, 240: 8362}
- **TF Seleccionados**: {-1: 1365, 60: 994, 5: 1020, 15: 1246, 240: 707}
- **DistATR** - Candidatos: avg=8.6 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.55 | Seleccionados: avg=1.32
- **Razones de selección**: {'BestIntelligentScore': 5332}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.