# 📊 KPI Suite Completa - Versión 2
**PinkButterfly CoreBrain - Análisis de Backtest**

**Fecha:** 2025-11-18 08:09:43  
**CSV File:** `..\..\NinjaTrader 8\PinkButterfly\logs\trades_20251118_080021.csv`  
**Trades Analizados:** 34

---

# 🎯 RESUMEN EJECUTIVO

## Operaciones

| Métrica | Valor |
|---------|-------|
| **Operaciones Registradas** | 34 |
| **Operaciones Ejecutadas (Cerradas)** | 0 |
| **Operaciones Canceladas** | 0 |
| **Operaciones Expiradas** | 0 |
| **Operaciones Pendientes** | 34 |

## Rentabilidad

| Métrica | Valor |
|---------|-------|
| **Win Rate** | 0.0% (0/0) |
| **Profit Factor** | 0.00 |
| **P&L Total (Puntos)** | +0.00 |
| **P&L Total (USD)** | $+0.00 |
| **Gross Profit** | $0.00 |
| **Gross Loss** | $0.00 |
| **Avg Win** | $0.00 |
| **Avg Loss** | $0.00 |
| **Avg R:R (Planned)** | 0.00 |

---

# 💰 ANÁLISIS DE RENTABILIDAD

## Trade Book (Libro de Operaciones)

### Operaciones Cerradas (0 total)

| Trade ID | Dir | Entry | SL | TP | Exit | Resultado | P&L (pts) | P&L ($) | R:R Plan | Entry Date | Exit Date |
|----------|-----|-------|----|----|------|-----------|-----------|---------|----------|------------|-----------|


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

**Análisis de 445 evaluaciones de HeatZones**

### Contribuciones Promedio por Componente

| Componente | Contribución Promedio | % del Total |
|------------|----------------------|-------------|
| **CoreScore** | 0.2000 | 27.1% |
| **Proximity** | 0.2347 | 31.8% |
| **Confluence** | 0.0600 | 8.1% |
| **Type** | 0.0133 | 1.8% |
| **Bias** | 0.1951 | 26.5% |
| **Momentum** | 0.0360 | 4.9% |
| **TOTAL (Avg Confidence)** | 0.7373 | 100% |

### Resumen de Señales

- **Evaluaciones totales:** 445
- **Señales generadas:** 445 (100.0%)
- **Señales rechazadas (WAIT):** 0 (0.0%)

### Diagnóstico de Calibración

**Componentes ordenados por contribución:**

1. **Proximity**: 0.2347
2. **CoreScore**: 0.2000
3. **Bias**: 0.1951
4. **Confluence**: 0.0600
5. **Momentum**: 0.0360
6. **Type**: 0.0133

**Recomendaciones de calibración:**


---

# 🎓 CONCLUSIONES Y RECOMENDACIONES

## Diagnóstico

### Rentabilidad

- ⚠️ **CRÍTICO:** Win Rate muy bajo (0.0% < 30%)
- **Problema:** El sistema está generando señales de baja calidad
- **Acción requerida:** Revisar pesos del DFM y criterios de entrada

- ⚠️ **CRÍTICO:** Profit Factor < 1.0 (sistema perdedor: 0.00)
- **Problema:** Las pérdidas superan las ganancias
- **Acción requerida:** 
  1. Revisar R:R de las operaciones
  2. Analizar cancelaciones por BOS
  3. Aumentar `MinConfidenceForEntry`


## Próximos Pasos

1. **Análisis Profundo de Operaciones Perdedoras:**
   - Activar `ShowScoringBreakdown = true`
   - Ejecutar Fast Load
   - Analizar scoring de las 0 operaciones con SL_HIT

2. **Calibración del DFM:**
   - Revisar pesos: CoreScore, Proximity, Bias, Confluence
   - Ajustar basándose en análisis científico

3. **Optimización de Gestión de Riesgo:**
   - Revisar R:R promedio: 0.00
   - Ajustar `MaxSLDistanceATR` y `MinTPDistanceATR`

4. **Nuevo Backtest:**
   - Aplicar cambios de calibración
   - Ejecutar con `BacktestBarsForAnalysis = 5000`
   - Comparar resultados

---

*Reporte generado automáticamente por el analizador de DFM v2.0*  
*Fecha: 2025-11-18 08:09:43*
