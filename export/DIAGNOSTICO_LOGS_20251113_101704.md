# Informe Diagnóstico de Logs - 2025-11-13 10:19:45

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_101704.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_101704.csv`

## DFM
- Eventos de evaluación: 922
- Evaluaciones Bull: 155 | Bear: 661
- Pasaron umbral (PassedThreshold): 816
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:72, 6:348, 7:355, 8:41, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2350
- KeptAligned: 4106/4106 | KeptCounter: 2761/2864
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.438 | AvgProxCounter≈ 0.234
  - AvgDistATRAligned≈ 1.50 | AvgDistATRCounter≈ 1.14
- PreferAligned eventos: 1272 | Filtradas contra-bias: 583

### Proximity (Pre-PreferAligned)
- Eventos: 2350
- Aligned pre: 4106/6867 | Counter pre: 2761/6867
- AvgProxAligned(pre)≈ 0.438 | AvgDistATRAligned(pre)≈ 1.50

### Proximity Drivers
- Eventos: 2350
- Alineadas: n=4106 | BaseProx≈ 0.754 | ZoneATR≈ 5.20 | SizePenalty≈ 0.975 | FinalProx≈ 0.735
- Contra-bias: n=2178 | BaseProx≈ 0.526 | ZoneATR≈ 4.76 | SizePenalty≈ 0.979 | FinalProx≈ 0.517

## Risk
- Eventos: 1954
- Accepted=1251 | RejSL=0 | RejTP=0 | RejRR=1305 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 308 (11.0% del total)
  - Avg Score: 0.38 | Avg R:R: 1.89 | Avg DistATR: 3.74
  - Por TF: TF5=87, TF15=221
- **P0_SWING_LITE:** 2500 (89.0% del total)
  - Avg Score: 0.57 | Avg R:R: 4.16 | Avg DistATR: 3.52
  - Por TF: TF15=536, TF60=1964


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 99 | Unmatched: 1183
- 0-10: Wins=21 Losses=78 WR=21.2% (n=99)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=21 Losses=78 WR=21.2% (n=99)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1282 | Aligned=782 (61.0%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.08 | Confidence≈ 0.00
- SL_TF dist: {'60': 160, '15': 929, '5': 176, '240': 17} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 304, '5': 345, '15': 475, '240': 158} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1251, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=171, 15m=910, 60m=156, 240m=14, 1440m=0
- RR plan por bandas: 0-10≈ 2.06 (n=1251), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10278 | Zonas con Anchors: 10266
- Dir zonas (zona): Bull=3741 Bear=6217 Neutral=320
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9891, 'tie-bias': 375, 'triggers-only': 12}
- TF Triggers: {'5': 5445, '15': 4833}
- TF Anchors: {'60': 10187, '240': 5970, '1440': 440}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 24, 'score decayó a 0,46': 1, 'score decayó a 0,21': 1, 'score decayó a 0,20': 1, 'score decayó a 0,42': 1, 'score decayó a 0,40': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 217 | Ejecutadas: 43 | Canceladas: 0 | Expiradas: 0
- BUY: 74 | SELL: 186

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 816
- Registered: 113
  - DEDUP_COOLDOWN: 13 | DEDUP_IDENTICAL: 106 | SKIP_CONCURRENCY: 80
- Intentos de registro: 312

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 38.2%
- RegRate = Registered / Intentos = 36.2%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 38.1%
- Concurrency = SKIP_CONCURRENCY / Intentos = 25.6%
- ExecRate = Ejecutadas / Registered = 38.1%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5417 | Total candidatos: 42735 | Seleccionados: 0
- Candidatos por zona (promedio): 7.9

### Take Profit (TP)
- Zonas analizadas: 5320 | Total candidatos: 51805 | Seleccionados: 5320
- Candidatos por zona (promedio): 9.7
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 51805}
- **Priority Seleccionados**: {'P3': 3641, 'NA': 1368, 'P0': 311}
- **Type Candidatos**: {'Swing': 51805}
- **Type Seleccionados**: {'P3_Swing': 3641, 'P4_Fallback': 1368, 'P0_Zone': 311}
- **TF Candidatos**: {5: 15617, 15: 14059, 60: 13786, 240: 8343}
- **TF Seleccionados**: {60: 1008, -1: 1368, 5: 991, 15: 1241, 240: 712}
- **DistATR** - Candidatos: avg=8.5 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.47 | Seleccionados: avg=1.30
- **Razones de selección**: {'BestIntelligentScore': 5320}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.