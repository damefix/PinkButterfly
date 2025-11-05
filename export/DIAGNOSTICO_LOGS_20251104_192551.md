# Informe Diagnóstico de Logs - 2025-11-04 19:35:22

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_192551.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_192551.csv`

## DFM
- Eventos de evaluación: 877
- Evaluaciones Bull: 84 | Bear: 3350
- Pasaron umbral (PassedThreshold): 1762
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:6, 4:8, 5:2515, 6:876, 7:29, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5827
- KeptAligned: 4324/8961 | KeptCounter: 37/20452
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.062 | AvgProxCounter≈ 0.001
  - AvgDistATRAligned≈ 0.89 | AvgDistATRCounter≈ 0.01
- PreferAligned eventos: 875 | Filtradas contra-bias: 15

### Proximity (Pre-PreferAligned)
- Eventos: 5827
- Aligned pre: 4324/4361 | Counter pre: 37/4361
- AvgProxAligned(pre)≈ 0.062 | AvgDistATRAligned(pre)≈ 0.89

### Proximity Drivers
- Eventos: 5827
- Alineadas: n=4324 | BaseProx≈ 0.420 | ZoneATR≈ 4.29 | SizePenalty≈ 0.981 | FinalProx≈ 0.413
- Contra-bias: n=22 | BaseProx≈ 0.401 | ZoneATR≈ 5.79 | SizePenalty≈ 0.961 | FinalProx≈ 0.384

## Risk
- Eventos: 882
- Accepted=3434 | RejSL=900 | RejTP=0 | RejRR=0 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=893 | SLDistATR≈ 19.07 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=7 | SLDistATR≈ 15.95 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:885,20-25:7,25+:1
- HistSL Counter 0-10:0,10-15:0,15-20:7,20-25:0,25+:0

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 8 | Unmatched: 3426
- 0-10: Wins=0 Losses=2 WR=0.0% (n=2)
- 10-15: Wins=3 Losses=3 WR=50.0% (n=6)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=3 Losses=5 WR=37.5% (n=8)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 3434 | Aligned=3419 (99.6%)
- Core≈ 1.00 | Prox≈ 0.43 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.41 | Confidence≈ 0.00
- SL_TF dist: {'60': 903, '-1': 5, '15': 2, '240': 2521, '5': 3} | SL_Structural≈ 99.9%
- TP_TF dist: {'-1': 85, '240': 3349} | TP_Structural≈ 97.5%

### SLPick por Bandas y TF
- Bandas: lt8=846, 8-10=18, 10-12.5=862, 12.5-15=1708, >15=0
- TF: 5m=3, 15m=2, 60m=903, 240m=2521, 1440m=0
- RR plan por bandas: 0-10≈ 1.74 (n=864), 10-15≈ 1.30 (n=2570)

## CancelBias (EMA200@60m)
- Eventos: 22
- Distribución Bias: {'Bullish': 13, 'Bearish': 9, 'Neutral': 0}
- Coherencia (Close>EMA): 13/22 (59.1%)

## StructureFusion
- Trazas por zona: 29413 | Zonas con Anchors: 29369
- Dir zonas (zona): Bull=13342 Bear=15074 Neutral=997
- Resumen por ciclo (promedios): TotHZ≈ 5.0, WithAnchors≈ 5.0, DirBull≈ 2.3, DirBear≈ 2.6, DirNeutral≈ 0.2
- Razones de dirección: {'triggers-only': 43, 'tie-bias': 2150, 'anchors+triggers': 27220}
- TF Triggers: {'15': 16778, '5': 12635}
- TF Anchors: {'60': 29290, '240': 28893}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 2078 | Distribución: {'Bullish': 934, 'Bearish': 1144, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 934/2078

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,42': 1, 'score decayó a 0,47': 1, 'score decayó a 0,26': 1, 'estructura no existe': 2, 'score decayó a 0,29': 1}
- Cancel_BOS (diag): por acción {'BUY': 3, 'SELL': 3} | por bias {'Bullish': 3, 'Bearish': 3, 'Neutral': 0}

## CSV de Trades
- Filas: 43 | Ejecutadas: 8 | Canceladas: 0 | Expiradas: 0
- BUY: 34 | SELL: 17

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1762
- Registered: 22
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 413 | SKIP_CONCURRENCY: 20
- Intentos de registro: 455

### TRADE DEDUP - Zonas y Persistencia
- Top 10 Zonas más deduplicadas (IDENTICAL):

| ZoneID | Duplicados | % del Total | Key Típica |
|--------|------------:|------------:|-----------:|
| a0890127-4b56-47da-a40b-76e9484dfa76 | 413 | 100.0% | 6799,50/6957,30/6571,25 |

- Distribución de DeltaBars (IDENTICAL):

| DeltaBars | Cantidad | % |
|-----------|---------:|---:|
| 1 | 0 | 0.0% |
| 2-5 | 0 | 0.0% |
| 6-12 | 0 | 0.0% |
| >12 | 0 | 0.0% |

- IDENTICAL por Acción: {'BUY': 0, 'SELL': 413}
- IDENTICAL por DomTF: {'240': 413}

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 25.8%
- RegRate = Registered / Intentos = 4.8%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 90.8%
- Concurrency = SKIP_CONCURRENCY / Intentos = 4.4%
- ExecRate = Ejecutadas / Registered = 36.4%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4342 | Total candidatos: 76823 | Seleccionados: 4340
- Candidatos por zona (promedio): 17.7
- **Edad (barras)** - Candidatos: med=34, max=149 | Seleccionados: med=21, max=99
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.52
- **TF Candidatos**: {60: 42908, 240: 26605, 15: 6682, 5: 628}
- **TF Seleccionados**: {60: 1788, 15: 9, 240: 2540, 5: 3}
- **DistATR** - Candidatos: avg=6.3 | Seleccionados: avg=9.4
- **Razones de selección**: {'Fallback<15': 2539, 'InBand[10,15]_TFPreference': 1801}
- **En banda [10,15] ATR**: 3917/76823 (5.1%)

### Take Profit (TP)
- Zonas analizadas: 4346 | Total candidatos: 89259 | Seleccionados: 4346
- Candidatos por zona (promedio): 20.5
- **Edad (barras)** - Candidatos: med=49, max=147 | Seleccionados: med=54, max=108
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.28
- **Priority Candidatos**: {'P3': 89259}
- **Priority Seleccionados**: {'P4_Fallback': 144, 'P3': 4202}
- **Type Candidatos**: {'Swing': 89259}
- **Type Seleccionados**: {'Calculated': 144, 'Swing': 4202}
- **TF Candidatos**: {5: 41194, 240: 22086, 15: 17190, 60: 8789}
- **TF Seleccionados**: {-1: 144, 240: 4202}
- **DistATR** - Candidatos: avg=6.1 | Seleccionados: avg=18.7
- **RR** - Candidatos: avg=0.59 | Seleccionados: avg=1.45
- **Razones de selección**: {'NoStructuralTarget': 144, 'SwingP3_TF>=60_RR>=Min_Dist>=12': 4202}

### 🎯 Recomendaciones
- ⚠️ SL: 42% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- KeptAligned ratio≈ 0.48.