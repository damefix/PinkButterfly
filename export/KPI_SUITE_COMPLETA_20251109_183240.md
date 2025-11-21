# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-09 18:36:27  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251109_183240.csv`  
**Trades Analizados:** 3

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 3 |
| **Operaciones Ejecutadas (Cerradas)** | 0 |
| **Operaciones Canceladas** | 0 |
| **Operaciones Expiradas** | 2 |
| **Operaciones Pendientes** | 1 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 0.0% (0/0) |
| **Profit Factor** | 0.00 |
| **P&L Total (Puntos)** | +0.00 |
| **P&L Total (USD)** | $+0.00 |
| **Gross Profit** | $0.00 |
| **Gross Loss** | $0.00 |
| **Avg Win** | $0.00 |
| **Avg Loss** | $0.00 |
| **Avg R:R (Planned)** | 0.00 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## Trade Book (Libro de Operaciones)

### Operaciones Cerradas (0 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Date | Exit Date |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|------------|-----------|


### Operaciones Canceladas (0 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|


### Operaciones Expiradas (2 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
| T0001 | SELL | 6731.75 | 6766.75 | 6666.25 | 1.87 | 2025-11-07 18:00:00 | STALE_DIST: 62 |
| T0002 | SELL | 6757.50 | 6792.50 | 6708.75 | 1.39 | 2025-11-07 20:45:00 | STALE_DIST: 14 |


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (0 total)

| Razón | Cantidad | % |
|-------|----------|---|


### Expiraciones (2 total)

| Razón | Cantidad | % |
|-------|----------|---|
| STALE_DIST: 62 | 1 | 50.0% |
| STALE_DIST: 14 | 1 | 50.0% |




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 26 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.2500 | 34.4% |
| **Proximity** | 0.2675 | 36.9% |
| **Confluence** | 0.1500 | 20.7% |
| **Type** | 0.0000 | 0.0% |
| **Bias** | 0.1181 | 16.3% |
| **Momentum** | 0.0000 | 0.0% |
| **TOTAL (Avg Confidence)** | 0.7258 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 26
- **Señales generadas:** 23 (88.5%)
- **Señales rechazadas (WAIT):** 3 (11.5%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **Proximity**: 0.2675
2. **CoreScore**: 0.2500
3. **Confluence**: 0.1500
4. **Bias**: 0.1181
5. **Type**: 0.0000
6. **Momentum**: 0.0000

**Recomendaciones de calibración:**


---

# 🎓 CONCLUSIONES Y RECOMENDACIONES

## Diagnóstico

### Rentabilidad

- ⚠️ **CRÍTICO:** Win Rate muy bajo (0.0% < 30%)
- **Problema:** El sistema está generando señales de baja calidad
- **Acción requerida:** Revisar pesos del DFM y criterios de entrada

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
   - Revisar R:R promedio: 0.00
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-11-09 18:36:27*
