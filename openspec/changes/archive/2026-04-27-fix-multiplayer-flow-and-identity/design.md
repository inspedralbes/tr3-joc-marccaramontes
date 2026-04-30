## Context

The current multiplayer implementation suffers from authority conflicts. `PlayerMovement` assumes it is a remote player if any `NetworkManager` exists, even if it is the local player's instance. Furthermore, the 1vs1 survival mode lacks the intermediate state where one player is dead but the match continues, leading to a "frozen" feeling for the first player to lose.

## Goals / Non-Goals

**Goals:**
- Ensure local player authority is correctly assigned in both Solo and Online modes.
- Sync `localPlayerId` with the name used in the Lobby/Server.
- Implement a "Waiting for Rival" state in `GameManager`.
- Stop the local survival timer immediately upon local death.

**Non-Goals:**
- Implementing a full spectator camera (just keeping the current view is enough for now).
- Changing the server-side broadcast logic.

## Decisions

### 1. Reliable Authority Assignment
**Decision**: In `PlayerMovement.Awake`, the script will assume `isLocalPlayer = true` by default. The `RemotePlayerManager` will be responsible for setting `isLocalPlayer = false` on the ghosts it instantiates.
**Rationale**: It is easier to "opt-out" of authority for specific remote instances than to "opt-in" through complex manager checks that might be uninitialized.

### 2. Identity Binding
**Decision**: `NetworkManager` will treat `localPlayerName` and `localPlayerId` as identical for this prototype phase. `LobbyController` will ensure both are set before connecting to the socket.
**Rationale**: The Node.js server currently uses the socket's `currentPlayerName` as the `playerId` in broadcasts. Aligning the client's internal ID with this name prevents "Unknown Player" bugs.

### 3. Survival Match Flow (State Machine)
**Decision**: Introduce a `matchEndedForLocal` flag in `GameManager`.
**Rationale**: `currentState = GameState.Playing` should remain true as long as the match is globally active, but we need a way to stop local updates (timer, input) for the player who died.

**State Machine Diagram:**
```ascii
[Playing] 
    |
    |-- (Local Dies) --> [Local Dead / Waiting] (Timer Stops, Input Disabled, HUD: "Waiting...")
    |                          |
    |-- (Rival Dies) ----------|-- (Rival Dies) --> [GameOver Transition]
```

## Risks / Trade-offs

- **[Risk] Ghost authority leakage** → **Mitigation**: `RemotePlayerManager` must set `isLocalPlayer = false` and `hasAuthority = false` immediately after instantiation.
- **[Risk] Timer Desync** → **Mitigation**: The local timer is only for UI; the server's reported survival time is the "Source of Truth" for results.
