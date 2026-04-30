## Why

El proyecto actualmente solo soporta multijugador local por turnos (hotseat). Para escalar la experiencia de juego y permitir que los jugadores compitan desde diferentes ubicaciones, es necesario implementar una arquitectura cliente-servidor con un backend en Node.js y comunicación en tiempo real mediante WebSockets.

## What Changes

- **Integración de Backend**: Creación de un servidor Node.js independiente para gestionar sesiones y relay de datos.
- **Comunicación en Red**: Implementación de `UnityWebRequest` para acciones asíncronas (login/salas) y `WebSockets` para el estado del juego en tiempo real.
- **Sincronización de Entidades**: Refactorización de `PlayerMovement` y `EnemySpawner` para soportar estados compartidos entre múltiples clientes.
- **Gestión de Salas (Lobby)**: Nueva interfaz y lógica para crear o unirse a partidas online.
- **Persistencia de Resultados**: Envío de tiempos de supervivencia al servidor al finalizar la partida para su registro.

## Capabilities

### New Capabilities
- `network-core`: Infraestructura básica de conexión, gestión de errores de red y ciclo de vida de la conexión con el servidor Node.js.
- `matchmaking`: Lógica para crear, listar y unirse a salas de juego compartidas.
- `state-sync`: Sistema de sincronización de posiciones, disparos y eventos de spawn entre el Host y los Clientes.

### Modified Capabilities
- `game-modes`: Se extiende para incluir el modo "Online Multiplayer" además de los modos "Solo" y "Local Multiplayer".

## Impact

- **Servidor**: Nuevo proyecto Node.js con Express y Socket.io.
- **Scripts Unity**: 
    - `NetworkManager.cs` (Nuevo): Corazón de la comunicación.
    - `GameManager.cs`: Adaptación para esperar confirmaciones del servidor.
    - `PlayerMovement.cs`: Soporte para "Ghost Players" (representación de otros jugadores).
    - `EnemySpawner.cs`: Lógica condicional (solo el Host spawnea).
- **Escenas**: Nueva escena `Lobby` y actualización de `Menu` para navegación online.
- **Dependencias**: Se requiere una librería de WebSockets para Unity (ej. `Socket.IO Unity` o nativa).
