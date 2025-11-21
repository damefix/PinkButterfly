# Informe Diagnóstico de Logs - 2025-11-06 10:20:58

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251106_101229.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251106_101229.csv`

## DFM
- Eventos de evaluación: 1301
- Evaluaciones Bull: 745 | Bear: 1182
- Pasaron umbral (PassedThreshold): 1411
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:10, 4:52, 5:278, 6:448, 7:540, 8:594, 9:5

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3990
- KeptAligned: 1499/1627 | KeptCounter: 7669/11591
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.136 | AvgProxCounter≈ 0.415
  - AvgDistATRAligned≈ 0.79 | AvgDistATRCounter≈ 1.83
- PreferAligned eventos: 863 | Filtradas contra-bias: 1037

### Proximity (Pre-PreferAligned)
- Eventos: 3990
- Aligned pre: 1499/9168 | Counter pre: 7669/9168
- AvgProxAligned(pre)≈ 0.136 | AvgDistATRAligned(pre)≈ 0.79

### Proximity Drivers
- Eventos: 3990
- Alineadas: n=1499 | BaseProx≈ 0.643 | ZoneATR≈ 5.04 | SizePenalty≈ 0.979 | FinalProx≈ 0.631
- Contra-bias: n=6632 | BaseProx≈ 0.560 | ZoneATR≈ 4.61 | SizePenalty≈ 0.984 | FinalProx≈ 0.551

## Risk
- Eventos: 3460
- Accepted=1933 | RejSL=0 | RejTP=588 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 1083 (16.2%)
- **P4_FALLBACK:** 5582 (83.8%)
- **FORCED_P3 por TF:**
  - TF5: 60 (5.5%)
  - TF15: 2 (0.2%)
  - TF60: 46 (4.2%)
  - TF240: 670 (61.9%)
  - TF1440: 305 (28.2%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 103 (1.3% del total)
  - Avg Score: 0.61 | Avg R:R: 1.52 | Avg DistATR: 10.07
  - Por TF: TF5=79, TF15=24
- **P0_SWING_LITE:** 932 (12.1% del total)
  - Avg Score: 0.30 | Avg R:R: 2.06 | Avg DistATR: 10.40
  - Por TF: TF15=452, TF60=480


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 63 | Unmatched: 1870
- 0-10: Wins=23 Losses=37 WR=38.3% (n=60)
- 10-15: Wins=2 Losses=1 WR=66.7% (n=3)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=25 Losses=38 WR=39.7% (n=63)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1933 | Aligned=244 (12.6%)
- Core≈ 1.00 | Prox≈ 0.61 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.44 | Confidence≈ 0.00
- SL_TF dist: {'5': 58, '-1': 127, '240': 263, '60': 1260, '15': 105, '1440': 120} | SL_Structural≈ 93.4%
- TP_TF dist: {'-1': 736, '5': 104, '15': 273, '60': 455, '240': 365} | TP_Structural≈ 61.9%

### SLPick por Bandas y TF
- Bandas: lt8=1827, 8-10=81, 10-12.5=20, 12.5-15=5, >15=0
- TF: 5m=58, 15m=105, 60m=1260, 240m=263, 1440m=120
- RR plan por bandas: 0-10≈ 1.44 (n=1908), 10-15≈ 1.23 (n=25)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 13218 | Zonas con Anchors: 13156
- Dir zonas (zona): Bull=7094 Bear=5533 Neutral=591
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.4, DirBear≈ 1.1, DirNeutral≈ 0.1
- Razones de dirección: {'tie-bias': 591, 'triggers-only': 49, 'anchors+triggers': 12578}
- TF Triggers: {'5': 8477, '15': 4741}
- TF Anchors: {'240': 12921, '60': 13039, '1440': 10425}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 19, 'score decayó a 0,25': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 203 | Ejecutadas: 24 | Canceladas: 0 | Expiradas: 0
- BUY: 152 | SELL: 75

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1411
- Registered: 102
  - DEDUP_COOLDOWN: 23 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 125

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 8.9%
- RegRate = Registered / Intentos = 81.6%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 18.4%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 23.5%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7503 | Total candidatos: 183151 | Seleccionados: 7465
- Candidatos por zona (promedio): 24.4
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 61434, 60: 55273, 15: 31653, 1440: 17705, 5: 17086}
- **TF Seleccionados**: {5: 219, 240: 844, 60: 4854, 15: 636, 1440: 912}
- **DistATR** - Candidatos: avg=20.0 | Seleccionados: avg=9.6
- **Razones de selección**: {'Fallback<15': 1826, 'InBand[8,15]_TFPreference': 5639}
- **En banda [10,15] ATR**: 30179/183151 (16.5%)

### Take Profit (TP)
- Zonas analizadas: 6665 | Total candidatos: 86697 | Seleccionados: 6665
- Candidatos por zona (promedio): 13.0
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=122
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.11
- **Priority Candidatos**: {'P3': 86697}
- **Priority Seleccionados**: {'P4_Fallback': 5582, 'P3': 1083}
- **Type Candidatos**: {'Swing': 86697}
- **Type Seleccionados**: {'Calculated': 5582, 'Swing': 1083}
- **TF Candidatos**: {240: 20682, 5: 17649, 15: 16690, 60: 16673, 1440: 15003}
- **TF Seleccionados**: {-1: 5582, 240: 670, 5: 60, 60: 46, 1440: 305, 15: 2}
- **DistATR** - Candidatos: avg=12.5 | Seleccionados: avg=12.8
- **RR** - Candidatos: avg=1.10 | Seleccionados: avg=1.05
- **Razones de selección**: {'NoStructuralTarget': 5582, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 179, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 22, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 14, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 231, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 69, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 67, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 16, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 211, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 112, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 23, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 38, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 7, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 49, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 3, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 63% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 84% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.92.