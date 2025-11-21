# Informe Diagnóstico de Logs - 2025-11-11 19:09:03

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_182510.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_182510.csv`

## DFM
- Eventos de evaluación: 172
- Evaluaciones Bull: 54 | Bear: 137
- Pasaron umbral (PassedThreshold): 172
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:5, 4:14, 5:11, 6:87, 7:67, 8:7, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2208
- KeptAligned: 1910/1910 | KeptCounter: 1263/1263
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.377 | AvgProxCounter≈ 0.237
  - AvgDistATRAligned≈ 0.53 | AvgDistATRCounter≈ 0.39
- PreferAligned eventos: 968 | Filtradas contra-bias: 374

### Proximity (Pre-PreferAligned)
- Eventos: 2208
- Aligned pre: 1910/3173 | Counter pre: 1263/3173
- AvgProxAligned(pre)≈ 0.377 | AvgDistATRAligned(pre)≈ 0.53

### Proximity Drivers
- Eventos: 2208
- Alineadas: n=1910 | BaseProx≈ 0.874 | ZoneATR≈ 5.01 | SizePenalty≈ 0.978 | FinalProx≈ 0.855
- Contra-bias: n=889 | BaseProx≈ 0.760 | ZoneATR≈ 4.74 | SizePenalty≈ 0.979 | FinalProx≈ 0.744

## Risk
- Eventos: 1432
- Accepted=191 | RejSL=0 | RejTP=0 | RejRR=162 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 30 (7.4% del total)
  - Avg Score: 0.41 | Avg R:R: 1.87 | Avg DistATR: 3.35
  - Por TF: TF5=7, TF15=23
- **P0_SWING_LITE:** 378 (92.6% del total)
  - Avg Score: 0.51 | Avg R:R: 4.01 | Avg DistATR: 3.41
  - Por TF: TF15=69, TF60=309


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 55 | Unmatched: 137
- 0-10: Wins=20 Losses=35 WR=36.4% (n=55)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=20 Losses=35 WR=36.4% (n=55)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 192 | Aligned=116 (60.4%)
- Core≈ 1.00 | Prox≈ 0.80 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.76 | Confidence≈ 0.00
- SL_TF dist: {'5': 39, '60': 13, '15': 140} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 48, '15': 98, '5': 35, '240': 11} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=191, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=38, 15m=140, 60m=13, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.76 (n=191), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 39641 | Zonas con Anchors: 39627
- Dir zonas (zona): Bull=8598 Bear=30100 Neutral=943
- Resumen por ciclo (promedios): TotHZ≈ 3.7, WithAnchors≈ 3.7, DirBull≈ 1.4, DirBear≈ 2.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 38502, 'tie-bias': 1125, 'triggers-only': 14}
- TF Triggers: {'15': 4276, '5': 4985}
- TF Anchors: {'60': 9203, '240': 5550, '1440': 516}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 1}

## CSV de Trades
- Filas: 90 | Ejecutadas: 35 | Canceladas: 0 | Expiradas: 0
- BUY: 50 | SELL: 75

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 172
- Registered: 48
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 23 | SKIP_CONCURRENCY: 4
- Intentos de registro: 75

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 43.6%
- RegRate = Registered / Intentos = 64.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 30.7%
- Concurrency = SKIP_CONCURRENCY / Intentos = 5.3%
- ExecRate = Ejecutadas / Registered = 72.9%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 559 | Total candidatos: 9179 | Seleccionados: 0
- Candidatos por zona (promedio): 16.4

### Take Profit (TP)
- Zonas analizadas: 549 | Total candidatos: 4314 | Seleccionados: 549
- Candidatos por zona (promedio): 7.9
- **Edad (barras)** - Candidatos: med=53, max=182 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.40 | Seleccionados: avg=0.70
- **Priority Candidatos**: {'P3': 4314}
- **Priority Seleccionados**: {'P3': 252, 'NA': 242, 'P0': 55}
- **Type Candidatos**: {'Swing': 4314}
- **Type Seleccionados**: {'P3_Swing': 252, 'P4_Fallback': 242, 'P0_Zone': 55}
- **TF Candidatos**: {60: 1384, 15: 1141, 5: 1092, 240: 697}
- **TF Seleccionados**: {60: 78, -1: 242, 15: 137, 5: 63, 240: 29}
- **DistATR** - Candidatos: avg=9.6 | Seleccionados: avg=3.6
- **RR** - Candidatos: avg=4.78 | Seleccionados: avg=1.34
- **Razones de selección**: {'BestIntelligentScore': 549}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.