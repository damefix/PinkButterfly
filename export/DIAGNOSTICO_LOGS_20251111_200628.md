# Informe Diagnóstico de Logs - 2025-11-11 20:25:30

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_200628.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_200628.csv`

## DFM
- Eventos de evaluación: 192
- Evaluaciones Bull: 74 | Bear: 136
- Pasaron umbral (PassedThreshold): 195
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:4, 4:11, 5:17, 6:99, 7:71, 8:8, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2212
- KeptAligned: 1904/1904 | KeptCounter: 1254/1254
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.383 | AvgProxCounter≈ 0.230
  - AvgDistATRAligned≈ 0.54 | AvgDistATRCounter≈ 0.38
- PreferAligned eventos: 884 | Filtradas contra-bias: 260

### Proximity (Pre-PreferAligned)
- Eventos: 2212
- Aligned pre: 1904/3158 | Counter pre: 1254/3158
- AvgProxAligned(pre)≈ 0.383 | AvgDistATRAligned(pre)≈ 0.54

### Proximity Drivers
- Eventos: 2212
- Alineadas: n=1904 | BaseProx≈ 0.873 | ZoneATR≈ 5.01 | SizePenalty≈ 0.978 | FinalProx≈ 0.854
- Contra-bias: n=994 | BaseProx≈ 0.758 | ZoneATR≈ 4.68 | SizePenalty≈ 0.981 | FinalProx≈ 0.744

## Risk
- Eventos: 1425
- Accepted=210 | RejSL=0 | RejTP=0 | RejRR=176 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 31 (7.1% del total)
  - Avg Score: 0.41 | Avg R:R: 1.91 | Avg DistATR: 3.47
  - Por TF: TF5=8, TF15=23
- **P0_SWING_LITE:** 407 (92.9% del total)
  - Avg Score: 0.51 | Avg R:R: 3.93 | Avg DistATR: 3.42
  - Por TF: TF15=72, TF60=335


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 60 | Unmatched: 151
- 0-10: Wins=23 Losses=37 WR=38.3% (n=60)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=23 Losses=37 WR=38.3% (n=60)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 211 | Aligned=123 (58.3%)
- Core≈ 1.00 | Prox≈ 0.80 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.76 | Confidence≈ 0.00
- SL_TF dist: {'60': 18, '15': 155, '5': 38} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 115, '60': 49, '5': 38, '240': 9} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=210, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=37, 15m=155, 60m=18, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.76 (n=210), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 39625 | Zonas con Anchors: 39611
- Dir zonas (zona): Bull=8842 Bear=29828 Neutral=955
- Resumen por ciclo (promedios): TotHZ≈ 3.7, WithAnchors≈ 3.7, DirBull≈ 1.5, DirBear≈ 2.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 38425, 'tie-bias': 1186, 'triggers-only': 14}
- TF Triggers: {'15': 4278, '5': 5012}
- TF Anchors: {'60': 9232, '240': 5441, '1440': 540}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 2}

## CSV de Trades
- Filas: 101 | Ejecutadas: 37 | Canceladas: 0 | Expiradas: 0
- BUY: 60 | SELL: 78

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 195
- Registered: 54
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 34 | SKIP_CONCURRENCY: 4
- Intentos de registro: 92

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 47.2%
- RegRate = Registered / Intentos = 58.7%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 37.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 4.3%
- ExecRate = Ejecutadas / Registered = 68.5%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 620 | Total candidatos: 10068 | Seleccionados: 0
- Candidatos por zona (promedio): 16.2

### Take Profit (TP)
- Zonas analizadas: 606 | Total candidatos: 4615 | Seleccionados: 606
- Candidatos por zona (promedio): 7.6
- **Edad (barras)** - Candidatos: med=53, max=182 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.40 | Seleccionados: avg=0.70
- **Priority Candidatos**: {'P3': 4615}
- **Priority Seleccionados**: {'NA': 271, 'P0': 69, 'P3': 266}
- **Type Candidatos**: {'Swing': 4615}
- **Type Seleccionados**: {'P4_Fallback': 271, 'P0_Zone': 69, 'P3_Swing': 266}
- **TF Candidatos**: {60: 1435, 15: 1226, 5: 1153, 240: 801}
- **TF Seleccionados**: {-1: 271, 15: 158, 60: 84, 5: 70, 240: 23}
- **DistATR** - Candidatos: avg=9.0 | Seleccionados: avg=3.6
- **RR** - Candidatos: avg=4.62 | Seleccionados: avg=1.33
- **Razones de selección**: {'BestIntelligentScore': 606}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.