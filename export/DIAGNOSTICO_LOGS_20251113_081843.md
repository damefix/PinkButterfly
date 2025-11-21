# Informe Diagnóstico de Logs - 2025-11-13 08:24:11

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_081843.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_081843.csv`

## DFM
- Eventos de evaluación: 900
- Evaluaciones Bull: 150 | Bear: 712
- Pasaron umbral (PassedThreshold): 862
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:55, 6:363, 7:389, 8:55, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2335
- KeptAligned: 4078/4078 | KeptCounter: 2819/2819
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.455 | AvgProxCounter≈ 0.264
  - AvgDistATRAligned≈ 1.52 | AvgDistATRCounter≈ 1.20
- PreferAligned eventos: 1238 | Filtradas contra-bias: 582

### Proximity (Pre-PreferAligned)
- Eventos: 2335
- Aligned pre: 4078/6897 | Counter pre: 2819/6897
- AvgProxAligned(pre)≈ 0.455 | AvgDistATRAligned(pre)≈ 1.52

### Proximity Drivers
- Eventos: 2335
- Alineadas: n=4078 | BaseProx≈ 0.787 | ZoneATR≈ 5.26 | SizePenalty≈ 0.974 | FinalProx≈ 0.767
- Contra-bias: n=2237 | BaseProx≈ 0.581 | ZoneATR≈ 4.91 | SizePenalty≈ 0.976 | FinalProx≈ 0.569

## Risk
- Eventos: 1912
- Accepted=1222 | RejSL=0 | RejTP=0 | RejRR=1236 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 313 (10.7% del total)
  - Avg Score: 0.40 | Avg R:R: 1.88 | Avg DistATR: 3.73
  - Por TF: TF5=85, TF15=228
- **P0_SWING_LITE:** 2606 (89.3% del total)
  - Avg Score: 0.57 | Avg R:R: 4.15 | Avg DistATR: 3.52
  - Por TF: TF15=506, TF60=2100


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 103 | Unmatched: 1159
- 0-10: Wins=46 Losses=57 WR=44.7% (n=103)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=46 Losses=57 WR=44.7% (n=103)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1262 | Aligned=771 (61.1%)
- Core≈ 1.00 | Prox≈ 0.72 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.99 | Confidence≈ 0.00
- SL_TF dist: {'5': 215, '60': 132, '15': 910, '240': 5} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 284, '5': 294, '15': 506, '240': 178} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1222, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=209, 15m=882, 60m=129, 240m=2, 1440m=0
- RR plan por bandas: 0-10≈ 1.98 (n=1222), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10198 | Zonas con Anchors: 10184
- Dir zonas (zona): Bull=3631 Bear=6197 Neutral=370
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9759, 'tie-bias': 425, 'triggers-only': 14}
- TF Triggers: {'5': 5432, '15': 4766}
- TF Anchors: {'60': 10114, '240': 6001, '1440': 438}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 21, 'estructura inactiva': 1, 'score decayó a 0,24': 1, 'score decayó a 0,36': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 207 | Ejecutadas: 45 | Canceladas: 0 | Expiradas: 0
- BUY: 70 | SELL: 182

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 862
- Registered: 108
  - DEDUP_COOLDOWN: 14 | DEDUP_IDENTICAL: 105 | SKIP_CONCURRENCY: 113
- Intentos de registro: 340

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 39.4%
- RegRate = Registered / Intentos = 31.8%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 35.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 33.2%
- ExecRate = Ejecutadas / Registered = 41.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5448 | Total candidatos: 45142 | Seleccionados: 0
- Candidatos por zona (promedio): 8.3

### Take Profit (TP)
- Zonas analizadas: 5346 | Total candidatos: 54240 | Seleccionados: 5346
- Candidatos por zona (promedio): 10.1
- **Edad (barras)** - Candidatos: med=41, max=204 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 54240}
- **Priority Seleccionados**: {'P3': 3616, 'NA': 1390, 'P0': 340}
- **Type Candidatos**: {'Swing': 54240}
- **Type Seleccionados**: {'P3_Swing': 3616, 'P4_Fallback': 1390, 'P0_Zone': 340}
- **TF Candidatos**: {5: 16464, 15: 14879, 60: 14398, 240: 8499}
- **TF Seleccionados**: {60: 959, 5: 990, -1: 1390, 15: 1275, 240: 732}
- **DistATR** - Candidatos: avg=8.6 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.47 | Seleccionados: avg=1.29
- **Razones de selección**: {'BestIntelligentScore': 5346}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.