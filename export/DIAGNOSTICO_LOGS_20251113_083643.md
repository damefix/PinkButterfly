# Informe Diagnóstico de Logs - 2025-11-13 08:40:31

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_083643.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_083643.csv`

## DFM
- Eventos de evaluación: 942
- Evaluaciones Bull: 167 | Bear: 683
- Pasaron umbral (PassedThreshold): 850
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:74, 6:372, 7:354, 8:50, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2355
- KeptAligned: 4159/4159 | KeptCounter: 2725/2831
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.446 | AvgProxCounter≈ 0.230
  - AvgDistATRAligned≈ 1.52 | AvgDistATRCounter≈ 1.13
- PreferAligned eventos: 1282 | Filtradas contra-bias: 581

### Proximity (Pre-PreferAligned)
- Eventos: 2355
- Aligned pre: 4159/6884 | Counter pre: 2725/6884
- AvgProxAligned(pre)≈ 0.446 | AvgDistATRAligned(pre)≈ 1.52

### Proximity Drivers
- Eventos: 2355
- Alineadas: n=4159 | BaseProx≈ 0.752 | ZoneATR≈ 5.18 | SizePenalty≈ 0.975 | FinalProx≈ 0.734
- Contra-bias: n=2144 | BaseProx≈ 0.527 | ZoneATR≈ 4.84 | SizePenalty≈ 0.978 | FinalProx≈ 0.518

## Risk
- Eventos: 1942
- Accepted=1270 | RejSL=0 | RejTP=0 | RejRR=1245 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 308 (10.7% del total)
  - Avg Score: 0.39 | Avg R:R: 1.89 | Avg DistATR: 3.74
  - Por TF: TF5=75, TF15=233
- **P0_SWING_LITE:** 2570 (89.3% del total)
  - Avg Score: 0.57 | Avg R:R: 4.18 | Avg DistATR: 3.48
  - Por TF: TF15=548, TF60=2022


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 81 | Unmatched: 1228
- 0-10: Wins=37 Losses=44 WR=45.7% (n=81)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=37 Losses=44 WR=45.7% (n=81)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1309 | Aligned=808 (61.7%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.09 | Confidence≈ 0.00
- SL_TF dist: {'60': 150, '15': 954, '5': 194, '240': 11} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 292, '5': 354, '15': 491, '240': 172} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1270, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=186, 15m=931, 60m=146, 240m=7, 1440m=0
- RR plan por bandas: 0-10≈ 2.07 (n=1270), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10317 | Zonas con Anchors: 10303
- Dir zonas (zona): Bull=3775 Bear=6202 Neutral=340
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9897, 'tie-bias': 406, 'triggers-only': 14}
- TF Triggers: {'5': 5457, '15': 4860}
- TF Anchors: {'60': 10229, '240': 5961, '1440': 445}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,46': 1, 'estructura no existe': 26, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,42': 1, 'score decayó a 0,34': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 198 | Ejecutadas: 35 | Canceladas: 0 | Expiradas: 0
- BUY: 71 | SELL: 162

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 850
- Registered: 104
  - DEDUP_COOLDOWN: 23 | DEDUP_IDENTICAL: 99 | SKIP_CONCURRENCY: 102
- Intentos de registro: 328

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 38.6%
- RegRate = Registered / Intentos = 31.7%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 37.2%
- Concurrency = SKIP_CONCURRENCY / Intentos = 31.1%
- ExecRate = Ejecutadas / Registered = 33.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5438 | Total candidatos: 43110 | Seleccionados: 0
- Candidatos por zona (promedio): 7.9

### Take Profit (TP)
- Zonas analizadas: 5333 | Total candidatos: 51129 | Seleccionados: 5333
- Candidatos por zona (promedio): 9.6
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 51129}
- **Priority Seleccionados**: {'P3': 3649, 'NA': 1367, 'P0': 317}
- **Type Candidatos**: {'Swing': 51129}
- **Type Seleccionados**: {'P3_Swing': 3649, 'P4_Fallback': 1367, 'P0_Zone': 317}
- **TF Candidatos**: {5: 15412, 15: 13912, 60: 13625, 240: 8180}
- **TF Seleccionados**: {60: 1009, 5: 1022, -1: 1367, 15: 1231, 240: 704}
- **DistATR** - Candidatos: avg=8.6 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.59 | Seleccionados: avg=1.33
- **Razones de selección**: {'BestIntelligentScore': 5333}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.