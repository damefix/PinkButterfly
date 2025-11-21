# Informe Diagnóstico de Logs - 2025-11-14 08:42:31

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_084018.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251114_084018.csv`

## DFM
- Eventos de evaluación: 937
- Evaluaciones Bull: 136 | Bear: 696
- Pasaron umbral (PassedThreshold): 832
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:66, 6:360, 7:340, 8:66, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2355
- KeptAligned: 4214/4214 | KeptCounter: 2722/2820
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.449 | AvgProxCounter≈ 0.226
  - AvgDistATRAligned≈ 1.56 | AvgDistATRCounter≈ 1.11
- PreferAligned eventos: 1298 | Filtradas contra-bias: 558

### Proximity (Pre-PreferAligned)
- Eventos: 2355
- Aligned pre: 4214/6936 | Counter pre: 2722/6936
- AvgProxAligned(pre)≈ 0.449 | AvgDistATRAligned(pre)≈ 1.56

### Proximity Drivers
- Eventos: 2355
- Alineadas: n=4214 | BaseProx≈ 0.750 | ZoneATR≈ 5.16 | SizePenalty≈ 0.975 | FinalProx≈ 0.732
- Contra-bias: n=2164 | BaseProx≈ 0.520 | ZoneATR≈ 4.87 | SizePenalty≈ 0.977 | FinalProx≈ 0.510

## Risk
- Eventos: 1956
- Accepted=1262 | RejSL=0 | RejTP=0 | RejRR=1279 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 331 (11.6% del total)
  - Avg Score: 0.39 | Avg R:R: 1.90 | Avg DistATR: 3.81
  - Por TF: TF5=87, TF15=244
- **P0_SWING_LITE:** 2519 (88.4% del total)
  - Avg Score: 0.59 | Avg R:R: 4.10 | Avg DistATR: 3.50
  - Por TF: TF15=574, TF60=1945


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 87 | Unmatched: 1214
- 0-10: Wins=43 Losses=44 WR=49.4% (n=87)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=43 Losses=44 WR=49.4% (n=87)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1301 | Aligned=794 (61.0%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.09 | Confidence≈ 0.00
- SL_TF dist: {'15': 918, '5': 202, '60': 161, '240': 20} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 298, '15': 485, '5': 341, '240': 177} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1262, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=194, 15m=895, 60m=157, 240m=16, 1440m=0
- RR plan por bandas: 0-10≈ 2.07 (n=1262), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10404 | Zonas con Anchors: 10390
- Dir zonas (zona): Bull=3710 Bear=6392 Neutral=302
- Resumen por ciclo (promedios): TotHZ≈ 4.2, WithAnchors≈ 4.2, DirBull≈ 1.5, DirBear≈ 2.6, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 10039, 'tie-bias': 351, 'triggers-only': 14}
- TF Triggers: {'15': 4905, '5': 5499}
- TF Anchors: {'60': 10316, '240': 6094, '1440': 185}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 26, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,42': 1, 'score decayó a 0,32': 1, 'score decayó a 0,34': 1, 'score decayó a 0,22': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 192 | Ejecutadas: 35 | Canceladas: 0 | Expiradas: 0
- BUY: 58 | SELL: 169

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 832
- Registered: 101
  - DEDUP_COOLDOWN: 12 | DEDUP_IDENTICAL: 100 | SKIP_CONCURRENCY: 102
- Intentos de registro: 315

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 37.9%
- RegRate = Registered / Intentos = 32.1%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 35.6%
- Concurrency = SKIP_CONCURRENCY / Intentos = 32.4%
- ExecRate = Ejecutadas / Registered = 34.7%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5495 | Total candidatos: 43047 | Seleccionados: 0
- Candidatos por zona (promedio): 7.8

### Take Profit (TP)
- Zonas analizadas: 5392 | Total candidatos: 50709 | Seleccionados: 5392
- Candidatos por zona (promedio): 9.4
- **Edad (barras)** - Candidatos: med=38, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 50709}
- **Priority Seleccionados**: {'P3': 3687, 'NA': 1387, 'P0': 318}
- **Type Candidatos**: {'Swing': 50709}
- **Type Seleccionados**: {'P3_Swing': 3687, 'P4_Fallback': 1387, 'P0_Zone': 318}
- **TF Candidatos**: {5: 15622, 15: 14075, 60: 13427, 240: 7585}
- **TF Seleccionados**: {60: 1014, -1: 1387, 15: 1239, 5: 1016, 240: 736}
- **DistATR** - Candidatos: avg=8.3 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.44 | Seleccionados: avg=1.32
- **Razones de selección**: {'BestIntelligentScore': 5392}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.