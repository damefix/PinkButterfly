# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-21 12:02:31  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251121_115355.csv`  
**Trades Analizados:** 12
  
**Última Operación Cerrada:** T0012 - SELL - 2025-11-18 21:30:00

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 12 |
| **Operaciones Ejecutadas (Cerradas)** | 7 |
| **Operaciones Canceladas** | 0 |
| **Operaciones Expiradas** | 4 |
| **Operaciones Pendientes** | 1 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 28.6% (2/7) |
| **Profit Factor** | 1.58 |
| **P&L Total (Puntos)** | +24.83 |
| **P&L Total (USD)** | $+124.14 |
| **Gross Profit** | $338.75 |
| **Gross Loss** | $214.61 |
| **Avg Win** | $169.38 |
| **Avg Loss** | $42.92 |
| **Avg R:R (Planned)** | 4.16 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## Trade Book (Libro de Operaciones)

### Operaciones Cerradas (7 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Date | Exit Date |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|------------|-----------|
| T0001 | SELL | 6875.00 | 6885.52 | 6830.00 | 6885.52 | [SL] SL | -10.52 | $-52.61 | 4.28 | 2025-10-30 14:15:00 | 2025-10-30 16:45:00 |
| T0002 | SELL | 6900.00 | 6912.75 | 6870.75 | 6912.75 | [SL] SL | -12.75 | $-63.75 | 2.29 | 2025-10-31 01:15:00 | 2025-10-31 13:15:00 |
| T0004 | SELL | 6875.00 | 6885.80 | 6830.00 | 6885.80 | [SL] SL | -10.80 | $-54.00 | 4.17 | 2025-10-31 15:30:00 | 2025-10-31 15:45:00 |
| T0008 | SELL | 6873.00 | 6896.49 | 6830.00 | 6830.00 | [TP] TP | +43.00 | $+215.00 | 1.83 | 2025-11-12 18:15:00 | 2025-11-13 15:45:00 |
| T0009 | SELL | 6762.00 | 6774.00 | 6723.25 | 6768.47 | [SL] SL | -6.47 | $-32.36 | 3.23 | 2025-11-14 01:45:00 | 2025-11-14 05:00:00 |
| T0010 | SELL | 6714.50 | 6716.88 | 6690.75 | 6716.88 | [SL] SL | -2.38 | $-11.89 | 9.98 | 2025-11-14 13:45:00 | 2025-11-14 16:00:00 |
| T0012 | SELL | 6680.25 | 6687.68 | 6655.50 | 6655.50 | [TP] TP | +24.75 | $+123.75 | 3.33 | 2025-11-18 15:45:00 | 2025-11-18 21:30:00 |


### Operaciones Canceladas (0 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|


### Operaciones Expiradas (4 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
| T0007 | SELL | 6875.00 | 6887.56 | 6831.00 | 3.50 | 2025-11-04 16:30:00 | STALE_DIST: 3 |
| T0006 | SELL | 6900.00 | 6906.15 | 6882.50 | 2.85 | 2025-11-03 09:30:00 | STALE_DIST: 3 |
| T0003 | SELL | 6900.00 | 6906.69 | 6882.50 | 2.62 | 2025-10-31 13:45:00 | ADAPTIVE_MOMENTUM: 0.202ATR/bar |
| T0005 | SELL | 6900.00 | 6912.54 | 6877.00 | 1.83 | 2025-11-03 00:15:00 | ADAPTIVE_MOMENTUM: 0.280ATR/bar |


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (0 total)

| Razón | Cantidad | % |
|-------|----------|---|


### Expiraciones (4 total)

| Razón | Cantidad | % |
|-------|----------|---|
| STALE_DIST: 3 | 2 | 50.0% |
| ADAPTIVE_MOMENTUM: 0.202ATR/bar | 1 | 25.0% |
| ADAPTIVE_MOMENTUM: 0.280ATR/bar | 1 | 25.0% |




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 232 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.2000 | 27.1% |
| **Proximity** | 0.2352 | 31.8% |
| **Confluence** | 0.0600 | 8.1% |
| **Type** | 0.0133 | 1.8% |
| **Bias** | 0.1972 | 26.7% |
| **Momentum** | 0.0360 | 4.9% |
| **TOTAL (Avg Confidence)** | 0.7388 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 232
- **Señales generadas:** 232 (100.0%)
- **Señales rechazadas (WAIT):** 0 (0.0%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **Proximity**: 0.2352
2. **CoreScore**: 0.2000
3. **Bias**: 0.1972
4. **Confluence**: 0.0600
5. **Momentum**: 0.0360
6. **Type**: 0.0133

**Recomendaciones de calibración:**


---

# 🎓 CONCLUSIONES Y RECOMENDACIONES

## Diagnóstico

### Rentabilidad

- ⚠️ **CRÍTICO:** Win Rate muy bajo (28.6% < 30%)
- **Problema:** El sistema está generando señales de baja calidad
- **Acción requerida:** Revisar pesos del DFM y criterios de entrada

- [OK] **OK:** Profit Factor aceptable (1.58)


## Próximos Pasos

1. **Análisis Profundo de Operaciones Perdedoras:**
   - Activar `ShowScoringBreakdown = true`
   - Ejecutar Fast Load
   - Analizar scoring de las 5 operaciones con SL_HIT

2. **Calibración del DFM:**
   - Revisar pesos: CoreScore, Proximity, Bias, Confluence
   - Ajustar basándose en análisis científico

3. **Optimización de Gestión de Riesgo:**
   - Revisar R:R promedio: 4.16
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-11-21 12:02:31*
