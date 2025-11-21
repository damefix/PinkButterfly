# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-10 09:15:30  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251110_091442.csv`  
**Trades Analizados:** 40
  
**Última Operación Cerrada:** T0020 - SELL - 2025-11-07 12:15:00

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 40 |
| **Operaciones Ejecutadas (Cerradas)** | 1 |
| **Operaciones Canceladas** | 0 |
| **Operaciones Expiradas** | 8 |
| **Operaciones Pendientes** | 31 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 0.0% (0/1) |
| **Profit Factor** | 0.00 |
| **P&L Total (Puntos)** | -25.98 |
| **P&L Total (USD)** | $-129.89 |
| **Gross Profit** | $0.00 |
| **Gross Loss** | $129.89 |
| **Avg Win** | $0.00 |
| **Avg Loss** | $129.89 |
| **Avg R:R (Planned)** | 1.50 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## Trade Book (Libro de Operaciones)

### Operaciones Cerradas (1 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Date | Exit Date |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|------------|-----------|
| T0020 | SELL | 6784.25 | 6846.48 | 6690.75 | 6730.77 | [SL] SL | -25.98 | $-129.89 | 1.50 | 2025-11-07 12:00:00 | 2025-11-07 12:15:00 |


### Operaciones Canceladas (0 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|


### Operaciones Expiradas (8 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
| T0028 | SELL | 6775.00 | 6824.23 | 6683.50 | 1.86 | 2025-11-07 17:30:00 | score decayó a 0 |
| T0017 | SELL | 6764.00 | 6772.80 | 6748.50 | 1.76 | 2025-11-07 10:45:00 | estructura no existe |
| T0025 | SELL | 6777.25 | 6824.23 | 6713.25 | 1.36 | 2025-11-07 15:30:00 | score decayó a 0 |
| T0011 | SELL | 6749.25 | 6774.48 | 6719.75 | 1.17 | 2025-11-07 05:30:00 | score decayó a 0 |
| T0010 | SELL | 6792.50 | 6824.23 | 6755.75 | 1.16 | 2025-11-07 04:00:00 | score decayó a 0 |
| T0019 | SELL | 6748.25 | 6774.48 | 6719.75 | 1.09 | 2025-11-07 11:30:00 | score decayó a 0 |
| T0012 | SELL | 6790.50 | 6824.23 | 6755.75 | 1.03 | 2025-11-07 06:15:00 | score decayó a 0 |
| T0013 | SELL | 6783.75 | 6824.23 | 6742.50 | 1.02 | 2025-11-07 07:00:00 | score decayó a 0 |


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (0 total)

| Razón | Cantidad | % |
|-------|----------|---|


### Expiraciones (8 total)

| Razón | Cantidad | % |
|-------|----------|---|
| score decayó a 0 | 7 | 87.5% |
| estructura no existe | 1 | 12.5% |




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 60 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.2500 | 34.4% |
| **Proximity** | 0.2595 | 35.7% |
| **Confluence** | 0.1500 | 20.6% |
| **Type** | 0.0000 | 0.0% |
| **Bias** | 0.1268 | 17.4% |
| **Momentum** | 0.0000 | 0.0% |
| **TOTAL (Avg Confidence)** | 0.7276 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 60
- **Señales generadas:** 60 (100.0%)
- **Señales rechazadas (WAIT):** 0 (0.0%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **Proximity**: 0.2595
2. **CoreScore**: 0.2500
3. **Confluence**: 0.1500
4. **Bias**: 0.1268
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
   - Analizar scoring de las 1 operaciones con SL_HIT

2. **Calibración del DFM:**
   - Revisar pesos: CoreScore, Proximity, Bias, Confluence
   - Ajustar basándose en análisis científico

3. **Optimización de Gestión de Riesgo:**
   - Revisar R:R promedio: 1.50
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-11-10 09:15:30*
