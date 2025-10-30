# Informe Diagnóstico de Logs - 2025-10-30 10:31:12

- Log: `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\backtest_20251030_100224.log`
- CSV: `C:\Users\meste\Documents\NinjaTrader 8\PinkButterfly\logs\trades_20251030_100224.csv`

## DFM
- Eventos de evaluación: 1153
- Evaluaciones Bull: 2986 | Bear: 390
- Pasaron umbral (PassedThreshold): 3227
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:38, 4:67, 5:110, 6:959, 7:1091, 8:691, 9:420

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 4064
- KeptAligned: 6133/20947 | KeptCounter: 926/6494
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.143 | AvgProxCounter≈ 0.038
  - AvgDistATRAligned≈ 1.70 | AvgDistATRCounter≈ 0.23
- PreferAligned eventos: 1507 | Filtradas contra-bias: 438

### Proximity (Pre-PreferAligned)
- Eventos: 4064
- Aligned pre: 6133/7059 | Counter pre: 926/7059
- AvgProxAligned(pre)≈ 0.143 | AvgDistATRAligned(pre)≈ 1.70

### Proximity Drivers
- Eventos: 4064
- Alineadas: n=6133 | BaseProx≈ 0.557 | ZoneATR≈ 17.28 | SizePenalty≈ 0.740 | FinalProx≈ 0.409
- Contra-bias: n=488 | BaseProx≈ 0.530 | ZoneATR≈ 13.33 | SizePenalty≈ 0.814 | FinalProx≈ 0.428

## Risk
- Eventos: 1710
- Accepted=3529 | RejSL=2101 | RejTP=172 | RejRR=819 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1926 | SLDistATR≈ 24.45 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=175 | SLDistATR≈ 19.73 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:978,20-25:351,25+:597
- HistSL Counter 0-10:0,10-15:0,15-20:127,20-25:33,25+:15

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 249 | Unmatched: 3280
- 0-10: Wins=66 Losses=38 WR=63.5% (n=104)
- 10-15: Wins=50 Losses=92 WR=35.2% (n=142)
- 15-20: Wins=0 Losses=3 WR=0.0% (n=3)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=116 Losses=133 WR=46.6% (n=249)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 3529 | Aligned=3293 (93.3%)
- Core≈ 0.99 | Prox≈ 0.43 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.37 | Confidence≈ 0.00
- SL_TF dist: {'-1': 1848, '60': 839, '240': 712, '5': 130} | SL_Structural≈ 47.6%
- TP_TF dist: {'-1': 1578, '5': 1951} | TP_Structural≈ 55.3%

### SLPick por Bandas y TF
- Bandas: lt8=1910, 8-10=509, 10-12.5=590, 12.5-15=520, >15=0
- TF: 5m=130, 15m=0, 60m=839, 240m=712, 1440m=0
- RR plan por bandas: 0-10≈ 2.92 (n=2419), 10-15≈ 1.19 (n=1110)

## CancelBias (EMA200@60m)
- Eventos: 216
- Distribución Bias: {'Bullish': 199, 'Bearish': 17, 'Neutral': 0}
- Coherencia (Close>EMA): 199/216 (92.1%)

## StructureFusion
- Trazas por zona: 27441 | Zonas con Anchors: 25454
- Dir zonas (zona): Bull=10912 Bear=16529 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 6.8, WithAnchors≈ 6.3, DirBull≈ 2.7, DirBear≈ 4.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 23390, 'triggers-only': 1736, 'tie-bias': 2315}
- TF Triggers: {'5': 9535, '60': 9120, '15': 8786}
- TF Anchors: {'240': 25382, '1440': 937}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 4064 | Distribución: {'Bullish': 2027, 'Bearish': 2037, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 2027/4064

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,47': 4, 'score decayó a 0,41': 1, 'score decayó a 0,26': 1, 'estructura no existe': 24, 'score decayó a 0,21': 1, 'score decayó a 0,38': 1, 'score decayó a 0,28': 1, 'score decayó a 0,22': 1, 'score decayó a 0,42': 1, 'score decayó a 0,44': 1, 'score decayó a 0,35': 1}
- Cancel_BOS (diag): por acción {'BUY': 9, 'SELL': 4} | por bias {'Bullish': 4, 'Bearish': 9, 'Neutral': 0}

## CSV de Trades
- Filas: 170 | Ejecutadas: 30 | Canceladas: 0 | Expiradas: 0
- BUY: 151 | SELL: 49

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 3911 | Total candidatos: 40422 | Seleccionados: 3746
- Candidatos por zona (promedio): 10.3
- **Edad (barras)** - Candidatos: med=36, max=80 | Seleccionados: med=36, max=80
- **Score** - Candidatos: avg=0.35 | Seleccionados: avg=0.32
- **TF Candidatos**: {5: 11624, 15: 11524, 240: 8998, 60: 8276}
- **TF Seleccionados**: {60: 2070, 240: 1401, 5: 275}
- **DistATR** - Candidatos: avg=9.1 | Seleccionados: avg=6.5
- **Razones de selección**: {'Fallback<15': 2720, 'InBand[10,15]': 1026}
- **En banda [10,15] ATR**: 5907/40422 (14.6%)

### Take Profit (TP)
- Zonas analizadas: 6621 | Total candidatos: 43961 | Seleccionados: 6621
- Candidatos por zona (promedio): 6.6
- **Edad (barras)** - Candidatos: med=20, max=80 | Seleccionados: med=5, max=76
- **Score** - Candidatos: avg=0.48 | Seleccionados: avg=0.27
- **Priority Candidatos**: {'P3': 43961}
- **Priority Seleccionados**: {'P4_Fallback': 2886, 'P3': 3735}
- **Type Candidatos**: {'Swing': 43961}
- **Type Seleccionados**: {'Calculated': 2886, 'Swing': 3735}
- **TF Candidatos**: {5: 13548, 15: 13104, 60: 11524, 240: 5785}
- **TF Seleccionados**: {-1: 2886, 5: 3731, 60: 4}
- **DistATR** - Candidatos: avg=18.9 | Seleccionados: avg=15.1
- **RR** - Candidatos: avg=3.37 | Seleccionados: avg=1.63
- **Razones de selección**: {'NoStructuralTarget': 2886, 'R:R_y_Distancia_OK': 3647, 'R:R_OK_(Distancia_ignorada)': 2, 'Distancia_OK_(R:R_ignorado)': 86}

### 🎯 Recomendaciones
- ⚠️ SL: 86% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 44% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.29.