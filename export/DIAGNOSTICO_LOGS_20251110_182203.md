# Informe Diagnóstico de Logs - 2025-11-10 18:48:43

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251110_182203.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251110_182203.csv`

## DFM
- Eventos de evaluación: 3767
- Evaluaciones Bull: 2626 | Bear: 2593
- Pasaron umbral (PassedThreshold): 5219
- ConfidenceBins acumulado: 0:0, 1:0, 2:5, 3:72, 4:209, 5:675, 6:1584, 7:1948, 8:726, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 8737
- KeptAligned: 19624/20430 | KeptCounter: 11630/14603
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.500 | AvgProxCounter≈ 0.325
  - AvgDistATRAligned≈ 3.62 | AvgDistATRCounter≈ 2.07
- PreferAligned eventos: 6558 | Filtradas contra-bias: 6379

### Proximity (Pre-PreferAligned)
- Eventos: 8737
- Aligned pre: 19624/31254 | Counter pre: 11630/31254
- AvgProxAligned(pre)≈ 0.500 | AvgDistATRAligned(pre)≈ 3.62

### Proximity Drivers
- Eventos: 8737
- Alineadas: n=19624 | BaseProx≈ 0.715 | ZoneATR≈ 6.37 | SizePenalty≈ 0.957 | FinalProx≈ 0.687
- Contra-bias: n=5251 | BaseProx≈ 0.567 | ZoneATR≈ 6.12 | SizePenalty≈ 0.958 | FinalProx≈ 0.546

## Risk
- Eventos: 8374
- Accepted=5315 | RejSL=0 | RejTP=0 | RejRR=6532 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 2889 (40.7% del total)
  - Avg Score: 0.64 | Avg R:R: 1.97 | Avg DistATR: 9.42
  - Por TF: TF5=1992, TF15=897
- **P0_SWING_LITE:** 4208 (59.3% del total)
  - Avg Score: 0.39 | Avg R:R: 8.03 | Avg DistATR: 9.61
  - Por TF: TF15=2365, TF60=1843


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 830 | Unmatched: 4485
- 0-10: Wins=302 Losses=526 WR=36.5% (n=828)
- 10-15: Wins=0 Losses=2 WR=0.0% (n=2)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=302 Losses=528 WR=36.4% (n=830)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 5315 | Aligned=4050 (76.2%)
- Core≈ 0.97 | Prox≈ 0.69 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.71 | Confidence≈ 0.00
- SL_TF dist: {'5': 959, '15': 4007, '60': 341, '240': 8} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 2153, '5': 1201, '60': 1646, '240': 315} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=5142, 8-10=90, 10-12.5=61, 12.5-15=20, >15=2
- TF: 5m=959, 15m=4007, 60m=341, 240m=8, 1440m=0
- RR plan por bandas: 0-10≈ 1.72 (n=5232), 10-15≈ 1.14 (n=81)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 99778 | Zonas con Anchors: 62143
- Dir zonas (zona): Bull=34350 Bear=61641 Neutral=3787
- Resumen por ciclo (promedios): TotHZ≈ 3.5, WithAnchors≈ 2.2, DirBull≈ 1.8, DirBear≈ 1.5, DirNeutral≈ 0.2
- Razones de dirección: {'triggers-only': 36111, 'tie-bias': 3787, 'anchors+triggers': 59880}
- TF Triggers: {'5': 20454, '15': 14579}
- TF Anchors: {'60': 21574, '240': 4984, '1440': 408}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 547, 'estructura inactiva': 1}
- Cancel_BOS (diag): por acción {'BUY': 46, 'SELL': 102} | por bias {'Bullish': 102, 'Bearish': 46, 'Neutral': 0}

## CSV de Trades
- Filas: 3485 | Ejecutadas: 541 | Canceladas: 0 | Expiradas: 0
- BUY: 2457 | SELL: 1569

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 5219
- Registered: 1793
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 1750
- Intentos de registro: 3543

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 67.9%
- RegRate = Registered / Intentos = 50.6%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 49.4%
- ExecRate = Ejecutadas / Registered = 30.2%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 20606 | Total candidatos: 214802 | Seleccionados: 0
- Candidatos por zona (promedio): 10.4

### Take Profit (TP)
- Zonas analizadas: 20606 | Total candidatos: 205280 | Seleccionados: 0
- Candidatos por zona (promedio): 10.0
- **Edad (barras)** - Candidatos: med=50, max=302 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 205280}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 205280}
- **Type Seleccionados**: {}
- **TF Candidatos**: {5: 83225, 15: 66219, 60: 48848, 240: 6988}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=8.1 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=3.06 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- KeptAligned ratio≈ 0.96.