# Informe Diagnóstico de Logs - 2025-11-04 07:17:43

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_071351.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_071351.csv`

## DFM
- Eventos de evaluación: 1589
- Evaluaciones Bull: 1505 | Bear: 562
- Pasaron umbral (PassedThreshold): 732
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:71, 4:855, 5:795, 6:273, 7:73, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5356
- KeptAligned: 4453/35722 | KeptCounter: 993/10724
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.156 | AvgProxCounter≈ 0.035
  - AvgDistATRAligned≈ 2.95 | AvgDistATRCounter≈ 0.36
- PreferAligned eventos: 2704 | Filtradas contra-bias: 176

### Proximity (Pre-PreferAligned)
- Eventos: 5356
- Aligned pre: 4453/5446 | Counter pre: 993/5446
- AvgProxAligned(pre)≈ 0.156 | AvgDistATRAligned(pre)≈ 2.95

### Proximity Drivers
- Eventos: 5356
- Alineadas: n=4453 | BaseProx≈ 0.428 | ZoneATR≈ 20.57 | SizePenalty≈ 0.720 | FinalProx≈ 0.303
- Contra-bias: n=817 | BaseProx≈ 0.441 | ZoneATR≈ 29.70 | SizePenalty≈ 0.615 | FinalProx≈ 0.268

## Risk
- Eventos: 3236
- Accepted=2140 | RejSL=2150 | RejTP=53 | RejRR=927 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1573 | SLDistATR≈ 26.24 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=577 | SLDistATR≈ 24.19 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:725,20-25:382,25+:466
- HistSL Counter 0-10:0,10-15:0,15-20:237,20-25:171,25+:169

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 416 | Unmatched: 1724
- 0-10: Wins=29 Losses=109 WR=21.0% (n=138)
- 10-15: Wins=170 Losses=108 WR=61.2% (n=278)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=199 Losses=217 WR=47.8% (n=416)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2140 | Aligned=1959 (91.5%)
- Core≈ 1.00 | Prox≈ 0.32 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 3.09 | Confidence≈ 0.00
- SL_TF dist: {'-1': 700, '15': 604, '60': 627, '240': 79, '5': 127, '1440': 3} | SL_Structural≈ 67.3%
- TP_TF dist: {'-1': 594, '15': 555, '240': 124, '60': 385, '5': 62, '1440': 420} | TP_Structural≈ 72.2%

### SLPick por Bandas y TF
- Bandas: lt8=423, 8-10=460, 10-12.5=548, 12.5-15=709, >15=0
- TF: 5m=127, 15m=604, 60m=627, 240m=79, 1440m=3
- RR plan por bandas: 0-10≈ 4.56 (n=883), 10-15≈ 2.06 (n=1257)

## CancelBias (EMA200@60m)
- Eventos: 282
- Distribución Bias: {'Bullish': 243, 'Bearish': 39, 'Neutral': 0}
- Coherencia (Close>EMA): 243/282 (86.2%)

## StructureFusion
- Trazas por zona: 46446 | Zonas con Anchors: 46015
- Dir zonas (zona): Bull=24319 Bear=22127 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.7, WithAnchors≈ 8.6, DirBull≈ 4.5, DirBear≈ 4.1, DirNeutral≈ 0.0
- Razones de dirección: {'tie-bias': 1027, 'triggers-only': 381, 'anchors+triggers': 45038}
- TF Triggers: {'15': 18462, '60': 21492, '5': 6492}
- TF Anchors: {'240': 45993, '1440': 39027}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5362 | Distribución: {'Bullish': 3736, 'Bearish': 1626, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3736/5362

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 2}

## CSV de Trades
- Filas: 149 | Ejecutadas: 72 | Canceladas: 0 | Expiradas: 0
- BUY: 190 | SELL: 31

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5211 | Total candidatos: 138574 | Seleccionados: 4465
- Candidatos por zona (promedio): 26.6
- **Edad (barras)** - Candidatos: med=37, max=151 | Seleccionados: med=38, max=146
- **Score** - Candidatos: avg=0.48 | Seleccionados: avg=0.56
- **TF Candidatos**: {240: 44222, 60: 40533, 15: 29616, 1440: 13833, 5: 10370}
- **TF Seleccionados**: {15: 1481, 60: 1705, 240: 561, 5: 670, 1440: 48}
- **DistATR** - Candidatos: avg=71.7 | Seleccionados: avg=11.0
- **Razones de selección**: {'Fallback<15': 1252, 'InBand[10,15]': 3213}
- **En banda [10,15] ATR**: 9831/138574 (7.1%)

### Take Profit (TP)
- Zonas analizadas: 5270 | Total candidatos: 61604 | Seleccionados: 5270
- Candidatos por zona (promedio): 11.7
- **Edad (barras)** - Candidatos: med=34, max=150 | Seleccionados: med=35, max=150
- **Score** - Candidatos: avg=0.50 | Seleccionados: avg=0.40
- **Priority Candidatos**: {'P3': 61604}
- **Priority Seleccionados**: {'P4_Fallback': 861, 'P3': 4409}
- **Type Candidatos**: {'Swing': 61604}
- **Type Seleccionados**: {'Calculated': 861, 'Swing': 4409}
- **TF Candidatos**: {60: 15613, 240: 15439, 15: 13609, 1440: 12959, 5: 3984}
- **TF Seleccionados**: {-1: 861, 15: 1611, 60: 1237, 240: 502, 5: 508, 1440: 551}
- **DistATR** - Candidatos: avg=81.1 | Seleccionados: avg=25.1
- **RR** - Candidatos: avg=5.75 | Seleccionados: avg=1.81
- **Razones de selección**: {'NoStructuralTarget': 861, 'R:R_y_Distancia_OK': 3660, 'R:R_OK_(Distancia_ignorada)': 733, 'Distancia_OK_(R:R_ignorado)': 16}

### 🎯 Recomendaciones
- ⚠️ SL: 46% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- KeptAligned ratio≈ 0.12.