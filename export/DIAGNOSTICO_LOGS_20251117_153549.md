# Informe Diagnóstico de Logs - 2025-11-17 15:44:23

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_153549.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251117_153549.csv`

## DFM
- Eventos de evaluación: 590
- Evaluaciones Bull: 58 | Bear: 197
- Pasaron umbral (PassedThreshold): 255
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:42, 6:112, 7:84, 8:17, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 22405
- KeptAligned: 1201/1201 | KeptCounter: 2426/2572
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.025 | AvgProxCounter≈ 0.031
  - AvgDistATRAligned≈ 0.10 | AvgDistATRCounter≈ 0.15
- PreferAligned eventos: 663 | Filtradas contra-bias: 766

### Proximity (Pre-PreferAligned)
- Eventos: 22405
- Aligned pre: 1201/3627 | Counter pre: 2426/3627
- AvgProxAligned(pre)≈ 0.025 | AvgDistATRAligned(pre)≈ 0.10

### Proximity Drivers
- Eventos: 22405
- Alineadas: n=1201 | BaseProx≈ 0.717 | ZoneATR≈ 5.01 | SizePenalty≈ 0.973 | FinalProx≈ 0.697
- Contra-bias: n=1660 | BaseProx≈ 0.499 | ZoneATR≈ 6.13 | SizePenalty≈ 0.961 | FinalProx≈ 0.477

## Risk
- Eventos: 1648
- Accepted=724 | RejSL=0 | RejTP=0 | RejRR=425 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 14 (0.8% del total)
  - Avg Score: 0.40 | Avg R:R: 1.95 | Avg DistATR: 4.00
  - Por TF: TF5=14
- **P0_SWING_LITE:** 1664 (99.2% del total)
  - Avg Score: 0.59 | Avg R:R: 9.77 | Avg DistATR: 3.79
  - Por TF: TF15=208, TF60=1456


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 227 | Unmatched: 497
- 0-10: Wins=173 Losses=54 WR=76.2% (n=227)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=173 Losses=54 WR=76.2% (n=227)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 724 | Aligned=218 (30.1%)
- Core≈ 1.00 | Prox≈ 0.53 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 3.78 | Confidence≈ 0.00
- SL_TF dist: {'15': 666, '5': 7, '1440': 1, '60': 50} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 175, '15': 334, '60': 145, '240': 70} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=724, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=7, 15m=666, 60m=50, 240m=0, 1440m=1
- RR plan por bandas: 0-10≈ 3.78 (n=724), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 107361 | Zonas con Anchors: 107361
- Dir zonas (zona): Bull=46804 Bear=51310 Neutral=9247
- Resumen por ciclo (promedios): TotHZ≈ 4.6, WithAnchors≈ 4.6, DirBull≈ 2.0, DirBear≈ 2.2, DirNeutral≈ 0.4
- Razones de dirección: {'anchors+triggers': 96482, 'tie-bias': 10879}
- TF Triggers: {'5': 93380, '15': 13981}
- TF Anchors: {'60': 107361, '240': 107361, '1440': 107361}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,48': 5, 'score decayó a 0,32': 7, 'score decayó a 0,47': 4, 'score decayó a 0,22': 2}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 88 | Ejecutadas: 12 | Canceladas: 0 | Expiradas: 0
- BUY: 12 | SELL: 88

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 255
- Registered: 44
  - DEDUP_COOLDOWN: 88 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 4
- Intentos de registro: 136

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 53.3%
- RegRate = Registered / Intentos = 32.4%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 64.7%
- Concurrency = SKIP_CONCURRENCY / Intentos = 2.9%
- ExecRate = Ejecutadas / Registered = 27.3%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 2537 | Total candidatos: 24250 | Seleccionados: 0
- Candidatos por zona (promedio): 9.6

### Take Profit (TP)
- Zonas analizadas: 2536 | Total candidatos: 42810 | Seleccionados: 2536
- Candidatos por zona (promedio): 16.9
- **Edad (barras)** - Candidatos: med=28, max=92 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.69
- **Priority Candidatos**: {'P3': 5261}
- **Priority Seleccionados**: {'P3': 2008, 'NA': 419, 'P0': 109}
- **Type Candidatos**: {'Swing': 5261}
- **Type Seleccionados**: {'P3_Swing': 2008, 'P4_Fallback': 419, 'P0_Zone': 109}
- **TF Candidatos**: {240: 3801, 60: 773, 15: 550, 5: 137}
- **TF Seleccionados**: {240: 601, 60: 331, -1: 419, 5: 424, 15: 761}
- **DistATR** - Candidatos: avg=13.6 | Seleccionados: avg=7.3
- **RR** - Candidatos: avg=15.10 | Seleccionados: avg=2.14
- **Razones de selección**: {'BestIntelligentScore': 2536}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.