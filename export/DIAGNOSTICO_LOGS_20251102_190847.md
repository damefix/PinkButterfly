# Informe Diagnóstico de Logs - 2025-11-02 19:12:16

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251102_190847.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251102_190847.csv`

## DFM
- Eventos de evaluación: 1685
- Evaluaciones Bull: 1834 | Bear: 390
- Pasaron umbral (PassedThreshold): 1523
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:245, 4:389, 5:118, 6:532, 7:462, 8:363, 9:115

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 3440/27446 | KeptCounter: 1789/15058
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.138 | AvgProxCounter≈ 0.059
  - AvgDistATRAligned≈ 2.72 | AvgDistATRCounter≈ 0.78
- PreferAligned eventos: 2036 | Filtradas contra-bias: 254

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 3440/5229 | Counter pre: 1789/5229
- AvgProxAligned(pre)≈ 0.138 | AvgDistATRAligned(pre)≈ 2.72

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=3440 | BaseProx≈ 0.443 | ZoneATR≈ 17.39 | SizePenalty≈ 0.764 | FinalProx≈ 0.341
- Contra-bias: n=1535 | BaseProx≈ 0.456 | ZoneATR≈ 32.42 | SizePenalty≈ 0.569 | FinalProx≈ 0.256

## Risk
- Eventos: 3026
- Accepted=2288 | RejSL=1797 | RejTP=66 | RejRR=824 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1227 | SLDistATR≈ 26.19 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=570 | SLDistATR≈ 26.23 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:561,20-25:352,25+:314
- HistSL Counter 0-10:0,10-15:0,15-20:261,20-25:102,25+:207

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 203 | Unmatched: 2085
- 0-10: Wins=22 Losses=31 WR=41.5% (n=53)
- 10-15: Wins=76 Losses=74 WR=50.7% (n=150)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=98 Losses=105 WR=48.3% (n=203)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2288 | Aligned=1501 (65.6%)
- Core≈ 1.00 | Prox≈ 0.33 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.91 | Confidence≈ 0.00
- SL_TF dist: {'-1': 696, '15': 1333, '5': 259} | SL_Structural≈ 69.6%
- TP_TF dist: {'-1': 1168, '15': 904, '5': 216} | TP_Structural≈ 49.0%

### SLPick por Bandas y TF
- Bandas: lt8=633, 8-10=324, 10-12.5=647, 12.5-15=684, >15=0
- TF: 5m=259, 15m=1333, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.60 (n=957), 10-15≈ 1.42 (n=1331)

## CancelBias (EMA200@60m)
- Eventos: 427
- Distribución Bias: {'Bullish': 327, 'Bearish': 100, 'Neutral': 0}
- Coherencia (Close>EMA): 327/427 (76.6%)

## StructureFusion
- Trazas por zona: 42504 | Zonas con Anchors: 42154
- Dir zonas (zona): Bull=26672 Bear=15832 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.5, WithAnchors≈ 8.4, DirBull≈ 5.3, DirBear≈ 3.2, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 40972, 'tie-bias': 1222, 'triggers-only': 310}
- TF Triggers: {'60': 20792, '15': 17057, '5': 4655}
- TF Anchors: {'240': 42128, '1440': 36528}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 6, 'score decayó a 0,47': 1, 'score decayó a 0,26': 1, 'score decayó a 0,46': 1, 'score decayó a 0,25': 2, 'score decayó a 0,36': 1, 'score decayó a 0,21': 1}
- Cancel_BOS (diag): por acción {'BUY': 16, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 16, 'Neutral': 0}

## CSV de Trades
- Filas: 186 | Ejecutadas: 60 | Canceladas: 0 | Expiradas: 0
- BUY: 179 | SELL: 67

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4373 | Total candidatos: 45303 | Seleccionados: 2910
- Candidatos por zona (promedio): 10.4
- **Edad (barras)** - Candidatos: med=53, max=150 | Seleccionados: med=51, max=145
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.47
- **TF Candidatos**: {15: 33629}
- **TF Seleccionados**: {15: 2910}
- **DistATR** - Candidatos: avg=22.6 | Seleccionados: avg=10.5
- **Razones de selección**: {'Fallback<15': 1027, 'InBand[10,15]': 1883}
- **En banda [10,15] ATR**: 4995/33629 (14.9%)

### Take Profit (TP)
- Zonas analizadas: 4975 | Total candidatos: 82971 | Seleccionados: 4126
- Candidatos por zona (promedio): 16.7
- **Edad (barras)** - Candidatos: med=18834, max=23183 | Seleccionados: med=9, max=144
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.22
- **Priority Candidatos**: {'P3': 77610}
- **Priority Seleccionados**: {'P4_Fallback': 1926, 'P3': 2200}
- **Type Candidatos**: {'Swing': 77610}
- **Type Seleccionados**: {'Calculated': 1926, 'Swing': 2200}
- **TF Candidatos**: {240: 22351, 1440: 21956, 60: 18026, 15: 15277}
- **TF Seleccionados**: {-1: 1926, 15: 2200}
- **DistATR** - Candidatos: avg=134.2 | Seleccionados: avg=18.2
- **RR** - Candidatos: avg=9.85 | Seleccionados: avg=1.39
- **Razones de selección**: {'NoStructuralTarget': 1926, 'R:R_y_Distancia_OK': 2072, 'Distancia_OK_(R:R_ignorado)': 28, 'R:R_OK_(Distancia_ignorada)': 100}

### 🎯 Recomendaciones
- ⚠️ SL: 65% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 47% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.13.