# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-17 19:12:21  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251117_190654.csv`  
**Trades Analizados:** 16
  
**Última Operación Cerrada:** T0011 - SELL - 2025-11-14 12:30:00

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 16 |
| **Operaciones Ejecutadas (Cerradas)** | 2 |
| **Operaciones Canceladas** | 0 |
| **Operaciones Expiradas** | 9 |
| **Operaciones Pendientes** | 5 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 100.0% (2/2) |
| **Profit Factor** | 0.00 |
| **P&L Total (Puntos)** | +109.00 |
| **P&L Total (USD)** | $+545.00 |
| **Gross Profit** | $545.00 |
| **Gross Loss** | $0.00 |
| **Avg Win** | $272.50 |
| **Avg Loss** | $0.00 |
| **Avg R:R (Planned)** | 1.54 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## Trade Book (Libro de Operaciones)

### Operaciones Cerradas (2 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Date | Exit Date |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|------------|-----------|
| T0007 | SELL | 6769.50 | 6805.03 | 6715.00 | 6715.00 | [TP] TP | +54.50 | $+272.50 | 1.53 | 2025-11-06 18:00:00 | 2025-11-07 12:45:00 |
| T0011 | SELL | 6769.50 | 6804.63 | 6715.00 | 6715.00 | [TP] TP | +54.50 | $+272.50 | 1.55 | 2025-11-13 17:15:00 | 2025-11-14 12:30:00 |


### Operaciones Canceladas (0 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|


### Operaciones Expiradas (9 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
| T0013 | SELL | 6799.00 | 6804.69 | 6764.00 | 6.15 | 2025-11-14 21:45:00 | score decayó a 0 |
| T0006 | SELL | 6799.00 | 6803.48 | 6778.00 | 4.69 | 2025-11-05 16:15:00 | score decayó a 0 |
| T0001 | SELL | 6800.50 | 6814.44 | 6771.50 | 2.08 | 2025-10-10 17:00:00 | STALE_DIST: 4 |
| T0002 | SELL | 6768.50 | 6799.46 | 6715.00 | 1.73 | 2025-10-10 17:30:00 | score decayó a 0 |
| T0010 | BUY | 6762.75 | 6739.64 | 6801.50 | 1.68 | 2025-11-10 18:00:00 | STALE_DIST: 3 |
| T0008 | SELL | 6770.50 | 6798.38 | 6726.50 | 1.58 | 2025-11-07 16:45:00 | STALE_DIST: 5 |
| T0005 | SELL | 6769.50 | 6804.74 | 6715.00 | 1.55 | 2025-10-22 18:45:00 | STALE_DIST: 3 |
| T0009 | BUY | 6756.50 | 6724.64 | 6801.50 | 1.41 | 2025-11-10 17:00:00 | STALE_DIST: 4 |
| T0003 | BUY | 6758.00 | 6724.65 | 6801.50 | 1.30 | 2025-10-20 18:15:00 | STALE_DIST: 3 |


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (0 total)

| Razón | Cantidad | % |
|-------|----------|---|


### Expiraciones (9 total)

| Razón | Cantidad | % |
|-------|----------|---|
| score decayó a 0 | 3 | 33.3% |
| STALE_DIST: 3 | 3 | 33.3% |
| STALE_DIST: 4 | 2 | 22.2% |
| STALE_DIST: 5 | 1 | 11.1% |




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 144 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.2000 | 26.8% |
| **Proximity** | 0.2233 | 29.9% |
| **Confluence** | 0.0600 | 8.0% |
| **Type** | 0.0133 | 1.8% |
| **Bias** | 0.2157 | 28.9% |
| **Momentum** | 0.0360 | 4.8% |
| **TOTAL (Avg Confidence)** | 0.7457 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 144
- **Señales generadas:** 144 (100.0%)
- **Señales rechazadas (WAIT):** 0 (0.0%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **Proximity**: 0.2233
2. **Bias**: 0.2157
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
   - Revisar R:R promedio: 1.54
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-11-17 19:12:21*
