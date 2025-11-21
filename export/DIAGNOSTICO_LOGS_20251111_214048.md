# Informe Diagnóstico de Logs - 2025-11-11 21:44:54

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_214048.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_214048.csv`

## DFM
- Eventos de evaluación: 187
- Evaluaciones Bull: 64 | Bear: 140
- Pasaron umbral (PassedThreshold): 185
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:4, 4:15, 5:13, 6:102, 7:61, 8:9, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2217
- KeptAligned: 1853/1853 | KeptCounter: 1270/1270
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.370 | AvgProxCounter≈ 0.235
  - AvgDistATRAligned≈ 0.53 | AvgDistATRCounter≈ 0.38
- PreferAligned eventos: 846 | Filtradas contra-bias: 234

### Proximity (Pre-PreferAligned)
- Eventos: 2217
- Aligned pre: 1853/3123 | Counter pre: 1270/3123
- AvgProxAligned(pre)≈ 0.370 | AvgDistATRAligned(pre)≈ 0.53

### Proximity Drivers
- Eventos: 2217
- Alineadas: n=1853 | BaseProx≈ 0.871 | ZoneATR≈ 5.00 | SizePenalty≈ 0.978 | FinalProx≈ 0.852
- Contra-bias: n=1036 | BaseProx≈ 0.765 | ZoneATR≈ 4.64 | SizePenalty≈ 0.981 | FinalProx≈ 0.750

## Risk
- Eventos: 1418
- Accepted=204 | RejSL=0 | RejTP=0 | RejRR=174 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 34 (7.9% del total)
  - Avg Score: 0.41 | Avg R:R: 1.99 | Avg DistATR: 3.55
  - Por TF: TF5=8, TF15=26
- **P0_SWING_LITE:** 398 (92.1% del total)
  - Avg Score: 0.51 | Avg R:R: 3.97 | Avg DistATR: 3.40
  - Por TF: TF15=73, TF60=325


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 57 | Unmatched: 148
- 0-10: Wins=20 Losses=37 WR=35.1% (n=57)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=20 Losses=37 WR=35.1% (n=57)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 205 | Aligned=119 (58.0%)
- Core≈ 1.00 | Prox≈ 0.81 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.78 | Confidence≈ 0.00
- SL_TF dist: {'60': 17, '5': 39, '15': 149} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 40, '60': 50, '15': 105, '240': 10} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=204, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=38, 15m=149, 60m=17, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.78 (n=204), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 39889 | Zonas con Anchors: 39875
- Dir zonas (zona): Bull=8983 Bear=29960 Neutral=946
- Resumen por ciclo (promedios): TotHZ≈ 3.7, WithAnchors≈ 3.7, DirBull≈ 1.5, DirBear≈ 2.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 38690, 'tie-bias': 1185, 'triggers-only': 14}
- TF Triggers: {'5': 5005, '15': 4301}
- TF Anchors: {'60': 9248, '240': 5467, '1440': 557}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 2}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 98 | Ejecutadas: 36 | Canceladas: 0 | Expiradas: 0
- BUY: 54 | SELL: 80

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 185
- Registered: 52
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 28 | SKIP_CONCURRENCY: 4
- Intentos de registro: 84

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 45.4%
- RegRate = Registered / Intentos = 61.9%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 33.3%
- Concurrency = SKIP_CONCURRENCY / Intentos = 4.8%
- ExecRate = Ejecutadas / Registered = 69.2%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 607 | Total candidatos: 9827 | Seleccionados: 0
- Candidatos por zona (promedio): 16.2

### Take Profit (TP)
- Zonas analizadas: 596 | Total candidatos: 4571 | Seleccionados: 596
- Candidatos por zona (promedio): 7.7
- **Edad (barras)** - Candidatos: med=53, max=182 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.40 | Seleccionados: avg=0.70
- **Priority Candidatos**: {'P3': 4571}
- **Priority Seleccionados**: {'NA': 266, 'P0': 62, 'P3': 268}
- **Type Candidatos**: {'Swing': 4571}
- **Type Seleccionados**: {'P4_Fallback': 266, 'P0_Zone': 62, 'P3_Swing': 268}
- **TF Candidatos**: {60: 1429, 15: 1222, 5: 1148, 240: 772}
- **TF Seleccionados**: {-1: 266, 5: 71, 60: 86, 15: 149, 240: 24}
- **DistATR** - Candidatos: avg=9.1 | Seleccionados: avg=3.6
- **RR** - Candidatos: avg=4.59 | Seleccionados: avg=1.34
- **Razones de selección**: {'BestIntelligentScore': 596}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.