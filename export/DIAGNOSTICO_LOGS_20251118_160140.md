# Informe Diagnóstico de Logs - 2025-11-18 16:05:38

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251118_160140.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251118_160140.csv`

## DFM
- Eventos de evaluación: 250
- Evaluaciones Bull: 12 | Bear: 205
- Pasaron umbral (PassedThreshold): 217
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:3, 6:59, 7:93, 8:62, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 626
- KeptAligned: 959/959 | KeptCounter: 1107/1147
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.379 | AvgProxCounter≈ 0.254
  - AvgDistATRAligned≈ 1.28 | AvgDistATRCounter≈ 1.73
- PreferAligned eventos: 266 | Filtradas contra-bias: 147

### Proximity (Pre-PreferAligned)
- Eventos: 626
- Aligned pre: 959/2066 | Counter pre: 1107/2066
- AvgProxAligned(pre)≈ 0.379 | AvgDistATRAligned(pre)≈ 1.28

### Proximity Drivers
- Eventos: 626
- Alineadas: n=959 | BaseProx≈ 0.772 | ZoneATR≈ 4.87 | SizePenalty≈ 0.981 | FinalProx≈ 0.757
- Contra-bias: n=960 | BaseProx≈ 0.438 | ZoneATR≈ 4.70 | SizePenalty≈ 0.979 | FinalProx≈ 0.430

## Risk
- Eventos: 559
- Accepted=343 | RejSL=0 | RejTP=0 | RejRR=322 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 90 (11.5% del total)
  - Avg Score: 0.37 | Avg R:R: 1.84 | Avg DistATR: 3.69
  - Por TF: TF5=33, TF15=57
- **P0_SWING_LITE:** 694 (88.5% del total)
  - Avg Score: 0.62 | Avg R:R: 5.19 | Avg DistATR: 3.60
  - Por TF: TF15=148, TF60=546


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 15 | Unmatched: 336
- 0-10: Wins=5 Losses=10 WR=33.3% (n=15)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=5 Losses=10 WR=33.3% (n=15)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 351 | Aligned=156 (44.4%)
- Core≈ 1.00 | Prox≈ 0.61 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.36 | Confidence≈ 0.00
- SL_TF dist: {'60': 33, '15': 243, '5': 52, '240': 23} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 42, '15': 113, '5': 93, '240': 103} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=343, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=50, 15m=241, 60m=33, 240m=19, 1440m=0
- RR plan por bandas: 0-10≈ 2.27 (n=343), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 3222 | Zonas con Anchors: 3222
- Dir zonas (zona): Bull=366 Bear=2741 Neutral=115
- Resumen por ciclo (promedios): TotHZ≈ 5.1, WithAnchors≈ 5.1, DirBull≈ 0.6, DirBear≈ 4.4, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 3090, 'tie-bias': 132}
- TF Triggers: {'5': 1570, '15': 1652}
- TF Anchors: {'60': 3190, '240': 3222, '1440': 3222}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 7, 'score decayó a 0,22': 1, 'score decayó a 0,24': 1}

## CSV de Trades
- Filas: 54 | Ejecutadas: 11 | Canceladas: 0 | Expiradas: 0
- BUY: 7 | SELL: 58

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 217
- Registered: 27
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 27

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 12.4%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 40.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 1473 | Total candidatos: 13136 | Seleccionados: 0
- Candidatos por zona (promedio): 8.9

### Take Profit (TP)
- Zonas analizadas: 1458 | Total candidatos: 17925 | Seleccionados: 1458
- Candidatos por zona (promedio): 12.3
- **Edad (barras)** - Candidatos: med=37, max=157 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.68
- **Priority Candidatos**: {'P3': 17925}
- **Priority Seleccionados**: {'P3': 1101, 'NA': 302, 'P0': 55}
- **Type Candidatos**: {'Swing': 17925}
- **Type Seleccionados**: {'P3_Swing': 1101, 'P4_Fallback': 302, 'P0_Zone': 55}
- **TF Candidatos**: {240: 6467, 15: 4126, 5: 4114, 60: 3218}
- **TF Seleccionados**: {60: 150, -1: 302, 15: 279, 5: 284, 240: 443}
- **DistATR** - Candidatos: avg=12.2 | Seleccionados: avg=6.1
- **RR** - Candidatos: avg=5.44 | Seleccionados: avg=1.43
- **Razones de selección**: {'BestIntelligentScore': 1458}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.