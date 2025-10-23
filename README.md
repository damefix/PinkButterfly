# 🦋 PinkButterfly - CoreBrain

**Motor Analítico Profesional Multi-Timeframe para NinjaTrader 8**

## 📝 Descripción

PinkButterfly CoreBrain es un sistema de análisis de mercado de nivel institucional que detecta, puntúa y mantiene actualizadas estructuras de precio multi-timeframe utilizando conceptos avanzados de Smart Money Concepts (SMC) y Price Action.

### Características Principales

- **Motor analítico puro** (invisible, sin gráficos)
- **Detección multi-timeframe** de estructuras avanzadas:
  - Fair Value Gaps (FVG)
  - Swings de alta/baja
  - Double Tops/Bottoms
  - Order Blocks (OB)
  - Break of Structure (BOS) / Change of Character (CHoCH)
  - Points of Interest (POI) con confluencias
  - Liquidity Grabs
- **Sistema de scoring dinámico** basado en:
  - Peso del timeframe
  - Frescura temporal (freshness)
  - Proximidad al precio actual
  - Número de toques (body/wick)
  - Confluencias entre estructuras
  - Momentum de rupturas
  - Decay temporal
- **Indexación eficiente** con Interval Tree (O(log n + k))
- **Thread-safe** con `ReaderWriterLockSlim`
- **Persistencia asíncrona** con debounce y validación de configuración
- **API pública** para consumo desde indicadores/estrategias
- **Arquitectura limpia** separada de NinjaTrader (migrable a servicios externos)

---

## 🏗️ Arquitectura

```
┌─────────────────────────────────────────────┐
│   Indicadores / Estrategias Consumidoras   │
│   (Acceden vía API pública)                 │
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│   CoreBrainIndicator (NinjaScript Wrapper)  │
│   • Singleton Instance                      │
│   • IBarDataProvider implementation         │
│   • Multi-TF Bar synchronization            │
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│   CoreEngine (POCO, sin dependencias NT)    │
│   • State management (thread-safe)          │
│   • Detector orchestration                  │
│   • Scoring engine                          │
│   • Persistence manager                     │
└──────────────────┬──────────────────────────┘
                   │
    ┌──────────────┼──────────────┐
    ▼              ▼              ▼
┌─────────┐  ┌──────────┐  ┌──────────┐
│   FVG   │  │  Swing   │  │   OB     │  ... (Detectores)
│Detector │  │ Detector │  │ Detector │
└─────────┘  └──────────┘  └──────────┘
```

### Separación de Responsabilidades

1. **CoreEngine**: Lógica pura C# (POCO) - sin referencias a NinjaTrader
2. **IBarDataProvider**: Interface que abstrae el acceso a datos de barras
3. **CoreBrainIndicator**: Wrapper NinjaScript que implementa `IBarDataProvider`
4. **Detectores**: Componentes inyectables que implementan `IDetector`

---

## 📦 Estructura del Proyecto

```
PinkButterfly/
├── src/
│   ├── Core/                      # Motor principal
│   │   ├── CoreEngine.cs          # Orquestador principal
│   │   ├── EngineConfig.cs        # Configuración completa
│   │   ├── IBarDataProvider.cs    # Interface de datos
│   │   └── StructureModels.cs     # Modelos de datos
│   ├── Detectors/                 # Detectores de estructuras
│   │   ├── IDetector.cs           # Interface base
│   │   ├── FVGDetector.cs         # Fair Value Gaps
│   │   ├── SwingDetector.cs       # Swing highs/lows
│   │   ├── DoubleDetector.cs      # Double tops/bottoms
│   │   ├── OrderBlockDetector.cs  # Order blocks
│   │   ├── BOSDetector.cs         # BOS/CHoCH
│   │   └── POIDetector.cs         # Points of Interest
│   ├── Infrastructure/            # Utilidades
│   │   ├── IntervalTree.cs        # Indexación por rango
│   │   ├── PersistenceManager.cs  # Guardado/carga JSON
│   │   ├── ILogger.cs             # Logging interface
│   │   └── BarSyncManager.cs      # Sincronización MTF
│   ├── NinjaTrader/               # Integración NT8
│   │   ├── CoreBrainIndicator.cs         # Wrapper principal
│   │   └── NinjaTraderBarDataProvider.cs # Implementación provider
│   └── Testing/                   # Testing y diagnósticos
│       ├── MockBarDataProvider.cs # Mock para tests
│       └── Diagnostics.cs         # RunSelfDiagnostics
├── docs/                          # Documentación
│   ├── especificacion-completa.md
│   └── brain_state_example.json
├── tests/                         # Tests unitarios
└── documentacion/                 # Documentación original
```

---

## ⚙️ Configuración por Defecto (EngineConfig)

### Timeframes
- **TimeframesToUse**: `[15, 60, 240, 1440]` (15m, 1H, 4H, Daily)

### Parámetros FVG
- **MinFVGSizeTicks**: `6`
- **MinFVGSizeATRfactor**: `0.12`
- **MergeConsecutiveFVGs**: `true`
- **DetectNestedFVGs**: `true`

### Parámetros Swing
- **MinSwingATRfactor**: `0.05`
- **nLeft**: `2` (barras a la izquierda)
- **nRight**: `2` (barras a la derecha)

### Parámetros Double Top/Bottom
- **priceToleranceTicks_DoubleTop**: `8`
- **MinBarsBetweenDouble**: `3`
- **MaxBarsBetweenDouble**: `200`
- **ConfirmBars_Double**: `3`

### Parámetros Order Block
- **OBBodyMinATR**: `0.6`

### Pesos de Scoring
- **TFWeights**: `{1440: 1.0, 240: 0.7, 60: 0.45, 15: 0.25}`
- **TypeWeights**: `{FVG: 1.0, SWING: 0.8, OB: 0.9, POI: 1.2, BOS: 0.85, DOUBLE_TOP: 0.75}`
- **ProxMaxATRFactor**: `2.5`
- **FreshnessLambda**: `20`
- **DecayLambda**: `100`
- **TouchBodyBonusPerTouch**: `0.12`
- **MaxTouchBodyCap**: `5`
- **ConfluenceWeight**: `0.18`

### Parámetros de Relleno
- **FillThreshold**: `0.90` (90% para considerar estructura "filled")
- **ResidualScore**: `0.05` (score mínimo después de fill)
- **FillPriceStayBars**: `1`

### Parámetros BOS/Momentum
- **BreakMomentumBodyFactor**: `0.6`
- **BreakMomentumMultiplierStrong**: `1.35`
- **BreakMomentumMultiplierWeak**: `1.1`

### Sistema
- **StateSaveIntervalSecs**: `30`
- **MaxStructuresPerTF**: `500`
- **EnableDebug**: `false`

---

## 🚀 Uso Básico

### Desde otro Indicador o Estrategia

```csharp
// Obtener instancia del CoreBrain (Singleton)
var core = CoreBrainIndicator.Instance;

if (core == null)
{
    Print("Error: CoreBrainIndicator no está cargado en el chart");
    return;
}

// Obtener FVGs activos en timeframe de 60 minutos con score mínimo de 0.3
var fvgs = core.GetActiveFVGs(60, minScore: 0.3);

foreach (var fvg in fvgs)
{
    // Score está en rango 0.0 - 1.0
    double scorePercent = fvg.Score * 100.0;
    
    Print($"FVG {fvg.Id} | TF: {fvg.TF} | " +
          $"Score: {scorePercent:F1}% | " +
          $"Direction: {fvg.Direction} | " +
          $"Range: {fvg.Low:F5} - {fvg.High:F5} | " +
          $"Touches Body: {fvg.TouchCount_Body} | " +
          $"Fill: {fvg.FillPercentage:P0}");
}

// Obtener Order Blocks
var orderBlocks = core.GetOrderBlocks(240, minScore: 0.4);

// Obtener Points of Interest (confluencias)
var pois = core.GetPOIs(60);

// Obtener estructura específica por ID
var structure = core.GetStructureById("guid-aqui");
```

### Inicialización del CoreBrain en un Chart

```csharp
// Añadir el indicador CoreBrainIndicator al gráfico
// El indicador no dibuja nada (es invisible)
// Otros indicadores/estrategias accederán a él vía Instance
```

---

## 📊 Sistema de Scoring

El sistema de scoring es multidimensional y dinámico. Cada estructura recibe un score en el rango `0.0 - 1.0` basado en:

### Fórmula de Scoring

```
score = TF_norm × freshness × proximity × typeNorm × touchFactor × confluence × momentumMultiplier
```

Donde:

- **TF_norm**: Peso normalizado del timeframe (Daily > 4H > 1H > 15m)
- **freshness**: `exp(-ageBars / FreshnessLambda)` - Estructuras nuevas puntúan más alto
- **proximity**: `1 - min(distanceTicks / ProxMax, 1.0)` - Cercanía al precio actual
- **typeNorm**: Peso del tipo de estructura (POI > OB > FVG > BOS > SWING > DOUBLE)
- **touchFactor**: `1 + bonus × min(touchCount, cap)` - Bonus por toques con el cuerpo
- **confluence**: `1 + weight × (count - 1)` - Bonus por confluencia con otras estructuras
- **momentumMultiplier**: Multiplica score si alineado con momentum de mercado (BOS/CHoCH)

### Manejo de Estructuras Rellenadas

- Si `FillPercentage >= 0.90` → `score = max(score, ResidualScore)`
- Estructuras rellenadas mantienen un score residual mínimo (0.05 por defecto)
- Esto permite trackear estructuras históricamente importantes

---

## 🔧 Persistencia

### Guardado Automático

- El estado se guarda automáticamente cada **30 segundos** (configurable)
- Guardado asíncrono con **debounce** para evitar I/O excesivo
- Solo se guarda si hay cambios (`_stateChanged = true`)

### Ubicación del Estado

Por defecto: `C:\Users\[User]\Documents\NinjaTrader 8\PinkButterfly\brain_state.json`

### Validación de Configuración

- Cada estado guardado incluye un **hash SHA256** de la configuración
- Al cargar, se valida que el hash coincida
- Si no coincide → requiere `forceLoad=true` (protege contra inconsistencias)

### Formato JSON

Ver ejemplo completo en: `docs/brain_state_example.json`

---

## 🧪 Testing y Diagnósticos

### RunSelfDiagnostics()

Ejecuta escenarios de prueba sintéticos para validar el sistema:

```csharp
var core = CoreBrainIndicator.Instance;
core.RunSelfDiagnostics();
```

**Genera**:
- `diagnostics.json` con resultados detallados
- Output en NinjaTrader Output Window

**Escenarios incluidos**:
- **Caso A**: FVG con solo toques de mecha (no fill)
- **Caso B**: FVG con fill completo (body touches)
- **Caso C**: Order Block con volumen spike
- **Caso D**: BOS con momentum fuerte/débil
- **Caso E**: Double Top con confirmación de neckline

---

## 📈 Performance

### Complejidad Temporal

- **Detección de estructuras**: O(1) por barra
- **Consulta de confluencias**: O(log n + k) con Interval Tree
- **GetActiveFVGs()**: O(n) donde n = estructuras activas (típicamente < 500)

### Optimizaciones

- **Interval Tree** para consultas espaciales eficientes
- **ReaderWriterLockSlim** para concurrencia optimizada (lecturas paralelas)
- **Purga automática** cuando se excede `MaxStructuresPerTF` (500 por defecto)
- **Guardado asíncrono** con debounce para evitar bloqueos

### Benchmarks Target

- **OnBarUpdate()**: < 5ms
- **Query API calls**: < 1ms
- **Memory footprint**: < 50MB con 500 estructuras × 4 TFs

---

## 🛠️ Desarrollo

### Fases de Implementación

1. ✅ **Fase 0**: Estructura de proyecto y GIT
2. ⏳ **Fase 1**: Fundaciones (Modelos, Config, IntervalTree, Engine skeleton)
3. ⏳ **Fase 2**: FVGDetector + Scoring básico
4. ⏳ **Fase 3**: Wrapper NinjaTrader + Integración
5. ⏳ **Fase 4**: Detectores restantes (Swing, Double, OB)
6. ⏳ **Fase 5**: BOS/CHoCH + POI + Persistencia
7. ⏳ **Fase 6**: Optimización + Documentación + Producción

### Compilación en NinjaTrader 8

1. Copiar todos los archivos `.cs` a: `Documents\NinjaTrader 8\bin\Custom\Indicators\`
2. Abrir NinjaTrader 8
3. Tools → New NinjaScript → Compile
4. Resolver errores si los hay
5. Aplicar `CoreBrainIndicator` a un gráfico

---

## 📄 Licencia

Copyright © 2025 - Sistema Propietario

---

## 🐛 Troubleshooting

### "CoreBrainIndicator.Instance es null"

- Asegúrate de que el indicador `CoreBrainIndicator` está aplicado al gráfico
- El indicador debe estar en estado `State.DataLoaded` o superior

### "No se detectan estructuras"

- Verifica que los timeframes configurados tienen suficiente historial de barras
- Ajusta parámetros `MinFVGSizeTicks`, `MinSwingATRfactor` si el mercado es poco volátil
- Activa `EnableDebug = true` en EngineConfig

### Performance lento

- Reduce `MaxStructuresPerTF` (default: 500)
- Reduce número de timeframes en `TimeframesToUse`
- Incrementa `StateSaveIntervalSecs` si I/O es problema

---

## 📞 Contacto

Desarrollado como sistema profesional de análisis de mercado para trading algorítmico.

**Versión**: 1.0.0 (en desarrollo)  
**Estado**: Fase 0 completada - Fundaciones en progreso

