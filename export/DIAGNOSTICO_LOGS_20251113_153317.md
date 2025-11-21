# Informe Diagnóstico de Logs - 2025-11-13 15:37:07

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251113_153317.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251113_153317.csv`

## DFM
- Eventos de evaluación: 945
- Evaluaciones Bull: 160 | Bear: 678
- Pasaron umbral (PassedThreshold): 838
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:65, 6:376, 7:348, 8:49, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2354
- KeptAligned: 4133/4133 | KeptCounter: 2805/2909
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.443 | AvgProxCounter≈ 0.231
  - AvgDistATRAligned≈ 1.51 | AvgDistATRCounter≈ 1.15
- PreferAligned eventos: 1275 | Filtradas contra-bias: 577

### Proximity (Pre-PreferAligned)
- Eventos: 2354
- Aligned pre: 4133/6938 | Counter pre: 2805/6938
- AvgProxAligned(pre)≈ 0.443 | AvgDistATRAligned(pre)≈ 1.51

### Proximity Drivers
- Eventos: 2354
- Alineadas: n=4133 | BaseProx≈ 0.752 | ZoneATR≈ 5.17 | SizePenalty≈ 0.975 | FinalProx≈ 0.734
- Contra-bias: n=2228 | BaseProx≈ 0.521 | ZoneATR≈ 4.88 | SizePenalty≈ 0.977 | FinalProx≈ 0.511

## Risk
- Eventos: 1951
- Accepted=1273 | RejSL=0 | RejTP=0 | RejRR=1233 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 310 (10.9% del total)
  - Avg Score: 0.39 | Avg R:R: 1.89 | Avg DistATR: 3.78
  - Por TF: TF5=79, TF15=231
- **P0_SWING_LITE:** 2523 (89.1% del total)
  - Avg Score: 0.58 | Avg R:R: 4.11 | Avg DistATR: 3.49
  - Por TF: TF15=601, TF60=1922


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 85 | Unmatched: 1226
- 0-10: Wins=36 Losses=49 WR=42.4% (n=85)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=36 Losses=49 WR=42.4% (n=85)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1311 | Aligned=789 (60.2%)
- Core≈ 1.00 | Prox≈ 0.67 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.10 | Confidence≈ 0.00
- SL_TF dist: {'15': 941, '60': 143, '5': 208, '240': 19} | SL_Structural≈ 100.0%
- TP_TF dist: {'15': 498, '5': 358, '60': 278, '240': 177} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1273, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=200, 15m=919, 60m=139, 240m=15, 1440m=0
- RR plan por bandas: 0-10≈ 2.08 (n=1273), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10364 | Zonas con Anchors: 10350
- Dir zonas (zona): Bull=3769 Bear=6268 Neutral=327
- Resumen por ciclo (promedios): TotHZ≈ 4.1, WithAnchors≈ 4.1, DirBull≈ 1.5, DirBear≈ 2.5, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9952, 'tie-bias': 398, 'triggers-only': 14}
- TF Triggers: {'5': 5445, '15': 4919}
- TF Anchors: {'60': 10276, '240': 5968, '1440': 536}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'score decayó a 0,27': 1, 'score decayó a 0,46': 1, 'estructura no existe': 26, 'score decayó a 0,18': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,42': 1, 'score decayó a 0,32': 1, 'score decayó a 0,34': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 203 | Ejecutadas: 37 | Canceladas: 0 | Expiradas: 0
- BUY: 71 | SELL: 169

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 838
- Registered: 107
  - DEDUP_COOLDOWN: 18 | DEDUP_IDENTICAL: 97 | SKIP_CONCURRENCY: 99
- Intentos de registro: 321

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 38.3%
- RegRate = Registered / Intentos = 33.3%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 35.8%
- Concurrency = SKIP_CONCURRENCY / Intentos = 30.8%
- ExecRate = Ejecutadas / Registered = 34.6%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5446 | Total candidatos: 42635 | Seleccionados: 0
- Candidatos por zona (promedio): 7.8

### Take Profit (TP)
- Zonas analizadas: 5340 | Total candidatos: 51633 | Seleccionados: 5340
- Candidatos por zona (promedio): 9.7
- **Edad (barras)** - Candidatos: med=40, max=195 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 51633}
- **Priority Seleccionados**: {'P3': 3626, 'NA': 1397, 'P0': 317}
- **Type Candidatos**: {'Swing': 51633}
- **Type Seleccionados**: {'P3_Swing': 3626, 'P4_Fallback': 1397, 'P0_Zone': 317}
- **TF Candidatos**: {5: 15517, 15: 14146, 60: 13436, 240: 8534}
- **TF Seleccionados**: {60: 976, 5: 1011, 15: 1258, -1: 1397, 240: 698}
- **DistATR** - Candidatos: avg=8.7 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.59 | Seleccionados: avg=1.33
- **Razones de selección**: {'BestIntelligentScore': 5340}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.