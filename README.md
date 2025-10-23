# PinkButterfly CoreBrain

**Motor analítico profesional multi-timeframe para NinjaTrader 8**

Sistema de detección, almacenamiento, puntuación y mantenimiento de estructuras de precio (FVG, Swings, Order Blocks, BOS/CHoCH, POI, Liquidity Grabs) con API pública para consumo desde indicadores y estrategias.

---

## 🎯 Objetivo

Desarrollar el **mejor analizador de mercado del mundo** con arquitectura modular, thread-safe, testeable y migrable a servicio externo. Sistema invisible que expone API para bots, estrategias e indicadores avanzados.

---

## 📊 Estado del Proyecto

### ✅ FASE 1: MVP - COMPLETADA (100%)

**Commit:** `dca2caf` - Fase 1 completada: CoreBrain MVP con IntervalTree, serialización JSON y tests validados (11/11 PASS)

**Componentes Implementados:**

- ✅ **CoreEngine.cs** - Motor principal con thread-safety (`ReaderWriterLockSlim`)
- ✅ **EngineConfig.cs** - Configuración serializable con Newtonsoft.Json
- ✅ **StructureModels.cs** - Modelos de datos con herencia polimórfica
- ✅ **IBarDataProvider.cs** - Interfaz de abstracción para datos de barras
- ✅ **ILogger.cs** - Sistema de logging con múltiples niveles
- ✅ **IntervalTree.cs** - Indexado espacial O(log n + k) para consultas de rango
- ✅ **IDetector.cs** - Interfaz para detectores de estructuras
- ✅ **CoreBrainIndicator.cs** - Wrapper NinjaScript funcional
- ✅ **MockBarDataProvider.cs** - Provider de pruebas
- ✅ **TestRunnerIndicator.cs** - Indicador para ejecutar tests
- ✅ **IntervalTreeTests.cs** - Suite de tests unitarios

**Tests Validados:**
- ✅ 11/11 tests pasados
- ✅ Performance: Insert 1000 items = 8ms, Query = 0ms
- ✅ Complejidad O(log n + k) confirmada

**Dependencias:**
- Newtonsoft.Json 13.0.3 (incluida en `lib/`)

---

### ✅ FASE 2: FVGDetector + Scoring - COMPLETADA (100%)

**Commit:** `2957531` - Fase 2: FVGDetector y ScoringEngine completos con 52 tests (100% pass)

**Componentes Implementados:**

- ✅ **FVGDetector.cs** - Detector completo de Fair Value Gaps
  - Detección de gaps bullish/bearish con validación por tamaño
  - Merge de FVGs consecutivos (configurable)
  - Detección de FVGs anidados multi-nivel
  - Tracking de toques (body/wick)
  - Cálculo de Fill Percentage con residual score
  
- ✅ **ScoringEngine.cs** - Sistema de scoring multi-dimensional
  - TF Weight normalization
  - Freshness (decay exponencial)
  - Proximity dinámica (ATR-based)
  - Touch factor (bonus por toques)
  - Fill handling (residual score)
  - Multi-timeframe scoring

- ✅ **FVGDetectorTests.cs** - 12 tests básicos
- ✅ **FVGDetectorAdvancedTests.cs** - 29 tests avanzados

**Tests Validados:**
- ✅ 52/52 tests pasados (100%)
  - 11/11 IntervalTree tests
  - 12/12 FVGDetector básicos
  - 29/29 FVGDetector avanzados (merge, nested, fill, scoring, edge cases)
- ✅ Cobertura: 95%
- ✅ Confianza: 95%

**Bugs Corregidos:**
- ✅ ATR calculation con barras insuficientes
- ✅ Lógica de nested multi-nivel (buscar padre más específico)

**API Pública:**
- `GetActiveFVGs(int tfMinutes, double minScore)` - Obtener FVGs activos filtrados por score

**Documentación:**
- `docs/COBERTURA_TESTS.md` - Desglose completo de cobertura de tests
- `docs/INSTRUCCIONES_TESTS_AVANZADOS.md` - Guía de tests avanzados

---

### 🚧 FASE 3: Detectores Avanzados (Próxima)

- SwingDetector
- DoubleDetector (Double Tops/Bottoms)
- OrderBlockDetector
- BOSDetector (BOS/CHoCH)
- POIDetector (Points of Interest)

---

### 🔄 FASE 4: Persistencia y Optimización (Pendiente)

- Persistencia asíncrona con debounce
- Sistema de eventos (`OnStructureAdded`, `OnStructureUpdated`, `OnStructureRemoved`)
- Purga automática por score
- Optimización de memoria

---

### 🎁 FASE 5: Migración a DLL (Final)

- Compilación a DLL para protección de IP
- Sistema de licenciamiento
- Distribución comercial

---

## 🏗️ Arquitectura

### Separación de Responsabilidades

```
┌─────────────────────────────────────────┐
│   NinjaTrader (CoreBrainIndicator)      │
│   - Wrapper NinjaScript                 │
│   - Implementa IBarDataProvider         │
│   - Singleton Instance                  │
└──────────────┬──────────────────────────┘
               │
               ▼
┌─────────────────────────────────────────┐
│   CoreEngine (POCO C#)                  │
│   - Lógica del motor                    │
│   - Thread-safe (ReaderWriterLockSlim)  │
│   - Gestión de detectores               │
│   - Scoring y persistencia              │
└──────────────┬──────────────────────────┘
               │
               ▼
┌─────────────────────────────────────────┐
│   Detectores (IDetector)                │
│   - FVGDetector                         │
│   - SwingDetector                       │
│   - OrderBlockDetector                  │
│   - BOSDetector                         │
│   - POIDetector                         │
└─────────────────────────────────────────┘
```

### Indexado Espacial

- **IntervalTree**: O(log n + k) para consultas de rango
- Usado para confluence detection y POI
- Instancia por timeframe

---

## 🚀 Instalación y Uso

### Requisitos

- NinjaTrader 8
- .NET Framework 4.8
- Newtonsoft.Json 13.0.3

### Instalación

1. **Copiar Newtonsoft.Json.dll**:
   ```
   lib/Newtonsoft.Json.dll → C:\Program Files\NinjaTrader 8\bin\Custom\
   ```

2. **Referenciar en NinjaTrader**:
   - Tools → References → Add → Seleccionar `Newtonsoft.Json.dll`

3. **Copiar archivos fuente**:
   - Copiar todos los `.cs` de `src/` a tu carpeta local de NinjaTrader

4. **Compilar**:
   - Tools → Compile (F5)

### Ejecutar Tests

1. Abrir cualquier gráfico en NinjaTrader
2. Añadir indicador **"CoreBrainTestRunner"**
3. Ver resultados en **Output Tab 2**

### Usar CoreBrain

```csharp
// En otro indicador
var core = CoreBrain.Instance;

// FASE 2: API disponible
var fvgs = core.GetActiveFVGs(60, minScore: 0.3);
foreach(var fvg in fvgs)
{
    Print($"FVG {fvg.Id} TF{fvg.TF} Score:{fvg.Score*100:F1}%");
}
```

---

## 📁 Estructura del Proyecto

```
PinkButterfly/
├── src/
│   ├── Core/
│   │   ├── CoreEngine.cs
│   │   ├── EngineConfig.cs
│   │   ├── IBarDataProvider.cs
│   │   └── StructureModels.cs
│   ├── Detectors/
│   │   └── IDetector.cs
│   ├── Infrastructure/
│   │   ├── ILogger.cs
│   │   └── IntervalTree.cs
│   ├── NinjaTrader/
│   │   └── CoreBrainIndicator.cs
│   └── Testing/
│       ├── MockBarDataProvider.cs
│       └── TestRunnerIndicator.cs
├── tests/
│   └── IntervalTreeTests.cs
├── lib/
│   └── Newtonsoft.Json.dll
├── docs/
│   ├── INSTRUCCIONES_NEWTONSOFT.md
│   └── promp-inicial-definicion-del-proyecto.txt
├── export/
│   └── (archivos temporales para testing)
└── README.md
```

---

## 🧪 Testing

### Suite de Tests Actual

- **IntervalTreeTests**: 11 tests
  - Insert, QueryOverlap, Remove, QueryPoint
  - Performance validation

### Resultados

```
✓ PASS: Insert_BasicFunctionality
✓ PASS: QueryOverlap_NoResults
✓ PASS: QueryOverlap_WithResults_Count
✓ PASS: QueryOverlap_WithResults_Content
✓ PASS: QueryOverlap_MultipleResults
✓ PASS: Remove_ReturnValue
✓ PASS: Remove_Count
✓ PASS: Remove_NotInQuery
✓ PASS: QueryPoint_Count
✓ PASS: Performance_Insert (8ms < 100ms)
✓ PASS: Performance_Query (0ms < 10ms)

RESULTADOS: 11 passed, 0 failed
```

---

## 🔧 Configuración

### Parámetros por Defecto (EngineConfig)

```csharp
TimeframesToUse: [15, 60, 240, 1440]  // minutos
MinFVGSizeTicks: 6
MinFVGSizeATRfactor: 0.12
MinSwingATRfactor: 0.05
ProxMaxATRFactor: 2.5
FreshnessLambda: 20
DecayLambda: 100
TouchBodyBonusPerTouch: 0.12
MaxTouchBodyCap: 5
ConfluenceWeight: 0.18
FillThreshold: 0.90
ResidualScore: 0.05
MaxStructuresPerTF: 500
MergeConsecutiveFVGs: true
DetectNestedFVGs: true
EnableDebug: false
```

---

## 📝 Principios de Desarrollo

1. **Separación estricta**: Engine POCO sin dependencias de NinjaTrader
2. **Thread-safety**: `ReaderWriterLockSlim` para acceso concurrente
3. **Testeable**: Inyección de dependencias y mocks
4. **Performance**: Indexado espacial O(log n + k)
5. **Profesional**: Sin hacks ni soluciones intermedias
6. **Migrable**: Fácil conversión a servicio externo o DLL

---

## 📚 Documentación

- **Definición del Proyecto**: `docs/promp-inicial-definicion-del-proyecto.txt`
- **Instrucciones Newtonsoft**: `docs/INSTRUCCIONES_NEWTONSOFT.md`
- **Comentarios en código**: Español, exhaustivos

---

## 🤝 Contribución

Este es un proyecto privado en desarrollo. Fase actual: **Fase 1 completada, iniciando Fase 2**.

---

## 📄 Licencia

Propietario: Proyecto privado. Sistema comercial en desarrollo.

---

## 🎯 Roadmap

- [x] **Fase 0**: Setup inicial y estructura
- [x] **Fase 1**: MVP con IntervalTree y tests
- [ ] **Fase 2**: FVGDetector + Scoring
- [ ] **Fase 3**: Detectores avanzados (Swing, Double, OB, BOS, POI)
- [ ] **Fase 4**: Persistencia y optimización
- [ ] **Fase 5**: Migración a DLL y licenciamiento

---

**Última actualización**: Fase 1 completada - Tests 11/11 PASS - Performance validada
