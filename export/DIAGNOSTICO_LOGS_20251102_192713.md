# Informe Diagnóstico de Logs - 2025-11-02 19:30:19

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251102_192713.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251102_192713.csv`

## DFM
- Eventos de evaluación: 1646
- Evaluaciones Bull: 1801 | Bear: 389
- Pasaron umbral (PassedThreshold): 1509
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:233, 4:379, 5:123, 6:513, 7:441, 8:376, 9:125

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 3465/26973 | KeptCounter: 1757/14773
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.140 | AvgProxCounter≈ 0.058
  - AvgDistATRAligned≈ 2.74 | AvgDistATRCounter≈ 0.74
- PreferAligned eventos: 2044 | Filtradas contra-bias: 249

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 3465/5222 | Counter pre: 1757/5222
- AvgProxAligned(pre)≈ 0.140 | AvgDistATRAligned(pre)≈ 2.74

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=3465 | BaseProx≈ 0.444 | ZoneATR≈ 16.84 | SizePenalty≈ 0.772 | FinalProx≈ 0.344
- Contra-bias: n=1508 | BaseProx≈ 0.457 | ZoneATR≈ 33.30 | SizePenalty≈ 0.564 | FinalProx≈ 0.253

## Risk
- Eventos: 2996
- Accepted=2252 | RejSL=1830 | RejTP=61 | RejRR=830 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1251 | SLDistATR≈ 25.28 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=579 | SLDistATR≈ 27.05 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:607,20-25:353,25+:291
- HistSL Counter 0-10:0,10-15:0,15-20:263,20-25:91,25+:225

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 243 | Unmatched: 2009
- 0-10: Wins=25 Losses=37 WR=40.3% (n=62)
- 10-15: Wins=89 Losses=92 WR=49.2% (n=181)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=114 Losses=129 WR=46.9% (n=243)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2252 | Aligned=1489 (66.1%)
- Core≈ 1.00 | Prox≈ 0.34 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.92 | Confidence≈ 0.00
- SL_TF dist: {'-1': 750, '15': 1277, '5': 225} | SL_Structural≈ 66.7%
- TP_TF dist: {'15': 842, '-1': 1199, '5': 211} | TP_Structural≈ 46.8%

### SLPick por Bandas y TF
- Bandas: lt8=671, 8-10=301, 10-12.5=619, 12.5-15=661, >15=0
- TF: 5m=225, 15m=1277, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.59 (n=972), 10-15≈ 1.41 (n=1280)

## CancelBias (EMA200@60m)
- Eventos: 518
- Distribución Bias: {'Bullish': 421, 'Bearish': 97, 'Neutral': 0}
- Coherencia (Close>EMA): 421/518 (81.3%)

## StructureFusion
- Trazas por zona: 41746 | Zonas con Anchors: 41393
- Dir zonas (zona): Bull=26160 Bear=15586 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.3, WithAnchors≈ 8.3, DirBull≈ 5.2, DirBear≈ 3.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 40178, 'tie-bias': 1255, 'triggers-only': 313}
- TF Triggers: {'60': 20420, '15': 16787, '5': 4539}
- TF Anchors: {'240': 41368, '1440': 35720}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 5, 'score decayó a 0,47': 2, 'score decayó a 0,46': 1, 'score decayó a 0,25': 2, 'score decayó a 0,36': 1, 'score decayó a 0,24': 1}
- Cancel_BOS (diag): por acción {'BUY': 18, 'SELL': 3} | por bias {'Bullish': 3, 'Bearish': 18, 'Neutral': 0}

## CSV de Trades
- Filas: 188 | Ejecutadas: 59 | Canceladas: 0 | Expiradas: 0
- BUY: 181 | SELL: 66

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4309 | Total candidatos: 42183 | Seleccionados: 2898
- Candidatos por zona (promedio): 9.8
- **Edad (barras)** - Candidatos: med=51, max=150 | Seleccionados: med=50, max=146
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.47
- **TF Candidatos**: {15: 31621}
- **TF Seleccionados**: {15: 2898}
- **DistATR** - Candidatos: avg=21.4 | Seleccionados: avg=10.5
- **Razones de selección**: {'Fallback<15': 1027, 'InBand[10,15]': 1871}
- **En banda [10,15] ATR**: 4896/31621 (15.5%)

### Take Profit (TP)
- Zonas analizadas: 4973 | Total candidatos: 75877 | Seleccionados: 4143
- Candidatos por zona (promedio): 15.3
- **Edad (barras)** - Candidatos: med=18641, max=23176 | Seleccionados: med=6, max=144
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.22
- **Priority Candidatos**: {'P3': 70954}
- **Priority Seleccionados**: {'P3': 2162, 'P4_Fallback': 1981}
- **Type Candidatos**: {'Swing': 70954}
- **Type Seleccionados**: {'Swing': 2162, 'Calculated': 1981}
- **TF Candidatos**: {240: 20674, 1440: 19644, 60: 16512, 15: 14124}
- **TF Seleccionados**: {15: 2162, -1: 1981}
- **DistATR** - Candidatos: avg=119.5 | Seleccionados: avg=17.8
- **RR** - Candidatos: avg=9.04 | Seleccionados: avg=1.38
- **Razones de selección**: {'R:R_y_Distancia_OK': 2029, 'NoStructuralTarget': 1981, 'Distancia_OK_(R:R_ignorado)': 32, 'R:R_OK_(Distancia_ignorada)': 101}

### 🎯 Recomendaciones
- ⚠️ SL: 63% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 48% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.13.