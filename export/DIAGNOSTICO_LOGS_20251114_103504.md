# Informe Diagnóstico de Logs - 2025-11-14 10:40:47

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_103504.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251114_103504.csv`

## DFM
- Eventos de evaluación: 907
- Evaluaciones Bull: 121 | Bear: 650
- Pasaron umbral (PassedThreshold): 771
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:47, 6:348, 7:303, 8:73, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2354
- KeptAligned: 3347/3347 | KeptCounter: 3081/3234
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.386 | AvgProxCounter≈ 0.250
  - AvgDistATRAligned≈ 1.36 | AvgDistATRCounter≈ 1.30
- PreferAligned eventos: 1112 | Filtradas contra-bias: 574

### Proximity (Pre-PreferAligned)
- Eventos: 2354
- Aligned pre: 3347/6428 | Counter pre: 3081/6428
- AvgProxAligned(pre)≈ 0.386 | AvgDistATRAligned(pre)≈ 1.36

### Proximity Drivers
- Eventos: 2354
- Alineadas: n=3347 | BaseProx≈ 0.760 | ZoneATR≈ 4.98 | SizePenalty≈ 0.977 | FinalProx≈ 0.744
- Contra-bias: n=2507 | BaseProx≈ 0.496 | ZoneATR≈ 4.85 | SizePenalty≈ 0.978 | FinalProx≈ 0.487

## Risk
- Eventos: 1926
- Accepted=1214 | RejSL=0 | RejTP=0 | RejRR=1238 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 286 (10.2% del total)
  - Avg Score: 0.39 | Avg R:R: 1.94 | Avg DistATR: 3.75
  - Por TF: TF5=82, TF15=204
- **P0_SWING_LITE:** 2507 (89.8% del total)
  - Avg Score: 0.54 | Avg R:R: 4.68 | Avg DistATR: 3.63
  - Por TF: TF15=494, TF60=2013


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 69 | Unmatched: 1185
- 0-10: Wins=18 Losses=51 WR=26.1% (n=69)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=18 Losses=51 WR=26.1% (n=69)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1254 | Aligned=702 (56.0%)
- Core≈ 1.00 | Prox≈ 0.65 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.06 | Confidence≈ 0.00
- SL_TF dist: {'60': 182, '5': 205, '15': 834, '240': 33} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 252, '5': 300, '15': 436, '240': 266} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1214, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=199, 15m=811, 60m=177, 240m=27, 1440m=0
- RR plan por bandas: 0-10≈ 2.04 (n=1214), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10417 | Zonas con Anchors: 10409
- Dir zonas (zona): Bull=3489 Bear=6480 Neutral=448
- Resumen por ciclo (promedios): TotHZ≈ 4.2, WithAnchors≈ 4.2, DirBull≈ 1.4, DirBear≈ 2.6, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 9843, 'tie-bias': 566, 'triggers-only': 8}
- TF Triggers: {'5': 5504, '15': 4913}
- TF Anchors: {'60': 10336, '240': 9661, '1440': 8103}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 15, 'score decayó a 0,21': 2, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,32': 1, 'score decayó a 0,22': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 166 | Ejecutadas: 38 | Canceladas: 0 | Expiradas: 0
- BUY: 54 | SELL: 150

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 771
- Registered: 88
  - DEDUP_COOLDOWN: 18 | DEDUP_IDENTICAL: 76 | SKIP_CONCURRENCY: 93
- Intentos de registro: 275

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 35.7%
- RegRate = Registered / Intentos = 32.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 34.2%
- Concurrency = SKIP_CONCURRENCY / Intentos = 33.8%
- ExecRate = Ejecutadas / Registered = 43.2%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4927 | Total candidatos: 39676 | Seleccionados: 0
- Candidatos por zona (promedio): 8.1

### Take Profit (TP)
- Zonas analizadas: 4858 | Total candidatos: 74519 | Seleccionados: 4858
- Candidatos por zona (promedio): 15.3
- **Edad (barras)** - Candidatos: med=36, max=192 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.50 | Seleccionados: avg=0.69
- **Priority Candidatos**: {'P3': 74519}
- **Priority Seleccionados**: {'P3': 3387, 'NA': 1263, 'P0': 208}
- **Type Candidatos**: {'Swing': 74519}
- **Type Seleccionados**: {'P3_Swing': 3387, 'P4_Fallback': 1263, 'P0_Zone': 208}
- **TF Candidatos**: {240: 32886, 60: 14425, 5: 13795, 15: 13413}
- **TF Seleccionados**: {60: 671, -1: 1263, 5: 876, 15: 1072, 240: 976}
- **DistATR** - Candidatos: avg=13.5 | Seleccionados: avg=5.2
- **RR** - Candidatos: avg=5.92 | Seleccionados: avg=1.39
- **Razones de selección**: {'BestIntelligentScore': 4858}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.