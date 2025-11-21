# Informe Diagnóstico de Logs - 2025-11-07 20:52:13

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251107_204828.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251107_204828.csv`

## DFM
- Eventos de evaluación: 70
- Evaluaciones Bull: 0 | Bear: 102
- Pasaron umbral (PassedThreshold): 86
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:1, 6:47, 7:48, 8:6, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 95
- KeptAligned: 1/1 | KeptCounter: 177/189
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.005 | AvgProxCounter≈ 0.512
  - AvgDistATRAligned≈ 0.05 | AvgDistATRCounter≈ 2.44
- PreferAligned eventos: 1 | Filtradas contra-bias: 3

### Proximity (Pre-PreferAligned)
- Eventos: 95
- Aligned pre: 1/178 | Counter pre: 177/178
- AvgProxAligned(pre)≈ 0.005 | AvgDistATRAligned(pre)≈ 0.05

### Proximity Drivers
- Eventos: 95
- Alineadas: n=1 | BaseProx≈ 0.507 | ZoneATR≈ 2.46 | SizePenalty≈ 1.000 | FinalProx≈ 0.507
- Contra-bias: n=174 | BaseProx≈ 0.536 | ZoneATR≈ 5.12 | SizePenalty≈ 0.980 | FinalProx≈ 0.526

## Risk
- Eventos: 95
- Accepted=102 | RejSL=0 | RejTP=0 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 5 (2.9%)
- **P4_FALLBACK:** 169 (97.1%)
- **FORCED_P3 por TF:**
  - TF5: 2 (40.0%)
  - TF15: 3 (60.0%)


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 1 | Unmatched: 101
- 0-10: Wins=1 Losses=0 WR=100.0% (n=1)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=1 Losses=0 WR=100.0% (n=1)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 102 | Aligned=0 (0.0%)
- Core≈ 1.00 | Prox≈ 0.53 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.00 | Confidence≈ 0.00
- SL_TF dist: {'60': 102} | SL_Structural≈ 100.0%
- TP_TF dist: {'-1': 99, '15': 1, '5': 2} | TP_Structural≈ 2.9%

### SLPick por Bandas y TF
- Bandas: lt8=102, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=0, 15m=0, 60m=102, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.00 (n=102), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 3713 | Zonas con Anchors: 3712
- Dir zonas (zona): Bull=101 Bear=3599 Neutral=13
- Resumen por ciclo (promedios): TotHZ≈ 1.9, WithAnchors≈ 1.9, DirBull≈ 0.0, DirBear≈ 1.9, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 3699, 'triggers-only': 1, 'tie-bias': 13}
- TF Triggers: {'15': 134, '5': 56}
- TF Anchors: {'60': 189, '240': 179, '1440': 44}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)

## CSV de Trades
- Filas: 5 | Ejecutadas: 1 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 6

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 86
- Registered: 3
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 6
- Intentos de registro: 9

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 10.5%
- RegRate = Registered / Intentos = 33.3%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 66.7%
- ExecRate = Ejecutadas / Registered = 33.3%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 174 | Total candidatos: 3875 | Seleccionados: 174
- Candidatos por zona (promedio): 22.3
- **Edad (barras)** - Candidatos: med=37, max=138 | Seleccionados: med=26, max=73
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 1549, 60: 1085, 15: 668, 5: 399, 1440: 174}
- **TF Seleccionados**: {60: 172, 240: 2}
- **DistATR** - Candidatos: avg=9.0 | Seleccionados: avg=5.7
- **Razones de selección**: {'InBand[4,8]_TFPreference': 170, 'Fallback<15': 4}
- **En banda [10,15] ATR**: 863/3875 (22.3%)

### Take Profit (TP)
- Zonas analizadas: 174 | Total candidatos: 498 | Seleccionados: 174
- Candidatos por zona (promedio): 2.9
- **Edad (barras)** - Candidatos: med=16, max=112 | Seleccionados: med=0, max=60
- **Score** - Candidatos: avg=0.48 | Seleccionados: avg=0.01
- **Priority Candidatos**: {'P3': 498}
- **Priority Seleccionados**: {'P4_Fallback': 169, 'P3': 5}
- **Type Candidatos**: {'Swing': 498}
- **Type Seleccionados**: {'Calculated': 169, 'Swing': 5}
- **TF Candidatos**: {15: 218, 5: 174, 60: 62, 240: 44}
- **TF Seleccionados**: {-1: 169, 15: 3, 5: 2}
- **DistATR** - Candidatos: avg=3.2 | Seleccionados: avg=9.6
- **RR** - Candidatos: avg=0.40 | Seleccionados: avg=1.00
- **Razones de selección**: {'NoStructuralTarget': 169, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 3, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 2}

### 🎯 Recomendaciones
- ⚠️ SL: 72% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 97% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.