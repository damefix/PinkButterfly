# TESTS ADICIONALES RECOMENDADOS
**Fecha**: 2025-10-30  
**Estado Actual**: 249/251 tests pasando (99.2%)

Esta es la propuesta de tests adicionales para alcanzar cobertura 100% y validar edge cases críticos.

---

## **🔴 PRIORIDAD CRÍTICA: RiskCalculator Edge Cases**

### **TEST 1: RiskCalculator_SL_Fallback_WithGuardrails**
**Objetivo**: Validar que SL fallback respeta configuración Y guardarraíles

```csharp
private void Test_RiskCalculator_SL_Fallback_WithGuardrails()
{
    var config = EngineConfig.LoadDefaults();
    config.SL_BufferATR = 0.5; // 50% del ATR
    config.MaxSLDistanceATR = 10.0; // Máximo 10 ATR
    config.MinSLDistanceATR = 2.0; // Mínimo 2 ATR
    
    // Test Case 1: SL fallback dentro de límites → ACEPTAR
    // ATR=20, entry=5000, SL_fallback=5000-(0.5*20)=4990
    // Distancia=10 puntos = 0.5 ATR → Dentro de [2, 10] ATR
    
    // Test Case 2: SL fallback excede MaxSLDistanceATR → RECHAZAR
    // config.SL_BufferATR = 12.0 → SL=5000-(12*20)=4760
    // Distancia=240 puntos = 12 ATR > 10 ATR → RECHAZAR
    
    // Test Case 3: SL fallback menor que MinSLDistanceATR → RECHAZAR
    // config.SL_BufferATR = 0.1 → SL=5000-(0.1*20)=4998
    // Distancia=2 puntos = 0.1 ATR < 2 ATR → RECHAZAR
}
```

**Valida**:
- SL usa `config.SL_BufferATR` (no hardcoded)
- Snapping a tick
- Guardarraíl `MaxSLDistanceATR`
- Guardarraíl `MinSLDistanceATR`
- Metadata `RejectReason` cuando viola límites

---

### **TEST 2: RiskCalculator_TP_Fallback_Coherence**
**Objetivo**: Validar que TP fallback es coherente con dirección

```csharp
private void Test_RiskCalculator_TP_Fallback_Coherence()
{
    var config = EngineConfig.LoadDefaults();
    config.MinRiskRewardRatio = 1.5;
    
    // Test Case 1: BUY con TP fallback coherente
    // Entry=5000, SL=4940, riskDistance=60
    // TP_fallback = 5000 + (60 * 1.5) = 5090 ✅
    // TP > entry → COHERENTE
    
    // Test Case 2: SELL con TP fallback coherente
    // Entry=5000, SL=5060, riskDistance=60
    // TP_fallback = 5000 - (60 * 1.5) = 4910 ✅
    // TP < entry → COHERENTE
    
    // Verificar:
    // - TP != 0
    // - R:R real ≈ MinRiskRewardRatio (±0.01)
    // - RiskCalculated = true
}
```

**Valida**:
- TP fallback calculado correctamente
- Coherencia con dirección (BUY: TP>entry, SELL: TP<entry)
- R:R resultante >= MinRiskRewardRatio

---

### **TEST 3: RiskCalculator_RejectIncoherentTP**
**Objetivo**: Validar rechazo cuando TP es imposible/incoherente

```csharp
private void Test_RiskCalculator_RejectIncoherentTP()
{
    // Escenario: Simular código buggeado que retorna TP=0
    // (actualmente esto pasa cuando coreEngine=null)
    
    var config = EngineConfig.LoadDefaults();
    config.MinRiskRewardRatio = 1.5;
    
    var zone = new HeatZone
    {
        Id = "HZ_BUY",
        Direction = "Bullish",
        High = 5010,
        Low = 5000,
        TFDominante = 60
    };
    zone.Metadata["ProximityFactor"] = 1.0;
    
    var snapshot = new DecisionSnapshot();
    snapshot.HeatZones = new List<HeatZone> { zone };
    
    var riskCalc = new RiskCalculator();
    riskCalc.Initialize(config, logger);
    riskCalc.Process(snapshot, barData, null, 10, 60, 100000);
    
    // ESPERADO: Zona debe ser RECHAZADA
    bool riskCalculated = (bool)zone.Metadata["RiskCalculated"];
    Assert(riskCalculated == false, "RejectIncoherentTP_Rejected");
    
    string rejectReason = (string)zone.Metadata["RejectReason"];
    Assert(rejectReason.Contains("TP incoherente") || rejectReason.Contains("R:R"), 
           "RejectIncoherentTP_Reason");
}
```

**Valida**:
- Sistema rechaza operación cuando TP es imposible
- Metadata incluye `RejectReason` descriptivo
- No se generan operaciones con R:R negativo

---

## **⚠️ PRIORIDAD MEDIA: LiquidityGrab Scoring**

### **TEST 4: LG_Score_ConfirmedBonus_Applied**
**Objetivo**: Validar que confirmación aumenta score

```csharp
private void Test_LG_Score_ConfirmedBonus_Applied()
{
    var provider = new MockBarDataProvider();
    var config = EngineConfig.LoadDefaults();
    config.EnableAutoPurge = false;
    var logger = new TestLogger(_print) { MinLevel = LogLevel.Error };
    var engine = new CoreEngine(provider, config, logger);
    
    // Crear LG y capturar score inicial (no confirmado)
    // ... (simular sweep + reversal)
    var lgUnconfirmed = engine.GetLiquidityGrabs(60)[0];
    double scoreUnconfirmed = lgUnconfirmed.Score;
    
    // Confirmar el LG (simular barras sin re-break hasta timeout)
    // ... (avanzar barras hasta confirmación)
    
    var lgConfirmed = engine.GetLiquidityGrabs(60)[0];
    double scoreConfirmed = lgConfirmed.Score;
    
    // ESPERADO: Score aumenta después de confirmación
    Assert(scoreConfirmed > scoreUnconfirmed, "LG_ConfirmedBonus_Applied",
           $"Before: {scoreUnconfirmed:F3}, After: {scoreConfirmed:F3}");
    
    // ESPERADO: Bonus entre 0.10 y 0.20 es razonable
    double bonus = scoreConfirmed - scoreUnconfirmed;
    Assert(bonus >= 0.10 && bonus <= 0.25, "LG_ConfirmedBonus_Magnitude",
           $"Bonus: {bonus:F3}");
}
```

**Valida**:
- Score aumenta con confirmación
- Bonus es de magnitud razonable
- Confirmación es un factor positivo en scoring

---

## **📝 PRIORIDAD BAJA: Boundary Testing**

### **TEST 5: DFM_MinConfluenceForEntry_ExactBoundary**
**Objetivo**: Validar filtro de confluencia en límite exacto

```csharp
private void Test_DFM_MinConfluenceForEntry_ExactBoundary()
{
    // Test Case 1: Justo en el límite → ACEPTA
    var config1 = EngineConfig.LoadDefaults();
    config1.MinConfluenceForEntry = 0.60;
    config1.MaxConfluenceReference = 5;
    
    var zone1 = new HeatZone { ConfluenceCount = 3 }; // 3/5 = 0.60
    // Procesar con DFM
    // ESPERADO: Zona ACEPTADA (0.60 >= 0.60)
    
    // Test Case 2: Justo debajo del límite → RECHAZA
    var config2 = EngineConfig.LoadDefaults();
    config2.MinConfluenceForEntry = 0.61;
    
    var zone2 = new HeatZone { ConfluenceCount = 3 }; // 3/5 = 0.60
    // Procesar con DFM
    // ESPERADO: Zona RECHAZADA (0.60 < 0.61)
    
    // Validar metadata DFM_RejectReason
}
```

**Valida**:
- Filtro funciona correctamente en límites exactos
- Metadata documenta rechazo por confluencia baja

---

### **TEST 6: RiskCalculator_SL_SnappingTick_Precision**
**Objetivo**: Validar que snapping a tick funciona correctamente

```csharp
private void Test_RiskCalculator_SL_SnappingTick_Precision()
{
    // Configurar tickSize=0.25 (ES futures)
    var provider = new MockBarDataProvider(tickSize: 0.25, "ES");
    
    // Test Case 1: SL=4997.13 → debe snap a 4997.00 (BUY: redondear abajo)
    // Test Case 2: SL=4997.38 → debe snap a 4997.25
    // Test Case 3: SL=4997.88 → debe snap a 4997.75
    
    // Validar que:
    // - SL siempre es múltiplo de tickSize
    // - BUY: redondea abajo (conservador)
    // - SELL: redondea arriba (conservador)
}
```

**Valida**:
- Snapping preciso a tick
- Comportamiento conservador (no optimista)

---

## **📊 IMPACTO ESPERADO**

| Tests Nuevos | Cobertura Adicional | Tiempo Estimado |
|--------------|---------------------|-----------------|
| Test 1-3 (RiskCalculator) | +10% en RiskCalculator | 2-3 horas |
| Test 4 (LG Scoring) | +5% en LG Detector | 1 hora |
| Test 5-6 (Boundary) | +2% general | 30 min |
| **TOTAL** | **~17% adicional** | **~4 horas** |

**Resultado Final Esperado**: **257/257 tests (100%)** con cobertura exhaustiva ✅

---

## **🎯 RECOMENDACIÓN**

**Fase Actual**: Implementar **Tests 1-3** (RiskCalculator) INMEDIATAMENTE antes de arreglar bugs #1 y #2.

**Beneficio**:
- Tests documentan comportamiento esperado ANTES del fix
- Desarrollo guiado por tests (TDD)
- Validación automática de que el fix funciona

**Fase Posterior**: Tests 4-6 para cobertura 100% después de arreglar bugs críticos.

