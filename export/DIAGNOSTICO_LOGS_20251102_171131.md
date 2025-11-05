# Informe Diagnóstico de Logs - 2025-11-02 17:16:41

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251102_171131.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251102_171131.csv`

## DFM
- Eventos de evaluación: 1892
- Evaluaciones Bull: 2181 | Bear: 398
- Pasaron umbral (PassedThreshold): 1884
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:212, 4:415, 5:123, 6:643, 7:578, 8:459, 9:149

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 5000
- KeptAligned: 4364/28379 | KeptCounter: 2054/15365
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.163 | AvgProxCounter≈ 0.065
  - AvgDistATRAligned≈ 3.67 | AvgDistATRCounter≈ 0.97
- PreferAligned eventos: 2382 | Filtradas contra-bias: 403

### Proximity (Pre-PreferAligned)
- Eventos: 5000
- Aligned pre: 4364/6418 | Counter pre: 2054/6418
- AvgProxAligned(pre)≈ 0.163 | AvgDistATRAligned(pre)≈ 3.67

### Proximity Drivers
- Eventos: 5000
- Alineadas: n=4364 | BaseProx≈ 0.451 | ZoneATR≈ 17.97 | SizePenalty≈ 0.754 | FinalProx≈ 0.343
- Contra-bias: n=1651 | BaseProx≈ 0.459 | ZoneATR≈ 36.27 | SizePenalty≈ 0.567 | FinalProx≈ 0.258

## Risk
- Eventos: 3375
- Accepted=2629 | RejSL=2302 | RejTP=78 | RejRR=1006 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=1632 | SLDistATR≈ 25.61 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=670 | SLDistATR≈ 28.68 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:742,20-25:478,25+:412
- HistSL Counter 0-10:0,10-15:0,15-20:263,20-25:116,25+:291

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 365 | Unmatched: 2264
- 0-10: Wins=57 Losses=59 WR=49.1% (n=116)
- 10-15: Wins=154 Losses=94 WR=62.1% (n=248)
- 15-20: Wins=0 Losses=1 WR=0.0% (n=1)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=211 Losses=154 WR=57.8% (n=365)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2629 | Aligned=1852 (70.4%)
- Core≈ 1.00 | Prox≈ 0.34 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.89 | Confidence≈ 0.00
- SL_TF dist: {'-1': 841, '15': 1537, '5': 251} | SL_Structural≈ 68.0%
- TP_TF dist: {'-1': 1359, '15': 1039, '5': 231} | TP_Structural≈ 48.3%

### SLPick por Bandas y TF
- Bandas: lt8=752, 8-10=343, 10-12.5=755, 12.5-15=779, >15=0
- TF: 5m=251, 15m=1537, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.57 (n=1095), 10-15≈ 1.40 (n=1534)

## CancelBias (EMA200@60m)
- Eventos: 484
- Distribución Bias: {'Bullish': 405, 'Bearish': 79, 'Neutral': 0}
- Coherencia (Close>EMA): 405/484 (83.7%)

## StructureFusion
- Trazas por zona: 43744 | Zonas con Anchors: 43377
- Dir zonas (zona): Bull=27608 Bear=16136 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 8.7, WithAnchors≈ 8.7, DirBull≈ 5.5, DirBear≈ 3.2, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 42095, 'tie-bias': 1318, 'triggers-only': 331}
- TF Triggers: {'60': 21464, '15': 17396, '5': 4884}
- TF Anchors: {'240': 43346, '1440': 37588}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3056, 'Bearish': 1944, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3056/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 7, 'score decayó a 0,45': 1, 'score decayó a 0,35': 1, 'score decayó a 0,34': 1, 'score decayó a 0,49': 2, 'score decayó a 0,28': 1, 'score decayó a 0,48': 1, 'score decayó a 0,42': 1, 'score decayó a 0,30': 1, 'score decayó a 0,37': 1, 'score decayó a 0,29': 2}
- Cancel_BOS (diag): por acción {'BUY': 19, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 19, 'Neutral': 0}

## CSV de Trades
- Filas: 221 | Ejecutadas: 68 | Canceladas: 0 | Expiradas: 0
- BUY: 236 | SELL: 53

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5440 | Total candidatos: 60845 | Seleccionados: 3449
- Candidatos por zona (promedio): 11.2
- **Edad (barras)** - Candidatos: med=59, max=150 | Seleccionados: med=53, max=147
- **Score** - Candidatos: avg=0.41 | Seleccionados: avg=0.47
- **TF Candidatos**: {15: 45468}
- **TF Seleccionados**: {15: 3449}
- **DistATR** - Candidatos: avg=24.9 | Seleccionados: avg=10.4
- **Razones de selección**: {'Fallback<15': 1214, 'InBand[10,15]': 2235}
- **En banda [10,15] ATR**: 5986/45468 (13.2%)

### Take Profit (TP)
- Zonas analizadas: 6015 | Total candidatos: 105305 | Seleccionados: 4835
- Candidatos por zona (promedio): 17.5
- **Edad (barras)** - Candidatos: med=18661, max=23193 | Seleccionados: med=6, max=150
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.22
- **Priority Candidatos**: {'P3': 97333}
- **Priority Seleccionados**: {'P4_Fallback': 2308, 'P3': 2527}
- **Type Candidatos**: {'Swing': 97333}
- **Type Seleccionados**: {'Calculated': 2308, 'Swing': 2527}
- **TF Candidatos**: {240: 27660, 1440: 27599, 60: 22278, 15: 19796}
- **TF Seleccionados**: {-1: 2308, 15: 2527}
- **DistATR** - Candidatos: avg=142.6 | Seleccionados: avg=18.4
- **RR** - Candidatos: avg=10.03 | Seleccionados: avg=1.37
- **Razones de selección**: {'NoStructuralTarget': 2308, 'R:R_y_Distancia_OK': 2378, 'Distancia_OK_(R:R_ignorado)': 44, 'R:R_OK_(Distancia_ignorada)': 105}

### 🎯 Recomendaciones
- ⚠️ SL: 64% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 48% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.15.