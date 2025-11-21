# Informe Diagnóstico de Logs - 2025-11-11 10:13:43

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_094049.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_094049.csv`

## DFM
- Eventos de evaluación: 189
- Evaluaciones Bull: 43 | Bear: 179
- Pasaron umbral (PassedThreshold): 180
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:1, 4:14, 5:14, 6:55, 7:136, 8:2, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2186
- KeptAligned: 1866/1866 | KeptCounter: 1045/1045
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.385 | AvgProxCounter≈ 0.243
  - AvgDistATRAligned≈ 0.53 | AvgDistATRCounter≈ 0.37
- PreferAligned eventos: 937 | Filtradas contra-bias: 329

### Proximity (Pre-PreferAligned)
- Eventos: 2186
- Aligned pre: 1866/2911 | Counter pre: 1045/2911
- AvgProxAligned(pre)≈ 0.385 | AvgDistATRAligned(pre)≈ 0.53

### Proximity Drivers
- Eventos: 2186
- Alineadas: n=1866 | BaseProx≈ 0.919 | ZoneATR≈ 5.10 | SizePenalty≈ 0.977 | FinalProx≈ 0.897
- Contra-bias: n=716 | BaseProx≈ 0.833 | ZoneATR≈ 4.94 | SizePenalty≈ 0.977 | FinalProx≈ 0.814

## Risk
- Eventos: 1362
- Accepted=223 | RejSL=0 | RejTP=0 | RejRR=158 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 32 (18.2% del total)
  - Avg Score: 0.67 | Avg R:R: 2.02 | Avg DistATR: 7.66
  - Por TF: TF5=14, TF15=18
- **P0_SWING_LITE:** 144 (81.8% del total)
  - Avg Score: 0.33 | Avg R:R: 9.00 | Avg DistATR: 9.64
  - Por TF: TF15=77, TF60=67


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 44 | Unmatched: 179
- 0-10: Wins=23 Losses=21 WR=52.3% (n=44)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=23 Losses=21 WR=52.3% (n=44)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 223 | Aligned=147 (65.9%)
- Core≈ 1.00 | Prox≈ 0.89 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.29 | Confidence≈ 0.00
- SL_TF dist: {'60': 2, '15': 178, '5': 43} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 43, '15': 78, '60': 86, '240': 16} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=223, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=43, 15m=178, 60m=2, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 2.29 (n=223), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 41908 | Zonas con Anchors: 41896
- Dir zonas (zona): Bull=7833 Bear=32925 Neutral=1150
- Resumen por ciclo (promedios): TotHZ≈ 3.6, WithAnchors≈ 3.6, DirBull≈ 1.3, DirBear≈ 2.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 40746, 'tie-bias': 1150, 'triggers-only': 12}
- TF Triggers: {'5': 4981, '15': 3912}
- TF Anchors: {'60': 8845, '240': 5431, '1440': 255}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,21': 1, 'score decayó a 0,43': 1, 'score decayó a 0,48': 2, 'score decayó a 0,30': 1, 'estructura no existe': 4, 'score decayó a 0,29': 1, 'score decayó a 0,18': 1}
- Cancel_BOS (diag): por acción {'BUY': 2, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 2, 'Neutral': 0}

## CSV de Trades
- Filas: 94 | Ejecutadas: 27 | Canceladas: 0 | Expiradas: 0
- BUY: 29 | SELL: 92

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 180
- Registered: 52
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 42 | SKIP_CONCURRENCY: 34
- Intentos de registro: 128

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 71.1%
- RegRate = Registered / Intentos = 40.6%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 32.8%
- Concurrency = SKIP_CONCURRENCY / Intentos = 26.6%
- ExecRate = Ejecutadas / Registered = 51.9%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 572 | Total candidatos: 10302 | Seleccionados: 0
- Candidatos por zona (promedio): 18.0

### Take Profit (TP)
- Zonas analizadas: 572 | Total candidatos: 4676 | Seleccionados: 0
- Candidatos por zona (promedio): 8.2
- **Edad (barras)** - Candidatos: med=53, max=250 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.44 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 4676}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 4676}
- **Type Seleccionados**: {}
- **TF Candidatos**: {60: 1448, 5: 1340, 15: 1247, 240: 641}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=8.9 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=4.00 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.