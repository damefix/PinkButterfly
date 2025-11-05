# Informe Diagnóstico de Logs - 2025-11-02 17:39:37

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251102_173509.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251102_173509.csv`

## DFM
- Eventos de evaluación: 1567
- Evaluaciones Bull: 1625 | Bear: 357
- Pasaron umbral (PassedThreshold): 1341
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:230, 4:340, 5:113, 6:453, 7:399, 8:345, 9:102

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 2999/27962 | KeptCounter: 1556/14920
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.127 | AvgProxCounter≈ 0.054
  - AvgDistATRAligned≈ 2.30 | AvgDistATRCounter≈ 0.63
- PreferAligned eventos: 1870 | Filtradas contra-bias: 203

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 2999/4555 | Counter pre: 1556/4555
- AvgProxAligned(pre)≈ 0.127 | AvgDistATRAligned(pre)≈ 2.30

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=2999 | BaseProx≈ 0.442 | ZoneATR≈ 16.94 | SizePenalty≈ 0.771 | FinalProx≈ 0.343
- Contra-bias: n=1353 | BaseProx≈ 0.468 | ZoneATR≈ 33.48 | SizePenalty≈ 0.565 | FinalProx≈ 0.260

## Risk
- Eventos: 2772
- Accepted=2029 | RejSL=1541 | RejTP=60 | RejRR=722 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1049 | SLDistATR≈ 25.34 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=492 | SLDistATR≈ 27.15 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:521,20-25:247,25+:281
- HistSL Counter 0-10:0,10-15:0,15-20:213,20-25:85,25+:194

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 285 | Unmatched: 1744
- 0-10: Wins=35 Losses=27 WR=56.5% (n=62)
- 10-15: Wins=98 Losses=124 WR=44.1% (n=222)
- 15-20: Wins=0 Losses=1 WR=0.0% (n=1)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=133 Losses=152 WR=46.7% (n=285)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2029 | Aligned=1320 (65.1%)
- Core≈ 1.00 | Prox≈ 0.34 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.97 | Confidence≈ 0.00
- SL_TF dist: {'-1': 605, '15': 1187, '5': 237} | SL_Structural≈ 70.2%
- TP_TF dist: {'-1': 998, '15': 834, '5': 197} | TP_Structural≈ 50.8%

### SLPick por Bandas y TF
- Bandas: lt8=561, 8-10=294, 10-12.5=566, 12.5-15=608, >15=0
- TF: 5m=237, 15m=1187, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.69 (n=855), 10-15≈ 1.45 (n=1174)

## CancelBias (EMA200@60m)
- Eventos: 374
- Distribución Bias: {'Bullish': 318, 'Bearish': 56, 'Neutral': 0}
- Coherencia (Close>EMA): 318/374 (85.0%)

## StructureFusion
- Trazas por zona: 42882 | Zonas con Anchors: 42526
- Dir zonas (zona): Bull=27391 Bear=15491 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.6, WithAnchors≈ 8.5, DirBull≈ 5.5, DirBear≈ 3.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 41194, 'tie-bias': 1365, 'triggers-only': 323}
- TF Triggers: {'60': 20976, '15': 17139, '5': 4767}
- TF Anchors: {'240': 42494, '1440': 36769}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,47': 1, 'estructura no existe': 4, 'score decayó a 0,32': 1, 'score decayó a 0,49': 1, 'score decayó a 0,37': 1, 'score decayó a 0,42': 1, 'score decayó a 0,23': 1}
- Cancel_BOS (diag): por acción {'BUY': 13, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 13, 'Neutral': 0}

## CSV de Trades
- Filas: 184 | Ejecutadas: 66 | Canceladas: 0 | Expiradas: 0
- BUY: 197 | SELL: 53

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 3869 | Total candidatos: 42973 | Seleccionados: 2510
- Candidatos por zona (promedio): 11.1
- **Edad (barras)** - Candidatos: med=57, max=150 | Seleccionados: med=52, max=145
- **Score** - Candidatos: avg=0.41 | Seleccionados: avg=0.45
- **TF Candidatos**: {15: 31900}
- **TF Seleccionados**: {15: 2510}
- **DistATR** - Candidatos: avg=23.9 | Seleccionados: avg=10.6
- **Razones de selección**: {'Fallback<15': 853, 'InBand[10,15]': 1657}
- **En banda [10,15] ATR**: 4501/31900 (14.1%)

### Take Profit (TP)
- Zonas analizadas: 4352 | Total candidatos: 75634 | Seleccionados: 3522
- Candidatos por zona (promedio): 17.4
- **Edad (barras)** - Candidatos: med=19033, max=23193 | Seleccionados: med=9, max=144
- **Score** - Candidatos: avg=0.43 | Seleccionados: avg=0.22
- **Priority Candidatos**: {'P3': 70386}
- **Priority Seleccionados**: {'P4_Fallback': 1628, 'P3': 1894}
- **Type Candidatos**: {'Swing': 70386}
- **Type Seleccionados**: {'Calculated': 1628, 'Swing': 1894}
- **TF Candidatos**: {1440: 20004, 240: 19975, 60: 16402, 15: 14005}
- **TF Seleccionados**: {-1: 1628, 15: 1894}
- **DistATR** - Candidatos: avg=144.6 | Seleccionados: avg=18.3
- **RR** - Candidatos: avg=10.67 | Seleccionados: avg=1.44
- **Razones de selección**: {'NoStructuralTarget': 1628, 'R:R_y_Distancia_OK': 1766, 'Distancia_OK_(R:R_ignorado)': 34, 'R:R_OK_(Distancia_ignorada)': 94}

### 🎯 Recomendaciones
- ⚠️ SL: 66% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 46% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.11.