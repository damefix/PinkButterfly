# Informe Diagnóstico de Logs - 2025-11-02 19:47:00

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251102_194339.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251102_194339.csv`

## DFM
- Eventos de evaluación: 1775
- Evaluaciones Bull: 1960 | Bear: 420
- Pasaron umbral (PassedThreshold): 1615
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:251, 4:426, 5:148, 6:527, 7:513, 8:390, 9:125

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 3480/26332 | KeptCounter: 1889/14627
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.141 | AvgProxCounter≈ 0.065
  - AvgDistATRAligned≈ 2.72 | AvgDistATRCounter≈ 0.77
- PreferAligned eventos: 2041 | Filtradas contra-bias: 280

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 3480/5369 | Counter pre: 1889/5369
- AvgProxAligned(pre)≈ 0.141 | AvgDistATRAligned(pre)≈ 2.72

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=3480 | BaseProx≈ 0.450 | ZoneATR≈ 17.49 | SizePenalty≈ 0.770 | FinalProx≈ 0.348
- Contra-bias: n=1609 | BaseProx≈ 0.466 | ZoneATR≈ 31.67 | SizePenalty≈ 0.574 | FinalProx≈ 0.266

## Risk
- Eventos: 3055
- Accepted=2441 | RejSL=1781 | RejTP=63 | RejRR=804 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1209 | SLDistATR≈ 25.08 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=572 | SLDistATR≈ 26.80 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:624,20-25:313,25+:272
- HistSL Counter 0-10:0,10-15:0,15-20:233,20-25:105,25+:234

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 220 | Unmatched: 2221
- 0-10: Wins=18 Losses=42 WR=30.0% (n=60)
- 10-15: Wins=89 Losses=71 WR=55.6% (n=160)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=107 Losses=113 WR=48.6% (n=220)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2441 | Aligned=1586 (65.0%)
- Core≈ 0.99 | Prox≈ 0.34 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.94 | Confidence≈ 0.00
- SL_TF dist: {'-1': 852, '15': 1346, '5': 243} | SL_Structural≈ 65.1%
- TP_TF dist: {'15': 928, '-1': 1300, '5': 213} | TP_Structural≈ 46.7%

### SLPick por Bandas y TF
- Bandas: lt8=768, 8-10=389, 10-12.5=640, 12.5-15=644, >15=0
- TF: 5m=243, 15m=1346, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.50 (n=1157), 10-15≈ 1.43 (n=1284)

## CancelBias (EMA200@60m)
- Eventos: 402
- Distribución Bias: {'Bullish': 297, 'Bearish': 105, 'Neutral': 0}
- Coherencia (Close>EMA): 297/402 (73.9%)

## StructureFusion
- Trazas por zona: 40959 | Zonas con Anchors: 40593
- Dir zonas (zona): Bull=25839 Bear=15120 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.2, WithAnchors≈ 8.1, DirBull≈ 5.2, DirBear≈ 3.0, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 39481, 'tie-bias': 1158, 'triggers-only': 320}
- TF Triggers: {'60': 19880, '15': 16512, '5': 4567}
- TF Anchors: {'240': 40574, '1440': 34954}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 6, 'score decayó a 0,47': 2, 'score decayó a 0,34': 1, 'score decayó a 0,25': 1, 'score decayó a 0,33': 1, 'score decayó a 0,45': 1, 'score decayó a 0,24': 1, 'score decayó a 0,21': 1}
- Cancel_BOS (diag): por acción {'BUY': 17, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 17, 'Neutral': 0}

## CSV de Trades
- Filas: 191 | Ejecutadas: 61 | Canceladas: 0 | Expiradas: 0
- BUY: 188 | SELL: 64

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4264 | Total candidatos: 33087 | Seleccionados: 2883
- Candidatos por zona (promedio): 7.8
- **Edad (barras)** - Candidatos: med=41, max=80 | Seleccionados: med=41, max=80
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.52
- **TF Candidatos**: {15: 24801}
- **TF Seleccionados**: {15: 2883}
- **DistATR** - Candidatos: avg=20.6 | Seleccionados: avg=10.2
- **Razones de selección**: {'Fallback<15': 1132, 'InBand[10,15]': 1751}
- **En banda [10,15] ATR**: 3928/24801 (15.8%)

### Take Profit (TP)
- Zonas analizadas: 5089 | Total candidatos: 69390 | Seleccionados: 4264
- Candidatos por zona (promedio): 13.6
- **Edad (barras)** - Candidatos: med=18735, max=23176 | Seleccionados: med=5, max=80
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.24
- **Priority Candidatos**: {'P3': 65097}
- **Priority Seleccionados**: {'P3': 2216, 'P4_Fallback': 2048}
- **Type Candidatos**: {'Swing': 65097}
- **Type Seleccionados**: {'Swing': 2216, 'Calculated': 2048}
- **TF Candidatos**: {1440: 20166, 240: 16978, 60: 15213, 15: 12740}
- **TF Seleccionados**: {15: 2216, -1: 2048}
- **DistATR** - Candidatos: avg=124.4 | Seleccionados: avg=17.6
- **RR** - Candidatos: avg=9.64 | Seleccionados: avg=1.42
- **Razones de selección**: {'R:R_y_Distancia_OK': 2075, 'NoStructuralTarget': 2048, 'Distancia_OK_(R:R_ignorado)': 34, 'R:R_OK_(Distancia_ignorada)': 107}

### 🎯 Recomendaciones
- ⚠️ SL: 53% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 48% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.13.