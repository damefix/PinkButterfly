# Informe Diagnóstico de Logs - 2025-11-04 07:05:34

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_065629.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_065629.csv`

## DFM
- Eventos de evaluación: 1848
- Evaluaciones Bull: 1738 | Bear: 725
- Pasaron umbral (PassedThreshold): 698
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:45, 4:1438, 5:556, 6:311, 7:102, 8:11, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 6981
- KeptAligned: 5843/54841 | KeptCounter: 1376/11072
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.142 | AvgProxCounter≈ 0.034
  - AvgDistATRAligned≈ 2.73 | AvgDistATRCounter≈ 0.32
- PreferAligned eventos: 3295 | Filtradas contra-bias: 423

### Proximity (Pre-PreferAligned)
- Eventos: 6981
- Aligned pre: 5843/7219 | Counter pre: 1376/7219
- AvgProxAligned(pre)≈ 0.142 | AvgDistATRAligned(pre)≈ 2.73

### Proximity Drivers
- Eventos: 6981
- Alineadas: n=5843 | BaseProx≈ 0.453 | ZoneATR≈ 20.60 | SizePenalty≈ 0.709 | FinalProx≈ 0.312
- Contra-bias: n=953 | BaseProx≈ 0.438 | ZoneATR≈ 31.79 | SizePenalty≈ 0.652 | FinalProx≈ 0.278

## Risk
- Eventos: 3862
- Accepted=2505 | RejSL=3054 | RejTP=49 | RejRR=1188 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=2382 | SLDistATR≈ 29.64 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=672 | SLDistATR≈ 30.18 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:824,20-25:475,25+:1083
- HistSL Counter 0-10:0,10-15:0,15-20:175,20-25:244,25+:253

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 199 | Unmatched: 2306
- 0-10: Wins=31 Losses=31 WR=50.0% (n=62)
- 10-15: Wins=76 Losses=61 WR=55.5% (n=137)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=107 Losses=92 WR=53.8% (n=199)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2505 | Aligned=2339 (93.4%)
- Core≈ 1.00 | Prox≈ 0.28 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 3.72 | Confidence≈ 0.00
- SL_TF dist: {'60': 889, '15': 610, '-1': 864, '240': 58, '5': 84} | SL_Structural≈ 65.5%
- TP_TF dist: {'15': 626, '240': 164, '60': 412, '1440': 692, '-1': 537, '5': 74} | TP_Structural≈ 78.6%

### SLPick por Bandas y TF
- Bandas: lt8=558, 8-10=639, 10-12.5=551, 12.5-15=757, >15=0
- TF: 5m=84, 15m=610, 60m=889, 240m=58, 1440m=0
- RR plan por bandas: 0-10≈ 5.09 (n=1197), 10-15≈ 2.48 (n=1308)

## CancelBias (EMA200@60m)
- Eventos: 303
- Distribución Bias: {'Bullish': 303, 'Bearish': 0, 'Neutral': 0}
- Coherencia (Close>EMA): 303/303 (100.0%)

## StructureFusion
- Trazas por zona: 65913 | Zonas con Anchors: 65565
- Dir zonas (zona): Bull=25158 Bear=40755 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 9.4, WithAnchors≈ 9.4, DirBull≈ 3.6, DirBear≈ 5.8, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 64473, 'tie-bias': 1127, 'triggers-only': 313}
- TF Triggers: {'15': 25327, '60': 27983, '5': 12603}
- TF Anchors: {'240': 65543, '1440': 60071}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 6981 | Distribución: {'Bullish': 3736, 'Bearish': 3245, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3736/6981

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 9, 'score decayó a 0,42': 1, 'score decayó a 0,43': 1, 'score decayó a 0,47': 1}

## CSV de Trades
- Filas: 140 | Ejecutadas: 35 | Canceladas: 0 | Expiradas: 0
- BUY: 173 | SELL: 2

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 6788 | Total candidatos: 190919 | Seleccionados: 5724
- Candidatos por zona (promedio): 28.1
- **Edad (barras)** - Candidatos: med=36, max=150 | Seleccionados: med=34, max=146
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.52
- **TF Candidatos**: {240: 62912, 60: 52762, 15: 37119, 1440: 26941, 5: 11185}
- **TF Seleccionados**: {60: 2808, 15: 1873, 240: 534, 5: 503, 1440: 6}
- **DistATR** - Candidatos: avg=77.8 | Seleccionados: avg=10.6
- **Razones de selección**: {'InBand[10,15]': 3806, 'Fallback<15': 1918}
- **En banda [10,15] ATR**: 11579/190919 (6.1%)

### Take Profit (TP)
- Zonas analizadas: 6796 | Total candidatos: 83774 | Seleccionados: 6796
- Candidatos por zona (promedio): 12.3
- **Edad (barras)** - Candidatos: med=36, max=150 | Seleccionados: med=33, max=148
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.40
- **Priority Candidatos**: {'P3': 83774}
- **Priority Seleccionados**: {'P3': 5659, 'P4_Fallback': 1137}
- **Type Candidatos**: {'Swing': 83774}
- **Type Seleccionados**: {'Swing': 5659, 'Calculated': 1137}
- **TF Candidatos**: {1440: 25736, 240: 19607, 60: 16727, 15: 16154, 5: 5550}
- **TF Seleccionados**: {60: 1533, 240: 805, 15: 1923, 1440: 817, -1: 1137, 5: 581}
- **DistATR** - Candidatos: avg=112.8 | Seleccionados: avg=25.3
- **RR** - Candidatos: avg=7.17 | Seleccionados: avg=1.85
- **Razones de selección**: {'R:R_y_Distancia_OK': 4720, 'R:R_OK_(Distancia_ignorada)': 932, 'Distancia_OK_(R:R_ignorado)': 7, 'NoStructuralTarget': 1137}

### 🎯 Recomendaciones
- ⚠️ SL: 51% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- KeptAligned ratio≈ 0.11.