# Informe Diagnóstico de Logs - 2025-11-02 19:37:35

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251102_193234.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251102_193234.csv`

## DFM
- Eventos de evaluación: 1687
- Evaluaciones Bull: 1904 | Bear: 385
- Pasaron umbral (PassedThreshold): 1608
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:241, 4:371, 5:127, 6:516, 7:495, 8:403, 9:136

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 3523/26692 | KeptCounter: 1764/14518
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.144 | AvgProxCounter≈ 0.059
  - AvgDistATRAligned≈ 2.72 | AvgDistATRCounter≈ 0.75
- PreferAligned eventos: 2074 | Filtradas contra-bias: 274

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 3523/5287 | Counter pre: 1764/5287
- AvgProxAligned(pre)≈ 0.144 | AvgDistATRAligned(pre)≈ 2.72

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=3523 | BaseProx≈ 0.455 | ZoneATR≈ 17.26 | SizePenalty≈ 0.766 | FinalProx≈ 0.348
- Contra-bias: n=1490 | BaseProx≈ 0.453 | ZoneATR≈ 32.53 | SizePenalty≈ 0.571 | FinalProx≈ 0.256

## Risk
- Eventos: 3025
- Accepted=2349 | RejSL=1776 | RejTP=57 | RejRR=831 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1207 | SLDistATR≈ 25.59 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=569 | SLDistATR≈ 26.38 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:566,20-25:345,25+:296
- HistSL Counter 0-10:0,10-15:0,15-20:251,20-25:101,25+:217

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 247 | Unmatched: 2102
- 0-10: Wins=28 Losses=62 WR=31.1% (n=90)
- 10-15: Wins=68 Losses=89 WR=43.3% (n=157)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=96 Losses=151 WR=38.9% (n=247)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2349 | Aligned=1578 (67.2%)
- Core≈ 1.00 | Prox≈ 0.35 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.99 | Confidence≈ 0.00
- SL_TF dist: {'-1': 829, '15': 1302, '5': 218} | SL_Structural≈ 64.7%
- TP_TF dist: {'15': 860, '-1': 1274, '5': 215} | TP_Structural≈ 45.8%

### SLPick por Bandas y TF
- Bandas: lt8=807, 8-10=327, 10-12.5=665, 12.5-15=550, >15=0
- TF: 5m=218, 15m=1302, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.67 (n=1134), 10-15≈ 1.35 (n=1215)

## CancelBias (EMA200@60m)
- Eventos: 483
- Distribución Bias: {'Bullish': 385, 'Bearish': 98, 'Neutral': 0}
- Coherencia (Close>EMA): 385/483 (79.7%)

## StructureFusion
- Trazas por zona: 41210 | Zonas con Anchors: 40847
- Dir zonas (zona): Bull=25752 Bear=15458 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.2, WithAnchors≈ 8.2, DirBull≈ 5.2, DirBear≈ 3.1, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 39728, 'tie-bias': 1163, 'triggers-only': 319}
- TF Triggers: {'60': 20288, '15': 16372, '5': 4550}
- TF Anchors: {'240': 40820, '1440': 35230}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 4, 'score decayó a 0,47': 1, 'score decayó a 0,25': 2, 'score decayó a 0,36': 1, 'score decayó a 0,24': 1}
- Cancel_BOS (diag): por acción {'BUY': 20, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 20, 'Neutral': 0}

## CSV de Trades
- Filas: 198 | Ejecutadas: 66 | Canceladas: 0 | Expiradas: 0
- BUY: 187 | SELL: 77

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 4247 | Total candidatos: 38969 | Seleccionados: 2880
- Candidatos por zona (promedio): 9.2
- **Edad (barras)** - Candidatos: med=47, max=150 | Seleccionados: med=48, max=146
- **Score** - Candidatos: avg=0.47 | Seleccionados: avg=0.47
- **TF Candidatos**: {15: 28966}
- **TF Seleccionados**: {15: 2880}
- **DistATR** - Candidatos: avg=20.4 | Seleccionados: avg=10.3
- **Razones de selección**: {'Fallback<15': 1042, 'InBand[10,15]': 1838}
- **En banda [10,15] ATR**: 4642/28966 (16.0%)

### Take Profit (TP)
- Zonas analizadas: 5013 | Total candidatos: 71154 | Seleccionados: 4212
- Candidatos por zona (promedio): 14.2
- **Edad (barras)** - Candidatos: med=18562, max=23172 | Seleccionados: med=2, max=135
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.22
- **Priority Candidatos**: {'P3': 66702}
- **Priority Seleccionados**: {'P3': 2113, 'P4_Fallback': 2099}
- **Type Candidatos**: {'Swing': 66702}
- **Type Seleccionados**: {'Swing': 2113, 'Calculated': 2099}
- **TF Candidatos**: {240: 19189, 1440: 18490, 60: 15690, 15: 13333}
- **TF Seleccionados**: {15: 2113, -1: 2099}
- **DistATR** - Candidatos: avg=113.0 | Seleccionados: avg=17.2
- **RR** - Candidatos: avg=8.87 | Seleccionados: avg=1.42
- **Razones de selección**: {'R:R_y_Distancia_OK': 1986, 'NoStructuralTarget': 2099, 'Distancia_OK_(R:R_ignorado)': 58, 'R:R_OK_(Distancia_ignorada)': 69}

### 🎯 Recomendaciones
- ⚠️ SL: 62% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 50% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.13.