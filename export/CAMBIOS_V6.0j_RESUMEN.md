# CAMBIOS V6.0j - SCORING TP INTELIGENTE + FIX BIAS

## ✅ CAMBIO 1: BIAS SINCRONIZADO (COMPLETADO)

### Archivo: `ContextManager.cs` (línea ~227)

**DESPUÉS de esta línea:**
```csharp
_logger.Info(string.Format(
    "[DIAGNOSTICO][Context] V6.0i Regime={0} BiasComposite={1} Score={2:F2} Threshold={3:F2} EMA20={4:F2} EMA50={5:F2} BOS={6:F2} Reg24h={7:F2}",
    snapshot.MarketRegime, snapshot.GlobalBias, compositeScore, biasThreshold, ema20Score, ema50Score, bosScore, regressionScore
));
```

**AÑADIR:**
```csharp
// ✅ FIX: Actualizar CoreEngine con el bias compuesto calculado
// Esto sincroniza el bias mostrado en el gráfico con el usado en la lógica
coreEngine.UpdateMarketBias(snapshot.GlobalBias);
```

**RESULTADO:** El gráfico mostrará el bias correcto (BEARISH) en lugar del viejo (Bullish).

---

## ✅ CAMBIO 2: SCORING TP INTELIGENTE (IMPLEMENTADO PARCIALMENTE)

### Archivo: `RiskCalculator.cs`

#### 2.1. **Nueva función `CalculateTPIntelligentScore` (línea ~2540)**

Ya añadida en el código.

#### 2.2. **P0 ya NO retorna prematuramente** (línea ~989-1000)

Ya modificado: P0 se añade a `allTPCandidates` en lugar de retornar inmediatamente.

#### 2.3. **PENDIENTE: Modificar el resto de P1, P2, P3, P4**

El código actual (líneas 1002-1243) tiene una jerarquía fija:
- P1 (Liquidity) → return
- P2 (Structure) → return
- P3 (Swing) → return
- P4 (Fallback) → return

**NECESITA CAMBIARSE A:**
- P1, P2, P3, P4 se añaden TODOS a `allTPCandidates`
- Al final, se calcula el score inteligente para cada uno
- Se elige el de mayor score
- Se retorna el ganador

### 📋 PLAN DE IMPLEMENTACIÓN:

Dado que el bloque es muy grande (241 líneas), voy a crear un archivo temporal con el código completo del método `CalculateStructuralTP_Buy` reescrito con la lógica inteligente.

**Archivos a generar:**
1. `export/CalculateStructuralTP_Buy_V6.0j.cs` - Método BUY completo
2. `export/CalculateStructuralTP_Sell_V6.0j.cs` - Método SELL completo (mismo cambio)

**Instrucciones para el usuario:**
1. Abrir `RiskCalculator.cs`
2. Buscar el método `CalculateStructuralTP_Buy`
3. REEMPLAZAR TODO EL MÉTODO por el contenido de `CalculateStructuralTP_Buy_V6.0j.cs`
4. Repetir para `CalculateStructuralTP_Sell`

---

## 📊 CAMBIO 3: INDICADOR DE DIAGNÓSTICO (PENDIENTE)

Ver `export/ExpertTraderDiag_PLAN.md` para el plan completo.

**Requiere:**
1. Copiar `ExpertTrader.cs` → `ExpertTraderDiag.cs`
2. Añadir parsing de logs de señales rechazadas
3. Pintar en AMARILLO las señales rechazadas con sus SL/TP

**Beneficio:** El usuario podrá ver visualmente TODAS las señales y sus motivos de rechazo.

---

## 🎯 RESULTADO ESPERADO

1. **Bias correcto**: El gráfico mostrará "Bajista" cuando el sistema esté operando SHORT
2. **TPs inteligentes**: Un TP cercano y alcanzable (ej. P4 Fallback @ 32 pts) ganará sobre un TP lejano e inalcanzable (ej. P0 @ 86 pts)
3. **Diagnóstico visual**: El usuario verá todas las señales rechazadas y podrá validar si los filtros son correctos

---

## ⏭️ PRÓXIMOS PASOS

1. ✅ Copiar `ContextManager.cs` → NinjaTrader
2. ⏳ Generar `CalculateStructuralTP_Buy_V6.0j.cs` y `CalculateStructuralTP_Sell_V6.0j.cs`
3. ⏳ Reemplazar métodos en `RiskCalculator.cs`
4. ⏳ Copiar `RiskCalculator.cs` → NinjaTrader
5. ⏳ Compilar (F5)
6. ⏳ Ejecutar backtest
7. ⏳ Analizar logs para ver si los TPs elegidos son más inteligentes

