# Informe Diagnóstico de Logs - 2025-11-19 10:08:04

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251119_100411.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_100411.csv`

## DFM
- Eventos de evaluación: 272
- Evaluaciones Bull: 0 | Bear: 301
- Pasaron umbral (PassedThreshold): 301
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:19, 6:149, 7:102, 8:31, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 623
- KeptAligned: 1275/1275 | KeptCounter: 907/942
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.314 | AvgProxCounter≈ 0.245
  - AvgDistATRAligned≈ 1.12 | AvgDistATRCounter≈ 1.37
- PreferAligned eventos: 235 | Filtradas contra-bias: 47

### Proximity (Pre-PreferAligned)
- Eventos: 623
- Aligned pre: 1275/2182 | Counter pre: 907/2182
- AvgProxAligned(pre)≈ 0.314 | AvgDistATRAligned(pre)≈ 1.12

### Proximity Drivers
- Eventos: 623
- Alineadas: n=1275 | BaseProx≈ 0.732 | ZoneATR≈ 4.62 | SizePenalty≈ 0.979 | FinalProx≈ 0.716
- Contra-bias: n=860 | BaseProx≈ 0.480 | ZoneATR≈ 5.10 | SizePenalty≈ 0.975 | FinalProx≈ 0.468

## Risk
- Eventos: 539
- Accepted=457 | RejSL=0 | RejTP=0 | RejRR=322 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 295 (19.5% del total)
  - Avg Score: 0.39 | Avg R:R: 1.81 | Avg DistATR: 3.92
  - Por TF: TF5=54, TF15=241
- **P0_SWING_LITE:** 1219 (80.5% del total)
  - Avg Score: 0.67 | Avg R:R: 4.05 | Avg DistATR: 3.80
  - Por TF: TF15=215, TF60=1004


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 22 | Unmatched: 435
- 0-10: Wins=8 Losses=14 WR=36.4% (n=22)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=8 Losses=14 WR=36.4% (n=22)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 457 | Aligned=300 (65.6%)
- Core≈ 1.00 | Prox≈ 0.68 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.00 | Confidence≈ 0.00
- SL_TF dist: {'15': 153, '5': 288, '60': 3, '240': 13} | SL_Structural≈ 100.0%
- TP_TF dist: {'240': 85, '15': 66, '5': 229, '60': 77} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=457, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=288, 15m=153, 60m=3, 240m=13, 1440m=0
- RR plan por bandas: 0-10≈ 2.00 (n=457), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 4220 | Zonas con Anchors: 4220
- Dir zonas (zona): Bull=148 Bear=3955 Neutral=117
- Resumen por ciclo (promedios): TotHZ≈ 6.7, WithAnchors≈ 6.7, DirBull≈ 0.2, DirBear≈ 6.3, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 4053, 'tie-bias': 167}
- TF Triggers: {'5': 1480, '15': 2740}
- TF Anchors: {'60': 4220, '240': 4220, '1440': 4220}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 5, 'score decayó a 0,29': 1}

## CSV de Trades
- Filas: 79 | Ejecutadas: 13 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 92

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 301
- Registered: 44
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 44

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 14.6%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 29.5%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 1926 | Total candidatos: 37654 | Seleccionados: 0
- Candidatos por zona (promedio): 19.6

### Take Profit (TP)
- Zonas analizadas: 1926 | Total candidatos: 50576 | Seleccionados: 1926
- Candidatos por zona (promedio): 26.3
- **Edad (barras)** - Candidatos: med=124, max=684 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.54 | Seleccionados: avg=0.71
- **Priority Candidatos**: {'P3': 50576}
- **Priority Seleccionados**: {'P3': 1328, 'NA': 442, 'P0': 156}
- **Type Candidatos**: {'Swing': 50576}
- **Type Seleccionados**: {'P3_Swing': 1328, 'P4_Fallback': 442, 'P0_Zone': 156}
- **TF Candidatos**: {15: 19463, 240: 16241, 5: 7868, 60: 7004}
- **TF Seleccionados**: {240: 464, 15: 264, 5: 519, -1: 442, 60: 237}
- **DistATR** - Candidatos: avg=15.8 | Seleccionados: avg=4.6
- **RR** - Candidatos: avg=6.87 | Seleccionados: avg=1.42
- **Razones de selección**: {'BestIntelligentScore': 1926}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.