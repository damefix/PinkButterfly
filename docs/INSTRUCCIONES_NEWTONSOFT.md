# 📦 Instrucciones para Agregar Newtonsoft.Json a NinjaTrader 8

## ⚠️ IMPORTANTE
El proyecto **PinkButterfly CoreBrain** requiere **Newtonsoft.Json.dll** para la serialización JSON profesional.

---

## 🔧 Paso 1: Descargar Newtonsoft.Json.dll

### Opción A: Desde NuGet.org (Recomendada)

1. Ve a: https://www.nuget.org/packages/Newtonsoft.Json/
2. Haz clic en **"Download package"** (lado derecho)
3. Descarga la versión **13.0.3** (última compatible con .NET Framework 4.5+)
4. Renombra el archivo descargado de `.nupkg` a `.zip`
5. Extrae el archivo ZIP
6. Navega a la carpeta: `lib\net45\`
7. Copia el archivo `Newtonsoft.Json.dll`

### Opción B: Desde tu sistema (si tienes Visual Studio)

Busca en tu PC:
```
C:\Users\[TuUsuario]\.nuget\packages\newtonsoft.json\13.0.3\lib\net45\Newtonsoft.Json.dll
```

---

## 📁 Paso 2: Copiar DLL al Proyecto

Crea una carpeta `lib` en tu proyecto PinkButterfly y copia la DLL:

```
C:\Users\Administrator\Documents\trading\PinkButterfly\
  lib\
    Newtonsoft.Json.dll  ← Copiar aquí
```

---

## 🔗 Paso 3: Agregar Referencia en NinjaTrader 8

1. Abre **NinjaTrader 8**
2. Ve a **Tools** → **Edit NinjaScript** → **Indicators**
3. Abre cualquier indicador (o crea uno nuevo temporal)
4. En el editor, ve a **Tools** → **References**
5. Haz clic en **Add...**
6. Navega a: `C:\Users\Administrator\Documents\trading\PinkButterfly\lib\`
7. Selecciona `Newtonsoft.Json.dll`
8. Haz clic en **OK**

**NOTA**: Esta referencia se agregará globalmente a NinjaTrader y estará disponible para todos los scripts.

---

## ✅ Paso 4: Compilar el Proyecto

1. Copia los 3 archivos desde `export\` a tu NinjaTrader local:
   - `EngineConfig.cs`
   - `StructureModels.cs`
   - `CoreBrainIndicator.cs`

2. En NinjaTrader, ve a **Tools** → **Compile**

3. Si todo está correcto, deberías ver:
   ```
   Compilation successful.
   ```

---

## 🚨 Solución de Problemas

### Error: "The type or namespace name 'Newtonsoft' could not be found"

**Causa**: La referencia a Newtonsoft.Json.dll no está agregada correctamente.

**Solución**:
1. Verifica que `Newtonsoft.Json.dll` esté en la carpeta `lib\`
2. Repite el Paso 3 para agregar la referencia
3. Reinicia NinjaTrader 8

### Error: "Could not load file or assembly 'Newtonsoft.Json'"

**Causa**: La DLL no está en una ubicación accesible.

**Solución**:
1. Copia `Newtonsoft.Json.dll` también a:
   ```
   C:\Users\Administrator\Documents\NinjaTrader 8\bin\Custom\
   ```
2. Reinicia NinjaTrader 8

---

## 📝 Verificación Final

Para verificar que todo funciona:

1. En NinjaTrader, ve a **Tools** → **Output Window**
2. Carga un gráfico
3. Agrega el indicador **CoreBrain**
4. Deberías ver en el Output:
   ```
   CoreBrain: Inicializando motor...
   CoreBrain: Inicializado (Skeleton - Fase 1)
   ```

---

## ✨ ¿Por qué Newtonsoft.Json?

- ✅ **Estándar de la industria** para .NET Framework
- ✅ **Robusto y probado** en millones de proyectos
- ✅ **Maneja polimorfismo** sin problemas
- ✅ **Compatible con DLL** futura (se incluirá en el assembly)
- ✅ **Solución profesional** sin compromisos de calidad

---

## 📞 Soporte

Si tienes problemas, verifica:
1. Versión de Newtonsoft.Json: **13.0.3**
2. Plataforma target: **.NET Framework 4.5+**
3. Ubicación de la DLL: `PinkButterfly\lib\Newtonsoft.Json.dll`

---

**Última actualización**: 23 de octubre de 2025
**Versión del proyecto**: Fase 1 - Fundaciones

