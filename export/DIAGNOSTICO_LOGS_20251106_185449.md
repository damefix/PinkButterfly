# Informe Diagnóstico de Logs - 2025-11-06 19:01:04

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251106_185449.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251106_185449.csv`

## DFM
- Eventos de evaluación: 1289
- Evaluaciones Bull: 723 | Bear: 1161
- Pasaron umbral (PassedThreshold): 1370
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:10, 4:54, 5:277, 6:433, 7:507, 8:598, 9:5

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 4003
- KeptAligned: 1497/1626 | KeptCounter: 7711/11660
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.135 | AvgProxCounter≈ 0.417
  - AvgDistATRAligned≈ 0.79 | AvgDistATRCounter≈ 1.82
- PreferAligned eventos: 863 | Filtradas contra-bias: 1056

### Proximity (Pre-PreferAligned)
- Eventos: 4003
- Aligned pre: 1497/9208 | Counter pre: 7711/9208
- AvgProxAligned(pre)≈ 0.135 | AvgDistATRAligned(pre)≈ 0.79

### Proximity Drivers
- Eventos: 4003
- Alineadas: n=1497 | BaseProx≈ 0.641 | ZoneATR≈ 5.05 | SizePenalty≈ 0.979 | FinalProx≈ 0.629
- Contra-bias: n=6655 | BaseProx≈ 0.563 | ZoneATR≈ 4.60 | SizePenalty≈ 0.984 | FinalProx≈ 0.555

## Risk
- Eventos: 3466
- Accepted=1892 | RejSL=0 | RejTP=603 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 1100 (16.4%)
- **P4_FALLBACK:** 5608 (83.6%)
- **FORCED_P3 por TF:**
  - TF5: 63 (5.7%)
  - TF15: 3 (0.3%)
  - TF60: 65 (5.9%)
  - TF240: 655 (59.5%)
  - TF1440: 314 (28.5%)

### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 100 (1.3% del total)
  - Avg Score: 0.61 | Avg R:R: 1.54 | Avg DistATR: 9.96
  - Por TF: TF5=77, TF15=23
- **P0_SWING_LITE:** 922 (11.9% del total)
  - Avg Score: 0.30 | Avg R:R: 2.07 | Avg DistATR: 10.46
  - Por TF: TF15=457, TF60=465


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 52 | Unmatched: 1840
- 0-10: Wins=25 Losses=24 WR=51.0% (n=49)
- 10-15: Wins=3 Losses=0 WR=100.0% (n=3)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=28 Losses=24 WR=53.8% (n=52)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1892 | Aligned=245 (12.9%)
- Core≈ 1.00 | Prox≈ 0.61 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.45 | Confidence≈ 0.00
- SL_TF dist: {'15': 121, '-1': 107, '5': 58, '60': 1238, '240': 249, '1440': 119} | SL_Structural≈ 94.3%
- TP_TF dist: {'-1': 712, '5': 102, '15': 280, '60': 457, '240': 341} | TP_Structural≈ 62.4%

### SLPick por Bandas y TF
- Bandas: lt8=1786, 8-10=77, 10-12.5=23, 12.5-15=6, >15=0
- TF: 5m=58, 15m=121, 60m=1238, 240m=249, 1440m=119
- RR plan por bandas: 0-10≈ 1.45 (n=1863), 10-15≈ 1.18 (n=29)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 13286 | Zonas con Anchors: 13251
- Dir zonas (zona): Bull=7075 Bear=5618 Neutral=593
- Resumen por ciclo (promedios): TotHZ≈ 2.7, WithAnchors≈ 2.7, DirBull≈ 1.4, DirBear≈ 1.1, DirNeutral≈ 0.1
- Razones de dirección: {'triggers-only': 34, 'tie-bias': 593, 'anchors+triggers': 12659}
- TF Triggers: {'5': 8540, '15': 4746}
- TF Anchors: {'60': 13147, '240': 12847, '1440': 10558}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 19}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 210 | Ejecutadas: 23 | Canceladas: 0 | Expiradas: 0
- BUY: 121 | SELL: 112

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1370
- Registered: 105
  - DEDUP_COOLDOWN: 26 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 131

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 9.6%
- RegRate = Registered / Intentos = 80.2%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 19.8%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 21.9%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7560 | Total candidatos: 182826 | Seleccionados: 7522
- Candidatos por zona (promedio): 24.2
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.45
- **TF Candidatos**: {240: 60625, 60: 55444, 15: 31581, 1440: 17867, 5: 17309}
- **TF Seleccionados**: {15: 664, 5: 232, 60: 4862, 1440: 934, 240: 830}
- **DistATR** - Candidatos: avg=19.8 | Seleccionados: avg=9.6
- **Razones de selección**: {'Fallback<15': 1823, 'InBand[8,15]_TFPreference': 5699}
- **En banda [10,15] ATR**: 30338/182826 (16.6%)

### Take Profit (TP)
- Zonas analizadas: 6708 | Total candidatos: 87769 | Seleccionados: 6708
- Candidatos por zona (promedio): 13.1
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=122
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.11
- **Priority Candidatos**: {'P3': 87769}
- **Priority Seleccionados**: {'P4_Fallback': 5608, 'P3': 1100}
- **Type Candidatos**: {'Swing': 87769}
- **Type Seleccionados**: {'Calculated': 5608, 'Swing': 1100}
- **TF Candidatos**: {240: 20773, 5: 17750, 60: 16945, 15: 16684, 1440: 15617}
- **TF Seleccionados**: {-1: 5608, 5: 63, 60: 65, 240: 655, 1440: 314, 15: 3}
- **DistATR** - Candidatos: avg=12.6 | Seleccionados: avg=12.8
- **RR** - Candidatos: avg=1.10 | Seleccionados: avg=1.05
- **Razones de selección**: {'NoStructuralTarget': 5608, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of2': 25, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 16, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of3': 240, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of6': 74, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of5': 68, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of10': 16, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of2': 196, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 186, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of4': 116, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of11': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of9': 25, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of8': 39, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of3': 7, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of5': 5, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of6': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of15': 5, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of13': 4, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of7': 48, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of14': 6, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of16': 3, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of19': 1, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of7': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of4': 4, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of8': 1, 'SwingP3_TF>=60_RR>=1.0_Dist>=6_NextCandidate_1of12': 2, 'SwingP3_ANYTF_RR>=1.0_Dist>=6_NextCandidate_1of12': 1}

### 🎯 Recomendaciones
- ⚠️ SL: 63% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 84% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.92.