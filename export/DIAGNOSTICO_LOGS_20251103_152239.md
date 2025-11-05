# Informe Diagnóstico de Logs - 2025-11-03 15:27:31

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251103_152239.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251103_152239.csv`

## DFM
- Eventos de evaluación: 635
- Evaluaciones Bull: 600 | Bear: 169
- Pasaron umbral (PassedThreshold): 234
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:75, 4:343, 5:200, 6:109, 7:41, 8:1, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 1415/17322 | KeptCounter: 574/7601
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.058 | AvgProxCounter≈ 0.022
  - AvgDistATRAligned≈ 0.99 | AvgDistATRCounter≈ 0.24
- PreferAligned eventos: 926 | Filtradas contra-bias: 47

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 1415/1989 | Counter pre: 574/1989
- AvgProxAligned(pre)≈ 0.058 | AvgDistATRAligned(pre)≈ 0.99

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=1415 | BaseProx≈ 0.468 | ZoneATR≈ 24.70 | SizePenalty≈ 0.665 | FinalProx≈ 0.311
- Contra-bias: n=527 | BaseProx≈ 0.488 | ZoneATR≈ 49.69 | SizePenalty≈ 0.489 | FinalProx≈ 0.232

## Risk
- Eventos: 1351
- Accepted=797 | RejSL=833 | RejTP=15 | RejRR=297 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=520 | SLDistATR≈ 29.40 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=313 | SLDistATR≈ 33.75 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:206,20-25:100,25+:214
- HistSL Counter 0-10:0,10-15:0,15-20:86,20-25:66,25+:161

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 148 | Unmatched: 649
- 0-10: Wins=17 Losses=65 WR=20.7% (n=82)
- 10-15: Wins=35 Losses=31 WR=53.0% (n=66)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=52 Losses=96 WR=35.1% (n=148)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 797 | Aligned=609 (76.4%)
- Core≈ 1.00 | Prox≈ 0.32 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.54 | Confidence≈ 0.00
- SL_TF dist: {'-1': 284, '60': 322, '15': 157, '5': 34} | SL_Structural≈ 64.4%
- TP_TF dist: {'-1': 270, '60': 312, '15': 178, '5': 37} | TP_Structural≈ 66.1%

### SLPick por Bandas y TF
- Bandas: lt8=257, 8-10=96, 10-12.5=228, 12.5-15=216, >15=0
- TF: 5m=34, 15m=157, 60m=322, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 3.14 (n=353), 10-15≈ 2.07 (n=444)

## CancelBias (EMA200@60m)
- Eventos: 103
- Distribución Bias: {'Bullish': 87, 'Bearish': 16, 'Neutral': 0}
- Coherencia (Close>EMA): 87/103 (84.5%)

## StructureFusion
- Trazas por zona: 24923 | Zonas con Anchors: 24792
- Dir zonas (zona): Bull=11924 Bear=12999 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 5.0, WithAnchors≈ 5.0, DirBull≈ 2.4, DirBear≈ 2.6, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 23923, 'triggers-only': 120, 'tie-bias': 880}
- TF Triggers: {'60': 19676, '15': 4111, '5': 1136}
- TF Anchors: {'240': 24681, '1440': 22732}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3274, 'Bearish': 1726, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3274/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 1}
- Cancel_BOS (diag): por acción {'BUY': 1, 'SELL': 0} | por bias {'Bullish': 0, 'Bearish': 1, 'Neutral': 0}

## CSV de Trades
- Filas: 146 | Ejecutadas: 69 | Canceladas: 0 | Expiradas: 0
- BUY: 186 | SELL: 29

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 1703 | Total candidatos: 21520 | Seleccionados: 847
- Candidatos por zona (promedio): 12.6
- **Edad (barras)** - Candidatos: med=43, max=100 | Seleccionados: med=25, max=100
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.62
- **TF Candidatos**: {60: 12507}
- **TF Seleccionados**: {60: 847}
- **DistATR** - Candidatos: avg=48.3 | Seleccionados: avg=9.7
- **Razones de selección**: {'Fallback<15': 396, 'InBand[10,15]': 451}
- **En banda [10,15] ATR**: 791/12507 (6.3%)

### Take Profit (TP)
- Zonas analizadas: 1942 | Total candidatos: 26926 | Seleccionados: 1326
- Candidatos por zona (promedio): 13.9
- **Edad (barras)** - Candidatos: med=2958, max=5650 | Seleccionados: med=21, max=100
- **Score** - Candidatos: avg=0.48 | Seleccionados: avg=0.32
- **Priority Candidatos**: {'P3': 22175}
- **Priority Seleccionados**: {'P4_Fallback': 442, 'P3': 884}
- **Type Candidatos**: {'Swing': 22175}
- **Type Seleccionados**: {'Calculated': 442, 'Swing': 884}
- **TF Candidatos**: {240: 7894, 1440: 7317, 60: 6964}
- **TF Seleccionados**: {-1: 442, 60: 884}
- **DistATR** - Candidatos: avg=152.9 | Seleccionados: avg=31.0
- **RR** - Candidatos: avg=11.10 | Seleccionados: avg=1.83
- **Razones de selección**: {'NoStructuralTarget': 442, 'R:R_y_Distancia_OK': 670, 'R:R_OK_(Distancia_ignorada)': 203, 'Distancia_OK_(R:R_ignorado)': 11}

### 🎯 Recomendaciones
- ⚠️ SL: 33% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 33% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.08.