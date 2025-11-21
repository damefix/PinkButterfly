# Informe Diagnóstico de Logs - 2025-11-06 15:46:50

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251106_134310.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251106_134310.csv`

## DFM
- Eventos de evaluación: 4502
- Evaluaciones Bull: 2907 | Bear: 3688
- Pasaron umbral (PassedThreshold): 4428
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:27, 4:210, 5:1116, 6:1752, 7:1830, 8:1632, 9:28

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 15784
- KeptAligned: 6784/7325 | KeptCounter: 28534/44316
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.147 | AvgProxCounter≈ 0.386
  - AvgDistATRAligned≈ 0.91 | AvgDistATRCounter≈ 1.72
- PreferAligned eventos: 3776 | Filtradas contra-bias: 4570

### Proximity (Pre-PreferAligned)
- Eventos: 15784
- Aligned pre: 6784/35318 | Counter pre: 28534/35318
- AvgProxAligned(pre)≈ 0.147 | AvgDistATRAligned(pre)≈ 0.91

### Proximity Drivers
- Eventos: 15784
- Alineadas: n=6784 | BaseProx≈ 0.641 | ZoneATR≈ 4.79 | SizePenalty≈ 0.982 | FinalProx≈ 0.631
- Contra-bias: n=23964 | BaseProx≈ 0.556 | ZoneATR≈ 4.60 | SizePenalty≈ 0.984 | FinalProx≈ 0.548

## Risk
- Eventos: 13181
- Accepted=6604 | RejSL=0 | RejTP=2459 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 4499 (17.8%)
- **P4_FALLBACK:** 20811 (82.2%)
- **FORCED_P3 por TF:**
  - TF5: 182 (4.0%)
  - TF15: 49 (1.1%)
  - TF60: 409 (9.1%)
  - TF240: 2403 (53.4%)
  - TF1440: 1456 (32.4%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 466 (1.6% del total)
  - Avg Score: 0.61 | Avg R:R: 1.65 | Avg DistATR: 9.26
  - Por TF: TF5=345, TF15=121
- **P0_SWING_LITE:** 2941 (10.2% del total)
  - Avg Score: 0.32 | Avg R:R: 2.09 | Avg DistATR: 9.91
  - Por TF: TF15=1305, TF60=1636


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 32 | Unmatched: 6572
- 0-10: Wins=12 Losses=19 WR=38.7% (n=31)
- 10-15: Wins=0 Losses=1 WR=0.0% (n=1)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=12 Losses=20 WR=37.5% (n=32)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 6604 | Aligned=1116 (16.9%)
- Core≈ 1.00 | Prox≈ 0.59 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.48 | Confidence≈ 0.00
- SL_TF dist: {'-1': 373, '5': 177, '60': 4119, '15': 600, '240': 954, '1440': 381} | SL_Structural≈ 94.4%
- TP_TF dist: {'-1': 2163, '5': 375, '60': 1798, '15': 982, '240': 1286} | TP_Structural≈ 67.2%

### SLPick por Bandas y TF
- Bandas: lt8=6287, 8-10=235, 10-12.5=63, 12.5-15=19, >15=0
- TF: 5m=177, 15m=600, 60m=4119, 240m=954, 1440m=381
- RR plan por bandas: 0-10≈ 1.47 (n=6522), 10-15≈ 1.50 (n=82)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 51641 | Zonas con Anchors: 51509
- Dir zonas (zona): Bull=25249 Bear=23700 Neutral=2692
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.3, DirBear≈ 1.2, DirNeutral≈ 0.1
- Razones de dirección: {'triggers-only': 128, 'tie-bias': 2692, 'anchors+triggers': 48821}
- TF Triggers: {'5': 33553, '15': 18088}
- TF Anchors: {'60': 51153, '240': 47506, '1440': 40148}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,36': 1, 'estructura no existe': 14, 'score decayó a 0,22': 1}
- Cancel_BOS (diag): por acción {'BUY': 1, 'SELL': 0} | por bias {'Bullish': 0, 'Bearish': 1, 'Neutral': 0}

## CSV de Trades
- Filas: 173 | Ejecutadas: 18 | Canceladas: 0 | Expiradas: 0
- BUY: 43 | SELL: 148

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 4428
- Registered: 87
  - DEDUP_COOLDOWN: 23 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 110

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 2.5%
- RegRate = Registered / Intentos = 79.1%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 20.9%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 20.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 28226 | Total candidatos: 692860 | Seleccionados: 28079
- Candidatos por zona (promedio): 24.5
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=40, max=150
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 213586, 60: 196574, 15: 125335, 1440: 89301, 5: 68064}
- **TF Seleccionados**: {5: 626, 60: 17749, 15: 2479, 240: 3494, 1440: 3731}
- **DistATR** - Candidatos: avg=21.8 | Seleccionados: avg=9.7
- **Razones de selección**: {'Fallback<15': 6588, 'InBand[8,15]_TFPreference': 21491}
- **En banda [10,15] ATR**: 107208/692860 (15.5%)

### Take Profit (TP)
- Zonas analizadas: 25310 | Total candidatos: 396639 | Seleccionados: 25310
- Candidatos por zona (promedio): 15.7
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=0, max=130
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.11
- **Priority Candidatos**: {'P3': 396639}
- **Priority Seleccionados**: {'P4_Fallback': 20811, 'P3': 4499}
- **Type Candidatos**: {'Swing': 396639}
- **Type Seleccionados**: {'Calculated': 20811, 'Swing': 4499}
- **TF Candidatos**: {240: 94111, 60: 80078, 1440: 79081, 5: 72198, 15: 71171}
- **TF Seleccionados**: {-1: 20811, 5: 182, 60: 409, 15: 49, 240: 2403, 1440: 1456}
- **DistATR** - Candidatos: avg=18.3 | Seleccionados: avg=13.0
- **RR** - Candidatos: avg=1.67 | Seleccionados: avg=1.06
- **Razones de selección**: {'NoStructuralTarget': 20811, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 61, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 79, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 40, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 440, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 1077, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 943, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 787, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 52, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 70, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 78, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 234, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 226, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 19, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 146, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 13, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 9, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 3, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 72, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 29, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 16, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of21': 3, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of25': 2, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 15, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 28, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 18, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of17': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of18': 12, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 3, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of10': 3, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of9': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of24': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of20': 8, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 64% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 82% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 0.93.