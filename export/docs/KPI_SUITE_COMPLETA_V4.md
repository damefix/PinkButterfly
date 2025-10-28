# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-10-26 16:23:55  
**CSV File:** `logs/trades_20251026_162124.csv`  
**Trades Analizados:** 14

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 14 |
| **Operaciones Ejecutadas (Cerradas)** | 11 |
| **Operaciones Canceladas** | 0 |
| **Operaciones Expiradas** | 2 |
| **Operaciones Pendientes** | 1 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 36.4% (4/11) |
| **Profit Factor** | 2.32 |
| **P&L Total (Puntos)** | +29.55 |
| **P&L Total (USD)** | $+147.75 |
| **Gross Profit** | $260.00 |
| **Gross Loss** | $112.25 |
| **Avg Win** | $65.00 |
| **Avg Loss** | $16.04 |
| **Avg R:R (Planned)** | 3.09 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## KPI 2.1: Trade Book (Libro de Operaciones)

### Operaciones Cerradas (11 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Bar | Exit Bar |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|-----------|----------|
| T0001 | SELL | 6563.00 | 6566.00 | 6554.75 | 6554.75 | [TP] TP | +8.25 | $+41.25 | 2.75 | 3129 | 3140 |
| T0002 | SELL | 6565.25 | 6568.25 | 6558.75 | 6568.25 | [SL] SL | -3.00 | $-15.00 | 2.17 | 4034 | 4037 |
| T0004 | SELL | 6521.00 | 6525.45 | 6516.00 | 6525.45 | [SL] SL | -4.45 | $-22.25 | 1.12 | 4428 | 4431 |
| T0005 | SELL | 6581.25 | 6584.25 | 6576.50 | 6584.25 | [SL] SL | -3.00 | $-15.00 | 1.58 | 4497 | 4503 |
| T0006 | SELL | 6581.25 | 6584.25 | 6576.50 | 6576.50 | [TP] TP | +4.75 | $+23.75 | 1.58 | 4503 | 4516 |
| T0009 | SELL | 6680.75 | 6683.75 | 6665.75 | 6683.75 | [SL] SL | -3.00 | $-15.00 | 5.00 | 5292 | 5295 |
| T0010 | SELL | 6718.00 | 6721.00 | 6688.25 | 6688.25 | [TP] TP | +29.75 | $+148.75 | 9.92 | 5332 | 5349 |
| T0011 | SELL | 6754.25 | 6757.25 | 6743.00 | 6757.25 | [SL] SL | -3.00 | $-15.00 | 3.75 | 6205 | 6207 |
| T0012 | SELL | 6768.75 | 6771.75 | 6763.50 | 6771.75 | [SL] SL | -3.00 | $-15.00 | 1.75 | 6256 | 6258 |
| T0013 | SELL | 6780.00 | 6783.00 | 6773.50 | 6783.00 | [SL] SL | -3.00 | $-15.00 | 2.17 | 6460 | 6462 |
| T0014 | SELL | 6778.25 | 6782.45 | 6769.00 | 6769.00 | [TP] TP | +9.25 | $+46.25 | 2.20 | 6742 | 6744 |


### Operaciones Canceladas (0 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Bar | Razón |
|----------|-----|-------|----|----|----------|-----------|-------|


### Operaciones Expiradas (2 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Bar | Razón |
|----------|-----|-------|----|----|----------|-----------|-------|
| T0008 | SELL | 6593.75 | 6598.70 | 6565.00 | 5.81 | 4817 | score decayó a 0 |
| T0003 | SELL | 6565.25 | 6568.25 | 6558.75 | 2.17 | 4044 | score decayó a 0 |


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (0 total)

| Razón | Cantidad | % |
|-------|----------|---|


### Expiraciones (2 total)

| Razón | Cantidad | % |
|-------|----------|---|
| score decayó a 0 | 2 | 100.0% |




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 632 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.1495 | 41.3% |
| **Proximity** | 0.1131 | 31.2% |
| **Confluence** | 0.0000 | 0.0% |
| **Type** | 0.0667 | 18.4% |
| **Bias** | 0.0568 | 15.7% |
| **Momentum** | 0.0000 | 0.0% |
| **TOTAL (Avg Confidence)** | 0.3618 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 632
- **Señales generadas:** 24 (3.8%)
- **Señales rechazadas (WAIT):** 608 (96.2%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **CoreScore**: 0.1495
2. **Proximity**: 0.1131
3. **Type**: 0.0667
4. **Bias**: 0.0568
5. **Confluence**: 0.0000
6. **Momentum**: 0.0000

**Recomendaciones de calibración:**

- ⚠️ **CoreScore bajo (0.1495)**: Las estructuras base tienen poca calidad
  - Acción: Revisar detectores o aumentar `Weight_CoreScore`

---

# 🎓 CONCLUSIONES Y RECOMENDACIONES

## Diagnóstico

### Rentabilidad

- ⚠️ **ADVERTENCIA:** Win Rate bajo (36.4% < 50%)
- **Acción sugerida:** Calibrar pesos del DFM

- [OK] **OK:** Profit Factor aceptable (2.32)


## Próximos Pasos

1. **Análisis Profundo de Operaciones Perdedoras:**
   - Activar `ShowScoringBreakdown = true`
   - Ejecutar Fast Load
   - Analizar scoring de las 7 operaciones con SL_HIT

2. **Calibración del DFM:**
   - Revisar pesos: CoreScore, Proximity, Bias, Confluence
   - Ajustar basándose en análisis científico

3. **Optimización de Gestión de Riesgo:**
   - Revisar R:R promedio: 3.09
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-10-26 16:23:55*
