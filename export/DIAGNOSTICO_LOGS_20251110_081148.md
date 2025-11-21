# Informe Diagnóstico de Logs - 2025-11-10 08:12:12

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251110_081148.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251110_081148.csv`

## DFM
- Eventos de evaluación: 52
- Evaluaciones Bull: 0 | Bear: 73
- Pasaron umbral (PassedThreshold): 73
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:0, 6:22, 7:28, 8:23, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 114
- KeptAligned: 220/220 | KeptCounter: 4/4
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.816 | AvgProxCounter≈ 0.035
  - AvgDistATRAligned≈ 2.16 | AvgDistATRCounter≈ 0.00
- PreferAligned eventos: 110 | Filtradas contra-bias: 0

### Proximity (Pre-PreferAligned)
- Eventos: 114
- Aligned pre: 220/224 | Counter pre: 4/224
- AvgProxAligned(pre)≈ 0.816 | AvgDistATRAligned(pre)≈ 2.16

### Proximity Drivers
- Eventos: 114
- Alineadas: n=220 | BaseProx≈ 0.871 | ZoneATR≈ 4.29 | SizePenalty≈ 0.985 | FinalProx≈ 0.859
- Contra-bias: n=4 | BaseProx≈ 0.996 | ZoneATR≈ 2.40 | SizePenalty≈ 1.000 | FinalProx≈ 0.996

## Risk
- Eventos: 114
- Accepted=73 | RejSL=0 | RejTP=0 | RejRR=16 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 42 (67.7% del total)
  - Avg Score: 0.68 | Avg R:R: 1.99 | Avg DistATR: 8.47
  - Por TF: TF5=10, TF15=32
- **P0_SWING_LITE:** 20 (32.3% del total)
  - Avg Score: 0.48 | Avg R:R: 7.24 | Avg DistATR: 7.14
  - Por TF: TF15=20


### SLPick por Bandas y TF
- Bandas: lt8=73, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=0, 15m=73, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.59 (n=73), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 1010 | Zonas con Anchors: 1010
- Dir zonas (zona): Bull=3 Bear=1006 Neutral=1
- Resumen por ciclo (promedios): TotHZ≈ 2.1, WithAnchors≈ 2.1, DirBull≈ 0.0, DirBear≈ 2.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 1009, 'tie-bias': 1}
- TF Triggers: {'15': 211, '5': 38}
- TF Anchors: {'60': 249, '240': 247, '1440': 136}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,33': 1}

## CSV de Trades
- Filas: 3 | Ejecutadas: 0 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 3

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 73
- Registered: 2
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 3 | SKIP_CONCURRENCY: 4
- Intentos de registro: 9

### TRADE DEDUP - Zonas y Persistencia
- Top 10 Zonas más deduplicadas (IDENTICAL):

| ZoneID | Duplicados | % del Total | Key Típica |
|--------|------------:|------------:|-----------:|
| 53d15a46-afc7-4581-a34d-735dc9a77f54 | 3 | 100.0% | 6764,00/6772,80/6748,50 |

- Distribución de DeltaBars (IDENTICAL):

| DeltaBars | Cantidad | % |
|-----------|---------:|---:|
| 0 | 0 | 0.0% |
| 1 | 0 | 0.0% |
| 2-5 | 2 | 66.7% |
| 6-12 | 1 | 33.3% |
| >12 | 0 | 0.0% |

- IDENTICAL por Acción: {'BUY': 0, 'SELL': 3}
- IDENTICAL por DomTF: {'1440': 3}

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 12.3%
- RegRate = Registered / Intentos = 22.2%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 33.3%
- Concurrency = SKIP_CONCURRENCY / Intentos = 44.4%
- ExecRate = Ejecutadas / Registered = 0.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 216 | Total candidatos: 1525 | Seleccionados: 0
- Candidatos por zona (promedio): 7.1

### Take Profit (TP)
- Zonas analizadas: 216 | Total candidatos: 1119 | Seleccionados: 0
- Candidatos por zona (promedio): 5.2
- **Edad (barras)** - Candidatos: med=49, max=110 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 1119}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 1119}
- **Type Seleccionados**: {}
- **TF Candidatos**: {15: 595, 5: 275, 60: 162, 240: 87}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=6.3 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=2.66 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.