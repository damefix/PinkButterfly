# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-17 17:28:54  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251117_171422.csv`  
**Trades Analizados:** 10
  
**Última Operación Cerrada:** T0006 - SELL - 2025-11-07 12:15:00

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 10 |
| **Operaciones Ejecutadas (Cerradas)** | 1 |
| **Operaciones Canceladas** | 0 |
| **Operaciones Expiradas** | 8 |
| **Operaciones Pendientes** | 1 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 100.0% (1/1) |
| **Profit Factor** | 0.00 |
| **P&L Total (Puntos)** | +51.75 |
| **P&L Total (USD)** | $+258.75 |
| **Gross Profit** | $258.75 |
| **Gross Loss** | $0.00 |
| **Avg Win** | $258.75 |
| **Avg Loss** | $0.00 |
| **Avg R:R (Planned)** | 2.53 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## Trade Book (Libro de Operaciones)

### Operaciones Cerradas (1 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Date | Exit Date |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|------------|-----------|
| T0006 | SELL | 6778.25 | 6798.73 | 6726.50 | 6726.50 | [TP] TP | +51.75 | $+258.75 | 2.53 | 2025-11-06 20:45:00 | 2025-11-07 12:15:00 |


### Operaciones Canceladas (0 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|


### Operaciones Expiradas (8 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
| T0008 | SELL | 6799.00 | 6804.60 | 6764.00 | 6.25 | 2025-11-13 20:00:00 | STALE_TIME: 17>16bars |
| T0009 | SELL | 6799.00 | 6804.89 | 6764.00 | 5.95 | 2025-11-14 16:15:00 | STALE_DIST: 3 |
| T0002 | SELL | 6799.00 | 6803.41 | 6778.00 | 4.76 | 2025-10-07 18:30:00 | STALE_DIST: 3 |
| T0006 | SELL | 6778.25 | 6798.73 | 6726.50 | 2.53 | 2025-11-06 20:45:00 | STALE_DIST: 3 |
| T0007 | BUY | 6757.50 | 6739.96 | 6801.50 | 2.51 | 2025-11-10 17:15:00 | STALE_DIST: 3 |
| T0001 | SELL | 6801.50 | 6814.01 | 6778.00 | 1.88 | 2025-10-07 18:00:00 | STALE_DIST: 5 |
| T0003 | SELL | 6801.50 | 6814.29 | 6778.00 | 1.84 | 2025-10-10 17:00:00 | score decayó a 0 |
| T0004 | BUY | 6757.50 | 6723.84 | 6801.50 | 1.31 | 2025-10-24 17:45:00 | STALE_DIST: 5 |


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (0 total)

| Razón | Cantidad | % |
|-------|----------|---|


### Expiraciones (8 total)

| Razón | Cantidad | % |
|-------|----------|---|
| STALE_DIST: 3 | 4 | 50.0% |
| STALE_DIST: 5 | 2 | 25.0% |
| score decayó a 0 | 1 | 12.5% |
| STALE_TIME: 17>16bars | 1 | 12.5% |




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 163 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.2000 | 27.5% |
| **Proximity** | 0.2070 | 28.5% |
| **Confluence** | 0.0600 | 8.3% |
| **Type** | 0.0133 | 1.8% |
| **Bias** | 0.2140 | 29.4% |
| **Momentum** | 0.0360 | 5.0% |
| **TOTAL (Avg Confidence)** | 0.7269 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 163
- **Señales generadas:** 163 (100.0%)
- **Señales rechazadas (WAIT):** 0 (0.0%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **Bias**: 0.2140
2. **Proximity**: 0.2070
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
   - Revisar R:R promedio: 2.53
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-11-17 17:28:54*
