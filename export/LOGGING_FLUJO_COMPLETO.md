# 🔬 LOGGING DE FLUJO COMPLETO - ANÁLISIS DESDE LA BASE

**Fecha**: 30 Oct 2025  
**Objetivo**: Analizar **TODA LA CADENA** desde detección hasta decisión para identificar dónde se rompe

---

## 🎯 FILOSOFÍA: ANÁLISIS DE FLUJO COMPLETO

```
┌─────────────┐   ┌─────────────┐   ┌─────────────┐   ┌─────────────┐   ┌─────────────┐
│ DETECCIÓN   │ → │  SCORING    │ → │   FUSIÓN    │ → │ SELECCIÓN   │ → │  DECISIÓN   │
│ Swings/FVGs │   │  <0.3?      │   │  HeatZones  │   │   SL/TP     │   │   Trade     │
│ Creados?    │   │  TFWeight?  │   │  Anchors?   │   │  Reject?    │   │  Execute?   │
└─────────────┘   └─────────────┘   └─────────────┘   └─────────────┘   └─────────────┘
      ↓                  ↓                  ↓                  ↓                  ↓
   ¿BIEN?           ¿BIEN?             ¿BIEN?             ¿BIEN?             ¿BIEN?
```

**No tiene sentido analizar los filtros si los datos de base están mal.**

---

## 📋 LOGGING IMPLEMENTADO - NIVEL POR NIVEL

### **NIVEL 1: DETECCIÓN - ¿Se crean estructuras?**

#### **1A. SwingDetector - Creación de swings**
```
[DIAG][SwingDetector] CREATED: TF=60 Bar=500 Type=Low Price=6750.25 Range=12.50 SizeTicks=50 ID=a8b3c...
```
**Verifica**:
- ✅ ¿Se están creando swings?
- ✅ ¿En qué TFs? (5, 15, 60, 240, 1440)
- ✅ ¿Con qué frecuencia?
- ✅ ¿Rangos razonables?

**Si NO aparece**: `MinSwingATRfactor=0.15` es demasiado alto → NO se detectan swings

#### **1B. SwingDetector - Snapshot cada 50 barras**
```
[DIAG][SwingDetector] TF=60 Bar=500 ActiveSwings=15 (High=8 Low=7)
```
**Verifica**:
- ✅ ¿Cuántos swings activos hay?
- ✅ ¿Se están purgando demasiado rápido?
- ✅ ¿Balance High vs Low correcto?

**Si ActiveSwings=0**: Problema de purga o no se están creando

---

### **NIVEL 2: SCORING - ¿Scores correctos?**

#### **2A. ScoringEngine - Scores bajos**
```
[DIAG][ScoringEngine] LOW_SCORE: Type=SWING TF=60 Score=0.18 TFw=0.50 Fresh=0.85 Prox=0.60 Decay=0.70
```
**Verifica**:
- ✅ ¿Por qué el score es bajo?
- ✅ TFWeight correcto? (60m debería ser 0.50)
- ✅ Freshness bajo? (estructura vieja)
- ✅ Proximity bajo? (lejos del precio)
- ✅ Decay alto? (no actualizada)

**Si TFw=0.50 pero score <0.30**: El problema NO es TFWeights, es otro componente

---

### **NIVEL 3: SELECCIÓN SL - ¿Por qué se rechazan?**

#### **3A. RiskCalculator - Sin candidatos**
```
[DIAG][Risk] NO_SL_CANDIDATES: Zone=HZ_123 Entry=6750.00 
    TotalFound=25 RejAge=10 RejScore=12 RejDist=3 SwingsByTF=[5:5,15:10,60:10]
```
**Verifica**:
- ✅ **TotalFound=0**: NO hay swings → Problema en NIVEL 1 (Detección)
- ✅ **TotalFound>0, RejAge alto**: Swings demasiado viejos → `MaxAgeForSL_ByTF` restrictivo
- ✅ **TotalFound>0, RejScore alto**: Swings con score bajo → Problema en NIVEL 2 (Scoring)
- ✅ **TotalFound>0, RejDist alto**: Swings demasiado cerca → `MinSLDistanceATR=1.0` restrictivo

#### **3B. RiskCalculator - Rechazos con candidatos**
```
[DIAG][Risk] SL_REJECTIONS: Zone=HZ_123 TotalFound=25 Accepted=3 RejAge=10 RejScore=12 RejDist=0
```
**Verifica**:
- ✅ Ratio de aceptación (3/25 = 12%)
- ✅ Qué filtro rechaza más

---

## 🔍 ANÁLISIS DE FLUJO - ESCENARIOS

### **ESCENARIO A: Problema en DETECCIÓN (Base rota)**
```
[DIAG][SwingDetector] TF=60 Bar=500 ActiveSwings=0
[DIAG][Risk] NO_SL_CANDIDATES: TotalFound=0 RejAge=0 RejScore=0 RejDist=0
```
**Conclusión**: No se crean swings → `MinSwingATRfactor=0.15` demasiado alto  
**Acción**: Reducir a 0.10 o analizar por qué se rechazan al detectar

---

### **ESCENARIO B: Problema en SCORING (Scores bajos)**
```
[DIAG][SwingDetector] CREATED: TF=60 Bar=500 Type=Low ...
[DIAG][ScoringEngine] LOW_SCORE: Type=SWING TF=60 Score=0.18 ...
[DIAG][Risk] NO_SL_CANDIDATES: TotalFound=25 RejAge=0 RejScore=24 RejDist=0
```
**Conclusión**: Swings se crean PERO tienen score bajo → Problema en cálculo de score  
**Acción**: Investigar por qué (Decay?, TFWeight?, Proximity?)

---

### **ESCENARIO C: Problema en FILTROS (Demasiado restrictivos)**
```
[DIAG][SwingDetector] CREATED: TF=60 Bar=500 Type=Low ...
[DIAG][ScoringEngine] Type=SWING TF=60 Score=0.42 (OK, >0.30)
[DIAG][Risk] NO_SL_CANDIDATES: TotalFound=25 RejAge=20 RejScore=3 RejDist=2
```
**Conclusión**: Swings OK, scores OK, PERO filtros rechazan  
**Acción**: Relajar `MaxAgeForSL_ByTF` o `MinSLScore`

---

### **ESCENARIO D: Problema en PURGA (Se eliminan demasiado rápido)**
```
[DIAG][SwingDetector] CREATED: TF=60 Bar=500 Type=Low ... (muchos logs)
[DIAG][SwingDetector] TF=60 Bar=550 ActiveSwings=2 (muy pocos!)
[DIAG][Risk] NO_SL_CANDIDATES: TotalFound=2 ...
```
**Conclusión**: Se crean swings PERO se purgan rápido → `EnableAutoPurge` agresivo  
**Acción**: Ajustar lógica de purga o desactivar temporalmente

---

## 📊 COMANDOS DE ANÁLISIS POST-BACKTEST

### **1. ¿Se están creando swings?**
```powershell
$swingsCreated = (Select-String -Path "logs\backtest_FECHA.log" -Pattern "\[DIAG\]\[SwingDetector\] CREATED").Count
Write-Host "Swings creados total: $swingsCreated"

# Por TF
Select-String -Path "logs\backtest_FECHA.log" -Pattern "\[DIAG\]\[SwingDetector\] CREATED.*TF=60" | Measure-Object
```

### **2. ¿Cuántos swings activos hay?**
```powershell
# Últimos 10 snapshots
Select-String -Path "logs\backtest_FECHA.log" -Pattern "\[DIAG\]\[SwingDetector\].*ActiveSwings" | 
    Select-Object -Last 10
```

### **3. ¿Por qué scores bajos?**
```powershell
# Ver primeros 20 casos de scores <0.30
Select-String -Path "logs\backtest_FECHA.log" -Pattern "\[DIAG\]\[ScoringEngine\] LOW_SCORE" | 
    Select-Object -First 20
```

### **4. ¿Dónde está el cuello de botella?**
```powershell
# Contar rechazos SL
$noSL = (Select-String -Path "logs\backtest_FECHA.log" -Pattern "\[DIAG\]\[Risk\] NO_SL_CANDIDATES").Count
Write-Host "Zonas SIN candidatos SL: $noSL"

# Sumar tipos de rechazo
Select-String -Path "logs\backtest_FECHA.log" -Pattern "\[DIAG\]\[Risk\] NO_SL_CANDIDATES" |
    ForEach-Object {
        if ($_.Line -match "RejAge=(\d+).*RejScore=(\d+).*RejDist=(\d+)") {
            [PSCustomObject]@{Age=[int]$Matches[1]; Score=[int]$Matches[2]; Dist=[int]$Matches[3]}
        }
    } | Measure-Object -Property Age,Score,Dist -Sum
```

---

## ✅ ARCHIVOS MODIFICADOS

- ✅ `src/Detectors/SwingDetector.cs` (líneas 75-83, 282-283)
- ✅ `src/Core/ScoringEngine.cs` (líneas 160-164)
- ✅ `src/Decision/RiskCalculator.cs` (líneas 1175-1190, 1308-1323)

**Estado**: Copiados a NinjaTrader  
**Próximo paso**: Recompilar y lanzar backtest 10 días

---

## 🎯 RESULTADO ESPERADO

Este logging nos dirá **EXACTAMENTE** en qué nivel se rompe la cadena:

| Nivel | ¿Funciona? | Siguiente paso |
|-------|------------|----------------|
| 1. Detección | ❌ NO | Ajustar `MinSwingATRfactor` o lógica de detección |
| 1. Detección | ✅ SÍ | Ir a Nivel 2 |
| 2. Scoring | ❌ NO | Investigar componentes (Decay, TFWeight, Proximity) |
| 2. Scoring | ✅ SÍ | Ir a Nivel 3 |
| 3. Filtros | ❌ NO | Relajar parámetros (`MaxAge`, `MinScore`, `MinDist`) |
| 3. Filtros | ✅ SÍ | ¿Problema en Fusión/HeatZones? |

**NO MÁS ADIVINANZAS** → Análisis **CIENTÍFICO** del flujo completo 🔬

---

*Documento generado: 30 Oct 2025 20:05*

