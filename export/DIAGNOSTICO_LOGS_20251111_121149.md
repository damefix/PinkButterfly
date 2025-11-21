# Informe Diagnóstico de Logs - 2025-11-11 12:54:57

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_121149.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_121149.csv`

## DFM
- Eventos de evaluación: 208
- Evaluaciones Bull: 61 | Bear: 181
- Pasaron umbral (PassedThreshold): 85
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:20, 4:5, 5:61, 6:105, 7:50, 8:1, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2184
- KeptAligned: 1738/1738 | KeptCounter: 1177/1177
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.368 | AvgProxCounter≈ 0.254
  - AvgDistATRAligned≈ 0.52 | AvgDistATRCounter≈ 0.38
- PreferAligned eventos: 896 | Filtradas contra-bias: 316

### Proximity (Pre-PreferAligned)
- Eventos: 2184
- Aligned pre: 1738/2915 | Counter pre: 1177/2915
- AvgProxAligned(pre)≈ 0.368 | AvgDistATRAligned(pre)≈ 0.52

### Proximity Drivers
- Eventos: 2184
- Alineadas: n=1738 | BaseProx≈ 0.918 | ZoneATR≈ 5.15 | SizePenalty≈ 0.976 | FinalProx≈ 0.896
- Contra-bias: n=861 | BaseProx≈ 0.836 | ZoneATR≈ 4.78 | SizePenalty≈ 0.978 | FinalProx≈ 0.818

## Risk
- Eventos: 1354
- Accepted=243 | RejSL=0 | RejTP=0 | RejRR=95 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 32 (8.1% del total)
  - Avg Score: 0.42 | Avg R:R: 1.83 | Avg DistATR: 3.35
  - Por TF: TF5=10, TF15=22
- **P0_SWING_LITE:** 362 (91.9% del total)
  - Avg Score: 0.62 | Avg R:R: 3.64 | Avg DistATR: 3.36
  - Por TF: TF15=88, TF60=274


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 28 | Unmatched: 217
- 0-10: Wins=17 Losses=11 WR=60.7% (n=28)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=17 Losses=11 WR=60.7% (n=28)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 245 | Aligned=151 (61.6%)
- Core≈ 1.00 | Prox≈ 0.88 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.02 | Confidence≈ 0.00
- SL_TF dist: {'60': 1, '15': 195, '5': 49} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 51, '15': 107, '60': 75, '240': 12} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=243, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=48, 15m=194, 60m=1, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.03 (n=243), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 42180 | Zonas con Anchors: 42168
- Dir zonas (zona): Bull=8031 Bear=33116 Neutral=1033
- Resumen por ciclo (promedios): TotHZ≈ 3.6, WithAnchors≈ 3.5, DirBull≈ 1.3, DirBear≈ 2.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 41020, 'tie-bias': 1148, 'triggers-only': 12}
- TF Triggers: {'5': 4952, '15': 3933}
- TF Anchors: {'60': 8837, '240': 5419, '1440': 303}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,48': 1, 'estructura no existe': 2, 'score decayó a 0,29': 1}

## CSV de Trades
- Filas: 41 | Ejecutadas: 14 | Canceladas: 0 | Expiradas: 0
- BUY: 3 | SELL: 52

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 85
- Registered: 23
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 16 | SKIP_CONCURRENCY: 15
- Intentos de registro: 54

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 63.5%
- RegRate = Registered / Intentos = 42.6%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 29.6%
- Concurrency = SKIP_CONCURRENCY / Intentos = 27.8%
- ExecRate = Ejecutadas / Registered = 60.9%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 549 | Total candidatos: 9649 | Seleccionados: 0
- Candidatos por zona (promedio): 17.6

### Take Profit (TP)
- Zonas analizadas: 542 | Total candidatos: 4527 | Seleccionados: 0
- Candidatos por zona (promedio): 8.4
- **Edad (barras)** - Candidatos: med=53, max=250 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 4527}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 4527}
- **Type Seleccionados**: {}
- **TF Candidatos**: {60: 1382, 5: 1291, 15: 1234, 240: 620}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=8.9 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=4.00 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.