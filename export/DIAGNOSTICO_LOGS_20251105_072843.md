# Informe Diagnóstico de Logs - 2025-11-05 07:37:30

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251105_072843.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251105_072843.csv`

## DFM
- Eventos de evaluación: 1657
- Evaluaciones Bull: 2981 | Bear: 145
- Pasaron umbral (PassedThreshold): 2454
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:84, 5:588, 6:1199, 7:1248, 8:7, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3928
- KeptAligned: 8925/9987 | KeptCounter: 1793/3002
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.512 | AvgProxCounter≈ 0.138
  - AvgDistATRAligned≈ 2.90 | AvgDistATRCounter≈ 0.64
- PreferAligned eventos: 3172 | Filtradas contra-bias: 877

### Proximity (Pre-PreferAligned)
- Eventos: 3928
- Aligned pre: 8925/10718 | Counter pre: 1793/10718
- AvgProxAligned(pre)≈ 0.512 | AvgDistATRAligned(pre)≈ 2.90

### Proximity Drivers
- Eventos: 3928
- Alineadas: n=8925 | BaseProx≈ 0.666 | ZoneATR≈ 4.70 | SizePenalty≈ 0.982 | FinalProx≈ 0.655
- Contra-bias: n=916 | BaseProx≈ 0.553 | ZoneATR≈ 4.60 | SizePenalty≈ 0.983 | FinalProx≈ 0.545

## Risk
- Eventos: 3613
- Accepted=3150 | RejSL=77 | RejTP=1990 | RejRR=0 | RejEntry=0
### Risk – Validación Doble Cerrojo (V6.0d)
- **RejSL_Points:** 4136 (rechazados por >60pts)
- **RejTP_Points:** 147 (rechazados por >120pts)
- **RejSL_HighVol:** 0 (rechazados por ATR>15 y DistATR>10)

**Rechazos SL por TF:**

| TF | RejSL_Points |
|----|--------------|
| -1 | 11 |
| 5 | 15 |
| 15 | 37 |
| 60 | 2379 |
| 240 | 799 |
| 1440 | 895 |

**Rechazos TP por TF:**

| TF | RejTP_Points |
|----|--------------|
| 5 | 1 |
| 1440 | 146 |

### TP Policy (V6.0c)
- **FORCED_P3:** 4664 (47.4%)
- **P4_FALLBACK:** 5177 (52.6%)
- **FORCED_P3 por TF:**
  - TF5: 113 (2.4%)
  - TF60: 110 (2.4%)
  - TF240: 721 (15.5%)
  - TF1440: 3720 (79.8%)

### Risk Drivers (Rechazos por SL)
- Alineadas: n=77 | SLDistATR≈ 16.62 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:75,20-25:1,25+:1
- HistSL Counter 0-10:0,10-15:0,15-20:0,20-25:0,25+:0

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 76 | Unmatched: 3074
- 0-10: Wins=24 Losses=49 WR=32.9% (n=73)
- 10-15: Wins=1 Losses=2 WR=33.3% (n=3)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=25 Losses=51 WR=32.9% (n=76)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 3150 | Aligned=3142 (99.7%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.08 | Confidence≈ 0.00
- SL_TF dist: {'15': 503, '-1': 24, '5': 129, '240': 210, '60': 2246, '1440': 38} | SL_Structural≈ 99.2%
- TP_TF dist: {'-1': 2766, '240': 183, '5': 97, '60': 85, '1440': 19} | TP_Structural≈ 12.2%

### SLPick por Bandas y TF
- Bandas: lt8=2583, 8-10=321, 10-12.5=149, 12.5-15=97, >15=0
- TF: 5m=129, 15m=503, 60m=2246, 240m=210, 1440m=38
- RR plan por bandas: 0-10≈ 1.08 (n=2904), 10-15≈ 1.01 (n=246)

## CancelBias (EMA200@60m)
- Eventos: 100
- Distribución Bias: {'Bullish': 97, 'Bearish': 3, 'Neutral': 0}
- Coherencia (Close>EMA): 97/100 (97.0%)

## StructureFusion
- Trazas por zona: 12989 | Zonas con Anchors: 12927
- Dir zonas (zona): Bull=7487 Bear=5502 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.5, DirBear≈ 1.1, DirNeutral≈ 0.0
- Razones de dirección: {'triggers-only': 52, 'tie-bias': 571, 'anchors+triggers': 12366}
- TF Triggers: {'5': 8336, '15': 4653}
- TF Anchors: {'240': 12694, '60': 12818, '1440': 10137}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3748, 'Bearish': 1252, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3748/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 16, 'score decayó a 0,33': 1, 'score decayó a 0,32': 1, 'score decayó a 0,27': 1}

## CSV de Trades
- Filas: 92 | Ejecutadas: 21 | Canceladas: 0 | Expiradas: 0
- BUY: 105 | SELL: 8

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 2454
- Registered: 46
  - DEDUP_COOLDOWN: 1 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 47

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 1.9%
- RegRate = Registered / Intentos = 97.9%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 2.1%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 45.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 9808 | Total candidatos: 250545 | Seleccionados: 9754
- Candidatos por zona (promedio): 25.5
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.44
- **TF Candidatos**: {240: 85121, 60: 76033, 15: 41720, 1440: 24871, 5: 22800}
- **TF Seleccionados**: {15: 868, 5: 265, 240: 1323, 60: 6156, 1440: 1142}
- **DistATR** - Candidatos: avg=21.9 | Seleccionados: avg=9.9
- **Razones de selección**: {'Fallback<15': 2091, 'InBand[8,15]_TFPreference': 7663}
- **En banda [10,15] ATR**: 42387/250545 (16.9%)

### Take Profit (TP)
- Zonas analizadas: 9841 | Total candidatos: 135816 | Seleccionados: 9841
- Candidatos por zona (promedio): 13.8
- **Edad (barras)** - Candidatos: med=34, max=150 | Seleccionados: med=0, max=68
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.35
- **Priority Candidatos**: {'P3': 135816}
- **Priority Seleccionados**: {'P4_Fallback': 5177, 'P3': 4664}
- **Type Candidatos**: {'Swing': 135816}
- **Type Seleccionados**: {'Calculated': 5177, 'Swing': 4664}
- **TF Candidatos**: {240: 30920, 5: 29574, 15: 29230, 60: 26871, 1440: 19221}
- **TF Seleccionados**: {-1: 5177, 240: 721, 5: 113, 60: 110, 1440: 3720}
- **DistATR** - Candidatos: avg=12.5 | Seleccionados: avg=16.2
- **RR** - Candidatos: avg=1.10 | Seleccionados: avg=1.37
- **Razones de selección**: {'NoStructuralTarget': 5177, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 4551, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 113}

### 🎯 Recomendaciones
- ⚠️ SL: 64% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 53% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.89.