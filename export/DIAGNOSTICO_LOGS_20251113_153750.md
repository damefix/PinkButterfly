# Informe Diagnóstico de Logs - 2025-11-13 15:41:08

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_153750.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_153750.csv`

## DFM
- Eventos de evaluación: 946
- Evaluaciones Bull: 160 | Bear: 680
- Pasaron umbral (PassedThreshold): 840
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:65, 6:376, 7:349, 8:50, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2354
- KeptAligned: 4132/4132 | KeptCounter: 2808/2912
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.443 | AvgProxCounter≈ 0.231
  - AvgDistATRAligned≈ 1.51 | AvgDistATRCounter≈ 1.15
- PreferAligned eventos: 1275 | Filtradas contra-bias: 577

### Proximity (Pre-PreferAligned)
- Eventos: 2354
- Aligned pre: 4132/6940 | Counter pre: 2808/6940
- AvgProxAligned(pre)≈ 0.443 | AvgDistATRAligned(pre)≈ 1.51

### Proximity Drivers
- Eventos: 2354
- Alineadas: n=4132 | BaseProx≈ 0.752 | ZoneATR≈ 5.17 | SizePenalty≈ 0.975 | FinalProx≈ 0.734
- Contra-bias: n=2231 | BaseProx≈ 0.521 | ZoneATR≈ 4.88 | SizePenalty≈ 0.977 | FinalProx≈ 0.512

## Risk
- Eventos: 1951
- Accepted=1278 | RejSL=0 | RejTP=0 | RejRR=1225 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 311 (11.0% del total)
  - Avg Score: 0.39 | Avg R:R: 1.89 | Avg DistATR: 3.77
  - Por TF: TF5=80, TF15=231
- **P0_SWING_LITE:** 2527 (89.0% del total)
  - Avg Score: 0.58 | Avg R:R: 4.11 | Avg DistATR: 3.49
  - Por TF: TF15=600, TF60=1927


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 85 | Unmatched: 1231
- 0-10: Wins=36 Losses=49 WR=42.4% (n=85)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=36 Losses=49 WR=42.4% (n=85)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1316 | Aligned=789 (60.0%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.10 | Confidence≈ 0.00
- SL_TF dist: {'15': 947, '60': 144, '5': 207, '240': 18} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 502, '5': 361, '60': 277, '240': 176} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1278, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=199, 15m=925, 60m=140, 240m=14, 1440m=0
- RR plan por bandas: 0-10≈ 2.09 (n=1278), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10363 | Zonas con Anchors: 10349
- Dir zonas (zona): Bull=3767 Bear=6268 Neutral=328
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9950, 'tie-bias': 399, 'triggers-only': 14}
- TF Triggers: {'5': 5443, '15': 4920}
- TF Anchors: {'60': 10275, '240': 5968, '1440': 536}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,27': 1, 'score decayó a 0,46': 1, 'estructura no existe': 26, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,42': 1, 'score decayó a 0,32': 1, 'score decayó a 0,34': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 203 | Ejecutadas: 37 | Canceladas: 0 | Expiradas: 0
- BUY: 71 | SELL: 169

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 840
- Registered: 107
  - DEDUP_COOLDOWN: 18 | DEDUP_IDENTICAL: 97 | SKIP_CONCURRENCY: 99
- Intentos de registro: 321

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 38.2%
- RegRate = Registered / Intentos = 33.3%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 35.8%
- Concurrency = SKIP_CONCURRENCY / Intentos = 30.8%
- ExecRate = Ejecutadas / Registered = 34.6%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5448 | Total candidatos: 42664 | Seleccionados: 0
- Candidatos por zona (promedio): 7.8

### Take Profit (TP)
- Zonas analizadas: 5342 | Total candidatos: 51694 | Seleccionados: 5342
- Candidatos por zona (promedio): 9.7
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 51694}
- **Priority Seleccionados**: {'P3': 3632, 'NA': 1394, 'P0': 316}
- **Type Candidatos**: {'Swing': 51694}
- **Type Seleccionados**: {'P3_Swing': 3632, 'P4_Fallback': 1394, 'P0_Zone': 316}
- **TF Candidatos**: {5: 15531, 15: 14171, 60: 13448, 240: 8544}
- **TF Seleccionados**: {60: 975, 5: 1021, 15: 1255, -1: 1394, 240: 697}
- **DistATR** - Candidatos: avg=8.7 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.61 | Seleccionados: avg=1.33
- **Razones de selección**: {'BestIntelligentScore': 5342}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.