## Context

The multiplayer system is architected as a set of microservices (Gateway, API, Game) using Raw WebSockets for real-time traffic. Currently, the `Game Service` fails to parse Unity's nested JSON payloads, and the lack of player identification in broadcasted messages prevents remote clients from knowing which entity to update.

## Goals / Non-Goals

**Goals:**
- Fix JSON parsing in `GameService.js` to handle `payload` as a string.
- Align event naming conventions between Unity and Node.js.
- Implement server-side `playerId` injection into broadcasted packets.
- Update `RemotePlayerManager` to correctly process identity-aware messages.

**Non-Goals:**
- Server-side physics validation or authoritative movement.
- Advanced lag compensation (rollback/prediction).
- Refactoring the entire microservice architecture.

## Decisions

### 1. Server-Side Identity Injection
**Decision**: The `Game Service` will inject the sender's unique ID into the broadcasted payload.
**Rationale**: Clients shouldn't have to "know" or send their own ID for every movement packet. The server knows who sent the message via the socket instance.
**Alternative**: Client sends its own ID. Rejected because it increases packet size and is less secure/reliable.

### 2. Standardized Event Naming
**Decision**: Use uppercase `MOVE`, `SHOOT`, `DEATH`, `PLAYER_JOINED`, `PLAYER_LEFT` throughout the system.
**Rationale**: Consistency reduces "magic string" bugs. Unity's `NetworkManager` already expects these types, so we align the server and movement scripts to them.

### 3. Nested JSON Parsing Strategy
**Decision**: Unity sends `{"type": "...", "payload": "{...}"}`. The server will `JSON.parse` the outer message, then `JSON.parse` the `payload` only for routing logic (like `JOIN_ROOM`). For broadcasts, it will treat `payload` as a string and wrap it with the ID.

#### Packet Flow Diagram
```
Unity (Client A)          Game Service (Server)        Unity (Client B)
       |                            |                         |
       | -- MOVE {"x":1, "y":2} --> |                         |
       |                            | -- MOVE {               |
       |                            |      "playerId": "A",   |
       |                            |      "payload": {...}   |
       |                            |    } -----------------> |
       |                            |                         |
```

## Risks / Trade-offs

- **[Risk]** Data Overhead → **Mitigation**: Using string payloads and simple ID injection keeps overhead minimal for a 1vs1 prototype.
- **[Risk]** Ghost Instantiation Latency → **Mitigation**: `RemotePlayerManager` will use a "Lazy Spawn" strategy: if a `MOVE` is received for an unknown ID, it spawns the ghost immediately.
