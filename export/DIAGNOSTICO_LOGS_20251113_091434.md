# Informe Diagnóstico de Logs - 2025-11-13 09:20:18

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_091434.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_091434.csv`

## DFM
- Eventos de evaluación: 940
- Evaluaciones Bull: 164 | Bear: 684
- Pasaron umbral (PassedThreshold): 848
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:71, 6:368, 7:359, 8:50, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2356
- KeptAligned: 4155/4155 | KeptCounter: 2733/2839
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.445 | AvgProxCounter≈ 0.231
  - AvgDistATRAligned≈ 1.53 | AvgDistATRCounter≈ 1.13
- PreferAligned eventos: 1282 | Filtradas contra-bias: 569

### Proximity (Pre-PreferAligned)
- Eventos: 2356
- Aligned pre: 4155/6888 | Counter pre: 2733/6888
- AvgProxAligned(pre)≈ 0.445 | AvgDistATRAligned(pre)≈ 1.53

### Proximity Drivers
- Eventos: 2356
- Alineadas: n=4155 | BaseProx≈ 0.751 | ZoneATR≈ 5.17 | SizePenalty≈ 0.975 | FinalProx≈ 0.733
- Contra-bias: n=2164 | BaseProx≈ 0.529 | ZoneATR≈ 4.84 | SizePenalty≈ 0.978 | FinalProx≈ 0.520

## Risk
- Eventos: 1942
- Accepted=1269 | RejSL=0 | RejTP=0 | RejRR=1258 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 313 (10.9% del total)
  - Avg Score: 0.39 | Avg R:R: 1.89 | Avg DistATR: 3.74
  - Por TF: TF5=79, TF15=234
- **P0_SWING_LITE:** 2556 (89.1% del total)
  - Avg Score: 0.57 | Avg R:R: 4.17 | Avg DistATR: 3.48
  - Por TF: TF15=546, TF60=2010


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 85 | Unmatched: 1224
- 0-10: Wins=37 Losses=48 WR=43.5% (n=85)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=37 Losses=48 WR=43.5% (n=85)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1309 | Aligned=811 (62.0%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.09 | Confidence≈ 0.00
- SL_TF dist: {'60': 154, '15': 952, '5': 192, '240': 11} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 291, '5': 353, '15': 495, '240': 170} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1269, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=184, 15m=928, 60m=150, 240m=7, 1440m=0
- RR plan por bandas: 0-10≈ 2.07 (n=1269), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10322 | Zonas con Anchors: 10308
- Dir zonas (zona): Bull=3768 Bear=6215 Neutral=339
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9905, 'tie-bias': 403, 'triggers-only': 14}
- TF Triggers: {'5': 5460, '15': 4862}
- TF Anchors: {'60': 10235, '240': 5972, '1440': 453}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,46': 1, 'estructura no existe': 25, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,42': 1, 'score decayó a 0,34': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 192 | Ejecutadas: 35 | Canceladas: 0 | Expiradas: 0
- BUY: 69 | SELL: 158

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 848
- Registered: 101
  - DEDUP_COOLDOWN: 21 | DEDUP_IDENTICAL: 106 | SKIP_CONCURRENCY: 97
- Intentos de registro: 325

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 38.3%
- RegRate = Registered / Intentos = 31.1%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 39.1%
- Concurrency = SKIP_CONCURRENCY / Intentos = 29.8%
- ExecRate = Ejecutadas / Registered = 34.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5446 | Total candidatos: 43289 | Seleccionados: 0
- Candidatos por zona (promedio): 7.9

### Take Profit (TP)
- Zonas analizadas: 5336 | Total candidatos: 51185 | Seleccionados: 5336
- Candidatos por zona (promedio): 9.6
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 51185}
- **Priority Seleccionados**: {'P3': 3656, 'NA': 1361, 'P0': 319}
- **Type Candidatos**: {'Swing': 51185}
- **Type Seleccionados**: {'P3_Swing': 3656, 'P4_Fallback': 1361, 'P0_Zone': 319}
- **TF Candidatos**: {5: 15394, 15: 13940, 60: 13665, 240: 8186}
- **TF Seleccionados**: {60: 1003, 5: 1026, -1: 1361, 15: 1244, 240: 702}
- **DistATR** - Candidatos: avg=8.6 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.57 | Seleccionados: avg=1.33
- **Razones de selección**: {'BestIntelligentScore': 5336}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.