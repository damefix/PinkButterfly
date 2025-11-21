# Informe Diagnóstico de Logs - 2025-11-13 15:48:46

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_154521.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_154521.csv`

## DFM
- Eventos de evaluación: 944
- Evaluaciones Bull: 160 | Bear: 682
- Pasaron umbral (PassedThreshold): 842
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:68, 6:375, 7:350, 8:49, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2355
- KeptAligned: 4136/4136 | KeptCounter: 2804/2908
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.443 | AvgProxCounter≈ 0.230
  - AvgDistATRAligned≈ 1.51 | AvgDistATRCounter≈ 1.15
- PreferAligned eventos: 1276 | Filtradas contra-bias: 576

### Proximity (Pre-PreferAligned)
- Eventos: 2355
- Aligned pre: 4136/6940 | Counter pre: 2804/6940
- AvgProxAligned(pre)≈ 0.443 | AvgDistATRAligned(pre)≈ 1.51

### Proximity Drivers
- Eventos: 2355
- Alineadas: n=4136 | BaseProx≈ 0.751 | ZoneATR≈ 5.18 | SizePenalty≈ 0.975 | FinalProx≈ 0.733
- Contra-bias: n=2228 | BaseProx≈ 0.520 | ZoneATR≈ 4.88 | SizePenalty≈ 0.977 | FinalProx≈ 0.510

## Risk
- Eventos: 1953
- Accepted=1280 | RejSL=0 | RejTP=0 | RejRR=1229 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 310 (11.0% del total)
  - Avg Score: 0.39 | Avg R:R: 1.89 | Avg DistATR: 3.77
  - Por TF: TF5=79, TF15=231
- **P0_SWING_LITE:** 2521 (89.0% del total)
  - Avg Score: 0.58 | Avg R:R: 4.07 | Avg DistATR: 3.49
  - Por TF: TF15=600, TF60=1921


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 85 | Unmatched: 1232
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
- Muestras: 1317 | Aligned=788 (59.8%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.09 | Confidence≈ 0.00
- SL_TF dist: {'15': 951, '60': 138, '5': 209, '240': 19} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 501, '5': 362, '60': 279, '240': 175} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1280, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=201, 15m=930, 60m=134, 240m=15, 1440m=0
- RR plan por bandas: 0-10≈ 2.07 (n=1280), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10362 | Zonas con Anchors: 10348
- Dir zonas (zona): Bull=3760 Bear=6272 Neutral=330
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9947, 'tie-bias': 401, 'triggers-only': 14}
- TF Triggers: {'5': 5447, '15': 4915}
- TF Anchors: {'60': 10274, '240': 5971, '1440': 542}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 27, 'score decayó a 0,27': 1, 'score decayó a 0,46': 1, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,42': 1, 'score decayó a 0,32': 1, 'score decayó a 0,34': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 203 | Ejecutadas: 37 | Canceladas: 0 | Expiradas: 0
- BUY: 71 | SELL: 169

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 842
- Registered: 107
  - DEDUP_COOLDOWN: 18 | DEDUP_IDENTICAL: 99 | SKIP_CONCURRENCY: 99
- Intentos de registro: 323

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 38.4%
- RegRate = Registered / Intentos = 33.1%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 36.2%
- Concurrency = SKIP_CONCURRENCY / Intentos = 30.7%
- ExecRate = Ejecutadas / Registered = 34.6%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5452 | Total candidatos: 42627 | Seleccionados: 0
- Candidatos por zona (promedio): 7.8

### Take Profit (TP)
- Zonas analizadas: 5347 | Total candidatos: 51802 | Seleccionados: 5347
- Candidatos por zona (promedio): 9.7
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 51802}
- **Priority Seleccionados**: {'P3': 3642, 'NA': 1390, 'P0': 315}
- **Type Candidatos**: {'Swing': 51802}
- **Type Seleccionados**: {'P3_Swing': 3642, 'P4_Fallback': 1390, 'P0_Zone': 315}
- **TF Candidatos**: {5: 15553, 15: 14196, 60: 13469, 240: 8584}
- **TF Seleccionados**: {60: 977, 5: 1018, 15: 1265, -1: 1390, 240: 697}
- **DistATR** - Candidatos: avg=8.6 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.56 | Seleccionados: avg=1.32
- **Razones de selección**: {'BestIntelligentScore': 5347}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.