# Informe Diagnóstico de Logs - 2025-11-03 22:02:52

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251103_215822.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251103_215822.csv`

## DFM
- Eventos de evaluación: 1505
- Evaluaciones Bull: 1595 | Bear: 352
- Pasaron umbral (PassedThreshold): 640
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:278, 4:752, 5:491, 6:322, 7:104, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 7113
- KeptAligned: 3091/39931 | KeptCounter: 1454/14233
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.092 | AvgProxCounter≈ 0.035
  - AvgDistATRAligned≈ 1.51 | AvgDistATRCounter≈ 0.39
- PreferAligned eventos: 1907 | Filtradas contra-bias: 172

### Proximity (Pre-PreferAligned)
- Eventos: 7113
- Aligned pre: 3091/4545 | Counter pre: 1454/4545
- AvgProxAligned(pre)≈ 0.092 | AvgDistATRAligned(pre)≈ 1.51

### Proximity Drivers
- Eventos: 7113
- Alineadas: n=3091 | BaseProx≈ 0.445 | ZoneATR≈ 17.52 | SizePenalty≈ 0.765 | FinalProx≈ 0.341
- Contra-bias: n=1282 | BaseProx≈ 0.460 | ZoneATR≈ 32.14 | SizePenalty≈ 0.560 | FinalProx≈ 0.250

## Risk
- Eventos: 2763
- Accepted=2042 | RejSL=1583 | RejTP=50 | RejRR=698 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1108 | SLDistATR≈ 26.34 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=475 | SLDistATR≈ 26.05 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:516,20-25:277,25+:315
- HistSL Counter 0-10:0,10-15:0,15-20:193,20-25:91,25+:191

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 353 | Unmatched: 1689
- 0-10: Wins=40 Losses=56 WR=41.7% (n=96)
- 10-15: Wins=137 Losses=120 WR=53.3% (n=257)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=177 Losses=176 WR=50.1% (n=353)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2042 | Aligned=1371 (67.1%)
- Core≈ 0.99 | Prox≈ 0.34 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.01 | Confidence≈ 0.00
- SL_TF dist: {'-1': 689, '15': 1163, '5': 190} | SL_Structural≈ 66.3%
- TP_TF dist: {'-1': 1063, '15': 814, '5': 165} | TP_Structural≈ 47.9%

### SLPick por Bandas y TF
- Bandas: lt8=661, 8-10=286, 10-12.5=593, 12.5-15=502, >15=0
- TF: 5m=190, 15m=1163, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.67 (n=947), 10-15≈ 1.43 (n=1095)

## CancelBias (EMA200@60m)
- Eventos: 214
- Distribución Bias: {'Bullish': 168, 'Bearish': 46, 'Neutral': 0}
- Coherencia (Close>EMA): 168/214 (78.5%)

## StructureFusion
- Trazas por zona: 54164 | Zonas con Anchors: 53808
- Dir zonas (zona): Bull=25400 Bear=28764 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 7.6, WithAnchors≈ 7.6, DirBull≈ 3.6, DirBear≈ 4.0, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 52686, 'tie-bias': 1160, 'triggers-only': 318}
- TF Triggers: {'60': 26705, '15': 20721, '5': 6738}
- TF Anchors: {'240': 53782, '1440': 48182}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 7113 | Distribución: {'Bullish': 3075, 'Bearish': 4038, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3075/7113

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 2, 'score decayó a 0,26': 1, 'score decayó a 0,44': 1}
- Cancel_BOS (diag): por acción {'BUY': 6, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 6, 'Neutral': 0}

## CSV de Trades
- Filas: 146 | Ejecutadas: 61 | Canceladas: 0 | Expiradas: 0
- BUY: 176 | SELL: 31

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 3785 | Total candidatos: 35699 | Seleccionados: 2514
- Candidatos por zona (promedio): 9.4
- **Edad (barras)** - Candidatos: med=47, max=150 | Seleccionados: med=48, max=146
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.47
- **TF Candidatos**: {15: 26055}
- **TF Seleccionados**: {15: 2514}
- **DistATR** - Candidatos: avg=20.4 | Seleccionados: avg=10.5
- **Razones de selección**: {'Fallback<15': 882, 'InBand[10,15]': 1632}
- **En banda [10,15] ATR**: 4202/26055 (16.1%)

### Take Profit (TP)
- Zonas analizadas: 4373 | Total candidatos: 63935 | Seleccionados: 3694
- Candidatos por zona (promedio): 14.6
- **Edad (barras)** - Candidatos: med=19272, max=23263 | Seleccionados: med=9, max=150
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.22
- **Priority Candidatos**: {'P3': 60163}
- **Priority Seleccionados**: {'P4_Fallback': 1714, 'P3': 1980}
- **Type Candidatos**: {'Swing': 60163}
- **Type Seleccionados**: {'Calculated': 1714, 'Swing': 1980}
- **TF Candidatos**: {1440: 18270, 240: 17012, 60: 13532, 15: 11349}
- **TF Seleccionados**: {-1: 1714, 15: 1980}
- **DistATR** - Candidatos: avg=126.5 | Seleccionados: avg=17.6
- **RR** - Candidatos: avg=9.42 | Seleccionados: avg=1.42
- **Razones de selección**: {'NoStructuralTarget': 1714, 'R:R_y_Distancia_OK': 1853, 'Distancia_OK_(R:R_ignorado)': 38, 'R:R_OK_(Distancia_ignorada)': 89}

### 🎯 Recomendaciones
- ⚠️ SL: 61% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 46% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.08.