# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-10-26 16:32:30  
**CSV File:** `logs/trades_20251026_162943.csv`  
**Trades Analizados:** 18

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 18 |
| **Operaciones Ejecutadas (Cerradas)** | 14 |
| **Operaciones Canceladas** | 2 |
| **Operaciones Expiradas** | 1 |
| **Operaciones Pendientes** | 1 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 42.9% (6/14) |
| **Profit Factor** | 2.00 |
| **P&L Total (Puntos)** | +41.20 |
| **P&L Total (USD)** | $+206.00 |
| **Gross Profit** | $411.25 |
| **Gross Loss** | $205.25 |
| **Avg Win** | $68.54 |
| **Avg Loss** | $25.66 |
| **Avg R:R (Planned)** | 2.97 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## KPI 2.1: Trade Book (Libro de Operaciones)

### Operaciones Cerradas (14 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Bar | Exit Bar |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|-----------|----------|
| T0001 | SELL | 6563.00 | 6566.00 | 6554.75 | 6554.75 | [TP] TP | +8.25 | $+41.25 | 2.75 | 3129 | 3140 |
| T0002 | SELL | 6557.25 | 6565.95 | 6538.75 | 6538.75 | [TP] TP | +18.50 | $+92.50 | 2.13 | 3915 | 3922 |
| T0003 | SELL | 6565.25 | 6568.25 | 6558.75 | 6568.25 | [SL] SL | -3.00 | $-15.00 | 2.17 | 4034 | 4037 |
| T0005 | SELL | 6521.00 | 6525.45 | 6516.00 | 6525.45 | [SL] SL | -4.45 | $-22.25 | 1.12 | 4428 | 4431 |
| T0006 | SELL | 6581.25 | 6584.25 | 6576.50 | 6584.25 | [SL] SL | -3.00 | $-15.00 | 1.58 | 4497 | 4503 |
| T0007 | SELL | 6581.25 | 6584.25 | 6576.50 | 6576.50 | [TP] TP | +4.75 | $+23.75 | 1.58 | 4503 | 4516 |
| T0009 | SELL | 6594.00 | 6598.70 | 6569.50 | 6598.70 | [SL] SL | -4.70 | $-23.50 | 5.21 | 4757 | 4773 |
| T0011 | SELL | 6592.75 | 6598.70 | 6565.00 | 6586.00 | [TP] TP | +11.75 | $+58.75 | 4.66 | 4820 | 4821 |
| T0012 | SELL | 6718.00 | 6721.00 | 6688.25 | 6688.25 | [TP] TP | +29.75 | $+148.75 | 9.92 | 5332 | 5349 |
| T0013 | SELL | 6754.25 | 6757.25 | 6743.00 | 6757.25 | [SL] SL | -3.00 | $-15.00 | 3.75 | 6205 | 6207 |
| T0014 | SELL | 6774.75 | 6782.45 | 6767.00 | 6782.45 | [SL] SL | -7.70 | $-38.50 | 1.01 | 6315 | 6329 |
| T0015 | SELL | 6780.00 | 6783.00 | 6773.50 | 6783.00 | [SL] SL | -3.00 | $-15.00 | 2.17 | 6460 | 6462 |
| T0016 | SELL | 6770.25 | 6782.45 | 6754.00 | 6782.45 | [SL] SL | -12.20 | $-61.00 | 1.33 | 6620 | 6641 |
| T0018 | SELL | 6778.25 | 6782.45 | 6769.00 | 6769.00 | [TP] TP | +9.25 | $+46.25 | 2.20 | 6742 | 6744 |


### Operaciones Canceladas (2 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Bar | Razón |
|----------|-----|-------|----|----|----------|-----------|-------|
| T0017 | SELL | 6807.00 | 6810.00 | 6774.00 | 11.00 | 6667 | BOS contradictorio |
| T0008 | SELL | 6565.00 | 6568.00 | 6546.75 | 6.08 | 4708 | BOS contradictorio |


### Operaciones Expiradas (1 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Bar | Razón |
|----------|-----|-------|----|----|----------|-----------|-------|
| T0004 | SELL | 6565.25 | 6568.25 | 6558.75 | 2.17 | 4044 | score decayó a 0 |


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (2 total)

| Razón | Cantidad | % |
|-------|----------|---|
| BOS contradictorio | 2 | 100.0% |


### Expiraciones (1 total)

| Razón | Cantidad | % |
|-------|----------|---|
| score decayó a 0 | 1 | 100.0% |




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 610 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.1495 | 37.9% |
| **Proximity** | 0.1513 | 38.3% |
| **Confluence** | 0.0000 | 0.0% |
| **Type** | 0.0667 | 16.9% |
| **Bias** | 0.0548 | 13.9% |
| **Momentum** | 0.0000 | 0.0% |
| **TOTAL (Avg Confidence)** | 0.3948 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 610
- **Señales generadas:** 30 (4.9%)
- **Señales rechazadas (WAIT):** 580 (95.1%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **Proximity**: 0.1513
2. **CoreScore**: 0.1495
3. **Type**: 0.0667
4. **Bias**: 0.0548
5. **Confluence**: 0.0000
6. **Momentum**: 0.0000

**Recomendaciones de calibración:**

- ⚠️ **CoreScore bajo (0.1495)**: Las estructuras base tienen poca calidad
  - Acción: Revisar detectores o aumentar `Weight_CoreScore`

---

# 🎓 CONCLUSIONES Y RECOMENDACIONES

## Diagnóstico

### Rentabilidad

- ⚠️ **ADVERTENCIA:** Win Rate bajo (42.9% < 50%)
- **Acción sugerida:** Calibrar pesos del DFM

- [OK] **OK:** Profit Factor aceptable (2.00)


## Próximos Pasos

1. **Análisis Profundo de Operaciones Perdedoras:**
   - Activar `ShowScoringBreakdown = true`
   - Ejecutar Fast Load
   - Analizar scoring de las 8 operaciones con SL_HIT

2. **Calibración del DFM:**
   - Revisar pesos: CoreScore, Proximity, Bias, Confluence
   - Ajustar basándose en análisis científico

3. **Optimización de Gestión de Riesgo:**
   - Revisar R:R promedio: 2.97
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-10-26 16:32:30*
