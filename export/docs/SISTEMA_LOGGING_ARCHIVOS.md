# Sistema de Logging a Archivos - PinkButterfly

## 📋 Resumen

Se ha implementado un **sistema profesional de logging a archivos** para capturar el 100% de las operaciones del backtest, sin las limitaciones del Output de NinjaTrader.

---

## 🎯 Problema Resuelto

### **Antes:**
- NinjaTrader trunca los logs después de ~2000 líneas
- Mensaje: "You have reached the maximum threshold of the NinjaScript Output window"
- **Imposible hacer análisis estadístico completo** del backtest

### **Después:**
- **Todos los logs se guardan en archivos** en el directorio `logs/`
- **CSV con todas las operaciones** para análisis en Excel/Python
- **Archivos con timestamp** para almacenar múltiples backtests

---

## 📁 Archivos Creados

### 1. **`FileLogger.cs`** (src/Infrastructure/)
Logger general que escribe a archivos de texto con timestamp.

**Características:**
- Escribe todos los logs del sistema (INFO, WARN, ERROR, DEBUG)
- Thread-safe (usa lock para escritura concurrente)
- Mantiene el output en consola (duplica los logs)
- Formato: `[HH:mm:ss.fff] [NIVEL] mensaje`

**Nombre de archivo:**
```
logs/backtest_YYYYMMDD_HHmmss.log
```

**Ejemplo:**
```
logs/backtest_20251026_143052.log
```

---

### 2. **`TradeLogger.cs`** (src/Infrastructure/)
Logger especializado para operaciones de trading que genera un CSV.

**Características:**
- Escribe un CSV con todas las operaciones
- Columnas: TradeID, Timestamp, Action, Direction, Entry, SL, TP, RiskPoints, RewardPoints, RR, Bar, StructureID, Status, ExitReason, ExitBar, ExitPrice, PnLPoints, PnLDollars
- Thread-safe
- Calcula P&L automáticamente (en puntos y dólares)

**Nombre de archivo:**
```
logs/trades_YYYYMMDD_HHmmss.csv
```

**Ejemplo:**
```
logs/trades_20251026_143052.csv
```

---

## 📊 Formato del CSV de Operaciones

### **Header:**
```csv
TradeID,Timestamp,Action,Direction,Entry,SL,TP,RiskPoints,RewardPoints,RR,Bar,StructureID,Status,ExitReason,ExitBar,ExitPrice,PnLPoints,PnLDollars
```

### **Ejemplo de registros:**

**Orden Registrada:**
```csv
T0001,2025-10-26 14:30:52,REGISTERED,SELL,6798.25,6812.45,6719.75,14.20,78.50,5.53,7726,9eb96e77-c99c-4665-93c7-ce34b72353f4,PENDING,,-,-,-,-
```

**Orden Cancelada:**
```csv
T0001,2025-10-26 14:31:15,CANCELLED,SELL,6798.25,-,-,-,-,-,7726,-,CANCELLED,BOS contradictorio,-,-,-,-
```

**Orden Cerrada por TP:**
```csv
T0005,2025-10-26 14:45:23,CLOSED,SELL,6832.00,-,6785.25,-,-,-,7743,-,TP_HIT,TP,7755,6785.25,46.75,233.75
```

**Orden Cerrada por SL:**
```csv
T0008,2025-10-26 15:02:10,CLOSED,SELL,6840.75,6843.75,-,-,-,-,7750,-,SL_HIT,SL,7762,6843.75,-3.00,-15.00
```

---

## 🔧 Integración en el Sistema

### **Modificaciones en `TradeManager.cs`:**

1. **Constructor actualizado:**
```csharp
public TradeManager(EngineConfig config, ILogger logger, TradeLogger tradeLogger = null, 
                    double contractSize = 1.0, double pointValue = 5.0)
```

2. **Logging automático en cada evento:**
- `RegisterTrade()` → `LogOrderRegistered()`
- `UpdateTrades()` (SL Hit) → `LogOrderClosedSL()`
- `UpdateTrades()` (TP Hit) → `LogOrderClosedTP()`
- `CheckExpiration()` → `LogOrderExpired()`
- `CheckBOSContradictory()` → `LogOrderCancelled()`

---

### **Modificaciones en `ExpertTrader.cs`:**

1. **Variables añadidas:**
```csharp
private FileLogger _fileLogger;
private TradeLogger _tradeLogger;
```

2. **Inicialización en `State.DataLoaded`:**
```csharp
string logDirectory = System.IO.Path.Combine(NinjaTrader.Core.Globals.UserDataDir, "logs");
_fileLogger = new FileLogger(logDirectory, "backtest", _logger, true);
_tradeLogger = new TradeLogger(logDirectory, "trades", _logger, true);
```

3. **Cierre en `State.Terminated`:**
```csharp
_tradeLogger?.Close();
_fileLogger?.Close();
```

---

## 📍 Ubicación de los Archivos

Los archivos se crean **automáticamente** en tu instalación local de NinjaTrader:

```
C:\Users\<TU_USUARIO>\Documents\NinjaTrader 8\logs\
```

**Ejemplo completo:**
```
C:\Users\meste\Documents\NinjaTrader 8\logs\
├── backtest_20251026_143052.log
├── trades_20251026_143052.csv
├── backtest_20251026_150230.log
└── trades_20251026_150230.csv
```

**IMPORTANTE:** 
- Los archivos se crean en **tu PC local** (donde ejecutas NinjaTrader)
- Usa `NinjaTrader.Core.Globals.UserDataDir` para obtener la ruta automáticamente
- El directorio `logs/` se crea automáticamente si no existe
- **Después del backtest, puedes copiar los archivos a este proyecto** para análisis

---

## 🎯 Uso del Sistema

### **1. Ejecutar un Backtest:**
1. Cargar el indicador `ExpertTrader` en NinjaTrader
2. Configurar los parámetros (BacktestBarsForAnalysis, etc.)
3. Aplicar a la gráfica
4. **El sistema automáticamente creará los archivos de log**

### **2. Analizar los Resultados:**

**Opción A: Abrir el CSV en Excel**
```
1. Ir a: C:\Users\<USUARIO>\Documents\NinjaTrader 8\logs\
2. Abrir el archivo trades_YYYYMMDD_HHmmss.csv
3. Filtrar por Status: TP_HIT, SL_HIT
4. Calcular Win Rate, Profit Factor, etc.
```

**Opción B: Usar Python**
```python
import pandas as pd

# Leer el CSV
df = pd.read_csv('logs/trades_20251026_143052.csv')

# Filtrar operaciones cerradas
closed = df[df['Status'].isin(['TP_HIT', 'SL_HIT'])]

# Calcular estadísticas
win_rate = (closed['Status'] == 'TP_HIT').sum() / len(closed) * 100
total_pnl = closed['PnLDollars'].sum()
profit_factor = closed[closed['PnLDollars'] > 0]['PnLDollars'].sum() / \
                abs(closed[closed['PnLDollars'] < 0]['PnLDollars'].sum())

print(f"Win Rate: {win_rate:.2f}%")
print(f"Total P&L: ${total_pnl:.2f}")
print(f"Profit Factor: {profit_factor:.2f}")
```

**Opción C: Leer el log completo**
```
Abrir: logs/backtest_20251026_143052.log
Buscar: [TradeManager] para ver todas las operaciones
```

---

## ✅ Ventajas del Sistema

1. **Sin límites de tamaño:** Captura el 100% de los logs
2. **Análisis estadístico completo:** CSV listo para Excel/Python
3. **Histórico de backtests:** Cada ejecución se guarda con timestamp
4. **Thread-safe:** Funciona correctamente con procesamiento paralelo
5. **Cálculo automático de P&L:** En puntos y dólares
6. **Mantiene output en consola:** No pierde funcionalidad de NinjaTrader

---

## 🔄 Próximos Pasos

Con este sistema ya podemos:

1. **Ejecutar backtest de 5000 barras completo**
2. **Obtener CSV con todas las operaciones**
3. **Calcular KPIs reales:**
   - Win Rate
   - Profit Factor
   - Average Win / Average Loss
   - Sharpe Ratio
   - Maximum Drawdown
4. **Analizar patrones de fallos** (operaciones con SL_HIT)
5. **Calibrar el DFM** basándose en datos reales

---

## 📝 Notas Técnicas

### **Thread Safety:**
Ambos loggers usan `lock` para garantizar escritura thread-safe:
```csharp
lock (_lockObject)
{
    File.AppendAllText(_csvFilePath, line + Environment.NewLine, Encoding.UTF8);
}
```

### **Encoding:**
Se usa `UTF-8` para soportar caracteres especiales (emojis, etc.)

### **Manejo de Errores:**
Si falla la creación del archivo, el sistema continúa funcionando (solo usa el logger de consola)

---

## 🎉 Resultado Final

**Ahora tendremos:**
- ✅ Log completo del backtest (sin truncamiento)
- ✅ CSV con todas las operaciones (listo para análisis)
- ✅ Archivos con timestamp (histórico de backtests)
- ✅ Cálculo automático de P&L
- ✅ Sistema profesional y robusto

**¡El sistema está listo para el análisis estadístico completo!** 🚀

