# Informe Diagnóstico de Logs - 2025-11-02 11:27:33

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251102_111158.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251102_111158.csv`

## DFM
- Eventos de evaluación: 1311
- Evaluaciones Bull: 1512 | Bear: 132
- Pasaron umbral (PassedThreshold): 1209
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:169, 4:234, 5:54, 6:368, 7:415, 8:319, 9:85

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 3453/28113 | KeptCounter: 1775/15071
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.140 | AvgProxCounter≈ 0.059
  - AvgDistATRAligned≈ 2.77 | AvgDistATRCounter≈ 0.75
- PreferAligned eventos: 2069 | Filtradas contra-bias: 291

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 3453/5228 | Counter pre: 1775/5228
- AvgProxAligned(pre)≈ 0.140 | AvgDistATRAligned(pre)≈ 2.77

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=3453 | BaseProx≈ 0.445 | ZoneATR≈ 17.32 | SizePenalty≈ 0.760 | FinalProx≈ 0.341
- Contra-bias: n=1484 | BaseProx≈ 0.460 | ZoneATR≈ 34.90 | SizePenalty≈ 0.567 | FinalProx≈ 0.257

## Risk
- Eventos: 3011
- Accepted=1693 | RejSL=2219 | RejTP=58 | RejRR=967 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1390 | SLDistATR≈ 24.84 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=829 | SLDistATR≈ 26.57 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:655,20-25:370,25+:365
- HistSL Counter 0-10:0,10-15:0,15-20:334,20-25:211,25+:284

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 229 | Unmatched: 1464
- 0-10: Wins=11 Losses=23 WR=32.4% (n=34)
- 10-15: Wins=116 Losses=79 WR=59.5% (n=195)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=127 Losses=102 WR=55.5% (n=229)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1693 | Aligned=1225 (72.4%)
- Core≈ 1.00 | Prox≈ 0.34 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.89 | Confidence≈ 0.00
- SL_TF dist: {'60': 656, '-1': 273, '240': 122, '15': 508, '5': 132, '1440': 2} | SL_Structural≈ 83.9%
- TP_TF dist: {'240': 120, '15': 496, '60': 360, '-1': 619, '1440': 17, '5': 81} | TP_Structural≈ 63.4%

### SLPick por Bandas y TF
- Bandas: lt8=395, 8-10=161, 10-12.5=505, 12.5-15=632, >15=0
- TF: 5m=132, 15m=508, 60m=656, 240m=122, 1440m=2
- RR plan por bandas: 0-10≈ 2.40 (n=556), 10-15≈ 1.64 (n=1137)

## CancelBias (EMA200@60m)
- Eventos: 313
- Distribución Bias: {'Bullish': 275, 'Bearish': 38, 'Neutral': 0}
- Coherencia (Close>EMA): 275/313 (87.9%)

## StructureFusion
- Trazas por zona: 43184 | Zonas con Anchors: 42828
- Dir zonas (zona): Bull=27417 Bear=15767 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.6, WithAnchors≈ 8.6, DirBull≈ 5.5, DirBear≈ 3.2, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 41497, 'tie-bias': 1362, 'triggers-only': 325}
- TF Triggers: {'60': 21068, '15': 17288, '5': 4828}
- TF Anchors: {'240': 42796, '1440': 37089}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,47': 1, 'score decayó a 0,33': 1, 'score decayó a 0,46': 1, 'estructura no existe': 4, 'score decayó a 0,16': 1}
- Cancel_BOS (diag): por acción {'BUY': 12, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 12, 'Neutral': 0}

## CSV de Trades
- Filas: 153 | Ejecutadas: 54 | Canceladas: 0 | Expiradas: 0
- BUY: 193 | SELL: 14

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4892 | Total candidatos: 203821 | Seleccionados: 3883
- Candidatos por zona (promedio): 41.7
- **Edad (barras)** - Candidatos: med=18222, max=23250 | Seleccionados: med=15141, max=22268
- **Score** - Candidatos: avg=0.40 | Seleccionados: avg=0.51
- **TF Candidatos**: {240: 63437, 60: 49036, 1440: 42518, 15: 36513}
- **TF Seleccionados**: {60: 1736, 240: 688, 15: 1448, 1440: 11}
- **DistATR** - Candidatos: avg=154.3 | Seleccionados: avg=11.1
- **Razones de selección**: {'Fallback<15': 932, 'InBand[10,15]': 2951}
- **En banda [10,15] ATR**: 9275/191504 (4.8%)

### Take Profit (TP)
- Zonas analizadas: 4937 | Total candidatos: 85072 | Seleccionados: 4231
- Candidatos por zona (promedio): 17.2
- **Edad (barras)** - Candidatos: med=18875, max=23284 | Seleccionados: med=90, max=22287
- **Score** - Candidatos: avg=0.43 | Seleccionados: avg=0.33
- **Priority Candidatos**: {'P3': 78993}
- **Priority Seleccionados**: {'P3': 3303, 'P4_Fallback': 928}
- **Type Candidatos**: {'Swing': 78993}
- **Type Seleccionados**: {'Swing': 3303, 'Calculated': 928}
- **TF Candidatos**: {240: 22504, 1440: 22319, 60: 18166, 15: 16004}
- **TF Seleccionados**: {240: 676, 15: 1401, 60: 1152, -1: 928, 1440: 74}
- **DistATR** - Candidatos: avg=144.1 | Seleccionados: avg=21.2
- **RR** - Candidatos: avg=8.19 | Seleccionados: avg=1.30
- **Razones de selección**: {'R:R_y_Distancia_OK': 2992, 'NoStructuralTarget': 928, 'Distancia_OK_(R:R_ignorado)': 12, 'R:R_OK_(Distancia_ignorada)': 299}

### 🎯 Recomendaciones
- ⚠️ SL: Estructuras muy antiguas (max 22287 barras). Considerar filtro de edad máxima.
- ⚠️ TP: Estructuras muy antiguas (max 22287 barras). Considerar filtro de edad máxima.
- ⚠️ SL: 57% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- KeptAligned ratio≈ 0.12.