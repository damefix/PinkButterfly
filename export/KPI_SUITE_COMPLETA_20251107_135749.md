# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-07 14:02:57  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251107_135749.csv`  
**Trades Analizados:** 10
  
**Última Operación Cerrada:** T0010 - BUY - 2025-06-12 03:30:00

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 10 |
| **Operaciones Ejecutadas (Cerradas)** | 7 |
| **Operaciones Canceladas** | 0 |
| **Operaciones Expiradas** | 3 |
| **Operaciones Pendientes** | 0 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 28.6% (2/7) |
| **Profit Factor** | 0.85 |
| **P&L Total (Puntos)** | -22.08 |
| **P&L Total (USD)** | $-110.37 |
| **Gross Profit** | $625.00 |
| **Gross Loss** | $735.37 |
| **Avg Win** | $312.50 |
| **Avg Loss** | $147.07 |
| **Avg R:R (Planned)** | 2.09 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## Trade Book (Libro de Operaciones)

### Operaciones Cerradas (7 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Date | Exit Date |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|------------|-----------|
| T0001 | BUY | 6139.50 | 6111.38 | 6203.00 | 6203.00 | [TP] TP | +63.50 | $+317.50 | 2.26 | 2024-12-23 16:15:00 | 2024-12-23 22:00:00 |
| T0002 | BUY | 6141.50 | 6112.88 | 6203.00 | 6203.00 | [TP] TP | +61.50 | $+307.50 | 2.15 | 2025-01-16 16:30:00 | 2025-01-17 17:30:00 |
| T0004 | BUY | 6141.50 | 6113.68 | 6194.25 | 6113.68 | [SL] SL | -27.82 | $-139.11 | 1.90 | 2025-02-25 16:00:00 | 2025-02-25 16:30:00 |
| T0007 | BUY | 6139.50 | 6111.32 | 6203.00 | 6111.32 | [SL] SL | -28.18 | $-140.89 | 2.25 | 2025-02-26 19:00:00 | 2025-02-26 20:00:00 |
| T0008 | BUY | 6141.50 | 6114.38 | 6194.25 | 6114.38 | [SL] SL | -27.13 | $-135.63 | 1.94 | 2025-02-26 20:45:00 | 2025-02-26 22:30:00 |
| T0009 | BUY | 6141.50 | 6106.18 | 6212.25 | 6106.18 | [SL] SL | -35.32 | $-176.61 | 2.00 | 2025-02-26 23:00:00 | 2025-02-27 16:15:00 |
| T0010 | BUY | 6141.50 | 6112.88 | 6203.00 | 6112.88 | [SL] SL | -28.63 | $-143.13 | 2.15 | 2025-06-11 20:30:00 | 2025-06-12 03:30:00 |


### Operaciones Canceladas (0 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|


### Operaciones Expiradas (3 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
| T0005 | BUY | 6172.50 | 6117.91 | 6244.25 | 1.31 | 2025-02-26 18:00:00 | STALE_DIST: 6 |
| T0003 | BUY | 6203.00 | 6159.05 | 6256.50 | 1.22 | 2025-02-24 19:00:00 | STALE_DIST: 2 |
| T0006 | BUY | 6184.25 | 6117.38 | 6256.50 | 1.08 | 2025-02-26 18:15:00 | STALE_DIST: 9 |


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (0 total)

| Razón | Cantidad | % |
|-------|----------|---|


### Expiraciones (3 total)

| Razón | Cantidad | % |
|-------|----------|---|
| STALE_DIST: 2 | 1 | 33.3% |
| STALE_DIST: 6 | 1 | 33.3% |
| STALE_DIST: 9 | 1 | 33.3% |




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 184 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.2500 | 37.0% |
| **Proximity** | 0.2256 | 33.4% |
| **Confluence** | 0.1500 | 22.2% |
| **Type** | 0.0000 | 0.0% |
| **Bias** | 0.0674 | 10.0% |
| **Momentum** | 0.0000 | 0.0% |
| **TOTAL (Avg Confidence)** | 0.6764 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 184
- **Señales generadas:** 119 (64.7%)
- **Señales rechazadas (WAIT):** 65 (35.3%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **CoreScore**: 0.2500
2. **Proximity**: 0.2256
3. **Confluence**: 0.1500
4. **Bias**: 0.0674
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
   - Revisar R:R promedio: 2.09
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-11-07 14:02:57*
