# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-07 20:52:13  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251107_204828.csv`  
**Trades Analizados:** 3
  
**Última Operación Cerrada:** T0001 - SELL - 2025-11-07 17:15:00

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 3 |
| **Operaciones Ejecutadas (Cerradas)** | 1 |
| **Operaciones Canceladas** | 0 |
| **Operaciones Expiradas** | 1 |
| **Operaciones Pendientes** | 1 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 100.0% (1/1) |
| **Profit Factor** | 0.00 |
| **P&L Total (Puntos)** | +64.57 |
| **P&L Total (USD)** | $+322.86 |
| **Gross Profit** | $322.86 |
| **Gross Loss** | $0.00 |
| **Avg Win** | $322.86 |
| **Avg Loss** | $0.00 |
| **Avg R:R (Planned)** | 1.00 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## Trade Book (Libro de Operaciones)

### Operaciones Cerradas (1 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Date | Exit Date |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|------------|-----------|
| T0001 | SELL | 6737.75 | 6802.32 | 6673.18 | 6673.18 | [TP] TP | +64.57 | $+322.86 | 1.00 | 2025-11-07 13:00:00 | 2025-11-07 17:15:00 |


### Operaciones Canceladas (0 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|


### Operaciones Expiradas (1 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
| T0002 | SELL | 6750.75 | 6802.77 | 6687.25 | 1.22 | 2025-11-07 20:45:00 | STALE_DIST: 18 |


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (0 total)

| Razón | Cantidad | % |
|-------|----------|---|


### Expiraciones (1 total)

| Razón | Cantidad | % |
|-------|----------|---|
| STALE_DIST: 18 | 1 | 100.0% |




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 70 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.2500 | 34.8% |
| **Proximity** | 0.2187 | 30.4% |
| **Confluence** | 0.1500 | 20.9% |
| **Type** | 0.0000 | 0.0% |
| **Bias** | 0.1000 | 13.9% |
| **Momentum** | 0.0000 | 0.0% |
| **TOTAL (Avg Confidence)** | 0.7187 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 70
- **Señales generadas:** 59 (84.3%)
- **Señales rechazadas (WAIT):** 11 (15.7%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **CoreScore**: 0.2500
2. **Proximity**: 0.2187
3. **Confluence**: 0.1500
4. **Bias**: 0.1000
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
   - Revisar R:R promedio: 1.00
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-11-07 20:52:13*
