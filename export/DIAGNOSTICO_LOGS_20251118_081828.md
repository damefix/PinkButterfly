# Informe Diagnóstico de Logs - 2025-11-18 08:23:06

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251118_081828.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251118_081828.csv`

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

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 574
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
- KeptAligned ratio≈ 1.00.