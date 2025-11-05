# Informe Diagnóstico de Logs - 2025-11-03 09:27:37

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251103_092306.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251103_092306.csv`

## DFM
- Eventos de evaluación: 1516
- Evaluaciones Bull: 1639 | Bear: 336
- Pasaron umbral (PassedThreshold): 729
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:197, 4:749, 5:545, 6:376, 7:108, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 3026/26899 | KeptCounter: 1517/14408
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.130 | AvgProxCounter≈ 0.054
  - AvgDistATRAligned≈ 2.17 | AvgDistATRCounter≈ 0.58
- PreferAligned eventos: 1887 | Filtradas contra-bias: 180

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 3026/4543 | Counter pre: 1517/4543
- AvgProxAligned(pre)≈ 0.130 | AvgDistATRAligned(pre)≈ 2.17

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=3026 | BaseProx≈ 0.445 | ZoneATR≈ 16.98 | SizePenalty≈ 0.774 | FinalProx≈ 0.344
- Contra-bias: n=1337 | BaseProx≈ 0.464 | ZoneATR≈ 31.36 | SizePenalty≈ 0.571 | FinalProx≈ 0.258

## Risk
- Eventos: 2778
- Accepted=2059 | RejSL=1510 | RejTP=52 | RejRR=742 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1031 | SLDistATR≈ 26.13 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=479 | SLDistATR≈ 25.82 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:495,20-25:265,25+:271
- HistSL Counter 0-10:0,10-15:0,15-20:198,20-25:92,25+:189

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 311 | Unmatched: 1748
- 0-10: Wins=33 Losses=51 WR=39.3% (n=84)
- 10-15: Wins=125 Losses=102 WR=55.1% (n=227)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=158 Losses=153 WR=50.8% (n=311)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2059 | Aligned=1358 (66.0%)
- Core≈ 1.00 | Prox≈ 0.35 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.99 | Confidence≈ 0.00
- SL_TF dist: {'-1': 709, '15': 1163, '5': 187} | SL_Structural≈ 65.6%
- TP_TF dist: {'15': 804, '-1': 1090, '5': 165} | TP_Structural≈ 47.1%

### SLPick por Bandas y TF
- Bandas: lt8=680, 8-10=302, 10-12.5=579, 12.5-15=498, >15=0
- TF: 5m=187, 15m=1163, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.61 (n=982), 10-15≈ 1.41 (n=1077)

## CancelBias (EMA200@60m)
- Eventos: 252
- Distribución Bias: {'Bullish': 212, 'Bearish': 40, 'Neutral': 0}
- Coherencia (Close>EMA): 212/252 (84.1%)

## StructureFusion
- Trazas por zona: 41307 | Zonas con Anchors: 40950
- Dir zonas (zona): Bull=25774 Bear=15533 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.3, WithAnchors≈ 8.2, DirBull≈ 5.2, DirBear≈ 3.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 39836, 'tie-bias': 1151, 'triggers-only': 320}
- TF Triggers: {'60': 20256, '15': 16536, '5': 4515}
- TF Anchors: {'240': 40926, '1440': 35293}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 2, 'score decayó a 0,44': 1}
- Cancel_BOS (diag): por acción {'BUY': 6, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 6, 'Neutral': 0}

## CSV de Trades
- Filas: 140 | Ejecutadas: 59 | Canceladas: 0 | Expiradas: 0
- BUY: 170 | SELL: 29

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 3755 | Total candidatos: 34499 | Seleccionados: 2512
- Candidatos por zona (promedio): 9.2
- **Edad (barras)** - Candidatos: med=47, max=150 | Seleccionados: med=48, max=146
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.47
- **TF Candidatos**: {15: 25618}
- **TF Seleccionados**: {15: 2512}
- **DistATR** - Candidatos: avg=20.2 | Seleccionados: avg=10.5
- **Razones de selección**: {'Fallback<15': 909, 'InBand[10,15]': 1603}
- **En banda [10,15] ATR**: 4097/25618 (16.0%)

### Take Profit (TP)
- Zonas analizadas: 4363 | Total candidatos: 62733 | Seleccionados: 3704
- Candidatos por zona (promedio): 14.4
- **Edad (barras)** - Candidatos: med=18514, max=23176 | Seleccionados: med=9, max=150
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.23
- **Priority Candidatos**: {'P3': 59109}
- **Priority Seleccionados**: {'P3': 1981, 'P4_Fallback': 1723}
- **Type Candidatos**: {'Swing': 59109}
- **Type Seleccionados**: {'Swing': 1981, 'Calculated': 1723}
- **TF Candidatos**: {1440: 16754, 240: 16615, 60: 14027, 15: 11713}
- **TF Seleccionados**: {15: 1981, -1: 1723}
- **DistATR** - Candidatos: avg=117.9 | Seleccionados: avg=17.2
- **RR** - Candidatos: avg=9.08 | Seleccionados: avg=1.41
- **Razones de selección**: {'R:R_y_Distancia_OK': 1855, 'NoStructuralTarget': 1723, 'Distancia_OK_(R:R_ignorado)': 37, 'R:R_OK_(Distancia_ignorada)': 89}

### 🎯 Recomendaciones
- ⚠️ SL: 61% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 47% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.11.