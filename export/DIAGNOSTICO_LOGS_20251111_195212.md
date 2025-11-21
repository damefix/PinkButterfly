# Informe Diagnóstico de Logs - 2025-11-11 19:56:12

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_195212.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_195212.csv`

## DFM
- Eventos de evaluación: 189
- Evaluaciones Bull: 68 | Bear: 139
- Pasaron umbral (PassedThreshold): 190
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:5, 4:11, 5:17, 6:94, 7:72, 8:8, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2208
- KeptAligned: 1897/1897 | KeptCounter: 1281/1281
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.382 | AvgProxCounter≈ 0.236
  - AvgDistATRAligned≈ 0.54 | AvgDistATRCounter≈ 0.38
- PreferAligned eventos: 872 | Filtradas contra-bias: 245

### Proximity (Pre-PreferAligned)
- Eventos: 2208
- Aligned pre: 1897/3178 | Counter pre: 1281/3178
- AvgProxAligned(pre)≈ 0.382 | AvgDistATRAligned(pre)≈ 0.54

### Proximity Drivers
- Eventos: 2208
- Alineadas: n=1897 | BaseProx≈ 0.873 | ZoneATR≈ 5.03 | SizePenalty≈ 0.978 | FinalProx≈ 0.854
- Contra-bias: n=1036 | BaseProx≈ 0.760 | ZoneATR≈ 4.71 | SizePenalty≈ 0.980 | FinalProx≈ 0.744

## Risk
- Eventos: 1435
- Accepted=207 | RejSL=0 | RejTP=0 | RejRR=180 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 32 (7.2% del total)
  - Avg Score: 0.41 | Avg R:R: 1.93 | Avg DistATR: 3.51
  - Por TF: TF5=8, TF15=24
- **P0_SWING_LITE:** 414 (92.8% del total)
  - Avg Score: 0.51 | Avg R:R: 3.94 | Avg DistATR: 3.42
  - Por TF: TF15=72, TF60=342


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 57 | Unmatched: 151
- 0-10: Wins=23 Losses=34 WR=40.4% (n=57)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=23 Losses=34 WR=40.4% (n=57)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 208 | Aligned=119 (57.2%)
- Core≈ 1.00 | Prox≈ 0.80 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.75 | Confidence≈ 0.00
- SL_TF dist: {'60': 16, '15': 153, '5': 39} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 110, '60': 50, '5': 39, '240': 9} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=207, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=38, 15m=153, 60m=16, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.75 (n=207), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 39840 | Zonas con Anchors: 39826
- Dir zonas (zona): Bull=8756 Bear=30136 Neutral=948
- Resumen por ciclo (promedios): TotHZ≈ 3.7, WithAnchors≈ 3.7, DirBull≈ 1.4, DirBear≈ 2.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 38692, 'tie-bias': 1134, 'triggers-only': 14}
- TF Triggers: {'5': 5004, '15': 4286}
- TF Anchors: {'60': 9232, '240': 5573, '1440': 535}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 2}

## CSV de Trades
- Filas: 101 | Ejecutadas: 37 | Canceladas: 0 | Expiradas: 0
- BUY: 58 | SELL: 80

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 190
- Registered: 54
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 31 | SKIP_CONCURRENCY: 4
- Intentos de registro: 89

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 46.8%
- RegRate = Registered / Intentos = 60.7%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 34.8%
- Concurrency = SKIP_CONCURRENCY / Intentos = 4.5%
- ExecRate = Ejecutadas / Registered = 68.5%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 624 | Total candidatos: 10229 | Seleccionados: 0
- Candidatos por zona (promedio): 16.4

### Take Profit (TP)
- Zonas analizadas: 611 | Total candidatos: 4741 | Seleccionados: 611
- Candidatos por zona (promedio): 7.8
- **Edad (barras)** - Candidatos: med=53, max=182 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.40 | Seleccionados: avg=0.70
- **Priority Candidatos**: {'P3': 4741}
- **Priority Seleccionados**: {'NA': 277, 'P0': 66, 'P3': 268}
- **Type Candidatos**: {'Swing': 4741}
- **Type Seleccionados**: {'P4_Fallback': 277, 'P0_Zone': 66, 'P3_Swing': 268}
- **TF Candidatos**: {60: 1525, 15: 1256, 5: 1159, 240: 801}
- **TF Seleccionados**: {-1: 277, 15: 151, 60: 86, 5: 71, 240: 26}
- **DistATR** - Candidatos: avg=9.4 | Seleccionados: avg=3.5
- **RR** - Candidatos: avg=4.75 | Seleccionados: avg=1.33
- **Razones de selección**: {'BestIntelligentScore': 611}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.