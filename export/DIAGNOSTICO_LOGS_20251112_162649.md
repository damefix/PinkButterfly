# Informe Diagnóstico de Logs - 2025-11-12 16:31:50

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_162649.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251112_162649.csv`

## DFM
- Eventos de evaluación: 964
- Evaluaciones Bull: 428 | Bear: 877
- Pasaron umbral (PassedThreshold): 1136
- ConfidenceBins acumulado: 0:0, 1:0, 2:8, 3:25, 4:84, 5:275, 6:498, 7:363, 8:52, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2345
- KeptAligned: 4202/4202 | KeptCounter: 2535/2635
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.447 | AvgProxCounter≈ 0.235
  - AvgDistATRAligned≈ 1.53 | AvgDistATRCounter≈ 1.09
- PreferAligned eventos: 1291 | Filtradas contra-bias: 573

### Proximity (Pre-PreferAligned)
- Eventos: 2345
- Aligned pre: 4202/6737 | Counter pre: 2535/6737
- AvgProxAligned(pre)≈ 0.447 | AvgDistATRAligned(pre)≈ 1.53

### Proximity Drivers
- Eventos: 2345
- Alineadas: n=4202 | BaseProx≈ 0.753 | ZoneATR≈ 5.21 | SizePenalty≈ 0.974 | FinalProx≈ 0.735
- Contra-bias: n=1962 | BaseProx≈ 0.551 | ZoneATR≈ 5.04 | SizePenalty≈ 0.976 | FinalProx≈ 0.540

## Risk
- Eventos: 1956
- Accepted=1305 | RejSL=0 | RejTP=0 | RejRR=1272 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 313 (11.0% del total)
  - Avg Score: 0.39 | Avg R:R: 1.91 | Avg DistATR: 3.75
  - Por TF: TF5=79, TF15=234
- **P0_SWING_LITE:** 2533 (89.0% del total)
  - Avg Score: 0.57 | Avg R:R: 4.16 | Avg DistATR: 3.46
  - Por TF: TF15=534, TF60=1999


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 130 | Unmatched: 1201
- 0-10: Wins=26 Losses=104 WR=20.0% (n=130)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=26 Losses=104 WR=20.0% (n=130)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1331 | Aligned=831 (62.4%)
- Core≈ 1.00 | Prox≈ 0.68 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.03 | Confidence≈ 0.00
- SL_TF dist: {'60': 152, '5': 185, '15': 990, '240': 4} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 313, '5': 340, '15': 500, '240': 178} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1305, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=180, 15m=973, 60m=148, 240m=4, 1440m=0
- RR plan por bandas: 0-10≈ 2.03 (n=1305), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10189 | Zonas con Anchors: 10175
- Dir zonas (zona): Bull=3845 Bear=5981 Neutral=363
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.4, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9749, 'tie-bias': 426, 'triggers-only': 14}
- TF Triggers: {'15': 4731, '5': 5458}
- TF Anchors: {'60': 10102, '240': 5906, '1440': 721}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,17': 1, 'estructura no existe': 31, 'score decayó a 0,27': 1, 'score decayó a 0,46': 1, 'score decayó a 0,42': 2, 'score decayó a 0,29': 1, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,34': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 268 | Ejecutadas: 52 | Canceladas: 0 | Expiradas: 0
- BUY: 155 | SELL: 165

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 1136
- Registered: 140
  - DEDUP_COOLDOWN: 24 | DEDUP_IDENTICAL: 146 | SKIP_CONCURRENCY: 131
- Intentos de registro: 441

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 38.8%
- RegRate = Registered / Intentos = 31.7%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 38.5%
- Concurrency = SKIP_CONCURRENCY / Intentos = 29.7%
- ExecRate = Ejecutadas / Registered = 37.1%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5429 | Total candidatos: 43347 | Seleccionados: 0
- Candidatos por zona (promedio): 8.0

### Take Profit (TP)
- Zonas analizadas: 5340 | Total candidatos: 50920 | Seleccionados: 5340
- Candidatos por zona (promedio): 9.5
- **Edad (barras)** - Candidatos: med=41, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 50920}
- **Priority Seleccionados**: {'P3': 3634, 'NA': 1369, 'P0': 337}
- **Type Candidatos**: {'Swing': 50920}
- **Type Seleccionados**: {'P3_Swing': 3634, 'P4_Fallback': 1369, 'P0_Zone': 337}
- **TF Candidatos**: {5: 14960, 15: 13718, 60: 13381, 240: 8861}
- **TF Seleccionados**: {60: 1009, 5: 1000, 15: 1245, -1: 1369, 240: 717}
- **DistATR** - Candidatos: avg=8.7 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.44 | Seleccionados: avg=1.31
- **Razones de selección**: {'BestIntelligentScore': 5340}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.