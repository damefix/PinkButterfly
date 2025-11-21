# Informe Diagnóstico de Logs - 2025-11-14 08:09:16

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_080542.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251114_080542.csv`

## DFM
- Eventos de evaluación: 926
- Evaluaciones Bull: 139 | Bear: 686
- Pasaron umbral (PassedThreshold): 825
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:65, 6:349, 7:349, 8:62, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2354
- KeptAligned: 4204/4204 | KeptCounter: 2718/2816
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.449 | AvgProxCounter≈ 0.222
  - AvgDistATRAligned≈ 1.56 | AvgDistATRCounter≈ 1.10
- PreferAligned eventos: 1296 | Filtradas contra-bias: 556

### Proximity (Pre-PreferAligned)
- Eventos: 2354
- Aligned pre: 4204/6922 | Counter pre: 2718/6922
- AvgProxAligned(pre)≈ 0.449 | AvgDistATRAligned(pre)≈ 1.56

### Proximity Drivers
- Eventos: 2354
- Alineadas: n=4204 | BaseProx≈ 0.750 | ZoneATR≈ 5.16 | SizePenalty≈ 0.975 | FinalProx≈ 0.732
- Contra-bias: n=2162 | BaseProx≈ 0.520 | ZoneATR≈ 4.87 | SizePenalty≈ 0.978 | FinalProx≈ 0.510

## Risk
- Eventos: 1953
- Accepted=1254 | RejSL=0 | RejTP=0 | RejRR=1292 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 329 (11.5% del total)
  - Avg Score: 0.39 | Avg R:R: 1.89 | Avg DistATR: 3.80
  - Por TF: TF5=86, TF15=243
- **P0_SWING_LITE:** 2521 (88.5% del total)
  - Avg Score: 0.59 | Avg R:R: 4.14 | Avg DistATR: 3.51
  - Por TF: TF15=573, TF60=1948


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 85 | Unmatched: 1207
- 0-10: Wins=37 Losses=48 WR=43.5% (n=85)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=37 Losses=48 WR=43.5% (n=85)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1292 | Aligned=784 (60.7%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.10 | Confidence≈ 0.00
- SL_TF dist: {'5': 211, '15': 903, '60': 158, '240': 20} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 294, '5': 343, '15': 476, '240': 179} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1254, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=203, 15m=881, 60m=154, 240m=16, 1440m=0
- RR plan por bandas: 0-10≈ 2.08 (n=1254), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10395 | Zonas con Anchors: 10381
- Dir zonas (zona): Bull=3724 Bear=6371 Neutral=300
- Resumen por ciclo (promedios): TotHZ≈ 4.2, WithAnchors≈ 4.2, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 10029, 'tie-bias': 352, 'triggers-only': 14}
- TF Triggers: {'15': 4899, '5': 5496}
- TF Anchors: {'60': 10307, '240': 6082, '1440': 177}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 27, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,42': 1, 'score decayó a 0,32': 1, 'score decayó a 0,34': 1, 'score decayó a 0,22': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 194 | Ejecutadas: 35 | Canceladas: 0 | Expiradas: 0
- BUY: 62 | SELL: 167

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 825
- Registered: 102
  - DEDUP_COOLDOWN: 12 | DEDUP_IDENTICAL: 97 | SKIP_CONCURRENCY: 100
- Intentos de registro: 311

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 37.7%
- RegRate = Registered / Intentos = 32.8%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 35.0%
- Concurrency = SKIP_CONCURRENCY / Intentos = 32.2%
- ExecRate = Ejecutadas / Registered = 34.3%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5498 | Total candidatos: 42968 | Seleccionados: 0
- Candidatos por zona (promedio): 7.8

### Take Profit (TP)
- Zonas analizadas: 5397 | Total candidatos: 50753 | Seleccionados: 5397
- Candidatos por zona (promedio): 9.4
- **Edad (barras)** - Candidatos: med=38, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 50753}
- **Priority Seleccionados**: {'P3': 3702, 'NA': 1380, 'P0': 315}
- **Type Candidatos**: {'Swing': 50753}
- **Type Seleccionados**: {'P3_Swing': 3702, 'P4_Fallback': 1380, 'P0_Zone': 315}
- **TF Candidatos**: {5: 15684, 15: 14091, 60: 13424, 240: 7554}
- **TF Seleccionados**: {60: 1031, 5: 1015, 15: 1238, -1: 1380, 240: 733}
- **DistATR** - Candidatos: avg=8.2 | Seleccionados: avg=5.6
- **RR** - Candidatos: avg=3.46 | Seleccionados: avg=1.33
- **Razones de selección**: {'BestIntelligentScore': 5397}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.