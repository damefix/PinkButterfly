# Informe Diagnóstico de Logs - 2025-11-13 10:02:45

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_095830.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_095830.csv`

## DFM
- Eventos de evaluación: 939
- Evaluaciones Bull: 163 | Bear: 684
- Pasaron umbral (PassedThreshold): 847
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:73, 6:371, 7:354, 8:49, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2356
- KeptAligned: 4156/4156 | KeptCounter: 2736/2842
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.445 | AvgProxCounter≈ 0.230
  - AvgDistATRAligned≈ 1.51 | AvgDistATRCounter≈ 1.14
- PreferAligned eventos: 1284 | Filtradas contra-bias: 565

### Proximity (Pre-PreferAligned)
- Eventos: 2356
- Aligned pre: 4156/6892 | Counter pre: 2736/6892
- AvgProxAligned(pre)≈ 0.445 | AvgDistATRAligned(pre)≈ 1.51

### Proximity Drivers
- Eventos: 2356
- Alineadas: n=4156 | BaseProx≈ 0.753 | ZoneATR≈ 5.18 | SizePenalty≈ 0.975 | FinalProx≈ 0.735
- Contra-bias: n=2171 | BaseProx≈ 0.529 | ZoneATR≈ 4.85 | SizePenalty≈ 0.978 | FinalProx≈ 0.520

## Risk
- Eventos: 1949
- Accepted=1265 | RejSL=0 | RejTP=0 | RejRR=1257 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 320 (11.1% del total)
  - Avg Score: 0.39 | Avg R:R: 1.89 | Avg DistATR: 3.75
  - Por TF: TF5=82, TF15=238
- **P0_SWING_LITE:** 2551 (88.9% del total)
  - Avg Score: 0.57 | Avg R:R: 4.13 | Avg DistATR: 3.49
  - Por TF: TF15=545, TF60=2006


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 89 | Unmatched: 1214
- 0-10: Wins=36 Losses=53 WR=40.4% (n=89)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=36 Losses=53 WR=40.4% (n=89)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1303 | Aligned=807 (61.9%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.06 | Confidence≈ 0.00
- SL_TF dist: {'60': 155, '5': 196, '15': 941, '240': 11} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 295, '5': 353, '15': 483, '240': 172} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1265, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=188, 15m=919, 60m=151, 240m=7, 1440m=0
- RR plan por bandas: 0-10≈ 2.04 (n=1265), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10315 | Zonas con Anchors: 10301
- Dir zonas (zona): Bull=3762 Bear=6228 Neutral=325
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9912, 'tie-bias': 389, 'triggers-only': 14}
- TF Triggers: {'15': 4859, '5': 5456}
- TF Anchors: {'60': 10228, '240': 5976, '1440': 462}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,46': 1, 'estructura no existe': 24, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,42': 1, 'score decayó a 0,34': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 192 | Ejecutadas: 36 | Canceladas: 0 | Expiradas: 0
- BUY: 70 | SELL: 158

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 847
- Registered: 101
  - DEDUP_COOLDOWN: 20 | DEDUP_IDENTICAL: 105 | SKIP_CONCURRENCY: 103
- Intentos de registro: 329

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 38.8%
- RegRate = Registered / Intentos = 30.7%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 38.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 31.3%
- ExecRate = Ejecutadas / Registered = 35.6%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5456 | Total candidatos: 43341 | Seleccionados: 0
- Candidatos por zona (promedio): 7.9

### Take Profit (TP)
- Zonas analizadas: 5349 | Total candidatos: 51244 | Seleccionados: 5349
- Candidatos por zona (promedio): 9.6
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 51244}
- **Priority Seleccionados**: {'P3': 3660, 'NA': 1371, 'P0': 318}
- **Type Candidatos**: {'Swing': 51244}
- **Type Seleccionados**: {'P3_Swing': 3660, 'P4_Fallback': 1371, 'P0_Zone': 318}
- **TF Candidatos**: {5: 15369, 15: 13946, 60: 13687, 240: 8242}
- **TF Seleccionados**: {60: 1003, 5: 1025, -1: 1371, 15: 1243, 240: 707}
- **DistATR** - Candidatos: avg=8.6 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.51 | Seleccionados: avg=1.31
- **Razones de selección**: {'BestIntelligentScore': 5349}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.