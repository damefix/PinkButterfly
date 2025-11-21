# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-21 18:00:27  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251121_175842.csv`  
**Trades Analizados:** 7
  
**Última Operación Cerrada:** T0007 - SELL - 2025-11-21 17:45:00

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 7 |
| **Operaciones Ejecutadas (Cerradas)** | 3 |
| **Operaciones Canceladas** | 0 |
| **Operaciones Expiradas** | 2 |
| **Operaciones Pendientes** | 2 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 33.3% (1/3) |
| **Profit Factor** | 1.23 |
| **P&L Total (Puntos)** | +8.55 |
| **P&L Total (USD)** | $+42.78 |
| **Gross Profit** | $232.50 |
| **Gross Loss** | $189.72 |
| **Avg Win** | $232.50 |
| **Avg Loss** | $94.86 |
| **Avg R:R (Planned)** | 2.40 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## Trade Book (Libro de Operaciones)

### Operaciones Cerradas (3 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Date | Exit Date |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|------------|-----------|
| T0002 | SELL | 6682.00 | 6710.98 | 6635.50 | 6635.50 | [TP] TP | +46.50 | $+232.50 | 1.60 | 2025-11-18 18:30:00 | 2025-11-18 22:15:00 |
| T0003 | SELL | 6636.75 | 6647.79 | 6594.00 | 6647.79 | [SL] SL | -11.04 | $-55.18 | 3.87 | 2025-11-19 05:00:00 | 2025-11-19 09:30:00 |
| T0007 | SELL | 6571.50 | 6598.68 | 6524.75 | 6598.41 | [SL] SL | -26.91 | $-134.54 | 1.72 | 2025-11-21 15:45:00 | 2025-11-21 17:45:00 |


### Operaciones Canceladas (0 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|


### Operaciones Expiradas (2 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
| T0004 | SELL | 6635.25 | 6646.93 | 6594.00 | 3.53 | 2025-11-19 10:00:00 | ADAPTIVE_MOMENTUM: 0.522ATR/bar |
| T0001 | SELL | 6683.00 | 6696.44 | 6638.50 | 3.31 | 2025-11-18 14:30:00 | STALE_DIST: 3 |


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (0 total)

| Razón | Cantidad | % |
|-------|----------|---|


### Expiraciones (2 total)

| Razón | Cantidad | % |
|-------|----------|---|
| STALE_DIST: 3 | 1 | 50.0% |
| ADAPTIVE_MOMENTUM: 0.522ATR/bar | 1 | 50.0% |




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 21 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.2000 | 30.0% |
| **Proximity** | 0.2178 | 32.7% |
| **Confluence** | 0.0600 | 9.0% |
| **Type** | 0.0133 | 2.0% |
| **Bias** | 0.1462 | 21.9% |
| **Momentum** | 0.0360 | 5.4% |
| **TOTAL (Avg Confidence)** | 0.6671 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 21
- **Señales generadas:** 21 (100.0%)
- **Señales rechazadas (WAIT):** 0 (0.0%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **Proximity**: 0.2178
2. **CoreScore**: 0.2000
3. **Bias**: 0.1462
4. **Confluence**: 0.0600
5. **Momentum**: 0.0360
6. **Type**: 0.0133

**Recomendaciones de calibración:**


---

# 🎓 CONCLUSIONES Y RECOMENDACIONES

## Diagnóstico

### Rentabilidad

- ⚠️ **ADVERTENCIA:** Win Rate bajo (33.3% < 50%)
- **Acción sugerida:** Calibrar pesos del DFM

- ⚠️ **ADVERTENCIA:** Profit Factor bajo (1.23 < 1.5)


## Próximos Pasos

1. **Análisis Profundo de Operaciones Perdedoras:**
   - Activar `ShowScoringBreakdown = true`
   - Ejecutar Fast Load
   - Analizar scoring de las 2 operaciones con SL_HIT

2. **Calibración del DFM:**
   - Revisar pesos: CoreScore, Proximity, Bias, Confluence
   - Ajustar basándose en análisis científico

3. **Optimización de Gestión de Riesgo:**
   - Revisar R:R promedio: 2.40
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-11-21 18:00:27*
