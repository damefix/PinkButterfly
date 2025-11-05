# Informe Diagnóstico de Logs - 2025-11-02 19:53:51

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251102_195044.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251102_195044.csv`

## DFM
- Eventos de evaluación: 1687
- Evaluaciones Bull: 1846 | Bear: 381
- Pasaron umbral (PassedThreshold): 1544
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:237, 4:378, 5:118, 6:511, 7:481, 8:376, 9:126

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 3524/26891 | KeptCounter: 1776/14400
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.144 | AvgProxCounter≈ 0.059
  - AvgDistATRAligned≈ 2.80 | AvgDistATRCounter≈ 0.76
- PreferAligned eventos: 2108 | Filtradas contra-bias: 268

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 3524/5300 | Counter pre: 1776/5300
- AvgProxAligned(pre)≈ 0.144 | AvgDistATRAligned(pre)≈ 2.80

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=3524 | BaseProx≈ 0.448 | ZoneATR≈ 17.15 | SizePenalty≈ 0.768 | FinalProx≈ 0.344
- Contra-bias: n=1508 | BaseProx≈ 0.454 | ZoneATR≈ 31.91 | SizePenalty≈ 0.567 | FinalProx≈ 0.253

## Risk
- Eventos: 3058
- Accepted=2290 | RejSL=1849 | RejTP=55 | RejRR=838 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1269 | SLDistATR≈ 25.50 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=580 | SLDistATR≈ 26.37 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:628,20-25:343,25+:298
- HistSL Counter 0-10:0,10-15:0,15-20:264,20-25:93,25+:223

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 201 | Unmatched: 2089
- 0-10: Wins=16 Losses=45 WR=26.2% (n=61)
- 10-15: Wins=74 Losses=66 WR=52.9% (n=140)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=90 Losses=111 WR=44.8% (n=201)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2290 | Aligned=1528 (66.7%)
- Core≈ 0.99 | Prox≈ 0.34 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.97 | Confidence≈ 0.00
- SL_TF dist: {'-1': 784, '15': 1287, '5': 219} | SL_Structural≈ 65.8%
- TP_TF dist: {'15': 865, '-1': 1226, '5': 199} | TP_Structural≈ 46.5%

### SLPick por Bandas y TF
- Bandas: lt8=738, 8-10=332, 10-12.5=612, 12.5-15=608, >15=0
- TF: 5m=219, 15m=1287, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.61 (n=1070), 10-15≈ 1.40 (n=1220)

## CancelBias (EMA200@60m)
- Eventos: 445
- Distribución Bias: {'Bullish': 361, 'Bearish': 84, 'Neutral': 0}
- Coherencia (Close>EMA): 361/445 (81.1%)

## StructureFusion
- Trazas por zona: 41291 | Zonas con Anchors: 40934
- Dir zonas (zona): Bull=25977 Bear=15314 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.3, WithAnchors≈ 8.2, DirBull≈ 5.2, DirBear≈ 3.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 39744, 'tie-bias': 1226, 'triggers-only': 321}
- TF Triggers: {'60': 20124, '15': 16613, '5': 4554}
- TF Anchors: {'240': 40909, '1440': 35266}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 5, 'score decayó a 0,47': 1, 'score decayó a 0,25': 3, 'score decayó a 0,24': 1}
- Cancel_BOS (diag): por acción {'BUY': 14, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 14, 'Neutral': 0}

## CSV de Trades
- Filas: 165 | Ejecutadas: 55 | Canceladas: 0 | Expiradas: 0
- BUY: 163 | SELL: 57

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4320 | Total candidatos: 39276 | Seleccionados: 2934
- Candidatos por zona (promedio): 9.1
- **Edad (barras)** - Candidatos: med=47, max=120 | Seleccionados: med=47, max=120
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.49
- **TF Candidatos**: {15: 29423}
- **TF Seleccionados**: {15: 2934}
- **DistATR** - Candidatos: avg=20.8 | Seleccionados: avg=10.4
- **Razones de selección**: {'Fallback<15': 1088, 'InBand[10,15]': 1846}
- **En banda [10,15] ATR**: 4697/29423 (16.0%)

### Take Profit (TP)
- Zonas analizadas: 5032 | Total candidatos: 74160 | Seleccionados: 4226
- Candidatos por zona (promedio): 14.7
- **Edad (barras)** - Candidatos: med=18564, max=23176 | Seleccionados: med=5, max=120
- **Score** - Candidatos: avg=0.48 | Seleccionados: avg=0.23
- **Priority Candidatos**: {'P3': 69443}
- **Priority Seleccionados**: {'P3': 2180, 'P4_Fallback': 2046}
- **Type Candidatos**: {'Swing': 69443}
- **Type Seleccionados**: {'Swing': 2180, 'Calculated': 2046}
- **TF Candidatos**: {240: 19817, 1440: 19455, 60: 16369, 15: 13802}
- **TF Seleccionados**: {15: 2180, -1: 2046}
- **DistATR** - Candidatos: avg=117.0 | Seleccionados: avg=17.3
- **RR** - Candidatos: avg=9.00 | Seleccionados: avg=1.39
- **Razones de selección**: {'R:R_y_Distancia_OK': 2044, 'NoStructuralTarget': 2046, 'Distancia_OK_(R:R_ignorado)': 40, 'R:R_OK_(Distancia_ignorada)': 96}

### 🎯 Recomendaciones
- ⚠️ SL: 59% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 48% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.13.