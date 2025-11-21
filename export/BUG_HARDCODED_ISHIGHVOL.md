# 🐛 BUG PENDIENTE: isHighVol hardcodeado en TradeManager

## **Descripción:**

En `TradeManager.cs` línea 119 existe un hardcoded temporal:

```csharp
bool isHighVol = true;  // TEMPORAL: asumir HighVol para test; mejorar después
```

## **Problema:**

- El gate por distancia (`MaxDistanceToEntry_ATR_HighVol`) **SIEMPRE se aplica**, incluso en régimen Normal
- Esto puede causar rechazos incorrectos cuando el mercado está en régimen Normal

## **Solución requerida:**

**Opción A - Pasar régimen desde ExpertTrader (RECOMENDADO):**

1. `ExpertTrader.cs` ya tiene `currentRegime` disponible (línea 546)
2. Ya se pasa a `_tradeManager.RegisterTrade(..., currentRegime)` (línea 589)
3. `TradeManager.cs` ya recibe el parámetro `currentRegime` (línea 84)
4. **Solo falta usar el parámetro en vez del hardcoded:**

```csharp
// TradeManager.cs línea 119 - CAMBIAR ESTO:
bool isHighVol = true;  // ❌ HARDCODED

// POR ESTO:
bool isHighVol = (currentRegime == "HighVol");  // ✅ CORRECTO
```

**Opción B - Pasar desde DecisionSnapshot:**

Si se prefiere mantener el régimen en la decisión:

1. `TradeDecision` ya tiene `DistanceToEntryATR` y `DistanceToEntryPoints`
2. Añadir `CurrentRegime` a `TradeDecision`
3. Extraer en `ExpertTrader` y pasar a `RegisterTrade`

## **Impacto:**

- **BAJO** en testing actual (todos los backtests recientes son HighVol)
- **ALTO** cuando se ejecute en mercados con régimen Normal

## **Fecha detectado:**

2025-11-07 (durante implementación V6.0i.9)

## **Estado:**

⏳ **PENDIENTE** - Usuario lo mencionó explícitamente: "no lo olvides más!"

---

*RECORDATORIO: Esto es parte del MVP de V6.0i.9. NO olvidar.*



