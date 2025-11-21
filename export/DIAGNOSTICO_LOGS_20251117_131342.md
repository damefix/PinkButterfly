# Informe Diagnóstico de Logs - 2025-11-17 13:22:06

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_131342.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251117_131342.csv`

## DFM
- Eventos de evaluación: 741
- Evaluaciones Bull: 225 | Bear: 151
- Pasaron umbral (PassedThreshold): 376
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:44, 6:216, 7:90, 8:26, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 25412
- KeptAligned: 1503/1503 | KeptCounter: 2532/2765
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.030 | AvgProxCounter≈ 0.032
  - AvgDistATRAligned≈ 0.13 | AvgDistATRCounter≈ 0.19
- PreferAligned eventos: 943 | Filtradas contra-bias: 649

### Proximity (Pre-PreferAligned)
- Eventos: 25412
- Aligned pre: 1503/4035 | Counter pre: 2532/4035
- AvgProxAligned(pre)≈ 0.030 | AvgDistATRAligned(pre)≈ 0.13

### Proximity Drivers
- Eventos: 25412
- Alineadas: n=1503 | BaseProx≈ 0.713 | ZoneATR≈ 5.21 | SizePenalty≈ 0.978 | FinalProx≈ 0.697
- Contra-bias: n=1883 | BaseProx≈ 0.476 | ZoneATR≈ 6.93 | SizePenalty≈ 0.950 | FinalProx≈ 0.450

## Risk
- Eventos: 2330
- Accepted=776 | RejSL=0 | RejTP=0 | RejRR=465 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 43 (2.2% del total)
  - Avg Score: 0.41 | Avg R:R: 2.83 | Avg DistATR: 3.69
  - Por TF: TF15=43
- **P0_SWING_LITE:** 1956 (97.8% del total)
  - Avg Score: 0.57 | Avg R:R: 4.04 | Avg DistATR: 3.55
  - Por TF: TF15=170, TF60=1786


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 224 | Unmatched: 552
- 0-10: Wins=30 Losses=194 WR=13.4% (n=224)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=30 Losses=194 WR=13.4% (n=224)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 776 | Aligned=394 (50.8%)
- Core≈ 1.00 | Prox≈ 0.59 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.95 | Confidence≈ 0.00
- SL_TF dist: {'15': 286, '5': 97, '1440': 4, '60': 389} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 176, '240': 145, '60': 63, '15': 392} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=776, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=97, 15m=286, 60m=389, 240m=0, 1440m=4
- RR plan por bandas: 0-10≈ 1.95 (n=776), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 77715 | Zonas con Anchors: 77715
- Dir zonas (zona): Bull=31566 Bear=38854 Neutral=7295
- Resumen por ciclo (promedios): TotHZ≈ 3.0, WithAnchors≈ 3.0, DirBull≈ 1.2, DirBear≈ 1.5, DirNeutral≈ 0.3
- Razones de dirección: {'anchors+triggers': 69072, 'tie-bias': 8643}
- TF Triggers: {'5': 64228, '15': 13487}
- TF Anchors: {'60': 77715, '240': 77715, '1440': 77715}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,47': 2, 'score decayó a 0,37': 1}

## CSV de Trades
- Filas: 46 | Ejecutadas: 13 | Canceladas: 0 | Expiradas: 0
- BUY: 17 | SELL: 42

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 376
- Registered: 23
  - DEDUP_COOLDOWN: 51 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 25
- Intentos de registro: 99

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 26.3%
- RegRate = Registered / Intentos = 23.2%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 51.5%
- Concurrency = SKIP_CONCURRENCY / Intentos = 25.3%
- ExecRate = Ejecutadas / Registered = 56.5%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 2924 | Total candidatos: 28004 | Seleccionados: 0
- Candidatos por zona (promedio): 9.6

### Take Profit (TP)
- Zonas analizadas: 2924 | Total candidatos: 41496 | Seleccionados: 2924
- Candidatos por zona (promedio): 14.2
- **Edad (barras)** - Candidatos: med=37, max=92 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.43 | Seleccionados: avg=0.66
- **Priority Candidatos**: {'P3': 8204}
- **Priority Seleccionados**: {'P3': 1493, 'NA': 759, 'P0': 672}
- **Type Candidatos**: {'Swing': 8204}
- **Type Seleccionados**: {'P3_Swing': 1493, 'P4_Fallback': 759, 'P0_Zone': 672}
- **TF Candidatos**: {240: 7166, 60: 992, 15: 46}
- **TF Seleccionados**: {240: 510, 60: 376, -1: 759, 15: 1017, 5: 262}
- **DistATR** - Candidatos: avg=18.1 | Seleccionados: avg=7.0
- **RR** - Candidatos: avg=6.41 | Seleccionados: avg=1.42
- **Razones de selección**: {'BestIntelligentScore': 2924}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- KeptAligned ratio≈ 1.00.