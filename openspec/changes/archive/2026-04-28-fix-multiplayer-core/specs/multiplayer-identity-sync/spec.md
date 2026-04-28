## ADDED Requirements

### Requirement: Unified Player Identity
The system SHALL assign and track a unique `playerId` for every connected socket session.

#### Scenario: ID assignment on join
- **WHEN** a player successfully joins a room
- **THEN** the server MUST assign a unique identifier and include it in all subsequent broadcasts originating from that player

### Requirement: Identity-Aware Broadcasts
The server MUST inject the sender's `playerId` into the payload of all game-state broadcasts (MOVE, SHOOT, DEATH).

#### Scenario: Server-side ID injection
- **WHEN** the server receives a `MOVE` event from Client A
- **THEN** it MUST add Client A's unique ID to the payload before broadcasting it to Client B
