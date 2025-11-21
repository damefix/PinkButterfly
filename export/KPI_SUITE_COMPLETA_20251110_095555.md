# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-10 09:56:32  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251110_095555.csv`  
**Trades Analizados:** 47
  
**Última Operación Cerrada:** T0035 - SELL - 2025-11-07 12:15:00

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 47 |
| **Operaciones Ejecutadas (Cerradas)** | 1 |
| **Operaciones Canceladas** | 0 |
| **Operaciones Expiradas** | 18 |
| **Operaciones Pendientes** | 28 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 0.0% (0/1) |
| **Profit Factor** | 0.00 |
| **P&L Total (Puntos)** | -27.01 |
| **P&L Total (USD)** | $-135.05 |
| **Gross Profit** | $0.00 |
| **Gross Loss** | $135.05 |
| **Avg Win** | $0.00 |
| **Avg Loss** | $135.05 |
| **Avg R:R (Planned)** | 1.03 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## Trade Book (Libro de Operaciones)

### Operaciones Cerradas (1 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Date | Exit Date |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|------------|-----------|
| T0035 | SELL | 6764.00 | 6772.72 | 6755.00 | 6729.74 | [SL] SL | -27.01 | $-135.05 | 1.03 | 2025-11-07 11:15:00 | 2025-11-07 12:15:00 |


### Operaciones Canceladas (0 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|


### Operaciones Expiradas (18 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
| T0024 | SELL | 6765.75 | 6845.29 | 6571.25 | 2.45 | 2025-11-07 06:15:00 | score decayó a 0 |
| T0026 | SELL | 6765.50 | 6845.21 | 6571.25 | 2.44 | 2025-11-07 07:00:00 | score decayó a 0 |
| T0025 | SELL | 6765.50 | 6845.28 | 6571.25 | 2.43 | 2025-11-07 06:30:00 | score decayó a 0 |
| T0043 | SELL | 6731.75 | 6769.55 | 6666.25 | 1.73 | 2025-11-07 17:45:00 | score decayó a 0 |
| T0044 | SELL | 6731.75 | 6769.66 | 6666.25 | 1.73 | 2025-11-07 18:00:00 | score decayó a 0 |
| T0028 | SELL | 6771.00 | 6844.91 | 6651.50 | 1.62 | 2025-11-07 08:45:00 | score decayó a 0 |
| T0022 | SELL | 6784.25 | 6845.43 | 6690.75 | 1.53 | 2025-11-07 05:45:00 | score decayó a 0 |
| T0023 | SELL | 6784.25 | 6845.34 | 6690.75 | 1.53 | 2025-11-07 06:00:00 | score decayó a 0 |
| T0027 | SELL | 6768.00 | 6845.04 | 6651.50 | 1.51 | 2025-11-07 08:30:00 | score decayó a 0 |
| T0020 | SELL | 6767.75 | 6845.39 | 6651.50 | 1.50 | 2025-11-07 05:00:00 | score decayó a 0 |


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (0 total)

| Razón | Cantidad | % |
|-------|----------|---|


### Expiraciones (18 total)

| Razón | Cantidad | % |
|-------|----------|---|
| score decayó a 0 | 18 | 100.0% |




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 82 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.2500 | 31.3% |
| **Proximity** | 0.2540 | 31.8% |
| **Confluence** | 0.1500 | 18.8% |
| **Type** | 0.0000 | 0.0% |
| **Bias** | 0.1497 | 18.7% |
| **Momentum** | 0.0000 | 0.0% |
| **TOTAL (Avg Confidence)** | 0.7991 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 82
- **Señales generadas:** 82 (100.0%)
- **Señales rechazadas (WAIT):** 0 (0.0%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **Proximity**: 0.2540
2. **CoreScore**: 0.2500
3. **Confluence**: 0.1500
4. **Bias**: 0.1497
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
   - Revisar R:R promedio: 1.03
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-11-10 09:56:32*
