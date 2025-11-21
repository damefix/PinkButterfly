# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-06 21:11:47  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251106_210503.csv`  
**Trades Analizados:** 11

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 11 |
| **Operaciones Ejecutadas (Cerradas)** | 0 |
| **Operaciones Canceladas** | 0 |
| **Operaciones Expiradas** | 11 |
| **Operaciones Pendientes** | 0 |

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


### Operaciones Expiradas (11 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
| T0006 | SELL | 6306.25 | 6322.14 | 6259.00 | 2.97 | 2025-02-20 10:00:00 | STALE_DIST: 4 |
| T0005 | SELL | 6306.25 | 6326.75 | 6259.00 | 2.30 | 2025-02-19 15:30:00 | STALE_DIST: 11 |
| T0004 | SELL | 6306.25 | 6327.13 | 6259.00 | 2.26 | 2025-02-18 15:45:00 | STALE_DIST: 9 |
| T0009 | SELL | 6306.25 | 6330.13 | 6259.00 | 1.98 | 2025-07-02 14:30:00 | STALE_DIST: 11 |
| T0008 | SELL | 6306.25 | 6336.07 | 6259.00 | 1.58 | 2025-02-21 16:15:00 | STALE_DIST: 47 |
| T0007 | SELL | 6306.25 | 6335.43 | 6264.00 | 1.45 | 2025-02-20 17:30:00 | STALE_DIST: 32 |
| T0010 | SELL | 6306.25 | 6335.59 | 6264.00 | 1.44 | 2025-07-08 17:45:00 | STALE_DIST: 28 |
| T0001 | SELL | 6306.25 | 6344.76 | 6259.00 | 1.23 | 2025-01-24 15:30:00 | STALE_DIST: 11 |
| T0002 | SELL | 6306.25 | 6345.13 | 6264.00 | 1.09 | 2025-01-24 22:00:00 | STALE_DIST: 15 |
| T0003 | BUY | 6306.25 | 6255.48 | 6357.02 | 1.00 | 2025-02-07 18:00:00 | STALE_DIST: 78 |


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (0 total)

| Razón | Cantidad | % |
|-------|----------|---|


### Expiraciones (11 total)

| Razón | Cantidad | % |
|-------|----------|---|
| STALE_DIST: 11 | 3 | 27.3% |
| STALE_DIST: 15 | 2 | 18.2% |
| STALE_DIST: 78 | 1 | 9.1% |
| STALE_DIST: 9 | 1 | 9.1% |
| STALE_DIST: 4 | 1 | 9.1% |
| STALE_DIST: 32 | 1 | 9.1% |
| STALE_DIST: 47 | 1 | 9.1% |
| STALE_DIST: 28 | 1 | 9.1% |




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 174 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.2500 | 38.2% |
| **Proximity** | 0.1989 | 30.4% |
| **Confluence** | 0.1500 | 22.9% |
| **Type** | 0.0000 | 0.0% |
| **Bias** | 0.0603 | 9.2% |
| **Momentum** | 0.0000 | 0.0% |
| **TOTAL (Avg Confidence)** | 0.6538 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 174
- **Señales generadas:** 88 (50.6%)
- **Señales rechazadas (WAIT):** 86 (49.4%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **CoreScore**: 0.2500
2. **Proximity**: 0.1989
3. **Confluence**: 0.1500
4. **Bias**: 0.0603
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
*Fecha: 2025-11-06 21:11:47*
