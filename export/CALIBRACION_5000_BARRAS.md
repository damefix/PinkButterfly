# 🎯 CALIBRACIÓN PARA BACKTEST DE 5000 BARRAS

## 📅 Fecha: 26 de Octubre de 2025

---

## 🎯 OBJETIVO

Aumentar la frecuencia de señales manteniendo la calidad del sistema, mediante:
1. Reducción del umbral de confianza
2. Rebalanceo de pesos del DFM
3. Reducción de I/O para mejorar velocidad
4. Backtest largo para validación estadística

---

## ✅ CAMBIOS APLICADOS EN `EngineConfig.cs`

### 1️⃣ **UMBRAL DE CONFIANZA**

```csharp
// ANTES:
MinConfidenceForEntry = 0.65;

// DESPUÉS:
MinConfidenceForEntry = 0.55;
```

**Impacto esperado:** ~2-3x más señales generadas

---

### 2️⃣ **PESOS DEL DECISION FUSION MODEL (DFM)**

```csharp
// ANTES:
Weight_CoreScore    = 0.30  // Calidad estructural
Weight_Proximity    = 0.35  // Cercanía al precio
Weight_Confluence   = 0.10  // Confluencia de estructuras
Weight_Type         = 0.10  // Tipo de estructura (OB, FVG, etc.)
Weight_Bias         = 0.15  // Alineación con tendencia
Weight_Momentum     = 0.00  // Momentum (BOS/CHoCH)
Weight_Volume       = 0.00  // Volumen
// SUMA = 1.00 ✅

// DESPUÉS:
Weight_CoreScore    = 0.40  // +0.10 → Más peso a calidad estructural
Weight_Proximity    = 0.25  // -0.10 → Menos penalización por distancia
Weight_Confluence   = 0.05  // -0.05 → Redundante con Fusión Jerárquica
Weight_Type         = 0.10  // Sin cambio
Weight_Bias         = 0.20  // +0.05 → Más alineación con tendencia
Weight_Momentum     = 0.00  // Sin cambio
Weight_Volume       = 0.00  // Sin cambio
// SUMA = 1.00 ✅
```

**Justificación:**
- **CoreScore ↑:** La Fusión Jerárquica ya crea zonas de alta calidad, darles más peso
- **Proximity ↓:** Permitir señales más alejadas del precio (pullbacks)
- **Confluence ↓:** Ya está implícito en la Fusión Jerárquica (evitar doble conteo)
- **Bias ↑:** Priorizar señales alineadas con la tendencia global

---

### 3️⃣ **BACKTEST LARGO**

```csharp
// ANTES:
BacktestBarsForAnalysis = 2000;  // ~14 días en 15m

// DESPUÉS:
BacktestBarsForAnalysis = 5000;  // ~35 días en 15m
```

**Objetivo:** Obtener 15-30 operaciones para validación estadística

---

### 4️⃣ **REDUCCIÓN DE I/O (PERFORMANCE)**

```csharp
// ANTES:
ProgressReportEveryNBars = 100;   // Reporte cada 100 barras
StateSaveIntervalSecs = 30;       // Guardar JSON cada 30 segundos

// DESPUÉS:
ProgressReportEveryNBars = 500;   // Reporte cada 500 barras
StateSaveIntervalSecs = 300;      // Guardar JSON cada 5 minutos
```

**Impacto:**
- Menos spam en logs
- Menos guardados de JSON (~50 MB cada uno)
- Mejora de velocidad del backtest

---

## 📊 RESULTADOS ESPERADOS

### **Backtest Anterior (2000 barras):**
- Señales Generadas: 15
- Operaciones Ejecutadas: 3
- Win Rate: 66.67% (2/3)
- Profit Factor: 4.67
- Resultado Neto: +$55 (MES) / +$550 (ES)

### **Backtest Nuevo (5000 barras) - PROYECCIÓN:**
- Señales Generadas: ~40-50 (con umbral 0.55)
- Operaciones Ejecutadas: ~15-25
- Win Rate esperado: 55-65%
- Profit Factor esperado: 2.5-4.0
- Resultado Neto esperado: +$200-400 (MES) / +$2,000-4,000 (ES)

---

## 🎯 MÉTRICAS A VALIDAR

1. **Frecuencia de Señales:**
   - ¿Aumentó 2-3x como esperado?
   - ¿Es suficiente para trading activo?

2. **Calidad de Señales:**
   - ¿Se mantuvo el Win Rate > 50%?
   - ¿Se mantuvo el Profit Factor > 2.0?

3. **Gestión de Riesgo:**
   - ¿Cuántas órdenes canceladas por BOS?
   - ¿Cuántas órdenes expiradas por Score Decay?

4. **Rentabilidad:**
   - ¿Resultado neto positivo?
   - ¿Drawdown máximo aceptable?

---

## 📝 PRÓXIMOS PASOS

1. **Compilar** el proyecto en NinjaTrader
2. **Cargar** el indicador `ExpertTrader` en un gráfico de 15m
3. **Esperar** a que procese las 5000 barras (~2-3 horas)
4. **Analizar** el log de resultados
5. **Decidir** si:
   - Mantener estos parámetros (si resultados son buenos)
   - Ajustar nuevamente (si necesita más calibración)
   - Probar en tiempo real (si validación es exitosa)

---

## ⚠️ NOTAS IMPORTANTES

- El JSON se sobrescribirá automáticamente (no borrar manualmente)
- El backtest puede tardar 2-3 horas en completarse
- Los nuevos paneles mostrarán estadísticas en tiempo real
- Fast Load estará disponible después de este backtest

---

## 📁 ARCHIVOS MODIFICADOS

- `src/Core/EngineConfig.cs` ✅
- `src/Visual/ExpertTrader.cs` ✅ (paneles actualizados)

**Archivos en export/ listos para copiar a NinjaTrader.**

