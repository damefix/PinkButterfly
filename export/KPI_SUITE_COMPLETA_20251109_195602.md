# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-09 19:56:27  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251109_195602.csv`  
**Trades Analizados:** 1

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 1 |
| **Operaciones Ejecutadas (Cerradas)** | 0 |
| **Operaciones Canceladas** | 0 |
| **Operaciones Expiradas** | 0 |
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

**Análisis de 25 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.2500 | 35.5% |
| **Proximity** | 0.2542 | 36.1% |
| **Confluence** | 0.1500 | 21.3% |
| **Type** | 0.0000 | 0.0% |
| **Bias** | 0.1335 | 19.0% |
| **Momentum** | 0.0000 | 0.0% |
| **TOTAL (Avg Confidence)** | 0.7033 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 25
- **Señales generadas:** 25 (100.0%)
- **Señales rechazadas (WAIT):** 0 (0.0%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **Proximity**: 0.2542
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
*Fecha: 2025-11-09 19:56:27*
