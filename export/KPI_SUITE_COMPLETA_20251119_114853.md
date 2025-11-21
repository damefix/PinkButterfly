# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-19 11:56:55  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251119_114853.csv`  
**Trades Analizados:** 7
  
**Última Operación Cerrada:** T0006 - SELL - 2025-11-06 16:00:00

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 7 |
| **Operaciones Ejecutadas (Cerradas)** | 1 |
| **Operaciones Canceladas** | 1 |
| **Operaciones Expiradas** | 5 |
| **Operaciones Pendientes** | 0 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 100.0% (1/1) |
| **Profit Factor** | 0.00 |
| **P&L Total (Puntos)** | +32.00 |
| **P&L Total (USD)** | $+160.00 |
| **Gross Profit** | $160.00 |
| **Gross Loss** | $0.00 |
| **Avg Win** | $160.00 |
| **Avg Loss** | $0.00 |
| **Avg R:R (Planned)** | 1.92 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## Trade Book (Libro de Operaciones)

### Operaciones Cerradas (1 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Date | Exit Date |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|------------|-----------|
| T0006 | SELL | 6828.00 | 6844.63 | 6796.00 | 6796.00 | [TP] TP | +32.00 | $+160.00 | 1.92 | 2025-11-05 23:00:00 | 2025-11-06 16:00:00 |


### Operaciones Canceladas (1 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
| T0007 | SELL | 6724.75 | 6749.76 | 6677.75 | 1.88 | 2025-11-14 15:15:00 | BOS contradictorio |


### Operaciones Expiradas (5 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
| T0003 | SELL | 6834.50 | 6859.16 | 6786.25 | 1.96 | 2025-11-04 08:45:00 | ADAPTIVE_MOMENTUM: 0.451ATR/bar |
| T0002 | SELL | 6834.50 | 6859.26 | 6786.25 | 1.95 | 2025-11-03 16:15:00 | STALE_DIST: 4 |
| T0001 | SELL | 6834.50 | 6859.95 | 6786.25 | 1.90 | 2025-10-31 17:00:00 | STALE_DIST: 3 |
| T0004 | SELL | 6805.50 | 6810.96 | 6796.00 | 1.74 | 2025-11-05 08:45:00 | STALE_DIST: 4 |
| T0005 | SELL | 6805.50 | 6811.12 | 6796.00 | 1.69 | 2025-11-05 22:00:00 | STALE_DIST: 3 |


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (1 total)

| Razón | Cantidad | % |
|-------|----------|---|
| BOS contradictorio | 1 | 100.0% |


### Expiraciones (5 total)

| Razón | Cantidad | % |
|-------|----------|---|
| STALE_DIST: 3 | 2 | 40.0% |
| STALE_DIST: 4 | 2 | 40.0% |
| ADAPTIVE_MOMENTUM: 0.451ATR/bar | 1 | 20.0% |




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 488 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.2000 | 27.2% |
| **Proximity** | 0.2326 | 31.6% |
| **Confluence** | 0.0600 | 8.2% |
| **Type** | 0.0133 | 1.8% |
| **Bias** | 0.2113 | 28.7% |
| **Momentum** | 0.0360 | 4.9% |
| **TOTAL (Avg Confidence)** | 0.7355 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 488
- **Señales generadas:** 488 (100.0%)
- **Señales rechazadas (WAIT):** 0 (0.0%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **Proximity**: 0.2326
2. **Bias**: 0.2113
3. **CoreScore**: 0.2000
4. **Confluence**: 0.0600
5. **Momentum**: 0.0360
6. **Type**: 0.0133

**Recomendaciones de calibración:**


---

# 🎓 CONCLUSIONES Y RECOMENDACIONES

## Diagnóstico

### Rentabilidad

- [OK] **OK:** Win Rate aceptable (100.0%)

- ⚠️ **CRÍTICO:** Profit Factor < 1.0 (sistema perdedor: 0.00)
- **Problema:** Las pérdidas superan las ganancias
- **Acción requerida:** 
  1. Revisar R:R de las operaciones
  2. Analizar cancelaciones por BOS
  3. Aumentar `MinConfidenceForEntry`


## Próximos Pasos

1. **Análisis Profundo de Operaciones Perdedoras:**
   - Activar `ShowScoringBreakdown = true`
   - Ejecutar Fast Load
   - Analizar scoring de las 0 operaciones con SL_HIT

2. **Calibración del DFM:**
   - Revisar pesos: CoreScore, Proximity, Bias, Confluence
   - Ajustar basándose en análisis científico

3. **Optimización de Gestión de Riesgo:**
   - Revisar R:R promedio: 1.92
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-11-19 11:56:55*
