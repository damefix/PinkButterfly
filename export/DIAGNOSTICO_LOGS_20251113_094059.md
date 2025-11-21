# Informe Diagnóstico de Logs - 2025-11-13 09:43:33

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_094059.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_094059.csv`

## DFM
- Eventos de evaluación: 940
- Evaluaciones Bull: 162 | Bear: 680
- Pasaron umbral (PassedThreshold): 842
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:72, 6:367, 7:354, 8:49, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2355
- KeptAligned: 4148/4148 | KeptCounter: 2742/2848
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.444 | AvgProxCounter≈ 0.231
  - AvgDistATRAligned≈ 1.52 | AvgDistATRCounter≈ 1.13
- PreferAligned eventos: 1283 | Filtradas contra-bias: 573

### Proximity (Pre-PreferAligned)
- Eventos: 2355
- Aligned pre: 4148/6890 | Counter pre: 2742/6890
- AvgProxAligned(pre)≈ 0.444 | AvgDistATRAligned(pre)≈ 1.52

### Proximity Drivers
- Eventos: 2355
- Alineadas: n=4148 | BaseProx≈ 0.752 | ZoneATR≈ 5.17 | SizePenalty≈ 0.975 | FinalProx≈ 0.733
- Contra-bias: n=2169 | BaseProx≈ 0.529 | ZoneATR≈ 4.84 | SizePenalty≈ 0.978 | FinalProx≈ 0.519

## Risk
- Eventos: 1946
- Accepted=1267 | RejSL=0 | RejTP=0 | RejRR=1259 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 310 (10.8% del total)
  - Avg Score: 0.39 | Avg R:R: 1.89 | Avg DistATR: 3.75
  - Por TF: TF5=79, TF15=231
- **P0_SWING_LITE:** 2562 (89.2% del total)
  - Avg Score: 0.57 | Avg R:R: 4.14 | Avg DistATR: 3.48
  - Por TF: TF15=544, TF60=2018


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 86 | Unmatched: 1221
- 0-10: Wins=36 Losses=50 WR=41.9% (n=86)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=36 Losses=50 WR=41.9% (n=86)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1307 | Aligned=804 (61.5%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.07 | Confidence≈ 0.00
- SL_TF dist: {'60': 154, '15': 949, '5': 193, '240': 11} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 289, '15': 497, '5': 347, '240': 174} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1267, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=185, 15m=925, 60m=150, 240m=7, 1440m=0
- RR plan por bandas: 0-10≈ 2.05 (n=1267), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10315 | Zonas con Anchors: 10301
- Dir zonas (zona): Bull=3770 Bear=6214 Neutral=331
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9906, 'tie-bias': 395, 'triggers-only': 14}
- TF Triggers: {'5': 5458, '15': 4857}
- TF Anchors: {'60': 10227, '240': 5965, '1440': 454}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,46': 1, 'estructura no existe': 25, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,42': 1, 'score decayó a 0,34': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 194 | Ejecutadas: 35 | Canceladas: 0 | Expiradas: 0
- BUY: 69 | SELL: 160

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 842
- Registered: 102
  - DEDUP_COOLDOWN: 23 | DEDUP_IDENTICAL: 109 | SKIP_CONCURRENCY: 93
- Intentos de registro: 327

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 38.8%
- RegRate = Registered / Intentos = 31.2%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 40.4%
- Concurrency = SKIP_CONCURRENCY / Intentos = 28.4%
- ExecRate = Ejecutadas / Registered = 34.3%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5454 | Total candidatos: 43415 | Seleccionados: 0
- Candidatos por zona (promedio): 8.0

### Take Profit (TP)
- Zonas analizadas: 5347 | Total candidatos: 51260 | Seleccionados: 5347
- Candidatos por zona (promedio): 9.6
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 51260}
- **Priority Seleccionados**: {'P3': 3666, 'NA': 1363, 'P0': 318}
- **Type Candidatos**: {'Swing': 51260}
- **Type Seleccionados**: {'P3_Swing': 3666, 'P4_Fallback': 1363, 'P0_Zone': 318}
- **TF Candidatos**: {5: 15402, 15: 13978, 60: 13688, 240: 8192}
- **TF Seleccionados**: {60: 1003, 5: 1019, -1: 1363, 15: 1254, 240: 708}
- **DistATR** - Candidatos: avg=8.6 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.52 | Seleccionados: avg=1.31
- **Razones de selección**: {'BestIntelligentScore': 5347}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.