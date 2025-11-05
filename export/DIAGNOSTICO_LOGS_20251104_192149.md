# Informe Diagnóstico de Logs - 2025-11-04 19:34:33

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_192149.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_192149.csv`

## DFM
- Eventos de evaluación: 756
- Evaluaciones Bull: 84 | Bear: 2866
- Pasaron umbral (PassedThreshold): 2211
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:6, 4:8, 5:1445, 6:1462, 7:29, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5706
- KeptAligned: 3012/9063 | KeptCounter: 744/21161
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.070 | AvgProxCounter≈ 0.106
  - AvgDistATRAligned≈ 0.63 | AvgDistATRCounter≈ 0.10
- PreferAligned eventos: 754 | Filtradas contra-bias: 722

### Proximity (Pre-PreferAligned)
- Eventos: 5706
- Aligned pre: 3012/3756 | Counter pre: 744/3756
- AvgProxAligned(pre)≈ 0.070 | AvgDistATRAligned(pre)≈ 0.63

### Proximity Drivers
- Eventos: 5706
- Alineadas: n=3012 | BaseProx≈ 0.534 | ZoneATR≈ 3.11 | SizePenalty≈ 0.995 | FinalProx≈ 0.532
- Contra-bias: n=22 | BaseProx≈ 0.401 | ZoneATR≈ 5.79 | SizePenalty≈ 0.961 | FinalProx≈ 0.384

## Risk
- Eventos: 761
- Accepted=2950 | RejSL=72 | RejTP=0 | RejRR=0 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=65 | SLDistATR≈ 17.46 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=7 | SLDistATR≈ 15.95 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:57,20-25:7,25+:1
- HistSL Counter 0-10:0,10-15:0,15-20:7,20-25:0,25+:0

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 8 | Unmatched: 2942
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
- Muestras: 2950 | Aligned=2935 (99.5%)
- Core≈ 1.00 | Prox≈ 0.54 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.61 | Confidence≈ 0.00
- SL_TF dist: {'60': 75, '-1': 5, '15': 2, '240': 2865, '5': 3} | SL_Structural≈ 99.8%
- TP_TF dist: {'-1': 85, '240': 2865} | TP_Structural≈ 97.1%

### SLPick por Bandas y TF
- Bandas: lt8=725, 8-10=18, 10-12.5=1448, 12.5-15=759, >15=0
- TF: 5m=3, 15m=2, 60m=75, 240m=2865, 1440m=0
- RR plan por bandas: 0-10≈ 1.74 (n=743), 10-15≈ 1.56 (n=2207)

## CancelBias (EMA200@60m)
- Eventos: 21
- Distribución Bias: {'Bullish': 13, 'Bearish': 8, 'Neutral': 0}
- Coherencia (Close>EMA): 13/21 (61.9%)

## StructureFusion
- Trazas por zona: 30224 | Zonas con Anchors: 30180
- Dir zonas (zona): Bull=14050 Bear=15177 Neutral=997
- Resumen por ciclo (promedios): TotHZ≈ 5.3, WithAnchors≈ 5.3, DirBull≈ 2.5, DirBear≈ 2.7, DirNeutral≈ 0.2
- Razones de dirección: {'triggers-only': 43, 'tie-bias': 2029, 'anchors+triggers': 28152}
- TF Triggers: {'15': 17831, '5': 12393}
- TF Anchors: {'60': 30101, '240': 29704}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 1957 | Distribución: {'Bullish': 934, 'Bearish': 1023, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 934/1957

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,42': 1, 'score decayó a 0,47': 1, 'score decayó a 0,26': 1, 'estructura no existe': 2, 'score decayó a 0,29': 1}
- Cancel_BOS (diag): por acción {'BUY': 3, 'SELL': 3} | por bias {'Bullish': 3, 'Bearish': 3, 'Neutral': 0}

## CSV de Trades
- Filas: 43 | Ejecutadas: 8 | Canceladas: 0 | Expiradas: 0
- BUY: 34 | SELL: 17

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 2211
- Registered: 22
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 353 | SKIP_CONCURRENCY: 20
- Intentos de registro: 395

### TRADE DEDUP - Zonas y Persistencia
- Top 10 Zonas más deduplicadas (IDENTICAL):

| ZoneID | Duplicados | % del Total | Key Típica |
|--------|------------:|------------:|-----------:|
| bcd87980-2ead-42c6-8e7d-f7fc510b7cad | 353 | 100.0% | 6825,50/6957,30/6571,25 |

- Distribución de DeltaBars (IDENTICAL):

| DeltaBars | Cantidad | % |
|-----------|---------:|---:|
| 1 | 0 | 0.0% |
| 2-5 | 0 | 0.0% |
| 6-12 | 0 | 0.0% |
| >12 | 0 | 0.0% |

- IDENTICAL por Acción: {'BUY': 0, 'SELL': 353}
- IDENTICAL por DomTF: {'240': 353}

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 17.9%
- RegRate = Registered / Intentos = 5.6%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 89.4%
- Concurrency = SKIP_CONCURRENCY / Intentos = 5.1%
- ExecRate = Ejecutadas / Registered = 36.4%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 3030 | Total candidatos: 54984 | Seleccionados: 3028
- Candidatos por zona (promedio): 18.1
- **Edad (barras)** - Candidatos: med=34, max=149 | Seleccionados: med=21, max=99
- **Score** - Candidatos: avg=0.50 | Seleccionados: avg=0.67
- **TF Candidatos**: {60: 30495, 240: 18733, 15: 5128, 5: 628}
- **TF Seleccionados**: {60: 132, 15: 9, 240: 2884, 5: 3}
- **DistATR** - Candidatos: avg=5.3 | Seleccionados: avg=8.6
- **Razones de selección**: {'Fallback<15': 2176, 'InBand[10,15]_TFPreference': 852}
- **En banda [10,15] ATR**: 1312/54984 (2.4%)

### Take Profit (TP)
- Zonas analizadas: 3034 | Total candidatos: 72312 | Seleccionados: 3034
- Candidatos por zona (promedio): 23.8
- **Edad (barras)** - Candidatos: med=74, max=2147483647 | Seleccionados: med=54, max=108
- **Score** - Candidatos: avg=0.53 | Seleccionados: avg=0.25
- **Priority Candidatos**: {'P3': 72312}
- **Priority Seleccionados**: {'P4_Fallback': 144, 'P3': 2890}
- **Type Candidatos**: {'Swing': 72312}
- **Type Seleccionados**: {'Calculated': 144, 'Swing': 2890}
- **TF Candidatos**: {5: 33851, 240: 16112, 15: 14770, 60: 7579}
- **TF Seleccionados**: {-1: 144, 240: 2890}
- **DistATR** - Candidatos: avg=5.0 | Seleccionados: avg=17.0
- **RR** - Candidatos: avg=0.55 | Seleccionados: avg=1.60
- **Razones de selección**: {'NoStructuralTarget': 144, 'SwingP3_TF>=60_RR>=Min_Dist>=12': 2890}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- KeptAligned ratio≈ 0.33.