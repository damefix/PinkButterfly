# Informe Diagnóstico de Logs - 2025-11-19 13:42:02

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_133723.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_133723.csv`

## DFM
- Eventos de evaluación: 643
- Evaluaciones Bull: 0 | Bear: 555
- Pasaron umbral (PassedThreshold): 555
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:28, 6:216, 7:238, 8:73, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3093
- KeptAligned: 3233/3233 | KeptCounter: 2636/2800
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.170 | AvgProxCounter≈ 0.125
  - AvgDistATRAligned≈ 0.81 | AvgDistATRCounter≈ 0.74
- PreferAligned eventos: 758 | Filtradas contra-bias: 9

### Proximity (Pre-PreferAligned)
- Eventos: 3093
- Aligned pre: 3233/5869 | Counter pre: 2636/5869
- AvgProxAligned(pre)≈ 0.170 | AvgDistATRAligned(pre)≈ 0.81

### Proximity Drivers
- Eventos: 3093
- Alineadas: n=3233 | BaseProx≈ 0.708 | ZoneATR≈ 4.41 | SizePenalty≈ 0.983 | FinalProx≈ 0.696
- Contra-bias: n=2627 | BaseProx≈ 0.475 | ZoneATR≈ 5.62 | SizePenalty≈ 0.965 | FinalProx≈ 0.460

## Risk
- Eventos: 1602
- Accepted=872 | RejSL=0 | RejTP=0 | RejRR=1738 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 833 (20.6% del total)
  - Avg Score: 0.39 | Avg R:R: 1.87 | Avg DistATR: 4.01
  - Por TF: TF5=184, TF15=649
- **P0_SWING_LITE:** 3210 (79.4% del total)
  - Avg Score: 0.63 | Avg R:R: 4.11 | Avg DistATR: 3.83
  - Por TF: TF15=503, TF60=2707


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 51 | Unmatched: 821
- 0-10: Wins=39 Losses=12 WR=76.5% (n=51)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=39 Losses=12 WR=76.5% (n=51)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 872 | Aligned=482 (55.3%)
- Core≈ 1.00 | Prox≈ 0.60 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.27 | Confidence≈ 0.00
- SL_TF dist: {'15': 546, '5': 242, '60': 69, '240': 15} | SL_Structural≈ 100.0%
- TP_TF dist: {'240': 276, '15': 303, '60': 80, '5': 213} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=872, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=242, 15m=546, 60m=69, 240m=15, 1440m=0
- RR plan por bandas: 0-10≈ 2.27 (n=872), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 18523 | Zonas con Anchors: 18523
- Dir zonas (zona): Bull=100 Bear=18322 Neutral=101
- Resumen por ciclo (promedios): TotHZ≈ 5.9, WithAnchors≈ 5.9, DirBull≈ 0.0, DirBear≈ 5.9, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 18412, 'tie-bias': 111}
- TF Triggers: {'5': 8147, '15': 10376}
- TF Anchors: {'60': 18523, '240': 18523, '1440': 18523}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,26': 3, 'score decayó a 0,31': 2, 'estructura no existe': 4, 'score decayó a 0,15': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 11} | por bias {'Bullish': 11, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 76 | Ejecutadas: 4 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 80

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 555
- Registered: 44
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 44

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 7.9%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 9.1%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5619 | Total candidatos: 90121 | Seleccionados: 0
- Candidatos por zona (promedio): 16.0

### Take Profit (TP)
- Zonas analizadas: 5619 | Total candidatos: 134330 | Seleccionados: 5619
- Candidatos por zona (promedio): 23.9
- **Edad (barras)** - Candidatos: med=91, max=687 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.57 | Seleccionados: avg=0.74
- **Priority Candidatos**: {'P3': 78547}
- **Priority Seleccionados**: {'P3': 4583, 'NA': 608, 'P0': 428}
- **Type Candidatos**: {'Swing': 78547}
- **Type Seleccionados**: {'P3_Swing': 4583, 'P4_Fallback': 608, 'P0_Zone': 428}
- **TF Candidatos**: {240: 43854, 15: 15137, 60: 11542, 5: 8014}
- **TF Seleccionados**: {240: 2349, 15: 1388, -1: 608, 60: 414, 5: 860}
- **DistATR** - Candidatos: avg=15.9 | Seleccionados: avg=5.1
- **RR** - Candidatos: avg=6.96 | Seleccionados: avg=1.49
- **Razones de selección**: {'BestIntelligentScore': 5619}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.