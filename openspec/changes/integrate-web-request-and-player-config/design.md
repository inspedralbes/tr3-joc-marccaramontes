## Context

The current architecture relies exclusively on Socket.io for all network interactions. While effective for real-time gameplay, it doesn't utilize standard HTTP requests as required by the project specifications. The project also lacks a way to capture player identity (even a simple name) before entering a match.

## Goals / Non-Goals

**Goals:**
- Implement a hybrid communication model: REST (HTTP) for management and WebSockets for gameplay.
- Capture and synchronize a player's display name.
- Report match results to a persistent (or at least external) endpoint.

**Non-Goals:**
- Implementing a persistent database (we will use in-memory storage on the server for this prototype).
- Implementing user authentication, passwords, or persistent accounts.
- Changing the core real-time synchronization logic (movement, shooting).

## Decisions

### 1. Hybrid Network Architecture
- **Decision**: Use `UnityWebRequest` for "Room Lifecycle" (Create/Join) and "Finalization" (Results). Use `Socket.io` only for "Active Gameplay" (Sync).
- **Rationale**: Meets the requirement of using both technologies for their appropriate use cases (HTTP for transactional, WebSockets for streaming).
- **Alternative**: Keeping everything in WebSockets (Rejected: Doesn't meet requirements).

### 2. Player Name as a Pre-requisite
- **Decision**: The "Create" and "Join" buttons will be disabled until a name is entered.
- **Rationale**: Ensures we always have a name to associate with the player in the match and the final results.

### 3. Server-Side Storage
- **Decision**: The `rooms` object in `index.js` will now store `playerName` along with the `socketId`.
- **Rationale**: Allows the server to know who is who without a complex database.

## UI Flow & State Machine

```ascii
      ┌──────────┐          ┌───────────────┐          ┌─────────────┐
      │  START   │          │    LOBBY      │          │   MATCHING  │
      └────┬─────┘          └───────┬───────┘          └──────┬──────┘
           │                        │                         │
     [Scene: Menu]            [Scene: Lobby]            [REST: /create]
           │                        │                         │
           ▼                        ▼                         ▼
      ┌──────────┐          ┌───────────────┐          ┌─────────────┐
      │  CLIC    ├─────────▶│ INPUT NAME    ├─────────▶│ CONNECT WS  │
      │  ONLINE  │          │ SELECT MODE   │          │ JOIN ROOM   │
      └──────────┘          └───────────────┘          └──────┬──────┘
                                                              │
                                                        [Scene: Game]
                                                              │
                                                              ▼
      ┌──────────┐          ┌───────────────┐          ┌─────────────┐
      │ RESULTS  │◀─────────┤   GAME OVER   │◀─────────┤   PLAYING   │
      │ /results │          │               │          │ (Websocket) │
      └──────────┘          └───────────────┘          └─────────────┘
```

## Risks / Trade-offs

- **[Risk]** → Connection Desync: If the HTTP request succeeds but the WebSocket fails, the room might have a "ghost" player.
  - **Mitigation** → Server-side timeout for rooms: if a player joins via REST but doesn't connect via WS within 10 seconds, remove them.
- **[Risk]** → Port Conflict: Both HTTP and WS will share the same port (3000) via Express.
  - **Mitigation** → This is standard for Socket.io and Express integration; ensure CORS is correctly configured.

## Prefab Hierarchy Changes

- **Lobby UI**:
    - `LobbyManager` (GameObject)
        - `MainPanel`
            - `NameInputField` (TMP_InputField) **[NEW]**
            - `RoomInputField`
            - `CreateButton`
            - `JoinButton`
        - `WaitingPanel`
            - `RoomCodeText`
            - `StartMatchButton` (Host only)
