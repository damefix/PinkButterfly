# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-21 08:58:01  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251121_085042.csv`  
**Trades Analizados:** 10
  
**Última Operación Cerrada:** T0009 - SELL - 2025-11-18 21:30:00

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 10 |
| **Operaciones Ejecutadas (Cerradas)** | 6 |
| **Operaciones Canceladas** | 0 |
| **Operaciones Expiradas** | 3 |
| **Operaciones Pendientes** | 1 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 83.3% (5/6) |
| **Profit Factor** | 12.43 |
| **P&L Total (Puntos)** | +145.75 |
| **P&L Total (USD)** | $+728.75 |
| **Gross Profit** | $792.50 |
| **Gross Loss** | $63.75 |
| **Avg Win** | $158.50 |
| **Avg Loss** | $63.75 |
| **Avg R:R (Planned)** | 2.12 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## Trade Book (Libro de Operaciones)

### Operaciones Cerradas (6 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Date | Exit Date |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|------------|-----------|
| T0001 | SELL | 6896.50 | 6909.25 | 6870.75 | 6909.25 | [SL] SL | -12.75 | $-63.75 | 2.02 | 2025-10-31 01:15:00 | 2025-10-31 13:00:00 |
| T0002 | SELL | 6896.50 | 6909.04 | 6870.75 | 6870.75 | [TP] TP | +25.75 | $+128.75 | 2.05 | 2025-11-03 00:15:00 | 2025-11-03 16:15:00 |
| T0004 | SELL | 6873.00 | 6896.49 | 6830.00 | 6830.00 | [TP] TP | +43.00 | $+215.00 | 1.83 | 2025-11-12 18:15:00 | 2025-11-13 15:45:00 |
| T0006 | SELL | 6749.50 | 6759.99 | 6730.75 | 6730.75 | [TP] TP | +18.75 | $+93.75 | 1.79 | 2025-11-14 08:30:00 | 2025-11-14 12:15:00 |
| T0007 | SELL | 6754.00 | 6780.91 | 6707.75 | 6707.75 | [TP] TP | +46.25 | $+231.25 | 1.72 | 2025-11-17 16:00:00 | 2025-11-17 20:00:00 |
| T0009 | SELL | 6680.25 | 6687.68 | 6655.50 | 6655.50 | [TP] TP | +24.75 | $+123.75 | 3.33 | 2025-11-18 15:45:00 | 2025-11-18 21:30:00 |


### Operaciones Canceladas (0 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|


### Operaciones Expiradas (3 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
| T0009 | SELL | 6680.25 | 6687.68 | 6655.50 | 3.33 | 2025-11-18 15:45:00 | score decayó a 0 |
| T0003 | SELL | 6889.00 | 6896.39 | 6870.75 | 2.47 | 2025-11-04 00:30:00 | STALE_DIST: 3 |
| T0005 | SELL | 6762.00 | 6778.54 | 6723.25 | 2.34 | 2025-11-14 00:15:00 | ADAPTIVE_DEPARTURE: 2.12x |


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (0 total)

| Razón | Cantidad | % |
|-------|----------|---|


### Expiraciones (3 total)

| Razón | Cantidad | % |
|-------|----------|---|
| STALE_DIST: 3 | 1 | 33.3% |
| ADAPTIVE_DEPARTURE: 2.12x | 1 | 33.3% |
| score decayó a 0 | 1 | 33.3% |




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 225 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.2000 | 27.0% |
| **Proximity** | 0.2354 | 31.8% |
| **Confluence** | 0.0600 | 8.1% |
| **Type** | 0.0133 | 1.8% |
| **Bias** | 0.1976 | 26.7% |
| **Momentum** | 0.0360 | 4.9% |
| **TOTAL (Avg Confidence)** | 0.7404 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 225
- **Señales generadas:** 225 (100.0%)
- **Señales rechazadas (WAIT):** 0 (0.0%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **Proximity**: 0.2354
2. **CoreScore**: 0.2000
3. **Bias**: 0.1976
4. **Confluence**: 0.0600
5. **Momentum**: 0.0360
6. **Type**: 0.0133

**Recomendaciones de calibración:**


---

# 🎓 CONCLUSIONES Y RECOMENDACIONES

## Diagnóstico

### Rentabilidad

- [OK] **OK:** Win Rate aceptable (83.3%)

- [OK] **OK:** Profit Factor aceptable (12.43)


## Próximos Pasos

1. **Análisis Profundo de Operaciones Perdedoras:**
   - Activar `ShowScoringBreakdown = true`
   - Ejecutar Fast Load
   - Analizar scoring de las 1 operaciones con SL_HIT

2. **Calibración del DFM:**
   - Revisar pesos: CoreScore, Proximity, Bias, Confluence
   - Ajustar basándose en análisis científico

3. **Optimización de Gestión de Riesgo:**
   - Revisar R:R promedio: 2.12
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-11-21 08:58:01*
