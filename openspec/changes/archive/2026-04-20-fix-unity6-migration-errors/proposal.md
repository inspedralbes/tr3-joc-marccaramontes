## Why

El proyecto ha sido migrado de Unity 2022.3 a Unity 6 (6000.3.5f2), lo que ha provocado un colapso en las dependencias de los paquetes principales (Burst, Collections, URP). Actualmente el proyecto tiene 284 errores de compilación que impiden su ejecución, causados principalmente por versiones de paquetes incompatibles entre sí y con la nueva versión del motor.

## What Changes

- **Actualización de Dependencias**: Sincronización de las versiones de `com.unity.burst` y `com.unity.collections` para que sean compatibles con Unity 6 y el Universal Render Pipeline (URP) 17.3.0.
- **Reparación de la Fundación (Burst)**: Asegurar que el compilador Burst esté correctamente instalado y reconocido por el proyecto.
- **Corrección de Errores de "Fantasmas"**: Una vez estabilizada la base del proyecto, se corregirán los errores de compilación en `NetworkManager.cs` y `GameManager.cs` que actualmente el compilador no reconoce correctamente debido al colapso de los paquetes.
- **Sincronización del Repositorio**: Asegurar que el estado del proyecto en disco coincida con lo que Unity espera (reparación de archivos .meta o referencias de ensamblado si es necesario).

## Capabilities

### New Capabilities
- `repository-sync`: Procedimiento para asegurar la integridad de los metadatos y dependencias tras una migración de versión mayor de Unity.

### Modified Capabilities
- `network-core`: Ajustes menores en la declaración de eventos y métodos para asegurar la compatibilidad con el nuevo backend de compilación.
- `game-state-management`: Verificación de la integridad de los métodos de gestión de estado tras la migración.

## Impact

- **Paquetes**: `Packages/manifest.json` y `Packages/packages-lock.json` serán modificados para forzar versiones compatibles.
- **Scripts**: `Assets/Networking/NetworkManager.cs` y `Assets/GameManager.cs` pueden requerir ajustes menores de sintaxis o referencias.
- **Entorno**: Se requiere una regeneración de los archivos de proyecto (.csproj, .sln) y posiblemente una limpieza de la carpeta `Library`.
