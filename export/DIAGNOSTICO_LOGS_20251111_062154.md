# Informe Diagnóstico de Logs - 2025-11-11 07:37:03

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251111_062154.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_062154.csv`

## DFM
- Eventos de evaluación: 3803
- Evaluaciones Bull: 2642 | Bear: 2603
- Pasaron umbral (PassedThreshold): 5245
- ConfidenceBins acumulado: 0:0, 1:0, 2:6, 3:83, 4:224, 5:669, 6:1596, 7:1944, 8:723, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 8761
- KeptAligned: 19598/20396 | KeptCounter: 11863/14955
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.501 | AvgProxCounter≈ 0.329
  - AvgDistATRAligned≈ 3.60 | AvgDistATRCounter≈ 2.12
- PreferAligned eventos: 6576 | Filtradas contra-bias: 6542

### Proximity (Pre-PreferAligned)
- Eventos: 8761
- Aligned pre: 19598/31461 | Counter pre: 11863/31461
- AvgProxAligned(pre)≈ 0.501 | AvgDistATRAligned(pre)≈ 3.60

### Proximity Drivers
- Eventos: 8761
- Alineadas: n=19598 | BaseProx≈ 0.716 | ZoneATR≈ 6.35 | SizePenalty≈ 0.957 | FinalProx≈ 0.688
- Contra-bias: n=5321 | BaseProx≈ 0.562 | ZoneATR≈ 6.05 | SizePenalty≈ 0.958 | FinalProx≈ 0.542

## Risk
- Eventos: 8401
- Accepted=5340 | RejSL=0 | RejTP=0 | RejRR=6531 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 2869 (40.6% del total)
  - Avg Score: 0.64 | Avg R:R: 1.97 | Avg DistATR: 9.41
  - Por TF: TF5=1982, TF15=887
- **P0_SWING_LITE:** 4204 (59.4% del total)
  - Avg Score: 0.39 | Avg R:R: 8.23 | Avg DistATR: 9.62
  - Por TF: TF15=2329, TF60=1875


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 890 | Unmatched: 4450
- 0-10: Wins=331 Losses=553 WR=37.4% (n=884)
- 10-15: Wins=0 Losses=6 WR=0.0% (n=6)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=331 Losses=559 WR=37.2% (n=890)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 5340 | Aligned=4058 (76.0%)
- Core≈ 0.97 | Prox≈ 0.69 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.72 | Confidence≈ 0.00
- SL_TF dist: {'15': 4029, '5': 961, '60': 342, '240': 8} | SL_Structural≈ 100.0%
- TP_TF dist: {'5': 1208, '15': 2185, '60': 1630, '240': 317} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=5169, 8-10=91, 10-12.5=60, 12.5-15=18, >15=2
- TF: 5m=961, 15m=4029, 60m=342, 240m=8, 1440m=0
- RR plan por bandas: 0-10≈ 1.73 (n=5260), 10-15≈ 1.16 (n=78)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 100204 | Zonas con Anchors: 62438
- Dir zonas (zona): Bull=34132 Bear=62224 Neutral=3848
- Resumen por ciclo (promedios): TotHZ≈ 3.5, WithAnchors≈ 2.2, DirBull≈ 1.8, DirBear≈ 1.6, DirNeutral≈ 0.2
- Razones de dirección: {'triggers-only': 36203, 'tie-bias': 3848, 'anchors+triggers': 60153}
- TF Triggers: {'15': 14778, '5': 20573}
- TF Anchors: {'60': 21793, '240': 5344, '1440': 177}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 904, 'estructura inactiva': 1}
- Cancel_BOS (diag): por acción {'BUY': 53, 'SELL': 122} | por bias {'Bullish': 122, 'Bearish': 53, 'Neutral': 0}

## CSV de Trades
- Filas: 3460 | Ejecutadas: 576 | Canceladas: 0 | Expiradas: 0
- BUY: 2501 | SELL: 1535

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 5245
- Registered: 1804
  - DEDUP_COOLDOWN: 0 | DEDUP_IDENTICAL: 0 | SKIP_CONCURRENCY: 1774
- Intentos de registro: 3578

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 68.2%
- RegRate = Registered / Intentos = 50.4%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 49.6%
- ExecRate = Ejecutadas / Registered = 31.9%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 20655 | Total candidatos: 215291 | Seleccionados: 0
- Candidatos por zona (promedio): 10.4

### Take Profit (TP)
- Zonas analizadas: 20655 | Total candidatos: 205443 | Seleccionados: 0
- Candidatos por zona (promedio): 9.9
- **Edad (barras)** - Candidatos: med=50, max=302 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.49 | Seleccionados: avg=0.00
- **Priority Candidatos**: {'P3': 205443}
- **Priority Seleccionados**: {}
- **Type Candidatos**: {'Swing': 205443}
- **Type Seleccionados**: {}
- **TF Candidatos**: {5: 82822, 15: 65818, 60: 49209, 240: 7594}
- **TF Seleccionados**: {}
- **DistATR** - Candidatos: avg=8.1 | Seleccionados: avg=0.0
- **RR** - Candidatos: avg=3.12 | Seleccionados: avg=0.00
- **Razones de selección**: {}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- KeptAligned ratio≈ 0.96.