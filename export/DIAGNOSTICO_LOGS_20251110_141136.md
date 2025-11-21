# Informe Diagnóstico de Logs - 2025-11-10 14:31:03

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251110_141136.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251110_141136.csv`

## DFM
- Eventos de evaluación: 1886
- Evaluaciones Bull: 1354 | Bear: 967
- Pasaron umbral (PassedThreshold): 2321
- ConfidenceBins acumulado: 0:0, 1:0, 2:6, 3:43, 4:154, 5:365, 6:710, 7:798, 8:245, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5150
- KeptAligned: 10828/11256 | KeptCounter: 7460/9270
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.491 | AvgProxCounter≈ 0.360
  - AvgDistATRAligned≈ 3.79 | AvgDistATRCounter≈ 2.32
- PreferAligned eventos: 3884 | Filtradas contra-bias: 4259

### Proximity (Pre-PreferAligned)
- Eventos: 5150
- Aligned pre: 10828/18288 | Counter pre: 7460/18288
- AvgProxAligned(pre)≈ 0.491 | AvgDistATRAligned(pre)≈ 3.79

### Proximity Drivers
- Eventos: 5150
- Alineadas: n=10828 | BaseProx≈ 0.698 | ZoneATR≈ 6.63 | SizePenalty≈ 0.954 | FinalProx≈ 0.669
- Contra-bias: n=3201 | BaseProx≈ 0.565 | ZoneATR≈ 6.12 | SizePenalty≈ 0.957 | FinalProx≈ 0.544

## Risk
- Eventos: 5003
- Accepted=2415 | RejSL=0 | RejTP=0 | RejRR=3686 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 1379 (41.3% del total)
  - Avg Score: 0.64 | Avg R:R: 1.98 | Avg DistATR: 9.46
  - Por TF: TF5=929, TF15=450
- **P0_SWING_LITE:** 1957 (58.7% del total)
  - Avg Score: 0.40 | Avg R:R: 6.96 | Avg DistATR: 9.43
  - Por TF: TF15=1422, TF60=535


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 1591 | Unmatched: 824
- 0-10: Wins=587 Losses=985 WR=37.3% (n=1572)
- 10-15: Wins=11 Losses=8 WR=57.9% (n=19)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=598 Losses=993 WR=37.6% (n=1591)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2415 | Aligned=1687 (69.9%)
- Core≈ 0.95 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.61 | Confidence≈ 0.00
- SL_TF dist: {'15': 1844, '5': 466, '60': 105} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 530, '15': 1330, '60': 555} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=2359, 8-10=34, 10-12.5=14, 12.5-15=8, >15=0
- TF: 5m=466, 15m=1844, 60m=105, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.62 (n=2393), 10-15≈ 1.29 (n=22)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 51258 | Zonas con Anchors: 13329
- Dir zonas (zona): Bull=22049 Bear=26795 Neutral=2414
- Resumen por ciclo (promedios): TotHZ≈ 3.5, WithAnchors≈ 1.2, DirBull≈ 1.9, DirBear≈ 1.4, DirNeutral≈ 0.2
- Razones de dirección: {'triggers-only': 36427, 'tie-bias': 2414, 'anchors+triggers': 12417}
- TF Triggers: {'5': 11946, '15': 8580}
- TF Anchors: {'60': 7094}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Cancel_BOS (diag): por acción {'BUY': 73, 'SELL': 138} | por bias {'Bullish': 138, 'Bearish': 73, 'Neutral': 0}

## CSV de Trades
- Filas: 3225 | Ejecutadas: 1282 | Canceladas: 0 | Expiradas: 0
- BUY: 2694 | SELL: 1813

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 2321
- Registered: 1732
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 1732

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 74.6%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 74.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 11085 | Total candidatos: 116594 | Seleccionados: 0
- Candidatos por zona (promedio): 10.5

### Take Profit (TP)
- Zonas analizadas: 11085 | Total candidatos: 93930 | Seleccionados: 0
- Candidatos por zona (promedio): 8.5
- **Edad (barras)** - Candidatos: med=51, max=302 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 93930}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 93930}
- **Type Seleccionados**: {}
- **TF Candidatos**: {5: 42366, 15: 34231, 60: 17333}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=7.5 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=2.78 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- KeptAligned ratio≈ 0.96.