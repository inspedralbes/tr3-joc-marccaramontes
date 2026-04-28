## Why

El modo multijugador actual tiene fallos críticos de visibilidad y sincronización que impiden una experiencia de juego funcional entre dos jugadores. Además, el Lobby carece de retroalimentación visual sobre quién está en la sala, lo que genera confusión durante la espera.

## What Changes

- **Lobby Visual Feedback**: Se añadirá una lista de jugadores en tiempo real dentro del panel de espera del Lobby.
- **Player Visibility Fix**: Se integrará el `RemotePlayerManager` en la escena de juego para que los jugadores remotos sean visibles.
- **Enemy Synchronization Fix**: Se corregirán los nombres de los eventos de red (`SPAWN_ENEMY`, `SHOOT`, etc.) para que coincidan entre el cliente y el servidor, permitiendo que el Jugador 2 vea enemigos y disparos.
- **Server Broadcast Correction**: El servidor ahora retransmitirá correctamente los eventos de spawneo y disparos usando los nombres de eventos estandarizados.

## Capabilities

### New Capabilities
- `lobby-player-list`: Capacidad de mostrar y actualizar la lista de jugadores presentes en una sala de espera mediante eventos de socket.

### Modified Capabilities
- `player-positioning`: Se actualizará para asegurar que el `RemotePlayerManager` gestione correctamente la instanciación de rivales en la escena.
- `enemy-spawning`: Los requisitos de red se ajustarán para usar eventos semánticos estandarizados (`SPAWN_ENEMY` en lugar de `spawn_enemy`).

## Impact

- **Código Unity**: `LobbyController.cs`, `NetworkManager.cs`, `EnemySpawner.cs`, `PlayerShooting.cs`.
- **Escenas**: `Lobby.unity`, `SampleScene.unity`.
- **Servidor**: `GameService.js`, `SocketController.js`.
- **Red**: Cambio en los nombres de eventos de socket (Protocolo).
