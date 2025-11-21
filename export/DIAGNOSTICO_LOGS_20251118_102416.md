# Informe Diagnóstico de Logs - 2025-11-18 10:29:06

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251118_102416.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251118_102416.csv`

## DFM
- Eventos de evaluación: 721
- Evaluaciones Bull: 12 | Bear: 562
- Pasaron umbral (PassedThreshold): 574
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:10, 6:169, 7:265, 8:130, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2921
- KeptAligned: 2007/2007 | KeptCounter: 2398/2492
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.149 | AvgProxCounter≈ 0.141
  - AvgDistATRAligned≈ 0.61 | AvgDistATRCounter≈ 0.67
- PreferAligned eventos: 562 | Filtradas contra-bias: 150

### Proximity (Pre-PreferAligned)
- Eventos: 2921
- Aligned pre: 2007/4405 | Counter pre: 2398/4405
- AvgProxAligned(pre)≈ 0.149 | AvgDistATRAligned(pre)≈ 0.61

### Proximity Drivers
- Eventos: 2921
- Alineadas: n=2007 | BaseProx≈ 0.750 | ZoneATR≈ 4.54 | SizePenalty≈ 0.982 | FinalProx≈ 0.739
- Contra-bias: n=2248 | BaseProx≈ 0.511 | ZoneATR≈ 4.85 | SizePenalty≈ 0.978 | FinalProx≈ 0.500

## Risk
- Eventos: 1281
- Accepted=937 | RejSL=0 | RejTP=0 | RejRR=1144 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 208 (11.4% del total)
  - Avg Score: 0.38 | Avg R:R: 1.81 | Avg DistATR: 4.00
  - Por TF: TF5=38, TF15=170
- **P0_SWING_LITE:** 1623 (88.6% del total)
  - Avg Score: 0.65 | Avg R:R: 4.88 | Avg DistATR: 3.86
  - Por TF: TF15=344, TF60=1279


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 21 | Unmatched: 924
- 0-10: Wins=5 Losses=16 WR=23.8% (n=21)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=5 Losses=16 WR=23.8% (n=21)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 945 | Aligned=416 (44.0%)
- Core≈ 1.00 | Prox≈ 0.62 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.43 | Confidence≈ 0.00
- SL_TF dist: {'15': 351, '1440': 61, '60': 450, '240': 25, '5': 58} | SL_Structural≈ 100.0%
- TP_TF dist: {'240': 441, '15': 186, '60': 221, '5': 97} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=937, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=56, 15m=349, 60m=450, 240m=21, 1440m=61
- RR plan por bandas: 0-10≈ 2.40 (n=937), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 11129 | Zonas con Anchors: 11129
- Dir zonas (zona): Bull=372 Bear=10602 Neutral=155
- Resumen por ciclo (promedios): TotHZ≈ 3.6, WithAnchors≈ 3.6, DirBull≈ 0.1, DirBear≈ 3.4, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 10947, 'tie-bias': 182}
- TF Triggers: {'15': 7250, '5': 3879}
- TF Anchors: {'60': 11093, '240': 11129, '1440': 11129}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,45': 6, 'score decayó a 0,38': 2, 'estructura no existe': 24, 'score decayó a 0,19': 1, 'score decayó a 0,22': 1, 'score decayó a 0,24': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 129 | Ejecutadas: 12 | Canceladas: 0 | Expiradas: 0
- BUY: 7 | SELL: 134

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 574
- Registered: 67
  - DEDUP_COOLDOWN: 154 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 40
- Intentos de registro: 261

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 45.5%
- RegRate = Registered / Intentos = 25.7%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 59.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 15.3%
- ExecRate = Ejecutadas / Registered = 17.9%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 3808 | Total candidatos: 34190 | Seleccionados: 0
- Candidatos por zona (promedio): 9.0

### Take Profit (TP)
- Zonas analizadas: 3793 | Total candidatos: 63767 | Seleccionados: 3793
- Candidatos por zona (promedio): 16.8
- **Edad (barras)** - Candidatos: med=36, max=157 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.72
- **Priority Candidatos**: {'P3': 25166}
- **Priority Seleccionados**: {'P3': 3248, 'NA': 487, 'P0': 58}
- **Type Candidatos**: {'Swing': 25166}
- **Type Seleccionados**: {'P3_Swing': 3248, 'P4_Fallback': 487, 'P0_Zone': 58}
- **TF Candidatos**: {240: 11033, 15: 4979, 60: 4667, 5: 4487}
- **TF Seleccionados**: {15: 647, 240: 1958, 5: 376, -1: 487, 60: 325}
- **DistATR** - Candidatos: avg=10.7 | Seleccionados: avg=5.3
- **RR** - Candidatos: avg=4.80 | Seleccionados: avg=1.36
- **Razones de selección**: {'BestIntelligentScore': 3793}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.