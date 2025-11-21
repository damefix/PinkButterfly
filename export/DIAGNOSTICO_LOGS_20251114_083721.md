# Informe Diagnóstico de Logs - 2025-11-14 08:39:49

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_083721.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251114_083721.csv`

## DFM
- Eventos de evaluación: 929
- Evaluaciones Bull: 135 | Bear: 695
- Pasaron umbral (PassedThreshold): 830
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:66, 6:359, 7:340, 8:65, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2355
- KeptAligned: 4214/4214 | KeptCounter: 2720/2818
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.449 | AvgProxCounter≈ 0.226
  - AvgDistATRAligned≈ 1.56 | AvgDistATRCounter≈ 1.11
- PreferAligned eventos: 1296 | Filtradas contra-bias: 554

### Proximity (Pre-PreferAligned)
- Eventos: 2355
- Aligned pre: 4214/6934 | Counter pre: 2720/6934
- AvgProxAligned(pre)≈ 0.449 | AvgDistATRAligned(pre)≈ 1.56

### Proximity Drivers
- Eventos: 2355
- Alineadas: n=4214 | BaseProx≈ 0.750 | ZoneATR≈ 5.16 | SizePenalty≈ 0.975 | FinalProx≈ 0.732
- Contra-bias: n=2166 | BaseProx≈ 0.520 | ZoneATR≈ 4.86 | SizePenalty≈ 0.978 | FinalProx≈ 0.510

## Risk
- Eventos: 1955
- Accepted=1257 | RejSL=0 | RejTP=0 | RejRR=1284 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 331 (11.6% del total)
  - Avg Score: 0.39 | Avg R:R: 1.90 | Avg DistATR: 3.81
  - Por TF: TF5=88, TF15=243
- **P0_SWING_LITE:** 2513 (88.4% del total)
  - Avg Score: 0.59 | Avg R:R: 4.13 | Avg DistATR: 3.50
  - Por TF: TF15=574, TF60=1939


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 82 | Unmatched: 1214
- 0-10: Wins=40 Losses=42 WR=48.8% (n=82)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=40 Losses=42 WR=48.8% (n=82)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1296 | Aligned=790 (61.0%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.10 | Confidence≈ 0.00
- SL_TF dist: {'15': 914, '5': 201, '60': 161, '240': 20} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 295, '5': 341, '15': 480, '240': 180} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1257, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=193, 15m=891, 60m=157, 240m=16, 1440m=0
- RR plan por bandas: 0-10≈ 2.08 (n=1257), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10402 | Zonas con Anchors: 10388
- Dir zonas (zona): Bull=3709 Bear=6388 Neutral=305
- Resumen por ciclo (promedios): TotHZ≈ 4.2, WithAnchors≈ 4.2, DirBull≈ 1.5, DirBear≈ 2.6, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 10034, 'tie-bias': 354, 'triggers-only': 14}
- TF Triggers: {'5': 5496, '15': 4906}
- TF Anchors: {'60': 10314, '240': 6092, '1440': 185}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 26, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,42': 1, 'score decayó a 0,32': 1, 'score decayó a 0,34': 1, 'score decayó a 0,22': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 192 | Ejecutadas: 35 | Canceladas: 0 | Expiradas: 0
- BUY: 58 | SELL: 169

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 830
- Registered: 101
  - DEDUP_COOLDOWN: 13 | DEDUP_IDENTICAL: 100 | SKIP_CONCURRENCY: 100
- Intentos de registro: 314

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 37.8%
- RegRate = Registered / Intentos = 32.2%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 36.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 31.8%
- ExecRate = Ejecutadas / Registered = 34.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5498 | Total candidatos: 43039 | Seleccionados: 0
- Candidatos por zona (promedio): 7.8

### Take Profit (TP)
- Zonas analizadas: 5395 | Total candidatos: 50763 | Seleccionados: 5395
- Candidatos por zona (promedio): 9.4
- **Edad (barras)** - Candidatos: med=38, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 50763}
- **Priority Seleccionados**: {'P3': 3695, 'NA': 1391, 'P0': 309}
- **Type Candidatos**: {'Swing': 50763}
- **Type Seleccionados**: {'P3_Swing': 3695, 'P4_Fallback': 1391, 'P0_Zone': 309}
- **TF Candidatos**: {5: 15651, 15: 14097, 60: 13441, 240: 7574}
- **TF Seleccionados**: {60: 1010, -1: 1391, 5: 1014, 15: 1242, 240: 738}
- **DistATR** - Candidatos: avg=8.2 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.43 | Seleccionados: avg=1.32
- **Razones de selección**: {'BestIntelligentScore': 5395}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.