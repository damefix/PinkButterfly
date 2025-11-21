# Informe Diagnóstico de Logs - 2025-11-11 19:38:01

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_192921.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_192921.csv`

## DFM
- Eventos de evaluación: 193
- Evaluaciones Bull: 68 | Bear: 144
- Pasaron umbral (PassedThreshold): 194
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:5, 4:13, 5:15, 6:103, 7:69, 8:7, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2209
- KeptAligned: 1918/1918 | KeptCounter: 1292/1292
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.380 | AvgProxCounter≈ 0.236
  - AvgDistATRAligned≈ 0.54 | AvgDistATRCounter≈ 0.38
- PreferAligned eventos: 874 | Filtradas contra-bias: 243

### Proximity (Pre-PreferAligned)
- Eventos: 2209
- Aligned pre: 1918/3210 | Counter pre: 1292/3210
- AvgProxAligned(pre)≈ 0.380 | AvgDistATRAligned(pre)≈ 0.54

### Proximity Drivers
- Eventos: 2209
- Alineadas: n=1918 | BaseProx≈ 0.873 | ZoneATR≈ 5.01 | SizePenalty≈ 0.978 | FinalProx≈ 0.854
- Contra-bias: n=1049 | BaseProx≈ 0.760 | ZoneATR≈ 4.67 | SizePenalty≈ 0.980 | FinalProx≈ 0.745

## Risk
- Eventos: 1434
- Accepted=212 | RejSL=0 | RejTP=0 | RejRR=185 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 32 (7.2% del total)
  - Avg Score: 0.41 | Avg R:R: 1.91 | Avg DistATR: 3.43
  - Por TF: TF5=8, TF15=24
- **P0_SWING_LITE:** 415 (92.8% del total)
  - Avg Score: 0.51 | Avg R:R: 3.90 | Avg DistATR: 3.42
  - Por TF: TF15=75, TF60=340


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 56 | Unmatched: 157
- 0-10: Wins=19 Losses=37 WR=33.9% (n=56)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=19 Losses=37 WR=33.9% (n=56)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 213 | Aligned=121 (56.8%)
- Core≈ 1.00 | Prox≈ 0.80 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.74 | Confidence≈ 0.00
- SL_TF dist: {'60': 16, '15': 158, '5': 39} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 111, '60': 50, '5': 38, '240': 14} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=212, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=38, 15m=158, 60m=16, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.74 (n=212), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 39757 | Zonas con Anchors: 39743
- Dir zonas (zona): Bull=8632 Bear=30178 Neutral=947
- Resumen por ciclo (promedios): TotHZ≈ 3.7, WithAnchors≈ 3.7, DirBull≈ 1.4, DirBear≈ 2.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 38610, 'tie-bias': 1133, 'triggers-only': 14}
- TF Triggers: {'5': 5018, '15': 4285}
- TF Anchors: {'60': 9245, '240': 5583, '1440': 535}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 2}

## CSV de Trades
- Filas: 101 | Ejecutadas: 36 | Canceladas: 0 | Expiradas: 0
- BUY: 57 | SELL: 80

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 194
- Registered: 55
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 32 | SKIP_CONCURRENCY: 4
- Intentos de registro: 91

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 46.9%
- RegRate = Registered / Intentos = 60.4%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 35.2%
- Concurrency = SKIP_CONCURRENCY / Intentos = 4.4%
- ExecRate = Ejecutadas / Registered = 65.5%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 632 | Total candidatos: 10235 | Seleccionados: 0
- Candidatos por zona (promedio): 16.2

### Take Profit (TP)
- Zonas analizadas: 617 | Total candidatos: 4752 | Seleccionados: 617
- Candidatos por zona (promedio): 7.7
- **Edad (barras)** - Candidatos: med=53, max=182 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.40 | Seleccionados: avg=0.70
- **Priority Candidatos**: {'P3': 4752}
- **Priority Seleccionados**: {'P3': 278, 'NA': 272, 'P0': 67}
- **Type Candidatos**: {'Swing': 4752}
- **Type Seleccionados**: {'P3_Swing': 278, 'P4_Fallback': 272, 'P0_Zone': 67}
- **TF Candidatos**: {60: 1539, 15: 1262, 5: 1162, 240: 789}
- **TF Seleccionados**: {60: 90, 5: 69, -1: 272, 15: 155, 240: 31}
- **DistATR** - Candidatos: avg=9.5 | Seleccionados: avg=3.6
- **RR** - Candidatos: avg=4.62 | Seleccionados: avg=1.33
- **Razones de selección**: {'BestIntelligentScore': 617}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.