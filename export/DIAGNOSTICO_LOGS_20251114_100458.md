# Informe Diagnóstico de Logs - 2025-11-14 10:11:24

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_100458.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251114_100458.csv`

## DFM
- Eventos de evaluación: 963
- Evaluaciones Bull: 141 | Bear: 722
- Pasaron umbral (PassedThreshold): 863
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:71, 6:389, 7:328, 8:75, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2368
- KeptAligned: 4285/4285 | KeptCounter: 2667/2767
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.436 | AvgProxCounter≈ 0.210
  - AvgDistATRAligned≈ 1.57 | AvgDistATRCounter≈ 1.09
- PreferAligned eventos: 1296 | Filtradas contra-bias: 465

### Proximity (Pre-PreferAligned)
- Eventos: 2368
- Aligned pre: 4285/6952 | Counter pre: 2667/6952
- AvgProxAligned(pre)≈ 0.436 | AvgDistATRAligned(pre)≈ 1.57

### Proximity Drivers
- Eventos: 2368
- Alineadas: n=4285 | BaseProx≈ 0.747 | ZoneATR≈ 5.18 | SizePenalty≈ 0.975 | FinalProx≈ 0.729
- Contra-bias: n=2202 | BaseProx≈ 0.512 | ZoneATR≈ 4.89 | SizePenalty≈ 0.977 | FinalProx≈ 0.502

## Risk
- Eventos: 1967
- Accepted=1318 | RejSL=0 | RejTP=0 | RejRR=1323 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 326 (11.1% del total)
  - Avg Score: 0.39 | Avg R:R: 1.91 | Avg DistATR: 3.87
  - Por TF: TF5=89, TF15=237
- **P0_SWING_LITE:** 2614 (88.9% del total)
  - Avg Score: 0.61 | Avg R:R: 4.18 | Avg DistATR: 3.43
  - Por TF: TF15=583, TF60=2031


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 66 | Unmatched: 1287
- 0-10: Wins=21 Losses=45 WR=31.8% (n=66)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=21 Losses=45 WR=31.8% (n=66)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1353 | Aligned=838 (61.9%)
- Core≈ 1.00 | Prox≈ 0.66 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.09 | Confidence≈ 0.00
- SL_TF dist: {'60': 205, '15': 921, '5': 202, '240': 25} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 315, '5': 345, '15': 499, '240': 194} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1318, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=197, 15m=897, 60m=204, 240m=20, 1440m=0
- RR plan por bandas: 0-10≈ 2.06 (n=1318), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10410 | Zonas con Anchors: 10400
- Dir zonas (zona): Bull=3708 Bear=6391 Neutral=311
- Resumen por ciclo (promedios): TotHZ≈ 4.2, WithAnchors≈ 4.2, DirBull≈ 1.5, DirBear≈ 2.6, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 10053, 'tie-bias': 347, 'triggers-only': 10}
- TF Triggers: {'5': 5502, '15': 4908}
- TF Anchors: {'60': 10310, '240': 6122, '1440': 209}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 27, 'score decayó a 0,21': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,27': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 191 | Ejecutadas: 41 | Canceladas: 0 | Expiradas: 0
- BUY: 61 | SELL: 171

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 863
- Registered: 100
  - DEDUP_COOLDOWN: 16 | DEDUP_IDENTICAL: 106 | SKIP_CONCURRENCY: 87
- Intentos de registro: 309

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 35.8%
- RegRate = Registered / Intentos = 32.4%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 39.5%
- Concurrency = SKIP_CONCURRENCY / Intentos = 28.2%
- ExecRate = Ejecutadas / Registered = 41.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5582 | Total candidatos: 49955 | Seleccionados: 0
- Candidatos por zona (promedio): 8.9

### Take Profit (TP)
- Zonas analizadas: 5485 | Total candidatos: 73867 | Seleccionados: 5485
- Candidatos por zona (promedio): 13.5
- **Edad (barras)** - Candidatos: med=35, max=182 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.53 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 73867}
- **Priority Seleccionados**: {'P3': 3731, 'NA': 1460, 'P0': 294}
- **Type Candidatos**: {'Swing': 73867}
- **Type Seleccionados**: {'P3_Swing': 3731, 'P4_Fallback': 1460, 'P0_Zone': 294}
- **TF Candidatos**: {60: 22874, 5: 20506, 240: 15712, 15: 14775}
- **TF Seleccionados**: {60: 952, -1: 1460, 5: 1043, 15: 1266, 240: 764}
- **DistATR** - Candidatos: avg=9.1 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.87 | Seleccionados: avg=1.32
- **Razones de selección**: {'BestIntelligentScore': 5485}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.