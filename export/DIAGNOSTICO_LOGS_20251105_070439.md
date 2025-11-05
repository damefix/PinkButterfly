# Informe Diagnóstico de Logs - 2025-11-05 07:10:42

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251105_070439.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251105_070439.csv`

## DFM
- Eventos de evaluación: 2380
- Evaluaciones Bull: 4677 | Bear: 1308
- Pasaron umbral (PassedThreshold): 4960
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:10, 4:138, 5:877, 6:2163, 7:2780, 8:17, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 3930
- KeptAligned: 8928/9990 | KeptCounter: 1792/3000
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.512 | AvgProxCounter≈ 0.138
  - AvgDistATRAligned≈ 2.90 | AvgDistATRCounter≈ 0.64
- PreferAligned eventos: 3173 | Filtradas contra-bias: 875

### Proximity (Pre-PreferAligned)
- Eventos: 3930
- Aligned pre: 8928/10720 | Counter pre: 1792/10720
- AvgProxAligned(pre)≈ 0.512 | AvgDistATRAligned(pre)≈ 2.90

### Proximity Drivers
- Eventos: 3930
- Alineadas: n=8928 | BaseProx≈ 0.666 | ZoneATR≈ 4.69 | SizePenalty≈ 0.982 | FinalProx≈ 0.655
- Contra-bias: n=917 | BaseProx≈ 0.553 | ZoneATR≈ 4.61 | SizePenalty≈ 0.983 | FinalProx≈ 0.545

## Risk
- Eventos: 3615
- Accepted=6009 | RejSL=99 | RejTP=3312 | RejRR=0 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=98 | SLDistATR≈ 16.58 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=1 | SLDistATR≈ 17.70 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:94,20-25:3,25+:1
- HistSL Counter 0-10:0,10-15:0,15-20:1,20-25:0,25+:0

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 58 | Unmatched: 5951
- 0-10: Wins=8 Losses=34 WR=19.0% (n=42)
- 10-15: Wins=3 Losses=13 WR=18.8% (n=16)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=11 Losses=47 WR=19.0% (n=58)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 6009 | Aligned=5935 (98.8%)
- Core≈ 1.00 | Prox≈ 0.71 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.22 | Confidence≈ 0.00
- SL_TF dist: {'-1': 37, '5': 136, '15': 520, '240': 636, '60': 3942, '1440': 738} | SL_Structural≈ 99.4%
- TP_TF dist: {'-1': 4681, '240': 183, '5': 113, '60': 85, '1440': 947} | TP_Structural≈ 22.1%

### SLPick por Bandas y TF
- Bandas: lt8=4571, 8-10=762, 10-12.5=465, 12.5-15=211, >15=0
- TF: 5m=136, 15m=520, 60m=3942, 240m=636, 1440m=738
- RR plan por bandas: 0-10≈ 1.23 (n=5333), 10-15≈ 1.07 (n=676)

## CancelBias (EMA200@60m)
- Eventos: 215
- Distribución Bias: {'Bullish': 189, 'Bearish': 26, 'Neutral': 0}
- Coherencia (Close>EMA): 189/215 (87.9%)

## StructureFusion
- Trazas por zona: 12990 | Zonas con Anchors: 12923
- Dir zonas (zona): Bull=7485 Bear=5505 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 2.6, WithAnchors≈ 2.6, DirBull≈ 1.5, DirBear≈ 1.1, DirNeutral≈ 0.0
- Razones de dirección: {'triggers-only': 58, 'tie-bias': 571, 'anchors+triggers': 12361}
- TF Triggers: {'5': 8334, '15': 4656}
- TF Anchors: {'240': 12690, '60': 12815, '1440': 10132}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3748, 'Bearish': 1252, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3748/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 10, 'score decayó a 0,33': 1, 'score decayó a 0,39': 1, 'score decayó a 0,18': 1, 'score decayó a 0,46': 1, 'score decayó a 0,43': 1}
- Cancel_BOS (diag): por acción {'BUY': 1, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 1, 'Neutral': 0}

## CSV de Trades
- Filas: 87 | Ejecutadas: 18 | Canceladas: 0 | Expiradas: 0
- BUY: 79 | SELL: 26

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 4960
- Registered: 44
  - DEDUP_COOLDOWN: 9 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 53

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 1.1%
- RegRate = Registered / Intentos = 83.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 17.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 40.9%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 9805 | Total candidatos: 250513 | Seleccionados: 9751
- Candidatos por zona (promedio): 25.5
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=39, max=150
- **Score** - Candidatos: avg=0.45 | Seleccionados: avg=0.44
- **TF Candidatos**: {240: 85107, 60: 76015, 15: 41716, 1440: 24869, 5: 22806}
- **TF Seleccionados**: {5: 264, 15: 867, 240: 1324, 60: 6154, 1440: 1142}
- **DistATR** - Candidatos: avg=21.9 | Seleccionados: avg=9.9
- **Razones de selección**: {'Fallback<15': 2088, 'InBand[8,15]_TFPreference': 7663}
- **En banda [10,15] ATR**: 42387/250513 (16.9%)

### Take Profit (TP)
- Zonas analizadas: 9845 | Total candidatos: 135868 | Seleccionados: 9845
- Candidatos por zona (promedio): 13.8
- **Edad (barras)** - Candidatos: med=34, max=150 | Seleccionados: med=0, max=68
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.35
- **Priority Candidatos**: {'P3': 135868}
- **Priority Seleccionados**: {'P4_Fallback': 5182, 'P3': 4663}
- **Type Candidatos**: {'Swing': 135868}
- **Type Seleccionados**: {'Calculated': 5182, 'Swing': 4663}
- **TF Candidatos**: {240: 30914, 5: 29567, 15: 29316, 60: 26868, 1440: 19203}
- **TF Seleccionados**: {-1: 5182, 240: 722, 5: 113, 60: 110, 1440: 3718}
- **DistATR** - Candidatos: avg=12.5 | Seleccionados: avg=16.2
- **RR** - Candidatos: avg=1.10 | Seleccionados: avg=1.37
- **Razones de selección**: {'NoStructuralTarget': 5182, 'SwingP3_TF>=60_RR>=1.0_Dist>=6': 4550, 'SwingP3_ANYTF_RR>=1.0_Dist>=6': 113}

### 🎯 Recomendaciones
- ⚠️ SL: 64% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 53% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.89.