# Informe Diagnóstico de Logs - 2025-11-18 16:26:19

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251118_161909.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251118_161909.csv`

## DFM
- Eventos de evaluación: 218
- Evaluaciones Bull: 0 | Bear: 171
- Pasaron umbral (PassedThreshold): 171
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:0, 6:54, 7:78, 8:39, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 616
- KeptAligned: 794/794 | KeptCounter: 654/706
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.312 | AvgProxCounter≈ 0.254
  - AvgDistATRAligned≈ 1.07 | AvgDistATRCounter≈ 1.37
- PreferAligned eventos: 234 | Filtradas contra-bias: 34

### Proximity (Pre-PreferAligned)
- Eventos: 616
- Aligned pre: 794/1448 | Counter pre: 654/1448
- AvgProxAligned(pre)≈ 0.312 | AvgDistATRAligned(pre)≈ 1.07

### Proximity Drivers
- Eventos: 616
- Alineadas: n=794 | BaseProx≈ 0.769 | ZoneATR≈ 5.21 | SizePenalty≈ 0.973 | FinalProx≈ 0.749
- Contra-bias: n=620 | BaseProx≈ 0.498 | ZoneATR≈ 5.36 | SizePenalty≈ 0.970 | FinalProx≈ 0.484

## Risk
- Eventos: 532
- Accepted=272 | RejSL=0 | RejTP=0 | RejRR=228 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 85 (10.3% del total)
  - Avg Score: 0.38 | Avg R:R: 1.86 | Avg DistATR: 4.11
  - Por TF: TF5=46, TF15=39
- **P0_SWING_LITE:** 741 (89.7% del total)
  - Avg Score: 0.63 | Avg R:R: 5.22 | Avg DistATR: 3.70
  - Por TF: TF15=240, TF60=501


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 16 | Unmatched: 256
- 0-10: Wins=5 Losses=11 WR=31.2% (n=16)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=5 Losses=11 WR=31.2% (n=16)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 272 | Aligned=160 (58.8%)
- Core≈ 1.00 | Prox≈ 0.70 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.04 | Confidence≈ 0.00
- SL_TF dist: {'60': 57, '15': 53, '5': 146, '240': 16} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 56, '5': 164, '15': 26, '240': 26} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=272, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=146, 15m=53, 60m=57, 240m=16, 1440m=0
- RR plan por bandas: 0-10≈ 2.04 (n=272), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 2487 | Zonas con Anchors: 2487
- Dir zonas (zona): Bull=286 Bear=2133 Neutral=68
- Resumen por ciclo (promedios): TotHZ≈ 4.0, WithAnchors≈ 4.0, DirBull≈ 0.5, DirBear≈ 3.4, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 2382, 'tie-bias': 105}
- TF Triggers: {'15': 915, '5': 1572}
- TF Anchors: {'60': 2358, '240': 2487, '1440': 2487}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 10, 'score decayó a 0,15': 1, 'score decayó a 0,43': 1, 'score decayó a 0,26': 1}

## CSV de Trades
- Filas: 71 | Ejecutadas: 10 | Canceladas: 0 | Expiradas: 0
- BUY: 0 | SELL: 81

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 171
- Registered: 43
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 43

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 25.1%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 23.3%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 1290 | Total candidatos: 16268 | Seleccionados: 0
- Candidatos por zona (promedio): 12.6

### Take Profit (TP)
- Zonas analizadas: 1289 | Total candidatos: 28366 | Seleccionados: 1289
- Candidatos por zona (promedio): 22.0
- **Edad (barras)** - Candidatos: med=249, max=728 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.68
- **Priority Candidatos**: {'P3': 28366}
- **Priority Seleccionados**: {'P3': 928, 'NA': 312, 'P0': 49}
- **Type Candidatos**: {'Swing': 28366}
- **Type Seleccionados**: {'P3_Swing': 928, 'P4_Fallback': 312, 'P0_Zone': 49}
- **TF Candidatos**: {15: 17894, 240: 4453, 5: 3641, 60: 2378}
- **TF Seleccionados**: {60: 160, 15: 240, -1: 312, 5: 374, 240: 203}
- **DistATR** - Candidatos: avg=16.5 | Seleccionados: avg=6.3
- **RR** - Candidatos: avg=7.07 | Seleccionados: avg=1.38
- **Razones de selección**: {'BestIntelligentScore': 1289}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.