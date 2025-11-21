# Informe Diagnóstico de Logs - 2025-11-07 19:58:21

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251107_195626.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251107_195626.csv`

## DFM
- Eventos de evaluación: 18
- Evaluaciones Bull: 0 | Bear: 18
- Pasaron umbral (PassedThreshold): 18
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:0, 6:0, 7:0, 8:18, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 43
- KeptAligned: 0/0 | KeptCounter: 54/64
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.000 | AvgProxCounter≈ 0.671
  - AvgDistATRAligned≈ 0.00 | AvgDistATRCounter≈ 1.18
- PreferAligned eventos: 0 | Filtradas contra-bias: 0

### Proximity (Pre-PreferAligned)
- Eventos: 43
- Aligned pre: 0/54 | Counter pre: 54/54
- AvgProxAligned(pre)≈ 0.000 | AvgDistATRAligned(pre)≈ 0.00

### Proximity Drivers
- Eventos: 43
- Contra-bias: n=54 | BaseProx≈ 0.703 | ZoneATR≈ 4.82 | SizePenalty≈ 0.987 | FinalProx≈ 0.697

## Risk
- Eventos: 39
- Accepted=18 | RejSL=0 | RejTP=0 | RejRR=0 | RejEntry=0
### TP Policy (V6.0c)
- **FORCED_P3:** 0 (0.0%)
- **P4_FALLBACK:** 54 (100.0%)


### SLPick por Bandas y TF
- Bandas: lt8=18, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=0, 15m=0, 60m=18, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.00 (n=18), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 2304 | Zonas con Anchors: 2304
- Dir zonas (zona): Bull=11 Bear=2293 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 0.9, WithAnchors≈ 0.9, DirBull≈ 0.0, DirBear≈ 0.9, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 2304}
- TF Triggers: {'15': 40, '5': 24}
- TF Anchors: {'60': 64, '240': 57, '1440': 9}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)

## CSV de Trades
- Filas: 1 | Ejecutadas: 0 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 1

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 18
- Registered: 1
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 5 | SKIP_CONCURRENCY: 10
- Intentos de registro: 16

### TRADE DEDUP - Zonas y Persistencia
- Top 10 Zonas más deduplicadas (IDENTICAL):

| ZoneID | Duplicados | % del Total | Key Típica |
|--------|------------:|------------:|-----------:|
| 7fe4a362-eaad-46e4-8665-23931f1063f7 | 3 | 60.0% | 6742,50/6851,19/6633,81 |
| cb33fe42-cf83-4e46-b7d6-9c6d42c5bd22 | 1 | 20.0% | 6742,25/6851,20/6633,30 |
| 276c7db3-dea0-4938-879e-028fcb306ce0 | 1 | 20.0% | 6742,50/6851,15/6633,85 |

- Distribución de DeltaBars (IDENTICAL):

| DeltaBars | Cantidad | % |
|-----------|---------:|---:|
| 0 | 0 | 0.0% |
| 1 | 1 | 20.0% |
| 2-5 | 3 | 60.0% |
| 6-12 | 1 | 20.0% |
| >12 | 0 | 0.0% |

- IDENTICAL por Acción: {'BUY': 0, 'SELL': 5}
- IDENTICAL por DomTF: {'1440': 5}

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 88.9%
- RegRate = Registered / Intentos = 6.2%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 31.2%
- Concurrency = SKIP_CONCURRENCY / Intentos = 62.5%
- ExecRate = Ejecutadas / Registered = 0.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 54 | Total candidatos: 1238 | Seleccionados: 54
- Candidatos por zona (promedio): 22.9
- **Edad (barras)** - Candidatos: med=34, max=96 | Seleccionados: med=55, max=68
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.41
- **TF Candidatos**: {240: 486, 60: 385, 15: 293, 1440: 54, 5: 20}
- **TF Seleccionados**: {60: 39, 240: 5, 1440: 10}
- **DistATR** - Candidatos: avg=9.6 | Seleccionados: avg=11.3
- **Razones de selección**: {'InBand[8,15]_TFPreference': 54}
- **En banda [10,15] ATR**: 315/1238 (25.4%)

### Take Profit (TP)
- Zonas analizadas: 54 | Total candidatos: 97 | Seleccionados: 54
- Candidatos por zona (promedio): 1.8
- **Edad (barras)** - Candidatos: med=13, max=57 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 97}
- **Priority Seleccionados**: {'P4_Fallback': 54}
- **Type Candidatos**: {'Swing': 97}
- **Type Seleccionados**: {'Calculated': 54}
- **TF Candidatos**: {15: 46, 5: 29, 60: 14, 240: 8}
- **TF Seleccionados**: {-1: 54}
- **DistATR** - Candidatos: avg=2.0 | Seleccionados: avg=15.2
- **RR** - Candidatos: avg=0.14 | Seleccionados: avg=1.00
- **Razones de selección**: {'NoStructuralTarget': 54}

### 🎯 Recomendaciones
- ⚠️ SL: 63% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 100% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.