# Informe Diagnóstico de Logs - 2025-11-12 16:22:57

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_162002.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251112_162002.csv`

## DFM
- Eventos de evaluación: 771
- Evaluaciones Bull: 319 | Bear: 654
- Pasaron umbral (PassedThreshold): 895
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:16, 4:37, 5:151, 6:382, 7:334, 8:53, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2346
- KeptAligned: 2844/2844 | KeptCounter: 1885/1885
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.418 | AvgProxCounter≈ 0.249
  - AvgDistATRAligned≈ 0.87 | AvgDistATRCounter≈ 0.64
- PreferAligned eventos: 1089 | Filtradas contra-bias: 361

### Proximity (Pre-PreferAligned)
- Eventos: 2346
- Aligned pre: 2844/4729 | Counter pre: 1885/4729
- AvgProxAligned(pre)≈ 0.418 | AvgDistATRAligned(pre)≈ 0.87

### Proximity Drivers
- Eventos: 2346
- Alineadas: n=2844 | BaseProx≈ 0.832 | ZoneATR≈ 4.97 | SizePenalty≈ 0.978 | FinalProx≈ 0.814
- Contra-bias: n=1524 | BaseProx≈ 0.671 | ZoneATR≈ 4.81 | SizePenalty≈ 0.979 | FinalProx≈ 0.658

## Risk
- Eventos: 1713
- Accepted=973 | RejSL=0 | RejTP=0 | RejRR=1025 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 206 (9.9% del total)
  - Avg Score: 0.41 | Avg R:R: 1.95 | Avg DistATR: 3.74
  - Por TF: TF5=59, TF15=147
- **P0_SWING_LITE:** 1885 (90.1% del total)
  - Avg Score: 0.57 | Avg R:R: 4.17 | Avg DistATR: 3.47
  - Por TF: TF15=413, TF60=1472


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 103 | Unmatched: 885
- 0-10: Wins=21 Losses=82 WR=20.4% (n=103)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=21 Losses=82 WR=20.4% (n=103)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 988 | Aligned=573 (58.0%)
- Core≈ 1.00 | Prox≈ 0.76 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.00 | Confidence≈ 0.00
- SL_TF dist: {'5': 122, '60': 100, '15': 762, '240': 4} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 218, '5': 261, '15': 391, '240': 118} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=973, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=120, 15m=751, 60m=98, 240m=4, 1440m=0
- RR plan por bandas: 0-10≈ 2.01 (n=973), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10191 | Zonas con Anchors: 10177
- Dir zonas (zona): Bull=3841 Bear=5988 Neutral=362
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.4, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9753, 'tie-bias': 424, 'triggers-only': 14}
- TF Triggers: {'5': 5462, '15': 4729}
- TF Anchors: {'60': 10104, '240': 5904, '1440': 720}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 17, 'score decayó a 0,27': 1, 'score decayó a 0,42': 2, 'score decayó a 0,40': 1, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 194 | Ejecutadas: 45 | Canceladas: 0 | Expiradas: 0
- BUY: 125 | SELL: 114

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 895
- Registered: 103
  - DEDUP_COOLDOWN: 8 | DEDUP_IDENTICAL: 89 | SKIP_CONCURRENCY: 89
- Intentos de registro: 289

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 32.3%
- RegRate = Registered / Intentos = 35.6%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 33.6%
- Concurrency = SKIP_CONCURRENCY / Intentos = 30.8%
- ExecRate = Ejecutadas / Registered = 43.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 3857 | Total candidatos: 32790 | Seleccionados: 0
- Candidatos por zona (promedio): 8.5

### Take Profit (TP)
- Zonas analizadas: 3796 | Total candidatos: 35004 | Seleccionados: 3796
- Candidatos por zona (promedio): 9.2
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.69
- **Priority Candidatos**: {'P3': 35004}
- **Priority Seleccionados**: {'P3': 2574, 'NA': 958, 'P0': 264}
- **Type Candidatos**: {'Swing': 35004}
- **Type Seleccionados**: {'P3_Swing': 2574, 'P4_Fallback': 958, 'P0_Zone': 264}
- **TF Candidatos**: {5: 10117, 60: 9568, 15: 9180, 240: 6139}
- **TF Seleccionados**: {60: 773, 5: 711, 15: 872, -1: 958, 240: 482}
- **DistATR** - Candidatos: avg=8.8 | Seleccionados: avg=5.0
- **RR** - Candidatos: avg=3.52 | Seleccionados: avg=1.32
- **Razones de selección**: {'BestIntelligentScore': 3796}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- KeptAligned ratio≈ 1.00.