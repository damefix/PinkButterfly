# Informe Diagnóstico de Logs - 2025-11-10 17:14:00

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251110_170101.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251110_170101.csv`

## DFM
- Eventos de evaluación: 3771
- Evaluaciones Bull: 2616 | Bear: 2597
- Pasaron umbral (PassedThreshold): 5213
- ConfidenceBins acumulado: 0:0, 1:0, 2:5, 3:71, 4:212, 5:676, 6:1562, 7:1962, 8:725, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 8738
- KeptAligned: 19631/20420 | KeptCounter: 11605/14594
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.501 | AvgProxCounter≈ 0.326
  - AvgDistATRAligned≈ 3.63 | AvgDistATRCounter≈ 2.09
- PreferAligned eventos: 6572 | Filtradas contra-bias: 6494

### Proximity (Pre-PreferAligned)
- Eventos: 8738
- Aligned pre: 19631/31236 | Counter pre: 11605/31236
- AvgProxAligned(pre)≈ 0.501 | AvgDistATRAligned(pre)≈ 3.63

### Proximity Drivers
- Eventos: 8738
- Alineadas: n=19631 | BaseProx≈ 0.715 | ZoneATR≈ 6.38 | SizePenalty≈ 0.957 | FinalProx≈ 0.687
- Contra-bias: n=5111 | BaseProx≈ 0.564 | ZoneATR≈ 6.13 | SizePenalty≈ 0.957 | FinalProx≈ 0.544

## Risk
- Eventos: 8360
- Accepted=5303 | RejSL=0 | RejTP=0 | RejRR=6492 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 2859 (40.7% del total)
  - Avg Score: 0.64 | Avg R:R: 1.97 | Avg DistATR: 9.42
  - Por TF: TF5=1977, TF15=882
- **P0_SWING_LITE:** 4174 (59.3% del total)
  - Avg Score: 0.39 | Avg R:R: 8.07 | Avg DistATR: 9.58
  - Por TF: TF15=2332, TF60=1842


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 25 | Unmatched: 5278
- 0-10: Wins=21 Losses=4 WR=84.0% (n=25)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=21 Losses=4 WR=84.0% (n=25)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 5303 | Aligned=4054 (76.4%)
- Core≈ 0.98 | Prox≈ 0.69 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.71 | Confidence≈ 0.00
- SL_TF dist: {'15': 4022, '5': 932, '60': 341, '240': 8} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 2170, '5': 1189, '60': 1627, '240': 317} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=5131, 8-10=92, 10-12.5=60, 12.5-15=18, >15=2
- TF: 5m=932, 15m=4022, 60m=341, 240m=8, 1440m=0
- RR plan por bandas: 0-10≈ 1.72 (n=5223), 10-15≈ 1.13 (n=78)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 99731 | Zonas con Anchors: 62022
- Dir zonas (zona): Bull=34003 Bear=61948 Neutral=3780
- Resumen por ciclo (promedios): TotHZ≈ 3.5, WithAnchors≈ 2.2, DirBull≈ 1.8, DirBear≈ 1.5, DirNeutral≈ 0.2
- Razones de dirección: {'triggers-only': 36178, 'tie-bias': 3780, 'anchors+triggers': 59773}
- TF Triggers: {'5': 20459, '15': 14555}
- TF Anchors: {'60': 21594, '240': 4950, '1440': 384}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 3} | por bias {'Bullish': 3, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 26 | Ejecutadas: 9 | Canceladas: 0 | Expiradas: 0
- BUY: 17 | SELL: 18

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 5213
- Registered: 14
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 3529
- Intentos de registro: 3543

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 68.0%
- RegRate = Registered / Intentos = 0.4%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 99.6%
- ExecRate = Ejecutadas / Registered = 64.3%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 20497 | Total candidatos: 213095 | Seleccionados: 0
- Candidatos por zona (promedio): 10.4

### Take Profit (TP)
- Zonas analizadas: 20497 | Total candidatos: 204054 | Seleccionados: 0
- Candidatos por zona (promedio): 10.0
- **Edad (barras)** - Candidatos: med=50, max=318 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 204054}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 204054}
- **Type Seleccionados**: {}
- **TF Candidatos**: {5: 82938, 15: 65663, 60: 48520, 240: 6933}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=8.1 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=3.07 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- KeptAligned ratio≈ 0.96.