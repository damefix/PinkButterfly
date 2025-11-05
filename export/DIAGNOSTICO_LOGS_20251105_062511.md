# Informe Diagnóstico de Logs - 2025-11-05 06:31:22

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251105_062511.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251105_062511.csv`

## DFM
- Eventos de evaluación: 3258
- Evaluaciones Bull: 4651 | Bear: 2905
- Pasaron umbral (PassedThreshold): 5697
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:96, 4:541, 5:1222, 6:2443, 7:3237, 8:17, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3928
- KeptAligned: 8922/9984 | KeptCounter: 1787/3006
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.511 | AvgProxCounter≈ 0.138
  - AvgDistATRAligned≈ 2.90 | AvgDistATRCounter≈ 0.63
- PreferAligned eventos: 3168 | Filtradas contra-bias: 869

### Proximity (Pre-PreferAligned)
- Eventos: 3928
- Aligned pre: 8922/10709 | Counter pre: 1787/10709
- AvgProxAligned(pre)≈ 0.511 | AvgDistATRAligned(pre)≈ 2.90

### Proximity Drivers
- Eventos: 3928
- Alineadas: n=8922 | BaseProx≈ 0.666 | ZoneATR≈ 4.70 | SizePenalty≈ 0.982 | FinalProx≈ 0.655
- Contra-bias: n=918 | BaseProx≈ 0.554 | ZoneATR≈ 4.60 | SizePenalty≈ 0.983 | FinalProx≈ 0.546

## Risk
- Eventos: 3611
- Accepted=7582 | RejSL=1981 | RejTP=0 | RejRR=0 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1802 | SLDistATR≈ 16.65 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=179 | SLDistATR≈ 16.92 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:1761,20-25:41,25+:0
- HistSL Counter 0-10:0,10-15:0,15-20:175,20-25:4,25+:0

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 45 | Unmatched: 7537
- 0-10: Wins=10 Losses=6 WR=62.5% (n=16)
- 10-15: Wins=8 Losses=21 WR=27.6% (n=29)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=18 Losses=27 WR=40.0% (n=45)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 7582 | Aligned=6847 (90.3%)
- Core≈ 1.00 | Prox≈ 0.68 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.39 | Confidence≈ 0.00
- SL_TF dist: {'5': 201, '-1': 83, '60': 4743, '15': 596, '240': 1119, '1440': 840} | SL_Structural≈ 98.9%
- TP_TF dist: {'-1': 3820, '240': 614, '5': 110, '60': 110, '1440': 2928} | TP_Structural≈ 49.6%

### SLPick por Bandas y TF
- Bandas: lt8=1257, 8-10=1028, 10-12.5=2530, 12.5-15=2767, >15=0
- TF: 5m=201, 15m=596, 60m=4743, 240m=1119, 1440m=840
- RR plan por bandas: 0-10≈ 1.63 (n=2285), 10-15≈ 1.29 (n=5297)

## CancelBias (EMA200@60m)
- Eventos: 103
- Distribución Bias: {'Bullish': 72, 'Bearish': 31, 'Neutral': 0}
- Coherencia (Close>EMA): 72/103 (69.9%)

## StructureFusion
- Trazas por zona: 12990 | Zonas con Anchors: 12958
- Dir zonas (zona): Bull=7487 Bear=5503 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.5, DirBear≈ 1.1, DirNeutral≈ 0.0
- Razones de dirección: {'triggers-only': 28, 'tie-bias': 569, 'anchors+triggers': 12393}
- TF Triggers: {'15': 4656, '5': 8334}
- TF Anchors: {'60': 12854, '240': 12681, '1440': 10122}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3748, 'Bearish': 1252, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3748/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 15, 'score decayó a 0,21': 1, 'score decayó a 0,27': 1, 'score decayó a 0,46': 1, 'score decayó a 0,32': 1}
- Cancel_BOS (diag): por acción {'BUY': 1, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 1, 'Neutral': 0}

## CSV de Trades
- Filas: 81 | Ejecutadas: 14 | Canceladas: 0 | Expiradas: 0
- BUY: 71 | SELL: 24

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 5697
- Registered: 41
  - DEDUP_COOLDOWN: 9 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 50

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 0.9%
- RegRate = Registered / Intentos = 82.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 18.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 34.1%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 9806 | Total candidatos: 250861 | Seleccionados: 9752
- Candidatos por zona (promedio): 25.6
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=40, max=150
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.44
- **TF Candidatos**: {240: 85101, 60: 76257, 15: 41786, 1440: 24868, 5: 22849}
- **TF Seleccionados**: {5: 266, 60: 6215, 15: 864, 240: 1265, 1440: 1142}
- **DistATR** - Candidatos: avg=21.9 | Seleccionados: avg=9.9
- **Razones de selección**: {'Fallback<15': 2088, 'InBand[8,15]_TFPreference': 7664}
- **En banda [10,15] ATR**: 42403/250861 (16.9%)

### Take Profit (TP)
- Zonas analizadas: 9840 | Total candidatos: 135948 | Seleccionados: 9840
- Candidatos por zona (promedio): 13.8
- **Edad (barras)** - Candidatos: med=34, max=150 | Seleccionados: med=0, max=68
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.35
- **Priority Candidatos**: {'P3': 135948}
- **Priority Seleccionados**: {'P4_Fallback': 5183, 'P3': 4657}
- **Type Candidatos**: {'Swing': 135948}
- **Type Seleccionados**: {'Calculated': 5183, 'Swing': 4657}
- **TF Candidatos**: {240: 30911, 5: 29503, 15: 29265, 60: 27075, 1440: 19194}
- **TF Seleccionados**: {-1: 5183, 240: 719, 5: 111, 60: 110, 1440: 3717}
- **DistATR** - Candidatos: avg=12.5 | Seleccionados: avg=16.2
- **RR** - Candidatos: avg=1.10 | Seleccionados: avg=1.37
- **Razones de selección**: {'NoStructuralTarget': 5183, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 4546, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 111}

### 🎯 Recomendaciones
- ⚠️ SL: 65% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 53% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.89.