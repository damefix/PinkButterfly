# Informe Diagnóstico de Logs - 2025-11-04 21:11:00

- Log: `..\..\NinjaTrader 8\PinkButterfly\logs\backtest_20251104_210441.log`
- CSV: `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251104_210441.csv`

## DFM
- Eventos de evaluación: 4595
- Evaluaciones Bull: 8384 | Bear: 4434
- Pasaron umbral (PassedThreshold): 10651
- ConfidenceBins acumulado: 0:0, 1:0, 2:0, 3:216, 4:1008, 5:2416, 6:4347, 7:4798, 8:33, 9:0

### DFM - Contribuciones promedio (desde logs)
- Sin muestras de desglose de componentes en el log.

## Proximity
- Eventos: 4998
- KeptAligned: 16476/19315 | KeptCounter: 3301/5953
- Promedios reportados (media de promedios por evento):
  - AvgProxAligned≈ 0.509 | AvgProxCounter≈ 0.151
  - AvgDistATRAligned≈ 3.35 | AvgDistATRCounter≈ 0.74
- PreferAligned eventos: 4286 | Filtradas contra-bias: 1590

### Proximity (Pre-PreferAligned)
- Eventos: 4998
- Aligned pre: 16476/19777 | Counter pre: 3301/19777
- AvgProxAligned(pre)≈ 0.509 | AvgDistATRAligned(pre)≈ 3.35

### Proximity Drivers
- Eventos: 4998
- Alineadas: n=16476 | BaseProx≈ 0.622 | ZoneATR≈ 6.17 | SizePenalty≈ 0.952 | FinalProx≈ 0.598
- Contra-bias: n=1711 | BaseProx≈ 0.529 | ZoneATR≈ 5.46 | SizePenalty≈ 0.963 | FinalProx≈ 0.512

## Risk
- Eventos: 4868
- Accepted=12856 | RejSL=4665 | RejTP=0 | RejRR=0 | RejEntry=0
### Risk Drivers (Rechazos por SL)
- Alineadas: n=4258 | SLDistATR≈ 17.79 | Prox≈ 0.000 | Core≈ 0.000
- Contra-bias: n=407 | SLDistATR≈ 18.41 | Prox≈ 0.000 | Core≈ 0.000
- HistSL Aligned 0-10:0,10-15:0,15-20:3579,20-25:564,25+:115
- HistSL Counter 0-10:0,10-15:0,15-20:324,20-25:58,25+:25

### WR vs SLDistATR (aceptaciones)
- Matched aceptaciones con CSV: 36 | Unmatched: 12820
- 0-10: Wins=2 Losses=4 WR=33.3% (n=6)
- 10-15: Wins=1 Losses=29 WR=3.3% (n=30)
- 15-20: Wins=0 Losses=0 WR=0.0% (n=0)
- 20-25: Wins=0 Losses=0 WR=0.0% (n=0)
- 25+: Wins=0 Losses=0 WR=0.0% (n=0)

### WR vs Confidence (aceptaciones)
- 0.50-0.60: Wins=3 Losses=33 WR=8.3% (n=36)
- 0.60-0.70: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.70-0.80: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.80-0.90: Wins=0 Losses=0 WR=0.0% (n=0)
- 0.90-1.00: Wins=0 Losses=0 WR=0.0% (n=0)

### Análisis de Calidad de Zonas Aceptadas
- Muestras: 12856 | Aligned=11558 (89.9%)
- Core≈ 1.00 | Prox≈ 0.65 | ConfC≈ 0.00 | ConfScore≈ 0.00 | RR≈ 1.38 | Confidence≈ 0.00
- SL_TF dist: {'-1': 293, '5': 310, '60': 8101, '1440': 1467, '15': 909, '240': 1776} | SL_Structural≈ 97.7%
- TP_TF dist: {'-1': 6384, '5': 123, '60': 232, '240': 1112, '1440': 5005} | TP_Structural≈ 50.3%

### SLPick por Bandas y TF
- Bandas: lt8=1996, 8-10=1817, 10-12.5=4590, 12.5-15=4453, >15=0
- TF: 5m=310, 15m=909, 60m=8101, 240m=1776, 1440m=1467
- RR plan por bandas: 0-10≈ 1.66 (n=3813), 10-15≈ 1.26 (n=9043)

## CancelBias (EMA200@60m)
- Eventos: 58
- Distribución Bias: {'Bullish': 44, 'Bearish': 14, 'Neutral': 0}
- Coherencia (Close>EMA): 44/58 (75.9%)

## StructureFusion
- Trazas por zona: 25268 | Zonas con Anchors: 25247
- Dir zonas (zona): Bull=15300 Bear=9968 Neutral=0
- Resumen por ciclo (promedios): TotHZ≈ 5.1, WithAnchors≈ 5.0, DirBull≈ 3.1, DirBear≈ 2.0, DirNeutral≈ 0.0
- Razones de dirección: {'tie-bias': 876, 'anchors+triggers': 24372, 'triggers-only': 20}
- TF Triggers: {'5': 10982, '15': 14286}
- TF Anchors: {'60': 25162, '240': 24775, '1440': 20813}

## ContextManager Bias
- Eventos: 0 | Distribución: {'Bullish': 0, 'Bearish': 0, 'Neutral': 0}

### Context (Diagnóstico)
- Eventos: 5000 | Distribución: {'Bullish': 3748, 'Bearish': 1252, 'Neutral': 0}
- Strength promedio: 1.00 | Close60>Avg200: 3748/5000

## TradeManager - Razones (desde log)
- Expiraciones: {'estructura no existe': 9}
- Cancel_BOS (diag): por acción {'BUY': 1, 'SELL': 0} | por bias {'Bullish': 0, 'Bearish': 1, 'Neutral': 0}

## CSV de Trades
- Filas: 57 | Ejecutadas: 10 | Canceladas: 0 | Expiradas: 0
- BUY: 42 | SELL: 25

## 📊 Embudo de Señales (Funnel)
- DFM Señales (PassedThreshold): 10651
- Registered: 29
  - DEDUP_COOLDOWN: 8 | DEDUP_IDENTICAL: 4 | SKIP_CONCURRENCY: 3626
- Intentos de registro: 3667

### TRADE DEDUP - Zonas y Persistencia
- Top 10 Zonas más deduplicadas (IDENTICAL):

| ZoneID | Duplicados | % del Total | Key Típica |
|--------|------------:|------------:|-----------:|
| 459c3a15-c7af-4a2f-849e-68f7236e473d | 2 | 50.0% | 6757,25/6738,65/6775,85 |
| 9e986a49-f21b-45d5-b162-8f98c1f5982a | 1 | 25.0% | 6830,50/6782,94/6897,00 |
| ae1a08ee-9788-4126-b20b-68327debd869 | 1 | 25.0% | 6830,25/6782,93/6897,00 |

- Distribución de DeltaBars (IDENTICAL):

| DeltaBars | Cantidad | % |
|-----------|---------:|---:|
| 0 | 0 | 0.0% |
| 1 | 1 | 25.0% |
| 2-5 | 3 | 75.0% |
| 6-12 | 0 | 0.0% |
| >12 | 0 | 0.0% |

- IDENTICAL por Acción: {'BUY': 4, 'SELL': 0}
- IDENTICAL por DomTF: {'240': 2, '5': 2}
- COOLDOWN por Zona (top 5): {'97fadbfb-b7d6-4a63-88e0-558f21b774fa': 6, '7b5999cf-56e7-4ee8-a5bd-66a7cc067c98': 2}

### Ratios del Funnel
- Coverage = Intentos / PassedThreshold = 34.4%
- RegRate = Registered / Intentos = 0.8%
- Dedup Rate = (COOLDOWN+IDENTICAL) / Intentos = 0.3%
- Concurrency = SKIP_CONCURRENCY / Intentos = 98.9%
- ExecRate = Ejecutadas / Registered = 34.5%

## Análisis Post-Mortem: SL/TP
### Stop Loss (SL)
- Zonas analizadas: 18053 | Total candidatos: 453646 | Seleccionados: 17832
- Candidatos por zona (promedio): 25.1
- **Edad (barras)** - Candidatos: med=39, max=150 | Seleccionados: med=40, max=150
- **Score** - Candidatos: avg=0.46 | Seleccionados: avg=0.44
- **TF Candidatos**: {240: 157550, 60: 130562, 15: 73355, 1440: 47740, 5: 44439}
- **TF Seleccionados**: {5: 408, 60: 11469, 1440: 2188, 15: 1471, 240: 2296}
- **DistATR** - Candidatos: avg=22.0 | Seleccionados: avg=10.0
- **Razones de selección**: {'Fallback<15': 3562, 'InBand[8,15]_TFPreference': 14270}
- **En banda [10,15] ATR**: 76340/453646 (16.8%)

### Take Profit (TP)
- Zonas analizadas: 18187 | Total candidatos: 244808 | Seleccionados: 18187
- Candidatos por zona (promedio): 13.5
- **Edad (barras)** - Candidatos: med=33, max=150 | Seleccionados: med=0, max=71
- **Score** - Candidatos: avg=0.52 | Seleccionados: avg=0.35
- **Priority Candidatos**: {'P3': 244808}
- **Priority Seleccionados**: {'P4_Fallback': 9817, 'P3': 8370}
- **Type Candidatos**: {'Swing': 244808}
- **Type Seleccionados**: {'Calculated': 9817, 'Swing': 8370}
- **TF Candidatos**: {15: 54732, 5: 53158, 60: 51645, 240: 51292, 1440: 33981}
- **TF Seleccionados**: {-1: 9817, 5: 129, 60: 241, 240: 1346, 1440: 6654}
- **DistATR** - Candidatos: avg=12.1 | Seleccionados: avg=16.5
- **RR** - Candidatos: avg=1.04 | Seleccionados: avg=1.34
- **Razones de selección**: {'NoStructuralTarget': 9817, 'SwingP3_ANYTF_RR>=Min_Dist>=8': 129, 'SwingP3_TF>=60_RR>=Min_Dist>=8': 8241}

### 🎯 Recomendaciones
- ⚠️ SL: 64% tienen score < 0.5. Considerar umbral mínimo de calidad.
- ⚠️ TP: 54% son fallback (sin estructura válida). Problema de calidad de estructuras.

## Observaciones automáticas
- KeptAligned ratio≈ 0.85.