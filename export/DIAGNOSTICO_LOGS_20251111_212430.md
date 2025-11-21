# Informe Diagnóstico de Logs - 2025-11-11 21:28:27

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_212430.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_212430.csv`

## DFM
- Eventos de evaluación: 182
- Evaluaciones Bull: 59 | Bear: 140
- Pasaron umbral (PassedThreshold): 180
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:4, 4:15, 5:12, 6:98, 7:61, 8:9, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2216
- KeptAligned: 1847/1847 | KeptCounter: 1268/1268
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.368 | AvgProxCounter≈ 0.234
  - AvgDistATRAligned≈ 0.53 | AvgDistATRCounter≈ 0.38
- PreferAligned eventos: 840 | Filtradas contra-bias: 232

### Proximity (Pre-PreferAligned)
- Eventos: 2216
- Aligned pre: 1847/3115 | Counter pre: 1268/3115
- AvgProxAligned(pre)≈ 0.368 | AvgDistATRAligned(pre)≈ 0.53

### Proximity Drivers
- Eventos: 2216
- Alineadas: n=1847 | BaseProx≈ 0.872 | ZoneATR≈ 5.02 | SizePenalty≈ 0.978 | FinalProx≈ 0.853
- Contra-bias: n=1036 | BaseProx≈ 0.763 | ZoneATR≈ 4.65 | SizePenalty≈ 0.981 | FinalProx≈ 0.748

## Risk
- Eventos: 1411
- Accepted=199 | RejSL=0 | RejTP=0 | RejRR=173 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 34 (8.0% del total)
  - Avg Score: 0.41 | Avg R:R: 1.99 | Avg DistATR: 3.55
  - Por TF: TF5=8, TF15=26
- **P0_SWING_LITE:** 393 (92.0% del total)
  - Avg Score: 0.51 | Avg R:R: 3.97 | Avg DistATR: 3.40
  - Por TF: TF15=71, TF60=322


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 55 | Unmatched: 145
- 0-10: Wins=20 Losses=35 WR=36.4% (n=55)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=20 Losses=35 WR=36.4% (n=55)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 200 | Aligned=115 (57.5%)
- Core≈ 1.00 | Prox≈ 0.80 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.78 | Confidence≈ 0.00
- SL_TF dist: {'60': 17, '5': 39, '15': 144} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 39, '60': 50, '15': 102, '240': 9} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=199, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=38, 15m=144, 60m=17, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.78 (n=199), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 39729 | Zonas con Anchors: 39715
- Dir zonas (zona): Bull=8849 Bear=29928 Neutral=952
- Resumen por ciclo (promedios): TotHZ≈ 3.7, WithAnchors≈ 3.7, DirBull≈ 1.5, DirBear≈ 2.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 38524, 'tie-bias': 1191, 'triggers-only': 14}
- TF Triggers: {'5': 5002, '15': 4290}
- TF Anchors: {'60': 9234, '240': 5463, '1440': 553}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 2}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 98 | Ejecutadas: 37 | Canceladas: 0 | Expiradas: 0
- BUY: 55 | SELL: 80

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 180
- Registered: 52
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 22 | SKIP_CONCURRENCY: 4
- Intentos de registro: 78

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 43.3%
- RegRate = Registered / Intentos = 66.7%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 28.2%
- Concurrency = SKIP_CONCURRENCY / Intentos = 5.1%
- ExecRate = Ejecutadas / Registered = 71.2%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 599 | Total candidatos: 9717 | Seleccionados: 0
- Candidatos por zona (promedio): 16.2

### Take Profit (TP)
- Zonas analizadas: 589 | Total candidatos: 4477 | Seleccionados: 589
- Candidatos por zona (promedio): 7.6
- **Edad (barras)** - Candidatos: med=53, max=182 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.40 | Seleccionados: avg=0.70
- **Priority Candidatos**: {'P3': 4477}
- **Priority Seleccionados**: {'NA': 265, 'P0': 60, 'P3': 264}
- **Type Candidatos**: {'Swing': 4477}
- **Type Seleccionados**: {'P4_Fallback': 265, 'P0_Zone': 60, 'P3_Swing': 264}
- **TF Candidatos**: {60: 1415, 15: 1186, 5: 1113, 240: 763}
- **TF Seleccionados**: {-1: 265, 5: 71, 60: 85, 15: 143, 240: 25}
- **DistATR** - Candidatos: avg=9.2 | Seleccionados: avg=3.6
- **RR** - Candidatos: avg=4.64 | Seleccionados: avg=1.34
- **Razones de selección**: {'BestIntelligentScore': 589}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.