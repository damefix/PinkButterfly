# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-11 10:56:00  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251111_104912.csv`  
**Trades Analizados:** 7
  
**Última Operación Cerrada:** T0007 - SELL - 2025-11-04 05:30:00

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 7 |
| **Operaciones Ejecutadas (Cerradas)** | 3 |
| **Operaciones Canceladas** | 0 |
| **Operaciones Expiradas** | 0 |
| **Operaciones Pendientes** | 4 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 33.3% (1/3) |
| **Profit Factor** | 0.77 |
| **P&L Total (Puntos)** | -7.18 |
| **P&L Total (USD)** | $-35.91 |
| **Gross Profit** | $120.00 |
| **Gross Loss** | $155.91 |
| **Avg Win** | $120.00 |
| **Avg Loss** | $77.95 |
| **Avg R:R (Planned)** | 2.37 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## Trade Book (Libro de Operaciones)

### Operaciones Cerradas (3 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Date | Exit Date |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|------------|-----------|
| T0005 | SELL | 6650.25 | 6676.75 | 6625.00 | 6676.75 | [SL] SL | -26.50 | $-132.50 | 0.95 | 2025-10-17 13:15:00 | 2025-10-17 16:00:00 |
| T0006 | SELL | 6781.50 | 6786.18 | 6759.00 | 6786.18 | [SL] SL | -4.68 | $-23.41 | 4.81 | 2025-10-21 17:00:00 | 2025-10-21 18:15:00 |
| T0007 | SELL | 6873.50 | 6891.20 | 6849.50 | 6849.50 | [TP] TP | +24.00 | $+120.00 | 1.36 | 2025-11-03 22:15:00 | 2025-11-04 05:30:00 |


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

**Análisis de 216 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.2493 | 42.5% |
| **Proximity** | 0.1335 | 22.8% |
| **Confluence** | 0.0000 | 0.0% |
| **Type** | 0.0000 | 0.0% |
| **Bias** | 0.1737 | 29.6% |
| **Momentum** | 0.0600 | 10.2% |
| **TOTAL (Avg Confidence)** | 0.5862 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 216
- **Señales generadas:** 47 (21.8%)
- **Señales rechazadas (WAIT):** 169 (78.2%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **CoreScore**: 0.2493
2. **Bias**: 0.1737
3. **Proximity**: 0.1335
4. **Momentum**: 0.0600
5. **Confluence**: 0.0000
6. **Type**: 0.0000

**Recomendaciones de calibración:**


---

# 🎓 CONCLUSIONES Y RECOMENDACIONES

## Diagnóstico

### Rentabilidad

- ⚠️ **ADVERTENCIA:** Win Rate bajo (33.3% < 50%)
- **Acción sugerida:** Calibrar pesos del DFM

- ⚠️ **CRÍTICO:** Profit Factor < 1.0 (sistema perdedor: 0.77)
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
   - Revisar R:R promedio: 2.37
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-11-11 10:56:00*
