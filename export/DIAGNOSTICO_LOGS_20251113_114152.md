# Informe Diagnóstico de Logs - 2025-11-13 11:46:57

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_114152.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_114152.csv`

## DFM
- Eventos de evaluación: 949
- Evaluaciones Bull: 161 | Bear: 677
- Pasaron umbral (PassedThreshold): 838
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:72, 6:362, 7:354, 8:50, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2356
- KeptAligned: 4164/4164 | KeptCounter: 2732/2838
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.448 | AvgProxCounter≈ 0.231
  - AvgDistATRAligned≈ 1.52 | AvgDistATRCounter≈ 1.14
- PreferAligned eventos: 1282 | Filtradas contra-bias: 575

### Proximity (Pre-PreferAligned)
- Eventos: 2356
- Aligned pre: 4164/6896 | Counter pre: 2732/6896
- AvgProxAligned(pre)≈ 0.448 | AvgDistATRAligned(pre)≈ 1.52

### Proximity Drivers
- Eventos: 2356
- Alineadas: n=4164 | BaseProx≈ 0.752 | ZoneATR≈ 5.18 | SizePenalty≈ 0.975 | FinalProx≈ 0.734
- Contra-bias: n=2157 | BaseProx≈ 0.526 | ZoneATR≈ 4.83 | SizePenalty≈ 0.978 | FinalProx≈ 0.516

## Risk
- Eventos: 1949
- Accepted=1271 | RejSL=0 | RejTP=0 | RejRR=1248 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 307 (10.7% del total)
  - Avg Score: 0.39 | Avg R:R: 1.90 | Avg DistATR: 3.75
  - Por TF: TF5=76, TF15=231
- **P0_SWING_LITE:** 2555 (89.3% del total)
  - Avg Score: 0.58 | Avg R:R: 4.17 | Avg DistATR: 3.48
  - Por TF: TF15=547, TF60=2008


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 80 | Unmatched: 1228
- 0-10: Wins=36 Losses=44 WR=45.0% (n=80)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=36 Losses=44 WR=45.0% (n=80)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1308 | Aligned=803 (61.4%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.09 | Confidence≈ 0.00
- SL_TF dist: {'5': 192, '60': 144, '15': 959, '240': 13} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 294, '5': 349, '15': 488, '240': 177} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1271, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=184, 15m=938, 60m=140, 240m=9, 1440m=0
- RR plan por bandas: 0-10≈ 2.07 (n=1271), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10337 | Zonas con Anchors: 10323
- Dir zonas (zona): Bull=3792 Bear=6204 Neutral=341
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9916, 'tie-bias': 407, 'triggers-only': 14}
- TF Triggers: {'5': 5461, '15': 4876}
- TF Anchors: {'60': 10250, '240': 6006, '1440': 483}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,46': 1, 'estructura no existe': 26, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,42': 1, 'score decayó a 0,34': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 195 | Ejecutadas: 34 | Canceladas: 0 | Expiradas: 0
- BUY: 71 | SELL: 158

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 838
- Registered: 103
  - DEDUP_COOLDOWN: 18 | DEDUP_IDENTICAL: 100 | SKIP_CONCURRENCY: 100
- Intentos de registro: 321

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 38.3%
- RegRate = Registered / Intentos = 32.1%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 36.8%
- Concurrency = SKIP_CONCURRENCY / Intentos = 31.2%
- ExecRate = Ejecutadas / Registered = 33.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5438 | Total candidatos: 42934 | Seleccionados: 0
- Candidatos por zona (promedio): 7.9

### Take Profit (TP)
- Zonas analizadas: 5333 | Total candidatos: 51401 | Seleccionados: 5333
- Candidatos por zona (promedio): 9.6
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 51401}
- **Priority Seleccionados**: {'P3': 3647, 'NA': 1372, 'P0': 314}
- **Type Candidatos**: {'Swing': 51401}
- **Type Seleccionados**: {'P3_Swing': 3647, 'P4_Fallback': 1372, 'P0_Zone': 314}
- **TF Candidatos**: {5: 15402, 15: 13958, 60: 13567, 240: 8474}
- **TF Seleccionados**: {60: 989, -1: 1372, 5: 1009, 15: 1251, 240: 712}
- **DistATR** - Candidatos: avg=8.6 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.52 | Seleccionados: avg=1.32
- **Razones de selección**: {'BestIntelligentScore': 5333}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.