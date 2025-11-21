# Informe Diagnóstico de Logs - 2025-11-14 10:17:36

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_101140.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251114_101140.csv`

## DFM
- Eventos de evaluación: 971
- Evaluaciones Bull: 143 | Bear: 727
- Pasaron umbral (PassedThreshold): 870
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:70, 6:392, 7:330, 8:78, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2370
- KeptAligned: 4285/4285 | KeptCounter: 2675/2776
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.435 | AvgProxCounter≈ 0.210
  - AvgDistATRAligned≈ 1.57 | AvgDistATRCounter≈ 1.09
- PreferAligned eventos: 1298 | Filtradas contra-bias: 469

### Proximity (Pre-PreferAligned)
- Eventos: 2370
- Aligned pre: 4285/6960 | Counter pre: 2675/6960
- AvgProxAligned(pre)≈ 0.435 | AvgDistATRAligned(pre)≈ 1.57

### Proximity Drivers
- Eventos: 2370
- Alineadas: n=4285 | BaseProx≈ 0.748 | ZoneATR≈ 5.17 | SizePenalty≈ 0.975 | FinalProx≈ 0.730
- Contra-bias: n=2206 | BaseProx≈ 0.512 | ZoneATR≈ 4.89 | SizePenalty≈ 0.977 | FinalProx≈ 0.502

## Risk
- Eventos: 1968
- Accepted=1334 | RejSL=0 | RejTP=0 | RejRR=1323 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 325 (11.0% del total)
  - Avg Score: 0.39 | Avg R:R: 1.92 | Avg DistATR: 3.85
  - Por TF: TF5=88, TF15=237
- **P0_SWING_LITE:** 2617 (89.0% del total)
  - Avg Score: 0.62 | Avg R:R: 4.20 | Avg DistATR: 3.42
  - Por TF: TF15=591, TF60=2026


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 66 | Unmatched: 1303
- 0-10: Wins=21 Losses=45 WR=31.8% (n=66)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=21 Losses=45 WR=31.8% (n=66)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1369 | Aligned=846 (61.8%)
- Core≈ 1.00 | Prox≈ 0.66 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.10 | Confidence≈ 0.00
- SL_TF dist: {'60': 208, '15': 933, '5': 203, '240': 25} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 313, '5': 351, '15': 509, '240': 196} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1334, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=198, 15m=909, 60m=207, 240m=20, 1440m=0
- RR plan por bandas: 0-10≈ 2.07 (n=1334), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10417 | Zonas con Anchors: 10407
- Dir zonas (zona): Bull=3718 Bear=6395 Neutral=304
- Resumen por ciclo (promedios): TotHZ≈ 4.2, WithAnchors≈ 4.2, DirBull≈ 1.5, DirBear≈ 2.6, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 10068, 'tie-bias': 339, 'triggers-only': 10}
- TF Triggers: {'5': 5503, '15': 4914}
- TF Anchors: {'60': 10316, '240': 6131, '1440': 213}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 27, 'score decayó a 0,21': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,27': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 193 | Ejecutadas: 41 | Canceladas: 0 | Expiradas: 0
- BUY: 61 | SELL: 173

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 870
- Registered: 101
  - DEDUP_COOLDOWN: 16 | DEDUP_IDENTICAL: 105 | SKIP_CONCURRENCY: 85
- Intentos de registro: 307

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 35.3%
- RegRate = Registered / Intentos = 32.9%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 39.4%
- Concurrency = SKIP_CONCURRENCY / Intentos = 27.7%
- ExecRate = Ejecutadas / Registered = 40.6%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5582 | Total candidatos: 50009 | Seleccionados: 0
- Candidatos por zona (promedio): 9.0

### Take Profit (TP)
- Zonas analizadas: 5483 | Total candidatos: 73890 | Seleccionados: 5483
- Candidatos por zona (promedio): 13.5
- **Edad (barras)** - Candidatos: med=35, max=182 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.53 | Seleccionados: avg=0.67
- **Priority Candidatos**: {'P3': 73890}
- **Priority Seleccionados**: {'P3': 3728, 'NA': 1454, 'P0': 301}
- **Type Candidatos**: {'Swing': 73890}
- **Type Seleccionados**: {'P3_Swing': 3728, 'P4_Fallback': 1454, 'P0_Zone': 301}
- **TF Candidatos**: {60: 22780, 5: 20483, 240: 15726, 15: 14901}
- **TF Seleccionados**: {60: 951, -1: 1454, 5: 1051, 15: 1264, 240: 763}
- **DistATR** - Candidatos: avg=9.1 | Seleccionados: avg=5.5
- **RR** - Candidatos: avg=3.92 | Seleccionados: avg=1.33
- **Razones de selección**: {'BestIntelligentScore': 5483}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.