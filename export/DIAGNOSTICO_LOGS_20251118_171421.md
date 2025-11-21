# Informe Diagnóstico de Logs - 2025-11-18 17:17:21

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251118_171421.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251118_171421.csv`

## DFM
- Eventos de evaluación: 173
- Evaluaciones Bull: 0 | Bear: 135
- Pasaron umbral (PassedThreshold): 135
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:0, 6:38, 7:57, 8:40, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 616
- KeptAligned: 735/735 | KeptCounter: 652/692
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.315 | AvgProxCounter≈ 0.255
  - AvgDistATRAligned≈ 1.08 | AvgDistATRCounter≈ 1.48
- PreferAligned eventos: 235 | Filtradas contra-bias: 54

### Proximity (Pre-PreferAligned)
- Eventos: 616
- Aligned pre: 735/1387 | Counter pre: 652/1387
- AvgProxAligned(pre)≈ 0.315 | AvgDistATRAligned(pre)≈ 1.08

### Proximity Drivers
- Eventos: 616
- Alineadas: n=735 | BaseProx≈ 0.768 | ZoneATR≈ 5.63 | SizePenalty≈ 0.973 | FinalProx≈ 0.747
- Contra-bias: n=598 | BaseProx≈ 0.474 | ZoneATR≈ 5.51 | SizePenalty≈ 0.970 | FinalProx≈ 0.461

## Risk
- Eventos: 533
- Accepted=197 | RejSL=0 | RejTP=0 | RejRR=225 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 58 (7.6% del total)
  - Avg Score: 0.39 | Avg R:R: 1.99 | Avg DistATR: 4.17
  - Por TF: TF5=41, TF15=17
- **P0_SWING_LITE:** 707 (92.4% del total)
  - Avg Score: 0.60 | Avg R:R: 4.87 | Avg DistATR: 3.67
  - Por TF: TF15=228, TF60=479


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 4 | Unmatched: 193
- 0-10: Wins=1 Losses=3 WR=25.0% (n=4)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=1 Losses=3 WR=25.0% (n=4)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 197 | Aligned=107 (54.3%)
- Core≈ 1.00 | Prox≈ 0.65 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.00 | Confidence≈ 0.00
- SL_TF dist: {'60': 44, '15': 31, '5': 106, '240': 16} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 33, '15': 21, '5': 116, '240': 27} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=197, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=106, 15m=31, 60m=44, 240m=16, 1440m=0
- RR plan por bandas: 0-10≈ 2.00 (n=197), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 2420 | Zonas con Anchors: 2420
- Dir zonas (zona): Bull=310 Bear=2051 Neutral=59
- Resumen por ciclo (promedios): TotHZ≈ 3.9, WithAnchors≈ 3.9, DirBull≈ 0.5, DirBear≈ 3.3, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 2330, 'tie-bias': 90}
- TF Triggers: {'15': 851, '5': 1569}
- TF Anchors: {'60': 2311, '240': 2420, '1440': 2420}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 4}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 33} | por bias {'Bullish': 33, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 82 | Ejecutadas: 3 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 85

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 135
- Registered: 42
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 42

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 31.1%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 7.1%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 1209 | Total candidatos: 15652 | Seleccionados: 0
- Candidatos por zona (promedio): 12.9

### Take Profit (TP)
- Zonas analizadas: 1209 | Total candidatos: 23055 | Seleccionados: 1209
- Candidatos por zona (promedio): 19.1
- **Edad (barras)** - Candidatos: med=225, max=661 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.66
- **Priority Candidatos**: {'P3': 23055}
- **Priority Seleccionados**: {'P3': 754, 'NA': 392, 'P0': 63}
- **Type Candidatos**: {'Swing': 23055}
- **Type Seleccionados**: {'P3_Swing': 754, 'P4_Fallback': 392, 'P0_Zone': 63}
- **TF Candidatos**: {15: 13966, 240: 4041, 5: 2967, 60: 2081}
- **TF Seleccionados**: {60: 115, -1: 392, 15: 169, 5: 320, 240: 213}
- **DistATR** - Candidatos: avg=18.4 | Seleccionados: avg=6.4
- **RR** - Candidatos: avg=7.59 | Seleccionados: avg=1.30
- **Razones de selección**: {'BestIntelligentScore': 1209}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.