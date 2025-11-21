# Informe Diagnóstico de Logs - 2025-11-17 21:29:18

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251117_212507.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251117_212507.csv`

## DFM
- Eventos de evaluación: 794
- Evaluaciones Bull: 87 | Bear: 551
- Pasaron umbral (PassedThreshold): 638
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:27, 6:259, 7:283, 8:69, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2539
- KeptAligned: 2068/2068 | KeptCounter: 2346/2477
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.223 | AvgProxCounter≈ 0.173
  - AvgDistATRAligned≈ 0.91 | AvgDistATRCounter≈ 1.02
- PreferAligned eventos: 711 | Filtradas contra-bias: 486

### Proximity (Pre-PreferAligned)
- Eventos: 2539
- Aligned pre: 2068/4414 | Counter pre: 2346/4414
- AvgProxAligned(pre)≈ 0.223 | AvgDistATRAligned(pre)≈ 0.91

### Proximity Drivers
- Eventos: 2539
- Alineadas: n=2068 | BaseProx≈ 0.729 | ZoneATR≈ 4.75 | SizePenalty≈ 0.978 | FinalProx≈ 0.714
- Contra-bias: n=1860 | BaseProx≈ 0.463 | ZoneATR≈ 5.38 | SizePenalty≈ 0.968 | FinalProx≈ 0.449

## Risk
- Eventos: 1379
- Accepted=1065 | RejSL=0 | RejTP=0 | RejRR=534 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 148 (8.0% del total)
  - Avg Score: 0.37 | Avg R:R: 1.78 | Avg DistATR: 3.97
  - Por TF: TF5=42, TF15=106
- **P0_SWING_LITE:** 1703 (92.0% del total)
  - Avg Score: 0.64 | Avg R:R: 5.63 | Avg DistATR: 4.01
  - Por TF: TF15=258, TF60=1445


### SLPick por Bandas y TF
- Bandas: lt8=1065, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=198, 15m=648, 60m=155, 240m=64, 1440m=0
- RR plan por bandas: 0-10≈ 2.27 (n=1065), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 12179 | Zonas con Anchors: 12179
- Dir zonas (zona): Bull=2772 Bear=9020 Neutral=387
- Resumen por ciclo (promedios): TotHZ≈ 3.9, WithAnchors≈ 3.9, DirBull≈ 0.9, DirBear≈ 2.9, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 11740, 'tie-bias': 439}
- TF Triggers: {'5': 5818, '15': 6361}
- TF Anchors: {'60': 12143, '240': 12179, '1440': 12179}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 638
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
- Zonas analizadas: 3383 | Total candidatos: 22704 | Seleccionados: 0
- Candidatos por zona (promedio): 6.7

### Take Profit (TP)
- Zonas analizadas: 3351 | Total candidatos: 52327 | Seleccionados: 3351
- Candidatos por zona (promedio): 15.6
- **Edad (barras)** - Candidatos: med=37, max=157 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.70
- **Priority Candidatos**: {'P3': 23735}
- **Priority Seleccionados**: {'P3': 2919, 'NA': 358, 'P0': 74}
- **Type Candidatos**: {'Swing': 23735}
- **Type Seleccionados**: {'P3_Swing': 2919, 'P4_Fallback': 358, 'P0_Zone': 74}
- **TF Candidatos**: {240: 10795, 60: 4392, 15: 4374, 5: 4174}
- **TF Seleccionados**: {240: 1329, -1: 358, 15: 712, 5: 478, 60: 474}
- **DistATR** - Candidatos: avg=11.0 | Seleccionados: avg=5.6
- **RR** - Candidatos: avg=5.10 | Seleccionados: avg=1.50
- **Razones de selección**: {'BestIntelligentScore': 3351}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- KeptAligned ratio≈ 1.00.