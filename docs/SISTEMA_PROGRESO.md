# Sistema de Progreso - PinkButterfly CoreBrain

## 📊 Descripción

El **Sistema de Progreso** proporciona información en tiempo real sobre el procesamiento histórico de barras, permitiendo monitorear el avance, tiempo transcurrido, tiempo estimado restante y rendimiento del sistema.

---

## ✨ Características

- **Barra de progreso visual ASCII** con porcentaje
- **Tiempo transcurrido** y **tiempo estimado restante** (ETA)
- **Velocidad de procesamiento** (barras por minuto)
- **Contador de estructuras detectadas**
- **Contador de guardados realizados**
- **Reportes configurables** (por cantidad de barras o por tiempo)
- **Reporte final** con resumen completo y recomendaciones

---

## 🎯 Ejemplo de Output

### Reporte de Progreso (Durante Procesamiento)

```
╔════════════════════════════════════════════════════════════════╗
║                    PROGRESO DE ANÁLISIS                        ║
╠════════════════════════════════════════════════════════════════╣
║ ████████████████████████████░░░░░░░░░░░░░░░░░░░░░░ 60.0%      ║
║ Progreso: 60.0% | Barra 6,000/10,000                           ║
║ ⏱️  Tiempo: 4h 32m | Restante: ~3h 05m                         ║
║ 🎯 ETA: 23:45:00                                               ║
║ 📊 Estructuras: 9,847 | Guardados: 285                         ║
║ 🚀 Velocidad: 22.0 barras/min                                  ║
╚════════════════════════════════════════════════════════════════╝
```

### Reporte Final (Al Completar)

```
╔════════════════════════════════════════════════════════════════╗
║                  ✅ ANÁLISIS COMPLETADO                        ║
╠════════════════════════════════════════════════════════════════╣
║ ██████████████████████████████████████████████████ 100.0%     ║
║ 📊 Barras procesadas: 10,000                                   ║
║ 🔍 Estructuras detectadas: 16,166                              ║
║ 💾 Guardados realizados: 476                                   ║
║ ⏱️  Tiempo total: 8h 03m                                       ║
║ 🚀 Velocidad promedio: 20.7 barras/min                         ║
║                                                                ║
║ ⚠️  OPTIMIZACIÓN: 476 guardados es excesivo                    ║
║    Recomendado: Reducir frecuencia a ~10 guardados            ║
║    Esto mejorará el rendimiento ~47x                          ║
╚════════════════════════════════════════════════════════════════╝
```

---

## 🔧 Configuración

### Parámetros en `EngineConfig.cs`

```csharp
// Activar/desactivar sistema de progreso
public bool ShowProgressBar { get; set; } = true;

// Reportar cada N barras procesadas
public int ProgressReportEveryNBars { get; set; } = 100;

// Reportar cada X minutos (para procesos lentos)
public int ProgressReportEveryMinutes { get; set; } = 5;

// Mostrar información detallada
public bool ShowDetailedProgress { get; set; } = true;
```

### Valores Recomendados

| Escenario | ProgressReportEveryNBars | ProgressReportEveryMinutes |
|-----------|--------------------------|----------------------------|
| **Desarrollo/Debug** | 50 | 2 |
| **Producción Normal** | 100 | 5 |
| **Procesos Largos** | 500 | 10 |
| **Procesos Muy Largos** | 1000 | 15 |

---

## 💻 Uso en Código

### 1. En el Indicador NinjaTrader (`ExpertTrader.cs`)

```csharp
protected override void OnStateChange()
{
    if (State == State.DataLoaded)
    {
        // Inicializar CoreEngine
        _coreEngine = new CoreEngine(_provider, _config, _logger);
        _coreEngine.Initialize();
        
        // IMPORTANTE: Inicializar sistema de progreso ANTES de procesar histórico
        if (CurrentBar >= 0 && BarsArray[0].Count > 0)
        {
            int totalBars = Math.Min(BarsArray[0].Count, BacktestBarsForAnalysis);
            _coreEngine.StartProgressTracking(totalBars);
        }
    }
    
    if (State == State.Terminated)
    {
        // Finalizar sistema de progreso y mostrar reporte final
        _coreEngine?.FinishProgressTracking();
        _coreEngine?.Dispose();
    }
}
```

### 2. En el CoreEngine (Ya Integrado)

El sistema se actualiza automáticamente en `OnBarClose()`:

```csharp
public void OnBarClose(int tfMinutes, int barIndex)
{
    // Actualizar progreso si el tracker está activo
    if (_progressTracker != null)
    {
        _progressTracker.Update(barIndex);
        
        if (_progressTracker.ShouldReport())
        {
            int structureCount = GetTotalStructureCount();
            _progressTracker.Report(structureCount, _saveCounter);
        }
    }
    
    // ... resto del procesamiento
}
```

---

## 📈 Información Proporcionada

### Durante el Procesamiento

| Métrica | Descripción | Utilidad |
|---------|-------------|----------|
| **Porcentaje** | % de barras procesadas | Saber cuánto falta |
| **Barra Actual/Total** | Posición exacta | Debugging preciso |
| **Tiempo Transcurrido** | Desde inicio | Evaluar rendimiento |
| **Tiempo Restante** | Estimación basada en velocidad | Planificar espera |
| **ETA** | Hora estimada de finalización | Saber cuándo volver |
| **Estructuras** | Total detectadas hasta ahora | Validar detección |
| **Guardados** | Cantidad de saves realizados | Detectar exceso |
| **Velocidad** | Barras por minuto | Identificar lentitud |

### En el Reporte Final

- **Resumen completo** de barras, estructuras y guardados
- **Tiempo total** de procesamiento
- **Velocidad promedio** del sistema
- **Recomendaciones automáticas** si detecta problemas (ej: muchos guardados)

---

## 🚨 Diagnóstico de Problemas

### Velocidad Muy Baja (< 10 barras/min)

**Posibles causas:**
- Demasiados guardados (ver contador)
- Detectores ineficientes
- Configuración de purga deshabilitada
- Demasiadas estructuras acumuladas

**Solución:**
1. Reducir frecuencia de guardado en `PersistenceManager`
2. Habilitar `EnableAutoPurge = true`
3. Ajustar `MaxStructuresPerTF`

### Muchos Guardados (> 50)

**Problema:**
Cada guardado serializa ~150 MB y escribe a disco, lo que puede tomar 1-2 minutos.

**Solución:**
Modificar la frecuencia de guardado en `CoreEngine.cs`:

```csharp
// Guardar cada 1000 barras en lugar de cada pocas barras
if (barCount % 1000 == 0)
{
    await SaveStateToJSONAsync();
}
```

### Proceso Colgado (Velocidad = 0)

**Diagnóstico:**
Si el reporte muestra velocidad = 0 o no avanza:
1. Verificar si el sistema entró en suspensión
2. Revisar Output Tab 2 para errores
3. Verificar uso de CPU/memoria

---

## 🎯 Mejores Prácticas

### 1. Siempre Activar en Procesos Largos

```csharp
if (BacktestBarsForAnalysis > 1000)
{
    _coreEngine.StartProgressTracking(BacktestBarsForAnalysis);
}
```

### 2. Ajustar Frecuencia Según Duración

```csharp
// Para 10,000 barras (8+ horas)
_config.ProgressReportEveryNBars = 500;  // Cada 500 barras
_config.ProgressReportEveryMinutes = 10; // Cada 10 minutos
```

### 3. Deshabilitar en Producción Real-Time

```csharp
// Solo para backtesting, no para trading en vivo
if (State == State.Historical)
{
    _coreEngine.StartProgressTracking(totalBars);
}
```

### 4. Revisar Reporte Final

El reporte final incluye **recomendaciones automáticas**. Si sugiere optimizaciones, aplícalas antes del siguiente procesamiento.

---

## 📊 Ejemplo Completo

```csharp
// En State.DataLoaded
if (State == State.DataLoaded)
{
    // 1. Crear y configurar CoreEngine
    var config = new EngineConfig
    {
        ShowProgressBar = true,
        ProgressReportEveryNBars = 100,
        ProgressReportEveryMinutes = 5
    };
    
    _coreEngine = new CoreEngine(_provider, config, _logger);
    _coreEngine.Initialize();
    
    // 2. Inicializar progreso
    int totalBars = Math.Min(BarsArray[0].Count, BacktestBarsForAnalysis);
    _coreEngine.StartProgressTracking(totalBars);
    
    _logger.Info($"Iniciando análisis de {totalBars} barras...");
}

// En State.Terminated
if (State == State.Terminated)
{
    // 3. Finalizar progreso (muestra reporte final)
    _coreEngine?.FinishProgressTracking();
    
    // 4. Limpiar recursos
    _coreEngine?.Dispose();
}
```

---

## 🔍 Troubleshooting

### No Se Muestra el Progreso

**Verificar:**
1. `ShowProgressBar = true` en config
2. `StartProgressTracking()` fue llamado
3. `totalBars > 0`
4. Logger está funcionando

### Progreso Incorrecto

**Causa:**
El `barIndex` pasado a `OnBarClose()` no corresponde al índice real.

**Solución:**
Asegurar que el índice de barra es secuencial y empieza desde 0.

### Reporte Final No Aparece

**Causa:**
`FinishProgressTracking()` no fue llamado.

**Solución:**
Llamar explícitamente en `State.Terminated`.

---

## 📝 Notas Técnicas

### Thread-Safety

El `ProgressTracker` es **thread-safe** para lectura, pero debe actualizarse desde el mismo thread que procesa las barras.

### Overhead de Performance

El impacto en rendimiento es **mínimo**:
- Actualización: < 0.1 ms por barra
- Reporte: ~50 ms cada 100 barras
- Total: < 0.1% del tiempo de procesamiento

### Compatibilidad

- ✅ Compatible con Fast Load
- ✅ Compatible con modo estático
- ✅ Compatible con backtesting
- ⚠️ No recomendado para trading en vivo (innecesario)

---

## 🚀 Roadmap Futuro

- [ ] Exportar progreso a archivo CSV
- [ ] Gráfico de velocidad en tiempo real
- [ ] Alertas si el proceso se ralentiza
- [ ] Estimación de memoria usada
- [ ] Progreso por timeframe individual

---

## 📚 Referencias

- `src/Core/ProgressTracker.cs` - Implementación del tracker
- `src/Core/CoreEngine.cs` - Integración en el motor
- `src/Core/EngineConfig.cs` - Parámetros de configuración

---

**Última actualización:** 2025-10-25  
**Versión:** 1.0  
**Autor:** PinkButterfly CoreBrain Team


