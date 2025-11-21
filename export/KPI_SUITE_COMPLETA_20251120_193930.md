# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-20 19:48:22  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251120_193930.csv`  
**Trades Analizados:** 27
  
**Última Operación Cerrada:** T0018_2 - SELL - 2025-11-17 16:45:00

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 27 |
| **Operaciones Ejecutadas (Cerradas)** | 3 |
| **Operaciones Canceladas** | 7 |
| **Operaciones Expiradas** | 11 |
| **Operaciones Pendientes** | 6 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 66.7% (2/3) |
| **Profit Factor** | 2.77 |
| **P&L Total (Puntos)** | +31.00 |
| **P&L Total (USD)** | $+155.00 |
| **Gross Profit** | $242.50 |
| **Gross Loss** | $87.50 |
| **Avg Win** | $121.25 |
| **Avg Loss** | $87.50 |
| **Avg R:R (Planned)** | 1.76 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## Trade Book (Libro de Operaciones)

### Operaciones Cerradas (3 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Date | Exit Date |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|------------|-----------|
| T0016 | SELL | 6757.50 | 6775.00 | 6723.25 | 6775.00 | [SL] SL | -17.50 | $-87.50 | 1.96 | 2025-11-14 16:00:00 | 2025-11-14 17:30:00 |
| T0018 | SELL | 6770.50 | 6788.74 | 6740.25 | 6749.75 | [TP] TP | +18.25 | $+91.25 | 1.66 | 2025-11-17 12:00:00 | 2025-11-17 14:15:00 |
| T0018_2 | SELL | 6770.50 | 6788.74 | 6740.25 | 6740.25 | [TP] TP | +30.25 | $+151.25 | 1.66 | 2025-11-17 12:00:00 | 2025-11-17 16:45:00 |


### Operaciones Canceladas (7 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
| T0010 | SELL | 6875.75 | 6886.93 | 6830.00 | 4.09 | 2025-11-13 03:45:00 | BOS contradictorio |
| T0009 | SELL | 6879.25 | 6889.77 | 6851.25 | 2.66 | 2025-11-13 02:30:00 | BOS contradictorio |
| T0014 | SELL | 6854.75 | 6867.09 | 6830.00 | 2.01 | 2025-11-13 15:45:00 | BOS contradictorio |
| T0007 | SELL | 6873.00 | 6896.49 | 6830.00 | 1.83 | 2025-11-12 18:15:00 | BOS contradictorio |
| T0008 | SELL | 6865.75 | 6886.38 | 6830.00 | 1.73 | 2025-11-13 02:15:00 | BOS contradictorio |
| T0012 | SELL | 6879.25 | 6890.63 | 6861.25 | 1.58 | 2025-11-13 05:45:00 | BOS contradictorio |
| T0013 | SELL | 6879.25 | 6890.70 | 6861.25 | 1.57 | 2025-11-13 10:45:00 | BOS contradictorio |


### Operaciones Expiradas (11 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
| T0003 | SELL | 6880.25 | 6886.89 | 6858.00 | 3.35 | 2025-10-31 02:15:00 | STALE_DIST: 3 |
| T0002 | SELL | 6854.75 | 6867.50 | 6812.75 | 3.29 | 2025-10-31 01:15:00 | STALE_DIST: 3 |
| T0005 | SELL | 6880.25 | 6887.02 | 6858.00 | 3.29 | 2025-11-04 01:45:00 | ADAPTIVE_DEPARTURE: 2.51x |
| T0006 | SELL | 6854.75 | 6867.48 | 6813.50 | 3.24 | 2025-11-04 16:45:00 | ADAPTIVE_DEPARTURE: 2.05x |
| T0004 | SELL | 6854.75 | 6868.55 | 6812.75 | 3.04 | 2025-10-31 21:15:00 | ADAPTIVE_DEPARTURE: 2.19x |
| T0015 | SELL | 6755.50 | 6773.71 | 6711.75 | 2.40 | 2025-11-14 15:45:00 | STALE_DIST: 3 |
| T0023 | SELL | 6693.75 | 6706.46 | 6667.50 | 2.07 | 2025-11-18 13:00:00 | STALE_DIST: 3 |
| T0021 | SELL | 6685.75 | 6709.69 | 6638.50 | 1.97 | 2025-11-18 11:00:00 | ADAPTIVE_MOMENTUM: 0.301ATR/bar |
| T0019 | SELL | 6782.25 | 6804.46 | 6740.25 | 1.89 | 2025-11-17 18:45:00 | ADAPTIVE_MOMENTUM: 0.798ATR/bar |
| T0026 | SELL | 6710.50 | 6736.25 | 6665.50 | 1.75 | 2025-11-18 21:15:00 | STALE_DIST: 3 |


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (7 total)

| Razón | Cantidad | % |
|-------|----------|---|
| BOS contradictorio | 7 | 100.0% |


### Expiraciones (11 total)

| Razón | Cantidad | % |
|-------|----------|---|
| STALE_DIST: 3 | 5 | 45.5% |
| ADAPTIVE_DEPARTURE: 2.19x | 1 | 9.1% |
| ADAPTIVE_DEPARTURE: 2.51x | 1 | 9.1% |
| ADAPTIVE_DEPARTURE: 2.05x | 1 | 9.1% |
| ADAPTIVE_MOMENTUM: 0.798ATR/bar | 1 | 9.1% |
| ADAPTIVE_MOMENTUM: 0.301ATR/bar | 1 | 9.1% |
| score decayó a 0 | 1 | 9.1% |




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 357 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.2000 | 27.5% |
| **Proximity** | 0.2315 | 31.9% |
| **Confluence** | 0.0600 | 8.3% |
| **Type** | 0.0133 | 1.8% |
| **Bias** | 0.1863 | 25.7% |
| **Momentum** | 0.0360 | 5.0% |
| **TOTAL (Avg Confidence)** | 0.7262 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 357
- **Señales generadas:** 357 (100.0%)
- **Señales rechazadas (WAIT):** 0 (0.0%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **Proximity**: 0.2315
2. **CoreScore**: 0.2000
3. **Bias**: 0.1863
4. **Confluence**: 0.0600
5. **Momentum**: 0.0360
6. **Type**: 0.0133

**Recomendaciones de calibración:**


---

# 🎓 CONCLUSIONES Y RECOMENDACIONES

## Diagnóstico

### Rentabilidad

- [OK] **OK:** Win Rate aceptable (66.7%)

- [OK] **OK:** Profit Factor aceptable (2.77)


## Próximos Pasos

1. **Análisis Profundo de Operaciones Perdedoras:**
   - Activar `ShowScoringBreakdown = true`
   - Ejecutar Fast Load
   - Analizar scoring de las 1 operaciones con SL_HIT

2. **Calibración del DFM:**
   - Revisar pesos: CoreScore, Proximity, Bias, Confluence
   - Ajustar basándose en análisis científico

3. **Optimización de Gestión de Riesgo:**
   - Revisar R:R promedio: 1.76
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-11-20 19:48:22*
