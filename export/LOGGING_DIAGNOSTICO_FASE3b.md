# 🔬 LOGGING DIAGNÓSTICO COMPLETO - FASE 3b

**Fecha**: 30 Oct 2025  
**Objetivo**: Identificar la causa raíz del fracaso de FASE 3b (0% SL estructural, 1.6% TP estructural, WR 37.2%)

---

## 📋 LOGGING IMPLEMENTADO

### **1. SwingDetector - Monitoreo de detección**

**Archivo**: `src/Detectors/SwingDetector.cs`  
**Frecuencia**: Cada 50 barras por TF  
**Formato**:
```
[DIAG][SwingDetector] TF={tf} Bar={bar} ActiveSwings={total} (High={nHigh} Low={nLow})
```

**Información proporcionada**:
- Cuántos swings activos hay por TF
- Distribución High vs Low
- Permite detectar si hay **escasez de swings** o si se están **purgando demasiado rápido**

---

### **2. RiskCalculator - Análisis detallado de rechazos SL (BUY)**

**Archivo**: `src/Decision/RiskCalculator.cs` → `FindProtectiveSwingLowBanded()`  
**Frecuencia**: Por cada HeatZone procesada  

#### **2A. Cuando NO hay candidatos** (caso crítico)
**Formato**:
```
[DIAG][Risk] NO_SL_CANDIDATES: Zone={id} Entry={price} TotalFound={n} RejAge={n} RejScore={n} RejDist={n} SwingsByTF=[5:X,15:Y,60:Z,240:W]
```

**Información proporcionada**:
- **TotalFound**: Swings detectados ANTES de filtros (si es 0 → problema en SwingDetector)
- **RejAge**: Rechazados por `age > MaxAgeForSL_ByTF` (si es alto → filtro de edad demasiado restrictivo)
- **RejScore**: Rechazados por `Score < MinSLScore` (si es alto → MinSLScore=0.35 es demasiado alto)
- **RejDist**: Rechazados por `distancia < MinSLDistanceATR` (si es alto → MinSLDistanceATR=1.0 es demasiado restrictivo)
- **SwingsByTF**: Distribución de swings encontrados por TF ANTES de filtros

#### **2B. Cuando SÍ hay candidatos (análisis de filtros)**
**Formato**:
```
[DIAG][Risk] SL_REJECTIONS: Zone={id} TotalFound={n} Accepted={n} RejAge={n} RejScore={n} RejDist={n}
```

**Información proporcionada**:
- Comparativa: TotalFound vs Accepted
- Permite ver **qué porcentaje de swings pasan** los filtros

---

### **3. RiskCalculator - Análisis detallado de rechazos SL (SELL)**

**Archivo**: `src/Decision/RiskCalculator.cs` → `FindProtectiveSwingHighBanded()`  
**Frecuencia**: Por cada HeatZone procesada  

Mismo formato y lógica que para BUY, pero para Swing Highs (SELL).

---

## 🎯 DIAGNÓSTICO ESPERADO

Con este logging, el próximo backtest nos dirá **EXACTAMENTE**:

### **Escenario A: No hay swings disponibles** (problema en detección)
```
[DIAG][SwingDetector] TF=60 Bar=500 ActiveSwings=0 (High=0 Low=0)
[DIAG][Risk] NO_SL_CANDIDATES: TotalFound=0 RejAge=0 RejScore=0 RejDist=0 SwingsByTF=[]
```
**Conclusión**: `MinSwingATRfactor=0.15` es demasiado alto → reducir a 0.10  
**O**: Swings se están purgando demasiado rápido → ajustar `EnableAutoPurge`

### **Escenario B: Hay swings, pero filtro de EDAD los rechaza**
```
[DIAG][SwingDetector] TF=60 Bar=500 ActiveSwings=15 (High=8 Low=7)
[DIAG][Risk] NO_SL_CANDIDATES: TotalFound=25 RejAge=24 RejScore=1 RejDist=0 SwingsByTF=[5:5,15:10,60:10]
```
**Conclusión**: `MaxAgeForSL_ByTF` demasiado restrictivo  
**Acción**: Revertir a valores originales:
- `{ 5, 200 }` (era 120)
- `{ 15, 100 }` (era 80)

### **Escenario C: Hay swings, pero filtro de SCORE los rechaza**
```
[DIAG][SwingDetector] TF=60 Bar=500 ActiveSwings=15 (High=8 Low=7)
[DIAG][Risk] NO_SL_CANDIDATES: TotalFound=25 RejAge=2 RejScore=22 RejDist=1 SwingsByTF=[5:5,15:10,60:10]
```
**Conclusión**: `MinSLScore=0.35` es demasiado alto  
**Acción**: Reducir a 0.25 o 0.20

### **Escenario D: Hay swings, pero filtro de DISTANCIA los rechaza**
```
[DIAG][SwingDetector] TF=60 Bar=500 ActiveSwings=15 (High=8 Low=7)
[DIAG][Risk] NO_SL_CANDIDATES: TotalFound=25 RejAge=2 RejScore=3 RejDist=20 SwingsByTF=[5:5,15:10,60:10]
```
**Conclusión**: `MinSLDistanceATR=1.0` rechaza swings cercanos válidos  
**Acción**: Reducir a 0.0 o 0.5

### **Escenario E: Combinación de filtros (el más probable)**
```
[DIAG][SwingDetector] TF=60 Bar=500 ActiveSwings=15 (High=8 Low=7)
[DIAG][Risk] NO_SL_CANDIDATES: TotalFound=25 RejAge=10 RejScore=12 RejDist=3 SwingsByTF=[5:5,15:10,60:10]
```
**Conclusión**: TODOS los filtros son demasiado restrictivos  
**Acción**: Relajar múltiples parámetros de forma balanceada

---

## 📊 ANÁLISIS POST-BACKTEST

Después del backtest con este logging:

1. **Extraer patrones del log**:
   ```powershell
   Select-String -Path "logs\backtest_FECHA.log" -Pattern "\[DIAG\]\[Risk\] NO_SL_CANDIDATES" | 
       Measure-Object | Select-Object -ExpandProperty Count
   ```

2. **Contar rechazos por tipo**:
   ```powershell
   Select-String -Path "logs\backtest_FECHA.log" -Pattern "\[DIAG\]\[Risk\] NO_SL_CANDIDATES" |
       ForEach-Object { if ($_ -match "RejAge=(\d+).*RejScore=(\d+).*RejDist=(\d+)") {
           [PSCustomObject]@{Age=$Matches[1]; Score=$Matches[2]; Dist=$Matches[3]}
       }} | Measure-Object -Property Age,Score,Dist -Average -Sum
   ```

3. **Ver distribución de swings activos**:
   ```powershell
   Select-String -Path "logs\backtest_FECHA.log" -Pattern "\[DIAG\]\[SwingDetector\]" |
       Select-Object -First 20
   ```

---

## ✅ ARCHIVOS MODIFICADOS

- ✅ `src/Detectors/SwingDetector.cs` (líneas 75-83)
- ✅ `src/Decision/RiskCalculator.cs` (líneas 1111-1190, 1244-1323)

**Estado**: Copiados a NinjaTrader  
**Próximo paso**: Recompilar (F5) y lanzar backtest de 17 días

---

## 🎯 EXPECTATIVA

Este logging nos dará **evidencia irrefutable** de:
- ✅ Si hay swings disponibles o no
- ✅ Cuál filtro es el culpable principal de rechazos
- ✅ Si el problema está en detección (SwingDetector) o selección (RiskCalculator)
- ✅ Qué parámetros específicos ajustar

**NO MÁS CONJETURAS** → Solo **DATOS DUROS** 📊

---

*Documento generado: 30 Oct 2025 19:40*

