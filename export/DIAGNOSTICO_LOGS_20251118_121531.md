# Informe Diagnóstico de Logs - 2025-11-18 12:20:25

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251118_121531.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251118_121531.csv`

## DFM
- Eventos de evaluación: 579
- Evaluaciones Bull: 58 | Bear: 424
- Pasaron umbral (PassedThreshold): 482
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:11, 6:136, 7:233, 8:102, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2595
- KeptAligned: 2836/2836 | KeptCounter: 3095/3235
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.300 | AvgProxCounter≈ 0.228
  - AvgDistATRAligned≈ 0.95 | AvgDistATRCounter≈ 1.24
- PreferAligned eventos: 948 | Filtradas contra-bias: 455

### Proximity (Pre-PreferAligned)
- Eventos: 2595
- Aligned pre: 2836/5931 | Counter pre: 3095/5931
- AvgProxAligned(pre)≈ 0.300 | AvgDistATRAligned(pre)≈ 0.95

### Proximity Drivers
- Eventos: 2595
- Alineadas: n=2836 | BaseProx≈ 0.764 | ZoneATR≈ 4.61 | SizePenalty≈ 0.980 | FinalProx≈ 0.750
- Contra-bias: n=2640 | BaseProx≈ 0.485 | ZoneATR≈ 4.85 | SizePenalty≈ 0.976 | FinalProx≈ 0.474

## Risk
- Eventos: 1887
- Accepted=747 | RejSL=0 | RejTP=0 | RejRR=879 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 278 (11.2% del total)
  - Avg Score: 0.39 | Avg R:R: 1.86 | Avg DistATR: 3.90
  - Por TF: TF5=45, TF15=233
- **P0_SWING_LITE:** 2202 (88.8% del total)
  - Avg Score: 589479922738830208.00 | Avg R:R: 4.31 | Avg DistATR: 3.94
  - Por TF: TF15=642, TF60=1560


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 114 | Unmatched: 646
- 0-10: Wins=58 Losses=56 WR=50.9% (n=114)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=58 Losses=56 WR=50.9% (n=114)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 760 | Aligned=390 (51.3%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.60 | Confidence≈ 0.00
- SL_TF dist: {'15': 534, '60': 114, '5': 74, '240': 38} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 426, '240': 187, '60': 53, '5': 94} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=747, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=72, 15m=527, 60m=114, 240m=34, 1440m=0
- RR plan por bandas: 0-10≈ 2.57 (n=747), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 12318 | Zonas con Anchors: 12318
- Dir zonas (zona): Bull=1410 Bear=10613 Neutral=295
- Resumen por ciclo (promedios): TotHZ≈ 3.9, WithAnchors≈ 3.9, DirBull≈ 0.5, DirBear≈ 3.4, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 11914, 'tie-bias': 404}
- TF Triggers: {'15': 8439, '5': 3879}
- TF Anchors: {'60': 10401, '240': 12318, '1440': 12318}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,47': 2, 'estructura no existe': 16, 'score decayó a 0,46': 1, 'score decayó a 0,19': 1, 'score decayó a 0,22': 1, 'score decayó a 0,24': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 131 | Ejecutadas: 25 | Canceladas: 0 | Expiradas: 0
- BUY: 22 | SELL: 134

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 482
- Registered: 67
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 67

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 13.9%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 37.3%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4674 | Total candidatos: 38344 | Seleccionados: 0
- Candidatos por zona (promedio): 8.2

### Take Profit (TP)
- Zonas analizadas: 4648 | Total candidatos: 98943 | Seleccionados: 4648
- Candidatos por zona (promedio): 21.3
- **Edad (barras)** - Candidatos: med=32, max=157 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=397840910727123712.00
- **Priority Candidatos**: {'P3': 33356}
- **Priority Seleccionados**: {'NA': 520, 'P3': 3995, 'P0': 133}
- **Type Candidatos**: {'Swing': 33356}
- **Type Seleccionados**: {'P4_Fallback': 520, 'P3_Swing': 3995, 'P0_Zone': 133}
- **TF Candidatos**: {240: 13509, 15: 10732, 60: 4712, 5: 4403}
- **TF Seleccionados**: {-1: 520, 15: 2414, 240: 1273, 60: 140, 5: 301}
- **DistATR** - Candidatos: avg=10.5 | Seleccionados: avg=8.7
- **RR** - Candidatos: avg=5.29 | Seleccionados: avg=3.61
- **Razones de selección**: {'BestIntelligentScore': 4648}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.