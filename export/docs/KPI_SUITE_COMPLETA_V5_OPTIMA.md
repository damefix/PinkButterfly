# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-10-26 17:25:01  
**CSV File:** `logs/trades_20251026_172256.csv`  
**Trades Analizados:** 23

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 23 |
| **Operaciones Ejecutadas (Cerradas)** | 15 |
| **Operaciones Canceladas** | 1 |
| **Operaciones Expiradas** | 4 |
| **Operaciones Pendientes** | 3 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 26.7% (4/15) |
| **Profit Factor** | 0.91 |
| **P&L Total (Puntos)** | -5.20 |
| **P&L Total (USD)** | $-26.00 |
| **Gross Profit** | $260.00 |
| **Gross Loss** | $286.00 |
| **Avg Win** | $65.00 |
| **Avg Loss** | $26.00 |
| **Avg R:R (Planned)** | 3.47 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## KPI 2.1: Trade Book (Libro de Operaciones)

### Operaciones Cerradas (15 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Bar | Exit Bar |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|-----------|----------|
| T0001 | SELL | 6563.00 | 6566.00 | 6554.75 | 6554.75 | [TP] TP | +8.25 | $+41.25 | 2.75 | 3129 | 3140 |
| T0003 | SELL | 6551.39 | 6554.39 | 6529.00 | 6554.39 | [SL] SL | -3.00 | $-15.00 | 7.46 | 3918 | 3920 |
| T0004 | SELL | 6565.25 | 6568.25 | 6558.75 | 6568.25 | [SL] SL | -3.00 | $-15.00 | 2.17 | 4034 | 4037 |
| T0006 | SELL | 6521.00 | 6525.45 | 6516.00 | 6525.45 | [SL] SL | -4.45 | $-22.25 | 1.12 | 4428 | 4431 |
| T0007 | SELL | 6581.25 | 6584.25 | 6576.50 | 6584.25 | [SL] SL | -3.00 | $-15.00 | 1.58 | 4497 | 4503 |
| T0008 | SELL | 6581.25 | 6584.25 | 6576.50 | 6576.50 | [TP] TP | +4.75 | $+23.75 | 1.58 | 4503 | 4516 |
| T0009 | SELL | 6557.00 | 6565.95 | 6537.50 | 6565.95 | [SL] SL | -8.95 | $-44.75 | 2.18 | 4589 | 4619 |
| T0011 | SELL | 6594.00 | 6598.70 | 6569.50 | 6598.70 | [SL] SL | -4.70 | $-23.50 | 5.21 | 4756 | 4773 |
| T0015 | SELL | 6658.50 | 6662.70 | 6639.75 | 6662.70 | [SL] SL | -4.20 | $-21.00 | 4.46 | 5065 | 5070 |
| T0016 | SELL | 6718.00 | 6721.00 | 6688.25 | 6688.25 | [TP] TP | +29.75 | $+148.75 | 9.92 | 5332 | 5349 |
| T0017 | SELL | 6754.25 | 6757.25 | 6743.00 | 6757.25 | [SL] SL | -3.00 | $-15.00 | 3.75 | 6205 | 6207 |
| T0018 | SELL | 6774.75 | 6782.45 | 6767.00 | 6782.45 | [SL] SL | -7.70 | $-38.50 | 1.01 | 6315 | 6329 |
| T0020 | SELL | 6779.75 | 6782.75 | 6763.75 | 6783.00 | [SL] SL | -3.00 | $-15.00 | 5.33 | 6461 | 6462 |
| T0021 | SELL | 6770.25 | 6782.45 | 6754.00 | 6782.45 | [SL] SL | -12.20 | $-61.00 | 1.33 | 6620 | 6641 |
| T0023 | SELL | 6778.25 | 6782.45 | 6769.00 | 6769.00 | [TP] TP | +9.25 | $+46.25 | 2.20 | 6742 | 6744 |


### Operaciones Canceladas (1 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Bar | Razón |
|----------|-----|-------|----|----|----------|-----------|-------|
| T0022 | SELL | 6807.00 | 6810.00 | 6774.00 | 11.00 | 6667 | BOS contradictorio |


### Operaciones Expiradas (4 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Bar | Razón |
|----------|-----|-------|----|----|----------|-----------|-------|
| T0010 | SELL | 6565.00 | 6568.00 | 6546.75 | 6.08 | 4705 | score decayó a 0 |
| T0013 | SELL | 6592.75 | 6598.70 | 6565.00 | 4.66 | 4817 | score decayó a 0 |
| T0014 | SELL | 6593.00 | 6598.70 | 6579.25 | 2.41 | 4857 | score decayó a 0 |
| T0005 | SELL | 6565.25 | 6568.25 | 6558.75 | 2.17 | 4044 | score decayó a 0 |


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (1 total)

| Razón | Cantidad | % |
|-------|----------|---|
| BOS contradictorio | 1 | 100.0% |


### Expiraciones (4 total)

| Razón | Cantidad | % |
|-------|----------|---|
| score decayó a 0 | 4 | 100.0% |




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 610 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.1993 | 48.6% |
| **Proximity** | 0.1513 | 36.9% |
| **Confluence** | 0.0000 | 0.0% |
| **Type** | 0.0333 | 8.1% |
| **Bias** | 0.0548 | 13.4% |
| **Momentum** | 0.0000 | 0.0% |
| **TOTAL (Avg Confidence)** | 0.4100 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 610
- **Señales generadas:** 41 (6.7%)
- **Señales rechazadas (WAIT):** 569 (93.3%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **CoreScore**: 0.1993
2. **Proximity**: 0.1513
3. **Bias**: 0.0548
4. **Type**: 0.0333
5. **Confluence**: 0.0000
6. **Momentum**: 0.0000

**Recomendaciones de calibración:**

- ⚠️ **CoreScore bajo (0.1993)**: Las estructuras base tienen poca calidad
  - Acción: Revisar detectores o aumentar `Weight_CoreScore`

---

# 🎓 CONCLUSIONES Y RECOMENDACIONES

## Diagnóstico

### Rentabilidad

- ⚠️ **CRÍTICO:** Win Rate muy bajo (26.7% < 30%)
- **Problema:** El sistema está generando señales de baja calidad
- **Acción requerida:** Revisar pesos del DFM y criterios de entrada

- ⚠️ **CRÍTICO:** Profit Factor < 1.0 (sistema perdedor: 0.91)
- **Problema:** Las pérdidas superan las ganancias
- **Acción requerida:** 
  1. Revisar R:R de las operaciones
  2. Analizar cancelaciones por BOS
  3. Aumentar `MinConfidenceForEntry`


## Próximos Pasos

1. **Análisis Profundo de Operaciones Perdedoras:**
   - Activar `ShowScoringBreakdown = true`
   - Ejecutar Fast Load
   - Analizar scoring de las 11 operaciones con SL_HIT

2. **Calibración del DFM:**
   - Revisar pesos: CoreScore, Proximity, Bias, Confluence
   - Ajustar basándose en análisis científico

3. **Optimización de Gestión de Riesgo:**
   - Revisar R:R promedio: 3.47
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-10-26 17:25:01*
