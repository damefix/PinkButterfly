# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-10 11:02:13  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251110_110127.csv`  
**Trades Analizados:** 75
  
**Última Operación Cerrada:** T0042 - SELL - 2025-11-07 12:15:00

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 75 |
| **Operaciones Ejecutadas (Cerradas)** | 2 |
| **Operaciones Canceladas** | 0 |
| **Operaciones Expiradas** | 0 |
| **Operaciones Pendientes** | 73 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 0.0% (0/2) |
| **Profit Factor** | 0.00 |
| **P&L Total (Puntos)** | -42.16 |
| **P&L Total (USD)** | $-210.84 |
| **Gross Profit** | $0.00 |
| **Gross Loss** | $210.84 |
| **Avg Win** | $0.00 |
| **Avg Loss** | $105.42 |
| **Avg R:R (Planned)** | 2.10 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## Trade Book (Libro de Operaciones)

### Operaciones Cerradas (2 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Date | Exit Date |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|------------|-----------|
| T0029 | SELL | 6765.50 | 6845.25 | 6571.25 | 6764.31 | [SL] SL | -15.06 | $-75.32 | 2.44 | 2025-11-07 06:45:00 | 2025-11-07 07:00:00 |
| T0042 | SELL | 6764.00 | 6772.75 | 6748.50 | 6729.65 | [SL] SL | -27.10 | $-135.52 | 1.77 | 2025-11-07 12:00:00 | 2025-11-07 12:15:00 |


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

**Análisis de 83 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.2500 | 31.1% |
| **Proximity** | 0.2555 | 31.8% |
| **Confluence** | 0.1500 | 18.6% |
| **Type** | 0.0000 | 0.0% |
| **Bias** | 0.1522 | 18.9% |
| **Momentum** | 0.0000 | 0.0% |
| **TOTAL (Avg Confidence)** | 0.8046 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 83
- **Señales generadas:** 83 (100.0%)
- **Señales rechazadas (WAIT):** 0 (0.0%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **Proximity**: 0.2555
2. **CoreScore**: 0.2500
3. **Bias**: 0.1522
4. **Confluence**: 0.1500
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
   - Analizar scoring de las 2 operaciones con SL_HIT

2. **Calibración del DFM:**
   - Revisar pesos: CoreScore, Proximity, Bias, Confluence
   - Ajustar basándose en análisis científico

3. **Optimización de Gestión de Riesgo:**
   - Revisar R:R promedio: 2.10
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-11-10 11:02:13*
