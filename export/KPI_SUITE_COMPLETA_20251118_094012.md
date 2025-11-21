# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-18 09:46:07  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251118_094012.csv`  
**Trades Analizados:** 1
  
**Última Operación Cerrada:** T0001 - SELL - 2025-11-07 12:15:00

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 1 |
| **Operaciones Ejecutadas (Cerradas)** | 1 |
| **Operaciones Canceladas** | 0 |
| **Operaciones Expiradas** | 0 |
| **Operaciones Pendientes** | 0 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 100.0% (1/1) |
| **Profit Factor** | 0.00 |
| **P&L Total (Puntos)** | +9.75 |
| **P&L Total (USD)** | $+48.75 |
| **Gross Profit** | $48.75 |
| **Gross Loss** | $0.00 |
| **Avg Win** | $48.75 |
| **Avg Loss** | $0.00 |
| **Avg R:R (Planned)** | 2.52 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## Trade Book (Libro de Operaciones)

### Operaciones Cerradas (1 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Date | Exit Date |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|------------|-----------|
| T0001 | SELL | 6768.75 | 6772.63 | 6759.00 | 6759.00 | [TP] TP | +9.75 | $+48.75 | 2.52 | 2025-10-03 22:00:00 | 2025-11-07 12:15:00 |


### Operaciones Canceladas (0 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|


### Operaciones Expiradas (0 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (0 total)

| Razón | Cantidad | % |
|-------|----------|---|


### Expiraciones (0 total)

| Razón | Cantidad | % |
|-------|----------|---|




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 445 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.2000 | 27.1% |
| **Proximity** | 0.2347 | 31.8% |
| **Confluence** | 0.0600 | 8.1% |
| **Type** | 0.0133 | 1.8% |
| **Bias** | 0.1951 | 26.5% |
| **Momentum** | 0.0360 | 4.9% |
| **TOTAL (Avg Confidence)** | 0.7373 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 445
- **Señales generadas:** 445 (100.0%)
- **Señales rechazadas (WAIT):** 0 (0.0%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **Proximity**: 0.2347
2. **CoreScore**: 0.2000
3. **Bias**: 0.1951
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
   - Revisar R:R promedio: 2.52
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-11-18 09:46:07*
