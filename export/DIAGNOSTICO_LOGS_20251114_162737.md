# Informe Diagnóstico de Logs - 2025-11-14 16:34:27

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_162737.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251114_162737.csv`

## DFM
- Eventos de evaluación: 941
- Evaluaciones Bull: 116 | Bear: 736
- Pasaron umbral (PassedThreshold): 852
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:51, 6:347, 7:356, 8:98, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2372
- KeptAligned: 3640/3640 | KeptCounter: 2938/3069
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.380 | AvgProxCounter≈ 0.237
  - AvgDistATRAligned≈ 1.40 | AvgDistATRCounter≈ 1.29
- PreferAligned eventos: 1136 | Filtradas contra-bias: 463

### Proximity (Pre-PreferAligned)
- Eventos: 2372
- Aligned pre: 3640/6578 | Counter pre: 2938/6578
- AvgProxAligned(pre)≈ 0.380 | AvgDistATRAligned(pre)≈ 1.40

### Proximity Drivers
- Eventos: 2372
- Alineadas: n=3640 | BaseProx≈ 0.753 | ZoneATR≈ 5.04 | SizePenalty≈ 0.977 | FinalProx≈ 0.736
- Contra-bias: n=2475 | BaseProx≈ 0.493 | ZoneATR≈ 4.96 | SizePenalty≈ 0.976 | FinalProx≈ 0.483

## Risk
- Eventos: 1961
- Accepted=1277 | RejSL=0 | RejTP=0 | RejRR=1333 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 329 (11.5% del total)
  - Avg Score: 0.39 | Avg R:R: 1.92 | Avg DistATR: 3.83
  - Por TF: TF5=115, TF15=214
- **P0_SWING_LITE:** 2533 (88.5% del total)
  - Avg Score: 0.59 | Avg R:R: 4.58 | Avg DistATR: 3.51
  - Por TF: TF15=533, TF60=2000


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 78 | Unmatched: 1230
- 0-10: Wins=34 Losses=44 WR=43.6% (n=78)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=34 Losses=44 WR=43.6% (n=78)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1308 | Aligned=756 (57.8%)
- Core≈ 1.00 | Prox≈ 0.64 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.08 | Confidence≈ 0.00
- SL_TF dist: {'15': 944, '60': 165, '5': 170, '240': 29} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 236, '5': 318, '15': 433, '240': 321} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1277, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=165, 15m=925, 60m=163, 240m=24, 1440m=0
- RR plan por bandas: 0-10≈ 2.05 (n=1277), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10471 | Zonas con Anchors: 10462
- Dir zonas (zona): Bull=2891 Bear=7212 Neutral=368
- Resumen por ciclo (promedios): TotHZ≈ 4.2, WithAnchors≈ 4.2, DirBull≈ 1.2, DirBear≈ 2.9, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9989, 'tie-bias': 473, 'triggers-only': 9}
- TF Triggers: {'15': 4944, '5': 5527}
- TF Anchors: {'60': 10368, '240': 9813, '1440': 8581}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 22, 'score decayó a 0,21': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,27': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 191 | Ejecutadas: 46 | Canceladas: 0 | Expiradas: 0
- BUY: 54 | SELL: 183

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 852
- Registered: 101
  - DEDUP_COOLDOWN: 16 | DEDUP_IDENTICAL: 90 | SKIP_CONCURRENCY: 103
- Intentos de registro: 310

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 36.4%
- RegRate = Registered / Intentos = 32.6%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 34.2%
- Concurrency = SKIP_CONCURRENCY / Intentos = 33.2%
- ExecRate = Ejecutadas / Registered = 45.5%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5184 | Total candidatos: 47561 | Seleccionados: 0
- Candidatos por zona (promedio): 9.2

### Take Profit (TP)
- Zonas analizadas: 5113 | Total candidatos: 92302 | Seleccionados: 5113
- Candidatos por zona (promedio): 18.1
- **Edad (barras)** - Candidatos: med=35, max=182 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.69
- **Priority Candidatos**: {'P3': 92302}
- **Priority Seleccionados**: {'P3': 3612, 'NA': 1273, 'P0': 228}
- **Type Candidatos**: {'Swing': 92302}
- **Type Seleccionados**: {'P3_Swing': 3612, 'P4_Fallback': 1273, 'P0_Zone': 228}
- **TF Candidatos**: {240: 34950, 60: 23959, 5: 19003, 15: 14390}
- **TF Seleccionados**: {60: 642, -1: 1273, 5: 955, 15: 1110, 240: 1133}
- **DistATR** - Candidatos: avg=13.2 | Seleccionados: avg=5.3
- **RR** - Candidatos: avg=5.84 | Seleccionados: avg=1.38
- **Razones de selección**: {'BestIntelligentScore': 5113}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.