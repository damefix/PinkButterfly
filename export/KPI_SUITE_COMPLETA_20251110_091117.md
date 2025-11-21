# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-10 09:11:51  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251110_091117.csv`  
**Trades Analizados:** 43
  
**Última Operación Cerrada:** T0020 - SELL - 2025-11-07 12:15:00

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 43 |
| **Operaciones Ejecutadas (Cerradas)** | 1 |
| **Operaciones Canceladas** | 0 |
| **Operaciones Expiradas** | 20 |
| **Operaciones Pendientes** | 22 |

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


### Operaciones Expiradas (20 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
| T0042 | SELL | 6744.25 | 6767.30 | 6683.50 | 2.64 | 2025-11-07 22:00:00 | STALE_TIME: 9>8bars |
| T0034 | SELL | 6774.75 | 6824.23 | 6664.00 | 2.24 | 2025-11-07 19:30:00 | STALE_TIME: 9>8bars |
| T0036 | SELL | 6774.75 | 6824.23 | 6664.00 | 2.24 | 2025-11-07 20:00:00 | STALE_TIME: 9>8bars |
| T0043 | SELL | 6753.25 | 6772.80 | 6712.25 | 2.10 | 2025-11-07 22:15:00 | STALE_TIME: 9>8bars |
| T0026 | SELL | 6775.25 | 6824.23 | 6683.50 | 1.87 | 2025-11-07 16:45:00 | STALE_TIME: 9>8bars |
| T0028 | SELL | 6775.00 | 6824.23 | 6683.50 | 1.86 | 2025-11-07 17:30:00 | STALE_TIME: 9>8bars |
| T0033 | SELL | 6774.75 | 6824.23 | 6683.50 | 1.84 | 2025-11-07 19:00:00 | STALE_TIME: 9>8bars |
| T0039 | SELL | 6747.50 | 6767.30 | 6712.25 | 1.78 | 2025-11-07 21:00:00 | STALE_TIME: 9>8bars |
| T0017 | SELL | 6764.00 | 6772.80 | 6748.50 | 1.76 | 2025-11-07 10:45:00 | estructura no existe |
| T0021 | SELL | 6764.00 | 6772.80 | 6748.50 | 1.76 | 2025-11-07 13:00:00 | STALE_TIME: 9>8bars |


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (0 total)

| Razón | Cantidad | % |
|-------|----------|---|


### Expiraciones (20 total)

| Razón | Cantidad | % |
|-------|----------|---|
| STALE_TIME: 9>8bars | 13 | 65.0% |
| score decayó a 0 | 6 | 30.0% |
| estructura no existe | 1 | 5.0% |




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
*Fecha: 2025-11-10 09:11:51*
