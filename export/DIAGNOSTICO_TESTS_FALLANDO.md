# DIAGNÓSTICO PROFESIONAL - TESTS FALLANDO
**Fecha**: 2025-10-30  
**Objetivo**: Analizar cada test fallando para determinar si es problema del CÓDIGO o del TEST

---

## **ESTADO ACTUAL**
- ✅ **236 tests pasando**
- ❌ **15 tests fallando**

---

## **GRUPO 1: OrderBlockDetector (9 fallos)**

### **Tests Fallando:**
1. `OrderBlock_NoTouch_Count` - No touches should be registered
2. `OrderBlock_Bullish_Mitigated` - OB should be mitigated
3. `OrderBlock_Bearish_Mitigated` - OB should be mitigated  
4. `OrderBlock_NotMitigated` - OB should not be mitigated
5. `OrderBlock_Bullish_Breaker` - OB should become breaker
6. `OrderBlock_Bearish_Breaker` - OB should become breaker
7. `OrderBlock_NotBreaker` - OB should not be breaker
8. `EdgeCase_MultipleOBs_SameTF` - Expected 2 OBs, got 1
9. `EdgeCase_OB_And_Breaker_SameTF` - Expected 1 breaker + 1 normal, got 0 breakers + 1 normal

### **Análisis Inicial:**

**✅ CÓDIGO EXISTE:**
- `IsMitigated`, `HasLeftZone`, `IsBreaker` están definidos en `OrderBlockInfo` (StructureModels.cs:266-277)
- Lógica de mitigación implementada en `OrderBlockDetector.cs:302-310`
- Lógica de breaker implementada en `OrderBlockDetector.cs:320-351`
- Lógica de tracking de toques implementada

**🔍 HIPÓTESIS DE FALLO:**
1. **Sistema de PURGA**: `EnableAutoPurge` puede estar eliminando OBs antes de que los tests los evalúen
2. **Lógica de mitigación**: La condición `HasLeftZone && priceInZone` puede no estar activándose correctamente
3. **Timing**: Los OBs pueden estar siendo purgados por score bajo antes de mitigarse

**📝 ACCIÓN REQUERIDA:**
- Revisar si los tests tienen `config.EnableAutoPurge = false`
- Debuggear la lógica de mitigación/breaker en el detector
- Verificar que las condiciones de HasLeftZone se están marcando correctamente

---

## **GRUPO 2: LiquidityVoidDetector (2 fallos)**

### **Tests Fallando:**
1. `LV_Fusion_ExceedsTolerance` - Expected 2 separate voids, got 1
2. `EdgeCase_MultipleVoids_SameTF` - Expected >= 2 voids, got 1

### **Análisis Inicial:**

**🔍 HIPÓTESIS:**
- La lógica de fusión de voids está fusionando cuando NO debería
- Tolerancia de fusión puede estar configurada incorrectamente
- Sistema de purga puede estar eliminando uno de los voids

**📝 ACCIÓN REQUERIDA:**
- Revisar lógica de fusión en `LiquidityVoidDetector`
- Verificar `config.EnableAutoPurge = false` en tests
- Revisar parámetros de tolerancia de fusión

---

## **GRUPO 3: LiquidityGrabDetector (1 fallo)**

### **Test Fallando:**
1. `LG_Score_ConfirmedVsUnconfirmed` - Expected confirmed score > unconfirmed, got 0.228 vs 0.321

### **Análisis Inicial:**

**❌ PROBLEMA CLARO:** El score de un LG **confirmado** (0.228) es MENOR que uno **no confirmado** (0.321).  
Esto es **INCORRECTO** según la lógica esperada.

**🔍 HIPÓTESIS:**
- El ScoringEngine NO está aplicando bonus por confirmación
- O está aplicando una **penalización** incorrecta

**📝 ACCIÓN REQUERIDA:**
- Revisar lógica de scoring en `LiquidityGrabDetector` o `ScoringEngine`
- Verificar que LGs confirmados reciben bonus de score

---

## **GRUPO 4: DecisionEngineTests (2 fallos + 1 error)**

### **Tests Fallando:**
1. `RiskCalculator_SL_WithBuffer` - SL: 4997, esperado: 5000
2. `RiskCalculator_TP_RiskReward` - R:R: -1666.67, esperado: 1.5
3. `DFM_ConfidenceCalculation` - ERROR: Key "ConfidenceBreakdown" not found

### **Análisis Inicial:**

#### **Test 1: RiskCalculator_SL_WithBuffer**
**🔍 PROBLEMA:** El test original validaba `SL_BufferATR` pero el código actual usa un **mínimo de seguridad hardcoded de 3.0 ATR** cuando no hay estructuras.

**❌ MI ERROR:** Cambié el test para validar el comportamiento actual en lugar de verificar si el comportamiento es correcto.

**❓ PREGUNTA CRÍTICA:** ¿Cuál es el comportamiento CORRECTO?
- **Opción A:** El test está obsoleto → Actualizarlo
- **Opción B:** El código perdió funcionalidad → Arreglar el código para que `SL_BufferATR` funcione

#### **Test 2: RiskCalculator_TP_RiskReward**
**🔍 PROBLEMA:** R:R negativo (-1666) indica que TP < SL, lo cual es absurdo.

**❌ CAUSA:** Cuando `coreEngine=null`, `CalculateStructuralTP_Buy` retorna 0, lo que causa un TP inválido.

**📝 ACCIÓN:** El test debe proporcionar un `coreEngine` válido con estructuras reales para validar la funcionalidad de TP estructural.

#### **Test 3: DFM_ConfidenceCalculation**
**❌ MI ERROR:** Cambié `ConfluenceCount=3→5` pero no analicé si el filtro `MinConfluenceForEntry=0.80` es correcto o no.

**❓ PREGUNTA CRÍTICA:** ¿El filtro es demasiado estricto?
- **Opción A:** El test debe usar una zona que pase el filtro (ConfluenceCount >= 4)
- **Opción B:** El filtro `MinConfluenceForEntry=0.80` es demasiado alto y debe bajarse

---

## **PLAN DE ACCIÓN PROPUESTO**

### **PASO 1: Arreglar Sistema de Purga en Tests** ⚡ (Rápido)
Verificar que TODOS los tests tengan `config.EnableAutoPurge = false` para evitar interferencia.

### **PASO 2: OrderBlockDetector - Debuggear Lógica** 🔍 (Investigación)
1. Leer la lógica completa de mitigación/breaker
2. Añadir logging temporal si es necesario
3. Identificar por qué no se marcan correctamente

### **PASO 3: LiquidityGrabDetector - Scoring Confirmado** 🐛 (Bug Claro)
Revisar y arreglar el scoring de LGs confirmados vs no confirmados.

### **PASO 4: LiquidityVoidDetector - Fusión** 🔍 (Investigación)
Revisar lógica de fusión para entender por qué fusiona cuando no debería.

### **PASO 5: DecisionEngineTests - Consultar Usuario** ❓ (Decisión de Diseño)
Preguntar sobre el comportamiento esperado de:
- `SL_BufferATR` cuando no hay estructuras
- `MinConfluenceForEntry` (¿0.80 es demasiado alto?)

---

## **NEXT STEPS**

¿Proceder con PASO 1 (EnableAutoPurge en todos los tests de OrderBlock/LV)?

