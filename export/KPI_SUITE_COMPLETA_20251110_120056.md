# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-10 12:01:22  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251110_120056.csv`  
**Trades Analizados:** 90
  
**Última Operación Cerrada:** T0081 - SELL - 2025-11-07 21:00:00

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 90 |
| **Operaciones Ejecutadas (Cerradas)** | 7 |
| **Operaciones Canceladas** | 2 |
| **Operaciones Expiradas** | 0 |
| **Operaciones Pendientes** | 81 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 85.7% (6/7) |
| **Profit Factor** | 18.81 |
| **P&L Total (Puntos)** | +199.55 |
| **P&L Total (USD)** | $+997.73 |
| **Gross Profit** | $1053.75 |
| **Gross Loss** | $56.02 |
| **Avg Win** | $175.62 |
| **Avg Loss** | $56.02 |
| **Avg R:R (Planned)** | 1.85 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## Trade Book (Libro de Operaciones)

### Operaciones Cerradas (7 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Date | Exit Date |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|------------|-----------|
| T0002 | BUY | 6758.00 | 6746.65 | 6785.25 | 6785.25 | [TP] TP | +27.25 | $+136.25 | 2.40 | 2025-11-05 03:30:00 | 2025-11-05 04:30:00 |
| T0021 | SELL | 6763.75 | 6785.80 | 6737.75 | 6748.50 | [TP] TP | +22.50 | $+112.50 | 1.18 | 2025-11-06 21:00:00 | 2025-11-06 22:00:00 |
| T0033 | SELL | 6791.25 | 6823.14 | 6767.25 | 6737.75 | [TP] TP | +26.00 | $+130.00 | 0.75 | 2025-11-07 05:00:00 | 2025-11-07 05:15:00 |
| T0055 | SELL | 6749.25 | 6763.83 | 6733.50 | 6748.50 | [TP] TP | +22.25 | $+111.25 | 1.08 | 2025-11-07 10:45:00 | 2025-11-07 11:00:00 |
| T0060 | SELL | 6764.50 | 6785.25 | 6748.50 | 6733.50 | [TP] TP | +37.25 | $+186.25 | 0.77 | 2025-11-07 12:00:00 | 2025-11-07 12:15:00 |
| T0065 | SELL | 6730.50 | 6741.79 | 6690.75 | 6690.75 | [TP] TP | +75.50 | $+377.50 | 3.52 | 2025-11-07 15:30:00 | 2025-11-07 15:45:00 |
| T0081 | SELL | 6792.50 | 6803.94 | 6755.00 | 6741.70 | [SL] SL | -11.20 | $-56.02 | 3.28 | 2025-11-07 20:45:00 | 2025-11-07 21:00:00 |


### Operaciones Canceladas (2 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
| T0010 | BUY | 6757.75 | 6746.72 | 6785.25 | 2.49 | 2025-11-06 03:15:00 | BOS contradictorio |
| T0004 | BUY | 6758.00 | 6746.29 | 6785.25 | 2.33 | 2025-11-05 16:30:00 | BOS contradictorio |


### Operaciones Expiradas (0 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (2 total)

| Razón | Cantidad | % |
|-------|----------|---|
| BOS contradictorio | 2 | 100.0% |


### Expiraciones (0 total)

| Razón | Cantidad | % |
|-------|----------|---|




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 93 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.2500 | 33.2% |
| **Proximity** | 0.2236 | 29.7% |
| **Confluence** | 0.1500 | 19.9% |
| **Type** | 0.0000 | 0.0% |
| **Bias** | 0.1459 | 19.4% |
| **Momentum** | 0.0000 | 0.0% |
| **TOTAL (Avg Confidence)** | 0.7519 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 93
- **Señales generadas:** 93 (100.0%)
- **Señales rechazadas (WAIT):** 0 (0.0%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **CoreScore**: 0.2500
2. **Proximity**: 0.2236
3. **Confluence**: 0.1500
4. **Bias**: 0.1459
5. **Type**: 0.0000
6. **Momentum**: 0.0000

**Recomendaciones de calibración:**


---

# 🎓 CONCLUSIONES Y RECOMENDACIONES

## Diagnóstico

### Rentabilidad

- [OK] **OK:** Win Rate aceptable (85.7%)

- [OK] **OK:** Profit Factor aceptable (18.81)


## Próximos Pasos

1. **Análisis Profundo de Operaciones Perdedoras:**
   - Activar `ShowScoringBreakdown = true`
   - Ejecutar Fast Load
   - Analizar scoring de las 1 operaciones con SL_HIT

2. **Calibración del DFM:**
   - Revisar pesos: CoreScore, Proximity, Bias, Confluence
   - Ajustar basándose en análisis científico

3. **Optimización de Gestión de Riesgo:**
   - Revisar R:R promedio: 1.85
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-11-10 12:01:22*
