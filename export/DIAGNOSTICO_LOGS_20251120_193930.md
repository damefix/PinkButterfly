# Informe Diagnóstico de Logs - 2025-11-20 19:48:22

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251120_193930.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251120_193930.csv`

## DFM
- Eventos de evaluación: 566
- Evaluaciones Bull: 0 | Bear: 515
- Pasaron umbral (PassedThreshold): 515
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:6, 6:229, 7:225, 8:55, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3122
- KeptAligned: 2881/2881 | KeptCounter: 2967/3239
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.187 | AvgProxCounter≈ 0.169
  - AvgDistATRAligned≈ 0.82 | AvgDistATRCounter≈ 0.88
- PreferAligned eventos: 762 | Filtradas contra-bias: 107

### Proximity (Pre-PreferAligned)
- Eventos: 3122
- Aligned pre: 2881/5848 | Counter pre: 2967/5848
- AvgProxAligned(pre)≈ 0.187 | AvgDistATRAligned(pre)≈ 0.82

### Proximity Drivers
- Eventos: 3122
- Alineadas: n=2881 | BaseProx≈ 0.721 | ZoneATR≈ 3.83 | SizePenalty≈ 0.989 | FinalProx≈ 0.713
- Contra-bias: n=2860 | BaseProx≈ 0.516 | ZoneATR≈ 4.56 | SizePenalty≈ 0.980 | FinalProx≈ 0.506

## Risk
- Eventos: 1764
- Accepted=785 | RejSL=0 | RejTP=0 | RejRR=683 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 966 (31.1% del total)
  - Avg Score: 0.39 | Avg R:R: 1.79 | Avg DistATR: 3.94
  - Por TF: TF5=102, TF15=864
- **P0_SWING_LITE:** 2138 (68.9% del total)
  - Avg Score: 0.67 | Avg R:R: 3.62 | Avg DistATR: 3.88
  - Por TF: TF15=252, TF60=1886


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 2 | Unmatched: 786
- 0-10: Wins=2 Losses=0 WR=100.0% (n=2)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=2 Losses=0 WR=100.0% (n=2)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 788 | Aligned=457 (58.0%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.48 | Confidence≈ 0.00
- SL_TF dist: {'5': 252, '15': 232, '60': 301, '240': 3} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 533, '15': 97, '240': 90, '5': 68} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=785, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=249, 15m=232, 60m=301, 240m=3, 1440m=0
- RR plan por bandas: 0-10≈ 2.48 (n=785), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 26381 | Zonas con Anchors: 26381
- Dir zonas (zona): Bull=3172 Bear=23024 Neutral=185
- Resumen por ciclo (promedios): TotHZ≈ 8.4, WithAnchors≈ 8.4, DirBull≈ 1.0, DirBear≈ 7.4, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 26099, 'tie-bias': 282}
- TF Triggers: {'15': 16804, '5': 9577}
- TF Anchors: {'60': 26381, '240': 26381, '1440': 26381}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## Timing y Confluencia HTF
- [TIMING_ADAPT] eventos: 171 | Approach rejects: 128
- Score/Req promedio: 2.46/2.00
- [HTF_CONFL] muestras: 928 | ok=928 | rejects=0
- median≈ 0.109 | thr≈ 0.101
- [BIAS_FAST] muestras: 1626 | Bull=232 Bear=1310 Neutral=84 | rejects=22
- score promedio: -0.52
- [HTF_CONFL] muestras: 928 | ok=928 | rejects=0
- median≈ 0.109 | thr≈ 0.101
- [RC_SL_MINFLOOR] filtrados: 0

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,33': 1, 'estructura no existe': 2, 'score decayó a 0,41': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 8} | por bias {'Bullish': 8, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 52 | Ejecutadas: 3 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 55

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 515
- Registered: 26
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 26

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 5.0%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 11.5%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5408 | Total candidatos: 48785 | Seleccionados: 50
- Candidatos por zona (promedio): 9.0

### Take Profit (TP)
- Zonas analizadas: 5405 | Total candidatos: 82194 | Seleccionados: 5405
- Candidatos por zona (promedio): 15.2
- **Edad (barras)** - Candidatos: med=93, max=1135 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.68
- **Priority Candidatos**: {'P3': 39315}
- **Priority Seleccionados**: {'P3': 4074, 'P0': 334, 'NA': 997}
- **Type Candidatos**: {'Swing': 39315}
- **Type Seleccionados**: {'P3_Swing': 4074, 'P0_Zone': 334, 'P4_Fallback': 997}
- **TF Candidatos**: {240: 18158, 15: 11188, 60: 6356, 5: 3613}
- **TF Seleccionados**: {60: 1225, 240: 1870, 15: 816, -1: 997, 5: 497}
- **DistATR** - Candidatos: avg=25.8 | Seleccionados: avg=7.3
- **RR** - Candidatos: avg=7.68 | Seleccionados: avg=1.63
- **Razones de selección**: {'BestIntelligentScore': 5405}

### 🎯 Recomendaciones
- ⚠️ SL: 100% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.