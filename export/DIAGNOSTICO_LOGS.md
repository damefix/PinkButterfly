# Informe Diagnóstico de Logs - 2025-10-28 15:27:10

- Log: `.\logs\backtest_20251028_151741.log`
- CSV: `.\logs\trades_20251028_151741.csv`

## DFM
- Eventos de evaluación: 1613
- Evaluaciones Bull: 1720 | Bear: 364
- Pasaron umbral (PassedThreshold): 1469
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:256, 4:288, 5:117, 6:567, 7:452, 8:313, 9:91

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 4999
- KeptAligned: 3100/23143 | KeptCounter: 1586/11913
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.129 | AvgProxCounter≈ 0.061
  - AvgDistATRAligned≈ 2.20 | AvgDistATRCounter≈ 0.54
- PreferAligned eventos: 1911 | Filtradas contra-bias: 232

### Proximity (Pre-PreferAligned)
- Eventos: 4999
- Aligned pre: 3100/4686 | Counter pre: 1586/4686
- AvgProxAligned(pre)≈ 0.129 | AvgDistATRAligned(pre)≈ 2.20

### Proximity Drivers
- Eventos: 4999
- Alineadas: n=3100 | BaseProx≈ 0.432 | ZoneATR≈ 15.72 | SizePenalty≈ 0.806 | FinalProx≈ 0.344
- Contra-bias: n=1354 | BaseProx≈ 0.484 | ZoneATR≈ 33.16 | SizePenalty≈ 0.595 | FinalProx≈ 0.284

## Risk
- Eventos: 2776
- Accepted=2160 | RejSL=1427 | RejTP=94 | RejRR=773 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=907 | SLDistATR≈ 26.41 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=520 | SLDistATR≈ 32.53 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:473,20-25:203,25+:231
- HistSL Counter 0-10:0,10-15:0,15-20:191,20-25:83,25+:246

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 328 | Unmatched: 1832
- 0-10: Wins=55 Losses=120 WR=31.4% (n=175)
- 10-15: Wins=47 Losses=105 WR=30.9% (n=152)
- 15-20: Wins=1 Losses=0 WR=100.0% (n=1)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=103 Losses=225 WR=31.4% (n=328)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 2160 | Aligned=1454 (67.3%)
- Core≈ 0.99 | Prox≈ 0.35 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.96 | Confidence≈ 0.00
- SL_TF dist: {'-1': 857, '15': 1034, '5': 269} | SL_Structural≈ 60.3%
- TP_TF dist: {'-1': 1140, '15': 835, '5': 185} | TP_Structural≈ 47.2%

### SLPick por Bandas y TF
- Bandas: lt8=850, 8-10=359, 10-12.5=475, 12.5-15=476, >15=0
- TF: 5m=269, 15m=1034, 60m=0, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.49 (n=1209), 10-15≈ 1.28 (n=951)

## CancelBias (EMA200@60m)
- Eventos: 477
- Distribución Bias: {'Bullish': 401, 'Bearish': 76, 'Neutral': 0}
- Coherencia (Close>EMA): 401/477 (84.1%)

## StructureFusion
- Trazas por zona: 35056 | Zonas con Anchors: 34619
- Dir zonas (zona): Bull=22204 Bear=12852 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 7.0, WithAnchors≈ 6.9, DirBull≈ 4.4, DirBear≈ 2.6, DirNeutral≈ 0.0
- Razones de dirección: {'anchors+triggers': 33787, 'triggers-only': 374, 'tie-bias': 895}
- TF Triggers: {'60': 16732, '15': 14107, '5': 4217}
- TF Anchors: {'240': 34558, '1440': 29934}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 4999 | Distribución: {'Bullish': 3142, 'Bearish': 1857, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3142/4999

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 14, 'score decayó a 0,41': 1, 'score decayó a 0,47': 1, 'score decayó a 0,30': 1, 'score decayó a 0,36': 1, 'score decayó a 0,48': 1}
- Cancel_BOS (diag): por acción {'BUY': 16, 'SELL': 3} | por bias {'Bullish': 3, 'Bearish': 16, 'Neutral': 0}

## CSV de Trades
- Filas: 255 | Ejecutadas: 86 | Canceladas: 0 | Expiradas: 0
- BUY: 270 | SELL: 71

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 3594 | Total candidatos: 22674 | Seleccionados: 2243
- Candidatos por zona (promedio): 6.3
- **Edad (barras)** - Candidatos: med=33, max=80 | Seleccionados: med=33, max=69
- **Score** - Candidatos: avg=0.43 | Seleccionados: avg=0.41
- **TF Candidatos**: {15: 16050}
- **TF Seleccionados**: {15: 2243}
- **DistATR** - Candidatos: avg=17.7 | Seleccionados: avg=10.0
- **Razones de selección**: {'Fallback<15': 942, 'InBand[10,15]': 1301}
- **En banda [10,15] ATR**: 2704/16050 (16.8%)

### Take Profit (TP)
- Zonas analizadas: 4454 | Total candidatos: 45988 | Seleccionados: 3748
- Candidatos por zona (promedio): 10.3
- **Edad (barras)** - Candidatos: med=5059, max=7902 | Seleccionados: med=3, max=74
- **Score** - Candidatos: avg=0.43 | Seleccionados: avg=0.20
- **Priority Candidatos**: {'P3': 42652}
- **Priority Seleccionados**: {'P4_Fallback': 1850, 'P3': 1898}
- **Type Candidatos**: {'Swing': 42652}
- **Type Seleccionados**: {'Calculated': 1850, 'Swing': 1898}
- **TF Candidatos**: {240: 11540, 60: 11044, 1440: 10299, 15: 9769}
- **TF Seleccionados**: {-1: 1850, 15: 1898}
- **DistATR** - Candidatos: avg=84.0 | Seleccionados: avg=16.9
- **RR** - Candidatos: avg=7.30 | Seleccionados: avg=1.41
- **Razones de selección**: {'NoStructuralTarget': 1850, 'R:R_y_Distancia_OK': 1794, 'R:R_OK_(Distancia_ignorada)': 64, 'Distancia_OK_(R:R_ignorado)': 40}

### 🎯 Recomendaciones
- ⚠️ SL: 72% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 49% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.13.