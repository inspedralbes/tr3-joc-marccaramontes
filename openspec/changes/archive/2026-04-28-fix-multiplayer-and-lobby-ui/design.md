## Context

El sistema multijugador actual utiliza una arquitectura híbrida: REST para la gestión de salas y WebSockets para la sincronización en tiempo real. Se han identificado discrepancias en el protocolo de red (nombres de eventos) y omisiones en la configuración de la escena de juego que impiden la visibilidad de los rivales y la sincronización de enemigos.

## Goals / Non-Goals

**Goals:**
- Sincronizar la lista de jugadores en el Lobby.
- Estandarizar el protocolo de eventos de socket (Mayúsculas).
- Garantizar la presencia del `RemotePlayerManager` en la escena de juego.
- Habilitar la retransmisión de eventos de combate y spawneo en el servidor.

**Non-Goals:**
- Implementar un sistema de chat.
- Añadir matchmaking automático (basado en salas por código).
- Mejorar el netcode de interpolación (se mantiene el lerp actual).

## Decisions

### 1. Estandarización del Protocolo (Case-Sensitivity)
- **Decisión**: Cambiar todos los eventos de red a MAYÚSCULAS (snake_case no, solo CAPS).
- **Razón**: El servidor Node.js es sensible a mayúsculas y actualmente hay una mezcla (p.ej. `spawn_enemy` vs `SPAWN_ENEMY`). Esto causa que el servidor ignore paquetes críticos.
- **Eventos**: `MOVE`, `SHOOT`, `SPAWN_ENEMY`, `ENEMY_SYNC`, `DEATH`, `START_MATCH`, `PLAYER_JOINED`.

### 2. Sincronización de Jugadores en Lobby
- **Decisión**: Modificar el evento `ROOM_JOINED_CONFIRMED` para que el servidor envíe el estado completo de la sala (lista de nombres).
- **Razón**: Permite al cliente que se une conocer a los jugadores que ya estaban allí, además de recibir notificaciones de nuevos ingresos.

### 3. Inyección de RemotePlayerManager
- **Decisión**: Usar el `SceneSetupHelper` o instanciación dinámica en el `NetworkManager` para asegurar que el `RemotePlayerManager` exista en la escena de juego.
- **Razón**: Es el componente que escucha los eventos `PLAYER_JOINED` y crea los fantasmas. Su ausencia es el motivo por el cual los rivales son invisibles.

## Risks / Trade-offs

- **[Risk]** Sobrecarga de mensajes en el Lobby → **Mitigation**: La lista de jugadores solo se envía al entrar o cuando alguien cambia (máximo 2-4 jugadores por sala).
- **[Risk]** Conflictos de IDs de enemigos → **Mitigation**: El Host sigue siendo la única autoridad que genera IDs (GUIDs) para los enemigos.

## Diagrama de Flujo del Lobby

```ascii
CLIENTE (Lobby)               SERVIDOR (GameService)
      │                               │
      │─── JOIN_ROOM ────────────────▶│
      │                               │ (Registra socket, busca sala)
      │◀── ROOM_JOINED_CONFIRMED ─────│ (Incluye: [Player1, Player2], isHost)
      │       (con lista de nombres)  │
      │                               │
      │◀── PLAYER_JOINED (Broadcast) ─│ (Notifica a otros si entran más)
      ▼                               ▼
(Actualiza UI Lista)
```
