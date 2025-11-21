# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-19 15:43:38  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_153900.csv`  
**Trades Analizados:** 39
  
**Última Operación Cerrada:** T0018 - SELL - 2025-11-14 08:30:00

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 39 |
| **Operaciones Ejecutadas (Cerradas)** | 4 |
| **Operaciones Canceladas** | 5 |
| **Operaciones Expiradas** | 10 |
| **Operaciones Pendientes** | 20 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 50.0% (2/4) |
| **Profit Factor** | 1.99 |
| **P&L Total (Puntos)** | +20.69 |
| **P&L Total (USD)** | $+103.42 |
| **Gross Profit** | $207.50 |
| **Gross Loss** | $104.08 |
| **Avg Win** | $103.75 |
| **Avg Loss** | $52.04 |
| **Avg R:R (Planned)** | 2.35 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## Trade Book (Libro de Operaciones)

### Operaciones Cerradas (4 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Date | Exit Date |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|------------|-----------|
| T0008 | SELL | 6812.25 | 6823.49 | 6786.25 | 6823.49 | [SL] SL | -11.24 | $-56.21 | 2.31 | 2025-11-05 13:45:00 | 2025-11-05 16:15:00 |
| T0009 | SELL | 6829.25 | 6836.38 | 6816.25 | 6816.25 | [TP] TP | +13.00 | $+65.00 | 1.82 | 2025-11-05 23:00:00 | 2025-11-06 01:30:00 |
| T0012 | SELL | 6814.25 | 6823.83 | 6795.00 | 6823.83 | [SL] SL | -9.57 | $-47.87 | 2.01 | 2025-11-06 09:00:00 | 2025-11-06 10:30:00 |
| T0018 | SELL | 6762.00 | 6770.80 | 6733.50 | 6733.50 | [TP] TP | +28.50 | $+142.50 | 3.24 | 2025-11-14 04:30:00 | 2025-11-14 08:30:00 |


### Operaciones Canceladas (5 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
| T0039 | SELL | 6676.25 | 6688.23 | 6642.75 | 2.80 | 2025-11-18 18:30:00 | BOS contradictorio |
| T0015 | SELL | 6881.50 | 6894.51 | 6851.25 | 2.33 | 2025-11-13 16:30:00 | BOS contradictorio |
| T0025 | SELL | 6686.00 | 6699.36 | 6655.50 | 2.28 | 2025-11-18 03:45:00 | BOS contradictorio |
| T0038 | SELL | 6680.25 | 6687.03 | 6665.25 | 2.21 | 2025-11-18 15:30:00 | BOS contradictorio |
| T0036 | SELL | 6680.25 | 6687.23 | 6665.25 | 2.15 | 2025-11-18 14:00:00 | BOS contradictorio |


### Operaciones Expiradas (10 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
| T0013 | SELL | 6846.75 | 6859.84 | 6812.50 | 2.62 | 2025-11-06 16:30:00 | STALE_DIST: 4 |
| T0003 | SELL | 6836.75 | 6858.97 | 6786.25 | 2.27 | 2025-11-04 08:00:00 | ADAPTIVE_MOMENTUM: 0.685ATR/bar |
| T0004 | SELL | 6836.75 | 6858.99 | 6786.25 | 2.27 | 2025-11-04 14:30:00 | STALE_DIST: 4 |
| T0002 | SELL | 6836.75 | 6859.13 | 6786.25 | 2.26 | 2025-11-03 16:00:00 | STALE_DIST: 4 |
| T0001 | SELL | 6836.75 | 6860.19 | 6786.25 | 2.15 | 2025-10-31 17:45:00 | STALE_DIST: 3 |
| T0031 | SELL | 6708.75 | 6717.93 | 6689.00 | 2.15 | 2025-11-18 09:45:00 | score decayó a 0 |
| T0005 | SELL | 6836.75 | 6851.82 | 6808.00 | 1.91 | 2025-11-04 20:45:00 | STALE_DIST: 3 |
| T0023 | SELL | 6744.50 | 6760.75 | 6713.50 | 1.91 | 2025-11-14 12:45:00 | estructura no existe |
| T0006 | SELL | 6827.75 | 6845.69 | 6796.00 | 1.77 | 2025-11-05 01:00:00 | STALE_DIST: 3 |
| T0011 | SELL | 6836.75 | 6851.30 | 6811.00 | 1.77 | 2025-11-06 03:45:00 | STALE_DIST: 3 |


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (5 total)

| Razón | Cantidad | % |
|-------|----------|---|
| BOS contradictorio | 5 | 100.0% |


### Expiraciones (10 total)

| Razón | Cantidad | % |
|-------|----------|---|
| STALE_DIST: 3 | 4 | 40.0% |
| STALE_DIST: 4 | 3 | 30.0% |
| ADAPTIVE_MOMENTUM: 0.685ATR/bar | 1 | 10.0% |
| estructura no existe | 1 | 10.0% |
| score decayó a 0 | 1 | 10.0% |




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 437 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.2000 | 27.5% |
| **Proximity** | 0.2244 | 30.9% |
| **Confluence** | 0.0600 | 8.3% |
| **Type** | 0.0133 | 1.8% |
| **Bias** | 0.2093 | 28.8% |
| **Momentum** | 0.0360 | 5.0% |
| **TOTAL (Avg Confidence)** | 0.7264 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 437
- **Señales generadas:** 437 (100.0%)
- **Señales rechazadas (WAIT):** 0 (0.0%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **Proximity**: 0.2244
2. **Bias**: 0.2093
3. **CoreScore**: 0.2000
4. **Confluence**: 0.0600
5. **Momentum**: 0.0360
6. **Type**: 0.0133

**Recomendaciones de calibración:**


---

# 🎓 CONCLUSIONES Y RECOMENDACIONES

## Diagnóstico

### Rentabilidad

- [OK] **OK:** Win Rate aceptable (50.0%)

- [OK] **OK:** Profit Factor aceptable (1.99)


## Próximos Pasos

1. **Análisis Profundo de Operaciones Perdedoras:**
   - Activar `ShowScoringBreakdown = true`
   - Ejecutar Fast Load
   - Analizar scoring de las 2 operaciones con SL_HIT

2. **Calibración del DFM:**
   - Revisar pesos: CoreScore, Proximity, Bias, Confluence
   - Ajustar basándose en análisis científico

3. **Optimización de Gestión de Riesgo:**
   - Revisar R:R promedio: 2.35
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-11-19 15:43:38*
