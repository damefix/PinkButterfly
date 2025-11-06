# Informe Diagnóstico de Logs - 2025-11-03 08:50:07

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251103_084557.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251103_084557.csv`

## DFM
- Eventos de evaluación: 1462
- Evaluaciones Bull: 1543 | Bear: 329
- Pasaron umbral (PassedThreshold): 684
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:182, 4:719, 5:515, 6:356, 7:100, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 2838/26849 | KeptCounter: 1395/14349
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.126 | AvgProxCounter≈ 0.052
  - AvgDistATRAligned≈ 1.99 | AvgDistATRCounter≈ 0.51
- PreferAligned eventos: 1816 | Filtradas contra-bias: 150

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 2838/4233 | Counter pre: 1395/4233
- AvgProxAligned(pre)≈ 0.126 | AvgDistATRAligned(pre)≈ 1.99

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=2838 | BaseProx≈ 0.450 | ZoneATR≈ 17.24 | SizePenalty≈ 0.771 | FinalProx≈ 0.346
- Contra-bias: n=1245 | BaseProx≈ 0.479 | ZoneATR≈ 30.86 | SizePenalty≈ 0.572 | FinalProx≈ 0.267

## Risk
- Eventos: 2662
- Accepted=1963 | RejSL=1393 | RejTP=48 | RejRR=679 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=958 | SLDistATR≈ 26.17 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=435 | SLDistATR≈ 26.03 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:462,20-25:240,25+:256
- HistSL Counter 0-10:0,10-15:0,15-20:184,20-25:81,25+:170

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 330 | Unmatched: 1633
- 0-10: Wins=45 Losses=55 WR=45.0% (n=100)
- 10-15: Wins=129 Losses=101 WR=56.1% (n=230)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=174 Losses=156 WR=52.7% (n=330)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1963 | Aligned=1298 (66.1%)
- Core≈ 0.99 | Prox≈ 0.35 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.98 | Confidence≈ 0.00
- SL_TF dist: {'-1': 666, '15': 1119, '5': 178} | SL_Structural≈ 66.1%
- TP_TF dist: {'15': 757, '-1': 1055, '5': 151} | TP_Structural≈ 46.3%

### SLPick por Bandas y TF
- Bandas: lt8=650, 8-10=280, 10-12.5=564, 12.5-15=469, >15=0
- TF: 5m=178, 15m=1119, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.60 (n=930), 10-15≈ 1.41 (n=1033)

## CancelBias (EMA200@60m)
- Eventos: 234
- Distribución Bias: {'Bullish': 196, 'Bearish': 38, 'Neutral': 0}
- Coherencia (Close>EMA): 196/234 (83.8%)

## StructureFusion
- Trazas por zona: 41198 | Zonas con Anchors: 40844
- Dir zonas (zona): Bull=25740 Bear=15458 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.2, WithAnchors≈ 8.2, DirBull≈ 5.1, DirBear≈ 3.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 39706, 'tie-bias': 1176, 'triggers-only': 316}
- TF Triggers: {'60': 20240, '15': 16455, '5': 4503}
- TF Anchors: {'240': 40818, '1440': 35177}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 2, 'score decayó a 0,44': 1}
- Cancel_BOS (diag): por acción {'BUY': 7, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 7, 'Neutral': 0}

## CSV de Trades
- Filas: 138 | Ejecutadas: 57 | Canceladas: 0 | Expiradas: 0
- BUY: 166 | SELL: 29

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 3519 | Total candidatos: 32325 | Seleccionados: 2363
- Candidatos por zona (promedio): 9.2
- **Edad (barras)** - Candidatos: med=46, max=150 | Seleccionados: med=47, max=147
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.47
- **TF Candidatos**: {15: 24068}
- **TF Seleccionados**: {15: 2363}
- **DistATR** - Candidatos: avg=20.3 | Seleccionados: avg=10.5
- **Razones de selección**: {'Fallback<15': 843, 'InBand[10,15]': 1520}
- **En banda [10,15] ATR**: 3870/24068 (16.1%)

### Take Profit (TP)
- Zonas analizadas: 4083 | Total candidatos: 58308 | Seleccionados: 3475
- Candidatos por zona (promedio): 14.3
- **Edad (barras)** - Candidatos: med=18637, max=23176 | Seleccionados: med=7, max=150
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.22
- **Priority Candidatos**: {'P3': 54927}
- **Priority Seleccionados**: {'P3': 1825, 'P4_Fallback': 1650}
- **Type Candidatos**: {'Swing': 54927}
- **Type Seleccionados**: {'Swing': 1825, 'Calculated': 1650}
- **TF Candidatos**: {1440: 15928, 240: 15485, 60: 12939, 15: 10575}
- **TF Seleccionados**: {15: 1825, -1: 1650}
- **DistATR** - Candidatos: avg=120.3 | Seleccionados: avg=17.3
- **RR** - Candidatos: avg=9.41 | Seleccionados: avg=1.42
- **Razones de selección**: {'R:R_y_Distancia_OK': 1699, 'NoStructuralTarget': 1650, 'Distancia_OK_(R:R_ignorado)': 39, 'R:R_OK_(Distancia_ignorada)': 87}

### 🎯 Recomendaciones
- ⚠️ SL: 60% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 47% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.11.