# Informe Diagnóstico de Logs - 2025-11-05 11:43:40

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251105_112151.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251105_112151.csv`

## DFM
- Eventos de evaluación: 2119
- Evaluaciones Bull: 2204 | Bear: 1458
- Pasaron umbral (PassedThreshold): 3004
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:3, 5:278, 6:936, 7:1120, 8:1311, 9:14

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3957
- KeptAligned: 541/608 | KeptCounter: 8297/12501
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.081 | AvgProxCounter≈ 0.441
  - AvgDistATRAligned≈ 0.34 | AvgDistATRCounter≈ 1.98
- PreferAligned eventos: 459 | Filtradas contra-bias: 886

### Proximity (Pre-PreferAligned)
- Eventos: 3957
- Aligned pre: 541/8838 | Counter pre: 8297/8838
- AvgProxAligned(pre)≈ 0.081 | AvgDistATRAligned(pre)≈ 0.34

### Proximity Drivers
- Eventos: 3957
- Alineadas: n=541 | BaseProx≈ 0.719 | ZoneATR≈ 4.62 | SizePenalty≈ 0.984 | FinalProx≈ 0.708
- Contra-bias: n=7411 | BaseProx≈ 0.555 | ZoneATR≈ 4.63 | SizePenalty≈ 0.984 | FinalProx≈ 0.547

## Risk
- Eventos: 3388
- Accepted=3676 | RejSL=55 | RejTP=594 | RejRR=0 | RejEntry=0
### Risk – Validación Doble Cerrojo (V6.0d)
- **RejSL_Points:** 1938 (rechazados por >60pts)
- **RejTP_Points:** 358 (rechazados por >120pts)
- **RejSL_HighVol:** 0 (rechazados por ATR>15 y DistATR>10)

**Rechazos SL por TF:**

| TF | RejSL_Points |
|----|--------------|
| -1 | 38 |
| 5 | 8 |
| 15 | 10 |
| 60 | 938 |
| 240 | 384 |
| 1440 | 560 |

**Rechazos TP por TF:**

| TF | RejTP_Points |
|----|--------------|
| -1 | 358 |

### TP Policy (V6.0c)
- **FORCED_P3:** 1067 (16.5%)
- **P4_FALLBACK:** 5383 (83.5%)
- **FORCED_P3 por TF:**
  - TF5: 56 (5.2%)
  - TF15: 1 (0.1%)
  - TF60: 49 (4.6%)
  - TF240: 688 (64.5%)
  - TF1440: 273 (25.6%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 97 (1.3% del total)
  - Avg Score: 0.60 | Avg R:R: 1.55 | Avg DistATR: 9.45
  - Por TF: TF5=71, TF15=26
- **P0_SWING_LITE:** 874 (11.8% del total)
  - Avg Score: 0.30 | Avg R:R: 2.06 | Avg DistATR: 10.37
  - Por TF: TF15=409, TF60=465

### Risk Drivers (Rechazos por SL)
- Contra-bias: n=55 | SLDistATR≈ 16.43 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:0,20-25:0,25+:0
- HistSL Counter 0-10:0,10-15:0,15-20:53,20-25:1,25+:1

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 89 | Unmatched: 3587
- 0-10: Wins=30 Losses=46 WR=39.5% (n=76)
- 10-15: Wins=0 Losses=13 WR=0.0% (n=13)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=30 Losses=59 WR=33.7% (n=89)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 3676 | Aligned=7 (0.2%)
- Core≈ 1.00 | Prox≈ 0.62 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.31 | Confidence≈ 0.00
- SL_TF dist: {'240': 470, '60': 2446, '5': 92, '-1': 110, '15': 391, '1440': 167} | SL_Structural≈ 97.0%
- TP_TF dist: {'-1': 2246, '240': 362, '5': 127, '15': 431, '60': 510} | TP_Structural≈ 38.9%

### SLPick por Bandas y TF
- Bandas: lt8=3089, 8-10=326, 10-12.5=183, 12.5-15=78, >15=0
- TF: 5m=92, 15m=391, 60m=2446, 240m=470, 1440m=167
- RR plan por bandas: 0-10≈ 1.32 (n=3415), 10-15≈ 1.15 (n=261)

## CancelBias (EMA200@60m)
- Eventos: 194
- Distribución Bias: {'Bullish': 156, 'Bearish': 38, 'Neutral': 0}
- Coherencia (Close>EMA): 156/194 (80.4%)

## StructureFusion
- Trazas por zona: 13109 | Zonas con Anchors: 13087
- Dir zonas (zona): Bull=7149 Bear=5375 Neutral=585
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.4, DirBear≈ 1.1, DirNeutral≈ 0.1
- Razones de dirección: {'tie-bias': 585, 'anchors+triggers': 12502, 'triggers-only': 22}
- TF Triggers: {'15': 4729, '5': 8380}
- TF Anchors: {'240': 12854, '60': 12975, '1440': 10178}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 14, 'score decayó a 0,20': 1, 'score decayó a 0,24': 1, 'score decayó a 0,18': 2, 'score decayó a 0,27': 1, 'score decayó a 0,46': 1, 'score decayó a 0,32': 1, 'score decayó a 0,47': 1}
- Cancel_BOS (diag): por acción {'BUY': 2, 'SELL': 31} | por bias {'Bullish': 31, 'Bearish': 2, 'Neutral': 0}

## CSV de Trades
- Filas: 163 | Ejecutadas: 23 | Canceladas: 0 | Expiradas: 0
- BUY: 76 | SELL: 110

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 3004
- Registered: 82
  - DEDUP_COOLDOWN: 9 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 91

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 3.0%
- RegRate = Registered / Intentos = 90.1%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 9.9%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 28.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7272 | Total candidatos: 178805 | Seleccionados: 7255
- Candidatos por zona (promedio): 24.6
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=147
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 60208, 60: 53338, 15: 31658, 5: 16847, 1440: 16754}
- **TF Seleccionados**: {240: 1071, 5: 184, 60: 4578, 15: 616, 1440: 806}
- **DistATR** - Candidatos: avg=20.0 | Seleccionados: avg=9.5
- **Razones de selección**: {'Fallback<15': 1864, 'InBand[8,15]_TFPreference': 5391}
- **En banda [10,15] ATR**: 29102/178805 (16.3%)

### Take Profit (TP)
- Zonas analizadas: 6450 | Total candidatos: 81907 | Seleccionados: 6450
- Candidatos por zona (promedio): 12.7
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=122
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.11
- **Priority Candidatos**: {'P3': 81907}
- **Priority Seleccionados**: {'P4_Fallback': 5383, 'P3': 1067}
- **Type Candidatos**: {'Swing': 81907}
- **Type Seleccionados**: {'Calculated': 5383, 'Swing': 1067}
- **TF Candidatos**: {240: 19641, 5: 17640, 60: 15675, 15: 15225, 1440: 13726}
- **TF Seleccionados**: {-1: 5383, 240: 688, 5: 56, 60: 49, 1440: 273, 15: 1}
- **DistATR** - Candidatos: avg=12.1 | Seleccionados: avg=12.7
- **RR** - Candidatos: avg=1.09 | Seleccionados: avg=1.05
- **Razones de selección**: {'NoStructuralTarget': 5383, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 243, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 214, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 60, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 45, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 184, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 12, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 8, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 102, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 53, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 21, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 15, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 21, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 41, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 7, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 7, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 3, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 62% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 83% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.89.