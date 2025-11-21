# Informe Diagnóstico de Logs - 2025-11-14 17:25:02

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_172008.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251114_172008.csv`

## DFM
- Eventos de evaluación: 893
- Evaluaciones Bull: 100 | Bear: 665
- Pasaron umbral (PassedThreshold): 765
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:44, 6:314, 7:313, 8:94, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2354
- KeptAligned: 3299/3299 | KeptCounter: 3246/3392
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.352 | AvgProxCounter≈ 0.253
  - AvgDistATRAligned≈ 1.27 | AvgDistATRCounter≈ 1.35
- PreferAligned eventos: 1046 | Filtradas contra-bias: 510

### Proximity (Pre-PreferAligned)
- Eventos: 2354
- Aligned pre: 3299/6545 | Counter pre: 3246/6545
- AvgProxAligned(pre)≈ 0.352 | AvgDistATRAligned(pre)≈ 1.27

### Proximity Drivers
- Eventos: 2354
- Alineadas: n=3299 | BaseProx≈ 0.757 | ZoneATR≈ 5.00 | SizePenalty≈ 0.977 | FinalProx≈ 0.741
- Contra-bias: n=2736 | BaseProx≈ 0.500 | ZoneATR≈ 4.97 | SizePenalty≈ 0.977 | FinalProx≈ 0.490

## Risk
- Eventos: 1941
- Accepted=1212 | RejSL=0 | RejTP=0 | RejRR=1281 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 320 (11.1% del total)
  - Avg Score: 0.39 | Avg R:R: 1.93 | Avg DistATR: 3.81
  - Por TF: TF5=96, TF15=224
- **P0_SWING_LITE:** 2552 (88.9% del total)
  - Avg Score: 0.55 | Avg R:R: 4.78 | Avg DistATR: 3.66
  - Por TF: TF15=491, TF60=2061


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 69 | Unmatched: 1183
- 0-10: Wins=28 Losses=41 WR=40.6% (n=69)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=28 Losses=41 WR=40.6% (n=69)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1252 | Aligned=668 (53.4%)
- Core≈ 1.00 | Prox≈ 0.64 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.04 | Confidence≈ 0.00
- SL_TF dist: {'60': 162, '5': 166, '15': 891, '240': 33} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 234, '15': 428, '5': 294, '240': 296} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1212, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=160, 15m=866, 60m=159, 240m=27, 1440m=0
- RR plan por bandas: 0-10≈ 2.02 (n=1212), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10479 | Zonas con Anchors: 10470
- Dir zonas (zona): Bull=2837 Bear=7302 Neutral=340
- Resumen por ciclo (promedios): TotHZ≈ 4.2, WithAnchors≈ 4.2, DirBull≈ 1.1, DirBear≈ 2.9, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 10040, 'tie-bias': 430, 'triggers-only': 9}
- TF Triggers: {'5': 5524, '15': 4955}
- TF Anchors: {'60': 10394, '240': 9826, '1440': 8585}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 15, 'score decayó a 0,21': 2, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,22': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 166 | Ejecutadas: 40 | Canceladas: 0 | Expiradas: 0
- BUY: 43 | SELL: 163

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 765
- Registered: 88
  - DEDUP_COOLDOWN: 13 | DEDUP_IDENTICAL: 63 | SKIP_CONCURRENCY: 101
- Intentos de registro: 265

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 34.6%
- RegRate = Registered / Intentos = 33.2%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 28.7%
- Concurrency = SKIP_CONCURRENCY / Intentos = 38.1%
- ExecRate = Ejecutadas / Registered = 45.5%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5049 | Total candidatos: 41260 | Seleccionados: 0
- Candidatos por zona (promedio): 8.2

### Take Profit (TP)
- Zonas analizadas: 4981 | Total candidatos: 77866 | Seleccionados: 4981
- Candidatos por zona (promedio): 15.6
- **Edad (barras)** - Candidatos: med=36, max=192 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.50 | Seleccionados: avg=0.69
- **Priority Candidatos**: {'P3': 77866}
- **Priority Seleccionados**: {'P3': 3466, 'NA': 1306, 'P0': 209}
- **Type Candidatos**: {'Swing': 77866}
- **Type Seleccionados**: {'P3_Swing': 3466, 'P4_Fallback': 1306, 'P0_Zone': 209}
- **TF Candidatos**: {240: 34862, 60: 14968, 5: 14169, 15: 13867}
- **TF Seleccionados**: {60: 646, 15: 1061, -1: 1306, 5: 889, 240: 1079}
- **DistATR** - Candidatos: avg=14.6 | Seleccionados: avg=5.2
- **RR** - Candidatos: avg=6.39 | Seleccionados: avg=1.38
- **Razones de selección**: {'BestIntelligentScore': 4981}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.