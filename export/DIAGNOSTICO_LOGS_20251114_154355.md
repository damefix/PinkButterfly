# Informe Diagnóstico de Logs - 2025-11-14 15:50:22

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251114_154355.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251114_154355.csv`

## DFM
- Eventos de evaluación: 904
- Evaluaciones Bull: 107 | Bear: 660
- Pasaron umbral (PassedThreshold): 767
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:0, 4:0, 5:45, 6:320, 7:311, 8:91, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 2353
- KeptAligned: 3276/3276 | KeptCounter: 3250/3393
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.349 | AvgProxCounter≈ 0.254
  - AvgDistATRAligned≈ 1.27 | AvgDistATRCounter≈ 1.34
- PreferAligned eventos: 1042 | Filtradas contra-bias: 502

### Proximity (Pre-PreferAligned)
- Eventos: 2353
- Aligned pre: 3276/6526 | Counter pre: 3250/6526
- AvgProxAligned(pre)≈ 0.349 | AvgDistATRAligned(pre)≈ 1.27

### Proximity Drivers
- Eventos: 2353
- Alineadas: n=3276 | BaseProx≈ 0.756 | ZoneATR≈ 5.00 | SizePenalty≈ 0.977 | FinalProx≈ 0.740
- Contra-bias: n=2748 | BaseProx≈ 0.502 | ZoneATR≈ 4.97 | SizePenalty≈ 0.977 | FinalProx≈ 0.492

## Risk
- Eventos: 1938
- Accepted=1223 | RejSL=0 | RejTP=0 | RejRR=1276 | RejEntry=0
### TP P0 HeatZone-Based (V6.0f-FASE2)
- **P0_ANY_DIR:** 316 (10.9% del total)
  - Avg Score: 0.39 | Avg R:R: 1.94 | Avg DistATR: 3.81
  - Por TF: TF5=94, TF15=222
- **P0_SWING_LITE:** 2570 (89.1% del total)
  - Avg Score: 0.55 | Avg R:R: 4.80 | Avg DistATR: 3.65
  - Por TF: TF15=488, TF60=2082


### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 70 | Unmatched: 1193
- 0-10: Wins=27 Losses=43 WR=38.6% (n=70)
- 10-15: Wins=0 Losses=0 WR=0.0% (n=0)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=27 Losses=43 WR=38.6% (n=70)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 1263 | Aligned=665 (52.7%)
- Core≈ 1.00 | Prox≈ 0.64 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 2.06 | Confidence≈ 0.00
- SL_TF dist: {'60': 167, '5': 166, '15': 897, '240': 33} | SL_Structural≈ 100.0%
- TP_TF dist: {'60': 240, '5': 300, '15': 427, '240': 296} | TP_Structural≈ 100.0%

### SLPick por Bandas y TF
- Bandas: lt8=1223, 8-10=0, 10-12.5=0, 12.5-15=0, >15=0
- TF: 5m=160, 15m=872, 60m=164, 240m=27, 1440m=0
- RR plan por bandas: 0-10≈ 2.04 (n=1223), 10-15≈ 0.00 (n=0)

## CancelBias (EMA200@60m)
- Eventos: 0
- Distribución Bias: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## StructureFusion
- Trazas por zona: 10450 | Zonas con Anchors: 10442
- Dir zonas (zona): Bull=2841 Bear=7273 Neutral=336
- Resumen por ciclo (promedios): TotHZ≈ 4.2, WithAnchors≈ 4.2, DirBull≈ 1.1, DirBear≈ 2.9, DirNeutral≈ 0.1
- Razones de dirección: {'anchors+triggers': 10024, 'tie-bias': 418, 'triggers-only': 8}
- TF Triggers: {'5': 5512, '15': 4938}
- TF Anchors: {'60': 10369, '240': 9788, '1440': 8547}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 15, 'score decayó a 0,21': 2, 'score decayó a 0,36': 1, 'score decayó a 0,44': 1, 'score decayó a 0,22': 1}
- Cancel_BOS (diag): por acción {'BUY': 0, 'SELL': 2} | por bias {'Bullish': 2, 'Bearish': 0, 'Neutral': 0}

## CSV de Trades
- Filas: 161 | Ejecutadas: 40 | Canceladas: 0 | Expiradas: 0
- BUY: 46 | SELL: 155

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 767
- Registered: 85
  - DEDUP_COOLDOWN: 13 | DEDUP_IDENTICAL: 71 | SKIP_CONCURRENCY: 97
- Intentos de registro: 266

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 34.7%
- RegRate = Registered / Intentos = 32.0%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 31.6%
- Concurrency = SKIP_CONCURRENCY / Intentos = 36.5%
- ExecRate = Ejecutadas / Registered = 47.1%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 5042 | Total candidatos: 41058 | Seleccionados: 0
- Candidatos por zona (promedio): 8.1

### Take Profit (TP)
- Zonas analizadas: 4972 | Total candidatos: 77948 | Seleccionados: 4972
- Candidatos por zona (promedio): 15.7
- **Edad (barras)** - Candidatos: med=36, max=192 | Seleccionados: med=0, max=0
- **Score** - Candidatos: avg=0.50 | Seleccionados: avg=0.69
- **Priority Candidatos**: {'P3': 77948}
- **Priority Seleccionados**: {'P3': 3453, 'NA': 1307, 'P0': 212}
- **Type Candidatos**: {'Swing': 77948}
- **Type Seleccionados**: {'P3_Swing': 3453, 'P4_Fallback': 1307, 'P0_Zone': 212}
- **TF Candidatos**: {240: 34798, 60: 15152, 5: 14128, 15: 13870}
- **TF Seleccionados**: {60: 657, 5: 883, -1: 1307, 15: 1057, 240: 1068}
- **DistATR** - Candidatos: avg=14.6 | Seleccionados: avg=5.2
- **RR** - Candidatos: avg=6.36 | Seleccionados: avg=1.37
- **Razones de selección**: {'BestIntelligentScore': 4972}

### 🎯 Recomendaciones
- ✅ No se detectaron problemas evidentes en la selección de SL/TP.

## Observaciones automáticas
- Predominio de evaluaciones y señales SELL.
- KeptAligned ratio≈ 1.00.