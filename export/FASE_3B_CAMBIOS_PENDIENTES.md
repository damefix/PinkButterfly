# 🚧 FASE 3b - CAMBIOS ESTRUCTURALES EN PROGRESO

**Fecha**: 30 Octubre 2025  
**Estado**: IMPLEMENTACIÓN PARCIAL  
**Progreso**: 30% completado

---

## ✅ COMPLETADO

### **1. StructureFusion - Anchor-First Logic**
- ✅ Tolerancia de overlap 2x para anchors
- ✅ Gating: Requerir anchor O alta confluencia (≥3)
- ✅ Dirección "Anchor-First" con multiplica

dor x5 si hay anchors fuertes
- ✅ Logging diagnóstico extenso por zona

**Archivos modificados:**
- `src/Decision/StructureFusion.cs` (líneas 87-170)

---

### **2. RiskCalculator SL - Función de Ranking**
- ✅ Función `ScoreSLCandidate()` implementada
  - 40% score intrínseco
  - 30% peso TF
  - 30% proximidad a banda 6-10 ATR

**Archivos modificados:**
- `src/Decision/RiskCalculator.cs` (líneas 1027-1050)

---

## ⏳ PENDIENTE DE COMPLETAR

### **3. RiskCalculator SL - Usar Ranking en Find**

**CAMBIO NECESARIO en `FindProtectiveSwingLowBanded` (línea ~1090)**:

```csharp
// ACTUAL: Recolecta candidatos en lista simple
candidates.Add(new Tuple<SwingInfo,int,double>(s, tf, slDistAtr));

// CAMBIAR A: Recolectar con ranking score
var candidatesWithRank = new List<Tuple<SwingInfo,int,double,double>>(); // (swing, tf, slDistATR, rankScore)

// En el loop de candidatos:
double rankScore = ScoreSLCandidate(s, entry, atr, tf);
candidatesWithRank.Add(new Tuple<SwingInfo,int,double,double>(s, tf, slDistATR, rankScore));

// DESPUÉS del loop: Ordenar por ranking score
if (candidatesWithRank.Count > 0)
{
    var best = candidatesWithRank.OrderByDescending(c => c.Item4).First();
    
    // LOGGING DIAGNÓSTICO
    _logger.Info(string.Format("[DIAG][SL] Candidates={0} Best=[TF:{1}, Score:{2:F2}, Rank:{3:F2}, Dist:{4:F2}ATR] Fallback=false",
        candidatesWithRank.Count, best.Item2, best.Item1.Score, best.Item4, best.Item3));
    
    return new Tuple<SwingInfo,int>(best.Item1, best.Item2);
}
```

**MISMO CAMBIO necesario en `FindProtectiveSwingHighBanded` (para SELL)**

---

### **4. RiskCalculator TP - Función de Ranking**

**AÑADIR nueva función** (después de `ScoreSLCandidate`):

```csharp
private double ScoreTPCandidate(StructureBase structure, double entry, double atr, int tf)
{
    // Componente 1: Calidad intrínseca (50% del peso)
    double scoreComponent = structure.Score * 0.5;
    
    // Componente 2: Peso de TF (30% del peso)
    double tfWeight = _config.TFWeights.ContainsKey(tf) ? _config.TFWeights[tf] : 0.5;
    double tfComponent = tfWeight * 0.3;
    
    // Componente 3: Distancia razonable (20% del peso)
    // Penalizar TPs muy lejanos (>20 ATR)
    double tpDistATR = Math.Abs(structure.Low - entry) / atr;
    double distPenalty = tpDistATR > 20 ? 0.5 : 1.0;
    double distComponent = distPenalty * 0.2;
    
    return scoreComponent + tfComponent + distComponent;
}
```

---

### **5. RiskCalculator TP - P1/P2 Alcanzables**

**CAMBIO en `CalculateStructuralTP_Buy` (línea ~713)**:

```csharp
// ACTUAL:
var structureTarget = FindOpposingStructure_Above(zone, coreEngine, barData, 0.7);

// CAMBIAR A (relajar umbral):
var structureTarget = FindOpposingStructure_Above(zone, coreEngine, barData, 0.35);
```

**CAMBIO en `FindSwingHigh_Above` (ampliar ventana)**:

```csharp
// Aumentar distancia máxima de búsqueda
double maxSearchDistance = 25 * atr; // Era ~15 ATR
```

**CAMBIO en `FindSwingHigh_Above` y `FindSwingLow_Below` (usar ranking)**:

Similar al SL, recolectar candidatos con ranking y ordenar por `ScoreTPCandidate`.

---

### **6. Logging Diagnóstico Extenso**

**AÑADIR en `CalculateStructuralRiskLevels` (después de calcular SL)**:

```csharp
_logger.Info(string.Format("[DIAG][SL] Candidates={0} Best=[TF:{1}, Score:{2:F2}, Rank:{3:F2}, Dist:{4:F2}ATR] Fallback={5}",
    candidateCount, selectedTF, selectedScore, rankScore, slDistATR, isFallback));
```

**AÑADIR en `CalculateStructuralTP_Buy/Sell` (logging de prioridades)**:

```csharp
_logger.Info(string.Format("[DIAG][TP] P1={0} P2={1} P3={2} Selected=[Priority:{3}, TF:{4}, Score:{5:F2}, Dist:{6:F2}ATR]",
    p1Found, p2Found, p3Found, priority, selectedTF, selectedScore, tpDistATR));
```

---

### **7. Ajustar Parámetros en EngineConfig.cs**

```csharp
// MinScores
MinSLScore: 0.25 → 0.35
MinTPScore: 0.15 → 0.25
MinSLDistanceATR: 0.0 → 1.0
MinConfluenceForEntry: 0.50 → 0.60

// MaxAgeForSL_ByTF
{ 5, 120 },    // Era 200
{ 15, 80 },    // Era 100
{ 60, 60 },    // Era 50 (OK)
{ 240, 40 },   // OK
{ 1440, 20 }   // OK

// MaxAgeForTP_ByTF
{ 5, 100 },    // Era 200
{ 15, 60 },    // Era 100
{ 60, 50 },    // OK
{ 240, 40 },   // OK
{ 1440, 20 }   // OK

// HeatZone_OverlapToleranceATR
0.5 → 1.0  // Para permitir más overlap de anchors
```

---

## 📋 RESUMEN DE TRABAJO RESTANTE

1. **RiskCalculator.cs**: Modificar `FindProtectiveSwingLowBanded` y `FindProtectiveSwingHighBanded` para usar ranking (~50 líneas)
2. **RiskCalculator.cs**: Añadir `ScoreTPCandidate` y modificar `FindSwingHigh_Above`, `FindSwingLow_Below`, `FindOpposingStructure_Above` (~100 líneas)
3. **RiskCalculator.cs**: Añadir logging diagnóstico en 3-4 puntos clave (~30 líneas)
4. **EngineConfig.cs**: Ajustar 8 parámetros (~10 líneas)
5. **Copiar archivos** a NinjaTrader
6. **Backtest** y validación de criterios de éxito

**Estimación tiempo restante**: 1-1.5 horas

---

## 🎯 CRITERIOS DE ÉXITO (Recordatorio)

| Métrica | Target |
|---------|--------|
| Anchors en zonas | ≥35% |
| SL Estructural | ≥30% |
| TP Estructural | ≥30% |
| SL Fallback | ≤50% |
| TP Fallback | ≤30% |
| Score avg SL | ≥0.35 |
| Score avg TP | ≥0.25 |
| Win Rate | ≥50% |
| Profit Factor | ≥1.3 |
| SL Dist ATR | 6-10 ATR concentrado |

---

**Estado**: Necesita completarse antes de backtest  
**Siguiente paso**: Completar cambios en RiskCalculator y EngineConfig

