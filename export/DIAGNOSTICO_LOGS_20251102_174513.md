# Informe Diagnóstico de Logs - 2025-11-02 17:49:32

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251102_174513.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251102_174513.csv`

## DFM
- Eventos de evaluación: 3004
- Evaluaciones Bull: 3230 | Bear: 679
- Pasaron umbral (PassedThreshold): 2726
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:421, 4:633, 5:196, 6:950, 7:838, 8:677, 9:205

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 8903
- KeptAligned: 6057/50082 | KeptCounter: 3176/26971
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.138 | AvgProxCounter≈ 0.059
  - AvgDistATRAligned≈ 2.73 | AvgDistATRCounter≈ 0.75
- PreferAligned eventos: 3731 | Filtradas contra-bias: 525

### Proximity (Pre-PreferAligned)
- Eventos: 9185
- Aligned pre: 6288/9548 | Counter pre: 3260/9548
- AvgProxAligned(pre)≈ 0.140 | AvgDistATRAligned(pre)≈ 2.75

### Proximity Drivers
- Eventos: 8825
- Alineadas: n=6126 | BaseProx≈ 0.445 | ZoneATR≈ 17.23 | SizePenalty≈ 0.761 | FinalProx≈ 0.341
- Contra-bias: n=2597 | BaseProx≈ 0.459 | ZoneATR≈ 34.78 | SizePenalty≈ 0.567 | FinalProx≈ 0.257

## Risk
- Eventos: 5338
- Accepted=4021 | RejSL=3163 | RejTP=113 | RejRR=1448 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=2137 | SLDistATR≈ 25.20 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=990 | SLDistATR≈ 28.17 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:1030,20-25:561,25+:546
- HistSL Counter 0-10:0,10-15:0,15-20:412,20-25:188,25+:390

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 484 | Unmatched: 3511
- 0-10: Wins=110 Losses=49 WR=69.2% (n=159)
- 10-15: Wins=211 Losses=112 WR=65.3% (n=323)
- 15-20: Wins=0 Losses=2 WR=0.0% (n=2)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=321 Losses=163 WR=66.3% (n=484)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 4009 | Aligned=2679 (66.8%)
- Core≈ 1.00 | Prox≈ 0.34 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.91 | Confidence≈ 0.00
- SL_TF dist: {'-1': 1248, '15': 2303, '5': 458} | SL_Structural≈ 68.9%
- TP_TF dist: {'-1': 2017, '15': 1616, '5': 376} | TP_Structural≈ 49.7%

### SLPick por Bandas y TF
- Bandas: lt8=1145, 8-10=557, 10-12.5=1110, 12.5-15=1165, >15=0
- TF: 5m=448, 15m=2290, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.55 (n=1708), 10-15≈ 1.45 (n=2310)

## CancelBias (EMA200@60m)
- Eventos: 734
- Distribución Bias: {'Bullish': 609, 'Bearish': 125, 'Neutral': 0}
- Coherencia (Close>EMA): 609/734 (83.0%)

## StructureFusion
- Trazas por zona: 79246 | Zonas con Anchors: 78580
- Dir zonas (zona): Bull=50092 Bear=29154 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.7, WithAnchors≈ 8.6, DirBull≈ 5.5, DirBear≈ 3.2, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 76135, 'tie-bias': 2505, 'triggers-only': 606}
- TF Triggers: {'60': 37858, '15': 31270, '5': 8894}
- TF Anchors: {'240': 77310, '1440': 66857}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 9062 | Distribución: {'Bullish': 5564, 'Bearish': 3498, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 5564/9062

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 10, 'score decayó a 0,47': 2, 'score decayó a 0,33': 2, 'score decayó a 0,26': 2, 'score decayó a 0,46': 2, 'score decayó a 0,42': 2, 'score decayó a 0,16': 2}
- Cancel_BOS (diag): por acción {'BUY': 34, 'SELL': 3} | por bias {'Bullish': 3, 'Bearish': 34, 'Neutral': 0}

## CSV de Trades
- Filas: 431 | Ejecutadas: 140 | Canceladas: 0 | Expiradas: 0
- BUY: 474 | SELL: 96

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 7812 | Total candidatos: 87270 | Seleccionados: 4980
- Candidatos por zona (promedio): 11.2
- **Edad (barras)** - Candidatos: med=58, max=150 | Seleccionados: med=54, max=149
- **Score** - Candidatos: avg=0.41 | Seleccionados: avg=0.46
- **TF Candidatos**: {15: 64214}
- **TF Seleccionados**: {15: 4980}
- **DistATR** - Candidatos: avg=24.1 | Seleccionados: avg=10.5
- **Razones de selección**: {'Fallback<15': 1708, 'InBand[10,15]': 3272}
- **En banda [10,15] ATR**: 8881/64214 (13.8%)

### Take Profit (TP)
- Zonas analizadas: 8925 | Total candidatos: 154841 | Seleccionados: 7128
- Candidatos por zona (promedio): 17.3
- **Edad (barras)** - Candidatos: med=19094, max=23193 | Seleccionados: med=7, max=148
- **Score** - Candidatos: avg=0.43 | Seleccionados: avg=0.21
- **Priority Candidatos**: {'P3': 139707}
- **Priority Seleccionados**: {'P4_Fallback': 3380, 'P3': 3748}
- **Type Candidatos**: {'Swing': 139707}
- **Type Seleccionados**: {'Calculated': 3380, 'Swing': 3748}
- **TF Candidatos**: {1440: 39773, 240: 39771, 60: 31846, 15: 28317}
- **TF Seleccionados**: {-1: 3380, 15: 3748}
- **DistATR** - Candidatos: avg=145.2 | Seleccionados: avg=18.3
- **RR** - Candidatos: avg=10.64 | Seleccionados: avg=1.41
- **Razones de selección**: {'NoStructuralTarget': 3380, 'R:R_y_Distancia_OK': 3502, 'Distancia_OK_(R:R_ignorado)': 53, 'R:R_OK_(Distancia_ignorada)': 193}

### 🎯 Recomendaciones
- ⚠️ SL: 66% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 47% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.12.