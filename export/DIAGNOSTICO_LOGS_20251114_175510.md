# Informe Diagnóstico de Logs - 2025-11-14 18:02:20

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_175510.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251114_175510.csv`

## DFM
- Eventos de evaluación: 941
- Evaluaciones Bull: 116 | Bear: 744
- Pasaron umbral (PassedThreshold): 860
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:51, 6:347, 7:360, 8:102, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2369
- KeptAligned: 3678/3678 | KeptCounter: 2921/3056
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.385 | AvgProxCounter≈ 0.233
  - AvgDistATRAligned≈ 1.42 | AvgDistATRCounter≈ 1.30
- PreferAligned eventos: 1141 | Filtradas contra-bias: 466

### Proximity (Pre-PreferAligned)
- Eventos: 2369
- Aligned pre: 3678/6599 | Counter pre: 2921/6599
- AvgProxAligned(pre)≈ 0.385 | AvgDistATRAligned(pre)≈ 1.42

### Proximity Drivers
- Eventos: 2369
- Alineadas: n=3678 | BaseProx≈ 0.753 | ZoneATR≈ 5.03 | SizePenalty≈ 0.977 | FinalProx≈ 0.737
- Contra-bias: n=2455 | BaseProx≈ 0.490 | ZoneATR≈ 4.97 | SizePenalty≈ 0.976 | FinalProx≈ 0.480

## Risk
- Eventos: 1965
- Accepted=1282 | RejSL=0 | RejTP=0 | RejRR=1339 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 335 (11.7% del total)
  - Avg Score: 0.39 | Avg R:R: 1.91 | Avg DistATR: 3.81
  - Por TF: TF5=116, TF15=219
- **P0_SWING_LITE:** 2522 (88.3% del total)
  - Avg Score: 0.60 | Avg R:R: 4.57 | Avg DistATR: 3.50
  - Por TF: TF15=534, TF60=1988


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 86 | Unmatched: 1227
- 0-10: Wins=33 Losses=53 WR=38.4% (n=86)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=33 Losses=53 WR=38.4% (n=86)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1313 | Aligned=768 (58.5%)
- Core≈ 1.00 | Prox≈ 0.65 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.09 | Confidence≈ 0.00
- SL_TF dist: {'5': 175, '60': 163, '15': 946, '240': 29} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 236, '5': 325, '15': 431, '240': 321} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1282, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=170, 15m=927, 60m=161, 240m=24, 1440m=0
- RR plan por bandas: 0-10≈ 2.06 (n=1282), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10485 | Zonas con Anchors: 10476
- Dir zonas (zona): Bull=2895 Bear=7208 Neutral=382
- Resumen por ciclo (promedios): TotHZ≈ 4.2, WithAnchors≈ 4.2, DirBull≈ 1.2, DirBear≈ 2.9, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 9987, 'tie-bias': 489, 'triggers-only': 9}
- TF Triggers: {'5': 5530, '15': 4955}
- TF Anchors: {'60': 10378, '240': 9848, '1440': 8616}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 21, 'score decayó a 0,21': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,27': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 193 | Ejecutadas: 49 | Canceladas: 0 | Expiradas: 0
- BUY: 54 | SELL: 188

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 860
- Registered: 101
  - DEDUP_COOLDOWN: 16 | DEDUP_IDENTICAL: 91 | SKIP_CONCURRENCY: 104
- Intentos de registro: 312

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 36.3%
- RegRate = Registered / Intentos = 32.4%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 34.3%
- Concurrency = SKIP_CONCURRENCY / Intentos = 33.3%
- ExecRate = Ejecutadas / Registered = 48.5%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5198 | Total candidatos: 47870 | Seleccionados: 0
- Candidatos por zona (promedio): 9.2

### Take Profit (TP)
- Zonas analizadas: 5132 | Total candidatos: 92467 | Seleccionados: 5132
- Candidatos por zona (promedio): 18.0
- **Edad (barras)** - Candidatos: med=35, max=182 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.69
- **Priority Candidatos**: {'P3': 92467}
- **Priority Seleccionados**: {'NA': 1281, 'P3': 3618, 'P0': 233}
- **Type Candidatos**: {'Swing': 92467}
- **Type Seleccionados**: {'P4_Fallback': 1281, 'P3_Swing': 3618, 'P0_Zone': 233}
- **TF Candidatos**: {240: 35014, 60: 23874, 5: 19110, 15: 14469}
- **TF Seleccionados**: {-1: 1281, 60: 633, 5: 963, 15: 1117, 240: 1138}
- **DistATR** - Candidatos: avg=13.2 | Seleccionados: avg=5.3
- **RR** - Candidatos: avg=5.83 | Seleccionados: avg=1.38
- **Razones de selección**: {'BestIntelligentScore': 5132}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.