## ADDED Requirements

### Requirement: Local Identity Binding
The `NetworkManager` SHALL store the local player's identity (`localPlayerId`) and name (`localPlayerName`) upon joining or creating a room.

#### Scenario: Identity assignment on Join
- **WHEN** a player successfully joins a room through the Lobby
- **THEN** the `NetworkManager` MUST set `localPlayerId` and `localPlayerName` to the values used in the request.

### Requirement: Identity-Aware Death Reporting
When a player dies, the `DEATH` event emitted to the server MUST include the player's unique identity.

#### Scenario: Reporting local death
- **WHEN** the local player dies in an Online match
- **THEN** the system MUST emit a `DEATH` event containing the `localPlayerId`.
