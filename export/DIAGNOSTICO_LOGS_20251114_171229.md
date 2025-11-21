# Informe Diagnóstico de Logs - 2025-11-14 17:24:34

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_171229.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251114_171229.csv`

## DFM
- Eventos de evaluación: 945
- Evaluaciones Bull: 116 | Bear: 740
- Pasaron umbral (PassedThreshold): 856
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:51, 6:346, 7:358, 8:101, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2370
- KeptAligned: 3664/3664 | KeptCounter: 2929/3062
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.383 | AvgProxCounter≈ 0.234
  - AvgDistATRAligned≈ 1.41 | AvgDistATRCounter≈ 1.29
- PreferAligned eventos: 1139 | Filtradas contra-bias: 460

### Proximity (Pre-PreferAligned)
- Eventos: 2370
- Aligned pre: 3664/6593 | Counter pre: 2929/6593
- AvgProxAligned(pre)≈ 0.383 | AvgDistATRAligned(pre)≈ 1.41

### Proximity Drivers
- Eventos: 2370
- Alineadas: n=3664 | BaseProx≈ 0.753 | ZoneATR≈ 5.04 | SizePenalty≈ 0.977 | FinalProx≈ 0.737
- Contra-bias: n=2469 | BaseProx≈ 0.492 | ZoneATR≈ 4.95 | SizePenalty≈ 0.976 | FinalProx≈ 0.482

## Risk
- Eventos: 1962
- Accepted=1284 | RejSL=0 | RejTP=0 | RejRR=1328 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 331 (11.6% del total)
  - Avg Score: 0.39 | Avg R:R: 1.91 | Avg DistATR: 3.83
  - Por TF: TF5=115, TF15=216
- **P0_SWING_LITE:** 2522 (88.4% del total)
  - Avg Score: 0.60 | Avg R:R: 4.57 | Avg DistATR: 3.51
  - Por TF: TF15=535, TF60=1987


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 82 | Unmatched: 1233
- 0-10: Wins=33 Losses=49 WR=40.2% (n=82)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=33 Losses=49 WR=40.2% (n=82)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1315 | Aligned=764 (58.1%)
- Core≈ 1.00 | Prox≈ 0.65 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.08 | Confidence≈ 0.00
- SL_TF dist: {'60': 165, '5': 171, '15': 950, '240': 29} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 238, '15': 434, '5': 320, '240': 323} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1284, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=166, 15m=931, 60m=163, 240m=24, 1440m=0
- RR plan por bandas: 0-10≈ 2.05 (n=1284), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10479 | Zonas con Anchors: 10470
- Dir zonas (zona): Bull=2902 Bear=7207 Neutral=370
- Resumen por ciclo (promedios): TotHZ≈ 4.2, WithAnchors≈ 4.2, DirBull≈ 1.2, DirBear≈ 2.9, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 9994, 'tie-bias': 476, 'triggers-only': 9}
- TF Triggers: {'5': 5524, '15': 4955}
- TF Anchors: {'60': 10373, '240': 9831, '1440': 8599}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 21, 'score decayó a 0,21': 1, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,27': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 1} | por bias {'Bullish': 1, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 189 | Ejecutadas: 47 | Canceladas: 0 | Expiradas: 0
- BUY: 54 | SELL: 182

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 856
- Registered: 99
  - DEDUP_COOLDOWN: 16 | DEDUP_IDENTICAL: 91 | SKIP_CONCURRENCY: 105
- Intentos de registro: 311

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 36.3%
- RegRate = Registered / Intentos = 31.8%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 34.4%
- Concurrency = SKIP_CONCURRENCY / Intentos = 33.8%
- ExecRate = Ejecutadas / Registered = 47.5%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5202 | Total candidatos: 47868 | Seleccionados: 0
- Candidatos por zona (promedio): 9.2

### Take Profit (TP)
- Zonas analizadas: 5132 | Total candidatos: 92425 | Seleccionados: 5132
- Candidatos por zona (promedio): 18.0
- **Edad (barras)** - Candidatos: med=35, max=182 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.51 | Seleccionados: avg=0.69
- **Priority Candidatos**: {'P3': 92425}
- **Priority Seleccionados**: {'P3': 3629, 'NA': 1271, 'P0': 232}
- **Type Candidatos**: {'Swing': 92425}
- **Type Seleccionados**: {'P3_Swing': 3629, 'P4_Fallback': 1271, 'P0_Zone': 232}
- **TF Candidatos**: {240: 34994, 60: 23930, 5: 19055, 15: 14446}
- **TF Seleccionados**: {60: 639, 15: 1120, -1: 1271, 5: 962, 240: 1140}
- **DistATR** - Candidatos: avg=13.2 | Seleccionados: avg=5.3
- **RR** - Candidatos: avg=5.82 | Seleccionados: avg=1.38
- **Razones de selección**: {'BestIntelligentScore': 5132}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.