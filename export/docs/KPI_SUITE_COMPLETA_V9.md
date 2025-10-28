# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-10-26 17:09:16  
**CSV File:** `logs/trades_20251026_170705.csv`  
**Trades Analizados:** 28

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 28 |
| **Operaciones Ejecutadas (Cerradas)** | 8 |
| **Operaciones Canceladas** | 9 |
| **Operaciones Expiradas** | 8 |
| **Operaciones Pendientes** | 3 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 37.5% (3/8) |
| **Profit Factor** | 1.41 |
| **P&L Total (Puntos)** | +13.25 |
| **P&L Total (USD)** | $+66.25 |
| **Gross Profit** | $228.75 |
| **Gross Loss** | $162.50 |
| **Avg Win** | $76.25 |
| **Avg Loss** | $32.50 |
| **Avg R:R (Planned)** | 2.64 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## KPI 2.1: Trade Book (Libro de Operaciones)

### Operaciones Cerradas (8 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Bar | Exit Bar |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|-----------|----------|
| T0002 | SELL | 6503.50 | 6514.70 | 6487.25 | 6514.70 | [SL] SL | -10.45 | $-52.25 | 1.45 | 3796 | 3805 |
| T0008 | SELL | 6714.75 | 6719.95 | 6701.50 | 6719.95 | [SL] SL | -5.20 | $-26.00 | 2.55 | 5679 | 5684 |
| T0015 | SELL | 6776.25 | 6782.45 | 6766.25 | 6782.45 | [SL] SL | -6.20 | $-31.00 | 1.61 | 6489 | 6509 |
| T0020 | SELL | 6778.25 | 6782.45 | 6769.00 | 6769.00 | [TP] TP | +9.25 | $+46.25 | 2.20 | 6742 | 6744 |
| T0024 | SELL | 6778.75 | 6782.45 | 6772.50 | 6782.45 | [SL] SL | -3.70 | $-18.50 | 1.69 | 6819 | 6821 |
| T0026 | SELL | 6763.25 | 6766.95 | 6747.25 | 6747.25 | [TP] TP | +16.00 | $+80.00 | 4.32 | 7564 | 7566 |
| T0027 | SELL | 6750.00 | 6756.95 | 6729.50 | 6729.50 | [TP] TP | +20.50 | $+102.50 | 2.95 | 7609 | 7641 |
| T0028 | SELL | 6750.00 | 6756.95 | 6719.75 | 6756.95 | [SL] SL | -6.95 | $-34.75 | 4.35 | 7654 | 7656 |


### Operaciones Canceladas (9 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Bar | Razón |
|----------|-----|-------|----|----|----------|-----------|-------|
| T0009 | SELL | 6730.13 | 6733.13 | 6698.50 | 10.54 | 5696 | BOS contradictorio |
| T0010 | SELL | 6730.13 | 6733.13 | 6702.00 | 9.38 | 5697 | BOS contradictorio |
| T0011 | SELL | 6730.13 | 6733.13 | 6702.00 | 9.38 | 5701 | BOS contradictorio |
| T0012 | SELL | 6728.50 | 6731.70 | 6712.75 | 4.92 | 5713 | BOS contradictorio |
| T0013 | SELL | 6728.50 | 6731.70 | 6712.75 | 4.92 | 5718 | BOS contradictorio |
| T0019 | SELL | 6776.25 | 6782.45 | 6758.00 | 2.94 | 6632 | BOS contradictorio |
| T0018 | SELL | 6775.00 | 6782.45 | 6755.75 | 2.58 | 6628 | BOS contradictorio |
| T0006 | SELL | 6714.75 | 6719.95 | 6701.50 | 2.55 | 5653 | BOS contradictorio |
| T0003 | SELL | 6522.00 | 6525.45 | 6518.55 | 1.00 | 3822 | BOS contradictorio |


### Operaciones Expiradas (8 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Bar | Razón |
|----------|-----|-------|----|----|----------|-----------|-------|
| T0021 | SELL | 6781.50 | 6784.50 | 6759.25 | 7.42 | 6789 | score decayó a 0 |
| T0023 | SELL | 6792.75 | 6797.70 | 6766.25 | 5.35 | 6817 | score decayó a 0 |
| T0005 | SELL | 6658.50 | 6662.70 | 6639.75 | 4.46 | 5251 | score decayó a 0 |
| T0014 | SELL | 6779.50 | 6782.50 | 6766.25 | 4.42 | 6488 | score decayó a 0 |
| T0016 | SELL | 6772.50 | 6782.45 | 6755.75 | 1.68 | 6596 | score decayó a 0 |
| T0004 | SELL | 6666.75 | 6681.45 | 6643.00 | 1.62 | 5211 | score decayó a 0 |
| T0022 | SELL | 6775.75 | 6782.45 | 6766.25 | 1.42 | 6807 | score decayó a 0 |
| T0025 | SELL | 6792.50 | 6797.70 | 6786.25 | 1.20 | 6822 | score decayó a 0 |


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (9 total)

| Razón | Cantidad | % |
|-------|----------|---|
| BOS contradictorio | 9 | 100.0% |


### Expiraciones (8 total)

| Razón | Cantidad | % |
|-------|----------|---|
| score decayó a 0 | 8 | 100.0% |




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 610 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.0997 | 24.9% |
| **Proximity** | 0.1513 | 37.8% |
| **Confluence** | 0.0497 | 12.4% |
| **Type** | 0.0667 | 16.7% |
| **Bias** | 0.0830 | 20.7% |
| **Momentum** | 0.0000 | 0.0% |
| **TOTAL (Avg Confidence)** | 0.4002 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 610
- **Señales generadas:** 88 (14.4%)
- **Señales rechazadas (WAIT):** 522 (85.6%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **Proximity**: 0.1513
2. **CoreScore**: 0.0997
3. **Bias**: 0.0830
4. **Type**: 0.0667
5. **Confluence**: 0.0497
6. **Momentum**: 0.0000

**Recomendaciones de calibración:**

- ⚠️ **CoreScore bajo (0.0997)**: Las estructuras base tienen poca calidad
  - Acción: Revisar detectores o aumentar `Weight_CoreScore`

---

# 🎓 CONCLUSIONES Y RECOMENDACIONES

## Diagnóstico

### Rentabilidad

- ⚠️ **ADVERTENCIA:** Win Rate bajo (37.5% < 50%)
- **Acción sugerida:** Calibrar pesos del DFM

- ⚠️ **ADVERTENCIA:** Profit Factor bajo (1.41 < 1.5)


## Próximos Pasos

1. **Análisis Profundo de Operaciones Perdedoras:**
   - Activar `ShowScoringBreakdown = true`
   - Ejecutar Fast Load
   - Analizar scoring de las 5 operaciones con SL_HIT

2. **Calibración del DFM:**
   - Revisar pesos: CoreScore, Proximity, Bias, Confluence
   - Ajustar basándose en análisis científico

3. **Optimización de Gestión de Riesgo:**
   - Revisar R:R promedio: 2.64
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-10-26 17:09:16*
