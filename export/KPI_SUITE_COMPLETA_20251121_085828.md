# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-21 09:05:36  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251121_085828.csv`  
**Trades Analizados:** 10
  
**Última Operación Cerrada:** T0009 - SELL - 2025-11-18 21:30:00

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 10 |
| **Operaciones Ejecutadas (Cerradas)** | 6 |
| **Operaciones Canceladas** | 0 |
| **Operaciones Expiradas** | 3 |
| **Operaciones Pendientes** | 1 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 83.3% (5/6) |
| **Profit Factor** | 16.65 |
| **P&L Total (Puntos)** | +158.61 |
| **P&L Total (USD)** | $+793.07 |
| **Gross Profit** | $843.75 |
| **Gross Loss** | $50.68 |
| **Avg Win** | $168.75 |
| **Avg Loss** | $50.68 |
| **Avg R:R (Planned)** | 2.43 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## Trade Book (Libro de Operaciones)

### Operaciones Cerradas (6 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Date | Exit Date |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|------------|-----------|
| T0001 | SELL | 6918.50 | 6933.24 | 6882.50 | 6882.50 | [TP] TP | +36.00 | $+180.00 | 2.44 | 2025-10-30 08:45:00 | 2025-10-30 15:00:00 |
| T0002 | SELL | 6900.50 | 6910.64 | 6865.25 | 6910.64 | [SL] SL | -10.14 | $-50.68 | 3.48 | 2025-10-31 02:00:00 | 2025-10-31 13:00:00 |
| T0004 | SELL | 6873.00 | 6896.49 | 6830.00 | 6830.00 | [TP] TP | +43.00 | $+215.00 | 1.83 | 2025-11-12 18:15:00 | 2025-11-13 15:45:00 |
| T0006 | SELL | 6749.50 | 6759.99 | 6730.75 | 6730.75 | [TP] TP | +18.75 | $+93.75 | 1.79 | 2025-11-14 08:30:00 | 2025-11-14 12:15:00 |
| T0007 | SELL | 6754.00 | 6780.91 | 6707.75 | 6707.75 | [TP] TP | +46.25 | $+231.25 | 1.72 | 2025-11-17 16:00:00 | 2025-11-17 20:00:00 |
| T0009 | SELL | 6680.25 | 6687.68 | 6655.50 | 6655.50 | [TP] TP | +24.75 | $+123.75 | 3.33 | 2025-11-18 15:45:00 | 2025-11-18 21:30:00 |


### Operaciones Canceladas (0 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|


### Operaciones Expiradas (3 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Date | Razón |
|----------|-----|-------|----|----|----------|------------|-------|
| T0009 | SELL | 6680.25 | 6687.68 | 6655.50 | 3.33 | 2025-11-18 15:45:00 | score decayó a 0 |
| T0003 | SELL | 6900.50 | 6913.04 | 6865.25 | 2.81 | 2025-11-03 00:15:00 | ADAPTIVE_MOMENTUM: 0.281ATR/bar |
| T0005 | SELL | 6762.00 | 6778.54 | 6723.25 | 2.34 | 2025-11-14 00:15:00 | ADAPTIVE_DEPARTURE: 2.12x |


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (0 total)

| Razón | Cantidad | % |
|-------|----------|---|


### Expiraciones (3 total)

| Razón | Cantidad | % |
|-------|----------|---|
| ADAPTIVE_MOMENTUM: 0.281ATR/bar | 1 | 33.3% |
| ADAPTIVE_DEPARTURE: 2.12x | 1 | 33.3% |
| score decayó a 0 | 1 | 33.3% |




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 213 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.2000 | 27.2% |
| **Proximity** | 0.2395 | 32.5% |
| **Confluence** | 0.0600 | 8.2% |
| **Type** | 0.0133 | 1.8% |
| **Bias** | 0.1894 | 25.7% |
| **Momentum** | 0.0360 | 4.9% |
| **TOTAL (Avg Confidence)** | 0.7361 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 213
- **Señales generadas:** 213 (100.0%)
- **Señales rechazadas (WAIT):** 0 (0.0%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **Proximity**: 0.2395
2. **CoreScore**: 0.2000
3. **Bias**: 0.1894
4. **Confluence**: 0.0600
5. **Momentum**: 0.0360
6. **Type**: 0.0133

**Recomendaciones de calibración:**


---

# 🎓 CONCLUSIONES Y RECOMENDACIONES

## Diagnóstico

### Rentabilidad

- [OK] **OK:** Win Rate aceptable (83.3%)

- [OK] **OK:** Profit Factor aceptable (16.65)


## Próximos Pasos

1. **Análisis Profundo de Operaciones Perdedoras:**
   - Activar `ShowScoringBreakdown = true`
   - Ejecutar Fast Load
   - Analizar scoring de las 1 operaciones con SL_HIT

2. **Calibración del DFM:**
   - Revisar pesos: CoreScore, Proximity, Bias, Confluence
   - Ajustar basándose en análisis científico

3. **Optimización de Gestión de Riesgo:**
   - Revisar R:R promedio: 2.43
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-11-21 09:05:36*
