# Informe Diagnóstico de Logs - 2025-11-11 10:43:06

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_103713.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_103713.csv`

## DFM
- Eventos de evaluación: 211
- Evaluaciones Bull: 52 | Bear: 193
- Pasaron umbral (PassedThreshold): 0
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:28, 4:3, 5:55, 6:153, 7:6, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2184
- KeptAligned: 1847/1847 | KeptCounter: 1046/1046
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.381 | AvgProxCounter≈ 0.244
  - AvgDistATRAligned≈ 0.53 | AvgDistATRCounter≈ 0.37
- PreferAligned eventos: 926 | Filtradas contra-bias: 326

### Proximity (Pre-PreferAligned)
- Eventos: 2184
- Aligned pre: 1847/2893 | Counter pre: 1046/2893
- AvgProxAligned(pre)≈ 0.381 | AvgDistATRAligned(pre)≈ 0.53

### Proximity Drivers
- Eventos: 2184
- Alineadas: n=1847 | BaseProx≈ 0.918 | ZoneATR≈ 5.08 | SizePenalty≈ 0.977 | FinalProx≈ 0.897
- Contra-bias: n=720 | BaseProx≈ 0.833 | ZoneATR≈ 4.96 | SizePenalty≈ 0.977 | FinalProx≈ 0.814

## Risk
- Eventos: 1346
- Accepted=246 | RejSL=0 | RejTP=0 | RejRR=98 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 39 (9.6% del total)
  - Avg Score: 0.43 | Avg R:R: 1.80 | Avg DistATR: 3.40
  - Por TF: TF5=11, TF15=28
- **P0_SWING_LITE:** 368 (90.4% del total)
  - Avg Score: 0.61 | Avg R:R: 3.73 | Avg DistATR: 3.40
  - Por TF: TF15=84, TF60=284


### SLPick por Bandas y TF
- Bandas: lt8=246, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=51, 15m=195, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.11 (n=246), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 41976 | Zonas con Anchors: 41964
- Dir zonas (zona): Bull=7899 Bear=32941 Neutral=1136
- Resumen por ciclo (promedios): TotHZ≈ 3.6, WithAnchors≈ 3.6, DirBull≈ 1.3, DirBear≈ 2.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 40828, 'tie-bias': 1136, 'triggers-only': 12}
- TF Triggers: {'5': 4969, '15': 3917}
- TF Anchors: {'60': 8838, '240': 5441, '1440': 270}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 0
- Registered: 0
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 0

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 0.0%
- RegRate = Registered / Intentos = 0.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 0.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 567 | Total candidatos: 10237 | Seleccionados: 0
- Candidatos por zona (promedio): 18.1

### Take Profit (TP)
- Zonas analizadas: 561 | Total candidatos: 4568 | Seleccionados: 0
- Candidatos por zona (promedio): 8.1
- **Edad (barras)** - Candidatos: med=53, max=250 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 4568}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 4568}
- **Type Seleccionados**: {}
- **TF Candidatos**: {60: 1402, 5: 1314, 15: 1214, 240: 638}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=9.0 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=4.01 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- KeptAligned ratio≈ 1.00.