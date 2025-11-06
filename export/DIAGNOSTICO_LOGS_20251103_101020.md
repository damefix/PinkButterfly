# Informe Diagnóstico de Logs - 2025-11-03 10:16:13

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251103_101020.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251103_101020.csv`

## DFM
- Eventos de evaluación: 1575
- Evaluaciones Bull: 1662 | Bear: 334
- Pasaron umbral (PassedThreshold): 654
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:213, 4:808, 5:562, 6:330, 7:83, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 2850/23878 | KeptCounter: 1404/13357
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.119 | AvgProxCounter≈ 0.047
  - AvgDistATRAligned≈ 2.19 | AvgDistATRCounter≈ 0.55
- PreferAligned eventos: 1846 | Filtradas contra-bias: 205

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 2850/4254 | Counter pre: 1404/4254
- AvgProxAligned(pre)≈ 0.119 | AvgDistATRAligned(pre)≈ 2.19

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=2850 | BaseProx≈ 0.419 | ZoneATR≈ 16.97 | SizePenalty≈ 0.775 | FinalProx≈ 0.325
- Contra-bias: n=1199 | BaseProx≈ 0.444 | ZoneATR≈ 31.87 | SizePenalty≈ 0.575 | FinalProx≈ 0.247

## Risk
- Eventos: 2632
- Accepted=2106 | RejSL=1246 | RejTP=45 | RejRR=652 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=857 | SLDistATR≈ 25.35 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=389 | SLDistATR≈ 25.99 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:419,20-25:212,25+:226
- HistSL Counter 0-10:0,10-15:0,15-20:167,20-25:73,25+:149

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 183 | Unmatched: 1923
- 0-10: Wins=19 Losses=61 WR=23.8% (n=80)
- 10-15: Wins=65 Losses=38 WR=63.1% (n=103)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=84 Losses=99 WR=45.9% (n=183)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2106 | Aligned=1430 (67.9%)
- Core≈ 0.99 | Prox≈ 0.32 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.89 | Confidence≈ 0.00
- SL_TF dist: {'-1': 846, '15': 1079, '5': 181} | SL_Structural≈ 59.8%
- TP_TF dist: {'15': 763, '-1': 1189, '5': 154} | TP_Structural≈ 43.5%

### SLPick por Bandas y TF
- Bandas: lt8=781, 8-10=327, 10-12.5=568, 12.5-15=430, >15=0
- TF: 5m=181, 15m=1079, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.35 (n=1108), 10-15≈ 1.37 (n=998)

## CancelBias (EMA200@60m)
- Eventos: 170
- Distribución Bias: {'Bullish': 129, 'Bearish': 41, 'Neutral': 0}
- Coherencia (Close>EMA): 129/170 (75.9%)

## StructureFusion
- Trazas por zona: 37235 | Zonas con Anchors: 36858
- Dir zonas (zona): Bull=23622 Bear=13613 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 7.4, WithAnchors≈ 7.4, DirBull≈ 4.7, DirBear≈ 2.7, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 35911, 'tie-bias': 997, 'triggers-only': 327}
- TF Triggers: {'60': 18040, '15': 14889, '5': 4306}
- TF Anchors: {'240': 36818, '1440': 31525}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,45': 1, 'estructura no existe': 2}
- Cancel_BOS (diag): por acción {'BUY': 9, 'SELL': 0} | por bias {'Bullish': 0, 'Bearish': 9, 'Neutral': 0}

## CSV de Trades
- Filas: 122 | Ejecutadas: 49 | Canceladas: 0 | Expiradas: 0
- BUY: 140 | SELL: 31

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 3249 | Total candidatos: 23057 | Seleccionados: 2136
- Candidatos por zona (promedio): 7.1
- **Edad (barras)** - Candidatos: med=36, max=149 | Seleccionados: med=35, max=149
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.54
- **TF Candidatos**: {15: 17180}
- **TF Seleccionados**: {15: 2136}
- **DistATR** - Candidatos: avg=19.6 | Seleccionados: avg=10.1
- **Razones de selección**: {'Fallback<15': 868, 'InBand[10,15]': 1268}
- **En banda [10,15] ATR**: 2843/17180 (16.5%)

### Take Profit (TP)
- Zonas analizadas: 4049 | Total candidatos: 39106 | Seleccionados: 3537
- Candidatos por zona (promedio): 9.7
- **Edad (barras)** - Candidatos: med=17632, max=23151 | Seleccionados: med=0, max=150
- **Score** - Candidatos: avg=0.57 | Seleccionados: avg=0.23
- **Priority Candidatos**: {'P3': 36571}
- **Priority Seleccionados**: {'P3': 1730, 'P4_Fallback': 1807}
- **Type Candidatos**: {'Swing': 36571}
- **Type Seleccionados**: {'Swing': 1730, 'Calculated': 1807}
- **TF Candidatos**: {1440: 10450, 60: 9371, 240: 8484, 15: 8266}
- **TF Seleccionados**: {15: 1730, -1: 1807}
- **DistATR** - Candidatos: avg=89.7 | Seleccionados: avg=16.6
- **RR** - Candidatos: avg=7.72 | Seleccionados: avg=1.42
- **Razones de selección**: {'R:R_y_Distancia_OK': 1628, 'NoStructuralTarget': 1807, 'Distancia_OK_(R:R_ignorado)': 24, 'R:R_OK_(Distancia_ignorada)': 78}

### 🎯 Recomendaciones
- ⚠️ SL: 45% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 51% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.12.