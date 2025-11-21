# Informe Diagnóstico de Logs - 2025-11-06 12:52:06

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251106_124857.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251106_124857.csv`

## DFM
- Eventos de evaluación: 688
- Evaluaciones Bull: 207 | Bear: 819
- Pasaron umbral (PassedThreshold): 731
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:7, 4:32, 5:161, 6:238, 7:294, 8:289, 9:5

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2296
- KeptAligned: 825/923 | KeptCounter: 4347/6736
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.130 | AvgProxCounter≈ 0.395
  - AvgDistATRAligned≈ 0.77 | AvgDistATRCounter≈ 1.75
- PreferAligned eventos: 478 | Filtradas contra-bias: 555

### Proximity (Pre-PreferAligned)
- Eventos: 2296
- Aligned pre: 825/5172 | Counter pre: 4347/5172
- AvgProxAligned(pre)≈ 0.130 | AvgDistATRAligned(pre)≈ 0.77

### Proximity Drivers
- Eventos: 2296
- Alineadas: n=825 | BaseProx≈ 0.644 | ZoneATR≈ 5.06 | SizePenalty≈ 0.979 | FinalProx≈ 0.633
- Contra-bias: n=3792 | BaseProx≈ 0.554 | ZoneATR≈ 4.57 | SizePenalty≈ 0.984 | FinalProx≈ 0.546

## Risk
- Eventos: 1908
- Accepted=1032 | RejSL=0 | RejTP=372 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 661 (17.1%)
- **P4_FALLBACK:** 3203 (82.9%)
- **FORCED_P3 por TF:**
  - TF5: 16 (2.4%)
  - TF15: 1 (0.2%)
  - TF60: 37 (5.6%)
  - TF240: 381 (57.6%)
  - TF1440: 226 (34.2%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 36 (0.8% del total)
  - Avg Score: 0.60 | Avg R:R: 1.49 | Avg DistATR: 9.05
  - Por TF: TF5=25, TF15=11
- **P0_SWING_LITE:** 473 (10.8% del total)
  - Avg Score: 0.30 | Avg R:R: 1.85 | Avg DistATR: 9.59
  - Por TF: TF15=215, TF60=258


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 25 | Unmatched: 1007
- 0-10: Wins=19 Losses=6 WR=76.0% (n=25)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=19 Losses=6 WR=76.0% (n=25)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1032 | Aligned=91 (8.8%)
- Core≈ 1.00 | Prox≈ 0.59 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.41 | Confidence≈ 0.00
- SL_TF dist: {'60': 548, '240': 227, '1440': 118, '15': 53, '5': 24, '-1': 62} | SL_Structural≈ 94.0%
- TP_TF dist: {'240': 214, '-1': 374, '5': 24, '15': 140, '60': 280} | TP_Structural≈ 63.8%

### SLPick por Bandas y TF
- Bandas: lt8=980, 8-10=41, 10-12.5=10, 12.5-15=1, >15=0
- TF: 5m=24, 15m=53, 60m=548, 240m=227, 1440m=118
- RR plan por bandas: 0-10≈ 1.41 (n=1021), 10-15≈ 1.28 (n=11)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 7659 | Zonas con Anchors: 7650
- Dir zonas (zona): Bull=2887 Bear=4423 Neutral=349
- Resumen por ciclo (promedios): TotHZ≈ 2.7, WithAnchors≈ 2.7, DirBull≈ 1.0, DirBear≈ 1.6, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 7301, 'tie-bias': 349, 'triggers-only': 9}
- TF Triggers: {'5': 4936, '15': 2723}
- TF Anchors: {'60': 7603, '240': 7517, '1440': 6449}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,31': 1, 'estructura no existe': 7}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 68 | Ejecutadas: 9 | Canceladas: 0 | Expiradas: 0
- BUY: 8 | SELL: 69

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 731
- Registered: 34
  - DEDUP_COOLDOWN: 6 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 40

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 5.5%
- RegRate = Registered / Intentos = 85.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 15.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 26.5%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4259 | Total candidatos: 108896 | Seleccionados: 4259
- Candidatos por zona (promedio): 25.6
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=38, max=150
- **Score** - Candidatos: avg=0.48 | Seleccionados: avg=0.49
- **TF Candidatos**: {240: 36048, 60: 30721, 15: 19445, 1440: 12503, 5: 10179}
- **TF Seleccionados**: {60: 2477, 15: 282, 5: 81, 240: 691, 1440: 728}
- **DistATR** - Candidatos: avg=18.2 | Seleccionados: avg=9.5
- **Razones de selección**: {'InBand[8,15]_TFPreference': 3127, 'Fallback<15': 1132}
- **En banda [10,15] ATR**: 18311/108896 (16.8%)

### Take Profit (TP)
- Zonas analizadas: 3864 | Total candidatos: 71645 | Seleccionados: 3864
- Candidatos por zona (promedio): 18.5
- **Edad (barras)** - Candidatos: med=40, max=150 | Seleccionados: med=0, max=122
- **Score** - Candidatos: avg=0.48 | Seleccionados: avg=0.10
- **Priority Candidatos**: {'P3': 71645}
- **Priority Seleccionados**: {'P3': 661, 'P4_Fallback': 3203}
- **Type Candidatos**: {'Swing': 71645}
- **Type Seleccionados**: {'Swing': 661, 'Calculated': 3203}
- **TF Candidatos**: {1440: 23217, 240: 16247, 60: 11070, 5: 10592, 15: 10519}
- **TF Seleccionados**: {240: 381, 1440: 226, -1: 3203, 5: 16, 60: 37, 15: 1}
- **DistATR** - Candidatos: avg=23.0 | Seleccionados: avg=12.9
- **RR** - Candidatos: avg=1.97 | Seleccionados: avg=1.05
- **Razones de selección**: {'SwingP3_TF>=60_RR>=1.0_Dist>=6': 119, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 104, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of18': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 9, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 99, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 154, 'NoStructuralTarget': 3203, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 48, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 47, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 31, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 9, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 19, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 3, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 2, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 55% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 83% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 0.89.