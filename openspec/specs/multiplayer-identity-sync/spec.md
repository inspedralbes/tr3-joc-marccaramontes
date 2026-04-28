## ADDED Requirements

### Requirement: Unified Player Identity
The system SHALL assign and track a unique `playerId` for every connected socket session.

#### Scenario: ID assignment on join
- **WHEN** a player successfully joins a room
- **THEN** the server MUST assign a unique identifier and include it in all subsequent broadcasts originating from that player

### Requirement: Local Identity Binding
The `NetworkManager` SHALL store the local player's identity (`localPlayerId`) and name (`localPlayerName`) upon joining or creating a room.

#### Scenario: Identity assignment on Join
- **WHEN** a player successfully joins a room through the Lobby
- **THEN** the `NetworkManager` MUST set `localPlayerId` and `localPlayerName` to the values used in the request.

### Requirement: Identity-Aware Broadcasts
The server MUST inject the sender's `playerId` into the payload of all game-state broadcasts (MOVE, SHOOT, DEATH).

#### Scenario: Server-side ID injection
- **WHEN** the server receives a `MOVE` event from Client A
- **THEN** it MUST add Client A's unique ID to the payload before broadcasting it to Client B

### Requirement: Identity-Aware Death Reporting
When a player dies, the `DEATH` event emitted to the server MUST include the player's unique identity.

#### Scenario: Reporting local death
- **WHEN** the local player dies in an Online match
- **THEN** the system MUST emit a `DEATH` event containing the `localPlayerId`.
