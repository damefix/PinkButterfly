# Informe Diagnóstico de Logs - 2025-10-30 16:46:37

- Log: `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251030_162535.log`
- CSV: `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251030_162535.csv`

## DFM
- Eventos de evaluación: 295
- Evaluaciones Bull: 16 | Bear: 5
- Pasaron umbral (PassedThreshold): 12
- ConfidenceBins acumulado: 0:0, 1:0, 2:1, 3:2, 4:5, 5:2, 6:3, 7:3, 8:5, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 415
- KeptAligned: 238/250 | KeptCounter: 122/165
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.415 | AvgProxCounter≈ 0.185
  - AvgDistATRAligned≈ 1.51 | AvgDistATRCounter≈ 0.53
- PreferAligned eventos: 238 | Filtradas contra-bias: 0

### Proximity (Pre-PreferAligned)
- Eventos: 415
- Aligned pre: 238/360 | Counter pre: 122/360
- AvgProxAligned(pre)≈ 0.415 | AvgDistATRAligned(pre)≈ 1.51

### Proximity Drivers
- Eventos: 0

## Risk
- Eventos: 360
- Accepted=295 | RejSL=25 | RejTP=16 | RejRR=24 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=20 | SLDistATR≈ 14.45 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=5 | SLDistATR≈ 12.25 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:15,15-20:2,20-25:3,25+:0
- HistSL Counter 0-10:0,10-15:4,15-20:1,20-25:0,25+:0

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 20 | Unmatched: 275
- 0-10: Wins=14 Losses=6 WR=70.0% (n=20)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=14 Losses=6 WR=70.0% (n=20)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 295 | Aligned=200 (67.8%)
- Core≈ 0.50 | Prox≈ 0.72 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.59 | Confidence≈ 0.00
- SL_TF dist: {'-1': 295} | SL_Structural≈ 0.0%
- TP_TF dist: {'-1': 279, '60': 16} | TP_Structural≈ 5.4%

### SLPick por Bandas y TF
- Bandas: lt8=274, 8-10=21, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=0, 15m=0, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.59 (n=295), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 415 | Zonas con Anchors: 27
- Dir zonas (zona): Bull=251 Bear=164 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 0.5, WithAnchors≈ 0.0, DirBull≈ 0.3, DirBear≈ 0.2, DirNeutral≈ 0.0
- Razones de dirección: {'triggers-only': 337, 'tie-bias': 51, 'anchors+triggers': 27}
- TF Triggers: {'60': 415}
- TF Anchors: {'240': 25, '1440': 18}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 4896 | Distribución: {'Bullish': 3099, 'Bearish': 1797, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3099/4896

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura inactiva': 2}

## CSV de Trades
- Filas: 16 | Ejecutadas: 6 | Canceladas: 0 | Expiradas: 0
- BUY: 22 | SELL: 0

## Análisis Post-Mortem: SL/TP
### Take Profit (TP)
- Zonas analizadas: 360 | Total candidatos: 61 | Seleccionados: 360
- Candidatos por zona (promedio): 0.2
- **Edad (barras)** - Candidatos: med=2, max=5 | Seleccionados: med=0, max=24
- **Score** - Candidatos: avg=0.22 | Seleccionados: avg=0.04
- **Priority Candidatos**: {'P3': 61}
- **Priority Seleccionados**: {'P4_Fallback': 300, 'P3': 60}
- **Type Candidatos**: {'Swing': 61}
- **Type Seleccionados**: {'Calculated': 300, 'Swing': 60}
- **TF Candidatos**: {60: 60, 240: 1}
- **TF Seleccionados**: {-1: 300, 60: 59, 240: 1}
- **DistATR** - Candidatos: avg=5.5 | Seleccionados: avg=7.1
- **RR** - Candidatos: avg=1.31 | Seleccionados: avg=1.47
- **Razones de selección**: {'NoStructuralTarget': 300, 'R:R_y_Distancia_OK': 60}

### 🎯 Recomendaciones
- ⚠️ TP: 83% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.95.