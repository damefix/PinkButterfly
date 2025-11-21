# 🎯 PLAN DE IMPLEMENTACIÓN: FRESHNESS ADAPTATIVO Y MEMORIA ESTRUCTURAL

**Fecha:** 2025-11-20  
**Versión:** 1.0 FINAL  
**Estado:** ✅ Aprobado para implementación

---

## 📋 CONTEXTO Y OBJETIVOS

### Problema Actual
- **Constantes hardcodeadas** no respetan TF, tipo ni régimen de mercado
- **Decay uniforme** degrada memoria estructural sin distinción
- **Swings históricos válidos** (NO rotos) pierden score artificialmente
- **Límites fijos** purgan estructuras importantes prematuramente
- **Score de estructuras >3 días** ≈ 0% por decay exponencial doble

### Solución Propuesta
- **Fase 1:** Swings NO rotos sin decay (permanentes como S/R)
- **Fase 2:** Freshness adaptativo por TF/tipo/efectiveAge
- **Fase 3:** Límites dinámicos por actividad + purga por calidad (retention)

### Impacto Esperado
- Swings 15m NO rotos: permanentes hasta ruptura
- Swings 60m: duración 3-4x mayor que 15m
- FVG intradía: decay más rápido (lógico)
- BOS históricos: se mantienen si son relevantes
- Menos operaciones con "score decayó a 0"

---

## 🔧 FASE 1: SWINGS NO ROTOS SIN DECAY + LÍMITE BOS

### Objetivos
- ✅ Swings NO rotos: `freshness = 1.0` (permanente)
- ✅ Aumentar límite BOS: 50 → 200 por TF
- ✅ Telemetría explícita para validación

### Cambios en `CoreEngine.cs`

#### ANCLA 1: Scoring dinámico (línea ~2238-2245)

**ANTES:**
```csharp
// Calcular freshness (edad de la estructura) para la fórmula rápida
double freshness = CalculateFreshness(age, tfMinutes, barIndex);
// PESOS DINÁMICOS: ajustar freshness/proximity según cercanía al precio
// Lejos del precio → más peso a freshness (S/R potencial)
```

**DESPUÉS:**
```csharp
// Calcular freshness (edad de la estructura) para la fórmula rápida
double freshness;
if (_config.FreshnessNoDecayForUnbrokenSwings && structure is SwingInfo sw1 && !sw1.IsBroken)
{
    freshness = 1.0;
    if (_config.EnablePerfDiagnostics)
        _logger.Info($"[FRESH_ADAPT] TF={tfMinutes} Type=SWING age={age} fresh=1.00 reason=UnbrokenSwing");
}
else
{
    freshness = CalculateFreshness(age, tfMinutes, barIndex);
}
// PESOS DINÁMICOS: ajustar freshness/proximity según cercanía al precio
// Lejos del precio → más peso a freshness (S/R potencial)
```

#### ANCLA 2: HTF ajuste por freshness (línea ~2471-2472)

**ANTES:**
```csharp
double fresh = CalculateFreshness(ageBars, tfMinutes, idxTF);
double adjusted = s.Score * fresh;
```

**DESPUÉS:**
```csharp
double fresh;
if (_config.FreshnessNoDecayForUnbrokenSwings && s is SwingInfo sw2 && !sw2.IsBroken)
{
    fresh = 1.0;
    if (_config.EnablePerfDiagnostics)
        _logger.Info($"[FRESH_ADAPT] TF={tfMinutes} Type=SWING age={ageBars} fresh=1.00 reason=UnbrokenSwing");
}
else
{
    fresh = CalculateFreshness(ageBars, tfMinutes, idxTF);
}
double adjusted = s.Score * fresh;
```

#### ANCLA 3: Gate de confluencia HTF (línea ~2509-2511)

**ANTES:**
```csharp
double fresh = CalculateFreshness(age, tf, idxTF);
double adj = s.Score * fresh;
```

**DESPUÉS:**
```csharp
double fresh;
if (_config.FreshnessNoDecayForUnbrokenSwings && s is SwingInfo sw3 && !sw3.IsBroken)
{
    fresh = 1.0;
    if (_config.EnablePerfDiagnostics)
        _logger.Info($"[FRESH_ADAPT] TF={tf} Type=SWING age={age} fresh=1.00 reason=UnbrokenSwing");
}
else
{
    fresh = CalculateFreshness(age, tf, idxTF);
}
double adj = s.Score * fresh;
```

### Cambios en `EngineConfig.cs`

**ANTES:**
```csharp
public int MaxStructuresByType_BOS { get; set; } = 50;
```

**DESPUÉS:**
```csharp
public int MaxStructuresByType_BOS { get; set; } = 200;
```

**NUEVO (añadir cerca de otros toggles/decay):**
```csharp
/// <summary>
/// Swings NO rotos no sufren decay de freshness (permanentes como S/R)
/// </summary>
public bool FreshnessNoDecayForUnbrokenSwings { get; set; } = true;
```

### Compilación y Validación Fase 1

**PowerShell:**
```powershell
Copy-Item "pinkbutterfly-produccion\CoreEngine.cs" "C:\Users\meste\Documents\NinjaTrader 8\bin\Custom\Indicators\PinkButterfly\CoreEngine.cs" -Force
Copy-Item "pinkbutterfly-produccion\EngineConfig.cs" "C:\Users\meste\Documents\NinjaTrader 8\bin\Custom\Indicators\PinkButterfly\EngineConfig.cs" -Force
```

**NinjaTrader:** F5 (compilar)

**Validación:**
1. ✅ **Determinismo:** 2 backtests idénticos → resultados exactos
2. ✅ **Log:** Buscar `[FRESH_ADAPT] ... reason=UnbrokenSwing`
3. ✅ **Swings activos:** De ~23 → >100 esperado
4. ✅ **BOS visibles:** De ~25 → >50 esperado
5. ✅ **Score >3 días:** De ~0% → >60% esperado
6. ✅ **Menos "score decayó a 0"** en expiraciones

---

## 🔧 FASE 2: FRESHNESS ADAPTATIVO POR TF/TIPO

### Objetivos
- ✅ Lambdas base por TF (5m, 15m, 60m, 240m, 1440m)
- ✅ Multiplicadores por tipo (Swing, BOS, FVG, OB, Liquidity, POI)
- ✅ EffectiveAge = min(ageSinceCreated, ageSinceLastTouch)
- ✅ Feature flag: `EnableAdaptiveFreshness` (OFF por defecto)
- ✅ Sin volAdj (volatilidad ya actúa en proximity)

### Cambios en `EngineConfig.cs`

**NUEVO (añadir):**
```csharp
// ============================================================
// FASE 2: FRESHNESS ADAPTATIVO
// ============================================================

/// <summary>Habilita freshness adaptativo por TF/tipo (Fase 2)</summary>
public bool EnableAdaptiveFreshness { get; set; } = false;

/// <summary>Freshness mínimo (floor) para evitar score=0</summary>
public double FreshnessFloor { get; set; } = 0.05;

// Multiplicadores por tipo
public double FreshnessMult_SwingUnbroken { get; set; } = 4.0;
public double FreshnessMult_BOS { get; set; } = 2.0;
public double FreshnessMult_OB { get; set; } = 0.9;
public double FreshnessMult_FVG_Intraday { get; set; } = 0.5;  // TF < 60
public double FreshnessMult_FVG_HTF { get; set; } = 0.8;       // TF >= 60
public double FreshnessMult_Liquidity { get; set; } = 0.6;
public double FreshnessMult_POI { get; set; } = 1.2;

// Ajustar memoria macro (opcional)
// public int DecayBasePeriod_240m { get; set; } = 480;  // aumentar si es necesario
// public int DecayBasePeriod_1440m { get; set; } = 300;
```

### Cambios en `CoreEngine.cs`

#### ANCLA X: Helpers adaptativos (DESPUÉS del cierre de `CalculateFreshness`)

**Localizar:** Buscar `return Math.Max(0.01, freshness);` (cierre de `CalculateFreshness`)  
**Añadir inmediatamente después:**

```csharp
/// <summary>
/// Calcula edad efectiva: mínimo entre edad desde creación y edad desde último toque
/// Fase 2: Estructuras tocadas recientemente mantienen relevancia
/// </summary>
private int GetEffectiveAgeBars(StructureBase s, int tfMinutes, int barIndex)
{
    int ageSinceCreated = barIndex - s.CreatedAtBarIndex;
    int lastTouchOrUpdate = s.LastUpdatedBarIndex > 0 ? s.LastUpdatedBarIndex : s.CreatedAtBarIndex;
    int ageSinceLastTouch = barIndex - lastTouchOrUpdate;
    int effective = Math.Min(ageSinceCreated, ageSinceLastTouch);
    return Math.Max(0, effective);
}

/// <summary>
/// Lambda adaptativo: baseLambda_TF * multiplicador_tipo
/// Fase 2: Sin volAdj (volatilidad ya actúa en proximity)
/// </summary>
private double GetAdaptiveFreshnessLambda(StructureBase s, int tfMinutes)
{
    int baseLambdaTF = tfMinutes switch
    {
        5 => _config.DecayBasePeriod_5m,
        15 => _config.DecayBasePeriod_15m,
        60 => _config.DecayBasePeriod_60m,
        240 => _config.DecayBasePeriod_240m,
        1440 => _config.DecayBasePeriod_1440m,
        _ => 120
    };

    double typeMult =
        s is SwingInfo sw && !sw.IsBroken ? _config.FreshnessMult_SwingUnbroken :
        s is StructureBreakInfo ? _config.FreshnessMult_BOS :
        s is OrderBlockInfo ? _config.FreshnessMult_OB :
        s is FairValueGapInfo ? (s.TF >= 60 ? _config.FreshnessMult_FVG_HTF : _config.FreshnessMult_FVG_Intraday) :
        s is LiquidityGrabInfo || s is LiquidityVoidInfo ? _config.FreshnessMult_Liquidity :
        s is PointOfInterestInfo ? _config.FreshnessMult_POI : 1.0;

    return Math.Max(1.0, baseLambdaTF * typeMult);
}

/// <summary>
/// Freshness adaptativo: exp(-effectiveAge / lambda_adaptativo)
/// Fase 2: Respeta flags de Fase 1 (swings no rotos) y añade adaptación por TF/tipo
/// </summary>
private double GetAdaptiveFreshness(StructureBase s, int tfMinutes, int barIndex)
{
    // Fase 1: swings no rotos siempre fresh=1.0
    if (_config.FreshnessNoDecayForUnbrokenSwings && s is SwingInfo sw && !sw.IsBroken)
        return 1.0;

    int effectiveAge = GetEffectiveAgeBars(s, tfMinutes, barIndex);
    double lambda = GetAdaptiveFreshnessLambda(s, tfMinutes);
    double f = Math.Exp(-effectiveAge / Math.Max(1.0, lambda));
    double clamped = Math.Max(_config.FreshnessFloor, Math.Min(1.0, f));

    if (_config.EnablePerfDiagnostics)
        _logger.Info($"[FRESH_ADAPT] TF={tfMinutes} Type={s.Type} age={effectiveAge} lambda={lambda:F1} fresh={clamped:F3}");

    return clamped;
}
```

#### Modificar las 3 ANCLAS de Fase 1

**Sustituir el cálculo de `freshness`/`fresh` en las 3 anclas:**

**ANCLA 1 (línea ~2238):**
```csharp
double freshness = _config.EnableAdaptiveFreshness
    ? GetAdaptiveFreshness(structure, tfMinutes, barIndex)
    : ((_config.FreshnessNoDecayForUnbrokenSwings && structure is SwingInfo sw1 && !sw1.IsBroken)
        ? 1.0
        : CalculateFreshness(age, tfMinutes, barIndex));
```

**ANCLA 2 (línea ~2471):**
```csharp
double fresh = (_config.FreshnessNoDecayForUnbrokenSwings && s is SwingInfo sw2 && !sw2.IsBroken)
    ? 1.0
    : (_config.EnableAdaptiveFreshness
        ? GetAdaptiveFreshness(s, tfMinutes, idxTF)
        : CalculateFreshness(ageBars, tfMinutes, idxTF));
```

**ANCLA 3 (línea ~2509):**
```csharp
double fresh = (_config.FreshnessNoDecayForUnbrokenSwings && s is SwingInfo sw3 && !sw3.IsBroken)
    ? 1.0
    : (_config.EnableAdaptiveFreshness
        ? GetAdaptiveFreshness(s, tf, idxTF)
        : CalculateFreshness(age, tf, idxTF));
```

### Compilación y Validación Fase 2

**PowerShell:** (mismos comandos que Fase 1)

**Validación (flag OFF primero):**
1. ✅ `EnableAdaptiveFreshness = false` → backtest idéntico a Fase 1
2. ✅ Activar `EnableAdaptiveFreshness = true`
3. ✅ **Log:** `[FRESH_ADAPT]` con lambdas diferentes por tipo
4. ✅ **FVG intradía:** Decay más rápido (lambda ~50)
5. ✅ **Swings 60m:** Score más estable (lambda ~960)
6. ✅ **BOS:** Lambda intermedio (~200)

---

## 🔧 FASE 3: LÍMITES DINÁMICOS + PURGA POR CALIDAD

### Objetivos
- ✅ Límites dinámicos: base + actividad reciente × alpha
- ✅ Ceiling proporcional: base × 3.0 (seguridad)
- ✅ Purga por retention: 0.5×freshness + 0.3×proximity + 0.2×touchBonus
- ✅ Tie-breakers deterministas (TF, CreatedAt, Low, High)
- ✅ Feature flags: `EnableDynamicLimits`, `EnableRetentionPurge`

### Cambios en `EngineConfig.cs`

**NUEVO (añadir):**
```csharp
// ============================================================
// FASE 3: LÍMITES DINÁMICOS Y PURGA POR CALIDAD
// ============================================================

/// <summary>Habilita límites dinámicos por actividad reciente (Fase 3)</summary>
public bool EnableDynamicLimits { get; set; } = false;

/// <summary>Factor de incremento por actividad (límite += createdInWindow × alpha)</summary>
public double DynamicLimitAlpha { get; set; } = 0.5;

/// <summary>Ceiling relativo (límite dinámico máximo = base × multiplicador)</summary>
public double DynamicLimitMaxMultiplier { get; set; } = 3.0;

/// <summary>Habilita purga por retention score en vez de score puro (Fase 3)</summary>
public bool EnableRetentionPurge { get; set; } = false;
```

### Cambios en `CoreEngine.cs`

#### ANCLA B: Helper de límite dinámico (ANTES de `PurgeByTypeLimit`)

**Añadir método nuevo:**
```csharp
/// <summary>
/// Calcula límite dinámico por tipo basado en actividad reciente
/// Fase 3: límite se ajusta automáticamente en mercados activos
/// </summary>
private int GetDynamicMaxByType(string type, int tfMinutes, int barIndex, int baseLimit)
{
    // Ventana de actividad: 3× el periodo de decay del TF
    int windowBars = Math.Max(50,
        3 * (tfMinutes switch
        {
            5 => _config.DecayBasePeriod_5m,
            15 => _config.DecayBasePeriod_15m,
            60 => _config.DecayBasePeriod_60m,
            240 => _config.DecayBasePeriod_240m,
            1440 => _config.DecayBasePeriod_1440m,
            _ => 120
        }));

    // Contar estructuras creadas en la ventana
    int createdInWindow = _structuresListByTF[tfMinutes]
        .Count(s => s.Type == type && (barIndex - s.CreatedAtBarIndex) <= windowBars);

    // Límite dinámico = base + actividad × alpha
    double dynamic = baseLimit + (createdInWindow * _config.DynamicLimitAlpha);
    
    // Ceiling proporcional al base (seguridad contra bugs)
    double ceil = baseLimit * _config.DynamicLimitMaxMultiplier;
    
    return (int)Math.Max(baseLimit, Math.Min(dynamic, ceil));
}
```

#### ANCLA A: Uso de límite dinámico en `PurgeByTypeLimit`

**Localizar (línea ~3434-3439):**
```csharp
string type = group.Key;
int count = group.Count();
int maxForType = GetMaxStructuresByType(type);
if (count > maxForType)
{
    int countToPurge = count - maxForType;
```

**DESPUÉS:**
```csharp
string type = group.Key;
int count = group.Count();
int baseForType = GetMaxStructuresByType(type);
int maxForType = baseForType;

// Fase 3: aplicar límite dinámico si está habilitado
if (_config.EnableDynamicLimits)
{
    maxForType = GetDynamicMaxByType(type, tfMinutes, barIndex, baseForType);
    _logger.Info($"[PURGE_LIMITS] TF={tfMinutes} Type={type} base={baseForType} dyn={maxForType}");
}

if (count > maxForType)
{
    int countToPurge = count - maxForType;
```

#### ANCLA C: Purga por retention con tie-breakers

**Localizar ordenamiento actual (línea ~3442-3451):**
```csharp
var ordered = group
    .OrderBy(s => s.Score)
    .ThenBy(s => s.TF)
    .ThenByDescending(s => s.CreatedAtBarIndex)
    .ThenByDescending(s => s.StartTime)
    .ThenBy(s => s.Low)
    .ThenBy(s => s.High)
    .ThenBy(s => s.Type, StringComparer.Ordinal)
    .ToList();
```

**DESPUÉS:**
```csharp
List<StructureBase> ordered;

// Fase 3: purga por retention score si está habilitado
if (_config.EnableRetentionPurge)
{
    ordered = group
        .Select(s =>
        {
            // 1. Freshness adaptativo
            double fresh = (_config.EnableAdaptiveFreshness
                ? GetAdaptiveFreshness(s, tfMinutes, barIndex)
                : CalculateFreshness(barIndex - s.CreatedAtBarIndex, tfMinutes, barIndex));

            // 2. Proximity factor
            double proximityFactor = 0.0;
            try
            {
                double currentPrice = _provider.GetClose(tfMinutes, barIndex);
                double atr = Math.Max(1e-9, _provider.GetATR(tfMinutes, 14, barIndex));
                double entryPrice = (s is SwingInfo sw && sw.IsHigh) ? sw.High :
                                    (s is SwingInfo sw2 && !sw2.IsHigh) ? sw2.Low :
                                    (s.High + s.Low) * 0.5;
                proximityFactor = 1.0 - Math.Min(1.0, Math.Abs(currentPrice - entryPrice) / (atr * _config.ProximityThresholdATR));
            }
            catch { }

            // 3. Touch bonus (swings con muchos toques → más relevantes)
            double touchBonus = 0.0;
            if (s is SwingInfo ssw)
            {
                int touches = ssw.TouchCount_Body + ssw.TouchCount_Wick;
                touchBonus = Math.Min(0.2, touches * 0.005);
            }

            // Retention score: 50% freshness + 30% proximity + 20% touches
            double retention = (0.5 * fresh) + (0.3 * proximityFactor) + (0.2 * touchBonus);
            return new { S = s, Ret = retention };
        })
        .OrderBy(x => x.Ret)                          // Menor retention primero (purga)
        .ThenBy(x => x.S.TF)                          // Tie-breaker 1: TF
        .ThenByDescending(x => x.S.CreatedAtBarIndex) // Tie-breaker 2: más antiguo primero
        .ThenBy(x => x.S.Low)                         // Tie-breaker 3: Low
        .ThenBy(x => x.S.High)                        // Tie-breaker 4: High
        .Select(x => x.S)
        .ToList();

    _logger.Info($"[PURGE_DECISION] TF={tfMinutes} Type={type} Using retention-based ordering");
}
else
{
    // Purga clásica por score
    ordered = group
        .OrderBy(s => s.Score)
        .ThenBy(s => s.TF)
        .ThenByDescending(s => s.CreatedAtBarIndex)
        .ThenByDescending(s => s.StartTime)
        .ThenBy(s => s.Low)
        .ThenBy(s => s.High)
        .ThenBy(s => s.Type, StringComparer.Ordinal)
        .ToList();
}
```

### Compilación y Validación Fase 3

**PowerShell:** (mismos comandos)

**Validación (flags OFF primero):**
1. ✅ Ambos flags = false → backtest idéntico a Fase 2
2. ✅ Activar `EnableDynamicLimits = true` solo
3. ✅ **Log:** `[PURGE_LIMITS]` con límites dinámicos
4. ✅ **Límites aumentan** en tramos con alta actividad
5. ✅ Activar `EnableRetentionPurge = true`
6. ✅ **Log:** `[PURGE_DECISION] ... Using retention-based ordering`
7. ✅ **Purgas priorizan** estructuras con baja retention
8. ✅ **Determinismo:** 2 runs idénticos

---

## ✅ CHECKLIST COMPLETO POR FASE

### Fase 1
- [ ] Modificar `CoreEngine.cs` (3 anclas)
- [ ] Modificar `EngineConfig.cs` (MaxBOS + flag)
- [ ] Copiar archivos a NinjaTrader
- [ ] Compilar (F5)
- [ ] Backtest × 2 (determinismo)
- [ ] Verificar log: `[FRESH_ADAPT] ... UnbrokenSwing`
- [ ] Comparar métricas: swings activos, BOS visibles, scores >3 días
- [ ] ✅ **APROBADO** antes de continuar Fase 2

### Fase 2
- [ ] Añadir parámetros en `EngineConfig.cs`
- [ ] Añadir 3 helpers en `CoreEngine.cs` (después de CalculateFreshness)
- [ ] Modificar 3 anclas para usar flag adaptativo
- [ ] Copiar archivos
- [ ] Compilar (F5)
- [ ] Backtest con flag OFF → idéntico a Fase 1
- [ ] Backtest con flag ON → verificar lambdas diferentes
- [ ] Verificar log: `[FRESH_ADAPT]` con types/lambdas variados
- [ ] ✅ **APROBADO** antes de continuar Fase 3

### Fase 3
- [ ] Añadir parámetros en `EngineConfig.cs`
- [ ] Añadir helper `GetDynamicMaxByType` en `CoreEngine.cs`
- [ ] Modificar `PurgeByTypeLimit` (ancla A, C)
- [ ] Copiar archivos
- [ ] Compilar (F5)
- [ ] Backtest con flags OFF → idéntico a Fase 2
- [ ] Backtest con `EnableDynamicLimits = true`
- [ ] Verificar log: `[PURGE_LIMITS]`
- [ ] Backtest con `EnableRetentionPurge = true`
- [ ] Verificar log: `[PURGE_DECISION]`
- [ ] Determinismo final: 2 runs idénticos
- [ ] ✅ **IMPLEMENTACIÓN COMPLETA**

---

## 📊 MÉTRICAS DE ÉXITO

### Antes (baseline actual)
- Swings activos: ~23 (solo 7 días)
- BOS visibles: ~25 (solo 3 días)
- Score >3 días: ~0%
- Operaciones 2500 barras: 3 (muy bajo)
- "Score decayó a 0": frecuente

### Después Fase 1
- Swings activos: >100 esperado
- BOS visibles: >50 esperado
- Score >3 días: >60% esperado
- Operaciones: aumento esperado
- "Score decayó a 0": reducción significativa

### Después Fase 2
- FVG 5m: decay rápido (lógico)
- Swings 60m: 4× más duraderos que 15m
- BOS: decay intermedio
- Lambda visible en logs por tipo

### Después Fase 3
- Límites dinámicos: ajuste automático
- Purgas inteligentes: por retention no edad
- Swings con 40+ touches: protegidos
- Estructuras cercanas al precio: protegidas

---

## 🚨 ADVERTENCIAS Y GUARDARRAÍLES

1. **NO saltar fases** - Implementar en orden secuencial
2. **SIEMPRE verificar determinismo** después de cada fase
3. **NO activar flags en producción** sin backtest previo
4. **Guardar versión anterior** antes de cada fase
5. **Si algo falla:** revertir y analizar logs antes de continuar
6. **Tie-breakers son CRÍTICOS** para determinismo en Fase 3
7. **VolAdj fue REMOVIDO** - no añadir de vuelta

---

## 📝 NOTAS FINALES

- **Todas las fases están aprobadas** para implementación
- **Feature flags** permiten rollback inmediato si hay problemas
- **Testing exhaustivo** en cada fase antes de continuar
- **Determinismo es obligatorio** - 2 runs deben dar resultados idénticos
- **Logs son tu amigo** - `[FRESH_ADAPT]`, `[PURGE_LIMITS]`, `[PURGE_DECISION]`

---

**DOCUMENTO LISTO PARA EJECUCIÓN - 2025-11-21**

*Generado automáticamente por PinkButterfly CoreBrain Analysis System*

