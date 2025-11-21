# Informe Diagnóstico de Logs - 2025-11-11 20:44:08

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_203825.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_203825.csv`

## DFM
- Eventos de evaluación: 185
- Evaluaciones Bull: 66 | Bear: 136
- Pasaron umbral (PassedThreshold): 186
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:4, 4:12, 5:15, 6:104, 7:61, 8:6, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2217
- KeptAligned: 1844/1844 | KeptCounter: 1273/1273
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.369 | AvgProxCounter≈ 0.233
  - AvgDistATRAligned≈ 0.53 | AvgDistATRCounter≈ 0.37
- PreferAligned eventos: 841 | Filtradas contra-bias: 242

### Proximity (Pre-PreferAligned)
- Eventos: 2217
- Aligned pre: 1844/3117 | Counter pre: 1273/3117
- AvgProxAligned(pre)≈ 0.369 | AvgDistATRAligned(pre)≈ 0.53

### Proximity Drivers
- Eventos: 2217
- Alineadas: n=1844 | BaseProx≈ 0.872 | ZoneATR≈ 5.02 | SizePenalty≈ 0.978 | FinalProx≈ 0.853
- Contra-bias: n=1031 | BaseProx≈ 0.764 | ZoneATR≈ 4.64 | SizePenalty≈ 0.981 | FinalProx≈ 0.749

## Risk
- Eventos: 1403
- Accepted=202 | RejSL=0 | RejTP=0 | RejRR=177 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 31 (7.1% del total)
  - Avg Score: 0.41 | Avg R:R: 1.91 | Avg DistATR: 3.47
  - Por TF: TF5=8, TF15=23
- **P0_SWING_LITE:** 407 (92.9% del total)
  - Avg Score: 0.51 | Avg R:R: 3.94 | Avg DistATR: 3.42
  - Por TF: TF15=72, TF60=335


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 56 | Unmatched: 147
- 0-10: Wins=20 Losses=36 WR=35.7% (n=56)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=20 Losses=36 WR=35.7% (n=56)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 203 | Aligned=120 (59.1%)
- Core≈ 1.00 | Prox≈ 0.80 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.76 | Confidence≈ 0.00
- SL_TF dist: {'60': 16, '15': 149, '5': 38} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 40, '15': 106, '60': 48, '240': 9} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=202, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=37, 15m=149, 60m=16, 240m=0, 1440m=0
- RR plan por bandas: 0-10≈ 1.76 (n=202), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 39738 | Zonas con Anchors: 39724
- Dir zonas (zona): Bull=8809 Bear=29963 Neutral=966
- Resumen por ciclo (promedios): TotHZ≈ 3.7, WithAnchors≈ 3.7, DirBull≈ 1.5, DirBear≈ 2.1, DirNeutral≈ 0.2
- Razones de dirección: {'anchors+triggers': 38521, 'tie-bias': 1203, 'triggers-only': 14}
- TF Triggers: {'15': 4297, '5': 4992}
- TF Anchors: {'60': 9231, '240': 5453, '1440': 550}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 2}

## CSV de Trades
- Filas: 95 | Ejecutadas: 35 | Canceladas: 0 | Expiradas: 0
- BUY: 55 | SELL: 75

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 186
- Registered: 51
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 27 | SKIP_CONCURRENCY: 6
- Intentos de registro: 84

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 45.2%
- RegRate = Registered / Intentos = 60.7%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 32.1%
- Concurrency = SKIP_CONCURRENCY / Intentos = 7.1%
- ExecRate = Ejecutadas / Registered = 68.6%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 607 | Total candidatos: 10024 | Seleccionados: 0
- Candidatos por zona (promedio): 16.5

### Take Profit (TP)
- Zonas analizadas: 594 | Total candidatos: 4570 | Seleccionados: 594
- Candidatos por zona (promedio): 7.7
- **Edad (barras)** - Candidatos: med=53, max=182 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.40 | Seleccionados: avg=0.70
- **Priority Candidatos**: {'P3': 4570}
- **Priority Seleccionados**: {'NA': 268, 'P0': 65, 'P3': 261}
- **Type Candidatos**: {'Swing': 4570}
- **Type Seleccionados**: {'P4_Fallback': 268, 'P0_Zone': 65, 'P3_Swing': 261}
- **TF Candidatos**: {60: 1416, 15: 1209, 5: 1145, 240: 800}
- **TF Seleccionados**: {-1: 268, 5: 73, 15: 150, 60: 81, 240: 22}
- **DistATR** - Candidatos: avg=9.0 | Seleccionados: avg=3.5
- **RR** - Candidatos: avg=4.65 | Seleccionados: avg=1.33
- **Razones de selección**: {'BestIntelligentScore': 594}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.