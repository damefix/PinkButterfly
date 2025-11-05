# Informe Diagnóstico de Logs - 2025-11-05 06:52:03

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251105_064617.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251105_064617.csv`

## DFM
- Eventos de evaluación: 3260
- Evaluaciones Bull: 4653 | Bear: 2896
- Pasaron umbral (PassedThreshold): 5693
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:96, 4:541, 5:1219, 6:2448, 7:3228, 8:17, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3930
- KeptAligned: 8914/9976 | KeptCounter: 1794/3012
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.511 | AvgProxCounter≈ 0.138
  - AvgDistATRAligned≈ 2.90 | AvgDistATRCounter≈ 0.64
- PreferAligned eventos: 3170 | Filtradas contra-bias: 875

### Proximity (Pre-PreferAligned)
- Eventos: 3930
- Aligned pre: 8914/10708 | Counter pre: 1794/10708
- AvgProxAligned(pre)≈ 0.511 | AvgDistATRAligned(pre)≈ 2.90

### Proximity Drivers
- Eventos: 3930
- Alineadas: n=8914 | BaseProx≈ 0.666 | ZoneATR≈ 4.70 | SizePenalty≈ 0.982 | FinalProx≈ 0.655
- Contra-bias: n=919 | BaseProx≈ 0.552 | ZoneATR≈ 4.61 | SizePenalty≈ 0.983 | FinalProx≈ 0.545

## Risk
- Eventos: 3614
- Accepted=7574 | RejSL=1981 | RejTP=0 | RejRR=0 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1802 | SLDistATR≈ 16.65 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=179 | SLDistATR≈ 16.92 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:1761,20-25:41,25+:0
- HistSL Counter 0-10:0,10-15:0,15-20:175,20-25:4,25+:0

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 46 | Unmatched: 7528
- 0-10: Wins=10 Losses=7 WR=58.8% (n=17)
- 10-15: Wins=8 Losses=21 WR=27.6% (n=29)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=18 Losses=28 WR=39.1% (n=46)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 7574 | Aligned=6840 (90.3%)
- Core≈ 1.00 | Prox≈ 0.68 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.39 | Confidence≈ 0.00
- SL_TF dist: {'-1': 78, '5': 197, '60': 4747, '15': 596, '240': 1116, '1440': 840} | SL_Structural≈ 99.0%
- TP_TF dist: {'-1': 3805, '60': 112, '240': 615, '5': 112, '1440': 2930} | TP_Structural≈ 49.8%

### SLPick por Bandas y TF
- Bandas: lt8=1248, 8-10=1027, 10-12.5=2530, 12.5-15=2769, >15=0
- TF: 5m=197, 15m=596, 60m=4747, 240m=1116, 1440m=840
- RR plan por bandas: 0-10≈ 1.64 (n=2275), 10-15≈ 1.29 (n=5299)

## CancelBias (EMA200@60m)
- Eventos: 88
- Distribución Bias: {'Bullish': 72, 'Bearish': 16, 'Neutral': 0}
- Coherencia (Close>EMA): 72/88 (81.8%)

## StructureFusion
- Trazas por zona: 12988 | Zonas con Anchors: 12960
- Dir zonas (zona): Bull=7497 Bear=5491 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.5, DirBear≈ 1.1, DirNeutral≈ 0.0
- Razones de dirección: {'triggers-only': 28, 'anchors+triggers': 12397, 'tie-bias': 563}
- TF Triggers: {'15': 4656, '5': 8332}
- TF Anchors: {'60': 12856, '240': 12684, '1440': 10126}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3748, 'Bearish': 1252, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3748/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 15, 'score decayó a 0,21': 1, 'score decayó a 0,27': 1, 'score decayó a 0,46': 1, 'score decayó a 0,32': 1}
- Cancel_BOS (diag): por acción {'BUY': 1, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 1, 'Neutral': 0}

## CSV de Trades
- Filas: 81 | Ejecutadas: 15 | Canceladas: 0 | Expiradas: 0
- BUY: 71 | SELL: 25

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 5693
- Registered: 41
  - DEDUP_COOLDOWN: 9 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 50

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 0.9%
- RegRate = Registered / Intentos = 82.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 18.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 36.6%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 9803 | Total candidatos: 250877 | Seleccionados: 9749
- Candidatos por zona (promedio): 25.6
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=40, max=150
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.44
- **TF Candidatos**: {240: 85112, 60: 76275, 15: 41790, 1440: 24870, 5: 22830}
- **TF Seleccionados**: {5: 262, 60: 6219, 15: 864, 240: 1262, 1440: 1142}
- **DistATR** - Candidatos: avg=21.9 | Seleccionados: avg=9.9
- **Razones de selección**: {'Fallback<15': 2083, 'InBand[8,15]_TFPreference': 7666}
- **En banda [10,15] ATR**: 42412/250877 (16.9%)

### Take Profit (TP)
- Zonas analizadas: 9833 | Total candidatos: 135975 | Seleccionados: 9833
- Candidatos por zona (promedio): 13.8
- **Edad (barras)** - Candidatos: med=34, max=150 | Seleccionados: med=0, max=68
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.35
- **Priority Candidatos**: {'P3': 135975}
- **Priority Seleccionados**: {'P4_Fallback': 5169, 'P3': 4664}
- **Type Candidatos**: {'Swing': 135975}
- **Type Seleccionados**: {'Calculated': 5169, 'Swing': 4664}
- **TF Candidatos**: {240: 30917, 5: 29508, 15: 29264, 60: 27074, 1440: 19212}
- **TF Seleccionados**: {-1: 5169, 60: 112, 240: 720, 5: 113, 1440: 3719}
- **DistATR** - Candidatos: avg=12.5 | Seleccionados: avg=16.2
- **RR** - Candidatos: avg=1.10 | Seleccionados: avg=1.37
- **Razones de selección**: {'NoStructuralTarget': 5169, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 4551, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 113}

### 🎯 Recomendaciones
- ⚠️ SL: 65% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 53% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.89.