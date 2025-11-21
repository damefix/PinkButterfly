# Informe Diagnóstico de Logs - 2025-11-10 13:02:08

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251110_130032.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251110_130032.csv`

## DFM
- Eventos de evaluación: 180
- Evaluaciones Bull: 35 | Bear: 316
- Pasaron umbral (PassedThreshold): 351
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:7, 5:7, 6:71, 7:139, 8:127, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 273
- KeptAligned: 709/709 | KeptCounter: 115/160
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.633 | AvgProxCounter≈ 0.144
  - AvgDistATRAligned≈ 2.80 | AvgDistATRCounter≈ 0.98
- PreferAligned eventos: 224 | Filtradas contra-bias: 48

### Proximity (Pre-PreferAligned)
- Eventos: 273
- Aligned pre: 709/824 | Counter pre: 115/824
- AvgProxAligned(pre)≈ 0.633 | AvgDistATRAligned(pre)≈ 2.80

### Proximity Drivers
- Eventos: 273
- Alineadas: n=709 | BaseProx≈ 0.787 | ZoneATR≈ 5.07 | SizePenalty≈ 0.975 | FinalProx≈ 0.771
- Contra-bias: n=67 | BaseProx≈ 0.447 | ZoneATR≈ 4.69 | SizePenalty≈ 0.987 | FinalProx≈ 0.444

## Risk
- Eventos: 265
- Accepted=351 | RejSL=0 | RejTP=0 | RejRR=201 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 141 (53.2% del total)
  - Avg Score: 0.65 | Avg R:R: 1.91 | Avg DistATR: 8.63
  - Por TF: TF5=78, TF15=63
- **P0_SWING_LITE:** 124 (46.8% del total)
  - Avg Score: 0.21 | Avg R:R: 10.36 | Avg DistATR: 9.11
  - Por TF: TF15=30, TF60=94


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 164 | Unmatched: 187
- 0-10: Wins=96 Losses=67 WR=58.9% (n=163)
- 10-15: Wins=1 Losses=0 WR=100.0% (n=1)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=97 Losses=67 WR=59.1% (n=164)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 351 | Aligned=327 (93.2%)
- Core≈ 1.00 | Prox≈ 0.74 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.57 | Confidence≈ 0.00
- SL_TF dist: {'60': 46, '15': 235, '5': 70} | SL_Structural≈ 100.0%
- TP_TF dist: {'240': 141, '60': 59, '15': 138, '5': 13} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=329, 8-10=20, 10-12.5=2, 12.5-15=0, >15=0
- TF: 5m=70, 15m=235, 60m=46, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.57 (n=349), 10-15≈ 1.79 (n=2)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 8168 | Zonas con Anchors: 8168
- Dir zonas (zona): Bull=868 Bear=7108 Neutral=192
- Resumen por ciclo (promedios): TotHZ≈ 3.0, WithAnchors≈ 3.0, DirBull≈ 0.6, DirBear≈ 2.3, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 7976, 'tie-bias': 192}
- TF Triggers: {'15': 658, '5': 211}
- TF Anchors: {'60': 869, '240': 869, '1440': 217}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Cancel_BOS (diag): por acción {'BUY': 7, 'SELL': 9} | por bias {'Bullish': 9, 'Bearish': 7, 'Neutral': 0}

## CSV de Trades
- Filas: 299 | Ejecutadas: 107 | Canceladas: 0 | Expiradas: 0
- BUY: 76 | SELL: 330

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 351
- Registered: 176
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 0
- Intentos de registro: 176

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 50.1%
- RegRate = Registered / Intentos = 100.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 0.0%
- ExecRate = Ejecutadas / Registered = 60.8%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 735 | Total candidatos: 8681 | Seleccionados: 0
- Candidatos por zona (promedio): 11.8

### Take Profit (TP)
- Zonas analizadas: 735 | Total candidatos: 8307 | Seleccionados: 0
- Candidatos por zona (promedio): 11.3
- **Edad (barras)** - Candidatos: med=53, max=333 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.38 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 8307}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 8307}
- **Type Seleccionados**: {}
- **TF Candidatos**: {240: 3115, 60: 2193, 15: 2182, 5: 817}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=10.0 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=4.10 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.