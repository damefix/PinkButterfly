# Informe Diagnóstico de Logs - 2025-11-06 11:15:02

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251106_110339.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251106_110339.csv`

## DFM
- Eventos de evaluación: 1306
- Evaluaciones Bull: 748 | Bear: 1189
- Pasaron umbral (PassedThreshold): 1419
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:10, 4:52, 5:279, 6:450, 7:545, 8:596, 9:5

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3995
- KeptAligned: 1493/1621 | KeptCounter: 7678/11606
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.135 | AvgProxCounter≈ 0.416
  - AvgDistATRAligned≈ 0.79 | AvgDistATRCounter≈ 1.83
- PreferAligned eventos: 859 | Filtradas contra-bias: 1031

### Proximity (Pre-PreferAligned)
- Eventos: 3995
- Aligned pre: 1493/9171 | Counter pre: 7678/9171
- AvgProxAligned(pre)≈ 0.135 | AvgDistATRAligned(pre)≈ 0.79

### Proximity Drivers
- Eventos: 3995
- Alineadas: n=1493 | BaseProx≈ 0.643 | ZoneATR≈ 5.06 | SizePenalty≈ 0.978 | FinalProx≈ 0.631
- Contra-bias: n=6647 | BaseProx≈ 0.560 | ZoneATR≈ 4.61 | SizePenalty≈ 0.984 | FinalProx≈ 0.552

## Risk
- Eventos: 3464
- Accepted=1944 | RejSL=0 | RejTP=595 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 1091 (16.3%)
- **P4_FALLBACK:** 5591 (83.7%)
- **FORCED_P3 por TF:**
  - TF5: 59 (5.4%)
  - TF15: 2 (0.2%)
  - TF60: 46 (4.2%)
  - TF240: 671 (61.5%)
  - TF1440: 313 (28.7%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 103 (1.3% del total)
  - Avg Score: 0.61 | Avg R:R: 1.52 | Avg DistATR: 10.04
  - Por TF: TF5=79, TF15=24
- **P0_SWING_LITE:** 932 (12.1% del total)
  - Avg Score: 0.30 | Avg R:R: 2.06 | Avg DistATR: 10.40
  - Por TF: TF15=452, TF60=480


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 56 | Unmatched: 1888
- 0-10: Wins=26 Losses=27 WR=49.1% (n=53)
- 10-15: Wins=2 Losses=1 WR=66.7% (n=3)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=28 Losses=28 WR=50.0% (n=56)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1944 | Aligned=244 (12.6%)
- Core≈ 1.00 | Prox≈ 0.61 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.44 | Confidence≈ 0.00
- SL_TF dist: {'15': 115, '-1': 126, '240': 263, '60': 1259, '5': 61, '1440': 120} | SL_Structural≈ 93.5%
- TP_TF dist: {'-1': 748, '5': 103, '15': 273, '60': 455, '240': 365} | TP_Structural≈ 61.5%

### SLPick por Bandas y TF
- Bandas: lt8=1836, 8-10=82, 10-12.5=21, 12.5-15=5, >15=0
- TF: 5m=61, 15m=115, 60m=1259, 240m=263, 1440m=120
- RR plan por bandas: 0-10≈ 1.44 (n=1918), 10-15≈ 1.22 (n=26)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 13227 | Zonas con Anchors: 13176
- Dir zonas (zona): Bull=7090 Bear=5551 Neutral=586
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.4, DirBear≈ 1.1, DirNeutral≈ 0.1
- Razones de dirección: {'triggers-only': 47, 'tie-bias': 586, 'anchors+triggers': 12594}
- TF Triggers: {'5': 8495, '15': 4732}
- TF Anchors: {'240': 12941, '60': 13059, '1440': 10433}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 21}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 3} | por bias {'Bullish': 3, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 254 | Ejecutadas: 27 | Canceladas: 0 | Expiradas: 0
- BUY: 180 | SELL: 101

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1419
- Registered: 127
  - DEDUP_COOLDOWN: 34 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 161

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 11.3%
- RegRate = Registered / Intentos = 78.9%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 21.1%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 21.3%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7521 | Total candidatos: 183334 | Seleccionados: 7483
- Candidatos por zona (promedio): 24.4
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 61497, 60: 55335, 15: 31682, 1440: 17713, 5: 17107}
- **TF Seleccionados**: {15: 650, 240: 846, 5: 220, 60: 4855, 1440: 912}
- **DistATR** - Candidatos: avg=20.0 | Seleccionados: avg=9.6
- **Razones de selección**: {'Fallback<15': 1840, 'InBand[8,15]_TFPreference': 5643}
- **En banda [10,15] ATR**: 30192/183334 (16.5%)

### Take Profit (TP)
- Zonas analizadas: 6682 | Total candidatos: 86959 | Seleccionados: 6682
- Candidatos por zona (promedio): 13.0
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=122
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.11
- **Priority Candidatos**: {'P3': 86959}
- **Priority Seleccionados**: {'P4_Fallback': 5591, 'P3': 1091}
- **Type Candidatos**: {'Swing': 86959}
- **Type Seleccionados**: {'Calculated': 5591, 'Swing': 1091}
- **TF Candidatos**: {240: 20731, 5: 17709, 15: 16724, 60: 16720, 1440: 15075}
- **TF Seleccionados**: {-1: 5591, 240: 671, 5: 59, 60: 46, 1440: 313, 15: 2}
- **DistATR** - Candidatos: avg=12.5 | Seleccionados: avg=12.8
- **RR** - Candidatos: avg=1.10 | Seleccionados: avg=1.05
- **Razones de selección**: {'NoStructuralTarget': 5591, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 180, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 21, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 14, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 231, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 75, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 67, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 16, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 211, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 114, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 23, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 38, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 7, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 49, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 3, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 63% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 84% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.92.