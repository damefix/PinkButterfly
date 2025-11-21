# Informe Diagnóstico de Logs - 2025-11-14 15:49:52

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_153651.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251114_153651.csv`

## DFM
- Eventos de evaluación: 943
- Evaluaciones Bull: 119 | Bear: 727
- Pasaron umbral (PassedThreshold): 846
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:52, 6:346, 7:350, 8:98, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2368
- KeptAligned: 3640/3640 | KeptCounter: 2930/3058
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.382 | AvgProxCounter≈ 0.236
  - AvgDistATRAligned≈ 1.42 | AvgDistATRCounter≈ 1.29
- PreferAligned eventos: 1137 | Filtradas contra-bias: 458

### Proximity (Pre-PreferAligned)
- Eventos: 2368
- Aligned pre: 3640/6570 | Counter pre: 2930/6570
- AvgProxAligned(pre)≈ 0.382 | AvgDistATRAligned(pre)≈ 1.42

### Proximity Drivers
- Eventos: 2368
- Alineadas: n=3640 | BaseProx≈ 0.752 | ZoneATR≈ 5.05 | SizePenalty≈ 0.977 | FinalProx≈ 0.735
- Contra-bias: n=2472 | BaseProx≈ 0.493 | ZoneATR≈ 4.96 | SizePenalty≈ 0.977 | FinalProx≈ 0.483

## Risk
- Eventos: 1959
- Accepted=1278 | RejSL=0 | RejTP=0 | RejRR=1334 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 323 (11.3% del total)
  - Avg Score: 0.39 | Avg R:R: 1.91 | Avg DistATR: 3.84
  - Por TF: TF5=110, TF15=213
- **P0_SWING_LITE:** 2538 (88.7% del total)
  - Avg Score: 0.59 | Avg R:R: 4.57 | Avg DistATR: 3.50
  - Por TF: TF15=534, TF60=2004


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 80 | Unmatched: 1229
- 0-10: Wins=33 Losses=47 WR=41.2% (n=80)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=33 Losses=47 WR=41.2% (n=80)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1309 | Aligned=754 (57.6%)
- Core≈ 1.00 | Prox≈ 0.64 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.09 | Confidence≈ 0.00
- SL_TF dist: {'60': 173, '5': 167, '15': 940, '240': 29} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 237, '5': 314, '15': 440, '240': 318} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1278, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=162, 15m=921, 60m=171, 240m=24, 1440m=0
- RR plan por bandas: 0-10≈ 2.06 (n=1278), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10451 | Zonas con Anchors: 10443
- Dir zonas (zona): Bull=2907 Bear=7177 Neutral=367
- Resumen por ciclo (promedios): TotHZ≈ 4.2, WithAnchors≈ 4.2, DirBull≈ 1.2, DirBear≈ 2.9, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9975, 'tie-bias': 468, 'triggers-only': 8}
- TF Triggers: {'5': 5516, '15': 4935}
- TF Anchors: {'60': 10349, '240': 9792, '1440': 8560}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 22, 'score decayó a 0,21': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,27': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 188 | Ejecutadas: 45 | Canceladas: 0 | Expiradas: 0
- BUY: 54 | SELL: 179

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 846
- Registered: 100
  - DEDUP_COOLDOWN: 16 | DEDUP_IDENTICAL: 94 | SKIP_CONCURRENCY: 100
- Intentos de registro: 310

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 36.6%
- RegRate = Registered / Intentos = 32.3%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 35.5%
- Concurrency = SKIP_CONCURRENCY / Intentos = 32.3%
- ExecRate = Ejecutadas / Registered = 45.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5181 | Total candidatos: 47461 | Seleccionados: 0
- Candidatos por zona (promedio): 9.2

### Take Profit (TP)
- Zonas analizadas: 5110 | Total candidatos: 91762 | Seleccionados: 5110
- Candidatos por zona (promedio): 18.0
- **Edad (barras)** - Candidatos: med=35, max=182 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.69
- **Priority Candidatos**: {'P3': 91762}
- **Priority Seleccionados**: {'P3': 3607, 'NA': 1267, 'P0': 236}
- **Type Candidatos**: {'Swing': 91762}
- **Type Seleccionados**: {'P3_Swing': 3607, 'P4_Fallback': 1267, 'P0_Zone': 236}
- **TF Candidatos**: {240: 34570, 60: 23938, 5: 18847, 15: 14407}
- **TF Seleccionados**: {60: 647, 5: 946, -1: 1267, 15: 1120, 240: 1130}
- **DistATR** - Candidatos: avg=13.2 | Seleccionados: avg=5.3
- **RR** - Candidatos: avg=5.83 | Seleccionados: avg=1.38
- **Razones de selección**: {'BestIntelligentScore': 5110}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.