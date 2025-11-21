# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-07 13:34:19  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251107_132509.csv`  
**Trades Analizados:** 8
  
**Última Operación Cerrada:** T0008 - BUY - 2025-06-12 03:30:00

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 8 |
| **Operaciones Ejecutadas (Cerradas)** | 7 |
| **Operaciones Canceladas** | 0 |
| **Operaciones Expiradas** | 1 |
| **Operaciones Pendientes** | 0 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 28.6% (2/7) |
| **Profit Factor** | 0.85 |
| **P&L Total (Puntos)** | -20.83 |
| **P&L Total (USD)** | $-104.12 |
| **Gross Profit** | $591.25 |
| **Gross Loss** | $695.37 |
| **Avg Win** | $295.62 |
| **Avg Loss** | $139.07 |
| **Avg R:R (Planned)** | 2.14 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## Trade Book (Libro de Operaciones)

### Operaciones Cerradas (7 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Date | Exit Date |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|------------|-----------|
| T0001 | BUY | 6139.50 | 6111.38 | 6203.00 | 6203.00 | [TP] TP | +63.50 | $+317.50 | 2.26 | 2024-12-23 16:15:00 | 2024-12-23 22:00:00 |
| T0002 | BUY | 6139.50 | 6112.88 | 6194.25 | 6194.25 | [TP] TP | +54.75 | $+273.75 | 2.06 | 2025-01-16 16:30:00 | 2025-01-17 15:30:00 |
| T0004 | BUY | 6139.50 | 6113.68 | 6194.25 | 6113.68 | [SL] SL | -25.82 | $-129.11 | 2.12 | 2025-02-25 16:00:00 | 2025-02-25 16:30:00 |
| T0005 | BUY | 6139.50 | 6111.32 | 6203.00 | 6111.32 | [SL] SL | -28.18 | $-140.89 | 2.25 | 2025-02-26 19:00:00 | 2025-02-26 20:00:00 |
| T0006 | BUY | 6139.50 | 6114.38 | 6194.25 | 6114.38 | [SL] SL | -25.13 | $-125.63 | 2.18 | 2025-02-26 20:45:00 | 2025-02-26 22:30:00 |
| T0007 | BUY | 6139.50 | 6106.18 | 6207.00 | 6106.18 | [SL] SL | -33.32 | $-166.61 | 2.03 | 2025-02-26 23:00:00 | 2025-02-27 16:15:00 |
| T0008 | BUY | 6139.50 | 6112.88 | 6194.25 | 6112.88 | [SL] SL | -26.63 | $-133.13 | 2.06 | 2025-06-11 20:30:00 | 2025-06-12 03:30:00 |


### Operaciones Canceladas (0 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|


### Operaciones Expiradas (1 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
| T0003 | BUY | 6203.00 | 6159.05 | 6256.50 | 1.22 | 2025-02-24 19:00:00 | STALE_DIST: 2 |


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (0 total)

| Razón | Cantidad | % |
|-------|----------|---|


### Expiraciones (1 total)

| Razón | Cantidad | % |
|-------|----------|---|
| STALE_DIST: 2 | 1 | 100.0% |




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 183 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.2500 | 36.9% |
| **Proximity** | 0.2265 | 33.5% |
| **Confluence** | 0.1500 | 22.2% |
| **Type** | 0.0000 | 0.0% |
| **Bias** | 0.0672 | 9.9% |
| **Momentum** | 0.0000 | 0.0% |
| **TOTAL (Avg Confidence)** | 0.6770 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 183
- **Señales generadas:** 118 (64.5%)
- **Señales rechazadas (WAIT):** 65 (35.5%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **CoreScore**: 0.2500
2. **Proximity**: 0.2265
3. **Confluence**: 0.1500
4. **Bias**: 0.0672
5. **Type**: 0.0000
6. **Momentum**: 0.0000

**Recomendaciones de calibración:**


---

# 🎓 CONCLUSIONES Y RECOMENDACIONES

## Diagnóstico

### Rentabilidad

- ⚠️ **CRÍTICO:** Win Rate muy bajo (28.6% < 30%)
- **Problema:** El sistema está generando señales de baja calidad
- **Acción requerida:** Revisar pesos del DFM y criterios de entrada

- ⚠️ **CRÍTICO:** Profit Factor < 1.0 (sistema perdedor: 0.85)
- **Problema:** Las pérdidas superan las ganancias
- **Acción requerida:** 
  1. Revisar R:R de las operaciones
  2. Analizar cancelaciones por BOS
  3. Aumentar `MinConfidenceForEntry`


## Próximos Pasos

1. **Análisis Profundo de Operaciones Perdedoras:**
   - Activar `ShowScoringBreakdown = true`
   - Ejecutar Fast Load
   - Analizar scoring de las 5 operaciones con SL_HIT

2. **Calibración del DFM:**
   - Revisar pesos: CoreScore, Proximity, Bias, Confluence
   - Ajustar basándose en análisis científico

3. **Optimización de Gestión de Riesgo:**
   - Revisar R:R promedio: 2.14
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-11-07 13:34:19*
