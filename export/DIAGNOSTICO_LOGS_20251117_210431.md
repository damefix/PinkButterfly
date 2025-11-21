# Informe Diagnóstico de Logs - 2025-11-17 21:09:41

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_210431.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251117_210431.csv`

## DFM
- Eventos de evaluación: 544
- Evaluaciones Bull: 75 | Bear: 336
- Pasaron umbral (PassedThreshold): 411
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:24, 6:191, 7:188, 8:8, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 1913
- KeptAligned: 1083/1083 | KeptCounter: 1207/1296
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.174 | AvgProxCounter≈ 0.139
  - AvgDistATRAligned≈ 0.77 | AvgDistATRCounter≈ 0.77
- PreferAligned eventos: 448 | Filtradas contra-bias: 333

### Proximity (Pre-PreferAligned)
- Eventos: 1913
- Aligned pre: 1083/2290 | Counter pre: 1207/2290
- AvgProxAligned(pre)≈ 0.174 | AvgDistATRAligned(pre)≈ 0.77

### Proximity Drivers
- Eventos: 1913
- Alineadas: n=1083 | BaseProx≈ 0.703 | ZoneATR≈ 4.72 | SizePenalty≈ 0.975 | FinalProx≈ 0.686
- Contra-bias: n=874 | BaseProx≈ 0.482 | ZoneATR≈ 6.14 | SizePenalty≈ 0.956 | FinalProx≈ 0.461

## Risk
- Eventos: 810
- Accepted=712 | RejSL=0 | RejTP=0 | RejRR=178 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 50 (4.7% del total)
  - Avg Score: 0.38 | Avg R:R: 1.63 | Avg DistATR: 4.50
  - Por TF: TF5=7, TF15=43
- **P0_SWING_LITE:** 1019 (95.3% del total)
  - Avg Score: 0.66 | Avg R:R: 6.00 | Avg DistATR: 4.23
  - Por TF: TF15=65, TF60=954


### SLPick por Bandas y TF
- Bandas: lt8=712, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=143, 15m=399, 60m=125, 240m=45, 1440m=0
- RR plan por bandas: 0-10≈ 2.30 (n=712), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 8948 | Zonas con Anchors: 8948
- Dir zonas (zona): Bull=2404 Bear=6283 Neutral=261
- Resumen por ciclo (promedios): TotHZ≈ 3.6, WithAnchors≈ 3.6, DirBull≈ 1.0, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 8645, 'tie-bias': 303}
- TF Triggers: {'5': 4289, '15': 4659}
- TF Anchors: {'60': 8948, '240': 8948, '1440': 8948}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 411
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
- Zonas analizadas: 1851 | Total candidatos: 9448 | Seleccionados: 0
- Candidatos por zona (promedio): 5.1

### Take Profit (TP)
- Zonas analizadas: 1834 | Total candidatos: 34179 | Seleccionados: 1834
- Candidatos por zona (promedio): 18.6
- **Edad (barras)** - Candidatos: med=34, max=105 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.72
- **Priority Candidatos**: {'P3': 5587}
- **Priority Seleccionados**: {'P3': 1719, 'NA': 101, 'P0': 14}
- **Type Candidatos**: {'Swing': 5587}
- **Type Seleccionados**: {'P3_Swing': 1719, 'P4_Fallback': 101, 'P0_Zone': 14}
- **TF Candidatos**: {240: 3920, 60: 1300, 15: 215, 5: 152}
- **TF Seleccionados**: {240: 813, -1: 101, 15: 368, 5: 198, 60: 354}
- **DistATR** - Candidatos: avg=8.1 | Seleccionados: avg=5.2
- **RR** - Candidatos: avg=4.73 | Seleccionados: avg=1.59
- **Razones de selección**: {'BestIntelligentScore': 1834}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- KeptAligned ratio≈ 1.00.