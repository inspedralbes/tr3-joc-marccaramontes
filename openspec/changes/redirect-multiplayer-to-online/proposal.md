## Why

El modo multijugador local por turnos actualmente no funciona correctamente o causa confusión en el flujo del menú. Los usuarios esperan que el botón "Multijugador" los lleve a una experiencia online. Redirigir este botón al Lobby simplifica el juego y elimina código redundante o inestable.

## What Changes

- **Redirección de Menú**: Modificar `MenuController.cs` para que el método `PlayMultiplayer()` cargue la escena `Lobby` en lugar de intentar iniciar una partida local.
- **Simplificación de Modos**: Marcar el modo `GameMode.Multiplayer` (local) como obsoleto o eliminar sus puntos de entrada para evitar confusión.
- **Actualización de UI**: Asegurar que el botón de Multijugador en el Menú Principal esté correctamente vinculado al flujo Online.

## Capabilities

### New Capabilities
- `menu-navigation`: Define el comportamiento de navegación entre el Menú Principal y las salas de espera (Lobby).

### Modified Capabilities
- `game-modes`: Modificar los requisitos para eliminar o deshabilitar el modo multijugador local.

## Impact

- **Código Afectado**: `MenuController.cs`, `GameManager.cs`, `GameTypes.cs`.
- **UI**: Botones del Menú Principal en la escena `Menu`.
- **Experiencia de Usuario**: Flujo de navegación más directo hacia el contenido funcional (Online).
