## Context

El proyecto fue migrado de Unity 2022.3 a Unity 6 sin una actualización completa de las dependencias de bajo nivel (Burst). Esto ha dejado al Universal Render Pipeline (URP) y al sistema de Colecciones (Collections) en un estado inconsistente, produciendo errores en cascada que afectan incluso a scripts de usuario que no tienen errores lógicos reales.

## Goals / Non-Goals

**Goals:**
- Restaurar la compilación total del proyecto en Unity 6.
- Sincronizar las versiones de Burst, Collections y URP según los requisitos de Unity 6 (6000.3.5f2).
- Resolver los errores de referencia en `NetworkManager` y `GameManager`.

**Non-Goals:**
- Refactorizar la lógica de red o de juego más allá de lo necesario para que compilen.
- Añadir nuevas funcionalidades de Unity 6 (como nuevas APIs de renderizado) en este paso.

## Decisions

### 1. Forzar la actualización de Burst en `manifest.json`
- **Decisión**: Añadir explícitamente `"com.unity.burst": "1.8.17"` (o la versión superior compatible con Unity 6) al archivo `manifest.json`.
- **Razón**: Aunque es una dependencia indirecta, forzarla en el manifiesto principal obliga a Unity a resolver los conflictos de versión que el actualizador automático omitió.

### 2. Regeneración de la carpeta `Library`
- **Decisión**: Eliminar la carpeta `Library/` tras actualizar el manifiesto.
- **Razón**: Unity 6 almacena cachés de compilación que pueden estar corruptas tras una migración fallida. Eliminarla fuerza un re-importado limpio de todos los assets y paquetes.

### 3. Uso de Nombres Completos (Qualified Names) para NetworkManager
- **Decisión**: Si los errores de "NetworkManager no contiene X" persisten, usaremos `global::NetworkManager` o moveremos la clase a un namespace único como `AEA.Networking`.
- **Razón**: Evitar colisiones con posibles clases internas de Unity 6 que compartan el mismo nombre.

## Risks / Trade-offs

- **[Riesgo]** La eliminación de `Library` puede tardar varios minutos en re-importar. → **Mitigación**: Avisar al usuario antes de proceder.
- **[Riesgo]** Incompatibilidades de API de C# en Unity 6 (ej. cambios en .NET 8). → **Mitigación**: Revisar los logs de error específicos de .NET si la actualización de paquetes no es suficiente.

## Migration Plan

1. Backup de `Packages/manifest.json`.
2. Edición manual de `manifest.json` para añadir Burst y actualizar Collections.
3. Ejecutar `Unity.exe` con el flag `-cleanDemo` o borrar `Library` manualmente.
4. Una vez compilen los paquetes, atacar los errores residuales en los scripts de Assets/.
