# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-07 13:15:12  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251107_130646.csv`  
**Trades Analizados:** 8
  
**Última Operación Cerrada:** T0008 - BUY - 2025-06-12 03:45:00

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 8 |
| **Operaciones Ejecutadas (Cerradas)** | 6 |
| **Operaciones Canceladas** | 0 |
| **Operaciones Expiradas** | 2 |
| **Operaciones Pendientes** | 0 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 33.3% (2/6) |
| **Profit Factor** | 1.02 |
| **P&L Total (Puntos)** | +2.39 |
| **P&L Total (USD)** | $+11.96 |
| **Gross Profit** | $625.00 |
| **Gross Loss** | $613.04 |
| **Avg Win** | $312.50 |
| **Avg Loss** | $153.26 |
| **Avg R:R (Planned)** | 2.10 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## Trade Book (Libro de Operaciones)

### Operaciones Cerradas (6 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Date | Exit Date |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|------------|-----------|
| T0001 | BUY | 6139.50 | 6111.38 | 6203.00 | 6203.00 | [TP] TP | +63.50 | $+317.50 | 2.26 | 2024-12-23 16:15:00 | 2024-12-23 22:00:00 |
| T0002 | BUY | 6141.50 | 6112.88 | 6203.00 | 6203.00 | [TP] TP | +61.50 | $+307.50 | 2.15 | 2025-01-16 16:30:00 | 2025-01-17 17:30:00 |
| T0004 | BUY | 6141.50 | 6113.68 | 6194.25 | 6113.68 | [SL] SL | -27.82 | $-139.11 | 1.90 | 2025-02-25 16:00:00 | 2025-02-25 16:30:00 |
| T0006 | BUY | 6139.50 | 6111.32 | 6203.00 | 6111.32 | [SL] SL | -28.18 | $-140.89 | 2.25 | 2025-02-26 19:00:00 | 2025-02-26 20:00:00 |
| T0007 | BUY | 6141.50 | 6106.18 | 6212.25 | 6106.18 | [SL] SL | -35.32 | $-176.61 | 2.00 | 2025-02-26 23:00:00 | 2025-02-27 16:15:00 |
| T0008 | BUY | 6139.50 | 6108.21 | 6203.00 | 6108.21 | [SL] SL | -31.29 | $-156.43 | 2.03 | 2025-06-11 20:45:00 | 2025-06-12 03:45:00 |


### Operaciones Canceladas (0 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|


### Operaciones Expiradas (2 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
| T0005 | BUY | 6172.50 | 6117.91 | 6244.25 | 1.31 | 2025-02-26 18:00:00 | STALE_DIST: 6 |
| T0003 | BUY | 6203.00 | 6159.05 | 6256.50 | 1.22 | 2025-02-24 19:00:00 | STALE_DIST: 2 |


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (0 total)

| Razón | Cantidad | % |
|-------|----------|---|


### Expiraciones (2 total)

| Razón | Cantidad | % |
|-------|----------|---|
| STALE_DIST: 2 | 1 | 50.0% |
| STALE_DIST: 6 | 1 | 50.0% |




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 183 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.2500 | 36.9% |
| **Proximity** | 0.2261 | 33.4% |
| **Confluence** | 0.1500 | 22.2% |
| **Type** | 0.0000 | 0.0% |
| **Bias** | 0.0672 | 9.9% |
| **Momentum** | 0.0000 | 0.0% |
| **TOTAL (Avg Confidence)** | 0.6766 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 183
- **Señales generadas:** 118 (64.5%)
- **Señales rechazadas (WAIT):** 65 (35.5%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **CoreScore**: 0.2500
2. **Proximity**: 0.2261
3. **Confluence**: 0.1500
4. **Bias**: 0.0672
5. **Type**: 0.0000
6. **Momentum**: 0.0000

**Recomendaciones de calibración:**


---

# 🎓 CONCLUSIONES Y RECOMENDACIONES

## Diagnóstico

### Rentabilidad

- ⚠️ **ADVERTENCIA:** Win Rate bajo (33.3% < 50%)
- **Acción sugerida:** Calibrar pesos del DFM

- ⚠️ **ADVERTENCIA:** Profit Factor bajo (1.02 < 1.5)


## Próximos Pasos

1. **Análisis Profundo de Operaciones Perdedoras:**
   - Activar `ShowScoringBreakdown = true`
   - Ejecutar Fast Load
   - Analizar scoring de las 4 operaciones con SL_HIT

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
*Fecha: 2025-11-07 13:15:12*
