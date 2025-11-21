# Informe Diagnóstico de Logs - 2025-11-12 08:37:44

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251112_083355.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251112_083355.csv`

## DFM
- Eventos de evaluación: 190
- Evaluaciones Bull: 68 | Bear: 140
- Pasaron umbral (PassedThreshold): 188
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:3, 4:17, 5:19, 6:101, 7:59, 8:9, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2227
- KeptAligned: 1817/1817 | KeptCounter: 1248/1248
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.357 | AvgProxCounter≈ 0.229
  - AvgDistATRAligned≈ 0.52 | AvgDistATRCounter≈ 0.36
- PreferAligned eventos: 830 | Filtradas contra-bias: 199

### Proximity (Pre-PreferAligned)
- Eventos: 2227
- Aligned pre: 1817/3065 | Counter pre: 1248/3065
- AvgProxAligned(pre)≈ 0.357 | AvgDistATRAligned(pre)≈ 0.52

### Proximity Drivers
- Eventos: 2227
- Alineadas: n=1817 | BaseProx≈ 0.871 | ZoneATR≈ 5.00 | SizePenalty≈ 0.979 | FinalProx≈ 0.853
- Contra-bias: n=1049 | BaseProx≈ 0.766 | ZoneATR≈ 4.61 | SizePenalty≈ 0.982 | FinalProx≈ 0.752

## Risk
- Eventos: 1402
- Accepted=208 | RejSL=0 | RejTP=0 | RejRR=164 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 34 (7.9% del total)
  - Avg Score: 0.41 | Avg R:R: 2.00 | Avg DistATR: 3.53
  - Por TF: TF5=8, TF15=26
- **P0_SWING_LITE:** 395 (92.1% del total)
  - Avg Score: 0.50 | Avg R:R: 4.07 | Avg DistATR: 3.40
  - Por TF: TF15=72, TF60=323


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 57 | Unmatched: 152
- 0-10: Wins=22 Losses=35 WR=38.6% (n=57)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=22 Losses=35 WR=38.6% (n=57)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 209 | Aligned=116 (55.5%)
- Core≈ 1.00 | Prox≈ 0.80 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.73 | Confidence≈ 0.00
- SL_TF dist: {'60': 14, '5': 37, '15': 158} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 100, '60': 61, '5': 39, '240': 9} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=208, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=36, 15m=158, 60m=14, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.73 (n=208), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 39845 | Zonas con Anchors: 39831
- Dir zonas (zona): Bull=9023 Bear=29908 Neutral=914
- Resumen por ciclo (promedios): TotHZ≈ 3.8, WithAnchors≈ 3.8, DirBull≈ 1.5, DirBear≈ 2.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 38680, 'tie-bias': 1151, 'triggers-only': 14}
- TF Triggers: {'15': 4361, '5': 5057}
- TF Anchors: {'60': 9360, '240': 5700, '1440': 768}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 94 | Ejecutadas: 37 | Canceladas: 0 | Expiradas: 0
- BUY: 56 | SELL: 75

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 188
- Registered: 49
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 23 | SKIP_CONCURRENCY: 7
- Intentos de registro: 79

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 42.0%
- RegRate = Registered / Intentos = 62.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 29.1%
- Concurrency = SKIP_CONCURRENCY / Intentos = 8.9%
- ExecRate = Ejecutadas / Registered = 75.5%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 596 | Total candidatos: 9820 | Seleccionados: 0
- Candidatos por zona (promedio): 16.5

### Take Profit (TP)
- Zonas analizadas: 587 | Total candidatos: 4638 | Seleccionados: 587
- Candidatos por zona (promedio): 7.9
- **Edad (barras)** - Candidatos: med=53, max=182 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.40 | Seleccionados: avg=0.70
- **Priority Candidatos**: {'P3': 4638}
- **Priority Seleccionados**: {'P0': 54, 'P3': 275, 'NA': 258}
- **Type Candidatos**: {'Swing': 4638}
- **Type Seleccionados**: {'P0_Zone': 54, 'P3_Swing': 275, 'P4_Fallback': 258}
- **TF Candidatos**: {60: 1475, 15: 1233, 5: 1152, 240: 778}
- **TF Seleccionados**: {15: 141, 60: 95, -1: 258, 5: 69, 240: 24}
- **DistATR** - Candidatos: avg=9.5 | Seleccionados: avg=3.6
- **RR** - Candidatos: avg=4.78 | Seleccionados: avg=1.34
- **Razones de selección**: {'BestIntelligentScore': 587}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.