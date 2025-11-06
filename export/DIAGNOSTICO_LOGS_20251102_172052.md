# Informe Diagnóstico de Logs - 2025-11-02 17:26:27

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251102_172052.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251102_172052.csv`

## DFM
- Eventos de evaluación: 1829
- Evaluaciones Bull: 2042 | Bear: 406
- Pasaron umbral (PassedThreshold): 1744
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:257, 4:386, 5:111, 6:606, 7:547, 8:404, 9:137

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 3965/28335 | KeptCounter: 1925/15225
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.153 | AvgProxCounter≈ 0.061
  - AvgDistATRAligned≈ 3.23 | AvgDistATRCounter≈ 0.88
- PreferAligned eventos: 2240 | Filtradas contra-bias: 343

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 3965/5890 | Counter pre: 1925/5890
- AvgProxAligned(pre)≈ 0.153 | AvgDistATRAligned(pre)≈ 3.23

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=3965 | BaseProx≈ 0.447 | ZoneATR≈ 17.61 | SizePenalty≈ 0.757 | FinalProx≈ 0.342
- Contra-bias: n=1582 | BaseProx≈ 0.452 | ZoneATR≈ 35.42 | SizePenalty≈ 0.569 | FinalProx≈ 0.253

## Risk
- Eventos: 3230
- Accepted=2503 | RejSL=2043 | RejTP=71 | RejRR=930 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1442 | SLDistATR≈ 25.22 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=601 | SLDistATR≈ 28.65 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:654,20-25:419,25+:369
- HistSL Counter 0-10:0,10-15:0,15-20:237,20-25:116,25+:248

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 296 | Unmatched: 2207
- 0-10: Wins=39 Losses=74 WR=34.5% (n=113)
- 10-15: Wins=116 Losses=66 WR=63.7% (n=182)
- 15-20: Wins=0 Losses=1 WR=0.0% (n=1)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=155 Losses=141 WR=52.4% (n=296)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2503 | Aligned=1720 (68.7%)
- Core≈ 1.00 | Prox≈ 0.33 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.89 | Confidence≈ 0.00
- SL_TF dist: {'-1': 808, '15': 1438, '5': 257} | SL_Structural≈ 67.7%
- TP_TF dist: {'-1': 1289, '15': 995, '5': 219} | TP_Structural≈ 48.5%

### SLPick por Bandas y TF
- Bandas: lt8=717, 8-10=359, 10-12.5=714, 12.5-15=713, >15=0
- TF: 5m=257, 15m=1438, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.53 (n=1076), 10-15≈ 1.40 (n=1427)

## CancelBias (EMA200@60m)
- Eventos: 596
- Distribución Bias: {'Bullish': 518, 'Bearish': 78, 'Neutral': 0}
- Coherencia (Close>EMA): 518/596 (86.9%)

## StructureFusion
- Trazas por zona: 43560 | Zonas con Anchors: 43200
- Dir zonas (zona): Bull=27517 Bear=16043 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.7, WithAnchors≈ 8.6, DirBull≈ 5.5, DirBear≈ 3.2, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 41883, 'tie-bias': 1352, 'triggers-only': 325}
- TF Triggers: {'60': 21356, '15': 17347, '5': 4857}
- TF Anchors: {'240': 43169, '1440': 37412}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 5, 'score decayó a 0,29': 3, 'score decayó a 0,45': 1, 'score decayó a 0,35': 1, 'score decayó a 0,33': 1, 'score decayó a 0,48': 1, 'score decayó a 0,47': 1, 'score decayó a 0,42': 1, 'score decayó a 0,30': 1}
- Cancel_BOS (diag): por acción {'BUY': 26, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 26, 'Neutral': 0}

## CSV de Trades
- Filas: 223 | Ejecutadas: 66 | Canceladas: 0 | Expiradas: 0
- BUY: 246 | SELL: 43

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4968 | Total candidatos: 55278 | Seleccionados: 3143
- Candidatos por zona (promedio): 11.1
- **Edad (barras)** - Candidatos: med=58, max=150 | Seleccionados: med=54, max=149
- **Score** - Candidatos: avg=0.41 | Seleccionados: avg=0.46
- **TF Candidatos**: {15: 41107}
- **TF Seleccionados**: {15: 3143}
- **DistATR** - Candidatos: avg=24.6 | Seleccionados: avg=10.4
- **Razones de selección**: {'Fallback<15': 1097, 'InBand[10,15]': 2046}
- **En banda [10,15] ATR**: 5430/41107 (13.2%)

### Take Profit (TP)
- Zonas analizadas: 5547 | Total candidatos: 96733 | Seleccionados: 4455
- Candidatos por zona (promedio): 17.4
- **Edad (barras)** - Candidatos: med=18823, max=23193 | Seleccionados: med=6, max=148
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.22
- **Priority Candidatos**: {'P3': 89387}
- **Priority Seleccionados**: {'P4_Fallback': 2116, 'P3': 2339}
- **Type Candidatos**: {'Swing': 89387}
- **Type Seleccionados**: {'Calculated': 2116, 'Swing': 2339}
- **TF Candidatos**: {240: 25432, 1440: 25422, 60: 20354, 15: 18179}
- **TF Seleccionados**: {-1: 2116, 15: 2339}
- **DistATR** - Candidatos: avg=144.0 | Seleccionados: avg=18.2
- **RR** - Candidatos: avg=10.37 | Seleccionados: avg=1.39
- **Razones de selección**: {'NoStructuralTarget': 2116, 'R:R_y_Distancia_OK': 2205, 'Distancia_OK_(R:R_ignorado)': 36, 'R:R_OK_(Distancia_ignorada)': 98}

### 🎯 Recomendaciones
- ⚠️ SL: 65% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 47% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.14.