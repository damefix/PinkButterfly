# Informe Diagnóstico de Logs - 2025-11-02 18:56:37

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251102_185259.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251102_185259.csv`

## DFM
- Eventos de evaluación: 1723
- Evaluaciones Bull: 1935 | Bear: 393
- Pasaron umbral (PassedThreshold): 1632
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:227, 4:385, 5:144, 6:541, 7:566, 8:352, 9:113

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 3437/25229 | KeptCounter: 1704/13771
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.141 | AvgProxCounter≈ 0.059
  - AvgDistATRAligned≈ 2.72 | AvgDistATRCounter≈ 0.71
- PreferAligned eventos: 2052 | Filtradas contra-bias: 261

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 3437/5141 | Counter pre: 1704/5141
- AvgProxAligned(pre)≈ 0.141 | AvgDistATRAligned(pre)≈ 2.72

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=3437 | BaseProx≈ 0.448 | ZoneATR≈ 17.76 | SizePenalty≈ 0.766 | FinalProx≈ 0.343
- Contra-bias: n=1443 | BaseProx≈ 0.464 | ZoneATR≈ 30.94 | SizePenalty≈ 0.584 | FinalProx≈ 0.267

## Risk
- Eventos: 2963
- Accepted=2398 | RejSL=1590 | RejTP=57 | RejRR=835 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1090 | SLDistATR≈ 26.54 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=500 | SLDistATR≈ 25.67 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:501,20-25:324,25+:265
- HistSL Counter 0-10:0,10-15:0,15-20:223,20-25:106,25+:171

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 202 | Unmatched: 2196
- 0-10: Wins=17 Losses=44 WR=27.9% (n=61)
- 10-15: Wins=59 Losses=82 WR=41.8% (n=141)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=76 Losses=126 WR=37.6% (n=202)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2398 | Aligned=1609 (67.1%)
- Core≈ 0.99 | Prox≈ 0.34 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.99 | Confidence≈ 0.00
- SL_TF dist: {'-1': 923, '15': 1222, '5': 253} | SL_Structural≈ 61.5%
- TP_TF dist: {'15': 876, '-1': 1322, '5': 200} | TP_Structural≈ 44.9%

### SLPick por Bandas y TF
- Bandas: lt8=831, 8-10=377, 10-12.5=651, 12.5-15=539, >15=0
- TF: 5m=253, 15m=1222, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.66 (n=1208), 10-15≈ 1.31 (n=1190)

## CancelBias (EMA200@60m)
- Eventos: 688
- Distribución Bias: {'Bullish': 521, 'Bearish': 167, 'Neutral': 0}
- Coherencia (Close>EMA): 521/688 (75.7%)

## StructureFusion
- Trazas por zona: 39000 | Zonas con Anchors: 38641
- Dir zonas (zona): Bull=23948 Bear=15052 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 7.8, WithAnchors≈ 7.7, DirBull≈ 4.8, DirBear≈ 3.0, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 37627, 'tie-bias': 1063, 'triggers-only': 310}
- TF Triggers: {'60': 19364, '15': 15214, '5': 4422}
- TF Anchors: {'240': 38610, '1440': 33225}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,27': 1, 'score decayó a 0,34': 1, 'score decayó a 0,25': 2, 'score decayó a 0,49': 1, 'score decayó a 0,24': 1, 'score decayó a 0,48': 1, 'estructura no existe': 2}
- Cancel_BOS (diag): por acción {'BUY': 21, 'SELL': 3} | por bias {'Bullish': 3, 'Bearish': 21, 'Neutral': 0}

## CSV de Trades
- Filas: 202 | Ejecutadas: 65 | Canceladas: 0 | Expiradas: 0
- BUY: 186 | SELL: 81

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 3992 | Total candidatos: 32076 | Seleccionados: 2638
- Candidatos por zona (promedio): 8.0
- **Edad (barras)** - Candidatos: med=41, max=149 | Seleccionados: med=43, max=136
- **Score** - Candidatos: avg=0.50 | Seleccionados: avg=0.51
- **TF Candidatos**: {15: 23610}
- **TF Seleccionados**: {15: 2638}
- **DistATR** - Candidatos: avg=18.7 | Seleccionados: avg=10.0
- **Razones de selección**: {'Fallback<15': 1112, 'InBand[10,15]': 1526}
- **En banda [10,15] ATR**: 3858/23610 (16.3%)

### Take Profit (TP)
- Zonas analizadas: 4880 | Total candidatos: 61820 | Seleccionados: 4193
- Candidatos por zona (promedio): 12.7
- **Edad (barras)** - Candidatos: med=18395, max=23151 | Seleccionados: med=3, max=133
- **Score** - Candidatos: avg=0.53 | Seleccionados: avg=0.24
- **Priority Candidatos**: {'P3': 57963}
- **Priority Seleccionados**: {'P3': 2113, 'P4_Fallback': 2080}
- **Type Candidatos**: {'Swing': 57963}
- **Type Seleccionados**: {'Swing': 2113, 'Calculated': 2080}
- **TF Candidatos**: {1440: 16623, 240: 15551, 60: 13995, 15: 11794}
- **TF Seleccionados**: {15: 2113, -1: 2080}
- **DistATR** - Candidatos: avg=107.8 | Seleccionados: avg=17.0
- **RR** - Candidatos: avg=8.72 | Seleccionados: avg=1.44
- **Razones de selección**: {'R:R_y_Distancia_OK': 1993, 'NoStructuralTarget': 2080, 'Distancia_OK_(R:R_ignorado)': 47, 'R:R_OK_(Distancia_ignorada)': 73}

### 🎯 Recomendaciones
- ⚠️ SL: 54% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 50% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.14.