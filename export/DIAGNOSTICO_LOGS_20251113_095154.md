# Informe Diagnóstico de Logs - 2025-11-13 09:56:32

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_095154.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_095154.csv`

## DFM
- Eventos de evaluación: 946
- Evaluaciones Bull: 167 | Bear: 684
- Pasaron umbral (PassedThreshold): 851
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:75, 6:374, 7:352, 8:50, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2355
- KeptAligned: 4160/4160 | KeptCounter: 2730/2836
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.446 | AvgProxCounter≈ 0.231
  - AvgDistATRAligned≈ 1.52 | AvgDistATRCounter≈ 1.13
- PreferAligned eventos: 1284 | Filtradas contra-bias: 567

### Proximity (Pre-PreferAligned)
- Eventos: 2355
- Aligned pre: 4160/6890 | Counter pre: 2730/6890
- AvgProxAligned(pre)≈ 0.446 | AvgDistATRAligned(pre)≈ 1.52

### Proximity Drivers
- Eventos: 2355
- Alineadas: n=4160 | BaseProx≈ 0.752 | ZoneATR≈ 5.17 | SizePenalty≈ 0.975 | FinalProx≈ 0.734
- Contra-bias: n=2163 | BaseProx≈ 0.530 | ZoneATR≈ 4.85 | SizePenalty≈ 0.978 | FinalProx≈ 0.521

## Risk
- Eventos: 1947
- Accepted=1268 | RejSL=0 | RejTP=0 | RejRR=1256 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 313 (10.9% del total)
  - Avg Score: 0.39 | Avg R:R: 1.89 | Avg DistATR: 3.76
  - Por TF: TF5=79, TF15=234
- **P0_SWING_LITE:** 2553 (89.1% del total)
  - Avg Score: 0.57 | Avg R:R: 4.19 | Avg DistATR: 3.49
  - Por TF: TF15=541, TF60=2012


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 90 | Unmatched: 1216
- 0-10: Wins=36 Losses=54 WR=40.0% (n=90)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=36 Losses=54 WR=40.0% (n=90)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1306 | Aligned=810 (62.0%)
- Core≈ 1.00 | Prox≈ 0.68 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.08 | Confidence≈ 0.00
- SL_TF dist: {'60': 156, '5': 193, '15': 946, '240': 11} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 292, '5': 358, '15': 486, '240': 170} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1268, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=185, 15m=924, 60m=152, 240m=7, 1440m=0
- RR plan por bandas: 0-10≈ 2.06 (n=1268), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10312 | Zonas con Anchors: 10298
- Dir zonas (zona): Bull=3758 Bear=6222 Neutral=332
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9902, 'tie-bias': 396, 'triggers-only': 14}
- TF Triggers: {'15': 4868, '5': 5444}
- TF Anchors: {'60': 10224, '240': 5983, '1440': 460}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,46': 1, 'estructura no existe': 26, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,42': 1, 'score decayó a 0,32': 1, 'score decayó a 0,34': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 196 | Ejecutadas: 36 | Canceladas: 0 | Expiradas: 0
- BUY: 72 | SELL: 160

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 851
- Registered: 103
  - DEDUP_COOLDOWN: 18 | DEDUP_IDENTICAL: 108 | SKIP_CONCURRENCY: 98
- Intentos de registro: 327

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 38.4%
- RegRate = Registered / Intentos = 31.5%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 38.5%
- Concurrency = SKIP_CONCURRENCY / Intentos = 30.0%
- ExecRate = Ejecutadas / Registered = 35.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5452 | Total candidatos: 43353 | Seleccionados: 0
- Candidatos por zona (promedio): 8.0

### Take Profit (TP)
- Zonas analizadas: 5344 | Total candidatos: 51144 | Seleccionados: 5344
- Candidatos por zona (promedio): 9.6
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 51144}
- **Priority Seleccionados**: {'P3': 3653, 'NA': 1371, 'P0': 320}
- **Type Candidatos**: {'Swing': 51144}
- **Type Seleccionados**: {'P3_Swing': 3653, 'P4_Fallback': 1371, 'P0_Zone': 320}
- **TF Candidatos**: {5: 15356, 15: 13905, 60: 13668, 240: 8215}
- **TF Seleccionados**: {60: 1005, 5: 1030, -1: 1371, 15: 1238, 240: 700}
- **DistATR** - Candidatos: avg=8.6 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.57 | Seleccionados: avg=1.32
- **Razones de selección**: {'BestIntelligentScore': 5344}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.