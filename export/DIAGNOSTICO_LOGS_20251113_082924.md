# Informe Diagnóstico de Logs - 2025-11-13 08:33:39

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_082924.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_082924.csv`

## DFM
- Eventos de evaluación: 947
- Evaluaciones Bull: 170 | Bear: 687
- Pasaron umbral (PassedThreshold): 857
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:78, 6:373, 7:356, 8:50, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2356
- KeptAligned: 4156/4156 | KeptCounter: 2725/2831
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.445 | AvgProxCounter≈ 0.230
  - AvgDistATRAligned≈ 1.52 | AvgDistATRCounter≈ 1.13
- PreferAligned eventos: 1282 | Filtradas contra-bias: 578

### Proximity (Pre-PreferAligned)
- Eventos: 2356
- Aligned pre: 4156/6881 | Counter pre: 2725/6881
- AvgProxAligned(pre)≈ 0.445 | AvgDistATRAligned(pre)≈ 1.52

### Proximity Drivers
- Eventos: 2356
- Alineadas: n=4156 | BaseProx≈ 0.752 | ZoneATR≈ 5.17 | SizePenalty≈ 0.975 | FinalProx≈ 0.734
- Contra-bias: n=2147 | BaseProx≈ 0.527 | ZoneATR≈ 4.85 | SizePenalty≈ 0.978 | FinalProx≈ 0.517

## Risk
- Eventos: 1940
- Accepted=1276 | RejSL=0 | RejTP=0 | RejRR=1233 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 314 (10.9% del total)
  - Avg Score: 0.39 | Avg R:R: 1.89 | Avg DistATR: 3.74
  - Por TF: TF5=80, TF15=234
- **P0_SWING_LITE:** 2561 (89.1% del total)
  - Avg Score: 0.57 | Avg R:R: 4.20 | Avg DistATR: 3.48
  - Por TF: TF15=546, TF60=2015


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 83 | Unmatched: 1232
- 0-10: Wins=37 Losses=46 WR=44.6% (n=83)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=37 Losses=46 WR=44.6% (n=83)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1315 | Aligned=816 (62.1%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.09 | Confidence≈ 0.00
- SL_TF dist: {'5': 206, '60': 149, '15': 949, '240': 11} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 297, '5': 356, '15': 491, '240': 171} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1276, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=198, 15m=926, 60m=145, 240m=7, 1440m=0
- RR plan por bandas: 0-10≈ 2.07 (n=1276), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10316 | Zonas con Anchors: 10302
- Dir zonas (zona): Bull=3775 Bear=6204 Neutral=337
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9899, 'tie-bias': 403, 'triggers-only': 14}
- TF Triggers: {'5': 5460, '15': 4856}
- TF Anchors: {'60': 10229, '240': 5961, '1440': 443}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,46': 1, 'estructura no existe': 26, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,42': 1, 'score decayó a 0,34': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 202 | Ejecutadas: 36 | Canceladas: 0 | Expiradas: 0
- BUY: 78 | SELL: 160

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 857
- Registered: 106
  - DEDUP_COOLDOWN: 23 | DEDUP_IDENTICAL: 108 | SKIP_CONCURRENCY: 97
- Intentos de registro: 334

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 39.0%
- RegRate = Registered / Intentos = 31.7%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 39.2%
- Concurrency = SKIP_CONCURRENCY / Intentos = 29.0%
- ExecRate = Ejecutadas / Registered = 34.0%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5444 | Total candidatos: 43165 | Seleccionados: 0
- Candidatos por zona (promedio): 7.9

### Take Profit (TP)
- Zonas analizadas: 5337 | Total candidatos: 51147 | Seleccionados: 5337
- Candidatos por zona (promedio): 9.6
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 51147}
- **Priority Seleccionados**: {'P3': 3647, 'NA': 1368, 'P0': 322}
- **Type Candidatos**: {'Swing': 51147}
- **Type Seleccionados**: {'P3_Swing': 3647, 'P4_Fallback': 1368, 'P0_Zone': 322}
- **TF Candidatos**: {5: 15428, 15: 13927, 60: 13623, 240: 8169}
- **TF Seleccionados**: {60: 1007, 5: 1028, -1: 1368, 15: 1232, 240: 702}
- **DistATR** - Candidatos: avg=8.6 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.58 | Seleccionados: avg=1.33
- **Razones de selección**: {'BestIntelligentScore': 5337}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.