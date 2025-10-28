# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-10-26 17:01:32  
**CSV File:** `logs/trades_20251026_165914.csv`  
**Trades Analizados:** 31

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 31 |
| **Operaciones Ejecutadas (Cerradas)** | 10 |
| **Operaciones Canceladas** | 9 |
| **Operaciones Expiradas** | 9 |
| **Operaciones Pendientes** | 3 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 40.0% (4/10) |
| **Profit Factor** | 1.46 |
| **P&L Total (Puntos)** | +16.50 |
| **P&L Total (USD)** | $+82.50 |
| **Gross Profit** | $260.00 |
| **Gross Loss** | $177.50 |
| **Avg Win** | $65.00 |
| **Avg Loss** | $29.58 |
| **Avg R:R (Planned)** | 2.46 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## KPI 2.1: Trade Book (Libro de Operaciones)

### Operaciones Cerradas (10 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Bar | Exit Bar |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|-----------|----------|
| T0002 | SELL | 6503.50 | 6514.70 | 6487.25 | 6514.70 | [SL] SL | -10.45 | $-52.25 | 1.45 | 3796 | 3805 |
| T0008 | SELL | 6714.75 | 6719.95 | 6701.50 | 6719.95 | [SL] SL | -5.20 | $-26.00 | 2.55 | 5679 | 5684 |
| T0014 | SELL | 6779.75 | 6782.75 | 6772.75 | 6782.75 | [SL] SL | -3.00 | $-15.00 | 2.33 | 6457 | 6461 |
| T0016 | SELL | 6776.25 | 6782.45 | 6766.25 | 6782.45 | [SL] SL | -6.20 | $-31.00 | 1.61 | 6489 | 6509 |
| T0021 | SELL | 6778.25 | 6782.45 | 6769.00 | 6769.00 | [TP] TP | +9.25 | $+46.25 | 2.20 | 6742 | 6744 |
| T0023 | SELL | 6792.00 | 6797.70 | 6785.75 | 6785.75 | [TP] TP | +6.25 | $+31.25 | 1.10 | 6791 | 6795 |
| T0026 | SELL | 6778.75 | 6782.45 | 6772.50 | 6782.45 | [SL] SL | -3.70 | $-18.50 | 1.69 | 6819 | 6821 |
| T0029 | SELL | 6763.25 | 6766.95 | 6747.25 | 6747.25 | [TP] TP | +16.00 | $+80.00 | 4.32 | 7564 | 7566 |
| T0030 | SELL | 6750.00 | 6756.95 | 6729.50 | 6729.50 | [TP] TP | +20.50 | $+102.50 | 2.95 | 7609 | 7641 |
| T0031 | SELL | 6750.00 | 6756.95 | 6719.75 | 6756.95 | [SL] SL | -6.95 | $-34.75 | 4.35 | 7654 | 7656 |


### Operaciones Canceladas (9 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Bar | Razón |
|----------|-----|-------|----|----|----------|-----------|-------|
| T0009 | SELL | 6730.13 | 6733.13 | 6698.50 | 10.54 | 5696 | BOS contradictorio |
| T0010 | SELL | 6730.13 | 6733.13 | 6702.00 | 9.38 | 5697 | BOS contradictorio |
| T0011 | SELL | 6730.13 | 6733.13 | 6702.00 | 9.38 | 5701 | BOS contradictorio |
| T0012 | SELL | 6728.50 | 6731.70 | 6712.75 | 4.92 | 5713 | BOS contradictorio |
| T0013 | SELL | 6728.50 | 6731.70 | 6712.75 | 4.92 | 5718 | BOS contradictorio |
| T0020 | SELL | 6776.25 | 6782.45 | 6758.00 | 2.94 | 6632 | BOS contradictorio |
| T0019 | SELL | 6775.00 | 6782.45 | 6755.75 | 2.58 | 6628 | BOS contradictorio |
| T0006 | SELL | 6714.75 | 6719.95 | 6701.50 | 2.55 | 5653 | BOS contradictorio |
| T0003 | SELL | 6522.00 | 6525.45 | 6518.55 | 1.00 | 3822 | BOS contradictorio |


### Operaciones Expiradas (9 total)

**Top 10 por R:R potencial:**

| Trade ID | Dir | Entry | SL | TP | R:R Plan | Entry Bar | Razón |
|----------|-----|-------|----|----|----------|-----------|-------|
| T0022 | SELL | 6781.50 | 6784.50 | 6759.25 | 7.42 | 6789 | score decayó a 0 |
| T0025 | SELL | 6792.75 | 6797.70 | 6766.25 | 5.35 | 6817 | score decayó a 0 |
| T0005 | SELL | 6658.50 | 6662.70 | 6639.75 | 4.46 | 5251 | score decayó a 0 |
| T0015 | SELL | 6779.50 | 6782.50 | 6766.25 | 4.42 | 6488 | score decayó a 0 |
| T0017 | SELL | 6772.50 | 6782.45 | 6755.75 | 1.68 | 6596 | score decayó a 0 |
| T0004 | SELL | 6666.75 | 6681.45 | 6643.00 | 1.62 | 5211 | score decayó a 0 |
| T0024 | SELL | 6775.75 | 6782.45 | 6766.25 | 1.42 | 6807 | score decayó a 0 |
| T0027 | SELL | 6792.50 | 6797.70 | 6786.25 | 1.20 | 6822 | score decayó a 0 |
| T0028 | SELL | 6595.25 | 6598.70 | 6591.80 | 1.00 | 7007 | score decayó a 0 |


## KPI 2.2: Razones de Cancelación y Expiración

### Cancelaciones (9 total)

| Razón | Cantidad | % |
|-------|----------|---|
| BOS contradictorio | 9 | 100.0% |


### Expiraciones (9 total)

| Razón | Cantidad | % |
|-------|----------|---|
| score decayó a 0 | 9 | 100.0% |




## KPI 2.3: Desglose de Contribuciones del DFM

**Análisis de 610 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.1496 | 34.0% |
| **Proximity** | 0.1511 | 34.4% |
| **Confluence** | 0.1492 | 34.0% |
| **Type** | 0.0000 | 0.0% |
| **Bias** | 0.0457 | 10.4% |
| **Momentum** | 0.0000 | 0.0% |
| **TOTAL (Avg Confidence)** | 0.4395 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 610
- **Señales generadas:** 93 (15.2%)
- **Señales rechazadas (WAIT):** 517 (84.8%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **Proximity**: 0.1511
2. **CoreScore**: 0.1496
3. **Confluence**: 0.1492
4. **Bias**: 0.0457
5. **Type**: 0.0000
6. **Momentum**: 0.0000

**Recomendaciones de calibración:**

- ⚠️ **CoreScore bajo (0.1496)**: Las estructuras base tienen poca calidad
  - Acción: Revisar detectores o aumentar `Weight_CoreScore`
- ⚠️ **Bias muy bajo (0.0457)**: El sesgo de mercado no está contribuyendo
  - Acción: Revisar `ContextManager` o aumentar `Weight_Bias`

---

# 🎓 CONCLUSIONES Y RECOMENDACIONES

## Diagnóstico

### Rentabilidad

- ⚠️ **ADVERTENCIA:** Win Rate bajo (40.0% < 50%)
- **Acción sugerida:** Calibrar pesos del DFM

- ⚠️ **ADVERTENCIA:** Profit Factor bajo (1.46 < 1.5)


## Próximos Pasos

1. **Análisis Profundo de Operaciones Perdedoras:**
   - Activar `ShowScoringBreakdown = true`
   - Ejecutar Fast Load
   - Analizar scoring de las 6 operaciones con SL_HIT

2. **Calibración del DFM:**
   - Revisar pesos: CoreScore, Proximity, Bias, Confluence
   - Ajustar basándose en análisis científico

3. **Optimización de Gestión de Riesgo:**
   - Revisar R:R promedio: 2.46
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-10-26 17:01:32*
