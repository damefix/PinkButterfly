# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-10 08:27:39  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251110_082659.csv`  
**Trades Analizados:** 17
  
**Última Operación Cerrada:** T0001 - SELL - 2025-11-06 17:30:00

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 17 |
| **Operaciones Ejecutadas (Cerradas)** | 1 |
| **Operaciones Canceladas** | 0 |
| **Operaciones Expiradas** | 8 |
| **Operaciones Pendientes** | 8 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 100.0% (1/1) |
| **Profit Factor** | 0.00 |
| **P&L Total (Puntos)** | +44.00 |
| **P&L Total (USD)** | $+220.00 |
| **Gross Profit** | $220.00 |
| **Gross Loss** | $0.00 |
| **Avg Win** | $220.00 |
| **Avg Loss** | $0.00 |
| **Avg R:R (Planned)** | 1.39 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## Trade Book (Libro de Operaciones)

### Operaciones Cerradas (1 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Date | Exit Date |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|------------|-----------|
| T0001 | SELL | 6792.50 | 6824.23 | 6748.50 | 6748.50 | [TP] TP | +44.00 | $+220.00 | 1.39 | 2025-11-06 16:30:00 | 2025-11-06 17:30:00 |


### Operaciones Canceladas (0 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|


### Operaciones Expiradas (8 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
| T0011 | SELL | 6774.75 | 6824.23 | 6665.50 | 2.21 | 2025-11-07 18:15:00 | STALE_DIST: 5 |
| T0009 | SELL | 6775.25 | 6824.23 | 6683.50 | 1.87 | 2025-11-07 16:45:00 | STALE_DIST: 6 |
| T0010 | SELL | 6775.00 | 6824.23 | 6683.50 | 1.86 | 2025-11-07 17:30:00 | STALE_DIST: 6 |
| T0006 | SELL | 6764.00 | 6772.80 | 6748.50 | 1.76 | 2025-11-07 10:45:00 | score decayó a 0 |
| T0007 | SELL | 6778.25 | 6824.23 | 6713.25 | 1.41 | 2025-11-07 14:15:00 | STALE_DIST: 5 |
| T0008 | SELL | 6777.25 | 6824.23 | 6713.25 | 1.36 | 2025-11-07 14:30:00 | STALE_DIST: 5 |
| T0013 | SELL | 6775.75 | 6824.23 | 6713.25 | 1.29 | 2025-11-07 20:15:00 | STALE_DIST: 4 |
| T0004 | SELL | 6792.50 | 6824.23 | 6755.75 | 1.16 | 2025-11-07 00:45:00 | score decayó a 0 |


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (0 total)

| Razón | Cantidad | % |
|-------|----------|---|


### Expiraciones (8 total)

| Razón | Cantidad | % |
|-------|----------|---|
| STALE_DIST: 5 | 3 | 37.5% |
| score decayó a 0 | 2 | 25.0% |
| STALE_DIST: 6 | 2 | 25.0% |
| STALE_DIST: 4 | 1 | 12.5% |




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 54 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.2500 | 33.6% |
| **Proximity** | 0.2629 | 35.3% |
| **Confluence** | 0.1500 | 20.2% |
| **Type** | 0.0000 | 0.0% |
| **Bias** | 0.1335 | 18.0% |
| **Momentum** | 0.0000 | 0.0% |
| **TOTAL (Avg Confidence)** | 0.7437 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 54
- **Señales generadas:** 54 (100.0%)
- **Señales rechazadas (WAIT):** 0 (0.0%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **Proximity**: 0.2629
2. **CoreScore**: 0.2500
3. **Confluence**: 0.1500
4. **Bias**: 0.1335
5. **Type**: 0.0000
6. **Momentum**: 0.0000

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
   - Revisar R:R promedio: 1.39
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-11-10 08:27:39*
