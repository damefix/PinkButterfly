# Informe Diagnóstico de Logs - 2025-11-11 08:20:40

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_075929.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_075929.csv`

## DFM
- Eventos de evaluación: 188
- Evaluaciones Bull: 43 | Bear: 177
- Pasaron umbral (PassedThreshold): 205
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:6, 5:19, 6:16, 7:63, 8:116, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2187
- KeptAligned: 1847/1847 | KeptCounter: 1044/1044
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.380 | AvgProxCounter≈ 0.243
  - AvgDistATRAligned≈ 0.53 | AvgDistATRCounter≈ 0.37
- PreferAligned eventos: 925 | Filtradas contra-bias: 324

### Proximity (Pre-PreferAligned)
- Eventos: 2187
- Aligned pre: 1847/2891 | Counter pre: 1044/2891
- AvgProxAligned(pre)≈ 0.380 | AvgDistATRAligned(pre)≈ 0.53

### Proximity Drivers
- Eventos: 2187
- Alineadas: n=1847 | BaseProx≈ 0.918 | ZoneATR≈ 5.08 | SizePenalty≈ 0.977 | FinalProx≈ 0.897
- Contra-bias: n=720 | BaseProx≈ 0.833 | ZoneATR≈ 4.97 | SizePenalty≈ 0.977 | FinalProx≈ 0.814

## Risk
- Eventos: 1349
- Accepted=221 | RejSL=0 | RejTP=0 | RejRR=157 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 32 (18.2% del total)
  - Avg Score: 0.67 | Avg R:R: 2.02 | Avg DistATR: 7.66
  - Por TF: TF5=14, TF15=18
- **P0_SWING_LITE:** 144 (81.8% del total)
  - Avg Score: 0.33 | Avg R:R: 9.02 | Avg DistATR: 9.66
  - Por TF: TF15=77, TF60=67


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 71 | Unmatched: 150
- 0-10: Wins=28 Losses=43 WR=39.4% (n=71)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=28 Losses=43 WR=39.4% (n=71)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 221 | Aligned=146 (66.1%)
- Core≈ 1.00 | Prox≈ 0.88 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.29 | Confidence≈ 0.00
- SL_TF dist: {'15': 176, '60': 2, '5': 43} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 78, '5': 43, '60': 84, '240': 16} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=221, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=43, 15m=176, 60m=2, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.29 (n=221), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 41933 | Zonas con Anchors: 41921
- Dir zonas (zona): Bull=7839 Bear=32930 Neutral=1164
- Resumen por ciclo (promedios): TotHZ≈ 3.6, WithAnchors≈ 3.5, DirBull≈ 1.3, DirBear≈ 2.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 40757, 'tie-bias': 1164, 'triggers-only': 12}
- TF Triggers: {'15': 3913, '5': 4977}
- TF Anchors: {'60': 8842, '240': 5387, '1440': 214}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,21': 1, 'score decayó a 0,38': 1, 'score decayó a 0,30': 1, 'score decayó a 0,48': 1, 'score decayó a 0,49': 1, 'estructura no existe': 6, 'score decayó a 0,29': 1, 'score decayó a 0,18': 1, 'score decayó a 0,43': 1}
- Cancel_BOS (diag): por acción {'BUY': 3, 'SELL': 3} | por bias {'Bullish': 3, 'Bearish': 3, 'Neutral': 0}

## CSV de Trades
- Filas: 151 | Ejecutadas: 47 | Canceladas: 0 | Expiradas: 0
- BUY: 63 | SELL: 135

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 205
- Registered: 84
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 27 | SKIP_CONCURRENCY: 59
- Intentos de registro: 170

### TRADE DEDUP - Zonas y Persistencia
- Top 10 Zonas más deduplicadas (IDENTICAL):

| ZoneID | Duplicados | % del Total | Key Típica |
|--------|------------:|------------:|-----------:|
| 33555c2c-d168-4f73-8820-5580504b4366 | 8 | 29.6% | 6912,00/6907,74/6917,75 |
| 3b0bd37f-aeda-433c-85e4-9617eefde5f9 | 6 | 22.2% | 6751,00/6785,50/6698,00 |
| e9d44478-8c64-471d-b29e-803908df82c6 | 2 | 7.4% | 6694,25/6723,74/6623,00 |
| 8061e0fd-dc14-444f-a7e0-4fcc84f7172a | 1 | 3.7% | 6662,50/6699,42/6605,25 |
| 5e50552a-ced0-4180-b1c6-f1a6107d8ac3 | 1 | 3.7% | 6695,25/6723,15/6623,00 |
| 81507a13-09b8-4e27-8406-3d9179bd39f9 | 1 | 3.7% | 6717,25/6725,67/6702,00 |
| 2d425907-afd8-4f99-8f5c-eee73925c8f7 | 1 | 3.7% | 6648,50/6706,94/6599,75 |
| f482965b-0f20-425f-845c-b25d7d4a25f3 | 1 | 3.7% | 6648,50/6707,00/6599,75 |
| ea2644bb-f41e-4191-bcdb-1d3b9c8834eb | 1 | 3.7% | 6912,75/6907,67/6917,75 |
| a5c750f4-8894-4df1-a0de-8b52db5213dd | 1 | 3.7% | 6918,00/6898,95/6955,00 |

- Distribución de DeltaBars (IDENTICAL):

| DeltaBars | Cantidad | % |
|-----------|---------:|---:|
| 0 | 0 | 0.0% |
| 1 | 11 | 40.7% |
| 2-5 | 12 | 44.4% |
| 6-12 | 4 | 14.8% |
| >12 | 0 | 0.0% |

- IDENTICAL por Acción: {'BUY': 10, 'SELL': 17}
- IDENTICAL por DomTF: {'60': 8, '5': 11, '15': 3, '240': 5}

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 82.9%
- RegRate = Registered / Intentos = 49.4%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 15.9%
- Concurrency = SKIP_CONCURRENCY / Intentos = 34.7%
- ExecRate = Ejecutadas / Registered = 56.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 558 | Total candidatos: 10109 | Seleccionados: 0
- Candidatos por zona (promedio): 18.1

### Take Profit (TP)
- Zonas analizadas: 558 | Total candidatos: 4612 | Seleccionados: 0
- Candidatos por zona (promedio): 8.3
- **Edad (barras)** - Candidatos: med=53, max=250 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 4612}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 4612}
- **Type Seleccionados**: {}
- **TF Candidatos**: {60: 1437, 5: 1311, 15: 1241, 240: 623}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=9.1 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=4.01 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.