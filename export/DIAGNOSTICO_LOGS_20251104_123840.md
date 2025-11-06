# Informe Diagnóstico de Logs - 2025-11-04 12:47:18

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_123840.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_123840.csv`

## DFM
- Eventos de evaluación: 2087
- Evaluaciones Bull: 2105 | Bear: 595
- Pasaron umbral (PassedThreshold): 744
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:260, 4:1241, 5:752, 6:368, 7:79, 8:0, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 9996
- KeptAligned: 7338/65673 | KeptCounter: 3051/37821
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.123 | AvgProxCounter≈ 0.060
  - AvgDistATRAligned≈ 2.26 | AvgDistATRCounter≈ 0.57
- PreferAligned eventos: 4043 | Filtradas contra-bias: 468

### Proximity (Pre-PreferAligned)
- Eventos: 9996
- Aligned pre: 7338/10389 | Counter pre: 3051/10389
- AvgProxAligned(pre)≈ 0.123 | AvgDistATRAligned(pre)≈ 2.26

### Proximity Drivers
- Eventos: 9996
- Alineadas: n=7338 | BaseProx≈ 0.452 | ZoneATR≈ 23.07 | SizePenalty≈ 0.684 | FinalProx≈ 0.307
- Contra-bias: n=2583 | BaseProx≈ 0.467 | ZoneATR≈ 35.15 | SizePenalty≈ 0.591 | FinalProx≈ 0.273

## Risk
- Eventos: 5783
- Accepted=2768 | RejSL=5110 | RejTP=262 | RejRR=1778 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=3529 | SLDistATR≈ 26.66 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=1581 | SLDistATR≈ 28.19 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:1495,20-25:729,25+:1305
- HistSL Counter 0-10:0,10-15:0,15-20:649,20-25:374,25+:558

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 340 | Unmatched: 2428
- 0-10: Wins=18 Losses=53 WR=25.4% (n=71)
- 10-15: Wins=149 Losses=120 WR=55.4% (n=269)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=167 Losses=173 WR=49.1% (n=340)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2768 | Aligned=2057 (74.3%)
- Core≈ 1.00 | Prox≈ 0.31 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.22 | Confidence≈ 0.00
- SL_TF dist: {'60': 804, '-1': 594, '5': 854, '15': 408, '240': 104, '1440': 4} | SL_Structural≈ 78.5%
- TP_TF dist: {'-1': 738, '60': 371, '5': 625, '15': 690, '240': 286, '1440': 58} | TP_Structural≈ 73.3%

### SLPick por Bandas y TF
- Bandas: lt8=621, 8-10=367, 10-12.5=832, 12.5-15=948, >15=0
- TF: 5m=854, 15m=408, 60m=804, 240m=104, 1440m=4
- RR plan por bandas: 0-10≈ 2.89 (n=988), 10-15≈ 1.85 (n=1780)

## CancelBias (EMA200@60m)
- Eventos: 322
- Distribución Bias: {'Bullish': 224, 'Bearish': 98, 'Neutral': 0}
- Coherencia (Close>EMA): 224/322 (69.6%)

## StructureFusion
- Trazas por zona: 103494 | Zonas con Anchors: 102177
- Dir zonas (zona): Bull=42655 Bear=60839 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 10.4, WithAnchors≈ 10.2, DirBull≈ 4.3, DirBear≈ 6.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 97084, 'triggers-only': 1212, 'tie-bias': 5198}
- TF Triggers: {'5': 29651, '60': 38810, '15': 35033}
- TF Anchors: {'240': 101178, '1440': 91230}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 9996 | Distribución: {'Bullish': 5885, 'Bearish': 4111, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 5885/9996

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 6, 'score decayó a 0,21': 1, 'score decayó a 0,36': 1, 'score decayó a 0,43': 1}
- Cancel_BOS (diag): por acción {'BUY': 1, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 1, 'Neutral': 0}

## CSV de Trades
- Filas: 252 | Ejecutadas: 114 | Canceladas: 0 | Expiradas: 0
- BUY: 285 | SELL: 81

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 9816 | Total candidatos: 289645 | Seleccionados: 8844
- Candidatos por zona (promedio): 29.5
- **Edad (barras)** - Candidatos: med=38, max=151 | Seleccionados: med=35, max=149
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.56
- **TF Candidatos**: {240: 81075, 60: 67533, 15: 57594, 5: 50704, 1440: 32711}
- **TF Seleccionados**: {60: 3005, 5: 3673, 15: 1470, 240: 655, 1440: 41}
- **DistATR** - Candidatos: avg=103.2 | Seleccionados: avg=10.9
- **Razones de selección**: {'Fallback<15': 2456, 'InBand[10,15]': 6388}
- **En banda [10,15] ATR**: 23611/289617 (8.2%)

### Take Profit (TP)
- Zonas analizadas: 9919 | Total candidatos: 176440 | Seleccionados: 9919
- Candidatos por zona (promedio): 17.8
- **Edad (barras)** - Candidatos: med=41, max=151 | Seleccionados: med=44, max=150
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.36
- **Priority Candidatos**: {'P3': 176440}
- **Priority Seleccionados**: {'P4_Fallback': 1138, 'P3': 8781}
- **Type Candidatos**: {'Swing': 176440}
- **Type Seleccionados**: {'Calculated': 1138, 'Swing': 8781}
- **TF Candidatos**: {60: 43925, 240: 42129, 15: 39993, 1440: 29030, 5: 21363}
- **TF Seleccionados**: {-1: 1138, 5: 2892, 60: 1741, 15: 2860, 240: 977, 1440: 311}
- **DistATR** - Candidatos: avg=108.7 | Seleccionados: avg=21.3
- **RR** - Candidatos: avg=8.16 | Seleccionados: avg=1.26
- **Razones de selección**: {'NoStructuralTarget': 1138, 'R:R_y_Distancia_OK': 7872, 'R:R_OK_(Distancia_ignorada)': 891, 'Distancia_OK_(R:R_ignorado)': 18}

### 🎯 Recomendaciones
- ⚠️ SL: 42% tienen score < 0.5. Considerar umbral mínimo de calidad.

## Observaciones automáticas
- KeptAligned ratio≈ 0.11.