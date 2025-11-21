# Informe Diagnóstico de Logs - 2025-11-18 10:00:25

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251118_095729.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251118_095729.csv`

## DFM
- Eventos de evaluación: 250
- Evaluaciones Bull: 12 | Bear: 217
- Pasaron umbral (PassedThreshold): 229
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:3, 6:61, 7:103, 8:62, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 627
- KeptAligned: 984/984 | KeptCounter: 1133/1175
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.382 | AvgProxCounter≈ 0.270
  - AvgDistATRAligned≈ 1.26 | AvgDistATRCounter≈ 1.75
- PreferAligned eventos: 267 | Filtradas contra-bias: 147

### Proximity (Pre-PreferAligned)
- Eventos: 627
- Aligned pre: 984/2117 | Counter pre: 1133/2117
- AvgProxAligned(pre)≈ 0.382 | AvgDistATRAligned(pre)≈ 1.26

### Proximity Drivers
- Eventos: 627
- Alineadas: n=984 | BaseProx≈ 0.777 | ZoneATR≈ 4.65 | SizePenalty≈ 0.984 | FinalProx≈ 0.765
- Contra-bias: n=986 | BaseProx≈ 0.446 | ZoneATR≈ 4.71 | SizePenalty≈ 0.979 | FinalProx≈ 0.438

## Risk
- Eventos: 573
- Accepted=355 | RejSL=0 | RejTP=0 | RejRR=355 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 97 (12.2% del total)
  - Avg Score: 0.37 | Avg R:R: 1.86 | Avg DistATR: 3.72
  - Por TF: TF5=35, TF15=62
- **P0_SWING_LITE:** 701 (87.8% del total)
  - Avg Score: 0.61 | Avg R:R: 5.03 | Avg DistATR: 3.65
  - Por TF: TF15=187, TF60=514


### SLPick por Bandas y TF
- Bandas: lt8=355, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=56, 15m=251, 60m=29, 240m=19, 1440m=0
- RR plan por bandas: 0-10≈ 2.22 (n=355), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 3246 | Zonas con Anchors: 3246
- Dir zonas (zona): Bull=371 Bear=2759 Neutral=116
- Resumen por ciclo (promedios): TotHZ≈ 5.2, WithAnchors≈ 5.2, DirBull≈ 0.6, DirBear≈ 4.4, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 3112, 'tie-bias': 134}
- TF Triggers: {'15': 1670, '5': 1576}
- TF Anchors: {'60': 3210, '240': 3246, '1440': 3246}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 229
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
- Zonas analizadas: 1525 | Total candidatos: 13550 | Seleccionados: 0
- Candidatos por zona (promedio): 8.9

### Take Profit (TP)
- Zonas analizadas: 1510 | Total candidatos: 17782 | Seleccionados: 1510
- Candidatos por zona (promedio): 11.8
- **Edad (barras)** - Candidatos: med=37, max=157 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.69
- **Priority Candidatos**: {'P3': 17782}
- **Priority Seleccionados**: {'P3': 1179, 'NA': 273, 'P0': 58}
- **Type Candidatos**: {'Swing': 17782}
- **Type Seleccionados**: {'P3_Swing': 1179, 'P4_Fallback': 273, 'P0_Zone': 58}
- **TF Candidatos**: {240: 6689, 15: 4040, 5: 3964, 60: 3089}
- **TF Seleccionados**: {5: 287, 240: 477, -1: 273, 15: 351, 60: 122}
- **DistATR** - Candidatos: avg=12.0 | Seleccionados: avg=6.0
- **RR** - Candidatos: avg=5.31 | Seleccionados: avg=1.41
- **Razones de selección**: {'BestIntelligentScore': 1510}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- KeptAligned ratio≈ 1.00.