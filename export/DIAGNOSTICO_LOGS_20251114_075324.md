# Informe Diagnóstico de Logs - 2025-11-14 07:56:13

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_075324.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251114_075324.csv`

## DFM
- Eventos de evaluación: 921
- Evaluaciones Bull: 141 | Bear: 679
- Pasaron umbral (PassedThreshold): 820
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:67, 6:347, 7:346, 8:60, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2355
- KeptAligned: 4198/4198 | KeptCounter: 2715/2813
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.449 | AvgProxCounter≈ 0.222
  - AvgDistATRAligned≈ 1.56 | AvgDistATRCounter≈ 1.11
- PreferAligned eventos: 1296 | Filtradas contra-bias: 553

### Proximity (Pre-PreferAligned)
- Eventos: 2355
- Aligned pre: 4198/6913 | Counter pre: 2715/6913
- AvgProxAligned(pre)≈ 0.449 | AvgDistATRAligned(pre)≈ 1.56

### Proximity Drivers
- Eventos: 2355
- Alineadas: n=4198 | BaseProx≈ 0.750 | ZoneATR≈ 5.16 | SizePenalty≈ 0.975 | FinalProx≈ 0.732
- Contra-bias: n=2162 | BaseProx≈ 0.521 | ZoneATR≈ 4.86 | SizePenalty≈ 0.978 | FinalProx≈ 0.511

## Risk
- Eventos: 1954
- Accepted=1243 | RejSL=0 | RejTP=0 | RejRR=1291 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 333 (11.7% del total)
  - Avg Score: 0.39 | Avg R:R: 1.90 | Avg DistATR: 3.80
  - Por TF: TF5=88, TF15=245
- **P0_SWING_LITE:** 2515 (88.3% del total)
  - Avg Score: 0.59 | Avg R:R: 4.10 | Avg DistATR: 3.51
  - Por TF: TF15=576, TF60=1939


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 76 | Unmatched: 1206
- 0-10: Wins=37 Losses=39 WR=48.7% (n=76)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=37 Losses=39 WR=48.7% (n=76)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1282 | Aligned=782 (61.0%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.10 | Confidence≈ 0.00
- SL_TF dist: {'60': 159, '5': 205, '15': 898, '240': 20} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 292, '5': 338, '15': 476, '240': 176} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1243, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=197, 15m=875, 60m=155, 240m=16, 1440m=0
- RR plan por bandas: 0-10≈ 2.08 (n=1243), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10380 | Zonas con Anchors: 10366
- Dir zonas (zona): Bull=3705 Bear=6374 Neutral=301
- Resumen por ciclo (promedios): TotHZ≈ 4.2, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 10012, 'tie-bias': 354, 'triggers-only': 14}
- TF Triggers: {'5': 5486, '15': 4894}
- TF Anchors: {'60': 10292, '240': 6074, '1440': 171}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 27, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,42': 1, 'score decayó a 0,32': 1, 'score decayó a 0,34': 1, 'score decayó a 0,22': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 196 | Ejecutadas: 35 | Canceladas: 0 | Expiradas: 0
- BUY: 62 | SELL: 169

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 820
- Registered: 103
  - DEDUP_COOLDOWN: 11 | DEDUP_IDENTICAL: 94 | SKIP_CONCURRENCY: 99
- Intentos de registro: 307

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 37.4%
- RegRate = Registered / Intentos = 33.6%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 34.2%
- Concurrency = SKIP_CONCURRENCY / Intentos = 32.2%
- ExecRate = Ejecutadas / Registered = 34.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5487 | Total candidatos: 43042 | Seleccionados: 0
- Candidatos por zona (promedio): 7.8

### Take Profit (TP)
- Zonas analizadas: 5386 | Total candidatos: 50502 | Seleccionados: 5386
- Candidatos por zona (promedio): 9.4
- **Edad (barras)** - Candidatos: med=38, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 50502}
- **Priority Seleccionados**: {'P3': 3690, 'NA': 1380, 'P0': 316}
- **Type Candidatos**: {'Swing': 50502}
- **Type Seleccionados**: {'P3_Swing': 3690, 'P4_Fallback': 1380, 'P0_Zone': 316}
- **TF Candidatos**: {5: 15587, 15: 14006, 60: 13369, 240: 7540}
- **TF Seleccionados**: {60: 1031, 5: 1011, -1: 1380, 15: 1234, 240: 730}
- **DistATR** - Candidatos: avg=8.2 | Seleccionados: avg=5.6
- **RR** - Candidatos: avg=3.47 | Seleccionados: avg=1.33
- **Razones de selección**: {'BestIntelligentScore': 5386}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.