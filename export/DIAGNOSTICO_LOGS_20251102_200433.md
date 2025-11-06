# Informe Diagnóstico de Logs - 2025-11-02 20:08:04

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251102_200433.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251102_200433.csv`

## DFM
- Eventos de evaluación: 1679
- Evaluaciones Bull: 1857 | Bear: 381
- Pasaron umbral (PassedThreshold): 1562
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:231, 4:375, 5:119, 6:512, 7:487, 8:385, 9:129

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 3567/26915 | KeptCounter: 1782/14488
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.144 | AvgProxCounter≈ 0.059
  - AvgDistATRAligned≈ 2.81 | AvgDistATRCounter≈ 0.75
- PreferAligned eventos: 2110 | Filtradas contra-bias: 271

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 3567/5349 | Counter pre: 1782/5349
- AvgProxAligned(pre)≈ 0.144 | AvgDistATRAligned(pre)≈ 2.81

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=3567 | BaseProx≈ 0.447 | ZoneATR≈ 17.21 | SizePenalty≈ 0.768 | FinalProx≈ 0.344
- Contra-bias: n=1511 | BaseProx≈ 0.455 | ZoneATR≈ 32.16 | SizePenalty≈ 0.562 | FinalProx≈ 0.251

## Risk
- Eventos: 3064
- Accepted=2298 | RejSL=1897 | RejTP=60 | RejRR=823 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1296 | SLDistATR≈ 25.48 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=601 | SLDistATR≈ 26.07 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:632,20-25:358,25+:306
- HistSL Counter 0-10:0,10-15:0,15-20:278,20-25:88,25+:235

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 195 | Unmatched: 2103
- 0-10: Wins=13 Losses=39 WR=25.0% (n=52)
- 10-15: Wins=86 Losses=57 WR=60.1% (n=143)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=99 Losses=96 WR=50.8% (n=195)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2298 | Aligned=1543 (67.1%)
- Core≈ 1.00 | Prox≈ 0.34 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.96 | Confidence≈ 0.00
- SL_TF dist: {'-1': 784, '15': 1289, '5': 225} | SL_Structural≈ 65.9%
- TP_TF dist: {'15': 875, '-1': 1229, '5': 194} | TP_Structural≈ 46.5%

### SLPick por Bandas y TF
- Bandas: lt8=728, 8-10=327, 10-12.5=646, 12.5-15=597, >15=0
- TF: 5m=225, 15m=1289, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.56 (n=1055), 10-15≈ 1.45 (n=1243)

## CancelBias (EMA200@60m)
- Eventos: 442
- Distribución Bias: {'Bullish': 361, 'Bearish': 81, 'Neutral': 0}
- Coherencia (Close>EMA): 361/442 (81.7%)

## StructureFusion
- Trazas por zona: 41403 | Zonas con Anchors: 41047
- Dir zonas (zona): Bull=25916 Bear=15487 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.3, WithAnchors≈ 8.2, DirBull≈ 5.2, DirBear≈ 3.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 39808, 'tie-bias': 1275, 'triggers-only': 320}
- TF Triggers: {'60': 20232, '15': 16637, '5': 4534}
- TF Anchors: {'240': 41023, '1440': 35389}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 5, 'score decayó a 0,47': 1, 'score decayó a 0,25': 3, 'score decayó a 0,24': 2}
- Cancel_BOS (diag): por acción {'BUY': 14, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 14, 'Neutral': 0}

## CSV de Trades
- Filas: 166 | Ejecutadas: 55 | Canceladas: 0 | Expiradas: 0
- BUY: 159 | SELL: 62

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4396 | Total candidatos: 41652 | Seleccionados: 2952
- Candidatos por zona (promedio): 9.5
- **Edad (barras)** - Candidatos: med=49, max=170 | Seleccionados: med=49, max=161
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.47
- **TF Candidatos**: {15: 31107}
- **TF Seleccionados**: {15: 2952}
- **DistATR** - Candidatos: avg=20.9 | Seleccionados: avg=10.5
- **Razones de selección**: {'Fallback<15': 1042, 'InBand[10,15]': 1910}
- **En banda [10,15] ATR**: 4962/31107 (16.0%)

### Take Profit (TP)
- Zonas analizadas: 5078 | Total candidatos: 75291 | Seleccionados: 4250
- Candidatos por zona (promedio): 14.8
- **Edad (barras)** - Candidatos: med=18608, max=23176 | Seleccionados: med=5, max=168
- **Score** - Candidatos: avg=0.48 | Seleccionados: avg=0.22
- **Priority Candidatos**: {'P3': 70474}
- **Priority Seleccionados**: {'P3': 2183, 'P4_Fallback': 2067}
- **Type Candidatos**: {'Swing': 70474}
- **Type Seleccionados**: {'Swing': 2183, 'Calculated': 2067}
- **TF Candidatos**: {240: 20237, 1440: 19720, 60: 16535, 15: 13982}
- **TF Seleccionados**: {15: 2183, -1: 2067}
- **DistATR** - Candidatos: avg=117.3 | Seleccionados: avg=17.7
- **RR** - Candidatos: avg=8.93 | Seleccionados: avg=1.39
- **Razones de selección**: {'R:R_y_Distancia_OK': 2047, 'NoStructuralTarget': 2067, 'Distancia_OK_(R:R_ignorado)': 33, 'R:R_OK_(Distancia_ignorada)': 103}

### 🎯 Recomendaciones
- ⚠️ SL: 62% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 49% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.13.