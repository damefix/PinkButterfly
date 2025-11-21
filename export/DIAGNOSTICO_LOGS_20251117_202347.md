# Informe Diagnóstico de Logs - 2025-11-17 20:26:43

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_202347.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251117_202347.csv`

## DFM
- Eventos de evaluación: 546
- Evaluaciones Bull: 0 | Bear: 572
- Pasaron umbral (PassedThreshold): 572
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:8, 6:150, 7:246, 8:168, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2536
- KeptAligned: 2596/2596 | KeptCounter: 1633/1717
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.242 | AvgProxCounter≈ 0.105
  - AvgDistATRAligned≈ 0.74 | AvgDistATRCounter≈ 0.46
- PreferAligned eventos: 678 | Filtradas contra-bias: 69

### Proximity (Pre-PreferAligned)
- Eventos: 2536
- Aligned pre: 2596/4229 | Counter pre: 1633/4229
- AvgProxAligned(pre)≈ 0.242 | AvgDistATRAligned(pre)≈ 0.74

### Proximity Drivers
- Eventos: 2536
- Alineadas: n=2596 | BaseProx≈ 0.796 | ZoneATR≈ 5.44 | SizePenalty≈ 0.968 | FinalProx≈ 0.770
- Contra-bias: n=1564 | BaseProx≈ 0.610 | ZoneATR≈ 6.31 | SizePenalty≈ 0.953 | FinalProx≈ 0.578

## Risk
- Eventos: 1186
- Accepted=710 | RejSL=0 | RejTP=0 | RejRR=1089 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 28 (1.8% del total)
  - Avg Score: 0.41 | Avg R:R: 1.60 | Avg DistATR: 4.62
  - Por TF: TF5=16, TF15=12
- **P0_SWING_LITE:** 1499 (98.2% del total)
  - Avg Score: 0.67 | Avg R:R: 3.22 | Avg DistATR: 4.05
  - Por TF: TF15=458, TF60=1041


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 370 | Unmatched: 340
- 0-10: Wins=294 Losses=76 WR=79.5% (n=370)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=294 Losses=76 WR=79.5% (n=370)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 710 | Aligned=432 (60.8%)
- Core≈ 1.00 | Prox≈ 0.70 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.65 | Confidence≈ 0.00
- SL_TF dist: {'15': 652, '1440': 6, '240': 4, '60': 48} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 324, '5': 82, '60': 62, '240': 242} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=710, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=0, 15m=652, 60m=48, 240m=4, 1440m=6
- RR plan por bandas: 0-10≈ 1.65 (n=710), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 11105 | Zonas con Anchors: 11105
- Dir zonas (zona): Bull=60 Bear=10126 Neutral=919
- Resumen por ciclo (promedios): TotHZ≈ 2.2, WithAnchors≈ 2.2, DirBull≈ 0.0, DirBear≈ 2.0, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 9832, 'tie-bias': 1273}
- TF Triggers: {'5': 6685, '15': 4420}
- TF Anchors: {'60': 11105, '240': 11105, '1440': 11105}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,27': 1}

## CSV de Trades
- Filas: 31 | Ejecutadas: 8 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 39

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 572
- Registered: 16
  - DEDUP_COOLDOWN: 47 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 78
- Intentos de registro: 141

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 24.7%
- RegRate = Registered / Intentos = 11.3%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 33.3%
- Concurrency = SKIP_CONCURRENCY / Intentos = 55.3%
- ExecRate = Ejecutadas / Registered = 50.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 3831 | Total candidatos: 27373 | Seleccionados: 0
- Candidatos por zona (promedio): 7.1

### Take Profit (TP)
- Zonas analizadas: 3831 | Total candidatos: 47886 | Seleccionados: 3831
- Candidatos por zona (promedio): 12.5
- **Edad (barras)** - Candidatos: med=20, max=93 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.57 | Seleccionados: avg=0.72
- **Priority Candidatos**: {'P3': 6640}
- **Priority Seleccionados**: {'P3': 3814, 'NA': 17}
- **Type Candidatos**: {'Swing': 6640}
- **Type Seleccionados**: {'P3_Swing': 3814, 'P4_Fallback': 17}
- **TF Candidatos**: {240: 4585, 60: 1193, 15: 521, 5: 341}
- **TF Seleccionados**: {15: 1112, 240: 1856, 5: 477, -1: 17, 60: 369}
- **DistATR** - Candidatos: avg=7.1 | Seleccionados: avg=5.7
- **RR** - Candidatos: avg=1.97 | Seleccionados: avg=1.06
- **Razones de selección**: {'BestIntelligentScore': 3831}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.