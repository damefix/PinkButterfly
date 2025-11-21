# Informe Diagnóstico de Logs - 2025-11-11 20:32:45

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_202644.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_202644.csv`

## DFM
- Eventos de evaluación: 185
- Evaluaciones Bull: 67 | Bear: 136
- Pasaron umbral (PassedThreshold): 188
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:4, 4:11, 5:14, 6:95, 7:71, 8:8, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2216
- KeptAligned: 1841/1841 | KeptCounter: 1264/1264
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.371 | AvgProxCounter≈ 0.233
  - AvgDistATRAligned≈ 0.53 | AvgDistATRCounter≈ 0.38
- PreferAligned eventos: 850 | Filtradas contra-bias: 260

### Proximity (Pre-PreferAligned)
- Eventos: 2216
- Aligned pre: 1841/3105 | Counter pre: 1264/3105
- AvgProxAligned(pre)≈ 0.371 | AvgDistATRAligned(pre)≈ 0.53

### Proximity Drivers
- Eventos: 2216
- Alineadas: n=1841 | BaseProx≈ 0.872 | ZoneATR≈ 5.01 | SizePenalty≈ 0.978 | FinalProx≈ 0.853
- Contra-bias: n=1004 | BaseProx≈ 0.760 | ZoneATR≈ 4.62 | SizePenalty≈ 0.981 | FinalProx≈ 0.746

## Risk
- Eventos: 1399
- Accepted=203 | RejSL=0 | RejTP=0 | RejRR=180 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 31 (7.1% del total)
  - Avg Score: 0.41 | Avg R:R: 1.91 | Avg DistATR: 3.47
  - Por TF: TF5=8, TF15=23
- **P0_SWING_LITE:** 407 (92.9% del total)
  - Avg Score: 0.50 | Avg R:R: 3.95 | Avg DistATR: 3.44
  - Por TF: TF15=72, TF60=335


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 59 | Unmatched: 145
- 0-10: Wins=22 Losses=37 WR=37.3% (n=59)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=22 Losses=37 WR=37.3% (n=59)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 204 | Aligned=123 (60.3%)
- Core≈ 1.00 | Prox≈ 0.80 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.76 | Confidence≈ 0.00
- SL_TF dist: {'60': 14, '15': 152, '5': 38} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 40, '15': 106, '60': 49, '240': 9} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=203, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=37, 15m=152, 60m=14, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.76 (n=203), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 39913 | Zonas con Anchors: 39899
- Dir zonas (zona): Bull=8993 Bear=29950 Neutral=970
- Resumen por ciclo (promedios): TotHZ≈ 3.7, WithAnchors≈ 3.7, DirBull≈ 1.5, DirBear≈ 2.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 38699, 'tie-bias': 1200, 'triggers-only': 14}
- TF Triggers: {'15': 4315, '5': 4999}
- TF Anchors: {'60': 9256, '240': 5449, '1440': 544}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 2}

## CSV de Trades
- Filas: 99 | Ejecutadas: 37 | Canceladas: 0 | Expiradas: 0
- BUY: 58 | SELL: 78

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 188
- Registered: 53
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 31 | SKIP_CONCURRENCY: 4
- Intentos de registro: 88

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 46.8%
- RegRate = Registered / Intentos = 60.2%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 35.2%
- Concurrency = SKIP_CONCURRENCY / Intentos = 4.5%
- ExecRate = Ejecutadas / Registered = 69.8%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 614 | Total candidatos: 10127 | Seleccionados: 0
- Candidatos por zona (promedio): 16.5

### Take Profit (TP)
- Zonas analizadas: 601 | Total candidatos: 4599 | Seleccionados: 601
- Candidatos por zona (promedio): 7.7
- **Edad (barras)** - Candidatos: med=53, max=182 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.40 | Seleccionados: avg=0.70
- **Priority Candidatos**: {'P3': 4599}
- **Priority Seleccionados**: {'NA': 273, 'P0': 64, 'P3': 264}
- **Type Candidatos**: {'Swing': 4599}
- **Type Seleccionados**: {'P4_Fallback': 273, 'P0_Zone': 64, 'P3_Swing': 264}
- **TF Candidatos**: {60: 1433, 15: 1216, 5: 1153, 240: 797}
- **TF Seleccionados**: {-1: 273, 5: 73, 15: 150, 60: 83, 240: 22}
- **DistATR** - Candidatos: avg=9.0 | Seleccionados: avg=3.5
- **RR** - Candidatos: avg=4.63 | Seleccionados: avg=1.33
- **Razones de selección**: {'BestIntelligentScore': 601}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.