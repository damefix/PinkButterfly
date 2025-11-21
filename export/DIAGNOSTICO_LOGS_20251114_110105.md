# Informe Diagnóstico de Logs - 2025-11-14 11:06:17

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_110105.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251114_110105.csv`

## DFM
- Eventos de evaluación: 895
- Evaluaciones Bull: 105 | Bear: 654
- Pasaron umbral (PassedThreshold): 759
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:47, 6:318, 7:319, 8:75, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2352
- KeptAligned: 3141/3141 | KeptCounter: 3308/3451
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.347 | AvgProxCounter≈ 0.259
  - AvgDistATRAligned≈ 1.24 | AvgDistATRCounter≈ 1.35
- PreferAligned eventos: 1030 | Filtradas contra-bias: 519

### Proximity (Pre-PreferAligned)
- Eventos: 2352
- Aligned pre: 3141/6449 | Counter pre: 3308/6449
- AvgProxAligned(pre)≈ 0.347 | AvgDistATRAligned(pre)≈ 1.24

### Proximity Drivers
- Eventos: 2352
- Alineadas: n=3141 | BaseProx≈ 0.758 | ZoneATR≈ 5.03 | SizePenalty≈ 0.977 | FinalProx≈ 0.741
- Contra-bias: n=2789 | BaseProx≈ 0.506 | ZoneATR≈ 4.94 | SizePenalty≈ 0.977 | FinalProx≈ 0.496

## Risk
- Eventos: 1929
- Accepted=1210 | RejSL=0 | RejTP=0 | RejRR=1259 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 305 (10.8% del total)
  - Avg Score: 0.39 | Avg R:R: 1.94 | Avg DistATR: 3.79
  - Por TF: TF5=91, TF15=214
- **P0_SWING_LITE:** 2529 (89.2% del total)
  - Avg Score: 0.54 | Avg R:R: 4.86 | Avg DistATR: 3.66
  - Por TF: TF15=481, TF60=2048


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 77 | Unmatched: 1176
- 0-10: Wins=28 Losses=49 WR=36.4% (n=77)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=28 Losses=49 WR=36.4% (n=77)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1253 | Aligned=638 (50.9%)
- Core≈ 1.00 | Prox≈ 0.64 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.05 | Confidence≈ 0.00
- SL_TF dist: {'5': 187, '60': 167, '15': 865, '240': 34} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 257, '15': 428, '5': 293, '240': 275} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1210, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=181, 15m=841, 60m=160, 240m=28, 1440m=0
- RR plan por bandas: 0-10≈ 2.04 (n=1210), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10410 | Zonas con Anchors: 10402
- Dir zonas (zona): Bull=2979 Bear=7110 Neutral=321
- Resumen por ciclo (promedios): TotHZ≈ 4.2, WithAnchors≈ 4.2, DirBull≈ 1.2, DirBear≈ 2.8, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 10000, 'tie-bias': 402, 'triggers-only': 8}
- TF Triggers: {'5': 5496, '15': 4914}
- TF Anchors: {'60': 10327, '240': 9820, '1440': 8433}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 15, 'score decayó a 0,21': 2, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,22': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 171 | Ejecutadas: 42 | Canceladas: 0 | Expiradas: 0
- BUY: 46 | SELL: 167

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 759
- Registered: 90
  - DEDUP_COOLDOWN: 14 | DEDUP_IDENTICAL: 68 | SKIP_CONCURRENCY: 106
- Intentos de registro: 278

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 36.6%
- RegRate = Registered / Intentos = 32.4%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 29.5%
- Concurrency = SKIP_CONCURRENCY / Intentos = 38.1%
- ExecRate = Ejecutadas / Registered = 46.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4946 | Total candidatos: 40369 | Seleccionados: 0
- Candidatos por zona (promedio): 8.2

### Take Profit (TP)
- Zonas analizadas: 4873 | Total candidatos: 76225 | Seleccionados: 4873
- Candidatos por zona (promedio): 15.6
- **Edad (barras)** - Candidatos: med=36, max=192 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.50 | Seleccionados: avg=0.69
- **Priority Candidatos**: {'P3': 76225}
- **Priority Seleccionados**: {'NA': 1316, 'P3': 3346, 'P0': 211}
- **Type Candidatos**: {'Swing': 76225}
- **Type Seleccionados**: {'P4_Fallback': 1316, 'P3_Swing': 3346, 'P0_Zone': 211}
- **TF Candidatos**: {240: 33894, 60: 15212, 5: 13608, 15: 13511}
- **TF Seleccionados**: {-1: 1316, 60: 692, 15: 1029, 5: 831, 240: 1005}
- **DistATR** - Candidatos: avg=14.6 | Seleccionados: avg=5.2
- **RR** - Candidatos: avg=6.48 | Seleccionados: avg=1.39
- **Razones de selección**: {'BestIntelligentScore': 4873}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.