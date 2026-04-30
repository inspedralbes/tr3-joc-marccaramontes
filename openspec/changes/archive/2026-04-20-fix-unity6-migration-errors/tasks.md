## 1. Reparación de Dependencias (Fundación)

- [x] 1.1 Crear copia de seguridad de `Packages/manifest.json`.
- [x] 1.2 Añadir explícitamente `"com.unity.burst": "1.8.17"` a las dependencias de `manifest.json`.
- [x] 1.3 Actualizar `"com.unity.collections": "2.6.2"` en `manifest.json` (o asegurar que es la versión estable para Unity 6).
- [x] 1.4 Eliminar manualmente la carpeta `Library/` del proyecto para limpiar cachés corruptas.

## 2. Estabilización de Scripts de Usuario

- [x] 2.1 Abrir el proyecto en Unity 6 y esperar a la re-importación completa.
- [x] 2.2 Verificar si los errores en `NetworkManager.cs` persisten tras la actualización de Burst. (Solucionado)
- [x] 2.3 Si persisten, envolver `NetworkManager` en el namespace `AEA.Networking` para evitar colisiones. (No necesario)
- [x] 2.4 Actualizar las referencias a `NetworkManager` en `EnemySpawner.cs` y `GameManager.cs` si se ha cambiado el namespace. (No necesario)
- [x] 2.5 Verificar que el método `ResetSession` en `GameManager.cs` ya es visible para el compilador. (Solucionado)

## 3. Validación y Limpieza

- [x] 3.1 Confirmar que el conteo de errores en la consola de Unity es 0. (Confirmado por el usuario)
- [x] 3.2 Realizar una prueba de entrada al Menú y carga de la SampleScene. (Confirmado por el usuario)
- [x] 3.3 Regenerar los archivos de solución (`.sln` y `.csproj`) desde Unity (Edit > Preferences > External Tools > Regenerate project files). (Realizado)
